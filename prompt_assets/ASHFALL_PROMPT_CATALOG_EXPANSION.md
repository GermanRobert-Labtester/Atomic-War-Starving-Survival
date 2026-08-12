# ASHFALL — Prompt Catalog Expansion
### Items · Locations · Survivors · Factions · Weather — everything not yet covered by the existing prompt library

This file is a **continuation**, not a replacement. Two prompt libraries already exist and remain valid:
1. `ASHFALL_Firefly_Item_Icon_Prompts.md` (Desktop) — 321 items, individually prompted. Excellent, keep using it as-is.
2. `ASHFALL_Firefly_Flux_200_Asset_Prompts.pdf` (Desktop) — 200 entries: key art, 5 shelter rooms + 9 bonus shelter variants, 31 named establishing shots, survivor/visitor archetypes, events, UI/map assets, VFX, tiles.

Both predate the game's current data (`items.json`/`locations.json`/`survivors.json` have grown since — expansion work added new catalog entries; see `docs/ai-art/EXISTING_PROMPT_AUDIT.md`). This file fills exactly the gap between what's covered and what exists in the live data today:
- **170 items** (of 419) with no individual prompt yet
- **42 locations** (of 47) with no prompt yet — only `abandoned_hospital`, `rural_gas_station`, `suburban_house`, `government_bunker`, `stranger_cache` are covered
- **All 96 named survivors** (the existing library only has 12 generic profession archetypes, not tied to specific ids)
- **5 factions** — no coverage yet
- **15 weather kinds** beyond the existing 6-icon generic summary sheet

Multi-model routing (FLUX.2, Nano Banana, Recraft, etc.) for any of this is documented in `docs/ai-art/IMAGE_MODEL_PROFILES.md` and `ASSET_TAXONOMY.md` — everything below defaults to **Flux 2 Pro via Adobe Firefly**, matching the established production pipeline in the two files above, so new output stays visually consistent with the ~300+ assets already prompted against it.

---

## GLOBAL STYLE — items (reused verbatim from the 321-item file; apply to every prompt in PART A)
```
post-apocalyptic survival game inventory icon, isolated object centered on
pure flat black (#000000) background, dramatic directional rim lighting from
top-left, volumetric dust particles, desaturated color palette with selective
orange-amber highlights, worn and weathered textures, micro-scratches and
grime detail, photorealistic material rendering, cinematic product shot,
no text, no labels, no shadows outside object, no background elements
```
**Model:** Flux 2 Pro via Firefly. **Negative:** `cartoon, anime, flat icon, logo, watermark, signature, text, label, gradient background, colorful background, bright colors, clean new condition, fantasy, sci-fi laser, alien, UI chrome`
**Aspect ratio:** 1:1 default (4:3 for long/large objects, 3:4 for worn full-body gear — see individual notes).

