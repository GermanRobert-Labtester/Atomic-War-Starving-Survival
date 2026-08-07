using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// H-5: Central registry of all game systems, categorized by tick frequency.
    /// Replaces the monolithic TickSystems() method in GameBootstrap with typed
    /// dispatch lists. Each system is registered in InitializeSystems() right
    /// after construction — if a system is constructed but not registered, the
    /// <see cref="GetUntickedSystems"/> diagnostic will flag it.
    ///
    /// Design (audit H-5): GameBootstrap was a 4,638-LOC god object where 22 of
    /// 26 newly added systems were constructed and save-wired but never ticked
    /// (C-1). The registry makes dead-state visible: a system that isn't in any
    /// tick list logs a warning in the diagnostics overlay.
    ///
    /// Usage in InitializeSystems:
    /// <code>
    ///   WeatherSystem = new WeatherSystem(profile, seed);
    ///   _registry.RegisterPerSubstep("weather", h => WeatherSystem.Tick(h));
    /// </code>
    ///
    /// The old per-system _lastXxxDay fields on GameBootstrap are consolidated
    /// into the registry's <see cref="_lastDailyRunDay"/> and per-key
    /// <see cref="_lastDayFlags"/> dictionary.
    /// </summary>
    public class SystemRegistry
    {
        // -----------------------------------------------------------------
        // Tick categories
        // -----------------------------------------------------------------

        /// <summary>
        /// Systems ticked every substep (every game-hour chunk). Signature:
        /// Action&lt;float&gt; where float = gameHours for this substep.
        /// </summary>
        private readonly List<(string name, Action<float> tick)> _perSubstep = new List<(string, Action<float>)>();

        /// <summary>
        /// Systems ticked once per game-day (idempotent on same day). Called
        /// at most once per day, usually from within the per-substep loop
        /// when the current day advances. Signature: Action&lt;int&gt; where
        /// int = currentDay.
        /// </summary>
        private readonly List<(string name, Action<int> tick)> _daily = new List<(string, Action<int>)>();

        /// <summary>
        /// Systems that are ONLY event-driven (no per-substep or daily tick).
        /// Registered for diagnostics so GetUntickedSystems doesn't flag them.
        /// </summary>
        private readonly HashSet<string> _eventDriven = new HashSet<string>();

        /// <summary>
        /// Systems that are save-only (state stored, no live tick). Registered
        /// for diagnostics so GetUntickedSystems doesn't flag them.
        /// </summary>
        private readonly HashSet<string> _saveOnly = new HashSet<string>();

        /// <summary>
        /// Every registered name across all four categories. Kept alongside the lists
        /// purely so <see cref="IsSystemTicked"/> is O(1): the C-1 diagnostic asks that
        /// question once per GameBootstrap system property, so a linear scan made the
        /// check quadratic in the number of systems.
        /// </summary>
        private readonly HashSet<string> _registeredNames = new HashSet<string>();

        // -----------------------------------------------------------------
        // Day-gating state (consolidated from GameBootstrap._lastXxxDay)
        // -----------------------------------------------------------------

        private int _lastDailyRunDay = -1;
        private readonly Dictionary<string, int> _lastDayFlags = new Dictionary<string, int>();

        /// <summary>Last day the daily pass ran (for tests).</summary>
        public int LastDailyRunDay => _lastDailyRunDay;

        /// <summary>Number of daily passes run (for tests).</summary>
        public int DailyRunCount { get; private set; }

        // -----------------------------------------------------------------
        // Registration
        // -----------------------------------------------------------------

        /// <summary>
        /// Register a system that ticks every substep. The tick delegate receives
        /// the gameHours delta for this substep. Called from InitializeSystems.
        /// </summary>
        public void RegisterPerSubstep(string name, Action<float> tick)
        {
            if (tick == null) return;
            _perSubstep.Add((name, tick));
            _registeredNames.Add(name);
        }

        /// <summary>
        /// Register a system that ticks once per game-day. The tick delegate
        /// receives the current day number. The registry handles idempotency:
        /// if called multiple times on the same day, only the first fires.
        /// Called from InitializeSystems.
        /// </summary>
        public void RegisterDaily(string name, Action<int> tick)
        {
            if (tick == null) return;
            _daily.Add((name, tick));
            _registeredNames.Add(name);
        }

        /// <summary>
        /// Register a system that is purely event-driven (fires via AI actions
        /// or EventBus, not via TickSystems). Used only for diagnostics.
        /// </summary>
        public void RegisterEventDriven(string name)
        {
            _eventDriven.Add(name);
            _registeredNames.Add(name);
        }

        /// <summary>
        /// Register a system that is save-only (state is captured/restored but
        /// no live tick — e.g., HiddenStorageSystem, ScrapWeaponSystem).
        /// Used only for diagnostics.
        /// </summary>
        public void RegisterSaveOnly(string name)
        {
            _saveOnly.Add(name);
            _registeredNames.Add(name);
        }

        /// <summary>
        /// Create a day-gated wrapper. The returned Action&lt;float&gt; can be
        /// registered via RegisterPerSubstep; it will only invoke the inner
        /// tick once per game-day. This replaces the scattered _lastXxxDay
        /// fields on GameBootstrap.
        ///
        /// Example:
        /// <code>
        ///   _registry.RegisterPerSubstep("deserter",
        ///       _registry.DayGated("deserter", day => DeserterSystem.TickDaily(...)));
        /// </code>
        /// </summary>
        public Action<float> DayGated(string key, Action<int> tick)
        {
            return gameHours =>
            {
                // This is called from the per-substep loop. We check the
                // current day from the context, which is injected at dispatch
                // time via _currentDayForDayGated.
                int day = _currentDayForDayGated;
                if (day < 0) return; // Not yet initialized.
                if (_lastDayFlags.TryGetValue(key, out int last) && last >= day)
                    return;
                _lastDayFlags[key] = day;
                tick(day);
            };
        }

        /// <summary>
        /// Current day set by TickAll before invoking per-substep ticks.
        /// Used by DayGated wrappers to check day advancement.
        /// </summary>
        private int _currentDayForDayGated = -1;

        // -----------------------------------------------------------------
        // Dispatch
        // -----------------------------------------------------------------

        /// <summary>
        /// Tick all registered systems. Call once per substep from
        /// GameBootstrap.TickSystems. The currentDay is passed down to
        /// DayGated wrappers and to daily ticks.
        /// </summary>
        public void TickAll(float gameHours, int currentDay)
        {
            if (gameHours <= 0f) return;

            // Set current day context for DayGated wrappers.
            _currentDayForDayGated = currentDay;

            // Per-substep ticks (always run).
            for (int i = 0; i < _perSubstep.Count; i++)
            {
                var (name, tick) = _perSubstep[i];
                try
                {
                    tick(gameHours);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"[SystemRegistry] Per-substep tick '{name}' threw: {ex.Message}");
                }
            }

            // Daily ticks (idempotent: once per game-day).
            if (currentDay != _lastDailyRunDay)
            {
                _lastDailyRunDay = currentDay;
                DailyRunCount++;
                for (int i = 0; i < _daily.Count; i++)
                {
                    var (name, tick) = _daily[i];
                    try
                    {
                        tick(currentDay);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"[SystemRegistry] Daily tick '{name}' threw: {ex.Message}");
                    }
                }
            }
        }

        // -----------------------------------------------------------------
        // Diagnostics (audit H-5: detect dead systems)
        // -----------------------------------------------------------------

        /// <summary>
        /// Systems that are known to be ticked (registered in any list).
        /// </summary>
        public int TickedSystemCount =>
            _perSubstep.Count + _daily.Count + _eventDriven.Count + _saveOnly.Count;

        /// <summary>
        /// Number of per-substep tick registrations.
        /// </summary>
        public int PerSubstepCount => _perSubstep.Count;

        /// <summary>
        /// Number of daily tick registrations.
        /// </summary>
        public int DailyCount => _daily.Count;

        /// <summary>
        /// Returns the names of all per-substep registered systems.
        /// For diagnostics overlays and tests.
        /// </summary>
        public IReadOnlyList<string> GetPerSubstepNames()
        {
            var names = new List<string>(_perSubstep.Count);
            for (int i = 0; i < _perSubstep.Count; i++)
                names.Add(_perSubstep[i].name);
            return names;
        }

        /// <summary>
        /// Returns the names of all daily registered systems.
        /// For diagnostics overlays and tests.
        /// </summary>
        public IReadOnlyList<string> GetDailyNames()
        {
            var names = new List<string>(_daily.Count);
            for (int i = 0; i < _daily.Count; i++)
                names.Add(_daily[i].name);
            return names;
        }

        /// <summary>
        /// Returns the names of all event-driven systems.
        /// </summary>
        public IReadOnlyCollection<string> GetEventDrivenNames() => _eventDriven;

        /// <summary>
        /// Returns the names of all save-only systems.
        /// </summary>
        public IReadOnlyCollection<string> GetSaveOnlyNames() => _saveOnly;

        /// <summary>
        /// Diagnostic: verify that a named system appears in at least one
        /// tick list or category. Useful in tests after InitializeSystems
        /// to catch the C-1 class of bug (system constructed but never ticked).
        /// </summary>
        public bool IsSystemTicked(string name) =>
            name != null && _registeredNames.Contains(name);

        /// <summary>
        /// Clear all registrations (for test teardown).
        /// </summary>
        public void Clear()
        {
            _perSubstep.Clear();
            _daily.Clear();
            _eventDriven.Clear();
            _saveOnly.Clear();
            _registeredNames.Clear();
            _lastDayFlags.Clear();
            _lastDailyRunDay = -1;
            DailyRunCount = 0;
            _currentDayForDayGated = -1;
        }
    }
}
