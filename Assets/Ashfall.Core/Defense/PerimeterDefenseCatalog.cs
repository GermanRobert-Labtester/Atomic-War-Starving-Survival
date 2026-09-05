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
