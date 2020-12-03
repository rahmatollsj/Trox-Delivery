using Harmony;
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game
{
    // Author: David Pagotto
    public class VehicleLightingController : MonoBehaviour
    {
        [Tooltip("In milliseconds")]
        [SerializeField] private float blinkingDelay = 650f;

        [SerializeField] private bool enableBlinkingWarningLights;
        [SerializeField] private bool enableBlinkingBrakeLight;
        [SerializeField] private bool enableBrakeLight;
        [SerializeField] private bool enableWarningLights;
        [SerializeField] private bool enableHeadLights;
        [SerializeField] private bool enableReverseLight;

        private Coroutine blinkingWarningLightsRoutine;
        private Coroutine blinkingBrakeLightsRoutine;

        private Light2D frontWarningLight;
        private Light2D rearWarningLight;
        private Light2D rearBrakeLight;
        private Light2D headlightLight;
        private Light2D reverseLight;

        private bool initialized = false;

        public bool EnableBlinkingWarningLights
        {
            get => blinkingWarningLightsRoutine != null;
            set
            {
                if (blinkingWarningLightsRoutine == null)
                {
                    if (value)
                        blinkingWarningLightsRoutine = StartCoroutine(BlinkingWarningLightsRoutine());
                }
                else
                {
                    if (!value && blinkingWarningLightsRoutine != null)
                    {
                        StopCoroutine(blinkingWarningLightsRoutine);
                        blinkingWarningLightsRoutine = null;
                        EnableWarningLights = false;
                    }
                }
                enableBlinkingWarningLights = value;
            }
        }
        public bool EnableBlinkingBrakeLights
        {
            get => blinkingBrakeLightsRoutine != null;
            set
            {
                if (blinkingBrakeLightsRoutine == null)
                {
                    if (value)
                        blinkingBrakeLightsRoutine = StartCoroutine(BlinkingBrakeLightsRoutine());
                }
                else
                {
                    if (!value && blinkingBrakeLightsRoutine != null)
                    {
                        StopCoroutine(blinkingBrakeLightsRoutine);
                        blinkingBrakeLightsRoutine = null;
                        EnableBrakeLight = false;
                    }
                }
                enableBlinkingBrakeLight = value;
            }
        }
        public bool EnableWarningLights
        {
            get => enableWarningLights;
            set
            {
                frontWarningLight.enabled = value;
                rearWarningLight.enabled = value;
                enableWarningLights = value;
            }
        }
        public bool EnableBrakeLight { get => rearBrakeLight.enabled; set => rearBrakeLight.enabled = value; }
        public bool EnableHeadLights { get => headlightLight.enabled; set => headlightLight.enabled = value; }
        public bool EnableReverseLight { get => reverseLight.enabled; set => reverseLight.enabled = value; }

        private IEnumerator BlinkingWarningLightsRoutine()
        {
            while (isActiveAndEnabled)
            {
                yield return new WaitForSeconds(blinkingDelay / 1000f);
                EnableWarningLights = !EnableWarningLights;
            }
        }

        private IEnumerator BlinkingBrakeLightsRoutine()
        {
            while (isActiveAndEnabled)
            {
                yield return new WaitForSeconds(blinkingDelay / 1000f);
                EnableBrakeLight = !EnableBrakeLight;
            }
        }

        void UpdateTruckLightsStates()
        {
            EnableBlinkingWarningLights = enableBlinkingWarningLights;
            EnableBlinkingBrakeLights = enableBlinkingBrakeLight;
            EnableBrakeLight = enableBrakeLight;
            EnableWarningLights = enableWarningLights;
            EnableHeadLights = enableHeadLights;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (initialized && EditorApplication.isPlaying)
                UpdateTruckLightsStates();
        }
#endif
        private void Awake()
        {
            var warningLights = transform.Find(GameObjects.WarningLights);
            frontWarningLight = warningLights.Find(GameObjects.EngineFlasher).GetComponent<Light2D>();
            rearWarningLight = warningLights.Find(GameObjects.RearFlasher).GetComponent<Light2D>();
            rearBrakeLight = transform.Find(GameObjects.RearBrakes).GetComponent<Light2D>();
            headlightLight = transform.Find(GameObjects.Headlights).GetComponent<Light2D>();
            reverseLight = transform.Find(GameObjects.Reverse).GetComponent<Light2D>();
            initialized = true;
            UpdateTruckLightsStates();
        }
    }
}