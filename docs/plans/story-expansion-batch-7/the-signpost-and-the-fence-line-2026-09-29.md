# Feature / Task Plan: The Signpost (hand-made signs at the forks) & The Fence Line (the kept boundary between ours and the wild)

<!-- UPON INTEGRATION (MANDATORY):
Prepend header at the top saying:
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**
And immediately move this file to .ai/plans/integrated/<category>/
-->

STATUS: DRAFT — awaiting user approval (not self-approved; CLAUDE.md Rule 8 requires `STATUS: APPROVED BY USER` before any code commit). Story batch 7, plans 3 of 4 (subjects 69–70).

> **Subjects covered (2 of the 8 in the "outward spring" batch):**
> 69. **The Signpost** — the hand-made signs at the forks: folk names, plain arrows, and one
> sign at the wrong fork. (Prefix `SP`.)
> 70. **The Fence Line** — the boundary kept between ours and the wild: mended posts, one
> deliberate gap, and a line that ends without reason. (Prefix `FC`.)
>
> **Prose / design companions that already exist and are extended, not replaced:**
> `.ai/plans/story-expansion-batch-6/the-wall-map-and-the-doorstep-2026-09-29.md` (the wall map:
> **its register governs the map-side rhyme**), `.ai/plans/story-expansion-batch-5/the-feeding-place-and-the-lamps-2026-09-29.md`
> (the plate at the yard's edge — the fence is its fence), `.ai/plans/the-mending-and-the-second-language-2026-09-29.md`
> (mend marks; observe-only repair discipline), `docs/plans/UNBLOCK_EXPANSION36_NIGHT_WATCH_INTEGRATION_PLAN.md`
> (perimeter and watch are theirs), cartography (Plan 163 — official maps untouched).
>
> **Not a claim.** This plan claims no path in `WORKTREE_OWNERSHIP.md`, edits no source, no data,
> no ledger and no other plan. It is documentation only. Paths below are *proposed*; `INT` marks
> integrator-owned shared seams.

> **Editorial note (prose texture).** Sections **0**, **1b**, **1c** and **12** carry story texture
> and voice. Sample lines are content candidates for `*.json` rows; they belong in data, never in
> code. No authority, path, decision, acceptance criterion or verification step is changed by any
> prose section.

---

## 0. Prologue — The Line That Invites and the Line That Holds

> *"A signpost is a promise that the road knows where it goes. A fence is a promise that the
> wild knows where it stops. Both promises are kept by hand."*

The signpost is the shelter's handwriting on the region. At every fork where a path leaves the
known road, someone has put up a board: folk names, plain arrows, cut with a knife and repainted
on no schedule. The signs are not authoritative — the wall map is wrong on purpose and the signs
are wrong on wood — but travellers use them, and a sign used is a sign true. One sign stands at
the wrong fork. It has always stood there. It points to a place that is not down that road.

The fence line is the other line the house keeps: posts and wire from the yard's edge to
wherever it stops, mended through the existing repair verbs and gapped in exactly one place on
purpose. The gap faces the feeding place. The wild comes in at the gap and the house lets it,
which is the whole of the fence's theology. And at the far end the fence simply *stops* — not at
a corner, not at a landmark, in the middle of the wild, as if the posts had run out of reasons.

**Tone & register.** Roadside-plain and boundary-quiet. The signpost's vocabulary is *fork, sign,
arrow, painted, pointing*; the fence's is *post, wire, gap, mended, ends*. Prose for the signs
should read like directions given by someone who assumes you will look up; prose for the fence
like a maintenance note kept against the wild's account.

**Mystery & texture.** Two silences hold the pair. The wrong-fork sign **points from a fork it
does not serve** and has never been moved (§12). And the fence line's far end **ends without
reason** — mid-wild, unmarked, unexplained (§12). Neither is a puzzle. Both are what happens
when hand-kept lines outlast the hands that started them.

**The second layer.** The signpost and the fence are the two sentences a household writes in the
landscape: one is a *welcome* and one is a *limit*, and both are spelled in posts and paint. The
plan's quiet thesis is that a shelter's clearest statements about itself are not its laws but its
lines — where it points and where it stops — and that both kinds of line are kept up by ordinary
people with ordinary tools, outlast their makers, and inherit their mistakes.

---

## 1. Goal & Outcome

> *Design intent: the player should read a fork sign and know this road is loved — and walk the
> fence's far end and see where the household's certainty runs out.*

### 1.1 The Signpost (SP)

- **Goal:** A **Sign Rows** table (≤ 12 rows: fork id (existing location id), sign text from the
  folk-name vocabulary + arrow direction, `wrong_fork` flag for the anomaly, `fresh_paint` flag
  for the maintained sign, `sign_missing` for a fork that once had one) and a derived **Sign
  Read** (the forks' signs in plain type, corrections this season, the anomaly flagged); one
  constrained verb — **Point** (a sign may be reset among authored alternates; free text
  forbidden — inherits DEC-WM-02's rule). No map or route state is touched (DEC-SP-01).
- **Outcome (observable):** on a fixed seed, the sign read renders each fork's sign with its
  folk name and arrow; the `wrong_fork` sign renders flatly like every other row; `fresh_paint`
  renders its flag beside the maintained sign; `Point` resets a sign to an authored alternate and
  writes one history fact; with no rows every surface behaves identically to today; save/load
  round-trips.
- **Non-Goals:** no navigation or route mechanics (cartography and travel owners keep their
  meanings); no map writes (the wall map is batch 6's and stays wrong on purpose); no signage
  crafting; no resolution of the wrong fork; no new save section; no new routed panel; no Unity.
- **"Done":** §6 acceptance passes via `bin/run-scoped-tests`; ship-dark parity holds; handoff
  lists untouched shared paths.

### 1.2 The Fence Line (FC)

- **Goal:** A **Fence Rows** table (≤ 16 rows: segment id along the yard's edge and out,
  `mended_count`, `gap_kept` for the deliberate gap, `ends_unmarked` for the far terminus) and a
  derived **Fence Read** (the line's segments in order, this season's mends, the gap and the
  terminus flagged); one optional duty hook routed through the *existing* repair verbs only
  (Mending Day discipline — the fence plan adds no repair verb, DEC-FC-02). No perimeter or
  defence state is touched (DEC-FC-01).
- **Outcome (observable):** on a fixed seed, the fence read renders the line's segments with
  their mended counts; the `gap_kept` segment renders its flag flatly beside the feeding place's
  stone; the `ends_unmarked` terminus renders flatly at the line's end; a mending duty writes
  mark-counts through the existing verbs; with no rows every owner behaves identically to today;
  save/load round-trips.
- **Non-Goals:** no defence, perimeter or raid mechanics (Watch and defense owners keep their
  meanings); no wildlife or ecology effects (the gap is *folklore*, not a mechanic — DEC-FC-03);
  no property or territory model; no new save section; no new routed panel; no Unity.
- **"Done":** §6 acceptance passes; ship-dark parity holds; handoff lists untouched shared paths.

### 1.3 The seam between them (X)

- **Goal:** One documented boundary and one optional hook: a sign may render beside a fence
  segment ("the gap's post has a small sign") as one read-only string; and the fence read may
  cite a folk name from the naming book. One string each way, shipped dark.

---

## 1b. Texture, Mystery & Voice

**The signs are wrong on wood.**

The signpost's error is its medium: folk names on boards cut by hand, arrows carved by whoever
had the knife. Prose must treat the signs as *true to travellers* and never as degraded
cartography — the wall map answers what the shelter feels, the signs answer what a walker needs.

**The gap is folklore, not a mechanic.**

The gap faces the feeding place and the wild comes in at it — and the plan's discipline is that
nothing about that is modelled. The gap is a *decision the household made* and keeps making, and
the prose may describe the plate on the stone outside it and nothing more.

**What the player is never told.**

- Why the wrong-fork sign stands where it stands. It has always been there (§12); moving it is a
  verb the plan provides and the house has never used.
- Who keeps the fresh paint. The maintained sign is flagged and unattributed (§12); the corpus's
  role-only discipline holds and the painter is a habit.
- Where the fence line would end if it had reasons. `ends_unmarked` is a terminus without a
  cause (§12); the posts stop and the wild begins and the record declines to say why.
- Whether the gap was made or found. `gap_kept` records the decision and not the origin (§12).

**Voice — sample fragments (content candidates for `sign_lines.json` / `fence_lines.json`).**

> "Fork at the wet road: the sign says 'the smoke' and points east, which is true. The sign is
> painted this spring. The sign is always painted this spring." — sign read (SP)

> "The wrong-fork sign points to the two lights down a road that does not go there. It has been
> there since before the book. Nobody has moved it, including people who have looked at it." —
> sign read (SP)

> "Fence, segment 9: mended 3. Post, wire, post. The line does not need to be straight to be
> kept." — fence read (FC)

> "The gap is kept. The plate is outside the gap. The wild comes in at the gap. These three
> sentences are the whole of the fence's theology and none of them is a mechanic." — fence read
> (FC)

**Design texture beats.**

- **No navigation effects** (DEC-SP-01): signs are words and arrows; the walker walks regardless.
- **The gap is folklore** (DEC-FC-03): no wildlife state, no breach probability, no defence
  change — the gap is a decision, kept in the open.
- **The flat-render family again**: `wrong_fork`, `fresh_paint`, `gap_kept`, `ends_unmarked` —
  all plain type, all unexplained by the format.
- **Mending is observed** (DEC-FC-02): the fence plan adds no repair verb; mark-counts accrue
  through the owners' own calls.

---

## 1c. The Deeper Layer — objects & held silences (second prose pass)

*(Second prose pass, non-contractual: narrative texture and writing guidance only — no authority,
no claimed path, no acceptance criterion, no verification step. §12's register is unchanged; the
fragments below are content candidates and deliberate silences, not new recorded questions. Where
a named data file holds no prose field, fragments are texture only and gain no schema.)*

**What the forks leave lying around.**

> "Sign board, back side: a child's cut lines, deep, in no alphabet. The board was turned once
> and nobody turned it back." — texture only

> "Paint tin at the fork's foot, lid lost, still usable. Somebody brings paint to this fork on a
> schedule the book does not have." — texture only

> "Sign missing at the north fork. The post has the holes. The board is gone and the holes are
> older than the book's first page."

**What the line leaves lying around.**

> "Segment 1, the yard's edge: mended 7. The mends are a stratigraphy and the fence is the
> shelter's longest sentence about its own edge." — texture only

> "The gap's posts: taller than the rest, capped. Somebody built the gap with more care than the
> fence." — texture only

> "Terminus: two posts and no wire. The line ends the way a sentence ends when the speaker stops,
> not when the thought does."

**Held silences (texture, not register rows).**

- What the board at the north fork said. The holes are older than the book (§1c); the board is
  gone and its words are not in any vocabulary. Texture only.
- Who built the gap with such care. Capped posts, taller than the rest (§1c); the builder is a
  role and the care is unrecorded.

---

## 1.4 Worked examples (non-normative)

**A season of signs (fixed seed).**

> The sign read renders: "the smoke — east (painted, spring)", "the wet road — south", "the two
> lights — west (`wrong_fork`, flat)", and the north fork — `sign_missing`, holes older than the
> book. A `Point` reset moves the wet-road sign to an authored alternate and writes one history
> fact. No map, route or travel state changes anywhere in the diff; the wall map remains wrong on
> purpose and untouched.

**A fence season (fixed seed).**

> The fence read renders sixteen segments in order: segment 1 mended 7, …, segment 9 (`gap_kept`,
> flatly, beside the plate's stone), …, segment 16 (`ends_unmarked`, flatly, where the posts
> stop). The mending duty writes mark-counts through the owners' own repair verbs; the diff adds
> no repair verb and touches no defence state.

---

## 2. Evidence table (checked against the corpus 2026-09-29; re-verify at P0 — Rule 7)

| # | Claim | Evidence | Status |
|---|---|---|---|
| E1 | Folk names beside ids; `Correct`/`Point` verbs add/reset alternates only (batch-4 NR, batch-6 WM precedents). | batch-4/6 plans | PROPOSED (plans are DRAFT) |
| E2 | The wall map is wrong on purpose; its register governs the map-side rhyme. | `.ai/plans/story-expansion-batch-6/the-wall-map-and-the-doorstep-2026-09-29.md` §12 | PROPOSED (plan is DRAFT) |
| E3 | The feeding place's stone sits at the yard's edge; its track/gift discipline is its own register. | `.ai/plans/story-expansion-batch-5/the-feeding-place-and-the-lamps-2026-09-29.md` | PROPOSED (plan is DRAFT) |
| E4 | The Mending observes repairs and adds no verb (DEC-MN-01); mark-counts accrue through owners' calls. | `.ai/plans/the-mending-and-the-second-language-2026-09-29.md` §1 | PROPOSED (plan is DRAFT) |
| E5 | Night Watch / defense owners own perimeter, readiness and raid surfaces. | `docs/plans/UNBLOCK_EXPANSION36_NIGHT_WATCH_INTEGRATION_PLAN.md`; defense owner evidence | LIVE |
| E6 | Location ids can carry fork/segment authoring (batch-3 SC-P0 adjacency). | `.ai/plans/the-signal-chain-and-the-listening-hour-2026-09-29.md` E7 | **VERIFY (P0)** |
| E7 | Whether fence segments can be authored along existing location edges without a geometry model. | additive-precedent discipline | **VERIFY (P0)** |
| E8 | Board render points for sign and fence reads. | batch-3–6 render precedents | **VERIFY (P0)** |

## 3. Authority table

| Concern | Owner (unchanged) | This plan adds |
|---|---|---|
| Maps, navigation | cartography / wall map | nothing; signs are boards, not routes |
| Repairs | each repair owner + The Mending | nothing; mends are observed through their verbs |
| Perimeter, defence | Watch / defense owners | nothing; the fence is a folk boundary |
| Wildlife | SN / Wildlife Trapping | nothing; the gap is folklore |
| Folk names | naming book (batch 4/7) | one read-only citation string |
| Sign/fence folk state | — | `SignRows` + `FenceRows` (pure Core; row facts only) — **DEC-SP-02 / DEC-FC-04** |

## 4. Claimed Paths (proposed; `INT` = integrator-owned)
**Core:** `Assets/Ashfall.Core/Folklore/SignRows.cs` (new, pure), `Edge/FenceRows.cs` (new, pure)
**Data:** `sign_rows.json`, `fence_rows.json`, `sign_lines.json`, `fence_lines.json`
**Host:** board surface (`INT`), repair-observer seam (`INT`, read-only)
**Presentation:** a "Signs" line on the existing travel surface; a "Fence" band on the existing edge/board surface — no new routed panel
**Tests:** `Ashfall.Core.Tests/Folklore/SignRowsTests.cs`, `Edge/FenceRowsTests.cs`, `Ashfall.Core.Tests/Save/SignFenceSaveTests.cs`

## 5. Packages

### SP-P0 — Premise audit (Auditor; read-only): close E6–E8; confirm fork authoring; confirm the wall map surface is read-only here.
### SP-P1 — Sign rows + read (Core + data): folk text + arrows, `wrong_fork`, `fresh_paint`, `sign_missing`, validator row-level. **Accept:** no navigation field exists anywhere (§6.3); determinism; round-trip.
### SP-P2 — Point verb + sign line (host): alternates only, history facts. **Accept:** no map write; the wrong-fork sign renders flatly.
### FC-P1 — Fence rows + read (Core + data): segments, mended counts, `gap_kept`, `ends_unmarked`, validator row-level. **Accept:** no defence/wildlife field exists anywhere (§6.4); determinism; round-trip.
### FC-P2 — Mending hook (host, dark): existing repair verbs only. **Accept:** no new repair verb in the diff.
### X-P1 — Seam hooks (host, dark): one string each way. **Accept:** no cross-reads.

## 6. Acceptance criteria
1. Core authority + host owner + event path + persistence + observable outcome agree (CLAUDE.md "integrated").
2. Ship-dark parity: no sign rows and no fence rows → maps, navigation, repair, defence and board outputs identical on a saved corpus.
3. No-navigation invariant: the diff contains no route, travel or map field of any kind (DEC-SP-01).
4. No-defence invariant: the fence touches no perimeter, raid or wildlife state; the gap is folklore (DEC-FC-01/03).
5. Flat-render invariant: `wrong_fork`, `fresh_paint`, `gap_kept` and `ends_unmarked` render in the same plain type as every other row (DEC-SP-03, DEC-FC-05).
6. No-new-verb invariant: mends accrue through existing repair verbs only (DEC-FC-02).
7. Determinism: identical sign reads, fence reads and mends on replay (`CampaignStreamIds` fork; never `System.Random`).
8. The wall map's, the feeding place's and the Mending's registers stay unanswered (§7).

## 7. Cross-plan boundaries
- **The Wall Map / cartography (batches 6 + Plan 163):** official maps and the wrong-on-purpose wall map are theirs; signs are *boards*, not maps. The wrong-fork sign may not be "fixed" toward truth by any surface.
- **The Feeding Place (batch 5):** the gap faces its stone; the gift/track discipline is its register and the fence's gap never explains it.
- **The Mending (batch 3):** mark-counts and observe-only discipline are inherited whole.
- **Night Watch / defence owners:** perimeter readiness is theirs; the fence is a folk boundary and a raid never consults it.
- **The Walk (batch 5):** the fence line is walked past, not walked to; the two plans share the word "fence" and no state.

## 8. Decision register (proposals — unsigned)
| ID | Decision | Type | Proposed |
|---|---|---|---|
| DEC-SP-01 | Signs touch no navigation, route or map state. | rule | Yes |
| DEC-SP-02 | Sign rows are folk facts: text from the vocabulary, arrows, flags. | architecture | Yes |
| DEC-SP-03 | `wrong_fork` renders flatly and is never corrected toward truth. | tone | Yes — **needs canon note** |
| DEC-FC-01 | The fence touches no perimeter, defence or property state. | rule | Yes |
| DEC-FC-02 | Mends accrue through existing repair verbs; no new verb exists. | rule | Yes |
| DEC-FC-03 | The gap is folklore: no wildlife, breach or ecology field exists. | tone/rule | Yes — **needs canon note** |
| DEC-FC-04 | Fence rows are folk facts: segments, counts, flags. | architecture | Yes |
| DEC-FC-05 | `gap_kept` and `ends_unmarked` render flatly, like every other row. | tone | Yes |
| DEC-X-01 | Both seam hooks ship dark; one string each way. | architecture | Yes |

## 9. Pre-flight Checks
- [ ] Go config/save validator passes (`bin/validate-config` / `bin/ashfall-dev validate-config`)
- [ ] No equivalent existing system found (search `SignPost`, `ForkSign`, `FenceRow`, `BoundaryFolk`, `GapKept`)
- [ ] Premise re-verified (Rule 7); the wall map, feeding place and Mending registers re-read; their silences listed in the handoff as untouchable
- [ ] Signed decisions in hand (at minimum DEC-SP-01, DEC-FC-01, DEC-FC-03)

## 10. Verification
- [ ] `bin/run-scoped-tests` on new tests + existing repair, defence and board tests (list from P0 selector)
- [ ] Defence host selftest with fence rows on and off asserting zero perimeter changes (VERIFY args)
- [ ] Scoped only (<30 s each); ≤10–15 test-edit steps per failure, then auto-flag in `.ai/state.md`; **no full suite** without `RUN FULL TESTS`
- [ ] Handoff in `AI_AGENT_WORKFLOW.md` format; `.ai/state.md` updated

## 11. Stop conditions (Rule 10)
Stop and report if: a sign would need a navigation effect to render; the gap would need a wildlife or breach mechanic; a mend would require a new repair verb; the wrong-fork sign could not render flatly; any path overlaps a live claim.

## 12. Open Mysteries & Deliberate Silence (lore register — no authority, no claimed path)

These questions are **intentionally unanswered**. They are not bugs, not TODOs, and not hidden
work items. They keep the sign wronger than its road and the fence shorter than its reasons. Any
future plan that answers one must name the decision that permits it.

| # | Question | Why it stays open | Who may answer it (later, signed) |
|---|---|---|---|
| SP-OM-1 | Why does the wrong-fork sign stand where it stands? | Always there (§12); moving it is a verb the house has never used and the plan declines to script. | Never — deliberately sealed. |
| SP-OM-2 | What did the north fork's board say? | The holes are older than the book (§1c); the board is gone and its words are in no vocabulary. | Never — texture by omission. |
| SP-OM-3 | Who keeps the fresh paint? | Flagged and unattributed (§12); the painter is a habit and inherits the corpus's role-only discipline. | Never — a rule, not a gap. |
| SP-OM-4 | Is the signpost wrong, or is the road? | The signs are wrong on wood (§1b); which side of the disagreement is truth is exactly what the wall map refuses to say. | Never — the pair stays unresolved with WM-OM-3 (mystery-index §3 discipline). |
| FC-OM-1 | Where would the fence end if it had reasons? | `ends_unmarked` is a terminus without cause (§12); the posts stop and the record declines. | Never — deliberately sealed. |
| FC-OM-2 | Was the gap made or found? | `gap_kept` records the decision and not the origin (§12). | Never — a rule, not a gap. |
| FC-OM-3 | Who built the gap with such care? | Capped posts, taller than the rest (§1c); the builder is a role and the care unrecorded. | Never — texture by omission. |
| FC-OM-4 | Does the wild know about the gap? | The gap is folklore (DEC-FC-03); the wild's knowledge is not modelled and the prose keeps the shiver for the reader. | Never — tone-locked. |
| FC-OM-5 | Are the fence's mends and the plate's mended plate the same keeping? | Two mended things, one household (§0); the kinship is the batch's rhyme and never a claim. | Never — the pair stays open. |
