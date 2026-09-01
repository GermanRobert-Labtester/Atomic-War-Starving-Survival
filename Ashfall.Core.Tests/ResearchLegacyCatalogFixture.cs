// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using Ashfall.Core;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Plan 34 parity fixture: the 31 research definitions that used to live in
    /// <c>ResearchSystem.RegisterDefaults()</c>, transplanted verbatim (values and
    /// registration order) before the hardcoded production authority was deleted.
    ///
    /// This is the ONLY sanctioned copy of the legacy definitions. Production code must
    /// load <c>research_knowledge.json</c> through <c>ResearchKnowledgeCatalogLoader</c>;
    /// the fixture exists so tests can prove the externalized catalog never drifted from
    /// the behavior old saves were written against.
    /// </summary>
    public static class ResearchLegacyCatalogFixture
    {
        /// <summary>Legacy C# registration order: original 15 first, then 16 relic blueprints.</summary>
        public static IReadOnlyList<ResearchKnowledgeDef> CreateLegacyDefinitions()
        {
            var defs = new List<ResearchKnowledgeDef>
            {
                // ── Original 15 (save-contract IDs) ──
                new ResearchKnowledgeDef(
                    "knowledge_water_basics", "Water Purification Basics", "survival",
                    "Boiling, charcoal filtration, and still-building from salvage.",
                    5),
                new ResearchKnowledgeDef(
                    "knowledge_water_advanced", "Advanced Water Filtration", "survival",
                    "Multi-stage ceramic filters reduce fallout particulate by 90%.",
                    12, prerequisites: new[] { "knowledge_water_basics" },
                    breakthroughItem: "item_water_filter_advanced"),
                new ResearchKnowledgeDef(
                    "knowledge_radiation_basics", "Radiation Medicine Basics", "medical",
                    "Iodine prophylaxis, chelation agents, and dose-ledger tracking.",
                    5),
                new ResearchKnowledgeDef(
                    "knowledge_radiation_shielding", "Radiation Shielding Materials", "engineering",
                    "Layered lead-cloth, borated polyethylene, and sky-layer armour panels.",
                    15, prerequisites: new[] { "knowledge_radiation_basics" },
                    breakthroughItem: "item_radiation_shielding_panel"),
                new ResearchKnowledgeDef(
                    "knowledge_gas_mask_improved", "Improved Gas Masks", "engineering",
                    "Charcoal-canister rebuild doubles filter lifespan under heavy fallout.",
                    10, prerequisites: new[] { "knowledge_radiation_basics" },
                    breakthroughItem: "item_gas_mask_improved"),
                new ResearchKnowledgeDef(
                    "knowledge_hydroponics", "Hydroponic Cultivation", "survival",
                    "Nutrient-film technique in recycled bunker trays yields greens in 14 days.",
                    8),
                new ResearchKnowledgeDef(
                    "knowledge_solar_basics", "Solar Power Basics", "engineering",
                    "Junction-box rebuild and panel-angle tracking from scrap photovoltaic cells.",
                    7),
                new ResearchKnowledgeDef(
                    "knowledge_solar_advanced", "Solar Power Systems", "engineering",
                    "Battery-bank topology and inverter rebuild for overnight draw.",
                    14, prerequisites: new[] { "knowledge_solar_basics" },
                    breakthroughItem: "item_solar_inverter"),
                new ResearchKnowledgeDef(
                    "knowledge_food_preservation", "Food Preservation", "survival",
                    "Salt-curing, cold-smoking, and vacuum-seal scavenge from ruined canneries.",
                    10),
                new ResearchKnowledgeDef(
                    "knowledge_radio_basics", "Radio Signal Processing", "science",
                    "Direction-finding, squelch calibration, and Morse decoding from static.",
                    6),
                new ResearchKnowledgeDef(
                    "knowledge_radio_advanced", "Encrypted Radio Communication", "science",
                    "One-time pad key exchange and frequency-hopping from salvaged cipher rotors.",
                    12, prerequisites: new[] { "knowledge_radio_basics" },
                    breakthroughItem: "item_radio_cipher_rotor"),
                new ResearchKnowledgeDef(
                    "knowledge_shelter_insulation", "Shelter Insulation", "engineering",
                    "Spray-foam salvage and thermal-barrier panels cut bunker heat loss by 40%.",
                    8),
                new ResearchKnowledgeDef(
                    "knowledge_air_filtration", "Air Filtration Systems", "engineering",
                    "HEPA-grade filter rebuild extends bunker air-filtration lifespan by 50%.",
                    10, prerequisites: new[] { "knowledge_shelter_insulation" },
                    breakthroughItem: "item_air_filter_hepa"),
                new ResearchKnowledgeDef(
                    "knowledge_scavenge_efficiency", "Scavenge Efficiency", "scavenging",
                    "Route-mapping and weight-distribution analysis cuts expedition fatigue by 15%.",
                    7),
                new ResearchKnowledgeDef(
                    "knowledge_combat_training", "Combat Training Doctrine", "combat",
                    "Close-quarters drills and cover-fire protocols improve survivor combat readiness.",
                    8),

                // ── 16 Relic Reverse-Engineering Blueprint Knowledge Nodes ──
                new ResearchKnowledgeDef(
                    "knowledge_micro_dosimeter_blueprint", "Micro-Dosimeter Blueprint", "medical",
                    "Circuit schematic for miniaturized solid-state gamma detectors.",
                    6, breakthroughItem: "item_dosimeter_calibrated"),
                new ResearchKnowledgeDef(
                    "knowledge_water_condenser_blueprint", "Atmospheric Water Condenser Blueprint", "engineering",
                    "Peltier condensation array blueprint for extracting humidity from shelter exhaust.",
                    8, breakthroughItem: "item_desal_membrane"),
                new ResearchKnowledgeDef(
                    "knowledge_signal_amplifier_blueprint", "Signal Amplifier Blueprint", "science",
                    "Low-noise FET pre-amplifier design for distant radio signal capture.",
                    6, breakthroughItem: "item_radio_vacuum_tube"),
                new ResearchKnowledgeDef(
                    "knowledge_battery_reconditioner_blueprint", "Battery Reconditioner Blueprint", "engineering",
                    "Pulse-desulfation charger topology to restore dead lead-acid and lithium cells.",
                    8, breakthroughItem: "item_battery_reconditioned"),
                new ResearchKnowledgeDef(
                    "knowledge_hydroponic_doser_blueprint", "Hydroponic Nutrient Doser Blueprint", "survival",
                    "Automated peristaltic dosing schematic for precise nutrient and pH delivery.",
                    7, breakthroughItem: "item_hydroponic_nutrients"),
                new ResearchKnowledgeDef(
                    "knowledge_uv_sterilizer_blueprint", "UV Sterilizer Chamber Blueprint", "medical",
                    "Shortwave germicidal UV-C reactor for medical instrument and water sterilization.",
                    7, breakthroughItem: "item_surgical_kit"),
                new ResearchKnowledgeDef(
                    "knowledge_hand_centrifuge_blueprint", "Hand-Cranked Centrifuge Blueprint", "medical",
                    "High-RPM mechanical separation schematic for blood fractioning and pathogen isolation.",
                    5, breakthroughItem: "item_reagent_clean"),
                new ResearchKnowledgeDef(
                    "knowledge_seismic_geophone_blueprint", "Seismic Geophone Sensor Blueprint", "scavenging",
                    "Piezoelectric ground-vibration sensor array to detect approaching burrowers and cave-ins.",
                    6, breakthroughItem: "item_seismic_detector"),
                new ResearchKnowledgeDef(
                    "knowledge_turret_controller_blueprint", "Automated Turret Controller Blueprint", "combat",
                    "Optical target-tracking logic board for perimeter defense sentry mounts.",
                    10, breakthroughItem: "item_sentry_targeting_chip"),
                new ResearchKnowledgeDef(
                    "knowledge_encrypted_radio_blueprint", "Encrypted Military Transceiver Blueprint", "science",
                    "Frequency-hopping spread spectrum transceiver blueprint for secure long-range communications.",
                    10, breakthroughItem: "item_military_radio_module"),
                new ResearchKnowledgeDef(
                    "knowledge_radar_scope_blueprint", "Doppler Radar Scope Blueprint", "scavenging",
                    "X-band weather and threat radar schematic for long-distance expedition reconnaissance.",
                    9, breakthroughItem: "item_radar_display_tube"),
                new ResearchKnowledgeDef(
                    "knowledge_power_armor_servo_blueprint", "Exoskeleton Actuator Blueprint", "engineering",
                    "High-torque pneumatic servo actuator blueprint for load-bearing exo-frames.",
                    12, breakthroughItem: "item_hydraulic_actuator"),
                new ResearchKnowledgeDef(
                    "knowledge_vault_breach_blueprint", "Thermal Lance Breaching Rig Blueprint", "scavenging",
                    "Magnesium-core thermal cutting torch design to penetrate reinforced blast doors.",
                    8, breakthroughItem: "item_thermal_lance"),
                new ResearchKnowledgeDef(
                    "knowledge_iff_transponder_blueprint", "IFF Transponder Beacon Blueprint", "combat",
                    "Pre-war military friend-or-foe beacon encoder to bypass automated defense grids.",
                    8, breakthroughItem: "item_iff_beacon"),
                new ResearchKnowledgeDef(
                    "knowledge_cbrn_filter_blueprint", "Advanced CBRN Filter Cartridge Blueprint", "survival",
                    "Electrostatic carbon-nanotube particulate filter for full-spectrum nuclear and chemical defense.",
                    9, breakthroughItem: "item_cbrn_cartridge"),
                new ResearchKnowledgeDef(
                    "knowledge_surgical_robot_blueprint", "Surgical Manipulator Assembly Blueprint", "medical",
                    "Micron-precision servo manipulator blueprint for autonomous emergency trauma surgery.",
                    12, breakthroughItem: "item_surgical_arm_servo"),
            };
            return defs;
        }

        /// <summary>The 15 original save-contract IDs, in legacy registration order.</summary>
        public static readonly string[] Original15Ids =
        {
            "knowledge_water_basics",
            "knowledge_water_advanced",
            "knowledge_radiation_basics",
            "knowledge_radiation_shielding",
            "knowledge_gas_mask_improved",
            "knowledge_hydroponics",
            "knowledge_solar_basics",
            "knowledge_solar_advanced",
            "knowledge_food_preservation",
            "knowledge_radio_basics",
            "knowledge_radio_advanced",
            "knowledge_shelter_insulation",
            "knowledge_air_filtration",
            "knowledge_scavenge_efficiency",
            "knowledge_combat_training",
        };

        /// <summary>
        /// Load the authoritative research_knowledge.json catalog into a system —
        /// the test-project replacement for the deleted RegisterDefaults().
        /// </summary>
        public static int LoadAuthoritativeCatalogInto(Ashfall.Core.ResearchSystem system)
        {
            return ResearchKnowledgeCatalogLoader.LoadAndRegister(
                system, ResolveDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
        }

        private static string ResolveDataDir()
        {
            string baseDir = System.AppContext.BaseDirectory;
            string probe = System.IO.Path.Combine(baseDir, "Assets", "StreamingAssets", "Data");
            if (System.IO.Directory.Exists(probe)) return probe;

            string dir = baseDir;
            for (int i = 0; i < 6; i++)
            {
                probe = System.IO.Path.Combine(dir, "Assets", "StreamingAssets", "Data");
                if (System.IO.Directory.Exists(probe)) return probe;
                var parent = System.IO.Directory.GetParent(dir);
                if (parent == null) break;
                dir = parent.FullName;
            }
            return probe;
        }
    }
}
