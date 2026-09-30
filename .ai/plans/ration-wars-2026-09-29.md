# Feature / Task Plan: The Ration Wars — the pantry as politics (table rules, quartermaster, ledger, audit)

<!-- UPON INTEGRATION (MANDATORY):
Prepend header at the top saying:
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**
And immediately move this file to .ai/plans/integrated/<category>/
-->

STATUS: DRAFT — awaiting user approval (not self-approved; CLAUDE.md Rule 8 requires `STATUS: APPROVED BY USER` before any code commit)

> Prose companion: `docs/expansions/expansion_ration_wars_plan.md`. Family index: `docs/expansions/expansion_shelter_under_pressure_index.md`.
> Not a claim. Ration conflict, rationing tiers, kitchen and justice each have an owner; this plan **adds a reader/reconciler and one small expected-portion seam** and creates no second food, resentment or law authority.

> **Editorial polish (prose pass):** sections **0**, **1b**, **1c** and **12** are narrative texture only. No
> authority, claimed path, decision, acceptance criterion or verification step changes. Sample lines
> are content candidates for `pantry_book_lines.json` / `quartermaster_voices.json` / `hard_table_events.json`
> rows; they belong in data, never in code.

---

## 0. Prologue — The Book

> *"Everyone can count. That is the whole problem, and the whole solution."*

Hunger in this game is already modelled, exhaustively: tiers, protocols, priority groups,
portioning, desperation events. What is not modelled is the thing that actually breaks a shelter —
the arithmetic of *who got what*. Not the food. The arithmetic.

The Pantry Ledger is a line of subtraction and nothing more: opening + received − served − spoiled
− closing = **unexplained**. It does not accuse anyone. It does not know anything the inventory
owner does not already know. It simply refuses to let the difference go unremarked, and in a
shelter of adults that is enough.

**Tone & register.** Domestic, forensic, quietly furious. The vocabulary is the kitchen and the
counting house: *portion, expected, served, spoiled, unexplained, Book, Count*. Prose should feel
like a kitchen argument that has been going on for weeks and has finally acquired a column of
figures. Never moralise about food. The numbers do that work.

**Mystery & texture.** E3b is the plan's great quiet discovery: priority bonuses of Critical 1.30,
High 1.15 and Standard 1.0 all clamp to **1.0** — the conflict meter literally cannot tell them
apart. That is not a bug this plan reports; it is a *fact about the shelter's own instruments* that
the Table Rule exists to correct. §12 keeps who else has noticed out of scope.

**The second layer.** Every column of the Book is a small refusal to forget. Addition remembers
what arrived; subtraction remembers what left; and the final column — *unexplained* — is the
shelter agreeing to hold a question open on purpose, on a page everyone can see. A ledger that
balances is arithmetic. A ledger that names its own imbalance is a conscience. And the Book is the shelter's proof that it
can stand to know.

## 1. Goal & Outcome

> *Design intent: resentment is not caused by hunger. It is caused by a mismatch between what you
> got and what you believe the rule to be. This plan authors the rule.*

- **Goal:** Make the pantry a political object: (a) a data-driven **Table Rule** that defines each survivor's *expected* portion so resentment is measured against the rule instead of the mean; (b) an optional **quartermaster post** and a daily **Pantry Ledger** (opening + received − served − spoiled − closing = *unexplained*); (c) **Audit** and public **Count** verbs that move legitimacy; (d) a staircase of **Hard Table events** before the desperation menu.
- **Outcome (observable):** on a fixed seed, changing the Table Rule from Equal to By-work changes a labourer's fairness deviation and resentment growth exactly per the data; a quartermaster in post with pressure ≥ threshold produces a seeded skim that genuinely removes units from a store and appears as `unexplained` in the Book; an Audit reveals the true figure; a Count changes legitimacy through the existing owner; no post and no rule → identical resentment, theft and serving behaviour to today; save/load mid-week round-trips.
- **Non-Goals:** no new food/inventory/serving log; no change to tier arithmetic, priority groups, protocols or `desperation_events.json`; no second resentment meter; no law rows (Shelter Governance owns Hoarding); no new save section; no new routed panel; no Unity.
- **"Done":** §6 acceptance passes via `bin/run-scoped-tests`; ship-dark parity holds; handoff lists untouched shared paths.

