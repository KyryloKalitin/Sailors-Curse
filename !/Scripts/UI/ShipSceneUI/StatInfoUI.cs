using TMPro;
using UnityEngine;
using Zenject;

public class StatInfoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _foodText;
    [SerializeField] private TextMeshProUGUI _waterText;
    [SerializeField] private TextMeshProUGUI _materialsText;

    private ShipInventoryService _shipInventoryService;

    [Inject]
    public void Construct(ShipInventoryService shipInventoryService)
    {
        _shipInventoryService = shipInventoryService;
    }

    private void Start()
    {
        _shipInventoryService.OnStatChanged += _shipInventoryService_OnStatChanged;

        _shipInventoryService_OnStatChanged();
    }

    private void _shipInventoryService_OnStatChanged()
    {
        _foodText.text = _shipInventoryService.FoodAmount.ToString();
        _waterText.text = _shipInventoryService.WaterAmount.ToString();
        _materialsText.text = _shipInventoryService.MaterialsAmount.ToString();
    }
}
