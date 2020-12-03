using Harmony;
using UnityEngine;

namespace Game
{
    // Author: David Pagotto
    [Findable(Tags.MainController)]
    public class BonusFactory : MonoBehaviour
    {
        public Bonus Create(BonusType type, Vector3 position)
        {
            return Object.Instantiate(Bonus.GetBonusPrefab(type), position, Quaternion.identity)
                .GetComponent<Bonus>();
        }
    }
}
