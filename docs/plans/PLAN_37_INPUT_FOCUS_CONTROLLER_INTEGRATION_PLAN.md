# Plan 37 — Hands On The Wheel: Input, Focus & Controller Reality
## Integration Plan (package `C2[15]` / `PLAN37-INPUT-FOCUS-CONTROLLER`)

**Status:** PLAN — not yet authorized for implementation. Phase 0 (the premise
audit) is executed by this document; its findings are recorded in §2 and §4.
**Source document:** `Next-steps-plans/shipped_to_chat/Plan_37_Hands_On_The_Wheel_Input_Focus_Controller.md`
(verified against `ccac926e`; re-verified here against current `Zcode_Branch` HEAD).
**Promotion record:** `docs/plans/UNBLOCKED_PLANS_AUDIT_2026-09-19.md` row 7 —
prerequisites `C2[9]`/Plan 28 (RETIRED/sealed 2026-09-18) and `C1[8]`/Plan 31
(DONE 2026-09-17) both resolved; entry gate is "standard premise audit first".
This document IS that premise audit plus the integration design.
**Date of evidence:** all file:line citations below were re-read on the current
worktree, 2026-09-19 session state.

---

# 1. Objective

Make ASHFALL's declared input layer true end-to-end on the fixed 1920×1080
desktop canvas:

1. **Every declared action does something, once.** 22 `ashfall_*` actions exist
   in `project.godot [input]`; the four `ashfall_nav_*` directionals and the
   `IsConfirmOrAccept` predicate are currently dead, and the joypad registration
   code path is unreachable dead code. Close the gap between "declared",
   "handled", and "visible to the player", then gate it in CI so it cannot
   drift apart again.
2. **A player who never touches the mouse can complete a full campaign day** on
   the ten highest-traffic surfaces, with deterministic initial focus, visible
   focus indication, focus trapping inside overlays, and focus restoration on
   close — by *wiring the already-existing* Plan 80 infrastructure
   (`AshfallFocusPolicy`, Core `ModalStackController`) instead of inventing a
   second focus system.
3. **Bounded gamepad parity + user rebinding.** Joypad bindings are authored
   into `project.godot` for a deliberately bounded action subset; a rebinding
   section lands in the existing `SettingsPanel` persisting through the
   existing `UserSettingsStore`/`UserSettingsCodec` authority (schema v1→v2);
   the guidance overlay renders *live* bindings through the existing
   `GetActionPrompt` seam.

Non-goals (explicit, from the source plan's own guardrails and the signed
Plan 184 map §3.2): no new input framework, no per-panel input sniffing left
standing where a shared owner exists, no parallel action vocabulary, no radial
menus or cursor-emulation gameplay rethink, no screen-reader/AT claims (Godot
4.x limitation, Plan 80 §7 remains authoritative), no absorption of Plan 184
colorblind/contrast preference territory, no gameplay rebinding of
simulation authority.

---

# 2. Current Reality

This section is the premise audit. Every row states whether the source plan's
claim **still holds**, was **resolved by later waves**, or is **corrected by
stronger current evidence**. File:line references are current.

## 2.1 Input action surface (verified against `project.godot` + `src/Host/AshfallInputActions.cs`)

| Source-plan claim (Evidence Inventory row) | Verdict today | Current evidence |
|---|---|---|
| #1 "21 actions defined" | **Stale count — now 22** | `project.godot [input]` lines 43–155 define 22 actions; `ashfall_guidance` (F2) was added by Plan 17B Phase D (`src/Host/AshfallInputActions.cs:35-38`, documented F1-collision note). |
| #2 "typed wrapper exists (good)" | **True, and extended** | `AshfallInputActions.cs` now also carries `CanonicalDefaults`, `EnsureActionsRegistered()`, `ReconcileCollisions()`, `GetJournalTabNumber`, and `GetActionPrompt` (dynamic `[Key]` prompt strings, `:303-333`). |
| #3 "3 predicates never called (`IsConfirmOrAccept`, `IsExpeditions`, `IsHoldfast`)" | **Partially stale — 1 orphan remains** | `IsExpeditions` is called at `src/Main.Application.cs:756`; `IsHoldfast` at `:751`. `IsConfirmOrAccept` (`AshfallInputActions.cs:229-232`) still has **zero** call sites (grep-verified across `src/`). |
| #4 "coverage thin, single dispatch around `Main.GameFlow.cs:667`" | **Stale location, worse shape** | Global hotkey dispatch now lives in `Main.Application.cs:713` (`_UnhandledKeyInput`, 10 hotkey branches, all gated on `GameState.Playing`). A **second** global close dispatcher exists at `Main.GameFlow.cs:858` (`_UnhandledInput`: Esc → `CloseAllOverlayPanels()` → else `ReturnToMenu()`). Plus a third, dormant one: `ModalManager.HandleInput` (`src/UI/ModalManager.cs:132`) with **zero callers**. |
| #5 "directional nav actions have no handler" | **Confirmed** | `NavUp/NavDown/NavLeft/NavRight` constants are referenced only inside `AshfallInputActions.cs` itself (registration lists, `:22-25,57-60,83-86,113-116`). No `IsNav*` predicates exist. Zero consumers anywhere in `src/`. |
| #6 "focus grabbed in two places (`MainMenuPanel`, `ModalManager`)" | **Stale — worse and better** | `GrabFocus` now appears in 7 files / 11 sites: `MainMenuPanel.cs:64,68`, `ModalManager.cs:196,205,214`, `StartingCohortSetupPanel.cs:240`, `ExpeditionPanel.cs:1241,1247`, `EmergencyResponseHud.cs:253`, `ConfirmationModal.cs:101`, `MoralChoiceModal.cs:274`. Ad hoc, no shared policy invocation. Meanwhile the Plan 80 authority `src/UI/AshfallFocusPolicy.cs` (`OpenWithFocus`, `TrapFocus`, `RestoreFocus`, `ApplyFocusVisibleStyle`, `FindFocusableControls`) has **zero production call sites** — grep-verified; only its own definitions match. |
| #7 "no controller support (0 `InputEventJoypad` in `project.godot`)" | **Confirmed, with a sharper finding** | `project.godot` still has zero joypad events. Additionally: `EnsureActionsRegistered()` — the only code that would add `JoyButton` events (`:110-118`) — is **never called** (zero call sites repo-wide outside its own definition), and even if called, `RegisterAction` (`:183-216`) only adds events when the action has *no* key event, which `project.godot` already provides for all 22 actions. The joypad branch is unreachable by construction. Controller support is exactly zero at runtime. |
| #8 "rebinding has no surface" | **Confirmed** | `src/UI/SettingsPanel.cs` exposes Display / Audio / Accessibility & Language / Gameplay sections only — no input section. `Assets/Ashfall.Core/Settings/UserSettingsData.cs` (`schema_version` 1) has no key-binding field. |
| #9 "text scale is per-widget constants; no user lever" | **Mostly resolved by Plan 184 Path α/β** | `UserSettingsStore.Apply` (`src/Settings/UserSettings.cs:180-210`) applies a central `ContentScaleFactor` computed by `AccessibilityPresentation.ResolveContentScaleFactor` (UiScale ×1.15 large-fonts ×1.05 high-contrast, clamped [0.5, 2.5]); `SettingsPanel` exposes "Interface Scale" 0.8×–1.5× plus Large Fonts / High Contrast toggles. Per-panel `AddThemeFontSizeOverride` sprinkles still exist but are no longer the only lever. Plan 37 must not build a second scale system; only the re-verification of overflow at scale variants remains ours (§18, §22). |
| #10 "fixed-viewport scaling 1920×1080, keep_height" | **Confirmed (constraint)** | `project.godot [display]`: viewport 1920×1080, `stretch/mode="canvas_items"`, `aspect="keep_height"`, `scale_mode="fractional"`, `window/size/mode=4` (headless-compatible). DESIGN.md confirms fixed desktop layout. |
| #11 "guidance overlay unrouteable; `ashfall_help` opens nothing" | **Resolved by later waves** | `Main.Application.cs:735` routes `IsHelp` → `OpenPlayerPanel("help")` → `_tutorialPanel`; `:740-747` toggles the `guidance` route (`PanelRegistry.TryClose` / `OpenPlayerPanel`) on F2. Both routes are registered in `PanelRegistryBootstrap.cs:17-18`. |
| #12 "audio/settings recovery documented but manual" | **Confirmed** | `docs/qa/AUDIO_AND_SETTINGS_RECOVERY_SMOKE_TEST.md` is a manual checklist (E1–E8); no input/rebinding coverage and no automated gate. |

## 2.2 Structural reality the source plan did not know

These are the load-bearing findings for the design; all grep-verified.

1. **A tested, engine-free modal stack already exists in Core and is unused.**
   `Assets/Ashfall.Core/UI/ModalStackController.cs` (`ModalStackController<TModal, TFocus>`:
   LIFO push/pop, self-close tracking, prior-focus retention, `ModalOpened`/
   `ModalClosed` events) is covered by `Ashfall.Core.Tests/UI/ModalStackControllerTests.cs`
   (7+ cases) and referenced by **nothing** in `src/`. The host instead has a
   parallel Godot-side `src/UI/ModalManager.cs` (same concepts, plus focus
   grab/restore) that is instantiated only inside `src/Audio/AudioSelfTest.cs:943`
   as a smoke specimen. `ModalManager.HandleInput` has zero callers.
2. **The live modal path is hand-rolled per modal.** `DailyBriefingModal`,
   `ConfirmationModal`, `MoralChoiceModal`, `OpeningProtocolModal`,
   `SafeCrackModal` each override `_UnhandledInput`, sniff close/confirm, and
   grab focus locally. There is no stack: two modals open at once have no
   defined LIFO order beyond "last one to handle input wins".
