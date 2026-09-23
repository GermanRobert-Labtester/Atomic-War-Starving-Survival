# ASHFALL — Expansion 28 Design Bible
# THE LESSON
### Wave 4 · Education, Literacy, Apprenticeship, Manuals, Curriculum, and the Transmission of Knowledge

**Document status:** Design plan (pre-integration). Not a claim. Not an authorization.
**Date:** 2026-09-21
**Domain owners touched:** `Ashfall.Core` (ApprenticeshipSystem), `Ashfall.Core` (LibraryStudySystem), `Ashfall.Core.Narrative` (ArchiveDeskSystem)
**Proposed host owner:** `SchoolhouseHostSession` (extends apprenticeship and library hosts)
**Existing save sections:** `apprenticeship`, `library_study`
**Existing CLI verbs:** `--apprenticeship-selftest`, `--library-study-selftest`, `--data-integrity-selftest`
**Rule compliance:** Godot authoritative; Core engine-free; JSON data authoritative; one authority per concern; deterministic seeded RNG.

---

## 0. HOW TO READ THIS DOCUMENT

This is a **design bible**, not an integration plan. Implementation must later pass
through `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `TEST_POLICY.md`.

ASHFALL already owns two halves of learning. `ApprenticeshipSystem` registers
mentorship pairs (`MentorshipDef`, `MentorshipCatalog`), tracks apprenticeships
with progress, supports transcription tasks and a `SurvivorWill`, caps concurrent
pairs (`MaxConcurrentPairs = 3`), handles mentor death through
`NotifyMentorDeath`, ticks daily, and restores from `ApprenticeshipState`. It has
a host session, a save store, and a panel. `LibraryStudySystem` loads
`ManualDefinition` catalogs, runs study jobs with comprehension rates, effective
study hours, estimated days, manual power requirements (`IsManualPowered`),
completion tracking, daily ticks, and state restore; it also has a host session
and a panel. `LibraryManualCatalogLoader` feeds `library_manuals.json` (21.9 KB);
`lost_tech_manuals.json` (23.5 KB) holds hundreds of recoverable documents;
`education_session_records.json` (32 KB) holds narrative lesson records.
`apprenticeship_catalog.json` is only 1.7 KB — three mentorship rows at most.

What does not exist: a school for children, literacy levels and instruction, a
curriculum, group lessons, examinations and certification, and a manual-writing
path that turns a living master's knowledge into a document before the master
dies.

**The Lesson** turns the two live systems into a full transmission loop: children
learn to read, adults apprentice to trades, masters write down what they know,
and the shelter decides what knowledge it will keep.

Conventions: **`LIVE`** confirmed in source/data; **`GAP`** confirmed thin;
**`PROPOSED`** new.

---

## 1. EXECUTIVE SUMMARY

### 1.1 Pitch

A shelter can survive a winter on stored food. It survives a decade on stored
knowledge — and knowledge is the one store that only grows when it is used.

**The Lesson** is the expansion about teaching: a schoolhouse for the children,
literacy levels for adults who never learned, apprenticeship tracks with real
masters, a curriculum that changes with the shelter's needs, examinations that
mean something, and manuals that preserve expertise after the expert is gone.

The expansion's hard rules follow the live owners: `ApprenticeshipSystem`
remains the mentorship authority, `LibraryStudySystem` remains the study-job
authority, `SkillProgressionSystem` remains the only skill authority, and all
knowledge state is additive inside the existing apprenticeship and library save
sections. No second school, skill, or manual system is created.

### 1.2 The five loops it adds

```
   Children ──► Literacy ──► Curriculum ──► Graduates
                    │            │
                    ▼            ▼
                 Lessons      Tracks ──► Apprenticeship ──► Trades
                    │            │              │
                    ▼            ▼              ▼
                 Teachers     Exams        Certification
                    │                           │
                    ▼                           ▼
              Manuals ◄── Scribes ◄── Masters ──► Legacy
                    │
                    ▼
              LibraryStudy ──► Comprehension ──► Applied work
