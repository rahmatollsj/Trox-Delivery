using Harmony;
using UnityEngine;

namespace Game
{
    // Author: Félix Bernier
    [Findable(Tags.MainController)]
    public class VehicleFreezeEventChannel : MonoBehaviour
    {
        public event VehicleFreezeEvent OnVehicleFreeze;

        public void Publish()
        {
            if (OnVehicleFreeze != null)
                OnVehicleFreeze();
        }

        public delegate void VehicleFreezeEvent();
    }
}