# Feature / Task Plan: The Season Words (the household's names for the year's turns) & The Goodbye at the Gate (leavetaking customs)

<!-- UPON INTEGRATION (MANDATORY):
Prepend header at the top saying:
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**
And immediately move this file to .ai/plans/integrated/<category>/
-->

STATUS: DRAFT — awaiting user approval (not self-approved; CLAUDE.md Rule 8 requires `STATUS: APPROVED BY USER` before any code commit). Story batch 7, plans 4 of 4 (subjects 71–72).

> **Subjects covered (2 of the 8 in the "outward spring" batch):**
> 71. **The Season Words** — the household's names for the year's turns: the mud weeks, the
> green fire, the long light. (Prefix `SE`.)
> 72. **The Goodbye at the Gate** — leavetaking customs: who is seen off, in what form, and the
> quiet leavetaker the book records as `unsaid`. (Prefix `GT`.)
>
> **Prose / design companions that already exist and are extended, not replaced:**
> `.ai/plans/story-expansion-batch-4/the-loan-shelf-and-the-wind-names-2026-09-29.md` (Wind-Names
> owns *weather* words; this plan owns *season* words — the carve-outs are siblings),
> `.ai/plans/the-mending-and-the-second-language-2026-09-29.md` (SL owns *words*; its register
> governs the vocabulary boundaries), `.ai/plans/the-fair-and-the-guest-book-2026-09-29.md`
> (Guest Book departures), `.ai/plans/year-two-the-long-thaw-2026-09-29.md` (the campaign
> calendar and climate values are theirs — read-only), expeditions (departures are theirs).
>
> **Not a claim.** This plan claims no path in `WORKTREE_OWNERSHIP.md`, edits no source, no data,
> no ledger and no other plan. It is documentation only. Paths below are *proposed*; `INT` marks
> integrator-owned shared seams.

> **Editorial note (prose texture).** Sections **0**, **1b**, **1c** and **12** carry story texture
> and voice. Sample lines are content candidates for `*.json` rows; they belong in data, never in
> code. No authority, path, decision, acceptance criterion or verification step is changed by any
> prose section.

---

## 0. Prologue — The Year's Names and the House's Last Sentence

> *"The calendar says what day it is. The season words say what the day is like. Only one of
> them is signed."*

The season words are the household's own calendar: *the mud weeks*, *the green fire*, *the long
light*, *the first quiet* — names for the year's turns that no almanac carries and every person
in the shelter uses. They are not weather words (the wind-names own those) and not coined words
(the Second Language owns those); they are *turns* — spans of days with a feel to them, named
after what the shelter notices. The formal calendar keeps its dates; the season words keep its
moods, and the two run side by side like the official map and the wall map.

The goodbye at the gate is the house's last sentence of the day. Departures end at the gate — not
inside, not at the door — and there is a form to it: who is seen off (as roles), how (spoken,
waved, watched until out of sight), and what is said (which the record keeps as a form, not a
text). One leavetaker every season leaves *before the gate*. The book records `unsaid` in the
same plain type as every other row.

**Tone & register.** Almanac-plain and threshold-quiet. The season words' vocabulary is *turn,
named, felt, fading*; the gate's is *seen off, form, unsaid, watched, gone*. Prose for the season
words should read like an almanac written from inside a house; prose for the goodbyes like the
last page of a day, written in the same hand as the first.

**Mystery & texture.** Two silences hold the pair. One season word names a **turn the year has
not made in the book's memory** — kept in the vocabulary, dated, and never in season (§12). And
the quiet leavetaker's `unsaid` rows are **always the same season** (§12). Neither is a puzzle.
Both are what happens when a household's calendars — one of weather, one of manners — keep time
beyond their occasions.

**The second layer.** The season words and the goodbyes are the two ways the house measures what
passes: one names the year's turns, one closes the day's departures, and both are *manners
extended to time itself*. The plan's quiet thesis is that a shelter becomes a culture when it
starts naming its spans and honouring its exits — the mud weeks and the gate are the two places
where the household says, out loud, that it noticed.

---

## 1. Goal & Outcome

> *Design intent: the player should hear "the mud weeks are here" and feel the year turn in the
> house's own words — and watch a leavetaking at the gate and see the form of it kept.*

### 1.1 The Season Words (SE)