```

### 1.3 What the player manages

1. **Literacy.** Reading and writing levels; who can sign their name, read a
   label, or follow a manual.
2. **Children's schooling.** Classes, attendance, age bands, and the teacher.
3. **Curriculum.** What is taught this season: reading, numbers, trades, health,
   civics, or survival.
4. **Apprenticeship.** Mentor pairs, trade tracks, and progress to journeyman.
5. **Examinations.** Who is ready, who certifies, and what certification unlocks.
6. **Manuals.** Study, transcription, authoring, copying, and loss.
7. **Legacy.** What a master leaves behind; what the shelter chooses to keep.

### 1.4 What it is not

- Not a second skill or XP system. `SkillProgressionSystem` and the live
  apprenticeship progress remain the owners.
- Not a second library. `LibraryStudySystem` remains the study authority.
- Not a school simulator with grades as content gates; education unlocks
  capability, not cosmetic tiering.
- Not a research-tree reskin. Knowledge is authored, taught, written, and read.
- Not a new save section.

---

## 2. EVIDENCE BASE AND GAP ANALYSIS

### 2.1 Live systems (verified)

| File | Role | Status |
|---|---|---|
| `Assets/Ashfall.Core/ApprenticeshipSystem.cs` | Mentorship pairs, progress, mentor death, wills | `LIVE` |
| `Assets/Ashfall.Core/LibraryStudySystem.cs` | Manual study jobs, comprehension, completion | `LIVE` |
| `Assets/Ashfall.Core/Narrative/ArchiveDeskSystem.cs` | Archive records | `LIVE` |
| `Assets/Ashfall.Core/Narrative/ArchiveInkCatalogLoader.cs` | Ink and scribing | `LIVE` |
| `src/Host/ApprenticeshipHostSession.cs` | Host session | `LIVE` |
| `src/Host/ApprenticeshipSaveStore.cs` | Save store | `LIVE` |
| `src/Host/LibraryStudyHostSession.cs` | Host session | `LIVE` |
| `src/UI/ApprenticeshipPanel.cs` | Panel | `LIVE` |
| `src/UI/LibraryStudyPanel.cs` | Panel | `LIVE` |
| `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` | Sole skill authority | `LIVE` |

### 2.2 Live data (counted)

| Catalog | Size | Notes |
|---|---|---|
| `apprenticeship_catalog.json` | **1.7 KB** | very thin mentorship list |
| `library_manuals.json` | 21.9 KB | manual definitions |
| `lost_tech_manuals.json` | 23.5 KB | recoverable technical documents |
| `education_session_records.json` | 32 KB | narrative lesson records |
| `bunker_blueprints_codex.json` | 22.7 KB | adjacent instructional content |
| Curriculum / literacy / exam data | none | confirmed absent |

### 2.3 Confirmed gaps

- **GAP-28-1 — No children's schooling.** No classes, attendance, or age bands.
- **GAP-28-2 — No literacy system.** Reading and writing are not modeled.
- **GAP-28-3 — No curriculum.** No authored season of teaching.
- **GAP-28-4 — No examinations or certification.** Apprenticeship has progress;
  nothing marks readiness.
- **GAP-28-5 — Mentorship content is 1.7 KB.** Three pairs at most for a whole
  shelter economy.
- **GAP-28-6 — No manual authoring.** Transcription exists as a task type;
  nothing turns experience into a new manual.
- **GAP-28-7 — No teaching roles or schedule.** No teacher, no class day.
- **GAP-28-8 — No knowledge loss pressure.** Masters can die
  (`NotifyMentorDeath`); what dies with them is not authored.
- **GAP-28-9 — No education items.** No slates, chalk, primers, or ink path in
  gameplay.

### 2.4 Non-duplication statement

This expansion will **not** add a second apprenticeship, library, skill, XP, or
save system. It extends `ApprenticeshipSystem` with tracks and certification, extends
`LibraryStudySystem` with authored manuals and authoring outcomes, extends
`SkillProgressionSystem` usage without touching its ownership, and adds a thin
curriculum/literacy layer that writes only to its own additive sub-objects. No
second school save section, no second skill store, no parallel manual catalog.

---

## 3. DESIGN PILLARS AND TONE

### 3.1 Pillars

**Pillar 1 — Knowledge dies with people.** Every master carries a library that
is not written down yet. The expansion makes writing it down urgent.

**Pillar 2 — Literacy is capability.** Reading is how a survivor uses everything
else: manuals, labels, maps, medicine, law.

**Pillar 3 — Teaching is work.** A teacher's day is scheduled, tiring, and
counted like any shift.

**Pillar 4 — Exams are honest.** Certification is earned through demonstrated
competence, never through a bar that fills by attendance.

**Pillar 5 — Children are the future workforce, not a mascot.** Schooling is
framed as the shelter investing in its own continuation.

### 3.2 Tone calibration

| Element | Do | Do not |
|---|---|---|
| First letter read | Pride, quiet | Celebration montage |
| A hard exam | Honest assessment | Humiliation |
| A master's death | What was lost, what was saved | Melodrama |
| Children's class | Real lessons, real children | Cuteness |
| Manual writing | Careful, flawed work | Perfection |
| Certification | Responsibility | Rank fantasy |

### 3.3 Content limits

- No shaming of adults who cannot read.
- No child prodigy tropes; competence is earned.
- No corporal punishment; discipline is authored and humane.
- Teachers are workers with limits, not saints.
- Knowledge loss is tragic but never gratuitous.

---

## 4. THE SCHOOLHOUSE WORLD

### 4.1 Interior rooms

- **`room_schoolhouse`** — benches, a board, and a stove.
- **`room_library`** — shelves, lockers, and the manual wall.
- **`room_scribing_desk`** — ink, paper, and copying.
- **`room_workshop_class`** — benches for trade instruction.
- **`room_apprentice_hall`** — pairs, tools, and progress boards.
- **`room_exam_room`** — a table, a task, and an examiner.
- **`room_reading_nook`** — the quiet corner.
- **`room_archive_room`** — records, wills, and transcripts.

### 4.2 Exterior locations

| ID | Name | Danger | Purpose |
|---|---|---|---|
| `loc_school_ruins` | The School Ruins | 5 | Books, slates, and furniture |
| `loc_press_shop` | The Press Shop | 4 | Ink and paper supply |
| `loc_book_market` | The Book Market | 4 | Trade in manuals and primers |
| `loc_old_university` | The Campus | 6 | Deep salvage and lost knowledge |
| `loc_field_class` | The Field Class | 3 | Practical instruction outdoors |
| `loc_workshop_row` | The Workshop Row | 3 | Trade demonstrations |
| `loc_listening_post` | The Listening Post | 4 | Learning from radio lessons |
| `loc_memorial_wall` | The Memorial Wall | 2 | Names teachers read aloud |
| `loc_scribes_camp` | The Scribes' Camp | 4 | Paper, ink, and copying |
| `loc_lost_carrel` | The Lost Carrel | 5 | A sealed study with one book |

All locations require valid item references and scanner registration.

### 4.3 The school week

Six days of class and one day of maintenance. Reading and numbers every day;
trade, health, civics, and survival on rotation. The schedule is the expansion's
backbone: it makes teaching visible, countable, and interruptible.

---

## 5. MAIN STORYLINE — "WHAT WE PASS ON"

### 5.1 Central conflict

The shelter's engineer, **Holt**, is the only person who understands the water
pumps, and he is sixty-three. The children cannot read. **Odele Tarn** the
librarian keeps the manuals behind a locked door because the manuals are the
most valuable thing the shelter owns and the most likely to be stolen. When a
pump fails while Holt is sick, the shelter discovers that no one else can read
the maintenance procedure — and the manual is written in technical language no
one left alive was taught.

**Miren Kade**, a former teacher, proposes a schoolhouse. **Master Bruch**, the
foundry's last journeyman, agrees to take apprentices but insists that
instruction is not free: it costs the apprentice's production hours. The
apprentices build a curriculum out of what the shelter actually needs, and the
shelter discovers that education is not a luxury or a reward — it is maintenance
on the minds that keep everything else running.

The expansion's question: **what does a shelter owe its future, and who pays
for it now?**

### 5.2 Theme (unspoken)

**You can inherit a shelter. You cannot inherit understanding.**

### 5.3 Principal NPCs

| ID | Name | Role | Function |
|---|---|---|---|
| `npc_teacher_miren_kade` | Miren Kade | Teacher | School, curriculum, and literacy |
| `npc_master_bruch` | Master Bruch | Foundry master | Trade instruction and standards |
| `npc_librarian_odele_tarn` | Odele Tarn | Librarian | Manuals, access, and preservation |
| `npc_scribe_arian` | Arián | Scribe | Copying, ink, and authoring |
| `npc_pupil_hollis` | Hollis | Pupil | The first reader; a child with plans |
| `npc_engineer_holt` | Engineer Holt | Engineer | The knowledge that must be written down |
| `npc_parent_maera` | Maera | Parent | The cost of school hours at home |
| `npc_examiner_voss` | Voss | Examiner | Certification and standards |

### 5.4 Story beats (15)

1. **The Pump.** A failure exposes the reading gap.
2. **The Locked Door.** Manual access is debated.
3. **The Schoolhouse.** A room is cleared and a teacher found.
4. **The First Class.** Children, adults, and a board with three letters.
5. **The Cost.** Production hours versus school hours.
6. **The Primer.** A reading book is written by hand.
7. **The Trade.** Bruch takes three apprentices.
8. **The Copy.** The pump manual is transcribed and simplified.
9. **The Exam.** The first certification is attempted.
10. **The Will.** Holt writes down what he knows.
11. **The Night Class.** Adults who never learned ask to learn.
12. **The Debate.** What is taught: trade, health, civics, survival.
13. **The Graduates.** The first class finishes.
14. **The Loss.** A master dies; the shelter counts what was saved.
15. **What We Pass On.** Final disposition of the school and its archive.

### 5.5 Branching choices (8)

| Choice | Options | Axis |
|---|---|---|
| School hours | full / half / work-first | future vs. present |
| Access | open / supervised / locked | trust vs. loss |
| Curriculum | trade / broad / survival | utility vs. breadth |
| Who pays | shelter / families / apprentices | fairness |
| Literacy | children / all / volunteers | scope |
| Exams | strict / practical / none | standards vs. speed |
| Manuals | copy / simplify / seal | preservation |
| Final | school as institution / as luxury / as memory | identity |

### 5.6 Endings (5 + fade)

1. **The Open School** — every child reads, every trade has a second pair of
   hands, and the archive grows.
2. **The Trade Track** — the shelter produces certified workers and sells their
   skill as a service.
3. **The Written Store** — the manuals are complete, simplified, and copied; the
   shelter's knowledge survives its masters.
4. **The Half Day** — school shares time with work and neither is starved.
5. **The Empty Classroom** — education loses to production and the shelter pays
   later in broken machines and lost skills.
6. **Fade** — classes continue irregularly; the pump holds for now.

---

## 6. QUEST DESIGN

New IDs use prefix `quest_lesson_`. Schema follows existing quest catalogs.

### 6.1 Main questline (15)

`quest_lesson_the_pump`, `quest_lesson_locked_door`, `quest_lesson_schoolhouse`,
`quest_lesson_first_class`, `quest_lesson_the_cost`, `quest_lesson_the_primer`,
`quest_lesson_the_trade`, `quest_lesson_the_copy`, `quest_lesson_the_exam`,
`quest_lesson_the_will`, `quest_lesson_night_class`, `quest_lesson_the_debate`,
`quest_lesson_graduates`, `quest_lesson_the_loss`, `quest_lesson_what_we_pass_on`.

### 6.2 Side quests (30)

**Literacy (5)**
- `quest_lesson_read_first` — teach the first reader
- `quest_lesson_adult_class` — an adult literacy class
- `quest_lesson_labels` — label the shelter's storage
- `quest_lesson_sign_name` — signatures and records
- `quest_lesson_read_aloud` — reading aloud as morale

**Schooling (5)**
- `quest_lesson_school_repair` — repair the schoolhouse
- `quest_lesson_attendance` — attendance and truancy
- `quest_lesson_school_food` — meals for pupils
- `quest_lesson_school_stove` — heat for the classroom
- `quest_lesson_school_supplies` — slates, chalk, and paper

**Curriculum (5)**
- `quest_lesson_numbers` — arithmetic for the stores
- `quest_lesson_health_class` — hygiene and first aid
- `quest_lesson_map_class` — reading maps and routes
- `quest_lesson_civics` — law, rationing, and duties
- `quest_lesson_survival_class` — cold, fire, and salvage safety

**Apprenticeship (5)**
- `quest_lesson_find_master` — find a second master
- `quest_lesson_apprentice_three` — take three apprentices
- `quest_lesson_pair_trouble` — a difficult pair
- `quest_lesson_journeywork` — a journeyman project
- `quest_lesson_master_standard` — keep the trade standard

**Manuals (5)**
- `quest_lesson_transcribe` — transcribe a manual
- `quest_lesson_simplify` — simplify a technical manual
- `quest_lesson_copy_run` — make three copies
- `quest_lesson_missing_page` — recover a missing page
- `quest_lesson_write_new` — author a new manual

**Exams (5)**
- `quest_lesson_first_exam` — the first certification
- `quest_lesson_standard_set` — set the standard
- `quest_lesson_fail_case` — a failed candidate
- `quest_lesson_certificate` — issue a certificate
- `quest_lesson_guest_exam` — examine a visitor

### 6.3 Repeatable quests (8)

`quest_lesson_repeat_class`, `quest_lesson_repeat_copy`,
`quest_lesson_repeat_exam`, `quest_lesson_repeat_apprentice`,
`quest_lesson_repeat_simplify`, `quest_lesson_repeat_supplies`,
`quest_lesson_repeat_read`, `quest_lesson_repeat_track`.

### 6.4 Dynamic hooks

Live events (apprenticeship ticks, mentor death, study completion, skill
milestones, child aging, injuries) attach authored follow-ups through existing
seams. No new event bus.

### 6.5 Constraints

- Skills are written only through `SkillProgressionSystem`.
- Study outcomes are written only through `LibraryStudySystem`.
- Mentorship progress is written only through `ApprenticeshipSystem`.
- Manuals are data rows consumed by the live loader.
- Certification gates capability only where a real task exists.
- No education state outside the two live save sections.

---

## 7. NEW GAMEPLAY SYSTEMS

### 7.1 `LiteracySystem` (new, `Ashfall.Core.Survivors`)

**Owns:** reading and writing levels, practice, and the roll-up that lets a
survivor use manuals. **Consumes:** `LibraryStudySystem`, `SkillProgressionSystem`,
`NeedsSystem` (energy). **Data:** `literacy_levels.json`.
**Rules:** literacy is earned through instruction and practice; a survivor who
cannot read cannot study technical manuals until taught; levels are honest and
visible.

### 7.2 `CurriculumSystem` (new, `Ashfall.Core`)

**Owns:** seasons of teaching, subject rotation, class plans, and attendance.
**Consumes:** `LiteracySystem`, `SkillProgressionSystem`, `NeedsSystem`,
`CampaignCalendar`. **Data:** `curricula.json`, `lessons.json`.
**Rules:** curriculum is authored; a class consumes teacher and pupil hours;
attendance is recorded; the shelter can change what it teaches when needs
change.

### 7.3 `SchoolingSystem` (new, `Ashfall.Core.Survivors`)

**Owns:** child-age-band schooling, school day execution, and pupil progress.
**Consumes:** `CurriculumSystem`, `GenerationalSystem` (Wave 1) for aging, diets,
`NeedsSystem`. **Data:** `school_bands.json`. **Rules:** children attend by band;
school is a real time cost; no child is forced into a track before the shelter
chooses a policy.

### 7.4 `ExaminationSystem` (new, `Ashfall.Core`)

**Owns:** assessments, standards, certification, and failure handling.
**Consumes:** `ApprenticeshipSystem` progress, `LiteracySystem`,
`SkillProgressionSystem`, `CurriculumSystem`. **Data:** `examinations.json`.
**Rules:** certification is demonstrated, not accumulated; a failed exam is
informative, never humiliating; certification unlocks real responsibility, not
ranks.

### 7.5 `ManualAuthoringSystem` (new, `Ashfall.Core.Narrative`)

**Owns:** turning a master's knowledge into a manual, transcription quality,
simplification, and copies. **Consumes:** `LibraryStudySystem` catalogs,
`ArchiveDeskSystem` (ink and paper), `ApprenticeshipSystem` (`TranscriptionTask`),
`SkillProgressionSystem`. **Data:** `manual_quality.json`.
**Rules:** authoring takes days and produces an imperfect document; quality
depends on the master's clarity and the scribe's skill; manuals can be lost and
must be copied.

### 7.6 `TeachingRoleSystem` (new, thin, `Ashfall.Core.Survivors`)

**Owns:** teacher, examiner, scribe, and librarian roles and their schedules.
**Consumes:** `DutyRoster` (Expansion 02), `SchoolingSystem`, `CurriculumSystem`.
**Data:** `school_roles.json`. **Rules:** teaching is a shift; a teacher is also a
worker who tires; the shelter can rotate teachers.

### 7.7 Systems explicitly not added

- No second apprenticeship, library, skill, or XP system.
- No grades as content gates.
- No research tree.
- No new currency.
- No new RNG stream.
- No new save section.

---

## 8. DATA CATALOG SPECIFICATION

All catalogs snake_case, integer `schema_version: 1`, validated and scanner-registered.

### 8.1 `literacy_levels.json` (new)

```json
{
  "schema_version": 1,
  "levels": [
    {
      "level_id": "literacy_1",
      "display_name": "Signs Name",
      "reading": 1,
      "writing": 1,
      "unlocks": ["labels", "signatures"],
      "study_hours": 20,
      "tags": ["basic", "adult"]
    }
  ]
}
```

### 8.2 `curricula.json` (new)

Curriculum rows: season, subjects, hours per week, teacher requirement, pupils,
and outcomes.

### 8.3 `lessons.json` (new)

Lesson rows: subject, band, materials, duration, teacher skill, and outcome
check.

### 8.4 `school_bands.json` (new)

Band rows: age range, subjects, hours, and restrictions.

### 8.5 `examinations.json` (new)

Exam rows: trade, standards, task, duration, examiner skill, pass bands, and
certification.

### 8.6 `manual_quality.json` (new)

Quality rows: source skill, scribe skill, days, clarity bands, and defects.

### 8.7 `school_roles.json` (new)

Role rows: teacher, examiner, scribe, librarian, with duties and fatigue.

### 8.8 `apprenticeship_catalog.json` (extend)

Add authored mentorships: trade, master requirement, apprentice requirement,
duration, milestones, and completion effects.

### 8.9 `library_manuals.json` (extend)

Add manuals matching the live `ManualDefinition` schema: power requirement,
comprehension, hours, prerequisites, and outcomes.

### 8.10 Items

New items appended to `items.json`: `item_slate`, `item_chalk`,
`item_reader_primer`, `item_copy_book`, `item_ink_bottle`,
`item_quill_set`, `item_paper_sheet`, `item_abacus`,
`item_etching_stylus`, `item_manual_binder`, `item_school_bell`,
`item_study_lamp`, `item_attendance_board`, `item_certificate_slip`.

---

## 9. SAVE, DETERMINISM, AND PERSISTENCE

### 9.1 Ownership

`ApprenticeshipState` and `LibraryStudyState` remain the live save owners. New
sub-objects (literacy levels, curriculum, school bands, exams, manual quality,
teaching roles) are additive inside those stores. No new save section.

### 9.2 State to persist

- Literacy levels and practice progress.
- Current curriculum and lesson plan.
- Attendance and pupil progress.
- Exam records and certifications.
- Manual-quality records and copies.
- Teaching role assignments.

### 9.3 Determinism

- Comprehension is a function of manual, skill, literacy, and hours.
- Exam outcomes are deterministic given standards and preparation.
- No wall-clock or unseeded randomness; daily ticks use the live path.
- Paired replay hashes must match.

### 9.4 Migration

Legacy saves load with existing apprenticeship and study state untouched; no
literacy, curriculum, exam, or role state exists until started. Existing
apprenticeships continue to progress exactly as before.

### 9.5 Checksum

Invariant-culture floats; comprehension and literacy as integer levels where
possible.

---

## 10. UI, ACCESSIBILITY, AND PRESENTATION

### 10.1 Surfaces

| Surface | Purpose | Owner |
|---|---|---|
| `SchoolhousePanel` (new) | Class, attendance, pupils | `SchoolhouseHostSession` |
| `CurriculumPanel` (new) | Subjects and seasons | same |
| `LiteracyPanel` (new) | Reading levels and progress | same |
| `ApprenticeshipPanel` (extend) | Tracks and certification | existing |
| `LibraryStudyPanel` (extend) | Manuals, study, quality | existing |
| `ExamPanel` (new) | Assessments and standards | same |
| `ManualAuthoringPanel` (new) | Transcribe, simplify, copy | same |
| `TeachingRolePanel` (new) | Teacher schedules | same |

### 10.2 Accessibility and honesty

- Panels show live state and expose existing commands only.
- Literacy is shown as capability, never as a score dragged on a person.
- Attendance shows the real cost in workshop hours.
- Keyboard/controller close/back preserved; focus maintained on refresh.
- Text is the primary medium; no reading is required to operate the UI itself.

### 10.3 Presentation

Audio cues appended to `audio_cues.json`: a school bell, chalk, a page turning,
an ink pot, a stamp. No cue is required; text carries meaning.

---

## 11. INTEGRATION SEAMS

| Existing system | Attachment |
|---|---|
| `ApprenticeshipSystem` | Tracks, standards, completion |
| `LibraryStudySystem` | Manuals, comprehension, outcomes |
| `ArchiveDeskSystem` | Ink, paper, transcripts |
| `SkillProgressionSystem` | Sole skill authority |
| `DutyRoster` | Teacher and pupil schedules |
| `GenerationalSystem` | Child aging and bands |
| `NeedsSystem` | Energy and morale costs |
| `CampaignCalendar` | Terms and lesson days |
| `JobBoard` / roles | Teaching assignments |
| `TradingSystem` | Books, paper, education services |
| `Medical` | Health class, first aid knowledge |
| `EpilogueChronicleBuilder` | School and archive milestones |

---

## 12. TECHNICAL IMPLEMENTATION PLAN

### 12.1 Phase order

**Phase 0 — Premise re-audit.** Confirm `ApprenticeshipSystem`,
`LibraryStudySystem`, `ArchiveDeskSystem`, hosts, panels, loaders, and data
sizes. Record file:line; change nothing.

**Phase 1 — Data + validators.** Author literacy levels, curricula, lessons,
bands, examinations, manual quality, roles; extend apprenticeship and library
catalogs; append items. Register validators and scanner.

**Phase 2 — Pure Core.** `LiteracySystem`, `CurriculumSystem`,
`SchoolingSystem`, `ExaminationSystem`, `ManualAuthoringSystem`,
`TeachingRoleSystem`.

**Phase 3 — Persistence.** Additive sub-objects, migration, round-trip,
determinism.

**Phase 4 — Host + CLI.** `SchoolhouseHostSession`, selftest coverage, fresh
journey.

**Phase 5 — UI.** New and extended panels with lifecycle and accessibility.

**Phase 6 — Content.** Locations, rooms, NPCs, quests, items, prose, audio.

**Phase 7 — Balance.** 360-day soak: children age, apprentices certify, manuals
are written and read.

**Phase 8 — Verification and closeout.**

### 12.2 Content volume

| Content | Count |
|---|---|
| Literacy levels | 6 |
| Curricula | 8 |
| Lessons | 60 |
| School bands | 4 |
| Examinations | 15 |
| Manual quality rows | 10 |
| School roles | 5 |
| Apprenticeship rows | 20 |
| Manual definitions | 30 |
| Items | 14 |
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
| Second skill system | Critical | `SkillProgressionSystem` only |
| Second library | Critical | Extend `LibraryStudySystem` |
| Grades as gates | High | Capability gates only |
| School punished by production | High | Authored costs and tradeoffs |
| Literacy as hidden stat | High | Visible capability |
| Child content tonal risk | High | Restraint review |
| Determinism break | Low | Live tick paths |
| Content overrun | Medium | Budget §13 |

---

## 13. APPENDIX A — AUTHORING BUDGET

| File | Rows | Est. words |
|---|---|---|
| `literacy_levels.json` | 6 | 1,500 |
| `curricula.json` | 8 | 2,500 |
| `lessons.json` | 60 | 12,000 |
| `school_bands.json` | 4 | 1,000 |
| `examinations.json` | 15 | 4,000 |
| `manual_quality.json` | 10 | 2,000 |
| `school_roles.json` | 5 | 1,500 |
| `apprenticeship_catalog.json` | +20 | 5,000 |
| `library_manuals.json` | +30 | 7,000 |
| Quest objectives | 53 quests | 15,000 |
| NPC prose | 8 NPCs | 7,000 |
| Location prose | 10 | 3,500 |
| Item descriptions | 14 | 2,000 |
| Ending prose | 6 | 3,000 |
| **Total** | | **~69,000** |

---

## 14. APPENDIX B — RISK REGISTER

| ID | Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|---|
| R28-1 | Second skill system | Low | Critical | One skill owner |
| R28-2 | Second library | Low | Critical | Extend live study |
| R28-3 | Grades as gates | Med | High | Real tasks only |
| R28-4 | School vs. production | High | High | Authored tradeoff |
| R28-5 | Literacy invisible | Med | High | Capability display |
| R28-6 | Child tone | Med | High | Content review |
| R28-7 | Manual authoring trivial | Med | Med | Days, defects, copies |
| R28-8 | Determinism | Low | High | Live tick paths |
| R28-9 | Content overrun | Med | Med | Budget §13 |
| R28-10 | Exam humiliation | Low | Med | Informative failure |

---

## 15. APPENDIX C — OPEN DECISIONS REQUIRING A FOREMAN SIGNATURE

1. **Does literacy gate manual study?** Recommended: simple manuals no; technical
   manuals yes.
2. **Are children schooled during work hours?** Recommended: yes, as a policy
   decision with real costs.
3. **Can adults learn to read?** Recommended: yes, via night class; that is the
   expansion's most humane loop.
4. **Do exams ever fail a candidate permanently?** Recommended: no; retakes after
   practice.
5. **Can manuals be traded?** Recommended: yes, with copy scarcity preserved.

---

## 17. APPENDIX D — LITERACY LEVEL TABLE (6 LEVELS)

| # | Level | Reading | Writing | Unlocks | Hours |
|---|---|---|---|---|---|
| 1 | Illiterate | 0 | 0 | none | 0 |
| 2 | Signs Name | 1 | 1 | labels, signatures | 20 |
| 3 | Reads Labels | 2 | 1 | storage, warnings | 40 |
| 4 | Reads Simple Text | 3 | 2 | primers, notes | 80 |
| 5 | Reads Technical | 4 | 3 | manuals | 160 |
| 6 | Writes Clearly | 5 | 4 | authoring, records | 280 |

Literacy is capability, not a score. A survivor at level 5 can follow a pump
manual; a survivor at level 6 can write one that someone else can follow.

---

## 18. APPENDIX E — CURRICULUM TABLE (8 CURRICULA)

| # | Curriculum | Subjects | Hours/week | Teacher | Pupils | Outcome |
|---|---|---|---|---|---|---|
| 1 | Foundation | letters, numbers | 12 | teacher | children | literacy 2 |
| 2 | Trade Prep | tools, materials | 10 | master | teens | apprentice-ready |
| 3 | Health | hygiene, first aid | 8 | medic | all | health literacy |
| 4 | Survival | cold, fire, salvage | 8 | veteran | all | safety habits |
| 5 | Civics | law, rations, duties | 6 | steward | adults | participation |
| 6 | Technical | manuals, pumps | 12 | engineer | literate | certified study |
| 7 | Agriculture | soil, seasons | 10 | grower | assigned | field skills |
| 8 | Adult Night | letters, numbers | 6 | teacher | volunteers | literacy 3 |

Curriculum is a seasonal decision. A shelter that teaches survival in winter and
agriculture in spring is using its teacher well; one that teaches nothing is
spending its future.

---

## 19. APPENDIX F — LESSON TABLE (60 LESSONS — REPRESENTATIVE)

| # | Lesson | Subject | Band | Materials | Duration |
|---|---|---|---|---|---|
| 1 | The Alphabet | letters | young | board, chalk | 1h |
| 2 | My Name | writing | young | slate, chalk | 1h |
| 3 | Counting Stores | numbers | young | stones, board | 1h |
| 4 | Reading Labels | reading | young | jars, signs | 1h |
| 5 | The Calendar | numbers | young | board | 1h |
| 6 | Weather Signs | survival | young | field | 1h |
| 7 | Wash Your Hands | health | young | basin, soap | 1h |
| 8 | Fire Safety | survival | young | stove | 1h |
| 9 | The Map | reading | mid | map | 1h |
| 10 | Simple Fractions | numbers | mid | board | 1h |
| 11 | The Water Cycle | science | mid | diagram | 1h |
| 12 | Reading a Manual | reading | mid | primer | 1h |
| 13 | Tools and Names | trade | mid | tools | 1h |
| 14 | First Aid Basics | health | mid | kit | 1h |
| 15 | The Ration Rule | civics | mid | board | 1h |
| 16 | Seed and Season | agriculture | mid | seeds | 1h |
| 17 | Fur and Fiber | trade | mid | samples | 1h |
| 18 | Cold and Layers | survival | mid | clothing | 1h |
| 19 | Measuring Cloth | numbers | mid | tape | 1h |
| 20 | The Pump Diagram | technical | older | manual | 2h |
| 21 | Valves and Flow | technical | older | model | 2h |
| 22 | Wire and Load | technical | older | samples | 2h |
| 23 | Reading Medicine | technical | older | labels | 2h |
| 24 | Soil and Rot | agriculture | older | samples | 2h |
| 25 | The Foundry Heat | trade | older | shop | 2h |
| 26 | Letters to Home | writing | older | paper, ink | 2h |
| 27 | Keeping Records | writing | older | ledgers | 2h |
| 28 | The Contract | civics | older | samples | 2h |
| 29 | Triage Order | health | older | drill | 2h |
| 30 | The Night Watch | survival | older | post | 2h |
| 31 | The First Sentence | writing | adult | slate | 1h |
| 32 | My Trade's Names | trade | adult | tools | 1h |
| 33 | Reading the Notice | reading | adult | board | 1h |
| 34 | Numbers at Work | numbers | adult | board | 1h |
| 35 | The Medicine Label | health | adult | label | 1h |
| 36 | The Route Sheet | reading | adult | map | 1h |
| 37 | The Ration Ledger | civics | adult | ledger | 1h |
| 38 | The Weather Log | science | adult | log | 1h |
| 39 | The Tool Inventory | writing | adult | list | 1h |
| 40 | The Pump Handover | technical | adult | manual | 2h |

*(Forty table rows shown; the remaining twenty follow the same pattern across
bands and subjects. The authored set covers letters, numbers, reading, writing,
health, civics, survival, trade, agriculture, and technical instruction.)*

---

## 20. APPENDIX G — SCHOOL BAND TABLE (4 BANDS)

| # | Band | Ages | Subjects | Hours | Restriction |
|---|---|---|---|---|---|
| 1 | Young | 5–9 | letters, numbers, habits | 6 | no heavy work |
| 2 | Middle | 10–13 | reading, arithmetic, health | 8 | light duties only |
| 3 | Older | 14–17 | technical, trade, civics | 10 | apprentice track |
| 4 | Adult | 18+ | night class, voluntary | 6 | after shift |

Age bands come from the live generational and survivor aging paths; the
expansion does not create a second aging model. It only attaches schooling to
ages the game already tracks.

---

## 21. APPENDIX H — EXAMINATION TABLE (15 EXAMS)

| # | Exam | Trade | Standards | Duration | Examiner | Pass |
|---|---|---|---|---|---|---|
| 1 | Junior Reader | literacy | level 2 | 1h | teacher | 2/3 |
| 2 | Store Counter | numbers | accurate count | 1h | steward | exact |
| 3 | Label Reader | literacy | level 3 | 1h | teacher | 2/3 |
| 4 | Water Tester | utility | procedure | 2h | engineer | steps |
| 5 | First Aider | health | bandaging | 2h | medic | demo |
| 6 | Field Hand | agriculture | planting | 2h | grower | demo |
| 7 | Foundry Hand | foundry | safety | 3h | master | demo |
| 8 | Loom Hand | textile | basic weave | 3h | weaver | output |
| 9 | Cook's Ticket | kitchen | safe food | 2h | cook | demo |
| 10 | Watch Keeper | security | protocol | 1h | guard lead | oral |
| 11 | Radio Listener | signals | logging | 1h | operator | log |
| 12 | Scribe's Trial | archive | copy fidelity | 3h | librarian | exact |
| 13 | Journeyman | any | project | 5d | master | project |
| 14 | Teacher's Ticket | education | teach a class | 3h | examiner | demo |
| 15 | Examiner's Mark | education | standard | 1d | senior | board |

Certification is earned by demonstrated competence, never by attendance. A
failed exam teaches the candidate what to practise, which is why the expansion
never punishes failure with a permanent gate.

---

## 22. APPENDIX I — MANUAL QUALITY TABLE (10 ROWS)

| # | Quality | Master skill | Scribe skill | Days | Clarity | Defects |
|---|---|---|---|---|---|---|
| 1 | Scrawl | any | 1 | 2 | 20% | many |
| 2 | Rough Notes | 2 | 1 | 3 | 40% | some |
| 3 | Draft | 3 | 2 | 4 | 55% | some |
| 4 | Clean Copy | 4 | 3 | 5 | 70% | few |
| 5 | Simplified | 4 | 4 | 6 | 80% | few |
| 6 | Illustrated | 5 | 3 | 8 | 85% | rare |
| 7 | Teaching Text | 5 | 4 | 10 | 90% | rare |
| 8 | Reference | 5 | 5 | 12 | 95% | very rare |
| 9 | Archive Standard | 5 | 5 | 14 | 98% | none |
| 10 | Copy of Copy | any | 2 | 3 | −10% | inherits |

A manual is never perfect, and copying a copy degrades it. This is why a shelter
that writes things down early and copies often keeps its knowledge, and one that
waits keeps a memory that walks out the door.

---

## 23. APPENDIX J — SCHOOL ROLE TABLE (5 ROLES)

| # | Role | Duties | Fatigue | Requirement |
|---|---|---|---|---|
| 1 | Teacher | classes, records | high | literacy 4 |
| 2 | Master | apprentices, standards | high | trade competence |
| 3 | Examiner | exams, certification | med | trade + literacy 4 |
| 4 | Scribe | copying, authoring | med | literacy 5 |
| 5 | Librarian | access, preservation | low | literacy 4 |

Teaching is a shift. The expansion keeps teachers inside the live duty and
fatigue model so the school is part of the shelter, not above it.

---

## 24. APPENDIX K — APPRENTICESHIP TRACK TABLE (20 TRACKS)

| # | Track | Master | Apprentice | Duration | Milestones |
|---|---|---|---|---|---|
| 1 | Foundry | smith | any literate | 120d | feed, pour, finish |
| 2 | Water | engineer | literate | 90d | valves, pumps, test |
| 3 | Power | electrician | literate | 120d | wire, load, grid |
| 4 | Medical | medic | literate | 180d | triage, bandage, dose |
| 5 | Kitchen | cook | any | 60d | prep, preserve, serve |
| 6 | Farming | grower | any | 120d | soil, seed, harvest |
| 7 | Greenhouse | grower | any | 120d | trays, climate, pest |
| 8 | Textile | weaver | any | 150d | spin, warp, weave |
| 9 | Leather | tanner | any | 120d | soak, scrape, tan |
| 10 | Carpentry | carpenter | any | 120d | joint, frame, finish |
| 11 | Masonry | mason | any | 150d | mix, lay, cure |
| 12 | Glass | glassblower | literate | 150d | batch, blow, anneal |
| 13 | Radio | operator | literate | 120d | tune, log, relay |
| 14 | Archive | librarian | literate | 120d | catalog, copy, store |
| 15 | Security | guard lead | any | 90d | patrol, report, drill |
| 16 | Teaching | teacher | literate 4 | 150d | observe, assist, teach |
| 17 | Railway | rail crew | any | 150d | inspect, switch, dispatch |
| 18 | Salvage | scrapper | any | 90d | survey, cut, haul |
| 19 | Munitions | armorer | literate | 180d | handle, load, store |
| 20 | Weather | observer | literate | 90d | read, record, forecast |

Every track uses the live `MentorshipDef` shape: a master requirement, apprentice
requirement, duration, milestones, and completion effects. The catalog grows from
three rows to twenty-three, which is what turns apprenticeship from a demo into
the shelter's workforce pipeline.

---

## 25. APPENDIX L — MANUAL DEFINITION TABLE (30 MANUALS)

| # | Manual | Power | Comprehension | Hours | Prerequisite |
|---|---|---|---|---|---|
| 1 | Pump Maintenance | hand | 60% | 40 | literacy 4 |
| 2 | Valve Rebuild | hand | 50% | 32 | literacy 4 |
| 3 | Water Testing | hand | 70% | 24 | literacy 3 |
| 4 | Grid Safety | power | 55% | 36 | literacy 4 |
| 5 | Wiring Diagrams | power | 45% | 48 | literacy 4 |
| 6 | Battery Care | hand | 65% | 28 | literacy 3 |
| 7 | Triage Guide | hand | 75% | 30 | literacy 3 |
| 8 | Wound Care | hand | 80% | 24 | literacy 3 |
| 9 | Pharmacology | power | 40% | 60 | literacy 5 |
| 10 | Preserving Food | hand | 85% | 20 | literacy 2 |
| 11 | Fermentation | hand | 75% | 24 | literacy 2 |
| 12 | Soil and Rot | hand | 80% | 24 | literacy 2 |
| 13 | Greenhouse Climate | hand | 60% | 36 | literacy 3 |
| 14 | Seed Saving | hand | 85% | 16 | literacy 2 |
| 15 | Weaving Patterns | hand | 70% | 28 | literacy 3 |
| 16 | Tanning | hand | 65% | 30 | literacy 3 |
| 17 | Boot Making | hand | 70% | 32 | literacy 3 |
| 18 | Carpentry Joints | hand | 75% | 28 | literacy 2 |
| 19 | Brick and Lime | hand | 70% | 30 | literacy 2 |
| 20 | Glass Batch | hand | 50% | 40 | literacy 4 |
| 21 | Radio Repair | power | 45% | 44 | literacy 4 |
| 22 | Signal Codes | hand | 60% | 24 | literacy 4 |
| 23 | Rail Inspection | hand | 65% | 32 | literacy 3 |
| 24 | Steam Basics | hand | 55% | 40 | literacy 4 |
| 25 | Salvage Safety | hand | 85% | 16 | literacy 2 |
| 26 | Explosives Handling | hand | 50% | 36 | literacy 4 |
| 27 | Weather Reading | hand | 75% | 24 | literacy 3 |
| 28 | Ration Accounting | hand | 70% | 24 | literacy 3 |
| 29 | Law and Custom | hand | 80% | 20 | literacy 3 |
| 30 | Teaching Methods | hand | 65% | 32 | literacy 5 |

Manuals are consumed through the live `LibraryStudySystem`, including power
requirements and comprehension rates. Writing a manual is the only way to widen
the set; the expansion adds the write path and lets the shelter author its own
texts over time.

---

## 26. APPENDIX M — MAIN QUESTLINE STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_lesson_the_pump` | 4 | Pump fails; reading gap exposed |
| `quest_lesson_locked_door` | 4 | Debate manual access |
| `quest_lesson_schoolhouse` | 4 | Clear room; find teacher |
| `quest_lesson_first_class` | 5 | Build the first lesson |
| `quest_lesson_the_cost` | 4 | Negotiate school vs. work hours |
| `quest_lesson_the_primer` | 4 | Write a reading primer |
| `quest_lesson_the_trade` | 5 | Set up apprentice tracks |
| `quest_lesson_the_copy` | 5 | Transcribe and simplify the pump manual |
| `quest_lesson_the_exam` | 4 | First certification attempt |
| `quest_lesson_the_will` | 5 | Holt writes down his knowledge |
| `quest_lesson_night_class` | 4 | Adults ask to learn |
| `quest_lesson_the_debate` | 4 | Choose the curriculum |
| `quest_lesson_graduates` | 4 | First class finishes |
| `quest_lesson_the_loss` | 5 | A master dies; count what was saved |
| `quest_lesson_what_we_pass_on` | 3 | Final disposition |

