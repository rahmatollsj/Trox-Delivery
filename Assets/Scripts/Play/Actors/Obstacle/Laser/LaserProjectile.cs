using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Game
{
    // Author: Benoit Simon-Turgeon
    public class LaserProjectile : MonoBehaviour
    {
        
        [SerializeField] private LayerMask ground;
        [SerializeField] private SpriteRenderer sprite;
        
        private new Rigidbody2D rigidbody;
        private Vector3 direction;
        
        private float speed;
        private bool isDying = false;

        public void LaserSpawnSetup(Vector3 laserDirection, float laserSpeed, float lifetime)
        {
            direction = laserDirection;
            speed = laserSpeed;
            
            rigidbody = GetComponent<Rigidbody2D>();

            transform.eulerAngles = new Vector3(0, 0, -Vector3.Angle(Vector3.right, direction));
            Destroy(gameObject, lifetime);
        }

        private void FixedUpdate()
        {
            if(!isDying)
                transform.position += direction * (speed * Time.fixedDeltaTime);
        }

        private IEnumerator KillLaser()
        {
            isDying = true;
            transform.GetChild(0).gameObject.SetActive(false);
            yield return sprite.DOFade(0f, 1f).WaitForCompletion();
            Destroy(gameObject);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (rigidbody.IsTouchingLayers(ground))
                StartCoroutine(KillLaser());
        }
    }
}