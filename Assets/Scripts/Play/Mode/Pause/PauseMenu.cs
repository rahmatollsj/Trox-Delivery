using Harmony;
using UnityEngine;

namespace Game
{
    //Author: Félix Bernier
    [Findable(Tags.PauseCanvas)]
    public class PauseMenu : MonoBehaviour
    {
        //Author: Félix Bernier
        private GamePausedEventChannel gamePausedEventChannel;
        private GameResumedEventChannel gameResumedEventChannel;
        private TimeManagement timeManagement;
        
        //Author: Seyed-Rahmatoll Javadi
        private Main main;
        private GameObject optionsMenu;
        private BonusResetEventChannel bonusResetEventChannel;

        private void Awake()
        {
            //Author: Félix Bernier
            gamePausedEventChannel = Finder.GamePausedEventChannel;
            gamePausedEventChannel.OnGamePaused += OnGamePaused;
            gameResumedEventChannel = Finder.GameResumedEventChannel;

            main = Finder.Main;

            //Author: Seyed-Rahmatoll Javadi
            optionsMenu = GameObject.Find(GameObjects.OptionsMenu);
            optionsMenu.SetActive(false);

            bonusResetEventChannel = Finder.BonusResetEventChannel;
            timeManagement = Finder.TimeManagement;
        }
        
        // Author: Félix Bernier
        private void OnDestroy()
        {
            gamePausedEventChannel.OnGamePaused -= OnGamePaused;
        }
        
        private void Start()
        {
            //Author: Félix Bernier
            gameObject.SetActive(timeManagement.GameIsPaused);
        }

        private void Update()
        {
            //Author: Félix Bernier
            gameObject.SetActive(timeManagement.GameIsPaused);
        }
        
        //Author: Félix Bernier
        private void OnGamePaused()
        {
            gameObject.SetActive(true);
        }
        
        //Author: Félix Bernier
        public void Play()
        {
            //Author: Félix Bernier
            timeManagement.UnfreezeGame();
            gameResumedEventChannel.Publish();
            gameObject.SetActive(false);
            //Author: Seyed-Rahmatoll Javadi
            optionsMenu.SetActive(false);
        }

        public void Restart()
        {
            //Author: Seyed-Rahmatoll Javadi
            bonusResetEventChannel.Publish();
            // Author: David Pagotto
            Finder.LevelManager.RestartCurrentLevel();
            Finder.LevelAbandonEventChannel.Publish();
            //Author: François-Xavier Bernier
            Play();
        }
        
        //Author: Félix Bernier
        public void Quit()
        {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
        
        //Author: Seyed-Rahmatoll Javadi
        public void ReturnToRootMenu()
        {
            bonusResetEventChannel.Publish();
            timeManagement.UnfreezeGame();
            gameObject.SetActive(false);
            main.UnloadGame();
            Finder.LevelAbandonEventChannel.Publish();
            Finder.LevelManager.UnloadCurrentLevel();
            Finder.MusicManager.Play(Scenes.Home);
            main.LoadHome();
        }
        
        //Author: Seyed-Rahmatoll Javadi
        public void ShowOptionsMenu()
        {
            gameObject.SetActive(false);
            optionsMenu.SetActive(true);
        }
    }
}