## GLOBAL STYLE — locations, survivors, factions, weather (reused verbatim from the 200-asset PDF; apply to every prompt in PARTS B–E)
```
Original 2D hand-painted survival-management game art, grounded grim realism,
charcoal pencil underdrawing and dry gouache texture, cold restrained palette
of charcoal, concrete grey, faded blue-grey, rust brown, dirty bone, and rare
muted amber practical light; radiation is a subtle cyan-green contamination
cue only. Nuclear-winter ash, condensation, repair marks, functional
materials. No text, logos, flags, brands, readable labels, fantasy, gore, or
weapon glamour.
```
**Negative:** `text, letters, numbers, watermark, logo, flag, brand, neon cyberpunk, glossy sci-fi, cartoon, anime, photorealism, oversaturated colors, gore, distorted anatomy, duplicated objects`
**Aspect ratio:** 16:9 for locations/events, 3:4 for portraits (matches the existing library's own convention).

> Note the two bibles are deliberately different mediums — items read as stylized studio-photo objects, everything else reads as hand-painted illustration. That split already exists in the production library; it's not an inconsistency to fix.

---

# PART A — Missing Items (170 → brings catalog to 419/419)

### A1. Deprecated scrap ammo — BATCH (19 calibers, one shared prompt)
> Small handful of corroded brass cartridge casings, green-black oxidation crusting the surface, dented and bent from age, tarnished headstamp barely visible, loose pile of 3–5 rounds. `[GLOBAL SUFFIX — items]`

Calibers (swap only the caliber label in the file name, e.g. `icon_ammo_deprecated_9x19.png`): 9×19, .380 ACP, 7.62×25, .45 ACP, 9×21, 7.65×21, 12ga, 16ga, 5.56×45, 7.62×39, 5.45×39, 7.62×51, .300 BLK, 5.7×28, 4.6×30, 7.62×54R, .338 Lapua, .408 CheyTac, .50 BMG.
Ids: `ammo_deprecated_cal_9x19`, `_380acp`, `_762x25`, `_45acp`, `_9x21`, `_765x21`, `_12ga`, `_16ga`, `_556x45`, `_762x39`, `_545x39`, `_762x51`, `_300blk`, `_57x28`, `_46x30`, `_762x54r`, `_338lapua`, `_408cheytac`, `_50bmg`.

### A2. New military dual-attribute rounds — BATCH (16 items, one shared prompt)
> Small cardboard ammunition box, lid half open, rows of clean brass-and-copper cartridges inside, caliber stencil-stamped on the end, one loose round resting beside the box. `[GLOBAL SUFFIX — items]`

Ids (swap caliber/loadout in file name): `ammo_545x39_jhp_ap`, `ammo_545x39_exi`, `ammo_545x39_api`, `ammo_300blk_jhp_ap`, `ammo_57x28_jhp_ap`, `ammo_57x28_exi`, `ammo_57x28_api`, `ammo_762x54r_jhp_ap`, `ammo_762x54r_exi`, `ammo_762x54r_api`, `ammo_338lapua_jhp_ap`, `ammo_408cheytac_jhp_ap`, `ammo_762x51_jhp_ap`, `ammo_762x51_exi`, `ammo_50bmg_jhp_ap`, `ammo_50bmg_exi`.

### A3. Pistols, SMGs, shotguns (22 items) — table, 4:3, generic silhouette (no brand names in-prompt, per established convention)
| id | Prompt |
|---|---|
| `pistol_cz75_9x19` | Compact all-metal service pistol, exposed hammer, worn bluing at the slide edges. `[SUFFIX]` |
| `pistol_beretta_92_9x19` | Full-size open-slide service pistol, worn matte finish, faint holster wear. `[SUFFIX]` |
| `pistol_steyr_m9_9x19` | Polymer-frame service pistol, blocky trapezoidal slide, scuffed grip texture. `[SUFFIX]` |
| `pistol_walther_ppk_380acp` | Small compact pocket pistol, worn matte finish, thin profile. `[SUFFIX]` |
| `pistol_grand_power_p380_380acp` | Compact rotary-barrel pistol, textured polymer grip, light holster wear. `[SUFFIX]` |
| `pistol_cz52_762x25` | Angular military sidearm, distinct roller-lock bulge above the trigger guard. `[SUFFIX]` |
| `pistol_norinco_type54_762x25` | Surplus military sidearm, blocky slide, lanyard loop, worn parkerizing. `[SUFFIX]` |
| `pistol_zastava_m57_762x25` | Elongated military sidearm, extended grip, worn dark finish. `[SUFFIX]` |
| `smg_m1928a1_thompson_45acp` | Heavy vertical-foregrip submachine gun, finned barrel, drum magazine. `[SUFFIX]` |
| `smg_hk_ump45_45acp` | Boxy polymer-framed submachine gun, side-folding stock, scuffed housing. `[SUFFIX]` |
| `smg_kriss_vector_45acp` | Distinctive offset-barrel submachine gun, angled recoil mechanism visible. `[SUFFIX]` |
| `pistol_bt_apc45_mini_45acp` | Stubby pistol-length compact SMG-pattern sidearm, closed bolt housing. `[SUFFIX]` |
| `smg_bt_apc45_45acp` | Compact modern submachine gun, side rail, worn matte coating. `[SUFFIX]` |
| `smg_sites_spectre_m4_9x21` | Submachine gun with bulky quad-stack magazine, blocky receiver. `[SUFFIX]` |
| `smg_imi_micro_uzi_9x21` | Tiny machine-pistol silhouette, boxy receiver, stubby barrel. `[SUFFIX]` |
| `smg_cz_scorpion_evo3_9x21` | Modern compact polymer submachine gun, side-folding stock. `[SUFFIX]` |
| `smg_steyr_solo_s1_100_765x21` | Milled-steel vintage submachine gun, worn wood furniture. `[SUFFIX]` |
| `smg_mp34_765x21` | Long milled-steel submachine gun, finned barrel jacket, worn wood stock. `[SUFFIX]` |
| `shotgun_benelli_m4_super90_12ga` | Tactical semi-auto shotgun, pistol-grip stock, worn matte finish. `[SUFFIX]` |
| `shotgun_remington_model1100_12ga` | Classic wood-stocked semi-auto shotgun, ribbed barrel, worn bluing. `[SUFFIX]` |
| `shotgun_browning_auto5_16ga` | Humpback-receiver vintage shotgun, worn wood furniture. `[SUFFIX]` |
| `shotgun_franchi_al48_16ga` | Slim lightweight alloy-receiver shotgun, worn wood stock. `[SUFFIX]` |

### A4. Rifles, carbines, PDWs, snipers (28 items) — table, 4:3
| id | Prompt |
|---|---|
| `rifle_m4a1_carbine_556x45` | Military carbine, STANAG magazine, worn flat-dark furniture. `[SUFFIX]` |
| `rifle_hk416_556x45` | Gas-piston assault rifle, ribbed handguard, worn matte coating. `[SUFFIX]` |
| `rifle_fn_scar_l_556x45` | Modular assault rifle, folding stock, full-length top rail. `[SUFFIX]` |
| `rifle_steyr_aug_a3_556x45` | Bullpup assault rifle, compact stock-less silhouette, worn housing. `[SUFFIX]` |
| `rifle_ak47_762x39` | Iconic long-stamped-receiver rifle, worn wood furniture, curved magazine. `[SUFFIX]` |
| `rifle_cmmg_mk47_mutant_762x39` | AR-pattern rifle body feeding a curved magazine, worn black finish. `[SUFFIX]` |
| `lmg_rpk74_545x39` | Squad automatic weapon, heavy barrel, folding bipod, drum magazine. `[SUFFIX]` |
| `rifle_ak74u_545x39` | Short-barrel carbine, folding stock, compact worn silhouette. `[SUFFIX]` |
| `rifle_fn_fal_762x51` | Full-power battle rifle, wood-and-steel furniture, worn bluing. `[SUFFIX]` |
| `rifle_hk_g3_762x51` | Roller-delayed battle rifle, ribbed handguard, worn dark finish. `[SUFFIX]` |
| `rifle_q_honey_badger_300blk` | Short-barrel PDW carbine, integrated suppressor housing. `[SUFFIX]` |
| `rifle_sig_mcx_rattler_300blk` | Ultra-short-barrel tactical carbine, folding brace. `[SUFFIX]` |
| `rifle_ddm4_pdw_300blk` | Compact AR-platform PDW, short handguard, worn black finish. `[SUFFIX]` |
| `pdw_fn_p90_57x28` | Bullpup PDW, translucent top-mounted magazine, compact housing. `[SUFFIX]` |
| `carbine_ruger_lc_57x28` | Lightweight bolt-over-barrel carbine, slim profile. `[SUFFIX]` |
| `pdw_hk_mp7a2_46x30` | Compact PDW, folding foregrip, worn matte housing. `[SUFFIX]` |
| `pdw_cmmg_four6_46x30` | AR-platform personal defense weapon, short handguard. `[SUFFIX]` |
| `pdw_tb_tactical_t7_46x30` | Modernized compact PDW, angular polymer housing. `[SUFFIX]` |
| `sniper_mosin_nagant_m9031_762x54r` | Bolt-action surplus rifle, worn wood stock, long barrel. `[SUFFIX]` |
| `sniper_svd_dragunov_762x54r` | Semi-automatic marksman rifle, skeletonized wood stock, scope rail. `[SUFFIX]` |
| `sniper_romak3_psl_762x54r` | Long-stroke piston marksman rifle, worn wood-and-steel furniture. `[SUFFIX]` |
| `sniper_steyr_ssg08_338lapua` | Precision bolt-action rifle, adjustable stock, worn matte finish. `[SUFFIX]` |
| `sniper_sako_trg42_338lapua` | Long-range bolt-action rifle, heavy fluted barrel. `[SUFFIX]` |
| `sniper_dsr1_338lapua` | Bullpup precision rifle, compact worn housing, long barrel forward. `[SUFFIX]` |
| `sniper_cheytac_m200_intervention_408cheytac` | Extreme-long-range anti-materiel rifle, massive worn stock. `[SUFFIX]` |
| `sniper_voere_mk_x3_408cheytac` | Take-down modular sniper chassis, worn matte housing. `[SUFFIX]` |
| `sniper_voere_mk_x4_408cheytac` | Heavy anti-materiel precision platform, thick fluted barrel. `[SUFFIX]` |
| `sniper_barrett_m82a1_50bmg` | Massive semi-automatic anti-materiel rifle, muzzle brake, bipod. `[SUFFIX]` |

### A5. Grenade, misc (1 item)
`item_emp_grenade` — Cylindrical grenade with a stubby antenna coil and a single toggle switch, matte grey casing, worn safety pin. `[SUFFIX — items]`

### A6. Container fill-level variants — BATCH (one prompt per container type; fill amount doesn't need separate art)
| Container | Prompt | Ids covered |
|---|---|---|
| Water bottle | Scratched clear plastic bottle, screw cap, condensation beads, visible water fill line. | `water_bottle_1l_of_2l`, `_0_5l_of_1l`, `_0_5l_of_2l`, `_1_5l_of_2l` |
| Fuel can | Dented metal fuel can, faded red paint, wire-secured cap, dark fuel stains. | `fuel_0_5l_of_1l`, `accelerant_half` |
| E-liquid bottle | Small plastic vape bottle, childproof cap, nicotine strength scratched into a makeshift label. | `ejuice_10ml_10mg`, `ejuice_10ml_20mg`, `ejuice_20ml_35mg` |
| Water purification tablets | Amber pill bottle, tablets visible through translucent plastic, worn cap; empty variant shows loose rattle and no tablets. | `water_purification_tablets_40_of_40`, `_20_of_40`, `_0_of_40` |

Individually distinct (not fill-variants, still batch-adjacent, 1:1): `iodine_pills_bottle_10_of_10` — small amber pill bottle, half the tablets visible, worn cap. `jetfuel_jerrycan_10l_of_10l` — large NATO-style jerrycan, stenciled markings worn, chained spout cap. `instant_coffee_10x_container` — small dented tin canister of dark granules, worn label. `ice_tea_0_5l_package` — collapsed foil drink pouch, printed graphics faded past reading. `package_rolled_oats_1kg_of_1kg` — paper sack, top folded and clipped, flour dust on the seams. `dry_rice_1kg_of_1kg` — clear plastic bag of rice, twist-tied top. `dried_pasta_2kg_of_2kg` — clear bag of dried pasta shapes, folded cardboard header worn blank. `soy_and_rice_milk_1l_of_1l` — carton with a fold-top spout, faded print, slight dent.

### A7. Remaining Device / Medical / Protective / Tool / Quest / Relic / Comfort / AntiRad / Filter / Material singles (~65 items) — table, 1:1 unless noted
| id | Prompt |
|---|---|
| `anti_rad` | Small amber medicine vial, dropper cap, radiation trefoil scratched off the label, half empty. |
| `prewar_letter` | Sealed cream envelope, edges browned and brittle, foreign stamp faded past reading, held shut by a paperclip. |
| `item_bioluminescent_moss` | Small clump of pale blue-green moss in a glass jar, faint self-lit glow, clinging to a stone fragment. |
| `hand_crank_radio` | Boxy portable radio, fold-out crank handle, scuffed housing, dial marked in worn grease pencil. |
| `item_uv_lamp_ballast` | Heavy rectangular magnetic ballast, coiled wire visible through vents, scorch mark at one corner. |
| `item_geothermal_valve` | Brass high-pressure valve wheel and stem, condensation beads, steam-stained patina. |
| `item_ro_membrane` | Cylindrical white filter membrane cartridge, mineral-crusted housing, worn pressure gauge on the cap. |
| `item_acoustic_decoy` | Small wind-up metronome wired to a tiny speaker horn, exposed wiring, key protruding from the side. |
| `item_logic_board` | Green circuit board studded with components, several scorched black, wiring dangling from one edge. |
| `item_co2_scrubber_cartridge` | Cylindrical white cartridge, perforated end caps, chemical-burn discoloration. |
| `item_rebreather_scrubber` | Compact canister, screw-thread collar, mineral crust around the seams. |
| `water_purification_tablets_40_of_40` / `_20_of_40` / `_0_of_40` | See A6 container batch. |
| `antiseptic_1l_of_1l` | Tall brown glass bottle, dropper cap, hand-written label. |
| `alcohol_wipes_box_10_of_10` | Small cardboard box of foil wipe packets, crushed corner. |
| `epi_pen` | Single yellow auto-injector pen, safety cap on, worn grip texture. |
| `decontamination_soap_5_of_5` | Bar of grey industrial soap in torn paper wrap. |
| `item_frostbite_salve` | Small tin of pale ointment, fingerprint smear on the lid, hand-written frost warning. |
| `item_scopolamine_root` | Dried twisted root fragments in a cloth pouch, dark and papery. |
| `item_lithium_salts` | Small glass jar of coarse white-grey crystals, cork stopper. |
| `item_amnestic_syrup` | Dark brown syrup in a stoppered glass vial, faint sediment at the base. |
| `item_snow_goggles_improvised` | Carved wood eye-guard, narrow horizontal slits, leather strap, soot-blackened interior. (3:4) |
| `item_lead_visor` | Heavy dark-tinted visor in a lead-mesh frame, thick strap, scratched lens. (3:4) |
| `item_ash_ghillie` | Loose net suit woven with pale ash-grey strips, frayed fabric, attached hood. (3:4) |
| `item_black_ice_sample` | Chunk of murky frozen water in a sealed sample jar, faint dark mineral streaks inside. |
| `item_cobalt_salt_canister` | Squat lead-lined canister, stenciled serial number worn but legible, heavy rounded cap. |
| `item_black_water_vial` | Small glass vial of iridescent black liquid, stopper sealed with wax. |
| `item_submerged_server` | Rectangular sealed server blade, corroded connectors, faint waterline staining. |
| `item_master_override` | Large ornate brass key with an encrypted digital chip embedded in the bow. |
| `item_hard_drive_platter` | Single mirror-polished magnetic disk in a static-proof sleeve, faint scratches across the surface. |
| `item_pre_war_photo_album` | Thick leather-bound album, water-stained pages, one photo corner peeking out. |
| `item_vinyl_collection` | Stack of worn vinyl records in torn paper sleeves, faded cover art. |
| `att_mil_double_scope_5x_10x` | Compact rifle scope, flip-magnification ring, scuffed matte housing, worn turret caps. |
| `sewing_kit_10_of_10` | Small tin of needles, thread spools, and pins, lid slightly bent. |
| `item_hand_crank_sled` | Low wooden sled with a hand-crank tow spool, frayed rope, iced runners. (4:3) |
| `item_geiger_tether` | Large spool of thin copper wire attached to a small sensor puck. |
| `item_pneumatic_jack` | Heavy compressed-air jack, thick hose, pressure gauge, oil-streaked. |
| `item_fungicide_fogger` | Pressurized canister with a fogging nozzle, blue-green residue crusted around the valve. |
| `item_mine_prod` | Long fiberglass rod, blunt steel tip, hand-worn grip tape. |
| `item_headphones_mil` | Padded military headset, boom mic, scuffed matte housing. |
| `item_epoxy_injector` | Caulk-gun-style resin injector, long nozzle, dried resin drips on the barrel. |
| `item_tether_harness` | Heavy canvas chest harness, steel D-rings, coiled steel cable. |
| `explosive_powder_nitroglycerin` | Small waxed-paper packet of pale unstable powder, hand-folded shut, warning symbol scratched on top. |
| `salvaged_tech_trash` | Tangle of broken circuit boards and wire fragments in a torn cloth sack. |
| `rope_2m_of_2m` | Coiled length of frayed natural-fiber rope, one end unraveling. |
| `copper_wire_10m_of_10m` | Tight spool of bare copper wire, oxidized green at the exposed ends. |
| `oat_flour` | Small cloth sack of fine pale flour, drawstring top, flour dust on the seams. |
| `plastic_contamination_bag_box_5` | Small stack of folded yellow hazmat bags with a printed trefoil, crushed box corner. |
| `item_cryo_coolant` | Pressurized steel cylinder, frost-rimed valve, hazard tag wired to the handle. |
| `item_thermal_paste` | Squat metal tube of grey compound, crusted cap, label worn blank. |
| `item_shoring_timber` | Short creosote-dark wooden beam, cracked grain, metal end-cap. (4:3) |
| `item_mycelium_bricks` | Stacked grey-brown compressed bricks, faint fungal texture, dusted with ash. |
| `item_faraday_mesh` | Folded sheet of fine woven copper mesh, frayed edges, faint sheen. |
| `item_sound_baffling` | Wedge-cut foam acoustic panel, charcoal grey, pitted surface. |
| `item_tungsten_core` | Small dense dark-grey metal cylinder, heavy for its size, machined grooves. |
| `item_pneumatic_hose` | Coiled reinforced rubber hose, brass fittings, one end cracked. |
| `item_galvanized_rebar` | Short bent length of ridged zinc-coated steel bar. |
| `item_welders_glass` | Small dark rectangular glass pane in a scratched frame. |
| `item_mirror_shard` | Jagged triangular shard of silvered glass, edge wrapped in cloth tape. |
| `item_bio_plastic` | Curved off-white plastic sheet fragment, faint organic grain texture. |
| `rubber_gasket` | Black rubber ring seal, cracked on one side, still pliable. |
| `concrete_patch_mix` | Paper sack of grey powder mix, top folded, dust residue on the seams. |
| `insulation_tape` | Half-used roll of black electrical tape, frayed edge. |
| `engine_block_intact` | Large corroded cast-iron engine block, missing accessories, oil-stained. (4:3) |
| `bearing_set_industrial` | Small greased-paper packet of steel bearings, a few visible through a torn corner. |
| `copper_tubing_1m` | Coiled length of dull copper pipe, dented in two places. |

All rows above append `[GLOBAL SUFFIX — items]` from the top of this document.

---

# PART B — Missing Locations (42 of 47; brings location coverage to 47/47)

Only `abandoned_hospital`, `rural_gas_station`, `suburban_house`, `government_bunker`, and `stranger_cache` are covered by the existing 200-asset PDF. The other 42 real locations in `locations.json` post-date it entirely (added during expansion work) and need establishing shots. 16:9, `[GLOBAL SUFFIX — environments]` on every entry below.

1. **Geo-Thermal Plant Ruins** (`location_geo_thermal_plant_ruins`). Cracked geothermal plant ruin, corroded pipes venting thin steam across boiling mud flats, blank warning signage rusted through, ground fractured in wide unstable plates.
2. **Arcology Sector 4** (`location_arcology_sector_4`). Massive corroded blast doors sealed behind velvet-draped ruins, opulent pre-war furnishings decaying under dust, barricades built from broken luxury fixtures.
3. **Frozen River Barge** (`location_frozen_river_barge`). Rusted cargo barge locked in river ice, cargo hatches chained shut, gutted-fish bones and a makeshift dockworker camp along the frozen deck.
4. **Crashed Icebreaker Convoy** (`location_crashed_icebreaker_convoy`). Derailed armored train cars scattered across snow, one cracked radioactive generator glowing faint cyan through a torn hull.
5. **The Silent Observatory** (`location_silent_observatory`). Isolated mountaintop observatory dome, frost-rimed telescope housing against an impossibly clear dark sky, thin cold light over a curved ash-grey horizon.
6. **Subterranean Seed Vault** (`location_subterranean_seed_vault`). Deep frozen vault corridor lined with sealed seed-storage drawers, frost coating every surface, tripwires strung low across the floor.
7. **Ministry of Truth Bunker** (`location_ministry_of_truth_bunker`). Windowless bunker corridor of humming server racks under emergency lighting, tangled cable runs, one active screen glowing in the dark.
8. **Ash Dune Cemetery** (`location_ash_dune_cemetery`). Wide open ash dunes with human shapes half-buried and preserved beneath drifted grey powder, wind carving slow ripples across the field.
9. **Abandoned Ski Resort** (`location_abandoned_ski_resort`). Frozen ski lodge, cable cars hanging mid-span over a snow-buried slope, furs and luxury debris scattered across a frost-glazed lobby.
10. **Geothermal Borehole Site** (`location_geothermal_borehole_site`). Towering drill rig over a black borehole shaft, toxic groundwater pooling around corroded equipment, steam rising into cold air.
11. **Flooded Subway Depot** (`location_flooded_subway_depot`). Flooded subway platform in near-total darkness, waist-deep water reflecting a single flashlight beam, rusted carriages half-submerged.
12. **Sub-Level 4 Transit Hub** (`location_sub_level_4_transit`). Sealed transit tunnel walls thickly overgrown with faint glowing blue-green fungal growth, pneumatic doors frozen half-open.
13. **Municipal Sewage Treatment** (`location_municipal_sewage`). Cramped sewage-treatment corridor of groaning pressurized pipes and settling tanks, thick sludge on the catwalks, gauges pinned into the red.
14. **Collapsed Salt Mine** (`location_collapsed_salt_mine`). Vast salt-crystal cavern, collapsed support beams and scattered shoring timber, pale crystalline walls catching lantern light.
15. **Bio-Remediation Lab** (`location_bio_remediation_lab`). Sealed lab corridor thick with pale spore haze, biohazard suits abandoned mid-collapse, fungal growth consuming equipment and floor alike.
16. **Submerged Data Center** (`location_submerged_data_center`). Flooded server-farm aisle, submerged racks glowing faint standby lights beneath still black water, raised floor panels lifted and scattered.
17. **Geothermal Vent Shaft** (`location_geothermal_vent_shaft`). Narrow vent shaft venting sulfurous steam through cracked rock, boiling mud glowing faint orange, valves crusted with mineral buildup.
18. **The Sump Cathedral** (`location_the_sump_cathedral`). Vast underground cistern lit entirely by glowing blue-green moss coating every wall, still black water reflecting the light, shrine offerings along the ledge.
19. **Abandoned Desalination Plant** (`location_abandoned_desalination`). Industrial desalination hall of rusted membrane cylinders and burst high-pressure hoses, fortified checkpoint barricades from salvaged pipe.
20. **Deep Core Borehole** (`location_deep_core_borehole`) — endgame. Immense vertical borehole shaft descending into darkness lit only by distant heat-glow far below, heavy machinery frozen mid-operation at the rim.
21. **UXO Highway Choke** (`location_uxo_highway_choke`). Wide abandoned highway densely littered with half-buried unexploded ordnance between rusted cars, painted hazard markers threading a narrow safe path.
22. **Radar Array Spire** (`location_radar_array_spire`). Towering radar dish frozen mid-rotation on a windswept ridge, metal groaning under ice load, shattered control-shack windows below.
23. **Drone Hive Silo** (`location_drone_hive_silo`). Open missile silo interior swarming with small hovering munitions clustered along the walls, launch bay doors rusted half open.
24. **Automated Mortar Pit** (`location_automated_mortar_pit`). Concrete mortar bunker with an automated turret mechanism aimed at the sky, stacked shell casings, scorch-streaked firing ports.
25. **Wire-Head Camp** (`location_scrap_neuromancer_camp`). Camp built inside the rusted fuselage of a downed cargo plane, faraday-mesh curtains and salvaged circuit boards strung along improvised stalls.
26. **Magnetic Crater** (`location_magnetic_anomaly_crater`). Wide impact crater, scattered metal debris hovering faintly off the ground, compass needles visibly spinning, a bright core glinting at the center.
27. **Abandoned Convoy Yard** (`location_abandoned_convoy_yard`). Rows of rusted transport trucks under snow, a turret-mounted sentry gun tracking slowly atop a watchtower, engine parts scattered in mud.
28. **Acoustic Test Facility** (`location_acoustic_testing_facility`). Padded anechoic chamber lined with foam wedge panels absorbing all sound, one figure's footprints the only disturbance in the dust.
29. **Substation Omega** (`location_substation_omega`). Electrical substation yard of towering transformers, faint blue arcs sparking between damaged capacitor banks, frost on every metal surface.
30. **The Dead Hand Core** (`location_the_dead_hand_core`) — endgame. Deep bunker command room dominated by a humming server core under red emergency light, one blinking override terminal at its base.
31. **Lethe Water Treatment** (`location_lethe_water_treatment`). Concealed water-treatment sub-level lined with sealed chemical vats and dripping overhead pipes, faint sweet chemical haze in the air.
32. **Shattered Observatory** (`location_observatory_dome`). Shattered observatory dome open to a harsh bright sky, telescope housing scorched pale, every surface bleached by unfiltered light.
33. **Submerged Luxury Arcology** (`location_submerged_arcology`). Flooded luxury bunker atrium, velvet furnishings rotting waterlogged beneath still dark water, gold fixtures dulled with grime.
34. **Concrete Batching Plant** (`location_concrete_batching_plant`). Industrial concrete plant, towering rusted mixing silos leaning at odd angles, rebar exposed through crumbling columns.
35. **Seed Vault Antechamber** (`location_seed_vault_antechamber`). Antechamber of a seed vault, outer blast door torn open and frost-choked, inner sealed door intact and untouched beyond it.
36. **Hospital Psychiatric Wing** (`location_hospital_psych_wing`). Abandoned psychiatric ward corridor, restraint straps hanging from empty beds, scratch marks along the walls, an overturned medication cart.
37. **Mirror Manufacturing Plant** (`location_mirror_factory`). Mirror-factory floor thick with shattered silvered glass reflecting fractured light in every direction, conveyor lines frozen mid-process.
38. **Radio Telescope Array** (`location_radio_telescope_array`). Field of massive radio telescope dishes tilted skyward, ice-crusted and groaning under wind, a dark control building below.
39. **Ash-Whale Carcass** (`location_ash_whale_carcass`). Enormous fossilized root-mass breaching the ash like a beached whale skeleton, hollow chambers within scavenged bare.
40. **The Memory Vault** (`location_the_memory_vault`) — endgame. Vast server-farm vault of endless dark racks under dim standby lights, dust thick on every unpowered unit, one active bank humming alone.
41. **Highway Pileup** (`highway_pileup`). Miles-long highway pileup of rusted fused vehicles under snow, one engine bay pried open and still faintly warm.
42. **Pre-War Medical Cache** (`prewar_medical_cache`). Sealed clinic basement storeroom, shelves of intact medical supplies behind a rusted grate, a fresh cult sigil scratched near the entrance.

---

# PART C — All 96 Named Survivors (individual portraits)

The existing library only has 12 *generic* profession-archetype portraits, not tied to specific survivor ids. All 96 real survivors below need a portrait. 3:4, chest-up three-quarter view, restrained expression (matches the existing "survivor and visitor portraits" convention), `[GLOBAL SUFFIX — environments/characters]`.

`elena_vasquez` has a full multi-model Master Asset Brief already in `docs/ai-art/prompts/pilot_batch.md` — use that instead of the row below.

| id | Profession | Visual delta |
|---|---|---|
| `elena_vasquez` | Paramedic | *(see pilot_batch.md)* |
| `marcus_olejnik` | Mechanical Engineer | Broad-shouldered older engineer, grease-stained hands, listening to a dead pipe. |
| `suki_tanaka` | Farmer | Weathered farmer kneeling at a grow bed, soil-stained fingers testing the roots. |
| `the_surgeon` | Surgeon | Gaunt surgeon staring at their own hands, exhausted, sterile case at their side. |
| `the_pharmacist` | Pharmacist | Pharmacist with pockets full of empty amber bottles, cautious alert eyes. |
| `the_vet` | Veterinarian | Veterinarian crouched beside a faintly glowing stray dog, gentle guarded hands. |
| `the_therapist` | Therapist | Therapist seated forward in a quiet corner, listening posture, notebook closed in lap. |
| `the_undertaker` | Undertaker | Undertaker with a shovel over one shoulder, dark coat, quiet resolve. |
| `the_veteran` | Soldier | Grizzled soldier listening hard to a radio handset, old unit patch faded blank. |
| `the_cop` | Police Officer | Officer with a badge under a worn coat, hand resting on an old lockbox. |
| `the_bouncer` | Bouncer | Heavyset figure standing alone at the hatch, arms crossed, watchful. |
| `the_hunter` | Hunter | Hunter with a drawn bow, tracking something pale out in the snow. |
| `the_prisoner` | Convict | Gaunt figure in prison-issue clothing, staring at a ring of old keys. |
| `the_plumber` | Plumber | Plumber with a pipe wrench, ear pressed to a groaning wall main. |
| `the_electrician` | Electrician | Electrician in rubber gloves, tracing a dead wire through an open wall panel. |
| `the_architect` | Architect | Architect hunched over unrolled blueprints in dim light, paper curling with damp. |
| `the_mechanic` | Mechanic | Mechanic beside a half-stripped engine, oil-blackened hands, focused. |
| `the_chemist` | Chemist | Chemist's gloved hand on a hissing valve, gas mask held ready. |
| `the_botanist` | Botanist | Botanist tending fragile seedlings under a cracked grow lamp. |
| `the_courier` | Courier | Lean courier with a strapped pack and map case, alert posture. |
| `the_burglar` | Burglar | Wiry figure with lockpicks, studying a vault door in low light. |
| `the_meteorologist` | Meteorologist | Figure atop a windswept rooftop station, reading an iced instrument dial. |
| `the_hazmat_tech` | Hazmat Technician | Hazmat-suited figure with a sealed case, visor fogged at the edges. |
| `the_teacher` | Teacher | Teacher holding a worn ledger of names, chalk dust on one sleeve. |
| `the_politician` | Politician | Composed figure addressing an unseen crowd, hand raised, coat neat despite the wear. |
| `the_priest` | Priest | Clergy figure kneeling beside someone in crisis, hand extended, calm face. |
| `the_reporter` | Reporter | Reporter with a battered notebook and pencil, guarded intent expression. |
| `the_radio_host` | Radio Host | Broadcaster hunched over a static-lit console, oversized headphones, sleepless eyes. |
| `the_chef` | Chef | Chef plating the last of the good food, apron stained, deliberate care. |
| `the_athlete` | Athlete | Lean runner mid-stride on a frozen road, breath visible, determined. |
| `the_firefighter` | Firefighter | Firefighter bare-handed near an open flame, no suit, resolute stance. |
| `the_tailor` | Tailor | Tailor stitching scavenged hides into armor, pins in mouth, focused hands. |
| `the_watchmaker` | Watchmaker | Watchmaker with a loupe over a disassembled heirloom clock, tiny gears scattered. |
| `the_historian` | Historian | Historian clutching a bound document from a burning archive, soot on one sleeve. |
| `the_defector` | Cult Defector | Gaunt figure with a scratched-off cult sigil on their coat, wary glance back. |
| `the_addict` | Addict | Figure with empty trembling hands held deliberately still, eyes fixed forward. |
| `the_parent` | Parent | Parent standing frozen beside a silent radio, coat clutched tight. |
| `the_fierce_mother` | Mother | Mother pushing her own ration bowl toward a child's hands. |
| `the_exhausted_father` | Father | Father slumped at a workbench mid-task, tools still in hand. |
| `the_naive_son` | Child | Small boy playing quietly among crates, oblivious half-smile. |
| `the_hardened_daughter` | Child | Girl standing rigid, arms crossed, refusing a comforting hand. |
| `the_psychopath` | Outsider | Unreadable figure keeping deliberate distance from others, flat expression. |
| `the_serial_killer` | Neighbor | Smiling, affable figure, eyes that don't match the smile. |
| `the_liar` | Storyteller | Animated figure mid-story, listeners' doubtful faces half in shadow. |
| `the_hoarder` | Collector | Figure hunched protectively over an overstuffed personal storage shelf. |
| `the_general` | Military | Older officer with a torn rank insignia, tracing a hand-drawn map. |
| `the_saboteur` | Rebel | Figure crouched at a rigged trap mechanism, careful precise hands. |
| `the_deserter` | Sniper | Flinching figure near a generator, half-turned away from the noise. |
| `the_quartermaster` | Logistics | Meticulous figure counting stacked supplies against a tally sheet. |
| `the_child_soldier` | Child | Child with a rifle slung too large for their frame, hollow eyes. |
| `the_empath` | Counselor | Figure kneeling at eye level with someone else, hand extended, tired posture. |
| `the_misanthrope` | Hermit | Solitary figure at the edge of a group, half-turned toward the exit. |
| `the_pollyanna` | Optimist | Figure smiling upward into falling ash, unbothered posture. |
| `the_martyr` | Caregiver | Figure pushing their own plate toward someone else without a word. |
| `the_arrogant_surgeon` | Surgeon | Surgeon with crossed arms and a cold appraising stare, spotless coat. |
| `the_relapsing_addict` | Addict | Figure with visibly shaking hands near a locked medicine cabinet. |
| `the_insomniac` | Night Watch | Hollow-eyed figure alone at the hatch through the night, pacing. |
| `the_hypochondriac` | Patient | Figure anxiously inspecting their own skin under a lamp. |
| `the_pyromaniac` | Arsonist | Figure staring too intently into an open heater flame. |
| `the_blind_preacher` | Preacher | Clouded-eyed figure speaking with hands raised, listeners' faces attentive. |
| `the_prepper` | Prepper | Wary figure surrounded by neatly hoarded personal supplies, guarding them. |
| `the_outcast` | Mutant | Gaunt isolated figure eating alone, subtle unnatural pallor. |
| `the_feral_orphan` | Child | Feral child crouched low on bare concrete, wary animal stillness. |
| `the_pacifist` | Monk | Serene unarmed figure with palms open, calm amid visible danger. |
| `the_widow` | Botanist | Grieving figure tending plants late at night, one photograph propped nearby. |
| `the_ex_con` | Laborer | Hard-eyed figure dragging a heavy load alone, ignoring outstretched hands. |
| `the_sheriff` | Lawman | Steady figure standing guard duty alone at the hatch, tired but upright. |
| `the_former_politician` | Politician | Figure practicing a speech to an empty room, forced charisma. |
| `the_tech_bro` | Engineer | Figure hunched over a dead tablet, tools scattered, stubborn optimism. |
| `the_news_anchor` | Anchor | Poised figure at a battered desk, journal open, composed despite the wear. |
| `the_nomad` | Scavenger | Restless figure standing at an open hatch, pack already on. |
| `the_exec` | Executive | Sharply dressed figure directing others, counting trade goods. |
| `survivor_cryo_tech` | HVAC Specialist | Figure adjusting a large duct valve, frost on the gloves. |
| `survivor_bunker_archivist` | Bureaucrat | Figure poring over old blueprints, spectacles fogged, meticulous. |
| `survivor_ice_walker` | Nomad | Silent figure listening to wind over black ice, still and alert. |
| `survivor_defrosted_aristocrat` | Politician | Figure in a tattered silk suit, disoriented amid concrete surroundings. |
| `survivor_thawed_medic` | Cryo-Medic | Disoriented medic staring at unfamiliar equipment, fragile composure. |
| `survivor_ash_diver` | Scavenger | Figure emerging from an ash-drift, breathing labored, scrap in hand. |
| `survivor_speleologist` | Cave Mapper | Figure with a headlamp studying rock strata, calm in the dark. |
| `survivor_sump_diver` | Scuba Scavenger | Pale damp-skinned figure surfacing from black water, copper scrap in hand. |
| `survivor_mycologist` | Fungus Farmer | Figure with blue-green-stained hands examining a cluster of fungus. |
| `survivor_hydro_engineer` | Pump Specialist | Figure with an ear to a pipe, hand on a pressure valve. |
| `survivor_mole` | Tunnel Rat | Small wiry figure squeezing through a narrow duct opening. |
| `survivor_rot_farmer` | Composter | Figure turning a compost heap bare-handed, unbothered expression. |
| `survivor_sapper` | EOD Tech | Figure kneeling motionless beside live ordnance, deliberate slow hands. |
| `survivor_lineman` | Telecomm Tech | Figure stripping copper wire from a wall conduit, careful focus. |
| `survivor_drone_op` | UAV Pilot | Figure flying a duct-taped quadcopter with a mirror rig, screen glow on face. |
| `survivor_acoustic_tech` | Sonar Operator | Figure with one ear turned toward the ground, listening intently. |
| `survivor_machinist` | CNC Operator | Figure at a hand lathe, calipers in hand, distrustful of nearby electronics. |
| `survivor_logistician` | Supply Chain Mgr | Figure meticulously counting ammunition into a labeled crate. |
| `survivor_bunker_born` | Scavenger/Scrap | Pale figure who flinches at open sky, comfortable only in tight dark spaces. |
| `survivor_the_waking` | Patient | Disoriented figure staring at scorch-shadows on a wall, flinching at a heater click. |
| `survivor_osteophage` | Scrapper | Gaunt figure chewing on a length of copper wire, faint tremor in the hands. |
| `survivor_archivist` | Monk/Historian | Robed figure guarding a shelf of old vinyl records, reverent posture. |
| `survivor_concrete_boss` | Foreman | Figure with an ear against a cracking wall, listening for structural failure. |
| `survivor_uv_penitent` | Cultist | Figure with burn-scarred skin turned deliberately toward a light source. |

---

# PART D — Factions (5, no coverage yet)

Establishing lineup format: 3–4 figures per faction, 16:9, `[GLOBAL SUFFIX — environments/characters]`. Individual named NPCs (`NPC_AshWidows`, `NPC_TheTollman`, `NPC_BurnedPatrol`, `NPC_TheCollector`, `NPC_FeralChildren`, `NPC_SurgeonsCaravan`, `NPC_Bandits` — see `ASSET_TAXONOMY.md`) should reuse whichever faction's visual DNA they belong to once that faction's lineup is generated and approved as an anchor.

**Central Garrison Remnants.** Disciplined survivors of the regional military garrison, faded olive fatigues and mismatched surplus body armor, insignia deliberately scratched off, weapons maintained meticulously despite the wear — composition reads chain-of-command even in ruin.

**Upland Provincial Militia.** Agrarian militia in layered hunting gear and hand-sewn patches, farm tools converted to weapons, faces weathered by fieldwork rather than combat — practical, unpolished, locally-made everything.

**Cultists of the Glow.** Robed figures in scavenged fabric, dyed and patched, symbols painted in rust-red across chest and hood, radiation-warning trefoils repurposed as religious icons, unsettling calm in high-radiation postures.

**Scavenger Warlords.** Raiders in mismatched scavenged armor bolted and welded together, improvised bladed weapons, aggressive asymmetric silhouettes built from other factions' salvaged gear.

**Safe Haven Communities.** Civilian survivors in practical layered clothing, no visible weapons, deliberately unthreatening posture and soft silhouette, communal/domestic details (aprons, tool belts, a child's toy) visible.

---

# PART E — Weather (15 kinds beyond the existing 6-icon generic summary sheet)

`Clear`/`Rain`/`Overcast`/`Ashfall`/`FalloutStorm`/`Blizzard`/`BlackRain` already have some coverage (key art panoramas + the existing weather-icon sheet). These 15 don't. 16:9 atmospheric shot or transparent overlay as noted, `[GLOBAL SUFFIX — environments]`.

| Weather kind | Prompt |
|---|---|
| `AcidSnow` | Corrosive pale-yellow snowfall pitting exposed metal surfaces, faint chemical haze low to the ground. |
| `BioFog` | Dense grey-green spore fog rolling low across ruins, visibility reduced to a few meters. |
| `BlackSnow` | Heavy soot-black snowfall coating every surface, footprints the only clean ground visible. |
| `BloodRain` | Thin rust-red rain streaking down concrete and glass, puddles tinted dark red. |
| `EMPStorm` | Violet-white static arcing across a clouded sky, streetlights and electronics flickering dead below. |
| `GlassStorm` | Fine glittering dust storm with a harsh glassy sheen, sky catching light unnaturally. |
| `RadHail` | Heavy grey hailstones pitting the ground, faint cyan-green residue where they land. |
| `AlgaeBloom` | Thick toxic blue-green film coating standing water, dead reeds at the waterline. |
| `AshLightning` | Jagged white static discharge forking through a dense ash cloud, brief harsh illumination. |
| `ParticulateFog` | Suspended radioactive haze reducing visibility to arm's length, faint cyan shimmer in the moisture. |
| `ThermalInversion` | Flat trapped grey haze under a warm ceiling layer, unnaturally still air. |
| `IceStorm` | Freezing rain sheeting every surface in clear ice, a hatch wheel frozen mid-turn. |
| `Silence` | Unnervingly clear still sky, no wind, no ash, no birds — wrongness in the stillness itself. |
| `FalseSpring` | Thin broken ash-cloud cover with harsh unfiltered light breaking through, deceptively bright. |
| `SilentSpring` | Cloudless sky with a searing pale light, heat-shimmer over frozen ground, no wind at all. |

---

## How to add a new entry as expansion work continues

This file is meant to be hand-edited. When new items/locations/survivors land in `items.json`/`locations.json`/`survivors.json`:

1. **Item:** pick the closest category table in Part A, add a row: `id | short visual prompt built from the item's real description field`. Append `[GLOBAL SUFFIX — items]` implicitly (stated once at the top, not per row).
2. **Location:** add a numbered entry to Part B in the same format — bold name, id, one sentence translating the location's `description` field into *visible* composition (what you'd actually see, not the backstory).
3. **Survivor:** add a row to Part C's table — id, profession, one concrete visual beat pulled from the bio's most filmable image.
4. **Faction/weather:** rare additions — follow the Part D/E format directly.

After editing, update the counts in `docs/ai-art/ASSET_MANIFEST.md` and re-sync this file to the game-root and Desktop copies (see that file's header for the sync locations).
