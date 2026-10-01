# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

> **STATUS: APPROVED BY USER** (user directive: "continue with the remaining and
> then were done!")

# Survival Legibility Thirteenth Wave — Remaining Small Panels + Chrome Ratchet

## The remaining small tasks (15 panels, then 6 registered panels)

Localized the 15 smallest remaining panels (route raw chrome through
`AshfallUiText`, add catalog keys, register with the drift gate):

1. `WeatherDetailPanel.cs` — `ui.weather_detail.no_system`.
2. `TimeCapsulePanel.cs` — `ui.time_capsule.unseal`.
3. `SurvivorRelationsPanel.cs` — `ui.relations.mediate`.
4. `KennelPanel.cs` — `ui.kennel.section.roster`.
5. `ExpansionsHubPanel.cs` — `ui.expansions.return`.
6. `CyberneticsPanel.cs` — `ui.cybernetics.section.registry`.
7. `CryogenicPermafrostCorePanel.cs` — `ui.cryo.no_samples`.
8. `BeliefsPanel.cs` — `ui.beliefs.section.doctrines`.
9. `AnomalyWatchPanel.cs` — `ui.anomaly.section.picture`.
10. `WeatherForecastPanel.cs` — `ui.weather_forecast.title` / `.close`.
11. `VinylMoralePanel.cs` — `ui.vinyl.section.selection` / `.select_album`.
12. `ShelterThermalPanel.cs` — `ui.thermal.toggle_boiler` / `.storm_seal`.
13. `SanitationPanel.cs` — `ui.sanitation.section.waste` / `.compost`.
14. `RadioIntelligencePanel.cs` — `ui.radio_intel.no_intercepts` / `.no_sos`.
15. `GameOverPanel.cs` — `ui.game_over.title` / `.new_game` / `.return_menu`.

The zero-tolerance registered-panel gate then surfaced 6 earlier-registered
panels that were still only partially localized; completed them too:

16. `ResearchPanel.cs`, `DutyRosterPanel.cs`, `SilentFoundryPanel.cs`,
    `SkillMatrixPanel.cs`, `TriangulationPanel.cs`, `DifficultySettingsPanel.cs`
    (~41 keys, reusing existing keys where present).

## Hardening

- **`RegisteredPanels_HaveNoRawChromeLiteral`** — every panel registered with
  the l10n drift gate must be chrome-clean (only the ASHFALL brand wordmark is
  exempt). A newly registered panel with a raw string fails.
- Registered `ExpeditionCampPanel.cs` after routing its `Tr` helper through
  `AshfallUiText`.
- Long-line allowlist extended with `.line` / `.estimate_line`.
- Identical en/de pin raised 20 → 21 (documented: `ui.triangulation.signal_dash`
  "Signal: —" is identical by design).

## Concurrent-churn repairs (real defects found by the gates)

- `ExpeditionCampPanel.Tr` delegated to `AshfallLocalization` directly → routed
  through `AshfallUiText`.
- `ui.expedition.radar.offline` and `ui.expedition.estimate_line` had unquoted
  commas → re-quoted.
- `ui.expedition.estimate_*` / `preview.*` rows carried the separator inside the
  value → moved into code (`" " + Tr(...)`).
- Duplicate keys and a stale read race resolved.

## Verification

- Test build: **0 warnings / 0 errors**.
- Host build (`dotnet build Ashfall.csproj`): **0 warnings / 0 errors**.
- `StatusPanelThresholdTests`: **211/211**; `NeedsDayDeltaTests`: **29/29**;
  `StringsCsvLocaleGateTests`: **4/4**; `LocalizationRatchetTests`: **2/2**;
  `ExpeditionLocaleKeysTests`: **5/5**.
- `python3 scripts/ci/l10n_drift_gate.py`: **PASS** (1183 keys, 636
  localized-surface references — up from 562 — German parity).
- `scripts/ci/ui-layout-check.sh`: **PASS / Failures: 0**.
- CSV integrity: 1183 rows, 0 malformed, 0 duplicates, 0 placeholder mismatch,
  0 edge whitespace, 20 identical en/de (≤21).
- `git diff --check`: clean. No commit; full suite not run; concurrent edits
  preserved.

## Remaining

The bulk of the remaining localization is still a broad multi-panel sweep
(~150 files, largest: `FactionsPanel`, `GreenhousePanel`, `ShelterBarterPanel`).
It is a larger program, not a set of "very small tasks". The per-panel chrome
gate is now the ratchet that keeps each one clean once localized.