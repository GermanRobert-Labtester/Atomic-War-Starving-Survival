# Feature / Task Plan: The Sick Room (the culture of the vigil) & The Birthday Book (dates for the living)

<!-- UPON INTEGRATION (MANDATORY):
Prepend header at the top saying:
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**
And immediately move this file to .ai/plans/integrated/<category>/
-->

STATUS: DRAFT — awaiting user approval (not self-approved; CLAUDE.md Rule 8 requires `STATUS: APPROVED BY USER` before any code commit). Story batch 5, plans 1 of 4 (subjects 49–50).

> **Subjects covered (2 of the 8 in the "counted days and tended edges" batch):**
> 49. **The Sick Room** — the vigil: who sits with the ill, what the room keeps, and the hours
> no roster records. (Prefix `SR`.)
> 50. **The Birthday Book** — dates for the living: arrivals, name-days and birthdays in one
> plain book, and one date with no owner. (Prefix `BB`.)
>
> **Prose / design companions that already exist and are extended, not replaced:**
> `docs/plans/UNBLOCK_EXPANSION38_THE_WARD_INTEGRATION_PLAN.md` (clinical triage and sterile
> supply — the medicine), `docs/plans/PLAN_24_CLOSEOUT.md` (the medical journey — the system),
> `DiseaseSystem` (infection mechanics), `.ai/plans/evenings-and-memory-work-2026-09-29.md`
> (remembrance of the dead; customs), `.ai/plans/the-wardrobe-and-the-night-shift-2026-09-29.md`
> (the ward's night line), Year Two's Rite of Passage (children only).
>
> **Not a claim.** This plan claims no path in `WORKTREE_OWNERSHIP.md`, edits no source, no data,
> no ledger and no other plan. It is documentation only. Paths below are *proposed*; `INT` marks
> integrator-owned shared seams.

> **Editorial note (prose texture).** Sections **0**, **1b**, **1c** and **12** carry story texture
> and voice. Sample lines are content candidates for `*.json` rows; they belong in data, never in
> code. No authority, path, decision, acceptance criterion or verification step is changed by any
> prose section.

---

## 0. Prologue — The Chair by the Bed

> *"Medicine is what the ward does. The vigil is what the household does. Only one of them has a
> roster."*

The sick room is two rooms in one: the clinical half, which belongs to the ward and its
protocols, and the chair half, which belongs to nobody and everybody. The chair half is this
plan. Someone sits with the ill through the hours that no treatment requires — mending in hand,
or nothing in hand — and the sitting is recorded the way the shelf records its hooks: in
aggregate, in format, without names.

The birthday book is the same attention pointed at well people. One book by the muster board:
arrivals, name-days, birthdays, in a plain column of dates. The wall is for the dead and the book
is for the living — the two ledgers are deliberately never cross-referenced, because grief and
gladness share a calendar and must not share a page. On the book's third page there is a date
with no name against it, kept, counted, and unclaimed.

**Tone & register.** Night-quiet and calendar-plain. The sick room's vocabulary is *sat, hours,
hand, quiet, turned*; the book's is *date, kept, marked, noted, shared*. Prose for the vigil
should read like the inside of a night that is going well; prose for the book like a household
almanac with the jokes left in. Never clinical in the vigil; never maudlin in the book.

**Mystery & texture.** Two silences hold the pair. The vigil ledger records *hours sat* and the
handovers record *nothing to note*, and between two such handovers a patient is recorded as
having recovered — nobody witnessed the turn, and the format has no column for witnessing (§12).
And the book's third page carries a **date with no owner**: kept every year, marked, and claimed
by no one living or dead (§12). Neither is a puzzle. Both are what happens when care and
calendar outlast their record-keepers.

**The second layer.** The vigil and the book are the two directions of household attention: one
looks *at* someone who is suffering, one looks *ahead* to someone who will be celebrated. Both
are acts of scheduling love — the willingness to spend hours and days on a person with no
deliverable attached. The plan's quiet thesis is that a shelter is measured by what it is willing
to do for one person at a time, and both of these systems exist to make that willingness
*legible without making it a task*.

---

## 1. Goal & Outcome

> *Design intent: the player should glance at the vigil ledger and see care happening — and open
> the birthday book and see the year's gladness laid out in a column of dates.*

### 1.1 The Sick Room (SR)

- **Goal:** A **Vigil Ledger** (additive nested DTO — no new save section) recording *hours sat*
  per patient-day in aggregate (patient id, hours, `nothing_to_note` count) with **no sitter
  names** (DEC-SR-03); one optional **Vigil** duty through the existing duty-roster seam whose
  only effects are existing positive-only stress relief through `SurvivorMentalHealthSystem`'s
  public call (DEC-SR-01); one authored **Sick-Room Rows** table for room-level facts (the chair,
  the window, the water jug — physical constants of the room); and a plain **Room Read** on the
  existing ward/board surface.
- **Outcome (observable):** on a fixed seed, a patient-day with a vigil duty accrues hours in the
  ledger; the stress effect is applied exactly once per day through the owner's public method;
  `nothing_to_note` counts accrue and render plainly; the room's facts render with the ward's own
  status beside them; with no vigil rows every owner behaves identically to today; save/load
  mid-night round-trips.
- **Non-Goals:** no medical mechanics (The Ward, Plan 24 and `DiseaseSystem` keep their
  meanings); no healing bonuses of any kind (DEC-SR-02); no sitter attribution; no death or
  grief mechanics (Memory Work owns them); no new save section; no new routed panel; no Unity.
- **"Done":** §6 acceptance passes via `bin/run-scoped-tests`; ship-dark parity holds; handoff
  lists untouched shared paths.

### 1.2 The Birthday Book (BB)

- **Goal:** A **Date Book** (closed table, ≤ 40 rows: date, subject-role or `unclaimed` flag,
  kind (`arrival` | `name-day` | `birthday`), `kept_count` of years marked) surfaced beside the
  muster board; one derived **Book Read** (whose date is next, whose is passed, what is marked);
  and one constrained verb — **Mark** (a date may be marked for its day: one line, one custom
  hook routed through existing owners only — a meal, a shared hour, nothing mechanical). Dates
  for the dead belong to the memorial and are refused by the format (DEC-BB-02).
- **Outcome (observable):** on a fixed seed, the book renders every authored date with its kind
  and years-kept; the unclaimed date renders with its `unclaimed` mark and a kept_count like any
  other; `Mark` writes one line and routes one existing-owner effect; with no rows every surface
  behaves identically to today; save/load round-trips.
- **Non-Goals:** no age model (ages are not tracked and must not be inferred — DEC-BB-03); no
  gift/economy mechanics; no festival machinery (EV owns customs; the Founding Day stays its
  register's); no children mechanics (Year Two owns the Rite); no new save section; no new routed
  panel; no Unity.
- **"Done":** §6 acceptance passes; ship-dark parity holds; handoff lists untouched shared paths.

### 1.3 The seam between them (X)

- **Goal:** One documented boundary and one optional hook: a marked date may render one vigil
  line if its subject is currently a patient ("date kept — quietly, in the sick room"), and a
  vigil night may note a date's eve. One string each way, both shipped dark.

---

## 1b. Texture, Mystery & Voice

**The format has no column for witnessing.**

The vigil ledger is deliberately aggregate: hours, counts, `nothing_to_note`. What it cannot
record is the thing that actually matters — the moment somebody's breathing changed while
someone else was looking. The prose must live in that gap: describe the chair, the light, the
hours, and let the record's flatness do the grieving.

**A date is a promise that the year repeats.**

The book's power is the *kept_count*: a date kept four years running is the household's smallest
tradition and its most reliable. Write the unclaimed date with exactly the same dignity as every
other row — the flatness is the format's fairness and the mystery's engine.

**What the player is never told.**

- Who sat the unrecorded hours. The ledger keeps aggregates (DEC-SR-03); the sitter is not in
  the format and must never be added.
- What happened between the two `nothing_to_note` handovers. The recovery is dated; the moment is
  not (§12). The format declines to invent a witness.
- Whose date the unclaimed row is. `unclaimed` is a status, not a hint; the book keeps the day
  and refuses the person (§12).
- Why the wall and the book are never cross-referenced. DEC-BB-02 is a boundary with teeth; the
  calendar is shared and the pages are not.

**Voice — sample fragments (content candidates for `sick_room_lines.json` / `birthday_book_lines.json`).**

> "Vigil ledger: hours sat 6. Notes: nothing to note. 'Nothing to note' is the best entry in
> this book and everyone who keeps it knows so." — night book (SR)

> "The chair faces the bed and the window both. You can watch a person and the weather at the
> same time from it, which is how the chair got its place." — room read (SR)

> "Handover 03:00 to 07:00: nothing to note. Handover 07:00: the fever broke at some point
> between. Nobody wrote down when, because the format has no column for when-it-was-watched."
> — night book (SR)

> "Book, third page: date kept, four years. No name. The household marks it anyway, which is
> the whole of what the book knows about it." — date book (BB)

> "Arrival, day 44: marked. The book counts years, not ages. The book has opinions about
> curiosity." — date book (BB)

**Design texture beats.**

- **No healing bonuses, ever** (DEC-SR-02). The vigil is care, not medicine; the moment it
  affects outcomes it becomes a buff and the chair empties.
- **Aggregates only in the ledger** (DEC-SR-03). Hours and counts are the whole record; the
  human moment stays in prose.
- **The unclaimed date is rendered exactly like the others** (DEC-BB-04). Flatness is fairness
  and mystery at once.
- **Marks route to existing owners only.** A birthday effect is a meal or a shared hour through
  verbs that already exist — never a new mechanic.

---

## 1c. The Deeper Layer — objects & held silences (second prose pass)

*(Second prose pass, non-contractual: narrative texture and writing guidance only — no authority,
no claimed path, no acceptance criterion, no verification step. §12's register is unchanged; the
fragments below are content candidates and deliberate silences, not new recorded questions. Where
a named data file holds no prose field, fragments are texture only and gain no schema.)*

**What the chair half leaves lying around.**

> "Water jug, refilled at 02:00 and 05:00. The refill times are older than the current rota and
> the rota keeps them anyway."

> "Mending in the chair's basket: a sock, half-darned, abandoned mid-row when the breathing
> changed. It is finished now. The finishing is not recorded anywhere."

> "Window, cracked open regardless of weather. Somebody's rule for air. The rule has no author
> and has never been discussed."

**What the book leaves lying around.**

> "Third page, middle row: date kept, five years. The ink of the first marking is different from
> the ink of the fifth. The book notices nothing; the book only counts."

> "Name-day card, slid between the pages: 'from the people on the night side.' The night side is
> not a roster; it is a name the house uses."

> "Page corner, dog-eared at the next date. The dog-ear moves around the book at the speed of
> the year." — texture only

**Held silences (texture, not register rows).**

- Whether the unclaimed date is a birthday or a death-day kept wrongly. The kind column says one
  of three living things (DEC-BB-02); the anomaly is the row's and the plan declines to fix it.
  Texture only.
- Who started the 02:00 and 05:00 refills. The times predate the rota (§1c); the water is kept
  and the keeper is not modelled.

---

## 1.4 Worked examples (non-normative)

**A vigil night (fixed seed).**

> Night 214 — patient 3 (fever, day 2): the `Vigil` duty is staffed by role `night-side`. The
> ledger accrues hours sat 6, `nothing_to_note` ×2. At 03:00 the fever breaks; the 03:00–07:00
> handover records "nothing to note" and the morning line records the break as a fact of the
> night, not a moment. The positive-only stress effect applies once, through
> `SurvivorMentalHealthSystem`'s public call. The sitter appears nowhere in any record.

**A kept date (fixed seed).**

> The third page's unclaimed date arrives: rendered with its `unclaimed` mark and kept_count 5 in
> the same plain type as the four named rows before it. The household `Mark`s it as it always
> does: one line — "date kept, quietly" — and one existing-owner effect (a shared meal through
> the kitchen's own verb). If its subject is a current patient, one string renders in the vigil
> book: "date kept — quietly, in the sick room."

---

## 2. Evidence table (checked against the corpus 2026-09-29; re-verify at P0 — Rule 7)

| # | Claim | Evidence | Status |
|---|---|---|---|
| E1 | `SurvivorMentalHealthSystem.AddStress` (positive-only) is the sanctioned stress seam; public APIs only. | `.ai/plans/the-deep-2026-09-29.md` E11 (`Needs/SurvivorMentalHealthSystem.cs` L130) | LIVE |
| E2 | The Ward owns clinical triage and sterile supply; Plan 24 the medical journey; `DiseaseSystem` infection rules untouched. | `docs/plans/UNBLOCK_EXPANSION38_THE_WARD_INTEGRATION_PLAN.md`; `docs/plans/PLAN_24_CLOSEOUT.md` | LIVE (closeouts) |
| E3 | Night-side roles and handover lines exist (batch-4 NS-P1); role-only attribution precedent. | `.ai/plans/story-expansion-batch-4/the-wardrobe-and-the-night-shift-2026-09-29.md` | PROPOSED (plan is DRAFT) |
| E4 | Memory Work owns remembrance of the dead (memorial wall, four minutes, the tally); the wall recopies (RK DEC-RK-06). | `.ai/plans/evenings-and-memory-work-2026-09-29.md`; `.ai/plans/record-keepers-2026-09-29.md` | PROPOSED (plans are DRAFT) |
| E5 | Custom machinery hosts authored customs routed through existing verbs (EV seam; Mending Day / Wood Day precedents). | batch-3/4 custom hooks | PROPOSED (soft dependency) |
| E6 | Roster duties can be authored as posts (Vigil; precedents: Rounds, link crew, bell duty). | batch-3/4 duty posts | LIVE (per corpus) |
| E7 | Kitchen/meal effects route through the Common Table's owners (one food authority — C1[4]). | `docs/plans/C1_planintegration[4].md` | LIVE (plan exists) |
| E8 | Whether an additive aggregate DTO (hours, counts) survives checksum in the ward/roster save owner. | codec / snapshot tests | **VERIFY (P0)** |
| E9 | Muster-board render point for the date book; board-band precedents. | batch-3/4 render precedents | **VERIFY (P0)** |

## 3. Authority table

| Concern | Owner (unchanged) | This plan adds |
|---|---|---|
| Medicine, triage, disease | The Ward / Plan 24 / `DiseaseSystem` | nothing; the vigil is not medicine |
| Stress | `SurvivorMentalHealthSystem` | one positive-only call per vigil day, through its public method |
| Night staffing | Night Shift (batch 4) | one optional duty role; handover voice unchanged |
| The dead | Memory Work / Record Keepers | nothing; the book refuses death-dates by format |
| Customs/meals | EV / one food authority | one optional `Mark` hook routed through existing verbs |
| Vigil/book state | — | `VigilLedger` + `DateBook` (pure Core; row facts only) nested additively — **DEC-SR-04 / DEC-BB-01** |

## 4. Claimed Paths (proposed; `INT` = integrator-owned)
**Core:** `Assets/Ashfall.Core/Care/VigilLedger.cs` (new, pure), `Social/DateBook.cs` (new, pure)
**Data:** `sick_room_rows.json`, `birthday_book_dates.json`, `sick_room_lines.json`, `birthday_book_lines.json`
**Host:** roster host session (`INT`), stress call (`INT`), muster-board surface (`INT`)
**Presentation:** a "Vigil" band beside the existing ward surface; a "Dates" band beside the muster board — no new routed panel
**Tests:** `Ashfall.Core.Tests/Care/VigilLedgerTests.cs`, `Social/DateBookTests.cs`, `Ashfall.Core.Tests/Save/VigilDateBookSaveTests.cs`

## 5. Packages

### SR-P0 — Premise audit (Auditor; read-only): close E8–E9; confirm the positive-only call shape with the stress owner; confirm the date format refuses death-dates by construction.
### SR-P1 — Vigil ledger + rows (Core + data): aggregates only, `nothing_to_note`, room facts, validator row-level. **Accept:** no sitter field exists anywhere (§6.3); determinism; round-trip.
### SR-P2 — Vigil duty + room read (host): one stress call per day; plain rendering. **Accept:** no healing effect exists anywhere (§6.4).
### BB-P1 — Date book + rows (Core + data): closed kinds, `unclaimed`, kept_counts, Mark verb. **Accept:** death-dates refused by format; no age fields; determinism.
### BB-P2 — Book band + custom hook (host, dark): existing-owner effects only. **Accept:** one line and one existing verb per Mark.
### X-P1 — Seam hooks (host, dark): one string each way between the two halves. **Accept:** no cross-reads.

## 6. Acceptance criteria
1. Core authority + host owner + event path + persistence + observable outcome agree (CLAUDE.md "integrated").
2. Ship-dark parity: no vigil rows and no date rows → ward, disease, stress, roster and board outputs identical on a saved corpus.
3. Attribution invariant: the vigil ledger contains no sitter field of any kind (DEC-SR-03).
4. No-medicine invariant: the diff contains no healing, recovery or disease effect of any kind (DEC-SR-02).
5. Boundary invariant: the date book's format refuses dates for the dead and carries no age fields (DEC-BB-02, DEC-BB-03).
6. Determinism: identical ledger aggregates, kept_counts and renders on replay (`CampaignStreamIds` fork; never `System.Random`).
7. Save round-trip mid-vigil and mid-year; old saves load; checksum-safe (E8).
8. The registered silences of The Ward, Memory Work, Record Keepers and EV stay unanswered (§7).

## 7. Cross-plan boundaries
- **The Ward / Plan 24 / DiseaseSystem:** all medicine is theirs; the vigil's only effect is one positive-only stress call per day and nothing else.
- **Memory Work / Record Keepers:** the wall is for the dead; the book is for the living; the two ledgers are never cross-referenced (DEC-BB-02).
- **Night Shift (batch 4):** the ward's night line is its record; the vigil ledger aggregates beside it and never merges.
- **Year Two:** children's dates may appear as `arrival` rows only; the Rite of Passage stays the umbrella's.
- **EV / Founding Day:** festival customs stay EV's; a marked date is one household hour, never a festival.

## 8. Decision register (proposals — unsigned)
| ID | Decision | Type | Proposed |
|---|---|---|---|
| DEC-SR-01 | The vigil's only mechanical effect is one positive-only stress call per patient-day. | rule | Yes |
| DEC-SR-02 | No healing, recovery or disease effect of any kind. | rule | Yes |
| DEC-SR-03 | Aggregates only: no sitter field exists in any schema. | tone/rule | Yes |
| DEC-SR-04 | The vigil ledger nests additively; no new save section. | architecture | Yes; confirm in P0 |
| DEC-BB-01 | The date book is a closed table of living dates with kept_counts. | architecture | Yes |
| DEC-BB-02 | The format refuses dates for the dead; the wall and the book never cross-reference. | boundary | Yes — **needs canon note** |
| DEC-BB-03 | No ages are tracked or inferable; the book counts years, not ages. | tone | Yes |
| DEC-BB-04 | The unclaimed date renders exactly like every other row. | tone | Yes — **needs canon note** |
| DEC-X-01 | Both seam hooks ship dark; one string each way. | architecture | Yes |

## 9. Pre-flight Checks
- [ ] Go config/save validator passes (`bin/validate-config` / `bin/ashfall-dev validate-config`)
- [ ] No equivalent existing system found (search `Vigil`, `Birthday`, `DateBook`, `SickRoom`, `Sitting`)
- [ ] Premise re-verified (Rule 7); The Ward, Memory Work, RK and EV registers re-read; their silences listed in the handoff as untouchable
- [ ] Signed decisions in hand (at minimum DEC-SR-02, DEC-BB-02)

## 10. Verification
- [ ] `bin/run-scoped-tests` on new tests + existing stress, ward and roster tests (list from P0 selector)
- [ ] Stress call selftest with vigil rows on and off (VERIFY args)
- [ ] Scoped only (<30 s each); ≤10–15 test-edit steps per failure, then auto-flag in `.ai/state.md`; **no full suite** without `RUN FULL TESTS`
- [ ] Handoff in `AI_AGENT_WORKFLOW.md` format; `.ai/state.md` updated

## 11. Stop conditions (Rule 10)
Stop and report if: a healing effect would be required by any surface or hook; a sitter field would be needed to persist the ledger; the date format could accept a death-date or an age; any path overlaps a live claim.

## 12. Open Mysteries & Deliberate Silence (lore register — no authority, no claimed path)

These questions are **intentionally unanswered**. They are not bugs, not TODOs, and not hidden
work items. They keep the chair facing two directions and the book open at a page with no name.
Any future plan that answers one must name the decision that permits it.

| # | Question | Why it stays open | Who may answer it (later, signed) |
|---|---|---|---|
| SR-OM-1 | Who sat the unrecorded hours? | Aggregates only by construction (DEC-SR-03); the sitter is not in the format. | Never — deliberately sealed. |
| SR-OM-2 | When exactly did the fever break? | Two `nothing_to_note` handovers and a recovery between them (§12); the format has no column for witnessing. | Never — a rule, not a gap. |
| SR-OM-3 | Who started the 02:00 and 05:00 refills? | The times predate the rota (§1c); the water is kept and the keeper unmodelled. | Never — texture by omission. |
| SR-OM-4 | Why does the window stay cracked? | Somebody's rule for air (§1c); the rule has no author and the plan declines to draft one. | Never — texture by omission. |
| SR-OM-5 | Is the vigil half of the room even the same room? | The chair half "belongs to nobody" (§0); the ward's jurisdiction stops at the bed and the plan never says so out loud. | The Ward's owner, if ever asserted. |
| BB-OM-1 | Whose date is the unclaimed row? | `unclaimed` is a status, not a hint (DEC-BB-04); the book keeps the day and refuses the person. | Never — deliberately sealed. |
| BB-OM-2 | Is it a birthday kept wrongly? | The kind column is closed (§1c); the anomaly is the row's and the plan declines to fix it. | Never — tone-locked. |
| BB-OM-3 | Who marked it the first year? | Five years kept, two inks (§1c); the first hand is unrecorded and inherits the Guest Book's first-hand silence. | Never — the pair stays unresolved (mystery-index §3 discipline). |
| BB-OM-4 | Why does the book count years and not ages? | DEC-BB-03 is a design rule; the household's disinterest in ages is not modelled. | Canon owner only, as a signed decision. |
| BB-OM-5 | Does the wall know about the book? | The ledgers never cross-reference (DEC-BB-02); whether the dead's dates are kept elsewhere is not asserted and must not be. | Never — a rule, not a gap. |
