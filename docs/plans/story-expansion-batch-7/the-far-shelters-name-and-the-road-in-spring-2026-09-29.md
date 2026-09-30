# Feature / Task Plan: The Far Shelter's Name (what the household calls the other shelters) & The Road in Spring (what the thaw gives back)

<!-- UPON INTEGRATION (MANDATORY):
Prepend header at the top saying:
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**
And immediately move this file to .ai/plans/integrated/<category>/
-->

STATUS: DRAFT — awaiting user approval (not self-approved; CLAUDE.md Rule 8 requires `STATUS: APPROVED BY USER` before any code commit). Story batch 7, plans 2 of 4 (subjects 67–68).

> **Subjects covered (2 of the 8 in the "outward spring" batch):**
> 67. **The Far Shelter's Name** — what this household calls the shelters two valleys over, and
> one hearsay row for what they call us. (Prefix `FN`.)
> 68. **The Road in Spring** — what the snow kept and the melt returns: the thaw's second
> excavation, along the roads instead of under floors. (Prefix `TR`.)
>
> **Prose / design companions that already exist and are extended, not replaced:**
> `.ai/plans/story-expansion-batch-4/the-letter-writing-and-the-names-of-the-rooms-2026-09-29.md`
> (folk names *beside* ids), `.ai/plans/story-expansion-batch-6/the-wall-map-and-the-doorstep-2026-09-29.md`
> (the wall map's folk layer: **its register governs the map-side rhyme**),
> `.ai/plans/living-region-2026-09-29.md` (settlement ids; `Heard/Told` discipline),
> `.ai/plans/the-cabinet-and-the-dig-2026-09-29.md` (the Dig's find-kind discipline: offer, never
> assume), `.ai/plans/story-expansion-batch-6/the-frost-book-and-what-the-dog-is-dreaming-2026-09-29.md`
> (the frost book's winter's end).
>
> **Not a claim.** This plan claims no path in `WORKTREE_OWNERSHIP.md`, edits no source, no data,
> no ledger and no other plan. It is documentation only. Paths below are *proposed*; `INT` marks
> integrator-owned shared seams.

> **Editorial note (prose texture).** Sections **0**, **1b**, **1c** and **12** carry story texture
> and voice. Sample lines are content candidates for `*.json` rows; they belong in data, never in
> code. No authority, path, decision, acceptance criterion or verification step is changed by any
> prose section.

---

## 0. Prologue — The Names We Gave Them, the Road We Got Back

> *"Every shelter in this region has two names: the one on its door and the one in our mouths.
> Only one of them is checked."*

The far shelter's name is the household's great act of naming outward. The Iron Basin is "the
smoke" and the Fen Crossing is "the wet road" on the wall map — but past those, at the edge of
the map's knowledge, there are shelters this household has never visited and has named anyway:
the far ones. Names arrive with travellers, get corrected at the Fair, and settle into use the
way river stones settle into a bed. And there is one hearsay row in the naming book: a traveller
said, once, what the far shelter calls *us*. The row is recorded in the `Heard` discipline — a
rumour with a route — and never confirmed.

The road in spring is the other gift from the beyond. When the snow goes, the road gives things
back: the winter's drops, the ruts of last year under this year's mud, and — at the same bend,
every spring, in every year the book has run — one thing that the snow kept and the melt
returned. The thaw is a second excavation: the Dig goes under floors, the melt goes along roads,
and both are sieves for what the before and the winter left behind.

**Tone & register.** Distant-plain and thaw-quiet. The naming book's vocabulary is *called,
heard, settled, corrected, far*; the road's is *melt, rut, returned, bend, spring*. Prose for the
names should read like a gazetteer kept by people who travel rarely and talk constantly; prose
for the road like the first walk of the year with the mud arguing.

**Mystery & texture.** Two silences hold the pair. The far shelter that we have named has
**never confirmed the name** — and one hearsay row suggests they have a name for *us* that we
would not have chosen (§12). And the bend's annual return is **always the same kind and never
the same object** — the book records it every spring and refuses to say what it is *for* (§12).
Neither is a puzzle. Both are what happens when distance and seasons do the work that
understanding usually does.

**The second layer.** Naming and thawing are both acts of *making the beyond legible*: one with
words, one with water. The naming book's hearsay row and the bend's annual return are the two
places where the beyond answers back in its own grammar — a name we did not choose, an object we
did not place — and both are recorded in the household's plain hand and left unexplained. The
plan's quiet thesis is that a shelter's neighbours include its winters, and both are met the same
way: by paying attention without demanding sense.

---

## 1. Goal & Outcome

> *Design intent: the player should page through the naming book and feel the region's distance
> in its names — and walk the road in the first thaw and see what the winter kept.*

### 1.1 The Far Shelter's Name (FN)

- **Goal:** A **Naming Rows** table (≤ 16 rows: place id (existing settlement/shelter ids or
  `far` for beyond-map shelters), household name, `settled`/`corrected`/`drifting` state,
  `hearsay` flag for the one row recording what they call us) and a derived **Naming Read** (the
  gazetteer in plain type: names beside ids, states, this season's corrections); one constrained
  verb — **Name** (an authored alternate may be added from the folk-name vocabulary; free text
  forbidden — inherits DEC-WM-02's rule). The naming book never writes to reputation, news or
  map owners (DEC-FN-01).
- **Outcome (observable):** on a fixed seed, the naming read renders each place's household name
  beside its id; `drifting` names render their state; the `hearsay` row renders flatly with its
  `Heard` provenance string and is never confirmed; `Name` adds an alternate and removes nothing;
  with no rows every surface behaves identically to today; save/load round-trips.
- **Non-Goals:** no diplomacy, reputation or faction effects (Exp. 45 / Plan 207 keep their
  meanings); no map writes (the wall map is batch 6's and stays wrong on purpose); no translation
  or language generation; no confirmation of any hearsay; no new save section; no new routed
  panel; no Unity.
- **"Done":** §6 acceptance passes via `bin/run-scoped-tests`; ship-dark parity holds; handoff
  lists untouched shared paths.

### 1.2 The Road in Spring (TR)

- **Goal:** A **Thaw Rows** table (≤ 20 rows: date, kind from a closed list (`ruts`, `drops`,
  `cloth`, `metal`, `wood`, `paper`, `bend_return`, `unnamed`), `melted_out` flag, `offered`
  flag when a find is offered to the Cabinet's display verb) and a derived **Thaw Read** (this
  spring's returns, the bend's annual row rendered flatly, the mud weeks' dates); one optional
  custom hook (evenings-and-memory-work seam) recording the first walk of the year. Finds follow
  the Dig's discipline: **offer only** (DEC-TR-02).
- **Outcome (observable):** on a fixed seed, the thaw read renders the season's finds with their
  kinds; the `bend_return` row appears once per spring and renders flatly like every other row;
  an `offered` find moves to the Cabinet's verb through its own rules or is refused; with no rows
  every owner behaves identically to today; save/load round-trips.
- **Non-Goals:** no weather or snow mechanics (weather owners keep their meanings); no
  archaeology mechanics (the Dig keeps its meaning; no strata here); no item creation (finds are
  records until the Cabinet accepts them — DEC-TR-02); no travel effects; no new save section; no
  new routed panel; no Unity.
- **"Done":** §6 acceptance passes; ship-dark parity holds; handoff lists untouched shared paths.

### 1.3 The seam between them (X)

- **Goal:** One documented boundary and one optional hook: a naming row may render beside a thaw
  find ("returned at the bend below the wet road") as one read-only string; and the thaw read may
  cite the frost book's winter's end date. One string each way, shipped dark.

---

## 1b. Texture, Mystery & Voice

**A name is a rumour with a route.**

The naming book inherits the Living Region's epistemology exactly: names arrive `Heard`, get
`corrected` at the Fair, and `settle` through use. Prose must respect the grades — a settled name
is a different fact from a drifting one — and the hearsay row must stay graded: a traveller said
it, once, and the book recorded who said it and not whether it is true.

**The melt is a sieve.**

The road in spring is the Dig's outdoor sibling: same three-kind discipline (record, offer,
refuse), different medium. Prose should treat the thaw as *the winter returning its receipts* —
matter-of-fact, slightly rueful, and never portentous. The bend's annual row is the plan's flat
miracle: same kind, different object, every year.

**What the player is never told.**

- What the far shelter calls us. The `hearsay` row records a traveller's words and not their
  truth (§12); the name itself is never confirmed and never printed as fact.
- What the bend's return is *for*. `bend_return` is a kind, not a purpose (§12); the book counts
  springs and declines to interpret.
- Why the names drift where they do. The states (`settled`/`corrected`/`drifting`) are observed;
  their causes are social and unmodelled.
- Whether the roads remember being walked. Last year's ruts under this year's mud are texture
  (§1c); the plan refuses to make the road a witness.

**Voice — sample fragments (content candidates for `naming_lines.json` / `thaw_lines.json`).**

> "Naming book: the far one past the wet road is called 'the two lights' here. What it calls
> itself is its own business. What it calls us is a hearsay row and I have written down who said
> it." — naming book (FN)

> "Corrected at the Fair: 'the smoke' is the Iron Basin and everyone at the stall agreed, which
> is how corrections happen." — naming book (FN)

> "Thaw, second week: the mud gave back a bootlace and last year's ruts. The road is returning
> its receipts." — thaw read (TR)

> "Bend return, spring 3. Same kind as spring 2 and spring 1. Not the same object. The book has
> stopped pretending to be surprised." — thaw read (TR)

**Design texture beats.**

- **Hearsay is graded, never confirmed** (DEC-FN-02): the `Heard` discipline is inherited whole
  and the row may not be promoted to `Told` by any mechanic in this plan.
- **Finds are records until the Cabinet accepts them** (DEC-TR-02): offer, refuse, or display —
  never assume.
- **The flat-render family again**: `hearsay` and `bend_return` render exactly like their
  neighbours; the format's fairness is the mystery's engine.
- **No names for people** — the traveller who said the hearsay is a role; the corpus's no-names
  discipline holds at distance too.

---

## 1c. The Deeper Layer — objects & held silences (second prose pass)

*(Second prose pass, non-contractual: narrative texture and writing guidance only — no authority,
no claimed path, no acceptance criterion, no verification step. §12's register is unchanged; the
fragments below are content candidates and deliberate silences, not new recorded questions. Where
a named data file holds no prose field, fragments are texture only and gain no schema.)*

**What the naming book leaves lying around.**

> "Gazetteer margin: three names crossed out, none struck. The crossings-out are the drift of
> five years and the book keeps every version."

> "The hearsay row's provenance: 'traveller, spring, role unknown.' The book records the shape of
> the source and declines the source itself." — texture only

> "A name that is a sentence: 'the one where the lamp is kept lit.' The far shelter does not know
> we call it that. The sentence is ours and stays ours."

**What the thaw leaves lying around.**

> "First-walk tally: two ruts, one drop, one piece of cloth, one piece of metal. Kinds, not
> stories. The road returns matter; the prose may wonder."

> "The bend, year 3: a return again. Somebody has scratched a small mark at the year's line — the
> book's only decoration and the book's only superstition." — texture only

> "Mud weeks, dated: the four weeks the road is not a road. The house plans around them the way
> it plans around the frost book's cold mornings."

**Held silences (texture, not register rows).**

- What the far shelter's name for us actually is. The hearsay row holds it in the traveller's
  words (§12); the words are recorded elsewhere and the plan declines to print them as fact.
  Texture only.
- Why the bend keeps returning things. Annual, same kind, different objects (§12); the plan
  refuses to animate the pattern and the reader may keep the shiver (inherits SW-OM-5's
  discipline).

---

## 1.4 Worked examples (non-normative)

**A naming season (fixed seed).**

> The gazetteer renders: "the smoke — Iron Basin (settled)", "the wet road — Fen Crossing
> (settled)", "the two lights — `far` (drifting)", and the hearsay row — flat, `Heard`-provenanced:
> "traveller, spring, role unknown." A `Name` correction at the Fair adds an authored alternate
> for "the two lights"; the old name stays printed. No reputation, news or map state changes
> anywhere in the diff.

**A thaw month (fixed seed).**

> Four weeks of mud, then the first walk: ruts, drops, cloth, metal, `unnamed` — each one line,
> each `melted_out` where the snow kept it. The bend returns its annual row: `bend_return`, spring
> 3, flatly rendered. One find is `offered` to the Cabinet and displayed under its verb; the rest
> are records and stay records. The frost book's winter's end date renders beside the first walk
> as one read-only string.

---

## 2. Evidence table (checked against the corpus 2026-09-29; re-verify at P0 — Rule 7)

| # | Claim | Evidence | Status |
|---|---|---|---|
| E1 | Folk names live beside ids and `Correct`-style verbs add alternates only (batch-4 NR, batch-6 WM precedents). | `.ai/plans/story-expansion-batch-4/the-letter-writing-and-the-names-of-the-rooms-2026-09-29.md`; batch-6 WM plan | PROPOSED (plans are DRAFT) |
| E2 | `Heard/Told/Seen` graded-news discipline with corrections (LR-P7); hearsay rows carry provenance. | `.ai/plans/living-region-2026-09-29.md` §1b | PROPOSED (plan is DRAFT) |
| E3 | The wall map is folk-layer and wrong on purpose; its register governs the map-side rhyme. | `.ai/plans/story-expansion-batch-6/the-wall-map-and-the-doorstep-2026-09-29.md` §12 | PROPOSED (plan is DRAFT) |
| E4 | The Dig's find-kind discipline: object/fragment/silence, offer-only to RT/Cabinet; strata terminal rules. | `.ai/plans/the-cabinet-and-the-dig-2026-09-29.md` §1.2/DEC-DG-04 | PROPOSED (plan is DRAFT) |
| E5 | The frost book marks winter's end implicitly (its rows are cold-morning draws; `clear_morning` precedent). | `.ai/plans/story-expansion-batch-6/the-frost-book-and-what-the-dog-is-dreaming-2026-09-29.md` | PROPOSED (plan is DRAFT) |
| E6 | Custom machinery hosts authored customs (EV seam; first-walk custom precedent). | batch-3–6 custom hooks | PROPOSED (soft dependency) |
| E7 | Whether `far` place ids can live beside settlement ids without namespace collision (sibling of `off_edge`/`shorthand`). | additive-precedent discipline | **VERIFY (P0)** |
| E8 | Board render points for gazetteer and thaw reads. | batch-3–6 render precedents | **VERIFY (P0)** |

## 3. Authority table

| Concern | Owner (unchanged) | This plan adds |
|---|---|---|
| Settlements, news | Living Region | one hearsay row through its `Heard` discipline; no writes |
| Official maps | cartography / wall map | nothing; the gazetteer is names beside ids |
| Reputation, diplomacy | Plan 207 / Exp. 45 | nothing; names move no standing |
| Weather, snow | weather owners | nothing; the thaw read observes dates only |
| Finds, display | the Cabinet's verbs | offer-only; the Cabinet decides |
| Naming/thaw folk state | — | `NamingRows` + `ThawRows` (pure Core; row facts only) — **DEC-FN-03 / DEC-TR-01** |

## 4. Claimed Paths (proposed; `INT` = integrator-owned)
**Core:** `Assets/Ashfall.Core/Folklore/NamingRows.cs` (new, pure), `World/ThawRows.cs` (new, pure)
**Data:** `naming_rows.json`, `thaw_rows.json`, `naming_lines.json`, `thaw_lines.json`
**Host:** board surface (`INT`), Cabinet offer hook (`INT`), custom hook (EV seam, `INT`)
**Presentation:** a "Names" band beside the gazetteer/map surface; a "Thaw" line on the existing spring surface — no new routed panel
**Tests:** `Ashfall.Core.Tests/Folklore/NamingRowsTests.cs`, `World/ThawRowsTests.cs`, `Ashfall.Core.Tests/Save/NamingThawSaveTests.cs`

## 5. Packages

### FN-P0 — Premise audit (Auditor; read-only): close E7–E8; confirm `far` ids cannot collide with settlement namespaces; confirm the hearsay row's `Heard`-provenance wording with the Living Region owner.
### FN-P1 — Naming rows + gazetteer (Core + data): states, hearsay flag, `Name` verb, validator row-level. **Accept:** no reputation/news/map write exists anywhere (§6.3); determinism; round-trip.
### FN-P2 — Naming band + corrections (host): adds alternates only. **Accept:** hearsay is never promoted to `Told` by any path.
### TR-P1 — Thaw rows + read (Core + data): closed kinds, `melted_out`, `bend_return`, validator row-level. **Accept:** no item creation (§6.4); determinism; round-trip.
### TR-P2 — First-walk custom + offers (host, dark): offer-only to the Cabinet. **Accept:** refusals leave records as records.
### X-P1 — Seam hooks (host, dark): one string each way. **Accept:** no cross-reads.

## 6. Acceptance criteria
1. Core authority + host owner + event path + persistence + observable outcome agree (CLAUDE.md "integrated").
2. Ship-dark parity: no naming rows and no thaw rows → news, reputation, maps, weather and board outputs identical on a saved corpus.
3. No-promotion invariant: the hearsay row is never confirmed or promoted by any mechanic (DEC-FN-02).
4. No-item invariant: thaw finds are records until the Cabinet accepts them; the diff creates no inventory (DEC-TR-02).
5. Flat-render invariant: `hearsay`, `far`, `bend_return` and `unnamed` render in the same plain type as every other row (DEC-FN-04, DEC-TR-03).
6. Determinism: identical gazetteer renders, thaw finds and bend returns on replay (`CampaignStreamIds` fork; never `System.Random`).
7. Save round-trip of row facts; old saves load; checksum-safe (E7 closed at P0).
8. The wall map's, the Dig's and the Living Region's registers stay unanswered (§7).

## 7. Cross-plan boundaries
- **The Wall Map (batch 6):** its register governs the map-side rhyme; the gazetteer is names at distance and the map is *wrong on purpose* — the two folk layers never merge.
- **The Names of the Rooms (batch 4):** rooms are named inward; far shelters are named outward; the vocabularies are siblings and the tables separate.
- **Living Region:** `Heard` discipline is inherited; the hearsay row lives or dies by its rules and writes no news.
- **The Dig / The Cabinet (batch 3):** find-kind discipline is the Dig's; thaw finds are offered and the Cabinet decides. The bend is not a stratum.
- **The Frost Book (batch 6):** winter's end is its date; the thaw cites it read-only.
- **Plan 207 / Exp. 45:** names are feelings; the diff moves no standing.

## 8. Decision register (proposals — unsigned)
| ID | Decision | Type | Proposed |
|---|---|---|---|
| DEC-FN-01 | The naming book writes no reputation, news or map state. | rule | Yes |
| DEC-FN-02 | Hearsay is graded and never promoted: no mechanic confirms it. | tone/rule | Yes |
| DEC-FN-03 | Naming rows are folk facts beside ids; alternates only, free text forbidden. | architecture | Yes |
| DEC-FN-04 | `hearsay` and `far` render flatly, like every other row. | tone | Yes — **needs canon note** |
| DEC-TR-01 | Thaw rows are records: kinds, dates, flags; no geography and no strata. | architecture | Yes |
| DEC-TR-02 | Finds are records until the Cabinet accepts them (offer-only). | rule | Yes |
| DEC-TR-03 | `bend_return` renders flatly: same kind, different object, every spring. | tone | Yes — **needs canon note** |
| DEC-X-01 | Both seam hooks ship dark; one string each way. | architecture | Yes |

## 9. Pre-flight Checks
- [ ] Go config/save validator passes (`bin/validate-config` / `bin/ashfall-dev validate-config`)
- [ ] No equivalent existing system found (search `NamingBook`, `Gazetteer`, `ThawRead`, `MeltFind`, `FarShelter`)
- [ ] Premise re-verified (Rule 7); the wall map, Dig and Living Region registers re-read; their silences listed in the handoff as untouchable
- [ ] Signed decisions in hand (at minimum DEC-FN-02, DEC-TR-02)

## 10. Verification
- [ ] `bin/run-scoped-tests` on new tests + existing news, cabinet and weather tests (list from P0 selector)
- [ ] Cabinet offer selftest with thaw rows on and off asserting zero inventory creation (VERIFY args)
- [ ] Scoped only (<30 s each); ≤10–15 test-edit steps per failure, then auto-flag in `.ai/state.md`; **no full suite** without `RUN FULL TESTS`
- [ ] Handoff in `AI_AGENT_WORKFLOW.md` format; `.ai/state.md` updated

## 11. Stop conditions (Rule 10)
Stop and report if: any path would confirm the hearsay row; a thaw find would need to become inventory without the Cabinet's verb; `far` ids would collide with a settlement namespace; a name would touch reputation or maps; any path overlaps a live claim.

## 12. Open Mysteries & Deliberate Silence (lore register — no authority, no claimed path)

These questions are **intentionally unanswered**. They are not bugs, not TODOs, and not hidden
work items. They keep the far shelters farther than their names and the road stranger than its
returns. Any future plan that answers one must name the decision that permits it.

| # | Question | Why it stays open | Who may answer it (later, signed) |
|---|---|---|---|
| FN-OM-1 | What do the far shelters call us? | The hearsay row is graded and never promoted (DEC-FN-02); the traveller's words are recorded elsewhere and never printed as fact. | Never — deliberately sealed. |
| FN-OM-2 | Why did the traveller say it and say nothing else? | Role unknown, spring, one sentence (§1c); the source is shape-only and the plan declines the source. | Never — texture by omission. |
| FN-OM-3 | Why do the names drift where they do? | States are observed, causes social and unmodelled (§1b); the gazetteer records the drift and not the weather that drove it. | Never — a rule, not a gap. |
| FN-OM-4 | Is 'the two lights' a description or a warning? | Names are feelings (§0); the sentence is ours and stays ours and the far shelter's view is not consulted. | Never — tone-locked. |
| TR-OM-1 | What is the bend's return for? | `bend_return` is a kind, not a purpose (§12); the book counts springs and declines to interpret. | Never — deliberately sealed. |
| TR-OM-2 | Why the same bend, every spring? | Annual, same kind, different objects (§12); the pattern is recorded and refused animation (inherits SW-OM-5's discipline). | Never — the pair stays unresolved (mystery-index §3 discipline). |
| TR-OM-3 | Do the roads remember being walked? | Last year's ruts under this year's mud are texture (§1c); the road is not made a witness and must not be. | Never — texture by omission. |
| TR-OM-4 | Who scratched the small mark at the year's line? | The book's only decoration and only superstition (§1c); the hand is unrecorded. | Never — texture by omission. |
| TR-OM-5 | Was the bend's first return before the book? | Year 1 recorded a return; whether the bend was returning before anyone counted is unasserted. | Canon owner only, as a signed decision. |
