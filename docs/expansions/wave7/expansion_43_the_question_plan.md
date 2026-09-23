# ASHFALL — Expansion 43 Design Bible
# THE QUESTION
### Wave 7 · Inquiry, Experiments, Field Studies, Archives, Peer Review, and Knowledge

**Document status:** Design plan (pre-integration). Not a claim. Not an authorization.
**Date:** 2026-09-24
**Domain owners touched:** `Ashfall.Core.Research` (ResearchSystem, ResearchKnowledgeDef, ResearchState, ResearchEligibility, PrewarArchiveDecryptionSystem, PrewarArchiveCatalog, TechSalvageCatalog)
**Proposed host owner:** `InquiryHostSession` (extends research, archive, and study surfaces)
**Existing save sections:** `ResearchState`, `PrewarArchiveDecryptionState`, library study state (read/write at its seam)
**Existing CLI verbs:** `--research-selftest` (if present), `--data-integrity-selftest`, `--content-utilization-selftest`
**Rule compliance:** Godot authoritative; Core engine-free; JSON data authoritative; one authority per concern; deterministic seeded RNG.

---

## 0. HOW TO READ THIS DOCUMENT

This is a **design bible**, not an integration plan. Implementation must later pass
through `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `TEST_POLICY.md`.

ASHFALL already has a knowledge tree. `ResearchSystem` exposes `State`,
`CatalogCount`, `Catalog`, `OnResearchCompleted`, `OnManualUnlocked`,
`Register(def)`, `UnlockManual(id)`, `IsManualUnlocked(id)`,
`HasCapability(id)`, `GetEligibility(id)`, `StartResearch(id, day)`,
`GetDaysRemaining(id)`, and `GetAvailableNodes()`. `ResearchKnowledgeDef`
carries `id`, `displayName`, `category`, `description`, `prerequisites[]`,
`breakthroughItem`, `daysToComplete`, `isUnlocked`, and `isCompleted`.
`ResearchState` carries `expansionUnlocked`, `currentDay`, `unlockedIds`,
`activeResearchId`, `activeResearchDays`, `completedIds`,
`researchPointsAvailable`, `researchPointsLifetimeEarned`, and
`blueprintProgress` of `BlueprintProgressState` (`blueprintId`, `progressPoints`,
`requiredPoints`, `discoveryState`). `ResearchEligibility` carries `Id`,
`CanStart`, `IsDiscovered`, `IsCompleted`, `IsActive`, `MissingPrerequisites`,
`ConflictingActiveResearchId`, and `Code`. `PrewarArchiveDecryptionSystem`
defines `ArchiveDecryptionStatus` and `PrewarArchiveProject` (`ArchiveId`,
`Status`, `Progress`, `TargetProgress`, `AssignedResearcherId`, `DayStarted`,
`DayCompleted`, `HasSolventApplied`). `prewar_archives.json` (4,072 B) holds
archives with `encryption_grade`, `required_room`, `cleaning_solvent_id`,
`base_effort_points`, and `reward_research_ids`.

What does not exist: questions, hypotheses, experiments, controls, field
studies, peer review, refutation, publication, specimen collections, method
training, and the culture that makes a claim worth believing. The knowledge
tree has 62 nodes and no method.

**The Question** turns the knowledge tree into a practice: asking, testing,
replicating, arguing, publishing, and keeping what survives. It extends the
live research and archive owners and hands every material consequence to the
systems that own labs, presses, and workshops.

Conventions: **`LIVE`** confirmed in source/data; **`GAP`** confirmed thin;
**`PROPOSED`** new.

### 0.1 Boundary contracts

| Neighbour | Owns | This plan does |
|---|---|---|
| 28 The Lesson | Education, manuals, study | Uses its study seam; teaches methods, not curriculum |
| 30 The Press | Paper, printing, publications | Requests printing; never owns the press |
| 39 The Reagent | Chemistry labs and safety | Uses its benches and rules |
| 38 The Ward | Clinical care and trials safety | Routes human studies through it |
| 20 The Quiet Hand | Intelligence and decryption tradecraft | Encourages archive work; never spies |
| 03 The Standing Record | Records and archives | Files discovered facts |
| 32 The Wild | Wildlife knowledge | Studies it; never owns species state |
| 46 The Long Change (Wave 7) | World evolution | Observes change; never mutates it |
| 40 The Wheel | Machine tools and prototypes | Orders prototype parts |
| 10 The Silent Foundry | Materials | Orders test coupons |

---

## 1. EXECUTIVE SUMMARY

### 1.1 Pitch

The shelter has beliefs, arguments, and a water supply that tastes wrong for
reasons nobody can prove.

**The Question** is the expansion about method: writing a question down,
proposing an answer, testing it against a control, repeating it, arguing about
it honestly, publishing the result, and teaching the habit to the next person.
It gives the shelter a question board, a bench, a field notebook, an archive
desk, a review circle, and a place to keep what survives.

### 1.2 The five loops it adds

```
  Ask ──► Guess ──► Test ──► Replicate ──► Keep
   │        │         │          │           │
   ▼        ▼         ▼          ▼           ▼
 Board,  Hypotheses  Bench,    Second      Holdings,
 method  ranked      controls  run         papers
                                      │
                                      ▼
                              Review ──► Refute or accept ──► Teach
