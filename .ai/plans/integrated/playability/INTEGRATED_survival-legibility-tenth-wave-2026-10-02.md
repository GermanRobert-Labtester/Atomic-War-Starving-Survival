# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

> **STATUS: APPROVED BY USER** (user directive: "Continue with these small tasks
> completely finish all of them, after please again do a loop of finding issues,
> repairing issues, hardening the spot where issues found … repeat for 3 loops
> and then suggest 15 very small tasks … Full integrate the given tasks first
> though before suggesting new ones!")

# Survival Legibility Tenth Wave — 15 Tasks (3 substeps each) + 3 Loops

## The 15 tasks (each with its 3 substeps)

1. **AchievementsPanel milestone/target prose.** (a) 12 keys. (b) Route.
   (c) `AchievementsPanel_MilestoneProse_IsLocalized`.
2. **AfflictionsPanel accommodation feedback.** (a) 8 keys. (b) `TrFormat`.
   (c) `AfflictionsPanel_AccommodationFeedback_IsLocalized`.
3. **MedicalPanel treatment buttons.** (a) 9 keys. (b) Route.
   (c) `MedicalPanel_TreatmentButtons_AreLocalized`.
4. **MedicalPanel cohort/care strings.** (a) 2 keys. (b) Route.
   (c) `MedicalPanel_CohortAndCare_AreLocalized`.
5. **MedicalPanel unnamed fallback.** (a) Key. (b) Route.
   (c) `MedicalPanel_UnnamedFallback_IsLocalized`.
6. **GameDashboardPanel profile bands.** (a) Read defaults. (b) Route.
   (c) `GameDashboardPanel_UsesProfileNeedBands`.
7. **ShelterHudPanel profile bands.** (a) Read defaults. (b) Route.
   (c) `ShelterHudPanel_UsesProfileNeedBands`.
8. **MedicalPanel health-rail bands.** (a) Decide scale. (b) Route.
   (c) `MedicalPanel_HealthRail_UsesProfileBands`.
9. **Achievement catalog localization path.** (a) Per-id key helper. (b) Route.
   (c) `AchievementsPanel_HasCatalogLocalizationPath`.
10. **Identical en/de allowlist gate.** (a) Enumerate. (b) Pin exactly.
    (c) `StringsCsv_IdenticalEnglishGerman_AllowlistComplete`.
11. **UI source-file gate.** (a) Detect. (b) Fail. (c) `StringsCsv_SourceFileExistsForUiKeys`.
12. **Registered-surface raw-text sweep.** (a) Detect. (b) Fail.
    (c) `LocalizedSurfaces_HaveNoRawTextAssignment`.
13. **README artifact contract.** (a) Document. (b) Note usage.
    (c) `CiReadme_DocumentsArtifactContract`.
14. **Runner env-var docs.** (a) Document. (b) Note both vars.
    (c) `GodotBoundedRunner_DocumentsEnvVars`.
15. **Wrapper mtime ordering gate.** (a) Detect. (b) Assert order.
    (c) `UiLayoutArtifact_MtimeAfterRunStart`.

## Three find → repair → harden loops

1. **MedicalPanel care-log + empty-state strings (real defect).** Five
   `AddCareEntry(...)` log strings and nine empty-state metadata lines were raw
   English. Repaired to 15 catalog keys. **Hardening:**
   `MedicalPanel_HasNoRawMetadataOrCareLiteral`,
   `MedicalPanel_CareEntriesAndEmptyStates_AreLocalized`.
2. **AfflictionsPanel empty states + unknown fallback (real defect).** Six
   `MakeDimLine("...")` states and the `Name` helper's `"Unknown"` were raw.
   Repaired to 6 keys. **Hardening:** (covered by the same raw-literal sweep
   and `StringsCsv_SourceFileExistsForUiKeys`).
3. **Dead `ui.status.forecast.short` key (real defect).** The concurrent
   forecast refactor superseded it with `ui.status.forecast.day_value_short`,
   but the old row lingered and the old assertion passed only because the dead
   key is a substring of `ui.status.forecast.shortfall`. Removed the row,
   tightened the assertion, and added `StatusPanel_ForecastKeys_AreLive` so a
   superseded forecast row fails instead of hiding.

## Verification

- Test build: **0 warnings / 0 errors**.
- Host build (`dotnet build Ashfall.csproj`): **0 warnings / 0 errors**.
- `StatusPanelThresholdTests`: **185/185**; `NeedsDayDeltaTests`: **29/29**;
  `StringsCsvLocaleGateTests`: **4/4**; `LocalizationRatchetTests`: **2/2**.
- `python3 scripts/ci/l10n_drift_gate.py`: **PASS** (806 keys, 115 pilot
  references, 336 localized-surface references — up from 315 — German parity).
- `scripts/ci/ui-layout-check.sh`: **PASS / Failures: 0**; artifact
  `{"test":"ui_layout_selftest","status":"PASS","failures":0}`.
- CSV integrity: 806 rows, 0 malformed, 0 duplicates, 0 placeholder mismatch,
  0 edge whitespace, 20 identical en/de (≤20), 0 named placeholders.
- `git diff --check`: clean. No commit; full suite not run; concurrent edits
  preserved (a read race reported a transient malformed row that did not exist
  on re-read; a transient `csc` SIGTERM resolved on retry).

## Next 15 very small tasks (each with 3 substeps)

1. Localize `MedicalHostSession.VigilStatusLine` (host-owned English) by
   formatting in the panels from the public `Vigil` state.
   (a) 5 keys. (b) Route both panels. (c) Gate.
2. Localize `MedicalPanel` disease/ward rows and `IDENTIFY`/`ISOLATE`/`RELEASE`/
   `APPLY` buttons. (a) 6 keys. (b) Route. (c) Gate.
3. Localize `AfflictionsPanel` treatment rows and `Apply` buttons.
   (a) 6 keys. (b) Route. (c) Gate.
4. Localize `AfflictionsPanel` affliction/condition names from the catalog.
   (a) Decide authority. (b) Route. (c) Gate.
5. Localize `StartingCohortSetupPanel` `strained` note + cohort labels.
   (a) 6 keys. (b) Route. (c) Gate.
6. Align `StartingCohortSetupPanel` `36f` threshold with the profile or a
   documented setup constant. (a) Decide. (b) Route. (c) Gate.
7. Localize `ShelterHudPanel` `ROSTER`/`HUNGER`/`THIRST` grid labels.
   (a) 6 keys. (b) Route. (c) Gate.
8. Localize `GameDashboardPanel` directive/shelter-status strings.
   (a) 6 keys. (b) Route. (c) Gate.
9. Add a `UiSources_DoNotRetypeSetupBandLiteral` gate for the cohort setup.
   (a) Detect. (b) Fix. (c) Verify.
10. Add a general `StringsCsv_NoDeadUiKey` gate scoped to a curated panel list.
    (a) Detect. (b) Allowlist dynamic. (c) Verify.
11. Add a `LocalizedSurfaces_RegisteredInDriftGate` test for every panel that
    uses `AshfallUiText.Tr`. (a) Detect. (b) Compare. (c) Verify.
12. Add a `strings.csv` German-uniqueness audit (no two keys share a German
    string unexpectedly). (a) Detect. (b) Pin. (c) Verify.
13. Add a `scripts/ci/README.md` localization-surface registry note.
    (a) Document. (b) Note growth. (c) Verify.
14. Add a `run-godot-bounded.sh` `--check-staleness-only` mode for CI.
    (a) Implement. (b) Document. (c) Verify.
15. Add a `UiLayoutArtifact_FailureCountField` schema assertion.
    (a) Detect. (b) Assert. (c) Verify.