using UnityEditor;
using Unity.GraphToolkit.Editor;
using System;

[Graph(AssetExtension)]
[Serializable]
public class GenericGraph : Graph
{
    public const string AssetExtension = "graph";

    [MenuItem("Assets/Create/Graph/Generic Graph")]
    static void CreateAssetField()
    {
        GraphDatabase.PromptInProjectBrowserToCreateNewAsset<GenericGraph>();
    }
}