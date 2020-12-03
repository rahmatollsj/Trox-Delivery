using System;
using System.Runtime.Serialization;
using UnityEngine;

namespace Game
{
    // Author: Benoit Simon-Turgeon
    [Serializable]
    public class MagnetBonusData : IBonusData
    {
        [Tooltip("Duration of the boost")] 
        [SerializeField] private float bonusDuration = 10;
        
        public BonusType BonusType => BonusType.Magnet;
        public float BonusDuration => bonusDuration;

        public IBonusData Clone()
        {
            return new MagnetBonusData
            {
                bonusDuration = bonusDuration
            };
        }

        // Author: David Pagotto
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("bonusDuration", bonusDuration, typeof(float));
        }

        // Author: David Pagotto
        public MagnetBonusData(SerializationInfo info, StreamingContext context)
        {
            bonusDuration = (float)info.GetValue("bonusDuration", typeof(float));
        }
        
        public MagnetBonusData() { }
    }
}