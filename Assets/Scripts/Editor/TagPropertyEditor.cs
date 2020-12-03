using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using UnityEngine.UIElements;
using Harmony;

namespace Game
{
    // Author: David Pagotto
    // https://docs.unity3d.com/ScriptReference/PropertyDrawer.html
    [CustomPropertyDrawer(typeof(Tag))]
    public class TagPropertyDrawer : PropertyDrawer
    {
        private GUIContent tagString = new GUIContent("tag");
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // https://answers.unity.com/questions/57952/getting-the-tag-array.html
            var tags = UnityEditorInternal.InternalEditorUtility.tags;
            var nameProperty = property.FindPropertyRelative("name");
            if (string.IsNullOrEmpty(nameProperty.stringValue))
                nameProperty.stringValue = tags[0];

            string selectedTag = nameProperty.stringValue;
            int selectedTagIndex = 0;
            for (; selectedTagIndex < tags.Length; selectedTagIndex++)
                if (tags[selectedTagIndex] == selectedTag)
                    break;
            EditorGUI.BeginChangeCheck();
            // https://forum.unity.com/threads/solved-using-popup-property-drawer-for-enumerations-with-special-symbols.264945/
            int newTag = EditorGUI.Popup(EditorGUI.PrefixLabel(position, label), selectedTagIndex, tags);

            if (EditorGUI.EndChangeCheck())
                nameProperty.stringValue = tags[newTag];
        }
    }
}
