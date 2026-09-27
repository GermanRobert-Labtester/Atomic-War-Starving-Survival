# PLAN-CARAVAN-ITEM-VALUE — Canonical Item Value Replaces the Caravan's Hardcoded Price Table
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **ARCHIVAL DIRECTIVE (MANDATORY): This plan is FULLY INTEGRATED. Move/keep this file in the integrated plans folder `docs/plans/integrated/<category>/`. It must never remain in `docs/plans/` as open work, and it must not be re-executed or reopened without a new foreman signature.**
> **INTEGRATION STATE: FULLY INTEGRATED — authored data bound · existing owner seam used · live composition site wired · CLI probe green · focused tests green · no parallel authority · no new save section or day event.**

## Integrated evidence

The caravan owner's designed `SetItemValueResolver` seam now reads the canonical item catalog (`tradeValue`), replacing its fallback literal table — 6 of 7 of those ids do not exist in `items.json` (`sterile_gauze`, `anesthetic_ether`, `surgical_scalpel`, `chemical_filter`, `electrical_wire`, `copper_fuse`; only `sandbags` does). Bound at the live construction site in `src/Main.AdvancedShelterSystems.cs`.

This removed duplicated gameplay values rather than adding a price authority: crisis, export-surplus and favoured-status multipliers stay exactly where they were, and a resolver miss still falls back safely instead of quoting free goods.

Probe `--caravan-item-value-selftest`: 9/9. Pre-existing `CaravanTradeNetworkTests` (6 tests) remains green.

## Original plan body (preserved for the record)

> **Package:** `economy`
> **Category:** CARAVAN-ITEM-VALUE
> **Plan type:** bind authored game data to the existing, designed seam on its live owner.
> **Date of evidence:** 2026-09-26, branch `integration/all-latest-2026-09-24`
> **Claim:** `claim-quad-i-economy-2026-09-26`

## 1. Objective
Make one already-designed Core bind seam actually receive its authored data, so
JSON remains authoritative (AGENTS.md rule 3) without creating a second owner.

## 2. Current evidence (verified in source, this session)
- `CaravanTradeNetworkSystem.SetItemValueResolver(Func<string,float>)` (Assets/Ashfall.Core/Economy/CaravanTradeNetworkSystem.cs:524) has **zero callers**.
- Without it, `GetCanonicalItemValue` (:540) falls through to a **hardcoded id switch** (`anesthetic_ether => 25f`, `sterile_gauze => 6f`, `surgical_scalpel => 15f`, `chemical_filter => 20f`, `electrical_wire => 8f`, `copper_fuse => 12f`, `sandbags => 5f` …).
- Measured against the authoritative item catalog: **6 of those 7 ids do not exist** in `Assets/StreamingAssets/Data/items.json` (724 items, only `sandbags` exists), so those goods price off stale literals instead of the authored `tradeValue` field (`ItemDefinitions.cs:110`).
- The owner is live: `src/Main.AdvancedShelterSystems.cs:49` constructs `_caravanTradeNetwork` from the authored route catalog, the live inventory, and a forked campaign RNG.

## 3. Design decision
Bind a resolver that reads the **existing** inventory item authority (`_inventory.Catalog.Get(id)?.tradeValue`) and pass it through the owner's own seam. This *removes* duplicated gameplay values instead of adding a second price source; caravan multipliers (export surplus, crisis, favoured-status) stay exactly where they are.

## 4. Non-goals
* No new save section, no new day-event heartbeat, no new Core system, no new catalog.
* No gameplay-rule invention: only data that already exists in `Assets/StreamingAssets/Data/`.
* No UI surface is added unless the command already exists on the owner.

## 5. Implementation surface
- `src/Main.PackageIBindings.cs` — `BindCanonicalCaravanItemValue()`, called after `_caravanTradeNetwork` is constructed.

## 6. Verification (bounded)
* One headless host probe: `--caravan-item-value-selftest`
* One focused engine-free Core suite: PLAN_CARAVAN_ITEM_VALUE_HOST_INTEGRATION.md0
* Adjacent gates only where this package can move them.

## 7. Acceptance
Authored rows are reachable through the live owner; the probe and suite pass; the
existing owner remains the single authority; no duplicate ledger/cache is created.
