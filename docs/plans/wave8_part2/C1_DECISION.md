# Wave 8 Part 2 C1 — Settlement Decision

## Decision ID

`WAVE8-PART2-C1-BLACK-MARKET-SETTLEMENT`

## Current verified fact

At commit `276a359872facedc24fc6632e9156a688c638d7a`, `BlackMarketSystem` owned black-market policy and state but did not settle player funds or goods. `HoldfastTradeSession.Value` was the live trade-value wallet, and `Inventory` was the canonical player-goods authority with an atomic transaction boundary. `BlackMarketPanel` was read-only. No canonical stash owner existed, and black-market debt already persisted through the existing Plan 211 save path.

## Why implementation cannot proceed safely

Wiring actions without a decision could have created a second wallet, panel-owned item grants, half-committed transactions, or a new stash/save scope. The missing policy was which existing owners execute each leg, the ordering and rollback rule, and whether immediate delivery is intended.

## Existing owners that constrain the decision

| Owner | Current responsibility | Relevant API/state | Must not own |
|---|---|---|---|
| `BlackMarketSystem` | Access, pricing, stock, debt, heat, trust, overdue effects | buy/sell/loan/repay preflights and state | Player wallet or canonical inventory |
| `HoldfastTradeSession` | Legitimate trade-value wallet and persistence | `Value`, debit/credit, capture/restore | Black-market policy or goods |
| `Inventory` | Canonical player items and atomic item transactions | `ValidateTransaction`, `TryExecuteTransaction` | Currency, market pricing, debt |
| `BlackMarketHostSession` plus settlement adapter | Compose commands and return typed results | host action methods/events | Policy, duplicate state, persistence |
| `BlackMarketPanel` | Render state, reasons, and route input | preview/result reads and host commands | Prices, eligibility, wallet or item mutation |

## Options

### Option A — Immediate canonical-inventory delivery

- **Behavior:** Buy delivers the canonical item immediately; Sell removes it immediately. Loan/repay settle the existing wallet.
- **Owner:** `BlackMarketSystem` retains policy; `HoldfastTradeSession` moves value; `Inventory` moves goods; a stateless coordinator composes them.
- **Data/save consequence:** Add the missing canonical `diamond` definition. No new save section; each existing owner persists its existing state.
- **UI consequence:** Expose Buy/Sell/Take Loan/Repay with authoritative disabled reasons and returned outcomes.
- **Compatibility consequence:** Existing headless overloads remain available; existing heat/trust and overdue policy remain unchanged.
- **Test consequence:** Command success/failure, second-leg rollback, no-reroll, save/restore, exactly-once overdue, UI lifecycle/a11y/snapshot.
- **Retirement/defer consequence:** Closes the read-only action blocker.

### Option B — Existing stash/location delivery

- **Behavior:** Goods move through a stash before player inventory.
- **Owner:** Requires a real stash/location authority.
- **Data/save consequence:** Would require a new or newly identified persisted delivery owner.
- **UI consequence:** Must explain destination and collection state.
- **Compatibility consequence:** Not viable at current `HEAD`; no such owner or state was found.
- **Test consequence:** Delivery persistence and collection atomicity would be mandatory.
- **Retirement/defer consequence:** Hold until a canonical stash owner is approved.

### Option C — Keep the panel read-only

- **Behavior:** Continue displaying prices, stock, debt, heat, and trust without player actions.
- **Owner:** Existing owners unchanged.
- **Data/save consequence:** None.
- **UI consequence:** Remove wording that implies actions are pending implementation.
- **Compatibility consequence:** Safest zero-code disposition but leaves no player-operable black-market trade.
- **Test consequence:** Read-only/no-reroll coverage only.
- **Retirement/defer consequence:** Close as `DECIDED-DEFERRED`.

## Recommended option

Option A. It reuses the live wallet and inventory owners, needs no new persisted state, and turns existing black-market policy into player-operable commands through a narrow stateless seam.

## Non-options

- A black-market-specific wallet or item store.
- Panel-side price, eligibility, debt, heat, trust, or restock logic.
- Restocking on open or refresh.
- A new stash or save section without a separately signed owner.
- Partial commits or best-effort compensation exposed as success.
- New heat/trust effects attached to UI actions.

## Signature

**Chosen option:** Option A — immediate delivery to canonical inventory.<br>
**Signer:** User/foreman<br>
**Date:** 2026-09-17<br>
**Conditions:**

1. Currency debits and credits use `HoldfastTradeSession.Value`.
2. Goods grants and removals use canonical `Inventory` transactions.
3. Buy uses ceiling rounding; Sell uses floor rounding. Loans and repayments use whole value units.
4. Black-market state is staged first, then the wallet leg and goods leg run inside rollback-safe owner boundaries. Any rejected or throwing leg restores every owner, so no half-transaction is observable.
5. Existing Core heat/trust/debt/overdue effects remain unchanged.
6. Read-only panel refresh is side-effect free and same-day reopen does not restock or consume RNG.
7. No stash and no new persisted section are introduced.
