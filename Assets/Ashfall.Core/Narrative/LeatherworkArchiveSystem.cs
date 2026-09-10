using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Inventory;

namespace Ashfall.Core.Narrative
{
    /// <summary>PLAN 159 — the four tanning/leatherwork process-record families.</summary>
    public enum LeatherProcessFamily
    {
        OakBarkVegetableTan,
        MineralTanLiquor,
        RawhideBatingFailure,
        HarnessCurrying
    }

    /// <summary>
    /// Read-only projection of one tanning/leatherwork record (PLAN 159
    /// Workstream E). All measurements (Barkometer density, steep months, pH,
    /// hydrothermal shrink temperature, oil percentage, tensile PSI) are
    /// ARCHIVAL SPECIFICATIONS of a historical batch — never live
    /// coefficients, timers, durability, protection or exposure values.
    /// The canonical item/location links exist only when proven.
    /// </summary>
    public sealed class LeatherworkRecord
    {
        public string RecordId { get; init; } = string.Empty;
        public LeatherProcessFamily Family { get; init; }
        public string FacilityLabel { get; init; } = string.Empty;
        public string MaterialSource { get; init; } = string.Empty;
        public string MeasurementSummary { get; init; } = string.Empty;
        public string FailureSummary { get; init; } = string.Empty;
        public string TimestampRelative { get; init; } = string.Empty;
        public string Prose { get; init; } = string.Empty;
        public IReadOnlyList<string> Tags { get; init; } = Array.Empty<string>();
        public string? CanonicalItemId { get; init; }
        public string? ProducerId { get; init; }
        public bool IsDiscovered { get; init; }
    }

    /// <summary>Serialized discovery bookkeeping: record IDs only (PLAN 159 §19).</summary>
    [Serializable]
    public sealed class LeatherworkArchiveState
    {
        public string systemId = LeatherworkArchiveSystem.SystemId;
        public List<string> discoveredRecordIds = new List<string>();
    }

    /// <summary>
    /// PLAN 159 — read-only knowledge archive over the TanningLeatherCatalog
    /// (30 records, four families: oak-bark vegetable tanning pit logs,
    /// mineral tanning liquor assays, rawhide deliming/bating failure reports,
    /// harness currying audits).
    ///
    /// Authority firewall (Plan 159 §5 — enforced by construction):
    ///   - Barkometer densities, steep durations, pH levels, hydrothermal
    ///     shrink temperatures, oil percentages and tensile strengths are
    ///     ARCHIVAL SPECIFICATIONS exposed only as display summaries. The
    ///     system has no API that mutates inventory, crafting, recipes,
    ///     timers, equipment condition, protection, health, hazards or trade
    ///     value. pH never becomes a chemical-exposure mechanic; steep
    ///     months never become crafting timers; tensile PSI never becomes
    ///     equipment durability; fatliquor formulas never become recipes.
    ///   - Canonical item links exist only for proven identities (gas-mask
    ///     liners, hide-curing salt, harness strap leather) and are validated
    ///     against the item authority at registration — fail closed.
    ///     Facility labels (vat/pit/workshop/bench equipment designations)
    ///     are never promoted to location identities.
    ///   - Discovery is idempotent, deterministic, producer-driven (existing
    ///     deep-lore sites and canonical item inspection), and persists
    ///     record IDs only. Old saves discover nothing automatically.
    /// </summary>
    public sealed class LeatherworkArchiveSystem
    {
        public const string SystemId = "leatherwork_archive";

        private readonly TanningLeatherCatalog _catalog;
        private readonly Func<string, ItemDefinition?>? _itemLookup;
        private readonly Func<string, bool>? _locationExists;

        private readonly Dictionary<string, string> _producerByRecordId =
            new Dictionary<string, string>(StringComparer.Ordinal);
        private readonly Dictionary<string, List<string>> _recordIdsByProducer =
            new Dictionary<string, List<string>>(StringComparer.Ordinal);
        private readonly Dictionary<string, string> _canonicalItemByRecordId =
            new Dictionary<string, string>(StringComparer.Ordinal);
        private LeatherworkArchiveState _state = new LeatherworkArchiveState();

