# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
# StatusPanel / SurvivorDetailPanel band hardening + 5x find-repair-harden loop

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
>
> Both open carried-over tasks (7, 9) closed; 13 of 15 re-verified as already
> integrated. New shared band authority `src/UI/AshfallUiBands.cs`. Five
> find -> repair -> harden loops completed, each finding recorded in section 5a
> and `.ai/state.md`. Host + test builds 0/0; focused tests 136/136 across four
> suites; headless runtime `ui_layout_selftest PASS (failures 0)`. No commit; no
> full suite; no save section; no new data file; no new gameplay authority.

STATUS: APPROVED BY USER

## 1. Bounded outcome

Two of the fifteen carried-over small tasks were still open against current
source evidence; the other thirteen were already integrated and re-verified.
This package closes the open two, extracts the band logic they share so the
next panel cannot drift, and then runs five find -> repair -> harden loops over
the touched seam.

### Carried-over task status (re-verified 2026-10-02 against source + data)

| # | Task | State | Evidence |
|---|------|-------|----------|
| 1 | Localize StatusPanel cohort rows | INTEGRATED | `src/UI/StatusPanel.cs:256-259` via `Tr("ui.status.cohort.*")` |
| 2 | Localize SurvivorsPanel ROSTER / TELEMETRY headers | INTEGRATED | `src/UI/SurvivorsPanel.cs:306,330` |
| 3 | Test: four filter labels resolve from strings.csv | INTEGRATED | `StatusPanelThresholdTests.cs:515,1150`; rows at `assets/l10n/strings.csv:507-510` |
| 4 | Extract Tr/TrFormat into shared AshfallUiText | INTEGRATED | `src/UI/AshfallUiText.cs`; forwarders in 4 panels |
| 5 | HoldfastRuntimeSession five-fallback-needs drift test | INTEGRATED | `StatusPanelThresholdTests.cs:101-112` |
| 6 | Gate GameHudSnapshotFixture vs UpdateX signatures | INTEGRATED | `StatusPanelThresholdTests.cs:114-130` |
| 7 | WarnThreshold band on SurvivorDetailPanel radiation row | **OPEN** | `SurvivorDetailPanel.cs:367` is still a 2-band test |
| 8 | StatusPanel morale drift row | INTEGRATED | `StatusPanel.cs:303-306` |
| 9 | Colour StatusPanel avgRad/avgMor cards from shared bands | **OPEN** | `StatusPanel.cs:256-259` pass no colour |
| 10 | a11y `ui.hud.needs.warmth` tooltip assertion | INTEGRATED | `AccessibilitySourceAuditTests.cs:208-210` |
| 11 | `radiation_high` warn-band toast from host | INTEGRATED | `SurvivorsHostSession.cs:210-230` |
| 12 | NeedsDayDeltaFormat NaN/Infinity -> 0 | INTEGRATED | `NeedsDayDeltaFormat.cs:20-23` |
| 13 | Gate UpdateHud survivors local guard | INTEGRATED | `StatusPanelThresholdTests.cs:216-222`; guard `Main.GameFlow.cs:805-806` |
| 14 | `game_hud_default` golden-presence assertion | INTEGRATED | `SnapshotCorpusPinTests.cs:56-66` |
| 15 | Localize Main.GameFlow low-water / low-food | INTEGRATED | `Main.GameFlow.cs:850,854` via `ui.resource.food` / `ui.resource.water` |

## 2. Non-goals

- No new gameplay authority, save section, ledger, or simulation.
- No re-derivation of a band constant; thresholds stay owned by `NeedsProfile`
  and `RadiationSystem`.
- No change to the Core/Godot split, and no new data file.
- No full test suite.

## 3. Files owned by this package

- `src/UI/AshfallUiBands.cs` (new) — shared presentation-only band -> token map.
- `src/UI/StatusPanel.cs` — task 9.
- `src/UI/SurvivorDetailPanel.cs` — task 7.
- `src/UI/GameHudOverlay.cs` — route the existing inline chip band logic through
  the shared helper (no behaviour change).
- `Ashfall.Core.Tests/UI/StatusPanelThresholdTests.cs` — gate tests.

## 4. Design

`AshfallUiBands` is the one place a measured value becomes a UI colour token.
It reads thresholds from the owning authorities and never re-types a number.

- `ForNeed(value, highIsBad, warnAt, criticalAt)` — the three-band rule already
  used by `GameHudOverlay.ApplyNeedChip` (Critical / Warm / Pale), plus a
  truthfulness guard: a NaN or infinite reading reads as critical, never nominal.
- `ForLow(value, warnAt, criticalAt)` — low-is-bad (morale, warmth).
- `ForDose(mSv)` — high-is-bad against `RadiationSystem.WarnThreshold` and
  `RadiationSystem.AcuteThreshold`, so the panel and the toast agree on the
  band the host just crossed.

## 5. Verification and termination criteria

