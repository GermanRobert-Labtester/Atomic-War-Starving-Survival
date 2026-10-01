# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

> **STATUS: APPROVED BY USER** (user directive: "Continue with these small tasks
> completely finish all of them, after please again do a loop of finding issues,
> repairing issues, hardening the spot where issues found … repeat for 3 loops
> and then suggest 15 very small tasks … Full integrate the given tasks first
> though before suggesting new ones!")

# Survival Legibility Eighth Wave — 15 Tasks (3 substeps each) + 3 Loops

## The 15 tasks (each with its 3 substeps)

1. **SurvivorDetailPanel `_statusList` labels.** (a) 3 keys. (b) Route.
   (c) `SurvivorDetailPanel_StatusListLabels_AreLocalized`.
2. **SurvivorDetailPanel dim lines.** (a) `no_selection`/`not_found`/`no_radiation`/
   `unknown`/`unknown_source`. (b) Route `MakeDimLine` + helpers.
   (c) `SurvivorDetailPanel_DimLines_AreLocalized`.
3. **Reconcile `critical_health` objective band.** (a) Decide warn vs critical.
   (b) Route `IsHealthCritical`. (c) `StatusPanel_CriticalHealthObjective_UsesCriticalBand`.
4. **`IsHealthWarn` predicate.** (a) Add to `NeedsProfile`. (b) Boundary test.
   (c) `Panels_UseHealthWarnPredicate_NotRawComparison`.
5. **Worldview/keepsake/duty/traits rows.** (a) 6 keys. (b) `TrFormat`.
   (c) `SurvivorDetailPanel_WorldviewAndIdentityRows_AreLocalized`.
6. **Faction/romance/family/lineage/bloc rows.** (a) 6 keys. (b) `TrFormat`.
   (c) `SurvivorDetailPanel_RelationshipRows_AreLocalized`.
7. **Demographic/personal-effects/records rows.** (a) 9 keys. (b) `TrFormat`.
   (c) `SurvivorDetailPanel_EffectsAndRecordsRows_AreLocalized`.
8. **Modifier-row formats.** (a) `modifier_row`/`recent_row`. (b) `TrFormat`.
   (c) `SurvivorDetailPanel_ModifierRows_AreLocalized`.
9. **Whole-file raw-`$"` ratchet.** (a) Detect letter-prefixed interpolation.
   (b) Allow value-only formatters. (c) `SurvivorDetailPanel_HasNoRawInterpolatedLabel`.
10. **Placeholder-name gate.** (a) Detect named `{count}`. (b) Fail with keys.
    (c) `StringsCsv_PlaceholderNames_AreNumeric`.
11. **Source-column extension gate.** (a) Detect non-`.cs`/`.json`. (b) Fail.
    (c) `StringsCsv_SourceColumn_IsCsOrJson`.
12. **StatusPanel section-header gate.** (a) 7 keys. (b) Assert.
    (c) `StatusPanel_SectionHeaders_AreLocalized`.
13. **SurvivorsPanel rail-caption gate.** (a) 5 keys. (b) Assert.
    (c) `SurvivorsPanel_RailCaptions_AreLocalized`.
14. **README layout-artifact entry.** (a) Document. (b) Note the mtime parse
    contract. (c) `CiReadme_DocumentsLayoutArtifact`.
15. **Wrapper artifact-freshness check.** (a) Record `start_epoch`. (b) Reject
    an artifact older than the run. (c) `UiLayoutCheckScript_RejectsStaleArtifact`.

## Three find → repair → harden loops

1. **Health-band drift (real defect).** `AfflictionsPanel` (×2) and `MedicalPanel`
   (×2) re-typed `30f` for survivor health; `SurvivalDetailPanel` compared
   `healthWarn` raw. Added `NeedsProfile.DefaultHealthWarn`/`DefaultHealthCritical`,
   routed the panels to the shared constant/predicate. **Hardening:**
   `UiSources_DoNotRetypeHealthBandLiterals` (word-boundary regex, excludes
   `filterHealth`) and `NeedsProfile_ExposesSharedHealthBandDefaults`.
2. **Radiation-band drift + raw forecast label (real defect).** Six panels
   re-typed the 50 mSv warn band (`MedicalPanel` ×3, `GameDashboardPanel` ×2,
   `RadiationDetailPanel` ×2, `RadiationHistoryPanel`, `ShelterHudPanel`,
   `SurvivorsPanel`, plus `AchievementsPanel`'s dose colour); `StatusPanel` still
   rendered `$"Day +{n}"`. All now read `RadiationSystem.WarnThreshold` /
   `AcuteThreshold` / `AshfallUiBands.ForDose`, and the forecast label resolves
   from `ui.status.forecast.day_label`. **Hardening:**
   `UiSources_DoNotRetypeRadiationWarnLiteral`, `StatusPanel_HasNoRawInterpolatedLabel`,
   `StatusPanel_ForecastDayLabel_IsLocalized`. Also de-duplicated
   `ui.status.forecast.day_label` (a concurrent sweep had added it too).
3. **Stale tests vs current authority.** The concurrent band sweep fixed
   `SurvivalDetailPanel` (shared bands, no 50/80 literals) and `StatusPanel`
   warmth (`AshfallUiBands.ForLow`), breaking two tests that pinned the old
   defect. Re-pinned `SurvivalDetailPanel_UsesSharedBands_NotMagicThresholds`
   and `StatusPanel_RosterWarmth_HasWarnBand` to current evidence. **Hardening:**
   `UiSources_DoNotRetypeCohortNeedLiterals`.

## Verification

- Test build: **0 warnings / 0 errors**.
- Host build (`dotnet build Ashfall.csproj`): **0 warnings / 0 errors**.
- `StatusPanelThresholdTests`: **138/138**; `NeedsDayDeltaTests`: **29/29**.
- `StringsCsvLocaleGateTests`: **4/4**; `LocalizationRatchetTests`: **2/2**.
- `python3 scripts/ci/l10n_drift_gate.py`: **PASS** (694 keys, 115 pilot
  references, 190 localized-surface references, German parity).
- `scripts/ci/ui-layout-check.sh`: **PASS / Failures: 0**; artifact
  `{"test":"ui_layout_selftest","status":"PASS","failures":0}`.
- CSV integrity: 694 rows, 0 malformed, 0 duplicates, 0 placeholder mismatch,
  0 edge whitespace, 19 identical en/de (≤20), 0 named placeholders.
- `git diff --check`: clean. No commit; full suite not run; concurrent edits
  preserved and noted.

## Next 15 very small tasks (each with 3 substeps)

1. Localize `AchievementsPanel` stat labels (`Days Survived`, `Roster Alive`,
   `Average Health`, `Average Dose`). (a) 4 keys. (b) Route. (c) Gate.
2. Localize `AfflictionsPanel` `Critical health` affliction row.
   (a) Key. (b) Route. (c) Gate.
3. Localize `MedicalPanel` survivor-row labels/HP/condition prose.
   (a) Keys. (b) Route. (c) Gate.
4. Add a `SurvivalDetailPanel` localization pass (the known-unlocalized twin).
   (a) Enumerate. (b) Route. (c) Gate.
5. Add a `UiSources_DoNotRetypeWarmthBandLiteral` gate.
   (a) Detect. (b) Fix hits. (c) Verify.
6. Add a `UiSources_DoNotRetypeMoraleBandLiteral` gate.
   (a) Detect. (b) Fix hits. (c) Verify.
7. Add a `UiSources_DoNotRetypeHungerThirstBandLiteral` gate.
   (a) Detect. (b) Fix hits. (c) Verify.
8. Add a `StringsCsv_KeysAreSnakeCaseDotted` gate.
   (a) Detect. (b) Pin pattern. (c) Verify.
9. Add a `StringsCsv_NoLeadingTrailingQuoteSpace` gate.
   (a) Detect. (b) Fail. (c) Verify.
10. Add a `StatusPanel_ForecastRowCount_Bounded` layout gate.
    (a) Detect. (b) Pin ≤4. (c) Verify.
11. Add a `SurvivorDetailPanel_RenderedRowCount_Accounting` gate.
    (a) Detect. (b) Assert. (c) Verify.
12. Add a `strings.csv` duplicate-source audit (same source, many keys).
    (a) Count. (b) Pin. (c) Verify.
13. Add a `ci/README.md` localization-key-count note. (a) Document.
    (b) Note parity. (c) Verify.
14. Add a `run-godot-bounded.sh` artifact-presence check. (a) Detect.
    (b) Fail. (c) Verify.
15. Add a `--ui-layout-selftest` artifact schema gate (required fields).
    (a) Detect. (b) Fail. (c) Verify.