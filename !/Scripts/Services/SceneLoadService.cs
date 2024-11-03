using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadService : MonoBehaviour
{
    public event Action OnSceneLoadingStarted;

    private string _menuSceneName = "MainMenu";
    private string _islandSceneName = "IslandScene";
    private string _shipSceneName = "ShipScene";

    private string _losingSceneName = "LosingScene";

    public enum Scene
    {
        Menu,
        Island,
        Ship,
        Losing
    }

    public void Load(Scene scene, bool immediately = false)
    {
        StartCoroutine(SceneLoadCoroutine(scene, immediately));
    }

    private IEnumerator SceneLoadCoroutine(Scene scene, bool immediately)
    {
        string sceneName = scene switch
        {
            Scene.Menu => _menuSceneName,
            Scene.Island => _islandSceneName,
            Scene.Ship => _shipSceneName,
            Scene.Losing => _losingSceneName,
            _ => _menuSceneName
        };

        if(immediately == false)
        {
            OnSceneLoadingStarted?.Invoke();

            AsyncOperation asyncSceneLoad = SceneManager.LoadSceneAsync(sceneName);
            asyncSceneLoad.allowSceneActivation = false;

            while(asyncSceneLoad.progress < 0.9f)
            {
                yield return null;
            }

            yield return new WaitForSeconds(2f);

            asyncSceneLoad.allowSceneActivation = true;
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }
} 
