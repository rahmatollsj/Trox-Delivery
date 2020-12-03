using UnityEngine;

namespace Game
{
    //Author : François-Xavier Bernier
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/AchievementBannerManager", order = 1)]
    public class AchievementBannerManager : ScriptableObject
    {
        [SerializeField] public string[] title;
        [SerializeField] public string[] description;
    }
}