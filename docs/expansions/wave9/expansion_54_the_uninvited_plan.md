# ASHFALL — Expansion 54 Design Bible
# THE UNINVITED
### Wave 9 · Pests, Prevention, Traps, Quarantine, Tolerance, Rot, and the Careful War for the Food Stores

**Document status:** Design plan (pre-integration). Not a claim. Not an authorization.
**Date:** 2026-09-24
**Domain owners touched:** `Ashfall.Core.Ecology` (`EcologicalInfestationSystem`, `EcologicalInfestationCatalog`, `EcologicalInfestationDefs`), `Ashfall.Core` (food and inventory loss seam), `Ashfall.Core.Shelter` (ventilation wear seam), `Ashfall.Core.Medical` (disease port seam)
**Proposed host owner:** `UninvitedHostSession` (extends `EcologicalInfestationSaveStore` + the Plan 28 lifecycle)
**Existing save sections:** `ecological_infestation` (`ecological_infestation_save.json`)
**Existing CLI verbs:** `--data-integrity-selftest`, `--content-utilization-selftest`, `--panel-bind-lifecycle-selftest` (no infestation-specific verb exists)
**Rule compliance:** Godot authoritative; Core engine-free; JSON data authoritative; one authority per concern; deterministic seeded RNG.

---

## 0. HOW TO READ THIS DOCUMENT

This is a **design bible**, not an integration plan. Implementation must later pass
through `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `TEST_POLICY.md`.

ASHFALL already models infestations as ecological events with real
consequences. `EcologicalInfestationSystem` (`SystemId` =
"ecological_infestation") defines `RecurrenceCooldownDays = 5` and the hard
cap `MaxFoodLossPerDay = 3`, and its API is live: `LoadDefinitions`,
`TryGetDefinition`, `IsActive`, `IsEligibleToTrigger(currentDay, season)`,
`TryTrigger(infestationId, day, rng, out reason)`, `TryClear(...)`,
`TryTolerateAndHarvest(...)`, `TickDay(...)` (which applies food loss through
the owning system, capped), and `RestoreState`. The definition family
(`EcologicalInfestationDefs`, `EcologicalInfestationCatalog`) carries authored
triggers, clear costs from live item ids, effects, and tolerated-harvest
alternatives. The data file `ecological_infestations.json` describes exactly
**ten affairs**: six location infestations (a subway molerat burrow, a quarry
hornet swarm, a cider-press cellar rot, an ordnance roach cluster, a bridge
fungal carpet, a printworks rat king) and four shelter crises (vent mold,
pantry weevils, a wall nest, tray cutworms). Its own notes document the design
contract: effects route through host callbacks into inventory food loss, the
Plan 09 disease port, and ventilation wear; three infestations have a bounded
non-combat tolerated-harvest path; and spore disease is seeded only through
the existing disease port. The save store and section exist under Plan 28.

What does not exist: the practice. There is no prevention program, no
inspection cadence, no storage standard, no trap line or bait plan, no
monitoring counts, no quarantine flow for incoming goods, no clearance team
or tools beyond item costs, no tolerance policy, no guardian-animal
coordination, no site histories, and no story about the war a shelter fights
inside its own pantry.

**The Uninvited** is the expansion about pests: what comes in with the sacks,
what grows in the wet corner, what nests in the wall, and what a shelter owes
both its food and the creatures it lives beside. It extends the live
infestation lifecycle and never duplicates an ecology, disease, food, or
wildlife authority.

Conventions: **`LIVE`** confirmed in source/data; **`GAP`** confirmed thin;
**`PROPOSED`** new.

### 0.1 Boundary contracts

| Neighbour | Owns | This plan does |
|---|---|---|
| `EcologicalInfestationSystem` | Trigger, clear, tolerate, loss | Extends with practice and content |
| 15 The Deep Root (Wave 1) | Crops, soil, livestock, vet | Reads crop-loss effects; never owns farming |
| 26 The Common Table (Wave 3) | Food, preservation, rations | Sends losses and prevention through it |
| 32 The Wild (Wave 5) | Wildlife, companions | Requests guardian animals; never owns them |
| 38 The Ward (Wave 6) | Disease and care | Spore effects route through the disease port |
| 22 The Clean Flow (Wave 3) | Sanitation and waste | Hygiene practice coordinates |
| 19 The Bitter Air (Wave 2) | Ventilation and filtration | Mold wear routes through the vent owner |
| 39 The Reagent (Wave 6) | Chemistry | No poisons; cleaning agents are orders only |
| 34 The Long Road (Wave 5) | Incoming goods | Quarantine inspects arrivals |
| 44 The Outpost (Wave 7) | Remote sites | Site infestations report home |
| 50 The Vault (Wave 8) | Paper preservation | Vault pests route through its climate owner |
| 47 The Brigade (Wave 8) | Fire and storage | Grease and nesting fire loads reported |
| `ExcavationSystem` (Wave 2) | Sites and hazards | Burrows at sites read its data |
| `StandingRecord` (Exp 03) | Records | Files inspections and histories |

---

## 1. EXECUTIVE SUMMARY

### 1.1 Pitch

The grain sacks come in from a salvage run, and by the fourth week the pantry
shelf is moving. There is mold in the vent above the laundry, a nest in the
wall behind the dormitory headboard, and something living under the printworks
floor that has been living there longer than the shelter has.

**The Uninvited** is the expansion about the small war that never ends: pests,
rot, mold, weevils, burrows, and nests. It gives the shelter inspections,
storage standards, sealing, trap lines, monitoring counts, quarantine, and a
tolerance policy that admits some edges are better shared than conquered. It
is the expansion about the difference between a shelter that fights pests and
a shelter that lives with them on purpose.

### 1.2 The five loops it adds

```
  Watch ──► Seal ──► Catch ──► Judge ──► Learn
     │        │         │          │          │
     ▼        ▼         ▼          ▼          ▼
  Counts,  screens,  traps,    clear/    histories,
  rounds   stores    teams     tolerate  changes
                                        │
                                        ▼
                          Quarantine ──► Record ──► Repeat