        /// <summary>Raised exactly once per record on first discovery.</summary>
        public event Action<string, LeatherProcessFamily>? OnRecordFirstDiscovered;
        public event Action? OnStateChanged;

        public LeatherworkArchiveSystem(
            TanningLeatherCatalog catalog,
            Func<string, ItemDefinition?>? itemLookup = null,
            Func<string, bool>? locationExists = null)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            _itemLookup = itemLookup;
            _locationExists = locationExists;
        }

        public TanningLeatherCatalog Catalog => _catalog;
        public LeatherworkArchiveState State => _state;

        // ── Registration (fail-closed) ──────────────────────────────────

        private bool RecordExists(string recordId)
            => _catalog.GetBark(recordId) != null || _catalog.GetMineral(recordId) != null
                || _catalog.GetBating(recordId) != null || _catalog.GetCurrying(recordId) != null;

        /// <summary>Registers the primary producer for one record. Fails closed for unknown records/locations or duplicates.</summary>
        public bool TryRegisterProducer(string recordId, string producerId)
        {
            if (string.IsNullOrWhiteSpace(recordId) || string.IsNullOrWhiteSpace(producerId))
                return false;
            if (!RecordExists(recordId)) return false;
            if (_producerByRecordId.ContainsKey(recordId)) return false;
            if (producerId.StartsWith("location_", StringComparison.Ordinal)
                && _locationExists != null && !_locationExists(producerId))
                return false;

            _producerByRecordId[recordId] = producerId;
            if (!_recordIdsByProducer.TryGetValue(producerId, out var list))
            {
                list = new List<string>();
                _recordIdsByProducer[producerId] = list;
            }
            list.Add(recordId);
            return true;
        }

        /// <summary>
        /// Links a record to a CANONICAL item id (Workstream B). Fails closed
        /// unless the item resolves in the item authority — historical
        /// material labels are never promoted to item identities.
        /// </summary>
        public bool TryRegisterCanonicalLink(string recordId, string itemId)
        {
            if (string.IsNullOrWhiteSpace(recordId) || string.IsNullOrWhiteSpace(itemId))
                return false;
            if (!RecordExists(recordId)) return false;
            if (_canonicalItemByRecordId.ContainsKey(recordId)) return false;
            var def = _itemLookup?.Invoke(itemId);
            if (def == null || !string.Equals(def.id, itemId, StringComparison.Ordinal))
                return false; // unresolved identity — stays non-canonical

            _canonicalItemByRecordId[recordId] = itemId;
            return true;
        }

        /// <summary>The canonical item linked to a record, or null (label remains provenance-only).</summary>
        public string? GetCanonicalItem(string recordId)
            => _canonicalItemByRecordId.TryGetValue(recordId, out var id) ? id : null;

        // ── Curated defaults (Workstreams B/D–H) ────────────────────────

        /// <summary>
        /// Proven canonical item links only: alum-tawed kidskin became gas-mask
        /// facepiece liners (record prose); coarse rock salt is the item
        /// authority's own hide-curing material; harness/strap leather matches
        /// the leather_strap bridle/satchel identity. No rawhide, tannin bark,
        /// fatliquor or chamois items exist — those references stay
        /// provenance-only (no items invented).
        /// </summary>
        public static IReadOnlyList<(string recordId, string itemId)> DefaultCanonicalLinks()
        {
            return new List<(string, string)>
            {
                ("mineral_tan_potassium_alum_white_tawing", "gas_mask"),
                ("rawhide_bate_salt_stain_calcium_phosphate_speck", "item_preservation_salt"),
                ("leather_harness_neatsfoot_oil_cold_stuffing", "leather_strap"),
                ("leather_harness_sulfur_gas_red_rot_powdering", "leather_strap")
            };
        }

