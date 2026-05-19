using UnityEngine;

public class ItemDataTester : MonoBehaviour
{
    [SerializeField]
    private ItemData itemData;

    private void Start()
    {
        if (itemData == null)
        {
            return;
        }

        Debug.Log($"Item Id : {itemData.itemId}");
        Debug.Log($"Item Name : {itemData.itemName}");
        Debug.Log($"Item Type : {itemData.itemType}");
        Debug.Log($"Buy Price : {itemData.buyPrice}");
        Debug.Log($"Sell Price : {itemData.sellPrice}");
        Debug.Log($"Stackable : {itemData.canStack}");
    }

}
