// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Factions;
using Ashfall.Core.YearOfAsh;

namespace Ashfall.Core.Muster
{
    /// <summary>
    /// Deterministic Core engine managing Muster warfare operations, real survivor commitments,
    /// doctrine-guided conflict resolution, and aftermath propagation.
    /// Invariants:
    /// 1. Ghost soldiers are forbidden — all combatants map to unique living survivor IDs.
    /// 2. Supply conservation: Issued + Remaining + Lost + Returned == TotalReserved.
    /// 3. Zero engine coupling — deterministic xorshift/seeded PRNG.
    /// </summary>
    public sealed class MusterWarfareEngine
    {
        private readonly MusterWarfareState _state;

        public event Action<MusterWarfareState>? OnMobilized;
        public event Action<MusterEngagementResult>? OnEngagementWon;
        public event Action<MusterEngagementResult>? OnEngagementLost;
        public event Action<MusterWarfareState>? OnWithdrawn;
        public event Action<MusterEngagementResult>? OnHeavyCasualties;
        public event Action? OnStateChanged;

        public MusterWarfareEngine(MusterWarfareState? state = null)
        {
            _state = state ?? new MusterWarfareState();
        }

        public MusterWarfareState State => _state;
        public MusterConflictPhase Phase => _state.Phase;
        public IReadOnlyList<MusterSoldier> ActiveRoster => _state.ActiveRoster;
        public MusterSupplyReservation Supplies => _state.Supplies;

        public bool CanMobilize(
            string branchId,
            FactionBranchCoordinator? coordinator,
            IReadOnlyList<MusterSoldier>? soldiers,
            MusterSupplyReservation? supplies,
            out string? reason)
        {
            if (string.IsNullOrEmpty(branchId))
            {
                reason = "missing_branch_id";
                return false;
            }

            if (_state.Phase != MusterConflictPhase.Idle && _state.Phase != MusterConflictPhase.Cancelled)
            {
                reason = $"cannot_mobilize_in_phase_{_state.Phase}";
                return false;
            }

            // Exclusivity / Commitment Check with FactionBranchCoordinator
            if (coordinator != null && coordinator.IsCommitted)
            {
                var requestedKind = coordinator.DetectBranchKind(branchId);
                if (coordinator.ActiveFactionKind != requestedKind)
                {
                    reason = $"Cannot muster for {requestedKind}; already committed to {coordinator.ActiveFactionKind} branch '{coordinator.ActiveBranchId}'.";
                    return false;
                }
            }

            // Real Roster Check
            if (soldiers == null || soldiers.Count == 0)
            {
                reason = "empty_roster_no_soldiers";
                return false;
            }

            var seenIds = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < soldiers.Count; i++)
            {
                var s = soldiers[i];
                if (s == null || string.IsNullOrWhiteSpace(s.SurvivorId))
                {
                    reason = "invalid_soldier_missing_id";
                    return false;
                }
                if (!seenIds.Add(s.SurvivorId))
                {
                    reason = $"duplicate_survivor_in_roster_{s.SurvivorId}";
                    return false;
                }
            }

            // Supply Check
            if (supplies == null || supplies.FoodRations <= 0 || supplies.CleanWaterUnits <= 0)
            {
                reason = "insufficient_essential_supplies";
                return false;
            }

            reason = null;
            return true;
        }

        public ActionResult Mobilize(
            string branchId,
            IReadOnlyList<MusterSoldier> soldiers,
            MusterSupplyReservation supplies,
            string doctrineId,
            string targetSectorId,
            FactionBranchCoordinator? coordinator = null)
        {
            if (!CanMobilize(branchId, coordinator, soldiers, supplies, out var reason))
                return ActionResult.Blocked("mobilization_blocked", reason ?? "cannot_mobilize");

            _state.CommittedBranchId = branchId;
            _state.ActiveDoctrineId = string.IsNullOrEmpty(doctrineId) ? "warlord_doctrine_toll" : doctrineId;
            _state.TargetSectorId = targetSectorId ?? string.Empty;
            _state.ActiveRoster.Clear();
            foreach (var s in soldiers)
                _state.ActiveRoster.Add(s.Clone());

            _state.Supplies = supplies.Clone();
            _state.Phase = MusterConflictPhase.Mobilizing;
            _state.DaysInCurrentPhase = 0;

            OnMobilized?.Invoke(_state);
            OnStateChanged?.Invoke();
            return ActionResult.Success($"muster.mobilized:{branchId}");
        }

