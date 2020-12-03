using Harmony;
using UnityEngine;

namespace Game
{
    // Author: David Pagotto
    public class Tower : MonoBehaviour
    {
        [Header("Game Objects")]
        [SerializeField] private GameObject explosionObject;
        [SerializeField] private Rigidbody2D rbTowerTop;
        [SerializeField] private Collider2D killTriggerCollider;

        [Header("Settings")]
        [SerializeField] private float fallForce = 15f;

        public bool HasTouchedDown { get; set; } = false;

        private SoundEffectsManager soundEffectManager;

        private void Awake()
        {
            soundEffectManager = Finder.SoundEffectsManager;
        }

        private void Update()
        {
            if (HasTouchedDown)
            {
                // Pour désactiver le kill trigger
                killTriggerCollider.enabled = false;
                enabled = false;
            }
        }

        public void Explode()
        {
            soundEffectManager.Play(SoundEffectType.Explosion);
            rbTowerTop.constraints = RigidbodyConstraints2D.None;
            rbTowerTop.AddForce(new Vector2(-fallForce, 0), ForceMode2D.Impulse);
            explosionObject.SetActive(true);
        }
    }
}
