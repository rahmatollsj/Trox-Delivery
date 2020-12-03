using System.Collections;
using Harmony;
using UnityEngine;

namespace Game
{
    //Author: Seyed-Rahmatoll Javadi
    public class LandMineStimulus : MonoBehaviour
    {
        [SerializeField] private int impulsionForce = 400;

        private const int GameObjectDestructionDelay = 1;
        
        private Animator animator;

        private void Awake() 
        {
            animator = GetComponent<Animator>();
            animator.enabled = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            GameObject otherGameObject = other.gameObject;
            IVehicle entity = otherGameObject.gameObject.GetComponentInParent<IVehicle>();

            if (entity != null)
            {
                animator.enabled = true;
                Finder.SoundEffectsManager.Play(SoundEffectType.Explosion);
                entity.ImpulseUp(impulsionForce);
                StartCoroutine(DestroyLandMine());
            }
        }

        private IEnumerator DestroyLandMine()
        {
            yield return new WaitForSeconds(GameObjectDestructionDelay);
            Destroy(gameObject);
        }
    }
}