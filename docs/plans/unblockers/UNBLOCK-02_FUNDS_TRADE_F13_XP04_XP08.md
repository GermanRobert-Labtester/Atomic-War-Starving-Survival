# ASHFALL — UNBLOCK PROGRAM · PLAN 2
## Funds Authority and Trade Unblock: F13 / XP-04, Trade Legs, Trade Routes, Seasonal Migration

**Status:** planning deliverable only. Read-only pass. No production, data, test,
save, or governance-ledger file is modified by this document. No path is claimed.
**Date:** 2026-09-21
**Baseline verified at:** `Zcode_Branch`, HEAD `5be1a30a63cd86cf23e4034473b739ac514f0f2a`
(2026-09-20 02:03 +0300) plus the current uncommitted worktree.
**Role:** unblock-plan author. This plan turns the funds/trade decision cluster
into an execution-ready package that releases other plans. It implements nothing.
**Authority chain:** `AGENTS.md`, `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`,
`TEST_POLICY.md`, `KNOWN_DEBT.md`, `docs/governance/DECISION_REGISTER.md`
(DEC-02, DEC-05), `docs/plans/wave8_part2/C3_DECISION.md`, and current source.

---

## 0. How to read this plan

This is the second of five sibling unblocker plans produced on 2026-09-21:

| Sibling | Region | Primary releases |
|---|---|---|
| Plan 1 | Equipment/body schema | XP-06, EN-04, Expansion 16 |
| **Plan 2 (this)** | Funds, trade legs, trade routes | XP-04, XP-08, EN-03, C3 holds 192/199, expansions 17/25/26 |
| Plan 3 | Semantic kind, voice, string freeze | D11/CF-P3, Plan 42/46/49, EN-05, DEC-11/13, D19b |
| Plan 4 | Register truth, quarantine, census, residuals | D3/D4/D13/D16/D19a/c/D21, E1, Plan 24 residual, EN-08 |
| Plan 5 | Newest expansion waves, C3/EN gate | Expansions 12–31 intake, EN-01/02/06/07, XP-07/09/10 |

Vocabulary: **release** = a plan/debt/pillar that becomes claimable once the
signature is recorded; **blast radius** = every file the downstream execution is
expected to touch; **premise audit** = the Rule 7 re-check before the first edit.

---

## 1. Executive summary

### 1.1 The block in one paragraph

The XP expansion proposal names `XP-04-ECONOMY-LEGS` as decision-blocked on
`F13`: three separate foreman calls — (1) sign `FundsLedger` as the canonical
player-side funds authority, (2) sign the merchant-restock priority ordering
(or amend its weights), and (3) sign which surfaces opt into funds in wave 1.
Nothing named "FundsLedger" exists anywhere in Core or host source today; trade
therefore still clears goods-for-goods through `HoldfastTradeSession` and
`BlackMarketSettlementService`. Because `XP-08-TRADE-MIGRATION` explicitly binds
its route tariffs to the XP-04 `FundsLedger`, and because mundane trade legs are
the precondition for the black-market contracts and seasonal migration in the
same pillar, one unsigned funds decision holds an entire chain: XP-04 → XP-08 →
C3 holds 192 (player trade routes) and 199 (seasonal human migration) → `EN-03
Underground Economy Pressure` authorization → several Wave 2/3/4 expansions that
were deliberately designed *around* the blocked economy but would benefit from
its resolution. The restock sub-decision is nearly free because DEC-05 already
signed and shipped an ordering rule; what remains is a *ledger reconciliation*
and one optional weight amendment, not new code.

### 1.2 The two-layer truth that makes this plan cheap

The queue today contains two different statements about restock:

1. `DEC-05` (2026-09-17): **SIGNED** — "Priority-weighted tier restock with
   deterministic PRNG; trade ledger updates canonically." Live in
   `ShelterBarterSystem.ComputeItemPriorityScore` (line 283) and
   `GetPrioritizedStock` (line 299), consumed by the arrival restock and the
   barter panel. `Plan147RestockPriorityTests` passes 6/6.
2. The XP-04 packet (`F13`) asks the foreman to sign a **richer** restock
   design (per-category weights, scarcity floors, largest-remainder allocation)
   that is *not* what DEC-05 signed and is *not* live.

These are not contradictory; they are scopes. DEC-05's Option C is "ordering
only; quantities and inclusion unchanged". The XP-04 design changes allocation
**quantities** when capacity is partial — a real gameplay change. This plan
therefore splits F13 into F13-A (funds authority — the true blocker), F13-B
(restock ledger reconciliation — zero code, and the XP-04 richer design becomes
an optional amendment F13-C), and F13-D (opt-in surface list). Only F13-A and
F13-D are required to release XP-04; XP-08 requires F13-A; `EN-03` requires
F13-A/D and is authorized by Plan 5.

### 1.3 Signature bundle in one glance

| # | Sign-off line | Releases | Blast radius |
|---|---|---|---|
| F13-A | `I sign FundsLedger (integer chits) as the canonical player-side funds authority; goods-clearing surfaces may opt in via catalog flag accepts_funds; no parallel wallet, item, or save section.` | XP-04 core, XP-08 tariffs, EN-03 gate | new Core `Economy/FundsLedger.cs`, `SaveSectionRegistry`, host sessions, panels |
| F13-B | `I ratify DEC-05 as the live restock rule and record the XP-04 richer allocation as an optional amendment, not a replacement.` | ledger truth; no code | `INTEGRATION_PLANS.md`, `DECISION_REGISTER.md`, `KNOWN_DEBT.md` wording |
| F13-C (optional) | `I sign the XP-04 partial-capacity restock allocation (weights + scarcity floors + largest-remainder) as an amendment executed after F13-A.` | economy depth | `ShelterBarterSystem`, `merchant` catalogs, tests |
| F13-D | `Wave-1 funds opt-in surfaces: black market + holdfast trade only (caravan routes arrive with XP-08).` | XP-04 wave 1 scope, XP-08 planning | opt-in flags in three catalogs, panels |
| F13-E | `I sign XP-08 Plan 192 scope: routes as scheduled caravans with reliability tiers; no new logistics layer; standing/raid/save seams named.` | C3 hold 192 lift, XP-08-F1..F4 | `trade_routes.json`, caravan host, save owner |
| F13-F | `I sign XP-08 Plan 199 scope: seasonal human migration as population-weight deltas on a deterministic daily tick; no NPC agents; humans never reuse wildlife state.` | C3 hold 199 lift, XP-08-F5/F6 | migration catalog, day owner, map/economy read-models |
| F13-G | `I sign the tier-4 exclusive-goods list discipline: one exclusive good id per route, no real-world brands, no strictly-superior goods.` | XP-08 tier 4 | `trade_routes.json` rows, validator |

### 1.4 What is NOT in this plan

- No new currency item id. Funds are an integer ledger, not an inventory item.
  A "chit item" would create a second inventory authority and a pickup economy
  the game does not need.
- No bank, interest, loans beyond the already-sealed black-market loan model,
  no stock market, no player-to-player trade.
- No change to `MarketSystem` pricing math beyond adding an explicit
  "settled in funds" leg where a surface opts in.
- No replacement of `HoldfastTradeSession.TotalValue`; the funds leg is additive
  at settlement time, and goods-only trade remains byte-identical when no
  surface opts in.
- No second debt or bounty ledger. Unpaid contracts route to the sealed
  debt/bounty owners.
- No agent simulation for migration; population weights only.
- No relocation of the sealed `BlackMarketSystem` heat mechanics; this plan
  defines the funds legs and lets heat be implemented by the XP-04 phase that
  already has a design.

---

## 2. Verified current reality

### 2.1 There is no funds authority

```
$ grep -rn "FundsLedger" --include=*.cs Assets/Ashfall.Core src
(no matches)
```

The economy's value flows today:

| Surface | Settlement today | Value unit |
|---|---|---|
| `HoldfastTradeSession` | goods bill validation + `TryConsumeBill`; `HoldfastTradeResult.TotalValue` (int) | derived value, goods side only |
| `BlackMarketSettlementService` | canonical-inventory settlement; `SettlementUnits` (long) in preview/result | derived value, goods side only |
| `ShelterBarterSystem` | barter/restock; priority ordering via DEC-05 | goods side only |
| `MarketSystem` | buy/sell price computation, shocks, category indices | price math, not player funds |
| Quests/rewards | item grants through canonical inventory | goods |

`HoldfastTradeSession.TotalValue` and `BlackMarketSettlementService.SettlementUnits`
prove the codebase already computes value-differences for a trade; what is
missing is a single **player-owned balance** that credit/debit movements mutate
atomically. That is the whole F13-A.

### 2.2 Restock is live and signed; the XP-04 design is a different scope

- `Assets/Ashfall.Core/Economy/ShelterBarterSystem.cs:283` —
  `public static int ComputeItemPriorityScore(CaravanStockItem item,
  MerchantCaravanDef def, Func<string, int>? customScorer = null)`.
- `:299` — `GetPrioritizedStock(MerchantCaravanDef def)`: sorts by score
  descending, ties by `item_id` ordinal.
- `:319` — the arrival restock consumes the prioritized list with the Plan 147
  per-arrival day gate; stock pins for the whole stay; reopening never rerolls.
- `DEC-05` is `SIGNED`; evidence field now says 6/6 with a corrected count note.
- The XP-04 packet's `restock_capacity_per_period`, `priority_categories`,
  `scarcity_floor`, and largest-remainder allocation are **not implemented**:
  grep finds no such keys in any catalog or Core file.

### 2.3 The value vocabulary already exists in two spellings

`BlackMarketSettlementService` uses `SettlementUnits`; XP-04 proposes "chits".
This plan standardizes the *ledger* name on chits but keeps settlement-unit
conversion explicit at the service boundary (one conversion function, tested).
No surface may print both units to the player without a labeled conversion.

### 2.4 The XP-08 prerequisites are exactly the C3 hold conditions

`docs/plans/wave8_part2/C3_DECISION.md`:

- **192 HOLD** — "Recheck condition: a signed player-route DTO with
  standing/raid/save seams named in a map amendment." XP-08-F1..F4 is that DTO
  plus its contract model. F13-E signs it.
- **199 HOLD** — "Recheck condition: product names a human population owner
  distinct from fauna." XP-08-F5/F6 names population weights as the owner and
  explicitly forbids reusing wildlife migration state. F13-F signs it.

Neither hold can be lifted by code evidence alone; each needs the product
signature this plan bundles. The XP-08 proposal itself says the tariffs bind to
the XP-04 `FundsLedger` — so F13-A must precede or accompany F13-E.

### 2.5 The expansion waves were designed around the block — and will benefit

Wave 2's index states the five plans were "chosen to avoid overlap with … the
XP-04/EN-03 economy legs". This is good discipline and must be preserved: the
expansions do **not** require funds. But three of them touch adjacent seams and
should be enriched at intake:

| Expansion | Adjacent seam | Intake note |
|---|---|---|
| 17 · The Long Evening | culture/leisure has no currency; its index says "no new currency" | confirm F13 does not leak into leisure economy; festivals may consume goods, not chits |
| 25 · The Iron Road | rail freight, schedules, rail towns | freight contracts are *not* player trade routes; keep separate from XP-08 F1; if a freight tariff is ever needed, it uses FundsLedger, not a second ledger |
| 26 · The Common Table | menus, ration policies, food culture | rationing stays in `ResourceRationingSystem`; food never becomes a currency |