```

### 1.3 What the player manages

1. **Questions.** What the shelter wants to know, ranked and owned.
2. **Hypotheses.** Guesses written down before the result is known.
3. **Experiments.** Apparatus, controls, variables, and repeats.
4. **Field studies.** Transects, samples, counts, and seasons.
5. **Archives.** Recovery, cleaning, decryption, and translation.
6. **Review.** Refutation, replication, and honest publication.
7. **Holdings.** What the shelter knows and can defend.
8. **Prototypes.** Turning a finding into a working object.
9. **Teaching.** Passing method, not answers, to the next person.
10. **Culture.** Doubt as respect, and being wrong as ordinary.

### 1.4 What it is not

- Not a tech-tree skin. Findings are earned through method, not points alone.
- Not a second research, study, or archive system. It extends the live owners.
- Not a replacement for 28's lessons; it teaches inquiry as a skill.
- Not a machine for manufacturing certainty; some questions stay open.
- Not a license for human harm; every study routes through consent and the ward.
- Not a new save section.

---

## 2. EVIDENCE BASE AND GAP ANALYSIS

### 2.1 Live systems (verified)

| File | Role | Status |
|---|---|---|
| `Assets/Ashfall.Core/Research/ResearchSystem.cs` | Knowledge unlock and progress | `LIVE` |
| `Assets/Ashfall.Core/Research/ResearchKnowledgeDef.cs` | Knowledge nodes | `LIVE` |
| `Assets/Ashfall.Core/Research/ResearchState.cs` | Unlocks and blueprint progress | `LIVE` |
| `Assets/Ashfall.Core/Research/ResearchEligibility.cs` | Eligibility and blockers | `LIVE` |
| `Assets/Ashfall.Core/Research/PrewarArchiveDecryptionSystem.cs` | Archive projects | `LIVE` |
| `Assets/Ashfall.Core/Research/PrewarArchiveCatalog.cs` | Archive content | `LIVE` |
| `Assets/Ashfall.Core/Research/TechSalvageCatalog.cs` | Salvage knowledge | `LIVE` |
| `Assets/Ashfall.Core/LibraryStudySystem.cs` | Study jobs and manuals | `LIVE` |
| `Assets/Ashfall.Core/Crafting` and `ShelterWorkshopSystem` | Prototype production | `LIVE` |

### 2.2 Live data (counted)

| Catalog | Size | Notes |
|---|---|---|
| `research_knowledge.json` | 21,516 B | 62 knowledge nodes |
| `prewar_archives.json` | **4,072 B** | thin archive list |
| `lost_tech_manuals.json` | 23.5 KB | manuals |
| `library_manuals.json` | 21.9 KB | manuals |
| `tech_salvage.json` | small | salvage knowledge |
| Experiments, field studies, review, holdings data | absent | confirmed none |

### 2.3 Confirmed gaps

- **GAP-43-1 — No question or hypothesis content.**
- **GAP-43-2 — No experiments, controls, or replication.**
- **GAP-43-3 — No field study content.**
- **GAP-43-4 — No review, refutation, or publication culture.**
- **GAP-43-5 — No specimen collections.**
- **GAP-43-6 — No holdings or claim tracking.**
- **GAP-43-7 — No method training.**
- **GAP-43-8 — Archive content is a 4 KB list.**
- **GAP-43-9 — No prototypes tied to findings.**
- **GAP-43-10 — No inquiry rooms or staffing content.**

### 2.4 Non-duplication statement

This expansion will **not** add a second research, study, archive, press, or
laboratory system. It extends `ResearchSystem` with inquiry content,
`PrewarArchiveDecryptionSystem` with archive projects and translation, and the
study seam of `LibraryStudySystem` for method teaching. It adds state only as
additive sub-objects of the existing research and archive stores. No new save
section.

---

## 3. DESIGN PILLARS AND TONE

### 3.1 Pillars

**Pillar 1 — Write it before you know.** A hypothesis recorded after the result
is a story, not a test.

**Pillar 2 — One run is a story; two runs are evidence.** Replication is the
cheapest form of honesty.

**Pillar 3 — The control is the hero.** No study without a comparison.

**Pillar 4 — Being wrong is ordinary.** Refuted work is recorded and taught.

**Pillar 5 — Method outlives facts.** Teach the habit, and the shelter can
learn anything.

### 3.2 Tone calibration

| Element | Do | Do not |
|---|---|---|
| Inquiry | Boards, cards, patience | Genius flashes |
| Experiments | Controls and repeats | Magic results |
| Field study | Seasons and samples | Nature documentary |
| Archives | Cleaning, patience, translation | Treasure hunt |
| Review | Honest argument | Humiliation |
| Refutation | Recorded and respected | Failure and shame |
| Publication | Careful and brief | Sensational |
| Teaching | Method and habit | Facts as authority |

### 3.3 Content limits

- No human experimentation, coercion, or secret trials; studies with people
  route through consent and the live ward owner.
- No torture or interrogation research; that domain belongs to no one.
- No miracle cures; clinical claims require replication and the ward.
- No genius-prophecy framing; knowledge is social, slow, and shared.
- No destruction of inconvenient records; refutation is preserved.
- No claims that contradict the dose ledger, disease engine, or world state.

---

## 4. THE QUESTION WORLD

### 4.1 Interior rooms

- **`room_question_board`** — cards, string, and the ranked list.
- **`room_bench`** — the experiment bench with a control rig.
- **`room_field_desk`** — maps, sample jars, and weather notes.
- **`room_archive_desk_inquiry`** — cleaning trays and a lamp.
- **`room_reading_circle`** — chairs, tea, and a strict agenda.
- **`room_holdings`** — the shelf of what the shelter knows.
- **`room_prototype_corner`** — a bench for making a finding real.
- **`room_method_class`** — the small room where method is taught.

### 4.2 Exterior locations

| ID | Name | Tone | Purpose |
|---|---|---|---|
| `loc_water_shed` | The Water Shed | 3 | Taste and source study |
| `loc_field_plot` | The Field Plot | 2 | Controlled growth |
| `loc_transect_line` | The Transect Line | 3 | Field counting |
| `loc_sample_spring` | The Sample Spring | 2 | Water sampling |
| `loc_ruin_study` | The Study Ruin | 4 | Structure and decay |
| `loc_archive_tent` | The Archive Tent | 2 | Recovery work |
| `loc_review_yard` | The Review Yard | 1 | Open disagreement |
| `loc_press_corner` | The Press Corner | 2 | Publication |
| `loc_observation_hill` | The Observation Hill | 2 | Weather and sky |
| `loc_trial_plot` | The Trial Plot | 3 | Comparison plots |

All locations require valid item references and scanner registration.

### 4.3 The method month

One question runs at a time; a review every week; a published note when a
finding survives; a teaching hour every month. The expansion's clock is the
study.

---

## 5. MAIN STORYLINE — "THE TASTE OF THE WATER"

### 5.1 Central conflict

The water tastes wrong and three people have three theories. **Vera Sol** the
researcher arrives with a notebook and a habit of asking what would make each
theory false. **Oren Bask** the archivist wants the old survey reels cleaned and
read. **Kito Marr** the experimenter wants a bench with a control. **Lune** the
statistician wants the counts to mean something. **Hart Doon** the skeptic wants
the shelter to stop believing whatever is said most confidently. **Silla** the
field researcher wants plots and seasons. **Marn** the inventor wants a finding
to become a thing someone can hold.

Then a beloved remedy is tested and fails, and the shelter learns that refuting
a belief is not an attack on the person who held it.

The expansion's question: **what would make you change your mind?**

### 5.2 Theme (unspoken)

**Doubt is a form of care.**

### 5.3 Principal NPCs

| ID | Name | Role | Function |
|---|---|---|---|
| `npc_lead_researcher_vera_sol` | Vera Sol | Researcher | Questions, method, direction |
| `npc_archivist_oren_bask` | Oren Bask | Archivist | Archives and translation |
| `npc_experimenter_kito_marr` | Kito Marr | Experimenter | Benches, controls, repeats |
| `npc_statistician_lune` | Lune | Statistician | Counts, uncertainty, plots |
| `npc_skeptic_hart_doon` | Hart Doon | Skeptic | Falsification, review |
| `npc_field_researcher_silla` | Silla | Field researcher | Transects and seasons |
| `npc_inventor_marn` | Marn | Inventor | Prototypes from findings |
| `npc_apprentice_researcher_tim` | Tim | Apprentice | Method, notes, checks |

### 5.4 Story beats (15)

1. **The Taste.** Three theories, one water supply.
2. **The Board.** The first question board is hung.
3. **The Guess.** Hypotheses are written before any test.
4. **The Control.** Kito builds a comparison rig and the shelter argues about it.
5. **The Count.** Lune makes the third count the one that matters.
6. **The Remedy.** A trusted remedy fails its test.
7. **The Circle.** The first review is held and nobody is humiliated.
8. **The Reel.** An old survey reel is cleaned and partly read.
9. **The Answer.** The water question closes with a real cause and a fix.
10. **The Open Question.** A second question refuses to close.
11. **The Publication.** The first careful note is printed.
12. **The Prototype.** Marn turns a finding into a working tool.
13. **The Teaching.** Method is taught to the next cohort.
14. **The Winter Count.** A field study survives a season and reports honestly.
15. **The Taste of the Water.** The shelter decides what knowing means.

### 5.5 Branching choices (8)

| Choice | Options | Axis |
|---|---|---|
| Questions | one at a time / three / open board | depth vs. breadth |
| Review | weekly / on result / on challenge | rigor vs. tempo |
| Archives | one project / parallel / paused | focus vs. discovery |
| Publication | notes / journal / oral only | record vs. reach |
| Prototypes | findings-first / needs-first / both | theory vs. practice |
| Teaching | everyone / selected / apprentices | spread vs. depth |
| Field work | single plot / multiple / seasonal | control vs. coverage |
| Final | inquiry as institution / habit / memory | identity |

### 5.6 Endings (5 + fade)

1. **The Method Kept** — the board, the bench, and the circle outlive the
   researcher who started them.
2. **The Honest Shelf** — every holding is defensible, and the shelter knows
   what it does not know.
3. **The Refuted Year** — a year of wrong guesses ends with better questions
   and no shame.
4. **The Long Study** — a field question takes years and reports truly.
5. **The Open Door** — the archive gives up something the old world wanted
   kept, and the shelter decides what to do with it.
6. **Fade** — a card on a board and a pencil beside it, waiting for the next
   question.

---

## 6. QUEST DESIGN

New IDs use prefix `quest_question_`. Schema follows existing quest catalogs.

### 6.1 Main questline (15)

`quest_question_taste`, `quest_question_board`, `quest_question_guess`,
`quest_question_control`, `quest_question_count`, `quest_question_remedy`,
`quest_question_circle`, `quest_question_reel`, `quest_question_answer`,
`quest_question_open`, `quest_question_publication`, `quest_question_prototype`,
`quest_question_teaching`, `quest_question_winter_count`,
`quest_question_taste_of_water`.

### 6.2 Side quests (30)

**Method (5)**
- `quest_question_method_card` — write a method card
- `quest_question_falsify` — state what would disprove it
- `quest_question_notes` — keep a real notebook
- `quest_question_uncertainty` — record uncertainty
- `quest_question_ledger` — keep a study ledger

**Experiment (5)**
- `quest_question_bench` — build the bench
- `quest_question_control_rig` — build the control
- `quest_question_repeat` — run a second trial
- `quest_question_apparatus` — improve apparatus
- `quest_question_open_data` — share raw counts

**Field (5)**
- `quest_question_plot` — lay out a plot
- `quest_question_transect` — walk a transect
- `quest_question_sample` — take clean samples
- `quest_question_season` — follow a season
- `quest_question_count_again` — recount to check

**Archive (5)**
- `quest_question_recover` — recover a reel
- `quest_question_clean` — clean and read
- `quest_question_translate` — translate an old hand
- `quest_question_provenance` — record where it came from
- `quest_question_decrypt` — finish a decryption

**Review (5)**
- `quest_question_review_hold` — hold a review
- `quest_question_replication` — replicate a claim
- `quest_question_refute` — record a refutation
- `quest_question_publish` — publish a note
- `quest_question_archive_result` — archive the outcome

**Teaching (5)**
- `quest_question_class` — teach a method class
- `quest_question_practice` — supervise a practice study
- `quest_question_apprentice_question` — guide an apprentice question
- `quest_question_reading` — read a paper together
- `quest_question_open_house` — open the bench to visitors

### 6.3 Repeatable quests (8)

`quest_question_repeat_count`, `quest_question_repeat_sample`,
`quest_question_repeat_review`, `quest_question_repeat_notes`,
`quest_question_repeat_clean`, `quest_question_repeat_teach`,
`quest_question_repeat_prototype`, `quest_question_repeat_file`.

### 6.4 Dynamic hooks

Live events (manual unlocks, research completions, archive progress, water and
crop quality, disease events, dose findings, weather seasons, world changes)
attach authored follow-ups through existing seams. No new event bus.

### 6.5 Constraints

- Knowledge unlock stays with `ResearchSystem`.
- Archives stay with `PrewarArchiveDecryptionSystem`.
- Study jobs stay with `LibraryStudySystem`.
- Printing stays with 30's press owner.
- Labs stay with 39 and 38; humans in studies route through the ward.
- No new save section.

---

## 7. NEW GAMEPLAY SYSTEMS

### 7.1 `InquirySystem` (new, `Ashfall.Core.Research`)

**Owns:** questions, hypotheses, programs, rankings, and discovery states.
**Consumes:** `ResearchSystem` nodes, `ResearchEligibility`, records owners.
**Data:** `question_board.json`, `research_programs.json`.
**Rules:** a question records what would falsify it; hypotheses are written
before results; a program runs one primary question at a time; closure requires
a review, not a vote.

### 7.2 `ExperimentSystem` (new, `Ashfall.Core.Research`)

**Owns:** protocols, apparatus, controls, variables, repeats, and raw counts.
**Consumes:** lab rooms (39, 38), `ShelterWorkshopSystem` (40), `Inventory`,
`NeedsSystem` for researcher fatigue. **Data:** `experiment_protocols.json`.
**Rules:** every protocol declares its control and its repeat count; raw counts
are kept and publishable; apparatus has condition; results are recorded before
interpretation.

### 7.3 `FieldStudySystem` (new, `Ashfall.Core.Research`)

**Owns:** study sites, transects, plots, samples, seasons, and field counts.
**Consumes:** `WildlifeEcosystemSystem` (Wave 5, read), weather (Wave 5),
`Field` and greenhouse owners (read), `Journey` owners for travel.
**Data:** `field_studies.json`, `specimen_catalog.json`.
**Rules:** a field study declares its site, method, and duration; counts are
repeated; seasons close studies; specimens are labeled and kept or released
honestly.

### 7.4 `ArchiveProjectSystem` (extend `PrewarArchiveDecryptionSystem`)

**Owns:** archive recovery, cleaning, translation, provenance, and reward
mapping. **Consumes:** the live decryption state, `PrewarArchiveCatalog`,
`MineralAcidSynthesisCatalog`/reagents for solvents (39), `ArchiveDeskSystem`
(Wave 4). **Data:** extends `prewar_archives.json` and adds
`archive_projects.json`. **Rules:** archives are physical objects that must be
recovered, cleaned, and read; effort and solvent are real; rewards unlock
through the live research owner.

### 7.5 `PeerReviewSystem` (new, thin, `Ashfall.Core.Research`)

**Owns:** review circles, replication requests, refutations, and publication
notes. **Consumes:** the findings ledger, `PressHostSession` (Wave 4) for
printing, `SchoolingSystem` ties. **Data:** `review_records.json`.
**Rules:** reviews are scheduled and chaired; anyone may challenge; refutations
are recorded with the same dignity as confirmations; publication is short and
careful.

### 7.6 `KnowledgeHoldingsSystem` (new, thin, `Ashfall.Core.Research`)

**Owns:** the holdings shelf: claims, confidence, sources, and open questions.
**Consumes:** research outcomes, reviews, archives, field results.
**Data:** `knowledge_holdings.json`. **Rules:** a holding records its evidence
and its uncertainty; retracted holdings remain visible as history; teaching
reads from holdings, never from rumor.

### 7.7 `PrototypeSystem` (new, `Ashfall.Core.Research`)

**Owns:** prototype records, test coupons, iterations, and field trials.
**Consumes:** `ShelterWorkshopSystem` (Wave 4/6), `CupolaFoundryEngine` (Exp
10), `ChemWorks` (Wave 6), `KnowledgeHoldingsSystem`. **Data:**
`prototype_records.json`. **Rules:** a prototype needs a finding, materials,
and a trial; failures are recorded and recycled; a proven prototype becomes a
workshop recipe through the live crafting owners.

### 7.8 Systems explicitly not added

- No second research, study, archive, press, or laboratory system.
- No human experimentation outside consent and the ward.
- No miracle cures or prophecy.
- No new currency.
- No new RNG stream.
- No new save section.

---

## 8. DATA CATALOG SPECIFICATION

All catalogs snake_case, integer `schema_version: 1`, validated and scanner-registered.

### 8.1 `question_board.json` (new)

```json
{
  "schema_version": 1,
  "questions": [
    {
      "question_id": "question_water_taste",
      "display_name": "Why does the water taste wrong?",
      "hypotheses": ["hypothesis_pipe", "hypothesis_source", "hypothesis_storage"],
      "falsify_if": "the taste persists with a new pipe and a new source",
      "owner": "vera_sol",
      "status": "open",
      "tags": ["water", "daily"]
    }
  ]
}
```

### 8.2 `research_programs.json` (new)

Programs: theme, questions, resources, staff, start, close.

### 8.3 `experiment_protocols.json` (new)

Protocols: question, variable, control, repeats, apparatus, duration, result.

### 8.4 `field_studies.json` (new)

Studies: site, method, transects, season, counts, close, findings.

### 8.5 `specimen_catalog.json` (new)

Specimens: label, origin, keeper, use, release, record.

### 8.6 `archive_projects.json` (new)

Projects: archive, status, solvent, researcher, effort, reward, provenance.

### 8.7 `review_records.json` (new)

Reviews: claim, chair, attendees, challenge, replication, outcome, note.

### 8.8 `knowledge_holdings.json` (new)

Holdings: claim, confidence, sources, uncertainty, status, teacher.

### 8.9 `prototype_records.json` (new)

Prototypes: finding, materials, trial, result, iteration, recipe link.

### 8.10 `method_cards.json` (new)

Method cards: name, steps, fallacy it prevents, teaching room, example.

### 8.11 Items

New items appended to `items.json`: `item_study_notebook`,
`item_question_card`, `item_sample_jar`, `item_slide_set`, `item_survey_stakes`,
`item_count_clicker`, `item_control_rig`, `item_test_coupon`,
`item_archive_solvent`, `item_reading_lamp_inquiry`, `item_prototype_frame`,
`item_field_press`, `item_chalk_mark_set`, `item_published_note`,
`item_reference_shelf`, `item_writing_slant`.

---

## 9. SAVE, DETERMINISM, AND PERSISTENCE

### 9.1 Ownership

`ResearchState` and `PrewarArchiveDecryptionState` remain the live save owners.
New sub-objects (questions, protocols, studies, specimens, archive projects,
reviews, holdings, prototypes, method cards) are additive inside them. No new
save section.

### 9.2 State to persist

- Questions, hypotheses, and status.
- Programs and resource assignments.
- Experiment protocols, raw counts, and repeats.
- Field sites, transects, seasons, and counts.
- Specimens and their disposition.
- Archive progress, solvent use, and provenance.
- Reviews, challenges, and refutations.
- Holdings, confidence, and retractions.
- Prototypes and recipe links.

### 9.3 Determinism

- Knowledge unlocks and days-to-complete remain in the live research system.
- Experiment results derive from protocol, apparatus, controls, and recorded
  counts, not from unseeded chance.
- Field counts derive from world state, season, and method; any variation uses
  the live seeded paths only where already used.
- Paired replay hashes must match; no wall-clock or `System.Random`.

### 9.4 Migration

Legacy saves load with unlocks, active research, completed ids, blueprint
progress, and archive state untouched; no questions, protocols, or holdings
exist until started. The 62 legacy knowledge nodes and existing archives keep
working; new content is additive.

### 9.5 Checksum

Invariant-culture floats; integer day, count, and effort fields.

---

## 10. UI, ACCESSIBILITY, AND PRESENTATION

### 10.1 Surfaces

| Surface | Purpose | Owner |
|---|---|---|
| `QuestionPanel` (new) | Questions and hypotheses | `InquiryHostSession` |
| `ExperimentPanel` (new) | Protocols, controls, counts | same |
| `FieldPanel` (new) | Sites, transects, seasons | same |
| `ArchivePanel` (new) | Recovery, cleaning, reading | same |
| `ReviewPanel` (new) | Circles and outcomes | same |
| `HoldingsPanel` (new) | Claims and confidence | same |
| `PrototypePanel` (new) | Findings into objects | same |

### 10.2 Accessibility and honesty

- Panels show live state and expose existing commands only.
- Counts and uncertainty are displayed as numbers with plain-language notes.
- No hidden probability; every result shows its method and repeat count.
- Keyboard/controller close/back preserved; focus maintained on refresh.
- Text alternatives for plots and diagrams; nothing depends on color alone.
- Refutations are shown as ordinary, not as failures.

### 10.3 Presentation

Audio cues appended to `audio_cues.json`: a pencil on paper, a jar lid, a
ticking counter, a lamp wick, a chair scraping in the circle, a press setting
type. No cue is required; text carries meaning.

---

## 11. INTEGRATION SEAMS

| Existing system | Attachment |
|---|---|
| `ResearchSystem` | Knowledge unlocks and capability |
| `ResearchKnowledgeDef` | Nodes and prerequisites |
| `ResearchEligibility` | Blockers and conflicts |
| `PrewarArchiveDecryptionSystem` | Archive projects |
| `PrewarArchiveCatalog` | Archive content |
| `LibraryStudySystem` (Wave 4) | Study seam and manuals |
| `ShelterWorkshopSystem` (Wave 4/6) | Prototypes and rigs |
| `ChemWorks` (Wave 6) | Solvents and apparatus |
| `MedicalWardSystem` (Wave 6) | Consent and clinical studies |
| `WildlifeEcosystemSystem` (Wave 5) | Field observation |
| `WeatherSystem` (Wave 5) | Seasons and study windows |
| `PressHostSession` (Wave 4) | Publication |
| `ArchiveDeskSystem` (Wave 4) | Records and provenance |
| `StandingRecord` (Exp 03) | Filed facts |
| `WatchHouseHostSession` (Wave 5) | Field safety and escorts |
| `EpilogueChronicleBuilder` | Inquiry milestones |

---

## 12. TECHNICAL IMPLEMENTATION PLAN

### 12.1 Phase order

**Phase 0 — Premise re-audit.** Confirm research, archive, study, press, lab,
ward, and world owners. Record file:line; change nothing.

**Phase 1 — Data + validators.** Author the ten catalogs; extend archive
content; append items; register validators and scanner.

**Phase 2 — Pure Core.** `InquirySystem`, `ExperimentSystem`,
`FieldStudySystem`, `ArchiveProjectSystem`, `PeerReviewSystem`,
`KnowledgeHoldingsSystem`, `PrototypeSystem`.

**Phase 3 — Persistence.** Additive sub-objects, migration, round-trip,
determinism.

**Phase 4 — Host + CLI.** `InquiryHostSession`, selftest coverage, fresh journey
from the taste to the method kept.

**Phase 5 — UI.** New panels with lifecycle and accessibility.

**Phase 6 — Content.** Rooms, locations, NPCs, quests, items, prose, audio.

**Phase 7 — Balance.** 360-day soak: water question, refuted remedy, archive
reel, winter count, first prototype.

**Phase 8 — Verification and closeout.**

### 12.2 Content volume

| Content | Count |
|---|---|
| Questions | 24 |
| Programs | 8 |
| Experiment protocols | 20 |
| Field studies | 12 |
| Specimens | 24 |
| Archive projects | 12 |
| Reviews | 16 |
| Holdings | 30 |
| Prototypes | 14 |
| Method cards | 12 |
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
| Tech-tree hollowing | High | Method-first design |
| Second research system | Critical | Extend live owner |
| Human harm studies | Critical | Consent and ward routing |
| Miracle results | High | Replication rules |
| Genius framing | Medium | Social, slow inquiry |
| Record destruction | Medium | Refutation preserved |
| Determinism break | Low | Live seeded paths |
| Content overrun | Medium | Budget §13 |

---

## 13. APPENDIX A — AUTHORING BUDGET

| File | Rows | Est. words |
|---|---|---|
| `question_board.json` | 24 | 5,000 |
| `research_programs.json` | 8 | 2,000 |
| `experiment_protocols.json` | 20 | 5,000 |
| `field_studies.json` | 12 | 3,000 |
| `specimen_catalog.json` | 24 | 3,500 |
| `archive_projects.json` | 12 | 3,000 |
| `review_records.json` | 16 | 3,500 |
| `knowledge_holdings.json` | 30 | 5,000 |
| `prototype_records.json` | 14 | 3,500 |
| `method_cards.json` | 12 | 3,000 |
| Quest objectives | 53 quests | 15,000 |
| NPC prose | 8 NPCs | 7,000 |
| Location prose | 10 | 3,500 |
| Item descriptions | 16 | 2,500 |
| Ending prose | 6 | 3,000 |
| **Total** | | **~66,500** |

---

## 14. APPENDIX B — RISK REGISTER

| ID | Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|---|
| R43-1 | Tech-tree hollowing | High | High | Method content |
| R43-2 | Second research system | Low | Critical | Live owner |
| R43-3 | Harmful studies | Low | Critical | Consent routing |
| R43-4 | Miracle results | Med | High | Replication |
| R43-5 | Genius framing | Med | Med | Social inquiry |
| R43-6 | Refutation as shame | Med | High | Dignity rule |
| R43-7 | Hidden probabilities | Med | Med | Open method |
| R43-8 | Determinism | Low | High | Live paths |
| R43-9 | Content overrun | Med | Med | Budget §13 |
| R43-10 | Archive overlap with 20 | Med | Med | Boundary §0.1 |

---

## 15. APPENDIX C — OPEN DECISIONS REQUIRING A FOREMAN SIGNATURE

1. **Can questions ever stay open forever?** Recommended: yes; open questions
   are recorded honestly and revisited by later cohorts.
2. **Do failed experiments waste resources?** Recommended: yes, modestly, and
   they always leave a recorded lesson.
3. **Who may propose a question?** Recommended: anyone, including apprentices,
   with the board ranking debated openly.
4. **Are holdings ever retracted publicly?** Recommended: yes, kept visible with
   the reason, never deleted.
5. **Do studies with people require ward approval?** Recommended: yes,
   unconditionally, with consent recorded.

---

## 17. APPENDIX D — QUESTION BOARD TABLE (24 QUESTIONS)

| # | Question | Category | Falsify if | Owner |
|---|---|---|---|---|
| 1 | Why does the water taste wrong? | water | pipe and source replaced | Vera |
| 2 | Which seed stores best? | food | storage order unchanged | Silla |
| 3 | Does the south plot yield more? | field | control plot matches | Silla |
| 4 | What makes the flour dark? | mill | stone dressed and unchanged | Orth tie |
| 5 | Does the ward soap reduce fevers? | care | ward comparison flat | Anil tie |
| 6 | Which fuel burns cleanest? | energy | smoke readings equal | Gren tie |
| 7 | Where do the rats enter? | stores | all entries blocked | Silla |
| 8 | Is the spring safe in spring? | water | seasonal samples clean | Vera |
| 9 | Does music help night shifts? | culture | rota outcomes same | Lune |
| 10 | Which timber dries without splitting? | build | drying outcomes equal | Burl tie |
| 11 | Does the vent hum cause broken sleep? | rest | sealed room unchanged | Ines tie |
| 12 | What killed the orchard row? | field | soil tests clean | Silla |
| 13 | Can brine be re-used? | chemistry | yields drop | Dala tie |
| 14 | Which lamp oil smokes least? | light | smoke readings equal | Vesh tie |
| 15 | Does the archive reel describe this valley? | history | landmarks do not match | Oren |
| 16 | Which mortar holds in frost? | build | frost test equal | Tol tie |
| 17 | Why do some wounds heal slowly? | care | feeding and rest controlled | Wynn tie |
| 18 | Does the high shelf cool better? | stores | temperatures equal | Lune |
| 19 | What does the gray lichen indicate? | field | transect counts equal | Silla |
| 20 | Which dusts the lungs most? | air | mask and flow equal | Anil tie |
| 21 | Can the river be re-stocked? | wild | counts do not rise | Essa tie |
| 22 | Does the old road flood first? | road | the new road floods first | Dero tie |
| 23 | What map symbols did the old survey use? | archive | translation fails | Oren |
| 24 | How much power does the works really need? | energy | metered demand differs | Reiska tie |

Twenty-four questions give the shelter a working agenda for years. Each row has
a falsification condition written before the work begins, which is the
expansion's entire thesis expressed as a table.

---

## 18. APPENDIX E — EXPERIMENT PROTOCOL TABLE (20 PROTOCOLS)

| # | Protocol | Variable | Control | Repeats | Apparatus |
|---|---|---|---|---|---|
| 1 | Water taste | pipe vs. source | old pipe, new source | 3 | jars, notes |
| 2 | Storage seed | jar type | old jar | 2 | scale, labels |
| 3 | Plot yield | plot position | center plot | 3 | scale, stakes |
| 4 | Flour color | stone dressing | worn stone | 2 | samples |
| 5 | Soap and fever | soap use | no-soap ward | 3 | ward tie |
| 6 | Fuel smoke | fuel type | dry wood | 3 | smoke card |
| 7 | Rat entry | blocked hole | open hole | 4 | flour trail |
| 8 | Spring season | month | bottled control | 12 | sterile jars |
| 9 | Music shift | music on | music off | 4 | roster tie |
| 10 | Timber dry | stack method | old stack | 3 | moisture feel |
| 11 | Vent sleep | sealed room | open room | 4 | rest logs |
| 12 | Orchard soil | soil sample | healthy row | 2 | test bed |
| 13 | Brine reuse | reuse count | fresh brine | 3 | titration |
| 14 | Lamp smoke | oil type | tallow | 3 | smoke card |
| 15 | Reel match | landmarks | modern map | 2 | translation |
| 16 | Mortar frost | mix ratio | old mix | 3 | frost box |
| 17 | Wound healing | feeding plan | standard diet | 4 | ward tie |
| 18 | Shelf cool | position | floor level | 2 | thermometer |
| 19 | Lichen meaning | transect | bare transect | 3 | count sheet |
| 20 | Dust lungs | mask use | no mask | 3 | clinic tie |

Twenty protocols, each with a variable, a control, a repeat count, and an
apparatus. The table is the bench's cookbook and the review circle's evidence
shelf at once.

---

## 19. APPENDIX F — FIELD STUDY TABLE (12 STUDIES)

| # | Study | Site | Method | Season | Output |
|---|---|---|---|---|---|
| 1 | Spring water | sample spring | samples | four seasons | water holding |
| 2 | Seed store | store room | count and weigh | winter | storage rules |
| 3 | South plot | field plot | paired plots | one year | yield finding |
| 4 | Orchard row | orchard | soil and bark | spring | cause or open |
| 5 | River fish | river | counts | two seasons | restock plan |
| 6 | Lichen belt | transect line | quadrat counts | summer | indicator holding |
| 7 | Dust fall | three roofs | collectors | ash season | air finding |
| 8 | Road flood | two roads | flood marks | spring | road rule |
| 9 | Ruin decay | study ruin | survey marks | year | decay rate |
| 10 | Night sounds | quiet yard | listening | autumn | rest finding |
| 11 | Snow pack | observation hill | stakes | winter | water forecast |
| 12 | Wildlife return | far boundary | track counts | year | migration note |

Field studies are the shelter's slow work, and the table gives every study a
site, a method, and a season. The wildlife and ruin rows connect the inquiry to
the broader world without owning any of it.

---

## 20. APPENDIX G — SPECIMEN TABLE (24 SPECIMENS)

| # | Specimen | Origin | Keeper | Use | Disposition |
|---|---|---|---|---|---|
| 1 | Water sample A | spring | Vera | taste test | archive |
| 2 | Water sample B | pipe end | Vera | compare | archive |
| 3 | Soil core | south plot | Silla | growth | archive |
| 4 | Soil core | orchard | Silla | cause | archive |
| 5 | Seed lot | store | Lune | storage | planted |
| 6 | Flour sample | mill | Lune | color | archive |
| 7 | Soap bar | works | Anil | ward | used |
| 8 | Smoke card | kitchen | Vesh | fuel | archive |
| 9 | Rat track | store | Silla | entry | archive |
| 10 | Rust scrap | rail yard | Dero | road | archive |
| 11 | Lichen | transect | Silla | indicator | archive |
| 12 | Dust jar | roof | Anil | air | archive |
| 13 | Timber offcut | yard | Burl | drying | used |
| 14 | Mortar cube | kiln | Tol | frost | archive |
| 15 | Brine jar | works | Dala | reuse | archive |
| 16 | Lamp oil | stores | Vesh | smoke | used |
| 17 | Reel fragment | archive | Oren | translation | archive |
| 18 | Old map | archive | Oren | symbols | archive |
| 19 | Bone shard | ruin | Silla | context | archive |
| 20 | Feather | boundary | Essa | species | released |
| 21 | Snow core | hill | Silla | pack | archive |
| 22 | Track cast | mud | Essa | wildlife | archive |
| 23 | Wound swab | ward | Anil | healing | destroyed |
| 24 | Ash layer | roof | Anil | fall | archive |

Specimens are kept, used, released, or destroyed with intention. The
disposition column is part of the method: a shelter that keeps everything is a
hoard, and one that keeps nothing cannot check its own work.

---

## 21. APPENDIX H — ARCHIVE PROJECT TABLE (12 PROJECTS)

| # | Project | Encryption | Solvent | Effort | Reward |
|---|---|---|---|---|---|
| 1 | Survey reels | survey_code | 2 | 120 | map symbols |
| 2 | Water ledger | municipal | 2 | 90 | quality rules |
| 3 | Seed index | agricultural | 1 | 80 | storage |
| 4 | Clinical notes | medical | 3 | 150 | care holding |
| 5 | Foundry recipes | industrial | 3 | 160 | alloy note |
| 6 | Rail timetable | logistics | 2 | 110 | schedule |
| 7 | Radio codebook | signals | 4 | 200 | frequency list |
| 8 | Weather ledger | meteorological | 2 | 100 | forecast method |
| 9 | Moss survey | ecological | 1 | 70 | indicator list |
| 10 | Shelter plan | engineering | 3 | 140 | build note |
| 11 | Ration tables | civil | 2 | 90 | diet holding |
| 12 | Personal letters | none | 0 | 20 | context |

Twelve archives give the inquiry years of patient work. The personal letters
row is included deliberately: not everything recovered is a technical treasure,
and the shelter's handling of ordinary lives matters as much as its handling of
formulas.

---

## 22. APPENDIX I — REVIEW TABLE (16 REVIEWS)

| # | Review | Claim | Challenge | Outcome |
|---|---|---|---|---|
| 1 | Circle one | water theory | pipe counter-test | refuted |
| 2 | Circle two | source theory | repeat sample | supported |
| 3 | Circle three | seed jar claim | second winter | supported |
| 4 | Circle four | plot claim | paired plot | weak |
| 5 | Circle five | soap claim | ward comparison | open |
| 6 | Circle six | fuel claim | smoke cards | supported |
| 7 | Circle seven | rat claim | blocked holes | supported |
| 8 | Circle eight | music claim | rota records | refuted |
| 9 | Circle nine | timber claim | third stack | supported |
| 10 | Circle ten | vent claim | sealed room | supported |
| 11 | Circle eleven | lichen claim | bare transect | open |
| 12 | Circle twelve | reel claim | landmark check | partly true |
| 13 | Circle thirteen | mortar claim | frost box | supported |
| 14 | Circle fourteen | dust claim | masks | weak |
| 15 | Circle fifteen | store cooling | thermometer | supported |
| 16 | Year review | holdings shelf | open questions | honest |

Sixteen reviews, five different outcomes, and only one word that matters: every
review is recorded. The circle's table is the shelter's answer to the fear of
being wrong, and it teaches that partial and open results are normal.

---

## 23. APPENDIX J — HOLDINGS TABLE (30 HOLDINGS)

| # | Holding | Confidence | Source | Status |
|---|---|---|---|---|
| 1 | Boiling purifies | high | manual | taught |
| 2 | Source water varies by season | medium | field study | taught |
| 3 | The pipe adds taste | high | experiment | taught |
| 4 | Dry seed stores best | high | two winters | taught |
| 5 | The south plot yields more | low | one year | open |
| 6 | Soap use and fevers unclear | low | ward note | open |
| 7 | Dry fuel smokes less | high | smoke cards | taught |
| 8 | Rats enter at the sill | high | tracks | fixed |
| 9 | Music and shift outcomes unclear | low | rota | retracted |
| 10 | Timber dries in thin stacks | high | three stacks | taught |
| 11 | Vent hum harms sleep | high | rest logs | fixed |
| 12 | Orchard row cause unknown | low | soil | open |
| 13 | Brine re-use limited | medium | titration | taught |
| 14 | Lamp oil varies by source | medium | smoking card | taught |
| 15 | The reel describes this valley | medium | landmarks | partly true |
| 16 | Frost mortar mix | high | frost box | taught |
| 17 | Feeding aids healing | medium | ward | taught |
| 18 | High shelf cools better | high | thermometer | taught |
| 19 | Lichen meaning uncertain | low | transect | open |
| 20 | Dust masks help | medium | clinic | taught |
| 21 | River restock uncertain | low | counts | open |
| 22 | Old road floods first | high | flood marks | taught |
| 23 | Map symbols partly read | medium | translation | open |
| 24 | Works demand metered | high | meter | taught |
| 25 | Ice keeps meat longer | high | kitchen | taught |
| 26 | Ash season closes the road | high | logs | taught |
| 27 | The ward's hand soap works | medium | practice | taught |
| 28 | The south wind precedes a front | medium | station | taught |
| 29 | Newcomers carry new skills | medium | interviews | taught |
| 30 | The valley supports a second outpost | low | survey | open |

Thirty holdings, each with a confidence level and a source. The table is the
shelter's intellectual balance sheet, and the low-confidence rows are as
important as the taught ones because they show what is still being earned.

---

## 24. APPENDIX K — PROTOTYPE TABLE (14 PROTOTYPES)

| # | Prototype | Finding | Materials | Trial | Result |
|---|---|---|---|---|---|
| 1 | Filter bed | purification | charcoal, sand | taste test | adopted |
| 2 | Seed jar | storage | clay, seal | winter | adopted |
| 3 | Paired plot rig | method | stakes, string | one season | adopted |
| 4 | Smoke card | fuel | paper, frame | kitchen | adopted |
| 5 | Sill plate | rat entry | steel, wood | store | adopted |
| 6 | Quiet curtain | vent study | cloth, hooks | rest room | adopted |
| 7 | Drying rack | timber | poles, slats | yard | adopted |
| 8 | Frost mortar | build | lime, sand | box | adopted |
| 9 | Brine gauge | reuse | glass, float | works | adopted |
| 10 | Lamp wick trim | fuel | scissors, cloth | night | adopted |
| 11 | Reading lens | archive | glass, frame | desk | adopted |
| 12 | Dust mask v2 | air study | cloth, tie | works | adopted |
| 13 | Restock cage | river study | net, frame | river | testing |
| 14 | Forecast slate | weather | slate, chalk | station | adopted |

Fourteen prototypes, thirteen adopted and one still being tested, each traced
to a finding. The table is where the inquiry touches the rest of the shelter and
proves that a question can end as a better tool.

---

## 25. APPENDIX L — METHOD CARD TABLE (12 CARDS)

| # | Card | Steps | Prevents |
|---|---|---|---|
| 1 | Write first | guess, date, sign | hindsight |
| 2 | Control | compare against something | confounding |
| 3 | Repeat | run it twice | luck |
| 4 | Blind | hide the label | bias |
| 5 | Count | numbers, not feelings | impression |
| 6 | Keep raw | store the counts | editing |
| 7 | Falsify | name what proves you wrong | attachment |
| 8 | Ask another | invite challenge | isolation |
| 9 | Record refutation | write the failure | erasure |
| 10 | Replicate | have someone else run it | private truth |
| 11 | Publish short | one page, honest | vanity |
| 12 | Teach it | hand the method on | loss |

The method cards are the expansion's curriculum in twelve lines. Each card
names a fallacy it prevents, which is how the game teaches inquiry as a craft
rather than as a password to unlock technology.

---

## 26. APPENDIX M — MAIN QUESTLINE STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_question_taste` | 4 | Three theories, one supply |
| `quest_question_board` | 3 | Board hung |
| `quest_question_guess` | 4 | Hypotheses written |
| `quest_question_control` | 5 | Control rig built |
| `quest_question_count` | 4 | Third count decisive |
| `quest_question_remedy` | 5 | Remedy tested and fails |
| `quest_question_circle` | 4 | First review |
| `quest_question_reel` | 5 | Reel cleaned and read |
| `quest_question_answer` | 4 | Water cause found and fixed |
| `quest_question_open` | 3 | A question stays open |
| `quest_question_publication` | 4 | First note printed |
| `quest_question_prototype` | 4 | Finding becomes a tool |
| `quest_question_teaching` | 4 | Method taught |
| `quest_question_winter_count` | 5 | Seasonal study closes |
| `quest_question_taste_of_water` | 3 | Final disposition |

