using SystemicOverload.Combat;
using UnityEngine;

public class TPS_TwoStepHit : MonoBehaviour
{
    [SerializeField]
    private float damageAmount = 10f;
    [SerializeField]
    private GameObject hitEffectPrefab;
    


    public void HandleHit(RaycastHit hit, AimResult aimResult)
    {
        string aimName = aimResult.didHit ? aimResult.hit.collider.name : "Null";

        string shotName = hit.collider.name;
        Debug.Log($"Camera Aim : {aimName} / Shot Aim : {shotName}");

        HealthComponent hc = hit.collider.GetComponentInParent<HealthComponent>();               

        if (hc != null && hc.IsAlive)
        {
            DamagePayload damage = new DamagePayload
            {
                Amount = damageAmount,
                Attacker = transform
            };

            hc.ApplyDamage(in damage);
        }

        if (hitEffectPrefab != null)
        {
            Instantiate(hitEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
        }       

    }
}
