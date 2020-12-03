using Harmony;
using UnityEngine;

namespace Game
{
    public class BackflipDetection : MonoBehaviour
    {
        private Rigidbody2D vehicleBody;
        private VehicleMover mover;
        private VehicleFlippedEventChannel vehicleFlippedEventChannel;
        
        private float flips;
        private int nbFlips;
        private float deltaRotation;
        private float windupRotation;
        private float previousRotation = 0f;
        private bool airTimingStarted = false;
        private float prevDelta;
        
        private void Start()
        {
            vehicleBody = Finder.Vehicle.VehicleBody.GetComponent<Rigidbody2D>();
            mover = Finder.Vehicle.GetComponent<VehicleMover>();
            vehicleFlippedEventChannel = Finder.VehicleFlippedEventChannel;
            
            nbFlips = 0;
            deltaRotation = 0;
            windupRotation = 0;
        }
        
        
        private void FixedUpdate()
        {
            if (mover.IsAirTiming && nbFlips == 0)
            {
                if (!airTimingStarted)
                {
                    ResetData();
                }
                else
                {
                    deltaRotation = vehicleBody.transform.eulerAngles.z - previousRotation;

                    previousRotation = NormalizeAngle(vehicleBody.transform.eulerAngles.z);
                    deltaRotation = NormalizeAngle(deltaRotation);

                    // Si la direction change
                    if ((prevDelta < 0 && deltaRotation > 0) || 
                        (prevDelta > 0 && deltaRotation < 0f))
                    {
                       DirectionIsChanged();
                    }
                    prevDelta = deltaRotation;

                    windupRotation += Mathf.Abs(deltaRotation);
                }
                
                CheckFirstFlip();
            }
            else
                airTimingStarted = false;
        }
        
        private float NormalizeAngle(float value)
        {
            if (value >= 300) 
                value -= 360;
            if (value <= -300) 
                value += 360;
            return value;
        }
        
        private void ResetData()
        {
            airTimingStarted = true;
            deltaRotation = 0f;
            windupRotation = 0f;
            previousRotation = vehicleBody.transform.eulerAngles.z;
            previousRotation = NormalizeAngle(previousRotation);
        }
        
        private void CheckFirstFlip()
        {
            flips = windupRotation / 360f;
            if (flips >= nbFlips + 1 && nbFlips == 0)
            {
                vehicleFlippedEventChannel.Publish();
                nbFlips++;
            }
        }

        private void DirectionIsChanged()
        {
            deltaRotation = 0f;
            windupRotation = 0f;
            prevDelta = deltaRotation;
            nbFlips = 0;
        }
    }
}