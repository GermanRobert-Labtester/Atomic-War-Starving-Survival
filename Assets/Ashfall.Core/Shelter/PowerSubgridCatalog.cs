// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.IO;

namespace Ashfall.Core.Shelter
{
    [Serializable]
    public sealed class PowerSubgridNodeDefinition
    {
        public string node_id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string target_room_id { get; set; } = string.Empty;
        public int max_capacity_watts { get; set; } = 3000;
        public int surge_limit_watts { get; set; } = 4000;
        public float transformer_oil_condition { get; set; } = 100.0f;
        public float cooling_efficiency { get; set; } = 0.9f;
        public int fuse_rating_amps { get; set; } = 25;
        public bool is_critical { get; set; }
    }

    [Serializable]
    public sealed class PowerSubgridNodesContainer
    {
        public int schema_version { get; set; } = 1;
        public List<PowerSubgridNodeDefinition> nodes { get; set; } = new List<PowerSubgridNodeDefinition>();
    }

    public static class PowerSubgridCatalogLoader
    {
        public const string DefaultFileName = "power_subgrid_nodes.json";

        public static List<PowerSubgridNodeDefinition> Load(
            string dataDir,
            IFileIO? fileIO = null,
            IJsonSerializer? json = null)
        {
            fileIO ??= new FileSystemIO();
            json ??= new SystemTextJsonSerializer();

            string path = Path.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path))
            {
                return new List<PowerSubgridNodeDefinition>();
            }

            try
            {
                string raw = fileIO.ReadAllText(path);
                var container = json.Deserialize<PowerSubgridNodesContainer>(raw);
                return container?.nodes ?? new List<PowerSubgridNodeDefinition>();
            }
            catch (Exception ex)
            {
                CatalogDiagnostics.Warn(path, "PowerSubgridNodesContainer", ex);
                return new List<PowerSubgridNodeDefinition>();
            }
        }
    }
}