- **Goal:** A **Season Rows** table (≤ 12 rows: word, the turn it names as a day-range anchored
  read-only to the existing campaign calendar, `word_fading` flag, `unmade_turn` flag for the
  anomaly) and a derived **Year Read** (which turn the shelter is in *by its own words*, this
  year's names beside last year's, corrections this season); one constrained verb — **Call It**
  (an authored alternate turn-name may be added; free text forbidden — inherits DEC-WM-02). No
  calendar or climate state is touched (DEC-SE-01).
- **Outcome (observable):** on a fixed seed, the year read renders the current turn's household
  word beside the calendar's date; `word_fading` words render their flag; the `unmade_turn` row
  renders flatly with its dated emptiness; `Call It` adds an alternate and removes nothing; with
  no rows every surface behaves identically to today; save/load round-trips.
- **Non-Goals:** no weather or climate mechanics (weather owners and Year Two climate keep their
  meanings); no word generation; no reconciliation with the Wind-Names or the Second Language
  (three vocabularies, three ledgers — DEC-SE-02); no new save section; no new routed panel; no
  Unity.
- **"Done":** §6 acceptance passes via `bin/run-scoped-tests`; ship-dark parity holds; handoff
  lists untouched shared paths.

### 1.2 The Goodbye at the Gate (GT)

- **Goal:** A **Gate Rows** table (≤ 16 rows: date, leaver-role (never names — DEC-GT-02), form
  from a closed list (`spoken`, `waved`, `watched_until_out_of_sight`, `hands_shaken`,
  `unsaid`), `at_gate` invariant) and a derived **Gate Read** (the season's leavetakings in
  plain type, the `unsaid` rows flatly among the rest); one verb — **See Off** (one history fact;
  the departure itself belongs to expeditions and the Guest Book). Goodbyes are said at the gate
  and nowhere else (DEC-GT-03).
- **Outcome (observable):** on a fixed seed, a departure writes one gate row with its form;
  `watched_until_out_of_sight` renders its full form in plain type; `unsaid` rows render flatly
  and are always in the same season in the data; with no rows every owner behaves identically to
  today; save/load round-trips.
- **Non-Goals:** no departure mechanics (expeditions and the Guest Book keep their meanings); no
  morale, bond or relationship effects of any kind (DEC-GT-01); no dialogue or text of farewells;
  no resolution of the quiet leavetaker; no new save section; no new routed panel; no Unity.
- **"Done":** §6 acceptance passes; ship-dark parity holds; handoff lists untouched shared paths.

### 1.3 The seam between them (X)

- **Goal:** One documented boundary and one optional hook: a gate row may render beside a season
  word ("seen off in the long light") as one read-only string; and the year read may cite the
  gate's season tally. One string each way, shipped dark.

---

## 1b. Texture, Mystery & Voice

**Three vocabularies, three ledgers.**

Words (the Second Language), weather words (the Wind-Names), season words (this plan) — the
corpus now keeps three separate registers of the house's speech and *none of them merges*. Prose
must respect the boundaries in its bones: a season word is a *turn*, not a word and not a wind.

**The form of a goodbye is the whole content.**

`watched_until_out_of_sight` is the longest entry in any closed list in the corpus, and that is
the point: the *form* is where the feeling lives. Prose must never write the farewells
themselves — the record keeps forms, not texts (DEC-GT-04).

**What the player is never told.**

- What turn the `unmade_turn` word names. Kept, dated, never in season (§12); the year has not
  made it in the book's memory and the book keeps the word anyway.
- Why the quiet leavetaker leaves before the gate. `unsaid` is recorded flatly and always in the
  same season (§12); whether it is one habit or one person is not modelled.
- What was said at any gate. Forms, not texts (DEC-GT-04); the corpus's no-contents discipline
  holds at the threshold.
- Why goodbyes are said at the gate and not inside. The invariant is kept and not explained
  (§12); the house's reason is in no table.

**Voice — sample fragments (content candidates for `season_words.json` / `gate_lines.json`).**

> "The mud weeks are here. The calendar says day 98. The house says 'the mud weeks,' which is a
> different and more useful fact." — year read (SE)

> "Green fire: the two weeks the new growth is brightest. Named for the light, not the plants.
> The house is precise about its own metaphors." — year read (SE)

> "The unmade turn's word is in the vocabulary with its dates. The dates have come and gone three
> years running without the turn. The word is kept." — year read (SE)

> "Seen off at the gate: watched until out of sight. The form is the longest in the list because
> looking is the longest part." — gate read (GT)

> "Unsaid, autumn again. Same season, every year. The book records the row and the row records
> nothing else." — gate read (GT)

