using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    private const float REQUIRED_PROGRESS = 0.9f;

    private enum Scene
    {
        LoadingScreen,
        Computer,
    }

    [SerializeField] private LoadingUI _loadingUI;

    [SerializeField] private Scene _targetScene;

    //public static void Load(Scene targetScene)
    //{
    //    _targetScene = targetScene;

    //    SceneManager.LoadScene(Scene.LoadingScreen.ToString());
    //}

    private void Start()
    {
        StartCoroutine(LoadYourAsyncScene(_targetScene));
    }

    IEnumerator LoadYourAsyncScene(Scene scene)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene.ToString());
        asyncLoad.allowSceneActivation = false;

        // Wait until the loading bar is fully completed
        while (asyncLoad.progress < REQUIRED_PROGRESS || !_loadingUI.IsLoadingBarCompleted())
        {
            _loadingUI.HandleLoadingBar(asyncLoad.progress >= REQUIRED_PROGRESS);
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(0.25f); // Loading pause

        GC.Collect();
        asyncLoad.allowSceneActivation = true; // Load scene
    }
}
