using UnityEngine;

public class StairSpawner : MonoBehaviour
{

    public GameObject stairPrefab;
    int index = 0;

    [SerializeField]
    float stairWidth = 2f, stairHeight = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 35; i++)
            spawnStairs();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void spawnStairs()
    {
        makeStair();
    }

    void makeStair()
    {
        Vector2 newPosition = new Vector2(Random.Range(-4.6f,4.6f), index * 5f);

        GameObject stair = Instantiate(stairPrefab, newPosition, Quaternion.identity);
        stair.transform.SetParent(transform);
        stair.transform.localScale = new Vector2(stairWidth, stairHeight);
        index++;
    }
}
