using UnityEngine;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// On-screen dosimeter readout: displays live exposure rate (rads/hr) and lifetime
    /// cumulative dose. Needle angle (or bar fill) updates strictly when system events fire.
    /// Supports F2 raw number debug mode. Thin MonoBehaviour.
    /// </summary>
    public class DosimeterHUD : MonoBehaviour
    {
        [SerializeField] private bool _showRawValues = false;

        public float CurrentRate { get; private set; }
        public float CumulativeDose { get; private set; }
        public float NeedleAngleNormalized { get; private set; }
        public bool ShowRawValues => _showRawValues;

        public void SetReading(float cumulativeDose, float currentRate)
        {
            CumulativeDose = Mathf.Max(0f, cumulativeDose);
            CurrentRate = Mathf.Max(0f, currentRate);

            // Normalize needle reading (0..100 rads/hr mapped to 0..1)
            NeedleAngleNormalized = Mathf.Clamp01(CurrentRate / 100f);
        }

        public void SetShowRawValues(bool show)
        {
            _showRawValues = show;
        }

        /// <summary>Calculates needle rotation angle in degrees (-45deg to +45deg dial sweep).</summary>
        public float GetNeedleRotationDegrees()
        {
            return Mathf.Lerp(-45f, 45f, NeedleAngleNormalized);
        }
    }
}
