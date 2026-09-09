using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Inventory;

namespace Ashfall.Core.Narrative
{
    /// <summary>PLAN 158 — the eight technical-material record families.</summary>
    public enum TechnicalMaterialFamily
    {
        HempFiberHackling,
        WireRopeStranding,
        ManilaHawserBreakage,
        TransmissionRopeSplicing,
        NeopreneGasketDegradation,
        AramidFiberRot,
        TireRetreading,
        CelluloidFilmDecomposition
    }

    /// <summary>
    /// Read-only projection of one technical-material record (PLAN 158
    /// Workstream C). Measurements are archival specifications, never live
    /// coefficients; the canonical item link exists only when proven.
    /// </summary>
    public sealed class TechnicalMaterialRecord
    {
        public string RecordId { get; init; } = string.Empty;
        public TechnicalMaterialFamily Family { get; init; }
        public string ObjectLabel { get; init; } = string.Empty;
        public string MeasurementSummary { get; init; } = string.Empty;
        public string FailureSummary { get; init; } = string.Empty;
        public string TimestampRelative { get; init; } = string.Empty;
        public string Prose { get; init; } = string.Empty;
        public IReadOnlyList<string> Tags { get; init; } = Array.Empty<string>();
        public string? CanonicalItemId { get; init; }
        public string? ProducerId { get; init; }
        public bool IsDiscovered { get; init; }
    }

    /// <summary>Serialized discovery bookkeeping: record IDs only (PLAN 158 §17).</summary>
    [Serializable]
    public sealed class TechnicalMaterialArchiveState
    {
        public string systemId = TechnicalMaterialArchiveSystem.SystemId;
        public List<string> discoveredRecordIds = new List<string>();
    }

    /// <summary>
    /// PLAN 158 — combined read-only archive over the CordageCableCatalog and
    /// PolymerTextileCatalog (60 records, eight families).
    ///
    /// Authority firewall (Plan 158 §5 — enforced by construction):
    ///   - Breaking strengths, break loads, residual tensile percentages,
    ///     degradation severities, road-wear ratings, combustion temperatures
    ///     and transmitted-power values are ARCHIVAL SPECIFICATIONS. The
    ///     system exposes them only as display summaries. It has no API that
    ///     mutates armor, vehicles, expedition equipment, power, fire, hazards
    ///     or crafting. Only the systems that already own those objects may
    ///     decide whether any historical failure is happening now.
    ///   - Canonical item links exist only for proven identities (rope, gas
    ///     masks, film, elastomer goods) and are validated against the item
    ///     authority at registration — fail closed. Historical model/spool/
    ///     casing labels (PASGT, M17, 11R20, …) never become item IDs.
    ///   - Discovery is idempotent, deterministic, producer-driven (existing
    ///     deep-lore sites and shelter rooms), and persists record IDs only.
    ///     Old saves discover nothing automatically.
    /// </summary>
    public sealed class TechnicalMaterialArchiveSystem
    {
        public const string SystemId = "technical_material_archive";

        private readonly CordageCableCatalog _cordage;
        private readonly PolymerTextileCatalog _polymer;
        private readonly Func<string, ItemDefinition?>? _itemLookup;
        private readonly Func<string, bool>? _locationExists;

        private readonly Dictionary<string, string> _producerByRecordId =
            new Dictionary<string, string>(StringComparer.Ordinal);
        private readonly Dictionary<string, List<string>> _recordIdsByProducer =
            new Dictionary<string, List<string>>(StringComparer.Ordinal);
        private readonly Dictionary<string, string> _canonicalItemByRecordId =
            new Dictionary<string, string>(StringComparer.Ordinal);
        private TechnicalMaterialArchiveState _state = new TechnicalMaterialArchiveState();

        /// <summary>Raised exactly once per record on first discovery.</summary>
        public event Action<string, TechnicalMaterialFamily>? OnRecordFirstDiscovered;
        public event Action? OnStateChanged;

