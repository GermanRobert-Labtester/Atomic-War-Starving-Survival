// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Radio
{
    /// <summary>
    /// Result of resolving a distress signal location against canonical expedition destinations.
    /// Invariant: Pure C#, zero engine dependencies.
    /// </summary>
    [Serializable]
    public sealed class DistressDestinationResolution
    {
        public bool IsValid { get; set; }
        public string RawLocationId { get; set; } = string.Empty;
        public string DestinationId { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public int DistanceTicks { get; set; }
        public int DangerLevel { get; set; }
        public string ScavengingTableId { get; set; } = string.Empty;
        public List<string> LootCategories { get; set; } = new List<string>();
        public string ResolutionMode { get; set; } = string.Empty; // Direct, Aliased, SignalThematic, Fallback
        public string? ErrorCode { get; set; }
    }

    /// <summary>
    /// Canonical expedition destination entry for resolution lookup.
    /// </summary>
    public sealed class CanonicalDestinationInfo
    {
        public string Id { get; }
        public string DisplayName { get; }
        public int DistanceTicks { get; }
        public int DangerLevel { get; }
        public string ScavengingTableId { get; }
        public IReadOnlyList<string> LootCategories { get; }

        public CanonicalDestinationInfo(
            string id,
            string displayName,
            int distanceTicks,
            int dangerLevel,
            string scavengingTableId,
            IReadOnlyList<string>? lootCategories = null)
        {
            Id = id ?? string.Empty;
            DisplayName = displayName ?? string.Empty;
            DistanceTicks = distanceTicks;
            DangerLevel = dangerLevel;
            ScavengingTableId = scavengingTableId ?? string.Empty;
            LootCategories = lootCategories ?? Array.Empty<string>();
        }
    }

    /// <summary>
    /// Authoritative resolver mapping distress signal location references to the 55 canonical
    /// expedition destinations in expeditions.json. Provides a versioned alias dictionary
    /// for legacy, world-map, and built-in identifiers.
    /// </summary>
    public sealed class DistressDestinationResolver
    {
        public const int SchemaVersion = 1;

        private readonly Dictionary<string, CanonicalDestinationInfo> _canonical =
            new Dictionary<string, CanonicalDestinationInfo>(StringComparer.OrdinalIgnoreCase);

        private readonly Dictionary<string, string> _aliasMap =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        private readonly Dictionary<string, string> _scavengingTableMap =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        private readonly Dictionary<string, string> _signalThematicMap =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        private static readonly Lazy<DistressDestinationResolver> s_defaultInstance =
            new Lazy<DistressDestinationResolver>(() => new DistressDestinationResolver());

        public static DistressDestinationResolver Default => s_defaultInstance.Value;

        public DistressDestinationResolver()
        {
            InitializeCanonicalDestinations();
            InitializeVersionedAliases();
            InitializeScavengingTableMappings();
            InitializeSignalThematicMappings();
        }

        public int TotalCanonicalDestinations => _canonical.Count;
        public int TotalAliases => _aliasMap.Count;

        public bool IsCanonicalDestination(string destinationId)
        {
            if (string.IsNullOrWhiteSpace(destinationId)) return false;
            return _canonical.ContainsKey(destinationId.Trim());
        }

        public CanonicalDestinationInfo? GetCanonicalInfo(string destinationId)
        {
            if (string.IsNullOrWhiteSpace(destinationId)) return null;
            return _canonical.TryGetValue(destinationId.Trim(), out var info) ? info : null;
        }

        public IReadOnlyCollection<CanonicalDestinationInfo> GetAllCanonical() => _canonical.Values;

        /// <summary>
        /// Resolves an arbitrary location ID (canonical, aliased, table_loot, or fallback)
        /// into a valid expedition destination from expeditions.json.
        /// </summary>
        public DistressDestinationResolution Resolve(string? rawLocationId)
        {
            if (string.IsNullOrWhiteSpace(rawLocationId))
            {
                // Fallback destination: collapsed_building
                var fallback = _canonical["collapsed_building"];
                return new DistressDestinationResolution
                {
                    IsValid = false,
                    RawLocationId = string.Empty,
                    DestinationId = fallback.Id,
                    DisplayName = fallback.DisplayName,
                    DistanceTicks = fallback.DistanceTicks,
                    DangerLevel = fallback.DangerLevel,
                    ScavengingTableId = fallback.ScavengingTableId,
                    LootCategories = new List<string>(fallback.LootCategories),
                    ResolutionMode = "Fallback",
                    ErrorCode = "EMPTY_LOCATION_ID"
                };
            }

            string trimmed = rawLocationId.Trim();

            // 1. Direct canonical match
            if (_canonical.TryGetValue(trimmed, out var direct))
            {
                return new DistressDestinationResolution
                {
                    IsValid = true,
                    RawLocationId = trimmed,
                    DestinationId = direct.Id,
                    DisplayName = direct.DisplayName,
                    DistanceTicks = direct.DistanceTicks,
                    DangerLevel = direct.DangerLevel,
                    ScavengingTableId = direct.ScavengingTableId,
                    LootCategories = new List<string>(direct.LootCategories),
                    ResolutionMode = "Direct",
                    ErrorCode = null
                };
            }

            // 2. Versioned alias map
            if (_aliasMap.TryGetValue(trimmed, out var targetId) && _canonical.TryGetValue(targetId, out var aliased))
            {
                return new DistressDestinationResolution
                {
                    IsValid = true,
                    RawLocationId = trimmed,
                    DestinationId = aliased.Id,
                    DisplayName = aliased.DisplayName,
                    DistanceTicks = aliased.DistanceTicks,
                    DangerLevel = aliased.DangerLevel,
                    ScavengingTableId = aliased.ScavengingTableId,
                    LootCategories = new List<string>(aliased.LootCategories),
                    ResolutionMode = "Aliased",
                    ErrorCode = null
                };
            }

            // 3. Scavenging table ID mapping (table_loot_*)
            if (trimmed.StartsWith("table_loot_", StringComparison.OrdinalIgnoreCase) &&
                _scavengingTableMap.TryGetValue(trimmed, out var tableTargetId) &&
                _canonical.TryGetValue(tableTargetId, out var fromTable))
            {
                return new DistressDestinationResolution
                {
                    IsValid = true,
                    RawLocationId = trimmed,
                    DestinationId = fromTable.Id,
                    DisplayName = fromTable.DisplayName,
                    DistanceTicks = fromTable.DistanceTicks,
                    DangerLevel = fromTable.DangerLevel,
                    ScavengingTableId = fromTable.ScavengingTableId,
                    LootCategories = new List<string>(fromTable.LootCategories),
                    ResolutionMode = "AliasedTable",
                    ErrorCode = null
                };
            }

            // 4. Unknown location fallback
            var defaultFallback = _canonical["collapsed_building"];
            return new DistressDestinationResolution
            {
                IsValid = false,
                RawLocationId = trimmed,
                DestinationId = defaultFallback.Id,
                DisplayName = defaultFallback.DisplayName,
                DistanceTicks = defaultFallback.DistanceTicks,
                DangerLevel = defaultFallback.DangerLevel,
                ScavengingTableId = defaultFallback.ScavengingTableId,
                LootCategories = new List<string>(defaultFallback.LootCategories),
                ResolutionMode = "Fallback",
                ErrorCode = $"UNMAPPED_LOCATION_{trimmed}"
            };
        }

        /// <summary>
        /// Resolves a distress signal definition to its canonical expedition destination.
        /// </summary>
        public DistressDestinationResolution ResolveSignal(DistressSignalDefinition? signal)
        {
            if (signal == null)
            {
                return Resolve(null);
            }

            // 1. Try revealed_location
            if (!string.IsNullOrWhiteSpace(signal.RevealedLocation))
            {
                var res = Resolve(signal.RevealedLocation);
                if (res.IsValid) return res;
            }

            // 2. Try location_reference
            if (!string.IsNullOrWhiteSpace(signal.LocationReference))
            {
                var res = Resolve(signal.LocationReference);
                if (res.IsValid) return res;
            }

            // 3. Try signal thematic map by FrequencyId
            if (!string.IsNullOrWhiteSpace(signal.FrequencyId) &&
                _signalThematicMap.TryGetValue(signal.FrequencyId, out var thematicDestId) &&
                _canonical.TryGetValue(thematicDestId, out var thematic))
            {
                return new DistressDestinationResolution
                {
                    IsValid = true,
                    RawLocationId = signal.FrequencyId,
                    DestinationId = thematic.Id,
                    DisplayName = thematic.DisplayName,
                    DistanceTicks = thematic.DistanceTicks,
                    DangerLevel = thematic.DangerLevel,
                    ScavengingTableId = thematic.ScavengingTableId,
                    LootCategories = new List<string>(thematic.LootCategories),
                    ResolutionMode = "SignalThematic",
                    ErrorCode = null
                };
            }

            // 4. Default resolution fallback
            var defaultFallback = _canonical["collapsed_building"];
            return new DistressDestinationResolution
            {
                IsValid = true,
                RawLocationId = signal.FrequencyId ?? string.Empty,
                DestinationId = defaultFallback.Id,
                DisplayName = defaultFallback.DisplayName,
                DistanceTicks = defaultFallback.DistanceTicks,
                DangerLevel = defaultFallback.DangerLevel,
                ScavengingTableId = defaultFallback.ScavengingTableId,
                LootCategories = new List<string>(defaultFallback.LootCategories),
                ResolutionMode = "DefaultThematic",
                ErrorCode = null
            };
        }

        private void InitializeCanonicalDestinations()
        {
            void Add(string id, string name, int dist, int danger, string table, params string[] categories)
            {
                _canonical[id] = new CanonicalDestinationInfo(id, name, dist, danger, table, categories);
            }

            Add("loc_the_allotments", "The Works Allotment Commune", 5, 2, "table_loot_farm", "scrap_metal", "clean_water", "bandage", "dried_rations");
            Add("loc_denial_cut_substation", "The Denial Cut Substation", 8, 4, "table_loot_power_substation", "dosimeter", "copper_wire_10m_of_10m", "fuel", "item_hydro_baron_queue_chit");
            Add("suburban_house", "Suburban House", 2, 2, "table_loot_apartment_block", "canned_food", "cloth", "battery", "book");
            Add("rural_gas_station", "Rural Gas Station", 3, 3, "table_loot_industrial_district", "fuel", "scrap_metal", "mechanical_parts", "tire");
            Add("concert_hall_ruins", "Concert Hall Ruins", 3, 2, "table_loot_concert_hall", "scrap_wood", "cloth", "book", "instrument_string");
            Add("family_bunker_backyard_shed", "Family Bunker: Backyard Shed", 2, 2, "table_loot_apartment_block", "canned_food", "clean_water", "battery", "flashlight");
            Add("old_library_cache", "Old Library Cache", 5, 3, "table_loot_school", "book", "paper", "pen", "canned_food");
            Add("ruined_garage", "Ruined Garage", 4, 3, "table_loot_warehouse", "scrap_metal", "mechanical_parts", "fuel", "tire");
            Add("collapsed_building", "Collapsed Building", 4, 3, "table_loot_collapsed_structure", "scrap_metal", "concrete_rubble", "pipe", "wire");
            Add("loc_grange_hall", "The Grange Hall", 2, 3, "table_loot_farm", "canned_food", "clean_water", "dried_rations", "scrap_wood");
            Add("loc_apiary_rows", "The Apiary Rows", 3, 3, "table_loot_apiary_rows", "honey_comb", "beeswax", "scrap_wood", "clean_water");
            Add("loc_school_gymnasium", "School Gymnasium", 3, 3, "table_loot_school", "cloth", "bandage", "canned_food", "first_aid_kit");
            Add("loc_water_station", "Water Station", 2, 3, "table_loot_chemical_plant", "clean_water", "water_filter_cartridge", "pipe", "chlorine_tablets");
            Add("prewar_medical_cache", "Pre-War Medical Cache", 6, 4, "table_loot_hospital", "medical_kit", "bandage", "antibiotics", "morphine");
            Add("electrical_substation", "Electrical Substation", 6, 4, "table_loot_power_substation", "copper_wire_10m_of_10m", "transformer_oil", "fuse", "electronic_scrap");
            Add("checkpoint_kilo_armory", "Checkpoint Kilo Armory", 6, 4, "table_loot_checkpoint", "ammo_556", "military_rations", "field_dressing_kit", "combat_knife");
            Add("convoy_echo7_cache", "Convoy Echo-7 Cache", 7, 4, "table_loot_convoy_cache", "military_mre", "water_purification_tablets", "ammo_762", "fuel");
            Add("loc_seed_library_annex", "Seed Library Annex", 3, 4, "table_loot_farm", "item_seed_potatoes", "fertilizer", "scrap_wood", "gardening_tools");
            Add("loc_veterinary_surgery", "Large-Animal Surgery", 4, 4, "table_loot_veterinary_surgery", "suture_needle", "antiseptic", "bandage", "scalpel");
            Add("loc_cider_press", "The Cider Press", 4, 4, "table_loot_farm", "apple_mash", "scrap_wood", "clean_water", "cloth");
            Add("loc_ration_queue_plaza", "Ration Plaza", 3, 4, "table_loot_ration_plaza", "ration_token", "canned_food", "scrap_metal", "pamphlet");
            Add("loc_conscription_office", "District Conscription Office", 3, 5, "table_loot_conscription_office", "uniform_fabric", "stamped_orders", "ammo_9mm", "canteen");
            Add("loc_municipal_archive", "Municipal Archive", 4, 5, "table_loot_municipal_archive", "archive_dossier", "microfilm_spool", "lead_ledger", "paper");
            Add("loc_dentists_row", "Dentists' Row", 4, 5, "table_loot_dentists_row", "novocaine_vial", "dental_pliers", "alcohol_rub", "cotton_gauze");
            Add("loc_printworks", "The Printworks", 4, 5, "table_loot_printworks", "printing_ink", "lead_type_sorts", "heavy_cardstock", "mechanical_parts");
            Add("loc_weighbridge", "The Weighbridge", 5, 5, "table_loot_weighbridge", "cast_iron_weights", "steel_cable", "hydraulic_jack", "grease_tub");
            Add("loc_motel_verity", "The Verity Motel", 6, 5, "table_loot_apartment_block", "bedsheets", "canned_food", "soap", "matches");
            Add("hospital_pharmacy", "Hospital Pharmacy", 8, 5, "table_loot_hospital", "iodine_pills", "antibiotics", "painkillers", "surgical_gloves");
            Add("loc_terrace_pumphouse", "Terrace Pumphouse", 5, 5, "table_loot_waterworks", "pump_impeller", "gasket_seal", "clean_water", "pipe_wrench");
            Add("loc_garrison_checkpoint_gamma", "Checkpoint Gamma", 5, 4, "table_loot_checkpoint", "ammo_556", "sandbag", "barbed_wire", "military_mre");
            Add("loc_grain_silo", "The Grain Exchange", 4, 3, "table_loot_farm", "grain_sack", "pest_poison", "burlap", "scrap_wood");
            Add("abandoned_hospital", "Abandoned Hospital", 4, 6, "table_loot_hospital", "hospital_bed", "wheelchair", "oxygen_tank", "medical_kit");
            Add("loc_transit_authority_hq", "Transit Authority", 5, 6, "table_loot_transit_depot", "schedule_slate", "tokens_bus", "brake_lining", "copper_cable");
            Add("loc_department_store", "Vansen's Department Store", 5, 6, "table_loot_shopping_center", "clothing", "canned_food", "shoes", "kitchenware");
            Add("loc_public_swimming_baths", "Municipal Baths", 4, 6, "table_loot_swimming_baths", "chlorine_drum", "tile_shards", "lead_pipe", "towel");
            Add("loc_recovery_yard", "Recovery Yard", 6, 6, "table_loot_recovery_yard", "hydraulic_ram", "winch_cable", "steel_plate", "acetylene_torch");
            Add("loc_diesel_tank_farm", "Tank Farm 4-East", 7, 6, "table_loot_tank_farm", "fuel_diesel", "transfer_hose", "brass_nozzle", "inspection_gauge");
            Add("loc_radio_relay_mast", "Relay Mast 12", 8, 6, "table_loot_relay_mast", "vacuum_tube_triode", "coaxial_feed_cable", "copper_ground_rod", "lightning_arrester");
            Add("loc_st_brigids_almshouse", "St Brigid's Almshouse", 6, 7, "table_loot_hospice_ward", "morphine_ampoule", "clean_linen", "brass_censer", "dried_herbs");
            Add("loc_ordnance_shoulder", "The Ordnance Shoulder", 8, 7, "table_loot_ordnance_shoulder", "primer_cord", "shell_casing_brass", "cordite_sticks", "trenching_tool");
            Add("loc_lock_gate_four", "Lock Gate Four", 10, 7, "table_loot_waterworks", "sluice_valve_key", "grease_cartridge", "chain_link_heavy", "clean_water");
            Add("loc_pump_station_nine", "Pump Station Nine", 11, 7, "table_loot_waterworks", "centrifugal_seal", "cast_pipe_fitting", "clean_water", "filter_gravel");
            Add("loc_the_shallows_market", "The Shallows", 13, 7, "table_loot_shallows_market", "dried_fish", "salt_crust", "driftwood", "salvaged_nylon");
            Add("location_flooded_subway_depot", "Flooded Subway Depot", 7, 7, "table_loot_metro_station", "copper_catenary", "relay_box", "rail_spike", "flashlight");
            Add("government_bunker", "Government Bunker", 8, 8, "table_loot_government_bunker", "classified_dossier", "keycard_alpha", "radiation_suit", "ration_pack_spec_ops");
            Add("location_geo_thermal_plant_ruins", "Geo-Thermal Plant Ruins", 12, 8, "table_loot_geo_thermal_plant", "heat_exchanger_core", "titanium_fitting", "sulfur_cake", "heavy_sealant");
            Add("location_silent_observatory", "The Silent Observatory", 14, 8, "table_loot_observatory", "quartz_lens", "spectrograph_plate", "brass_gearset", "star_chart");
            Add("location_arcology_sector_4", "Arcology Sector 4", 16, 9, "table_loot_arcology_sector_4", "aeroponic_tray", "composite_structural_beam", "solar_cell", "filter_membrane");
            Add("location_ministry_of_truth_bunker", "Ministry of Truth Bunker", 12, 9, "table_loot_ministry_bunker", "shredder_teeth", "surveillance_tape", "lead_sheet", "typewriter");
            Add("location_the_dead_hand_core", "The Dead Hand Core", 18, 10, "table_loot_dead_hand_core", "rad_away", "anti_rad", "geiger_counter", "electronic_scrap");
            Add("loc_settlement_tinkers_notch", "Tinker's Notch Market", 3, 2, "table_loot_tinkers_notch", "electronic_scrap", "copper_wire_10m_of_10m", "battery", "clean_water");
            Add("loc_settlement_pilgrim_hearth", "The Pilgrim's Hearth Priory", 4, 2, "table_loot_pilgrim_hearth", "medical_kit", "bandage", "clean_water", "scrap_wood");
            Add("loc_settlement_brine_pans", "Brine-Pan Hollow Salt Camp", 4, 3, "table_loot_brine_pans", "item_crossing_traded_salt", "clean_water", "dried_rations", "scrap_metal");
            Add("loc_forestry_compound", "Forestry Compound", 6, 4, "table_loot_forestry_compound", "scrap_wood", "mechanical_parts", "scrap_metal", "fuel");
            Add("loc_warehouse_district", "Warehouse District", 5, 5, "table_loot_warehouse", "scrap_metal", "mechanical_parts", "canned_food", "battery");
        }

        private void InitializeVersionedAliases()
        {
            // Direct distress catalog non-canonical mappings
            _aliasMap["raider_ambush_site"] = "collapsed_building";
            _aliasMap["loc_bridge_seven"] = "loc_weighbridge";
            _aliasMap["location_substation_omega"] = "electrical_substation";

            // Fallback signal aliases from RadioDistressSystem built-ins
            _aliasMap["loc_checkpoint_kilo"] = "checkpoint_kilo_armory";
            _aliasMap["loc_bunker_4_east_trap"] = "collapsed_building";
            _aliasMap["loc_sector_9_substation"] = "electrical_substation";
            _aliasMap["loc_relay_44_bunker"] = "loc_weighbridge";
            _aliasMap["loc_marsh_caravan_wreck"] = "loc_water_station";
            _aliasMap["loc_meridian_cold_store"] = "loc_the_allotments";
            _aliasMap["loc_river_barge_olenka"] = "loc_lock_gate_four";
            _aliasMap["loc_field_medic_post"] = "prewar_medical_cache";

            // World map location mappings to expedition destinations
            _aliasMap["bunker_4_east"] = "collapsed_building";
            _aliasMap["bunker_4_east_trap"] = "collapsed_building";
            _aliasMap["sector_9_substation"] = "electrical_substation";
            _aliasMap["relay_44_bunker"] = "loc_weighbridge";
            _aliasMap["marsh_caravan_wreck"] = "loc_water_station";
            _aliasMap["meridian_cold_store"] = "loc_the_allotments";
            _aliasMap["river_barge_olenka"] = "loc_lock_gate_four";
            _aliasMap["field_medic_post"] = "prewar_medical_cache";
            _aliasMap["substation_omega"] = "electrical_substation";
            _aliasMap["checkpoint_kilo"] = "checkpoint_kilo_armory";
            _aliasMap["convoy_echo_7"] = "convoy_echo7_cache";
            _aliasMap["primary_school_14"] = "loc_school_gymnasium";
            _aliasMap["cape_verity_lighthouse"] = "loc_motel_verity";
            _aliasMap["north_dam_control"] = "loc_pump_station_nine";
        }

        private void InitializeScavengingTableMappings()
        {
            _scavengingTableMap["table_loot_farm"] = "loc_the_allotments";
            _scavengingTableMap["table_loot_power_substation"] = "loc_denial_cut_substation";
            _scavengingTableMap["table_loot_apartment_block"] = "suburban_house";
            _scavengingTableMap["table_loot_industrial_district"] = "rural_gas_station";
            _scavengingTableMap["table_loot_concert_hall"] = "concert_hall_ruins";
            _scavengingTableMap["table_loot_school"] = "loc_school_gymnasium";
            _scavengingTableMap["table_loot_warehouse"] = "loc_warehouse_district";
            _scavengingTableMap["table_loot_collapsed_structure"] = "collapsed_building";
            _scavengingTableMap["table_loot_chemical_plant"] = "loc_water_station";
            _scavengingTableMap["table_loot_hospital"] = "hospital_pharmacy";
            _scavengingTableMap["table_loot_checkpoint"] = "checkpoint_kilo_armory";
            _scavengingTableMap["table_loot_convoy_cache"] = "convoy_echo7_cache";
            _scavengingTableMap["table_loot_weighbridge"] = "loc_weighbridge";
            _scavengingTableMap["table_loot_waterworks"] = "loc_terrace_pumphouse";
            _scavengingTableMap["table_loot_recovery_yard"] = "loc_recovery_yard";
            _scavengingTableMap["table_loot_tank_farm"] = "loc_diesel_tank_farm";
            _scavengingTableMap["table_loot_relay_mast"] = "loc_radio_relay_mast";
            _scavengingTableMap["table_loot_hospice_ward"] = "loc_st_brigids_almshouse";
            _scavengingTableMap["table_loot_ordnance_shoulder"] = "loc_ordnance_shoulder";
            _scavengingTableMap["table_loot_shallows_market"] = "loc_the_shallows_market";
            _scavengingTableMap["table_loot_metro_station"] = "location_flooded_subway_depot";
            _scavengingTableMap["table_loot_government_bunker"] = "government_bunker";
            _scavengingTableMap["table_loot_geo_thermal_plant"] = "location_geo_thermal_plant_ruins";
            _scavengingTableMap["table_loot_observatory"] = "location_silent_observatory";
            _scavengingTableMap["table_loot_arcology_sector_4"] = "location_arcology_sector_4";
            _scavengingTableMap["table_loot_ministry_bunker"] = "location_ministry_of_truth_bunker";
            _scavengingTableMap["table_loot_dead_hand_core"] = "location_the_dead_hand_core";
            _scavengingTableMap["table_loot_tinkers_notch"] = "loc_settlement_tinkers_notch";
            _scavengingTableMap["table_loot_pilgrim_hearth"] = "loc_settlement_pilgrim_hearth";
            _scavengingTableMap["table_loot_brine_pans"] = "loc_settlement_brine_pans";
            _scavengingTableMap["table_loot_forestry_compound"] = "loc_forestry_compound";
        }

        private void InitializeSignalThematicMappings()
        {
            // Expansion & primary signals with narrative origins mapped to thematic canonical destinations
            _signalThematicMap["freq_distress_77_3"] = "loc_the_allotments";        // Meridian Cold Store seed potatoes
            _signalThematicMap["freq_distress_162_8"] = "loc_lock_gate_four";      // Barge Olenka
            _signalThematicMap["freq_distress_55_1"] = "loc_school_gymnasium";      // Primary School 14
            _signalThematicMap["freq_distress_311_0"] = "loc_settlement_brine_pans"; // Salt Mine Survey
            _signalThematicMap["freq_distress_401_9"] = "loc_radio_relay_mast";    // Relay Kestrel-9
            _signalThematicMap["freq_distress_88_5"] = "rural_gas_station";         // Caravan Greybell
            _signalThematicMap["freq_distress_217_4"] = "abandoned_hospital";       // District Hospital 3
            _signalThematicMap["freq_distress_148_2"] = "loc_motel_verity";        // Cape Verity Lighthouse
            _signalThematicMap["freq_distress_392_7"] = "loc_pump_station_nine";    // Weather Station Gamma / North Dam Control
            _signalThematicMap["freq_distress_101_3"] = "location_silent_observatory"; // Unknown Origin Band 6 Loop
            _signalThematicMap["freq_distress_55_6"] = "checkpoint_kilo_armory";    // Patrol Echo-4
            _signalThematicMap["freq_distress_88_9"] = "loc_the_allotments";        // Allotment 7 Greenhouse
        }
    }
}
