# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
# Cohort truth + localization wave — 15 recorded follow-on tasks

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
>
> All fifteen recorded follow-on tasks closed. `SurvivalDetailPanel` no longer
> teaches thresholds the simulation does not enforce and no longer ships
> hardcoded English; the l10n drift gate now enforces that panel. Three
> find -> repair -> harden loops completed (see 5a), each finding recorded in
> `.ai/state.md`. Test project 0/0; l10n gate PASS (716 keys); focused suites
> 189/189 across four classes. One **foreign-lane blocker** is reported in 5:
> `src/Main.UiTests.PlayerPanels.cs:115` breaks the shared host build and is
> not this package's to repair. No commit; no full suite; no save section; no
> new data file; no new gameplay authority.

STATUS: APPROVED BY USER

## 1. Bounded outcome

Close all 15 follow-on tasks recorded in
`.ai/plans/integrated/ui/INTEGRATED_status-survivor-band-hardening-2026-10-02.md`
section 6, then run three find -> repair -> harden loops over the newly touched
seam.

Headline: **`SurvivalDetailPanel` stops teaching thresholds the simulation does
not enforce, and stops shipping hardcoded English.** It was the last cohort
panel with zero `Tr` calls and the only one still banding against literal
numbers instead of the owning `NeedsProfile`.

## 2. Non-goals

- No new gameplay authority, save section, ledger, or simulation change.
- No re-derivation of a threshold; every band reads the owning authority.
- No change to `DoseLedgerPanel.MapBand` (a legitimately different, cumulative
  dose scale owned by `DoseLedgerSystem`) — only a test pins the separation.
- No full test suite. No commit.

## 3. Files owned

- `assets/l10n/strings.csv` — new `ui.survival_detail.*` rows + forecast keys.
- `src/UI/SurvivalDetailPanel.cs` — tasks 1, 2.
- `src/UI/StatusPanel.cs` — tasks 3, 6, 7, 8, 9, 10.
- `src/UI/GameHudOverlay.cs` — task 4.
- `src/UI/SurvivorDetailPanel.cs` — task 11.
- `scripts/ci/l10n_drift_gate.py` — add `SurvivalDetailPanel` to
  `LOCALIZED_SURFACES` so task 2 is enforced, not merely done once.
- `Ashfall.Core.Tests/UI/StatusPanelThresholdTests.cs` — tasks 5, 12, 13, 15.
- `Ashfall.Core.Tests/UI/AccessibilitySourceAuditTests.cs` — task 14.

## 4. The fifteen tasks

| # | Task | Kind |
|---|------|------|
| 1 | `SurvivalDetailPanel` magic bands -> owning `NeedsProfile` | repair |
| 2 | `SurvivalDetailPanel` localization | feature |
| 3 | `StatusPanel.RenderThermal` warmth band -> `ForLow` | repair |
| 4 | HUD radiation meter fill shares the label's band | repair |
| 5 | Pin `DoseLedgerPanel.MapBand` vs `ForDose` scale separation | test |
| 6 | Band Water/Food Stores rows | feature |
| 7 | Band Survivor Cohort card when `alive < total` | feature |
| 8 | Localize `[UNNAMED]` injury placeholder | feature |
| 9 | Localize `Day +N` forecast prefix | feature |
| 10 | Localize the composed `— SHORT` forecast suffix | feature |
| 11 | Band tooltips on detail-panel radiation rows | feature |
| 12 | Pin `NeedsDayDeltaFormat.Signed` clamp bounds | test |
| 13 | Pin the `Signed` call in `StatusPanel.AddDriftRow` | test |
| 14 | Add `AshfallUiBands` to the a11y source-audit token list | test |
| 15 | Pin `ui.resource.food` / `ui.resource.water` as the toast keys | test |

## 5. Verification and termination criteria

- [x] Plan approved before any production edit.
- [x] `dotnet build Ashfall.csproj` — the six files this package touched compile
      clean. The project as a whole currently fails on ONE foreign line,
      `src/Main.UiTests.PlayerPanels.cs:115` (`Button.Press()` does not exist in
      Godot 4). Roslyn reports every error in a single pass and returned exactly
      that one, so no error is attributed to this package. Reported, not edited
      (rule 6/10).
- [x] `dotnet build Ashfall.Core.Tests` — 0 errors / 0 warnings.
- [x] `scripts/ci/l10n_drift_gate.py` PASS — 716 keys, 190 localized-surface
      references, German parity verified.
- [x] Focused test classes only (no full suite): `StatusPanelThresholdTests`
      167/167, `AccessibilitySourceAuditTests` 9/9, `StatusGlanceAndForecastGateTests`
      7/7, `UiScrimContrastGateTests` 6/6 — **189/189**.
