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
            float morale = EstimateAverageMorale();
            // Lifetime completed sorties (not ActiveCount — that is in-flight only).
            int expeditions = _expeditions?.Engine?.CompletedCount ?? 0;

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

        /// <summary>
        /// Derives an authoritative endgame outcome snapshot from live campaign authorities (FX-01 / Plan 19).
        /// Both the manual epilogue surface and game-over resolution consume this single projection.
        /// </summary>
        public CampaignOutcomeSnapshot BuildCampaignOutcomeSnapshot()
        {
            SetupSurvivors();
            SetupExpansions();
            SetupVerdict();
            SetupRegionalTreaty();
            SetupSurvivorFate();
            SetupMemorial();
            SetupDoseLedger();

            int days = _yearOfAsh != null ? _yearOfAsh.Timeline.CurrentDay : _simDay;
            int living = _survivors?.Roster?.LivingCount ?? (_survivors?.RosterState?.Count ?? 0);
            int dead = System.Math.Max(_survivorFate?.DeathCount ?? 0, _memorial?.Entries?.Count ?? 0);

            var input = new CampaignOutcomeEvaluationInput
            {
                TotalDaysSurvived = days,
                LivingDwellerCount = living,
                TotalDeathsRecorded = dead,
                TreatiesState = _regionalTreaty?.System?.State,
                VerdictReckoningState = _verdict?.Reckoning?.State,
                EnrolledEvidenceCount = _verdict?.Evidence?.Count ?? 0,
                LedgerTampered = _expansions?.Ledger?.LedgerTampered ?? false,
                Debts = _expansions?.Ledger?.Contracts,
                CohortChildren = _doseLedger?.Cohort?.Children,
                GenerationalState = _expansions?.Generational?.CaptureState(),
                Flags = _consequenceLedger
            };

            return CampaignOutcomeEvaluator.Evaluate(input);
        }

        /// <summary>
        /// Mean morale across living roster survivors; neutral 50 when roster is empty/unavailable.
        /// </summary>
        private float EstimateAverageMorale()
        {
            var roster = _survivors?.RosterState;
            if (roster == null || roster.Count == 0)
                return 50f;

            float sum = 0f;
            int n = 0;
            for (int i = 0; i < roster.Count; i++)
            {
                var s = roster[i];
                if (s == null || !s.IsAliveState) continue;
                sum += s.Morale;
                n++;
            }

            return n > 0 ? sum / n : 50f;
        }
    }
}
