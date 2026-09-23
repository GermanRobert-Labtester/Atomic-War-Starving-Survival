# ASHFALL — Expansion 32 Design Bible
# THE WILD
### Wave 5 · Wildlife, Ecology, Hunting, Trapping, Migration, and Companion Animals

**Document status:** Design plan (pre-integration). Not a claim. Not an authorization.
**Date:** 2026-09-22
**Domain owners touched:** `Ashfall.Core.World` (WildlifeEcosystemSystem, WildlifeMigrationSystem), `Ashfall.Core` (WildlifeTrappingSystem), `Ashfall.Core.Ecology` (CompanionAnimalSystem), `Ashfall.Core` (WildlifeSeasonalCalendar)
**Proposed host owner:** `WildlandsHostSession` (extends trapping, ecosystem, and companion surfaces)
**Existing save sections:** wildlife trapping state, ecosystem state, companion state, migration state
**Existing CLI verbs:** `--wildlife-trapping-selftest`, `--data-integrity-selftest`, `--content-utilization-selftest`
**Rule compliance:** Godot authoritative; Core engine-free; JSON data authoritative; one authority per concern; deterministic seeded RNG.

---

## 0. HOW TO READ THIS DOCUMENT

This is a **design bible**, not an integration plan. Implementation must later pass
through `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `TEST_POLICY.md`.

ASHFALL already runs a real ecology. `WildlifeEcosystemSystem` (32 KB) defines
`FaunaSpeciesDef`, `PredatorPreyEdgeDef`, `SeasonalMoveDef`, `PressureKeyState`,
`ApexActivityState`, `DomesticAnimalState`, `WildlifeObservation`, and
`WildlifeEcosystemState`; it exposes `SectorSpeciesPopulation`,
`IsLocallyExtinct`, `SectorDensityMultiplier`, `RecordHuntingPressure`,
`PressureOn`, `RecordObservation`, `ObservationCount`, `KnowledgeLevel`, and
`CanTame`, and it ticks daily. `WildlifeMigrationSystem` tracks packs and moves
them. `WildlifeTrappingSystem` runs trap sites with assigned hunters, bait
types, trap types (snare, deadfall, cage, pit), durability, breakage, and
separate seeded RNG streams for deployment, encounters, and incidents.
`CompanionAnimalSystem` (37 KB) loads companion species profiles, assigns and
feeds companions, and resolves results with reason codes.
`WildlifeSeasonalCalendar` and `WildlifeTrappingEvents` exist,
`WildlifeTrappingLocalization` ships, and `WildlifeTrappingCatalog` consumes a
23 KB data file. The ecosystem catalog itself is `wildlife_ecosystem.json` at
3.8 KB — the entire food web of the world in a single small file.

What does not exist: hunting as a discipline, seasons and quotas that the player
sees, evidence and tracking, population pressure the player can read, predator
conflict with the shelter, taming progression as content, working animals,
culling and toxic removal as authored events, and any real field knowledge
system beyond a knowledge-level string.

**The Wild** turns that machinery into the shelter's relationship with the land:
what lives out there, what it needs, what the shelter takes, and what comes
back when the taking stops.

Conventions: **`LIVE`** confirmed in source/data; **`GAP`** confirmed thin;
**`PROPOSED`** new.

---

## 1. EXECUTIVE SUMMARY

### 1.1 Pitch

The shelter can measure its pantry, its water, and its fuel. It cannot measure
the forest — and the forest is where the meat comes from.

**The Wild** is the expansion about the living world beyond the gate: species,
predators, prey, seasons, migration, hunting, trapping, taming, and the
consequences of taking too much. It extends the live ecosystem, migration,
trapping, and companion systems with authored species content, hunting and
tracking gameplay, quotas and pressure, predator conflicts, companion work
roles, and field knowledge that the shelter earns by observing rather than by
reading.

The expansion's hard rules follow the live owners: `WildlifeEcosystemSystem`
remains the population authority, `WildlifeMigrationSystem` moves packs,
`WildlifeTrappingSystem` owns trap sites and seeded rolls, `CompanionAnimalSystem`
owns taming and feeding, and all contamination routes through the radiation and
disease pipelines. No second ecology, hunting, or companion system is created.

### 1.2 The five loops it adds

```
   Observe ──► Know ──► Hunt / Trap ──► Take
      │           │           │            │
      ▼           ▼           ▼            ▼
   Field       Species     Seasons,     Pressure ──► Population
   notes,      knowledge   quotas       recorded    falls
   tracks                                     │
                                              ▼
   Predators ◄── Prey crash ◄── Overhunt ──► Migration in
      │
      ▼
   Conflict ──► Deterrence, culling, coexistence
      │
      ▼
   Companion ──► Tame, feed, work, breed
