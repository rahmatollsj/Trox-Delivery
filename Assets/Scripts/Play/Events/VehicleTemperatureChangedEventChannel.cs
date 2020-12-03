using Harmony;
using UnityEngine;

namespace Game
{
    //Author: Seyed-Rahmatoll Javadi
    [Findable(Tags.GameController)]
    public class VehicleTemperatureChangedEventChannel : MonoBehaviour
    {
        public event TemperatureGaugeLevelChangedEvent OnTemperatureGaugeLevelChanged;

        public void Publish(Vehicle vehicle)
        {
            if (OnTemperatureGaugeLevelChanged != null)
                OnTemperatureGaugeLevelChanged(vehicle);
        }
    }

    public delegate void TemperatureGaugeLevelChangedEvent(Vehicle sender);
}