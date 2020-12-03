using Harmony;
using UnityEngine;

namespace Game
{
    // Author: Benoit Simon-Turgeon
    public class KillTrigger : MonoBehaviour
    {
        private LevelFailedEventChannel levelFailedEventChannel;
        
        private void Awake()
        {
            levelFailedEventChannel = Finder.LevelFailedEventChannel;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.CompareTag(Tags.VehicleBody))
                // Author: Benoit Simon-Turgeon
                levelFailedEventChannel.Publish();
        }
    }
}