```

### 1.3 What the player manages

1. **Knowledge.** Which species, where, when, and what they need.
2. **Hunting.** Stalking, scent, wind, range, shot placement, recovery, and
   spoilage.
3. **Trapping.** Sets, bait, checks, durability, bycatch, and humane practice.
4. **Seasons and quotas.** Closed seasons, breeding windows, and take limits
   that the ecology actually enforces.
5. **Pressure.** Hunting pressure per sector and species, visible in the
   ecosystem, with local extinction as a real outcome.
6. **Predators.** Apex activity, livestock loss, deterrence, and the choice to
   cull or coexist.
7. **Companions.** Taming, feeding, bonding, work roles, and breeding.
8. **Field study.** Observation, tracking, and authored knowledge that improves
   the shelter's practice.

### 1.4 What it is not

- Not a second ecology. `WildlifeEcosystemSystem` remains the authority.
- Not a second trap system. `WildlifeTrappingSystem` keeps trap sites and rolls.
- Not a second companion system. `CompanionAnimalSystem` keeps taming and
  feeding.
- Not a hunting minigame with a zoomed rifle; hunting is authored encounters,
  stalking, and consequence.
- Not trophy content. The expansion explicitly refuses trophy hunting as a
  reward loop.
- Not a new save section.

---

## 2. EVIDENCE BASE AND GAP ANALYSIS

### 2.1 Live systems (verified)

| File | Role | Status |
|---|---|---|
| `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs` | Species, predator-prey, pressure, knowledge, taming | `LIVE` |
| `Assets/Ashfall.Core/WildlifeMigrationSystem.cs` | Pack movement | `LIVE` |
| `Assets/Ashfall.Core/WildlifeTrappingSystem.cs` | Trap sites, bait, durability, seeded rolls | `LIVE` |
| `Assets/Ashfall.Core/WildlifeTrappingEvents.cs` | Trapping events | `LIVE` |
| `Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` | Companion assign, feed, results | `LIVE` |
| `Assets/Ashfall.Core/WildlifeSeasonalCalendar.cs` | Seasonal windows | `LIVE` |
| `Assets/Ashfall.Core/WildlifeTrappingCatalog.cs` | Trap catalog | `LIVE` |
| `Assets/Ashfall.Core/Localization/WildlifeTrappingLocalization.cs` | Strings | `LIVE` |

### 2.2 Live data (counted)

| Catalog | Size | Notes |
|---|---|---|
| `wildlife_trapping_catalog.json` | 23.2 KB | traps and bait |
| `wildlife_ecosystem.json` | **3.8 KB** | entire food web |
| Ecosystem predator/prey content | thin | few authored edges |
| Hunting/tracking/quota data | none | confirmed absent |
| Companion work/breeding data | none | confirmed absent |

### 2.3 Confirmed gaps

- **GAP-32-1 — The food web is one small file.** Species, edges, and seasonal
  moves are barely authored.
- **GAP-32-2 — No hunting gameplay.** No stalking, scent, wind, tracking, or
  recovery.
- **GAP-32-3 — No quotas or seasons the player sees.** The calendar exists;
  take limits do not.
- **GAP-32-4 — Pressure is recorded but not authored or communicated.** The
  player cannot read the consequences before they land.
- **GAP-32-5 — No predator conflict content.** Apex activity is a state, not a
  story.
- **GAP-32-6 — No companion content.** Taming exists; profiles, work roles, and
  bonding content are thin.
- **GAP-32-7 — No field knowledge content.** `KnowledgeLevel` is a string with
  nothing behind it.
- **GAP-32-8 — No toxic or contaminated wildlife content** despite the
  radiation setting.
- **GAP-32-9 — No locations for the wild.**

### 2.4 Non-duplication statement

This expansion will **not** add a second ecosystem, migration, trap, companion,
disease, or radiation system. It extends `WildlifeEcosystemSystem` with authored
species, edges, and observations; extends `WildlifeTrappingSystem` with authored
sets and quotas; extends `CompanionAnimalSystem` with work roles and breeding;
extends `WildlifeMigrationSystem` with corridors and events; and routes all
contamination through the existing radiation and disease pipelines. It adds
state only as additive sub-objects of the existing wildlife stores. No new save
section.

---

## 3. DESIGN PILLARS AND TONE

### 3.1 Pillars

**Pillar 1 — The land is a ledger.** Every animal taken is entered somewhere,
and the wild keeps the account.

**Pillar 2 — Knowledge is earned outdoors.** The shelter learns species by
watching, tracking, and being wrong, not by reading a codex.

**Pillar 3 — Subsistence, not sport.** Hunting is food, hide, bone, and safety.
The expansion never rewards killing for its own sake.

**Pillar 4 — Predators are neighbors with teeth.** Apex species are a risk and a
signal; the shelter chooses deterrence, coexistence, or removal, and each has a
cost.

**Pillar 5 — Companions are partners, not equipment.** A working animal eats,
tires, bonds, and can die.

### 3.2 Tone calibration

| Element | Do | Do not |
|---|---|---|
| A hunt | Patience, wind, one clean act | Power fantasy |
| A trap line | Cold work, consequence | Gore |
| Extinction | Absence, silence | Punishment spectacle |
| A companion death | Grief and function lost | Sentimental overload |
| A predator at the fence | Fear and respect | Monster |
| Field notes | Careful, imperfect | Encyclopedia |

### 3.2 Content limits

- No trophy hunting, no mounted heads as rewards, no kill-count scoring.
- No torture or baiting for entertainment.
- Predators are animals, never monsters or villains.
- Contaminated wildlife is a body-horror risk handled with restraint.
- Hunting is authored as subsistence and safety, not sport or status.

---

## 4. THE WILDLANDS WORLD

### 4.1 Interior rooms

- **`room_butchery`** — dressing, hanging, and cutting.
- **`room_drying_rack`** — meat and hide drying.
- **`room_tannery_annex`** — the wildlife hides that feed the Thread.
- **`room_aviary`** — birds, eggs, and messages.
- **`room_kennel`** — working dogs and their quarters.
- **`room_byre`** — draft animals and bedding.
- **`room_field_room`** — maps, notes, and specimen shelves.
- **`room_freezer`** — cold storage for seasonal meat.

### 4.2 Exterior locations

| ID | Name | Danger | Purpose |
|---|---|---|---|
| `loc_old_growth` | The Old Growth | 5 | Deep forest and apex territory |
| `loc_browse_meadow` | The Browse Meadow | 3 | Deer, rabbits, and grazing |
| `loc_marsh_edge` | The Marsh Edge | 4 | Waterfowl, fish, and leeches |
| `loc_den_field` | The Den Field | 6 | Predator dens and risk |
| `loc_salt_lick` | The Salt Lick | 4 | Observation and ambush sites |
| `loc_migration_gap` | The Gap | 4 | Seasonal migration corridor |
| `loc_burnt_woods` | The Burnt Woods | 5 | Recovering habitat, thin game |
| `loc_poacher_camp` | The Poacher Camp | 5 | Unlicensed take and conflict |
| `loc_kennel_yard` | The Kennel Yard | 2 | Companion training |
| `loc_lookout_tree` | The Lookout Tree | 4 | Survey and observation |

All locations require valid item references and scanner registration.

### 4.3 The seasonal round

Spring breeding closures, summer abundance, autumn migration and harvest, winter
scarcity and predator pressure. The wild's calendar is the expansion's pacing,
and it runs on the live seasonal systems rather than a new clock.

---

## 5. MAIN STORYLINE — "WHAT THE WOODS KEPT"

### 5.1 Central conflict

The shelter's meat has been coming from the same two sectors for two years.
**Alder Quist**, the shelter's hunter, says the browse meadow is empty and the
trappers are catching juveniles. **Mira Gess** runs the trap line and insists the
take is fine; the numbers say she is wrong. When the deer crash, the apex
predators push closer to the shelter, and a working dog is killed at the fence.

At the same time, **Osa Reed** is keeping field notes nobody reads, and her
notes say the migration corridor has shifted because of a burn two valleys over.
If the shelter can move its take to the corridor in autumn and let the meadow
rest, the herd can come back. If it cannot, it will spend the winter eating its
breeding stock and fighting predators it created.

The expansion's question: **what does a shelter owe the land that feeds it?**

### 5.2 Theme (unspoken)

**Hunger that ignores the forest eventually eats the forest.**

### 5.3 Principal NPCs

| ID | Name | Role | Function |
|---|---|---|---|
| `npc_hunter_alder_quist` | Alder Quist | Hunter | Stalking, seasons, and quota discipline |
| `npc_trapper_mira_gess` | Mira Gess | Trapper | Trap lines, bait, and bycatch |
| `npc_naturalist_osa_reed` | Osa Reed | Naturalist | Observation, notes, and species knowledge |
| `npc_herder_tobin_skel` | Tobin Skel | Herder | Working animals, breeding, and care |
| `npc_child_kell` | Kell | Child | First tracks and first lessons |
| `npc_warden_dace` | Dace | Warden | Predator deterrence and safety |
| `npc_butcher_hem` | Hem | Butcher | Dressing, yield, and waste |
| `npc_trader_yarrow` | Yarrow | Hide trader | Pelts, bones, and prices |

### 5.4 Story beats (15)

1. **The Empty Meadow.** The take is falling; the trappers disagree.
2. **The Juvenile.** A young animal is caught; the line is questioned.
3. **The Count.** Osa's notes are read; a survey is proposed.
4. **The Corridor.** The migration gap is found and mapped.
5. **The Rest.** A closed season is proposed and argued.
6. **The Line.** Trap sets are repositioned; bycatch is addressed.
7. **The Quota.** Take limits are set and posted.
8. **The Dogs.** A companion is killed at the fence.
9. **The Apex.** Predator activity is tracked and deterrence chosen.
10. **The Old Growth.** A dangerous survey proves the corridor.
11. **The Poisoned.** Contaminated animals are found and handled.
12. **The Tame.** A companion is tamed and trained.
13. **The Autumn.** The first quota-led migration harvest.
14. **The Winter.** Scarcity tests the shelter's discipline.
15. **What the Woods Kept.** Final disposition of the wild.

### 5.5 Branching choices (8)

| Choice | Options | Axis |
|---|---|---|
| Take now | maximum / quota / closed | hunger vs. future |
| Predators | cull / deter / coexist | safety vs. balance |
| Trap line | extend / reposition / rest | yield vs. ecology |
| Companion | none / one / working pack | cost vs. capability |
| Study | invest / minimal / none | knowledge vs. time |
| Poisoned meat | use / test / discard | hunger vs. safety |
| Corridor | hunt / observe / protect | take vs. study |
| Final | wild as pantry / as partner / as trust | identity |

### 5.6 Endings (5 + fade)

1. **The Full Wood** — the herd recovers, the quota holds, and the shelter eats
   from a land that is still alive.
2. **The Managed Range** — trapping, quotas, and companions make the wild a
   working partner.
3. **The Quiet Meadow** — the meadow stays empty for years; the shelter eats
   stored food and remembers.
4. **The Predator Winter** — apex pressure and empty prey make the fence a front
   line.
5. **The Poisoned Season** — contamination enters the meat supply and costs the
   shelter dearly.
6. **Fade** — the same two sectors are hunted a little harder; nothing changes
   yet.

---

## 6. QUEST DESIGN

New IDs use prefix `quest_wild_`. Schema follows existing quest catalogs.

### 6.1 Main questline (15)

`quest_wild_empty_meadow`, `quest_wild_juvenile`, `quest_wild_count`,
`quest_wild_corridor`, `quest_wild_rest`, `quest_wild_line`, `quest_wild_quota`,
`quest_wild_dogs`, `quest_wild_apex`, `quest_wild_old_growth`,
`quest_wild_poisoned`, `quest_wild_tame`, `quest_wild_autumn`, `quest_wild_winter`,
`quest_wild_what_woods_kept`.

### 6.2 Side quests (30)

**Hunting (5)**
- `quest_wild_stalk` — stalk a deer without a shot
- `quest_wild_track` — follow a track to its bed
- `quest_wild_clean_shot` — one clean, respectful take
- `quest_wild_recover` — track and recover a wounded animal
- `quest_wild_scent` — wind and scent discipline

**Trapping (5)**
- `quest_wild_set_line` — set a correct line
- `quest_wild_bait_test` — test bait types
- `quest_wild_check` — timely trap checks
- `quest_wild_bycatch` — reduce bycatch
- `quest_wild_humane` — humane dispatch practice

**Ecology (5)**
- `quest_wild_survey` — sector survey
- `quest_wild_edge` — prove a predator-prey edge
- `quest_wild_extinct` — document a local extinction
- `quest_wild_recover_herd` — rest a sector and verify return
- `quest_wild_pressure_read` — read the pressure ledger

**Companions (5)**
- `quest_wild_tame_first` — tame a first animal
- `quest_wild_feed` — establish feeding
- `quest_wild_train` — train a work role
- `quest_wild_bond` — deepen a bond
- `quest_wild_breed` — breed a working line

**Field study (5)**
- `quest_wild_notes` — start field notes
- `quest_wild_knowledge` — raise species knowledge
- `quest_wild_season_map` — map seasonal moves
- `quest_wild_toxic_watch` — identify contaminated animals
- `quest_wild_share` — share findings with the shelter

### 6.3 Repeatable quests (8)

`quest_wild_repeat_trap`, `quest_wild_repeat_hunt`,
`quest_wild_repeat_survey`, `quest_wild_repeat_check`,
`quest_wild_repeat_feed`, `quest_wild_repeat_train`,
`quest_wild_repeat_quota`, `quest_wild_repeat_notes`.

### 6.4 Dynamic hooks

Live events (trap catches, breakage, migration ticks, ecosystem ticks, companion
feed results, seasonal windows, radiation surveys) attach authored follow-ups
through existing seams. No new event bus.

### 6.5 Constraints

- Population changes only through `WildlifeEcosystemSystem`.
- Takes are recorded through `RecordHuntingPressure`.
- Trap outcomes use the live seeded streams.
- Taming uses `CanTame` and `CompanionAnimalSystem` only.
- Contamination routes through radiation and disease pipelines.
- No infinite meat; no respawn without migration or recovery.
- No trophy, scoring, or sport framing.

---

## 7. NEW GAMEPLAY SYSTEMS

### 7.1 `HuntingSystem` (new, `Ashfall.Core.World`)

**Owns:** stalking, tracking, scent and wind, ranges, shot resolution, recovery
of wounded animals, and field dressing. **Consumes:**
`WildlifeEcosystemSystem`, `WildlifeMigrationSystem`, `Inventory`,
`SkillProgressionSystem`, `NeedsSystem`. **Data:** `hunting_grounds.json`,
`quarry_profiles.json`. **Rules:** hunting is an authored encounter with real
concealment and wind; a bad shot wounds and the animal runs; meat spoils with
time and temperature; every take is recorded as pressure.

### 7.2 `WildQuotaSystem` (new, thin, `Ashfall.Core.World`)

**Owns:** seasons, take limits, closures, and quota enforcement. **Consumes:**
`WildlifeEcosystemSystem` pressure, `WildlifeSeasonalCalendar`, `PolicySystem`.
**Data:** `quotas.json`. **Rules:** quotas are policy the shelter writes; the
ecology enforces the consequences; poaching is possible and recorded.

### 7.3 `PredatorConflictSystem` (new, `Ashfall.Core.World`)

**Owns:** apex activity events, livestock and companion predation, deterrence,
and culling decisions. **Consumes:** `WildlifeEcosystemSystem` apex state,
`CompanionAnimalSystem`, `Shelter` defenses, `NeedsSystem` morale, combat
systems for deterrence. **Data:** `predator_conflicts.json`.
**Rules:** predators act from real prey pressure; deterrence costs; culling has
ecological consequences; no monster framing.

### 7.4 `FieldStudySystem` (new, `Ashfall.Core.World`)

**Owns:** observation jobs, field notes, species knowledge progression, and
survey results. **Consumes:** `WildlifeEcosystemSystem.RecordObservation`,
`KnowledgeLevel`, `Inventory`, `DutyRoster`. **Data:** `field_notes.json`,
`species_profiles.json`. **Rules:** knowledge is earned by hours outdoors and
by being wrong; observations feed the live ledger; notes can be shared with the
school and the press.

### 7.5 `CompanionWorkSystem` (extend `CompanionAnimalSystem`)

**Owns:** work roles, training progression, bonding, breeding lines, and
retirement. **Consumes:** `CompanionAnimalSystem` assign/feed results,
`NeedsSystem`, `DutyRoster`, veterinary care through the medical pipeline.
**Data:** `companion_work.json`, `breeding_lines.json`.
**Rules:** companions eat real food, tire, learn, and die; work roles are real
capabilities with real upkeep; breeding needs a healthy pair and time.

### 7.6 `WildlifeHealthSystem` (new, thin, `Ashfall.Core.World`)

**Owns:** contamination and disease presence in fauna, testing, and safe
handling. **Consumes:** `RadiationSystem`, `DiseaseSystem.TryExpose`,
`Inventory` (testing), `MedicalPipelineCoordinator`. **Data:**
`wildlife_contamination.json`. **Rules:** contaminated animals exist; testing
before eating is a real decision; handling routes exposure through the live
pipelines; no new disease model.

### 7.7 Systems explicitly not added

- No second ecosystem, migration, trap, companion, disease, or radiation system.
- No hunting minigame with twitch aiming.
- No trophy or kill-score system.
- No infinite meat or respawn.
- No new currency.
- No new RNG stream.
- No new save section.

---

## 8. DATA CATALOG SPECIFICATION

All catalogs snake_case, integer `schema_version: 1`, validated and scanner-registered.

### 8.1 `species_profiles.json` (new, extends ecosystem content)

```json
{
  "schema_version": 1,
  "species": [
    {
      "species_id": "deer_browse",
      "display_name": "Browse Deer",
      "role": "prey",
      "diet": "browse",
      "breeding_window": "spring",
      "migration": true,
      "sectors": ["browse_meadow", "migration_gap"],
      "pressure_tolerance": 40,
      "recovery_rate": 0.04,
      "tameable": false,
      "tags": ["game", "hide", "quota"]
    }
  ]
}
```

### 8.2 `predator_prey_edges.json` (new)

Edges: predator, prey, strength, season, and sector modifiers.

### 8.3 `migration_corridors.json` (new)

Corridors: entry sector, exit sector, species, window, and interruption causes.

### 8.4 `hunting_grounds.json` (new)

Grounds: sector, concealment, wind patterns, quarry, and danger.

### 8.5 `quarry_profiles.json` (new)

Quarry: size, wariness, speed, wound behavior, yield, hide, and danger.

### 8.6 `quotas.json` (new)

Quotas: species, sector, season, limit, enforcement, and consequence.

### 8.7 `trap_sets.json` (new, extends trapping catalog)

Sets: trap type, bait, target, bycatch risk, durability, and check interval.

### 8.8 `companion_work.json` (new)

Roles: guard, draft, herding, hunting, hauling, scent, and egg production, with
training time, food needs, and capability.

### 8.9 `breeding_lines.json` (new)

Lines: species, pair requirements, gestation, litter, traits, and cull policy.

### 8.10 `wildlife_contamination.json` (new)

Contamination: species, sector, severity, test, and handling.

### 8.11 `field_notes.json` (new)

Notes: observation, knowledge raised, and unlock.

### 8.12 Items

New items appended to `items.json`: `item_hunting_bow`, `item_arrows`,
`item_tracking_kit`, `item_scent_mask`, `item_field_notes`, `item_binoculars`
(optics tie), `item_trap_snare`, `item_trap_cage`, `item_bait_root`,
`item_bait_meat`, `item_hide_green`, `item_hide_cured`, `item_bone_tools`,
`item_antler_material`, `item_meat_fresh`, `item_meat_smoked`,
`item_companion_feed`, `item_lead_rope`, `item_bell_collar`, `item_toxic_test_kit`.

---

## 9. SAVE, DETERMINISM, AND PERSISTENCE

### 9.1 Ownership

Wildlife trapping state, ecosystem state, migration state, and companion state
remain the live save owners. New sub-objects (hunting encounters, quotas,
predator conflicts, field notes, work training, breeding lines, contamination
records) are additive inside them. No new save section.

### 9.2 State to persist

- Hunter and tracking progress.
- Quota definitions and take counts.
- Trap sets and bycatch records.
- Predator conflict history and deterrence.
- Field notes and knowledge.
- Companion training, bonding, and breeding.
- Contamination test results.

### 9.3 Determinism

- Trapping and incidents use the live seeded streams already in
  `WildlifeTrappingState` (`rngSeed`, `encounterRngSeed`, `incidentRngSeed`).
- Hunting encounters resolve deterministically from authored profile, skill, and
  conditions; any randomness uses the live extraction path.
- Ecosystem pressure and recovery are deterministic daily ticks.
- Paired replay hashes must match.

### 9.4 Migration

Legacy saves load with existing ecosystem, trapping, and companion state
untouched; no hunting, quota, conflict, note, training, breeding, or
contamination state exists until started. Existing trap sites keep working.

### 9.5 Checksum

Invariant-culture floats; integer counts for animals taken and population
levels.

---

## 10. UI, ACCESSIBILITY, AND PRESENTATION

### 10.1 Surfaces

| Surface | Purpose | Owner |
|---|---|---|
| `WildlandsPanel` (new) | Sectors, species, pressure | `WildlandsHostSession` |
| `HuntPanel` (new) | Stalk, track, take, dress | same |
| `TrapLinePanel` (extend) | Sets, bait, checks | existing |
| `QuotaPanel` (new) | Seasons and limits | same |
| `CompanionPanel` (extend) | Tame, feed, train, breed | existing |
| `FieldNotesPanel` (new) | Observations and knowledge | same |
| `PredatorPanel` (new) | Activity, deterrence, conflict | same |

### 10.2 Accessibility and honesty

- Panels show live state and expose existing commands only.
- Pressure and population are shown as readable trends, not hidden math.
- Quota effects are stated before the take.
- Keyboard/controller close/back preserved; focus maintained on refresh.
- Hunting encounters offer non-twitch options; reflex is not the gate.

### 10.3 Presentation

Audio cues appended to `audio_cues.json`: wind through trees, a branch snap, a
trap line bell, a companion bark, an apex call at night, a knife on hide. No cue
is required; text carries meaning.

---

## 11. INTEGRATION SEAMS

| Existing system | Attachment |
|---|---|
| `WildlifeEcosystemSystem` | Population, pressure, knowledge, taming |
| `WildlifeMigrationSystem` | Corridors and packs |
| `WildlifeTrappingSystem` | Sets, bait, checks, seeded rolls |
| `CompanionAnimalSystem` | Taming, feeding, work roles |
| `WildlifeSeasonalCalendar` | Seasons and closures |
| `WildlifeTrappingEvents` | Event surfacing |
| `RadiationSystem` | Contaminated fauna |
| `DiseaseSystem` | Handling exposure |
| `MedicalPipelineCoordinator` | Treatments and bites |
| `SkillProgressionSystem` | Hunting and field skills |
| `NeedsSystem` | Fatigue, morale, hunger |
| `Inventory` | Meat, hides, tools |
| `TradingSystem` | Hide and meat trade |
| `SchoolingSystem` (Wave 4) | Field lessons and notes |
| `PressSystem` (Wave 4) | Published surveys |
| `EpilogueChronicleBuilder` | Wild milestones |

---

## 12. TECHNICAL IMPLEMENTATION PLAN

### 12.1 Phase order

**Phase 0 — Premise re-audit.** Confirm `WildlifeEcosystemSystem`,
`WildlifeMigrationSystem`, `WildlifeTrappingSystem`, `CompanionAnimalSystem`,
seasonal calendar, catalog loaders, hosts, and data sizes. Record file:line;
change nothing.

**Phase 1 — Data + validators.** Author species, edges, corridors, grounds,
quarry, quotas, trap sets, companion work, breeding lines, contamination, field
notes; append items. Register validators and scanner.

**Phase 2 — Pure Core.** `HuntingSystem`, `WildQuotaSystem`,
`PredatorConflictSystem`, `FieldStudySystem`, `CompanionWorkSystem`,
`WildlifeHealthSystem`.

**Phase 3 — Persistence.** Additive sub-objects, migration, round-trip,
determinism.

**Phase 4 — Host + CLI.** `WildlandsHostSession`, selftest coverage, fresh
journey.

**Phase 5 — UI.** New and extended panels with lifecycle and accessibility.

**Phase 6 — Content.** Locations, rooms, NPCs, quests, items, prose, audio.

**Phase 7 — Balance.** 720-day soak: overhunt, crash, predator push, rest, and
recovery.

**Phase 8 — Verification and closeout.**

### 12.2 Content volume

| Content | Count |
|---|---|
| Species profiles | 30 |
| Predator-prey edges | 25 |
| Migration corridors | 10 |
| Hunting grounds | 12 |
| Quarry profiles | 15 |
| Quotas | 12 |
| Trap sets | 15 |
| Companion roles | 8 |
| Breeding lines | 10 |
| Contamination rows | 8 |
| Field notes | 40 |
| Items | 20 |
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
| Second ecology | Critical | Extend live ecosystem |
| Second trap system | Critical | Extend live trapping |
| Infinite meat | High | Pressure and recovery |
| Trophy framing | High | Explicit exclusion |
| Extinction unrecoverable | High | Migration back and rest |
| Companion death cruel | High | Authored, gentle handling |
| Determinism break | Low | Live seeded streams |
| Content overrun | Medium | Budget §13 |

---

## 13. APPENDIX A — AUTHORING BUDGET

| File | Rows | Est. words |
|---|---|---|
| `species_profiles.json` | 30 | 7,000 |
| `predator_prey_edges.json` | 25 | 4,000 |
| `migration_corridors.json` | 10 | 2,500 |
| `hunting_grounds.json` | 12 | 3,000 |
| `quarry_profiles.json` | 15 | 4,000 |
| `quotas.json` | 12 | 2,500 |
| `trap_sets.json` | 15 | 3,000 |
| `companion_work.json` | 8 | 2,500 |
| `breeding_lines.json` | 10 | 2,500 |
| `wildlife_contamination.json` | 8 | 2,000 |
| `field_notes.json` | 40 | 6,000 |
| Quest objectives | 53 quests | 15,000 |
| NPC prose | 8 NPCs | 7,000 |
| Location prose | 10 | 3,500 |
| Item descriptions | 20 | 2,500 |
| Ending prose | 6 | 3,000 |
| **Total** | | **~70,000** |

---

## 14. APPENDIX B — RISK REGISTER

| ID | Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|---|
| R32-1 | Second ecology | Low | Critical | One population owner |
| R32-2 | Second trap system | Low | Critical | Live trap sites |
| R32-3 | Infinite meat | Med | High | Pressure and quotas |
| R32-4 | Trophy framing | Low | High | Exclusion rule |
| R32-5 | Permadeath ecology | Med | High | Migration and rest |
| R32-6 | Companion cruelty | Med | High | Gentle authored handling |
| R32-7 | Contamination horror | Low | Med | Restraint |
| R32-8 | Determinism | Low | High | Seeded streams |
| R32-9 | Content overrun | Med | Med | Budget §13 |
| R32-10 | Poaching unpunished | Med | Med | Recorded consequence |

---

## 15. APPENDIX C — OPEN DECISIONS REQUIRING A FOREMAN SIGNATURE

1. **Can the shelter hunt a species to local extinction?** Recommended: yes, with
   migration return only after a rest period.
2. **Is hunting reflex-based or authored-encounter based?** Recommended:
   authored, with skill and preparation; no twitch gate.
3. **Do companions die permanently?** Recommended: yes, but handled gently and
   never as a punishment.
4. **Can contaminated meat be eaten knowingly?** Recommended: yes, with real
   radiation consequences; the choice is the content.
5. **Do quotas apply to NPC poachers?** Recommended: yes, as recorded offense
   with policy response.

---

## 17. APPENDIX D — SPECIES TABLE (30 SPECIES)

| # | Species | Role | Diet | Season | Sector | Pressure tol. |
|---|---|---|---|---|---|---|
| 1 | Browse Deer | prey | browse | all | meadow | 40 |
| 2 | Grey Hare | prey | grass | all | meadow | 60 |
| 3 | Boar | prey | omnivore | autumn | old growth | 35 |
| 4 | Wire Goat | prey | scrub | all | hills | 45 |
| 5 | Marsh Fowl | prey | seeds | spring | marsh | 70 |
| 6 | Ash Pheasant | prey | seeds | autumn | burnt woods | 55 |
| 7 | Mole Rat | prey | roots | all | fields | 80 |
| 8 | Gulden Wolf | predator | meat | winter | old growth | 20 |
| 9 | Ash Hound | predator | meat | all | den field | 25 |
| 10 | Kestrel | predator | small prey | summer | lookout | 30 |
| 11 | Crow | scavenger | carrion | all | everywhere | 90 |
| 12 | Fox | mesopredator | small prey | all | meadow | 50 |
| 13 | Badger | mesopredator | roots | summer | woods | 45 |
| 14 | Otter | predator | fish | all | river | 35 |
| 15 | Viper | hazard | small prey | summer | rocks | 40 |
| 16 | Hornet Cloud | hazard | nectar | late summer | woods | 30 |
| 17 | Cave Bat | insectivore | insects | all | caves | 50 |
| 18 | River Carp | fish | omnivore | spring | river | 65 |
| 19 | Mud Eel | fish | carrion | summer | marsh | 55 |
| 20 | Shell Crab | fish | detritus | summer | river | 60 |
| 21 | Field Dog | companion | omnivore | all | kennel | n/a |
| 22 | Draft Ox | companion | grass | all | byre | n/a |
| 23 | Herding Goat | companion | scrub | all | byre | n/a |
| 24 | Message Pigeon | companion | seeds | all | aviary | n/a |
| 25 | Guard Goose | companion | grass | all | yard | n/a |
| 26 | Scent Ferret | companion | meat | all | kennel | n/a |
| 27 | Wool Sheep | companion | grass | all | field | n/a |
| 28 | Ash Moth | insect | plant | summer | woods | 60 |
| 29 | Rust Beetle | insect | plant | all | fields | 80 |
| 30 | Iron Wasp | hazard | predator | summer | ruins | 25 |

Thirty species across prey, predator, scavenger, hazard, fish, companion, and
insect categories. The food web is the expansion's foundation: every hunt, trap,
and predator event reads from this table and writes pressure back into it.

---

## 18. APPENDIX E — PREDATOR-PREY EDGE TABLE (25 EDGES)

| # | Predator | Prey | Strength | Season | Sector mod |
|---|---|---|---|---|---|
| 1 | Gulden Wolf | Browse Deer | 0.8 | winter | old growth |
| 2 | Gulden Wolf | Wire Goat | 0.6 | winter | hills |
| 3 | Gulden Wolf | Grey Hare | 0.3 | all | meadow |
| 4 | Ash Hound | Deer fawn | 0.7 | spring | den field |
| 5 | Ash Hound | Marsh Fowl | 0.4 | spring | marsh |
| 6 | Ash Hound | Field Dog | 0.5 | all | fence |
| 7 | Kestrel | Mole Rat | 0.7 | summer | fields |
| 8 | Kestrel | Ash Moth | 0.5 | summer | woods |
| 9 | Fox | Hare | 0.6 | all | meadow |
| 10 | Fox | Marsh Fowl | 0.7 | spring | marsh |
| 11 | Fox | Pigeon | 0.5 | all | aviary |
| 12 | Badger | Mole Rat | 0.4 | summer | fields |
| 13 | Otter | River Carp | 0.7 | all | river |
| 14 | Otter | Shell Crab | 0.5 | summer | river |
| 15 | Viper | Hare young | 0.4 | summer | rocks |
| 16 | Cave Bat | Ash Moth | 0.6 | all | caves |
| 17 | Crow | Carrion | 0.9 | all | everywhere |
| 18 | Crow | Eggs | 0.6 | spring | everywhere |
| 19 | Hornet Cloud | Rust Beetle | 0.4 | late summer | woods |
| 20 | Iron Wasp | Cave Bat | 0.3 | summer | ruins |
| 21 | Mud Eel | Shell Crab | 0.4 | summer | marsh |
| 22 | Boar | Roots | 0.7 | autumn | old growth |
| 23 | Deer | Browse | 0.9 | all | meadow |
| 24 | Goat | Scrub | 0.9 | all | hills |
| 25 | Moth | Leaves | 0.8 | summer | woods |

Edges are the live ledger between predator and prey. Authored edges make apex
pressure legible: when the deer fall, the wolves turn to goats and then to the
fence, and the shelter can see it coming if it has been reading the land.

---

## 19. APPENDIX F — MIGRATION CORRIDOR TABLE (10 CORRIDORS)

| # | Corridor | Entry | Exit | Species | Window |
|---|---|---|---|---|---|
| 1 | The Gap | north woods | meadow | deer | autumn |
| 2 | River Bend | marsh | river | fowl, carp | spring |
| 3 | Ash Pass | burnt woods | hills | boar | autumn |
| 4 | Old Road | fields | old growth | hare | all |
| 5 | Den Run | hills | den field | wolves | winter |
| 6 | Marsh Cut | marsh | meadow | otter | summer |
| 7 | Bat Funnel | caves | ruins | bats | spring |
| 8 | Kestrel Line | lookout | fields | kestrel | summer |
| 9 | Crow Road | everywhere | ruins | crow | all |
| 10 | Shell Channel | river | marsh | crab | summer |

Corridors are why a shelter can lose a meadow and still eat: the land moves, and
the shelter that mapped the movement follows it. The expansion makes corridors
findable, mappable, and huntable — but only with knowledge.

---

## 20. APPENDIX G — HUNTING GROUND TABLE (12 GROUNDS)

| # | Ground | Sector | Concealment | Wind | Quarry | Danger |
|---|---|---|---|---|---|---|
| 1 | The Browse Meadow | meadow | low | steady | deer, hare | 2 |
| 2 | The Old Growth | old growth | high | shifting | boar, wolf | 5 |
| 3 | The Salt Lick | meadow | blind | calm | deer | 3 |
| 4 | The Marsh Edge | marsh | reeds | wet | fowl, eel | 4 |
| 5 | The Gap | corridor | brush | channeled | deer | 4 |
| 6 | The Burnt Woods | burnt woods | deadfall | open | boar, moth | 5 |
| 7 | The Hills | hills | rock | rising | goat, fox | 4 |
| 8 | The River Bank | river | willow | damp | carp, otter | 3 |
| 9 | The Den Field | den field | grass | open | wolf | 6 |
| 10 | The Lookout Tree | forest | height | high | kestrel | 4 |
| 11 | The Ruins Edge | ruins | rubble | gusty | wasp, bat | 5 |
| 12 | The Field Margins | fields | hedgerow | light | rat, kestrel | 2 |

Grounds describe what a real hunt requires: where you can hide, how the wind
moves, what you might meet, and how much trouble you are in if it goes wrong.

---

## 21. APPENDIX H — QUARRY PROFILE TABLE (15 QUARRY)

| # | Quarry | Wariness | Speed | Wound behavior | Yield | Hide |
|---|---|---|---|---|---|---|
| 1 | Browse Deer | high | fast | runs far | 40 kg | good |
| 2 | Grey Hare | high | fast | short run | 3 kg | poor |
| 3 | Boar | med | med | charges | 60 kg | tough |
| 4 | Wire Goat | high | fast | climbs | 25 kg | good |
| 5 | Marsh Fowl | med | short | bursts | 2 kg | feathers |
| 6 | Ash Pheasant | high | short | bursts | 1.5 kg | feathers |
| 7 | Mole Rat | low | slow | none | 1 kg | poor |
| 8 | Gulden Wolf | high | fast | turns | 35 kg | excellent |
| 9 | Ash Hound | med | fast | pack | 30 kg | good |
| 10 | Kestrel | high | air | flies | 0.5 kg | feathers |
| 11 | Fox | high | fast | dodges | 6 kg | good |
| 12 | Badger | med | slow | fights | 10 kg | tough |
| 13 | Otter | high | swims | dives | 8 kg | excellent |
| 14 | River Carp | low | swims | thrashes | 5 kg | none |
| 15 | Shell Crab | low | slow | pinches | 2 kg | shell |

Quarry profiles make the hunt honest: a deer takes patience, a boar can turn, a
wolf is rarely worth the risk, and a hare is a stew rather than a trophy.

---

## 22. APPENDIX I — QUOTA TABLE (12 QUOTAS)

| # | Quota | Species | Sector | Season | Limit |
|---|---|---|---|---|---|
| 1 | Deer Autumn Take | deer | Gap | autumn | 6 |
| 2 | Deer Meadow Rest | deer | meadow | all | 0 |
| 3 | Hare Steady Take | hare | meadow | all | 20 |
| 4 | Boar Cull | boar | old growth | autumn | 4 |
| 5 | Fowl Spring Limit | fowl | marsh | spring | 10 |
| 6 | Fowl Autumn | fowl | marsh | autumn | 25 |
| 7 | Goat Range Take | goat | hills | summer | 3 |
| 8 | Predator Target | wolf | den field | winter | 1 |
| 9 | Fox Control | fox | meadow | all | 8 |
| 10 | River Net Limit | carp | river | spring | 40 |
| 11 | Bat Protection | bat | caves | all | 0 |
| 12 | Crow Open | crow | all | all | none |

Quotas are policy, written by the shelter and enforced by the ecology. A shelter
that ignores its own numbers is not punished by a menu; it is punished by an
empty winter and a fence line that gets tested.

---

## 23. APPENDIX J — TRAP SET TABLE (15 SETS)

| # | Set | Trap | Bait | Target | Bycatch | Durability |
|---|---|---|---|---|---|---|
| 1 | Hare Snare | snare | root | hare | rabbit | 6 |
| 2 | Deer Snare | snare | salt | deer | dog | 4 |
| 3 | Fowl Net | net | seed | fowl | none | 8 |
| 4 | Boar Deadfall | deadfall | meat | boar | dog | 3 |
| 5 | Rat Cage | cage | grain | rat | bird | 10 |
| 6 | Fox Cage | cage | meat | fox | dog | 6 |
| 7 | Pit Trap | pit | none | deer | any | 2 |
| 8 | Fish Trap | basket | none | carp | eel | 9 |
| 9 | Crab Trap | basket | carrion | crab | eel | 9 |
| 10 | Bird Line | line | seed | fowl | none | 7 |
| 11 | Scent Lure | snare | scent | fox | dog | 6 |
| 12 | Wolf Snare | snare | meat | wolf | dog | 3 |
| 13 | Mole Line | snare | root | rat | none | 8 |
| 14 | Otter Trap | cage | fish | otter | none | 6 |
| 15 | Humane Box | cage | food | any small | none | 10 |

Trap sets use the live trap types and durability model. Bycatch is the
consequence that makes the trap line a moral practice rather than a vending
machine, and the humane box is the shelter's better answer.

---

## 24. APPENDIX K — COMPANION WORK TABLE (8 ROLES)

| # | Role | Species | Training | Food | Capability |
|---|---|---|---|---|---|
| 1 | Guard Dog | Field Dog | 30d | meat | fence alert |
| 2 | Scent Dog | Field Dog | 45d | meat | tracking |
| 3 | Hunting Dog | Field Dog | 60d | meat | hunt assist |
| 4 | Draft Ox | Ox | 60d | grass | hauling |
| 5 | Herding Goat | Goat | 30d | scrub | flock control |
| 6 | Message Pigeon | Pigeon | 20d | seeds | messages |
| 7 | Guard Goose | Goose | 10d | grass | alarm |
| 8 | Scent Ferret | Ferret | 30d | meat | den survey |

Companions are capability with upkeep: a guard dog watches the fence but eats
meat the shelter could preserve; a draft ox hauls the kiln's lime but needs
pasture. The expansion makes that trade explicit.

---

## 25. APPENDIX L — BREEDING LINE TABLE (10 LINES)

| # | Line | Species | Pair | Gestation | Litter | Trait |
|---|---|---|---|---|---|---|
| 1 | Fence Line | Field Dog | healthy | 63d | 4 | alert |
| 2 | Trail Line | Field Dog | scent | 63d | 3 | tracking |
| 3 | Haul Line | Ox | strong | 280d | 1 | strength |
| 4 | Wool Line | Sheep | woolly | 150d | 1 | fleece |
| 5 | Milk Line | Goat | milky | 150d | 2 | milk |
| 6 | Alarm Line | Goose | loud | 30d | 4 | alarm |
| 7 | Message Line | Pigeon | homing | 18d | 2 | homing |
| 8 | Den Line | Ferret | bold | 42d | 3 | courage |
| 9 | Guard Line | Field Dog | large | 63d | 4 | size |
| 10 | Herd Line | Goat | calm | 150d | 2 | calm |

Breeding is how the shelter stops depending on luck. Lines are authored traits,
not stat inflation, and every line needs food, space, and time to produce
anything at all.

---

## 26. APPENDIX M — CONTAMINATION TABLE (8 ROWS)

| # | Species group | Sector | Severity | Test | Handling |
|---|---|---|---|---|---|
| 1 | Boar | burnt woods | high | kit | discard |
| 2 | Hare | fields | med | kit | test then use |
| 3 | Deer | old growth | low | kit | monitor |
| 4 | Carp | river | med | kit | test then use |
| 5 | Fowl | marsh | high | kit | discard |
| 6 | Crow | everywhere | high | kit | never use |
| 7 | Crab | river | med | kit | test then use |
| 8 | Moth | woods | none | none | ignore |

Contamination turns hunting into a test-and-decide loop: the shelter can eat,
sell, or discard, and the radiation pipeline applies the consequence. This is
the expansion's clearest tie to the setting's central hazard.

---

## 27. APPENDIX N — FIELD NOTE TABLE (40 NOTES — REPRESENTATIVE)

| # | Note | Species | Knowledge | Unlock |
|---|---|---|---|---|
| 1 | First Tracks | deer | level 1 | tracking basics |
| 2 | Browse Pattern | deer | level 2 | meadow map |
| 3 | Rutting Season | deer | level 3 | autumn quota |
| 4 | Hare Runs | hare | level 1 | snare sites |
| 5 | Hare Litter | hare | level 2 | breeding |
| 6 | Boar Wallow | boar | level 2 | cull site |
| 7 | Boar Temper | boar | level 3 | safety |
| 8 | Goat Trails | goat | level 2 | range map |
| 9 | Wolf Kill | wolf | level 3 | apex read |
| 10 | Pack Size | wolf | level 4 | deterrence |
| 11 | Den Location | wolf | level 5 | cull decision |
| 12 | Hound Fence | hound | level 2 | fence watch |
| 13 | Fox Cache | fox | level 2 | control |
| 14 | Badger Set | badger | level 2 | bycatch |
| 15 | Otter Slide | otter | level 2 | river trap |
| 16 | Carp Spawn | carp | level 2 | net season |
| 17 | Eel Run | eel | level 3 | marsh trap |
| 18 | Crab Bed | crab | level 2 | basket sites |
| 19 | Fowl Nest | fowl | level 2 | spring closure |
| 20 | Fowl Flyway | fowl | level 3 | corridor |
| 21 | Kestrel Hover | kestrel | level 2 | field margin |
| 22 | Crow Roost | crow | level 1 | scavenging |
| 23 | Bat Funnel | bat | level 2 | protection |
| 24 | Moth Hatch | moth | level 1 | season note |
| 25 | Beetle Bloom | beetle | level 1 | crop risk |
| 26 | Wasp Ground | wasp | level 2 | hazard map |
| 27 | Viper Rocks | viper | level 2 | hazard map |
| 28 | Dog Lineage | dog | level 3 | breeding |
| 29 | Ox Temperament | ox | level 3 | work role |
| 30 | Goat Milk Yield | goat | level 3 | dairy |
| 31 | Pigeon Homing | pigeon | level 3 | messages |
| 32 | Goose Alarm | goose | level 2 | fence |
| 33 | Ferret Den | ferret | level 3 | survey |
| 34 | Migration Gap | deer | level 4 | corridor |
| 35 | Burn Rebound | boar | level 4 | recovery |
| 36 | Pressure Signs | any | level 4 | quota |
| 37 | Empty Meadow | any | level 5 | conservation |
| 38 | Poison Bloom | fowl | level 4 | testing |
| 39 | Predator Push | wolf | level 5 | conflict |
| 40 | Recovery Check | deer | level 5 | rest verified |

Field notes are the expansion's knowledge currency. They are written by
observation, shared through the school and the press, and they change what the
shelter can do outdoors.

---

## 28. APPENDIX O — MAIN QUESTLINE STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_wild_empty_meadow` | 4 | Count take; compare with catch |
| `quest_wild_juvenile` | 4 | Think about the take |
| `quest_wild_count` | 5 | Survey and read notes |
| `quest_wild_corridor` | 5 | Find and map the gap |
| `quest_wild_rest` | 4 | Argue and close the meadow |
| `quest_wild_line` | 4 | Reposition the trap line |
| `quest_wild_quota` | 4 | Write and post take limits |
| `quest_wild_dogs` | 5 | Loss at the fence; decide |
| `quest_wild_apex` | 5 | Track activity; choose response |
| `quest_wild_old_growth` | 5 | Dangerous survey proves the corridor |
| `quest_wild_poisoned` | 4 | Find and test contaminated animals |
| `quest_wild_tame` | 5 | Tame and train a first companion |
| `quest_wild_autumn` | 5 | First quota-led harvest |
| `quest_wild_winter` | 5 | Scarcity tests discipline |
| `quest_wild_what_woods_kept` | 3 | Final disposition |

