using Harmony;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputActionRebindingExtensions;
using System.Linq;

namespace Game
{
    // Author: David Pagotto
    public class KeySelector : MonoBehaviour
    {
        [SerializeField] private InputActionReference inputActionReference;
        [SerializeField] private int bindingId;

        public event KeybindUpdatedEvent OnKeybindUpdated;

        private InputAction targetInputAction;

        private Button button;
        private TextMeshProUGUI text;
        private RebindingOperation rebindingOperation;

        private void Awake()
        {
            button = GetComponent<Button>();
            text = GetComponentInChildren<TextMeshProUGUI>();

            targetInputAction = Finder.Inputs.InputActions.First(a => a.id == inputActionReference.action.id);

            button.onClick.AddListener(OnRebindClicked);
            UpdateButtonText();
        }

        public void UpdateButtonText()
        {
            text.text = targetInputAction.GetBindingDisplayString(bindingId);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(OnRebindClicked);
        }

        private void ResetBindingOperation()
        {
            rebindingOperation.Dispose();
            rebindingOperation = null;
            button.enabled = true;
        }

        private void PerformRebinding()
        {
            // https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/manual/ActionBindings.html#runtime-rebinding
            targetInputAction.Disable();
            rebindingOperation = targetInputAction.PerformInteractiveRebinding(bindingId)
                .WithBindingGroup("Keyboard")
                .WithCancelingThrough("<Keyboard>/escape")
                .OnComplete(o =>
                {
                    ResetBindingOperation();
                    targetInputAction.Enable();
                    OnKeybindUpdated?.Invoke(targetInputAction, targetInputAction.bindings[bindingId]);
                    UpdateButtonText();
                }).OnCancel(o =>
                {
                    UpdateButtonText();
                    ResetBindingOperation();
                    targetInputAction.Enable();
                })
                .Start();
            text.text = "Press key";

        }

        private void OnRebindClicked()
        {
            button.enabled = false;
            PerformRebinding();
        }

        public delegate void KeybindUpdatedEvent(InputAction inputAction, InputBinding inputBinding);
    }
}
