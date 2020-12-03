using Harmony;
using UnityEngine;

namespace Game
{
    //Author: Félix Bernier
    [Findable(Tags.GameController)]
    public class GamePausedEventChannel : MonoBehaviour
    {
        public event GamePausedEvent OnGamePaused;

        public void Publish()
        {
            if (OnGamePaused != null)
                OnGamePaused();
        }
        public delegate void GamePausedEvent();
    }
}