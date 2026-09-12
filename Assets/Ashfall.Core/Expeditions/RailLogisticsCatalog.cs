// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Expeditions
{
    /// <summary>One rail transit logistics edge definition from rail_logistics_catalog.json (Plan 73 §7.2).</summary>
    public sealed class RailLogisticsEdgeDef
    {
        public string rail_edge_id { get; set; } = string.Empty;
        public string from_node { get; set; } = string.Empty;
        public string to_node { get; set; } = string.Empty;
        public string gauge_tag { get; set; } = "standard_gauge";
        public string track_condition { get; set; } = "fair";
        public float grade { get; set; } = 0.0f;
        public string switchyard_id { get; set; } = string.Empty;
        public string clearance_requirement { get; set; } = "standard_clearance";
        public int derailment_risk_bp { get; set; } = 200;
        public string obstacle_profile { get; set; } = "clear";
        public string repair_requirement { get; set; } = "steel_rail_segment";
        public List<string> tags { get; set; } = new List<string>();
    }

    /// <summary>Container shape for rail_logistics_catalog.json (the authority).</summary>
    public sealed class RailLogisticsCatalogContainer
    {
        public int schema_version { get; set; } = 1;
        public List<RailLogisticsEdgeDef> edges { get; set; } = new List<RailLogisticsEdgeDef>();
    }

    /// <summary>
    /// Loads rail transit logistics definitions from JSON.
    /// Engine-agnostic: uses IFileIO and IJsonSerializer ports.
    /// RailwaySystem remains the sole rail authority — this catalog parameterizes
    /// edge logistics, grade, derailment risk, and obstacle profiles.
    /// </summary>
    public static class RailLogisticsCatalogLoader
    {
        public const string DefaultFileName = "rail_logistics_catalog.json";

        public static List<RailLogisticsEdgeDef> Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
                return new List<RailLogisticsEdgeDef>();

            string path = fileIO.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path))
                return new List<RailLogisticsEdgeDef>();

            string rawText = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(rawText))
                return new List<RailLogisticsEdgeDef>();

            var container = json.Deserialize<RailLogisticsCatalogContainer>(rawText);
            return container?.edges ?? new List<RailLogisticsEdgeDef>();
        }
    }
}
