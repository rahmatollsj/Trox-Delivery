using System.Collections;
using UnityEngine;

namespace Game
{
    // Author: Benoit Simon-Turgeon
    public class UFO : MonoBehaviour
    {
        [Tooltip("Time between each shot")] 
        [SerializeField] private float laserShootFrequency = 4f;
        
        private LaserShooter[] laserShooters;
        
        private bool isReadyToShoot = true;

        private void Awake()
        {
            laserShooters = gameObject.GetComponentsInChildren<LaserShooter>();
        }

        private void FixedUpdate()
        {
            if (isReadyToShoot)
                StartCoroutine(SendShoot());
        }

        private IEnumerator SendShoot()
        {
            isReadyToShoot = false;

            foreach (var laserShooter in laserShooters)
                laserShooter.ShootLaser();

            yield return new WaitForSeconds(laserShootFrequency);
            isReadyToShoot = true;
        }
    }
}