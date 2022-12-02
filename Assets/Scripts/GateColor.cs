using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace SF
{
    public enum GateColor
    {
        Red, Green, Yellow
    }

    public static class GateColorExtensions
    {
        public static Color ToColor(this GateColor color)
        {
            switch (color)
            {
                case GateColor.Red:
                    return GateColorData.Instance.red;
                case GateColor.Green:
                    return GateColorData.Instance.green;
                case GateColor.Yellow:
                    return GateColorData.Instance.yellow;
                default:
                    return Color.white;
            }
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(GateColor))]
    public class GateColorDrawer : PropertyDrawer
    {
        const float Padding = 4f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            var width = position.height;
            var rect = new Rect(position.x, position.y, position.width - width - Padding, position.height);
            var colorRect = new Rect(position.x + position.width - width, position.y, width, position.height);

            EditorGUI.PropertyField(rect, property, GUIContent.none);
            m039.Common.GUIExtensions.DrawRect(colorRect, ((GateColor)property.intValue).ToColor());;

            EditorGUI.EndProperty();
        }
    }
#endif
}
