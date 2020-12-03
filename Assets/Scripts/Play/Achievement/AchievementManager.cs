using System.Collections.Generic;
using Harmony;
using UnityEngine;

namespace Game
{
    [Findable(Tags.MainController)]
    public class AchievementManager : MonoBehaviour
    {
        [SerializeField] private GameObject prefabToSpawn;
        [SerializeField] private int nbDeathToDo = 3;
        public bool[] IsAchievementDone { get; private set; }
        // Author: Benoit Simon-Turgeon
        private Queue<AchievementTypes> achievementToUnlock = new Queue<AchievementTypes>();
        private Achievement lastAchievement;
        // Author: Félix Bernier
        public AchievementTypes LastAchievementType { get; private set; }
        //Author: Félix Bernier
        private BonusCollectedEventChannel bonusCollectedEventChannel;
        private VehicleFlippedEventChannel vehicleFlippedEventChannel;
        private VehicleCrashedEventChannel vehicleCrashedEventChannel;
        private LevelSuccessEventChannel levelSuccessEventChannel;
        private SaveChangedEventChannel saveChangedEventChannel;
        private VehicleHotEventChannel vehicleHotEventChannel;
        private VehicleFreezeEventChannel vehicleFreezeEventChannel;
        // Author: Benoit Simon-Turgeon
        private HighJumpSuccessfulEventChannel highJumpSuccessfulEventChannel;
        //Author: Seyed-Rahmatoll Javadi
        private LevelFailedEventChannel levelLostEventChannel;
        //Author : François-Xavier Bernier
        private SlightProblemEventChannel slightProblemEventChannel;
        private ProgressionManager progressionManager;
        private int[] Achievements { get; set; }
        private int[] minimalCountPerAchievements;
        // Author: Benoit Simon-Turgeon
        private bool isBannerDown = true;
        
        private void Awake()
        {
            //Author: Félix Bernier
            Achievements = new int[(int)AchievementTypes.NumberOfTypes];
            minimalCountPerAchievements = new int[(int)AchievementTypes.NumberOfTypes];
            IsAchievementDone = new bool[(int) AchievementTypes.NumberOfTypes];
            progressionManager = Finder.ProgressionManager;
            
            InitiateMinimalCountArray();
            
            vehicleHotEventChannel = Finder.VehicleHotEventChannel;
            vehicleHotEventChannel.OnVehicleHot += OnVehicleHot;
            saveChangedEventChannel = Finder.SaveChangedEventChannel;
            saveChangedEventChannel.OnSaveChanged += OnSaveChanged;
            levelSuccessEventChannel = Finder.LevelSuccessEventChannel;
            levelSuccessEventChannel.OnLevelSuccess += OnGameFinished;
            vehicleFreezeEventChannel = Finder.VehicleFreezeEventChannel;
            vehicleFreezeEventChannel.OnVehicleFreeze += OnVehicleFreeze;
            vehicleCrashedEventChannel = Finder.VehicleCrashedEventChannel;
            vehicleCrashedEventChannel.OnVehicleCrashed += OnVehicleCrashed;
            bonusCollectedEventChannel = Finder.BonusCollectedEventChannel;
            bonusCollectedEventChannel.OnBonusCollected += OnBonusCollected;
            vehicleFlippedEventChannel = Finder.VehicleFlippedEventChannel;
            vehicleFlippedEventChannel.OnVehicleFlipped += OnVehicleFlipped;
            // Author: Benoit Simon-Turgeon
            highJumpSuccessfulEventChannel = Finder.HighJumpSuccessfulEventChannel;
            highJumpSuccessfulEventChannel.OnHighJumpSuccessful += OnHighJumpSuccessful;
            //Author : François-Xavier Bernier
            slightProblemEventChannel = Finder.SlightProblemEventChannel;
            slightProblemEventChannel.OnRockHitVehicle += OnRockHitVehicle;
            levelLostEventChannel = Finder.LevelFailedEventChannel;
            levelLostEventChannel.OnLevelFailedEventChannel += OnLevelFailed;
        }
        
        private void OnDestroy()
        {
            bonusCollectedEventChannel.OnBonusCollected -= OnBonusCollected;
            vehicleFlippedEventChannel.OnVehicleFlipped -= OnVehicleFlipped;
            vehicleCrashedEventChannel.OnVehicleCrashed -= OnVehicleCrashed;
            levelSuccessEventChannel.OnLevelSuccess -= OnGameFinished;
            // Author: Benoit Simon-Turgeon
            highJumpSuccessfulEventChannel.OnHighJumpSuccessful -= OnHighJumpSuccessful;
            //Author: Seyed-Rahmatoll Javadi
            levelLostEventChannel.OnLevelFailedEventChannel -= OnLevelFailed;
        }

        private void Start()
        {
            //Author: Félix Bernier
            UnloadAchievements();
        }

