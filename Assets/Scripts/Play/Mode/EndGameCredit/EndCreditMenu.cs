using Harmony;
using UnityEngine;

namespace Game
{
    //Author : François-Xavier Bernier
    public class EndCreditMenu : MonoBehaviour
    {
        private LastLevelFinishedEventChannel lastLevelFinishedEventChannel;
        private Inputs inputs;

        private void Awake()
        {
            inputs = Finder.Inputs;
            lastLevelFinishedEventChannel = Finder.LastLevelFinishedEventChannel;
            lastLevelFinishedEventChannel.OnLastLevelFinished += OnLastLevelFinished;
            gameObject.SetActive(false);
        }

        private void Start()
        {
            gameObject.SetActive(Time.timeScale == 0f);
        }

        private void Update()
        {
            gameObject.SetActive(Time.timeScale == 0f);
            if (inputs.Game.Exit.triggered)
            {
                Time.timeScale = 1f;
            }
        }

        private void OnLastLevelFinished(Vehicle vehicle)
        {
            vehicle.enabled = false;
            gameObject.SetActive(true);
            Time.timeScale = 0f;
        }

        private void OnDestroy()
        {
            lastLevelFinishedEventChannel.OnLastLevelFinished -= OnLastLevelFinished;
        }
    }
}