        /// <summary>
        /// First-pass producer map: 29 records across 10 EXISTING deep-lore
        /// sites, covering all four families. Only one record is deferred
        /// (chamois aviation fuel-filter tannage — no matching site exists).
        /// Facility labels in the catalog (vat/pit/workshop designations) are
        /// equipment context, never producer IDs; producers are the real
        /// locations where such records would plausibly be recovered.
        /// </summary>
        public static IReadOnlyList<(string recordId, string producerId)> DefaultProducerMap()
        {
            return new List<(string, string)>
            {
                // Oak-bark vegetable tanning: woodland bark sources at the
                // logging camp; botanical experiments at agricultural research.
                ("oak_bark_tan_chestnut_liquor_density", "location_upland_logging_camp"),
                ("oak_bark_tan_pit_sour_fermentation_bloom", "location_upland_logging_camp"),
                ("oak_bark_tan_tannin_case_hardening_strike", "location_upland_logging_camp"),
                ("oak_bark_tan_iron_stain_black_rust_spot", "location_upland_logging_camp"),
                ("oak_bark_tan_sumac_leaf_light_saddlery_cure", "location_agricultural_research"),
                ("oak_bark_tan_willow_bark_flexible_boot_upper", "location_agricultural_research"),
                // Spent tanbark briquettes were burned in a foundry
                // reverberatory furnace; effluent hit the drainage canal.
                ("oak_bark_tan_spent_tanbark_fuel_briquetting", "location_steelworks"),
                ("oak_bark_tan_tannery_effluent_oxygen_depletion", "location_drainage_network"),
                // Mineral tanning chemistry at the chemical plant; mineral
                // armor at the ammunition depot.
                ("mineral_tan_potassium_alum_white_tawing", "location_chemical_plant"),
                ("mineral_tan_basic_chromium_sulfate_shrink_temp", "location_chemical_plant"),
                ("mineral_tan_basification_sodium_bicarbonate_drawn_grain", "location_chemical_plant"),
                ("mineral_tan_iron_vitriol_tanning_brittleness", "location_chemical_plant"),
                ("mineral_tan_hexavalent_chromium_toxic_rash", "location_chemical_plant"),
                ("mineral_tan_synthetic_syntan_naphthalene_substitute", "location_chemical_plant"),
                ("mineral_tan_zirconium_sulfate_snow_white_armor", "location_ammunition_depot"),
                // Beamhouse/bating failures at the hide source — the abattoir.
                ("rawhide_bate_ammonium_sulfate_deliming_stall", "location_automated_abattoir"),
                ("rawhide_bate_pancreatic_trypsin_over_digestion", "location_automated_abattoir"),
                ("rawhide_bate_sodium_sulfide_unhairing_burn", "location_automated_abattoir"),
                ("rawhide_bate_acid_swelling_pickling_rupture", "location_automated_abattoir"),
                ("rawhide_bate_fleshing_beam_knife_gouge", "location_automated_abattoir"),
                ("rawhide_bate_salt_stain_calcium_phosphate_speck", "location_automated_abattoir"),
                ("rawhide_bate_bacterial_putrefaction_hair_slip", "location_automated_abattoir"),
                // Currying/harness workshops: heavy gear at the depot, steam
                // and drive belts at the steelworks, leather equipment repair
                // at the police station, stitch work in the metro, dubbin for
                // wetland mud.
                ("leather_harness_neatsfoot_oil_cold_stuffing", "location_ammunition_depot"),
                ("leather_harness_spew_stearic_acid_white_bloom", "location_ammunition_depot"),
                ("leather_harness_sulfur_gas_red_rot_powdering", "location_steelworks"),
                ("leather_harness_brass_rivet_verdigris_corrosion", "location_police_station"),
                ("leather_harness_wax_thread_linen_rot_failure", "location_metro_station"),
                ("leather_harness_currying_dubbin_waterproofing", "location_frozen_wetland"),
                ("leather_harness_rawhide_lace_tensile_braiding", "location_steelworks")
            };
        }

        // ── Discovery (idempotent, deterministic) ───────────────────────

        /// <summary>
        /// Item-inspection producer (Plan 159 Workstream G): viewing a
        /// canonical item first-discovers its linked leatherwork records.
        /// Idempotent — repeat inspections return empty. Returns newly
        /// discovered ids in ordinal order; mutates only the discovered-id
        /// ledger. Never touches item quantity, condition or trade value.
        /// </summary>
        public IReadOnlyList<string> DiscoverForItem(string itemId)
        {
            var newlyDiscovered = new List<string>();
            if (string.IsNullOrWhiteSpace(itemId)) return newlyDiscovered;

            foreach (var kv in _canonicalItemByRecordId
                .Where(kv => kv.Value == itemId)
                .OrderBy(kv => kv.Key, StringComparer.Ordinal))
            {
                if (DiscoverRecord(kv.Key)) newlyDiscovered.Add(kv.Key);
            }
            return newlyDiscovered;
        }

