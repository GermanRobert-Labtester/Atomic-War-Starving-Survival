# ASHFALL — Expansion 44 Design Bible
# THE OUTPOST
### Wave 7 · Colonies, Founding, Supply Lines, Remote Life, Defense, and Recall

**Document status:** Design plan (pre-integration). Not a claim. Not an authorization.
**Date:** 2026-09-24
**Domain owners touched:** `Ashfall.Core.Expeditions` (ColonySystem, ColonyState, SupplyLine, ArmoredCrawlerExpeditionSystem), `Ashfall.Core.Logistics` (Long Line seam)
**Proposed host owner:** `OutpostHostSession` (extends colony, supply, and remote-face surfaces)
**Existing save sections:** `ColonyState`, expedition state, supply and radio state
**Existing CLI verbs:** `--colony-selftest` (if present), `--data-integrity-selftest`, `--content-utilization-selftest`
**Rule compliance:** Godot authoritative; Core engine-free; JSON data authoritative; one authority per concern; deterministic seeded RNG.

---

## 0. HOW TO READ THIS DOCUMENT

This is a **design bible**, not an integration plan. Implementation must later pass
through `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `TEST_POLICY.md`.

ASHFALL already has colonies as state. `ColonySystem` exposes `TotalColonyCount`,
`ActiveSupplyLineCount`, `OnColonyEstablished`,
`OnSupplyLineStatusChanged`, `OnColonySuppliesUpdated`, `EstablishColony(...)`,
`EstablishSupplyLine(...)`, `SetSupplyLineStatus(lineId, status)`,
`AssignSurvivorToColony(colonyId, survivorId)`,
`TransferSupplies(colonyId, amount)`, `TickDay(currentDay)`, `GetColonies()`,
`GetColony(colonyId)`, `CaptureState()`, and `RestoreState(state)`.
`ColonyOutpost` carries `ColonyId`, `LocationId`, `Name`, `Type` (default
`Outpost`), `EstablishedDay`, `DefenseRating` (50f), `MoraleRating` (70f),
`PopulationIds`, `StoredSupplies` (50f), and `IsActive`. `SupplyLine` carries
`LineId`, `OriginId`, `DestinationId`, `Status`, `CargoCapacity` (100f),
`DailyFlow` (10f), and `LastSupplyDay`. `ColonyState` carries `SchemaVersion`,
`NextSequence`, `Colonies`, and `SupplyLines`. The expedition family also
includes `ArmoredCrawlerExpeditionSystem`, `ColonyType`, and `SupplyLineStatus`.
The data holds `settlements.json` (24,974 B) and
`wasteland_settlement_npcs.json` (30,940 B) describing other settlements.

What does not exist: charters, site surveys, founding parties, depots, supply
schedules, outpost life, remote morale, defense plans, radio reports, mail,
rotation, recall, and abandonment. Colonies are state without a story.

**The Outpost** turns remote settlements into lived places: why they exist,
who lives there, what they need, how they are supplied, how they are defended,
how they are spoken to, and when they come home. It extends the live colony,
supply, and expedition owners and hands route, defense, and comms facts to
their live owners.

Conventions: **`LIVE`** confirmed in source/data; **`GAP`** confirmed thin;
**`PROPOSED`** new.

### 0.1 Boundary contracts

| Neighbour | Owns | This plan does |
|---|---|---|
| 34 The Long Road (Wave 5) | Routes, convoys, bridges | Schedules convoys; never defines the road |
| 11 The Long Line | Communications and logistics lines | Uses its radio and dispatch seams |
| 36 The Watch (Wave 5) | Defense, patrols, territory | Requests watch cover; never owns patrols |
| 42 The Core (Wave 7) | Power generation | Requests remote power; never owns generation |
| 43 The Question (Wave 7) | Site surveys and study | Uses survey findings for founding |
| 45 The Envoy (Wave 7) | Diplomacy and neighbors | Uses agreements; never negotiates them |
| 46 The Long Change (Wave 7) | World change over time | Observes site change; never mutates it |
| 22 The Clean Flow (Wave 3) | Water and sanitation | Requests a site water plan |
| 31 The Kiln (Wave 4) | Construction materials | Orders builds; never fires them |
| 03 The Standing Record | Records and charters | Files the charter; never owns it |

---

## 1. EXECUTIVE SUMMARY

### 1.1 Pitch

The shelter is full, the valley has a good site, and nobody has ever lived away
from the main gate.

**The Outpost** is the expansion about distance: choosing a site, writing a
charter, sending people, building a place, running a supply line, hearing a
voice on the radio every night, defending a fence with six people, and deciding
when a remote place becomes home or becomes a retreat.

### 1.2 The five loops it adds

```
  Charter ──► Found ──► Supply ──► Sustain ──► Recall
     │          │         │          │           │
     ▼          ▼         ▼          ▼           ▼
   Purpose,  Site,      Convoys,  Life,       Rotation,
   people    build      depots    morale      closure
                                        │
                                        ▼
                                 Report ──► Home herd ──► Decide
