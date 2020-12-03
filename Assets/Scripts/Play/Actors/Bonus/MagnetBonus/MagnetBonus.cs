using System.Collections;
using UnityEngine;

namespace Game
{
    // Author: Benoit Simon-Turgeon
    public class MagnetBonus : Bonus
    {
        [SerializeField] private MagnetBonusData bonusData;
        
        public override BonusType BonusType => bonusData.BonusType;
        public override IBonusData Data => bonusData;

        protected override void OnBonusActivated()
        {
            StartCoroutine(ActiveMagnetBonus());
        }
        
        private IEnumerator ActiveMagnetBonus()
        {
            var magnet = Target.VehicleBody.GetComponentInChildren<MagnetRange>();
            magnet.IsMagnetActive = true;
            yield return new WaitForSeconds(bonusData.BonusDuration);
            magnet.IsMagnetActive = false;
        }
        
        public override void UpdateData(IBonusData data)
        {
            MarkAsCollected();
            bonusData = (MagnetBonusData)data;
        }
    }
}