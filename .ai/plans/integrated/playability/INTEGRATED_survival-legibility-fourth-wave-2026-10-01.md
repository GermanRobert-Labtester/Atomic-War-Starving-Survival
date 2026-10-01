# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# Survival Legibility Fourth Wave — 15 Tasks (3 substeps each) + 5 Loops

> **STATUS: APPROVED BY USER** (user directive: "Continue with these small tasks,
> after please again do a loop … repeat for 5 loops and then suggest 15 very
> small tasks … with 3 substeps [per] small task!")

## The 15 tasks (each with its 3 substeps)

1. **Route remaining `T(...)` helpers through `AshfallUiText` + ratchet gate.**
   (a) Delegated the 9 panel helpers (`DutyRosterPanel`, `ShelterHudPanel`,
   `SilentFoundryPanel`, `SkillMatrixPanel`, `TriangulationPanel`,
   `VisitorIntegrationPanel`, `WorkshopPanel`, `OnboardingHintPanel`,
   `ResearchPanel`) and `ResearchPanel.F`. (b) Added
   `NoUiTextHelper_CallsAshfallLocalizationDirectly`. (c) Host build 0/0.
2. **Gate `ApplyNeedChip` assigns `TooltipText` exactly once.**
   (a) Counted assignments in the method body. (b) Added
   `Hud_ApplyNeedChip_AssignsTooltipExactlyOnce`. (c) Verified.
3. **Localize `StatusPanel` rows.**
   (a) Localized Current Day/Survivors/External Conditions/Power Reserve/
   Water Stores/Food Stores/Power Grid/Radiation Shielding/Outdoor Radiation and
   the 6 section headers + title. (b) Added 17 `ui.status.*` keys. (c) l10n gate.
4. **Localize `SurvivorsPanel` shell title.**
   (a) `ui.survivors.shell.title`. (b) Wrapped in `Tr`. (c) l10n gate.
5. **`ui.resource.*` presence gate for every toast noun.**
   (a) `ui.resource.food`/`water` keys. (b) `ResourceToastNouns_AreLocalized`.
   (c) Verified.
6. **`NeedsDayDeltaFormat.Signed` culture-invariance test.**
   (a) Test under `tr-TR`. (b) `NeedsDayDeltaFormat_IsCultureInvariant`.
   (c) 26/26.
7. **Gate `DayDelta` unit suffix vs non-positive `NeedDaySpan`.**
   (a) `DaySpan_NonPositiveAge_ClampsToOne`. (b) Source gate on the `span > 1`
   guard. (c) Verified.
8. **Host dedupe test for `radiation_warn_<id>` re-arm.**
   (a) `RadiationWarnToast_IsDedupedAndReArmedBelowBand`. (b) Asserts Remove/Add.
   (c) Verified.
9. **`StatusPanel` morale drift `positiveIsGood` test.**
   (a) `StatusPanel_MoraleDrift_IsPositiveIsGood`. (b) Verified.
10. **`SurvivorDetailPanel` Lifetime Exposure from `ChronicLifetimeThreshold`.**
    (a) Coloured the row. (b) Verified via build. (c) Recorded.
11. **Gate no player-path `radiation_high` emission.**
    (a) `PlayerPath_DoesNotEmitRadiationHighToast`. (b) Verified.
12. **Assert German rows non-empty for new keys.**
    (a) `NewLocalizedRows_HaveNonEmptyGerman`. (b) Verified.
13. **`GameHudSnapshotFixture` WARM-chip state assertion.**
    (a) `GameHudSnapshotFixture_PinsWarmthChipState`. (b) Verified.
14. **`strings.csv` duplicate-key gate.**
    (a) `StringsCsv_HasNoDuplicateKeys`. (b) Verified.
15. **Bounded `--ui-layout-selftest` for StatusPanel cohort rows.**
    (a) Source bounded-layout gate. (b) Ran the real Godot headless selftest.
    (c) `Failures: 0` / `UI_LAYOUT_SELFTEST PASS`.

## Five find → repair → harden loops

1. **Dead localization imports:** `StatusPanel`, `SurvivorsPanel`,
   `GameHudOverlay`, `ResearchPanel`, `OnboardingHintPanel` still imported the
   catalog namespace after delegating → removed + `LocalizationHelpers_DoNotImportTheCatalogNamespace`.
2. **Ratchet coverage:** the `T`/`Tr` helper ratchet now also covers the
   multi-line delegate form via the `AshfallLocalization.Tr(key, fallback);`
   ban → verified.
3. **Redundant catalog init:** `ResearchPanel.F` called
   `AshfallLocalization.Initialize()` before `AshfallUiText.Tr` (which
   initializes) → removed + `ResearchPanel_FormatHelper_DoesNotReinitializeTheCatalog`.
4. **Remaining hardcoded StatusPanel rows:** thermal + forecast labels were
   still English → localized + `StatusPanel_ThermalAndForecastRows_AreLocalized`.
5. **Drift formatting centralization:** pinned both survival panels to
   `NeedsDayDeltaFormat` and banned the inline `(delta >= 0f ?` form →
   `DriftFormatting_IsCentralized`.

## Verification

- Host build: **0 warnings / 0 errors**.
- `StatusPanelThresholdTests` **53/53**; `NeedsDayDeltaTests` **26/26**.
- `l10n_drift_gate` **PASS** (535 keys, German parity).
- Scoped suite: **17/17 passed, 0 failed**.
- Godot headless `--ui-layout-selftest`: **Failures: 0 / PASS**.
- `git diff --check` clean. No commit; full suite not run; foreign worktree
  preserved.

## Next 15 very small tasks (each with 3 substeps)

1. Localize `SurvivorsPanel` roster metadata strings.
   (a) `ui.survivors.empty.roster`/`no_match`/`no_session` keys.
   (b) Route the 3 `MakeMetadata` calls. (c) Catalog gate.
2. Localize `SurvivorDetailPanel` needs/trait labels.
   (a) Keys for Health/Hunger/Thirst/Fatigue/Warmth/Morale/Hygiene.
   (b) Wrap in `AshfallUiText.Tr`. (c) l10n gate.
3. Add a `ui.status.*` key-presence gate.
   (a) Enumerate the 23 status keys. (b) Assert in `strings.csv`. (c) Verify.
4. Localize `SurvivorsPanel` rail card captions (`LIVING`/`AVG HP`/…).
   (a) 5 keys. (b) Route `AddCard`. (c) Catalog gate.
5. Add a `strings.csv` source-column validity gate.
   (a) Assert each source path exists. (b) Assert it references the key or a
   shared file. (c) Verify.
6. Colour `StatusPanel` `Roster Warmth` from `profile.warmthWarn`.
   (a) Add warn band. (b) Test. (c) Verify.
7. Add a `NeedsProfile.IsHungerCritical`/`IsThirstCritical` predicate test.
   (a) Symmetry with `IsWarmthCritical`. (b) Boundary cases. (c) Verify.
8. Gate `SurvivorDetailPanel` needs rows to profile bands.
   (a) Replace `Morale < 20` literal. (b) Source gate. (c) Verify.
9. Add a host re-arm test for `need_critical_` dedupe.
   (a) Below-band re-arm. (b) Source gate. (c) Verify.
10. Localize `StatusPanel` "No survivor roster bound." + NOT MONITORED.
    (a) Keys. (b) Wrap. (c) l10n gate.
11. Add a `ui.status.forecast.*` key-presence gate.
    (a) Enumerate. (b) Assert. (c) Verify.
12. Gate `AshfallUiText` stays presentation-only.
    (a) No `Ashfall.Core.Survivors`/`Godot` value imports. (b) Source gate.
    (c) Verify.
13. Add a `GameHudSnapshotFixture` stale-fixture guard.
    (a) Assert fixture lives in `src/UI`. (b) Assert it is referenced by
    `SnapshotHarness`. (c) Verify.
14. Add a duplicate-`TooltipText` sweep gate across `src/UI`.
    (a) Regex two consecutive assignments. (b) Fail with file names. (c) Verify.
15. Add a bounded `--ui-layout-selftest` wrapper script target.
    (a) `scripts/ci/ui-layout-check.sh`. (b) Invoke the bounded runner.
    (c) Document in the plan.
