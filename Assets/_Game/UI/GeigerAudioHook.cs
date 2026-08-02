using UnityEngine;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// Geiger counter audio controller. Click frequency scales with live exposure rate (rads/hr).
    /// Exposes a designer-tunable AnimationCurve so click cadence curve can be retuned without code.
    /// Event-driven update; no polling.
    /// </summary>
    public class GeigerAudioHook : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _clickClip;

        [Tooltip("Maps exposure rate (0..100 rads/hr) to click frequency (0..30 Hz).")]
        public AnimationCurve CadenceCurve = new AnimationCurve(
            new Keyframe(0f, 0f),
            new Keyframe(10f, 2f),
            new Keyframe(40f, 10f),
            new Keyframe(100f, 30f)
        );

        public float CurrentRate { get; private set; }
        public float CurrentClickFrequency { get; private set; }

        private void Awake()
        {
            if (_audioSource == null)
            {
                _audioSource = GetComponent<AudioSource>();
            }
        }

        /// <summary>Subscribes to live exposure rate changes from RadiationSystem or Dosimeter.</summary>
        public void UpdateExposureRate(float radsPerHour)
        {
            CurrentRate = Mathf.Max(0f, radsPerHour);
            CurrentClickFrequency = CadenceCurve != null ? CadenceCurve.Evaluate(CurrentRate) : CurrentRate * 0.3f;
            ApplyAudioCadence();
        }

        private void ApplyAudioCadence()
        {
            if (_audioSource == null || CurrentClickFrequency <= 0f) return;

            // Trigger a single click or schedule click interval
            if (_clickClip != null && _audioSource.enabled)
            {
                _audioSource.PlayOneShot(_clickClip, Mathf.Clamp01(CurrentRate / 50f + 0.1f));
            }
        }
    }
}
