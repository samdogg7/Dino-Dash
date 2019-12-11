using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    public GameObject LoadingCanvas;
    public Slider slider;
    public DinoAnimator dinoAnimator;
    public bool loadMainMenu = false;
    public float loadingWaitTime = 1f;
    private bool loading = false;
    private float elapsedTime = 0f;
    private AsyncOperation operation;

    private void Start()
    {
        if(loadMainMenu)
        {
            LoadlLevel(1);
        } else
        {
            LoadingCanvas.SetActive(false);
        }
    }

    private void Update()
    {
        if(loading)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            elapsedTime += Time.deltaTime;
            if(elapsedTime >= loadingWaitTime)
            {
                slider.value = progress;
                operation.allowSceneActivation = true;
            } else
            {
                if (operation.progress < .9f)
                {
                    slider.value = progress;
                } else
                {
                    slider.value = .9f;
                }
            }
        }
    }

    public void LoadlLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsync(sceneIndex));
    }

    private IEnumerator LoadAsync(int sceneIndex)
    {
        operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;
        LoadingCanvas.SetActive(true);
        dinoAnimator.RunningAnimation(Settings.instance.GetRunningSprites());
        loading = true;
        yield return null;
    }
}
