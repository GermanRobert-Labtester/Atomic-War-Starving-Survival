// SPDX-License-Identifier: MIT
# DISTRESS SIGNAL STAGE CONTRACT (Tasks 9–12 Wave 1)

> Status: **SEALED — Wave 1.** This is the live authority for distress-signal
> message-stage semantics. Baseline evidence:
> `docs/radio/DISTRESS_SIGNAL_TASKS_9_12_BASELINE.md`.

## 1. Stage authority

`message_fragments` on `DistressSignalDefinition`
(`Assets/Ashfall.Core/Radio/RadioDistressSystem.cs`) **is** the stage model.
No redundant `stages` field exists and none may be added (Wave 0 decision gate
PC-2). Each fragment is one presentation/intelligence stage of the SAME
canonical signal identity.

## 2. Fragment grammar

| Field | Type | Rule |
|---|---|---|
| `day` | int | **Absolute campaign-day threshold.** Stage N becomes audible when `campaignDay >= fragments[N].day`. Strictly ascending across fragments. |
| `clarity` | float | Repository-standard 0..1 scale. Non-decreasing across fragments (ascending in all authored data). Propagation multiplies by the atmospheric attenuation factor downstream — the authored value is never mutated. |
| `text` | string | Player-facing transmission text. Non-empty. May repeat across stages only for authored automated-loop signals (currently `freq_distress_367_9`). |
| `outcome_hint` | string, optional | Player-facing intelligence clue for this stage (Wave 1 additive field). Empty/absent = no hint at this stage. Must NOT expose hidden truth (trap authenticity, quest internals) the player has not legitimately learned. |

## 3. Resolver (sole selection authority)

`DistressStageResolver` (`Assets/Ashfall.Core/Radio/DistressStageResolver.cs`):

- `ResolveStageIndex(signal, campaignDay)` → first index of the highest
  fragment day `<= campaignDay`; falls back to index 0 when no threshold is
  satisfied yet (legacy fallback — stage 0 is audible from day 0).
- `Resolve(signal, campaignDay)` → `DistressStageView?` (index + fragment);
  null when the signal has no fragments (builtin-only fallback signals keep
  their legacy `SourceName` behavior).
- Ties on equal day keep the first occurrence (legacy parity; validator
  forbids duplicate days in authored data).

Properties: side-effect free, deterministic, no audio calls, no quest
mutation, no trust mutation. Both former duplicated selection loops
(`RadioTuner.EvaluateFrequency`, `RadioPropagation.Evaluate`) now delegate
here; propagation applies its clarity attenuation AFTER the resolver.

## 4. Derived-state rule

Stage index, text, clarity, and hint are **purely derivable** from
`signal definition + current campaign day`. Nothing stage-related is persisted
beyond the pre-existing `ActiveDistressSignal.HighestClarity` (monotonic,
already saved). Campaign time is monotonic, so stages can never regress, and
multi-day skips advance directly to the correct stage.

## 5. Validation (data-integrity gate)

`CatalogIntegrityValidator.ValidateDistressSignalStages` runs inside the
permanent `--data-integrity-selftest` gate over both catalogs:

- within-file duplicate `frequency_id` → **error**
- cross-file duplicate → **warning** (documented primary-wins override;
  `radio_distress_signals.json` loads last and wins)
- missing/empty `message_fragments` → error
- duplicate stage day / descending stage day → error
- clarity outside [0,1] / decreasing → error
- empty stage text → error
- `outcome_hint` present but empty → error

## 6. Scope (current evidence)

- 47 runtime-registered signals: 43 JSON-backed (25 primary + 23 expansion −
  5 primary-wins overrides) + 4 builtin-only fragment-less fallbacks.
- All 43 JSON-backed signals carry 2–7 ascending stages.
- Stage tests: `Ashfall.Core.Tests/Radio/DistressStageResolverTests.cs`
  (17 cases, including the legacy-consumer parity oracle across all signals
  × days 0–60).

## 7. What a stage is NOT

A stage is not quest progression, an expedition stage, an encounter node, a
survivor-state transition, or a trust event. Reaching a later stage must never
mutate quest, expedition, trust, or encounter state (Task 9 → 10/11 rule).
