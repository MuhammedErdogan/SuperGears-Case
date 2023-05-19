
namespace Others
{
    public interface IAudioService
    {
        void PlaySound();
        void StopSound();
        void UpdatePitch(float rpm, float maxRpm);
    }
}