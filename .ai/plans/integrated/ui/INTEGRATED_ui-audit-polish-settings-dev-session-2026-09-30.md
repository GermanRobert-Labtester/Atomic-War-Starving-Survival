# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
>
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
>
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# UI AUDIT POLISH — SETTINGS COMPLETENESS + INVENTORY ASSET WIRING + DEV SESSION

> **STATUS: APPROVED BY USER** (user request 2026-09-30: "audit the UI, diagnose and
> repair with UI correction, UI functionality, UI animations polishing … adding UI
> where its missing … settings menu functionality … wiring in inventory assets and
> building a start game option which is a developer session where i can check all
> items and their assets and etc")

## Bounded outcome

Close four evidence-backed UI gaps without creating any new gameplay authority,
save section, or data owner. Presentation-only + one existing settings contract.

## Premise audit (verified in source before editing)

| Finding | Evidence | Verdict |
|---|---|---|
| `AutoSaveOnDay` persisted setting has **zero UI** | `Assets/Ashfall.Core/Settings/UserSettingsData.cs` `auto_save_on_day`; `src/UI/SettingsPanel.cs:79` declares `private Button _btnAutoSave = null!;` and it is **never assigned** anywhere in the file | REAL GAP |
| `VisualAudioAlerts` (Plan 169) persisted, **zero UI** | `UserSettingsData.visual_audio_alerts`; no `_working.VisualAudioAlerts` reference in `SettingsPanel.cs` | REAL GAP |
| `AudioMixPreset` (Plan 169) persisted + applied (`Main.AudioAccessibility.cs:37`, `AudioManager.ApplyAccessibilityMixPreset`), **zero UI selector** | `UserSettingsData.audio_mix_preset`; no control in `SettingsPanel.cs` | REAL GAP |
| `ModsEnabled` persisted, **zero UI** | `UserSettingsData.mods_enabled`; no control in `SettingsPanel.cs` | REAL GAP |
| Item icon fallback resolves to a **wrong, domain-specific sprite** | `AshfallUiHelpers.MakeItemIcon` final fallback `res://assets/ui/Icons/icon_pill_dependency.svg` instead of `AssetRegistry.FallbackIconPath` (`assets/ui/Icons/icon_placeholder.png`) | REAL BUG |
| Dev session card shows id/name/kind/path/status but **no item stats** | `AssetInspectorPanel.BuildCard` | GAP vs "check all items … and etc" |
| No unsaved-changes affordance in Settings | `SettingsPanel` header has title + ✕ only | POLISH GAP |
| Settings load/save recovery is silent | `UserSettingsStore.LastDiagnosticMessage` / `HasDiagnosticError` exist but are never surfaced in the panel | SILENT-FAILURE GAP |
| Panel open/close animation seam | Centralized: `Main.PanelLifecycle.ShowPanelLifecycle` → `UiMotion.AnimateOpen`, `ClosePanelAnimated` → `UiMotion.AnimateClose`; value-change pulse already in `AshfallMetricCard.SetValue` | NOT A GAP — do not duplicate |

## Owned paths (exact)

- `src/UI/SettingsPanel.cs` — settings completeness, dirty indicator, diagnostic banner
- `src/UI/AshfallUiHelpers.cs` — `MakeItemIcon` canonical fallback only
- `src/UI/AssetInspectorPanel.cs` — item stat rows + grid transition polish
- `src/UI/MainMenuPanel.cs` — `START GAME — DEV SESSION` entry (start run + open inspector)
- `src/Main.UiPanels.cs` — wire the new dev-session event only
- `.ai/state.md` + this plan + `WORKTREE_OWNERSHIP.md` claim entry

Read-only / untouched: all Core, data JSON, save sections, `AssetRegistry`,
`UserSettingsData`/`UserSettingsCodec`/`UserSettingsStore`, `UiMotion`,
`Main.PanelLifecycle`, every other panel.

## Non-goals

No new save section, no new gameplay owner, no Core change, no data JSON change,
no localization freeze break (new strings are dev/settings labels matching the
existing hardcoded-label convention in `SettingsPanel`), no commit, no full suite.

## Acceptance

1. `dotnet build Ashfall.csproj` 0 errors. **PASS** (0 errors / 6 pre-existing
   warnings, all in untouched `src/Host/HostCli.*` files).
2. `bin/run-scoped-tests Ashfall.Core.Tests/Settings/UserSettingsRecoveryTests.cs`
   **PASS 20/20** (settings contract untouched).
3. `godot --headless -- --settings-selftest` **PASS** (Failures: 0).
4. `godot --headless -- --inventory-uitest` **PASS** (16/16 checks).
5. Scoped `git diff --check` clean on owned paths. **PASS**.

## Additional gates run

- `godot --headless -- --player-panels-uitest` **PASS** (panel bind lifecycle
  22/22, player panels all True).
- `godot --headless -- --ui-accessibility-selftest` **PASS** (5/5, static scan
  over 267 UI files).
- `bin/run-scoped-tests Ashfall.Core.Tests/CraftingSystemTests.cs
  Ashfall.Core.Tests/InventorySystemTests.cs` **PASS** (2/2 suites).

## Delivered (exact)

**Settings menu functionality** (`src/UI/SettingsPanel.cs`) — four persisted
settings that previously had **no UI at all** are now controllable:
`auto_save_on_day` (the dead `_btnAutoSave` field is finally constructed and
wired), `visual_audio_alerts` (Plan 169), `audio_mix_preset` (Plan 169, three
authored presets forwarded to the existing AudioAccessibility owner), and
`mods_enabled`. Added an `● UNSAVED CHANGES` header indicator driven by a codec
round-trip diff of the working copy vs the loaded baseline (additive to future
settings fields, no per-control wiring drift), and a `SETTINGS RECOVERY`
banner that surfaces `UserSettingsStore.LastDiagnosticMessage` so corrupt-file
recovery/clamping is no longer a silent log-only failure.

**Inventory asset wiring** (`src/UI/AshfallUiHelpers.cs`,
`src/UI/InventoryDetailPanel.cs`, `src/UI/CraftingPanel.cs`) —
`MakeItemIcon`'s final fallback was `icon_pill_dependency.svg`, a
medical-domain sprite that made every un-arted item look like a pill; it now
falls back to the canonical `AssetRegistry.FallbackIconPath`. The item detail
view was **text-only** and now leads with the item's resolved art (72px,
registry path as tooltip). Crafting recipe cards now carry the output item's
art in the header and per-ingredient art in each ingredient row.

