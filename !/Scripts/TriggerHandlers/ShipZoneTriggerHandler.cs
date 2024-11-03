using UnityEngine;
using Zenject;

public class ShipZoneTriggerHandler : MonoBehaviour
{
    [SerializeField] private ShipZoneUI _shipZoneUI;

    public bool IsPlayerStaying => _currentPlayerController != null;

    private IslandPlayerController _currentPlayerController;
    private ShipZoneInventoryService _shipInventoryService;

    [Inject]
    public void Construct(IShipInventoryService shipInventoryService)
    {
        _shipInventoryService = (ShipZoneInventoryService)shipInventoryService;
    }

    private void Update()
    {
        if (IsPlayerStaying)
            _shipZoneUI.Show();
        else
            _shipZoneUI.Hide();
    }

    public void TakePlayerWeapon()
    {
        _shipInventoryService.WeaponSO = _currentPlayerController.PlayerInventoryService.Weapon?.WeaponSO;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out IslandPlayerController playerController))
        {
            _currentPlayerController = playerController;

            playerController.PlayerInventoryService.UnboxAll(_shipInventoryService);
            return;
        }

        if(other.gameObject.TryGetComponent(out IUnboxable item))
        {
            if(other.gameObject.TryGetComponent(out HandheldItem handheldItem) && !handheldItem.GetComponent<Collider>().isTrigger)
            {
                item.Unbox(_shipInventoryService);
            }
        }       
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IslandPlayerController playerController))
        {
            _currentPlayerController = playerController;

            playerController.PlayerInventoryService.UnboxAll(_shipInventoryService);
            return;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IslandPlayerController playerController))
        {
            _currentPlayerController = null;
        }
    }
}
