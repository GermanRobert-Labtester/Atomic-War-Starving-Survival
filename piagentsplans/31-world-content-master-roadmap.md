# ASHFALL — World Content Master Roadmap

> **Scope:** A world-content multiplication plan that takes the existing Kimi K3 / Pi-Agent
> roadmap (`00-master-roadmap.md` + plans `06`–`30`) as a baseline and expands ASHFALL into a
> denser, more believable, interconnected game world.
>
> **Method:** Forensic inspection of `Assets/Ashfall.Core/`, `src/`, `Assets/StreamingAssets/Data/`
> (411 JSON files, ~284 location records, 159 items, 77 events, 36 NPCs, 19 factions, 50 radio
> broadcasts), `piagentsplans/00-master-roadmap.md`, and live system/data cross-referencing.
> All counts in this document were verified against the repository on 2026-08-30.
>
> **Bias:** ~75% content/data/narrative/world work, ~15% integration/wiring, <10% architecture.
> Every plan reuses existing systems before inventing new ones. New systems are rare and always
> flagged `NEW SYSTEM JUSTIFICATION REQUIRED`.

---

## 1. Executive assessment

ASHFALL is **system-complete but content-thin at specific seams**. The migration to Godot is
done, the Core library carries zero engine references, the test suite is green, and ~30
checksummed save stores work. The bottleneck is no longer engineering — it is **world density**.

Three structural weaknesses dominate:

1. **Eight live systems ship with no data catalog at all.** `SkillProgressionSystem` (47 skills
   hardcoded in `SkillDef.cs`), `ResearchSystem` (15 nodes hardcoded in `RegisterDefaults`),
   `WildlifeTrappingSystem`, `ExcavationSystem`, `SkyLayerArmorSystem`,
   `OrbitalHarrowTelemetrySystem`, `LedgerDebtSystem`, and `ShelterAssignmentSystem` are all
   fully implemented, wired into `GameBootstrap`/`Main.cs`, and save-supported — but they read no
   JSON and generate everything from runtime parameters. They are invisible content multipliers
   waiting for data.

2. **The world map is a skeleton.** `wasteland_map_v1.json` has **6 nodes / 7 routes**. Against
   ~284 location records across nine catalog files, almost none are placed on a navigable graph.
   Expeditions, caravans, and faction territory have almost no spatial substrate to act on.

3. **Locations lack interior identity and loot ecology.** The 115 entries in `locations.json` are
   richly named (`location_acoustic_testing_facility`, `location_the_sump_cathedral`) but most
   are single-record points with no sub-zones, no location-specific scavenging tables, no
   resident groups, and no environmental-storytelling bundles. The world reads as a list of
   labels, not a set of places.

The good news: the systems to fix all three — `ExpeditionSystem`, `EconomySystem`,
`TradeTellEngine`, `WeatherSystem`, `TacticalCombatSystem`, `SignalIntelligenceCatalog`,
`JournalSystem`, `CatalogIntegrityValidator` — already exist and are data-driven. The work
below is overwhelmingly **authoring content that flows through live systems**.

---

## 2. Existing roadmap preservation

The existing `00-master-roadmap.md` (20 ranked steps) and plans `06`–`30` (25 files × 3 tasks =
~75 tasks) remain high-value. This roadmap **does not discard them**. The table below marks which
existing plans are preserved as-is, which are superseded by a deeper plan here, and which are
folded into a new cross-system chain.

| Existing plan | Status | Notes |
|---|---|---|
| `01` Needs/Radiation save round-trips | **PRESERVE (P0)** | Prerequisite for all content; do first |
| `02` Loader bare-catch hardening | **PRESERVE (P0)** | Prevents silent content loss |
| `03` schema_version sweep | **PRESERVE (P0)** | Establish convention before scaling |
| `04` Relic blueprints 6→30 | **PRESERVE → extend by W49** | W49 adds component catalog + multi-stage chains |
| `05` Vinyl catalog 1→20 | **PRESERVE → extend by W38** | W38 adds individual record definitions |
| `06` Narrative trilogy | **PRESERVE** | Final wishes, echoes, faction war |
| `07` Audio production | **PRESERVE** | Audio is orthogonal to world content |
| `08` Visual art | **PRESERVE** | Art follows content; schedule after locations land |
| `09` Medical depth | **PRESERVE → deepen by W36** | W36 adds outbreak world-events |
| `10` Combat/expedition depth | **PRESERVE → deepen by W29** | W29 adds road/vehicle encounter pack |
| `11` World exploration | **SUPERSEDED in part by W1–W10, W15** | Location families + excavation sites |
| `12` Social/shelter life | **PRESERVE → deepen by W19** | W19 adds room-definition data |
| `13` Economy loop | **PRESERVE → deepen by W14** | W14 adds trapping species/bait data |
| `14` UX/onboarding | **PRESERVE** | Not a world-content task |
| `15` Endgame/meta | **PRESERVE → chain by W41** | W41 wires campaign-band events into epilogue |
| `16` Cartography 6→60 | **PRESERVE → executed by W29** | W29 is the data execution |
| `17` Env-storytelling | **PRESERVE → deepen by W20, W25** | W20 loot identity, W25 documents |
| `18` Expansion deepening | **PRESERVE** | Holdfast/Standing/Crossing/Verdict quests |
| `19` Dynamic world | **PRESERVE → deepen by W17, W48** | W17 telemetry events, W48 weather gating |
| `20` Wasteland inhabitants | **PRESERVE → deepen by W44** | W44 recurring-NPC temporal arcs |
| `21` Phantom/memory | **PRESERVE → executed by W31, W32** | Data execution of triggers/secrets |
| `22` Foundry/greenhouse | **PRESERVE → executed by W33, W34** | Data execution |
| `23` Maritime | **PRESERVE → executed by W39** | Dive-site data execution |
| `24` Radio | **PRESERVE → chain by C7** | C7 unified broadcast schedule |
| `25` Faction ecology | **PRESERVE → deepen by W43** | W43 territorialization & patrols |
| `26` Knowledge/research | **SUPERSEDED by W12, W13** | W12/W13 are the data-authority execution |
| `27` Body/mind | **PRESERVE → executed by W35** | Data execution |
| `28` Wildlife ecology | **PRESERVE → deepen by W14** | W14 species catalog feeds migration |
| `29` Shelter as character | **PRESERVE → deepen by W19** | W19 room definitions |
| `30` Ritual/faith | **PRESERVE → deepen by W46** | W46 specific cult/movement content |

**Net:** ~30 existing plans preserved, ~10 superseded/deepened by new plans here, ~50 genuinely
new plans added. Total active plans: **80**.

---

## 3. Content saturation map

| Domain | Status | Evidence |
|---|---|---|
| Shelter (interior systems) | HEALTHY | 30 save stores, assignment/schedule/memorial live; **room-definition data THIN** |
| Surface exploration | THIN | 115 named locations but no interior zones, no loot identity, 6-node map |
| Underground exploration | VERY THIN | `ExcavationSystem` has no site data; ~5 subterranean loc_ ids only |
| Factions | HEALTHY | 19 factions with lore; **territorial/patrol behavior THIN** |
| Settlements | VERY THIN | No settlement catalog; 6 map nodes only |
| Wilderness | THIN | 11 wildlife packs, 10 landmarks in `world_evolution_seeds.json` |
| Quests | HEALTHY | ~190 quest records across catalogs; **multi-stage chains THIN** |
| NPCs | THIN | 36 characters, mostly one-state; **temporal continuity absent** |
| Combat | HEALTHY | 5-lane tactical system, catalog present; **enemy behavior variety THIN** |
| Noncombat encounters | THIN | 77 events, mostly combat/scavenge; negotiation/medical/engineering rare |
| Weather | HEALTHY | 22 states, forecasting live; **world consequences THIN** |
| Radio | HEALTHY | 50 broadcasts + 13 corpus + distress signals; **scheduling THIN** |
| Scavenging | VERY THIN | No location-specific loot tables; generic `items.json` pulls |
| World history | HEALTHY | 272 narrative files, rich env-storytelling corpus |
| Collectibles | VERY THIN | 1 vinyl item, 4 cassette sets; no photo/poster/badge catalogs |
| Long campaign progression | THIN | Generational/lineage systems live but no band-specific events |

---

## 4. Underused live systems — content opportunities

Verified on 2026-08-30. These are the highest-leverage content multipliers: fully implemented,
wired, save-supported, but reading **no data** or **hardcoded content**.

| # | System | Core file | Data status | Content opportunity |
|---|---|---|---|---|
| U1 | `SkillProgressionSystem` | `Survivors/SkillProgressionSystem.cs` | `skills.json` **MISSING**; 47 skills hardcoded in `SkillDef.cs` | Externalize 47 → JSON; add 15 action skills with real XP thresholds |
| U2 | `ResearchSystem` | `Research/ResearchSystem.cs` | No JSON referenced; 15 nodes hardcoded in `RegisterDefaults()` | Create `research_catalog.json`; expand 15 → 40 nodes across 6 disciplines |
| U3 | `WildlifeTrappingSystem` | `WildlifeTrappingSystem.cs` | No catalog loader; species/bait/yield all runtime params | Create `wildlife_trapping_catalog.json` (15 species, 8 baits, yield/toxin tables) |
| U4 | `ExcavationSystem` | `ExcavationSystem.cs` | No catalog loader; sites via runtime `AddSite()` | Create `excavation_sites.json` (10 predefined sites with room blueprints) |
| U5 | `SkyLayerArmorSystem` | `Shelter/SkyLayerArmorSystem.cs` | No catalog; armor state runtime-only | Create `sky_layer_armor_catalog.json` (6 reinforcement tiers, material costs) |
| U6 | `OrbitalHarrowTelemetrySystem` | `OrbitalHarrowTelemetrySystem.cs` | No event catalog; string interpolation only | Create `orbital_harrow_events.json` (12 escalating impact events) |
| U7 | `LedgerDebtSystem` | `LedgerDebtSystem.cs` | No contract templates; runtime-only | Create `ledger_debt_templates.json` (10 templates, faction-specific) |
| U8 | `ShelterAssignmentSystem` | `Shelter/ShelterAssignmentSystem.cs` | No room definitions; runtime `CreateDefault()` | Create `shelter_rooms.json` (12 room types with capacity/comfort) |

**Thin catalogs (exist, few entries)** — second-tier opportunities, covered by preserved plans +
new data-execution plans: `relic_recipes` (6), `cassette_sets` (4), `trade_tell_lines` (60, 3 per
combo), `dive_sites` (4), `foundry_accords` (4), `archive_inks` (3), `autopsy_procedures` (3),
`dose_registers` (4), `muster_witnesses` (3), `wasteland_map` (6 nodes), `shelter_schedules` (3),
`phantom_triggers` (7), `confession_secrets` (8), `greenhouse_items` (14), `foundry_production`
(11), `chemical_dependency_items` (13), `disease_catalog` (7).

---

## 5. World expansion pillars

Eight content pillars organize the new plans. Each pillar is a region of the world that several
plans build toward simultaneously, forming a world graph rather than a content list.

| Pillar | Plans | What it builds |
|---|---|---|
| **P1 — Ruined Urban Belt** | W1, W2, W3, W4, W11 | Apartment blocks, hospital, metro, civic district, micro-locations |
| **P2 — Frozen Rural Hinterland** | W8, W11, W24 | Villages, farms, monasteries, famine crisis, roadside finds |
| **P3 — Industrial Corridor** | W5, W23, W27 | Chemical/steel/power sites, fuel shortage, rail recovery |
| **P4 — Subterranean Infrastructure** | W9, W15, W17 | Deep mines, tunnels, excavation sites, telemetry events |
| **P5 — Military Frontier** | W6, W47 | Checkpoints, depots, front lines, hidden-bunker competition |
| **P6 — Scientific Cluster** | W7, W13 | Labs, weather stations, research tree externalization |
| **P7 — Faction Territorialization** | W43, W45, W25, W18 | Patrols, allegiance shifts, smuggling, debt |
| **P8 — Humanitarian & Crisis** | W22, W24, W36, W46 | Refugee columns, famine, disease outbreaks, cults |
| **P9 — Signal & Mystery** | W17, W48, C1–C30 | Telemetry, weather gating, cross-system decode chains |
| **P10 — Long-Term Society** | W12, W19, W41, W44 | Skills, rooms, campaign bands, recurring NPCs |

---

## 6. Implementation plans

> **Numbering:** W1–W50 are **new** plans authored here. The preservation table in §2 carries the
> existing ~30 plans. Together they form the 80-plan roadmap. Each new plan follows the required
> TASK format. Categories: WORLD / LOCATION / QUEST / ENCOUNTER / FACTION / NPC / ITEM /
> SYSTEM-CONTENT / NARRATIVE / EXPEDITION / INTEGRATION.

---

## TASK W1 — Urban Residential Belt: Apartment Blocks & Tenements

**Category:** LOCATION
**Priority:** P1
**Player Value:** VERY HIGH
**Implementation Risk:** LOW
**Estimated Size:** LARGE

### Why this matters
`locations.json` has `suburban_house` and a handful of urban names but no **interiorized
residential** locations. Apartment blocks are the most common pre-war structure and the natural
first-scavenge target. They currently have no sub-zones, no loot identity, and no resident
evidence. This is the single biggest "the world feels empty" gap in the urban tier.

### Existing systems reused
`locations.json` (115 entries, `loc_` ids), `ExpeditionSystem` (dispatch + travel ticks),
`EconomySystem` (scavenging yields), `JournalSystem` (discovery entries), `events.json`
(77 events), `environmental_atmosphere_expansion.json`, `CatalogIntegrityValidator` (loc_ prefix).

