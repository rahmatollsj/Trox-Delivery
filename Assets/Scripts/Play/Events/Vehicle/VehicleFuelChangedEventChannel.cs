using Harmony;
using UnityEngine;

namespace Game
{
    // Author: David Pagotto
    [Findable(Tags.GameController)]
    public class VehicleFuelChangedEventChannel : MonoBehaviour
    {
        public delegate void VehicleFuelLevelChangedEvent(float speed);
        public event VehicleFuelLevelChangedEvent OnFuelLevelChanged;

        public void Publish(float fuel)
            => OnFuelLevelChanged?.Invoke(fuel);
    }
}
