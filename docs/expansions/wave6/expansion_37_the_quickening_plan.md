# ASHFALL — Expansion 37 Design Bible
# THE QUICKENING
### Wave 6 · Antenatal Care, Birth, Infancy, Early Years, and Family Support

**Document status:** Design plan (pre-integration). Not a claim. Not an authorization.
**Date:** 2026-09-23
**Domain owners touched:** `Ashfall.Core.Survivors` (ChildDevelopmentSystem, GenerationalSystem, CaregivingSystem, CohortSystem), `Ashfall.Core` (GenerationalLineageExtension, SurvivorRelationsSystem), medical pipeline
**Proposed host owner:** `NurseryHostSession` (extends child, lineage, caregiving surfaces)
**Existing save sections:** `ChildDevelopmentState`, `CaregivingSaveState`, `SurvivorRelationsState`, lineage records
**Existing CLI verbs:** `--children-selftest` (if present), `--data-integrity-selftest`, `--content-utilization-selftest`
**Rule compliance:** Godot authoritative; Core engine-free; JSON data authoritative; one authority per concern; deterministic seeded RNG.

---

## 0. HOW TO READ THIS DOCUMENT

This is a **design bible**, not an integration plan. Implementation must later pass
through `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `TEST_POLICY.md`.

ASHFALL already tracks children. `ChildDevelopmentSystem` defines `ChildProfile`
with `ChildId`, `Name`, `BirthDay`, `DevelopmentStage` (default `Infant`),
`AssignedCaregiverId`, `EducationScore`, `ChoreEfficiency`, `ParentIds`, and
`Milestones`, and it exposes `ResolveStage(ageDays)`, `ResolveCanonicalAgeDays`,
`ResolveCanonicalStage`, `ProjectCanonicalChild`, `RegisterChild(child, day)`,
`TickDay(currentDay)`, `RecordEducation`, `AssignCaregiver`,
`GetChoreWorkCapacity`, and `GetChild`. `CohortSystem.CohortChild` carries
`parentIds`, `guessBand`/`trueBand` (the dose-story band), `birthDay`,
`baselineCorrected`, `moralityMemory` ("the story told, not the dose"),
`isMatured`, `maturationDay`, `isDeceased`, and `deathDay`.
`GenerationalLineageExtension` defines `LineageRecord` (`parentId`, `childId`,
`relationshipType` of "parent", "adopted", or "mentor", `establishedDay`,
`isActive`, `inheritedTraitIds`, `spouseId`, `familyName`) and `FamilyUnit`.
`CaregivingSystem` defines `CaregivingAssignmentState` (`CaregiverId`,
`PatientId`, `BondStrength`) with `RecoverySpeedBonus = 0.30f`,
`AffinityGainPerDay = 5f`, `CaregiverFatigueDrain = 0.15f`,
`MinBondForDialogue = 0.5f`, and `BondGrowthPerDay = 0.02f`.
`SurvivorRelationsSystem` records `RelationshipEntry` (`affinity`, `trust`,
`resentment`, `grief`, `bondType`, `recentCauses`) plus `ConflictEntry` and
`MediationEntry`.

What does not exist: pregnancy care, birth attendance, nurseries, infant feeding,
postpartum recovery, early-years development content, family planning privacy
rules, maternity records, and the staff who do the work. There is no maternity
data catalog at all. The life arc is missing its first chapter.

**The Quickening** adds that first chapter: the months before a birth, the night
of it, and the years when a shelter teaches a person to be safe. It extends the
live child, lineage, caregiving, and relations owners and it hands every
consequence to the system that already owns it: medical care, mental health,
grief, education at school age, and the campaign's dose ledger.

Conventions: **`LIVE`** confirmed in source/data; **`GAP`** confirmed thin;
**`PROPOSED`** new.

### 0.1 Boundary contracts

| Neighbour | Owns | This plan does |
|---|---|---|
| 12 The Second Generation | School age, curriculum, youth rites, succession | Hands off at school age (§7.4) |
| 16 The Rebuilt Body | Prosthetics, bionics, therapy devices | Requests assistive devices only |
| 24 The Long Goodbye | Grief, mourning, legacy, memorial | Routes loss through its owners (§7.6) |
| 22 The Clean Flow | Water, hygiene, disease exposure | Uses its exposure and hygiene rules |
| 07 The Dose / dose ledger | Radiation history and bands | Reads bands, never re-derives them |
| 19 The Bitter Air | Hazard exposure protocols | Uses its routes for protection |
| 26 The Common Table | Nutrition and food culture | Requests diets, never invents nutrition |

---

## 1. EXECUTIVE SUMMARY

### 1.1 Pitch

Nobody in the shelter has delivered a baby here yet. The first one is coming in
winter.

**The Quickening** is the expansion about the beginning of life in a place built
for survival after the end of the world: antenatal care, birth attendance,
nurseries, infant feeding, postpartum recovery, early childhood, and the quiet
network of people who hold a family upright. The shelter must build a clinic, a
ward, a nursery, a schedule, and a set of rules about who is allowed to know what
about a person's body.

### 1.2 The five loops it adds

```
   Care ──► Prepare ──► Attend ──► Recover ──► Raise
     │         │           │          │          │
     ▼         ▼           ▼          ▼          ▼
   Visits,   Plans,       Labor,     Rest,      Feeding,
   feeding,  delivery     birth      strength   play,
   rest      room                              milestones
                                                  │
                                                  ▼
                                             Handoff to school ──► 12
