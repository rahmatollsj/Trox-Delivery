using System;
using System.Runtime.Serialization;
using UnityEngine;

namespace Game
{
    // Author: Benoit Simon-Turgeon
    [Serializable]
    public class NitroBonusData : IBonusData
    {
        [Tooltip("Speed multiplier during the boost duration")] 
        [SerializeField] private float speedMultiplier = 5;

        [Tooltip("Duration of the boost")] 
        [SerializeField] private float boostDuration = 3;
        
        public BonusType BonusType => BonusType.Nitro;
        
        public float SpeedMultiplier => speedMultiplier;
        public float BoostDuration => boostDuration;


        public IBonusData Clone()
        {
            return new NitroBonusData
            {
                boostDuration = boostDuration, 
                speedMultiplier = speedMultiplier
            };
        }

        // Author: David Pagotto
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("speedMultiplier", speedMultiplier, typeof(float));
            info.AddValue("boostDuration", boostDuration, typeof(float));
        }

        // Author: David Pagotto
        public NitroBonusData(SerializationInfo info, StreamingContext context)
        {
            speedMultiplier = (float)info.GetValue("speedMultiplier", typeof(float));
            boostDuration = (float)info.GetValue("boostDuration", typeof(float));
        }

        public NitroBonusData() { }
    }
}