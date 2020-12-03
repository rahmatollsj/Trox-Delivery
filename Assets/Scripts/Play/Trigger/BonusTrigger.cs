using Harmony;
using UnityEngine;

namespace Game
{
    // Author: David Pagotto
    public class BonusTrigger : MonoBehaviour
    {
        private BonusCollectedEventChannel bonusCollectedEventChannel;
        private Bonus bonus;

        private void Awake()
        {
            bonusCollectedEventChannel = Finder.BonusCollectedEventChannel;
            bonus = gameObject.GetComponent<Bonus>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var vehicle = other?.gameObject?.GetComponentInParent<Vehicle>();
            if (vehicle != null)
                bonusCollectedEventChannel.Publish(bonus);
        }
    }
}
