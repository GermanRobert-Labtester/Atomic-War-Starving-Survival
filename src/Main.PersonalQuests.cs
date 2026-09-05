// SPDX-License-Identifier: MIT
// ASHFALL personal quests host triad (save enrollment for personal_quests section).

using Godot;
using Ashfall.Core;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private PersonalQuestHostSession? _personalQuests;
        private bool _personalQuestsDirty;

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
    }
}
