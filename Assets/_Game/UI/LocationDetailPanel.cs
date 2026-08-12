using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AtomicWar._Game.UI
{
    /// <summary>Preview entry for a loot item in the location detail panel.</summary>
    public class LootPreviewEntry
    {
        public string ItemId;
        public string DisplayName;
        public float DropChance;
    }

    /// <summary>
    /// LocationDetailPanel — shows procedural danger, radiation, collapse risk,
    /// faction owner, and loot preview for a selected expedition location.
    /// </summary>
    public class LocationDetailPanel : MonoBehaviour
    {
        [SerializeField] private UIDocument _document;
        private VisualElement _root;
        private Label _locationName, _radiationValue, _factionName;
        private ProgressBar _radiationBar, _collapseBar;
        private VisualElement _dangerSkulls, _factionIcon, _lootPreview;
        private bool _bound;

        public void BindDocument(UIDocument doc) { _document = doc; _bound = false; }

        private void EnsureBound()
        {
            if (_bound || _document == null) return;
            _root = _document.rootVisualElement?.Q<VisualElement>("location-detail-panel");
            if (_root == null)
            {
                // #region agent log
                AtomicWar._Game.Utilities.AgentDebugLog.Write("H1", "LocationDetailPanel.EnsureBound", "root missing",
                    "{\"docNull\":" + (_document == null ? "true" : "false")
                    + ",\"visualRootNull\":" + (_document != null && _document.rootVisualElement == null ? "true" : "false") + "}");
                // #endregion
                return;
            }
            _locationName = _root.Q<Label>("location-name");
            _radiationBar = _root.Q<ProgressBar>("radiation-bar");
            _radiationValue = _root.Q<Label>("radiation-value");
            _collapseBar = _root.Q<ProgressBar>("collapse-bar");
            _dangerSkulls = _root.Q<VisualElement>("danger-skulls");
            _factionIcon = _root.Q<VisualElement>("faction-icon");
            _factionName = _root.Q<Label>("faction-name");
            _lootPreview = _root.Q<VisualElement>("loot-preview");
            _bound = true;
            // #region agent log
            AtomicWar._Game.Utilities.AgentDebugLog.Write("H1", "LocationDetailPanel.EnsureBound", "bound",
                "{\"rootFound\":true}");
            // #endregion
        }

        public void ShowLocation(string locationId, string displayName,
            float dangerLevel, float ambientSv, float collapseRisk,
            string factionOwner, List<LootPreviewEntry> lootPreview)
        {
            EnsureBound();
            if (_root == null) return;
            _root.style.display = DisplayStyle.Flex;
            if (_locationName != null) _locationName.text = displayName;

            // Danger skulls (0-5)
            if (_dangerSkulls != null)
            {
                _dangerSkulls.Clear();
                int skulls = Mathf.Clamp(Mathf.RoundToInt(dangerLevel * 5f), 0, 5);
                for (int i = 0; i < 5; i++)
                {
                    var skull = new Label("☠") { style = { opacity = i < skulls ? 1f : 0.2f, fontSize = 16, color = i < skulls ? new StyleColor(Color.red) : new StyleColor(Color.grey) } };
                    _dangerSkulls.Add(skull);
                }
            }

            if (_radiationBar != null) { _radiationBar.value = ambientSv * 100f; _radiationBar.highValue = 100f; }
            if (_radiationValue != null) _radiationValue.text = $"{ambientSv:F2} Sv/hr";
            if (_collapseBar != null) { _collapseBar.value = collapseRisk * 100f; _collapseBar.highValue = 100f; }
            if (_factionName != null) _factionName.text = string.IsNullOrEmpty(factionOwner) || factionOwner == "none" ? "Uncontrolled" : factionOwner;
            if (_factionIcon != null) { _factionIcon.Clear(); _factionIcon.AddToClassList($"faction-{factionOwner}"); }

            if (_lootPreview != null && lootPreview != null)
            {
                _lootPreview.Clear();
                foreach (var entry in lootPreview)
                {
                    var row = new VisualElement { style = { flexDirection = FlexDirection.Row } };
                    row.Add(new Label(entry.DisplayName) { style = { flexGrow = 1, fontSize = 12, color = new StyleColor(new Color(0.88f, 0.88f, 0.88f)) } });
                    row.Add(new Label($"{entry.DropChance:P0}") { style = { fontSize = 12, color = new StyleColor(new Color(1f, 0.76f, 0.03f)) } });
                    _lootPreview.Add(row);
                }
            }
        }

        public void Hide() { if (_root != null) _root.style.display = DisplayStyle.None; }
    }
}
