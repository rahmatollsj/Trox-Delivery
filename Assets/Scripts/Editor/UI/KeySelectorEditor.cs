using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    // https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/manual/ActionBindings.html#runtime-rebinding
    // Inspiré de l'exemple Unity "Rebinding UI"
    [CustomEditor(typeof(KeySelector))]
    public class KeySelectorEditor : Editor
    {
        private SerializedProperty inputActionReference = null;
        private SerializedProperty bindingId = null;
        private GUIContent[] availableBindings = null;
        private GUIContent selectedBindingString = new GUIContent("Selected binding");

        private bool IsInputActionReferenceNull => inputActionReference.objectReferenceValue == null;

        private void Awake()
        {
            inputActionReference = serializedObject.FindProperty("inputActionReference");
            bindingId = serializedObject.FindProperty("bindingId");
        }

        private void OnEnable()
        {
            RefreshBindingsPopup();
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.LabelField("Bindings");
            using (new EditorGUI.IndentLevelScope())
            {
                EditorGUILayout.PropertyField(inputActionReference);

                if (availableBindings != null)
                {
                    int newBinding = EditorGUILayout.Popup(selectedBindingString, bindingId.intValue, availableBindings);
                    if (newBinding != bindingId.intValue)
                        bindingId.intValue = newBinding;
                }
            }

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                RefreshBindingsPopup();
            }
        }

        private void RefreshBindingsPopup()
        {
            if (IsInputActionReferenceNull)
                return;
            var actionReference = (InputActionReference)inputActionReference.objectReferenceValue;
            var inputAction = actionReference.action;
            var flags = InputBinding.DisplayStringOptions.DontUseShortDisplayNames
                | InputBinding.DisplayStringOptions.IgnoreBindingOverrides
                | InputBinding.DisplayStringOptions.DontOmitDevice;

            availableBindings = inputAction.bindings.Select(b => new GUIContent(b.ToDisplayString(flags))).ToArray();
        }
    }
}
