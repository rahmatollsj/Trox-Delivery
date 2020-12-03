using Harmony;
using UnityEngine;

namespace Game
{
    //Author François-Xavier Bernier
    [Findable(Tags.GameController)]
    public class BoxOutOfTruckBedEventChannel : MonoBehaviour
    {
        public event BoxOutInTruckBedEvent OnBoxOut;

        public void Publish()
        {
            if (OnBoxOut != null)
                OnBoxOut();
        }

        public delegate void BoxOutInTruckBedEvent();
    }
}