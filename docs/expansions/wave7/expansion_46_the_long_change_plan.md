# ASHFALL — Expansion 46 Design Bible
# THE LONG CHANGE
### Wave 7 · Landmarks, Location States, Observation, Reclamation, and the Record of Years

**Document status:** Design plan (pre-integration). Not a claim. Not an authorization.
**Date:** 2026-09-24
**Domain owners touched:** `Ashfall.Core` (LocationEvolutionSystem, LandmarkDegradationSystem, EvolvingWorldCatalog, ExpansionEnrichmentCatalog), `Ashfall.Core.World/Expeditions` seams
**Proposed host owner:** `WorldChangeHostSession` (extends observation, landmark, and location-state surfaces)
**Existing save sections:** `LocationEvolutionSaveState`, `LandmarkSaveState`, world seed container (read at init)
**Existing CLI verbs:** `--world-evolution-selftest` (if present), `--data-integrity-selftest`, `--content-utilization-selftest`
**Rule compliance:** Godot authoritative; Core engine-free; JSON data authoritative; one authority per concern; deterministic seeded RNG.

---

## 0. HOW TO READ THIS DOCUMENT

This is a **design bible**, not an integration plan. Implementation must later pass
through `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `TEST_POLICY.md`.

ASHFALL already ages the world. `LocationEvolutionSystem` defines
`LocationEvolutionSaveState` (`schema_version`, `systemId`, `lastEvolutionDay`,
`mutations`) and `LocationMutationRecord` (`locationId`, `currentOwner` default
"none", `contaminationLevel`, `lootDepletionFactor`, `isCleared`, `isRuined`).
`LandmarkDegradationSystem` defines `LandmarkSaveState` (`schema_version`,
`systemId`, `lastDegradationDay`, `landmarks`) and `LandmarkStatusRecord`
(`landmarkId`, `locationId`, `structuralIntegrity` = 100f, `ashBurialCm`,
`isCollapsed`, `isScavenged`, `collapseDay`). `EvolvingWorldCatalog` defines
`EvolvingWorldSeedContainer` (`schema_version`, `collection_id`, `description`,
`shelter_sector_id`, `scarcity_goods`, `sectors`, `packs`, `landmarks`,
`location_seeds`) with `SectorSeedRecord`, `PackSeedRecord`,
`LandmarkSeedRecord`, and `LocationSeedRecord`. The seed file
`world_evolution_seeds.json` (20,728 B) describes a 24-sector migration graph,
24 wildlife packs across 12 species, 30 landmark integrity baselines, 40
location ownership and contamination seeds, and 4 scarcity goods.
`damaged_map_zones.json` (15,568 B) describes damaged regions.

What does not exist: observation posts, survey rounds, landmark care decisions,
reclamation work, scarcity watches, threshold alerts, site histories, and the
human experience of watching a world change across years. The world ages
silently and the shelter never notices until something is gone.

**The Long Change** makes the world's aging observable and actionable: what is
being lost, what can be saved, what should be salvaged, what is coming back,
and what the shelter decides to let go. It extends the live evolution and
degradation owners and hands every material consequence to the systems that own
building, cleaning, and travel.

Conventions: **`LIVE`** confirmed in source/data; **`GAP`** confirmed thin;
**`PROPOSED`** new.

### 0.1 Boundary contracts

| Neighbour | Owns | This plan does |
|---|---|---|
| 163 Cartography (partial) | Map-making and survey plotting | Never owns maps; reads and annotates them |
| 32 The Wild (Wave 5) | Wildlife state and migration | Observes it; never mutates it |
| 33 The Weather (Wave 5) | Weather and seasons | Correlates; never generates weather |
| 34 The Long Road (Wave 5) | Route condition | Observes route decay; reports to its owner |
| 18 The Underneath (Wave 2) | Subterranean systems | Observes surface consequence only |
| 22 The Clean Flow (Wave 3) | Contamination and sanitation | Requests cleanup; never owns contamination |
| 31 The Kiln (Wave 4) | Construction and materials | Orders restoration materials |
| 43 The Question (Wave 7) | Field studies and holdings | Hosts long-term observation data |
| 44 The Outpost (Wave 7) | Remote settlements | Reports change to them |
| 03 The Standing Record | Records | Files the world chronicle |
| 24 The Long Goodbye | Memory and legacy | Uses its memory forms for lost places |

---

## 1. EXECUTIVE SUMMARY

### 1.1 Pitch

The old tower has a crack that was not there last year, the river has moved
forty meters, the ash is burying a road the shelter still needs, and the deer
are coming back to a valley they had abandoned.

**The Long Change** is the expansion about living in a world that is still
getting older: observing, measuring, recording, and deciding what to save, what
to salvage, what to reclaim, and what to let go. It gives the shelter survey
posts, a landmark register, threshold alerts, reclamation plans, and a
year-by-year chronicle of the valley it lives in.

### 1.2 The five loops it adds

```
  Observe ──► Measure ──► Decide ──► Act ──► Record
     │          │           │         │         │
     ▼          ▼           ▼         ▼         ▼
   Rounds,   Trends,     Save,     Stabilize, Year notes,
   posts     thresholds  salvage   reclaim    histories
                                        │
                                        ▼
                              Learn ──► Predict ──► Prepare
```

### 1.3 What the player manages

1. **Observation.** Rounds, posts, notes, and instruments.
2. **Measurement.** Integrity, burial, contamination, depletion, counts.
3. **Thresholds.** What must be reported before it becomes urgent.
4. **Landmarks.** Stabilize, restore, scavenge, or witness a collapse.
5. **Locations.** Ownership, contamination, clearing, ruin, and reoccupation.
6. **Scarcity.** What is getting harder to find and what substitutes.
7. **Reclamation.** Cleaning, rebuilding, and returning to places.
8. **Chronicle.** Year notes, before-and-after records, and memory.
9. **Prediction.** Trends used for planning rather than fortune-telling.
10. **Acceptance.** Letting some things go, deliberately and with record.

### 1.4 What it is not

- Not a second map or survey system. Cartography stays with its owner.
- Not a second wildlife, weather, contamination, or route system. It observes
  and reports.
- Not a doom clock. Loss is gradual, partial, and sometimes reversible.
- Not a hoarding incentive; salvage is a decision with consequences.
- Not a nostalgia trap; the shelter may choose to let a ruin rest.
- Not a new save section.

---

## 2. EVIDENCE BASE AND GAP ANALYSIS

### 2.1 Live systems (verified)

| File | Role | Status |
|---|---|---|
| `Assets/Ashfall.Core/LocationEvolutionSystem.cs` | Location ownership, contamination, ruin | `LIVE` |
| `Assets/Ashfall.Core/LandmarkDegradationSystem.cs` | Integrity, burial, collapse, scavenging | `LIVE` |
| `Assets/Ashfall.Core/EvolvingWorldCatalog.cs` | Sectors, packs, landmarks, seeds, scarcity | `LIVE` |
| `Assets/Ashfall.Core/WildlifeMigrationSystem.Live.cs` | Migration ledger | `LIVE` |
| `Assets/Ashfall.Core/WeatherSystem.cs` | Weather and seasons (Wave 5) | `LIVE` |
| `Assets/Ashfall.Core/World/RouteInfrastructureSystem.cs` | Route decay (Wave 5) | `LIVE` |
| `Assets/Ashfall.Core/ExcavationSystem.cs` | Sites and hazards | `LIVE` |
| `Assets/Ashfall.Core/StandingRecord` | Records | `LIVE` |

### 2.2 Live data (counted)

| Catalog | Size | Notes |
|---|---|---|
| `world_evolution_seeds.json` | 20,728 B | 24 sectors, 24 packs, 30 landmarks, 40 locations, 4 scarcities |
| `damaged_map_zones.json` | 15,568 B | damaged regions |
| Observation, register, threshold, reclamation data | absent | confirmed none |
| Site histories and world chronicle | absent | confirmed none |

### 2.3 Confirmed gaps

- **GAP-46-1 — No observation posts or rounds.**
- **GAP-46-2 — No landmark register or care decisions.**
- **GAP-46-3 — No threshold alerts for slow change.**
- **GAP-46-4 — No location state decisions (clear, reclaim, ruin).**
- **GAP-46-5 — No scarcity watch content.**
- **GAP-46-6 — No reclamation plans.**
- **GAP-46-7 — No site histories.**
- **GAP-46-8 — No world chronicle.**
- **GAP-46-9 — No observation staffing or instruments content.**
- **GAP-46-10 — Seeded world change has no player-facing experience.**

### 2.4 Non-duplication statement

This expansion will **not** add a second map, wildlife, weather, contamination,
route, excavation, or record system. It extends `LandmarkDegradationSystem`
with content and care actions, `LocationEvolutionSystem` with state decisions,
`EvolvingWorldCatalog` with observation content, and the record owners with the
world chronicle. It adds state only as additive sub-objects of the existing
evolution and landmark stores. No new save section.

---

## 3. DESIGN PILLARS AND TONE

### 3.1 Pillars

**Pillar 1 — The world is a patient, not a backdrop.** It changes whether or
not anyone watches.

**Pillar 2 — Slow change hides until it is measured.** Thresholds are how a
shelter notices a decade in a month.

**Pillar 3 — Saving costs, and so does losing.** Every landmark decision trades
labor against memory and use.

**Pillar 4 — Some things should be let go.** Deliberate release is a decision,
not neglect.

**Pillar 5 — A chronicle makes a place real.** What the shelter writes down
becomes the valley's memory.

### 3.2 Tone calibration

| Element | Do | Do not |
|---|---|---|
| Erosion | Slow, measurable | Apocalyptic |
| Collapse | Warning, decision, aftermath | Spectacle |
| Salvage | Careful, recorded | Looting glee |
| Reclamation | Work and patience | Instant restoration |
| Wildlife return | Counts and hope | Nature documentary |
| Scarcity | Substitution and planning | Hoarding panic |
| Memory | Names and notes | Melancholy wallowing |
| Letting go | Deliberate and recorded | Erasure |

### 3.3 Content limits

- No real landmarks, monuments, or heritage sites copied.
- No catastrophe spectacle; collapse is warned, partial, and consequential.
- No looting reward loops; salvage decisions carry records and costs.
- No despair framing; the valley is changing, not dying.
- No erasure of the dead; human sites are handled with respect and the memory
  owners.
- No new save section.

---

## 4. THE LONG CHANGE WORLD

### 4.1 Interior rooms

- **`room_survey_office`** — charts, registers, and the round board.
- **`room_sample_room`** — bags, swabs, and the sample press.
- **`room_chronicle_room`** — the yearbook and the before-cards.
- **`room_threshold_board`** — alerts and the watchlist.
- **`room_reclamation_store`** — mortar, brushes, posts, and signs.
- **`room_observation_kit`** — instruments, stakes, and ledgers.
- **`room_site_memory`** — site histories and photographs of places.
- **`room_long_view`** — the window where the valley is watched.

### 4.2 Exterior locations

| ID | Name | Tone | Purpose |
|---|---|---|---|
| `loc_old_tower` | The Old Tower | 4 | Integrity and collapse |
| `loc_river_bend_change` | The River Bend | 3 | Channel change |
| `loc_ash_road` | The Buried Road | 4 | Burial and clearing |
| `loc_return_valley` | The Return Valley | 3 | Wildlife recovery |
| `loc_cracked_dam` | The Cracked Dam | 4 | Structure and risk |
| `loc_silt_harbor` | The Silt Harbor | 3 | Sediment and access |
| `loc_old_orchard_change` | The Old Orchard | 2 | Reclaim or let go |
| `loc_scar_slope` | The Scar Slope | 4 | Erosion and hazard |
| `loc_new_spring` | The New Spring | 2 | Opportunity |
| `loc_year_stone` | The Year Stone` | 2 | Chronicle markers |

