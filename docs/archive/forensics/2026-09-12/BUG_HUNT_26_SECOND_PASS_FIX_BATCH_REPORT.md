# Bug Hunt Second Pass — Handoff Report

**Date:** 2026-09-12
**Scope:** Continue forensic hunting after the optimized 25-bug batch; fix remaining confirmed high-ROI bugs.
**Authority checked:** `WORKTREE_OWNERSHIP.md` — no active claims.
**Deferred:** DistressRescue `ClaimIdempotentRewards` / `OnRewardsGranted` host wire; Plans 146–149 OPEN-only; DEBT-PLAN167/168; theater fake-feedback panels; ChemWarfare `OnToxicExposureResolved`; PreserveHide; Ballistics attach_optic UI gap.

---

## Strategy

1. Re-verify second-hunt candidates against live source (not prior audit names).
2. Prefer player-visible dead wires and false-success inventory commits first.
3. Seal CaptureState-by-reference save leaks with the FoodPreservation JSON-clone pattern.
4. Persist WaystationNetwork as a companion field on the existing `waystation` section (no new SaveSectionRegistry row).
5. Focused Core/host builds + filtered xUnit only.

---

## Bugs fixed (15)

| # | Bug | Fix |
|---|-----|-----|
| 1 | Greenhouse water parse ignores `water:25:clean` / `water:50:tainted` | Parse units + quality after first colon; tainted quality now applies |
| 2 | CombatHud Fire/Suppress/ClearJam/EndTurn unwired | Wire to `_combat.Action*` with default hostile/player subject helpers |
| 3 | Faction matrix/narrative selection dead | `OnFactionSelected += OpenFactionDetailPanel` |
| 4 | Dose ledger survivor click dead | Bind + open `SurvivorDetailPanel` for selected id |
| 5 | Expedition camp resolve does not refresh HUD | `OnCampResolved` → `UpdateHud` + expedition panel refresh |
| 6 | Hope beacon ignores `TryCommit` result | Commit with `onCommitted` mutation; fail string on false |
| 7 | Pathogen cure mutates then ignores commit | Commit wraps `StartCureProject`; catch reject / commit fail |
| 8 | Wildlife SetTrap ignores commit | Undo via `RemoveTrap` if commit fails |
| 9 | Wildlife RepairTrap ignores commit | Repair inside `TryCommit(onCommitted)` |
| 10 | Generational CaptureState by reference | JSON clone Capture/Restore; host saves `CaptureState()` |
| 11 | Stealth CaptureState by reference | JSON clone; host saves `CaptureState()` |
| 12 | Mutation CaptureState by reference | JSON clone; host saves `CaptureState()` |
| 13 | PrisonerSystem CaptureState by reference | JSON clone; host saves `CaptureState()` |
| 14 | WaystationNetwork never persisted | Clone Capture; companion `network` on `WaystationSystemState`; Setup/Save roundtrip |
| 15 | Radio station constructed with null harrow | Pass `EnsureOrbitalHarrowTelemetry()` at construction |
| 16 | SurvivorMentalHealth Capture by reference | JSON clone Capture/Restore |
| 17 | VehicleGarage Capture by reference | JSON clone Capture/Restore |

*(Counts as 15 primary + 2 sibling CaptureState seals found in the same pass.)*

---

## Files touched

**Core:**
`GenerationalSystem.cs`, `StealthSystem.cs`, `MutationSystem.cs`, `PrisonerSystem.cs`, `WaystationNetworkSystem.cs`, `WaystationSystem.cs`, `SurvivorMentalHealthSystem.cs`, `VehicleGarageSystem.cs`

**Host/UI:**
`Main.World.cs`, `Main.UiPanels.cs`, `Main.MoraleContagion.cs`, `Main.PathogenStrains.cs`, `Main.Plans46_49.cs`, `Main.Plans178_181.cs`, `Main.ShelterInfrastructure.cs`, `WildlifeTrappingHostSession.cs`, `CombatHostSession.cs`

---

## Verification

| Check | Result |
|-------|--------|
| `dotnet build Ashfall.Core/Ashfall.Core.csproj` | 0 errors, 0 warnings |
| `dotnet build Ashfall.csproj` | 0 errors, 0 warnings |
| Filtered xUnit (greenhouse, generational, stealth, mutation, prisoner, waystation, pathogen, morale contagion, trapping persistence, vehicle garage, survivor mental health) | **97 passed**, 0 failed |

```bash
dotnet build Ashfall.Core/Ashfall.Core.csproj -v q
dotnet build Ashfall.csproj -v q
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj \
  --filter "FullyQualifiedName~GreenhouseSystemTests|FullyQualifiedName~GenerationalSystemTests|FullyQualifiedName~StealthSystemTests|FullyQualifiedName~MutationSystemTests|FullyQualifiedName~PrisonerSystemTests|FullyQualifiedName~ShelterPrisonerSystemTests|FullyQualifiedName~WaystationSystemTests|FullyQualifiedName~PathogenStrainSystemTests|FullyQualifiedName~MoraleContagionSystemTests|FullyQualifiedName~WildlifeTrappingPersistenceTests|FullyQualifiedName~VehicleGarage|FullyQualifiedName~SurvivorMentalHealth" \
  --nologo -v q
```

---

## Remaining / deferred findings (not fixed)

| Item | Why deferred |
|------|----------------|
| DistressRescue rewards never claimed from host | Larger quest/radio reward seam; needs intentional host owner |
| Plans 146–149 OPEN-only job UI | Prior approval deferral |
| DEBT-PLAN167/168 espionage consequence routing | Known debt |
| Theater / fake-feedback panels | Content/policy, not a one-line wire |
| ChemWarfare `OnToxicExposureResolved` | Needs host toxic-exposure owner confirmation |
| PreserveHide / Ballistics attach_optic | UI gap / catalog consumer; lower ROI |
| Other `CaptureState => _state.Capture()/Clone()` sites | Those already clone; remaining raw `_state` aliases were sealed in this pass |

---

## Intentionally untouched shared paths

- `SaveSectionRegistry.cs` (network rides existing `waystation` section)
- `INTEGRATION_PLANS.md` / `KNOWN_DEBT.md` / ownership ledger (not foreman)
- DistressRescue / RadioHostSession reward grant path