3. **The close path is triple-implemented and inconsistent.** (a) Global:
   `Main.Application._UnhandledKeyInput` close branch + `Main.GameFlow._UnhandledInput`
   Esc branch. (b) Per-panel: ~114 files under `src/` override `_UnhandledInput`
   or `_UnhandledKeyInput`. Within those, three patterns coexist: the canonical
   predicate (`IsCloseOrCancel` — 12 verified call sites incl. `CraftingPanel:447`,
   `StatusPanel:354`, `InventoryPanel:260`, `MedicalPanel:913`, `ShelterBarterPanel:444`,
   `ResearchPanel:446`, `ResearchAtlasPanel:471`, `DailyBriefingModal:154`,
   `GameOverPanel:111`, `HoldfastTerminalPanel:465`, `ModalManager:136`,
   `Main.Application:766,783`), raw `Key.Escape` comparisons (verified:
   `SurvivorsPanel.cs:308-317`, `SettingsPanel.cs:87-98`, `OnboardingHintPanel.cs:243-252`),
   and panels with **no local close handling at all** (verified: the brand-new
   `VehicleGaragePanel.cs`, 520 lines, zero `_UnhandledInput`/`GrabFocus`/`FocusMode`).
4. **Overlay teardown ignores focus entirely.** `CloseAllOverlayPanels`
   (`src/Main.PanelLifecycle.cs:10-124`) is a hand-maintained array of ~120
   panel fields set `Visible = false`; no focus capture on open, no focus
   restore on close, no close-event fan-out. If a hidden control held keyboard
   focus, focus is dropped to nowhere (Godot clears focus on hide for the
   focused control, leaving *no* focus owner — keyboard navigation is then dead
   until the next click).
