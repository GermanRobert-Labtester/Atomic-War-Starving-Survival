// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
#pragma warning disable CS0649
#pragma warning disable CS8618

namespace Ashfall.Core.Shelter
{
    /// <summary>
    /// Plan 210 — sanitation facility definition. Authored gameplay values
    /// only; no real-world sewage or hazardous-waste procedures.
    /// <c>processing_rate</c> is units/day the facility can process when
    /// powered and staffed; storage-type facilities instead contain waste
    /// into their own <c>capacity</c>. <c>room_tags</c> reference the
    /// canonical shelter_rooms.json vocabulary.
    /// </summary>
    [Serializable]
    public sealed class SanitationFacilityDefinition
    {
        public string id = string.Empty;
        public string facility_type = string.Empty;
        public int capacity = 0;
        public List<string> waste_types = new List<string>();
        public int processing_rate = 0;
        public int power_draw = 0;
        public int water_draw = 0;
        public int labor_required = 0;
        public List<string> room_tags = new List<string>();
        public List<string> output_tags = new List<string>();
        public int hazard_reduction = 0;
    }

    /// <summary>Load outcome: rows plus validation errors (domain result, no exceptions).</summary>
    public sealed class SanitationFacilityLoadResult
    {
        public List<SanitationFacilityDefinition> Facilities { get; } = new List<SanitationFacilityDefinition>();
        public List<string> Errors { get; } = new List<string>();
        public bool HasErrors => Errors.Count > 0;
    }

    /// <summary>In-memory facility catalog with validation. Immutable after load.</summary>
    public sealed class SanitationFacilityCatalog
    {
        private readonly Dictionary<string, SanitationFacilityDefinition> _byId =
            new Dictionary<string, SanitationFacilityDefinition>(StringComparer.Ordinal);

        public IReadOnlyDictionary<string, SanitationFacilityDefinition> ById => _byId;
        public int Count => _byId.Count;

        public SanitationFacilityDefinition? Find(string id)
        {
            return !string.IsNullOrEmpty(id) && _byId.TryGetValue(id, out var def) ? def : null;
        }

        internal void Add(SanitationFacilityDefinition def) => _byId[def.id] = def;
    }

    /// <summary>
    /// Engine-agnostic loader for sanitation_facilities.json with load-time
    /// validation: duplicate ids, unknown waste types, non-positive rates,
    /// closed room_tag vocabulary (the canonical shelter_rooms.json tags).
    /// Errors are collected, never thrown.
    /// </summary>
    public static class SanitationFacilityCatalogLoader
    {
        public const string FileName = "sanitation_facilities.json";
        public const int CurrentSchemaVersion = 1;

        /// <summary>Closed waste-type vocabulary (public taxonomy — keep it simple).</summary>
        public static readonly IReadOnlyList<string> AcceptedWasteTypes =
            new[] { "organic", "chemical", "radioactive" };

        /// <summary>
        /// Canonical room tags from shelter_rooms.json (verified current
        /// branch vocabulary). Kept in one place so an authored facility
        /// cannot silently reference a room tag no room carries.
        /// </summary>
        public static readonly IReadOnlyList<string> AcceptedRoomTags = new[]
        {
            "agriculture", "canteen", "circulation", "combat", "comfort", "communications",
            "crafting", "defense", "expedition", "flagship11", "general", "heavy_industrial",
            "high_density", "hope", "hydroponics", "infrastructure", "intel", "life_support",
            "logistics", "medical", "morale", "nutrition", "perimeter", "power", "precision",
            "quarantine", "repair", "research", "residential", "science", "secure", "social",
            "spine", "standard", "study", "surgery", "triage", "unassignable"
        };

        public static SanitationFacilityLoadResult Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            var result = new SanitationFacilityLoadResult();
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
            {
                result.Errors.Add("loader requires dataDir, IFileIO and IJsonSerializer");
                return result;
            }