None of the three gains a hard dependency on this plan; each gains a clarification.

### 2.6 Black-market legs today

- `BlackMarketSystem` (Core) owns contact discovery, daily stock snapshots,
  pricing, buy/sell preflight, loans, trust/heat decay.
- `BlackMarketSettlementService` (Core) implements immediate canonical-inventory
  settlement for buy/sell/take-loan/repay (signed DEC-02).
- Actions exist at the host/panel layer (Wave 8 Part 2 C1 sealed).
- What is missing per XP-04: funds legs (buy pays chits, sell receives chits),
  contract escrow, heat band effects, purity tiers on fenced goods. The
  canonical-inventory settlement is the *existing* behavior and must remain the
  default when funds do not opt in.

### 2.7 Caravan/trade-route owners today

| File | Role | Player-route? |
|---|---|---|
| `Assets/Ashfall.Core/Economy/CaravanTradeRouteCatalog.cs` | authored NPC routes | NPC |
| `Assets/Ashfall.Core/Economy/CaravanTradeNetworkSystem.cs` | network tick, `travel_days` overlay | NPC |
| `Assets/Ashfall.Core/Economy/TravelingCaravanSystem.cs` | caravan movement/availability | NPC |
| `Assets/Ashfall.Core/Economy/CaravanAtomicTrader.cs` | caravan trade behavior | NPC |
| `Assets/Ashfall.Core/Economy/ShelterBarterSystem.cs` | shelter-side barter/restock | shelter |

No player-route authority exists; `trade_route_disrupted` is an orphaned key
(C3 evidence). XP-08's contract model attaches player routes to the *existing*
caravan dispatch rather than building a logistics layer, which is consistent
with the one-authority rule.

### 2.8 Debt, bounty, and standing owners (must not be duplicated)

- `FactionBountySystem` — canonical bounty authority (used by black market).
- `LedgerDebtSystem` — canonical debt authority.
- faction standing engine (`FactionStanceEngine`/`FactionWarSystem` standing
  path) — route tier gates and migration friction read it, never write it.

### 2.9 Determinism and save constraints

- Black-market stock uses a per-day fork of `CampaignRngManager`; save/restore
  never rerolls (R4 guard tested).
- `SaveSectionRegistry` is the single save-section authority; new sections are
  added by the integrator seam.
- `SaveChecksum` culture-invariant formatting; integer funds avoid float drift.
- Migration rule for a new `funds_ledger` section: old saves get an empty
  ledger at zero balance, never a fabricated starting balance unless the
  campaign-difficulty starting grant authorizes it (XP-01 owns starting bonuses;
  this plan does not).

### 2.10 Scale of the block

| Measure | Value |
|---|---|
| Pillars directly blocked | XP-04, XP-08 |
| Pillars indirectly sequenced | EN-03 (gate), XP-07 (README sequence note; no true dependency) |
| C3 holds lifted by the XP-08 signatures | 192, 199 |
| Expansions clarified | 17, 25, 26 (no hard dependency) |
| Decision lines req. to release XP-04 | F13-A, F13-D |
| Decision lines req. to release XP-08 | + F13-E, F13-F (and F13-A) |
| Code already existing | restock ordering (DEC-05), settlement service, caravan owners |
| Est. execution | 6–9 builder-days for F13-A/D; +4–6 for XP-08 F1..F4; +4–6 for F5/F6 |

### 2.11 Restock ledger drift to reconcile (F13-B, zero code)

`docs/plans/UNBLOCKED_PLANS_AUDIT_2026-09-19.md` §7 found:

1. `INTEGRATION_PLANS.md` still describes merchant restock priority as "Still
   deferred with authority question" while DEC-05 is SIGNED and live.
2. DEC-05's evidence originally said 14/14; corrected to 6/6 on 2026-09-19.
3. `CF-P5-RESTOCK-RECONCILE` is itself listed as an available, unexecuted plan
   (program Plan 03) whose whole scope is this ledger reconciliation.

F13-B can close the CF-P5 package as part of this bundle's Phase 0 — the
reconciliation is wording-only and its verification is the existing 6/6 test.

---

## 3. Blocked-plan inventory released by this plan

### 3.1 `XP-04-ECONOMY-LEGS` (the primary release)

| Feature | Blocked by | Released by | Output |
|---|---|---|---|
| F1 `FundsLedger` | F13-A unsigned | F13-A | `FundsLedger` Core + save section + tests |
| F2 black-market trade legs | F13-A + F13-D | F13-A/D | buy/sell/fence/contract legs bound to ledger |
| F3 restock priority | **already signed as DEC-05** | F13-B (+ optional F13-C) | ledger truth; optional richer allocation |
| F4 heat/attention | design defined; not decision-blocked once F2 exists | F13-A/D execution | heat bands + relocation anti-reroll |
| F5 purity tiers | design defined; does not strictly need funds | F13-A execution (with F2) | tier rolls persisted per instance |
| F6 funds-denominated debt | F2/F1 | F13-A execution | unpaid escrow → sealed debt owner |

### 3.2 `XP-08-TRADE-MIGRATION`

- **F1–F4 (routes, economics, risk binding, reliability tiers)** — released by
  F13-A + F13-E. Requires the route DTO signature that also satisfies C3 192.
- **F5–F6 (seasonal migration, consequences)** — released by F13-F. Satisfies
  C3 199.
- **Risk binding (F3)** additionally consumes the sealed Plan 32 graph-travel
  work (`WastelandMapSystem.PlanRoute`, Unknown-fog gate) — already sealed, so
  no new dependency.
- **Acceptance** mirrors XP-08 §5: `TradeRouteContractTests`,
  `RouteReliabilityTests`, `RouteRiskBindingTests`, `SeasonalMigrationTests`,
  `MigrationConsequenceTests`, reload-replay equality.

### 3.3 `EN-03 Underground Economy Pressure`

- Hard-blocked on F13 (per the 2026-09-19 audit).
- Released by F13-A/D; its authorization line is signed in Plan 5 only after
  the funds ledger executes, because the audit explicitly warns that
  authorizing an EN ahead of its bundle recreates the blocker one layer up.
- Enrichment: EN-03's tier function must read FundsLedger balance movements and
  heat; it may not invent a second scarcity or attention model.

### 3.4 C3 HOLDs

| Hold | Condition | Satisfied by | Consequence |
|---|---|---|---|
| 192 | signed player-route DTO with standing/raid/save seams | F13-E (route contract + tier gates + save owner named) | HOLD → released; `trade_route_disrupted` gains a producer |
| 199 | human population owner distinct from fauna | F13-F (population-weight owner named) | HOLD → released; no wildlife state reuse |

### 3.5 Sequencing notes for other XP pillars

- **XP-07 (Plan 190 item provenance)** — the 2026-09-19 audit lists it as
  "sequenced after XP-04" for release-train order, not because it reads funds.
  Its own decisions (F1–F3) are independent and belong to Plan 5's premise
  checks. This plan notes the ordering so the release train stays coherent but
  claims no release of XP-07.
- **XP-09/XP-10** — premise-check against RETIRED rows DEC-17/DEC-18 (Plan 5).
- **XP-05 (fuel)** — already live per the W1 premise correction; verify-and-record
  only.

### 3.6 Secondary releases

| Item | Effect |
|---|---|
| `CF-P5-RESTOCK-RECONCILE` | closable in Phase 0 (wording-only; test evidence already green) |
| `trade_route_disrupted` orphan | gains a producer through XP-08; content-utilization improves |
| Expansion 17/25/26 intake | clarified boundaries (no currency leak, freight ≠ player routes, food ≠ currency) |
| `MerchantCaravanDef` partial-capacity behavior | F13-C optionally makes it data-driven and testable |

### 3.7 Non-releases (explicitly not claimed)

- No new currency item.
- No player-to-player trade, auction house, or banking.
- No second debt/bounty/standing authority.
- No NPC-agent migration simulation.
- No re-litigation of DEC-02 (black-market settlement) or DEC-05 (restock);
  both are SIGNED and stay.

---

## 4. Decision packet

### 4.1 F13-A — FundsLedger authority

**Sign-off line:**

> `I sign FundsLedger (integer chits) as the canonical player-side funds authority; goods-clearing surfaces may opt in via catalog flag accepts_funds; no parallel wallet, item, or save section.`

**Normative shape (Core, engine-free):**

```csharp
public sealed class FundsLedger
{
    public int Balance { get; }                       // integer chits; never float
    public FundsResult TryDebit(int amount, string reasonKey, string sourceId);
    public FundsResult TryCredit(int amount, string reasonKey, string sourceId);
    // bounded append-only movement log (128), same discipline as MedicalRecordLog:
    // day + delta + reasonKey + counterpartyId, never free-text
}
```

**Rules:**

1. Integer only. No float formatting in checksum paths.
2. One writer: the ledger itself, through the two methods. Panels call commands.
3. No negative balance; a debit that would overdraw fails typed
   (`InsufficientFunds`), never partially applies.
4. Movement log bounded at 128 entries, oldest-first eviction, campaign-scoped.
5. `funds_ledger` save section owned by the economy save group; old saves get
   balance 0 and empty log.
6. No RNG in any funds path.
7. Conversion at settlement boundaries only:
   `settlement_units → chits` is one pure function, tested once.

**Options considered:**

| Option | Verdict |
|---|---|
| A. Integer ledger, opt-in surfaces (recommended) | smallest, no inventory churn, matches sealed precedents |
| B. A currency item (`item_chit`) | rejected: creates a second inventory authority, pickup UI, weight/capacity interactions, and a new exploit surface |
| C. Per-faction wallets | rejected: multiplies state and makes cross-faction trade need conversion tables |
| D. No funds ever; goods-only forever | valid decline; XP-04 dies, XP-08 loses tariffs and its contract model, EN-03 stays gated |

**Blast radius:** new `Assets/Ashfall.Core/Economy/FundsLedger.cs`;
`SaveSectionRegistry`; economy save/store plumbing; opt-in flags in
`black_market_inventory.json` / merchant catalogs; host sessions; panels;
tests; generated architecture/save matrices.

### 4.2 F13-B — restock ledger reconciliation (no code)

**Sign-off line:**

> `I ratify DEC-05 as the live restock rule and record the XP-04 richer allocation as an optional amendment, not a replacement.`

**Effect:** closes `CF-P5-RESTOCK-RECONCILE` by correcting
`INTEGRATION_PLANS.md`'s stale restock sentence, confirming DEC-05's 6/6
evidence, and marking the XP-04 `restock_capacity_per_period` design as
"amendment F13-C — not required for F13-A". No production change. Verification:
`bash scripts/run_test.sh Ashfall.Core.Tests/Economy/Plan147RestockPriorityTests.cs`
(6/6) plus the binding/call-site grep the CF-P5 plan already names.

### 4.3 F13-C — optional richer restock allocation

**Sign-off line:**

> `I sign the XP-04 partial-capacity restock allocation (weights + scarcity floors + largest-remainder) as an amendment executed after F13-A.`

**Scope:** only when restock capacity is partial does allocation order become
allocation *quantity*. The algorithm is deterministic integer math:

1. effective weight = `weight × (stock < scarcity_floor ? 2 : 1)`;
2. allocate capacity proportional to effective weights with largest-remainder
   rounding;
3. within a category, lowest `stock/target_par` first;
4. tie-break authored `restock_order`, then item id ordinal.

**Decline consequence:** partial restocks keep DEC-05 ordering-only behavior;
nothing in XP-04/XP-08 fails.

