using Harmony;
using UnityEngine;

namespace Game
{
    // Author: Félix Bernier
    public class EndPointTrigger : MonoBehaviour
    {
        //Author Félix Bernier 
        private LevelSuccessEventChannel levelSuccessEventChannel;
        private LevelSpecs levelSpecs;
        private LevelFailedEventChannel levelFailedEventChannel;

        //Author François-Xavier Bernier 
        private LastLevelFinishedEventChannel lastLevelFinishedEventChannel;
        private GameBox gameBox;

        private void Awake()
        {
            //Author Félix Bernier 
            levelFailedEventChannel = Finder.LevelFailedEventChannel;
            levelSuccessEventChannel = Finder.LevelSuccessEventChannel;
            levelSpecs = Finder.LevelSpecs;
            
            //Author François-Xavier Bernier
            lastLevelFinishedEventChannel = Finder.LastLevelFinishedEventChannel;
        }

        private void Start()
        {
            gameBox = Finder.GameBox;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            //Author Félix Bernier
            var otherGameObject = other.gameObject;
            var vehicle = otherGameObject.GetComponentInParent<Vehicle>();
            if (!other.CompareTag(Tags.VehicleBody))
                return;
            //Author : François-Xavier Bernier
            if (gameBox.BoxCount >= levelSpecs.NbBoxNeededToPassLevel &&
                levelSpecs.IsLastLevelOfTheGame)
            {
                lastLevelFinishedEventChannel.Publish(vehicle);
                return;
            }

            //Author Félix Bernier et François-Xavier Bernier
            if (gameBox.BoxCount >= levelSpecs.NbBoxNeededToPassLevel)
                levelSuccessEventChannel.Publish();
            else if (gameBox.BoxCount < levelSpecs.NbBoxNeededToPassLevel)
                levelFailedEventChannel.Publish();
        }
    }
}