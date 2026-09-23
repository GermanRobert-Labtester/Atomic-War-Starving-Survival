// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core.Survivors;
using Godot;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private NeedsPerformanceHostSession? _needsPerformance;

        public NeedsPerformanceHostSession EnsureNeedsPerformance()
        {
            if (_needsPerformance == null)
            {
                _needsPerformance = new NeedsPerformanceHostSession(this);
                _needsPerformance.Initialize(_dataDir);
            }
            return _needsPerformance;
        }

        public void SetupNeedsPerformance()
        {
            EnsureNeedsPerformance();
        }

        public void ResetNeedsPerformance()
        {
            _needsPerformance?.Dispose();
            _needsPerformance = null;
        }

        public void TickNeedsPerformance(int day)
        {
            _needsPerformance?.TickDay(day);
        }

        public NeedsPerformanceModifiers GetNeedsPerformanceModifiers(string survivorId)
        {
            return _needsPerformance?.GetModifiers(survivorId) ?? NeedsPerformanceModifiers.Neutral;
        }

        public NeedsPerformanceCensus GetNeedsPerformanceCensus()
        {
            return _needsPerformance?.GetCensus() ?? new NeedsPerformanceCensus(0, 0, 0, 0);
        }
    }
}