## 1b. Texture, Mystery & Voice

**Expected is the plan's most dangerous word.**

RW-P1 gives each survivor an *expected* portion so resentment is measured against a rule rather
than against the mean. That single substitution is the whole design: the mean is a statistic, but a
rule is a **promise someone made**. Announce rule changes with a delay and a three-day surcharge
(DEC-RW-04) — the surcharge is the price of changing a promise in public.

**`unexplained` is not an accusation.**

The Book's final column names a quantity, not a person. RW-P2 requires that a clean week shows
`unexplained == 0`. The plan's discipline is that the ledger never infers, never attributes, and
never remembers intent. Attribution is a *verb the player performs* — via Audit, then Count.

**What the player is never told.**

- Who skimmed. RW-P3 makes a skim a **real withdrawal** through the inventory port — or drops the
  mechanic entirely if E13 fails. Either way, motive is not modelled and must not be narrated.
- Whether the quartermaster's miscount is dishonest. A miscount changes *reported* only. The plan
  offers no way to distinguish error from theft except by Audit.
- What the Hard Table events are *leading to*. RW-P5 requires they always precede
  `desperation_events`, which are never softened (DEC-RW-07). The staircase is authored and its
  landing is not.
- Why the tiers clamp the way they do (E3b). The plan corrects the outcome when a rule is active and
  leaves the underlying arithmetic alone.

**Voice — sample fragments (content candidates for `pantry_book_lines.json` / `quartermaster_voices.json`).**

> "Book, day 41. Opening 60. Received 0. Served 58. Spoiled 1. Closing 0. Unexplained 1. I am
> writing it down because not writing it down is how this starts."

> "The rule says By-work. The rule said Equal last week. Neither of those is what people are angry
> about."

> "Audit: one half survivor-day. The figure is now known. What we do with it is a different column."

> "Count. The Count is weekly and legitimacy is table-driven, which is the politest sentence in
> this book."

**Design texture beats.**

- **Ship-dark parity is a tone promise (§6.2).** No rule, no post → identical resentment, theft and
  serving behaviour. This plan must never *introduce* a grievance.
- **An honestly announced shortage costs less than a discovered one (RW-P4).** That asymmetry is
  the plan's entire political theory, and it should be visible in the numbers.
- **Conservation holds everywhere (§6.3).** A skim is a transfer, a feast is a spend, and nothing
  is ever conjured. The Book is checkable and that is why anyone believes it.
- **Desperation is never softened (DEC-RW-07).** Hard Table events come first; they do not replace.

---

## 1c. The Deeper Layer — scenes, artifacts & held silences (second prose pass)

*(Second prose pass, non-contractual: narrative texture and writing guidance only — no authority,
no claimed path, no acceptance criterion, no verification step. §12's register is unchanged; the
fragments below are content candidates and deliberate silences, not new recorded questions. Where
a named data file holds no prose field, fragments are texture only and gain no schema.)*

**What the shelter leaves lying around.**

> "Rule card, new: By-work. The old card is underneath it in the drawer — Equal, dated. Both are true in the drawer."

> "Spoilage slip: 1. No name. The Book accepts the slip. The Book accepts every slip and that is exactly the problem."

> "Count sheet, week 3. Everyone signed. One signature is smaller than the others and no comment is made about it, on the sheet or anywhere else."

**Scenes the player may piece together.**

> "The kitchen argument and the kitchen are in different rooms. The Book is where they meet: one line, subtraction only."

> "A skim of half a survivor-day is the smallest possible theft and the largest possible fact. The Book records the half. The Book does not record the hand."

**Held silences (texture, not register rows).**

- Who calibrated the instruments that clamp the tiers (E3b). The plan corrects the outcome and leaves the arithmetic alone; the calibration sheet is not in the data and must not be added. Texture only.
- What a fair Count would sound like. The Count is table-driven (§6); fairness is a thing the reader brings to the column and the column does not supply.

**Third pass — three fragments (texture only; §12 register unchanged).**

> "Spoiled: 1. Reported by the same hand that served. The Book does not comment; that is the design."

