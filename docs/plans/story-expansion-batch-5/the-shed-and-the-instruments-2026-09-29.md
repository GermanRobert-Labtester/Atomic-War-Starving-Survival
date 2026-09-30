# Feature / Task Plan: The Shed (tool custody and the workshop's folk ownership) & The Instruments (measures and who keeps them honest)

<!-- UPON INTEGRATION (MANDATORY):
Prepend header at the top saying:
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**
And immediately move this file to .ai/plans/integrated/<category>/
-->

STATUS: DRAFT — awaiting user approval (not self-approved; CLAUDE.md Rule 8 requires `STATUS: APPROVED BY USER` before any code commit). Story batch 5, plans 3 of 4 (subjects 53–54).

> **Subjects covered (2 of the 8 in the "counted days and tended edges" batch):**
> 53. **The Shed** — where the household's hands live: tool homes on hooks and benches, folk
> custody, and the workshop's quiet claim on everyone's time. (Prefix `SH`.)
> 54. **The Instruments** — the shelter's measures: the scale, the barometer, the clock — and who
> keeps them honest. (Prefix `IN`.)
>
> **Prose / design companions that already exist and are extended, not replaced:**
> `.ai/plans/the-loan-shelf-and-the-wind-names-2026-09-29.md` (loans circulate; the shed is where
> they *live*), `.ai/plans/the-mending-and-the-second-language-2026-09-29.md` (mark-counts on
> tools), `docs/plans/PLAN_B75_BALLISTICS_WORKBENCH_CLOSEOUT.md` and
> `docs/plans/PLAN_129_FOUNDRY_PRODUCTION_CLOSEOUT.md` (benches and foundries — the industrial
> owners), `.ai/plans/the-deep-2026-09-29.md` E10 (`DosimeterCalibrationSystem` — calibration
> mechanics are theirs), `.ai/plans/the-sky-2026-09-29.md` E8b (instruments that disagree).
>
> **Not a claim.** This plan claims no path in `WORKTREE_OWNERSHIP.md`, edits no source, no data,
> no ledger and no other plan. It is documentation only. Paths below are *proposed*; `INT` marks
> integrator-owned shared seams.

> **Editorial note (prose texture).** Sections **0**, **1b**, **1c** and **12** carry story texture
> and voice. Sample lines are content candidates for `*.json` rows; they belong in data, never in
> code. No authority, path, decision, acceptance criterion or verification step is changed by any
> prose section.

---

## 0. Prologue — The Hand and the Eye

> *"Tools are hands that stay put. Instruments are eyes that do not blink. A shelter is judged
> by where it keeps both."*

The shed is not a building; it is a *wall of homes*. Every tool has a hook, a bench, a drawer —
a place it returns to and is expected at. The Loan Shelf lends; the shed *keeps*. Its ledger is
the household's most domestic map: the hammer that lives second from the left, the good
screwdriver that lives in the tin and not on the board, the saw that has hung in the same spot
long enough that the spot is lighter than the wall. Ownership here is folk ownership: the tools
are the shelter's, their *places* are somebody's.

The instruments are the shed's other half — the eye instead of the hand. The scale by the store,
the barometer by the window, the clock that the bell answers. Instruments do not measure the
world; they measure the *distance between people's claims about the world*, and keeping them
honest is a household job like any other: checked, dated, and — when two of them disagree —
recorded as disagreeing. The dosimeter's lesson is the plan's motto: two readings prove an
instrument is consistent. They do not prove it is honest.

**Tone & register.** Workshop-plain and metrological. The shed's vocabulary is *hook, home, out,
back, worn*; the instruments' is *checked, dated, agreed, disagreed, out of true*. Prose should
read like a tool wall annotated by the people who use it and a calibration sheet kept by someone
who enjoys being the last line of defence.

