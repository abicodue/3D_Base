using UnityEngine;
using UnityEngine.InputSystem;

public class ItemUseTester : MonoBehaviour
{
    [SerializeField]
    private PlayerStatus playerStatus;
    [SerializeField]
    private ItemData itemToUse;
    [SerializeField]
    private InputAction useItemAction = new InputAction("UseItem", InputActionType.Button, "<Keyboard>/u");

    private void OnEnable()
    {
        useItemAction.performed += OnUseItemPerformed;
        useItemAction.Enable();
    }

    private void OnDisable()
    {
        useItemAction.performed -= OnUseItemPerformed;
        useItemAction.Disable();
    }

    private void OnUseItemPerformed(InputAction.CallbackContext context)
    {
        UseItem(itemToUse);
    }

    public void UseItem(ItemData itemData)
    {
        if (itemData == null)
        {
            return;
        }

        if (!itemData.canUse)
        {
            Debug.Log("non-usable item");
            return;
        }

        Debug.Log($"아이템 사용: {itemData.itemName}");

        foreach (ItemEffect effect in itemData.effects)
        {
            ApplyEffect(effect);
        }
    }

    private void ApplyEffect(ItemEffect effect)
    {
        switch (effect.effectType)
        {
            case ItemEffectType.HealHp:
                playerStatus.HealHp(effect.value);
                break;
            case ItemEffectType.HealMp:
                playerStatus.HealMp(effect.value);
                break;
            case ItemEffectType.IncreaseAttack:
                playerStatus.IncreaseAttack(effect.value);
                break;
            case ItemEffectType.IncreaseDefense:
                playerStatus.IncreaseDefense(effect.value);
                break;
            case ItemEffectType.AddGold:
                playerStatus.AddGold(effect.value);
                break;

        }
    }


}
