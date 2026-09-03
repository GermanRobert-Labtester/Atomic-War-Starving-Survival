# Plan 56 Phase 6 — Provenance-Aware Waystation Resupply (Live Host)

> Wires the `WaystationNetworkSystem` shortage policy into the live host and
> surfaces import-lapse in the waystation panel. Snapshot pinning deferred —
> see §5 (pre-existing environment blocker, documented with evidence).

## 1. Session ownership

`WaystationHostSession` now owns the trade-stock network:

- `WaystationNetworkSystem? Network` — attached via
  `AttachNetwork(network, catalog, isSuppliesShort)`, which binds the
  shortage policy (goods catalog + live `Market.IsSuppliesShort()`).
- `TickDaily` chains `Network?.TickDay()` — the network's 7-day provenance-
  aware resupply runs on the same daily cadence as the shelter waystation.

**Main wiring** (`SetupWaystation`): constructs the network (default
waystation catalog), binds the policy against the live economy session's
catalog and market (null-safe closure — the economy session may bind later).

## 2. Panel surfacing

`WaystationNetworkPanel` gains a **REGION TRADE STOCK** section (hidden when
no network is attached): per station, one row per stock item with

- `[stocked]` — present in availability, or
- `[import lapsed — market short]` — present in the station definition but
  dropped by the shortage resupply (computed via the new
  `WaystationNetworkSystem.LapsedImports(def, station)` helper).

Text-first per Plan 14: the lapse state is words, never a color-only signal.
`Bind` defers its first refresh (`CallDeferred`) so pre-`_Ready` snapshot
fixtures bind safely — same pattern as the market panel.

## 3. Core tests (`Plan56Phase6Tests`, 5)

- Shortage resupply keeps local + general stock, lapses pure imports
  (two-station fixture: industrial_belt / ash_flats).
- `LapsedImports` reports exactly the definition-minus-availability ids.
- Normal market: full stock, zero lapse.
- Unbound network: legacy resupply byte-exact.
- State capture/restore preserves the filtered stock.

## 4. Snapshot pinning — DEFERRED (environment blocker)

`WaystationNetworkPanel` is **not** added to `SnapshotHarness.Targets` in
this phase. Evidence:

1. `snapshots/shelter_decor_default.png` is **corrupt** in this repository
   (57,182 bytes, no PNG magic, truncated before IEND) — pre-existing, not
   Plan 56 scope. `SnapshotOrchestrator.EvaluateAgainstGolden` calls
   `Image.LoadFromFile` on every golden; decoding this file raises a mono
   **SIGILL** (`Godot.Image.LoadFromFile` → "Not a PNG file" → crash dump),
   after which the running diff **silently skips one target** (verified: 32
   targets, 31 result lines, the waystation target omitted with no output).
2. The orchestrator mounts panels on background threads
   (`propagate_notification` cross-thread errors throughout the run); UI
   work from fixture threads is SIGILL-prone on this mono runtime.

Fixing the corrupt golden (regenerate with a rendered session) and the
thread discipline is a separate maintenance task — the panel, session
ownership, and lapse semantics are fully implemented and unit-tested at the
Core layer; the snapshot pin lands with that cleanup.

## 5. Gates

```
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   # all green (incl. 5 phase-6)
dotnet build Ashfall.csproj                                 # 0 errors
--data-integrity-selftest / --bridge-selftest               # PASS
--economy-selftest / --caravan-selftest                     # PASS
```
