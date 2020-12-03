using Harmony;
using TMPro;
using UnityEngine;

namespace Game
{
    // Author: Seyed-Rahmatoll Javadi
    public class HeadUpDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text airTimeCombo;
        //Author: Seyed-Rahmatoll Javadi
        [SerializeField] private Indicator progression;
        [SerializeField] private Indicator thermometer;
        [SerializeField] private Gauge speedGauge;
        //Author: David Pagotto
        [SerializeField] private Gauge fuelGauge;
        //Author François-Xavier Bernier
        [SerializeField] private EngineStateSign overHeatEngineSign;
        [SerializeField] private EngineStateSign frozenEngineSign;
        
        //Author: Seyed-Rahmatoll Javadi
        private const float GaugeTotalPercentage = 100f;

        //Author: Seyed-Rahmatoll Javadi
        private GameProgressionEventChannel gameProgressionEventChannel;
        //Author: David Pagotto
        private VehicleSpeedChangedEventChannel vehicleSpeedChangedEventChannel;
        private VehicleFuelChangedEventChannel vehicleFuelChangedEventChannel;

        private GamePausedEventChannel gamePausedEventChannel;
        private GameResumedEventChannel gameResumedEventChannel;
        
        //Author: Seyed-Rahmatoll Javadi
        private LevelLoadedEventChannel levelLoadedEventChannel;
        private VehicleTemperatureChangedEventChannel vehicleTemperatureChangedEventChannel;

        //Author Félix Bernier
        private ScoreCalculator scoreCalculator;
        private TimeManagement timeManagement;
        private LevelFailedEventChannel levelFailedEventChannel;
        private LevelSuccessEventChannel levelSucessEventChannel;
        
        private VehicleOutOfDangerousZoneEventChannel outOfDangerousZoneEventChannel;

        private void Awake()
        {
            // Félix Bernier
            airTimeCombo.text = "";
            
            //Author: Seyed-Rahmatoll Javadi
            InitializeEventChannels();
            scoreCalculator = Finder.ScoreCalculator;
        }

        private void Start()
        {
            timeManagement = Finder.TimeManagement;
            gameObject.SetActive(!timeManagement.GameIsPaused);
        }

        private void Update()
        {
            UpdateAirTimeUI();
                
            //Author: Félix Bernier
            gameObject.SetActive(!timeManagement.GameIsPaused);
        }

        private void OnDestroy()
        {
            //Author: Félix Bernier
            gameResumedEventChannel.OnGameResumed -= OnGameResumed;
            levelSucessEventChannel.OnLevelSuccess -= OnLevelEnded;
            levelFailedEventChannel.OnLevelFailedEventChannel -= OnLevelEnded;
            gameProgressionEventChannel.OnGameProgressionChanged -= OnGameProgressionChanged;
            vehicleTemperatureChangedEventChannel.OnTemperatureGaugeLevelChanged -= OnVehicleTemperatureChanged;
            vehicleSpeedChangedEventChannel.OnVehicleSpeedChanged -= OnVehicleSpeedChanged;
            vehicleFuelChangedEventChannel.OnFuelLevelChanged -= OnVehicleFuelChanged;
            outOfDangerousZoneEventChannel.OnVehicleOutOfDangerousZoneEvent -= OnVehicleSafe;
            levelLoadedEventChannel.OnLevelLoaded -= OnLevelLoaded;
        }

