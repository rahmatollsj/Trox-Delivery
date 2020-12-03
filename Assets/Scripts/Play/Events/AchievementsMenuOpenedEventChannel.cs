using Harmony;
using UnityEngine;

namespace Game
{
    //Author: François-Xavier Bernier
    [Findable(Tags.MainController)]
    public class AchievementsMenuOpenedEventChannel : MonoBehaviour
    {
        public event AchievementsMenuOpenedEvent OnAchievementsMenuOpened;

        public void Publish()
        {
            if (OnAchievementsMenuOpened != null)
                OnAchievementsMenuOpened();
        }

        public delegate void AchievementsMenuOpenedEvent();
    }
}