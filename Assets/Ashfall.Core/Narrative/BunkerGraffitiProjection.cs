using System;
using System.Collections.Generic;

namespace Ashfall.Core.Narrative
{
    /// <summary>
    /// Plan 145: Maps authored graffiti/wall-text placements to canonical shelter rooms
    /// (room_*) and world locations (loc_* / canonical IDs).
    /// Prevents location string drift and keeps graffiti presentation spatially coherent.
    /// </summary>
    public static class BunkerGraffitiProjection
    {
        // Explicit 1:1 mapping for all 76 authored postings
        private static readonly Dictionary<string, string> ExplicitPostingMap =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                // ── room_bunker_corridor (Central Corridor & Concourse) ──
                ["graf_19_the_chess_tournament_standings"] = "room_bunker_corridor",
                ["graf_26_the_clandestine_cross_graffiti"] = "room_bunker_corridor",
                ["graf_acc_17_lied"] = "room_bunker_corridor",

                // ── room_filtration (Filtration Stack, Boilers & Generators) ──
                ["graf_01_the_first_stoker_rule"] = "room_filtration",
                ["graf_04_the_first_clandestine_slogan"] = "room_filtration",
                ["graf_21_the_rat_racing_league"] = "room_filtration",
                ["graf_24_the_first_flower_in_the_duct"] = "room_filtration",
                ["graf_25_the_generator_noise_complaint"] = "room_filtration",
                ["graf_34_the_last_coal_wagon_graffiti"] = "room_filtration",
                ["graf_warn_09_filter"] = "room_filtration",
                ["graf_joke_21_stoker"] = "room_filtration",
                ["graf_joke_25_boots"] = "room_filtration",
                ["graf_mark_37_fuel"] = "room_filtration",

                // ── room_kitchen (Galley Kitchen & Canteen) ──
                ["graf_02_ration_biscuit_warning"] = "room_kitchen",
                ["graf_11_the_mushroom_soup_boycott"] = "room_kitchen",
                ["graf_27_the_tea_substitute_review"] = "room_kitchen",
                ["graf_32_the_great_pancake_day"] = "room_kitchen",
                ["graf_dir_06_mess_hall"] = "room_kitchen",
                ["graf_joke_20_biscuit"] = "room_kitchen",
                ["graf_joke_23_coffee"] = "room_kitchen",
                ["graf_joke_24_tuesday"] = "room_kitchen",

                // ── room_clinic (Medical Ward & Infirmary) ──
                ["graf_06_vel_dispensary_notice"] = "room_clinic",
                ["graf_13_the_baby_tally"] = "room_clinic",
                ["graf_dir_03_school"] = "room_clinic",
                ["graf_acc_18_who"] = "room_clinic",
                ["graf_grief_29_victor"] = "room_clinic",
                ["graf_grief_30_river"] = "room_clinic",

                // ── room_workshop (Workshop & Fabrication) ──
                ["graf_03_missing_spanner"] = "room_workshop",
                ["graf_09_the_shoe_sole_controversy"] = "room_workshop",
                ["graf_20_the_sapper_elegy"] = "room_workshop",
                ["graf_22_the_candle_ration_strike"] = "room_workshop",

                // ── room_bunks (Bunk Living Quarters) ──
                ["graf_05_the_snoring_manifesto"] = "room_bunks",
                ["graf_grief_26_dima"] = "room_bunks",
                ["graf_grief_31_empty"] = "room_bunks",
                ["graf_mark_33_key"] = "room_bunks",

                // ── room_storage_bay (Storage Bay & Lockers) ──
                ["graf_warn_10_dont_drink"] = "room_storage_bay",
                ["graf_acc_15_thief"] = "room_storage_bay",
                ["graf_mark_32_tally"] = "room_storage_bay",

                // ── room_radio_tuner (Tuner Station) ──
                ["graf_18_the_black_wire_warning"] = "room_radio_tuner",
                ["graf_23_the_radio_song_request_slate"] = "room_radio_tuner",

                // ── room_foundry (Silent Foundry) ──
                ["graf_30_the_foundry_strike_threat"] = "room_foundry",

                // ── room_greenhouse (Greenhouse / Hydroponics) ──
                ["graf_07_secret_romance_cipher"] = "room_greenhouse",
                ["graf_16_the_hydroponic_slug_bounty"] = "room_greenhouse",

                // ── room_main (Main Vault / Assembly Concourse / Crypt) ──
                ["graf_10_the_weepers_psalm"] = "room_main",
                ["graf_15_the_century_seed_pledge"] = "room_main",
                ["graf_31_the_secret_library_alcove"] = "room_main",
                ["graf_35_the_charter_ratification_notice"] = "room_main",
                ["graf_grief_27_names"] = "room_main",
                ["graf_long_40_wall"] = "room_main",

                // ── room_airlock (Airlock Hatch & Sentry Cupola) ──
                ["graf_08_voss_inspection_order"] = "room_airlock",
                ["graf_14_tobacco_contraband_warning"] = "room_airlock",
                ["graf_29_the_sentry_weather_rhyme"] = "room_airlock",
                ["graf_33_the_first_surface_wedding_announcement"] = "room_airlock",
                ["graf_36_the_final_slate_greeting"] = "room_airlock",
                ["graf_warn_13_quiet"] = "room_airlock",
                ["graf_joke_22_sun"] = "room_airlock",
                ["graf_mark_35_shift"] = "room_airlock",
                ["graf_long_39_sun_face"] = "room_airlock",

