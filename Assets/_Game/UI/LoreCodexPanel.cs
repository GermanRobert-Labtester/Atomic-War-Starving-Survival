using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AtomicWar._Game.UI
{
    public class LoreCodexPanel : MonoBehaviour
    {
        [SerializeField] private UIDocument _document;
        private VisualElement _root, _historyContent, _factionsContent, _charactersContent;
        private ScrollView _historyScroll, _factionsScroll, _charactersScroll;
        private Button _tabHistory, _tabFactions, _tabCharacters, _closeButton;
        private readonly HashSet<string> _unlockedKeys = new HashSet<string>();
        private bool _bound;

        public void BindDocument(UIDocument doc) { _document = doc; _bound = false; }
        private void EnsureBound()
        {
            if (_bound || _document == null) return;
            _root = _document.rootVisualElement?.Q<VisualElement>("lore-codex-panel");
            if (_root == null) return;
            _tabHistory = _root.Q<Button>("tab-history");
            _tabFactions = _root.Q<Button>("tab-factions");
            _tabCharacters = _root.Q<Button>("tab-characters");
            _closeButton = _root.Q<Button>("close-button");
            _historyContent = _root.Q<VisualElement>("history-content");
            _factionsContent = _root.Q<VisualElement>("factions-content");
            _charactersContent = _root.Q<VisualElement>("characters-content");
            _historyScroll = _root.Q<ScrollView>("history-scroll");
            _factionsScroll = _root.Q<ScrollView>("factions-scroll");
            _charactersScroll = _root.Q<ScrollView>("characters-scroll");
            if (_tabHistory != null) _tabHistory.clicked += () => SwitchTab("history");
            if (_tabFactions != null) _tabFactions.clicked += () => SwitchTab("factions");
            if (_tabCharacters != null) _tabCharacters.clicked += () => SwitchTab("characters");
            if (_closeButton != null) _closeButton.clicked += Hide;
            _bound = true;
        }

        public void Show() { EnsureBound(); if (_root != null) _root.style.display = DisplayStyle.Flex; SwitchTab("history"); }
        public void Hide() { if (_root != null) _root.style.display = DisplayStyle.None; }

        public void SwitchTab(string tabId)
        {
            if (_historyContent != null) _historyContent.style.display = tabId == "history" ? DisplayStyle.Flex : DisplayStyle.None;
            if (_factionsContent != null) _factionsContent.style.display = tabId == "factions" ? DisplayStyle.Flex : DisplayStyle.None;
            if (_charactersContent != null) _charactersContent.style.display = tabId == "characters" ? DisplayStyle.Flex : DisplayStyle.None;
        }

        public void AddHistoryEntry(string knowledgeKey, string era, string date,
            string title, string body, string discoverySource, bool isDiscovered)
        {
            EnsureBound();
            if (_historyScroll == null) return;
            var eraSection = _historyScroll.Q<VisualElement>($"era-{era.Replace("_","-")}");
            if (eraSection == null) return;

            var entry = new VisualElement { name = knowledgeKey };
            entry.AddToClassList("history-entry");
            entry.Add(new Label(date) { style = { fontSize = 11, color = new StyleColor(new Color(1f, 0.76f, 0.03f)) } });
            entry.Add(new Label(title) { style = { fontSize = 14, color = Color.white } });

            if (isDiscovered)
            {
                entry.Add(new Label(body) { style = { fontSize = 12, color = new StyleColor(new Color(0.88f, 0.88f, 0.88f)) } });
                entry.Add(new Label($"Discovered via: {discoverySource}") { style = { fontSize = 10, color = new StyleColor(new Color(0.62f, 0.62f, 0.62f)), unityFontStyleAndWeight = FontStyle.Italic } });
            }
            else
            {
                var overlay = new VisualElement { style = { position = Position.Absolute, top = 0, left = 0, right = 0, bottom = 0, backgroundColor = new StyleColor(new Color(0,0,0,0.7f)) } };
                overlay.Add(new Label("UNDISCOVERED") { style = { fontSize = 14, color = new StyleColor(new Color(0.46f, 0.46f, 0.46f)), unityTextAlign = TextAnchor.MiddleCenter } });
                entry.Add(overlay);
            }
            eraSection.Add(entry);
            _unlockedKeys.Add(knowledgeKey);
        }

        public void UnlockHistoryEntry(string knowledgeKey)
        {
            EnsureBound();
            // Find and replace locked overlay with actual content — simplified: re-add
            _unlockedKeys.Add(knowledgeKey);
        }

        public void AddFactionEntry(string factionId, string displayName, string ideology,
            string originStory, string[] keyBeliefs, string signatureQuote,
            Dictionary<string, string> relationships, bool isDiscovered)
        {
            EnsureBound();
            if (_factionsScroll == null) return;
            var entry = new VisualElement { name = $"faction-{factionId}" };
            entry.AddToClassList("faction-entry");
            entry.Add(new Label(displayName) { style = { fontSize = 18, color = new StyleColor(new Color(1f, 0.76f, 0.03f)) } });
            entry.Add(new Label(ideology) { style = { fontSize = 12, color = new StyleColor(new Color(0.62f, 0.62f, 0.62f)), unityFontStyleAndWeight = FontStyle.Italic } });
            if (isDiscovered)
            {
                entry.Add(new Label(originStory) { style = { fontSize = 12, color = new StyleColor(new Color(0.88f, 0.88f, 0.88f)) } });
                if (keyBeliefs != null)
                    foreach (var b in keyBeliefs)
                        entry.Add(new Label($"• {b}") { style = { fontSize = 11, color = new StyleColor(new Color(0.76f, 0.76f, 0.76f)), marginLeft = 12 } });
                entry.Add(new Label($"\"{signatureQuote}\"") { style = { fontSize = 13, color = new StyleColor(new Color(1f, 0.76f, 0.03f)), unityFontStyleAndWeight = FontStyle.Italic, unityTextAlign = TextAnchor.MiddleCenter, marginTop = 8 } });
            }
            _factionsScroll.Add(entry);
        }

        public void AddCharacterEntry(string survivorId, string displayName,
            string profession, string bio, string[] traits, int arcMilestone,
            string arcBranch, bool isArcComplete)
        {
            EnsureBound();
            if (_charactersScroll == null) return;
            var entry = new VisualElement { name = $"char-{survivorId}" };
            entry.Add(new Label(displayName) { style = { fontSize = 16, color = Color.white } });
            entry.Add(new Label(profession) { style = { fontSize = 11, color = new StyleColor(new Color(0.62f, 0.62f, 0.62f)) } });
            entry.Add(new Label(bio) { style = { fontSize = 12, color = new StyleColor(new Color(0.88f, 0.88f, 0.88f)) } });
            if (traits != null)
                entry.Add(new Label($"Traits: {string.Join(", ", traits)}") { style = { fontSize = 10, color = new StyleColor(new Color(1f, 0.76f, 0.03f)) } });
            var arcLabel = new Label(isArcComplete ? $"Arc: Complete (Branch {arcBranch})" : $"Arc: Stage {arcMilestone}/3") { style = { fontSize = 10, color = new StyleColor(new Color(0.3f, 0.69f, 0.31f)) } };
            entry.Add(arcLabel);
            _charactersScroll.Add(entry);
        }

        public void UpdateCharacterArc(string survivorId, int newMilestone, string branch) { /* simplified */ }
    }
}
