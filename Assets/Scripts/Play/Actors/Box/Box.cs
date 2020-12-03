using System.Collections;
using DG.Tweening;
using Harmony;
using UnityEngine;

namespace Game
{
    //Author : François-Xavier Bernier
    [Findable(Tags.Box)]
    public class Box : MonoBehaviour
    {
        [SerializeField] private Vector2 boxScale;
        [SerializeField] private float mass = 0.2f;
        // Author: Benoit Simon-Turgeon
        [SerializeField] private float despawnTime = 2f;
        [Tooltip("Box will disappear after some time if it touches an object with any of these tags")] 
        [SerializeField] private LayerMask groundTags;
        [SerializeField] private float fadeOutDuration = 1f;
        
        private Rigidbody2D rigidBody;
        private SpriteRenderer boxSprite;
        private IEnumerator boxTouchingGround;
        
        // Author: Benoit Simon-Turgeon 
        public bool IsBoxOut { get; set; } = true;
        private bool IsBoxRemoverCountdownActive { get; set; } = false;
        
        //Author : François-Xavier Bernier
        private void Awake()
        {
            boxSprite = GetComponent<SpriteRenderer>();
            rigidBody = GetComponent<Rigidbody2D>();
            rigidBody.mass = mass;

            gameObject.transform.localScale += (Vector3) boxScale;
            boxTouchingGround = OnBoxTouchingGround();
        }
        
        // Author: Benoit Simon-Turgeon 
        private void FixedUpdate()
        {
            if (IsBoxRemoverCountdownActive)
            {
                if (!IsBoxOut || !rigidBody.IsTouchingLayers(groundTags))
                {
                    IsBoxRemoverCountdownActive = false;
                    StopCoroutine(boxTouchingGround);
                    boxTouchingGround = OnBoxTouchingGround();
                }
            }
            else if (IsBoxOut && rigidBody.IsTouchingLayers((groundTags)))
            {
                IsBoxRemoverCountdownActive = true;
                StartCoroutine(boxTouchingGround);
            }
        }
        
        // Author: Benoit Simon-Turgeon 
        private IEnumerator OnBoxTouchingGround()
        {
            yield return new WaitForSeconds(despawnTime);
            StartCoroutine(FadeOut());
            IsBoxRemoverCountdownActive = false;
        }
        
        // Author: Benoit Simon-Turgeon 
        private IEnumerator FadeOut()
        {
            yield return boxSprite.DOFade(0f, fadeOutDuration).WaitForCompletion();
            gameObject.SetActive(false);
        }
    }
}