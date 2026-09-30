# Feature / Task Plan: The Day Bell (the shelter's everyday calls) & The Wood Line (the supply of warmth)

<!-- UPON INTEGRATION (MANDATORY):
Prepend header at the top saying:
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**
And immediately move this file to .ai/plans/integrated/<category>/
-->

STATUS: DRAFT — awaiting user approval (not self-approved; CLAUDE.md Rule 8 requires `STATUS: APPROVED BY USER` before any code commit). Story batch 4, plans 2 of 4 (subjects 43–44).

> **Subjects covered (2 of the 8 in the "rhythm and circulation" batch):**
> 43. **The Day Bell** — the shelter's everyday calls: muster, meal, curfew, come-in. A closed
> ring table, one verb, one line per ring. Emergencies are not this bell. (Prefix `BL`.)
> 44. **The Wood Line** — the supply of warmth itself: who cuts, hauls and stacks, what the week
> burns, and the old cordwood on the ridge nobody remembers stacking. (Prefix `WD`.)
>
> **Prose / design companions that already exist and are extended, not replaced:**
> `docs/expansions/wave3/expansion_23_the_alarm_plan.md` (Exp. 23 *The Alarm* owns fire,
> emergency response and the alarm bell — `item_alarm_bell` semantics are theirs),
> `.ai/plans/the-signal-chain-and-the-listening-hour-2026-09-29.md` (the chain's fire pits and its
> closed pattern table — sibling grammar), `.ai/plans/evenings-and-memory-work-2026-09-29.md`
> (custom machinery), `.ai/plans/ration-wars-2026-09-29.md` (fuel tallies in the Book),
> `docs/plans/UNBLOCK_EXPANSION36_NIGHT_WATCH_INTEGRATION_PLAN.md` (night rosters).
>
> **Not a claim.** This plan claims no path in `WORKTREE_OWNERSHIP.md`, edits no source, no data,
> no ledger and no other plan. It is documentation only. Paths below are *proposed*; `INT` marks
> integrator-owned shared seams.

> **Editorial note (prose texture).** Sections **0**, **1b**, **1c** and **12** carry story texture
> and voice. Sample lines are content candidates for `*.json` rows; they belong in data, never in
> code. No authority, path, decision, acceptance criterion or verification step is changed by any
> prose section.

---

## 0. Prologue — The Calls and the Warmth

> *"A bell tells a household what time it is. A woodpile tells it what winter is. Between them
> they invent the word 'us'."*

The day bell is the shelter's oldest software. Four rings, learned by everybody within a week:
muster, meal, curfew, come-in. Nothing about the rings is written down in the fiction — the
table is the shelter's own tongue, and the bell is the sentence it says most often. Emergencies
belong to the alarm bell and the shout; this bell is the sound of nothing being wrong, repeated
daily until it becomes the sound of home.

The wood line is what the day bell is made of: warmth with a supply chain. Someone cuts, someone
hauls, someone stacks, and the stack is the household's honest forecast — a woodpile is a
statement about the winter you expect. The kitchen stove, the boiler, the chain's fire pits, the
siege hearths: everything warm in this world is downstream of the line. And on the ridge above
the last relay point there are cordwood stacks in a pattern nobody taught this shelter, weathered
past anyone's memory of stacking them.

**Tone & register.** Rhythmic and domestic-forested. The bell's vocabulary is *ring, call,
ringer, answered*; the wood's is *cut, cord, haul, stack, burn, weather*. Prose for the bell
should feel like a day being counted aloud; prose for the wood like a supply officer's notebook
with sawdust in it. Neither is ceremonial. The bell is a tool; the wood is work.

**Mystery & texture.** Two silences hold the pair. The ring table contains a **fifth ring** that
is defined, learnable and assigned to no occasion — the shelter rings four, and the fifth is
there (§12). And the ridge's cordwood stacks are *patterned* — a stacking method no living
survivor uses and every stack obeys (§12). Neither is a puzzle. Both are the household
inheriting habits from hands it cannot name.

