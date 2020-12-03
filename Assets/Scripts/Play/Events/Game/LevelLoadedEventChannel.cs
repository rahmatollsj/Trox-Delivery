using Harmony;
using UnityEngine;

namespace Game
{
    //Author: Seyed-Rahmatoll Javadi
    [Findable(Tags.MainController)]
    public class LevelLoadedEventChannel : MonoBehaviour
    {
        public event LevelLoadedEvent OnLevelLoaded;

        public void Publish()
        {
            if (OnLevelLoaded != null)
                OnLevelLoaded();
        }
    }

    public delegate void LevelLoadedEvent();
}