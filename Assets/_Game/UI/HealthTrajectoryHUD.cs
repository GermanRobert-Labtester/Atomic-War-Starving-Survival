using UnityEngine;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// Subtle "health trajectory" indicator, distinct from the dosimeter's live exposure
    /// rate: it shows nothing until a medical exam is performed, and does not refresh on
    /// its own afterwards. Deliberately not wired into the per-frame HUD tick -- the
    /// player should have to go looking (and feel the silence in between). Thin MonoBehaviour.
    /// </summary>
    public class HealthTrajectoryHUD : MonoBehaviour
    {
        [SerializeField] private bool _showRawValues = false;

        public bool HasBeenExamined { get; private set; }
        public PrognosisStage Stage { get; private set; }
        public float EstimatedDaysToNextStage { get; private set; }
        public bool ShowRawValues => _showRawValues;

        /// <summary>Push a reading from a medical exam. Only caller of this method is a doctor/exam action.</summary>
        public void SetReading(PrognosisEstimate estimate)
        {
            HasBeenExamined = true;
            Stage = estimate.Stage;
            EstimatedDaysToNextStage = Mathf.Max(0f, estimate.EstimatedDaysToNextStage);
        }

        public void SetShowRawValues(bool show)
        {
            _showRawValues = show;
        }
    }
}
