# §27 Follow-On Expansion 2 — Source Maintenance/Failure Events (B5–B8 continuation)

> Flagship §27: "additional source maintenance/failure events using the
> standardized power projection." Before this increment every shelter source
> degraded silently — the player discovered wear only when output dropped.
> Now each source emits a typed, edge-triggered fact; the canonical journal
> authority reports it.

## What landed

### Typed degradation/failure edges (Core, runtime latches)
| Source | Event | Trigger | Reset |
|---|---|---|---|
| Base generator (`PowerGridSystem`) | `PowerGridEventKind.GeneratorWorn` | condition crosses below the 50 degradation threshold on a burning day (output now derated) | generator service |
| Base generator | `PowerGridEventKind.FuelStarved` | fuel-starved tick (partial output) | a refueled day |
| Deep-well pump (`DeepWellSystem`) | `OnPumpWorn` | pump condition crosses below `PumpWornThreshold` (40) while pumping | pump service |
| Condenser membrane (`AtmosphericCondenserSystem`) | `OnMembraneSpent` | membrane integrity reaches 0 (hard functional failure) | membrane replacement |

- Every edge fires **exactly once per degradation cycle**; service actions
  clear the latch so a fresh wear cycle can warn again; `RestoreState`
  re-seeds each latch from the restored state — a reload never replays a
  warning (flagship §14.3 discipline, pinned by test).
- Runtime latches only — zero save-shape change; all fixtures byte-parity.

### Host wiring (presentation only — journal authority reports)
- `SetupPowerGrid` / `SetupDeepWell` / `SetupWaterCondenser` subscribe and
  write restrained journal entries ("MAINTENANCE: The generator is wearing
  out — output derated…", "The condensation membrane is spent…"), deduped by
  the edges themselves. Facts stay with the sources; the journal owns prose.

### Real bug found by the expansion tests
The condenser's original wear model (per condensed liter) decayed
**exponentially** — wear ∝ yield ∝ integrity — so a fouling membrane would
*never* reach spent in live play. Replaced with per-condensing-day wear
(linear, 200-day membrane lifetime). The test suite caught the design flaw
before production ever hit it.

## Verification

| Gate | Result |
|---|---|
| `Ashfall.Core.Tests/Shelter/SourceFailureEventTests.cs` | 7/7 PASS (new) |
| `Water/AtmosphericCondenserSystemTests.cs` (updated wear assert) | 9/9 PASS |
| `Water/DeepWellSystemTests.cs` | 10/10 PASS |
| Full `dotnet test` | **11,002 / 11,006** — the same 4 pre-existing dirty-worktree failures; zero new |
| `dotnet build Ashfall.csproj` | 0 warnings, 0 errors |

## Files changed

- `Assets/Ashfall.Core/Shelter/PowerGridSystem.cs` (+2 event kinds, latches, restore re-seed)
- `Assets/Ashfall.Core/DeepWellSystem.cs` (`OnPumpWorn` + threshold + latch)
- `Assets/Ashfall.Core/AtmosphericCondenserSystem.cs` (`OnMembraneSpent` + latch; wear-model fix)
- `src/Main.World.cs`, `src/Main.DeepWell.cs`, `src/Main.WaterCondenser.cs` (journal subscriptions)
- `Ashfall.Core.Tests/Shelter/SourceFailureEventTests.cs` (new, 7 tests)
- `Ashfall.Core.Tests/Water/AtmosphericCondenserSystemTests.cs` (wear assert)
- `docs/plans/flagship_b5_b8/EXPANSION2_SOURCE_FAILURE_EVENTS.md` (this file)
