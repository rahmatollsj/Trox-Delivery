using Harmony;
using UnityEngine;

namespace Game
{
    //Author: Félix Bernier
    public class Bridge : MonoBehaviour
    {
        [SerializeField] private GameObject endBridge;
        
        public void Fall()
        {
            endBridge.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            var links = gameObject.GetComponentsInChildren<BoxCollider2D>();
            for (int i = 0; i < links.Length; i++)
            {
                var bridgeCollider = links[i].GetComponent<BoxCollider2D>();
                var vehicleColliders = Finder.Vehicle.GetComponentsInChildren<Collider2D>();
                foreach(var vehicle in vehicleColliders)
                    Physics2D.IgnoreCollision(vehicle, bridgeCollider);
            }
            Physics2D.IgnoreLayerCollision(Layers.Bridge, Layers.Box, true);
        }
    }
}