**The second layer.** A bell and a woodpile are both ways of *storing time*. The bell stores the
day — its rings are hours made audible; the woodpile stores the year — its height is a winter
measured in advance. Everything domestic is a storage technology for time: the shelf stores
obligation, the guest book stores hospitality, and these two store rhythm and warmth. The fifth
ring and the patterned stacks are the same mystery at two scales: something here keeps time
better than the people who inherited it.

---

## 1. Goal & Outcome

> *Design intent: the player should hear the meal ring and feel the day turn over — and glance at
> the wood stack and know exactly how much winter is left in it.*

### 1.1 The Day Bell (BL)

- **Goal:** A closed **Ring Table** (≤ 6 rows: ring id, cadence, meaning, `occasion_unassigned`
  flag for the fifth ring), one verb — **Ring** (authored duty-role gated), a one-line **Bell
  Log** (ring, ringer-role, day, `answered` count read from the roster seam), and rendering of
  the day's rings on the existing board/briefing surface. Emergency signalling is explicitly out
  of scope: the alarm bell, the shout and the cascade belong to Exp. 23 (DEC-BL-04).
- **Outcome (observable):** on a fixed seed, the meal ring fires at its authored cadence through
  the roster's ordinary muster flow; the bell log gains one line with the ringer's *role* and the
  answered count; the fifth ring may be rung by the same duty verb and renders with its
  `occasion_unassigned` mark and no meaning; with no ring rows every owner behaves identically to
  today; save/load mid-day round-trips.
- **Non-Goals:** no emergency signalling (Exp. 23 owns it); no timekeeping/clock changes; no new
  audio pipeline (existing audio surfaces only, or ships dark — DEC-BL-03); no coercion: ringing
  a call moves nobody by force; no new save section; no new routed panel; no Unity.
- **"Done":** §6 acceptance passes via `bin/run-scoped-tests`; ship-dark parity holds; handoff
  lists untouched shared paths.

### 1.2 The Wood Line (WD)

