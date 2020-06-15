using UnityEngine;

public class StairSpawner : MonoBehaviour
{
    public GameObject stairPrefab;
    public GameObject Orb;
    public GameObject Vortex;
    public GameObject spikeyStairL;
    public GameObject spikeyStairR;
    private int index = 0;
    private int score;
    private float stairGap;
    private float hue;
    private bool spikeyIsRecent;
    private Vector2 newPosition;
    private GameObject stair;
    public static StairSpawner instance = null;

    private void Start()
    {
        spikeyIsRecent = false;
        if (instance == null)
        {
            instance = this;
        }

        stairGap = 6f;
        for (int i = 0; i <= 5; i++)
        {
            MakeStair();
        }

        SpawnStairs();
        InitColour(0.9f);
    }

    private void SpawnStairs()
    {
        InvokeRepeating("MakeStair", 0, 0.3f);
    }

    public void GetScore(int s)
    {
        score = s;
    }


    public void MakeStair()
    {
        if (index == 0)
        {
            newPosition = new Vector2(0, 5);
        }
        else
        {
            newPosition = new Vector2(UnityEngine.Random.Range(-4.6f, 4.6f), index * stairGap);
        }

        if (UnityEngine.Random.Range(1, 10) <= 2 && index > 30 && spikeyIsRecent == false)
        {
            if (newPosition.x > 0)
            {
                stair = Instantiate(spikeyStairR, newPosition, Quaternion.identity);
            }
            else
            {
                stair = Instantiate(spikeyStairL, newPosition, Quaternion.identity);
            }

            spikeyIsRecent = true;
        }

        else
        {
            stair = Instantiate(stairPrefab, newPosition, Quaternion.identity);
            spikeyIsRecent = false;
        }

        stair.transform.SetParent(transform);
        stair.GetComponent<SpriteRenderer>().color = Color.HSVToRGB(Random.Range(0f, 0.9f), 1f, 0.15f);

        if (UnityEngine.Random.Range(0, 15) == 1 && score > 2)
        {
            MakeOrb(index);
        }

        index++;
    }

    public void SpecialStairs(float pos)
    {
        newPosition = new Vector2(UnityEngine.Random.Range(-4.6f, 4.6f), pos);
        stair = Instantiate(stairPrefab, newPosition, Quaternion.identity);
        stair.transform.SetParent(transform);
    }


    public void MakeOrb(int index)
    {
        Instantiate(Orb, new Vector2(UnityEngine.Random.Range(-3f, 3f), index * 3.5f), Quaternion.identity);
        if (index % 120 == 0)
        {
            Instantiate(Vortex, new Vector2(UnityEngine.Random.Range(-4f, 4f), index * 3.5f), Quaternion.identity);
        }
    }

    public void InitColour(float value)
    {
        hue = UnityEngine.Random.Range(0f, 1f);
        Camera.main.backgroundColor = Color.HSVToRGB(hue, value, value);
        Visualizer.instance.GetColourForBar(Color.HSVToRGB(1 - hue, value, value));
    }
}