### 4.4 F13-D — wave-1 opt-in surfaces

**Sign-off line:**

> `Wave-1 funds opt-in surfaces: black market + holdfast trade only (caravan routes arrive with XP-08).`

**Rationale:** two surfaces prove the ledger without touching the caravan
network's sealed travel semantics. Caravan/route funds arrive with XP-08
(F13-E/F), where the contract model defines when a tariff is paid.

**Decline alternative:** "black market only" also works; "all surfaces at once"
is discouraged because it makes the funds legs and the route contracts land in
one unreviewable tranche.

### 4.5 F13-E — XP-08 Plan 192 route-contract signature

**Sign-off line:**

> `I sign XP-08 Plan 192 scope: routes as scheduled caravans with reliability tiers; no new logistics layer; standing/raid/save seams named.`

**Named seams (this is what the C3 recheck demands):**

| Seam | Name |
|---|---|
| standing | `FactionStanceEngine` standing read for tier gates; route never writes standing directly |
| raid | route runs use the existing caravan dispatch/encounter path; a raid outcome is a caravan outcome, not a route-owned combat |
| save | route instances persist additively inside the existing caravan/economy save owner; no new save section unless the integrator prefers a named `trade_routes` section — the decision must name exactly one |
| scheduling | the caravan day-owner schedules runs; no second logistics tick |

**Contract model (normative, from XP-08 §3):** authored templates in
`trade_routes.json` + runtime instances; cadence, tariff in chits, goods legs,
`caravan_slots_required`, reliability tiers 1–4 with the XP-08 thresholds
(5/12/20 successful runs, tariff −25% at tier 3, one exclusive good at tier 4),
reliability +1 on-time / −2 failed, floor 0, 30-day cooldown after cancel.

### 4.6 F13-F — XP-08 Plan 199 migration signature

**Sign-off line:**

> `I sign XP-08 Plan 199 scope: seasonal human migration as population-weight deltas on a deterministic daily tick; no NPC agents; humans never reuse wildlife state.`

**Named owner:** a human-population-weight read/write model inside the existing
economy/faction aggregate that already tracks regional demand, applied by the
existing campaign day-owner sequence. It is explicitly **not**
`WildlifeEcosystemSystem` and must not write wildlife state. Consequences:
market availability multipliers, labor candidate pool scaling, friction event
weights, caravan escort demand — all reads or existing-owner writes.

**Determinism:** authored per-faction schedule of deltas per region; daily tick;
10-day hysteresis per season phase; no RNG; paired-replay equality tested.

### 4.7 F13-G — tier-4 exclusive goods discipline

**Sign-off line:**

> `I sign the tier-4 exclusive-goods list discipline: one exclusive good id per route, no real-world brands, no strictly-superior goods.`

**Rules:** one exclusive id per route; the id must be an existing authored good;
the exclusivity is a market-availability flag, not a new item; the good may not
be strictly superior to a crafted equivalent (the named-item balance rule). The
list is authored in `trade_routes.json` and validated.

### 4.8 Signing order and first safe step

1. Sign F13-A, F13-B, F13-D together — this releases XP-04 core + EN-03 gate.
2. Optionally sign F13-C (restock depth) — independent.
3. Execute F13-A/D phases (ledger → black-market legs → holdfast legs).
4. Sign F13-E/F/G with the XP-08 phases; these lift C3 192/199.
5. Record in `DECISION_REGISTER.md` as new rows (proposed DEC-25..DEC-30) and
   update the C3 dispositions.

**First safe step:** Phase 0 (reconciliation + premise re-verification). No
production edit before F13-A signs.---

## 5. Technical design (execution-ready after signature)

### 5.1 FundsLedger — full contract

**Location proposal:** `Assets/Ashfall.Core/Economy/FundsLedger.cs` (engine-free,
netstandard2.1, no Godot).

**Public surface:**

```csharp
public enum FundsFailure
{
    None = 0,
    InsufficientFunds = 1,
    InvalidAmount = 2,
    UnknownReasonKey = 3,
}

public readonly struct FundsResult
{
    public bool Success { get; }
    public FundsFailure Failure { get; }
    public int NewBalance { get; }
    public string ReasonKey { get; }
}

public sealed class FundsMovement
{
    public int Day { get; }
    public int Delta { get; }
    public string ReasonKey { get; }
    public string CounterpartyId { get; }
}

public sealed class FundsLedger
{
    public const int MaxMovements = 128;
    public FundsLedger(int startingBalance = 0);
    public int Balance { get; }
    public IReadOnlyList<FundsMovement> Movements { get; }   // newest last

    public FundsResult TryDebit(int amount, string reasonKey, string counterpartyId, int day);
    public FundsResult TryCredit(int amount, string reasonKey, string counterpartyId, int day);

    // persistence
    public FundsLedgerSaveState Capture();
    public static FundsLedger Restore(FundsLedgerSaveState? state);
}

public sealed class FundsLedgerSaveState
{
    public int schemaVersion { get; set; } = 1;
    public int Balance { get; set; }
    public List<FundsMovementSave> Movements { get; set; } = new();
}
```

**Reason keys (closed vocabulary, authored in code as constants):**

| Key | Meaning |
|---|---|
| `bm_buy` | black-market purchase debit |
| `bm_sell` | black-market sale credit |
| `bm_fence` | fencing fee debit (or net credit when fee taken) |
| `bm_contract_escrow` | contract escrow debit |
| `bm_contract_payout` | contract completion credit |
| `bm_contract_refund` | escrow refund credit |
| `holdfast_buy` / `holdfast_sell` | holdfast trade legs |
| `holdfast_appraisal` | optional appraisal fee |
| `route_tariff` | XP-08 route tariff debit |
| `route_proceeds` | XP-08 route goods-sale credit |
| `quest_reward` | reward credit where a quest authors funds |
| `debt_settlement` | debt payment debit/credit through the sealed debt owner |

The vocabulary is **closed by default**: an unknown key is a typed failure, and
adding a key is a one-line change with a test — not an open string. This keeps
the movement log analytically usable and prevents free-text leakage into saves
(the MedicalRecordLog discipline).

**Balance rules:**

- `amount > 0` required; `amount == 0` fails `InvalidAmount` (a zero movement is
  a bug, not a no-op — tests assert it).
- Debit fails when `Balance < amount`; no partial application.
- Credit has no cap; the balance is bounded only by int arithmetic and authored
  content. Overflow guard: fail `InvalidAmount` if `Balance > int.MaxValue -
  amount` (tested with an explicit near-max case).
- `TryDebit`/`TryCredit` are the only mutators; `Balance` has no setter.

**Movement log:**

- Bound 128. On append beyond the bound, evict the oldest.
- Serialized oldest-first; restore is order-preserving but tolerant of a
  truncated/absent list (old saves: empty).
- Movements are facts; presentation may format them, never reinterpret them.

**Determinism:**

- No RNG; day comes from the existing campaign day source.
- Checksum: integer fields only; culture-invariant by construction.
- Paired replay: ledger state must be identical after continuous vs.
  save/restore runs.

### 5.2 Save integration

**New section row (proposal; integrator lands it):**

```csharp
new("funds_ledger", "SaveFundsLedger", "SetupFundsLedger", "economy",
    "Player-side fungible funds; integer chits; bounded movement log"),
```

Filename row: `{ "funds_ledger", "funds_ledger_save.json" }`.

**Migration:** absent section → balance 0, empty log, schemaVersion 1. No
starting grant is fabricated. If the difficulty system ever grants starting
funds, it does so through the existing XP-01 starting-bonus path, not through
this migration.

**Save-store discipline:** follow the newest-batch pattern
(`SaveStoreHub.FromCodec` + `SchemaVersionedEnvelope`) exactly as the
black-market/sanitation/vehicle stores do. Do not invent a new codec.

**Section-count gates:** adding a section changes the counts pinned by
`ComprehensiveSaveStoreCorruptionAndMigrationTests`, `VersionReportContractTests`,
the architecture map, and the save-store matrix. Each is updated by its owning
generator or its one-line pin, in the same package, exactly as CF-P6 did for
vehicle armor grades.

### 5.3 Market opt-in flag

**Catalog flag:** `accepts_funds` (bool, default false) on opt-in surfaces. Two
levels:

| Level | Where | Meaning |
|---|---|---|
| surface-level | `black_market_inventory.json` top level / `MerchantCaravanDef` | this surface can settle in funds |
| entry-level (optional) | per stock entry | this entry prefers funds when offered (used by routes later) |

When no flag is set anywhere, every existing code path is byte-identical.
The flag is additive; the loader binds it with a default of false.

**Settlement behavior when opted in:**

- Buy: debit `price_chits`; on insufficient funds, the existing goods-bill
  path remains available only if the surface also accepts goods (configurable
  per surface; the proposal: black market accepts both, holdfast accepts both,
  routes require funds for tariffs only).
- Sell: credit `price_chits`.
- The existing `SettlementUnits`/`TotalValue` computations become the source of
  the chit price through one conversion function
  `ChitsFromSettlementUnits(units)`, tested for rounding (floor, no hidden
  rounding gains).

### 5.4 Black-market legs (XP-04-F2)

Four actions, each with explicit legs and typed failures:

| Action | Debit | Credit | Goods leg | Failure states |
|---|---|---|---|---|
| `bm_buy` | chits → merchant | — | stock entry → shelter inventory | insufficient funds; heat cap; stock empty |
| `bm_sell` | — | merchant → player | goods → merchant stockpile | stockpile cap; heat cap; item not sellable |
| `bm_fence` | fee (20–35% of value) | net credited | flagged goods → unflagged | purity roll failure; heat spike; flagged classification missing |
| `bm_contract` | escrow full value | payout on delivery | goods delivered N days later | convoy loss; seller default; cancel before dispatch refunds minus fee |

**Contracts and debt:** an undelivered contract whose seller defaults converts
its escrow into a debt row in the sealed `LedgerDebtSystem`; the black market
does not own enforcement. A lost convoy goes through the sealed caravan-loss
path; the escrow handling is authored per contract template (refund/forfeit),
never invented at runtime.

**Exactly-once:** every action records a settlement key in the existing
black-market fired-event style so a reload cannot re-apply a credit/debit.

**Heat (XP-04-F4):** heat deltas/bands/relocation are implemented in this
phase, but they read/write only the existing black-market state; they do not
enter the funds ledger as a balance modifier. Heat relocation uses the existing
persisted seeded sub-stream and must not reroll on reload (tested).

### 5.5 Purity tiers (XP-04-F5)

- Purity rides item-instance state (the sealed equipment-condition/instance
  pattern); one tier per instance, persisted.
- Tier roll is seeded at acquisition and persisted — never re-rolled on load.
- Appraisal skill (Plan 191, live) improves the *displayed* tier accuracy; it
  never changes the underlying roll.
- A `cut` tier item carries an affliction-risk flag consumed on use through the
  sealed disease/medical pipeline — no parallel effect engine.

### 5.6 Route contract model (XP-08-F1..F4)

**Authored templates — `Assets/StreamingAssets/Data/trade_routes.json`:**

```json
{
  "schema_version": 1,
  "routes": [
    {
      "id": "route_shelter_to_krasthold",
      "counterparty_id": "holdfast_krasthold",
      "goods_out": [{ "item_id": "item_iodine", "units_per_run": 6 }],
      "goods_in":  [{ "item_id": "item_grain_sack", "units_per_run": 10 }],
      "cadence_days": 5,
      "tariff_chits": 4,
      "min_reliability_tier": 1,
      "caravan_slots_required": 1,
      "exclusive_good_id": null
    }
  ]
}
```

