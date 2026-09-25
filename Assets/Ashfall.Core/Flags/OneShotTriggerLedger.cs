// SPDX-License-Identifier: MIT
// ============================================================================
// CORE-MECH W11 core — one-shot trigger primitive (shared by W8 and later waves).
//
// Plan: docs/plans/CORE_GAME_MECHANICS_GAP_SEAL_MASTER_INTEGRATION_PLAN.md
//       (W11-ONE-SHOT-TRIGGER-PRIMITIVE, appendices BK.3 / BL).
//
// The gap this closes: several waves need "fire exactly once, on or after day D,
// if gate G holds", and without a shared primitive each wave reinvents a local
// guard. This is that guard, once. It persists through the EXISTING flag ledger
// (no new save section, no second store), is pure and deterministic, and makes
// re-arming an explicit act rather than an accident.
// ============================================================================

using System;
using System.Collections.Generic;
using Ashfall.Core.Flags;

namespace Ashfall.Core.Flags
{
    /// <summary>Snapshot of the armed/fired state of one trigger.</summary>
    public sealed class OneShotTriggerState
    {
        public string TriggerId { get; set; } = string.Empty;
        public int DayThreshold { get; set; }
        public string GateFlagId { get; set; } = string.Empty;
        public bool Armed { get; set; }
        public bool Fired { get; set; }
        public int FiredDay { get; set; } = -1;
    }

    /// <summary>
    /// A named collection of one-shot triggers backed by an <see cref="IFlagLedger"/>.
    /// Keys are namespaced (<c>trigger.*</c>) so adopters cannot collide.
    /// </summary>
    public sealed class OneShotTriggerLedger
    {
        public const string KeyPrefix = "trigger.";

        private readonly IFlagLedger _ledger;
        private readonly Dictionary<string, OneShotTriggerState> _states =
            new Dictionary<string, OneShotTriggerState>(StringComparer.Ordinal);

        public OneShotTriggerLedger(IFlagLedger ledger)
        {
            _ledger = ledger ?? throw new ArgumentNullException(nameof(ledger));
        }

        private static string Key(string kind, string triggerId) => KeyPrefix + kind + "." + triggerId;

        /// <summary>
        /// Arm a trigger. Arming a trigger that already fired does nothing — the
        /// one-shot history is immutable unless <see cref="ReArm"/> is called.
        /// </summary>
        public void Arm(string triggerId, int dayThreshold, string gateFlagId = "")
        {
            if (string.IsNullOrWhiteSpace(triggerId)) return;
            if (_states.TryGetValue(triggerId, out var existing) && existing.Fired) return;

            _states[triggerId] = new OneShotTriggerState
            {
                TriggerId = triggerId,
                DayThreshold = Math.Max(0, dayThreshold),
                GateFlagId = gateFlagId ?? string.Empty,
                Armed = true,
                Fired = false,
                FiredDay = -1
            };
            Persist(triggerId);
        }

        /// <summary>
        /// Fire the trigger when it is armed, the day has arrived, and (when the
        /// caller set one) its gate flag is set. Exactly one caller can win: the
        /// fired flag is written before the call returns, so a second call in the
        /// same tick — or after a save/load — returns false.
        /// </summary>
        public bool TryFire(string triggerId, int day)
        {
            if (string.IsNullOrWhiteSpace(triggerId)) return false;
            var state = State(triggerId);
            if (state == null || !state.Armed || state.Fired) return false;
            if (day < state.DayThreshold) return false;
            if (!string.IsNullOrEmpty(state.GateFlagId) && !_ledger.IsSet(state.GateFlagId)) return false;

            state.Fired = true;
            state.FiredDay = day;
            _ledger.Set(Key("fired", triggerId), nameof(OneShotTriggerLedger), "one_shot", day, triggerId);
            return true;
        }

        /// <summary>Explicit re-arm. Never implicit, never automatic.</summary>
        public void ReArm(string triggerId, int dayThreshold = 0, string gateFlagId = "")
        {
            if (string.IsNullOrWhiteSpace(triggerId)) return;
            var state = State(triggerId);
            if (state == null)
            {
                Arm(triggerId, dayThreshold, gateFlagId);
                return;
            }
            state.Armed = true;
            state.Fired = false;
            state.FiredDay = -1;
            state.DayThreshold = Math.Max(0, dayThreshold);
            state.GateFlagId = gateFlagId ?? string.Empty;
            _ledger.Clear(Key("fired", triggerId));
            Persist(triggerId);
        }

        public bool HasFired(string triggerId)
            => State(triggerId)?.Fired ?? _ledger.IsSet(Key("fired", triggerId));

        public bool IsArmed(string triggerId) => State(triggerId)?.Armed ?? false;

        /// <summary>Known triggers, Ordinal-sorted for stable reporting.</summary>
        public IReadOnlyList<OneShotTriggerState> Census()
        {
            var ids = new List<string>(_states.Keys);
            ids.Sort(StringComparer.Ordinal);
            var list = new List<OneShotTriggerState>(ids.Count);
            foreach (var id in ids) list.Add(_states[id]);
            return list;
        }

        private OneShotTriggerState? State(string triggerId)
        {
            if (_states.TryGetValue(triggerId, out var state)) return state;
            // Rebuild from the persisted ledger so a reloaded campaign keeps its
            // one-shot history (AC.2 / AC.4).
            var armedFlag = Key("armed", triggerId);
            if (!_ledger.IsSet(armedFlag) && !_ledger.IsSet(Key("fired", triggerId))) return null;

            state = new OneShotTriggerState
            {
                TriggerId = triggerId,
                DayThreshold = _ledger.GetCounter(Key("day", triggerId)),
                GateFlagId = string.Empty,
                Armed = _ledger.IsSet(armedFlag),
                Fired = _ledger.IsSet(Key("fired", triggerId)),
                FiredDay = _ledger.IsSet(Key("fired", triggerId))
                    ? _ledger.GetCounter(Key("fired_day", triggerId))
                    : -1
            };
            _states[triggerId] = state;
            return state;
        }

        private void Persist(string triggerId)
        {
            var state = _states[triggerId];
            if (state.Armed) _ledger.Set(Key("armed", triggerId), nameof(OneShotTriggerLedger), "armed", 0, triggerId);
            else _ledger.Clear(Key("armed", triggerId));
            if (state.DayThreshold > 0) _ledger.Increment(Key("day", triggerId), state.DayThreshold, nameof(OneShotTriggerLedger), "threshold", 0, triggerId);
            if (state.Fired) _ledger.Increment(Key("fired_day", triggerId), Math.Max(1, state.FiredDay), nameof(OneShotTriggerLedger), "fired", state.FiredDay, triggerId);
        }
    }
}
