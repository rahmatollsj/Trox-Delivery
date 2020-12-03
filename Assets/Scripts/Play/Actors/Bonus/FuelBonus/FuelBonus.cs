using UnityEngine;

namespace Game
{
    // Author: David Pagotto
    public class FuelBonus : Bonus
    {
        [SerializeField] private FuelBonusData bonusData;
        public override BonusType BonusType => bonusData.BonusType;
        public override IBonusData Data => bonusData;

        protected override void OnBonusActivated()
        {
            Target.CurrentFuel = Mathf.Clamp(Target.CurrentFuel + bonusData.FuelValue, 0, Target.MaxFuel);
        }
        
        public override void UpdateData(IBonusData data)
        {
            MarkAsCollected();
            bonusData = (FuelBonusData)data;
        }
    }
}