```

### 1.3 What the player manages

1. **Antenatal care.** Visits, rest, food, safety, and a written plan.
2. **Delivery.** A clean room, a warm room, attendants, light, and supplies.
3. **Attendance.** Who is in the room and who owns the decisions.
4. **Nursery.** Cots, warmth, feeding, washing, sleep, and staffing.
5. **Recovery.** Postpartum rest and care for a person who has just worked hard.
6. **Infancy.** Feeding, sleep, milestones, illness, and vaccination routes.
7. **Early years.** Play, language, safety, first chores, and development.
8. **Support.** Family schedules, sibling care, and household help.
9. **Records.** What is written, who can read it, and what is never written.
10. **Loss.** The hardest path, handled through the live grief owners.

### 1.4 What it is not

- Not explicit, graphic, or fetishized. Birth happens off-screen in elision and
  aftermath; the expansion is about care, not spectacle.
- Not a eugenics or selection system. Children are never ranked by dose, health,
  or future value.
- Not pronatalist pressure. Family planning is private, supported, and never
  coerced.
- Not a second child-development or lineage system. It extends the live owners.
- Not a second medical, mental-health, or grief system. It routes to them.
- Not a new save section.

---

## 2. EVIDENCE BASE AND GAP ANALYSIS

### 2.1 Live systems (verified)

| File | Role | Status |
|---|---|---|
| `Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` | Child profiles, stages, milestones | `LIVE` |
| `Assets/Ashfall.Core/Survivors/GenerationalSystem.cs` | Phases, education levels, formative events | `LIVE` |
| `Assets/Ashfall.Core/Survivors/CaregivingSystem.cs` | Caregiver assignments and bonds | `LIVE` |
| `Assets/Ashfall.Core/Survivors/CohortSystem.cs` | Cohort children, bands, maturation | `LIVE` |
| `Assets/Ashfall.Core/GenerationalLineageExtension.cs` | Lineage records and family units | `LIVE` |
| `Assets/Ashfall.Core/SurvivorRelationsSystem.cs` | Relationships, conflicts, mediation | `LIVE` |
| `Assets/Ashfall.Core/Medical/MedicalPipelineCoordinator.cs` | Care routing | `LIVE` |
| `Assets/Ashfall.Core/Needs/NeedsSystem.cs` | Hunger, fatigue, warmth, morale, hygiene | `LIVE` |
| `Assets/Ashfall.Core/Shelter/ShelterAssignmentSystem.cs` | Rooms and assignments | `LIVE` |

### 2.2 Live data (counted)

| Catalog | Size | Notes |
|---|---|---|
| Maternity / antenatal / nursery data | absent | confirmed none |
| `ChildDevelopmentSystem` milestones | 7 development traits | code-defined |
| `CaregivingSystem` constants | bond and recovery tuning | code-defined |
| `SurvivorRelationsSystem` | relationship state | code-defined |

### 2.3 Confirmed gaps

- **GAP-37-1 — No pregnancy state.** Nobody is ever expectant in data.
- **GAP-37-2 — No antenatal care content.** No visits, checks, or plans.
- **GAP-37-3 — No birth model.** Birth is not represented at any scale.
- **GAP-37-4 — No nursery.** Cots, feeding, warmth, and washing are absent.
- **GAP-37-5 — No postpartum recovery content.**
- **GAP-37-6 — No infant feeding content.**
- **GAP-37-7 — No early-years activity or milestone content.**
- **GAP-37-8 — No family planning or privacy rules.**
- **GAP-37-9 — No maternity staff or training content.**
- **GAP-37-10 — No maternity records or confidentiality table.**

### 2.4 Non-duplication statement

This expansion will **not** add a second child-development, lineage, caregiving,
relations, medical, mental-health, or grief system. It extends
`ChildDevelopmentSystem` for early-years content, `GenerationalLineageExtension`
for family records, `CaregivingSystem` for care assignments, and the medical
pipeline for clinical care. It adds state only as additive sub-objects of the
existing child, caregiving, lineage, and medical stores. No new save section.

---

## 3. DESIGN PILLARS AND TONE

### 3.1 Pillars

**Pillar 1 — Care is labor, and labor is a schedule.** Someone feeds the baby at
two in the morning and someone else sleeps; the expansion makes that visible.

**Pillar 2 — The body is not a scorecard.** Dose bands, health, and outcomes are
history, not rank. No mechanic ever compares children.

**Pillar 3 — Privacy is a load-bearing wall.** A person's body and medical
history are theirs; the shelter keeps records sealed by default.

**Pillar 4 — The first years are infrastructure.** A nursery, a schedule, and a
clean room are as real as a generator.

**Pillar 5 — Loss is met with company, not with a mechanic.** The hardest paths
route to the grief owners and never reward the player for them.

### 3.2 Tone calibration

| Element | Do | Do not |
|---|---|---|
| Pregnancy | Quiet preparation | Medical spectacle |
| Birth | Elision, aftermath, relief | Graphic depiction |
| Newborn | Warmth, fragility, routine | Cuteness product |
| Feeding | Practical support | Moralized choice |
| Early years | Play, language, safety | Accelerated labor |
| Loss | Company, silence, care | Fail-state language |
| Records | Privacy and trust | Public ledger |
| Family planning | Private choice | Pressure or quota |

### 3.3 Content limits (binding)

- No explicit sexual, obstetric, or anatomical content. Birth is represented by
  onset, attendance, elapsed time, and outcome; the event itself is elided.
- No graphic medical detail, no imagery of bodies in distress, no gore.
- No child ranking, no eugenics, no dose-based selection, no "defective" framing.
- No pronatalist pressure, no quotas, no rewards for births.
- Infertility and loss are never failures; the shelter supports without judgment.
- Children are never exposed to harm as content. Nothing in this expansion puts
  a child in danger for drama.
- The plan never uses a child's death as a reward, lesson, or milestone token;
  loss routes to `MemorialSystem` and grief sinks.

---

## 4. THE QUICKENING WORLD

### 4.1 Interior rooms

- **`room_antenatal_clinic`** — visiting room, scales, records cabinet.
- **`room_delivery_room`** — clean, warm, private, with a bell.
- **`room_nursery`** — cots, low light, washing corner.
- **`room_mothers_rest`** — recovery beds near the nursery.
- **`room_feeding_room`** — quiet, warm, screened.
- **`room_early_years_room`** — padded floor, toys, low shelves.
- **`room_records_office`** — sealed files and the registrar's desk.
- **`room_linen_store`** — cloth, bedding, and clean wraps.

### 4.2 Exterior locations

| ID | Name | Tone | Purpose |
|---|---|---|---|
| `loc_clinic_door` | The Clinic Door | 2 | Where visits begin |
| `loc_wash_line` | The Wash Line | 2 | Cloth and hygiene |
| `loc_sheltered_yard` | The Sheltered Yard | 2 | First outdoor air |
| `loc_kettle_corner` | The Kettle Corner | 1 | Waiting families |
| `loc_quiet_garden` | The Quiet Garden | 2 | Rest and green |
| `loc_handcart_lane` | The Handcart Lane | 3 | Supply runs |
| `loc_school_door` | The School Door | 2 | The handoff at five |
| `loc_winter_walk` | The Winter Walk | 3 | Cold-weather care |
| `loc_memorial_wall` | The Memorial Wall | 2 | Loss and remembering |
| `loc_north_gate_clinic` | The North Gate Post | 4 | Arrivals who need care |

All locations require valid item references and scanner registration.

### 4.3 The first year

The expansion's clock: one pregnancy, one winter, one birth, one year of a
child's life. The shelter learns its own clinic in the time it takes a person to
grow from nothing to standing.

---

## 5. MAIN STORYLINE — "FIRST BREATH"

### 5.1 Central conflict

**Maeve Orr** is pregnant and the shelter has no clinic, no midwife, no nursery,
and no plan. **Esme Var** has delivered babies in worse places and wants the
shelter to prepare properly. **Sim Okoye** the doctor wants a delivery room and a
clean water line. **Ivy Dunn** wants a nursery with a schedule. **Bex** the
grandmother wants the old ways respected without pretending they can replace
clean hands and warm water. **Ness** the registrar wants records that protect
people. **Olem**, the father, wants to be useful and is told at every turn that
the useful thing is patience.

Then winter comes early, the clinic is not finished, and the shelter discovers
that the hardest part of a birth is the week after it.

The expansion's question: **what does a shelter owe the first person it brings
into the world?**

### 5.2 Theme (unspoken)

**Everything a shelter knows about care, it learns on the smallest patient.**

### 5.3 Principal NPCs

| ID | Name | Role | Function |
|---|---|---|---|
| `npc_midwife_esme_var` | Esme Var | Midwife | Care, attendance, training |
| `npc_expectant_maeve_orr` | Maeve Orr | Expectant mother | The first birth |
| `npc_nurse_tam_ferro` | Tam Ferro | Nurse | Clinic, records, supplies |
| `npc_nursery_keeper_ivy_dunn` | Ivy Dunn | Nursery keeper | Cots, feeding, schedule |
| `npc_grandmother_bex` | Bex | Elder | Tradition, hands, comfort |
| `npc_doctor_sim_okoye` | Sim Okoye | Physician | Medicine, risk, referral |
| `npc_registrar_ness` | Ness | Registrar | Records and privacy |
| `npc_father_olem` | Olem | Father | Support, repairs, patience |

### 5.4 Story beats (15)

1. **The News.** A pregnancy is confirmed and the shelter has nothing ready.
2. **The List.** Esme writes what a birth needs; the list is long.
3. **The Room.** A delivery room is cleaned, warmed, and prepared.
4. **The Visits.** Antenatal care becomes a schedule.
5. **The Water.** A clean line reaches the clinic.
6. **The Nursery.** Cots, cloth, and a night rota.
7. **The Plan.** Maeve writes what she wants and who decides.
8. **The Winter.** Early snow and a closed road.
9. **The Onset.** Labor begins in the cold hours.
10. **The Birth.** Hours, attendants, and elision; a child arrives.
11. **The Week.** Postpartum recovery and the first hard nights.
12. **The Feed.** Feeding works, then stops working, then works again.
13. **The Records.** Ness seals the file and explains why.
14. **The Milestones.** The first year passes; the child stands.
15. **First Breath.** The shelter decides what it has become.

### 5.5 Branching choices (8)

| Choice | Options | Axis |
|---|---|---|
| Delivery room | clinic / home room / improvised | safety vs. comfort |
| Attendance | midwife only / team / family present | privacy vs. support |
| Feeding plan | nursing / cloth and gruel / mixed | preference vs. supply |
| Nursery | central / family rooms / shift rotation | schedule vs. attachment |
| Records | sealed / family / open | privacy vs. learning |
| Recovery | full rest / shared care / early return | health vs. labor |
| Handoff age | five / six / readiness | institution vs. family |
| Final | clinic as institution / as practice / as memory | identity |

### 5.6 Endings (5 + fade)

1. **The Warm Room** — the clinic, the nursery, and the rota work, and the next
   birth is easier.
2. **The House Full** — families keep their children close and the nursery
   becomes a shared room rather than an institution.
3. **The Quiet Handoff** — the first child walks to the school door and the
   shelter has completed an arc it did not know it was building.
4. **The Long Winter** — a hard season teaches hard lessons; the clinic rebuilds
   with what it learned.
5. **The Empty Cot** — a loss the shelter mourns through its own memorial
   systems, and the expansion never treats it as a score.
6. **Fade** — a cradle by a heater and a kettle on, and someone awake.

---

## 6. QUEST DESIGN

New IDs use prefix `quest_quickening_`. Schema follows existing quest catalogs.

### 6.1 Main questline (15)

`quest_quickening_news`, `quest_quickening_list`, `quest_quickening_room`,
`quest_quickening_visits`, `quest_quickening_water`, `quest_quickening_nursery`,
`quest_quickening_plan`, `quest_quickening_winter`, `quest_quickening_onset`,
`quest_quickening_birth`, `quest_quickening_week`, `quest_quickening_feed`,
`quest_quickening_records`, `quest_quickening_milestones`,
`quest_quickening_first_breath`.

### 6.2 Side quests (30)

**Antenatal (5)**
- `quest_quickening_check` — a routine visit
- `quest_quickening_rest` — enforce rest
- `quest_quickening_food` — feeding the mother well
- `quest_quickening_walk` — gentle movement
- `quest_quickening_worry` — a concern heard properly

**Birth (5)**
- `quest_quickening_kit` — assemble the birth kit
- `quest_quickening_room_warm` — heat the room
- `quest_quickening_bell` — set the call bell
- `quest_quickening_hands` — train attendants
- `quest_quickening_after` — the first hour

**Nursery (5)**
- `quest_quickening_cots` — build cots
- `quest_quickening_rota` — write the night rota
- `quest_quickening_wash` — laundry for infants
- `quest_quickening_quiet` — a quiet corner
- `quest_quickening_watch` — night nursery watch

**Early years (5)**
- `quest_quickening_play` — a safe play space
- `quest_quickening_words` — language and songs
- `quest_quickening_steps` — first walking practice
- `quest_quickening_safety` — childproofing
- `quest_quickening_chores` — first small tasks

**Support (5)**
- `quest_quickening_siblings` — sibling care
- `quest_quickening_sleep` — the sleep plan
- `quest_quickening_help` — household help
- `quest_quickening_counsel` — support after a hard week
- `quest_quickening_family` — a family record

**Records (5)**
- `quest_quickening_file` — open a sealed file
- `quest_quickening_names` — the naming record
- `quest_quickening_handoff` — the school-door handoff
- `quest_quickening_review` — a care review
- `quest_quickening_archive` — the first-year archive

### 6.3 Repeatable quests (8)

`quest_quickening_repeat_visit`, `quest_quickening_repeat_feed`,
`quest_quickening_repeat_wash`, `quest_quickening_repeat_night`,
`quest_quickening_repeat_play`, `quest_quickening_repeat_check`,
`quest_quickening_repeat_record`, `quest_quickening_repeat_restock`.

### 6.4 Dynamic hooks

Live events (births, deaths, illness, exhaustion, relationship changes, water
quality, weather, clinic supplies) attach authored follow-ups through existing
seams. No new event bus.

### 6.5 Constraints

- Child state remains with `ChildDevelopmentSystem`; lineage with
  `GenerationalLineageExtension`; care bonds with `CaregivingSystem`; grief with
  the memorial and grief owners.
- Dose bands are read, never recomputed or ranked.
- No explicit content; birth is elided.
- No new save section.

---

## 7. NEW GAMEPLAY SYSTEMS

### 7.1 `AntenatalCareSystem` (new, `Ashfall.Core.Survivors`)

**Owns:** pregnancy state, visit schedules, care flags, and risk notes for
expectant survivors. **Consumes:** `NeedsSystem`, medical pipeline, dose ledger
(read-only), `CaregivingSystem`, `ChildDevelopmentSystem` (registration at
birth). **Data:** `antenatal_care.json`, `pregnancy_visits.json`.
**Rules:** pregnancy advances with campaign days; care visits are scheduled and
recorded; flags are clinical prompts routed to the medical owner, never
standalone conditions. Deterministic from day, dose band, care, and needs.

### 7.2 `BirthSystem` (new, `Ashfall.Core.Survivors`)

**Owns:** birth plans, attendance rosters, onset, elapsed labor, and outcome
bands for mother and child. **Consumes:** `AntenatalCareSystem`,
`CaregivingSystem`, `MedicalPipelineCoordinator`, `MedicalWardSystem` (referral),
`ChildDevelopmentSystem.RegisterChild`, `GenerationalLineageExtension`.
**Data:** `birth_plans.json`, `birth_attendance.json`.
**Rules:** onset and duration resolve deterministically; outcome bands are
influenced by care, staffing, readiness, and dose history; loss outcomes route to
the live grief and memorial owners with no reward or score. Attendance records
who was present, who decided, and what was done.

### 7.3 `NurserySystem` (new, `Ashfall.Core.Survivors`)

**Owns:** cots, nursery rooms, infant care schedules, feeding support, and night
rotas. **Consumes:** `ChildDevelopmentSystem`, `CaregivingSystem`,
`ShelterAssignmentSystem`, `NeedsSystem`, `FoodPreservationSystem` (Wave 3) for
safe feeding stores, `KitchenNutritionSystem` for diets (requests only).
**Data:** `nursery_schedules.json`, `infant_feeding.json`.
**Rules:** an infant has a care schedule with wake, feed, wash, and sleep blocks;
missed care degrades through the live needs and bond systems, never through a
hidden infant meter. Feeding methods are player preference with supply-based
consequences, never moralized.

### 7.4 `EarlyYearsSystem` (new, `Ashfall.Core.Survivors`)

**Owns:** early-years activities, safety, language, movement, and small-task
content for ages zero to five. **Consumes:** `ChildDevelopmentSystem` stages and
milestones, `GenerationalSystem` phases, `GetChoreWorkCapacity`, schooling
handoff. **Data:** `early_years_activities.json`, `child_milestones.json`.
**Rules:** milestones come from the live stage resolver and are recorded, never
awarded as currency; at school age the child is handed to 12's systems with a
complete, private record. No child is ever given hazardous work; chore capacity
is bounded by the live capacity function.

### 7.5 `MaternalSupportSystem` (new, thin, `Ashfall.Core.Survivors`)

**Owns:** postpartum recovery schedules, feeding support, and referral routing
for mood and health concerns. **Consumes:** `CaregivingSystem`,
`MedicalPipelineCoordinator`, mental-health owners, `NeedsSystem`.
**Data:** `maternal_support.json`. **Rules:** recovery is rest and help; concerns
route to the live mental-health systems; nothing in this system diagnoses,
scores, or blames.

### 7.6 Loss path (no new system)

Loss outcomes produce facts that are handed to `MemorialSystem`,
`RelationsGriefSink`, and the mental-health owners. This plan adds prose,
procedures, and dignity, not a mechanic.

### 7.7 `MaternityRecordsSystem` (new, thin, extends records owners)

**Owns:** sealed maternity files, consent flags, and the confidentiality table.
**Consumes:** `MedicalRecordLog`, `PatientRecord`, archive owners.
**Data:** `maternity_records.json`. **Rules:** sealed by default; family access
by consent; clinical access for care; nothing about a body is public data.

### 7.8 Systems explicitly not added

- No second child, lineage, caregiving, relations, medical, or grief system.
- No dose selection, ranking, or eugenics content.
- No explicit content of any kind.
- No new currency, no birth rewards, no population score.
- No new RNG stream.
- No new save section.

---

## 8. DATA CATALOG SPECIFICATION

All catalogs snake_case, integer `schema_version: 1`, validated and scanner-registered.

### 8.1 `antenatal_care.json` (new)

```json
{
  "schema_version": 1,
  "visits": [
    {
      "visit_id": "visit_first",
      "display_name": "First Visit",
      "week_min": 6,
      "week_max": 12,
      "checks": ["rest", "food", "worry"],
      "staff": ["midwife", "nurse"],
      "duration_hours": 1,
      "tags": ["clinic", "private"]
    }
  ]
}
```

### 8.2 `pregnancy_visits.json` (new)

Schedule rows: week band, visit, required checks, referral triggers.

### 8.3 `birth_plans.json` (new)

Plans: room, attendance, decisions, preferences, backups.

### 8.4 `birth_attendance.json` (new)

Attendance: role, person, training, decisions owned, hours.

### 8.5 `nursery_schedules.json` (new)

Routines: wake, feed, wash, sleep, quiet, night rota.

### 8.6 `infant_feeding.json` (new)

Feeding: method, supplies, support, supply notes, safe storage.

### 8.7 `early_years_activities.json` (new)

Activities: play, language, movement, safety, first tasks, ages, supervision.

### 8.8 `child_milestones.json` (new)

Milestones: stage, event, record, private flag, handoff note.

### 8.9 `maternal_support.json` (new)

Support: recovery, feeding help, mood referral, household help, sleep plans.

### 8.10 `maternity_records.json` (new)

Record rules: field, seal level, consent requirement, retention, access roles.

### 8.11 Items

New items appended to `items.json`: `item_birth_kit`, `item_clean_wraps`,
`item_cradle`, `item_nursery_lamp`, `item_feeding_cup`, `item_cloth_diapers`,
`item_scales`, `item_warm_stone`, `item_quiet_blanket`, `item_child_creep_pen`,
`item_soft_play_mat`, `item_first_steps_bar`, `item_record_cabinet`,
`item_sealed_folder`, `item_kettle_large`, `item_washtub_small`.

---

## 9. SAVE, DETERMINISM, AND PERSISTENCE

### 9.1 Ownership

`ChildDevelopmentState`, `CaregivingSaveState`, `SurvivorRelationsState`, and
lineage records remain the live save owners. New sub-objects (pregnancy,
antenatal visits, birth records, nursery state, early-years milestones,
maternity files) are additive inside them. No new save section.

### 9.2 State to persist

- Pregnancy state, due windows, and care schedules.
- Antenatal visit history and flags.
- Birth plans, attendance, and outcome records.
- Nursery resources, schedules, and rotas.
- Feeding methods and supplies.
- Early-years activities and recorded milestones.
- Maternal recovery state and referrals.
- Record seals and consent flags.

### 9.3 Determinism

- Pregnancy progression is a pure function of campaign day.
- Birth onset and duration derive from day, care score, staff, and dose history
  read from the live ledger; variation uses the campaign's seeded stream only.
- Milestones resolve through `ChildDevelopmentSystem` stage functions.
- Care bonds use `CaregivingSystem` constants.
- Paired replay hashes must match; no wall-clock or `System.Random`.

### 9.4 Migration

Legacy saves load with children, lineage, and relations untouched; no pregnancy,
nursery, or file state exists until started. Existing `CohortChild` records and
`ChildProfile` entries keep working. A legacy child crossing school age hands to
12 normally.

### 9.5 Checksum

Invariant-culture floats; integer days, counts, and visit numbers.

---

## 10. UI, ACCESSIBILITY, AND PRESENTATION

### 10.1 Surfaces

| Surface | Purpose | Owner |
|---|---|---|
| `ClinicPanel` (new) | Visits, schedule, care flags | `NurseryHostSession` |
| `BirthPlanPanel` (new) | Plan, attendance, decisions | same |
| `NurseryPanel` (new) | Cots, routines, rota | same |
| `EarlyYearsPanel` (new) | Activities and milestones | same |
| `SupportPanel` (new) | Recovery and household help | same |
| `RecordsPanel` (new) | Sealed files and consent | same |
| `FamilyPanel` (new) | Lineage and household view | same |

### 10.2 Accessibility and honesty

- Panels show live state and expose existing commands only.
- No countdown timers create pressure; schedules are readable cards.
- Records show their seal level before opening.
- Keyboard/controller close/back preserved; focus maintained on refresh.
- All content has text equivalents; nothing critical is audio-only.
- Content is written so it can be read aloud without embarrassment.

### 10.3 Presentation

Audio cues appended to `audio_cues.json`: a kettle, a bell, a cradle creak,
quiet footsteps, a lullaby hum, a door closing softly. No cue is required; text
carries meaning.

---

## 11. INTEGRATION SEAMS

| Existing system | Attachment |
|---|---|
| `ChildDevelopmentSystem` | Registration, stages, milestones, handoff |
| `GenerationalLineageExtension` | Lineage and family records |
| `CaregivingSystem` | Care assignments and bonds |
| `CohortSystem` | Cohort bands and maturation |
| `SurvivorRelationsSystem` | Bonds, conflicts, mediation |
| `MedicalPipelineCoordinator` | Referrals and care |
| `MedicalWardSystem` | Surgical and ward referral |
| `NeedsSystem` | Rest, food, warmth, hygiene |
| `ShelterAssignmentSystem` | Nursery and clinic rooms |
| `KitchenNutritionSystem` | Diets (requests only) |
| `FoodPreservationSystem` | Safe feeding stores |
| `MemorialSystem` | Loss and remembrance |
| `RelationsGriefSink` | Grief routing |
| `MentalHealthCrisisSystem` | Mood and crisis referral |
| `DoseLedgerSystem` | Read-only dose history |
| `EpilogueChronicleBuilder` | First-year milestones |

---

## 12. TECHNICAL IMPLEMENTATION PLAN

### 12.1 Phase order

**Phase 0 — Premise re-audit.** Confirm `ChildDevelopmentSystem`,
`GenerationalSystem`, `CaregivingSystem`, `CohortSystem`, lineage extension,
relations, medical pipeline, and needs owners. Record file:line; change nothing.

**Phase 1 — Data + validators.** Author the ten catalogs; append items; register
validators and scanner.

**Phase 2 — Pure Core.** `AntenatalCareSystem`, `BirthSystem`, `NurserySystem`,
`EarlyYearsSystem`, `MaternalSupportSystem`, `MaternityRecordsSystem`.

**Phase 3 — Persistence.** Additive sub-objects, migration, round-trip,
determinism.

**Phase 4 — Host + CLI.** `NurseryHostSession`, selftest coverage, fresh
journey from news to handoff.

**Phase 5 — UI.** New panels with lifecycle and accessibility.

**Phase 6 — Content.** Rooms, locations, NPCs, quests, items, prose, audio.

**Phase 7 — Balance.** 360-day soak: a pregnancy, a birth, a year of infancy,
and the handoff.

**Phase 8 — Verification and closeout.**

### 12.2 Content volume

| Content | Count |
|---|---|
| Antenatal visits | 12 |
| Birth plan options | 10 |
| Attendance roles | 8 |
| Nursery routines | 10 |
| Feeding methods | 8 |
| Early-years activities | 24 |
| Milestones | 30 |
| Support services | 10 |
| Record rules | 12 |
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
| Explicit content | Critical | Elision contract §3.3 |
| Eugenics or ranking | Critical | No-score contract |
| Pronatalist pressure | Critical | Private choice protocol |
| Loss as game over | High | Grief routing, no reward |
| Second child system | High | Extend live owners |
| Privacy leak | High | Seal table |
| Determinism break | Low | Live seeded paths |
| Content overrun | Medium | Budget §13 |

---

## 13. APPENDIX A — AUTHORING BUDGET

| File | Rows | Est. words |
|---|---|---|
| `antenatal_care.json` | 12 | 3,000 |
| `pregnancy_visits.json` | 12 | 2,500 |
| `birth_plans.json` | 10 | 2,500 |
| `birth_attendance.json` | 8 | 2,000 |
| `nursery_schedules.json` | 10 | 2,500 |
| `infant_feeding.json` | 8 | 2,000 |
| `early_years_activities.json` | 24 | 4,000 |
| `child_milestones.json` | 30 | 4,500 |
| `maternal_support.json` | 10 | 2,500 |
| `maternity_records.json` | 12 | 2,500 |
| Quest objectives | 53 quests | 15,000 |
| NPC prose | 8 NPCs | 7,000 |
| Location prose | 10 | 3,500 |
| Item descriptions | 16 | 2,500 |
| Ending prose | 6 | 3,000 |
| **Total** | | **~59,000** |

---

## 14. APPENDIX B — RISK REGISTER

| ID | Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|---|
| R37-1 | Explicit content | Low | Critical | Elision rules |
| R37-2 | Dose-based ranking | Low | Critical | No-score contract |
| R37-3 | Pronatalism | Med | Critical | Private choice |
| R37-4 | Loss as fail state | Med | High | Grief routing |
| R37-5 | Second child system | Low | High | Extend live owners |
| R37-6 | Privacy leak | Med | High | Seal table |
| R37-7 | Hidden infant meter | Med | High | Live needs only |
| R37-8 | Determinism | Low | High | Pure functions |
| R37-9 | Content overrun | Med | Med | Budget §13 |
| R37-10 | Handoff friction | Med | Med | Boundary §0.1 |

---

## 15. APPENDIX C — OPEN DECISIONS REQUIRING A FOREMAN SIGNATURE

1. **Is pregnancy visible in the survivor list, or private until chosen?**
   Recommended: private by default, revealed by the person's choice.
2. **Can outcomes be influenced by preparation?** Recommended: yes, meaningfully
   but never deterministically; care improves odds and never guarantees.
3. **Who decides in the delivery room?** Recommended: the person giving birth;
   the plan records that decision and the shelter honors it.
4. **When is the handoff to schooling?** Recommended: readiness-based with a
   default of five years, per the branch in §5.5.
5. **Is loss ever preventable by perfect play?** Recommended: no; perfect care
   is the right of every person and never a guarantee.

---

## 17. APPENDIX D — ANTENATAL VISIT TABLE (12 VISITS)

| # | Visit | Week band | Checks | Staff | Notes |
|---|---|---|---|---|---|
| 1 | First visit | 6–12 | rest, food, worry | midwife, nurse | history taken |
| 2 | Check one | 12–16 | rest, food | nurse | supply review |
| 3 | Check two | 16–20 | rest, safety | midwife | work adjusted |
| 4 | Midpoint | 20–24 | food, sleep, work | midwife | plan drafted |
| 5 | Plan visit | 24–28 | plan, room | midwife, doctor | delivery choice |
| 6 | Check three | 28–30 | rest, mood | nurse | support added |
| 7 | Check four | 30–32 | food, movement | midwife | walking plan |
| 8 | Near visit | 32–34 | rest, supplies | nurse | kit checked |
| 9 | Check five | 34–36 | all | midwife, doctor | team briefed |
| 10 | Ready visit | 36–38 | room, kit, bell | midwife | rota posted |
| 11 | Watch visit | 38–40 | daily notes | nurse | family on call |
| 12 | After visit | 0–6 days | recovery | midwife | first record |

Visits are a schedule and a relationship. The table is written so a player can
see what care a person is owed and whether the shelter actually gave it.

---

## 18. APPENDIX E — BIRTH KIT TABLE

| Item | Purpose | Amount | Source | Replacement |
|---|---|---|---|---|
| Clean wraps | warmth, drying | 6 | wash line | weekly |
| Warm stones | heat | 4 | kiln | per use |
| Kettle | boiling water | 1 large | workshop | rare |
| Washtub | clean water | 1 small | workshop | rare |
| Candle or lamp | light | 2 | stores | burn out |
| Cord ties | cord care | 3 | clinic | per use |
| Scissors | cutting | 1 clean | clinic | sterilize |
| Aprons | staff | 2 | thread | wash |
| Soap | hand hygiene | 1 bar | reagent works | monthly |
| Bucket | waste | 1 | stores | rare |
| Blankets | mother | 2 | thread | wash |
| Drinking water | mother | 2 L | clean flow | daily |
| Soft cloths | cleaning | 6 | thread | wash |
| Pad stock | care | 12 | thread | per use |
| Record sheet | notes | 1 | press | per birth |
| Bell | call for help | 1 | stores | rare |

The list is the expansion's quiet argument that a birth is an infrastructure
event. A missing kettle matters more than a missing speech.

---

## 19. APPENDIX F — BIRTH ATTENDANCE TABLE

| Role | Trained by | Owns | Present when | Backup |
|---|---|---|---|---|
| Lead midwife | experience | decisions, care | always | senior nurse |
| Nurse | clinic | supplies, records | always | aide |
| Doctor | medical | referrals, risk | flagged | midwife |
| Helper | midwife | water, warmth, linens | always | family |
| Runner | any | messages, fetching | always | neighbor |
| Family member | chosen by mother | support | if chosen | none |
| Registrar | clerk | after record | after | clerk |
| Night aide | rota | relief | nights | helper |

Attendance is a roster with clear ownership. The table is also a privacy
document: it records who may be in the room only because the person giving
birth said so.

---

## 20. APPENDIX G — NURSERY ROUTINE TABLE

| Block | Time | Care | Staff | Quiet rule |
|---|---|---|---|---|
| First light | 06:00 | wash, feed | keeper | voices low |
| Morning | 08:00 | cot change, play | keeper, helper | yes |
| Midday | 12:00 | feed, sleep | keeper | strict |
| Afternoon | 14:00 | quiet play | helper | yes |
| Late | 17:00 | feed, wash | keeper | yes |
| Evening | 20:00 | settle | keeper | strict |
| Night one | 23:00 | night feed | rota | strict |
| Night two | 02:00 | check, settle | rota | strict |
| Night three | 05:00 | early feed | rota | strict |
| Handover | 06:00 | notes, count | both | yes |

Routines are how a nursery survives a bad night. The rota is the expansion's
most important nursery content and the least visible one.

---

## 21. APPENDIX H — INFANT FEEDING TABLE

| Method | Supplies | Support | Notes | Not moralized |
|---|---|---|---|---|
| Nursing | food for mother | feeding room | works with supply | choice |
| Cloth and gruel | grain, clean water | kitchen | steady supply | choice |
| Mixed | both | clinic | common answer | choice |
| Cup feeding | clean cups | clinic | gentle route | choice |
| Animal milk | herd access | barn | careful storage | choice |
| Broths | kitchen | cook | warm, thin | choice |
| Emergency | stores | clinic | scarcity only | choice |
| Supplements | clinic | doctor | clinical need | choice |

Feeding is support, not scoring. The expansion's rule is that the shelter helps
the method work rather than ranking the method.

---

## 22. APPENDIX I — EARLY YEARS ACTIVITY TABLE (24 ACTIVITIES)

| # | Activity | Age band | Learns | Supervision |
|---|---|---|---|---|
| 1 | Peek and find | first months | attention | keeper |
| 2 | Soft blocks | first months | grasping | keeper |
| 3 | Songs | all | sound, rhythm | any adult |
| 4 | Naming things | all | words | any adult |
| 5 | Crawl track | 6–12 months | movement | keeper |
| 6 | Stacking cups | 6–12 months | coordination | keeper |
| 7 | Mirror play | 6–12 months | self | keeper |
| 8 | First steps bar | 9–15 months | walking | keeper |
| 9 | Rolling ball | 12–24 months | play | helper |
| 10 | Picture cards | 12–24 months | words | helper |
| 11 | Sand tray | 18–30 months | touch | helper |
| 12 | Water pouring | 18–30 months | control | helper |
| 13 | Chalk drawing | 24–36 months | expression | helper |
| 14 | Counting stones | 24–36 months | numbers | helper |
| 15 | Sorting chores | 30–48 months | order | adult |
| 16 | Story circle | 30–48 months | listening | any adult |
| 17 | Carrying tasks | 36–60 months | helping | adult |
| 18 | Garden watering | 36–60 months | care | adult |
| 19 | Simple knots | 48–60 months | craft | adult |
| 20 | Kitchen help | 48–60 months | safety | adult |
| 21 | Path walks | 36–60 months | familiarity | adult |
| 22 | Quiet play | all | rest | keeper |
| 23 | Group games | 36–60 months | fairness | adult |
| 24 | Record of firsts | all | memory | registrar |

Activities are play with a purpose that never becomes labor. The last row is
the point of the table: the shelter remembers, privately and kindly.

---

## 23. APPENDIX J — MILESTONE TABLE (30 MILESTONES)

| # | Milestone | Stage | Record | Private |
|---|---|---|---|---|
| 1 | First feed | newborn | yes | yes |
| 2 | First sleep cycle | newborn | yes | yes |
| 3 | Cord healed | newborn | yes | yes |
| 4 | First bath | newborn | yes | yes |
| 5 | First smile | infant | yes | yes |
| 6 | Head held | infant | yes | yes |
| 7 | Rolls over | infant | yes | yes |
| 8 | Sits up | infant | yes | yes |
| 9 | First food | infant | yes | yes |
| 10 | Crawls | infant | yes | yes |
| 11 | First tooth | infant | yes | yes |
| 12 | Pulls to stand | infant | yes | yes |
| 13 | First steps | toddler | yes | yes |
| 14 | First word | toddler | yes | yes |
| 15 | Names people | toddler | yes | yes |
| 16 | Runs | toddler | yes | no |
| 17 | Feeds self | toddler | yes | no |
| 18 | Washes hands | toddler | yes | no |
| 19 | Asks why | toddler | yes | no |
| 20 | Sorts shapes | toddler | yes | no |
| 21 | Counts to five | toddler | yes | no |
| 22 | Helps carry | toddler | yes | no |
| 23 | Knows the paths | child | yes | no |
| 24 | Tells a story | child | yes | no |
| 25 | Makes a friend | child | yes | no |
| 26 | Keeps a promise | child | yes | no |
| 27 | Ties a knot | child | yes | no |
| 28 | Listens fully | child | yes | no |
| 29 | Asks for help | child | yes | no |
| 30 | Walks to school | child | yes | no |

Milestones are records, not trophies. Nothing here grants currency, points, or
privilege, and the private column is a promise the shelter keeps.

---

## 24. APPENDIX K — SUPPORT SERVICE TABLE

| Service | Who | When | Route |
|---|---|---|---|
| Recovery rest | mother | after birth | needs system |
| Feeding help | clinic | as asked | clinic |
| Household help | neighbors | weeks after | steward |
| Sibling care | helpers | school-day hours | nursery |
| Sleep plan | keeper | first year | nursery |
| Mood support | counselor | as asked | mental health |
| Legal record | registrar | birth | records |
| Food support | kitchen | as needed | kitchen |
| Warmth support | stores | winter | inventory |
| Respite hour | rota | weekly | nursery |

Support services are the expansion's answer to the naive belief that a clinic
alone raises a child. The table is mostly ordinary help given by ordinary
neighbors, scheduled so it actually happens.

---

## 25. APPENDIX L — MATERNITY RECORD PRIVACY TABLE

| Field | Seal | Family | Clinical | Archive |
|---|---|---|---|---|
| Name | family | yes | yes | yes |
| Birth day | family | yes | yes | yes |
| Visit notes | sealed | consent | yes | no |
| Risk flags | sealed | consent | yes | no |
| Dose history | sealed | consent | yes | no |
| Birth record | family | yes | yes | yes |
| Feeding notes | sealed | consent | yes | no |
| Recovery notes | sealed | consent | yes | no |
| Loss record | sealed | yes | yes | yes |
| Consent form | sealed | yes | yes | yes |
| Handoff note | family | yes | yes | yes |
| Access log | sealed | no | yes | yes |

The access log is the expansion's final safeguard: every time a sealed file is
opened, the reason and the reader are recorded, and the family can ask.

---

## 26. APPENDIX M — CLINIC AND NURSERY EQUIPMENT TABLE

| Equipment | Room | Source | Upkeep | Criticality |
|---|---|---|---|---|
| Cot | nursery | workshop | repairs | high |
| Cradle | nursery | workshop | repairs | high |
| Scales | clinic | workshop | calibration | med |
| Lamp | both | glass | fuel | high |
| Warm stones | delivery | kiln | reheat | high |
| Washtub | both | workshop | clean | high |
| Line | wash line | thread | restring | med |
| Cabinet | records | workshop | lock | high |
| Screen | feeding | thread | wash | med |
| Bell | delivery | stores | replace | med |
| Bench | clinic | workshop | repairs | low |
| Stool | clinic | workshop | repairs | low |
| Warm bricks | delivery | kiln | reheat | med |
| Soft mat | early years | thread | wash | med |
| Shelf | early years | workshop | repairs | low |
| Record box | records | press | seal | high |

Equipment is where the expansion touches the other waves: kiln, thread, glass,
workshop, and press all supply the smallest room in the shelter.

---

## 27. APPENDIX N — MATERNITY STAFFING AND TRAINING TABLE

| Role | Prerequisite | Training | Hours | Recertify |
|---|---|---|---|---|
| Midwife | medical basics | 6 weeks | full | yearly |
| Nurse | first aid | 4 weeks | full | yearly |
| Helper | none | 1 week | shifts | no |
| Runner | none | 1 day | on call | no |
| Nursery keeper | patience | 2 weeks | rota | yearly |
| Night aide | keeper trust | 1 week | nights | no |
| Registrar | literacy | 1 week | office | no |
| Counselor | training | existing | sessions | existing |

Training is modest, local, and honest: the shelter teaches the people it has,
and the plan never pretends that six weeks makes a person equal to a lifetime
of practice.

---

## 28. APPENDIX O — MAIN QUESTLINE STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_quickening_news` | 4 | Confirmation and quiet |
| `quest_quickening_list` | 4 | Esme's list assembled |
| `quest_quickening_room` | 5 | Delivery room prepared |
| `quest_quickening_visits` | 4 | Visit schedule running |
| `quest_quickening_water` | 4 | Clean water at the clinic |
| `quest_quickening_nursery` | 5 | Cots and rota ready |
| `quest_quickening_plan` | 4 | Birth plan written |
| `quest_quickening_winter` | 5 | Supplies secured before snow |
| `quest_quickening_onset` | 3 | Labor begins |
| `quest_quickening_birth` | 5 | Attendance and outcome |
| `quest_quickening_week` | 5 | First week of recovery |
| `quest_quickening_feed` | 4 | Feeding support works |
| `quest_quickening_records` | 4 | File sealed and explained |
| `quest_quickening_milestones` | 4 | First year recorded |
| `quest_quickening_first_breath` | 3 | Final disposition |

