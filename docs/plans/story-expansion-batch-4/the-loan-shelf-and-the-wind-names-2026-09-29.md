# Feature / Task Plan: The Loan Shelf (household circulation over the existing inventory seam) & The Wind-Names (the regional weather vocabulary)

<!-- UPON INTEGRATION (MANDATORY):
Prepend header at the top saying:
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**
And immediately move this file to .ai/plans/integrated/<category>/
-->

STATUS: DRAFT — awaiting user approval (not self-approved; CLAUDE.md Rule 8 requires `STATUS: APPROVED BY USER` before any code commit). Story batch 4, plans 1 of 4 (subjects 41–42).

> **Subjects covered (2 of the 8 in the "rhythm and circulation" batch):**
> 41. **The Loan Shelf** — the shelf by the door where the household's spare things circulate:
> taken without asking, returned when done, mended if broken. (Prefix `LN`.)
> 42. **The Wind-Names** — the region's shared words for weather: a mapping file for the four
> unmapped vocabularies Living Region E5 found, weather words only. (Prefix `WN`.)
>
> **Prose / design companions that already exist and are extended, not replaced:**
> `.ai/plans/the-fair-and-the-guest-book-2026-09-29.md` (the Guest Book's borrowed/returned
> columns — guest scale), `.ai/plans/the-mending-and-the-second-language-2026-09-29.md` (DEC-SL-04
> refuses vocabulary reconciliation; this plan is the weather-slice exception), the inventory owner
> and duty-roster seam, `docs/plans/PLAN_22_CONSUMABLE_BILLS_INTEGRATION_PLAN.md` (item tags).
>
> **Not a claim.** This plan claims no path in `WORKTREE_OWNERSHIP.md`, edits no source, no data,
> no ledger and no other plan. It is documentation only. Paths below are *proposed*; `INT` marks
> integrator-owned shared seams.

> **Editorial note (prose texture).** Sections **0**, **1b**, **1c** and **12** carry story texture
> and voice. Sample lines are content candidates for `*.json` rows; they belong in data, never in
> code. No authority, path, decision, acceptance criterion or verification step is changed by any
> prose section.

---

## 0. Prologue — The Shelf and the Sky

> *"A loan shelf is a promise that nobody here has to own everything. Wind-names are a promise
> that nobody here has to face the weather alone."*

The shelf is the least dramatic institution in the shelter and the first one every visitor
understands: a board by the door, four hooks, a lamp that is nobody's lamp. Tools, a coat, a
book, a length of cord. Take it. Bring it back. Mend it if you break it. No sign-out, no
quartermaster, no penalty — the shelf runs on the only currency a household of twelve has enough
of, which is *being seen*.

The wind-names are the shelf's analogue for weather. Every settlement in the region has words for
winds that the formal weather tables do not name: the ash-drift that means rain by Thursday, the
cold that comes up the river, the dry wind that makes the roof complain. The Living Region's
survey found four such vocabularies and mapped none of them. This plan writes the one mapping
file the survey invited — weather words only — and adds and deletes nothing else.

**Tone & register.** Domestic and meteorological, both plain. The shelf's vocabulary is *taken,
back, mended, on the hook*; the wind's is *word, weather, settlement, disputed*. Prose for the
shelf should read like a household's shared memory of its own objects; prose for the wind-names
like a farmer's almanac margin — short entries, weather-tested, occasionally arguing.

**Mystery & texture.** Two silences hold the pair. One object has been *on loan* since before the
shelf's earliest recorded inventory — checked out to nobody, present in every stocktake, never on
its hook (§12). And one wind-name is claimed by two settlements with *opposite* meanings — the
same word, two weathers, and the mapping file keeps both readings (§12). Neither is a puzzle to
solve. Both are what happens when a household and a region run on trust instead of records.

