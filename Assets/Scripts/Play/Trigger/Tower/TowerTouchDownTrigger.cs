using Harmony;
using UnityEngine;

namespace Game
{
    // Author: David Pagotto
    public class TowerTouchDownTrigger : MonoBehaviour
    {
        [SerializeField] private Tower tower;

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.CompareTag(Tags.Terrain))
                tower.HasTouchedDown = true;
        }
    }
}