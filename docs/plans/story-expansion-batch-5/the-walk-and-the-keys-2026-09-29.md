# Feature / Task Plan: The Walk (the habitual circuit that is not patrol) & The Keys (the custody of access)

<!-- UPON INTEGRATION (MANDATORY):
Prepend header at the top saying:
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**
And immediately move this file to .ai/plans/integrated/<category>/
-->

STATUS: DRAFT — awaiting user approval (not self-approved; CLAUDE.md Rule 8 requires `STATUS: APPROVED BY USER` before any code commit). Story batch 5, plans 4 of 4 (subjects 55–56).

> **Subjects covered (2 of the 8 in the "counted days and tended edges" batch):**
> 55. **The Walk** — the circuit somebody walks every day for no assigned reason: named,
> habitual, and nobody's job. (Prefix `WK`.)
> 56. **The Keys** — the custody of access: which key opens what, who holds the ring, and the
> key that was never offered. (Prefix `KY`.)
>
> **Prose / design companions that already exist and are extended, not replaced:**
> `docs/plans/UNBLOCK_EXPANSION36_NIGHT_WATCH_INTEGRATION_PLAN.md` (patrol — the Walk is not
> patrol), `.ai/plans/the-feeding-place-and-the-lamps-2026-09-29.md` (the plate's checker; the
> lamp route — sibling habits), `.ai/plans/other-beginnings-and-the-hard-road-2026-09-29.md`
> (the sergeant's key ring: **its silence governs this plan** — see §7/§12),
> `.ai/plans/the-loan-shelf-and-the-wind-names-2026-09-29.md` (household circulation),
> `.ai/plans/the-wardrobe-and-the-night-shift-2026-09-29.md` (role-only records).
>
> **Not a claim.** This plan claims no path in `WORKTREE_OWNERSHIP.md`, edits no source, no data,
> no ledger and no other plan. It is documentation only. Paths below are *proposed*; `INT` marks
> integrator-owned shared seams.

> **Editorial note (prose texture).** Sections **0**, **1b**, **1c** and **12** carry story texture
> and voice. Sample lines are content candidates for `*.json` rows; they belong in data, never in
> code. No authority, path, decision, acceptance criterion or verification step is changed by any
> prose section.

---

## 0. Prologue — The Route and the Ring

> *"A patrol is a job. A walk is a person. A key is a promise that somebody is keeping the
> doors."*

Every household has someone who walks the same circuit every day — out the north door, along the
fence, past the stacks, back through the yard — and cannot say why. It is not patrol: the Watch
does that, with readiness numbers and sound ranging. The Walk has no readiness, no report, no
objective. It is *named* (everyone knows who walks it) and *purposeless* (nobody can say what it
accomplishes), and it happens every day in all weathers, which is the exact inverse of the night
rota's phantom line — that one is unnamed and purposeful.

The keys are the household's other map of itself: the ring by the door, the tag board, the small
ceremony of who holds what. A key is a promise in metal — *this is kept and you may keep it*. The
shelter's ring carries the store key, the gate key, the box key, and one key on a ring the
sergeant has never offered to anyone. The key book records what each key opens and who holds it,
and where the record cannot say what a key opens, the format keeps the blank.

**Tone & register.** Footpath-plain and custodial. The walk's vocabulary is *circuit, weather,
pace, out, back*; the keys' is *ring, tag, opens, held, offered*. Prose for the walk should read
like weather notes taken on the move; prose for the keys like a door inventory kept by someone
who says "kept" and means it.

**Mystery & texture.** Two silences hold the pair. The walk's **purpose is unnamed in every
record** — the route is authored, the walker is named, and the reason is in no table (§12). And
one key on the ring **opens nothing in any table** — tagged, held, weighed, and blank in the
`opens` column (§12). The sergeant's unoffered key is a *third* silence, and it belongs to
another plan: this one mirrors the row and defers entirely. None of these is a puzzle. They are
what happens when habits and hardware outlast their paperwork.

**The second layer.** A route and a ring are the two ways a household knows its edges: one is
walked, one is turned. Both are daily acts of boundary-keeping that no ledger demands and both
are kept anyway — the walk like a liturgy, the keys like an oath. The plan's quiet thesis is that
a shelter's borders are not walls but *habits*: the circuit someone walks and the promise someone
holds. The unnamed purpose and the blank `opens` column are the places where the habit is
visibly older than its explanation.

---

## 1. Goal & Outcome

> *Design intent: the player should see the walk marked on the day's board and understand it is
> nobody's job — and see the key book's blank and understand it is somebody's promise.*

### 1.1 The Walk (WK)

- **Goal:** A **Walk Row** set (≤ 4 rows: route id, authored waypoint list over existing location
  ids, named-walker convention (a *role-habit*, not a duty — DEC-WK-01), `always_walked` flag)
  and a derived **Walk Read** (walked today / missed, weather word cited, `always_walked`
  rendering); one optional custom hook (evenings-and-memory-work seam) whose only effect is the
  walk's history fact. The walk produces **no readiness, no report and no gameplay effect of any
  kind** (DEC-WK-02).
- **Outcome (observable):** on a fixed seed, the walk read renders "walked, day 214" beside the
  board's other plain lines; a missed walk renders "not walked" with no penalty and no alarm; the
  `always_walked` route renders its flag in the same plain type as every other row; an optional
  weather word (Wind-Names citation) may render beside the day's walk; with no rows every owner
  behaves identically to today; save/load round-trips.
- **Non-Goals:** no patrol or readiness mechanics (Exp. 36 owns them); no stamina or travel
  effects; no scouting, foraging or discovery yields of any kind; no route/exploration changes; no
  new save section; no new routed panel; no Unity.
- **"Done":** §6 acceptance passes via `bin/run-scoped-tests`; ship-dark parity holds; handoff
  lists untouched shared paths.

### 1.2 The Keys (KY)

- **Goal:** A **Key Book** (≤ 16 rows: key id, `opens` (door/box id or blank), held-by-role,
  `never_offered` flag for the sergeant's ring key) and a derived **Key Read** (the ring's
  contents in the board's plain type); one constrained verb — **Pass** (a key may change hands:
  one history fact, role to role, never to a name — DEC-KY-02). The key book records custody and
  nothing else: no lock mechanics, no door state, no access rules (DEC-KY-01).
- **Outcome (observable):** on a fixed seed, the key read renders each key with its `opens` value
  or its blank and its holder-role; a `Pass` writes one history fact; the `never_offered` row
  renders its flag flatly, and the sergeant's key's `opens` blank mirrors the sibling plan's
  silence without resolving it; a key whose `opens` is blank renders blank — never guessed; with
  no rows every owner behaves identically to today; save/load round-trips.
- **Non-Goals:** no lock/door mechanics (door owners keep their meanings); no security or
  contraband effects (The Quiet War / contraband owners keep theirs); no key crafting; no
  resolution of the sibling plan's silence (§7); no new save section; no new routed panel; no
  Unity.
- **"Done":** §6 acceptance passes; ship-dark parity holds; handoff lists untouched shared paths.

### 1.3 The seam between them (X)

- **Goal:** One documented boundary and one optional hook: the walk's route may cite the gate key
  ("the gate is checked because the gate is walked") as one read-only string; and a `Pass` may
  note the walker's habit in one history line. One string each way, shipped dark.

---

## 1b. Texture, Mystery & Voice

**A habit older than its explanation.**

The walk's prose must never supply a reason. Describe the weather, the pace, the waypoints —
"out the north door, along the fence, past the stacks, back through the yard" — and let the
purpose's absence sit exactly where the record leaves it. The `always_walked` flag is the
format's way of saying *this has always happened*, which is not an explanation and is not meant
to be.

**A blank is a promise with its face turned away.**

The key book's `opens` column is the plan's held breath: rows render, tags are counted, hands
pass rings — and where the column is blank, the format prints nothing. The prose must treat the
blank the way the glossary treats its one undefined word: as the place where the record admits
its edge.

**What the player is never told.**

- What the walk accomplishes. No readiness, no report, no yield (DEC-WK-02); the purpose is in
  no table and must never be added (§12).
- Why the walker walks. Whether it is duty, peace or memory is not modelled; the walker is named
  and the reason is not.
- What the blank key opens. It is tagged, held, weighed; the `opens` column is empty by
  construction (§12).
- Why the sergeant has never offered the cage key. **That silence belongs to the sibling plan's
  register** and this plan mirrors the row without touching it (§7).

**Voice — sample fragments (content candidates for `walk_lines.json` / `key_book_lines.json`).**

> "Walked, day 214. North door, fence, stacks, yard. Four waypoints and no purpose, kept for two
> hundred days." — board (WK)

> "Not walked, day 215: rain. The house noticed before the walker did. The walk read does not
> comment; the walk read has one line." — board (WK)

> "Route 1, `always_walked`. The flag is the format's way of saying 'always' without saying
> 'why.'" — board (WK)

> "Key book: gate — opens the gate. Store — opens the store. Ring key — opens: blank. Held by:
> sergeant. Offered: never." — key read (KY)

> "The blank is not missing. The blank is what the record looks like when the promise keeps its
> face turned away." — key read (KY)

**Design texture beats.**

- **The walk produces nothing** (DEC-WK-02). The moment it yields or reports, it becomes patrol
  and the person becomes a job.
- **Blanks render blank** (DEC-KY-03). Never guess, never grey-out, never tooltip.
- **The `never_offered` row renders flatly** (the flat-render family: `unattributed`, `off_route`,
  `home_older`, and now `never_offered`).
- **Passes are role-to-role history facts.** Names never enter the key book; the ring changes
  hands, the hands are roles.

---

## 1c. The Deeper Layer — objects & held silences (second prose pass)

*(Second prose pass, non-contractual: narrative texture and writing guidance only — no authority,
no claimed path, no acceptance criterion, no verification step. §12's register is unchanged; the
fragments below are content candidates and deliberate silences, not new recorded questions. Where
a named data file holds no prose field, fragments are texture only and gain no schema.)*

**What the circuit leaves lying around.**

> "Waypoint 3, the stacks: a stone moved slightly, every day, back. The walk keeps the stone's
> habit and the stone keeps the walk's." — texture only

> "Boot prints, the fence line, in the same two places every morning. The fence is walked past,
> not walked *to*; the prints disagree quietly." — texture only

> "Weather note, in the walk's margin: 'the ash-drift again.' The walker keeps a weather diary
> inside a walk diary inside no system at all."

**What the ring leaves lying around.**

> "Tag board, four tags, three descriptions. The fourth tag is worn smooth on both sides and the
> board is not going to comment."

> "Pass record, day 88: store key, role to role. The ring changed hands at the door, in daylight,
> with both parties present. The ceremony is not in the format; the ceremony is why the format
> exists."

> "The sergeant's key: weighed once, on the ring's own scale. The weight is recorded. Nothing
> else about it is."

**Held silences (texture, not register rows).**

- Whether the stone is part of the walk or the walk is part of the stone. Two habits, one
  waypoint (§1c); the plan keeps the reciprocity unasserted. Texture only.
- What the smooth tag was for. Worn on both sides, unreadable, still on the board (§1c); the
  blank `opens` column is the format's only answer.

---

## 1.4 Worked examples (non-normative)

**A week of walking (fixed seed).**

> Days 213–217: walked, walked, not walked (rain), walked, walked. Each day's line is one plain
> entry; the `always_walked` route renders its flag beside route 1 like every other row. On day
> 215 the household notices the miss before the walker does — and the walk read comments on
> nothing. The optional weather word renders beside day 214's line: "walked — through the
> river-cold", one string, read-only.

**A key book (fixed seed).**

> The ring renders: gate (opens: the gate), store (opens: the store), box key (opens: box 2),
> ring key (opens: **blank**, held: sergeant, `never_offered`). The blank renders blank — same
> width, same type, no grey-out. A `Pass` moves the store key from role `store-keeper` to role
> `night-side` and writes one history fact. The sibling plan's silence about the cage key is
> mirrored in the row and resolved nowhere.

---

## 2. Evidence table (checked against the corpus 2026-09-29; re-verify at P0 — Rule 7)

| # | Claim | Evidence | Status |
|---|---|---|---|
| E1 | Night Watch owns patrol readiness and sound ranging; "the Watch hears and decides nothing". | `docs/plans/UNBLOCK_EXPANSION36_NIGHT_WATCH_INTEGRATION_PLAN.md` | LIVE (closeout) |
| E2 | The plate's checker keeps an unvarying habit (batch-5 FP §1c); the lamp route has an authored order. | `.ai/plans/story-expansion-batch-5/the-feeding-place-and-the-lamps-2026-09-29.md` | PROPOSED (plan is DRAFT) |
| E3 | The sergeant's key ring: "the cage key is on it. The key has not been offered, and the game will not say why." | `.ai/plans/other-beginnings-and-the-hard-road-2026-09-29.md` §1c | PROPOSED (plan is DRAFT) — **its register governs** |
| E4 | Wind-Names citation seam (one string) exists (batch-4 WN-P2). | `.ai/plans/story-expansion-batch-4/the-loan-shelf-and-the-wind-names-2026-09-29.md` | PROPOSED (plan is DRAFT) |
| E5 | Role-only history-fact precedents: handover lines (NS), bell log (BL), vigil ledger (SR). | batch-4/5 plans | PROPOSED (plans are DRAFT) |
| E6 | Location ids can carry waypoint lists; reach adjacency authoring exists (batch-3 SC-P0). | `.ai/plans/the-signal-chain-and-the-listening-hour-2026-09-29.md` E7 | **VERIFY (P0)** |
| E7 | Whether a `Pass` history fact can be stored additively without a key field on items. | additive-precedent discipline | **VERIFY (P0)** |
| E8 | Board render points for walk reads and key reads; band precedents. | batch-3/4/5 render precedents | **VERIFY (P0)** |

## 3. Authority table

| Concern | Owner (unchanged) | This plan adds |
|---|---|---|
| Patrol, readiness | Exp. 36 | nothing; the walk produces no report and no effect |
| Doors, locks | door owners | nothing; the key book records custody only |
| Security, contraband | The Quiet War / contraband owners | nothing |
| Habits/customs | EV | one optional walk custom; provenance silence inherited |
| Weather words | Wind-Names (batch 4) | one read-only citation string |
| The cage key | the sibling plan's register (OB) | mirrors the row; resolves nothing |
| Walk/keys state | — | `WalkRows` + `KeyBook` (pure Core; row facts only) — **DEC-WK-03 / DEC-KY-01** |

## 4. Claimed Paths (proposed; `INT` = integrator-owned)
**Core:** `Assets/Ashfall.Core/Household/WalkRows.cs` (new, pure), `Shelter/KeyBook.cs` (new, pure)
**Data:** `walk_rows.json`, `key_book_rows.json`, `walk_lines.json`, `key_book_lines.json`
**Host:** board surface (`INT`), custom hook (EV seam, `INT`)
**Presentation:** a "Walk" line and a "Keys" band on the existing board/door surface — no new routed panel
**Tests:** `Ashfall.Core.Tests/Household/WalkRowsTests.cs`, `Shelter/KeyBookTests.cs`, `Ashfall.Core.Tests/Save/WalkKeySaveTests.cs`

## 5. Packages

### WK-P0 — Premise audit (Auditor; read-only): close E6–E8; confirm waypoint authoring; confirm the walk read cannot reach readiness surfaces.
### WK-P1 — Walk rows + read (Core + data): routes, `always_walked`, plain one-line entries, validator row-level. **Accept:** no effect field exists anywhere (§6.3); determinism; round-trip.
### WK-P2 — Walk read + custom hook (host, dark): history facts only. **Accept:** missed walks render plainly; no penalty path exists.
### KY-P1 — Key book + rows (Core + data): `opens` (incl. blanks), held-by-roles, `never_offered`, `Pass` verb. **Accept:** blanks render blank; no lock state exists anywhere (§6.4).
### KY-P2 — Key read + pass history (host): role-to-role only. **Accept:** no name field; the sibling silence is mirrored and unresolved (§7).
### X-P1 — Seam hooks (host, dark): one string each way. **Accept:** no cross-reads.

## 6. Acceptance criteria
1. Core authority + host owner + event path + persistence + observable outcome agree (CLAUDE.md "integrated").
2. Ship-dark parity: no walk rows and no key rows → patrol, doors, security, customs and board outputs identical on a saved corpus.
3. No-effect invariant: the walk yields nothing and reports nothing (DEC-WK-02); the diff contains no readiness, stamina, scouting or discovery field.
4. No-lock invariant: the key book records custody and touches no lock or door state (DEC-KY-01).
5. Flat-render invariant: blanks render blank; `always_walked` and `never_offered` render in the same plain type as every other row (DEC-KY-03, DEC-WK-04).
6. Attribution invariant: keys pass between roles, never names (DEC-KY-02).
7. Determinism: identical walk reads, key reads and pass histories on replay (`CampaignStreamIds` fork; never `System.Random`).
8. The sibling plan's key-ring silence is mirrored and unresolved; Exp. 36 and EV registers stay unanswered (§7).

## 7. Cross-plan boundaries
- **Other Beginnings / The Hard Road (batch 2):** the sergeant's cage key is *its* register's silence. This plan mirrors the row (`never_offered`, blank `opens`) and must not name what the key opens or why it is withheld. That pair stays governed by the OB plan's §25.
- **Night Watch (Exp. 36):** patrol and readiness are theirs; the walk is the person, the Watch is the job.
- **The Feeding Place / The Lamps (batch 5):** sibling habits; the checker's tracks and the lamp route are read-only echoes and the tables never merge.
- **Wind-Names (batch 4):** one citation string; weather words stay WN's.
- **The Quiet War / contraband:** key custody has no security semantics; what a key *could* do is out of scope and out of tone.
- **EV:** the walk custom is one custom; who started habits stays EV's register.

## 8. Decision register (proposals — unsigned)
| ID | Decision | Type | Proposed |
|---|---|---|---|
| DEC-WK-01 | The walker is a role-habit, not a duty: named convention, no roster requirement. | design | Yes |
| DEC-WK-02 | The walk produces nothing: no readiness, no report, no yield, no effect. | tone/rule | Yes |
| DEC-WK-03 | Walk rows are authored routes over existing location ids; state is history facts only. | architecture | Yes; confirm in P0 |
| DEC-WK-04 | `always_walked` renders flatly; "always" without "why." | tone | Yes — **needs canon note** |
| DEC-KY-01 | The key book records custody only; no lock, door or access state of any kind. | rule | Yes |
| DEC-KY-02 | Keys pass between roles, never names. | tone | Yes |
| DEC-KY-03 | Blanks render blank: never guessed, greyed or tooltipped. | tone/rule | Yes — **needs canon note** |
| DEC-KY-04 | The `never_offered` row mirrors the sibling plan's silence and resolves nothing. | boundary | Yes |
| DEC-X-01 | Both seam hooks ship dark; one string each way. | architecture | Yes |

## 9. Pre-flight Checks
- [ ] Go config/save validator passes (`bin/validate-config` / `bin/ashfall-dev validate-config`)
- [ ] No equivalent existing system found (search `WalkRoute`, `HabitualWalk`, `KeyBook`, `KeyRing`, `KeyCustody`)
- [ ] Premise re-verified (Rule 7); the OB key-ring register and Exp. 36's boundaries re-read; their silences listed in the handoff as untouchable
- [ ] Signed decisions in hand (at minimum DEC-WK-02, DEC-KY-01, DEC-KY-04)

## 10. Verification
- [ ] `bin/run-scoped-tests` on new tests + existing roster, door and board tests (list from P0 selector)
- [ ] Board host selftest with walk/key rows on and off (VERIFY args)
- [ ] Scoped only (<30 s each); ≤10–15 test-edit steps per failure, then auto-flag in `.ai/state.md`; **no full suite** without `RUN FULL TESTS`
- [ ] Handoff in `AI_AGENT_WORKFLOW.md` format; `.ai/state.md` updated

## 11. Stop conditions (Rule 10)
Stop and report if: the walk read could reach a readiness or yield surface; a key row would need lock state; a blank could not render blank by construction; the sibling plan's silence would have to be answered to build any row; any path overlaps a live claim.

## 12. Open Mysteries & Deliberate Silence (lore register — no authority, no claimed path)

These questions are **intentionally unanswered**. They are not bugs, not TODOs, and not hidden
work items. They keep the circuit older than its purpose and the ring heavier than its record.
Any future plan that answers one must name the decision that permits it.

| # | Question | Why it stays open | Who may answer it (later, signed) |
|---|---|---|---|
| WK-OM-1 | What does the walk accomplish? | Nothing, by construction (DEC-WK-02); the purpose is in no table and adding one would end the walk. | Never — deliberately sealed. |
| WK-OM-2 | Why does the walker walk? | Duty, peace and memory all fit the record; the plan refuses to choose (inherits FA-OM-3's discipline). | Never — texture by omission. |
| WK-OM-3 | Is the stone part of the walk or the walk part of the stone? | Two habits, one waypoint (§1c); the reciprocity is observed and unasserted. | Never — texture by omission. |
| WK-OM-4 | Was the walk ever not walked before the rain? | Two hundred days of records; whether the series has a gap is not asserted (inherits NS-OM-2's discipline). | Never — tone-locked. |
| WK-OM-5 | Does the walk go further than the waypoints? | The route is authored and the waypoints end; what the walker does past the last one is not modelled. | Never — a rule, not a gap. |
| KY-OM-1 | What does the blank key open? | Blank by construction (DEC-KY-03); the promise keeps its face turned away. | Never — deliberately sealed. |
| KY-OM-2 | Why has the sergeant never offered the cage key? | **Governed by the sibling plan's register** (OB, batch 2); this plan mirrors the row and defers entirely. | The OB plan's owner only (its §25 governs). |
| KY-OM-3 | What was the smooth tag? | Worn on both sides, unreadable, still on the board (§1c); the blank column is the format's only answer. | Never — texture by omission. |
| KY-OM-4 | Who weighed the ring's keys, and why once? | The weight is recorded and the scale is unowned (§1c); metrology of the ring is not a data concern. | Never — texture by omission. |
| KY-OM-5 | Do the walk and the ring guard the same boundary? | One is walked, one is turned (§0); whether they are the same border-keeping is the batch's closing image and never a claim (mystery-index §3 discipline). | Never — the pair stays open. |
