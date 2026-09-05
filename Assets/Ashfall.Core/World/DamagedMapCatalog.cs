using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.IO;

namespace Ashfall.Core.World
{
    /// <summary>Raw deserialization shape for damaged_map_zones.json.</summary>
    [Serializable]
    public sealed class DamagedMapCatalogContainer
    {
        /// <summary>Schema version of the damaged treasure-map zone catalog.</summary>
        public int schema_version { get; set; } = 1;

        /// <summary>Collection of damaged-map zone definitions.</summary>
        public List<DamagedMapZoneDef> zones { get; set; } = new List<DamagedMapZoneDef>();
    }

    /// <summary>Data Transfer Object for one damaged-map treasure zone.</summary>
    [Serializable]
    public sealed class DamagedMapZoneDef
    {
        /// <summary>Stable zone identifier (save contract once persisted).</summary>
        public string zone_id { get; set; } = string.Empty;

        /// <summary>Display name shown in map/presenter surfaces.</summary>
        public string zone_name { get; set; } = string.Empty;

        /// <summary>Declared fragment count; must equal fragments.Count.</summary>
        public int total_fragments { get; set; }

        /// <summary>Hidden installation identity revealed on completion.</summary>
        public string hidden_installation_id { get; set; } = string.Empty;

        /// <summary>Hidden installation display name.</summary>
        public string hidden_installation_name { get; set; } = string.Empty;

        /// <summary>Grounded environmental-story description of the installation.</summary>
        public string installation_description { get; set; } = string.Empty;

        /// <summary>Item ids surfaced as the installation's signature salvage.</summary>
        public List<string> revealed_items { get; set; } = new List<string>();

        /// <summary>Fragment definitions that assemble into this zone's map.</summary>
        public List<DamagedMapFragmentDef> fragments { get; set; } = new List<DamagedMapFragmentDef>();
    }

    /// <summary>Data Transfer Object for one map fragment.</summary>
    [Serializable]
    public sealed class DamagedMapFragmentDef
    {
        /// <summary>Stable fragment identifier (discovery token key, not an item id).</summary>
        public string fragment_id { get; set; } = string.Empty;

        /// <summary>Short fragment label.</summary>
        public string label { get; set; } = string.Empty;

        /// <summary>Spatial-evidence description of the fragment.</summary>
        public string description { get; set; } = string.Empty;
    }

    /// <summary>
    /// A validation error found while loading the damaged-map catalog.
    /// </summary>
    public sealed class DamagedMapValidationError
    {
        /// <summary>Zone or fragment the error relates to (may be empty).</summary>
        public string Subject { get; set; } = string.Empty;

        /// <summary>Human-readable description of the structural problem.</summary>
        public string ErrorMessage { get; set; } = string.Empty;
    }

    /// <summary>Runtime record for one damaged-map treasure zone.</summary>
    public sealed class DamagedMapZone
    {
        /// <summary>Stable zone identifier.</summary>
        public string ZoneId { get; }

        /// <summary>Display name.</summary>
        public string ZoneName { get; }

        /// <summary>Declared fragment count (== Fragments.Count).</summary>
        public int TotalFragments => Fragments.Count;

        /// <summary>Hidden installation identity (may be prefixed or unprefixed; see ResolveRevealNodeId).</summary>
        public string InstallationId { get; }

        /// <summary>Hidden installation display name.</summary>
        public string InstallationName { get; }

        /// <summary>Installation environmental description.</summary>
        public string InstallationDescription { get; }

        /// <summary>Item ids surfaced as the installation's signature salvage.</summary>
        public IReadOnlyList<string> RevealedItems { get; }

        /// <summary>Fragment definitions for this zone.</summary>
        public IReadOnlyList<DamagedMapFragmentDef> Fragments { get; }

        /// <summary>Creates a runtime zone record from its definition.</summary>
        public DamagedMapZone(DamagedMapZoneDef def)
        {
            ZoneId = def.zone_id ?? string.Empty;
            ZoneName = string.IsNullOrEmpty(def.zone_name) ? ZoneId : def.zone_name;
            InstallationId = def.hidden_installation_id ?? string.Empty;
            InstallationName = string.IsNullOrEmpty(def.hidden_installation_name) ? InstallationId : def.hidden_installation_name;
            InstallationDescription = def.installation_description ?? string.Empty;
            RevealedItems = def.revealed_items != null ? new List<string>(def.revealed_items) : new List<string>();
            var fragments = new List<DamagedMapFragmentDef>();
            if (def.fragments != null) fragments.AddRange(def.fragments);
            Fragments = fragments;
        }
    }

