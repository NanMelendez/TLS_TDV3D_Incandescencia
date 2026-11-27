using System;

[Serializable]
public class TutorialNode : GenericNode
{
    public static readonly string STEP_INDEX_PORT_NAME = "StepIndex";
    public static readonly string REQUIRED_KEYS_PORT_NAME = "RequiredKeys";

    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context.AddInputPort(INPUT_PORT_NAME).Build();
        context.AddInputPort<int>(STEP_INDEX_PORT_NAME).Build();
        context.AddInputPort<string>(REQUIRED_KEYS_PORT_NAME).Build();
        context.AddOutputPort(OUTPUT_PORT_NAME).Build();
    }
}