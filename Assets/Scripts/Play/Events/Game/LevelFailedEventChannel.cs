using Harmony;
using UnityEngine;

namespace Game
{
    //Author: Félix Bernier
    [Findable(Tags.MainController)]
    public class LevelFailedEventChannel : MonoBehaviour
    {
        public event LevelFailedEvent OnLevelFailedEventChannel;

        public void Publish()
        {
            if (OnLevelFailedEventChannel != null)
                OnLevelFailedEventChannel();
        }

        public delegate void LevelFailedEvent();
    }
}