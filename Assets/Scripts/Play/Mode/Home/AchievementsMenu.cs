using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    //Author : François-Xavier Bernier
    public class AchievementsMenu : MonoBehaviour
    {
        private HomeMenu homeMenu;
        private Button backButton;
        private GameResumedEventChannel gameResumedEventChannel;

        private void Awake()
        {
            homeMenu = GetComponentInParent<HomeMenu>();
            if (homeMenu != null)
            {
                homeMenu.OnHomeStateChanged += OnHomeStateChanged;
            }
            else
            {
                gameResumedEventChannel = Finder.GameResumedEventChannel;
            }

            var buttons = GetComponentsInChildren<Button>();
            backButton = buttons.WithName(GameObjects.BackAchvButton);
            Finder.AchievementsMenuOpenedEventChannel.Publish();
        }

        private void OnEnable()
        {
            backButton.onClick.AddListener(BackToRootMenu);
        }

        private void OnDisable()
        {
            backButton.onClick.RemoveListener(BackToRootMenu);
        }

        private void OnHomeStateChanged()
        {
            gameObject.SetActive(homeMenu.State == HomeState.AchievementsMenu);
        }

        private void BackToRootMenu()
        {
            Return();
        }

        private void OnDestroy()
        {
            homeMenu.OnHomeStateChanged -= OnHomeStateChanged;
        }

        private void Return()
        {
            if (homeMenu != null)
            {
                homeMenu.State = HomeState.RootMenu;
            }
            else
            {
                Time.timeScale = 1f;
                gameResumedEventChannel.Publish();
                transform.parent.gameObject.SetActive(false);
            }
        }
    }
}