                // ── room_water_pump (Water Pump & Sump Grates) ──
                ["graf_17_the_frozen_pipe_blame_game"] = "room_water_pump",
                ["graf_28_the_lost_wedding_band"] = "room_water_pump",
                ["graf_dir_01_pump"] = "room_water_pump",
                ["graf_dir_02_well"] = "room_water_pump",
                ["graf_mark_36_water"] = "room_water_pump",

                // ── World Locations ──
                ["graf_warn_14_gas"] = "location_submerged_data_center",
                ["graf_warn_11_floor"] = "suburban_house",
                ["graf_acc_19_left"] = "suburban_house",
                ["graf_grief_28_stone"] = "location_ash_dune_cemetery",
                ["graf_dir_05_wrong_way"] = "rural_gas_station",
                ["graf_long_38_stop"] = "rural_gas_station",
                ["graf_warn_07_rad"] = "government_bunker",
                ["graf_warn_08_rad_added"] = "government_bunker",
                ["graf_12_the_ghost_train_rumor"] = "location_sub_level_4_transit",
                ["graf_warn_12_live"] = "location_geo_thermal_plant_ruins",
                ["graf_mark_34_locker"] = "location_municipal_sewage",
                ["graf_dir_04_cache"] = "stranger_cache",
                ["graf_acc_16_raid"] = "stranger_cache"
            };

        /// <summary>
        /// Resolves the canonical target ID for a graffiti posting.
        /// Returns a canonical shelter room ID ("room_*") or canonical world location ID.
        /// </summary>
        public static string ResolveCanonicalTarget(BunkerGraffitiEntry entry)
        {
            if (entry == null) return "room_bunker_corridor";
            return ResolveCanonicalTarget(entry.posting_id, entry.location);
        }

        /// <summary>
        /// Resolves the canonical target ID for a given posting ID and location string.
        /// </summary>
        public static string ResolveCanonicalTarget(string postingId, string locationText)
        {
            if (!string.IsNullOrEmpty(postingId) && ExplicitPostingMap.TryGetValue(postingId, out var target))
            {
                return target;
            }

            if (string.IsNullOrEmpty(locationText))
            {
                return "room_bunker_corridor";
            }

            string loc = locationText.ToLowerInvariant();

            // Pattern heuristics for any future expansion postings
            if (loc.Contains("pump") || loc.Contains("well") || loc.Contains("water manifold"))
                return "room_water_pump";
            if (loc.Contains("filter") || loc.Contains("exhaust") || loc.Contains("generator") || loc.Contains("boiler") || loc.Contains("lignite") || loc.Contains("fuel"))
                return "room_filtration";
            if (loc.Contains("kitchen") || loc.Contains("canteen") || loc.Contains("mess hall") || loc.Contains("baker"))
                return "room_kitchen";
            if (loc.Contains("clinic") || loc.Contains("infirmary") || loc.Contains("dispensary") || loc.Contains("ward"))
                return "room_clinic";
            if (loc.Contains("workshop") || loc.Contains("machine shop") || loc.Contains("tool rack"))
                return "room_workshop";
            if (loc.Contains("bunk") || loc.Contains("bed"))
                return "room_bunks";
            if (loc.Contains("storage") || loc.Contains("stores") || loc.Contains("locker"))
                return "room_storage_bay";
            if (loc.Contains("airlock") || loc.Contains("blast door") || loc.Contains("cupola") || loc.Contains("sentry"))
                return "room_airlock";
            if (loc.Contains("radio") || loc.Contains("tuner"))
                return "room_radio_tuner";
            if (loc.Contains("foundry") || loc.Contains("smelter"))
                return "room_foundry";
            if (loc.Contains("hydroponic") || loc.Contains("greenhouse"))
                return "room_greenhouse";
            if (loc.Contains("assembly") || loc.Contains("crypt") || loc.Contains("library") || loc.Contains("archive") || loc.Contains("tally"))
                return "room_main";
            if (loc.Contains("data center"))
                return "location_submerged_data_center";
            if (loc.Contains("house") || loc.Contains("suburban"))
                return "suburban_house";
            if (loc.Contains("cemetery") || loc.Contains("grave"))
                return "location_ash_dune_cemetery";
            if (loc.Contains("road") || loc.Contains("crossroad"))
                return "rural_gas_station";
            if (loc.Contains("bunker perimeter") || loc.Contains("government bunker"))
                return "government_bunker";
            if (loc.Contains("rail") || loc.Contains("transit"))
                return "location_sub_level_4_transit";
            if (loc.Contains("substation") || loc.Contains("geothermal"))
                return "location_geo_thermal_plant_ruins";
            if (loc.Contains("sewage") || loc.Contains("baths"))
                return "location_municipal_sewage";
            if (loc.Contains("scavenger"))
                return "stranger_cache";

            return "room_bunker_corridor";
        }
    }
}
