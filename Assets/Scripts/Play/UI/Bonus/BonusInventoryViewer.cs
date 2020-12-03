using Harmony;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    // Author: David Pagotto
    public class BonusInventoryViewer : MonoBehaviour
    {
        private BonusInventorySlot[] slots = null;
        private BonusInventory inventory = null;
        
        private void Awake()
        {
            inventory = Finder.BonusInventory;

            Finder.BonusCollectedEventChannel.OnBonusCollected += OnBonusInventoryUpdate;
            Finder.BonusFinishedEventChannel.OnBonusFinished += OnBonusInventoryUpdate;
            Finder.BonusActivatedEventChannel.OnBonusActivated += OnBonusInventoryUpdate;

            slots = transform.Children()
                .Select(s => s.GetComponent<BonusInventorySlot>())
                .Where(s => s != null)
                .ToArray();

            // On force les icônes à se mettre à jour.
            OnBonusInventoryUpdate(null);
        }
        
        private void OnDestroy()
        {
            // Parfois quand on quitte le jeu Main est supprimé avant que OnDestroy soit appelé.
            if (Finder.Main)
            {
                Finder.BonusCollectedEventChannel.OnBonusCollected -= OnBonusInventoryUpdate;
                Finder.BonusFinishedEventChannel.OnBonusFinished -= OnBonusInventoryUpdate;
                Finder.BonusActivatedEventChannel.OnBonusActivated -= OnBonusInventoryUpdate;
            }
        }
        
        private Sprite GetBonusSprite(BonusType type)
        {
            return Bonus.GetBonusPrefab(type).GetComponent<SpriteRenderer>().sprite;
        } 
        
        private void OnBonusInventoryUpdate(Bonus bonus)
        {
            for (int i = 0; i < inventory.Slots.Length; i++)
            {
                if (inventory.Slots[i] != null)
                {
                    slots[i].Icon = GetBonusSprite(inventory.Slots[i].BonusType);
                    slots[i].HasItem = true;
                }
                else
                {
                    slots[i].HasItem = false;
                }
            }
        }
    }
}