# ASHFALL — Expansion 12 Design Bible
# THE SECOND GENERATION
### Wave 1 · Generational Survival, Schooling, Kinship, and Succession

**Document status:** Design plan (pre-integration). Not a claim. Not an authorization.
**Date:** 2026-09-20
**Domain owners touched:** `Ashfall.Core.Survivors`, `Ashfall.Core.Cohort`, `Ashfall.Core.Legacy`, `Ashfall.Core.Apprenticeship`
**Proposed host owner:** `GenerationalHostSession` (extends the existing `GenerationalSaveStore`)
**Existing save section:** `child_development` + cohort state inside the dose/generational envelope
**Proposed CLI verbs:** `--generational-selftest`, `--cohort-lifecycle-selftest` (both already exist; extended, not replaced)
**Rule compliance:** Godot authoritative; Core engine-free; JSON data authoritative; one authority per concern; deterministic seeded RNG only.

---

## 0. HOW TO READ THIS DOCUMENT

This is a **design bible**, not an integration plan. It describes what *should*
exist, why the repository evidence supports it, and how it can attach to live
owners without creating a parallel system. Every implementation phase must later
pass through `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `TEST_POLICY.md`
before any code is written.

Three conventions are used:

- **`LIVE`** — confirmed by reading current source or data during the evidence pass.
- **`GAP`** — confirmed absent or thin relative to its live owner.
- **`PROPOSED`** — new content or contract that does not yet exist and needs a foreman signature.

The expansion deliberately does **not** invent a second needs model, a second
inventory, a second save store, or a second RNG. It extends the exact owners that
already exist. That constraint is the reason this expansion is shippable at all.

---

## 1. EXECUTIVE SUMMARY

### 1.1 Pitch

The shelter survived the first generation. Now it has to raise the next one.

ASHFALL already simulates children: `CohortSystem` books a child with a guessed
radiation baseline, `ChildDevelopmentSystem` tracks infant→young-adult stage,
`GenerationalSystem` runs formative events and education XP, and
`GenerationalSuccessionEngine` can pass a dead survivor's role to an heir. But the
authored content is almost entirely absent. There are **seven** development traits
in `development_traits.json`, **four** tuning numbers in `cohort_tuning.json`, and
almost no quests, locations, or characters that treat a child as anything but a
future adult stat block.

**The Second Generation** turns the cohort layer from a background state container
into the emotional and logistical spine of a late campaign. It adds a schoolhouse,
a nursery, a rite-of-passage circuit, a kinship graph with real obligations, and a
succession dossier that decides who inherits a dead founder's tools, debts, and
grievances. It asks the only question a survival game can ask after it has asked
"will we live": **what are we handing to the people who inherit this?**

The expansion is built on a single hard constraint: **children consume resources
and produce meaning before they produce labor.** A child is a liability for
in-game months. That is the point. The expansion is a long-horizon bet, not a
power fantasy.

### 1.2 The core loop it adds

```
Birth / intake → Nursery care → Schoolhouse curriculum → Formative event
        │                                                        │
        ▼                                                        ▼
   Ration pressure  ───────────────────────────────►  Development trait
        │                                                        │
        ▼                                                        ▼
   Caregiver fatigue ────► Trauma load ────► Apprenticeship ────► Rite of passage
                                                                        │
                                                                        ▼
                                                          Adult survivor with dossier
                                                                        │
                                                                        ▼
                                                    Inheritance / succession / grievance
