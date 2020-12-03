using Harmony;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Linq;

namespace Game
{
    // Author: David Pagotto
    public class ControlsMenu : MonoBehaviour
    {
        [SerializeField] private GameObject optionsMenu;
        private HomeMenu homeMenu;
        private Toggle invertedRotationToggle;
        private OptionsManager optionsManager;
        private Inputs inputs;
        private KeySelector[] keySelectors;

        private List<KeyValuePair<InputAction, InputBinding>> updatedBindings = new List<KeyValuePair<InputAction, InputBinding>>();
        private bool previousInvertedRotation = false;
        private bool isInitialized = false;

        private void Awake()
        {
            homeMenu = Finder.HomeMenu;
            optionsManager = Finder.OptionsManager;
            inputs = Finder.Inputs;
            invertedRotationToggle = transform.Find(GameObjects.Toggle).GetComponent<Toggle>();

            // On détecte tous les KeySelectors enfant
            keySelectors = transform.Children()
                .Select(o => o.GetComponent<KeySelector>())
                .Where(o => o).ToArray();

            foreach (var keySelector in keySelectors)
                keySelector.OnKeybindUpdated += OnBindingChanged;

            if (homeMenu)
                homeMenu.OnHomeStateChanged += OnHomeStateChanged;
        }

        private void OnEnable()
        {
            if(!homeMenu && !isInitialized)
            {
                isInitialized = true;
                gameObject.Parent().SetActive(false);
            }
        }

        private void OnDestroy()
        {
            foreach (var keySelector in keySelectors)
                keySelector.OnKeybindUpdated -= OnBindingChanged;

            if (homeMenu)
                homeMenu.OnHomeStateChanged -= OnHomeStateChanged;
        }

        private void RefreshUI()
        {
            previousInvertedRotation = inputs.UseInvertedRotation;
            invertedRotationToggle.isOn = previousInvertedRotation;
            foreach (var keySelector in keySelectors)
                keySelector.UpdateButtonText();
        }

        private void OnHomeStateChanged()
        {
            if (homeMenu.State == HomeState.ControlsMenu)
            {
                RefreshUI();
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        private void OnBindingChanged(InputAction inputAction, InputBinding binding)
        {
            updatedBindings.Add(new KeyValuePair<InputAction, InputBinding>(inputAction, binding));
        }

        private void ReturnToPreviousMenu()
        {
            if (homeMenu)
                homeMenu.State = HomeState.OptionsMenu;
            else
            {
                optionsMenu.SetActive(true);
                gameObject.Parent().SetActive(false);
            }
        }

        public void OnBtnSave()
        {
            // On sauvegarde les touches changées
            inputs.UseInvertedRotation = invertedRotationToggle.isOn;
            optionsManager.WriteInvertedRotationEnabled(inputs.UseInvertedRotation);
            foreach (var bind in updatedBindings)
                optionsManager.WriteCustomBindingPath(bind.Value);

            updatedBindings.Clear();
            ReturnToPreviousMenu();
        }

        public void OnBtnBack()
        {
            // On re-applique l'ancienne rotation
            inputs.UseInvertedRotation = previousInvertedRotation;
            invertedRotationToggle.isOn = previousInvertedRotation;
            // Si on ne sauvegarde pas les changements, on ré-applique les anciennes touches
            foreach (var bind in updatedBindings)
            {
                var inputAction = bind.Key;
                inputAction.Disable();
                inputAction.RemoveBindingOverride(bind.Value);
                inputAction.Enable();
            }

            RefreshUI();
            ReturnToPreviousMenu();
        }
    }
}