# Verdict Radio Kind Contract

> **Runtime Implementation:** `VerdictCatalogLoader.VerdictRadioEntry.kind` (string field)
> **Presentation Consumer:** `src/VerdictPanel.cs` (`kind switch`)

---

## 1. Kind Vocabulary Taxonomy

The live system accepts string-based `kind` tags. The following taxonomy governs all 30 broadcasts:

| Kind | Role & Semantic Function | Existing Baseline Count | Plan 94 Additions | Final Total Count |
|---|---|---|---|---|
| `telemetry` | Quantitative instrumentation reports (pressure, river stage, acoustic density) | 3 | 5 | 8 |
| `maintenance` | Automated facility upkeep, desiccant purges, breaker tests, battery cycles | 2 | 4 | 6 |
| `census` | Registry tallies, capacity margins, unverified addition warnings | 0 | 3 | 3 |
| `calibration` | Self-reported instrument drift, baseline offsets, curve validations | 0 | 2 | 2 |
| `anomaly` | Unscheduled carrier bursts, telemetry phase inversions | 0 | 2 | 2 |
| `emergency` | Automated schedule suspension and master carrier lock | 0 | 1 | 1 |
| `call` | Formal Office of Censuses summoning and custody directives | 4 | 0 | 4 |
| `witness` | Leaked civilian/amateur transmission bleed | 1 | 0 | 1 |
| `readings` | Discrete pulse/interval detections | 1 | 0 | 1 |
| `count` | Physical reel/ledger tallies | 1 | 0 | 1 |
| `carrier` | Pilot tones and unmodulated carrier waves | 1 | 0 | 1 |
| **Total** | | **13** | **17** | **30** |

---

## 2. Invariants
- All 17 new broadcasts map strictly into the 6 requested categories (`telemetry`: 5, `maintenance`: 4, `census`: 3, `calibration`: 2, `anomaly`: 2, `emergency`: 1).
- No new enum or C# class is introduced in Core; `kind` strings are read dynamically and cleanly rendered.
