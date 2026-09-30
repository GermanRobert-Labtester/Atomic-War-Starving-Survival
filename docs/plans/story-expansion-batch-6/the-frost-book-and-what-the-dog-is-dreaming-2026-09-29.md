# Feature / Task Plan: The Frost Book (window frost recorded as shapes) & What the Dog Is Dreaming (folk readings of sleeping companions)

<!-- UPON INTEGRATION (MANDATORY):
Prepend header at the top saying:
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**
And immediately move this file to .ai/plans/integrated/<category>/
-->

STATUS: DRAFT — awaiting user approval (not self-approved; CLAUDE.md Rule 8 requires `STATUS: APPROVED BY USER` before any code commit). Story batch 6, plans 1 of 4 (subjects 57–58).

> **Subjects covered (2 of the 8 in the "inward winter" batch):**
> 57. **The Frost Book** — window frost on the inside of the glass, recorded as shapes, compared
> between years. Unitless. Never predictive. (Prefix `FB`.)
> 58. **What the Dog Is Dreaming** — the household's running folk commentary on sleeping
> companions. Never modelled, always attributed to whoever said it. (Prefix `DD`.)
>
> **Prose / design companions that already exist and are extended, not replaced:**
> `docs/plans/UNBLOCK_PLAN177_DREAM_SYSTEM_INTEGRATION_PLAN.md` (human sleep events — **its
> register governs this plan's silence**), `docs/plans/UNBLOCK_EXPANSION41_THE_QUIET_INTEGRATION_PLAN.md`
> (sleep quality, crowding, air), `.ai/plans/crews-and-companions-2026-09-29.md`
> (`CompanionAnimalSystem` — companions read-only), `.ai/plans/the-shed-and-the-instruments-2026-09-29.md`
> (the `unitless` instrument's sibling motif), `.ai/plans/story-expansion-batch-4/the-loan-shelf-and-the-wind-names-2026-09-29.md`
> (Wind-Names citation strings).
>
> **Not a claim.** This plan claims no path in `WORKTREE_OWNERSHIP.md`, edits no source, no data,
> no ledger and no other plan. It is documentation only. Paths below are *proposed*; `INT` marks
> integrator-owned shared seams.

> **Editorial note (prose texture).** Sections **0**, **1b**, **1c** and **12** carry story texture
> and voice. Sample lines are content candidates for `*.json` rows; they belong in data, never in
> code. No authority, path, decision, acceptance criterion or verification step is changed by any
> prose section.

---

## 0. Prologue — The Glass and the Sleeping Dog

> *"The frost makes pictures on the inside of the window because the house is breathing. The dog
> dreams because it is alive. Neither is telling us anything, and we take notes anyway."*

The frost book is a notebook kept on the windowsill. On the mornings when the inside of the glass
freezes — which happens when the house is warm and the night is cold and the air is doing what
air does — somebody draws the frost's shape in the book. Not what it means. What it *looks like*.
The book has run for two winters and its drawings are compared across years the way farmers
compare harvests: not for prophecy, but for the pleasure of finding that this January's frost
looks like last January's frost, only more so.

What the dog is dreaming is the household's other running reading. Dogs twitch in their sleep;
people narrate. The companion's half-run, the paddling paw, the muffled bark into the floor — all
of it gets *attributed* at the fire: to the road, to the goat, to a day three weeks ago. The
attribution is folklore and the plan treats it as such: dream content is never generated, never
stored, never connected to any mechanic. The dog is asleep. The house is talking.

**Tone & register.** Frost-plain and fireside. The frost book's vocabulary is *shape, drawn,
morning, glass, compared*; the dog's is *asleep, twitch, said, attributed, fire-side*. Prose for
the frost should read like a naturalist's notebook kept by an amateur who has decided the
phenomenon is worth *attention* but not *theory*; prose for the dog like the low talk of people
who know they are being silly and do it anyway.

**Mystery & texture.** Two silences hold the pair. The frost book contains **one drawing that is
not a shape** — a page of careful, unrecognisable marks recorded on a morning the glass was
clear (§12). And the dog's dream attributions have **never once been repeated** — every
fire-side guess is new, and the household has noticed and says so (§12). Neither is a puzzle.
Both are what happens when a household pays attention to things that will not answer.

**The second layer.** Both subjects are *readings without instruments*: the house interpreting
signals that carry no message. The frost book is meteorology with the science removed; the dog's
dreams are psychology with the subject removed. What remains in both cases is the *attention
itself* — the fact that this household finds shapes worth drawing and dreams worth narrating on
winter mornings. The plan's quiet thesis is that a shelter's culture is most visible where its
information is poorest: the less a thing can be known, the more the house says about it.

---

## 1. Goal & Outcome

> *Design intent: the player should open the frost book and see two winters of shapes — and hear
> the fire-side talk about the sleeping dog and know, warmly, that none of it is true.*

### 1.1 The Frost Book (FB)

- **Goal:** A **Frost Rows** table (≤ 30 rows: date, shape-id from a closed *shape vocabulary*
  (`feathers`, `ferns`, `panes`, `writing-like`, `shore-like`, …), `drawn_by_role`, `clear_morning`
  flag for the anomaly) and a derived **Book Read** (this winter's shapes beside last winter's,
  counts by shape, the anomaly page flagged); one optional custom hook (evenings-and-memory-work
  seam) whose only effect is the day's drawing being recorded; and plain rendering beside the
  existing weather briefing. Frost is observed and drawn; **no weather state is read into or
  written from the book** (DEC-FB-02).
- **Outcome (observable):** on a fixed seed, cold mornings resolve a seeded shape from the closed
  vocabulary into a row; the book read renders both winters' shapes in parallel columns; the
  `clear_morning` anomaly renders its flag in the same plain type as every other row; with no rows
  every surface behaves identically to today; save/load round-trips.
- **Non-Goals:** no weather mechanics or forecasting (weather owners keep their meanings; the
  book never predicts — DEC-FB-02); no art or drawing system; no divination of any kind; no new
  save section; no new routed panel; no Unity.
- **"Done":** §6 acceptance passes via `bin/run-scoped-tests`; ship-dark parity holds; handoff
  lists untouched shared paths.

### 1.2 What the Dog Is Dreaming (DD)

- **Goal:** A **Fire-Side Rows** table (≤ 24 rows: date, sleeper-id (companion or person-role
  id), `said_by_role`, the folk attribution as a *closed phrase list* (`the road`, `the goat`,
  `a rabbit`, `the long day`, `nobody knows`)) and a derived **Dream Read** (the household's
  running commentary, this week's sayings); one optional read hook: a companion's sleeping state
  from `CompanionAnimalSystem` may render beside its row. **No dream content is ever generated,
  stored or modelled** (DEC-DD-01).
- **Outcome (observable):** on a fixed seed, sleeping companions and night-side roles may seed a
  fire-side row from the closed phrase list; the dream read renders the week's attributions with
  their `said_by_role`; no attribution has ever been repeated is *observable in the data* and
  rendered as a plain note; with no rows every owner behaves identically to today; save/load
  round-trips.
- **Non-Goals:** no dream mechanics (the Dream System's register governs; human sleep events stay
  theirs); no animal behaviour changes; no companion state changes (`CompanionAnimalSystem`
  read-only); no personality or bond mechanics; no new save section; no new routed panel; no Unity.
- **"Done":** §6 acceptance passes; ship-dark parity holds; handoff lists untouched shared paths.

### 1.3 The seam between them (X)

- **Goal:** One documented boundary and one optional hook: a frost shape may render beside a
  fire-side night ("frost like feathers; the dog dreamed of the road") as one string; and the
  dream read may cite a weather word from the Wind-Names seam. One string each way, shipped dark.

---

## 1b. Texture, Mystery & Voice

**A shape is a shape; a saying is a saying.**

Both ledgers share the plan's core discipline: *record, don't interpret*. The frost vocabulary
names shapes, never meanings; the phrase list names attributions, never contents. Prose must
honour the double refusal — it may describe the drawing and the talk, and it may never confirm
the reading.

**The anomaly is a page, not a portent.**

The `clear_morning` drawing and the never-repeated attributions render flatly. The flat-render
family continues (`unattributed`, `off_route`, `home_older`, `never_offered`, and now
`clear_morning`): the format treats every page the same and lets the reader do the wondering.

**What the player is never told.**

- What the anomaly page's marks are. The glass was clear and the page is full (§12); the marks
  are drawn, not described, and the vocabulary refuses them.
- Why no attribution is ever repeated. Whether the household avoids repetition or the dog avoids
  dreams is not modelled (§12).
- What the dog is *actually* dreaming. **The Dream System's register governs**: dream content is
  never generated here and the sibling silence stays untouched (§7).
- Whether the frost shapes are the same shapes across years or only compared as such. The book
  compares; the comparison is a pleasure, not a claim.

**Voice — sample fragments (content candidates for `frost_book_lines.json` / `fireside_lines.json`).**

> "Frost book, 14 January: ferns, and more of them than last January. Drawn by the store hand.
> The book takes the drawing and not the opinion." — frost book (FB)

> "Clear glass, full page. The marks are careful and the page is not in the shape list. Filed
> anyway, which is what the book is for." — frost book (FB)

> "Comparing Januarys is the winter's only argument and nobody has ever won it." — frost book (FB)

> "The dog is running again. Somebody says it is running the east road. Somebody else says it is
> running from the goat. Both are certain. Neither is correct, probably." — fire-side (DD)

> "Two hundred nights, two hundred guesses, not one repeated. The house has noticed and the house
> is proud of itself." — fire-side (DD)

**Design texture beats.**

- **No divination, no forecast, no dream content** (DEC-FB-02, DEC-DD-01). The moment a reading
  predicts anything, the folklore becomes a mechanic and dies.
- **Closed vocabularies only**: a shape list and a phrase list. The anomaly page is the *one*
  sanctioned row outside the list (like the glossary's one blank).
- **The sayings are attributed to roles, not names** (inherits the corpus's role-only discipline).
- **The dog is never a subject of mechanics.** Companions are read-only; the dog is asleep and
  the house is talking.

---

## 1c. The Deeper Layer — objects & held silences (second prose pass)

*(Second prose pass, non-contractual: narrative texture and writing guidance only — no authority,
no claimed path, no acceptance criterion, no verification step. §12's register is unchanged; the
fragments below are content candidates and deliberate silences, not new recorded questions. Where
a named data file holds no prose field, fragments are texture only and gain no schema.)*

**What the sill leaves lying around.**

> "Frost book, the pencil tied to it with string. The string is newer than the book; the pencil
> is older than both."

> "Last January's page, corner folded. The fold marks the morning the shapes looked like the
> year before, which the book records as a drawing and not as a coincidence."

> "Page of marks, clear morning. Somebody has written beside it: 'not frost.' Two words, in the
> hand that draws worst." — texture only

**What the fire leaves lying around.**

> "Fire-side, night 44: 'the goat again.' The goat is asleep in the corner. The goat's own dreams
> are not recorded; the goat's public life is material enough."

> "Blanket by the fire, indented. The indentation is the only proof anyone was there, and the
> fire-side keeps no roster."

> "Phrase list, worn at one corner: `nobody knows` is the most-used phrase and the only one nobody
> ever picks on purpose."

**Held silences (texture, not register rows).**

- Who wrote 'not frost' beside the anomaly page. Two words, one hand (§1c); the hand draws badly
  and writes plainly and is otherwise unrecorded. Texture only.
- Whether the goat dreams. The goat's public life is "material enough" (§1c); the register
  refuses it a row and the plan agrees.

---

## 1.4 Worked examples (non-normative)

**A frost week (fixed seed).**

> Six cold mornings: ferns, ferns, panes, feathers, writing-like, ferns. The book read renders
> two columns — this January beside last — with shape counts: ferns 3 (+1 over last year).
> Morning seven was clear glass and the anomaly page filled anyway: `clear_morning`, rendered in
> the same plain type, filed after the ferns like every other row. The optional custom records
> each drawing and nothing else; no weather state is read at any point.

**A fire-side week (fixed seed).**

> Seven nights, seven rows: `the road`, `the goat`, `a rabbit`, `nobody knows`, `the long day`,
> `the hill`, `the door left open`. Note the draw's discipline: by the fourth night `the road`
> had left the phrase list's *unused* tail, so the seeded draw cannot produce it twice. The dream
> read renders the week with `said_by_role` beside each line and the plain note: "attributions not
> repeated in 214 nights." The companion's sleeping state renders beside its row; no dream
> content exists anywhere in the diff.

---

## 2. Evidence table (checked against the corpus 2026-09-29; re-verify at P0 — Rule 7)

| # | Claim | Evidence | Status |
|---|---|---|---|
| E1 | The Dream System owns human sleep events; its register's silences govern adjacent subjects. | `docs/plans/UNBLOCK_PLAN177_DREAM_SYSTEM_INTEGRATION_PLAN.md` | LIVE (closeout) |
| E2 | `CompanionAnimalSystem` exposes care, bond, grief and state queries (read-only consumers precedented). | `.ai/plans/crews-and-companions-2026-09-29.md` E4 | LIVE |
| E3 | The Quiet owns sleep quality, soundproofing, crowding; atmosphere inputs exist (`ShelterAtmosphereSystem`). | Exp. 41 closeout; `.ai/plans/the-deep-2026-09-29.md` E12 | LIVE |
| E4 | Weather owners expose states and windows; Wind-Names citation strings exist (batch-4 WN-P2). | `.ai/plans/the-sky-2026-09-29.md` E10/E14; batch-4 WN plan | LIVE / PROPOSED |
| E5 | `unitless` motif precedent: one measure outside the unit vocabulary (batch-5 IN, DEC-IN-04). | `.ai/plans/story-expansion-batch-5/the-shed-and-the-instruments-2026-09-29.md` | PROPOSED (plan is DRAFT) |
| E6 | Custom machinery hosts authored customs (EV seam; drawing-recording custom precedent). | batch-3/4/5 custom hooks | PROPOSED (soft dependency) |
| E7 | Whether the no-repeat draw can be enforced with a consumed-tail seeded fork (`CampaignStreamIds`). | determinism precedents | **VERIFY (P0)** |
| E8 | Board/briefing render points for book and dream reads; role-only attribution semantics. | batch-3–5 render precedents | **VERIFY (P0)** |

## 3. Authority table

| Concern | Owner (unchanged) | This plan adds |
|---|---|---|
| Dreams, sleep events | Dream System / The Quiet | nothing; dream content is refused by construction |
| Companions | `CompanionAnimalSystem` | one read-only state citation |
| Weather | weather owners | nothing; the book never reads a forecast into a shape |
| Words | Second Language / Wind-Names | nothing; the shape list and phrase list are folklore vocabularies, not words |
| Customs | EV | one optional drawing custom |
| Frost/dream folk state | — | `FrostRows` + `FireSideRows` (pure Core; row facts only) — **DEC-FB-01 / DEC-DD-02** |

## 4. Claimed Paths (proposed; `INT` = integrator-owned)
**Core:** `Assets/Ashfall.Core/Household/FrostRows.cs` (new, pure), `Social/FireSideRows.cs` (new, pure)
**Data:** `frost_shapes.json`, `fireside_phrases.json`, `frost_book_lines.json`, `fireside_lines.json`
**Host:** day-owner resolution (`INT`), companion read (`INT`), board surface (`INT`)
**Presentation:** a "Frost Book" band beside the weather briefing; a "Fire-side" line on the existing evening surface — no new routed panel
**Tests:** `Ashfall.Core.Tests/Household/FrostRowsTests.cs`, `Social/FireSideRowsTests.cs`, `Ashfall.Core.Tests/Save/FrostFireSideSaveTests.cs`

## 5. Packages

### FB-P0 — Premise audit (Auditor; read-only): close E7–E8; confirm the shape vocabulary's scope; confirm the book cannot reach weather state.
### FB-P1 — Frost rows + book read (Core + data): closed shape vocabulary, anomaly flag, two-column compare, validator row-level. **Accept:** no prediction field exists anywhere (§6.3); determinism; round-trip.
### FB-P2 — Drawing custom + book band (host, dark): history facts only. **Accept:** the anomaly renders flatly; no weather writes.
### DD-P1 — Fire-side rows + dream read (Core + data): closed phrase list, `said_by_role`, no-repeat consumed tail. **Accept:** no dream-content field exists anywhere (§6.4); determinism; round-trip.
### DD-P2 — Companion citation + fire-side line (host, dark): read-only state string. **Accept:** companions unchanged; roles only.
### X-P1 — Seam hooks (host, dark): one string each way. **Accept:** no cross-reads.

## 6. Acceptance criteria
1. Core authority + host owner + event path + persistence + observable outcome agree (CLAUDE.md "integrated").
2. Ship-dark parity: no frost rows and no fire-side rows → weather, dreams, companions and board outputs identical on a saved corpus.
3. No-prediction invariant: the frost book contains no forecast, meaning or divination field of any kind (DEC-FB-02).
4. No-dream invariant: the diff contains no dream-content, sleep-event or animal-behaviour field of any kind (DEC-DD-01).
5. Closed-vocabulary invariant: shape and phrase lists are closed; the anomaly page is the one sanctioned out-of-list row (DEC-FB-03).
6. Determinism: identical seeded shape draws, no-repeat draws and renders on replay (`CampaignStreamIds` fork; never `System.Random`).
7. Flat-render invariant: `clear_morning` renders in the same plain type as every other row (DEC-FB-04).
8. The Dream System's, The Quiet's and EV's registers stay unanswered (§7).

## 7. Cross-plan boundaries
- **The Dream System:** its register governs all dream content. Human sleep events are its; this plan stores folk *sayings about* sleep and generates nothing. The dog's actual dreams are its silence and stay silent.
- **The Quiet / atmosphere:** sleep quality and air are theirs; frost is weather's; the book sits between them and belongs to neither.
- **Companions (Crews and Companions):** companions are read-only; the goat has no row and gains none.
- **The Second Language / Wind-Names:** the shape list and phrase list are *folklore vocabularies*, not words; the rhyme with the glossary's one blank and the instruments' `unitless` measure is deliberate and the tables never merge.
- **EV:** the drawing custom is one custom; custom provenance stays EV's register.

## 8. Decision register (proposals — unsigned)
| ID | Decision | Type | Proposed |
|---|---|---|---|
| DEC-FB-01 | Frost rows store drawings-as-shapes only; no meaning field exists. | tone/rule | Yes |
| DEC-FB-02 | No prediction of any kind: the book never reads weather state in or out. | rule | Yes |
| DEC-FB-03 | The shape vocabulary is closed; the anomaly page is the one sanctioned exception. | rule | Yes — **needs canon note** |
| DEC-FB-04 | The `clear_morning` page renders flatly, filed among the ferns. | tone | Yes |
| DEC-DD-01 | Dream content is never generated, stored or modelled; the Dream System's silence governs. | boundary/rule | Yes |
| DEC-DD-02 | Fire-side rows store folk attributions from a closed phrase list; roles only, no names. | architecture | Yes |
| DEC-DD-03 | The no-repeat discipline is enforced by drawing from the phrase list's unused tail. | rule | Yes |
| DEC-DD-04 | The goat gets no row, ever. | tone | Yes — **needs canon note** |
| DEC-X-01 | Both seam hooks ship dark; one string each way. | architecture | Yes |

## 9. Pre-flight Checks
- [ ] Go config/save validator passes (`bin/validate-config` / `bin/ashfall-dev validate-config`)
- [ ] No equivalent existing system found (search `Frost`, `FrostBook`, `FireSide`, `DreamNote`, `DreamFolk`)
- [ ] Premise re-verified (Rule 7); the Dream System's register re-read and its silences listed in the handoff as untouchable
- [ ] Signed decisions in hand (at minimum DEC-FB-02, DEC-DD-01)

## 10. Verification
- [ ] `bin/run-scoped-tests` on new tests + existing dream, companion and weather tests (list from P0 selector)
- [ ] Companion read selftest with rows on and off (VERIFY args)
- [ ] Scoped only (<30 s each); ≤10–15 test-edit steps per failure, then auto-flag in `.ai/state.md`; **no full suite** without `RUN FULL TESTS`
- [ ] Handoff in `AI_AGENT_WORKFLOW.md` format; `.ai/state.md` updated

## 11. Stop conditions (Rule 10)
Stop and report if: any surface would require a meaning or forecast field; dream content would be needed to render the fire-side line; the anomaly page could not render flatly; a companion state would be written; any path overlaps a live claim.

## 12. Open Mysteries & Deliberate Silence (lore register — no authority, no claimed path)

These questions are **intentionally unanswered**. They are not bugs, not TODOs, and not hidden
work items. They keep the glass unreadable and the dog unknowable. Any future plan that answers
one must name the decision that permits it.

| # | Question | Why it stays open | Who may answer it (later, signed) |
|---|---|---|---|
| FB-OM-1 | What are the marks on the clear-morning page? | Drawn, not described; the shape vocabulary refuses them (DEC-FB-03). | Never — deliberately sealed. |
| FB-OM-2 | Who wrote 'not frost' beside the page? | Two words, one hand (§1c); the hand draws badly and writes plainly and is otherwise unrecorded. | Never — texture by omission. |
| FB-OM-3 | Are the compared shapes really the same shapes? | The book compares; the comparison is a pleasure, not a claim (§1b). | Never — a rule, not a gap. |
| FB-OM-4 | Why does the frost book exist at all? | Someone started it; custom provenance belongs to EV's register and is inherited unchanged. | EV plan's owner (its register governs). |
| FB-OM-5 | Does the frost picture anything? | The plan's thesis is that it pictures the house *breathing* (§0) and stops there; deeper readings are the reader's and stay theirs. | Never — tone-locked by DEC-FB-02. |
| DD-OM-1 | What is the dog actually dreaming? | **Governed by the Dream System's register**; dream content is never generated here and its silence stays untouched. | The Dream System's plan owner only. |
| DD-OM-2 | Why is no attribution ever repeated? | Avoidance or coincidence is not modelled (§12); the house is proud and the plan declines to explain. | Never — deliberately sealed. |
| DD-OM-3 | Does the goat dream? | No row, ever (DEC-DD-04); the goat's public life is material enough. | Never — a rule, not a gap. |
| DD-OM-4 | Who picks `nobody knows`? | It is the most-used phrase and nobody picks it on purpose (§1c); the format's honesty is doing quiet work. | Never — texture by omission. |
| DD-OM-5 | Are the frost and the dreams the same reading? | Two ledgers, one attention (§0); the rhyme is the batch's image and never a claim (mystery-index §3 discipline). | Never — the pair stays open. |
