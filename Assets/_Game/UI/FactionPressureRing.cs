using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace AtomicWar._Game.UI
{
    [Serializable]
    public class FactionThreatSnapshot
    {
        public float GarrisonThreat01 = 0.5f;
        public float MilitiaThreat01 = 0.3f;
        public float CultistThreat01 = 0.15f;
        public float WarlordThreat01 = 0.05f;
        public string DominantFaction = "GARRISON";
    }

    public class FactionPressureRing : MonoBehaviour
    {
        [SerializeField] private UIDocument _document;
        [SerializeField] private FactionThreatSnapshot _data = new FactionThreatSnapshot();

        private VisualElement _root;
        private Label _dominantFactionLabel;
        private VisualElement _garrisonFill;
        private Label _garrisonValLabel;
        private VisualElement _militiaFill;
        private Label _militiaValLabel;
        private VisualElement _cultistFill;
        private Label _cultistValLabel;
        private VisualElement _warlordFill;
        private Label _warlordValLabel;

        public event Action<FactionThreatSnapshot> OnStateChanged;

        public FactionThreatSnapshot CurrentData => _data;

        private void OnEnable()
        {
            if (_document == null)
            {
                _document = GetComponent<UIDocument>();
            }

            if (_document != null && _document.rootVisualElement != null)
            {
                _root = _document.rootVisualElement.Q("faction-pressure-root") 
                      ?? _document.rootVisualElement.Q("faction_pressure_root");
                Bind();
                RefreshUI();
            }
        }

        private void Bind()
        {
            if (_root == null) return;

            _dominantFactionLabel = _root.Q<Label>("dominant_faction_label");
            _garrisonFill = _root.Q<VisualElement>("garrison_bar_fill");
            _garrisonValLabel = _root.Q<Label>("garrison_val_label");
            _militiaFill = _root.Q<VisualElement>("militia_bar_fill");
            _militiaValLabel = _root.Q<Label>("militia_val_label");
            _cultistFill = _root.Q<VisualElement>("cultist_bar_fill");
            _cultistValLabel = _root.Q<Label>("cultist_val_label");
            _warlordFill = _root.Q<VisualElement>("warlord_bar_fill");
            _warlordValLabel = _root.Q<Label>("warlord_val_label");
        }

        public void SetData(FactionThreatSnapshot snapshot)
        {
            if (snapshot == null) return;
            _data = snapshot;
            RefreshUI();
            OnStateChanged?.Invoke(_data);
        }

        public void UpdateThreats(float garrison, float militia, float cultist, float warlord)
        {
            _data.GarrisonThreat01 = Mathf.Clamp01(garrison);
            _data.MilitiaThreat01 = Mathf.Clamp01(militia);
            _data.CultistThreat01 = Mathf.Clamp01(cultist);
            _data.WarlordThreat01 = Mathf.Clamp01(warlord);

            // Determine dominant
            float max = _data.GarrisonThreat01;
            string dom = "GARRISON";
            if (_data.MilitiaThreat01 > max) { max = _data.MilitiaThreat01; dom = "MILITIA"; }
            if (_data.CultistThreat01 > max) { max = _data.CultistThreat01; dom = "CULTISTS"; }
            if (_data.WarlordThreat01 > max) { max = _data.WarlordThreat01; dom = "WARLORDS"; }
            _data.DominantFaction = dom;

            RefreshUI();
            OnStateChanged?.Invoke(_data);
        }

        private void RefreshUI()
        {
            if (_root == null) return;

            if (_dominantFactionLabel != null)
                _dominantFactionLabel.text = _data.DominantFaction;

            if (_garrisonFill != null) _garrisonFill.style.width = Length.Percent(_data.GarrisonThreat01 * 100f);
            if (_garrisonValLabel != null) _garrisonValLabel.text = $"{Mathf.RoundToInt(_data.GarrisonThreat01 * 100f)}%";

            if (_militiaFill != null) _militiaFill.style.width = Length.Percent(_data.MilitiaThreat01 * 100f);
            if (_militiaValLabel != null) _militiaValLabel.text = $"{Mathf.RoundToInt(_data.MilitiaThreat01 * 100f)}%";

            if (_cultistFill != null) _cultistFill.style.width = Length.Percent(_data.CultistThreat01 * 100f);
            if (_cultistValLabel != null) _cultistValLabel.text = $"{Mathf.RoundToInt(_data.CultistThreat01 * 100f)}%";

            if (_warlordFill != null) _warlordFill.style.width = Length.Percent(_data.WarlordThreat01 * 100f);
            if (_warlordValLabel != null) _warlordValLabel.text = $"{Mathf.RoundToInt(_data.WarlordThreat01 * 100f)}%";
        }

        public void Show() => _root?.RemoveFromClassList("hidden");
        public void Hide() => _root?.AddToClassList("hidden");
    }
}