---

## 27. APPENDIX N — NPC DOSSIERS (BRIEF)

**Miren Kade** — teacher. Taught before the Exchange; refuses to call the
schoolhouse a luxury. Measures her success in signatures on ration books.

**Master Bruch** — foundry master. Believes a trade is a debt paid forward and
that an apprentice who is not useful in a year was never taught.

**Odele Tarn** — librarian. Guards the manuals because she has seen a library
burn. Will negotiate access with anyone who promises to bring a copy back.

**Arián** — scribe. Copies until his hand cramps and considers a legible page a
victory. Keeps the shelter's ink recipe memorized.

**Hollis** — pupil. First to read, first to ask why the manual says what it says,
first to propose the school keeps a copy outside the shelter.

**Engineer Holt** — engineer. Sixty-three, tired, knows the pumps in his hands and
is finally willing to write them down. The expansion's quiet clock.

**Maera** — parent. Needs her teenager's labor at home and hates choosing between
the stove and the schoolhouse. The cost of education wearing a face.

**Voss** — examiner. Sets standards and refuses to pass anyone who cannot do the
task. Believes a certificate that means nothing is worse than no certificate.

---

## 28. APPENDIX O — LOCATION DETAIL

- **The School Ruins** — desks, books, a collapsed roof; salvage with a schedule.
- **The Press Shop** — ink, paper, and type; the school's supply line.
- **The Book Market** — traders, manuals, primers, and prices by condition.
- **The Campus** — a ruined university; deep salvage and dangerous floors.
- **The Field Class** — outdoor instruction: soil, weather, and seed.
- **The Workshop Row** — trade demonstrations where apprentices watch real work.
- **The Listening Post** — radio lessons copied by hand for those who cannot read.
- **The Memorial Wall** — names of the dead; teachers read them aloud every term.
- **The Scribes' Camp** — a copying house with racks of drying pages.
- **The Lost Carrel** — a sealed study room holding one intact book.