---

## 29. APPENDIX P — SIDE QUEST STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_quickening_check` | 3 | Visit kept and noted |
| `quest_quickening_rest` | 3 | Work reduced |
| `quest_quickening_food` | 3 | Diet requested and met |
| `quest_quickening_walk` | 3 | Gentle movement habit |
| `quest_quickening_worry` | 4 | A concern taken seriously |
| `quest_quickening_kit` | 4 | Kit assembled |
| `quest_quickening_room_warm` | 3 | Heat held overnight |
| `quest_quickening_bell` | 3 | Bell installed and tested |
| `quest_quickening_hands` | 5 | Attendants trained |
| `quest_quickening_after` | 3 | First hour watched |
| `quest_quickening_cots` | 3 | Cots built |
| `quest_quickening_rota` | 4 | Night rota written |
| `quest_quickening_wash` | 3 | Laundry routine running |
| `quest_quickening_quiet` | 3 | Quiet corner made |
| `quest_quickening_watch` | 4 | Night watch kept |
| `quest_quickening_play` | 3 | Play space safe |
| `quest_quickening_words` | 4 | Songs and naming |
| `quest_quickening_steps` | 3 | Walking practice |
| `quest_quickening_safety` | 4 | Childproofing done |
| `quest_quickening_chores` | 3 | First tasks bounded |
| `quest_quickening_siblings` | 4 | Sibling care arranged |
| `quest_quickening_sleep` | 3 | Sleep plan agreed |
| `quest_quickening_help` | 3 | Household help rota |
| `quest_quickening_counsel` | 4 | Support after a hard week |
| `quest_quickening_family` | 3 | Family record written |
| `quest_quickening_file` | 3 | Sealed file opened |
| `quest_quickening_names` | 3 | Naming recorded |
| `quest_quickening_handoff` | 4 | School-door handoff |
| `quest_quickening_review` | 3 | Care review held |
| `quest_quickening_archive` | 4 | First-year archive |