All locations require valid item references and scanner registration.

### 4.3 The year round

Rounds every season; a full survey every year; threshold alerts as they trip; a
year note written each winter. The expansion's clock is the year.

---

## 5. MAIN STORYLINE — "WHAT THE ASH COVERS"

### 5.1 Central conflict

The old tower is the valley's best landmark and it is failing. **Dunc** the
surveyor has marks that show the crack growing. **Petra** the ruin warden wants
a decision: stabilize, salvage, or let it fall and record the fall. **Janka**
the restorer wants mortar and time the shelter can barely spare. **Berk** the
geologist wants the river's new channel surveyed before it moves again.
**Rosso** the wildlife monitor wants the return valley counted — the deer are
back and nobody has noticed. **Merle** the observer wants thresholds written so
the shelter stops being surprised by things that took ten years to happen.

Then the dam cracks in the same season the road is buried, and the shelter has
to choose which piece of its world it can afford to save — and write down what
it decided and why, so the next generation inherits reasons instead of ruins.

The expansion's question: **what is worth keeping, and who decides?**

### 5.2 Theme (unspoken)

**A valley becomes home when someone writes down what happened to it.**

### 5.3 Principal NPCs

| ID | Name | Role | Function |
|---|---|---|---|
| `npc_surveyor_dunc` | Dunc | Surveyor | Rounds, marks, trends |
| `npc_ruin_warden_petra` | Petra | Ruin warden | Landmark decisions |
| `npc_restorer_janka` | Janka | Restorer | Stabilize and rebuild |
| `npc_geologist_berk` | Berk | Geologist | Ground and water change |
| `npc_wildlife_monitor_rosso` | Rosso | Wildlife monitor | Recovery counts |
| `npc_observer_merle` | Merle | Observer | Thresholds and watchlist |
| `npc_historian_ava_kell` | Ava Kell | Historian | Chronicle and site histories |
| `npc_apprentice_mapper_bea` | Bea | Apprentice | Rounds and records |

### 5.4 Story beats (15)

1. **The Crack.** The tower's crack is measured and compared.
2. **The Register.** Landmarks enter a registry with names and numbers.
3. **The Rounds.** Seasonal rounds are walked and noted.
4. **The Threshold.** The shelter writes what it wants to be warned about.
5. **The River.** The channel has moved and the old survey is wrong.
6. **The Decision.** The tower gets a plan: save, salvage, or witness.
7. **The Dam.** A second structure fails its inspection.
8. **The Choice.** The shelter can only afford one major restoration.
9. **The Clearing.** The buried road is dug or abandoned, and recorded.
10. **The Return.** Deer counts prove a valley is recovering.
11. **The Scarcity.** A familiar salvage is running out.
12. **The Reclamation.** A ruined site is cleaned and reoccupied, or left.
13. **The Chronicle.** The first year note is written and read aloud.
14. **The Successor.** Bea walks the rounds alone and finds a new crack.
15. **What the Ash Covers.** The valley's future is decided as a practice.

### 5.5 Branching choices (8)

| Choice | Options | Axis |
|---|---|---|
| Priorities | one save / several / none | focus vs. spread |
| Thresholds | strict / moderate / loose | vigilance vs. labor |
| Landmarks | stabilize / salvage / witness | preservation vs. use |
| Locations | reclaim / clear / leave | return vs. respect |
| Scarcity | substitute / ration / search | adaptation |
| Rounds | seasonal / monthly / event | observation depth |
| Chronicle | detailed / brief / oral | memory vs. effort |
| Final | care as institution / practice / memory | identity |

### 5.6 Endings (5 + fade)

1. **The Watched Valley** — observation and thresholds make slow change
   ordinary and survivable.
2. **The Kept Stone** — the shelter saves one landmark properly and lets others
   go with records and respect.
3. **The Long Survey** — a decade of rounds makes the shelter expert in its own
   valley.
4. **The Return** — wildlife, water, and ground recover where the shelter
   helped, and the counts prove it.
5. **The Letting Go** — the tower falls and the shelter watches, records, and
   moves on without pretending it was nothing.
6. **Fade** — a year stone with a new line carved in it and a valley that looks
   almost the same as last year.

---

## 6. QUEST DESIGN

New IDs use prefix `quest_change_`. Schema follows existing quest catalogs.

### 6.1 Main questline (15)

`quest_change_crack`, `quest_change_register`, `quest_change_rounds`,
`quest_change_threshold`, `quest_change_river`, `quest_change_decision`,
`quest_change_dam`, `quest_change_choice`, `quest_change_clearing`,
`quest_change_return`, `quest_change_scarcity`, `quest_change_reclamation`,
`quest_change_chronicle`, `quest_change_successor`,
`quest_change_what_ash_covers`.

### 6.2 Side quests (30)

**Observation (5)**
- `quest_change_post` — set a survey post
- `quest_change_instrument` — maintain instruments
- `quest_change_round_walk` — walk a seasonal round
- `quest_change_notes` — keep field notes
- `quest_change_marks` — set durable marks

**Landmarks (5)**
- `quest_change_tower_survey` — survey the tower
- `quest_change_stabilize` — stabilize a landmark
- `quest_change_salvage_landmark` — salvage with a record
- `quest_change_witness` — witness a collapse
- `quest_change_plaque` — mark a place's story

**Locations (5)**
- `quest_change_clear` — clear a location
- `quest_change_decon_site` — decontaminate a site
- `quest_change_reclaim_site` — reclaim a site
- `quest_change_ruin_record` — record a ruin
- `quest_change_owner_note` — record ownership

**Scarcity (5)**
- `quest_change_watch` — watch a scarcity
- `quest_change_substitute` — find a substitute
- `quest_change_ration` — ration a material
- `quest_change_search` — search farther
- `quest_change_notice` — warn the shelter

**Water and ground (5)**
- `quest_change_channel` — map a channel change
- `quest_change_spring` — test a new spring
- `quest_change_slope` — survey erosion
- `quest_change_silt` — measure siltation
- `quest_change_dam_check` — inspect a structure

**Chronicle (5)**
- `quest_change_year_note` — write a year note
- `quest_change_before` — keep before-cards
- `quest_change_site_history` — write a site history
- `quest_change_read_aloud` — read the year aloud
- `quest_change_archive_change` — file the change record