            string path = fileIO.Combine(dataDir, FileName);
            if (!fileIO.FileExists(path))
            {
                result.Errors.Add("catalog file missing: " + FileName);
                return result;
            }
            string raw = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw))
            {
                result.Errors.Add("catalog file empty: " + FileName);
                return result;
            }

            SanitationFacilityRoot root;
            try
            {
                root = json.Deserialize<SanitationFacilityRoot>(raw);
            }
            catch (Exception e)
            {
                result.Errors.Add("catalog malformed JSON: " + e.Message);
                return result;
            }
            if (root == null)
            {
                result.Errors.Add("catalog parsed to null");
                return result;
            }
            if (root.schema_version > CurrentSchemaVersion)
            {
                result.Errors.Add($"catalog schema {root.schema_version} is newer than supported {CurrentSchemaVersion}");
                return result;
            }

            var rows = root.facilities;
            if (rows == null)
            {
                result.Errors.Add("catalog facilities array is null");
                return result;
            }

            var seen = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < rows.Count; i++)
            {
                var row = rows[i];
                if (row == null)
                {
                    result.Errors.Add($"entry [{i}] is null");
                    continue;
                }
                string id = row.id ?? string.Empty;
                if (string.IsNullOrWhiteSpace(id))
                {
                    result.Errors.Add($"entry [{i}] missing id");
                    continue;
                }
                if (!IsSnakeCase(id))
                {
                    result.Errors.Add($"entry [{i}] id '{id}' is not snake_case");
                    continue;
                }
                if (!seen.Add(id))
                {
                    result.Errors.Add($"duplicate id '{id}' at entry [{i}]");
                    continue;
                }
                if (string.IsNullOrWhiteSpace(row.facility_type))
                {
                    result.Errors.Add($"'{id}' missing facility_type");
                    continue;
                }
                if (row.capacity <= 0)
                {
                    result.Errors.Add($"'{id}' capacity must be > 0 (got {row.capacity})");
                    continue;
                }
                if (row.waste_types == null || row.waste_types.Count == 0)
                {
                    result.Errors.Add($"'{id}' must accept at least one waste type");
                    continue;
                }
                var typeSeen = new HashSet<string>(StringComparer.Ordinal);
                foreach (var wt in row.waste_types)
                {
                    if (string.IsNullOrWhiteSpace(wt) || !AcceptedWasteTypes.Contains(wt, StringComparer.Ordinal))
                    {
                        result.Errors.Add($"'{id}' unknown waste type '{wt}'");
                        continue;
                    }
                    if (!typeSeen.Add(wt))
                        result.Errors.Add($"'{id}' duplicate waste type '{wt}'");
                }
                if (row.processing_rate <= 0)
                {
                    result.Errors.Add($"'{id}' processing_rate must be > 0 (got {row.processing_rate})");
                    continue;
                }
                if (row.power_draw < 0 || row.water_draw < 0)
                {
                    result.Errors.Add($"'{id}' power/water draw must be >= 0");
                    continue;
                }
                if (row.labor_required < 0)
                {
                    result.Errors.Add($"'{id}' labor_required must be >= 0 (got {row.labor_required})");
                    continue;
                }
                if (row.hazard_reduction < 0 || row.hazard_reduction > 50)
                {
                    result.Errors.Add($"'{id}' hazard_reduction must be in [0,50] (got {row.hazard_reduction})");
                    continue;
                }
                if (row.room_tags == null || row.room_tags.Count == 0)
                {
                    result.Errors.Add($"'{id}' must declare at least one room tag");
                    continue;
                }
                foreach (var tag in row.room_tags)
                {
                    if (!AcceptedRoomTags.Contains(tag, StringComparer.Ordinal))
                    {
                        result.Errors.Add($"'{id}' room tag '{tag}' is not in the canonical shelter_rooms.json vocabulary");
                    }
                }

                result.Facilities.Add(new SanitationFacilityDefinition
                {
                    id = id,
                    facility_type = row.facility_type!.Trim(),
                    capacity = row.capacity ?? 0,
                    waste_types = row.waste_types.Where(t => AcceptedWasteTypes.Contains(t, StringComparer.Ordinal)).ToList(),
                    processing_rate = row.processing_rate ?? 0,
                    power_draw = row.power_draw ?? 0,
                    water_draw = row.water_draw ?? 0,
                    labor_required = row.labor_required ?? 0,
                    room_tags = row.room_tags.Where(tag => AcceptedRoomTags.Contains(tag, StringComparer.Ordinal)).ToList(),
                    output_tags = row.output_tags ?? new List<string>(),
                    hazard_reduction = row.hazard_reduction ?? 0
                });
            }
            return result;
        }

        public static SanitationFacilityCatalog ToCatalog(SanitationFacilityLoadResult load)
        {
            var catalog = new SanitationFacilityCatalog();
            if (load != null)
            {
                for (int i = 0; i < load.Facilities.Count; i++)
                    catalog.Add(load.Facilities[i]);
            }
            return catalog;
        }

        internal static bool IsSnakeCase(string id)
        {
            if (string.IsNullOrEmpty(id)) return false;
            for (int i = 0; i < id.Length; i++)
            {
                char c = id[i];
                bool ok = (c >= 'a' && c <= 'z') || (c >= '0' && c <= '9') || c == '_';
                if (!ok) return false;
            }
            return id[0] != '_' && id[id.Length - 1] != '_';
        }

        /// <summary>Schema-envelope root for sanitation_facilities.json.</summary>
        private class SanitationFacilityRoot
        {
            public int schema_version = 1;
            public List<RawSanitationFacility> facilities = new List<RawSanitationFacility>();
        }

        /// <summary>Strict DTO: null fields mean ABSENT, not defaulted.</summary>
        private class RawSanitationFacility
        {
            public string id;
            public string facility_type;
            public int? capacity;
            public List<string> waste_types;
            public int? processing_rate;
            public int? power_draw;
            public int? water_draw;
            public int? labor_required;
            public List<string> room_tags;
            public List<string> output_tags;
            public int? hazard_reduction;
        }
    }
}
