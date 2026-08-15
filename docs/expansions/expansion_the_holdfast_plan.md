# ASHFALL — Expansion Design Bible

**Title:** ASHFALL: THE HOLDFAST  
**Internal id:** `expansion_the_holdfast`  
**Status:** Design bible for review. No game data has been edited.  
**All new ids below are PROPOSED** unless marked *existing*.  
**Tone lock:** cold, exhausted, human, restrained. Specificity over adjectives. The game never tells the player how to feel.

---

# ANALYSIS PHASE

## 1. Current strengths and weaknesses

### Strengths (what the repo actually has)

- **A finished political thesis, not a mood board.** The lore bible (`docs/lore/00–06`) and live `world_history.json` already connect three canon beats — *The Bunker Boom*, *The Quiet Evacuation*, *The Final Broadcasts* — into the Continuity Allocation Schedule. The player's bunker is Allocation 12. They are not Allocation 12. That sentence can carry a whole expansion if the destination of the people who *were* allocated is ever shown.
- **A closed, legible map.** Sector 4 is five named sub-regions (Grid / Verge / Spine / Toll / Drown) held by four Powers. The Powers/Currents split in `05_FACTIONS.md` is the correct way to add people without diluting territory. `faction_lore.json` now holds six entries (the original four plus `faction_rebuilders` and `faction_black_ops`), and `FactionLoreCatalogLoader` was extended to match.
- **Located knowledge is now wired.** `LoreDiscoveryIndex` + `world_history.json` `discovery_location_id` / `knowledge_key` means history is found in a place. Trust-reactive prose (`threateningBodyText`) exists in `events.json` and is still underused — that is an opportunity, not a missing system.
- **Survival loop is deep enough.** Needs (hunger, thirst, fatigue, warmth, morale, radiation, health), shelter degradation, air-filter wear, `WaterEconomySystem` (catchment + 3-tier purifier), `OzoneScourgeSystem` (False Spring / Silent Spring UV), 15 `Victory_*.cs` paths, 27+ afflictions, Utility AI (not an LLM), event bus, JSON + ScriptableObject pipeline.
- **Orphan ids that are gifts.** `FactionSO.Ids.HydroBarons = "hydro_barons"` is already documented as *"Hydro-Barons of Sector 8 — remnants of the municipal water authority"* and has **no** `faction_lore.json` entry, **no** gazetteer region, and **no** questline. `location_abandoned_desalination`, `location_crashed_icebreaker_convoy`, `location_frozen_river_barge` sit on the map as flavour. `Victory_Icebreaker` and `Victory_Migration` both walk north and then **fade to white**. The destination was never written.

### Weaknesses (what the expansion must not pretend isn't true)

- **Content sprawl without a second geography.** Expansion V (`docs/superpowers/specs/2026-08-12-ashfall-massive-content-expansion-design.md`) asked for +50 locations *inside* Sector 4. The lore bible then *filled* Sector 4 (40 locations, 14 Currents, The List). Adding another 50 POIs to the same five sub-regions would make the map mush. The territorial map is **closed on purpose** (`00_OVERVIEW.md`: no fifth Power).
- **Two faction id namespaces remain a live defect.** Lore/UI (`iron_garrison`) vs systems (`faction_central_garrison`). New content must not pick a side. Currents and a *new district catalog* sidestep this. Do not add a seventh row to `FactionRelationships` unless the DTO is deliberately grown.
- **Endings that leave.** `Victory_Migration` cannibalises the heater, takes four seats, and drives into ice that cracks. `Victory_Icebreaker` is a skeleton: contact a vessel 30 nodes away, haul 100 explosives, extract. Neither has a place, a people, or a cost that survives the credits. Blood & Wine / Shadow of the Erdtree scale, in *this* genre, is **giving those fades a map**.
- **Systems exist as ghosts.** `NPC_BlackOps` is a dormant ghost. `VehicleSystem` is ~100 lines (engine/tires/chassis/battery). Forward outposts and livestock were proposed in Expansion V and should only be built if this expansion *needs* them.
- **Tone debt in older data.** `location_frozen_river_barge` is "cannibalistic dockworkers." `Victory_TrueEnding` is off-world terraforming. `NPC_Android` exists. `FactionSO` still has "scrap-neuromancers." This expansion does not extend any of that. It also does not retcon it in the patch notes; it simply does not use it.
- **The game is still 2D management.** Runtime presentation is UI Toolkit over a near-black orthographic camera (`docs/ai-art/GAME_VISUAL_DNA.md`). There is no authored 2D overworld renderer yet. "Open world" here means **node graph + location cards + expedition ticks**, not a walkable coast.

## 2. Top 3 opportunities

| # | Opportunity | Why it is the largest lever |
|---|---|---|
| 1 | **Show the allocated world** | The List is the spine. Sector 4 is the unlisted. The expansion is what the formula *bought*. No new thesis required — only the other room. |
| 2 | **Give Migration / Icebreaker a destination** | Both victories already point north. `hydro_barons` already names Sector 8. The Ice Road is a seasonal gate, which is a *system*, not a loading screen. |
| 3 | **Invert the water problem** | Sector 4 dies of thirst (`faction_rebuilders` / Allotments / brass fittings). Sector 8 drowns in brine. Same metal, opposite shortage. Brass, iodine, resin, and calories become a two-district economy without a new currency. |

## 3. Critical gaps and assumptions

| Gap | Assumption used in this bible |
|---|---|
| No playable "companion" party system (Witcher-style) | Companions are **named survivors / NPCs** assignable to northern expeditions and to one waystation. Utility AI scores their actions. They are not a combat party. |
| No Sector 8 geography in the gazetteer | Coastal District 8 sits **downriver of the Drown**, north of Sector 4, on the cold sea the gazetteer already implies. |
| `hydro_barons` has an id and a one-line comment, no lore | They are the municipal water authority remnant. They run the plant. They are not a fifth Sector 4 Power. |
| `location_crashed_icebreaker_convoy` description says "derailed military train" while `Victory_Icebreaker` is a vessel | Treat the *convoy* as ice-capable military rolling stock that tried to reach the coast. Treat the *victory* as a continuity tender frozen in pack ice. Both can be true. Recast flavour later; do not invent a nuclear-sub action setpiece. |
| Sela Renn's Day 200 hatch scene is designed, not necessarily shipped as a quest | Expansion **hooks** that scene (`lore_af_the_claim`) but does not replace it. If the claim has not fired, Edor Vale's census can still start the district. |
| Faction DTO cannot take a 7th Power in `faction_lore.json` | Sector 8 factions live in a **new** `holdfast_factions.json` (Currents-style catalog). `hydro_barons` stays on `FactionSO.Ids` for trade. |
| Tessarat (`location_tessarat_crematorium`, `loc_tessarat_water_plant`) | Stale leftover name. **Do not use.** Sector 4 / Sector 8 only. |
| `Node_Sector7G` | Dev-room easter egg. **Not** Sector 8. Do not confuse with radio line "activity in sector 7." |
| How big is the player's shelter population? | Design against 8–14 living survivors (Allocation 12 was provisioned for 14). |
| Combat model | Encounters are expedition-tick + stance + Utility AI + resource spend. "Bosses" are **multi-phase crises**, not action arenas. |

## 4. Three expansion concepts (brief)

**A. THE HOLDFAST — Coastal District 8** *(recommended)*  
The Quiet Evacuation went north. The people who scored ≥60 arrived. They have desalinated water, heat, and the Schedule. They are dying of calories, resin, children, and the trades the formula discarded. They have come south to collect the unlisted — politely, with forms.

**B. THE SECOND WINTER — temporal expansion inside Sector 4**  
A harsher nuclear-winter season, new weather, deeper shelter degradation, no new map. Cheaper. Does not pay off The List. Does not use `hydro_barons`. Feels like a difficulty pack, not Blood & Wine.

**C. THE DEAD HAND — automated military belt**  
Lean into `location_the_dead_hand_core`, `location_drone_hive_silo`, `location_automated_mortar_pit`. Strong setpieces, weak thesis. Risks sci-fi action and glorified hardware. D/9 already owns "orders that outlived the state."

## 5. Choice and why

**Proceeding with A — THE HOLDFAST.**

It is the only concept that (1) respects the closed Sector 4 map, (2) spends the orphan `hydro_barons` / desalination / icebreaker / Migration assets, (3) inverts hunger/thirst instead of adding a fifth affliction, (4) pays off The List without contradicting Sela Renn, and (5) is feasible as node-graph 2D management rather than a new genre.

Blood & Wine, in this house, is not a sunny duchy. It is a **district that looks like it survived**, and then you live there long enough to see the board.

---

# SECTION 1 — EXPANSION OVERVIEW

| Field | Value |
|---|---|
| **Title** | ASHFALL: THE HOLDFAST |
| **id** | `expansion_the_holdfast` |
| **Hook** | The formula worked. The people it selected are still alive. They have come to collect the trades it threw away — including yours. |
| **Tagline (UI, not marketing-speak)** | *District 8 kept the list. The list is thirsty.* |
| **Genre lock** | Same game. 2D survival-**management**. Expeditions are node ticks. No 3D coast, no action RPG, no co-op. |
| **Playtime (new content)** | **12–18 hours** for the main claim + district loop on a mid-game save; **20–28 hours** if the player winters on the Ice Road and completes side catalogs. |
| **Witcher 3 B&W / SotE scale — honest conversion** | Those DLCs are 20–30 hours of action-RPG. The equivalent *here* is a **second district with its own economy, seasonal gate, story, and endings**, not a walkable overworld. Flagged unrealistic: full VO, 50-hour combat campaign, netcode, 3D ice physics. |
| **Progression gate (soft)** | Day **90+**, shelter can field a 3-person expedition of ≥8 hours, `water_filter` or purifier online, radiation shielding module present. |
| **Progression gate (story)** | Knowledge key `lore_pre_the_formula` **or** `lore_pre_allocation_letters` **or** Ostrowski has sold `item_map_sheet_ice_road`. Recommended but not required: Day 150+ Archivists / `lore_bs_the_vault_holds`. |
| **Progression gate (hard ending)** | Day 200+ List Layer 5 (`lore_af_the_claim`) **or** completing `quest_holdfast_the_second_list`. The hatch scene and the census are two doors into the same house. |
| **Does not require** | Any specific Sector 4 Power allied. Does not require D/9 stand-down. Does not require Rebuilders saved — but their brass/water state **changes the prices** in District 8. |

### One-paragraph pitch

North of the Drown the river reaches a cold sea. Continuity did not put its best-scoring households in holes in Sector 4. It put them in **Allocation Cluster 7**, a planned coastal settlement heated by waste steam from a desalination plant the municipal water authority had been upgrading for the Water Wars. They arrived. The hatch authenticated. The formula, for once, ran to completion. Five years later the plant still makes water that is almost drinkable, the Cluster still has numbered apartments, and the Office still holds the Schedule for **every district**, including Sector 4. They have a labour shortage in every trade the Reconstruction Utility Rating scored below twenty. Caretakers. Records clerks. Veterinarians. The people who cut ice. The people who keep lamps. They do not raid. They **levy**.

