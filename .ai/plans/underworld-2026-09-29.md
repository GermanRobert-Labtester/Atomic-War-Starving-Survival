# Feature / Task Plan: The Underworld — brokers, runs and hunters over the existing black-market, bounty and loan owners

<!-- UPON INTEGRATION (MANDATORY):
Prepend header at the top saying:
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**
And immediately move this file to .ai/plans/integrated/<category>/
-->

STATUS: DRAFT — awaiting user approval (not self-approved; CLAUDE.md Rule 8 requires `STATUS: APPROVED BY USER` before any code commit)

> Prose companion: `docs/expansions/expansion_the_underworld_plan.md`. Family index: `docs/expansions/expansion_new_pressures_and_places_index.md`.
> Not a claim. `BlackMarketSystem` (Plan 211), `FactionBountySystem`, `MercenarySystem`, `LoanSharkEnforcerEngine`, `BlackMarketHeatAttentionEngine` and the contraband engine keep their meaning. This plan **adds people (brokers), a small in-transit run record, and hunters that consume risk numbers nobody reads today**. It creates no second market, bounty, debt or heat authority. **No real-world crime instruction anywhere in data or text.**

> **Editorial polish (prose pass):** sections **0**, **1b** and **12** are narrative texture only. No
> authority, claimed path, decision, acceptance criterion or verification step changes. Sample lines
> are content candidates for `underworld_trail_lines.json` rows; they belong in data, never in code.
> Nothing in these additions may be read as real-world instruction — DEC-UW-10 governs.

---

## 0. Prologue — The Quiet Counter

> *"Nobody in this world trades goods. They trade the gap between what a thing is worth and what a
> person can bear to pay for it today."*

The market did not end when the bombs fell. It went *downstairs*. What the shelter calls the black
market is really three syndicates wearing one coat — the Quiet Counter, the Ash Market, the Cold
Ledger — and the difference between them is not what they sell but what they remember about you.

A broker is not a shopkeeper. A broker is a **person who knows what your name is worth in three
districts**, and who will lend you that knowledge at a cut. A run is not an expedition. A run is
a manifest leaving with somebody's cousin. And a hunter is not a soldier. A hunter is what a
number looks like when it finally stands up.

**Tone & register.** Dry, laconic, transactional. Everyone in this plan speaks in prices and
caveats. The prose should feel overheard rather than narrated — no moralising, no glamour, no
Robin Hood. The fiction is entirely invented: three fictional syndicates, fictional territories,
fictional currencies of trust. Nothing here is a method; it is a *cost table with a face*.

**Mystery & texture.** Four engines already run in the dark with no gameplay caller (E5, E6, E8).
That is the plan's real mystery: the machinery of consequence has been here all along, patient and
unread. Feeding it is not a feature addition — it is waking something up. Keep that framing in the
writing: the hunters were always possible. The player simply became legible.

## 1. Goal & Outcome

> *Design intent: the underworld should never feel like a menu of crimes. It should feel like
> borrowing against a reputation you did not know you had.*

- **Goal:** (a) **Brokers**: authored people bound to a syndicate, met through the existing contact-discovery call, with a visible cut and a standing; (b) **Runs**: a manifest leaves inventory through atomic billing, resolves on its due day against a read-only **Scrutiny** number, and pays or fails through existing settlement paths; (c) **Hunters**: an active mark plus risk from the enforcer and the live heat starts a four-leg trail (Word, Road, Door, Standoff) with pay, deal, hide, counter-bounty, lead-off, fight and hand-over answers; (d) a **read-only combined debt view** across the two loan ledgers.
- **Outcome (observable):** on a fixed seed a defaulted loan with an active mark and enforcer risk draws a hunter whose trail advances by leg each day; paying through the existing repay call clears the mark through the bounty owner's own resolve; a counter-bounty posts through `MercenarySystem.PostBounty` unchanged; a run's goods leave and return atomically (delivered, seized or burned) with survivors and items conserved; scrutiny reads embargo/patrol/war/heat and writes none; with no broker, run or hunter rows the black market, bounty and loan behaviour is identical to today; save/load mid-trail round-trips.
- **Non-Goals:** no real-world crime instruction; no change to premiums, heat, loan or bounty arithmetic; no merge of the two debt ledgers; no automatic combat; no new save section; no new routed panel; no Unity.
- **"Done":** §6 acceptance passes via `bin/run-scoped-tests`; ship-dark parity holds; handoff lists untouched shared paths.

