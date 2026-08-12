using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// UI Element #08 — Water Purity Gauge.
    /// Right sidebar: water reserve (L), purification status, output rate, contamination %.
    /// Lethe-cyan for clean, critical red for contaminated.
    /// Raises OnPurityChanged on state change.
    /// </summary>
    public class WaterPurityGauge : MonoBehaviour
    {
        public enum PurificationStatus { Filtering, Idle, Broken }
        public event Action<float, PurificationStatus> OnPurityChanged;

        [SerializeField] private UIDocument _document;

        private VisualElement _root;
        private Label _reserveLabel;
        private Label _statusLabel;
        private Label _outputRateLabel;
        private Label _contaminationLabel;
        private VisualElement _purityFill;

        private float _reserveLitres;
        private float _outputRatePerDay;
        private float _contaminationPct;
        private PurificationStatus _status;

        [Serializable]
        public struct SaveState
        {
            public float reserveLitres;
            public float outputRatePerDay;
            public float contaminationPct;
            public PurificationStatus status;
        }
        public SaveState CaptureState() => new SaveState
        {
            reserveLitres = _reserveLitres, outputRatePerDay = _outputRatePerDay,
            contaminationPct = _contaminationPct, status = _status
        };
        public void RestoreState(SaveState s)
        {
            SetWaterData(s.reserveLitres, s.outputRatePerDay, s.contaminationPct, s.status);
        }

        private void OnEnable()
        {
            if (_document == null) _document = GetComponent<UIDocument>();
            if (_document == null) return;
            _root = _document.rootVisualElement.Q("water-purity-root");
            if (_root == null) return;
            _reserveLabel       = _root.Q<Label>("water-reserve-label");
            _statusLabel        = _root.Q<Label>("water-status-label");
            _outputRateLabel    = _root.Q<Label>("water-output-label");
            _contaminationLabel = _root.Q<Label>("water-contamination-label");
            _purityFill         = _root.Q("water-purity-fill");
            Refresh();
        }

        public void SetWaterData(float reserveLitres, float outputRatePerDay,
                                 float contaminationPct, PurificationStatus status)
        {
            _reserveLitres   = reserveLitres;
            _outputRatePerDay= outputRatePerDay;
            _contaminationPct= Mathf.Clamp01(contaminationPct);
            _status          = status;
            Refresh();
            OnPurityChanged?.Invoke(_contaminationPct, _status);
        }

        public void Show() => _root?.RemoveFromClassList("hidden");
        public void Hide() => _root?.AddToClassList("hidden");

        private void Refresh()
        {
            if (_root == null) return;
            bool contaminated = _contaminationPct > 0.3f || _status == PurificationStatus.Broken;

            if (_reserveLabel       != null) _reserveLabel.text       = $"RESERVE: {_reserveLitres:F1} L";
            if (_outputRateLabel    != null) _outputRateLabel.text    = $"OUTPUT: {_outputRatePerDay:F1} L/day";
            if (_contaminationLabel != null) _contaminationLabel.text = $"CONTAMINATION: {_contaminationPct * 100f:F0}%";
            if (_statusLabel        != null)
            {
                _statusLabel.text = _status switch
                {
                    PurificationStatus.Filtering => "FILTERING",
                    PurificationStatus.Idle      => "IDLE — NO POWER",
                    PurificationStatus.Broken    => "BROKEN — UNSAFE WATER",
                    _                            => "UNKNOWN"
                };
                _statusLabel.EnableInClassList("water-status--critical",  contaminated);
                _statusLabel.EnableInClassList("water-status--filtering", _status == PurificationStatus.Filtering);
            }

            float purityPct = 1f - _contaminationPct;
            if (_purityFill != null)
            {
                _purityFill.style.width = Length.Percent(purityPct * 100f);
                _purityFill.EnableInClassList("water-purity-fill--critical", contaminated);
                _purityFill.EnableInClassList("water-purity-fill--clean",    !contaminated);
            }
            _root.EnableInClassList("diegetic-panel--critical", contaminated);
        }
    }
}
