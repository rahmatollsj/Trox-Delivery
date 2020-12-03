using Harmony;
using UnityEngine;

namespace Game
{

    // Author: David Pagotto
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleTrigger : MonoBehaviour
    {
        [Tooltip("Only trigger the particles when the object we collided with has this tag.")]
        [SerializeField] private Tag requiredTag = Tags.VehicleBody;

        private new ParticleSystem particleSystem = null;

        private void Awake()
            => particleSystem = GetComponent<ParticleSystem>();

        private void OnTriggerEnter2D(Collider2D other)
            => ProcessCollision(other?.gameObject);

        private void OnCollisionEnter2D(Collision2D other)
            => ProcessCollision(other?.gameObject);

        private void ProcessCollision(GameObject other)
        {
            if ((other.CompareTag(requiredTag)) && other != null)
                particleSystem.Play();
        }
    }
}
