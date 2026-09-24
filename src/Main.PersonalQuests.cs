// SPDX-License-Identifier: MIT
// ASHFALL personal quests host triad (save enrollment for personal_quests section).

using Godot;
using Ashfall.Core;
using Ashfall.Core.Quests;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private PersonalQuestHostSession? _personalQuests;
        private PersonalQuestPanel? _personalQuestPanel;
        private bool _personalQuestsDirty;

        public PersonalQuestHostSession PersonalQuests => EnsurePersonalQuests();

        public PersonalQuestHostSession EnsurePersonalQuests()
        {
            if (_personalQuests != null) return _personalQuests;
            SetupPersonalQuests();
            return _personalQuests!;
        }

        private void SetupPersonalQuests()
        {
            if (_personalQuests != null) return;
            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("personal_quests") : new SeededRng(83);
            _personalQuests = PersonalQuestHostSession.Create(_dataDir, rng, new GodotLog());
            _personalQuests.StateChanged += () => _personalQuestsDirty = true;

            var saved = PersonalQuestSaveStore.TryLoad();
            if (saved != null)
                _personalQuests.RestoreState(saved);
        }

        public void TickPersonalQuests(int day)
        {
            EnsurePersonalQuests();
            _personalQuests?.TickDay(day);
            _personalQuestsDirty = true;
        }

        private void SavePersonalQuests()
        {
            if (_personalQuests == null) return;
            if (CaptureSection("personal_quests", _personalQuests.TryCapturePersisted()))
                _personalQuestsDirty = false;
        }

        private void FlushPersonalQuestsIfDirty()
        {
            if (_personalQuestsDirty) SavePersonalQuests();
        }

        private void SetupPersonalQuestPanel()
        {
            if (_personalQuestPanel != null && _personalQuestPanel.IsInsideTree())
                return;

            var session = EnsurePersonalQuests();
            _personalQuestPanel = new PersonalQuestPanel();
            _personalQuestPanel.Bind(session);
            _personalQuestPanel.OnClose += () => _personalQuestPanel.Visible = false;
            _personalQuestPanel.Visible = false;
            AddChild(_personalQuestPanel);
        }

        public PersonalQuestCensus GetPersonalQuestsCensus() =>
            _personalQuests?.System.GetCensus() ?? default;

        public void ResetPersonalQuests()
        {
            _personalQuests = null;
            _personalQuestsDirty = false;
        }

        public void ShowPersonalQuestPanel()
        {
            SetupPersonalQuestPanel();
            if (_personalQuestPanel != null)
            {
                _personalQuestPanel.Visible = true;
                _personalQuestPanel.RefreshView();
            }
        }
    }
}

