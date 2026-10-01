# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# Survival Legibility Sixth Wave — 15 Tasks (3 substeps each) + 5 Loops

> **STATUS: APPROVED BY USER** (user directive: "complete all of these small
> tasks, after please again do a loop … repeat for 5 loops and then suggest 15
> very small tasks … with 3 substeps [per] small task!")

## The 15 tasks (each with its 3 substeps)

1. **SurvivorsPanel filter hints + sidebar title.**
   (a) 5 keys. (b) Routed `Hint` + shell title. (c) `SurvivorsPanel_FilterHintsAndSidebar_AreLocalized`.
2. **SurvivorDetailPanel origin/fitness/critical-flags.**
   (a) 7 keys. (b) `TrFormat`. (c) `SurvivorDetailPanel_StatusRows_AreLocalized`.
3. **StatusPanel objectives.**
   (a) 7 keys. (b) Routed `objectives.Add`. (c) `StatusPanel_Objectives_AreLocalized`.
4. **StatusPanel system-status values.**
   (a) 5 keys. (b) `TrFormat` with pre-formatted numbers. (c) `StatusPanel_SystemValues_AreLocalized`.
5. **`ui.survivor.*` presence gate.**
   (a) 22 keys enumerated. (b) `UiSurvivorKeys_ResolveFromStringsCsv`. (c) Verified.
6. **`ui.survivors.*` presence gate.**
   (a) 26 keys enumerated. (b) `UiSurvivorsKeys_ResolveFromStringsCsv`. (c) Verified.
7. **Predicate usage sweep.**
   (a) Ban raw `>= profile.hungerCritical`/`thirstCritical`/`fatigueCritical`.
   (b) `UiSources_UseProfilePredicates_NotRawCriticals`. (c) Verified.
8. **Identical en/de audit.**
   (a) Count shared terms. (b) Pin `<= 20`. (c) `StringsCsv_IdenticalEnglishGerman_StaysBounded`.
9. **Detail-panel localization purity.**
   (a) No catalog import. (b) `SurvivorDetailPanel_DoesNotImportCatalogNamespace`. (c) Verified.
10. **Layout wrapper summary parser.**
    (a) Parse `Failures:`. (b) Non-zero exit on >0. (c) `UiLayoutCheckScript_ParsesFailures`.
11. **Localize `ROSTER OPS` sidebar title.**
    (a) Key. (b) Routed. (c) Same gate as task 1.
12. **HUD need-chip label gate.**
    (a) 5 keys. (b) `HudNeedChipLabels_ResolveFromStringsCsv`. (c) Verified.
13. **Key-prefix collision audit.**
    (a) Detect dot-prefix keys. (b) Pin `<= 10`. (c) `StringsCsv_KeyPrefixCollisions_StayBounded`.
14. **`scripts/ci/README.md` layout entry.**
    (a) Document invocation. (b) Note the 180s cap. (c) `CiReadme_DocumentsLayoutCheck`.
15. **`strings.csv` line-length gate.**
    (a) Flag >400. (b) Allowlist prose suffixes. (c) `StringsCsv_LongLines_AreAllowlisted`.

## Five find → repair → harden loops

1. **Stale acute-rad gate:** `StatusGlanceAndForecastGateTests` asserted the
   English phrase my localization moved into the catalog → pinned the key +
   catalog row instead.
2. **Factors trailing-space key:** `ui.survivor.status.factors` ended in a space
   (fragile CSV field) → moved the separator into code + gate.
3. **Objective tags still raw:** `[PRIMARY]`/`[DAILY]`/… → `ObjectiveTag` mapping
   + 4 keys + gate.
4. **Forecast values still raw:** `food X · water Y` / `— SHORT` / shortfall
   sentence → 3 keys + gate.
5. **Thermal values still raw:** roster/clothing/shelter values + boiler on/off →
   5 keys + gate.

**Bonus repair (foreign-adjacent):** a concurrent edit added
`ui.status.expedition_injury` to `StatusPanel`; the l10n gate flagged it missing
then duplicated. Confirmed the concurrent session had since added the row and
removed the duplicate I briefly introduced; gate PASS at 623 keys.

## Verification

- Host build: **0 warnings / 0 errors** (after the foreign `ExpeditionHostSession`
  duplicate-method break was resolved by its owner).
- `StatusPanelThresholdTests` **90/90**; `NeedsDayDeltaTests` **29/29**.
- `l10n_drift_gate` **PASS** (623 keys, German parity).
- Scoped suite: **18/18 passed, 0 failed**.
- `scripts/ci/ui-layout-check.sh`: **Failures: 0 / PASS** (new summary parser).
- `git diff --check` clean. No commit; full suite not run; foreign worktree
  preserved.

## Next 15 very small tasks (each with 3 substeps)

1. Localize the `StatusPanel` expedition-injury row value.
   (a) `ui.status.expedition_injury.value` with `{0}`/`{1}`/`{2}`.
   (b) `TrFormat` the row. (c) Catalog gate.
2. Localize `StatusPanel` `DAY & ENVIRONMENT` remaining values.
   (a) `ui.status.day.value.*` keys. (b) Route. (c) Catalog gate.
3. Add a `ui.status.objective.tag.*` presence gate.
   (a) 4 keys. (b) Assert. (c) Verify.
4. Add a `ui.status.thermal.*` presence gate.
   (a) Enumerate 9 keys. (b) Assert. (c) Verify.
5. Localize `SurvivorDetailPanel` `Body`/`Clothing`/`Specialization` rows.
   (a) Keys. (b) `TrFormat`. (c) Catalog gate.
6. Add a `SurvivorDetailPanel` `Tr`-only ratchet.
   (a) Ban `$"` labels in `_needsList`/`_traitsList`. (b) Gate. (c) Verify.
7. Add a `SurvivorsPanel` filter-hint presence gate.
   (a) 4 keys. (b) Assert. (c) Verify.
8. Localize `SurvivorsPanel` `LastEvent` prefix + `Roster empty` variants.
   (a) Keys. (b) Route. (c) Catalog gate.
9. Add a `strings.csv` trailing/leading-space audit.
   (a) Flag fields with edge whitespace. (b) Allowlist none. (c) Verify.
10. Add a `strings.csv` empty-German gate for all `ui.*` rows.
    (a) Detect empty. (b) Fail with keys. (c) Verify.
11. Add a `StatusPanel` `ObjectiveTag` exhaustiveness test.
    (a) All 4 codes mapped. (b) Unknown falls to standing. (c) Verify.
12. Add a `NeedsProfile` predicate symmetry gate for `IsHealthCritical`.
    (a) Add predicate. (b) Test. (c) Verify.
13. Gate `StatusPanel` `AddStatRow` label keys resolve.
    (a) Enumerate stat labels. (b) Assert. (c) Verify.
14. Add a `scripts/ci/README.md` l10n entry.
    (a) Document the gate. (b) Note German parity. (c) Verify.
15. Add a bounded `--ui-layout-selftest` result JSON artifact.
    (a) Write `artifacts/ui-layout-selftest.json`. (b) Parse in wrapper.
    (c) Verify.