```

### 1.3 What the player manages

1. **Watching.** Rounds, counts, bait stations, and early signs.
2. **Sealing.** Screens, gaps, doors, sacks, and storage standards.
3. **Stores.** Dry, cold, sealed, rotated, and inspected.
4. **Traps.** Lines, sets, checks, humanness, and evidence.
5. **Clearance.** Teams, tools, protective gear, and site work.
6. **Tolerance.** Which edges are shared, harvested, or left.
7. **Quarantine.** Incoming goods, salvage, and isolation timing.
8. **Guardians.** Cats and other working animals requested from their owner.
9. **Records.** Histories, lessons, and the annual loss figure.
10. **People.** Fatigue, exposure, gloves, and nobody working alone.

### 1.4 What it is not

- Not a second ecology or wildlife system; the wild stays outside.
- Not a chemical warfare expansion; no poisons, ever.
- Not a kill-everything game; tolerance is a first-class choice.
- Not a cruelty system; traps are mechanical and checked.
- Not animal companions; guardians are owned by the companion system.
- Not a second disease or food system.
- Not a new save section.

---

## 2. EVIDENCE BASE AND GAP ANALYSIS

### 2.1 Live systems (verified)

| File | Role | Status |
|---|---|---|
| `Assets/Ashfall.Core/Ecology/EcologicalInfestationSystem.cs` | Lifecycle: trigger, clear, tolerate | `LIVE` |
| `Assets/Ashfall.Core/Ecology/EcologicalInfestationDefs.cs` | Definitions | `LIVE` |
| `Assets/Ashfall.Core/Ecology/EcologicalInfestationCatalog.cs` | Catalog | `LIVE` |
| `src/Host/EcologicalInfestationSaveStore.cs` | `ecological_infestation` save | `LIVE` |
| `Assets/Ashfall.Core` food-loss callback | Inventory loss capped at 3/day | `LIVE` |
| Plan 09 disease port | Spore disease seeding | `LIVE` |
| `VentilationSystem` | Mold wear sink | `LIVE` |
| `CompanionAnimalSystem` (Wave 5) | Guardian animals | `LIVE` |

### 2.2 Live data (counted)

| Catalog | Size | Notes |
|---|---|---|
| `ecological_infestations.json` | one collection | **10 infestations** (6 location, 4 shelter) |
| Prevention, trap, inspection catalogs | absent | confirmed none |
| Quarantine and guardian content | absent | confirmed none |
| Site history and lesson content | absent | confirmed none |

### 2.3 Confirmed gaps

- **GAP-54-1 — Ten affairs and no prevention program.**
- **GAP-54-2 — No inspection cadence or early-sign content.**
- **GAP-54-3 — No storage standards or sealing content.**
- **GAP-54-4 — No trap lines, sets, or check content.**
- **GAP-54-5 — No clearance team, gear, or work method content.**
- **GAP-54-6 — No tolerance policy or harvest rules.**
- **GAP-54-7 — No quarantine flow for incoming goods.**
- **GAP-54-8 — No guardian-animal coordination.**
- **GAP-54-9 — No histories, lessons, or loss records.**
- **GAP-54-10 — The live lifecycle runs with no practice around it.**

### 2.4 Non-duplication statement

This expansion will **not** add a second ecology, wildlife, disease, food,
sanitation, ventilation, or companion system. It extends
`EcologicalInfestationSystem` with practice and content, sends losses through
the existing food and inventory owners, spore effects through the Plan 09
disease port, mold wear through the vent owner, guardian animals through the
companion owner, and records through `StandingRecord`. All new state is
additive inside the `ecological_infestation` section. No new save section.

---

## 3. DESIGN PILLARS AND TONE

### 3.1 Pillars

**Pillar 1 — Pests are a storage problem first.** A shelter that seals its
stores is fighting half the war on the shelf.

**Pillar 2 — Watch beats poison.** Counts and early signs are how a small
problem stays small.

**Pillar 3 — Some edges are shared.** Tolerance with harvest is a legitimate
strategy, with rules and records.

**Pillar 4 — Humane and careful.** Traps are mechanical, checked, and
cleaned; nobody tortures anything.

**Pillar 5 — Loss is measured honestly.** The annual figure is a real number
and the shelter publishes it.

### 3.2 Tone calibration

| Element | Do | Do not |
|---|---|---|
| Rounds | Quiet, methodical | Monster hunting |
| Sealing | Gaps, screens, sacks | Fortress fantasy |
| Traps | Mechanical, checked | Cruelty or spectacle |
| Tolerance | Rules, harvest, records | Surrender |
| Mold | Damp, spores, ventilation | Horror imagery |
| Quarantine | Patience, isolation | Stigmatizing people |
| Guardians | Working animals, cared for | Gadget pets |
| Records | Numbers and lessons | Blame |

### 3.3 Content limits

- No poison or chemical pest control; cleaning agents are reagent orders.
- No torture, glue traps for spectacle, or animal suffering framing.
- No kill-everything reward loop; tolerance is fully viable.
- No children in clearance work; apprentices are 16+ and supervised.
- No stigmatizing quarantine of people; goods are quarantined, not residents.
- No real-world pest species claims or chemical products copied.
- No new save section.

---

## 4. THE UNINVITED WORLD

### 4.1 Interior rooms

- **`room_pest_office`** — the round board, the count book, and the map.
- **`room_granary`** — sealed bins, raised floors, and the dry corner.
- **`room_trap_store`** — traps, cages, gloves, and the cleaning bench.
- **`room_quarantine_bay`** — incoming goods and the timer shelf.
- **`room_dry_room`** — the dehumidified store with its card.
- **`room_clearance_locker`** — masks, coveralls, brushes, and lamps.
- **`room_kennel_link`** — the liaison office for guardian animals.
- **`room_ledger_room`** — site histories and the annual figure.

### 4.2 Exterior locations

| ID | Name | Tone | Purpose |
|---|---|---|---|
| `loc_pantry_shed` | The Pantry Shed | 3 | First weevils |
| `loc_cellar_rot` | The Cider Cellar | 3 | Rot and damp |
| `loc_burrow_field` | The Burrow Field | 3 | Molerat case |
| `loc_hornet_quarry` | The Quarry Face | 4 | Hornet swarm |
| `loc_fungal_bridge` | The Rust Bridge | 3 | Fungal carpet |
| `loc_printworks_floor` | The Printworks | 4 | Rat king |
| `loc_ordnance_stack` | The Ordnance Stack | 4 | Roach cluster |
| `loc_wall_nest_house` | The Nest House | 3 | Wall nest |
| `loc_tray_rows` | The Tray Rows | 2 | Cutworms |
| `loc_loss_stone` | The Loss Stone | 2 | Annual figures cut |

All locations require valid item or map-node references and scanner registration.

### 4.3 The rhythm

Rounds weekly, counts monthly, the granary inspected before every salvage
intake, traps checked daily, quarantine shelf on a timer, and the annual loss
figure cut into the stone each winter. The expansion's clock is the round.

---

## 5. MAIN STORYLINE — "WHAT COMES IN WITH THE SACKS"

### 5.1 Central conflict

**Wilm Branwell** has been fighting pests alone for years with a bad lamp and
a worse notebook. When a salvage run brings grain into a shelter that has
never had a granary, the pantry weevils arrive within a month, and Wilm's
private war becomes a public problem. **Zanna Mireley** wants storage
standards that would have stopped the weevils at the sack. **Zel Hasken**
wants a trap line and the authority to check it every day. **Bexley Garth**
wants the granary rebuilt with raised floors and sealed bins. **Dunlin Greve**
will crawl into the molerat burrow if he has to. **Roslin Nix** wants a
quarantine shelf for every incoming sack. **Otta Pitt** wants the kitchen's
own dry corner to stop being the first casualty.

Then the vent mold blooms and spores reach the laundry, and the shelter learns
that an infestation is not only a food problem. **Fenna Kett**, sixteen,
counts the tray cutworms every morning and writes numbers nobody reads until
the numbers save the spring planting. The hard argument arrives last: the
quarry hornets have been there longer than the shelter, the fungal carpet is
part of the bridge now, and the shelter has to decide which wars it fights and
which edges it farms.

The expansion's question: **what does a shelter owe the creatures it lives
beside?**

### 5.2 Theme (unspoken)

**The pantry is the first place a shelter is honest about its habits.**

### 5.3 Principal NPCs

| ID | Name | Role | Function |
|---|---|---|---|
| `npc_pest_wilm_branwell` | Wilm Branwell | Pest officer | Rounds and war policy |
| `npc_stores_zanna_mireley` | Zanna Mireley | Stores inspector | Standards and checks |
| `npc_trap_zel_hasken` | Zel Hasken | Trap maker | Lines and humanness |
| `npc_granary_bexley_garth` | Bexley Garth | Granary keeper | Sealing and rotation |
| `npc_burrow_dunlin_greve` | Dunlin Greve | Burrow runner | Site clearance |
| `npc_quarantine_roslin_nix` | Roslin Nix | Quarantine aide | Incoming goods |
| `npc_kitchen_otta_pitt` | Otta Pitt | Kitchen vigilance | Dry corners and fats |
| `npc_apprentice_fenna_kett` | Fenna Kett | Apprentice | Counts and cards |

### 5.4 Story beats (15)

1. **The Sacks.** Grain arrives and the count begins.
2. **The Pantry.** Weevils are found and named.
3. **The Rounds.** Weekly inspection becomes real.
4. **The Traps.** A trap line is set and checked.
5. **The Granary.** Raised floors and sealed bins.
6. **The Quarantine.** Every sack waits on a shelf.
7. **The Mold.** Vent spores reach the laundry.
8. **The Cellar.** Rot is found, cleaned, and re-lined.
9. **The Burrow.** The molerat case is worked.
10. **The Hornets.** The quarry decision is made.
11. **The Bridge.** The fungal carpet is tolerated with rules.
12. **The Guardian.** A working cat is requested and cared for.
13. **The Printworks.** The rat king case ends with a rebuilt floor.
14. **The Loss Stone.** The year's figures are cut.
15. **What Comes In With the Sacks.** Prevention becomes habit.

### 5.5 Branching choices (8)

| Choice | Options | Axis |
|---|---|---|
| Policy | clear all / watch and clear / live with edges | war vs. peace |
| Storage | sealed bins / sacks on pallets / improvised | discipline |
| Traps | full line / targeted / none | effort |
| Tolerance | harvest / leave / none | ethics |
| Quarantine | every intake / suspect loads / none | safety vs. speed |
| Guardians | cats / none / other | care |
| Records | full / annual / none | honesty |
| Final | sealed shelter / shared edges / peace with limits | identity |

### 5.6 Endings (5 + fade)

1. **The Sealed Granary** — storage standards hold, losses fall to almost
   nothing, and the pantry stops making news.
2. **The Watched Shelf** — the shelter cannot eliminate pests but always
   knows their numbers, and the annual loss is small and honest.
3. **The Tolerated Edge** — the hornets, the fungal carpet, and the orchard
   edge are shared with rules, harvests, and respect.
4. **The Clean Vent** — the mold war is won with dry air, cards, and a filter
   schedule, and the laundry stops coughing.
5. **The Lesson Kept** — every case is recorded with its cause and fix, and
   the shelter's history of pests becomes its best prevention.
6. **Fade** — a granary in the morning, sealed bins, a trap line checked in
   the same order every day, and a loss stone with a smaller number on it.

---

## 6. QUEST DESIGN

New IDs use prefix `quest_uninvited_`. Schema follows existing quest catalogs.

### 6.1 Main questline (15)

`quest_uninvited_sacks`, `quest_uninvited_pantry`, `quest_uninvited_rounds`,
`quest_uninvited_traps`, `quest_uninvited_granary`, `quest_uninvited_quarantine`,
`quest_uninvited_mold`, `quest_uninvited_cellar`, `quest_uninvited_burrow`,
`quest_uninvited_hornets`, `quest_uninvited_bridge`, `quest_uninvited_guardian`,
`quest_uninvited_printworks`, `quest_uninvited_loss_stone`,
`quest_uninvited_habits`.

### 6.2 Side quests (30)

**Watching (5)**
- `quest_uninvited_count` — counts taken
- `quest_uninvited_bait_map` — bait stations mapped
- `quest_uninvited_early_sign` — early sign logged
- `quest_uninvited_night_check` — night check
- `quest_uninvited_round_card` — round card kept

**Sealing and stores (5)**
- `quest_uninvited_screen` — screens fitted
- `quest_uninvited_gap` — gaps sealed
- `quest_uninvited_raised` — pallets and shelves
- `quest_uninvited_bin` — sealed bins
- `quest_uninvited_rotation` — first-in rotation

**Traps (5)**
- `quest_uninvited_line` — trap line set
- `quest_uninvited_check` — daily check
- `quest_uninvited_humane` — humane set
- `quest_uninvited_evidence` — evidence logged
- `quest_uninvited_disposal` — disposal proper

**Clearance (5)**
- `quest_uninvited_gear` — gear ready
- `quest_uninvited_pair` — pair rule
- `quest_uninvited_cellar_clear` — cellar cleared
- `quest_uninvited_vent_clear` — vent cleared
- `quest_uninvited_debris` — debris removed

**Tolerance (5)**
- `quest_uninvited_harvest` — harvest rules
- `quest_uninvited_leave` — leave an edge
- `quest_uninvited_boundary` — boundary set
- `quest_uninvited_hornet_truce` — hornet truce held
- `quest_uninvited_share_record` — shared edge recorded

**Records (5)**
- `quest_uninvited_history` — site history written
- `quest_uninvited_lesson` — lesson taught
- `quest_uninvited_loss` — loss figured
- `quest_uninvited_stone` — stone cut
- `quest_uninvited_review` — annual review

### 6.3 Repeatable quests (8)

`quest_uninvited_repeat_round`, `quest_uninvited_repeat_check`,
`quest_uninvited_repeat_count`, `quest_uninvited_repeat_seal`,
`quest_uninvited_repeat_quarantine`, `quest_uninvited_repeat_bait`,
`quest_uninvited_repeat_loss`, `quest_uninvited_repeat_review`.

### 6.4 Dynamic hooks

Live events (`TryTrigger`, `TryClear`, `TryTolerateAndHarvest`, `TickDay`,
season transitions, salvage intakes, ventilation state, disease-port events)
attach authored follow-ups through existing seams. No new event bus.

### 6.5 Constraints

- Trigger, clear, tolerate, and loss stay with `EcologicalInfestationSystem`.
- Food loss stays capped by the live `MaxFoodLossPerDay`.
- Spore disease routes through the Plan 09 disease port only.
- Mold wear routes through the ventilation owner.
- Guardian animals route through the companion owner.
- No poisons; cleaning agents are reagent orders.
- No new save section.

---

## 7. NEW GAMEPLAY SYSTEMS

### 7.1 `PestWatchSystem` (new, `Ashfall.Core.Ecology`)

**Owns:** rounds, counts, bait maps, early signs, and the count book.
**Consumes:** the live infestation state, seasons, food stores. **Data:**
`pest_rounds.json`, `pest_counts.json`. **Rules:** a round is a route with
stops and a card; counts are numbers, never vibes; early signs trigger a
review, not a panic.

### 7.2 `SealingSystem` (new, `Ashfall.Core.Ecology`)

**Owns:** storage standards, screens, gaps, seals, pallets, bin specs, and
dry-corner rules. **Consumes:** workshop, kiln, press, inventory. **Data:**
`pest_storage.json`. **Rules:** a standard is checkable; sealed bins and raised
floors reduce trigger odds; an improvised store is recorded as a risk.

### 7.3 `TrapSystem` (new, `Ashfall.Core.Ecology`)

**Owns:** trap types, lines, sets, checks, evidence, and humane rules.
**Consumes:** trap store, inventory. **Data:** `pest_traps.json`. **Rules:**
every set is checked daily; mechanical traps only; a missed check is recorded;
traps never endanger children or guardian animals.

### 7.4 `ClearanceTeamSystem` (extend `EcologicalInfestationSystem`)

**Owns:** clearance teams, protective gear, work method, and pair rules.
**Consumes:** live `TryClear` costs, locker, ventilation for masks. **Data:**
`pest_clearance.json`. **Rules:** clearance follows the live item costs;
nobody works a bad space alone; gear is cleaned after; debris is disposed
through the sanitation owner.

### 7.5 `ToleranceSystem` (new, `Ashfall.Core.Ecology`)

**Owns:** tolerance policy, harvest rules, boundaries, truces, and records.
**Consumes:** live `TryTolerateAndHarvest` path, site data. **Data:**
`pest_tolerance.json`. **Rules:** a tolerated edge has a boundary, a harvest
limit, and a review date; tolerance is recorded as a decision, not neglect.

### 7.6 `QuarantineSystem` (new, thin, `Ashfall.Core.Ecology`)

**Owns:** incoming-goods quarantine, timers, inspection, and release.
**Consumes:** salvage and trade arrivals, dry room. **Data:**
`pest_quarantine.json`. **Rules:** goods wait on a shelf for the authored
period; release needs a check; quarantined goods are labelled, never mixed
with stores; people are never quarantined as suspects.

### 7.7 `GuardianLinkSystem` (new, thin, `Ashfall.Core.Ecology`)

**Owns:** requests and coordination with the companion-animal owner: which
working animal covers which store, feeding responsibility, and health checks.
**Consumes:** `CompanionAnimalSystem` (owner), vet owner. **Data:**
`pest_guardians.json`. **Rules:** guardians are owned by the companion system;
this plan only requests, places, and records; an animal's care is never
optional; no animal is exposed to poison or traps.

### 7.8 `InfestationRecordSystem` (new, thin, `Ashfall.Core.Ecology`)

**Owns:** site histories, lessons, annual loss figures, and the loss stone.
**Data:** `pest_records.json`. Records through `StandingRecord`. **Rules:**
every case records cause, action, and lesson; the annual loss is published;
the stone carries the year's number for whoever comes next.

### 7.9 Systems explicitly not added

- No second ecology, wildlife, disease, food, sanitation, or ventilation
  system.
- No poisons, chemical baits, or fumigation content.
- No cruelty, spectacle, or kill-everything loop.
- No animal companions owned here.
- No quarantine of people.
- No new currency.
- No new RNG stream beyond the live trigger path.
- No new save section.

---

## 8. DATA CATALOG SPECIFICATION

All catalogs snake_case, integer `schema_version: 1`, validated and scanner-registered.

### 8.1 `pest_rounds.json` (new)

```json
{
  "schema_version": 1,
  "rounds": [
    {
      "round_id": "round_granary_weekly",
      "display_name": "Granary Weekly",
      "stops": ["granary_a", "dry_room", "pantry_shelf"],
      "check": ["sacks", "bins", "boards", "traps"],
      "cadence": "weekly",
      "staff": 1,
      "tags": ["stores", "priority"]
    }
  ]
}
```

### 8.2 `pest_counts.json` (new)

Counts: site, kind, number, trend, card day, note.

### 8.3 `pest_storage.json` (new)

Standards: store, standard, gap check, pallet height, bin seal, review.

### 8.4 `pest_traps.json` (new)

Traps: kind, set, bait, check cadence, humanness, disposal.

### 8.5 `pest_clearance.json` (new)

Clearance: case, method, gear, pair rule, duration, disposal route.

### 8.6 `pest_tolerance.json` (new)

Tolerance: edge, kind, boundary, harvest limit, review date, keeper.

### 8.7 `pest_quarantine.json` (new)

Quarantine: goods class, timer days, check, release, label.

### 8.8 `pest_guardians.json` (new)

Guardians: animal ref, store covered, feeding owner, vet check, note.

### 8.9 `pest_records.json` (new)

Records: case, cause, action, lesson, year, figure, keeper.

### 8.10 `pest_loss.json` (new)

Losses: year, food lost, prevention cost, figure, note.

### 8.11 Items

New items appended to `items.json`: `item_trap_spring`, `item_trap_cage`,
`item_bait_station`, `item_sealed_bin`, `item_screen_mesh`,
`item_gap_filler`, `item_pallet_low`, `item_count_book`,
`item_round_card`, `item_gloves_thick`, `item_mask_dust`,
`item_coverall`, `item_brush_wire`, `item_lamp_pest`,
`item_quarantine_label`, `item_loss_tally_board`.

---

## 9. SAVE, DETERMINISM, AND PERSISTENCE

### 9.1 Ownership

The `ecological_infestation` section remains the live save owner. New
sub-objects (rounds, counts, standards, traps, clearance, tolerance,
quarantine, guardians, records, losses) are additive inside it. No new save
section.

### 9.2 State to persist

- Active and cleared infestations with their live state.
- Round history and count numbers.
- Storage standards and gap checks.
- Trap lines, sets, and check history.
- Clearance teams and gear condition.
- Tolerance edges, boundaries, and review dates.
- Quarantine shelf contents and timers.
- Guardian placements and care notes.
- Site histories, lessons, and annual figures.

### 9.3 Determinism

- Triggers, recurrence, and loss stay on the live seeded path with
  `RecurrenceCooldownDays` and `MaxFoodLossPerDay`.
- Round outcomes derive from state and counts.
- Clearance results derive from live costs and gear.
- Tolerance harvest yields derive from authored limits.
- Quarantine timers use campaign days.
- Paired replay hashes must match; no `System.Random`.

### 9.4 Migration

Legacy saves load with active infestations exactly as recorded; new practice
state (rounds, traps, standards) starts empty. A shelter with an active rat
king keeps it, and the first round that touches it begins a case file rather
than retriggering anything.

### 9.5 Checksum

Invariant-culture floats; integer day, count, and permille fields.

---

## 10. UI, ACCESSIBILITY, AND PRESENTATION

### 10.1 Surfaces

| Surface | Purpose | Owner |
|---|---|---|
| `InfestationPanel` (extend) | Live cases and actions | `UninvitedHostSession` |
| `PestRoundPanel` (new) | Rounds and counts | same |
| `StorageStandardPanel` (new) | Sealing and stores | same |
| `TrapLinePanel` (new) | Sets and checks | same |
| `QuarantinePanel` (new) | Incoming goods | same |
| `TolerancePanel` (new) | Edges and harvests | same |
| `LossRecordPanel` (new) | Histories and figures | same |

### 10.2 Accessibility and honesty

- Panels show live state and expose existing commands only.
- Counts are numbers with trends, never vague dread.
- Trap checks are a checklist, never a timed action.
- Tolerance screens state the boundary and review date before acceptance.
- Quarantine shows goods, not people, as the subject.
- Keyboard/controller close/back preserved; focus maintained on refresh.

### 10.3 Presentation

Audio cues appended to `audio_cues.json`: a sack set down, a bin lid sealing, a
trap spring, a count pencil, a brush on a wall, a stone being cut. No cue is
required; text carries meaning.

---

## 11. INTEGRATION SEAMS

| Existing system | Attachment |
|---|---|
| `EcologicalInfestationSystem` | Trigger, clear, tolerate, loss |
| `FoodPreservationSystem` | Food loss and prevention |
| `Inventory` | Costs, traps, bins, sacks |
| `CompanionAnimalSystem` (Wave 5) | Guardian animals |
| `VeterinarySystem` (Wave 1) | Guardian health |
| Disease port (Plan 09) | Spore effects |
| `VentilationSystem` | Mold wear and filters |
| `SanitationSystem` (Wave 3) | Waste and debris |
| `RouteInfrastructureSystem` (Wave 5) | Incoming goods |
| 44 The Outpost (Wave 7) | Site cases |
| `ShelterWorkshopSystem` (Wave 6) | Traps, bins, screens |
| `KilnworksHostSession` (Wave 4) | Stoneware and seals |
| `PressHostSession` (Wave 4) | Cards, labels, forms |
| `ShelterThermalSystem` | Dry heat and cold stores |
| `StandingRecord` (Exp 03) | Histories and figures |
| `JournalSystem` | Round notes in journals |
| `EpilogueChronicleBuilder` | Pest history lines |

---

## 12. TECHNICAL IMPLEMENTATION PLAN

### 12.1 Phase order

**Phase 0 — Premise re-audit.** Confirm the infestation system, catalog, data,
save store, food, disease, ventilation, companion, vet, and record owners.
Record file:line; change nothing.

**Phase 1 — Data + validators.** Author the ten catalogs; register validators
and scanner.

**Phase 2 — Pure Core.** `PestWatchSystem`, `SealingSystem`, `TrapSystem`,
`ClearanceTeamSystem`, `ToleranceSystem`, `QuarantineSystem`,
`GuardianLinkSystem`, `InfestationRecordSystem`.

**Phase 3 — Persistence.** Additive sub-objects, migration, round-trip,
determinism.

**Phase 4 — Host + CLI.** `UninvitedHostSession`, focused selftest coverage,
fresh journey from the sacks to the loss stone.

**Phase 5 — UI.** New panels with lifecycle and accessibility.

**Phase 6 — Content.** Rooms, locations, NPCs, quests, items, prose, audio.

**Phase 7 — Balance.** Multi-year soak: prevention lowers triggers, tolerance
holds edges, quarantine slows but protects, losses fall.

**Phase 8 — Verification and closeout.**

### 12.2 Content volume

| Content | Count |
|---|---|
| Rounds | 12 |
| Counts | 24 |
| Storage standards | 16 |
| Traps | 12 |
| Clearance cases | 12 |
| Tolerance edges | 8 |
| Quarantine classes | 10 |
| Guardians | 6 |
| Records | 24 |
| Loss years | 10 |
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
| Ecology duplication | Critical | Live owner only |
| Poison drift | High | No chemicals by contract |
| Cruelty content | High | Mechanical, checked traps |
| Kill-everything loop | High | Tolerance first-class |
| Disease authority | Medium | Port only |
| Companion ownership | Medium | Request-and-record only |
| Quarantine of people | Critical | Goods only |
| Determinism break | Low | Live seeded path |

---

## 13. APPENDIX A — AUTHORING BUDGET

| File | Rows | Est. words |
|---|---|---|
| `pest_rounds.json` | 12 | 2,500 |
| `pest_counts.json` | 24 | 3,000 |
| `pest_storage.json` | 16 | 3,000 |
| `pest_traps.json` | 12 | 2,500 |
| `pest_clearance.json` | 12 | 3,000 |
| `pest_tolerance.json` | 8 | 2,000 |
| `pest_quarantine.json` | 10 | 2,000 |
| `pest_guardians.json` | 6 | 1,500 |
| `pest_records.json` | 24 | 4,000 |
| `pest_loss.json` | 10 | 2,000 |
| Quest objectives | 53 quests | 15,000 |
| NPC prose | 8 NPCs | 7,000 |
| Location prose | 10 | 3,500 |
| Item descriptions | 16 | 2,500 |
| Ending prose | 6 | 3,000 |
| **Total** | | **~56,500** |

---

## 14. APPENDIX B — RISK REGISTER

| ID | Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|---|
| R54-1 | Ecology overlap | Low | Critical | Live owner |
| R54-2 | Poison drift | Low | High | Contract |
| R54-3 | Cruelty | Low | High | Humane rules |
| R54-4 | Kill loop | Med | High | Tolerance path |
| R54-5 | Disease | Low | Medium | Port |
| R54-6 | Companion | Low | Medium | Owner |
| R54-7 | People quarantine | Low | Critical | Goods only |
| R54-8 | Determinism | Low | High | Live path |
| R54-9 | Content overrun | Med | Medium | Budget |
| R54-10 | Grind rounds | Med | Medium | Counts and meaning |

---

## 15. APPENDIX C — OPEN DECISIONS REQUIRING A FOREMAN SIGNATURE

1. **Do infestations ever become permanent?** Recommended: only tolerated
   edges with rules; every crisis has a clear or tolerate path.
2. **May the shelter kill pests?** Recommended: yes, mechanically and
   humanely, with checks; tolerance is equally valid.
3. **Do guardian animals ever get sick or die?** Recommended: handled by the
   companion and vet owners, with care, never as plot convenience.
4. **Is the annual loss figure public?** Recommended: yes; it is cut into the
   loss stone.
5. **Can quarantine be skipped?** Recommended: yes for suspect-free goods with
   a recorded risk, never for salvage from infested sites.

---

## 17. APPENDIX D — AFFAIR TABLE (10 CASES)

| # | Affair | Kind | Site | Effect | Path |
|---|---|---|---|---|---|
| 1 | Subway molerat burrow | animal | subway | stores, soil | clear or run-off |
| 2 | Quarry hornet swarm | insect | quarry | stings, work | tolerate with boundary |
| 3 | Cider-press cellar rot | rot | cellar | food, damp | clear and re-line |
| 4 | Ordnance roach cluster | insect | ordnance | contamination | clear only |
| 5 | Bridge fungal carpet | fungus | rust bridge | structural | tolerate with checks |
| 6 | Printworks rat king | animal | printworks | stores, paper | clear and rebuild |
| 7 | Vent mold | mold | vent plant | air, disease port | clear and dry |
| 8 | Pantry weevils | insect | pantry | food loss | clear and seal |
| 9 | Wall nest | animal | dorm wall | noise, smell | clear and seal |
| 10 | Tray cutworms | insect | tray rows | crops | watch and clear |

Ten affairs, and the path column is the expansion's whole philosophy made
concrete: two are tolerated by design, five are cleared, and three are mixed.
The roach row is the one that cannot be shared, and the plan says so plainly —
tolerance is a policy for edges, never for contamination.

---

## 18. APPENDIX E — ROUND TABLE

| # | Round | Stops | Cadence | Checks | Staff |
|---|---|---|---|---|---|
| 1 | Granary | granary, bins | weekly | sacks, boards | 1 |
| 2 | Pantry | shelves, dry corner | weekly | sacks, lids | 1 |
| 3 | Cold store | cold room, door | weekly | seams, floor | 1 |
| 4 | Kitchen | stores, fats | daily | grease, lids | 1 |
| 5 | Dorm walls | headboards, vents | monthly | stains, sounds | 2 |
| 6 | Vent plant | intakes, filters | weekly | damp, mold | 1 |
| 7 | Laundry | lint, damp corners | weekly | lint, water | 1 |
| 8 | Cellar | press, barrels | monthly | rot, damp | 1 |
| 9 | Printworks | floor, paper stacks | monthly | droppings, paper | 2 |
| 10 | Tray rows | soil, trays | weekly | cutworms, leaf | 1 |
| 11 | Outpost stores | storage, traps | with visits | all | visitor |
| 12 | Site survey | burrows, bridges | seasonal | nests, fungus | 2 |

Twelve rounds with stops and cadences, and the two-staff rows are the ones
where the work happens above head height or in dark spaces. The outpost row is
performed by whoever visits, which is how a small settlement without a pest
officer stays in the count.

---

## 19. APPENDIX F — STORAGE STANDARD TABLE

| # | Standard | Applies to | Check | Risk if skipped |
|---|---|---|---|---|
| 1 | Sealed bin | grain, flour | lid, gasket | weevils |
| 2 | Raised pallet | all sacks | gap under | damp, rodents |
| 3 | First-in rotation | all food | date order | spoilage |
| 4 | Dry corner card | pantry | humidity | mold |
| 5 | Screen mesh | vents, windows | tears | insects |
| 6 | Gap under 6 mm | all walls | feeler test | rodents |
| 7 | Door sweep | stores | wear | rodents |
| 8 | No cardboard | food rooms | visual | eggs |
| 9 | Fat lid rule | kitchen | lids | weevils |
| 10 | Paper in boxes | printworks | tightness | rats |
| 11 | Cage over sacks | kitchen | condition | rodents |
| 12 | Cold room seal | cold store | ice, seams | rot |
| 13 | Quarantine shelf | arrivals | label | imports |
| 14 | Trap-free guardians | cat zones | set map | harm |
| 15 | Bedding check | dorms | weekly | nests |
| 16 | Annual deep clean | all | schedule | buildup |

Sixteen standards with a risk column, and the two rows that matter most are
unfashionable: the six-millimeter gap and the no-cardboard rule, because most
of a shelter's pest problem is decided by details smaller than a fingernail.
The guardian row exists so that cats and traps never share a floor.

---

## 20. APPENDIX G — TRAP TABLE

| # | Trap | Kind | Bait | Check | Humanness |
|---|---|---|---|---|---|
| 1 | Snap trap | mechanical | fat | daily | quick |
| 2 | Cage trap | live | grain | twice daily | release or transfer |
| 3 | Bucket trap | live | water-free | daily | release |
| 4 | Sticky board | banned | — | — | never used |
| 5 | Bait station | monitor | block | weekly | no poison |
| 6 | Tunnel trap | mechanical | fat | daily | quick |
| 7 | Glue-free board | monitor | none | weekly | counts only |
| 8 | Net | capture | none | event | release |
| 9 | Smoke (vent) | non-lethal | none | event | moves animals |
| 10 | Hornet lure jar | monitor | sweet | daily | relocate |
| 11 | Mole barrier | exclusion | none | weekly | no catch |
| 12 | Repellent net | exclusion | none | weekly | no catch |

Twelve trap types, and the sticky-board row is marked banned on purpose, in
the table where every trapper reads it. The smoke and barrier rows show the
expansion's preference for moving and excluding over killing, which is both
the kinder and the more effective choice when a shelter intends to keep the
same walls for decades.

---

## 21. APPENDIX H — CLEARANCE CASE TABLE

| # | Case | Method | Gear | Pair | Duration |
|---|---|---|---|---|---|
| 1 | Pantry weevils | empty, clean, seal | gloves, masks | yes | 2 days |
| 2 | Vent mold | scrub, dry, filter | masks, lamps | yes | 3 days |
| 3 | Cellar rot | scrape, lime, re-line | masks, tools | yes | 4 days |
| 4 | Wall nest | open, remove, seal | gloves, lamps | yes | 1 day |
| 5 | Printworks | clear, floor rebuild | gloves, masks | yes | 6 days |
| 6 | Roach cluster | bag, remove, burn-safe | coveralls | yes | 2 days |
| 7 | Molerat burrow | net, relocate, seal | gloves, nets | yes | 2 days |
| 8 | Tray cutworms | hand-pick, trap rows | gloves | no | daily |
| 9 | Hornet response | lure, relocate | veils, smoke | yes | 1 day |
| 10 | Fungal checks | scrape, treat, record | masks | yes | seasonal |

Ten clearance cases with pair rules and durations, and the trap-made row list
includes the cutworms as a no-pair daily task on purpose: the work that
protects the spring planting is small, repetitive, and done by one careful
person with a lamp, which is how most pest work actually looks.

---

## 22. APPENDIX I — TOLERANCE TABLE

| # | Edge | Boundary | Harvest | Review | Keeper |
|---|---|---|---|---|---|
| 1 | Quarry hornets | quarry face only | honeycomb in late autumn | yearly | Dunlin |
| 2 | Rust bridge fungus | bridge deck and abutments | none | season | Wilm |
| 3 | Orchard beetles | orchard edge | none | yearly | Otta |
| 4 | Cellar spiders | cellar corners | none | yearly | Bexley |
| 5 | Tray row ladybirds | tray rows | none | season | Fenna |
| 6 | Printworks mice | under-floor run | none | yearly | Zel |
| 7 | Riverbank reeds | reed bed | cut in winter | yearly | Dunlin |
| 8 | Hedgerow nests | hedge line | none | yearly | Zanna |

Eight tolerated edges, each with a boundary, a harvest rule, a review date,
and a named keeper, and the ladybirds row is included because not everything
shared is a problem: some edges are neighbours the shelter wants. The table's
real content is the keeper column, because a truce nobody owns is just an
accident with better manners.

---

## 23. APPENDIX J — QUARANTINE TABLE

| # | Goods | Timer | Check | Release | Note |
|---|---|---|---|---|---|
| 1 | Grain sacks | 7 days | sift, smell | pass | first priority |
| 2 | Flour | 5 days | seal check | pass | pantry |
| 3 | Dried goods | 5 days | visual | pass | dry room |
| 4 | Salvage paper | 10 days | page riffle | pass | vault tie |
| 5 | Cloth bales | 7 days | fold check | pass | thread tie |
| 6 | Seed lots | 7 days | count, seal | pass | farm |
| 7 | Timber | 14 days | bark check | pass | workshop |
| 8 | Furniture | 10 days | joints, seams | pass | quarters |
| 9 | Outpost returns | 7 days | all above | pass | parcels |
| 10 | Medicine crates | 3 days | seal, date | pass | ward |

Ten quarantine classes with timers, and the medicine row is the shortest
because the ward cannot wait, which the plan handles by inspection and
sealing rather than by pretending urgency does not exist. The timber row is
the longest because bark hides the most, and the shelter learns that from the
printworks floor.

---

## 24. APPENDIX K — GUARDIAN TABLE

| # | Guardian | Store covered | Feeder | Vet check | Note |
|---|---|---|---|---|---|
| 1 | Cat: Ash | granary | Bexley | monthly | sleeps on bins |
| 2 | Cat: Nub | pantry | Otta | monthly | hates lid noise |
| 3 | Cat: Tallow | kitchen store | Otta | monthly | fat thief |
| 4 | Cat: Prickle | printworks | Zel | monthly | paper isle |
| 5 | Terrier: Pip | yard stores | Dunlin | monthly | no traps zone |
| 6 | Owl box | cellar | Bexley | season | bats and mice |

Six guardians, and every row names a feeder and a vet cadence because the
slash between "working animal" and "neglected animal" is a feeding rota. The
owl box is included to show that guardians need not be pets: sometimes the
right arrangement is a box on a wall and a promise not to spray the corners.

---

## 25. APPENDIX L — SITE HISTORY TABLE

| # | Site | First case | Cause | Action | Status |
|---|---|---|---|---|---|
| 1 | Pantry | year 1 weevils | wet sacks | sealed bins | holding |
| 2 | Vent plant | year 1 mold | damp filter | dry, filter | holding |
| 3 | Cellar | year 2 rot | old press | lime, re-line | holding |
| 4 | Dorm wall | year 2 nest | warm pipe | seal, move bed | holding |
| 5 | Printworks | year 3 rats | old floor | rebuilt | holding |
| 6 | Quarry | year 3 hornets | always there | truce | shared |
| 7 | Bridge | year 4 fungus | wet years | monitored | shared |
| 8 | Tray rows | year 4 cutworms | seed stock | hand-pick | holding |
| 9 | Ordnance stack | year 5 roaches | old crates | bagged, cleared | holding |
| 10 | Outpost | year 6 mice | open sacks | sealed bins | holding |

Ten site histories with first cases, causes, and current status, and the
status column keeps its promises: six holdings means six sites that are being
kept, not six sites that are finished. The two shared rows are marked as
shared forever, which is the honest label for a truce that will outlive the
person who signed it.

---

## 26. APPENDIX M — MAIN QUESTLINE STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_uninvited_sacks` | 3 | Intake counted |
| `quest_uninvited_pantry` | 4 | Weevils found |
| `quest_uninvited_rounds` | 4 | Rounds established |
| `quest_uninvited_traps` | 4 | Trap line set |
| `quest_uninvited_granary` | 4 | Granary rebuilt |
| `quest_uninvited_quarantine` | 3 | Shelf opened |
| `quest_uninvited_mold` | 5 | Vent cleared |
| `quest_uninvited_cellar` | 4 | Cellar re-lined |
| `quest_uninvited_burrow` | 4 | Burrow resolved |
| `quest_uninvited_hornets` | 5 | Quarry decided |
| `quest_uninvited_bridge` | 4 | Bridge tolerated |
| `quest_uninvited_guardian` | 3 | Guardian placed |
| `quest_uninvited_printworks` | 5 | Floor rebuilt |
| `quest_uninvited_loss_stone` | 3 | Figures cut |
| `quest_uninvited_habits` | 3 | Prevention habit |

