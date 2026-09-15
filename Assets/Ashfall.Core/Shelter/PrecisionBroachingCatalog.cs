// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.IO;

namespace Ashfall.Core.Shelter
{
    // ─────────────────────────────────────────────────────────────────
    // Plan 144 — Precision internal machining / industrial broaching
    // (catalog DTOs). Wave 1 contract only: data shapes + parse helper.
    // General industrial components only — no weapon-barrel content.
    // ─────────────────────────────────────────────────────────────────

    [Serializable]
    public sealed class BroachBenchDef
    {
        public string bench_id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public Dictionary<string, int> construction_required_items { get; set; } = new Dictionary<string, int>();
        public int construction_labor_days { get; set; } = 3;
        public float max_condition { get; set; } = 100f;
        public int maintenance_interval_days { get; set; } = 6;
        public Dictionary<string, int> maintenance_required_items { get; set; } = new Dictionary<string, int>();
        public string room_id { get; set; } = string.Empty;
        public string machine_load_class { get; set; } = "high";
    }

    [Serializable]
    public sealed class BroachOperationDef
    {
        public string operation_id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string input_item_id { get; set; } = string.Empty;
        public string tool_class { get; set; } = string.Empty;
        public int labor_ticks { get; set; } = 240;
        public string tolerance_target { get; set; } = "standard";
        public string output_item_id { get; set; } = string.Empty;
        public int tool_wear_per_job { get; set; } = 10;
        public int machine_load_permille { get; set; } = 500;
    }

    [Serializable]
    public sealed class BroachQualityTierDef
    {
        public string tier_id { get; set; } = string.Empty;
        /// <summary>Bounded quality multiplier in (0, 1]; feeds generic equipment-quality rules only.</summary>
        public float quality_value { get; set; } = 0.6f;
    }

    [Serializable]
    public sealed class PrecisionBroachingCatalog
    {
        public int schema_version { get; set; } = 1;
        public BroachBenchDef bench { get; set; } = new BroachBenchDef();
        public List<BroachOperationDef> operations { get; set; } = new List<BroachOperationDef>();
        public List<string> failure_states { get; set; } = new List<string>();
        public List<BroachQualityTierDef> quality_tiers { get; set; } = new List<BroachQualityTierDef>();

        public BroachOperationDef? FindOperation(string operationId)
        {
            foreach (var op in operations)
                if (op.operation_id == operationId) return op;
            return null;
        }

        public BroachQualityTierDef? FindQualityTier(string tierId)
        {
            foreach (var t in quality_tiers)
                if (t.tier_id == tierId) return t;
            return null;
        }

        /// <summary>Hardened parse: failures route through <see cref="CatalogDiagnostics"/> and return null (never throw).</summary>
        public static PrecisionBroachingCatalog? FromJson(string json, string path)
        {
            try { return System.Text.Json.JsonSerializer.Deserialize<PrecisionBroachingCatalog>(json); }
            catch (Exception e) { CatalogDiagnostics.Warn(path, "precision_broaching_catalog", e); return null; }
        }
    }
}
