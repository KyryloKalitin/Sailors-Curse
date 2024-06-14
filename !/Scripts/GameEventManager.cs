using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GameEventManager 
{
    private ShipInventoryService _shipInventoryService;
    private PlayerStatsService _playerStatsService;

    private int _daysAmount;

    private string _EVENT_SET_PATH = "Data/GameEventSet";
    private GameEventSetSO _eventSetSO;

    private List<GameEventSO> _lastGameEventsList;

    [Inject]
    public void Construct(ShipInventoryService shipInventoryService)
    {
        _shipInventoryService = shipInventoryService;

        Initialize();
    }

    private void Initialize()
    {
        GameProgressData gameProgressData = GameProgressDataIO.LoadData();

        _lastGameEventsList = gameProgressData.lastGameEvents.GetGameEventsList();
        _playerStatsService = new(gameProgressData.playerStatsData);
        _daysAmount = gameProgressData.daysAmount;

        if (_lastGameEventsList.Count == 0)
        {
            _shipInventoryService.OnStatOver += _shipInventoryService_OnStatOver;
            _eventSetSO = Resources.Load<GameEventSetSO>(_EVENT_SET_PATH);

            StartGameEvents();
            _shipInventoryService.GetStats();

            _shipInventoryService.UpdateStats(_eventSetSO.defaultStatLossSO.statsChanged);
            _shipInventoryService.GetStats();

            GameProgressDataIO.SaveData(new GameProgressData(_shipInventoryService, _playerStatsService, 1, _lastGameEventsList));
        }
    }

    public List<GameEventSO> GetEventsList()
    {
        return _lastGameEventsList;
    }

    private void _shipInventoryService_OnStatOver()
    {
        //SceneManager.LoadScene("MainMenu");
        //GameProgressDataIO.DeleteGameProgressData();
    }

    private void StartGameEvents()
    {
        if (_daysAmount == 0)
        {
            StartStartingGameEvents();
        }

        if (_playerStatsService.TypedHitsList.Count != 0)
        {
            StartDamageGameEvents();
        }

        StartStatGameEvents();
        StartItemGameEvents();
    }

    private void StartStatGameEvents()
    {
        StatGameEventSO statGameEventSO = GameRandomizer.GetRandomItem(_eventSetSO.statEvents);

        _lastGameEventsList.Add(statGameEventSO);

        StatGameEvent statGameEvent = new(statGameEventSO);
        statGameEvent.ApplyEventToInventory(_shipInventoryService);
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

        DamageType topDamageType = totalDamage.OrderByDescending(x => x.Value).FirstOrDefault().Key;

        DamageGameEventSO damageGameEventSO = _eventSetSO.damageGameEvents.FirstOrDefault(eventSO => eventSO.damageType == topDamageType);
        _lastGameEventsList.Add(damageGameEventSO);

        DamageGameEvent damageGameEvent = new(damageGameEventSO);
        damageGameEvent.ApplyEventToInventory(_shipInventoryService);
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
            _lastGameEventsList.Add(gameEvent);

            LootGameEvent lootGameEvent = new(gameEvent);
            lootGameEvent.ApplyEventToInventory(_shipInventoryService);
        }
    }

    private void StartItemGameEvents()
    {
        ItemGameEventSO itemGameEventSO = GameRandomizer.GetRandomItem(_eventSetSO.itemEvents);

        ItemGameEvent itemGameEvent = new(itemGameEventSO);

        if(itemGameEvent.TryApplyEventToInventory(_shipInventoryService))
        {
            _lastGameEventsList.Add(itemGameEventSO);
        }
        else
        {
            if(itemGameEvent is ComplexItemGameEvent complexItemGameEvent)
            {
                ComplexItemGameEventSO complexItemGameEventSO = (ComplexItemGameEventSO)complexItemGameEvent.eventSO;

                _lastGameEventsList.Add(complexItemGameEventSO.alternativeItemGameEventSO);
            }
        }
    }
}