        public ActionResult SetReady()
        {
            if (_state.Phase != MusterConflictPhase.Mobilizing)
                return ActionResult.Blocked("not_mobilizing", $"cannot_ready_in_phase_{_state.Phase}");

            _state.Phase = MusterConflictPhase.Ready;
            _state.DaysInCurrentPhase = 0;
            OnStateChanged?.Invoke();
            return ActionResult.Success("muster.ready");
        }

        public ActionResult Deploy()
        {
            if (_state.Phase != MusterConflictPhase.Ready)
                return ActionResult.Blocked("not_ready", $"cannot_deploy_in_phase_{_state.Phase}");

            _state.Phase = MusterConflictPhase.Deployed;
            _state.DaysInCurrentPhase = 0;
            OnStateChanged?.Invoke();
            return ActionResult.Success("muster.deployed");
        }

        public ActionResult Engage()
        {
            if (_state.Phase != MusterConflictPhase.Deployed)
                return ActionResult.Blocked("not_deployed", $"cannot_engage_in_phase_{_state.Phase}");

            _state.Phase = MusterConflictPhase.Engaged;
            _state.DaysInCurrentPhase = 0;
            OnStateChanged?.Invoke();
            return ActionResult.Success("muster.engaged");
        }

        public ActionResult CancelMobilization()
        {
            if (_state.Phase != MusterConflictPhase.Mobilizing && _state.Phase != MusterConflictPhase.Ready)
                return ActionResult.Blocked("cannot_cancel", $"cannot_cancel_in_phase_{_state.Phase}");

            _state.Phase = MusterConflictPhase.Cancelled;
            _state.ActiveRoster.Clear();
            _state.DaysInCurrentPhase = 0;
            OnStateChanged?.Invoke();
            return ActionResult.Success("muster.cancelled");
        }

        public ActionResult Withdraw()
        {
            if (_state.Phase != MusterConflictPhase.Deployed && _state.Phase != MusterConflictPhase.Engaged)
                return ActionResult.Blocked("cannot_withdraw", $"cannot_withdraw_in_phase_{_state.Phase}");

            _state.Phase = MusterConflictPhase.Withdrawn;
            _state.RecoveryDaysRemaining = 2;
            _state.DaysInCurrentPhase = 0;

            OnWithdrawn?.Invoke(_state);
            OnStateChanged?.Invoke();
            return ActionResult.Success("muster.withdrawn");
        }