**The second layer.** Lending and naming are the two oldest technologies of living together. A
loan shelf says *yours is mine when I need it*; a wind-name says *what happens to you happens to
me too, and now we can say so*. Both are anti-inventory: neither stores anything, both circulate
everything. The plan's quiet thesis is that a shelter is not the sum of what it owns but the
speed at which what it owns gets used — and the shelf's ledger is a single hook that is either
empty or full.

---

## 1. Goal & Outcome

> *Design intent: the player should glance at the shelf by the door and know, in one look, what
> the household is currently short of — without anyone having been assigned anything.*

### 1.1 The Loan Shelf (LN)

- **Goal:** A **Shelf Table** of loanable household objects (≤ 20 rows: item id, home hook,
  `always_out` flag for the pre-inventory object), a minimal **Loan Ledger** (object id, out-day,
  `return_by_convention` — an expected day with **no enforcement** (DEC-LN-02)), and two verbs
  routed through ordinary inventory transactions: **Take** and **Return** (plus the mending
  convention: a returned object may carry the Mending plan's opaque mark, read-only). Shelf state
  is derived: which hooks are full, which are empty, how long each object has been out.
- **Outcome (observable):** on a fixed seed, taking `item_lamp_02` issues it through the
  inventory owner and renders its hook empty with an out-day; returning it fills the hook and
  closes the loan line; an object out past its convention day renders plainly ("out 12 days") with
  no penalty and no enforcement of any kind; the `always_out` object renders its empty hook with
  its loan line open since its recorded day; with no shelf rows every owner behaves identically to
  today; save/load mid-loan round-trips.
- **Non-Goals:** no ownership transfer; no lending economy, rent or credit; no enforcement,
  reminders or quests; no item catalog additions beyond the shelf rows (Plan 22 owns tags);
  no new save section; no new routed panel; no Unity.
- **"Done":** §6 acceptance passes via `bin/run-scoped-tests`; ship-dark parity holds; handoff
  lists untouched shared paths.

### 1.2 The Wind-Names (WN)

- **Goal:** A **Wind-Names Table** (≤ 24 rows: word, part of speech, settlement of origin,
  mapped weather-state id, `use_disputed` flag, optional `mapped_by` provenance tag) written as
  the *one mapping file* Living Region E5 invites; derived glossary rendering (word, weather it
  names, who says it, disputed mark); and one optional read hook: a board line or a weather
  briefing may *cite* a wind-name beside its formal state. Nothing is deleted, renamed or
  reconciled (DEC-WN-01).
- **Outcome (observable):** on a fixed seed, the glossary renders every authored word with its
  settlement and mapped weather state; `use_disputed` rows render both settlements' readings
  side by side; a cited board line shows "the [word] — state" and writes nothing anywhere; with
  no rows every surface behaves identically to today; save/load preserves glossary state.
- **Non-Goals:** no weather mechanics (weather owners unchanged); no language generation; no
  reconciliation with the Second Language or the region's non-weather vocabularies (DEC-SL-04
  discipline); no localization changes; no new save section; no new routed panel; no Unity.
- **"Done":** §6 acceptance passes; ship-dark parity holds; handoff lists untouched shared paths.

### 1.3 The seam between them (X)

- **Goal:** One documented boundary and one optional hook: a loan out in bad weather may render
  one wind-name beside its out-day ("out 12 days, through the [word]"); and the shelf's
  `always_out` object may be cited by a wind-name row as `mapped_by` folklore provenance — a
  read of one string field, shipped dark. Neither half reads the other's state.

---

## 1b. Texture, Mystery & Voice

**A hook is a boolean.**

The shelf's whole interface is the empty hook, and the empty hook is the plan's best sentence: it
says *someone has it* without saying who, how long, or why. Write the shelf state as inventory's
most human projection — not who holds what, but what the household is *missing*.

**Disputed words are the mapping file's conscience.**

A `use_disputed` row keeps both readings and adopts neither. The prose must never average them:
one settlement's rain-wind is another's dry-wind, and the file's fairness is that both get
printed in the same plain type.

