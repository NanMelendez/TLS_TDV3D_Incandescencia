using System;
using UnityEngine;

[Serializable]
public class WaitForTriggerNode : GenericNode
{
    public static readonly string COLLIDER_TAG_PORT_NAME = "ColliderTag";
    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context.AddInputPort(INPUT_PORT_NAME).Build();
        context.AddInputPort<string>(COLLIDER_TAG_PORT_NAME).Build();
        context.AddOutputPort(OUTPUT_PORT_NAME).Build();
    }
}