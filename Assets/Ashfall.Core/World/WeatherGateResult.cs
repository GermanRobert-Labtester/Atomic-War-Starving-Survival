using System;
using System.Collections.Generic;
using Ashfall.Core.World;

namespace Ashfall.Core.World
{
    /// <summary>
    /// Result of evaluating a gate set against a weather state (F9/F10/F12).
    /// Pure data: same inputs always produce the same result. Never mutates
    /// live route availability or the gate catalog.
    /// </summary>
    public sealed class WeatherGateResult
    {
        /// <summary>Gate states in catalog order, then ordinal gate id.</summary>
        public IReadOnlyList<WeatherGateState> States { get; init; }
            = Array.Empty<WeatherGateState>();

        /// <summary>Contiguous positive-gate windows derived from States (F10/A6).</summary>
        public IReadOnlyList<ForecastGateWindow> Windows { get; init; }
            = Array.Empty<ForecastGateWindow>();

        /// <summary>True when every evaluated gate is open.</summary>
        public bool AllOpen
        {
            get
            {
                foreach (var s in States)
                    if (!s.IsOpen) return false;
                return true;
            }
        }
    }
}
