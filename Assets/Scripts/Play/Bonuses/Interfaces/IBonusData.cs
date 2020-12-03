using System.Runtime.Serialization;
namespace Game
{
    // Author: David Pagotto
    public interface IBonusData : ISerializable
    {
        BonusType BonusType { get; }
        IBonusData Clone();
    }
}
