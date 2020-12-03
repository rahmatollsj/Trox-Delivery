using Harmony;
using UnityEngine;

namespace Game
{
    //Author: Seyed-Rahmatoll Javadi
    public class SignStimulus : MonoBehaviour
    {
        [SerializeField] private FollowCamera gameCamera;
        [SerializeField] private float zoom = 1.5f;
        [SerializeField] private float zoomSpeed = 4f;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag(Tags.VehicleBody))
            {
                if(gameCamera.IsZoomingIn == false)
                    gameCamera.ZoomIn(gameObject.transform.position, zoom, zoomSpeed);   
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag(Tags.VehicleBody))
            {
                if(gameCamera.IsZoomingOut == false)
                    gameCamera.ZoomOut(zoomSpeed);   
            }
        }
    }
}