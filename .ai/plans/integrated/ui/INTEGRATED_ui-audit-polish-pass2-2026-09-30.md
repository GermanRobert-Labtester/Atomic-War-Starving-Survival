# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
>
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
>
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# UI AUDIT POLISH — PASS 2 (gap sweep, repair, and inventory-asset wiring)

> **STATUS: APPROVED BY USER** (user directive 2026-09-30: "Please address all
> the gaps and go for another pass for repair!")

Continuation of `claim-ui-audit-polish-settings-dev-session-2026-09-30`.
Bounded second pass over every gap the first audit surfaced but did not close.

## Pass 2 gap sweep (evidence found before editing)

| Finding | Evidence | Verdict |
|---|---|---|
| 13 `private ... = null!;` controls declared and **never constructed** | `grep` occurrence count = 1 (declaration only) per field; each class has exactly one file (no partial split); global `_contentVBox` search found no cross-file use | **Dead code / unfinished refactor — removed** |
| `_radonLabel` declared in `GameDashboardPanel` but never built | `GameDashboardPanel.cs:88` vs 0 assignments | **Missing UI — built** |
| Air quality + radon fused into one unreadable string | `_airQualityValue.Text = "AIR QUALITY: {…}% · RADON: {…} Bq/m³ [STABLE]"` | **UI correction — split into two rows** |
| Radon had no threshold semantics | Authoritative thresholds exist and are `public const` in `YearOfAshRadonSystem` (`SafeRadonThreshold = 200f`, `DangerousRadonThreshold = 800f`) | **Bound to the owner's constants — no invented numbers** |
| "RESET TUTORIALS" button asserted success it had not achieved | Button set its own text to `TUTORIALS RESET` *before* the confirmation modal resolved; cancelling still left the false claim | **Real bug — removed the false claim** |
| `EnabledMods` had a toggle but no visibility | `UserSettingsData.enabled_mods` list rendered nowhere | **Missing readout — added** |
| Inspector had no way to get the missing-art set *out* | Only an on-screen filter | **Missing dev affordance — export added** |
| Value-change pulse only covered `AshfallMetricCard` | Plain readout labels (dashboard stores) snapped | **Central seam added** |
| Item art absent from barter + relic rows | `ShelterBarterPanel` offer rows, `PhantomMemoryPanel` relic rows | **Wired** |
| `UiPanelFlow.Pulse` scaled from top-left | No `PivotOffset` set | **Polish — centre pivot** |

## Owned paths (exact, Pass 2)

`src/UI/UiPanelFlow.cs`, `src/UI/AshfallUiHelpers.cs`, `src/UI/GameDashboardPanel.cs`,
`src/UI/SettingsPanel.cs`, `src/UI/AssetInspectorPanel.cs`, `src/UI/ShelterBarterPanel.cs`,
`src/UI/PhantomMemoryPanel.cs`, and one-line dead-field removal in
`src/UI/AchievementsPanel.cs`, `src/UI/CombatDetailPanel.cs`,
`src/UI/DutyRosterDetailPanel.cs`, `src/UI/EventDetailPanel.cs`,
`src/UI/EventsLogPanel.cs`, `src/UI/ExpansionsHubPanel.cs`,
`src/UI/JournalDetailPanel.cs`, `src/UI/RadiationDetailPanel.cs`,
`src/UI/RadiationHistoryPanel.cs`, `src/UI/SaveLoadPanel.cs`,
`src/UI/SurvivalDetailPanel.cs`, `src/UI/TutorialPanel.cs`.

## Non-goals honored

No Core change (only *read* of an existing `public const` on the radon owner),
no data JSON change, no new save section, no new gameplay owner, no commit, no
full suite. Pre-existing dirty worktree preserved untouched
(`src/Host/HoldfastRuntimeSession.cs`, `src/Host/HostCli.cs`,
`src/Main.Application.cs`, `src/Main.Cooking.cs`, `src/UI/GameDashboardPanel.cs`
foreign WEEK-ONE nav edits preserved within the same file, data JSON, sprite
PNGs, `.ai/state.md` Batch 05).

## Verification (Pass 2)

```
dotnet build Ashfall.csproj                                     0 errors / 6 pre-existing warnings (untouched HostCli.*)
bin/run-scoped-tests UserSettingsRecoveryTests CraftingSystemTests InventorySystemTests   3/3 suites PASS
godot --headless -- --settings-selftest                         Failures: 0
godot --headless -- --dashboard-uitest                          PASS (shell=True rootOverlay=True inventory=True liveSources=True)
godot --headless -- --ui-accessibility-selftest                 PASS (5/5, 490 text elements, 267 UI files)
godot --headless -- --player-panels-uitest                      PASS (bind lifecycle 22/22)
godot --headless -- --barter-selftest                           PASS (7/7 checks)
godot --headless -- --economy-uitest                            PASS
git diff --check                                                clean
```

Full suite not run (per `TEST_POLICY.md`).
