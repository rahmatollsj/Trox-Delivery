using System;
using System.Runtime.Serialization;

namespace Game
{
    //Author: Seyed-Rahmatoll Javadi
    [Serializable]
    public class RepairBonusData : IBonusData
    {
        public BonusType BonusType => BonusType.Repair;
        
        public IBonusData Clone()
        {
            var other = new RepairBonusData();
            return other;
        }

        // Author: David Pagotto
        public void GetObjectData(SerializationInfo info, StreamingContext context) { }

        // Author: David Pagotto
        public RepairBonusData(SerializationInfo info, StreamingContext context) { }

        public RepairBonusData() { }
    }
}