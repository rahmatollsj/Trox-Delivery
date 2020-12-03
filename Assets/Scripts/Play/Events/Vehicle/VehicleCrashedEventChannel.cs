using Harmony;
using UnityEngine;

namespace Game
{
    // Author: Félix Bernier
    [Findable(Tags.MainController)]
    public class VehicleCrashedEventChannel : MonoBehaviour
        {
            public event VehicleCrashedEvent OnVehicleCrashed;

            public void Publish()
            {
                if (OnVehicleCrashed != null)
                    OnVehicleCrashed();
            }

            public delegate void VehicleCrashedEvent();
        }
}