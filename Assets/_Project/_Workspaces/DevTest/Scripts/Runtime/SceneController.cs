using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void ReloadScene(float delay)
    {
        LoadSceneByIndex(SceneManager.GetActiveScene().buildIndex, delay);
    }

    public void LoadNextScene(float delay)
    {
        LoadSceneByIndex(SceneManager.GetActiveScene().buildIndex + 1, delay);
    }

    public void LoadPreviousScene(float delay)
    {
        LoadSceneByIndex(SceneManager.GetActiveScene().buildIndex - 1, delay);
    }

    public IEnumerator LoadSceneByIndex(int sceneIndex, float delay)
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