using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Visualizer : MonoBehaviour
{
    public RectTransform[] rectTransforms;
    Color visualiserColor = Color.gray;


    // Start is called before the first frame update
    void Start()
    {
        rectTransforms = GetComponentsInChildren<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        for(int i=0;i<rectTransforms.Length;i++)
        {
            //rectTransforms[i].GetComponent<Image>().color=visualiserColor;
        }
    }
}