**What the player is never told.**

- Who took the `always_out` object. The loan line is open since its recorded day and no name is
  in the format (DEC-LN-03). The empty hook is the only biography the object gets.
- Why the shelf has no sign-out. Whether this is discipline or trust is not modelled; the shelf's
  rule is three clauses and no author.
- Which settlement is *right* about the disputed wind. Both readings are mapped to real weather
  states; which state the word "really" names is refused by construction.
- What the four vocabularies were before they were weather words. E5 mapped nothing and this plan
  maps weather only; the rest of the words are out of scope and out of reach.

**Voice — sample fragments (content candidates for `loan_shelf_lines.json` / `wind_name_words.json`).**

> "Shelf, evening: coat out, lamp out, cord back. The shelf does not say who. The shelf says
> what is missing, which is the household's own name for who." — shelf board (LN)

> "Rule of the shelf, three clauses, no author: take it, bring it back, mend it if you break
> it." — shelf board (LN)

> "Out twelve days. Out through the ash-drift and the cold-that-climbs. Nobody is in trouble.
> The convention is a hope with a date on it." — shelf board (LN)

> "*River-cold*, n. (Fen Crossing) The cold that comes up the water. Names state `cold_advection`.
> Two settlements say it; one means it kindly." — glossary (WN)

> "*Ash-drift*, n. (Iron Basin) Fine ash moving low and slow; rain within three days. Mapped,
> dated, argued about at every Fair." — glossary (WN)

**Design texture beats.**

- **No enforcement, ever** (DEC-LN-02). The moment the shelf can punish, it becomes the
  quartermaster's shelf and the fiction dies.
- **The empty hook is the UI.** Resist panels; the shelf's state should fit in a glance.
- **Disputed rows render both readings.** Averaging two settlements' weather words is the one
  unforgivable editorial act in this plan.
- **The mapping file adds and deletes nothing** (DEC-WN-01). It is a bridge document; bridge
  documents are never allowed to be thorough.

---

## 1c. The Deeper Layer — objects & held silences (second prose pass)

*(Second prose pass, non-contractual: narrative texture and writing guidance only — no authority,
no claimed path, no acceptance criterion, no verification step. §12's register is unchanged; the
fragments below are content candidates and deliberate silences, not new recorded questions. Where
a named data file holds no prose field, fragments are texture only and gain no schema.)*

**What the shelf leaves lying around.**

> "Hook 3 has been empty since before the first stocktake. The stocktakes are honest about it:
> 'hook 3, out.' Every stocktake, the same two words."

> "The cord came back mended. The mark under the mend is a tick and a date. The shelf does not
> record marks; the shelf only has hooks."

> "Rule card, three clauses, no author. Somebody added a fourth in pencil — 'and bring it back
> dry' — and somebody else has been keeping it."

**What the sky leaves lying around.**

> "Mapping row, disputed: *dry-that-isn't*. Iron Basin says the rain follows it. Fen Crossing says
> the rain preceded it. Both are printed."

> "Old glossary margin: three words crossed out, none struck. The crossings-out are weather words
> from a settlement the survey did not reach."

> "Briefing line, cited: 'the ash-drift — state: ash_low.' The citation is a courtesy the
> weather tables cannot do alone."

**Held silences (texture, not register rows).**

- Who has the hook-3 object. Twelve people and none of them are modelled holding it (§12); the
  format keeps no names and must not grow any. Texture only.
- What the three crossed-out margin words named. A settlement the survey did not reach is not in
  the data; the words survive as crossings-out and must stay that way.

---

## 1.4 Worked examples (non-normative)

**A loan at the shelf (fixed seed).**

