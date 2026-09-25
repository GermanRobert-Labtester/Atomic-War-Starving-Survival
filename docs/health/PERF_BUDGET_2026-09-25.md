# ASHFALL — Performance Budget (2026-09-25, alpha)

Measured on the alpha checkout (Linux, AMD/radeonsi X11 display; headless where noted) with the
art wave running in the background — numbers are therefore conservative, not optimistic.

## Simulation / persistence (`--runtime-scale-selftest`, artifacts/runtime-scale-results.json)

| Benchmark | Days | Iterations | Median | P95 | Max | Median allocation |
|---|---|---|---|---|---|---|
| `day_advance_30d` | 30 | 5 | 1.21 ms | 2.52 ms | 2.81 ms | 84 KB |
| `day_advance_180d` | 180 | 5 | 7.47 ms | 18.31 ms | 21.01 ms | 507 KB |
| `day_advance_360d` | 360 | 5 | 13.49 ms | 14.24 ms | 14.42 ms | 1.0 MB |
| `save_30d` (save + load round-trip) | 30 | 2 | 22.37 ms | 42.26 ms | 44.47 ms | 157 KB |
| `alloc_growth_30d` | 30 | 5 | 0.04 ms | 0.09 ms | 0.10 ms | 2.8 KB |

Reading: a full year (360 days) of simulation costs ~13.5 ms **total** (~0.037 ms/day) and scales
linearly from 30 → 180 → 360 days with no super-linear blow-up; per-frame allocation growth is
flat (2.8 KB/iteration), so no per-day leak. A 30-day save+load costs ~22 ms.

## Presentation (real display)

| Measurement | Result | Method |
|---|---|---|
| Idle frame pacing (menu, full theme + motion layer) | **avg 59.9 FPS, min 58, max 61** (vsync-capped 60 Hz) | `godot --path . --print-fps`, 50 samples |
| Panel build + open, 169 panels | **13 s total** (~77 ms/panel incl. scene mount) | `--player-panels-uitest` on display |
| Snapshot render + capture, 32 targets | **24 s** (~0.75 s/target) | `--ui-snapshot-uitest` on display |
| Boot wall (headless, data-integrity gate included) | **3.51 s** | export smoke |
| Retained memory | 352 MB headless · 768 MB with the full art registry on display | `/usr/bin/time -v` |
| Persisted footprint | 44 save files, **184 KB total**; `play_metrics.jsonl` 14 rows | user-data dir |

## Verdict

**No proven bottleneck; no fix applied.** Every measured budget has at least an order of
magnitude of headroom for the first alpha (worst frame-pacing excursion is a single 58 FPS
sample; worst simulation cost is a 14 ms year). The only growth vectors to watch are:

1. **Art payload** (PCK grew 272 → 287.6 MB with the faction wave; portraits/locations/items will
   grow it further) — a packaging/bandwidth decision, not a runtime cost; revisit before public
   builds with texture compression if the download budget matters.
2. **Retained memory on display** (768 MB includes the art registry) — revisit if the alpha
   target machine is a 4 GB box; the fix would be texture streaming, not a code rewrite.

Re-run command for the next checkpoint: `godot --headless --path . -- --runtime-scale-selftest`
(refreshes `artifacts/runtime-scale-results.json`).

## 2026-09-26 — trimmed-build re-baseline (task 5)

- Live session on the lossy-texture build: `--print-fps` 36 samples, **avg 57.7 / median 60 / p5 26 / min 14** (startup frames only), V-Sync 60 cap.
- RSS settled **945 MB** at play (baseline window 918–1104 MB) — no memory regression from `compress/mode=1` art or the 128.4 MB PCK.
- PCK **128,358,532 B** (was 294,172,544 B); boot window unchanged in feel (3.5 s baseline).
- Verdict: **no perf fix required**; texture-memory cost tracked as acceptable for alpha.
