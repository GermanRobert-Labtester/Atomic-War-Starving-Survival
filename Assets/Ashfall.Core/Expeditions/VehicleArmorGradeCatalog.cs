// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Expeditions
{
    [Serializable]
    public sealed class VehicleArmorGradeCost
    {
        public string item_id = string.Empty;
        public int amount;
    }

    [Serializable]
    public sealed class VehicleArmorGradeDefinition
    {
        public string id = string.Empty;
        public string display_name = string.Empty;
        public string description = string.Empty;
        public int tier;
        public bool is_default;
        public int mitigation_permille;
        public int wear_absorption_permille;
        public int integrity_pool_permille;
        public float speed_multiplier_delta;
        public float fuel_consumption_multiplier = 1.0f;
        public List<string> compatible_terrain_types = new List<string>();
        public List<VehicleArmorGradeCost> install_cost = new List<VehicleArmorGradeCost>();
        public int install_labor_ticks;
        public List<VehicleArmorGradeCost> reforge_cost = new List<VehicleArmorGradeCost>();
        public List<string> tags = new List<string>();
    }

    [Serializable]
    public sealed class VehicleArmorGradeCatalog
    {
        public int schema_version = 1;
        public string default_grade_id = string.Empty;
        public List<VehicleArmorGradeDefinition> grades = new List<VehicleArmorGradeDefinition>();
    }

    /// <summary>Read-only vehicle-facing projection of a fitted armor grade.</summary>
    public sealed class VehicleArmorProfile
    {
        public string GradeId = string.Empty;
        public string DisplayName = string.Empty;
        public int Tier;
        public bool IsDefault;
        public int MitigationPermille;
        public int WearAbsorptionPermille;
        public int IntegrityPermille;
        public int IntegrityMaxPermille;
        public string MaterialProfileId = string.Empty;
        public string Purity = "Standard";
        public string ConditionBand = "none";
        public float SpeedMultiplierDelta;
        public float FuelConsumptionMultiplier = 1f;
    }

    public sealed class VehicleArmorGradeLoadResult
    {
        public VehicleArmorGradeCatalog? Catalog { get; internal set; }
        public List<string> Errors { get; } = new List<string>();
        public bool HasErrors => Errors.Count > 0;
    }

    /// <summary>
    /// Engine-neutral loader for the vehicle armor grade authority. Detailed
    /// bounds, vocabulary, item references, and ladder checks live in
    /// CatalogIntegrityValidator so the data gate can report every authored
    /// defect in one run.
    /// </summary>
    public static class VehicleArmorGradeCatalogLoader
    {
        public const string FileName = "vehicle_armor_grades.json";
        public const int CurrentSchemaVersion = 1;

        public static VehicleArmorGradeLoadResult Load(string dataDir, IFileIO files, IJsonSerializer json)
        {
            var result = new VehicleArmorGradeLoadResult();
            if (files == null || json == null || string.IsNullOrEmpty(dataDir))
            {
                result.Errors.Add("vehicle_armor_grades.json: loader requires dataDir, IFileIO and IJsonSerializer");
                return result;
            }

            string path = files.Combine(dataDir, FileName);
            if (!files.FileExists(path))
            {
                result.Errors.Add("vehicle_armor_grades.json: catalog file missing");
                return result;
            }

            string raw = files.ReadAllText(path);
            return LoadJson(raw, json);
        }

        public static VehicleArmorGradeLoadResult LoadJson(string raw, IJsonSerializer json)
        {
            var result = new VehicleArmorGradeLoadResult();
            if (json == null)
            {
                result.Errors.Add("vehicle_armor_grades.json: serializer is required");
                return result;
            }
            if (string.IsNullOrWhiteSpace(raw))
            {
                result.Errors.Add("vehicle_armor_grades.json: catalog file empty");
                return result;
            }

            VehicleArmorGradeCatalog? catalog;
            try
            {
                catalog = json.Deserialize<VehicleArmorGradeCatalog>(raw);
            }
            catch (Exception ex)
            {
                result.Errors.Add("vehicle_armor_grades.json: malformed JSON: " + ex.Message);
                return result;
            }

            if (catalog == null)
            {
                result.Errors.Add("vehicle_armor_grades.json: catalog parsed to null");
                return result;
            }
            if (catalog.schema_version > CurrentSchemaVersion)
                result.Errors.Add($"vehicle_armor_grades.json: schema {catalog.schema_version} is newer than supported {CurrentSchemaVersion}");
            if (catalog.grades == null)
            {
                result.Errors.Add("vehicle_armor_grades.json: grades array is null");
                return result;
            }

            var seen = new HashSet<string>(StringComparer.Ordinal);
            foreach (var grade in catalog.grades)
            {
                if (grade == null)
                {
                    result.Errors.Add("vehicle_armor_grades.json: grades contains a null row");
                    continue;
                }
                if (string.IsNullOrWhiteSpace(grade.id))
                    result.Errors.Add("vehicle_armor_grades.json: grade id is missing");
                else if (!seen.Add(grade.id))
                    result.Errors.Add($"vehicle_armor_grades.json: duplicate grade id '{grade.id}'");
            }

            result.Catalog = catalog;
            return result;
        }
    }
}