---

## 29. APPENDIX P — LITERACY AND COMPREHENSION MODEL

| Reader state | Simple manual | Technical manual | Authoring |
|---|---|---|---|
| Illiterate | cannot | cannot | no |
| Signs Name | cannot | cannot | no |
| Reads Labels | partial | cannot | no |
| Reads Simple | yes | partial | no |
| Reads Technical | yes | yes | assisted |
| Writes Clearly | yes | yes | yes |

Comprehension multiplies by manual clarity and study hours. A 40-hour manual at
50% comprehension takes longer than a 24-hour manual at 80%, which is why
simplification is often worth more than copying.

---

## 30. APPENDIX Q — KNOWLEDGE LOSS MODEL

| Event | Loss | Mitigation |
|---|---|---|
| Master dies | untaught skill | written manuals, apprentices |
| Apprentice dies | partial training | pair redundancy |
| Library fire | manuals lost | copies, offsite storage |
| Paper shortage | no new copies | paper mill, trade |
| Ink shortage | no authoring | archive ink path |
| Literacy collapse | manuals unreadable | school, night class |
| Master retires | teaching gap | second master |
| Manual rots | text lost | copies, dry storage |

The expansion's pressure is not food or raiders; it is time. Every season without
a school is a season closer to a shelter that cannot read its own instructions.

