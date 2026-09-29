# Main Scene Frame Baseline

**Captured:** 2026-09-27, 300.04 s idle Main menu session
**Runtime:** Godot 4.7.1 Mono, Compatibility renderer, Linux x86_64, Mesa llvmpipe
**Viewport:** project default 1920×1080
**Capture:** 4,500 sampled frames at a verified 15 FPS cap, after five warmup frames
**Raw evidence:** `artifacts/performance/three-plan-2026-09-27/ashfall-frame-correct-before.json` and the matching `.ui.json` file

## Baseline

| Measure | Mean | P50 | P95 | P99 | Max |
|---|---:|---:|---:|---:|---:|
| Wall-clock frame interval (ms) | 66.675 | 66.664 | 68.848 | 74.322 | 112.099 |
| Godot process time (ms) | 42.087 | 39.060 | 69.006 | 95.551 | 110.668 |
| Draw calls/frame | 41.87 | 42 | 42 | 42 | 42 |

Static memory at capture end was 268,474,788 bytes. The VM was capped at 15 FPS for a bounded background run; this is a lifecycle and idle-work comparison, not a 60 FPS hardware target result. Rendering ran through llvmpipe, so these frame times should not be compared directly with a GPU-backed player machine.

## Five measured UI callbacks

Counts cover the 300-second capture. C# duration is from the opt-in `Stopwatch` scopes and includes six scene setup/teardown callbacks per class beyond the capture-window count.

| File / callback | Calls during capture | Scope calls | Scope total (ms) | Mean per scope (ms) | Baseline classification |
|---|---:|---:|---:|---:|---|
| `FeedbackPanel.cs` | 4,500 | 4,506 | 17.922 | 0.00398 | Empty-queue polling; toast lifetime needs timing only while toasts exist |
| `UiBackgroundCarousel.cs` | 9,000 | 9,012 | 157.888 | 0.01752 | Two carousel instances; process work must follow visible transitions/parallax |
| `DailyBriefingModal.cs` | 0 | 0 | 0 | — | Modal was not instantiated on the idle menu path |
| `ExpeditionPanel.cs` | 4,500 | 4,506 | 35.812 | 0.00795 | Hidden-panel polling; only an active autoplay banner needs a timer |
| `SnapshotOrchestrator.cs` | 0 | 0 | 0 | — | Not instantiated on the idle menu path; implementation already disables process work outside capture |

`DailyBriefingModal` and `SnapshotOrchestrator` therefore have no normal idle-menu callback cost to optimize. Their lifecycle gates are still updated so an instantiated or hidden animation does not poll when idle.

## After comparison

The matching 300-second profile, per-file callback deltas, and measurement limits
are recorded in `FRAME_AFTER.md`.
