using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 在Inspector中显示中文变量名
/// </summary>
public class RenameInInspectorAttribute : PropertyAttribute
{
    public string Name { get; private set; }
    public RenameInInspectorAttribute(string name)
    {
        Name = name;
    }
}