**UI animations polishing** (`src/UI/SettingsPanel.cs`,
`src/UI/AssetInspectorPanel.cs`) — the shared value-change pulse seam
(`UiPanelFlow.Pulse`) is now applied to settings volume readouts and the
inspector coverage summary. Panel open/close animation was audited and found
already centralized (`Main.PanelLifecycle` → `UiMotion`); deliberately **not**
duplicated per-panel.

**Developer session / start game** (`src/UI/MainMenuPanel.cs`,
`src/Main.UiPanels.cs`, `src/UI/AssetInspectorPanel.cs`) — new
`START GAME — DEV SESSION (RUN + INSPECT)` main-menu entry that boots a fresh
campaign through the canonical `StartNewGame()` command and then opens the
read-only Item & Asset Inspector over it (pure composition of two existing
commands). The inspector cards now show authored item stats read straight from
the authoritative catalog row (weight, value, stack, rad protection,
durability) so every item can be checked with its asset and its numbers.

## Non-goals honored

No Core change, no data JSON change, no new save section, no new gameplay
owner, no commit, no full suite. Pre-existing dirty worktree files
(`src/Host/HoldfastRuntimeSession.cs`, `src/Host/HostCli.cs`,
`src/Main.Application.cs`, `src/Main.Cooking.cs`, data JSON, sprite PNGs) were
left untouched.
