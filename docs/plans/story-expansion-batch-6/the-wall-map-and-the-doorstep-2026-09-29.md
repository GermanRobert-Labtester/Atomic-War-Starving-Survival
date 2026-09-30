# Feature / Task Plan: The Wall Map (the hand-drawn map beside the official one) & The Doorstep (anonymous exchanges at the threshold)

<!-- UPON INTEGRATION (MANDATORY):
Prepend header at the top saying:
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**
And immediately move this file to .ai/plans/integrated/<category>/
-->

STATUS: DRAFT — awaiting user approval (not self-approved; CLAUDE.md Rule 8 requires `STATUS: APPROVED BY USER` before any code commit). Story batch 6, plans 4 of 4 (subjects 63–64).

> **Subjects covered (2 of the 8 in the "inward winter" batch):**
> 63. **The Wall Map** — the map drawn by hand beside the official one: folk names for far
> places, pencil routes, pins for places people think about. (Prefix `WM`.)
> 64. **The Doorstep** — what the outside leaves at the threshold and what the house leaves
> there: anonymous, unrecorded as persons, kept as one-line facts. (Prefix `DS`.)
>
> **Prose / design companions that already exist and are extended, not replaced:**
> `docs/plans/PLAN_207_SHELTER_REPUTATION_INTEGRATION_LOG.md` (external perception — read-only
> context), `.ai/plans/living-region-2026-09-29.md` (settlements and their vocabularies),
> `.ai/plans/the-letter-writing-and-the-names-of-the-rooms-2026-09-29.md` (folk names *beside*
> ids; the out-tray's sibling), `.ai/plans/story-expansion-batch-5/the-feeding-place-and-the-lamps-2026-09-29.md`
> (the plate's gift discipline and its track etiquette), cartography (Plan 163 — **its official
> maps are untouched here**).
>
> **Not a claim.** This plan claims no path in `WORKTREE_OWNERSHIP.md`, edits no source, no data,
> no ledger and no other plan. It is documentation only. Paths below are *proposed*; `INT` marks
> integrator-owned shared seams.

> **Editorial note (prose texture).** Sections **0**, **1b**, **1c** and **12** carry story texture
> and voice. Sample lines are content candidates for `*.json` rows; they belong in data, never in
> code. No authority, path, decision, acceptance criterion or verification step is changed by any
> prose section.

---

## 0. Prologue — The Map That Is Wrong and the Step That Is Kept

> *"The official map is where things are. The wall map is where things are to us. Both are true
> and only one of them is checked."*

The wall map is the shelter's great collective drawing: pinned, pencilled, corrected in four
handwritings, and never once authoritative. It shows the settlements with their folk names — the
Iron Basin is "the smoke", Fen Crossing is "the wet road" — the routes people have walked and the
routes people *mean* to walk, and a scattering of pins for places that are in no other table. The
official map keeps its precision; the wall map keeps its *feelings*, and the shelter consults
them for different questions.

The doorstep is the household's other negotiation with the outside: the flat stone by the door
where things are left and taken without witnesses. A mended thing returned anonymously. A tool
set down at night. A note in an envelope — the envelope discipline of the letter book holds:
address, weight, no contents. Nothing on the doorstep is ever claimed, attributed or thanked in
public. The doorstep record is one line per exchange and the line has no column for who.

**Tone & register.** Pencil-plain and threshold-quiet. The wall map's vocabulary is *pin, route,
named, corrected, edge*; the doorstep's is *left, taken, step, anonymous, morning*. Prose for the
map should read like four people arguing quietly over a drawing they all love; prose for the
doorstep like a ledger kept of gifts by someone determined not to make them awkward.

**Mystery & texture.** Two silences hold the pair. The wall map carries **a pencil line nobody
drawn admits to** — a route to a place off the map's edge, redrawn whenever it fades (§12). And
the doorstep's exchanges have a **third party**: things are sometimes left that nobody in the
shelter left, and the record's `anonymous` column is permanent (§12). Neither is a puzzle. Both
are what happens when a household's care for the beyond outruns its bookkeeping.

**The second layer.** The map and the doorstep are the two faces of the threshold: one draws the
beyond *inward* (names, routes, pins — the world made legible from the fire), the other accepts
the beyond *at the door* (gifts, returns, notes — the world made present without a face). Both
are acts of relationship that deliberately refuse identification: the folk name is not a claim
and the anonymous column is not a mystery to solve. The plan's quiet thesis is that a shelter
loves the world in exactly the two ways available to it — by picturing it and by feeding it —
and both ways end in a blank the house has chosen not to fill.

---

## 1. Goal & Outcome

> *Design intent: the player should stand before the wall map and see how the shelter imagines
> the region — and find something on the step in the morning and understand it will not be
> claimed.*

### 1.1 The Wall Map (WM)

- **Goal:** A **Wall Map Rows** table (≤ 24 rows: place id (existing settlement/location ids or
  `off_edge` for the anomaly), folk name, `pinned` flag, `route_walked` / `route_intended`
  flags, `pencil_line_unknown` for the unclaimed route) and a derived **Map Read** (the drawing
  rendered beside the official map's facts, corrections this month, the anomaly line flagged);
  one constrained verb — **Correct** (an authored alternate name may be added to a place from the
  folk-name vocabulary; free text forbidden — DEC-WM-02). The official map is never edited
  (DEC-WM-01).
- **Outcome (observable):** on a fixed seed, the map read renders folk names beside official
  ones in the wall map's own plain type; `pinned` places render their pins; the
  `pencil_line_unknown` route renders flatly like every other row; `Correct` adds an alternate
  name and never removes one; with no rows every surface behaves identically to today; save/load
  round-trips.
- **Non-Goals:** no cartography mechanics (Plan 163 and the map owners keep their meanings); no
  travel, route or discovery effects; no geographic truth claims (the wall map is *wrong on
  purpose* — DEC-WM-03); no new save section; no new routed panel; no Unity.
- **"Done":** §6 acceptance passes via `bin/run-scoped-tests`; ship-dark parity holds; handoff
  lists untouched shared paths.

### 1.2 The Doorstep (DS)

- **Goal:** A **Doorstep Rows** table (≤ 20 rows: left-day, kind from a closed list (`mended`,
  `food`, `tool`, `note_envelope`, `cloth`, `unnamed`), `taken`/`left` status, `anonymous`
  permanent flag) and a derived **Step Read** (this week's exchanges, the etiquette's plain
  note); one verb — **Leave** (an item leaves stock through the ordinary inventory transaction
  and becomes a doorstep row) and one verb — **Take** (the reverse); and one optional custom hook
  whose only effect is the exchange being recorded. Attribution is refused by construction
  (DEC-DS-01).
- **Outcome (observable):** on a fixed seed, `Leave` moves an item through the inventory owner
  and writes one doorstep row with `anonymous` set; `Take` reverses it; the step read renders the
  week with the etiquette's plain note ("left, taken, not claimed"); rows that no shelter member
  left render identically to all others; with no rows every owner behaves identically to today;
  save/load round-trips.
- **Non-Goals:** no economy or gift-value mechanics; no reputation effects (Plan 207 keeps its
  meaning); no NPC or visitor behaviours; no resolution of who leaves things; no new save
  section; no new routed panel; no Unity.
- **"Done":** §6 acceptance passes; ship-dark parity holds; handoff lists untouched shared paths.

### 1.3 The seam between them (X)

- **Goal:** One documented boundary and one optional hook: a doorstep note's envelope may render
  the wall map's folk name for its destination ("to the smoke") as one read-only string; and a
  `Correct` may cite the doorstep's kinds as provenance folklore. One string each way, shipped
  dark.

---

## 1b. Texture, Mystery & Voice

**Wrong on purpose.**

The wall map's error is its craft: folk names displace official ones, routes wander, pins mark
places no table holds. The prose must treat the drawing as *true to the household* and never as
a degraded copy of the official map — the two maps answer different questions and neither
answers the other's.

**The anonymous column is permanent.**

Like the good bread and the plate's tracks, the doorstep's `anonymous` is not a mystery field to
be filled; it is a boundary the format keeps. Prose must never wink toward who left what — the
etiquette's whole dignity is that nobody is thanked in public.

**What the player is never told.**

- Who draws the unknown pencil line. It fades and is redrawn (§12); four handwritings are on the
  map and the line matches none of them.
- What is off the map's edge. `off_edge` is a place id for a place the map does not hold (§12);
  the drawing ends and the line goes on.
- Who leaves the third-party things. The `anonymous` column is permanent (DEC-DS-01); whether
  they come from guests, travellers or neighbours is not modelled and must not be.
- What the `unnamed` doorstep kind is. The closed list's one blank, sibling of the sweep's and
  the glossary's — three blanks in a corpus-wide motif, and the plan keeps them unmotivated.

**Voice — sample fragments (content candidates for `wall_map_lines.json` / `doorstep_lines.json`).**

> "Wall map, the smoke: pinned twice, named once officially and once properly. The corrections
> this month are all in the same hand and none of them are wrong." — map read (WM)

> "The pencil line goes past the edge of the paper. That is allowed on this map. That is almost
> the point of this map." — map read (WM)

> "Step, morning: a mended thing. Left, taken, not claimed. The record's third column is empty
> on purpose and the purpose is politeness." — step read (DS)

> "Envelope on the step: to the smoke. Address, weight, no contents. The doorstep and the
> out-tray are the same idea in two coats." — step read (DS)

**Design texture beats.**

- **The official map is never edited** (DEC-WM-01): folk names live beside ids, exactly as room
  names do.
- **The wall map is wrong on purpose** (DEC-WM-03): no surface may "correct" it toward truth.
- **`anonymous` is permanent** (DEC-DS-01): the flat-render family's newest member, and the only
  one that is a whole column.
- **Three blanks, one motif**: the sweep's `unnamed`, the glossary's blank definition, the
  doorstep's `unnamed` kind — siblings, never merged (§12 keeps their kinship unasserted).

---

## 1c. The Deeper Layer — objects & held silences (second prose pass)

*(Second prose pass, non-contractual: narrative texture and writing guidance only — no authority,
no claimed path, no acceptance criterion, no verification step. §12's register is unchanged; the
fragments below are content candidates and deliberate silences, not new recorded questions. Where
a named data file holds no prose field, fragments are texture only and gain no schema.)*

**What the wall leaves lying around.**

> "Under the current map, a corner of the older one. The older map's coast is different and its
> pins are in different places. Both maps are pinned by the same four hands." — texture only

> "The pin for the place that is in no other table: red head, bent twice, pushed in harder than
> the others. Being believed in is a physical process." — texture only

> "Route corrections: 'walked' and 'meant to'. The wall map keeps both tenses and refuses to
> merge them."

**What the step leaves lying around.**

> "Snow, around the step: one set of prints, going and coming, unremarkable. The doorstep's
> etiquette extends to the weather: the tracks are noted and not followed."

> "Blank coin on the step, Tuesday. Blank coin again, Friday. The step read records two rows and
> declines to call it a habit." — texture only

> "Thank-you note, left on the step by the house, next morning: taken. The exchange of thanks is
> itself anonymous, which the record notes as one more row."

**Held silences (texture, not register rows).**

- What the older map's different coast was right about. The corner shows a different shore (§1c);
  which drawing is truer is exactly the question the wall map refuses. Texture only.
- Whether the blank coins are the same coin. Two rows, one kind (§1c); the record counts and the
  plan declines to wonder aloud.

---

## 1.4 Worked examples (non-normative)

**A map month (fixed seed).**

> The map read renders: folk names beside official ones ("the smoke — Iron Basin"), pins on three
> `pinned` places (one of which is in no other table), `route_walked` on the coast road and
> `route_intended` on the north one, and the `pencil_line_unknown` line — flat, unclaimed, past
> the paper's edge. `Correct` adds an authored alternate for the wet road; the old name stays
> printed beside it. The official map is byte-identical at the end of the month.

**A doorstep week (fixed seed).**

> Monday: `Leave(mended_thing)` — the item moves through the inventory owner and becomes one row,
> `anonymous`. Tuesday: `blank_coin`, left by nobody in the shelter. Wednesday: `note_envelope`
> addressed "to the smoke" — rendered with the wall map's folk name in one read-only string.
> Friday: `blank_coin` again; the step read records two rows and declines to call it a habit. The
> etiquette's note renders every morning: "left, taken, not claimed."

---

## 2. Evidence table (checked against the corpus 2026-09-29; re-verify at P0 — Rule 7)

| # | Claim | Evidence | Status |
|---|---|---|---|
| E1 | Cartography owns official maps (Plan 163; the SN/RB id namespaces finding). | `.ai/plans/second-nature-and-ruins-of-the-before-2026-09-29.md`; AGENTS queue note on Plan 163 | LIVE (per corpus) |
| E2 | Folk names live beside ids, never over them (batch-4 NR DEC-NR-01; the letter-writing name-pair precedent). | `.ai/plans/story-expansion-batch-4/the-letter-writing-and-the-names-of-the-rooms-2026-09-29.md` | PROPOSED (plan is DRAFT) |
| E3 | Living Region owns settlement ids and regional vocabularies (Iron Basin, Fen Crossing cited in its sample lines). | `.ai/plans/living-region-2026-09-29.md` §1b | PROPOSED (plan is DRAFT) |
| E4 | The envelope discipline: letters carry address and weight, no contents (batch-4 LT DEC-LT-01). | `.ai/plans/story-expansion-batch-4/the-letter-writing-and-the-names-of-the-rooms-2026-09-29.md` | PROPOSED (plan is DRAFT) |
| E5 | The gift/track etiquette precedent: the plate's `anonymous` discipline and its noted-but-unfollowed tracks. | `.ai/plans/story-expansion-batch-5/the-feeding-place-and-the-lamps-2026-09-29.md` §1c | PROPOSED (plan is DRAFT) |
| E6 | Plan 207 shelter reputation is external perception (read-only context only). | `docs/plans/PLAN_207_SHELTER_REPUTATION_INTEGRATION_LOG.md` | LIVE (closeout) |
| E7 | Inventory leave/take transactions-as-rows precedented (loan shelf, letter items). | batch-3/4 plans | PROPOSED (plans are DRAFT) |
| E8 | Whether `off_edge` place ids can exist in a folk table without colliding with id namespaces. | additive-precedent discipline | **VERIFY (P0)** |
| E9 | Board render points for map and step reads; band precedents. | batch-3–6 render precedents | **VERIFY (P0)** |

## 3. Authority table

| Concern | Owner (unchanged) | This plan adds |
|---|---|---|
| Official maps, geography | cartography (Plan 163) | nothing; the wall map is wrong on purpose |
| Settlements, vocabularies | Living Region | one folk-name table beside its ids; no writes |
| Letters, envelopes | Letter-Writing (batch 4) | one read-only name string; envelope discipline inherited |
| Gifts, tracks | Feeding Place (batch 5) | sibling etiquette; the tables never merge |
| Reputation | Plan 207 | nothing; the doorstep moves no reputation |
| Items | inventory owner | `Leave`/`Take` are its own transactions |
| Folk state | — | `WallMapRows` + `DoorstepRows` (pure Core; row facts only) — **DEC-WM-04 / DEC-DS-02** |

## 4. Claimed Paths (proposed; `INT` = integrator-owned)
**Core:** `Assets/Ashfall.Core/Folklore/WallMapRows.cs` (new, pure), `Social/DoorstepRows.cs` (new, pure)
**Data:** `wall_map_rows.json`, `doorstep_rows.json`, `wall_map_lines.json`, `doorstep_lines.json`
**Host:** inventory host session (`INT`), board surface (`INT`), custom hook (EV seam, `INT`)
**Presentation:** a "Wall Map" band beside the official map surface; a "Step" line on the existing morning surface — no new routed panel
**Tests:** `Ashfall.Core.Tests/Folklore/WallMapRowsTests.cs`, `Social/DoorstepRowsTests.cs`, `Ashfall.Core.Tests/Save/WallMapDoorstepSaveTests.cs`

## 5. Packages

### WM-P0 — Premise audit (Auditor; read-only): close E8–E9; confirm `off_edge` rows cannot collide with id namespaces; confirm the official map surface is read-only here.
### WM-P1 — Wall map rows + read (Core + data): folk names beside ids, pins, both route tenses, anomaly line, validator row-level. **Accept:** no cartography write exists anywhere (§6.3); determinism; round-trip.
### WM-P2 — Correct verb + map band (host): authored alternates only. **Accept:** correction reorders/adds and never deletes; the map stays wrong on purpose.
### DS-P1 — Doorstep rows + step read (Core + data): closed kinds, `anonymous` permanent, Leave/Take transactions. **Accept:** no attribution field exists anywhere (§6.4); conservation through inventory.
### DS-P2 — Step line + custom hook (host, dark): the etiquette's plain note. **Accept:** third-party rows render identically to all others.
### X-P1 — Seam hooks (host, dark): one string each way. **Accept:** no cross-reads.

## 6. Acceptance criteria
1. Core authority + host owner + event path + persistence + observable outcome agree (CLAUDE.md "integrated").
2. Ship-dark parity: no wall-map rows and no doorstep rows → cartography, news, reputation, inventory and board outputs identical on a saved corpus.
3. No-cartography invariant: the diff contains no write to any map owner; the wall map is wrong on purpose and uncorrectable toward truth (DEC-WM-01/03).
4. No-attribution invariant: the doorstep's `anonymous` column is permanent and empty of names forever (DEC-DS-01).
5. Flat-render invariant: `pencil_line_unknown`, `off_edge` and `unnamed` render in the same plain type as every other row (DEC-WM-05, DEC-DS-03).
6. Conservation: Leave/Take balance exactly through the inventory owner (E7 closed at P0).
7. Determinism: identical renders, corrections and rows on replay (`CampaignStreamIds` fork; never `System.Random`).
8. The cartography boundary, the envelope discipline and the gift etiquette stay unanswered (§7).

## 7. Cross-plan boundaries
- **Cartography (Plan 163):** official maps are its authority; the wall map is *wrong on purpose* and never corrected toward truth. The two maps answer different questions.
- **The Letter-Writing / The Names of the Rooms (batch 4):** folk names beside ids and envelope discipline are inherited; the doorstep's note is not a letter and the out-tray's tray is not a step — siblings, never merged.
- **The Feeding Place (batch 5):** the gift/track etiquette is its silence; the doorstep's anonymous column is the household-side sibling and the two must never be conflated.
- **Living Region / Plan 207:** settlements and reputation are theirs; pins are feelings and move nothing.
- **The Sweep's `unnamed` / the glossary's blank (batches 3, 6):** three blanks, one motif; the kinship is the corpus's image and never a claim (mystery-index §3 discipline).

## 8. Decision register (proposals — unsigned)
| ID | Decision | Type | Proposed |
|---|---|---|---|
| DEC-WM-01 | The official map is never edited; folk names live beside ids. | rule | Yes |
| DEC-WM-02 | `Correct` adds authored alternates only; free text is forbidden. | rule | Yes |
| DEC-WM-03 | The wall map is wrong on purpose: no surface corrects it toward truth. | tone | Yes — **needs canon note** |
| DEC-WM-04 | Wall map rows are folk facts only; the table stores no geography. | architecture | Yes |
| DEC-WM-05 | The unknown pencil line renders flatly and is redrawn by no recorded hand. | tone | Yes — **needs canon note** |
| DEC-DS-01 | `anonymous` is permanent: no attribution field exists anywhere, ever. | tone/rule | Yes |
| DEC-DS-02 | Doorstep rows are one-line facts; Leave/Take are ordinary inventory transactions. | architecture | Yes |
| DEC-DS-03 | `unnamed` is the closed list's one blank; rendered flatly like every kind. | tone | Yes |
| DEC-X-01 | Both seam hooks ship dark; one string each way. | architecture | Yes |

## 9. Pre-flight Checks
- [ ] Go config/save validator passes (`bin/validate-config` / `bin/ashfall-dev validate-config`)
- [ ] No equivalent existing system found (search `WallMap`, `FolkMap`, `Doorstep`, `Threshold`, `AnonymousExchange`)
- [ ] Premise re-verified (Rule 7); the cartography, letter-writing and feeding-place registers re-read; their silences listed in the handoff as untouchable
- [ ] Signed decisions in hand (at minimum DEC-WM-01, DEC-DS-01, DEC-WM-03)

## 10. Verification
- [ ] `bin/run-scoped-tests` on new tests + existing cartography, inventory and board tests (list from P0 selector)
- [ ] Inventory selftest with rows on and off asserting conservation (VERIFY args)
- [ ] Scoped only (<30 s each); ≤10–15 test-edit steps per failure, then auto-flag in `.ai/state.md`; **no full suite** without `RUN FULL TESTS`
- [ ] Handoff in `AI_AGENT_WORKFLOW.md` format; `.ai/state.md` updated

## 11. Stop conditions (Rule 10)
Stop and report if: any surface would "correct" the wall map toward truth; an attribution field would be needed to render a step row; the pencil line could not render flatly; Leave/Take would not conserve through inventory; any path overlaps a live claim.

## 12. Open Mysteries & Deliberate Silence (lore register — no authority, no claimed path)

These questions are **intentionally unanswered**. They are not bugs, not TODOs, and not hidden
work items. They keep the map larger than the paper and the step wider than the door. Any future
plan that answers one must name the decision that permits it.

| # | Question | Why it stays open | Who may answer it (later, signed) |
|---|---|---|---|
| WM-OM-1 | Who draws the unknown pencil line? | Four handwritings on the map; the line matches none (§12). Redrawn whenever it fades, by no recorded hand. | Never — deliberately sealed. |
| WM-OM-2 | What is off the map's edge? | `off_edge` is a place id for a place the map does not hold (§12); the drawing ends and the line goes on. | Never — deliberately sealed. |
| WM-OM-3 | What was the older map's coast right about? | A different shore under the current drawing (§1c); which is truer is exactly the question the wall map refuses. | Never — tone-locked by DEC-WM-03. |
| WM-OM-4 | What is the pinned place that is in no other table? | Being believed in is a physical process (§1c); the pin is feeling, not fact, and moves nothing. | Never — texture by omission. |
| WM-OM-5 | Are 'walked' and 'meant to' the same route? | Two tenses, one map (§1c); the wall map keeps both and refuses to merge them. | Never — a rule, not a gap. |
| DS-OM-1 | Who leaves the third-party things? | `anonymous` is permanent (DEC-DS-01); guests, travellers and neighbours all fit and none is chosen. | Never — deliberately sealed. |
| DS-OM-2 | Are the blank coins the same coin? | Two rows, one kind (§1c); the record counts and the plan declines to wonder aloud. | Never — texture by omission. |
| DS-OM-3 | Why is thanks itself anonymous? | The exchange of thanks is one more row (§1c); the etiquette's circularity is its charm and not a mechanism. | Never — texture by omission. |
| DS-OM-4 | What is the `unnamed` kind? | The closed list's one blank (DEC-DS-03); sibling of the sweep's and the glossary's blanks and never merged with them. | Never — the triple stays unresolved (mystery-index §3 discipline). |
| DS-OM-5 | Does the doorstep lead anywhere? | The step is the threshold and the threshold is the whole geography (§0); what lies past the prints is the region's, not the record's. | Never — a rule, not a gap. |