- [x] Headless runtime `--ui-layout-selftest` PASS (failures 0) — covering this
      package's changes through Loop 2. The Loop-3 metric-card bridge was added
      afterwards and is compile-verified but not yet runtime-exercised; re-run
      once the foreign line is repaired.
- [x] Three loops recorded in `.ai/state.md`.

## 5a. Loop log

| Loop | Finding | Action |
|------|---------|--------|
| 1 | **I duplicated the helper I had just built.** The new stores rows used a local `BandStores` returning `Lethe` for nominal, making them the only banded rows on the card off the shared nominal token. Stores are low-is-bad exactly like morale | Second band rule deleted; rows use `AshfallUiBands.ForLow`; test now forbids its return |
| 1 | Four dead `private Label _lbl*Title` fields in `SurvivalDetailPanel` — declared, never read or written (titles live in the .tscn, bound by SceneBinder) | Removed, with a comment explaining why they were misleading dead state |
| 2 | Three rows used `Theme.Dim` — the panel's token for *absent secondary detail* — to render states that are actually good news: "0 above threshold", "0 critically ill", and a shelter figure that read identically whether sound or collapsed | Healthy states use `Calm`; the shelter figure is banded with named consts; gate forbids `? Theme.Dim` returning as a health state |
| 3 | Runtime PASS. A static sweep then found the UI tree still carries a **second band vocabulary**: status-rail metric cards run on `AshfallMetricCard.Criticality`, and **123 UI files** still band them against re-typed literals (fervor 70, catalyst 40, condition 50, dose rate 10, stamina 30) | Folding all of that in is its own package. Added a sanctioned bridge — `ToCriticality` / `CriticalityForNeed` / `CriticalityForDose` — that reuses the band rules instead of restating them, and pinned it |

## 5b. Coverage note (test hygiene)

Tasks 1, 3, 9, 12, 13 and 15 were **already gated** by a concurrent session's
ratchet suite (`UiSources_DoNotRetypeHealthBandLiterals`,
`UiSources_DoNotRetypeRadiationWarnLiteral`, `UiSources_DoNotRetypeCohortNeedLiterals`,
`StatusPanel_HasNoRawInterpolatedLabel`, `StatusPanel_ForecastDayLabel_IsLocalized`).
Per TEST_POLICY rule 9 this package did **not** re-add gates for them in a second
wording. Task 9 was likewise already implemented; it was verified, not redone.

## 6. Fifteen follow-on small tasks

Ordered by evidence. #1 is the largest remaining truthfulness debt in the UI tree.

1. **Migrate the metric-card vocabulary** — convert the 123
   `AshfallMetricCard.Criticality` call sites to `AshfallUiBands.CriticalityFor*`
   in ranked batches (start with radiation/dose, then needs, then condition).
   The bridge from Loop 3 exists precisely for this.
2. Repair `src/Main.UiTests.PlayerPanels.cs:115` — replace `Button.Press()` with
   `EmitSignal(BaseButton.SignalName.Pressed)`; it currently breaks the shared
   host build for every agent.
3. `BeliefsPanel` avgFervor `>= 70` — read the owning fervour authority instead
   of the literal.
4. `CyberneticsPanel` avgCondition `< 50f` — same treatment.
5. `ChemicalLabPanel` avgCatalyst `< 40f` — same treatment.
6. `DoseGeographyPanel` peak `< 10f` / risk `< 5` — same treatment.
7. `ExpeditionPanel` stamina `< 30` — same treatment.
8. Re-run `--ui-layout-selftest` and re-baseline nothing; confirm the Loop-3
   bridge is runtime-clean once #2 lands.
9. Add a ratchet forbidding new `AshfallMetricCard.Criticality.Warn` call sites
   that pass a bare numeric literal, so the migration cannot regress.
10. `ChemicalDependencyPanel` detox rows use `DesignTheme.Hot`/`Lethe`/`Dim`
    outside the token system — route through the shared band vocabulary.
11. Add `SurvivalDetailPanel` to the golden snapshot corpus (it currently has no
    `survival_detail_default.png`), so its twelve rows gain a visual contract.
12. Add a test pinning that `AshfallUiBands.Calm` and `.Nominal` are never used
    interchangeably on the same card.
13. Localize the `SurvivalDetailPanel` section titles by moving them out of the
    `.tscn` into `MakeSectionHeader(Tr(...))` (needs the dead-field decision
    revisited — the .tscn currently owns them).
14. Add the `ui.survival_detail.*` keys to the l10n key-shape test that pins
    `{0}`/`{1}` placeholder parity per row.
15. Add a gate that every `AshfallUiBands` call site passes `warnAt:`/`criticalAt:`
    as named arguments, preventing the Loop-1 argument-order bug class entirely.
