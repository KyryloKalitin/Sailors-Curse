using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button _newGameButton;
    [SerializeField] private Button _continueGameButton;
    [SerializeField] private Button _exitGameButton;

    private InputService _inputService;
    private SceneLoadService _sceneLoader;

    [Inject]
    public void Construct(SceneLoadService sceneLoader)
    {
        _sceneLoader = sceneLoader;
    }

    private void Start()
    {
        _inputService = new();
        _inputService.SetCursorMode(CursorMode.Menu);

        _newGameButton.onClick.AddListener(NewGame);
        _continueGameButton.onClick.AddListener(ContinueGame);
        _exitGameButton.onClick.AddListener(ExitGame);

        if (!GameProgressDataIO.HasSavedGameProgress())
            _continueGameButton.interactable = false;
        else
            _continueGameButton.interactable = true;
    }

    private void NewGame()
    {
        GameProgressDataIO.DeleteGameProgressData();

        _sceneLoader.Load(SceneLoadService.Scene.Ship);
    }

    private void ContinueGame()
    {
        _sceneLoader.Load(SceneLoadService.Scene.Ship);
    }

    private void ExitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    private void OnDestroy()
    {
        _newGameButton.onClick.RemoveListener(NewGame);
        _continueGameButton.onClick.RemoveListener(ContinueGame);
        _exitGameButton.onClick.RemoveListener(ExitGame);
    }
}
