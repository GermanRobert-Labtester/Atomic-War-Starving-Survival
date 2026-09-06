// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Ashfall.Core.Research
{
    public sealed class PrewarArchiveDef
    {
        public string id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public string encryption_grade { get; set; } = "basic";
        public string required_room { get; set; } = "room_laboratory_research";
        public string cleaning_solvent_id { get; set; } = "chemicals";
        public int cleaning_solvent_count { get; set; } = 1;
        public float base_effort_points { get; set; } = 100.0f;
        public List<string> reward_research_ids { get; set; } = new List<string>();
        public List<string> tags { get; set; } = new List<string>();
    }

    public sealed class PrewarArchiveCatalog
    {
        public int schema_version { get; set; } = 1;
        public List<PrewarArchiveDef> archives { get; set; } = new List<PrewarArchiveDef>();

        private readonly Dictionary<string, PrewarArchiveDef> _archivesById = new Dictionary<string, PrewarArchiveDef>(StringComparer.Ordinal);

        public void Index()
        {
            _archivesById.Clear();
            foreach (var archive in archives)
            {
                if (!string.IsNullOrEmpty(archive.id))
                    _archivesById[archive.id] = archive;
            }
        }

        public PrewarArchiveDef? GetArchive(string archiveId)
        {
            if (string.IsNullOrEmpty(archiveId)) return null;
            _archivesById.TryGetValue(archiveId, out var archive);
            return archive;
        }

        public IReadOnlyCollection<PrewarArchiveDef> GetAllArchives() => archives;
    }

    public static class PrewarArchiveCatalogLoader
    {
        public static PrewarArchiveCatalog Load(string dataDir, IFileIO fileIo)
        {
            string path = Path.Combine(dataDir, "prewar_archives.json");
            if (!fileIo.FileExists(path))
            {
                return new PrewarArchiveCatalog();
            }

            string json = fileIo.ReadAllText(path);
            var catalog = JsonSerializer.Deserialize<PrewarArchiveCatalog>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new PrewarArchiveCatalog();

            catalog.Index();
            return catalog;
        }
    }
}
