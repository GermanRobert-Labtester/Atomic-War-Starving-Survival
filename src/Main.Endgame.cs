// SPDX-License-Identifier: MIT
// ASHFALL campaign endgame & epilogue host wiring (Plan 84 / Task B25).

using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Endgame;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private EndgameHostSession? _endgame;
        private bool _endgameDirty;

        private void SetupEndgame()
        {
            if (_endgame != null) return;
            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("endgame") : new SeededRng(84);
            _endgame = EndgameHostSession.Create(_dataDir, rng);
            _endgame.StateChanged += () => _endgameDirty = true;

            var save = EndgameSaveStore.TryLoad();
            if (save != null)
            {
                _endgame.RestoreState(save);
            }
        }

        private void SaveEndgame()
        {
            if (_endgame == null) return;
            if (CaptureSection("endgame", _endgame.TryCapturePersisted()))
                _endgameDirty = false;
        }

        private void FlushEndgameIfDirty()
        {
            if (_endgameDirty) SaveEndgame();
        }

        public void CheckAndTriggerEndgame(int day)
        {
            if (_endgame == null || _endgame.IsSealed || _endgame.Phase != EndgamePhase.Active)
                return;

            int living = _survivors?.Roster?.LivingCount ?? (_survivors?.RosterState?.Count ?? 0);
            int dead = _survivorFate?.DeathCount ?? 0;
            float morale = 50f;
            int expeditions = _expeditions?.Definitions?.Count ?? 0;

            // Check trigger conditions: Extinction OR Day >= 360
            if (living == 0 || day >= 360)
            {
                var ctx = new CampaignEvaluationContext
                {
                    CurrentDay = day,
                    LivingSurvivors = living,
                    DeceasedSurvivors = dead,
                    AverageMorale = morale,
                    ExpeditionsCount = expeditions,
                    ForceExtinction = living == 0
                };
                _endgame.TriggerEnding(ctx);
                _endgameDirty = true;
                GD.Print($"[Main.Endgame] Endgame triggered on Day {day}: {_endgame.System.State.selectedEndingId}");
            }
        }
    }
}
