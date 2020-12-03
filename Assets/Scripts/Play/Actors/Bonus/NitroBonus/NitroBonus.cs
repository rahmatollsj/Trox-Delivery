using System.Collections;
using Harmony;
using UnityEngine;

namespace Game
{
    // Author: Benoit Simon-Turgeon
    public class NitroBonus : Bonus
    {
        [SerializeField] private NitroBonusData bonusData;
        
        private BonusNitroChangedEventChannel bonusNitroChangedEventChannel;
        
        public override BonusType BonusType => bonusData.BonusType;
        public override IBonusData Data => bonusData;
        

        protected override void OnBonusActivated()
        {
            StartCoroutine(ActivateNitroBonus());
        }
        
        private IEnumerator ActivateNitroBonus()
        {
            bonusNitroChangedEventChannel.Publish(bonusData.SpeedMultiplier);
            yield return new WaitForSeconds(bonusData.BoostDuration);
            bonusNitroChangedEventChannel.Publish(1f);
        }
        
        protected override void InitializeBonus()
        {
            bonusNitroChangedEventChannel = Finder.BonusNitroChangedEventChannel;
        }
        
        public override void UpdateData(IBonusData data)
        {
            MarkAsCollected();
            bonusData = (NitroBonusData)data;
        }
    }
}