using Harmony;
using UnityEngine;

namespace Game
{
    // Author: Seyed-Rahmatoll Javadi
    public class RootMenu : MonoBehaviour
    {
        private HomeMenu homeMenu;

        private void Awake()
        {
            homeMenu = Finder.HomeMenu;
            homeMenu.OnHomeStateChanged += OnHomeStateChanged;
        }

        private void OnDestroy()
        {
            homeMenu.OnHomeStateChanged += OnHomeStateChanged;
        }

        private void OnHomeStateChanged()
        {
            gameObject.SetActive(homeMenu.State == HomeState.RootMenu);
        }

        public void OnBtnLoad()
        {
            homeMenu.State = HomeState.SaveMenu;
        }

        public void OnBtnOptions()
        {
            homeMenu.State = HomeState.OptionsMenu;
        }

        //Author : François-Xavier Bernier
        public void OnBtnAchievements()
        {
            homeMenu.State = HomeState.AchievementsMenu;
        }
    }
}