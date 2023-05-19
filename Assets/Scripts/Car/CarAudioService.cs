using Others;
using UnityEngine;

namespace Car.Other
{
    public class CarAudioService : IAudioService
    {
        #region Injected Services
        [Inject] private AudioSource _engineAudioSource;
        [Inject] private AudioClip _engineAudioClip;
        #endregion

        #region Private Fields
        private bool _isEngineSoundPlaying;
        #endregion

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
}
