# Feature / Task Plan: The Hearth (the order of the seats by the fire) & The Sweeping (the daily sweep and what it finds)

<!-- UPON INTEGRATION (MANDATORY):
Prepend header at the top saying:
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**
And immediately move this file to .ai/plans/integrated/<category>/
-->

STATUS: DRAFT — awaiting user approval (not self-approved; CLAUDE.md Rule 8 requires `STATUS: APPROVED BY USER` before any code commit). Story batch 6, plans 3 of 4 (subjects 61–62).

> **Subjects covered (2 of the 8 in the "inward winter" batch):**
> 61. **The Hearth** — the order of the seats by the fire: who sits where, why it is not fair,
> and the seat kept for someone not yet home. (Prefix `HT`.)
> 62. **The Sweeping** — the daily sweep as custom: the order of the rooms, the small things the
> broom finds, and the etiquette of putting them back. (Prefix `SW`.)
>
> **Prose / design companions that already exist and are extended, not replaced:**
> `.ai/plans/ration-wars-2026-09-29.md` (the Table Rule — *portions*; the hearth rule is
> *places*), `docs/plans/C1_planintegration[4].md` (one food authority: the table's meaning),
> `.ai/plans/story-expansion-batch-5/the-day-bell-and-the-wood-line-2026-09-29.md` (the wood that
> feeds the fire), `docs/plans/PLAYER_FACING_TRIAD_B_EXERCISE_DREAM_SANITATION_INTEGRATION_PLAN.md`
> (sanitation **mechanics** are its; the sweep is custom), `.ai/plans/shelter-governance-2026-09-29.md`
> (**its empty-chair silence governs §7/§12**).
>
> **Not a claim.** This plan claims no path in `WORKTREE_OWNERSHIP.md`, edits no source, no data,
> no ledger and no other plan. It is documentation only. Paths below are *proposed*; `INT` marks
> integrator-owned shared seams.

> **Editorial note (prose texture).** Sections **0**, **1b**, **1c** and **12** carry story texture
> and voice. Sample lines are content candidates for `*.json` rows; they belong in data, never in
> code. No authority, path, decision, acceptance criterion or verification step is changed by any
> prose section.

---

## 0. Prologue — The Warm Side and the Broom

> *"A table rule is about how much. A hearth rule is about how near. The second one causes more
> trouble."*

The hearth is the shelter's oldest parliament. There are six seats near the fire and twelve
people, and the order in which they are filled is a piece of law nobody has written down: the
coldest first, the eldest kept, the guest placed, the child on the stone. The order is *not
fair* — the house knows this and says so — and it is kept anyway, because warmth arranged by
argument is warmth wasted. The hearth rule is about places, not temperature; the thermal owner
keeps the temperature and the household keeps the manners.

The sweeping is the other daily order. Every morning one person sweeps: the same rooms in the
same sequence, ending where it ends. The sweep finds things — a button, a pin, a blank coin, a
folded note with nothing in it — and the sweep's etiquette, older than the rota, is that small
finds go back where they were found and are *said aloud* at the fire. The finds are recorded as
shapes and words; the record is one line per find and has no column for value.

**Tone & register.** Hearthside-plain and broom-quiet. The hearth's vocabulary is *seat, near,
kept, cold, placed*; the sweep's is *room, order, found, put back, said aloud*. Prose for the
hearth should read like the minutes of a meeting nobody called; prose for the sweep like a
morning's inventory in a house that has agreed to be tidy.

**Mystery & texture.** Two silences hold the pair. The hearth order has a **seat kept for
someone not yet home** — the seat is placed, warmed, and empty, and the house does not say who it
is for (§12). And the sweep's finds arrive **every third morning**, punctual as rent, on a
sequence the broom's own keeper cannot predict (§12). Neither is a puzzle. Both are what happens
when domestic order keeps its cadence past its explanations.

**The second layer.** The hearth and the broom are the two shapes domesticity gives to *time*:
the hearth is the evening's order, the sweep is the morning's. Both are arrangements of people
around warmth and cleanliness that no rule compels and both are kept with the tenacity of law —
which is the plan's quiet thesis: the manners of a shelter are its real constitution, and they
are written nowhere, and they are enforced by everyone.

---

## 1. Goal & Outcome

> *Design intent: the player should look at the fire and read the household's hierarchy in who
> sits where — and watch the broom's morning round and understand the house has decided to be
> tidy in an order.*

### 1.1 The Hearth (HT)

- **Goal:** A **Hearth Rows** table (≤ 8 rows: seat id, position-near-fire, `kept_for` (a
  role-habit, never a name — DEC-HT-03), `placed` flag for guest seats, `seat_held` flag for the
  kept-and-empty seat) and a derived **Hearth Read** (tonight's seating as *roles*, the order's
  plain fairness note, the held seat rendered flatly); one optional custom hook
  (evenings-and-memory-work seam) whose only effect is the evening's seating being recorded. No
  thermal, morale or comfort effect of any kind (DEC-HT-02).
- **Outcome (observable):** on a fixed seed, the hearth read renders six seats with their
  positions and tonight's role-seating; the `seat_held` seat renders its flag in the same plain
  type as every other row, empty; a guest's `placed` seat renders beside its role; with no rows
  every owner behaves identically to today; save/load round-trips.
- **Non-Goals:** no warmth/comfort/temperature mechanics (thermal and Quiet owners keep their
  meanings); no social hierarchy mechanics (relationships and Assembly keep theirs); no seating
  enforcement; no resolution of the governance plan's empty-chair silence (§7); no new save
  section; no new routed panel; no Unity.
- **"Done":** §6 acceptance passes via `bin/run-scoped-tests`; ship-dark parity holds; handoff
  lists untouched shared paths.

### 1.2 The Sweeping (SW)

- **Goal:** A **Sweep Rows** table (sweep order over existing room ids; find vocabulary closed
  (≤ 10 kinds: `button`, `pin`, `blank_coin`, `blank_note`, `string`, `bone`, `glass`, `lid`,
  `chalk_end`, `unnamed`); `every_third_morning` cadence flag) and a derived **Sweep Read**
  (today's round, finds this week, the put-back etiquette as a plain note); one optional custom
  hook whose only effect is the round being recorded. Finds are **facts of one line** and gain no
  value, weight or use (DEC-SW-02).
- **Outcome (observable):** on a fixed seed, the sweep read renders the room order and this
  week's finds; every-third-morning cadence seeds a find from the closed vocabulary; finds render
  "put back, said aloud" as the etiquette's plain note; the `unnamed` kind renders flatly as one
  kind among ten; with no rows every owner behaves identically to today; save/load round-trips.
- **Non-Goals:** no sanitation or disease mechanics (Triad B and `DiseaseSystem` keep their
  meanings); no item creation (finds are *records*, not inventory — DEC-SW-02); no cleaning
  effects; no new save section; no new routed panel; no Unity.
- **"Done":** §6 acceptance passes; ship-dark parity holds; handoff lists untouched shared paths.

### 1.3 The seam between them (X)

- **Goal:** One documented boundary and one optional hook: a find may be *said aloud* at the
  hearth as one read-only string ("a button, said at the fire"), and the hearth read may cite the
  sweep's morning order. One string each way, shipped dark.

---

## 1b. Texture, Mystery & Voice

**The order is not fair and the house knows it.**

The hearth read's fairness note is the plan's honesty device: the seating renders plainly and the
note says "not fair" in the same plain type. Prose must never moralise the order — describe the
arrangement and the cold, and let the twelve-against-six arithmetic do its own work.

**A find is a line, not a thing.**

The sweep's finds are one-line records with no value field. The `unnamed` kind is the closed
vocabulary's one blank — the broom finds something and the vocabulary has no word for it, and the
record says so and stops. This is the corpus's blank motif again, deliberately and sparingly
recurred.

**What the player is never told.**

- Who the held seat is for. `kept_for` is a role-habit and the seat is empty (§12); the house
  keeps it and the record declines the name (inherits the corpus's no-names discipline).
- Why the finds come every third morning. Cadence, not cause (§12); the broom's keeper cannot
  predict it and the plan refuses to.
- What the `unnamed` find is. One kind among ten, rendered flatly; the vocabulary's blank is the
  format's only answer.
- Whether the sweep order means anything. The rooms are swept in an authored sequence; its origin
  is unmodelled and the etiquette is older than the rota.

**Voice — sample fragments (content candidates for `hearth_lines.json` / `sweep_lines.json`).**

> "Hearth, evening: cold hands nearest, eldest kept, guest placed, child on the stone. Six seats,
> twelve people, one order. The order is not fair. The order is warm." — hearth read (HT)

> "Seat 4: kept, empty, warmed. The house does not say who for and the fire does not ask." —
> hearth read (HT)

> "Sweep, morning: two rooms, the third is last because it has always been last. Found: a pin.
> Put back. Said aloud at the fire." — sweep read (SW)

> "Found: unnamed. The vocabulary has ten words and this is not one of them. The record says
> 'unnamed' and stops, which is the etiquette." — sweep read (SW)

**Design texture beats.**

- **No effects of any kind** (DEC-HT-02, DEC-SW-02): the hearth moves no number and the broom
  creates no item. Folklore and manners only.
- **The held seat renders flatly** (the flat-render family grows: `seat_held` joins
  `unattributed`, `off_route`, `home_older`, `never_offered`, `clear_morning`).
- **Roles only, never names** (DEC-HT-03): the seat is kept for a *kind of person*, and the
  household knows perfectly well what that means.
- **The `unnamed` find is the vocabulary's one blank** (DEC-SW-03): the rhyme with the glossary's
  blank is deliberate and singular.

---

## 1c. The Deeper Layer — objects & held silences (second prose pass)

*(Second prose pass, non-contractual: narrative texture and writing guidance only — no authority,
no claimed path, no acceptance criterion, no verification step. §12's register is unchanged; the
fragments below are content candidates and deliberate silences, not new recorded questions. Where
a named data file holds no prose field, fragments are texture only and gain no schema.)*

**What the fire side leaves lying around.**

> "Seat 4's cushion, turned every week. The turning is somebody's small defence against the damp
> and the house calls it 'keeping the seat.'"

> "The guest seat is placed two fingers nearer the fire than the eldest's. Nobody has ever
> measured this. Everybody knows." — texture only

> "The order was argued once, at length, on a night nobody enjoyed. The order is the same. The
> arguing is not repeated." — texture only

**What the broom leaves lying around.**

> "Third-morning tally: find, every time, in the same two rooms. The tally has run thirty weeks.
> The pattern is recorded as a pattern and explained as nothing."

> "The unnamed find, put back where it was found. Put-back is the etiquette and the etiquette has
> no exceptions, which is why the find is still there."

> "Bristles, replaced at the season's turn. The old bristles go to the fire and the fire says
> nothing."

**Held silences (texture, not register rows).**

- Whether the held seat's person is coming back. The seat is kept and warmed (§12); the house's
  hope is not a field and the plan declines to add one. Texture only.
- Why the third morning, and why those two rooms. Cadence, recorded as pattern (§1c); cause is
  refused exactly as the frost book refuses meaning.

---

## 1.4 Worked examples (non-normative)

**An evening at the fire (fixed seed).**

> Six seats render with positions; tonight's roles: two night-side (cold hands), the eldest, a
> guest `placed`, a child on the stone, and seat 4 — `seat_held`, empty, warmed, rendered flatly
> like every other row. The fairness note reads "not fair" in the same plain type. The optional
> custom records the seating and nothing else; no thermal or morale state is touched anywhere in
> the diff.

**A sweep week (fixed seed).**

> Seven mornings, the same three-room order (the third room last, always last). Finds on the
> third morning: `pin`; the sixth: `blank_note`; the ninth: `unnamed` — rendered flatly, put back,
> said aloud at the fire. The fire-side line is one read-only string: "a pin, said at the fire."
> Every find is one line; the diff contains no item, no weight and no value field of any kind.

---

## 2. Evidence table (checked against the corpus 2026-09-29; re-verify at P0 — Rule 7)

| # | Claim | Evidence | Status |
|---|---|---|---|
| E1 | The Table Rule owns *expected portions* and resentment; one food authority owns meals. | `.ai/plans/ration-wars-2026-09-29.md` §1b; `docs/plans/C1_planintegration[4].md` | LIVE (plans exist) |
| E2 | Thermal/atmosphere owners own temperature and air (`ShelterThermalSystem`, `ShelterAtmosphereSystem`). | `.ai/plans/works-below-and-machine-in-the-walls-2026-09-29.md` §1.1 | LIVE |
| E3 | Triad B owns sanitation mechanics; `DiseaseSystem` owns infection. | `docs/plans/PLAYER_FACING_TRIAD_B_EXERCISE_DREAM_SANITATION_INTEGRATION_PLAN.md` | LIVE (plan exists) |
| E4 | Shelter Governance's register holds the empty-chair silence ("the empty chairs are not furniture and must never be furnished"). | `.ai/plans/shelter-governance-2026-09-29.md` §1c/§12 | PROPOSED (plan is DRAFT) — **its register governs** |
| E5 | Custom machinery hosts authored customs (EV seam; seating/sweep customs are rows). | batch-3–6 custom hooks | PROPOSED (soft dependency) |
| E6 | Role-only attribution precedents (handover, fire-side, bell log, vigil). | batch-4–6 plans | PROPOSED (plans are DRAFT) |
| E7 | Whether find records can live entirely in derived reads without any inventory touch. | derived-render precedents (batch-5/6) | **VERIFY (P0)** |
| E8 | Evening/morning render points for hearth and sweep reads. | batch-3–6 render precedents | **VERIFY (P0)** |

## 3. Authority table

| Concern | Owner (unchanged) | This plan adds |
|---|---|---|
| Portions, resentment | The Table Rule (RW) | nothing; the hearth rule is places, not portions |
| Temperature, air | thermal / atmosphere owners | nothing; warmth in prose is weather of the room |
| Sanitation, disease | Triad B / `DiseaseSystem` | nothing; the sweep is custom, not hygiene |
| Empty chairs | Shelter Governance's register | mirrors nothing; the held seat is a *different* silence (§7) |
| Custom provenance | EV | one optional seating custom and one sweep custom |
| Folk state | — | `HearthRows` + `SweepRows` (pure Core; row facts only) — **DEC-HT-01 / DEC-SW-01** |

## 4. Claimed Paths (proposed; `INT` = integrator-owned)
**Core:** `Assets/Ashfall.Core/Household/HearthRows.cs` (new, pure), `Household/SweepRows.cs` (new, pure)
**Data:** `hearth_rows.json`, `sweep_rows.json`, `hearth_lines.json`, `sweep_lines.json`
**Host:** evening/morning render (`INT`), custom hooks (EV seam, `INT`)
**Presentation:** a "Fire-side" seating band on the existing evening surface; a "Sweep" line on the existing morning surface — no new routed panel
**Tests:** `Ashfall.Core.Tests/Household/HearthRowsTests.cs`, `SweepRowsTests.cs`, `Ashfall.Core.Tests/Save/HearthSweepSaveTests.cs`

## 5. Packages

### HT-P0 — Premise audit (Auditor; read-only): close E7–E8; confirm the governance register's empty-chair wording and this plan's boundary; confirm no thermal hook exists.
### HT-P1 — Hearth rows + read (Core + data): seats, `kept_for` (roles), `placed`, `seat_held`, validator row-level. **Accept:** no effect field exists anywhere (§6.3); determinism; round-trip.
### HT-P2 — Seating render + custom hook (host, dark): history facts only. **Accept:** the held seat renders flatly; no name field exists.
### SW-P1 — Sweep rows + read (Core + data): room order, closed find vocabulary, third-morning cadence, `unnamed` blank. **Accept:** no item/value field exists anywhere (§6.4); determinism; round-trip.
### SW-P2 — Sweep render + custom hook (host, dark): finds are one-line records. **Accept:** the put-back etiquette renders as plain note; no inventory touch.
### X-P1 — Seam hooks (host, dark): one string each way. **Accept:** no cross-reads.

## 6. Acceptance criteria
1. Core authority + host owner + event path + persistence + observable outcome agree (CLAUDE.md "integrated").
2. Ship-dark parity: no hearth rows and no sweep rows → table, thermal, sanitation, governance and board outputs identical on a saved corpus.
3. No-effect invariant: the hearth moves no number (DEC-HT-02); the diff contains no morale, comfort or temperature field.
4. No-item invariant: sweep finds are records and never inventory (DEC-SW-02); no value, weight or use field exists.
5. Flat-render invariant: `seat_held` and `unnamed` render in the same plain type as every other row (DEC-HT-04, DEC-SW-03).
6. Attribution invariant: seats are kept for roles, never names (DEC-HT-03).
7. Determinism: identical seatings, finds and renders on replay (`CampaignStreamIds` fork; never `System.Random`).
8. The governance plan's empty-chair silence and EV's custom provenance stay unanswered (§7).

## 7. Cross-plan boundaries
- **Shelter Governance:** its empty-chair silence ("not furniture, never furnished") governs the Assembly's schism chairs. The hearth's `seat_held` is a *kept-and-warmed* seat for the absent — a different silence, held in this register, and the two must never be conflated (§12 pairs them deliberately).
- **The Table Rule / one food authority:** portions and meals are theirs; the hearth's law is proximity and it has no say over food.
- **Triad B / `DiseaseSystem`:** sanitation is theirs; the sweep is manners, not hygiene, and its finds change no state.
- **The Wood Line / The Lamps (batch 4–5):** the fire is fed and lit by theirs; the hearth is who sits near it.
- **The Cabinet (batch 3):** finds are *records*, not exhibits; a find becomes an object only through the Cabinet's own verb and by a different plan's act.
- **EV:** customs are theirs; who started the order and the etiquette stays its register's.

## 8. Decision register (proposals — unsigned)
| ID | Decision | Type | Proposed |
|---|---|---|---|
| DEC-HT-01 | Hearth rows are folk facts only: seats, roles, flags; no state beyond the table. | architecture | Yes |
| DEC-HT-02 | No thermal, comfort or morale effect of any kind. | rule | Yes |
| DEC-HT-03 | Seats are kept for roles, never names. | tone | Yes |
| DEC-HT-04 | `seat_held` renders flatly — empty, warmed, unexplained. | tone | Yes — **needs canon note** |
| DEC-SW-01 | Sweep rows are folk facts: order, finds, cadence; derived renders only. | architecture | Yes |
| DEC-SW-02 | Finds are records, never inventory: no value, weight or use field exists. | rule | Yes |
| DEC-SW-03 | `unnamed` is the vocabulary's one blank; rendered flatly like every kind. | tone | Yes — **needs canon note** |
| DEC-SW-04 | The put-back etiquette has no exceptions — which is why the finds remain. | tone | Yes |
| DEC-X-01 | Both seam hooks ship dark; one string each way. | architecture | Yes |

## 9. Pre-flight Checks
- [ ] Go config/save validator passes (`bin/validate-config` / `bin/ashfall-dev validate-config`)
- [ ] No equivalent existing system found (search `Hearth`, `Seating`, `Sweep`, `Chore`, `CleaningCustom`)
- [ ] Premise re-verified (Rule 7); the governance register's empty-chair silence re-read; it is listed in the handoff as untouchable
- [ ] Signed decisions in hand (at minimum DEC-HT-02, DEC-SW-02, DEC-HT-04)

## 10. Verification
- [ ] `bin/run-scoped-tests` on new tests + existing thermal, table and custom tests (list from P0 selector)
- [ ] Inventory selftest asserting zero touches with rows on and off (VERIFY args)
- [ ] Scoped only (<30 s each); ≤10–15 test-edit steps per failure, then auto-flag in `.ai/state.md`; **no full suite** without `RUN FULL TESTS`
- [ ] Handoff in `AI_AGENT_WORKFLOW.md` format; `.ai/state.md` updated

## 11. Stop conditions (Rule 10)
Stop and report if: any surface would require a comfort or morale effect; a find would need to become inventory; the held seat could not render flatly without a name; the governance silence would have to be answered to build a row; any path overlaps a live claim.

## 12. Open Mysteries & Deliberate Silence (lore register — no authority, no claimed path)

These questions are **intentionally unanswered**. They are not bugs, not TODOs, and not hidden
work items. They keep the fire taller than its order and the broom older than its reasons. Any
future plan that answers one must name the decision that permits it.

| # | Question | Why it stays open | Who may answer it (later, signed) |
|---|---|---|---|
| HT-OM-1 | Who is the held seat for? | `kept_for` is a role-habit and the seat is empty (DEC-HT-04); the name is refused by construction. | Never — deliberately sealed. |
| HT-OM-2 | Is the person the seat is kept for coming back? | The house's hope is not a field (§1c); the plan declines to add one. | Never — a rule, not a gap. |
| HT-OM-3 | Why is the guest nearer the fire than the eldest? | Observed and unmeasured (§1c); the two fingers are folklore and the folklore is not a rule. | Never — texture by omission. |
| HT-OM-4 | Was the order argued only once? | One night, at length (§1c); whether the order pre-dates the arguing is unasserted. | Never — texture by omission. |
| HT-OM-5 | Is the held seat the same silence as the Assembly's empty chairs? | Different silences, deliberately paired (§7, §12); the conflation is refused and the rhyme kept. | Never — the pair stays unresolved (mystery-index §3 discipline). |
| SW-OM-1 | Why every third morning, in those two rooms? | Cadence recorded as pattern, cause refused (§12); the broom's keeper cannot predict it. | Never — deliberately sealed. |
| SW-OM-2 | What is the `unnamed` find? | The vocabulary's one blank (DEC-SW-03); the record says `unnamed` and stops. | Never — deliberately sealed. |
| SW-OM-3 | Who started the put-back etiquette? | Older than the rota (§0); custom provenance is EV's register and is inherited unchanged. | EV plan's owner (its register governs). |
| SW-OM-4 | Why is the third room always last? | The order is authored; its origin is unmodelled and the sweep keeps it anyway. | Never — texture by omission. |
| SW-OM-5 | Do the finds know they are being counted? | The tally records patterns (§1c); the plan refuses to animate the pattern and the reader may keep the shiver. | Never — tone-locked. |
