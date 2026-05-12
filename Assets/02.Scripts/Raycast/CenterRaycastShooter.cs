using UnityEngine;
using UnityEngine.InputSystem;

public class CenterRaycastShooter : MonoBehaviour
{
    [Header("Raycast")]
    // 카메라 (화면)의 중점에서 Ray 쏘고자 함 -> 카메라를 알아야함
    [SerializeField]
    private Camera m_cam;
    [SerializeField]
    private LayerMask m_hittableMask;
    [SerializeField]
    private float m_maxDistance = 100.0f;

    private PlayerInput _pi;
    private InputAction _fire;

    /*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + transform.forward * 10.0f, 5.0f);
    }
    */

    private void Awake()
    {
        _pi = GetComponent<PlayerInput>();
        _fire = _pi.actions.FindAction("Fire", true);

        if (m_cam == null) m_cam = Camera.main;
    }

    private void OnEnable()
    {
        _fire.performed += RaycastExample;
    }

    private void OnDisable()
    {
        _fire.performed -= RaycastExample;
    }

    private void RaycastExample(InputAction.CallbackContext _)
    {
        Vector2 _screenCenter = new(Screen.width * 0.5f, Screen.height * 0.5f);
        Ray _ray = m_cam.ScreenPointToRay(_screenCenter);

        if (Physics.Raycast(_ray, out RaycastHit hit, m_maxDistance, m_hittableMask, QueryTriggerInteraction.Ignore))
        {
            Debug.Log($"[CenterRaycastShooter] Hit {hit.collider.name} at {hit.point}");

            Renderer rend = hit.collider.GetComponent<Renderer>();
            if (rend) rend.material.color = Color.red;

            Debug.DrawLine(_ray.origin, hit.point, Color.green, 1.0f);
        }
        else
        {
            Debug.DrawLine(_ray.origin, _ray.direction * m_maxDistance, Color.yellow, 0.5f);
        }

    }

    void SphereCastExample()
    {
        float radius = 5.0f;
        float maxDistance = 10.0f;

        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;

        if (Physics.SphereCast(origin, radius, direction, out RaycastHit hit, maxDistance, m_hittableMask))
        {
            Debug.Log($"Sphere Hit {hit.collider.name}");
        }

        Renderer rend = hit.collider.GetComponent<Renderer>();
        if (rend) rend.material.color = Color.blue;

    }

    void OverlapSphereExample(Vector3 centerPosition)
    {
        Vector3 center = centerPosition;
        float radius = 5.0f;

        Collider[] hitColliders = Physics.OverlapSphere(center, radius);

        foreach(Collider hitCollider in hitColliders)
        {
            Debug.Log($"Detected : {hitCollider.name}");
        }

    }

    private Collider[] results = new Collider[10];

    void OptimizedOverlapSphereExample()
    {

        int count = Physics.OverlapSphereNonAlloc(transform.position, 5.0f, results);

        for (int i = 0; i < count; i++)
        {
            Debug.Log($"NonAlloc Hit : {results[i].name}");
        }

    }

}
