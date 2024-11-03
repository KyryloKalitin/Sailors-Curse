using System;
using Zenject;

public class Door : SelectableItem, IUntakeableItem
{
    private SceneLoadService _sceneLoader;

    [Inject]
    public void Construct(SceneLoadService sceneLoadService)
    {
        _sceneLoader = sceneLoadService;
    }

    public void Interact()
    {
        _sceneLoader.Load(SceneLoadService.Scene.Island);
    }    
}