### Integration strategy

| Layer | How it attaches |
|---|---|
| **Map** | New `GeneratedMap` region tag `region_holdfast` with 4 sub-regions (see §2). Reaches from existing Drown/Toll nodes via `loc_ice_road_gate` (new) and optionally via boat from `loc_the_shallows_market` (*existing*) if the Kittiwake chart has been copied. |
| **Travel** | `IceRoadSystem` opens a node chain for 11–20 day freeze windows. Outside the window, only the Shallows boat (slow, wet, high rads) or a snow-crawler (`Victory_Migration` chassis — if built, it can go *to* District 8 instead of fading). |
| **Economy** | No new currency. District 8 buys **calories, brass, resin, iodine, seeds, children's clothes**. District 8 sells **clean-enough water, salt, heat-credits, RO membranes, pre-war packaged stock**. Hooks `DynamicEconomySystem` + `hydro_barons` trade prefs. |
| **Lore** | New `world_history` era entries under `ashfall` and a new `holdfast` discovery set. Does not rewrite Sector 4 history. |
| **Factions** | New catalog `holdfast_factions.json`. Do **not** add to `faction_lore.json` (DTO is at 6). `hydro_barons` remains the economy id. |
| **Victories** | Does **not** add a 16th unrelated `Victory_*.cs` unless shipping requires a discrete flag. District 8 **feeds** `Victory_Migration` (destination = Cluster or Fleet), `Victory_Icebreaker` (the tender is now a place with people), and `Victory_Unifier` (the Office will not sign a Sector 4 peace; they will sign a *levy treaty*). Story endings in §3 are narrative flags, not new win-screens, except `victory_the_holdfast` as an optional epilogue slide. |
| **Save** | One expansion flag `exp_holdfast_unlocked` + per-system state blobs. Old saves load; the Ice Road is dark until the gate quest. |
| **UI** | Lore Codex tab "District 8" (or "The Holdfast"). Waystation panel reuses shelter vitals at reduced module set. Ice Road calendar on the map screen. Census ledger as a diegetic document, not a reputation bar. |

### What the player is managing in District 8

The same seven needs. The **weights flip**.

| Need | Sector 4 | The Holdfast |
|---|---|---|
| Thirst | Scarce clean water | Brine everywhere; drinkable water is a **process** (resin + iodine + heat) |
| Hunger | Verge calories, fragile | Cluster hydroponics of four crops, failing; they will pay anything for Verge seed |
| Warmth | Nuclear winter, heater vs fuel | Waste steam from the plant — until the plant stutters, then the Cluster dies in 48 hours |
| Radiation | Ash, fallout storms | Lower ash, **higher UV** (coastal ozone, ice albedo). `OzoneScourgeSystem` is native here, not rare |
| Fatigue | Expedition distance | Ice Road windows force long hauls on little sleep |
| Morale | Guilt, crowding, death | The allocated are polite and correct and they will inventory your people |
| Health | ARS, cold injury | Salt-rash, UV blistering, electrolyte crash, resin-fume lung |
| Shelter | Your bunker degrades | Waystation degrades *faster* (salt air). Home bunker still degrades while you are north |

---

# SECTION 2 — NEW WORLD REGIONS

**District 8 — Hydrographic District 8** (pre-war) / **the Holdfast** (what the allocated call it) / **the Salt** (what Sector 4 will call it after they have been there).

Held by: nobody the way Voss holds the Grid. **The Office** administers the Cluster. **The Salt** (`hydro_barons`) keep the plant. **The Cutters** keep the Ice Road. **The Fleet** has not come ashore. The Drown still belongs to nobody; its north outlet is how you arrive.

Visual DNA for all four: same dry-gouache, ash-grey / concrete / rust / terminal amber. District 8 adds **salt rime, UV-scoured white, numbered prefab, painted lines that were never allowed to fade.** Too clean. Too quiet. The ash is thinner. The light is worse.

Travel banding from the player's bunker (Grid/Verge seam, per gazetteer):

| Sub-region | `travelHours` | Danger | Signature hazard |
|---|---|---|---|
| The Cut | 6.0–8.0 | 5–7 | Ice, brine, UV |
| The Saltworks | 7.5–9.5 | 6–8 | Brine flood, resin fumes, heat failure |
| The Cluster | 8.0–10.0 | 4–6 | Census, morale, sudden cold if steam dies |
| The Shelf | 10.0–14.0 | 7–10 | Pack ice, RTG shine, Fleet etiquette |

---

## 2.1 The Cut — *Estuary Approach 8-South*

**id prefix:** `loc_cut_*`  
**Visual:** River ice the colour of old milk. Channel markers that do not match the Kittiwake chart. A road that exists only in winter. High-vis jackets faded to the colour of bone.  
**Lore:** Pre-war this was a dredged shipping cut. The Exchange froze it into a seasonal highway. The Cutters light it the way Lamplighters light Sector 4 — except a dark lamp here does not mean ambush. It means the ice is wrong.  
**Unique mechanic:** `IceRoadSystem` — travel only while `ice_road_open == true`. Walking the Cut off-season is swimming in brine at −20°C (immediate warmth + health crisis).  
**Who you meet:** Cutters; Undertow-adjacent wreckers who drifted north; Long Walk (once a year); Edor Vale's first interview.

### POIs (8)

| id | Name | d | hrs | rads | Hook |
|---|---|---|--:|--:|---|
| `loc_ice_road_gate` | The Gate | 5 | 6.0 | 28 | First northern node. A boom across ice. A ledger of axle weights. The Tollman's cousin-profession, without the joke. |
| `loc_cut_kilometre_19` | Kilometre 19 | 5 | 6.5 | 30 | Reflector post. Last Sector 4 lamp. Ivy Corrigan will not cross it. The ledger in Sector 4 stops here. |
| `loc_cut_weigh_hut` | Ice Weigh Hut | 5 | 7.0 | 26 | Mass is still how District 8 prices passage. Favours included. Receipts in triplicate. |
| `loc_cut_dredger_hulk` | Dredger *Moth* | 6 | 7.5 | 38 | Frozen in, stack still smoking on geothermal bleed. Someone lives in the superstructure and sells charts that contradict Ostrowski. |
| `loc_cut_brine_pool` | The Open Pool | 7 | 7.5 | 44 | Ice that never takes. Warm from a brine outfall. Fish here are a bad idea and a protein source. |
| `loc_cut_waystation_a` | Waystation A | 5 | 8.0 | 24 | The only legal overnight. Four bunks, a stove, a filter that lasts 11 days. **Forward-outpost candidate.** |
| `loc_cut_accident_12` | Accident 12 | 7 | 8.5 | 36 | A column went through. The ice did not. Cargo still readable on the crates: `ALLOC-7 / NOT FOR GENERAL ISSUE`. |
| `loc_cut_south_beacon` | South Beacon | 6 | 8.0 | 32 | Cutter-maintained. If it goes dark during a window, the road is considered closed even if the ice is thick. |

**Map note:** A single spine of nodes, south→north, with two optional spurs (hulk, brine pool). No branching empire. The road is a sentence.

---

## 2.2 The Saltworks — *Municipal Desalination 8*

**id prefix:** `loc_salt_*`  
**Visual:** Concrete intakes, salt-white yards, steam that smells like hot metal and iodine. RO halls like naves. Workers in suits that were never hazmat — they were *plant* suits, and they have been patched with inner-tube.  
**Lore:** The Hydro-Barons (`hydro_barons`, *existing id*) were not barons. They were grade 4–7 municipal engineers. The Toll coined the name the same way it coined Rebuilders. They kept the plant because turning it off kills the Cluster in two days, and because nobody issued a stop order.  
**Unique mechanic:** `BrineWaterSystem` — water output is high; potability requires `item_ro_resin` + `iodine_pills` + heat. Plant steam is a regional warmth source. If the membrane hall trips, Cluster indoor °C falls on a 48-hour clock.  
**Who you meet:** Leva Quist; Salt workers; Office auditors taking meter readings.

### POIs (8)

| id | Name | d | hrs | rads | Hook |
|---|---|---|--:|--:|---|
| `location_abandoned_desalination` | Municipal Desalination (*existing*) | 7 | 8.5 | 45 | No longer "abandoned" once the expansion is live — **occupied, failing, named**. Recast description on implementation. Reverse osmosis hall. Hydro-Baron seat. |
| `loc_salt_membrane_hall` | Membrane Hall 2 | 7 | 9.0 | 40 | The still-working bank. Resin drums counted twice a day. The count is always short. |
| `loc_salt_intake_caisson` | Intake Caisson | 8 | 9.5 | 52 | Below the ice. Diving if `ShelterModule_SubBay` ever ships; until then, a timed expedition with rebreather loot. |
| `loc_salt_iodine_store` | Iodine Store | 6 | 8.5 | 28 | Locked. The Office has the key. The Salt has a copy they do not admit. Thyroid medicine and water treatment in the same cage. |
| `loc_salt_outfall` | Brine Outfall | 7 | 9.0 | 48 | Where the plant returns what it does not want. The Open Pool's parent. Salt-rash cases come from working here without a shift limit. |
| `loc_salt_grade_hut` | The Grade Hut | 5 | 8.0 | 22 | Hydro-Baron committee room. Minutes. Same grammar as Ottilie Frayne's Works, different water. |
| `loc_salt_cooling_canal` | Cooling Canal | 6 | 8.5 | 34 | Steam to the Cluster runs along this. Sabotage here is a war crime by local definition and a repair job by yours. |
| `loc_salt_scrap_membranes` | Spent Stack | 6 | 9.0 | 42 | Failed RO membranes. Toxic to handle, valuable to people who still believe they can be recoated. |

**Map note:** Hub-and-spoke around the desalination plant. Cluster is a steam-pipe's length north-east.

---

## 2.3 The Cluster — *Allocation Cluster 7*

**id prefix:** `loc_cluster_*`  
**Visual:** Four-storey prefab, numbered stairwells, a playground with the chains still on the swings and the seats unscrewed (brass). Paint still the civil-service cream. Queue lines painted on asphalt, repainted, like Ration Plaza, except here the queue is for **work tickets**, not bread.  
**Lore:** 400 allocated households. Occupancy at present: 211 living, 40 apartments kept "for arrivals." They have been keeping those forty for five years. Your unlisted people are, on paper, some of the arrivals.  
**Unique mechanic:** `CensusClaimSystem` — the Office can name your survivors by occupation and score. A levy is not a raid. Refusing a levy is a *status*, and it follows you home.  
**Who you meet:** Registrar-General Cael Ormund; allocated civilians who will not make eye contact with unlisted guests; children born after the Exchange who have never been hungry in the Sector 4 sense and are salt-hungry instead.