---

## 27. APPENDIX N — SIDE QUEST STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_uninvited_count` | 3 | Counts taken |
| `quest_uninvited_bait_map` | 3 | Map made |
| `quest_uninvited_early_sign` | 3 | Sign logged |
| `quest_uninvited_night_check` | 3 | Night check |
| `quest_uninvited_round_card` | 3 | Card kept |
| `quest_uninvited_screen` | 3 | Screens fitted |
| `quest_uninvited_gap` | 3 | Gaps sealed |
| `quest_uninvited_raised` | 3 | Pallets built |
| `quest_uninvited_bin` | 3 | Bins sealed |
| `quest_uninvited_rotation` | 3 | Rotation running |
| `quest_uninvited_line` | 3 | Line set |
| `quest_uninvited_check` | 3 | Daily check |
| `quest_uninvited_humane` | 3 | Humane rules |
| `quest_uninvited_evidence` | 3 | Evidence logged |
| `quest_uninvited_disposal` | 3 | Disposal proper |
| `quest_uninvited_gear` | 3 | Gear ready |
| `quest_uninvited_pair` | 3 | Pair rule kept |
| `quest_uninvited_cellar_clear` | 4 | Cellar cleared |
| `quest_uninvited_vent_clear` | 4 | Vent cleared |
| `quest_uninvited_debris` | 3 | Debris removed |
| `quest_uninvited_harvest` | 3 | Harvest rules |
| `quest_uninvited_leave` | 3 | An edge left |
| `quest_uninvited_boundary` | 3 | Boundary set |
| `quest_uninvited_hornet_truce` | 4 | Truce held |
| `quest_uninvited_share_record` | 3 | Shared recorded |
| `quest_uninvited_history` | 3 | History written |
| `quest_uninvited_lesson` | 3 | Lesson taught |
| `quest_uninvited_loss` | 3 | Loss figured |
| `quest_uninvited_stone` | 3 | Stone cut |
| `quest_uninvited_review` | 4 | Review held |

