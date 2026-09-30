# Feature / Task Plan: The Wardrobe (the household's clothes and their histories) & The Night Shift (the culture of night work)

<!-- UPON INTEGRATION (MANDATORY):
Prepend header at the top saying:
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**
And immediately move this file to .ai/plans/integrated/<category>/
-->

STATUS: DRAFT — awaiting user approval (not self-approved; CLAUDE.md Rule 8 requires `STATUS: APPROVED BY USER` before any code commit). Story batch 4, plans 3 of 4 (subjects 45–46).

> **Subjects covered (2 of the 8 in the "rhythm and circulation" batch):**
> 45. **The Wardrobe** — what the household wears: hand-me-downs with histories, the best coat,
> the work clothes, and the small provenance of garments. (Prefix `WR`.)
> 46. **The Night Shift** — the culture of working nights: the night roles, the night's different
> rules, and the rota line that is covered but never assigned. (Prefix `NS`.)
>
> **Prose / design companions that already exist and are extended, not replaced:**
> `.ai/plans/the-mending-and-the-second-language-2026-09-29.md` (mend marks on garments; the
> wardrobe *reads* marks and never writes them), `docs/plans/PLAN_22_CONSUMABLE_BILLS_INTEGRATION_PLAN.md`
> (item tags; consumable clothing-as-item), `docs/plans/UNBLOCK_EXPANSION36_NIGHT_WATCH_INTEGRATION_PLAN.md`
> (patrol readiness — the Watch is not the Shift), `docs/plans/UNBLOCK_EXPANSION41_THE_QUIET_INTEGRATION_PLAN.md`
> (sleep, soundproofing, crowding), `.ai/plans/the-signal-chain-and-the-listening-hour-2026-09-29.md`
> (the Listening Hour is a night-hour custom — an optional neighbour).
>
> **Not a claim.** This plan claims no path in `WORKTREE_OWNERSHIP.md`, edits no source, no data,
> no ledger and no other plan. It is documentation only. Paths below are *proposed*; `INT` marks
> integrator-owned shared seams.

> **Editorial note (prose texture).** Sections **0**, **1b**, **1c** and **12** carry story texture
> and voice. Sample lines are content candidates for `*.json` rows; they belong in data, never in
> code. No authority, path, decision, acceptance criterion or verification step is changed by any
> prose section.

---

## 0. Prologue — What We Wear, and Who We Are at Three in the Morning

> *"A coat outlives its owner in this world. So does a shift."*

The wardrobe is not a clothing system. It is the household's outward biography: whose coat is on
whose shoulders, which boots have been resoled twice, the good scarf that only comes out for
fairs and funerals. Every garment in a shelter of twelve has been worn by someone else first, and
the hand-me-down chain is the closest thing to a family tree this place keeps. The plan makes that
chain *legible* — one read over the inventory seam: what each person wears, what condition it is
in, and where it came from.

The night shift is the other uniform. After the curfew ring the shelter keeps a different set of
rules: fewer voices, softer doors, work that the day people never see. Stokers, the sick ward's
night watch, the hour of silence at the radio, the walk between the stacks. Night people become
a tribe without anyone deciding they should — they share a schedule, and a schedule is the
smallest possible culture.

**Tone & register.** Textile-plain and low-lit. The wardrobe's vocabulary is *worn, mended,
passed, outgrown, kept*; the shift's is *shift, rota, cover, quiet, handover*. Prose for the
wardrobe should read like a careful inventory written by someone who knows every garment's
history; prose for the night shift like a handover notebook — clipped, kind, and awake.

**Mystery & texture.** Two silences hold the pair. One coat in the wardrobe fits everyone who
tries it, has been passed down four times, and carries no mark, no label and no name (§12). And
in the night rota there is a line that is **covered every night and assigned to nobody** —
signed for in a hand that matches no roster entry (§12). Neither is a puzzle with a solution in
this plan. Both are what happens when households and schedules outlast their record-keepers.