### 6.3 Repeatable quests (8)

`quest_change_repeat_round`, `quest_change_repeat_sample`,
`quest_change_repeat_notes`, `quest_change_repeat_mark`,
`quest_change_repeat_threshold`, `quest_change_repeat_count`,
`quest_change_repeat_clear`, `quest_change_repeat_review`.

### 6.4 Dynamic hooks

Live events (landmark degradation ticks, location mutations, migration ledgers,
route decay, weather seasons, excavation finds, scarcity changes, outpost
reports) attach authored follow-ups through existing seams. No new event bus.

### 6.5 Constraints

- Location evolution stays with `LocationEvolutionSystem`.
- Landmark degradation stays with `LandmarkDegradationSystem`.
- Wildlife, weather, contamination, routes, and excavation stay with their
  owners.
- Maps stay with the cartography owner; this plan annotates, never plots.
- Human remains and graves route to the memory owners.
- No new save section.

---

## 7. NEW GAMEPLAY SYSTEMS

### 7.1 `WorldObservationSystem` (new, `Ashfall.Core`)

**Owns:** observation posts, seasonal rounds, instruments, field notes, and
durable marks. **Consumes:** `LandmarkDegradationSystem` (read),
`LocationEvolutionSystem` (read), `WeatherSystem`, field study seam (43),
`NeedsSystem` for observer fatigue. **Data:** `observation_posts.json`,
`survey_rounds.json`. **Rules:** observations are records, never mutations; an
unobserved world still changes; rounds have routes, seasons, and upkeep.

### 7.2 `LandmarkCareSystem` (extend `LandmarkDegradationSystem`)

**Owns:** the landmark register, care decisions, stabilization, restoration,
salvage records, and collapse witnesses. **Consumes:** the live landmark state,
`BuildWorksSystem` (Wave 4), `KilnworksHostSession` (Wave 4), `Inventory`,
memories owners. **Data:** `landmark_register.json`. **Rules:** every landmark
has a decision record; stabilization slows integrity loss; salvage reduces
integrity but yields materials with a record; collapse is warned and witnessed.

### 7.3 `LocationStateSystem` (extend `LocationEvolutionSystem`)

**Owns:** ownership notes, contamination handling, clearing, ruin status, and
reoccupation. **Consumes:** the live mutation state, `DecontaminationSystem`
(Wave 2), `SanitationSystem`, `RouteInfrastructureSystem`, `ExcavationSystem`.
**Data:** `location_state.json`. **Rules:** states change only through the live
system; player actions record decisions and route materials; a ruined site can
be reclaimed but never instantly restored.

### 7.4 `ScarcitySystem` (new, thin, `Ashfall.Core`)

**Owns:** scarcity watches, depletion trends, substitutions, and notices.
**Consumes:** `EvolvingWorldCatalog.scarcity_goods`, salvage and inventory
owners, trade access agreements (45) — never prices. **Data:**
`scarcity_watch.json`. **Rules:** scarcity is a trend with a notice and a
substitution plan; the shelter adapts by planning, not by panic.

### 7.5 `ReclamationSystem` (new, `Ashfall.Core`)

**Owns:** reclamation plans, work camps, clean-up phases, and reoccupation.
**Consumes:** `BuildWorksSystem`, `DecontaminationSystem`, `Clean Flow` owners,
`Caretaker` staffing, `OutpostHostSession` (44) for remote work.
**Data:** `reclamation_plans.json`. **Rules:** reclamation is phased, staffed,
and recorded; phases have thresholds and can be paused; a site is reoccupied
only when water, shelter, and safety are proven again.

### 7.6 `ChangeChronicleSystem` (new, thin, `Ashfall.Core`)

**Owns:** year notes, before-cards, site histories, and the read-aloud. Records
through `StandingRecord` and memory owners. **Data:** `world_notes.json`,
`site_histories.json`. **Rules:** the chronicle records what changed, what was
decided, and who decided; it is written to be read aloud; lost places keep
their names.

### 7.7 `ThresholdWatchSystem` (new, thin, `Ashfall.Core`)

**Owns:** thresholds, alerts, watchlists, and reviews. **Consumes:** observation
data, live change systems, `NoticeSystem` (Wave 4), `AlarmSystem` (Wave 3) for
urgent structural alerts. **Data:** `threshold_alerts.json`. **Rules:** a
threshold is a number and a consequence written down before it trips; alerts go
to named people; every trip produces a review and often a change of practice.

### 7.8 Systems explicitly not added

- No second map, wildlife, weather, contamination, route, or excavation system.
- No catastrophe spectacle or doom clock.
- No looting reward loop.
- No erasure of graves or memory sites.
- No new currency.
- No new RNG stream.
- No new save section.

---

## 8. DATA CATALOG SPECIFICATION

All catalogs snake_case, integer `schema_version: 1`, validated and scanner-registered.

### 8.1 `observation_posts.json` (new)

```json
{
  "schema_version": 1,
  "posts": [
    {
      "post_id": "post_old_tower",
      "display_name": "Tower Observation Post",
      "location_id": "loc_old_tower",
      "instruments": ["crack gauge", "level", "ash gauge"],
      "round_route": "round_north",
      "season": "all",
      "staff": 1,
      "tags": ["landmark", "structure"]
    }
  ]
}
```

### 8.2 `survey_rounds.json` (new)

Rounds: route, stops, instruments, season, duration, notes.

### 8.3 `landmark_register.json` (new)

Register: landmark, baseline, decision, plan, cost, record.

### 8.4 `location_state.json` (new)

States: ownership, contamination, cleared, ruined, action, review.

### 8.5 `scarcity_watch.json` (new)

Watches: good, trend, evidence, substitute, notice, review.

### 8.6 `reclamation_plans.json` (new)

Plans: site, phases, thresholds, staff, materials, reoccupation.

### 8.7 `world_notes.json` (new)

Notes: year, changes, decisions, names, read date.

### 8.8 `site_histories.json` (new)

Histories: site, first record, changes by year, current state, keeper.

### 8.9 `threshold_alerts.json` (new)

Alerts: metric, threshold, consequence, owner, trip, review.

### 8.10 `change_claims.json` (new)

Claims: observed change, evidence, confidence, action, archive.

### 8.11 Items

New items appended to `items.json`: `item_crack_gauge`, `item_ash_gauge`,
`item_level_bar`, `item_survey_stakes_change`, `item_note_ledger`,
`item_chart_tube`, `item_sample_bags`, `item_marker_cairn_stone`,
`item_warning_sign`, `item_survey_rope`, `item_sample_press`,
`item_yearbook`, `item_before_cards`, `item_contamination_swabs`,
`item_restoration_mortar`, `item_site_number_plate`.

---

## 9. SAVE, DETERMINISM, AND PERSISTENCE

### 9.1 Ownership

`LocationEvolutionSaveState` and `LandmarkSaveState` remain the live save
owners. New sub-objects (posts, rounds, register, states, watches, plans,
notes, histories, alerts, claims) are additive inside them. No new save
section.

### 9.2 State to persist

- Posts, instruments, and condition.
- Round completion and notes.
- Landmark decisions and works.
- Location state decisions and actions.
- Scarcity watches and notices.
- Reclamation phases and staffing.
- Year notes and site histories.
- Threshold alerts and reviews.
- Observed change claims.

### 9.3 Determinism

- Location mutations and landmark degradation remain in the live systems and
  their day ticks.
- Observation only records what the live systems report; it never rolls its own
  world change.
- Reclamation outcomes derive from phases, materials, labor, and live
  contamination state.
- Wildlife counts come from the migration ledger.
- Paired replay hashes must match; no wall-clock or `System.Random`.

### 9.4 Migration

Legacy saves load with mutations, landmarks, and seeds untouched; no posts,
rounds, alerts, or notes exist until started. Existing collapsed landmarks and
ruined locations appear in the register with recovered baselines where the
data allows, and as unknown where it does not.

### 9.5 Checksum

Invariant-culture floats; integer day, count, and year fields.

---

## 10. UI, ACCESSIBILITY, AND PRESENTATION

### 10.1 Surfaces

| Surface | Purpose | Owner |
|---|---|---|
| `ObservationPanel` (new) | Posts, rounds, notes | `WorldChangeHostSession` |
| `LandmarkPanel` (new) | Register and decisions | same |
| `LocationStatePanel` (new) | Ownership and site state | same |
| `ScarcityPanel` (new) | Watches and substitutes | same |
| `ReclamationPanel` (new) | Phases and staffing | same |
| `ThresholdPanel` (new) | Alerts and reviews | same |
| `ChroniclePanel` (new) | Year notes and histories | same |

### 10.2 Accessibility and honesty

- Panels show live state and expose existing commands only.
- Trends are shown as numbers with years, never as vague dread.
- Thresholds are visible before they trip, with their consequences written out.
- Maps and site plans have text descriptions.
- Keyboard/controller close/back preserved; focus maintained on refresh.
- The chronicle can be read aloud and printed through the press owner.

