// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.IO;

namespace Ashfall.Core
{
    /// <summary>
    /// Mechanical treaty catalog for <see cref="RegionalTreatySystem"/>.
    /// Does not read narrative protocols or foundry accords.
    /// </summary>
    public static class RegionalTreatyCatalogLoader
    {
        public const string FileName = "regional_treaties.json";

        private sealed class FileDto
        {
            public int schema_version { get; set; } = 1;
            public List<TreatyDto>? treaties { get; set; }
        }

        private sealed class TreatyDto
        {
            public string treaty_id { get; set; } = string.Empty;
            public string display_name { get; set; } = string.Empty;
            public string faction_id { get; set; } = string.Empty;
            public string description { get; set; } = string.Empty;
            public float ratification_cost_scrap { get; set; }
            public int ratification_cost_day { get; set; }
            public List<string>? prerequisites { get; set; }
            public List<EffectDto>? effects { get; set; }
            public List<string>? signatory_factions { get; set; }
            public float compliance_check_interval_days { get; set; } = 30f;
            public float violation_penalty_affinity { get; set; } = -20f;
            public float term_days { get; set; }

            public TreatyDefinition ToDefinition()
            {
                var def = new TreatyDefinition
                {
                    treaty_id = treaty_id,
                    display_name = display_name,
                    faction_id = faction_id,
                    description = description,
                    ratification_cost_scrap = ratification_cost_scrap,
                    ratification_cost_day = ratification_cost_day,
                    compliance_check_interval_days = compliance_check_interval_days,
                    violation_penalty_affinity = violation_penalty_affinity,
                    term_days = term_days
                };
                if (prerequisites != null) def.prerequisites.AddRange(prerequisites);
                if (signatory_factions != null) def.signatory_factions.AddRange(signatory_factions);
                if (effects != null)
                {
                    foreach (var e in effects)
                    {
                        if (e == null) continue;
                        def.effects.Add(new TreatyEffect
                        {
                            effect_type = e.effect_type,
                            target_id = e.target_id,
                            value = e.value
                        });
                    }
                }
                return def;
            }
        }

        private sealed class EffectDto
        {
            public string effect_type { get; set; } = string.Empty;
            public string target_id { get; set; } = string.Empty;
            public float value { get; set; }
        }

        public static List<TreatyDefinition> Load(string dataDir, IFileIO files, IJsonSerializer json)
        {
            var list = new List<TreatyDefinition>();
            if (files == null || json == null || string.IsNullOrEmpty(dataDir)) return list;
            string path = Path.Combine(dataDir, FileName);
            if (!files.FileExists(path)) return list;
            string raw = files.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw)) return list;
            var dto = json.Deserialize<FileDto>(raw);
            if (dto?.treaties == null) return list;
            foreach (var t in dto.treaties)
            {
                if (t != null && !string.IsNullOrEmpty(t.treaty_id))
                    list.Add(t.ToDefinition());
            }
            return list;
        }
    }
}
