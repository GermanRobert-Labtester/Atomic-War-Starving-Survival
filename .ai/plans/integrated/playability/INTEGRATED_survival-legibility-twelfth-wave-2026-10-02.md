# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

> **STATUS: APPROVED BY USER** (user directive: "Continue with these small tasks
> completely finish all of them, after please again do a loop of finding issues,
> repairing issues, hardening the spot where issues found … repeat for 3 loops
> and then suggest list only remaining if any very small tasks … Full integrate
> the given tasks first though before suggesting new ones!")

# Survival Legibility Twelfth Wave — 15 Tasks (3 substeps each) + 3 Loops

## The 15 tasks (each with its 3 substeps)

1. **MedicalWardPanel data-row labels.** (a) 9 keys. (b) Route.
   (c) covered by `MedicalWardPanel_HasNoRawChromeLiteral`.
2. **MedicalWardPanel bed-unit/vacant prose.** (a) already localized in the
   prior wave. (b) — (c) verified.
3. **ShelterHudPanel headers/buttons.** (a) 8 keys. (b) Route.
   (c) `ShelterHudPanel_ConditionLabels_AreLocalized` + chrome gate.
4. **ShelterHudPanel filter buttons.** (a) 2 keys. (b) Route. (c) chrome gate.
5. **GameDashboard headers + brand.** (a) 19 keys (brand wordmark exempt).
   (b) Route. (c) chrome gate.
6. **GameDashboard nav/action buttons.** (a) 8 keys. (b) Route. (c) chrome gate.
7. **Wire the concurrent `ui.expedition.*` keys with the separator in code.**
   (a) Route. (b) Fix the spacing regression. (c) dead-key gate.
8. **ExpeditionPanel world-state line.** (a) `ui.expedition.world_line` (already
   wired). (b) verified. (c) dead-key gate.
9. **Ward band-literal gate.** (a) Detect. (b) Clean.
   (c) `UiSources_DoNotRetypeWardBandLiteral`.
10. **Curated dead-key gate → `ui.expedition.`.** (a) Detect. (b) Reserve
    exemptions. (c) `StringsCsv_CuratedPrefixesHaveNoDeadKey`.
11. **Registered-panel existence gate.** (a) Detect. (b) Fail.
    (c) `StringsCsv_SourceColumnExistsForRegisteredPanels`.
12. **Registered-surface count floor.** (a) Count. (b) Pin ≥20.
    (c) `LocalizedSurfaces_CountStaysBounded`.
13. **README German-duplicate note.** (a) Document. (b) Note pin.
    (c) `CiReadme_DocumentsGermanDuplicateBound`.
14. **Assembly-mtime diagnostic.** (a) Implement. (b) Document.
    (c) `--print-assembly-mtime` verified.
15. **Artifact status/failure consistency.** (a) Detect. (b) Pin.
    (c) `UiLayoutArtifact_FailureCountMatchesStatus`.

## Three find → repair → harden loops

1. **Dashboard/ShelterHud raw chrome (real defect).** Seven raw
   `MakeBody`/`MakeActionButton`/`MakeMetadata` strings (filter buttons,
   directive, event default, autosave, advance) were hardcoded. Repaired to
   6 keys (reusing `ui.dashboard.keep_quiet`). **Hardening:**
   `DashboardAndShelterHud_HaveNoRawChromeLiteral` (brand wordmark exempt).
2. **Concurrent compile break (real defect).** A concurrent
   `ExpeditionLocaleKeysTests.UiExpeditionKeyPattern` used a verbatim string
   with an escaped quote (`@"\"…\""`), which does not compile. Repaired to a
   regular string. **Hardening:** the coverage audit confirmed every
   `AshfallUiText.Tr` user is registered with the drift gate.
3. **Concurrent `achievement.*` gate expansion (real defect).** The drift gate
   began deriving `achievement.{id}.name/description` from `achievements.json`,
   surfacing 32 missing rows. Added the German translations (then de-duplicated
   against the concurrent rows that landed mid-write).

## Verification

- Test build: **0 warnings / 0 errors**.
- Host build (`dotnet build Ashfall.csproj`): **0 warnings / 0 errors**.
- `StatusPanelThresholdTests`: **210/210**; `NeedsDayDeltaTests`: **29/29**;
  `StringsCsvLocaleGateTests`: **4/4**; `LocalizationRatchetTests`: **2/2**;
  `ExpeditionLocaleKeysTests`: **4/4**.
- `python3 scripts/ci/l10n_drift_gate.py`: **PASS** (1038 keys, 282
  pilot+dynamic references, 212 dynamic-family keys, 540 localized-surface
  references, German parity).
- `scripts/ci/ui-layout-check.sh`: **PASS / Failures: 0**; artifact
  `{"test":"ui_layout_selftest","status":"PASS","failures":0}`.
- `run-godot-bounded.sh --print-assembly-mtime` / `--check-staleness-only`: work.
- CSV integrity: 1038 rows, 0 malformed, 0 duplicates, 0 placeholder mismatch,
  0 edge whitespace, 20 identical en/de (≤20).
- `git diff --check`: clean. No commit; full suite not run; concurrent edits
  preserved (compile break fixed, duplicate rows de-duplicated).

## Remaining very small tasks (only what is genuinely small and clean)

The bulk of the remaining work is a broad ~162-file localization sweep (not a
small task). The following are the smallest, fully bounded panels, each a clean
single-panel task (localize the listed raw chrome + extend the chrome gate):

1. `WeatherDetailPanel.cs` — 1 string (`No weather system bound.`).
2. `TimeCapsulePanel.cs` — 1 (`UNSEAL CAPSULE`).
3. `SurvivorRelationsPanel.cs` — 1 (`Mediate Active Conflict`).
4. `KennelPanel.cs` — 1 (`ANIMAL ROSTER`).
5. `ExpansionsHubPanel.cs` — 1 (`RETURN TO DASHBOARD [ESC]`).
6. `CyberneticsPanel.cs` — 1 (`IMPLANT REGISTRY`).
7. `CryogenicPermafrostCorePanel.cs` — 1 (`No samples stabled in this vault.`).
8. `BeliefsPanel.cs` — 1 (`HELD DOCTRINES`).
9. `AnomalyWatchPanel.cs` — 1 (`SURVEY PICTURE`).
10. `WeatherForecastPanel.cs` — 2.
11. `VinylMoralePanel.cs` — 2.
12. `ShelterThermalPanel.cs` — 2.
13. `SanitationPanel.cs` — 2.
14. `RadioIntelligencePanel.cs` — 2.
15. `GameOverPanel.cs` — 3.