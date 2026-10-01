# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# L10N Dynamic-Family Coverage, Literal Extraction, German First-Hour, POT Regeneration (L01–L04)

STATUS: APPROVED BY USER (2026-10-02) · INTEGRATED 2026-10-02

> **Integration outcome:** L01 (declarative `DYNAMIC_FAMILIES` drift gate + 32 missing
> `achievement.*` rows + `L10nDynamicFamilyGateTests`) and L03 (4 first-hour German quality
> fixes) were delivered under this plan. L02 (`extract_l10n_inventory.py` Make* inventory +
> regenerated `artifacts/l10n-inventory.json`) and L04 (`generate_pot_template.py` +
> `template.pot` + `docs/l10n/TRANSLATOR_HANDOFF.md` + the `pot_template_drift` gate) were
> delivered concurrently by another builder and verified here rather than duplicated or
> overwritten. All green: l10n gate PASS (1183 keys, 212 dynamic-family keys), POT `--check`
> PASS, 6 pinned l10n suites green, host build 0 errors, `git diff --check` clean.

User directive (2026-10-02): "Please start working, coding on these tasks aswell as repair
any leaks, bugs, missing tool calls or recieving tool calls, missing panels, missing UI
animations, warnings, errors! make sure that after coding in these tasks you run a sweep
loop of find issue fix issue repeat for 3 loops!"

| id | task | premise note |
|---|---|---|
| L01 | Extend l10n drift gate to all dynamic key families | `ae6e54387` completed the stage-key family by hand |
| L02 | Extract remaining hardcoded UI literals | Find `src/UI` literals bypassing the l10n authority |
| L03 | German completeness/quality pass on first-hour strings | only en+de exist; first-hour is priority |
| L04 | Regenerate `template.pot` + translator handoff doc | keep the extraction artifact in sync |

## Verified premise (Rule 7 — evidence before change)

`scripts/ci/l10n_drift_gate.py` derives **one** dynamic key family (onboarding stage
title/objective from `OnboardingJourney.cs`) plus literal `onboarding.hint.*`. Every other
dynamically-constructed key is invisible to the gate. Enumerated from `src/`:

| dynamic family | built in | authoritative source | rows today | gate covers |
|---|---|---|---|---|
| `onboarding.{stage}.title/objective` | `Main.Onboarding.cs:303-304` | `OnboardingJourney.cs` | 77 | ✅ |
| `onboarding.hint.*` | `OnboardingHintPanel` (literals) | literal strings | — | ✅ |
| `discovery.{id}.title/description/choice.{cid}` | `ExpeditionPanel.cs:1207,1242,1318` | `micro_locations.json` (28 ids) | 28/28/79 | ❌ |
| `achievement.{id}.name/description` | `AchievementsPanel.cs:124,128` | `achievements.json` (16 ids) | **0** | ❌ |

Non-families verified and excluded: `stage.{n}.hint` (`MakeHintKey`) and
`wildlife.bycatch.{species}` are **journal knowledge keys**, not localization keys.

`assets/l10n/template.pot` is a stale 10-entry hand-written file (POT-Creation-Date
2026-08-31) while `strings.csv` holds 995 keys — the extraction artifact is far out of sync.
`strings.csv`: 0 empty German; 20 `de==en` (all legitimate: `OK`/`AUDIO`/`HUD` tokens and
format-only strings); 92 first-hour keys (`onboarding.*` + `tutorial.*`).

## Bounded outcome

- **L01** — Refactor the gate to a declarative `DYNAMIC_FAMILIES` registry and register all
  four families above. Add the 32 missing `achievement.{id}.name/description` rows (English
  from `achievements.json`, German translated). A new stage/achievement/micro-location with
  no rows now fails the gate.
- **L02** — Extend `extract_l10n_inventory.py` to also capture `Make{Label,Button,Body,
  Small,Mono,SectionHeader}(` literal first arguments (the 1047-site class it currently
  misses), regenerate `artifacts/l10n-inventory.json`, and report the top panels.
- **L03** — German completeness/quality audit of the 92 first-hour keys; fix any real
  quality issue found (completeness is already 100%).
- **L04** — New `scripts/ci/generate_pot_template.py` that regenerates `template.pot` from
  `strings.csv` deterministically (msgid=key, msgstr=English, `#: source` comment, header
  counts) with a `--check` mode; new `docs/l10n/TRANSLATOR_HANDOFF.md`; register the
  `pot_template_drift` fast gate in `docs/ci/CI_GATE_MANIFEST.json`.

## Non-goals

- No mass localization of the ~556 literal `Text=`/`TooltipText=` and ~1047 literal `Make*()`
  call sites across `src/UI` — L02 delivers the accurate inventory, not the panel waves.
- No touch of other sessions' `strings.csv` rows (expedition/ward/railway tooltip).
- No change to `StatusPanelThresholdTests.cs` (its gate-content assertions stay valid).
- No tier change for `compiler_warning_baseline` (separate authority question, prior batch).

## Exact files

Edit: `scripts/ci/l10n_drift_gate.py`, `scripts/ci/extract_l10n_inventory.py`,
`assets/l10n/strings.csv` (achievement + first-hour rows only), `assets/l10n/template.pot`,
`docs/ci/CI_GATE_MANIFEST.json`, `artifacts/l10n-inventory.json`, `.ai/state.md`,
`WORKTREE_OWNERSHIP.md`.
New: `scripts/ci/generate_pot_template.py`, `docs/l10n/TRANSLATOR_HANDOFF.md`,
`Ashfall.Core.Tests/Tooling/L10nDynamicFamilyGateTests.cs`, this plan.

## Verification

`python3 scripts/ci/l10n_drift_gate.py`; `python3 scripts/ci/generate_pot_template.py --check`;
focused xUnit `L10nDynamicFamilyGateTests`, `StringsCsvLocaleGateTests`,
`LocalizationRatchetTests`; `run-gates.py --check-only`; host build 0/0. No full suite; no commit.