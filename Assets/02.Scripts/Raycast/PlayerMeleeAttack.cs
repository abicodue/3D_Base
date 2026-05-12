using SystemicOverload.Combat;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMeleeAttack : MonoBehaviour
{
    [SerializeField]
    private Transform attackPoint;
    [SerializeField]
    private LayerMask hittableMask;
    [SerializeField]
    private float radius = 2.0f;

    private PlayerInput pi;
    private InputAction attack;

    private Collider[] results = new Collider[10];

    private void Awake()
    {
        pi = GetComponent<PlayerInput>();
        attack = pi.actions.FindAction("Attack", true);

    }

    private void OnEnable()
    {
        attack.performed += OverlapSphereAttack;
    }

    private void OnDisable()
    {
        attack.performed -= OverlapSphereAttack;
    }

    private void OverlapSphereAttack(InputAction.CallbackContext _)
    {
        
        

        int count = Physics.OverlapSphereNonAlloc(attackPoint.position, radius, results, hittableMask, QueryTriggerInteraction.Ignore);

        for (int i = 0; i < count; i++)
        {            

            HealthComponent hc = results[i].GetComponentInParent<HealthComponent>();

            if (hc == null || !hc.IsAlive)
            {
                continue;
            }

            DamagePayload damage = new DamagePayload
            {
                Amount = 20f,
                Attacker = transform
            };

            Debug.Log($"[NonAlloc Hit] {results[i].name}");

            hc.ApplyDamage(in damage);
            
        }

    }

    private void OnDrawGizmos()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, radius);
    }



}