**Design texture beats.**

- **No effects of any kind** (DEC-SE-01, DEC-GT-01): the season words move no climate and the
  goodbyes move no morale. Manners and names only.
- **The flat-render family's final members**: `unmade_turn`, `word_fading`, `unsaid` — plain
  type, unexplained by the format, siblings to the whole corpus's flag discipline.
- **Forms, not texts** (DEC-GT-04): the envelope discipline reached the threshold; a goodbye is
  an address with a gesture.
- **The at-gate invariant** (DEC-GT-03): the one place in the corpus where *location* is a rule.

---

## 1c. The Deeper Layer — objects & held silences (second prose pass)

*(Second prose pass, non-contractual: narrative texture and writing guidance only — no authority,
no claimed path, no acceptance criterion, no verification step. §12's register is unchanged; the
fragments below are content candidates and deliberate silences, not new recorded questions. Where
a named data file holds no prose field, fragments are texture only and gain no schema.)*

**What the year leaves lying around.**

> "Almanac margin: 'the mud weeks — what my mother called the ugly month.' The margin is where
> the vocabulary argues with itself and the book keeps every version." — texture only

> "A fading word, used once this season and correctly. The word is going away being right." —
> texture only

> "The unmade turn's dates, marked in the calendar in pencil. Somebody checks the calendar at the
> turn's edge, every year, in pencil." — texture only

**What the gate leaves lying around.**

> "Gate post, worn smooth at hand height on the leaving side. The wear is the house's record of
> its own departures and the fence cannot see it from here." — texture only

> "See-off record: 'waved.' Four letters. The form list's shortest entry is chosen by the people
> with the least to say and the most to watch." — texture only

> "One autumn's `unsaid` rows in a cluster. The cluster is visible in the data and invisible in
> the prose, which is where clusters belong." — texture only

**Held silences (texture, not register rows).**

- Whether the unmade turn is coming or gone. The word is kept with its dates (§12); the pencil
  marks at the turn's edge are a habit and not a forecast. Texture only.
- Why the leaving side of the gate post is worn and not the other. Texture (§1c); the house's
  traffic is its own and the record keeps the wear, not the reasons.

---

## 1.4 Worked examples (non-normative)

**A year in the house's words (fixed seed).**

> The year read renders: "the first quiet (day 12–40)", "the mud weeks (98–126, in season now)",
> "the long light (180–240, `word_fading`)", and the unmade turn — flat, dated, empty: kept in
> the vocabulary with its three-year-old dates. A `Call It` adds an alternate for "the green
> fire"; the old word stays printed beside it. No calendar or climate state changes anywhere in
> the diff.

**A season of goodbyes (fixed seed).**

