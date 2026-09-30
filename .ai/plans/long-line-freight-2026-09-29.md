# Feature / Task Plan: The Long Line: Freight — run a caravan company

<!-- UPON INTEGRATION (MANDATORY):
Prepend header at the top saying:
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**
And immediately move this file to .ai/plans/integrated/<category>/
-->

STATUS: DRAFT — awaiting user approval (not self-approved; CLAUDE.md Rule 8 requires `STATUS: APPROVED BY USER` before any code commit)

> Prose companion: `docs/expansions/expansion_long_line_freight_plan.md`. Family index: `docs/expansions/expansion_world_moves_without_you_index.md`.
> Not a claim. Foreman adds the ledger entry and per-package claims. **Name collision with Expansion 11 "The Long Line" (telephone trunk) — DEC-LF-01 first.**

> **Editorial polish (prose pass):** sections **0**, **1b**, **1c** and **12** are narrative texture only. No
> authority, claimed path, decision, acceptance criterion or verification step changes. Sample lines
> are content candidates for `trade_ledger_lines.json` / `trade_wayside_events.json` rows; they belong
> in data, never in code.

---

## 0. Prologue — The Long Line

> *"A road is a rumour. A route is a rumour with a tariff and a season."*

There are ten routes in this world and all of them end. The season closes at Day 280 to 360 and
the company that thought it was permanent discovers it was always a **year**.

Freight is not trading. Trading is a decision; freight is a *commitment* — a manifest leaving with
a driver's name against it, a wagon whose wear is being spent somewhere you cannot see, cargo that
is neither here nor there for four to twelve days. The Long Line is the plan about that
meanwhile: the interval in which the shelter has already paid and has not yet learned.

**Tone & register.** Ledger-plain and road-weary. The vocabulary is the depot office: *manifest,
leg, tariff, wear, rung, charter, house mark*. Prose should be written like margin notes in a
freight book — terse, dated, occasionally honest. Never glamorise the road. The road is an
administrative fact with weather attached.

**Mystery & texture.** The company rungs — Contract → Charter → House → Line — are derived, never
bought (LF-P3). You cannot purchase a house name; you can only become one. And the season table is
the plan's hidden antagonist: a company that reads as permanent is simply a company that has not
yet met a year it could not close. §12 keeps the rest open.

**The second layer.** The ten days when nothing can be done are the plan's real subject. In that
interval the shelter owns nothing — not the wagon, not the cargo, not the driver — only the
*entry*, which is a promise somebody made before the leaving. Freight is the art of being
responsible for things you cannot reach, and the ledger line is the only instrument that makes
such responsibility feel like a fact. And because the entry outlives the wagon, the company is the
only thing on the road that can be trusted and the only thing that can betray you on paper.

**Note on naming.** Expansion 11 *The Long Line* is a telephone trunk — a proposal, unimplemented,
with no `Ashfall.Core.LongLine` and no `long_line` section registered (E12). The collision is real
and unresolved. Until DEC-LF-01 is decided, treat every occurrence of the name as provisional.
That provisional quality is not a defect; it is, for once, exactly how a road gets named.

## 1. Goal & Outcome

> *Design intent: the interesting part of a freight run is the ten days when nothing can be done.
> Everything in this plan is built to make that interval matter.*

- **Goal:** Make a trade-route run a resolved journey (legs, weather, war, crew, wagon, cargo) inside the *existing* route authority, and give the shelter a derived company rung (Contract → Charter → House → Line) with rolling stock, crews and depot nodes.
- **Outcome (observable):** on a fixed seed a Chartered route dispatches a run with a driver and wagon, resolves at arrival to `OnTime / Late / Failed` with cargo delivered short or whole, moves goods through the inventory owner, writes one ledger line, and survives save/load mid-run.
- **Non-Goals:** no second route/caravan/economy authority; no new save section; no new routed panel; no new routes in v1; no real-time convoy minigame; no change to contracts without a Charter (ship-dark); no merge with Expansion 11 without DEC-LF-01; no Unity.
- **"Done":** §6 acceptance passes via `bin/run-scoped-tests`; ship-dark parity holds; handoff lists untouched shared paths.

## 1b. Texture, Mystery & Voice

**Resolve at arrival (DEC-LF-02) is a narrative decision.**

The legs are summarised and only wayside events interrupt. That means the player learns the run's
story *after* it is over, from paperwork. Lean into this. The ledger line is the only surviving
witness and it is written by whoever is still employed.

**The House name cannot be bought.**

