// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.IO;

namespace Ashfall.Core.Medical
{
    [Serializable]
    public sealed class SurgicalProcedureDefinition
    {
        public string procedure_id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string target_system { get; set; } = string.Empty;
        public string target_limb { get; set; } = string.Empty;
        public bool requires_operating_table { get; set; } = true;
        public string required_surgeon_skill { get; set; } = "skill_medical_triage";
        public List<string> required_tools { get; set; } = new List<string>();
        public Dictionary<string, int> consumable_costs { get; set; } = new Dictionary<string, int>(StringComparer.Ordinal);
        public int base_duration_hours { get; set; } = 4;
        public float base_shock_risk { get; set; } = 0.2f;
        public float base_complication_risk { get; set; } = 0.15f;
        public int recovery_days { get; set; } = 3;
    }

    [Serializable]
    public sealed class SurgicalProceduresContainer
    {
        public int schema_version { get; set; } = 1;
        public List<SurgicalProcedureDefinition> procedures { get; set; } = new List<SurgicalProcedureDefinition>();
    }

    public static class SurgicalProcedureCatalogLoader
    {
        public const string DefaultFileName = "surgical_procedures.json";

        public static List<SurgicalProcedureDefinition> Load(
            string dataDir,
            IFileIO? fileIO = null,
            IJsonSerializer? json = null)
        {
            fileIO ??= new FileSystemIO();
            json ??= new SystemTextJsonSerializer();

            string path = Path.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path))
            {
                return new List<SurgicalProcedureDefinition>();
            }

            try
            {
                string raw = fileIO.ReadAllText(path);
                var container = json.Deserialize<SurgicalProceduresContainer>(raw);
                return container?.procedures ?? new List<SurgicalProcedureDefinition>();
            }
            catch (Exception ex)
            {
                CatalogDiagnostics.Warn(path, "SurgicalProceduresContainer", ex);
                return new List<SurgicalProcedureDefinition>();
            }
        }
    }
}