> "The surcharge is paid in three days of grumbling. It is the cheapest honest price in the shelter."

> "Unexplained: 0. A clean week reads like a held breath."

**Fourth pass — the column that is a question (texture only; §12 register unchanged).**

*(Polish pass, non-contractual: wording and texture only — no authority, no claimed path, no
decision, no acceptance criterion, no verification step. §12 gains no row and loses no silence;
fragments remain content candidates for `pantry_book_lines.json` / `quartermaster_voices.json` /
`hard_table_events.json`.)*

**The shape of the polish.** Kitchen prose that has acquired a column of figures. The fury is in
the past tense and the arithmetic is in the present, and the two should never share a clause —
that separation is what lets the Book be believed. `unexplained` is the only word in the design
that is allowed to be plural about a person, and the writing must keep it a quantity.

**What the shelter leaves lying around.**

> "Book, day 42: the same five lines as day 41. The handwriting is steadier. The columns are not."

> "Spoilage slip, cause field left blank. The field is ruled. Somebody ruled it expecting a cause
> to be written, and the slip is filed as filed."

> "Count sheet, week 4. Everyone signed again. The signatures are in the same order as last week,
> which nobody arranged."

**Held silences (texture, not register rows).**

- What the unexplained column would say if it could speak. It names a quantity, never a person
  (§1b); the column is a question kept open on purpose and it must never acquire a voice. Texture
  only.
- Who reads the Book besides the people who count it. It hangs where everyone eats; the reading is
  not modelled and the seeing-together is the whole politics of it.

---

## 2. Evidence table (verified 2026-09-29; re-verify at P0 — Rule 7)

| # | Claim | Evidence | Status |
|---|---|---|---|
| E1 | Per-survivor allocation; fairness deviation 0.20; resentment +0.10/−0.03 per day; confrontation 0.70; theft 0.85; morale −10/−15; 3 events; capture/restore. | `Survivors/RationConflictSystem.cs` L31–54, L74, L97, L161–178 | LIVE |
| E2 | `Tick` compares each allocation to `GetAverageAllocation()`; `perceivedFairness = 1 − |mine − average|`; only a **deficit** ≥ 0.20 (`average − mine`) builds resentment, aimed at the most-allocated survivor. Allocation is clamped to 0..1; default 0.5. | `RationConflictSystem.cs` L74–135 | LIVE |
| E3 | `RationConflictHostSession` is the only `SetAllocation` caller; it sets each survivor's allocation to the rationing owner's `PriorityBonus` (or 1 if ≤ 0), so the ration **tier** never reaches the conflict meter on this path. | `src/Host/RationConflictHostSession.cs` L85–100, L140; `src/Main.RationConflict.cs` | LIVE |
| E3b | Priority bonuses are Critical 1.30, High 1.15, Standard 1.0, Low 0.75; after `Clamp01` the first three are all **1.0** — the conflict meter cannot tell them apart, and only Low can be short-changed. | `Economy/ResourceRationingSystem.cs` L286–294; `RationConflictSystem.cs` L74–77 | LIVE / GAP (prove with a P0 test) |
| E4 | 7 tiers, 4 priority groups, 4 protocols, `ApplyProtocol`, `AssignSurvivorPriority`, `AuthorizeAllocation`, `DeclareCrisis`. | `Economy/ResourceRationingSystem.cs` L53–80, L226, L342, L388, L430; `rationing_protocols.json` | LIVE |
| E5 | Tiers are per resource id; on the conflict path a tier does **not** reach per-survivor allocation (see E3) — confirm no other path does. | `src/Main.Economy.cs`, `Main.RationConflict.cs` | **VERIFY (P0)** |
| E6 | Kitchen serving log with retention (newest-200). | `KitchenNutritionSystem.cs` L400; `Records/RetentionPolicy.cs` L240 | LIVE |
| E7 | Inventory withdrawals carry a reason/source tag. | inventory port | **VERIFY (P0, critical for DEC-RW-03)** |
| E8 | `CrimeType.Hoarding` exists; 4 laws only, none for Hoarding. | `Narrative/JusticeSystem.cs` L15; `wasteland_laws.json` | LIVE / GAP |
| E9 | Justice evidence uses `evidenceClues` with `confidenceWeight`; law has `min_evidence_confidence`. | `JusticeSystem.cs` L51, L71, L229–251 | LIVE |
| E10 | Desperation menu: 3 rows with taboo, prion risk, mutiny pressure. | `desperation_events.json`; `Survivors/DesperationSystem.cs` | LIVE |
| E11 | 8 roles; no quartermaster; rooms kitchen, storage bay/secure, common mess hall. | `survivor_roles.json`; `shelter_rooms.json` | LIVE / GAP |
| E12 | Duty roster assigns by role/post. | `DutyRoster/DutyRosterSystem.cs` | LIVE (VERIFY post model) |
| E13 | Personal belongings owner (Plan 210) can hold consumables. | Plan 210 log | **VERIFY (P0)** |
| E14 | Celebrations can declare a food cost. | `shelter_celebrations.json` | **VERIFY (P0)** |
| E15 | Legitimacy is written through one public API. | leadership/politics owners | **VERIFY (P0)** |
| E16 | Panels: `KitchenNutritionPanel`, `DesperationCrisisPanel`, `EconomyMarketPanel`. | `src/UI/` | LIVE |

