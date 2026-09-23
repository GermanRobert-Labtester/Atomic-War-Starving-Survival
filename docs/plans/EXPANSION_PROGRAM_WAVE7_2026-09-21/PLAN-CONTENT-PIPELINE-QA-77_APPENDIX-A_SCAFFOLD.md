# PLAN-CONTENT-PIPELINE-QA-77 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator. Built with the two
session toolkit plans as its authority: the integration pattern set of
**PLAN-INTEGRATION-KIT-02** (host session, save store, day owner, panel route,
CLI probe, focused fixture) and the appendix/maintenance convention of
**PLAN-ORPHAN-SEAL-01** (generated, reproducible, never hand-edited).
**Status:** scaffolding only — no production file is created by this appendix.
A claim still owns the edits; this page exists so the claim starts from real
paths, real names, and a fixture skeleton derived from this plan's own packages.

## 1. Source inventory (43 files)

| File | Lines |
|---|---:|
| `CatalogIntegrityValidator.cs` | 3141 |
| `Content/CatalogDefinitionCounter.cs` | 89 |
| `Content/CollectibleCatalogIntegrityValidator.cs` | 433 |
| `Content/ContentAcceptanceLadder.cs` | 102 |
| `Content/ContentAcceptancePipeline.cs` | 150 |
| `Content/ContentAcceptanceRung.cs` | 40 |
| `Content/ContentDeepChainGate.cs` | 445 |
| `Content/ContentExemption.cs` | 150 |
| `Content/ContentOrphanCertificationEngine.cs` | 136 |
| `Content/ContentUtilizationGate.cs` | 247 |
| `Content/ContentUtilizationGraph.cs` | 399 |
| `Content/ContentUtilizationInstrumentation.cs` | 223 |
| `Content/ContentUtilizationManifest.cs` | 227 |
| `Content/ContentUtilizationScanner.cs` | 2073 |
| `DoseContentCatalog.cs` | 224 |
| `Expeditions/ExpeditionAggregate.cs` | 116 |
| `Expeditions/ExpeditionLootValidator.cs` | 115 |
| `Governance/StandingGateRegistry.cs` | 204 |
| `IO/CatalogBootValidator.cs` | 339 |
| `Inventory/EquipLimbGate.cs` | 104 |
| `Medical/PatientRecordIntegrityValidator.cs` | 195 |
| `Narrative/ContrabandCatalogValidator.cs` | 286 |
| `Narrative/PatrolEncounterValidator.cs` | 337 |
| `NarrativeConsequence/NarrativeValidator.cs` | 280 |
| `Orchestration/BootstrapLifecycleGate.cs` | 128 |
| `Orchestration/LedgerTruthIntegrityGate.cs` | 137 |
| `Survivors/SurvivorAggregate.cs` | 194 |
| `Survivors/SurvivorIntegrityValidator.cs` | 406 |
| `UI/ConfirmationFlowGate.cs` | 58 |
| `World/RouteGateContextResolver.cs` | 292 |
| `World/TravelGraphKnowledgeGate.cs` | 136 |
| `World/WeatherGate.cs` | 94 |
| `World/WeatherGateCatalog.cs` | 194 |
| `World/WeatherGateCatalogLoader.cs` | 193 |
| `World/WeatherGateContextEvaluator.cs` | 228 |
| `World/WeatherGateContextModifier.cs` | 45 |
| `World/WeatherGateEvaluationContext.cs` | 49 |
| `World/WeatherGateEvaluator.cs` | 438 |
| `World/WeatherGateFile.cs` | 45 |
| `World/WeatherGateRadioHooks.cs` | 207 |

… and 3 more.

## 2. Data bindings

| Catalog | Records/shape |
|---|---|
| `weather_route_gates.json` | object(2 keys) |
| `standing_gates.json` | object(2 keys) |
| `blast_gate_mechanical_audits.json` | 8 |

## 3. Host attachment

- Candidate host partials: none — new attachment or headless-only

- Proposed setup method: `SetupContentScaffold` · proposed save method: `SaveContentScaffold` (checked against the 228 existing setup names in Plan 1 Appendix AI — extend the real owner name if one exists)
- Loop coupling: see Plan 1 Appendix AA before adding any day/hour step; a new timer is forbidden.

## 4. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Content/CP77ScaffoldTests.cs`

```csharp
// Scaffold skeleton — derived from this plan's packages below.
// Fixtures assert one package each; replace the TODO bodies at claim time.
public class CP77ScaffoldTests
{
    [Fact] public void CP77A_TODO() { /* pipeline doc: one page per content type with the five stages and the exact comma */ }
    [Fact] public void CP77B_TODO() { /* per-type checklists in `docs/content/<type>.md` (items, recipes, quests, narrati */ }
    [Fact] public void CP77C_TODO() { /* PR template additions: author attests the five stages; CI verifies what is verif */ }
    [Fact] public void CP77D_TODO() { /* unified report: one command prints all content findings for a diff (changed cata */ }
    [Fact] public void CP77E_TODO() { /* exemplar content changes: one tiny valid change per type committed as a referenc */ }
    [Fact] public void CP77F_TODO() { /* authoring guard: placeholder/TODO/lorem scan and a duplicate-id scan run on chan */ }
}
```

## 5. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/Content/`
- Reachability/structure checks: re-run Plan 1 Appendix generators after the claim lands (Appendix AJ lists triggers).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 6. Scaffolding rules

1. No production file is created before a claim; this appendix is documentation.
2. Names adopt existing conventions where an owner exists (Plan 1 Appendices AH/AI).
3. Every fixture above maps to a package in the parent plan; a package without a fixture is a finding.
4. The scaffold is regenerated, not edited — see Appendix AM.
