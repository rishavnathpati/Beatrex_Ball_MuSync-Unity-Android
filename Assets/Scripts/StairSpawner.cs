using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairSpawner : MonoBehaviour
{

    public GameObject stairPrefab;
    int index = -1;

    [SerializeField]
    float stairWidth = 2f, stairHeight = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        spawnStairs();
    }

    void spawnStairs()
    {
        Vector2 newPosition = new Vector2(0, index * 5);

        GameObject stair = Instantiate(stairPrefab, newPosition,Quaternion.identity);
        stair.transform.SetParent(transform);
        stair.transform.localScale = new Vector2(stairWidth, stairHeight);
        index++;
    }
}
