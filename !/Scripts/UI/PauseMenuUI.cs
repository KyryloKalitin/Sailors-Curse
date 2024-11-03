using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject _UIWindow;

    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _mainMenuButton;

    [SerializeField] private GameObject _itemMenusContainer;

    private SceneLoadService _sceneLoadService;
    private InputService _inputService;

    [Inject]
    public void Construct(SceneLoadService sceneLoadService, InputService inputService)
    {
        _sceneLoadService = sceneLoadService;
        _inputService = inputService;
    }

    private void Start()
    {
        Hide();

        _continueButton.onClick.AddListener(ContinueButtton_OnClick);
        _mainMenuButton.onClick.AddListener(MainMenuButton_OnClick);

        _inputService.playerInput.Exit.Exit.performed += Exit_performed;
    }

    private void OnDestroy()
    {
        _continueButton.onClick.RemoveListener(ContinueButtton_OnClick);
        _mainMenuButton.onClick.RemoveListener(MainMenuButton_OnClick);

        _inputService.playerInput.Exit.Exit.performed -= Exit_performed;
    }

    private void Exit_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (_UIWindow.activeSelf)
        {
            Hide();
            return;
        }

        foreach (Transform child in _itemMenusContainer.transform)
        {
            if (child.gameObject.activeSelf)
                return;
        }

        Show();
    }

    private void Show()
    {
        _UIWindow.SetActive(true);

        _inputService.SetCursorMode(CursorMode.Menu);
        _inputService.EnableInteractions(false);
        _inputService.EnableCameraLook(false);
        _inputService.EnableMovement(false);
    }

    private void Hide()
    {
        _UIWindow.SetActive(false);

        _inputService.SetCursorMode(CursorMode.Game);
        _inputService.EnableInteractions(true);
        _inputService.EnableCameraLook(true);
        _inputService.EnableMovement(true);
    }

    private void ContinueButtton_OnClick()
    {
        Hide();
    }

    private void MainMenuButton_OnClick()
    {
        _sceneLoadService.Load(SceneLoadService.Scene.Menu);
    }
}