LF-P3 keeps in `company` only what cannot be recomputed: house name, mark id, wagon list, in-flight
runs, ledger tail. That is a portrait of an institution — five fields and a mark. Let the mark be
something a player would recognise at distance on a wagon door.

**What the player is never told.**

- Who is driving. `One driver + optional escort` (DEC-LF-05) is a role, and the run resolves without
  a voice. A killed driver is recorded through the death/legacy seam and is never narrated here.
- What the wayside is. `trade_wayside_events.json` supplies events; the wayside has no geography and
  must not be given any.
- Whether a rival house exists. DEC-LF-10 makes rivals **presentation only**, drawn over existing NPC
  arrivals. They may be real. The plan will not say.
- Why the season closes where it does. 280–360 is authored per route (E5) and per year (LF-P8). No
  calendar explanation is offered and none should be.

**Voice — sample fragments (content candidates for `trade_ledger_lines.json`).**

> "Run 14, On Time. Cargo whole. Nothing happened, which the ledger records as a success and I
> record as eleven days."

> "Run 15, Late. Weather on the second leg. The driver is fine. The driver is always fine in this
> book."

> "Charter granted. We now have a mark. We had a mark before; now people are willing to draw it."

> "Season ends in forty days. Nothing we own stops existing. It only stops belonging to this year."

**Design texture beats.**

- **Cargo conservation is the fiction's spine (§6.4).** Out − lost = delivered, and the player can
  check the arithmetic. Nothing makes a freight game honest like a balanced book.
- **Un-Chartered contracts keep the old path bit-for-bit (DEC-LF-07).** Ship-dark parity is also a
  tone promise: the plan does not impose itself.
- **One ledger line per resolved run (§6.6).** Never zero, never two. A run has exactly one
  sentence in the company's memory.
- **The season table is the antagonist.** Per-year `season_end_day` (DEC-LF-06) is the difference
  between a company that survives Year Two and one that quietly stops.

---

## 1c. The Deeper Layer — scenes, artifacts & held silences (second prose pass)

*(Second prose pass, non-contractual: narrative texture and writing guidance only — no authority,
no claimed path, no acceptance criterion, no verification step. §12's register is unchanged; the
fragments below are content candidates and deliberate silences, not new recorded questions. Where
a named data file holds no prose field, fragments are texture only and gain no schema.)*

**What the shelter leaves lying around.**

> "Manifest, run 15: one crate short at arrival. The crate is not on the ledger. The ledger's arithmetic is."

> "Wagon door, painted mark. The mark was painted before the Charter. The Charter changed who is willing to paint it."

> "Season table, Year Two: one route closes at 280. Somebody has written 'next year' beside it. 'Next year' is not a column."

**Scenes the player may piece together.**

> "The run resolves at arrival and the story arrives as paperwork. The driver's name is against the manifest and there is no other line about the driver anywhere in the book."

> "A rival house's wagon passes on the second leg. It may be real. The ledger declines to comment and so does the plan."

**Held silences (texture, not register rows).**

- Why the provisional name persists. DEC-LF-01 is unsigned; the road is named the way roads are named — provisionally, then permanently, without anyone deciding. Texture only.
- What the wayside implies about geography. `trade_wayside_events.json` supplies events and the wayside has no geography; the impression that it must lie somewhere is left standing on purpose.

**Fourth pass — the meanwhile (texture only; §12 register unchanged).**

*(Polish pass, non-contractual: wording and texture only — no authority, no claimed path, no
decision, no acceptance criterion, no verification step. §12 gains no row and loses no silence;
fragments remain content candidates for `trade_ledger_lines.json` / `trade_wayside_events.json`.
The name collision with Expansion 11 stands unresolved; DEC-LF-01 governs.)*

**The shape of the polish.** Freight prose is margin prose: terse, dated, occasionally honest. The
ten-day meanwhile is the plan's subject, so the writing should live in the tense of *already paid,
not yet known* — everything in the future perfect. The ledger line is the only surviving witness and
it is written by whoever is still employed, which is why every line is one sentence long.

**What the shelter leaves lying around.**

> "Ledger margin, run 12: 'eleven days.' The margin is the only place the book admits to time."

> "Charter seal, pressed once. The impression is in the ledger and the seal is on a shelf and the
> shelf is not in the book."

> "Waybill, second copy requested by nobody. The company keeps two of everything now. The second
> copy is the one that survives."

**Held silences (texture, not register rows).**

- Who is still employed to write the ledger line. Resolve-at-arrival paperwork is written by
  whoever remains (§1b); the hand is anonymous and must stay so. Texture only.
