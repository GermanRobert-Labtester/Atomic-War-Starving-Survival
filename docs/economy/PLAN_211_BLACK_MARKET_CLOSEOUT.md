# Plan 211 — Black Market & Underworld Syndicates: Closeout

**Status:** COMPLETE · **Date:** 2026-09-13 · **Action surface sealed:** 2026-09-17 · **Batch:** `PLANS-210-213-FLAGSHIP-ECONOMY`

## Delivered

| Layer | Artifact |
|---|---|
| Core | `Assets/Ashfall.Core/Economy/BlackMarketSystem.cs`, `BlackMarketInventoryCatalog.cs` |
| Data | `Assets/StreamingAssets/Data/black_market_inventory.json` (3 syndicates, 7 entries) + 2 syndicate lore rows in `faction_lore.json` (D4) |
| Host | `src/Host/BlackMarketHostSession.cs`, `src/Host/BlackMarketSaveStore.cs`, `src/Main.BlackMarket.cs` |
| Save | `SaveSectionRegistry` row (`economy`) + `black_market_save.json` (bounty snapshot rides additively — §171.18) |
| RNG | `black_market_stock/bounty/debt_event` streams (snake_case-gated, fork-per-day) |
| UI | `src/UI/BlackMarketPanel.cs`, route `black_market` (Expanded; Buy/Sell/Take Loan/Repay; explicit due dates and text disabled reasons; hidden internals never shown) |
| Day owner | `underworld_market` (phase 4 — sorts after `debt_ledger`) |
| Tests | Core 19/19 · wiring 3/3 · scenario B in 6/6 · Wave 8 settlement 18/18 · panel contracts 3/3 |

## Contract guarantees

- **No duplicate bounty ledger (R3).** Bounties escalate through the canonical `FactionBountySystem`; `FactionBountySystemState` persists via the additive `black_market` snapshot until a dedicated store exists.
- **No second price authority (R6).** Illicit prices = canonical `MarketSystem` value × risk premium × scarcity response × bounded trust discount, hard-floored above canonical +25% — round-trips strictly lose value (tested).
- **Stock never rerolls (R4).** Daily-persistent snapshots; same-day reopen returns the persisted lines; day advance refreshes via the forked RNG stream.
- **Real faction IDs (D4).** Syndicates resolve through `FactionStandingIdResolver`; new ids authored in `faction_lore.json` (data-integrity resolver clean).
- **Debt is idempotent.** One active loan per syndicate; overdue fires exactly once (fired-key ledger); trust −30 / heat +25; save/restore never duplicates collection events (scenario B).
- **Atomic transactions.** Access-tier, stock, funds, and inventory preflight precede the rollback-safe composed command. A failed currency or goods leg restores black-market, wallet, and inventory state.
- **Legacy defaults.** Undiscovered contacts, zero debt, empty stock.

## Wave 8 Part 2 C1 action-surface addendum

The previously unsigned funds/goods seam was signed and implemented on 2026-09-17. The full evidence bundle is under `docs/plans/wave8_part2/`.

| Action | Policy owner | Currency owner | Goods owner | Settlement rule |
|---|---|---|---|---|
| Buy | `BlackMarketSystem` | `HoldfastTradeSession.Value` debit | canonical `Inventory` grant | ceiling-rounded total; all legs roll back on failure |
| Sell | `BlackMarketSystem` | `HoldfastTradeSession.Value` credit | canonical `Inventory` removal | floor-rounded total; all legs roll back on failure |
| Take Loan | `BlackMarketSystem` | `HoldfastTradeSession.Value` credit | none | whole units; existing trust and due-date behavior |
| Repay | `BlackMarketSystem` | `HoldfastTradeSession.Value` debit | none | whole units; existing debt/trust behavior |

No stash, second wallet, inventory cache, save section, or new heat/trust rule was added. Opening or refreshing the panel remains read-only; daily stock still advances only through the underworld day owner. All seven authored black-market item IDs now resolve through the canonical item catalog, including the additive `diamond` definition.

### Wave 8 verification

- Settlement commands and rollback: 18/18.
- Panel authority/lifecycle source gates: 3/3.
- Economy directory: 184/184 after one independently proven stale Plan 14 quote assertion was rematched to Plan 56 regional-supply truth.
- Core and Godot host builds: 0 warnings, 0 errors.
- Data integrity: PASS, 332 catalogs, 0 errors.
- Panel lifecycle: 17/17; UI accessibility: 5/5.
- `black_market_default` snapshot: MATCH at 1280×800.
- Save round-trip and debt overdue exactly-once: PASS.

The repository-wide snapshot invocation still reports 31 unrelated baseline drifts, and the existing D3 RID/resource shutdown signature remains open for its lifetime-triage package. Neither changes the black-market action contract or its matching target.
