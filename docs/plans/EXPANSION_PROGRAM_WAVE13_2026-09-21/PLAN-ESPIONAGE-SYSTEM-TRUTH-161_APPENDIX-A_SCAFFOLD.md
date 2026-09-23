# PLAN-ESPIONAGE-SYSTEM-TRUTH-161 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, batch 3).
**Scaffolding authority:** [`PLAN-ESPIONAGE-COUNTERINTEL-41`](../EXPANSION_PROGRAM_WAVE5_2026-09-21/PLAN-ESPIONAGE-COUNTERINTEL-41.md) — adopt the authority's patterns rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|

## 2. Source inventory (40 files, Core + host)

| File | Lines |
|---|---:|
| `Espionage/InformantNetworkTradecraftEngine.cs` | 222 |
| `Factions/CounterIntelligenceState.cs` | 77 |
| `Factions/CounterIntelligenceSystem.cs` | 315 |
| `Factions/EspionageConsequenceRouter.cs` | 120 |
| `Factions/EspionageSystem.cs` | 734 |
| `Factions/FactionBountySystem.cs` | 254 |
| `Factions/FactionBranchCoordinator.cs` | 668 |
| `Factions/FactionCovertOpsCoordinator.cs` | 518 |
| `Factions/FactionDisplayNameCatalog.cs` | 118 |
| `Factions/FactionIntelligenceCatalog.cs` | 57 |
| `Factions/FactionStandingIdResolver.cs` | 131 |
| `Factions/ForcedLaborSystem.cs` | 401 |
| `Factions/IndependentBranchCatalog.cs` | 95 |
| `Factions/IndependentBranchIds.cs` | 162 |
| `Factions/IndependentBranchSave.cs` | 76 |
| `Factions/IndependentBranchState.cs` | 70 |
| `Factions/IndependentBranchSystem.cs` | 311 |
| `Factions/InfiltratorCatalog.cs` | 52 |
| `Factions/InfiltratorCatalogLoader.cs` | 34 |
| `Factions/MilitaryBranchCatalog.cs` | 85 |
| `Factions/MilitaryBranchIds.cs` | 153 |
| `Factions/MilitaryBranchSave.cs` | 84 |
| `Factions/MilitaryBranchState.cs` | 73 |
| `Factions/MilitaryBranchSystem.cs` | 271 |
| `Factions/PrisonerSystem.cs` | 403 |
| `Factions/PrpfIds.cs` | 30 |
| `Factions/PrpfSave.cs` | 76 |
| `Factions/PrpfStandingSystem.cs` | 204 |
| `Factions/PrpfState.cs` | 57 |
| `Factions/RebelBranchCatalog.cs` | 84 |
| `Factions/RebelBranchIds.cs` | 144 |
| `Factions/RebelBranchSave.cs` | 85 |
| `Factions/RebelBranchState.cs` | 63 |
| `Factions/RebelBranchSystem.cs` | 255 |
| `Factions/ShelterEspionageSystem.cs` | 341 |
| `Factions/TerritoryControlSystem.cs` | 451 |
| `Factions/WeightOfChoicesSave.cs` | 189 |
| `host:Host/EspionageHostSession.cs` | 122 |
| `host:Host/EspionageSaveStore.cs` | 27 |
| `host:Host/ShelterEspionageSaveStore.cs` | 36 |

## 3. Data bindings

| Catalog | Records/shape |
|---|---|
| `espionage_missions.json` | object(3 keys) |
| `espionage_operations.json` | object(3 keys) |

## 4. Host attachment

- Candidate host partials: none — new attachment or headless-only
- Proposed method names: `SetupFactionsESScaffold` / `SaveFactionsESScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Factions/ES161ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class ES161ScaffoldTests
{
    [Fact] public void EST161A_TODO() { /* operation lifecycle + transition table. */ }
    [Fact] public void EST161B_TODO() { /* exposure model + seeded determinism test. */ }
    [Fact] public void EST161C_TODO() { /* consequence routing to Plan 29/121 owners (no private score proof). */ }
    [Fact] public void EST161D_TODO() { /* network-state read contract with Plan 41's sealed engines. */ }
    [Fact] public void EST161E_TODO() { /* save round-trip + no re-roll on load. */ }
    [Fact] public void ES161_AuthorityConformance_TODO() { /* pattern parity with PLAN-ESPIONAGE-COUNTERINTEL-41 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/Factions/`
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
