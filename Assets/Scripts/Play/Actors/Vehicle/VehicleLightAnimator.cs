using Harmony;
using UnityEngine;

namespace Game
{
    // Author: David Pagotto
    public class VehicleLightAnimator : MonoBehaviour
    {
        private Rigidbody2D rbVehicle;
        private VehicleLightingController lightingController;
        private VehicleState state;
        // https://answers.unity.com/questions/364910/direction-of-rigidbodyvelocitymagnitude.html
        private float VehicleSpeed => Finder.Vehicle.VehicleBody.transform.InverseTransformDirection(rbVehicle.velocity).x;
        private void Awake()
        {
            rbVehicle = Finder.Vehicle.VehicleBody.GetComponent<Rigidbody2D>();
            lightingController = Finder.Vehicle.transform.Find(GameObjects.Body).transform.Find(GameObjects.Lighting).GetComponent<VehicleLightingController>();
        }

        private void OnStateChanged()
        {
            lightingController.EnableBlinkingWarningLights = false;
            lightingController.EnableBlinkingBrakeLights = false;
            lightingController.EnableReverseLight = false;
            lightingController.EnableBrakeLight = false;

        }

        public void OnVehicleForward()
        {
            if (state != VehicleState.Forward)
            {
                OnStateChanged();
                state = VehicleState.Forward;
            }
            if (VehicleSpeed < 0f)
                lightingController.EnableBrakeLight = true;
            else
                lightingController.EnableBrakeLight = false;
        }

        public void OnVehicleBackward()
        {
            if (state != VehicleState.Backward)
            {
                OnStateChanged();
                state = VehicleState.Backward;
            }
            if (VehicleSpeed > 0f)
            {
                lightingController.EnableBlinkingWarningLights = false;
                lightingController.EnableBrakeLight = true;
                lightingController.EnableReverseLight = false;
            }
            else
            {
                lightingController.EnableBlinkingWarningLights = true;
                lightingController.EnableReverseLight = true;
                lightingController.EnableBrakeLight = false;
            }
        }

        public void OnVehicleAirborne()
        {
            if (state != VehicleState.Airborne)
            {
                OnStateChanged();
                state = VehicleState.Airborne;
            }
            lightingController.EnableBlinkingWarningLights = true;
            lightingController.EnableBlinkingBrakeLights = true;
        }

        private enum VehicleState
        {
            Unknown,
            Forward,
            Backward,
            Airborne
        }
    }
}