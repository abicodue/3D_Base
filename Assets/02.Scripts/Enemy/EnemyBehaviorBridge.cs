using UnityEngine;

//SO -> 'Runtime' 같은 애인데, 지금은 EnemyController : MonoBehaviour 를 안만들고 그냥 여기에 MonoBehaviour를 붙여서 쓴다.
public class EnemyBehaviorBridge : MonoBehaviour
{
    [SerializeField] private EnemyConfigSO config;
    [SerializeField] private Transform[] patrolPoints;
    public EnemyConfigSO Config => config;
    public Transform[] PatrolPoints => patrolPoints;
    public bool HasConfig => config != null;
    public bool HasPatrolPoints => patrolPoints != null && patrolPoints.Length > 0;
    public Vector3 GetPatrolPosition(int index)

    {
        if (!HasPatrolPoints)
            return transform.position;
        int safeIndex = Mathf.Abs(index) % patrolPoints.Length;
        Transform point = patrolPoints[safeIndex];
        return point != null ? point.position : transform.position;
    }
}