### 10.3 Presentation

Audio cues appended to `audio_cues.json`: a stone shifting, a pencil on a
gauge card, a river in a new channel, a yearbook opening, a plaque being set, a
count of deer in the distance. No cue is required; text carries meaning.

---

## 11. INTEGRATION SEAMS

| Existing system | Attachment |
|---|---|
| `LocationEvolutionSystem` | Location state and mutations |
| `LandmarkDegradationSystem` | Integrity, burial, collapse |
| `EvolvingWorldCatalog` | Seeds, sectors, packs, scarcity |
| `WildlifeMigrationSystem` | Counts and returns |
| `WeatherSystem` (Wave 5) | Seasonal correlation |
| `RouteInfrastructureSystem` (Wave 5) | Route decay observation |
| `ExcavationSystem` (Wave 2) | Site finds and hazards |
| `DecontaminationSystem` (Wave 2) | Site cleanup |
| `SanitationSystem` (Wave 3) | Waste and water at sites |
| `BuildWorksSystem` (Wave 4) | Stabilization and rebuilding |
| `KilnworksHostSession` (Wave 4) | Mortar and materials |
| `PressHostSession` (Wave 4) | Printed chronicle |
| `NoticeSystem` (Wave 4) | Notices and warnings |
| `AlarmSystem` (Wave 3) | Structural emergency alerts |
| `StandingRecord` (Exp 03) | Filed changes |
| `MemorialSystem` (Wave 3) | Memory for lost places |
| `InquiryHostSession` (Wave 7) | Long observation data |
| `OutpostHostSession` (Wave 7) | Remote site reports |
| `EpilogueChronicleBuilder` | Year notes |

---

## 12. TECHNICAL IMPLEMENTATION PLAN

### 12.1 Phase order

**Phase 0 — Premise re-audit.** Confirm evolution, landmark, seed, migration,
weather, route, excavation, build, record, and memory owners. Record file:line;
change nothing.

**Phase 1 — Data + validators.** Author the ten catalogs; register validators
and scanner.

**Phase 2 — Pure Core.** `WorldObservationSystem`, `LandmarkCareSystem`,
`LocationStateSystem`, `ScarcitySystem`, `ReclamationSystem`,
`ChangeChronicleSystem`, `ThresholdWatchSystem`.

**Phase 3 — Persistence.** Additive sub-objects, migration, round-trip,
determinism.

**Phase 4 — Host + CLI.** `WorldChangeHostSession`, selftest coverage, fresh
journey from the crack to the year stone.

**Phase 5 — UI.** New panels with lifecycle and accessibility.

**Phase 6 — Content.** Rooms, locations, NPCs, quests, items, prose, audio.

**Phase 7 — Balance.** Multi-year soak: tower, river, scarcity, recovery, and
one deliberate letting go.

**Phase 8 — Verification and closeout.**

### 12.2 Content volume

| Content | Count |
|---|---|
| Observation posts | 10 |
| Survey rounds | 8 |
| Landmark register rows | 20 |
| Location states | 20 |
| Scarcity watches | 10 |
| Reclamation plans | 10 |
| Year notes | 10 |
| Site histories | 12 |
| Threshold alerts | 14 |
| Change claims | 16 |
| Items | 16 |
| Locations | 10 |
| Rooms | 8 |
| NPCs | 8 |
| Main quests | 15 |
| Side quests | 30 |
| Repeatable | 8 |
| Endings | 5 + fade |
| Prose estimate | 55,000–70,000 words |

### 12.3 Risks

| Risk | Severity | Mitigation |
|---|---|---|
| Map overlap with 163 | High | Annotate, never plot |
| Doom-clock tone | High | Gradual, partial change |
| Looting loop | High | Recorded salvage costs |
| Mutation bypass | Critical | Live systems only |
| Memory erasure | Critical | Memory owners routed |
| Observation busywork | Medium | Thresholds and meaning |
| Determinism break | Low | Live seeded paths |
| Content overrun | Medium | Budget §13 |

---

## 13. APPENDIX A — AUTHORING BUDGET

| File | Rows | Est. words |
|---|---|---|
| `observation_posts.json` | 10 | 2,500 |
| `survey_rounds.json` | 8 | 2,000 |
| `landmark_register.json` | 20 | 4,500 |
| `location_state.json` | 20 | 4,000 |
| `scarcity_watch.json` | 10 | 2,500 |
| `reclamation_plans.json` | 10 | 3,000 |
| `world_notes.json` | 10 | 2,500 |
| `site_histories.json` | 12 | 3,500 |
| `threshold_alerts.json` | 14 | 3,000 |
| `change_claims.json` | 16 | 3,000 |
| Quest objectives | 53 quests | 15,000 |
| NPC prose | 8 NPCs | 7,000 |
| Location prose | 10 | 3,500 |
| Item descriptions | 16 | 2,500 |
| Ending prose | 6 | 3,000 |
| **Total** | | **~61,500** |

---

## 14. APPENDIX B — RISK REGISTER

| ID | Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|---|
| R46-1 | Map overlap | High | High | Annotate only |
| R46-2 | Doom clock | Med | High | Gradual change |
| R46-3 | Looting reward | Med | High | Salvage records |
| R46-4 | Mutation bypass | Low | Critical | Live owners |
| R46-5 | Memory erasure | Low | Critical | Memory routing |
| R46-6 | Busywork rounds | Med | Medium | Thresholds |
| R46-7 | Melancholy overload | Med | Medium | Hope and recovery |
| R46-8 | Determinism | Low | High | Live paths |
| R46-9 | Content overrun | Med | Med | Budget §13 |
| R46-10 | Cartography friction | Med | Medium | Boundary §0.1 |

---

## 15. APPENDIX C — OPEN DECISIONS REQUIRING A FOREMAN SIGNATURE

1. **Can landmarks be fully restored?** Recommended: no; stabilization and
   partial restoration only, with honest results.
2. **Do collapsed landmarks become permanent memorials?** Recommended: yes,
   recorded by the memory owners with names and dates.
3. **Who decides between saving and salvaging?** Recommended: the steward with
   the warden's assessment and a public reading of the decision.
4. **Can scarcity ever be reversed?** Recommended: yes, through recovery and
   substitution, though slowly.
5. **Do outposts keep their own chronicles?** Recommended: yes, merged with the
   main chronicle each year.

---

## 17. APPENDIX D — OBSERVATION POST TABLE (10 POSTS)

| # | Post | Location | Instruments | Route | Season | Staff |
|---|---|---|---|---|---|---|
| 1 | Tower | old tower | crack gauge, level | north | all | 1 |
| 2 | River bend | river bend | level, flow stick | river | all | 1 |
| 3 | Buried road | ash road | ash gauge, stakes | east | all | 1 |
| 4 | Return valley | return valley | count sheet | south | spring, autumn | 1 |
| 5 | Dam | cracked dam | seep jar, level | west | all | 1 |
| 6 | Harbor | silt harbor | depth pole | river | summer | 1 |
| 7 | Orchard | old orchard | bud count | yard | spring | 1 |
| 8 | Scar slope | scar slope | stake row | ridge | all | 1 |
| 9 | New spring | new spring | flow jar, taste card | west | all | 1 |
| 10 | Year stone | year stone | chisel, ledger | yard | winter | 1 |

Ten posts, each a place where a person stands and writes something down. The
instrument column is deliberately modest: most change is observed with a gauge,
a stick, a jar, and patience rather than with rare equipment.

---

## 18. APPENDIX E — SURVEY ROUND TABLE

| # | Round | Stops | Duration | Season | Output |
|---|---|---|---|---|---|
| 1 | North structure | tower, dam | 2 days | all | integrity |
| 2 | River line | bend, harbor, spring | 3 days | spring | channel |
| 3 | East route | buried road, marks | 2 days | autumn | burial |
| 4 | South life | return valley, orchard | 3 days | spring | counts |
| 5 | Ridge line | scar slope, posts | 2 days | summer | erosion |
| 6 | Full year | all posts | 8 days | autumn | year note |
| 7 | Emergency | one post | 1 day | event | alert |
| 8 | Training round | two posts | 2 days | spring | apprentice |

Eight rounds give the survey a rhythm and the apprentice a curriculum. The
full-year round is the expansion's big annual event, walked in pieces across
a week and ending in the year note.

---

## 19. APPENDIX F — LANDMARK REGISTER TABLE (20 LANDMARKS)

