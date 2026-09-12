# Bug Hunt Third Pass — Handoff Report

**Date:** 2026-09-12
**Scope:** Deeper forensic slice after passes 1–2; seal remaining save-alias leaks and DistressRescue reward chain.
**Authority checked:** `WORKTREE_OWNERSHIP.md` — no active claims.
**Deferred:** Ballistics `attach_optic` UI button; ChemWarfare `OnToxicExposureResolved`; PreserveHide; Plans 146–149 OPEN-only; DEBT-PLAN167/168; theater fake-feedback panels.

---

## Strategy

1. Hunt host `TryCapturePersisted(….State)` aliases and Core systems missing `CaptureState`.
2. Add JSON-clone `CaptureState` / clone `RestoreState` where missing.
3. Switch host saves to `CaptureState()`.
4. Seal DistressRescue end-to-end: expedition bridge → terminal stage → `ClaimIdempotentRewards` → inventory/reputation grant.
5. Focused Core/host builds + filtered xUnit only.

---

## Bugs fixed

| # | Bug | Fix |
|---|-----|-----|
| 1 | Fallout save aliased live `_state` (no CaptureState) | JSON-clone Capture/Restore; host saves `CaptureState()` |
| 2 | Desperation save alias | same |
| 3 | Mercenary save alias | same |
| 4 | Archaeology save alias | same |
| 5 | Amputation save alias | same |
| 6 | Fungi cultivation save alias | same |
| 7 | Justice save alias | same |
| 8 | Railway save alias | same |
| 9 | Plastic pyrolysis host saved `.State` despite CaptureState | Host → `CaptureState()`; Restore clones |
| 10 | Cargo airdrop host saved `.State` | Host → `CaptureState()`; Restore clones |
| 11 | Bio fermentation host saved `.State` | Host → `CaptureState()` |
| 12 | Shelter decor host saved `.System.State` | Host → `CaptureState()` |
| 13 | DistressRescue rewards never claimed from host | Auto-claim on TerminalRescued/Survived in `RadioHostSession` |
| 14 | DistressRescue rewards never granted to inventory/rep | `OnRewardsGranted` → TryProduce + ModifyTrust + journal |
| 15 | DistressRescue never advanced by expeditions | Bridge dispatch/arrive on expedition started/completed |
| 16 | No destination lookup for active rescue missions | `GetActiveMissionByDestination` |

---

## Files touched

**Core:**
`FalloutSystem.cs`, `DesperationSystem.cs`, `MercenarySystem.cs`, `ArchaeologySystem.cs`, `AmputationSystem.cs`, `FungiCultivationSystem.cs`, `JusticeSystem.cs`, `RailwaySystem.cs`, `PlasticPyrolysisSystem.cs`, `CargoAirdropSystem.cs`, `DistressRescueMissionManager.cs`

**Host:**
`Main.Plans186_189.cs`, `Main.Plans190_193.cs`, `Main.Plans202_205.cs`, `Main.Plans126_129.cs`, `Main.ShelterBatch3.cs`, `Main.Narrative.cs`, `Main.Expeditions.cs`, `RadioHostSession.cs`

---

## Verification

| Check | Result |
|-------|--------|
| `dotnet build Ashfall.Core/Ashfall.Core.csproj` | 0 errors, 0 warnings |
| `dotnet build Ashfall.csproj` | 0 errors, 0 warnings |
| Filtered xUnit | **126 passed**, 0 failed |

```bash
dotnet build Ashfall.Core/Ashfall.Core.csproj -v q
dotnet build Ashfall.csproj -v q
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj \
  --filter "FullyQualifiedName~DistressRescueMissionTests|FullyQualifiedName~FalloutSystemTests|FullyQualifiedName~DesperationSystemTests|FullyQualifiedName~MercenarySystemTests|FullyQualifiedName~ArchaeologySystemTests|FullyQualifiedName~AmputationSystemTests|FullyQualifiedName~FungiCultivationSystemTests|FullyQualifiedName~JusticeSystemTests|FullyQualifiedName~RailwaySystemTests|FullyQualifiedName~PlasticPyrolysis|FullyQualifiedName~CargoAirdrop|FullyQualifiedName~BioFermentation|FullyQualifiedName~ShelterDecor" \
  --nologo -v q
```

---

## Remaining / deferred

| Item | Why deferred |
|------|----------------|
| Ballistics `attach_optic` UI | Host handler exists; panel has no button |
| ChemWarfare `OnToxicExposureResolved` | Needs toxic-exposure consumer owner |
| PreserveHide | Trapping host wire; prior deferral |
| Plans 146–149 / DEBT-PLAN167/168 | Prior approval deferral |

---

## Intentionally untouched

- `SaveSectionRegistry.cs`
- Governance ledgers (`INTEGRATION_PLANS.md`, `KNOWN_DEBT.md`, ownership)
