using Harmony;
using UnityEngine;

namespace Game
{
    //Author: François-Xavier Bernier
    [Findable(Tags.GameController)]
    public class VehicleOutOfDangerousZoneEventChannel : MonoBehaviour
    {
        public event VehicleOutOfDangerousZoneEvent OnVehicleOutOfDangerousZoneEvent;

        public void Publish()
        {
            if (OnVehicleOutOfDangerousZoneEvent != null)
                OnVehicleOutOfDangerousZoneEvent();
        }
    }
    public delegate void VehicleOutOfDangerousZoneEvent();
}