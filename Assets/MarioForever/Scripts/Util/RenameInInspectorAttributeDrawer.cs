using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace SweetMoleHouse.MarioForever.Scripts.Util
{
    [CustomPropertyDrawer(typeof(RenameInInspectorAttribute))]
    public class RenameInInspectorAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            RenameInInspectorAttribute attr = attribute as RenameInInspectorAttribute;
            if (attr.Name.Length > 0)
            {
                label.text = attr.Name;
            }
            EditorGUI.PropertyField(position, property, label);
            //base.OnGUI(position, property, label);
        }
    }
}
#endif