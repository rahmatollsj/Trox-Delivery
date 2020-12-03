using DG.Tweening;
using Harmony;
using System.Collections;
using UnityEngine;

namespace Game {
    // Author: David Pagotto
    public class TowerCinematicTrigger : MonoBehaviour
    {
        [Header("Game Objects")]
        [SerializeField] private Tower tower = null;
        [Tooltip("The cinematic camera used to show the target object")]
        [SerializeField] private new Camera camera = null;
        [Tooltip("The cinematic camera will go to this object's position")]
        [SerializeField] private Transform targetCinematicFocusObject = null;

        [Header("Cinematic")]
        [Range(0, 100)] [SerializeField] private float cinematicOrtographicSize = 20f;
        [Range(0, 10)] [SerializeField] private float cinematicMoveSpeed = 2f;
        [Range(0, 10)] [SerializeField] private float cinematicZoomSpeed = 1f;
        [Tooltip("Time in seconds with which the cinematic will wait once the explosion is triggered")]
        [Range(0, 10)] [SerializeField] private float cinematicExplosionStayTime = 2f;
        [Tooltip("Used for the camera shake when the tower explodes and when it lands on the ground")]
        [Range(0, 1)] [SerializeField] private float cinematicCameraShakeTime = 0.2f;
        [Tooltip("Used for the camera shake when the tower explodes and when it lands on the ground")]
        [Range(0, 100)] [SerializeField] private float cinematicCameraRotationShakeStrength = 3f;
        [Tooltip("Used for the camera shake when the tower explodes and when it lands on the ground")]
        [Range(0, 100)] [SerializeField] private float cinematicCameraPositionShakeStrength = 10f;

        private VehicleMover vehicleMover;
        private FollowCamera followCamera;
        private bool cinematicActivated = false;

        private Vector3 CinematicTargetPosition
        {
            get
            {
                Vector3 position = targetCinematicFocusObject.position;
                position.z = camera.transform.position.z;
                return position;
            }
        }

        private void Awake()
        {
            vehicleMover = Finder.Vehicle.GetComponent<VehicleMover>();
            followCamera = camera.GetComponent<FollowCamera>();
        }

        private IEnumerator TowerExplosionRoutine()
        {
            vehicleMover.Freeze = true;
            followCamera.enabled = false;

            var initialOrthographicSize = camera.orthographicSize;
            var initialPosition = camera.transform.position;

            Sequence cinematicSequence = DOTween.Sequence();
            // Déplacement de la caméra vers la tour
            cinematicSequence.Append(camera.DOOrthoSize(cinematicOrtographicSize, cinematicZoomSpeed))
                .Join(camera.transform.DOMove(CinematicTargetPosition, cinematicMoveSpeed))
                // Exécution des particules d'explosion et la chute de la tour
                .AppendCallback(tower.Explode)
                .Join(camera.DOShakePosition(cinematicCameraShakeTime, cinematicCameraPositionShakeStrength))
                .Join(camera.DOShakeRotation(cinematicCameraShakeTime, cinematicCameraRotationShakeStrength))
                // Courte attente pour laisser le temps de voir l'explosion
                .AppendInterval(cinematicExplosionStayTime)
                // On retourne à la position initiale du joueur
                .Append(camera.DOOrthoSize(initialOrthographicSize, cinematicZoomSpeed))
                .Join(camera.transform.DOMove(initialPosition, cinematicMoveSpeed))
                .WaitForCompletion();

            yield return cinematicSequence.WaitForCompletion();

            followCamera.enabled = true;
            vehicleMover.Freeze = false;

            yield return new WaitUntil(() => tower.HasTouchedDown);

            camera.DOShakePosition(cinematicCameraShakeTime, cinematicCameraPositionShakeStrength);
            camera.DOShakeRotation(cinematicCameraShakeTime, cinematicCameraRotationShakeStrength);
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (!collider.CompareTag(Tags.VehicleBody) || cinematicActivated)
                return;

            cinematicActivated = true;
            StartCoroutine(TowerExplosionRoutine());
        }
    }
}