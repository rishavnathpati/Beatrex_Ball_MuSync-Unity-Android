using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public GameObject loadingScreen;
    public TextMeshProUGUI loadingText;
    public Slider slider;
    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsync(sceneIndex));
    }

    IEnumerator LoadAsync(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        loadingScreen.SetActive(true);
        while(!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            //Debug.Log(operation.progress);
            slider.value = progress;
            loadingText.text = "L O A D I N G : " + (Convert.ToInt32(progress) * 100).ToString() + " %";
            yield return null;
        }
        //operation.progress
    }
}
