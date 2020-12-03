using Harmony;
using UnityEngine;

namespace Game
{
    // Author: David Pagotto
    [Findable(Tags.MainController)]
    public class BonusCollectedEventChannel : MonoBehaviour
    {
        public delegate void BonusCollectedEvent(Bonus bonus);
        public event BonusCollectedEvent OnBonusCollected;

        public void Publish(Bonus bonusOwner) 
            => OnBonusCollected?.Invoke(bonusOwner);
    }
}