---

## 29. APPENDIX P — NPC DOSSIERS (BRIEF)

**Alder Quist** — hunter. Took his first deer at fourteen and has not lost the
habit of sitting still since. Believes a hunt ends when the shelter eats, not
when the animal falls.

**Mira Gess** — trapper. Runs a long line with cold efficiency and resents the
word "overhunt" until the meadow proves it. Capable of changing her mind, which
is the point.

**Osa Reed** — naturalist. Keeps notes nobody reads until the winter they matter.
Treats observation as a debt to the future.

**Tobin Skel** — herder. Knows every animal by name and temperament and believes
companions are workers with rights.

**Kell** — child. Finds tracks before adults do because children look at the
ground. Learning that the land is a ledger.

**Dace** — warden. Holds the fence, counts wolves, and hates culling more than
anyone, because Dace is the one who does it.

**Hem** — butcher. Turns a carcass into food, hide, glue, and bone with almost
no waste, and considers waste a moral failure.

**Yarrow** — hide trader. Prices pelts fairly and knows when a seller has been
poaching; keeps that knowledge for a useful day.

---

## 30. APPENDIX Q — LOCATION DETAIL

- **The Old Growth** — deep forest, apex territory, and the corridor's source.
- **The Browse Meadow** — the shelter's old hunting ground, now being rested.
- **The Marsh Edge** — waterfowl, eels, leeches, and soft ground.
- **The Den Field** — den sites, wolf sign, and the worst place to be at dusk.
- **The Salt Lick** — deer concentrations and the best blind in the valley.
- **The Gap** — the migration corridor, narrow, brushy, and decisive.
- **The Burnt Woods** — recovering habitat with contaminated game.
- **The Poacher Camp** — unlicensed take, snares, and a conflict with a face.
- **The Kennel Yard** — training, bonding, and the noise of working animals.
- **The Lookout Tree** — survey, kestrel nests, and a wide view of the migration.

