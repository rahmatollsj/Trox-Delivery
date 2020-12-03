using Harmony;
using UnityEngine;

namespace Game
{
    // Author: David Pagotto
    [Findable(Tags.GameController)]
    public class VehicleSpeedChangedEventChannel : MonoBehaviour
    {
        public delegate void VehicleSpeedChangedEvent(float speed);
        public event VehicleSpeedChangedEvent OnVehicleSpeedChanged;

        public void Publish(float speed)
            => OnVehicleSpeedChanged?.Invoke(speed);
    }
}