- Whether a wayside event happened to this run or to the road. The wayside has no geography (held
  silence above); the ledger declines to locate anything and must keep declining.

---

## 2. Evidence table (verified 2026-09-29; re-verify at P0 — Rule 7)

| # | Claim | Evidence | Status |
|---|---|---|---|
| E1 | `TickDay` debits tariff → `RecordRunOutcome(OnTime)`; insufficient funds → `Late`. No risk roll, no crew, no goods movement. | `src/Host/TradeRouteHostSession.cs` L198–232; `src/Main.TradeRoutes.cs` L46 | GAP |
| E2 | `GoodsOut`/`GoodsIn` have no consumer outside contract/system. | grep `Assets/Ashfall.Core`, `src` | GAP |
| E3 | `TradeRouteRiskBindingEngine.EvaluateTransitRisk` returns viability, raid probability, disruption risk, attrition, escort mitigation; not called by the tick. | `Economy/TradeRouteRiskBindingEngine.cs`; `TradeRouteHostSession.EvaluateContractRisk` | LIVE, unwired |
| E4 | Contract state: cadence, tariff, slots, exclusive good, reliability, runs, next run, suspend/cancel; save section `trade_routes`, store `trade_routes_save.json`. | `PlayerTradeRouteSystem.cs`, `TradeRouteContract.cs`, `Save/SaveSectionRegistry.cs` L313, L659 | LIVE |
| E5 | 10 routes, 4 regions (supply tags), travel 4–12 d, risk 90–340‰, **season ends Day 280–360**. | `caravan_trade_routes.json` | LIVE |
| E6 | Saturation constants (25% premium, 2.5%/unit, 5%/day recovery, ≤5 units/run, 3/slot). | `TradeRouteMonopolyEngine.cs` | LIVE (VERIFY tick wiring) |
| E7 | NPC caravans + disruptions persisted separately. | `CaravanTradeNetworkSystem.cs`; `TravelingCaravanSystem.cs` | LIVE |
| E8 | Vehicle seam: breakdown outcomes, vehicle state, `vehicles.json`, modules, armor grades. | `Assets/Ashfall.Core/ExpeditionVehicleSystem.cs` | LIVE (VERIFY wagon fit) |
| E9 | `FundsLedger.ReasonRouteTariff`, `TradeCreditCoordinator`, `LoanSharkEnforcerEngine`. | `Economy/` | LIVE |
| E10 | `MercenarySystem` exists; escort-hire contract type unconfirmed. | `Economy/MercenarySystem.cs` | VERIFY |
| E11 | Selftests: `--trade-route-selftest`, `--trade-routes-selftest`, `--trade-route-risk-selftest`, `--caravan-selftest`, `--traveling-caravan-selftest`. | `src/Host/HostCli*.cs` | LIVE (VERIFY args) |
| E12 | Expansion 11 "The Long Line" = telephone trunk, proposal only; no `Ashfall.Core.LongLine`; no `long_line` section registered. | `docs/expansions/expansion_11_the_long_line_creative_pack.md`; `EXPANSIONS_MASTER_CATALOG.md` L28, L134; grep registry = 0 | Name collision |

## 3. Authority table

| Concern | Owner (unchanged) | This plan adds |
|---|---|---|
| Contracts, tariffs, reliability | `PlayerTradeRouteSystem` (+ `TradeRouteHostSession`) | run-resolution step in the same tick path; additive nested `company` state |
| Run risk | `TradeRouteRiskBindingEngine` | wiring only (called, not rewritten) |
| Cargo stock | existing inventory owner | debit at departure, credit at arrival |
| Money | `FundsLedger` | new reasons only if absent (`ReasonRouteIncome`, VERIFY) |
| Vehicles | vehicle seam (E8) | wagon entries in same catalog family (DEC-LF-04) |
| Crew availability | duty roster / survivor assignment | consumer only |
| Deaths/injuries | survivor death-legacy / medical owners | consumer only |
| Depots | Outposts/Waystations (custody signed) | read-only nodes |
| NPC caravans/rivals | `CaravanTradeNetworkSystem` | read-only; rivals as presentation over existing arrivals |
| Save | `trade_routes` section | nested additive DTO, no new section |

