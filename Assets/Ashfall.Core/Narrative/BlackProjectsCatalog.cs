using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Narrative
{
    public sealed class OrbitalKineticEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("callsign")]
        public string Callsign { get; set; } = string.Empty;

        [JsonPropertyName("entry_type")]
        public string EntryType { get; set; } = string.Empty;

        [JsonPropertyName("orbital_altitude_km")]
        public float OrbitalAltitudeKm { get; set; }

        [JsonPropertyName("decay_rate_meters_per_day")]
        public float DecayRateMetersPerDay { get; set; }

        [JsonPropertyName("payload_status")]
        public string PayloadStatus { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("telemetry_channel")]
        public string TelemetryChannel { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class DroneCarrierBlackboxEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("carrier_id")]
        public string CarrierId { get; set; } = string.Empty;

        [JsonPropertyName("record_type")]
        public string RecordType { get; set; } = string.Empty;

        [JsonPropertyName("altitude_feet")]
        public int AltitudeFeet { get; set; }

        [JsonPropertyName("airspeed_knots")]
        public int AirspeedKnots { get; set; }

        [JsonPropertyName("system_health")]
        public string SystemHealth { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class CobaltDirectiveEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("directive_code")]
        public string DirectiveCode { get; set; } = string.Empty;

        [JsonPropertyName("classification")]
        public string Classification { get; set; } = string.Empty;

        [JsonPropertyName("issuing_authority")]
        public string IssuingAuthority { get; set; } = string.Empty;

        [JsonPropertyName("authorized_salvo_size")]
        public int AuthorizedSalvoSize { get; set; }

        [JsonPropertyName("effective_day_range")]
        public string EffectiveDayRange { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class ArchitectVaultAuditEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("vault_id")]
        public string VaultId { get; set; } = string.Empty;

        [JsonPropertyName("audit_type")]
        public string AuditType { get; set; } = string.Empty;

        [JsonPropertyName("sub_level")]
        public string SubLevel { get; set; } = string.Empty;

        [JsonPropertyName("auditor_designation")]
        public string AuditorDesignation { get; set; } = string.Empty;

        [JsonPropertyName("compliance_status")]
        public string ComplianceStatus { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class BlackProjectsCatalog
    {
        private readonly List<OrbitalKineticEntry> _orbitalEntries = new List<OrbitalKineticEntry>();
        private readonly List<DroneCarrierBlackboxEntry> _droneEntries = new List<DroneCarrierBlackboxEntry>();
        private readonly List<CobaltDirectiveEntry> _cobaltEntries = new List<CobaltDirectiveEntry>();
        private readonly List<ArchitectVaultAuditEntry> _vaultEntries = new List<ArchitectVaultAuditEntry>();

        private readonly Dictionary<string, object> _entriesById =
            new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyList<OrbitalKineticEntry> OrbitalEntries => _orbitalEntries;
        public IReadOnlyList<DroneCarrierBlackboxEntry> DroneEntries => _droneEntries;
        public IReadOnlyList<CobaltDirectiveEntry> CobaltEntries => _cobaltEntries;
        public IReadOnlyList<ArchitectVaultAuditEntry> VaultEntries => _vaultEntries;

        public int TotalCount => _orbitalEntries.Count + _droneEntries.Count + _cobaltEntries.Count + _vaultEntries.Count;

        public static BlackProjectsCatalog LoadFromDirectory(string directoryPath)
        {
            var catalog = new BlackProjectsCatalog();
            if (!Directory.Exists(directoryPath)) return catalog;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };

            // 1. Orbital Telemetry
            string orbitalPath = Path.Combine(directoryPath, "orbital_kinetic_telemetry.json");
            if (File.Exists(orbitalPath))
            {
                var list = CatalogLocator.LoadWrappedList<OrbitalKineticEntry>(File.ReadAllText(orbitalPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._orbitalEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 2. Drone Blackboxes
            string dronePath = Path.Combine(directoryPath, "drone_carrier_blackboxes.json");
            if (File.Exists(dronePath))
            {
                var list = CatalogLocator.LoadWrappedList<DroneCarrierBlackboxEntry>(File.ReadAllText(dronePath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._droneEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 3. Cobalt Directives
            string cobaltPath = Path.Combine(directoryPath, "cobalt_arming_directives.json");
            if (File.Exists(cobaltPath))
            {
                var list = CatalogLocator.LoadWrappedList<CobaltDirectiveEntry>(File.ReadAllText(cobaltPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._cobaltEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 4. Architect Vault Audits
            string vaultPath = Path.Combine(directoryPath, "architect_vault_audits.json");
            if (File.Exists(vaultPath))
            {
                var list = CatalogLocator.LoadWrappedList<ArchitectVaultAuditEntry>(File.ReadAllText(vaultPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._vaultEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            return catalog;
        }

        public OrbitalKineticEntry? GetOrbital(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is OrbitalKineticEntry e ? e : null;
        }

        public DroneCarrierBlackboxEntry? GetDrone(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is DroneCarrierBlackboxEntry e ? e : null;
        }

        public CobaltDirectiveEntry? GetCobalt(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is CobaltDirectiveEntry e ? e : null;
        }

        public ArchitectVaultAuditEntry? GetVault(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is ArchitectVaultAuditEntry e ? e : null;
        }
    }
}
