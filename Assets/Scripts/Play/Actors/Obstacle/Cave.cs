using Harmony;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Game
{
    public class Cave : MonoBehaviour
    {
        [SerializeField] private Light2D light2D;
        [SerializeField] private Color defaultColor = Color.white;
        [SerializeField] private float defaultIntensity = 1;
        [SerializeField] private Color darkColor = Color.blue;
        [SerializeField] private float darkIntensity = 0.5f;
        [SerializeField] private VehicleLightingController vehicleLightingController;

        private VehicleEnterDangerousZoneEventChannel vehicleEnterDangerousZoneEventChannel;
        private VehicleOutOfDangerousZoneEventChannel vehicleOutOfDangerousZoneEventChannel;
        
        private void Awake()
        {
            light2D.lightType = Light2D.LightType.Global;
            light2D.color = defaultColor;
            light2D.intensity = defaultIntensity;

            vehicleEnterDangerousZoneEventChannel = Finder.VehicleEnterDangerousZoneEventChannel;
            vehicleEnterDangerousZoneEventChannel.OnVehicleEnterDangerousZoneEvent += OnVehicleEnterDangerousZone;
            vehicleOutOfDangerousZoneEventChannel = Finder.VehicleOutOfDangerousZoneEventChannel;
            vehicleOutOfDangerousZoneEventChannel.OnVehicleOutOfDangerousZoneEvent += OnVehicleOutOfDangerousZone;
        }

        private void OnDestroy()
        {
            vehicleEnterDangerousZoneEventChannel.OnVehicleEnterDangerousZoneEvent -= OnVehicleEnterDangerousZone;
            vehicleOutOfDangerousZoneEventChannel.OnVehicleOutOfDangerousZoneEvent -= OnVehicleOutOfDangerousZone;
        }

        private void ChangeColor(Color color)
        {
            light2D.color = color;
        }

        private void ChangeIntensity(float intensity)
        {
            light2D.intensity = intensity;
        }

        private void OnVehicleEnterDangerousZone()
        {
            ChangeColor(darkColor);
            ChangeIntensity(darkIntensity);
            vehicleLightingController.EnableHeadLights = true;
        }

        private void OnVehicleOutOfDangerousZone()
        {
            ChangeColor(defaultColor);
            ChangeIntensity(defaultIntensity);
            vehicleLightingController.EnableHeadLights = false;
        }
    }
}