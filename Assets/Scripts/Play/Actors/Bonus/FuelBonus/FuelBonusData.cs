using System;
using System.Runtime.Serialization;

namespace Game
{
    // Author: David Pagotto
    [Serializable]
    public class FuelBonusData : IBonusData
    {
        public BonusType BonusType => BonusType.Fuel;
        public float FuelValue = 45f;

        public IBonusData Clone()
        {
            var other = new FuelBonusData();
            other.FuelValue = FuelValue;
            return other;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("FuelValue", FuelValue, typeof(float));
        }

        public FuelBonusData(SerializationInfo info, StreamingContext context)
        {
            FuelValue = (float)info.GetValue("FuelValue", typeof(float));
        }
        public FuelBonusData() { }
    }
}
