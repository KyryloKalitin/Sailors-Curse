using TMPro;
using UnityEngine;
using Zenject;

public class GamePlayTimerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private GameTimerService _gameTimerService;

    [Inject]
    public void Construct(GameTimerService gameTimerService)
    {
        _gameTimerService = gameTimerService;

        _gameTimerService.OnGameplayStarted += Show;
        _gameTimerService.OnGameplayEnded += Hide;
    }

    private void Start()
    {
        Hide();
    }

    private void Update()
    {
        if (_gameTimerService.GamePlayTime <= 10)
            _text.color = Color.red;

        _text.text = _gameTimerService.GamePlayTime.ToString();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        _gameTimerService.OnGameplayStarted -= Show;
        _gameTimerService.OnGameplayEnded -= Hide;
    }
}