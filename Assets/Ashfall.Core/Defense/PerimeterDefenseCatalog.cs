// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.IO;

namespace Ashfall.Core.Defense
{
    [Serializable]
    public sealed class PerimeterDefenseDefinition
    {
        public string defense_id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string defense_type { get; set; } = string.Empty; // barrier, entanglement, early_warning, automated_turret, blast_barrier, observation
        public int max_hp { get; set; } = 200;
        public float cover_bonus { get; set; }
        public float slow_factor { get; set; }
        public int power_draw_watts { get; set; }
        public string required_ammo_type { get; set; } = string.Empty;
        public int magazine_capacity { get; set; }
        public float base_damage { get; set; }
        public int fire_rate_burst { get; set; }
        public float night_accuracy_bonus { get; set; }
        public bool prevents_stealth_breach { get; set; }
        public Dictionary<string, int> build_costs { get; set; } = new Dictionary<string, int>(StringComparer.Ordinal);

        // ── Plan 203 additions (additive; defaults preserve legacy catalogs) ──
        /// <summary>Attacker capability tags that neutralize this defense (cutting_tools, explosives, stealth, vehicle_breach, emp).</summary>
        public List<string> counter_tags { get; set; } = new List<string>();
        /// <summary>HP lost per severe-weather day (corrosion, frost heave, debris).</summary>
        public float weather_wear_per_storm { get; set; }
        /// <summary>Per-day base false-trigger chance (basis points) for alert devices.</summary>
        public int false_alarm_rate_bp { get; set; }
        /// <summary>True for triggered-alert devices: spends on trigger and needs reset.</summary>
        public bool alert_device { get; set; }

        // ── B5–B8 Phase 7 (Plan 67 §10.5) additions ──
        /// <summary>Research knowledge id gating construction (empty = basic
        /// fieldworks, no gate). Research is permission: the host queries the
        /// live capability and passes it to <c>ConstructEmplacement</c> — an
        /// unlock never grants the built emplacement (§15.3).</summary>
        public string required_knowledge { get; set; } = string.Empty;
    }

    /// <summary>Canonical perimeter sector ids (Plan 203 §6.4 — no second world map).</summary>
    public static class PerimeterSector
    {
        public const string North = "north";
        public const string East = "east";
        public const string South = "south";
        public const string West = "west";
        public const string Gate = "gate";

        public static readonly IReadOnlyList<string> All = new[] { North, East, South, West, Gate };

        public static bool IsValid(string sectorId)
        {
            foreach (var s in All) if (s == sectorId) return true;
            return false;
        }
    }

    [Serializable]
    public sealed class PerimeterSectorState
    {
        public string sector_id { get; set; } = string.Empty;
        public List<string> emplacement_ids { get; set; } = new List<string>();
        /// <summary>Alert devices in this sector are armed (not manually disarmed).</summary>
        public bool alarm_armed { get; set; } = true;
        /// <summary>Triggered devices await reset; stealth denial and re-trigger are suppressed.</summary>
        public bool alarm_spent { get; set; }
        public int last_trigger_day { get; set; }
        public int false_alarm_count { get; set; }
        public int hostile_trigger_count { get; set; }
    }

    [Serializable]
    public sealed class PerimeterIntrusionLogEntry
    {
        public int day { get; set; }
        public string sector_id { get; set; } = string.Empty;
        public string kind { get; set; } = string.Empty; // false_alarm | hostile_trigger | breach | repelled
        public string detail { get; set; } = string.Empty;
    }

    /// <summary>
    /// Encounter-context modifiers the combat/raid authority consumes at encounter
    /// start. The perimeter never runs combat (Plan 203 §6.1) — it supplies context.
    /// </summary>
    public sealed class PerimeterEncounterSnapshot
    {
        public bool stealth_denied { get; set; }
        public float movement_delay_multiplier { get; set; } = 1f;
        public float detection_initiative_bonus { get; set; }
        public List<string> protected_sectors { get; set; } = new List<string>();
        public List<string> countered_emplacement_ids { get; set; } = new List<string>();

        // ── B5–B8 expansion (§10.11 formal snapshot) — typed readiness counts ──
        /// <summary>Emplacements intact and answering (the raid loop's subject count).</summary>
        public int emplacements_ready { get; set; }
        /// <summary>Turrets currently armed with ammo (auto-reloading ammo semantics live in LoadAmmo).</summary>
        public int turrets_ready { get; set; }
        /// <summary>Emplacements destroyed or disabled in the field — the repair authority owns recovery.</summary>
        public int emplacements_disabled { get; set; }
        /// <summary>Turrets frozen by a jam (barrel-wear consequence; field-strip resets).</summary>
        public int turrets_jammed { get; set; }
        /// <summary>Sector alarm states by id (armed / spent / disarmed) — the early-warning readiness surface.</summary>
        public List<string> alarms_spent { get; set; } = new List<string>();
        /// <summary>Sectors with no intact emplacement — the honest breach-surface read.</summary>
        public List<string> unguarded_sectors { get; set; } = new List<string>();
    }

    [Serializable]
    public sealed class PerimeterDefensesContainer
    {
        public int schema_version { get; set; } = 1;
        public List<PerimeterDefenseDefinition> defenses { get; set; } = new List<PerimeterDefenseDefinition>();
    }

    public static class PerimeterDefenseCatalogLoader
    {
        public const string DefaultFileName = "perimeter_defenses.json";

        public static List<PerimeterDefenseDefinition> Load(
            string dataDir,
            IFileIO? fileIO = null,
            IJsonSerializer? json = null)
        {
            fileIO ??= new FileSystemIO();
            json ??= new SystemTextJsonSerializer();

            string path = Path.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path))
            {
                return new List<PerimeterDefenseDefinition>();
            }

            try
            {
                string raw = fileIO.ReadAllText(path);
                var container = json.Deserialize<PerimeterDefensesContainer>(raw);
                return container?.defenses ?? new List<PerimeterDefenseDefinition>();
            }
            catch (Exception ex)
            {
                CatalogDiagnostics.Warn(path, "PerimeterDefensesContainer", ex);
                return new List<PerimeterDefenseDefinition>();
            }
        }
    }
}
