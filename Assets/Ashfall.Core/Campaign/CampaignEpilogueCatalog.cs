// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Ashfall.Core.Campaign
{
    public sealed class EpilogueVignetteDef
    {
        public string id { get; set; } = string.Empty;
        public string category { get; set; } = string.Empty;
        public int priority { get; set; } = 1;
        public int min_survivors { get; set; } = 0;
        public int max_survivors { get; set; } = 999;
        public int min_paroled_captives { get; set; } = 0;
        public int max_penal_shifts { get; set; } = 999;
        public int min_archives_decrypted { get; set; } = 0;
        public int min_starvation_deaths { get; set; } = 0;
        public int max_starvation_deaths { get; set; } = 999;
        public string title { get; set; } = string.Empty;
        public string narrative { get; set; } = string.Empty;
        public List<string> tags { get; set; } = new List<string>();
    }

    public sealed class CampaignEpilogueCatalog
    {
        public int schema_version { get; set; } = 1;
        public List<EpilogueVignetteDef> vignettes { get; set; } = new List<EpilogueVignetteDef>();

        private readonly Dictionary<string, EpilogueVignetteDef> _vignettesById = new Dictionary<string, EpilogueVignetteDef>(StringComparer.Ordinal);

        public void Index()
        {
            _vignettesById.Clear();
            foreach (var v in vignettes)
            {
                if (!string.IsNullOrEmpty(v.id))
                    _vignettesById[v.id] = v;
            }
        }

        public EpilogueVignetteDef? GetVignette(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            _vignettesById.TryGetValue(id, out var v);
            return v;
        }

        public IReadOnlyCollection<EpilogueVignetteDef> GetAllVignettes() => vignettes;
    }

    public static class CampaignEpilogueCatalogLoader
    {
        public static CampaignEpilogueCatalog Load(string dataDir, IFileIO fileIo)
        {
            string path = Path.Combine(dataDir, "campaign_epilogues.json");
            if (!fileIo.FileExists(path))
            {
                return new CampaignEpilogueCatalog();
            }

            string json = fileIo.ReadAllText(path);
            var catalog = JsonSerializer.Deserialize<CampaignEpilogueCatalog>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new CampaignEpilogueCatalog();

            catalog.Index();
            return catalog;
        }
    }
}
