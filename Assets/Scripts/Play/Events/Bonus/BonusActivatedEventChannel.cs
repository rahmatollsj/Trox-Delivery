using Harmony;
using UnityEngine;

namespace Game
{
    // Author: David Pagotto
    [Findable(Tags.MainController)]
    public class BonusActivatedEventChannel : MonoBehaviour
    {
        // Author: David Pagotto
        public delegate void BonusActivatedEvent(Bonus bonus);
        public event BonusActivatedEvent OnBonusActivated;

        public void Publish(Bonus sender)
            => OnBonusActivated?.Invoke(sender);
    }
}
