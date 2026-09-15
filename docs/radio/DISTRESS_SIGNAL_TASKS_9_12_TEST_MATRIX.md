// SPDX-License-Identifier: MIT
# DISTRESS SIGNAL TASKS 9–12 TEST MATRIX

> Status: **SEALED — Wave 5.** Maps the plan §14 test matrix to the sealed
> focused suites. All targets run via `bash scripts/run_test.sh <target>`.

## Stage tests (§14.1) — `DistressStageResolverTests.cs` (17)

| Matrix cell | Test |
|---|---|
| all stage thresholds | `SignalProgressesThroughAllStages` |
| clarity monotonicity | `SignalClarityIncreasesOverTime` (all 43 signals × days 0–60) |
| text changes | `SignalTextChangesAtEachStage` |
| hint timing | `OutcomeHintsRevealAtExpectedStages` |
| skipped days | `SignalProgressesThroughAllStages` (day 10/1000) |
| final-stage stability | `SignalProgressesThroughAllStages` |
| legacy parity | `StageSelectionMatchesLegacyConsumersForAllAuthoredSignals` (oracle) |
| catalog load / overrides | `AllAuthoredSignalsLoadWithMultiStageFragments`, `PrimaryAuthorityWinsForCrossFileDuplicateIds` |
| deterministic replay | `MultiStageProgressionIsDeterministic` |
| validation rules | 7 validator cases (dup day, desc day, desc clarity, empty text, missing frags, range, accept) |

## Trust tests (§14.2) — `SignalTrustTests.cs` (21)

answered / ignored / undiscovered-exclusion / legacy-expiry / trap-ignore
exclusion / trap penalty / rescue / late arrival / exactly-once ×3 / clamping
/ determinism / save-load / V5 round-trip / V4 migration / availability
weights ×3 / authenticity immutability.

## Chaining tests (§14.3) — `DistressFollowUpTests.cs` (19)

qualifying triggers ×3 / distinct content / deterministic timing / save-load
/ ignored suppression / trap suppression / trap aftermath / duplicate
scheduling+fire / reload no-refire / same-day order / terminal non-reschedule
/ validator ×3 / DTO binding / V6 round-trip / V5 migration / stage-vs-follow-up.

## Audio tests (§14.4) — `DistressAudioCueTests.cs` (10)

deterministic cue / clarity-stage cue transitions / Option-A precedence /
text-only fallbacks / definition round-trip / follow-up cue / single
detection gate / no-mutation / validator accept+reject / real-catalog clean.

## Integration tests (§14.5) — `DistressSignalTasks912ReplayTests.cs` (5)

| Matrix cell | Test |
|---|---|
| stage + audio + trust + follow-up trace | `ScenarioA_ContinuousRescueLifecycleTrace` |
| save between answer and follow-up; load on due day | `ScenarioB_SaveLoadMidLifecycle_ProducesIdenticalTrace` |
| ignore path differs exactly as authored | `ScenarioC_IgnorePath_DiffersExactlyAsAuthored` |
| trap penalty + genuine follow-up suppression + identity | `ScenarioD_TrapResponse_PenaltyApplies_GenuineFollowUpSuppressed` |
| full-lifecycle fingerprint (incl. V6 save payload) | `FullLifecycleFingerprintIsStableAcrossRuns` |
| all 43 signals load + 4 builtin fallbacks | `DistressStageResolverTests.AllAuthoredSignalsLoadWithMultiStageFragments` |
| no resolved signal respawns | `InterceptIsTheSingleDetectionGate` (Wave 4) + mission terminal guards (Wave 2) |

## Suite totals after Wave 5

Radio directory: **321 focused cases** (266 Wave-1-era + 21 trust + 19
follow-up + 10 audio + 5 replay), all green. Adjacent suites
(FactionRadioBroadcastExpansion, NpcArcData/System, RadioSaveCodecTests)
green. Gates: data-integrity, content-utilization, panel lifecycle, audio
selftest — PASS.
