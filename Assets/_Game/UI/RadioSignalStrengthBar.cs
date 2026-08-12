using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace AtomicWar._Game.UI
{
    public enum RadioStationType
    {
        Emergency,
        Numbers,
        Broadcast,
        Unknown
    }

    [Serializable]
    public class RadioSignalData
    {
        public float FrequencyMhz = 104.5f;
        public int SignalStrengthBars = 3; // 0..5
        public RadioStationType StationType = RadioStationType.Emergency;
        public float NoiseLevelPercent = 18f;
    }

    public class RadioSignalStrengthBar : MonoBehaviour
    {
        [SerializeField] private UIDocument _document;
        [SerializeField] private RadioSignalData _data = new RadioSignalData();

        private VisualElement _root;
        private Label _freqLabel;
        private Label _stationTypeLabel;
        private Label _noiseLabel;
        private VisualElement[] _bars = new VisualElement[5];

        public event Action<RadioSignalData> OnStateChanged;

        public RadioSignalData CurrentData => _data;

        private void OnEnable()
        {
            if (_document == null)
                _document = GetComponent<UIDocument>();

            if (_document != null && _document.rootVisualElement != null)
            {
                _root = _document.rootVisualElement.Q("radio-signal-root") 
                      ?? _document.rootVisualElement.Q("radio_signal_root");
                Bind();
                RefreshUI();
            }
        }

        private void Bind()
        {
            if (_root == null) return;
            _freqLabel = _root.Q<Label>("radio_frequency_label");
            _stationTypeLabel = _root.Q<Label>("radio_station_type_label");
            _noiseLabel = _root.Q<Label>("radio_noise_label");

            for (int i = 0; i < 5; i++)
            {
                _bars[i] = _root.Q<VisualElement>($"signal_bar_{i + 1}");
            }
        }

        public void SetData(RadioSignalData data)
        {
            if (data == null) return;
            _data = data;
            RefreshUI();
            OnStateChanged?.Invoke(_data);
        }

        public void SetSignal(float freq, int bars, RadioStationType stationType, float noisePct)
        {
            _data.FrequencyMhz = freq;
            _data.SignalStrengthBars = Mathf.Clamp(bars, 0, 5);
            _data.StationType = stationType;
            _data.NoiseLevelPercent = Mathf.Clamp(noisePct, 0f, 100f);

            RefreshUI();
            OnStateChanged?.Invoke(_data);
        }

        private void RefreshUI()
        {
            if (_root == null) return;

            if (_freqLabel != null)
                _freqLabel.text = $"{_data.FrequencyMhz:F1} MHz";

            if (_stationTypeLabel != null)
                _stationTypeLabel.text = _data.StationType.ToString().ToUpper();

            if (_noiseLabel != null)
                _noiseLabel.text = $"{Mathf.RoundToInt(_data.NoiseLevelPercent)}%";

            for (int i = 0; i < 5; i++)
            {
                if (_bars[i] != null)
                {
                    if (i < _data.SignalStrengthBars)
                        _bars[i].AddToClassList("active");
                    else
                        _bars[i].RemoveFromClassList("active");
                }
            }
        }

        public void Show() => _root?.RemoveFromClassList("hidden");
        public void Hide() => _root?.AddToClassList("hidden");
    }
}
