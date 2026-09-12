# Bug Hunt Fourth Pass — Handoff Report

**Date:** 2026-09-12
**Scope:** Deeper forensic slice after passes 1–3; seal remaining save-alias leaks plus high-ROI host/UI unreachable paths.
**Authority checked:** `WORKTREE_OWNERSHIP.md` — no active claims.
**Deferred:** ChemWarfare `OnToxicExposureResolved` (+ `EvaluateActorExposure` never called in production); SaltMine persistence (hub envelope decision); dual Greenhouse authorities; StandingRecord/PerimeterDefense shallow clones; atlas `On*Selected` wires; Caravan barter sidebar; Expedition radar dispatch; Plans 146–149 OPEN-only; DEBT-PLAN167/168; theater fake-feedback panels.

---

## Strategy

1. Parallel explore hunts: Core save/determinism + host/UI unwired gaps.
2. Confirm save aliases still live after pass 3 (WaterTreatment, Disease, SilentFoundry).
3. Seal deferred UI/host paths with clear existing owners (trapping butcher/hide, ballistics optic, survival overlays, recon guards).
4. Harden DistressRescue grant honesty and PhantomMemory consume fail-closed.
5. Focused Core/host builds + filtered xUnit only.

---

## Bugs fixed

| # | Bug | Fix |
|---|-----|-----|
| 1 | WaterTreatment `CaptureState`/`RestoreState` aliased live `_state` | JSON-clone Capture/Restore |
| 2 | Disease `CaptureState` returned live ward into hub/save | JSON-clone Capture; invert alias-pin test; update day-owner comment |
| 3 | SilentFoundry Capture returned live state; Restore aliased lists; consequence Capture aliased ledger | JSON-clone Capture/Consequence; Restore copies cloned lists into readonly `_state` |
| 4 | Survival workstation OPEN INVENTORY / OPEN CRAFTING dead | Wire overlays to `OpenPlayerPanel("inventory"/"crafting")` |
| 5 | Ballistics `attach_optic` host ready, UI missing | Optic item field + ATTACH OPTIC button |
| 6 | Recon telemetry `survey`/`forecast` unguarded `[0]` | Empty-list guards matching recover/scout |
| 7 | Wildlife catch food unreachable; no Butcher UI | Panel Butcher Catch → `host.Butcher` |
| 8 | `PreserveHide` Core-only; no host/UI path | Host `PreserveHide` + inventory grant; Preserve Hide button |
| 9 | DistressRescue `TryProduce` ignored; journal always claimed stow | Count ok/fail; truthful journal text |
| 10 | PhantomMemory `TryConsume` ignored consume failure | Consume the id that has stock; return false on fail |

---

## Files touched

**Core:**
`WaterTreatmentSystem.cs`, `Disease/DiseaseSystem.cs`, `Foundry/SilentFoundrySystem.TreatyLabor.cs`

**Host / Main / UI:**
`Main.UiPanels.cs`, `Main.Expeditions.cs`, `Main.Narrative.cs`, `Main.CampaignOwners.cs`, `UI/Plans74To77Panels.cs`, `UI/WildlifeTrappingPanel.cs`, `Host/WildlifeTrappingHostSession.cs`, `Host/PhantomMemoryHostSession.cs`

**Tests:**
`DiseaseSystemTests.cs` (`CaptureState_ReturnsIndependentClone_NotLiveAlias`)

---

## Verification

| Check | Result |
|-------|--------|
| `dotnet build Ashfall.Core/Ashfall.Core.csproj` | 0 errors, 0 warnings |
| `dotnet build Ashfall.csproj` | 0 errors, 0 warnings |
| Filtered xUnit | **174 passed**, 0 failed |

```bash
dotnet build Ashfall.Core/Ashfall.Core.csproj -v q
dotnet build Ashfall.csproj -v q
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj \
  --filter "FullyQualifiedName~WaterTreatmentSystemTests|FullyQualifiedName~DiseaseSystemTests|FullyQualifiedName~SilentFoundrySystemTests|FullyQualifiedName~SilentFoundryConsequenceTests|FullyQualifiedName~Plans74To77SystemsTests|FullyQualifiedName~WildlifeTrappingAcquisitionTests|FullyQualifiedName~WildlifeTrappingEdgeCaseTests|FullyQualifiedName~CombatBallisticsTests|FullyQualifiedName~DistressRescueMissionTests|FullyQualifiedName~WaterTreatmentCommandTests|FullyQualifiedName~WaterTreatmentIntegrationTests|FullyQualifiedName~FoundryTreatyConsequenceExpansionTests|FullyQualifiedName~FoundryAccordExpansionTests" \
  --nologo -v q
```

---

## Remaining / deferred

| Item | Why deferred |
|------|----------------|
| ChemWarfare `OnToxicExposureResolved` | Needs toxic-exposure consumer owner; `EvaluateActorExposure` also never called from host combat tick |
| SaltMine persistence | Hub/foundry envelope decision — runtime-only today |
| Dual Greenhouse authorities | Architecture: player `_greenhouse` vs expansion hub Greenhouse both day-ticked |
| StandingRecord / PerimeterDefense clones | Medium save alias; lower ROI than sealed batch |
| Atlas `OnLocationSelected` / quest / research select | Only `OnClose` wired; needs destination panel owners |
| Caravan barter sidebar / Expedition radar dispatch | UI design (do not treat section ids as factions; avoid dispatch-on-select) |
| Plans 146–149 / DEBT-PLAN167/168 | Prior approval deferral |

---

## Intentionally untouched

- `SaveSectionRegistry.cs`
- Governance ledgers (`INTEGRATION_PLANS.md`, `KNOWN_DEBT.md`, ownership)
- ChemWarfare combat exposure tick path (blocked on consumer mapping)
- SaltMine hub envelope / Greenhouse authority merge
