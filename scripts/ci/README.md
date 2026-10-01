# ASHFALL CI scripts

Bounded, deterministic verification entry points. Prefer these over invoking
`godot` or `dotnet test` directly so the test policy (15 FPS, 180s cap, scoped
selection) is enforced.

## UI layout check

`bash scripts/ci/ui-layout-check.sh`

Runs the Godot headless `--ui-layout-selftest` through
`scripts/ci/run-godot-bounded.sh`. The launcher enforces a hard **180-second**
cap at 15 FPS and blocks when the compiled Godot assembly is older than the
newest C# source. Run `run-godot-bounded.sh --check-staleness-only` to gate on
that freshness without booting Godot (CI-friendly). The wrapper additionally parses the selftest's `Failures:`
summary and exits non-zero on any failure, even if the runner itself exits 0.

Use it after any change to a panel's layout or section structure.

The selftest writes `artifacts/ui-layout-selftest.json`
(`{"test":"ui_layout_selftest","status":"PASS","failures":0}`). The wrapper
verifies the file exists, reports `PASS`, and was written by the current run
(its mtime is not older than the run start), so a stale or absent verdict
cannot be scraped as green. It also exports `ASHFALL_EXPECT_ARTIFACT` for the
bounded runner, which independently fails when the promised file is missing.

## Localization drift check

`python3 scripts/ci/l10n_drift_gate.py`

Verifies that every `Tr(...)`/`TrFormat(...)` key referenced by the pilot and
localized wave surfaces (ResearchPanel, OnboardingHintPanel, `Main.Onboarding`,
GameHudOverlay, StatusPanel, DutyRosterPanel, SilentFoundryPanel, …) resolves to
a row in `assets/l10n/strings.csv` **and carries a non-empty German translation**.
It also fails on duplicate keys and on `{placeholder}` parity drift between the
English and German columns. Run it after any UI string change so a new key
without German parity fails loudly instead of shipping untranslated. The gate
prints the exact catalog size (currently several hundred keys) on every pass;
the count is not pinned in this README so it cannot rot. The set of checked
surfaces (`LOCALIZED_SURFACES`) is the registry of panels that resolve through
`AshfallUiText`; a panel that starts localizing must be added there, and
`LocalizedSurfaces_RegisteredInDriftGate` fails when one is missing.

## Other gates

- `python3 scripts/ci/l10n_drift_gate.py` — localization key parity (en/de).
- `scripts/ci/triad-drift-gate.sh` — Triad drift source gate.
- `scripts/ci/asset-orphan-sweep.sh` — orphan asset detection.
