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
#pragma warning disable 0414 // only read inside #if DEVELOPMENT_BUILD || UNITY_EDITOR below
        [SerializeField] private bool _drawDebugGui = true;
#pragma warning restore 0414

        public float CurrentRate { get; private set; }
        public float CumulativeDose { get; private set; }
        public float NeedleAngleNormalized { get; private set; }
        public bool ShowRawValues => _showRawValues;

#if DEVELOPMENT_BUILD || UNITY_EDITOR
        private void OnGUI()
        {
            if (!_drawDebugGui || !Application.isPlaying) return;
            DrawDebugOverlay();
        }
#endif

        private void DrawDebugOverlay()
        {
            const float width = 200f;
            const float height = 70f;
            var rect = new Rect(Screen.width - width - 10f, 10f, width, height);
            GUI.Box(rect, GUIContent.none);
            GUILayout.BeginArea(rect);
            GUILayout.Label("DOSIMETER", GUILayout.Height(20f));
            GUILayout.Label($"Dose: {CumulativeDose:0.00} Sv", GUILayout.Height(18f));
            GUILayout.Label($"Rate: {CurrentRate:0.0} rads/hr", GUILayout.Height(18f));
            GUILayout.EndArea();
        }

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