---

## 27. APPENDIX N — SIDE QUEST STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_question_method_card` | 3 | Card written |
| `quest_question_falsify` | 2 | Condition named |
| `quest_question_notes` | 3 | Notebook kept |
| `quest_question_uncertainty` | 2 | Uncertainty recorded |
| `quest_question_ledger` | 3 | Study ledger |
| `quest_question_bench` | 4 | Bench built |
| `quest_question_control_rig` | 4 | Control built |
| `quest_question_repeat` | 3 | Second trial |
| `quest_question_apparatus` | 3 | Apparatus improved |
| `quest_question_open_data` | 2 | Raw counts shared |
| `quest_question_plot` | 3 | Plot laid out |
| `quest_question_transect` | 3 | Transect walked |
| `quest_question_sample` | 3 | Samples clean |
| `quest_question_season` | 4 | Season followed |
| `quest_question_count_again` | 2 | Recount done |
| `quest_question_recover` | 4 | Reel recovered |
| `quest_question_clean` | 3 | Cleaned and read |
| `quest_question_translate` | 4 | Hand translated |
| `quest_question_provenance` | 3 | Origin recorded |
| `quest_question_decrypt` | 4 | Decryption finished |
| `quest_question_review_hold` | 3 | Review held |
| `quest_question_replication` | 4 | Claim replicated |
| `quest_question_refute` | 3 | Refutation recorded |
| `quest_question_publish` | 3 | Note published |
| `quest_question_archive_result` | 3 | Result archived |
| `quest_question_class` | 3 | Class taught |
| `quest_question_practice` | 4 | Practice supervised |
| `quest_question_apprentice_question` | 4 | Apprentice guided |
| `quest_question_reading` | 3 | Paper read together |
| `quest_question_open_house` | 3 | Bench open |