- **Goal:** A derived **Wood Ledger** (cut / hauled / stacked / burned by week, read from the
  inventory owner's fuel counts and the existing expedition yields) plus one authored **Wood
  Rows** table (source sites and their yields, through existing gather seams); one optional
  **Wood Day** custom (evenings-and-memory-work seam, Mending Day precedent) whose only effect is
  existing gather/haul work; and a **Stack View** reading the household's current fuel as a
  cord-height with a plain week-remaining figure. Nothing in this plan cuts anything by itself.
- **Outcome (observable):** on a fixed seed, a gather expedition's fuel yield appears in the wood
  ledger as its week's `cut`; stove and boiler consumption (existing owners) appear as `burned`;
  the stack view derives cord-height and weeks-remaining from real counts; the optional Wood Day
  custom routes to existing gather verbs only; with no rows and no custom every owner behaves
  identically to today; save/load mid-week round-trips.
- **Non-Goals:** no new resource (fuel is the existing stock); no forest/regrowth model (Second
  Nature owns ecology); no fuel-economy changes; no weather mechanics; no new save section; no new
  routed panel; no Unity.
- **"Done":** §6 acceptance passes; ship-dark parity holds; handoff lists untouched shared paths.

### 1.3 The seam between them (X)

- **Goal:** One documented boundary and one optional hook: the day's ring cadence may surface one
  wood line beside it ("curfew — stack at 4 weeks"), and a Wood Day custom may begin with the
  muster ring. One string and one boolean, both shipped dark.

---

## 1b. Texture, Mystery & Voice

**A ring is a word everyone already knows.**

The ring table is a *language* in exactly the sense batch 3's pattern table is: closed, learned,
and older than its documentation. Write each ring as something the shelter *says*, not something
the game does — the meal ring is not a notification, it is a sentence with a whole household
finishing it.

**A woodpile is a forecast you can stand in.**

The stack view should render in cords and weeks, and the prose should treat the stack as the
household's most honest public document: nobody lies to the woodpile, because the winter audits
it personally.

**What the player is never told.**

- What the fifth ring is for. It is defined, learnable, and `occasion_unassigned` (§12). Whether
  it awaits an occasion or remembers one is not modelled and must not be.
- Who stacked the ridge cordwood. The pattern is consistent across every stack and matches no
  living practice (§12); the survey that would date it does not exist.
- Whether the bell predates the shelter. It hangs where the muster board hangs; its casting, its
  age and its first ringer are not authored.
- Why the wood ledger keeps week-granularity when the stock is counted daily. The format's
  coarseness is deliberate; the week is the household's true unit of warmth.

**Voice — sample fragments (content candidates for `day_bell_lines.json` / `wood_line_lines.json`).**

> "Ring table, four rows of habit and one row of patience. The fifth ring is in everybody's ears
> and nobody's day." — muster board (BL)

> "Curfew ring, day 214. Twelve answered. The bell does not count; the roster counts. The bell
> only asks." — bell log (BL)

> "Come-in ring, dusk. It is the only call that means 'stop being outside,' and it is rung by the
> person whose job is the least popular and most trusted." — bell log (BL)

> "Stack at the yard: eleven cords. Winter at the current burn: nine weeks. The stack is a
> forecast you can stand in." — wood line (WD)

> "Ridge stacks, above link 2: four rows, patterned, weathered past memory. Nobody cuts from
> them. Nobody proposes to." — wood line (WD)

**Design texture beats.**

- **Rings move nobody by force** (DEC-BL-02). The bell is a request with a cadence; coercion
  would turn a household into a barracks.
- **The fifth ring renders without meaning.** Never add a tooltip; the `occasion_unassigned` mark
  is its whole presentation.
- **The wood ledger observes; it never cuts** (inherits MN-P1 discipline). The plan is a
  projection of work that other owners already do.
- **Week-granularity is a fiction decision.** Resist daily resolution; the week is the warm
  unit.

---

## 1c. The Deeper Layer — objects & held silences (second prose pass)

*(Second prose pass, non-contractual: narrative texture and writing guidance only — no authority,
no claimed path, no acceptance criterion, no verification step. §12's register is unchanged; the
fragments below are content candidates and deliberate silences, not new recorded questions. Where
a named data file holds no prose field, fragments are texture only and gain no schema.)*

**What the bell leaves lying around.**

> "Rope, replaced twice. The knot at the pull is the same knot both times, and knots are how this
> shelter signs things."

> "Bell log, day 88: muster, answered 11. The twelfth was the ringer. The log does not mark this;
> the log has one column."

> "Fifth ring, rung once at a test. The log entry is complete — ring, role, day — and the meaning
> column is blank by format, not by oversight."

**What the wood leaves lying around.**

> "Cut tally: 3 cords, ridge 4. Written in the same hand that writes the saw's sharpening. Work
> signs for tools; tools sign for work."

> "Stack sheet, week 3: burned 2, cut 0, hauled 1. The arithmetic of a week the weather took."

> "Ridge stacks: four rows, patterned. A child's height is marked on the nearest stack's end
> board. The mark is older than the child-rearing any record knows about." — texture only

**Held silences (texture, not register rows).**

- What occasion the fifth ring was cut for. The table says `occasion_unassigned`; whether it is
  waiting or remembering is the player's to wonder and the engine's to decline. Texture only.
- Why nobody cuts from the ridge stacks. The stacks are inventory in no table and the plan
  refuses to make them one; the household's restraint is observed and unexplained.

---

## 1.4 Worked examples (non-normative)

**A bell day (fixed seed).**

> Day 214 — the authored cadence rings `muster` at the roster's ordinary muster flow. One line:
> "muster, role: bell-duty, day 214, answered 12." `meal` at midday; `curfew` at dusk (answered
> 12, including the ringer — one column, no marks). `come-in` is rung by the night role and the
> line records the role, never the person.
>
> The fifth ring is available to the same duty verb all day. Rung or not rung, it renders on the
> board with its `occasion_unassigned` mark and no meaning column. The day does not change.

**A wood week (fixed seed).**

> Week 3 — a gather expedition yields 3 cords (site `ridge_4`); the wood ledger records `cut: 3,
> hauled: 1` (weather delayed the haul). Stove and boiler consumption through their owners
> records `burned: 2`. The stack view renders: eleven cords, nine weeks at the current burn. The
> optional Wood Day custom performs existing gather verbs only and begins, if enabled, with the
> muster ring.

---

## 2. Evidence table (checked against the corpus 2026-09-29; re-verify at P0 — Rule 7)

| # | Claim | Evidence | Status |
|---|---|---|---|
| E1 | Muster/roster flows exist (`item_muster_board` item precedent; duty-roster posts; Rounds/Mending-Day custom precedents). | Exp. 23 item list; batch-3/4 custom hooks; `duty_roster_*.json` | LIVE (per corpus) |
| E2 | Exp. 23 *The Alarm* owns fire, emergency response, rescue, cascade, the alarm bell. | `docs/expansions/wave3/expansion_23_the_alarm_plan.md` | LIVE (design bible) |
| E3 | Existing audio surfaces (`MachineTellAudioSync`); no new audio pipeline permitted (MW non-goal). | `.ai/plans/works-below-and-machine-in-the-walls-2026-09-29.md` §1.2 | PROPOSED (plan is DRAFT) |
| E4 | Fuel is existing stock; gather/expedition yields exist; fuel tallies appear in siege/convoy ledgers. | `.ai/plans/ration-wars-2026-09-29.md` (Book columns), convoy/LS plans §1c | LIVE (per corpus) |
| E5 | Kitchen stove and boiler consumption run through existing owners (Common Table domain; `ShelterThermalSystem`). | Exp. 26 domain owners; `.ai/plans/works-below-and-machine-in-the-walls-2026-09-29.md` §1.1 | LIVE |
| E6 | Signal Chain fire pits consume fuel through inventory (batch-3 SC-P2); wood is their supply line. | `.ai/plans/the-signal-chain-and-the-listening-hour-2026-09-29.md` §1.1/E6 | PROPOSED (plan is DRAFT) |
| E7 | Custom machinery can host one authored custom (EV seam); Mending Day is the precedent. | `.ai/plans/the-mending-and-the-second-language-2026-09-29.md` MN-P2 | PROPOSED (plan is DRAFT) |
| E8 | Whether a bell-log row can carry role-only attribution (no person id) without schema drift. | log-format precedents (chain/hour logs: no attribution by construction) | **VERIFY (P0)** |
| E9 | Board surface render point for rings + wood line; additive DTO checksum-safety if any state persists. | batch-3/4 render precedents; codec tests | **VERIFY (P0)** |

## 3. Authority table

| Concern | Owner (unchanged) | This plan adds |
|---|---|---|
| Muster, rosters, duties | duty-roster owner | one duty-role gate for `Ring`; no roster semantics change |
| Emergency response | Exp. 23 *The Alarm* | nothing; the day bell never signals emergencies (DEC-BL-04) |
| Audio | existing audio surfaces | nothing; ships dark if no surface fits (DEC-BL-03) |
| Fuel stock, yields | inventory + expedition owners | nothing; the wood ledger is derived |
| Stove/boiler consumption | thermal/kitchen owners | nothing; `burned` is observed |
| Ecology/regrowth | Second Nature | nothing; wood rows are ordinary gather sites |
| Ring/wood presentation | — | `BellRings` (pure closed table) + `WoodLedger` (pure derived) — **DEC-BL-01 / DEC-WD-01** |

## 4. Claimed Paths (proposed; `INT` = integrator-owned)
**Core:** `Assets/Ashfall.Core/Shelter/BellRings.cs` (new, pure), `Household/WoodLedger.cs` (new, pure)
**Data:** `day_bell_rings.json`, `wood_rows.json`, `day_bell_lines.json`, `wood_line_lines.json`
**Host:** roster host session (`INT`), board/briefing surface (`INT`), custom hook (EV seam, `INT`)
**Presentation:** the day's rings on the existing board surface; a "Stack" band beside the existing stock view — no new routed panel
**Tests:** `Ashfall.Core.Tests/Shelter/BellRingsTests.cs`, `Household/WoodLedgerTests.cs`, `Ashfall.Core.Tests/Save/BellWoodSaveTests.cs`

## 5. Packages

### BL-P0 — Premise audit (Auditor; read-only): close E8–E9; confirm the audio boundary with Exp. 23's and MW's owners; confirm role-only log semantics.
### BL-P1 — Ring table + bell log (Core + data): ≤ 6 rows, fifth-ring flag, role-only log, validator row-level. **Accept:** determinism; no rows → identical behaviour; the fifth ring renders with no meaning.
### BL-P2 — Ring verb + board band (host): duty-gated, cadence-driven through existing muster flow. **Accept:** no coercion path (§6.3); no emergency semantics anywhere.
### WD-P1 — Wood ledger + rows (Core + data): derived cut/hauled/burned, authored source rows, stack view. **Accept:** conservation with inventory counts; observation only.
### WD-P2 — Wood Day custom (host, dark): existing gather/haul verbs only; optional muster-ring start. **Accept:** no new verb in the diff.
### X-P1 — Seam hooks (host, dark): one wood string beside a ring; one boolean for the custom's start. **Accept:** no cross-reads.

## 6. Acceptance criteria
1. Core authority + host owner + event path + persistence + observable outcome agree (CLAUDE.md "integrated").
2. Ship-dark parity: no ring rows and no wood rows → roster, inventory, thermal, audio and board outputs identical on a saved corpus.
3. No-coercion invariant: ringing moves nobody; the diff contains no forced-move path of any kind (DEC-BL-02).
4. Conservation: wood ledger figures reconcile exactly with inventory counts and owner consumption (DEC-WD-02).
5. Determinism: identical ring cadences, log contents and stack renders on replay (`CampaignStreamIds` fork; never `System.Random`).
6. Attribution invariant: no bell-log line carries a person id — role only (E8 closed at P0).
7. The fifth ring renders `occasion_unassigned` with no meaning and no tooltip (DEC-BL-05).
8. Exp. 23's emergency semantics, MW's audio boundaries and EV's custom silences stay untouched (§7).

## 7. Cross-plan boundaries
- **The Alarm (Exp. 23):** fire, rescue, evacuation, the alarm bell are theirs. The day bell is the calls when nothing is wrong; the two instruments must never share a ring row.
- **Works Below / Machine in the Walls:** audio surfaces are theirs; the bell ships dark rather than extend any pipeline.
- **Evenings and Memory Work:** custom machinery is theirs; Wood Day is one authored custom, and "who started the first custom" stays in EV's register.
- **The Signal Chain (batch 3):** the chain's fire pits burn this wood; the sibling grammar (pattern table vs ring table) is deliberate and the two tables must never merge.
- **Second Nature:** no ecology modelling; wood rows are ordinary gather sites in existing data.
- **Ration Wars:** the Book's fuel columns are its accounting; the wood ledger is a projection and never a second tally authority.

## 8. Decision register (proposals — unsigned)
| ID | Decision | Type | Proposed |
|---|---|---|---|
| DEC-BL-01 | The ring table is closed (≤ 6 rows) and its rows are *words*, not events. | design | Yes |
| DEC-BL-02 | Rings move nobody by force; coercion would turn the household into a barracks. | tone/rule | Yes |
| DEC-BL-03 | No new audio pipeline; if no existing surface fits, the bell ships dark and renders on the board only. | architecture | Yes |
| DEC-BL-04 | The day bell never signals emergencies; Exp. 23's instrument is a different bell. | boundary | Yes |
| DEC-BL-05 | The fifth ring is `occasion_unassigned` forever; no tooltip, no meaning, no event. | tone | Yes — **needs canon note** |
| DEC-WD-01 | The wood ledger is derived; the plan adds no resource and cuts nothing. | rule | Yes |
| DEC-WD-02 | Ledger figures reconcile exactly with inventory; week-granularity is the presentation unit. | architecture | Yes |
| DEC-WD-03 | The ridge stacks are inventory in no table; nobody cuts from them and the plan refuses to say why. | tone | Yes — **needs canon note** |
| DEC-X-01 | Both seam hooks ship dark; one string and one boolean. | architecture | Yes |

## 9. Pre-flight Checks
- [ ] Go config/save validator passes (`bin/validate-config` / `bin/ashfall-dev validate-config`)
- [ ] No equivalent existing system found (search `Bell`, `Ring`, `WoodLedger`, `FuelLedger`, `Cord`)
- [ ] Premise re-verified (Rule 7); Exp. 23, MW and EV evidence re-read; their registered silences listed in the handoff as untouchable
- [ ] Signed decisions in hand (at minimum DEC-BL-01, DEC-BL-04, DEC-WD-01)

## 10. Verification
- [ ] `bin/run-scoped-tests` on new tests + existing roster, inventory and thermal tests (list from P0 selector)
- [ ] Roster host selftest with ring rows on and off (VERIFY args)
- [ ] Scoped only (<30 s each); ≤10–15 test-edit steps per failure, then auto-flag in `.ai/state.md`; **no full suite** without `RUN FULL TESTS`
- [ ] Handoff in `AI_AGENT_WORKFLOW.md` format; `.ai/state.md` updated

## 11. Stop conditions (Rule 10)
Stop and report if: a ring row would require a forced-move or coercion path; the fifth ring cannot render without meaning by construction; the wood ledger cannot reconcile with inventory counts without a second tally authority; the bell would touch Exp. 23's emergency semantics; any path overlaps a live claim.

## 12. Open Mysteries & Deliberate Silence (lore register — no authority, no claimed path)

These questions are **intentionally unanswered**. They are not bugs, not TODOs, and not hidden
work items. They keep the day taller than its timetable and the winter older than its wood. Any
future plan that answers one must name the decision that permits it.

| # | Question | Why it stays open | Who may answer it (later, signed) |
|---|---|---|---|
| BL-OM-1 | What occasion is the fifth ring for? | `occasion_unassigned` forever (DEC-BL-05); whether it waits or remembers is not modelled. | Never — deliberately sealed. |
| BL-OM-2 | Who cast the bell? | Its age, casting and first ringer are unauthored; the bell hangs where the muster board hangs and says nothing. | Canon owner only, as a signed decision. |
| BL-OM-3 | Why is the rope's knot the same knot twice? | Texture (§1c); knots are how the shelter signs things and the signing is not modelled. | Never — texture by omission. |
| BL-OM-4 | What does the night shift hear in the fourth ring that the day shift does not? | The ring means `come-in`; the night's relationship to it is the queued Night Shift plan's subject (batch-4, plan 3) and is not asserted here. | The queued NS plan's owner. |
| BL-OM-5 | Do other shelters ring the same four? | The table is the household's tongue; regional comparison is unmodeled and must not be added. | The Living Region's owner, if ever authored. |
| WD-OM-1 | Who stacked the ridge cordwood? | The pattern matches no living practice (§12); dating it would turn stacks into history with owners. | Never — deliberately sealed. |
| WD-OM-2 | Why does nobody cut from the ridge stacks? | The restraint is observed and unexplained (§1c); the stacks are in no table and must not become one. | Never — a rule, not a gap. |
| WD-OM-3 | What is the patterned stacking method counting? | Four rows, one pattern, no name (§12); the pattern may be measurement, memory or manners and the plan refuses to choose. | Never — the pair stays unresolved with BL-OM-1 (mystery-index §3 discipline). |
| WD-OM-4 | What was the year the weather took? | The week-3 sheet's phrasing is texture; no weather model supports the story and none may be bent to it. | Never — tone-locked. |
| WD-OM-5 | Why is the week the unit of warmth? | DEC-WD-02 is a presentation decision; why the household thinks in weeks is not modelled. | Canon owner only, if ever asserted. |
