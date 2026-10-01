# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

> **STATUS: APPROVED BY USER** (user directive: "Continue with these small tasks
> completely finish all of them, after please again do a loop of finding issues,
> repairing issues, hardening the spot where issues found … repeat for 3 loops
> and then suggest 15 very small tasks … Full integrate the given tasks first
> though before suggesting new ones!")

# Survival Legibility Eleventh Wave — 15 Tasks (3 substeps each) + 3 Loops

## The 15 tasks (each with its 3 substeps)

1. **Vigil formatter.** (a) 5 keys + `MedicalVigilText`. (b) Route both panels,
   drop the English-substring severity heuristic. (c) `MedicalVigilText_IsLocalized`.
2. **MedicalPanel disease rows + buttons.** (a) 9 keys. (b) Route.
   (c) `MedicalPanel_DiseaseRowsAndButtons_AreLocalized`.
3. **AfflictionsPanel supply rows.** (a) 7 keys. (b) Route.
   (c) `AfflictionsPanel_SupplyRows_AreLocalized`.
4. **AfflictionsPanel condition names.** (a) 4 keys. (b) Route.
   (c) `AfflictionsPanel_ConditionNames_AreLocalized`.
5. **StartingCohortSetupPanel chrome + notes.** (a) 11 keys. (b) Route.
   (c) `StartingCohortSetupPanel_ChromeAndNotes_AreLocalized`.
6. **Named setup strain constant.** (a) `StartingStrainPreviewThreshold`.
   (b) Route. (c) `StartingCohortSetupPanel_UsesNamedStrainThreshold`.
7. **ShelterHud condition labels.** (a) 5 keys. (b) Route.
   (c) `ShelterHudPanel_ConditionLabels_AreLocalized`.
8. **GameDashboard directive/status.** (a) 12 keys. (b) Route.
   (c) `GameDashboardPanel_DirectiveAndStatus_AreLocalized`.
9. **Setup band-literal gate.** (a) Detect. (b) Clean.
   (c) `UiSources_DoNotRetypeSetupBandLiteral`.
10. **Curated dead-key gate.** (a) Detect. (b) Pin families.
    (c) `StringsCsv_CuratedPrefixesHaveNoDeadKey`.
11. **Registered-surface gate.** (a) Detect. (b) Register 6 panels.
    (c) `LocalizedSurfaces_RegisteredInDriftGate`.
12. **German-duplicate bound.** (a) Count. (b) Pin ≤4.
    (c) `StringsCsv_GermanDuplicates_StayBounded`.
13. **README surface registry note.
    ** (a) Document. (b) Note enforcement. (c) `CiReadme_DocumentsLocalizedSurfaceRegistry`.
14. **Staleness-only mode.** (a) Implement. (b) Document.
    (c) `GodotBoundedRunner_HasStalenessOnlyMode`.
15. **Artifact failures-field schema.** (a) Detect. (b) Pin integer.
    (c) `UiLayoutArtifact_FailureCountField`.

## Three find → repair → harden loops

1. **MedicalPanel chrome (real defect).** Four raw section-header/button
   strings (`CANCEL / RELEASE RESERVATION`, three headers) were still hardcoded.
   Repaired to 4 keys. **Hardening:** `MedicalPanel_HasNoRawChromeLiteral`.
2. **MedicalWardPanel chrome (real defect).** Thirteen raw section headers,
   empty states, and row labels were hardcoded; the ward had no localization
   helper at all. Added `Tr`/`TrFormat` and repaired to 18 keys.
   **Hardening:** `MedicalWardPanel_Chrome_IsLocalized`,
   `MedicalWardPanel_HasNoRawChromeLiteral`.
3. **Concurrent leading-whitespace fields (real defect).** Four new
   `ui.expedition.radar.*`/`ui.expedition.world_*` rows carried the separator
   inside the value (`" [PUSH]"`, `" · RUINED"`), tripping
   `StringsCsv_HasNoEdgeWhitespace`. Stripped the edge whitespace so the
   separator belongs in code, per the established precedent.
   **Hardening:** the existing edge-whitespace gate.

## Verification

- Test build: **0 warnings / 0 errors**.
- Host build (`dotnet build Ashfall.csproj`): **0 warnings / 0 errors**.
- `StatusPanelThresholdTests`: **204/204**; `NeedsDayDeltaTests`: **29/29**;
  `StringsCsvLocaleGateTests`: **4/4**; `LocalizationRatchetTests`: **2/2**.
- `python3 scripts/ci/l10n_drift_gate.py`: **PASS** (944 keys, 115 pilot
  references, 480 localized-surface references — up from 451 — German parity).
- `scripts/ci/ui-layout-check.sh`: **PASS / Failures: 0**; artifact
  `{"test":"ui_layout_selftest","status":"PASS","failures":0}`.
- `scripts/ci/run-godot-bounded.sh --check-staleness-only`: **exit 0**.
- CSV integrity: 944 rows, 0 malformed, 0 duplicates, 0 placeholder mismatch,
  0 edge whitespace, 20 identical en/de (≤20), 0 named placeholders.
- `git diff --check`: clean. No commit; full suite not run; concurrent edits
  preserved (transient Core-DLL build races and a concurrent leading-space row
  set both resolved).

## Next 15 very small tasks (each with 3 substeps)

1. Localize `MedicalWardPanel` data-row labels (`Category`, `Isolation
   Protocol`, `Occupancy State`, `OCCUPIED by`, `VACANT`, isolation states).
   (a) 6 keys. (b) Route. (c) Gate.
2. Localize `MedicalWardPanel` bed-unit/vacant/picture prose.
   (a) 4 keys. (b) Route. (c) Gate.
3. Localize `ShelterHudPanel` headers/buttons (`STORES WATCH`, `CONDITION
   REPORT`, `CURRENT DIRECTIVE`, `AIR FILTRATION`, `OPEN INVENTORY`,
   `OPEN SHELTER`, `ADVANCE TO NEXT DAY`, `SAVE LEDGER`). (a) 8 keys. (b) Route.
   (c) Gate.
4. Localize `ShelterHudPanel` filter buttons (`SERVICE FILTER`, `REPLACE HEPA`).
   (a) 2 keys. (b) Route. (c) Gate.
5. Localize `GameDashboardPanel` section headers + brand (`ASHFALL`, `HOLDFAST
   COMMAND`, `BUNKER OPERATIONS`, `WEEK ONE`, `ALL SURFACES`, `EXPANSION
   SURFACES`, `SUBSYSTEM CONSOLES`). (a) 7 keys. (b) Route. (c) Gate.
6. Localize `GameDashboardPanel` nav/action buttons. (a) 8 keys. (b) Route.
   (c) Gate.
7. Wire the concurrent `ui.expedition.radar.push_suffix` / `world_ruined` /
   `world_threats` keys into `ExpeditionRadarPanel`/`ExpeditionPanel` with the
   separator in code. (a) Route. (b) Verify. (c) Gate.
8. Localize `ExpeditionPanel` world-state line (`WORLD:`).
   (a) Key. (b) Route. (c) Gate.
9. Add `UiSources_DoNotRetypeWardBandLiteral` for the ward. (a) Detect.
   (b) Fix. (c) Verify.
10. Extend `StringsCsv_CuratedPrefixesHaveNoDeadKey` to `ui.expedition.` once
    the concurrent rows are wired. (a) Add family. (b) Fix dead. (c) Verify.
11. Add a `StringsCsv_SourceColumnExistsForRegisteredPanels` gate.
    (a) Detect. (b) Fail. (c) Verify.
12. Add a `LocalizedSurfaces_CountStaysBounded` ratchet (registered surfaces
    must not shrink silently). (a) Count. (b) Pin. (c) Verify.
13. Add a `scripts/ci/README.md` entry for the German-duplicate bound.
    (a) Document. (b) Note pin. (c) Verify.
14. Add a `run-godot-bounded.sh --print-assembly-mtime` diagnostic.
    (a) Implement. (b) Document. (c) Verify.
15. Add a `UiLayoutArtifact_FailureCountMatchesStatus` consistency gate.
    (a) Detect. (b) Assert. (c) Verify.