| # | Landmark | Baseline | Decision | Plan | Cost | Record |
|---|---|---|---|---|---|---|
| 1 | Old tower | 88 | stabilize | buttress | stone, labor | yes |
| 2 | Cracked dam | 62 | salvage | remove fittings | parts | yes |
| 3 | Stone bridge | 74 | stabilize | repoint | mortar | yes |
| 4 | Old church wall | 55 | witness | none | none | yes |
| 5 | Water tower | 70 | stabilize | band | steel | yes |
| 6 | Rail viaduct | 66 | salvage | track first | rails | yes |
| 7 | Old mill | 72 | restore | roof, stones | timber | yes |
| 8 | Brick school | 60 | reclaim | roof, floor | brick | yes |
| 9 | Chimney stack | 45 | witness | none | none | yes |
| 10 | Concrete bunker | 80 | restore | door, vents | parts | yes |
| 11 | Iron gate | 40 | salvage | hinges | iron | yes |
| 12 | Monument stone | 90 | leave | none | none | yes |
| 13 | Old hospital | 52 | reclaim | clean, roof | glass, brick | yes |
| 14 | Grain silo | 68 | stabilize | bands | steel | yes |
| 15 | Bridgehouse | 58 | witness | none | none | yes |
| 16 | Chapel ruin | 35 | leave | none | none | yes |
| 17 | Waterwheel house | 77 | restore | wheel, roof | timber | yes |
| 18 | Signal tower | 64 | reclaim | mast | parts | yes |
| 19 | Old warehouse | 71 | salvage | contents first | goods | yes |
| 20 | Kiln chimney | 49 | witness | none | none | yes |

Twenty landmarks with baselines, decisions, and costs. The witness and leave
rows are as important as the restores: a shelter that saves everything saves
nothing well, and a shelter that records what it lets go is doing a different
kind of preservation.

---

## 20. APPENDIX G — LOCATION STATE TABLE (20 LOCATIONS)

| # | Location | Ownership | Contamination | State | Action | Review |
|---|---|---|---|---|---|---|
| 1 | Old tower | none | clean | risky | stabilize | yearly |
| 2 | River bend | none | clean | open | observe | yearly |
| 3 | Buried road | none | trace | blocked | clear | season |
| 4 | Return valley | none | clean | recovered | count | season |
| 5 | Cracked dam | none | clean | risky | salvage | yearly |
| 6 | Silt harbor | shared | clean | silting | dredge | season |
| 7 | Orchard | none | clean | reclaimable | graft | yearly |
| 8 | Scar slope | none | clean | eroding | stake | yearly |
| 9 | New spring | none | clean | opportunity | test | yearly |
| 10 | School | none | clean | ruined | reclaim | yearly |
| 11 | Bunker | none | clean | sound | restore | yearly |
| 12 | Hospital | none | trace | ruined | reclaim | yearly |
| 13 | Warehouse | none | clean | looted | salvage | once |
| 14 | Signal tower | none | clean | sound | reclaim | yearly |
| 15 | Chapel | none | clean | ruin | leave | yearly |
| 16 | Kiln chimney | none | clean | failing | witness | yearly |
| 17 | Mill | shared | clean | restorable | restore | yearly |
| 18 | Rail viaduct | none | clean | failing | salvage | yearly |
| 19 | Water tower | shelter | clean | sound | stabilize | yearly |
| 20 | Year stone | shelter | clean | maintained | carve | yearly |

Twenty location states with owners, contamination, and a recorded action. The
leave and witness rows are deliberate: not every place wants to be saved, and
the shelter's maturity is measured partly by what it decides to let rest.

---

## 21. APPENDIX H — SCARCITY WATCH TABLE

| # | Good | Trend | Evidence | Substitute | Notice |
|---|---|---|---|---|---|
| 1 | Salvage wire | falling | yard counts | pull fencing | posted |
| 2 | Glass sheet | falling | window stock | smaller panes | posted |
| 3 | Dry timber | falling | yard counts | green, kiln | posted |
| 4 | Copper scrap | falling | bearing heap | bronze mix | posted |
| 5 | Rope fiber | steady | store | nettle cord | none |
| 6 | Medicine salts | falling | pharmacy | regrow stock | urgent |
| 7 | Fuel oil | falling | tank levels | synthesis | posted |
| 8 | Seed varieties | falling | seed bank | exchange | posted |
| 9 | Salt | steady | pans | brine | none |
| 10 | Paper | falling | press stock | rag paper | posted |

Ten scarcity watches, each with a trend, evidence, and a substitution plan. The
medicine row is urgent on purpose: some scarcities affect lives directly, and
the system says so plainly rather than treating all shortages as equal.

---

## 22. APPENDIX I — RECLAMATION PLAN TABLE

| # | Plan | Site | Phases | Staff | Materials | Reoccupy when |
|---|---|---|---|---|---|---|
| 1 | School return | brick school | clear, roof, floor | 6 | brick, timber | dry and warm |
| 2 | Hospital clean | old hospital | survey, clean, seal | 8 | swabs, glass | trace cleared |
| 3 | Bunker repair | bunker | door, vents, dry | 4 | steel, felt | sealed |
| 4 | Mill restore | old mill | wheel, roof, clean | 6 | timber, stone | grinds test |
| 5 | Tower buttress | old tower | foot, buttress, cap | 8 | stone, mortar | survey holds |
| 6 | Orchard graft | old orchard | prune, graft, water | 4 | grafts, compost | buds set |
| 7 | Harbor dredge | silt harbor | dredge, wall, mark | 10 | stone, channel | depth reads |
| 8 | Road dig | buried road | probe, clear, pack | 12 | stakes, gravel | passable |
| 9 | Spring capture | new spring | test, box, pipe | 5 | stone, pipe | water clear |
| 10 | Signal raise | signal tower | mast, guy, seat | 5 | timber, rope | signal sent |

Ten reclamation plans show the shelter returning to places rather than only
leaving them. Each has phases, staffing, and a proof condition, so reoccupation
is never a mood but always a demonstrated readiness.

---

## 23. APPENDIX J — YEAR NOTE TABLE (10 YEARS)

| # | Year | Change | Decision | Named | Read |
|---|---|---|---|---|---|
| 1 | Year one | crack found | stabilize tower | no | yes |
| 2 | Year two | river moved | survey bend | no | yes |
| 3 | Year three | road buried | clear east | no | yes |
| 4 | Year four | deer returned | count valley | yes, valley | yes |
| 5 | Year five | dam failed | salvage fittings | yes, dam | yes |
| 6 | Year six | school reclaimed | reopen | yes, school | yes |
| 7 | Year seven | wire scarce | substitute | no | yes |
| 8 | Year eight | hospital clean | prepare | yes, hospital | yes |
| 9 | Year nine | orchard grafted | tend | yes, orchard | yes |
| 10 | Year ten | tower holds | continue survey | yes, tower | yes |

Ten year notes, each one page, each read aloud in the winter. The named column
is how a valley acquires stories: the shelter names what it saved, what it lost,
and what came back, and the names outlive the people who chose them.

---

## 24. APPENDIX K — SITE HISTORY TABLE (12 SITES)

| # | Site | First record | Change | Current state | Keeper |
|---|---|---|---|---|---|
| 1 | Tower | year one survey | crack grew, buttressed | holding | Dunc |
| 2 | Dam | year one survey | failed year five | salvaged | Petra |
| 3 | Road | year two probe | buried, cleared | passable | Janka |
| 4 | Valley | year four count | deer returned | recovering | Rosso |
| 5 | River | year two mark | channel moved | stable | Berk |
| 6 | School | year three find | reclaimed | in use | Ava |
| 7 | Harbor | year two depth | silted, dredged | usable | Janka |
| 8 | Orchard | year three prune | grafted | budding | Merle |
| 9 | Bunker | year five survey | door restored | stored | Petra |
| 10 | Hospital | year seven survey | cleaned | preparing | Ikra tie |
| 11 | Signal tower | year eight reclaim | mast raised | signaling | Miri tie |
| 12 | Year stone | year one carve | new lines | carved | Ava |

Twelve site histories, each with a first record and a keeper. The keeper column
is the expansion's way of saying that memory is a job: every important place
has a person responsible for its history, and that person changes generation by
generation.

---

## 25. APPENDIX L — THRESHOLD ALERT TABLE

| # | Metric | Threshold | Consequence | Owner | Review |
|---|---|---|---|---|---|
| 1 | Tower integrity | below 70 | buttress or witness | Petra | tripped |
| 2 | Dam seep | any flow | inspect, salvage | Petra | tripped |
| 3 | Road burial | over 30 cm | clear or reroute | Dero tie | tripped |
| 4 | Contamination trace | any | survey, clean | Anil tie | tripped |
| 5 | Wire stock | below season | substitute, search | Dero | tripped |
| 6 | Medicine stock | below month | urgent notice | Ikra | tripped |
| 7 | Deer count | above target | note recovery | Rosso | not tripped |
| 8 | River channel | over 20 m | remap, mark | Berk | tripped |
| 9 | Silt depth | over 1 m | dredge | Janka | tripped |
| 10 | Spring flow | below drink | test, box | Merle | not tripped |
| 11 | Ash depth | over 20 cm | gauge, record | Dunc | tripped |
| 12 | Slope stake | over 10 cm | survey, warn | Berk | tripped |
| 13 | Structural noise | new sound | inspect | Petra | tripped |
| 14 | Year note late | winter | write it | Ava | never |

Fourteen thresholds, each written before it trips and each with a consequence
and an owner. The last row is a joke about the shelter's own discipline, and
the year note has never once been late.

---

