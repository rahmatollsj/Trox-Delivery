using UnityEngine;

namespace Game
{
    //Author: Seyed-Rahmatoll Javadi
    public class TemperatureStimulus : MonoBehaviour
    {
        [Tooltip("Indicates what is the temperature in this zone")] 
        [SerializeField] private TemperatureStimulusTypes type;

        // Author: Benoit Simon-Turgeon
        private bool isOut = false;

        //Author: Seyed-Rahmatoll Javadi
        private void OnTriggerStay2D(Collider2D other)
        {
            // Author: Benoit Simon-Turgeon
            if (isOut)
                return;

            GameObject otherGameObject = other.gameObject;
            IVehicle entity = otherGameObject.gameObject.GetComponentInParent<IVehicle>();

            if (entity != null)
                entity.ChangeTemperature(type);
        }

        //Author: François-Xavier Bernier
        private void OnTriggerExit2D(Collider2D other)
        {
            // Author: Benoit Simon-Turgeon
            isOut = true;

            GameObject otherGameObject = other.gameObject;
            IVehicle entity = otherGameObject.gameObject.GetComponentInParent<IVehicle>();

            if(entity != null) 
                entity.OutOfDangerousZone(CurrentZoneState.Neutral);
        }

        // Author: Benoit Simon-Turgeon
        private void OnTriggerEnter2D(Collider2D other)
        {
            isOut = false;
            //Author: Seyed-Rahmatoll Javadi
            GameObject otherGameObject = other.gameObject;
            IVehicle entity = otherGameObject.gameObject.GetComponentInParent<IVehicle>();

            if(entity != null) 
                entity.EnterDangerousZone();
        }
    }
}