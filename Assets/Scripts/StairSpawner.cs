using UnityEngine;

public class StairSpawner : MonoBehaviour
{
    public GameObject stairPrefab;
    public GameObject Orb;
    public GameObject Vortex;
    public GameObject spikeyStair;
    public int index = 0;


    //[SerializeField]
    float stairWidth = 4f;
    float stairHeight = 0.6f;
    float hue;
    bool spikeyIsRecent = false;
    int score;
    GameObject stair;
    public float stairGap;

    Vector2 newPosition;
    public static StairSpawner instance = null;


    // Start is called before the first frame update
    void Start()
    {
        spikeyIsRecent = false;
        if (instance == null)
            instance = this;
        stairGap = 5f;

        for (int i = 0; i < 5; i++)
            makeStair();
        spawnStairs();
        InitColour(0.9f);
    }

    void spawnStairs()
    {
        InvokeRepeating("makeStair", 0, 0.2f);
    }

    public void SetScore(int s)
    {
        score = s;
    }

    public void SetStairWidth(float w)
    {
        stairWidth -= w;
    }


    public void makeStair()
    {
        if (index == 0)
            newPosition = new Vector2(0, 5);
        else
            newPosition = new Vector2(UnityEngine.Random.Range(-4.6f, 4.6f), index * stairGap);

        if (UnityEngine.Random.Range(1, 10) <= 2 && score > 10 && spikeyIsRecent == false)
        {
            stair = Instantiate(spikeyStair, newPosition, Quaternion.identity);
            spikeyIsRecent = true;
        }

        else
        {
            stair = Instantiate(stairPrefab, newPosition, Quaternion.identity);
            spikeyIsRecent = false; ;
        }


        stair.transform.SetParent(transform);
        stair.transform.localScale = new Vector2(stairWidth, stairHeight);

        if (UnityEngine.Random.Range(0, 8) < 1)
            makeOrb(index);
        index++;
    }

    public void SpecialStairs(float pos)
    {
        newPosition = new Vector2(UnityEngine.Random.Range(-4.6f, 4.6f), pos);
        stair = Instantiate(stairPrefab, newPosition, Quaternion.identity);
        stair.transform.SetParent(transform);
        stair.transform.localScale = new Vector2(stairWidth, stairHeight);
    }


    public void makeOrb(int index)
    {
        Instantiate(Orb, new Vector2(UnityEngine.Random.Range(-3f, 3f), index * 3.5f), Quaternion.identity);
        if (UnityEngine.Random.Range(1, 20) == 1 && score > 10)
            Instantiate(Vortex, new Vector2(UnityEngine.Random.Range(-4f, 4f), index * 3.5f), Quaternion.identity);
    }

    public void InitColour(float value)
    {
        hue = UnityEngine.Random.Range(0f, 1f);
        Camera.main.backgroundColor = Color.HSVToRGB(hue, value, value);
        Visualizer.instance.GetColourForBar(Color.HSVToRGB(1 - hue, value, value));
    }
}