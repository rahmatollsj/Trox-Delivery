using System.Collections;
using Harmony;
using UnityEngine;

namespace Game
{
    //Author: Félix Bernier
    public class EnteringBridgeTrigger : MonoBehaviour
    {
        [SerializeField] private float timeBeforeBridgeFall;

        [SerializeField] private Bridge bridge;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag(Tags.VehicleBody))
                return;
            var vehicle = other.GetComponentInParent<Vehicle>();
            StartCoroutine(VehicleEnteringBridge());
        }
        
        private IEnumerator VehicleEnteringBridge()
        {
            yield return new WaitForSeconds(timeBeforeBridgeFall);
            bridge.Fall();
        }
    }
}