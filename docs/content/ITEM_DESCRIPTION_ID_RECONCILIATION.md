# Item Description ID Reconciliation Register

Authoritative audit of all 184 entries in `item_description_texts.json` against canonical item catalogs.

| # | description_id_before | canonical_id | status | evidence | action | notes |
|---|-----------------------|--------------|--------|----------|--------|-------|
| 1 | `dosimeter` | `dosimeter` | EXACT_MATCH | items.json (Dosimeter) | DIRECT_MAP | Active canonical item (Device) |
| 2 | `geiger_counter` | `geiger_counter` | EXACT_MATCH | items.json (Geiger Counter) | DIRECT_MAP | Active canonical item (Device) |
| 3 | `iodine_pills` | `iodine_pills` | EXACT_MATCH | items.json (Iodine Pills) | DIRECT_MAP | Active canonical item (Iodine) |
| 4 | `anti_rad` | `anti_rad` | EXACT_MATCH | items.json (Anti-Rad) | DIRECT_MAP | Active canonical item (AntiRad) |
| 5 | `gas_mask` | `gas_mask` | EXACT_MATCH | items.json (Gas Mask) | DIRECT_MAP | Active canonical item (Protective) |
| 6 | `hazmat_suit` | `hazmat_suit` | EXACT_MATCH | items.json (Hazmat Suit) | DIRECT_MAP | Active canonical item (Protective) |
| 7 | `water_filter` | `water_filter` | EXACT_MATCH | items.json (Water Filter) | DIRECT_MAP | Active canonical item (Filter) |
| 8 | `air_filter` | `air_filter` | EXACT_MATCH | items.json (Air Filter) | DIRECT_MAP | Active canonical item (Filter) |
| 9 | `clean_water` | `clean_water` | EXACT_MATCH | items.json (Clean Water) | DIRECT_MAP | Active canonical item (Water) |
| 10 | `irradiated_water` | `irradiated_water` | EXACT_MATCH | items.json (Irradiated Water) | DIRECT_MAP | Active canonical item (IrradiatedWater) |
| 11 | `canned_food` | `canned_food` | EXACT_MATCH | items.json (Canned Food) | DIRECT_MAP | Active canonical item (Food) |
| 12 | `bandage` | `bandage` | EXACT_MATCH | items.json (Bandage) | DIRECT_MAP | Active canonical item (Medical) |
| 13 | `medical_kit` | `medical_kit` | EXACT_MATCH | items.json (Medical Kit) | DIRECT_MAP | Active canonical item (Medical) |
| 14 | `fuel` | `fuel` | EXACT_MATCH | items.json (Fuel) | DIRECT_MAP | Active canonical item (Fuel) |
| 15 | `battery` | `battery` | EXACT_MATCH | items.json (Battery) | DIRECT_MAP | Active canonical item (Material) |
| 16 | `cloth` | `cloth` | EXACT_MATCH | items.json (Cloth) | DIRECT_MAP | Active canonical item (Material) |
| 17 | `scrap_metal` | `scrap_metal` | EXACT_MATCH | items.json (Scrap Metal) | DIRECT_MAP | Active canonical item (Material) |
| 18 | `mechanical_parts` | `mechanical_parts` | EXACT_MATCH | items.json (Mechanical Parts) | DIRECT_MAP | Active canonical item (Material) |
| 19 | `electronic_scrap` | `electronic_scrap` | EXACT_MATCH | items.json (Electronic Scrap) | DIRECT_MAP | Active canonical item (Material) |
| 20 | `chemicals` | `chemicals` | EXACT_MATCH | items.json (Chemicals) | DIRECT_MAP | Active canonical item (Material) |
| 21 | `handheld_radio` | `handheld_radio` | EXACT_MATCH | items.json (Handheld Radio) | DIRECT_MAP | Active canonical item (Device) |
| 22 | `weapon_pipe_rifle` | `weapon_pipe_rifle` | EXACT_MATCH_COMBAT | combat_catalog.json (weapons) | DIRECT_MAP | Active weapon in combat catalog |
| 23 | `weapon_scrap_shotgun` | `weapon_scrap_shotgun` | EXACT_MATCH_COMBAT | combat_catalog.json (weapons) | DIRECT_MAP | Active weapon in combat catalog |
| 24 | `weapon_bolt_rifle` | `weapon_bolt_rifle` | EXACT_MATCH_COMBAT | combat_catalog.json (weapons) | DIRECT_MAP | Active weapon in combat catalog |
| 25 | `weapon_assault_rifle` | `weapon_assault_rifle` | EXACT_MATCH_COMBAT | combat_catalog.json (weapons) | DIRECT_MAP | Active weapon in combat catalog |
| 26 | `armor_cloth` | `armor_cloth` | EXACT_MATCH_COMBAT | combat_catalog.json (materials) | DIRECT_MAP | Active material/armor in combat catalog |
| 27 | `armor_kevlar` | `armor_kevlar` | EXACT_MATCH_COMBAT | combat_catalog.json (materials) | DIRECT_MAP | Active material/armor in combat catalog |
| 28 | `armor_plate` | `armor_plate` | EXACT_MATCH_COMBAT | combat_catalog.json (materials) | DIRECT_MAP | Active material/armor in combat catalog |
| 29 | `clothing_coat` | `item_heavy_wool_coat` | CANONICAL_ALIASED | items.json (item_heavy_wool_coat) | ALIAS_RESOLVE | Flavor alias to heavy winter coat in items.json |
| 30 | `clothing_boots` | `item_insulated_boots` | CANONICAL_ALIASED | items.json (item_insulated_boots) | ALIAS_RESOLVE | Flavor alias to insulated boots in items.json |
| 31 | `clothing_gloves` | `item_fur_mittens` | CANONICAL_ALIASED | items.json (item_fur_mittens) | ALIAS_RESOLVE | Flavor alias to fur mittens in items.json |
| 32 | `tool_crowbar` | — | ORPHANED_LEGACY | Prose warehouse (tool) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A steel crowbar, heavy, curved at one end. Th... |
| 33 | `tool_knife` | — | ORPHANED_LEGACY | Prose warehouse (tool) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A hunting knife, fixed blade, leather handle.... |
| 34 | `tool_axe` | — | ORPHANED_LEGACY | Prose warehouse (tool) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A hatchet, short handle, heavy head. The edge... |
| 35 | `tool_hammer` | — | ORPHANED_LEGACY | Prose warehouse (tool) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A claw hammer, steel head, wooden handle. The... |
| 36 | `tool_wrench` | — | ORPHANED_LEGACY | Prose warehouse (tool) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A pipe wrench, adjustable, heavy. The jaws ar... |
| 37 | `tool_screwdriver` | — | ORPHANED_LEGACY | Prose warehouse (tool) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A flathead screwdriver, steel shaft, plastic ... |
| 38 | `tool_pliers` | — | ORPHANED_LEGACY | Prose warehouse (tool) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A pair of pliers, steel, serrated jaws, sprin... |
| 39 | `tool_saw` | — | ORPHANED_LEGACY | Prose warehouse (tool) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A hacksaw, metal frame, replaceable blade. Th... |
| 40 | `container_backpack` | — | ORPHANED_LEGACY | Prose warehouse (container) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A military backpack, olive drab, multiple com... |
| 41 | `container_bottle` | — | ORPHANED_LEGACY | Prose warehouse (container) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A plastic water bottle, scratched, dented, bu... |
| 42 | `container_tin` | — | ORPHANED_LEGACY | Prose warehouse (container) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A metal tin, dented, rusted at the seams. The... |
| 43 | `light_flashlight` | `cigarette_lighter` | CANONICAL_ALIASED | items.json (cigarette_lighter) | ALIAS_RESOLVE | Flavor alias: portable illumination/fire source in items.json |
| 44 | `light_candle` | — | ORPHANED_LEGACY | Prose warehouse (light) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A candle, wax, with a wick. The wax is soft. ... |
| 45 | `light_lantern` | — | ORPHANED_LEGACY | Prose warehouse (light) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A kerosene lantern, glass globe, metal frame.... |
| 46 | `communication_signal_mirror` | — | ORPHANED_LEGACY | Prose warehouse (communication) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A signal mirror, glass, with a sighting hole.... |
| 47 | `communication_whistle` | — | ORPHANED_LEGACY | Prose warehouse (communication) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A metal whistle, pealess, loud. The metal is ... |
| 48 | `defense_trap` | — | ORPHANED_LEGACY | Prose warehouse (defense) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A snare trap, wire, with a trigger mechanism.... |
| 49 | `defense_alarm` | — | ORPHANED_LEGACY | Prose warehouse (defense) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A tripwire alarm, cans on a string. The cans ... |
| 50 | `defense_barrier` | — | ORPHANED_LEGACY | Prose warehouse (defense) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A makeshift barrier, plywood and nails. The w... |
| 51 | `electronics_walkie_talkie` | — | ORPHANED_LEGACY | Prose warehouse (electronics) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A pair of walkie-talkies, yellow plastic, sho... |
| 52 | `electronics_solar_panel` | — | ORPHANED_LEGACY | Prose warehouse (electronics) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A portable solar panel, folding, with a charg... |
| 53 | `electronics_battery_charger` | — | ORPHANED_LEGACY | Prose warehouse (electronics) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A battery charger, manual, hand-crank. The ge... |
| 54 | `electronics_two_way_radio` | — | ORPHANED_LEGACY | Prose warehouse (electronics) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A two-way radio, base station, with antenna a... |
| 55 | `quest_item_map` | — | ORPHANED_LEGACY | Prose warehouse (quest) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A hand-drawn map, ink on paper, showing a loc... |
| 56 | `quest_item_key` | — | ORPHANED_LEGACY | Prose warehouse (quest) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A metal key, old, with a unique shape. The te... |
| 57 | `quest_item_document` | — | ORPHANED_LEGACY | Prose warehouse (quest) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A document, typed on paper, with official sta... |
| 58 | `luxury_jewelry` | `jewelry` | CANONICAL_ALIASED | items.json (jewelry) | ALIAS_RESOLVE | Direct alias to jewelry in items.json |
| 59 | `luxury_cigarette` | — | ORPHANED_LEGACY | Prose warehouse (luxury) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A cigarette, tobacco, in paper. The paper is ... |
| 60 | `luxury_book` | `book` | CANONICAL_ALIASED | items.json (book) | ALIAS_RESOLVE | Direct alias to book in items.json |
| 61 | `comfort_blanket` | — | ORPHANED_LEGACY | Prose warehouse (comfort) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A wool blanket, heavy, warm, with a pattern. ... |
| 62 | `comfort_pillow` | — | ORPHANED_LEGACY | Prose warehouse (comfort) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A pillow, stuffed with foam, with a cotton co... |
| 63 | `comfort_photo` | — | ORPHANED_LEGACY | Prose warehouse (comfort) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A photograph, family portrait, in a frame. Th... |
| 64 | `comfort_toy` | — | ORPHANED_LEGACY | Prose warehouse (comfort) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A stuffed animal, teddy bear, with button eye... |
| 65 | `material_rope` | `rope` | CANONICAL_ALIASED | items.json (rope) | ALIAS_RESOLVE | Direct alias to rope in items.json |
| 66 | `material_wood` | `material_wood` | EXACT_MATCH_COMBAT | combat_catalog.json (materials) | DIRECT_MAP | Active material/armor in combat catalog |
| 67 | `material_nails` | — | ORPHANED_LEGACY | Prose warehouse (material) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A box of nails, steel, various sizes. The box... |
| 68 | `material_glass` | — | ORPHANED_LEGACY | Prose warehouse (material) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A sheet of glass, clear, with sharp edges. Th... |
| 69 | `material_fabric` | — | ORPHANED_LEGACY | Prose warehouse (material) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A bolt of fabric, cotton, with a pattern. The... |
| 70 | `material_leather` | — | ORPHANED_LEGACY | Prose warehouse (material) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A piece of leather, cowhide, with a rough sur... |
| 71 | `material_wire` | — | ORPHANED_LEGACY | Prose warehouse (material) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A coil of wire, steel, with a kink. The wire ... |
| 72 | `material_pipe` | — | ORPHANED_LEGACY | Prose warehouse (material) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A length of pipe, steel, threaded at both end... |
| 73 | `material_bolt` | — | ORPHANED_LEGACY | Prose warehouse (material) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A box of bolts, steel, with nuts and washers.... |
| 74 | `survival_fire_starter` | — | ORPHANED_LEGACY | Prose warehouse (survival) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A ferrocerium rod, magnesium, with a striker.... |
| 75 | `survival_tinder` | — | ORPHANED_LEGACY | Prose warehouse (survival) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A bundle of dry tinder, cotton, with wax. The... |
| 76 | `survival_water_purification_tablets` | `water_purification_tablets` | CANONICAL_ALIASED | items.json (water_purification_tablets) | ALIAS_RESOLVE | Direct alias to water_purification_tablets in items.json |
| 77 | `survival_emergency_blanket` | — | ORPHANED_LEGACY | Prose warehouse (survival) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: An emergency blanket, Mylar, reflective. The ... |
| 78 | `survival_first_aid_kit` | — | ORPHANED_LEGACY | Prose warehouse (survival) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A first aid kit, compact, with bandages, anti... |
| 79 | `survival_compass` | — | ORPHANED_LEGACY | Prose warehouse (survival) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A compass, liquid-filled, with a rotating bez... |
| 80 | `survival_map` | — | ORPHANED_LEGACY | Prose warehouse (survival) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A topographic map, waterproof, with contour l... |
| 81 | `cooking_pot` | — | ORPHANED_LEGACY | Prose warehouse (cooking) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A cooking pot, aluminum, with a handle. The p... |
| 82 | `cooking_pan` | — | ORPHANED_LEGACY | Prose warehouse (cooking) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A cooking pan, cast iron, with a handle. The ... |
| 83 | `cooking_utensils` | — | ORPHANED_LEGACY | Prose warehouse (cooking) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A set of cooking utensils, wooden spoon, spat... |
| 84 | `cooking_spices` | — | ORPHANED_LEGACY | Prose warehouse (cooking) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A collection of spices, salt, pepper, herbs. ... |
| 85 | `hygiene_soap` | — | ORPHANED_LEGACY | Prose warehouse (hygiene) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A bar of soap, lye, with a rough texture. The... |
| 86 | `hygiene_toothbrush` | — | ORPHANED_LEGACY | Prose warehouse (hygiene) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A toothbrush, nylon bristles, with a plastic ... |
| 87 | `hygiene_towel` | — | ORPHANED_LEGACY | Prose warehouse (hygiene) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A towel, cotton, with a rough texture. The to... |
| 88 | `hygiene_toilet_paper` | — | ORPHANED_LEGACY | Prose warehouse (hygiene) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A roll of toilet paper, thin, with a cardboar... |
| 89 | `sports_ball` | — | ORPHANED_LEGACY | Prose warehouse (sports) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A soccer ball, leather, with a valve. The lea... |
| 90 | `sports_deck_of_cards` | — | ORPHANED_LEGACY | Prose warehouse (sports) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A deck of cards, paper, with a box. The cards... |
| 91 | `sports_dice` | — | ORPHANED_LEGACY | Prose warehouse (sports) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A pair of dice, plastic, with dots. The dice ... |
| 92 | `sports_book_of_games` | — | ORPHANED_LEGACY | Prose warehouse (sports) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A book of games, paper, with rules. The book ... |
| 93 | `electronics_laptop` | — | ORPHANED_LEGACY | Prose warehouse (electronics) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A laptop computer, battered, with a cracked s... |
| 94 | `electronics_tablet` | — | ORPHANED_LEGACY | Prose warehouse (electronics) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A tablet computer, thin, with a touchscreen. ... |
| 95 | `electronics_smartphone` | — | ORPHANED_LEGACY | Prose warehouse (electronics) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A smartphone, cracked, with a dead battery. T... |
| 96 | `electronics_camera` | — | ORPHANED_LEGACY | Prose warehouse (electronics) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A digital camera, with a lens and a memory ca... |
| 97 | `electronics_drone` | — | ORPHANED_LEGACY | Prose warehouse (electronics) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A small drone, with rotors and a camera. The ... |
| 98 | `electronics_binoculars` | — | ORPHANED_LEGACY | Prose warehouse (electronics) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A pair of binoculars, with lenses and a strap... |
| 99 | `electronics_telescope` | — | ORPHANED_LEGACY | Prose warehouse (electronics) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A telescope, with a lens and a tripod. The le... |
| 100 | `electronics_walkie_talkie_pair` | — | ORPHANED_LEGACY | Prose warehouse (electronics) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A pair of walkie-talkies, yellow plastic, sho... |
| 101 | `electronics_solar_charger` | — | ORPHANED_LEGACY | Prose warehouse (electronics) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A solar charger, folding, with USB ports. The... |
| 102 | `electronics_hand_crank_radio` | — | ORPHANED_LEGACY | Prose warehouse (electronics) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A hand-crank radio, with a flashlight and a U... |
| 103 | `electronics_battery_pack` | `battery_pack` | CANONICAL_ALIASED | items.json (battery_pack) | ALIAS_RESOLVE | Direct alias to battery_pack in items.json |
| 104 | `electronics_speaker` | — | ORPHANED_LEGACY | Prose warehouse (electronics) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A portable speaker, Bluetooth, with a battery... |
| 105 | `electronics_headphones` | — | ORPHANED_LEGACY | Prose warehouse (electronics) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A pair of headphones, over-ear, with a cable.... |
| 106 | `electronics_earbuds` | — | ORPHANED_LEGACY | Prose warehouse (electronics) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A pair of earbuds, in-ear, with a cable. The ... |
| 107 | `electronics_gaming_console` | — | ORPHANED_LEGACY | Prose warehouse (electronics) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A gaming console, with controllers and games.... |
| 108 | `electronics_dvd_player` | — | ORPHANED_LEGACY | Prose warehouse (electronics) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A DVD player, with a remote and cables. The p... |
| 109 | `electronics_projector` | — | ORPHANED_LEGACY | Prose warehouse (electronics) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A projector, with a lens and a remote. The pr... |
| 110 | `electronics_tv` | — | ORPHANED_LEGACY | Prose warehouse (electronics) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A television, flat screen, with a remote. The... |
| 111 | `electronics_radio_alarm_clock` | — | ORPHANED_LEGACY | Prose warehouse (electronics) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A radio alarm clock, with a display and a spe... |
| 112 | `electronics_calculator` | — | ORPHANED_LEGACY | Prose warehouse (electronics) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A calculator, solar-powered, with a display. ... |
| 113 | `electronics_watch` | — | ORPHANED_LEGACY | Prose warehouse (electronics) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A wristwatch, digital, with a battery. The wa... |
| 114 | `electronics_fitness_tracker` | — | ORPHANED_LEGACY | Prose warehouse (electronics) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A fitness tracker, wristband, with a display.... |
| 115 | `electronics_blood_pressure_monitor` | — | ORPHANED_LEGACY | Prose warehouse (electronics) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A blood pressure monitor, with a cuff and a d... |
| 116 | `electronics_thermometer` | — | ORPHANED_LEGACY | Prose warehouse (electronics) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A thermometer, digital, with a display. The t... |
| 117 | `electronics_stethoscope` | `stethoscope` | CANONICAL_ALIASED | items.json (stethoscope) | ALIAS_RESOLVE | Direct alias to stethoscope in items.json |
| 118 | `vehicle_bicycle` | — | ORPHANED_LEGACY | Prose warehouse (vehicle) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A bicycle, mountain bike, with gears and brak... |
| 119 | `vehicle_motorcycle` | — | ORPHANED_LEGACY | Prose warehouse (vehicle) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A motorcycle, with an engine and wheels. The ... |
| 120 | `vehicle_car` | — | ORPHANED_LEGACY | Prose warehouse (vehicle) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A car, sedan, with an engine and wheels. The ... |
| 121 | `vehicle_truck` | — | ORPHANED_LEGACY | Prose warehouse (vehicle) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A truck, pickup, with an engine and wheels. T... |
| 122 | `vehicle_van` | — | ORPHANED_LEGACY | Prose warehouse (vehicle) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A van, cargo, with an engine and wheels. The ... |
| 123 | `furniture_chair` | — | ORPHANED_LEGACY | Prose warehouse (furniture) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A chair, wooden, with a seat and back. The wo... |
| 124 | `furniture_table` | — | ORPHANED_LEGACY | Prose warehouse (furniture) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A table, wooden, with a top and legs. The woo... |
| 125 | `furniture_bed` | — | ORPHANED_LEGACY | Prose warehouse (furniture) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A bed, with a mattress and frame. The mattres... |
| 126 | `furniture_shelf` | — | ORPHANED_LEGACY | Prose warehouse (furniture) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A shelf, wooden, with brackets and boards. Th... |
| 127 | `furniture_cabinet` | — | ORPHANED_LEGACY | Prose warehouse (furniture) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A cabinet, wooden, with doors and shelves. Th... |
| 128 | `furniture_drawer` | — | ORPHANED_LEGACY | Prose warehouse (furniture) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A drawer, wooden, with slides and handles. Th... |
| 129 | `art_painting` | — | ORPHANED_LEGACY | Prose warehouse (art) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A painting, oil on canvas, with a frame. The ... |
| 130 | `art_sculpture` | — | ORPHANED_LEGACY | Prose warehouse (art) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A sculpture, stone, with a base. The stone is... |
| 131 | `art_photograph` | — | ORPHANED_LEGACY | Prose warehouse (art) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A photograph, printed on paper, with a frame.... |
| 132 | `art_vase` | — | ORPHANED_LEGACY | Prose warehouse (art) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A vase, ceramic, with a rim and base. The cer... |
| 133 | `art_candle_holder` | — | ORPHANED_LEGACY | Prose warehouse (art) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A candle holder, metal, with a base and cup. ... |
| 134 | `art_mirror` | — | ORPHANED_LEGACY | Prose warehouse (art) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A mirror, glass, with a frame. The glass is c... |
| 135 | `art_clock` | — | ORPHANED_LEGACY | Prose warehouse (art) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A clock, mechanical, with a face and hands. T... |
| 136 | `art_rug` | — | ORPHANED_LEGACY | Prose warehouse (art) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A rug, woven, with a pattern. The rug is worn... |
| 137 | `art_curtain` | — | ORPHANED_LEGACY | Prose warehouse (art) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A curtain, fabric, with a rod. The fabric is ... |
| 138 | `art_pillow` | — | ORPHANED_LEGACY | Prose warehouse (art) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A pillow, decorative, with a cover. The pillo... |
| 139 | `weapon_pistol` | `pistol_cz75_9x19` | CANONICAL_ALIASED | items.json (pistol_cz75_9x19) | ALIAS_RESOLVE | Flavor alias to standard service sidearm in items.json |
| 140 | `weapon_revolver` | `weapon_revolver` | EXACT_MATCH | items.json (Six-Shot Revolver) | DIRECT_MAP | Active canonical item (Weapon) |
| 141 | `weapon_shotgun` | `weapon_pipe_shotgun` | CANONICAL_ALIASED | items.json (weapon_pipe_shotgun) | ALIAS_RESOLVE | Flavor alias to pipe shotgun in items.json |
| 142 | `weapon_sniper_rifle` | `weapon_marksman_rifle` | CANONICAL_ALIASED | items.json (weapon_marksman_rifle) | ALIAS_RESOLVE | Flavor alias to marksman rifle in items.json |
| 143 | `weapon_smg` | `weapon_smg` | EXACT_MATCH | items.json (Civilian SMG) | DIRECT_MAP | Active canonical item (Weapon) |
| 144 | `ammo_9mm` | `ammo_9x19` | CANONICAL_ALIASED | items.json (ammo_9x19) | ALIAS_RESOLVE | Direct alias to ammo_9x19 in items.json |
| 145 | `ammo_45` | — | ORPHANED_LEGACY | Prose warehouse (ammunition) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A box of .45 ACP ammunition, brass casings, c... |
| 146 | `ammo_556` | `ammo_556` | EXACT_MATCH | items.json (5.56mm Rounds) | DIRECT_MAP | Active canonical item (Ammo) |
| 147 | `ammo_762` | `ammo_762` | EXACT_MATCH | items.json (7.62mm Rounds) | DIRECT_MAP | Active canonical item (Ammo) |
| 148 | `ammo_12g` | `ammo_12g` | EXACT_MATCH | items.json (12-Gauge Shells) | DIRECT_MAP | Active canonical item (Ammo) |
| 149 | `ammo_308` | `ammo_308` | EXACT_MATCH | items.json (.308 Rounds) | DIRECT_MAP | Active canonical item (Ammo) |
| 150 | `ammo_arrow` | — | ORPHANED_LEGACY | Prose warehouse (ammunition) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A quiver of arrows, wooden shafts, metal tips... |
| 151 | `armor_helmet` | — | ORPHANED_LEGACY | Prose warehouse (armor) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A helmet, military, with a chin strap. The he... |
| 152 | `armor_vest` | — | ORPHANED_LEGACY | Prose warehouse (armor) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A tactical vest, with pockets and pouches. Th... |
| 153 | `clothing_shirt` | — | ORPHANED_LEGACY | Prose warehouse (clothing) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A shirt, cotton, with buttons. The shirt is t... |
| 154 | `clothing_pants` | — | ORPHANED_LEGACY | Prose warehouse (clothing) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A pair of pants, denim, with a zipper. The pa... |
| 155 | `clothing_hat` | — | ORPHANED_LEGACY | Prose warehouse (clothing) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A hat, baseball cap, with a brim. The hat is ... |
| 156 | `clothing_scarf` | — | ORPHANED_LEGACY | Prose warehouse (clothing) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A scarf, wool, with a fringe. The scarf is mo... |
| 157 | `clothing_belt` | — | ORPHANED_LEGACY | Prose warehouse (clothing) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A belt, leather, with a buckle. The leather i... |
| 158 | `clothing_underwear` | — | ORPHANED_LEGACY | Prose warehouse (clothing) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A pair of underwear, cotton, with elastic. Th... |
| 159 | `clothing_socks` | — | ORPHANED_LEGACY | Prose warehouse (clothing) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A pair of socks, wool, with cuffs. The socks ... |
| 160 | `clothing_jacket` | — | ORPHANED_LEGACY | Prose warehouse (clothing) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A jacket, denim, with a zipper. The jacket is... |
| 161 | `clothing_vest` | — | ORPHANED_LEGACY | Prose warehouse (clothing) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A vest, fleece, with a zipper. The vest is pi... |
| 162 | `tool_pickaxe` | — | ORPHANED_LEGACY | Prose warehouse (tool) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A pickaxe, with a steel head and wooden handl... |
| 163 | `tool_shovel` | — | ORPHANED_LEGACY | Prose warehouse (tool) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A shovel, with a steel blade and wooden handl... |
| 164 | `tool_rake` | — | ORPHANED_LEGACY | Prose warehouse (tool) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A rake, with steel tines and a wooden handle.... |
| 165 | `tool_hoe` | — | ORPHANED_LEGACY | Prose warehouse (tool) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A hoe, with a steel blade and wooden handle. ... |
| 166 | `tool_rake` | — | DUPLICATE | item_description_texts.json:2811 | DEDUPLICATE | Verbatim duplicate of tool_rake (#164); removed in deduplication pass |
| 167 | `tool_sickle` | — | ORPHANED_LEGACY | Prose warehouse (tool) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A sickle, with a curved blade and wooden hand... |
| 168 | `tool_scythe` | — | ORPHANED_LEGACY | Prose warehouse (tool) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A scythe, with a long blade and wooden handle... |
| 169 | `tool_pitchfork` | — | ORPHANED_LEGACY | Prose warehouse (tool) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A pitchfork, with steel tines and a wooden ha... |
| 170 | `tool_wrench_large` | — | ORPHANED_LEGACY | Prose warehouse (tool) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A large wrench, steel, with adjustable jaws. ... |
| 171 | `tool_socket_set` | — | ORPHANED_LEGACY | Prose warehouse (tool) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A socket set, with various sizes and a ratche... |
| 172 | `tool_tape_measure` | — | ORPHANED_LEGACY | Prose warehouse (tool) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A tape measure, steel, with a spring. The tap... |
| 173 | `tool_level` | — | ORPHANED_LEGACY | Prose warehouse (tool) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A level, with a bubble and a frame. The bubbl... |
| 174 | `tool_square` | — | ORPHANED_LEGACY | Prose warehouse (tool) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A square, steel, with a handle. The square is... |
| 175 | `tool_clamps` | — | ORPHANED_LEGACY | Prose warehouse (tool) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A set of clamps, steel, with various sizes. T... |
| 176 | `tool_vise` | — | ORPHANED_LEGACY | Prose warehouse (tool) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A vise, steel, with a screw handle. The jaws ... |
| 177 | `tool_grinder` | — | ORPHANED_LEGACY | Prose warehouse (tool) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A grinder, electric, with a wheel. The motor ... |
| 178 | `tool_welder` | — | ORPHANED_LEGACY | Prose warehouse (tool) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A welder, electric, with a torch. The motor i... |
| 179 | `tool_soldering_iron` | — | ORPHANED_LEGACY | Prose warehouse (tool) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A soldering iron, electric, with a tip. The t... |
| 180 | `tool_multimeter` | — | ORPHANED_LEGACY | Prose warehouse (tool) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A multimeter, digital, with probes. The displ... |
| 181 | `tool_wire_strippers` | — | ORPHANED_LEGACY | Prose warehouse (tool) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A pair of wire strippers, steel, with handles... |
| 182 | `tool_crimper` | — | ORPHANED_LEGACY | Prose warehouse (tool) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A crimper, steel, with handles. The jaws are ... |
| 183 | `tool_cable_tester` | — | ORPHANED_LEGACY | Prose warehouse (tool) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A cable tester, digital, with probes. The dis... |
| 184 | `tool_circuit_tester` | — | ORPHANED_LEGACY | Prose warehouse (tool) | RETAIN_FLAVOR_ONLY | Non-inventory object flavor: A circuit tester, with a light and probes. Th... |
