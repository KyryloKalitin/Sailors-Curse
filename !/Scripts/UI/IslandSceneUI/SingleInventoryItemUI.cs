using UnityEngine;
using UnityEngine.UI;

public class SingleInventoryItemUI : MonoBehaviour
{
    [SerializeField] private Image _itemIcon;

    public void SetIcon(Sprite sprite)
    {
        if(sprite != null)
            _itemIcon.sprite = sprite;
    }    
}
