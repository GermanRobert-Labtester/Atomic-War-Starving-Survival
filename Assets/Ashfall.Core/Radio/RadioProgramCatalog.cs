// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Ashfall.Core.Radio
{
    /// <summary>
    /// Player-authored radio program template (Plan 173). Templates bind to an
    /// existing station schedule slot and may optionally name a PsyOps campaign id.
    /// They never own frequencies, stations, or schedule windows.
    /// </summary>
    public sealed class RadioProgramTemplateDef
    {
        public string id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        /// <summary>Existing <c>radio_stations.json</c> station id.</summary>
        public string station_id { get; set; } = string.Empty;
        /// <summary>Existing <see cref="RadioProgramSlot.SlotId"/> on that station.</summary>
        public string slot_id { get; set; } = string.Empty;
        /// <summary>Optional <c>propaganda_campaigns.json</c> id started on successful delivery.</summary>
        public string psyops_campaign_id { get; set; } = string.Empty;
        /// <summary>Prep work ticks required before the program may air.</summary>
        public int prep_ticks_required { get; set; } = 1;
        public string genre { get; set; } = "civilian_news";
        /// <summary>
        /// Inventory item ids the presenter must possess (not consumed). Empty = no gate.
        /// </summary>
        public List<string> required_equipment_item_ids { get; set; } = new List<string>();
        /// <summary>Optional inventory item consumed when prep starts.</summary>
        public string prep_cost_item_id { get; set; } = string.Empty;
        /// <summary>Count consumed from <see cref="prep_cost_item_id"/> on successful StartPrep.</summary>
        public int prep_cost_count { get; set; }
    }

    public sealed class RadioProgramCatalog
    {
        public int schema_version { get; set; } = 1;
        public List<RadioProgramTemplateDef> programs { get; set; } = new List<RadioProgramTemplateDef>();

        private readonly Dictionary<string, RadioProgramTemplateDef> _byId =
            new Dictionary<string, RadioProgramTemplateDef>(StringComparer.Ordinal);

        public void Index()
        {
            _byId.Clear();
            foreach (var program in programs)
            {
                if (program != null && !string.IsNullOrEmpty(program.id))
                    _byId[program.id] = program;
            }
        }

        public RadioProgramTemplateDef? Get(string programId)
        {
            if (string.IsNullOrEmpty(programId)) return null;
            _byId.TryGetValue(programId, out var def);
            return def;
        }

        public IReadOnlyCollection<RadioProgramTemplateDef> All => _byId.Values;
    }

    public static class RadioProgramCatalogLoader
    {
        public const string DefaultFileName = "radio_programs.json";

        public static RadioProgramCatalog Load(string dataDir, IFileIO fileIo)
        {
            if (fileIo == null || string.IsNullOrEmpty(dataDir))
                return Empty();

            string path = Path.Combine(dataDir, DefaultFileName);
            if (!fileIo.FileExists(path))
                return Empty();

            string json = fileIo.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(json))
                return Empty();

            var catalog = JsonSerializer.Deserialize<RadioProgramCatalog>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? Empty();

            catalog.Index();
            return catalog;
        }

        private static RadioProgramCatalog Empty()
        {
            var catalog = new RadioProgramCatalog();
            catalog.Index();
            return catalog;
        }
    }
}
