using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    [Findable(Tags.Button)]
    public class ButtonAudio : MonoBehaviour
    {
        private SoundEffectsManager soundEffectManager;
        private AudioSource audioSource;
        private Button button;

        private void Awake()
        {
            soundEffectManager = Finder.SoundEffectsManager;
            
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = false;
            audioSource.volume = soundEffectManager.Volume;

            button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            button.onClick.AddListener(Play);
        }

        private void OnDisable()
        {
            button.onClick.RemoveListener(Play);
        }

        private void Play()
        {
            soundEffectManager.Play(SoundEffectType.Button);
        }
    }
}