---

## 28. APPENDIX O — NPC DOSSIERS (BRIEF)

**Vera Sol** — researcher. Arrived with a notebook and a habit of asking what
would prove her wrong. Believes method is a gift a shelter gives its future.

**Oren Bask** — archivist. Cleans reels with a soft brush and reads old hands
better than print. Treats every recovery as someone's last message.

**Kito Marr** — experimenter. Builds control rigs out of salvage and refuses to
run anything once. Believes a bench is a moral object.

**Lune** — statistician. Counts things twice and says uncertain when it is
uncertain. Believes numbers are a way of being kind to the truth.

**Hart Doon** — skeptic. Asks the uncomfortable question in the middle of a
happy meeting and is thanked later, usually. Believes doubt is respect.

**Silla** — field researcher. Walks transects in all weather and knows the
meadow by its worst days. Believes the outside world keeps its own books.

**Marn** — inventor. Turns a finding into a thing someone can hold within a
week. Believes a prototype is a finding that has agreed to work.

**Tim** — apprentice. Keeps the note cards, dates the guesses, and asks why the
control is not just another name for a second try.

---

## 29. APPENDIX P — LOCATION DETAIL

- **The Water Shed** — three theories and one bucket.
- **The Field Plot** — a square of ground divided on purpose.
- **The Transect Line** — a string through the grass, walked weekly.
- **The Sample Spring** — jars, labels, and four seasons.
- **The Study Ruin** — a wall that is failing for reasons the shelter can
  measure.