---

## 31. APPENDIX R — POPULATION AND PRESSURE MODEL

| Pressure level | Population trend | Visible sign | Recovery |
|---|---|---|---|
| 0–20% | stable/growing | abundant sign | n/a |
| 20–40% | stable | normal sign | fast |
| 40–60% | declining | fewer juveniles | medium |
| 60–80% | falling | empty grounds | slow |
| 80–95% | critical | rare sightings | very slow |
| 95%+ | local extinction | silence | migration only |

Pressure is recorded through the live system and shown as readable signs. The
shelter does not need a spreadsheet to know the meadow is failing; it needs to
be honest about what it already sees.

---

## 32. APPENDIX S — HUNTING RESOLUTION MODEL

| Step | Factor | Effect |
|---|---|---|
| Locate | sign, knowledge | finds encounter |
| Stalk | concealment, wind | approach chance |
| Range | weapon, skill | shot quality |
| Release | composure, skill | hit, wound, miss |
| Recover | wound track, time | animal found or lost |
| Dress | skill | yield and spoilage |
| Record | take | pressure rise |

Hunting is a sequence of authored decisions rather than a reflex test. The
shelter's best hunter is not the fastest but the one who knows when not to
release.

---

## 33. APPENDIX T — COMPANION CARE MODEL

| Need | Requirement | Effect if missed |
|---|---|---|
| Food | daily feed | hunger, refusal |
| Water | daily access | weakness |
| Shelter | kennel/byre | illness |
| Exercise | work or run | agitation |
| Bond | handling | no work entry |
| Health | vet checks | hidden injury |
| Rest | off-duty days | exhaustion |
| Company | same-species | distress |

