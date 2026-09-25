# ASHFALL — Alpha Save/Load Soak (2026-09-25)

Companion to `docs/health/PERF_BUDGET_2026-09-25.md`; both were produced by the same probes.

## What was run

| Probe | Scope | Result |
|---|---|---|
| `--runtime-scale-selftest` | 30/180/360-day day-advance workloads, 30-day save+load, 30-day allocation-growth | **PASS 6/6**; save+load 22 ms; alloc growth 2.8 KB/iter (no leak trend) |
| `--session-durability-selftest` | slot capacity/isolation, interrupted-write + backup recovery, soak verdicts, capture round-trip | **21/21 PASS** |
| `--7-day-smoke-selftest` | map discovery, weather rolls, needs drift, mid-run save/reload round-trip across 10 gates | **PASS** |
| `--save-load-ui-failure-selftest` | missing / corrupt / checksum-invalid saves → recoverable messages, live session intact | **PASS** |
| `--playable-metrics-selftest` | metrics recorder + funnel + save-section round-trip (with the new JSONL sink subscribed) | **16/16 PASS** |

## Long-run behaviour (evidence)

* **Save footprint stays small**: 44 store files totalling **184 KB** after all probes, largest
  single store in the low tens of KB. The soak's save/load churn does not grow the footprint.
* **Metrics sink grows linearly and is bounded by events, not days**: `play_metrics.jsonl` holds
  one row per recorded event; the recorder's in-memory buffer is bounded at 2,000 events
  (Core) and the sink **never drains** it, so the `playable_metrics` save section and the
  end-of-campaign report keep their rows. A long session grows the JSONL file, which is expected
  for a local audit log; the save section stays governed by the recorder's own bounded state.
* **Interrupted-write recovery** is covered by `--session-durability-selftest` (backup recovery
  audit) and by the UI failure-path probe for missing/corrupt/checksum-invalid saves.

## Verdict

The alpha save/load path is **soak-clean** at the measured scale (year-long simulation, 30-day
save/load cycles, repeated failure-path injections). No schema, ownership, or determinism change
was needed. Residual watch item: a real human multi-hour session should be captured once with
`scripts/tools/alpha_playtest_report.sh` so the sink and save sizes are observed together with
actual play (recorded as an alpha acceptance step in `docs/alpha/FIRST_HOUR_PLAYTEST_KIT.md`).

Re-run commands:

```bash
godot --headless --path . -- --runtime-scale-selftest
godot --headless --path . -- --session-durability-selftest
godot --headless --path . -- --7-day-smoke-selftest
godot --headless --path . -- --save-load-ui-failure-selftest
```
