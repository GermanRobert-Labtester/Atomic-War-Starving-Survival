# Plan 211 — Black Market & Underworld Syndicates: Closeout

**Status:** COMPLETE · **Date:** 2026-09-13 · **Batch:** `PLANS-210-213-FLAGSHIP-ECONOMY`

## Delivered

| Layer | Artifact |
|---|---|
| Core | `Assets/Ashfall.Core/Economy/BlackMarketSystem.cs`, `BlackMarketInventoryCatalog.cs` |
| Data | `Assets/StreamingAssets/Data/black_market_inventory.json` (3 syndicates, 7 entries) + 2 syndicate lore rows in `faction_lore.json` (D4) |
| Host | `src/Host/BlackMarketHostSession.cs`, `src/Host/BlackMarketSaveStore.cs`, `src/Main.BlackMarket.cs` |
| Save | `SaveSectionRegistry` row (`economy`) + `black_market_save.json` (bounty snapshot rides additively — §171.18) |
| RNG | `black_market_stock/bounty/debt_event` streams (snake_case-gated, fork-per-day) |
| UI | `src/UI/BlackMarketPanel.cs`, route `black_market` (Expanded; explicit due dates; hidden internals never shown) |
| Day owner | `underworld_market` (phase 4 — sorts after `debt_ledger`) |
| Tests | Core 19/19 · wiring 3/3 · scenario B in 6/6 |

## Contract guarantees

- **No duplicate bounty ledger (R3).** Bounties escalate through the canonical `FactionBountySystem`; `FactionBountySystemState` persists via the additive `black_market` snapshot until a dedicated store exists.
- **No second price authority (R6).** Illicit prices = canonical `MarketSystem` value × risk premium × scarcity response × bounded trust discount, hard-floored above canonical +25% — round-trips strictly lose value (tested).
- **Stock never rerolls (R4).** Daily-persistent snapshots; same-day reopen returns the persisted lines; day advance refreshes via the forked RNG stream.
- **Real faction IDs (D4).** Syndicates resolve through `FactionStandingIdResolver`; new ids authored in `faction_lore.json` (data-integrity resolver clean).
- **Debt is idempotent.** One active loan per syndicate; overdue fires exactly once (fired-key ledger); trust −30 / heat +25; save/restore never duplicates collection events (scenario B).
- **Atomic transactions.** Access-tier + stock preflight before any mutation.
- **Legacy defaults.** Undiscovered contacts, zero debt, empty stock.