---

## 30. APPENDIX Q — NPC DOSSIERS (BRIEF)

**Esme Var** — midwife. Has delivered in worse places and says so rarely.
Believes a birth is a room, a schedule, and a calm voice. Trains everyone who
will listen.

**Maeve Orr** — expectant mother. Decides what happens in her room and asks
the shelter for the rest. Wants the nursery to be a place she can leave and
return to without guilt.

**Tam Ferro** — nurse. Keeps the clinic stocked, the records accurate, and the
kettle hot. Notices when a visit is missed before anyone else does.

**Ivy Dunn** — nursery keeper. Runs the rota like a watch and the cots like a
store. Believes a baby sleeps when the adults are calm.

**Bex** — grandmother. Knows the old ways and is the first to say when a
warm memory is not a medical plan. Sits with people and stays.

**Sim Okoye** — physician. Wants clean water, a referral route, and permission
to say when a risk is beyond the room.

**Ness** — registrar. Writes the first birth record in the settlement's
history, seals it, and explains the seal twice.

**Olem** — father. Repairs the cradle, heats the stones, and learns that
patience is a job.

---

## 31. APPENDIX R — LOCATION DETAIL

- **The Clinic Door** — where a visit starts, and where a person may turn back.
- **The Wash Line** — cloth, steam, and the quiet work of keeping clean.
- **The Sheltered Yard** — first air, first light, first winter walks.
- **The Kettle Corner** — waiting families and the best conversations.
- **The Quiet Garden** — green, bench, and a place to be still.
- **The Handcart Lane** — where supplies arrive and men learn to fetch.
- **The School Door** — the handoff, at last.
- **The Winter Walk** — cold-weather care and short trips.
- **The Memorial Wall** — where loss is kept, not hidden.
- **The North Gate Post** — arrivals who need care first and answers second.