**The second layer.** Clothes and shifts are both *skins*: one against the weather, one against
the dark. And both carry their histories in the same way — a darn, a covered line — as small
signs that someone maintained a thing for someone not yet here. The plan's quiet thesis is that a
shelter's true inventory is not what it owns but what it *keeps working*: coats kept warm, hours
kept staffed. The fitting coat and the phantom rota line are the two things this household
maintains without understanding, which is close enough to the definition of a tradition.

---

## 1. Goal & Outcome

> *Design intent: the player should be able to look at the night roster and feel the difference
> between a shift that is filled and a shift that is *kept*.*

### 1.1 The Wardrobe (WR)

- **Goal:** A **Wardrobe Read** (derived, pure) over the inventory and roster seams: per person,
  the garment categories they hold (coat / boots / work-clothes / good-clothes), each garment's
  condition *as the inventory owner already tracks it*, and an optional opaque **pass-chain**
  tag string recording only *how many previous wearers* (a count, never names — DEC-WR-03); one
  authored **Wardrobe Rows** table for household-level garments (the best coat, the shared
  oilskins) with `fits_all` flag; and plain rendering on the existing board/wardrobe surface.
  Garments are ordinary inventory items; the wardrobe owns no stock (DEC-WR-01).
- **Outcome (observable):** on a fixed seed, each person renders their garment categories with
  condition and pass-count; the `fits_all` coat renders its chain count (4) with no names and no
  marks; a garment passed on increments its chain through the inventory owner's own transfer;
  with no wardrobe rows every owner behaves identically to today; save/load round-trips.
