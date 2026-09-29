# Main Scene Frame Profile — After UI Process Gating

**Captured:** 2026-09-27, 300.03 s idle Main menu session
**Runtime:** Godot 4.7.1 Mono, Compatibility renderer, Linux x86_64, Mesa llvmpipe
**Capture:** verified configured cap of 15 FPS; 2,749 frames observed
**Raw evidence:** `artifacts/performance/three-plan-2026-09-27/ashfall-frame-after.json` and the matching `.ui.json` file

## Frame comparison

| Measure | Before mean / P50 / P95 | After mean / P50 / P95 | Interpretation |
|---|---:|---:|---|
| Wall-clock frame interval (ms) | 66.675 / 66.664 / 68.848 | 109.142 / 66.830 / 286.119 | P50 was nearly unchanged, but the after run had large long-tail stalls; aggregate timing is not a clean speedup comparison |
| Godot process time (ms) | 42.087 / 39.060 / 69.006 | 113.009 / 52.018 / 329.803 | High variance and tail in the after run; do not attribute these stalls to the callback edits |
| Draw calls/frame | 41.87 / 42 / 42 | 41.86 / 42 / 42 | Effectively unchanged |

The after capture still used the 15 FPS cap and the same renderer, resolution and idle Main scene, but observed only 2,749 frames in 300 seconds because of the long stalls (about 9.16 FPS overall). The cause was not established. This environment run does **not** demonstrate an overall frame-time reduction; callback counts below demonstrate only that unnecessary idle callbacks were stopped. A GPU-backed target machine and Godot's interactive profiler are still needed to characterize the frame-time tail.

## Per-file callback comparison

| File / callback | Before calls (300 s) | After calls (300 s) | Change / result |
|---|---:|---:|---|
| `FeedbackPanel.cs` | 4,500 | 0 | No empty-queue polling; process resumes when the first toast is created and stops after the final toast is dismissed |
| `UiBackgroundCarousel.cs` | 9,000 (2.00/frame) | 2,749 (1.00/frame) | The hidden ancestor carousel stopped processing; the visible carousel continues its transition/parallax animation |
| `DailyBriefingModal.cs` | 0 | 0 | Not instantiated in idle Main; typewriter runs only while a visible report is incomplete |
| `ExpeditionPanel.cs` | 4,500 | 0 | Hidden-panel polling stopped; the callback resumes only for a visible autoplay banner with time remaining |
| `SnapshotOrchestrator.cs` | 0 | 0 | Not instantiated in idle Main; its existing capture-only process lifecycle remains gated |

The C# stopwatch scope for the remaining visible carousel measured 2,755 calls / 399.337 ms total / 0.14495 ms mean. The before scope measured 9,012 calls / 157.888 ms total / 0.01752 ms mean across both instances. The higher after wall-time-per-call coincides with the run's frame stalls and cannot be used as evidence of intrinsic callback regression or improvement.

Static memory was 269,681,236 bytes after capture (baseline: 268,474,788 bytes). Draw-call and memory deltas are small relative to the runtime environment and are not claimed as optimizations.