---

## 32. APPENDIX S — DOSE AND MATERNITY CONTRACT

| Rule | Statement |
|---|---|
| Read only | Dose history comes from the live ledger |
| No ranking | No child is compared to another |
| No selection | The shelter never chooses who is born |
| Story over number | `moralityMemory` records what was told, not the dose |
| Care for all | Every person receives the same standard of care |
| Privacy | Dose history is sealed like every other record |
| Language | Clinical, plain, and never fatalistic |
| Support | Concerns route to clinicians, not to the player's judgement |

The contract exists because a settlement game that measured children by their
contamination would be a machine for cruelty. This expansion measures care
instead, and only care.

---

## 33. APPENDIX T — WORKED 360-DAY MATERNITY SCENARIO

**Days 1–30.** Confirmation; Esme writes the list; the room is chosen.

**Days 31–60.** Delivery room cleaned and warmed; water line planned; visits
begin; the first worry is heard and answered.

**Days 61–90.** Nursery corner cleared; cots ordered from the workshop; rota
drafted; Bex begins sitting with Maeve in the evenings.

**Days 91–120.** Birth plan written; attendance chosen; the kit is assembled
item by item; two items are missing and are made.

**Days 121–150.** Water reaches the clinic; records cabinet built; Ness opens
the first sealed file and explains the access log.