> Four departures: `waved` (departure to the wet road), `spoken` (a call's return), `watched_
> until_out_of_sight` (the long freight run), and `unsaid` — autumn again, flatly rendered. Each
> row is a form; the record keeps no text and the diff contains no dialogue field of any kind. A
> gate row renders beside its season word: "watched until out of sight — in the long light."

---

## 2. Evidence table (checked against the corpus 2026-09-29; re-verify at P0 — Rule 7)

| # | Claim | Evidence | Status |
|---|---|---|---|
| E1 | The Wind-Names own weather words under DEC-WN-02's carve-out; the Second Language owns words under DEC-SL-04's refusal. | batch-4 WN plan; `.ai/plans/the-mending-and-the-second-language-2026-09-29.md` | PROPOSED (plans are DRAFT) |
| E2 | Year Two's climate values and campaign calendar are authored, read-only here (F4/F1 umbrella evidence). | `.ai/plans/year-two-the-long-thaw-2026-09-29.md` §2 | LIVE (umbrella evidence) |
| E3 | Guest Book departures and expeditions own leaving mechanics; role-only rows precedented. | batch-3 GB plan; expeditions owner | PROPOSED / LIVE |
| E4 | The envelope/no-contents discipline (DEC-LT-01) provides the forms-not-texts precedent. | batch-4 LT plan | PROPOSED (plan is DRAFT) |
| E5 | `Heard`-style grading and alternate-name verbs (`Correct`, `Point`, `Name`, `Call It`) precedented. | batch-4/6/7 plans | PROPOSED (plans are DRAFT) |
| E6 | Custom machinery hosts authored customs (EV seam; gate custom precedent). | batch-3–7 custom hooks | PROPOSED (soft dependency) |
| E7 | Whether a day-range anchored read-only to the campaign calendar survives checksum as an additive row. | codec / snapshot tests | **VERIFY (P0)** |
| E8 | Board render points for year and gate reads. | batch-3–7 render precedents | **VERIFY (P0)** |

## 3. Authority table

| Concern | Owner (unchanged) | This plan adds |
|---|---|---|
| Calendar, climate | Year Two / weather owners | nothing; season words read dates only |
| Weather words | Wind-Names (batch 4) | nothing; the carve-outs are siblings and never merge |
| Words | Second Language (batch 3) | nothing; season words are turns, not words (DEC-SE-02) |
| Departures | expeditions / Guest Book | one history fact per leavetaking; no departure mechanics |
| Morale, bonds | relationship owners | nothing; goodbyes move no number |
| Season/gate folk state | — | `SeasonRows` + `GateRows` (pure Core; row facts only) — **DEC-SE-03 / DEC-GT-05** |

## 4. Claimed Paths (proposed; `INT` = integrator-owned)
**Core:** `Assets/Ashfall.Core/Folklore/SeasonRows.cs` (new, pure), `Social/GateRows.cs` (new, pure)
**Data:** `season_words.json`, `gate_rows.json`, `season_words_lines.json`, `gate_lines.json`
**Host:** calendar read (`INT`), departure hook (`INT`, read-only), board surface (`INT`)
**Presentation:** a "Turn" line beside the calendar surface; a "Gate" band on the existing departure surface — no new routed panel
**Tests:** `Ashfall.Core.Tests/Folklore/SeasonRowsTests.cs`, `Social/GateRowsTests.cs`, `Ashfall.Core.Tests/Save/SeasonGateSaveTests.cs`

## 5. Packages

### SE-P0 — Premise audit (Auditor; read-only): close E7–E8; confirm the calendar read is strictly read-only; confirm the three-vocabulary boundary wording with the SL owner.
### SE-P1 — Season rows + year read (Core + data): turns, `word_fading`, `unmade_turn`, `Call It`, validator row-level. **Accept:** no calendar/climate field exists anywhere (§6.3); determinism; round-trip.
### SE-P2 — Turn line + alternates (host): adds alternates only. **Accept:** no weather-word or coined-word table is touched.
### GT-P1 — Gate rows + read (Core + data): closed form list, `unsaid`, `at_gate` invariant, validator row-level. **Accept:** no effect field exists anywhere (§6.4); no text/dialogue field exists (§6.5).
### GT-P2 — See Off verb + gate band (host): one history fact per departure. **Accept:** departures remain the owners'; forms render flatly.
### X-P1 — Seam hooks (host, dark): one string each way. **Accept:** no cross-reads.

## 6. Acceptance criteria
1. Core authority + host owner + event path + persistence + observable outcome agree (CLAUDE.md "integrated").
2. Ship-dark parity: no season rows and no gate rows → calendar, climate, weather words, departures and board outputs identical on a saved corpus.
3. No-climate invariant: the diff contains no weather, climate or calendar field of any kind (DEC-SE-01).
4. No-effect invariant: goodbyes move no morale, bond or relationship number (DEC-GT-01).
5. Forms-not-texts invariant: the diff contains no farewell text, dialogue or message field (DEC-GT-04).
6. Flat-render invariant: `unmade_turn`, `word_fading` and `unsaid` render in the same plain type as every other row (DEC-SE-04, DEC-GT-06).
7. Determinism: identical year reads, gate rows and renders on replay (`CampaignStreamIds` fork; never `System.Random`).
8. The three-vocabulary boundary and the Guest Book's departure discipline stay unanswered (§7).

## 7. Cross-plan boundaries
- **The Wind-Names / The Second Language (batches 3–4):** three vocabularies, three ledgers (DEC-SE-02). Weather words are winds; coined words are words; season words are turns. None merges, and the SL register governs the boundary.
- **Year Two / weather owners:** dates and climate are theirs; the season words read dates and name moods.
- **The Guest Book / expeditions:** departures are theirs; the gate records the *form* of the leaving and never the leaving.
- **The Wall Map / The Names of the Rooms:** folk naming rhymes across all three (rooms inward, shelters at distance, seasons in time) and the tables never merge.
- **EV:** gate customs are its register's; who started saying goodbye at the gate stays unanswered here.

## 8. Decision register (proposals — unsigned)
| ID | Decision | Type | Proposed |
|---|---|---|---|
| DEC-SE-01 | Season words touch no calendar, climate or weather state. | rule | Yes |
| DEC-SE-02 | Three vocabularies, three ledgers: turns ≠ weather words ≠ coined words. | boundary | Yes — **needs canon note** |
| DEC-SE-03 | Season rows are folk facts: words, day-ranges, flags. | architecture | Yes; confirm in P0 |
| DEC-SE-04 | `unmade_turn` and `word_fading` render flatly; the word is kept regardless. | tone | Yes |
| DEC-GT-01 | Goodbyes move no morale, bond or relationship number. | rule | Yes |
| DEC-GT-02 | Leavers are roles, never names. | tone | Yes |
| DEC-GT-03 | Goodbyes are said at the gate and nowhere else. | tone/rule | Yes — **needs canon note** |
| DEC-GT-04 | Forms, not texts: no farewell text exists in any schema. | tone/rule | Yes |
| DEC-GT-05 | Gate rows are history facts in an additive table; no new save section. | architecture | Yes; confirm in P0 |
| DEC-GT-06 | `unsaid` renders flatly and clusters silently in the data. | tone | Yes — **needs canon note** |

## 9. Pre-flight Checks
- [ ] Go config/save validator passes (`bin/validate-config` / `bin/ashfall-dev validate-config`)
- [ ] No equivalent existing system found (search `SeasonWord`, `TurnName`, `GateFarewell`, `SeeOff`, `Leavetaking`)
- [ ] Premise re-verified (Rule 7); the SL/WN registers and the Guest Book's departure discipline re-read; their silences listed in the handoff as untouchable
- [ ] Signed decisions in hand (at minimum DEC-SE-02, DEC-GT-03, DEC-GT-04)

## 10. Verification
- [ ] `bin/run-scoped-tests` on new tests + existing calendar, departure and custom tests (list from P0 selector)
- [ ] Departure hook selftest with gate rows on and off asserting zero departure-mechanic changes (VERIFY args)
- [ ] Scoped only (<30 s each); ≤10–15 test-edit steps per failure, then auto-flag in `.ai/state.md`; **no full suite** without `RUN FULL TESTS`
- [ ] Handoff in `AI_AGENT_WORKFLOW.md` format; `.ai/state.md` updated

## 11. Stop conditions (Rule 10)
Stop and report if: a season word would need climate or weather state; a goodbye would need an effect or a text; the at-gate invariant could not hold by construction; a vocabulary table outside this plan would be touched; any path overlaps a live claim.

## 12. Open Mysteries & Deliberate Silence (lore register — no authority, no claimed path)

These questions are **intentionally unanswered**. They are not bugs, not TODOs, and not hidden
work items. They keep the year taller than its calendar and the gate older than its reasons. Any
future plan that answers one must name the decision that permits it.

| # | Question | Why it stays open | Who may answer it (later, signed) |
|---|---|---|---|
| SE-OM-1 | What turn does the unmade word name? | Kept, dated, never in season (§12); the year has not made it in the book's memory. | Never — deliberately sealed. |
| SE-OM-2 | Is the unmade turn coming or gone? | The pencil marks at its edge are a habit, not a forecast (§1c); the plan refuses the reading. | Never — a rule, not a gap. |
| SE-OM-3 | Whose mother called it the ugly month? | The margin argues with the vocabulary (§1c); the book keeps every version and credits none. | Never — texture by omission. |
| SE-OM-4 | Why do some words fade and not others? | `word_fading` is observed; the sociology of vocabulary is unmodelled and inherits SL's register. | The SL plan's owner (its register governs). |
| GT-OM-1 | Who is the quiet leavetaker? | `unsaid` is flat and role-less (DEC-GT-02); one habit or one person is not modelled. | Never — deliberately sealed. |
| GT-OM-2 | Why is `unsaid` always the same season? | The cluster is visible in the data and invisible in the prose (§1c); the plan keeps it that way. | Never — tone-locked by DEC-GT-06. |
| GT-OM-3 | Why the gate and not the door? | The invariant is kept and unexplained (§12); the house's reason is in no table and inherits EV's custom-provenance silence. | Never — the pair stays unresolved (mystery-index §3 discipline). |
| GT-OM-4 | What was said at the gate? | Forms, not texts (DEC-GT-04); the corpus's no-contents discipline reaches the threshold and stops there. | Never — a rule, not a gap. |
| GT-OM-5 | Why is only the leaving side of the post worn? | Texture (§1c); the house's traffic is its own and the record keeps the wear, not the reasons. | Never — texture by omission. |