**Mystery & texture.** Two silences hold the pair. One tool's home **predates the shed** — the
hook is older than the wall it is in, and the tool has been second-from-the-left since before
anyone's memory (§12). And one instrument is **out of true the same way every time** — it
disagrees with the others by a constant, is checked anyway, and is kept (§12). Neither is a
puzzle. Both are what happens when a household keeps its hands and eyes longer than it keeps
records.

**The second layer.** A tool's home and an instrument's zero are the two smallest acts of
civility in a shelter: the promise that things have places and that measurements have reasons.
The shed teaches that work is a relationship (the saw has a spot; the spot has a saw); the
instruments teach that truth is a *practice* (checked, dated, argued with). The old hook and the
constant error are the two things this household maintains without understanding, which is —
again — close enough to the definition of a tradition.

---

## 1. Goal & Outcome

> *Design intent: the player should look at the tool wall and see the household's habits — and
> look at the check-sheet and see the household's honesty.*

### 1.1 The Shed (SH)

- **Goal:** A **Shed Table** (≤ 24 rows: tool id, home (`hook_n`, `bench`, `tin`), `home_older`
  flag for the pre-shed hook) and a derived **Shed Read** over the inventory and loan seams: which
  homes are filled, which are out, how long each tool has been out *as the Loan Shelf already
  records it*, and each tool's mark-count read-only from the Mending plan. One optional custom
  hook (evenings-and-memory-work seam): a **Sharpening Hour** whose only effects are existing
  repair verbs (Mending Day precedent). The shed owns no stock and adds no repair verb
  (DEC-SH-01).
- **Outcome (observable):** on a fixed seed, each tool renders its home and state; an out tool
  renders "out N days" borrowing the Loan Shelf's plain arithmetic; the `home_older` hook renders
  its flag in the same type as every other row; mark-counts render read-only beside their tools;
  with no rows every owner behaves identically to today; save/load round-trips.
- **Non-Goals:** no tool stats, durability or quality model (The Mending's boundary: repair verbs
  and condition are owners'); no crafting or production (foundry/workbench owners keep their
  meanings); no ownership transfer; no new save section; no new routed panel; no Unity.
- **"Done":** §6 acceptance passes via `bin/run-scoped-tests`; ship-dark parity holds; handoff
  lists untouched shared paths.

### 1.2 The Instruments (IN)