## 4. Claimed Paths (proposed; `INT` = integrator-owned)
**Core:** `Assets/Ashfall.Core/Economy/TradeRunResolver.cs` (new, pure), `HouseCharter.cs` (new, derived rung), `TradeRouteContract.cs` + `PlayerTradeRouteSystem.cs` (additive nested state), `Economy/TradeRouteRiskBindingEngine.cs` (read-only consumer), `CatalogIntegrityValidator.cs` (`INT`), `Random/` stream id (`INT`)
**Data:** `caravan_trade_routes.json` (per-year season table, additive), `house_charter.json` (rungs, gates), `trade_wayside_events.json`, `trade_ledger_lines.json`, `vehicles.json` (wagon entries)
**Host:** `src/Host/TradeRouteHostSession.cs` (`INT`, tick), `src/Main.TradeRoutes.cs` (`INT`), existing trade-route screen surface
**Tests:** `Ashfall.Core.Tests/Economy/TradeRunResolverTests.cs`, `HouseCharterTests.cs`, `Ashfall.Core.Tests/Save/LongLineFreightSaveTests.cs`; extend existing trade-route tests

## 5. Packages

### LF-P0 — Premise audit & naming (Auditor)
- Re-verify E1–E12; resolve E8 (wagon fit) and E10 (escort contract); confirm whether tick is called once/day (`Main.TradeRoutes.cs`); foreman signs DEC-LF-01…10.
- **Accept:** each VERIFY row closed or a named blocker; DEC-LF-01 decided before any id is created.

### LF-P1 — Run resolver (Core, pure) — the central package
- Inputs: contract, manifest (cargo, crew, wagon, escort), risk-binding result, leg conditions (weather/embargo, regional tension, quarantine, road state). Output: `OnTime/Late/Failed`, cargo delivered, wear, casualties, wayside events, ledger line id.
- Determinism via a `CampaignStreamIds` fork keyed by `(day, routeId)`; no `System.Random`.
- **Accept:** table-driven tests; same inputs+seed → same output; resolves **once** at arrival; cargo conserved (out − lost = delivered).

### LF-P2 — Wire into the tick (Host, `INT`)
- `TradeRouteHostSession.TickDay`: contract without a Charter keeps today's path **bit-for-bit**; Chartered contract dispatches/resolves via LF-P1, moves cargo through the inventory owner, posts income/tariff to `FundsLedger`.
- **Accept:** ship-dark parity test (existing selftest outputs unchanged); an in-flight run survives save/load.

### LF-P3 — Company state & rung derivation (Core)
- `HouseCharter` derives rung from reliability tiers + owned wagons/crews + signed depot; nested `company` DTO holds only what cannot be recomputed (house name, mark id, wagon list, in-flight runs, ledger tail).
- **Accept:** round-trip identical; rung derivable from state; no new section.

### LF-P4 — Rolling stock & crews (Core + data)
- Wagon entries in the vehicle catalog family; wear per leg; repair as existing workshop job. One driver + optional escort per run in v1.
- **Accept:** a run without a driver does not leave; a killed driver is recorded through the existing death/legacy seam; wagon wear persists.

### LF-P5 — Depot nodes (Core, read-only)
- Outposts/waystations as depot nodes shorten leg risk/extend endurance; **no custody or save change**.
- **Accept:** removing a depot only changes risk, never state of the outpost; Year Two P6 tests still pass.

### LF-P6 — Market feedback & rivals
- Wire saturation into arrival; rivals are presentation over existing NPC arrivals; a stopped route emits a shortage signal that *The Living Region* (if present) can read.
- **Accept:** premium falls with saturation and recovers; no writes to caravan-network state.

### LF-P7 — Ledger surface (Presentation)
- Extend the existing trade-route screen: manifest preview (risk summary), crews/wagons, last ten ledger lines. Keyboard/controller focus preserved.
- **Accept:** presenter tests; no new gameplay authority in the panel (CLAUDE.md UI rule).

### LF-P8 — Season table per year (data)
- Per-year `season_end_day` so the company does not silently die at Day 360 (depends on Year Two P1 horizon lift; otherwise capped at 360 and documented).
- **Accept:** legacy years unchanged; Year Two years read new rows.

### LF-P9 — Content waves W1–W4 and governance close
- Per prose §7; validators + voice lock; archival only when integrator accepts.

## 6. Acceptance criteria
1. Core authority + host owner + event path + persistence + observable outcome agree (CLAUDE.md "integrated").
2. Ship-dark parity: no Charter → `--trade-route-selftest`, `--trade-routes-selftest` unchanged.
3. Determinism: identical run sequence on replay.
4. Cargo conservation (property test).
5. Save round-trip with a run in flight.
6. Every resolved run yields exactly one ledger line.

