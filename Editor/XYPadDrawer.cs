using UnityEditor;
using UnityEngine;

namespace AuggoToolkit
{
    /// Paints fields tagged [XYPad]. See Runtime/XYPadAttribute.cs.
    [CustomPropertyDrawer(typeof(XYPadAttribute))]
    public class XYPadDrawer : PropertyDrawer
    {
        const float PadSize = 160f;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight + PadSize + 4f;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Vector2)
            {
                EditorGUI.LabelField(position, label.text, "[XYPad] only works on Vector2");
                return;
            }

            EditorGUI.LabelField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), label);

            Rect pad = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 2f, PadSize, PadSize);
            EditorGUI.DrawRect(pad, new Color(0.12f, 0.12f, 0.14f));

            // Dragging: y is flipped so "up" on screen is 1.
            Event e = Event.current;
            int id = GUIUtility.GetControlID(FocusType.Passive);
            if ((e.type == EventType.MouseDown && pad.Contains(e.mousePosition)) ||
                (e.type == EventType.MouseDrag && GUIUtility.hotControl == id))
            {
                GUIUtility.hotControl = id;
                property.vector2Value = new Vector2(
                    Mathf.Clamp01((e.mousePosition.x - pad.x) / pad.width),
                    Mathf.Clamp01(1f - (e.mousePosition.y - pad.y) / pad.height));
                e.Use();
            }
            else if (e.type == EventType.MouseUp && GUIUtility.hotControl == id)
            {
                GUIUtility.hotControl = 0;
                e.Use();
            }

            // Crosshair and dot at the current value.
            Vector2 v = property.vector2Value;
            float px = pad.x + v.x * pad.width, py = pad.y + (1f - v.y) * pad.height;
            Color line = new Color(1f, 1f, 1f, 0.15f);
            EditorGUI.DrawRect(new Rect(pad.x, py, pad.width, 1f), line);
            EditorGUI.DrawRect(new Rect(px, pad.y, 1f, pad.height), line);
            EditorGUI.DrawRect(new Rect(px - 4f, py - 4f, 8f, 8f), new Color(0.55f, 0.85f, 1f));

            // Optional axis hints.
            var tag = (XYPadAttribute)attribute;
            GUIStyle hint = new GUIStyle(EditorStyles.miniLabel) { normal = { textColor = new Color(1f, 1f, 1f, 0.4f) } };
            GUIStyle hintRight = new GUIStyle(hint) { alignment = TextAnchor.UpperRight };
            if (tag.Left != null) GUI.Label(new Rect(pad.x + 3f, pad.yMax - 15f, 70f, 14f), tag.Left, hint);
            if (tag.Right != null) GUI.Label(new Rect(pad.xMax - 73f, pad.yMax - 15f, 70f, 14f), tag.Right, hintRight);
            if (tag.Bottom != null) GUI.Label(new Rect(pad.x + 3f, pad.yMax - 28f, 80f, 14f), tag.Bottom, hint);
            if (tag.Top != null) GUI.Label(new Rect(pad.x + 3f, pad.y + 1f, 80f, 14f), tag.Top, hint);
        }
    }
}
