using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class IslandGameManager : MonoBehaviour
{
    public event Action OnWaitingStarted;
    public event Action OnGameStarted;

    public event Action OnGameWon;
    public event Action OnGameLost;

    private float _waitingToStartTime = 1f;
    private float _countdownToStartTime = 3f;
    private float _gameTime = 120f;

    private PlayerStatsService _playerStatsService;
    private PlayerInventoryService _playerInventoryService;
    private ShipZoneTriggerHandler _shipZoneTriggerHandler;

    private ShipInventoryService _shipInventoryService;


    private GameState _currentGameState = GameState.WaitingToStart;
    private GameState _lastGameState = GameState.None;

    [Inject]
    public void Construct(ShipInventoryService shipInventoryService, PlayerInventoryService playerInventoryService, PlayerStatsService playerStatsService, ShipZoneTriggerHandler shipZoneTriggerHandler)
    {
        _playerInventoryService = playerInventoryService;
        _shipInventoryService = shipInventoryService;
        _playerStatsService = playerStatsService;
        _shipZoneTriggerHandler = shipZoneTriggerHandler;
    }

    private void Start()
    {
        _playerStatsService.OnPlayerDied += _playerStatsService_OnPlayerDied;
    }

    private void Update()
    {
        UpdateGameState();
    }

    private void UpdateGameState()
    {
        if (_currentGameState == _lastGameState)
            return;

        _lastGameState = _currentGameState;

        switch (_currentGameState)
        {
            case GameState.WaitingToStart:
                StartCoroutine(WaitingToStartCoroutine());
                break;
            case GameState.CountdownToStart:
                StartCoroutine(CountdownToStartCoroutine());
                break;
            case GameState.GamePlaying:
                GamePlaing();
                break;
            case GameState.Winning:
                Winning();
                break;
            case GameState.Losing:
                Losing();
                break;
        }       
    }

    private void GamePlaing()
    {
        OnGameStarted?.Invoke();

        StartCoroutine(GamePlayCorountine());
    }


    private void Winning()
    {
        OnGameWon?.Invoke();

        _shipInventoryService.Weapon = _playerInventoryService.GetWeapon();

        ShipInventoryService previousShipInventoryService = new(GameProgressDataIO.LoadData().shipInventoryData);

        previousShipInventoryService += _shipInventoryService;

        GameProgressDataIO.SaveData(new GameProgressData(previousShipInventoryService, _playerStatsService));
        SceneManager.LoadScene("ShipScene");
    }

    private void Losing()
    {
        OnGameLost?.Invoke();

        SceneManager.LoadScene("MainMenu");
        GameProgressDataIO.DeleteGameProgressData();
    }


    private void _playerStatsService_OnPlayerDied()
    {
        _currentGameState = GameState.Losing;
    }

    private IEnumerator WaitingToStartCoroutine()
    {
        yield return new WaitForSeconds(_waitingToStartTime);

        _currentGameState = GameState.CountdownToStart;
    }

    private IEnumerator CountdownToStartCoroutine()
    {
        for (int i = 1; i <= _countdownToStartTime; i++)
        {
            Debug.Log(i);
            yield return new WaitForSeconds(1f);
        }

        _currentGameState = GameState.GamePlaying;
    }

    private IEnumerator GamePlayCorountine()
    {
        Debug.Log("Game started!");

        for (int i = 1; i <= _gameTime; i++)
        {
            Debug.Log(i);
            yield return new WaitForSeconds(1f);
        }

        if (_shipZoneTriggerHandler.IsPlayerStaying())
            _currentGameState = GameState.Winning;
        else
            _currentGameState = GameState.Losing;
    }
}
