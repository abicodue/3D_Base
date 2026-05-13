using UnityEngine;
using UnityEngine.InputSystem;

public class TPS_TwoStepHitscanWeapon : MonoBehaviour
{
    [SerializeField]
    private Camera aimCamera;
    [SerializeField]
    private float aimRange = 20f;
    [SerializeField]
    private LayerMask aimMask;
    [SerializeField]
    private LayerMask shotMask;
    [SerializeField]
    private LayerMask muzzleBlockMask;
    [SerializeField]
    private float muzzleBlockRadius = 0.5f;
    [SerializeField]
    private Transform muzzle;
    [SerializeField]
    private float shotRange = 20f;
    [SerializeField]
    private float shotRadius;
    [SerializeField]
    private TPS_TwoStepHit tpsHitPrefab;
    [SerializeField]
    private SimpleObjectPool bulletPool;
    [SerializeField]
    private float bulletSpeed = 3f;
    [SerializeField]
    private float bulletLifeTime = 3f;
    [SerializeField]
    private float damageAmount = 10f;



    private bool checkMuzzleBlocked = true;


    private PlayerInput pi;
    private InputAction fire;

    private void Awake()
    {
        if (aimCamera == null)
        {
            aimCamera = Camera.main;
        }

        pi = GetComponentInParent<PlayerInput>();
        fire = pi.actions.FindAction("Fire", true);        

    }

    private void OnEnable()
    {
        fire.performed += Fire;
    }

    private void OnDisable()
    {
        fire.performed -= Fire;
    }


    private void DrawDebugRays(AimResult aim, ShotResult shot)
    {
        float aimDistance = aim.didHit ? aim.hit.distance : aimRange;

        Debug.DrawRay(aim.ray.origin, aim.ray.direction * aimDistance, Color.cyan, 0.5f);

        float shotDistance = shot.didHit ? shot.hit.distance : shot.distance;

        Debug.DrawRay(shot.origin, shot.direction * shotDistance, shot.didHit ? Color.red : Color.yellow, 0.5f);
    }

    private void OnDrawGizmosSelected()
    {
        if (muzzle != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(muzzle.position, muzzleBlockRadius);
        }
    }

    private AimResult ResolveAimPoint()
    {
        Ray aimRay = aimCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        AimResult result = new AimResult
        {
            ray = aimRay,
            didHit = false,
            point = aimRay.GetPoint(aimRange)
        };

        if (Physics.Raycast(aimRay, out RaycastHit hit, aimRange, aimMask, QueryTriggerInteraction.Ignore))
        {
            result.didHit = true;
            result.hit = hit;
            result.point = hit.point;
        }

        return result;
    }

    private ShotResult FireFromMuzzle(AimResult aimResult)
    {
        Vector3 toAimPoint = aimResult.point - muzzle.position;

        if (toAimPoint.sqrMagnitude < 0.0001f)
        {
            toAimPoint = aimCamera.transform.forward;
        }

        Vector3 shotDirection = toAimPoint.normalized;
        float distanceToAimPoint = toAimPoint.magnitude;

        float castDistance = aimResult.didHit ? Mathf.Min(shotRange, distanceToAimPoint + 0.05f) : shotRange;

        ShotResult result = new ShotResult
        {
            origin = muzzle.position,
            direction = shotDirection,
            distance = castDistance,
            didHit = false
        };

        if (CastShot(muzzle.position, shotDirection, castDistance, out RaycastHit shotHit))
        {
            result.didHit = true;
            result.hit = shotHit;
        }

        return result;

    }

    private bool CastShot ( Vector3 origin, Vector3 direction, float distance, out RaycastHit hit)
    {
        if (shotRadius > 0f)
        {
            return Physics.SphereCast(origin, shotRadius, direction, out hit, distance, shotMask, QueryTriggerInteraction.Ignore);
        }

        return Physics.Raycast(origin, direction, out hit, distance, shotMask, QueryTriggerInteraction.Ignore);
    }

    private bool IsMuzzleBlocked()
    {
        if (checkMuzzleBlocked == false)
        {
            return false;
        }

        return Physics.CheckSphere(muzzle.position, muzzleBlockRadius, muzzleBlockMask, QueryTriggerInteraction.Ignore);
    }

    private void LogAimAndShot(AimResult aimResult, ShotResult shotResult)
    {
        string aimName = aimResult.didHit && aimResult.hit.collider != null
            ? aimResult.hit.collider.name
            : "Null";

        string shotName = shotResult.didHit && shotResult.hit.collider != null
            ? shotResult.hit.collider.name
            : "Null";

        Debug.Log($"Camera Aim : {aimName} / Shot Aim : {shotName}");
    }

    public void Fire(InputAction.CallbackContext _)
    {
        if (aimCamera == null || muzzle == null)
        {
            Debug.LogWarning("Aim Camera or Muzzle is missing");
            return;
        }

        if (IsMuzzleBlocked())
        {
            Debug.Log("Can't Fire: Gun is too close to Obstacles");
            return;
        }

        AimResult aimResult = ResolveAimPoint();
        ShotResult shotResult = FireFromMuzzle(aimResult);

        DrawDebugRays(aimResult, shotResult);

        // 즉시타격 방식 != 오브젝트풀링 X
        /*
        if (shotResult.didHit && tpsHitPrefab != null)
        {
            tpsHitPrefab.HandleHit(shotResult.hit, aimResult);
        }
        */

        LogAimAndShot(aimResult, shotResult);

        GameObject newBullet = bulletPool.GetObject();

        newBullet.transform.SetPositionAndRotation(muzzle.position, Quaternion.LookRotation(shotResult.direction));

        TPS_TwoStepHit bullet = newBullet.GetComponent<TPS_TwoStepHit>();

        if (bullet != null)
        {
            Transform attacker = pi != null ? pi.transform : transform.root;

            bullet.Init(
                bulletPool, 
                shotResult.direction, 
                bulletSpeed, 
                bulletLifeTime, 
                damageAmount,
                attacker);
        }

    }

}