5. **Documentation drift is already present and must be corrected, not
   propagated.** `docs/ui/INPUT_AND_NAVIGATION_AUDIT.md` §1 asserts "Every
   action maps to primary keyboard, secondary keyboard, and Joypad gamepad
   inputs" — false at runtime (§2.1 row #7); its table also omits
   `ashfall_guidance`. `docs/ACCESSIBILITY.md` §2 describes focus open/trap/
   restore as "Implemented & CI-Gated" — the class and the CI lint exist
   (`UiAccessibilitySelfTest` Gate 1 enforces `FocusMode != None` on
   interactive controls; `AccessibilitySourceAuditTests` pins source rules),
   but the open/trap/restore *helpers are not wired to any panel*. Both docs
   need a truthfulness pass as part of this package (bounded: correct the
   claims, don't rewrite the documents).
6. **Default-binding collision latent in code.** `CanonicalDefaults` binds
   `NavDown`→`Key.S` **and** `HoldfastStatus`→`Key.S` (`AshfallInputActions.cs:84,105`).
   `project.godot` avoids this by binding nav to arrow keys (keycodes
   4194319–4194322) while `ashfall_holdfast_status` keeps `S` (83). The
   never-called `ReconcileCollisions` would "repair" `holdfast_status` back to
   its colliding canonical default — the reconciliation policy is itself
   unsound for context-scoped bindings (B/S are holdfast-terminal-local). Any
   reuse of this code must first define context scopes (§6.4).
7. **Input-injection precedent exists for tests.** `UiAccessibilitySelfTest`
   Gate 4 (`src/Host/UiAccessibilitySelfTest.cs:140-160`) synthesizes
   `InputEventKey` and calls `_UnhandledInput` directly on a detached
   `DailyBriefingModal` specimen. This is the harness pattern Plan 37 extends
   for focus/hotkey probes — no new test framework.
8. **CI gate infrastructure is ready for a new row.** `docs/ci/CI_GATE_MANIFEST.json`
   (schema 1.0.0, 53 gates, `fast`/`full` classification, `expected_summary`
   strings, `critical` flags) is the registration point for the input-map
   gate; `scripts/ci/` has 50+ script siblings and no input gate today
   (`ls scripts/ci | grep -i input` → nothing).
9. **Panel registry is the route authority and is gate-protected.**
   `PanelRegistryBootstrap.RegisterAll()` registers ~140 routes across
   Dashboard/Secondary/Expanded/MainMenu groups; `PanelRouteGateTests`
   (`Ashfall.Core.Tests/UI/PanelRouteGateTests.cs`, 375 lines) proves
   switch↔registry parity. `OpenPlayerPanel` (`Main.GameFlow.cs:250-304`)
   resolves routes through the registry with visible diagnostics for dead
   routes. Any action→route binding must validate against this registry, not a
   parallel table.

## 2.3 Controller device reality

DESIGN.md targets fixed 1920×1080 desktop; `project.godot` ships keyboard-only
bindings; no code reads `InputEventJoypadMotion` (grep: `InputEventJoypad`/
`JoypadAxis` appears only inside the dead `RegisterAction`). A gamepad today
produces *no* in-game effect except whatever Godot's built-in `ui_*` defaults
map (Godot's project-wide default `ui_accept`/`ui_cancel`/`ui_focus_*` actions
do carry joypad defaults from the engine, and our `IsCloseOrCancel`/`IsConfirm`
already OR against `ui_cancel`/`ui_accept` — so *some* incidental gamepad
response exists on those two verbs only, unauthored and untested). Controller
parity is therefore greenfield-but-bounded: it rides on top of the focus spine
(Phase 5+), never before it.

---

# 3. Required Delta

| # | From (current reality, §2) | To (definition of done, §24) |
|---|---|---|
| D1 | 22 actions declared; 4 nav actions + `IsConfirmOrAccept` dead; joypad registration unreachable | Every declared action either has a live, tested dispatch path or is explicitly removed from the map; CI gate fails on any future orphan either direction |
| D2 | Three global close paths + ~114 per-panel input overrides + dormant ModalManager | One global unhandled-key dispatch owner (`Main.Application`), one overlay/modal close owner, panels consuming a shared close/focus contract instead of sniffing raw keys |
| D3 | `AshfallFocusPolicy` + Core `ModalStackController` dormant; focus grab ad hoc in 7 files | Plan 80 policy wired: deterministic initial focus on every overlay open, Tab/Shift+Tab trap inside overlays, focus restored to opener on close — via the existing classes, host adapter over the Core stack |
| D4 | Nav actions dead; no arrow/WASD focus movement anywhere | `ashfall_nav_*` drive focus movement within the active focus scope; lists/grids get row navigation on the top-10 surfaces first |
| D5 | Zero joypad bindings; dead registration code | Authored joypad bindings in `project.godot` for the bounded subset; registration code either repaired-and-called as headless fallback or deleted (decision §6.4) |
| D6 | No rebinding surface; `UserSettingsData` v1 | Rebinding section in the existing `SettingsPanel`; additive `key_bindings` map on `UserSettingsData` v2 with codec recovery; conflict detection; per-action + global reset |
| D7 | Help/guidance copy does not reflect live bindings | Guidance/help surfaces render `GetActionPrompt`-derived binding labels (existing seam), so rebinds update help automatically |
| D8 | `INPUT_AND_NAVIGATION_AUDIT.md` and `ACCESSIBILITY.md` overclaim | Docs corrected to match wired reality; a generated keyboard map (`docs/ui/KEYBOARD.md`) produced from the action table, never hand-edited |

---

# 4. Evidence

## 4.1 Full action-by-action audit (22 actions)

Predicate/dispatch columns verified by grep across `src/` on the current
worktree. "Dispatch target" = what pressing the key does today.

| Action | project.godot key | Predicate | Call sites | Dispatches to | Joypad today | Disposition |
|---|---|---|---|---|---|---|
| `ashfall_close` | Escape | `IsCloseOrCancel` (ORs `ui_cancel`) | 12 sites (§2.2.3) + `Main.Application:766,783` | Global close-all + per-panel close | none (incidental via engine `ui_cancel`) | Keep; unify close ownership (Phase 2) |
| `ashfall_confirm` | Enter | `IsConfirm` (ORs `ui_accept`) | `GameOverPanel:106`, `DailyBriefingModal:154` | Modal acknowledge | none (incidental via engine `ui_accept`) | Keep; extend to focused-control activation contract (Phase 3) |
| `ashfall_next_tab` | Tab | `IsNextTab` | `DailyBriefingModal:160` | Briefing skip/section advance | none | Keep; ALSO consumed by focus trap — define precedence: trap owns raw Tab inside modal scopes, action owns panel tab bars (§6.3) |
| `ashfall_nav_up` | Up | — none — | 0 | nothing | none | Wire to focus navigator (Phase 3) |
| `ashfall_nav_down` | Down | — none — | 0 | nothing | none | Wire to focus navigator (Phase 3) |
| `ashfall_nav_left` | Left | — none — | 0 | nothing | none | Wire to focus navigator (Phase 3) |
| `ashfall_nav_right` | Right | — none — | 0 | nothing | none | Wire to focus navigator (Phase 3) |
| `ashfall_journal` | J | `IsJournal` | `Main.Application:727` | journal panel / journal book toggle | none | Keep; add route-validation to gate |
| `ashfall_help` | F1 | `IsHelp` | `Main.Application:735` | `help` route (TutorialPanel) | none | Keep (source plan's "opens nothing" is stale) |
| `ashfall_guidance` | F2 | `IsGuidance` | `Main.Application:740` | `guidance` route toggle | none | Keep (do NOT re-propose; Plan 17B owns it) |
| `ashfall_forecast` | F | `IsForecast` | `Main.Application:717` | `OpenWeatherForecastPanel()` | none | Keep; migrate to `OpenPlayerPanel("weather_forecast")` route for registry parity (Phase 1, one-line) |
| `ashfall_weather_history` | H | `IsWeatherHistory` | `Main.Application:722` | `OpenWeatherHistoryPanel()` | none | Keep; same route-parity migration |
| `ashfall_events` | E | `IsEvents` | `Main.Application:761` | `OpenEventsLogPanel()` | none | Keep; same route-parity migration |
| `ashfall_expeditions` | X | `IsExpeditions` | `Main.Application:756` | `expeditions` route | none | Keep (orphan claim stale) |
| `ashfall_holdfast` | T | `IsHoldfast` | `Main.Application:751` | `holdfast` route | none | Keep (orphan claim stale) |
| `ashfall_journal_tab_1..5` | 1–5 | `GetJournalTabNumber` | `Main.Application:778` | journal book tab switch (journal-open scoped) | none | Keep; scope already correct |
| `ashfall_holdfast_build` | B | `IsHoldfastBuild` | `HoldfastTerminalPanel:477` | terminal build tab | none | Keep; context-scoped (holdfast only) — record scope in contract |
| `ashfall_holdfast_status` | S | `IsHoldfastStatus` | `HoldfastTerminalPanel:482` (Ctrl-guarded) | terminal status tab | none | Keep; **collides with `CanonicalDefaults[NavDown]=S`** in code defaults (latent; project.godot avoids it) — fix defaults, record scope |
| (`IsConfirmOrAccept`) | n/a — alias of `IsConfirm` | defined `:229` | **0** | nothing | n/a | **Delete the alias** (it is a pure pass-through to `IsConfirm`; nothing is gained) OR adopt as the modal-default-button predicate. Decision: delete — one predicate per verb, §10 |

## 4.2 Panel focus/keyboard parity audit (by class; verified samples cited)

245 files under `src/UI/*.cs`; 114 files across `src/` override
`_UnhandledInput`/`_UnhandledKeyInput`. Classification:

| Class | Members (verified) | Close handling | Initial focus | Focus trap | Notes |
|---|---|---|---|---|---|
| Global dispatchers | `Main.Application` (`:713`), `Main.GameFlow` (`:858`) | both global | n/a | none | Two owners; `GameFlow` Esc also triggers `ReturnToMenu` when no overlay open — a destructive-ish shortcut with no confirm |
| Scene-backed detail panels (22, `UI_PANEL_ARCHITECTURE_GUIDE` matrix) | e.g. `InventoryDetailPanel`, `CraftingPanel`, `MedicalPanel`, `SurvivorDetailPanel` | mixed: predicate (Crafting/Medical/Inventory) vs raw-Escape (SurvivorDetail family pattern per `SurvivorsPanel:308`) | none | none | `%CloseButton` contract exists in scenes; keyboard path varies per panel age |
| Recent Wave-7/8 panels | `VehicleGaragePanel` (verified: no input override at all), `SanitationPanel`, `SkyDefenseBatteryPanel`, `DynamicQuestlinePanel` | rely on global Esc only | none | none | Global close works, but focus lands nowhere after open; newest panels are *less* keyboard-equipped than mid-age ones |
| Modals | `DailyBriefingModal` (Enter/Tab/Esc + typewriter), `ConfirmationModal` (GrabFocus cancel), `MoralChoiceModal` (GrabFocus first), `OpeningProtocolModal`, `SafeCrackModal` | per-modal hand-rolled | ad hoc GrabFocus | none | No shared stack; two concurrent modals undefined |
| Menu/shell | `MainMenuPanel` (GrabFocus continue/new), `SettingsPanel` (raw Esc, cancel-restore), `GameDashboardPanel` (`AddNavButton` rail `:619-630`, ~60 nav buttons, no focus policy) | panel-local | menu only | none | Dashboard nav rail is the natural keyboard home base; currently mouse-first |
| HUD/terminal | `HoldfastTerminalPanel` (predicate + B/S scoped), `CombatHudOverlay`, `EmergencyResponseHud` (GrabFocus acknowledge) | predicate | 1 site | none | Holdfast B/S correctly context-scoped — the model for all scoped bindings |
| Remainder (~180 files) | atlas panels, expanded consoles, prototypes | per-file `_UnhandledInput` overrides mostly close-only | none | none | Phase 8 sweep; no per-panel rewrites before the shared contract lands |

## 4.3 Documentation-drift evidence (to correct, bounded)

| Document | Claim | Verified reality | Correction scope |
|---|---|---|---|
| `docs/ui/INPUT_AND_NAVIGATION_AUDIT.md` §1 | "Every action maps to … Joypad gamepad inputs"; table omits `ashfall_guidance` | 0 joypad events shipped; guidance exists | Regenerate the table from the action contract (Phase 7); mark the doc as generated-truth downstream of the gate |
| `docs/ACCESSIBILITY.md` §2 | Focus open/trap/restore "Implemented & CI-Gated" | Classes exist and lint gates exist; helpers unwired | Amend wording to "infrastructure present; wiring delivered by Plan 37" — one paragraph, no rewrite |
| `docs/ui/UI_PANEL_ARCHITECTURE_GUIDE.md` §"UI ARCHITECTURE INVARIANTS" #4 | Modals support Enter/Space/Esc "without trapping keyboard navigation" | True for the listed modals individually; no shared guarantee | Add the modal-stack contract reference once wired |

## 4.4 Harness and gate precedents (reuse targets)

- `UiAccessibilitySelfTest` Gate 1 (FocusMode validity), Gate 4 (synthesized
  `InputEventKey` into a detached modal) — the input-injection pattern.
- `PanelRouteGateTests` — registry parity pattern the action→route check joins.
- `ModalStackControllerTests` — existing Core coverage of the stack we adopt.
- `docs/ci/CI_GATE_MANIFEST.json` row format + `scripts/ci/run-gates.py` runner.
- `scripts/run_test.sh` — focused xUnit runner (180 s cap) per `TEST_POLICY.md`.
- `docs/ui/UI_NODE_DIAGNOSTICS_AND_LEAK_TRIAGE.md` — Tier-3 diagnostic
  classification, first-open caching vs monotonic-growth leak triage, and the
  §4/§5.1 shutdown-signature taxonomy our lifecycle tests must respect.

---

# 5. Existing Extension Seams

| Seam | Path | What it already provides | How Plan 37 extends it |
|---|---|---|---|
| Input vocabulary | `src/Host/AshfallInputActions.cs` | 22 constants, predicates, `GetActionPrompt`, collision/registration code | Add `IsNav*` predicates + `InputScope` notion; delete `IsConfirmOrAccept`; repair or delete dead registration path (§6.4) |
| Focus authority | `src/UI/AshfallFocusPolicy.cs` | open/trap/restore/style/focusable-enumeration helpers | **Wire it** from the panel open/close owner; add directional movement helper (spatial-aware) beside `TrapFocus` |
| Modal stack (engine-free) | `Assets/Ashfall.Core/UI/ModalStackController.cs` | tested LIFO + prior-focus retention + events | Reuse as the state authority inside the Godot adapter (no new Core type) |
| Modal adapter (host) | `src/UI/ModalManager.cs` + `src/UI/IModalPanel.cs` | Godot-side stack with GrabFocus/Restore | Re-implement internals over `ModalStackController<IModalPanel, Control>`; route actual modals through it |
| Panel routes | `Assets/Ashfall.Core/UI/PanelRegistry*.cs` + `PanelRegistryBootstrap` | ~140 validated routes, open actions, groups, menu-availability | Action→route bindings reference registry ids; gate validates resolvability |
| Overlay teardown | `src/Main.PanelLifecycle.cs` `CloseAllOverlayPanels` | single fan-out point | Add focus capture/restore hooks here (one place, whole-game effect) |
| Settings authority | `UserSettingsData`/`UserSettingsCodec`/`UserSettingsStore`/`SettingsPanel` | v1 DTO, recovery codec, atomic store, APPLY&SAVE panel | Additive `key_bindings` (v2), INPUT & CONTROLS section in the same panel — no second settings surface |
| A11y presentation | `src/Settings/AccessibilityPresentation.cs`, `AshfallUiHelpers.ToColor` (colorblind map) | preference predicates, scale factor | Unchanged; rebinding UI consumes the same focus/scale rules |
| Hotkey prompts | `AshfallInputActions.GetActionPrompt` | live binding → `[Key]` string | Consume from guidance/help/tutorial copy so rebinds propagate |
| UI test harness | `UiAccessibilitySelfTest`, `Main.UiTests.*`, `--player-panels-uitest`, `--panel-bind-lifecycle-selftest` | synthesized-input and lifecycle probes | Extend with focus-order and nav-key probes; no new harness |
| CI gate format | `docs/ci/CI_GATE_MANIFEST.json`, `scripts/ci/run-gates.py` | 53-gate manifest, fast/full tiers | Add gate 54 `input_map_contract` (fast, critical) |

Deliberately **not** seams: `project.godot`'s engine-default `ui_*` actions
(we read them via the existing OR-predicates, never rebind them),
`AudioManager` cue cooldowns (respected, not extended), and anything under
`Assets/Ashfall.Core/` beyond the already-existing `ModalStackController`
(justification in §6.1).

---

# 6. Proposed Architecture

## 6.1 Placement decision: everything new lives in `src/` (host)

Input events, focus rings, and the Godot `Control` tree are engine-coupled by
definition. The only engine-neutral piece of this package — modal stack state —
**already exists in Core** (`ModalStackController<TModal, TFocus>`, tested).
Therefore: **no new Core types, no Core references to Godot, no engine-neutral
"focus contract" extraction.** Extracting an interface vocabulary for focus
would create a second, speculative abstraction with exactly one implementation
— the thing Rule 5 forbids. If a future headless-UI-test need arises, the
seam is the existing `ModalStackController`, not a new Core layer.

## 6.2 Component map (four owners, no overlaps)

```
project.godot [input]  ──(authoritative shipped bindings)──►  Godot InputMap
        ▲                                                        │
        │ additive v2 overrides applied at startup               │ events
src/Settings/KeyBindingApplicator.cs  ◄── UserSettingsData.key_bindings (Core DTO field)
        │                                                        ▼
src/Host/AshfallInputActions.cs  (vocabulary + predicates + prompts + contract metadata)
        │                                                        │
        ▼                                                        ▼
src/Main.Application.cs  _UnhandledKeyInput  (SOLE global hotkey/close dispatch)
        │
        ├──► src/UI/ModalManager.cs  (stack adapter over Core ModalStackController)
        │          └── owns: modal LIFO close, prior-focus capture/restore
        └──► src/UI/AshfallFocusPolicy.cs  (+ new AshfallFocusNavigator)
                   └── owns: initial focus, Tab trap, nav-key movement, focus ring
```

- **Owner 1 — Dispatch:** `Main.Application._UnhandledKeyInput` remains the one
  global hotkey dispatcher. `Main.GameFlow._UnhandledInput`'s close branch is
  **merged into it** (the Esc→close-overlays→else-menu logic moves; GameFlow's
  override is deleted), eliminating the dual-owner race where both fire on the
  same Esc press (today Application marks handled first only by tree-order
  luck).
- **Owner 2 — Modal/focus stack:** `ModalManager`, rebuilt internally over the
  Core `ModalStackController`, instantiated once by `Main` and consulted by the
  dispatcher before any panel sees input. Its public API is preserved (it is
  exercised by `AudioSelfTest`) but its state machine delegates to Core.
- **Owner 3 — Focus navigation:** `AshfallFocusPolicy` stays the static policy
  home; a sibling `src/UI/AshfallFocusNavigator.cs` adds directional movement
  and per-panel focus-scope registration. No per-panel `FocusMode` audits —
  factories already produce focusable buttons (Godot `Button` default is
  `FocusMode.All`); the a11y selftest Gate 1 already fails any regression to
  `None`.
- **Owner 4 — Bindings persistence:** the existing settings quartet
  (`UserSettingsData` + `UserSettingsCodec` + `UserSettingsStore` +
  `SettingsPanel`). One additive DTO field, one new panel section, one
  applicator (`src/Settings/KeyBindingApplicator.cs`) that diffs overrides
  onto the live `InputMap` at startup and on APPLY&SAVE.

## 6.3 Focus scope contract

Every overlay panel already has a single root `Control`. The contract:

1. **Registration:** the panel open path (`OpenPlayerPanel` /
   `RegisterPlayerSurfaces` open actions) calls
   `AshfallFocusPolicy.OpenWithFocus(panelRoot, initial: panel.InitialFocusControl ?, opener: currentlyFocused)`.
   Panels do not call `GrabFocus` themselves after migration (the 7 ad hoc
   sites are folded into the contract).
2. **Trapping:** while an overlay is the topmost visible overlay, the
   dispatcher routes unhandled `ui_focus_next`/`ui_focus_prev` (raw Tab /
   Shift+Tab) into `AshfallFocusPolicy.TrapFocus(root, event)`. Precedence
   rule: **the focus trap owns Tab whenever a modal/overlay scope is active;
   `ashfall_next_tab` is reserved for genuine tab-bar controls** (today only
   the briefing modal consumes it; tab-bar panels consume it inside their
   `_GuiInput`, which runs before unhandled dispatch, so no conflict).
3. **Directional movement:** `ashfall_nav_*` events reaching the dispatcher
   are routed to `AshfallFocusNavigator.MoveDirection(root, dir)`, which
   enumerates `FindFocusableControls(root)` and moves focus spatially
   (nearest neighbor in the requested axis, row-aware: left/right stay within
   the current row band when one exists — the grid rule from the source plan,
   implemented geometrically over control rects rather than per-panel wiring).
4. **Restoration:** every close path — global close-all, modal pop,
   panel-local close button — funnels through one teardown helper that calls
   `RestoreFocusFromRoot(panelRoot)` before hiding. `CloseAllOverlayPanels`
   gains exactly this hook (capture on open is what makes restore possible).
5. **Freed-node safety:** restore targets are validated with
   `GodotObject.IsInstanceValid && IsInsideTree && Visible` (the existing
   `RestoreFocus` guard); on invalid, focus falls back to the dashboard nav
   rail's first button — never to a freed node (the source plan's crash class,
   §17 F3).

## 6.4 Binding authority: `project.godot` wins; registration code is deleted, not repaired

Decision (recommended, rationale below): **delete `EnsureActionsRegistered()`
and `ReconcileCollisions()` from `AshfallInputActions.cs`** and make
`project.godot` the sole shipped binding source, with the new
`KeyBindingApplicator` applying user overrides on top at runtime.

Rationale: (a) the registration code is dead today and its `!hasKey` guard
means it can *never* add joypad events over a `project.godot` that ships keys —
repairing it means inverting that guard, at which point it duplicates the
applicator's job; (b) two writers to the `InputMap` (registration defaults +
applicator overrides) is exactly the parallel-authority shape Rule 5 bans;
(c) headless selftests run with `project.godot` loaded (they boot the project),
so no headless gap exists that would require code-side registration.
**Fallback if the foreman prefers retention:** keep `EnsureActionsRegistered`
but fix its guard to *also* add missing joypad/secondary events, call it from
`Main._Ready` before the applicator, and fix the `NavDown`/`HoldfastStatus`
`S` collision by scoping holdfast bindings. The plan proceeds with deletion;
the retention path is recorded here so the decision is explicit at review.

