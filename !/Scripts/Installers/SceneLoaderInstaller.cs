using Zenject;

public class SceneLoaderInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        SceneLoaderBind();
    }

    private void SceneLoaderBind()
    {
        var sceneLoader = Container.InstantiateComponentOnNewGameObject<SceneLoadService>("SceneLoader");

        Container
            .Bind<SceneLoadService>()
            .FromInstance(sceneLoader)
            .AsSingle()
            .NonLazy();
    }
}