### POIs (7)

| id | Name | d | hrs | rads | Hook |
|---|---|---|--:|--:|---|
| `loc_cluster_gatehouse` | Cluster Gatehouse | 4 | 8.5 | 18 | Authentication. They will ask for an Allocation number. "12" is a known discrepancy. They have a procedure for discrepancies. |
| `loc_cluster_quad` | The Quad | 4 | 8.5 | 16 | Civic square. Hydroponic troughs along the south wall, four cultivars, two failing. A noticeboard with the labour rota and a missing-persons strip that is all Sector 4 trades. |
| `loc_cluster_block_c` | Block C | 5 | 9.0 | 20 | Guest block. You may be housed here during a window. The nameplates are populated. Yours would be a paper tag. |
| `loc_cluster_clinic` | Cluster Clinic | 5 | 9.0 | 22 | Real autoclave. Real iodine. No veterinarian. Human formulary intact — Ianov would weep. They will not send a copy south unless the levy is honoured. |
| `loc_cluster_school` | Cluster School | 4 | 8.5 | 16 | Nineteen children. Curriculum includes the Reconstruction Utility Rating, taught as arithmetic. Wren, if brought, will sit in the back and not speak. |
| `loc_cluster_office` | The Office | 5 | 9.0 | 18 | Continuity civil service that *arrived*. Ormund's desk. The Sector 4 Schedule in a drawer, complete. Margit Sole's copy in the Drown is no longer the only one. |
| `loc_cluster_steam_substation` | Steam Substation | 6 | 9.5 | 28 | If this dies, Block C hits 2°C in a night. The plant can be up and this still down — valves, not ideology. |

**Map note:** Compact. Indoor nodes. This is Blood & Wine's "city" — not size, *density of manners*.

---

## 2.4 The Shelf — *Pack Ice / Continuity Roadstead*

**id prefix:** `loc_shelf_*`  
**Visual:** Pressure ridges. A harbour that is now a white field. Hulls at impossible angles. One vessel still upright, still drawing a little current, still answering on a schedule. RTG shine on the crashed convoy visible as a bruise on the ice.  
**Lore:** Continuity's coastal roadstead. The Fleet was told to wait for a stand-up order that used the same authentication family as D/9's pad. They waited. Some came ashore in year two and were absorbed into the Cluster. The tender *Hearth-4* did not.  
**Unique mechanic:** Deep expeditions (10–14h). `Victory_Icebreaker` content lives here as **place**, not minigame. Explosives to "blast the ice" remain possible and are recast as a *bad* idea that the Cutters will say so out loud.  
**Who you meet:** Halden Mire (if he has come ashore); Fleet remaining; nobody, for hours.

### POIs (6)

| id | Name | d | hrs | rads | Hook |
|---|---|---|--:|--:|---|
| `location_frozen_river_barge` | Frozen River Barge (*existing*) | 6 | 10.0 | 30 | Recast: dock crew who have been living on frozen cargo. Not a cannibal carnival. They will trade a crate for a way off the ice. |
| `location_crashed_icebreaker_convoy` | Icebreaker Convoy (*existing*) | 7 | 11.0 | 85 | RTG hotspot. Military rolling stock that tried to reach the roadstead. Loot: tungsten, tracks, a map fragment `Victory_Migration` already wants. |
| `loc_shelf_hearth4` | Tender *Hearth-4* | 8 | 12.0 | 40 | Upright. Authenticator light still lit. This is the Icebreaker victory's body. People inside. A hatch that wants a number. |
| `loc_shelf_roadstead_crane` | Roadstead Crane | 7 | 11.5 | 36 | Only heavy lift on the coast. Ties `VehicleSystem` / Recovery Yard analog. |
| `loc_shelf_pressure_ridge` | The Ridge | 8 | 12.5 | 44 | Walking route to *Hearth-4* when blasting is refused. Warmth and fatigue bill. |
| `loc_shelf_foghorn` | Foghorn 8 | 6 | 10.5 | 32 | Still sounds in fog, on a timer. The Cutters navigate by it. If you silence it to avoid attention, something else also loses the coast. |

**Map note:** Endgame lobe. Not required for the census story. Required for Icebreaker / "leave on the water" endings.

---

## 2.5 Existing Sector 4 nodes that change meaning (not new regions)

When `exp_holdfast_unlocked`:

- `loc_the_shallows_market` — Nomi Fisk will run north once per window, for a price, if etiquette has not been broken.
- `loc_weighbridge` / `loc_toll_house` — Edor Vale's first appearance; Tollman charges for *introductions*.
- `location_ministry_of_truth_bunker` — a second copy of the Schedule's cover letter, addressed to District 8.
- `loc_the_allotments` — Frayne's brass demand and District 8's brass demand **stack**. The nameplates in the tin become a three-way silence.
- `loc_low_background_lab` — Cold Count can prove fallout provenance; District 8 does not want this any more than Sector 4 did. They will ask anyway.

---

# SECTION 3 — MAIN STORYLINE

## Central conflict

**Two correct documents.**

Sela Renn's laminated card says Allocation 12 belongs to a water engineer's dependent.  
The Office's Schedule says Allocation 12 was never occupied by its assignees, and that Sector 4 still owes District 8 the **unallocated labour** Continuity reserved as a reconstruction pool — caretakers, clerks, vets, ice-cutters, the people who scored under 60.

Your shelter is the discrepancy.  
Ormund wants it resolved.  
He is not Voss. He will not shoot deserters. He will **file**. Filing, in District 8, moves people.

The player must decide whether the unlisted are a community or a labour reserve the formula stored in the wrong building.

## Theme (unspoken)

Fairness that was published in advance is still a selection. Survival does not make the arithmetic kind. Kindness does not make the arithmetic go away.

## Principal NPCs (6)

### 1. `npc_cael_ormund` — Registrar-General Cael Ormund

- **Where:** `loc_cluster_office`
- **Was:** Logistics planner, Office of Continuity, **RUR 34, score 62.1, ALLOCATED**
- **Wants:** The Sector 4 reconstruction pool released as written. Completeness. He and Margit Sole want the same noun and are opposite people.
- **Will not:** Falsify a score. Raise his voice. Call anyone a thief.
- **Voice:** Civil-service present tense. "The discrepancy is noted." Never "please." Never "or else." The threat is the next paragraph of the form.
- **Snippet:**
  > "You are living in a facility that authenticated for fourteen. The fourteen did not arrive. Under Continuity Reconstruction Order 12-C, unallocated occupants of an authenticated facility constitute a labour reserve. I am not collecting you. I am **scheduling** you."

### 2. `npc_edor_vale` — Census Clerk Grade III Edor Vale *(companion)*

- **Where:** first at `loc_weighbridge`, then the Cut, then your hatch
- **Was:** Junior enumerator. Score 60.4 — the lowest allocated band. He knows it.
- **Wants:** Names, occupations, dependent counts. A completed return. He has never closed a Sector 4 file.
- **Will not:** Enter your bunker uninvited. He will wait in the ash. The waiting is the pressure.
- **Voice:** Polite, tired, slightly too precise about dates of birth. He always offers to read the form again.
- **Snippet:**
  > "Most people want it read again. That's all right. There isn't a time limit on understanding it. There is a time limit on the ice."

### 3. `npc_leva_quist` — Shift Lead Leva Quist *(companion)*

- **Where:** `loc_salt_grade_hut` / Membrane Hall
- **Was:** Municipal RO technician. Never allocated — she was **already at the plant** on Hour Zero, which the Office filed as "in situ essential" rather than a score. She has opinions about that.
- **Wants:** Resin, brass valve seats, iodine, people who will do the outfall shift. The plant to outlive the Office.
- **Will not:** Shut the plant to spite Ormund. She has done the 48-hour math.
- **Voice:** Technical, dry, counts out loud. Calls the allocated "the indoors."
- **Snippet:**
  > "They scored high enough to be continued. The membranes don't care. I need four people on the outfall by morning or the indoors freeze in their numbers."

### 4. `npc_yara_holm` — Cutter Yara Holm *(companion)*

- **Where:** `loc_cut_waystation_a`
- **Was:** Harbour ice-pilot. Score 44. Unlisted. District 8 hired her because the allocated would not go out on the ice in year one and some of them died of that.
- **Wants:** Lamp oil, spikes, a second beacon keeper. The road open. No blasting.
- **Will not:** Guide a column onto ice she has marked dark. Same spine as Ivy Corrigan; different district; they should never be forced to meet, and if they do, they will agree without liking it.
- **Voice:** Short. Distances. "Dark" and "lit" as moral words.
- **Snippet:**
  > "I don't open it for you. I open it. If it's dark, you wait. If you don't wait, I write the accident in the book and I don't fetch you."

### 5. `npc_halden_mire` — Sparks Halden Mire *(companion, late)*

- **Where:** `loc_shelf_hearth4` then ashore
- **Was:** Fleet radioman. Has listened for a stand-up order for five years. Same wound as Anneke Ruhl, different frequency. If the player has stood D/9 down with Sole's paper, Mire will **ask to see the form**. It will not authenticate on his pad. He will not be angry. He will be interested.
- **Wants:** To be told the wait is over, or to be told it is not, in writing.
- **Will not:** Open *Hearth-4*'s hatch for unauthenticated boarding. The Icebreaker explosives plan reads to him as murder.
- **Voice:** Radio procedure. "Say again." Dead air as punctuation.
- **Snippet:**
  > "I can hear you. That is not the same as a stand-up. I need a stand-up."

### 6. `npc_sela_renn` — Sela Renn *(existing spine; companion if admitted)*

- **Where:** player shelter if `alloc12_honoured` or `alloc12_letter_only`
- **Wants:** The adults who walked with her not to be a line item. Later: her father's improvised kit at 12-B recognised as **engineering**, not salvage.
- **In this expansion:** District 8's clinic will try to **claim her** as a dependent of an allocated water engineer (Halvard, deceased) — which would make her a Cluster child, not a Sector 4 labour unit. Ormund will prefer this. Sela may not.
- **Snippet:**
  > "They have a school. They have iodine. They have my father's number in a drawer. That isn't the same as having him."

**Margit Sole** remains in the Drown. Late-game, the player can carry a copy of Ormund's levy north-to-south. Sole will file it. She will not sign it. That is a quest, not a boss.

## Story beats (10)

