// SPDX-License-Identifier: MIT
// ASHFALL Plan 145 — Unified Ending Resolution & Epilogue Personalization Host Wiring.

using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Endgame;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private UnifiedEndingHostSession? _unifiedEnding;
        private bool _unifiedEndingDirty;

        public UnifiedEndingHostSession? UnifiedEnding => _unifiedEnding;

        public void SetupUnifiedEnding()
        {
            if (_unifiedEnding != null) return;

            _unifiedEnding = UnifiedEndingHostSession.Create(_dataDir);

            var saved = UnifiedEndingSaveStore.TryLoad();
            if (saved != null)
            {
                _unifiedEnding.RestoreState(saved);
            }

            _unifiedEnding.StateChanged += () => _unifiedEndingDirty = true;
        }

        public void SaveUnifiedEnding()
        {
            if (_unifiedEnding == null) return;
            var state = _unifiedEnding.CaptureState();
            UnifiedEndingSaveStore.TrySave(state);
            if (CaptureSection("unified_ending", UnifiedEndingSaveStore.TryCapturePersisted(state)))
            {
                _unifiedEndingDirty = false;
            }
        }

        public void TickUnifiedEnding(int day)
        {
            if (_unifiedEnding == null) SetupUnifiedEnding();
        }

        public void FlushUnifiedEndingIfDirty()
        {
            if (_unifiedEndingDirty)
            {
                SaveUnifiedEnding();
            }
        }

        public void ResetUnifiedEnding()
        {
            _unifiedEnding = null;
            _unifiedEndingDirty = false;
        }

        /// <summary>
        /// Projects live campaign authorities into the comprehensive <see cref="UnifiedEndingContext"/> (Plan 145).
        /// Gathers factual evidence from survivors, factions, quests, treaties, ledger, and research.
        /// </summary>
        public UnifiedEndingContext BuildCurrentUnifiedEndingContext()
        {
            SetupSurvivors();
            SetupExpansions();
            SetupVerdict();
            SetupRegionalTreaty();
            SetupSurvivorFate();
            SetupMemorial();
            SetupDoseLedger();
            SetupFactionBranch();
            SetupResearchUnlockBridge();

            int days = _yearOfAsh != null ? _yearOfAsh.Timeline.CurrentDay : _simDay;
            int living = _survivors?.Roster?.LivingCount ?? (_survivors?.RosterState?.Count ?? 0);
            int dead = System.Math.Max(_survivorFate?.DeathCount ?? 0, _memorial?.Entries?.Count ?? 0);

            var ctx = new UnifiedEndingContext
            {
                totalDaysSurvived = days,
                livingDwellerCount = living,
                totalDeathsRecorded = dead,
                grandTreatySigned = _regionalTreaty?.System?.State?.treaties?.Any(t => t.status == TreatyStatus.Active || t.status == TreatyStatus.Ratified) == true,
                tempestDecommissioned = _consequenceLedger != null && _consequenceLedger.IsSet("tempest_decommissioned"),
                debtLedgersBurned = _expansions?.Ledger?.LedgerTampered == true,
                childrenSurvived = (_doseLedger?.Cohort?.Children?.Count ?? 0) > 0,
                velSecretExposed = _consequenceLedger != null && _consequenceLedger.IsSet("vel_secret_exposed"),
                factionBranchId = _factionBranch?.Coordinator?.ActiveBranchId ?? "None",
                holdfastEndingId = _endgame?.System?.State?.selectedEndingId ?? string.Empty,
                verdictEndingId = _verdict?.Reckoning?.State?.countPresented == true ? "ending_verdict_the_sector_recounts"
                    : _verdict?.Reckoning?.State?.countHeld == true ? "ending_verdict_the_count_is_held"
                    : _verdict?.Reckoning?.State?.offerIsLease == true ? "ending_verdict_the_offer_is_a_lease"
                    : "None"
            };

            // Estimate moral choice band from average survivor morale
            float morale = EstimateAverageMorale();
            if (morale >= 75f) ctx.moralChoiceBand = "VeryPositive";
            else if (morale >= 60f) ctx.moralChoiceBand = "Positive";
            else if (morale <= 25f) ctx.moralChoiceBand = "VeryEvil";
            else if (morale <= 40f) ctx.moralChoiceBand = "Negative";
            else ctx.moralChoiceBand = "Neutral";

            // Extract living key survivor fates
            if (_survivors?.RosterState != null)
            {
                foreach (var s in _survivors.RosterState)
                {
                    if (s == null) continue;
                    var def = _survivors?.Roster?.FindDefinition(s.Id);
                    string name = def?.displayName ?? s.Id;
                    string trait = (def?.traitIds != null && def.traitIds.Count > 0) ? def.traitIds[0] : (def?.profession ?? "survivor");
                    ctx.keySurvivorFates.Add(new SurvivorEpilogueFate
                    {
                        survivorId = s.Id,
                        survivorName = name,
                        status = s.IsAliveState ? SurvivorFateStatus.Alive : SurvivorFateStatus.Deceased,
                        notableTrait = trait
                    });
                }
            }

            // Extract shelter upgrades from research unlock capabilities
            if (_researchUnlock?.UnlockedCapabilities != null)
            {
                foreach (var cap in _researchUnlock.UnlockedCapabilities)
                {
                    if (cap != null && cap.StartsWith("shelter:", StringComparison.OrdinalIgnoreCase))
                    {
                        ctx.shelterUpgrades.Add(cap.Substring("shelter:".Length));
                    }
                    else if (cap != null && cap.StartsWith("expedition:", StringComparison.OrdinalIgnoreCase))
                    {
                        ctx.expeditionDiscoveries.Add(cap.Substring("expedition:".Length));
                    }
                }
            }

            // Extract faction standings
            if (_yearOfAsh?.FactionWar?.State?.factions != null)
            {
                foreach (var f in _yearOfAsh.FactionWar.State.factions)
                {
                    if (f != null && !string.IsNullOrEmpty(f.factionId))
                    {
                        ctx.factionStandings[f.factionId] = f.standing;
                    }
                }
            }

            return ctx;
        }

        /// <summary>
        /// Resolves the campaign ending using Plan 145 UnifiedEndingResolver,
        /// generating personalized prose, survivor epilogues, and awarding legacy traits.
        /// </summary>
        public UnifiedEndingResult ResolveUnifiedEnding()
        {
            SetupUnifiedEnding();
            var ctx = BuildCurrentUnifiedEndingContext();
            var result = _unifiedEnding!.Resolve(ctx);
            _unifiedEndingDirty = true;
            return result;
        }
    }
}
