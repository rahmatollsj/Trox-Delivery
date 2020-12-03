using System.Collections;
using Harmony;
using UnityEngine;

namespace Game
{
    //Author François-Xavier Bernier
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField][Tooltip("Distance from the vehicle(X'Y'Z)")] 
        private Vector3 offset = new Vector3(3, 0, -10);

        private Transform vehicle;
        //Author: Seyed-Rahmatoll Javadi
        private new Camera camera;
        private float defaultOrthographicSize;
        
        private bool isAllowedToFollow;

        public bool IsZoomingIn { get; private set; }

        public bool IsZoomingOut { get; private set; }

        private void Awake()
        {
            vehicle = Finder.Vehicle.transform.Find(GameObjects.Body);
            //Author: Seyed-Rahmatoll Javadi
            camera = GetComponent<Camera>();
            defaultOrthographicSize = camera.orthographicSize;
            
            isAllowedToFollow = true;
            IsZoomingIn = false;
            IsZoomingOut = false;
        }

        private void LateUpdate()
        {
            if(isAllowedToFollow)
                transform.position = vehicle.transform.position + offset;
        }
        
        //Author: Seyed-Rahmatoll Javadi
        public void ZoomIn(Vector3 targetPosition, float zoom, float speed)
        {
            isAllowedToFollow = false;
            IsZoomingIn = true;
            StartCoroutine(ZoomInCoroutine(transform.position, targetPosition, zoom, speed));
        }

        //Author: Seyed-Rahmatoll Javadi
        private IEnumerator ZoomInCoroutine(Vector3 cameraCurrentPosition, Vector3 targetPosition, float zoom, float speed)
        {
            for (float i = 0; i < 1; i += speed * Time.deltaTime)
            {
                Vector2 lerp = Vector2.Lerp(cameraCurrentPosition, targetPosition, i);
                transform.position = new Vector3(lerp.x, lerp.y, cameraCurrentPosition.z);
                
                camera.orthographicSize = Mathf.Lerp(defaultOrthographicSize, zoom, i);
                yield return null;
            }
            
            yield return new WaitForSeconds(1);
            IsZoomingIn = false;
        }

        //Author: Seyed-Rahmatoll Javadi
        public void ZoomOut(float speed)
        {
            IsZoomingOut = true;
            StartCoroutine(ZoomOutCoroutine(camera.orthographicSize, speed));
        }
        
        //Author: Seyed-Rahmatoll Javadi
        private IEnumerator ZoomOutCoroutine(float zoom, float speed)
        {
            isAllowedToFollow = true;
            
            for (float i = 0; i < 1; i += speed * Time.deltaTime)
            {
                camera.orthographicSize = Mathf.Lerp(zoom, defaultOrthographicSize, i);
                yield return null;
            }

            yield return new WaitForSeconds(1);
            IsZoomingOut = false;
        }
    }
}