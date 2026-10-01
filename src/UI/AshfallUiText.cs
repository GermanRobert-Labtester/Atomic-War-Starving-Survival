// SPDX-License-Identifier: MIT
using AtomicWar.GodotApp.Localization;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — shared localization accessors for Godot UI panels.
    ///
    /// <para>Task 4 (survival-legibility third wave): panels previously each
    /// re-declared a private <c>Tr</c>/<c>TrFormat</c> pair that could silently
    /// drift (one panel falling back to a raw key, another to English). This
    /// single helper is the one place a UI panel resolves a string key, so a
    /// future localization change lands everywhere at once.</para>
    ///
    /// <para>Presentation only — no simulation logic, no mutable state.</para>
    /// </summary>
    public static class AshfallUiText
    {
        /// <summary>Resolves a localized UI string, falling back to
        /// <paramref name="fallback"/> (or the key) when the catalog has no row.</summary>
        public static string Tr(string key, string? fallback = null)
            => AshfallLocalization.Tr(key, fallback);

        /// <summary>Resolves a localized format string with the supplied args.</summary>
        public static string TrFormat(string key, params object[] args)
            => AshfallLocalization.TrFormat(key, args);
    }
}
