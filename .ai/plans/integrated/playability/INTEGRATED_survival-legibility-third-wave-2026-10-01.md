# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# Survival Legibility Third Wave — 15 Small Tasks + 5 Find→Repair→Harden Loops

> **STATUS: APPROVED BY USER** (user directive: "Continue with these small tasks,
> after please again do a loop of finding issues, repairing issues, hardening …
> repeat for 5 loops and then suggest 15 very small tasks …")

## Outcome

The 15 recorded follow-ups from the P009–P012 plan are landed:

1. `StatusPanel` cohort rows localized (`ui.status.cohort.*`).
2. `SurvivorsPanel` `RESIDENT ROSTER` / `COHORT TELEMETRY` headers localized.
3. Filter-label + cohort/header/resource key-presence assertions added to the
   catalog gate.
4. `Tr`/`TrFormat` extracted into shared `AshfallUiText`; StatusPanel,
   SurvivorsPanel, and GameHudOverlay delegate to it.
5. `HoldfastRuntimeSession` fallback test now pins all five fallback needs.
6. `GameHudSnapshotFixture` source gate pins every `UpdateX` signature.
7. `SurvivorDetailPanel` radiation row colours from `RadiationSystem.WarnThreshold`.
8. `StatusPanel` morale drift row added (tracker already tracked morale).
9. `SurvivorsPanel` `avgRad`/`avgMor` rail cards colour from shared bands.
10. `ui.hud.needs.warmth` tooltip assertion added to the a11y source audit.
11. `radiation_high` warn-band toast moved to `SurvivorsHostSession` (host-owned,
    per-survivor dedupe); player-path duplicate removed.
12. `NeedsDayDeltaFormat.Signed` returns `"0"` for `NaN`/`±Infinity` + theory.
13. `UpdateHud` survivors-local guard pinned by a source test.
14. `game_hud_default` golden-presence + harness-registration assertion added.
15. Low-food/low-water toast arguments now pass a localized resource display
    name (`ui.resource.food`/`water`); catalog templates use `{1}`.

## Five find → repair → harden loops

1. **Stale strain gate:** `SurvivorsPanel_StrainThresholds_ReadTheProfile`
   asserted `profile.warmthCritical`; the live code moved to the
   `profile.IsWarmthCritical(...)` predicate → assertion repaired, gate kept.
2. **HUD localizer drift:** `GameHudOverlay` still resolved strings directly →
   delegated to `AshfallUiText` + gate forbids `=> AshfallLocalization.Tr(`.
3. **Dead tooltip assignment:** `ApplyNeedChip` set `TooltipText` twice, so the
   authored `.high`/`.low` band tooltips were never shown → duplicate removed +
   gate forbids the generic-key overwrite.
4. **Radiation warn not re-armed on restore:** `_radWarnNotified` survived
   `RestoreSave` → cleared on restore + gate.
5. **Roster delta bypassed the shared formatter:** `SurvivorsPanel.DayDelta`
   re-implemented signed formatting (could render `-0`/`NaN`) → routed through
   `NeedsDayDeltaFormat.Signed(delta)` + gate.

## Verification

- Host build: 0 warnings / 0 errors.
- `StatusPanelThresholdTests` 37/37; `NeedsDayDeltaTests` 23/23;
  `FeedbackMessageTests` 11/11; `AccessibilitySourceAuditTests` 7/7;
  `SnapshotCorpusPinTests` 3/3.
- Scoped suite: 17 targets ran, 16 passed. The single failure is a **foreign
  concurrent regression** in `SceneBindingTruthGateTests`
  (`WeatherDetailPanel.tscn` / `WaterTreatmentPanel.tscn` scene-root mismatch)
  unrelated to this wave; its paths were not touched here.
- `l10n_drift_gate` PASS (512 keys, German parity); `git diff --check` clean.
- No commit; full suite not run; foreign dirty worktree preserved.

## Next 15 very small tasks (immediately integrable)

1. Route the remaining `T(...)` private helpers (`DutyRosterPanel`,
   `ShelterHudPanel`, `SilentFoundryPanel`, `SkillMatrixPanel`,
   `TriangulationPanel`, `VisitorIntegrationPanel`, `WorkshopPanel`) through
   `AshfallUiText`, with a ratchet gate.
2. Add a source gate that `ApplyNeedChip` assigns `TooltipText` exactly once.
3. Localize `StatusPanel` "Water Stores"/"Food Stores"/"External Conditions"/
   "Power Reserve"/"Radiation Shielding" rows.
4. Localize `SurvivorsPanel` shell title `SURVIVOR ROSTER & DUTY COHORT`.
5. Add a `ui.resource.*` key-presence assertion for every feedback toast noun.
6. Add a `NeedsDayDeltaFormat` `Signed` culture-invariance test (tr-TR locale).
7. Gate `SurvivorsPanel.DayDelta` unit suffix (`/{span}d`) against
   `NeedDaySpan` returning 0/negative.
8. Add a host-side dedupe test for `radiation_warn_<id>` re-arm below band.
9. Add a `StatusPanel` test that the morale drift row uses `positiveIsGood`.
10. Colour `SurvivorDetailPanel` "Lifetime Exposure" row from
    `RadiationSystem.ChronicLifetimeThreshold`.
11. Add a `Main.GameFlow` source gate that no `radiation_high` toast is emitted
    on the player path.
12. Assert `ui.status.cohort.*` and `ui.survivors.header.*` German rows are
    non-empty in the l10n gate.
13. Add a `GameHudSnapshotFixture` fixture-state assertion for the WARM chip.
14. Add a `strings.csv` duplicate-key gate (first-wins silently today).
15. Add a bounded `--ui-layout-selftest` snapshot for the StatusPanel cohort
    rows at 1920×1080.
