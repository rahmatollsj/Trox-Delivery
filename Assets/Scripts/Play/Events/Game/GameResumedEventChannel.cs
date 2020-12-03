using Harmony;
using UnityEngine;

namespace Game
{
    //Author: Félix Bernier
    [Findable(Tags.GameController)]
    public class GameResumedEventChannel : MonoBehaviour
    {
        public event GameResumedEvent OnGameResumed;

        public void Publish()
        {
            if (OnGameResumed != null)
                OnGameResumed();
        }
        public delegate void GameResumedEvent();
    }
}