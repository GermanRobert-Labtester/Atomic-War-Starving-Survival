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

        /// <summary>Prompt #203 — revealed LatentDamage when Radiologist (or kit) examines.</summary>
        public float RevealedLatentDamage { get; private set; }
        /// <summary>Prompt #203 — revealed OnsetTimer days when Radiologist (or kit) examines.</summary>
        public float RevealedOnsetTimer { get; private set; }
        /// <summary>True when LatentDamage/OnsetTimer were pushed with the exam.</summary>
        public bool HasLatentReveal { get; private set; }

        /// <summary>Push a reading from a medical exam. Only caller of this method is a doctor/exam action.</summary>
        public void SetReading(PrognosisEstimate estimate)
        {
            HasBeenExamined = true;
            Stage = estimate.Stage;
            EstimatedDaysToNextStage = Mathf.Max(0f, estimate.EstimatedDaysToNextStage);
        }

        /// <summary>
        /// Prompt #203 — push stage estimate plus hidden LatentDamage / OnsetTimer
        /// (Radiologist free look, or MedicalKit exam).
        /// </summary>
        public void SetReading(
            PrognosisEstimate estimate,
            float latentDamage,
            float onsetTimer)
        {
            SetReading(estimate);
            RevealedLatentDamage = Mathf.Max(0f, latentDamage);
            RevealedOnsetTimer = Mathf.Max(0f, onsetTimer);
            HasLatentReveal = true;
            _showRawValues = true;
        }

        public void SetShowRawValues(bool show)
        {
            _showRawValues = show;
        }
    }
}
