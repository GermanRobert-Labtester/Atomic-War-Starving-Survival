// System_AdaptiveWarlords.cs — Adaptive AI for Warlord Encounters (Prompt #861)
// If player relied on Snipers in Playthrough A and died, Warlords in Playthrough B
// spawn with SmokeGrenades and Kevlar. Game learns dominant strategies across wipes.
using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Serializable state for the Adaptive Warlords system (Prompt #861).
    /// Tracks player strategies across playthroughs and generates counters.
    /// </summary>
    [Serializable]
    public class AdaptiveWarlordsState
    {
        public string system_id = "system_adaptive_warlords";
        public List<StrategyCount> previous_strategies = new List<StrategyCount>();
        public List<StrategyCounter> counters = new List<StrategyCounter>();
        public List<string> warlord_gear_modifiers = new List<string>();
    }

    [Serializable]
    public class StrategyCount
    {
        public string strategy_id;
        public int use_count;

        public StrategyCount() { }

        public StrategyCount(string id, int count)
        {
            strategy_id = id;
            use_count = count;
        }
    }

    [Serializable]
    public class StrategyCounter
    {
        public string strategy_id;
        public string counter_id;

        public StrategyCounter() { }

        public StrategyCounter(string strategy, string counter)
        {
            strategy_id = strategy;
            counter_id = counter;
        }
    }

    /// <summary>
    /// Adaptive Warlords system (Prompt #861).
    /// Tracks the top 3 most-used player strategies and generates counters:
    /// snipers → smoke + kevlar, turrets → EMP, stealth → dogs, traps → sappers.
    /// Applies to all warlord encounters in the next game.
    /// </summary>
    public class System_AdaptiveWarlords
    {
        // ── Events ─────────────────────────────────────────────────────
        public event Action<string, int> OnStrategyRecorded;
        public event Action<string, string> OnCounterApplied;
        public event Action<string, string> OnWarlordGearModified;

        // ── Constants ──────────────────────────────────────────────────
        private const int MaxTrackedStrategies = 3;

        // Hard-coded counter table (Prompt #861 rules)
        private static readonly Dictionary<string, string> CounterTable =
            new Dictionary<string, string>
            {
                { "snipers", "smoke_kevlar" },
                { "turrets", "emp" },
                { "stealth", "dogs" },
                { "traps", "sappers" }
            };

        // ── State ──────────────────────────────────────────────────────
        private AdaptiveWarlordsState _state = new AdaptiveWarlordsState();

        // ── Public API ─────────────────────────────────────────────────

        /// <summary>
        /// Record that the player used a particular strategy during gameplay.
        /// </summary>
        public void RecordStrategy(string strategyId)
        {
            var existing = _state.previous_strategies.Find(s => s.strategy_id == strategyId);
            if (existing != null)
            {
                existing.use_count++;
                OnStrategyRecorded?.Invoke(strategyId, existing.use_count);
            }
            else
            {
                _state.previous_strategies.Add(new StrategyCount(strategyId, 1));
                OnStrategyRecorded?.Invoke(strategyId, 1);
            }
        }

        /// <summary>
        /// Called at playthrough end. Sorts strategies, keeps top 3,
        /// and generates counters for the next playthrough.
        /// </summary>
        public void OnPlaythroughEnd()
        {
            // Sort descending by use count and keep top 3
            _state.previous_strategies.Sort((a, b) => b.use_count.CompareTo(a.use_count));
            if (_state.previous_strategies.Count > MaxTrackedStrategies)
            {
                _state.previous_strategies.RemoveRange(
                    MaxTrackedStrategies,
                    _state.previous_strategies.Count - MaxTrackedStrategies);
            }

            // Generate counters
            _state.counters.Clear();
            _state.warlord_gear_modifiers.Clear();

            for (int i = 0; i < _state.previous_strategies.Count; i++)
            {
                string strat = _state.previous_strategies[i].strategy_id;
                string counter = GetCounterStrategy(strat);
                if (!string.IsNullOrEmpty(counter))
                {
                    _state.counters.Add(new StrategyCounter(strat, counter));
                    _state.warlord_gear_modifiers.Add(counter);
                }
            }
        }

        /// <summary>
        /// Load previously generated counters (called at new game start).
        /// </summary>
        public void LoadCounters()
        {
            // Counters are already in state after RestoreState; nothing extra needed.
        }

        /// <summary>
        /// Returns warlord gear with counter modifiers appended.
        /// </summary>
        public string GetWarlordGear(string baseGear)
        {
            if (_state.warlord_gear_modifiers.Count == 0)
                return baseGear;

            string modified = baseGear;
            for (int i = 0; i < _state.warlord_gear_modifiers.Count; i++)
            {
                modified += "+" + _state.warlord_gear_modifiers[i];
                OnWarlordGearModified?.Invoke(baseGear, _state.warlord_gear_modifiers[i]);
            }
            return modified;
        }

        /// <summary>
        /// Returns the counter strategy id for a given player strategy,
        /// or empty string if no counter exists.
        /// </summary>
        public string GetCounterStrategy(string playerStrategy)
        {
            if (CounterTable.TryGetValue(playerStrategy, out string counter))
            {
                OnCounterApplied?.Invoke(playerStrategy, counter);
                return counter;
            }
            return string.Empty;
        }

        // ── Save / Load ────────────────────────────────────────────────

        public AdaptiveWarlordsState CaptureState()
        {
            return _state;
        }

        public void RestoreState(AdaptiveWarlordsState state)
        {
            _state = state ?? new AdaptiveWarlordsState();
        }
    }
}
