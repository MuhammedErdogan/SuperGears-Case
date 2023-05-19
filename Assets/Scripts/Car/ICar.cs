
namespace Car
{
    public interface ICar
    {
        float CurrentSpeedMs { get; }
        float MaxSpeedKmh { get; }
        int CurrentRpm { get; }
        int NumberOfGears { get; }

        void Drive(DriveState state);
    }
}
