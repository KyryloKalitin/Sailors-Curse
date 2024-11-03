using UnityEngine;
using Zenject;

public class LoadingScreenController : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    [Inject]
    public void Consturct(SceneLoadService sceneLoader)
    {
        sceneLoader.OnSceneLoadingStarted += SceneLoader_OnSceneLoadingStarted;
    }

    private void Start()
    {
        _animator.SetTrigger("Disappear");
    }

    private void SceneLoader_OnSceneLoadingStarted()
    {
        _animator.SetTrigger("Appear");
    }
}