- [x] Plan written and approved before any production edit.
- [x] `dotnet build Ashfall.csproj` — 0 errors / 0 warnings.
- [x] `dotnet build Ashfall.Core.Tests` — 0 errors / 0 warnings.
- [x] Focused tests on the changed areas only (never the full suite):
      `StatusPanelThresholdTests` 116/116, `AccessibilitySourceAuditTests` 7/7,
      `StatusGlanceAndForecastGateTests` 7/7, `UiScrimContrastGateTests` 6/6.
- [x] Runtime (not just compile-green):
      `run-godot-bounded.sh --path . --headless -- --ui-layout-selftest`
      -> `ui_layout_selftest PASS, failures 0`, artifact written.
- [x] Five find -> repair -> harden loops recorded in `.ai/state.md`.
- [x] 15 follow-on small tasks recorded below.

## 5a. Loop log (summary; full detail in `.ai/state.md`)

| Loop | Finding | Action |
|------|---------|--------|
| 1 | I had just passed `healthCritical` into the `warnAt` slot, killing the warn band (any health <= 30 rendered Critical though the profile sets critical at 25) | Repaired; named arguments at every call site |
| 1 | `ForNeed(..., highIsBad:false, ...)` was a second independent copy of the `ForLow` rule | It now delegates; one implementation each |
| 2 | `GameHudOverlay.UpdateRadiation` re-derived 100/50 while the detail row and the host toast used 80/50 — at 85 mSv the HUD said "amber", the panel said "critical" | Routed through `AshfallUiBands.ForDose` |
| 3 | That fix flattened the nominal dose token to Pale, but the `game_hud_default` fixture projects 41 mSv (nominal) — it would have silently broken a shipped golden | Named the token `Calm` (Lethe) instead of re-baselining a golden to fit my own edit; gated the distinction |
| 4 | `SurvivalDetailPanel.Avg Dose` is the cohort twin of the task-7 row and was still 2-band — fixing one side made the split worse | Repaired to `AshfallUiBands.ForDose(avgDose)` |
| 4 | Rest of `SurvivalDetailPanel` bands against magic numbers (`< 50` health, `>= 80` hunger/thirst/fatigue) that contradict `NeedsProfile` | **Reported, not repaired** — 12-row rewrite outside scope; pinned as known debt + queued as follow-up #1 |
| 5 | `run-godot-bounded.sh` without a `--` separator swallows the selftest flag, boots normally, writes no artifact, and still exits 0 — a silent false green | Pinned the split contract in a test; did not edit the shared CI script (not in claim). Correct form documented in the test |

## 6. Fifteen follow-on small tasks (immediately integrable, recorded here)

Ordered by evidence: the first five come directly from the loop findings above.

1. Replace `SurvivalDetailPanel`'s magic bands with the owning profile
   (`avgHealth < 50` -> `ForNeed` on `healthWarn`/`healthCritical`; the
   `>= 80` hunger/thirst/fatigue rows -> `profile.*Critical`). Closes the
   "teaches numbers the simulation does not enforce" defect.
2. Localize `SurvivalDetailPanel` (every row is hardcoded English — it is the
   only cohort panel with no `Tr` calls at all).
3. Route `StatusPanel.RenderThermal`'s inline warmth band through
   `AshfallUiBands.ForLow(minWarmth, profile.warmthWarn, profile.warmthCritical)`.
4. Route the `GameHudOverlay` radiation *meter fill* through the same band as
   its label, so bar and text cannot disagree.
5. Add a gate test pinning `DoseLedgerPanel.MapBand` against
   `AshfallUiBands.ForDose` so the two dose scales stay explicitly distinct.
6. Colour the StatusPanel `Water Stores` / `Food Stores` rows from thresholds
   declared as named consts in the panel (currently unbanded).
7. Give the `Survivor Cohort` card a band when `alive < roster.Count`, matching
   what `RenderDayInfo` already does for the same fact.
8. Localize the `[UNNAMED]` expedition-injury placeholder in `StatusPanel`.
9. Localize the `Day +N` forecast row prefix in `RenderForecast`.
10. Localize the ` — SHORT` forecast suffix separator.
11. Add `ui.survivor.trait.*` band tooltips to the detail-panel radiation rows.
12. Add a test pinning the `NeedsDayDeltaFormat.Signed` clamp bounds.
13. Add a source test pinning the `Signed` call in `StatusPanel.AddDriftRow`.
14. Add `AshfallUiBands` to the UI a11y source-audit token list.
15. Add a test asserting `ui.resource.food` / `ui.resource.water` are the exact
    keys `Main.GameFlow.cs:850,854` pass to the low-food / low-water toasts.

## 7. Shared paths intentionally untouched

`scripts/ci/run-godot-bounded.sh` (shared CI harness, outside this claim — the
false-green arg split is pinned by test instead of edited),
`DoseLedgerPanel.MapBand` (a legitimately different dose authority:
`DoseLedgerSystem.RedMsv/AmberMsv/BlackMsv`, cumulative ledger scale), and the 21
`Theme.Dim` secondary-text rows in `SurvivorDetailPanel` (panel-wide convention,
not a band). All pre-existing dirty worktree changes preserved; no foreign code
reverted.
