using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CaptainsLogSingleItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _itemAmount;
    [SerializeField] private Image _itemIcon;

    public void SetValues(int amount, Sprite sprite)
    {
        _itemAmount.text = amount.ToString();
        _itemIcon.sprite = sprite;
    }
}
