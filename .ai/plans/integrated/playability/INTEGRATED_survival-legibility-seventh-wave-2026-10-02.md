# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

> **STATUS: APPROVED BY USER** (user directive: "Continue with these small
> tasks, after please again do a loop of finding issues, repairing issues,
> hardening the spot where issues found to prevent future hiccups and then
> repeat for 5 loops and then suggest 15 very small tasks … : 15 very small
> tasks immediately integrable")

# Survival Legibility Seventh Wave — 15 Tasks (3 substeps each) + 5 Loops

## The 15 tasks (each with its 3 substeps)

1. **StatusPanel expedition-injury value.**
   (a) `ui.status.expedition_injury.value` `{0}`/`{1}`/`{2}`. (b) `TrFormat`
   the row. (c) `StatusPanel_ExpeditionInjuryValue_IsLocalized`.
2. **StatusPanel day/environment remaining value.**
   (a) `ui.status.day.weather_value`. (b) Route the condition wrapper.
   (c) `StatusPanel_DayWeatherValue_IsLocalized`.
3. **`ui.status.objective.tag.*` presence gate.**
   (a) 4 keys. (b) Assert in catalog + code. (c) `StatusPanel_ObjectiveTagKeys_ResolveFromStringsCsv`.
4. **`ui.status.thermal.*` presence gate.**
   (a) 9 keys. (b) Assert. (c) `StatusPanel_ThermalKeys_ResolveFromStringsCsv`.
5. **SurvivorDetailPanel Body/Clothing/Specialization.**
   (a) 3 keys. (b) `TrFormat`. (c) `SurvivorDetailPanel_BodyClothingSpecialization_AreLocalized`.
6. **Detail-panel Tr-only ratchet.**
   (a) Ban raw `$"` labels in `_needsList`/`_traitsList`. (b) Gate.
   (c) `SurvivorDetailPanel_NeedsAndTraits_RowsRouteThroughTr`.
7. **SurvivorsPanel filter-hint presence gate.**
   (a) 4 keys. (b) Assert. (c) `SurvivorsPanel_FilterHintKeys_ResolveFromStringsCsv`.
8. **SurvivorsPanel LastEvent prefix + empty variant.**
   (a) `ui.survivors.event.line` + `ui.survivors.event.none`. (b) Route.
   (c) `SurvivorsPanel_EventLine_IsLocalized`.
9. **`strings.csv` edge-whitespace audit.**
   (a) Flag edge whitespace. (b) Allowlist none. (c) `StringsCsv_HasNoEdgeWhitespace`.
10. **`ui.*` empty-German / copied-English gate.**
    (a) Detect. (b) Fail with keys + reviewed allowlist.
    (c) `UiStringsCsv_HasNoEmptyGerman_AndOnlyAllowlistedIdenticalRows`.
11. **ObjectiveTag exhaustiveness.**
    (a) 4 codes. (b) Unknown falls back to standing.
    (c) `ObjectiveTag_MapsEveryCode_AndFallsBackToStanding`.
12. **`IsHealthCritical` predicate.**
    (a) Add to `NeedsProfile`. (b) Boundary test. (c) `NeedsProfile_ExposesHealthCriticalPredicate`
    + `Panels_UseHealthCriticalPredicate_NotRawComparison`.
13. **StatusPanel AddStatRow label keys.**
    (a) Enumerate 13 labels. (b) Assert catalog + code.
    (c) `StatusPanel_AddStatRowLabels_ResolveFromStringsCsv`.
14. **`scripts/ci/README.md` l10n entry.**
    (a) Document the gate. (b) Note German parity. (c) `CiReadme_DocumentsL10nGate`.
15. **`--ui-layout-selftest` result JSON artifact.**
    (a) Write `artifacts/ui-layout-selftest.json`. (b) Verify in the wrapper.
    (c) `UiLayoutCheckScript_VerifiesArtifact`.

## Five find → repair → harden loops

1. **Malformed CSV row (real defect).** `ui.expedition.dispatch_speed` had an
   unquoted German decimal comma (`SCHNELL-EINSATZ ENTSENDEN (1,5x)`) that
   split the row into five fields, so the source-column gate read `5x)`. The
   German field is now quoted. Hardening already present:
   `Catalog_EveryRowHasFourFields…` fails on any non-4-field row.
2. **Trailing-space prefix keys (real defect).** `ui.duty_roster.assign_prefix`
   (`"ASSIGN: "`) and `ui.foundry.machine_prefix` (`"MACHINE // "`) carried the
   separator inside the catalog value. Both now store the bare label and the
   separator is added in code; `StringsCsv_HasNoEdgeWhitespace` prevents a
   future fragile field.
3. **Stale radiation-band test vs. new authority.** The concurrent sweep moved
   `SurvivorDetailPanel`'s dose row from an inline `RadiationSystem.WarnThreshold`
   comparison to the shared `AshfallUiBands.ForDose` helper. The existing
   `SurvivorDetailPanel_RadiationRow_UsesSharedWarnBand` gate still asserted the
   old literal; it now pins the helper the row actually consumes (current
   evidence).
4. **My own unquoted-comma keys (self-caught).** `ui.survivor.info.specialization`
   and `ui.survivor.performance.row` contained commas in the English/German
   fields and were re-quoted after the source-column gate flagged them.
5. **Identical-format keys.** `ui.status.day.weather_value` and
   `ui.survivors.event.line` are structural formats (`{0} — {1}`, `{0}: {1}`)
   that are identical by design; they are allowlisted in the new `ui.*`
   copied-English gate rather than silently tolerated.
6. **Bonus foreign-adjacent repair.** The concurrent sweep added a `TrFmt` helper
   in `ExpeditionPanel.cs` that called `AshfallLocalization.Tr(...)` directly,
   tripping `NoUiTextHelper_CallsAshfallLocalizationDirectly`. Routed through
   `AshfallUiText.Tr(...)` so every UI helper keeps one resolution path.

## Concurrent-edit note

`assets/l10n/strings.csv` and `src/UI/SurvivorDetailPanel.cs` were being
edited by a concurrent localization sweep during this package. The append of
the eight new keys was clobbered once and re-applied; the final catalog (635
keys) contains all of them and the drift gate is green. My four
`SurvivorDetailPanel` edits and the concurrent dose-row refactor coexist in the
file. No foreign path was reverted.

## Verification

- Test build: **0 warnings / 0 errors**.
- Host build (`dotnet build Ashfall.csproj`): **0 warnings / 0 errors**.
- `StatusPanelThresholdTests`: **106/106**.
- `NeedsDayDeltaTests`: **29/29**.
- `StringsCsvLocaleGateTests`: **4/4**; `LocalizationRatchetTests`: **2/2**.
- `python3 scripts/ci/l10n_drift_gate.py`: **PASS** (635 keys, 115 pilot
  references, 174 localized-surface references, German parity).
- `scripts/ci/ui-layout-check.sh`: **PASS / Failures: 0**, artifact
  `artifacts/ui-layout-selftest.json` = `{"test":"ui_layout_selftest","status":"PASS","failures":0}`.
- `git diff --check`: clean. No commit; full suite not run; foreign worktree
  preserved.

## Next 15 very small tasks (each with 3 substeps)

1. Localize `SurvivorDetailPanel` `_statusList` labels
   (`Top active need contributors`, `Recent need contributors`,
   `+N other active contributors`). (a) 3 keys. (b) Route. (c) Extend the
   Tr-only ratchet to `_statusList`.
2. Localize the `SurvivorDetailPanel` not-found/no-selection dim lines.
   (a) 2 keys. (b) Route `MakeDimLine`. (c) Catalog gate.
3. Reconcile the StatusPanel `critical_health` objective band: it counts
   `Health < healthWarn` but the key/text say "critical". (a) Decide warn vs
   critical. (b) Route through the predicate. (c) Gate the chosen band.
4. Add `IsHealthWarn` predicate symmetry for the health warn band.
   (a) Add. (b) Test. (c) Use in the two panels.
5. Localize the `SurvivorDetailPanel` worldview/keepsake/duty/trait row labels.
   (a) 6 keys. (b) Route. (c) Gate.
6. Localize the `SurvivorDetailPanel` faction/romance/family/lineage/bloc rows.
   (a) 6 keys. (b) Route. (c) Gate.
7. Localize the `SurvivorDetailPanel` demographic/personal-effects/records rows.
   (a) 6 keys. (b) Route. (c) Gate.
8. Localize the `SurvivorDetailPanel` modifier-row headers and `+N` overflow.
   (a) 4 keys. (b) Route. (c) Gate.
9. Add a `SurvivorDetailPanel` placeholder/raw-`$"` ratchet for the whole file.
   (a) Detect. (b) Allowlist the code-like formatters. (c) Gate.
10. Add a `strings.csv` `ui.*` placeholder-name gate (e.g. `{0}` only, no
    `{count}` drift). (a) Detect. (b) Pin allowed names. (c) Verify.
11. Add a `strings.csv` source-column extension check (`.cs` or `.json` only).
    (a) Detect. (b) Fail otherwise. (c) Verify.
12. Add a `StatusPanel` section-header presence gate (7 headers).
    (a) Enumerate. (b) Assert. (c) Verify.
13. Add a `SurvivorsPanel` rail-caption presence gate (5 keys). (a) Enumerate.
    (b) Assert. (c) Verify.
14. Add a `scripts/ci/README.md` artifact entry for
    `artifacts/ui-layout-selftest.json`. (a) Document. (b) Note the parse
    contract. (c) Verify.
15. Add a wrapper artifact-freshness check (mtime newer than the run log).
    (a) Compare. (b) Fail on stale. (c) Verify.