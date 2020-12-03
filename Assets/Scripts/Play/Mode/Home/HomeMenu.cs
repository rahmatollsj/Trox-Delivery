using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    //Author : Félix Bernier
    [Findable(Tags.Home)]
    public class HomeMenu : MonoBehaviour
    {
#if UNITY_EDITOR
        private void OnGUI(){
            if(GUI.Button(new Rect(0, 0, 100, 20), "Unlock all")){
                Finder.ProgressionManager.UnlockLevel(Scenes.Desert);
                Finder.ProgressionManager.UnlockLevel(Scenes.Jungle);
                Finder.ProgressionManager.UnlockLevel(Scenes.Moon);
                Finder.ProgressionManager.UnlockLevel(Scenes.Arctic);
                Finder.ProgressionManager.UnlockLevel(Scenes.Volcan);
            }
        }
#endif
        private Main main;

        private Button backButton;
        
        //Author: Seyed-Rahmatoll Javadi
        private Button playButton;
        private Button quitButton;
        public event HomeStateEvent OnHomeStateChanged;
        private HomeState state;
        
        public HomeState State
        {
            get => state;
            set
            {
                state = value;
                if (OnHomeStateChanged != null) OnHomeStateChanged();
            }
        }

        private void Awake()
        {
            main = Finder.Main;

            var buttons = GetComponentsInChildren<Button>();
            //Author: Félix Bernier
            backButton = buttons.WithName(GameObjects.BackToRoot);

            //Author: Seyed-Rahmatoll Javadi
            playButton = buttons.WithName(GameObjects.Play);
            quitButton = buttons.WithName(GameObjects.Quit);
        }

        // Author: David Pagotto
        private void Start()
        {
            State = HomeState.RootMenu;
        }

        private void OnEnable()
        {
            //Author: Félix Bernier
            if(backButton != null)
                backButton.onClick.AddListener(OnBackClicked);
            //Author: Seyed-Rahmatoll Javadi
            playButton.onClick.AddListener(OnPlayClick);
            quitButton.onClick.AddListener(QuitApplication);
        }

        private void OnBackClicked()
        {
            State = HomeState.RootMenu;
        }

        private void OnDisable()
        {
            //Author: Félix Bernier
            if(backButton != null)
                backButton.onClick.RemoveListener(OnBackClicked);
            //Author: Seyed-Rahmatoll Javadi
            playButton.onClick.RemoveListener(OnPlayClick);
            quitButton.onClick.RemoveListener(QuitApplication);
        }

        public void StartGame(string sceneName)
        {
            main.UnloadHome();
            main.LoadGame(sceneName);
        }

        // Author : François-Xavier Bernier
        private void OnPlayClick()
        {
            StartGame(Scenes.Tutorial);
        }

        private void QuitApplication()
        {
            ApplicationExtensions.Quit();
        }
    }
    public delegate void HomeStateEvent();
}