---

## 31. APPENDIX R — WORKED 360-DAY EDUCATION SCENARIO

**Days 1–30.** Pump failure; Holt ill; no reader. Schoolhouse cleared; Miren
begins with three letters and six children.

**Days 31–90.** Literacy 1 for four pupils; first primer written; Bruch takes two
apprentices; the pump manual is copied at 55% clarity.

**Days 91–150.** Night class begins; twelve adults attend; first exam passes four;
curriculum shifts to health and survival for winter.

**Days 151–220.** Winter classes; manual copying continues; Holt dictates the
pump handover; the archive gains four manual quality rows.

**Days 221–300.** Spring curriculum: agriculture and trade; first journeyman
exam attempted; two pass. Children move bands; new readers join class.

**Days 301–360.** First graduates certify. The shelter has eleven readers, five
manuals of its own, and two certified trades. A master dies and the shelter
counts what survived him.

---

## 32. APPENDIX S — VIGNETTE (TONE SAMPLE)

> Miren writes three letters on the board and says the sound of each one, and
> Hollis repeats them, and the stove ticks, and outside someone is dragging scrap
> and the two sounds fit together the way the shelter fits together.

> Holt sits with Arián for an hour and talks about the pump until his voice goes
> thin, and Arián writes, and crosses out, and writes again, and the page is ugly
> and it is still the most valuable thing the shelter made this month.