---

## 28. APPENDIX O — NPC DOSSIERS (BRIEF)

**Wilm Branwell** — pest officer. Fought alone for years with a bad lamp and
learned that the war is mostly punctuation: gaps, lids, dates, and rounds.
Believes a shelter cannot win the war, only keep the numbers small.

**Zanna Mireley** — stores inspector. Checks the same shelf the same way and
can find a tear in a sack by running two fingers along it. Believes a standard
is a kindness to the person who inherits the store.

**Zel Hasken** — trap maker. Builds snap traps, cage traps, and sticks that
never see a board. Believes a trap that is not checked is a cruelty, not a
tool.

**Bexley Garth** — granary keeper. Raised the floor, sealed the bins, and
counts the sacks every week without being asked. Believes a pantry is a
promise about the winter.

**Dunlin Greve** — burrow runner. Crawls into places nobody else fits and
comes out with a map and a complaint. Believes most animals would rather leave
than fight, and most of the job is giving them a door.

**Roslin Nix** — quarantine aide. Labels every sack, runs the timers, and
releases nothing early. Believes patience is the cheapest pesticide.

**Otta Pitt** — kitchen vigilance. Runs a spotless dry corner and a fat lid
regime that the whole kitchen resents and copies. Believes the kitchen is the
first line.