> Day 51 — `Take(item_lamp_02)` through the inventory owner. Hook 2 renders empty; loan line:
> "lamp, out, day 51, convention: when done."
>
> Day 57 — the board's shelf band reads "lamp out 6 days". No penalty. No reminder. The plain
> arithmetic is the entire social mechanism.
>
> Day 58 — `Return(item_lamp_02)`, carrying the Mending plan's opaque tick. Hook 2 fills; the
> loan line closes; the tick is not the shelf's business and is not recorded here.
>
> `item_hook_3` renders its empty hook and its open loan line "since day 0" in the same plain
> type as everything else.

**A disputed wind (fixed seed).**

> The glossary renders *dry-that-isn't* with both readings in equal type: Iron Basin's "rain
> follows" and Fen Crossing's "rain preceded". The row carries `use_disputed: true` and two
> weather-state ids. A briefing may cite the word beside one formal state and must print the
> dispute mark beside it; the citation writes nothing.

---

## 2. Evidence table (checked against the corpus 2026-09-29; re-verify at P0 — Rule 7)

| # | Claim | Evidence | Status |
|---|---|---|---|
| E1 | Inventory owner expresses issue/return as ordinary transactions; guest borrows already designed as `borrowed`/`returned` pairs. | `.ai/plans/the-fair-and-the-guest-book-2026-09-29.md` GB-P1/E7 | PROPOSED (plan is DRAFT) |
| E2 | Item-tag vocabulary exists for household objects (Plan 22 shared item tags). | `docs/plans/PLAN_22_CONSUMABLE_BILLS_INTEGRATION_PLAN.md` | LIVE (plan exists) |
| E3 | Living Region E5: four unmapped region vocabularies; "one mapping file is added and nothing is deleted". | `.ai/plans/living-region-2026-09-29.md` §1b | PROPOSED (plan is DRAFT) |
| E4 | Weather states exist as authored rows (`year_of_ash_storm_windows.json` 14 windows; `weather_gameplay_effects.json`; `WeatherIntelligenceCoordinator`). | `.ai/plans/the-sky-2026-09-29.md` E10/E14 | LIVE |
| E5 | Second Language DEC-SL-04 refuses vocabulary reconciliation; glossary rendering precedents exist (SL-P1). | `.ai/plans/the-mending-and-the-second-language-2026-09-29.md` | PROPOSED (plan is DRAFT) |
| E6 | Mending opaque marks are tags readable by consumers (MN-P1 mark_count). | same, DEC-MN-03 | PROPOSED (plan is DRAFT) |
| E7 | Board/briefing surfaces can render one cited line without a new routed panel (precedents: chain band, shelf band). | batch-3 plans §4 | **VERIFY (P0)** |
| E8 | Whether the inventory owner allows an item to be *issued to nobody* (the `always_out` row's open loan) without corrupting counts. | inventory owner semantics | **VERIFY (P0)** |
| E9 | Glossary/mapping persistence needs (derived render vs. stored rows) and additive DTO checksum-safety. | codec / snapshot tests | **VERIFY (P0)** |

## 3. Authority table

| Concern | Owner (unchanged) | This plan adds |
|---|---|---|
| Ownership, stock counts | inventory owner | nothing; Take/Return are its own transactions |
| Item identity/tags | Plan 22 tag vocabulary | shelf rows reference existing tags; no new items |
| Guest debts | Guest Book (batch 3) | nothing; household loans are a separate ledger and stay separate |
| Weather states | weather owners | nothing; wind-names map *to* state ids and never change them |
| Regional vocabularies | Living Region E5 discipline | the one weather-slice mapping file; adds and deletes nothing |
| Coinage/words | Second Language (batch 3) | boundary only: weather words are WN's; all other words stay SL's |
| Shelf/loan state | — | `LoanShelf` (pure Core; ledger rows only) nested additively — **DEC-LN-01** |
| Wind-names | — | `WindNames` (pure Core, closed table + validator) + glossary rows in data |

## 4. Claimed Paths (proposed; `INT` = integrator-owned)
**Core:** `Assets/Ashfall.Core/Household/LoanShelf.cs` (new, pure), `World/WindNames.cs` (new, pure)
**Data:** `loan_shelf_rows.json`, `wind_name_words.json`, `loan_shelf_lines.json`
**Host:** inventory host session (`INT`), board/briefing surface (`INT`)
**Presentation:** a "Shelf" band on the existing board surface; a "Words" band beside the weather briefing — no new routed panel
**Tests:** `Ashfall.Core.Tests/Household/LoanShelfTests.cs`, `World/WindNamesTests.cs`, `Ashfall.Core.Tests/Save/LoanShelfSaveTests.cs`

## 5. Packages

### LN-P0 — Premise audit (Auditor; read-only): close E7–E9; confirm issue-to-nobody semantics with the inventory owner; confirm the shelf-band render point.
### LN-P1 — Shelf table + loan ledger (Core + data): ≤ 20 rows, `always_out`, convention days with no enforcement, validator row-level. **Accept:** round-trip; conservation through inventory; no rows → identical behaviour.
### LN-P2 — Take/Return verbs + shelf band (host): ordinary transactions, derived shelf state, plain out-day rendering. **Accept:** no enforcement path exists anywhere in the diff (§6.3).
### WN-P1 — Wind-names table + glossary (Core + data): ≤ 24 rows, `use_disputed` renders both readings, mapping file discipline (adds/deletes nothing). **Accept:** no weather-owner writes; no prose in code.
### WN-P2 — Citation hook (host, dark): board/briefing may cite one word beside a state with the dispute mark. **Accept:** citation is read-only; identical behaviour with no rows.
### X-P1 — Seam hooks (host, shipped dark): weather word beside an out-day; `mapped_by` provenance string. **Accept:** one string each way; no other coupling.

## 6. Acceptance criteria
1. Core authority + host owner + event path + persistence + observable outcome agree (CLAUDE.md "integrated").
2. Ship-dark parity: no shelf rows and no wind-name rows → inventory, weather, board and briefing outputs identical on a saved corpus.
3. No-enforcement invariant: the diff contains no reminder, penalty, quest, or follow-up path of any kind (DEC-LN-02).
4. Conservation: every Take/Return balances through the inventory owner; the `always_out` object is counted exactly once in every state (E8 closed at P0).
5. Determinism: identical shelf states and glossary renders on replay (`CampaignStreamIds` fork; never `System.Random`).
6. Save round-trip mid-loan; old saves load; checksum-safe (E9).
7. Mapping-file discipline: the wind-names table adds no weather state, deletes no vocabulary entry, and prints both readings of every disputed row (DEC-WN-01).
8. The registered silences of the Guest Book, Second Language and Living Region stay unanswered (§7).

## 7. Cross-plan boundaries
- **The Guest Book (batch 3):** guest debts are its ledger; household loans are this one. The two formats rhyme and must never merge.
- **The Mending / Second Language (batch 3):** marks are read-only; weather words are WN's *by exception* (DEC-SL-04's narrow carve-out), all other words stay SL's.
- **Living Region:** the mapping file is the file E5 invited; the board may cite it and never writes to it.
- **Plan 22 / Flagship XII:** tags are referenced, never extended; a collectible on loan is still a collectible and XII's mechanics apply unchanged.
- **The Fair (batch 3):** wind-names may be "argued about at every Fair" in prose only; no Fair mechanic reads this table.