        public MusterEngagementResult ResolveEngagement(
            int seed,
            int day,
            float oppositionPower = 35.0f,
            FactionWarSystem? factionWar = null)
        {
            if (_state.Phase != MusterConflictPhase.Engaged)
                throw new InvalidOperationException($"Cannot resolve engagement in phase {_state.Phase}. Must be Engaged.");

            _state.Phase = MusterConflictPhase.Resolving;
            _state.EngagementSeed = seed;
            var rng = new SeededRng(seed);
            var doctrine = MusterDoctrineModifier.FromDoctrineId(_state.ActiveDoctrineId);

            // Compute total force combat power
            float totalPower = 0f;
            foreach (var soldier in _state.ActiveRoster)
            {
                float p = soldier.CombatStrength * Math.Clamp(soldier.Readiness, 0.2f, 1.2f);
                if (soldier.Role == MusterSoldierRole.Heavy) p *= 1.3f;
                else if (soldier.Role == MusterSoldierRole.Infantry) p *= 1.1f;
                totalPower += p;
            }

            // Apply munitions & doctrine boost
            if (_state.Supplies.AmmoUnits >= 20) totalPower += 10f;
            totalPower *= (1f + (doctrine.RiskTolerance - 0.5f) * 0.4f);

            // Consume operational supplies
            _state.Supplies.FoodRations = Math.Max(0, _state.Supplies.FoodRations - _state.ActiveRoster.Count);
            _state.Supplies.CleanWaterUnits = Math.Max(0, _state.Supplies.CleanWaterUnits - _state.ActiveRoster.Count);
            _state.Supplies.AmmoUnits = Math.Max(0, _state.Supplies.AmmoUnits - Math.Min(_state.Supplies.AmmoUnits, 15));

            float combatRoll = (float)rng.NextDouble() * 20f;
            float totalScore = totalPower + combatRoll;
            float margin = totalScore - oppositionPower;

            var result = new MusterEngagementResult
            {
                Day = day,
                Outcome = margin >= 5f ? "Victory" : (margin >= -5f ? "Stalemate" : "Defeat")
            };

            // Casualties and Wounds
            int casualtyCount = 0;
            if (result.Outcome == "Defeat")
            {
                casualtyCount = Math.Min(_state.ActiveRoster.Count, (int)Math.Ceiling(_state.ActiveRoster.Count * doctrine.CasualtyTolerance));
            }
            else if (result.Outcome == "Stalemate" && _state.ActiveRoster.Count > 2)
            {
                casualtyCount = 1;
            }

            for (int i = 0; i < _state.ActiveRoster.Count; i++)
            {
                var s = _state.ActiveRoster[i];
                if (i < casualtyCount)
                {
                    s.IsCasualty = true;
                    result.Casualties.Add(s.SurvivorId);
                }
                else if (rng.NextDouble() < 0.35)
                {
                    s.IsWounded = true;
                    result.Wounded.Add(s.SurvivorId);
                }
            }

            // Geopolitical Tension & Standing
            result.TensionDelta = result.Outcome == "Victory" ? doctrine.EscalationWeight : (doctrine.EscalationWeight / 2);
            result.StandingDelta = result.Outcome == "Victory" ? 20 : -15;

            if (factionWar != null)
            {
                factionWar.ModifyStanding(_state.CommittedBranchId, result.StandingDelta);
                // Adjust tension
                factionWar.State.activeWarTension = Math.Clamp(factionWar.State.activeWarTension + result.TensionDelta, 0, 100);
            }

            result.Summary = $"Muster engagement in {_state.TargetSectorId} concluded with {result.Outcome}. Casualties: {result.Casualties.Count}, Wounded: {result.Wounded.Count}.";
            result.EscalationEvent = $"muster_{result.Outcome.ToLowerInvariant()}";

            _state.LastEngagementResult = result;
            _state.Phase = MusterConflictPhase.Aftermath;
            _state.DaysInCurrentPhase = 0;

            if (result.Outcome == "Victory")
            {
                OnEngagementWon?.Invoke(result);
            }
            else
            {
                OnEngagementLost?.Invoke(result);
            }

            if (result.Casualties.Count >= 2 || (float)result.Casualties.Count / Math.Max(1, _state.ActiveRoster.Count) >= 0.4f)
            {
                OnHeavyCasualties?.Invoke(result);
            }

            OnStateChanged?.Invoke();
            return result;
        }

        public ActionResult CompleteAftermath(int recoveryDays = 2)
        {
            if (_state.Phase != MusterConflictPhase.Aftermath)
                return ActionResult.Blocked("not_in_aftermath", $"cannot_complete_aftermath_in_{_state.Phase}");

            _state.Phase = MusterConflictPhase.Recovering;
            _state.RecoveryDaysRemaining = Math.Max(1, recoveryDays);
            _state.DaysInCurrentPhase = 0;
            OnStateChanged?.Invoke();
            return ActionResult.Success("muster.recovering");
        }

        public void TickDay(int currentDay)
        {
            _state.DaysInCurrentPhase++;

            if (_state.Phase == MusterConflictPhase.Recovering)
            {
                _state.RecoveryDaysRemaining--;
                if (_state.RecoveryDaysRemaining <= 0)
                {
                    _state.Phase = MusterConflictPhase.Idle;
                    _state.ActiveRoster.Clear();
                    _state.DaysInCurrentPhase = 0;
                }
                OnStateChanged?.Invoke();
            }
        }
    }
}
