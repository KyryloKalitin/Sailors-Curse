using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class GameEventManager 
{
    public List<GameEventSO> LastGameEventsList { get; private set; }

    private ShipInventoryService _shipInventoryService;
    private PlayerStatsService _playerStatsService;

    private int _daysAmount;

    private string _EVENT_SET_PATH = "Data/GameEventSet";
    private GameEventSetSO _eventSetSO;


    [Inject]
    public void Construct(ShipInventoryService shipInventoryService)
    {
        _shipInventoryService = shipInventoryService;

        Initialize();
    }

    private void Initialize()
    {
        GameProgressData gameProgressData = GameProgressDataIO.LoadData();        

        LastGameEventsList = gameProgressData.LastGameEvents.GameEventsSOList;

        _playerStatsService = new(gameProgressData.PlayerStatsData);
        _daysAmount = gameProgressData.DaysAmount;

        if (LastGameEventsList.Count == 0)
        {
            _shipInventoryService.OnStatOver += _shipInventoryService_OnStatOver;
            _eventSetSO = Resources.Load<GameEventSetSO>(_EVENT_SET_PATH);

            StartGameEvents();

            _shipInventoryService.UpdateStats(_eventSetSO.defaultStatLossSO.statsChanged);

            GameProgressDataIO.SaveData(new GameProgressData   (new(_shipInventoryService), new(_playerStatsService), 
                                                                new(LastGameEventsList), gameProgressData.DaysAmount));
        }
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

        LastGameEventsList.Add(statGameEventSO);

        var eventHandler = GameEventHandlerFactory.CreateHandler(statGameEventSO);
        eventHandler.ApplyToInventory(_shipInventoryService);
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

    private void StartItemGameEvents()
    {
        ItemGameEventSO itemGameEventSO = GameRandomizer.GetRandomItem(_eventSetSO.itemEvents);

        IConditionalGameEventHandler eventHandler = (IConditionalGameEventHandler)GameEventHandlerFactory.CreateHandler(itemGameEventSO);

        ComplexItemGameEventSO complexEventSO = itemGameEventSO as ComplexItemGameEventSO;

        if(eventHandler.TryApplyToInventory(_shipInventoryService))
        {
            LastGameEventsList.Add(itemGameEventSO);
        }
        else if(complexEventSO != null)
        {
            LastGameEventsList.Add(complexEventSO.alternativeItemGameEventSO);
        }
    }
}