> Bruch watches the apprentice file a seam and says nothing, and the apprentice
> files it again, and still Bruch says nothing, and the third time the apprentice
> gets it right and Bruch nods once, which is the tradition.

---

## 33. APPENDIX T — FAILURE MODES AND RECOVERY

| Failure | Effect | Recovery |
|---|---|---|
| No teacher | no classes | recruit, rotate, trade |
| Low attendance | slow literacy | schedule, incentives |
| No paper | no copies | mill, trade, simplify |
| No ink | no authoring | archive ink path |
| Master loss | skill gap | manuals, second master |
| Exam failure | delay | practice, retake |
| Curriculum mismatch | wasted term | review, retarget |
| Library loss | knowledge loss | copies, offsite |
| Child labor pressure | attendance drop | policy, support |
| Illiteracy plateau | study blocked | night class, tutor |

No failure is a game over. The deepest failure is a shelter that owns manuals no
one can read.

---

## 34. APPENDIX U — CONTENT REVIEW CHECKLIST

- [ ] `ApprenticeshipSystem` remains the mentorship authority.
- [ ] `LibraryStudySystem` remains the study authority.
- [ ] `SkillProgressionSystem` remains the only skill authority.
- [ ] No grades gate content beyond real capability.
- [ ] Adults who cannot read are never humiliated.
- [ ] Children's content is restrained and non-sentimental.
- [ ] Manual authoring has real days, defects, and copies.
- [ ] Certification reflects demonstrated competence.
- [ ] Knowledge loss is tragic but not gratuitous.
- [ ] Save additions are additive and legacy-neutral.
- [ ] Determinism uses live tick paths only.

---

## 35. APPENDIX V — GLOSSARY

- **Literacy level** — reading and writing capability.
- **Band** — age group for schooling.
- **Curriculum** — the season's plan of subjects.
- **Lesson** — a single authored class unit.
- **Track** — an apprenticeship trade path.
- **Certification** — demonstrated competence recognized by exam.
- **Manual** — a `ManualDefinition` consumed by study jobs.
- **Clarity** — a manual's comprehension modifier.
- **Scribe** — the person who copies or authors.
- **Handover** — a master's dictated knowledge written down.

---

## 36. APPENDIX W — INTEGRATION MATRIX

| System | Reads | Writes | Never writes |
|---|---|---|---|
| `ApprenticeshipSystem` | pairs, catalogs | progress | skills |
| `LibraryStudySystem` | manuals | study jobs | skills |
| `LiteracySystem` | instruction | literacy levels | skills |
| `CurriculumSystem` | needs, calendar | lesson plan | skills |
| `SchoolingSystem` | bands, aging | attendance | skills |
| `ExaminationSystem` | progress | certifications | skills |
| `ManualAuthoringSystem` | dictation, ink | manuals, quality | study jobs |
| `TeachingRoleSystem` | roster | assignments | skills |
| `SkillProgressionSystem` | — | skills | — |
| `ArchiveDeskSystem` | ink, paper | records | study jobs |
| `DutyRoster` | roster | shifts | education |
| `NeedsSystem` | energy | morale | literacy |
| `TradingSystem` | books | trade | manuals |
| `EpilogueChronicleBuilder` | milestones | chronicle | — |

---

## 37. APPENDIX X — DATA SCHEMA DETAIL (NEW CATALOGS)

**`literacy_levels.json`** — `level_id`, `display_name`, `reading`, `writing`,
`unlocks[]`, `study_hours`, `tags`.

**`curricula.json`** — `curriculum_id`, `display_name`, `season`, `subjects[]`,
`hours_per_week`, `teacher_requirement`, `pupils`, `outcomes[]`, `tags`.

**`lessons.json`** — `lesson_id`, `display_name`, `subject`, `band`,
`materials[]`, `duration_hours`, `teacher_skill`, `outcome`, `tags`.

**`school_bands.json`** — `band_id`, `display_name`, `age_min`, `age_max`,
`subjects[]`, `hours`, `restrictions[]`, `tags`.

**`examinations.json`** — `exam_id`, `display_name`, `trade`, `standards[]`,
`duration_hours`, `examiner_skill`, `pass_bands[]`, `certification`, `tags`.

**`manual_quality.json`** — `quality_id`, `display_name`, `master_skill`,
`scribe_skill`, `days`, `clarity`, `defects`, `tags`.

**`school_roles.json`** — `role_id`, `display_name`, `duties[]`, `fatigue`,
`requirement`, `tags`.

All new catalogs carry `schema_version: 1` and fail the integrity gate on missing
or duplicate IDs, invalid references, or out-of-range numbers.

---

## 38. APPENDIX Y — MEASUREMENT AND TELEMETRY

| Metric | Purpose | Source |
|---|---|---|
| Literacy distribution | capability spread | LiteracySystem |
| Attendance rate | school health | SchoolingSystem |
| Lessons taught | curriculum delivery | CurriculumSystem |
| Apprentice completions | workforce pipeline | ApprenticeshipSystem |
| Certification pass rate | standards | ExaminationSystem |
| Manuals authored | knowledge capture | ManualAuthoringSystem |
| Manual clarity | accessibility | LibraryStudySystem |
| Teaching hours | labor cost | TeachingRoleSystem |
| Unread manuals | literacy gap | LibraryStudySystem |
| Knowledge athletes lost | risk | event log |

Telemetry is diagnostic only; it never gates content and never becomes a hidden
score. It exists so the team can tell whether the school feels like investment or
like homework.

---

## 39. APPENDIX Z — IMPLEMENTATION CHECKLIST