- **Non-Goals:** no clothing stats, warmth or insulation model (The Quiet and weather owners keep
  their meanings); no character appearance system; no laundry/cleaning mechanics (deferred: see
  §8 DEC-WR-05); no crafting or tailoring (The Mending's boundary: repair verbs are theirs);
  no new save section; no new routed panel; no Unity.
- **"Done":** §6 acceptance passes via `bin/run-scoped-tests`; ship-dark parity holds; handoff
  lists untouched shared paths.

### 1.2 The Night Shift (NS)

- **Goal:** A **Night Rota Read** (derived) over the duty-roster seam: which roles are staffed in
  the authored night window, the handover lines between shifts (one line per role per night:
  state, `covered`/`bare`, role — never a person), and one optional **Night Custom** hook
  (evenings-and-memory-work seam): an authored night practice (the soft-door rule, the kettle,
  the shared silence) whose effects route to existing owners only. The Watch's patrol readiness
  stays theirs; the Shift is *culture*, not security (DEC-NS-02).
- **Outcome (observable):** on a fixed seed, the night window shows its staffed roles; a bare role
  renders "bare" plainly with no penalty and no alarm; the phantom line renders as `covered` with
  its unmatched hand as a format artefact and nothing more; the optional night custom performs no
  verb of its own; with no night rows every owner behaves identically to today; save/load
  mid-night round-trips.
- **Non-Goals:** no watch/patrol mechanics (Exp. 36 owns them); no sleep or dream mechanics (The
  Quiet and the Dream System own them); no shift-work health model; no coercion onto nights; no
  new save section; no new routed panel; no Unity.
- **"Done":** §6 acceptance passes; ship-dark parity holds; handoff lists untouched shared paths.

### 1.3 The seam between them (X)

- **Goal:** One documented boundary and one optional hook: night roles may render the garment
  they wear against the cold ("night watch — the long coat, mended×2") by reading the wardrobe's
  pass-count and the Mending plan's mark-count *read-only*; and a handover line may cite the
  night's weather word (Wind-Names seam, batch 4 plan 1). One string and one count each way,
  shipped dark.

---

## 1b. Texture, Mystery & Voice

**A garment is a sentence with a hem.**

The pass-chain count is the wardrobe's whole poetry: `passed×4` in plain type says everything
without one name. Write garments the way the Cabinet writes objects — plainly, materially — but
with the one difference that matters: these objects are *still in use*. The wardrobe is the
Cabinet's living cousin.

**Night prose is day prose at lower volume.**

The shift's register should be the daytime register with the adjectives removed. Handover lines
clip. Nothing at three in the morning is described; it is *noted*. The phantom line must be
rendered exactly like every other line — the format's flatness is what makes it strange.

**What the player is never told.**

- Who the `fits_all` coat was made for. It fits everyone, has passed four times, and carries no
  maker's mark — the only unmarked garment in the house (§12).
- Who covers the phantom line. The hand matches no roster entry; whether it is one hand or a
  habit of handwriting is not modelled and must not be.
- Why good-clothes exist in a shelter with nowhere to go. The category is authored; its use (the
  Fair, the wall) is implied by the corpus and never asserted here.
- What the night's rules *feel* like. The Quiet owns sleep; this plan owns only the rota and the
  custom, and the feeling is prose's job.

**Voice — sample fragments (content candidates for `wardrobe_lines.json` / `night_shift_lines.json`).**

> "Wardrobe: twelve coats, nine worn, two mended, one that fits everyone and has fitted four
> people before this one. The chain count is the only family tree the house keeps." — wardrobe
> read (WR)

> "Boots, pair 3: resoled twice, passed twice. The boot does not know any of this. The boot is
> the only party here without an opinion." — wardrobe read (WR)

> "Handover, 03:00. Ward: quiet. Stove: banked. North corridor: the draft, as always. Kettle
> boiled for the next hand, which is not in the book and is expected anyway." — night book (NS)

> "Rota, line 7: covered. Hand unmatched. The book does not correct it; the book has no column
> for doubt." — night book (NS)

**Design texture beats.**

- **Counts, never names** (DEC-WR-03). The moment a pass-chain names a wearer, it becomes a
  genealogy and the garment becomes a relic.
- **Bare is not failure** (DEC-NS-01). A bare night role renders plainly with no alarm; the
  household's quiet coping is the tone.
- **The phantom line renders identically to every other line.** Flatness is the format's honesty
  and the mystery's engine.
- **Night customs route to existing owners only.** The shift's culture must never become a
  mechanic with its own verbs.

---

## 1c. The Deeper Layer — objects & held silences (second prose pass)

*(Second prose pass, non-contractual: narrative texture and writing guidance only — no authority,
no claimed path, no acceptance criterion, no verification step. §12's register is unchanged; the
fragments below are content candidates and deliberate silences, not new recorded questions. Where
a named data file holds no prose field, fragments are texture only and gain no schema.)*

**What the wardrobe leaves lying around.**

> "Hem, let down twice. Two threads of older dye in the fold. The garment has been three heights
> in its life and worn them all well."

> "Oilskin, shared: hung by the door, in the same corner, since the first stocktake. The corner
> is on no plan of the room. The corner is where it hangs."

> "Good-clothes, one set, pressed. Pressed by whom the book does not say. Pressed *for* what the
> book does not ask."

**What the night leaves lying around.**

> "Handover, line 1: 'nothing happened.' Three words, every quiet night, for two hundred nights.
> The repetition is the reassurance."

> "Kettle rule: boiled for the next hand. The rule has no author and has never been skipped,
> including by people who do not drink it."

> "Rota, line 7's hand. The same pressure, the same slant, every night. A hand that consistent is
> a habit, and a habit is not a person." — texture only

**Held silences (texture, not register rows).**

- Who pressed the good-clothes and for what. The set is authored and its occasions are implied by
  other plans' calendars; the pressing is recorded and unowned. Texture only.
- Whether the phantom line is covered by one person or by the shelter's conscience. The hand is
  consistent (§1c) and the roster is silent; the plan keeps the difference unresolved.

---

## 1.4 Worked examples (non-normative)

**A wardrobe read (fixed seed).**

> Survivor 3's render: coat `item_coat_07` (condition: Worn, passed×4 — the `fits_all` garment,
> unmarked), boots `item_boots_03` (resoled×2), work-clothes (Worn), good-clothes (kept). Every
> count is a number; no name appears anywhere in the surface. Passing `item_coat_07` to survivor 7
> increments its chain to 5 through the inventory owner's ordinary transfer and nothing else
> changes.

