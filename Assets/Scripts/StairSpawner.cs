using UnityEngine;

public class StairSpawner : MonoBehaviour
{
    public GameObject stairPrefab, Orb;
    public int index = 0;
    

    //[SerializeField]
    float stairWidth = 3f;
    float stairHeight = 0.6f;
    float hue;
    public float stairGap;

    Vector2 newPosition;
    public static StairSpawner stairSpawner = null;
    

    // Start is called before the first frame update
    void Start()
    {
        if (stairSpawner == null)
            stairSpawner = this;
        stairGap = 5f;

        for (int i = 0; i < 5; i++)
            spawnStairs();
        InitColour();
    }

    void spawnStairs()
    {
        InvokeRepeating("makeStair", 0, 0.5f);
    }

    public void makeStair()
    {
        if (index == 0)
            newPosition = new Vector2(0, index * stairGap);
        else
            newPosition = new Vector2(UnityEngine.Random.Range(-4.6f, 4.6f), index * 5f);

        GameObject stair = Instantiate(stairPrefab, newPosition, Quaternion.identity);
        stair.transform.SetParent(transform);
        stair.transform.localScale = new Vector2(stairWidth, stairHeight);
        index++;
        if (UnityEngine.Random.Range(0, 5) < 1)
            makeOrb(index);
    }

    public void makeOrb(int index)
    {
        GameObject orb = Instantiate(Orb, new Vector2(UnityEngine.Random.Range(-3f, 3f), index * 3.5f), Quaternion.identity);

    }

    public void InitColour()
    {
        hue = UnityEngine.Random.Range(0f, 1f);
        Camera.main.backgroundColor = Color.HSVToRGB(hue, 0.9f, 0.9f);
    }
}
