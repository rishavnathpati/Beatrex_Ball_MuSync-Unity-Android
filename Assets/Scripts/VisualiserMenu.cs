﻿using UnityEngine;
using UnityEngine.UI;

public class VisualiserMenu : MonoBehaviour
{
    public VisualizerObjectScript[] visualizerObjects;


    float minHeight = 25.0f;
    float maxHeight = 625.0f;
    float updateSensitivity = 1.0f;
    int visualizerSimples = 64;
    public AudioSource audioSource;
    public static VisualiserMenu instance;
    public Color visualizerColor;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        visualizerObjects = GetComponentsInChildren<VisualizerObjectScript>();
    }

    [System.Obsolete]
    void Update()
    {
        float[] spectrumData = audioSource.GetSpectrumData(visualizerSimples, 0, FFTWindow.Rectangular);

        for (int i = 0; i < visualizerObjects.Length; i++)
        {
            Vector2 newSize = visualizerObjects[i].GetComponent<RectTransform>().rect.size;
            newSize.y = Mathf.Clamp(Mathf.Lerp(newSize.y, minHeight + (spectrumData[i] * (maxHeight - minHeight) * 5.0f), updateSensitivity * 0.5f), minHeight, maxHeight);
            visualizerObjects[i].GetComponent<RectTransform>().sizeDelta = newSize;
            visualizerObjects[i].GetComponent<Image>().color = visualizerColor;
        }
    }
}