using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : UntakeableItem
{
    private ShipInventoryService _shipInventoryService;
    private PlayerStatsService _playerStatsService;

    public void Construct(ShipInventoryService shipInventoryService, PlayerStatsService playerStatsService)
    {
        _shipInventoryService = shipInventoryService;
        _playerStatsService = playerStatsService;
    }    

    public override void Interact()
    {
        SceneManager.LoadScene("IslandScene");
    }
}