        public TechnicalMaterialArchiveSystem(
            CordageCableCatalog cordage,
            PolymerTextileCatalog polymer,
            Func<string, ItemDefinition?>? itemLookup = null,
            Func<string, bool>? locationExists = null)
        {
            _cordage = cordage ?? throw new ArgumentNullException(nameof(cordage));
            _polymer = polymer ?? throw new ArgumentNullException(nameof(polymer));
            _itemLookup = itemLookup;
            _locationExists = locationExists;
        }

        public CordageCableCatalog Cordage => _cordage;
        public PolymerTextileCatalog Polymer => _polymer;
        public TechnicalMaterialArchiveState State => _state;

        // ── Registration (fail-closed) ──────────────────────────────────

        private bool RecordExists(string recordId)
            => _cordage.GetHemp(recordId) != null || _cordage.GetWire(recordId) != null
                || _cordage.GetHawser(recordId) != null || _cordage.GetSplice(recordId) != null
                || _polymer.GetGasket(recordId) != null || _polymer.GetAramid(recordId) != null
                || _polymer.GetTire(recordId) != null || _polymer.GetFilm(recordId) != null;

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
        /// unless the item resolves in the item authority — historical labels
        /// are never promoted to item identities.
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
        /// Proven canonical item links only: hemp→rope, neoprene→masks/gloves,
        /// celluloid→film goods, elastomer→gasket/hose. Aramid armor labels
        /// and tire casing ids are historical designations — deliberately
        /// unlinked (no canonical vest/tire items exist).
        /// </summary>
        public static IReadOnlyList<(string recordId, string itemId)> DefaultCanonicalLinks()
        {
            return new List<(string, string)>
            {
                ("hemp_fiber_dew_retting_pectin_breakdown", "rope"),
                ("hemp_fiber_flax_blend_high_tensile_cord", "rope"),
                ("gasket_degrade_ozone_corona_cracking", "gas_mask"),
                ("gasket_degrade_low_temperature_glass_transition", "item_gas_mask_improved"),
                ("gasket_degrade_gamma_chain_scission_crosslinking", "protective_rubber_gloves"),
                ("gasket_degrade_plasticizer_migration_brittleness", "item_hermetic_hatch_silicone_gasket"),
                ("gasket_degrade_diesel_soot_oil_softening", "rubber_hose"),
                ("celluloid_decay_nitric_acid_sweat_vinegar_syndrome", "film_reel"),
                ("celluloid_decay_gelatin_emulsion_silver_bronzing", "photographic_film"),
                ("celluloid_decay_projection_gate_flash_ignition", "film_reel")
            };
        }

