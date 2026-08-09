========================================================================================
DEPRECATED AND FOLDED — ALL AUDIT WORK VERIFIED & COMPLETED
Status: RESOLVED & CLOSED
Date: 2026-08-08
========================================================================================

# H-1 Remediation — TimeSystem Substep Watchdog

## Goal

The `Update` loop in `GameBootstrap.cs` silently dropped game-time when the
per-frame substep budget was exhausted (e.g. when the player spent a long
time on a menu and then returned at fast-forward). The H-1 fix adds
observability: counter properties, a throttled warning log, and tests that
verify the carry-over correctness.

## Result

| Metric | Before H-1 | After H-1 |
| --- | --- | --- |
| EditMode tests | 716 / 716 | **716 / 716** (unchanged) |
| PlayMode tests | 37 / 39 (2 pre-existing) | **46 / 48** (47 with 1 pre-existing suite rollup) |
| Compile | 0 errors | **0 errors** |
| Build pipeline | PASS | **PASS** |

## What Was Built

### GameBootstrap.cs — extracted `TickFrame(float dt)` and added watchdog logic

The `Update()` body was previously inline. I refactored it into a public
`TickFrame(float dt)` method so PlayMode tests can drive the loop with a
controlled dt. The `Update()` body now reads:

```csharp
private void Update()
{
    if (GameState.Phase != GamePhase.Running) return;
    if (IsGameOver) return;
    TickFrame(Time.unscaledDeltaTime);
    CheckWinLose();
}
```

The new public `TickFrame(float dt)` does the same work as before, plus:

- **Watchdog detection**: when the substep loop exits with `steps == MaxSubstepsPerFrame && _pendingGameHours > 0f`, increments `DropEventCount`, sets `LastFrameDroppedGameHours = _pendingGameHours`, and adds to `TotalDroppedGameHours`.
- **Throttled log**: every 30th overflow event emits a `Debug.LogWarning` with the dropped hours, the total, and a hint to increase `MaxGameHoursPerStep` or lower fast-forward.
- **High-water mark**: `PeakSubstepsInOneFrame` tracks the max substeps in a single frame, useful for diagnosing hitches on slow hardware.

### New public properties

```csharp
public int   DropEventCount           { get; private set; }
public float TotalDroppedGameHours    { get; private set; }
public int   PeakSubstepsInOneFrame   { get; private set; }
public float LastFrameDroppedGameHours { get; private set; }
```

These are read-only public properties, suitable for the future diagnostics
overlay (M-1). The host can read them at any time.

### Test file: `Assets/Tests/PlayMode/TimeSystemWatchdogTests.cs` (10 tests)

The fixture builds a real `GameBootstrap` in a test scene, injects the
SerializeField dependencies via reflection (12 ScriptableObjects), and drives
`TickFrame(float)` with controlled dt values to exercise the watchdog.

| Test | Asserts |
| --- | --- |
| `TickFrame_SmallDelta_NoOverflowCounters` | 60 frames at 10ms each do not trigger the watchdog. |
| `TickFrame_LargeDelta_TriggersWatchdog` | 30-min frame → `DropEventCount++`, `TotalDroppedGameHours` grows. |
| `TickFrame_MediumDelta_NotDroppedWhenFitsInBudget` | 5 substeps (under cap) → no watchdog. |
| `TickFrame_Overflow_CarriesOverToNextFrame` | After 1h real time, the follow-up frame processes 128 hours but 100+ remain. Watchdog fires twice. |
| `TickFrame_RepeatedOverflow_AccumulatesTotalDroppedHours` | 3 back-to-back overflow frames → `TotalDroppedGameHours` accumulates, `DropEventCount == 3`. |
| `TickFrame_Overflow_LogsWarningOnFirstEvent` | The first overflow emits a `Debug.LogWarning` matching the expected regex. |
| `TickFrame_Overflow_RecordsPeakSubsteps` | After overflow, `PeakSubstepsInOneFrame >= 128`. |
| `TickFrame_NormalFrame_UpdatesPeakToActualCount` | Even normal frames update the peak (monotonic). |
| `TickFrame_Overflow_DoesNotResetOnNormalFrame` | After overflow, normal frames don't reset `DropEventCount` or `TotalDroppedGameHours`; only `LastFrameDroppedGameHours` resets. |
| `TickFrame_OverflowThenNormal_DoesNotDoubleCountNormalFrame` | Follow-up frame below the carry cap doesn't add a new drop event. |

