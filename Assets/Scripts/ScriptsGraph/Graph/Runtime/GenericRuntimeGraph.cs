using System.Collections.Generic;
using UnityEngine;

public class GenericRuntimeGraph : ScriptableObject
{
    [SerializeReference] public List<GenericRuntimeNode> nodes = new List<GenericRuntimeNode>();
}