| # | Beat | Day / gate | What happens |
|---|---|---|---|
| 1 | **The sheet** | Day 90+ | Ostrowski sells a waxed sheet of the estuary that includes a road that is not there in summer. He will not say who surveyed it. |
| 2 | **The clerk** | After beat 1 or Toll trust ≥ 20 | Edor Vale at the weighbridge, reading a census return. He already has three of your survivors' pre-war occupations, wrong by one each. |
| 3 | **The window** | First freeze after beat 2 | Ice Road opens. Yara Holm at the Gate. First crossing. Waystation A. Warmth/fatigue bill. |
| 4 | **The plant is not abandoned** | Arrive Saltworks | `location_abandoned_desalination` recast. Leva's minutes. Steam visible toward the Cluster. |
| 5 | **Authentication** | Cluster Gatehouse | They ask for a number. "12" opens a discrepancy file. Guest housing in Block C. The playground chains. |
| 6 | **The drawer** | Office | Ormund shows the Sector 4 Schedule. Sole's entry is there. Frayne is not (RUR 11). Halvard Renn is, marked ALLOCATED / NOT ARRIVED / 12-B UNCONFIRMED. |
| 7 | **The levy** | After a night in Block C | A labour return: 3 survivors for 30 days, named by trade. This is the first **branch**. |
| 8 | **The membrane** | Timed, 8–20 days after levy | Plant trips. 48-hour freeze clock on the Cluster. Resin + brass + bodies. Sector 4's thirst and District 8's thirst become one job. |
| 9 | **The second list** | After membrane or if levy refused | Ormund produces Reconstruction Order 12-C: unlisted occupants of Allocation 12 are a reserve. He will come south when the ice allows. |
| 10 | **The hatch, again** | Day 200+ or next window | Not Sela (or Sela *and* this). An Office escort. Forms. Temperature. The game stops talking. |

## Branching choices (5)

| id | Choice | Immediate | Long |
|---|---|---|---|
| `holdfast_levy_honour` | Send the three named survivors north for 30 days | Calories/medicine inflow; those three take Ice Road fatigue + salt-rash risk | Cluster trust up; Sector 4 morale hit on remaining; named survivors may refuse to return |
| `holdfast_levy_substitute` | Send three *other* people | Ormund notes the substitution as irregular | Later audit. Possible second levy. Yara respects it; Edor does not |
| `holdfast_levy_refuse` | Refuse, in writing or by silence | No combat. Edor waits. Ice Road access withdrawn after 11 days (lamps-out cousin) | Beat 9 accelerates. `threateningBodyText` on all Office scenes |
| `holdfast_membrane_sector4` | Strip Sector 4 (Rebuilders brass, iodine, filters) to save the plant | Cluster lives; Allotments thirst clock shortens; Frayne's minutes record a shortage | Medical market shock (`Mutation_MedicalSupplyGone` adjacent) |
| `holdfast_membrane_let_drop` | Let steam die | 211 people enter a cold they have not trained for. Not instant death — a week of bad decisions | Office legitimacy cracks. Salt may offer a separate bargain. Unifier path hardens |

Additional silent branch: **the nameplates**. District 8 will pay more than the Works. Still no comment. `lore_hz_nameplates` remains a tin.

## Endings (4 narrative + 1 fade)

All endings write a `world_history` second paragraph discoverable at `loc_cluster_office` or the player's hatch. The game does not rank them.

| id | Name | Condition | Slide (house voice) |
|---|---|---|---|
| `ending_holdfast_schedule` | **The Schedule Holds** | Levy honoured + membrane saved + Ormund's 12-C accepted in part | Some of yours live numbered in Block C. The bunker is easier to feed. The duty roster on the wall has names on it that are not the names that slept there. |
| `ending_holdfast_reserve` | **The Reserve** | 12-C enforced: Office takes unlisted labour, Sela claimed as allocated dependent if present | The ice takes a column south and a column north. Receipts in triplicate. Nobody is shot. |
| `ending_holdfast_dark_road` | **The Road Goes Dark** | Levy refused + Cutters withdraw + plant saved or not | District 8 continues without you. Forty empty apartments stay empty. Edor's return is found in a weigh-hut, incomplete, in a good hand. |
| `ending_holdfast_tender` | **Stand-Up** | *Hearth-4* opened without blasting; Mire's pad; people come ashore **or** you board | The Fleet stops being a rumour. The Cluster has to vote on beds. Migration/Icebreaker epilogues land **in a place**. |
| `ending_holdfast_white` | **The White** | Player uses snow-crawler to *leave* both districts | Existing Migration fade, now with a radio fragment from Foghorn 8 that does not ask them to come back. |

**TrueEnding terraformer / android / neuromancer content is not used.**

## Lore revelations (what standing there teaches)

1. The Quiet Evacuation's destination was **District 8**, not "the bunkers" as a class. Allocation 12 was a **local** overflow hole.
2. Reconstruction Order 12-C treated unlisted occupants of authenticated shelters as a labour pool. It was published. Nobody in Sector 4 had a copy that survived.
3. Water engineers were allocated *away from plants* into shelters. Frayne was right. District 8 has membranes and almost no one who can do *field* water — Halvard's 12-B kit is more useful than a Cluster apartment.
4. Sole and Ormund have the same Schedule. Completeness vs execution. Neither is lying.
5. The Fleet's stand-up authentication is the same family as D/9's, and Sole still cannot sign for a ship. Some paper only works on land.
6. The brass playground seats. The tin behind the filtration stack. Same metal. Same silence.

---

# SECTION 4 — QUEST DESIGN

Quest runtime: `QuestRuntime` / `QuestRegistry` / `QuestlineSO.Ids` (*existing*). New ids must be registered there at implementation. Types: `expedition`, `shelter`, `faction`, `personal`, `repeatable`.

**Word-count budget (quests only):** ~14,000 words main (objectives, stage text, choice bodies) + ~16,000 side. See §7 for full narrative budget.

---

## 4.1 Main questline (10)

| id | Name | Type | Prereqs | Synopsis | Objectives | Rewards | Time |
|---|---|---|---|---|---|---|---|
| `quest_holdfast_the_sheet` | The Sheet That Shouldn't | expedition | Day 90; Ostrowski met *or* Toll visited | Bram will not say who walked the estuary. The waxed paper shows a road. | 1. Buy or copy `item_map_sheet_ice_road`. 2. Compare to Kittiwake log if owned. 3. Ask a Lamplighter about Kilometre 19. 4. Survive the asking (Ivy will not cross; she will confirm the post exists). | Map fragment; travel-time hint; `knowledge_key: lore_hf_sheet` | 45–70 min |
| `quest_holdfast_the_clerk` | The Return | shelter / faction | Sheet **or** Day 110 | Edor Vale at the weighbridge with a census form. He offers to read it twice. | 1. Hear the form. 2. Confirm or deny three occupations. 3. Choose whether he may wait near the hatch. 4. Optional: show Sela's card if owned. | `item_census_return_blank`; Office awareness flag; Toll receipt | 30–50 min |
| `quest_holdfast_the_window` | When the Cut Takes | expedition | Clerk started; `IceRoadSystem` first window | Yara Holm opens the Gate. The window is 14 days. | 1. Outfit a 3-person run (warmth gear, iodine, food, `item_welders_glass`). 2. Cross to Waystation A. 3. Do not walk marked-dark ice. 4. Return or winter the last bunk. | Waystation unlock; Cutter access; freeze injury if underprepared | 90–120 min |
| `quest_holdfast_the_plant` | In Situ Essential | expedition | Waystation A | The desalination plant is staffed. Leva's minutes are current. | 1. Enter Grade Hut. 2. Tour Membrane Hall (rad + fume). 3. Deliver or refuse a resin gift. 4. See steam line toward Cluster. | Salt trade unlocked; `item_ro_resin_spent` sample; recast location flag | 60–90 min |
| `quest_holdfast_authentication` | Take a Number | expedition | Plant visited | Cluster Gatehouse. Allocation 12 is a known discrepancy. | 1. State a number or none. 2. Accept Block C or sleep the Gatehouse floor. 3. Walk the Quad. 4. Do not take brass from the playground (or do — silent). | Guest housing; Clinic access at a price; morale event | 50–80 min |
| `quest_holdfast_the_drawer` | The Drawer | exploration | Authenticated | Ormund opens the Sector 4 Schedule. Names you know. Names you buried. | 1. Read Sole. 2. Read Renn. 3. Search Frayne (absent). 4. Optional: ask about 12-C. | `knowledge_key: lore_hf_two_schedules`; Codex dump; stress on any parent | 40–60 min |
| `quest_holdfast_the_levy` | Reconstruction Pool | faction | Drawer | Three names. Thirty days. The ice will not wait for a better feeling. | 1. Review named survivors. 2. Honour / substitute / refuse. 3. If sending: kit them for salt + UV. 4. Inform remaining shelter (morale). | Branch flags; trade rates; possible companion lock-in | 40–70 min |
| `quest_holdfast_the_membrane` | Forty-Eight Hours | crisis | Levy resolved (any branch) or Day+14 in district | Membrane bank trips. Cluster steam clock starts. | 1. Diagnose (Leva). 2. Gather resin, brass, iodine, 2 workers. 3. Outfall shift (health). 4. Choose Sector 4 strip vs local salvage vs let drop. | Plant state; Cluster indoor °C; Rebuilders hegemony delta | 90–150 min |
| `quest_holdfast_the_second_list` | Order 12-C | story | Membrane done **or** levy refused | The labour reserve clause. Ormund will come south. | 1. Obtain a copy of 12-C. 2. Optionally carry it to Sole (she files, does not sign). 3. Optionally show Voss (he wants the pool). 4. Prepare hatch. | `item_order_12c`; threatening prose unlocked; Voss/Ormund triangle | 60–90 min |
| `quest_holdfast_the_hatch` | The Claim, Reversed | shelter | 12-C **or** Day 200 List Layer 5 | Forms at the outer hatch. Escort in faded Continuity jackets. Temperature. | 1. Open or keep shut. 2. If open: authenticate, house, or levy. 3. If shut: wait 40 days (quiet). 4. Write nothing on the duty roster, or write. | Ending flag; history second paragraph; possible `victory_the_holdfast` slide | 30–50 min |

**Main quest total player time:** ~8–12 hours including travel/needs management, not including side content.

---

## 4.2 Side quests (18)

### Hydro-Barons / The Salt (`hydro_barons`) — 3

| id | Giver | Location | Hook | Objectives | Rewards |
|---|---|---|---|---|---|
| `quest_salt_resin_count` | Leva Quist | Membrane Hall | The drum count is short every Tuesday. Nobody is stealing. The spent stack is growing. | 1. Audit drums. 2. Follow a night shift. 3. Find evaporation, not theft. 4. Recoat or write off. | `item_ro_resin` x2; Salt trust; recipe `recipe_resin_recoat` (low yield) |
| `quest_salt_outfall_limit` | Grade Hut minutes | Outfall | Shift limits exist on paper. They are not kept. Salt-rash is up. | 1. Work one limited shift. 2. Work one unlimited (or refuse). 3. Bring iodine protocol from Cluster Clinic or Ianov. | Affliction knowledge; Clinic/Salt friction; antiseptic |
| `quest_salt_brass_seats` | Leva | Grade Hut | Valve seats. Playground. Tin behind your filter. She will not ask where brass comes from. | 1. Deliver 8 `brass_fittings`. 2. Or deliver none and watch a leak scheduled. | Steam stability; silent nameplate flag; Works price shock |

