# PLAN-AUDIO-MIX-AUTHORITY-97 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, batch 3).
**Scaffolding authority:** [`PLAN-AUDIO-CONDITION-TRUTH-255`](../EXPANSION_PROGRAM_WAVE18_2026-09-21/PLAN-AUDIO-CONDITION-TRUTH-255.md) — adopt the authority's patterns rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|
| `ACT-255A` | device model + band table. |
| `ACT-255B` | fidelity-effect tests per band. |
| `ACT-255C` | repair path + conservation. |

## 2. Source inventory (30 files, Core + host)

| File | Lines |
|---|---:|
| `Audio/AudioAccessibilityCoordinator.cs` | 294 |
| `Audio/AudioSettingsCodec.cs` | 189 |
| `Audio/AudioSettingsData.cs` | 175 |
| `Audio/CassettePlaybackSystem.cs` | 237 |
| `Audio/CassetteSetCatalogLoader.cs` | 73 |
| `Audio/ReactiveAmbienceEvaluator.cs` | 167 |
| `Audio/ScarcityAudioStateMachine.cs` | 300 |
| `Audio/ShelterAcousticDirector.cs` | 148 |
| `Audio/ShelterAudioCueCatalog.cs` | 55 |
| `AudioConditionSystem.cs` | 128 |
| `Needs/SleepAcousticRestEngine.cs` | 230 |
| `Radio/AcousticDirectionFindingCatalog.cs` | 137 |
| `Radio/DistressAudioCueResolver.cs` | 60 |
| `Radio/DistressRescueMissionManager.cs` | 911 |
| `Radio/RescueDispatchPreflight.cs` | 42 |
| `Radio/RescuedArcProjection.cs` | 153 |
| `Shelter/MachineIdentity/MachineTellAudioSync.cs` | 137 |
| `host:Audio/AudioConditionHostBridge.cs` | 88 |
| `host:Audio/AudioCueCatalog.cs` | 505 |
| `host:Audio/AudioEventBridge.cs` | 633 |
| `host:Audio/AudioManager.cs` | 1012 |
| `host:Audio/AudioSelfTest.cs` | 1465 |
| `host:Audio/AudioSettings.cs` | 338 |
| `host:Audio/AudioStateCoordinator.cs` | 124 |
| `host:Audio/ExpansionAudioBridge.cs` | 170 |
| `host:Audio/ShelterAcousticBridge.cs` | 78 |
| `host:Audio/ShelterAudioController.cs` | 209 |
| `host:Audio/ShelterOperationsAudioBridge.cs` | 369 |
| `host:Audio/SurfaceAmbienceController.cs` | 186 |
| `host:Main.Audio.cs` | 51 |

## 3. Data bindings

| Catalog | Records/shape |
|---|---|
| `audio_logs_expansion_05.json` | object(2 keys) |
| `acoustic_triangulation_catalog.json` | object(4 keys) |
| `audio_cues.json` | object(2 keys) |
| `shelter_audio_cues.json` | object(3 keys) |
| `cassette_sets.json` | 12 |
| `audio_accessibility_cues.json` | object(3 keys) |

## 4. Host attachment

- Candidate host partials: `Main.Audio.cs`
- Proposed method names: `SetupAudioAMXScaffold` / `SaveAudioAMXScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Audio/AMX97ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class AMX97ScaffoldTests
{
    [Fact] public void AMX97A_TODO() { /* bus layout + routing doc; one host implementation; default bus resource checked  */ }
    [Fact] public void AMX97B_TODO() { /* loudness targets + measurement script over the existing catalog (report, no asse */ }
    [Fact] public void AMX97C_TODO() { /* ducking rules + tests over the evaluator state machine (priority table). */ }
    [Fact] public void AMX97D_TODO() { /* caption table generation for information-bearing cues + toggle wiring test. */ }
    [Fact] public void AMX97E_TODO() { /* orphan wiring packages (two types) or explicit deferral notes with owners. */ }
    [Fact] public void AMX97_AuthorityConformance_TODO() { /* pattern parity with PLAN-AUDIO-CONDITION-TRUTH-255 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/Audio/`
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
