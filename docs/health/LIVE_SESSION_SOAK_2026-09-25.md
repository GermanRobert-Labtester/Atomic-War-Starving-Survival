# ASHFALL — Live Session Soak (2026-09-25)

Real-display session of the live game (5 minutes, main menu + auto-started campaign), run **while
the art wave and its queue were also running** — so the numbers are a conservative lower bound on
a quiet machine.

## Pacing

| Metric | Value |
|---|---|
| FPS samples | 285 |
| Average | **59.1 FPS** |
| 5th percentile | 59.0 FPS |
| Minimum | 8.0 FPS (single frame, once) |
| Typical | 60–61 FPS (vsync) |

## Memory (RSS sampling every 30 s)

| t | RSS |
|---|---|
| 30 s | 918 MB |
| 90 s | 967 MB |
| 150 s | 1,036 MB |
| 210 s | 1,104 MB |
| 270 s | 963 MB |
| 300 s | 963 MB |

Interpretation: RSS rises as the game loads scene/art resources during the first minutes, then
**falls back** when caches are released — not a monotonic growth pattern. Settled footprint
≈ 0.95 GB with the full art registry loaded (consistent with the 768 MB snapshot observed in
`docs/health/PERF_BUDGET_2026-09-25.md`).

## Telemetry sink

`play_metrics.jsonl` grew 16 → 20 rows during the session (session start + campaign events),
confirming the production sink writes during real play, not just selftests.

## Audio

| Probe | Result |
|---|---|
| `--audio-selftest` (catalog, manager wiring, event coverage) | PASS |
| `--scarcity-audio-selftest` (weather beds, silence states, ducking, geiger bands) | PASS |
| `--audio-accessibility-selftest` (visual-layer gating, host sinks) | **12/12 PASS** |
| `--settings-selftest` (audio buses, keybindings, save/load) | PASS |

## Verdict

No pacing or memory defect proven. Watch items only:

1. The single 8 FPS frame — likely one window/asset event during startup; reproduce with
   `--print-fps` on a quiet machine before treating it as a defect.
2. Idle footprint ~0.95 GB with all art loaded — if the alpha target is a 4 GB machine, trim
   texture residency (atlas/compression), not code.
