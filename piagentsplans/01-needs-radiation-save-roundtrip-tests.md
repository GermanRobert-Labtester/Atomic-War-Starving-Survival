# Plan 01 — NeedsSystem & RadiationSystem Save Round-Trip Tests (closes H10)

## Goal (2 lines)
Close known issue H10: add save/load round-trip coverage for `NeedsSystem` and
`RadiationSystem`, whose 58 existing tests cover only tick behavior, not capture/restore.

## Files to touch
- `Ashfall.Core.Tests/NeedsRadiationSystemTests.cs` (extend, or new
  `NeedsRadiationSaveRoundTripTests.cs` — prefer new file, one system per concern)
- Read-only references: `Assets/Ashfall.Core/Survivors/NeedsSystem.cs`,
  `Assets/Ashfall.Core/Radiation/RadiationSystem.cs`,
  `src/Host/HoldfastRuntimeSession.cs` (fallback decay path, `:164`)

## Steps
1. Read both systems' `CaptureState()` / `RestoreState(SystemState)` implementations and DTO
   shapes. Note float formatting and null/empty normalization requirements from
   `SaveChecksum.cs`.
2. Write tests per system:
   - capture → restore into fresh instance → assert field equality (all 8 vitals for Needs;
     dose accumulation + phase for Radiation);
   - capture → tick N days → capture again → assert states differ (guard against no-op capture);
   - restore of default/empty state → assert no exception and documented defaults;
   - checksum stability: same state → same `SaveChecksum` hash across two captures.
3. Add a test for the `HoldfastRuntimeSession` fallback path: `Survivors == null` → fallback
   decay only, no projection.
4. Determinism check: restore must not change subsequent `ISeededRng` streams (paired
   capture/restore → identical tick outcomes).

## Verification
```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test  Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "NeedsRadiation"
dotnet test  Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   # full suite green
dotnet build Ashfall.csproj                                  # 0 errors 0 warnings
```

## Risk
LOW — test-only change, no production code touched.

## Definition of Done
- Round-trip + checksum-stability tests for both systems pass.
- H10 can be marked RESOLVED in `AGENTS.md` (separate small docs commit).