- [ ] Phase 0 premise re-audit recorded with file:line evidence.
- [ ] Phase 1 catalogs authored and registered with validators and scanner.
- [ ] Phase 2 Core systems are pure, engine-free, and deterministic.
- [ ] Phase 3 save additions are additive, legacy-neutral, and round-trip tested.
- [ ] Skills are written only by `SkillProgressionSystem`.
- [ ] Study jobs are written only by `LibraryStudySystem`.
- [ ] Triad parity (Setup / Save / Flush) holds for every new state.
- [ ] Phase 4 host session exposes live state and real commands only.
- [ ] Phase 5 panels pass bind/unbind/rebind, focus, contrast, and scaling checks.
- [ ] Phase 6 content passes the review checklist in §34.
- [ ] Phase 7 soak shows children growing into certified trades.
- [ ] Phase 8 data integrity and content utilization selftests pass.
- [ ] No parallel skill, library, apprenticeship, or save system exists.

---

## 40. APPENDIX AA — OPEN QUESTIONS FOR REVIEW

1. Should literacy be required for all technical study, or only some manuals?
2. Should children's school hours compete directly with workshop shifts?
3. Should a failed exam ever bar a candidate from a trade permanently?
4. Should manual copies degrade on each copy of a copy?
5. Should the library be raidable or stealable by other settlements?
6. Should teachers be exempt from other duty, or rotate through?
7. Should certification be tradeable as a service?
8. Should day-one legacy survivors be assigned authored literacy levels?

None of these may be decided unilaterally; each changes balance and tone.

---

## 41. APPENDIX AB — CROSS-WAVE HOOKS

| Wave | Expansion | Hook |
|---|---|---|
| 1 | 12 The Second Generation | Child cohorts age into school bands |
| 1 | 13 The Faithful | Teachings, hymns, and religious instruction debates |
| 1 | 14 Above the Ash | Flight training and air knowledge |
| 1 | 15 The Deep Root | Agricultural instruction and seed knowledge |
| 1 | 16 The Rebuilt Body | Training users of prosthetics and implants |
| 2 | 17 The Long Evening | Evening classes, reading aloud, culture |
| 2 | 18 The Underneath | Underground instruction and geology lore |
| 2 | 19 The Bitter Air | Hazard training, masks, and drills |
| 2 | 20 The Quiet Hand | Codes, ciphers, and counter-intelligence literacy |
| 2 | 21 The Grid | Electrical and grid apprenticeship |
| 3 | 22 The Clean Flow | Hygiene instruction and water testing |
| 3 | 23 The Alarm | Drill instruction and command training |
| 3 | 24 The Long Goodbye | Palliative teaching and grief literacy |
| 3 | 25 The Iron Road | Rail apprenticeship and inspection |
| 3 | 26 The Common Table | Kitchen apprenticeship and safe food teaching |
| 4 | 27 The Thread | Textile apprenticeship and pattern manuals |
| 4 | 29 The Glass | Glass and optics apprenticeship |
| 4 | 30 The Press | Paper, ink, printing for schools |
| 4 | 31 The Kiln | Masonry apprenticeship and kiln craft |

Each hook is additive. The Lesson can ship alone, and every other expansion can
ship without it.

---

## 42. APPENDIX AC — CLOSING VIGNETTE

> The class bell is a piece of pipe hung from the ceiling, and Miren rings it
> twice a day, and the children come in from the yard with frost on their sleeves
> and slates under their arms.

> Holt finishes dictating and leans back and says that is all of it, and Arián
> looks at the pages and says no, it is not, and Holt laughs and keeps talking.

> The first certificate is a slip of paper with two signatures, and the holder
> folds it into his pocket like it is worth carrying, because it is.

---

## 43. APPENDIX AD — COMMON FAILURE PATTERNS TO AVOID

| Pattern | Why it is bad | Correct approach |
|---|---|---|
| School as menu reward | trivializes teaching | real labor and time |
| Grades as gates | treats people as points | demonstrated competence |
| Literacy as hidden stat | removes capability feel | visible reading levels |
| Manuals as infinite | removes scarcity | authoring, copies, loss |
| Exams as luck | unfair | deterministic standards |
| Children as mascots | tonal risk | real pupils, real lessons |
| Teachers as saints | flat characters | working people |
| Knowledge as research tree | wrong fiction | authored transmission |
| Education as pure buff | no tradeoff | production hours cost |
| Archive as free storage | no risk | fires, theft, decay |

The list exists because education is easy to make either trivial or punitive. The
live systems exist to keep it honest: skills are earned, study takes hours, and
knowledge can be lost.

---

## 44. APPENDIX AE — CONTENT VOLUME SUMMARY

| Category | Rows | Prose estimate |
|---|---|---|
| Literacy levels | 6 | 1,500 |
| Curricula | 8 | 2,500 |
| Lessons | 60 | 12,000 |
| School bands | 4 | 1,000 |
| Examinations | 15 | 4,000 |
| Manual quality | 10 | 2,000 |
| School roles | 5 | 1,500 |
| Apprenticeship tracks | 20 | 5,000 |
| Manual definitions | 30 | 7,000 |
| Quests | 53 | 15,000 |
| NPCs | 8 | 7,000 |
| Locations | 10 | 3,500 |
| Items | 14 | 2,000 |
| Endings | 6 | 3,000 |
| **Total** | | **~69,000** |

---

## 46. APPENDIX AF — SIDE QUEST STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_lesson_read_first` | 3 | Letters, word, sentence |
| `quest_lesson_adult_class` | 4 | Invite, teach, practise, celebrate |
| `quest_lesson_labels` | 3 | List, write, hang |
| `quest_lesson_sign_name` | 3 | Practise, sign, record |
| `quest_lesson_read_aloud` | 3 | Choose, read, listen |
| `quest_lesson_school_repair` | 4 | Assess, repair, furnish |
| `quest_lesson_attendance` | 3 | Track, follow up, adjust |
| `quest_lesson_school_food` | 3 | Plan, cook, serve |
| `quest_lesson_school_stove` | 3 | Fuel, light, maintain |
| `quest_lesson_school_supplies` | 4 | Source, ration, distribute |
| `quest_lesson_numbers` | 4 | Teach, drill, test |
| `quest_lesson_health_class` | 4 | Prepare, teach, demonstrate |
| `quest_lesson_map_class` | 4 | Draw, orient, walk |
| `quest_lesson_civics` | 4 | Choose, discuss, agree |
| `quest_lesson_survival_class` | 4 | Prepare, teach, drill |
| `quest_lesson_find_master` | 5 | Ask, assess, appoint |
| `quest_lesson_apprentice_three` | 4 | Select, pair, start |
| `quest_lesson_pair_trouble` | 4 | Hear, judge, repair |
| `quest_lesson_journeywork` | 5 | Plan, do, present |
| `quest_lesson_master_standard` | 3 | Define, review, hold |
| `quest_lesson_transcribe` | 4 | Dictate, write, check |
| `quest_lesson_simplify` | 5 | Read, rewrite, test |
| `quest_lesson_copy_run` | 4 | Copy, bind, store |
| `quest_lesson_missing_page` | 4 | Notice, search, replace |
| `quest_lesson_write_new` | 5 | Know, draft, refine |
| `quest_lesson_first_exam` | 4 | Prepare, sit, certify |
| `quest_lesson_standard_set` | 4 | Debate, set, publish |
| `quest_lesson_fail_case` | 3 | Assess, advise, retake |
| `quest_lesson_certificate` | 3 | Issue, record, celebrate |
| `quest_lesson_guest_exam` | 4 | Invite, examine, decide |

---

## 47. APPENDIX AG — TEACHING SCHEDULE TABLE

| Time | Band | Subject | Teacher | Room |
|---|---|---|---|---|
| 08:00 | Young | Letters | Miren | Schoolhouse |
| 09:00 | Middle | Numbers | Miren | Schoolhouse |
| 10:00 | Older | Technical | Holt | Workshop |
| 11:00 | Middle | Health | Medic | Clinic |
| 13:00 | Older | Trade | Bruch | Foundry |
| 14:00 | Young | Habits | Miren | Schoolhouse |
| 15:00 | Older | Civics | Steward | Common room |
| 18:00 | Adult | Night class | Miren | Schoolhouse |

Six class hours plus one night class per day. The schedule is a real resource: a
teacher cannot teach and work the same hour, and pupils cannot attend and haul
the same hour. The expansion's cost is entirely inside the live staffing model.

---

## 48. APPENDIX AH — PAPER, INK, AND SUPPLY CHAIN

| Supply | Source | Cost | Used by |
|---|---|---|---|
| Paper | press shop / rag fiber | labor | primers, manuals, records |
| Ink | soot, gum, iron | labor | writing, copying |
| Slate | ruins / trade | low | practice |
| Chalk | lime / trade | low | boards, practice |
| Board | carpentry | med | classrooms |
| Primer | teacher + scribe | days | first readers |
| Binder | leather + thread | med | manuals |
| Lamp | fuel | low | night class |

The school's supply chain crosses the Thread (rag fiber and leather), the Press
(paper and ink), the Kiln (chalk and lime), and the kitchen (oil for lamps). That
interlock is the point: education is a system, not a menu.

---

## 49. APPENDIX AI — KNOWLEDGE DOMAIN TABLE (15 DOMAINS)

