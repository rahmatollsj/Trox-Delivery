using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Indicator : MonoBehaviour
    {
        private Image pointImage;
        private float pointMinX;
        private float pointMaxX;

        private void Awake()
        {
            Image lineImage = transform.Find(GameObjects.Line).GetComponent<Image>();
            pointImage = transform.Find(GameObjects.Point).GetComponent<Image>();

            pointMinX = lineImage.transform.position.x;
            pointMaxX = pointMinX + lineImage.rectTransform.rect.width;
        }
        
        public void ChangePointPosition(float percentage)
        {
            float arrowCurrentX = Mathf.Lerp(pointMinX, pointMaxX, percentage);
            pointImage.transform.position = new Vector3(arrowCurrentX, pointImage.transform.position.y, pointImage.transform.position.z);
        }
    }
}