Joypad bindings are then **authored data in `project.godot`** (diffable,
reviewable, gate-checked) — see §15 for the exact table.

## 6.5 Rebinding model

- **Storage:** `UserSettingsData` v2 gains one field:
  `Dictionary<string, List<int>> key_bindings` — action name → list of
  Godot `Key` enum values (integers; culture-invariant, locale-proof). Joypad
  overrides are out of v2 scope (bounded controller decision, §22) — the
  field's value type leaves room (`List<int>` carries joypad button indices in
  a later schema bump without a format break, documented in the codec).
- **Codec:** `UserSettingsCodec` bump to accept v1 (absent field → empty map)
  and emit v2; recovery path unchanged (unknown/corrupt entries dropped with
  diagnostic, never throw) — same discipline as the existing E1–E8 matrix.
- **Application:** `KeyBindingApplicator.Apply(UserSettingsData)`:
  for each of the 22 canonical actions (iterated over the *sorted* action list
  — determinism rule), erase key events added by a previous override, apply
  override keys, then run conflict detection (§17 F6). Never touches
  `ui_*` engine actions. Applied at startup after settings load and on
  SettingsPanel APPLY&SAVE.
- **Conflict policy:** conflicts are scoped — global actions (close/confirm/
  hotkeys/nav) may not share a key; context-scoped actions
  (`holdfast_build/status`, `journal_tab_*`) may collide with globals because
  they only fire inside their context (the existing B/S precedent). The
  applicator rejects a rebinding that collides within scope and reports the
  owning action to the UI.

## 6.6 What is deliberately NOT built

- No `InputManager` singleton, no event bus for input, no command-pattern
  abstraction over panel actions.
- No per-panel base-class rewrite (`AshfallPanelBase`) in this package — the
  contract is delivered through the open/close owner and the dispatcher, so
  245 panel files stay untouched except the close-sniff migration (Phase 8,
  mechanical, gated).
- No mouse-cursor-emulation gamepad mode, no radial menus, no stick scrolling
  physics. Stick = repeated directional focus steps with a deadzone (0.5
  already in the map) and a 150 ms repeat cadence in the navigator.
- No new save section, no new JSON data catalog, no RNG, no Core simulation
  contact of any kind.

---

# 7. Ownership Matrix

Per `WORKTREE_OWNERSHIP.md` (read 2026-09-19): **no active claim covers
`project.godot`, `src/Host/AshfallInputActions.cs`, `src/UI/` focus/settings
files, or `src/Settings/`.** Two coordination notes: (a) `claim-c1-plan24-survivor-ledger-2026-09-16`
lists `src/UI/ExpeditionPanel.cs` among its paths — Plan 37's only contact with
that file is removing two ad hoc `GrabFocus` lines during Phase 8, which must
be sequenced with the integrator or deferred to them; (b) governance files
(`INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`) are foreman/integrator-owned
— this package proposes rows, it does not write them.

Proposed claim rows (exact paths) for the builder package:

| Phase | Claim | Exact paths |
|---|---|---|
| P1 | Contract + gate | `src/Host/AshfallInputActions.cs`, `scripts/ci/input-map-gate.sh` (new), `docs/ci/CI_GATE_MANIFEST.json` (+1 row), `Ashfall.Core.Tests/Tooling/InputMapContractTests.cs` (new), `src/Main.Application.cs` (route-parity lines only) |
| P2 | Close ownership | `src/Main.Application.cs`, `src/Main.GameFlow.cs` (delete `_UnhandledInput` close branch), `src/Main.PanelLifecycle.cs`, `src/UI/ModalManager.cs`, `src/UI/IModalPanel.cs`, modal files (`DailyBriefingModal`, `ConfirmationModal`, `MoralChoiceModal`, `OpeningProtocolModal`, `SafeCrackModal`) |
| P3 | Focus spine | `src/UI/AshfallFocusPolicy.cs`, `src/UI/AshfallFocusNavigator.cs` (new), `src/Main.GameFlow.cs` (open-path hook), `src/UI/GameDashboardPanel.cs` (nav-rail fallback focus target) |
| P4 | Top-10 surfaces | focus-order verification only on: dashboard, survivors, expeditions, inventory, crafting, medical, trade, duty_roster, map, briefing — **no redesigns**, wiring + per-panel row-nav where a list exists |
| P5 | Controller bindings | `project.godot` ([input] joypad events only), `src/Host/AshfallInputActions.cs` (delete dead registration), `src/UI/AshfallFocusNavigator.cs` (stick repeat) |
| P6 | Rebinding | `Assets/Ashfall.Core/Settings/UserSettingsData.cs` (+1 field), `Assets/Ashfall.Core/Settings/UserSettingsCodec.cs` (v2), `src/Settings/UserSettings.cs` (no logic change expected; apply hook), `src/Settings/KeyBindingApplicator.cs` (new), `src/UI/SettingsPanel.cs` (+1 section), `Ashfall.Core.Tests/Settings/UserSettingsRecoveryTests.cs` (extend) |
| P7 | Docs & help truth | `docs/ui/INPUT_AND_NAVIGATION_AUDIT.md` (correct), `docs/ACCESSIBILITY.md` (one paragraph), `docs/ui/KEYBOARD.md` (new, generated), generator `scripts/ci/generate-keyboard-map.py` (new), `src/UI/OnboardingHintPanel.cs` + `src/UI/TutorialPanel.cs` (consume `GetActionPrompt` where hardcoded) |
| P8 | Close-sniff sweep | the ~100 remaining `_UnhandledInput` close-only overrides, mechanical, in tranches of ≤20 files with the gate green between tranches |