        /// <summary>First-discovers all records assigned to a producer; returns newly discovered ids (ordinal order). Repeat calls return empty.</summary>
        public IReadOnlyList<string> DiscoverAtProducer(string producerId)
        {
            var newlyDiscovered = new List<string>();
            if (string.IsNullOrWhiteSpace(producerId)
                || !_recordIdsByProducer.TryGetValue(producerId, out var records))
            {
                return newlyDiscovered;
            }

            foreach (var recordId in records.OrderBy(x => x, StringComparer.Ordinal))
            {
                if (DiscoverRecord(recordId)) newlyDiscovered.Add(recordId);
            }
            return newlyDiscovered;
        }

        /// <summary>First-discovers a single record (document/manual producers). Returns true on first discovery only.</summary>
        public bool DiscoverRecord(string recordId)
        {
            if (string.IsNullOrWhiteSpace(recordId) || !RecordExists(recordId)) return false;
            if (_state.discoveredRecordIds.Contains(recordId)) return false;

            _state.discoveredRecordIds.Add(recordId);
            _state.discoveredRecordIds.Sort(StringComparer.Ordinal);
            OnRecordFirstDiscovered?.Invoke(recordId, FamilyOf(recordId));
            OnStateChanged?.Invoke();
            return true;
        }

        public bool IsDiscovered(string recordId)
            => !string.IsNullOrEmpty(recordId) && _state.discoveredRecordIds.Contains(recordId);

        public bool HasProducer(string recordId) => _producerByRecordId.ContainsKey(recordId);

        /// <summary>The registered primary producer of a record, or null (deferred).</summary>
        public string? GetProducer(string recordId)
            => _producerByRecordId.TryGetValue(recordId, out var p) ? p : null;

        /// <summary>Projects one record (read-only; measurements are archival batch data).</summary>
        public LeatherworkRecord? GetRecord(string recordId)
        {
            if (!RecordExists(recordId)) return null;

            var bark = _catalog.GetBark(recordId);
            if (bark != null) return Project(bark.Id, LeatherProcessFamily.OakBarkVegetableTan,
                bark.TanneryVatId,
                bark.BarkSourceBotanical,
                $"Barkometer {bark.BarkometerDensityDegrees:0.#}° · steep {bark.HideSteepDurationMonths:0.#} mo",
                bark.TimestampRelative, bark.Prose, bark.Tags);

            var mineral = _catalog.GetMineral(recordId);
            if (mineral != null) return Project(mineral.Id, LeatherProcessFamily.MineralTanLiquor,
                mineral.MineralTanLiquorId,
                mineral.MineralTanningAgent,
                $"pH {mineral.LiquorPhLevel:0.#} · shrink temp {mineral.HydrothermalShrinkTempCelsius:0.#} °C",
                mineral.TimestampRelative, mineral.Prose, mineral.Tags);

            var bating = _catalog.GetBating(recordId);
            if (bating != null) return Project(bating.Id, LeatherProcessFamily.RawhideBatingFailure,
                bating.BeamhousePitId,
                bating.DelimingChemicalAgent,
                failureSummary: bating.PhenolphthaleinTestStatus,
                timestamp: bating.TimestampRelative, prose: bating.Prose, tags: bating.Tags);

            var currying = _catalog.GetCurrying(recordId);
            if (currying != null) return Project(currying.Id, LeatherProcessFamily.HarnessCurrying,
                currying.CurryingWorkshopId,
                currying.FatliquorCompoundFormula,
                $"oil {currying.OilContentPercentage:0.#}% · tensile {currying.TensileStrengthPsi:0} PSI",
                currying.TimestampRelative, currying.Prose, currying.Tags);

            return null;
        }

