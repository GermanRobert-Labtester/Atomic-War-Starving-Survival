using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace AtomicWar._Game.UI
{
    [Serializable]
    public class DayNightClockData
    {
        public int DayNumber = 42;
        public float NormalizedTime01 = 0.6f; // 0..1 (0.25 = 06:00, 0.5 = 12:00, etc.)
        public string SeasonName = "FALLOUT WINTER";
    }

    public class DayNightArcClock : MonoBehaviour
    {
        [SerializeField] private UIDocument _document;
        [SerializeField] private DayNightClockData _data = new DayNightClockData();

        private VisualElement _root;
        private Label _timeText;
        private Label _dayText;
        private Label _seasonText;
        private VisualElement _arcDay;
        private VisualElement _arcNight;

        public event Action<DayNightClockData> OnStateChanged;

        public DayNightClockData CurrentData => _data;

        private void OnEnable()
        {
            if (_document == null)
                _document = GetComponent<UIDocument>();

            if (_document != null && _document.rootVisualElement != null)
            {
                _root = _document.rootVisualElement.Q("day-night-clock-root") 
                      ?? _document.rootVisualElement.Q("day_night_clock_root");
                Bind();
                RefreshUI();
            }
        }

        private void Bind()
        {
            if (_root == null) return;
            _timeText = _root.Q<Label>("clock_time_text");
            _dayText = _root.Q<Label>("clock_day_text");
            _seasonText = _root.Q<Label>("clock_season_text");
            _arcDay = _root.Q<VisualElement>("clock_arc_day");
            _arcNight = _root.Q<VisualElement>("clock_arc_night");
        }

        public void SetData(DayNightClockData data)
        {
            if (data == null) return;
            _data = data;
            RefreshUI();
            OnStateChanged?.Invoke(_data);
        }

        public void SetTime(int dayNumber, float time01, string season)
        {
            _data.DayNumber = dayNumber;
            _data.NormalizedTime01 = Mathf.Clamp01(time01);
            _data.SeasonName = season;

            RefreshUI();
            OnStateChanged?.Invoke(_data);
        }

        private void RefreshUI()
        {
            if (_root == null) return;

            if (_dayText != null)
                _dayText.text = $"DAY { _data.DayNumber:D3}";

            if (_seasonText != null)
                _seasonText.text = _data.SeasonName;

            float totalHours = _data.NormalizedTime01 * 24f;
            int hours = Mathf.FloorToInt(totalHours);
            int minutes = Mathf.FloorToInt((totalHours - hours) * 60f);

            if (_timeText != null)
                _timeText.text = $"{hours:D2}:{minutes:D2}";

            // Day sector (6:00 - 18:00 -> 0.25 to 0.75) vs Night sector
            if (_arcDay != null && _arcNight != null)
            {
                bool isDaytime = _data.NormalizedTime01 >= 0.25f && _data.NormalizedTime01 <= 0.75f;
                _arcDay.style.opacity = isDaytime ? 0.9f : 0.2f;
                _arcNight.style.opacity = !isDaytime ? 0.9f : 0.2f;
            }
        }

        public void Show() => _root?.RemoveFromClassList("hidden");
        public void Hide() => _root?.AddToClassList("hidden");
    }
}
