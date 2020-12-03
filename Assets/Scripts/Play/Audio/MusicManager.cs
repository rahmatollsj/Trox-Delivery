using Harmony;
using UnityEngine;

namespace Game
{
    //Author: Seyed-Rahmatoll Javadi
    [Findable(Tags.Music)]
    public class MusicManager : MonoBehaviour
    {
        [Header("Music options")]
        [SerializeField] private float defaultVolume = 0.3f;

        [Header("Musics")]
        [SerializeField] private AudioClip homeMusic;
        [SerializeField] private AudioClip tutorialMusic;
        [SerializeField] private AudioClip constructionSiteMusic;
        [SerializeField] private AudioClip jungleMusic;
        [SerializeField] private AudioClip moonMusic;
        [SerializeField] private AudioClip arcticMusic;
        [SerializeField] private AudioClip volcanoMusic;

        private AudioSource audioSource;

        public float Volume => audioSource.volume;

        private void Awake()
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = true;

            var optionsManager = Finder.OptionsManager;
            var volume = optionsManager.ReadMusicVolume();

            if (!volume.HasValue)
            {
                optionsManager.WriteMusicVolume(defaultVolume);
                audioSource.volume = defaultVolume;
            }
            else
                audioSource.volume = volume.Value;

            Play(Scenes.Home);
        }

        public void Play(string scene)
        {
            switch (scene)
            {
                case Scenes.Home:
                    audioSource.clip = homeMusic;
                    break;
                case Scenes.Tutorial:
                    audioSource.clip = tutorialMusic;
                    break;
                case Scenes.Desert:
                    audioSource.clip = constructionSiteMusic;
                    break;
                case Scenes.Jungle:
                    audioSource.clip = jungleMusic;
                    break;
                case Scenes.Moon:
                    audioSource.clip = moonMusic;
                    break;
                case Scenes.Arctic:
                    audioSource.clip = arcticMusic;
                    break;
                case Scenes.Volcan:
                    audioSource.clip = volcanoMusic;
                    break;
            }
            
            audioSource.Play();
        }

        public void ChangeVolume(float volume)
        {
            if(volume >= 0 && volume <= 1)
                audioSource.volume = volume;
        }
    }
}