**Days 151–180.** Early snow closes the road; supplies are counted; the nursery
rota is tested on ordinary nights and adjusted twice.

**Days 181–200.** Onset in the cold hours; the room holds heat; the birth is
attended and elided; a child is registered; a lineage record is written.

**Days 201–230.** The first week: little sleep, steady care, a feeding
problem met with support rather than advice; recovery proceeds.

**Days 231–260.** Feeding stabilizes; the household help rota runs; 
sibling care is arranged; a mood concern is referred and supported.

**Days 261–300.** First milestones recorded; the nursery becomes routine; the
night watch and the nursery rota begin to share a shift culture.

**Days 301–330.** First winter behind the family; the clinic reviews its own
performance honestly and changes two procedures.

**Days 331–360.** The child stands; the first-year archive is closed and
sealed; the handoff plan to schooling is written for year five.

---

## 34. APPENDIX U — VIGNETTES (TONE SAMPLE)

> Esme writes the list on the back of a maintenance sheet, and it is a long
> list, and nobody argues with the kettle being first. The kettle is first
> because a warm room and clean water are the two things the shelter can
> actually promise.

> Bex sits with Maeve in the kettle corner and tells her about the winter she
> was born in, and the story is not a plan, and Maeve knows that, and the story
> helps anyway.

> Olem heats the stones three times, checks the bell twice, and then sits
> outside the door and learns that the useful thing to do is to stay where he
> is until someone needs him.

> In the records office, Ness writes the first line of the first file and then
> explains the seal, and Maeve listens to the whole explanation and nods, and
> the paper goes in the box, and the box is locked.

---

## 35. APPENDIX V — LOSS AND GRIEF PROTOCOL

| Step | Action | Owner |
|---|---|---|
| First | Care for the person | medical |
| Second | Stop all routine tasks | nursery |
| Third | Provide company, not instructions | Bex |
| Fourth | Record the fact privately | registrar |
| Fifth | Route to grief owners | relations |
| Sixth | Offer ritual, never require | 13 / 24 |
| Seventh | Keep the record sealed | records |
| Eighth | Return when they ask | all |

The protocol exists so that no programmer, designer, or player ever has to
improvise around a loss. The steps are quiet, the record is private, and the
shelter's job is to stay.

---

## 36. APPENDIX W — FAILURE MODES AND RECOVERY

