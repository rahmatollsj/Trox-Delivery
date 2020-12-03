using Harmony;
using System;
using UnityEngine;

namespace Game
{
    // Author: David Pagotto
    [Findable(Tags.MainController)]
    public class ProgressionManager : MonoBehaviour
    {
        [SerializeField] private string saveFilePath = "save{0}.dat";

        private SaveFile activeSaveFile = null;
        private SaveChangedEventChannel saveChangedEventChannel;

        public void ChangeSaveFile(uint index)
        {
            activeSaveFile?.Close();
            activeSaveFile = new SaveFile(String.Format(saveFilePath, index));
            saveChangedEventChannel.Publish();
        }

        public void UnlockLevel(string level)
            => activeSaveFile.SaveData(SaveTag.Level, true, level);

        public bool HasUnlockedLevel(string level)
            => activeSaveFile.ReadData<bool>(SaveTag.Level, level);

        // Author: Félix Bernier
        public int[] ReadAchievementsProgress()
            => activeSaveFile.ReadData<int[]>(SaveTag.Achievement);

        // Author: Félix Bernier
        public void WriteAchievementsProgress(int[] achievements)
            => activeSaveFile.SaveData(SaveTag.Achievement, achievements);

        public IBonusData ReadBonus(int slot)
            => activeSaveFile.ReadData<IBonusData>(SaveTag.Bonus, slot.ToString());

        public void WriteBonus(int slot, IBonusData bonus)
            => activeSaveFile.SaveData(SaveTag.Bonus, bonus, slot.ToString());

        private void Awake()
        {
            saveChangedEventChannel = Finder.SaveChangedEventChannel;
            ChangeSaveFile(1);
        }

        private void OnDestroy()
        {
            activeSaveFile.Close();
        }
    }
}