---

# 8. Data Flow

**Key press → outcome (after integration):**

1. Godot delivers the event; focused control's `_GuiInput` gets first refusal
   (tab bars, text edits — unchanged engine behavior).
2. Unhandled → active modal scope: `ModalManager.HandleInput` (now actually
   called from the dispatcher) gets close/confirm first; Tab goes to
   `AshfallFocusPolicy.TrapFocus`; nav keys go to
   `AshfallFocusNavigator.MoveDirection` scoped to the modal root.
3. Still unhandled → overlay scope: same trap/nav routing against the topmost
   visible overlay root (tracked by the open-path hook, one stack of roots,
   owned by Main — not a new manager, an array beside `CloseAllOverlayPanels`).
4. Still unhandled → global hotkeys: the existing `Main.Application` branch
   chain (forecast/weather/journal/help/guidance/holdfast/expeditions/events/
   close/journal-tabs), all gated on `GameState.Playing` as today.
5. Every consumed path calls `SetInputAsHandled()` exactly once — no
   double-dispatch (today's Application+GameFlow overlap is eliminated).

**Rebind flow:** SettingsPanel INPUT section → capture-next-press → conflict
check (scoped) → working copy → APPLY&SAVE → `UserSettingsStore.Save` (atomic
.tmp swap, existing) → `KeyBindingApplicator.Apply` → live `InputMap` →
`GetActionPrompt` consumers (guidance/help) show the new binding on next
paint. No restart.

**Focus lifecycle:** open → `OpenWithFocus` records opener meta + focuses
initial/first focusable (deferred call, existing pattern) → user navigates via
Tab-trap or nav keys → close → teardown helper restores opener, falling back
to dashboard rail if the opener is invalid/freed.

---

# 9. State Model

| State | Owner | Persistence | Lifetime |
|---|---|---|---|
| Canonical action list + default keys | `AshfallInputActions` constants + `project.godot` | `project.godot` (repo) | static |
| User key overrides | `UserSettingsData.key_bindings` (v2) | `user://settings.json` via `UserSettingsStore` (atomic) | per-user, survives reinstall of saves; independent of campaign saves by design (existing separation, QA doc §1) |
| Modal stack + prior focus | Core `ModalStackController` inside host `ModalManager` | none (runtime) | session |
| Focus scope stack (overlay roots) | `Main` (array beside `CloseAllOverlayPanels`) | none | session |
| Focused control | Godot viewport | none | frame |

No campaign-save contact: input/focus state never enters `SaveOrchestrator`,
no save section, no migration of gameplay saves. The only persisted artifact
is the settings DTO bump (v1→v2, additive, recovery-tested). Determinism note:
nothing here touches `CampaignRngManager` or simulation state; replay harnesses
are unaffected. UI selftests that synthesize input remain deterministic
(fixed event objects, no wall-clock).

---

# 10. API/Contracts

## 10.1 `AshfallInputActions` additions/changes (host, `src/Host/AshfallInputActions.cs`)

```csharp
// NEW predicates — the only vocabulary addition in the package
public static bool IsNavUp(InputEvent e)    => e.IsActionPressed(NavUp);
public static bool IsNavDown(InputEvent e)  => e.IsActionPressed(NavDown);
public static bool IsNavLeft(InputEvent e)  => e.IsActionPressed(NavLeft);
public static bool IsNavRight(InputEvent e) => e.IsActionPressed(NavRight);

/// <summary>Scope in which an action fires. Drives conflict policy and the gate.</summary>
public enum InputScope { Global, PlayingOnly, JournalBook, HoldfastTerminal }

/// <summary>Static contract row consumed by the gate script and the rebinding UI.
/// Single source for: action → scope → route (nullable) → rebindable flag.</summary>
public sealed record InputActionContract(string Action, InputScope Scope, string? RouteId, bool Rebindable);
public static readonly IReadOnlyList<InputActionContract> Contract = new[]
{ /* 22 rows, one per action; e.g. */ new InputActionContract(Close, InputScope.Global, null, true), … };

// DELETE: IsConfirmOrAccept (orphan alias), EnsureActionsRegistered, ReconcileCollisions
// (dead code per §6.4; conflict detection moves into KeyBindingApplicator with scopes)
// KEEP unchanged: all existing predicates, GetJournalTabNumber, GetActionPrompt,
// CanonicalDefaults (fixed: NavDown default corrected to Key.Down to match
// project.godot — S remains holdfast-scoped).
```

## 10.2 `AshfallFocusNavigator` (new, `src/UI/AshfallFocusNavigator.cs`)

```csharp
public static class AshfallFocusNavigator
{
    /// <summary>Move focus within root in the given direction. Spatial:
    /// nearest focusable whose center lies in the requested half-plane,
    /// preferring same-row/column band (grid rule). Returns true if focus moved.</summary>
    public static bool MoveDirection(Control root, Vector2I direction);

    /// <summary>Route the four nav actions to MoveDirection. Called from the
    /// dispatcher before global hotkeys. Returns true when consumed.</summary>
    public static bool HandleNavInput(Control scopeRoot, InputEvent @event);

    /// <summary>Stick support: converts held joypad motion into repeated
    /// directional steps on a 150 ms cadence (deadzone comes from the action map).
    /// Called from _Process only while a scope is active; respects
    /// AccessibilityPresentation.MotionAllowed for cadence scaling OFF (no
    /// animation, focus jumps are instant either way — motion flag is N/A here
    /// but recorded for honesty).</summary>
    public static void TickStickRepeat(Control scopeRoot, double delta);
}
```

## 10.3 `ModalManager` rebuilt over Core (host adapter, public API preserved)

```csharp
public sealed class ModalManager
{
    private readonly ModalStackController<IModalPanel, Control> _core = new();
    // PushModal/PopTopModal/CloseAll/HandleInput signatures unchanged;
    // internals delegate; focus grab/restore stay in the adapter (Godot side).
    // HandleInput gains: Tab→TrapFocus, nav→MoveDirection on TopModal root,
    // confirm→TopModal default button activation, close→PopTopModal+restore.
}
```

## 10.4 `KeyBindingApplicator` (new, `src/Settings/KeyBindingApplicator.cs`)

```csharp
public static class KeyBindingApplicator
{
    /// <summary>Diff user overrides onto the live InputMap. Iterates the
    /// sorted canonical action list (deterministic order). Never touches ui_*.
    /// Returns applied-override count for diagnostics.</summary>
    public static int Apply(UserSettingsData data);

    /// <summary>Validate a proposed binding for action: scope-aware conflict
    /// check. Returns the conflicting action id or null.</summary>
    public static string? FindConflict(string action, Key proposed);

    /// <summary>Reset one action to CanonicalDefaults; ResetAll() clears the
    /// override map. Used by SettingsPanel buttons.</summary>
    public static void Reset(UserSettingsData data, string action);
    public static void ResetAll(UserSettingsData data);
}
```

## 10.5 Settings DTO (Core, additive)

```csharp
// Assets/Ashfall.Core/Settings/UserSettingsData.cs — schema_version 1 → 2
[JsonPropertyName("key_bindings")]
public Dictionary<string, List<int>> KeyBindings { get; set; } = new();
// Clone() extended; codec: v1 input → empty map; v2 round-trip; corrupt
// entries dropped with diagnostic (existing DeserializeWithRecovery pattern).
```

## 10.6 Gate contract (new, `scripts/ci/input-map-gate.sh` + xUnit mirror)

Fail when: (a) a `project.godot [input]` action has no `InputActionContract`
row; (b) a contract row has no handler evidence (dispatcher branch, predicate
call site, or explicit `Scope`-local handler); (c) a predicate exists with no
call site; (d) any handler references an undeclared action name; (e) two
global-scope actions share a default key; (f) an action→route row names a
route absent from `PanelRegistry` (mirror of `PanelRouteGateTests` logic).
Manifest row: `gate_id: "input_map_contract"`, category "Host Selftests &
Lifecycle", classification `fast`, `critical: true`, `expected_summary:
"input map contract PASS"` — matching the existing manifest schema 1.0.0.

---

# 11. Data Changes

One additive field on `UserSettingsData` (§10.5) and its codec version bump.
**No changes under `Assets/StreamingAssets/Data/`** — keybindings are user
preferences, not authored game data; the JSON data authority is untouched and
`CatalogIntegrityValidator` gains no rules. `project.godot [input]` gains
joypad `InputEventJoypadButton` entries for the bounded subset (§15) — engine
config data, diff-reviewed and gate-checked.

---

# 12. Save/Load

Campaign saves: **zero contact.** No new save section, no `SaveSectionRegistry`
row, no migration. User settings: v1→v2 additive; v1 files load with an empty
override map (no binding changes); corrupt `key_bindings` entries drop
individually with a diagnostic line via the existing recovery path (extends
the E1–E8 matrix with E9/E10 rows in the QA doc, §18). Safe-mode boot
(hold Shift at launch → skip override application for that session) is
implemented as a four-line check in the applicator call site — the support
escape hatch the source plan asks for, without a second settings file.

---

# 13. Determinism

Input handling is presentation-layer; no simulation state, no RNG, no
`System.Random`, no wall-clock reads in any new code (stick-repeat cadence
uses frame `delta`, not wall time, and never feeds gameplay). Applicator
iteration is over a **sorted** action list; conflict reporting order is
therefore stable across runs and cultures (the project already pins
culture-invariant formatting elsewhere — same discipline). Headless selftests
synthesize fixed `InputEventKey`/`InputEventJoypadButton` objects (existing
Gate 4 pattern), so no device dependence. The determinism gates
(`CampaignDayOwnerDeterminismGateTests`, golden saves) are unaffected and are
not re-baselined.

---

# 14. System/Event Wiring

No Core event surface changes. Host wiring only:

- `Main._Ready`: `KeyBindingApplicator.Apply(UserSettingsStore.Current)` after
  the existing settings load/apply (one line beside the current call).
- `SettingsPanel.OnSettingsApplied` → existing refresh owner
  (`Main.RefreshAccessibilityPreferenceSurfaces` per Plan 184 map §3.3) gains
  one applicator call — same event, no new event type.
- `ModalManager.ModalOpened/ModalClosed` (existing events, now actually
  raised in production) → audio layer may listen for open/close cues later;
  this package does not add audio cues (audio edge policy unchanged).
- Panel open/close hooks live inside `OpenPlayerPanel` and the teardown
  helper — no panel-specific event subscriptions are added, keeping the
  `PanelSubscriptionHygiene` gate green.

---

# 15. Godot Integration

## 15.1 Controller binding table (authored into `project.godot [input]`, bounded subset)

| Action | Keyboard (shipped) | Joypad (new) | Scope | Consuming surface |
|---|---|---|---|---|
| `ashfall_close` | Escape | `JoyButton.B` | Global | dispatcher close / modal pop |
| `ashfall_confirm` | Enter (+Space via code default — see note) | `JoyButton.A` | Global | focused control / modal default |
| `ashfall_next_tab` | Tab | `JoyButton.RightShoulder` | scoped (tab bars) | briefing + tab controls |
| `ashfall_nav_up` | Up | `JoyButton.DpadUp` + left-stick (motion, deadzone 0.5 existing) | Global | focus navigator |
| `ashfall_nav_down` | Down | `JoyButton.DpadDown` + left-stick | Global | focus navigator |
| `ashfall_nav_left` | Left | `JoyButton.DpadLeft` + left-stick | Global | focus navigator |
| `ashfall_nav_right` | Right | `JoyButton.DpadRight` + left-stick | Global | focus navigator |
| `ashfall_journal` | J | `JoyButton.Y` | PlayingOnly | journal route |
| `ashfall_help` | F1 | `JoyButton.Back` | PlayingOnly | help route |
| `ashfall_guidance` | F2 | — (deliberately unbound; menu-only secondary) | PlayingOnly | guidance toggle |
| `ashfall_forecast` / `ashfall_weather_history` / `ashfall_events` / `ashfall_expeditions` / `ashfall_holdfast` | F / H / E / X / T | — (keyboard-only; reachable on pad via nav + journal/help) | PlayingOnly | routes |
| `ashfall_journal_tab_1..5` | 1–5 | — | JournalBook | journal tabs |
| `ashfall_holdfast_build` / `ashfall_holdfast_status` | B / S | — | HoldfastTerminal | terminal tabs |

Bounded-scope honesty (source plan 37C step 1): the pad target is **menus,
focus navigation, confirm, cancel, next-tab, journal, help** — not button
remaps of every hotkey and not cursor emulation. Recorded here so "half a
controller mode" is a deliberate, documented boundary rather than an accident.
Note: `Confirm`'s Space secondary currently exists only in the dead code path;
if Space-confirm is wanted it must be authored into `project.godot` explicitly
(decision for implementation: yes, add — it matches the architecture guide's
modal invariant #4).

## 15.2 Engine specifics

- Fixed 1920×1080 canvas with `keep_height`/`canvas_items`: focus geometry
  math uses `GetGlobalRect()` in canvas space — resolution-independent, no
  scaling bugs at 1280×800 review passes.
- `window/size/mode=4` keeps headless selftests able to boot the real project
  with the real input map — the gate and the nav probes run against shipped
  bindings, not a test double.
- No theme changes: the focus ring is the existing
  `AshfallFocusPolicy.MakeFocusVisibleStyleBox()` (2px `Theme.Hot` border,
  0-radius corners — DESIGN.md compliant), applied by `ApplyFocusVisibleStyle`
  from the shared button factory (`AshfallUiHelpers.MakeButton` — one call,
  whole-game effect, the "fix it in the factory" rule) and to scene-backed
  `%CloseButton`s via the open hook.
- Contrast: `Theme.Hot` on `Theme.Ink` is 12.5:1 (WCAG AAA) per
  `docs/ACCESSIBILITY.md` §4; the ring also adds border *width* (shape/weight
  change), satisfying the no-color-only rule; colorblind mapping flows through
  the existing `AshfallUiHelpers.ToColor` mapper untouched.

---

# 16. Narrative/Content Integration

Not applicable: no quest, faction, text, or lore content is authored or
modified. The only player-visible strings are rebinding-UI labels and the
generated keyboard map, which use existing restrained UI vocabulary and
`T()`-keyed conventions where a label is user-facing (per Wave 3 keyed-string
discipline; no hardcoded prose in new UI).

---

# 17. Failure Modes

| # | Failure mode | Detection | Behavior (contract) | Covering test |
|---|---|---|---|---|
| F1 | Modal stacking: two modals open (e.g. confirmation over briefing) | `ModalManager` stack depth | LIFO: close/confirm/nav apply to top only; underlying modal inert; focus trapped to top | modal-stack xUnit (Core controller, existing) + host adapter probe |
| F2 | Focus loss on panel close (opener freed or never recorded) | restore guard (`IsInstanceValid && IsInsideTree && Visible`) | fallback to dashboard nav-rail first button; never leave viewport with zero focus owner | freed-node focus test (open → free opener → close → assert fallback) |
| F3 | Focus on a freed node after session swap (new game / load while panel open) | open hooks re-run per session; panels rebind per existing lifecycle | teardown helper clears scope stack on `CloseAllOverlayPanels`; restore validates instances | lifecycle selftest extension: swap session with panel open → assert no invalid-focus crash, focus on rail |
| F4 | Controller disconnect mid-session | Godot joypad events simply stop; no state held per-device | keyboard/mouse unaffected; no reconnect logic needed (stateless); documented | manual QA checklist row only (no harness can unplug hardware) |
| F5 | Echo/repeat spam (held Tab or nav key opening five modals) | `@event.IsEcho()` guard already at dispatcher head (`Main.Application.cs:714`) | echo events ignored for hotkeys; navigator uses cadence for held stick; key echo does not move focus faster than engine repeat | unit: echo event → no-op probe |
| F6 | Rebind conflict (two global actions on one key) | `FindConflict` at capture time + applicator re-check | capture rejected with owning-action label; applicator refuses and keeps previous binding; scoped actions exempt per §6.5 | xUnit: conflict matrix (global×global rejected, global×scoped allowed) |
| F7 | Corrupt/unknown entries in `key_bindings` | codec recovery | drop bad entries with diagnostic, keep valid ones, never throw; file rewritten clean on next save | extend `UserSettingsRecoveryTests` (E9/E10) |
| F8 | Player binds self into a corner (e.g. unbinds close everywhere) | rebinding UI forbids empty binding for `close`/`confirm`; safe-mode boot (Shift) skips overrides | close/confirm always keep at least their canonical default; safe mode documented on the settings surface | xUnit: reset + safe-mode flag probe |
| F9 | `ui_*` shadowing (custom action collides with engine ui_accept/ui_cancel semantics) | gate rule (e): global defaults must not duplicate `ui_accept`(Enter/Space)/`ui_cancel`(Esc) keys unless intentional (close/confirm deliberately OR them) | contract rows mark the two intentional overlaps; everything else fails the gate | gate self-test with fixture project.godot fragment |
| F10 | Nav keys leak to gameplay while LineEdit has focus (typing WASD/arrows in a text field) | engine: LineEdit consumes keys in `_GuiInput` before unhandled dispatch | no change needed; probe asserts navigator never sees events while a LineEdit owns focus | harness probe on SaveLoadPanel filename field |
| F11 | Panel opened while `_state != Playing` (menu state) | existing `GameState` gates in dispatcher | hotkeys inert in menu except close/settings — unchanged | existing menu-route tests stay green |
| F12 | Scale change (1.5×) pushing focusable controls off-screen | overflow probes on top-10 panels at 0.8/1.0/1.5 | scroll containers already required by panel conventions; probe fails on off-viewport focusable | snapshot/overflow probe set (extends existing snapshot manifest targets, no rebaseline of unrelated panels) |
| F13 | Double-dispatch Esc closing overlay AND returning to menu | single-owner merge (§6.2) | one owner, one `SetInputAsHandled` | regression probe: Esc with open panel → panel closed, still Playing |
| F14 | Disabled control receiving focus | `FindFocusableControls` already skips disabled buttons | navigator and trap share the same enumerator — one rule | existing Gate 1 + navigator unit probe |
| F15 | Leak: focus-scope stack retaining panel roots after free | teardown clears entries; entries hold weak validity via instance checks | no retention; `UI_NODE_DIAGNOSTICS` triage rules apply (first-open deltas benign, monotonic growth actionable) | diagnostics run on the lifecycle selftest per `UI_NODE_DIAGNOSTICS_AND_LEAK_TRIAGE.md` §3 |

---

# 18. Test Strategy

Per `TEST_POLICY.md`: focused files only, new test files run alone first via
`bash scripts/run_test.sh <path>` (180 s cap), no full-suite runs by default,
aggregate only homogeneous contract rows (the 22-action gate table is exactly
that), keep lifecycle/freed-node/mutation probes independent.

## 18.1 New xUnit: `Ashfall.Core.Tests/Tooling/InputMapContractTests.cs` (mirror of the shell gate, run alone first)

1. Every `project.godot` action has a contract row (parse the file; no Godot needed — text parse, same technique as existing static gates).
2. Every contract row's action exists in `project.godot`.
3. Every `Is*` predicate has ≥1 call site in `src/` (source-scan).
4. No call site references an undeclared action constant.
5. Global-scope default keys are pairwise distinct (the intentional `ui_*` OR-overlaps excepted by explicit contract flag).
6. Every contract `RouteId` resolves in `PanelRegistry` (reuse `PanelRegistryBootstrap.RegisterAll()`, the `PanelRouteGateTests` pattern).
7. `CanonicalDefaults` has no within-map collision (pins the NavDown/HoldfastStatus fix).
8. `IsConfirmOrAccept` does not exist (deletion pin; reversed if the foreman keeps it — the test encodes the decision).
9. Rebind conflict matrix (F6) via `KeyBindingApplicator.FindConflict` — pure function over the contract list, host-side but engine-free signature (string/enum in, string out) so it can live in the Tooling suite against a stub contract… **correction at design time:** `KeyBindingApplicator.Apply` touches Godot `InputMap`, so the applicator lives in `src/` and its *policy* (`FindConflict` over contract rows) is the xUnit-targetable pure part; the Godot-touching half is covered by headless selftest. Recorded so nobody forces Core-inappropriate code into the test project.
10. Settings codec: v1 file → empty map; v2 round-trip; corrupt-entry drop + diagnostic (extends `UserSettingsRecoveryTests` instead if that reads cleaner — decision at implementation, one home only).

## 18.2 Headless selftest extensions (host, existing harnesses)

- `UiAccessibilitySelfTest` gains Gate 6: synthesized nav-key events move focus
  within a specimen panel in the expected order (reuses the Gate 4 injection
  pattern); Gate 7: open→close restores focus to a recorded opener specimen.
- `--panel-bind-lifecycle-selftest` (production lifecycle path, zero-warning
  baseline per leak-triage §5.1): add "open overlay, free opener, close →
  fallback focus" probe (F3) and "session swap with panel open" probe (F3b).
  Must emit **zero** new shutdown RIDs per the known-benign signature rules.
- `--player-panels-uitest`: top-10 surfaces gain "initial focus owner exists
  after open" assertions (cheap, per-panel one-liner in the existing loop).
- Mouseless-day journey: extend the existing journey harness
  (`Main.UiTests.RealCampaignJourney` / `--playable-shell-selftest` family)
  with a scripted key-only path: open dashboard → nav to survivors → open →
  assign → close → advance day → briefing acknowledge (Enter) → back. This is
  the DoD journey; it runs as one focused selftest, not a suite.

## 18.3 Gates

- New gate 54 `input_map_contract` in `CI_GATE_MANIFEST.json` (fast, critical),
  command `bash scripts/ci/input-map-gate.sh`, `expected_summary` string per
  §10.6; the script reuses the manifest's runner conventions.
- Existing gates that must stay green: `ui_accessibility` (5 gates),
  `panel_bind_lifecycle`, `panel_route_gate`, `player_surface_coverage`,
  snapshot diff (only the touched-panel targets may drift; unrelated drift is
  a failure signal, per D1 verification-truth discipline).

## 18.4 Manual QA

Extend `docs/qa/AUDIO_AND_SETTINGS_RECOVERY_SMOKE_TEST.md` with rows
E9 (corrupt `key_bindings`), E10 (safe-mode boot), plus a 10-row keyboard/
controller checklist (mouseless day, rebind persistence across restart,
conflict rejection, controller connect→navigate→confirm→cancel). Manual rows
stay manual; nothing manual blocks CI.

---

# 19. Dependency-Ordered Phases

Order is strict: each phase is independently shippable, gate-green, and does
not require the next. The source plan's own ordering constraint (37A → 37B →
37C: no controller bindings before focus exists) is preserved.

| Phase | Content | Depends on | Exit evidence |
|---|---|---|---|
| **P0 Premise audit** | This document; no code | — | Foreman acceptance of §2 verdicts (esp. stale rows) |
| **P1 Contract + gate** | `InputActionContract` table, nav predicates, delete `IsConfirmOrAccept`, route-parity one-liners (forecast/weather-history/events → `OpenPlayerPanel` routes), gate 54 + xUnit mirror | P0 | gate fails on a seeded orphan fixture, passes on HEAD; focused tests green |
| **P2 Close ownership** | Merge GameFlow Esc into Application dispatch; teardown helper with restore hook; ModalManager over Core controller; modals routed through it | P1 | F13 regression probe green; modal-stack adapter tests green; lifecycle selftest unchanged-green |
| **P3 Focus spine** | Wire `OpenWithFocus`/trap/restore into open/close owner; focus ring in button factory + close buttons; `AshfallFocusNavigator`; dashboard rail fallback | P2 | a11y Gate 6/7 green; dashboard fully keyboard-navigable |
| **P4 Top-10 surfaces** | Initial focus + row navigation on the ten named panels; per-panel verification probes | P3 | `--player-panels-uitest` focus assertions green on the 10 |
| **P5 Controller bindings** | `project.godot` joypad events per §15.1; delete dead registration code; stick repeat cadence | P3 (focus must exist first) | headless joypad-event probe navigates dashboard; manual pad checklist |
| **P6 Rebinding** | DTO v2 + codec, applicator, SettingsPanel INPUT section, conflict UI, safe mode | P1 (contract), P5 (so joypad defaults are visible truth) | E9/E10 recovery tests; persistence across restart probe |
| **P7 Docs & help truth** | Correct the two drifting docs; generate `docs/ui/KEYBOARD.md` from the contract; guidance/help consume `GetActionPrompt` | P6 (so the map shows rebindable truth) | generator `--check` in CI; docs match gate output byte-for-byte |
| **P8 Close-sniff sweep** | Mechanical migration of remaining ~100 close-only `_UnhandledInput` overrides to the shared contract, ≤20 files per tranche; ExpeditionPanel GrabFocus lines coordinated with the Plan 24 integrator | P2–P3 | gate + lifecycle green per tranche; zero raw-`Key.Escape` close sniffing left outside the contract |

Estimated focused-test budget per TEST_POLICY: P1 ≈ 10 cases; P2 ≈ 8; P3 ≈ 10;
P6 ≈ 8; selftest extensions additive to existing harnesses. No phase exceeds
the 100-case builder norm.

---

# 20. File Impact Map

| File | Change | Reason |
|---|---|---|
| `src/Host/AshfallInputActions.cs` | +nav predicates, +`InputActionContract`, −`IsConfirmOrAccept`, −`EnsureActionsRegistered`, −`ReconcileCollisions`, fix NavDown default | single vocabulary owner; delete dead/unreachable code (§6.4) |
| `src/Main.Application.cs` | absorb GameFlow Esc branch; route nav events to navigator; route-parity one-liners | one dispatch owner (D2) |
| `src/Main.GameFlow.cs` | delete `_UnhandledInput` close branch; open-path focus hook | one dispatch owner; focus lifecycle |
| `src/Main.PanelLifecycle.cs` | teardown helper: scope-stack clear + focus restore | D3, F2/F3/F15 |
| `src/UI/ModalManager.cs` | internals over Core `ModalStackController`; trap/nav/confirm in `HandleInput` | adopt dormant tested authority (§2.2.1) |
| `src/UI/IModalPanel.cs` | unchanged (contract already sufficient) | — |
| `src/UI/AshfallFocusPolicy.cs` | unchanged helpers; gains callers | wiring, not rewrite |
| `src/UI/AshfallFocusNavigator.cs` | new (~150 lines) | directional/stick focus movement |
| `src/UI/AshfallUiHelpers.cs` | `MakeButton` applies focus style (one line) | factory-level visible focus |
| `src/UI/GameDashboardPanel.cs` | nav-rail registers fallback focus target | F2 fallback home |
| 5 modal files | route close/focus through ModalManager | P2 |
| `src/UI/SettingsPanel.cs` | +INPUT & CONTROLS section (~200 lines) | existing settings surface (D6) |
| `Assets/Ashfall.Core/Settings/UserSettingsData.cs` | +`key_bindings`, Clone, v2 | sole preference authority |
| `Assets/Ashfall.Core/Settings/UserSettingsCodec.cs` | v2 emit/accept + recovery rows | codec discipline |
| `src/Settings/KeyBindingApplicator.cs` | new (~120 lines) | single InputMap writer besides project.godot |
| `project.godot` | +joypad events (bounded subset), +Space on confirm | shipped binding truth |
| `src/UI/OnboardingHintPanel.cs`, `src/UI/TutorialPanel.cs` | consume `GetActionPrompt` where keys are hardcoded | D7 |
| `src/UI/ExpeditionPanel.cs` | remove 2 ad hoc GrabFocus lines (**P8, coordinated with Plan 24 claim**) | contract compliance |
| ~100 panel files | close-sniff migration, tranches (P8) | D2 completion |
| `Ashfall.Core.Tests/Tooling/InputMapContractTests.cs` | new | §18.1 |
| `Ashfall.Core.Tests/Settings/UserSettingsRecoveryTests.cs` | extend (or sibling new file) | F7 |
| `src/Host/UiAccessibilitySelfTest.cs`, lifecycle/uitest hosts | probe extensions | §18.2 |
| `scripts/ci/input-map-gate.sh`, `docs/ci/CI_GATE_MANIFEST.json` | new gate + row | D1 permanence |
| `scripts/ci/generate-keyboard-map.py`, `docs/ui/KEYBOARD.md` | new generated doc | D8 |
| `docs/ui/INPUT_AND_NAVIGATION_AUDIT.md`, `docs/ACCESSIBILITY.md` | bounded corrections | §4.3 |
| `docs/qa/AUDIO_AND_SETTINGS_RECOVERY_SMOKE_TEST.md` | +E9/E10 + input checklist | §18.4 |

Explicitly untouched: `Assets/StreamingAssets/Data/*`, all save stores and
`SaveOrchestrator`, `CampaignRngManager`, `Theme.cs`, `AccessibilityPresentation.cs`,
`ColorblindColorMapper`, audio manager/cooldowns, every Core simulation system.

---

# 21. Risks

| Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|
| Focus wiring changes click-path behavior somewhere subtle (e.g. a panel that relied on *no* initial focus) | Medium | Medium | P4 limited to 10 surfaces with probes; P8 tranches ≤20 files with gate green between; any behavioral surprise reverts per-file (contract is additive) |
| Freed-node focus crash class surfaces during migration | Medium | High (crash) | F2/F3 probes land in P2/P3 *before* the sweep; restore guard is validity-checked |
| Rebinding schema v2 mishandles a real user's v1 file | Low | Medium | recovery tests first (tests precede completion claim); v1→empty-map is the no-op path; atomic writes already exist |
| Snapshot drift from factory-level focus style (ring on every button) | High | Low–Medium | expected, bounded: rebaseline only the touched snapshot targets in P3 with renderer-capable session (known environment constraint from Plan 24 closeout); unrelated drift = failure |
| Controller support over-promised in docs/store copy | Low | Medium | §15.1 boundary is the honesty record; `ACCESSIBILITY.md` correction names exactly what pad coverage exists |
| Conflict with active `claim-xp-wave1-difficulty` or Plan 24 claim paths | Low | Low | path sets are disjoint except `ExpeditionPanel.cs` (2 lines, P8, explicitly coordinated) and governance rows (integrator-written) |
| Gate false-positives blocking unrelated PRs | Medium | Low | gate ships with a seeded-orphan fixture proving it fails for the right reason; error messages name action/rule/file |

---

# 22. Out of Scope

- Screen-reader / platform AT support (engine-blocked; Plan 80 §7 stands).
- Joypad *rebinding* UI (v2 storage leaves room; the capture surface is
  keyboard-first; pad defaults are authored, not user-editable, in this wave).
- Mouse-emulation cursor on gamepad, radial menus, gyro, vibration (off-by-default
  vibration isn't built at all — nothing to toggle).
- Per-panel font-size refactor (Plan 184's central scale lever already exists;
  §2.1 row #9).
- The "go to route" palette (source plan 37B step 8): valuable, deferred as a
  follow-up proposal once the focus spine lands — it is a feature, not a
  repair, and this package is already at its safe size.
- Localization of the keyboard map beyond keyed-string discipline (l10n gates
  already cover string extraction).
- Any change to gameplay hotkey *semantics* (what J or F1 do) — routes stay
  as shipped.

---

# 23. Rollback Strategy

- Phases are independently revertible: P1 is additive+deletion-of-dead-code
  (revert = restore file); P2–P3 keep every panel's existing local behavior
  until its tranche migrates (the shared contract runs alongside, never
  instead-of, until P8).
- The input-map gate can be demoted from `critical: true` to diagnostic by a
  one-field manifest edit if it false-positives during stabilization — with a
  written reason per the quarantine rules, and re-raised only with a passing
  focused target.
- Settings v2: codec accepts v1 forever; deleting `key_bindings` from a v2
  file degrades to defaults — no user action can brick startup (safe-mode boot
  is the final net).
- `project.godot` joypad rows are data; reverting them restores keyboard-only
  behavior with zero code change.
- No save data, catalog data, or simulation state is ever at risk — rollback
  cannot strand a campaign.

---

# 24. Definition of Done

1. Gate 54 `input_map_contract` is fast-tier critical and green; a seeded
   orphan/predicate/route violation demonstrably fails it.
2. All 22 declared actions dispatch to a verified outcome or are removed from
   the map; zero uncalled predicates; zero dead registration code.
3. One global dispatch owner; one close owner; Esc with an open overlay closes
   the overlay and nothing else (F13 probe).
4. Dashboard + top-10 surfaces: deterministic initial focus, Tab trap, arrow
   navigation, visible ring, restore-to-opener on close — asserted in the
   extended selftests.
5. The scripted mouseless-day journey passes headless.
6. Joypad bindings shipped per §15.1; headless joypad probe navigates and
   confirms; bounded scope documented.
7. Rebinding: persists across restart, rejects scoped conflicts, resets per
   action and globally, recovers from corruption, safe-mode boot works.
8. `docs/ui/KEYBOARD.md` generated and `--check`-gated; the two drifting docs
   corrected; guidance/help show live bindings.
9. `ui_accessibility`, `panel_bind_lifecycle`, `panel_route_gate`,
   `player_surface_coverage` gates green; no new shutdown warnings beyond the
   classified known-benign signature; leak diagnostics show no monotonic
   growth on migrated panels.
10. No Core engine references added; no save sections; no data catalogs; no
    new RNG; focused-test ledger per phase reported per TEST_POLICY.

---

# 25. Implementation Handoff

## MUST PRESERVE

- `Main.Application._UnhandledKeyInput`'s existing hotkey branches and their
  `GameState.Playing` gates — routes and semantics are shipped truth.
- Plan 80/184 authorities: `AshfallFocusPolicy` class shape, Theme contrast
  floors, `AccessibilityPresentation` predicates, `ColorblindColorMapper` —
  wire them, don't edit their policy.
- Core engine-freedom: no Godot references in `Assets/Ashfall.Core/`; the only
  Core touch is one additive settings DTO field + codec version.
- `UserSettingsStore` atomic-write + recovery discipline; settings stay
  independent of campaign saves.
- Keyboard/controller close/back behavior, focus visibility, and contrast per
  AGENTS.md §UI; `SetInputAsHandled` exactly-once per consumed event.
- The `IsEcho()` guard at the dispatcher head and the holdfast B/S
  context-scoping precedent.
- All unrelated dirty worktree files listed in `git status` (Seal-steps/*,
  docs/plans/BLOCKED_PLANS_UNBLOCKER_PLAN_2026-09-19.md) — untouched.

## MUST ADD

- `InputActionContract` table + nav predicates; delete `IsConfirmOrAccept`,
  `EnsureActionsRegistered`, `ReconcileCollisions`; fix `NavDown` default.
- Gate 54 (`scripts/ci/input-map-gate.sh` + manifest row + xUnit mirror).
- Focus wiring: open hook, teardown restore, navigator, factory focus ring.
- `ModalManager` rebuilt over Core `ModalStackController`; modals routed.
- `key_bindings` (v2) + `KeyBindingApplicator` + SettingsPanel INPUT section
  + safe-mode boot + conflict UI.
- `project.godot` joypad rows per §15.1 (+ Space on confirm).
- Selftest probes: a11y Gates 6–7, lifecycle freed-node/session-swap probes,
  top-10 initial-focus assertions, mouseless-day journey, joypad probe.
- `docs/ui/KEYBOARD.md` generator + `--check`; corrections to
  `INPUT_AND_NAVIGATION_AUDIT.md` and `ACCESSIBILITY.md`; QA doc E9/E10 +
  input checklist.

## MUST NOT DO

- No second input framework, no `InputManager` singleton, no parallel action
  vocabulary, no new Core focus abstraction.
- No per-panel `_UnhandledInput` close sniffing added anywhere new; no
  raw-`Key.Escape` outside the contract after P8.
- No controller scope creep (no cursor emulation, radial menus, vibration,
  pad rebinding UI).
- No campaign-save, catalog-JSON, RNG, or simulation contact; no new save
  section; no `System.Random`.
- No store-page claims of screen-reader/AT support; no full-suite runs without
  a dedicated window; no mass-format of `src/UI/`.
- Do not touch `src/UI/ExpeditionPanel.cs` before P8 coordination with the
  Plan 24 claim owner; do not write governance ledgers (foreman/integrator).
- Do not re-propose the Guidance action or reopen the F1/F2 decision (Plan 17B
  owns it); do not treat `INPUT_AND_NAVIGATION_AUDIT.md`'s gamepad table as
  evidence of shipped behavior — it is drift to correct.

## VERIFY WITH

- `bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/InputMapContractTests.cs` (new file, alone first)
- `bash scripts/run_test.sh Ashfall.Core.Tests/Settings/UserSettingsRecoveryTests.cs`
- `bash scripts/run_test.sh Ashfall.Core.Tests/UI/` (route gate + modal stack neighbors)
- `dotnet build Ashfall.csproj` — 0 errors, 0 new warnings
- `bash scripts/ci/input-map-gate.sh` (new gate 54, incl. seeded-failure fixture)
- `godot --headless --path . -- --ui-accessibility-selftest` (Gates 1–7)
- `godot --headless --path . -- --panel-bind-lifecycle-selftest` (zero new warnings)
- `godot --headless --path . -- --player-panels-uitest` (top-10 focus assertions)
- `godot --headless --path . -- --playable-shell-selftest` (mouseless-day journey once extended)
- `python3 scripts/ci/generate-keyboard-map.py --check`
- Manual: `docs/qa/AUDIO_AND_SETTINGS_RECOVERY_SMOKE_TEST.md` E9/E10 + pad checklist

## FIRST SAFE IMPLEMENTATION STEP

Land Phase 1 exactly as scoped: add the four `IsNav*` predicates and the
22-row `InputActionContract` to `src/Host/AshfallInputActions.cs`, delete the
three dead members (`IsConfirmOrAccept`, `EnsureActionsRegistered`,
`ReconcileCollisions`), correct `CanonicalDefaults[NavDown]` to `Key.Down`,
write `Ashfall.Core.Tests/Tooling/InputMapContractTests.cs`, run it alone via
`bash scripts/run_test.sh`, then wire `scripts/ci/input-map-gate.sh` + the
manifest row and prove the gate fails on a seeded orphan fixture and passes on
HEAD. No panel, modal, settings, or `project.godot` change happens before this
contract-and-gate phase is green — every later phase is measured against it.
