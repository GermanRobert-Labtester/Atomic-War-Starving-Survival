# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# F13-C Restock Capacity Allocation Host Integration

**STATUS: APPROVED BY USER** (user authorized XP-04/F13, 2026-09-26)
**Package:** `F13C-RESTOCK-ALLOCATION-SURFACE`
**Gate authority:** `Ashfall.Core.Economy.RestockAllocationEngine` (previously 0 `src/` references)

## Bounded outcome

Give the signed F13-C engine an operational host surface. It allocates partial restock capacity
deterministically across categories (effective weight doubled below the scarcity floor) using
largest-remainder rounding, then orders items by stock/target_par with authored-order and item-id
tie-breaks. Pure static; no save section, no duplicate merchant authority.

## Delivered

- New `src/Host/HostCli.RestockAllocation.cs` — 8-check probe `RestockAllocationSelfTest`.
- `Assets/Ashfall.Core/HostCliRegistry.cs` — enum `RestockAllocationSelfTest` + descriptor
  `--restock-allocation-selftest` (alias `--restock-allocation-engine-selftest`).
- `src/Host/HostCli.cs` — host enum + parse + `PrintHelp` entry.
- `src/Main.Application.cs` — dispatch.

## Verification

Build 0 errors; `godot --headless -- --restock-allocation-selftest` → 8/8 (worked example 6/4;
scarcity-floor doubling; largest-remainder tie-break; rational item sort; determinism; zero/negative
capacity; null categories; all-zero weights). Parity 4/4; manifest 269 tests; CLI catalog 329 entries.

## Non-goals

No change to `RestockAllocationEngine`; no merchant-state mutation (the engine returns an allocation
plan; the existing merchant restock owner applies it); no save section.

## Gameplay integration (2026-09-27) — the surface is now consumed

The signed engine is now the single allocator on the canonical merchant restock
path. `ShelterBarterSystem.RestockCaravan` builds `RestockCategory` /
`RestockItemCandidate` rows from the authored caravan stock and calls
`RestockAllocationEngine.Allocate`; a capacity of 0 reproduces the legacy full
restock exactly (one `general` category, target par = authored quantity).
`MerchantCaravanDef.restock_capacity`, `CaravanStockItem.restock_category`, and
`CaravanStockItem.target_par` are additive (defaults keep stock byte-identical),
and `RestockCapacityProvider` lets a host bind a live capacity. The earlier
"existing merchant restock owner applies it / no merchant-state mutation"
non-goal is superseded: that owner is the engine's production caller.

Verification: `PlanF13RestockAllocationIntegrationTests` 4/4; pre-existing
`Plan147RestockPriorityTests` 6/6; host build 0 errors.