**Fenna Kett** — apprentice. Sixteen, supervised, counts the tray rows every
morning and writes the numbers in a careful column nobody read until it saved
the spring. Believes counting is a way of caring.

---

## 29. APPENDIX P — LOCATION DETAIL

- **The Pantry Shed** — sacks, shelves, and the first honest count.
- **The Cider Cellar** — old press, new lime, and no more rot.
- **The Burrow Field** — holes, nets, and a relocation.
- **The Quarry Face** — a swarm older than the shelter and a truce older
  than the swarm.
- **The Rust Bridge** — a fungus that holds what the bridge lost.
- **The Printworks** — a rebuilt floor over an old king's hall.
- **The Ordnance Stack** — crates gone, contamination gone.
- **The Nest House** — a wall closed and a bed moved.
- **The Tray Rows** — green rows and a morning count.
- **The Loss Stone** — a smaller number every year. 

---

## 30. APPENDIX Q — PANTRY CHARTER

| Clause | Promise |
|---|---|
| Watch | Every store has a round and a count |
| Seal | Bins, floors, screens, and gaps are checked |
| Catch | Traps are mechanical, humane, and checked daily |
| Clean | Clearance is paired, geared, and disposed properly |
| Share | Tolerated edges have boundaries, keepers, and reviews |
| Wait | Incoming goods quarantine on a timer |
| Care | Guardians are fed, checked, and kept from traps |
| Record | Every case has a cause and a lesson |
| Count | The annual loss is cut into the stone |
| Honest | No poison, no torture, no story we would not tell a child |

