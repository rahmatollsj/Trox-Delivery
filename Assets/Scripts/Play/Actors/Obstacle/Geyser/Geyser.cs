using System.Collections;
using Harmony;
using UnityEngine;

namespace Game
{
    // Author: Benoit Simon-Turgeon
    public class Geyser : MonoBehaviour
    {
        [SerializeField] private float geyserForce = 3f;
        [Tooltip("Time in seconds between each cycle")]
        [SerializeField] private float timeCycle = 8f;
        
        private Animator animator;
        private bool isAnimActive = false;
        
        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag(Tags.VehicleBody))
                other.gameObject.GetComponentInParent<Vehicle>().ImpulseUp(geyserForce);
        }

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
        }

        private void FixedUpdate()
        {
            if (isAnimActive)
                return;

            isAnimActive = true;
            StartCoroutine(PlayAnimation());
        }
        
        private IEnumerator PlayAnimation()
        {
            animator.Play("geyser");
            yield return new WaitForSeconds(timeCycle);
            animator.Play("Inactive");
            isAnimActive = false;
        }
    }
}