**Runtime instance shape:** route id, established day, reliability score, tier,
next-run day, suspended flag, last-outcome code, exclusive-unlocked flag.
Persisted inside the existing caravan/economy save owner (the F13-E signature
must name exactly one; both options are viable, but one must be chosen).

**Scheduling:** the caravan day-owner schedules a run when `current_day >=
next_run_day` and the route is active; the run consumes a caravan slot from the
existing dispatch capacity. No second logistics tick. If slots are unavailable,
the run is delayed (not lost) and reliability is not penalized for a slot
shortage — only for a run that actually dispatches and fails late.

**Reliability math (normative):**

```text
on_time:      +1
late:         -1
lost/raided:  -2
floor 0; no upper cap needed for tiers 1-4
tier thresholds: t1 any, t2 >= 5, t3 >= 12, t4 >= 20 (evaluated after the run)
tier-3 effect: tariff -25% (floor: authored minimum), scarce-goods access flag
tier-4 effect: unlock exclusive_good_id (one per route)
cancel: reliability resets after 30-day cooldown; suspend: -1 standing tick
```

**Risk binding (F3):** each run's path uses the sealed
`WastelandMapSystem.PlanRoute`; flooded/Unknown/blocked edges convert to a
delay or loss outcome deterministically, and waystation decisions follow the
sealed XP-03-F3 vocabulary. The route does not own combat; caravan encounter
resolution stays with the existing settlement.

**Anti-spam:** caravan slots are the scarce resource (bounded by vehicles and
drivers already in the game); reliability farming with trivial goods scores at
tier-1 volumes only; no route can target a counterparty that is at war with the
shelter.

### 5.7 Seasonal migration (XP-08-F5/F6)

**Catalog — `seasonal_migration.json`:**

```json
{
  "schema_version": 1,
  "dwell_days": 10,
  "factions": [
    {
      "faction_id": "faction_lowland_holdfasts",
      "schedule": [
        { "phase": "deep_winter", "region_id": "region_shelter_valley", "population_delta": 12 },
        { "phase": "thaw",        "region_id": "region_shelter_valley", "population_delta": -8 }
      ]
    }
  ]
}
```

**Owner:** one human population-weight read/write model inside the existing
economy/faction aggregate. It stores per-region integer weights; it does not
store individuals. `WildlifeEcosystemSystem` is explicitly excluded.

**Tick:** once per campaign day, after the existing season phase is resolved;
deltas apply with dwell hysteresis (no oscillation). No RNG. Paired-replay
equality required.

**Consequences (each through an existing owner):**

| Consequence | Owner | Rule |
|---|---|---|
| market availability | `MarketSystem` region multipliers | read weights; no direct price mutation |
| labor pool | apprenticeship/caregiving candidate read | scale candidate ordering, not truth |
| territorial friction | live ideological-friction event weights | weight modifier only |
| caravan demand | caravan contract/quest hooks | authored hook, not a new quest system |

**No double application:** deltas apply exactly once per day-phase transition;
the fired-phase key persists (same discipline as the black-market fired keys).

### 5.8 Restock amendment algorithm (F13-C, optional)

Normative once signed:

```text
capacity = restock_capacity_per_period
for each category: eff = weight * (stock < scarcity_floor ? 2 : 1)
allocate floor(capacity * eff / total_eff) to each category
distribute remaining units by largest fractional remainder (ties by authored
category order, then name)
within category: sort by stock/target_par ascending (rational compare),
tie-break authored restock_order, then item id ordinal
```

Allocation is quantity-only; prices, stock generation, and the day gate are
untouched. Tests cover: exact allocation on a worked example, largest-remainder
edge (all remainders equal), scarcity floor trigger, tie-breaks, determinism.

### 5.9 Holdfast funds legs

- `HoldfastTradeSession` keeps `TryConsumeBill` for goods-only exchanges.
- Opt-in: when `accepts_funds` and the player chooses the funds leg, the session
  calls the ledger for the chit value and skips the goods bill for the funds
  side. Mixed bills (some goods + some funds) are **out of scope for wave 1**;
  decline explicitly if proposed later.
- `HoldfastTradeResult` gains additive fields: `FundsDelta`, `FundsFailure`
  (additive, default none). Old callers are unaffected.

### 5.10 Quest reward integration (minimal)

A quest may author a funds reward. The reward path calls `TryCredit` with
`quest_reward`. No quest content is authored by this plan; the seam exists so
Plan 5's expansions can use it after their own authorization. If no quest
authors funds in wave 1, the content-utilization scan must not flag the seam
(it is a code seam, not a catalog row).

### 5.11 Debt and violence consequences (XP-04-F6)

- Unpaid `bm_contract` escrow → `LedgerDebtSystem` row (sealed owner).
- Unpaid debt escalation → `FactionBountySystem` (the black market already
  delegates, per the sealed Plan 211 design).
- Funds never enable a "pay to remove bounty" shortcut; debts settle through
  the sealed debt rules.

### 5.12 Panels and presentation

| Surface | Requirement |
|---|---|
| Funds readout | one line on opt-in surfaces: `FUNDS <balance> chits`; words + number, no color-only |
| Movement history | optional bounded list (day, +/- delta, reason label); labels from the closed vocabulary, localized keys |
| Black-market actions | buy/sell/fence/contract buttons with typed failure text; no restock button (R4: reopening never rerolls) |
| NEXT SUPPLY strip (F13-C only) | category + quantity arriving next period; words, not bars |
| Routes (XP-08) | cadence, next run, tier, reliability; suspend/cancel with confirmation |
| Migration | map/war widget population shift notes; no individual NPCs shown |
| Heat | `ATTENTION` wording, bands as words + numbers |

Panel work follows the `PanelRouteGate`/`PlayerSurfaceCoverageGate` and
`PlayerSurfaceBindingPurityGate` requirements; any new panel registers through
the existing bootstrap and manifest, never a side door.

### 5.13 Failure modes and diagnostics

| Failure | Behavior | Diagnostic |
|---|---|---|
| insufficient funds | typed failure, no partial debit | panel text from `FundsFailure` |
| unknown reason key | typed failure, no movement | logged once; test prevents new keys without constants |
| movement log malformed on restore | restore truncates to the valid prefix, logs once | save-load test |
| balance overflow | typed `InvalidAmount` | near-max test |
| route references missing item | validator error | data-integrity gate names route/item |
| migration region missing | validator error | data-integrity gate |
| migration double-apply | prevented by fired-phase key | reload-replay test |
| contract lost then reloaded | exactly-once settlement key | replay test |
| caravan slot shortage | delay only, no reliability penalty | route test |
| opt-in surface has no chit price | typed failure; goods path remains | surface test |

### 5.14 Anti-exploit analysis

| Exploit | Guard |
|---|---|
| arbitrage across funds and goods | one ledger; symmetric price legs; arbitrage probes extended |
| sell high, buy back low within one day | existing market day gates and stock pinning; route tariffs non-refundable |
| funds duplication on reload | exactly-once settlement keys; paired replay |
| route spam | caravan slots bounded; war state blocks counterparties |
| reliability farming | tier-1 volume cap on trivial runs; volume commitments per tier |
| migration oscillation | 10-day dwell hysteresis |
| purity re-roll on reload | seeded roll persisted per instance |
| contract default abuse | escrow forfeit/refund authored per template; debt owner enforces |
| chit printing via quests | rewards authored, validator-checked, bounded; no procedural generation |

### 5.15 Compatibility with sealed systems

- `MarketSystem` v2 additive: no change to its save shape; region multipliers
  derived, not stored.
- `BlackMarketSystem`: funds legs are additive; the sealed canonical-inventory
  settlement remains the default when no opt-in flag is present.
- `ShelterBarterSystem`: DEC-05 ordering untouched by F13-A; F13-C modifies
  allocation only, behind the signature.
- Caravan owners: XP-08 attaches to dispatch; no second travel/route authority.
- `LedgerDebtSystem`/`FactionBountySystem`: read/write through their existing
  APIs only.

### 5.16 Test fixture design

- Funds fixture: a ledger builder with deterministic day/amount helpers.
- Route fixture: authored route + a fake caravan slot pool.
- Migration fixture: two factions, two regions, four phases; asserts exact
  weights per day.
- All fixtures respect `docs/testing/FIXTURE_POLICY.md`; no anonymous campaign
  constructions.

### 5.17 Open questions requiring execution-time verification

1. The exact holdfast merchant catalog that would carry `accepts_funds`
   (confirm the live file(s); the `MerchantCaravanDef` type is in
   `ShelterBarterSystem` but the authored merchant rows may be elsewhere).
2. The precise `BlackMarketSystem` fired-key mechanism for contract exactly-once
   (read before adding a parallel key list).
3. The existing caravan slot capacity API and whether routes can reserve slots
   without changing dispatch semantics.
4. The day-owner ordering number for a migration tick (attach after season
   resolution; confirm the owner order comment).
5. Whether `SaveSectionRegistry` count updates are generated or pinned in the
   needed tests (check before editing).
6. Whether `trade_route_disrupted` already has a key constant usable by XP-08
   or needs one added to the event vocabulary.

Each is recorded as a premise note; none blocks F13-A.

### 5.18 Enrichment appendix — exact texts for XP-04/XP-08/C3/expansions

1. XP-04 packet: annotate F3 as "closed by DEC-05; optional amendment F13-C";
   annotate F1/F2 as "released by UNBLOCK-02 F13-A/D".
2. XP-08 packet: annotate F1–F4 "released by F13-E; C3 192 satisfied";
   F5–F6 "released by F13-F; C3 199 satisfied".
3. `docs/plans/wave8_part2/C3_DECISION.md`: add a dated addendum stating 192 and
   199 HOLDs are lifted by the signed F13-E/F and naming the owners; do not
   rewrite the historical table.
4. Expansions 17/25/26: insert the boundary notes from §2.5.
5. `UNBLOCKED_PLANS_AUDIT_2026-09-19.md`: superseding note for the restock rows.
6. `DECISION_REGISTER.md`: new rows DEC-25..DEC-30 (or a single bundle row)
   for F13-A..G, plus DEC-05 evidence stays 6/6.

### 5.19 Tone and presentation guard

- Funds are called chits, never "money" in player-facing text where chits is
  authored; no real-world currency symbols or brand likeness.
- Route counterparties are fictional holdfast names already in data.
- Migration prose describes weather and work, not ethnic or national movement.
- No gambling, no exploitation of scarcity in text; the economy is grim but
  never celebratory.---

## 6. Phased execution program

### Phase 0 — Reconciliation + premise (no production edits; 0.5 day)

Steps:

1. Close `CF-P5-RESTOCK-RECONCILE` per F13-B: correct the stale restock sentence
   in `INTEGRATION_PLANS.md`; confirm DEC-05 6/6; confirm the XP-04 richer design
   is labelled an amendment.
2. Run the Appendix A premise commands; locate the merchant catalog, black
   market fired-key mechanism, caravan slot API, day-owner order line, and save
   count pins.
3. Record premise notes.

**Exit:** CF-P5 reconciled with green evidence; premise notes recorded; no
production edit.

### Phase 1 — FundsLedger Core + save (1–1.5 days)

**SIGNATURE REQUIRED:** F13-A.

