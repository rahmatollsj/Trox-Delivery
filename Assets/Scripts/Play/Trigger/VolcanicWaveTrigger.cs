using UnityEngine;

namespace Game
{
    // Author :François-Xavier Bernier
    public class VolcanicWaveTrigger : MonoBehaviour
    {
        [SerializeField] private VolcanicWave volcanicWave;

        private void OnTriggerEnter2D(Collider2D other)
        {
            var vehicle = other?.gameObject.GetComponentInParent<Vehicle>();
            if (vehicle != null)
            {
                volcanicWave.ActiveWave();
            }
        }
    }
}