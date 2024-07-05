using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void ReloadScene(float delay)
    {
        StartCoroutine(Co_LoadSceneByIndex(SceneManager.GetActiveScene().buildIndex, delay));
    }

    public void LoadNextScene(float delay)
    {
        StartCoroutine(Co_LoadSceneByIndex(SceneManager.GetActiveScene().buildIndex + 1, delay));
    }

    public void LoadPreviousScene(float delay)
    {
        StartCoroutine(Co_LoadSceneByIndex(SceneManager.GetActiveScene().buildIndex - 1, delay));
    }

    public void LoadMenu(float delay)
    {
        StartCoroutine(Co_LoadSceneByIndex(0, delay));
    }

    private IEnumerator Co_LoadSceneByIndex(int sceneIndex, float delay)
    {
        yield return new WaitForSeconds(delay);
        
        if (sceneIndex >= 0 && sceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(sceneIndex);
        }
        else
        {
            Debug.LogWarning("Scene index is out of range. Make sure there are enough scenes in the build settings.");
        }
    }
}