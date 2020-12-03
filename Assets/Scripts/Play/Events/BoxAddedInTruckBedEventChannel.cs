using Harmony;
using UnityEngine;

namespace Game
{
    //Author François-Xavier Bernier
    [Findable(Tags.GameController)]
    public class BoxAddedInTruckBedEventChannel : MonoBehaviour
    {
        public event BoxAddedInTruckBedEvent OnBoxAdded;

        public void Publish()
        {
            if (OnBoxAdded != null)
                OnBoxAdded();
        }

        public delegate void BoxAddedInTruckBedEvent();
    }
}