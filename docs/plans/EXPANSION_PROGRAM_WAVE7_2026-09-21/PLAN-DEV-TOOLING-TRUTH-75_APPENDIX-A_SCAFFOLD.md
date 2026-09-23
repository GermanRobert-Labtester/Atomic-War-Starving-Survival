# PLAN-DEV-TOOLING-TRUTH-75 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator. Built with the two
session toolkit plans as its authority: the integration pattern set of
**PLAN-INTEGRATION-KIT-02** (host session, save store, day owner, panel route,
CLI probe, focused fixture) and the appendix/maintenance convention of
**PLAN-ORPHAN-SEAL-01** (generated, reproducible, never hand-edited).
**Status:** scaffolding only — no production file is created by this appendix.
A claim still owns the edits; this page exists so the claim starts from real
paths, real names, and a fixture skeleton derived from this plan's own packages.

## 1. Source inventory (42 files)

| File | Lines |
|---|---:|
| `BrineWaterHeadlessDemo.cs` | 131 |
| `CensusHeadlessDemo.cs` | 128 |
| `Cluster12CHeadlessDemo.cs` | 110 |
| `Combat/CombatHeadlessDemo.cs` | 289 |
| `CrossingArbitrationHeadlessDemo.cs` | 166 |
| `CrossingHeadlessDemo.cs` | 113 |
| `DeepCoastHeadlessDemo.cs` | 380 |
| `Disease/DiseaseHeadlessDemo.cs` | 490 |
| `DutyRoster/DutyRosterHeadlessDemo.cs` | 153 |
| `Economy/EconomyHeadlessDemo.cs` | 103 |
| `Endgame/EndgameHeadlessDemo.cs` | 102 |
| `EndingsHeadlessDemo.cs` | 105 |
| `Expeditions/ExpeditionHeadlessDemo.cs` | 120 |
| `Expeditions/ReconTelemetryHeadlessDemo.cs` | 94 |
| `Foundry/SilentFoundryHeadlessDemo.cs` | 188 |
| `Greenhouse/GreenhouseHeadlessDemo.cs` | 110 |
| `HoldfastHeadlessDemo.cs` | 115 |
| `IceRoadHeadlessDemo.cs` | 138 |
| `InfrastructureHeadlessDemo.cs` | 168 |
| `LedgerDebtHeadlessDemo.cs` | 337 |
| `Medical/MedicalHeadlessDemo.cs` | 108 |
| `Muster/FactionEcologyHeadlessDemo.cs` | 199 |
| `Muster/MusterHeadlessDemo.cs` | 103 |
| `Narrative/NarrativeHeadlessDemo.cs` | 93 |
| `Narrative/TravelEncounterHeadlessDemo.cs` | 159 |
| `Performance/PerfResult.cs` | 57 |
| `Performance/PerfSample.cs` | 44 |
| `Performance/PerfSession.cs` | 165 |
| `Performance/PerfStatistics.cs` | 123 |
| `Performance/PerfStopwatch.cs` | 58 |
| `Performance/PerfTestMarker.cs` | 10 |
| `Performance/PerfWorkloadContext.cs` | 47 |
| `Performance/ScaleTier.cs` | 100 |
| `Performance/WorkloadProfile.cs` | 83 |
| `Quests/PersonalQuestHeadlessDemo.cs` | 100 |
| `Shelter/GeothermalAquiferHeadlessDemo.cs` | 119 |
| `StandingRecord/StandingRecordHeadlessDemo.cs` | 144 |
| `Survivors/SurvivorsHeadlessDemo.cs` | 77 |
| `TravelingCaravanHeadlessDemo.cs` | 72 |
| `UtilityAI/UtilityAiHeadlessDemo.cs` | 96 |

… and 2 more.

## 2. Data bindings

No catalog name-matched the domain tokens; verify loader paths before claiming a data dependency.

## 3. Host attachment

- Candidate host partials: none — new attachment or headless-only

- Proposed setup method: `SetupPerformanceScaffold` · proposed save method: `SavePerformanceScaffold` (checked against the 228 existing setup names in Plan 1 Appendix AI — extend the real owner name if one exists)
- Loop coupling: see Plan 1 Appendix AA before adding any day/hour step; a new timer is forbidden.

## 4. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Performance/DT75ScaffoldTests.cs`

```csharp
// Scaffold skeleton — derived from this plan's packages below.
// Fixtures assert one package each; replace the TODO bodies at claim time.
public class DT75ScaffoldTests
{
    [Fact] public void DT75A_TODO() { /* dev-console contract: command list, permissions, and a `dev_mode` build flag tha */ }
    [Fact] public void DT75B_TODO() { /* seed/state commands: set seed, jump to day, grant/remove item — all with a confi */ }
    [Fact] public void DT75C_TODO() { /* overlay suite: owner timing, key needs, dose, roster, active events; measured co */ }
    [Fact] public void DT75D_TODO() { /* inspector commands: `inspect item <id>`, `inspect catalog <id>`, `inspect owner  */ }
    [Fact] public void DT75E_TODO() { /* fault injection: `inject bad-catalog`, `inject owner-overrun`, `inject bad-mod`  */ }
    [Fact] public void DT75F_TODO() { /* release guard: an export check fails if dev commands/overlays are compiled into  */ }
}
```

## 5. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/Performance/`
- Reachability/structure checks: re-run Plan 1 Appendix generators after the claim lands (Appendix AJ lists triggers).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 6. Scaffolding rules

1. No production file is created before a claim; this appendix is documentation.
2. Names adopt existing conventions where an owner exists (Plan 1 Appendices AH/AI).
3. Every fixture above maps to a package in the parent plan; a package without a fixture is a finding.
4. The scaffold is regenerated, not edited — see Appendix AM.