- **The Archive Tent** — trays, brushes, and a lamp that stays low.
- **The Review Yard** — an outdoor circle where disagreement is safe.
- **The Press Corner** — a small note, printed carefully.
- **The Observation Hill** — wind, stakes, and a slate.
- **The Trial Plot** — two plots side by side, and nothing else to argue about.

---

## 30. APPENDIX Q — REVIEW PROTOCOL TABLE

| Step | Action | Tone |
|---|---|---|
| Present | findings in one page | plain |
| Question | any attendee may ask | open |
| Challenge | state a counter-test | specific |
| Agree test | define what would settle it | shared |
| Record | outcome written down | honest |
| Assign | who replicates | named |
| Revisit | date set | patient |
| Publish | short note if held | careful |
| File | holdings updated | permanent |
| Thank | thank the challenger | explicit |

The review protocol is the expansion's beating heart. Its last row is the culture
expressed as etiquette: the person who asked the hardest question is thanked by
name, every time.

---

## 31. APPENDIX R — WORKED 360-DAY INQUIRY SCENARIO

**Days 1–20.** The water tastes wrong; three theories circulate; Vera writes
them on cards and pins them to a board.

**Days 21–50.** Hypotheses are dated and signed before any test; Kito builds a
control rig from a spare table and two jars.

