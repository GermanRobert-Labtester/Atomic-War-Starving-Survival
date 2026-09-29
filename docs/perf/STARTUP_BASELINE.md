# Main Scene Startup Baseline

**Captured:** 2026-09-27
**Runtime:** Godot 4.7.1 Mono, Linux x86_64, Compatibility renderer on Mesa llvmpipe
**Path:** process launch through the first Main scene `_Process` callback, which follows Main setup and catalog validation
**Raw evidence:** `artifacts/performance/three-plan-2026-09-27/ashfall-startup-correct-before.json`

| Measure | Result |
|---|---:|
| Process start to first Main process callback | 10,111.7 ms |
| Engine start to first Main process callback | 9,909 ms |
| Narrative catalog file-read samples during setup | 16 |
| Total sampled narrative `File.ReadAllText` time | 2.266 ms |
| Largest individual read | 0.664 ms (`rag_pulp_beater_records.json`) |
| Runtime assembly bytes from reflection | unavailable (`Assembly.Location` is empty for this Godot load mode) |

The OS page cache was not cleared, and this is a fresh process rather than a controlled cold-disk measurement. These reads measure file access only, not parsing or downstream registration. Their combined observed cost is too small to justify deferred catalog loading. The 10.1-second startup is a measurement to investigate further with Godot's startup profiler; it is not attributed to narrative file reads.

The matching after run measured 11,631.8 ms from process start and 11,588 ms from engine start to the first Main process callback, with 16 reads totalling 2.069 ms. The OS page cache and VM load differed, so the 1.52-second startup increase is not attributed to the UI callback changes. The full after artifact is `artifacts/performance/three-plan-2026-09-27/ashfall-startup-after.json`.

The raw startup artifacts contain per-catalog read times and allocated bytes. The 1.52-second difference between the two captures is not attributed to these edits: the OS page cache and VM load differed, and neither run cleared the disk cache.

## Exported assembly measurement

The Linux/X11 `ExportRelease` assembly with host selftests measured **16,151,552 bytes** before the compile exclusion. The exported `ExportRelease/linux-x64` assembly measures **15,301,120 bytes** after exclusion, a reduction of **850,432 bytes (5.3%)**. The current output is `builds/linux/data_Ashfall_linuxbsd_x86_64/Ashfall.dll`; the with-selftests artifact was captured during the same integration before export. ExportRelease excludes host selftest source files and symbols while Debug retains `ASHFALL_SELFTEST`.

The finalized Linux package measured **222,148,800-byte PCK** and **73,627,864-byte executable**. The previous PCK artifact was from an older project/data snapshot, so its 156 MB size is not a valid before comparison. Export presets now omit repository-only `docs/`, `scripts/`, build output, agent metadata, and prior export output. The package parity gate verified all 1,423 catalogs byte-for-byte and parseable.

Verification: export symbol gate passed; exported binary completed the 60-frame headless boot; `--data-integrity-selftest` passed 427/427 catalogs (five documented primary-wins warnings); `--research-catalog-selftest` passed; `--bridge-selftest` passed; export parity passed. The exported boot logs nonfatal editor-only Godot MCP plugin parse errors and missing localization/art warnings, then initializes the campaign scene and exits cleanly. Those warnings are retained here as environment/package observations rather than startup-performance findings.
