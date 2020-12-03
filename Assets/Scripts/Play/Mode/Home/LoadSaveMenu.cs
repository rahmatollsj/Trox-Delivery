using Harmony;
using UnityEngine;

namespace Game
{
    // Author: David Pagotto
    public class LoadSaveMenu : MonoBehaviour
    {
        private HomeMenu homeMenu;
        private ProgressionManager progressionManager;
        
        private void Awake()
        {
            homeMenu = Finder.HomeMenu;
            progressionManager = Finder.ProgressionManager;
            homeMenu.OnHomeStateChanged += OnHomeStateChanged;
        }

        private void OnDestroy()
        {
            homeMenu.OnHomeStateChanged -= OnHomeStateChanged;
        }

        private void OnHomeStateChanged()
        {
            gameObject.SetActive(homeMenu.State == HomeState.SaveMenu);
        }

        public void OnLoadSave(int level)
        {
            progressionManager.ChangeSaveFile((uint)level);
            homeMenu.State = HomeState.LevelsMenu;
        }
    }
}
