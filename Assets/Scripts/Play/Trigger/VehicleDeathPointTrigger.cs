using System;
using Harmony;
using UnityEngine;

namespace Game
{
    // Author: Félix Bernier
    public class VehicleDeathPointTrigger : MonoBehaviour
    {
        [SerializeField] private LayerMask groundLayers;

        private LevelFailedEventChannel levelFailedEventChannel;
        private VehicleCrashedEventChannel vehicleCrashedEventChannel;
        
        private void Start()
        {
            levelFailedEventChannel = Finder.LevelFailedEventChannel;
            vehicleCrashedEventChannel = Finder.VehicleCrashedEventChannel;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if ((groundLayers & (1 << other.gameObject.layer)) > 0)
            {
                levelFailedEventChannel.Publish();
                vehicleCrashedEventChannel.Publish();
            }
        }
    }
}