**A night's book (fixed seed).**

> Night 214, authored night window: stove (staffed), ward (staffed), radio-hour (staffed — the
> Listening Hour custom is enabled tonight and the handover cites it in one string), north
> corridor walk (bare — renders plainly). Line 7: covered, hand unmatched, rendered exactly like
> the others. At 03:00 the handover is written in the book's clipped voice and the day bells
> begin again at muster.

---

## 2. Evidence table (checked against the corpus 2026-09-29; re-verify at P0 — Rule 7)

| # | Claim | Evidence | Status |
|---|---|---|---|
| E1 | Inventory owner tracks items and condition per owner precedents (garments as ordinary items; Plan 22 tags). | `docs/plans/PLAN_22_CONSUMABLE_BILLS_INTEGRATION_PLAN.md`; inventory owner | LIVE (per corpus) |
| E2 | Mending plan: opaque mark tags, `mark_count`, read-only consumers (MN-P1/E7). | `.ai/plans/the-mending-and-the-second-language-2026-09-29.md` | PROPOSED (plan is DRAFT) |
| E3 | Duty-roster posts, roles and handover-adjacent seams (Rounds, link crew, bell duty precedents). | batch-3/4 plans; `duty_roster_*.json` | LIVE (per corpus) |
| E4 | Night Watch owns patrol readiness and sound ranging; "the Watch hears everything and is not authorised to decide anything". | `docs/plans/UNBLOCK_EXPANSION36_NIGHT_WATCH_INTEGRATION_PLAN.md` | LIVE (closeout) |
| E5 | The Quiet owns sleep quality, soundproofing, crowding; the Dream System owns sleep events. | Exp. 41 / `docs/plans/UNBLOCK_PLAN177_DREAM_SYSTEM_INTEGRATION_PLAN.md` | LIVE (closeouts) |
| E6 | Custom machinery hosts authored customs routed through existing verbs (EV seam; Mending Day, Wood Day precedents). | batch-3/4 custom hooks | PROPOSED (soft dependency) |
| E7 | Wind-Names citation seam (one string) exists in batch-4 plan 1. | `.ai/plans/story-expansion-batch-4/the-loan-shelf-and-the-wind-names-2026-09-29.md` WN-P2 | PROPOSED (plan is DRAFT) |
| E8 | Whether an item transfer can carry a pure incrementing counter without schema drift (pass-chain). | additive-field precedents (mark_count); codec tests | **VERIFY (P0)** |
| E9 | Roster surface render point for night reads and handover lines; role-only attribution semantics. | BL-P0/E8 (role-only log precedent) | **VERIFY (P0)** |

## 3. Authority table

| Concern | Owner (unchanged) | This plan adds |
|---|---|---|
| Garments, stock, condition | inventory owner | nothing; pass-count is one additive counter on transfer |
| Repair marks | The Mending (batch 3) | read-only; the wardrobe never writes marks |
| Roles, rosters | duty-roster owner | nothing; the night read is derived |
| Watch/patrol | Exp. 36 | nothing; culture is not security (DEC-NS-02) |
| Sleep | The Quiet / Dream System | nothing |
| Customs | evenings-and-memory-work | one optional night custom |
| Wardrobe/night presentation | — | `WardrobeRead` + `NightRotaRead` (pure derived) — **DEC-WR-02 / DEC-NS-03** |

## 4. Claimed Paths (proposed; `INT` = integrator-owned)
**Core:** `Assets/Ashfall.Core/Household/WardrobeRead.cs` (new, pure), `Shelter/NightRotaRead.cs` (new, pure)
**Data:** `wardrobe_rows.json`, `night_custom_rows.json`, `wardrobe_lines.json`, `night_shift_lines.json`
**Host:** inventory transfer hook (`INT`), roster host session (`INT`), custom hook (EV seam, `INT`)
**Presentation:** a "Wardrobe" band on the existing roster/stock surface; a "Night" band on the existing duty surface — no new routed panel
**Tests:** `Ashfall.Core.Tests/Household/WardrobeReadTests.cs`, `Shelter/NightRotaReadTests.cs`, `Ashfall.Core.Tests/Save/WardrobeNightSaveTests.cs`

