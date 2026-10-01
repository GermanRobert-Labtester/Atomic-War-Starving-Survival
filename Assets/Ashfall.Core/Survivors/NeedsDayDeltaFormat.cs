// SPDX-License-Identifier: MIT
using System;

namespace Ashfall.Core.Survivors
{
    /// <summary>
    /// Loop-5 hardening: presentation formatting for day-over-day need drift.
    /// Kept in Core so the rounding rule (notably the "-0" edge case) is unit
    /// testable without the Godot host.
    /// </summary>
    public static class NeedsDayDeltaFormat
    {
        /// <summary>
        /// Formats a signed whole-number drift, e.g. <c>+19</c> / <c>-4</c>.
        /// Rounds away from zero first so a small negative delta (e.g. -0.4)
        /// renders <c>0</c> rather than the misleading <c>-0</c>.
        /// </summary>
        public static string Signed(float value)
        {
            // Task 12 — a NaN/Infinity delta (corrupt baseline or a divide-by-
            // zero upstream) must render a truthful "0" instead of the raw
            // "NaN"/"Infinity" token leaking into the HUD.
            if (float.IsNaN(value) || float.IsInfinity(value)) return "0";
            // Clamp to the authored need range before the int cast: converting a
            // huge double to int is unspecified and must never leak.
            double clamped = Math.Clamp((double)value, -100d, 100d);
            int rounded = (int)Math.Round(clamped, MidpointRounding.AwayFromZero);
            if (rounded == 0) return "0";
            return $"{(rounded > 0 ? "+" : "")}{rounded}";
        }
    }
}
