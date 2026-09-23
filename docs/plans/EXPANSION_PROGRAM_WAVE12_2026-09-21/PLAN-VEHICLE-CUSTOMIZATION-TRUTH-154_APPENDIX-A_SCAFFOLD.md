# PLAN-VEHICLE-CUSTOMIZATION-TRUTH-154 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, batch 3).
**Scaffolding authority:** [`PLAN-EXPEDITION-VEHICLE-TRUTH-219`](../EXPANSION_PROGRAM_WAVE16_2026-09-21/PLAN-EXPEDITION-VEHICLE-TRUTH-219.md) — adopt the authority's patterns rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|
| `EVT-219A` | rig model + suitability table. |
| `EVT-219B` | range/refusal tests per route class. |
| `EVT-219C` | breakdown + field-repair fixtures. |

## 2. Source inventory (9 files, Core + host)

| File | Lines |
|---|---:|
| `ExpeditionVehicleSystem.cs` | 349 |
| `Expeditions/VehicleArmorGradeCatalog.cs` | 155 |
| `Expeditions/VehicleGarageCatalog.cs` | 62 |
| `Expeditions/VehicleGarageSystem.cs` | 865 |
| `Vehicles/VehicleCustomizationSystem.cs` | 435 |
| `host:Host/HostCli.VehicleGarage.cs` | 243 |
| `host:Host/VehicleGarageSaveStore.cs` | 36 |
| `host:Main.VehicleGarage.cs` | 54 |
| `host:UI/VehicleGaragePanel.cs` | 636 |

## 3. Data bindings

| Catalog | Records/shape |
|---|---|
| `vehicle_modifications.json` | object(2 keys) |
| `vehicles.json` | object(3 keys) |
| `vehicle_armor_grades.json` | object(3 keys) |
| `vehicle_modules.json` | object(2 keys) |

## 4. Host attachment

- Candidate host partials: `Main.VehicleGarage.cs`
- Proposed method names: `SetupVehiclesVCScaffold` / `SaveVehiclesVCScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Vehicles/VC154ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class VC154ScaffoldTests
{
    [Fact] public void VCT154A_TODO() { /* slot model + capacity table per class. */ }
    [Fact] public void VCT154B_TODO() { /* compatibility rules + illegal-fit refusal tests. */ }
    [Fact] public void VCT154C_TODO() { /* effect rows handed to Plan 30 (no local modifiers). */ }
    [Fact] public void VCT154D_TODO() { /* removal/recall conservation test. */ }
    [Fact] public void VCT154E_TODO() { /* persistence round-trip + no re-fit on load. */ }
    [Fact] public void VC154_AuthorityConformance_TODO() { /* pattern parity with PLAN-EXPEDITION-VEHICLE-TRUTH-219 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/Vehicles/`
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
