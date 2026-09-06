# Plan IV — Ledger Debt Consequences, Trade Credit & Headless Integration — Implementation Log

**Plan:** ASHFALL Flagship Integration Plan IV (F1/F2/F3)
**Status:** IMPLEMENTED — debt-focused gates pass; the wrapper expansion selftest is blocked by missing imported Godot assets in this checkout.
**Verification date:** 2026-09-06

## Authority map

```text
ledger construction:       src/Host/ExpansionHostSession.cs
template catalog:           DebtTemplateCatalogLoader
campaign day:               _core.Clock.Day
save owner:                 ExpansionHubSave / ExpansionHubSaveCodec
dispatcher lifetime:        ExpansionHostSession + Main lifecycle
standing authority:         FactionWarSystem.ModifyStanding
embargo authority:          FactionEmbargoLedger
raid authority:             IronRaidersSystem.ProvokeRaid
inventory authority:        injected inventory mutation delegates
labor persistence:          DebtConsequenceBridgeState (bounded endDay records)
trade credit entry:         HoldfastTradeSession.Buy → HoldfastTerminalPanel
```

The ledger is ticked by the dedicated phase-4 debt day owner. The older narrative day owner no longer ticks the same ledger, preventing accelerated defaults and duplicate dispatch attempts.

## F1 — Host consequence wiring

`ExpansionHostSession` owns one loaded catalog and one `DebtConsequenceDispatcher`. `Main` composes `DebtConsequenceHostBridge` once, restores its state before daily simulation, and detaches it on quit, reset, and session disposal. Recomposition clears the bridge/coordinator references and does not leave subscriptions attached to the old ledger.

The dispatcher persists fired identities in `DebtDispatcherState` using `debtor@signedDay:consequenceId`. The bridge uses consequence-aware source IDs for embargo and labor records, so escalation stages cannot collide. Default side effects route through the canonical faction, embargo, raid, inventory, and bounded labor authorities. Every authored nonzero standing delta is emitted, including deltas attached to embargo, bounty, collateral, labor, raid, and forgiveness effects. `forgiveness` calls `LedgerDebtSystem.ForgiveContract`, so it clears debt without consuming repayment resources.

Failed credit transactions now call `LedgerDebtSystem.CancelDraft`; an unsigned draft cannot remain after principal transfer or signature failure.

## F2 — Trade credit

`TradeCreditCoordinator` exposes an ephemeral `CreditOffer` projection of a catalog template. It deterministically matches principal items, canonicalizes aliases, and gates offers by:

- canonical hostile standing threshold;
- unresolved same-creditor debt;
- active embargo;
- principal relevance;
- authored template active/day-window fields.

Acceptance revalidates the offer, performs the ledger’s two-reading ceremony, transfers principal through the inventory authority, signs at the current campaign day, and compensates or cancels the draft if either side fails. The Holdfast terminal presents creditor, principal, quantity, term, rate, repayment estimate, forfeit, consequence summary, and explicit debt wording. Only `ACCEPT CREDIT` signs; decline and other terminal actions leave ledger and inventory unchanged.

Reachable authored trade-credit contexts include:

| Context | Creditor | Template | Principal |
|---|---|---|---|
| food/rations | `faction_supply_corps` | `debt_supply_corps_rations` | `canned_food × 8` |
| fuel | `faction_railway_guild` | `debt_railway_guild_fuel` | `diesel_fuel × 10` |
| medical | `faction_supply_corps` | `debt_supply_corps_medical` | `medical_kit × 3` |

## F3 — Headless closure

`LedgerDebtHeadlessDemo` loads the live catalog and verifies:

- 15 templates and 10 consequences;
- template and escalation foreign keys;
- acyclic escalation graph;
- two-reading/signing ceremony;
- authored standing, collateral, embargo, bounty/raid, and forgiveness behavior;
- dispatcher fired-state JSON roundtrip with zero redispatches.

The demo is registered through `ExpansionMasterSession` and the dedicated CLI route reports **57/57 PASS**.

## Current verification

```text
focused debt tests        43 passed, 0 failed
full xUnit suite           8490 passed, 0 failed, 0 skipped
dotnet build Ashfall.csproj        0 warnings, 0 errors
--ledger-debt-selftest    PASS 57/57
--data-integrity-selftest PASS; 269 catalogs, 0 errors, 0 warnings
--content-utilization-selftest PASS; 552 catalogs, 0 orphaned
```

`--expansion-selftest` was also attempted. In this checkout it aborts while the Godot host is initializing because imported font/audio/texture resources are absent and the `user://logs` file cannot be opened; this occurs before the debt result is available. The dedicated debt selftest remains green and does not depend on those imported presentation assets.

## Scope notes

- The production trade surface is the Holdfast terminal because the older `TradeScreenPresenter`/`CaravanAtomicTrader` path has no host execution sink or funds model.
- The current duty roster has five fixed wall-chart roles, so debt labor remains a persisted, bounded obligation bridge rather than a fabricated permanent roster role.
- Embargo records are creditor-wide; finer `trade_offers` versus credit-only scopes remain future work.
- Late-payment embargo lifting and standing restoration remain outside this milestone’s existing `HandlePaid` behavior.