**Days 51–80.** The first counts are taken; Lune insists on three samples per
source; the pipe theory begins to fail and is not yet abandoned.

**Days 81–110.** The first review circle is held; the pipe theory is refuted
with a counter-test, and nobody is humiliated; the finding is recorded.

**Days 111–140.** Silla lays out the south plot and the control plot; the field
season starts; the seed jar study enters its second winter.

**Days 141–170.** A trusted remedy fails its trial in the ward; the result is
published as a refutation, and the remedy keeps being used only for comfort.

**Days 171–200.** Oren cleans the first survey reel; the translation is partial;
the map-symbol question opens and stays open honestly.

**Days 201–230.** The water question closes with a real cause: a badly joined
pipe section; the pipe is replaced and the taste resolves; the holding is
recorded with high confidence.

**Days 231–260.** The first careful note is printed; the press corner publishes
four paragraphs and a chart of counts.

**Days 261–290.** Marn turns the filter finding into a bed that serves the
whole shelter; the prototype is adopted and the recipe enters the workshop.

**Days 291–320.** Method classes begin; Tim runs a supervised practice study
on lamp oils and reports honestly that his first count was wrong.

**Days 321–360.** Winter closes the field season; the annual review reads the
holdings shelf aloud; twelve questions remain open and none is a failure.

---

## 32. APPENDIX S — VIGNETTES (TONE SAMPLE)

> Vera pins the third card to the board and reads all three out loud, and then
> says the sentence that changes the shelter: what would make this false? and
> waits, and nobody has an answer ready, and that is the first honest moment in
the whole study.

> Kito sets the two jars side by side, one from the pipe and one from the
> spring, and drinks from both with a straight face, and writes the date and the
> order on the tally sheet before he says which one tastes wrong.

> Hart asks the hard question in the middle of a proud morning, and the room
goes quiet, and Lune says good, and writes it down, and the review is scheduled
for the afternoon, and the work continues as if disagreement were weather.

