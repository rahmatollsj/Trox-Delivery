using Harmony;
using UnityEngine;

namespace Game
{
    //Author: Seyed-Rahmatoll Javadi
    [Findable(Tags.Vehicle)]
    public class VehicleAudio : MonoBehaviour
    {
        [SerializeField] private AudioClip engineSoundEffect;
        [SerializeField] private AudioClip brakeSoundEffect;

        public const float DefaultEngineSoundPitch = 0.5f;
        public const float SpeedRequiredToPlayBrakeSoundEffect = 0.15f;

        private AudioSource engineAudioSource;
        private AudioSource brakeAudioSource;

        private GamePausedEventChannel gamePausedEventChannel;
        private GameResumedEventChannel gameResumedEventChannel;
        private LevelSuccessEventChannel levelSuccessEventChannel;
        private LevelFailedEventChannel levelFailedEventChannel;

        public float Volume
        {
            set
            {
                engineAudioSource.volume = value;
                brakeAudioSource.volume = value;
            }
        }
        
        private void Awake()
        {
            engineAudioSource = gameObject.AddComponent<AudioSource>();
            engineAudioSource.loop = true;
            engineAudioSource.pitch = DefaultEngineSoundPitch;
            engineAudioSource.clip = engineSoundEffect;
            
            brakeAudioSource = gameObject.AddComponent<AudioSource>();
            brakeAudioSource.loop = false;

            gamePausedEventChannel = Finder.GamePausedEventChannel;
            gamePausedEventChannel.OnGamePaused += OnGamePaused;
            
            gameResumedEventChannel = Finder.GameResumedEventChannel;
            gameResumedEventChannel.OnGameResumed += OnGameResumed;
            
            levelSuccessEventChannel = Finder.LevelSuccessEventChannel;
            levelSuccessEventChannel.OnLevelSuccess += OnLevelFinished;
            
            levelFailedEventChannel = Finder.LevelFailedEventChannel;
            levelFailedEventChannel.OnLevelFailedEventChannel += OnLevelFinished;
        }

        private void OnDestroy()
        {
            gamePausedEventChannel.OnGamePaused -= OnGamePaused;
            gameResumedEventChannel.OnGameResumed -= OnGameResumed;
            levelSuccessEventChannel.OnLevelSuccess -= OnLevelFinished;
            levelFailedEventChannel.OnLevelFailedEventChannel -= OnLevelFinished;
        }

        public void PlayEngineSoundEffect()
        {
            if (!engineAudioSource.isPlaying)
            {
                engineAudioSource.Play();
            }
        }

        public void PlayBrakeSoundEffect(float currentSpeed, float maximumSpeed)
        {
            if (currentSpeed / maximumSpeed >= SpeedRequiredToPlayBrakeSoundEffect && !brakeAudioSource.isPlaying)
            {
                brakeAudioSource.PlayOneShot(brakeSoundEffect);
            }
        }
        
        public void ChangeEngineSoundEffectPitch(float pitch)
        {
            engineAudioSource.pitch = DefaultEngineSoundPitch + pitch;
        }

        private void Pause()
        {
            engineAudioSource.Pause();
            brakeAudioSource.Pause();
        }

        private void UnPause()
        {
            engineAudioSource.UnPause();
            brakeAudioSource.UnPause();
        }

        private void OnGamePaused()
        {
            Pause();
        }

        private void OnGameResumed()
        {
            UnPause();
        }

        private void OnLevelFinished()
        {
            Pause();
        }
    }
}