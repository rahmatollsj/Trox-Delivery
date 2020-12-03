using Harmony;
using UnityEngine;

namespace Game
{
    //Author: François-Xavier Bernier
    [Findable(Tags.GameController)]
    public class LastLevelFinishedEventChannel : MonoBehaviour
    {
        public event LastLevelEvent OnLastLevelFinished;

        public void Publish(Vehicle vehicle)
        {
            if (OnLastLevelFinished != null)
                OnLastLevelFinished(vehicle);
        }
        public delegate void LastLevelEvent(Vehicle vehicle);
    }
}