using TMPro;
using UnityEngine;

public class CaptainsLogSingleCellUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _eventDescription;

    [SerializeField] private Transform _itemContainer;
    [SerializeField] private Transform _itemTemplate;

    public void SetEventSO(GameEventSO gameEventSO)
    {
        _eventDescription.text = gameEventSO.description;

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

                Sprite itemSprite = StatSpriteLoader.GetSpriteByType(item.statsType);

                itemCell.GetComponent<CaptainsLogSingleItemUI>().SetValues(item.statAmount, itemSprite);
            }
        }
    }
}