## 26. APPENDIX M — MAIN QUESTLINE STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_change_crack` | 4 | Crack measured |
| `quest_change_register` | 4 | Register opened |
| `quest_change_rounds` | 3 | Rounds walked |
| `quest_change_threshold` | 4 | Thresholds written |
| `quest_change_river` | 4 | Channel measured |
| `quest_change_decision` | 5 | Tower decided |
| `quest_change_dam` | 5 | Dam inspected |
| `quest_change_choice` | 5 | One save chosen |
| `quest_change_clearing` | 4 | Road cleared or left |
| `quest_change_return` | 4 | Deer counted |
| `quest_change_scarcity` | 4 | Scarcity noticed |
| `quest_change_reclamation` | 5 | Site reclaimed or left |
| `quest_change_chronicle` | 4 | Year note read |
| `quest_change_successor` | 4 | Apprentice alone |
| `quest_change_what_ash_covers` | 3 | Final disposition |

---

## 27. APPENDIX N — SIDE QUEST STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_change_post` | 3 | Post set |
| `quest_change_instrument` | 3 | Instruments kept |
| `quest_change_round_walk` | 3 | Round walked |
| `quest_change_notes` | 3 | Notes kept |
| `quest_change_marks` | 3 | Marks set |
| `quest_change_tower_survey` | 4 | Tower surveyed |
| `quest_change_stabilize` | 5 | Landmark stabilized |
| `quest_change_salvage_landmark` | 4 | Salvage recorded |
| `quest_change_witness` | 3 | Collapse witnessed |
| `quest_change_plaque` | 3 | Place marked |
| `quest_change_clear` | 4 | Location cleared |
| `quest_change_decon_site` | 4 | Site decontaminated |
| `quest_change_reclaim_site` | 4 | Site reclaimed |
| `quest_change_ruin_record` | 3 | Ruin recorded |
| `quest_change_owner_note` | 3 | Ownership noted |
| `quest_change_watch` | 3 | Scarcity watched |
| `quest_change_substitute` | 4 | Substitute found |
| `quest_change_ration` | 3 | Material rationed |
| `quest_change_search` | 3 | Farther search |
| `quest_change_notice` | 3 | Notice posted |
| `quest_change_channel` | 4 | Channel mapped |
| `quest_change_spring` | 3 | Spring tested |
| `quest_change_slope` | 3 | Slope surveyed |
| `quest_change_silt` | 3 | Silt measured |
| `quest_change_dam_check` | 4 | Structure inspected |
| `quest_change_year_note` | 3 | Note written |
| `quest_change_before` | 3 | Before-cards kept |
| `quest_change_site_history` | 4 | History written |
| `quest_change_read_aloud` | 3 | Year read aloud |
| `quest_change_archive_change` | 3 | Record filed |

---

## 28. APPENDIX O — NPC DOSSIERS (BRIEF)

**Dunc** — surveyor. Keeps a hundred stakes and a book of marks, and can tell
you what the tower did in any month of the last five years. Believes a
measurement is a promise to the future.

**Petra** — ruin warden. Decides which structures get help and which get
witnessed, and writes the reasons down either way. Believes letting a thing
fall is a decision, not a failure.

**Janka** — restorer. Mixes mortar, fits roofs, and works with the patience of
someone who knows a repaired building must outlive the repair. Believes
restoration is a form of respect with a schedule.

**Berk** — geologist. Reads ground and water and argues with rivers politely.
Believes landscapes are honest if you measure them long enough.

**Rosso** — wildlife monitor. Counts deer at dawn and shares every count, even
the disappointing ones. Believes recovery is real even when it is slow.

**Merle** — observer. Writes thresholds so the shelter is warned before it is
surprised. Believes the trick is deciding what you want to know.

**Ava Kell** — historian. Writes the year notes and reads them aloud in winter.
Believes a valley becomes home when its years are named.

**Bea** — apprentice. Walks rounds with a notebook and has not yet found
disappointment or a mistake she minds. Believes the marks will still be there.

---

## 29. APPENDIX P — LOCATION DETAIL

- **The Old Tower** — a tall crack and a careful gauge.
- **The River Bend** — water choosing a new channel and taking its time.
- **The Buried Road** — a route the ash keeps trying to forget.
- **The Return Valley** — deer, brush, and a count sheet.
- **The Cracked Dam** — a structure that will not be argued with.
- **The Silt Harbor** — depth poles and the price of a quiet river.
- **The Old Orchard** — grafting knives and a decision to try again.
- **The Scar Slope** — stakes in a hillside that is leaving.
- **The New Spring** — a chance the valley has offered.
- **The Year Stone** — a rock that accumulates the shelter's memory. 

---

## 30. APPENDIX Q — LANDMARK DECISION PROTOCOL

| Step | Action | Owner |
|---|---|---|
| Survey | measure and record | Dunc |
| Assess | judge use, memory, cost | Petra |
| Consult | ask the people who use it | steward |
| Choose | stabilize, salvage, witness, leave | steward |
| Record | write the decision and the reason | Ava |
| Work | do the chosen work | Janka |
| Re-survey | measure the result | Dunc |
| Review | revisit in a year | all |
| Name | name the outcome in the year note | Ava |
| Close | mark the register | Petra |

The decision protocol turns sentiment into a process. Its second-to-last step is
the expansion's heart: whatever the shelter decides, the outcome gets a name
and a paragraph in the yearbook, so a place is never simply lost.

---

## 31. APPENDIX R — WORKED FIVE-YEAR WORLD SCENARIO

**Year one.** Dunc's first round marks the tower crack at 88 integrity and the
river channel at its old line. The register opens with twenty names. Thresholds
are written, including the wet one about the year note.

**Year two.** The river moves twenty-four meters in spring melt; Berk remaps the
bend and the harbor's depth poles. The buried road gains four centimeters of
ash. The shelter notices for the first time that change is continuous.

**Year three.** The east road passes its burial threshold; the shelter clears
half of it and reroutes the rest, and Ava writes the first real decision para-
graph. The decision is less than anyone wanted and exactly what was affordable.

**Year four.** Rosso counts deer in the return valley for the first time in
years and gets seventeen, then thirty-one in autumn. The valley is named in the
year note, and the count becomes an annual line.

**Year five.** The dam seeps in spring and passes its threshold. Petra salvages
the fittings before the season ends; the structure is witnessed in autumn when a
section goes. The salvage pays for the school's roof, and everyone knows the
exchange.

**Year six.** The school is reclaimed: cleared, roofed, floored, and warmed,
then reopened in autumn. The first class writes its own page inside the year
note, and the shelter has a building that came back.

**Year seven.** Salvage wire runs scarce; Dero posts a notice and the yard pulls
fencing from the far field. The substitution works because the notice went out
before the shortage did.

**Year eight.** The old hospital is surveyed and cleaned in phases; traces are
cleared and rechecked twice. The shelter prepares a ward for a building it will
not occupy for two more years, which is how patience looks from inside.

**Year nine.** The old orchard is pruned and grafted with stock from the seed
bank; the first buds set. Merle keeps the count, and the year note includes a
photograph of a branch.

**Year ten.** The tower holds at 79 after the buttress and the yearly surveys;
the register records a decade of decisions, twelve names, three salvages, four
restorations, and two deliberate relinquishments. The year note is read aloud
at the year stone, and Bea reads the tower's line because Dunc's voice is old
now, and the marks are still there.

---

## 32. APPENDIX S — VIGNETTES (TONE SAMPLE)

> Dunc sets the crack gauge in the morning and reads it at noon and writes 88.6
> in the ledger, and the number is not dramatic, and the number is the whole
> reason the tower is still standing, because the number was three millimeters
> more than last year and somebody noticed.

> Petra stands under the dam and listens to the seep with the back of her hand
> against the stone and decides, and then writes the decision down before she
> tells anyone, because a decision told before it is written becomes an opinion.

> Ava reads the year note in the cold room, and the last paragraph is about the
deer coming back, and an old resident who has complained about the shelter all
> winter says nothing and cries a little and then asks who counted them.

> Bea walks the north round alone for the first time and finds a new hairline
> crack in the buttress and does everything exactly by the card, and Dunc reads
> her note in the evening and adds one line: found it early, good.

---

## 33. APPENDIX T — FAILURE MODES AND RECOVERY

| Failure | Effect | Recovery |
|---|---|---|
| No register | places vanish unnamed | open register, backfill |
| Late threshold | surprise collapse | review thresholds |
| Bad survey | wrong decision | re-measure, correct |
| Unaffordable plan | half-finished work | phase, pause, record |
| Salvage greed | memory loss | record and interpret |
| Contamination missed | risk to people | resample, clean |
| Reclamation rushed | collapse or illness | stop, prove, reoccupy |
| Scarcity unannounced | panic | notice, substitute |
| Year note skipped | institutional amnesia | write it late, read it |
| Loss unnamed | grief without memory | name it in the note |

No change is unmanageable if it is observed, and no loss is complete if it is
named. The table's last row is the expansion's promise: the shelter may not be
able to save a place, but it can always write down what the place was.