```

### 1.3 What the player actually manages

1. **Rations.** `cohort_tuning.json` already sets `child_ration_fraction: 0.5`.
   The expansion makes that number negotiable: a hungry child gets half rations,
   but sustained half-rations ratchet `nutritionScore` down and produce the
   `development_trait_malnourished_growth` outcome that already exists in data.
2. **Caregiver time.** A caregiver assigned to the nursery cannot work the
   foundry. This is a real labor opportunity cost, not a debuff.
3. **Curriculum.** Which of survival, craft, letters, medicine, or arms a child
   learns changes their adult skill floor and their available apprenticeships.
4. **Safety.** Formative events are authored; the shelter chooses whether a child
   witnesses an execution, a burial, a birth, a radio discovery, or a strike.
5. **Succession.** When a founder dies, `GenerationalSuccessionEngine` can name
   an heir. The expansion gives that heir a dossier of obligations: debts to pay,
   tools to maintain, promises to keep, and enemies inherited.

### 1.4 What it explicitly is **not**

- Not a romance or pairing system. Parentage is authored/assigned through intake
  and cohort booking; the expansion does not add a courtship minigame.
- Not a population-growth number-go-up system. Child mortality stays real
  (`CohortChild.isDeceased`, `deathCause`) and grief integrates with the existing
  `MemorialSystem` and `GuiltInsomniaSystem`.
- Not a second survivor model. Children are the existing survivor entity plus the
  existing cohort/development components.
- Not a real-world religion or a real-world family structure. All content is
  fictional and setting-specific, per the tone rules in `AGENTS.md`.

---

## 2. EVIDENCE BASE AND GAP ANALYSIS

### 2.1 Live systems (verified during this pass)

| File | Role | Status |
|---|---|---|
| `Assets/Ashfall.Core/CohortSystem.cs` | Books children, guessed dose band, mortality, maturation flag | `LIVE` |
| `Assets/Ashfall.Core/CohortTuning.cs` | Loads `cohort_tuning.json` | `LIVE` |
| `Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` | Stage, education score, chore efficiency, caregiver, milestones | `LIVE` |
| `Assets/Ashfall.Core/Survivors/GenerationalSystem.cs` | Phases, schoolhouse education, trauma, stunting, one-time adult transition | `LIVE` |
| `Assets/Ashfall.Core/GenerationalLineageExtension.cs` | Lineage link between generations | `LIVE` |
| `Assets/Ashfall.Core/Legacy/GenerationalSuccessionEngine.cs` | Role/tool succession on death | `LIVE` |
| `Assets/Ashfall.Core/ApprenticeshipSystem.cs` | Master/apprentice training under `cohort_tuning.max_active_apprentices` | `LIVE` |
| `Assets/Ashfall.Core/Survivors/StartingCohortCatalog.cs` | Loads starting cohort profiles | `LIVE` |
| `src/Host/GenerationalSaveStore.cs` | Persistence for generational state | `LIVE` |
| `src/Host/ChildDevelopmentSaveStore.cs` | Persistence for child development | `LIVE` |

### 2.2 Live data (counted, not estimated)

| Catalog | Entries | Notes |
|---|---|---|
| `Assets/StreamingAssets/Data/cohort_tuning.json` | **4 fields** | `child_ration_fraction`, `schooling_age_days`, `maturation_age_days`, `max_active_apprentices` |
| `Assets/StreamingAssets/Data/development_traits.json` | **7 traits** | resilient, field_medic_instinct, careful_scavenger, mechanically_minded, hypervigilant, malnourished_growth, authority_averse |
| `Assets/StreamingAssets/Data/starting_survivor_cohorts.json` | profiles | starting rosters only |
| `Assets/StreamingAssets/Data/starting_survivors.json` | flat list | legacy starting survivor data |

### 2.3 Confirmed gaps

- **GAP-12-1 — Trait scarcity.** Seven development traits cannot express the
  space the systems model. `GenerationalSystem` has five development phases and
  four education levels; seven traits cover a fraction of the outcomes.
- **GAP-12-2 — No curriculum catalog.** `educationFocusId` defaults to
  `"practical_survival"` but there is no authored curriculum list, no per-focus
  XP curve, and no adult-transition mapping in data.
- **GAP-12-3 — No milestone catalog.** `ChildDevelopmentSystem.Milestones` is a
  free string list; no authored milestones, ages, or effects exist.
- **GAP-12-4 — No kinship content.** `CohortChild.parentIds` exists but there is
  no relation graph beyond parentage, no sibling/cousin/guardian semantics, and no
  obligation model.
- **GAP-12-5 — No schoolhouse location or quests.** No `loc_` node, no quest, and
  no NPC in the corpus is dedicated to education.
- **GAP-12-6 — Succession is mechanical only.** `GenerationalSuccessionEngine`
  moves a role; there is no authored dossier, no inherited grievance, no debt.
- **GAP-12-7 — No child-facing items.** There are no primers, slates, toys,
  heirloom tools, or coming-of-age tokens in `items.json`.
- **GAP-12-8 — No rite-of-passage stream.** The maturation flag fires but nothing
  marks it narratively.

### 2.4 Why the gap exists

The cohort layer was built to answer a *simulation* question — "what happens to
the dose baseline across generations?" — and it answers that question well. It was
never given a *content* budget. That is the honest gap this expansion fills. It
does not need new architecture; it needs authored data and a small number of new
pure systems that consume the existing components.

### 2.5 Non-duplication statement

This expansion will **not** add:
- a second needs, morale, or health model (uses `NeedsSystem`, `MoraleContagionSystem`);
- a second inventory or item authority (uses `Inventory` and `items.json`);
- a second save store (extends `GenerationalSaveStore` / `ChildDevelopmentSaveStore`);
- a second RNG (uses the host-forked `ISeededRng` via `CampaignRngStream`);
- a second memorial or grief path (uses `MemorialSystem`, `GuiltInsomniaSystem`);
- a second skill system (uses `SkillProgressionSystem` and its catalog).

---

## 3. DESIGN PILLARS AND TONE

### 3.1 Pillars

**Pillar 1 — The long bet.** Every child is a months-long resource sink before
they are a contributor. The expansion must never let a player "solve" a child into
an immediate worker. This is enforced by `maturation_age_days: 180` and by keeping
`isMatured` one-way.

**Pillar 2 — Meaning under scarcity.** The interesting decisions are not "do I
feed the child" but "which child gets the last dose of vitamins," "does the twelve-
year-old work the sorting line instead of attending letters," and "whose name do
we put on the plaque when a child dies of something we could have prevented."

**Pillar 3 — Inheritance is debt.** A dead founder passes tools and a role, but
also obligations. The dossier should make the player *feel* the second generation
inheriting a world they did not make.

**Pillar 4 — No child soldier fantasy.** Children can defend, but the expansion
does not celebrate it. Combat exposure routes through `CombatTraumaSystem` and
`GuiltInsomniaSystem` and produces lasting trauma traits, never a power spike.

**Pillar 5 — Restrained, human, fictional.** No real countries, wars, people, or
faiths. Names are invented. Prose is quiet. A birth is a small, frightening event,
not a stat popup.

### 3.2 Tone calibration

| Element | Do | Do not |
|---|---|---|
| A birth | A midwife's lamp, a held breath, a name chosen carefully | Glowing celebration, confetti |
| A death | A shelf item kept, a name carved, a silence | Loot popup, revenge quest |
| Coming of age | A practical test the shelter agrees on | Supernatural chosen-one beat |
| A mentor | A tired craftsperson with an apprentice and a defect rate | Wise sage |
| A school | Recitation, slates, a lesson interrupted by a blackout | Idyllic academy |

### 3.3 Content limits (hard)

- No real-world religious practice, symbol, or scripture.
- No sexual content, no minors in romantic content, no birth illustrated.
- No real historical atrocity used as color.
- Child combat is framed as failure and last resort, never reward.

---

## 4. NEW WORLD REGIONS AND LOCATIONS

All new locations are added to `Assets/StreamingAssets/Data/locations.json`
(canonical location authority) with the schema already used there: `id`,
`displayName`, `description`, `dangerLevel`, `travelHours`, `baseRadsPerHour`,
`region`, `inspect`, `overlay_on_unlock` where applicable. New interior rooms go
to `shelter_rooms.json` using the existing room schema.

### 4.1 The Crèche — interior shelter room

- **`room_creche`** — a converted storage bay lined with three cribs made from
  crate slats. Adds nursery capacity. Consumes power for a heater; warmth is
  safety, and safety gates formative trauma.
- **`room_schoolhouse`** — a former briefing room with slates, a ration-board, and
  a stove. Enables curriculum assignment and teacher roles.
- **`room_workshop_apprentice_bay`** — a corner of the foundry reserved for
  apprentice work; couples to `ApprenticeshipSystem`.
- **`room_infirmary_pediatric`** — an annex to the existing medical ward. Child
  illness routes through the live `DiseaseSystem`, never a parallel one.
- **`room_memorial_niche`** — a wall of carved names; couples to `MemorialSystem`.

### 4.2 Exterior locations

| ID | Name | Danger | Purpose |
|---|---|---|---|
| `loc_ash_orphanage` | The Silt Orphanage | 5 | Ruined pre-war children's home; scavenge primers, toys, records |
| `loc_buried_school` | The Buried School | 6 | Collapsed primary school under ash; chalk slates, gym hoist, shelter |
| `loc_playground_markers` | The Painted Yard | 3 | Faded playground; a cache of toys and a grief encounter |
| `loc_midwife_cabin` | The Far Midwife's Cabin | 4 | Isolated cabin of a wasteland midwife; trade medicine and knowledge |
| `loc_lineage_vault` | The Lineage Vault | 7 | Municipal records on family units; enables dossier reconstruction |
| `loc_rites_ring` | The Cinder Ring | 6 | A ring of stones where gangs hold coming-of-age trials |
| `loc_cousin_farm` | Kinstead | 5 | A farmstead held by an extended family; diplomatic kinship |
| `loc_letter_archive` | The Undelivered Office | 6 | A dead postal sorting depot; letters from parents who never returned |

All entries must pass `CatalogIntegrityValidator` with valid referencing of
`items.json` IDs and be registered in `ContentUtilizationScanner` so they are not
dead data.

### 4.3 Region framing

The Crèche is not a separate map. It is a room cluster inside the shelter, consistent
with `docs/shelter/` and the existing expanded-shelter save. The exterior nodes
attach to existing region strings already present in `locations.json`
(`region` field) so map and travel systems need no new schema.

---

## 5. MAIN STORYLINE — "THE LEDGER OF NAMES"

### 5.1 Central conflict

A child born in the shelter is old enough to ask who her mother was. The answer
was never fully written down. The shelter kept a census, a dose ledger, and a
ration board — but no names, because names were considered a luxury. The player
discovers that the first generation deliberately chose not to record parentage,
because knowing the true dose bands of parents would have meant knowing which
children were doomed.

The expansion's central conflict is therefore **epistemic and moral**: reconstruct
the ledger of names, decide who has a right to know their parentage and dose band,
and decide whether the second generation should inherit the truths or the comfort
of the first.

### 5.2 Theme (unspoken)

**You cannot protect the next generation by withholding their inheritance. You can
only choose what shape the inheritance takes.**

### 5.3 Principal NPCs

| ID | Name | Role | Function |
|---|---|---|---|
| `npc_midwife_orsa_vell` | Orsa Vell | Midwife, elder | Guilt about a birth she recorded wrongly |
| `npc_teacher_bran_ike` | Bran Ike | Schoolteacher, ex-clerk | Believes literacy is a survival skill |
| `npc_child_petra_nine` | Petra "Nine" | Eleven-year-old, cohort tracker | Central child character; her true band is the mystery |
| `npc_guardian_halder_rusk` | Halder Rusk | Caregiver, widower | Wants to protect children from the truth |
| `npc_archivist_meret_low` | Meret Low | Records clerk | Holds the key to the name ledger |
| `npc_heir_adam_vell` | Adam Vell | Coming-of-age heir | Succession candidate; inherited debt |
| `npc_rival_kess_idre` | Kess Idre | Gang youth leader | Represents the "children raised wild" path |
| `npc_grandmother_sia` | "Sia" | Elder with dementia | Remembers a name nobody recorded; unreliable narrator |

Each NPC lives in `characters.json` (canonical character authority) plus, where
they are recruitable, in `survivors.json`. They are not duplicated into a new file.

### 5.4 Story beats (12)

1. **The Question.** Petra asks who her mother was. The ledger has a number, not a name.
2. **The Nursery Ledger.** The player inspects the old census ledger; parentage is blank.
3. **Orsa's Confession.** The midwife admits she was ordered to omit parent links.
4. **The Silt Orphanage.** Expedition recovers a box of intake cards.
5. **The First Name.** Reconstruct one parent link; the child reacts according to curriculum and trauma.
6. **The Truth Meeting.** The shelter argues whether to tell the children their bands.
7. **The Dissent.** Halder leaves if the truth is released too harshly; a rival faction forms.
8. **The Buried School.** The schoolhouse slate gives the curriculum a historical anchor.
9. **Kess's Challenge.** The wild children demand recognition and rations.
10. **The Coming-of-Age.** Petra (or the cohort's eldest) faces the rite the shelter designs.
11. **The Succession.** A founder dies (scripted if none die naturally); the dossier opens.
12. **The Second Ledger.** The player chooses the form of the new ledger: full truth, sealed comfort, or communal memory only.

### 5.5 Branching choices (6 major)

| Choice | Options | Consequence axis |
|---|---|---|
| Who learns their band | All / adults only / nobody / only the children | Trust, cohesion, rebellion |
| Daily ration allocation | Equal / worker-weighted / child-first | Nutrition vs. labor vs. resentment |
| Curriculum priority | Letters / craft / medicine / arms / survival | Adult skill floor, faction reactions |
| Fate of Kess's group | Admit / exclude / sponsor elsewhere | Population, safety, guilt |
| Succession rule | Merit / blood / election / needs of the role | Stability, factions |
| Final ledger form | Truth / sealed / oral | Endgame epilogue variants |

### 5.6 Endings (4 + fade)

1. **The Open Ledger** — full truth; higher early friction, higher long-term cohesion.
2. **The Sealed Book** — comfort; short-term stability, eventual discovery event.
3. **The Spoken Names** — oral tradition; no records, strong culture, vulnerable to loss.
4. **The Broken Line** — truth released cruelly; a faction splits and takes children.
5. **Fade** — the player refuses the decision; the ledger is left for the next generation unresolved.

Endings integrate with `epilogue_chronicle.json` and the existing ending matrix.
No new ending authority is created; new ending rows are appended to the canonical
endgame data.

---

## 6. QUEST DESIGN

Quest data follows the live schema in `year_of_ash_quests.json` / `questline_master.json`
(`id`, `title`, `faction`, `minDay`, `stages[]` with `stageIndex`, `objective`,
`requiredItemId`). New IDs use the prefix `quest_gen_` so they can be validated
and audited together.

### 6.1 Main questline — The Ledger of Names (12)

| ID | Title | Stage focus |
|---|---|---|
| `quest_gen_the_question` | The Question | Prompt; ledger inspection |
| `quest_gen_blank_columns` | The Blank Columns | Discover omission; confront archivist |
| `quest_gen_orsa_confession` | What Orsa Signed | Midwife testimony |
| `quest_gen_silt_intake` | Intake Cards | Expedition to `loc_ash_orphanage` |
| `quest_gen_first_name` | A Name Restored | Reconstruct one parent link |
| `quest_gen_the_meeting` | The Meeting | Shelter argument; choice lock |
| `quest_gen_holders_leave` | The Ones Who Leave | Rivalry consequence |
| `quest_gen_buried_school` | Lesson Under Ash | `loc_buried_school` expedition |
| `quest_gen_kess_challenge` | The Wild Ones | Gulf children negotiation |
| `quest_gen_first_rite` | The First Rite | Rite design and trial |
| `quest_gen_the_dossier` | The Dossier | Succession; inherited debt |
| `quest_gen_second_ledger` | The Second Ledger | Final form choice |

### 6.2 Side quests (24)

**Schoolhouse (5)**
- `quest_gen_slate_shortage` — craft slates from `item_scrap_slate`
- `quest_gen_banned_book` — a recovered manual the teacher refuses to teach
- `quest_gen_blackout_lesson` — teach through a power outage
- `quest_gen_field_trip` — a supervised short scavenge
- `quest_gen_exam_ration` — the school barter for exam-day extra rations

**Nursery and care (5)**
- `quest_gen_crib_safety` — reinforce the creche
- `quest_gen_milk_distribution` — allocation dispute
- `quest_gen_child_fever` — pediatric illness through `DiseaseSystem`
- `quest_gen_lullaby` — record a song for a sleepless child
- `quest_gen_heirloom_blanket` — a dead parent's blanket recovered

**Kinship (4)**
- `quest_gen_sibling_rival` — two siblings, one ration
- `quest_gen_guardian_oath` — adopt an orphan formally
- `quest_gen_grandmothers_name` — test Sia's memory against records
- `quest_gen_inherited_feud` — two families, an old grievance

**Coming of age (4)**
- `quest_gen_the_cinder_ring` — range trial
- `quest_gen_the_ledger_test` — literacy trial
- `quest_gen_the_tool_trial` — craft trial
- `quest_gen_the_choice_not_made` — a child refuses the rite

**Wild children (3)**
- `quest_gen_rust_markers` — find Kess's camp
- `quest_gen_stolen_rations` — theft investigation
- `quest_gen_the_truce_meal` — negotiate

**Succession (3)**
- `quest_gen_tools_of_the_dead` — distribute a founder's tools
- `quest_gen_debt_acknowledged` — inherit a debt to a faction
- `quest_gen_name_on_the_wall` — memorial inscription

### 6.3 Repeatable quests (5)

`quest_gen_repeat_lesson` (teach a class), `quest_gen_repeat_nursery` (a care
shift), `quest_gen_repeat_rite_observe` (attend a rite), `quest_gen_repeat_ledger`
(record a death), `quest_gen_repeat_kinship_meal` (a shared meal).

### 6.4 Dynamic / emergent hooks

The existing `DynamicQuestGenerator` can be extended with generational triggers
without a new generator. Triggers read live state: child stage change, caregiver
death, education threshold, trauma threshold, cohort graduation. Each emits a
typed fact; the host adapter decides presentation.

### 6.5 Quest design constraints

- Every quest must reference only IDs present in canonical catalogs.
- No quest may grant a matured adult child without the `maturation_age_days` gate.
- Child-in-danger quests must route consequences through `CombatTraumaSystem`
  and `GuiltInsomniaSystem`.
- No quest may create a parallel population counter; children are roster entries.

---

## 7. NEW GAMEPLAY SYSTEMS

All systems are pure Core, engine-free, deterministic, and attach to existing
owners. Each proposed system consumes existing state and emits facts; the host
applies presentation and persistence.

### 7.1 `CurriculumSystem` (new, `Ashfall.Core.Survivors`)

**Owns:** which curriculum a child studies (`letters`, `craft`, `medicine`,
`arms`, `survival`), daily education XP, and the adult-transition skill floor.

**Consumes:** `ChildDevelopmentSystem.EducationScore`, `GenerationalSystem`
education XP, `SkillProgressionSystem`.

**Does not own:** skill levels (that is `SkillProgressionSystem`), morale, needs.

**Data:** `education_curricula.json` (new).

**API sketch (pure, deterministic):**

```csharp
public sealed class CurriculumSystem
{
    public void AssignFocus(string childId, string focusId);
    public CurriculumTickResult Tick(int day, IReadOnlyList<string> enrolledChildIds);
    public string? MapToApprenticeshipTrack(string focusId, float educationScore);
}
```

Bounded rules: XP is integer-permille; no curriculum exceeds the skill cap;
education cannot substitute for an apprenticeship gate.

### 7.2 `DevelopmentalMilestoneSystem` (new, extends `ChildDevelopmentSystem`)

**Owns:** authored milestone definitions, age windows, and effect application.

**Consumes:** child stage, nutrition, safety, trauma. **Data:** `childhood_milestones.json`.

Milestones are additive to the existing `Milestones` string list; the system loads
definitions from JSON and records fired milestone IDs. It never rewrites stage.

### 7.3 `FamilyKinshipSystem` (new, `Ashfall.Core.Survivors`)

**Owns:** the relation graph beyond parentage — sibling, half-sibling, guardian,
ward, cousin, in-law-style obligations. It does **not** own parent IDs; those stay
on `CohortChild.parentIds`. It derives relations from authored records plus
parentage where known.

**Data:** `family_kinship.json` (new; authored links + obligations).

**Effects:** bereavement routing, guardian consent for rites, inheritance
eligibility, and faction reaction to nepotism. It emits typed facts; the host maps
them to morale/grief via existing owners.

### 7.4 `SuccessionDossierSystem` (new, extends `GenerationalSuccessionEngine`)

**Owns:** the dossier — role, tools, promises, debts, grievances — and eligibility
ranking. It does **not** move the role; `GenerationalSuccessionEngine` does.

**Data:** `succession_dossiers.json` (new).

**Rules:** a dossier is created on a founder's death; the heir is chosen by the
authored rule; inherited debt is applied to `FactionStanceEngine` standing as a
typed fact; inherited grief routes to `GuiltInsomniaSystem`.

### 7.5 `ComingOfAgeRiteSystem` (new, `Ashfall.Core.Survivors`)

**Owns:** authored rite definitions, eligibility, the trial's deterministic
resolution, and the outcome trait grant. It consumes `CohortSystem.isMatured`
and grants the adult transition through existing paths only.

**Data:** `coming_of_age_rites.json` (new).

### 7.6 `ChildWellbeingSystem` (new, thin, `Ashfall.Core.Survivors`)

**Owns:** a derived wellbeing score from nutrition, safety, trauma, and caregiver
presence, used only for player surfacing and milestone gating. It is **not** a
needs model; it reads `NeedsSystem` and `ChildDevelopmentSystem` and computes a
read-only index.

### 7.7 Systems explicitly **not** added

- No `PregnancySystem` (births are authored/intake events).
- No `RomanceSystem`.
- No second `NeedsSystem`, `HealthSystem`, `InventorySystem`, `SkillSystem`.
- No `ChildLaborSystem`; chore efficiency is read from `ChildDevelopmentSystem`.
- No new RNG stream; uses `CampaignRngStream` deterministic forks.

---

## 8. DATA CATALOG SPECIFICATION

Every new catalog is snake_case, integer `schema_version: 1`, validated by
`CatalogIntegrityValidator`, and registered with `ContentUtilizationScanner`.

### 8.1 `education_curricula.json` (new)

```json
{
  "schema_version": 1,
  "curricula": [
    {
      "curriculum_id": "curriculum_letters",
      "display_name": "Letters & Ledgers",
      "description": "Reading, ration arithmetic, and record-keeping. The shelter's memory depends on it.",
      "daily_education_xp": 6,
      "primary_skill_ids": ["skill_literacy", "skill_bureaucracy"],
      "adult_skill_floor": 2,
      "apprenticeship_tracks": ["track_clerk", "track_medic_records"],
      "required_materials": [{ "item_id": "item_slate", "quantity": 1 }],
      "teacher_role_id": "role_teacher",
      "tags": ["letters", "records"]
    }
  ]
}
```

Proposed rows (12): letters, craft, medicine, arms, survival, tailoring, brewing,
radio, cooking, animal husbandry, construction, rhetoric.

### 8.2 `childhood_milestones.json` (new)

```json
{
  "schema_version": 1,
  "milestones": [
    {
      "milestone_id": "milestone_first_walk",
      "display_name": "First Walk",
      "min_stage": "Infant",
      "max_stage": "Toddler",
      "min_age_days": 20,
      "max_age_days": 60,
      "requires_safety_min": 40.0,
      "effect": { "development_progress": 3.0, "morale_delta": 2.0 },
      "description": "The child crosses the creche floor to a caregiver's hands."
    }
  ]
}
```

Proposed rows (30): first walk, first word, first chore, first recitation, first
loss, first radio, first tool, first aid, first hunt, first burial, etc.

### 8.3 `family_kinship.json` (new)

Authored relation records plus obligation types (`ward`, `guardian`, `sibling`,
`cousin`, `kin_debt`, `blood_grudge`).

### 8.4 `succession_dossiers.json` (new)

Dossier templates (role, tools, promises, debts, grievances) and selection rules.

### 8.5 `coming_of_age_rites.json` (new)

Rite definitions with trial type (`range`, `ledger`, `tool`, `mercy`, `vigil`),
deterministic difficulty, and granted trait IDs.

### 8.6 `development_traits.json` (extend, 7 → 60)

New trait IDs must remain in the existing schema (`trait_id`, `display_name`,
`description`, `polarity`, `stat_modifiers`, `skill_modifiers`, `morale_modifiers`,
`work_modifiers`, `weight`, `min_trauma`, `max_trauma`, `min_education`,
`exclusive_group`). Proposed additions include:

- `development_trait_ledger_minded` (letters)
- `development_trait_steady_hands` (medicine)
- `development_trait_quiet_step` (survival)
- `development_trait_deaf_to_panic` (arms)
- `development_trait_scar_tongued` (trauma)
- `development_trait_raised_by_hands` (craft)
- `development_trait_underfed_frame` (stunting variant)
- `development_trait_never_alone` (sibling bond)
- `development_trait_orphaned_early` (loss)
- `development_trait_makeshift_teacher` (self-taught)
- `development_trait_ration_guilt` (food insecurity)
- `development_trait_kind_under_fire` (mercy)
- `development_trait_owns_nothing` (poverty)
- `development_trait_keeps_promises` (dossier)
- `development_trait_inherited_grudge` (succession)

### 8.7 `cohort_tuning.json` (extend, 4 → ~18 fields)

Additive fields only, backward compatible:
`nursery_capacity`, `caregiver_ratio`, `education_xp_per_day_base`,
`trauma_decay_per_day`, `stunting_recovery_per_day`,
`adult_transition_skill_floor`, `rite_required`, `milestone_check_interval_days`,
`child_labor_max_shift_hours`, `memorial_name_required`, etc. Existing four fields
must not change meaning.

### 8.8 Items

New items are appended to `items.json` with existing fields (`id`, `displayName`,
`description`, `type`, `stackMax`, `weight`, `tradeValue`). Proposed items:

`item_slate`, `item_chalk`, `item_primer_letters`, `item_crib_slat`,
`item_warm_blanket`, `item_heirloom_wrench`, `item_carved_token`,
`item_dose_card_blank`, `item_ledger_book`, `item_rite_charm`,
`item_child_medicine_syrup`, `item_wooden_toy_hound`.

All descriptions follow the restrained tone rules and the existing description
catalog conventions (`item_description_texts.json` may carry long-form prose).

---

## 9. SAVE, DETERMINISM, AND PERSISTENCE

### 9.1 Save ownership

State is captured by `src/Host/GenerationalSaveStore.cs` and
`src/Host/ChildDevelopmentSaveStore.cs`. New sub-objects are added inside the
existing envelopes; no new save section, no new slot-root.

### 9.2 Capture/restore parity

Every stateful Core system must implement capture and restore before integration,
and pass the Triad drift gate (`Setup` / `Save` / `Flush` parity). New state:

- `CurriculumSystem`: per-child focus + XP.
- `FamilyKinshipSystem`: authored + discovered relations and obligations.
- `SuccessionDossierSystem`: active dossiers and inheritance outcomes.
- `ComingOfAgeRiteSystem`: rite history and granted traits.

### 9.3 Determinism contract

- All rolls use the host-forked `ISeededRng` through `CampaignRngStream`; never
  `System.Random`, never wall-clock, never hash iteration order.
- Rite resolution, milestone firing, and complication rolls must produce byte-
  identical traces across a continuous run and a run interrupted by save/load.
- A paired-replay test must hash the full generational state and compare.

### 9.4 Migration

Legacy saves without the new sub-objects load with neutral defaults: no children
lost, no traits granted, no relations invented. Frozen-shape legacy structs follow
the existing V→V pattern used by `RadioSave`.

### 9.5 Checksum

`SaveChecksum` formatting must remain culture-invariant. Any new float in a save
payload is serialized with invariant culture; integer-permille is preferred.

---

## 10. UI, ACCESSIBILITY, AND PRESENTATION

### 10.1 Surfaces

| Surface | Purpose | Owner |
|---|---|---|
| `CrechePanel` (new) | Nursery capacity, warmth, caregiver assignment | `GenerationalHostSession` |
| `SchoolhousePanel` (new) | Curriculum assignment, teacher role, class roster | same |
| `KinshipPanel` (new) | Family graph, obligations, guardians | same |
| `SuccessionPanel` (new) | Dossier view, heir ranking, inherited debt | same |
| `RitePanel` (new) | Rite selection and resolution | same |
| SurvivorsPanel (extend) | Child stage, education, wellbeing sub-tab | `SurvivorsHostSession` |
| MemorialPanel (extend) | Child names and cause | `MemorialSystem` host |

### 10.2 Accessibility

- Full keyboard/controller navigation and a clear back/close path.
- Focus is preserved on refresh; panels bind/unbind/rebind through the existing
  panel lifecycle.
- Contrast meets the project's readability rules; no information is conveyed by
  color alone.
- Text scales without clipping at the fixed 1920×1080 target.
- Child names and outcomes are always readable, never truncated mid-name.

### 10.3 Presentation

- Audio cues for milestone, rite, and loss are registered in `audio_cues.json`
  and resolved through the existing audio manager; undiscovered events are silent.
- No new audio authority; the expansion adds cue IDs only.
- Ambient nursery audio is a low, warm hum; the schoolhouse has slate-scratch and
  stove crackle. Both are optional and degrade to text.

---

## 11. INTEGRATION SEAMS

| Existing system | How the expansion attaches |
|---|---|
| `NeedsSystem` | Reads morale/hunger/thirst/warmth for wellbeing; writes nothing |
| `MoraleContagionSystem` | Registers grief and joy facts from milestones and rites |
| `GuiltInsomniaSystem` | Receives inherited-grief and child-loss typed facts |
| `MemorialSystem` | Receives child death and memorial inscription facts |
| `SkillProgressionSystem` | Receives adult-transition skill floor; grants apprentice XP |
| `ApprenticeshipSystem` | Consumes `CurriculumSystem.MapToApprenticeshipTrack` |
| `DiseaseSystem` | Pediatric illness uses the existing vector/incubation model |
| `CombatTraumaSystem` | Child combat exposure produces trauma, never a buff |
| `FactionStanceEngine` | Inherited debts and nepotism reactions |
| `PowerGridSystem` | Creche heat and school lighting draw real watts |
| `Inventory` | Slates, primers, blankets, tokens are normal items |
| `WeatherSystem` | Cold snaps stress nursery warmth and school attendance |

No existing system is modified in a way that changes its public contract. All
attachments are additive reads or typed events.

---

## 12. TECHNICAL IMPLEMENTATION PLAN

### 12.1 Phase order (dependency first)

**Phase 0 — Premise re-audit.** Confirm each claimed `LIVE` system still exists
and its API has not drifted. Confirm save stores still own the envelopes. Record
evidence; do not change code.

**Phase 1 — Data schemas.** Author `education_curricula.json`,
`childhood_milestones.json`, `family_kinship.json`, `succession_dossiers.json`,
`coming_of_age_rites.json`; extend `development_traits.json` and
`cohort_tuning.json`. Register validators and content scanner. No gameplay yet.

**Phase 2 — Pure Core systems.** Implement `CurriculumSystem`,
`DevelopmentalMilestoneSystem`, `FamilyKinshipSystem`, `SuccessionDossierSystem`,
`ComingOfAgeRiteSystem`, `ChildWellbeingSystem` with unit tests and no host wiring.

**Phase 3 — Persistence.** Extend the two save stores with capture/restore and
legacy migration; add round-trip and determinism tests.

**Phase 4 — Host session and CLI.** Extend `GenerationalHostSession`; add
`--generational-selftest` assertions and a fresh journey check.

**Phase 5 — UI surfaces.** Build the six panels, bind lifecycle, accessibility.

**Phase 6 — Content.** Author locations, NPCs, quests, items, prose, audio cues.

**Phase 7 — Balance and soak.** 30-day and 180-day deterministic soak; tune.

**Phase 8 — Verification and closeout.** Full focused suite, data integrity,
content utilization, docs index, and a closeout log.

### 12.2 Estimated content volume

| Content | Count |
|---|---|
| New development traits | 53 (7 → 60) |
| Curricula | 12 |
| Milestones | 30 |
| Side quests | 24 |
| Main quests | 12 |
| Repeatable | 5 |
| NPCs | 8 |
| Locations | 8 |
| Rooms | 5 |
| Items | 12 |
| Endings | 4 + fade |
| Prose estimate | 45,000–60,000 words |

### 12.3 Risks

| Risk | Severity | Mitigation |
|---|---|---|
| Second-generation power creep | High | One-way maturation, long timer, no instant worker |
| Tone drift toward glamorized child combat | High | Trauma routing, no combat reward path |
| Save bloat | Medium | Compact integer state, per-child records only |
| Duplicate needs model | High | `ChildWellbeingSystem` is read-only derived |
| Content dead-data | Medium | ContentUtilizationScanner registration and CI gate |
| Drift with `GenerationalSystem` Plan 178 | Medium | Premise re-audit before each phase; extend, never fork |

---

## 13. TEST AND VERIFICATION PLAN

Focused, per `TEST_POLICY.md`. A builder stays below 100 cases per file.

### 13.1 New test files

- `Ashfall.Core.Tests/Survivors/CurriculumSystemTests.cs`
- `Ashfall.Core.Tests/Survivors/DevelopmentalMilestoneTests.cs`
- `Ashfall.Core.Tests/Survivors/FamilyKinshipTests.cs`
- `Ashfall.Core.Tests/Survivors/SuccessionDossierTests.cs`
- `Ashfall.Core.Tests/Survivors/ComingOfAgeRiteTests.cs`
- `Ashfall.Core.Tests/Survivors/GenerationalSaveRoundTripTests.cs`
- `Ashfall.Core.Tests/Survivors/GenerationalDeterminismTests.cs`
- `Ashfall.Core.Tests/Content/GenerationalCatalogIntegrityTests.cs`

### 13.2 Required assertions

- One-way maturation: `isMatured` cannot revert; maturation only after the timer.
- Curriculum XP bounded; skill floor never exceeds cap; apprenticeship gated.
- Milestone firing order deterministic; no duplicate fire.
- Kinship derived relations stable across save/load.
- Dossier selection deterministic; inherited debt applied exactly once.
- Rite resolution identical in continuous and interrupted runs.
- Legacy save loads neutral; no invented children or traits.
- Data integrity: all new IDs valid; no orphan references.

### 13.3 Commands

```bash
bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/
godot --headless --path . -- --generational-selftest
godot --headless --path . -- --cohort-lifecycle-selftest
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --content-utilization-selftest
dotnet build Ashfall.csproj --no-restore
```

No full-suite run unless the foreman requests it.

---

## 14. ACCEPTANCE CRITERIA (DEFINITION OF DONE)

A component of this expansion is done only when all are true:

1. **Core authority** — the behavior lives in an existing or explicitly claimed
   engine-free Core system with a documented authority split.
2. **Data** — content lives in canonical JSON, integer `schema_version`, passing
   `CatalogIntegrityValidator`.
3. **Persistence** — state round-trips through the existing store; legacy loads
   neutral; Triad parity holds.
4. **Determinism** — paired-replay hashes match; no `System.Random`.
5. **Host** — reachable through a real route or event, surfaced honestly in UI.
6. **Observation** — the player can see the outcome; no silent state.
7. **Tests** — focused tests pass; data integrity and utilization pass.
8. **Docs** — the catalog and authority references are updated; no hand-edited
   generated file.

A compile-green result is not acceptance. Runtime integration is required.

---

## 15. CROSS-EXPANSION HOOKS (WAVE 1)

| Other expansion | Hook |
|---|---|
| 13 The Faithful | A child's coming-of-age rite can be religious; belief movements recruit youth |
| 14 Above the Ash | Children watch aircraft; a first flight is a milestone |
| 15 The Deep Root | School gardens; children as apprentice growers |
| 16 The Rebuilt Body | A child with a prosthetic; pediatric integration content |

Hooks are additive. Each expansion can ship without the others.

---

## 16. PLAYER ENGAGEMENT AND RETENTION

- **Day-one after unlock:** a child asks for a name; the ledger is blank.
- **Week one:** curriculum choice and first milestone.
- **Month one:** first coming-of-age rite; first inheritance.
- **Long tail:** the second generation's dossier changes how the endgame reads.
- **Return motivation:** the ledger's final form is a real branching ending.

No monetization is proposed. This is campaign content.

---

## 17. LORE AND CONTINUITY CHECK

### 17.1 Must not contradict

- The dose baseline's inherited uncertainty (`CohortSystem`).
- Child mortality's existing permanence (`isDeceased`, `deathCause`).
- The shelter's ration scarcity and the `child_ration_fraction` rule.
- The established factions and their dual ID namespace.
- The restrained tone and fiction-only rule.

### 17.2 New canon introduced

- Named shelter generations and the deliberately blank parentage columns.
- The Crèche, the Schoolhouse, and the Cinder Ring as places.
- The rite forms (range, ledger, tool, mercy, vigil).
- The dossier obligation vocabulary.

### 17.3 Provenance

New canon is recorded in `docs/ASHFALL_IMPLEMENTED_CANON_REGISTRY.md` when
integrated, with the owning file and test that proves reachability.

---

## 18. APPENDIX A — PROPOSED ID REGISTRY

**Traits:** `development_trait_ledger_minded`, `development_trait_steady_hands`,
`development_trait_quiet_step`, `development_trait_deaf_to_panic`,
`development_trait_scar_tongued`, `development_trait_raised_by_hands`,
`development_trait_underfed_frame`, `development_trait_never_alone`,
`development_trait_orphaned_early`, `development_trait_makeshift_teacher`,
`development_trait_ration_guilt`, `development_trait_kind_under_fire`,
`development_trait_owns_nothing`, `development_trait_keeps_promises`,
`development_trait_inherited_grudge`.

**Curricula:** `curriculum_letters`, `curriculum_craft`, `curriculum_medicine`,
`curriculum_arms`, `curriculum_survival`, `curriculum_tailoring`,
`curriculum_brewing`, `curriculum_radio`, `curriculum_cooking`,
`curriculum_husbandry`, `curriculum_construction`, `curriculum_rhetoric`.

**Rooms:** `room_creche`, `room_schoolhouse`, `room_workshop_apprentice_bay`,
`room_infirmary_pediatric`, `room_memorial_niche`.

**Locations:** `loc_ash_orphanage`, `loc_buried_school`, `loc_playground_markers`,
`loc_midwife_cabin`, `loc_lineage_vault`, `loc_rites_ring`, `loc_cousin_farm`,
`loc_letter_archive`.

**NPCs:** `npc_midwife_orsa_vell`, `npc_teacher_bran_ike`, `npc_child_petra_nine`,
`npc_guardian_halder_rusk`, `npc_archivist_meret_low`, `npc_heir_adam_vell`,
`npc_rival_kess_idre`, `npc_grandmother_sia`.

**Quests:** all `quest_gen_*` as listed in §6.

**Items:** `item_slate`, `item_chalk`, `item_primer_letters`, `item_crib_slat`,
`item_warm_blanket`, `item_heirloom_wrench`, `item_carved_token`,
`item_dose_card_blank`, `item_ledger_book`, `item_rite_charm`,
`item_child_medicine_syrup`, `item_wooden_toy_hound`.

**Milestones:** `milestone_*` (30 rows).

**Dossiers:** `dossier_*` templates.

**Rites:** `rite_range`, `rite_ledger`, `rite_tool`, `rite_mercy`, `rite_vigil`.

---

## 19. APPENDIX B — PROSE AND VOICE GUIDE

The expansion's prose must read like the rest of ASHFALL: quiet, specific,
fictional, and human. Rules:

- Name objects and actions, not abstractions. "The slate cracked along the chalk
  line," not "education was hard."
- Let grief be understated. A kept object says more than a speech.
- Children's dialogue is concrete and repetitive; adults' dialogue is tired.
- Never use real religious language, real prayers, or real political terms.
- Avoid uplift. The shelter endures; it does not triumph.

Sample milestone prose (First Walk):

> She crossed the creche floor between two pairs of hands and did not fall. Orsa
> wrote the day on the door frame with a pencil stub, and then, because the pencil
> was nearly gone, she wrote it small.

Sample loss prose (A Name on the Wall):

> The wall had room for eleven more names. They carved hers shallow, the way you
> carve when you are not sure you will be allowed to finish.

---

## 20. APPENDIX C — OPEN DECISIONS REQUIRING A FOREMAN SIGNATURE

1. **Parentage truth policy** — does the campaign canon allow a child's true dose
   band to be permanently unknowable? Recommended: yes, per `CohortSystem`'s
   existing guess/correction model.
2. **Child combat floor** — what is the minimum age for any combat exposure?
   Recommended: adolescent stage only, with trauma routing.
3. **Succession rule default** — merit, blood, or election? Recommended: merit
   with an authored override, so the player can choose.
4. **Save section placement** — extend `child_development` or add a sibling
   `generational_kinship` sub-object inside the same envelope? Recommended:
   same envelope, additive sub-object.
5. **Prose volume** — confirm 45–60k words is in budget before authoring.

Until these are signed, implementation stays at Phase 1 (schemas and validators),
which is safe and reversible.

---

## 21. APPENDIX D — FULL DEVELOPMENT TRAIT CATALOG (7 LIVE + 53 PROPOSED)

Schema is unchanged from `development_traits.json`. `min_education` and
`max_trauma` are the gating axes. `exclusive_group` prevents contradictory traits
from co-occurring on one survivor.

| # | Trait ID | Display | Group | min_edu | max_trauma | Signature effect |
|---|---|---|---|---|---|---|
| 1 | `development_trait_resilient` *(LIVE)* | Hardened Resilience | resilience | 20 | 50 | +10 stamina, +0.15 cold resist, survival +15 |
| 2 | `development_trait_field_medic_instinct` *(LIVE)* | Field Medic Instinct | vocation | 40 | 40 | medicine +25, treatment speed +0.2 |
| 3 | `development_trait_careful_scavenger` *(LIVE)* | Careful Scavenger | scavenging | 15 | 60 | carry +5, scavenging +20, stealth +15 |
| 4 | `development_trait_mechanically_minded` *(LIVE)* | Mechanically Minded | vocation | 25 | 45 | repair/craft bonus |
| 5 | `development_trait_hypervigilant` *(LIVE)* | Hypervigilant | resilience | 10 | 90 | reaction speed +, rest penalty |
| 6 | `development_trait_malnourished_growth` *(LIVE)* | Stunted Growth | stricken | 0 | 70 | stamina penalty, illness risk + |
| 7 | `development_trait_authority_averse` *(LIVE)* | Authority-Averse | social | 20 | 55 | obedience penalty, initiative + |
| 8 | `development_trait_ledger_minded` | Ledger-Minded | letters | 35 | 40 | bureaucracy +20, arithmetic tasks faster |
| 9 | `development_trait_steady_hands` | Steady Hands | medicine | 35 | 45 | surgery precision +, no tremor penalty in crisis |
| 10 | `development_trait_quiet_step` | Quiet Step | survival | 20 | 55 | stealth +20, noise events reduced |
| 11 | `development_trait_deaf_to_panic` | Deaf to Panic | arms | 25 | 65 | fear resistance +, morale contagion resistance + |
| 12 | `development_trait_scar_tongued` | Scar-Tongued | stricken | 10 | 95 | social friction +, interrogation resistance + |
| 13 | `development_trait_raised_by_hands` | Raised by Hands | vocation | 20 | 45 | craft speed +, tool wear reduced |
| 14 | `development_trait_underfed_frame` | Underfed Frame | stricken | 0 | 60 | carry penalty, food efficiency + (needs less) |
| 15 | `development_trait_never_alone` | Never Alone | kinship | 15 | 50 | morale support + in groups, solo penalty |
| 16 | `development_trait_orphaned_early` | Orphaned Early | kinship | 5 | 80 | self-reliance +, grief susceptibility + |
| 17 | `development_trait_makeshift_teacher` | Makeshift Teacher | letters | 30 | 40 | training speed for others + |
| 18 | `development_trait_ration_guilt` | Ration Guilt | stricken | 10 | 75 | hoarding tendency, morale drain when others starve |
| 19 | `development_trait_kind_under_fire` | Kind Under Fire | mercy | 30 | 60 | fear contagion reduced in others, mercy options unlocked |
| 20 | `development_trait_owns_nothing` | Owns Nothing | poverty | 5 | 55 | theft resistance, morale flat |
| 21 | `development_trait_keeps_promises` | Keeps Promises | honor | 25 | 45 | faction standing decay reduced |
| 22 | `development_trait_inherited_grudge` | Inherited Grudge | succession | 15 | 85 | faction-specific penalty at start of adult life |
| 23 | `development_trait_first_heir` | First Heir | succession | 30 | 40 | leadership bonus, nepotism reaction |
| 24 | `development_trait_clinic_raised` | Clinic-Raised | medicine | 30 | 50 | disease resistance +, anatomy familiarity |
| 25 | `development_trait_fire_tender` | Fire Tender | survival | 15 | 40 | fuel efficiency +, warmth retention + |
| 26 | `development_trait_grain_counter` | Grain Counter | letters | 25 | 35 | spoilage detection + |
| 27 | `development_trait_ditch_digger` | Ditch Digger | labor | 10 | 45 | construction +, endurance + |
| 28 | `development_trait_pipe_reader` | Pipe Reader | vocation | 25 | 40 | plumbing/fluid repair + |
| 29 | `development_trait_wall_listener` | Wall Listener | survival | 20 | 60 | structural danger warning + |
| 30 | `development_trait_lamp_keeper` | Lamp Keeper | labor | 10 | 35 | night work penalty reduced |
| 31 | `development_trait_seed_saver` | Seed Saver | agriculture | 25 | 40 | seed viability + |
| 32 | `development_trait_beast_quiet` | Beast-Quiet | husbandry | 15 | 45 | animal bond rate + |
| 33 | `development_trait_stitch_mouth` | Stitch-Mouth | social | 20 | 70 | secrecy +, trust gain slow |
| 34 | `development_trait_open_hand` | Open Hand | social | 20 | 40 | gift/trade bonus, theft vulnerability |
| 35 | `development_trait_ash_lung` | Ash Lung | stricken | 0 | 65 | respiratory penalty, dust resistance + |
| 36 | `development_trait_rad_sighted` | Rad-Sighted | stricken | 10 | 55 | anomaly detection +, vision penalty in bright light |
| 37 | `development_trait_second_winter` | Second Winter | resilience | 20 | 50 | cold tolerance + after a survived winter |
| 38 | `development_trait_raised_on_song` | Raised on Song | culture | 20 | 35 | morale support +, ritual participation + |
| 39 | `development_trait_rite_keeper` | Rite Keeper | culture | 30 | 45 | ceremony preparation bonus |
| 40 | `development_trait_forge_ear` | Forge Ear | vocation | 20 | 40 | foundry defect rate reduced |
| 41 | `development_trait_chem_nose` | Chem Nose | vocation | 30 | 40 | compound purity + |
| 42 | `development_trait_wound_reader` | Wound Reader | medicine | 35 | 55 | triage accuracy + |
| 43 | `development_trait_night_scout` | Night Scout | arms | 20 | 60 | night watch efficiency + |
| 44 | `development_trait_blood_steady` | Blood-Steady | medicine | 30 | 50 | bleeding control + |
| 45 | `development_trait_coward_wise` | Wise Coward | survival | 15 | 70 | retreat bonus, no death-from-bravado |
| 46 | `development_trait_mule_back` | Mule Back | labor | 5 | 45 | carry capacity + |
| 47 | `development_trait_deaf_ear` | Half-Deaf | stricken | 0 | 60 | hearing penalty, sleep through noise + |
| 48 | `development_trait_tally_keeper` | Tally Keeper | letters | 30 | 35 | stock audit accuracy + |
| 49 | `development_trait_feral_raised` | Feral-Raised | wild | 0 | 75 | survival +, social integration penalty |
| 50 | `development_trait_wall_raised` | Wall-Raised | shelter | 20 | 40 | shelter familiarity +, expedition fear + |
| 51 | `development_trait_tunnel_eye` | Tunnel Eye | survival | 20 | 55 | dark navigation + |
| 52 | `development_trait_bargain_blood` | Bargain Blood | trade | 25 | 45 | trade value + |
| 53 | `development_trait_thin_patience` | Thin Patience | social | 15 | 60 | task completion speed +, conflict risk + |
| 54 | `development_trait_long_memory` | Long Memory | honor | 25 | 50 | grievance tracking + |
| 55 | `development_trait_short_memory` | Short Memory | mercy | 10 | 40 | grudge decay +, lesson retention - |
| 56 | `development_trait_ash_artist` | Ash Artist | culture | 25 | 45 | memorial/art value + |
| 57 | `development_trait_machinist_patient` | Patient Machinist | vocation | 35 | 35 | precision work + |
| 58 | `development_trait_fireproof_will` | Fireproof Will | resilience | 25 | 70 | panic recovery + |
| 59 | `development_trait_ledger_debt` | Ledger Debt | succession | 20 | 60 | starts adult life with an obligation |
| 60 | `development_trait_last_child` | Last Child | kinship | 15 | 80 | survivor guilt, cohesion drive + |

Weights are authored per row and must sum sensibly within each `exclusive_group`.
Additive fields only; the seven live rows keep their exact IDs and numbers.

---

## 22. APPENDIX E — FULL MILESTONE CATALOG (30 ROWS)

| # | Milestone ID | Stage window | Age window (days) | Effect |
|---|---|---|---|---|
| 1 | `milestone_first_walk` | Infant→Toddler | 20–60 | +3 development, +2 morale |
| 2 | `milestone_first_word` | Infant→Toddler | 30–90 | +2 development |
| 3 | `milestone_first_solid_food` | Toddler | 40–120 | nutrition baseline set |
| 4 | `milestone_first_chore` | Toddler→Child | 90–180 | +1 chore efficiency |
| 5 | `milestone_first_recitation` | Child | 180–260 | +4 education XP |
| 6 | `milestone_first_loss` | Child | any | trauma +, grief flag |
| 7 | `milestone_first_radio` | Child | any | wonder flag, +2 morale |
| 8 | `milestone_first_tool` | Child | any | +2 craft XP |
| 9 | `milestone_first_aid` | Child | any | +3 medicine XP |
| 10 | `milestone_first_burial` | Child→Adolescent | any | grief flag, memorial link |
| 11 | `milestone_first_ration_board` | Child | any | letters XP, guilt risk |
| 12 | `milestone_first_sibling` | Child | any | kinship link |
| 13 | `milestone_first_hunt` | Adolescent | any | survival XP, trauma risk |
| 14 | `milestone_first_night_watch` | Adolescent | any | arms familiarity, fear check |
| 15 | `milestone_first_repair` | Child→Adolescent | any | craft XP |
| 16 | `milestone_first_garden` | Child | any | agriculture XP |
| 17 | `milestone_first_beast` | Child | any | husbandry bond |
| 18 | `milestone_first_ledger` | Adolescent | any | letters XP |
| 19 | `milestone_first_defect` | Adolescent | any | craft humility flag |
| 20 | `milestone_first_apology` | Child→Adolescent | any | honor flag |
| 21 | `milestone_first_lie_caught` | Child→Adolescent | any | trust friction flag |
| 22 | `milestone_first_promise_kept` | Adolescent | any | honor flag |
| 23 | `milestone_first_promise_broken` | Adolescent | any | guilt source |
| 24 | `milestone_first_aircraft` | Child | any | wonder flag |
| 25 | `milestone_first_storm` | Child | any | survival XP, fear check |
| 26 | `milestone_first_shift` | Adolescent | any | labor XP, exhaustion risk |
| 27 | `milestone_first_voice_recorded` | Child | any | culture flag |
| 28 | `milestone_first_letter_written` | Child→Adolescent | any | letters XP |
| 29 | `milestone_first_name_carved` | Adolescent | any | memorial link |
| 30 | `milestone_adult_transition` | Adolescent→Adult | maturation gate | adult survivor emitted |

Firing is deterministic and once-only. A milestone whose conditions are not met
by `max_age_days` becomes missed, and missed milestones are recorded (not erased),
because the ledger should remember what did not happen.

---

## 23. APPENDIX F — FULL CURRICULUM TABLE

| Curriculum | Focus | Daily XP | Skill floor | Tracks | Material cost |
|---|---|---|---|---|---|
| `curriculum_letters` | Literacy, records | 6 | 2 | clerk, records medic | 1 slate |
| `curriculum_craft` | Fabrication | 7 | 2 | machinist, smith | 1 scrap tool |
| `curriculum_medicine` | Care | 5 | 3 | medic, surgeon assistant | 1 bandage |
| `curriculum_arms` | Defense | 7 | 2 | guard, scout | 1 training round |
| `curriculum_survival` | Fieldcraft | 8 | 2 | scavenger, runner | none |
| `curriculum_tailoring` | Textiles | 6 | 2 | tailor, leatherworker | 1 cloth |
| `curriculum_brewing` | Fermentation | 6 | 3 | brewer, chemist | 1 mash |
| `curriculum_radio` | Signals | 5 | 4 | operator, cryptographer | 1 wire |
| `curriculum_cooking` | Nutrition | 6 | 2 | cook, preserver | 1 ration |
| `curriculum_husbandry` | Animals | 5 | 2 | herder, vet assistant | 1 feed |
| `curriculum_construction` | Building | 7 | 2 | builder, foreman | 1 timber |
| `curriculum_rhetoric` | Persuasion | 5 | 3 | negotiator, envoy | 1 book |

A child may hold one active curriculum. Switching costs a transition penalty and
is allowed only once per stage, so the choice has weight.

---

## 24. APPENDIX G — ECONOMY AND BALANCE MODEL

### 24.1 Cost curve

A child consumes `child_ration_fraction × adult_ration` for
`maturation_age_days`. With the live default of 0.5 and 180 days, the shelter
expends roughly 90 adult-ration-days per child before any return. Two caregivers
at nursery ratio 1:3 remove up to two-thirds of a worker from the labor pool at
peak. The expansion must make this cost visible and must not let a child pay it
back inside a single season.

### 24.2 Return curve

- Toddler: zero labor, morale effect only.
- Child: chore efficiency 0.2–0.4 of adult; light duties only.
- Adolescent: 0.4–0.7 of adult; apprenticeship XP begins.
- Adult transition: full survivor with curriculum skill floor and a dossier.

Payback is intended at roughly day 240–300 for a well-fed child, and never for a
starved one. This preserves the long-bet pillar.

### 24.3 Scarcity pressure

Childcare competes directly with`PowerGridSystem` (creche heat),
`GreenhouseSystem` (food), `MedicalWard` (pediatric care), and
`SilentFoundrySystem` (materials). The expansion adds no new resource; it makes
children a competing use for the existing ones.

### 24.4 Failure states

- Sustained half-rations → `development_trait_malnourished_growth`.
- Unsafe creche (cold, breach, blackout) → trauma load and milestone misses.
- Caregiver death → `npc_guardian` loss event; kinship obligation reassigned.

---

## 25. APPENDIX H — AUDIO CUE REGISTRY

Cue IDs are appended to `audio_cues.json` and resolved through the existing audio
manager. No cue is required for correctness; text always carries the meaning.

| Cue ID | Trigger | Character |
|---|---|---|
| `cue_birth_breath` | Birth/intake | one held breath, a low tone |
| `cue_milestone_soft` | Milestone fired | two soft wooden taps |
| `cue_lesson_slate` | School day | chalk on slate, stove crackle |
| `cue_nursery_hum` | Creche ambient | warm low hum, distant pipe |
| `cue_rite_start` | Rite begins | a single struck iron note |
| `cue_rite_pass` | Rite success | the note resolves |
| `cue_rite_fail` | Rite failure | the note decays unresolved |
| `cue_child_lost` | Child death | a long silence then a latch |
| `cue_dossier_open` | Succession panel | paper and a pen |
| `cue_inherited_debt` | Debt applied | a ledger closing |

---

## 26. APPENDIX I — UI STATE REQUIREMENTS

Each panel must truthfully expose current state and must not become a gameplay
authority.

**CrechePanel** — capacity, current occupants, warmth (from power), caregiver
assignment, wellbeing index. Commands: assign caregiver, build crib, toggle heat
priority. Keyboard: full focus order, back closes.

**SchoolhousePanel** — class roster, per-child curriculum, teacher, daily XP,
attendance (blocked by cold/blackout). Commands: assign curriculum, assign teacher,
schedule class.

**KinshipPanel** — relation graph, obligation list, guardianship, disputes.
Commands: recognize kin, assign guardian, resolve dispute.

**SuccessionPanel** — open dossiers, heir ranking with stated rule, inherited
items, inherited debts, inherited grievances. Commands: confirm heir, contest,
acknowledge debt.

**RitePanel** — eligible adolescents, rite options, requirements, deterministic
outcome preview (probability expressed in-world, not as a debug number).

**MemorialPanel extension** — child names, cause, age, and ledger form.

All panels follow bind/unbind/rebind, focus preservation, contrast, and text
scaling rules already enforced by the panel lifecycle selftest.

---

## 27. APPENDIX J — CONTENT AUTHORING BUDGET

| File | Rows | Est. words |
|---|---|---|
| `education_curricula.json` | 12 | 1,800 |
| `childhood_milestones.json` | 30 | 3,500 |
| `family_kinship.json` | 40 links | 2,500 |
| `succession_dossiers.json` | 15 templates | 2,500 |
| `coming_of_age_rites.json` | 5 | 1,200 |
| `development_traits.json` | +53 | 6,000 |
| `cohort_tuning.json` | +14 fields | 400 |
| Quest objectives | 41 quests | 12,000 |
| NPC dialogue/prose | 8 NPCs | 8,000 |
| Location prose | 8 nodes | 3,000 |
| Item descriptions | 12 | 1,800 |
| Ending prose | 5 | 2,500 |
| **Total** | | **~45,200** |

Add codex/journal entries and the total lands in the 45–60k word range.

---

## 28. APPENDIX K — INTEGRATION LEDGER TEMPLATE

When this plan is promoted to implementation, the owning package must record the
following before editing:

| Field | Value |
|---|---|
| Package name | e.g. `GEN-WAVE1-CURRICULUM` |
| Owner | named builder |
| Exact paths | Core, tests, host, UI, data, docs |
| Boundaries untouched | shared seams, other claims |
| Premise evidence | file:line proofs for each assumed API |
| Acceptance | the eight criteria in §14 |
| Focused verify | run_test.sh target + selftest verbs |
| Limitations | anything not covered |

This keeps the expansion honest. A plan is not authority; the ledger is.

---

## 29. APPENDIX L — RISK REGISTER (EXPANDED)

| ID | Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|---|
| R12-1 | Power creep | Med | High | Long maturation, no instant labor, capped traits |
| R12-2 | Tone drift to child soldiers | Med | High | Trauma routing, no combat reward, last-resort framing |
| R12-3 | Duplicate needs | Low | High | Read-only wellbeing index |
| R12-4 | Save bloat | Med | Med | Compact DTOs, per-child only |
| R12-5 | Dead data | Med | Med | Scanner + CI utilization gate |
| R12-6 | Drift with Plan 178 | Med | Med | Premise re-audit per phase |
| R12-7 | Determinism break | Low | High | CampaignRngStream only, paired replay |
| R12-8 | Accessibility regression | Low | Med | Lifecycle + contrast gates |
| R12-9 | Balance trivialized by heirlooms | Med | Med | Bounded modifiers, story-only unique effects |
| R12-10 | Content overrun | Med | Med | Authored budget per file in §27 |

---

## 30. APPENDIX M — GLOSSARY OF NEW TERMS

- **Cohort** — the current child population tracked by `CohortSystem`.
- **Band** — the guessed/corrected radiation baseline of a child.
- **Dossier** — the inherited role, tools, promises, debts, and grievances.
- **Rite** — a shelter-agreed test marking adulthood.
- **Wellbeing index** — a read-only derived score, not a needs model.
- **Ledger of Names** — the in-fiction record the expansion is about.

---

## 31. CLOSING STATEMENT

ASHFALL is a game about what survives. It already simulates the arithmetic of the
next generation. What it lacks is the story of it. The Second Generation gives the
cohort layer the content it was always shaped for: names, lessons, rites, debts,
and the quiet, terrible weight of handing a broken world to someone younger than
the brokenness. It adds no new authority, no new scarcity model, and no new
fantasy. It adds a ledger, and asks the player what to write in it.