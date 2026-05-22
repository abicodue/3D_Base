using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Set Last Attack Time", story: "Set [LastAttackTime] to current time", category: "Action/AI", id: "ea6ff71cf5118f8512c822a2ab4788f6")]
public partial class SetLastAttackTimeAction : Action
{
    [SerializeReference] public BlackboardVariable<float> LastAttackTime;

    protected override Status OnUpdate()
    {
        LastAttackTime.Value = Time.time;
        return Status.Success;
    }

}