## 1b. Texture, Mystery & Voice

**Brokers as people, not menus.**

`secret` and `temper` are the two fields that make a broker a character. Never resolve `secret` in
play. It is the reason a cut is 8% and not 30%, and the moment you narrate it, the number becomes
a quest reward. Let temper be observable — a broker who raises the cut after a bad week is doing
characterisation with arithmetic.

**The four legs of the trail.** Word, Road, Door, Standoff is a *sonata form*. Each leg should read
escalating in proximity: a rumour in the third person, a sighting at distance, a face at the
threshold, and finally a conversation with consequences. Never skip a leg — the dread lives in
the sequence, not the destination.

**What the player is never told.**

- Which of the three syndicates filed the mark. `FactionBountySystem` records a faction id; the
  trail never prints it.
- Whether the hunter was hired or is working a private grievance. Both are consistent with the
  numbers and the plan refuses to choose.
- What the Cold Ledger is *for*. A syndicate whose name describes its method is not obliged to
  describe its purpose.
- Whether the combined debt view is complete. It lists both ledgers. It does not promise those are
  all the ledgers.

**Voice — sample fragments (content candidates for `underworld_trail_lines.json`).**

> "Word: somebody is asking after the shelter's face. Not the shelter. The face."

> "Road: two sets of tracks on the east approach, one of them walking backwards for a while."

> "Door: he did not knock. He waited until the watch changed and then he was simply standing there."

> "Standoff: he asked for the man, not the goods. That is the part I keep turning over."

> "The Quiet Counter takes no interest. That is why their interest costs more."

**Design texture beats.**

- **Scrutiny writes nothing.** A number that only reads is a number the player can trust. Make the
  panel say so in one plain line.
- **A run's goods are conserved, always.** Delivered, seized or burned — never *gone*. The
  arithmetic is the fiction: if the player can add it up, they can forgive it.
- **The hunter targets the face, never a child (DEC-UW-08).** This is a tone rule with mechanical
  teeth. It also makes leadership feel like exposure, which is the correct feeling.
- **Counter-bounty is a mirror, not a fix.** Posting one should feel like answering a letter in the
  same handwriting.

---

## 2. Evidence table (verified 2026-09-29; re-verify at P0 — Rule 7)

