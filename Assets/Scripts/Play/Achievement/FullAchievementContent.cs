using System.Collections.Generic;
using Harmony;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game
{
    //Author : François-Xavier Bernier
    public class FullAchievementContent : MonoBehaviour
    {
        public AchievementBannerManager achBannerManager;

        private List<AchievementDescription> allAchievements;
        private GameObject achievementToAdd;
        private AchievementManager achievementManager;
        private AchievementsMenuOpenedEventChannel achievementsMenuOpenedEventChannel;

        private void Awake()
        {
            achievementManager = Finder.AchievementManager;
            achievementsMenuOpenedEventChannel = Finder.AchievementsMenuOpenedEventChannel;
            achievementsMenuOpenedEventChannel.OnAchievementsMenuOpened += OnMenuOpened;
            achievementToAdd = gameObject;
            allAchievements = new List<AchievementDescription>();
            InstantiateChildBanners();
            CreateList();
        }

        private void OnMenuOpened()
        {
            int i = 0;
            foreach (var achievement in allAchievements)
            {
                if (achievementManager.IsAchievementDone[i])
                    achievement.SetUnlockedColor();
                else
                    achievement.SetLockedColor();
                i++;
            }
        }

        private void Start()
        {
            InstantiateNewBannersInfo();
        }

        private void CreateList()
        {
            AchievementDescription[] achivementsArray = gameObject.GetComponentsInChildren<AchievementDescription>();
            if (achivementsArray == null)
                return;

            foreach (AchievementDescription achChild in achivementsArray)
            {
                if (achChild != null && transform.gameObject != null)
                    allAchievements.Add(achChild);
            }
        }

        private void InstantiateNewBannersInfo()
        {
            int i = 0;
            foreach (var achievement in allAchievements)
            {
                achievement.SetInfos(achBannerManager.title[i], achBannerManager.description[i]);
                i++;
            }
        }

        private void InstantiateChildBanners()
        {
            for (int i = 0; i < (int) AchievementTypes.NumberOfTypes; i++)
            {
                var obj = Instantiate(Prefabs.AchievementsDescriptionGO, transform, true);
            }
        }
    }
}