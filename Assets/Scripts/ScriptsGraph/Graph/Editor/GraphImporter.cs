using System;
using System.Linq;
using System.Collections.Generic;
using Unity.GraphToolkit.Editor;
using UnityEditor.AssetImporters;
using UnityEngine;

[ScriptedImporter(1, GenericGraph.AssetExtension)]
public class GraphImporter : ScriptedImporter
{
    public override void OnImportAsset(AssetImportContext ctx)
    {
        GenericGraph graph = GraphDatabase.LoadGraphForImporter<GenericGraph>(ctx.assetPath);
        StartNode startNode = graph.GetNodes().OfType<StartNode>().FirstOrDefault();
        GenericRuntimeGraph runtimeGraph = ScriptableObject.CreateInstance<GenericRuntimeGraph>();
        INode nextNode = GetNextNode(startNode);
        while (nextNode != null)
        {
            List<GenericRuntimeNode> nodes = ConvertNodes(nextNode);
            runtimeGraph.nodes.AddRange(nodes);
            nextNode = GetNextNode(nextNode);
        }
        ctx.AddObjectToAsset("RuntimeGraph", runtimeGraph);
        ctx.SetMainObject(runtimeGraph);
    }

    static INode GetNextNode(INode node)
    {
        if (node == null)
            return null;
        IPort outputPort = node.GetOutputPortByName(GenericNode.OUTPUT_PORT_NAME);
        if (outputPort == null || !outputPort.isConnected)
            return null;
        IPort nextNodePort = outputPort.firstConnectedPort;
        if (nextNodePort == null)
            return null;
        return nextNodePort.GetNode();
    }

    static List<GenericRuntimeNode> ConvertNodes(INode node)
    {
        List<GenericRuntimeNode> runtimeNodes = new List<GenericRuntimeNode>();
        switch (node)
        {
            case TextNode textNode:
                runtimeNodes.Add(new TextRuntimeNode
                {
                    text = GetInputPortValue<string>(textNode.GetInputPortByName(TextNode.TEXT_PORT_NAME)),
                    waitTime = GetInputPortValue<int>(textNode.GetInputPortByName(TextNode.WAITTIME_PORT_NAME))
                });
                break;
            case ImageNode imageNode:
                runtimeNodes.Add(new ImageRuntimeNode
                {
                    image = GetInputPortValue<Sprite>(imageNode.GetInputPortByName(ImageNode.IMAGE_PORT_NAME))
                });
                break;
            case TutorialNode tutorialNode:
                string keys = GetInputPortValue<string>(tutorialNode.GetInputPortByName(TutorialNode.REQUIRED_KEYS_PORT_NAME));
                runtimeNodes.Add(new TutorialRuntimeNode
                {
                    stepIndex = GetInputPortValue<int>(tutorialNode.GetInputPortByName(TutorialNode.STEP_INDEX_PORT_NAME)),
                    requiredKeys = ParseRequiredKeys(keys)
                });
                break;
            case WaitForTriggerNode triggerNode:
                runtimeNodes.Add(new WaitForTriggerRuntimeNode
                {
                    colliderTag = GetInputPortValue<string>(triggerNode.GetInputPortByName(WaitForTriggerNode.COLLIDER_TAG_PORT_NAME))
                });
                break;
        }
        return runtimeNodes;
    }
    static List<KeyCode> ParseRequiredKeys(string keys)
    {
        List<KeyCode> result = new List<KeyCode>();

        if (string.IsNullOrWhiteSpace(keys))
            return result;
        string[] keyNames = keys.Split(',');
        foreach (string keyName in keyNames)
        {
            string trimmedKey = keyName.Trim();
            if (Enum.TryParse(trimmedKey, true, out KeyCode key) && Enum.IsDefined(typeof(KeyCode), key))
            {
                if (!result.Contains(key))
                    result.Add(key);
            }
            else
            {
                Debug.LogWarning($"GraphImporter: Invalid KeyCode '{trimmedKey}'.");
            }
        }
        return result;
    }

    static T GetInputPortValue<T>(IPort port)
    {
        T value = default;
        if (port == null)
            return value;
        if (port.isConnected)
        {
            switch (port.firstConnectedPort.GetNode())
            {
                case IVariableNode variableNode:
                    variableNode.variable.TryGetDefaultValue<T>(out value);
                    return value;

                case IConstantNode constantNode:
                    constantNode.TryGetValue<T>(out value);
                    return value;
            }
        }
        else
        {
            port.TryGetValue<T>(out value);
        }
        return value;
    }
}