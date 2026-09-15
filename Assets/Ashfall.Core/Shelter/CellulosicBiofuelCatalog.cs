// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.IO;

namespace Ashfall.Core.Shelter
{
    // ─────────────────────────────────────────────────────────────────
    // Plan 142 — Subterranean cellulosic biofuel industry (catalog DTOs).
    // Wave 1 contract only: data shapes + parse helper. Runtime wiring,
    // process engine, and host sessions land in later waves.
    // All values are game abstractions — no real process chemistry.
    // ─────────────────────────────────────────────────────────────────

    [Serializable]
    public sealed class BiofuelMachineDef
    {
        public string machine_id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public Dictionary<string, int> construction_required_items { get; set; } = new Dictionary<string, int>();
        public int construction_labor_days { get; set; } = 3;
        public float max_condition { get; set; } = 100f;
        public int maintenance_interval_days { get; set; } = 5;
        public Dictionary<string, int> maintenance_required_items { get; set; } = new Dictionary<string, int>();
        public string room_id { get; set; } = string.Empty;
        public string power_class { get; set; } = "high";
        public string ventilation_class { get; set; } = "industrial";
        public float ventilation_load_per_active_day { get; set; } = 0.06f;
    }

    [Serializable]
    public sealed class BiofuelProcessDef
    {
        public string process_id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public List<string> accepted_item_ids { get; set; } = new List<string>();
        public string feedstock_tag { get; set; } = "cellulose";
        public int batch_input_units { get; set; } = 10;
        public string conversion_tier { get; set; } = "standard";
        public int conversion_efficiency_permille { get; set; } = 480;
        public int fermentation_ticks { get; set; } = 2880;
        public int refining_ticks { get; set; } = 1440;
        public float energy_cost_kwh_per_day { get; set; } = 7f;
        public float process_water_units { get; set; } = 4f;
        public string fuel_output_id { get; set; } = string.Empty;
        public int yield_permille { get; set; } = 520;
        public int fouling_risk_permille { get; set; } = 90;
        public string vapor_hazard_tier { get; set; } = "elevated";
    }

    [Serializable]
    public sealed class BiofuelGradeDef
    {
        public string grade_id { get; set; } = string.Empty;
        public string output_item_id { get; set; } = string.Empty;
        /// <summary>Fuel-value worth when fed to the canonical power grid (grid fuel units per canister).</summary>
        public float grid_worth_units { get; set; } = 1f;
        /// <summary>Vehicle wear multiplier when burned in an expedition vehicle (&gt;= 1; worse fuel wears engines faster).</summary>
        public float vehicle_wear_multiplier { get; set; } = 1f;
    }

    [Serializable]
    public sealed class BiofuelHazardOutcomes
    {
        public List<string> faults { get; set; } = new List<string>();
        /// <summary>Semantic event name raised toward the industrial-fire hazard authority.</summary>
        public string fire_hazard_event { get; set; } = "IndustrialFireHazardRequested";
    }

    [Serializable]
    public sealed class BiofuelStorageDef
    {
        public int tank_max_units { get; set; } = 60;
    }

    [Serializable]
    public sealed class CellulosicBiofuelCatalog
    {
        public int schema_version { get; set; } = 1;
        public BiofuelMachineDef machine { get; set; } = new BiofuelMachineDef();
        public List<BiofuelProcessDef> processes { get; set; } = new List<BiofuelProcessDef>();
        public List<BiofuelGradeDef> fuel_grades { get; set; } = new List<BiofuelGradeDef>();
        public BiofuelHazardOutcomes hazard_outcomes { get; set; } = new BiofuelHazardOutcomes();
        public BiofuelStorageDef storage { get; set; } = new BiofuelStorageDef();

        public BiofuelProcessDef? FindProcess(string processId)
        {
            foreach (var p in processes)
                if (p.process_id == processId) return p;
            return null;
        }

        public BiofuelGradeDef? FindGrade(string gradeId)
        {
            foreach (var g in fuel_grades)
                if (g.grade_id == gradeId) return g;
            return null;
        }

        /// <summary>Hardened parse: failures route through <see cref="CatalogDiagnostics"/> and return null (never throw).</summary>
        public static CellulosicBiofuelCatalog? FromJson(string json, string path)
        {
            try { return System.Text.Json.JsonSerializer.Deserialize<CellulosicBiofuelCatalog>(json); }
            catch (Exception e) { CatalogDiagnostics.Warn(path, "cellulosic_ethanol_catalog", e); return null; }
        }
    }
}
