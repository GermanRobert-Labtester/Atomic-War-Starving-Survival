// SPDX-License-Identifier: MIT
// ============================================================================
// ASHFALL Plan 165 — Modding Support host surface.
// Reads the manifest-contract authority consulted by ModRuntime at startup and
// persists enable/disable selections through the sole user-settings authority.
// No campaign save section: a mod's enablement is a user preference, not
// campaign state.
// ============================================================================

using System;
using System.Linq;
using Godot;
using Ashfall.Core.Mods;
using AtomicWar.GodotApp.Host;
using AtomicWar.GodotApp.Settings;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        /// <summary>Plan 165 — the manifest-contract authority consulted this run.</summary>
        public ModSupportHostSession? ModSupport => ModRuntime.Support;

        public ModSupportCensus GetModSupportCensus() => ModRuntime.Support?.Census ?? default;

        /// <summary>The authority's deterministic dependency-ordered load order.</summary>
        public System.Collections.Generic.IReadOnlyList<string> GetModLoadOrder() =>
            ModRuntime.Support?.ResolveLoadOrder() ?? (System.Collections.Generic.IReadOnlyList<string>)Array.Empty<string>();

        /// <summary>
        /// Plan 165 — enables or disables a mod and persists the selection through
        /// the sole user-settings authority. Takes effect on the next startup
        /// (mods are composed before any campaign exists).
        /// </summary>
        public bool SetModEnabled(string modId, bool enabled)
        {
            var support = ModRuntime.Support;
            if (support == null || string.IsNullOrWhiteSpace(modId)) return false;
            if (!support.SetModEnabled(modId.Trim(), enabled)) return false;

            var settings = UserSettingsStore.Current;
            if (enabled)
            {
                if (!settings.EnabledMods.Contains(modId, StringComparer.Ordinal))
                    settings.EnabledMods.Add(modId.Trim());
            }
            else
            {
                settings.EnabledMods.RemoveAll(id => string.Equals(id, modId, StringComparison.Ordinal));
            }
            UserSettingsStore.Save(settings);
            return true;
        }
    }
}
