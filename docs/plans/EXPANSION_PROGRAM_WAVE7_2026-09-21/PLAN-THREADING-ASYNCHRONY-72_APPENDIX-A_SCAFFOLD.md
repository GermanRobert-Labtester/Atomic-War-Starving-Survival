# PLAN-THREADING-ASYNCHRONY-72 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (content-based
variant: threading is found by usage, not by filename). Built with
**PLAN-INTEGRATION-KIT-02**'s host/main-thread contract and
**PLAN-ORPHAN-SEAL-01**'s generated-appendix convention.
**Status:** scaffolding only — no production file is created here.

## 1. Async/threading surface (171 files, by usage)

| File | Total refs | async | await | Task | Thread | lock | Interlocked | Lines |
|---|---:|---:|---:|---:|---:|---:|---:|---:|
| `Assets/Ashfall.Core/Flags/CampaignConsequenceLedger.cs` | 14 | 0 | 0 | 0 | 0 | 14 | 0 | 301 |
| `src/Main.Medical.cs` | 9 | 0 | 0 | 9 | 0 | 0 | 0 | 528 |
| `Assets/Ashfall.Core/RegionalTreatySystem.cs` | 8 | 0 | 0 | 8 | 0 | 0 | 0 | 389 |
| `Assets/Ashfall.Core/WildlifeTrappingSystem.cs` | 7 | 0 | 0 | 7 | 0 | 0 | 0 | 1621 |
| `Assets/Ashfall.Core/Medical/MedicalTreatmentCatalog.cs` | 7 | 0 | 0 | 7 | 0 | 0 | 0 | 221 |
| `Assets/Ashfall.Core/Medical/MedicalPipelineCoordinator.cs` | 6 | 0 | 0 | 6 | 0 | 0 | 0 | 747 |
| `src/Main.UiHandlers.cs` | 6 | 2 | 3 | 1 | 0 | 0 | 0 | 321 |
| `src/UI/MedicalPanel.cs` | 6 | 0 | 0 | 6 | 0 | 0 | 0 | 939 |
| `src/Main.ShelterSocial.cs` | 5 | 0 | 0 | 5 | 0 | 0 | 0 | 603 |
| `src/Host/HostCli.Collectibles.cs` | 5 | 0 | 0 | 5 | 0 | 0 | 0 | 496 |
| `Assets/Ashfall.Core/WildlifeTrappingCatalog.cs` | 4 | 0 | 0 | 4 | 0 | 0 | 0 | 389 |
| `Assets/Ashfall.Core/Medical/PsychologyAfflictionHandlers.cs` | 4 | 0 | 0 | 4 | 0 | 0 | 0 | 262 |
| `Assets/Ashfall.Core/Shelter/RoomIdentity/ShelterRoomIdentityCatalog.cs` | 4 | 0 | 0 | 4 | 0 | 0 | 0 | 460 |
| `Assets/Ashfall.Core/Save/SaveEnvelopeHelper.cs` | 4 | 0 | 0 | 1 | 0 | 0 | 3 | 320 |
| `Assets/Ashfall.Core/Institutions/InstitutionAssignmentLedger.cs` | 4 | 0 | 0 | 0 | 0 | 4 | 0 | 57 |
| `src/Main.MoralChoice.cs` | 4 | 0 | 0 | 4 | 0 | 0 | 0 | 288 |
| `src/Main.CampaignOwners.cs` | 4 | 0 | 0 | 4 | 0 | 0 | 0 | 1520 |
| `src/UI/SaveLoadPanel.cs` | 4 | 0 | 0 | 4 | 0 | 0 | 0 | 403 |
| `Assets/Ashfall.Core/Narrative/BlackProjectsArchiveSystem.cs` | 3 | 0 | 0 | 3 | 0 | 0 | 0 | 367 |
| `Assets/Ashfall.Core/Narrative/OralLoreCatalog.cs` | 3 | 0 | 0 | 3 | 0 | 0 | 0 | 240 |
| `Assets/Ashfall.Core/Radio/RadioScheduleCoordinator.cs` | 3 | 0 | 0 | 3 | 0 | 0 | 0 | 427 |
| `Assets/Ashfall.Core/Survivors/LaborProductivity.cs` | 3 | 0 | 0 | 3 | 0 | 0 | 0 | 206 |
| `Assets/Ashfall.Core/Feedback/FeedbackMessageCatalogLoader.cs` | 3 | 0 | 0 | 3 | 0 | 0 | 0 | 343 |
| `Assets/Ashfall.Core/MoralChoice/MoralChoiceIds.cs` | 3 | 0 | 0 | 2 | 1 | 0 | 0 | 260 |
| `Assets/Ashfall.Core/Telemetry/PlaySessionRecorder.cs` | 3 | 0 | 0 | 0 | 0 | 3 | 0 | 402 |
| `src/Main.ShelterBatch3.cs` | 3 | 0 | 0 | 3 | 0 | 0 | 0 | 468 |
| `src/Main.GameFlow.cs` | 3 | 1 | 1 | 1 | 0 | 0 | 0 | 926 |
| `src/Host/MedicalHostSession.cs` | 3 | 0 | 0 | 3 | 0 | 0 | 0 | 305 |
| `src/Host/WildlifeTrappingHostSession.cs` | 3 | 0 | 0 | 3 | 0 | 0 | 0 | 644 |
| `src/Host/Phase0HostSession.cs` | 3 | 0 | 0 | 3 | 0 | 0 | 0 | 1096 |
| `src/Host/HostCli.cs` | 3 | 0 | 0 | 3 | 0 | 0 | 0 | 834 |
| `src/UI/AfflictionsPanel.cs` | 3 | 0 | 0 | 3 | 0 | 0 | 0 | 448 |
| `src/UI/ChemicalDependencyPanel.cs` | 3 | 0 | 0 | 3 | 0 | 0 | 0 | 413 |
| `src/UI/FeedbackMessages.cs` | 3 | 0 | 0 | 3 | 0 | 0 | 0 | 560 |
| `src/UI/Phase0Panel.cs` | 3 | 0 | 0 | 3 | 0 | 0 | 0 | 339 |

… and 136 more files with threading refs.

## 2. Existing threading tests (0)


## 3. Host attachment

- The main-thread contract lives in the host composition (Plan 71); any
  candidate off-thread work must pass through the host session, not a raw task.
- Proposed method names: `SetupThreadingScaffold` / `SaveThreadingScaffold`
  (Plan 1 Appendix AI: collision-free).
- No day/hour coupling (Plan 1 Appendix AA) — threading work is not a tick.

## 4. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Performance/TA72ScaffoldTests.cs`

```csharp
// Scaffold skeleton — one fixture per package below.
public class TA72ScaffoldTests
{
    [Fact] public void TA72A_TODO() { /* main-thread contract: no core call off-thread */ }
    [Fact] public void TA72B_TODO() { /* async IO wrapper: cancellation + timeout */ }
    [Fact] public void TA72C_TODO() { /* static gate: banned primitives scan */ }
    [Fact] public void TA72D_TODO() { /* watchdog integration: stalled work reported */ }
    [Fact] public void TA72E_TODO() { /* deterministic outcome: same inputs, same state */ }
}
```

## 5. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/Performance/`
- Static scan for `Thread(`, `Task.Run(`, `Interlocked.` outside allowed files is part of the plan's gate; the table above is its starting allow/deny census.
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 6. Scaffolding rules

1. No production edit before a claim; this appendix is documentation.
2. The table is regenerated, never hand-edited (Plan 1 Appendix AM).
3. A file moving between allowed/denied sets is a finding with that file's owner named.
