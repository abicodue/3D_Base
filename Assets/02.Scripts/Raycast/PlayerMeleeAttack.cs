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
        attack.performed += OverlabSphereAttack;
    }

    private void OnDisable()
    {
        attack.performed -= OverlabSphereAttack;
    }

    private void OverlabSphereAttack(InputAction.CallbackContext _)
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

            hc.ApplyDamage(in damage);            
            
            Debug.Log($"[NonAlloc Hit] {results[i].name}");
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