| Failure | Effect | Recovery |
|---|---|---|
| Missed visit | worry unspoken | reschedule, listen |
| Cold room | discomfort, risk | heat, rebuild |
| Missing kit item | delay | borrow, make |
| No attendance plan | confusion | write plan |
| Feeding problem | stress | clinic support |
| Missed night feed | exhaustion | rota repair |
| Unsealed record | trust loss | seal, review |
| No handoff note | repetition | write before five |
| Overworked helper | burnout | rota shrank, thanks |
| Loss | grief | protocol §35 |

No failure is a punishment mechanic, and no failure is scored. The shelter
learns, repairs, and keeps caring.

---

## 37. APPENDIX X — CONTENT REVIEW CHECKLIST

- [ ] `ChildDevelopmentSystem` remains the child authority.
- [ ] `GenerationalLineageExtension` keeps lineage records.
- [ ] `CaregivingSystem` keeps care bonds.
- [ ] Dose history is read-only and never ranked.
- [ ] Birth is elided; nothing is explicit.
- [ ] No child is compared, scored, or selected.
- [ ] Family planning is private and unpressured.
- [ ] Loss routes to grief owners with no reward.
- [ ] Records are sealed by default with an access log.
- [ ] Save additions are additive and legacy-neutral.
- [ ] Determinism uses live seeded paths only.

---

## 38. APPENDIX Y — GLOSSARY

- **Antenatal** — care before a birth.
- **Elision** — representing birth by what surrounds it, not by the event.
- **Nursery** — the room and routine that keep an infant safe and fed.
- **Rota** — the written care schedule.
- **Band** — the dose story a cohort carries; never a rank.
- **Seal** — the privacy level of a record.
- **Handoff** — the point where early years hands a child to schooling.
- **Directive** — who is allowed to decide during care.
- **Formative event** — the live generational record of what shaped a person.

---

## 39. APPENDIX Z — INTEGRATION MATRIX

| System | Reads | Writes | Never writes |
|---|---|---|---|
| `ChildDevelopmentSystem` | stages | milestones | pregnancy |
| `AntenatalCareSystem` | needs, dose | visits, flags | child state |
| `BirthSystem` | care score | birth record, attendance | grief |
| `NurserySystem` | inventory | routines, rotas | development |
| `EarlyYearsSystem` | stages | activities | education score |
| `MaternalSupportSystem` | caregiving | recovery plan | mental health |
| `MaternityRecordsSystem` | medical log | seals, access log | clinical facts |
| `CaregivingSystem` | assignments | bonds | child state |
| `GenerationalLineageExtension` | records | lineage | child state |
| `SurvivorRelationsSystem` | bonds | relationships | grief policy |
| `MemorialSystem` | loss fact | memorial | birth record |
| `NeedsSystem` | fatigue | needs | schedules |
| `DoseLedgerSystem` | history | nothing | nothing |
| `EpilogueChronicleBuilder` | milestones | chronicle | child state |

---

## 40. APPENDIX AA — DATA SCHEMA DETAIL (NEW CATALOGS)

**`antenatal_care.json`** — `visit_id`, `display_name`, `week_min`, `week_max`,
`checks[]`, `staff[]`, `duration_hours`, `tags`.

**`pregnancy_visits.json`** — `visit_id`, `week_band`, `required_checks[]`,
`referral_triggers[]`, `tags`.

**`birth_plans.json`** — `plan_id`, `display_name`, `room`, `attendance[]`,
`decisions[]`, `preferences[]`, `backups[]`, `tags`.

**`birth_attendance.json`** — `role_id`, `display_name`, `training`,
`owns[]`, `present_when`, `backup`, `tags`.

**`nursery_schedules.json`** — `block_id`, `time`, `care[]`, `staff[]`,
`quiet_rule`, `tags`.

**`infant_feeding.json`** — `method_id`, `display_name`, `supplies[]`,
`support[]`, `notes`, `tags`.

**`early_years_activities.json`** — `activity_id`, `display_name`, `age_band`,
`learns`, `supervision`, `tags`.

**`child_milestones.json`** — `milestone_id`, `display_name`, `stage`,
`record`, `private`, `handoff_note`, `tags`.

**`maternal_support.json`** — `service_id`, `display_name`, `who`, `when`,
`route`, `tags`.

**`maternity_records.json`** — `field_id`, `display_name`, `seal`, `family`,
`clinical`, `archive`, `tags`.

All new catalogs carry `schema_version: 1` and fail the integrity gate on missing
or duplicate IDs, invalid references, or out-of-range numbers.

---

## 41. APPENDIX AB — MEASUREMENT AND TELEMETRY

| Metric | Purpose | Source |
|---|---|---|
| Visits kept | care coverage | Antenatal |
| Kit completeness | readiness | Birth |
| Attendance readiness | safety | Birth |
| Nursery coverage | night care | Nursery |
| Feeding stability | support quality | Nursery |
| Recovery rest hours | maternal health | Support |
| Milestones recorded | memory | EarlyYears |
| Handoff notes | continuity | EarlyYears |
| Sealed access count | privacy | Records |
| Loss follow-ups | care quality | Grief owners |

Telemetry is diagnostic only; it never gates content and never becomes a score
against a person.

---

## 42. APPENDIX AC — IMPLEMENTATION CHECKLIST

- [ ] Phase 0 premise re-audit recorded with file:line evidence.
- [ ] Phase 1 catalogs authored and registered with validators and scanner.
- [ ] Phase 2 Core systems are pure, engine-free, and deterministic.
- [ ] Phase 3 save additions are additive, legacy-neutral, and round-trip tested.
- [ ] Child, lineage, and care authorities remain untouched.
- [ ] Triad parity (Setup / Save / Flush) holds for every new state.
- [ ] Phase 4 host session exposes live state and real commands only.
- [ ] Phase 5 panels pass bind/unbind/rebind, focus, contrast, and scaling checks.
- [ ] Phase 6 content passes the review checklist in §37.
- [ ] Phase 7 soak shows a full pregnancy, birth, and first year.
- [ ] Phase 8 data integrity and content utilization selftests pass.
- [ ] No dose ranking, no selection, no explicit content exists.

---

## 43. APPENDIX AD — OPEN QUESTIONS FOR REVIEW

1. Should pregnancy be visible to the player from the start or only when the
   person shares it?
2. Should the nursery be a shared room or a set of family corners?
3. How much should preparation shift birth outcomes without guaranteeing them?
4. Is the handoff at five, six, or readiness-based by default?
5. Should the shelter offer a hall on the Memorial Wall for a loss, and who
   keeps it?
6. Are siblings given formal care rotations, and how are they thanked?
7. Should feeding methods have any mechanical advantage at all, or only supply
   consequences?
8. Does the clinic ever treat arrivals from outside, and who decides?

None of these may be decided unilaterally; each changes tone and balance.

---

## 44. APPENDIX AE — CROSS-WAVE HOOKS

| Wave | Expansion | Hook |
|---|---|---|
| 1 | 12 The Second Generation | The school-door handoff |
| 1 | 13 The Faithful | Naming and quiet rites |
| 1 | 15 The Deep Root | Milk, soft food, garden calm |
| 1 | 16 The Rebuilt Body | Assistive devices for a hard birth |
| 2 | 17 The Long Evening | Cradle songs and evening quiet |
| 2 | 18 The Underneath | Sealed clinic rooms |
| 2 | 19 The Bitter Air | Masked clinic procedures |
| 2 | 21 The Grid | Warmth and light for the nursery |
| 3 | 22 The Clean Flow | Clean water and hand hygiene |
| 3 | 23 The Alarm | Call bell and evacuation carry |
| 3 | 24 The Long Goodbye | Grief and legacy |
| 3 | 26 The Common Table | Feeding and recovery diets |
| 4 | 27 The Thread | Cloth, wraps, and warm bedding |
| 4 | 28 The Lesson | The handoff curriculum |
| 4 | 29 The Glass | Scales, lamps, and windows |
| 4 | 30 The Press | Records, files, and seals |
| 4 | 31 The Kiln | Warm stones and building the room |
| 5 | 32 The Wild | Milk, quiet, and gentle animals |
| 5 | 33 The Weather | Winter preparation |
| 5 | 34 The Long Road | Supplies before closure |
| 5 | 35 The Habit | Care without shame |
| 5 | 36 The Watch | Nursery rota and night cover |

Each hook is additive. The Quickening can ship alone, and every other expansion
can ship without it.

---

## 45. APPENDIX AF — ENDING PROSE SKETCHES

**The Warm Room.** The delivery room holds its heat through the coldest night of
the year, and the shelter learns that this is what preparedness feels like.

**The House Full.** The nursery is a shared room with three cots and a kettle,
and children grow up knowing every adult's voice.

**The Quiet Handoff.** At five years old, the first child of the settlement walks
to the school door holding a hand, and the record follows, sealed and complete.

**The Long Winter.** A hard season closes the road and tests the rota, and the
clinic comes out of it with two changed procedures and no complaints.

**The Empty Cot.** The cot is folded and the wall keeps the name, and the
shelter does not turn a loss into a lesson or a score.

