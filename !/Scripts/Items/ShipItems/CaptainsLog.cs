using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CaptainsLog : SelectableItem, IUntakeableItem
{
    [SerializeField] private GameObject _UIWindow;
    [SerializeField] private Button _button;

    private bool _isAnimating = false;

    private InputService _inputService;

    private void Start()
    {
        _button.onClick.AddListener(Hide);
        _inputService.playerInput.Exit.Exit.performed += Exit_performed;

        Hide();
    }

    [Inject]
    public void Construct(InputService inputService)
    {
        _inputService = inputService;
    }    

    public void Interact()
    {
        Show();
    }

    public void Show()
    {
        if (_isAnimating)
            return;

        _isAnimating = true;

        _UIWindow.SetActive(true);

        var defaultScale = _UIWindow.transform.localScale;
        _UIWindow.transform.localScale = Vector3.zero;

        _UIWindow.transform.DOScale(defaultScale, 0.2f).OnComplete(() => _isAnimating = false);

        _inputService.SetCursorMode(CursorMode.Menu);
        _inputService.EnableInteractions(false);
        _inputService.EnableCameraLook(false);
        _inputService.EnableMovement(false);
    }

    public void Hide()
    {
        if (_isAnimating)
            return;

        _isAnimating = true;

        var defaultScale = _UIWindow.transform.localScale;

        _UIWindow.transform.DOScale(0, 0.2f).OnComplete(() =>
        {
            _isAnimating = false;

            _UIWindow.SetActive(false);
            _UIWindow.transform.localScale = defaultScale;
        });

        _inputService.SetCursorMode(CursorMode.Game);
        _inputService.EnableInteractions(true);
        _inputService.EnableCameraLook(true);
        _inputService.EnableMovement(true);
    }

    private void Exit_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (_UIWindow.activeSelf)
            Hide();
    }

    private void OnDestroy()
    {
        _inputService.playerInput.Exit.Exit.performed -= Exit_performed;
    }
}