Companions age, tire, and need care. The expansion's rule is that a companion
can do real work only if the shelter is willing to feed and handle it like a
partner.

---

## 34. APPENDIX U — PREDATOR CONFLICT MODEL

| Prey state | Apex behavior | Shelter risk | Options |
|---|---|---|---|
| abundant | stays in range | low | none |
| falling | ranges closer | medium | deter |
| low | hunts livestock | high | guard, cull |
| collapsed | fence testing | very high | cull, relocate |
| extinct prey | den bypass | extreme | starvation, conflict |

The predator model is causal: no monsters, only hungry animals acting on the
arithmetic the shelter created. Deterrence is labor, culling is loss, and
coexistence is possible when the prey is healthy.

---

## 35. APPENDIX V — WORKED 720-DAY ECOLOGY SCENARIO

**Days 1–90.** Hunting continues in the meadow. Pressure crosses 40%. Osa's
survey is dismissed.

**Days 91–180.** Catch falls; more juveniles are taken. Pressure crosses 60%.
The deer herd drops.

**Days 181–270.** Dogs are lost at the fence; apex activity rises. The meadow
is closed as an emergency, not a plan.

**Days 271–360.** The corridor is found and mapped; the trap line is moved; the
first quota is written. Winter is lean but not desperate.

**Days 361–540.** The autumn harvest follows the migration. Pressure in the
meadow falls below 40% and the first fawns are seen.