        // Author: Benoit Simon-Turgeon
        private void FixedUpdate()
        {
            isBannerDown = lastAchievement == null || lastAchievement.IsDone;
                
                if (isBannerDown)
                    if(achievementToUnlock.Count > 0)
                        InstantiateAchievement(achievementToUnlock.Dequeue());
        }

        //Author: Félix Bernier
        private void InitiateMinimalCountArray()
        {
            for (int i = 0; i < (int) AchievementTypes.NumberOfTypes; i++)
                minimalCountPerAchievements[i] = 1;
            minimalCountPerAchievements[(int) AchievementTypes.TimeForRoadRage] = nbDeathToDo;
        }

        //Author: Félix Bernier
        private void OnSaveChanged()
        {
            UnloadAchievements();
        }
        
        private void OnGameFinished()
        {
            Achievements[(int) AchievementTypes.TimeForRoadRage] = 0;
            //Author: Félix Bernier
            switch (Finder.LevelManager.CurrentLevel.name)
            {
                case Scenes.Desert:
                    InstantiateAchievement(AchievementTypes.InTheSandSchemeOfThings);
                    break;
                case Scenes.Jungle:
                    InstantiateAchievement(AchievementTypes.KeepingSteady);
                    break;
                case Scenes.Moon:
                    InstantiateAchievement(AchievementTypes.ZeroG);
                    break;
                case Scenes.Arctic:
                    InstantiateAchievement(AchievementTypes.Sherpa);
                    break;
                case Scenes.Volcan:
                    InstantiateAchievement(AchievementTypes.OnFire);
                    break;
                default:
                    InstantiateAchievement(AchievementTypes.GoodLuck);
                    break;
            }
        }

        //Author: Félix Bernier
        private bool AchievementIsDone()
        {
            lastAchievement = Instantiate(prefabToSpawn, transform.position, Quaternion.identity)/*Benoit ->*/.GetComponentInChildren<Achievement>();
            return lastAchievement.IsDone;
        }

        //Author: Félix Bernier
        private void InstantiateAchievement(AchievementTypes type)
        {
            // Author: Benoit Simon-Turgeon
            if (!isBannerDown)
            {
                achievementToUnlock.Enqueue(type);
                return;
            }
            
            // Author: Félix Bernier
            Achievements[(int) type]++;
            if (Achievements[(int) type] >= minimalCountPerAchievements[(int) type] && IsAchievementDone[(int) type] == false)
            {
                IsAchievementDone[(int) type] = true;
                LastAchievementType = type;
                isBannerDown = /*<- Benoit*/AchievementIsDone();
            }
            SaveAchievements();
        }
        
        //Author: Félix Bernier
        private void UnloadAchievements()
        {
            ResetData();
            if (progressionManager.ReadAchievementsProgress() != null)
            {
                Achievements = progressionManager.ReadAchievementsProgress();
                for (int i = 0; i < Achievements.Length; i++)
                {
                    if (Achievements[i] >= minimalCountPerAchievements[i] && IsAchievementDone[i] == false)
                    {
                        IsAchievementDone[i] = true;
                    }
                }
            }
        }

        //Author: Félix Bernier
        private void ResetData()
        {
            for (int i = 0; i < Achievements.Length; i++)
            {
                IsAchievementDone[i] = false;
                Achievements[i] = 0;
            }
        }

        //Author: Félix Bernier
        private void SaveAchievements()
        {
            progressionManager.WriteAchievementsProgress(Achievements);
        }
        
        //Author: Félix Bernier
        private void OnBonusCollected(Bonus bonus)
        {
            if(bonus.BonusType == BonusType.Fuel)
                InstantiateAchievement(AchievementTypes.BigFuelTank);
        }
        
        //Author: Seyed-Rahmatoll Javadi
        private void OnLevelFailed()
        {
            InstantiateAchievement(AchievementTypes.TimeForRoadRage);
        }

        //Author : François-Xavier Bernier
        private void OnRockHitVehicle()
        {
            InstantiateAchievement(AchievementTypes.SlightProblem);
        }
        
        //Author: Félix Bernier
        private void OnVehicleCrashed()
        {
            InstantiateAchievement(AchievementTypes.BadDriver);
        }

        //Author: Félix Bernier
        private void OnVehicleFlipped()
        {
            InstantiateAchievement(AchievementTypes.Barrel);
        }
        
        //Author: Benoit Simon-Turgeon
        private void OnHighJumpSuccessful()
        {
            InstantiateAchievement(AchievementTypes.HighJump);
        }
        
        private void OnVehicleHot()
        {
            InstantiateAchievement(AchievementTypes.HeavyFoot);
        }

        private void OnVehicleFreeze()
        {
            InstantiateAchievement(AchievementTypes.GoFasterKaren);
        }
    }
}