| # | Claim | Evidence | Status |
|---|---|---|---|
| E1 | 3 syndicates (Quiet Counter, Ash Market, Cold Ledger), 7 stock entries, premiums, credit limits, access tiers. | `black_market_inventory.json` | LIVE |
| E2 | `BlackMarketSystem`: contact discovery, deterministic daily stock, previews, buy/sell, loans, repay, trust and heat; `OnDebtIssued/Repaid/Overdue/BountyPlaced`. | `Economy/BlackMarketSystem.cs` L163–187, L270, L298, L461–498, L563–707 | LIVE |
| E3 | On default: trust −30, heat +25, a patrol bounty through `FactionBountySystem`; daily heat decay by profile. | `BlackMarketSystem.cs` L707–753 | LIVE |
| E4 | Bounty records carry faction id, authored standing delta, severity, state, provenance; **no reader of active bounties** outside the owner. | `Factions/FactionBountySystem.cs` L16–75; grep | LIVE / GAP |
| E5 | Heat attention engine: bands, cooling, relocation, `GetRaidRiskPermille`, `CheckRaidTrigger`; ticked daily; **fed only from a self-test CLI** (`AddSyndicateHeat`); raid methods have **no gameplay caller**. | `Economy/BlackMarketHeatAttentionEngine.cs` L147–259; `src/Main.EconomyFamily.cs` L53; `src/Host/EconomyFamilyHostSession.cs` L71; `HostCli.EconomyFamily.cs` L58 | LIVE / GAP |
| E6 | Loan-shark engine: escalation stages, sanction check, enforcer raid risk/trigger; ticked daily; **`CheckEnforcerRaidTrigger` has no caller**; separate `loan_shark` save; a **second** debt ledger. | `Economy/LoanSharkEnforcerEngine.cs` L9–20, L238, L393–410; `src/Main.ExpandedShelterSystems.cs` L516; `SaveSectionRegistry.cs` L98 | LIVE / GAP |
| E7 | Mercenary contracts: templates (4), post/accept/claim, rival hunter id and progress; board generated with candidate targets. | `Economy/MercenarySystem.cs` L119–348; `src/Main.SubsystemComposition.cs` L511; `bounty_board.json` | LIVE |
| E8 | Contraband: classification enum, buy/sell/fence/escrow/payout action types, `ExecuteTransaction` (wrapped by the economy-family session; no gameplay caller); stash claims; barter contraband broker caravan. | `Economy/BlackMarketContrabandEngine.cs` L6–233; `Narrative/ContrabandStashSystem.cs`; `Economy/ShelterBarterSystem.cs` L523 | LIVE (VERIFY callers) |
| E9 | Embargo owner gates routes by weather (`IsRouteBlocked`, progress multipliers); no inspection. | `Economy/TradeEmbargoSystem.cs` L212–249 | LIVE |
| E10 | Expeditions carry no outbound cargo (outbound is travel ticks). | `Expeditions/ExpeditionSystem.cs` L686–694 | LIVE / GAP |
| E11 | `CrossingCatalog` names `faction_the_smugglers_court`. | `CrossingCatalog.cs` L287 | LIVE |
| E12 | Save owner `black_market` (+ `mercenary_bounties`, `contraband_stash`, `shelter_barter`, `loan_shark`). | `SaveSectionRegistry.cs` L72, L98, L195, L204, L205 | LIVE |
| E13 | Gate adapter for a hunter's arrival (QW design). | `docs/expansions/expansion_new_ways_to_play_index.md` §4 | PROPOSED (soft dependency) |
| E14 | Panels: `BlackMarketPanel`, `MercenaryBountyBoardPanel`. | `src/UI/` | LIVE |
| E15 | Where the shelter's public "face" (leader) is read. | `LeadershipSystem` | **VERIFY (P0)** |
| E16 | Public resolve/forgive calls on a bounty and repay calls on debts. | `FactionBountySystem.cs` L143–157; `BlackMarketSystem.cs` L653 | LIVE |

## 3. Authority table

| Concern | Owner (unchanged) | This plan adds |
|---|---|---|
| Syndicate stock, trust, heat, loans | `BlackMarketSystem` | broker standing derived from its ledger; one bridge feeding the attention engine |
| Bounties (marks) | `FactionBountySystem` | read of active marks; resolve through its own call |
| Contracts | `MercenarySystem` | counter-bounty through `PostBounty` |
| Loans (second ledger) | `LoanSharkEnforcerEngine` | read-only |
| Attention bands | `BlackMarketHeatAttentionEngine` | fed from the live heat by one bridge — **DEC-UW-09** |
| Embargo/war/patrol signals | their owners | read-only Scrutiny |
| Brokers, runs (in transit), hunters, trail | — | `UnderworldPeople` (pure Core), nested `brokers[]`, `runs[]`, `hunters[]` in the `black_market` save DTO — **DEC-UW-01** |

## 4. Claimed Paths (proposed; `INT` = integrator-owned)
**Core:** `Assets/Ashfall.Core/Economy/UnderworldBrokers.cs` (new, pure), `Economy/UnderworldRuns.cs` (new), `Economy/UnderworldHunters.cs` (new), `Economy/UnderworldScrutiny.cs` (new, read-only), `Economy/BlackMarketSystem.cs` (additive nested DTO fields only), `CatalogIntegrityValidator.cs` (`INT`), `Random/` stream id (`INT`)
**Data:** `underworld_brokers.json`, `underworld_hunters.json`, `underworld_run_manifests.json`, `underworld_trail_lines.json`, `underworld_scrutiny_weights.json`
**Host:** `src/Main.BlackMarket.cs` (`INT`), `src/Host/BlackMarketHostSession.cs` (`INT`), `src/Main.EconomyFamily.cs` (`INT`, heat bridge), day-owner registration (`src/Main.CampaignOwners.cs`, `INT`)
**Presentation:** extend `BlackMarketPanel` (brokers, runs, trail) and `MercenaryBountyBoardPanel` (counter-bounty) — **DEC-UW-06**; no new routed panel
**Tests:** `Ashfall.Core.Tests/Economy/UnderworldBrokersTests.cs`, `UnderworldRunsTests.cs`, `UnderworldHuntersTests.cs`, `UnderworldScrutinyTests.cs`, `Ashfall.Core.Tests/Save/UnderworldSaveTests.cs`; extend black-market, bounty and loan-shark tests