        //Author: Seyed-Rahmatoll Javadi
        private void InitializeEventChannels()
        {
            // Author: Seyed-Rahmatoll Javadi
            levelLoadedEventChannel = Finder.LevelLoadedEventChannel;
            levelLoadedEventChannel.OnLevelLoaded += OnLevelLoaded;
    
            gameProgressionEventChannel = Finder.GameProgressionEventChannel;
            gameProgressionEventChannel.OnGameProgressionChanged += OnGameProgressionChanged;
    
            vehicleTemperatureChangedEventChannel = Finder.VehicleTemperatureChangedEventChannel;
            vehicleTemperatureChangedEventChannel.OnTemperatureGaugeLevelChanged += OnVehicleTemperatureChanged;

            // Author: David Pagotto
            vehicleSpeedChangedEventChannel = Finder.VehicleSpeedChangedEventChannel;
            vehicleFuelChangedEventChannel = Finder.VehicleFuelChangedEventChannel;

            vehicleSpeedChangedEventChannel.OnVehicleSpeedChanged += OnVehicleSpeedChanged;
            vehicleFuelChangedEventChannel.OnFuelLevelChanged += OnVehicleFuelChanged;

            outOfDangerousZoneEventChannel = Finder.VehicleOutOfDangerousZoneEventChannel;
            outOfDangerousZoneEventChannel.OnVehicleOutOfDangerousZoneEvent += OnVehicleSafe;
    
            //Author: Félix Bernier
            gamePausedEventChannel = Finder.GamePausedEventChannel;
            gameResumedEventChannel = Finder.GameResumedEventChannel;
            gameResumedEventChannel.OnGameResumed += OnGameResumed;
            levelSucessEventChannel = Finder.LevelSuccessEventChannel;
            levelSucessEventChannel.OnLevelSuccess += OnLevelEnded;
            levelFailedEventChannel = Finder.LevelFailedEventChannel;
            levelFailedEventChannel.OnLevelFailedEventChannel += OnLevelEnded;
        }
        
        //Author: Seyed-Rahmatoll Javadi
        private void OnLevelLoaded()
        {
            //Author: Seyed-Rahmatoll Javadi
            timeManagement.UnfreezeGame();
            //Author François-Xavier Bernier
            gameObject.SetActive(true);
            OnVehicleSafe();
        }
        
        //Author: Félix Bernier
        private void OnGameResumed()
        {
            gameObject.SetActive(true);
            //Author: Seyed-Rahmatoll Javadi
            timeManagement.UnfreezeGame();
        }

        private void OnGameProgressionChanged(Vehicle vehicle)
        {
            progression.ChangePointPosition(vehicle.Progression);
        }

        // Author: David Pagotto
        private void OnVehicleFuelChanged(float fuel) 
        {
            fuelGauge.NeedleRotationPercent = Mathf.Clamp01(fuel) * GaugeTotalPercentage;
        }

        // Author: David Pagotto
        private void OnVehicleSpeedChanged(float speed)
        {
            //Author: Seyed-Rahmatoll Javadi
            speedGauge.NeedleRotationPercent = Mathf.Clamp01(speed) * GaugeTotalPercentage;
        }


        //Author : François-Xavier Bernier
        private void OnVehicleSafe()
        {
            overHeatEngineSign.gameObject.SetActive(false);
            frozenEngineSign.gameObject.SetActive(false);
            overHeatEngineSign.ResetNbFlashed();
            frozenEngineSign.ResetNbFlashed();
        }

        private void OnVehicleTemperatureChanged(Vehicle vehicle)
        {
            thermometer.ChangePointPosition(vehicle.Temperature);

            switch (vehicle.CurrentVehicleZoneState)
            {
                case CurrentZoneState.Cold:
                    frozenEngineSign.gameObject.SetActive(true);
                    break;
                case CurrentZoneState.Hot:
                    overHeatEngineSign.gameObject.SetActive(true);
                    break;
            }
        }

        private void UpdateAirTimeUI()
        {
            if (scoreCalculator.ScoreCombo > scoreCalculator.MinimumAirTimePoints)
            {
                airTimeCombo.alpha = 1f;
                if(scoreCalculator.ScoreCombo % scoreCalculator.RangeOfPoints == 0)
                    airTimeCombo.text = "+" + (scoreCalculator.ScoreCombo - (scoreCalculator.ScoreCombo % scoreCalculator.RangeOfPoints));
            }
            else
                airTimeCombo.alpha = 0f;
        }
        
        //Author: Félix Bernier
        public void Pause()
        {
            timeManagement.FreezeGame();
            gamePausedEventChannel.Publish();
            gameObject.SetActive(false);
        }
        
        //Author: Félix Bernier
        private void OnLevelEnded()
        {
            timeManagement.FreezeGame();
            gameObject.SetActive(false);
        }
    }
}