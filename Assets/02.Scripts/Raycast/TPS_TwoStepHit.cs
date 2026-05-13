using SystemicOverload.Combat;
using UnityEngine;

public class TPS_TwoStepHit : MonoBehaviour
{
    [SerializeField]
    private float damageAmount = 10f;
    [SerializeField]
    private GameObject hitEffectPrefab;
    [SerializeField]
    private Transform attacker;
    [SerializeField]
    private SimpleObjectPool bulletPool;
    [SerializeField]
    private Vector3 moveDirection;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float lifeTime;
    [SerializeField]
    private float lifeTimer;

    private bool isReturned;

    public void Init(
        SimpleObjectPool pool, 
        Vector3 direction, 
        float speed, 
        float lifeTime, 
        float damageAmount, 
        Transform attacker)
    {
        bulletPool = pool;
        moveDirection = direction.normalized;
        moveSpeed = speed;

        this.damageAmount = damageAmount;
        this.attacker = attacker;        

        lifeTimer = lifeTime;
        isReturned = false;

        if (moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }
    }


    private void Update()
    {
        if (isReturned)
        {
            return;
        }

        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        lifeTimer -= Time.deltaTime;

        if (lifeTimer <= 0f)
        {
            ReturnToPool();
        }

    }



    private void OnTriggerEnter(Collider other)
    {
        if (isReturned)
        {
            return;
        }

        if (attacker != null && other.transform.root == attacker.root)
        {
            return;
        }
        
        HealthComponent hc = other.GetComponentInParent<HealthComponent>();

        if (hc != null && hc.IsAlive)
        {
            DamagePayload damage = new DamagePayload
            {
                Amount = damageAmount,
                Attacker = attacker
            };

            hc.ApplyDamage(in damage);
        }     

        if (hitEffectPrefab != null)
        {
            Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
        }

        ReturnToPool();
    }

    

    private void ReturnToPool()
    {
        if (isReturned)
        {
            return;
        }

        isReturned = true;

        if (bulletPool != null)
        {
            bulletPool.ReturnObject(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }


    // 오브젝트풀링 없이 즉시타격
    /*
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
    */

}
