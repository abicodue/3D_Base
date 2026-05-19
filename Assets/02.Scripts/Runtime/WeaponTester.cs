using UnityEngine;

public class WeaponTester : MonoBehaviour
{
    [SerializeField]
    private WeaponData[] weapons;

    private void Start()
    {
        if (weapons == null || weapons.Length <= 0)
        {
            return;
        }

        foreach (WeaponData weaponData in weapons)
        {
            Debug.Log("==========");
            Debug.Log($"Item Id: {weaponData.weaponId}");
            Debug.Log($"Item Name: {weaponData.weaponName}");
            Debug.Log($"Class Type: {weaponData.classType}");
            Debug.Log($"Range Weapon?: {weaponData.isRange}");
            Debug.Log($"Use Ammo?: {weaponData.useAmmo}");
            Debug.Log($"Stackable: {weaponData.canStack}");
        }

    }

}
