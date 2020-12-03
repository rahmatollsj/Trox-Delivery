using UnityEngine;

namespace Game
{
    public class AsteroidTrigger : MonoBehaviour
    {
        [SerializeField] private GameObject asteroid;

        private void OnTriggerEnter2D(Collider2D other)
        {
            var vehicle = other?.gameObject?.GetComponentInParent<Vehicle>();
            if (vehicle != null)
            {
                asteroid.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                asteroid.GetComponent<Rigidbody2D>().mass = 100f;
            }
        }
    }
}