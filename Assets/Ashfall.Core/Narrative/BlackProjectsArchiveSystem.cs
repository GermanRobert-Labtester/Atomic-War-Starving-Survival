// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Narrative
{
    /// <summary>
    /// PLAN 152 — truth class for a Black Projects record. Intelligence
    /// taxonomy only; no record is executable.
    /// </summary>
    public enum BlackProjectsTruthClass
    {
        InstrumentTelemetry,   // orbital kinetic telemetry logs
        VehicleBlackbox,       // drone carrier blackbox records
        ClassifiedDirective,   // Cobalt arming directives (historical document fields)
        ComplianceAudit,       // Architect vault audit records
        Allegation             // ambiguous/unconfirmed intelligence
    }

    /// <summary>Serialized discovery bookkeeping. Record IDs only — never telemetry values (Plan 152 §16).</summary>
    [Serializable]
    public sealed class BlackProjectsArchiveState
    {
        public string systemId = BlackProjectsArchiveSystem.SystemId;
        public List<string> discoveredRecordIds = new List<string>();
    }

    /// <summary>A deterministic cross-record intelligence relation (Plan 152 Task H).</summary>
    public sealed class BlackProjectsRelatedRecord
    {
        public string RecordId { get; }
        public string RelationKind { get; }
        public string Detail { get; }

        public BlackProjectsRelatedRecord(string recordId, string relationKind, string detail)
        {
            RecordId = recordId;
            RelationKind = relationKind;
            Detail = detail;
        }
    }

    /// <summary>
    /// PLAN 152 — read-only intelligence projection over the BlackProjectsCatalog
    /// (30 records across four families: orbital kinetic telemetry, drone carrier
    /// blackboxes, Cobalt arming directives, Architect vault audits).
    ///
    /// Authority firewall (Plan 152 §4 — enforced by construction):
    ///   - The archive exposes ONLY: producer registration, discovery
    ///     bookkeeping, and derived read-only relations. It has no APIs that
    ///     touch weapons, drones, launches, doors, clearance, hazards, factions
    ///     or world state. Salvo sizes, decay rates, payload statuses and
    ///     compliance flags are historical document fields, never commands.
    ///   - Discovery is player-facing via producers (deep-lore sites, caches)
    ///     and is idempotent: the first discovery of a record is the only one
    ///     that reports it as new — journal/codex rewards cannot replay
    ///     (Plan 152 §17 negative fixtures).
    ///   - Related-record relations are derived from current catalog data
    ///     (callsign / carrier / vault / curated corroboration pairs), sorted
    ///     deterministically, and only surface between already-discovered
    ///     records — the archive never spoils undiscovered intelligence.
    ///   - Persistence: stable record IDs only, ordinal-sorted, tolerant of
    ///     unknown future IDs (preserved inertly). Old saves restore to an
    ///     empty archive without any world effect.
    /// </summary>
    public sealed class BlackProjectsArchiveSystem
    {
        public const string SystemId = "black_projects_archive";

        private readonly BlackProjectsCatalog _catalog;
        private readonly Func<string, bool>? _locationExists;
        private readonly Dictionary<string, string> _producerByRecordId =
            new Dictionary<string, string>(StringComparer.Ordinal);
        private readonly Dictionary<string, List<string>> _recordsByProducer =
            new Dictionary<string, List<string>>(StringComparer.Ordinal);
        private BlackProjectsArchiveState _state = new BlackProjectsArchiveState();

        /// <summary>Raised once per record on its first discovery (never on repeats).</summary>
        public event Action<string, BlackProjectsTruthClass>? OnRecordFirstDiscovered;
        public event Action? OnStateChanged;

        public BlackProjectsArchiveSystem(BlackProjectsCatalog catalog, Func<string, bool>? locationExists = null)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            _locationExists = locationExists;
        }

        public BlackProjectsCatalog Catalog => _catalog;
        public BlackProjectsArchiveState State => _state;

        // ── Producer registration ───────────────────────────────────────

        /// <summary>
        /// Registers the primary producer for one record. Fails closed for
        /// unknown record ids, unknown producer locations (when a location
        /// validator is injected), or records that already have a producer —
        /// every record has exactly one primary producer (Plan 152 §7).
        /// </summary>
        public bool TryRegisterProducer(string recordId, string producerLocationId)
        {
            if (string.IsNullOrWhiteSpace(recordId) || string.IsNullOrWhiteSpace(producerLocationId))
                return false;
            if (_catalog.GetOrbital(recordId) == null
                && _catalog.GetDrone(recordId) == null
                && _catalog.GetCobalt(recordId) == null
                && _catalog.GetVault(recordId) == null)
            {
                return false; // unknown record
            }
            if (_locationExists != null && !_locationExists(producerLocationId))
                return false; // invalid producer location
            if (_producerByRecordId.ContainsKey(recordId))
                return false; // one primary producer per record

            _producerByRecordId[recordId] = producerLocationId;
            if (!_recordsByProducer.TryGetValue(producerLocationId, out var list))
            {
                list = new List<string>();
                _recordsByProducer[producerLocationId] = list;
            }
            list.Add(recordId);
            return true;
        }

        /// <summary>
        /// Plan 152 §19 first-pass activation: 16 of 30 records across 9
        /// deep-lore producer sites. Every producer is an EXISTING
        /// deep_lore_locations.json site — no new locations were authored.
        /// The 14 deferred records have explicit dispositions in
        /// docs/content/BLACK_PROJECTS_INTELLIGENCE_MATRIX.md and no producer.
        /// </summary>
        public static IReadOnlyList<(string recordId, string producerLocationId)> DefaultProducerMap()
        {
            return new List<(string, string)>
            {
                // location_radar_site — telemetry intercepts and IFF logs
                ("telemetry_olympus_perigee_decay", "location_radar_site"),
                ("telemetry_olympus_ground_station_silence", "location_radar_site"),
                ("blackbox_valkyrie_radar_interrogation_loop", "location_radar_site"),
                // location_weather_station — atmospheric/power degradation logs
                ("telemetry_olympus_solar_array_spallation", "location_weather_station"),
                // location_ammunition_depot — military cache documents and maintenance dumps
                ("telemetry_olympus_inertial_gyro_drift", "location_ammunition_depot"),
                ("blackbox_valkyrie_takeoff_sortie_01", "location_ammunition_depot"),
                ("blackbox_valkyrie_target_misidentification", "location_ammunition_depot"),
                ("directive_cobalt_dual_key_authorization", "location_ammunition_depot"),
                // location_irradiated_forest — the burned flight path
                ("blackbox_valkyrie_waypoint_radiation_burn", "location_irradiated_forest"),
                // location_chemical_plant — special-munitions chemistry paperwork
                ("directive_cobalt_salted_casing_inspection", "location_chemical_plant"),
                ("directive_cobalt_atmospheric_saturation_quota", "location_chemical_plant"),
                // location_steelworks — ordnance metallurgy and structural audits
                ("directive_cobalt_sub_surface_silo_flood", "location_steelworks"),
                ("audit_architect_vault_lead_shielding_subsidence", "location_steelworks"),
                // location_agricultural_research — bio-synthesis paper trail
                ("audit_architect_vault_nutrient_broth_rancidity", "location_agricultural_research"),
                // location_metro_station — evacuation-era civic archive
                ("audit_architect_vault_biometric_decay", "location_metro_station"),
                ("audit_architect_vault_air_filtration_overhaul", "location_metro_station")
            };
        }

        // ── Queries (pure reads) ────────────────────────────────────────

        public bool IsDiscovered(string recordId)
            => !string.IsNullOrEmpty(recordId) && _state.discoveredRecordIds.Contains(recordId);

        public bool HasProducer(string recordId) => _producerByRecordId.ContainsKey(recordId);

        public string? GetProducer(string recordId)
            => _producerByRecordId.TryGetValue(recordId, out var p) ? p : null;

        /// <summary>Records with no producer — the explicit deferred list (Plan 152 §19).</summary>
        public IReadOnlyList<string> DeferredRecordIds()
        {
            var result = new List<string>();
            foreach (var id in AllRecordIds())
            {
                if (!_producerByRecordId.ContainsKey(id)) result.Add(id);
            }
            result.Sort(StringComparer.Ordinal);
            return result;
        }

        /// <summary>Truth class for a record (taxonomy only — never a permission).</summary>
        public BlackProjectsTruthClass TruthClassFor(string recordId)
        {
            if (_catalog.GetDrone(recordId) != null) return BlackProjectsTruthClass.VehicleBlackbox;
            if (_catalog.GetCobalt(recordId) != null) return BlackProjectsTruthClass.ClassifiedDirective;
            if (_catalog.GetVault(recordId) != null) return BlackProjectsTruthClass.ComplianceAudit;
            return BlackProjectsTruthClass.InstrumentTelemetry;
        }

        // ── Discovery (idempotent, fail-closed) ─────────────────────────

        /// <summary>
        /// Discovers all records assigned to a producer site. Returns the
        /// NEWLY discovered record ids in deterministic ordinal order; a
        /// repeat call returns an empty list (idempotent — journal/codex
        /// rewards fire exactly once). Unknown producer sites fail closed.
        /// Discovery mutates only the archive's own discovered-id set.
        /// </summary>
        public IReadOnlyList<string> DiscoverAtProducer(string producerLocationId)
        {
            var newlyDiscovered = new List<string>();
            if (string.IsNullOrWhiteSpace(producerLocationId)
                || !_recordsByProducer.TryGetValue(producerLocationId, out var records))
            {
                return newlyDiscovered;
            }

            foreach (var recordId in records.OrderBy(x => x, StringComparer.Ordinal))
            {
                if (DiscoverRecord(recordId)) newlyDiscovered.Add(recordId);
            }
            return newlyDiscovered;
        }

        /// <summary>
        /// Discovers a single record (quest/document-cache producers).
        /// Returns true only on first discovery.
        /// </summary>
        public bool DiscoverRecord(string recordId)
        {
            if (string.IsNullOrWhiteSpace(recordId)) return false;
            if (_state.discoveredRecordIds.Contains(recordId)) return false;

            _state.discoveredRecordIds.Add(recordId);
            _state.discoveredRecordIds.Sort(StringComparer.Ordinal);
            OnRecordFirstDiscovered?.Invoke(recordId, TruthClassFor(recordId));
            OnStateChanged?.Invoke();
            return true;
        }

        // ── Related-record graph (Task H — derived, deterministic) ──────

        /// <summary>
        /// Deterministic intelligence relations for a discovered record,
        /// surfaced ONLY between already-discovered records. Relation kinds:
        /// related_by_callsign, related_by_carrier, related_by_vault,
        /// related_by_authority, corroborates, contradicts. Pure derivation
        /// from current catalog data — relations never mutate world state.
        /// </summary>
        public IReadOnlyList<BlackProjectsRelatedRecord> GetRelated(string recordId)
        {
            var result = new List<BlackProjectsRelatedRecord>();
            if (string.IsNullOrEmpty(recordId) || !IsDiscovered(recordId)) return result;

            foreach (var relation in CuratedRelations())
            {
                string other;
                string kind;
                string detail;
                if (relation.a == recordId) { other = relation.b; kind = relation.kindAtoB; detail = relation.detail; }
                else if (relation.b == recordId) { other = relation.a; kind = relation.kindBtoA; detail = relation.detail; }
                else continue;

                if (!IsDiscovered(other)) continue; // spoiler-safe: never reveal undiscovered records
                result.Add(new BlackProjectsRelatedRecord(other, kind, detail));
            }

            // Structural relations: same callsign / carrier / vault.
            string? callsign = _catalog.GetOrbital(recordId)?.Callsign;
            string? carrier = _catalog.GetDrone(recordId)?.CarrierId;
            string? vault = _catalog.GetVault(recordId)?.VaultId;

            var structural = new List<(string id, string kind, string detail)>();
            foreach (var o in _catalog.OrbitalEntries)
            {
                if (o.Id != recordId && callsign != null && o.Callsign == callsign)
                    structural.Add((o.Id, "related_by_callsign", callsign));
            }
            foreach (var d in _catalog.DroneEntries)
            {
                if (d.Id != recordId && carrier != null && d.CarrierId == carrier)
                    structural.Add((d.Id, "related_by_carrier", carrier));
            }
            foreach (var v in _catalog.VaultEntries)
            {
                if (v.Id != recordId && vault != null && v.VaultId == vault)
                    structural.Add((v.Id, "related_by_vault", vault));
            }

            foreach (var (id, kind, detail) in structural.OrderBy(r => r.id, StringComparer.Ordinal))
            {
                if (IsDiscovered(id))
                    result.Add(new BlackProjectsRelatedRecord(id, kind, detail));
            }

            return result
                .OrderBy(r => r.RecordId, StringComparer.Ordinal)
                .ThenBy(r => r.RelationKind, StringComparer.Ordinal)
                .ToList();
        }

        /// <summary>
        /// Curated cross-family intelligence pairs (Plan 152 Task H). Both
        /// directions stated explicitly; detail strings are player-facing.
        /// </summary>
        private static IEnumerable<(string a, string b, string kindAtoB, string kindBtoA, string detail)> CuratedRelations()
        {
            return new List<(string, string, string, string, string)>
            {
                ("blackbox_valkyrie_radar_interrogation_loop", "blackbox_valkyrie_target_misidentification",
                    "corroborates", "corroborates",
                    "The IFF interrogation timeout on the same carrier explains the strike log that follows it."),
                ("telemetry_olympus_perigee_decay", "telemetry_olympus_terminal_deorbit_burn",
                    "chronology_relation", "chronology_relation",
                    "Perigee decay on the same platform precedes the terminal flight profile in the archive's chronology."),
                ("audit_architect_vault_governance_ai_clock_drift", "audit_architect_vault_last_survivor_cremation",
                    "contradicts", "contradicts",
                    "The mainframe clock-drift finding casts doubt on the census timestamps; one of these records is wrong, and the archive does not say which."),
                ("directive_cobalt_dual_key_authorization", "directive_cobalt_dead_hand_perimetr_link",
                    "related_by_authority", "related_by_authority",
                    "Both directives concern release authority; whether the automated trigger postdates or supersedes the dual-key order is not established."),
                ("telemetry_olympus_ground_station_silence", "blackbox_valkyrie_radar_interrogation_loop",
                    "corroborates", "corroborates",
                    "An unanswered uplink below and an unanswered interrogation above: the command web was gone on both sides of the sky.")
            };
        }

        // ── Persistence (Plan 152 §16) ──────────────────────────────────

        public BlackProjectsArchiveState CaptureState()
        {
            // Ordinal-sorted copy: deterministic across hosts and saves.
            var copy = new BlackProjectsArchiveState
            {
                discoveredRecordIds = new List<string>(_state.discoveredRecordIds)
            };
            copy.discoveredRecordIds.Sort(StringComparer.Ordinal);
            return copy;
        }

        public void RestoreState(BlackProjectsArchiveState? saved)
        {
            if (saved == null)
            {
                _state = new BlackProjectsArchiveState();
                OnStateChanged?.Invoke();
                return;
            }

            // Tolerant restore: unknown future record ids are preserved
            // inertly (round-trip preservation, never re-derived).
            _state = new BlackProjectsArchiveState
            {
                discoveredRecordIds = new List<string>(saved.discoveredRecordIds ?? new List<string>())
            };
            _state.discoveredRecordIds.RemoveAll(string.IsNullOrEmpty);
            _state.discoveredRecordIds.Sort(StringComparer.Ordinal);
            OnStateChanged?.Invoke();
        }

        private IReadOnlyList<string> AllRecordIds()
        {
            var ids = new List<string>();
            ids.AddRange(_catalog.OrbitalEntries.Select(e => e.Id));
            ids.AddRange(_catalog.DroneEntries.Select(e => e.Id));
            ids.AddRange(_catalog.CobaltEntries.Select(e => e.Id));
            ids.AddRange(_catalog.VaultEntries.Select(e => e.Id));
            return ids;
        }
    }
}