## 3. Authority table

| Concern | Owner (unchanged) | This plan adds |
|---|---|---|
| Resentment, theft, confrontation | `RationConflictSystem` | optional *expected-allocation provider* (default = mean) |
| Tiers, protocols, priority groups | `ResourceRationingSystem` | read-only |
| Serving history | `KitchenNutritionSystem` | read-only |
| Inventory | inventory owner | withdrawal through the existing port only |
| Crime, evidence, verdict | `JusticeSystem` | clue emission through its public API |
| Legitimacy | leadership/politics owner | `Count` write through public API |
| Table rule, quartermaster post, ledger rows | — | `PantryLedger` reader + nested DTO in the ration-conflict save (**DEC-RW-01**) |

## 4. Claimed Paths (proposed; `INT` = integrator-owned)
**Core:** `Assets/Ashfall.Core/Survivors/RationConflictSystem.cs` (additive optional provider + nested DTO field only), `Survivors/TableRules.cs` (new), `Survivors/PantryLedger.cs` (new, pure), `Survivors/QuartermasterPost.cs` (new), `CatalogIntegrityValidator.cs` (`INT`), `Random/` stream id (`INT`)
**Data:** `table_rules.json`, `hard_table_events.json`, `quartermaster_voices.json`, `pantry_book_lines.json` (all new; snake_case; validator-registered)
**Host:** `src/Host/RationConflictHostSession.cs` (`INT` — sole `SetAllocation` caller; provider wiring), one day-owner registration (`src/Main.CampaignOwners.cs`, `INT`)
**Presentation:** extend `KitchenNutritionPanel` (Book strip, Rule picker, Audit/Count buttons) — **DEC-RW-08**; no new routed panel
**Tests:** `Ashfall.Core.Tests/Survivors/TableRulesTests.cs`, `PantryLedgerTests.cs`, `QuartermasterSkimTests.cs`, `Ashfall.Core.Tests/Save/PantryLedgerSaveTests.cs`; extend ration-conflict tests

## 5. Packages

### RW-P0 — Premise audit (Auditor; read-only)
- Close E2, E5, E7, E12–E15: how fairness uses the mean; how tiers reach per-survivor allocation; whether withdrawals carry reasons; duty-roster post model; personal-belongings consumables; celebration costs; legitimacy write API; list every reader of `RationConflictSystem` state; foreman signs DEC-RW-01…10.
- **Accept:** each VERIFY answered with `path:line` or a test; the **fallback ledger scope** (kitchen serving log + storage-bay pantry row) chosen if E7 fails.