## 5. Packages

### UW-P0 — Premise audit (Auditor; read-only)
- Close E8 callers, E15: who (if anyone) calls the contraband engine and the two raid methods; whether the attention engine's heat can be bridged from the live heat without double-count; the leader read API; how two debt ledgers are surfaced today; gate adapter status; list every reader of `FactionBountySystem.State`; foreman signs DEC-UW-01…10.
- **Accept:** each VERIFY answered with `path:line` or a test; heat bridge shape chosen.

### UW-P1 — Brokers (Core + data, nested DTO)
- `Broker { brokerId, syndicateId, territory[], cutBp, temper, secret, standing }`; discovery hook on the existing contact call; nested additive `brokers[]`.
- **Accept:** round-trip; old saves load; standing derived from the syndicate trust ledger (never stored twice); no rows → no change.

### UW-P2 — Scrutiny (Core, read-only)
- Per-region number from embargo/route-blocked, faction patrol pressure, war, and the shelter's own heat via weights in data.
- **Accept:** table-driven; **no writes** to any source; deterministic.

### UW-P3 — Runs (Core + host)
- `Run { runId, brokerId, manifest[], fromRegion, toRegion, dueDay, runnerId, status }`; dispatch removes goods atomically into the in-transit record; resolve on the due day (seeded, `CampaignStreamIds` fork keyed `(day, runId)`): Clean / Stopped / Burned; payout/refund through existing settlement.
- **Accept:** conservation (items in = delivered + seized + returned); same seed → same outcome; ship dark with no manifests.

### UW-P4 — Hunters (Core + data)
- Authored archetypes (~6); trigger = active mark (E4) ∧ risk from enforcer (E6) and live heat (via P0 bridge); four-leg trail with legs advancing daily; target = the shelter's face (E15), never a child.
- **Accept:** seeded arrival; the previously unread methods gain exactly one consumer each; no hunter without a mark.

### UW-P5 — Hunter encounters & responses (host, soft gate adapter)
- Word (rumour/news line), Road (watch sighting or expedition encounter variant), Door (gate adapter), Standoff (bounded choice): pay, deal, hide, counter-bounty, lead-off, fight (encounter authority), hand-over.
- **Accept:** each response calls only its owner's public command; pay clears the mark through the bounty owner; fight is an offer, never automatic.

### UW-P6 — Combined debt view (Core, read-only)
- One projection listing debts from both ledgers by source label; no merge, no write.
- **Accept:** totals equal each ledger's own; a scripted default appears exactly once per source.

### UW-P7 — Heat bridge (Core + host)
- Feed the attention engine from the live per-syndicate heat by one bridge so bands and raid risk reflect play.
- **Accept:** attention heat is a pure function of the live heat; no new heat source; parity when the bridge is off.

### UW-P8 — Presentation
- Extend `BlackMarketPanel` (brokers, runs, trail, combined debt) and `MercenaryBountyBoardPanel` (counter-bounty). Focus/back preserved.
- **Accept:** presenter tests; panels hold no authority.

### UW-P9 — Content waves W1–W4 & governance close.

## 6. Acceptance criteria
1. Core authority + host owner + event path + persistence + observable outcome agree (CLAUDE.md "integrated").
2. Ship-dark parity: no broker/run/hunter rows → black market, bounty, loan and heat outputs identical on a saved corpus.
3. Conservation: run goods and payouts balance; no item or unit is created or lost outside owner transactions.
4. Determinism: identical trails, run outcomes and arrivals on replay (`CampaignStreamIds` fork; never `System.Random`).
5. Save round-trip mid-run and mid-trail.
6. No writes to bounty, debt, market or heat state except via owners' public APIs; Scrutiny writes nothing.
7. Hunters never target children; a leader read failure disables hunters rather than picking a survivor.

## 7. Cross-plan boundaries
- **The Quiet War:** gate adapter for the Door leg.
- **The Long Line: Freight:** a run may become a contraband route contract when it exists.
- **The Plague Year / The Living Region:** read-only signals and news grades.
- **Crews and Companions:** runs may be parties.
- **Radio Free Ashfall:** a broker's warning is a short broadcast.
- **Shelter Governance:** contraband law changes the shelter's own scrutiny.
- **The Sky:** impact salvage may be fenced.
- **Faith and Schism:** a claim may cover a hunter; no shared state.

