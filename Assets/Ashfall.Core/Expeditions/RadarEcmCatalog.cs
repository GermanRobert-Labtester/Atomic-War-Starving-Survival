// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.IO;

namespace Ashfall.Core.Expeditions
{
    // ─────────────────────────────────────────────────────────────────
    // Plan 143 — Wasteland electronic countermeasures (catalog DTOs).
    // Wave 1 contract only: data shapes + parse helper. The threat-lock
    // owner is TravelEncounterSystem (see SensorThreatState contract);
    // ECM profiles here provide bounded modifiers only. No real RF data.
    // ─────────────────────────────────────────────────────────────────

    [Serializable]
    public sealed class EcmProfileDef
    {
        public string profile_id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public List<string> modes { get; set; } = new List<string>();
        public string power_draw_class { get; set; } = "high";
        public int heat_rate_per_active_tick { get; set; } = 4;
        public int max_heat_permille { get; set; } = 1000;
        public int lock_break_bonus_permille { get; set; } = 220;
        public int detection_reduction_permille { get; set; } = 120;
        public int counter_detection_risk_permille { get; set; } = 120;
        public int condition_wear_per_active_tick { get; set; } = 4;
        public int cooldown_ticks { get; set; } = 240;
        public string install_item_id { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class EcmModeRuleDef
    {
        public string power_draw { get; set; } = "moderate";
        public int lock_break_bonus_permille { get; set; } = 0;
        public int counter_detection_risk_permille { get; set; } = 0;
        /// <summary>Deception-mode abstract false-track pressure (ghost contacts on the threat's picture).</summary>
        public int false_track_pressure { get; set; } = 0;
        /// <summary>Burst-mode active window in ticks; 0 = persistent mode.</summary>
        public int duration_ticks { get; set; } = 0;
    }

    [Serializable]
    public sealed class EcmThreatProfileDef
    {
        public string threat_profile_id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public int base_detection_permille { get; set; } = 400;
        public int lock_rate_permille { get; set; } = 120;
    }

    [Serializable]
    public sealed class EcmInvariantsDef
    {
        /// <summary>Hard cap on the lock-break bonus any single mode may contribute (anti-immunity bound).</summary>
        public int single_mode_lock_break_cap_permille { get; set; } = 400;
        /// <summary>Documentation invariant: ECM can never reduce lock confidence below attack capability.</summary>
        public bool no_immunity { get; set; } = true;
    }

    [Serializable]
    public sealed class RadarEcmCatalog
    {
        public int schema_version { get; set; } = 1;
        public List<EcmProfileDef> ecm_profiles { get; set; } = new List<EcmProfileDef>();
        public Dictionary<string, EcmModeRuleDef> mode_rules { get; set; } = new Dictionary<string, EcmModeRuleDef>();
        public List<EcmThreatProfileDef> threat_profiles { get; set; } = new List<EcmThreatProfileDef>();
        public EcmInvariantsDef invariants { get; set; } = new EcmInvariantsDef();

        public EcmProfileDef? FindProfile(string profileId)
        {
            foreach (var p in ecm_profiles)
                if (p.profile_id == profileId) return p;
            return null;
        }

        public EcmModeRuleDef? FindModeRule(string modeId) =>
            mode_rules.TryGetValue(modeId, out var rule) ? rule : null;

        public EcmThreatProfileDef? FindThreatProfile(string threatProfileId)
        {
            foreach (var t in threat_profiles)
                if (t.threat_profile_id == threatProfileId) return t;
            return null;
        }

        /// <summary>Hardened parse: failures route through <see cref="CatalogDiagnostics"/> and return null (never throw).</summary>
        public static RadarEcmCatalog? FromJson(string json, string path)
        {
            try { return System.Text.Json.JsonSerializer.Deserialize<RadarEcmCatalog>(json); }
            catch (Exception e) { CatalogDiagnostics.Warn(path, "radar_ecm_catalog", e); return null; }
        }
    }
}
