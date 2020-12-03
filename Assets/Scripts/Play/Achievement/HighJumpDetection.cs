using System.Collections;
using Harmony;
using UnityEngine;

namespace Game
{
    // Author: Benoit Simon-Turgeon
    public class HighJumpDetection : MonoBehaviour
    {
        [Tooltip("Time in seconds above the meters from ground needed for the achievement to be unlocked")]
        [SerializeField] private float airTimeNeeded = 4f;
        [Tooltip("Minimum distance from the ground the vehicle has to be for the air time duration")]
        [SerializeField] private float metersFromGround = 5f;
        [SerializeField] private LayerMask groundLayers;
        
        private Vehicle vehicle;
        private GameObject vehicleBody;
        private VehicleMover vehicleMover;

        private HighJumpSuccessfulEventChannel highJumpSuccessfulEventChannel;
        
        private IEnumerator highJumpStarted;
        
        private bool jumpStarted = false;

        private void Awake()
        {
            vehicle = Finder.Vehicle;
            vehicleMover = vehicle.GetComponent<VehicleMover>();
            vehicleBody = vehicle.VehicleBody;
            
            highJumpStarted = OnHighJumpStarted();
            highJumpSuccessfulEventChannel = Finder.HighJumpSuccessfulEventChannel;
        }

        private void FixedUpdate()
        {
            if (jumpStarted)
            {
                if (!vehicleMover.IsAirTiming || IsGroundDetected())
                {
                    StopCoroutine(highJumpStarted);
                    jumpStarted = false;
                    highJumpStarted = OnHighJumpStarted();
                }
            }
            else if (vehicleMover.IsAirTiming && !IsGroundDetected())
            {
                StartCoroutine(highJumpStarted);
            }
        }

        private IEnumerator OnHighJumpStarted()
        {
            jumpStarted = true;
            yield return new WaitForSeconds(airTimeNeeded);
            highJumpSuccessfulEventChannel.Publish();
        }

        private bool IsGroundDetected()
        {
            var vehiclePosition = vehicleBody.transform.position;
            return Physics2D.Linecast(vehiclePosition, vehiclePosition - new Vector3(0, metersFromGround, 0), groundLayers);
        }
    }
}