**Days 541–660.** Companion breeding produces a fence line. Predator pressure
falls with the prey returning. The quota holds.

**Days 661–720.** The meadow carries a stable herd, the shelter eats from a
living range, and the notes are published. The land and the shelter have a
working relationship.

---

## 36. APPENDIX W — VIGNETTE (TONE SAMPLE)

> Alder kneels by the track and touches the print with two fingers and reads the
> depth and the direction and the time, and then stands and walks uphill because
> the wind is wrong and the deer will smell him before he sees it.

> Mira checks the line at dawn and finds a hare and a torn snare, and she
> repairs the snare with cold hands and sets it half a metre to the side, and
> that small correction is the whole craft of trapping.

> Osa writes the date and the sector and the number of birds and underlines the
> word fewer, and closes the notebook, and tells nobody, because nobody has
> asked yet.

---

## 37. APPENDIX X — FAILURE MODES AND RECOVERY

| Failure | Effect | Recovery |
|---|---|---|
| Overhunt | population crash | rest, quota, migration |
| Local extinction | empty sector | corridor return |
| Wound loss | spoilage, pressure | tracking practice |
| Bycatch | companion loss | reposition, cage sets |
| Contamination | illness risk | testing, discard |
| Predator push | livestock loss | deterrence, cull |
| Companion death | capability loss | breeding, training |
| Quota ignored | trust loss | enforcement, policy |
| Poaching | unfair take | patrol, fine |
| Lost notes | knowledge loss | rewrite, teach |

