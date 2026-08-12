using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Environment
{
    /// <summary>
    /// Protocol Zero — Ash Tide Scheduler. A massive, roaming super-storm of
    /// radioactive black ice and ash that sweeps the map every 14 days.
    ///
    /// During an Ash-Tide, surface travel is locked. Ash piles up on exterior
    /// intake valves, degrading AirFiltration 5x faster. Survivors must
    /// manually enter the AirlockSystem in degrading hazmat suits to scrape
    /// the grates, risking lethal radiation exposure.
    ///
    /// Save/load safe. Plain C#.
    /// </summary>
    [Serializable]
    public class AshTideSave
    {
        public string systemId = "ash_tide";
        public bool isActive;
        public int lastTideDay;
        public int nextTideDay;
        public float hoursRemaining;
        public float grateClogPercent;
        public int totalTidesEndured;
    }

    /// <summary>
    /// Events raised by the Ash Tide system.
    /// </summary>
    public struct AshTideArrivedEvent
    {
        public int Day;
        public float DurationHours;
        public bool IsFirstTide;
    }

    public struct AshTideRecededEvent
    {
        public int TotalTidesEndured;
        public bool GrateClearedBySurvivors;
    }

    public class AshTideScheduler
    {
        /// <summary>Days between Ash-Tide storms.</summary>
        public const int TideIntervalDays = 14;

        /// <summary>Default duration of an Ash-Tide in game-hours.</summary>
        public const float DefaultTideDurationHours = 36f;

        /// <summary>Multiplier applied to AirFiltration degradation during tide.</summary>
        public const float FiltrationDegradeMultiplier = 5f;

        /// <summary>Grate clog rate per hour during tide.</summary>
        public const float GrateClogRatePerHour = 2.5f;

        /// <summary>Grate clearing rate per hour by a survivor in hazmat.</summary>
        public const float GrateClearingRatePerHour = 8f;

        /// <summary>Radiation dose per hour while scraping grates outside.</summary>
        public const float GrateClearingRadsPerHour = 12f;

        // ── Events ────────────────────────────────────────────────────
        public event Action<AshTideArrivedEvent> OnTideArrived;
        public event Action<AshTideRecededEvent> OnTideReceded;
        public event Action<float> OnGrateClogChanged; // percent 0..100

        // ── State ─────────────────────────────────────────────────────
        private bool _isActive;
        private int _lastTideDay;
        private int _nextTideDay = TideIntervalDays;
        private float _hoursRemaining;
        private float _grateClogPercent;
        private int _totalTidesEndured;
        private bool _grateClearedBySurvivors;

        public bool IsActive => _isActive;
        public float HoursRemaining => _hoursRemaining;
        public float GrateClogPercent => _grateClogPercent;
        public int TotalTidesEndured => _totalTidesEndured;
        public int NextTideDay => _nextTideDay;
        public bool IsSurfaceTravelLocked => _isActive;
        public float FiltrationMultiplier => _isActive ? FiltrationDegradeMultiplier : 1f;

        // ── Tick ──────────────────────────────────────────────────────
        /// <summary>
        /// Called every game-hour. Checks for tide arrival, advances duration,
        /// and models grate clog accumulation.
        /// </summary>
        public void Tick(float gameHours, int currentDay)
        {
            if (gameHours <= 0f) return;

            // Check for tide arrival
            if (!_isActive && currentDay >= _nextTideDay)
            {
                _isActive = true;
                _lastTideDay = currentDay;
                _hoursRemaining = DefaultTideDurationHours;
                _grateClogPercent = 0f;
                _grateClearedBySurvivors = false;

                OnTideArrived?.Invoke(new AshTideArrivedEvent
                {
                    Day = currentDay,
                    DurationHours = DefaultTideDurationHours,
                    IsFirstTide = _totalTidesEndured == 0
                });
            }

            // Advance tide
            if (_isActive)
            {
                _hoursRemaining = Mathf.Max(0f, _hoursRemaining - gameHours);
                _grateClogPercent = Mathf.Min(100f, _grateClogPercent + GrateClogRatePerHour * gameHours);
                OnGrateClogChanged?.Invoke(_grateClogPercent);

                // Tide receded
                if (_hoursRemaining <= 0f)
                {
                    _isActive = false;
                    _totalTidesEndured++;
                    _nextTideDay = _lastTideDay + TideIntervalDays;

                    OnTideReceded?.Invoke(new AshTideRecededEvent
                    {
                        TotalTidesEndured = _totalTidesEndured,
                        GrateClearedBySurvivors = _grateClearedBySurvivors
                    });
                }
            }
        }

        /// <summary>
        /// Send a survivor outside to scrape the grates. Returns radiation dose absorbed.
        /// Requires a working hazmat suit (caller checks).
        /// </summary>
        public float ScrapeGrates(float hoursSpent)
        {
            if (!_isActive || hoursSpent <= 0f) return 0f;

            _grateClogPercent = Mathf.Max(0f, _grateClogPercent - GrateClearingRatePerHour * hoursSpent);
            _grateClearedBySurvivors = true;
            OnGrateClogChanged?.Invoke(_grateClogPercent);

            return GrateClearingRadsPerHour * hoursSpent;
        }

        /// <summary>Force a tide for testing / scripted events.</summary>
        public void ForceTide(int currentDay, float durationHours = -1f)
        {
            _isActive = true;
            _lastTideDay = currentDay;
            _hoursRemaining = durationHours > 0f ? durationHours : DefaultTideDurationHours;
            _grateClogPercent = 0f;
            _grateClearedBySurvivors = false;
        }

        /// <summary>Force end the current tide.</summary>
        public void ForceClear()
        {
            _isActive = false;
            _hoursRemaining = 0f;
            _grateClogPercent = 0f;
        }

        // ── Save / Load ────────────────────────────────────────────────
        public AshTideSave CaptureState()
        {
            return new AshTideSave
            {
                isActive = _isActive,
                lastTideDay = _lastTideDay,
                nextTideDay = _nextTideDay,
                hoursRemaining = _hoursRemaining,
                grateClogPercent = _grateClogPercent,
                totalTidesEndured = _totalTidesEndured
            };
        }

        public void RestoreState(AshTideSave save)
        {
            if (save == null)
            {
                _isActive = false;
                _hoursRemaining = 0f;
                _grateClogPercent = 0f;
                _nextTideDay = TideIntervalDays;
                _totalTidesEndured = 0;
                _grateClearedBySurvivors = false;
                return;
            }

            _isActive = save.isActive;
            _lastTideDay = save.lastTideDay;
            _nextTideDay = save.nextTideDay;
            _hoursRemaining = save.hoursRemaining;
            _grateClogPercent = save.grateClogPercent;
            _totalTidesEndured = save.totalTidesEndured;
        }
    }
}