### World-content addition
* 5 apartment-block locations (exterior + 3–4 interior zones each = ~20 sub-zones)
* 8 location-specific scavenging table entries (household goods, canned food, personal documents)
* 6 environmental-storytelling bundles (evacuation lists, family photos, sealed doors)
* 4 encounters (trapped resident, rival scavengers, structural collapse, contaminated water)
* 3 unique loot items (pre-war id cards, ration cards, a child's backpack)
* 1 short quest (the sealed apartment)

### Gameplay loop
`Expedition dispatched → exterior approach (weather hazard) → floor-by-floor scavenging with
per-room loot tables → encounter trigger (trapped resident or collapse) → moral decision
(help/loot/ignore) → journal entry + unique item → later: resident or rival remembers the choice`

### Content specification
* `loc_apartment_block_krasny`, `loc_tenement_block_4`, `loc_highrise_shell`,
  `loc_prewar_condominium`, `loc_workers_dormitory`
* Each: `zones[]` (lobby, stairwell, 2–3 apartment units, roof), `loot_identity`,
  `hazard` (fire damage / collapsed floor / contaminated water tank), `resident_evidence`
* Encounters: `enc_trapped_elder_tenement`, `enc_rival_scavengers_highrise`,
  `enc_floor_collapse_dormitory`, `enc_contaminated_tank_condominium`
* Quest: `quest_sealed_apartment_krasny` (5 stages: rumor → key → entry → discovery → consequence)

### Integration points
`locations.json` (add 5 records), `events.json` (add 4), `items.json` (add 3 with `item_` prefix),
`narrative/` (add 6 environmental docs), `questline_master.json` or `moral_choice_quests.json`
(add 1 chain), flags `flag_sealed_apartment_opened`, `flag_tenement_resident_saved`.

### Substeps
1. Inventory existing `loc_` residential ids; confirm none are interiorized; pick 5 snake_case ids.
2. Define the zone schema by inspecting an existing multi-zone location in `locations.json` (e.g. `location_arcology_sector_4`).
3. Author 5 location records with `zones[]`, `loot_identity`, `hazard`, `resident_evidence` fields.
4. Add 8 scavenging-table entries keyed to `loc_` ids in a new `scavenging_tables.json` (see W20).
5. Author 6 environmental documents in `narrative/` (evacuation list, family photo set, sealed-door notice, ration card, maintenance log, child's drawing).
6. Add 4 encounters to `events.json` with `trigger`, `choices`, `requirements`, `outcome`.
7. Add 3 items to `items.json` (`item_prewar_id_card`, `item_ration_card_booklet`, `item_childs_backpack`).
8. Author the 5-stage quest `quest_sealed_apartment_krasny` in `moral_choice_quests.json`.
9. Wire 2 persistent flags through `InMemoryFlagLedger`; verify case-insensitive key handling.
10. Run `--data-integrity-selftest`; add a reachability test for the quest via `ashfall-dialog-graph-lint`.

### Acceptance Criteria
5 locations resolve in `CatalogIntegrityValidator`; 4 encounters fire from `events.json`; quest
plays end-to-end headless; 6 narrative docs load; data-integrity selftest reports 0 errors.

### QA / Validation
`--data-integrity-selftest`, `ashfall-dialog-graph-lint` (quest reachability), `ashfall-narrative-continuity`,
snapshot diff if a panel surfaces the quest.

### Follow-on opportunities
1. W2 hospital references a tenement evacuation route. 2. W3 metro connects to a highrise basement.
3. W11 micro-locations scatter around the residential belt. 4. W44 recurring NPC "the locked-out
tenant" reappears. 5. W22 refugee column passes through the dormitory.

---

## TASK W2 — Regional Hospital Complex Expansion

**Category:** LOCATION
**Priority:** P1
**Player Value:** VERY HIGH
**Implementation Risk:** MEDIUM
**Estimated Size:** LARGE

### Why this matters
`abandoned_hospital` exists as a single record. A regional hospital is the richest medical
scavenging target in the genre and the natural anchor for the medical system (`disease_catalog`,
`chemical_dependency_items`, `dose_registers`). Expanding it into a multi-floor complex with
infection hazards turns a label into the medical content hub.

### Existing systems reused
`locations.json`, `disease_catalog.json` (7), `chemical_dependency_items.json` (13),
`dose_registers.json` (4), `MedicalSystem`, `pharma_recipes.json` (25), `JournalSystem`,
`CombatTraumaSystem`, `events.json`.

### World-content addition
* 1 hospital complex (exterior + 5 interior zones: ER, wards, pharmacy, morgue, basement lab)
* 12 medical scavenging entries (surgical kits, chemicals, contaminated waste, records)
* 3 infection-hazard encounters (sealed ward, biohazard spill, trapped patient)
* 1 rescue questline (4 stages: signal → triage → transport → shelter cost)
* 8 environmental documents (patient charts, evacuation order, quarantine notice, dose ledger)
* 2 unique items (`item_surgical_field_kit`, `item_contaminated_blood_samples`)
* 1 recurring NPC (the last nurse)

### Gameplay loop
`Radio distress signal → expedition to hospital → ER triage of trapped survivors →
biohazard exposure check (disease_catalog) → pharmacy scavenging (pharma_recipes loot) →
morgue environmental story → transport decision (costs shelter medical supplies) →
nurse recruitable → later: nurse enables advanced pharma crafting`

### Content specification
* `loc_regional_hospital_complex` with `zones[]`: `er_triage`, `inpatient_wards`,
  `pharmacy_vault`, `morgue`, `basement_research_lab`
* Quest: `quest_hospital_rescue_signal` (4 stages)
* NPC: `npc_last_nurse_ianov` (skill: medical, weakness: chemical dependency, recruitable)
* Encounters: `enc_sealed_quarantine_ward`, `enc_biohazard_spill_pharmacy`, `enc_trapped_patient_er`
* Documents: `narrative/hospital_evacuation_order.json`, `narrative/hospital_quarantine_notice.json`,
  `narrative/hospital_patient_charts.json`, `narrative/hospital_dose_ledger.json`

### Integration points
`locations.json`, `events.json`, `characters.json`, `moral_choice_quests.json`, `items.json`,
`disease_catalog.json` (exposure hooks), `pharma_recipes.json` (loot source), flags
`flag_hospital_nurse_recruited`, `flag_hospital_quarantine_breached`.

### Substeps
1. Read `abandoned_hospital` record; confirm it is single-zone; supersede with the complex.
2. Define 5 zones with distinct `loot_identity` (ER = trauma kits, pharmacy = pharma components, morgue = records, lab = chemicals).
3. Wire 3 infection encounters to `disease_catalog` exposure rolls via `ISeededRng`.
4. Author the 4-stage rescue quest with a shelter-resource-cost branch (medical supplies).
5. Add `npc_last_nurse_ianov` to `characters.json` with skill/weakness/recruitable fields.
6. Author 8 environmental documents in `narrative/` using `ashfall-write` tone rules.
7. Add 2 unique items; verify `item_` prefix against `CatalogIntegrityValidator`.
8. Wire nurse recruitment to unlock an advanced `pharma_recipes` branch (data-only: add recipe prerequisites).
9. Add persistent flags; verify `InMemoryFlagLedger` case handling.
10. Run integrity + narrative-continuity + dialog-graph-lint.

### Acceptance Criteria
Hospital complex resolves; 3 infection encounters roll against `disease_catalog`; nurse
recruitable; quest plays headless; 8 documents load; integrity 0 errors.

### QA / Validation
`--data-integrity-selftest`, `ashfall-narrative-continuity`, `ashfall-dialog-graph-lint`,
`ashfall-balance-sim` (shelter medical-supply cost branch — 2 coupled variables → cross-tool QA).

### Follow-on opportunities
1. W1 tenement evacuation route ends at the hospital. 2. W36 disease outbreak originates here.
3. W35 dose-register quests reference the hospital dose ledger. 4. W44 nurse recurs on Day 40/80.
5. C2 medical-supply chain connects hospital → shelter → caravan.

---

## TASK W3 — Metro & Subway Network

**Category:** LOCATION
**Priority:** P1
**Player Value:** HIGH
**Implementation Risk:** MEDIUM
**Estimated Size:** LARGE

### Why this matters
`location_flooded_subway_depot` and `location_sub_level_4_transit` hint at a transit system but
it is not a **network**. A metro system is the natural subterranean artery connecting urban
locations (W1, W2, W4) and a high-value shelter-expansion substrate via `ExcavationSystem`.

### Existing systems reused
`locations.json`, `ExcavationSystem` (tunneling, shoring, cave-ins), `WeatherSystem`
(flooding), `ExpeditionSystem`, `currents.json` (17 — water dynamics reuse), `events.json`,
`JournalSystem`.

### World-content addition
* 1 metro network (4 stations + 3 tunnel segments = 7 connected nodes)
* 6 scavenging entries (transit maps, maintenance tools, emergency caches, commuter belongings)
* 4 encounters (flooded tunnel, cave-in, stranded commuters, faction patrol)
* 2 short quests (restore a tunnel section; find the sealed express line)
* 5 environmental documents (station notices, maintenance logs, commuter lost-and-found)
* 1 unique item (`item_metro_transit_map` — unlocks fast-travel between connected stations)

### Gameplay loop
`Discover station entrance (urban location) → descend → flooded/collapsed tunnel hazard
(ExcavationSystem shoring) → station scavenging → stranded-commuter encounter →
restore tunnel (engineering quest) → fast-travel unlocked between 2 stations →
later: faction uses the restored line for patrols`

### Content specification
* `loc_metro_central_station`, `loc_metro_riverside_station`, `loc_metro_industrial_station`,
  `loc_metro_maintenance_yard` + tunnel segments as `loc_metro_tunnel_*`
* Quests: `quest_restore_tunnel_section`, `quest_sealed_express_line`
* Encounters: `enc_flooded_tunnel`, `enc_metro_cave_in`, `enc_stranded_commuters`,
  `enc_faction_metro_patrol`
* Item: `item_metro_transit_map` (grants fast-travel flag)

### Integration points
`locations.json`, `excavation_sites.json` (W15 — tunnel segments as excavation sites),
`events.json`, `items.json`, `narrative/`, flags `flag_metro_tunnel_restored`,
`flag_metro_express_opened`, `flag_metro_fast_travel`.

### Substeps
1. Inventory existing transit loc_ ids; map a 4-station graph with 3 tunnel edges.
2. Author 4 station records with `zones[]` (platform, mezzanine, maintenance room).
3. Define 3 tunnel segments as `ExcavationSystem` sites in `excavation_sites.json` (W15) with shoring/cave-in params.
4. Wire flooding hazard to `WeatherSystem` (storm → water level rises in low tunnels).
5. Author 4 encounters; the faction-patrol one ties to W43 (territorialization).
6. Author 2 quests; the restore quest sets `flag_metro_tunnel_restored` enabling fast-travel.
7. Add `item_metro_transit_map` with a fast-travel effect (data-only flag, no new system).
8. Author 5 environmental documents.
9. Verify the station graph is connected; add a reachability test.
10. Run integrity + dialog-graph-lint.

### Acceptance Criteria
4 stations + 3 tunnels resolve; restore quest enables fast-travel flag; flooding responds to
weather; faction patrol appears post-restoration; integrity 0 errors.

### QA / Validation
`--data-integrity-selftest`, `ashfall-dialog-graph-lint`, `ashfall-balance-sim` (fast-travel
economy impact), determinism check on flooding rolls.

### Follow-on opportunities
1. W1 highrise basement connects to a metro station. 2. W9 subterranean infrastructure merges
with deep tunnels. 3. W43 faction patrols use restored lines. 4. W27 rail recovery parallels
metro. 5. C3 metro-rail-faction chain.

---

## TASK W4 — Civic & Municipal District

**Category:** LOCATION
**Priority:** P2
**Player Value:** HIGH
**Implementation Risk:** LOW
**Estimated Size:** MEDIUM

### Why this matters
No police station, fire station, school, or municipal building exists as an explorable location
despite being canonical survival-genre targets and natural sources of `Document` items (14 exist)
and government lore. The civic tier grounds the pre-war society.

### Existing systems reused
`locations.json`, `items.json` (Document category), `faction_lore.json` (government factions),
`JournalSystem`, `events.json`, `narrative/` (bunker_court_verdicts_codex reuse).

### World-content addition
* 6 civic locations (police station, fire station, school, municipal hall, courthouse, post office)
* 10 scavenging entries (uniforms, radios, records, maps, stationery, evidence lockers)
* 5 encounters (evidence-locker dispute, school shelter refugees, courthouse tribunal, fire-station generator, post-office dead letters)
* 3 short quests (the dead-letter archive; the courthouse records; the school shelter)
* 12 environmental documents (arrest records, court verdicts, class registers, dead letters, fire logs)
* 2 unique items (`item_evidence_locker_key`, `item_civic_archive_index`)

### Gameplay loop
`Expedition to civic building → records scavenging (Document loot) → encounter (refugees or
tribunal) → quest hook (dead letters reveal a pre-war secret) → journal/codex unlock →
later: courthouse records become Verdict evidence (W27/C4)`

### Content specification
* `loc_police_station_central`, `loc_fire_station_3`, `loc_municipal_school`,
  `loc_city_hall_ruins`, `loc_courthouse`, `loc_post_office`
* Quests: `quest_dead_letter_archive`, `quest_courthouse_records`, `quest_school_shelter`
* Encounters: `enc_evidence_locker`, `enc_school_refugees`, `enc_courthouse_tribunal`,
  `enc_fire_generator`, `enc_dead_letters`

### Integration points
`locations.json`, `events.json`, `items.json`, `narrative/` (reuse
`bunker_court_verdicts_codex.json` structure), `verdict_questlines.json` (courthouse feeds
Verdict), flags `flag_dead_letter_secret_read`.

### Substeps
1. Pick 6 snake_case loc_ ids; confirm none collide with existing civic names.
2. Author 6 records with distinct `loot_identity` (police = evidence/weapons, school = books/food, courthouse = records).
3. Add 10 scavenging entries to `scavenging_tables.json` (W20).
4. Author 5 encounters; the tribunal one uses moral-choice branching.
5. Author 3 short quests; the dead-letter quest sets a flag feeding W27/C4.
6. Author 12 documents in `narrative/` using existing court-verdict codex format.
7. Add 2 unique items.
8. Wire courthouse records → `verdict_questlines.json` evidence (data-only reference).
9. Run integrity + narrative-continuity.
10. Add reachability tests for the 3 quests.

### Acceptance Criteria
6 locations resolve; 5 encounters fire; 3 quests playable; 12 documents load; courthouse→Verdict
link resolves; integrity 0 errors.

### QA / Validation
`--data-integrity-selftest`, `ashfall-narrative-continuity`, `ashfall-dialog-graph-lint`.

### Follow-on opportunities
1. W2 hospital evacuation order was issued from city hall. 2. W6 military checkpoint references
police records. 3. W27 verdict dossiers pull from courthouse. 4. W28 museum is a civic building.
5. C4 civic-records→Verdict chain.

---

## TASK W5 — Industrial Corridor: Chemical, Steel & Power

**Category:** LOCATION
**Priority:** P1
**Player Value:** HIGH
**Implementation Risk:** MEDIUM
**Estimated Size:** LARGE

### Why this matters
Several industrial loc_ ids exist (`location_geo_thermal_plant_ruins`,
`location_concrete_batching_plant`, `location_abandoned_desalination`) but they are isolated
points. A systematic industrial corridor is the source of `Material` (38 items) and `Fuel` (4
items) and the anchor for the fuel-shortage crisis (W23) and rail recovery (W27).

### Existing systems reused
`locations.json`, `items.json` (Material/Fuel/Component), `EconomySystem`, `foundry_production.json`
(11), `foundry_faction.json`, `WeatherSystem` (chemical fog), `events.json`, `LedgerDebtSystem`.

### World-content addition
* 5 industrial locations (chemical plant, steelworks, refinery, power substation, machine works)
* 12 scavenging entries (chemicals, steel, fuel, electrical parts, tools, hazardous waste)
* 5 encounters (chemical leak, trapped workers, faction resource dispute, unstable structure, foundry labor)
* 2 questlines (restart the substation; the steelworks labor strike)
* 8 environmental documents (safety logs, shift records, accident reports, union notices)
* 3 unique items (`item_chemical_drum_sealed`, `item_high_voltage_component`, `item_steel_billet_batch`)

### Gameplay loop
`Expedition to industrial site → hazard (chemical fog / unstable structure) →
scavenging (Material/Fuel loot) → faction dispute encounter (foundry vs hydro_barons) →
engineering quest (restart substation → regional power benefit) → later: powers W27 rail,
enables foundry production recipes`

### Content specification
* `loc_chemical_plant_verdansk`, `loc_steelworks_kommersant`, `loc_refinery_dusk`,
  `loc_power_substation_beta`, `loc_machine_works_ural`
* Quests: `quest_restart_substation_beta` (engineering, 4 stages),
  `quest_steelworks_labor_strike` (faction, 4 stages)
* Encounters: `enc_chemical_leak`, `enc_trapped_workers_steelworks`,
  `enc_resource_dispute_refinery`, `enc_unstable_structure_machine_works`, `enc_foundry_labor`

### Integration points
`locations.json`, `events.json`, `items.json`, `foundry_production.json` (steelworks feeds
recipes), `LedgerDebtSystem` (W18 — labor strike involves debt contracts), `narrative/`,
flags `flag_substation_beta_online`, `flag_steelworks_strike_resolved`.

### Substeps
1. Inventory existing industrial loc_ ids; pick 5 non-colliding names.
2. Author 5 records with `loot_identity` (chemical = chemicals/waste, steel = steel/tools, refinery = fuel).
3. Wire chemical-fog hazard to `WeatherSystem` contaminated-fog state.
4. Author 5 encounters; the resource dispute ties foundry vs hydro_barons (faction_lore).
5. Author 2 questlines; substation restart sets a regional-power flag feeding W27 rail.
6. Add 12 scavenging entries to `scavenging_tables.json` (W20).
7. Author 8 documents; reuse `narrative/` industrial-log format.
8. Add 3 unique items.
9. Wire steelworks strike to `LedgerDebtSystem` templates (W18) — debt contract as labor grievance.
10. Run integrity + balance-sim (substation power benefit — 2 coupled vars → cross-tool QA).

### Acceptance Criteria
5 locations resolve; substation flag enables a downstream benefit; strike quest resolves via
debt system; 5 encounters fire; integrity 0 errors.

### QA / Validation
`--data-integrity-selftest`, `ashfall-balance-sim`, `ashfall-narrative-continuity`.

### Follow-on opportunities
1. W23 fuel shortage crisis centers on the refinery. 2. W27 rail recovery needs substation power.
3. Existing 22C foundry labor politics deepens the strike. 4. C5 industrial-power-rail chain.
5. W17 orbital strike targets the substation.

---

## TASK W6 — Military Frontier: Checkpoints, Depots & Front Lines

**Category:** LOCATION
**Priority:** P2
**Player Value:** VERY HIGH
**Implementation Risk:** MEDIUM
**Estimated Size:** LARGE

### Why this matters
Military loc_ ids exist (`location_radar_array_spire`, `location_drone_hive_silo`,
`location_automated_mortar_pit`, `location_the_dead_hand_core`) but there is no **frontier** —
checkpoints, ammo depots, field hospitals, and abandoned front lines that would surround a
post-exchange region. The military factions (`iron_garrison`, `central_garrison`, `black_ops`)
have lore but no physical territory.

### Existing systems reused
`locations.json`, `combat_catalog.json`, `items.json` (Ammo/Weapon), `faction_lore.json`
(military factions), `TacticalCombatSystem`, `OrbitalHarrowTelemetrySystem` (W17),
`events.json`, `SignalIntelligenceCatalog` (wiretap transcripts).

### World-content addition
* 6 military locations (checkpoint, ammo depot, field hospital, artillery battery, command bunker, abandoned front line)
* 10 scavenging entries (ammo, uniforms, comms gear, rations, medical supplies, unexploded ordnance)
* 6 encounters (checkpoint standoff, UXO disposal, field-hospital triage, artillery misfire, dead-hand activation, deserter surrender)
* 2 questlines (the dead-hand core; the deserter column)
* 10 environmental documents (orders, casualty lists, comms transcripts, desertion notices)
* 3 unique items (`item_military_cipher_key`, `item_field_surgical_kit`, `item_unexploded_shell`)
* 1 recurring NPC (the deserter officer)

### Gameplay loop
`Expedition to military site → checkpoint standoff (combat or negotiation) →
UXO hazard (skill check) → scavenging (ammo/comms) → dead-hand encounter (orbital telemetry
warning) → deserter NPC (recruit or report) → later: NPC affects faction war (existing 06C)`

### Content specification
* `loc_military_checkpoint_alpha`, `loc_ammo_depot_silo_7`, `loc_field_hospital_forward`,
  `loc_artillery_battery_ridge`, `loc_command_bunker_sigma`, `loc_abandoned_front_line_east`
* Quests: `quest_dead_hand_core` (5 stages), `quest_deserter_column` (4 stages)
* NPC: `npc_deserter_officer_voss` (skill: combat, weakness: trauma, faction: iron_garrison defector)
* Encounters: `enc_checkpoint_standoff`, `enc_uxo_disposal`, `enc_field_hospital_triage`,
  `enc_artillery_misfire`, `enc_dead_hand_activation`, `enc_deserter_surrender`

### Integration points
`locations.json`, `combat_catalog.json`, `events.json`, `characters.json`, `items.json`,
`narrative/` (reuse `bunker_wiretap_transcripts.json`), `OrbitalHarrowTelemetrySystem` (W17
dead-hand event), flags `flag_dead_hand_neutralized`, `flag_deserter_recruited`,
`flag_checkpoint_bypassed`.

### Substeps
1. Inventory existing military loc_ ids; pick 6 frontier names.
2. Author 6 records with `loot_identity` (depot = ammo/comms, field hospital = medical, front line = UXO/records).
3. Wire 6 encounters; checkpoint standoff has combat + negotiation branches.
4. Author 2 questlines; dead-hand quest triggers an `OrbitalHarrowTelemetrySystem` event (W17).
5. Add `npc_deserter_officer_voss`; wire defection to faction-war flags (existing 06C).
6. Add 10 scavenging entries; UXO disposal is a skill check (SkillProgressionSystem — W12).
7. Author 10 documents; reuse wiretap-transcript format for comms.
8. Add 3 unique items; cipher key feeds SignalIntelligence decode (existing 11B/C1).
9. Run integrity + combat selftest + balance-sim (checkpoint combat difficulty).
10. Add reachability + narrative-continuity tests.

### Acceptance Criteria
6 locations resolve; dead-hand event fires from telemetry; deserter NPC recruitable; checkpoint
has both branches; integrity 0 errors.

### QA / Validation
`--data-integrity-selftest`, combat selftest, `ashfall-balance-sim`, `ashfall-narrative-continuity`.

### Follow-on opportunities
1. W47 hidden-bunker competition centers on command bunker sigma. 2. W43 iron_garrison patrols
the checkpoints. 3. C1 dead-hand→telemetry→cipher chain. 4. W17 orbital events target the
artillery battery. 5. Existing 06C faction war uses the deserter.

---

## TASK W7 — Scientific Research Cluster

**Category:** LOCATION
**Priority:** P2
**Player Value:** HIGH
**Implementation Risk:** MEDIUM
**Estimated Size:** MEDIUM

### Why this matters
Scientific loc_ ids exist (`location_bio_remediation_lab`, `location_silent_observatory`,
`location_subterranean_seed_vault`) but no cluster ties them to `ResearchSystem` (15 hardcoded
nodes, W13) or `library_manuals.json`. A research cluster is the natural source of knowledge
unlocks and the bridge between exploration and the tech tree.

### Existing systems reused
`locations.json`, `ResearchSystem` (W13 externalization), `library_manuals.json` (broken ids —
fix here), `SkillProgressionSystem` (W12), `items.json` (Document/Device), `JournalSystem`,
`narrative/` (research logs).

### World-content addition
* 5 scientific locations (university lab, weather station, radiation-monitoring facility, agricultural research site, observatory)
* 8 scavenging entries (instruments, manuals, samples, data tapes, chemicals)
* 4 encounters (contaminated sample, locked lab, rival researcher, automated system still running)
* 1 questline (the observatory data — 5 stages decoding pre-war atmospheric records)
* 8 environmental documents (research logs, data tapes, calibration records, grant proposals)
* 2 unique items (`item_research_data_tape`, `item_calibration_instrument`)

### Gameplay loop
`Expedition to research site → locked-lab encounter (skill check) →
scavenging (manuals unlock research nodes — W13) → automated-system encounter (hazard) →
observatory quest (decode data → reveal a hidden location coordinate) →
later: coordinate unlocks a W9 subterranean site`

### Content specification
* `loc_university_lab_physics`, `loc_weather_station_pole`, `loc_radiation_monitoring_facility`,
  `loc_agricultural_research_station`, `loc_observatory_data_vault`
* Quest: `quest_observatory_data_decode` (5 stages)
* Encounters: `enc_contaminated_sample`, `enc_locked_lab`, `enc_rival_researcher`,
  `enc_automated_system_running`
* Fix `library_manuals.json` broken/None ids as part of this task.

### Integration points
`locations.json`, `events.json`, `items.json`, `research_catalog.json` (W13 — manuals unlock
nodes), `library_manuals.json` (fix), `narrative/`, flags `flag_observatory_coordinate_revealed`.

### Substeps
1. Inventory existing scientific loc_ ids; pick 5; fix `library_manuals.json` None ids first.
2. Author 5 records with `loot_identity` (lab = instruments/manuals, weather station = data tapes, ag station = seeds).
3. Wire manual scavenging to `research_catalog.json` node prerequisites (W13).
4. Author 4 encounters; locked-lab uses SkillProgressionSystem check (W12).
5. Author the 5-stage observatory quest; final stage reveals a coordinate → W9 subterranean site.
6. Add 8 documents; reuse research-log format.
7. Add 2 unique items; data tape feeds SignalIntelligence decode.
8. Wire `flag_observatory_coordinate_revealed` → unlocks a W9 location.
9. Run integrity + dialog-graph-lint + narrative-continuity.
10. Add a test that manuals resolve to research nodes.

### Acceptance Criteria
5 locations resolve; manuals unlock research nodes; observatory quest reveals a coordinate;
`library_manuals.json` ids fixed; integrity 0 errors.

### QA / Validation
`--data-integrity-selftest`, `ashfall-dialog-graph-lint`, `ashfall-narrative-continuity`,
manual→node resolution test.

### Follow-on opportunities
1. W9 subterranean site unlocked by observatory coordinate. 2. W13 research tree fed by manual
loot. 3. W17 telemetry uses weather-station data. 4. C6 signal→research→location chain.
5. W28 museum connects to the university lab.

---

## TASK W8 — Frozen Rural Hinterland: Villages, Farms & Monasteries

**Category:** LOCATION
**Priority:** P1
**Player Value:** HIGH
**Implementation Risk:** LOW
**Estimated Size:** LARGE

### Why this matters
Rural content is nearly absent (`rural_gas_station` only). The rural hinterland is the food
source (`greenhouse_items`, `economy_goods`), the refuge for fleeing populations (W22), and the
setting for the famine crisis (W24). It also balances the urban/industrial density with a
distinct survival texture (cold, isolation, distance).

### Existing systems reused
`locations.json`, `greenhouse_items.json` (14), `economy_goods.json` (16),
`WildlifeTrappingSystem` (W14), `WeatherSystem` (blizzard/cold), `CohortSystem`,
`events.json`, `narrative/` (bunker_children_folklore reuse).

### World-content addition
* 6 rural locations (village, farmstead, forestry compound, hunting cabin, monastery, frozen lakeside community)
* 10 scavenging entries (seed stock, preserved food, tools, firewood, religious objects, hunting gear)
* 5 encounters (starving family, poacher dispute, monastery refuge, frozen-lake breakthrough, wildlife)
* 2 questlines (the village famine; the monastery schism)
* 8 environmental documents (harvest ledgers, parish records, hunting logs, monastery chronicles)
* 2 unique items (`item_heirloom_seed_tin`, `item_monastery_bell_relic`)

### Gameplay loop
`Expedition to village → famine encounter (moral: share food or hoard) →
farmstead scavenging (seed stock → greenhouse) → monastery refuge (schism quest) →
frozen-lake hazard (weather-linked) → later: seed tin enables greenhouse recipe (W33)`

### Content specification
* `loc_village_krasnopolye`, `loc_farmstead_oster`, `loc_forestry_compound_taiga`,
  `loc_hunting_cabin_pinewood`, `loc_monastery_sveti`, `loc_frozen_lakeside_community`
* Quests: `quest_village_famine` (4 stages), `quest_monastery_schism` (4 stages)
* Encounters: `enc_starving_family`, `enc_poacher_dispute`, `enc_monastery_refuge`,
  `enc_frozen_lake_breakthrough`, `enc_rural_wildlife`

### Integration points
`locations.json`, `events.json`, `items.json`, `greenhouse_items.json` (seed tin),
`WildlifeTrappingSystem` (W14 — hunting cabin), `CohortSystem` (village children),
`narrative/`, flags `flag_village_fed`, `flag_monastery_schism_resolved`,
`flag_seed_tin_recovered`.

### Substeps
1. Inventory rural loc_ ids; pick 6; confirm cold-weather hazard wiring.
2. Author 6 records with `loot_identity` (farm = seed/food, monastery = religious objects/records, cabin = hunting gear).
3. Wire frozen-lake hazard to `WeatherSystem` cold/blizzard state.
4. Author 5 encounters; famine encounter has moral branching feeding W24.
5. Author 2 questlines; monastery schism ties to W46 (belief movements).
6. Add 10 scavenging entries; seed tin unlocks a `greenhouse_items` recipe (W33).
7. Author 8 documents; reuse parish-record and folklore formats.
8. Add 2 unique items.
9. Wire village children to `CohortSystem` (adoption hook — existing 12A).
10. Run integrity + narrative-continuity + balance-sim (famine food-cost branch).

### Acceptance Criteria
6 locations resolve; famine moral branch persists; seed tin enables greenhouse recipe;
monastery schism playable; integrity 0 errors.

### QA / Validation
`--data-integrity-selftest`, `ashfall-narrative-continuity`, `ashfall-balance-sim`,
`ashfall-dialog-graph-lint`.

### Follow-on opportunities
1. W24 famine crisis centers on the village. 2. W22 refugees flee through the village.
3. W46 monastery schism feeds belief movements. 4. W14 trapping species near the hunting cabin.
5. C8 rural-famine-greenhouse chain.

---

## TASK W9 — Subterranean Infrastructure: Deep Mines, Tunnels & Sealed Bunkers

**Category:** LOCATION
**Priority:** P1
**Player Value:** VERY HIGH
**Implementation Risk:** MEDIUM
**Estimated Size:** LARGE

### Why this matters
`ExcavationSystem` (depth, shoring, cave-ins) is fully coded but has **no predefined sites**
(W15). Subterranean loc_ ids exist (`location_collapsed_salt_mine`,
`location_sub_level_4_transit`, `location_subterranean_seed_vault`) but are not wired to
excavation. This pillar turns the excavation system into a real expedition tier and connects
to W3 (metro) and W7 (observatory coordinate).

### Existing systems reused
`locations.json`, `ExcavationSystem` (W15 sites), `SkyLayerArmorSystem` (W16),
`OrbitalHarrowTelemetrySystem` (W17), `bunker_blueprints_codex.json` (narrative),
`disease_catalog.json` (deep mold), `events.json`, `JournalSystem`.

### World-content addition
* 5 subterranean locations (deep mine, maintenance tunnel network, sealed military bunker, buried factory, forgotten storage chamber)
* 8 scavenging entries (ore, machinery, sealed supplies, pre-war archives, hazardous gas canisters)
* 5 encounters (cave-in, gas pocket, sealed-door mystery, buried-squad remains, subterranean fauna)
* 1 questline (the sealed military bunker — 5 stages)
* 8 environmental documents (mining logs, sealed orders, maintenance records, emergency rations manifest)
* 2 unique items (`item_sealed_archive_cylinder`, `item_deep_core_sample`)

### Gameplay loop
`Expedition to subterranean site → excavation (shoring/cave-in via ExcavationSystem) →
gas-pocket hazard (disease_catalog exposure) → sealed-door mystery (skill check) →
buried-squad remains (environmental story) → bunker quest (sealed archive → faction
consequence) → later: archive feeds Verdict (W27)`

### Content specification
* `loc_deep_mine_shaft_9`, `loc_maintenance_tunnel_grid`, `loc_sealed_military_bunker_kappa`,
  `loc_buried_factory_sublevel`, `loc_forgotten_storage_chamber`
* Quest: `quest_sealed_bunker_kappa` (5 stages)
* Encounters: `enc_cave_in_deep`, `enc_gas_pocket`, `enc_sealed_door_mystery`,
  `enc_buried_squad_remains`, `enc_subterranean_fauna`
* Sites also registered in `excavation_sites.json` (W15) with depth/shoring params.

### Integration points
`locations.json`, `excavation_sites.json` (W15), `disease_catalog.json` (deep mold),
`bunker_blueprints_codex.json`, `events.json`, `items.json`, `narrative/`, flags
`flag_bunker_kappa_opened`, `flag_sealed_archive_recovered`.

### Substeps
1. Inventory existing subterranean loc_ ids; pick 5; confirm `ExcavationSystem` AddSite API.
2. Author 5 records; register each in `excavation_sites.json` (W15) with depth/structural-risk params.
3. Wire cave-in encounters to `ExcavationSystem` shoring state.
4. Wire gas-pocket hazard to `disease_catalog` (deep mold — add 1 disease entry).
5. Author 5 encounters; sealed-door uses SkillProgressionSystem check (W12).
6. Author the 5-stage bunker quest; archive item feeds Verdict (W27) via flag.
7. Author 8 documents; reuse `bunker_blueprints_codex.json` format.
8. Add 2 unique items; archive cylinder is a Document triggering a codex unlock.
9. Wire `flag_bunker_kappa_opened` → W47 faction competition for the bunker.
10. Run integrity + excavation save round-trip + determinism check.

### Acceptance Criteria
5 sites resolve and register in `ExcavationSystem`; cave-in rolls deterministically; bunker
quest plays; archive feeds Verdict flag; integrity 0 errors.

### QA / Validation
`--data-integrity-selftest`, excavation save round-trip, `ashfall-determinism-guard`,
`ashfall-dialog-graph-lint`.

### Follow-on opportunities
1. W7 observatory coordinate unlocks one site. 2. W3 metro tunnels merge with maintenance grid.
3. W47 factions compete for bunker kappa. 4. W17 orbital strike targets surface above a site.
5. C9 observatory→excavation→archive→Verdict chain.

---

## TASK W10 — Wilderness Hazard Belts

**Category:** LOCATION
**Priority:** P2
**Player Value:** MEDIUM
**Implementation Risk:** LOW
**Estimated Size:** MEDIUM

### Why this matters
Wilderness loc_ ids are sparse (`location_ash_dune_cemetery`, `location_abandoned_ski_resort`).
The wilderness belts are where `WeatherSystem` (fallout storms, blizzards, contaminated fog) and
`WildlifeMigrationSystem` (11 packs) should bite hardest, but no locations exploit this.

### Existing systems reused
`locations.json`, `WeatherSystem` (22 states), `WildlifeMigrationSystem` (existing 28),
`WildlifeTrappingSystem` (W14), `RadiationSystem`, `events.json`, `narrative/`.

### World-content addition
* 5 wilderness locations (irradiated forest, frozen wetland, poisoned valley, burned woodland, blizzard corridor)
* 8 scavenging entries (foraged food, contaminated water, animal parts, dead survival caches)
* 5 encounters (mutated fauna, contaminated water, lost hiker, fallout-storm exposure, trapper's cache)
* 2 short quests (the lost hiker; the trapper's last line)
* 6 environmental documents (trail markers, survival notes, contamination warnings, hunting blinds)

### Gameplay loop
`Expedition to wilderness belt → weather hazard (fallout storm / blizzard) →
radiation exposure check → mutated-fauna encounter (WildlifeMigrationSystem) →
trapper's cache scavenging → lost-hiker rescue → later: hiker becomes a recurring NPC (W44)`

### Content specification
* `loc_irradiated_forest_pine`, `loc_frozen_wetland_mire`, `loc_poisoned_valley_ash`,
  `loc_burned_woodland_char`, `loc_blizzard_corridor_pass`
* Quests: `quest_lost_hiker`, `quest_trappers_last_line`
* Encounters: `enc_mutated_fauna_forest`, `enc_contaminated_water_wetland`,
  `enc_lost_hiker`, `enc_fallout_storm_exposure`, `enc_trappers_cache`

### Integration points
`locations.json`, `events.json`, `WeatherSystem` (per-location weather modifiers),
`WildlifeMigrationSystem` (pack spawns), `RadiationSystem`, `narrative/`, flags
`flag_lost_hiker_rescued`.

### Substeps
1. Inventory wilderness loc_ ids; pick 5; confirm weather-state keys.
2. Author 5 records with per-location `weather_modifiers` (fallout storm in forest, blizzard in corridor).
3. Wire mutated-fauna encounter to `WildlifeMigrationSystem` pack spawn (existing 28).
4. Wire radiation exposure to `RadiationSystem` dose accumulation.
5. Author 5 encounters; fallout-storm exposure is weather-gated (W48).
6. Author 2 short quests; lost hiker becomes `npc_lost_hiker` (W44 recurring).
7. Add 8 scavenging entries; contaminated water uses existing `IrradiatedWater` item category.
8. Author 6 documents.
9. Run integrity + determinism (weather + pack spawn rolls).
10. Add reachability tests.

### Acceptance Criteria
5 locations resolve; weather modifiers alter encounter odds; fauna spawns from migration
system; hiker NPC created; integrity 0 errors.

### QA / Validation
`--data-integrity-selftest`, `ashfall-determinism-guard`, `ashfall-balance-sim`.

### Follow-on opportunities
1. W14 trapping species spawn in these belts. 2. Existing 28 migration routes pass through.
3. W48 weather gating makes some expeditions seasonal. 4. W44 lost hiker recurs. 5. C10
weather→wildlife→trapping chain.

---

## TASK W11 — Roadside Micro-Location Pack

**Category:** LOCATION
**Priority:** P1
**Player Value:** HIGH
**Implementation Risk:** LOW
**Estimated Size:** MEDIUM

### Why this matters
Expeditions travel between major locations but the routes are empty. Micro-locations are
small, one-time discoveries that make travel feel lived-in without requiring full location
records. This is a **new content category** not present in the existing roadmap.

### Existing systems reused
`locations.json` (lightweight records), `ExpeditionSystem` (route events), `events.json`,
`items.json` (one-time loot), `JournalSystem` (discovery entries), `narrative/`.

### World-content addition
* 25 micro-locations (roadside memorial, crashed truck, frozen evacuation bus, improvised grave,
  radio tower, destroyed checkpoint, abandoned tent, hunting blind, collapsed bridge, drainage
  pipe, rail siding, dead livestock area, ruined greenhouse, shell crater, field kitchen,
  generator, shrine, emergency cache, observation post, makeshift clinic, abandoned barricade,
  stranded car, wayside shrine, frozen well, marker stone)
* 25 one-time loot entries
* 15 short encounters (mostly noncombat: observation, small ethical decision, rumor)
* 25 environmental-storytelling snippets

### Gameplay loop
`Travel between major locations → micro-location triggered on route →
short encounter (observation / small decision / one-time loot) → journal discovery entry →
no return value (depleted) → world texture accumulated`

### Content specification
* `loc_micro_roadside_memorial`, `loc_micro_crashed_truck`, `loc_micro_frozen_evac_bus`,
  `loc_micro_improvised_grave`, `loc_micro_radio_tower`, ... (25 total, `loc_micro_` prefix)
* Each: `one_time_loot`, `encounter_id`, `story_text`, `depleted_flag`

### Integration points
`locations.json` (new `loc_micro_` prefix — validate against `CatalogIntegrityValidator`
master list), `events.json`, `items.json`, `narrative/`, per-micro `flag_micro_*_depleted`.

### Substeps
1. **Validate `loc_micro_` prefix** against `CatalogIntegrityValidator` master list; add if needed (this is the one allowed new-prefix case — confirm before authoring).
2. Define a lightweight micro-location schema (id, loot, encounter, story, depleted_flag).
3. Author 25 micro-locations across all pillars (urban, rural, industrial, military, wilderness).
4. Add 25 one-time loot entries to `items.json` or a `micro_loot.json` table.
5. Author 15 short encounters (prioritize noncombat: observation, rumor, small decision).
6. Author 25 story snippets in `narrative/` (1–3 sentences each).
7. Wire micro-location triggers into `ExpeditionSystem` route events (data-only probability table).
8. Add per-micro depleted flags.
9. Run integrity + narrative-continuity.
10. Add a test that micro-locations deplete correctly (one-time loot).

### Acceptance Criteria
25 micro-locations resolve; 15 encounters fire; one-time loot depletes; `loc_micro_` prefix
validated; integrity 0 errors.

### QA / Validation
`--data-integrity-selftest`, `ashfall-narrative-continuity`, depletion test.

### Follow-on opportunities
1. Scatter micro-locations around W1–W10 major locations. 2. W44 recurring NPCs first appear at
a micro-location. 3. W22 refugee columns leave micro-location evidence. 4. C11 micro-location
rumor→major-quest hook chain.

---

## TASK W12 — Skill Externalization & New Action Skills

**Category:** SYSTEM-CONTENT
**Priority:** P1
**Player Value:** HIGH
**Implementation Risk:** MEDIUM
**Estimated Size:** MEDIUM

### Why this matters
`SkillProgressionSystem` documents `skills.json` as the authority in `SkillDef.cs:9` but the
file **does not exist**; 47 skills are hardcoded via `MakeSkill()`/`MakeMilestone()`. This
violates the JSON-authority invariant (Invariant 6) and blocks every skill-check encounter in
W1–W10. 38 milestone skills have `xpThreshold=999999` (unreachable except narratively).

### Existing systems reused
`SkillProgressionSystem`, `SkillDef.cs`, `CatalogIntegrityValidator` (`skill_` prefix),
`ApprenticeshipSystem` (references skill ids), `ExpeditionSystem` (capability checks).

### World-content addition
* `skills.json` externalizing all 47 existing skills
* 15 new action skills with real XP thresholds (lockpicking, field surgery, UXO disposal, negotiation, tracking, mechanics, etc.)
* Descriptions for the 38 milestone skills (currently displayName-as-placeholder)

### Gameplay loop
`Skill-check encounter (W1–W10) → SkillProgressionSystem reads skills.json →
XP awarded on success → milestone unlocked narratively → skill gates future encounters`

### Content specification
* `skills.json` with `schema_version`, 62 skill entries (`skill_` prefix)
* Each action skill: `id`, `display_name`, `description`, `xp_threshold`, `action_source`
* Each milestone skill: `id`, `display_name`, `description`, `grant_source`

### Integration points
`SkillDef.cs` (add `LoadFromDirectory` — minor Core extension to read JSON; keep hardcoded as
fallback), `ApprenticeshipSystem` (skill id references), `CatalogIntegrityValidator`,
W1–W10 encounters reference skill ids.

### Substeps
1. Read `SkillDef.cs` `RegisterDefaults()`; extract all 47 skill definitions to a JSON skeleton.
2. Create `skills.json` with `schema_version` and `skill_` ids.
3. Add a `LoadFromDirectory` path in `SkillDef` that reads `skills.json` if present (fallback to hardcoded for save compat — **NEW SYSTEM JUSTIFICATION REQUIRED**: minimal loader, no behavior change, preserves determinism).
4. Author 15 new action skills with real XP thresholds.
5. Add descriptions to all 38 milestone skills.
6. Verify all skill ids referenced by `ApprenticeshipSystem` resolve in the new JSON.
7. Wire W1–W10 skill-check encounters to the new skill ids.
8. Add a `CatalogIntegrityValidator` tier for `skill_` prefix resolution.
9. Run integrity + skill round-trip test.
10. Add a test that `skills.json` loads and matches the hardcoded fallback exactly.

### Acceptance Criteria
`skills.json` exists and loads; 62 skills resolve; apprenticeship references valid; hardcoded
fallback preserved; integrity 0 errors; skill round-trip green.

### QA / Validation
`--data-integrity-selftest`, skill load/round-trip test, `ashfall-determinism-guard`.

### Follow-on opportunities
1. W1–W10 encounters use the new skills. 2. Existing 26 trade specialties reference skills.
3. W19 room definitions gate on skills. 4. C12 skill→encounter→apprenticeship chain.

> **NEW SYSTEM JUSTIFICATION REQUIRED:** Adding a JSON loader to `SkillDef` is the minimal Core
> change needed to satisfy Invariant 6 (data authority is JSON). The hardcoded path is retained
> as fallback for save compatibility. No new gameplay system is introduced.

---

## TASK W13 — Research Tech-Tree Externalization & Expansion

**Category:** SYSTEM-CONTENT
**Priority:** P1
**Player Value:** HIGH
**Implementation Risk:** MEDIUM
**Estimated Size:** MEDIUM

### Why this matters
`ResearchSystem` hardcodes 15 nodes in `RegisterDefaults()` with no JSON file even referenced.
This violates Invariant 6 and leaves the tech tree tiny (15 nodes, 6 disciplines) and immutable.
Supersedes existing plan 26A with the data-authority execution.

### Existing systems reused
`ResearchSystem`, `ResearchHostSession`, `library_manuals.json` (fix in W7),
`SkillProgressionSystem` (W12), `foundry_production.json` (advanced recipes unlocked),
`CatalogIntegrityValidator` (`knowledge_` prefix).

### World-content addition
* `research_catalog.json` externalizing 15 existing nodes
* 25 new research nodes (tier 3+) across 6 disciplines
* Cross-discipline prerequisites and research-item costs
* Manual-loot unlocks (W7 manuals → research nodes)

### Gameplay loop
`Scavenge manual (W7) → research node unlocked in research_catalog.json →
assign researcher (duty roster) → day-progress tracking → node complete →
unlocks advanced recipe (foundry/greenhouse) or skill (W12)`

### Content specification
* `research_catalog.json` with `schema_version`, 40 nodes (`knowledge_` prefix)
* Each node: `id`, `discipline`, `tier`, `prerequisites[]`, `cost`, `unlock[]`, `manual_source`

### Integration points
`ResearchSystem` (add `LoadFromDirectory` — minimal Core extension, hardcoded fallback),
`library_manuals.json` (W7 fix), `foundry_production.json`, `greenhouse_items.json`,
`SkillProgressionSystem` (W12), `CatalogIntegrityValidator`.

### Substeps
1. Read `ResearchSystem.RegisterDefaults()`; extract 15 nodes to JSON skeleton.
2. Create `research_catalog.json` with `schema_version` and `knowledge_` ids.
3. Add a `LoadFromDirectory` path in `ResearchSystem` (fallback to hardcoded for save compat — **NEW SYSTEM JUSTIFICATION REQUIRED**: minimal loader, no behavior change).
4. Author 25 new tier-3+ nodes with cross-discipline prerequisites.
5. Wire `library_manuals.json` ids (fixed in W7) as `manual_source` for nodes.
6. Wire node `unlock[]` to `foundry_production.json` advanced recipes and `skills.json` (W12).
7. Add research-item costs (components from `items.json`).
8. Add a `CatalogIntegrityValidator` tier for `knowledge_` prefix resolution.
9. Run integrity + research round-trip test.
10. Add a test that manual loot unlocks the correct node.

### Acceptance Criteria
`research_catalog.json` exists and loads; 40 nodes resolve; manual→node unlock works; advanced
recipes gated; integrity 0 errors; research round-trip green.

### QA / Validation
`--data-integrity-selftest`, research round-trip test, `ashfall-determinism-guard`.

### Follow-on opportunities
1. W7 scientific cluster feeds manuals. 2. W34 foundry advanced tech unlocked. 3. W33
greenhouse advanced cultivars. 4. C6 signal→research→location chain. 5. Existing #17
cloud-seeding gated behind a tier-3 node.

> **NEW SYSTEM JUSTIFICATION REQUIRED:** Adding a JSON loader to `ResearchSystem` is the minimal
> Core change to satisfy Invariant 6. Hardcoded fallback retained for save compatibility.

---

## TASK W14 — Wildlife Trapping Species & Bait Catalog

**Category:** SYSTEM-CONTENT
**Priority:** P1
**Player Value:** MEDIUM
**Implementation Risk:** LOW
**Estimated Size:** MEDIUM

### Why this matters
`WildlifeTrappingSystem` (deadfalls, snares, butchery, rad-taint) is fully implemented and
save-supported but has **no data catalog** — species, bait, yield, and toxicity are all runtime
parameters with no definitions. This is the cheapest content multiplier for the hunting loop
(existing plan 13B covers the mechanic; this is the data).

### Existing systems reused
`WildlifeTrappingSystem`, `WildlifeMigrationSystem` (existing 28 packs), `items.json` (bait items,
Food category), `economy_goods.json`, `ISeededRng` (deterministic yields).

### World-content addition
* `wildlife_trapping_catalog.json` with 15 species, 8 bait types, yield/toxin tables
* 6 new bait items in `items.json`
* Species linked to `WildlifeMigrationSystem` packs (existing 28)

### Gameplay loop
`Craft bait (recipe) → set trap at location (W8/W10) →
species determined by migration pack (existing 28) + bait →
yield/toxin roll (ISeededRng) → butchery → food/animal-parts →
toxin removal decision (medical cost)`

### Content specification
* `wildlife_trapping_catalog.json`: `species[]` (id, yield, toxin_chance, preferred_bait, migration_pack),
  `baits[]` (id, item_id, attractiveness)
* Species: `species_rabbit_dust`, `species_deer_radstag`, `species_boar_glow`,
  `species_cave_molerat`, `species_frozen_fox`, ... (15 total)
* Baits: `item_bait_dried_meat`, `item_bait_foraged_root`, `item_bait_rad_lure`, ... (6 new items)

### Integration points
`WildlifeTrappingSystem` (add catalog load — minimal), `WildlifeMigrationSystem` (pack
references), `items.json` (bait items), `economy_goods.json` (animal parts), `ISeededRng`.

### Substeps
1. Read `WildlifeTrappingSystem` API; identify where species/bait/yield params enter.
2. Create `wildlife_trapping_catalog.json` with `schema_version`.
3. Author 15 species with yield/toxin/preferred_bait; link each to an existing-28 migration pack.
4. Author 8 bait types; add 6 new bait items to `items.json`.
5. Add catalog load to `WildlifeTrappingSystem` (minimal — **NEW SYSTEM JUSTIFICATION REQUIRED**: loader only, runtime params become defaults).
6. Wire species availability to `WildlifeMigrationSystem` pack presence at the location.
7. Wire toxin removal to `MedicalSystem` (chelation/anti-rad — existing).
8. Run determinism check on yield/toxin rolls.
9. Run integrity + balance-sim (food economy impact).
10. Add a trapping round-trip + determinism test.

### Acceptance Criteria
Catalog loads; 15 species + 8 baits resolve; species gated by migration pack; yields
deterministic; integrity 0 errors.

### QA / Validation
`--data-integrity-selftest`, `ashfall-determinism-guard`, `ashfall-balance-sim`.

### Follow-on opportunities
1. W8 hunting cabin + W10 wilderness use the catalog. 2. Existing 28 migration drives species
availability. 3. W33 greenhouse grows bait crops. 4. C10 weather→wildlife→trapping chain.

> **NEW SYSTEM JUSTIFICATION REQUIRED:** Adding catalog load to `WildlifeTrappingSystem` is a
> loader-only change; runtime parameters become JSON-driven defaults. No new gameplay system.

---

## TASK W15 — ExcavationSystem Predefined Sites Catalog

**Category:** SYSTEM-CONTENT
**Priority:** P1
**Player Value:** HIGH
**Implementation Risk:** MEDIUM
**Estimated Size:** MEDIUM

### Why this matters
`ExcavationSystem` (depth, shoring, cave-ins, room blueprints) is fully coded but sites are
created only via runtime `AddSite()` — there are **no predefined excavation sites**. This is
the data substrate for W9 (subterranean) and W3 (metro tunnels).

### Existing systems reused
`ExcavationSystem`, `ExcavationHostSession`, `bunker_blueprints_codex.json` (room IDs),
`SkyLayerArmorSystem` (W16 — surface above), `disease_catalog.json` (deep mold),
`ISeededRng` (cave-in rolls).

### World-content addition
* `excavation_sites.json` with 10 predefined sites
* Each site: depth, structural risk, room blueprint, hazard, loot identity
* 5 sites from W9 (subterranean) + 3 from W3 (metro tunnels) + 2 standalone

### Gameplay loop
`Discover site (W7 coordinate / W9 expedition) → register in ExcavationSystem →
assign workers (duty roster) → excavation progress (shoring) →
cave-in roll (ISeededRng) → completion → room blueprint unlocked →
shelter expansion`

### Content specification
* `excavation_sites.json`: `sites[]` (id, loc_id, depth, structural_risk, room_blueprint,
  hazard, loot_identity)
* Sites: `excav_site_deep_mine_9`, `excav_site_tunnel_grid`, `excav_site_bunker_kappa`,
  `excav_site_metro_tunnel_a`, ... (10 total)
* Room blueprints reference `bunker_blueprints_codex.json` IDs.

### Integration points
`ExcavationSystem` (add catalog load — minimal), `bunker_blueprints_codex.json`,
`disease_catalog.json`, `SkyLayerArmorSystem` (W16), `ISeededRng`, W3/W9 locations.

### Substeps
1. Read `ExcavationSystem.AddSite` API; identify params.
2. Create `excavation_sites.json` with `schema_version`.
3. Author 10 sites; 5 from W9, 3 from W3, 2 standalone; each references a `bunker_blueprints_codex` room.
4. Add catalog load to `ExcavationHostSession` (minimal — **NEW SYSTEM JUSTIFICATION REQUIRED**: loader only).
5. Wire cave-in probability to `structural_risk` + shoring state via `ISeededRng`.
6. Wire deep-mold hazard to `disease_catalog` exposure.
7. Wire completion → room blueprint unlock in `bunker_blueprints_codex`.
8. Run excavation save round-trip + determinism check.
9. Run integrity + balance-sim (worker-assignment cost).
10. Add a test that sites load and cave-ins roll deterministically.

### Acceptance Criteria
10 sites load; cave-ins deterministic; room blueprints unlock; integrity 0 errors; excavation
round-trip green.

### QA / Validation
`--data-integrity-selftest`, excavation round-trip, `ashfall-determinism-guard`,
`ashfall-balance-sim`.

### Follow-on opportunities
1. W9 subterranean locations register here. 2. W3 metro tunnels register here. 3. W16 sky-armor
protects surface above. 4. W17 orbital strikes damage surface above active sites. 5. C9
observatory→excavation chain.

> **NEW SYSTEM JUSTIFICATION REQUIRED:** Adding catalog load to `ExcavationHostSession` is a
> loader-only change; `AddSite` params become JSON-driven. No new gameplay system.

---

## TASK W16 — Sky-Layer Armor Reinforcement Catalog

**Category:** SYSTEM-CONTENT
**Priority:** P2
**Player Value:** MEDIUM
**Implementation Risk:** LOW
**Estimated Size:** SMALL

### Why this matters
`SkyLayerArmorSystem` (cell-grid roof armor, kinetic penetration) is complete but has no data
catalog for armor types, reinforcement materials, or degradation curves — all runtime state.
Existing plan 19B covers strike events; this is the armor **type** data.

### Existing systems reused
`SkyLayerArmorSystem`, `items.json` (Material), `OrbitalHarrowTelemetrySystem` (W17 events),
`foundry_production.json` (reinforcement materials), `bunker_blueprints_codex.json`.

### World-content addition
* `sky_layer_armor_catalog.json` with 6 reinforcement tiers
* Each tier: material, integrity bonus, resource cost, degradation curve
* 3 new material items

### Gameplay loop
`Orbital strike warning (W17) → player reinforces roof cells (material cost) →
strike resolves → armor degrades per curve → repair loop (foundry materials)`

### Content specification
* `sky_layer_armor_catalog.json`: `tiers[]` (id, material_id, integrity_bonus, cost, degradation_rate)
* Tiers: `armor_sandbag_berm`, `armor_concrete_slab`, `armor_steel_plate`,
  `armor_composite_mesh`, `armor_reactive_tile`, `armor_orbital_grade`
* Items: `item_sandbag_stack`, `item_steel_plate_roof`, `item_reactive_tile_set`

### Integration points
`SkyLayerArmorSystem` (add catalog load — minimal), `items.json`, `foundry_production.json`,
`bunker_blueprints_codex.json`, W17 events.

### Substeps
1. Read `SkyLayerArmorSystem` armor-state API.
2. Create `sky_layer_armor_catalog.json` with `schema_version`.
3. Author 6 tiers with escalating cost/bonus/degradation.
4. Add 3 material items to `items.json`.
5. Add catalog load to `SkyLayerArmorSystem` (minimal loader — **NEW SYSTEM JUSTIFICATION REQUIRED**).
6. Wire reinforcement cost to `foundry_production.json` materials.
7. Wire degradation to W17 strike events.
8. Run integrity + balance-sim (armor economy).
9. Add a round-trip test.
10. Add a determinism check on degradation rolls.

### Acceptance Criteria
6 tiers load; reinforcement costs materials; degradation deterministic; integrity 0 errors.

### QA / Validation
`--data-integrity-selftest`, `ashfall-balance-sim`, `ashfall-determinism-guard`.

### Follow-on opportunities
1. W17 strikes test the armor. 2. W5 steelworks produces armor materials. 3. W9 excavation
unlocks orbital-grade armor blueprint. 4. C13 strike→armor→repair chain.

> **NEW SYSTEM JUSTIFICATION REQUIRED:** Loader-only change to `SkyLayerArmorSystem`; runtime
> armor state becomes JSON-driven defaults.

---

## TASK W17 — Orbital Harrow Telemetry Event Catalog

**Category:** SYSTEM-CONTENT
**Priority:** P2
**Player Value:** HIGH
**Implementation Risk:** LOW
**Estimated Size:** MEDIUM

### Why this matters
`OrbitalHarrowTelemetrySystem` generates telemetry text at runtime via string interpolation —
there is **no event catalog**. Existing plan 19B covers orbital kinetic strikes as a concept;
this is the **data-driven event catalog** that makes strikes vary, escalate, and target real
locations (W5 substation, W6 artillery, W9 excavation surface).

### Existing systems reused
`OrbitalHarrowTelemetrySystem`, `SkyLayerArmorSystem` (W16), `SignalIntelligenceCatalog`
(seismic/emp logs exist), `WeatherSystem`, `events.json`, `JournalSystem`.

### World-content addition
* `orbital_harrow_events.json` with 12 escalating impact events
* Each event: grid coordinates, energy, armor damage, warning text, day threshold, target loc_id
* 6 telemetry warning broadcasts (radio content)

### Gameplay loop
`Telemetry warning (radio) → player observes target grid → reinforce roof (W16) or
evacuate → strike resolves → armor damage per W16 →
post-strike scavenging at crater (W11 micro-location) → later: dead-hand escalation (W6)`

### Content specification
* `orbital_harrow_events.json`: `events[]` (id, grid_x, grid_y, energy, armor_damage,
  warning_text, min_day, target_loc_id)
* Events: `harrow_debris_strike_1` ... `harrow_kinetic_barrage_3` (12, escalating by `min_day`)
* Radio: `radio_harrow_warning_1` ... `radio_harrow_warning_6`

### Integration points
`OrbitalHarrowTelemetrySystem` (add catalog load — minimal), `SkyLayerArmorSystem` (W16),
`radio.json`, `events.json`, `SignalIntelligenceCatalog` (seismic_array_fault_alarms),
W5/W6/W9 target locations.

### Substeps
1. Read `OrbitalHarrowTelemetrySystem` string-interpolation points.
2. Create `orbital_harrow_events.json` with `schema_version`.
3. Author 12 events with escalating `min_day` thresholds; target real loc_ ids (W5 substation, W6 artillery).
4. Add catalog load to `OrbitalHarrowTelemetrySystem` (minimal — **NEW SYSTEM JUSTIFICATION REQUIRED**).
5. Author 6 radio warning broadcasts in `radio.json`.
6. Wire armor damage to `SkyLayerArmorSystem` (W16) cell grid.
7. Wire post-strike crater to W11 micro-location spawn.
8. Wire dead-hand escalation to W6 quest (flag chain).
9. Run integrity + determinism (strike rolls).
10. Add a test that events fire on correct day thresholds.

### Acceptance Criteria
12 events load; strikes damage armor cells; warnings broadcast; day-gating works; integrity
0 errors; determinism green.

### QA / Validation
`--data-integrity-selftest`, `ashfall-determinism-guard`, `ashfall-balance-sim`.

### Follow-on opportunities
1. W6 dead-hand quest triggers the highest event. 2. W16 armor is tested by these events.
3. W11 craters spawn post-strike. 4. C1 dead-hand→telemetry→cipher chain. 5. Existing #17
cloud-seeding suppresses strike weather.

> **NEW SYSTEM JUSTIFICATION REQUIRED:** Loader-only change to `OrbitalHarrowTelemetrySystem`;
> string interpolation becomes JSON-driven event text.

---

## TASK W18 — Ledger Debt Contract Templates

**Category:** SYSTEM-CONTENT
**Priority:** P2
**Player Value:** MEDIUM
**Implementation Risk:** LOW
**Estimated Size:** SMALL

### Why this matters
`LedgerDebtSystem` (compound debt, collateral forfeiture) is fully implemented with save
support but has **no contract templates** — all contracts are runtime-created with caller
params. Existing plan 14 covers debt-collection bounty raids; this is the **template data**
that makes debt varied and faction-specific.

### Existing systems reused
`LedgerDebtSystem`, `faction_lore.json`, `EconomySystem`, `TradeTellEngine`,
`HoldfastTradeSession`, `events.json`.

### World-content addition
* `ledger_debt_templates.json` with 10 templates
* Each template: principal range, term, rate tier, collateral type, forfeit consequence, faction
* Faction-specific contract flavors (iron_garrison = labor debt, hydro_barons = water debt)

### Gameplay loop
`Trade/caravan creates debt → LedgerDebtSystem loads template →
compound interest accrues → default → escalating collector (existing 14) →
forfeit consequence (collateral loss or raid) → reputation shift`

### Content specification
* `ledger_debt_templates.json`: `templates[]` (id, faction, principal_min, principal_max,
  term_days, rate, collateral_type, forfeit_consequence)
* Templates: `debt_labor_garrison`, `debt_water_hydro`, `debt_arms_foundry`,
  `debt_salt_freeholders`, ... (10 total)

### Integration points
`LedgerDebtSystem` (add catalog load — minimal), `faction_lore.json`, `EconomySystem`,
`TradeTellEngine`, `HoldfastTradeSession`, existing 14 bounty raids.

### Substeps
1. Read `LedgerDebtSystem` contract-creation API.
2. Create `ledger_debt_templates.json` with `schema_version`.
3. Author 10 templates; 5 faction-specific, 5 generic.
4. Add catalog load to `LedgerDebtSystem` (minimal — **NEW SYSTEM JUSTIFICATION REQUIRED**).
5. Wire forfeit consequences to existing 14 bounty-raid hooks.
6. Wire faction reputation shifts on default.
7. Run integrity + balance-sim (debt economy — 2 coupled vars → cross-tool QA).
8. Add a debt round-trip test.
9. Add a determinism check on interest accrual.
10. Verify save compatibility with existing debt saves.

### Acceptance Criteria
10 templates load; faction-specific flavors work; forfeit hooks fire; integrity 0 errors;
debt round-trip green.

### QA / Validation
`--data-integrity-selftest`, `ashfall-balance-sim`, `ashfall-determinism-guard`, save migration.

### Follow-on opportunities
1. W5 steelworks strike uses a labor-debt template. 2. Existing 14 bounty raids consume
forfeit consequences. 3. W25 smuggling routes create off-book debt. 4. C14 debt→raid→reputation chain.

> **NEW SYSTEM JUSTIFICATION REQUIRED:** Loader-only change to `LedgerDebtSystem`; runtime
> contract params become JSON-driven defaults.

---

## TASK W19 — Shelter Room Definitions Catalog

**Category:** SYSTEM-CONTENT
**Priority:** P2
**Player Value:** MEDIUM
**Implementation Risk:** LOW
**Estimated Size:** MEDIUM

### Why this matters
`ShelterAssignmentSystem` (room assignment, capacity, compatibility) is fully implemented but
rooms are created only via runtime `CreateDefault()` — there are **no room definitions**.
Existing plan 29A covers room identity/history narratively; this is the **definition data** that
gives rooms mechanical identity (capacity, comfort, proximity rules).

### Existing systems reused
`ShelterAssignmentSystem`, `ShelterAssignmentHostSession`, `NeedsSystem` (comfort → morale),
`MemorialSystem`, `ShelterScheduleCatalogLoader` (3 schedules), `bunker_blueprints_codex.json`.

### World-content addition
* `shelter_rooms.json` with 12 room types
* Each room: capacity, comfort bonus, proximity rules, upgrade path, schedule compatibility
* 5 new decor items (links to existing 12C shelter decor)

### Gameplay loop
`Excavation unlocks room blueprint (W15) → ShelterAssignmentSystem loads room definition →
assign survivors (compatibility check) → comfort bonus → NeedsSystem morale →
upgrade path (materials) → schedule compatibility (W30)`

### Content specification
* `shelter_rooms.json`: `rooms[]` (id, capacity, comfort_bonus, proximity_rules,
  upgrade_path, schedule_ids)
* Rooms: `room_barracks`, `room_private_quarters`, `room_medical_bay`, `room_workshop`,
  `room_greenhouse_bay`, `room_armory`, `room_kitchen`, `room_radio_room`,
  `room_memorial_niche`, `room_storage`, `room_common_hall`, `room_isolation_ward`
* Items: `room_decor_poster`, `room_decor_plaque`, ... (5, link to existing 12C)

### Integration points
`ShelterAssignmentSystem` (add catalog load — minimal), `NeedsSystem`, `MemorialSystem`,
`ShelterScheduleCatalogLoader` (W30), `bunker_blueprints_codex.json`, existing 12C decor.

### Substeps
1. Read `ShelterAssignmentHostSession.CreateDefault()`; extract room params.
2. Create `shelter_rooms.json` with `schema_version`.
3. Author 12 room types with capacity/comfort/proximity/upgrade.
4. Add catalog load to `ShelterAssignmentSystem` (minimal — **NEW SYSTEM JUSTIFICATION REQUIRED**).
5. Wire comfort bonus to `NeedsSystem` morale.
6. Wire memorial niche to `MemorialSystem` (existing).
7. Wire schedule compatibility to `ShelterScheduleCatalogLoader` (W30).
8. Add 5 decor items; link to existing 12C decor slots.
9. Run integrity + balance-sim (comfort economy).
10. Add a room round-trip test.

### Acceptance Criteria
12 room types load; comfort affects morale; schedule compatibility works; integrity 0 errors;
room round-trip green.

### QA / Validation
`--data-integrity-selftest`, `ashfall-balance-sim`, snapshot diff (room panel).

### Follow-on opportunities
1. W15 excavation unlocks room blueprints. 2. W30 schedules attach to rooms. 3. Existing 12C
decor slots into rooms. 4. Existing 29 room identity/history narrates these. 5. C15
excavation→room→schedule→morale chain.

> **NEW SYSTEM JUSTIFICATION REQUIRED:** Loader-only change to `ShelterAssignmentSystem`; runtime
> `CreateDefault` becomes JSON-driven defaults.

---

## TASK W20 — Location-Specific Scavenging Tables

**Category:** SYSTEM-CONTENT
**Priority:** P1
**Player Value:** VERY HIGH
**Implementation Risk:** LOW
**Estimated Size:** LARGE

### Why this matters
Scavenging is currently generic — `items.json` pulls without location identity. A hospital
should yield medicine, a rail yard should yield mechanical parts. This is the single biggest
"exploration feels samey" gap. `scavenging_tables.json` is referenced by W1–W10.

### Existing systems reused
`items.json` (159), `EconomySystem`, `ExpeditionSystem`, `locations.json`, `economy_goods.json`,
`ISeededRng` (loot rolls).

### World-content addition
* `scavenging_tables.json` keyed by `loc_` id / location family
* ~60 table entries (one per major location across W1–W10 + existing locations)
* Each table: weighted item list, rarity, hazard modifier, depletion flag

### Gameplay loop
`Expedition to location → EconomySystem reads scavenging_tables[loc_id] →
weighted loot roll (ISeededRng) → hazard modifier (radiation/contamination) →
depletion tracking → location-specific reward identity`

### Content specification
* `scavenging_tables.json`: `tables[]` (loc_id, entries[{item_id, weight, rarity}],
  hazard_modifier, depletion_rate)
* Hospital → medical/surgical/chemicals/records/contaminated_waste
* Rail yard → mechanical_parts/fuel/tools/steel/electrical
* School → books/stationery/food_remnants/survivor_history
* Military depot → ammo/uniforms/comms/repair_parts/UXO
* (60 tables across all location families)

### Integration points
`EconomySystem` (add table lookup — minimal), `items.json`, `ExpeditionSystem`,
`locations.json`, `ISeededRng`, W1–W10 locations.

### Substeps
1. Read `EconomySystem` scavenging-yield path; identify where items.json is pulled.
2. Create `scavenging_tables.json` with `schema_version`.
3. Author ~60 tables; one per major location; weighted by logical loot identity.
4. Add table lookup to `EconomySystem` (minimal — **NEW SYSTEM JUSTIFICATION REQUIRED**: lookup, fallback to generic).
5. Wire hazard modifiers to `RadiationSystem`/`disease_catalog`.
6. Wire depletion tracking per location (one-time vs renewable).
7. Verify all `item_id` references resolve in `items.json`.
8. Run integrity + balance-sim (loot economy — 2 coupled vars → cross-tool QA).
9. Add a determinism check on loot rolls.
10. Add a test that tables produce location-appropriate loot.

### Acceptance Criteria
60 tables load; loot is location-appropriate; depletion works; all item refs resolve; integrity
0 errors; determinism green.

### QA / Validation
`--data-integrity-selftest`, `ashfall-balance-sim`, `ashfall-determinism-guard`.

### Follow-on opportunities
1. W1–W10 locations all get tables. 2. W11 micro-locations get one-time tables. 3. W33
greenhouse grows items that appear in rural tables. 4. C16 scavenging→economy→trade chain.

> **NEW SYSTEM JUSTIFICATION REQUIRED:** Adding table lookup to `EconomySystem` is a lookup-only
> change with generic fallback; no new gameplay system.

---

## TASK W21 — Collectibles Catalog: Photographs, Posters, Books, Badges

**Category:** ITEM
**Priority:** P2
**Player Value:** HIGH
**Implementation Risk:** LOW
**Estimated Size:** MEDIUM

### Why this matters
Collectibles are nearly absent — 1 vinyl item, 4 cassette sets. The pre-war culture is invisible.
Photographs, posters, books, badges, and patches are low-cost, high-flavor items that provide
morale, knowledge, recipe clues, and location hints. Extends existing plan 05 (vinyl) to a full
collectibles system.

### Existing systems reused
`items.json` (Media category — 2 items), `VinylMoraleSystem`, `JournalSystem` (codex unlocks),
`ResearchSystem` (W13 — books unlock nodes), `NeedsSystem` (morale), `CatalogIntegrityValidator`.

### World-content addition
* `collectibles.json` with 75 collectible items across 6 categories
* Photographs (15), posters (10), books/manuals (15), magazines (10), badges/patches (10), personal letters (15)
* Each: morale bonus, knowledge unlock, recipe clue, or location hint
* 5 codex unlocks

### Gameplay loop
`Scavenge collectible (W20 table) → item acquired →
morale bonus (NeedsSystem) or knowledge unlock (ResearchSystem W13) or
codex entry (JournalSystem) or location hint (W7 coordinate) →
collection completion bonus`

### Content specification
* `collectibles.json`: `items[]` (id, category, morale_bonus, knowledge_unlock, codex_entry,
  location_hint, rarity)
* Photographs: `item_photo_family_01` ... `item_photo_evac_queue_03`
* Books: `item_book_engineering_manual`, `item_book_field_medicine`, ...
* Badges: `item_badge_fire_brigade`, `item_badge_civil_defense`, ...

### Integration points
`items.json` (add 75 items, `item_` prefix), `VinylMoraleSystem` (generalize to collectibles —
minimal), `JournalSystem`, `ResearchSystem` (W13), `NeedsSystem`, `CatalogIntegrityValidator`.

### Substeps
1. Inventory existing Media/Document items; pick 75 snake_case ids.
2. Create `collectibles.json` with `schema_version`.
3. Author 75 items across 6 categories with morale/knowledge/codex/hint effects.
4. Add items to `items.json` with `item_` prefix.
5. Wire morale bonus to `NeedsSystem`.
6. Wire book/manual unlocks to `ResearchSystem` nodes (W13).
7. Wire codex unlocks to `JournalSystem`.
8. Wire location hints to W7 coordinates / W9 sites.
9. Add collectibles to W20 scavenging tables (rare slots).
10. Run integrity + narrative-continuity.

### Acceptance Criteria
75 items resolve; morale/knowledge/codex/hint effects fire; collectibles appear in scavenging
tables; integrity 0 errors.

### QA / Validation
`--data-integrity-selftest`, `ashfall-narrative-continuity`, `ashfall-balance-sim` (morale economy).

### Follow-on opportunities
1. W20 tables drop collectibles. 2. W7 books unlock research. 3. Existing 21 phantom triggers tie
to photographs. 4. W28 museum displays collectibles. 5. C17 collectible→codex→location chain.

---

## TASK W22 — Failed Evacuation Routes & Refugee Columns

**Category:** WORLD
**Priority:** P1
**Player Value:** VERY HIGH
**Implementation Risk:** MEDIUM
**Estimated Size:** LARGE

### Why this matters
The world has no signs of **population movement** — no failed evacuation routes, refugee
columns, or temporary camps. This is the core "the world exists independently of the player"
requirement. Connects W8 (villages), W2 (hospital evacuation), W4 (civic evacuation orders),
and existing 12A (war orphans).

### Existing systems reused
`locations.json` (W11 micro-locations as route evidence), `events.json`, `CohortSystem`
(refugees → children), `faction_lore.json` (faction response to refugees), `JournalSystem`,
`narrative/` (evacuation lists).

### World-content addition
* 6 evacuation-route corridors (each a chain of W11 micro-locations + 1 major site)
* 4 refugee-column encounters (moving groups, not static)
* 3 temporary-camp locations
* 1 crisis questline (the stranded column — 5 stages)
* 12 environmental documents (evacuation orders, route manifests, refugee rosters)
* 2 recurring NPCs (the column leader; the lost child)

### Gameplay loop
`Discover evacuation route (W11 micro-locations) → follow the trail →
encounter moving refugee column → moral decision (aid/ignore/exploit) →
temporary camp (trade/refugee recruitment) → crisis quest (stranded column needs
escort) → later: refugees join a settlement (W45) or feed faction war (existing 06C)`

### Content specification
* Routes: `route_evac_corridor_north`, `route_evac_corridor_river`, ... (6, chains of loc_micro_ ids)
* Camps: `loc_refugee_camp_riverside`, `loc_temporary_camp_crossroads`,
  `loc_displaced_camp_forest_edge`
* Quest: `quest_stranded_column` (5 stages)
* NPCs: `npc_column_leader_holm`, `npc_lost_child_mira`
* Encounters: `enc_refugee_column_moving`, `enc_refugee_column_sick`,
  `enc_refugee_column_blocked`, `enc_refugee_column_hostile_faction`

### Integration points
`locations.json`, `events.json`, `characters.json`, `CohortSystem` (children),
`faction_lore.json`, `narrative/`, flags `flag_column_aided`, `flag_child_rescued`,
`flag_refugees_settled`.

### Substeps
1. Define 6 evacuation corridors as chains of W11 micro-locations + 1 terminal major site.
2. Author 3 temporary-camp locations with trade/recruitment hooks.
3. Author 4 refugee-column encounters (moving groups — use `ExpeditionSystem` travel ticks).
4. Wire refugee children to `CohortSystem` (adoption — existing 12A).
5. Author the 5-stage crisis quest; escort branch uses `ExpeditionSystem`.
6. Add 2 recurring NPCs with temporal continuity (W44).
7. Wire faction response (iron_garrison = turn away; cult_of_ash_sign = recruit).
8. Author 12 documents; reuse evacuation-list format.
9. Wire `flag_refugees_settled` → W45 settlement allegiance.
10. Run integrity + narrative-continuity + dialog-graph-lint.

### Acceptance Criteria
6 routes resolve; 4 moving-column encounters fire; crisis quest playable; refugees feed
CohortSystem; integrity 0 errors.

### QA / Validation
`--data-integrity-selftest`, `ashfall-narrative-continuity`, `ashfall-dialog-graph-lint`,
`ashfall-balance-sim` (aid resource cost).

### Follow-on opportunities
1. W8 village receives refugees. 2. W2 hospital was an evacuation target. 3. W4 city hall
issued the orders. 4. W45 settlement allegiance shifts from refugee outcome. 5. C18
evacuation→refugee→settlement chain.

---

## TASK W23 — Fuel Shortage Crisis Chain

**Category:** WORLD
**Priority:** P2
**Player Value:** HIGH
**Implementation Risk:** MEDIUM
**Estimated Size:** MEDIUM

### Why this matters
`items.json` has only 4 Fuel items and no shortage narrative. Fuel is the lifeblood of
`ExpeditionVehicleSystem` (existing 10C) and the rail recovery (W27). A fuel shortage crisis
makes the industrial corridor (W5 refinery) and the economy (`economy_goods`) matter.

### Existing systems reused
`items.json` (Fuel), `EconomySystem` (dynamic pricing), `ExpeditionVehicleSystem`,
`LedgerDebtSystem` (W18 — fuel debt), `events.json`, `foundry_faction.json`,
`economy_goods.json`.

### World-content addition
* 1 crisis questline (the refinery dispute — 5 stages)
* 6 fuel-shortage events (price spikes, rationing, convoy raids, black-market fuel)
* 3 new fuel items (diesel drum, siphoned petrol, improvised biofuel)
* 4 environmental documents (fuel ration notices, convoy manifests, black-market ledgers)

### Gameplay loop
`Fuel price spike (EconomySystem) → refinery expedition (W5) →
faction dispute (foundry vs hydro_barons) → rationing event →
black-market / convoy-raid decision → long-term: vehicle expeditions cost more or
rail recovery (W27) stalls`

### Content specification
* Quest: `quest_refinery_dispute` (5 stages)
* Events: `enc_fuel_price_spike`, `enc_fuel_rationing`, `enc_fuel_convoy_raid`,
  `enc_blackmarket_fuel`, `enc_siphon_operation`, `enc_biofuel_improvisation`
* Items: `item_diesel_drum_sealed`, `item_siphoned_petrol`, `item_improvised_biofuel`

### Integration points
`items.json`, `EconomySystem` (dynamic pricing hooks), `ExpeditionVehicleSystem`,
`LedgerDebtSystem` (W18), `events.json`, `foundry_faction.json`, W5 refinery, flags
`flag_fuel_shortage_active`, `flag_refinery_control`.

### Substeps
1. Inventory Fuel items; add 3 new fuel types.
2. Author 6 fuel-shortage events; wire price spikes to `EconomySystem` dynamic pricing.
3. Wire convoy raids to `ExpeditionVehicleSystem` travel risk.
4. Author the 5-stage refinery dispute quest; ties W5 foundry vs hydro_barons.
5. Wire rationing to shelter fuel consumption (NeedsSystem warmth).
6. Wire black-market fuel to `LedgerDebtSystem` templates (W18).
7. Author 4 documents.
8. Run balance-sim (fuel economy — 2 coupled vars → cross-tool QA).
9. Add a determinism check on price spikes.
10. Run integrity + narrative-continuity.

### Acceptance Criteria
6 events fire; price spikes affect vehicle expeditions; quest playable; integrity 0 errors;
balance-sim green.

### QA / Validation
`--data-integrity-selftest`, `ashfall-balance-sim`, `ashfall-determinism-guard`.

### Follow-on opportunities
1. W5 refinery is the crisis center. 2. W27 rail recovery depends on fuel. 3. Existing 10C
vehicles consume fuel. 4. W25 smuggling moves black-market fuel. 5. C5 industrial-power-rail chain.

---

## TASK W24 — Famine & Food Shortage Crisis Chain

**Category:** WORLD
**Priority:** P2
**Player Value:** HIGH
**Implementation Risk:** MEDIUM
**Estimated Size:** MEDIUM

### Why this matters
Food is a core need (`NeedsSystem` hunger) but there is no **famine** world-state. The rural
hinterland (W8) and greenhouse (`greenhouse_items`, W33) need a crisis that makes food
acquisition a campaign-level concern. Connects to W22 (refugees) and existing 13A (seasonal
resource swing).

### Existing systems reused
`NeedsSystem` (hunger), `EconomySystem` (food pricing), `greenhouse_items.json` (W33),
`economy_goods.json`, `CohortSystem` (starving children), `WeatherSystem` (crop-failure
weather), `events.json`.

### World-content addition
* 1 crisis questline (the village famine — 5 stages, extends W8)
* 6 famine events (crop failure, ration riot, food convoy, seed vault expedition, cannibalism rumor, relief camp)
* 3 new food items (emergency ration block, foraged root, contaminated grain)
* 4 environmental documents (harvest ledgers, ration theft records, relief notices)

### Gameplay loop
`Crop-failure weather (WeatherSystem) → food price spike (EconomySystem) →
village famine quest (W8) → ration-riot event → seed-vault expedition (W7/W9) →
relief camp (moral: distribute or hoard) → long-term: greenhouse investment (W33)`

### Content specification
* Quest: `quest_village_famine_crisis` (5 stages, extends W8 `quest_village_famine`)
* Events: `enc_crop_failure`, `enc_ration_riot`, `enc_food_convoy_ambush`,
  `enc_seed_vault_expedition`, `enc_cannibalism_rumor`, `enc_relief_camp`
* Items: `item_emergency_ration_block`, `item_foraged_bitter_root`, `item_contaminated_grain`

### Integration points
`NeedsSystem`, `EconomySystem`, `greenhouse_items.json` (W33), `economy_goods.json`,
`CohortSystem`, `WeatherSystem`, `events.json`, W8 village, flags
`flag_famine_active`, `flag_seed_vault_recovered`, `flag_relief_distributed`.

### Substeps
1. Add 3 food items; wire contaminated grain to `disease_catalog`.
2. Author 6 famine events; wire crop failure to `WeatherSystem` seasonal state.
3. Wire food price spikes to `EconomySystem`.
4. Author the 5-stage famine quest; seed-vault stage ties W7/W9.
5. Wire ration riot to `CohortSystem` (children at risk).
6. Wire relief camp moral branch to `flag_relief_distributed` → W45.
7. Wire seed-vault recovery to `greenhouse_items` recipe unlock (W33).
8. Author 4 documents.
9. Run balance-sim (food economy — 2 coupled vars → cross-tool QA).
10. Run integrity + narrative-continuity.

### Acceptance Criteria
6 events fire; famine affects NeedsSystem; quest playable; greenhouse unlock works; integrity
0 errors; balance-sim green.

### QA / Validation
`--data-integrity-selftest`, `ashfall-balance-sim`, `ashfall-narrative-continuity`.

### Follow-on opportunities
1. W8 village is the crisis center. 2. W7/W9 seed vault is the solution. 3. W33 greenhouse
is the long-term fix. 4. W22 refugees worsen the famine. 5. C8 rural-famine-greenhouse chain.

---

## TASK W25 — Smuggling Routes & Black Markets

**Category:** WORLD
**Priority:** P2
**Player Value:** HIGH
**Implementation Risk:** MEDIUM
**Estimated Size:** MEDIUM

### Why this matters
`economy_goods.json` (16) and `TradeTellEngine` exist but there is no **smuggling** layer —
off-book trade that bypasses faction checkpoints (W43) and `LedgerDebtSystem` (W18). Smuggling
makes the economy feel alive and gives faction territory (W43) mechanical teeth.

### Existing systems reused
`EconomySystem`, `TradeTellEngine` (60 lines), `LedgerDebtSystem` (W18), `faction_lore.json`,
`HoldfastTradeSession`, `events.json`, `items.json` (Trade category — 5 items).

### World-content addition
* 4 smuggling-route locations (back trails, river crossings, tunnel passes, night markets)
* 6 smuggling encounters (bribe checkpoint, contraband cache, double-cross, informant, raid, rival smuggler)
* 1 questline (the salt-freeholders' contraband — 4 stages)
* 8 contraband items (untaxed salt, stolen medical supplies, black-market fuel, banned weapons)
* 6 environmental documents (smuggler ledgers, bribe receipts, route maps)

### Gameplay loop
`Discover smuggling route → bribe/stealth past checkpoint (W43) →
contraband trade (no LedgerDebtSystem record) → double-cross or raid risk →
rival smuggler competition → later: faction discovers route → checkpoint tightens`

### Content specification
* Locations: `loc_smuggle_river_crossing`, `loc_smuggle_back_trail`,
  `loc_smuggle_tunnel_pass`, `loc_night_market_hidden`
* Quest: `quest_salt_freeholders_contraband` (4 stages)
* Encounters: `enc_bribe_checkpoint`, `enc_contraband_cache`, `enc_smuggler_doublecross`,
  `enc_smuggler_informant`, `enc_smuggle_raid`, `enc_rival_smuggler`
* Items: `item_untaxed_salt_bricks`, `item_stolen_medical_supplies`,
  `item_blackmarket_fuel_drum`, `item_banned_weapon_crates`

### Integration points
`EconomySystem`, `TradeTellEngine`, `LedgerDebtSystem` (W18 — off-book vs on-book),
`faction_lore.json` (salt_freeholders, raiders), `HoldfastTradeSession`, `events.json`,
`items.json`, W43 checkpoints, flags `flag_smuggle_route_exposed`, `flag_contraband_deal`.

### Substeps
1. Author 4 smuggling-route locations; wire to W43 checkpoint network.
2. Author 6 encounters; bribe branch uses `EconomySystem` price; stealth uses SkillProgressionSystem (W12).
3. Author the 4-stage contraband quest; ties salt_freeholders vs raiders.
4. Add 8 contraband items to `items.json`.
5. Wire off-book trade to bypass `LedgerDebtSystem` (no debt record — risk: raid).
6. Author 6 documents; reuse smuggler-ledger format.
7. Wire `flag_smuggle_route_exposed` → W43 checkpoint tightens.
8. Run balance-sim (contraband economy — 2 coupled vars → cross-tool QA).
9. Run integrity + narrative-continuity.
10. Add a determinism check on raid rolls.

### Acceptance Criteria
4 routes resolve; 6 encounters fire; off-book trade works; exposure tightens checkpoints;
integrity 0 errors; balance-sim green.

### QA / Validation
`--data-integrity-selftest`, `ashfall-balance-sim`, `ashfall-determinism-guard`,
`ashfall-narrative-continuity`.

### Follow-on opportunities
1. W43 checkpoints are the obstacle. 2. W18 debt is the on-book alternative. 3. W23 black-market
fuel. 4. Existing 13 economy loop. 5. C19 smuggling→checkpoint→faction chain.

---

## TASK W26 — Contaminated River Systems

**Category:** LOCATION
**Priority:** P2
**Player Value:** MEDIUM
**Implementation Risk:** LOW
**Estimated Size:** MEDIUM

### Why this matters
`location_lethe_water_treatment` and `currents.json` (17) hint at water systems but rivers are
not a **contamination vector**. Rivers connect W8 (rural), W5 (industrial), existing 23
(maritime) and are the natural carrier for `disease_catalog` (cholera) and `RadiationSystem`
(irradiated water).

### Existing systems reused
`locations.json`, `currents.json` (17), `RadiationSystem` (irradiated water),
`disease_catalog.json` (waterborne), `WeatherSystem` (flooding), `EconomySystem` (water trade),
`events.json`.

### World-content addition
* 4 river-system locations (contaminated bend, industrial outflow, frozen crossing, river settlement)
* 6 contamination encounters (cholera cluster, irradiated water, dead fish, flooded well, river trade, water filter quest)
* 1 questline (the poisoned valley river — 4 stages)
* 5 environmental documents (water notices, contamination reports, fishing logs)

### Gameplay loop
`Expedition to river → contamination check (RadiationSystem / disease_catalog) →
water filter decision (item cost) → cholera cluster encounter (disease outbreak W36) →
river settlement trade (clean water premium) → later: upstream industrial site (W5) is the source`

### Content specification
* `loc_river_contaminated_bend`, `loc_river_industrial_outflow`,
  `loc_river_frozen_crossing`, `loc_river_settlement_pier`
* Quest: `quest_poisoned_valley_river` (4 stages)
* Encounters: `enc_cholera_cluster`, `enc_irradiated_water_source`, `enc_dead_fish_run`,
  `enc_flooded_well`, `enc_river_water_trade`, `enc_water_filter_shortage`

### Integration points
`locations.json`, `currents.json`, `RadiationSystem`, `disease_catalog.json`,
`WeatherSystem`, `EconomySystem` (water trade), `events.json`, W5 industrial, W8 rural,
existing 23 maritime, flags `flag_river_source_identified`.

### Substeps
1. Author 4 river locations; wire to `currents.json` flow dynamics.
2. Wire contamination to `RadiationSystem` (irradiated water) + `disease_catalog` (cholera — add 1 entry).
3. Author 6 encounters; cholera cluster ties to W36 outbreak.
4. Author the 4-stage quest; upstream source is a W5 industrial site.
5. Wire river settlement trade to `EconomySystem` (clean water premium).
6. Wire flooding to `WeatherSystem`.
7. Author 5 documents.
8. Run integrity + balance-sim (water economy).
9. Add a determinism check on contamination rolls.
10. Run narrative-continuity.

### Acceptance Criteria
4 locations resolve; contamination rolls deterministic; cholera ties to outbreak; trade works;
integrity 0 errors.

### QA / Validation
`--data-integrity-selftest`, `ashfall-balance-sim`, `ashfall-determinism-guard`.

### Follow-on opportunities
1. W5 industrial is the upstream source. 2. W8 rural river settlement. 3. W36 cholera outbreak.
4. Existing 23 maritime at the river mouth. 5. C20 river→disease→industry chain.

---

## TASK W27 — Dead Railway Systems & Rail Recovery

**Category:** WORLD
**Priority:** P2
**Player Value:** VERY HIGH
**Implementation Risk:** MEDIUM
**Estimated Size:** LARGE

### Why this matters
`faction_railway_guild` exists in `faction_lore.json` but the railway is not a **recoverable
system**. Rail recovery is the highest-value long-term infrastructure project: it connects
settlements (W45), enables caravans (existing 16B), and depends on fuel (W23) and power (W5).
This is a flagship cross-system chain (C5).

### Existing systems reused
`faction_lore.json` (railway_guild), `ExpeditionVehicleSystem`, `EconomySystem`,
`LedgerDebtSystem` (W18), `wasteland_map_v1.json` (W29), `events.json`, `foundry_faction.json`,
`characters.json`.

### World-content addition
* 5 rail locations (derelict station, rail yard, collapsed tunnel, rail bridge, rail depot)
* 1 major questline (restore the northern rail line — 7 stages, multi-system)
* 8 rail encounters (track patrol, derailed train, rail bandits, rail guild envoy, fuel convoy, repair crew, signal repair, tunnel clear)
* 1 recurring NPC (the rail guild engineer)
* 10 environmental documents (rail manifests, repair logs, guild charters, derailment reports)
* 3 unique items (`item_rail_signal_key`, `item_track_repair_kit`, `item_locomotive_part`)

### Gameplay loop
`Rail guild envoy (NPC) → derelict station expedition → track assessment →
fuel acquisition (W23) + substation power (W5) + repair crew (duty roster) →
collapsed tunnel clear (ExcavationSystem W15) → signal repair →
line restored → caravan travel improves (existing 16B) → later: faction competes to control the line (W43)`

### Content specification
* Locations: `loc_rail_derelict_station_north`, `loc_rail_yard_central`,
  `loc_rail_collapsed_tunnel_3`, `loc_rail_bridge_river`, `loc_rail_depot_south`
* Quest: `quest_restore_northern_rail_line` (7 stages)
* NPC: `npc_rail_guild_engineer_kell` (skill: mechanics, faction: railway_guild)
* Encounters: `enc_track_patrol`, `enc_derailed_train`, `enc_rail_bandits`,
  `enc_rail_guild_envoy`, `enc_fuel_convoy_rail`, `enc_repair_crew`, `enc_signal_repair`,
  `enc_tunnel_clear`

### Integration points
`faction_lore.json`, `ExpeditionVehicleSystem`, `EconomySystem`, `LedgerDebtSystem` (W18),
`wasteland_map_v1.json` (W29), `ExcavationSystem` (W15 — tunnel clear), `foundry_faction.json`
(steel for tracks), `events.json`, `characters.json`, `items.json`, flags
`flag_rail_line_north_restored`, `flag_rail_guild_allied`, `flag_rail_control_contested`.

### Substeps
1. Author 5 rail locations; wire to `wasteland_map_v1.json` route graph (W29).
2. Author the 7-stage quest; stages require fuel (W23), power (W5), crew (duty roster), tunnel clear (W15).
3. Wire line restoration to `ExpeditionVehicleSystem` (caravan speed multiplier).
4. Add `npc_rail_guild_engineer_kell`; wire to railway_guild faction.
5. Author 8 encounters; tunnel clear uses `ExcavationSystem` (W15).
6. Wire `flag_rail_line_north_restored` → existing 16B caravan network improves.
7. Wire `flag_rail_control_contested` → W43 faction territorialization.
8. Add 3 unique items; track repair kit uses `foundry_production` steel.
9. Author 10 documents; reuse rail-manifest format.
10. Run integrity + balance-sim (caravan economy — 2 coupled vars → cross-tool QA) + dialog-graph-lint.

### Acceptance Criteria
5 locations resolve; 7-stage quest playable; line restoration improves caravans; tunnel clear
uses excavation; integrity 0 errors; balance-sim green.

### QA / Validation
`--data-integrity-selftest`, `ashfall-balance-sim`, `ashfall-dialog-graph-lint`,
`ashfall-narrative-continuity`.

### Follow-on opportunities
1. W5 substation powers the line. 2. W23 fuel runs the locomotive. 3. W43 factions contest
control. 4. W45 settlements on the line grow. 5. C5 industrial-power-rail chain (flagship).

---

## TASK W28 — Pre-War Museum & Artifact Content

**Category:** LOCATION
**Priority:** P3
**Player Value:** MEDIUM
**Implementation Risk:** LOW
**Estimated Size:** MEDIUM

### Why this matters
No museum or curated-artifact location exists. A museum is the natural home for collectibles
(W21), relics (`relic_recipes`), and pre-war cultural context. It also anchors the
"pre-war world was a real society" requirement.

### Existing systems reused
`locations.json`, `relic_recipes.json` (6, W49), `items.json` (Document/Media),
`WorkshopReverseEngineeringSystem`, `JournalSystem` (codex), `narrative/`, W21 collectibles.

### World-content addition
* 1 museum complex (exterior + 4 galleries: natural history, art, industry, civic)
* 8 scavenging entries (artifacts, records, exhibit plaques, preserved specimens)
* 4 encounters (looter dispute, preserved-specimen hazard, curator's ghost-record, sealed vault)
* 1 questline (the curator's archive — 4 stages)
* 12 exhibit documents (placards, curator notes, acquisition records)
* 5 unique relics (for W49 workshop)

### Gameplay loop
`Expedition to museum → gallery scavenging (relics + collectibles) →
curator's ghost-record encounter (audio log) → sealed-vault quest (skill check) →
relic acquisition (W49 workshop restoration) → codex unlock (JournalSystem)`

### Content specification
* `loc_pre_war_museum_complex` with `zones[]`: `gallery_natural_history`,
  `gallery_art`, `gallery_industry`, `gallery_civic`, `sealed_vault`
* Quest: `quest_curators_archive` (4 stages)
* Encounters: `enc_museum_looter_dispute`, `enc_preserved_specimen_hazard`,
  `enc_curator_ghost_record`, `enc_museum_sealed_vault`
* Relics: `relic_amber_pendant`, `relic_brass_astrolabe`, `relic_wax_cylinder_player`,
  `relic_diorama_motor`, `relic_curators_lens`

### Integration points
`locations.json`, `relic_recipes.json` (W49), `items.json`, `WorkshopReverseEngineeringSystem`,
`JournalSystem`, `narrative/`, W21 collectibles, flags `flag_museum_vault_opened`,
`flag_curator_archive_read`.

### Substeps
1. Author the museum complex with 4 galleries + sealed vault.
2. Author 8 scavenging entries in `scavenging_tables.json` (W20).
3. Author 4 encounters; sealed vault uses SkillProgressionSystem (W12).
4. Author the 4-stage curator's archive quest; ghost-record is an audio log (existing 07).
5. Add 5 relics to `relic_recipes.json` (W49); wire to `WorkshopReverseEngineeringSystem`.
6. Author 12 exhibit documents.
7. Wire codex unlocks to `JournalSystem`.
8. Add collectible drops (W21) to gallery scavenging.
9. Run integrity + narrative-continuity.
10. Add reachability test for the quest.

### Acceptance Criteria
Museum resolves; 4 encounters fire; relics restore in workshop; codex unlocks; integrity 0 errors.

### QA / Validation
`--data-integrity-selftest`, `ashfall-narrative-continuity`, `ashfall-dialog-graph-lint`.

### Follow-on opportunities
1. W21 collectibles displayed here. 2. W49 relics restored from here. 3. W7 university lab
connects to the museum. 4. W4 civic district includes the museum. 5. C17 collectible→codex chain.

---

## TASK W29 — Wasteland Map Expansion & Sector Graph

**Category:** WORLD
**Priority:** P1
**Player Value:** VERY HIGH
**Implementation Risk:** MEDIUM
**Estimated Size:** LARGE

### Why this matters
`wasteland_map_v1.json` has **6 nodes / 7 routes**. Against ~284 location records, the map is a
skeleton. This is the data execution of existing plan 16A (6→60), expanded into a sector graph
that places W1–W10 locations, W22 refugee routes, W25 smuggling routes, and W27 rail lines.

### Existing systems reused
`wasteland_map_v1.json` (6 nodes), `locations.json` (W1–W10), `ExpeditionSystem` (route
travel), `EconomySystem` (caravan routes), `faction_lore.json` (territory), `WeatherSystem`
(route hazards), `currents.json`.

### World-content addition
* Expand `wasteland_map_v1.json` from 6 → 60 nodes across 8 sectors
* Expand routes from 7 → ~80 edges
* Each sector: biome, faction territory, weather modifier, hazard level
* Place all W1–W10 major locations + W11 micro-locations + W22/W25/W27 routes

### Gameplay loop
`Open map → sector reveals (fog of war via exploration) →
plan expedition route (weather/faction/hazard) → travel ticks →
encounter on route (W11/W25) → destination scavenging →
sector state evolves (W43 faction territory shifts)`

### Content specification
* `wasteland_map_v1.json`: `nodes[]` (60, each: id, name, sector, biome, loc_id, faction,
  weather_mod, hazard), `routes[]` (~80, each: from, to, distance, terrain, hazard, faction_control)
* Sectors: `sector_urban_belt`, `sector_rural_hinterland`, `sector_industrial_corridor`,
  `sector_military_frontier`, `sector_scientific_cluster`, `sector_subterranean`,
  `sector_wilderness_north`, `sector_river_delta`

### Integration points
`wasteland_map_v1.json`, `ExpeditionSystem`, `EconomySystem`, `faction_lore.json`,
`WeatherSystem`, `currents.json`, W1–W10 locations, W11 micro-locations, W22/W25/W27 routes,
W43 territory, `CatalogIntegrityValidator` (`sector_` prefix).

### Substeps
1. Read `wasteland_map_v1.json` schema; design 8-sector graph.
2. Author 60 nodes; place W1–W10 major locations + key existing loc_ ids.
3. Author ~80 routes; wire terrain/hazard/faction_control.
4. Wire sector weather modifiers to `WeatherSystem`.
5. Wire faction territory to `faction_lore.json` (W43).
6. Wire route hazards to `ExpeditionSystem` travel ticks.
7. Place W11 micro-locations as route events (not nodes).
8. Place W22 refugee corridors, W25 smuggling routes, W27 rail line as route chains.
9. Validate `sector_` prefix in `CatalogIntegrityValidator`.
10. Run integrity + balance-sim (travel economy) + a connectivity test (graph is connected).

### Acceptance Criteria
60 nodes / ~80 routes resolve; graph is connected; sectors have biome/faction/weather; all
W1–W10 locations placed; integrity 0 errors.

### QA / Validation
`--data-integrity-selftest`, `ashfall-balance-sim`, graph connectivity test.

### Follow-on opportunities
1. W43 territory shifts alter route faction_control. 2. W48 weather gating closes routes
seasonally. 3. Existing 16B caravans use the routes. 4. W27 rail is a special route type.
5. C21 map→faction→weather chain.

---

## TASK W30 — Shelter Schedule Expansion

**Category:** SYSTEM-CONTENT
**Priority:** P3
**Player Value:** LOW
**Implementation Risk:** LOW
**Estimated Size:** SMALL

### Why this matters
`shelter_schedules.json` has **3 schedules** (Standard, Night, Locked-Down Curfew) for a system
that drives fatigue recovery, lighting demand, and curfew enforcement. Thin for the shelter-life
depth (existing 12, W19).

### Existing systems reused
`ShelterScheduleCatalogLoader`, `ShelterAssignmentSystem` (W19), `NeedsSystem` (fatigue),
`events.json`, `CohortSystem`.

### World-content addition
* Expand `shelter_schedules.json` from 3 → 10 schedules
* Add: emergency, winter rationing, siege, quarantine, festival, mourning, shift-work, expedition-prep
* Each: lighting demand, fatigue recovery rate, curfew rules, morale modifier

### Gameplay loop
`Crisis triggers schedule change (siege / quarantine / famine) →
ShelterScheduleCatalogLoader loads schedule → fatigue/lighting/morale adjust →
curfew enforcement → later: schedule returns to normal`

### Content specification
* `shelter_schedules.json`: 10 entries (id, name, lighting_demand, fatigue_recovery,
  curfew_start, curfew_end, morale_mod)
* Schedules: `schedule_emergency`, `schedule_winter_rationing`, `schedule_siege`,
  `schedule_quarantine`, `schedule_festival`, `schedule_mourning`, `schedule_shift_work`,
  `schedule_expedition_prep`

### Integration points
`ShelterScheduleCatalogLoader`, `ShelterAssignmentSystem` (W19), `NeedsSystem`, `events.json`,
`CohortSystem`, W19 room compatibility.

### Substeps
1. Read `shelter_schedules.json` schema; confirm 3 existing.
2. Author 7 new schedules with distinct fatigue/lighting/curfew/morale.
3. Wire schedule triggers to crisis events (siege → W43, quarantine → W36 outbreak, famine → W24).
4. Wire fatigue recovery to `NeedsSystem`.
5. Wire curfew enforcement to shelter events.
6. Wire schedule-room compatibility (W19).
7. Run integrity + balance-sim (fatigue economy).
8. Add a schedule round-trip test.
9. Add a determinism check on schedule transitions.
10. Run narrative-continuity.

### Acceptance Criteria
10 schedules load; crisis triggers switch schedules; fatigue/lighting adjust; integrity 0 errors.

### QA / Validation
`--data-integrity-selftest`, `ashfall-balance-sim`, `ashfall-determinism-guard`.

### Follow-on opportunities
1. W19 rooms attach to schedules. 2. W24 famine triggers rationing schedule. 3. W43 siege
triggers siege schedule. 4. W36 outbreak triggers quarantine. 5. Existing 12 social life.

---

## TASK W31 — Phantom Trigger Expansion (Data Execution)

**Category:** NARRATIVE
**Priority:** P2
**Player Value:** MEDIUM
**Implementation Risk:** LOW
**Estimated Size:** MEDIUM

### Why this matters
`phantom_triggers.json` has **7** entries for a system that drives memory objects and heirlooms
(existing 21A targets 7→30). This is the data execution: 23 new triggers tied to W1–W10
locations and W21 collectibles.

### Existing systems reused
`phantom_triggers.json` (7), `VinylMoraleSystem` (flashback suppression), `MemorialSystem`,
`JournalSystem`, W21 collectibles (photographs), W1–W10 locations.

### World-content addition
* Expand `phantom_triggers.json` from 7 → 30 triggers
* Each trigger: location, object, memory text, morale effect, flashback flag
* Tie 15 triggers to W21 collectibles (photographs, letters)
* Tie 10 triggers to W1–W10 locations (apartment, hospital, school)

### Gameplay loop
`Scavenge memory object (W21 photo / W1 apartment) → phantom trigger fires →
memory text + morale effect → flashback (unless vinyl suppression) →
memorial entry (MemorialSystem) → later: heirloom inheritance (existing 21B)`

### Content specification
* `phantom_triggers.json`: 30 entries (id, loc_id, object_id, memory_text, morale_effect,
  flashback_flag)
* Triggers: `phantom_apartment_family_photo`, `phantom_hospital_childs_drawing`,
  `phantom_school_register`, `phantom_rail_station_ticket`, ... (23 new)

### Integration points
`phantom_triggers.json`, `VinylMoraleSystem`, `MemorialSystem`, `JournalSystem`, W21
collectibles, W1–W10 locations, `narrative/`.

### Substeps
1. Read `phantom_triggers.json` schema; confirm 7 existing.
2. Author 23 new triggers; 15 tied to W21 collectibles, 10 to W1–W10 locations.
3. Wire flashback suppression to `VinylMoraleSystem`.
4. Wire memorial creation to `MemorialSystem`.
5. Wire morale effect to `NeedsSystem`.
6. Author memory text using `ashfall-write` tone rules.
7. Run integrity + narrative-continuity.
8. Add a trigger round-trip test.
9. Add a determinism check on trigger firing.
10. Verify all object_id refs resolve in `items.json`/W21.

### Acceptance Criteria
30 triggers resolve; flashbacks suppressible; memorials created; integrity 0 errors.

### QA / Validation
`--data-integrity-selftest`, `ashfall-narrative-continuity`, `ashfall-determinism-guard`.

### Follow-on opportunities
1. W21 collectibles fire triggers. 2. Existing 21B heirlooms inherit from triggers. 3. W1
apartment triggers. 4. C22 collectible→phantom→memorial chain.

---

## TASK W32 — Confession & Secrets Expansion

**Category:** NARRATIVE
**Priority:** P2
**Player Value:** MEDIUM
**Implementation Risk:** LOW
**Estimated Size:** MEDIUM

### Why this matters
`confession_secrets.json` has **8** entries (existing 21C targets 8→18). This is the data
execution expanded to 25: secrets that feed Verdict evidence (W27), faction blackmail (W43),
and bunker events (existing 12B).

### Existing systems reused
`confession_secrets.json` (8), `JournalSystem`, `verdict_questlines.json` (W27),
`faction_lore.json` (W43 blackmail), `events.json`, `narrative/`.

### World-content addition
* Expand `confession_secrets.json` from 8 → 25 secrets
* Each: source NPC, faction, secret text, consequence, evidence flag
* Tie 8 to W27 Verdict evidence, 7 to W43 faction blackmail, 5 to W6 military desertion

### Gameplay loop
`Discover secret (W4 courthouse / W6 military / W9 archive) →
confession recorded → player decides: reveal (Verdict evidence W27) / blackmail (W43) / bury →
faction reputation shift → later: Verdict dossier (existing 15B) includes the secret`

### Content specification
* `confession_secrets.json`: 25 entries (id, source_npc, faction, secret_text, consequence,
  evidence_flag)
* Secrets: `secret_deserter_voss`, `secret_hydro_baron_water_theft`,
  `secret_foundry_labor_coverup`, `secret_courthouse_bribe`, ... (17 new)

### Integration points
`confession_secrets.json`, `verdict_questlines.json` (W27), `faction_lore.json` (W43),
`events.json`, `narrative/`, W4 courthouse, W6 military, W9 archive, flags
`flag_secret_revealed_*`.

### Substeps
1. Read `confession_secrets.json` schema; confirm 8 existing.
2. Author 17 new secrets; 8 Verdict, 7 blackmail, 5 military.
3. Wire reveal branch to `verdict_questlines.json` evidence (W27).
4. Wire blackmail branch to faction reputation (W43).
5. Wire bury branch to delayed consequence (flag).
6. Author secret text using `ashfall-write` tone rules.
7. Run integrity + narrative-continuity.
8. Add a secret round-trip test.
9. Verify all source_npc refs resolve in `characters.json`.
10. Run dialog-graph-lint.

### Acceptance Criteria
25 secrets resolve; reveal/blackmail/bury branches work; Verdict evidence receives secrets;
integrity 0 errors.

### QA / Validation
`--data-integrity-selftest`, `ashfall-narrative-continuity`, `ashfall-dialog-graph-lint`.

### Follow-on opportunities
1. W27 Verdict dossiers. 2. W43 faction blackmail. 3. W6 deserter secret. 4. Existing 15B
epilogue. 5. C23 secret→verdict→blackmail chain.

---

## TASK W33 — Greenhouse & Apiculture Food Depth

**Category:** SYSTEM-CONTENT
**Priority:** P2
**Player Value:** MEDIUM
**Implementation Risk:** LOW
**Estimated Size:** MEDIUM

### Why this matters
`greenhouse_items.json` has **14** entries (existing 22B targets depth). This is the data
execution expanded to 30: the long-term food solution for the famine crisis (W24) and the
source of bait crops (W14).

### Existing systems reused
`greenhouse_items.json` (14), `GreenhouseSystem`, `economy_goods.json`, `NeedsSystem` (food),
`WildlifeTrappingSystem` (W14 — bait crops), `WeatherSystem` (blight), W24 famine.

### World-content addition
* Expand `greenhouse_items.json` from 14 → 30 cultivars
* Add: blight-resistant strains, medicinal herbs, bait crops, contaminant-filtering plants
* 5 new greenhouse recipes
* 3 blight events (weather-linked)

### Gameplay loop
`Recover seed tin (W8) → greenhouse cultivation (greenhouse_items) →
blight event (WeatherSystem) → blight-resistant strain → food yield (NeedsSystem) →
bait crop (W14 trapping) → long-term famine solution (W24)`

### Content specification
* `greenhouse_items.json`: 30 entries (id, cultivar, yield, growth_days, blight_resistance,
  season, use)
* New: `cultivar_hardy_root_blight_resistant`, `cultivar_medicinal_herb_yarrow`,
  `cultivar_bait_crop_sweet_root`, `cultivar_filter_reed`, ... (16 new)
* Recipes: `recipe_greenhouse_bait_lure`, `recipe_greenhouse_herb_poultice`, ... (5)

### Integration points
`greenhouse_items.json`, `GreenhouseSystem`, `economy_goods.json`, `NeedsSystem`,
`WildlifeTrappingSystem` (W14), `WeatherSystem` (blight), W24 famine, W8 seed tin,
`CatalogIntegrityValidator` (`cultivar_` prefix — validate).

### Substeps
1. Read `greenhouse_items.json` schema; confirm 14 existing.
2. Validate `cultivar_` prefix in `CatalogIntegrityValidator`.
3. Author 16 new cultivars; 4 blight-resistant, 3 medicinal, 3 bait, 3 filtering, 3 food.
4. Wire bait crops to `WildlifeTrappingSystem` (W14).
5. Wire medicinal herbs to `pharma_recipes.json`.
6. Author 5 greenhouse recipes.
7. Author 3 blight events; wire to `WeatherSystem` contaminated-fog.
8. Wire blight-resistant strains to survive blight events.
9. Run integrity + balance-sim (food economy).
10. Add a greenhouse round-trip test.

### Acceptance Criteria
30 cultivars resolve; bait crops feed trapping; blight events work; integrity 0 errors;
balance-sim green.

### QA / Validation
`--data-integrity-selftest`, `ashfall-balance-sim`, `ashfall-determinism-guard`.

### Follow-on opportunities
1. W24 famine solution. 2. W14 bait crops. 3. W8 seed tin unlocks cultivars. 4. Existing 22B
apiculture. 5. C8 rural-famine-greenhouse chain.

---

## TASK W34 — Foundry Production Expansion

**Category:** SYSTEM-CONTENT
**Priority:** P2
**Player Value:** MEDIUM
**Implementation Risk:** LOW
**Estimated Size:** MEDIUM

### Why this matters
`foundry_production.json` has **11** entries (existing 22A targets 11→25). This is the data
execution expanded to 25: the source of armor materials (W16), track repair (W27), and
ordnance (existing #17 cloud-seeding shells).

### Existing systems reused
`foundry_production.json` (11), `foundry_faction.json`, `foundry_accords.json` (4),
`SkyLayerArmorSystem` (W16), W27 rail, `items.json` (Material), `EconomySystem`.

### World-content addition
* Expand `foundry_production.json` from 11 → 25 recipes
* Add: armor plate, track rail, reactive tile, ordnance shell, structural steel, tool heads
* 5 new material items

### Gameplay loop
`Acquire steel (W5 steelworks) → foundry production recipe →
armor plate (W16) / track rail (W27) / ordnance (#17) →
resource sink → faction labor (existing 22C)`

### Content specification
* `foundry_production.json`: 25 entries (id, output, inputs, labor, facility_tier)
* New: `recipe_armor_plate_steel`, `recipe_track_rail_segment`, `recipe_reactive_tile`,
  `recipe_ordnance_shell_cloud_seed`, `recipe_structural_beam`, ... (14 new)
* Items: `item_steel_plate_cast`, `item_track_rail_segment`, `item_reactive_tile_cast`,
  `item_ordnance_shell_empty`, `item_structural_beam`

### Integration points
`foundry_production.json`, `foundry_faction.json`, `foundry_accords.json`,
`SkyLayerArmorSystem` (W16), W27 rail, `items.json`, `EconomySystem`, existing #17
cloud-seeding, existing 22C labor.

### Substeps
1. Read `foundry_production.json` schema; confirm 11 existing.
2. Author 14 new recipes; armor/track/ordnance/structural.
3. Add 5 material items to `items.json`.
4. Wire armor plate to `SkyLayerArmorSystem` (W16).
5. Wire track rail to W27 rail recovery.
6. Wire ordnance shell to existing #17 cloud-seeding.
7. Wire structural steel to W5 steelworks output.
8. Wire labor cost to existing 22C foundry labor.
9. Run integrity + balance-sim (material economy).
10. Add a foundry round-trip test.

### Acceptance Criteria
25 recipes resolve; armor/track/ordnance outputs work; integrity 0 errors; balance-sim green.

### QA / Validation
`--data-integrity-selftest`, `ashfall-balance-sim`, `ashfall-determinism-guard`.

### Follow-on opportunities
1. W16 armor materials. 2. W27 track rails. 3. Existing #17 ordnance. 4. W5 steelworks supply.
5. C13 strike→armor→repair chain.

---

## TASK W35 — Chemical Dependency & Detox Expansion

**Category:** SYSTEM-CONTENT
**Priority:** P3
**Player Value:** MEDIUM
**Implementation Risk:** LOW
**Estimated Size:** MEDIUM

### Why this matters
`chemical_dependency_items.json` has **13** entries (existing 09B targets depth). This is the
data execution expanded to 25 substances + withdrawal-symptom tables + detox protocols,
deepening the medical interior (existing 27C).

### Existing systems reused
`chemical_dependency_items.json` (13), `ChemicalDependencySystem`, `MedicalSystem`,
`pharma_recipes.json` (25), `NeedsSystem` (morale), `disease_catalog.json`.

### World-content addition
* Expand `chemical_dependency_items.json` from 13 → 25 substances
* `withdrawal_symptoms.json` (new) — symptom progression tables
* `detox_protocols.json` (new) — medication-assisted detox
* 5 new substance items + 3 detox medications

### Gameplay loop
`Substance use (combat/morale/crafting) → dependency forms (ChemicalDependencySystem) →
withdrawal symptoms (withdrawal_symptoms.json) →
detox protocol (detox_protocols.json + medical supplies) →
managed detox vs cold turkey → morale/health consequence`

### Content specification
* `chemical_dependency_items.json`: 25 entries (12 new: nicotine, barbiturates, hallucinogens,
  stimulants, sedatives)
* `withdrawal_symptoms.json`: symptom tiers by substance
* `detox_protocols.json`: medication + duration + success rate

### Integration points
`chemical_dependency_items.json`, `ChemicalDependencySystem`, `MedicalSystem`,
`pharma_recipes.json`, `NeedsSystem`, `disease_catalog.json`, W2 hospital (detox clinic).

### Substeps
1. Read `chemical_dependency_items.json` schema; confirm 13 existing.
2. Author 12 new substances; add 5 items to `items.json`.
3. Create `withdrawal_symptoms.json` with tiered progression.
4. Create `detox_protocols.json` with medication-assisted paths.
5. Add 3 detox medications to `pharma_recipes.json`.
6. Wire withdrawal to `ChemicalDependencySystem` (minimal — read new catalogs).
7. Wire detox to `MedicalSystem` (W2 hospital clinic).
8. Run integrity + balance-sim (dependency economy).
9. Add a dependency round-trip test.
10. Add a determinism check on withdrawal rolls.

### Acceptance Criteria
25 substances load; withdrawal/detox protocols work; integrity 0 errors; balance-sim green.

### QA / Validation
`--data-integrity-selftest`, `ashfall-balance-sim`, `ashfall-determinism-guard`.

### Follow-on opportunities
1. W2 hospital detox clinic. 2. Existing 09B dependency depth. 3. Existing 27 body-and-mind.
4. C24 dependency→detox→hospital chain.

---

## TASK W36 — Disease Outbreak World Events

**Category:** ENCOUNTER
**Priority:** P2
**Player Value:** HIGH
**Implementation Risk:** MEDIUM
**Estimated Size:** MEDIUM

### Why this matters
`disease_catalog.json` has **7** pathogens (existing 09A targets 7→15) but there are no
**outbreak** world events — a cholera cluster, a radiation-sickness epidemic, a fungal
infection spread. Outbreaks make disease a world-state, not just a survivor affliction.
Connects W26 (rivers), W2 (hospital), W30 (quarantine schedule).

### Existing systems reused
`disease_catalog.json` (7), `MedicalSystem`, `WeatherSystem` (contaminated fog),
`CohortSystem` (children at risk), `events.json`, `shelter_schedules.json` (W30 quarantine),
W26 rivers, W2 hospital.

### World-content addition
* 8 outbreak world events (cholera, rad-fever, deep mold, frostbite-gangrene, fungal bloom, radiation cluster, dysentery, respiratory plague)
* Each: trigger, spread mechanic, location, severity, countermeasure
* 4 new disease entries (deep mold, frostbite-gangrene, fungal bloom, respiratory plague)
* 1 crisis questline (the hospital quarantine — 5 stages)

### Gameplay loop
`Outbreak trigger (weather / contaminated water / expedition exposure) →
disease spreads (MedicalSystem) → quarantine schedule (W30) →
countermeasure (clean water / chelation / isolation) →
hospital quest (W2) → later: route becomes contaminated if ignored`

### Content specification
* Events: `outbreak_cholera_riverside`, `outbreak_rad_fever_ruins`,
  `outbreak_deep_mold_excavation`, `outbreak_frostbite_gangrene_camp`,
  `outbreak_fungal_bloom_greenhouse`, `outbreak_radiation_cluster_hotspot`,
  `outbreak_dysentery_refugee`, `outbreak_respiratory_plague_shelter`
* Diseases: `disease_deep_mold`, `disease_frostbite_gangrene`,
  `disease_fungal_bloom`, `disease_respiratory_plague` (4 new in `disease_catalog.json`)
* Quest: `quest_hospital_quarantine_crisis` (5 stages)

### Integration points
`disease_catalog.json`, `MedicalSystem`, `WeatherSystem`, `CohortSystem`, `events.json`,
`shelter_schedules.json` (W30), W26 rivers, W2 hospital, flags
`flag_outbreak_contained`, `flag_route_contaminated`.

### Substeps
1. Add 4 new diseases to `disease_catalog.json`.
2. Author 8 outbreak events; wire triggers to `WeatherSystem`/water/exposure.
3. Wire spread to `MedicalSystem` (survivor-to-survivor via `CohortSystem` proximity).
4. Wire quarantine to `shelter_schedules.json` (W30).
5. Wire countermeasures to clean water / chelation / isolation items.
6. Author the 5-stage hospital quarantine quest; ties W2.
7. Wire `flag_route_contaminated` → W29 map route hazard.
8. Run balance-sim (disease spread — 2 coupled vars → cross-tool QA).
9. Add a determinism check on spread rolls.
10. Run integrity + narrative-continuity.

### Acceptance Criteria
8 outbreaks trigger; spread works deterministically; quarantine schedule switches; quest
playable; integrity 0 errors.

### QA / Validation
`--data-integrity-selftest`, `ashfall-balance-sim`, `ashfall-determinism-guard`,
`ashfall-narrative-continuity`.

### Follow-on opportunities
1. W26 river cholera. 2. W2 hospital quarantine. 3. W30 quarantine schedule. 4. W22 refugee
dysentery. 5. C20 river→disease→quarantine chain.

---

## TASK W37 — Trade Tell Line Expansion II (Faction-Specific)

**Category:** SYSTEM-CONTENT
**Priority:** P2
**Player Value:** MEDIUM
**Implementation Risk:** LOW
**Estimated Size:** MEDIUM

### Why this matters
`trade_tell_lines.json` has 60 lines (3 per stance×band combo). A player who trades frequently
sees repetition. Existing plan 07 (master roadmap #7) targets 4→40; this is the deeper expansion
to 240 with faction-specific variants.

### Existing systems reused
`TradeTellEngine` (5 stances × 4 trust bands), `faction_lore.json` (19 factions),
`HoldfastTradeSession`, `EconomySystem`.

### World-content addition
* Expand `trade_tell_lines.json` from 60 → 240 lines (8–10 per combo)
* Add faction-specific tell variants for 6 major factions
* Add expansion stances (holdfast, crossing)

### Gameplay loop
`Trade interaction → TradeTellEngine reads stance×band×faction →
tell line displayed → player reads intent → negotiation decision →
trust band shifts → tell variety reduces repetition`

### Content specification
* `trade_tell_lines.json`: 240 entries (stance, band, faction, line)
* Faction variants: iron_garrison, hydro_barons, salt_freeholders, railway_guild,
  ordnance_foundry, raiders

### Integration points
`TradeTellEngine`, `faction_lore.json`, `HoldfastTradeSession`, `EconomySystem`,
`CatalogIntegrityValidator`.

### Substeps
1. Read `trade_tell_lines.json` schema; confirm 60 existing.
2. Author 180 new lines; 8–10 per stance×band.
3. Add faction-specific variants for 6 factions.
4. Add holdfast/crossing expansion stances.
5. Author using `ashfall-write` tone rules.
6. Run integrity.
7. Add a tell-variety test (no repetition within 5 consecutive trades).
8. Verify all faction refs resolve.
9. Run narrative-continuity.
10. Add a determinism check on tell selection (seeded).

### Acceptance Criteria
240 lines load; faction variants work; no 5-trade repetition; integrity 0 errors.

### QA / Validation
`--data-integrity-selftest`, `ashfall-narrative-continuity`, `ashfall-determinism-guard`.

### Follow-on opportunities
1. W25 smuggling uses off-book tells. 2. W43 faction reputation affects band. 3. Existing 13
economy. 4. C19 smuggling→tell→faction chain.

---

## TASK W38 — Cassette & Vinyl Expansion II (Individual Records)

**Category:** ITEM
**Priority:** P3
**Player Value:** MEDIUM
**Implementation Risk:** LOW
**Estimated Size:** SMALL

### Why this matters
`cassette_sets.json` has **4** sets (existing 05 targets vinyl 1→20). This is the deeper
expansion: individual `vinyl_records.json` definitions with genre/morale/flashback-suppression,
feeding `VinylMoraleSystem` and W31 phantom triggers.

### Existing systems reused
`VinylMoraleSystem`, `cassette_sets.json` (4), `VinylRecordCatalog`, `NeedsSystem` (morale),
W31 phantom triggers (flashback suppression).

### World-content addition
* `vinyl_records.json` (new) — 20 individual record definitions
* Expand `cassette_sets.json` from 4 → 12 sets
* Each record: genre, morale_daily_bonus, flashback_suppression, audio_cue_id, rarity

### Gameplay loop
`Scavenge record (W20 table) → play in shelter (VinylMoraleSystem) →
daily morale bonus → flashback suppression (W31) →
rare record triggers cultural broadcast (radio) → collection completion`

### Content specification
* `vinyl_records.json`: 20 entries (id, genre, morale_bonus, flashback_suppression, audio_cue,
  rarity)
* Genres: folk, classical, protest, factory-work songs, children's, military marches
* `cassette_sets.json`: 8 new sets

### Integration points
`VinylMoraleSystem`, `VinylRecordCatalog`, `cassette_sets.json`, `NeedsSystem`, W31 phantom
triggers, `radio.json` (cultural broadcast), W20 scavenging, `CatalogIntegrityValidator`.

### Substeps
1. Read `VinylRecordCatalog` schema; confirm no `vinyl_records.json` exists.
2. Create `vinyl_records.json` with `schema_version`.
3. Author 20 records across 6 genres with distinct buff profiles.
4. Expand `cassette_sets.json` to 12 sets.
5. Wire playback to `VinylMoraleSystem`.
6. Wire flashback suppression to W31 triggers.
7. Wire rare records to `radio.json` cultural broadcasts.
8. Add records to W20 scavenging tables (rare slots).
9. Run integrity + balance-sim (morale economy).
10. Add a vinyl round-trip test.

### Acceptance Criteria
20 records load; playback gives morale; flashback suppression works; integrity 0 errors.

### QA / Validation
`--data-integrity-selftest`, `ashfall-balance-sim`, `ashfall-determinism-guard`.

### Follow-on opportunities
1. W31 phantom triggers suppressed by records. 2. W21 collectibles include records. 3. W28
museum displays records. 4. Existing 05 vinyl catalog. 5. C22 collectible→phantom→vinyl chain.

---

## TASK W39 — Dive Site Expansion II (Deep-Wreck Identity)

**Category:** LOCATION
**Priority:** P3
**Player Value:** MEDIUM
**Implementation Risk:** MEDIUM
**Estimated Size:** MEDIUM

### Why this matters
`dive_sites.json` has **4** entries (existing 23B targets 4→14). This is the data execution
with deep-wreck identity: each dive site has a distinct history, hazard, and loot signature,
feeding relic recovery (W49) and Verdict dossiers (existing 15B).

### Existing systems reused
`dive_sites.json` (4), `currents.json` (17), `black_flotilla_items.json`,
`ExpeditionVehicleSystem` (boat), `WorkshopReverseEngineeringSystem` (W49 relics),
`JournalSystem`, `narrative/`.

### World-content addition
* Expand `dive_sites.json` from 4 → 14 sites
* Each: wreck identity, depth, current hazard, loot signature, relic
* 6 new relic drops (for W49)
* 4 dive encounters (salvage dispute, decompression hazard, trapped wreck, sea creature)

### Gameplay loop
`Boat expedition to dive site → current hazard (currents.json) →
dive (depth/decompression) → wreck scavenging (relic + dossier) →
salvage dispute encounter → relic restoration (W49) →
Verdict dossier (existing 15B)`

### Content specification
* `dive_sites.json`: 14 entries (id, wreck_name, depth, current_id, hazard, loot_identity, relic_id)
* New sites: `dive_wreck_icebreaker`, `dive_wreck_cargo_freighter`,
  `dive_wreck_gunboat`, ... (10 new)
* Relics: `relic_brass_compass_marine`, `relic_sealed_log_book`, ... (6)

### Integration points
`dive_sites.json`, `currents.json`, `black_flotilla_items.json`, `ExpeditionVehicleSystem`,
`WorkshopReverseEngineeringSystem` (W49), `JournalSystem`, `narrative/`, existing 15B Verdict,
existing 23 maritime.

### Substeps
1. Read `dive_sites.json` schema; confirm 4 existing.
2. Author 10 new dive sites with wreck identity + current hazard.
3. Wire current hazards to `currents.json` flow dynamics.
4. Author 4 dive encounters; decompression uses SkillProgressionSystem (W12).
5. Add 6 relics to `relic_recipes.json` (W49).
6. Wire dossiers to existing 15B Verdict.
7. Author wreck-history documents.
8. Run integrity + balance-sim (dive economy).
9. Add a dive round-trip + determinism test.
10. Run narrative-continuity.

### Acceptance Criteria
14 sites resolve; current hazards work; relics restore; dossiers feed Verdict; integrity 0 errors.

### QA / Validation
`--data-integrity-selftest`, `ashfall-balance-sim`, `ashfall-determinism-guard`.

### Follow-on opportunities
1. W49 relic restoration. 2. Existing 15B Verdict dossiers. 3. Existing 23 maritime/flotilla.
4. W26 river mouth. 5. C25 dive→relic→verdict chain.

---

## TASK W40 — Muster Witness Expansion (Data Execution)

**Category:** NARRATIVE
**Priority:** P3
**Player Value:** MEDIUM
**Implementation Risk:** LOW
**Estimated Size:** SMALL

### Why this matters
`muster_witnesses.json` has **3** entries (existing 25B targets 3→15). This is the data
execution expanded to 15, feeding the late-game Muster spine (existing 25C) and epilogue
(existing 15A).

### Existing systems reused
`muster_witnesses.json` (3), `MusterSystem`, `verdict_questlines.json`, `JournalSystem`,
`narrative/`, existing 25B/25C, existing 15A.

### World-content addition
* Expand `muster_witnesses.json` from 3 → 15 witnesses
* Each: testimony, faction, reliability, evidence flag
* Tie 6 to W32 secrets, 4 to W6 military, 5 to W27 Verdict

### Gameplay loop
`Muster convenes (existing 25C) → witnesses called (muster_witnesses.json) →
testimony evaluated (reliability) → evidence cross-check (W32 secrets / W27 Verdict) →
verdict → epilogue (existing 15A)`

### Content specification
* `muster_witnesses.json`: 15 entries (id, name, faction, testimony, reliability, evidence_flag)
* Witnesses: `witness_deserter_voss`, `witness_hydro_baron_foreman`,
  `witness_hospital_nurse`, ... (12 new)

### Integration points
`muster_witnesses.json`, `MusterSystem`, `verdict_questlines.json`, `JournalSystem`,
`narrative/`, W32 secrets, W6 military, W27 Verdict, existing 25B/25C, existing 15A.

### Substeps
1. Read `muster_witnesses.json` schema; confirm 3 existing.
2. Author 12 new witnesses; 6 from W32 secrets, 4 from W6, 5 from W27.
3. Wire testimony reliability to cross-check flags.
4. Wire evidence to `verdict_questlines.json` (W27).
5. Author testimony using `ashfall-write` tone rules.
6. Wire verdict to existing 15A epilogue.
7. Run integrity + narrative-continuity + dialog-graph-lint.
8. Add a witness round-trip test.
9. Verify all evidence_flag refs resolve.
10. Run continuity check.

### Acceptance Criteria
15 witnesses resolve; testimony cross-checks work; verdict feeds epilogue; integrity 0 errors.

### QA / Validation
`--data-integrity-selftest`, `ashfall-narrative-continuity`, `ashfall-dialog-graph-lint`.

### Follow-on opportunities
1. W32 secrets become testimony. 2. W6 deserter testifies. 3. W27 Verdict receives evidence.
4. Existing 25C Muster spine. 5. C23 secret→witness→verdict chain.

---

## TASK W41 — Time-Based Campaign Bands (Early/Mid/Late Events)

**Category:** WORLD
**Priority:** P1
**Player Value:** VERY HIGH
**Implementation Risk:** MEDIUM
**Estimated Size:** LARGE

### Why this matters
ASHFALL has no **campaign-phase** content bands. The world should evolve: early confusion,
mid faction consolidation, late hardened blocs. This reuses `WeatherSystem`, `faction_lore`,
`events.json`, and `minDay`/`maxDay` (existing in `CatalogIntegrityValidator` ranges tier)
rather than inventing a campaign engine.

### Existing systems reused
`events.json` (77, `minDay`/`maxDay`), `faction_lore.json`, `WeatherSystem`,
`wasteland_map_v1.json` (W29), `CohortSystem`, `LedgerDebtSystem` (W18), `JournalSystem`,
`CatalogIntegrityValidator` (RANGES tier).

### World-content addition
* 30 phase-gated events across 3 bands (early Day 1–30, mid Day 31–120, late Day 121+)
* Early: confusion, emergency scavenging, refugees, collapsing services, temporary authority
* Mid: faction consolidation, shortages, territorial control, organized expeditions, black markets
* Late: hardened blocs, severe depletion, infrastructure projects, generational concerns, irreversible consequences
* 3 phase-transition crisis events (the authority collapse, the faction treaty, the long winter)

### Gameplay loop
`Day threshold crossed → phase-gated events unlock (minDay/maxDay) →
world-state shifts (faction territory W43, map W29, economy) →
phase-transition crisis → player adapts → late-game consequences (epilogue existing 15A)`

### Content specification
* `events.json`: 30 new entries with `minDay`/`maxDay` bands
* Early (10): `event_emergency_siren`, `event_refugee_wave_early`,
  `event_collapsing_water_service`, `event_temporary_authority_decree`, ...
* Mid (10): `event_faction_checkpoint_appears`, `event_fuel_shortage_mid`,
  `event_black_market_opens`, `event_organized_expedition_call`, ...
* Late (10): `event_long_winter_sets_in`, `event_faction_treaty_summit`,
  `event_generational_census`, `event_infrastructure_project_vote`, ...
* Crises: `crisis_authority_collapse`, `crisis_faction_treaty`, `crisis_long_winter`

### Integration points
`events.json` (`minDay`/`maxDay`), `faction_lore.json` (W43), `wasteland_map_v1.json` (W29),
`WeatherSystem`, `CohortSystem`, `LedgerDebtSystem` (W18), `JournalSystem`, existing 15A
epilogue, `CatalogIntegrityValidator` RANGES tier.

### Substeps
1. Read `events.json` schema; confirm `minDay`/`maxDay` fields exist.
2. Author 10 early-band events (Day 1–30); wire to refugees (W22), services collapse.
3. Author 10 mid-band events (Day 31–120); wire to faction consolidation (W43), shortages (W23/W24).
4. Author 10 late-band events (Day 121+); wire to infrastructure (W27 rail), generational (CohortSystem).
5. Author 3 phase-transition crises; wire to map state (W29) and economy.
6. Verify `minDay` < `maxDay` ordering (RANGES tier).
7. Wire late-band consequences to existing 15A epilogue axes.
8. Run integrity + balance-sim (phase economy shifts).
9. Add a determinism check on phase-gated event firing.
10. Run narrative-continuity + dialog-graph-lint.

### Acceptance Criteria
30 events fire on correct day bands; phase transitions trigger crises; late consequences feed
epilogue; RANGES tier passes; integrity 0 errors.

### QA / Validation
`--data-integrity-selftest` (RANGES tier), `ashfall-balance-sim`, `ashfall-determinism-guard`,
`ashfall-narrative-continuity`, `ashfall-dialog-graph-lint`.

### Follow-on opportunities
1. W22 refugees peak in early band. 2. W43 faction consolidation in mid band. 3. W27 rail
recovery in late band. 4. Existing 15A epilogue reads late-band state. 5. C26 campaign-band chain.

---

## TASK W42 — World-State Consequence Flag Network

**Category:** INTEGRATION
**Priority:** P1
**Player Value:** HIGH
**Implementation Risk:** LOW
**Estimated Size:** MEDIUM

### Why this matters
W1–W41 set dozens of `flag_*` consequences but there is no **central registry** ensuring flags
cross-reference correctly. This task audits, registers, and tests the flag network so earlier
actions reliably alter later content. Uses existing `InMemoryFlagLedger` (case-normalization
risk noted in AGENTS.md Invariant 4).

### Existing systems reused
`InMemoryFlagLedger`, `CatalogIntegrityValidator`, `ashfall-dialog-graph-lint`,
`ashfall-narrative-continuity`, all W1–W41 flags.

### World-content addition
* A `flag_registry.json` documenting every `flag_*` set by W1–W41
* Each flag: setter task, consumer tasks, consequence, day-gating
* Cross-reference validation in `CatalogIntegrityValidator` (new tier for flags)
* Fix `InMemoryFlagLedger` case-normalization drift (Invariant 4)

### Gameplay loop
`Player action sets flag → flag registry validates consumer exists →
later content reads flag → consequence fires → continuity lint passes`

### Content specification
* `flag_registry.json`: `flags[]` (id, setter, consumers[], consequence, day_gated)
* ~80 flags across W1–W41

### Integration points
`InMemoryFlagLedger`, `CatalogIntegrityValidator`, `ashfall-dialog-graph-lint`,
`ashfall-narrative-continuity`, W1–W41.

### Substeps
1. Grep all `flag_*` references across W1–W41 plans and existing data.
2. Create `flag_registry.json` documenting setter/consumer/consequence for each.
3. Add a `CatalogIntegrityValidator` tier for flag cross-references (setter must exist if consumer does).
4. Audit `InMemoryFlagLedger` for case-normalization drift (Invariant 4); normalize to `StringComparer.Ordinal`.
5. Add a flag-network reachability test (no orphan consumers).
6. Run `ashfall-dialog-graph-lint` over all flag-gated content.
7. Run `ashfall-narrative-continuity` over flag-dependent narrative.
8. Add a determinism check (flag state is save-stable).
9. Run integrity.
10. Document the flag network in the plan.

### Acceptance Criteria
`flag_registry.json` complete; no orphan consumers; `InMemoryFlagLedger` case-normalization
fixed; integrity 0 errors; continuity lint passes.

### QA / Validation
`--data-integrity-selftest`, `ashfall-dialog-graph-lint`, `ashfall-narrative-continuity`,
`ashfall-determinism-guard`, flag-network test.

### Follow-on opportunities
1. Every W1–W41 flag is registered. 2. Existing 15A epilogue reads the flag network. 3. C1–C30
chains depend on this. 4. W43 territory shifts are flag-driven. 5. NG+ (existing 15C) inherits flags.

---

## TASK W43 — Faction Territorialization & Patrol Behavior

**Category:** FACTION
**Priority:** P1
**Player Value:** VERY HIGH
**Implementation Risk:** MEDIUM
**Estimated Size:** LARGE

### Why this matters
19 factions have lore (`faction_lore.json`) but **no territorial behavior**. Factions should
visibly affect the map: checkpoints appear, roads become unsafe, patrols increase, prices
change, settlements shift allegiance. This is the core "world exists independently" faction
layer. Deepens existing 25A.

### Existing systems reused
`faction_lore.json` (19), `wasteland_map_v1.json` (W29 routes `faction_control`),
`EconomySystem` (regional pricing), `events.json`, `TradeTellEngine` (W37),
`LedgerDebtSystem` (W18), `TacticalCombatSystem`, `JournalSystem`.

### World-content addition
* `faction_territory.json` (new) — territory per faction per sector
* `faction_patrols.json` (new) — patrol routes, frequency, aggression
* 12 faction-situation events (checkpoint appears, road unsafe, trader disappears, patrol increases, price change, allegiance shift, refugee movement, radio change, resource scarcity, border skirmish, recruitment drive, tribute demand)
* 1 questline (the border war — 5 stages)

### Gameplay loop
`Faction territory shifts (flag/W41 band) → map route faction_control updates (W29) →
patrol encounters on routes → checkpoint event → regional price change (EconomySystem) →
allegiance shift (settlement W45) → border-war quest → later: faction war (existing 06C)`

### Content specification
* `faction_territory.json`: `territories[]` (faction_id, sector_id, control_level, neighbors)
* `faction_patrols.json`: `patrols[]` (faction_id, route_ids[], frequency, aggression, day_gated)
* Events: `enc_faction_checkpoint_appears`, `enc_road_unsafe_patrol`,
  `enc_trader_disappears_faction`, `enc_patrol_increase`, `enc_regional_price_change`,
  `enc_allegiance_shift`, `enc_refugee_movement_faction`, `enc_radio_message_change`,
  `enc_resource_scarcity_faction`, `enc_border_skirmish`, `enc_recruitment_drive`,
  `enc_tribute_demand`
* Quest: `quest_border_war_north` (5 stages)

### Integration points
`faction_lore.json`, `wasteland_map_v1.json` (W29), `EconomySystem`, `events.json`,
`TradeTellEngine` (W37), `LedgerDebtSystem` (W18), `TacticalCombatSystem`, `JournalSystem`,
W25 smuggling, W27 rail control, W45 settlements, existing 06C faction war, `CatalogIntegrityValidator`.

### Substeps
1. Read `faction_lore.json`; map 19 factions to W29 sectors.
2. Create `faction_territory.json` with control levels per sector.
3. Create `faction_patrols.json` with route-based patrols.
4. Wire `faction_control` on W29 routes to patrol spawn.
5. Author 12 faction-situation events; wire to territory shifts + W41 bands.
6. Wire regional price changes to `EconomySystem`.
7. Wire allegiance shifts to W45 settlements.
8. Author the 5-stage border-war quest; ties to existing 06C faction war.
9. Wire tribute demands to `LedgerDebtSystem` (W18).
10. Run integrity + balance-sim (territory economy — 2 coupled vars → cross-tool QA) + dialog-graph-lint.

### Acceptance Criteria
Territory/patrol catalogs load; 12 events fire; routes update faction_control; prices shift;
quest playable; integrity 0 errors; balance-sim green.

### QA / Validation
`--data-integrity-selftest`, `ashfall-balance-sim`, `ashfall-determinism-guard`,
`ashfall-dialog-graph-lint`, `ashfall-narrative-continuity`.

### Follow-on opportunities
1. W29 map shows territory. 2. W25 smuggling bypasses checkpoints. 3. W27 rail control
contested. 4. W45 settlements shift allegiance. 5. C27 faction-territory-economy chain.

---

## TASK W44 — Recurring NPC Temporal Continuity

**Category:** NPC
**Priority:** P1
**Player Value:** HIGH
**Implementation Risk:** MEDIUM
**Estimated Size:** MEDIUM

### Why this matters
`characters.json` has 36 NPCs but they are mostly one-state. Temporal continuity — a trader
met on Day 15 reappears injured on Day 40, becomes a faction official by Day 80 — is the core
"the world remembers" requirement. Deepens existing 20B.

### Existing systems reused
`characters.json` (36), `faction_lore.json`, `events.json` (`minDay`/`maxDay`),
`JournalSystem`, `InMemoryFlagLedger` (W42), W41 bands, W1–W10 NPCs.

### World-content addition
* 12 recurring NPCs with 3-state arcs (early/mid/late)
* Each NPC: occupation, skill, weakness, objective, faction, 3 state-variants by day band
* 24 state-transition events (NPC reappears changed)
* 6 NPC-specific quests (personal arcs)

### Gameplay loop
`Meet NPC early (W1–W10) → flag set → NPC reappears mid (state changed by flag) →
NPC reappears late (faction official / dead / ally) → personal quest →
later: NPC state affects faction war (existing 06C) / epilogue (existing 15A)`

### Content specification
* `characters.json`: 12 new NPCs + 3 state-variants each
* NPCs: `npc_trader_holm` (Day 15 trader → Day 40 injured → Day 80 faction official),
  `npc_lost_hiker_mira` (Day 10 lost → Day 50 shelter recruit → Day 90 scout),
  `npc_deserter_voss` (W6), `npc_last_nurse_ianov` (W2), `npc_rail_engineer_kell` (W27),
  `npc_column_leader_holm` (W22), `npc_curator_ghost` (W28), ...
* Events: 24 state-transition events with `minDay`/`maxDay` + flag prerequisites

### Integration points
`characters.json`, `faction_lore.json`, `events.json` (`minDay`/`maxDay`), `JournalSystem`,
`InMemoryFlagLedger` (W42), W41 bands, W1–W10 NPCs, existing 06C/15A, `CatalogIntegrityValidator`.

### Substeps
1. Read `characters.json` schema; confirm NPC fields.
2. Author 12 recurring NPCs with 3 state-variants (early/mid/late).
3. Author 24 state-transition events; gate by `minDay`/`maxDay` + flag prerequisites (W42).
4. Wire NPC state to faction affiliation changes (W43).
5. Author 6 personal-arc quests; tie to W1–W10 locations.
6. Wire late-state NPCs to existing 06C faction war / 15A epilogue.
7. Add NPCs to W29 map locations.
8. Run integrity + narrative-continuity + dialog-graph-lint.
9. Add an NPC state round-trip test.
10. Verify all flag prerequisites resolve (W42).

### Acceptance Criteria
12 NPCs with 3 states each; transitions fire on day+flag; personal quests playable; integrity
0 errors; continuity lint passes.

### QA / Validation
`--data-integrity-selftest`, `ashfall-narrative-continuity`, `ashfall-dialog-graph-lint`,
`ashfall-determinism-guard`.

### Follow-on opportunities
1. W1–W10 NPCs recur. 2. W43 faction affiliation changes. 3. Existing 06C/15A use NPC state.
4. W42 flags drive transitions. 5. C28 NPC-arc chain.

---

## TASK W45 — Settlement Allegiance & Growth

**Category:** FACTION
**Priority:** P2
**Player Value:** HIGH
**Implementation Risk:** MEDIUM
**Estimated Size:** LARGE

### Why this matters
No settlement catalog exists — settlements are implicit in `wasteland_map_v1.json` (6 nodes).
Settlements that grow, shift allegiance (W43), and offer trade/quests are the social backbone.
Connects W22 (refugees), W27 (rail), W43 (territory).

### Existing systems reused
`wasteland_map_v1.json` (W29 nodes), `faction_lore.json` (W43), `EconomySystem`,
`TradeTellEngine` (W37), `CohortSystem`, `events.json`, `JournalSystem`, `narrative/`.

### World-content addition
* `settlements.json` (new) — 12 settlements with population, allegiance, trade, quests, growth
* Each: population, faction, trade_goods, shortages, quest_hooks, growth_stage, allegiance_history
* 8 settlement-situation events (allegiance shift, trade embargo, population growth, shortage, refugee influx, disease outbreak, faction recruitment, market fluctuation)
* 1 questline (the settlement alliance — 5 stages)

### Gameplay loop
`Discover settlement (W29 node) → trade (EconomySystem) →
allegiance shift (W43 territory) → settlement grows (growth_stage) →
refugee influx (W22) / rail connection (W27) → alliance quest →
later: settlement bloc affects faction war (existing 06C)`

### Content specification
* `settlements.json`: `settlements[]` (id, name, node_id, population, faction, trade_goods,
  shortages, growth_stage, allegiance_history)
* Settlements: `settlement_riverside_market`, `settlement_crossroads_town`,
  `settlement_rail_junction`, `settlement_forest_refuge`, `settlement_lakeside_fishing`, ...
* Events: `enc_settlement_allegiance_shift`, `enc_settlement_trade_embargo`,
  `enc_settlement_population_growth`, `enc_settlement_shortage`,
  `enc_settlement_refugee_influx`, `enc_settlement_disease_outbreak`,
  `enc_settlement_recruitment`, `enc_settlement_market_fluctuation`
* Quest: `quest_settlement_alliance` (5 stages)

### Integration points
`wasteland_map_v1.json` (W29), `faction_lore.json` (W43), `EconomySystem`, `TradeTellEngine`
(W37), `CohortSystem`, `events.json`, `JournalSystem`, W22 refugees, W27 rail, W43 territory,
existing 06C, `CatalogIntegrityValidator` (`settlement_` prefix — validate).

### Substeps
1. Validate `settlement_` prefix in `CatalogIntegrityValidator`.
2. Create `settlements.json` with 12 settlements; place on W29 nodes.
3. Wire allegiance to `faction_lore.json` (W43).
4. Wire trade to `EconomySystem` + `TradeTellEngine` (W37).
5. Author 8 settlement-situation events; gate by W41 bands + W43 territory.
6. Wire refugee influx to W22; rail connection to W27.
7. Author the 5-stage alliance quest; ties settlements into a bloc.
8. Wire settlement bloc to existing 06C faction war.
9. Run integrity + balance-sim (settlement economy — 2 coupled vars → cross-tool QA).
10. Run narrative-continuity + dialog-graph-lint.

### Acceptance Criteria
12 settlements resolve; allegiance shifts work; trade/growth functional; alliance quest
playable; integrity 0 errors; balance-sim green.

### QA / Validation
`--data-integrity-selftest`, `ashfall-balance-sim`, `ashfall-narrative-continuity`,
`ashfall-dialog-graph-lint`.

### Follow-on opportunities
1. W29 map nodes become settlements. 2. W43 territory shifts allegiance. 3. W22 refugees grow
population. 4. W27 rail connects settlements. 5. C27 faction-territory-settlement chain.

---

## TASK W46 — Religious Movements & Cult Expansion

**Category:** FACTION
**Priority:** P3
**Player Value:** MEDIUM
**Implementation Risk:** LOW
**Estimated Size:** MEDIUM

### Why this matters
`cult_of_the_glow` and `cult_of_ash_sign` exist but belief movements have no **behavioral**
content. Religious movements — schisms, pilgrimages, proselytizing, shrine-building — add a
distinct social texture. Deepens existing 30C.

### Existing systems reused
`faction_lore.json` (cult_of_the_glow, cult_of_ash_sign), `events.json`, `CohortSystem`,
`MemorialSystem`, `JournalSystem`, `narrative/` (bunker_children_folklore), W8 monastery,
W11 shrines.

### World-content addition
* 3 new belief movements (the Frozen Pilgrims, the Ash Reckoners, the Silent Congregation)
* 8 movement events (schism, pilgrimage, proselytizing, shrine-building, heresy trial, miracle rumor, donation drive, prophecy)
* 1 questline (the monastery schism — 5 stages, extends W8)
* 6 religious documents (scriptures, pilgrimage logs, heresy records)

### Gameplay loop
`Encounter movement (W11 shrine / W8 monastery) → proselytizing event →
player joins/ignores/opposes → schism quest → pilgrimage (expedition) →
prophecy (world-state flag) → later: movement affects faction war (existing 06C)`

### Content specification
* Movements: `faction_frozen_pilgrims`, `faction_ash_reckoners`, `faction_silent_congregation`
* Events: `enc_schism`, `enc_pilgrimage`, `enc_proselytizing`, `enc_shrine_building`,
  `enc_heresy_trial`, `enc_miracle_rumor`, `enc_donation_drive`, `enc_prophecy`
* Quest: `quest_monastery_schism_crisis` (5 stages, extends W8)

### Integration points
`faction_lore.json`, `events.json`, `CohortSystem`, `MemorialSystem`, `JournalSystem`,
`narrative/`, W8 monastery, W11 shrines, existing 30C, existing 06C, `CatalogIntegrityValidator`.

### Substeps
1. Add 3 movements to `faction_lore.json`; validate `faction_` prefix.
2. Author 8 movement events; wire schism to W8 monastery.
3. Wire pilgrimage to `ExpeditionSystem` (travel to a shrine).
4. Wire shrine-building to W11 micro-locations.
5. Author the 5-stage schism quest; extends W8.
6. Wire prophecy to a world-state flag (W42) affecting existing 06C.
7. Author 6 religious documents; reuse folklore format.
8. Wire donation drive to `EconomySystem`.
9. Run integrity + narrative-continuity + dialog-graph-lint.
10. Add a movement round-trip test.

### Acceptance Criteria
3 movements resolve; 8 events fire; schism quest playable; prophecy flag set; integrity 0 errors.

### QA / Validation
`--data-integrity-selftest`, `ashfall-narrative-continuity`, `ashfall-dialog-graph-lint`.

### Follow-on opportunities
1. W8 monastery schism. 2. W11 shrines. 3. Existing 30C belief movements. 4. Existing 06C
faction war. 5. C29 belief→schism→war chain.

---

## TASK W47 — Hidden Bunker Competition

**Category:** WORLD
**Priority:** P2
**Player Value:** HIGH
**Implementation Risk:** MEDIUM
**Estimated Size:** MEDIUM

### Why this matters
W9 introduces sealed bunkers but once opened they are static. The design requirement is:
"Reveal hidden bunker → factions compete for it." This makes bunker discovery a world-state
crisis, not just a loot pinata. Connects W9, W43, existing 06C.

### Existing systems reused
`locations.json` (W9 bunkers), `faction_lore.json` (W43), `events.json`, `LedgerDebtSystem`
(W18), `TacticalCombatSystem`, `JournalSystem`, W42 flags.

### World-content addition
* 6 bunker-competition events (faction claim, siege, negotiation, auction, sabotage, shared-use treaty)
* 1 questline (the bunker kappa dispute — 5 stages, extends W9)
* 3 bunker-control outcomes (player keeps, faction takes, shared treaty)

### Gameplay loop
`Open bunker (W9, flag set) → faction-claim event →
player decides: keep / negotiate / fight / auction →
siege or treaty → long-term: bunker becomes a settlement (W45) or faction stronghold (W43)`

### Content specification
* Events: `enc_bunker_faction_claim`, `enc_bunker_siege`, `enc_bunker_negotiation`,
  `enc_bunker_auction`, `enc_bunker_sabotage`, `enc_bunker_shared_treaty`
* Quest: `quest_bunker_kappa_dispute` (5 stages, extends W9)
* Outcomes: `flag_bunker_player_controlled`, `flag_bunker_faction_controlled`,
  `flag_bunker_shared_treaty`

### Integration points
`locations.json` (W9), `faction_lore.json` (W43), `events.json`, `LedgerDebtSystem` (W18),
`TacticalCombatSystem`, `JournalSystem`, W42 flags, W45 settlements, existing 06C.

### Substeps
1. Wire bunker-open flags (W9) to trigger competition events.
2. Author 6 competition events; claim event fires on `flag_bunker_kappa_opened`.
3. Author the 5-stage dispute quest; branches to 3 outcomes.
4. Wire siege to `TacticalCombatSystem`.
5. Wire auction to `LedgerDebtSystem` (W18) + `EconomySystem`.
6. Wire shared-treaty to W43 faction diplomacy.
7. Wire outcomes to W45 (bunker becomes settlement) / W43 (faction stronghold).
8. Run integrity + balance-sim (bunker economy) + dialog-graph-lint.
9. Add a bunker-competition round-trip test.
10. Run narrative-continuity.

### Acceptance Criteria
6 events fire on bunker-open; 3 outcomes reachable; quest playable; integrity 0 errors.

### QA / Validation
`--data-integrity-selftest`, `ashfall-balance-sim`, `ashfall-dialog-graph-lint`,
`ashfall-narrative-continuity`.

### Follow-on opportunities
1. W9 bunkers trigger this. 2. W43 factions compete. 3. W45 bunker-settlement. 4. Existing 06C
faction war. 5. C30 bunker→competition→settlement chain.

---

## TASK W48 — Weather-Linked Expedition Gating

**Category:** WORLD
**Priority:** P2
**Player Value:** HIGH
**Implementation Risk:** LOW
**Estimated Size:** MEDIUM

### Why this matters
22 weather states exist but expeditions are not **weather-gated**. A fallout storm should close
surface routes, a blizzard should close mountain passes, contaminated fog should raise
encounter risk. This is pure data wiring into `ExpeditionSystem` + `WeatherSystem`. Connects
W10 (wilderness), W29 (map), W41 (bands).

### Existing systems reused
`WeatherSystem` (22 states), `ExpeditionSystem`, `wasteland_map_v1.json` (W29 routes),
`events.json`, `RadiationSystem`, `currents.json`.

### World-content addition
* `weather_route_gates.json` (new) — route open/closed/risk by weather state
* 22 weather-state → route-effect mappings
* 8 weather-gated encounter variants (storm-bound, blizzard-stranded, fog-ambush, acid-rain-exposure)
* 4 weather-closed-route crisis events

### Gameplay loop
`Weather state changes (WeatherSystem) → route gate updates (W29) →
expedition blocked or risk-raised → player waits or braves the weather →
weather-gated encounter → later: seasonal route closures (W41 bands)`

### Content specification
* `weather_route_gates.json`: `gates[]` (weather_state, route_terrain, effect: open/closed/risk, encounter_id)
* Mappings: fallout_storm → surface routes closed; blizzard → mountain passes closed;
  contaminated_fog → risk raised; acid_snow → exposure damage; emp → vehicle failure
* Encounters: `enc_storm_bound`, `enc_blizzard_stranded`, `enc_fog_ambush`,
  `enc_acid_rain_exposure`, ... (8)

### Integration points
`WeatherSystem`, `ExpeditionSystem`, `wasteland_map_v1.json` (W29), `events.json`,
`RadiationSystem`, `currents.json`, W10 wilderness, W41 bands, `CatalogIntegrityValidator`.

### Substeps
1. Read `WeatherSystem` state keys; confirm 22 states.
2. Create `weather_route_gates.json` with `schema_version`.
3. Author 22 weather→route-effect mappings by terrain type.
4. Wire gates to `ExpeditionSystem` route availability (minimal — read gate table).
5. Author 8 weather-gated encounters; wire to gate effects.
6. Wire radiation exposure to `RadiationSystem`.
7. Wire vehicle failure (EMP) to `ExpeditionVehicleSystem`.
8. Wire seasonal closures to W41 bands.
9. Run integrity + balance-sim (travel economy) + determinism (weather rolls).
10. Add a weather-gate round-trip test.

### Acceptance Criteria
22 gates load; routes open/close by weather; encounters fire; integrity 0 errors; determinism green.

### QA / Validation
`--data-integrity-selftest`, `ashfall-balance-sim`, `ashfall-determinism-guard`.

### Follow-on opportunities
1. W10 wilderness routes close in storms. 2. W29 map shows closures. 3. W41 bands add seasonal
closures. 4. Existing 13C weather crisis. 5. C10 weather→wildlife→gating chain.

---

## TASK W49 — Workshop Relic Component & Multi-Stage Restoration

**Category:** ITEM
**Priority:** P2
**Player Value:** MEDIUM
**Implementation Risk:** LOW
**Estimated Size:** MEDIUM

### Why this matters
`relic_recipes.json` has **6** relics (existing 04 targets 6→30). This is the deeper expansion:
a **component catalog** + multi-stage restoration chains so relics are a progression spine,
not a one-shot repair. Connects W28 (museum), W39 (dive relics).

### Existing systems reused
`WorkshopReverseEngineeringSystem`, `relic_recipes.json` (6), `items.json` (Component),
`NeedsSystem` (morale bonus), `JournalSystem`, W28 museum, W39 dive relics.

### World-content addition
* Expand `relic_recipes.json` from 6 → 30 relics (existing 04 wave 1) + component catalog
* `relic_components.json` (new) — components per relic, multi-stage restoration steps
* 24 new relics (12 from W28 museum, 6 from W39 dives, 6 standalone)
* 5 multi-stage restoration chains (3–5 steps each)

### Gameplay loop
`Scavenge relic (W28 museum / W39 dive / W20 table) → workshop →
component identification (relic_components) → multi-stage restoration (components + skill) →
morale bonus + dialogue event + world flag → later: relic unlocks a quest or codex`

### Content specification
* `relic_recipes.json`: 30 entries (24 new)
* `relic_components.json`: `components[]` (relic_id, stages[{step, component_id, skill_id, days}])
* Relics: `relic_sewing_machine`, `relic_clock_tower_movement`, `relic_telescope_brass`,
  `relic_printing_press`, `relic_film_projector_restored`, ... (24 new)
* Components: `item_clock_gear_set`, `item_lens_grinding_kit`, `item_print_type_case`, ...

### Integration points
`WorkshopReverseEngineeringSystem`, `relic_recipes.json`, `items.json` (Component),
`NeedsSystem`, `JournalSystem`, SkillProgressionSystem (W12), W28 museum, W39 dives, W20
tables, `CatalogIntegrityValidator` (`relic_` prefix).

### Substeps
1. Read `relic_recipes.json` schema; confirm 6 existing.
2. Author 24 new relics; 12 museum, 6 dive, 6 standalone.
3. Create `relic_components.json` with multi-stage restoration for 5 complex relics.
4. Wire component scavenging to W20 tables + W28/W39.
5. Wire restoration skill checks to SkillProgressionSystem (W12).
6. Wire morale bonus to `NeedsSystem`.
7. Wire dialogue events + world flags (W42).
8. Add components to `items.json` (Component category).
9. Run integrity + balance-sim (relic economy).
10. Add a relic restoration round-trip test.

### Acceptance Criteria
30 relics resolve; multi-stage restoration works; components scavengable; morale bonus fires;
integrity 0 errors.

### QA / Validation
`--data-integrity-selftest`, `ashfall-balance-sim`, `ashfall-determinism-guard`.

### Follow-on opportunities
1. W28 museum relics. 2. W39 dive relics. 3. Existing 04 wave 1. 4. W12 skill checks. 5. C17
collectible→relic→codex chain.

---

## TASK W50 — Cross-System Mega-Chain Integration Audit

**Category:** INTEGRATION
**Priority:** P1
**Player Value:** HIGH
**Implementation Risk:** MEDIUM
**Estimated Size:** MEDIUM

### Why this matters
W1–W49 create dozens of cross-system connections (§7). This task audits and tests the **strongest
chains** end-to-end so they actually work as designed, not just as isolated plans. This is the
verification capstone for the content roadmap.

### Existing systems reused
All W1–W49 systems + `ashfall-dialog-graph-lint`, `ashfall-narrative-continuity`,
`ashfall-balance-sim`, `ashfall-determinism-guard`, `ashfall-seed-replay`.

### World-content addition
* 12 chain-integration tests (one per representative mega-chain in §7)
* 1 end-to-end seeded playthrough harness exercising C1–C12
* Chain documentation in `flag_registry.json` (W42)

### Gameplay loop
`Run chain test → each link fires in order → flags propagate →
final consequence verifies → determinism hash stable across runs`

### Content specification
* 12 xUnit chain tests in `Ashfall.Core.Tests/`
* 1 seeded playthrough script exercising C1–C12
* Chain-link documentation

### Integration points
All W1–W49, `Ashfall.Core.Tests`, `ashfall-seed-replay`, `ashfall-balance-sim`,
`ashfall-dialog-graph-lint`, `ashfall-narrative-continuity`, W42 flag registry.

### Substeps
1. Document each mega-chain (§7) as a flag-sequence in `flag_registry.json` (W42).
2. Write 12 chain-integration tests; each verifies a chain fires end-to-end.
3. Write 1 seeded playthrough harness; exercise C1–C12 in order.
4. Verify determinism hash stable across 2 runs (`ashfall-seed-replay`).
5. Run `ashfall-balance-sim` over chain-coupled variables.
6. Run `ashfall-dialog-graph-lint` over chain quest reachability.
7. Run `ashfall-narrative-continuity` over chain narrative.
8. Fix any broken links found (report, don't auto-fix content).
9. Run full verification checklist.
10. Document chain health in the plan.

### Acceptance Criteria
12 chain tests green; seeded playthrough completes; determinism hash stable; balance-sim
green; all lints pass.

### QA / Validation
Full verification checklist + `ashfall-seed-replay` + `ashfall-balance-sim` +
`ashfall-dialog-graph-lint` + `ashfall-narrative-continuity`.

### Follow-on opportunities
1. Each chain in §7. 2. W42 flag registry. 3. Existing 15A epilogue reads chain state. 4. NG+
(existing 15C) inherits chains. 5. Future expansion chains build on this harness.

---

## 7. Cross-system mega chains

These are the highest-value content structures. Each connects 4+ systems. Task W50 audits them.

| # | Chain | Systems spanned | Tasks |
|---|---|---|---|
| C1 | Dead-hand → telemetry → cipher → bunker | OrbitalHarrow (W17) → SignalIntel → Excavation (W15/W9) → Verdict (W27) | W6, W9, W17, W27 |
| C2 | Hospital rescue → medical supply → caravan → shelter | W2 → pharma_recipes → EconomySystem → caravans (existing 16B) | W2, W36, existing 16B |
| C3 | Metro restore → fast-travel → faction patrol → territory | W3 → ExpeditionVehicle → W43 territory → W29 map | W3, W29, W43 |
| C4 | Civic records → courthouse → Verdict dossier → Muster witness | W4 → verdict_questlines → W40 muster → existing 15A | W4, W27, W40, existing 15A |
| C5 | Industrial power → substation → rail recovery → caravan economy | W5 → W27 rail → existing 16B caravans → EconomySystem | W5, W23, W27, existing 16B |
| C6 | Signal → research manual → research node → location unlock | W7 → library_manuals → ResearchSystem (W13) → W9 subterranean | W7, W9, W13 |
| C7 | Radio schedule → war broadcast → faction war → epilogue | existing 24A → existing 06C → existing 25C → existing 15A | existing 24A/06C/25C/15A |
| C8 | Rural famine → seed vault → greenhouse → food economy | W8 → W7/W9 seed vault → W33 greenhouse → NeedsSystem | W8, W9, W24, W33 |
| C9 | Observatory → excavation → sealed archive → Verdict | W7 → W15/W9 → archive item → W27 Verdict | W7, W9, W15, W27 |
| C10 | Weather → wildlife migration → trapping → food economy | W10/W48 → WildlifeMigration (existing 28) → W14 trapping → EconomySystem | W10, W14, W48 |
| C11 | Micro-location rumor → major-quest hook → location → faction | W11 → quest hook → W1–W10 → W43 | W11, W1–W10, W43 |
| C12 | Skill → encounter → apprenticeship → mentorship | W12 → W1–W10 encounters → ApprenticeshipSystem → GenerationalLineage | W12, existing 12A/26B |
| C13 | Orbital strike → sky-armor → foundry repair → economy | W17 → W16 armor → W34 foundry → EconomySystem | W16, W17, W34 |
| C14 | Debt default → bounty raid → reputation → territory | W18 → existing 14 raids → W43 reputation → W29 map | W18, existing 14, W43, W29 |
| C15 | Excavation → room → schedule → morale | W15 → W19 room → W30 schedule → NeedsSystem | W15, W19, W30 |
| C16 | Scavenging table → economy → trade tell → caravan | W20 → EconomySystem → W37 tells → existing 16B | W20, W37, existing 16B |
| C17 | Collectible → codex unlock → location hint → research | W21 → JournalSystem → W7 coordinate → W13 research | W21, W7, W13 |
| C18 | Evacuation route → refugee column → settlement → faction war | W22 → CohortSystem → W45 settlement → existing 06C | W22, W45, existing 06C |
| C19 | Smuggling → checkpoint → faction reputation → debt | W25 → W43 checkpoint → reputation → W18 debt | W25, W43, W18 |
| C20 | River → disease outbreak → quarantine → industry source | W26 → W36 outbreak → W30 quarantine → W5 industry | W26, W36, W30, W5 |
| C21 | Map → faction territory → weather gate → caravan | W29 → W43 territory → W48 weather gate → existing 16B | W29, W43, W48, existing 16B |
| C22 | Collectible → phantom trigger → vinyl suppression → memorial | W21 → W31 phantom → W38 vinyl → MemorialSystem | W21, W31, W38 |
| C23 | Secret → Verdict dossier → Muster witness → epilogue | W32 → W27 Verdict → W40 muster → existing 15A | W32, W27, W40, existing 15A |
| C24 | Chemical dependency → detox → hospital clinic → morale | W35 → MedicalSystem → W2 hospital → NeedsSystem | W35, W2 |
| C25 | Dive wreck → relic → workshop restoration → Verdict dossier | W39 → W49 relic → Workshop → existing 15B | W39, W49, existing 15B |
| C26 | Campaign band → phase event → territory shift → epilogue | W41 → W43 territory → existing 15A epilogue | W41, W43, existing 15A |
| C27 | Faction territory → settlement allegiance → trade → war | W43 → W45 settlement → EconomySystem → existing 06C | W43, W45, existing 06C |
| C28 | NPC arc → faction affiliation → faction war → epilogue | W44 → W43 faction → existing 06C → existing 15A | W44, W43, existing 06C/15A |
| C29 | Belief movement → schism → pilgrimage → faction war | W46 → W8 monastery → ExpeditionSystem → existing 06C | W46, W8, existing 06C |
| C30 | Bunker open → faction competition → settlement → territory | W9 → W47 competition → W45 settlement → W43 | W9, W47, W45, W43 |

---

## 8. New world-content totals

Completing W1–W50 plus the preserved existing plans adds approximately:

* **+42 major locations** (W1–W10: ~47 sites; W22 camps: 3; W25: 4; W26: 4; W27: 5; W28: 1 complex; W45: 12 settlements — net new major locations after dedup with existing ~284)
* **+25 micro-locations** (W11)
* **+60 scavenging tables** (W20)
* **+95 encounters/events** (W1–W10: ~50; W22–W36: ~45)
* **+18 questlines** (multi-stage: W1, W2, W3, W5, W6, W7, W8, W9, W22, W23, W24, W25, W26, W27, W28, W36, W41, W43, W45, W46, W47)
* **+40 short quests** (W1–W10, W11, W14, W20, W32, W40)
* **+12 recurring NPCs** with 3-state arcs (W44) + ~10 new named NPCs across W2/W6/W22/W27/W28
* **+200 environmental-storytelling entries** (W1–W10, W11, W22, W28 documents)
* **+75 collectibles** (W21)
* **+30 relics** (W49) + component catalog
* **+40 radio transmissions** (W17 warnings, W46 movements, existing 24 expansion)
* **+30 faction situations** (W43: 12 events; W45: 8; W46: 8; W47: 6)
* **+8 new data catalogs** for underused systems (W12–W19)
* **+60 map nodes / ~80 routes** (W29)
* **+12 settlements** (W45)
* **+3 belief movements** (W46)
* **+30 phase-gated campaign events** (W41)
* **+80 world-state flags** (W42)

---

## 9. NOW / NEXT / LATER

### NOW (next 5–10 tasks — low risk, high value, unblock others)
1. **W42** Flag registry network (unblocks all consequence chains)
2. **W12** Skill externalization (unblocks all skill-check encounters in W1–W10)
3. **W20** Scavenging tables (unblocks location loot identity for W1–W10)
4. **W11** Micro-location pack (cheapest world-texture win, validates `loc_micro_` prefix)
5. **W29** Wasteland map expansion (spatial substrate for everything)
6. **W15** Excavation sites catalog (unblocks W9, W3)
7. **W14** Wildlife trapping catalog (cheapest underused-system fill)
8. **W1** Urban residential belt (highest player-value location family)

### NEXT (cross-system + moderate integration)
9. **W2** Hospital complex → 10. **W9** Subterranean → 11. **W43** Faction territorialization →
12. **W41** Campaign bands → 13. **W44** Recurring NPCs → 14. **W27** Rail recovery (flagship chain C5) →
15. **W22** Refugee columns → 16. **W36** Disease outbreaks → 17. **W47** Bunker competition →
18. **W48** Weather gating → 19. **W13** Research tree → 20. **W21** Collectibles

### LATER (large world-state, high-risk, structural)
21. **W45** Settlement allegiance → 22. **W46** Belief movements → 23. **W23** Fuel crisis →
24. **W24** Famine crisis → 25. **W25** Smuggling → 26. **W26** River systems →
27. **W28** Museum → 28. **W49** Relic multi-stage → 29. **W50** Chain integration audit →
30. **W35** Chemical dependency depth → 31. **W38** Vinyl expansion → 32. **W39** Dive expansion →
33. **W40** Muster witnesses → 34. **W34** Foundry expansion → 35. **W33** Greenhouse depth →
36. **W37** Trade tell expansion → 37. **W30** Shelter schedules → 38. **W31/W32** Phantom/secrets

---

## 10. Top 20 highest-value tasks

| Rank | Task | Why (player-value-to-cost ratio) |
|---|---|---|
| 1 | W42 Flag registry | Unlocks every consequence chain; low cost; prevents the #1 silent-failure mode |
| 2 | W20 Scavenging tables | 60 tables fix the "exploration feels samey" gap with pure data; no new system |
| 3 | W29 Map expansion | 6→60 nodes gives every other plan a spatial substrate; high leverage |
| 4 | W11 Micro-locations | 25 discoveries make travel lived-in for the cost of lightweight records |
| 5 | W12 Skill externalization | Fixes an invariant violation + unblocks all skill-check encounters |
| 6 | W1 Urban residential | Most common pre-war structure; highest "world feels empty" fix |
| 7 | W14 Trapping catalog | Cheapest underused-system fill; turns a passive drip into a decision loop |
| 8 | W43 Faction territorialization | Makes 19 factions visibly affect the map; core "world exists independently" |
| 9 | W41 Campaign bands | 30 phase-gated events make the world evolve with zero new engine |
| 10 | W9 Subterranean | Turns ExcavationSystem into a real expedition tier; high discovery value |
| 11 | W2 Hospital complex | Medical content hub; anchors disease/dependency/dose systems |
| 12 | W27 Rail recovery | Flagship cross-system chain (C5); long-term infrastructure payoff |
| 13 | W44 Recurring NPCs | Temporal continuity is the "world remembers" requirement; 12 arcs |
| 14 | W22 Refugee columns | Population movement is the most visible "world beyond the player" |
| 15 | W48 Weather gating | 22 weather states finally affect expeditions; pure data wiring |
| 16 | W15 Excavation sites | 10 predefined sites unlock W9/W3; loader-only system change |
| 17 | W13 Research tree | Fixes invariant violation; 40 nodes give mid/late progression spine |
| 18 | W21 Collectibles | 75 low-cost items add culture + morale + knowledge + hints |
| 19 | W47 Bunker competition | Makes bunker discovery a world-state crisis, not a loot pinata |
| 20 | W36 Disease outbreaks | Makes disease a world-state; connects rivers/hospital/quarantine |

---

## 11. Recommended next agent prompts

These are execution-ready prompts for a coding/content agent, specific to this audit's findings.

1. **Author the urban residential belt (W1):** Create 5 apartment-block locations in `locations.json` with `zones[]`, `loot_identity`, `hazard`, `resident_evidence`; add 8 entries to `scavenging_tables.json`; author 6 environmental documents in `narrative/`; add 4 encounters to `events.json`; add the 5-stage `quest_sealed_apartment_krasny` to `moral_choice_quests.json`; gate with `ashfall-dialog-graph-lint` and `--data-integrity-selftest`.

2. **Externalize skills to JSON (W12):** Read `SkillDef.cs` `RegisterDefaults()`, extract all 47 skills to `skills.json` with `schema_version` and `skill_` ids, add a `LoadFromDirectory` fallback path, author 15 new action skills with real XP thresholds, add descriptions to 38 milestone skills, verify `ApprenticeshipSystem` references resolve, run skill round-trip + determinism tests.

3. **Build the scavenging table system (W20):** Create `scavenging_tables.json` with ~60 tables keyed by `loc_` id, add a lookup path in `EconomySystem` with generic fallback, wire hazard modifiers to `RadiationSystem`/`disease_catalog`, add depletion tracking, verify all `item_id` refs resolve, run `ashfall-balance-sim` (loot economy) and `ashfall-determinism-guard`.

4. **Expand the wasteland map to 60 nodes (W29):** Read `wasteland_map_v1.json`, design an 8-sector graph, author 60 nodes placing W1–W10 locations + key existing loc_ ids, author ~80 routes with terrain/hazard/faction_control, wire sector weather modifiers to `WeatherSystem`, validate `sector_` prefix in `CatalogIntegrityValidator`, run a graph connectivity test + `ashfall-balance-sim`.

5. **Author the roadside micro-location pack (W11):** Validate `loc_micro_` prefix against `CatalogIntegrityValidator` (add if needed), define a lightweight micro-location schema, author 25 micro-locations across all pillars, add 25 one-time loot entries, author 15 short noncombat encounters, wire triggers into `ExpeditionSystem` route events, add per-micro depleted flags, run a depletion test.

6. **Produce the faction territorialization layer (W43):** Create `faction_territory.json` mapping 19 factions to W29 sectors, create `faction_patrols.json` with route-based patrols, author 12 faction-situation events, wire `faction_control` on routes to patrol spawn, wire regional price changes to `EconomySystem`, author the 5-stage border-war quest, run `ashfall-balance-sim` (territory economy).

7. **Generate location-specific loot for the hospital (W2):** Author `loc_regional_hospital_complex` with 5 zones and distinct `loot_identity` per zone, add 12 medical scavenging entries to `scavenging_tables.json`, wire 3 infection encounters to `disease_catalog` exposure rolls, author the 4-stage rescue quest with a shelter-resource-cost branch, add `npc_last_nurse_ianov`, run `ashfall-balance-sim` (medical-supply cost).

8. **Audit and register the flag network (W42):** Grep all `flag_*` references across W1–W49 and existing data, create `flag_registry.json` documenting setter/consumer/consequence, add a `CatalogIntegrityValidator` tier for flag cross-references, audit `InMemoryFlagLedger` for case-normalization drift (Invariant 4), normalize to `StringComparer.Ordinal`, add a flag-network reachability test.

9. **Author the campaign-band events (W41):** Read `events.json` schema, author 30 phase-gated events (10 early Day 1–30, 10 mid Day 31–120, 10 late Day 121+) with `minDay`/`maxDay`, author 3 phase-transition crises, wire late-band consequences to existing 15A epilogue axes, verify `minDay` < `maxDay` (RANGES tier), run `ashfall-balance-sim` + `ashfall-determinism-guard`.

10. **Build the recurring NPC arcs (W44):** Read `characters.json` schema, author 12 recurring NPCs with 3 state-variants each (early/mid/late), author 24 state-transition events gated by `minDay`/`maxDay` + flag prerequisites, wire NPC state to faction affiliation changes (W43), author 6 personal-arc quests, run `ashfall-narrative-continuity` + `ashfall-dialog-graph-lint`.

11. **Implement the rail recovery questline (W27):** Author 5 rail locations, author the 7-stage `quest_restore_northern_rail_line` (stages require fuel W23, power W5, crew, tunnel clear W15), wire line restoration to `ExpeditionVehicleSystem` caravan speed, add `npc_rail_guild_engineer_kell`, author 8 encounters, wire `flag_rail_control_contested` to W43, run `ashfall-balance-sim` + `ashfall-dialog-graph-lint`.

12. **Author the refugee column crisis (W22):** Define 6 evacuation corridors as chains of W11 micro-locations, author 3 temporary-camp locations, author 4 moving-column encounters, wire refugee children to `CohortSystem`, author the 5-stage stranded-column quest, add 2 recurring NPCs, wire faction response, run `ashfall-narrative-continuity` + `ashfall-balance-sim` (aid cost).

13. **Create the weather-route gating system (W48):** Read `WeatherSystem` state keys, create `weather_route_gates.json` with 22 weather→route-effect mappings by terrain, wire gates to `ExpeditionSystem` route availability, author 8 weather-gated encounters, wire EMP to `ExpeditionVehicleSystem` failure, wire seasonal closures to W41 bands, run `ashfall-balance-sim` + `ashfall-determinism-guard`.

14. **Externalize the research tech tree (W13):** Read `ResearchSystem.RegisterDefaults()`, extract 15 nodes to `research_catalog.json` with `knowledge_` ids, add a `LoadFromDirectory` fallback, author 25 tier-3+ nodes with cross-discipline prerequisites, wire `library_manuals.json` (fixed in W7) as `manual_source`, wire `unlock[]` to `foundry_production.json` + `skills.json`, run research round-trip + determinism tests.

15. **Author the collectibles catalog (W21):** Create `collectibles.json` with 75 items across 6 categories (photographs, posters, books, magazines, badges, letters), add to `items.json` with `item_` prefix, wire morale bonus to `NeedsSystem`, wire book unlocks to `ResearchSystem` (W13), wire codex unlocks to `JournalSystem`, wire location hints to W7 coordinates, add to W20 scavenging tables (rare slots), run `ashfall-balance-sim` (morale economy).

16. **Build the excavation sites catalog (W15):** Read `ExcavationSystem.AddSite` API, create `excavation_sites.json` with 10 predefined sites (5 from W9, 3 from W3, 2 standalone), add catalog load to `ExcavationHostSession`, wire cave-in probability to `structural_risk` + shoring via `ISeededRng`, wire deep-mold hazard to `disease_catalog`, wire completion to `bunker_blueprints_codex` room unlock, run excavation round-trip + determinism.

17. **Author the disease outbreak events (W36):** Add 4 new diseases to `disease_catalog.json`, author 8 outbreak events with triggers (weather/water/exposure), wire spread to `MedicalSystem` via `CohortSystem` proximity, wire quarantine to `shelter_schedules.json` (W30), author the 5-stage hospital quarantine quest, wire `flag_route_contaminated` to W29 map hazard, run `ashfall-balance-sim` (disease spread) + `ashfall-determinism-guard`.

18. **Run the cross-system chain integration audit (W50):** Document each mega-chain (§7 C1–C30) as a flag-sequence in `flag_registry.json`, write 12 chain-integration tests, write 1 seeded playthrough harness exercising C1–C12, verify determinism hash stable across 2 runs (`ashfall-seed-replay`), run `ashfall-balance-sim` + `ashfall-dialog-graph-lint` + `ashfall-narrative-continuity`, report broken links.

19. **Audit radio content for the unified schedule (existing 24A + W17):** Inventory `radio.json` (50) + `faction_radio_corpus.json` (13) + distress signals, build a unified broadcast schedule, add 6 orbital-harrow warnings (W17), add war broadcasts (existing 06C), wire schedule to `WeatherSystem`/W41 bands, run `ashfall-narrative-continuity`.

20. **Build the environmental-storytelling bundle for one location family:** Pick W5 (industrial), author 8 documents (safety logs, shift records, accident reports, union notices) in `narrative/` reusing the existing industrial-log format, wire document discovery to W20 scavenging tables (Document slots), wire document reads to `JournalSystem` codex unlocks, run `ashfall-narrative-continuity`.

21. **Produce the bunker competition crisis (W47):** Wire bunker-open flags (W9) to trigger competition events, author 6 competition events (claim, siege, negotiation, auction, sabotage, treaty), author the 5-stage `quest_bunker_kappa_dispute` with 3 outcomes, wire siege to `TacticalCombatSystem`, wire auction to `LedgerDebtSystem` (W18), wire outcomes to W45/W43, run `ashfall-balance-sim` + `ashfall-dialog-graph-lint`.

22. **Author the fuel shortage crisis chain (W23):** Add 3 fuel items, author 6 fuel-shortage events, wire price spikes to `EconomySystem` dynamic pricing, wire convoy raids to `ExpeditionVehicleSystem` travel risk, author the 5-stage refinery dispute quest (W5 foundry vs hydro_barons), wire rationing to shelter fuel consumption, wire black-market fuel to `LedgerDebtSystem` (W18), run `ashfall-balance-sim` (fuel economy).

23. **Run a continuity review over the full content graph:** Execute `ashfall-narrative-continuity` + `ashfall-dialog-graph-lint` across all W1–W50 data once authored, report orphan quests, dead endings, missing flag producers/consumers, and `minDay`/`maxDay` window violations, produce a fix-list.

24. **Generate the settlement catalog (W45):** Validate `settlement_` prefix, create `settlements.json` with 12 settlements on W29 nodes, wire allegiance to `faction_lore.json` (W43), wire trade to `EconomySystem` + `TradeTellEngine` (W37), author 8 settlement-situation events gated by W41 bands, author the 5-stage alliance quest, run `ashfall-balance-sim` (settlement economy).

25. **Author the subterranean infrastructure pillar (W9):** Author 5 subterranean locations, register each in `excavation_sites.json` (W15) with depth/structural-risk, wire cave-in to `ExcavationSystem` shoring, wire gas-pocket to `disease_catalog` (add deep mold), author the 5-stage sealed-bunker quest, add 2 unique items, wire `flag_bunker_kappa_opened` to W47, run excavation round-trip + `ashfall-determinism-guard`.

---

## Final self-review (15-point checklist)

1. **≥60 substantial tasks?** YES — 50 new (W1–W50) + ~30 preserved existing = 80 active plans.
2. **Majority world/content work?** YES — ~75% content/data/narrative; only W12–W19/W50 involve minimal loader additions.
3. **Preserved good existing work?** YES — §2 preservation table; ~30 existing plans kept, ~10 deepened.
4. **Avoided duplicating systems?** YES — every plan reuses existing systems; 8 loader-only changes flagged `NEW SYSTEM JUSTIFICATION REQUIRED`.
5. **Locations significantly expanded?** YES — +42 major locations across 7 families + 25 micro-locations.
6. **Quests/encounters expanded?** YES — +18 questlines, +40 short quests, +95 encounters.
7. **Factions physically represented?** YES — W43 territorialization + W45 settlements + W46 movements.
8. **Survivors/NPCs recurring roles?** YES — W44 12 recurring NPCs with 3-state arcs.
9. **Existing systems reused aggressively?** YES — 8 underused systems filled (W12–W19); all plans cite real systems.
10. **Cross-system connections explicit?** YES — §7 lists 30 mega-chains; W50 audits them.
11. **Grounded tone?** YES — no magic/supernatural/zombies; industrial/military/scientific grounded.
12. **Implementation-ready?** YES — each plan has 6–10 concrete substeps + acceptance criteria.
13. **Substeps concrete?** YES — e.g. "Read `SkillDef.cs` `RegisterDefaults()`; extract all 47 skills to JSON."
14. **Acceptance criteria measurable?** YES — e.g. "60 tables load; loot is location-appropriate; integrity 0 errors."
15. **Genuinely new opportunities?** YES — micro-locations, scavenging tables, faction territorialization, campaign bands, weather gating, bunker competition, river systems, smuggling, famine/fuel crises are all new vs. existing 06–30.

All 15 checks pass.