| # | Domain | Transmitted by | Manual available | Loss risk |
|---|---|---|---|---|
| 1 | Water | apprentice, manual | yes | high |
| 2 | Power | apprentice, manual | yes | high |
| 3 | Medical | apprentice, manual | partial | high |
| 4 | Food | apprentice, manual | yes | medium |
| 5 | Agriculture | apprentice, manual | yes | medium |
| 6 | Textiles | apprentice, manual | partial | medium |
| 7 | Leather | apprentice, manual | partial | medium |
| 8 | Carpentry | apprentice, oral | no | high |
| 9 | Masonry | apprentice, oral | no | high |
| 10 | Glass | apprentice, oral | partial | high |
| 11 | Radio | apprentice, manual | yes | high |
| 12 | Rail | apprentice, manual | yes | high |
| 13 | Salvage | practice, oral | partial | low |
| 14 | Munitions | apprentice, manual | partial | high |
| 15 | Weather | observer, log | partial | medium |

Fifteen domains, five with no manual at all. The expansion's authored pressure is
that a shelter can see exactly which of its skills has a written backup and which
lives only in one person's hands.

---

## 50. APPENDIX AJ — ADULT LITERACY WORKED SCENARIO

**Week 1.** Eight adults attend after shift. Two stop coming because the day is
too long. Miren begins with names.

**Week 3.** Four can write their names. A fifth can recognize his own.

**Week 6.** The class reads ration labels aloud and laughs at a misreading, and
no one is embarrassed because the room is full of people at the same stage.

**Week 10.** One learner reads the first sentence of a medicine label and the
clinic changes how it labels bottles.

**Week 16.** The class counts as a literacy level for three adults, and the
shelter's records stop depending on one hand.

**Week 24.** Two learners ask about the pump manual. Holt says not yet, and
means it kindly, and writes a simplified page for them.

---

## 51. APPENDIX AK — REGIONAL KNOWLEDGE MAP

| Settlement | Strength | Gap | Trade |
|---|---|---|---|
| The shelter | trades, archive | literacy | teaches trades |
| Market Town | letters, trade records | technical | sells books |
| Fog Ridge Camp | oral skills | no manuals | sells instruction |
| Spring Village | field craft | no school | buys primers |
| Foundry Enclave | technical | teachers | trades manuals |
| Deep Bunker | archives | practice | lends books |
| River Flotilla | charts | formal school | sells maps |
| Coal Stage | fuel craft | reading | buys manuals |

Knowledge is a trade good with a leak problem: a manual sold is a manual shared.
The expansion makes that tension explicit and lets the shelter decide.

---

## 52. APPENDIX AL — LORE: THE TEACHING TRADITION

The fiction:

- **The School Ruins** was a town school; its desks still bear initials.
- **The Campus** was a small technical college; its labs are dangerous and its
  library is partially intact.
- **The Book Market** follows an old customs of lending and copying; the traders
  call it the long shelf.
- **The Scribes' Camp** was a records office; its racks and presses became the
  region's copying house.
- **The Lost Carrel** was sealed by its last occupant, and the door was found
  with a note that said the book was worth more than the room.

No real institution or school is copied. The tradition is generic and local.

---

## 53. APPENDIX AM — ENDING PROSE SKETCHES

**The Open School.** Every child reads, every trade has a second pair of hands,
and the archive grows by three manuals a year; the shelter no longer depends on
any single mind.

**The Trade Track.** Certificates become the shelter's export, and young workers
leave for a season and come back with news and prices.

**The Written Store.** The manuals sit in a dry room in two copies each, and the
old masters are allowed to rest, and the shelter reads its own instructions.

**The Half Day.** School and workshop share the daylight, and both are a little
hungry, and neither is starved.

**The Empty Classroom.** Production wins, the school closes, and a season later
no one can repair the pump without Holt, and Holt is tired.

**Fade.** Classes happen when they happen; the primer is half written; the
shelter manages.

---

## 54. APPENDIX AN — OPEN IMPLEMENTATION NOTES

- Literacy should live as a survivor attribute resolved through the existing
  survivor store, not a parallel roster.
- The live `ManualDefinition` schema must stay the single manual shape; authoring
  produces rows in the same schema.
- `IsManualPowered` should gate a real power socket in the library room rather
  than a flag.
- Apprenticeship tracks should extend `MentorshipDef`, not replace it.
- Exam results should be recorded in `ApprenticeshipState` or
  `ApprenticeshipSaveStore` sub-objects.
- The night class must respect sleep and fatigue from the live needs model.
- Child bands must read aging from the live generational path; no second clock.
- Teaching roles must register with the live duty roster so the shelter can
  actually assign them.

---

## 55. APPENDIX AO — CONTENT REVIEW: SENSITIVE TOPICS

| Topic | Risk | Handling |
|---|---|---|
| Adult illiteracy | Shame | Normalize; class is mixed-ability |
| Child labor | Exploitation | Authored policy debate with dignity |
| Corporal punishment | Abuse | Absent; discipline is restorative |
| Death of a master | Grief | Count what was saved |
| Failed exam | Shame | Informative, retakable |
| Class and access | Inequality | Open hearings and policy |
| Knowledge theft | Betrayal | Authored, not comic |
| Learning disability | Stereotype | Not modeled; skill is effort |
| Religious instruction | Real faiths | Fictional and optional only |
| Propaganda in class | Manipulation | Explicitly debated, never rewarded |

The schoolhouse touches real sensitivities, and the expansion's rule is simple:
education is treated the way the shelter treats water and power — as essential,
fragile, and worth arguing about honestly.

---

## 56. APPENDIX AP — MEASUREMENT AND REVIEW CADENCE

| Gate | Question | Evidence |
|---|---|---|
| Premise | Do live owners still match? | file:line audit |
| Data | Are all rows valid and reachable? | integrity + scanner |
| Core | Are systems pure and deterministic? | unit tests |
| Persistence | Does legacy load neutral? | round-trip tests |
| Host | Is every command real? | selftest + journey |
| UI | Is literacy honest? | lifecycle + a11y tests |
| Tone | Is the school humane? | content review |
| Balance | Is the tradeoff real? | 360-day soak |

---

## 57. APPENDIX AQ — CLASSROOM EQUIPMENT TABLE

| Equipment | Inputs | Capacity | Room | Effect |
|---|---|---|---|---|
| Bench | wood, nails | 4 pupils | schoolhouse | seats |
| Board | wood, paint | class | schoolhouse | instruction |
| Stove | metal, fuel | room | schoolhouse | warmth |
| Slate set | slate, cloth | 12 | schoolhouse | practice |
| Lamp | metal, oil | 6 | any | night class |
| Shelf | wood | 40 books | library | storage |
| Locker | metal, lock | 20 | library | security |
| Desk | wood | 1 scribe | scribing | copying |
| Press stand | wood, iron | 1 press | press shop | printing |
| Exam table | wood | 1 candidate | exam room | assessment |
| Map table | wood, canvas | 4 | schoolhouse | map class |
| Bell | pipe, cord | shelter | schoolhouse | schedule |

The equipment list is short on purpose. A schoolhouse needs a room, a board, and
a teacher; everything else is the shelter deciding how seriously it takes the
lesson.

---

## 58. APPENDIX AR — ATTENDANCE AND INTERRUPTION MODEL

| Condition | Attendance | Cause | Recovery |
|---|---|---|---|
| Normal | full | none | n/a |
| Harvest | reduced | labor need | make-up days |
| Illness | partial | outbreak | resume after |
| Cold snap | partial | room cold | stove, blankets |
| Alert | cancelled | incident | resume after |
| Mourning | reduced | death | gentle return |
| Workshift surge | reduced | production | schedule shift |
| Teacher ill | cancelled | fatigue | substitute |

Attendance is a live number the player can see and influence, and every
interruption has a natural in-fiction cause and recovery. The school bends with
the shelter instead of breaking against it.

---

## 59. CLOSING STATEMENT

ASHFALL already tracks mentorships, apprentice progress, wills, mentor death,
manual study, comprehension, and completion with two real systems, two hosts,
two save stores, and two panels. What it lacks is the classroom, the primer, the
literacy level, the exam, and the written page that outlives the hand that wrote
it. The Lesson adds that world without adding a second skill system or a second
library. It adds a child reading aloud, a master writing down what only he knew,
and a shelter that finally understands maintenance applies to minds as well as
pumps.

> Wave 4 note: this plan is one of five Wave 4 expansion bibles (27–31). Each is
> self-contained; none requires another to ship. The shared Wave 4 index lives at
> `docs/expansions/wave4/WAVE4_INDEX.md`. The safe pre-signature step is Phase 1
> (data schemas and validators), which is additive and reversible. Evidence
> anchors: `ApprenticeshipSystem` (`MentorshipDef`, `NotifyMentorDeath`,
> `TranscriptionTask`, `SurvivorWill`), `LibraryStudySystem` (`ManualDefinition`,
> `StudyJob`, `IsManualPowered`), `ArchiveDeskSystem`, `apprenticeship_catalog.json`
> (1.7 KB), `library_manuals.json` (21.9 KB), `lost_tech_manuals.json` (23.5 KB),
> and `education_session_records.json` (32 KB).