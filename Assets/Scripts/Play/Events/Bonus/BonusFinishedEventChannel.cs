using Harmony;
using UnityEngine;

namespace Game
{
    // Author: David Pagotto
    [Findable(Tags.MainController)]
    public class BonusFinishedEventChannel : MonoBehaviour
    {
        public delegate void BonusFinishedEvent(Bonus bonus);
        public event BonusFinishedEvent OnBonusFinished;

        public void Publish(Bonus bonusOwner)
            => OnBonusFinished?.Invoke(bonusOwner);
    }
}
