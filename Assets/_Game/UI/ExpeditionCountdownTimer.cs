using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace AtomicWar._Game.UI
{
    [Serializable]
    public class ExpeditionTimerData
    {
        public string SurvivorName = "SGT. VASQUEZ";
        public string DestinationZone = "SECTOR 7-G";
        public float ElapsedSeconds = 105f;
        public float EstimatedSeconds = 120f;
        public float DangerLevel01 = 0.4f;
    }

    public class ExpeditionCountdownTimer : MonoBehaviour
    {
        [SerializeField] private UIDocument _document;
        [SerializeField] private ExpeditionTimerData _data = new ExpeditionTimerData();

        private VisualElement _root;
        private Label _survivorNameLabel;
        private Label _zoneLabel;
        private Label _elapsedLabel;
        private Label _etaLabel;
        private VisualElement _dangerFill;
        private VisualElement _overdueBanner;

        public event Action<ExpeditionTimerData> OnStateChanged;

        public ExpeditionTimerData CurrentData => _data;
        public bool IsOverdue => _data.ElapsedSeconds > _data.EstimatedSeconds;

        private void OnEnable()
        {
            if (_document == null)
                _document = GetComponent<UIDocument>();

            if (_document != null && _document.rootVisualElement != null)
            {
                _root = _document.rootVisualElement.Q("expedition-countdown-root") 
                      ?? _document.rootVisualElement.Q("expedition_countdown_root");
                Bind();
                RefreshUI();
            }
        }

        private void Bind()
        {
            if (_root == null) return;
            _survivorNameLabel = _root.Q<Label>("expedition_survivor_name");
            _zoneLabel = _root.Q<Label>("expedition_zone_label");
            _elapsedLabel = _root.Q<Label>("expedition_elapsed_time");
            _etaLabel = _root.Q<Label>("expedition_eta_label");
            _dangerFill = _root.Q<VisualElement>("expedition_danger_fill");
            _overdueBanner = _root.Q<VisualElement>("expedition_overdue_banner");
        }

        private void Update()
        {
            if (_root != null && !_root.ClassListContains("hidden") && _data != null)
            {
                _data.ElapsedSeconds += Time.deltaTime;
                RefreshTimes();
            }
        }

        public void SetData(ExpeditionTimerData data)
        {
            if (data == null) return;
            _data = data;
            RefreshUI();
            OnStateChanged?.Invoke(_data);
        }

        public void UpdateProgress(float elapsed, float estimated, float danger01)
        {
            _data.ElapsedSeconds = elapsed;
            _data.EstimatedSeconds = estimated;
            _data.DangerLevel01 = Mathf.Clamp01(danger01);
            RefreshUI();
            OnStateChanged?.Invoke(_data);
        }

        private void RefreshUI()
        {
            if (_root == null) return;

            if (_survivorNameLabel != null) _survivorNameLabel.text = _data.SurvivorName;
            if (_zoneLabel != null) _zoneLabel.text = _data.DestinationZone;
            if (_dangerFill != null) _dangerFill.style.width = Length.Percent(_data.DangerLevel01 * 100f);

            RefreshTimes();
        }

        private void RefreshTimes()
        {
            if (_elapsedLabel != null) _elapsedLabel.text = FormatTime(_data.ElapsedSeconds);
            if (_etaLabel != null) _etaLabel.text = FormatTime(_data.EstimatedSeconds);

            bool overdue = IsOverdue;
            if (_overdueBanner != null)
            {
                if (overdue)
                    _overdueBanner.RemoveFromClassList("hidden");
                else
                    _overdueBanner.AddToClassList("hidden");
            }
        }

        private string FormatTime(float seconds)
        {
            TimeSpan ts = TimeSpan.FromSeconds(Mathf.Max(0, seconds));
            return $"{ts.Hours:D2}:{ts.Minutes:D2}:{ts.Seconds:D2}";
        }

        public void Show() => _root?.RemoveFromClassList("hidden");
        public void Hide() => _root?.AddToClassList("hidden");
    }
}
