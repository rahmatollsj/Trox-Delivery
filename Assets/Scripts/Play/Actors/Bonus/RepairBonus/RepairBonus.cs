using UnityEngine;

namespace Game
{
    //Author: Seyed-Rahmatoll Javadi
    public class RepairBonus : Bonus
    {
        [SerializeField] private RepairBonusData bonusData;
        
        public override BonusType BonusType => bonusData.BonusType;
        public override IBonusData Data => bonusData;
        
        protected override void OnBonusActivated()
        {
            Target.Temperature = 0.5f;
            // Author: Benoit Simon-Turgeon
            Target.UpdateTemperature();
        }

        public override void UpdateData(IBonusData data)
        {
            MarkAsCollected();
            bonusData = (RepairBonusData)data;
        }
    }
}