## 5. Packages

### WR-P0 — Premise audit (Auditor; read-only): close E8–E9; confirm the transfer-counter semantics; confirm role-only handover rendering; confirm category mapping to existing item tags.
### WR-P1 — Wardrobe read + rows (Core + data): categories, condition read-through, pass-counts, `fits_all`, validator row-level. **Accept:** counts never names; no stock writes; determinism.
### WR-P2 — Wardrobe band (host): plain rendering; increments through the owner's own transfer. **Accept:** no clothing stats exist anywhere in the diff (§6.3).
### NS-P1 — Night rota read + handover lines (Core + data): staffed/bare, role-only lines, phantom-line flat rendering. **Accept:** bare renders with no penalty; no person ids (§6.4).
### NS-P2 — Night custom hook (host, dark): existing verbs only. **Accept:** no new verb in the diff.
### X-P1 — Seam hooks (host, dark): garment line citing mark-count; handover citing a weather word. **Accept:** one count and one string; no cross-reads.

## 6. Acceptance criteria
1. Core authority + host owner + event path + persistence + observable outcome agree (CLAUDE.md "integrated").
2. Ship-dark parity: no wardrobe rows and no night rows → inventory, roster, watch, sleep and board outputs identical on a saved corpus.
3. No-stats invariant: the diff introduces no warmth, insulation, fatigue or clothing stat of any kind (DEC-WR-04).
4. Attribution invariant: pass-chains are counts, handover lines are roles — no names anywhere (DEC-WR-03, DEC-NS-04).
5. Conservation: garment transfers balance through the inventory owner; pass-counts increment exactly once per transfer (E8 closed at P0).
6. Determinism: identical wardrobe reads, rota reads and handover contents on replay (`CampaignStreamIds` fork; never `System.Random`).
7. Bare roles render plainly with no penalty and no alarm (DEC-NS-01); the phantom line renders with format-identical flatness (DEC-NS-05).
8. The registered silences of The Mending, The Quiet, Exp. 36 and evenings-and-memory-work stay unanswered (§7).

## 7. Cross-plan boundaries
- **The Mending (batch 3):** marks are its language; the wardrobe counts them and never writes them.
- **The Quiet / Dream System:** sleep and crowding are theirs; the shift owns only rota and custom.
- **Night Watch (Exp. 36):** patrol readiness is theirs; "the Watch hears and decides nothing" is preserved — the Shift decides even less.
- **The Loan Shelf / The Guest Book (batch 3/4):** a borrowed coat is a loan, not a wardrobe entry; a guest's clothes are the guest's.
- **The Day Bell (batch 4):** the curfew ring starts the night window by convention; no state is shared.
- **Evenings and Memory Work:** custom machinery is theirs; who started the first custom stays in EV's register (the kettle rule inherits that silence).

## 8. Decision register (proposals — unsigned)
| ID | Decision | Type | Proposed |
|---|---|---|---|
| DEC-WR-01 | Garments are ordinary inventory items; the wardrobe owns no stock. | architecture | Yes |
| DEC-WR-02 | The wardrobe is a derived read plus one additive pass-count. | architecture | Yes; confirm in P0 |
| DEC-WR-03 | Pass-chains record counts, never names. | tone | Yes |
| DEC-WR-04 | No clothing stats of any kind: warmth and insulation stay with their owners. | rule | Yes |
| DEC-WR-05 | Laundry/cleaning mechanics deferred; the good-clothes stay pressed by no one in particular. | scope | Yes — **needs signature to revisit** |
| DEC-NS-01 | Bare is not failure: no penalties, no alarms, no morale writes. | tone/rule | Yes |
| DEC-NS-02 | Culture, not security: the Shift never touches watch or patrol state. | boundary | Yes |
| DEC-NS-03 | The night read is derived; handover lines are stored only as history facts. | architecture | Yes; confirm in P0 |
| DEC-NS-04 | Handover lines carry roles, never persons. | tone | Yes |
| DEC-NS-05 | The phantom line renders with format-identical flatness and is never explained. | tone | Yes — **needs canon note** |

