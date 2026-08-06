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
        // ── Strategy / counter ids (snake_case) ────────────────────────
        public const string StrategySnipers = "snipers";
        public const string StrategyTurrets = "turrets";
        public const string StrategyStealth = "stealth";
        public const string StrategyTraps = "traps";

        public const string CounterSmokeKevlar = "smoke_kevlar";
        public const string CounterEmp = "emp";
        public const string CounterDogs = "dogs";
        public const string CounterSappers = "sappers";

        // ── Events ─────────────────────────────────────────────────────
        public event Action<string, int> OnStrategyRecorded;
        public event Action<string, string> OnCounterApplied;
        public event Action<string, string> OnWarlordGearModified;
        public event Action OnPlaythroughCountersReady;

        // ── Constants ──────────────────────────────────────────────────
        private const int MaxTrackedStrategies = 3;

        // Hard-coded counter table (Prompt #861 rules)
        private static readonly Dictionary<string, string> CounterTable =
            new Dictionary<string, string>(StringComparer.Ordinal)
            {
                { StrategySnipers, CounterSmokeKevlar },
                { StrategyTurrets, CounterEmp },
                { StrategyStealth, CounterDogs },
                { StrategyTraps, CounterSappers }
            };

        // ── State ──────────────────────────────────────────────────────
        private AdaptiveWarlordsState _state = new AdaptiveWarlordsState();

        public int TrackedStrategyCount =>
            _state.previous_strategies != null ? _state.previous_strategies.Count : 0;

        public int ActiveCounterCount =>
            _state.counters != null ? _state.counters.Count : 0;

        public IReadOnlyList<string> GearModifiers =>
            _state.warlord_gear_modifiers ?? (IReadOnlyList<string>)Array.Empty<string>();

        // ── Public API ─────────────────────────────────────────────────

        /// <summary>
        /// Record that the player used a particular strategy during gameplay.
        /// </summary>
        public void RecordStrategy(string strategyId)
        {
            if (string.IsNullOrEmpty(strategyId)) return;
            if (_state.previous_strategies == null)
                _state.previous_strategies = new List<StrategyCount>();

            for (int i = 0; i < _state.previous_strategies.Count; i++)
            {
                var existing = _state.previous_strategies[i];
                if (existing == null) continue;
                if (!string.Equals(existing.strategy_id, strategyId, StringComparison.Ordinal))
                    continue;
                existing.use_count++;
                OnStrategyRecorded?.Invoke(strategyId, existing.use_count);
                return;
            }

            _state.previous_strategies.Add(new StrategyCount(strategyId, 1));
            OnStrategyRecorded?.Invoke(strategyId, 1);
        }

        /// <summary>How many times a strategy was recorded this run (pre-trim).</summary>
        public int GetStrategyUseCount(string strategyId)
        {
            if (string.IsNullOrEmpty(strategyId) || _state.previous_strategies == null) return 0;
            for (int i = 0; i < _state.previous_strategies.Count; i++)
            {
                var s = _state.previous_strategies[i];
                if (s != null && string.Equals(s.strategy_id, strategyId, StringComparison.Ordinal))
                    return s.use_count;
            }
            return 0;
        }

        /// <summary>
        /// Called at playthrough end. Sorts strategies, keeps top 3,
        /// and generates counters for the next playthrough.
        /// </summary>
        public void OnPlaythroughEnd()
        {
            if (_state.previous_strategies == null)
                _state.previous_strategies = new List<StrategyCount>();

            // Drop null / empty entries before ranking.
            for (int i = _state.previous_strategies.Count - 1; i >= 0; i--)
            {
                var s = _state.previous_strategies[i];
                if (s == null || string.IsNullOrEmpty(s.strategy_id) || s.use_count <= 0)
                    _state.previous_strategies.RemoveAt(i);
            }

            // Sort descending by use count and keep top 3
            _state.previous_strategies.Sort((a, b) => b.use_count.CompareTo(a.use_count));
            if (_state.previous_strategies.Count > MaxTrackedStrategies)
            {
                _state.previous_strategies.RemoveRange(
                    MaxTrackedStrategies,
                    _state.previous_strategies.Count - MaxTrackedStrategies);
            }

            // Generate counters
            if (_state.counters == null) _state.counters = new List<StrategyCounter>();
            else _state.counters.Clear();
            if (_state.warlord_gear_modifiers == null)
                _state.warlord_gear_modifiers = new List<string>();
            else
                _state.warlord_gear_modifiers.Clear();

            for (int i = 0; i < _state.previous_strategies.Count; i++)
            {
                string strat = _state.previous_strategies[i].strategy_id;
                string counter = LookupCounter(strat);
                if (string.IsNullOrEmpty(counter)) continue;

                _state.counters.Add(new StrategyCounter(strat, counter));
                if (!_state.warlord_gear_modifiers.Contains(counter))
                    _state.warlord_gear_modifiers.Add(counter);
                OnCounterApplied?.Invoke(strat, counter);
            }

            OnPlaythroughCountersReady?.Invoke();
        }

        /// <summary>
        /// Load previously generated counters (called at new game start).
        /// Counters live in state after RestoreState — no extra work needed.
        /// </summary>
        public void LoadCounters()
        {
            // Intentionally empty: state is the source of truth after save restore.
        }

        /// <summary>
        /// Returns warlord gear with counter modifiers appended (e.g. "rifle+smoke_kevlar+dogs").
        /// </summary>
        public string GetWarlordGear(string baseGear)
        {
            if (string.IsNullOrEmpty(baseGear)) baseGear = "standard";
            if (_state.warlord_gear_modifiers == null || _state.warlord_gear_modifiers.Count == 0)
                return baseGear;

            string modified = baseGear;
            for (int i = 0; i < _state.warlord_gear_modifiers.Count; i++)
            {
                string mod = _state.warlord_gear_modifiers[i];
                if (string.IsNullOrEmpty(mod)) continue;
                modified += "+" + mod;
                OnWarlordGearModified?.Invoke(baseGear, mod);
            }
            return modified;
        }

        /// <summary>True if a counter gear tag is active for the next warlord spawn.</summary>
        public bool HasGearModifier(string counterId)
        {
            if (string.IsNullOrEmpty(counterId) || _state.warlord_gear_modifiers == null) return false;
            return _state.warlord_gear_modifiers.Contains(counterId);
        }

        /// <summary>
        /// Returns the counter strategy id for a given player strategy,
        /// or empty string if no counter exists. Does not raise events.
        /// </summary>
        public string GetCounterStrategy(string playerStrategy)
        {
            return LookupCounter(playerStrategy);
        }

        // ── Save / Load ────────────────────────────────────────────────

        public AdaptiveWarlordsState CaptureState()
        {
            var copy = new AdaptiveWarlordsState
            {
                system_id = "system_adaptive_warlords",
                previous_strategies = new List<StrategyCount>(),
                counters = new List<StrategyCounter>(),
                warlord_gear_modifiers = new List<string>()
            };

            if (_state.previous_strategies != null)
            {
                for (int i = 0; i < _state.previous_strategies.Count; i++)
                {
                    var s = _state.previous_strategies[i];
                    if (s == null || string.IsNullOrEmpty(s.strategy_id)) continue;
                    copy.previous_strategies.Add(new StrategyCount(s.strategy_id, s.use_count));
                }
            }

            if (_state.counters != null)
            {
                for (int i = 0; i < _state.counters.Count; i++)
                {
                    var c = _state.counters[i];
                    if (c == null || string.IsNullOrEmpty(c.strategy_id)) continue;
                    copy.counters.Add(new StrategyCounter(c.strategy_id, c.counter_id));
                }
            }

            if (_state.warlord_gear_modifiers != null)
            {
                for (int i = 0; i < _state.warlord_gear_modifiers.Count; i++)
                {
                    if (!string.IsNullOrEmpty(_state.warlord_gear_modifiers[i]))
                        copy.warlord_gear_modifiers.Add(_state.warlord_gear_modifiers[i]);
                }
            }

            return copy;
        }

        public void RestoreState(AdaptiveWarlordsState saved)
        {
            if (saved == null)
            {
                _state = new AdaptiveWarlordsState();
                return;
            }

            _state = new AdaptiveWarlordsState
            {
                system_id = "system_adaptive_warlords",
                previous_strategies = new List<StrategyCount>(),
                counters = new List<StrategyCounter>(),
                warlord_gear_modifiers = new List<string>()
            };

            if (saved.previous_strategies != null)
            {
                for (int i = 0; i < saved.previous_strategies.Count; i++)
                {
                    var s = saved.previous_strategies[i];
                    if (s == null || string.IsNullOrEmpty(s.strategy_id)) continue;
                    _state.previous_strategies.Add(new StrategyCount(s.strategy_id, s.use_count));
                }
            }

            if (saved.counters != null)
            {
                for (int i = 0; i < saved.counters.Count; i++)
                {
                    var c = saved.counters[i];
                    if (c == null || string.IsNullOrEmpty(c.strategy_id)) continue;
                    _state.counters.Add(new StrategyCounter(c.strategy_id, c.counter_id));
                }
            }

            if (saved.warlord_gear_modifiers != null)
            {
                for (int i = 0; i < saved.warlord_gear_modifiers.Count; i++)
                {
                    if (!string.IsNullOrEmpty(saved.warlord_gear_modifiers[i]))
                        _state.warlord_gear_modifiers.Add(saved.warlord_gear_modifiers[i]);
                }
            }
        }

        private static string LookupCounter(string playerStrategy)
        {
            if (string.IsNullOrEmpty(playerStrategy)) return string.Empty;
            return CounterTable.TryGetValue(playerStrategy, out string counter) ? counter : string.Empty;
        }
    }
}
