# C2 — Decision

**Task:** Wave 8 Part 2, TASK C2
**Status:** PARTIAL — avatar truth sealed; equipment split; expedition awaiting signature
**Date:** 2026-09-17
**Decider:** foreman/user (user authorized C2 on 2026-09-17; the equipment/expedition
contract is the separately-gated decision per C2 §1.6 / Decision gate)

## Decisions

### D1 — Avatar placeholder (EXECUTED, no signature required)

The recorded out-of-scope decision ("no survivor-avatar owner exists") already
exists, so the delete-vs-stub choice was made by the plan's own preference:
**delete the dead path and its misleading TODO.** `RefreshSurvivorVisuals` and
its five call sites were removed from `src/Main.Plans190_193.cs` and
`src/Main.Bionics.cs`. No presentation illusion was added. Host build 0/0.

### D2 — Equipment restriction (SPLIT, blocked)

**Do not implement inside C2.** The equipment model has no handedness or
limb-requirement field, so any arm-state equip restriction requires a
`ItemDefinition`/`EquipSlot` schema redesign. Per C2 §2 this becomes a separate
signed package. The existing debt row stays **BLOCKED → SPLIT-SEALED** with the
equipment half named as the follow-up.

*(Interim truth: amputation still does not restrict gear. No presentation-only
limb illusion is used as a substitute.)*

### D3 — Expedition movement consumer (EXECUTED 2026-09-17)

**Foreman signature received:** "I approve C2-D3". Implemented as proposed:

- **Core `ExpeditionSystem`:** added `ExpeditionState.survivorSpeedMultiplier`
  (default 1; additive, old saves neutral) and `ExpeditionEstimate.survivorSpeedMultiplier`;
  `Start` / `Estimate` / `PreviewStart` / `ExecuteStart` gained an optional
  `float survivorSpeedMultiplier = 1f`; `AdvanceOutbound` and `AdvanceInbound`
  now multiply `SurvivorTravelMultiplier(exp)` after vehicle/weather. Bounded by
  `MinSurvivorSpeedMultiplier = 0.15f` / `MaxSurvivorSpeedMultiplier = 2f`;
  absent/zero reads as intact 1.0. Zero RNG.
- **Host `ExpeditionHostSession`:** new `SurvivorMovementSpeedProvider`
  (`Func<string, float>`) sampled once at dispatch into state, and into
  `EstimateExpedition` (new optional `survivorId`) so estimate and runtime agree.
- **Composition:** `src/Main.Expeditions.cs` binds the provider to
  `AmputationSystem.GetMovementSpeedMultiplier` (lazy medical setup; unbound ⇒ 1).
- **UI:** `ExpeditionPanel.UpdateEstimateLine` passes the selected survivor.
- **Tests:** `Ashfall.Core.Tests/Expeditions/C2AmputationTravelTests.cs` (7) —
  intact parity, amputated leg slows estimate + runtime, medical→estimate
  contract, dispatch sampling + legacy default, bound clamp, save round-trip.

Verified: C2 tests 7/7; full Core suite 11,716/11,716; host build 0/0;
`--expedition-selftest`, `--campaign-journey-selftest`,
`--panel-bind-lifecycle-selftest` PASS.

## Result

- Phase 4: **DONE**.
- Phases 1 (evidence): **DONE** (`C2_PREMISE_EVIDENCE.md`).
- Phase 2 (equipment): **SPLIT / BLOCKED** (schema redesign, separate package).
- Phase 3 (expedition): **DONE** (D3 executed under the 2026-09-17 signature).
- Phase 5 verification: green (7 new tests + full suite + three host selftests).