    /// <summary>
    /// Loads the damaged treasure-map zone catalog (damaged_map_zones.json).
    /// Engine-agnostic: uses IFileIO and IJsonSerializer ports only.
    /// The catalog is static content; campaign progress lives in
    /// WastelandMapState via <see cref="DamagedMapSystem"/>.
    /// </summary>
    public static class DamagedMapCatalogLoader
    {
        /// <summary>Default catalog file name inside the data authority.</summary>
        public const string DefaultFileName = "damaged_map_zones.json";

        /// <summary>
        /// Validates structural rules across the raw container. Safe to call
        /// on partially loaded data; each problem becomes one error record.
        /// </summary>
        public static List<DamagedMapValidationError> Validate(DamagedMapCatalogContainer? container)
        {
            var errors = new List<DamagedMapValidationError>();
            if (container == null)
            {
                errors.Add(new DamagedMapValidationError { ErrorMessage = "Catalog container failed to deserialize." });
                return errors;
            }
            if (container.zones == null || container.zones.Count == 0)
            {
                errors.Add(new DamagedMapValidationError { ErrorMessage = "Catalog defines no zones." });
                return errors;
            }

            var zoneIds = new HashSet<string>(StringComparer.Ordinal);
            var fragmentIds = new HashSet<string>(StringComparer.Ordinal);
            var installationIds = new HashSet<string>(StringComparer.Ordinal);

            foreach (var zone in container.zones)
            {
                if (zone == null)
                {
                    errors.Add(new DamagedMapValidationError { ErrorMessage = "Null zone record." });
                    continue;
                }
                string subject = zone.zone_id ?? string.Empty;

                if (string.IsNullOrWhiteSpace(zone.zone_id))
                    errors.Add(new DamagedMapValidationError { Subject = subject, ErrorMessage = "Zone has an empty zone_id." });
                else if (!zoneIds.Add(zone.zone_id))
                    errors.Add(new DamagedMapValidationError { Subject = subject, ErrorMessage = $"Duplicate zone_id '{zone.zone_id}'." });

                if (string.IsNullOrWhiteSpace(zone.zone_name))
                    errors.Add(new DamagedMapValidationError { Subject = subject, ErrorMessage = "Zone has an empty zone_name." });

                if (string.IsNullOrWhiteSpace(zone.hidden_installation_id))
                    errors.Add(new DamagedMapValidationError { Subject = subject, ErrorMessage = "Zone has an empty hidden_installation_id." });
                else if (!installationIds.Add(zone.hidden_installation_id))
                    errors.Add(new DamagedMapValidationError { Subject = subject, ErrorMessage = $"Duplicate hidden_installation_id '{zone.hidden_installation_id}'." });

                if (string.IsNullOrWhiteSpace(zone.hidden_installation_name))
                    errors.Add(new DamagedMapValidationError { Subject = subject, ErrorMessage = "Zone has an empty hidden_installation_name." });

                if (string.IsNullOrWhiteSpace(zone.installation_description))
                    errors.Add(new DamagedMapValidationError { Subject = subject, ErrorMessage = "Zone has an empty installation_description." });

                if (zone.fragments == null || zone.fragments.Count == 0)
                {
                    errors.Add(new DamagedMapValidationError { Subject = subject, ErrorMessage = "Zone defines no fragments." });
                }
                else
                {
                    if (zone.total_fragments != zone.fragments.Count)
                        errors.Add(new DamagedMapValidationError
                        {
                            Subject = subject,
                            ErrorMessage = $"total_fragments ({zone.total_fragments}) != fragments.Count ({zone.fragments.Count})."
                        });

                    var perZone = new HashSet<string>(StringComparer.Ordinal);
                    foreach (var fragment in zone.fragments)
                    {
                        if (fragment == null)
                        {
                            errors.Add(new DamagedMapValidationError { Subject = subject, ErrorMessage = "Null fragment record." });
                            continue;
                        }
                        if (string.IsNullOrWhiteSpace(fragment.fragment_id))
                        {
                            errors.Add(new DamagedMapValidationError { Subject = subject, ErrorMessage = "Fragment has an empty fragment_id." });
                            continue;
                        }
                        if (!perZone.Add(fragment.fragment_id))
                            errors.Add(new DamagedMapValidationError { Subject = subject, ErrorMessage = $"Duplicate fragment_id '{fragment.fragment_id}' within zone." });
                        if (!fragmentIds.Add(fragment.fragment_id))
                            errors.Add(new DamagedMapValidationError { Subject = subject, ErrorMessage = $"Duplicate fragment_id '{fragment.fragment_id}' across zones." });
                        if (string.IsNullOrWhiteSpace(fragment.label))
                            errors.Add(new DamagedMapValidationError { Subject = subject, ErrorMessage = $"Fragment '{fragment.fragment_id}' has an empty label." });
                        if (string.IsNullOrWhiteSpace(fragment.description))
                            errors.Add(new DamagedMapValidationError { Subject = subject, ErrorMessage = $"Fragment '{fragment.fragment_id}' has an empty description." });
                    }
                }

                if (zone.revealed_items == null || zone.revealed_items.Count == 0)
                {
                    errors.Add(new DamagedMapValidationError { Subject = subject, ErrorMessage = "Zone has no revealed_items." });
                }
                else
                {
                    var seen = new HashSet<string>(StringComparer.Ordinal);
                    foreach (var item in zone.revealed_items)
                    {
                        if (string.IsNullOrWhiteSpace(item))
                            errors.Add(new DamagedMapValidationError { Subject = subject, ErrorMessage = "revealed_items contains an empty entry." });
                        else if (!seen.Add(item))
                            errors.Add(new DamagedMapValidationError { Subject = subject, ErrorMessage = $"revealed_items contains duplicate '{item}'." });
                    }
                }
            }

            return errors;
        }

