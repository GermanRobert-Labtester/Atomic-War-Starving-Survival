# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
>
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
>
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# UI AUDIT POLISH — PASS 3 (dead readouts, grid icon wiring, a11y tooltips)

> **STATUS: APPROVED BY USER** (user directive 2026-09-30: "another pass!")

Third bounded pass. Continuation of `claim-ui-audit-polish-pass2-2026-09-30`.

## Pass 3 gap sweep (evidence found before editing)

| Finding | Evidence | Verdict |
|---|---|---|
| `InventoryPanel._weightLabel` built once as `"CAPACITY // —"` and **never written again** | `grep` showed 4 refs, 0 assignments; whole-UI scan for "declared, referenced, never written" labels | **Real dead readout — wired** |
| `AshfallDataGrid.Cell.IconTexture` already existed and was **already rendered** by `MakeCellControl`, but no caller ever populated it | `AshfallDataGrid.cs:59` field + `:422` render vs 0 assignment sites | **Wiring gap, not a component gap** |
| Item art resolvable through 3 divergent chains | `MakeItemIcon` chain inline; grid cells had none | **Shared `ResolveItemTexture` seam added** |
| Symbol-only buttons carried no accessible name | `✕` (SettingsPanel), `×` (FeedbackPanel), `-`/`+` volume, `-`/`+` barter counters, `[---]` bind | **Tooltips added** |
| Settings resolution dropdown **lied about saved state** | `RefreshControls` defaulted `resIndex = 3` (1920×1080); a saved non-preset resolution was relabelled as the default while the working copy kept the custom size | **Real bug — `Custom (W × H)` entry added** |
| `EconomyDetailPanel` market demand rows text-only | `AddRow(_marketList, …)` with `d.itemId` available | **Item art wired** |
| Latent UI selftest failures | Ran the wider battery (`panel-lifecycle`, `panel-bind`, `ui-layout`, `accessibility-settings`) | **All green — no latent failures** |

## Owned paths (exact, Pass 3)

`src/UI/AshfallUiHelpers.cs`, `src/UI/InventoryPanel.cs`,
`src/UI/SurvivalWorkstationPanel.cs`, `src/UI/EconomyDetailPanel.cs`,
`src/UI/SettingsPanel.cs`, `src/UI/ShelterBarterPanel.cs`,
`src/UI/FeedbackPanel.cs`.

## Delivered

- **Dead readout repaired** — `InventoryPanel` capacity line now tracks live
  weight vs carry limit with the same criticality ladder as the status rail, and
  pulses on change through the shared seam.
- **Item art through one chain** — new `AshfallUiHelpers.ResolveItemTexture`
  returns the raw `Texture2D?` from the canonical registry chain; `MakeItemIcon`
  now delegates to it. Populated `AshfallDataGrid.Cell.IconTexture` for
  `SurvivalWorkstationPanel` recipe outputs (the grid already rendered icons —
  they were simply never supplied).
- **Economy item rows** — `EconomyDetailPanel.AddItemRow` variant leads market
  demand rows with resolved good art.
- **a11y tooltips** on every symbol-only button touched in Passes 1–3.
- **Settings resolution truthfulness** — a saved resolution outside the preset
  list now appears as `Custom (W × H)` instead of being silently relabelled as
  1920×1080.

## Non-goals honored

No Core change, no data JSON change, no new save section, no new gameplay
owner, no shared-component behavior change (`AshfallDataGrid` untouched — only
its existing `IconTexture` field is now populated), no commit, no full suite.

## Verification (Pass 3)

```
dotnet build Ashfall.csproj                  0 errors / 6 pre-existing warnings (untouched HostCli.*)
bin/run-scoped-tests UserSettingsRecoveryTests InventorySystemTests   2/2 suites PASS
--inventory-uitest                           PASS
--economy-uitest                             PASS
--settings-selftest                          Failures: 0
--dashboard-uitest                           PASS
--ui-accessibility-selftest                  PASS (5/5)
--player-panels-uitest                       PASS (bind lifecycle 22/22)
--panel-lifecycle-selftest / --panel-bind-selftest / --ui-layout-selftest / --accessibility-settings-selftest   PASS (12/12)
git diff --check                             clean
```

Whole-UI scan for "declared, referenced, never written" labels now returns
zero. Full suite not run (per `TEST_POLICY.md`).