The pantry charter is the expansion's first-class design object, posted in the
granary where every keeper reads it at the top of a round. Its last clause is
the one that binds the whole design: the shelter's war for its food is
conducted in a way it would be willing to explain to the youngest person
living in it.

---

## 32. APPENDIX R — WORKED PANTRY YEAR

**Month one.** The salvage grain arrives in eleven sacks and Roslin puts every
one of them on the quarantine shelf. Six days later the first sack is opened,
and by the fourth week the pantry shelf is moving. Wilm names the affair
pantry weevils in the count book, and the shelter stops arguing about whether
it has a pest problem.

**Month two.** Zanna runs the first full round with a card and finds three
torn sacks, a gap under the pantry door, and a cardboard box that should not
be there. The standards go up on the wall the same week, and Bexley starts
building raised pallets before the next delivery arrives.

**Month three.** Zel sets the first trap line and puts a check card beside it.
Six checks in, the count drops from nineteen to four, and the card proves that
the line is doing something rather than simply existing.

**Month four.** The granary rebuild begins: raised floor, sealed bins, a dry
corner with a humidity card. The flour bin gets a gasket. Otta posts the fat
lid rule in the kitchen and is resented for exactly eleven days and copied
thereafter.

**Month five.** The vent mold blooms above the laundry. Dunlin and Wilm clear
it in three days with masks and lamps, the filter schedule is changed, and the
spore cases that reach the clinic are exactly two, both mild, both traced by
the disease port rather than by guesswork.

**Month six.** The cider cellar is found rotten behind the old press. The crew
scrapes, limes, and re-lines it in four days, and the cellar's history begins
with the sentence: rot found in the corner nobody used.

**Month seven.** Dunlin maps the molerat burrow, and the shelter relocates
eleven animals with nets and grain rather than trapping them. The burrow is
sealed with rubble and the field edge gets a barrier, and Dunlin writes the
line that becomes the shelter's pest motto: most animals would rather leave.

**Month eight.** The hornet argument happens. The swarm has been at the quarry
face for longer than the shelter has existed, and clearing it means smoke,
veils, risk, and the loss of the only honeycomb within a day's walk. The
shelter signs a truce: quarry face only, harvest in late autumn, annual review,
Dunlin as keeper.

**Month nine.** The fungal carpet on the rust bridge is inspected and
monitored rather than scraped, because the fungus is holding the bank together.
Checks go into the schedule twice a season, and the bridge becomes the first
shared edge the shelter records as an asset instead of an enemy.

**Month ten.** A working cat is requested from the companion system. Ash
arrives, sleeps on the granary bins, is fed by Bexley, checked monthly, and is
kept out of the trap zone by a map on the door.

**Month eleven.** The printworks floor is opened, cleared, and rebuilt over
six days, and the rat king's case file ends with a floor plan and a lesson:
paper stored in cardboard is a rat's larder with a lid.

**Month twelve.** The annual figures are computed, and the loss stone is cut
with the year and the number. The number is small. Wilm reads the round cards
aloud at the stone, and Fenna's cutworm column is read last, and it is the
column that saved the spring.

---

## 33. APPENDIX S — VIGNETTES (TONE SAMPLE)

> Zel kneels at the trap line and checks each set in the same order, and when
> someone asks why the order matters, he says: because a missed check is a
> dead animal nobody was counting, and that is not who we are.

> Roslin labels the eleventh sack and sets the timer and says to the porter:
> seven days, not six, and the porter complains, and the weevils arrive in a
> sack that came through the shelf, and nobody complains again.

> Dunlin comes out of the burrow covered in dust and says there are eleven in
> there and they are terrified, and the shelter spends a day moving them
> instead of killing them, and the count book records it without sentiment
> because the count book is for numbers.

> Fenna counts the tray rows on a cold morning and writes the column and
> nobody reads it until May, when the cutworm line explains the survival of
> half the seedlings, and then suddenly everybody reads the column.

---

## 34. APPENDIX T — FAILURE MODES AND RECOVERY

| Failure | Effect | Recovery |
|---|---|---|
| No round | late discovery | round board |
| Open sacks | imports | sealed bins |
| Cardboard | rat larder | paper boxes |
| No quarantine | weevil wave | shelf and timer |
| Unchecked trap | suffering | daily check |
| Poison temptation | danger | banned by charter |
| Tolerated forever | unchecked edge | keeper and review |
| Uncleared mold | spores | dry and filter |
| Guardian untended | animal suffers | feeding rota |
| Loss unrecorded | vanished food | annual figure |

Recovery is a habit in every row, and the plan's position is that the war is
not won by any single campaign but by the boring sequence of rounds, lids,
dates, and checks that a shelter either keeps or slowly stops keeping.

---

## 35. APPENDIX U — CONTENT REVIEW CHECKLIST