Files: new `Assets/Ashfall.Core/Economy/FundsLedger.cs`;
`Assets/Ashfall.Core/Save/SaveSectionRegistry.cs` (integrator seam);
new save-store + host wiring (economy group);
`Ashfall.Core.Tests/Economy/FundsLedgerTests.cs` (new);
count-pin tests updated by their one-line pins; generators regenerated.

Work: ledger contract, bounded log, typed failures, overflow guard; capture/
restore; section row + filename; migration empty; tests.

Exits: `FundsLedgerTests` green (debit/credit atomicity, bounded eviction,
restore fidelity, overflow, unknown key, zero amount); save round-trip; count
gates updated; build 0/0; data-integrity unaffected.

### Phase 2 — Black-market legs (2–3 days)

**SIGNATURE REQUIRED:** F13-A, F13-D (black market).

Files: `BlackMarketSettlementService.cs` (funds path additive),
`BlackMarketSystem.cs` (preflight), `black_market_inventory.json`
(`accepts_funds`), host session, `BlackMarketPanel`, tests
(`BlackMarketFundsLegTests`, heat/fence tests).

Work: buy/sell/fence/contract legs; chit conversion; exactly-once settlement
keys; heat bands/relocation; purity tiers; debt routing; panel readout and
typed failures.

Exits: leg tests green; sealed settlement tests unchanged (goods default);
heat relocation anti-reroll test; save/load replay; panel lifecycle PASS;
data-integrity PASS; content-utilization PASS.

### Phase 3 — Holdfast funds leg (1 day)

**SIGNATURE REQUIRED:** F13-D (holdfast).

Files: `HoldfastTradeSession.cs`, host wiring, panel, tests
(`HoldfastFundsLegTests`).

Exits: funds leg test; goods-only path byte-identical; panel readout.

### Phase 4 — Restock amendment (optional; 1–2 days)

**SIGNATURE REQUIRED:** F13-C.

Files: `ShelterBarterSystem.cs`, merchant catalogs, tests
(`MerchantRestockAllocationTests`), panel NEXT SUPPLY strip.

Exits: allocation math tests; DEC-05 tests unchanged; panel contract.

### Phase 5 — Route contracts (XP-08 F1–F4; 3–4 days)

**SIGNATURE REQUIRED:** F13-E (and F13-A landed).

Files: `trade_routes.json`; Core route contract/instance/reliability types;
caravan host scheduling hook; save owner (named in F13-E); panel ROUTES strip;
tests `TradeRouteContractTests`, `RouteReliabilityTests`,
`RouteRiskBindingTests`.

Exits: contract/reliability/risk tests; reload replay; caravan regression;
data-integrity + content-utilization; `trade_route_disrupted` producer present.

### Phase 6 — Seasonal migration (XP-08 F5–F6; 3–4 days)

**SIGNATURE REQUIRED:** F13-F.

Files: `seasonal_migration.json`; Core population-weight model; day-owner hook;
market/labor/friction/caravan read hooks; map/widget presentation; tests
`SeasonalMigrationTests`, `MigrationConsequenceTests`.

Exits: schedule application test; consequence no-double-apply; reload replay;
wildlife system untouched (grep-verified); data-integrity PASS.

### Phase 7 — Exclusive goods + closeout (0.5–1 day)

**SIGNATURE REQUIRED:** F13-G.

Steps: author tier-4 lists; validator rule; closeout; ledger/register updates;
C3 addendum; XP-04/XP-08 annotations; handoff.

**Total estimated effort:** 11–16 builder-days across phases; each phase a
separate claim window.

---

## 7. Verification plan

### 7.1 Focused test matrix

| Phase | Target | Expected |
|---|---|---|
| 0 | `Plan147RestockPriorityTests.cs` | 6/6 (reconciliation evidence) |
| 1 | `FundsLedgerTests.cs` | new, 15–20 cases |
| 1 | save count gates | updated by owning pins |
| 2 | `BlackMarketFundsLegTests.cs` | new, 10–16 cases |
| 2 | existing black-market suite | unchanged + additive |
| 3 | `HoldfastFundsLegTests.cs` | new, 6–10 cases |
| 4 | `MerchantRestockAllocationTests.cs` | new, 8–12 cases |
| 5 | `TradeRouteContractTests.cs` | new, 10–14 cases |
| 5 | `RouteReliabilityTests.cs` | new, 6–10 cases |
| 5 | `RouteRiskBindingTests.cs` | new, 5–8 cases |
| 6 | `SeasonalMigrationTests.cs` | new, 8–12 cases |
| 6 | `MigrationConsequenceTests.cs` | new, 6–10 cases |
| all | `--data-integrity-selftest` | 0 errors |
| all | `--content-utilization-selftest` | 0 new orphans |
| all | `--panel-bind-lifecycle-selftest` | PASS |
| all | paired replay | equality |
| close | `MainTriadDriftGateTests` | green |

### 7.2 Commands

```bash
bash scripts/run_test.sh Ashfall.Core.Tests/Economy/FundsLedgerTests.cs
bash scripts/run_test.sh Ashfall.Core.Tests/Economy/BlackMarketFundsLegTests.cs
bash scripts/run_test.sh Ashfall.Core.Tests/Economy/HoldfastFundsLegTests.cs
bash scripts/run_test.sh Ashfall.Core.Tests/Economy/MerchantRestockAllocationTests.cs
bash scripts/run_test.sh Ashfall.Core.Tests/Economy/TradeRouteContractTests.cs
bash scripts/run_test.sh Ashfall.Core.Tests/Economy/RouteReliabilityTests.cs
bash scripts/run_test.sh Ashfall.Core.Tests/Economy/RouteRiskBindingTests.cs
bash scripts/run_test.sh Ashfall.Core.Tests/Economy/SeasonalMigrationTests.cs
bash scripts/run_test.sh Ashfall.Core.Tests/Economy/MigrationConsequenceTests.cs
bash scripts/run_test.sh Ashfall.Core.Tests/Economy/Plan147RestockPriorityTests.cs
dotnet build Ashfall.csproj --no-restore
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --content-utilization-selftest
godot --headless --path . -- --panel-bind-lifecycle-selftest
```

### 7.3 Failure-proof obligations

- Funds: the near-max overflow and zero-amount tests must fail before the guard.
- Exactly-once: a replay test must fail if the settlement key is removed.
- Allocation: the worked-example test must fail if largest-remainder is dropped.
- Migration: the double-apply test must fail if the fired-phase key is removed.
- Routes: the slot-shortage test must fail if reliability is penalized for a
  delay.

### 7.4 What is not accepted as evidence

- A goods-only test passing as proof of the funds leg.
- A panel screenshot without lifecycle/a11y gates.
- "The proposal defined it" — only current source + tests.

---

## 8. Risks and mitigations

| # | Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|---|
| 1 | Funds become an arbitrage faucet | medium | high | one ledger; symmetric legs; arbitrage probe extension |
| 2 | Funds duplicate on reload | medium | high | exactly-once settlement keys + replay tests |
| 3 | The richer restock design is executed without F13-C | medium | medium | F13-B explicitly labels it an amendment; DEC-05 is the live rule |
| 4 | Route work silently builds a second logistics layer | medium | high | F13-E names seam owners; caravan dispatch remains sole mover |
| 5 | Migration reuses or mutates wildlife state | low | high (architecture) | F13-F forbids it; grep gate + test |
| 6 | Migration oscillates with weather | medium | medium | 10-day dwell hysteresis + phase key |
| 7 | Section-count/doc generators drift | medium | low | run owning generators `--check` in each phase |
| 8 | Opt-in flag leaks into non-opted surfaces | low | medium | default false; goods path byte-identical tests |
| 9 | Contract escrow creates a second debt ledger | low | high | route to `LedgerDebtSystem`/`FactionBountySystem` only |
| 10 | Purity becomes a hidden affliction model | low | high | risk flag consumed by the sealed pipeline only |
| 11 | `trade_route_disrupted` remains orphaned after routes | low | low | route emits it; content-utilization verified |
| 12 | Tier-4 exclusive goods power-creep | medium | medium | one id per route; not strictly superior; validator |
| 13 | Catalog collisions with Plan 1/Plan 5 | low | medium | different files except economy goods; check ownership before edit |
| 14 | EN-03 authorized too early | medium | medium | Plan 5 signs EN-03 only after Phase 2 lands |

---

## 9. Ownership, sequencing, and coordination

### 9.1 Proposed claims

| Phase | Claim | Owner | Disjoint paths |
|---|---|---|---|
| 0 | `UNBLOCK-02-P0-RECONCILE` | integrator | INTEGRATION_PLANS/DECISION_REGISTER wording |
| 1 | `UNBLOCK-02-P1-FUNDS-CORE` | builder | Economy/FundsLedger + save |
| 2 | `UNBLOCK-02-P2-BM-LEGS` | builder | BlackMarket* + catalog flag + panel |
| 3 | `UNBLOCK-02-P3-HOLDFAST` | builder | HoldfastTradeSession + panel |
| 4 | `UNBLOCK-02-P4-RESTOCK` | builder | ShelterBarterSystem + catalogs |
| 5 | `UNBLOCK-02-P5-ROUTES` | builder | trade_routes + caravan host + save owner |
| 6 | `UNBLOCK-02-P6-MIGRATION` | builder | seasonal_migration + day owner + read hooks |
| 7 | `UNBLOCK-02-P7-CLOSEOUT` | integrator | ledgers/register/enrichment |

Phases 1→2→3 sequential; 4 independent after 1; 5 after 1 (uses tariff);
6 after 1 (uses nothing but day owner, but keep builders disjoint); 7 last.

### 9.2 Shared paths and race rules

- `SaveSectionRegistry.cs` is an integrator seam: one writer at a time.
- `Main.CampaignOwners.cs` day-owner line: keep phase 6 and any other active
  day-owner edits in separate claim windows.
- Economy catalog region (`economy_goods.json`): Plan 1 does not touch it;
  expansion intake may; check ownership.
- `BlackMarketSystem.cs` already has a sealed claim history; read the latest
  handoff before editing.

### 9.3 Cross-plan coordination

| Sibling | Interface | Rule |
|---|---|---|
| Plan 1 | `items.json` region only | different files; no ordering constraint |
| Plan 3 | panel strings/localization | if string freeze lands first, author funds/route labels with freeze keys |
| Plan 4 | D3 catalog ordering; register rows | Plan 4 records DEC-25.. rows after execution; no file overlap |
| Plan 5 | expansions 17/25/26; EN-03 authorization; C3 addendum | Plan 5 signs EN-03 only after Phase 2; adds C3 addendum after F13-E/F |

---

## 10. Rollback and decline paths

- **Before Phase 1:** deleting this document changes nothing.
- **Phase 1 only:** new section exists but no surface opts in; rollback removes
  the section row (old saves unaffected because balance is 0/empty).
- **Phase 2/3:** disable the opt-in flags; goods paths remain; ledger state can
  remain dormant in saves. Do not delete the section while saves may contain it.
- **Phase 4 (optional):** revert to DEC-05 ordering; allocation code behind the
  flag.
- **Phase 5/6:** routes/migration state inside their save owner; rollback
  removes the catalog rows and leaves dormant state that restores harmlessly.
- **Decline of F13-A:** XP-04/XP-08/EN-03 stay blocked; 192/199 stay held; the
  expansions still ship without funds; record the decline so Plan 5 does not
  assume the chain.