---

## 34. APPENDIX U — CONTENT REVIEW CHECKLIST

- [ ] No real landmarks or heritage sites are copied.
- [ ] No catastrophe spectacle or doom clock exists.
- [ ] No looting reward loop exists.
- [ ] `LocationEvolutionSystem` remains the location state authority.
- [ ] `LandmarkDegradationSystem` remains the landmark authority.
- [ ] Maps stay with the cartography owner; this plan annotates only.
- [ ] Human sites route memory through the live memory owners.
- [ ] Every loss gets a name and a record.
- [ ] Reclamation requires proven water, shelter, and safety.
- [ ] Save additions are additive and legacy-neutral.
- [ ] Determinism uses live seeded paths only.

---

## 35. APPENDIX V — GLOSSARY

- **Round** — a scheduled walk that observes the valley's posts.
- **Mark** — a durable physical reference that makes change measurable.
- **Threshold** — a number and a consequence written before it trips.
- **Register** — the list of landmarks with baselines and decisions.
- **Stabilize** — slow a landmark's decline without pretending to reverse it.
- **Witness** — record a collapse rather than prevent it.
- **Salvage** — remove useful materials with a record and a cost.
- **Reclaim** — bring a location back to use in proven phases.
- **Scarcity watch** — a tracked trend with a substitution plan.
- **Year note** — the one-page annual record read aloud at winter.

---

## 36. APPENDIX W — INTEGRATION MATRIX

| System | Reads | Writes | Never writes |
|---|---|---|---|
| `LocationEvolutionSystem` | world | location state | observations |
| `LandmarkDegradationSystem` | time | landmark state | register |
| `WorldObservationSystem` | live state | posts, rounds | world state |
| `LandmarkCareSystem` | register | decisions, works | landmark state |
| `LocationStateSystem` | mutations | decisions | mutation state |
| `ScarcitySystem` | seeds | watches, notices | inventory |
| `ReclamationSystem` | site state | phases | contamination |
| `ChangeChronicleSystem` | all | notes, histories | world state |
| `ThresholdWatchSystem` | metrics | alerts | world state |
| `WildlifeMigrationSystem` | counts | nothing | nothing |
| `RouteInfrastructureSystem` | routes | nothing | nothing |
| `WeatherSystem` | weather | nothing | nothing |
| `BuildWorksSystem` | plans | builds | landmark state |
| `DecontaminationSystem` | sites | cleanup | contamination |
| `StandingRecord` | notes | records | nothing |
| `MemorialSystem` | losses | memory | nothing |
| `EpilogueChronicleBuilder` | years | chronicle | nothing |

---

## 37. APPENDIX X — DATA SCHEMA DETAIL (NEW CATALOGS)

**`observation_posts.json`** — `post_id`, `display_name`, `location_id`,
`instruments[]`, `round_route`, `season`, `staff`, `tags`.

**`survey_rounds.json`** — `round_id`, `display_name`, `stops[]`, `duration`,
`season`, `output`, `tags`.

**`landmark_register.json`** — `landmark_id`, `baseline`, `decision`, `plan`,
`cost[]`, `record`, `tags`.

**`location_state.json`** — `location_id`, `ownership`, `contamination`,
`state`, `action`, `review`, `tags`.

**`scarcity_watch.json`** — `watch_id`, `good`, `trend`, `evidence`,
`substitute`, `notice`, `tags`.

**`reclamation_plans.json`** — `plan_id`, `site`, `phases[]`, `staff`,
`materials[]`, `reoccupy_when`, `tags`.

**`world_notes.json`** — `note_id`, `year`, `change`, `decision`, `named`,
`read`, `tags`.

**`site_histories.json`** — `site_id`, `first_record`, `change`, `current_state`,
`keeper`, `tags`.

**`threshold_alerts.json`** — `alert_id`, `metric`, `threshold`, `consequence`,
`owner`, `trip`, `review`, `tags`.

**`change_claims.json`** — `claim_id`, `change`, `evidence`, `confidence`,
`action`, `archive`, `tags`.

All new catalogs carry `schema_version: 1` and fail the integrity gate on
missing or duplicate IDs, invalid references, or out-of-range numbers.

---

## 38. APPENDIX Y — MEASUREMENT AND TELEMETRY

| Metric | Purpose | Source |
|---|---|---|
| Rounds completed | observation discipline | Survey |
| Marks maintained | measurement quality | Posts |
| Thresholds tripped | vigilance | Alerts |
| Thresholds missed | review priority | Alerts |
| Landmarks stabilized | care | Register |
| Landmarks lost and named | memory | Chronicle |
| Sites reclaimed | return | Reclamation |
| Scarcity notices posted | adaptation | Scarcity |
| Year notes written | institutional memory | Chronicle |
| Site histories kept | continuity | Histories |

Telemetry is diagnostic only; it never gates content and never becomes a score
against a surveyor or a year.

---

## 39. APPENDIX Z — IMPLEMENTATION CHECKLIST

- [ ] Phase 0 premise re-audit recorded with file:line evidence.
- [ ] Phase 1 catalogs authored and registered with validators and scanner.
- [ ] Phase 2 Core systems are pure, engine-free, and deterministic.
- [ ] Phase 3 save additions are additive, legacy-neutral, and round-trip tested.
- [ ] Evolution, landmark, wildlife, weather, route, and record authorities
      remain untouched.
- [ ] Triad parity (Setup / Save / Flush) holds for every new state.
- [ ] Phase 4 host session exposes live state and real commands only.
- [ ] Phase 5 panels pass bind/unbind/rebind, focus, contrast, and scaling checks.
- [ ] Phase 6 content passes the review checklist in §34.
- [ ] Phase 7 soak shows a crack, a channel change, a scarcity, and a reclaim.
- [ ] Phase 8 data integrity and content utilization selftests pass.
- [ ] No doom, looting, or real-landmark content exists.

---

## 40. APPENDIX AA — OPEN QUESTIONS FOR REVIEW

1. Can a landmark's baseline be restored after collapse, or does it close?
2. Who inherits a site history when its keeper leaves or dies?
3. Should thresholds ever be publicly posted for all residents?
4. Can reclaimed sites be taken up by outposts rather than the main shelter?
5. Does observing a place ever change it, and how is that shown?
6. Are scarcity substitutes ever worse, and does the notice say so honestly?
7. How many years does a year note sequence need before it becomes a tradition?
8. Can the shelter choose to stop surveying a place, and what happens then?

None of these may be decided unilaterally; each changes tone and balance.

---

## 41. APPENDIX AB — CROSS-WAVE HOOKS

| Wave | Expansion | Hook |
|---|---|---|
| 1 | 12 The Second Generation | Apprentices in survey |
| 1 | 15 The Deep Root | Orchard and soil return |
| 2 | 18 The Underneath | Subsidence observation |
| 2 | 21 The Grid | Line and structure decay |
| 3 | 22 The Clean Flow | Contamination in sites |
| 3 | 23 The Alarm | Structural emergency calls |
| 3 | 24 The Long Goodbye | Memory for lost places |
| 3 | 25 The Iron Road | Viaduct and rail decay |
| 4 | 27 The Thread | Restorer clothing and covers |
| 4 | 30 The Press | Printed chronicle |
| 4 | 31 The Kiln | Mortar and restoration |
| 5 | 32 The Wild | Return valley counts |
| 5 | 33 The Weather | Erosion and seasons |
| 5 | 34 The Long Road | Route burial and clearing |
| 5 | 36 The Watch | Posts and rounds overlap |
| 6 | 38 The Ward | Hospital reclamation |
| 6 | 40 The Wheel | Mill restoration |
| 7 | 42 The Core | Deep vault monitoring |
| 7 | 43 The Question | Long-term observation |
| 7 | 44 The Outpost | Remote site reports |
| 7 | 45 The Envoy | Treaties across change |

Each hook is additive. The Long Change can ship alone, and every other
expansion can ship without it.

---

## 42. APPENDIX AC — ENDING PROSE SKETCHES

**The Watched Valley.** Observation and thresholds make slow change ordinary,
and the shelter is never surprised by anything it could have measured.

**The Kept Stone.** One landmark is saved properly and several are let go with
records and respect, and the shelter learns the difference between saving and
hoarding.

**The Long Survey.** A decade of rounds makes the shelter the valley's expert,
and its year notes become the only continuous history anyone has.

**The Return.** Wildlife, water, and ground recover where the shelter helped,
and the counts prove that patience is a form of repair.

**The Letting Go.** The tower falls in a long autumn and the shelter watches,
records the sound, names the year, and moves on without pretending it was
nothing.

**Fade.** A year stone with a new line carved in it and a valley that looks
almost the same as last year, which is the best thing a valley can do.

---

## 43. APPENDIX AD — COMMON FAILURE PATTERNS TO AVOID

