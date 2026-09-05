# Micro-Location Determinism Contract (F10)

Verified against source on the F9–F12 flagship wave. Every claim here is
pinned by a test in `MicroLocationDeterminismTests` /
`NarrativeEncounterDepletionTests` / `MicroLocationPersistenceWaveTests`.
Where the plan anticipated a different architecture, the verified reality is
documented instead (plan F10.9 escape clause).

## 1. Authoritative RNG source

One stream per expedition session: the host constructs a single
`SeededRng` (xorshift64\*, `Assets/Ashfall.Core/HostDefaults.cs`) and passes
the **same instance** to `ExpeditionSystem.TickHours` and to
`ExpeditionEncounterBridge`, which forwards it to
`NarrativeEncounterSystem.SelectEncounter`. There is no second RNG anywhere
in the selection path — gated by
`MicroLocationDeterminismTests.SelectionPath_IntroducesNoIndependentRng`
(source scan: no `new SeededRng(`, `new Random(`, `Guid.NewGuid`,
`DateTime.Now` in `NarrativeEncounterSystem.cs`, `ExpeditionEncounterBridge.cs`,
`ExpeditionSystem.cs`) and behaviorally by `SameStreamState_ReplaysIdenticalSelection`.

## 2. Seed/state ownership

- The expedition session seed is host-owned (`ExpeditionHostSession.DemoSeed`
  for the demo/default session; campaigns thread their own seed).
- `SeededRng` exposes `Seed` but **no state getter / state constructor on the
  current trunk** (removed upstream during this wave). A save-boundary
  checkpoint is therefore a **draw count** taken from a test-only counting
  wrapper; a fresh world replays that many `NextDouble()` draws from the same
  seed (every public draw consumes exactly one `NextRaw()`, so replay position
  is exact). See `MicroLocationDeterminismHarness` header comment.

## 3. Encounter selection call chain (verified)

```
ExpeditionSystem.TickHours(hours, rng)              single stream
  -> per active expedition (ordinal-sorted keys)
     ApplyStaminaDrain -> RollEncounter(exp, rng)   ExpeditionSystem.cs
       rng.NextDouble() < encounterChancePerTick (stance/weather-adjusted)
       -> OnEncounterTriggered(exp)
          -> ExpeditionEncounterBridge.Surface(exp)
             -> NarrativeEncounterSystem.SelectEncounter(stance, danger, locationId, rng)
                pass 1: sum eligible non-filtered weights (no RNG)
                pass 2: one rng.NextDouble() roll over the filtered total
             -> OnSurfaced(dto) -> NarrativeEncounterSystem.EnqueuePending(...)
player choice
  -> bridge.ResolveChoice / NarrativeEncounterSystem.TryResolve   (no RNG)
  -> host ApplyEncounterConsequences: item -> journal -> location -> flag (no RNG)
```

## 4. Candidate ordering rule

The catalog iterates in **registration order** (`List<EncounterDefinition>
_catalog`): `narrative_encounters.json`, then `narrative_encounters_npc_arcs.json`,
then `micro_locations.json` (`NarrativeEncounterCatalogLoader.Load`). File
order inside each file is JSON array order. Registration order is stable
within a process and across processes because loading is a fixed sequence of
fixed files. No `HashSet`/dictionary iteration feeds the weighted roll.

## 5. Depletion filtering rule

Depleted encounters are excluded in **both** passes **before** weighting, so
they neither distort the weight sum nor consume a zero-weight roll
(`NarrativeEncounterSystem.SelectEncounter`, F1 comment). The depleted set is
`HashSet<string>` with `StringComparer.Ordinal`, never iterated for selection.
Weather-gate-filtered and zero-weight candidates are likewise excluded before
the roll. Verified by `DepletedCandidate_Filtering_IsDeterministic` and
`DepletingEncounter_IsExcludedFromSelection`.

## 6. Cadence / cooldown rule

**No cooldown/cadence state exists for micro-locations.** Per-tick exposure is
gated solely by `encounterChancePerTick` (authored per destination, stance-
and multiplier-adjusted in `RollEncounter`), weights, danger gates,
required-location gates, weather gates, and depletion. There is no cooldown
state to persist across a save boundary. (Plan F10.12 is satisfied by proving
the chance-roll + selection pipeline deterministic; nothing else carries over.)

## 7. Save/load RNG continuation rule

Persisted across saves (production envelope via `NarrativeSaveStore`):
depletion set, pending surfaced queue, resolution history, cumulative
morale/guilt. **Not persisted:** the host session's RNG draw position — a
reloaded host restarts its encounter stream from `DemoSeed`. Consequence:
post-reload sequences are deterministic for the same seed but are not a
continuation of the pre-save draw sequence. Core harnesses prove strict
continuation parity at the serialization boundary:
`SaveAtTick4_Continuation_EqualsUninterruptedEightTicks` (INV-09) replays the
checkpointed draw count into a fresh world and requires trace equality with an
uninterrupted 8-tick run.

## 8. Operations forbidden from consuming RNG

- eligibility / weight queries (`GetEffectiveWeight`), `IsDepleted`, `Find`,
  catalog scans, pending-queue reads, depletion snapshots (`CaptureState`)
- diagnostics, tracing, instrumentation
  (`ContentUtilizationInstrumentation` is passive)
- save capture, restore, and all host consequence application

Pinned by `EligibilityMetadata_ConsumesZeroRngDraws` (exact count) and
`SelectEncounter_ZeroEligibleContext_ConsumesZeroDraws` (no candidates ⇒ no
roll). The documented priced operation is `SelectEncounter`: **exactly one
draw** per call when any candidate is eligible, zero when none is.

## 9. Test harness coverage

`Ashfall.Core.Tests/MicroLocationDeterminismHarness.cs` rebuilds the
production wiring (full narrative catalog, registered expedition
destinations, location-typed scavenging authority, one shared counting-wrapped
stream) and records a passive per-tick trace (tick, surfaced encounter,
micro flag, ordinal depletion snapshot, cumulative draws, resolved count).
Coverage:

| Gate | Test |
|---|---|
| named seed scenarios repeat (seeds 42/99/7, 8 ticks) | `Seed42_*`, `Seed99_*`, `Seed7_*` |
| same-process repetition | runA/B/C in `Seed42_*` |
| save@tick4 continuation == uninterrupted | `SaveAtTick4_Continuation_EqualsUninterruptedEightTicks` |
| metadata consumes zero draws | `EligibilityMetadata_ConsumesZeroRngDraws` |
| empty context consumes zero draws | `SelectEncounter_ZeroEligibleContext_ConsumesZeroDraws` |
| no independent RNG in selection path | `SelectionPath_IntroducesNoIndependentRng` |
| stream-identical replay | `SameStreamState_ReplaysIdenticalSelection` |
| depletion filtering deterministic | `DepletedCandidate_Filtering_IsDeterministic` |
| 100-seed sweep, zero divergences | `HundredSeedHarness_HasZeroDivergences` |

## 10. Mismatch diagnostics

`MicroLocationDeterminismHarness.AssertTracesEqual` prints the seed,
destination, first divergent tick, and expected/actual trace entries
(tick, encounter, micro flag, depletion set, resolved count, cumulative
draws) followed by both canonical traces. The 100-seed sweep aggregates all
divergent seeds into its failure message. Canonical format:

```
seed=42|exp=loc_the_allotments
tick=1|enc=micro_crashed_truck|micro=1|dep=[]|resolved=0|draws=7
...
final_phase=3
```
