# Plan P1D-Residual: UI Controller Parity & Close/Back Sweep

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

- The 4 claimed files keep raw Esc until their claim closes; the ratchet
  allowlist names them with reasons, so conversion is a one-line follow-up.
- Joypad close on `SettingsPanel`'s own handler is still key-gated upstream of
  the rebind-capture logic; pad B falls through to Main's central
  `IsCloseOrCancel` dismissal, which covers it.
- `DutyRosterPanel.cs:400` SHIFT DETAIL non-wrapping metadata (audit finding,
  same class as C15) remains blocked by claim-c1-plan24 — not fixed here.
- Runtime pad-in-hand confirmation was not performed (no interactive input
  automation); behavior is proven by the InputMap bindings + predicate wiring
  + selftests above.
