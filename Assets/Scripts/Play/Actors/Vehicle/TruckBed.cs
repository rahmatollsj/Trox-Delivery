using Harmony;
using UnityEngine;

namespace Game
{
    [Findable(Tags.GameController)]
    //Author François-Xavier Bernier
    public class TruckBed : MonoBehaviour
    {
        private BoxAddedInTruckBedEventChannel boxAddedInTruckBedEventChannel;
        private BoxOutOfTruckBedEventChannel boxOutOfTruckBedEventChannel;
        
        //Author François-Xavier Bernier
        private void Awake()
        {
            boxAddedInTruckBedEventChannel = Finder.BoxAddedInTruckBedEventChannel;
            boxOutOfTruckBedEventChannel = Finder.BoxOutOfTruckBedEventChannel;
        }
        //Author François-Xavier Bernier
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(Tags.Box))
            {
                boxAddedInTruckBedEventChannel.Publish();
                // Benoit Simon-Turgeon
                other.gameObject.GetComponent<Box>().IsBoxOut = false;
            }
        }
        //Author François-Xavier Bernier
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag(Tags.Box))
            {
                 boxOutOfTruckBedEventChannel.Publish();
                 // Benoit Simon-Turgeon
                 other.gameObject.GetComponent<Box>().IsBoxOut = true;
            }
        }
    }
}