| Pattern | Why it is bad | Correct approach |
|---|---|---|
| Doom clock | anxious | gradual, partial change |
| Looting glee | hollow | recorded salvage |
| Instant restore | dishonest | phases and proofs |
| Map takeover | authority break | annotate only |
| Unnamed losses | erasure | names and notes |
| Busywork rounds | boring | thresholds and meaning |
| Nostalgia loop | stagnant | decisions and futures |
| Mutation bypass | critical break | live owners |
| Perfect prediction | false | trends with uncertainty |
| Permanent decline | bleak | recovery and return |

The list exists because worlds that age are easy to write as a countdown.
The expansion's rule is that change is ordinary, measurable, and sometimes
kind, and that a shelter's maturity is measured by what it observes and names.

---

## 44. APPENDIX AE — CONTENT VOLUME SUMMARY

| Category | Rows | Prose estimate |
|---|---|---|
| Observation posts | 10 | 2,500 |
| Survey rounds | 8 | 2,000 |
| Landmark register | 20 | 4,500 |
| Location states | 20 | 4,000 |
| Scarcity watches | 10 | 2,500 |
| Reclamation plans | 10 | 3,000 |
| Year notes | 10 | 2,500 |
| Site histories | 12 | 3,500 |
| Threshold alerts | 14 | 3,000 |
| Change claims | 16 | 3,000 |
| Quests | 53 | 15,000 |
| NPCs | 8 | 7,000 |
| Locations | 10 | 3,500 |
| Items | 16 | 2,500 |
| Endings | 6 | 3,000 |
| **Total** | | **~61,500** |

---

## 45. APPENDIX AF — FIRST FIVE YEARS OF THE VALLEY

| Year | Focus | Milestone |
|---|---|---|
| 1 | Crack | register opened |
| 2 | River | channel remapped |
| 3 | Road | burial decision |
| 4 | Return | deer named |
| 5 | Dam | salvage and witness |

Five years is the minimum honest arc for world change. By the end, the shelter
has a register, a set of thresholds, a named recovery, a deliberate salvage, and
a year note that reads like a place rather than a survival log.

---

## 46. APPENDIX AG — WORLD MEMORY COVENANT

| Clause | Promise |
|---|---|
| Observe | The world changes whether or not we watch |
| Measure | Marks and gauges make change real |
| Warn | Thresholds are written before they trip |
| Decide | Every landmark gets a decision and a reason |
| Work | Restoration is phased and proven |
| Name | Every loss and recovery is named |
| Record | Year notes and site histories are kept |
| Hand over | Every site has a keeper |
| Adapt | Scarcity is met with notices and substitutes |
| Respect | Human places route through the memory owners |

The covenant is the expansion's first-class design object. A shelter cannot stop
a valley from changing, but it can know what is happening, choose carefully,
and leave a memory that is longer than any person's time in it.

---

## 47. APPENDIX AH — SITE KEEPER SUCCESSION TABLE

| Site | First keeper | Successor | Record | Handover |
|---|---|---|---|---|
| Tower | Dunc | Bea | marks book | walk together |
| Dam | Petra | Bea | decision file | read aloud |
| Road | Janka | crew lead | clearing log | season walk |
| Valley | Rosso | new counter | count book | dawn count |
| River | Berk | apprentice | channel marks | spring round |
| School | Ava | teacher | year pages | read in class |
| Orchard | Merle | gardener | bud count | prune season |
| Hospital | Ikra | ward lead | clean record | survey walk |
| Signal tower | Miri | operator | signal log | test call |
| Year stone | Ava | next reader | carved lines | winter reading |

Succession is how a valley keeps its memory after the people who made it are
gone. The handover column is the practice: nobody inherits a site from a file
alone; they walk it with the person who kept it.

---

## 48. APPENDIX AI — MARK AND INSTRUMENT TABLE

| Mark | Made of | Reads | Replaced | Notes |
|---|---|---|---|---|
| Crack gauge | glass strip | crack width | yearly | tower |
| Ash gauge | painted pole | burial depth | season | road |
| Level bar | wood, tube | tilt | yearly | dam |
| Survey stake | hardwood | position | yearly | slope |
| Stone cairn | stones | route | rare | paths |
| Depth pole | painted pole | silt | season | harbor |
| Flow stick | notched stick | water speed | season | river |
| Taste card | chart | water quality | monthly | spring |
| Bud count board | slate | orchard | season | orchard |
| Count clicker | wood | deer | yearly | valley |
| Seep jar | glass | seep rate | event | dam |
| Year chisel | steel | years | reuse | stone |

Twelve marks and instruments, most of them wood, paint, and patience. The list
is a quiet argument that observation is a craft of durable small objects rather
than rare technology, and that a piece of painted wood can hold ten years of a
valley's honesty.

---

## 49. APPENDIX AJ — CHANGE CLAIM TABLE

| # | Claim | Evidence | Confidence | Action | Archive |
|---|---|---|---|---|---|
| 1 | Tower is failing | gauge trend | high | stabilize | yes |
| 2 | River moved | old marks | high | remap | yes |
| 3 | Road is burying | ash gauge | high | clear plan | yes |
| 4 | Deer returned | counts | medium | keep counting | yes |
| 5 | Dam seeps | jar rate | high | salvage plan | yes |
| 6 | Harbor silts | depth poles | high | dredge plan | yes |
| 7 | Orchard can return | buds | medium | graft | yes |
| 8 | Slope is moving | stakes | high | warn | yes |
| 9 | Spring is clean | taste, jar | medium | capture | yes |
| 10 | Wire is scarce | yard count | high | substitute | yes |
| 11 | Ash falls slower | gauge | low | keep watching | yes |
| 12 | Cold comes earlier | notes | medium | plan | yes |
| 13 | School is dry | survey | high | reclaim | yes |
| 14 | Hospital traces fade | swabs | medium | recheck | yes |
| 15 | Valley herd growing | counts | medium | keep counting | yes |
| 16 | Nothing changed this year | rounds | honest | write it | yes |

The last claim is included on purpose: a year in which the valley did nothing
notable is still a finding, it is written down with the same care, and the
shelter's record stays continuous because of it.

---

## 50. APPENDIX AK — RECLAMATION PHASE TABLE

| Phase | Work | Proof | Can pause |
|---|---|---|---|
| Survey | measure and mark | map and numbers | yes |
| Clear | remove debris | safe access | yes |
| Clean | decontaminate | swabs pass | yes |
| Structure | roof, walls, floor | wind and rain tight | yes |
| Services | water, heat, light | tested | yes |
| Fit | furniture, stores | usable | yes |
| Prove | live in it briefly | comfortable | yes |
| Occupy | hand over | keeper named | no |

Eight phases with a proof between each, and seven of them can be paused. Pausing
is a feature, not a failure: a shelter with limited labor needs to be able to
walk away from a half-reclaimed building without losing the work or the record.

---

## 51. APPENDIX AL — YEAR ROUND CALENDAR TABLE

| Season | Rounds | Decisions | Note |
|---|---|---|---|
| Spring | river, valley | water plans | draft |
| Summer | ridge, harbor | erosion plans | draft |
| Autumn | east route, full round | salvage, reclamation | year note |
| Winter | structure, stone | read aloud | read |
| Event | emergency round | threshold response | alert note |

The year round has a shape the whole shelter can learn. Observations happen in
three seasons, decisions land in autumn when the harvest is in, and the year is
read aloud in winter when there is time to remember.

---

## 52. CLOSING STATEMENT

ASHFALL already ages the world: locations change owners and states, landmarks
lose integrity and vanish under ash, wildlife migrates, routes decay, and a
seed file describes all of it. What it lacks is the human side: observation
posts, rounds, registers, thresholds, decisions, reclamation, and the yearbook
that lets a shelter know its own valley. The Long Change adds that practice
without adding a map system, a doom clock, or a looting loop. It adds a gauge on
a cracking wall, a count of returning deer, a decision recorded with its
reason, and a year stone with a new line carved in it.

> Wave 7 note: this plan is one of five Wave 7 expansion bibles (42–46). Each is
> self-contained; none requires another to ship. The shared Wave 7 index lives
> at `docs/expansions/wave7/WAVE7_INDEX.md`. The safe pre-signature step is
> Phase 1 (data schemas and validators), which is additive and reversible.
> Evidence anchors: `LocationEvolutionSystem` (`LocationEvolutionSaveState` with
> `lastEvolutionDay` and `mutations`; `LocationMutationRecord` with
> `currentOwner`, `contaminationLevel`, `lootDepletionFactor`, `isCleared`,
> `isRuined`), `LandmarkDegradationSystem` (`LandmarkSaveState` with
> `lastDegradationDay` and `landmarks`; `LandmarkStatusRecord` with
> `structuralIntegrity`, `ashBurialCm`, `isCollapsed`, `isScavenged`,
> `collapseDay`), `EvolvingWorldCatalog` (`EvolvingWorldSeedContainer` and its
> sector, pack, landmark, location-seed, and scarcity records), and
> `world_evolution_seeds.json` (20,728 B describing a 24-sector migration graph,
> 24 wildlife packs, 30 landmark baselines, 40 location seeds, and 4 scarcity
> goods) plus `damaged_map_zones.json` (15,568 B).