- [ ] No second ecology, disease, food, or ventilation system.
- [ ] Triggers, recurrence, and loss stay on the live seeded path.
- [ ] Food loss respects `MaxFoodLossPerDay`.
- [ ] Spore effects route through the Plan 09 disease port only.
- [ ] No poison, fumigation, or chemical pest control exists.
- [ ] Traps are mechanical, checked, and never sticky boards.
- [ ] Tolerance edges have boundaries, keepers, and reviews.
- [ ] Guardian animals route through the companion and vet owners.
- [ ] Quarantine applies to goods, never people.
- [ ] Save additions are additive inside `ecological_infestation`.

---

## 36. APPENDIX V — GLOSSARY

- **Affair** — one authored infestation with a lifecycle.
- **Round** — an inspection route with stops and a card.
- **Count** — a number taken at a stop on a schedule.
- **Standard** — a checkable storage rule.
- **Trap line** — a set sequence of traps checked in order.
- **Clearance** — paired, geared work to end a case.
- **Tolerance** — a bounded, owned, reviewed truce with an edge.
- **Quarantine shelf** — where incoming goods wait out a timer.
- **Guardian** — a working animal owned and cared for elsewhere.
- **Loss stone** — the annual figure, cut where everyone can read it.

---

## 37. APPENDIX W — INTEGRATION MATRIX

| System | Reads | Writes | Never writes |
|---|---|---|---|
| `EcologicalInfestationSystem` | seasons | case state | food |
| `PestWatchSystem` | stores | rounds, counts | cases |
| `SealingSystem` | inventory | standards | cases |
| `TrapSystem` | store | trap state | cases |
| `ClearanceTeamSystem` | gear | teams | cases |
| `ToleranceSystem` | cases | edges | cases |
| `QuarantineSystem` | arrivals | shelf | inventory |
| `GuardianLinkSystem` | companions | placements | companions |
| `InfestationRecordSystem` | history | records | cases |
| `FoodPreservationSystem` | nothing | nothing | nothing |
| Disease port | spores | nothing | nothing |
| `VentilationSystem` | wear | nothing | nothing |
| `CompanionAnimalSystem` | placements | nothing | nothing |
| `VeterinarySystem` | checks | nothing | nothing |
| `ShelterWorkshopSystem` | builds | nothing | nothing |
| `PressHostSession` | cards | prints | nothing |
| `Inventory` | items | nothing | nothing |
| `StandingRecord` | records | records | nothing |

---

## 38. APPENDIX X — DATA SCHEMA DETAIL (NEW CATALOGS)

**`pest_rounds.json`** — `round_id`, `display_name`, `stops[]`, `check[]`,
`cadence`, `staff`, `tags[]`.

**`pest_counts.json`** — `count_id`, `site_id`, `kind`, `number`, `trend`,
`card_day`, `note`, `tags[]`.

**`pest_storage.json`** — `standard_id`, `applies_to[]`, `check`, `risk`,
`review`, `tags[]`.

**`pest_traps.json`** — `trap_id`, `kind`, `bait`, `check_cadence`, `humanness`,
`disposal`, `tags[]`.

**`pest_clearance.json`** — `case_id`, `method`, `gear[]`, `pair_rule`,
`duration_days`, `disposal_route`, `tags[]`.

**`pest_tolerance.json`** — `edge_id`, `kind`, `boundary`, `harvest_limit`,
`review_date`, `keeper_id`, `tags[]`.

**`pest_quarantine.json`** — `class_id`, `goods`, `timer_days`, `check`,
`release`, `label`, `tags[]`.

**`pest_guardians.json`** — `guardian_ref`, `store_id`, `feeder_id`,
`vet_check`, `note`, `tags[]`.

**`pest_records.json`** — `case_id`, `site_id`, `cause`, `action`, `lesson`,
`year`, `keeper_id`, `tags[]`.

**`pest_loss.json`** — `year`, `food_lost`, `prevention_cost`, `figure`,
`note`, `tags[]`.

All new catalogs carry `schema_version: 1` and fail the integrity gate on
missing or duplicate IDs, invalid item or site references, or out-of-range
numbers.

---

## 39. APPENDIX Y — MEASUREMENT AND TELEMETRY

| Metric | Purpose | Source |
|---|---|---|
| Rounds completed | discipline | Rounds |
| Counts taken | watchfulness | Counts |
| Cases opened | load | Cases |
| Cases cleared | action | Cases |
| Days to clear | efficiency | Cases |
| Tolerated edges | policy | Tolerance |
| Trap checks missed | humanness | Traps |
| Quarantine releases | safety | Quarantine |
| Food loss per year | honest figure | Loss |
| Guardian checks | care | Guardians |

Telemetry is diagnostic only; it never gates content and never ranks a
storekeeper or a trapper.

---

## 40. APPENDIX Z — IMPLEMENTATION CHECKLIST

- [ ] Phase 0 premise re-audit recorded with file:line evidence.
- [ ] Phase 1 catalogs authored and registered with validators and scanner.
- [ ] Phase 2 Core systems are pure, engine-free, and deterministic.
- [ ] Phase 3 save additions are additive inside `ecological_infestation`.
- [ ] Triad parity (Setup / Save / Flush) holds for every new state.
- [ ] Phase 4 host session exposes live state and real commands only.
- [ ] Phase 5 panels pass bind/unbind/rebind, focus, contrast, and scaling.
- [ ] Phase 6 content passes the review checklist in §35.
- [ ] Phase 7 soak shows prevention lowering triggers and tolerance holding.
- [ ] Phase 8 data integrity and content utilization selftests pass.
- [ ] No poison, cruelty, or people-quarantine content exists.

---

## 41. APPENDIX AA — OPEN QUESTIONS FOR REVIEW

1. Does a guardian animal ever catch an infestation, and how is that shown?
2. Can a tolerated edge ever be cancelled, and what happens to the harvest?
3. Are traps allowed in the same room as children's bedding?
4. Who inspects the inspector's own quarters?
5. Does the outpost run its own rounds or rely on visitors?
6. Can quarantine be extended beyond the timer, and who decides?
7. Is the loss stone ever cut with a bad year, and does anyone explain it?
8. What happens to a burrow that is cleared and comes back?

None of these may be decided unilaterally; each changes tone and balance.

---

## 42. APPENDIX AB — CROSS-WAVE HOOKS

| Wave | Expansion | Hook |
|---|---|---|
| 1 | 15 The Deep Root | Crop pests and seed stores |
| 1 | 16 The Rebuilt Body | Gloves, masks, and coveralls |
| 2 | 19 The Bitter Air | Vent mold and filters |
| 3 | 22 The Clean Flow | Waste, debris, and hygiene |
| 3 | 26 The Common Table | Food loss and preservation |
| 4 | 27 The Thread | Cloth bales and bedding checks |
| 4 | 30 The Press | Cards, labels, and paper storage |
| 4 | 31 The Kiln | Stoneware bins and lime |
| 5 | 32 The Wild | Guardian animals requested |
| 5 | 34 The Long Road | Incoming goods quarantine |
| 5 | 36 The Watch | Site surveys and burrows |
| 6 | 38 The Ward | Spore cases through the disease port |
| 6 | 39 The Reagent | Cleaning agents ordered |
| 7 | 44 The Outpost | Remote rounds and stores |
| 7 | 46 The Long Change | Edges that move with the world |
| 8 | 47 The Brigade | Nesting fire loads reported |
| 8 | 50 The Vault | Paper pests and climate |
| 9 | 52 The Warm Ground | Warm rooms and damp corners |
| 9 | 55 The Quarter | Dorm checks and shared walls |

Each hook is additive. The Uninvited can ship alone, and every other expansion
can ship without it.

---

## 43. APPENDIX AC — ENDING PROSE SKETCHES

**The Sealed Granary.** Storage standards hold, losses fall to almost nothing,
and the pantry stops making news.

**The Watched Shelf.** The shelter cannot eliminate pests but always knows
their numbers, and the annual loss is small and honest.

**The Tolerated Edge.** The hornets, the fungal carpet, and the orchard edge
are shared with rules, harvests, and respect.

**The Clean Vent.** The mold war is won with dry air, cards, and a filter
schedule, and the laundry stops coughing.

**The Lesson Kept.** Every case is recorded with its cause and fix, and the
shelter's history of pests becomes its best prevention.

**Fade.** A granary in the morning, sealed bins, a trap line checked in the
same order every day, and a loss stone with a smaller number on it.

---

## 44. APPENDIX AD — COMMON FAILURE PATTERNS TO AVOID

| Pattern | Why it is bad | Correct approach |
|---|---|---|
| Kill-everything | hollow and cruel | tolerance path |
| Poison shortcut | dangerous | banned by charter |
| Sticky traps | cruelty | mechanical only |
| Monster framing | tone break | numbers and habits |
| People quarantine | harmful | goods only |
| Ignored guardians | neglect | feeding rota |
| Endless rounds | tedium | counts and cases |
| Loss hidden | dishonesty | loss stone |
| Ecology reimplementation | authority break | live owner |
| Disease shortcut | authority break | port only |

The list exists because pests are the easiest thing in a survival game to turn
into a grind or a cruelty. The expansion's rule is that the war is small,
counted, and humanely run, and that the best outcome is a boring pantry.

---

## 45. APPENDIX AE — CONTENT VOLUME SUMMARY

