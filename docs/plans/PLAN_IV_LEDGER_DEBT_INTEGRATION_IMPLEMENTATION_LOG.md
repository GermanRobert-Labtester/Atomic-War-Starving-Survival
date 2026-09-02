# Plan IV — Ledger Debt Consequences, Trade Credit & Headless Integration — Implementation Log

**Plan:** ASHFALL Flagship Integration Plan IV (F1/F2/F3)
**Status:** COMPLETE — all gates green
**Commits:** `54472ba3` (Core systems + tests) · `0657daa8` (host wiring + v5 + data; bundled by the concurrent sync stream together with the Plans 46–49 work) · `c48b29c3` (F3 test pins + Plans 46–49 compile repairs)

## Phase 0 — Architecture reconnaissance

Status: PASS

Recorded ownership graph:

```text
ledger construction owner:      src/Host/ExpansionHostSession.cs (Ledger property, ctor)
template catalog owner:         DebtTemplateCatalogLoader (was UNWIRED in host before this task)
campaign day source:            _core.Clock.Day (HoldfastRuntimeSession.Day projects the same clock)
save owner:                     ExpansionHubSave / ExpansionHubSaveCodec (v4 → v5)
session teardown path:          ExpansionHostSession.ShutdownDebtIntegration() (new)
standing authority:             Ashfall.Core.YearOfAsh.FactionWarSystem.ModifyStanding (clamp ±100, hostile ≤ −50)
embargo authority:              FactionEmbargoLedger (new — day-derived windows, source-id dedupe)
raid/bounty authority:          Ashfall.Core.Muster.IronRaidersSystem.ProvokeRaid (deterministic)
inventory authority:            InventoryHostSession / Inventory container (AddById/RemoveById/CountById)
labor authority:                bounded obligation ledger inside DebtConsequenceHostBridge (DutyRoster's fixed 5-role wall chart cannot host bounded debt labor — documented divergence)
trade insufficient-funds path:  HoldfastTradeSession.Buy → HoldfastTradeFailure.InsufficientFunds
```

Baseline: Core tests 6077/6077 PASS; host build broken by a concurrent stream's untracked mid-edit `Main.Plans46_49.cs` (pre-existing, not this task's — repaired only after that stream committed it broken; see divergences).

## Stage 1 — Dispatcher persistence & idempotency

Status: PASS

- `DebtDispatcherState` (serializable fired-set) + `CaptureState/RestoreState` on `DebtConsequenceDispatcher`.
- Stable identity: `debtorId@signedDay:consequenceId` — no counters, no RNG, deterministic across save/load.
- Typed `OnStandingPenalty(DebtConsequence, string, DebtContract)` now carries the FIRED consequence (escalated deltas apply exactly; the old signature re-derived the delta from the base template and applied the wrong value after escalation).
- Collateral falls back to the template principal at the lent quantity when the consequence authors no `collateralItemId` (all shipped consequences author none — the pledged good is the intended seizure).
- `forgiveness` effect now calls the canonical `LedgerDebtSystem.ForgiveContract` — a real state transition, not an event.

## Stage 2 — Host consequence adapters (F1)

Status: PASS

`DebtConsequenceHostBridge` (Core, host-agnostic):

| Consequence | Canonical target | Side effect | Reload protection |
|---|---|---|---|
| standing | FactionWarSystem.ModifyStanding | authored delta, clamped | dispatcher fired-set |
| embargo | FactionEmbargoLedger.TryAddEmbargo | day-derived [start, start+days) window | fired-set + source-id dedupe |
| bounty/raid | IronRaidersSystem.ProvokeRaid (activate on first) | +1 raid per consequence stage | fired-set |
| collateral | inventory delegates (all-or-nothing; shortfall logs and takes nothing) | canonical remove | fired-set |
| labor | bounded obligation (startDay/endDay, released at endDay) | survivor bound | fired-set + source-id dedupe |
| forgiveness | LedgerDebtSystem.ForgiveContract | balance cleared, no payment | fired-set + ledger state |
| telemetry | ILog structured line (campaignDay/debtor/template/creditor/consequence/effectType/dispatchId) | diagnostic only | never the idempotency store |

Host wiring (`Main.DebtCredit.cs`): `EnsureDebtConsequenceIntegration()` composes the bridge against `_yearOfAsh.FactionWar`, `_muster.IronRaiders`, the shared `_inventory.Inventory`; `ExpansionHostSession` owns ledger + single catalog instance + exactly one dispatcher (`ShutdownDebtIntegration` detaches); phase-4 `DebtLedgerDayOwner` ticks `Ledger.TickDaily` + `bridge.TickDaily` each campaign day — debt ages in the real campaign.

