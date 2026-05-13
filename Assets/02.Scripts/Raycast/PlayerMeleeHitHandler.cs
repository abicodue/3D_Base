using SystemicOverload.Combat;
using UnityEngine;

public class PlayerMeleeHitHandler : MonoBehaviour
{
    [SerializeField]
    private float damageAmount = 20.0f;

    public void HandleMeleeHit(HealthComponent targetHp, Collider hitCollider)
    {
        if (targetHp == null || !targetHp.IsAlive)
        {
            return;
        }

        DamagePayload damage = new DamagePayload
        {
            Amount = damageAmount,
            Attacker = transform
        };

        Debug.Log($"[Melee Hit] {hitCollider.name}");

        targetHp.ApplyDamage(in damage);

    }
   
}
