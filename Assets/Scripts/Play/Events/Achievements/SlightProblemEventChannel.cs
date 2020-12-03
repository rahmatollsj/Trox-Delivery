using Harmony;
using UnityEngine;

namespace Game
{
    [Findable(Tags.MainController)]
    public class SlightProblemEventChannel : MonoBehaviour
    {
        public event SlightProblemEvent OnRockHitVehicle;

        public void Publish()
        {
            if (OnRockHitVehicle != null)
                OnRockHitVehicle();
        }
        public delegate void SlightProblemEvent();
    }
}