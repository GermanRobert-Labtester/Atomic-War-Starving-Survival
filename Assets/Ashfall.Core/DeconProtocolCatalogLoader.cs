using System;
using System.Collections.Generic;
using System.IO;
#pragma warning disable CS8618

namespace Ashfall.Core
{
    /// <summary>
    /// Decontamination protocol catalog loaded from
    /// <c>Assets/StreamingAssets/Data/decontamination_protocol_catalog.json</c>.
    /// Defines wash stages, effluent treatment parameters, and gear disposal rules.
    /// </summary>
    [Serializable]
    public sealed class DeconProtocolCatalog
    {
        public int schema_version = 1;
        public List<DeconProtocolDef> protocols = new List<DeconProtocolDef>();
        public DeconEffluentTreatmentDef effluent_treatment = new DeconEffluentTreatmentDef();
        public DeconGearDisposalDef gear_disposal = new DeconGearDisposalDef();
    }

    [Serializable]
    public sealed class DeconProtocolDef
    {
        public string protocol_id = string.Empty;
        public string display_name = string.Empty;
        public string description = string.Empty;
        public List<DeconStageDef> stages = new List<DeconStageDef>();
        public float total_water_liters;
        public int total_surfactant_units;
        public int total_chelator_units;
        public int total_duration_ticks;
        public float ideal_removal_target;
        public float interlock_threshold_mSv_per_h = 0.5f;
        public List<string> tags = new List<string>();
    }

    [Serializable]
    public sealed class DeconStageDef
    {
        public string stage_id = string.Empty;
        public int stage_order;
        public string display_name = string.Empty;
        public int duration_ticks;
        public float water_liters;
        public int surfactant_units;
        public int chelator_units;
        public float external_contamination_multiplier;
        public float effluent_contamination_contribution;
        public float equipment_wear_factor;
        public bool requires_operator;
        public float operator_skill_factor;
    }

    [Serializable]
    public sealed class DeconEffluentTreatmentDef
    {
        public float default_tank_capacity_liters = 200f;
        public int settling_ticks = 4;
        public float water_recovery_fraction = 0.15f;
        public float hazardous_sludge_per_liter = 0.02f;
        public string treatment_required_item = "item_lead_lined_effluent_filter";
        public float filter_lifetime_liters = 500f;
    }

    [Serializable]
    public sealed class DeconGearDisposalDef
    {
        public string disposal_item_id = "item_sealed_waste_bin";
        public float max_safe_contamination_cleaning = 0.95f;
        public float disposal_threshold = 0.85f;
    }

    /// <summary>
    /// Loads <c>decontamination_protocol_catalog.json</c> from the data directory.
    /// </summary>
    public static class DeconProtocolCatalogLoader
    {
        public static DeconProtocolCatalog Load(string dataDir, IFileIO files, IJsonSerializer json)
        {
            if (string.IsNullOrWhiteSpace(dataDir)) throw new ArgumentException("dataDir required", nameof(dataDir));
            if (files == null) throw new ArgumentNullException(nameof(files));
            if (json == null) throw new ArgumentNullException(nameof(json));

            string path = files.Combine(dataDir, "decontamination_protocol_catalog.json");
            if (!files.FileExists(path))
                return new DeconProtocolCatalog(); // Empty catalog; system operates without protocols

            string raw = files.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw))
                return new DeconProtocolCatalog();

            var catalog = json.Deserialize<DeconProtocolCatalog>(raw);
            if (catalog == null)
                throw new InvalidOperationException($"Failed to deserialize decontamination_protocol_catalog.json");

            // Validate structural integrity
            var seenProtocolIds = new HashSet<string>(StringComparer.Ordinal);
            foreach (var protocol in catalog.protocols)
            {
                if (string.IsNullOrEmpty(protocol.protocol_id))
                    throw new InvalidOperationException("Decon protocol catalog: protocol_id is required");
                if (!seenProtocolIds.Add(protocol.protocol_id))
                    throw new InvalidOperationException($"Decon protocol catalog: duplicate protocol_id '{protocol.protocol_id}'");

                var seenStageIds = new HashSet<string>(StringComparer.Ordinal);
                int expectedOrder = 0;
                foreach (var stage in protocol.stages)
                {
                    if (string.IsNullOrEmpty(stage.stage_id))
                        throw new InvalidOperationException($"Decon protocol '{protocol.protocol_id}': stage_id is required");
                    if (!seenStageIds.Add(stage.stage_id))
                        throw new InvalidOperationException($"Decon protocol '{protocol.protocol_id}': duplicate stage_id '{stage.stage_id}'");
                    if (stage.stage_order != expectedOrder)
                        throw new InvalidOperationException($"Decon protocol '{protocol.protocol_id}': stage order mismatch at '{stage.stage_id}' (expected {expectedOrder}, got {stage.stage_order})");
                    if (stage.duration_ticks <= 0)
                        throw new InvalidOperationException($"Decon protocol '{protocol.protocol_id}', stage '{stage.stage_id}': duration_ticks must be > 0");
                    if (stage.water_liters < 0)
                        throw new InvalidOperationException($"Decon protocol '{protocol.protocol_id}', stage '{stage.stage_id}': water_liters must be >= 0");
                    if (stage.external_contamination_multiplier < 0 || stage.external_contamination_multiplier > 1)
                        throw new InvalidOperationException($"Decon protocol '{protocol.protocol_id}', stage '{stage.stage_id}': external_contamination_multiplier must be 0-1");
                    expectedOrder++;
                }
            }

            return catalog;
        }
    }
}