- **Goal:** An **Instrument Register** (≤ 12 rows: instrument id, keeper-role, check-cadence,
  `disputed` flag with the disagreeing partner id and constant offset, `unitless` flag for
  measures that count in marks or heights) and a derived **Check Sheet** (checked-day per
  instrument, agreed/disagreed state, streak of agreement); one optional duty — **Keeper**
  (existing roster seam; the only effect is the check's history fact). Calibration mechanics stay
  with their owners (DEC-IN-02): the register records *checks and disagreements*, never
  corrections.
- **Outcome (observable):** on a fixed seed, the check sheet renders each instrument's last check,
  keeper-role and agreement state; a `disputed` pair renders both readings in equal type with the
  constant offset stated plainly; a `unitless` instrument renders its marks without units; a
  missed check renders "unchecked, one cycle" with no penalty; with no rows every owner behaves
  identically to today; save/load round-trips.
- **Non-Goals:** no calibration mechanics (`DosimeterCalibrationSystem` keeps its meaning); no
  weather or measurement effects; no new units, no conversions; no metrology of the before; no new
  save section; no new routed panel; no Unity.
- **"Done":** §6 acceptance passes; ship-dark parity holds; handoff lists untouched shared paths.

### 1.3 The seam between them (X)

- **Goal:** One documented boundary and one optional hook: a tool's shed row may cite the
  instrument that checked it ("saw — set by the good square, day 44") as one read-only string; and
  an instrument's check line may cite the shed's sharpening custom. One string each way, shipped
  dark.

---

## 1b. Texture, Mystery & Voice

**A home is a boolean with a memory.**

The shed read's poetry is *place*: filled/out, second-from-the-left, the tin. Prose should treat
the tool wall the way the Cabinet treats its shelf — plainly, materially — with the one
difference that matters again: these objects are working. The shed is the Cabinet's working
cousin, twice removed from the wardrobe.

**A disagreement is a reading, not a verdict.**

The `disputed` pair renders both instruments in equal type with their constant offset — exactly
the discipline of the wind-names' disputed rows and the catalogue/evaluator conflict in The Sky.
The corpus's ethics of disagreement rhyme across three plans; this plan keeps the rhyme.

**What the player is never told.**

- Who hung the older hook. It predates the wall it is in (§12); whether it came from another
  building is not modelled and must not be.
- What the unitless instrument measures. It counts marks or heights; what is being counted is
  not in the vocabulary and must never be added (§12).
- Why the out-of-true instrument is kept. Replacement is never proposed; the household checks it
  anyway, and the register declines to say why (§12).
- Whether the sharpening is a custom or a reflex. Custom provenance belongs to EV's register and
  is inherited unchanged.

**Voice — sample fragments (content candidates for `shed_lines.json` / `instrument_lines.json`).**

> "Tool wall, evening: hammer home, saw out, square home. The wall does not say who. The wall
> says where, which is the household's own name for who." — shed read (SH)

> "The good square set three saws this month. It is checked more than any other instrument and
> trusted exactly as much." — shed read (SH)

> "Hook 1 is older than the wall. The wall was built around the hook, which is the reverse of
> how walls are usually built." — shed read (SH)

> "Check sheet: scale agreed, barometer agreed, clock disagreed by its usual one. Disagreed by
> its *usual* one — that is the sentence that keeps this instrument honest." — check sheet (IN)

> "The unitless measure reads 4 and 3. What it counts is not in this book. That it counts
> reliably is." — check sheet (IN)

**Design texture beats.**

- **No tool stats, ever** (DEC-SH-02). Condition and repair stay with owners; the shed is place
  and habit only.
- **Disagreements render in equal type** (DEC-IN-03). The constant offset is stated plainly; the
  plan never arbitrates.
- **The older hook renders flatly** (inherits LP-DEC-LP-03's discipline): one row among rows.
- **Checks are history facts.** The register records that a check happened, never that an
  instrument was corrected.

---

## 1c. The Deeper Layer — objects & held silences (second prose pass)

*(Second prose pass, non-contractual: narrative texture and writing guidance only — no authority,
no claimed path, no acceptance criterion, no verification step. §12's register is unchanged; the
fragments below are content candidates and deliberate silences, not new recorded questions. Where
a named data file holds no prose field, fragments are texture only and gain no schema.)*

**What the wall leaves lying around.**

> "Chalk outline, faded, behind the third hook. Somebody once drew the shape of a tool that is
> not on the wall. The outline has outlasted two repaints." — texture only

> "Tin, opened: the good screwdriver, three bits, a folded note. The note is a list of parts with
> numbers and no units. The tin keeps the note and the note keeps its mystery."

> "Sharpening stone, hollowed in the middle. The hollow is everybody's and fits no one hand."

**What the check sheet leaves lying around.**

> "Check sheet, month 9: every instrument checked on its day. The sheet is signed by role only.
> The role is the keeper and the keeper is the signature."

> "Barometer, beside the window. The window is where it reads truest. Nobody moved it; the
> instrument found its place and the household noticed."

> "The out-of-true instrument's line: 'disagreed by 1, as usual.' The 'as usual' is doing more
> work than the number."

**Held silences (texture, not register rows).**

- What the chalk outline was the shape of. A tool that is not on the wall is not in any table
  (§1c); the outline is texture and the plan declines to identify it. Texture only.
- What the tin's note was a list of. Parts and numbers, no units (§1c); the note is kept and
  unread, and the tin is not an exhibit.

---

## 1.4 Worked examples (non-normative)

**A tool wall (fixed seed).**

> Evening render: hammer (home, hook 2, mark-count 3), saw (out 2 days — the Loan Shelf's plain
> arithmetic, borrowed), good square (home, tin, checked day 44 — cited by three tools' rows),
> hook 1's tool (home, `home_older` flag in the same type as every other row). The optional
> Sharpening Hour performs two existing repair verbs and writes two mark-counts through the
> Mending plan's own seam; the diff adds no repair verb anywhere.

**A check cycle (fixed seed).**

> Month 10's checks: scale agreed (keeper: store), barometer agreed (keeper: weather-side),
> clock disagreed by its usual one — rendered in equal type beside the bell-clock's reading with
> "offset: 1, constant across 9 months" stated plainly. The unitless measure reads 4 and 3 and is
> checked like everything else. One instrument missed its cycle: "unchecked, one cycle" — no
> penalty, no alarm.

---

## 2. Evidence table (checked against the corpus 2026-09-29; re-verify at P0 — Rule 7)

| # | Claim | Evidence | Status |
|---|---|---|---|
| E1 | Loan Shelf records out-days and borrows; the shed read borrows its plain arithmetic. | `.ai/plans/story-expansion-batch-4/the-loan-shelf-and-the-wind-names-2026-09-29.md` LN-P1 | PROPOSED (plan is DRAFT) |
| E2 | The Mending exposes mark-counts read-only (MN-P1); the wardrobe precedent for count-only consumers. | `.ai/plans/the-mending-and-the-second-language-2026-09-29.md`; batch-4 WR plan | PROPOSED (plans are DRAFT) |
| E3 | `DosimeterCalibrationSystem` owns calibration (`calibrationQuality`, `errorBandMsv`); its lesson ("two readings prove consistency, not honesty") is citable. | `.ai/plans/the-deep-2026-09-29.md` E10/§1b | LIVE |
| E4 | Instrument-disagreement precedents: sky catalogue vs evaluator (E8b) resolved *as a decision*, disputed rows printed in pairs (Wind-Names DEC-WN-01). | `.ai/plans/the-sky-2026-09-29.md` E8b; batch-4 WN plan | LIVE / PROPOSED |
| E5 | Foundries and workbenches are industrial owners (B75, Plan 129 closeouts); no production mechanics may change here. | `docs/plans/PLAN_B75_BALLISTICS_WORKBENCH_CLOSEOUT.md`; `docs/plans/PLAN_129_FOUNDRY_PRODUCTION_CLOSEOUT.md` | LIVE (closeouts) |
| E6 | Custom machinery hosts authored customs through existing verbs (EV seam; Mending Day / Wood Day precedents). | batch-3/4 custom hooks | PROPOSED (soft dependency) |
| E7 | Whether a derived shed read can reference item homes without adding a "home" field to inventory items. | additive-precedent discipline (DEC-SH-03) | **VERIFY (P0)** |
| E8 | Check-sheet persistence needs (history facts) and additive DTO checksum-safety. | codec / snapshot tests | **VERIFY (P0)** |

## 3. Authority table

| Concern | Owner (unchanged) | This plan adds |
|---|---|---|
| Tools, stock, condition | inventory owner + The Mending | nothing; homes are authored rows and mark-counts are read-only |
| Loans | Loan Shelf (batch 4) | nothing; out-days are read |
| Production | foundry / workbench owners | nothing |
| Calibration | `DosimeterCalibrationSystem` and other instrument owners | nothing; checks and disagreements are facts, never corrections |
| Customs | EV | one optional Sharpening Hour |
| Shed/instrument state | — | `ShedTable` + `InstrumentRegister` (pure Core; row facts only) — **DEC-SH-03 / DEC-IN-01** |

## 4. Claimed Paths (proposed; `INT` = integrator-owned)
**Core:** `Assets/Ashfall.Core/Household/ShedTable.cs` (new, pure), `Shelter/InstrumentRegister.cs` (new, pure)
**Data:** `shed_rows.json`, `instrument_rows.json`, `shed_lines.json`, `instrument_lines.json`
**Host:** inventory/loan read (`INT`), roster duty (`INT`), custom hook (EV seam, `INT`)
**Presentation:** a "Wall" band beside the stock surface; a "Checks" band beside the board — no new routed panel
**Tests:** `Ashfall.Core.Tests/Household/ShedTableTests.cs`, `Shelter/InstrumentRegisterTests.cs`, `Ashfall.Core.Tests/Save/ShedInstrumentSaveTests.cs`

## 5. Packages

### SH-P0 — Premise audit (Auditor; read-only): close E7–E8; confirm homes-as-authored-rows needs no inventory field; confirm mark-count read wording with the Mending owner.
### SH-P1 — Shed table + read (Core + data): homes, `home_older`, out-days read-through, validator row-level. **Accept:** no tool stats exist anywhere (§6.3); determinism; round-trip.
### SH-P2 — Wall band + Sharpening Hour (host, dark): existing repair verbs only. **Accept:** no new verb in the diff.
### IN-P1 — Instrument register + check sheet (Core + data): ≤ 12 rows, `disputed` pairs, `unitless`, streaks. **Accept:** no correction write exists anywhere (§6.4); disagreements in equal type.
### IN-P2 — Keeper duty + checks band (host): history facts only. **Accept:** missed checks render plainly.
### X-P1 — Seam hooks (host, dark): one string each way. **Accept:** no cross-reads.

## 6. Acceptance criteria
1. Core authority + host owner + event path + persistence + observable outcome agree (CLAUDE.md "integrated").
2. Ship-dark parity: no shed rows and no instrument rows → inventory, loans, calibration, production and board outputs identical on a saved corpus.
3. No-stats invariant: the diff contains no durability, quality or tool-stat field of any kind (DEC-SH-02).
4. No-correction invariant: the register records checks and disagreements and never writes a calibration or correction (DEC-IN-02).
5. Equal-type invariant: `disputed`, `unitless` and `home_older` marks render in the same plain type as all other rows (DEC-IN-03, DEC-SH-04).
6. Determinism: identical shed reads, check sheets and streaks on replay (`CampaignStreamIds` fork; never `System.Random`).
7. Save round-trip of row facts; old saves load; checksum-safe (E8).
8. The registered silences of The Mending, EV and the instrument owners stay unanswered (§7).

## 7. Cross-plan boundaries
- **The Loan Shelf / The Mending (batches 3–4):** loans and marks are theirs; the shed reads both and writes neither. The shed never lends and never repairs.
- **The Cabinet & The Dig:** display is the Cabinet's verb; a tool on the wall is *working*, not exhibited, and the two ledgers must never merge.
- **The Feeding Place / The Lamps (batch 5):** the `home_older` flag is the flat-render sibling of `off_route` and `unattributed`; the rhymes stay deliberate and the tables stay separate.
- **Instrument owners (dosimeter, weather, clock):** calibration and measurement semantics are theirs; a disagreement is never resolved here.
- **EV:** the Sharpening Hour is one custom; who started customs stays EV's register.

## 8. Decision register (proposals — unsigned)
| ID | Decision | Type | Proposed |
|---|---|---|---|
| DEC-SH-01 | The shed owns no stock and adds no repair verb. | rule | Yes |
| DEC-SH-02 | No tool stats of any kind: condition and repair stay with owners. | rule | Yes |
| DEC-SH-03 | Homes are authored rows, not inventory fields. | architecture | Yes; confirm in P0 |
| DEC-SH-04 | The `home_older` flag renders flatly, one row among rows. | tone | Yes |
| DEC-IN-01 | The register stores row facts only: checks, disagreements, streaks. | architecture | Yes |
| DEC-IN-02 | No corrections, ever: calibration stays with instrument owners. | rule | Yes |
| DEC-IN-03 | Disagreements render in equal type with the constant offset stated plainly. | tone/rule | Yes — **needs canon note** |
| DEC-IN-04 | `unitless` instruments keep their missing units; no vocabulary is added. | tone | Yes — **needs canon note** |
| DEC-X-01 | Both seam hooks ship dark; one string each way. | architecture | Yes |

## 9. Pre-flight Checks
- [ ] Go config/save validator passes (`bin/validate-config` / `bin/ashfall-dev validate-config`)
- [ ] No equivalent existing system found (search `Shed`, `ToolHome`, `InstrumentRegister`, `CheckSheet`, `CalibrationRead`)
- [ ] Premise re-verified (Rule 7); Loan Shelf, Mending and instrument-owner registers re-read; their silences listed in the handoff as untouchable
- [ ] Signed decisions in hand (at minimum DEC-SH-01, DEC-IN-02)

## 10. Verification
- [ ] `bin/run-scoped-tests` on new tests + existing inventory, loan and dosimeter-calibration tests (list from P0 selector)
- [ ] Inventory/calibration selftests with rows on and off (VERIFY args)
- [ ] Scoped only (<30 s each); ≤10–15 test-edit steps per failure, then auto-flag in `.ai/state.md`; **no full suite** without `RUN FULL TESTS`
- [ ] Handoff in `AI_AGENT_WORKFLOW.md` format; `.ai/state.md` updated

## 11. Stop conditions (Rule 10)
Stop and report if: a tool stat would be required by any surface; a correction write would be needed to persist a check; the disputed pair could not render in equal type; homes would require an inventory schema field; any path overlaps a live claim.

## 12. Open Mysteries & Deliberate Silence (lore register — no authority, no claimed path)

These questions are **intentionally unanswered**. They are not bugs, not TODOs, and not hidden
work items. They keep the wall older than its building and the measures truer than their
readings. Any future plan that answers one must name the decision that permits it.

| # | Question | Why it stays open | Who may answer it (later, signed) |
|---|---|---|---|
| SH-OM-1 | Who hung the older hook? | It predates the wall (§12); its origin is unmodeled and the wall was built around it. | Never — deliberately sealed. |
| SH-OM-2 | What was the chalk outline? | A tool that is not on the wall is in no table (§1c); the outline is texture and is never identified. | Never — texture by omission. |
| SH-OM-3 | What is the tin's note a list of? | Parts and numbers, no units (§1c); the note is kept and unread and the tin is not an exhibit. | Never — texture by omission. |
| SH-OM-4 | Why does the saw always come back to hook 3? | Folk ownership (§0) is observed as habit; whether the habit is human or geometric is not modelled. | Never — tone-locked. |
| SH-OM-5 | Is the shed the Cabinet's working cousin? | The family resemblance (§1b) is an image; the ledgers must never merge and the kinship is never asserted. | Never — a rule, not a gap. |
| IN-OM-1 | Why is the out-of-true instrument kept? | Replacement is never proposed (§12); the household checks it anyway and the register declines to say why. | Never — deliberately sealed. |
| IN-OM-2 | What does the unitless measure count? | It counts marks or heights; the vocabulary is closed and must not be extended (DEC-IN-04). | Never — deliberately sealed. |
| IN-OM-3 | Who decided the check cadences? | The cadences are authored; their origin is unmodeled and the keeper role predates the sheet. | Canon owner only, as a signed decision. |
| IN-OM-4 | Is "disagreed by 1, as usual" a comfort? | The register is a document, not a mood (§1c); the household's feelings about the constant offset are prose's job. | Never — texture by omission. |
| IN-OM-5 | Do the instruments ever agree about what they are for? | Purpose is not in the vocabulary (inherits the Cabinet's label discipline); the pair with SH-OM-2 stays unresolved together (mystery-index §3 discipline). | Never — the pair stays open. |
