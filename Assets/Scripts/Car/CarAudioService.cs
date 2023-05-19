using UnityEngine;

public class CarAudioService : IAudioService
{
    [Inject] private AudioSource _engineAudioSource;
    [Inject] private AudioClip _engineAudioClip;

    private bool _isEngineSoundPlaying;

    public void PlaySound()
    {
        if (!_isEngineSoundPlaying && _engineAudioSource != null)
        {
            _engineAudioSource.Play();
            _isEngineSoundPlaying = true;
        }
    }

    public void StopSound()
    {
        if (_isEngineSoundPlaying && _engineAudioSource != null)
        {
            _engineAudioSource.Stop();
            _isEngineSoundPlaying = false;
        }
    }

    public void UpdatePitch(float rpm, float maxRpm)
    {
        if (_engineAudioSource != null)
        {
            _engineAudioSource.pitch = Mathf.Lerp(0.8f, 1.8f, rpm / maxRpm);
        }
    }
}