```

### 1.3 What the player manages

1. **Charters.** Why the outpost exists and who owns it.
2. **Sites.** Water, road, exposure, neighbors, and build ground.
3. **Founding.** The first party, first winter, and first build.
4. **Supply.** Lines, depots, convoys, priorities, and losses.
5. **Life.** Housing, food, morale, skills, and routines.
6. **Defense.** Fences, watch, radios, and drills.
7. **Comms.** Scheduled calls, reports, mail, and silence.
8. **Relations.** Neighbors, agreements, and local labor.
9. **Rotation.** Who goes, who stays, and who comes home.
10. **Recall.** Evacuation, closure, or permanence.

### 1.4 What it is not

- Not a city-builder spinoff. It is the shelter's far hand, not a second game.
- Not a second colony or supply system. It extends the live owners.
- Not an autonomous settlement that plays itself; it depends on people.
- Not a conquest mechanic; neighbors are people with agreements and boundaries.
- Not a punishment posting; outpost service is chosen work with real costs.
- Not a new save section.

---

## 2. EVIDENCE BASE AND GAP ANALYSIS

### 2.1 Live systems (verified)

| File | Role | Status |
|---|---|---|
| `Assets/Ashfall.Core/Expeditions/ColonySystem.cs` | Outposts, supply lines, population | `LIVE` |
| `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` | Parties and camps | `LIVE` |
| `Assets/Ashfall.Core/Expeditions/ArmoredCrawlerExpeditionSystem.cs` | Heavy expedition transport | `LIVE` |
| `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` | Discovery results | `LIVE` |
| `Assets/Ashfall.Core/World/RouteInfrastructureSystem.cs` | Road state (Wave 5) | `LIVE` |
| `Assets/Ashfall.Core/Radio/*` | Radio calls (Wave 5 ties) | `LIVE` |
| `Assets/Ashfall.Core/Shelter/ShelterAssignmentSystem.cs` | Housing | `LIVE` |
| `Assets/Ashfall.Core/Needs/NeedsSystem.cs` | Condition and morale | `LIVE` |

### 2.2 Live data (counted)

| Catalog | Size | Notes |
|---|---|---|
| `settlements.json` | 24,974 B | external settlements |
| `wasteland_settlement_npcs.json` | 30,940 B | settlement people |
| `expeditions.json` | 30,255 B | expedition content |
| `anomalous_expedition_encounters.json` | **2,267 B** | thin |
| Charters, sites, depots, reports, rotation data | absent | confirmed none |

### 2.3 Confirmed gaps

- **GAP-44-1 — No charters or purposes.**
- **GAP-44-2 — No site survey or founding content.**
- **GAP-44-3 — No depots or supply schedules.**
- **GAP-44-4 — No outpost life content.**
- **GAP-44-5 — No remote morale or skill content.**
- **GAP-44-6 — No defense plans.**
- **GAP-44-7 — No radio reports or mail.**
- **GAP-44-8 — No rotation or recall.**
- **GAP-44-9 — No neighbor relations at the outpost scale.**
- **GAP-44-10 — No outpost rooms, staff, or roster content.**

### 2.4 Non-duplication statement

This expansion will **not** add a second colony, supply, road, defense, comms,
or housing system. It extends `ColonySystem` with charters and life content,
the expedition owner with founding parties, the road and radio owners at their
seams, and the watch at its defense seam. It adds state only as additive
sub-objects of the existing colony and expedition stores. No new save section.

---

## 3. DESIGN PILLARS AND TONE

### 3.1 Pillars

**Pillar 1 — Distance is a cost, not a flavor.** Every kilometer is food,
fuel, risk, and time.

**Pillar 2 — A charter prevents drift.** The reason for a place is written
before it is built and re-read when it changes.

**Pillar 3 — Supply is the umbilical.** An outpost lives as long as its line.

**Pillar 4 — Remote people are people.** Housing, food, mail, rest, and
rotation matter more at a distance, not less.

**Pillar 5 — Coming home is a success.** Recall is a plan, not a failure.

### 3.2 Tone calibration

| Element | Do | Do not |
|---|---|---|
| Founding | Work, weather, patience | Frontier romance |
| Supply | Schedules and losses | Logistics porn |
| Remote life | Routine, loneliness, humor | Martyrdom |
| Defense | Fences, drills, radio | Gunplay pride |
| Radio call | Voices and codes | Melodrama |
| Neighbors | Agreements and respect | Colonial framing |
| Recall | Careful, planned, dignified | Shame |
| Closure | Recorded and remembered | Erasure |

### 3.3 Content limits

- No colonial framing; outposts are shelters of the same community, never
  settlements over other peoples.
- No child labor, no punishment postings, no forced relocation.
- No defense spectacle; violence stays with the live combat owner and is rare.
- No useless sacrifice; every outpost has a recall plan and the right to use it.
- No autonomous economy that bypasses the shelter's owners.
- No new save section.

---

## 4. THE OUTPOST WORLD

### 4.1 Interior rooms

- **`room_charter_desk`** — the charter and the map.
- **`room_dispatch`** — the board of convoys and priorities.
- **`room_radio_corner`** — a set, a clock, and a log.
- **`room_outpost_kit`** — crates, tents, and departure loads.
- **`room_home_herd`** — the return room where families wait.
- **`room_survey_desk`** — site reports and photographs.
- **`room_recall_board`** — plans and thresholds.
- **`room_mail_room`** — bags, letters, and the courier's board.

### 4.2 Exterior locations

| ID | Name | Tone | Purpose |
|---|---|---|---|
| `loc_valley_site` | The Valley Site | 3 | The first outpost |
| `loc_road_junction` | The Junction | 3 | Supply division |
| `loc_depot_halfway` | The Halfway Depot | 3 | Fuel and food cache |
| `loc_outpost_gate` | The Outpost Gate | 4 | Arrival and departure |
| `loc_ridge_relay` | The Ridge Relay | 3 | Radio link |
| `loc_river_flat` | The River Flat | 3 | Water and fields |
| `loc_old_homestead` | The Old Homestead | 3 | Reoccupation |
| `loc_neighbor_line` | The Neighbor Line | 4 | Agreement boundary |
| `loc_winter_track` | The Winter Track | 4 | Cold route |
| `loc_recall_point` | The Recall Point | 3 | Assembly for return |

All locations require valid item references and scanner registration.

### 4.3 The relief cycle

Convoys and radio calls structure remote life. Every week has a call; every
season has a convoy; every year has a review. The expansion's clock is the
relief cycle.

---

## 5. MAIN STORYLINE — "THE VALLEY SITE"

### 5.1 Central conflict

The shelter needs room for its fields and its people, and there is a valley
site with water and a road. **Halla Ryn** volunteers to lead the first party
and wants a charter that says exactly what the outpost is for. **Dero** the
supply captain wants a depot and a schedule before anyone sleeps up there.
**Miri** the wireless operator wants a nightly radio window and a strict
silence protocol. **Ikra** the medic wants medicine cached and a recall
threshold written down. **Bron** the builder wants to know what the ground
does in winter. **Yara** the scout wants the route walked in bad weather
before it is trusted.

Then the first winter closes the road for three weeks, the radio fails, and the
shelter has to decide whether the silence means weather, failure, or something
worse — and whether it will break its own supply plan to go looking.

The expansion's question: **what do we owe the people we send away?**

### 5.2 Theme (unspoken)

**A community is only as wide as its willingness to bring people home.**

### 5.3 Principal NPCs

| ID | Name | Role | Function |
|---|---|---|---|
| `npc_outpost_leader_halla_ryn` | Halla Ryn | Outpost leader | Charter, life, decisions |
| `npc_supply_captain_dero` | Dero | Supply captain | Convoys, depots, priorities |
| `npc_wireless_operator_miri` | Miri | Wireless operator | Calls, codes, silence |
| `npc_outpost_medic_ikra` | Ikra | Outpost medic | Medicine, thresholds, care |
| `npc_builder_bron` | Bron | Builder | Site work and winter proofs |
| `npc_scout_yara` | Yara | Scout | Routes and weather |
| `npc_trader_lef` | Lef | Trader | Neighbor relations and goods |
| `npc_family_lia` | Lia | Family | The home herd and return |

### 5.4 Story beats (15)

1. **The Crowding.** The shelter needs room.
2. **The Site.** The valley is walked and surveyed.
3. **The Charter.** Purpose, population, and limits are written.
4. **The First Party.** Who goes, with what, for how long.
5. **The Build.** Shelter, water, fence, and a radio mast.
6. **The Depot.** The halfway cache is built.
7. **The First Call.** The nightly radio window begins.
8. **The Neighbors.** A nearby family is met at the boundary line.
9. **The Winter.** The road closes and the outpost is alone.
10. **The Silence.** The radio fails and the shelter must choose.
11. **The Relief.** A winter convoy is attempted or refused.
12. **The Rota.** Rotation is written and the first party comes home.
13. **The Second Year.** The outpost becomes a place rather than a project.
14. **The Threshold.** A recall condition is met and tested.
15. **The Valley Site.** The outpost's future is decided.

### 5.5 Branching choices (8)

| Choice | Options | Axis |
|---|---|---|
| Purpose | farm / mine / way-station / mixed | focus vs. resilience |
| Size | six / twelve / twenty | viability vs. burden |
| Supply | weekly / seasonal / emergency | cost vs. risk |
| Autonomy | dependent / semi / independent | control vs. trust |
| Defense | fence / watch / neighbor agreement | arms vs. relations |
| Comms | nightly / scheduled / silence | contact vs. security |
| Rotation | permanent / yearly / rotating | attachment vs. fairness |
| Final | permanent / seasonal / recalled | growth vs. home |

### 5.6 Endings (5 + fade)

1. **The Second Hearth** — the outpost becomes a real place with a future and
   a shared charter.
2. **The Long Line Home** — the supply line and radio make two settlements one
   community.
3. **The Careful Frontier** — the outpost stays small and safe and never
   becomes a burden.
4. **The Winter That Held** — a hard season tests everything and the plan
   holds, because the plan was honest.
5. **The Recall** — the outpost comes home with its people, its records, and
   its name, and no one is left.
6. **Fade** — a lamp in a valley window and a radio call at nine.

---

## 6. QUEST DESIGN

New IDs use prefix `quest_outpost_`. Schema follows existing quest catalogs.

### 6.1 Main questline (15)

`quest_outpost_crowding`, `quest_outpost_site`, `quest_outpost_charter`,
`quest_outpost_first_party`, `quest_outpost_build`, `quest_outpost_depot`,
`quest_outpost_first_call`, `quest_outpost_neighbors`, `quest_outpost_winter`,
`quest_outpost_silence`, `quest_outpost_relief`, `quest_outpost_rota`,
`quest_outpost_second_year`, `quest_outpost_threshold`,
`quest_outpost_valley_site`.

### 6.2 Side quests (30)

**Founding (5)**
- `quest_outpost_survey` — survey the site
- `quest_outpost_water_site` — water at the site
- `quest_outpost_ground` — test the ground
- `quest_outpost_kit_load` — load the first kits
- `quest_outpost_raise_flag` — raise the mark

**Supply (5)**
- `quest_outpost_line` — establish the line
- `quest_outpost_convoy` — run a convoy
- `quest_outpost_depot_cache` — cache supplies
- `quest_outpost_fuel_cache` — fuel at the halfway
- `quest_outpost_loss` — handle a lost load

**Life (5)**
- `quest_outpost_house` — build housing
- `quest_outpost_food` — food at the outpost
- `quest_outpost_rest` — rest and routine
- `quest_outpost_school` — letters and lessons
- `quest_outpost_festival` — a celebration at distance

**Defense (5)**
- `quest_outpost_fence` — fence the site
- `quest_outpost_watch_post` — a watch post
- `quest_outpost_drill` — outpost drill
- `quest_outpost_neighbor_guard` — shared watch
- `quest_outpost_radio_alarm` — radio alarm plan

**Comms (5)**
- `quest_outpost_call_window` — set the window
- `quest_outpost_code` — write a code
- `quest_outpost_mail` — carry the mail
- `quest_outpost_report` — weekly report
- `quest_outpost_silence_rule` — silence protocol

**Return (5)**
- `quest_outpost_rotation` — write rotation
- `quest_outpost_threshold_write` — recall threshold
- `quest_outpost_pack` — pack to return
- `quest_outpost_handover_outpost` — handover
- `quest_outpost_home_herd` — the return room

### 6.3 Repeatable quests (8)

`quest_outpost_repeat_convoy`, `quest_outpost_repeat_call`,
`quest_outpost_repeat_report`, `quest_outpost_repeat_depot`,
`quest_outpost_repeat_drill`, `quest_outpost_repeat_mail`,
`quest_outpost_repeat_rotate`, `quest_outpost_repeat_cache`.

### 6.4 Dynamic hooks

Live events (road closures, weather, convoy losses, radio failures, neighbor
contact, expedition returns, power and water state, world change) attach
authored follow-ups through existing seams. No new event bus.

### 6.5 Constraints

- Colony, supply, and population state stay with `ColonySystem`.
- Routes stay with the road owner; radio with the comms owner; defense with the
  watch owner.
- Site surveys route through 43; neighbor agreements through 45.
- No colonial framing, forced relocation, or defense spectacle.
- No new save section.

---

## 7. NEW GAMEPLAY SYSTEMS

### 7.1 `OutpostCharterSystem` (new, `Ashfall.Core.Expeditions`)

**Owns:** charters, purposes, population plans, site surveys, and founding
parties. **Consumes:** `ColonySystem.EstablishColony`, `ExpeditionSystem`
(party), 43's survey findings, 34's route state, 31's build owners.
**Data:** `outpost_charters.json`, `outpost_sites.json`, `outpost_roles.json`.
**Rules:** a charter names purpose, size, limits, and a recall threshold; a
site must pass water, route, and ground checks before founding; the charter is
re-read at every review.

### 7.2 `OutpostLogisticsSystem` (new, `Ashfall.Core.Expeditions`)

**Owns:** supply lines, depots, convoy schedules, priorities, and losses.
**Consumes:** `ColonySystem` supply lines, `RouteInfrastructureSystem`,
`TravelingCaravanSystem`, `Inventory`. **Data:** `supply_lines.json`,
`outpost_depots.json`. **Rules:** every line has capacity, flow, and a schedule;
depots hold real stock; losses are recorded and answered with a new plan.

### 7.3 `OutpostLifeSystem` (new, `Ashfall.Core.Expeditions`)

**Owns:** remote housing, food, routines, morale inputs, skills, and local
rules. **Consumes:** `ColonyOutpost.MoraleRating`, `NeedsSystem`,
`ShelterAssignmentSystem` (site-side), kitchen and water owners, 28's teaching
seam. **Data:** `outpost_life.json`. **Rules:** remote life runs on the same
needs owners as home; morale is affected by distance, mail, rest, and fairness;
nothing invents a second needs system.

### 7.4 `OutpostDefenseSystem` (new, thin, `Ashfall.Core.Expeditions`)

**Owns:** outpost fences, watch requests, drills, and alarm plans.
**Consumes:** `WatchHouseHostSession` (Wave 5), `ShelterFireHazardSystem`,
`Inventory`, radio. **Data:** `outpost_defense.json`. **Rules:** defense is
watch, fence, and agreement before it is force; every outpost has a drill and
a call-for-help plan; force stays with the live combat owner.

### 7.5 `OutpostCommsSystem` (new, thin, `Ashfall.Core.Expeditions`)

**Owns:** call windows, codes, reports, mail bags, and silence protocol.
**Consumes:** the radio owners, `Mail`-free courier content, `StandingRecord`.
**Data:** `outpost_reports.json`, `mail_bags.json`. **Rules:** calls are
scheduled and logged; silence has a threshold and a response; reports are short
and honest; mail is carried, not transmitted.

### 7.6 `OutpostReturnSystem` (new, thin, `Ashfall.Core.Expeditions`)

**Owns:** rotation, recall thresholds, evacuation, handover, closure, and the
home-herd room. **Consumes:** `ColonySystem` (`IsActive`, population,
transfer), `ExpeditionSystem`, records owners. **Data:** `recall_plans.json`.
**Rules:** every outpost has a written recall threshold; recall is planned and
dignified; closure records what was built and why it ended; nobody is left.

### 7.7 `OutpostRelationsSystem` (new, thin, `Ashfall.Core.Expeditions`)

**Owns:** neighbor contact at the outpost scale, shared work, and boundary
agreements. **Consumes:** 45's diplomacy owners, `PatrolTerritoryAuthority`
(Wave 5), `settlements.json`. **Data:** `outpost_relations.json`.
**Rules:** neighbors are met at lines and invited to shared work; agreements
are written; disputes route to the live territory and diplomacy owners.

### 7.8 Systems explicitly not added

- No second colony, supply, route, defense, or comms system.
- No colonial or conquest framing.
- No forced relocation or punishment postings.
- No new currency.
- No new RNG stream.
- No new save section.

---

## 8. DATA CATALOG SPECIFICATION

All catalogs snake_case, integer `schema_version: 1`, validated and scanner-registered.

### 8.1 `outpost_charters.json` (new)

```json
{
  "schema_version": 1,
  "charters": [
    {
      "charter_id": "charter_valley_farm",
      "display_name": "Valley Farm Outpost",
      "purpose": "food",
      "population_target": 12,
      "limits": ["no mining", "no permanent wall"],
      "recall_threshold": "two failed relief seasons",
      "review_days": 180,
      "tags": ["farm", "valley"]
    }
  ]
}
```

### 8.2 `outpost_sites.json` (new)

Sites: water, route, ground, exposure, neighbors, build ground, hazards.

### 8.3 `outpost_roles.json` (new)

Roles: leader, builder, grower, medic, radio, scout, cook, teacher.

### 8.4 `supply_lines.json` (new)

Lines: origin, destination, capacity, flow, schedule, status, losses.

### 8.5 `outpost_depots.json` (new)

Depots: location, cache, capacity, condition, keeper.

### 8.6 `outpost_life.json` (new)

Life: housing, food, routines, morale inputs, skills, local rules.

### 8.7 `outpost_defense.json` (new)

Defense: fence, watch, drills, alarm plan, neighbor help.

### 8.8 `outpost_reports.json` (new)

Reports: week, weather, work, supply, health, needs, next.

### 8.9 `mail_bags.json` (new)

Mail: bag, courier, letters, parcels, date, delivery.

### 8.10 `recall_plans.json` (new)

Recall: threshold, assembly point, transport, records, closure rites.

### 8.11 `outpost_relations.json` (new)

Relations: neighbor, boundary, agreement, shared work, review.

### 8.12 Items

New items appended to `items.json`: `item_outpost_flag`, `item_charter_scroll`,
`item_field_radio`, `item_supply_crate`, `item_depot_cache_box`,
`item_water_barrel_outpost`, `item_fence_kit`, `item_report_ledger`,
`item_mail_bag`, `item_road_marker`, `item_field_tent`, `item_field_stove`,
`item_seed_box`, `item_medicine_chest`, `item_outpost_tool_kit`,
`item_return_crate`.

---

## 9. SAVE, DETERMINISM, AND PERSISTENCE

### 9.1 Ownership

`ColonyState` and expedition state remain the live save owners. New sub-objects
(charters, sites, lines, depots, life, defense, reports, mail, recall,
relations) are additive inside them. No new save section.

### 9.2 State to persist

- Charters, purposes, limits, and review dates.
- Site survey results and founding parties.
- Supply lines, schedules, and losses.
- Depot caches and condition.
- Outpost housing, food, routines, and morale inputs.
- Fences, drills, and alarm plans.
- Call windows, codes, reports, and silence state.
- Mail in transit and delivered.
- Rotation, recall thresholds, and closure records.
- Neighbor agreements.

### 9.3 Determinism

- Colony and supply state remain in the live system; `TickDay`, transfer, and
  status changes are the only writers.
- Convoy outcomes derive from route state, weather, load, and escort.
- Remote needs resolve through the live needs owners.
- Radio windows and reports resolve by day and hour, never wall-clock.
- Paired replay hashes must match; no `System.Random`.

### 9.4 Migration

Legacy saves load with colonies, supply lines, and populations untouched; no
charter, depot, report, or recall state exists until started. Existing outposts
gain a default charter and a written recall threshold when first opened.

### 9.5 Checksum

Invariant-culture floats; integer day, load, and count fields.

---

## 10. UI, ACCESSIBILITY, AND PRESENTATION

### 10.1 Surfaces

| Surface | Purpose | Owner |
|---|---|---|
| `CharterPanel` (new) | Purpose, limits, review | `OutpostHostSession` |
| `SitePanel` (new) | Surveys and founding | same |
| `SupplyPanel` (new) | Lines, depots, convoys | same |
| `OutpostLifePanel` (new) | Housing, food, routines | same |
| `DefensePanel` (new) | Fence, watch, drills | same |
| `RadioWindowPanel` (new) | Calls, codes, silence | same |
| `RotationPanel` (new) | Rotation and recall | same |
| `MailPanel` (new) | Bags and delivery | same |

### 10.2 Accessibility and honesty

- Panels show live state and expose existing commands only.
- Supply is shown as a schedule with honest stock, never a hidden number.
- Silence has a countdown with a written threshold, not a mood.
- Maps and routes have text descriptions; nothing depends on color alone.
- Keyboard/controller close/back preserved; focus maintained on refresh.
- Reports can be read aloud; mail has text forms.

### 10.3 Presentation

Audio cues appended to `audio_cues.json`: a radio warming up, a call sign, a
gate latch, a crate set down, a convoy engine in the distance, a kettle at the
depot. No cue is required; text carries meaning.

---

## 11. INTEGRATION SEAMS

| Existing system | Attachment |
|---|---|
| `ColonySystem` | Outposts, supply lines, population |
| `ColonyOutpost` | Ratings, supplies, population |
| `SupplyLine` | Capacity, flow, status |
| `ExpeditionSystem` | Founding parties and camps |
| `ArmoredCrawlerExpeditionSystem` | Heavy transport |
| `RouteInfrastructureSystem` (Wave 5) | Route condition |
| `TravelingCaravanSystem` | Convoy runs |
| `Radio` owners (Wave 5) | Calls and codes |
| `WatchHouseHostSession` (Wave 5) | Defense and drills |
| `PatrolTerritoryAuthority` (Wave 5) | Boundary lines |
| `DiplomaticSummitSystem` (Wave 7) | Neighbor agreements |
| `InquiryHostSession` (Wave 7) | Site surveys |
| `ReactorHostSession` (Wave 7) | Remote power planning |
| `WorldChange` (Wave 7) | Site change over time |
| `ShelterAssignmentSystem` | Site housing |
| `NeedsSystem` | Remote condition and morale |
| `StandingRecord` (Exp 03) | Charters and records |
| `EpilogueChronicleBuilder` | Outpost milestones |

---

## 12. TECHNICAL IMPLEMENTATION PLAN

### 12.1 Phase order

**Phase 0 — Premise re-audit.** Confirm colony, expedition, supply, road, radio,
watch, needs, and record owners. Record file:line; change nothing.

**Phase 1 — Data + validators.** Author the eleven catalogs; append items;
register validators and scanner.

**Phase 2 — Pure Core.** `OutpostCharterSystem`, `OutpostLogisticsSystem`,
`OutpostLifeSystem`, `OutpostDefenseSystem`, `OutpostCommsSystem`,
`OutpostReturnSystem`, `OutpostRelationsSystem`.

**Phase 3 — Persistence.** Additive sub-objects, migration, round-trip,
determinism.

**Phase 4 — Host + CLI.** `OutpostHostSession`, selftest coverage, fresh journey
from crowding to the valley site.

**Phase 5 — UI.** New panels with lifecycle and accessibility.

**Phase 6 — Content.** Rooms, locations, NPCs, quests, items, prose, audio.

**Phase 7 — Balance.** 360-day soak: founding winter, silent radio, relief
decision, rotation, and threshold test.

**Phase 8 — Verification and closeout.**

### 12.2 Content volume

| Content | Count |
|---|---|
| Charters | 10 |
| Sites | 12 |
| Roles | 10 |
| Supply lines | 10 |
| Depots | 8 |
| Life plans | 12 |
| Defense plans | 10 |
| Reports | 24 |
| Mail bags | 12 |
| Recall plans | 8 |
| Relations | 10 |
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
| Colonial framing | Critical | Community contract §3.3 |
| Second colony system | Critical | Extend live owner |
| Playout autonomy | High | People-driven design |
| Defense spectacle | High | Watch and agreement first |
| Remote needs drift | High | Live needs owners only |
| Forgotten return | High | Written recall threshold |
| Determinism break | Low | Live seeded paths |
| Content overrun | Medium | Budget §13 |

---

## 13. APPENDIX A — AUTHORING BUDGET

| File | Rows | Est. words |
|---|---|---|
| `outpost_charters.json` | 10 | 2,500 |
| `outpost_sites.json` | 12 | 3,000 |
| `outpost_roles.json` | 10 | 2,000 |
| `supply_lines.json` | 10 | 2,500 |
| `outpost_depots.json` | 8 | 2,000 |
| `outpost_life.json` | 12 | 3,000 |
| `outpost_defense.json` | 10 | 2,500 |
| `outpost_reports.json` | 24 | 4,500 |
| `mail_bags.json` | 12 | 2,500 |
| `recall_plans.json` | 8 | 2,000 |
| `outpost_relations.json` | 10 | 2,500 |
| Quest objectives | 53 quests | 15,000 |
| NPC prose | 8 NPCs | 7,000 |
| Location prose | 10 | 3,500 |
| Item descriptions | 16 | 2,500 |
| Ending prose | 6 | 3,000 |
| **Total** | | **~63,000** |

---

## 14. APPENDIX B — RISK REGISTER

| ID | Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|---|
| R44-1 | Colonial framing | Low | Critical | Community contract |
| R44-2 | Second colony system | Low | Critical | Live owner |
| R44-3 | Autonomous play | Med | High | People-driven |
| R44-4 | Defense spectacle | Med | High | Watch-first rule |
| R44-5 | Remote needs fork | Med | High | Live needs owners |
| R44-6 | No recall plan | Med | High | Written threshold |
| R44-7 | Supply as decoration | Med | Med | Real stock |
| R44-8 | Determinism | Low | High | Live paths |
| R44-9 | Content overrun | Med | Med | Budget §13 |
| R44-10 | Road/comms overlap | Med | Med | Boundary §0.1 |

---

## 15. APPENDIX C — OPEN DECISIONS REQUIRING A FOREMAN SIGNATURE

1. **How many outposts can exist at once?** Recommended: two or three, with the
   second only after the first survives a year.
2. **Can an outpost be abandoned without recall?** Recommended: no; closure
   always moves people home or to a named alternative.
3. **Is remote power independent or grid-tied?** Recommended: independent
   micro-source with a request path to 42, never a second generation owner.
4. **Who commands defense?** Recommended: the outpost leader under the watch
   owner's standards.
5. **Does service at an outpost earn standing?** Recommended: yes, recorded in
   03's standing record, never as currency.

---

## 17. APPENDIX D — CHARTER TABLE (10 CHARTERS)

| # | Charter | Purpose | Target | Limits | Recall threshold |
|---|---|---|---|---|---|
| 1 | Valley farm | food | 12 | no mining, no wall | two failed relief seasons |
| 2 | River flat | water | 8 | no damming | water fails two seasons |
| 3 | Old homestead | reoccupation | 6 | preserve the house | roof fails in winter |
| 4 | Ridge relay | comms | 4 | no farming | relay down 30 days |
| 5 | Winter track | way-station | 6 | no permanent fields | road closed 60 days |
| 6 | Quarry spur | stone | 10 | no deep pit | air unsafe |
| 7 | Mill camp | timber | 8 | no clear-cut | timber exhausted |
| 8 | Neighbor line | shared work | 6 | no claims | agreement ends |
| 9 | Germ seed | seed bank | 5 | no animals | storage fails |
| 10 | Second hearth | full settlement | 20 | any, under review | two thresholds met |

A charter is a promise about purpose and a brake on ambition. The limits column
is what keeps a useful outpost from quietly becoming something the community
never agreed to, and the threshold column is how it can come home with honor.

---

## 18. APPENDIX E — SITE TABLE (12 SITES)

| # | Site | Water | Route | Ground | Neighbors | Score |
|---|---|---|---|---|---|---|
| 1 | Valley site | stream | road | firm | none near | best |
| 2 | River flat | river | track | soft | fishers | good |
| 3 | Old homestead | well | road | firm | none | good |
| 4 | Ridge relay | spring | path | rock | none | comms |
| 5 | Winter track | snow | road | mixed | none | seasonal |
| 6 | Quarry spur | dry | road | stone | miners | hazard |
| 7 | Mill camp | stream | path | firm | none | timber |
| 8 | Neighbor line | shared | road | firm | close | diplom |
| 9 | Germ seed | well | road | firm | none | careful |
| 10 | South gap | dry | trail | loose | passing | poor |
| 11 | Ash flat | none | none | ash | none | rejected |
| 12 | Deep vale | spring | tunnel | firm | none | dark |

Site selection is the expansion's first real decision, and the table gives it
twelve honest candidates with flaws. The rejected rows matter as much as the
best one: a shelter learns from the ground it decides not to occupy.

---

## 19. APPENDIX F — OUTPOST ROLE TABLE

| Role | Needed for | Minimum | Notes |
|---|---|---|---|
| Leader | all | 1 | charter keeper |
| Builder | founding | 1 | winter proofs |
| Grower | food | 1 | soil and seasons |
| Medic | all | 1 | cached medicine |
| Radio | all | 1 | call window |
| Scout | routes | 1 | weather reading |
| Cook | life | 1 | morale |
| Teacher | children | 0–1 | letters and lessons |
| Watch lead | defense | 1 | drills |
| Store keeper | supply | 1 | depot honesty |

Ten roles, most of them one person doing several. The table is the staffing
plan the leader argues about, and the teacher row is deliberately optional
because the shelter sends letters and lessons only when there are children to
include.

---

## 20. APPENDIX G — SUPPLY LINE TABLE

| # | Line | Capacity | Flow | Schedule | Risk |
|---|---|---|---|---|---|
| 1 | Main to valley | 100 | 10 | weekly | weather |
| 2 | Main to river | 80 | 8 | weekly | soft ground |
| 3 | Main to homestead | 60 | 6 | fortnight | road |
| 4 | Main to relay | 40 | 4 | monthly | climb |
| 5 | Main to winter track | 80 | 8 | seasonal | snow |
| 6 | Main to quarry | 120 | 12 | fortnight | load |
| 7 | Main to mill | 100 | 10 | fortnight | path |
| 8 | Neighbor shared | 60 | 6 | monthly | agreement |
| 9 | Seed run | 30 | 3 | rare | care |
| 10 | Emergency only | 100 | burst | on event | risk |

Supply lines are the outpost's pulse. The emergency-only row is the last
resort that must be planned for and never used casually, and the risk column is
the reason the depot exists at all.

---

## 21. APPENDIX H — DEPOT TABLE

| # | Depot | Holds | Capacity | Keeper | Check |
|---|---|---|---|---|---|
| 1 | Halfway | food, fuel | 80 | Dero | weekly |
| 2 | Ridge cache | batteries | 30 | Miri | monthly |
| 3 | River cache | rope, tools | 40 | Yara | monthly |
| 4 | Winter cache | fuel, blankets | 60 | Bron | season |
| 5 | Quarry cache | parts | 50 | keeper | weekly |
| 6 | Neighbor cache | gifts, salt | 30 | Lef | monthly |
| 7 | Return cache | food, water | 60 | crew | prime |
| 8 | Emergency box | medical | 20 | Ikra | monthly |

Depots are small promises left along a road. The return cache is primed even
when the outpost is thriving, because the day a person needs it is not the day
to discover it was emptied for convenience.

---

## 22. APPENDIX I — OUTPOST LIFE TABLE

| # | Aspect | Standard | Check | Fix |
|---|---|---|---|---|
| 1 | Housing | wind-tight | winter | add felt |
| 2 | Food | three weeks | weekly | supply |
| 3 | Water | daily clean | daily | boil, filter |
| 4 | Rest | room dark | monthly | curtain |
| 5 | Heat | stove working | daily | fuel |
| 6 | Hygiene | wash line | weekly | soap |
| 7 | Mail | weekly | weekly | courier |
| 8 | Work | fair rota | weekly | review |
| 9 | Company | one shared meal | daily | table |
| 10 | Rest day | one per week | weekly | rota |
| 11 | Letters home | monthly | monthly | bag |
| 12 | Celebration | seasonal | season | plan |

Remote life is ordinary life with higher stakes, and the table keeps the
standards the same as home. The shared meal and rest-day rows are the
small things that keep a small place livable through a long winter.

---

## 23. APPENDIX J — DEFENSE PLAN TABLE

| # | Layer | Content | Drill | Fallback |
|---|---|---|---|---|
| 1 | Fence | posts, wire | walk | gate |
| 2 | Gate | heavy bar | close | watch post |
| 3 | Watch | two posts | drill | retreat |
| 4 | Lighting | lamps | check | dark |
| 5 | Alarm | bell, radio | drill | runner |
| 6 | Radio call | code | drill | relay |
| 7 | Neighbor help | agreement | joint drill | wait |
| 8 | Withdraw | plan | walk | main gate |
| 9 | Supplies | cache | count | return cache |
| 10 | Medicine | chest | check | ward |

Defense is layered, boring, and mostly about knowing when to withdraw. The last
three rows are the shelter's values written as tactics: prepare to leave, keep
people fed, and keep people well.

---

## 24. APPENDIX K — OUTPOST REPORT TABLE (24 REPORTS — REPRESENTATIVE)

| # | Week | Weather | Work | Supply | Health | Need |
|---|---|---|---|---|---|---|
| 1 | Week one | clear | footings | full | fine | nails |
| 2 | Week two | rain | frame | full | fine | boards |
| 3 | Week three | rain | roof | three-quarter | one cold | felt |
| 4 | Week four | clear | fence | full | fine | wire |
| 5 | Week five | wind | well | full | fine | rope |
| 6 | Week six | clear | fields | half | fine | seed |
| 7 | Week seven | hot | irrigation | full | fine | shade |
| 8 | Week eight | storm | repair | low | one injury | splints |
| 9 | Week nine | clear | harvest | full | fine | sacks |
| 10 | Week ten | rain | store | full | fine | lime |
| 11 | Week eleven | frost | winterize | three-quarter | fine | fuel |
| 12 | Week twelve | snow | inside work | half | fine | tea |
| 13 | Week thirteen | snow | shovelling | half | fine | fuel |
| 14 | Week fourteen | snow | radio fix | low | fine | batteries |
| 15 | Week fifteen | clear | ice work | low | one cold | medicine |
| 16 | Week sixteen | clear | road cut | half | fine | food |
| 17 | Week seventeen | rain | mud work | full | fine | boots |
| 18 | Week eighteen | clear | fencing | full | fine | wire |
| 19 | Week nineteen | wind | watch | full | fine | lamps |
| 20 | Week twenty | clear | planting | full | fine | seed |
| 21 | Week twenty-one | rain | repairs | full | fine | timber |
| 22 | Week twenty-two | clear | fields | full | fine | hands |
| 23 | Week twenty-three | clear | harvest | full | fine | crates |
| 24 | Week twenty-four | snow | review | full | fine | review |

Twenty-four weeks of honest reports, most of them ordinary. The table is the
expansion's texture: a place is built out of weeks like these, and the two
hard weeks are only survivable because the other twenty-two were boring.

---

## 25. APPENDIX L — MAIL AND COURIER TABLE

| # | Bag | Contents | Courier | Frequency | Route |
|---|---|---|---|---|---|
| 1 | Home bag | letters | runner | weekly | road |
| 2 | Outpost bag | letters | runner | weekly | road |
| 3 | Parcel bag | tools, seed | convoy | monthly | road |
| 4 | Medicine bag | ordered items | convoy | monthly | road |
| 5 | Seed bag | samples | scout | season | trail |
| 6 | Record bag | reports | runner | monthly | road |
| 7 | Gift bag | neighbor gifts | Lef | monthly | line |
| 8 | Winter bag | mail held | rations | seasonal | snow |
| 9 | Urgent bag | priority | runner | on event | fastest |
| 10 | Return bag | last mail | recall crew | closure | road |
| 11 | Archive bag | records | courier | yearly | road |
| 12 | Family bag | photographs | runner | rare | road |

Mail is the cheapest thing on the table and the one people remember longest.
The return bag is the saddest and most important: when a place closes, its
letters go home with its people.

---

## 26. APPENDIX M — RECALL PLAN TABLE

| # | Plan | Threshold | Assembly | Transport | Record |
|---|---|---|---|---|---|
| 1 | Farm recall | two failed reliefs | gate | convoys | farm log |
| 2 | River recall | water fails | flat | boats, carts | water log |
| 3 | Homestead recall | roof fails | yard | carts | house log |
| 4 | Relay recall | relay dead | tower | packs | signal log |
| 5 | Track recall | road closed 60 | shelter | crawler | road log |
| 6 | Quarry recall | air unsafe | spur | carts | air log |
| 7 | Mill recall | timber gone | camp | carts | timber log |
| 8 | Second hearth | two thresholds | square | full move | charter |

Eight recall plans, each written before it is needed. The second-hearth row is
the one the shelter hopes never to use and keeps updated anyway, because a
place that grows into a town should know how to become a village again.

---

## 27. APPENDIX N — RELATIONS TABLE

| # | Neighbor | Boundary | Agreement | Shared work | Review |
|---|---|---|---|---|---|
| 1 | River fishers | line at bend | fish, no claim | shared watch | yearly |
| 2 | Homestead family | fence line | quiet, no claim | tool lending | yearly |
| 3 | Quarry crew | spur | stone for food | haulage | season |
| 4 | Passing traders | gate only | road use | markets | event |
| 5 | Ridge shepherds | path | grazing, no claim | weather sharing | yearly |
| 6 | Neighbor line | painted stones | shared patrol | joint drill | season |
| 7 | Old families | valley mouth | respect, no claim | letters | yearly |
| 8 | Wandering parties | boundary | entry by charter | trade | event |
| 9 | Forest camp | cut line | no clear-cut | fire watch | season |
| 10 | Deep vale | none | leave alone | none | yearly |

Neighbors are met at lines and agreements, not at bayonets. The last row is
included for a place the shelter chooses not to visit at all, which is also a
relation and sometimes the wisest one.

---

## 28. APPENDIX O — MAIN QUESTLINE STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_outpost_crowding` | 4 | Room needed |
| `quest_outpost_site` | 5 | Valley surveyed |
| `quest_outpost_charter` | 4 | Charter written |
| `quest_outpost_first_party` | 4 | Party chosen |
| `quest_outpost_build` | 5 | First build up |
| `quest_outpost_depot` | 4 | Halfway depot |
| `quest_outpost_first_call` | 3 | Radio window |
| `quest_outpost_neighbors` | 4 | Line meeting |
| `quest_outpost_winter` | 5 | Winter alone |
| `quest_outpost_silence` | 5 | Radio fails |
| `quest_outpost_relief` | 5 | Relief decided |
| `quest_outpost_rota` | 4 | Rotation begins |
| `quest_outpost_second_year` | 4 | Place, not project |
| `quest_outpost_threshold` | 4 | Threshold tested |
| `quest_outpost_valley_site` | 3 | Final disposition |

---

## 29. APPENDIX P — SIDE QUEST STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_outpost_survey` | 4 | Site walked |
| `quest_outpost_water_site` | 3 | Water tested |
| `quest_outpost_ground` | 3 | Ground tested |
| `quest_outpost_kit_load` | 3 | Kits loaded |
| `quest_outpost_raise_flag` | 2 | Mark raised |
| `quest_outpost_line` | 4 | Line established |
| `quest_outpost_convoy` | 4 | Convoy run |
| `quest_outpost_depot_cache` | 3 | Cache laid |
| `quest_outpost_fuel_cache` | 3 | Fuel stored |
| `quest_outpost_loss` | 4 | Loss handled |
| `quest_outpost_house` | 4 | Housing built |
| `quest_outpost_food` | 3 | Food secured |
| `quest_outpost_rest` | 3 | Routine set |
| `quest_outpost_school` | 3 | Letters and lessons |
| `quest_outpost_festival` | 3 | Celebration held |
| `quest_outpost_fence` | 4 | Fence built |
| `quest_outpost_watch_post` | 3 | Post staffed |
| `quest_outpost_drill` | 4 | Drill held |
| `quest_outpost_neighbor_guard` | 3 | Shared watch |
| `quest_outpost_radio_alarm` | 3 | Alarm plan |
| `quest_outpost_call_window` | 3 | Window set |
| `quest_outpost_code` | 3 | Code written |
| `quest_outpost_mail` | 3 | Mail carried |
| `quest_outpost_report` | 3 | Report filed |
| `quest_outpost_silence_rule` | 3 | Silence protocol |
| `quest_outpost_rotation` | 4 | Rotation written |
| `quest_outpost_threshold_write` | 3 | Threshold written |
| `quest_outpost_pack` | 3 | Return packed |
| `quest_outpost_handover_outpost` | 4 | Handover done |
| `quest_outpost_home_herd` | 3 | Return room |

---

## 30. APPENDIX Q — NPC DOSSIERS (BRIEF)

**Halla Ryn** — outpost leader. Volunteers for hard places and writes the
reason down before leaving. Believes a charter is a promise to the people who
grow up in a place.

**Dero** — supply captain. Runs convoys like a metronome and hates wasted
weight. Believes an outpost is exactly as alive as its line.

**Miri** — wireless operator. Reads static like weather and keeps the call
window sacred. Believes a voice once a night is worth more than a wall.

**Ikra** — outpost medic. Caches medicine before food and writes thresholds
without sentiment. Believes caring for six people is the same craft as caring
for sixty.

**Bron** — builder. Tests ground, roof, and frost before winter and takes no
satisfaction in hurried work. Believes a building is a hypothesis about weather.

**Yara** — scout. Walks the route in bad weather so the good weather can be
trusted. Believes a road is only as good as its worst day.

**Lef** — trader. Meets neighbors at the line with salt and questions. Believes
agreements are cheaper than fences and harder to build.

**Lia** — family. Keeps the return room warm and the letters sorted. Believes
at home that distance is a detail and not a separation.

---

## 31. APPENDIX R — LOCATION DETAIL

- **The Valley Site** — water, ground, and a view that makes people argue for
  the wrong reasons.
- **The Junction** — where supply divides and the schedule is kept.
- **The Halfway Depot** — a cache that has saved more lives than any weapon.
- **The Outpost Gate** — a bar, a bell, and a list of names.
- **The Ridge Relay** — wind, batteries, and the call that comes at nine.
- **The River Flat** — fertile and soft, a promise with a caveat.
- **The Old Homestead** — someone else's house, treated with respect.
- **The Neighbor Line** — painted stones and a conversation.
- **The Winter Track** — the road that teaches the shelter patience.
- **The Recall Point** — where everyone is counted before anyone goes home.

---

## 32. APPENDIX S — RELIEF DECISION PROTOCOL

| Step | Action | Owner |
|---|---|---|
| Silence | note the missed call | Miri |
| Wait | one window of grace | leader |
| Check | try the ridge relay | Miri |
| Assess | weather and road state | Yara |
| Decide | meet the written threshold | steward |
| Prepare | load the relief cache | Dero |
| Go | send the smallest safe party | leader |
| Arrive | assist, do not blame | all |
| Report | record the real cause | scribe |
| Review | change one plan | all |

The relief protocol is the expansion's central drama handled as procedure. The
threshold is met or not, the decision is named, and the party that goes is the
smallest safe one — and when they arrive, their first job is help rather than
judgment.

---

## 33. APPENDIX T — WORKED 720-DAY OUTPOST SCENARIO

**Days 1–30.** Crowding discussed; the valley site surveyed; water and ground
tested; the charter written with limits and a threshold.

**Days 31–60.** The first party of eight leaves; the half-built site gets a
shelter, a well, and a fence corner; the halfway depot is stocked.

**Days 61–90.** The first radio call at nine; call signs chosen; mail bags
begin their weekly run; a neighbor family is met at the river bend.

**Days 91–150.** Fields cleared; the first crops planted; housing improved;
the shared meal becomes a habit; a doctor visits monthly.

**Days 151–210.** Summer: irrigation, harvest, and a second convoy schedule;
the outpost's store reaches a full winter; the fence is completed.

**Days 211–260.** First frost; the road degrades; convoys slow; the winter
cache is primed; a watch drill is held with the neighbor line.

**Days 261–320.** Deep winter: the road closes for three weeks; the radio
fails on the fourth day of the closure; the shelter works the relief protocol
and decides to wait one more window.

**Days 321–340.** The relay is repaired; the call comes through; the outpost is
cold but healthy; the shelter records the near-miss and adds a second relay.

**Days 341–420.** Spring relief and rotation: half the first party comes home;
new people go out; Lia's return room hosts a welcome dinner that is quiet and
long.

**Days 421–540.** The valley becomes a place: a second field, a workshop
corner, a school letter to the main shelter, and a written agreement with the
fishers that outlasts the season.

**Days 541–620.** Harvest is better than the main shelter's for the first time;
the outpost asks for a millstone and a loom, and both are delivered on the
winter road in good time.

**Days 621–700.** A threshold is tested: water quality drops after a flood;
the charter says two failed seasons; the outpost fixes the well and survives at
one; the threshold remains written and unused.

**Days 701–720.** Review year two: the charter is re-read aloud; the outpost
stays; two new families volunteer; the valley lamp is listed in the main
shelter's log as a place, not a project.

---

## 34. APPENDIX U — VIGNETTES (TONE SAMPLE)

> Halla reads the charter out loud on the first night at the site, and the last
> line is the recall threshold, and the reading of it is not morbid, it is a
> promise that nobody here is meant to be stranded.

> Miri turns the dial and gets static and then gets the main shelter, and the
> voice says the weather and the count, and she writes it in the log, and eight
> people in a valley feel less alone than they did a minute ago.

> Bron digs a footing and finds old ash under the clay and sets the corner one
> step to the side without being asked, and the new foundation is better than
> the plan, and he writes the reason in the margin.

> Dero counts the crates at the halfway depot, and one is missing, and he does
> not say anything to anyone until he has counted twice, and then he writes the
> loss honestly and schedules a smaller load, because the road is the road.

---

## 35. APPENDIX V — FAILURE MODES AND RECOVERY

| Failure | Effect | Recovery |
|---|---|---|
| Bad site | hard founding | resite, record why |
| Supply loss | hunger risk | depot, relief, ration |
| Radio failure | silence | relay, runner |
| Winter close | isolation | cache, wait, relief |
| Injury | strain | medic, rotate |
| Morale drop | friction | mail, meal, rest |
| Neighbor dispute | boundary tension | meeting, agreement |
| Build failure | exposure | rebuild, proof |
| Threshold met | recall decision | protocol, dignity |
| Outpost abandoned | grief | records, return, memory |

Every failure has a practiced answer and every answer ends with the same
obligation: the people come home, and the records come with them. The
abandonment row is included because the expansion believes a closed place
deserves a proper ending.

---

## 36. APPENDIX W — CONTENT REVIEW CHECKLIST

- [ ] No colonial framing or conquest content exists.
- [ ] No forced relocation or punishment postings exist.
- [ ] `ColonySystem` remains the colony and supply authority.
- [ ] Routes stay with the road owner; radio with comms; defense with watch.
- [ ] Remote needs use the live needs owners only.
- [ ] Every outpost has a written recall threshold.
- [ ] Neighbors are met with agreements, not arms.
- [ ] Closure moves people and records home.
- [ ] Save additions are additive and legacy-neutral.
- [ ] Determinism uses live seeded paths only.
- [ ] Outpost service is voluntary and recorded.

---

## 37. APPENDIX X — GLOSSARY

- **Charter** — the written purpose, size, limits, and threshold of an outpost.
- **Line** — the supply route with capacity and flow.
- **Depot** — a stocked cache along a route.
- **Relief** — the scheduled or emergency run that keeps a place alive.
- **Window** — the scheduled radio call time.
- **Threshold** — the written condition that triggers recall.
- **Rotation** — the planned replacement of people over time.
- **Home herd** — the people and room waiting at home.
- **Handover** — the record and walk-through between outpost keepers.
- **Recall** — bringing everyone home, with dignity and records.

---

## 38. APPENDIX Y — INTEGRATION MATRIX

| System | Reads | Writes | Never writes |
|---|---|---|---|
| `ColonySystem` | colonies | colony state | charters |
| `OutpostCharterSystem` | surveys | charters | colony state |
| `OutpostLogisticsSystem` | routes | lines, depots | route state |
| `OutpostLifeSystem` | needs | life plans | needs state |
| `OutpostDefenseSystem` | watch | plans, drills | patrol state |
| `OutpostCommsSystem` | radio | reports, mail | signal state |
| `OutpostReturnSystem` | colony | recall plans | colony state |
| `OutpostRelationsSystem` | neighbors | agreements | diplomacy |
| `RouteInfrastructureSystem` | roads | nothing | nothing |
| `ExpeditionSystem` | parties | party state | colony state |
| `WatchHouseHostSession` | drills | nothing | nothing |
| `DiplomaticSummitSystem` | agreements | nothing | nothing |
| `NeedsSystem` | morale | needs | life plans |
| `StandingRecord` | charters | records | nothing |
| `EpilogueChronicleBuilder` | milestones | chronicle | colony state |

---

## 39. APPENDIX Z — DATA SCHEMA DETAIL (NEW CATALOGS)

**`outpost_charters.json`** — `charter_id`, `display_name`, `purpose`,
`population_target`, `limits[]`, `recall_threshold`, `review_days`, `tags`.

**`outpost_sites.json`** — `site_id`, `display_name`, `water`, `route`,
`ground`, `neighbors`, `build_ground`, `hazards[]`, `tags`.

**`outpost_roles.json`** — `role_id`, `display_name`, `needed_for`, `minimum`,
`notes`, `tags`.

**`supply_lines.json`** — `line_id`, `origin`, `destination`, `capacity`,
`flow`, `schedule`, `status`, `tags`.

**`outpost_depots.json`** — `depot_id`, `display_name`, `holds[]`, `capacity`,
`keeper`, `check`, `tags`.

**`outpost_life.json`** — `aspect_id`, `display_name`, `standard`, `check`,
`fix`, `tags`.

**`outpost_defense.json`** — `layer_id`, `display_name`, `content`,
`drill`, `fallback`, `tags`.

**`outpost_reports.json`** — `report_id`, `week`, `weather`, `work`, `supply`,
`health`, `need`, `tags`.

**`mail_bags.json`** — `bag_id`, `contents[]`, `courier`, `frequency`,
`route`, `tags`.

**`recall_plans.json`** — `plan_id`, `threshold`, `assembly`, `transport`,
`record`, `tags`.

**`outpost_relations.json`** — `relation_id`, `neighbor`, `boundary`,
`agreement`, `shared_work`, `review`, `tags`.

All new catalogs carry `schema_version: 1` and fail the integrity gate on
missing or duplicate IDs, invalid references, or out-of-range numbers.

---

## 40. APPENDIX AA — MEASUREMENT AND TELEMETRY

| Metric | Purpose | Source |
|---|---|---|
| Line uptime | supply health | Logistics |
| Depot stock | readiness | Depots |
| Radio windows kept | comms discipline | Comms |
| Reports filed | honesty | Reports |
| Mail delivered | connection | Mail |
| Rotation completed | fairness | Return |
| Recall thresholds met | safety | Recall |
| Neighbor agreements | peace | Relations |
| Drills held | defense | Defense |
| Outpost morale | life quality | Life |

Telemetry is diagnostic only; it never gates content and never becomes a score
against a remote community.

---

## 41. APPENDIX AB — IMPLEMENTATION CHECKLIST

- [ ] Phase 0 premise re-audit recorded with file:line evidence.
- [ ] Phase 1 catalogs authored and registered with validators and scanner.
- [ ] Phase 2 Core systems are pure, engine-free, and deterministic.
- [ ] Phase 3 save additions are additive, legacy-neutral, and round-trip tested.
- [ ] Colony, supply, route, radio, watch, and needs authorities remain untouched.
- [ ] Triad parity (Setup / Save / Flush) holds for every new state.
- [ ] Phase 4 host session exposes live state and real commands only.
- [ ] Phase 5 panels pass bind/unbind/rebind, focus, contrast, and scaling checks.
- [ ] Phase 6 content passes the review checklist in §36.
- [ ] Phase 7 soak shows a founding winter, a radio silence, and a rotation.
- [ ] Phase 8 data integrity and content utilization selftests pass.
- [ ] No colonial, conquest, or forced-relocation content exists.

---

## 42. APPENDIX AC — OPEN QUESTIONS FOR REVIEW

1. Can an outpost found a second outpost of its own?
2. Do outposts count toward the shelter's population record?
3. Should neighbor agreements ever include defense obligations?
4. Who inherits an outpost leader's authority in a crisis?
5. Are outposts represented in the charter's review, or audited separately?
6. Does mail ever go astray, and how is that handled?
7. Can a recall be reversed after arrival home?
8. What does the shelter do with buildings it leaves behind?

None of these may be decided unilaterally; each changes tone and balance.

---

## 43. APPENDIX AD — CROSS-WAVE HOOKS

| Wave | Expansion | Hook |
|---|---|---|
| 1 | 12 The Second Generation | Families on rotation |
| 1 | 15 The Deep Root | Valley fields and seed |
| 2 | 18 The Underneath | Deep vale and tunnels |
| 2 | 21 The Grid | Remote micro-power |
| 3 | 22 The Clean Flow | Site water plan |
| 3 | 23 The Alarm | Outpost drills |
| 3 | 25 The Iron Road | Rail spur planning |
| 4 | 27 The Thread | Field clothing |
| 4 | 28 The Lesson | Letters and lessons |
| 4 | 31 The Kiln | Site construction |
| 5 | 32 The Wild | Site wildlife |
| 5 | 33 The Weather | Road and winter |
| 5 | 34 The Long Road | Convoys and routes |
| 5 | 36 The Watch | Shared defense |
| 6 | 37 The Quickening | Birth at the outpost |
| 6 | 38 The Ward | Remote medicine |
| 6 | 40 The Wheel | Millstone delivery |
| 7 | 42 The Core | Remote power requests |
| 7 | 43 The Question | Site surveys |
| 7 | 45 The Envoy | Neighbor agreements |
| 7 | 46 The Long Change | Site evolution |

Each hook is additive. The Outpost can ship alone, and every other expansion
can ship without it.

---

## 44. APPENDIX AE — ENDING PROSE SKETCHES

**The Second Hearth.** The valley becomes a real place with a charter, a
school letter, and a lamp that the main shelter lists by name.

**The Long Line Home.** The supply line and the nightly call make two
settlements one community, and neither is fully whole without the other.

**The Careful Frontier.** The outpost stays small and safe, and the shelter
learns that a modest place that endures is worth more than an ambitious one
that must be abandoned.

**The Winter That Held.** The hard season tests every plan and the plan holds,
because the plan was honest about the road and the weather.

**The Recall.** The outpost comes home with its people, its records, and its
name, and the valley is left clean, marked, and remembered.

**Fade.** A lamp in a valley window, a radio call at nine, and two settlements
saying goodnight.

---

## 45. APPENDIX AF — COMMON FAILURE PATTERNS TO AVOID

| Pattern | Why it is bad | Correct approach |
|---|---|---|
| Colonial framing | harmful | community contract |
| Frontier romance | empty | work and weather |
| Autonomous town | second game | people-driven |
| Supply decoration | hollow | real stock and loss |
| Defense spectacle | tone | watch and agreement |
| Forgotten people | cruel | rotation and recall |
| No threshold | unsafe | written recall |
| Erased closure | disrespect | records and rites |
| Neighbor as enemy | default hostility | agreements first |
| Distance as flavor | dishonest | cost and schedule |

The list exists because remote settlements are easy to write as either a
romance or a spreadsheet. The expansion's rule is that distance is a cost paid
by real people, and the community's job is to keep paying attention.

---

## 46. APPENDIX AG — CONTENT VOLUME SUMMARY

| Category | Rows | Prose estimate |
|---|---|---|
| Charters | 10 | 2,500 |
| Sites | 12 | 3,000 |
| Roles | 10 | 2,000 |
| Supply lines | 10 | 2,500 |
| Depots | 8 | 2,000 |
| Life plans | 12 | 3,000 |
| Defense plans | 10 | 2,500 |
| Reports | 24 | 4,500 |
| Mail bags | 12 | 2,500 |
| Recall plans | 8 | 2,000 |
| Relations | 10 | 2,500 |
| Quests | 53 | 15,000 |
| NPCs | 8 | 7,000 |
| Locations | 10 | 3,500 |
| Items | 16 | 2,500 |
| Endings | 6 | 3,000 |
| **Total** | | **~63,000** |

---

## 47. APPENDIX AH — FIRST YEAR OF THE OUTPOST

| Month | Focus | Milestone |
|---|---|---|
| 1 | Survey | site chosen |
| 2 | Charter | limits written |
| 3 | Party | eight leave |
| 4 | Build | shelter, well, fence |
| 5 | Depot | halfway stocked |
| 6 | Call | nightly window |
| 7 | Neighbors | line meeting |
| 8 | Fields | first planting |
| 9 | Harvest | first crop |
| 10 | Winterize | cache primed |
| 11 | Silence | radio and relief |
| 12 | Review | charter re-read |

A year of the outpost is a year of building a place out of weather, schedules,
and letters, and the shelter ends it knowing whether it can be trusted with
distance.

---

## 48. APPENDIX AI — COMMUNITY DISTANCE COVENANT

| Clause | Promise |
|---|---|
| Charter | Purpose and limits written first |
| Water | No place founded without clean water |
| Supply | A line with real capacity and a depot |
| Voice | A scheduled call and a silence protocol |
| Rotation | Nobody stays forever by accident |
| Threshold | The recall condition is written and honored |
| Neighbors | Agreements before arms |
| Records | Reports, mail, and logs come home |
| Closure | People and records leave together, never separately |
| Memory | A closed place keeps its name |

The covenant is the expansion's first-class design object. Distance is where a
community finds out whether it means what it says, and this table is what the
shelter says.

---

## 49. APPENDIX AJ — OUTPOST STAFFING TABLE

| Position | First year | Later | Rotation | Notes |
|---|---|---|---|---|
| Leader | Halla | second leader | 2 years | charter keeper |
| Builder | Bron | crew | 1 year | winter proof |
| Grower | Silla tie | 2 growers | 1 year | soil memory |
| Medic | Ikra | rotate | 6 months | cache discipline |
| Radio | Miri | trainee | 1 year | call window |
| Scout | Yara | pair | 1 year | route reading |
| Cook | volunteered | fixed | 1 year | morale |
| Watch lead | Boden tie | rotate | 1 year | drills |
| Store keeper | appointed | fixed | 2 years | depot honesty |
| Teacher | letters only | full | season | children |

Ten positions and a rotation plan, because an outpost that depends on one
irreplaceable person is a single accident away from becoming a problem. The
last row shows the shelter's answer to distance: at first letters, and later a
teacher when there are children to include.

---

## 50. APPENDIX AK — CONVOY LOAD TABLE

| Load | Weight | Frequency | Priority | Risk |
|---|---|---|---|---|
| Food | heavy | weekly | first | weather |
| Fuel | heavy | weekly | first | spill |
| Medicine | light | monthly | first | loss |
| Seed | light | seasonal | high | moisture |
| Tools | medium | monthly | medium | theft |
| Timber | heavy | seasonal | medium | load |
| Stone | heavy | seasonal | low | road |
| Mail | light | weekly | high | none |
| Gifts | light | event | high | protocol |
| People | varies | rotation | first | comfort |

Convoy loads are prioritized like this because the depot cannot hold everything
and the road cannot carry everything at once. The medicine and mail rows are
light on purpose: the cheapest things to carry are often the most important to
receive.

---

## 51. APPENDIX AL — OUTPOST WINTER PROOF TABLE

| Proof | Test | Pass | Failure |
|---|---|---|---|
| Wind tight | paper test | no flutter | felt gaps |
| Roof | snow load | no sag | brace |
| Stove | draft test | steady | clean flue |
| Water | freeze test | flows | insulate |
| Fuel | weeks count | enough | ration |
| Food | three weeks | enough | supply |
| Radio | cold start | works | warm box |
| Watch | cold drill | works | rotate |
| Medicine | frost check | stable | move |
| Return cache | count | full | restock |

Ten winter proofs, run in autumn before the weather makes them real. The return
cache row is the last check before the road closes, and it is never skipped,
because the cache is the difference between a hard winter and a tragedy.

---

## 52. APPENDIX AM — HOME HERD TABLE

| Family | Wait room | Letter cadence | Visit | Return plan |
|---|---|---|---|---|
| Ryn family | home herd | weekly | seasonal | rotation |
| Ferro tie | home herd | weekly | none | rotation |
| New couple | quarters | weekly | none | yearly |
| Children | school | letters | none | holidays |
| Old parents | home herd | weekly | none | yearly |
| Siblings | kitchen | weekly | none | rotation |
| Friends | hall | weekly | none | none |
| New volunteers | quarters | weekly | preparation | first year |

The home herd is everyone who stays and waits, and the table treats that as a
real assignment. Letters go out weekly from the outpost whether or not there is
news, because a short letter saying "all well" is the cheapest morale system
in the game.

---

## 53. APPENDIX AN — NEIGHBOR CONTACT TABLE

| Contact | First meeting | Gift | Agreement | Review |
|---|---|---|---|---|
| River fishers | bend | salt | fishing line | yearly |
| Homestead family | yard gate | bread | quiet and help | yearly |
| Quarry crew | spur | tools | stone exchange | season |
| Passing traders | outpost gate | none | road use | event |
| Ridge shepherds | path | wool | grazing line | yearly |
| Neighbor line | stones | cloth | shared watch | season |
| Old families | valley mouth | letters | respect | yearly |
| Wandering party | boundary | none | entry terms | event |
| Forest camp | cut line | smoke pack | fire watch | season |
| Deep vale | none | none | leave alone | yearly |

Ten first contacts, each with a gift, an agreement, and a review. The
deep-vale row is included because the shelter's decision to stay away is a
relation too, and respecting a place can be an act of diplomacy.

---

## 54. APPENDIX AO — OUTPOST REVIEW TABLE

| Review | Frequency | Question | Owner |
|---|---|---|---|
| Supply | weekly | full or short? | Dero |
| Health | weekly | anyone unwell? | Ikra |
| Rota | weekly | anyone exhausted? | leader |
| Watch | weekly | drills kept? | watch lead |
| Radio | weekly | windows kept? | Miri |
| Neighbor | monthly | agreements held? | Lef |
| Charter | half-year | still the right purpose? | steward |
| Winter | seasonal | proofs passing? | Bron |
| Rotation | yearly | fair and staffed? | leader |
| Recall | yearly | threshold still right? | steward |

Ten reviews keep an outpost honest from a distance. The charter and recall rows
are the shelter's promise that distance will never quietly become an excuse
to forget the people who live there.

---

## 55. APPENDIX AP — FOUNDING PARTY TABLE

| Role | First party | Backup | Kit | Weight |
|---|---|---|---|---|
| Leader | Halla | Dero | charter, radio | light |
| Builder | Bron | crew | tools, nails | heavy |
| Grower | Silla tie | helper | seed, stakes | medium |
| Medic | Ikra | aide | chest | medium |
| Radio | Miri | trainee | set, batteries | medium |
| Scout | Yara | pair | route notes | light |
| Cook | volunteer | rotate | pots, rations | heavy |
| Watch | Boden tie | rotate | fence kit | medium |

The founding party is deliberately heavy on tools and light on sentiment. The
table is what goes up the valley on the first day, and the backup column is the
shelter's promise that no single person is the only one who knows how the place
works.

---

## 56. APPENDIX AQ — RETURN PACK TABLE

| Item | Why it returns | Who carries | Record |
|---|---|---|---|
| Charter copy | shared history | leader | archive |
| Reports | memory | scribe | archive |
| Mail | letters home | courier | family |
| Tools | accountability | builder | store |
| Medicine chest | restock | medic | clinic |
| Seed samples | continuity | grower | bank |
| Radio set | reassignment | operator | comms |
| Site plate | memory | anyone | museum wall |

Eight things that come home when an outpost closes, and the last one is the
most important: the site plate goes onto the wall beside every other place the
shelter has been, so a valley that was lived in is never simply a coordinate
with nobody left to name it.

---

## 57. CLOSING STATEMENT

ASHFALL already has colonies as state: outposts with supplies, morale, and
populations, supply lines with capacity and flow, and expedition systems that
can carry people and cargo. What it lacks is the life of distance: charters,
site surveys, founding parties, depots, schedules, radio windows, mail,
rotation, and the plan that brings everyone home. The Outpost adds that life
without adding a second colony or a frontier fantasy. It adds a valley with a
charter and a lamp in the window, a nightly call at nine, and a shelter that
measures itself by whether the people it sends away come back.

> Wave 7 note: this plan is one of five Wave 7 expansion bibles (42–46). Each is
> self-contained; none requires another to ship. The shared Wave 7 index lives
> at `docs/expansions/wave7/WAVE7_INDEX.md`. The safe pre-signature step is
> Phase 1 (data schemas and validators), which is additive and reversible.
> Evidence anchors: `ColonySystem` (`TotalColonyCount`, `ActiveSupplyLineCount`,
> `OnColonyEstablished`, `OnSupplyLineStatusChanged`, `OnColonySuppliesUpdated`,
> `EstablishColony`, `EstablishSupplyLine`, `SetSupplyLineStatus`,
> `AssignSurvivorToColony`, `TransferSupplies`, `TickDay`, `CaptureState`,
> `RestoreState`), `ColonyOutpost` (`ColonyId`, `LocationId`, `Type`,
> `EstablishedDay`, `DefenseRating`, `MoraleRating`, `PopulationIds`,
> `StoredSupplies`, `IsActive`), `SupplyLine` (`OriginId`, `DestinationId`,
> `Status`, `CargoCapacity`, `DailyFlow`, `LastSupplyDay`), and the data
> `settlements.json` (24,974 B) and `wasteland_settlement_npcs.json` (30,940 B).