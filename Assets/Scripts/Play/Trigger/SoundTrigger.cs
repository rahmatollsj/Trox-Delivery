using Harmony;
using UnityEngine;

namespace Game
{

    // Author: David Pagotto
    public class SoundTrigger : MonoBehaviour
    {
        [SerializeField] private SoundEffectType soundType;
        [Tooltip("Only trigger the sound when the object we collided with has this tag.")]
        [SerializeField] private Tag requiredTag = Tags.VehicleBody;

        private SoundEffectsManager soundEffectsManager;

        private void Awake()
            => soundEffectsManager = Finder.SoundEffectsManager;

        private void OnTriggerEnter2D(Collider2D other)
            => ProcessCollision(other?.gameObject);

        private void OnCollisionEnter2D(Collision2D other)
            => ProcessCollision(other?.gameObject);

        private void ProcessCollision(GameObject other)
        {
            if ((other.CompareTag(requiredTag)) && other != null)
                soundEffectsManager.Play(soundType);
        }
    }
}