        /// <summary>
        /// First-pass producer map: 24 records (3 per family) across 9
        /// producers spanning maritime, maintenance, protective, vehicle,
        /// archive and agricultural contexts — all EXISTING deep-lore sites.
        /// The remaining 36 records stay archival depth with deferred
        /// dispositions (see docs/content/TECHNICAL_MATERIAL_PRODUCER_MAP.md).
        /// </summary>
        public static IReadOnlyList<(string recordId, string producerId)> DefaultProducerMap()
        {
            return new List<(string, string)>
            {
                // Maritime: hawsers at the flooded-district drainage network
                ("manila_hawser_abaca_fiber_saltwater_steep", "location_drainage_network"),
                ("manila_hawser_capstan_friction_thermal_glaze", "location_drainage_network"),
                ("manila_hawser_braided_eight_strand_anti_rotation", "location_drainage_network"),
                // Maritime/ice: rescue winch line
                ("manila_hawser_three_strand_hockling_twist", "location_frozen_wetland"),
                // Maintenance/industrial: wire rope at the steelworks hoists
                ("wire_rope_6x19_seale_construction_fatigue", "location_steelworks"),
                ("wire_rope_lang_lay_sheave_scrubbing_wear", "location_steelworks"),
                ("wire_rope_socketing_molten_zinc_spelter_cone", "location_steelworks"),
                // Maintenance/industrial: transmission rope drives
                ("rope_transmission_differential_groove_speed_slip", "location_steelworks"),
                ("rope_transmission_grooved_sheave_wedge_grip", "location_power_substation"),
                ("rope_transmission_tension_carriage_travel_limit", "location_power_substation"),
                // Protective equipment: gasket aging at the chemical plant
                ("gasket_degrade_ozone_corona_cracking", "location_chemical_plant"),
                ("gasket_degrade_gamma_chain_scission_crosslinking", "location_chemical_plant"),
                ("gasket_degrade_chlorinated_water_bleaching_pinhole", "location_chemical_plant"),
                // Vehicles: tire retread workshop logs at the depot
                ("tire_retread_carbon_black_reinforcement_mix", "location_ammunition_depot"),
                ("tire_retread_reclaimed_crumb_rubber_hysteresis", "location_ammunition_depot"),
                ("tire_retread_steel_belt_corrosion_blistering", "location_ammunition_depot"),
                // Protective/armor history: evidence lockers and caches
                ("aramid_rot_bullet_shatter_yarn_fraying", "location_police_station"),
                ("aramid_rot_dry_cleaning_solvent_swelling", "location_police_station"),
                ("aramid_rot_soil_bacteria_enzymatic_degradation", "location_ammunition_depot"),
                // Archives: celluloid at the broadcast and civic archives
                ("celluloid_decay_projection_gate_flash_ignition", "location_television_studio"),
                ("celluloid_decay_camphor_plasticizer_crystallization_sublimation", "location_television_studio"),
                ("celluloid_decay_spontaneous_combustion_canister_explosion", "location_television_studio"),
                ("celluloid_decay_gelatin_emulsion_silver_bronzing", "location_municipal_library"),
                ("celluloid_decay_acetic_acid_vinegar_gas_inhalation", "location_municipal_library"),
                // Agriculture: hemp fiber plots
                ("hemp_fiber_dew_retting_pectin_breakdown", "location_agricultural_research"),
                ("hemp_fiber_scutching_wooden_blade_shive_eject", "location_agricultural_research")
            };
        }

        // ── Discovery (idempotent, deterministic) ───────────────────────

        /// <summary>
        /// Item-inspection producer (Plan 158 §10): viewing a canonical item
        /// first-discovers its linked technical records. Idempotent — repeat
        /// inspections return empty. Returns newly discovered ids in ordinal
        /// order; mutates only the discovered-id ledger.
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

        /// <summary>First-discovers a single record (quest/document producers). Returns true on first discovery only.</summary>
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

        /// <summary>Projects one record (read-only; measurements are archival data).</summary>
        public TechnicalMaterialRecord? GetRecord(string recordId)
        {
            if (!RecordExists(recordId)) return null;

            var hemp = _cordage.GetHemp(recordId);
            if (hemp != null) return Project(hemp.Id, TechnicalMaterialFamily.HempFiberHackling,
                hemp.RettingFloorId,
                $"retting {hemp.RettingDurationDays:0.#} d · tenacity {hemp.FiberTensileTenacityCnTex:0.#} cN/tex",
                hemp.TimestampRelative, hemp.Prose, hemp.Tags);
            var wire = _cordage.GetWire(recordId);
            if (wire != null) return Project(wire.Id, TechnicalMaterialFamily.WireRopeStranding,
                wire.CableSpoolIdentifier,
                $"{wire.WireRopeConstruction} · Ø {wire.NominalDiameterMm:0.#} mm · breaking {wire.BreakingStrengthMetricTons:0.#} t",
                wire.TimestampRelative, wire.Prose, wire.Tags);
            var manila = _cordage.GetHawser(recordId);
            if (manila != null) return Project(manila.Id, TechnicalMaterialFamily.ManilaHawserBreakage,
                manila.HawserCoilId,
                $"{manila.FiberBotanicalOrigin} · Ø {manila.RopeDiameterInches:0.##} in · break load {manila.TensileBreakLoadKn:0.#} kN",
                manila.TimestampRelative, manila.Prose, manila.Tags);
            var trans = _cordage.GetSplice(recordId);
            if (trans != null) return Project(trans.Id, TechnicalMaterialFamily.TransmissionRopeSplicing,
                trans.DriveLineShaftId,
                $"{trans.RopeDriveSystem} · {trans.TransmittedPowerKilowatts:0.#} kW · splice {trans.SpliceLengthDiameters:0.#} diameters",
                trans.TimestampRelative, trans.Prose, trans.Tags);
            var gasket = _polymer.GetGasket(recordId);
            if (gasket != null) return Project(gasket.Id, TechnicalMaterialFamily.NeopreneGasketDegradation,
                gasket.MaskModelDesignation,
                $"{gasket.ElastomerPolymerType} · ozone {gasket.OzoneExposurePpm:0.###} ppm",
                gasket.TimestampRelative, gasket.Prose, gasket.Tags,
                failureSummary: gasket.DegradationSeverity);
            var aramid = _polymer.GetAramid(recordId);
            if (aramid != null) return Project(aramid.Id, TechnicalMaterialFamily.AramidFiberRot,
                aramid.ArmorItemId,
                $"{aramid.AramidYarnType} · residual tensile {aramid.ResidualTensileStrengthPct:0.#}%",
                aramid.TimestampRelative, aramid.Prose, aramid.Tags,
                failureSummary: aramid.FailurePhenomenon);
            var tire = _polymer.GetTire(recordId);
            if (tire != null) return Project(tire.Id, TechnicalMaterialFamily.TireRetreading,
                tire.TireCasingId,
                $"{tire.RubberCompoundFormula} · cure {tire.VulcanizationTempCelsius:0.#} °C",
                tire.TimestampRelative, tire.Prose, tire.Tags,
                failureSummary: tire.RoadWearRating);
            var film = _polymer.GetFilm(recordId);
            if (film != null) return Project(film.Id, TechnicalMaterialFamily.CelluloidFilmDecomposition,
                film.FilmArchiveReelId,
                $"{film.PolymerBaseChemistry} · ignition ≈ {film.CombustionTemperatureCelsius:0.#} °C",
                film.TimestampRelative, film.Prose, film.Tags,
                failureSummary: film.DecompositionStage);

            return null;
        }