## 8. Decision register (proposals — unsigned)
| ID | Decision | Type | Proposed |
|---|---|---|---|
| DEC-UW-01 | Brokers, runs and hunters nest in the `black_market` save DTO; no new section. | architecture | Yes; confirm in P0 |
| DEC-UW-02 | Hunters derive from existing marks and risk numbers; no second bounty authority. | rule | Yes |
| DEC-UW-03 | A run is an in-transit record (mirroring escrow/payout), not an expedition. | architecture | Yes |
| DEC-UW-04 | Two debt ledgers stay; a read-only combined view is added. | architecture | Yes |
| DEC-UW-05 | Scrutiny is read-only from existing owners. | rule | Yes |
| DEC-UW-06 | No new routed panel; extend the two existing panels. | UI | Yes |
| DEC-UW-07 | Counter-bounty uses `PostBounty` unchanged. | boundary | Yes |
| DEC-UW-08 | A hunter's target is the shelter's face (leader), never a child or a random survivor. | tone | Yes |
| DEC-UW-09 | The attention engine is fed from the live heat by one bridge; one heat, never two. | architecture | Yes; confirm in P0 |
| DEC-UW-10 | Fictional syndicates and no real crime instruction in text or data. | tone | Yes |

## 9. Pre-flight Checks
- [ ] Go config/save validator passes (`bin/validate-config` / `bin/ashfall-dev validate-config`)
- [ ] No equivalent existing system found (search `Broker`, `Hunter`, `Scrutiny`, `Run`)
- [ ] Premise re-verified (Rule 7); ledger files re-read; no overlapping live claim on economy/bounty/loan-shark paths
- [ ] Signed decisions in hand

## 10. Verification
- [ ] `bin/run-scoped-tests` on new tests + existing black-market, faction-bounty, mercenary, loan-shark and heat-attention tests (list from P0 selector)
- [ ] `BlackMarketSelfTest`, loan-shark and economy-family host selftests (VERIFY args)
- [ ] Scoped only (<30 s each); ≤10–15 test-edit steps per failure, then auto-flag in `.ai/state.md`; **no full suite** without `RUN FULL TESTS`
- [ ] Handoff in `AI_AGENT_WORKFLOW.md` format; `.ai/state.md` updated

## 11. Stop conditions (Rule 10)
Stop and report if: the attention engine cannot be fed without inventing a second heat; a hunter cannot be triggered without a new bounty authority; the shelter's face cannot be read; goods in transit cannot be conserved through owner transactions; any path overlaps a live claim.

## 12. Open Mysteries & Deliberate Silence (lore register — no authority, no claimed path)

These questions are **intentionally unanswered** — not gaps, not TODOs, not deferred work. They
keep the underworld beneath the level the player can audit. Any future plan that answers one must
name the signed decision that permits it.

| # | Question | Why it stays open | Who may answer it (later, signed) |
|---|---|---|---|
| UW-OM-1 | What is the Cold Ledger keeping books *of*? | The three syndicates are differentiated by method (E1). Purpose is deliberately unauthored so each campaign can read its own. | Never — texture by omission. |
| UW-OM-2 | Why does `faction_the_smugglers_court` exist in `CrossingCatalog` and nowhere else? | E11 is a real orphan. It is allowed to remain one; the coast is bigger than the coastlines we mapped. | *The Drowned Coast*, if a harbour ever needs a jurisdiction. |
| UW-OM-3 | Who taught the hunters the four-leg form? | Archetypes are authored (~6). Their training is not. Naming a school turns dread into lore. | Never — tone-locked by DEC-UW-10. |
| UW-OM-4 | Are there other debt ledgers besides the two? | DEC-UW-04 keeps two and adds a read-only view. The view is *labelled by source* — it does not assert totality. | Never — a rule, not a gap. |
| UW-OM-5 | Why was `CheckEnforcerRaidTrigger` written before anyone called it? | E6's uncalled methods read like a prepared consequence. The plan wakes them; it does not explain who prepared them. | Never — the artefact is the atmosphere. |
| UW-OM-6 | Does a broker ever *want* the player to default? | `temper` implies it; `secret` forbids proving it. Both readings survive. | Never — DEC-UW-01's boundary. |
