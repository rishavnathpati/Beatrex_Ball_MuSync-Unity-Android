using UnityEngine;
using UnityEngine.UI;

public class Visualizer : MonoBehaviour
{
    public VisualizerObjectScript[] visualizerObjects;
    public static Visualizer instance;

    readonly float minHeight = 25.0f;
    readonly float maxHeight = 625.0f;
    readonly float updateSensitivity = 1.0f;
    readonly int visualizerSimples = 64;

    AudioSource audioSource;
    Color visualizerColor;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        visualizerObjects = GetComponentsInChildren<VisualizerObjectScript>();
    }

    public void GetAudioSource(AudioSource source)
    {
        audioSource = source;
    }

    public void GetColourForBar(Color color)
    {
        color.a = 0.5f;
        visualizerColor = color;
    }

    [System.Obsolete]
    void FixedUpdate()
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
