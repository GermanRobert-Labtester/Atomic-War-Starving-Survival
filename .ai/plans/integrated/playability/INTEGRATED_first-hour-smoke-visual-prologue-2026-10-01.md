# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# First-hour smoke + visual-binding gate + diegetic prologue (2026-10-01)

STATUS: APPROVED BY USER
(User directive: "tackle those 3 suggested next and after suggest 5 much bigger
tasks!".)

## Bounded outcome

Three playability-readiness increments that make the game easier to *see* and
*smoke test* by a human, each bounded and verified:

1. **First 30 minutes smoke harness** — an end-to-end first-hour journey test
   plus the human checklist the automated gates mirror.
2. **Visual-binding completeness gate** — the catalog coverage report now
   classifies and *gates* placeholder bindings instead of hiding them behind
   "resolved".
3. **Diegetic prologue** — authored opening beats surfaced through the existing
   Opening Protocol day-goal seam.

## 1. First 30 minutes smoke harness

- New `Ashfall.Core.Tests/Onboarding/FirstHourPlaythroughSmokeTests.cs`
  (4/4): a fresh `OnboardingJourney.CreateFirstHour()` completes when all seven
  real signals are recorded; a **mid-journey save/restore resumes without
  re-demanding finished stages**; every stage names a non-empty "show me where"
  route spanning ≥5 distinct surfaces; `FirstHourOrder` and `FirstHour` defs are
  consistent.
- New `docs/qa/FIRST_HOUR_SMOKE_TEST.md` — the human checklist: the seven
  onboarding beats mapped to the real UI action, expected visible evidence, the
  sigil each one emits, the save/load durability step, and screenshots to attach.
- Read-only: composes the existing journey/producers; no new runtime path.

## 2. Visual-binding completeness gate

- `AssetCoverageScanner.RunFullCoverageSweep` now classifies each resolved id as
  **real** vs **placeholder** (`IsPlaceholderPath`) and returns
  `(TotalIds, Missing, Placeholder)`.
- `AssetCoverageReport.PrintFullCoverageSweep` prints per-category placeholder
  counts and a gating summary.
- `HostCli.RunAssetCoverageReport` is now a **gate**: a missing or placeholder
  binding in any catalog category fails the run ("kill the placeholder
  squares"). Current authored state: **1661/1661 resolved, 0 missing,
  0 placeholder**.
- The duplicated `AssetRegistrySelfTest.RunFullCoverage` body was replaced with a
  thin delegate to the scanner so the two implementations cannot drift.

## 3. Diegetic prologue

- New data `Assets/StreamingAssets/Data/prologue_sequence.json` — 3 authored
  opening beats (First light / The hatch / The first protocol), restrained and
  fictional.
- New Core authority `Assets/Ashfall.Core/Narrative/PrologueSequence.cs` —
  engine-free, no save section; validates and orders beats, rejects malformed or
  duplicate-day catalogs rather than inventing copy, and resolves by campaign
  day (`TryGetBeatForDay`).
- New host partial `src/Main.Prologue.cs` loads the catalog once and exposes
  `TryGetPrologueGoal`. `Main.RefreshOpeningProtocolDayGoal` now prefers the
  slice beat, then the prologue beat, then clears the goal — no new modal.

## Verification

- `FirstHourPlaythroughSmokeTests` 4/4, `PrologueSequenceTests` 4/4;
  `bin/run-scoped-tests` 5/5 mapped targets PASSED.
- `--asset-coverage-report` gate PASS (0 missing / 0 placeholder);
  `--asset-registry-selftest` PASS (55/55); headless boot 0 script errors;
  host build 0 errors.

## Limitations

- The smoke harness is headless; the human checklist covers the visible path.
- The visual gate classifies placeholders by resolved path; a placeholder renamed
  away from "placeholder" would evade it (documented, acceptable).
- The prologue is text-only opening beats; it is not a cinematic.

## Files

`Ashfall.Core.Tests/Onboarding/FirstHourPlaythroughSmokeTests.cs`,
`Ashfall.Core.Tests/Narrative/PrologueSequenceTests.cs`,
`docs/qa/FIRST_HOUR_SMOKE_TEST.md`, `src/Host/AssetCoverageScanner.cs`,
`src/Host/AssetCoverageReport.cs`, `src/Host/HostCli.Command.RunAssetCoverageReport.cs`,
`src/Host/AssetRegistry.cs`, `Assets/Ashfall.Core/Narrative/PrologueSequence.cs`,
`Assets/StreamingAssets/Data/prologue_sequence.json`, `src/Main.Prologue.cs`,
`src/Main.SliceScenario.cs`, this plan, `.ai/state.md`,
`INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`.
