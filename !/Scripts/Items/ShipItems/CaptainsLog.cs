using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CaptainsLog : UntakeableItem
{
    [SerializeField] private GameObject _UIWindow;
    [SerializeField] private Button _button;

    private InputService _inputService;

    private void Start()
    {

        _button.onClick.AddListener(Hide);

        Hide();
    }


    [Inject]
    public void Construct(InputService inputService)
    {
        _inputService = inputService;
    }    

    public override void Interact()
    {
        Show();
    }

    public void Show()
    {
        _UIWindow.SetActive(true);

        _inputService.SetCursorEnabled(true);
        _inputService.SetLookingEnabled(false);
        _inputService.SetMovementEnabled(false);
    }

    public void Hide()
    {
        _UIWindow.SetActive(false);

        _inputService.SetCursorEnabled(false);
        _inputService.SetLookingEnabled(true);
        _inputService.SetMovementEnabled(true);
    }
}