No failure is a game over. The deepest failure is a shelter that empties its own
range and blames the winter.

---

## 38. APPENDIX Y — CONTENT REVIEW CHECKLIST

- [ ] `WildlifeEcosystemSystem` remains the population authority.
- [ ] `WildlifeTrappingSystem` keeps trap sites and seeded rolls.
- [ ] `CompanionAnimalSystem` keeps taming and feeding.
- [ ] No infinite meat or respawn without migration.
- [ ] No trophy, scoring, or sport framing.
- [ ] Predators are animals, not monsters.
- [ ] Contamination routes through radiation and disease pipelines.
- [ ] Companion death is handled gently.
- [ ] Field knowledge is earned by observation.
- [ ] Save additions are additive and legacy-neutral.
- [ ] Determinism uses live seeded streams only.

---

## 39. APPENDIX Z — GLOSSARY

- **Pressure** — recorded hunting take on a sector and species.
- **Corridor** — a migration route between sectors.
- **Quota** — a shelter-written take limit.
- **Bycatch** — a non-target animal caught in a set.
- **Knowledge level** — what the shelter knows about a species.
- **Apex state** — predator activity driven by prey.
- **Bond** — companion trust required for work.
- **Line** — a breeding lineage with an authored trait.
- **Dress** — field butchery after a successful take.
- **Rest** — a closed sector recovering.

---

## 40. APPENDIX AA — INTEGRATION MATRIX

| System | Reads | Writes | Never writes |
|---|---|---|---|
| `WildlifeEcosystemSystem` | pressure | populations | inventory |
| `WildlifeMigrationSystem` | corridors | packs | populations |
| `WildlifeTrappingSystem` | sets, bait | catches | populations |
| `CompanionAnimalSystem` | food | care | training |
| `HuntingSystem` | game | takes, pressure | populations |
| `WildQuotaSystem` | policy | limits | populations |
| `PredatorConflictSystem` | apex | conflicts | populations |
| `FieldStudySystem` | observation | notes, knowledge | populations |
| `CompanionWorkSystem` | companions | training, work | care |
| `WildlifeHealthSystem` | samples | tests, flags | radiation |
| `RadiationSystem` | fauna | dose | ecology |
| `DiseaseSystem` | handling | exposure | ecology |
| `Inventory` | meat, hides | stock | populations |
| `TradingSystem` | hides | trade | ecology |
| `EpilogueChronicleBuilder` | milestones | chronicle | — |

---

## 41. APPENDIX AB — DATA SCHEMA DETAIL (NEW CATALOGS)

**`species_profiles.json`** — `species_id`, `display_name`, `role`, `diet`,
`breeding_window`, `migration`, `sectors[]`, `pressure_tolerance`,
`recovery_rate`, `tameable`, `tags`.

**`predator_prey_edges.json`** — `predator`, `prey`, `strength`, `season`,
`sector_modifier`, `tags`.

**`migration_corridors.json`** — `corridor_id`, `entry`, `exit`, `species[]`,
`window`, `interruptions[]`, `tags`.

**`hunting_grounds.json`** — `ground_id`, `sector`, `concealment`, `wind`,
`quarry[]`, `danger`, `tags`.

**`quarry_profiles.json`** — `quarry_id`, `wariness`, `speed`,
`wound_behavior`, `yield`, `hide`, `tags`.

**`quotas.json`** — `quota_id`, `species`, `sector`, `season`, `limit`,
`enforcement`, `consequence`, `tags`.

**`trap_sets.json`** — `set_id`, `trap`, `bait`, `target`, `bycatch[]`,
`durability`, `check_interval`, `tags`.

**`companion_work.json`** — `role_id`, `species`, `training_days`, `food`,
`capability`, `tags`.

**`breeding_lines.json`** — `line_id`, `species`, `pair_requirement`,
`gestation`, `litter`, `trait`, `tags`.

**`wildlife_contamination.json`** — `group_id`, `sector`, `severity`, `test`,
`handling`, `tags`.

**`field_notes.json`** — `note_id`, `species`, `knowledge_level`, `unlock`,
`text`, `tags`.

All new catalogs carry `schema_version: 1` and fail the integrity gate on missing
or duplicate IDs, invalid references, or out-of-range numbers.

---

## 42. APPENDIX AC — MEASUREMENT AND TELEMETRY

| Metric | Purpose | Source |
|---|---|---|
| Sector density | range health | Ecosystem |
| Pressure per species | take discipline | Ecosystem |
| Local extinctions | conservation state | Ecosystem |
| Catch per trap day | trapping efficiency | Trapping |
| Bycatch rate | line quality | Trapping |
| Quota compliance | policy | Quota |
| Predator conflicts | risk | Conflict |
| Companion work days | capability | Companion |
| Contamination tests | safety | Health |
| Knowledge levels | field study | FieldStudy |

Telemetry is diagnostic only; it never gates content and never becomes a hidden
score.

---

## 43. APPENDIX AD — IMPLEMENTATION CHECKLIST

- [ ] Phase 0 premise re-audit recorded with file:line evidence.
- [ ] Phase 1 catalogs authored and registered with validators and scanner.
- [ ] Phase 2 Core systems are pure, engine-free, and deterministic.
- [ ] Phase 3 save additions are additive, legacy-neutral, and round-trip tested.
- [ ] Takes route through `RecordHuntingPressure` only.
- [ ] Triad parity (Setup / Save / Flush) holds for every new state.
- [ ] Phase 4 host session exposes live state and real commands only.
- [ ] Phase 5 panels pass bind/unbind/rebind, focus, contrast, and scaling checks.
- [ ] Phase 6 content passes the review checklist in §38.
- [ ] Phase 7 soak shows overhunt, crash, rest, and recovery.
- [ ] Phase 8 data integrity and content utilization selftests pass.
- [ ] No parallel ecology, trap, or companion system exists.

---

## 44. APPENDIX AE — OPEN QUESTIONS FOR REVIEW

1. Can the shelter exceed its own quota, and what records the offense?
2. Does a wounded animal always escape, or can tracking guarantee recovery?
3. Should contaminated meat test as one batch or per animal?
4. Does a companion's death affect the whole shelter's morale or only its
   handler?
5. Can predators be relocated instead of culled, and at what cost?
6. Do NPC settlements hunt the same sectors, and can overhunt be shared?
7. Should field notes be tradeable to other settlements?
8. Do migration corridors shift permanently after a burn or a crash?

None of these may be decided unilaterally; each changes balance and tone.

---

## 45. APPENDIX AF — CROSS-WAVE HOOKS

| Wave | Expansion | Hook |
|---|---|---|
| 1 | 12 The Second Generation | Children learning the land |
| 1 | 13 The Faithful | Rites for the taken animal |
| 1 | 14 Above the Ash | Surveying game from the air |
| 1 | 15 The Deep Root | Companion species, manures, wool |
| 1 | 16 The Rebuilt Body | Prosthetics for trappers and hunters |
| 2 | 17 The Long Evening | Songs and stories of the hunt |
| 2 | 18 The Underneath | Cave bats, subterranean fauna |
| 2 | 19 The Bitter Air | Contaminated wildlife and testing |
| 2 | 20 The Quiet Hand | Poacher networks and informants |
| 2 | 21 The Grid | Fence lighting and alarms |
| 3 | 22 The Clean Flow | Marsh water, fish safety |
| 3 | 23 The Alarm | Predator alarms and wildland fire |
| 3 | 24 The Long Goodbye | Companion aging and loss |
| 3 | 25 The Iron Road | Game trains, feeding a rail town |
| 3 | 26 The Common Table | Butchery, smoking, and lean seasons |
| 4 | 27 The Thread | Hides, leather, and fur |
| 4 | 28 The Lesson | Field classes and naturalist notes |
| 4 | 29 The Glass | Binoculars and observation optics |
| 4 | 30 The Press | Published surveys and quotas |
| 4 | 31 The Kiln | Salt, smoke, and drying capacity |

Each hook is additive. The Wild can ship alone, and every other expansion can
ship without it.

---

## 46. APPENDIX AG — ENDING PROSE SKETCHES

**The Full Wood.** The herd returns, the corridor fills in autumn, and the
shelter's hunters write quotas the next generation keeps.

**The Managed Range.** Traps, dogs, and a written line make the wild a working
partner, and the shelter eats without emptying the land.

**The Quiet Meadow.** The meadow stays empty, and the shelter remembers it every
winter when the stores run thin and the smokehouse is doing the work of the
forest.

**The Predator Winter.** Wolves at the fence and no deer behind them, and the
shelter fights an animal problem it made and cannot shoot its way out of.

**The Poisoned Season.** Contamination enters the meat, and the clinic's
summer is spent on the mistake the testing would have prevented.

**Fade.** The same two sectors are hunted a little harder, and the woods still
look full, and nothing changes yet.

---

## 47. APPENDIX AH — COMMON FAILURE PATTERNS TO AVOID

