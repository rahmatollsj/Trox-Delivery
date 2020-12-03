using Harmony;
using System;
using System.Linq;
using UnityEngine;

namespace Game
{
    // Author: David Pagotto
    [Findable(Tags.MainController)]
    public class BonusInventory : MonoBehaviour
    {
        private const int NumBonusSlots = 3;

        public IBonusData[] Slots { get; } = new IBonusData[NumBonusSlots];
        private int AvailableSlots => Slots.Count((s) => s == null);

        private void Awake()
        {
            Finder.BonusCollectedEventChannel.OnBonusCollected += OnBonusCollected;
            Finder.BonusFinishedEventChannel.OnBonusFinished += OnBonusFinished;
        }

        private void OnDestroy()
        {
            Finder.BonusCollectedEventChannel.OnBonusCollected -= OnBonusCollected;
            Finder.BonusFinishedEventChannel.OnBonusFinished -= OnBonusFinished;
        }

        private void ActivateBonus(int index)
        {
            var data = Slots[index];
            Bonus bonus = Finder.BonusFactory.Create(data.BonusType, Vector3.zero);
            Slots[index] = null;
            bonus.UpdateData(data);
            bonus.TriggerBonus(Finder.Vehicle);
        }

        private void OnBonusFinished(Bonus bonus)
            => RemoveBonus(bonus);

        private void OnBonusCollected(Bonus bonus) 
            => AddBonus(bonus);

        public void ActivateBonus(Vehicle vehicle, int index)
        {
            if (index < 0 || index >= Slots.Length)
                throw new Exception("Invalid bonus index (out of bounds)");
            if (Slots[index] != null)
                ActivateBonus(index);
        }

        public void AddBonus(IBonusData data)
        {
            if (data == null)
                throw new NullReferenceException("Bonus data can't be null.");
            // Aucun espace disponible, donc le dernier bonus devient le bonus récolté.
            if (AvailableSlots == 0)
            {
                Slots[NumBonusSlots - 1] = data.Clone();
                return;
            }

            for (int i = 0; i < Slots.Length; i++)
            {
                if (Slots[i] == null)
                {
                    Slots[i] = data.Clone();
                    return;
                }
            }
        }

        private void AddBonus(Bonus bonus)
        {
            if (bonus == null)
                throw new NullReferenceException("Bonus can't be null.");
            AddBonus(bonus.Data);
        }

        private void RemoveBonus(Bonus bonus)
        {
            for (int i = 0; i < Slots.Length; i++)
                if (Slots[i] == bonus.Data)
                    Slots[i] = null;
        }

        public void Reset()
        {
            for (int i = 0; i < Slots.Length; i++)
                Slots[i] = null;
        }
    }
}