### RW-P1 — Table Rules & expected portion (Core + data)
- `table_rules.json`: group multipliers, work weight, process-fairness bonus; `ExpectedAllocation(survivorId)` provider optionally consumed by `RationConflictSystem`; announcement delay and surcharge (first three days) as data.
- Allocations under a rule must be **distinguishable**: the rule supplies a per-survivor *served share* on the 0..1 scale so that E3b's saturation (three tiers → 1.0) no longer hides Critical/High/Standard differences when a rule is active. With no rule the existing bonus path is untouched.
- **Accept:** provider unset → resentment sequences identical to today on a fixed corpus (parity test); each rule table-driven; deviation computed against the provider when present; with a rule set, Critical/High/Standard survivors have distinct allocations in a test.

### RW-P2 — Pantry Ledger (Core, pure reader)
- `PantryReconciliation { day, opening, received, served, spoiled, closing, unexplained }` per tracked food group; sources only from existing owners; nested rolling list with a retention policy row (**INT**: `retention_policies.json` additive).
- **Accept:** on a scripted week with no quartermaster, `unexplained == 0` (or exactly the theft events); arithmetic identity test; save round-trip; ledger holds no stock of its own.

### RW-P3 — Quartermaster post & skim/miscount (Core + host)
- Post via duty roster; seeded skim (probability from tier, the keeper's hunger, dependants) that **withdraws real units** through the inventory port into the personal-belongings owner (E13) — or is dropped per DEC-RW-05; miscount changes *reported* only.
- **Accept:** same seed → same skim; conservation (store + belongings + eaten + spoiled unchanged across a skim); no post → no skim; reported ≠ true only when miscount fires.

### RW-P4 — Audit & Count (Core + host)
- Audit reveals the true figure to the player (half a survivor-day); Count is a weekly action; legitimacy delta table-driven, written only through the E15 API; a discrepancy raises a tribunal clue (E9).
- **Accept:** deltas bounded and data-driven; an honestly announced shortage costs less than a discovered one; no direct legitimacy mutation.

### RW-P5 — Hard Table events (Core + content)
- `hard_table_events.json` on the existing event path; gated by tier, resentment, `unexplained`; always precedes `desperation_events`; each event has a kind option.
- **Accept:** no event fires without its gate; desperation content unchanged; deterministic under a fork keyed `(day, eventId)`.

### RW-P6 — Hoarding hook (soft dependency on Shelter Governance)
- If a Hoarding statute exists, a discrepancy attributed to a survivor becomes a chargeable incident; otherwise the quartermaster's discretion (warning only).
- **Accept:** with no statute → warning-only path, no crash; with statute → uses only `JusticeSystem` public commands.

### RW-P7 — Feast & fast (Core + content)
- Celebration cost or Table Rule flag (per E14); voluntary austerity via `PolicySystem` if present.
- **Accept:** a feast changes stores and resentment exactly per data; no bypass of the policy owner.

### RW-P8 — Presentation
- Extend `KitchenNutritionPanel` (Book strip, Rule picker, Audit/Count). Focus/back preserved.
- **Accept:** presenter tests; panel holds no authority.

### RW-P9 — Content waves W1–W4 & governance close.

## 6. Acceptance criteria
1. Core authority + host owner + event path + persistence + observable outcome agree (CLAUDE.md "integrated").
2. Ship-dark parity: no rule, no post → `RationConflictSystem` outputs identical on a saved corpus.
3. Conservation: no skim, feast or event creates or destroys goods; identity holds.
4. Determinism: identical skim, events and Count deltas on replay (`CampaignStreamIds` fork, never `System.Random`).
5. Save round-trip mid-week with a rule change pending and a discrepancy open.
6. No writes to inventory, legitimacy, justice or celebrations except via public APIs.
7. Hard Table events never bypass or edit `desperation_events`.

## 7. Cross-plan boundaries
- **Shelter Governance:** Assembly consent for rule changes; Hoarding statute consumed only.
- **The Long Siege:** a *Siege Table* preset is a Table Rule + tier; the Siege decides when.
- **The Plague Year:** wards are a priority group; no new line.
- **The Record Keepers:** the Book is a record; medium and slant apply through their provenance, not through this plan.
- **The Quiet War:** no shared state.
- **Year Two:** outposts eat their own stores; supply is Year Two's.
- **The Deep Works:** a cold store may modify spoilage only through an existing hook (P0 decides).

## 8. Decision register (proposals — unsigned)
| ID | Decision | Type | Proposed |
|---|---|---|---|
| DEC-RW-01 | Ledger and post nest in the ration-conflict save DTO; no new section. | architecture | Yes; confirm in P0 |
| DEC-RW-02 | Expected-portion provider is an additive optional seam on `RationConflictSystem`; unset = today. | architecture | Yes |
| DEC-RW-03 | Ledger scope: full inventory if withdrawals carry reasons, else kitchen log + pantry row. | scope | P0 decides |
| DEC-RW-04 | Five Table Rules, changes take a day and carry a 3-day surcharge. | design | Yes |
| DEC-RW-05 | Hoard = real transfer to personal belongings; otherwise drop the stash mechanic. | rule | Yes; depends on E13 |
| DEC-RW-06 | Quartermaster is a duty-roster post, not a new role row. | architecture | Yes |
| DEC-RW-07 | Hard Table events always precede desperation events; desperation is never softened. | rule | Yes |
| DEC-RW-08 | No new routed panel; extend `KitchenNutritionPanel`. | UI | Yes |
| DEC-RW-09 | Hoarding law is consumed from Shelter Governance, never authored here. | boundary | Yes |
| DEC-RW-10 | Feast/fast ship only if celebrations can carry a cost (E14). | scope | P0 decides |

## 9. Pre-flight Checks
- [ ] Go config/save validator passes (`bin/validate-config` / `bin/ashfall-dev validate-config`)
- [ ] No equivalent existing system found (search `Quartermaster`, `Pantry`, `TableRule`, `Discrepancy`)
- [ ] Premise re-verified (Rule 7); ledger files re-read; no overlapping live claim on ration/kitchen/justice paths
- [ ] Signed decisions in hand

## 10. Verification
- [ ] `bin/run-scoped-tests` on new tests + existing ration-conflict, rationing, kitchen serving-log and justice tests (list from P0 selector)
- [ ] Ration-conflict host selftest (VERIFY args in `HostCli.RationConflict.cs`)
- [ ] Scoped only (<30 s each); ≤10–15 test-edit steps per failure, then auto-flag in `.ai/state.md`; **no full suite** without `RUN FULL TESTS`
- [ ] Handoff in `AI_AGENT_WORKFLOW.md` format; `.ai/state.md` updated

## 11. Stop conditions (Rule 10)
Stop and report if: withdrawals carry no reason **and** the fallback scope cannot balance; hoarding can only be implemented as a parallel stockpile; legitimacy has no public write API; the expected-portion seam would require changing tier or priority arithmetic; any path overlaps a live claim.

## 12. Open Mysteries & Deliberate Silence (lore register — no authority, no claimed path)

These questions are **intentionally unanswered** — not gaps, not TODOs, not deferred work. They
keep the pantry larger than the ledger that meters it. Any future plan that answers one must name
the signed decision that permits it.

| # | Question | Why it stays open | Who may answer it (later, signed) |
|---|---|---|---|
| RW-OM-1 | Who skimmed? | RW-P3 withdraws real units and RW-P2 reports a quantity, never a person. Attribution is a player verb (Audit, Count), never a system output. | Never — a rule, not a gap. |
| RW-OM-2 | Why do Critical/High/Standard all clamp to 1.0? | E3b is a live arithmetic fact the plan corrects *only when a rule is active*. The cause is left in place deliberately. | A balance pass that must say so in its register. |
| RW-OM-3 | What is the Book written in? | `pantry_book_lines.json` supplies lines; the medium is never authored. *The Record Keepers* owns custody and has not spoken. | The Record Keepers, if the Book is ever a Place. |
| RW-OM-4 | Where does a hoard go? | DEC-RW-05 makes it a real transfer to personal belongings *or drops the mechanic entirely*. If E13 fails, the question is unaskable. | Plan 210's owner, if belongings can hold consumables. |
| RW-OM-5 | Is a quartermaster trusted before they are appointed? | DEC-RW-06 makes it a duty-roster post, not a role row. The plan supplies no reputation and will not. | Never — texture by omission. |
| RW-OM-6 | Why are there exactly five Table Rules? | DEC-RW-04 authorises five and stops. The number is a scope decision that reads as a history of arguments. | Never — the artefact reads as canon. |