### The Office (`faction_the_office`) — 3

| id | Giver | Location | Hook | Objectives | Rewards |
|---|---|---|---|---|---|
| `quest_office_missing_strip` | Quad noticeboard | Cluster Quad | Sector 4 trades listed as missing. One name is a survivor you have, living. | 1. Match the strip. 2. Tell them, or don't. 3. If told, they will file a retrieval. | Morale; possible retrieval event; Codex |
| `quest_office_school_sum` | School teacher (unnamed, `npc_cluster_teacher`) | Cluster School | Children adding RUR scores as homework. A dependent is worth points. | 1. Sit the lesson. 2. Correct a sum or let it stand. 3. Wren present: record what you tell her. | Morale split; Wren truth flag; no items |
| `quest_office_forty_rooms` | Ormund | Block C | Forty apartments kept for arrivals. Dusted. | 1. Walk three of them. 2. Find children's boots sizes 1–4 (mirrors your crate). 3. Leave them or take them. | `knowledge_key: lore_hf_forty_rooms`; morale; boots as warmth items if taken |

### The Cutters (`faction_the_cutters`) — 3

| id | Giver | Location | Hook | Objectives | Rewards |
|---|---|---|---|---|---|
| `quest_cut_dark_lamp` | Yara Holm | South Beacon | A beacon is dark during a window. Accident 12's cousin. | 1. Walk the dark stretch. 2. Relight or leave dark. 3. If relight for a trap, Yara withdraws (Corrigan rule, northern). | Road safety; or Cutters access lost |
| `quest_cut_accident_book` | Weigh Hut | Kilometre 19 | The accident book has a column that went through in year three. Cargo `ALLOC-7`. | 1. Read the book. 2. Salvage one crate or bury the marking. 3. Tell Cluster or Salt. | Alloc-7 rations; faction delta |
| `quest_cut_no_blast` | Yara | Gate | Someone (player or Office) wants explosives for the Shelf. | 1. Hear Yara's refusal. 2. Take the Ridge instead or blast anyway. | Icebreaker branch lock; Cutter trust |

### The Fleet (`faction_the_fleet`) — 3

| id | Giver | Location | Hook | Objectives | Rewards |
|---|---|---|---|---|---|
| `quest_fleet_schedule` | Foghorn 8 / radio | Shelf | A voice on a fixed frequency, fixed time. Not D/9's. | 1. Listen three nights. 2. Answer once with authentication or without. 3. Meet Mire ashore or not. | `Victory_Icebreaker` contact without ham-radio exploit; companion unlock |
| `quest_fleet_pad` | Halden Mire | *Hearth-4* | He wants a stand-up. Sole's D/9 form will not verify. | 1. Show Sole's paper if owned. 2. Find the Fleet annex in Ministry files. 3. Accept that some waits do not end. | Lore; no stand-down; Mire still comes ashore if asked as a person |
| `quest_fleet_boarding` | Mire | *Hearth-4* | Boarding without blasting. Hatch wants a number. | 1. Authenticate with an allocated companion or fail. 2. Inventory the living. 3. Offer Cluster beds or leave them. | Ending_tender progress; calories cost |

### Companion (3) + Sela (1)

| id | Giver | Location | Hook | Objectives | Rewards |
|---|---|---|---|---|---|
| `quest_comp_edor_dob` | Edor Vale | Hatch / Weighbridge | A date of birth on his own return is recorded twice. He notices, late. | 1. Compare to Convoy 12 lore. 2. Do not joke. 3. Let him correct it or leave the error. | Edor loyalty; he will lie once for you if you leave the error — and hate it |
| `quest_comp_leva_ashore` | Leva | Grade Hut | She has never been to the Cluster school. The indoors asked her to speak about water. | 1. Escort her. 2. She tells the truth about membranes. 3. Office unhappy; children less so. | Salt/Office friction; Leva morale |
| `quest_comp_yara_south` | Yara | Kilometre 19 | She will not cross. A Lamplighter is dark on the other side. | 1. Carry oil south. 2. Do not ask Ivy to come north. 3. Bring a receipt. | Cross-district etiquette; lamp oil economy |
| `quest_comp_sela_clinic` | Sela / Clinic | Cluster Clinic | They can claim her as Halvard's dependent. Iodine, school, a number. | 1. Let her hear it. 2. She chooses. 3. If she stays, Allocation 12 loses its water memory. | Ending modifier; clinic access; shelter grief |

### Exploration (3)

| id | Giver | Location | Hook | Objectives | Rewards |
|---|---|---|---|---|---|
| `quest_exp_dredger_moth` | none / Ostrowski | Dredger *Moth* | Charts that contradict the sheet. | 1. Board. 2. Pay in food. 3. Copy soundings. | Alternate Ice Road spur; Drown navigation bonus |
| `quest_exp_rtg_bruise` | none | Icebreaker convoy | The shine. 85 rads/h. Tungsten. Tracks. | 1. Timed loot. 2. Leave before dose window. 3. Optional Migration fragment. | `tracks_salvaged`; `tungsten_bar`; ARS risk |
| `quest_exp_forty_first` | none | Block C empty apt | A paper tag already written: a Sector 4 occupation, your shelter's. | 1. Find it. 2. Burn, return to Office, or keep. | Census threat; morale |

### Repeatable (2)

| id | Giver | Location | Hook | Objectives | Rewards |
|---|---|---|---|---|---|
| `quest_rep_ice_window_haul` | Yara / Office | Ice Road | Each freeze window: haul calories north, water/salt south. | 1. Fill a 3-crate manifest. 2. Cross during lit hours. 3. Weigh in. | DynamicEconomy prices; fatigue; Cutter credit |
| `quest_rep_steam_watch` | Leva | Steam substation | After membrane saved: weekly valve walk. | 1. Send 1 survivor 8h. 2. Pass a Utility AI "careful" check or cause a leak. | Heat credit for waystation; salt-rash chance |

---

# SECTION 5 — NEW GAMEPLAY SYSTEMS

**Cap:** 4 new plain-C# systems + 1 content-only vehicle extension. No LLM. Event-raising. Save-safe. Host-callback injection like `ShelterDegradationSystem`. Do **not** rebuild BilgePumps, OzoneScourge, WaterEconomy, Vehicle chassis, or WorldStateConsequence — **hook** them.

Confirmed Expansion V gaps this expansion is allowed to spend: Forward Outpost (as Waystation A only), Vehicle depth (ice tires), FalloutForecast (nice-to-have, **not required** — Ice Road forecast is narrower).

---

## 5.1 `IceRoadSystem`

**id:** `ice_road_system`  
**What it is:** A seasonal gate on `region_holdfast` travel. Not a minigame. A calendar with teeth.

**Mechanics:**
- Tick daily from `TimeSystem` + `WeatherSystem`.
- `ice_thickness_m` rises in Blizzard / IceStorm / BlackSnow / prolonged sub-zero; falls in FalseSpring, Rain, ThermalInversion thaws, Silence (clear UV eats the surface).
- Window opens when thickness ≥ threshold **and** Yara's beacons are lit (`cutters_access == true`).
- Window length 11–20 days, seeded.
- While open: Cut nodes traversable; warmth penalty −8°C extra; fatigue ×1.35; UV albedo multiplier on `OzoneScourgeSystem` for daytime ticks.
- While closed: Cut nodes blocked except Shallows boat (if Nomi available) at 1.6× travel hours and drowning/hypothermia checks.
- Dark beacon: that segment blocked even if ice is thick.
- Events: `OnIceRoadOpened`, `OnIceRoadClosed`, `OnBeaconDark(string locId)`, `OnAccidentLogged`.

**UI/UX:** Map screen: a thin white bar (the window). Nodes grey when dark. Diegetic: Yara's ledger, not a tooltip that says "DLC AREA LOCKED."

**Balance:** First window should not coincide with a fallout storm. Second window can. Never open year-round. If the player allied Cutters then betrayed (blast / trap lamp), next window is shorter, not gone, unless Yara withdrew permanently.

**Integration:** `ExpeditionSystem` node filter; `EventContext.WorldFlags["ice_road_open"]`; Flashpoint can force a premature thaw (`weather_event_trigger` already exists).

**Unrealistic (do not build):** real-time ice physics, destructible 3D sheets, co-op road maintenance.

---

## 5.2 `BrineWaterSystem`

**id:** `brine_water_system`  
**What it is:** District 8 water is plentiful and not potable. Inverts `WaterEconomySystem` without replacing it.

**Mechanics:**
- New water quality `Brine` in addition to irradiated / dirty / clean.
- Plant output: brine → `item_process_water` (thirst 40%, health risk if drunk raw) → clean if `item_ro_resin` + heat + `iodine_pills` (small).
- Cluster cisterns run on process water. Player waystation can buy barrels.
- Membrane integrity 0–100. Ticks down with load. Outfall shifts slow the drop. Resin repairs. Brass seats stop leaks (`brass_fittings`).
- If integrity < 15: `OnSteamTrip`. Cluster indoor °C interpolates toward outdoor over 48h. Waystation too if piped.
- Salt-rash: new **content** affliction only if it does not overlap `Affliction_TrenchFoot` / lead — propose `affliction_salt_rash` (skin, morale, iodine soothes not cures). **Hard cap still applies** — implement only after side-by-side vs 27 existing. If overlap, use existing skin/cold injury and **reskin the cause**.

**UI/UX:** A potability glyph on water stacks (already have irradiated vs clean). Plant integrity on Location Detail Panel when standing in Saltworks. 48h clock as a shelter-style bar **only while in District 8** — do not clutter Sector 4 HUD.

**Balance:** District 8 should never make Sector 4 thirst irrelevant. Transport loss: 20–30% of clean water hauling south. Rebuilders still need tablets. Player cannot pipe the plant to Allocation 12 (distance, D/9 denial, freeze).

**Integration:** `WaterEconomySystem.OnWaterStateChanged`; `DynamicEconomySystem` prices for resin/iodine/salt; Rebuilders `quest_rebuilders_thirst` remains mutually exclusive with cult purity — District 8 is a **third** water politics, not a lockout.

---

## 5.3 `CensusClaimSystem`

**id:** `census_claim_system`  
**What it is:** The Office's relationship model. Not hegemony. **Access + named claims.** Matches Currents design: you cannot conquer them; you can lose the Ice Road and gain a file.

