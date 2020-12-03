using Harmony;
using UnityEngine;

namespace Game
{
    // Author: David Pagotto
    [Findable(Tags.GameController)]
    public class LevelAbandonEventChannel : MonoBehaviour
    {
        // Author: David Pagotto
        public event LevelAbandonEvent OnLevelAbandon;

        public void Publish()
            => OnLevelAbandon?.Invoke();

        public delegate void LevelAbandonEvent();
    }
}
