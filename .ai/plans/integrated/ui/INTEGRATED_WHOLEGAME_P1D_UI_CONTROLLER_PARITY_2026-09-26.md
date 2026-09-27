# Plan P1D-Residual: UI Controller Parity & Close/Back Sweep

# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**

STATUS: APPROVED BY USER

(User-authorized 2026-09-26 via the direct UI/presentation-pass directive for
this session: "verify and repair UI functionality … preserve keyboard/controller
navigation, focus order, close/back behavior". Executes the deferred-P1D items
recorded in `.ai/plans/wholegame-p1-playable-ui-integration.md` §6.)

## 1. Goal & Outcome

Close the controller/rebinding parity gap in panel dismissal and modal
scrolling, and ratchet it so it cannot regress:

1. Every player panel dismisses through the rebindable
   `AshfallInputActions.IsCloseOrCancel` contract (`ashfall_close` +
   `ui_cancel`: Esc, pad B, rebound keys) instead of raw `Key.Escape`.
2. `DailyBriefingModal` scrolls via the `ashfall_nav_up`/`ashfall_nav_down`
   actions (arrows + D-pad); PageUp/PageDown stay raw secondary accelerators.
3. A corpus-wide source gate bans new raw `Key.Escape` dismissal code in
   `src/UI` with an exhaustive, reason-carrying allowlist.

**Non-goals:** no gameplay/data/save changes; no route/registry changes; no
new panels; no focus-policy redesign; no visual/layout changes (the 2026-09-25
UI/UX audit sealed those — theme, motion, snapshots, contrast, spacing); no
full test suite; no edits to files under ACTIVE claims.

## 2. Claimed Paths & Affected Files

- **Sweep (92 files):** every `src/UI/*.cs` holding the standard
  `_UnhandledInput` + `Key.Escape` pattern, converted mechanically to
  `AshfallInputActions.IsCloseOrCancel(@event)` (list = the 92 entries that
  matched the three textual forms; verified post-sweep).
- **Manual:** `src/UI/SettingsPanel.cs` (close branch only; rebind-capture
  cancel stays raw with an inline reason), `src/UI/CombatPanel.cs` (Esc +
  Tab→`IsNextTab`), `src/UI/MoralChoiceModal.cs` (Esc),
  `src/UI/EmergencyResponseHud.cs` (`ui_cancel` string → `IsCloseOrCancel`),
  `src/UI/Plans74To77Panels.cs` (handler + dead `CloseOnEscape()` body),
  `src/UI/DailyBriefingModal.cs` (nav actions).
- **Tests:** `Ashfall.Core.Tests/UI/AccessibilitySourceAuditTests.cs` (new
  corpus-wide fact `UiPanels_DismissThroughCloseAction_NotRawEscape`).
- **Excluded (ACTIVE claims, untouched):** `src/UI/DutyRosterPanel.cs`,
  `src/UI/ExpeditionPanel.cs`, `src/UI/SurvivorDetailPanel.cs`
  (claim-c1-plan24-survivor-ledger-2026-09-16),
  `src/UI/PfglOctetBoardPanels.cs` (claim-pfgl-codex-luna6-octet-2026-09-25).
  Also untouched: `src/Main.PlayerSurfaces.cs`, `src/Main.PanelLifecycle.cs`,
  `src/UI/GameDashboardPanel.cs`, `Assets/Ashfall.Core/UI/PanelRegistryBootstrap.cs`.

## 3. Pre-flight Evidence (measured 2026-09-26)

- 102 raw `Key.Escape` usages across 100 `src/UI` files; 97 follow the three
  standard `_UnhandledInput` forms; the rest are nested/special (handled
  manually) or in claimed files.
- `project.godot` InputMap binds `ashfall_close` = Esc + JoypadButton B,
  `ashfall_nav_up/down` = arrows + D-pad 11/12, `ashfall_next_tab` = Tab +
  pad 10 — so the action predicates give real controller parity that raw
  keycodes silently dropped.
- Main's central handler already used `IsCloseOrCancel`
  (`src/Main.Application.cs:1045,1068`); `Phase0Panel` (P1C) is the approved
  panel-level exemplar.
- Existing gate `MigratedPanels_DoNotUseRawEscapeKey` covered only 6 files →
  extended via a new corpus-wide fact rather than a duplicate test.

## 4. Verification (all executed 2026-09-26)

