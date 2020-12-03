using Harmony;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    [Findable(Tags.Vehicle)]
    public class Vehicle : MonoBehaviour, IVehicle
    {
        [Header("Configuration")]
        //Author: Benoit Simon-Turgeon
        [SerializeField] private GameObject frontWheel;
        [SerializeField] private GameObject rearWheel;
        [SerializeField] private GameObject vehicleBody;

        [Space(5)]
        
        //Author: Seyed-Rahmatoll Javadi
        [SerializeField] private GameObject endPoint;
        
        [SerializeField] private float temperatureSpeedMultiplierWhenAccelerating = 2f;
        // Author: Benoit Simon-Turgeon
        [SerializeField] private float temperatureChangingSpeedDivider = 20f;
        //Author: Seyed-Rahmatoll Javadi
        [SerializeField] private float temperatureChangingSpeed = 0.01f;

        //Author: Seyed-Rahmatoll Javadi
        private GameProgressionEventChannel gameProgressionEventChannel;
        private VehicleTemperatureChangedEventChannel vehicleTemperatureChangedEventChannel;
        private LevelFailedEventChannel levelFailedEventChannel;
        private ScoreCalculator scoreCalculator;
        private VehicleEnterDangerousZoneEventChannel vehicleEnterDangerousZoneEventChannel;
        private VehicleAudio vehicleAudio;

        // Author: David Pagotto
        private VehicleLightAnimator lightAnimator;
        private VehicleSpeedChangedEventChannel vehicleSpeedChangedEventChannel;
        private VehicleFuelChangedEventChannel vehicleFuelChangedEventChannel;
        
        //Author: Félix Bernier
        private VehicleCrashedEventChannel vehicleCrashedEventChannel;
        private VehicleFreezeEventChannel vehicleFreezeEventChannel;
        private VehicleHotEventChannel vehicleHotEventChannel;
        
        //Author : François-Xavier Bernier
        public CurrentZoneState CurrentVehicleZoneState { get; private set; }
        private VehicleOutOfDangerousZoneEventChannel onPlayerNowSafe;
        
        // Author: Benoit Simon-Turgeon
        public GameObject FrontWheel => frontWheel;
        public GameObject RearWheel => rearWheel;
        public GameObject VehicleBody => vehicleBody;
        
        private VehicleMover mover;
        
        private Inputs inputs;
        
        private float temperatureSpeedMultiplier = 1f;
        private float baseTemperature;
        
        //Author: Seyed-Rahmatoll Javadi
        private float startingPointX;
        
        public float Progression { get; private set; }
        public float Temperature { get; set; }

        // Author: David Pagotto
        public float MaxFuel => Finder.LevelSpecs.InitialFuel;
        public float CurrentFuel { get; set; }
        
        private void Awake()
        {
            //Author: François-Xavier Bernier
            CurrentVehicleZoneState = CurrentZoneState.Neutral;
            onPlayerNowSafe = Finder.VehicleOutOfDangerousZoneEventChannel;
            
            // Author: Benoit Simon-Turgeon
            mover = GetComponent<VehicleMover>();
            lightAnimator = GetComponent<VehicleLightAnimator>();
            inputs = Finder.Inputs;

            //Author: Seyed-Rahmatoll Javadi
            gameProgressionEventChannel = Finder.GameProgressionEventChannel;
            startingPointX = vehicleBody.transform.position.x;
            Progression = 0;

            vehicleTemperatureChangedEventChannel = Finder.VehicleTemperatureChangedEventChannel;
            baseTemperature = Finder.LevelSpecs.InitialTemperature;
            // Author: Benoit Simon-Turgeon
            Temperature = baseTemperature;
            //Author: Seyed-Rahmatoll Javadi
            vehicleTemperatureChangedEventChannel.Publish(this);
            levelFailedEventChannel = Finder.LevelFailedEventChannel;
            scoreCalculator = Finder.ScoreCalculator;

            vehicleSpeedChangedEventChannel = Finder.VehicleSpeedChangedEventChannel;
            vehicleFuelChangedEventChannel = Finder.VehicleFuelChangedEventChannel;

            vehicleEnterDangerousZoneEventChannel = Finder.VehicleEnterDangerousZoneEventChannel;

            //Author: Seyed-Rahmatoll Javadi
            vehicleAudio = GetComponent<VehicleAudio>();

            // Author: David Pagotto
            CurrentFuel = MaxFuel;
            
            //Author: Félix Bernier
            vehicleFreezeEventChannel = Finder.VehicleFreezeEventChannel;
            vehicleHotEventChannel = Finder.VehicleHotEventChannel;
        }
        
        // Author: David Pagotto
        private void Update()
        {
            if (Time.timeScale != 1f)
                return;

            if (inputs.Game.Bonus1.triggered)
                Finder.BonusInventory.ActivateBonus(this, 0);
            if (inputs.Game.Bonus2.triggered)
                Finder.BonusInventory.ActivateBonus(this, 1);
            if (inputs.Game.Bonus3.triggered)
                Finder.BonusInventory.ActivateBonus(this, 2);

            CurrentFuel -= Time.deltaTime;
        }
        
        private void FixedUpdate()
        {
            // Author: Benoit Simon-Turgeon
            if (Time.timeScale != 1f)
                return;
            
            //Author: Seyed-Rahmatoll Javadi
            if (CurrentFuel <= 0 || Temperature <= 0 || Temperature >= 1)
            {
                onPlayerNowSafe.Publish();
                levelFailedEventChannel.Publish(); 
                if(Temperature<0)
                    vehicleFreezeEventChannel.Publish();
                else
                    vehicleHotEventChannel.Publish();
                // Author: Benoit Simon-Turgeon
                return;
            }

            // Author: David Pagotto
            if (!mover.IsGrounded)
            {
                lightAnimator.OnVehicleAirborne();
                if (inputs.Game.Forward.phase == InputActionPhase.Started)
                {
                    if (inputs.UseInvertedRotation)
                        mover.RotateCw();
                    else
                        mover.RotateCcw();
                }
                else if (inputs.Game.Backward.phase == InputActionPhase.Started)
                {
                    if (inputs.UseInvertedRotation)
                        mover.RotateCcw();
                    else
                        mover.RotateCw();
                }
                // Author Benoit Simon-Turgeon
                if (CurrentVehicleZoneState == CurrentZoneState.Neutral && Temperature > baseTemperature)
                    ChangeTemperatureOnAcceleration(-(temperatureChangingSpeed / temperatureChangingSpeedDivider));
            }
            else
            {
                // Author: Benoit Simon-Turgeon
                if (inputs.Game.Brake.phase == InputActionPhase.Started)
                {
                    mover.Brake();
                    //Author: Seyed-Rahmatoll Javadi
                    vehicleAudio.PlayBrakeSoundEffect(mover.Speed, mover.MaximumSpeed);
                }
                else if (inputs.Game.Forward.phase == InputActionPhase.Started)
                {
                    mover.Forward();
                    // Author: David Pagotto
                    lightAnimator.OnVehicleForward();
                }
                // Author: Benoit Simon-Turgeon
                else if (inputs.Game.Backward.phase == InputActionPhase.Started)
                {
                    mover.Backward();
                    // Author: David Pagotto
                    lightAnimator.OnVehicleBackward();
                }

                // Author: Benoit Simon-Turgeon
                if (CurrentVehicleZoneState == CurrentZoneState.Neutral)
                {
                    if (inputs.Game.Forward.phase == InputActionPhase.Started || inputs.Game.Backward.phase == InputActionPhase.Started)
                        ChangeTemperatureOnAcceleration(temperatureChangingSpeed / temperatureChangingSpeedDivider);
                    else if (Temperature > baseTemperature)
                        ChangeTemperatureOnAcceleration(-(temperatureChangingSpeed / temperatureChangingSpeedDivider));
                }
            }
                
            // Author: Félix Bernier
            if(mover.IsAirTiming)
                scoreCalculator.OnVehicleInAir();
            if (!mover.IsAirTiming && scoreCalculator.ScoreCombo != 0)
                scoreCalculator.OnVehicleLanded();

            //Author: Seyed-Rahmatoll Javadi
            vehicleAudio.PlayEngineSoundEffect();
            vehicleAudio.ChangeEngineSoundEffectPitch(mover.Speed/mover.MaximumSpeed);
        }

        //Author: Seyed-Rahmatoll Javadi
        private void LateUpdate()
        {
            Progression = Mathf.InverseLerp(startingPointX, endPoint.transform.position.x, vehicleBody.transform.position.x);
            gameProgressionEventChannel.Publish(this);
            vehicleSpeedChangedEventChannel.Publish(mover.Speed / mover.MaximumSpeed);

            // Author: David Pagotto
            vehicleFuelChangedEventChannel.Publish(CurrentFuel / MaxFuel);
        }

        // Author: Benoit Simon-Turgeon
        public void UpdateTemperature()
        {
            vehicleTemperatureChangedEventChannel.Publish(this);
        }
        
        // Author: Benoit Simon-Turgeon
        private void ChangeTemperatureOnAcceleration(float temperatureChange)
        {
            Temperature += temperatureChange;
            UpdateTemperature();
        }
        
        //Author: Seyed-Rahmatoll Javadi
        public void ChangeTemperature(TemperatureStimulusTypes type)
        {
            // Author: Benoit Simon-Turgeon
            if (inputs.Game.Forward.phase == InputActionPhase.Started || inputs.Game.Backward.phase == InputActionPhase.Started)
                temperatureSpeedMultiplier = temperatureSpeedMultiplierWhenAccelerating;
            else
                temperatureSpeedMultiplier = 1;
            
            //Author: Seyed-Rahmatoll Javadi
            switch (type)
            {
                case TemperatureStimulusTypes.Cold:
                    CurrentVehicleZoneState = CurrentZoneState.Cold;
                    Temperature -= temperatureChangingSpeed * (1/temperatureSpeedMultiplier) * Time.deltaTime;
                    UpdateTemperature();
                    break;
                case TemperatureStimulusTypes.Hot:
                    CurrentVehicleZoneState = CurrentZoneState.Hot;
                    Temperature += temperatureChangingSpeed * temperatureSpeedMultiplier * Time.deltaTime;
                    UpdateTemperature();
                    break;
            }
        }
        
        //Author: Seyed-Rahmatoll Javadi
        public void EnterDangerousZone()
        {
            vehicleEnterDangerousZoneEventChannel.Publish();
        }

        //Author : François-Xavier Bernier
        public void OutOfDangerousZone(CurrentZoneState currentZoneState)
        {
            CurrentVehicleZoneState = currentZoneState;
            onPlayerNowSafe.Publish();
        }

        // Author: Seyed-Rahmatoll Javadi
        public void ImpulseUp(float force)
        {
            mover.ImpulseUp(force);
        }
    }
}