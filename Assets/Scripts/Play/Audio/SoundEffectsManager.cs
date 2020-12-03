using Harmony;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    //Author: Seyed-Rahmatoll Javadi
    [Findable(Tags.SoundEffect)]
    public class SoundEffectsManager : MonoBehaviour
    {
        [SerializeField] private float defaultVolume = 0.5f;
        [SerializeField] private AudioClip explosionSoundEffect;
        [SerializeField] private AudioClip metalSoundEffect;
        [SerializeField] private AudioClip bonusSoundEffect;
        [SerializeField] private AudioClip achievementSoundEffect;
        [SerializeField] private AudioClip buttonSoundEffect;
        // Author: David Pagotto
        [SerializeField] private AudioClip[] bushSoundEffects;
        // Author: François-Xavier Bernier
        [SerializeField] private AudioClip volcanoRockExplosionEffect;
        [SerializeField] private AudioClip volcanoRockDownFallEffect;
        // Author: Benoit Simon-Turgeon
        [SerializeField] private AudioClip laserSoundEffect;

        //Author: Seyed-Rahmatoll Javadi
        private AudioSource audioSource;
        private VehicleAudio vehicleAudio;

        private LevelLoadedEventChannel levelLoadedEventChannel;
        // Author: David Pagotto
        private LevelSuccessEventChannel levelSuccessEventChannel;
        private LevelFailedEventChannel levelFailedEventChannel;
        private GamePausedEventChannel gamePausedEventChannel;
        private GameResumedEventChannel gameResumedEventChannel;
        private LevelAbandonEventChannel levelAbandonEventChannel;


        public float Volume => audioSource.volume;

        private void Awake()
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = false;

            var optionsManager = Finder.OptionsManager;
            var volume = optionsManager.ReadGameVolume();

            if (!volume.HasValue)
            {
                optionsManager.WriteGameVolume(defaultVolume);
                audioSource.volume = defaultVolume;
            }
            else
                audioSource.volume = volume.Value;

            levelLoadedEventChannel = Finder.LevelLoadedEventChannel;
            levelLoadedEventChannel.OnLevelLoaded += OnLevelLoaded;

            levelSuccessEventChannel = Finder.LevelSuccessEventChannel;
            levelFailedEventChannel = Finder.LevelFailedEventChannel;
            levelSuccessEventChannel.OnLevelSuccess += OnLevelEnded;
            levelFailedEventChannel.OnLevelFailedEventChannel += OnLevelEnded;
        }

        private void OnDestroy()
        {
            levelLoadedEventChannel.OnLevelLoaded -= OnLevelLoaded;
            levelSuccessEventChannel.OnLevelSuccess -= OnLevelEnded;
            levelFailedEventChannel.OnLevelFailedEventChannel -= OnLevelEnded;

        }

        public void Play(SoundEffectType soundEffectType)
        {
            switch (soundEffectType)
            {
                case SoundEffectType.Explosion:
                    audioSource.PlayOneShot(explosionSoundEffect, Volume);
                    break;
                case SoundEffectType.Bonus:
                    audioSource.PlayOneShot(bonusSoundEffect, Volume);
                    break;
                case SoundEffectType.Button:
                    audioSource.PlayOneShot(buttonSoundEffect, Volume);
                    break;
                // Author: David Pagotto{
                case SoundEffectType.MetalCollision:
                    audioSource.PlayOneShot(metalSoundEffect, Volume);
                    break;
                //}
                // Author: François-Xavier Bernier{
                case SoundEffectType.VolcanoDownFall:
                    audioSource.PlayOneShot(volcanoRockDownFallEffect, Volume);
                    break;
                case SoundEffectType.VolcanoRockExplosion:
                    audioSource.PlayOneShot(volcanoRockExplosionEffect, Volume);
                    break;
                //}
                case SoundEffectType.Achievement:
                    audioSource.PlayOneShot(achievementSoundEffect, Volume);
                    break;
                case SoundEffectType.Bush:
                    audioSource.PlayOneShot(bushSoundEffects.Random(), Volume);
                    break;
                // Author: Benoit Simon-Turgeon{
                case SoundEffectType.Laser:
                    audioSource.PlayOneShot(laserSoundEffect, Volume);
                    break;
                //}
            }
        }
        
        public void ChangeVolume(float volume)
        {
            if (volume >= 0 && volume <= 1)
            {
                audioSource.volume = volume;
                if(vehicleAudio != null)
                    vehicleAudio.Volume = volume;
            }
        }

        private void OnLevelLoaded()
        {
            vehicleAudio = Finder.VehicleAudio;
            vehicleAudio.Volume = Volume;
            // Author: David Pagotto
            gamePausedEventChannel = Finder.GamePausedEventChannel;
            gameResumedEventChannel = Finder.GameResumedEventChannel;
            levelAbandonEventChannel = Finder.LevelAbandonEventChannel;
            levelAbandonEventChannel.OnLevelAbandon += OnLevelEnded;
            gamePausedEventChannel.OnGamePaused += audioSource.Pause;
            gameResumedEventChannel.OnGameResumed += audioSource.UnPause;
        }

        // Author: David Pagotto
        private void OnLevelEnded()
        {
            audioSource.Stop();
            gamePausedEventChannel.OnGamePaused -= audioSource.Pause;
            gameResumedEventChannel.OnGameResumed -= audioSource.UnPause;
            levelAbandonEventChannel.OnLevelAbandon -= OnLevelEnded;
        }
    }
}