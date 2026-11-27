using System;

[Serializable]
public class TextNode : GenericNode
{
    public static readonly string TEXT_PORT_NAME = "Text";
    public static readonly string WAITTIME_PORT_NAME = "WaitTime";
    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context.AddInputPort(INPUT_PORT_NAME).Build();
        context.AddInputPort<string>(TEXT_PORT_NAME).Build();
        context.AddInputPort<int>(WAITTIME_PORT_NAME).Build();
        context.AddOutputPort(OUTPUT_PORT_NAME).Build();
    }
}