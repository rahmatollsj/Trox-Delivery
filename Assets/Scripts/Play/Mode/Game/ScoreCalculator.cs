using Harmony;
using UnityEngine;

namespace Game
{
    // Author: Félix Bernier
    [Findable(Tags.GameController)]
    public class ScoreCalculator : MonoBehaviour
    {
        [SerializeField] private uint pointsPerBoxes = 555;
        [SerializeField] private uint pointsPerSecondsLeft = 555;
        [SerializeField] public int pointsPerAirTime = 1;
        [SerializeField] private int scoreMultiplier;
        [SerializeField] private int minimumAirTimePoints;
        [SerializeField] private int rangeOfPoints;
        
        private int score;
        private int scoreCombo;
        
        public int MinimumAirTimePoints => minimumAirTimePoints;

        public int RangeOfPoints => rangeOfPoints;
        
        public int Score
        {
            get => score;
            private set => score = value;
        }
        
        public int ScoreCombo
        {
            get => scoreCombo;
            private set => scoreCombo = value;
        }
        
        public void OnVehicleInAir()
        {
            ScoreCombo += pointsPerAirTime;
        }
        
        public void OnVehicleLanded()
        {
            if (ScoreCombo > minimumAirTimePoints)
                Score += (ScoreCombo - (ScoreCombo % rangeOfPoints));
            ScoreCombo = 0;
        }
        public int CalculateScore(int comboScore, int boxCount)
        {
            return (comboScore * scoreMultiplier) + (int)(boxCount * pointsPerBoxes + Finder.Vehicle.CurrentFuel * pointsPerSecondsLeft);
        }
    }
}