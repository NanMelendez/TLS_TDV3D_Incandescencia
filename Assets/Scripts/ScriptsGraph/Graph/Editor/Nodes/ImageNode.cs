using System;
using UnityEngine;

[Serializable]
public class ImageNode : GenericNode
{
    public static readonly string IMAGE_PORT_NAME = "Image";

    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context.AddInputPort(INPUT_PORT_NAME).Build();
        context.AddInputPort<Sprite>(IMAGE_PORT_NAME).Build();
        context.AddOutputPort(OUTPUT_PORT_NAME).Build();
    }
}