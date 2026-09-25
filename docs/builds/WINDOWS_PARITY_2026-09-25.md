# ASHFALL — Windows Runtime Parity under Wine (2026-09-25)

The Windows export is smoke-tested on this Linux host via wine so platform drift is caught before
a real-hardware check. Build under test: `builds/windows/` (exported 2026-09-25 ~08:00, three
artifacts: `ashfall.exe`, `ashfall.pck`, `data_Ashfall_windows_x86_64/`).

## Results

| Probe | Command (inside `builds/windows/`) | Result |
|---|---|---|
| Data authority | `wine ashfall.exe --headless -- --data-integrity-selftest` | **PASS — 425 catalogs, 0 errors** |
| Player panels (clickability / focusability / truthfulness, 169 panels) | `wine ashfall.exe --headless -- --player-panels-uitest` | **PASS** |
| Layout selftest (motion contracts, spacing, style coverage) | `wine ashfall.exe --headless -- --ui-layout-selftest` | **PASS** |
| Accessibility selftest (contrast, scale, focus, audio-visual gating) | `wine ashfall.exe --headless -- --ui-accessibility-selftest` | **PASS** |

Run with `WINEDEBUG=-all` to keep the log readable; wine initialises a prefix on first run.

## Residual (honest)

* Wine is not Windows: rendering, input APIs and audio devices are emulated. The parity suite
  proves **logic/data/UI construction** runs on the Windows build; a real-hardware (or VM) pass
  is still the acceptance step for windowing, input, and audio.
* The parity suite runs headless; a windowed run on real hardware should repeat
  `--ui-accessibility-selftest` and one snapshot pass.
* Re-run after any export:
  `cd builds/windows && WINEDEBUG=-all wine ashfall.exe --headless -- --player-panels-uitest`
  (plus the other three flags above).

## Windowed parity (added 2026-09-25)

The Windows build also runs **windowed** under wine with the snapshot harness:

```
cd builds/windows && DISPLAY=:0 WINEDEBUG=-all wine ashfall.exe -- --ui-snapshot-uitest
UI_SNAPSHOT_UITEST SUMMARY: 32 targets — 27 match, 0 new, 5 drift, 0 fail
```

The five drifts (`trade_default`, `caravan_barter_default`, `market_default`,
`muster_atlas_default`, `standing_record_atlas_default`) reproduce on a freshly exported
Windows build, so they are **wine font/text rasterization differences** on text-heavy panels,
not a stale-build artifact and not a product defect. Interpretation: windowing, rendering and
panel construction work on Windows; a real-hardware pass should confirm text rendering and input.
