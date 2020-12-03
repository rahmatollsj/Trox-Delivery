using Harmony;
using UnityEngine;

namespace Game
{
    //Author: Seyed-Rahmatoll Javadi
    [Findable(Tags.GameController)]
    public class VehicleEnterDangerousZoneEventChannel : MonoBehaviour
    {
        public event VehicleEnterDangerousZoneEvent OnVehicleEnterDangerousZoneEvent;

        public void Publish()
        {
            if (OnVehicleEnterDangerousZoneEvent != null)
                OnVehicleEnterDangerousZoneEvent();
        }
    }
    
    public delegate void VehicleEnterDangerousZoneEvent();
}