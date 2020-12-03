using Harmony;
using UnityEngine;

namespace Game
{
    // Author: Félix Bernier
    [Findable(Tags.MainController)]
    public class VehicleHotEventChannel : MonoBehaviour
    {
        public event VehicleHotEvent OnVehicleHot;

        public void Publish()
        {
            if (OnVehicleHot != null)
                OnVehicleHot();
        }

        public delegate void VehicleHotEvent();
    }
}