using UnityEngine;
using UnityEngine.UI;
using System;

namespace Game
{
    // Author: David Pagotto
    public class Gauge : MonoBehaviour
    {
        private const float OffsetToNeedleBasePosition = -90f;

        [SerializeField] private bool isCounterClockwise = false;
        [SerializeField] [Range(0, 100)] private float needleRotationPercent = 0f;
        [SerializeField] [Range(0, 100)] private float smoothness = 10f;

        [SerializeField] public Image needleImage;
        [SerializeField] [HideInInspector] private Vector2 needleLeftAnglePointOffset;
        [SerializeField] [HideInInspector] private Vector2 needleRightAnglePointOffset;
        [SerializeField] [HideInInspector] private bool isInitializedByEditor = false;

        public bool IsInitializedByEditor { get => isInitializedByEditor; set => isInitializedByEditor = value; }
        public Vector2 NeedleLeftAnglePointOffset { get => needleLeftAnglePointOffset; set => needleLeftAnglePointOffset = value; }
        public Vector2 NeedleRightAnglePointOffset { get => needleRightAnglePointOffset; set => needleRightAnglePointOffset = value; }
        public Vector2 NeedleLeftAnglePoint => (Vector2)needleImage.transform.position + NeedleLeftAnglePointOffset;
        public Vector2 NeedleRightAnglePoint => (Vector2)needleImage.transform.position + NeedleRightAnglePointOffset;

        private Vector2 NeedlePivotOffset
        {
            get
            {
                var rect = needleImage.rectTransform.rect;
                float x = rect.x + rect.width * needleImage.rectTransform.pivot.x;
                float y = rect.y + rect.height * needleImage.rectTransform.pivot.y;
                return new Vector2(x, y);
            }
        }

        public Vector2 NeedlePivotPosition => NeedlePivotOffset + (Vector2)needleImage.transform.position;

        public float NeedleRotationPercent
        {
            get => needleRotationPercent;
            set
            {
                if (float.IsNaN(value))
                    throw new Exception("Needle rotation percent can't be NaN.");
                needleRotationPercent = value;
            }
        }
        private float NeedleAngle
        {
            get
            {
                if (isCounterClockwise)
                    return beginAngle - (1 - NeedleRotationPercent / 100f) * needleRange;
                else
                    return beginAngle - (NeedleRotationPercent / 100f) * needleRange;
            }
        }

        private float beginAngle = 0f;
        private float endAngle = 0f;
        private float needleRange = 0f;

        public void UpdateNeedleAvailableRange()
        {
            if (!IsInitializedByEditor)
                return;
            beginAngle = NeedleLeftAnglePoint.AngleTo(NeedlePivotPosition) * Mathf.Rad2Deg + OffsetToNeedleBasePosition;
            endAngle = NeedleRightAnglePoint.AngleTo(NeedlePivotPosition) * Mathf.Rad2Deg + OffsetToNeedleBasePosition;
            needleRange = beginAngle - endAngle;
        }

        private void Awake()
        {
            UpdateNeedleAvailableRange();
            needleImage.transform.eulerAngles = Vector3.forward * beginAngle;
        }

        private void Update()
        {
            float previousAngle = needleImage.transform.eulerAngles.z;
            needleImage.transform.eulerAngles = Vector3.forward * Mathf.LerpAngle(previousAngle, NeedleAngle, smoothness * Time.deltaTime);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(NeedlePivotPosition, 1f);
        }
#endif
    }

}