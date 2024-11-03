using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GameEventManager : IInitializable
{
    public List<GameEventSO> LastGameEventsList { get; private set; } = new();

    private ShipInventoryService _shipInventoryService;
    private PlayerStatsService _playerStatsService = new();
    private InputService _inputService;
    private SceneLoadService _sceneLoader;

    private int _daysAmount;

    private GameEventSOSet _eventSetSO;
    
    [Inject]
    public void Construct(IShipInventoryService shipInventoryService, InputService inputService, SceneLoadService sceneLoader)
    {
        _shipInventoryService = (ShipInventoryService)shipInventoryService;
        _inputService = inputService;
        _sceneLoader = sceneLoader;
    }

    public void Initialize()
    {
        EnablePlayerControl();

        LoadEventSet();

        LoadGameProgress();

        if (IsNewRound())
        {
            SubscribeToEvents();

            StartGameEvents();

            SaveGameProgress();
        }
    }

    private void EnablePlayerControl()
    {
        _inputService.EnableInteractions(true);
        _inputService.EnableCameraLook(true);
        _inputService.EnableMovement(true);
        _inputService.EnableExit(true);

        _inputService.SetCursorMode(CursorMode.Game);
    }

    private void LoadEventSet()
    {
        _eventSetSO = SOSetLoader.LoadGameEventSOSet();
    }

    private void LoadGameProgress()
    {
        GameProgressData gameProgressData = GameProgressDataIO.LoadData();

        LastGameEventsList.DeserializeFromData(gameProgressData.LastGameEvents);
        _shipInventoryService.DeserializeFromData(gameProgressData.ShipInventoryData);
        _playerStatsService.DeserializeFromData(gameProgressData.PlayerStatsData);
        _daysAmount = gameProgressData.DaysAmount;
    }

    private bool IsNewRound()
    {
        return LastGameEventsList.Count == 0;
    }

    private void SubscribeToEvents()
    {
        _shipInventoryService.OnStatOver += _shipInventoryService_OnStatOver;
    }

    private void SaveGameProgress()
    {
        GameProgressDataIO.SaveData(new GameProgressData(_shipInventoryService, _playerStatsService, _daysAmount, LastGameEventsList));
    }

    private void _shipInventoryService_OnStatOver(StatsType statsType)
    {
        LosingReasonSOSet.LosingReason losingReason = statsType switch
        {
            StatsType.Food => LosingReasonSOSet.LosingReason.RunOutFood,
            StatsType.Water => LosingReasonSOSet.LosingReason.RunOutWater,
            StatsType.Material => LosingReasonSOSet.LosingReason.RunOutMaterials
        };

        LosingReasonSOSet.currentLosingReason = losingReason;

        _sceneLoader.Load(SceneLoadService.Scene.Losing, true);
    }

    private void StartGameEvents()
    {
        if (_daysAmount == 1)
        {
            StartStartingGameEvents();
        }

        if (_playerStatsService.TypedHitsList.Count != 0)
        {
            StartDamageGameEvents();
        }

        StartStatGameEvents();
        StartItemGameEvents();
        StartDefaultGameEvent();
    }

    private void StartStartingGameEvents()
    {
        List<LootGameEventSO> startingEvents = new();

        LootGameEventSO foodStartingGameEventSO = GameRandomizer.GetRandomItem(_eventSetSO.foodStartingEvents);
        startingEvents.Add(foodStartingGameEventSO);

        LootGameEventSO waterStartingGameEventSO = GameRandomizer.GetRandomItem(_eventSetSO.waterStartingEvents);
        startingEvents.Add(waterStartingGameEventSO);

        LootGameEventSO materialsStartingGameEventSO = GameRandomizer.GetRandomItem(_eventSetSO.materialsStartingEvents);
        startingEvents.Add(materialsStartingGameEventSO);

        LootGameEventSO rareItemsStartingGameEventSO = GameRandomizer.GetRandomItem(_eventSetSO.rareItemsStartingEvents);
        startingEvents.Add(rareItemsStartingGameEventSO);

        foreach (var gameEvent in startingEvents)
        {
            LastGameEventsList.Add(gameEvent);

            var eventHandler = GameEventHandlerFactory.CreateHandler(gameEvent);
            eventHandler.ApplyToInventory(_shipInventoryService);
        }
    }

    private void StartDamageGameEvents()
    {
        Dictionary<DamageType, float> totalDamage = new();

        foreach (var hit in _playerStatsService.TypedHitsList)
        {
            if (!totalDamage.ContainsKey(hit.damageType))
            {
                totalDamage[hit.damageType] = 0f;
            }

            totalDamage[hit.damageType] += hit.damage;
        }

        // Searching for the key corresponding to the largest value
        DamageType topDamageType = totalDamage.OrderByDescending(x => x.Value).FirstOrDefault().Key;

        // Searching for an event corresponding to a key
        DamageGameEventSO damageGameEventSO = _eventSetSO.damageGameEvents.FirstOrDefault(eventSO => eventSO.damageType == topDamageType);

        LastGameEventsList.Add(damageGameEventSO);

        var eventHandler = GameEventHandlerFactory.CreateHandler(damageGameEventSO);
        eventHandler.ApplyToInventory(_shipInventoryService);
    }

    private void StartStatGameEvents()
    {
        StatGameEventSO statGameEventSO = GameRandomizer.GetRandomItem(_eventSetSO.statEvents);

        LastGameEventsList.Add(statGameEventSO);

        var eventHandler = GameEventHandlerFactory.CreateHandler(statGameEventSO);
        eventHandler.ApplyToInventory(_shipInventoryService);
    }

    private void StartItemGameEvents()
    {
        ItemGameEventSO itemGameEventSO = GameRandomizer.GetRandomItem(_eventSetSO.itemEvents);

        var eventHandler = GameEventHandlerFactory.CreateHandler(itemGameEventSO);

        if(eventHandler.TryApplyToInventory(_shipInventoryService))
        {
            LastGameEventsList.Add(itemGameEventSO);
        }
        else if(itemGameEventSO is ComplexItemGameEventSO complexEventSO)
        {
            LastGameEventsList.Add(complexEventSO.alternativeGameEventSO);
        }
    }

    private void StartDefaultGameEvent()
    {
        var gameEvent = _eventSetSO.defaultStatLossSO;

        LastGameEventsList.Add(gameEvent);

        var eventHandler = GameEventHandlerFactory.CreateHandler(gameEvent);
        eventHandler.ApplyToInventory(_shipInventoryService);
    }
}
