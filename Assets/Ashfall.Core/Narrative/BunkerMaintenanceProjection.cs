// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Narrative
{
    public enum SubsystemCategory
    {
        HeatingAndSteam,
        WaterAndDrainage,
        PowerAndElectrical,
        VentilationAndAir,
        StructuralAndAirlock,
        BiosphereAndGreenhouse,
        CommunicationsAndSensors,
        CommandAndControl,
        Other
    }

    /// <summary>
    /// Plan 148: Projects authored subterranean maintenance glitches to canonical shelter rooms (room_*),
    /// subsystem categories, and live machine condition keys without mutating simulation state.
    /// </summary>
    public static class BunkerMaintenanceProjection
    {
        private static readonly Dictionary<string, string> GlitchToRoomMap =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["glitch_01_radiator_header_steam_fracture"] = "room_bunker_corridor",
                ["glitch_02_artesian_intake_cavitation_hammer"] = "room_water_pump",
                ["glitch_03_main_substation_neutral_ground_loop"] = "room_workshop",
                ["glitch_04_intake_air_filter_black_mold_clog"] = "room_filtration",
                ["glitch_05_boiler_firebox_anthracite_slagging"] = "room_filtration",
                ["glitch_06_sewage_sump_float_switch_jam"] = "room_water_pump",
                ["glitch_07_diesel_genset_injector_carbon_knock"] = "room_filtration",
                ["glitch_08_hydroponic_led_ballast_resonance_buzz"] = "room_greenhouse",
                ["glitch_09_blast_door_hydraulic_ram_seal_leak"] = "room_airlock",
                ["glitch_10_central_chronometer_escapement_slip"] = "room_bunker_corridor",
                ["glitch_11_co2_chemical_scrubber_bed_channeling"] = "room_filtration",
                ["glitch_12_refrigerated_seed_vault_glycol_chiller_freeze"] = "room_storage_bay",
                ["glitch_13_pneumatic_tube_dispatch_capsule_jam"] = "room_clinic",
                ["glitch_14_decontamination_shower_thermostatic_valve_failure"] = "room_airlock",
                ["glitch_15_potable_water_ozonizer_high_voltage_sparkover"] = "room_water_pump",
                ["glitch_16_periscope_optical_prism_dewing_condensation"] = "room_airlock",
                ["glitch_17_canteen_grease_trap_anaerobic_gas_burp"] = "room_kitchen",
                ["glitch_18_substation_battery_rack_thermal_runaway"] = "room_workshop",
                ["glitch_19_radio_transmitter_mercury_arc_rectifier_flashover"] = "room_radio_tuner",
                ["glitch_20_the_century_seed_pneumatic_gate_final_lube"] = "room_main",
            };

        private static readonly Dictionary<string, SubsystemCategory> GlitchToCategoryMap =
            new Dictionary<string, SubsystemCategory>(StringComparer.OrdinalIgnoreCase)
            {
                ["glitch_01_radiator_header_steam_fracture"] = SubsystemCategory.HeatingAndSteam,
                ["glitch_02_artesian_intake_cavitation_hammer"] = SubsystemCategory.WaterAndDrainage,
                ["glitch_03_main_substation_neutral_ground_loop"] = SubsystemCategory.PowerAndElectrical,
                ["glitch_04_intake_air_filter_black_mold_clog"] = SubsystemCategory.VentilationAndAir,
                ["glitch_05_boiler_firebox_anthracite_slagging"] = SubsystemCategory.HeatingAndSteam,
                ["glitch_06_sewage_sump_float_switch_jam"] = SubsystemCategory.WaterAndDrainage,
                ["glitch_07_diesel_genset_injector_carbon_knock"] = SubsystemCategory.PowerAndElectrical,
                ["glitch_08_hydroponic_led_ballast_resonance_buzz"] = SubsystemCategory.BiosphereAndGreenhouse,
                ["glitch_09_blast_door_hydraulic_ram_seal_leak"] = SubsystemCategory.StructuralAndAirlock,
                ["glitch_10_central_chronometer_escapement_slip"] = SubsystemCategory.CommandAndControl,
                ["glitch_11_co2_chemical_scrubber_bed_channeling"] = SubsystemCategory.VentilationAndAir,
                ["glitch_12_refrigerated_seed_vault_glycol_chiller_freeze"] = SubsystemCategory.BiosphereAndGreenhouse,
                ["glitch_13_pneumatic_tube_dispatch_capsule_jam"] = SubsystemCategory.CommunicationsAndSensors,
                ["glitch_14_decontamination_shower_thermostatic_valve_failure"] = SubsystemCategory.StructuralAndAirlock,
                ["glitch_15_potable_water_ozonizer_high_voltage_sparkover"] = SubsystemCategory.WaterAndDrainage,
                ["glitch_16_periscope_optical_prism_dewing_condensation"] = SubsystemCategory.CommandAndControl,
                ["glitch_17_canteen_grease_trap_anaerobic_gas_burp"] = SubsystemCategory.WaterAndDrainage,
                ["glitch_18_substation_battery_rack_thermal_runaway"] = SubsystemCategory.PowerAndElectrical,
                ["glitch_19_radio_transmitter_mercury_arc_rectifier_flashover"] = SubsystemCategory.CommunicationsAndSensors,
                ["glitch_20_the_century_seed_pneumatic_gate_final_lube"] = SubsystemCategory.StructuralAndAirlock,
            };

        public static string ResolveRoomForGlitch(string glitchId)
        {
            if (string.IsNullOrEmpty(glitchId)) return "room_bunker_corridor";
            if (GlitchToRoomMap.TryGetValue(glitchId, out var roomId))
            {
                return roomId;
            }
            return "room_bunker_corridor";
        }

        public static SubsystemCategory ResolveCategory(string glitchId)
        {
            if (string.IsNullOrEmpty(glitchId)) return SubsystemCategory.Other;
            if (GlitchToCategoryMap.TryGetValue(glitchId, out var category))
            {
                return category;
            }
            return SubsystemCategory.Other;
        }

        public static List<BunkerGlitchEntry> GetGlitchesForRoom(BunkerMaintenanceCatalog catalog, string roomId)
        {
            var results = new List<BunkerGlitchEntry>();
            if (catalog == null || string.IsNullOrEmpty(roomId)) return results;

            for (int i = 0; i < catalog.AllGlitches.Count; i++)
            {
                var g = catalog.AllGlitches[i];
                if (string.Equals(ResolveRoomForGlitch(g.glitch_id), roomId, StringComparison.OrdinalIgnoreCase))
                {
                    results.Add(g);
                }
            }
            return results;
        }

        public static List<BunkerGlitchEntry> GetGlitchesForCategory(BunkerMaintenanceCatalog catalog, SubsystemCategory category)
        {
            var results = new List<BunkerGlitchEntry>();
            if (catalog == null) return results;

            for (int i = 0; i < catalog.AllGlitches.Count; i++)
            {
                var g = catalog.AllGlitches[i];
                if (ResolveCategory(g.glitch_id) == category)
                {
                    results.Add(g);
                }
            }
            return results;
        }

        public static List<BunkerGlitchEntry> MatchGlitchesForConditionKey(BunkerMaintenanceCatalog catalog, string conditionKey)
        {
            var results = new List<BunkerGlitchEntry>();
            if (catalog == null || string.IsNullOrEmpty(conditionKey)) return results;

            SubsystemCategory targetCat;
            if (conditionKey.StartsWith("power.", StringComparison.OrdinalIgnoreCase))
                targetCat = SubsystemCategory.PowerAndElectrical;
            else if (conditionKey.StartsWith("thermal.", StringComparison.OrdinalIgnoreCase) || conditionKey.StartsWith("foundry.", StringComparison.OrdinalIgnoreCase))
                targetCat = SubsystemCategory.HeatingAndSteam;
            else if (conditionKey.StartsWith("ventilation.", StringComparison.OrdinalIgnoreCase) || conditionKey.StartsWith("hepa.", StringComparison.OrdinalIgnoreCase))
                targetCat = SubsystemCategory.VentilationAndAir;
            else if (conditionKey.StartsWith("water.", StringComparison.OrdinalIgnoreCase))
                targetCat = SubsystemCategory.WaterAndDrainage;
            else if (conditionKey.StartsWith("airlock.", StringComparison.OrdinalIgnoreCase))
                targetCat = SubsystemCategory.StructuralAndAirlock;
            else
                return results;

            return GetGlitchesForCategory(catalog, targetCat);
        }
    }
}
