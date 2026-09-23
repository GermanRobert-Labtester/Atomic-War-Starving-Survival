# PLAN-RAIL-MAINTENANCE-TRUTH-158 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, batch 3).
**Scaffolding authority:** [`PLAN-MAINTENANCE-DECAY-TRUTH-119`](../EXPANSION_PROGRAM_WAVE10_2026-09-21/PLAN-MAINTENANCE-DECAY-TRUTH-119.md) — adopt the authority's patterns rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|
| `MDT-119A` | decay class table (use event → rate row) with data rows schema-checked. |
| `MDT-119B` | difficulty binding test (SPARING vs DIRGE on one class). |
| `MDT-119C` | repair rules + kit consumption through the inventory seam. |

## 2. Source inventory (14 files, Core + host)

| File | Lines |
|---|---:|
| `Collectibles/CollectibleTutorialTracker.cs` | 182 |
| `Expeditions/DraisineRerailingSystem.cs` | 255 |
| `Expeditions/RailGrindingCatalogLoader.cs` | 128 |
| `Expeditions/RailGrindingEngine.cs` | 336 |
| `Expeditions/RailLogisticsCatalog.cs` | 59 |
| `Expeditions/RailwayInterlockEngine.cs` | 730 |
| `Expeditions/RailwaySystem.cs` | 831 |
| `Rail/RailTrackMaintenanceEngine.cs` | 243 |
| `host:Host/RailGrindingHostSession.cs` | 88 |
| `host:Host/RailGrindingSaveStore.cs` | 32 |
| `host:Host/RailwaySaveStore.cs` | 32 |
| `host:UI/AshfallStatusRail.cs` | 131 |
| `host:UI/RailGrindingPanel.cs` | 335 |
| `host:UI/RailwayTerminalPanel.cs` | 243 |

## 3. Data bindings

| Catalog | Records/shape |
|---|---|
| `rail_grinding_catalog.json` | object(4 keys) |
| `rail_logistics_catalog.json` | object(2 keys) |
| `rail_network.json` | object(4 keys) |
| `railway_interlock_catalog.json` | object(6 keys) |
| `rerailing_equipment_catalog.json` | object(2 keys) |

## 4. Host attachment

- Candidate host partials: none — new attachment or headless-only
- Proposed method names: `SetupRailRMScaffold` / `SaveRailRMScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Rail/RM158ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class RM158ScaffoldTests
{
    [Fact] public void RMT158A_TODO() { /* segment model + attachment check. */ }
    [Fact] public void RMT158B_TODO() { /* wear rules via Plan 119's contract (no private table). */ }
    [Fact] public void RMT158C_TODO() { /* closure threshold + route-block test. */ }
    [Fact] public void RMT158D_TODO() { /* repair crew path + material conservation. */ }
    [Fact] public void RMT158E_TODO() { /* recovery-at-current-condition test. */ }
    [Fact] public void RM158_AuthorityConformance_TODO() { /* pattern parity with PLAN-MAINTENANCE-DECAY-TRUTH-119 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/Rail/`
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