        private TechnicalMaterialRecord Project(
            string id, TechnicalMaterialFamily family, string objectLabel,
            string measurementSummary, string timestamp, string prose, IReadOnlyList<string> tags,
            string? failureSummary = null)
        {
            return new TechnicalMaterialRecord
            {
                RecordId = id,
                Family = family,
                ObjectLabel = objectLabel ?? string.Empty,
                MeasurementSummary = measurementSummary,
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
        public TechnicalMaterialFamily FamilyOf(string recordId)
        {
            if (_cordage.GetHemp(recordId) != null) return TechnicalMaterialFamily.HempFiberHackling;
            if (_cordage.GetWire(recordId) != null) return TechnicalMaterialFamily.WireRopeStranding;
            if (_cordage.GetHawser(recordId) != null) return TechnicalMaterialFamily.ManilaHawserBreakage;
            if (_cordage.GetSplice(recordId) != null) return TechnicalMaterialFamily.TransmissionRopeSplicing;
            if (_polymer.GetGasket(recordId) != null) return TechnicalMaterialFamily.NeopreneGasketDegradation;
            if (_polymer.GetAramid(recordId) != null) return TechnicalMaterialFamily.AramidFiberRot;
            if (_polymer.GetTire(recordId) != null) return TechnicalMaterialFamily.TireRetreading;
            return TechnicalMaterialFamily.CelluloidFilmDecomposition;
        }

        // ── Query surfaces (Workstream C) — pure, deterministic reads ───

        /// <summary>Discovered records linked to a canonical item (item-inspection surface).</summary>
        public List<TechnicalMaterialRecord> RecordsForItem(string itemId)
        {
            return _canonicalItemByRecordId
                .Where(kv => kv.Value == itemId && IsDiscovered(kv.Key))
                .Select(kv => GetRecord(kv.Key)!)
                .OrderBy(r => r.RecordId, StringComparer.Ordinal)
                .ToList();
        }

        /// <summary>Discovered records of one family.</summary>
        public List<TechnicalMaterialRecord> RecordsByFamily(TechnicalMaterialFamily family)
        {
            return _state.discoveredRecordIds
                .Where(id => FamilyOf(id) == family)
                .Select(id => GetRecord(id)!)
                .ToList();
        }

        /// <summary>Discovered maritime rope records (hemp + manila families).</summary>
        public List<TechnicalMaterialRecord> MaritimeRopeRecords()
            => RecordsByFamily(TechnicalMaterialFamily.HempFiberHackling)
                .Concat(RecordsByFamily(TechnicalMaterialFamily.ManilaHawserBreakage))
                .OrderBy(r => r.RecordId, StringComparer.Ordinal)
                .ToList();

        /// <summary>Discovered protective-material records (gasket + aramid families).</summary>
        public List<TechnicalMaterialRecord> ProtectiveMaterialRecords()
            => RecordsByFamily(TechnicalMaterialFamily.NeopreneGasketDegradation)
                .Concat(RecordsByFamily(TechnicalMaterialFamily.AramidFiberRot))
                .OrderBy(r => r.RecordId, StringComparer.Ordinal)
                .ToList();

        /// <summary>Discovered fire-sensitive archive records (celluloid family).</summary>
        public List<TechnicalMaterialRecord> ArchiveFireSensitiveRecords()
            => RecordsByFamily(TechnicalMaterialFamily.CelluloidFilmDecomposition);

        /// <summary>Discovered records that document an actual failure/degradation event.</summary>
        public List<TechnicalMaterialRecord> FailureReports()
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
            foreach (TechnicalMaterialFamily family in Enum.GetValues(typeof(TechnicalMaterialFamily)))
            {
                foreach (var record in RecordsOfFamilyAll(family))
                {
                    if (!_producerByRecordId.ContainsKey(record.Id)) deferred.Add(record.Id);
                }
            }
            deferred.Sort(StringComparer.Ordinal);
            return deferred;
        }

        private IEnumerable<(string Id, string TimestampRelative, List<string> Tags)> RecordsOfFamilyAll(TechnicalMaterialFamily family)
        {
            switch (family)
            {
                case TechnicalMaterialFamily.HempFiberHackling:
                    return _cordage.HempEntries.Select(e => (e.Id, e.TimestampRelative, e.Tags));
                case TechnicalMaterialFamily.WireRopeStranding:
                    return _cordage.WireEntries.Select(e => (e.Id, e.TimestampRelative, e.Tags));
                case TechnicalMaterialFamily.ManilaHawserBreakage:
                    return _cordage.HawserEntries.Select(e => (e.Id, e.TimestampRelative, e.Tags));
                case TechnicalMaterialFamily.TransmissionRopeSplicing:
                    return _cordage.SpliceEntries.Select(e => (e.Id, e.TimestampRelative, e.Tags));
                case TechnicalMaterialFamily.NeopreneGasketDegradation:
                    return _polymer.GasketEntries.Select(e => (e.Id, e.TimestampRelative, e.Tags));
                case TechnicalMaterialFamily.AramidFiberRot:
                    return _polymer.AramidEntries.Select(e => (e.Id, e.TimestampRelative, e.Tags));
                case TechnicalMaterialFamily.TireRetreading:
                    return _polymer.TireEntries.Select(e => (e.Id, e.TimestampRelative, e.Tags));
                default:
                    return _polymer.FilmEntries.Select(e => (e.Id, e.TimestampRelative, e.Tags));
            }
        }

        // ── Persistence (§17) ───────────────────────────────────────────

        public TechnicalMaterialArchiveState CaptureState()
        {
            var copy = new TechnicalMaterialArchiveState
            {
                discoveredRecordIds = new List<string>(_state.discoveredRecordIds)
            };
            copy.discoveredRecordIds.Sort(StringComparer.Ordinal);
            return copy;
        }

        public void RestoreState(TechnicalMaterialArchiveState? saved)
        {
            if (saved == null)
            {
                _state = new TechnicalMaterialArchiveState();
                OnStateChanged?.Invoke();
                return;
            }

            _state = new TechnicalMaterialArchiveState
            {
                discoveredRecordIds = new List<string>(saved.discoveredRecordIds ?? new List<string>())
            };
            _state.discoveredRecordIds.RemoveAll(string.IsNullOrEmpty);
            _state.discoveredRecordIds.Sort(StringComparer.Ordinal);
            OnStateChanged?.Invoke();
        }
    }
}
