using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// UI Element #01 — Radiation Dosimeter Widget.
    /// Persistent left-side panel: accumulated dose (Sv), dose rate (mSv/hr),
    /// 10-segment arc bar (0–100%), pulsing CRITICAL label above 400 mSv.
    /// Uses --lethe for safe range, --critical for danger.
    /// Raises OnStateChanged on every data update.
    /// Save/load-safe via SaveState struct.
    /// </summary>
    public class RadiationDosimeterWidget : MonoBehaviour
    {
        [SerializeField] private UIDocument _document;
        [SerializeField] private float _criticalThresholdMSv  = 400f;   // 0.4 Sv
        [SerializeField] private float _fullScaleSv           = 1f;     // 1 Sv = 100 %

        private VisualElement _root;
        private Label         _doseLabel;
        private Label         _rateLabel;
        private Label         _statusLabel;
        private Label         _iodineLabel;
        private VisualElement[] _segs = new VisualElement[10];

        // Backing state
        private float _accumulatedDoseSv;
        private float _doseRateMSvHr;
        private int   _iodineCount;
        private bool  _isCritical;

        // Public accessors
        public float AccumulatedDoseSv => _accumulatedDoseSv;
        public float DoseRateMSvHr    => _doseRateMSvHr;
        public bool  IsCritical       => _isCritical;

        public event Action<float, float, bool> OnStateChanged;

        // ── Save / Load ───────────────────────────────────────────────────────
        [Serializable]
        public struct SaveState
        {
            public float accumulatedDoseSv;
            public float doseRateMSvHr;
            public int   iodineCount;
        }

        public SaveState CaptureState() => new SaveState
        {
            accumulatedDoseSv = _accumulatedDoseSv,
            doseRateMSvHr     = _doseRateMSvHr,
            iodineCount       = _iodineCount
        };

        public void RestoreState(SaveState s)
        {
            SetDosimeterData(s.accumulatedDoseSv, s.doseRateMSvHr, s.iodineCount);
        }

        // ── Lifecycle ─────────────────────────────────────────────────────────
        private void OnEnable()
        {
            if (_document == null) _document = GetComponent<UIDocument>();
            if (_document == null) return;
            _root = _document.rootVisualElement.Q("radiation-dosimeter-root");
            if (_root == null) return;

            _doseLabel   = _root.Q<Label>("rad-dose-value");
            _rateLabel   = _root.Q<Label>("rad-rate-value");
            _statusLabel = _root.Q<Label>("rad-status-label");
            _iodineLabel = _root.Q<Label>("rad-iodine-label");

            for (int i = 0; i < 10; i++)
                _segs[i] = _root.Q($"rad-seg-{i:D2}");

            Refresh();
        }

        // ── Public API ────────────────────────────────────────────────────────
        /// <param name="accumulatedDoseSv">Total accumulated dose in Sieverts.</param>
        /// <param name="doseRateMSvHr">Current dose rate in mSv/hr.</param>
        /// <param name="iodineCount">Number of iodine pills in inventory.</param>
        public void SetDosimeterData(float accumulatedDoseSv, float doseRateMSvHr, int iodineCount = 0)
        {
            _accumulatedDoseSv = Mathf.Max(0f, accumulatedDoseSv);
            _doseRateMSvHr     = Mathf.Max(0f, doseRateMSvHr);
            _iodineCount       = iodineCount;
            _isCritical        = (_accumulatedDoseSv * 1000f) >= _criticalThresholdMSv
                                 || _doseRateMSvHr >= _criticalThresholdMSv;
            Refresh();
            OnStateChanged?.Invoke(_accumulatedDoseSv, _doseRateMSvHr, _isCritical);
        }

        public void Show() => _root?.RemoveFromClassList("hidden");
        public void Hide() => _root?.AddToClassList("hidden");

        // ── Private Refresh ───────────────────────────────────────────────────
        private void Refresh()
        {
            if (_root == null) return;

            if (_doseLabel   != null) _doseLabel.text   = $"{_accumulatedDoseSv * 1000f:F0} mSv";
            if (_rateLabel   != null) _rateLabel.text   = $"DOSE RATE: {_doseRateMSvHr:F1} mSv/hr";
            if (_iodineLabel != null) _iodineLabel.text = $"IODINE PILLS: {_iodineCount}";

            if (_statusLabel != null)
            {
                _statusLabel.text = _isCritical ? "CRITICAL — CHELATION NEEDED" :
                                    _doseRateMSvHr > 100f              ? "ELEVATED — SEEK SHELTER" : "SAFE";
                _statusLabel.EnableInClassList("rad-status--critical",  _isCritical);
                _statusLabel.EnableInClassList("rad-status--elevated",  !_isCritical && _doseRateMSvHr > 100f);
            }

            // Segment fill — 0–10 segments based on accumulated dose fraction
            float fraction    = Mathf.Clamp01(_accumulatedDoseSv / _fullScaleSv);
            int   filledSegs  = Mathf.RoundToInt(fraction * 10f);
            for (int i = 0; i < 10; i++)
            {
                if (_segs[i] == null) continue;
                bool filled = i < filledSegs;
                _segs[i].EnableInClassList("rad-seg--filled",          filled && !_isCritical);
                _segs[i].EnableInClassList("rad-seg--filled-critical", filled && _isCritical);
            }

            _root.EnableInClassList("diegetic-panel--critical", _isCritical);
        }
    }
}
