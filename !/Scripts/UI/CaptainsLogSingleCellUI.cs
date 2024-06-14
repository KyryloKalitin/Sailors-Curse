using TMPro;
using UnityEngine;

public class CaptainsLogSingleCellUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _eventDescription;

    [SerializeField] private Transform _itemContainer;
    [SerializeField] private Transform _itemTemplate;

    private string _STAT_TYPES_SPRITES = "Data/StatTypesSprites";

    public void SetEventSO(GameEventSO gameEventSO)
    {
        _eventDescription.text = gameEventSO._description;

        foreach (Transform child in _itemContainer)
        {
            Destroy(child.gameObject);
        }

        if(gameEventSO is LootGameEventSO lootGameEventSO)
        {
            foreach (var item in lootGameEventSO.inventoryItemsSOList)
            {
                Transform itemCell = Instantiate(_itemTemplate, _itemContainer);
                itemCell.gameObject.SetActive(true);

                itemCell.GetComponent<CaptainsLogSingleItemUI>().SetValues(item.amount, item.inventoryItemSO.sprite);
            }
        }

        if(gameEventSO is StatGameEventSO statGameEventSO)
        {
            if (statGameEventSO.statsChanged == null)
                return;

            if (statGameEventSO.statsChanged.Count == 0)
                return;

            foreach (var item in statGameEventSO.statsChanged)
            {
                Transform itemCell = Instantiate(_itemTemplate, _itemContainer);
                itemCell.gameObject.SetActive(true);

                StatTypesSprites statTypesSprites = Resources.Load<StatTypesSprites>(_STAT_TYPES_SPRITES);

                Sprite itemSprite;

                switch (item.statsType)
                {
                    case StatsType.Food:
                        itemSprite = statTypesSprites.foodSprite.sprite;
                        break;
                    case StatsType.Water:
                        itemSprite = statTypesSprites.waterSprite.sprite;
                        break;
                    case StatsType.Material:
                        itemSprite = statTypesSprites.materialsSprite.sprite;
                        break;
                    default:
                        itemSprite = statTypesSprites.foodSprite.sprite;
                        break;
                }

                itemCell.GetComponent<CaptainsLogSingleItemUI>().SetValues(item.statAmount, itemSprite);
            }
        }

    }
}