## 8. Decision register (proposals — unsigned)
| ID | Decision | Type | Proposed |
|---|---|---|---|
| DEC-LN-01 | Loan ledger nests additively; shelf state is derived except loan rows. | architecture | Yes; confirm in P0 |
| DEC-LN-02 | No enforcement of any kind: convention days are hopes with dates. | rule | Yes |
| DEC-LN-03 | Loans carry no names; the format records objects and days only. | tone | Yes |
| DEC-LN-04 | The `always_out` row is one object, open since day 0; its holder is never modelled. | tone | Yes — **needs canon note** |
| DEC-WN-01 | The mapping file adds nothing and deletes nothing; disputed rows print both readings. | rule | Yes |
| DEC-WN-02 | Weather words are WN's by the one exception DEC-SL-04 permits; the carve-out is words-about-weather only. | boundary | Yes |
| DEC-WN-03 | A citation may show the dispute mark; a citation may never resolve one. | tone | Yes |
| DEC-X-01 | Both seam hooks ship dark; one string field each way. | architecture | Yes |

## 9. Pre-flight Checks
- [ ] Go config/save validator passes (`bin/validate-config` / `bin/ashfall-dev validate-config`)
- [ ] No equivalent existing system found (search `Loan`, `Shelf`, `WindName`, `Glossary`, `Vocabulary`)
- [ ] Premise re-verified (Rule 7); E5 and DEC-SL-04 re-read; their registered silences listed in the handoff as untouchable
- [ ] Signed decisions in hand (at minimum DEC-LN-02, DEC-WN-01)

