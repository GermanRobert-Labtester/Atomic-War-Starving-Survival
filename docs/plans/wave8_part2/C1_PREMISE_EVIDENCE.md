# Wave 8 Part 2 C1 — Premise Evidence

**Task:** Unblock black-market trade actions<br>
**Premise commit:** `276a359872facedc24fc6632e9156a688c638d7a`<br>
**Verified:** 2026-09-17<br>
**Path claim:** `claim-wave8-part2-c1-black-market-actions-2026-09-17`

## Blocker at entry

Plan 211 had a deterministic, persisted black-market policy owner, daily stock, pricing, debt, heat, and trust. `BlackMarketPanel` displayed those read models but exposed no Buy, Sell, Take Loan, or Repay command. `BlackMarketSystem.Buy`, `Sell`, `TakeLoan`, and `RepayDebt` changed only black-market state; no signed contract said which owner moved Holdfast value or canonical goods. The deferred row in `INTEGRATION_PLANS.md` named this exact authority gap.

## Current owner paths found before editing

| Concern | Existing owner/path | Entry seam verified |
|---|---|---|
| Black-market policy, stock, debt, heat, trust | `Assets/Ashfall.Core/Economy/BlackMarketSystem.cs` | `Buy`, `Sell`, `TakeLoan`, `RepayDebt`, `EnsureStockSnapshot`, `TickDaily`, capture/restore |
| Legitimate trade wallet | `Assets/Ashfall.Core/HoldfastTradeSession.cs` | `Value`, legitimate `Buy`/`Sell`, capture/restore |
| Canonical player goods | `Assets/Ashfall.Core/Inventory/Inventory.cs` | `ValidateTransaction`, `TryExecuteTransaction`, capture/restore |
| Canonical item definitions | `Assets/Ashfall.Core/ItemCatalog.cs`, `Assets/StreamingAssets/Data/items.json` | `ItemCatalog.Get` and inventory definitions |
| Black-market host adapter | `src/Host/BlackMarketHostSession.cs` | state projection and daily stock refresh; no command settlement |
| Black-market presentation | `src/UI/BlackMarketPanel.cs` | prices, stock, debt, heat, trust; no action controls |
| Route/composition | `src/Main.BlackMarket.cs` | panel construction and binding |

## Evidence that the blocker still existed

- Whole-tree action search found no second black-market panel or command route.
- The panel had no Buy, Sell, Take Loan, or Repay buttons and no settlement calls.
- Opening the panel only bound/read state; daily restock lived in the host/day path through `EnsureStockSnapshot`, not panel refresh.
- Legitimate trade already used `HoldfastTradeSession.Value` and the canonical `Inventory`; creating a black-market wallet or inventory would duplicate authority.
- Six of seven black-market `item_id` values already resolved through the canonical item catalog. The authored `diamond` stock ID did not, so immediate delivery required one additive canonical item definition rather than an alternate stash.
- No canonical black-market stash/location owner or persisted stash state existed. Adding one would have enlarged save scope beyond this package.

## Baseline behavior and constraints

- Existing Plan 211 Core tests: 19/19 passing.
- Existing black-market host wiring tests: 3/3 passing.
- Same-day stock was already daily-persistent and must remain unchanged by open, close, or refresh.
- Loan overdue processing was already idempotent through `firedEventKeys` and had to remain exactly once after restore.
- Existing heat/trust effects were already owned by `BlackMarketSystem` and were out of scope for redesign.

## Premise conclusion

The blocker was live and bounded: an unsigned funds/goods contract and missing command surface. Existing wallet, inventory, save, pricing, stock, debt, heat, and trust owners were present and reusable. The task therefore proceeded to the signed decision in `C1_DECISION.md` without creating new authority.
