using Harmony;
using UnityEngine;

namespace Game
{
    [Findable(Tags.GameController)]
    public class Game : MonoBehaviour
    {
        //Author : François-Xavier Bernier
        private LastLevelFinishedEventChannel lastLevelFinishedEventChannel;
        private LevelSuccessEventChannel gameFinishedEventChannel;

        private GamePausedEventChannel gamePausedEventChannel;
        private GameResumedEventChannel gameResumedEventChannel;
        
        // Author: Benoit Simon-Turgeon
        private Inputs inputs;

        // Author: David Pagotto
        private TimeManagement timeManagement;

        private void Awake()
        {
            inputs = Finder.Inputs;
            inputs.Game.Enable();
            
            lastLevelFinishedEventChannel = Finder.LastLevelFinishedEventChannel;
            lastLevelFinishedEventChannel.OnLastLevelFinished += OnLastLevelFinished;

            gamePausedEventChannel = Finder.GamePausedEventChannel;
            gameResumedEventChannel = Finder.GameResumedEventChannel;
            timeManagement = Finder.TimeManagement;
        }

        private void Update()
        {
            if (inputs.Game.Exit.triggered)
            {
                if (timeManagement.GameIsPaused)
                {
                    timeManagement.UnfreezeGame();
                    gameResumedEventChannel.Publish();
                }
                else
                {
                    timeManagement.FreezeGame();
                    gamePausedEventChannel.Publish();
                }
            }
        }

        private void OnDestroy()
        {
            //Author : François-Xavier Bernier
            lastLevelFinishedEventChannel.OnLastLevelFinished -= OnLastLevelFinished;
        }
        
        //Author : François-Xavier Bernier
        private void OnLastLevelFinished(Vehicle vehicle)
        {
            vehicle.enabled = false;
        }
    }
}