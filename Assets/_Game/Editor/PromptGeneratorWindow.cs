using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace AtomicWar._Game.Editor
{
    /// <summary>
    /// Reads the asset manifest and generates ready-to-use prompt files
    /// for Firefly/FLUX.2 AI image generation. Outputs .txt files with
    /// one prompt per line, organized by batch.
    ///
    /// Run via: Tools → ASHFALL → Generate AI Prompts → [Batch]
    /// </summary>
    public class PromptGeneratorWindow : EditorWindow
    {
        private const string PromptsOutputDir = "Assets/_Game/Sprites/Prompts";
        private const string ManifestPath = "Assets/_Game/Sprites/asset_manifest.json";

        private static readonly string ItemSuffix =
            "post-apocalyptic survival game inventory icon, isolated object centered on " +
            "pure flat black (#000000) background, dramatic directional rim lighting from " +
            "top-left, volumetric dust particles, desaturated color palette with selective " +
            "orange-amber highlights, worn and weathered textures, micro-scratches and " +
            "grime detail, photorealistic material rendering, cinematic product shot, " +
            "no text, no labels, no shadows outside object, no background elements";

        private static readonly string ItemNegative =
            "cartoon, anime, flat icon, logo, watermark, signature, text, label, " +
            "gradient background, colorful background, bright colors, clean new condition, " +
            "fantasy, sci-fi laser, alien, UI chrome";

        private static readonly string EnvSuffix =
            "Original 2D hand-painted survival-management game art, grounded grim realism, " +
            "charcoal pencil underdrawing and dry gouache texture, cold restrained palette " +
            "of charcoal, concrete grey, faded blue-grey, rust brown, dirty bone, and rare " +
            "muted amber practical light; radiation is a subtle cyan-green contamination " +
            "cue only. Nuclear-winter ash, condensation, repair marks, functional materials. " +
            "No text, logos, flags, brands, readable labels, fantasy, gore, or weapon glamour.";

        private static readonly string PortraitSuffix =
            "Original 2D hand-painted survival-management game art, grounded grim realism, " +
            "charcoal pencil underdrawing and dry gouache texture, cold restrained palette " +
            "of charcoal, concrete grey, faded blue-grey, rust brown, dirty bone. " +
            "Chest-up three-quarter view portrait, restrained expression, no text, no logos.";

        private static readonly string GlobalNegative =
            "text, letters, numbers, watermark, logo, flag, brand, neon cyberpunk, " +
            "glossy sci-fi, cartoon, anime, photorealism, oversaturated colors, gore, " +
            "distorted anatomy, duplicated objects";

        [MenuItem("Tools/ASHFALL/Generate AI Prompts/All Item Batches")]
        public static void GenerateAllItemPrompts()
        {
            GenerateAmmoDeprecatedPrompts();
            GenerateAmmoBoxPrompts();
            GenerateContainerPrompts();
            GenerateDevicePrompts();
            Debug.Log("[PromptGen] All item prompt files generated in " + PromptsOutputDir);
        }

        [MenuItem("Tools/ASHFALL/Generate AI Prompts/All From Manifest")]
        public static void GenerateAllFromManifest()
        {
            if (!File.Exists(ManifestPath))
            {
                Debug.LogError("[PromptGen] asset_manifest.json not found.");
                return;
            }

            var manifest = JsonUtility.FromJson<ManifestRoot>(File.ReadAllText(ManifestPath));
            if (manifest?.categories == null)
            {
                Debug.LogError("[PromptGen] Manifest categories failed to parse.");
                return;
            }

            WriteCategoryPrompts("batch_A1_deprecated_ammo.txt", "Deprecated Ammo", "item",
                manifest.categories.items_ammo_deprecated);
            WriteCategoryPrompts("batch_A2_military_ammo_boxes.txt", "Military Ammo Boxes", "item",
                manifest.categories.items_ammo_military_boxes);
            WriteCategoryPrompts("batch_A3_weapons.txt", "Weapons", "item",
                manifest.categories.items_weapons);
            WriteCategoryPrompts("batch_A6_containers.txt", "Containers", "item",
                manifest.categories.items_containers);
            WriteCategoryPrompts("batch_A7_devices_medical.txt", "Devices Medical Tools", "item",
                manifest.categories.items_devices_medical_tools);
            WriteCategoryPrompts("batch_D_locations.txt", "Locations", "location",
                manifest.categories.locations);
            WriteCategoryPrompts("batch_E_portraits.txt", "Survivor Portraits", "portrait",
                manifest.categories.survivors);
            WriteCategoryPrompts("batch_F_factions.txt", "Factions", "faction",
                manifest.categories.factions);
            WriteCategoryPrompts("batch_F_weather.txt", "Weather", "weather",
                manifest.categories.weather);

            Debug.Log("[PromptGen] All manifest prompt files generated in " + PromptsOutputDir);
        }

        private static void WriteCategoryPrompts(string filename, string title, string kind, ManifestSpriteList list)
        {
            if (list?.sprites == null || list.sprites.Length == 0) return;

            var sb = new StringBuilder();
            string suffix = kind == "item" ? ItemSuffix : kind == "portrait" ? PortraitSuffix : EnvSuffix;
            string negative = kind == "item" ? ItemNegative : GlobalNegative;
            string aspect = kind == "item" ? "1:1 | 1024×1024" : kind == "portrait" ? "3:4 | 768×1024" : "16:9 | 1920×1080";

            sb.AppendLine($"# ASHFALL — {title} ({list.sprites.Length})");
            sb.AppendLine($"# Model: Flux 2 Pro via Firefly | Aspect: {aspect}");
            sb.AppendLine("# Suffix: " + suffix);
            sb.AppendLine("# Negative: " + negative);
            sb.AppendLine();

            foreach (var id in list.sprites)
            {
                string readable = id.Replace('_', ' ');
                string subject = kind switch
                {
                    "item" => $"Isolated inventory object: {readable}.",
                    "portrait" => $"Chest-up three-quarter portrait of survivor {readable}, exhausted restrained expression, worn cold-weather layers.",
                    "location" => $"Establishing shot of {readable} after nuclear winter, ash, repair marks, no people in focus.",
                    "faction" => $"Group lineup of {readable}, three to four figures, functional gear, no flags or readable insignia.",
                    _ => $"Weather overlay of {readable} over ruined concrete and ash."
                };
                sb.AppendLine($"## {id}");
                sb.AppendLine($"{subject} [{suffix}]");
                sb.AppendLine($"--negative {negative}");
                sb.AppendLine($"Output: {id}.png");
                sb.AppendLine();
            }

            WritePromptFile(filename, sb.ToString());
        }

        [System.Serializable]
        private class ManifestRoot
        {
            public ManifestCategories categories;
        }

        [System.Serializable]
        private class ManifestCategories
        {
            public ManifestSpriteList items_ammo_deprecated;
            public ManifestSpriteList items_ammo_military_boxes;
            public ManifestSpriteList items_weapons;
            public ManifestSpriteList items_containers;
            public ManifestSpriteList items_devices_medical_tools;
            public ManifestSpriteList locations;
            public ManifestSpriteList survivors;
            public ManifestSpriteList factions;
            public ManifestSpriteList weather;
        }

        [System.Serializable]
        private class ManifestSpriteList
        {
            public string[] sprites;
        }

        [MenuItem("Tools/ASHFALL/Generate AI Prompts/A1 - Deprecated Ammo (19)")]
        public static void GenerateAmmoDeprecatedPrompts()
        {
            var ids = new[] { "9x19", "380acp", "762x25", "45acp", "9x21", "765x21",
                "12ga", "16ga", "556x45", "762x39", "545x39", "762x51", "300blk",
                "57x28", "46x30", "762x54r", "338lapua", "408cheytac", "50bmg" };

            var sb = new StringBuilder();
            sb.AppendLine("# ASHFALL — Deprecated Ammo Batch (19 items)");
            sb.AppendLine("# Model: Flux 2 Pro via Firefly | Aspect: 1:1 | Resolution: 1024×1024");
            sb.AppendLine("# Suffix: " + ItemSuffix);
            sb.AppendLine("# Negative: " + ItemNegative);
            sb.AppendLine();

            foreach (var cal in ids)
            {
                sb.AppendLine($"## ammo_deprecated_{cal.Replace(".","")}");
                sb.AppendLine($"Small handful of corroded brass cartridge casings, green-black " +
                    $"oxidation crusting the surface, dented and bent from age, tarnished " +
                    $"headstamp barely visible, loose pile of 3-5 rounds. [{ItemSuffix}]");
                sb.AppendLine($"--negative {ItemNegative}");
                sb.AppendLine($"Output: ammo_deprecated_{cal.Replace(".","")}.png");
                sb.AppendLine();
            }

            WritePromptFile("batch_A1_deprecated_ammo.txt", sb.ToString());
        }

        [MenuItem("Tools/ASHFALL/Generate AI Prompts/A2 - Military Ammo Boxes (16)")]
        public static void GenerateAmmoBoxPrompts()
        {
            var ids = new[] { "ammo_545x39_jhp_ap", "ammo_545x39_exi", "ammo_545x39_api",
                "ammo_300blk_jhp_ap", "ammo_57x28_jhp_ap", "ammo_57x28_exi",
                "ammo_57x28_api", "ammo_762x54r_jhp_ap", "ammo_762x54r_exi",
                "ammo_762x54r_api", "ammo_338lapua_jhp_ap", "ammo_408cheytac_jhp_ap",
                "ammo_762x51_jhp_ap", "ammo_762x51_exi",
                "ammo_50bmg_jhp_ap", "ammo_50bmg_exi" };

            var sb = new StringBuilder();
            sb.AppendLine("# ASHFALL — Military Ammo Box Batch (16 items)");
            sb.AppendLine("# Model: Flux 2 Pro via Firefly | Aspect: 1:1 | Resolution: 1024×1024");
            sb.AppendLine("# Suffix: " + ItemSuffix);
            sb.AppendLine("# Negative: " + ItemNegative);
            sb.AppendLine();

            foreach (var id in ids)
            {
                sb.AppendLine($"## {id}");
                sb.AppendLine($"Small cardboard ammunition box, lid half open, rows of clean " +
                    $"brass-and-copper cartridges inside, caliber stencil-stamped on the end, " +
                    $"one loose round resting beside the box. [{ItemSuffix}]");
                sb.AppendLine($"--negative {ItemNegative}");
                sb.AppendLine($"Output: {id}.png");
                sb.AppendLine();
            }

            WritePromptFile("batch_A2_military_ammo_boxes.txt", sb.ToString());
        }

        [MenuItem("Tools/ASHFALL/Generate AI Prompts/A6 - Containers (20)")]
        public static void GenerateContainerPrompts()
        {
            var prompts = new Dictionary<string, string>
            {
                {"water_bottle", "Scratched clear plastic bottle, screw cap, condensation beads, visible water fill line. [{ItemSuffix}]"},
                {"fuel_0_5l_of_1l", "Dented metal fuel can, faded red paint, wire-secured cap, dark fuel stains. [{ItemSuffix}]"},
                {"accelerant_half", "Dented metal accelerant can, faded label, wire-secured cap, dark stains. [{ItemSuffix}]"},
                {"ejuice", "Small plastic vape bottle, childproof cap, nicotine strength scratched into a makeshift label. [{ItemSuffix}]"},
                {"water_purification_tablets", "Amber pill bottle, tablets visible through translucent plastic, worn cap. [{ItemSuffix}]"},
                {"iodine_pills_bottle_10_of_10", "Small amber pill bottle, half the tablets visible, worn cap. [{ItemSuffix}]"},
                {"jetfuel_jerrycan_10l_of_10l", "Large NATO-style jerrycan, stenciled markings worn, chained spout cap. [{ItemSuffix}]"},
                {"instant_coffee_10x_container", "Small dented tin canister of dark granules, worn label. [{ItemSuffix}]"},
                {"ice_tea_0_5l_package", "Collapsed foil drink pouch, printed graphics faded past reading. [{ItemSuffix}]"},
                {"package_rolled_oats_1kg_of_1kg", "Paper sack, top folded and clipped, flour dust on the seams. [{ItemSuffix}]"},
                {"dry_rice_1kg_of_1kg", "Clear plastic bag of rice, twist-tied top. [{ItemSuffix}]"},
                {"dried_pasta_2kg_of_2kg", "Clear bag of dried pasta shapes, folded cardboard header worn blank. [{ItemSuffix}]"},
                {"soy_and_rice_milk_1l_of_1l", "Carton with a fold-top spout, faded print, slight dent. [{ItemSuffix}]"}
            };

            var sb = new StringBuilder();
            sb.AppendLine("# ASHFALL — Container Batch");
            sb.AppendLine("# Model: Flux 2 Pro via Firefly | Aspect: 1:1 | Resolution: 1024×1024");
            sb.AppendLine("# Negative: " + ItemNegative);
            sb.AppendLine();

            foreach (var kv in prompts)
            {
                sb.AppendLine($"## {kv.Key}");
                sb.AppendLine(kv.Value);
                sb.AppendLine($"--negative {ItemNegative}");
                sb.AppendLine($"Output: {kv.Key}.png");
                sb.AppendLine();
            }

            WritePromptFile("batch_A6_containers.txt", sb.ToString());
        }

        [MenuItem("Tools/ASHFALL/Generate AI Prompts/A7 - Devices & Medical (65)")]
        public static void GenerateDevicePrompts()
        {
            var prompts = new Dictionary<string, string>
            {
                {"anti_rad", "Small amber medicine vial, dropper cap, radiation trefoil scratched off the label, half empty. [{ItemSuffix}]"},
                {"prewar_letter", "Sealed cream envelope, edges browned and brittle, foreign stamp faded past reading, held shut by a paperclip. [{ItemSuffix}]"},
                {"item_bioluminescent_moss", "Small clump of pale blue-green moss in a glass jar, faint self-lit glow, clinging to a stone fragment. [{ItemSuffix}]"},
                {"hand_crank_radio", "Boxy portable radio, fold-out crank handle, scuffed housing, dial marked in worn grease pencil. [{ItemSuffix}]"},
                {"item_uv_lamp_ballast", "Heavy rectangular magnetic ballast, coiled wire visible through vents, scorch mark at one corner. [{ItemSuffix}]"},
                {"item_geothermal_valve", "Brass high-pressure valve wheel and stem, condensation beads, steam-stained patina. [{ItemSuffix}]"},
                {"item_ro_membrane", "Cylindrical white filter membrane cartridge, mineral-crusted housing, worn pressure gauge on the cap. [{ItemSuffix}]"},
                {"item_acoustic_decoy", "Small wind-up metronome wired to a tiny speaker horn, exposed wiring, key protruding from the side. [{ItemSuffix}]"},
                {"item_logic_board", "Green circuit board studded with components, several scorched black, wiring dangling from one edge. [{ItemSuffix}]"},
                {"item_co2_scrubber_cartridge", "Cylindrical white cartridge, perforated end caps, chemical-burn discoloration. [{ItemSuffix}]"},
                {"item_rebreather_scrubber", "Compact canister, screw-thread collar, mineral crust around the seams. [{ItemSuffix}]"},
                {"antiseptic_1l_of_1l", "Tall brown glass bottle, dropper cap, hand-written label. [{ItemSuffix}]"},
                {"alcohol_wipes_box_10_of_10", "Small cardboard box of foil wipe packets, crushed corner. [{ItemSuffix}]"},
                {"epi_pen", "Single yellow auto-injector pen, safety cap on, worn grip texture. [{ItemSuffix}]"},
                {"decontamination_soap_5_of_5", "Bar of grey industrial soap in torn paper wrap. [{ItemSuffix}]"},
                {"item_frostbite_salve", "Small tin of pale ointment, fingerprint smear on the lid, hand-written frost warning. [{ItemSuffix}]"},
                {"item_scopolamine_root", "Dried twisted root fragments in a cloth pouch, dark and papery. [{ItemSuffix}]"},
                {"item_lithium_salts", "Small glass jar of coarse white-grey crystals, cork stopper. [{ItemSuffix}]"},
                {"item_amnestic_syrup", "Dark brown syrup in a stoppered glass vial, faint sediment at the base. [{ItemSuffix}]"},
                {"item_snow_goggles_improvised", "Carved wood eye-guard, narrow horizontal slits, leather strap, soot-blackened interior. [{ItemSuffix}]"},
                {"item_lead_visor", "Heavy dark-tinted visor in a lead-mesh frame, thick strap, scratched lens. [{ItemSuffix}]"},
                {"item_ash_ghillie", "Loose net suit woven with pale ash-grey strips, frayed fabric, attached hood. [{ItemSuffix}]"},
                {"item_black_ice_sample", "Chunk of murky frozen water in a sealed sample jar, faint dark mineral streaks inside. [{ItemSuffix}]"},
                {"item_cobalt_salt_canister", "Squat lead-lined canister, stenciled serial number worn but legible, heavy rounded cap. [{ItemSuffix}]"},
                {"item_black_water_vial", "Small glass vial of iridescent black liquid, stopper sealed with wax. [{ItemSuffix}]"},
                {"item_submerged_server", "Rectangular sealed server blade, corroded connectors, faint waterline staining. [{ItemSuffix}]"},
                {"item_master_override", "Large ornate brass key with an encrypted digital chip embedded in the bow. [{ItemSuffix}]"},
                {"item_hard_drive_platter", "Single mirror-polished magnetic disk in a static-proof sleeve, faint scratches across the surface. [{ItemSuffix}]"},
                {"item_pre_war_photo_album", "Thick leather-bound album, water-stained pages, one photo corner peeking out. [{ItemSuffix}]"},
                {"item_vinyl_collection", "Stack of worn vinyl records in torn paper sleeves, faded cover art. [{ItemSuffix}]"},
                {"item_headphones_mil", "Padded military headset, boom mic, scuffed matte housing. [{ItemSuffix}]"},
                {"item_tether_harness", "Heavy canvas chest harness, steel D-rings, coiled steel cable. [{ItemSuffix}]"},
                {"rope_2m_of_2m", "Coiled length of frayed natural-fiber rope, one end unraveling. [{ItemSuffix}]"},
                {"copper_wire_10m_of_10m", "Tight spool of bare copper wire, oxidized green at the exposed ends. [{ItemSuffix}]"},
                {"engine_block_intact", "Large corroded cast-iron engine block, missing accessories, oil-stained. [{ItemSuffix}]"},
                {"bearing_set_industrial", "Small greased-paper packet of steel bearings, a few visible through a torn corner. [{ItemSuffix}]"},
                {"copper_tubing_1m", "Coiled length of dull copper pipe, dented in two places. [{ItemSuffix}]"}
            };

            var sb = new StringBuilder();
            sb.AppendLine("# ASHFALL — Devices, Medical & Tools Batch (65 items)");
            sb.AppendLine("# Model: Flux 2 Pro via Firefly | Aspect: 1:1 | Resolution: 1024×1024");
            sb.AppendLine("# Negative: " + ItemNegative);
            sb.AppendLine();

            foreach (var kv in prompts)
            {
                sb.AppendLine($"## {kv.Key}");
                sb.AppendLine(kv.Value);
                sb.AppendLine($"--negative {ItemNegative}");
                sb.AppendLine($"Output: {kv.Key}.png");
                sb.AppendLine();
            }

            WritePromptFile("batch_A7_devices_medical.txt", sb.ToString());
        }

        [MenuItem("Tools/ASHFALL/Generate AI Prompts/All Location Prompts (42)")]
        public static void GenerateLocationPrompts()
        {
            var locations = new (string id, string prompt)[]
            {
                ("location_silent_observatory", "Isolated mountaintop observatory dome, frost-rimed telescope housing against an impossibly clear dark sky, thin cold light over a curved ash-grey horizon."),
                ("location_the_sump_cathedral", "Vast underground cistern lit entirely by glowing blue-green moss coating every wall, still black water reflecting the light, shrine offerings along the ledge."),
                ("location_deep_core_borehole", "Immense vertical borehole shaft descending into darkness lit only by distant heat-glow far below, heavy machinery frozen mid-operation at the rim."),
                ("location_the_dead_hand_core", "Deep bunker command room dominated by a humming server core under red emergency light, one blinking override terminal at its base."),
                ("location_the_memory_vault", "Vast server-farm vault of endless dark racks under dim standby lights, dust thick on every unpowered unit, one active bank humming alone."),
                ("location_submerged_data_center", "Flooded server-farm aisle, submerged racks glowing faint standby lights beneath still black water, raised floor panels lifted and scattered."),
                ("location_magnetic_anomaly_crater", "Wide impact crater, scattered metal debris hovering faintly off the ground, compass needles visibly spinning, a bright core glinting at the center."),
                ("location_acoustic_testing_facility", "Padded anechoic chamber lined with foam wedge panels absorbing all sound, one figure's footprints the only disturbance in the dust."),
                ("location_ash_dune_cemetery", "Wide open ash dunes with human shapes half-buried and preserved beneath drifted grey powder, wind carving slow ripples across the field."),
                ("location_crashed_icebreaker_convoy", "Derailed armored train cars scattered across snow, one cracked radioactive generator glowing faint cyan through a torn hull."),
                ("location_geo_thermal_plant_ruins", "Cracked geothermal plant ruin, corroded pipes venting thin steam across boiling mud flats, blank warning signage rusted through, ground fractured in wide unstable plates."),
                ("location_flooded_subway_depot", "Flooded subway platform in near-total darkness, waist-deep water reflecting a single flashlight beam, rusted carriages half-submerged."),
                ("location_bio_remediation_lab", "Sealed lab corridor thick with pale spore haze, biohazard suits abandoned mid-collapse, fungal growth consuming equipment and floor alike."),
                ("location_radio_telescope_array", "Field of massive radio telescope dishes tilted skyward, ice-crusted and groaning under wind, a dark control building below."),
                ("location_ash_whale_carcass", "Enormous fossilized root-mass breaching the ash like a beached whale skeleton, hollow chambers within scavenged bare."),
                ("location_hospital_psych_wing", "Abandoned psychiatric ward corridor, restraint straps hanging from empty beds, scratch marks along the walls, an overturned medication cart."),
                ("location_mirror_factory", "Mirror-factory floor thick with shattered silvered glass reflecting fractured light in every direction, conveyor lines frozen mid-process."),
                ("location_substation_omega", "Electrical substation yard of towering transformers, faint blue arcs sparking between damaged capacitor banks, frost on every metal surface."),
                ("location_abandoned_ski_resort", "Frozen ski lodge, cable cars hanging mid-span over a snow-buried slope, furs and luxury debris scattered across a frost-glazed lobby."),
                ("location_concrete_batching_plant", "Industrial concrete plant, towering rusted mixing silos leaning at odd angles, rebar exposed through crumbling columns.")
            };

            var sb = new StringBuilder();
            sb.AppendLine("# ASHFALL — Location Establishing Shots (42 total, 20 priority shown)");
            sb.AppendLine("# Model: Flux 2 Pro via Firefly | Aspect: 16:9 | Resolution: 1920×1080");
            sb.AppendLine("# Suffix: " + EnvSuffix);
            sb.AppendLine("# Negative: " + GlobalNegative);
            sb.AppendLine();

            foreach (var (id, prompt) in locations)
            {
                sb.AppendLine($"## {id}");
                sb.AppendLine($"{prompt} [{EnvSuffix}]");
                sb.AppendLine($"--negative {GlobalNegative}");
                sb.AppendLine($"Output: {id}.png");
                sb.AppendLine();
            }

            WritePromptFile("batch_D_locations_priority20.txt", sb.ToString());
        }

        [MenuItem("Tools/ASHFALL/Generate AI Prompts/All Faction + Weather Prompts (20)")]
        public static void GenerateFactionWeatherPrompts()
        {
            var sb = new StringBuilder();
            sb.AppendLine("# ASHFALL — Factions + Weather Batch (20)");
            sb.AppendLine("# Model: Flux 2 Pro via Firefly | Aspect: 16:9 | Resolution: 1920×1080");
            sb.AppendLine("# Suffix: " + EnvSuffix);
            sb.AppendLine("# Negative: " + GlobalNegative);
            sb.AppendLine();

            // Factions
            string[] factions = {
                "faction_central_garrison_remnants:Disciplined survivors of the regional military garrison, faded olive fatigues and mismatched surplus body armor, insignia deliberately scratched off, weapons maintained meticulously despite the wear — composition reads chain-of-command even in ruin.",
                "faction_upland_militia:Agrarian militia in layered hunting gear and hand-sewn patches, farm tools converted to weapons, faces weathered by fieldwork rather than combat — practical, unpolished, locally-made everything.",
                "faction_cultists_of_the_glow:Robed figures in scavenged fabric, dyed and patched, symbols painted in rust-red across chest and hood, radiation-warning trefoils repurposed as religious icons, unsettling calm in high-radiation postures.",
                "faction_scavenger_warlords:Raiders in mismatched scavenged armor bolted and welded together, improvised bladed weapons, aggressive asymmetric silhouettes built from other factions' salvaged gear.",
                "faction_safe_haven_communities:Civilian survivors in practical layered clothing, no visible weapons, deliberately unthreatening posture and soft silhouette, communal/domestic details visible."
            };
            foreach (var f in factions)
            {
                var parts = f.Split(':');
                sb.AppendLine($"## {parts[0]}");
                sb.AppendLine($"{parts[1]} [{EnvSuffix}]");
                sb.AppendLine($"--negative {GlobalNegative}");
                sb.AppendLine($"Output: {parts[0]}.png\n");
            }

            // Weather
            string[] weather = {
                "weather_acid_snow:Corrosive pale-yellow snowfall pitting exposed metal surfaces, faint chemical haze low to the ground.",
                "weather_bio_fog:Dense grey-green spore fog rolling low across ruins, visibility reduced to a few meters.",
                "weather_black_snow:Heavy soot-black snowfall coating every surface, footprints the only clean ground visible.",
                "weather_blood_rain:Thin rust-red rain streaking down concrete and glass, puddles tinted dark red.",
                "weather_emp_storm:Violet-white static arcing across a clouded sky, streetlights and electronics flickering dead below.",
                "weather_glass_storm:Fine glittering dust storm with a harsh glassy sheen, sky catching light unnaturally.",
                "weather_rad_hail:Heavy grey hailstones pitting the ground, faint cyan-green residue where they land.",
                "weather_ash_lightning:Jagged white static discharge forking through a dense ash cloud, brief harsh illumination.",
                "weather_ice_storm:Freezing rain sheeting every surface in clear ice, a hatch wheel frozen mid-turn.",
                "weather_silence:Unnervingly clear still sky, no wind, no ash, no birds — wrongness in the stillness itself.",
                "weather_false_spring:Thin broken ash-cloud cover with harsh unfiltered light breaking through, deceptively bright.",
                "weather_silent_spring:Cloudless sky with a searing pale light, heat-shimmer over frozen ground, no wind at all."
            };
            foreach (var w in weather)
            {
                var parts = w.Split(':');
                sb.AppendLine($"## {parts[0]}");
                sb.AppendLine($"{parts[1]} [{EnvSuffix}]");
                sb.AppendLine($"--negative {GlobalNegative}");
                sb.AppendLine($"Output: {parts[0]}.png\n");
            }

            WritePromptFile("batch_E_factions_weather.txt", sb.ToString());
        }

        private static void WritePromptFile(string filename, string content)
        {
            if (!Directory.Exists(PromptsOutputDir))
                Directory.CreateDirectory(PromptsOutputDir);

            string path = Path.Combine(PromptsOutputDir, filename);
            File.WriteAllText(path, content);
            AssetDatabase.Refresh();
            Debug.Log($"[PromptGen] Written: {path} ({content.Length} chars)");
        }
    }
}