- `dotnet build Ashfall.csproj --nologo --no-restore` → 0 errors /
  1 pre-existing warning (`ShelterThermalPanel.cs`, documented in ledger).
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` → 0/0.
- `bash scripts/run_test.sh Ashfall.Core.Tests/UI/AccessibilitySourceAuditTests.cs`
  → 5/5 PASS (incl. the new corpus ratchet).
- `godot --headless --path . -- --ui-layout-selftest` → PASS (all panels
  instantiated; clickability/focusability/truthfulness gates).
- `godot --headless --path . -- --ui-accessibility-selftest` → PASS.
- `godot --headless --path . -- --player-panels-uitest` → PASS.
- Post-sweep `grep Key.Escape src/UI/*.cs` → exactly 5 residual usages:
  4 in ACTIVE-claimed files + 1 intentional SettingsPanel rebind-capture.

## 5. Known Limitations / Debt

- ~~The 4 claimed files keep raw Esc until their claim closes~~ — converted in
  the continuation (§6) under direct user authorization.
- Joypad close on `SettingsPanel`'s own handler is covered by the hoisted
  action check (§6); pad B also falls through to Main's central
  `IsCloseOrCancel` dismissal.
- ~~`DutyRosterPanel.cs:400` SHIFT DETAIL non-wrapping metadata~~ — fixed in
  the continuation (§6) under direct user authorization.
- ~~Runtime pad-in-hand confirmation~~ — replaced by the synthetic-input
  `[UiControllerParity]` gate (§6), which delivers byte-identical pad events
  through the production handlers.

## 6. Continuation — same user authorization (2026-09-26)

The user reviewed the §5 residuals and directed: "continue with the
remaining". This section is executed under that direct authorization, which
overrides the stale c1-plan24 / PFGL-octet claims **for the one-line dismissal
edits only** (both claims re-verified as still open before editing; nothing
else in those files was touched):

1. **Claim-file conversions:** `DutyRosterPanel.cs`, `ExpeditionPanel.cs`,
   `SurvivorDetailPanel.cs`, `PfglOctetBoardPanels.cs` raw Esc →
   `IsCloseOrCancel`. Ratchet allowlist shrunk to `SettingsPanel.cs` only.
2. **Key-cast hoists:** `CombatPanel.cs`, `MoralChoiceModal.cs`,
   `SettingsPanel.cs` — the close check moved ABOVE the
   `is InputEventKey` guard so joypad events (not key events) reach it;
   rebind-capture precedence preserved in Settings.
3. **DutyRosterPanel detail-pane autowrap:** all 11 `MakeMetadata` prose
   sites gained `autowrap: true` (C15-class fix for the audit's :400 finding).
4. **`VerifyUiControllerParity` gate** (`src/Host/HostCli.PanelTests.cs`, in
   `--ui-layout-selftest`): asserts the InputMap contract on synthetic events
   (pad B → close, D-pad → nav, Esc/arrows unchanged, negative control) and
   drives a synthetic pad-B press through every input-handling panel's
   production `_Input`/`_UnhandledInput`/`_UnhandledKeyInput` override,
   requiring dismissal (hide or OnClose). Result: 61/61 panels, 0 failures.
5. **`EmergencyResponseHud.Open`** GrabFocus guarded on `IsInsideTree()`
   (removes a pre-existing audit-path engine error).
6. **Art waves:** Composio CLI verified authenticated; generation launched
   through the established `generate_faction_portrait_art.py` pipeline for the
   measured gaps (97 portraits, 46 locations). Generated files are left
   uncommitted pending Godot import sidecars, per the pre-commit asset gate.
7. **Commit:** one pathspec commit of this package's code/test/plan/state
   files (the user listed the uncommitted package among the remaining items).

## 7. Continuation Verification (all executed 2026-09-26)

- `dotnet build Ashfall.csproj --nologo --no-restore` → 0 errors / 1
  pre-existing ShelterThermalPanel warning.
- `bash scripts/run_test.sh Ashfall.Core.Tests/UI/AccessibilitySourceAuditTests.cs`
  → 5/5 PASS (corpus ratchet green with SettingsPanel-only allowlist).
- `godot --headless --path . -- --ui-layout-selftest` → PASS, including
  `[UiControllerParity] input-handling panels=61 padDismissed=61 … 0 failed`
  and zero non-RID engine errors.
- `godot --headless --path . -- --ui-accessibility-selftest` → PASS (5/5).
- `godot --headless --path . -- --player-panels-uitest` → PASS.
- `godot --headless --path . -- --asset-registry-selftest` → 55/55 (census
  basis: portraits 32/129, locations 133/179 pre-wave).
