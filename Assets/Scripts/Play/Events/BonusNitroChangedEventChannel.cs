using Harmony;
using UnityEngine;

namespace Game
{
    // Author: Benoit Simon-Turgeon
    [Findable(Tags.GameController)]
    public class BonusNitroChangedEventChannel : MonoBehaviour
    {
        public event BonusNitroChanged OnNitroChanged;

        public void Publish(float speedMultiplier)
        {
            OnNitroChanged?.Invoke(speedMultiplier);
        }

        public delegate void BonusNitroChanged(float speedMultiplier);
    }
}