## 10. Verification
- [ ] `bin/run-scoped-tests` on new tests + existing inventory, weather and board tests (list from P0 selector)
- [ ] Inventory host selftest with shelf rows on and off (VERIFY args)
- [ ] Scoped only (<30 s each); ≤10–15 test-edit steps per failure, then auto-flag in `.ai/state.md`; **no full suite** without `RUN FULL TESTS`
- [ ] Handoff in `AI_AGENT_WORKFLOW.md` format; `.ai/state.md` updated

## 11. Stop conditions (Rule 10)
Stop and report if: the inventory owner cannot express issue-to-nobody without count corruption; any enforcement path would be required by a hook or surface; a disputed row cannot print both readings in equal type; the mapping file would need to rename or delete anything; any path overlaps a live claim.

## 12. Open Mysteries & Deliberate Silence (lore register — no authority, no claimed path)

These questions are **intentionally unanswered**. They are not bugs, not TODOs, and not hidden
work items. They keep the shelf circulating and the sky larger than its tables. Any future plan
that answers one must name the decision that permits it.

| # | Question | Why it stays open | Who may answer it (later, signed) |
|---|---|---|---|
| LN-OM-1 | Who has the hook-3 object? | The format records objects and days, never names (DEC-LN-03); the empty hook is the whole biography. | Never — deliberately sealed. |
| LN-OM-2 | Why is there no sign-out? | Trust or discipline is not modelled and the shelf's three-clause rule has no author. | Never — texture by omission. |
| LN-OM-3 | Who added the fourth clause in pencil? | "Bring it back dry" is kept by someone unrecorded (§1c); the shelf's margin is not a register. | Never — tone-locked. |
| LN-OM-4 | Will the hook-3 object ever come back? | The ledger row is open and no mechanic closes it; the plan refuses to script its return. | A signed content pass that names its decision. |
| WN-OM-1 | Which settlement is right about the disputed wind? | Both readings map to real states (DEC-WN-01); the file is a bridge and bridges do not adjudicate. | Never — deliberately sealed. |
| WN-OM-2 | What did the three crossed-out words name? | A settlement the survey did not reach is outside the data (§12); the crossings-out are the only trace. | Never — texture by omission. |
| WN-OM-3 | Are the wind-names older than the weathers they name? | Words map to states; which came first is not modelled and must not be asserted. | Canon owner only, as a signed decision. |
| WN-OM-4 | Do the other three vocabularies have a mapping file waiting? | E5 says one file is added; whether the others are ever written is not asserted. | Living Region's owner, if ever authored. |
| WN-OM-5 | Why do two settlements share a word for opposite weathers? | Convergence, inheritance and coincidence all fit the data; the plan refuses to choose (inherits FA-OM-3's discipline). | Never — the pair stays unresolved together. |
