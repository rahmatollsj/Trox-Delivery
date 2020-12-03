using Harmony;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    // Author: Benoit Simon-Turgeon
    [Findable(Tags.MainController)]
    public class Inputs : MonoBehaviour
    {
        private OptionsManager optionsManager;
        public InputActions.GameActions Game => InputActions.Game;
        public InputActions InputActions { get; private set; }

        public bool UseInvertedRotation { get; set; }

        private void Awake()
        {
            InputActions = new InputActions();
            optionsManager = Finder.OptionsManager;
        }
        private void Start()
        {
            LoadCustomBindings();
        }

        // Author: David Pagotto
        private void LoadCustomBindings()
        {
            var inputActions = InputActions;
            foreach (var inputAction in inputActions)
            {
                // Chaque InputAction peut avoir plusieurs bindings
                for (int i = 0; i < inputAction.bindings.Count; i++)
                {
                    var binding = inputAction.bindings[i];
                    var savedBindingPath = optionsManager.ReadCustomBindingPath(binding);
                    // On charge le nouveau binding s'il y en avait un de sauvegardé
                    if (savedBindingPath != null)
                    {
                        inputAction.Disable();
                        inputAction.ApplyBindingOverride(i, savedBindingPath);
                        inputAction.Enable();
                    }
                }
            }
            UseInvertedRotation = optionsManager.ReadInvertedRotationEnabled();
        }
    }
}