## Stage 3+4 — Trade credit (F2 backend)

Status: PASS

`TradeCreditCoordinator` (Core): `CreditOffer` is a projection of the debt template (never contract state); deterministic catalog-order template matching with `ItemAliases` canonicalization; gates = hostile standing (canonical `FactionWarSystem.HostileStandingThreshold`, −50 exactly is ineligible), same-creditor unresolved debt (signed && !paid && !forgiven; paid/forgiven never block), active embargo, principal relevance, stale-offer revalidation on acceptance. Acceptance = ledger two-reading ceremony → grant → sign, with compensating revoke on sign failure and fail-before-ink on grant failure. Reload double-disbursement is structurally prevented by the same-creditor gate.

## Stage 5 — Trade UI

Status: PASS (host-verified build + holdfast selftest; snapshot not produced)

`HoldfastTerminalPanel`: insufficient-funds refusal shows the full offer in plain text (creditor, principal+quantity, term, rate, total owed, forfeit, consequence summary, explicit "This is debt"), a disabled-until-offered `ACCEPT CREDIT` button, and `PressAcceptCredit()` as the only signing path; any other action declines. No hover-only terms, no color-only cues. Full accessibility snapshot remains open (see remaining).

## Stage 6 — Headless oracle (F3)

Status: PASS — 57/57 checks, verified through `--ledger-debt-selftest` and inside `--expansions-selftest` (499/499).

Covers: catalog load (0 errors, 15 templates, 10 consequences), template→consequence and escalation FKs with per-reference diagnostics, escalation cycle walk, rations scenario (forfeit→standing −5 on `faction_supply_corps` exactly once, payload identity), dispatcher fired-state JSON roundtrip with zero redispatches, scavenger collateral (15 × dried_rations, no standing), embargo payload (creditor_faction/14d), bounty+raid escalation (exactly 2), fixture-forced `forgiveness_rare` (contract forgiven, no payment consumed).

## Stage 7 — Expansion master registration

Status: PASS (already registered; call site now forwards the data directory).

## Stage 8 — Full regression closure

Status: PASS

```text
dotnet build Ashfall.Core.Tests   → 0 warnings, 0 errors
dotnet test                       → 6124/6124 PASSED (baseline 6077 + 47 new/updated)
dotnet build Ashfall.csproj       → 0 warnings, 0 errors
--data-integrity-selftest         → PASS 180 catalogs, 0 errors, 0 warnings (9575 ids)
--bridge-selftest                 → PASS
--ledger-debt-selftest            → PASS 57/57
--expansions-selftest             → PASS 499/499 ALL EXPANSIONS GREEN
--holdfast-selftest               → PASS (terminal incl. credit UI additions)
```

## Divergences from the plan

1. **Trade surface.** `TradeScreenPresenter`/`CaravanAtomicTrader` have no host-side execution sink and no funds concept; the canonical insufficient-funds path is `HoldfastTradeSession.Buy` behind the Holdfast terminal. Credit integrates there. The five creditors were added to `holdfast_factions.json` and all 15 principals to `holdfast_items.json` (+ `ItemAliases` mappings) so every template is reachable through ordinary trade — not just the required three.
2. **Labor authority.** `DutyRosterSystem` is a fixed 5-role wall chart with auto-assign semantics; a bounded debt obligation would break it. The bridge owns a persisted, endDay-bounded obligation ledger instead (documented here as the sanctioned home).
3. **Bounty mapping.** `bounty` and escalated `raid` consequences both map to `OnBountyRequested` → one raid provocation per authored stage (2 for a bounty→raid chain). Deterministic and catalog-driven; a dedicated raid scheduler remains future work.
4. **Concurrent stream.** `Main.Plans46_49.cs` was committed broken mid-edit (wrong field `_inventoryHost`, `??` across `ICampaignRngManager`/`ISeededRng`, `Survivors.HostSession`); repaired with canonical APIs (`SetupInventory()`/`_inventory`, `Rng.Fork(stream)`, `_survivors.Needs`). Commit `c48b29c3`.

## Remaining known limitations

- Credit-offer accessibility snapshot at target resolution not yet rendered (structural accessibility checklist implemented; snapshot run pending the UI vision-QA lane).
- Embargo scope is authored but applies creditor-wide; per-scope filtering (e.g. trade_offers vs. credit only) is future data/runtime work.
- `HandlePaid` (embargo lift / standing restoration on late payment) remains a stub, as before.