| Pattern | Why it is bad | Correct approach |
|---|---|---|
| Infinite wildlife | removes consequence | pressure and recovery |
| Fantasy monsters | tone break | real animals, real teeth |
| Trophy loop | glorifies killing | subsistence framing |
| Hidden pressure math | unfair | readable signs |
| Companions as gear | no care | feed, bond, upkeep |
| Trap vending machine | no bycatch | bycatch and checks |
| Ecology permadeath | too punishing | migration return |
| Hunt as twitch test | excludes players | authored encounters |
| Contamination ignored | fantasy | testing choice |
| Poaching unrecorded | no consequence | recorded offense |

The list exists because wildlife is easy to make either infinite or cruel. The
live ecosystem already keeps the account; the expansion's job is to make that
account readable and meaningful.

---

## 48. APPENDIX AI — CONTENT VOLUME SUMMARY

| Category | Rows | Prose estimate |
|---|---|---|
| Species | 30 | 7,000 |
| Predator-prey edges | 25 | 4,000 |
| Migration corridors | 10 | 2,500 |
| Hunting grounds | 12 | 3,000 |
| Quarry profiles | 15 | 4,000 |
| Quotas | 12 | 2,500 |
| Trap sets | 15 | 3,000 |
| Companion roles | 8 | 2,500 |
| Breeding lines | 10 | 2,500 |
| Contamination rows | 8 | 2,000 |
| Field notes | 40 | 6,000 |
| Quests | 53 | 15,000 |
| NPCs | 8 | 7,000 |
| Locations | 10 | 3,500 |
| Items | 20 | 2,500 |
| Endings | 6 | 3,000 |
| **Total** | | **~70,000** |

---

## 49. APPENDIX AJ — SIDE QUEST STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_wild_stalk` | 4 | Locate, approach, hold, withdraw |
| `quest_wild_track` | 4 | Find sign, follow, read, arrive |
| `quest_wild_clean_shot` | 3 | Wait, release, verify |
| `quest_wild_recover` | 4 | Follow blood, search, find, dress |
| `quest_wild_scent` | 3 | Test wind, reposition, confirm |
| `quest_wild_set_line` | 4 | Survey, set, mark, record |
| `quest_wild_bait_test` | 4 | Choose, set, compare, decide |
| `quest_wild_check` | 3 | Walk, check, reset |
| `quest_wild_bycatch` | 4 | Record, reduce, verify |
| `quest_wild_humane` | 3 | Learn, practice, commit |
| `quest_wild_survey` | 4 | Walk, count, note, tally |
| `quest_wild_edge` | 4 | Observe, link, test, record |
| `quest_wild_extinct` | 3 | Search, confirm, report |
| `quest_wild_recover_herd` | 5 | Close, rest, monitor, verify |
| `quest_wild_pressure_read` | 3 | Gather, read, act |
| `quest_wild_tame_first` | 5 | Approach, feed, handle, lead |
| `quest_wild_feed` | 3 | Source, schedule, verify |
| `quest_wild_train` | 4 | Teach, repeat, test, release |
| `quest_wild_bond` | 4 | Handle, work, rest, trust |
| `quest_wild_breed` | 5 | Pair, wait, birth, raise |
| `quest_wild_notes` | 3 | Start, write, share |
| `quest_wild_knowledge` | 4 | Observe, repeat, raise level |
| `quest_wild_season_map` | 4 | Track, map, predict, verify |
| `quest_wild_toxic_watch` | 4 | Sample, test, mark, report |
| `quest_wild_share` | 3 | Write, teach, publish |

---

## 50. APPENDIX AK — HUNTING SEASON TABLE

| Season | Game | Closed | Quota bias | Note |
|---|---|---|---|---|
| Spring | fowl, carp | deer, goat | low | breeding |
| Summer | goat, hare | fowl nests | medium | abundance |
| Autumn | deer, boar | hare litters | high | migration |
| Winter | hare, wolf cull | deer does | low | scarcity |

The shelter's year is written in seasons, and the expansion makes the calendar
legible at the gate: what is in season, what is closed, and why.

---

## 51. APPENDIX AL — FIELD SKILL TABLE

| Skill | Practice | Unlocks |
|---|---|---|
| Tracking | following sign | corridor reading |
| Stalking | wind and cover | close approach |
| Marksmanship | target practice | clean take |
| Dressing | field butchery | better yield |
| Tanning | hide work | leather quality |
| Trapping | set craft | lower bycatch |
| Taming | handling | work roles |
| Observation | note keeping | knowledge |
| Survey | counting | pressure read |
| Safety | hazard work | fewer injuries |

Field skills are the shelter's outdoor competence, earned by practice and
recorded through the live skill system. None of them can be bought; all of them
can be taught.

---

## 52. APPENDIX AM — WILDLAND POLICY TABLE

| Policy | Trigger | Effect | Cost |
|---|---|---|---|
| Open Range | default | no limits | ecology risk |
| Seasonal Quota | survey | take limits | policy work |
| Total Closure | crash | no take | food pressure |
| Predator Cull | conflict | fewer predators | prey rebound risk |
| Deterrence | conflict | fewer losses | labor and fences |
| Companion Program | decision | work animals | feed cost |
| Contamination Hold | test | safety | food loss |
| Corridor Protection | knowledge | future take | no hunting there |

Wildland policy is the shelter's contract with the land, written like any other
policy and reviewed in the open. The expansion treats conservation as governance
rather than a green morality meter.

---

## 53. APPENDIX AN — OPEN IMPLEMENTATION NOTES

- Species catalogs should extend the live `FaunaSpeciesDef` shape so the
  ecosystem loader remains the single consumer.
- Pressure must always be recorded through `RecordHuntingPressure`; no local
  counters.
- Hunting encounters should be authored data with deterministic resolution; use
  the live seeded path only where an outcome genuinely needs one.
- Companion work roles should ride on existing `CompanionAssignResult` and feed
  results rather than a parallel companion state.
- Contamination tests must call the radiation pipeline, never a local flag.
- Field notes should register as readable content and feed the school and press
  where present.
- Quota offenses should be recorded as policy events so the shelter can respond
  in its own systems.
- Migration corridors should be authored in data and verified against live
  sector ids.

---

## 54. APPENDIX AO — SENSITIVE TOPICS TABLE

| Topic | Risk | Handling |
|---|---|---|
| Hunting | Glorification | Subsistence framing |
| Wounding | Cruelty | Recovery and humane practice |
| Predator culling | Violence | Cost, reluctance, consequence |
| Companion loss | Grief | Gentle, functional |
| Contamination | Horror | Testing and safety |
| Extinction | Despair | Recovery and lessons |
| Poaching | Crime | Policy and consequence |
| Children hunting | Exploitation | Observation and training only |
| Trapping pain | Cruelty | Humane sets promoted |
| Trophy imagery | Tone break | Prohibited |

The wild is where the shelter's ethics are tested outside the walls, and the
expansion's contract is that killing is a necessity with a cost, never a
spectacle.

---

## 55. APPENDIX AP — MEASUREMENT AND REVIEW CADENCE

| Gate | Question | Evidence |
|---|---|---|
| Premise | Do live owners still match? | file:line audit |
| Data | Are all rows valid and reachable? | integrity + scanner |
| Core | Are systems pure and deterministic? | unit tests |
| Persistence | Does legacy load neutral? | round-trip tests |
| Host | Is every command real? | selftest + journey |
| UI | Is pressure honest? | lifecycle + a11y tests |
| Tone | Is the hunt restrained? | content review |
| Balance | Is the range sustainable? | 720-day soak |

---

## 56. APPENDIX AQ — FIRST YEAR OF THE WILD

| Month | Focus | Milestone |
|---|---|---|
| 1 | Survey | first sector map |
| 2 | Trap line | repositioned sets |
| 3 | Corridor | migration gap found |
| 4 | Quota | limits posted |
| 5 | Rest | meadow closed |
| 6 | Predators | deterrence chosen |
| 7 | Companions | first taming |
| 8 | Training | first work role |
| 9 | Testing | contaminated game found |
| 10 | Autumn take | quota harvest |
| 11 | Notes | surveys published |
| 12 | Winter count | recovery verified |

A year in the wild is measured in observation and restraint as much as in meat,
and the schedule keeps that rhythm visible to the player from the first month.

---

## 57. CLOSING STATEMENT

ASHFALL already simulates species, predator-prey edges, hunting pressure,
migration, trap sites with seeded outcomes, and companions that eat and can die.
What it lacks is the relationship: stalking, seasons, quotas, predator conflict,
field knowledge, working animals, and the visible arithmetic of taking too much.
The Wild adds that world without adding a second ecology or a second trap
system. It adds a track in wet ground, a quota that keeps a meadow alive, a dog
that works, and the winter the shelter chose to leave the herd alone.

> Wave 5 note: this plan is one of five Wave 5 expansion bibles (32–36). Each is
> self-contained; none requires another to ship. The shared Wave 5 index lives at
> `docs/expansions/wave5/WAVE5_INDEX.md`. The safe pre-signature step is Phase 1
> (data schemas and validators), which is additive and reversible. Evidence
> anchors: `WildlifeEcosystemSystem` (`FaunaSpeciesDef`, `PredatorPreyEdgeDef`,
> `RecordHuntingPressure`, `KnowledgeLevel`, `CanTame`),
> `WildlifeMigrationSystem`, `WildlifeTrappingSystem` (seeded deployment,
> encounter, and incident streams), `CompanionAnimalSystem` (assign and feed
> results), `WildlifeSeasonalCalendar`, `wildlife_ecosystem.json` (3.8 KB), and
> `wildlife_trapping_catalog.json` (23.2 KB).