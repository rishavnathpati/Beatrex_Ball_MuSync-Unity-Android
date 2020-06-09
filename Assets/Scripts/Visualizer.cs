using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class Visualizer : MonoBehaviour
{
    public VisualizerObjectScript[] visualizerObjects;
    

    float minHeight = 25.0f;
    float maxHeight = 625.0f;
    float updateSensitivity = 1.0f;
    int visualizerSimples = 64;
    AudioSource audioSource;
    public static Visualizer instance;
    Color visualizerColor;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    // Start is called before the first frame update
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
        color.a = 1f;
        visualizerColor = color;
    }

    // Update is called once per frame
    [System.Obsolete]
    void FixedUpdate()
    {
        float[] spectrumData = audioSource.GetSpectrumData(visualizerSimples, 0, FFTWindow.Rectangular);

        for (int i = 0; i < visualizerObjects.Length; i++)
        {
            Vector2 newSize = visualizerObjects[i].GetComponent<RectTransform>().rect.size;

            newSize.y = Mathf.Clamp(Mathf.Lerp(newSize.y, minHeight + (spectrumData[i] * (maxHeight - minHeight) * 5.0f), updateSensitivity * 0.5f), minHeight, maxHeight);
            //newSize.y = Mathf.Lerp(newSize.y, minHeight + (spectrumData[i] * (maxHeight - minHeight) * 5.0f), updateSensitivity * 0.5f);
            visualizerObjects[i].GetComponent<RectTransform>().sizeDelta = newSize;
            visualizerObjects[i].GetComponent<Image>().color = visualizerColor;
        }
    }
}