**Fade.** A cradle by a heater, a kettle on, and someone awake, because someone
is always awake.

---

## 46. APPENDIX AG — COMMON FAILURE PATTERNS TO AVOID

| Pattern | Why it is bad | Correct approach |
|---|---|---|
| Birth as minigame | spectacle | elision and aftermath |
| Score per child | eugenics | care measured, never people |
| Hidden infant meter | dishonesty | live needs only |
| Moralized feeding | judgement | support the method |
| Public medical files | privacy harm | seals and access logs |
| Loss as failure | cruelty | protocol and company |
| Baby item treadmill | padding | a short, real list |
| Countdown pressure | anxiety | readable schedules |
| Child labor | exploitation | bounded live capacity |
| Handoff cliff | discontinuity | written private note |

The list exists because birth content is easy to turn into either a spectacle or
a scoreboard. The expansion's rule is that the smallest patient teaches care,
and care is measured by whether it was given.

---

## 47. APPENDIX AH — CONTENT VOLUME SUMMARY

| Category | Rows | Prose estimate |
|---|---|---|
| Antenatal visits | 12 | 3,000 |
| Visit schedule | 12 | 2,500 |
| Birth plans | 10 | 2,500 |
| Attendance roles | 8 | 2,000 |
| Nursery routines | 10 | 2,500 |
| Feeding methods | 8 | 2,000 |
| Early-years activities | 24 | 4,000 |
| Milestones | 30 | 4,500 |
| Support services | 10 | 2,500 |
| Record rules | 12 | 2,500 |
| Quests | 53 | 15,000 |
| NPCs | 8 | 7,000 |
| Locations | 10 | 3,500 |
| Items | 16 | 2,500 |
| Endings | 6 | 3,000 |
| **Total** | | **~59,000** |

---

## 48. APPENDIX AI — FIRST YEAR OF THE CLINIC

| Month | Focus | Milestone |
|---|---|---|
| 1 | News and list | room chosen |
| 2 | Delivery room | heated and clean |
| 3 | Visits | schedule running |
| 4 | Nursery corner | cots installed |
| 5 | Plan | attendance chosen |
| 6 | Water | clinic line live |
| 7 | Kit | assembled |
| 8 | Winter prep | stores counted |
| 9 | Birth | first record written |
| 10 | Recovery | rota tested |
| 11 | Feeding | support stable |
| 12 | Handoff plan | year-five note filed |

A year of the clinic is a year of small preparations that all matter on one
night, and the shelter ends it knowing how to do the next one better.

---

## 49. APPENDIX AJ — CONSENT AND DIGNITY CONTRACT

| Clause | Promise |
|---|---|
| Voice | The person giving birth decides |
| Privacy | Records sealed by default |
| Dignity | No explicit content, no spectacle |
| Equality | Every child receives the same care |
| Loss | Company, never a mechanic |
| Choice | Family planning is private and unpressured |
| Companionship | Support is offered, never forced |
| Rest | Recovery is time, protected |
| Language | Plain, warm, and never fatalistic |
| Memory | The story is kept, kindly and privately |

The contract is the expansion's first-class design object. Everything else in
the plan is implementation; this table is the reason it exists.

---

## 50. APPENDIX AK — NIGHT ROTA TABLE (7 NIGHTS)

| Night | Keeper | Relief | Feed blocks | Notes |
|---|---|---|---|---|
| First | Ivy | Tam | 3 | quiet strict |
| Second | helper | ivy | 3 | wash night |
| Third | Tam | keeper | 3 | linen change |
| Fourth | Ivy | helper | 3 | records update |
| Fifth | helper | Tam | 3 | rest review |
| Sixth | Tam | Ivy | 3 | supply count |
| Seventh | rotation | all | 3 | weekly review |

Seven nights is the minimum honest rota: long enough to show fatigue, short
enough to test fairly. The relief column exists because a rota with no relief
is not a rota; it is a hope.

---

## 51. APPENDIX AL — SUPPLY CHAIN TABLE

| Supply | Weekly use | Source wave | Runs out |
|---|---|---|---|
| Clean water | high | 22 | never, if maintained |
| Cloth | med | 27 | wash cycle |
| Soap | low | 39 | monthly |
| Fuel | med | 31 | weekly |
| Lamps | low | 29 | yearly |
| Food | high | 26 | never, if rationed |
| Medicine | low | 30, 38 | variable |
| Records paper | low | 30 | seasonal |
| Warm stones | med | 31 | per use |
| Bedding | med | 27 | wash cycle |

The clinic is a consumer of every other wave, and the table makes the
dependency visible. A shelter that ignores its supply chain learns about it on
the coldest night of the year.

---

## 52. APPENDIX AM — SIBLING AND HOUSEHOLD CARE TABLE

| Task | Who | When | Bounded |
|---|---|---|---|
| Hold a hand | sibling | as asked | no lifting |
| Fetch water | sibling | short runs | adult nearby |
| Sit nearby | sibling | quiet hours | adult nearby |
| Read aloud | any | evening | optional |
| Serve food | sibling | meals | adult nearby |
| Clean toys | sibling | daily | small only |
| Night call | sibling | none | never |
| Door answer | sibling | none | never |

The table is short on purpose. Siblings are family, not staff, and the
bounded column is the contract that keeps the expansion from quietly inventing
child labor.

---

## 53. APPENDIX AN — REVIEW CADENCE TABLE

| Review | Frequency | Question | Owner |
|---|---|---|---|
| Visit check | weekly | any missed care? | midwife |
| Rota check | weekly | anyone exhausted? | keeper |
| Supply count | weekly | what runs out first? | nurse |
| Record audit | monthly | any seal opened wrongly? | registrar |
| Care review | monthly | what would we change? | clinic |
| Family check | monthly | is support landing? | steward |
| Handoff review | yearly | is the note complete? | school |
| Loss review | as needed | was everyone cared for? | clinic |

Reviews are how a practice becomes an institution without becoming a
bureaucracy. The loss review is last because it is the one that matters most and
the one nobody looks forward to.

---

## 54. APPENDIX AO — HANDOFF READINESS TABLE

| Readiness | Age default | Evidence | Owner |
|---|---|---|---|
| Speaks in sentences | 3–4 | milestone record | family |
| Follows simple orders | 4 | keeper note | nursery |
| Toilet independence | 3–5 | routine record | family |
| Separates without distress | 4–5 | observed | family |
| Names safe adults | 4–5 | activity note | nursery |
| Knows the paths | 4–5 | walks record | family |
| Listens in a group | 5 | story circle | early years |
| Follows a routine | 5 | rota experience | nursery |
| Handles a cup | 3–4 | feeding record | nursery |
| Asks for help | any | observed | all |

The handoff is readiness-based with a default age, which means a child walks to
the school door when the record says they are ready and the family agrees. The
table protects both the child and the school from an arbitrary date.

---

## 55. APPENDIX AP — WAITING FAMILY TABLE

| Family | Support offered | Quiet hours | Records access |
|---|---|---|---|
| Expectant parents | visits, kettle, rest | yes | by consent |
| New parents | meals, laundry, night | yes | by consent |
| Siblings | play, school, company | yes | no |
| Grandparents | sitting, stories, help | yes | by consent |
| Adoptive parents | briefing, support | yes | by consent |
| Single caregivers | relief hours | yes | by consent |
| Bereaved family | company, rite, time | yes | yes |
| Arrivals with children | clinic first, forms later | yes | by consent |

The waiting family table is the expansion's recognition that a clinic treats a
household, not an individual. Everyone waiting gets a chair, a kettle, and the
truth at the pace they can hear it.

---

## 56. CLOSING STATEMENT

ASHFALL already tracks children, lineage, care bonds, and relationships, and it
already owns the ledger that records what a body has been through. What it lacks
is the beginning of the arc: the months before a birth, the night of it, and the
years when a shelter teaches a person to be safe. The Quickening adds that
chapter without adding a second child system, a score, or a spectacle. It adds a
clinic, a warm room, a night rota, sealed files, a handoff, and the quiet truth
that the smallest patient teaches a shelter the most.

> Wave 6 note: this plan is one of five Wave 6 expansion bibles (37–41). Each is
> self-contained; none requires another to ship. The shared Wave 6 index lives at
> `docs/expansions/wave6/WAVE6_INDEX.md`. The safe pre-signature step is Phase 1
> (data schemas and validators), which is additive and reversible. Evidence
> anchors: `ChildDevelopmentSystem` (`ChildProfile`, `DevelopmentStage`,
> `ResolveStage`, `RegisterChild`, `TickDay`, `RecordEducation`,
> `AssignCaregiver`, `GetChoreWorkCapacity`), `CohortSystem` (`CohortChild` with
> `guessBand`/`trueBand` and `moralityMemory`), `GenerationalLineageExtension`
> (`LineageRecord`, `FamilyUnit`), `CaregivingSystem` (`CaregivingAssignmentState`
> and its bond constants), `SurvivorRelationsSystem` (`RelationshipEntry`), and
> `GenerationalSystem` (`DevelopmentPhase`, `EducationLevel`, `FormativeEvent`).