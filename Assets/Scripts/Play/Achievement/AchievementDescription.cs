using Harmony;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    //Author : François-Xavier Bernier
    public class AchievementDescription : MonoBehaviour
    {
        [SerializeField] Color imgColorLocked;
        [SerializeField] private Color imgColorUnLocked;
        
        private TextMeshProUGUI titleText;
        private TextMeshProUGUI descriptionText;
        
        private Image imageAchievement;
        
        private void Awake()
        {
            imageAchievement = GetComponent<Image>();
            titleText = transform?.Find(GameObjects.TitleAch).GetComponent<TextMeshProUGUI>();
            descriptionText = transform?.Find(GameObjects.DescriptionText).GetComponent<TextMeshProUGUI>();
            imageAchievement.color = new Color(imgColorLocked.r, imgColorLocked.g, imgColorLocked.b);
        }

        public void SetLockedColor()
        {
            if (imageAchievement != null)
            {
                imageAchievement.color = new Color(imgColorLocked.r, imgColorLocked.g, imgColorLocked.b);
            }
        }

        public void SetUnlockedColor()
        {
            if (imageAchievement != null)
            {
                imageAchievement.color = new Color(imgColorUnLocked.r, imgColorUnLocked.g, imgColorUnLocked.b);
            }
        }

        public void SetInfos(string title, string description)
        {
            titleText.text = title;
            descriptionText.SetText(description);
        }
    }
}