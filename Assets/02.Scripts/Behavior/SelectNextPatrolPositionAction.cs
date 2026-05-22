using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SelectNextPatrolPosition", story: "Selet next patrol position from [self] into [currentPatrolPosition] and [CurrentPatrolIndex]", category: "Action", id: "8c78a7c2cea071b67cac90c4fb7919b4")]
public partial class SelectNextPatrolPositionAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<Vector3> CurrentPatrolPosition;
    [SerializeReference] public BlackboardVariable<int> CurrentPatrolIndex;
    protected override Status OnUpdate()
    {
        if (Self?.Value == null)
        {
            return Status.Failure;
        }

        EnemyBehaviorBridge bridge = Self.Value.GetComponent<EnemyBehaviorBridge>();
        
        if (bridge == null || !bridge.HasPatrolPoints)
        {
            return Status.Failure;
        }

        CurrentPatrolPosition.Value = bridge.GetPatrolPosition(CurrentPatrolIndex.Value);
        CurrentPatrolIndex.Value = CurrentPatrolIndex.Value + 1;        
        
        return Status.Success;
    }

 
}

