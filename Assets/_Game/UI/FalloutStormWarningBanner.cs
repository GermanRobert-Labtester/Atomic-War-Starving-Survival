using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// UI Element #04 — Fallout Storm Warning Banner.
    /// Full-width banner: storm name, wind direction, intensity, countdown.
    /// Slides down from top, auto-dismisses. Raises OnBannerDismissed.
    /// </summary>
    public class FalloutStormWarningBanner : MonoBehaviour
    {
        public enum StormIntensity { Light, Moderate, Heavy, BlackRain }
        public event Action OnBannerDismissed;

        [SerializeField] private UIDocument _document;
        [SerializeField] private float _autoDismissSeconds = 12f;

        private VisualElement _root;
        private Label _stormNameLabel;
        private Label _windLabel;
        private Label _intensityLabel;
        private VisualElement _countdownFill;
        private Label _countdownLabel;

        private Coroutine _dismissCoroutine;

        private bool _active;
        private string _stormName;
        private string _windDirection;
        private StormIntensity _intensity;
        private float _countdownSeconds;

        [Serializable]
        public struct SaveState
        {
            public bool active;
            public string stormName;
            public string windDirection;
            public StormIntensity intensity;
            public float countdownSeconds;
        }
        public SaveState CaptureState() => new SaveState
        {
            active = _active, stormName = _stormName, windDirection = _windDirection,
            intensity = _intensity, countdownSeconds = _countdownSeconds
        };
        public void RestoreState(SaveState s)
        {
            if (s.active) ShowStorm(s.stormName, s.windDirection, s.intensity, s.countdownSeconds);
            else Hide();
        }

        private void OnEnable()
        {
            if (_document == null) _document = GetComponent<UIDocument>();
            if (_document == null) return;
            _root = _document.rootVisualElement.Q("fallout-storm-root");
            if (_root == null) return;
            _stormNameLabel  = _root.Q<Label>("storm-name-label");
            _windLabel       = _root.Q<Label>("storm-wind-label");
            _intensityLabel  = _root.Q<Label>("storm-intensity-label");
            _countdownFill   = _root.Q("storm-countdown-fill");
            _countdownLabel  = _root.Q<Label>("storm-countdown-label");
            Hide();
        }

        public void ShowStorm(string name, string windDirection, StormIntensity intensity, float durationSeconds)
        {
            _stormName       = name;
            _windDirection   = windDirection;
            _intensity       = intensity;
            _countdownSeconds = durationSeconds;
            _active          = true;
            RefreshLabels();
            Show();
            if (_dismissCoroutine != null) StopCoroutine(_dismissCoroutine);
            _dismissCoroutine = StartCoroutine(AutoDismiss(durationSeconds));
        }

        public void Show() => _root?.RemoveFromClassList("hidden");
        public void Hide()
        {
            _active = false;
            _root?.AddToClassList("hidden");
        }

        private void RefreshLabels()
        {
            if (_root == null) return;
            if (_stormNameLabel != null) _stormNameLabel.text = _stormName?.ToUpper() ?? "";
            if (_windLabel      != null) _windLabel.text      = $"WIND: {_windDirection?.ToUpper()}";
            if (_intensityLabel != null)
            {
                string txt = _intensity switch
                {
                    StormIntensity.Light    => "INTENSITY: LIGHT FALLOUT",
                    StormIntensity.Moderate => "INTENSITY: MODERATE FALLOUT",
                    StormIntensity.Heavy    => "INTENSITY: HEAVY FALLOUT",
                    StormIntensity.BlackRain=> "INTENSITY: BLACK RAIN — SHELTER NOW",
                    _                      => "INTENSITY: UNKNOWN"
                };
                _intensityLabel.text = txt;
                _intensityLabel.EnableInClassList("storm-intensity--critical",
                    _intensity >= StormIntensity.Heavy);
            }
            _root.EnableInClassList("diegetic-panel--critical", _intensity >= StormIntensity.Heavy);
        }

        private IEnumerator AutoDismiss(float total)
        {
            float elapsed = 0f;
            while (elapsed < total)
            {
                elapsed += Time.deltaTime;
                float pct = 1f - Mathf.Clamp01(elapsed / total);
                if (_countdownFill != null) _countdownFill.style.width = Length.Percent(pct * 100f);
                float remaining = total - elapsed;
                if (_countdownLabel != null) _countdownLabel.text = $"{Mathf.CeilToInt(remaining):D3}s";
                yield return null;
            }
            Hide();
            OnBannerDismissed?.Invoke();
        }
    }
}
