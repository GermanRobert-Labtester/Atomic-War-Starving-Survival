========================================================================================
DEPRECATED AND FOLDED — ALL AUDIT WORK VERIFIED & COMPLETED
Status: RESOLVED & CLOSED
Date: 2026-08-08
========================================================================================

# H-1 Remediation Plan — TimeSystem Substep Watchdog

## Goal

The current Update loop in `GameBootstrap.cs` (lines 326-336) silently drops
game-time when the `_pendingGameHours` carry exceeds the substep budget. The
patient symptom: a player who opens the menu for 30 minutes, then returns to
gameplay with fast-forward on, observes a game time that lags wall time by
several hours. No warning is logged.

## Changes

1. **Detect overflow** in `Update()`:
   - Track the leftover `_pendingGameHours` when the while loop exits.
   - If `steps == MaxSubstepsPerFrame && _pendingGameHours > 0f` → overflow.
   - Drop a `Debug.LogWarning` with the dropped hours and frame context.
   - The leftover hours stay in `_pendingGameHours` (rolled into the next
     frame's budget), so no time is *lost* — but the player is warned that
     they are running on the substep budget.

2. **Track counters** as read-only properties:
   - `int DropEventCount` — number of frames with overflow.
   - `float TotalDroppedGameHours` — cumulative game hours that were carried
     into the next frame.
   - `int PeakSubstepsInOneFrame` — high-water mark of substeps per frame.

3. **Test coverage** in `Assets/Tests/PlayMode/TimeSystemWatchdogTests.cs`:
   - **Overflow detection**: pre-load `_pendingGameHours` to a value that
     exceeds `MaxSubstepsPerFrame * MaxGameHoursPerStep`; assert that the
     `DropEventCount` increments after one `Update` call.
   - **Carry-over correctness**: assert the leftover is preserved (no time
     is lost) and consumed in subsequent frames.
   - **No false positive**: a small delta that fits in the substep budget
     does NOT increment the counters.

4. **Surface the counters** in the diagnostics overlay. The overlay does
   not exist yet (this is a separate M-1 issue). For now, expose them as
   public read-only properties; the future overlay reads them.

## Why not just raise `MaxSubstepsPerFrame`?

Larger substep budget = more work per frame = hitch when the budget is hit.
A 30-minute menu pause at 3x scale produces 540 game-hours. With
MaxGameHoursPerStep=1, that's 540 substeps. At a target of 16ms/frame on
typical hardware, each substep is <30µs (cheap), but the cumulative
substep time can dominate a single frame.

The right answer: keep the per-frame substep budget low enough to stay
under 16ms, and warn the player when time is being carried. The carry
will recover over the next few frames at 3x scale.

## Files

- `Assets/_Game/Core/GameBootstrap.cs` — add overflow detection + counters
- `Assets/Tests/PlayMode/TimeSystemWatchdogTests.cs` — PlayMode tests
  (require a running Update loop)
- `Assets/_Game/Core/TimeSystem.cs` — possibly add a helper for overflow
  (out of scope if GameBootstrap is the only consumer)

## Risk

- The watchdog is read-only observability. It does not change game-time
  semantics. Players who see the warning can decide to lower fast-forward
  or wait for the system to catch up.
- A noisy warning (every overflow frame) would flood the log. We use
  `Debug.LogWarning` and rely on Unity's log throttling for spam. We also
  accumulate over a window to avoid log spam.
