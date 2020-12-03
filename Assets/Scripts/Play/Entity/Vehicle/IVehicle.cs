namespace Game
{
    //Author: Seyed-Rahmatoll Javadi
    public interface IVehicle
    {
        void ChangeTemperature(TemperatureStimulusTypes type);
        
        void EnterDangerousZone();

        void OutOfDangerousZone(CurrentZoneState currentZoneState);

        void ImpulseUp(float force);
    }
}