- **Decline of F13-E/F while keeping F13-A:** funds still release XP-04 and the
  black-market legs; XP-08 and the C3 lifts stay held.

---

## 11. Definition of done and handoff

### 11.1 DoD per line

| Line | Done when |
|---|---|
| F13-A | ledger tests + save round-trip + section matrix green |
| F13-B | CF-P5 closed; DEC-05 6/6; stale sentence corrected |
| F13-C | allocation tests green; DEC-05 behavior unchanged when flag off |
| F13-D | both surfaces settle in funds; goods default preserved |
| F13-E | route contract/reliability/risk tests green; C3 192 addendum |
| F13-F | migration schedule + consequence tests green; wildlife untouched; C3 199 addendum |
| F13-G | exclusive rows validated; no strictly-superior goods |

### 11.2 Handoff fields

Outcome, files, contract, evidence, shared paths untouched, proposed ledger
edits, next safe step — per `AI_AGENT_WORKFLOW.md`.

### 11.3 First safe step

> Phase 0 reconciliation and premise notes. No production edit before F13-A is
> signed.

---

## 12. Worked scenarios

### 12.1 A black-market purchase in funds

1. Player opens the black market panel; funds readout shows `FUNDS 12 chits`.
2. The player selects a stock entry priced 8 chits and chooses the funds leg.
3. `TryDebit(8, bm_buy, entry_id, day)` succeeds; balance 4; one movement logged.
4. The item moves to shelter inventory; heat +1; the panel refreshes.
5. Save and reload: balance 4; the movement list shows the one purchase; no
   second debit; heat unchanged.

### 12.2 A route is established and lost

1. The player signs a contract for `route_shelter_to_krasthold` (tariff 4
   chits; goods legs authored). Reliability tier 1.
2. Five successful runs over 25 days: reliability 5 → tier 2, cadence −1 day,
   volumes +25%.
3. A flooded edge delays a run: reliability −1 (late) and the waystation
   decision routes per the sealed vocabulary.
4. A war front closes the counterparty's region; the route auto-suspends and
   standing ticks down once. Cancel resets reliability after a 30-day cooldown.
5. Save/reload mid-cadence: next-run day, reliability, and tier identical.

### 12.3 Seasonal migration changes the valley

1. Deep winter phase applies `faction_lowland_holdfasts` +12 population weight
   to the shelter valley; dwell gate prevents oscillation.
2. Market availability multipliers shift toward calorie demand; the labor
   candidate pool orders more hopefuls; friction event weights rise slightly.
3. Thaw applies −8 after the dwell window; boat-passage edges reopen through
   the sealed map system.
4. A reload at day 3 of the phase applies no second delta (fired-phase key).
5. Wildlife systems are untouched; a grep test proves no wildlife state write.

### 12.4 Restock amendment in a siege (F13-C only)

1. Capacity 40; medical stock 2 (floor 3) → effective weight doubled.
2. Allocation gives medical its proportional share plus remainder; calories a
   smaller share.
3. Panel NEXT SUPPLY shows "MEDICAL +6, CALORIES +11, …"; words and numbers.
4. Draining medical guarantees medical priority next period — intended siege
   logic, bounded by capacity.

---

## 13. Foreman briefing — anticipated questions

**Q1. Why one funds ledger instead of per-market purses?**
Per-market purses multiply state, complicate cross-market price comparison, and
add conversion tables with no gameplay gain. One ledger makes arbitrage
symmetric and testable.

**Q2. Isn't this just adding "money" to a barter game?**
No. Barter remains the default; funds are an *opt-in medium* on two surfaces.
The design deliberately keeps goods-clearing byte-identical when the flag is
absent, so the survival-barter identity is preserved and can stay the norm.

**Q3. What stops a funds economy from trivializing survival?**
Routes and tariffs are calibrated to stability, not profit (XP-08 §6: ~2–4
chits margin per run); chits buy stock that restock rules already gate; no
funds purchase of food bypasses spoilage, rationing, or the day gate.

**Q4. Why is restock not the primary blocker?**
DEC-05 already signed and shipped the live ordering; the XP-04 document asked
for a richer allocation that changes quantities. That is real but optional;
naming it F13-C prevents accidental execution without a signature.

**Q5. What happens to goods-only players who never opt in?**
Nothing changes. No surface force-converts; the ledger sits at 0.

**Q6. Does XP-08 need Plan 32 sealed first?**
Plan 32's graph travel is already sealed (expeditions + caravans + fog gate),
so route risk binding consumes a sealed owner. No new dependency.

**Q7. Can routes and migration ship separately?**
Yes. F13-E and F13-F are independent; routes-first is recommended because
migration consequences read market weights that routes make more meaningful.

**Q8. Is a new save section really needed?**
Yes for funds (a new domain), no for routes/migration (additive inside the
existing caravan/economy owner). The save-count gates must be updated by their
owners in the same package.

**Q9. What about the sealed black-market settlement (DEC-02)?**
It remains the default. Funds legs are additive; the exactly-once and
canonical-inventory guarantees are preserved and tested on the funds path too.

**Q10. What is the smallest release?**
F13-A + F13-D + Phase 1–3: a funds medium on two surfaces, releasing the XP-04
core and the EN-03 gate. Roughly 4–5 builder-days.

**Q11. What is the full release?**
All phases: XP-04 + XP-08 + C3 192/199 + expansions clarifications. Roughly
11–16 builder-days, mostly content and tests.

**Q12. How do we know a hold is truly lifted rather than re-signed?**
The C3 addendum names the owner and the DTO; the route/migration tests prove a
consumed surface exists; the content-utilization gate proves no orphan key
remains (`trade_route_disrupted` gains its producer).

**Q13. Could funds be declined but restock still amended?**
Yes — F13-C is independent of F13-A. But without funds, XP-04/XP-08/EN-03
remain blocked; the amendment alone is economy polish, not an unblock.

**Q14. What is the interaction with difficulty starting bonuses?**
XP-01 may grant starting items; if it ever grants funds, it must call
`TryCredit` through the ledger rather than authoring a starting balance in the
migration. This plan does not grant starting funds.

**Q15. Are there text-freeze implications?**
Funds/route/heat labels are new player-facing strings. If Plan 3 declares a
string freeze first, author these labels with freeze keys. If not, record them
as part of the freeze inventory later.

---

## 14. Enrichment texts (paste-ready after execution)

### 14.1 XP-04 packet header

> **Status update <date>:** F13-A (FundsLedger) signed and executed as
> UNBLOCK-02 Phases 1–3; F13-B reconciled DEC-05 as the live restock rule;
> F13-C is an optional amendment. See
> `docs/plans/unblockers/UNBLOCK-02_FUNDS_TRADE_F13_XP04_XP08.md`.

### 14.2 XP-08 packet header

> **Status update <date>:** F13-E/F signed; route contracts and seasonal
> population migration executed as UNBLOCK-02 Phases 5–7. C3 192/199 HOLDs
> lifted by named seams (standing/raid/save; human population owner).

### 14.3 C3 addendum

> **Addendum <date> — holds lifted:** 192 and 199 are lifted under the signed
> route-contract and population-owner decisions (F13-E/F). The historical HOLD
> table above is retained for provenance.

### 14.4 Expansion 17/25/26 intake notes

- 17: "Festivals consume goods and labor; no funds leak into leisure."
- 25: "Freight and rail schedules are not player trade routes (XP-08 F1); if a
  freight tariff is proposed, it uses the canonical FundsLedger."
- 26: "Rationing remains `ResourceRationingSystem`; food is never a currency."

### 14.5 INTEGRATION_PLANS row

```markdown
| `UNBLOCK-02-FUNDS-TRADE` | Integrator (signed <date>) | [exact paths] | **DONE <date>:** funds ledger + black-market/holdfast legs + route contracts + seasonal migration; XP-04/XP-08 released; C3 192/199 lifted | focused suites + data-integrity + content-utilization + panel-lifecycle + paired replay |
```

---

## Appendix A — Premise commands

```bash
# 1. No funds authority exists
grep -rn "FundsLedger" --include=*.cs Assets/Ashfall.Core src

# 2. Restock live evidence
grep -n "ComputeItemPriorityScore\|GetPrioritizedStock" -A 4 \
  Assets/Ashfall.Core/Economy/ShelterBarterSystem.cs | head -40
bash scripts/run_test.sh Ashfall.Core.Tests/Economy/Plan147RestockPriorityTests.cs

# 3. XP-04 richer keys absent
grep -rn "restock_capacity_per_period\|scarcity_floor\|priority_categories" \
  Assets/StreamingAssets/Data Assets/Ashfall.Core src

# 4. Settlement value units
grep -n "SettlementUnits\|TotalValue" \
  Assets/Ashfall.Core/Economy/BlackMarketSettlementService.cs \
  Assets/Ashfall.Core/HoldfastTradeSession.cs | head

# 5. Caravan owners
ls Assets/Ashfall.Core/Economy/ | grep -i caravan

# 6. Debt/bounty owners
grep -rn "LedgerDebtSystem\|FactionBountySystem" \
  Assets/Ashfall.Core/Economy/BlackMarketSystem.cs | head

# 7. Orphan key
grep -rn "trade_route_disrupted" Assets/ src/ --include=*.cs --include=*.json | head

# 8. Save owner and counts
grep -n "\"economy\"" -A 3 Assets/Ashfall.Core/Save/SaveSectionRegistry.cs | head
grep -rn "section-count\|sectionCount" Ashfall.Core.Tests/Save/ComprehensiveSaveStoreCorruptionAndMigrationTests.cs | head

# 9. Day-owner order
grep -rn "OwnerOrder\|ownerId" src/Main.CampaignOwners.cs | head -20

# 10. Migration boundary
grep -rn "seasonal\|migration" Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs | head
```

---

## Appendix B — File inventory proposal

**Core:** new `Economy/FundsLedger.cs`; `Economy/BlackMarketSettlementService.cs`;
`Economy/BlackMarketSystem.cs`; `Economy/ShelterBarterSystem.cs`;
`HoldfastTradeSession.cs`; `Save/SaveSectionRegistry.cs`;
route and migration contract types; read-model helpers.
**Data:** `black_market_inventory.json`; merchant catalog(s) (confirm);
`trade_routes.json` (new); `seasonal_migration.json` (new); `economy_goods.json`
only if a validator row demands it.
**Host/UI:** black-market host + panel; holdfast host + panel; caravan host +
route panel strip; migration read hooks + map/widget lines.
**Tests:** the nine focused files in §7.1 plus one replay test if not already
covered by an existing route.
**Generated:** architecture map, save-store matrix, catalog registry, selftest
manifest, docs index (all via generators).
**Governance:** register/integration/debt/C3 addenda by the integrator.

---

## Appendix C — Glossary

| Term | Meaning |
|---|---|
| chit | integer funds unit, 1 ≈ one canned ration at standard baseline pricing |
| settlement units | the existing derived value unit in black-market previews |
| opt-in surface | a trade surface with `accepts_funds` authored true |
| reliability tier | route maturity 1–4 unlocked by successful runs |
| population weight | aggregate regional human-population delta, never individuals |
| fired-phase key | persisted exactly-once marker for a season delta |

---

## Appendix D — Cross-plan interface table

