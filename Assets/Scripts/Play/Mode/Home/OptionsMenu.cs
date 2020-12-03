using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    //Author: Seyed-Rahmatoll Javadi
    public class OptionsMenu : MonoBehaviour
    {
        [SerializeField] private GameObject controlsMenu = null;
        private MusicManager musicManager;
        private SoundEffectsManager soundEffectsManager;
        
        private Slider musicVolumeSlider;
        private Slider soundEffectVolumeSlider;
        private Button cancelButton;
        private Button saveButton;
        
        private HomeMenu homeMenu;

        private GameResumedEventChannel gameResumedEventChannel;

        // Author: David Pagotto
        private OptionsManager optionsManager;

        private void Awake()
        {
            musicManager = Finder.MusicManager;
            soundEffectsManager = Finder.SoundEffectsManager;
            optionsManager = Finder.OptionsManager;

            musicVolumeSlider = transform.Find(GameObjects.MusicSlider).GetComponent<Slider>();
            musicVolumeSlider.value = musicManager.Volume;
            soundEffectVolumeSlider = transform.Find(GameObjects.SoundEffectSlider).GetComponent<Slider>();
            soundEffectVolumeSlider.value = soundEffectsManager.Volume;
            
            var buttons = GetComponentsInChildren<Button>();
            cancelButton = buttons.WithName(GameObjects.Cancel);
            saveButton = buttons.WithName(GameObjects.Save);
            
            homeMenu = GetComponentInParent<HomeMenu>();
            if (homeMenu != null)
                homeMenu.OnHomeStateChanged += OnHomeStateChanged;
            else
                gameResumedEventChannel = Finder.GameResumedEventChannel;

        }

        private void OnEnable()
        {
            cancelButton.onClick.AddListener(Cancel);
            saveButton.onClick.AddListener(Save);
        }

        private void OnDisable()
        {
            cancelButton.onClick.RemoveListener(Cancel);
            saveButton.onClick.RemoveListener(Save);
        }

        private void OnDestroy()
        {
            if(homeMenu != null)
                homeMenu.OnHomeStateChanged -= OnHomeStateChanged;
        }

        private void OnHomeStateChanged()
        {
            gameObject.SetActive(homeMenu.State == HomeState.OptionsMenu);
        }

        private void Save()
        {
            float gameVolume = soundEffectVolumeSlider.value;
            float musicVolume = musicVolumeSlider.value;

            optionsManager.WriteMusicVolume(musicVolume);
            musicManager.ChangeVolume(musicVolume);

            optionsManager.WriteGameVolume(gameVolume);
            soundEffectsManager.ChangeVolume(gameVolume);

            Return();
        }

        private void Cancel()
        {
            musicVolumeSlider.value = musicManager.Volume;
            soundEffectVolumeSlider.value = soundEffectsManager.Volume;
            Return();
        }

        private void Return()
        {
            if (homeMenu != null)
            {
                homeMenu.State = HomeState.RootMenu;
            }
            else
            {
                gameResumedEventChannel.Publish();
                transform.parent.gameObject.SetActive(false);
            }
        }

        public void OnBtnControls()
        {
            if (homeMenu)
                homeMenu.State = HomeState.ControlsMenu;
            else
            {
                controlsMenu.gameObject.SetActive(true);
                gameObject.Parent().SetActive(false);
            }
        }
    }
}