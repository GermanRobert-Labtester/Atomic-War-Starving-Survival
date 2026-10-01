# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# Survival Legibility Fifth Wave — 15 Tasks (3 substeps each) + 5 Loops

> **STATUS: APPROVED BY USER** (user directive: "complete all of these small
> tasks, after please again do a loop … repeat for 5 loops and then suggest 15
> very small tasks … with 3 substeps [per] small task!")

## The 15 tasks (each with its 3 substeps)

1. **Localize `SurvivorsPanel` roster metadata.**
   (a) 4 keys (`empty.no_session`/`empty.roster`/`empty.no_match`/`event.latest`).
   (b) Routed the 3 `MakeMetadata` calls. (c) `SurvivorsPanel_MetadataAndRailCaptions_AreLocalized`.
2. **Localize `SurvivorDetailPanel` needs/trait labels.**
   (a) 12 keys. (b) `AshfallUiText.Tr`/`TrFormat`. (c) `SurvivorDetailPanel_NeedsAndTraits_AreLocalized`.
3. **`ui.status.*` key-presence gate.**
   (a) 24 keys enumerated. (b) `StatusPanel_Keys_ResolveFromStringsCsv`. (c) Verified.
4. **Localize `SurvivorsPanel` rail captions.**
   (a) 5 keys. (b) Routed `AddCard`. (c) Same gate as task 1.
5. **`strings.csv` source-column validity gate.**
   (a) Quote-aware CSV parse. (b) `StringsCsv_SourceColumn_PointsAtExistingPaths`.
   (c) Fixed one stale row (`warning.radiation.storm` → `LocalizationService.cs`).
6. **Colour `Roster Warmth` from `profile.warmthWarn`.**
   (a) 3-band readout. (b) `StatusPanel_RosterWarmth_HasWarnBand`. (c) Verified.
7. **`IsHungerCritical`/`IsThirstCritical` (+fatigue/morale) predicates.**
   (a) Added 4 predicates. (b) `NeedsProfile_ExposesCriticalPredicates`.
   (c) 27/27 Core tests.
8. **Gate `SurvivorDetailPanel` needs rows to profile bands.**
   (a) Replaced `Morale < 20`. (b) `SurvivorDetailPanel_Needs_ReadProfilePredicates`.
   (c) Verified.
9. **Host `need_critical_` rising-edge re-arm test.**
   (a) Asserted Core guard. (b) `NeedCriticalDedupe_FiresOncePerRisingEdge`.
   (c) Verified.
10. **Localize NOT MONITORED / No roster bound.**
    (a) 2 keys. (b) Replaced 8 + 1 occurrences. (c) `StatusPanel_NotMonitored_IsLocalized`.
11. **`ui.status.forecast.*` presence gate.**
    (a) Enumerated. (b) `StatusPanel_ForecastKeys_ArePresent`. (c) Verified.
12. **Gate `AshfallUiText` presentation-only.**
    (a) No Godot/Core imports. (b) `AshfallUiText_StaysPresentationOnly`. (c) Verified.
13. **Fixture stale guard.**
    (a) File exists + harness registration.
    (b) `GameHudFixture_LivesInUiAndIsRegistered`. (c) Verified.
14. **Duplicate-`TooltipText` sweep across `src/UI`.**
    (a) Same-receiver within 4 lines regex.
    (b) `UiSources_HaveNoConsecutiveDuplicateTooltipAssignment`. (c) Verified.
15. **Bounded layout wrapper.**
    (a) `scripts/ci/ui-layout-check.sh`. (b) Invokes the bounded launcher.
    (c) `Failures: 0 / PASS`.

## Five find → repair → harden loops

1. **Stale source path:** `warning.radiation.storm` pointed at
   `Assets/Ashfall.Core/Weather/WeatherSystem.cs` (never existed) → corrected to
   `LocalizationService.cs` + task-5 gate.
2. **StatusPanel day-info values still English:** Day N / alive count / hazard /
   nominal / battery → 5 keys + `StatusPanel_DayInfoValues_AreLocalized`.
3. **SurvivorDetailPanel identity rows still English:** Name / Profession /
   Unspecified / Alive / Max Health Cap / tenure → 6 keys +
   `SurvivorDetailPanel_IdentityRows_AreLocalized`.
4. **SurvivorsPanel status labels leaked internal codes:** split the stable
   `FilterPass` code from a localized `StatusLabel` display → 4 keys +
   `SurvivorsPanel_StatusLabels_AreLocalized`.
5. **SurvivorsPanel cohort summary hardcoded:** → `ui.survivors.cohort.summary`
   + `SurvivorsPanel_CohortSummary_IsLocalized`.

## Verification

- Host build: **0 warnings / 0 errors**.
- `StatusPanelThresholdTests` **71/71**; `NeedsDayDeltaTests` **27/27**.
- `l10n_drift_gate` **PASS** (574 keys, German parity).
- Scoped suite: **17/17 passed, 0 failed**.
- `scripts/ci/ui-layout-check.sh`: **Failures: 0 / PASS**.
- `git diff --check` clean. No commit; full suite not run; foreign worktree
  preserved.

## Next 15 very small tasks (each with 3 substeps)

1. Localize `SurvivorsPanel` filter captions + hints.
   (a) `ui.survivors.filter.*.hint` keys. (b) Route sidebar `Hint`. (c) Catalog gate.
2. Localize `SurvivorDetailPanel` trait/status remaining rows.
   (a) `Origin`/`Fitness`/`Critical flags` keys. (b) `TrFormat`. (c) l10n gate.
3. Localize `StatusPanel` objective strings.
   (a) `ui.status.objective.*` keys. (b) Route `objectives.Add`. (c) Catalog gate.
4. Localize `StatusPanel` system-status values.
   (a) Power Grid/Shielding/Outdoor value keys. (b) `TrFormat`. (c) Catalog gate.
5. Add a `ui.survivor.*` key-presence gate.
   (a) Enumerate 22 keys. (b) Assert. (c) Verify.
6. Add a `ui.survivors.*` key-presence gate.
   (a) Enumerate 19 keys. (b) Assert. (c) Verify.
7. Add a `NeedsProfile` predicate usage sweep gate.
   (a) Ban raw `>= profile.hungerCritical` in UI. (b) Allow predicate only.
   (c) Verify.
8. Add a `StringsCsv` German-vs-English identical-row audit (informational).
   (a) Count identical rows. (b) Pin an upper bound. (c) Verify.
9. Gate `SurvivorDetailPanel` `AshfallUiText`-only localization.
   (a) No catalog import. (b) Assert. (c) Verify.
10. Add a `--ui-layout-selftest` summary parser to the wrapper.
    (a) Grep `Failures:` from output. (b) Exit non-zero if > 0. (c) Verify.
11. Localize `SurvivorsPanel` `Roster ops` sidebar title.
    (a) Key. (b) Route. (c) Catalog gate.
12. Add a `GameHudOverlay` need-chip label key-presence gate.
    (a) 5 keys. (b) Assert. (c) Verify.
13. Add a duplicate `ui.*` key-vs-prefix collision audit.
    (a) Detect a key that is a prefix of another. (b) Report. (c) Verify.
14. Add a `scripts/ci/README.md` entry for the layout wrapper.
    (a) Document invocation. (b) Note the 180s cap. (c) Verify.
15. Add a bounded `strings.csv` line-length gate.
    (a) Flag > 400 chars. (b) Allowlist long lesson bodies. (c) Verify.