## 7. Cross-plan boundaries
- **Year Two P6 (The Road):** convoy supply is the *demand*; a Chartered house may be a *supplier* through a read-only offer seam; P6 does not depend on this plan.
- **The Living Region:** reads route blocked/short state; may supply tension; never writes.
- **The Plague Year:** quarantine is a leg condition; caravans are a *vector* seam owned by Plague Year.
- **The Drowned Coast:** water legs are a leg kind resolved by the Coast's voyage rules; the resolver receives them as conditions.

## 8. Decision register (proposals — unsigned)
| ID | Decision | Type | Proposed |
|---|---|---|---|
| DEC-LF-01 | Naming vs Expansion 11 (keep both with subtitles / rename this / merge). | governance | Decide before P1 |
| DEC-LF-02 | Resolve at arrival (summarised legs), stepping only on wayside events. | design | Yes |
| DEC-LF-03 | Nested `company` inside `trade_routes`, no new section. | architecture | Yes |
| DEC-LF-04 | Wagon = catalog entry in vehicle family vs separate catalog. | architecture | Decide in P0 (prefer same family) |
| DEC-LF-05 | One driver + optional escort per run (v1). | scope | Yes |
| DEC-LF-06 | Per-year `season_end_day` table. | data | Yes |
| DEC-LF-07 | Un-Chartered contracts keep the old path. | compatibility | Yes |
| DEC-LF-08 | No new routed panel. | UI | Yes |
| DEC-LF-09 | Ride-along runs (player plays a leg as an expedition) — **excluded from v1**. | scope | Defer |
| DEC-LF-10 | Rival houses are presentation only. | design | Yes |

## 9. Pre-flight Checks
- [ ] Go config/save validator passes (`bin/validate-config` / `bin/ashfall-dev validate-config`)
- [ ] No equivalent existing system found (search `TradeRun`, `HouseCharter`, `company`)
- [ ] Premise re-verified (Rule 7); ledger files re-read; no overlapping live claim (esp. any Expansion 11 claim)
- [ ] Signed decisions for the package in hand

## 10. Verification
- [ ] `bin/run-scoped-tests` on new tests + existing trade-route/caravan/monopoly/risk tests (list from P0 selector)
- [ ] `--trade-route-selftest`, `--trade-routes-selftest`, `--trade-route-risk-selftest`, `--caravan-selftest` (VERIFY args)
- [ ] Scoped only (<30 s each); ≤10–15 test-edit steps per failure, then auto-flag in `.ai/state.md`; **no full suite** without `RUN FULL TESTS`
- [ ] Handoff in `AI_AGENT_WORKFLOW.md` format; `.ai/state.md` updated

## 11. Stop conditions (Rule 10)
Stop and report if: DEC-LF-01 is unresolved and ids would collide; the resolver would need its own cargo store; the vehicle seam cannot represent a wagon without a parallel catalog; the tick change would alter un-Chartered behaviour; any path overlaps a live claim.

## 12. Open Mysteries & Deliberate Silence (lore register — no authority, no claimed path)

These questions are **intentionally unanswered** — not gaps, not TODOs, not deferred work. They
keep the road larger than the ten routes that meter it. Any future plan that answers one must name
the signed decision that permits it.

| # | Question | Why it stays open | Who may answer it (later, signed) |
|---|---|---|---|
| LF-OM-1 | Who named it The Long Line? | DEC-LF-01 leaves the collision with Expansion 11 unresolved. The name is provisional and the fiction declines to canonise a name the governance has not settled. | Foreman, when DEC-LF-01 is signed. |
| LF-OM-2 | What is the wayside? | `trade_wayside_events.json` supplies events with no geography. Giving the wayside a place would turn a leg into a map. | Never — DEC-LF-02's boundary. |
| LF-OM-3 | Do rival houses exist? | DEC-LF-10 makes rivals presentation only over existing NPC arrivals. Their reality is not asserted either way. | Never — a rule, not a gap. |
| LF-OM-4 | What happens to the wagon between legs? | Wear is applied per leg and repair is an existing workshop job. The in-between is unmodelled and must stay so. | Never — the abstraction is the design. |
| LF-OM-5 | Why does the season close between Day 280 and 360? | E5 authors it per route; LF-P8 authors it per year. No calendar explanation is offered anywhere. | Never — texture by omission. |
| LF-OM-6 | Is a house mark earned or inherited? | LF-P3 derives the rung and stores the mark. Whether the mark has a history is not modelled. | *The Record Keepers*, if a charter is ever archived. |
