using Harmony;
using UnityEngine;

namespace Game
{
    //Author: Seyed-Rahmatoll Javadi
    [Findable(Tags.GameController)]
    public class GameProgressionEventChannel : MonoBehaviour
    {
        public event GameProgressionEvent OnGameProgressionChanged;
        
        public void Publish(Vehicle vehicle)
        {
            if (OnGameProgressionChanged != null)
                OnGameProgressionChanged(vehicle);
        }
    }

    public delegate void GameProgressionEvent(Vehicle sender);
}