## 9. Pre-flight Checks
- [ ] Go config/save validator passes (`bin/validate-config` / `bin/ashfall-dev validate-config`)
- [ ] No equivalent existing system found (search `Wardrobe`, `Clothing`, `NightRota`, `Handover`, `Shift`)
- [ ] Premise re-verified (Rule 7); Mending, Exp. 36, The Quiet and EV registers re-read; their silences listed in the handoff as untouchable
- [ ] Signed decisions in hand (at minimum DEC-WR-03, DEC-NS-02, DEC-NS-05)

## 10. Verification
- [ ] `bin/run-scoped-tests` on new tests + existing inventory, roster and watch tests (list from P0 selector)
- [ ] Inventory transfer hook selftest with wardrobe rows on and off (VERIFY args)
- [ ] Scoped only (<30 s each); ≤10–15 test-edit steps per failure, then auto-flag in `.ai/state.md`; **no full suite** without `RUN FULL TESTS`
- [ ] Handoff in `AI_AGENT_WORKFLOW.md` format; `.ai/state.md` updated

## 11. Stop conditions (Rule 10)
Stop and report if: a pass-count cannot be maintained without naming wearers; any clothing stat would be required by a surface or hook; the phantom line cannot render flat by construction; the night read would touch watch or sleep state; any path overlaps a live claim.

## 12. Open Mysteries & Deliberate Silence (lore register — no authority, no claimed path)

These questions are **intentionally unanswered**. They are not bugs, not TODOs, and not hidden
work items. They keep the wardrobe deeper than its inventory and the night larger than its rota.
Any future plan that answers one must name the decision that permits it.

| # | Question | Why it stays open | Who may answer it (later, signed) |
|---|---|---|---|
| WR-OM-1 | Who was the `fits_all` coat made for? | Four wearers, no mark, no label (§12); naming the maker converts a garment into a legend. | Never — deliberately sealed. |
| WR-OM-2 | Why does it fit everyone? | Tailoring is not modelled (DEC-WR-04); the fit is a fact of the data and the plan declines to explain it. | Never — tone-locked. |
| WR-OM-3 | Who pressed the good-clothes, and for what? | Pressing is recorded and unowned (§1c); the occasions live in other plans' calendars and this one refuses to consult them. | Never — texture by omission. |
| WR-OM-4 | Where did the older dye threads come from? | The hem's two threads are texture (§1c); garment archaeology is not a data concern and must not become one. | Never — texture by omission. |
| WR-OM-5 | Is the wardrobe the same thing the Cabinet would be if the house stopped? | Living cousinship (§1b) is an image, not a claim; the two ledgers must never merge. | Never — a rule, not a gap. |
| NS-OM-1 | Who covers the phantom line? | The hand matches no roster entry (§12); one hand or many is not modelled (DEC-NS-05). | Never — deliberately sealed. |
| NS-OM-2 | Was there a night the line went bare? | The book records two hundred covered nights; whether the series has a gap is not asserted. | Never — tone-locked. |
| NS-OM-3 | Who started the kettle rule? | Custom provenance belongs to EV's register and is inherited here unchanged. | EV plan's owner (its register governs). |
| NS-OM-4 | What does the night shift hear in the fourth ring that the day shift does not? | Cross-referenced from BL-OM-4; this plan observes the difference and does not describe it. | Never — the pair stays unresolved (mystery-index §3 discipline). |
| NS-OM-5 | Are the night people different people, or the same people at lower volume? | Tribes from schedules (§0) is the plan's image; the roster holds roles and the prose holds the rest. | Never — texture by omission. |
