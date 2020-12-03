using Harmony;
using UnityEngine;

namespace Game
{
    [Findable(Tags.GameController)]
    public class TimeManagement : MonoBehaviour
    {
        public bool GameIsPaused { get; private set; }

        private void Awake()
        {
            GameIsPaused = false;
        }

        public void FreezeGame()
        {
            Time.timeScale = 0f;
            GameIsPaused = true;
        }

        public void UnfreezeGame()
        {
            Time.timeScale = 1f;
            GameIsPaused = false;
        }
    }
}