# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# Onboarding truthfulness guard (T03) — 2026-10-01

STATUS: APPROVED BY USER
(Batch mandate: "please code and intensively repair! | T03 | Onboarding-truthfulness
guard (no `HINT: —` / empty objectives) | Regression guard for the `ae6e54387` fix class".)

## Goal

Add the missing regression guard for the `ae6e54387` fix class and repair the
one remaining truthfulness hole in the same code path.

`ae6e54387` fixed the Duty/Dose first-hour stages rendering `HINT: —` by adding
the missing `onboarding.duty.*` / `onboarding.dose.*` localization rows and the
two hint switch arms. Nothing in the repository now prevents the same silent
failure for a *future* stage: a stage can be added to `OnboardingCatalog`
without its localized title/objective/hint copy, and `BuildHintLine` still
carries a fabricated `_ => "HINT: —"` default.

Bounded outcome:
1. **Guard** — a focused source/data gate that enumerates every
   `OnboardingStage` from the Core catalog (both profiles) and fails if any
   stage lacks non-empty localized title, objective, or hint copy, or if the
   hint panel fabricates the empty sentinel.
2. **Repair** — remove the fabricated `HINT: —` default and consolidate the two
   parallel hint switches into one authoritative map so a stage can no longer
   be half-wired; share one stage-localization-id normalization between the
   panel and the status bar.

## Files

- `src/UI/OnboardingHintPanel.cs` — single `StageHintCopy` map; truthful empty
  default; `StageLocalizationId` shared helper; unbound/offline label no longer
  the `HINT: —` sentinel.
- `src/Main.Onboarding.cs` — status bar uses `OnboardingHintPanel.StageLocalizationId`.
- `assets/l10n/strings.csv` — `onboarding.hint.empty` value becomes
  `NO HINT YET` / `NOCH KEIN HINWEIS`.
- `Ashfall.Core.Tests/UI/OnboardingTruthfulnessGateTests.cs` — new 4-test guard.
- `scripts/ci/l10n_drift_gate.py` — stage-key family derived from the catalog,
  and per-stage `onboarding.hint.*` keys now covered (83 → 97 references).

## Non-goals

- No new save section, no mutable onboarding state, no parallel objective ledger.
- No change to the onboarding journey machine or stage order.
- No Unity, no full test suite, no snapshot rebaseline.

## Verification

- `scripts/run_test.sh Ashfall.Core.Tests/UI/OnboardingTruthfulnessGateTests.cs`
- `scripts/run_test.sh Ashfall.Core.Tests/UI/OnboardingWiringGateTests.cs`
- `scripts/run_test.sh Ashfall.Core.Tests/Localization/StringsCsvLocaleGateTests.cs`
- `scripts/ci/l10n_drift_gate.py`
- Host build 0 errors.

## Integration record (2026-10-01)

- `OnboardingHintPanel.BuildHintLine` now reads one authoritative
  `StageHintCopy` map (14 stages); an unknown stage returns no hint rather than
  the fabricated `HINT: —`; the parallel `contextualKey`/`fallback` switches
  are gone. `StageLocalizationId` is the single normalization, reused by
  `Main.Onboarding.cs` for the status bar.
- `strings.csv` `onboarding.hint.empty` is now `NO HINT YET` / `NOCH KEIN HINWEIS`.
- `scripts/ci/l10n_drift_gate.py` no longer hardcodes the stage-key family:
  `stage_family_keys()` enumerates `OnboardingCatalog` (from
  `OnboardingJourney.cs`) plus the panel's `StageHintCopy` literals, so the
  Python gate now checks the per-stage hint keys it previously ignored. Proven
  behavior-preserving (14 titles / 14 objectives exactly match the old
  hardcoded set) and drift-detecting (a fake catalog stage surfaces
  `onboarding.newthing.title`/`.objective` as missing).
- New `OnboardingTruthfulnessGateTests` 4/4: every stage has localized
  title/objective/hint; every stage has a non-empty, non-sentinel hint mapping;
  the panel never fabricates `HINT: —`; the status bar shares the panel id.
