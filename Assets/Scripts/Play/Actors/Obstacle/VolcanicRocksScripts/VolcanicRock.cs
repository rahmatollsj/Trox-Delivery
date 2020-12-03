using Harmony;
using UnityEngine;
using DG.Tweening;

namespace Game
{
    //Author : François-Xavier Bernier
    public class VolcanicRock : MonoBehaviour
    {
        [Tooltip("Rock will disappear if it touches an object with any of these tags")]
        [SerializeField] private LayerMask vehicleTagsForCollision;
        
        [Tooltip("The camera used to show the impact")]
        [SerializeField] private new Camera camera = null;
        
        [Range(0, 1)] [SerializeField] private float explosionCameraShakeTime = 0.4f;
        
        [Range(0, 100)] [SerializeField] private float explosionCameraRotationShakeStrength = 3f;
       
        [Range(0, 100)] [SerializeField] private float explosionCameraPositionShakeStrength = 5f;
        
        [SerializeField] private float gravityEffectOnRock = 0.45f;
        [SerializeField]private float basicShakeStrength = 1; 
        [SerializeField]private float carShakeStrength = 3;
        private float ShakeMultiplier { get; set; }
        private Rigidbody2D rigidBody;
        private ComplexVolcanicRock compVolcanicRock;
        private ParticleSystem volRocParticleSystem;
        private SoundEffectsManager soundEffectsManager;
        private SlightProblemEventChannel slightProblemEventChannel;
        private FollowCamera followCamera;


        private void Awake()
        {
            followCamera = camera.GetComponent<FollowCamera>();
            rigidBody = GetComponentInChildren<Rigidbody2D>();
            compVolcanicRock = GetComponentInParent<ComplexVolcanicRock>();
            volRocParticleSystem = compVolcanicRock.GetComponentInChildren<ParticleSystem>();
            soundEffectsManager = Finder.SoundEffectsManager;
            slightProblemEventChannel = Finder.SlightProblemEventChannel;
            ShakeMultiplier = basicShakeStrength;
        }

        public void ActivateRock()
        {
            rigidBody.bodyType = RigidbodyType2D.Dynamic;
            rigidBody.gravityScale = gravityEffectOnRock;
            soundEffectsManager.Play(SoundEffectType.VolcanoDownFall);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (rigidBody.IsTouchingLayers(vehicleTagsForCollision))
            {
                ShakeMultiplier = carShakeStrength;
                Explode();
                gameObject.SetActive(false);
                slightProblemEventChannel.Publish();
                ShakeMultiplier = basicShakeStrength;
            }
        }

        public void Explode()
        {
            if (volRocParticleSystem != null)
            {   soundEffectsManager.Play(SoundEffectType.VolcanoRockExplosion);
                followCamera.transform.DOShakePosition(explosionCameraShakeTime*ShakeMultiplier, explosionCameraPositionShakeStrength*ShakeMultiplier);
                followCamera.transform.DOShakeRotation(explosionCameraShakeTime*ShakeMultiplier, explosionCameraRotationShakeStrength*ShakeMultiplier);
                volRocParticleSystem.transform.position = rigidBody.transform.position;
                volRocParticleSystem.Play();
                gameObject.SetActive(false);
            }
        }
    }
}