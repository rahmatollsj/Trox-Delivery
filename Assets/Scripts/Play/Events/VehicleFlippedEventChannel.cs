using Harmony;
using UnityEngine;

namespace Game
{
    //Author: Félix Bernier
    [Findable(Tags.MainController)]
    public class VehicleFlippedEventChannel : MonoBehaviour
    {
        public event VehicleFlippedEvent OnVehicleFlipped;

        public void Publish()
        {
            if (OnVehicleFlipped != null)
                OnVehicleFlipped();
        }

        public delegate void VehicleFlippedEvent();
    }
}