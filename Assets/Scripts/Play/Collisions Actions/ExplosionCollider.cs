using Harmony;
using System.Collections;
using UnityEngine;

namespace Game
{
    // Author: David Pagotto
    [RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SpriteRenderer))]
    public class ExplosionCollider : MonoBehaviour
    {
        [SerializeField] private float boomDelay = 1.3f;
        [SerializeField] private Vector2 animationScale = new Vector2(0.25f, 0.25f);
        [SerializeField] private SoundEffectType explosionSoundEffect = SoundEffectType.Explosion;

        private new Rigidbody2D rigidbody;
        private Animator animator;
        private new Collider2D collider;
        private SoundEffectsManager soundEffectsManager;

        private void Awake()
        { 
            soundEffectsManager = Finder.SoundEffectsManager;
            rigidbody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            collider = GetComponent<Collider2D>();
            animator.enabled = false;
        }

        private IEnumerator WaitForExplosionRoutine()
        {
            yield return new WaitForSeconds(boomDelay);

            rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
            rigidbody.rotation = 0f;
            collider.enabled = false;
            transform.localScale = animationScale;

            animator.enabled = true;
            soundEffectsManager.Play(explosionSoundEffect);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.GetComponentInParent<Vehicle>())
                StartCoroutine(WaitForExplosionRoutine());
        }
    }
}