| Interface | This plan provides | Consumed by |
|---|---|---|
| `FundsLedger` | F13-A | XM-04, XP-08, EN-03, quest rewards, expansions (optional) |
| black-market funds legs | Phase 2 | XP-04 F2/F4/F5/F6 |
| restock rule truth | F13-B | CF-P5 closure, PM economy audits |
| route contract model | F13-E/Phase 5 | C3 192, EN-02/EN-03 read-models |
| population-weight owner | F13-F/Phase 6 | C3 199, EN-02/EN-03, expansions 17/25/26 |
| exclusive-goods discipline | F13-G | content balance |

**End of UNBLOCK-02.** Proposal only; no execution, claim, or certification.
The next action is the foreman's: sign F13-A/B/D, decline, or defer with a
condition.---

## 15. Release-train view — how this plan unblocks the queue

The queue has a dependency shape that is invisible in any single document. This
section draws it so the foreman can see the marginal value of each signature.

### 15.1 The blocked chain, drawn

```text
                       F13-A FundsLedger (unsigned)
                                │
             ┌──────────────────┼───────────────────
             ▼                  ▼                   ▼
        XP-04 legs         XP-08 F1–F4        XP-08 F5–F6
        (F2/F4/F5/F6)      (routes)           (migration)
             │                  │                   │
             ▼                  ▼                   ▼
        EN-03 gate         C3 192 lift        C3 199 lift
        (authorization)    (route DTO)        (population owner)
             │                  │                   │
             ▼                  ▼                   ▼
      Wave-2/3/4 expansion content that *references* an economy but was
      designed not to require it (17 Long Evening, 25 Iron Road, 26 Common Table)
```

Every arrow above is either a signature or a named owner. There is no unknown
technical step. The chain has been blocked since the XP proposal was written
because the first signature was not taken, not because anything was difficult.

### 15.2 Marginal value of each signature

| Signature | Standalone value | Unlocks next signature |
|---|---|---|
| F13-A | a player funds medium on two surfaces | F13-D, F13-E, F13-F, EN-03 gate |
| F13-B | queue truth; CF-P5 closure | nothing (free) |
| F13-C (optional) | partial-restock depth | nothing |
| F13-D | wave-1 surface scope | nothing |
| F13-E | routes; C3 192 lift | EN-02/EN-03 read-models |
| F13-F | migration; C3 199 lift | EN-02/EN-03 read-models |
| F13-G | tier-4 balance discipline | expansion economy hooks |

### 15.3 What happens if only F13-A/D are signed and executed

- XP-04 closes (all six features) and EN-03 becomes authorizable in Plan 5.
- XP-08 stays held because its contract and migration DTOs are unsigned.
- C3 192/199 stay held.
- The expansion waves still ship (they were designed not to need funds), but
  their "optional economy hooks" stay unavailable.
- The queue's nonterminal count drops by one pillar; the hold count does not.

### 15.4 What happens if F13-E/F also ship

- XP-08 closes; C3 192/199 are lifted with named owners.
- EN-02 (Living Map) and EN-03 read-models gain their economy legs.
- Expansions 25 (rail freight) and 26 (food culture) gain truthful boundaries
  and optional hooks without duplicating a ledger.
- The release train for Wave 5 (expansion intake) has a complete economic spine.

### 15.5 Interaction with the other four unblock plans

| Plan | Shared resource | Ordering rule |
|---|---|---|
| Plan 1 (body schema) | none | any order |
| Plan 3 (voice/strings) | new panel labels | if string freeze first, use freeze keys |
| Plan 4 (register truth) | DEC register rows; D3 catalog | Plan 4 signs D3 before economy catalog edits; Plan 4 records DEC-25.. after execution |
| Plan 5 (expansion intake) | EN-03 authorization; C3 addendum; expansion notes | Plan 5 authorizes EN-03 only after Phase 2; adds C3 addendum after F13-E/F |

---

## 16. Decision memo — F13-A in full (foreman depth read)

This memo follows the repo's decision-memo style: current state, options,
recommendation, blast radius, recheck.

### 16.1 Current state (verified)

- No `FundsLedger` symbol.
- `HoldfastTradeSession.TryConsumeBill` settles goods bills; `TotalValue` is
  derived, not owned.
- `BlackMarketSettlementService` settles against canonical inventory; the
  preview/result carry `SettlementUnits`.
- `ShelterBarterSystem` implements DEC-05 ordering; no quantities change.
- The XP-04 proposal's funds design is complete and balanced (chit/ration
  parity, fence fee, heat bands, purity tiers).

### 16.2 Options

**Option A — Integer ledger + opt-in surfaces (proposal).**
One new Core type, one new save section, two opt-in surfaces, additive fields
on existing results. Old paths byte-identical.

**Option B — Currency item.**
Rejected. It would create a second inventory authority (the exact failure mode
`AGENTS.md` Rule 5 forbids), add pickup/weight/stacking UI, and open a
duplication surface through every inventory operation. The only thing it buys is
physicality, which the panel readout already supplies.

**Option C — Per-faction purses.**
Rejected. Multiplies state by counterparties, forces conversion tables, and
makes cross-market arbitrage asymmetric in a way that is hard to test.

**Option D — Decline entirely.**
Valid but consequential: XP-04 is retired, XP-08 loses tariffs and its contract
model, C3 192/199 stay held, EN-03 stays gated, and the expansion economy hooks
are permanently unavailable. The decline path must be recorded explicitly so
Plan 5 does not intake expansions against an assumed economy spine.

### 16.3 Recommendation

Option A. The evidence for it is not just design preference: every neighboring
system already computes value, the repo has three worked precedents for
additive stateful systems with bounded logs (`MedicalRecordLog`,
`BlackMarketState.factionBounties`, difficulty completion-history v2), and the
save-store pattern is fully burned in. The risk is arbitrage and duplication,
both handled by the single-ledger rule and exactly-once keys.

### 16.4 Blast radius (recap with census)

| Area | New/changed | Count |
|---|---|---|
| New Core files | `FundsLedger.cs` (+ route/migration types in later phases) | 1 (+4 later) |
| Changed Core files | settlement service, black market, barter, holdfast session | 4–6 |
| New save section | `funds_ledger` | 1 |
| New catalog files | `trade_routes.json`, `seasonal_migration.json` | 2 (later phases) |
| Changed catalogs | black-market inventory, merchant catalog(s), economy goods (validator only) | 1–3 |
| New tests | nine focused files | 9 |
| Panels | black market, holdfast, economy ticker strip, routes strip, map widget | 5 surfaces |
| Generated docs | arch map, save matrix, catalog registry, selftest manifest, docs index | 5 generators |

### 16.5 Recheck trigger

Any proposal to (a) add a second funds-like store, (b) make funds an inventory
item, (c) change the movement-log bound, or (d) authorize EN-03 before Phase 2
lands.

---

## 17. Surface inventory — each trade surface and its funds delta

This appendix exists so the builder does not have to re-derive which surfaces
exist and which are in/out of wave 1.

### 17.1 In wave 1 (F13-D)

| Surface | Current behavior | Funds delta | Default if flag absent |
|---|---|---|---|
| Black market buy/sell | canonical-inventory settlement | debit/credit chits; goods leg unchanged | goods-only |
| Black market fence | not in wave 1 design? The XP-04 design includes it | fee debit + net credit; flagged→unflagged | not offered |
| Black market contract | loans exist; delivery contracts are new | escrow debit; payout/refund | not offered |
| Holdfast buy/sell | goods bill | debit/credit chits instead of goods bill | goods-only |

### 17.2 In XP-08 (later phases)

| Surface | Current behavior | Funds delta |
|---|---|---|
| Route tariff | n/a (no player routes) | fixed tariff debit per run at dispatch |
| Route proceeds | n/a | optional credit when goods are sold on return |
| Route cancel/suspend | n/a | no ledger movement; standing/cooldown rules only |

### 17.3 Explicitly out (wave 1 and XP-08)

| Surface | Reason |
|---|---|
| `MarketSystem` buy/sell | remains goods/price authority; no player wallet interaction |
| Quest rewards | seam exists; content comes later; no wave-1 content |
| Debt settlement | routes through the sealed debt owner; funds only as the payment medium |
| Bounties | sealed bounty owner; not a funds surface |
| Caravan NPC trade | NPC-to-NPC remains goods-based; no player funds |
| Trade network interfaces | NPC route network untouched |
| Player-to-player | no multiplayer |
| Banking/loans beyond sealed black-market loans | out of scope |

### 17.4 Surface table consequences for tests

- Each in-wave surface needs one "flag absent ⇒ byte-identical" test.
- Each in-wave surface needs one "insufficient funds ⇒ typed failure, no
  partial settlement" test.
- Each later phase surface needs one reload exactly-once test.
- Out-of-scope surfaces need a guard test that they never read the ledger
  (static grep gate or a focused test asserting no call site).

---

## 18. Balance tables and worked numbers

These are the numbers a reviewer will demand; each is traceable to the XP-04/
XP-08 proposals and is pinned by a test once implemented.

### 18.1 Chit/ration parity

| Baseline | Value |
|---|---|
| 1 chit | one canned ration at standard difficulty baseline pricing |
| Rationale | players price everything against food, the survival floor |
| Test | a fixture price check: standard canned ration value = 1 chit after conversion |

### 18.2 Black-market legs

| Leg | Formula | Example |
|---|---|---|
| buy | canonical value × risk premium (≥1.25×) | value 10 → ≥13 chits |
| sell | canonical value × (1 − broker spread) | value 10 → ~7–8 chits |
| fence fee | 20–35% of value | value 10 → 2–3.5 chits fee |
| floor | never below canonical +25% | buy never < 12.5 at value 10 |
| purity price | sealed 100%, clean 90%, suspect 65%, cut 45% | applied to buy price |

### 18.3 Heat bands

| Trigger | Delta |
|---|---|
| any transaction | +1 |
| fence | +2 |
| failed purity roll | +3 |
| idle decay | −1 per 2 days |

| Band | Effect |
|---|---|
| 0–4 | normal |
| 5–9 | price premium +10%, fewer stock slots |
| 10+ | market relocates 7–14 days (seeded, anti-reroll) |

### 18.4 Restock allocation worked example (F13-C)

Capacity 40; weights medical 30, calories 25, fuel 15, tools 12, luxury 8,
weapons 10; medical stock below floor.

1. effective: medical 60, calories 25, fuel 15, tools 12, luxury 8, weapons 10
   → total 130.
2. proportional: medical 18.46, calories 7.69, fuel 4.62, tools 3.69, luxury
   2.46, weapons 3.08 → floors 18/7/4/3/2/3 = 37; remainder 3.
3. remainders: medical .46, calories .69, fuel .62, tools .69, luxury .46,
   weapons .08 → largest: calories .69, tools .69, fuel .62 → +1 each.
4. final: medical 18, calories 8, fuel 5, tools 4, luxury 2, weapons 3 = 40.

Deterministic; ties broken by authored order then name.

### 18.5 Route economics

| Item | Value |
|---|---|
| Base cadence | 5 days |
| Example exchange | 6 iodine out, 10 grain in |
| Per-run margin | ~2–4 chits at standard pricing |
| Tier gates | 5 / 12 / 20 successful runs |
| Tier 2 | cadence −1, volumes +25% |
| Tier 3 | tariff −25%, scarce-goods flag |
| Tier 4 | one exclusive good |
| Reliability moves | +1 on time, −1 late, −2 lost |
| Cancel cooldown | 30 days |

### 18.6 Seasonal migration schedule (example)

