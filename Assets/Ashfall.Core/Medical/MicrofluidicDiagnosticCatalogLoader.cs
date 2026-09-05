// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Medical
{
    /// <summary>
    /// Microfluidic diagnostic catalog loaded from
    /// <c>Assets/StreamingAssets/Data/microfluidic_diagnostic_catalog.json</c>.
    /// Assays and machine parameters are data authority — the engine registers from here.
    /// </summary>
    [Serializable]
    public sealed class MicrofluidicDiagnosticCatalog
    {
        public int schema_version = 1;
        public MicrofluidicCatalogMachineDef machine = new MicrofluidicCatalogMachineDef();
        public List<MicrofluidicCatalogAssayDef> assays = new List<MicrofluidicCatalogAssayDef>();
    }

    [Serializable]
    public sealed class MicrofluidicCatalogMachineDef
    {
        public float nominal_power_kw = 2.5f;
        public float curing_oven_temp_c = 65.0f;
        public int reader_channels = 4;
    }

    [Serializable]
    public sealed class MicrofluidicCatalogAssayDef
    {
        public string id = string.Empty;
        public string display_name_key = string.Empty;
        public string display_name = string.Empty;
        public List<string> target_disease_ids = new List<string>();
        public string cartridge_item_id = "item_microfluidic_cartridge_general";
        public List<string> reagent_item_ids = new List<string> { "item_assay_reagent_pack" };
        public string reader_item_id = "item_microfluidic_reader";
        public float base_duration_minutes = 30.0f;
        public float sensitivity = 0.94f;
        public float specificity = 0.98f;
        public float early_detection_modifier = 0.40f;
        public float invalid_run_base_chance = 0.04f;
        public float minimum_operator_skill = 0.35f;
    }

    public static class MicrofluidicDiagnosticCatalogLoader
    {
        public static MicrofluidicDiagnosticCatalog Load(string dataDir, IFileIO files, IJsonSerializer json)
        {
            if (string.IsNullOrWhiteSpace(dataDir)) throw new ArgumentException("dataDir required", nameof(dataDir));
            if (files == null) throw new ArgumentNullException(nameof(files));
            if (json == null) throw new ArgumentNullException(nameof(json));

            string path = files.Combine(dataDir, "microfluidic_diagnostic_catalog.json");
            if (!files.FileExists(path))
                return new MicrofluidicDiagnosticCatalog(); // Empty catalog; engine seeds stay in force

            string raw = files.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw))
                return new MicrofluidicDiagnosticCatalog();

            var catalog = json.Deserialize<MicrofluidicDiagnosticCatalog>(raw);
            if (catalog == null)
                throw new InvalidOperationException("Failed to deserialize microfluidic_diagnostic_catalog.json");

            var seenIds = new HashSet<string>(StringComparer.Ordinal);
            foreach (var assay in catalog.assays)
            {
                if (string.IsNullOrEmpty(assay.id) || !seenIds.Add(assay.id))
                    throw new InvalidOperationException($"Duplicate or empty microfluidic assay id: '{assay?.id}'");
                if (assay.base_duration_minutes <= 0f)
                    throw new InvalidOperationException($"microfluidic assay '{assay.id}' must have a positive duration");
                if (string.IsNullOrEmpty(assay.cartridge_item_id))
                    throw new InvalidOperationException($"microfluidic assay '{assay.id}' references an empty cartridge item id");
            }
            return catalog;
        }
    }

    /// <summary>Mapping helpers from catalog DTOs onto engine definitions.</summary>
    public static class MicrofluidicDiagnosticCatalogMapping
    {
        public static IEnumerable<MicrofluidicAssayDef> ToAssayDefs(this MicrofluidicDiagnosticCatalog catalog)
        {
            foreach (var a in catalog.assays)
            {
                yield return new MicrofluidicAssayDef
                {
                    Id = a.id,
                    DisplayName = string.IsNullOrEmpty(a.display_name) ? a.display_name_key : a.display_name,
                    TargetDiseaseIds = new List<string>(a.target_disease_ids),
                    CartridgeItemId = a.cartridge_item_id,
                    ReagentItemId = a.reagent_item_ids.Count > 0 ? a.reagent_item_ids[0] : "item_assay_reagent_pack",
                    ReaderItemId = a.reader_item_id,
                    BaseDurationMinutes = a.base_duration_minutes,
                    Sensitivity = a.sensitivity,
                    Specificity = a.specificity,
                    EarlyDetectionModifier = a.early_detection_modifier,
                    InvalidRunBaseChance = a.invalid_run_base_chance,
                    MinimumOperatorSkill = a.minimum_operator_skill
                };
            }
        }
    }
}
