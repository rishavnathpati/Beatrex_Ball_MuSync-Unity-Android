using System;
using System.Collections.Generic;
using UnityEngine;

public class StairSpawner : MonoBehaviour
{

    public GameObject stairPrefab;
    int index = 0;

    [SerializeField]
    float stairWidth = 2f, stairHeight = 0.5f;

    Vector2 newPosition;

    public static StairSpawner stairSpawner=null;
    List<GameObject> stairList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        if (stairSpawner == null)
            stairSpawner = this;

        for (int i = 0; i < 5; i++)
            spawnStairs();
    }

    void spawnStairs()
    {
        makeStair();
    }

    public void makeStair()
    {
        if(index==0)
             newPosition = new Vector2(0, index * 5f);
        else
         newPosition = new Vector2(UnityEngine.Random.Range(-4.6f,4.6f), index * 5f);

        GameObject stair = Instantiate(stairPrefab, newPosition, Quaternion.identity);
        stair.transform.SetParent(transform);
        stair.transform.localScale = new Vector2(stairWidth, stairHeight);
        index++;
    }
}
