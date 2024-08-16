using UnityEngine;
using Zenject;

public class ShipZoneTriggerHandler : MonoBehaviour
{
    [SerializeField] private ShipZoneUI _shipZoneUI;

    private ShipInventoryService _shipInventoryService;

    private bool _isPlayerStaying;

    [Inject]
    public void Construct(ShipInventoryService shipInventoryService)
    {
        _shipInventoryService = shipInventoryService;
    }

    private void Update()
    {
        if (IsPlayerStaying())
            _shipZoneUI.Show();
        else
            _shipZoneUI.Hide();
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out IslandPlayerController playerController))
        {
            _isPlayerStaying = true;

            playerController.PlayerInventoryService.UnboxAll(_shipInventoryService);
            return;
        }

        if(other.gameObject.TryGetComponent(out IUnboxable item))
        {
            if(other.gameObject.TryGetComponent(out HandheldItem handheldItem) && !handheldItem.HasParent())
            {
                item.Unbox(_shipInventoryService);
            }
        }       
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IslandPlayerController playerController))
        {
            _isPlayerStaying = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IslandPlayerController playerController))
        {
            _isPlayerStaying = false;
        }
    }

    public bool IsPlayerStaying()
    {
        return _isPlayerStaying;
    }   
}
