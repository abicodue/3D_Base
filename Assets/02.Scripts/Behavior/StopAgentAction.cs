using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Stop Agent", story: "Stop [Self] Agent", category: "Action/AI", id: "2fef012da57e7e9dbe06bfb63be8d9bd")]
public partial class StopAgentAction : Action
{

    [SerializeReference] public BlackboardVariable<GameObject> Self;

    protected override Status OnUpdate()
    {
        if (Self?.Value == null)
        {
            return Status.Failure;
        }

        NavMeshAgent agent = Self.Value.GetComponent<NavMeshAgent>();

        if (agent == null || !agent.isOnNavMesh)
        {
            return Status.Failure;
        }

        agent.isStopped = true;
        agent.ResetPath();
        return Status.Success;
    }

}

