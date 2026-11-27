using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TutorialRuntimeNode : GenericRuntimeNode
{
    public int stepIndex;
    public List<KeyCode> requiredKeys = new List<KeyCode>();
}