        private LeatherworkRecord Project(
            string id, LeatherProcessFamily family, string facilityLabel,
            string materialSource, string? measurementSummary = null,
            string? timestamp = null, string? prose = null, IReadOnlyList<string>? tags = null,
            string? failureSummary = null)
        {
            return new LeatherworkRecord
            {
                RecordId = id,
                Family = family,
                FacilityLabel = facilityLabel ?? string.Empty,
                MaterialSource = materialSource ?? string.Empty,
                MeasurementSummary = measurementSummary ?? string.Empty,
                FailureSummary = failureSummary ?? string.Empty,
                TimestampRelative = timestamp ?? string.Empty,
                Prose = prose ?? string.Empty,
                Tags = tags ?? Array.Empty<string>(),
                CanonicalItemId = GetCanonicalItem(id),
                ProducerId = GetProducer(id),
                IsDiscovered = IsDiscovered(id)
            };
        }

        /// <summary>Family of a record.</summary>
        public LeatherProcessFamily FamilyOf(string recordId)
        {
            if (_catalog.GetBark(recordId) != null) return LeatherProcessFamily.OakBarkVegetableTan;
            if (_catalog.GetMineral(recordId) != null) return LeatherProcessFamily.MineralTanLiquor;
            if (_catalog.GetBating(recordId) != null) return LeatherProcessFamily.RawhideBatingFailure;
            return LeatherProcessFamily.HarnessCurrying;
        }

        // ── Query surfaces (Workstream E) — pure, deterministic reads ───

        /// <summary>Discovered records linked to a canonical item (item-inspection surface).</summary>
        public List<LeatherworkRecord> RecordsForItem(string itemId)
        {
            return _canonicalItemByRecordId
                .Where(kv => kv.Value == itemId && IsDiscovered(kv.Key))
                .Select(kv => GetRecord(kv.Key)!)
                .OrderBy(r => r.RecordId, StringComparer.Ordinal)
                .ToList();
        }

        /// <summary>Discovered records of one family.</summary>
        public List<LeatherworkRecord> RecordsByFamily(LeatherProcessFamily family)
        {
            return _state.discoveredRecordIds
                .Where(id => FamilyOf(id) == family)
                .Select(id => GetRecord(id)!)
                .ToList();
        }

        /// <summary>Discovered records that document a process/failure event.</summary>
        public List<LeatherworkRecord> FailureReports()
        {
            return _state.discoveredRecordIds
                .Select(id => GetRecord(id)!)
                .Where(r => !string.IsNullOrEmpty(r.FailureSummary))
                .OrderBy(r => r.RecordId, StringComparer.Ordinal)
                .ToList();
        }

        /// <summary>Records with no producer — the explicit deferred list.</summary>
        public IReadOnlyList<string> DeferredRecordIds()
        {
            var deferred = new List<string>();
            foreach (LeatherProcessFamily family in Enum.GetValues(typeof(LeatherProcessFamily)))
            {
                var all = family switch
                {
                    LeatherProcessFamily.OakBarkVegetableTan => _catalog.BarkEntries.Select(e => e.Id),
                    LeatherProcessFamily.MineralTanLiquor => _catalog.MineralEntries.Select(e => e.Id),
                    LeatherProcessFamily.RawhideBatingFailure => _catalog.BatingEntries.Select(e => e.Id),
                    _ => _catalog.CurryingEntries.Select(e => e.Id)
                };
                foreach (var recordId in all)
                {
                    if (!_producerByRecordId.ContainsKey(recordId)) deferred.Add(recordId);
                }
            }
            deferred.Sort(StringComparer.Ordinal);
            return deferred;
        }

        // ── Persistence (§19) ───────────────────────────────────────────

        public LeatherworkArchiveState CaptureState()
        {
            var copy = new LeatherworkArchiveState
            {
                discoveredRecordIds = new List<string>(_state.discoveredRecordIds)
            };
            copy.discoveredRecordIds.Sort(StringComparer.Ordinal);
            return copy;
        }

        public void RestoreState(LeatherworkArchiveState? saved)
        {
            if (saved == null)
            {
                _state = new LeatherworkArchiveState();
                OnStateChanged?.Invoke();
                return;
            }

            _state = new LeatherworkArchiveState
            {
                discoveredRecordIds = new List<string>(saved.discoveredRecordIds ?? new List<string>())
            };
            _state.discoveredRecordIds.RemoveAll(string.IsNullOrEmpty);
            _state.discoveredRecordIds.Sort(StringComparer.Ordinal);
            OnStateChanged?.Invoke();
        }
    }
}
