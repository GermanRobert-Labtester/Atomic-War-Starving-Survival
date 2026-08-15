using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// UI Element #02 — Geiger Sweep Gauge.
    /// Inline widget: CPM count, 5-segment signal bars, status labels.
    /// Raises OnGeigerUpdated on state change.
    /// </summary>
    public class GeigerSweepGauge : MonoBehaviour
    {
        public enum GeigerStatus { Clear, Elevated, Alert }

        public event Action<float, GeigerStatus> OnGeigerUpdated;

        [SerializeField] private UIDocument _document;

        private VisualElement _root;
        private Label _cpmLabel;
        private Label _statusLabel;
        private VisualElement[] _bars = new VisualElement[5];

        private float _cpm;
        private GeigerStatus _status;

        [Serializable]
        public struct SaveState { public float cpm; public GeigerStatus status; }

        public SaveState CaptureState() => new SaveState { cpm = _cpm, status = _status };
        public void RestoreState(SaveState s) { _cpm = s.cpm; _status = s.status; Refresh(); }

        private void OnEnable()
        {
            if (_document == null) _document = GetComponent<UIDocument>();
            if (_document == null) return;
            _root = _document.rootVisualElement?.Q("geiger-sweep-root");
            if (_root == null) return;
            _cpmLabel    = _root.Q<Label>("geiger-cpm-label");
            _statusLabel = _root.Q<Label>("geiger-status-label");
            for (int i = 0; i < 5; i++)
                _bars[i] = _root.Q($"geiger-bar-{i:D2}");
            Refresh();
        }

        public void SetCPM(float cpm)
        {
            _cpm = cpm;
            _status = cpm < 50f ? GeigerStatus.Clear : cpm < 200f ? GeigerStatus.Elevated : GeigerStatus.Alert;
            Refresh();
            OnGeigerUpdated?.Invoke(_cpm, _status);
        }

        public void Show() => _root?.RemoveFromClassList("hidden");
        public void Hide() => _root?.AddToClassList("hidden");

        private void Refresh()
        {
            if (_root == null) return;
            if (_cpmLabel    != null) _cpmLabel.text    = $"{_cpm:F0} CPM";
            if (_statusLabel != null)
            {
                _statusLabel.text = _status.ToString().ToUpper();
                _statusLabel.EnableInClassList("geiger-status--alert",    _status == GeigerStatus.Alert);
                _statusLabel.EnableInClassList("geiger-status--elevated", _status == GeigerStatus.Elevated);
            }
            int filledBars = _status == GeigerStatus.Clear ? 1 :
                             _status == GeigerStatus.Elevated ? 3 : 5;
            for (int i = 0; i < 5; i++)
            {
                if (_bars[i] == null) continue;
                _bars[i].EnableInClassList("geiger-bar--filled",    i < filledBars);
                _bars[i].EnableInClassList("geiger-bar--critical",  i < filledBars && _status == GeigerStatus.Alert);
            }
            _root.EnableInClassList("diegetic-panel--critical", _status == GeigerStatus.Alert);
            _root.EnableInClassList("diegetic-panel--warning",  _status == GeigerStatus.Elevated);
        }
    }
}
