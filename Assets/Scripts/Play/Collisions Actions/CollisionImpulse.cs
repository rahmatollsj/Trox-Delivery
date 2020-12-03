using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Game
{
    // Author: David Pagotto
    [RequireComponent(typeof(Rigidbody2D))]
    public class CollisionImpulse : MonoBehaviour
    {
        [Header("Gameplay")]
        [Tooltip("Object will self destruct after it detected a collision")]
        [SerializeField] private bool enableSelfDestruct = true;

        [Tooltip("Object will only collide once with the player.")]
        [SerializeField] private bool onlyCollideOnceWithPlayer;

        [Tooltip("Delay before the object fades out and get destroyed")]
        [SerializeField] private float destroyStartTime = 6f;

        [Tooltip("Force applied to the object on collision")]
        [SerializeField] private Vector2 collisionForce = new Vector2(50, 50);

        [Header("Visual")]
        [Tooltip("Temps (secondes) du \"fade-out\" avant la destruction de l'objet")]
        [SerializeField] private float fadeOutTime = 1f;

        private new Rigidbody2D rigidbody;
        private SpriteRenderer spriteRenderer;
        private new Collider2D collider;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            collider = GetComponent<Collider2D>();

        }

        private IEnumerator SelfDestructionRoutine()
        {
            yield return new WaitForSeconds(destroyStartTime);
            rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
            collider.enabled = false;
            yield return spriteRenderer.DOFade(0, fadeOutTime).WaitForCompletion();
            gameObject.SetActive(false);
        }

        private void OnCollisionEnter2D(Collision2D collider)
        {
            if (collider.gameObject.GetComponentInParent<Vehicle>())
            {
                rigidbody.AddForce(collisionForce, ForceMode2D.Impulse);
                rigidbody.constraints = RigidbodyConstraints2D.None;

                if(onlyCollideOnceWithPlayer)
                    Physics2D.IgnoreCollision(collider.gameObject.GetComponentInParent<Collider2D>(), this.collider, true);

                if (enableSelfDestruct)
                    StartCoroutine(SelfDestructionRoutine());
            }
        }
    }
}