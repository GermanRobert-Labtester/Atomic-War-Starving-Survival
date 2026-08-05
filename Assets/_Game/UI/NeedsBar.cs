using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.UI
{
    [System.Serializable]
    public class NeedBarData
    {
        public string NeedId;
        public string DisplayName;
        public float CurrentValue;
        public float MaxValue = 100f;
        public bool IsCritical;
        public Color NormalColor = new Color(0.2f, 0.8f, 0.3f);
        public Color CriticalColor = new Color(0.9f, 0.2f, 0.2f);
    }

    /// <summary>
    /// Renders a survivor's 7 need values (hunger, thirst, fatigue, warmth, morale,
    /// health, radiation) as progress bars. Supports critical color-shifts, pulse triggers,
    /// and raw number debug overlays. Event-driven only.
    /// </summary>
    public class NeedsBar : MonoBehaviour
    {
        [SerializeField] private bool _showRawValues = false;

        private readonly Dictionary<string, NeedBarData> _needBars = new Dictionary<string, NeedBarData>();
        private readonly Dictionary<string, float> _pulseTimers = new Dictionary<string, float>();

        public bool ShowRawValues => _showRawValues;
        public IReadOnlyDictionary<string, NeedBarData> NeedBars => _needBars;

        private void Awake()
        {
            EnsureInitialized();
        }

        public void EnsureInitialized()
        {
            if (_needBars.Count == 0)
            {
                InitializeDefaultBars();
            }
        }

        public void InitializeDefaultBars()
        {
            _needBars.Clear();
            AddBar("hunger", "Hunger", 0f, normal: new Color(0.8f, 0.6f, 0.2f));
            AddBar("thirst", "Thirst", 0f, normal: new Color(0.2f, 0.6f, 0.9f));
            AddBar("fatigue", "Fatigue", 0f, normal: new Color(0.6f, 0.4f, 0.8f));
            AddBar("warmth", "Warmth", 100f, normal: new Color(0.9f, 0.5f, 0.2f));
            AddBar("morale", "Morale", 100f, normal: new Color(0.2f, 0.8f, 0.4f));
            AddBar("health", "Health", 100f, normal: new Color(0.9f, 0.3f, 0.3f));
            AddBar("radiation", "Radiation", 0f, normal: new Color(0.3f, 0.9f, 0.3f));
        }

        private void AddBar(string id, string name, float initialVal, Color normal)
        {
            _needBars[id] = new NeedBarData
            {
                NeedId = id,
                DisplayName = name,
                CurrentValue = initialVal,
                NormalColor = normal,
                CriticalColor = new Color(0.95f, 0.15f, 0.15f)
            };
        }

        public void SetNeeds(Needs needs, float health = 100f, float radiation = 0f)
        {
            SetNeeds(needs, health, radiation, survivor: null, personalQuests: null, rng: null);
        }

        /// <summary>
        /// #255 Pathological Liar / Deceptive: when the survivor is distressed,
        /// UI may report Hunger/Thirst/Health as full so the player cannot trust the bars.
        /// Mask decision is rolled once per refresh for a consistent snapshot.
        /// </summary>
        /// <summary>
        /// #264 Denialist: last displayed RadiationAnxiety (0 while denial holds).
        /// Tests and HUD read this instead of raw Survivor.RadiationAnxiety.
        /// </summary>
        public float DisplayedRadiationAnxiety { get; private set; }

        public void SetNeeds(
            Needs needs,
            float health,
            float radiation,
            Survivor survivor,
            PersonalQuestSystem personalQuests,
            System.Random rng = null)
        {
            EnsureInitialized();
            if (needs == null) return;

            bool mask = personalQuests != null
                        && survivor != null
                        && personalQuests.ShouldMaskNeedsInUi(survivor, rng);

            UpdateNeed("hunger", mask ? 100f : needs.Hunger);
            UpdateNeed("thirst", mask ? 100f : needs.Thirst);
            UpdateNeed("fatigue", needs.Fatigue);
            UpdateNeed("warmth", needs.Warmth);
            UpdateNeed("morale", needs.Morale);
            UpdateNeed("health", mask ? 100f : health);
            UpdateNeed("radiation", radiation);

            // #264 Denialist: UI always reports RadiationAnxiety as 0.
            float realAnxiety = survivor != null ? survivor.RadiationAnxiety : 0f;
            DisplayedRadiationAnxiety = personalQuests != null && survivor != null
                ? personalQuests.GetDisplayedRadiationAnxiety(survivor, realAnxiety)
                : realAnxiety;
        }

        public void UpdateNeed(string needId, float value)
        {
            EnsureInitialized();
            if (!_needBars.TryGetValue(needId.ToLowerInvariant(), out var data)) return;

            data.CurrentValue = Mathf.Clamp(value, 0f, data.MaxValue);
            data.IsCritical = CheckIsCritical(needId.ToLowerInvariant(), data.CurrentValue);
            PulseOnEvent(needId);
        }

        public void PulseOnEvent(string needId)
        {
            _pulseTimers[needId.ToLowerInvariant()] = 0.5f;
        }

        public void SetShowRawValues(bool show)
        {
            _showRawValues = show;
        }

        private static bool CheckIsCritical(string needId, float val)
        {
            switch (needId)
            {
                case "hunger":
                case "thirst":
                case "fatigue":
                    return val >= 80f;
                case "warmth":
                case "morale":
                case "health":
                    return val <= 20f;
                case "radiation":
                    return val >= 60f;
                default:
                    return false;
            }
        }

        public Color GetActiveColor(string needId)
        {
            EnsureInitialized();
            if (_needBars.TryGetValue(needId.ToLowerInvariant(), out var data))
            {
                return data.IsCritical ? data.CriticalColor : data.NormalColor;
            }
            return Color.white;
        }

        public float GetPulse(string needId)
        {
            if (_pulseTimers.TryGetValue(needId.ToLowerInvariant(), out float t) && t > 0f)
            {
                return t;
            }
            return 0f;
        }
    }
}