> Oren holds the reel up to the lamp and reads three characters and one number,
and the number is a date, and the date is older than the shelter, and he sets it
back in its tray and turns the lamp lower so it will keep.

---

## 33. APPENDIX T — REFUTATION AND DIGNITY PROTOCOL

| Step | Action |
|---|---|
| Record | Write the claim exactly as believed |
| Test | Run the counter-test agreed in review |
| Report | State the result plainly, without adjectives |
| Credit | Name the person who proposed the claim |
| Explain | Say what the failure teaches |
| Keep | File the claim in holdings as refuted |
| Thank | Thank everyone who argued in good faith |
| Move | Choose the next question without delay |

The protocol exists because the expansion's theme lives or dies here. A shelter
that punishes being wrong will only ever hear confident nonsense, and a shelter
that honors refutation can learn anything.

---

## 34. APPENDIX U — FAILURE MODES AND RECOVERY

| Failure | Effect | Recovery |
|---|---|---|
| No control | false result | rebuild protocol |
| One run only | luck | repeat twice |
| Hidden data | distrust | publish raw counts |
| Refutation hidden | wrong holdings | reopen and correct |
| Bad apparatus | noise | repair, recalibrate |
| Wrong sample | bad finding | resample cleanly |
| Reviewer absent | delay | chair alternate |
| Archive damage | loss | handle gently |
| Overclaiming | retraction | narrow the claim |
| Teacher gatekeeping | stagnation | open the classes |

Failures in inquiry are expected and cheap, which is the whole advantage of
method. Every row ends with a specific corrective practice rather than a
shame.

---

## 35. APPENDIX V — CONTENT REVIEW CHECKLIST

- [ ] No human studies outside consent and the ward exist.
- [ ] No miracle cures or prophecy exist.
- [ ] `ResearchSystem` remains the knowledge authority.
- [ ] `PrewarArchiveDecryptionSystem` remains the archive authority.
- [ ] `LibraryStudySystem` remains the study authority.
- [ ] Printing stays with 30's press owner.
- [ ] Every claim shows method and repeat count.
- [ ] Refutations are recorded, never erased.
- [ ] Uncertainty is displayed honestly.
- [ ] Save additions are additive and legacy-neutral.
- [ ] Determinism uses live seeded paths only.

---

## 36. APPENDIX W — GLOSSARY

- **Question** — what the shelter wants to know, written down.
- **Hypothesis** — a guess dated before the test.
- **Control** — the comparison that makes a result mean anything.
- **Replication** — someone else runs it and gets the same answer.
- **Falsification** — the condition that would prove the guess wrong.
- **Holding** — a claim the shelter keeps with its evidence and uncertainty.
- **Refutation** — a recorded failure, kept with dignity.
- **Provenance** — where a specimen or archive came from.
- **Review circle** — the meeting where claims are tested socially.
- **Method** — the habit that outlives every individual finding.

---

## 37. APPENDIX X — INTEGRATION MATRIX

| System | Reads | Writes | Never writes |
|---|---|---|---|
| `ResearchSystem` | nodes | unlocks | questions |
| `InquirySystem` | nodes | board, programs | unlocks |
| `ExperimentSystem` | apparatus | counts | unlocks |
| `FieldStudySystem` | world | counts | world state |
| `ArchiveProjectSystem` | archives | progress | unlocks |
| `PrewarArchiveDecryptionSystem` | efforts | progress | holdings |
| `PeerReviewSystem` | findings | reviews | unlocks |
| `KnowledgeHoldingsSystem` | evidence | holdings | unlocks |
| `PrototypeSystem` | findings | prototypes | recipes |
| `LibraryStudySystem` | manuals | study jobs | research |
| `PressHostSession` | notes | print jobs | findings |
| `MedicalWardSystem` | consent | care | research |
| `WildlifeEcosystemSystem` | counts | nothing | nothing |
| `ArchiveDeskSystem` | files | records | research |
| `EpilogueChronicleBuilder` | milestones | chronicle | findings |

---

## 38. APPENDIX Y — DATA SCHEMA DETAIL (NEW CATALOGS)

**`question_board.json`** — `question_id`, `display_name`, `hypotheses[]`,
`falsify_if`, `owner`, `status`, `tags`.

**`research_programs.json`** — `program_id`, `display_name`, `questions[]`,
`resources[]`, `staff[]`, `start`, `close`, `tags`.

**`experiment_protocols.json`** — `protocol_id`, `question`, `variable`,
`control`, `repeats`, `apparatus[]`, `duration`, `result`, `tags`.

**`field_studies.json`** — `study_id`, `site`, `method`, `transects`,
`season`, `counts[]`, `close`, `findings[]`, `tags`.

**`specimen_catalog.json`** — `specimen_id`, `origin`, `keeper`, `use`,
`disposition`, `record`, `tags`.

**`archive_projects.json`** — `project_id`, `archive_id`, `status`, `solvent`,
`researcher`, `effort`, `reward`, `provenance`, `tags`.

**`review_records.json`** — `review_id`, `claim`, `chair`, `attendees[]`,
`challenge`, `replication`, `outcome`, `note`, `tags`.

**`knowledge_holdings.json`** — `holding_id`, `claim`, `confidence`, `sources[]`,
`uncertainty`, `status`, `teacher`, `tags`.

**`prototype_records.json`** — `prototype_id`, `finding`, `materials[]`,
`trial`, `result`, `iteration`, `recipe_link`, `tags`.

**`method_cards.json`** — `card_id`, `display_name`, `steps[]`, `prevents`,
`teaching_room`, `example`, `tags`.

All new catalogs carry `schema_version: 1` and fail the integrity gate on
missing or duplicate IDs, invalid references, or out-of-range numbers.

---

## 39. APPENDIX Z — MEASUREMENT AND TELEMETRY

| Metric | Purpose | Source |
|---|---|---|
| Questions opened | activity | Board |
| Questions closed | throughput | Board |
| Replication rate | rigor | Review |
| Refutations recorded | honesty | Review |
| Field seasons completed | patience | Field |
| Archives recovered | discovery | Archive |
| Holdings with evidence | quality | Holdings |
| Prototypes adopted | impact | Prototype |
| Method classes taught | continuity | Teaching |
| Open questions at year end | honesty | Board |

Telemetry is diagnostic only; it never gates content and never becomes a score
against a researcher or a question.

---

## 40. APPENDIX AA — IMPLEMENTATION CHECKLIST

- [ ] Phase 0 premise re-audit recorded with file:line evidence.
- [ ] Phase 1 catalogs authored and registered with validators and scanner.
- [ ] Archive and knowledge catalogs extended with authored content.
- [ ] Phase 2 Core systems are pure, engine-free, and deterministic.
- [ ] Phase 3 save additions are additive, legacy-neutral, and round-trip tested.
- [ ] Research, archive, study, press, lab, and ward authorities remain untouched.
- [ ] Triad parity (Setup / Save / Flush) holds for every new state.
- [ ] Phase 4 host session exposes live state and real commands only.
- [ ] Phase 5 panels pass bind/unbind/rebind, focus, contrast, and scaling checks.
- [ ] Phase 6 content passes the review checklist in §35.
- [ ] Phase 7 soak shows the water question, a refutation, a reel, and a winter count.
- [ ] Phase 8 data integrity and content utilization selftests pass.
- [ ] No miracle, prophecy, or harmful-study content exists.

---

## 41. APPENDIX AB — OPEN QUESTIONS FOR REVIEW

1. Can the shelter refuse to answer a question for ethical reasons?
2. Do failed experiments ever consume rare materials, and how is that shown?
3. Who chairs a review when the researcher is the subject?
4. Are method classes mandatory for apprentices?
5. Should the board publish open questions to the whole shelter?
6. Do holdings ever expire, or stay until replaced?
7. Can a refuted claim be re-opened with new evidence?
8. Does the press publish everything, or does the circle hold some work back?

None of these may be decided unilaterally; each changes tone and balance.

---

## 42. APPENDIX AC — CROSS-WAVE HOOKS

| Wave | Expansion | Hook |
|---|---|---|
| 1 | 12 The Second Generation | Apprenticeship in method |
| 1 | 15 The Deep Root | Seed and soil studies |
| 1 | 16 The Rebuilt Body | Clinical inquiry with consent |
| 2 | 18 The Underneath | Deep survey questions |
| 2 | 20 The Quiet Hand | Archive boundaries |
| 2 | 21 The Grid | Demand metering |
| 3 | 22 The Clean Flow | Water quality studies |
| 3 | 23 The Alarm | Evidence after incidents |
| 3 | 26 The Common Table | Preservation trials |
| 4 | 28 The Lesson | Method classes |
| 4 | 29 The Glass | Lenses and instruments |
| 4 | 30 The Press | Publication |
| 5 | 32 The Wild | Field studies |
| 5 | 33 The Weather | Seasonal studies |
| 5 | 36 The Watch | Field safety |
| 6 | 38 The Ward | Clinical review |
| 6 | 39 The Reagent | Lab work and solvents |
| 6 | 40 The Wheel | Apparatus and rigs |
| 7 | 42 The Core | Old documentation |
| 7 | 44 The Outpost | Survey and site studies |
| 7 | 46 The Long Change | Long-term observation |