| Phase | Region | Delta | Consequence |
|---|---|---|---|
| deep winter | shelter valley | +12 | calorie demand +30%, labor pool + |
| thaw | shelter valley | −8 | boat edges reopen |
| growing season | fields/outposts | +6 | forager encounter mix |
| ash storms | metro/deep shelters | +9 | markets relocate |

Dwell: 10 days per phase; hysteresis prevents oscillation.

---

## 19. Simulation and soak expectations

The implemented package should be able to run a bounded deterministic soak:

- **20-day funds soak:** buy/sell/fence/contract across two surfaces; assert
  balance path identical continuous vs. reload at day 10; movement log bounded.
- **40-day route soak:** establish two routes; force one flood delay and one
  raid loss; assert reliability path, tier transitions, tariff debits exactly
  once per run.
- **30-day migration soak:** four phase transitions with dwell; assert weights
  per day, market multipliers shift, no wildlife change.
- **combined 60-day campaign:** funds + routes + migration + one black-market
  debt default; assert fields and checksum equality across a mid-run reload.

These are soaks, not full-suite runs; each is a focused harness bounded to the
economy domain per TEST_POLICY.

---

## 20. Queue-state ledger patch list (post-execution)

For the integrator, the exact rows to touch after each phase:

| File | Row/area | Patch |
|---|---|---|
| `INTEGRATION_PLANS.md` | current batch | add UNBLOCK-02 package row after the active batch permits |
| `INTEGRATION_PLANS.md` | stale restock sentence | correct per F13-B (CF-P5 closure) |
| `DECISION_REGISTER.md` | after DEC-20 | add DEC-25 (funds), DEC-26 (restock truth), DEC-27 (restock amendment, if signed), DEC-28 (opt-in surfaces), DEC-29 (routes), DEC-30 (migration), DEC-31 (exclusive goods) |
| `DECISION_REGISTER.md` | DEC-05 | evidence stays 6/6; no verdict change |
| `docs/plans/wave8_part2/C3_DECISION.md` | 192/199 rows | dated addendum lifting both |
| `KNOWN_DEBT.md` | `DEBT-PLANS170-199-PORTFOLIO` | evidence line updated to show 192/199 lifted |
| `AGENTS.md` | decision-blocked list | remove F13 once signed; remove 192/199 holds once lifted |
| `docs/plans/UNBLOCKED_PLANS_AUDIT_2026-09-19.md` | §4 | superseding note |
| `docs/expansions/wave*/…` | intake notes | per §14.4 |

No ledger edit happens before the corresponding phase is green.

---

## Appendix E — Test fixture inventory

| Fixture | Purpose | Location rule |
|---|---|---|
| ledgers | deterministic balance paths | near the ledger tests |
| merchant surface | opt-in flag absent/present | reuse existing merchant fixtures |
| black market | stock, heat, purity | extend existing Plan 211 fixtures |
| routes | slots, counterparties, edges | new bounded fixture |
| migration | factions, regions, phases | new bounded fixture |

All fixtures obey `docs/testing/FIXTURE_POLICY.md` and use authority data where
the policy requires it.

## Appendix F — Recheck conditions for this plan

- If any symbol `FundsLedger` appears in source before signature, re-audit: the
  plan was written assuming absence.
- If DEC-05's verdict changes, F13-B's wording must change.
- If the XP-08 design is amended by a newer document, F13-E/F must be re-issued
  against the newer text (a plan is not proof a design still stands).
- If `SaveSectionRegistry` count mechanisms change, Phase 1's generator steps
  change.
- If Plan 5 lands the expansions first, their intake notes (§14.4) must still be
  applied to keep boundaries truthful.

**End of UNBLOCK-02.**---

## Appendix G — Expansion-wave economy hook analysis (all 20 plans)

This appendix is the intake guard: it tells Plan 5 which of the untracked
expansion bibles touch economic seams, and exactly what boundary each needs so
that no expansion duplicates the funds/trade authority this plan defines. The
analysis is based on the wave indexes and the plans' stated authority splits;
each is re-verified at intake.

### G.1 Direct economy contact (needs a boundary note before intake)

| Expansion | Stated seam | Boundary required |
|---|---|---|
| 17 · The Long Evening | culture/leisure, vinyl, hobbies | Festivals consume goods and labor; no chits in leisure loops; gambling harm routes to existing systems; no new currency. Wave-1 funds surface list (F13-D) excludes leisure. |
| 25 · The Iron Road | rail freight, schedules, rail towns, regional commons | Freight is not a player trade route. If a freight tariff ever needs a medium, it uses `FundsLedger` with a new closed reason key; it may not build a freight ledger. Player routes remain XP-08's scheduled caravans. |
| 26 · The Common Table | kitchen, rationing, preservation, food culture | Food is never a currency; rationing stays `ResourceRationingSystem`; menu pricing, if any, reads `MarketSystem`, not the ledger. Deprivation routes through `NeedsSystem.Modify`. |
| 21 · The Grid | load politics, fuel chains, storage doctrine | Power/fuel accounting is not funds accounting. No chit pricing of grid capacity; fuel trade (if any) is goods-based or route-based through the sealed owners. |
| 18 · The Underneath | mining, ore, deep geology | Ore/goods trade may use existing goods paths; no mining-scrip; heritage grants no economic power. |
| 30 · The Press | paper, print, publications, notices | Publications are informational; corvid/postal fees, if proposed, are goods or funds through the canonical ledger with an authored key; print never mints funds. |

### G.2 Indirect contact (no boundary note needed, but mentioned for completeness)

| Expansion | Why no note | 
|---|---|
| 12 · Second Generation | generational content; inheritance routes through Plan 206's sealed legacy flow, not funds |
| 13 · Faithful | belief/ritual; offerings use goods through canonical inventory |
| 14 · Above the Ash | aviation/sky trade: "sky trade" is aerial caravan-style content; if it ever settles, it uses the same route contract model, but wave 1 does not include it |
| 15 · Deep Root | agriculture/livestock; produce is goods |
| 16 · Rebuilt Body | body schema (Plan 1's territory); prosthetics are crafted goods |
| 19 · Bitter Air | hazards; decon consumes goods and power |
| 20 · Quiet Hand | espionage; informant payments, if authored, are goods or authored funds rewards with a closed key; no second ledger |
| 22 · Clean Flow | water quality; water is not currency |
| 23 · Alarm | fire/evacuation; supplies are goods |
| 24 · Long Goodbye | care/grief; funerals consume goods |
| 27 · Thread | garments are goods and equipment |
| 28 · Lesson | schooling consumes time/skill, not funds |
| 29 · Glass | optics are crafted goods |
| 31 · Kiln | masonry is goods and shelter upgrades |

### G.3 The three rules every expansion must obey

1. **One ledger rule:** any funds use routes through `FundsLedger` with a closed
   reason key; no expansion introduces a parallel wallet, scrip, token, ticket,
   coupon, or credit item without a new signed schema decision.
2. **Goods-first rule:** an expansion's default settlement is goods; funds are
   opt-in and must be justified by a player-visible reason beyond "money is
   convenient".
3. **No power-currency rule:** no expansion may make its central resource
   (power, water, food, ore, glass, print, data) purchasable with funds in a way
   that bypasses the expansion's own effort loop.

---

## Appendix H — Open product questions this plan deliberately leaves open

These are product calls, not blockers; each is safe to answer after F13-A ships:

1. **Starting funds:** does a new campaign start at 0 chits, or does the
   difficulty preset grant a small starting balance? Proposal: 0 by default,
   difficulty preset may authorize a grant through the existing XP-01
   starting-bonus path.
2. **Fence economics:** is fencing always worse than honest sale for clean
   goods (proposal yes) and the only option for flagged goods (proposal yes)?
3. **Route exclusivity feel:** does tier-4 exclusivity read as "our road is
   special" or "we cornered a market"? Both are acceptable; the prose must
   choose one tone per route.
4. **Migration visibility:** should population shifts be visible as numbers,
   words, or only as market effects? Proposal: one restrained map note plus a
   market consequence; no population numbers.
5. **Purity display:** should the tier always be displayed or only after
   appraisal? Proposal: display the band always; appraisal improves accuracy of
   the band, not its existence.
6. **Funds in the briefing:** does the daily briefing mention funds? Proposal:
   only on authored events (a route lost, a debt due), never a daily balance
   line.
7. **Chit iconography:** no real-world currency symbols; if a glyph is made, it
   is fictional and text-labeled.

---

## Appendix I — Re-verification checklist before any signature

Run this list immediately before presenting the bundle; if any item fails, the
decision lines must be re-issued against new evidence:

- [ ] `grep -rn "FundsLedger"` still empty.
- [ ] `DEC-05` still SIGNED; restock tests still 6/6.
- [ ] XP-04/XP-08 proposal text still matches §2 and §4 of this plan.
- [ ] C3 decision still lists 192/199 HOLD with the same conditions.
- [ ] Black-market settlement tests still green on the current HEAD.
- [ ] Caravan owners still exist with the same names.
- [ ] `SaveSectionRegistry` still the single section authority.
- [ ] No active claim holds the files in Appendix B.
- [ ] The expansion wave indexes still state the no-currency boundaries.
- [ ] `AGENTS.md` still lists F13 in the decision-blocked set (if it has been
      removed, this plan's premise has changed).

---

## Closing statement

F13 is the cheapest high-leverage signature in the queue: one integer ledger,
two opted-in surfaces, one optional allocation amendment, and two product
statements about routes and migration. Its cost is a bounded Core class and a
save section; its value is an entire pillar chain — XP-04, XP-08, EN-03's gate,
C3's last two holds, and clean boundaries for six expansions. The restock half
of the controversy is already resolved by DEC-05; this plan's job is to stop
re-litigating it and to sign the part that was never signed.

Recommended first action: sign F13-A, F13-B, F13-D, execute Phases 0–3, then
return for F13-E/F/G with the XP-08 phase evidence in hand.

**End of UNBLOCK-02 appendices.**
---

## Appendix J — Signature sheet (copy into the decision packet)

```text
[ ] F13-A FundsLedger canonical authority ............... signed ____ / declined ____
[ ] F13-B restock ledger reconciliation (DEC-05) ........ ratified ____ / declined ____
[ ] F13-C richer restock allocation (optional) .......... signed ____ / declined ____
[ ] F13-D wave-1 opt-in surfaces ........................ black market + holdfast ____ / other ____
[ ] F13-E XP-08 Plan 192 route contracts ................ signed ____ / declined ____
[ ] F13-F XP-08 Plan 199 seasonal migration ............. signed ____ / declined ____
[ ] F13-G tier-4 exclusive-goods discipline ............. signed ____ / declined ____
```

### What to do first

Sign F13-A, F13-B, and F13-D in one session; execute Phases 0–3; return for
F13-E/F/G with the XP-08 phase evidence in hand. Do not authorize EN-03 before
Phase 2 lands. Do not execute the richer restock allocation without F13-C.
Record every line in `DECISION_REGISTER.md` as new rows (proposed
DEC-25..DEC-31) and add the C3 192/199 addendum only after F13-E/F execute.

### What this plan refuses to do

- It refuses to add money as an inventory item, a per-faction purse, or a
  parallel wallet.
- It refuses to execute the XP-04 richer restock design on the strength of
  DEC-05 (which signed ordering-only).
- It refuses to build a second logistics layer for routes, or a second
  migration model for humans.
- It refuses to authorize EN-03 ahead of the funds ledger.

**End of UNBLOCK-02.** This document is a proposal to release blocked plans; it
does not execute, claim, or certify any of them. The next action belongs to the
foreman.