**Mechanics:**
- Ledger of player survivors: occupation guess, listed/unlisted, score if known.
- Edor fills this by interview and by **looking** (tools, clinic behaviour).
- `LevyOrder`: up to 3 survivor ids, duration days, destination node.
- Honour / substitute / refuse as flags.
- While a levy is active, those survivors are not in the home shelter (needs tick at waystation or Cluster with different modifiers).
- `Order12C`: if active, Office may appear at hatch on a window (reuses hatch-dilemma constants in `ExpeditionSystem` — contamination vs morale — **do not retune without Prompt #26 discipline**).
- Threat is tone: `threateningFactionId: faction_the_office` (new) + `threateningTrustBelow`.
- Events: `OnCensusUpdated`, `OnLevyIssued`, `OnLevyResolved`, `On12CActivated`.

**UI/UX:** Diegetic document `item_census_return` in inventory / Lore Codex. Names. Occupations. Blank score column unless they found the formula. **No red "WANTED" stamp.**

**Balance:** Cannot kidnap the whole roster. Always 3. Always named. Always a window. Voss will try to intercept a levy column (conscription). Delacroix will vote if the column passes the Verge.

**Integration:** `NeedsSystem` while assigned away; `WorldStateConsequenceSystem` mutations:
- `mutation_ice_road_tax` (Warlords charge for the Gate if they smell District 8 traffic)
- `mutation_levy_column` (travel risk)
Do **not** add Office to `_hegemony` unless DTO/work is scheduled. Prefer Currents-style trust float on `NPC_TheOffice`.

---

## 5.4 `WaystationSystem` (Forward Outpost, scoped)

**id:** `waystation_system`  
**What it is:** One secondary camp at `loc_cut_waystation_a` (later optional `loc_cluster_block_c` guesting). Expansion V's outpost, **justified**.

**Mechanics:**
- 4 bunks, 1 stove, 1 filter (`air_filter` degrades 1.4× vs home — salt air).
- Shielding lower than home bunker. Radiation and UV both bite.
- Resupply required each window or the stove dies.
- Raid chance: low if Cutter access; higher if dark-road / 12-C hostile.
- Player may leave 1–2 survivors as watch (Utility AI: stoke, filter, sleep, panic).
- Does not replace home. Home still ticks `ShelterDegradationSystem`.

**UI/UX:** A reduced vitals strip when "focused" on the waystation (toggle on map). Not a second full HUD.

**Balance:** Cannot store the whole colony. If the player tries to migrate everyone here, hunger fails (no Verge). That is the point.

**Integration:** Reuse shelter module instances at smaller caps; `HatchDefenseSystem` analog is a **barred door**, not a hatch — simpler integrity.

---

## 5.5 Vehicle content-only (no new class if possible)

Extend `VehicleSystem` catalog:
- `vehicle_ice_tyre_set` — component; without it, Ice Road speed = walking and accident chance up.
- Snow-crawler (`vehicle_snow_crawler`, *existing Migration id*) can **arrive** at `loc_ice_road_gate` instead of ending the game, if the player chooses "go to District 8" at Phase 5. Flag: `migration_destination_holdfast`.

**Unrealistic:** a driving game on ice.

---

## Systems explicitly not in this expansion

- No new victory-path architecture beyond a flag/epilogue.
- No fifth Sector 4 Power.
- No livestock unless Verge seed quest needs a Cluster coop (content, not `LivestockHusbandrySystem`).
- No FalloutForecast unless spare sprint — Yara's ledger is the forecast.
- No `ShelterModule_SubBay` required; intake caisson can be a dangerous expedition until SubBay exists.

---

# SECTION 6 — NEW CHARACTERS & ENEMIES

## 6.1 Companions (4)

Not a Witcher party. Assignable to Ice Road expeditions and waystation watch. Each has Utility AI weight tweaks and a "will not" that cannot be bought off.

| id | Name | Role | AI bias | Will not | If they die |
|---|---|---|---|---|---|
| `npc_edor_vale` | Edor Vale | Census / guide | Prefer talk, wait, document | Enter hatch uninvited; falsify a DOB | 12-C still proceeds; tone goes colder (another clerk) |
| `npc_leva_quist` | Leva Quist | Plant / water | Prefer repair, outfall, refuse office meetings | Shut the plant for politics | Membrane integrity ticks faster; Salt will not forgive |
| `npc_yara_holm` | Yara Holm | Ice Road | Prefer wait, relight, refuse blast | Guide on dark ice | Next window shorter; accidents up |
| `npc_halden_mire` | Halden Mire | Fleet / radio | Prefer listen, authenticate, refuse explosives | Open *Hearth-4* for a raid | Icebreaker blasting becomes the only remaining path — and it is worse |

Sela is a **conditional fifth** (`npc_sela_renn`), not a starter.

**Utility AI:** add actions `Action_IceWatch`, `Action_CensusInterview`, `Action_MembraneShift`, `Action_RadioSchedule` scored from warmth, fatigue, morale, and faction access. Deterministic seed `_worldSeed + 808`.

---

## 6.2 Enemy / encounter variants (12)

Human danger in ASHFALL is **people in conditions**. No fantasy mutants. Fauna stays irradiated-animal, not demons. "Combat" = expedition encounter resolution already in `ExpeditionSystem.Encounters`.

| id | Name | Where | What they cost | Notes |
|---|---|---|---|---|
| `enc_census_escort` | Census escort | Cut, hatch | Ammo or time; morale if you shoot first | Polite. Armed. Not Garrison. Pell would recognise the manners and hate them. |
| `enc_office_auditor` | Meter auditor | Saltworks | Time, access | Not a fight. A delay. Threatening prose if 12-C hostile. |
| `enc_salt_shift_end` | Exhausted shift | Outfall | Medical, morale | They want your iodine. They will not rob if you explain. |
| `enc_salt_levy_refusal` | Workers who will not go indoors | Grade Hut | Labour, steam | Dangerous only if you try to force them for Ormund. |
| `enc_cutter_wrecker` | "Lucky we were close" | Dark ice | Cargo, health | Undertow grammar. Never proven. |
| `enc_uv_team` | Scoured survey | Shelf, daytime | Eyes, skin | Need `item_welders_glass`. Sun-Seekers will want their visors. |
| `enc_barge_crew` | Cargo-starved dock | Frozen barge | Food vs passage | Recast existing location. They are not a gore joke. |
| `enc_allocated_runner` | Cluster deserter | Cut | Shelter dilemma | An allocated person running *south*. Voss would call them a prize. |
| `enc_garrison_intercept` | Voss's northern patrol | Ice Road Gate | Hegemony | Only if levy column is moving. Existing military patrol, new place. |
| `fauna_brine_dogs` | Brine dogs | Outfall, pools | Health, ammo | Variant of `Fauna_IrradiatedDogs` — salt sores, less pack, more territorial. |
| `fauna_ice_crows` | Ice crows | Shelf | Morale, food | `Fauna_AshCrows` coastal. They wait on accidents. |
| `enc_fleet_watch` | Tender watch | *Hearth-4* | Authentication fail = denied, not slaughtered | If player blasts, this becomes a disaster encounter, not a boss fight. |

---

## 6.3 Crisis "bosses" (6) — multi-phase, not arenas

**Technically unrealistic:** Elden Ring boss HP bars, 3D arenas, 50-hour combat roster.  
**What we ship:** three-phase crises with changing needs, access, and prose.

| id | Name | Phases | Failure | Success looks like |
|---|---|---|---|---|
| `crisis_the_census` | The Census | 1 Notice (form) 2 Interview (named) 3 Levy 4 Escort at hatch | Lose Ice Road or lose people | A completed return you can live with |
| `crisis_the_membrane` | The Membrane | 1 Pressure drop 2 Brine in the hall 3 48h steam 4 Sector 4 strip or local | Cluster cold-death cascade (days, not a cutscene) | Plant at 40%+ integrity |
| `crisis_the_window` | The Window | 1 Freeze 2 Traffic 3 Thaw 4 Accident check | Expedition stuck north; waystation must hold | Column home before dark |
| `crisis_the_tender` | *Hearth-4* | 1 Schedule 2 Pad 3 Hatch number 4 Beds | Blasting kills the waiters; Yara leaves | People ashore or you aboard |
| `crisis_12c` | The Second Claim | 1 Paper 2 Column south 3 Hatch 4 Aftermath 40 days | Quiet refusal (card-in-the-ash cousin) | Terms that are Garrison-shaped or community-shaped |
| `crisis_two_schedules` | Sole / Ormund | 1 Copy 2 Drown boat 3 Filing 4 Unsigned | Voss intercepts the copy | Two complete records, no execution |

Ormund is not a final boss. If he dies (player-caused), the Office **continues**; a deputy files the same form. Killing him is possible, costly, and does not empty the forty rooms.

---

# SECTION 7 — ITEMS & REWARDS

Existing tools remain canonical: `dosimeter`, `geiger_counter`, `iodine_pills`, `anti_rad`, `hazmat_suit` (degrades faster in salt/UV), `water_filter`, `air_filter`, `item_welders_glass`, `item_lead_visor`, `brass_fittings`.

All item ids below **PROPOSED** except noted.

## 7.1 Themed sets (7)

| Set id | Pieces | Function |
|---|---|---|
| `set_cutter_kit` | `item_ice_spike_bar`, `item_beacon_oil`, `item_cutter_ledger_blank`, `item_ice_tyre_set` | Ice Road safety; accident chance down |
| `set_salt_shift` | `item_plant_suit_patched`, `item_resin_gloves`, `item_fume_rag`, `item_shift_whistle` | Outfall: salt-rash down, fatigue up (whistle enforces limits) |
| `set_office_paper` | `item_census_return_blank`, `item_order_12c`, `item_allocation_tag`, `item_triplicate_carbon` | Quest keys; morale when visible in shelter |
| `set_cluster_guest` | `item_work_ticket`, `item_steam_token`, `item_block_c_key` | Indoor access; warmth if steam live |
| `set_ro_process` | `item_ro_resin`, `item_ro_resin_spent`, `item_iodine_crystal`, `item_process_barrel` | Brine chain |
| `set_shelf_radio` | `item_schedule_crystal`, `item_fleet_pad_copy` (does not authenticate), `item_foghorn_key` | Tender path |
| `set_two_district` | `item_map_sheet_ice_road`, `item_kittiwake_copy` (*if chart copied*), `item_weigh_receipt_hf` | Travel |

## 7.2 Legendaries (12) — unique, not glowing

ASHFALL "legendary" = **one in the world, with a history**. No magic.

| id | Name | Where | What it does | Flavour (first line) |
|---|---|---|---|---|
| `item_schedule_sector4_copy` | The Other Schedule | Ormund's drawer | Codex unlock; 12-C path | Every name is legible. Including yours, in a column you were not meant to see. |
| `item_halvard_kit_notes` | Improvised Potable | 12-B / Sela | Water craft bonus at waystation | His handwriting gets smaller toward the end. The diagrams do not. |
| `item_sole_unsigned` | Filed, Not Signed | Sole, after 12-C | D/9 stand-down still works; Fleet pad still does not | She blotted the date. She did not blot the refusal. |
| `item_playground_seat` | One Seat | Cluster Quad | 1× `brass_fittings` that everyone notices | The chain is still there. The brass is in your pack. |
| `item_edor_return_self` | Clerk's Own Return | Edor quest | Once: he will omit a name | The birth year is written twice. Once correctly. |
| `item_yara_dark_mark` | Dark Mark | Yara, if you blasted | Ice Road access destroyed | She did not raise her voice. The beacon is dark. |
| `item_leva_minutes_vol12` | Volume 12 | Grade Hut | Steam trip warning 6h early | Motion: that we keep running. Carried. |
| `item_hearth4_hatch_log` | Hatch Log | Tender | Icebreaker without 100 explosives if authenticated | They logged every refusal. There are a lot of refusals. |
| `item_alloc7_ration_tin` | ALLOC-7 Tin | Accident 12 | Food; morale down if opened in Sector 4 | NOT FOR GENERAL ISSUE. The issue is you. |
| `item_cluster_formulary` | Human Formulary | Clinic | Ianov quest payoff; surgery odds | Dosage for a species the Verge has been approximating. |
| `item_foghorn_timer` | Foghorn Escapement | Foghorn 8 | Shelf navigation; Silence weather still dangerous | It sounds whether anyone is coming or not. |
| `item_tin_fourteenth` | The Fourteenth Plate | Player shelter | Only if you sell nameplates north | The tin is lighter. Nobody mentions it. |

## 7.3 Consumables (new)

| id | Effect |
|---|---|
| `item_ro_resin` | Converts brine→process; plant repair |
| `item_iodine_crystal` | Bulk iodine; thirst-process; thyroid |
| `item_salt_rash_salve` | Symptom relief (not rad) |
| `item_steam_token` | 8h waystation warmth (Cluster currency-in-kind) |
| `item_beacon_oil` | Relight; Cutter tithe |
| `item_uv_grease` | Skin UV blistering down 1 expedition |
| `item_electrolyte_salts` | Counters process-water drinking |
| `item_process_barrel` | Transport; 20% spoilage chance on Ice Road accident |

## 7.4 Cosmetics (diegetic, not shop skins)

No premium skins. Shelter/waystation **objects**:

- Continuity high-vis (faded) on a hook — morale +1 if Edor is present, −1 if a deserter is present
- Cluster cream paint patch on a bunker wall — argument event
- Foghorn heard faintly on Silence nights if `item_foghorn_timer` owned (audio: text + existing radio, **not** full VO)

## 7.5 Achievements (24)

Use `ach_*` ids. No jokes that break tone. No "kill 100."

| id | Name | Condition |
|---|---|---|
| `ach_sheet` | The Sheet | Obtain ice road map |
| `ach_window` | Lit | First Ice Road crossing both ways |
| `ach_dark` | Dark | Walk a dark segment and live |
| `ach_authenticated` | Authenticated | Enter Cluster with a number |
| `ach_discrepancy` | Discrepancy | Enter with none |
| `ach_drawer` | Two Schedules | Read Sole and Renn in the drawer |
| `ach_levy_honour` | Scheduled | Honour the levy as written |
| `ach_levy_refuse` | Unscheduled | Refuse 12-C |
| `ach_substitute` | Irregular | Substitute levy names |
| `ach_forty_eight` | Forty-Eight | Save steam |
| `ach_let_drop` | The Indoors | Let steam die |
| `ach_brass_quiet` | The Tin | Sell nameplates north |
| `ach_brass_kept` | Put Back | Find the tin, leave it |
| `ach_sela_clinic` | Dependent | Sela hears the claim |
| `ach_sela_stays` | Tunnel | Sela refuses Cluster |
| `ach_yara_ivy` | Two Ledgers | Complete Yara south oil run |
| `ach_mire_pad` | Say Again | Show Sole's form to Mire |
| `ach_tender_open` | Stand-Up | Open *Hearth-4* without blasting |
| `ach_tender_blast` | The Charges | Blast anyway |
| `ach_ianov_book` | Arithmetic | Deliver formulary to Ianov |
| `ach_frayne_notice` | No One Left Who Can Do Water | Tell Frayne about District 8 plant (she already knew the shape) |
| `ach_winter_watch` | Stove | Keep waystation through a closed window |
| `ach_haul_three` | Manifest | Three repeatable hauls |
| `ach_hatch_reversed` | The Claim, Reversed | Finish `quest_holdfast_the_hatch` |

## 7.6 Narrative word-count estimate (all new prose)

| Bucket | Words | Notes |
|---|---|---|
| Main quest stage/choice text | 12,000 | 10 quests × ~1,200 |
| Side quests | 16,000 | 18 × ~900 |
| Location `description` + lore bodies | 8,000 | ~30 nodes |
| NPC barks / threatening variants | 6,000 | Trust-reactive pairs |
| `world_history` + Codex | 5,000 | 15–20 entries |
| Item flavour | 2,500 | sets + legendaries |
| Radio (District 8 frequencies) | 2,500 | 12–18 clips, confidence scores |
| Ending slides + accident book | 2,000 | |
| **Total** | **~54,000** | Short-novel. Feasible. **Full VO of this = unrealistic.** Text + sparse radio acting only if ElevenLabs pipeline already exists (`EXTERNAL_AUDIO_REQUIREMENTS.md`). |

---

# SECTION 8 — TECHNICAL IMPLEMENTATION PLAN

## 8.1 Architecture mapping (Unity 6 LTS, 2D, URP)

| Concern | Existing pattern | Holdfast |
|---|---|---|
| Data | `StreamingAssets/Data/*.json` + JsonUtility-safe DTOs | `holdfast_factions.json`, `holdfast_locations.json` (or append `locations.json`), `holdfast_quests.json`, world_history append |
| Logic | Plain C# systems, events, save blobs | `IceRoadSystem`, `BrineWaterSystem`, `CensusClaimSystem`, `WaystationSystem` |
| Host | `GameBootstrap` partials + `SaveSystem.SetXxx` | `GameBootstrap.Holdfast.cs` |
| AI | `UtilityAI` + `ActionScorer` | New `SurvivorAction`s, no LLM |
| UI | UI Toolkit, Lore Codex, Location Detail | Ice Road bar; census document; waystation strip |
| Map | `GeneratedMap` nodes | `region_holdfast` tag, travelHours 6–14 |
| Economy | `DynamicEconomySystem`, `FactionSO.Ids` | Use `hydro_barons`; do not new-currency |
| Lore | `LoreDiscoveryIndex` | New `knowledge_key`s, `location_explore` |
| Quests | `QuestRuntime` / `QuestRegistry` / `QuestlineSO.Ids` | Register all `quest_holdfast_*` |
| Consequences | `WorldStateConsequenceSystem` | New mutations; **do not** put Office in `_hegemony` without a ticket |

**Faction catalog rule:** `holdfast_factions.json` follows `PhantomTriggerCatalogLoader` / proposed `currents.json` shape: `id, display_name, alignment, home_region, trust, wants[], offers[], signature_quote, access_rule`. No `relationships` dictionary.

**Ids namespace:** `loc_cut_*`, `loc_salt_*`, `loc_cluster_*`, `loc_shelf_*`, `faction_the_office`, `faction_the_cutters`, `faction_the_fleet`, `hydro_barons` (*existing*), `npc_*`, `quest_holdfast_*`, `enc_*`, `crisis_*`, `lore_hf_*`, `mutation_*`, `ending_holdfast_*`.

## 8.2 Asset list (specify only; generate later into `generated_AIassets/`)

Per visual DNA: dry-gouache, isolated objects, no readable AI text, no flags, no gore, no fantasy glow.

| Asset | Type | Notes |
|---|---|---|
| Location cards × ~30 | 2D illustration | Salt rime, prefab, ice road, tender hull |
| Faction badges × 3 | Badge | Office (boring civil), Cutters (high-vis), Fleet (funnel mark abstract). Hydro-Barons: check if orphan badge exists; else new |
| NPC portraits × 6 | Chest-up, deferred if no UI destination | Ormund, Edor, Leva, Yara, Mire, teacher |
| Items × ~40 icons | Inventory 64–128 px | Resin drum, census paper, ice spike, faded high-vis |
| Waystation UI | UITK | Reduced vitals |
| Ice Road map overlay | UITK | White bar, dark nodes |
| Radio clips × 12 | Optional audio | Schedule, foghorn, plant tannoy. Text fallback mandatory |
| **Not in scope** | 3D coast, VO for 54k words, new music album (reuse ash ambience + one foghorn motif) | |

## 8.3 Sprints (4 × 3 weeks) — *vertical slice to content-complete core*

Honest: a Blood & Wine *volume* is not four sprints for a small team. These four sprints ship a **playable district loop**. Remaining catalog (side quests 10–18, Shelf, polish) rides the 3–6 month roadmap in §9.

| Sprint | Goal | Deliverables | Verify |
|---|---|---|---|
| **S1 — Ice & paper** | Gate works | `IceRoadSystem` + `CensusClaimSystem` stubs; `loc_ice_road_gate`, Kilometre 19, Waystation A; quests sheet/clerk/window; Edor + Yara; JSON locations; Lore keys | EditMode tests: window open/close, save roundtrip, levy flags. Unity batch compile **PASS** |
| **S2 — Salt & steam** | Inversion works | `BrineWaterSystem`; recast desalination; Leva; membrane crisis; brass/iodine economy; waystation vitals | Water potability tests; 48h clock; compile PASS |
| **S3 — Cluster & claim** | Story works | Cluster POIs; Ormund; drawer; levy; 12-C; hatch reversed; threateningBodyText pairs; Codex tab | Quest registry ids unique; hatch dilemma reuse; compile PASS |
| **S4 — Shelf & endings** | Destinations work | *Hearth-4*, barge recast, convoy loot; Mire; four ending flags; Migration destination hook; Icebreaker contact hook; 8 side quests (Salt/Office/Cutters) | Ending flags exclusive; compile PASS; PlayMode: one window loop |

**QA (all sprints):** needs still tick at home while north; filters degrade; no 7th `faction_lore` DTO surprise; namespace aliases (`iron_garrison` vs `faction_central_garrison`) not worsened.

## 8.4 Risks

| Risk | Mitigation |
|---|---|
| Faction DTO / Codex overflow | Separate catalog. Never a 7th Power in `faction_lore.json` without a dedicated task |
| Scope blow-up (50 POIs + 5 systems + VO) | Cap 4 systems. POIs above can be description-only. No VO |
| Membrane crisis feels like a new genre | Keep it expedition + inventory + timer, same as filter_failure |
| Ice Road is a loading screen | Accidents, weigh huts, dark lamps, UV — the road is content |
| Sela / Day 200 desync | Hatch reversed must play **without** Sela; Sela only modifies |
| `Victory_TrueEnding` lore leak | Do not reference terraformers |
| Tessarat ids | Do not use |
| Performance | Node graph, not entities. 30 location cards. UITK already the bottleneck — no extra world renderer |
| Cross-tool QA rule (≥2 coupled variables) | Ice thickness × beacon state × levy column is **three**. Implementer ≠ reviewer (Prompt #26). Reviewer sees diff + this spec only |

## 8.5 Performance budget

- Ice Road tick: O(nodes in region) daily, not per frame
- Census ledger: O(survivors) ≤ 14
- No new realtime physics
- Location art: 1024² max, atlas where possible
- Waystation: do not instantiate a second full `Shelter` simulation if a reduced `WaystationState` will do

## 8.6 QA cases (minimum)

1. Old save → Ice Road dark → sheet quest → window opens  
2. Window closes while expedition is on Cut → stuck-north / boat fallback  
3. Levy honour: 3 survivors absent from home needs, present at Cluster  
4. Membrane trip: Cluster °C drop; home bunker unchanged  
5. Refuse 12-C: no combat; 40-day quiet; receipt in ash  
6. Nameplates sold: no morale event  
7. Yara dark-lamp trap: Cutters withdraw; lamps out 11 days  
8. Migration Phase 5 → Holdfast instead of fade  
9. Icebreaker contact via Foghorn without explosives  
10. `hydro_barons` trade works without Codex relationship field  
11. Compile + EditMode PASS before "done"

---

# SECTION 9 — PLAYER ENGAGEMENT & RETENTION

## Day-one (post-unlock)

- Ostrowski or Toll: the sheet. A **place that should not be on a Sector 4 map**.
- First window is a event, like a fallout storm: you feel it in warmth and calendar, not a DLC popup.
- First Cluster night: numbered bunks, painted queues, playground chains. Show, don't preach.

## 3–6 month roadmap (after S4 core)

| Month | Content | Why they return |
|---|---|---|
| M1 | Remaining side quests (Fleet, companion, repeatable hauls); radio pack; accident-book entries | Ice Road is a seasonal loop — players already wait windows |
| M2 | Long Walk visits District 8 (11-month circuit *existing Current*); Grain Exchange price shock from northern calories | Cross-Current interlock |
| M3 | Optional: SubBay intake dive **if** `ShelterModule_SubBay` ships; otherwise description-only | Don't block on ghosts |
| M4–6 | Community: census-return sharing (screenshots of *their* named levy — generated from live survivor list, not user-identifiable). No live service map. Seasonal second winter modifiers (thinner ice) as a **data** SeasonProfile, not a new executable | Retention = the window, not battle-pass |

## Community

- Shareable: accident book lines, levy names (procedural from *their* roster), ending second paragraphs.
- Do not ask players to vote which ending is canon.
- Mod-friendly: JSON catalogs first.

## Monetization

**Not applicable unless the owner decides the expansion is paid DLC.** If paid: one purchase, no cosmetics gacha, no loot boxes, no "iodine pills microtransaction." If free: still gate on mid-game so new players aren't dumped in brine.

## Feedback loops

| Loop | Need served |
|---|---|
| Haul calories north / water south | Hunger vs thirst inversion |
| Window calendar | Fatigue, planning, weather |
| Levy | Morale, labour, identity |
| Membrane | Warmth (steam), health (fumes), radiation (hall) |
| Hatch reversed | The List payoff — same emotional machine as Sela, inverted |
| Brass silence | Continuity of Sector 4's quietest object |

---

# SECTION 10 — LORE CONSISTENCY CHECK

## 10.1 What this expansion must not contradict

| Canon | Source | Holdfast stance |
|---|---|---|
| Sector 4 map closed; no fifth Power | `00_OVERVIEW.md` | District 8 is another district. Office is not a Sector 4 Power |
| Player bunker = Allocation 12, unlisted occupants | `02_THE_LIST.md` | 12-C explains the Office's claim; does not make the player allocated |
| Sela's card is genuine; four hatch branches | `02_THE_LIST.md` | Hatch reversed is a **second** hatch scene, not a rewrite |
| Sole files, score 41.2, not allocated | `02_THE_LIST.md` | She is in Ormund's drawer as NOT ALLOCATED. He will not "fix" her |
| Frayne: no water engineers on the surface | `06_*.md` | They were allocated into holes **and** not posted to the plant. Plant staff were in-situ essentials (Leva). Both true |
| D/9 hostile to everyone; stand-down ≠ ally; traps stay | `06_*.md` | Fleet pad ≠ D/9 pad. Sole cannot stand down a ship |
| Cult glow is not divine; game never adjudicates | `00_OVERVIEW.md` | District 8 treats rad as industrial. Cult may call the Shelf consecrated. No adjudication |
| Lamplighter rule: no exception | `05_FACTIONS.md` | Yara's dark-lamp rule is the cousin, not a contradiction. Ivy does not cross Kilometre 19 |
| Rebuilders medical supply / brass | `06` + code | Demand stacks. Nameplates still uncommented |
| Hydro-Barons = Sector 8 water authority | `FactionSO.Ids` | Reclaimed, not replaced |
| Gazetteer sea to the north | `01_GAZETTEER.md` | District 8 is that coast |
| 15 victories, no new affliction crowding | lore overview / Expansion V | Endings are flags; salt-rash only if distinct |
| No magic, no real countries/people, no glorified violence | `AGENTS.md` | Held |

## 10.2 Explicit retcons (small, justified)

| Item | Change | Why |
|---|---|---|
| `location_abandoned_desalination` description | Occupied, failing, Hydro-Baron seat | The word "Abandoned" is what Sector 4 believed. Located knowledge. |
| `location_frozen_river_barge` "cannibalistic dockworkers" | Cargo-starved crew, trade for passage | Tone. Existing line is a different game. |
| `Victory_Icebreaker` flavour | Tender *Hearth-4* with people; blasting optional and condemned | Skeleton victory becomes a place. Numbers (30 nodes, 100 explosives) can remain as the **stupid plan** Yara refuses |
| `Victory_Migration` Phase 5 | Optional destination Holdfast | Fade-to-white was a missing district, not a philosophy |

**Not retconned:** TrueEnding terraformer (ignored). Tessarat names (ignored). Sector 7G easter egg (untouched). Android / Wire-Heads (untouched).

## 10.3 Timeline

| When | Event |
|---|---|
| Exchange−4Y | Office of Continuity (*existing*) |
| Exchange−3Y | Bunker Boom; Cluster 7 construction; desalination upgrade (Water Wars plant) |
| Exchange−1M | Quiet Evacuation **north** to District 8; Convoy 12 held on DOB (*existing*) |
| Exchange+0 | Cluster authenticates. Allocation 12 hatch on standby. Plant stays up (in situ) |
| Exchange+3W | Drown lock fails (*existing*); estuary begins the Cut |
| Exchange+2Y | Some Fleet come ashore; *Hearth-4* does not |
| Exchange+4Y | Ice Road regularised by unlisted pilots (Yara's cohort) |
| Exchange+5Y | **Now.** Census of Sector 4 reconstruction pool. Player discrepancy. |

## 10.4 Base-game references (use them)

Ostrowski, Ivy Corrigan, Nomi Fisk, Margit Sole, Sela Renn, Ottilie Frayne, Anneke Ruhl (pad comparison only), Colonel Voss, Delacroix, The Tollman, The Vessel (do not resolve), Ianov, Wren, Cold Count, Undertow grammar on dark ice, Lamplighter rule, Rebuilders minutes, brass tin, Kittiwake chart, Shallows etiquette, `OzoneScourgeSystem`, `WaterEconomySystem`, `Victory_Migration` / `Victory_Icebreaker`.

## 10.5 Remaining contradictions in *base* data (do not worsen)

- Lore vs systems faction namespaces  
- `world_history` "China" / "nations" vs AGENTS.md no real countries — **do not add more**  
- Icebreaker convoy vs train vs sub — this bible separates convoy / tender  
- Expansion II/III FactionSO sci-fi names — unused here  

## 10.6 Word to the implementer

If a system wants a seventh Codex relationship or a 16th victory class, **stop and ticket it**. The expansion is a district, a road, a plant, a form, and a hatch. That is enough.

---

# APPENDIX A — Proposed id checklist (collision notes)

Verified non-colliding against `locations.json` / `locations_expansion3.json` / `QuestlineSO.Ids` samples at time of writing. Re-grep before commit.

**Existing reused:** `hydro_barons`, `location_abandoned_desalination`, `location_crashed_icebreaker_convoy`, `location_frozen_river_barge`, `loc_the_shallows_market`, `loc_weighbridge`, `brass_fittings`, `iodine_pills`, `item_welders_glass`, `vehicle_snow_crawler`, `victory_icebreaker`, `victory_migration`.

**New (selected):** `expansion_the_holdfast`, `region_holdfast`, `faction_the_office`, `faction_the_cutters`, `faction_the_fleet`, `npc_cael_ormund`, `npc_edor_vale`, `npc_leva_quist`, `npc_yara_holm`, `npc_halden_mire`, `loc_ice_road_gate`, `loc_cut_waystation_a`, `loc_cluster_office`, `loc_shelf_hearth4`, `quest_holdfast_the_hatch`, `item_order_12c`, `lore_hf_two_schedules`, `mutation_levy_column`, `ending_holdfast_schedule`.

Full lists live in §§2–7.

---

# APPENDIX B — Next prompt (implementation)

> Implement Sprint 1 of `docs/expansions/expansion_the_holdfast_plan.md`: `IceRoadSystem` + `CensusClaimSystem` (plain C#, events, save/load), JSON locations for the Cut (gate, km 19, waystation A), quests `quest_holdfast_the_sheet` / `_the_clerk` / `_the_window`, NPCs Edor Vale and Yara Holm. Do not add a 7th faction to `faction_lore.json`. Register new quest ids in `QuestlineSO.Ids`. Verify with Unity batch compile and EditMode tests. Re-grep all new ids for collisions first.

---

# APPENDIX C — House-voice samples (location descriptions, shippable)

**`loc_ice_road_gate`**
> A boom laid across ice that was a shipping cut. Someone has painted a queue line. It has been repainted. The ledger is axle weights and dates and a column for "remarks" that is almost never used, and is used, when it is used, for the dead.

**`loc_cluster_quad`**
> Hydroponic troughs along the south wall. Four cultivars, two of them yellow. A playground with chains and no seats. The noticeboard has a labour rota and a strip of missing trades. One of the trades is yours.

**`loc_shelf_hearth4`**
> A tender, still upright. A light in the authenticator that has no reason to still be a light. The ice has come up to the Plimsoll mark and stopped, as if it were waiting for the same order the people inside are waiting for.
