using Harmony;
using UnityEngine;

namespace Game
{
 
    // Author: David Pagotto
    public class ProgressionEventListener : MonoBehaviour
    {
        private ProgressionManager progressionManager;
        private BonusInventory inventory;
        private LevelManager levelManager;
        private LevelSuccessEventChannel levelSuccessEventChannel;
        private SaveChangedEventChannel onSaveChanged;
        //Author: Seyed-Rahmatoll Javadi
        private BonusResetEventChannel bonusResetEventChannel;

        private void Awake()
        {
            levelSuccessEventChannel = Finder.LevelSuccessEventChannel;
            progressionManager = Finder.ProgressionManager;
            levelManager = Finder.LevelManager;
            inventory = Finder.BonusInventory;
            onSaveChanged = Finder.SaveChangedEventChannel;
            levelSuccessEventChannel.OnLevelSuccess += OnLevelSuccess;
            onSaveChanged.OnSaveChanged += RefreshGameState;
            //Author: Seyed-Rahmatoll Javadi
            bonusResetEventChannel = Finder.BonusResetEventChannel;
            bonusResetEventChannel.OnBonusReset += RefreshGameState;
        }

        private void OnDestroy()
        {
            levelSuccessEventChannel.OnLevelSuccess -= OnLevelSuccess;
            onSaveChanged.OnSaveChanged -= RefreshGameState;
            //Author: Seyed-Rahmatoll Javadi
            bonusResetEventChannel.OnBonusReset -= RefreshGameState;
        }

        private void RefreshGameState()
        {
            inventory.Reset();
            for (int i = 0; i < inventory.Slots.Length; i++)
            {
                var savedBonus = progressionManager.ReadBonus(i);
                if (savedBonus != null)
                    inventory.AddBonus(savedBonus);
            }
        }

        private void OnLevelSuccess()
        { 
            progressionManager.UnlockLevel(levelManager.GetNextLevel());
            for (int i = 0; i < inventory.Slots.Length; i++)
                 progressionManager.WriteBonus(i, inventory.Slots[i]);
        }
    }
}
