# Localization Coverage Record — Enhancement Task 6 (2026-09-27)

> **STATUS: COMPLETE — commit `0f8dccdd0`.**

## Outcome

`assets/l10n/strings.csv` now has German for all 359 rows (was 176/359 by
naive count). English stays 359/359. Zero duplicate keys, zero empty cells.

## What changed

- **184 German fills** across `ui.common` (15), `settings` (27), `tutorial`
  (15), `warning` (6), `codex` (5), `discovery` (116). All 183 empty keys had
  CSV English identical to Core's `LocalizationService` registrations, so the
  fills translate verified text. `tutorial.*` reuses the existing German of the
  parallel `onboarding.*` family. `HP` stays `HP`, matching the HUD labels.
- **Five malformed rows repaired.** Unquoted embedded commas made them parse
  as five fields, and `LocalizationService.LoadFromCsv` consumed them wrongly:
  - `codex.manual.radiation`, `onboarding.inventory_use.objective`: English
    truncated at the comma; the English tail was registered as German.
  - `onboarding.tooltip.hint`, `onboarding.inspect.objective`,
    `onboarding.weather.objective`: German truncated at the comma.
- **Eleven rows lost redundant quotes only** (the minimal-quote writer). Their
  parsed content is identical.

## Gate

New `Ashfall.Core.Tests/Localization/StringsCsvLocaleGateTests.cs` covers the
whole catalog: header `key,en,de,source`, four fields per row, unique keys,
non-empty `en` and `de`, and `{placeholder}` identifier parity. Its parser
mirrors `LocalizationService.ParseCsvLine`, so a row the runtime would misread
fails the gate. The field-count check alone would have caught all five
malformed rows.

## Verification

| Command | Result |
|---|---|
| `bash scripts/run_test.sh Ashfall.Core.Tests/Localization/StringsCsvLocaleGateTests.cs` | 2/2 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Localization/LocalizationPilotTests.cs` | 4/4 PASS |
| `python3 scripts/ci/l10n_drift_gate.py` | PASS (359 keys, 69 pilot refs) |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/LocalizationRatchetTests.cs` | 1 FAIL: literals 608 vs 603 baseline |

The ratchet failure comes from the concurrent UI lane's uncommitted panel
edits. This task changed no UI source. Leave the baseline alone until that lane
lands.

## Findings not fixed here

- The runtime-fallback premise was disproven: empty German cells already fall
  back to English and never show raw keys. The real gap was coverage.
- Core `LocalizationService` duplicates 19 German micro-location strings in
  code. Two copies can drift apart; the CSV should be the single source.
- The Godot CSV importer treats the `source` column as a locale and produces an
  inert `strings.source.translation`. Renaming the column needs a coordinated
  loader and re-import change.
- `artifacts/l10n-inventory.json` is stale against concurrent state.
- Orphan-key gating was skipped: keys are built dynamically
  (`wildlife.{category}.{id}.{field}`, `{tutorialId}.title`), so a static
  reference list is unreliable.