        /// <summary>
        /// Loads and validates the catalog. Zones from malformed records are
        /// skipped; structural problems are returned alongside the zones.
        /// </summary>
        public static (List<DamagedMapZone> zones, List<DamagedMapValidationError> errors) LoadWithValidation(
            string dataDir,
            IFileIO? fileIO = null,
            IJsonSerializer? json = null)
        {
            var zones = new List<DamagedMapZone>();
            fileIO ??= new FileSystemIO();
            json ??= new SystemTextJsonSerializer();

            if (string.IsNullOrEmpty(dataDir))
                return (zones, new List<DamagedMapValidationError>());

            string path = fileIO.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path))
                return (zones, new List<DamagedMapValidationError>());

            string rawText = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(rawText))
                return (zones, new List<DamagedMapValidationError>());

            DamagedMapCatalogContainer? container;
            try
            {
                container = json.Deserialize<DamagedMapCatalogContainer>(rawText);
            }
            catch (Exception ex)
            {
                CatalogDiagnostics.Warn("DamagedMapCatalog", path, ex);
                return (zones, new List<DamagedMapValidationError>
                {
                    new DamagedMapValidationError { ErrorMessage = "Catalog JSON failed to deserialize." }
                });
            }

            var errors = Validate(container);
            bool hasFatal = false;
            foreach (var e in errors)
            {
                if (e.ErrorMessage.Contains("failed to deserialize") || e.ErrorMessage.Contains("no zones"))
                    hasFatal = true;
            }
            if (hasFatal || container == null || container.zones == null)
                return (zones, errors);

            var brokenZoneIds = new HashSet<string>(StringComparer.Ordinal);
            foreach (var e in errors)
            {
                if (!string.IsNullOrEmpty(e.Subject)) brokenZoneIds.Add(e.Subject);
            }

            foreach (var def in container.zones)
            {
                if (def == null || string.IsNullOrWhiteSpace(def.zone_id)) continue;
                if (brokenZoneIds.Contains(def.zone_id)) continue;
                zones.Add(new DamagedMapZone(def));
            }

            return (zones, errors);
        }

        /// <summary>
        /// Loads the catalog and builds a <see cref="DamagedMapSystem"/> bound
        /// to the live wasteland map (the authoritative reveal target).
        /// Returns null when the catalog is missing or invalid.
        /// </summary>
        public static DamagedMapSystem? CreateSystem(
            string dataDir,
            WastelandMapSystem? wastelandMap,
            IFileIO? fileIO = null,
            IJsonSerializer? json = null)
        {
            var (zones, errors) = LoadWithValidation(dataDir, fileIO, json);
            if (zones.Count == 0 || errors.Count > 0)
                return null;
            return new DamagedMapSystem(zones, wastelandMap);
        }
    }
}