Each hook is additive. The Question can ship alone, and every other expansion
can ship without it.

---

## 43. APPENDIX AD — ENDING PROSE SKETCHES

**The Method Kept.** The board, the bench, and the circle outlive the person who
started them, and the next cohort cannot imagine a shelter without them.

**The Honest Shelf.** Every holding is defensible and every open question is
listed, and the shelter knows exactly how much it does not know.

**The Refuted Year.** A year of wrong guesses ends with better questions, a
thicker notebook, and not one person diminished by having been wrong.

**The Long Study.** A field question takes years and reports truly, and the
shelter learns that some answers are paid for in seasons.

**The Open Door.** The archive gives up something the old world wanted kept, and
the shelter decides to publish it for everyone, carefully.

**Fade.** A card on a board and a pencil beside it, and a room where the next
question is already half written.

---

## 44. APPENDIX AE — COMMON FAILURE PATTERNS TO AVOID

| Pattern | Why it is bad | Correct approach |
|---|---|---|
| Tech tree only | hollow | method-first design |
| Genius flashes | untrue | slow social inquiry |
| Miracle results | dishonest | replication |
| Hidden probability | unfair | open method |
| Refutation as shame | cruel | dignity protocol |
| Erased failures | dishonest | holdings keep history |
| Human harm studies | harmful | consent and ward |
| Secret inventions | stagnant | open classes |
| Certainty machine | false | uncertainty shown |
| Answer for everything | hollow | open questions kept |

The list exists because research systems turn into vending machines very
easily. The expansion's rule is that the shelter earns its answers with method,
and that the method matters more than any single answer.

---

## 45. APPENDIX AF — CONTENT VOLUME SUMMARY

| Category | Rows | Prose estimate |
|---|---|---|
| Questions | 24 | 5,000 |
| Programs | 8 | 2,000 |
| Protocols | 20 | 5,000 |
| Field studies | 12 | 3,000 |
| Specimens | 24 | 3,500 |
| Archive projects | 12 | 3,000 |
| Reviews | 16 | 3,500 |
| Holdings | 30 | 5,000 |
| Prototypes | 14 | 3,500 |
| Method cards | 12 | 3,000 |
| Quests | 53 | 15,000 |
| NPCs | 8 | 7,000 |
| Locations | 10 | 3,500 |
| Items | 16 | 2,500 |
| Endings | 6 | 3,000 |
| **Total** | | **~66,500** |

---

## 46. APPENDIX AG — FIRST YEAR OF THE QUESTION

| Month | Focus | Milestone |
|---|---|---|
| 1 | Taste | three theories |
| 2 | Board | questions ranked |
| 3 | Guess | hypotheses dated |
| 4 | Control | rig built |
| 5 | Count | three samples |
| 6 | Circle | first review |
| 7 | Remedy | refutation recorded |
| 8 | Reel | archive read |
| 9 | Answer | pipe cause fixed |
| 10 | Publication | first note |
| 11 | Prototype | filter adopted |
| 12 | Teaching | classes begin |

A year of the question is a year of learning how to be wrong safely, and the
shelter ends it with a method it can hand to anyone.

---

## 47. APPENDIX AH — INQUIRY COVENANT

| Clause | Promise |
|---|---|
| Write first | Guesses are dated before tests |
| Control | Every study has a comparison |
| Repeat | Nothing is decided on one run |
| Raw data | Counts are kept and shareable |
| Consent | People are never studied without it |
| Refute openly | Failure is recorded, never buried |
| Show uncertainty | Confidence is stated, not implied |
| Keep history | Refuted claims remain on the shelf |
| Teach method | The habit is passed on deliberately |
| Stay curious | Open questions are not failures |

The covenant is the expansion's first-class design object. A shelter that can
say what it does not know is a shelter that can be trusted with what it does.

---

## 48. APPENDIX AI — QUESTION LIFECYCLE TABLE

| Stage | Meaning | Exit |
|---|---|---|
| Proposed | someone asked | ranked |
| Ranked | board orders it | program |
| Framed | falsification written | protocol |
| Running | study active | result |
| Reported | counts complete | review |
| Reviewed | challenged | replicate |
| Replicated | confirmed by another | holding |
| Refuted | disproved | holding |
| Open | unresolved | revisit |
| Taught | entered method class | done |

A question's life is longer than a study and sometimes longer than a person.
The lifecycle table is how the shelter keeps faith with both the work and the
people who started it.

---

## 49. APPENDIX AJ — TEACHING PLAN TABLE

| Lesson | Audience | Frequency | Practice | Assessment |
|---|---|---|---|---|
| Write first | all | monthly | date a guess | notebook check |
| Control | apprentices | monthly | build a rig | rig review |
| Repeat | apprentices | monthly | run twice | tally check |
| Blind | researchers | quarterly | label swap | review test |
| Count | all | monthly | tally a jar | count sheet |
| Keep raw | all | monthly | file counts | archive check |
| Falsify | all | monthly | write a condition | board check |
| Ask another | all | weekly | invite a challenge | circle record |
| Record refutation | all | per event | write the failure | holdings check |
| Replicate | researchers | per claim | second run | review record |
| Publish short | researchers | per finding | one page | press file |
| Teach it | senior | per cohort | run a class | attendance |

Twelve lessons, one per method card, taught in rotation. The assessment column
is deliberately practical: the shelter tests method by asking people to do it,
not by asking them to recite it.

---

## 50. APPENDIX AK — STUDY BUDGET TABLE

| Study type | People | Days | Materials | Review |
|---|---|---|---|---|
| Bench trial | 2 | 3–10 | small | one circle |
| Field season | 3 | 60–120 | stakes, jars | two circles |
| Archive project | 1–2 | 20–60 | solvent, lamp | one circle |
| Clinical observation | 2–4 | 30–90 | ward tie | ward review |
| Prototype trial | 2 | 10–30 | workshop parts | field test |
| Long survey | 3 | 365 | stakes, slates | annual review |

A study budget is time, people, and materials, and the table makes the shelter's
choices visible. The clinical row is lowest only because it carries the highest
obligations, which is exactly the right ordering for a settlement that wants to
remain decent.

---

## 51. APPENDIX AL — READING CIRCLE TABLE

| Reading | Text | Question asked | Attends |
|---|---|---|---|
| First | water holding | what is missing? | all |
| Second | refutation | what changed our mind? | circle |
| Third | field counts | where is the uncertainty? | circle |
| Fourth | archive notes | what did we assume? | circle |
| Fifth | method cards | which card did we skip? | students |
| Year | holdings shelf | what do we not know? | all |

A reading circle meets around a single page and one question, which is how the
shelter keeps inquiry social. The last row is the most important: the year
reading is about ignorance, and it is conducted with the same seriousness as
any celebration.

---

## 52. CLOSING STATEMENT

ASHFALL already has a knowledge tree with 62 nodes, a research system with
eligibility and unlocks, blueprint progress, and a small archive of prewar
records. What it lacks is method: questions, hypotheses, controls, repeats,
field counts, review, refutation, holdings, and the habit of teaching doubt as
respect. The Question adds that practice without adding a second research
system or a single miracle. It adds a card on a board, a control rig, a jar of
water, a review circle where being wrong is safe, and a shelter that knows what
it knows.

> Wave 7 note: this plan is one of five Wave 7 expansion bibles (42–46). Each is
> self-contained; none requires another to ship. The shared Wave 7 index lives
> at `docs/expansions/wave7/WAVE7_INDEX.md`. The safe pre-signature step is
> Phase 1 (data schemas and validators), which is additive and reversible.
> Evidence anchors: `ResearchSystem` (`State`, `Catalog`, `OnResearchCompleted`,
> `OnManualUnlocked`, `Register`, `UnlockManual`, `IsManualUnlocked`,
> `HasCapability`, `GetEligibility`, `StartResearch`, `GetDaysRemaining`,
> `GetAvailableNodes`), `ResearchKnowledgeDef` (`id`, `displayName`, `category`,
> `prerequisites`, `breakthroughItem`, `daysToComplete`), `ResearchState`
> (`unlockedIds`, `activeResearchId`, `activeResearchDays`, `completedIds`,
> `researchPointsAvailable`, `blueprintProgress`), `ResearchEligibility`
> (`CanStart`, `MissingPrerequisites`, `ConflictingActiveResearchId`, `Code`),
> `PrewarArchiveProject` (`ArchiveId`, `Status`, `Progress`, `TargetProgress`,
> `AssignedResearcherId`, `HasSolventApplied`), `prewar_archives.json`
> (4,072 B; `encryption_grade`, `required_room`, `cleaning_solvent_id`,
> `base_effort_points`, `reward_research_ids`), and `research_knowledge.json`
> (21,516 B, 62 nodes).