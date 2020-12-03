using Harmony;
using UnityEngine;

namespace Game
{
    //Author : François-Xavier Bernier
    [Findable(Tags.LevelSpec)]
    public class LevelSpecs : MonoBehaviour
    {
        //Author : François-Xavier Bernier
        [Tooltip("Minimum number of boxes to pass level")]
        [SerializeField] private uint nbBoxNeededToPassLevel = 1;

        //Author : François-Xavier Bernier
        [Tooltip("Set to TRUE iof its the last level of the game")]
        [SerializeField] private bool isLastLevelOfTheGame = false;

        [Space(5)]

        // Author: David Pagotto
        [Header("Level Settings")]
        [Tooltip(
            "Indicates the amount of fuel with which the vehicle will start (time in seconds with which the vehicle can run)")]
        [SerializeField] private float initialFuel = 100f;

        //Author: Seyed-Rahmatoll Javadi
        [Tooltip(
            "Indicates the temperature with which the vehicle will start (The engine is cold under 50 and it is hot above)")]
        [SerializeField] private float initialTemperature = 0.50f;

        // Author: Benoit Simon-Turgeon
        [Tooltip("Gravity of the level in m/s. Default: 0,-9.8f")]
        [SerializeField] private Vector2 gravityAcceleration = new Vector2(0, -9.8f);

        // Author: Benoit Simon-Turgeon
        public float InitialFuel => initialFuel;
        public float InitialTemperature => initialTemperature;
        public bool IsLastLevelOfTheGame => isLastLevelOfTheGame;
        public uint NbBoxNeededToPassLevel => nbBoxNeededToPassLevel;

        // Author: Benoit Simon-Turgeon
        private void Start()
        {
            Physics2D.gravity = gravityAcceleration;
        }
    }
}