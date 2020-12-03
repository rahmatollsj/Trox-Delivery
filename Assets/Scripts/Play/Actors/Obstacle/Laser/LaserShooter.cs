using Harmony;
using UnityEngine;

namespace Game
{
    // Author: Benoit Simon-Turgeon
    public class LaserShooter: MonoBehaviour
    {
        [Tooltip("Projectile to shoot")]
        [SerializeField] private Transform laser;
        [SerializeField] private float laserSpeed = 3f;
        [SerializeField] private float laserLifetime = 5f;
        
        private SoundEffectsManager soundEffectsManager;

        private void Awake()
        {
            soundEffectsManager = Finder.SoundEffectsManager;
        }

        public void ShootLaser()
        {
            if (transform.childCount <= 0)
                return;

            var shooterPosition = transform.position;
            var shootDirection = (transform.GetChild(0).position - shooterPosition).normalized;
            
            var laserProjectile = Instantiate(laser, shooterPosition, Quaternion.identity);
            laserProjectile.GetComponent<LaserProjectile>().LaserSpawnSetup(shootDirection, laserSpeed, laserLifetime);
            soundEffectsManager.Play(SoundEffectType.Laser);
        }
    }
}