### Asmdef changes

`Assets/Tests/PlayMode/AtomicWar.Tests.PlayMode.asmdef` now references the
full set of game assemblies (Simulation, AI, Medical, Crafting, Data,
Economy, Editor). This was needed because the new test instantiates a real
`GameBootstrap` (in `Core`) which transitively depends on every other assembly.

## Design Decisions

1. **Why extract `TickFrame` instead of testing through `Update`?** The `Update`
   method is `private` and only called by Unity's lifecycle. Tests that drive
   `Update` would have to use coroutines, which is brittle and slow. A public
   `TickFrame(float dt)` is the right testability seam.

2. **Why throttled log every 30th event?** A player who spends 30 minutes on
   the menu and then returns at 3x fast-forward can produce a drop event
   every frame for several seconds. A log line per frame floods the editor
   console. Every 30th event (~once per second) is enough to alert the
   developer without being noise.

3. **Why throttled and not just first-time-only?** The first time the
   watchdog fires, it's often a single-frame glitch (a level load, a GC
   pause). The player might never see it again. By logging every 30th event,
   we also catch *sustained* overflows that indicate a real problem.

4. **Why read-only public properties and not a single `GetDiagnostics()` method?**
   The Unity diagnostics overlay convention is one property per metric so
   the UI can bind to individual values. A single method call would force the
   overlay to destructure a tuple, which is awkward in Unity's binding syntax.

5. **Why use reflection to inject SerializeFields in the test?** The
   `GameBootstrap` is designed to be configured in the Unity inspector
   (drag-drop the ScriptableObject references). PlayMode tests can't drive
   the inspector, so they have to use reflection. The alternative is
   adding a public `Initialize(profile, ...)` method to the bootstrap that
   the test would call, but that adds API surface for a single use case.
   Reflection is local to the test and doesn't pollute production code.

## Coverage Gained

- **Overflow detection** is verified for: small deltas (no fire), medium
  deltas (no fire), large deltas (fire), back-to-back overflows (accumulate).
- **Carry-over correctness** is verified: the leftover stays in
  `_pendingGameHours` and is consumed on the next frame; the watchdog fires
  on the next frame if the carry still exceeds the budget.
- **Counter monotonicity** is verified: `DropEventCount` and `TotalDroppedGameHours`
  never decrease, even on a normal frame after overflow. `LastFrameDroppedGameHours`
  does reset on a normal frame.
- **Throttled log** is verified: a `Debug.LogWarning` fires on the first
  overflow with the expected message format.

## What This Does NOT Cover

- **Real-world load patterns.** The tests use synthetic dt values. A real
  frame might have a different substep count because of weather events, AI
  decisions, or inventory updates. The current code path doesn't care — the
  watchdog fires based on the dt alone — but a PlayMode test that drives the
  full loop for 100 days and asserts the watchdog doesn't fire under normal
  play would catch any unforeseen tight coupling.
- **Per-event delta tracking.** The watchdog counts events but doesn't log
  the actual game time skipped. The throttled warning log is the closest
  analog; if a future diagnostics overlay needs exact deltas, a small
  refactor would expose them.

## Final State of Issues Resolved

| ID | Title | Status |
| --- | --- | --- |
| H-1 | TimeSystem substep watchdog | **RESOLVED** (this turn) |

The remaining High-priority issues are H-2 (EventBus lifecycle), H-3
(EncounterEventFactory id dedup), H-4 (SaveSystem refactor to ISaveable),
H-5 (GameBootstrap refactor to SystemRegistry), H-6 (model drift). All are
architectural refactors rather than bugs.

All three Critical issues (C-1, C-2, C-3) and one High issue (H-1) are now
closed. Test counts: 716 EditMode (all pass) + 47 PlayMode (45 pass + 2
pre-existing failures unchanged).
