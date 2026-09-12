# Bug Hunt + Optimized Fix Batch — Handoff Report

**Date:** 2026-09-12
**Scope:** Deep verify / deep code audit for errors, warnings, unwired and dysfunctional code; hunt ~25 bugs; optimize strategy; fix approved batch.
**Authority checked:** `INTEGRATION_PLANS.md` / `WORKTREE_OWNERSHIP.md` had no active batch claims blocking these paths.
**Deferred (per approval):** Plans 146–149 OPEN-only job UI; DEBT-PLAN167/168 espionage consequence routing; wildlife `PreserveHide` host wire.

---

## Strategy (applied before fixes)

1. **Wire dead UI first** — one-line `OnActionRequested` subscriptions + thin handlers (highest player-visible ROI).
2. **Core save/logic next** — assault persistence, day seed, float wall HP, agri light, espionage confirm, shelter espionage clone, fluid freeze, medical refund.
3. **Host dirty events** — SkyDefense / FoodPreservation / Perimeter intrusion.
4. **Settings / false-success / a11y** — LargeFonts control, Apply presentation, save-slot select, medical apply bools, journal logging, reduced-motion tween gate, VerboseRadioLog consumer.
5. **Focused verification only** — Core + host builds; filtered xUnit on touched systems.

---

## Bugs fixed (24)

| # | Bug | Fix |
|---|-----|-----|
| 1 | Prisoner panel actions unwired | `_prisonerPanel.OnActionRequested += HandlePrisonerAction` + Core interrogate/recruit/release |
| 2 | Nursery guardian/teacher unwired | `_nurseryPanel.OnActionRequested += HandleNurseryAction` + AssignGuardian/Teacher |
| 3 | Geothermal aquifer console unwired | `_geothermalAquiferPanel.OnActionRequested += HandleGeothermalAction` |
| 4 | Recon telemetry console unwired | `_reconTelemetryPanel.OnActionRequested += HandleReconTelemetryAction` |
| 5 | Accessibility flags Apply no-op | `UserSettingsStore.Apply` sets `ContentScaleFactor`, HighContrast modulate on first CanvasItem |
| 6 | `VerboseRadioLog` never read | Gate `FactionRadioHudPanel` archive append on `Current.VerboseRadioLog` |
| 7 | `LargeFonts` button never built | Settings row + Apply scale boost |
| 8 | Save-slot SELECT false-success | Panel no longer optimistically selects; host checks `SelectSlot` bool |
| 9 | Medical apply delegates always `true` | Propagate `Unknown survivor` failure from host strings |
| 10 | Medical consume without refund | `TryProduce` refund on `ApplyTreatment` failure (immediate + scheduled) |
| 11 | *(deferred)* Plans 146–149 OPEN-only | Not in approved batch |
| 12 | Reduced motion ignores tweens | `AudioManager` snaps when `ReducedMotion` |
| 13 | Perimeter `assault_count` not saved | Capture/Restore persist field |
| 14 | Assault day not passed | `SimulateRaiderAssault(..., currentDay: day)` |
| 15 | Agri Harvest hardcodes light=1 | Cache `_lastEnv` from `TickDay`; Harvest uses lighting |
| 16 | Espionage Estimate never Confirmed | Upgrade existing facts at intel≥4; respect discovery threshold |
| 17 | Wall HP int division | Float Walls/DamagePenalties + float division |
| 18 | Fluid `frozen` dead | Freeze-dominant hazard sets `frozen` instead of burst |
| 19 | SkyDefense `OnMaintenanceDue` unsubscribed | Host marks dirty |
| 20 | Perimeter `OnIntrusionLogged` unsubscribed | Host marks dirty |
| 21 | FoodPreservation `OnFoodConsumed` unsubscribed | Host marks dirty |
| 22 | ShelterEspionage Capture by reference | JSON clone Capture/Restore (FoodPreservation pattern) |
| 23 | Journal/expedition silent catches | `GD.PrintErr` on catalog/lore load failures |
| 24 | Hazard text setting unused | `FormatDoseSource` respects `HazardTextLabels` |

---

## Files touched (intentional)

**Core:** `DefenseSystem.cs`, `PerimeterDefenseSystem.cs`, `AgricultureSystem.cs`, `EspionageSystem.cs`, `ShelterEspionageSystem.cs`, `PrisonerSystem.cs`, `FluidLogisticsSystem.cs`, `MedicalPipelineCoordinator.cs`

**Host/UI:** `Main.UiPanels.cs`, `Main.Plans178_181.cs`, `Main.FlagshipInstitutions.cs`, `Main.Plans62_65.cs`, `Main.AdvancedShelterSystems.cs`, `Main.Medical.cs`, `SaveLoadPanel.cs`, `SettingsPanel.cs`, `UserSettings.cs`, `AudioManager.cs`, `AudioStateCoordinator.cs`, `JournalCatalogData.cs`, `ExpeditionPanel.cs`, `FactionRadioHudPanel.cs`, `AshfallUiHelpers.cs`

**Tests:** `Plan168FluidLogisticsTests.cs` (freeze expects `frozen`, not burst)

---

## Verification

| Check | Result |
|-------|--------|
| `dotnet build Ashfall.Core/Ashfall.Core.csproj` | 0 errors, 0 warnings |
| `dotnet build Ashfall.csproj` | 0 errors, 0 warnings |
| Filtered xUnit (Defense, Agriculture, Espionage, Fluid, Prisoner, Medical pipeline) | **74 passed**, 0 failed |

Commands:

```bash
dotnet build Ashfall.Core/Ashfall.Core.csproj -v q
dotnet build Ashfall.csproj -v q
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj \
  --filter "FullyQualifiedName~AgricultureSystemTests|FullyQualifiedName~AgriculturePersistenceTests|FullyQualifiedName~ShelterEspionageSystemTests|FullyQualifiedName~Plan167EspionageTests|FullyQualifiedName~Plan168FluidLogisticsTests|FullyQualifiedName~PrisonerSystemTests|FullyQualifiedName~MedicalPipelineTests|FullyQualifiedName~MedicalPipelinePhase2Tests|FullyQualifiedName~DefenseSystemTests" \
  --nologo -v q
```

---

## Limitations / intentionally untouched

- Plans **146–149** job-start UI still OPEN-only (approved deferral).
- **DEBT-PLAN167/168** espionage consequence routing unchanged.
- Wildlife **PreserveHide** host wire left for a later package.
- HighContrast is a light CanvasItem modulate + scale nudge, not a full theme rewrite.
- Godot headless UI click-through for prisoner/nursery/geothermal/recon buttons not run this turn (handlers + subscriptions verified statically; Core APIs covered by existing tests).
- Unrelated dirty worktree noise (deleted skills/cache, other Core/host edits) was not modified.

---

## Handoff

**Outcome:** Approved optimized batch implemented; Core/host green; 74 focused tests green.
**Shared paths untouched:** save orchestrator registry, panel registry bootstrap, INTEGRATION_PLANS / KNOWN_DEBT ledgers (builder role).
**Next recommended packages:** Plans 146–149 action buttons; wildlife hide preservation host wire; deeper HighContrast theme tokens if art direction requires it.
