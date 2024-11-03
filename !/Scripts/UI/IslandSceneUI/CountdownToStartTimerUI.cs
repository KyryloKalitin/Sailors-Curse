using System.Collections;
using TMPro;
using UnityEngine;
using Zenject;

public class CountdownToStartTimerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private GameTimerService _gameTimerService;

    private bool _isCountdownOver = false;

    [Inject]
    public void Construct(GameTimerService gameTimerService)
    {
        _gameTimerService = gameTimerService;

        _gameTimerService.OnCountdownStarted += _gameTimerService_OnCountdownStarted;
        _gameTimerService.OnGameplayStarted += _gameTimerService_OnGameStarted;
    }

    private void Start()
    {
        Hide();
    }

    private void Update()
    {
        if (!_isCountdownOver)
            _text.text = _gameTimerService.CountdownToStartTime.ToString();
        else
        {
            _text.color = Color.green;
            _text.text = "Start!";
        }
    }

    private void _gameTimerService_OnGameStarted()
    {
        _isCountdownOver = true;

        StartCoroutine(DelayToStartCoroutine());
    }

    private IEnumerator DelayToStartCoroutine()
    {
        yield return new WaitForSeconds(1f);
        
        Hide();
    }

    private void _gameTimerService_OnCountdownStarted()
    {
        Show();
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
        _gameTimerService.OnCountdownStarted -= _gameTimerService_OnCountdownStarted;
        _gameTimerService.OnGameplayStarted -= _gameTimerService_OnGameStarted;
    }
}
