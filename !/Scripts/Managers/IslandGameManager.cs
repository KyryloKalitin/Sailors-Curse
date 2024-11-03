using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class IslandGameManager : MonoBehaviour
{
    private GameTimerService _gameTimerService;
    private PlayerStatsService _playerStatsService;
    private LoadingScreenController _loadingScreenController;

    private GameState _currentGameState = GameState.None;

    private GameWinHandler _gameWinHandler;
    private GameLoseHandler _gameLoseHandler;

    private LosingReasonSOSet.LosingReason _losingReason;

    [Inject]
    public void Construct(PlayerStatsService playerStatsService, LoadingScreenController loadingScreenController, GameTimerService gameTimerService, GameWinHandler gameWinHandler, GameLoseHandler gameLoseHandler)
    {
        _playerStatsService = playerStatsService;
        _loadingScreenController = loadingScreenController;
        _gameTimerService = gameTimerService;

        _gameWinHandler = gameWinHandler;
        _gameLoseHandler = gameLoseHandler;
    }

    private void Start()
    {
        _gameTimerService.OnCountdownEnded += _gameTimerService_OnCountdownEnded;
        _gameTimerService.OnGameplayEnded += _gameTimerService_OnGameplayEnded;

        _playerStatsService.OnPlayerDied += _playerStatsService_OnPlayerDied;

        _loadingScreenController.GetComponentInChildren<LoadingScreenBackground>().OnAnimationEnded += IslandGameManager_OnAnimationEnded;
    }

    private void UpdateGameState(GameState newState)
    {
        if (_currentGameState == newState)
            return;

        _currentGameState = newState;

        switch (_currentGameState)
        {
            case GameState.CountdownToStart:
                CountdownToStart();
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

    private void CountdownToStart()
    {
        StartCoroutine(_gameTimerService.CountdownToStartCoroutine());
    }

    private void GamePlaing()
    {
        StartCoroutine(_gameTimerService.GamePlayCorountine());
    }

    private void Winning()
    {
        _gameWinHandler.HandleWin();
    }

    private void Losing()
    {
        _gameLoseHandler.HandleLose(_losingReason);
    }

    private void IslandGameManager_OnAnimationEnded()
    {
        UpdateGameState(GameState.CountdownToStart);
    }

    private void _playerStatsService_OnPlayerDied()
    {
        _losingReason = LosingReasonSOSet.LosingReason.PlayerDied;

        UpdateGameState(GameState.Losing);
    }

    private void _gameTimerService_OnGameplayEnded()
    {
        CalculateGameResult();
    }

    private void CalculateGameResult()
    {
        var lastState = _gameWinHandler.IsPlayerInWinningZone ? GameState.Winning : GameState.Losing;

        if (lastState == GameState.Losing)
            _losingReason = LosingReasonSOSet.LosingReason.EndTime;

        UpdateGameState(lastState);       
    }

    private void _gameTimerService_OnCountdownEnded()
    {
        UpdateGameState(GameState.GamePlaying);
    }
}

public class GameTimerService
{
    public event Action OnCountdownStarted;
    public event Action OnGameplayStarted;

    public event Action OnCountdownEnded;
    public event Action OnGameplayEnded;

    public float CountdownToStartTime { get; private set; } = 3f;
    public float GamePlayTime { get; private set; } = 60f;

    public IEnumerator CountdownToStartCoroutine()
    {
        OnCountdownStarted?.Invoke();

        while (CountdownToStartTime > 0)
        {
            yield return new WaitForSeconds(1f);
            CountdownToStartTime--;
        }

        OnCountdownEnded?.Invoke();
    }

    public IEnumerator GamePlayCorountine()
    {
        OnGameplayStarted?.Invoke();

        while (GamePlayTime > 0)
        {
            yield return new WaitForSeconds(1f);
            GamePlayTime--;
        }

        OnGameplayEnded?.Invoke();
    }
}

public class GameWinHandler
{
    public bool IsPlayerInWinningZone => _shipZoneTriggerHandler.IsPlayerStaying;

    private ShipZoneTriggerHandler _shipZoneTriggerHandler;
    private SceneLoadService _sceneLoader;

    private PlayerStatsService _playerStatsService;
    private ShipZoneInventoryService _shipZoneInventoryService;

    [Inject]
    public void Construct(  ShipZoneTriggerHandler shipZoneTriggerHandler, IShipInventoryService shipZoneInventoryService,
                            SceneLoadService sceneLoadService, PlayerStatsService playerStatsService)
    {
        _shipZoneTriggerHandler = shipZoneTriggerHandler;
        _shipZoneInventoryService = (ShipZoneInventoryService)shipZoneInventoryService;
        _sceneLoader = sceneLoadService;
        _playerStatsService = playerStatsService;
    }

    public void HandleWin()
    {
        SaveRoundProgress();
        _sceneLoader.Load(SceneLoadService.Scene.Ship);
    }

    private void SaveRoundProgress()
    {
        _shipZoneTriggerHandler.TakePlayerWeapon();

        GameProgressData gameData = GameProgressDataIO.LoadData();

        ShipInventoryService shipInventoryService = new();
        shipInventoryService.DeserializeFromData(gameData.ShipInventoryData);

        shipInventoryService.ApplyChangesAfterRound(_shipZoneInventoryService);

        GameProgressDataIO.SaveData(new GameProgressData(shipInventoryService, _playerStatsService, gameData.DaysAmount + 1));
    }
}

public class GameLoseHandler
{
    private SceneLoadService _sceneLoader;

    [Inject]
    public void Construct(SceneLoadService sceneLoadService)
    {
        _sceneLoader = sceneLoadService;
    }

    public void HandleLose(LosingReasonSOSet.LosingReason losingReason)
    {
        LosingReasonSOSet.currentLosingReason = losingReason;

        _sceneLoader.Load(SceneLoadService.Scene.Losing);
    }    
}
