using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Ecology
{
    /// <summary>
    /// Plan 28 Phase 4 — ecological infestation lifecycle for location and
    /// shelter infestations, rebuilt on the retired-content contract.
    ///
    /// Authority boundaries (no duplicate simulation):
    ///   • inventory/food  — food-loss routed through host callbacks
    ///   • disease         — Plan 09 port (<see cref="Disease.IDiseaseOutbreakSource"/>)
    ///   • market          — existing demand-adjust path only
    ///   • seasons         — Plan 19 <see cref="World.SeasonWindowDef"/>
    /// State is one small record per authored infestation id; every roll
    /// comes from the caller's per-day fork (deterministic, no wall clock).
    /// </summary>
    public sealed class EcologicalInfestationSystem
    {
        public const string SystemId = "ecological_infestation";

        /// <summary>Minimum days between recurring occurrences of one infestation.</summary>
        public const int RecurrenceCooldownDays = 5;

        /// <summary>Hard cap on per-day shelter food loss from any single infestation.</summary>
        public const int MaxFoodLossPerDay = 3;

        private readonly ILog _log;
        private readonly Dictionary<string, EcologicalInfestationDefinition> _definitions =
            new Dictionary<string, EcologicalInfestationDefinition>(StringComparer.Ordinal);
        private EcologicalInfestationState _state = new EcologicalInfestationState();

        public EcologicalInfestationState State => _state;

        public event Action<string>? OnInfestationTriggered;
        public event Action<string, bool>? OnInfestationClearAttempt;
        public event Action<string, string, int>? OnInfestationHarvested;
        public event Action<string>? OnInfestationExhausted;

        public EcologicalInfestationSystem(ILog? log = null)
        {
            _log = log ?? NullLog.Instance;
        }

        // ── Catalog ─────────────────────────────────────────────────────

        public void LoadDefinitions(IEnumerable<EcologicalInfestationDefinition>? definitions)
        {
            _definitions.Clear();
            foreach (var def in definitions ?? Enumerable.Empty<EcologicalInfestationDefinition>())
            {
                if (def == null || string.IsNullOrEmpty(def.id)) continue;
                _definitions[def.id] = def;
            }
        }

        public IReadOnlyCollection<EcologicalInfestationDefinition> Definitions => _definitions.Values;

        public bool TryGetDefinition(string infestationId, out EcologicalInfestationDefinition? def) =>
            _definitions.TryGetValue(infestationId ?? string.Empty, out def);

        // ── Queries ─────────────────────────────────────────────────────

        public EcologicalInfestationRecord? GetRecord(string infestationId)
        {
            if (string.IsNullOrEmpty(infestationId)) return null;
            return _state.records.Find(r => r != null
                && string.Equals(r.infestation_id, infestationId, StringComparison.Ordinal));
        }

        public EcologicalInfestationRecord GetOrCreateRecord(string infestationId)
        {
            var record = GetRecord(infestationId);
            if (record != null) return record;
            record = new EcologicalInfestationRecord { infestation_id = infestationId };
            _state.records.Add(record);
            return record;
        }

        private static bool IsActiveRecord(EcologicalInfestationRecord? r) =>
            r != null && (r.status == (int)EcologicalInfestationStatus.Active
                          || r.status == (int)EcologicalInfestationStatus.ToleratedHarvesting);

        public bool IsActive(string infestationId)
        {
            var r = GetRecord(infestationId);
            return r != null && (r.status == (int)EcologicalInfestationStatus.Active
                                 || r.status == (int)EcologicalInfestationStatus.ToleratedHarvesting);
        }

        // ── Lifecycle ───────────────────────────────────────────────────

        /// <summary>
        /// Deterministic daily eligibility for a trigger roll: definition
        /// known, not terminal, outside the recurrence cooldown, and (when
        /// authored) inside the current Plan 19 season window.
        /// </summary>
        public bool IsEligibleToTrigger(string infestationId, int currentDay, World.SeasonWindowDef? season)
        {
            if (!_definitions.TryGetValue(infestationId, out var def) || def == null) return false;
            var record = GetRecord(infestationId);
            if (record != null && record.status == (int)EcologicalInfestationStatus.ResolvedCleared) return false;
            if (record != null && record.last_action_day >= 0
                && currentDay - record.last_action_day < RecurrenceCooldownDays) return false;
            if (def.eligible_seasons is { Count: > 0 } && season != null
                && !def.eligible_seasons.Contains(season.id)) return false;
            return true;
        }

        /// <summary>
        /// Trigger an eligible infestation. <paramref name="dayRng"/> carries
        /// the authored daily chance; 0-chance definitions always trigger
        /// (host-scripted site events). Idempotent: an active or resolved
        /// infestation never re-triggers.
        /// </summary>
        public bool TryTrigger(string infestationId, int currentDay, ISeededRng? dayRng, out string reason)
        {
            if (!_definitions.TryGetValue(infestationId, out var def) || def == null)
            {
                reason = "unknown_infestation";
                return false;
            }
            var record = GetOrCreateRecord(infestationId);
            if (record.status != (int)EcologicalInfestationStatus.Inactive)
            {
                reason = "not_inactive";
                return false;
            }
            if (record.last_action_day >= 0 && currentDay - record.last_action_day < RecurrenceCooldownDays)
            {
                reason = "recovery_cooldown";
                return false;
            }
            if (def.trigger_chance_per_day > 0f)
            {
                if (dayRng == null)
                {
                    reason = "no_rng_fork";
                    return false;
                }
                _state.rollCount++;
                record.trigger_roll_count++;
                if (dayRng.NextDouble() > def.trigger_chance_per_day)
                {
                    reason = "roll_failed";
                    return false;
                }
            }

            record.status = (int)EcologicalInfestationStatus.Active;
            record.triggered_day = currentDay;
            record.last_action_day = currentDay;
            OnInfestationTriggered?.Invoke(infestationId);
            reason = "triggered";
            return true;
        }

        /// <summary>
        /// Attempt one authored clear option. The item cost is consumed
        /// through the caller's inventory authority (never a hidden write);
        /// the success roll comes from the caller's day fork.
        /// </summary>
        public bool TryClear(
            string infestationId, string optionId, int currentDay,
            Func<string, int, bool> consumeItem,
            ISeededRng dayRng,
            out string resultSummary)
        {
            resultSummary = string.Empty;
            if (!_definitions.TryGetValue(infestationId, out var def) || def == null)
            {
                resultSummary = "unknown_infestation";
                return false;
            }
            var record = GetRecord(infestationId);
            if (record == null || record.status == (int)EcologicalInfestationStatus.Inactive
                || record.status == (int)EcologicalInfestationStatus.ResolvedCleared)
            {
                resultSummary = "not_active";
                return false;
            }
            var option = def.clear_options?.Find(o => o != null
                && string.Equals(o.option_id, optionId, StringComparison.Ordinal));
            if (option == null)
            {
                resultSummary = "unknown_option";
                return false;
            }

            if (!string.IsNullOrEmpty(option.required_item_id) && option.required_item_count > 0)
            {
                if (consumeItem == null || !consumeItem(option.required_item_id, option.required_item_count))
                {
                    resultSummary = "missing_" + option.required_item_id;
                    return false;
                }
            }

            _state.rollCount++;
            if (dayRng == null || dayRng.NextDouble() <= Math.Clamp(option.success_chance, 0f, 1f))
            {
                record.status = (int)EcologicalInfestationStatus.ResolvedCleared;
                record.last_action_day = currentDay;
                resultSummary = string.IsNullOrEmpty(option.outcome_summary) ? "cleared" : option.outcome_summary;
                OnInfestationClearAttempt?.Invoke(infestationId, true);
                return true;
            }

            record.failed_clear_count++;
            record.last_action_day = currentDay;
            if (option.failure_backlash_chance > 0f && dayRng.NextDouble() < option.failure_backlash_chance
                && !string.IsNullOrEmpty(option.required_item_id))
            {
                consumeItem?.Invoke(option.required_item_id, 1);
                resultSummary = "backlash_" + option.required_item_id;
                return false;
            }
            resultSummary = "clear_failed";
            return false;
        }

        /// <summary>
        /// Tolerate an active infestation and take one bounded harvest of its
        /// benefit. Ongoing costs (food loss, hazard roll) continue in
        /// <see cref="TickDay"/> — "leave it" is a tradeoff, never free.
        /// </summary>
        public bool TryTolerateAndHarvest(
            string infestationId, int currentDay,
            out string itemId, out int amount, out string summary)
        {
            itemId = string.Empty;
            amount = 0;
            summary = string.Empty;
            if (!_definitions.TryGetValue(infestationId, out var def) || def == null)
            {
                summary = "unknown_infestation";
                return false;
            }
            var record = GetOrCreateRecord(infestationId);
            if (record.status != (int)EcologicalInfestationStatus.Active
                && record.status != (int)EcologicalInfestationStatus.ToleratedHarvesting)
            {
                summary = "not_active";
                return false;
            }
            if (string.IsNullOrEmpty(def.leave_resource_item_id) || def.leave_resource_amount <= 0)
            {
                summary = "no_beneficial_resource";
                return false;
            }
            if (record.harvests_taken >= Math.Max(1, def.max_harvests))
            {
                summary = "harvest_exhausted";
                return false;
            }

            record.status = (int)EcologicalInfestationStatus.ToleratedHarvesting;
            record.last_action_day = currentDay;
            record.harvests_taken++;
            itemId = def.leave_resource_item_id;
            amount = def.leave_resource_amount;
            summary = def.leave_benefit_summary;
            OnInfestationHarvested?.Invoke(infestationId, itemId, amount);

            if (record.harvests_taken >= Math.Max(1, def.max_harvests))
                OnInfestationExhausted?.Invoke(infestationId);
            return true;
        }

        /// <summary>
        /// Daily consequences for every active infestation. Bounded: at most
        /// one food-loss event and one hazard roll per infestation per day.
        /// All effects route through the host callbacks.
        /// </summary>
        /// <summary>
        /// Daily consequences for every active infestation, plus bounded
        /// natural die-off: when an authored season window ends, a seasonal
        /// infestation collapses back to Inactive (cooldown applies). This
        /// gives every crisis a terminal state even if the player never
        /// acts — "leave it" cannot run forever (28BB balance gate).
        /// </summary>
        public void TickDay(
            int day,
            ISeededRng? dayRng,
            Action<string, int>? foodLoss,
            Action<string>? diseaseRisk,
            World.SeasonWindowDef? season = null)
        {
            _state.rollCount++;
            foreach (var def in _definitions.Values)
            {
                if (def == null) continue;
                var record = GetRecord(def.id);
                if (record == null || (record.status != (int)EcologicalInfestationStatus.Active
                    && record.status != (int)EcologicalInfestationStatus.ToleratedHarvesting)) continue;

                // Seasonal die-off: warm-damp blooms die in hard cold; the
                // moth front dies at first hard frost. Natural, deterministic,
                // terminal for the window (cooldown gates the next occurrence).
                if (def.eligible_seasons is { Count: > 0 } && season != null
                    && !def.eligible_seasons.Contains(season.id))
                {
                    record.status = (int)EcologicalInfestationStatus.Inactive;
                    record.last_action_day = day;
                    _state.rollCount++;
                    continue;
                }

                if (def.food_loss_per_day > 0 && foodLoss != null)
                    foodLoss(def.id, Math.Min(def.food_loss_per_day, MaxFoodLossPerDay));

                if (!string.IsNullOrEmpty(def.linked_disease_id)
                    && def.tolerated_hazard_risk > 0f
                    && dayRng != null && dayRng.NextDouble() < def.tolerated_hazard_risk)
                {
                    diseaseRisk?.Invoke(def.linked_disease_id);
                    _state.rollCount++;
                }
            }
        }

        // ── Save (capture/restore) ──────────────────────────────────────

        /// <summary>Deep-capture for the checksummed campaign envelope; the
        /// live records must never alias the save state.</summary>
        public EcologicalInfestationState CaptureState()
        {
            var clone = new EcologicalInfestationState
            {
                schema_version = _state.schema_version,
                rollCount = _state.rollCount
            };
            foreach (var r in _state.records)
            {
                if (r == null) continue;
                clone.records.Add(new EcologicalInfestationRecord
                {
                    infestation_id = r.infestation_id,
                    status = r.status,
                    triggered_day = r.triggered_day,
                    last_action_day = r.last_action_day,
                    harvests_taken = r.harvests_taken,
                    failed_clear_count = r.failed_clear_count,
                    trigger_roll_count = r.trigger_roll_count
                });
            }
            return clone;
        }

        /// <summary>Restore wholesale; a reload never re-rolls or duplicates
        /// trigger outcomes (§12 — reload may not duplicate harvests or clear
        /// an infestation accidentally).</summary>
        public void RestoreState(EcologicalInfestationState? saved)
        {
            if (saved == null) return;
            _state = new EcologicalInfestationState
            {
                schema_version = saved.schema_version,
                rollCount = saved.rollCount
            };
            if (saved.records == null) return;
            foreach (var r in saved.records)
            {
                if (r == null) continue;
                _state.records.Add(new EcologicalInfestationRecord
                {
                    infestation_id = r.infestation_id,
                    status = r.status,
                    triggered_day = r.triggered_day,
                    last_action_day = r.last_action_day,
                    harvests_taken = r.harvests_taken,
                    failed_clear_count = r.failed_clear_count,
                    trigger_roll_count = r.trigger_roll_count
                });
            }
        }
    }
}
