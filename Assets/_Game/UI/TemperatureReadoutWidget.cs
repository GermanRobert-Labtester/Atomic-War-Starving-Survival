using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// UI Element #09 — Temperature Readout Widget.
    /// Top-right: internal/external °C, heat status, fuel reserve hours, heating source.
    /// Raises OnTemperatureUpdated on state change.
    /// </summary>
    public class TemperatureReadoutWidget : MonoBehaviour
    {
        public enum HeatStatus { Optimal, Cold, Freezing, Critical }
        public event Action<float, HeatStatus> OnTemperatureUpdated;

        [SerializeField] private UIDocument _document;
        [SerializeField] private float _criticalTempC = -10f;
        [SerializeField] private float _freezingTempC = 0f;
        [SerializeField] private float _coldTempC     = 12f;

        private VisualElement _root;
        private Label _internalTempLabel;
        private Label _externalTempLabel;
        private Label _heatStatusLabel;
        private Label _fuelHoursLabel;
        private Label _heatSourceLabel;

        private float _internalTempC;
        private float _externalTempC;
        private float _fuelHoursRemaining;
        private string _heatSourceName;
        private HeatStatus _status;

        [Serializable]
        public struct SaveState
        {
            public float internalTempC;
            public float externalTempC;
            public float fuelHoursRemaining;
            public string heatSourceName;
        }
        public SaveState CaptureState() => new SaveState
        {
            internalTempC = _internalTempC, externalTempC = _externalTempC,
            fuelHoursRemaining = _fuelHoursRemaining, heatSourceName = _heatSourceName
        };
        public void RestoreState(SaveState s)
        {
            SetTemperatureData(s.internalTempC, s.externalTempC, s.fuelHoursRemaining, s.heatSourceName);
        }

        private void OnEnable()
        {
            if (_document == null) _document = GetComponent<UIDocument>();
            if (_document == null) return;
            _root = _document.rootVisualElement?.Q("temperature-readout-root");
            if (_root == null) return;
            _internalTempLabel = _root.Q<Label>("temp-internal-label");
            _externalTempLabel = _root.Q<Label>("temp-external-label");
            _heatStatusLabel   = _root.Q<Label>("temp-status-label");
            _fuelHoursLabel    = _root.Q<Label>("temp-fuel-label");
            _heatSourceLabel   = _root.Q<Label>("temp-source-label");
            Refresh();
        }

        public void SetTemperatureData(float internalC, float externalC,
                                       float fuelHours, string heatSource)
        {
            _internalTempC      = internalC;
            _externalTempC      = externalC;
            _fuelHoursRemaining = fuelHours;
            _heatSourceName     = heatSource;
            _status = internalC <= _criticalTempC ? HeatStatus.Critical :
                      internalC <= _freezingTempC ? HeatStatus.Freezing :
                      internalC <= _coldTempC     ? HeatStatus.Cold     : HeatStatus.Optimal;
            Refresh();
            OnTemperatureUpdated?.Invoke(_internalTempC, _status);
        }

        public void Show() => _root?.RemoveFromClassList("hidden");
        public void Hide() => _root?.AddToClassList("hidden");

        private void Refresh()
        {
            if (_root == null) return;
            if (_internalTempLabel != null) _internalTempLabel.text = $"INT: {_internalTempC:+0;-0;0}°C";
            if (_externalTempLabel != null) _externalTempLabel.text = $"EXT: {_externalTempC:+0;-0;0}°C";
            if (_fuelHoursLabel    != null) _fuelHoursLabel.text    = $"FUEL: {_fuelHoursRemaining:F0}h";
            if (_heatSourceLabel   != null) _heatSourceLabel.text   = _heatSourceName?.ToUpper() ?? "NO SOURCE";

            if (_heatStatusLabel != null)
            {
                _heatStatusLabel.text = _status.ToString().ToUpper();
                _heatStatusLabel.EnableInClassList("temp-status--critical",  _status == HeatStatus.Critical);
                _heatStatusLabel.EnableInClassList("temp-status--freezing",  _status == HeatStatus.Freezing);
                _heatStatusLabel.EnableInClassList("temp-status--cold",      _status == HeatStatus.Cold);
            }
            _root.EnableInClassList("diegetic-panel--critical", _status == HeatStatus.Critical);
            _root.EnableInClassList("diegetic-panel--warning",  _status == HeatStatus.Freezing);
        }
    }
}