| Category | Rows | Prose estimate |
|---|---|---|
| Affairs | 10 | 2,500 |
| Rounds | 12 | 2,500 |
| Counts | 24 | 3,000 |
| Standards | 16 | 3,000 |
| Traps | 12 | 2,500 |
| Clearance | 10 | 3,000 |
| Tolerance | 8 | 2,000 |
| Quarantine | 10 | 2,000 |
| Guardians | 6 | 1,500 |
| Site histories | 10 | 3,000 |
| Quests | 53 | 15,000 |
| NPCs | 8 | 7,000 |
| Locations | 10 | 3,500 |
| Items | 16 | 2,500 |
| Endings | 6 | 3,000 |
| **Total** | | **~57,000** |

---

## 46. APPENDIX AF — LOSS YEAR TABLE

| # | Year | Cases | Food lost | Figure | Note |
|---|---|---|---|---|---|
| 1 | 1 | 4 | high | 94 | learning |
| 2 | 2 | 5 | high | 88 | granary |
| 3 | 3 | 4 | mid | 61 | standards |
| 4 | 4 | 3 | mid | 44 | traps |
| 5 | 5 | 3 | low | 31 | quarantine |
| 6 | 6 | 2 | low | 22 | rounds |
| 7 | 7 | 3 | low | 27 | bad winter |
| 8 | 8 | 2 | low | 18 | shared edges |
| 9 | 9 | 2 | low | 15 | habit |
| 10 | 10 | 1 | low | 11 | quiet |

Ten years of honest figures, and the shape is the expansion's whole argument:
losses fall because of standards, traps, quarantine, and rounds, and the bad
year in row seven is included because a plan that shows only improvement is a
brochure, not a record. The stone gets the number and the year and nothing
else, and that is enough.

---

## 47. APPENDIX AG — FIRST YEAR OF THE PANTRY

| Month | Focus | Milestone |
|---|---|---|
| 1 | Sacks | quarantine shelf |
| 2 | Pantry | weevils named |
| 3 | Rounds | cards up |
| 4 | Traps | line running |
| 5 | Granary | rebuilt |
| 6 | Mold | vent cleared |
| 7 | Cellar | re-lined |
| 8 | Burrow | relocated |
| 9 | Hornets | truce signed |
| 10 | Guardian | cat placed |
| 11 | Printworks | floor rebuilt |
| 12 | Stone | figure cut |

A year from moving shelves to a cut stone, and the order is deliberate: the
shelter puts its storage in order before it makes any truce or cuts any
figure, because a policy made before prevention is just an opinion with a
nickname.

---

## 48. APPENDIX AH — BAIT STATION MAP TABLE

| # | Station | Site | Cadence | Reading | Trend |
|---|---|---|---|---|---|
| 1 | B-01 | granary door | weekly | 2 | down |
| 2 | B-02 | pantry rear | weekly | 5 | down |
| 3 | B-03 | cold room | weekly | 0 | flat |
| 4 | B-04 | kitchen dry | weekly | 1 | flat |
| 5 | B-05 | dorm wall | monthly | 3 | down |
| 6 | B-06 | vent base | weekly | 0 | flat |
| 7 | B-07 | printworks | monthly | 6 | falling |
| 8 | B-08 | tray rows | weekly | 2 | seasonal |

Eight bait stations with readings and trends, and the printworks row is the
one that shows the whole system working: a high reading in month three, a
cleared floor in month eleven, and a falling trend that proves the count book
and the clearance case are the same story told twice.

---

## 49. APPENDIX AI — COUNT BOOK TABLE (SAMPLE WEEK)

| # | Day | Granary | Pantry | Printworks | Note |
|---|---|---|---|---|---|
| 1 | Mon | 2 | 4 | 6 | after rain |
| 2 | Tue | 1 | 3 | 6 | sealed bin |
| 3 | Wed | 1 | 3 | 5 | lid found |
| 4 | Thu | 0 | 2 | 5 | gap sealed |
| 5 | Fri | 0 | 2 | 4 | trap line |
| 6 | Sat | 0 | 1 | 4 | check |
| 7 | Sun | 0 | 1 | 3 | review |

Seven days of counts from three stores, and the shape of the week is the whole
expansion in miniature: numbers fall when a gap is sealed and a lid is found,
and rise again only when it rains, which is not a failure but a fact that the
count book is able to record instead of denying.

---

## 50. APPENDIX AJ — PANTRY COVENANT

| Clause | Promise |
|---|---|
| Watch | Every store has a round and a count, and the rounds are kept |
| Seal | Bins, screens, sweeps, and gaps are checked against a standard |
| Check | Traps are set and checked, and no set is left unattended |
| Humane | Mechanics and relocation before killing, always |
| Share | A tolerated edge is owned, bounded, and reviewed |
| Wait | Incoming goods quarantine before they touch the stores |
| Care | Guardians are fed, vetted, and kept away from traps |
| Record | Every case keeps its cause and its lesson |
| Tell | The annual loss is cut into the stone and read aloud |
| Clean | The war is fought with lids, dates, and soap, not poison |

The pantry covenant is the expansion's first-class design object, and its last
clause is a practical instruction as much as an ethic: lids, dates, and soap
are what actually keep a shelter's food, and the drama of the work is
deliberately small.

---

## 51. APPENDIX AK — PANTRY SUCCESSION TABLE

| Role | First | Successor | Handover |
|---|---|---|---|
| Pest officer | Wilm | Zanna | one full year of rounds |
| Stores inspector | Zanna | Bexley | one intake season |
| Trap maker | Zel | Dunlin | one line check |
| Granary keeper | Bexley | Otta | one rotation week |
| Burrow runner | Dunlin | apprentice | one survey |
| Quarantine aide | Roslin | Bexley | one shelf week |
| Kitchen vigilance | Otta | cook | one dry corner |
| Apprentice | Fenna | next recruit | one count season |

The succession table is measured in the units the work actually uses — a year
of rounds, an intake season, a line check, a count season — because a pantry
changes hands through habit rather than a handover meeting. The pest officer's
successor takes a full year, and the table says so, because the person who
inherits the count book should have seen winter once through its pages.

---

## 52. APPENDIX AL — HOUSEHOLD ODDS TABLE

| # | Odds | Where | Fix | Checked by |
|---|---|---|---|---|
| 1 | Gap under door | all stores | sweep | Zanna |
| 2 | Tear in mesh | vents, windows | patch | Wilm |
| 3 | Wet corner | laundry, cellar | dry, vent | Otta |
| 4 | Open sack | pantry | seal or use first | Bexley |
| 5 | Cardboard box | food rooms | replace | Zanna |
| 6 | Fat without lid | kitchen | lid | Otta |
| 7 | Warm pipe gap | dorm wall | seal | Dunlin |
| 8 | Paper on floor | printworks | rack | Zel |
| 9 | Standing water | cellar, sump | pump, drain | Bexley |
| 10 | Crumb line | pantry shelf | sweep | Fenna |
| 11 | Unlabelled parcel | quarantine | label or isolate | Roslin |
| 12 | Trap set alone | any line | pair or barrier | Zel |
| 13 | Guardian bowl empty | any store | feeder checks | owner |
| 14 | Nest in bedding | dorms | wash, seal | Sil tie |
| 15 | Bin gasket dry | granary | grease or replace | Bexley |
| 16 | Roof drip | stores | patch | workshop |

Sixteen small things, each of which is how a real infestation starts, and the
checked-by column gives every one of them a name. The expansion's argument is
that pest control is not a heroic campaign but a list of household habits, and
the list fits on one card that hangs beside the granary door.

---

## 53. CLOSING STATEMENT

ASHFALL already models infestations with real restraint: ten authored affairs
with triggers, clear costs, disease-port effects, ventilation wear, a hard cap
on daily food loss, and three cases with a bounded tolerated-harvest path. What
it lacks is the practice around the lifecycle: rounds, counts, sealing,
traps, clearance teams, quarantine, tolerance policy, guardian coordination,
histories, and a loss stone. The Uninvited adds that practice and takes a
position the genre rarely does — that some edges are better shared than
conquered, and that a shelter's honesty about its own pantry is the first
defense of its food. It adds a trap line checked in the same order every
morning and a stone with a smaller number cut into it.

> Wave 9 note: this plan is one of five Wave 9 expansion bibles (52–56). Each is
> self-contained; none requires another to ship. The shared Wave 9 index lives
> at `docs/expansions/wave9/WAVE9_INDEX.md`. The safe pre-signature step is
> Phase 1 (data schemas and validators), which is additive and reversible.
> Evidence anchors: `EcologicalInfestationSystem` (`SystemId` =
> "ecological_infestation", `RecurrenceCooldownDays = 5`,
> `MaxFoodLossPerDay = 3`, `LoadDefinitions`, `IsActive`,
> `IsEligibleToTrigger`, `TryTrigger`, `TryClear`, `TryTolerateAndHarvest`,
> `TickDay`, `RestoreState`), `EcologicalInfestationDefs` and
> `EcologicalInfestationCatalog`, the `ecological_infestation` save section
> with `EcologicalInfestationSaveStore`, and
> `ecological_infestations.json` (ten affairs: subway molerat burrow, quarry
> hornet swarm, cider-press cellar rot, ordnance roach cluster, bridge fungal
> carpet, printworks rat king; vent mold, pantry weevils, wall nest, tray
> cutworm) whose notes document the host-callback effects into food loss, the
> Plan 09 disease port, and ventilation wear.