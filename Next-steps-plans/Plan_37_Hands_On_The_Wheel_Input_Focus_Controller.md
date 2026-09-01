# Plan 37 — Hands on the Wheel: Input, Focus, and Controller Reality

> **Wave:** Continuity Wave 5 — *The Human Interface*
> **Depends on:** Wave 1's 17B (the guidance overlay this plan gives a key), Wave 3's 25A/25B (keyed
> strings so rebinding labels survive localization).
>
> **Theme:** the project defines **21 input actions**, including four directional navigation
> actions — and there is no focus system to navigate. `FocusMode`/`MoveFocus` appear in essentially
> none of the 164 UI files; focus is grabbed in two places (`MainMenuPanel`, `ModalManager`). No
> controller bindings exist at all. Three of thirteen input helper predicates are never called. For
> a dense management game played on a fixed 1920×1080 canvas with 135 routes, keyboard/controller
> parity is not an afterthought — it is the difference between "playable" and "inspectable".

---

## Evidence Inventory (re-verified @ `ccac926e`)

| # | Fact | Evidence |
|---|---|---|
| 1 | 21 actions defined | `project.godot [input]`: `ashfall_close`, `ashfall_confirm`, `ashfall_next_tab`, `ashfall_nav_up/down/left/right`, `ashfall_help`, `ashfall_journal`, `ashfall_journal_tab_1..5`, `ashfall_events`, `ashfall_expeditions`, `ashfall_forecast`, `ashfall_weather_history`, `ashfall_holdfast`, `ashfall_holdfast_status`, `ashfall_holdfast_build` |
| 2 | A typed wrapper exists (good) | `src/Host/AshfallInputActions.cs` — 13 `Is…` predicates (`:138–207`), e.g. `IsCloseOrCancel`, `IsConfirm`, `IsNextTab`, `IsJournal`, `IsHelp` |
| 3 | …but **3 are never called** | `IsConfirmOrAccept`, `IsExpeditions`, `IsHoldfast` → 0 call sites each |
| 4 | Coverage is thin and single-dispatch | most predicates have exactly **1** caller (concentrated around `src/Main.GameFlow.cs:667`); `IsCloseOrCancel` has 4 incl. `src/Host/HoldfastTerminalPanel.cs:335` |
| 5 | **Directional nav actions have no handler** | no `ashfall_nav_*` constant is referenced by any `Is…` predicate, and grep for the literals in `src/` outside `AshfallInputActions.cs` → nothing |
| 6 | **No focus order to navigate** | `grep "FocusMode\|MoveFocus" src/UI/*.cs` → **0**; `GrabFocus` only in `src/UI/MainMenuPanel.cs:62,66` and `src/UI/ModalManager.cs:195,204,213` |
| 7 | **No controller support** | `grep -c "InputEventJoypad" project.godot` → **0** bindings |
| 8 | Rebinding has no surface | settings layer is `src/Settings/UserSettings.cs` (`UserSettingsStore` with audio/etc.) — no key-rebinding UI, and Wave 4's 34B adds difficulty into the same store |
| 9 | Text scale is per-widget constants | `Theme.FontSizeBody/H1` overrides are sprinkled per panel (e.g. `AchievementsPanel.cs:97,105,127`, `AfflictionsPanel.cs:254,262`) — no user-facing size control, so low-vision players have no lever |
| 10 | Fixed-viewport scaling | `project.godot`: `viewport 1920×1080`, `stretch/mode="canvas_items"`, `aspect="keep_height"` — legible at large windows, but text size does not scale independently of the window (see `ashfall-input-map-audit`, `ashfall-ui-access`) |
| 11 | The guidance overlay is unrouteable (Wave 1's 17B) | `OnboardingHintPanel` has no open route; `ashfall_help` is *defined and its predicate is called once* — the key exists, the panel doesn't open |
| 12 | Audio/settings recovery is documented but manual | `docs/qa/AUDIO_AND_SETTINGS_RECOVERY_SMOKE_TEST.md` exists as a checklist, not a gate |

**Reading:** no new input *concept* is needed. The action map, a typed wrapper, and one dispatcher
already exist; the missing third of the layer is focus order, controller bindings, a rebinding
surface, and a gate that keeps the action map and the handlers from drifting apart again (which they
already have).

---

## Task 37A — Make every declared action actually do something

**Goal:** one dispatcher, no orphans, and a gate proving the action list and the handler list match.

**Files:** `project.godot [input]`, `src/Host/AshfallInputActions.cs`, `src/Main.GameFlow.cs`
(dispatch region), `src/Main.UiPanels.cs`, `src/Main.PanelLifecycle.cs`, `src/UI/ModalManager.cs`,
`src/Main.Onboarding.cs` (guidance route), new `Ashfall.Core.Tests/InputMapContractTests.cs`,
`scripts/ci/generate-cli-catalog.sh` sibling or new `scripts/ci/input-map-gate.sh`,
`docs/ui/*` + `docs/CI.md`.

### Substeps

1. **Enumerate reality**: for each of the 21 actions — predicate exists? dispatcher handles it?
   target panel reachable? Produce the table; three predicates and four nav actions already fail it.
2. **Decide the disposition of each orphan** rather than deleting blindly: `IsExpeditions` /
   `IsHoldfast` presumably should route to `expeditions` / `holdfast` panels (routes exist);
   `IsConfirmOrAccept` is either used by modal default-buttons or deleted. Record each decision in
   the table.
3. **Centralise unhandled-key dispatch** in one place (a single `_UnhandledInput`/shortcut owner)
   instead of panels each sniffing input — `HoldfastTerminalPanel.cs:335` sniffing close/cancel
   locally is exactly how routes become inconsistent.
4. **Give every hotkey a visible affordance**: tooltips and the guidance overlay must show the key
   (`ashfall_ui` conventions), otherwise the layer is undiscoverable by design.
5. **Wire `ashfall_help` to the guidance overlay** (Wave 1's 17B) — the key exists today and opens
   nothing; this is the single highest-value one-liner in the plan.
6. **Route the remaining hotkeys through `PanelRegistry`** so an action's target is validated against
   the registry (`PanelRouteGateTests` already proves routes exist — extend it to prove
   action→route targets resolve too).
7. **Add an input-map gate**: fail CI when (a) an action exists in `project.godot` with no handler,
   (b) a predicate exists with no caller, or (c) a handler references an undeclared action. Tier-1
   critical, `expected_summary` in the manifest format.
8. **Conflict detection**: no two actions bound to the same key in the same context, and no shadowing
   of Godot's `ui_*` defaults — run `ashfall-input-map-audit` as the review procedure and record its
   output.
9. **Repeat-key and held-key behaviour**: define debouncing so `Tab` spam doesn't open five modals;
   respect the existing cooldown discipline in the audio layer (17C) so hotkeys don't machine-gun
   cues.
10. **Modal correctness**: `ModalManager` grabs focus (`:195,204,213`) — add "close returns focus to
    the opener" so keyboard users don't lose their place, and verify Escape semantics match
    `ashfall_close` everywhere.
11. **Tests**: one test per action asserting "pressed → intended state change", an orphan-detection
    unit test, and a headless input-injection probe in the existing UI-test composition root.
12. **Run the checklist** + `verify-fast.sh`.

**DoD:** every declared key does something, does it once, and says so on screen.

---

## Task 37B — Focus order and keyboard navigation: make the nav keys navigate

**Goal:** a player who never touches the mouse can complete a full campaign day.

**Files:** `src/UI/*` (164 files), `src/UI/AshfallUiHelpers.cs` (button/row factories),
`src/UI/ModalManager.cs`, `src/UI/GameDashboardPanel.cs` (`AddNavButton:570–580`),
`src/Host/AshfallInputActions.cs`, `project.godot` (nav action handlers),
`docs/ui/UI_PANEL_ARCHITECTURE_GUIDE.md`, `docs/ui/DESIGN_SYSTEM_RULES.md`.

### Substeps

1. **Set focus policy in the shared factory, not per panel**: `AshfallUiHelpers.MakeActionButton` /
   `MakeDataRow` decide `FocusMode` — interactive = `All`, decorative labels = `None`. One change,
   whole-game effect (the same "fix it in the factory" lesson as 17C's UI cues).
2. **Define reading order per panel**: top-to-bottom, left-to-right, grouped by section, with
   `MoveFocus`/tab-order wired to the existing nav actions (currently bound to nothing, rows 5–6).
3. **Arrow keys navigate lists**, not just Tab: roster, expedition party, inventory grid, duty
   roster, map node list, and the briefing entries (Wave 4's 31B adds click-through; keyboard needs
   the same affordance).
4. **Visible focus**: a focus style in the design system that survives the graphite/brass palette and
   passes contrast — plus a "no colour-only" rule (the focus ring must also change shape/weight).
5. **Grid navigation**: the fixed 1920×1080 dashboard is a grid; define spatial navigation so
   left/right in a row stays in the row (Godot's `ui_left/…` defaults exist but are overridden by
   custom actions — reconcile the two).
6. **Deep panels first**: instrument the 10 highest-traffic surfaces (dashboard, survivors,
   expeditions, inventory, crafting, medical, trade, duty roster, map, briefing) and validate a
   mouseless day on those, before sweeping all 164 files.
7. **Every mouse action has a key**: the 30 consoles from Wave 1's 16A verdict either get keys or
   aren't player-facing — no dead ends for keyboard users.
8. **Search/jump**: with 135 routes, a "go to…" palette bound to a single key is the cheapest
   discoverability win; it reuses `PanelRegistry` and needs no new navigation concept.
9. **Focus restoration across rebind/session swap**: after load or new game (Wave 1's 16B/16C), focus
   must not land on a freed node — a real crash class in Godot when a freed control holds focus.
10. **Screen-reader/assistive parity**: accessible names for icon-only buttons (`icon_shock_war.png`
    style assets exist), and a text equivalent for every colour-coded status.
11. **Tests**: a mouseless journey test (new game → assign → craft → dispatch → advance day →
    briefing → open source panel), per-panel focus-order unit probes, and a freed-node focus-safety
    test after session swap.
12. **Docs**: keyboard map generated into `docs/ui/KEYBOARD.md` (generated, never hand-edited —
    Wave 3's 29A rule).
13. **Run the checklist** + `ashfall-ui-access` + snapshot review at 1280×800.

**DoD:** a full campaign day is completable without a mouse, and the key map is a generated document.

---

## Task 37C — Controller, rebinding, and user-controlled legibility

**Goal:** gamepad support, user rebinding, and text/UI scale controls — the three things a store
release is expected to have and this project currently cannot offer.

**Files:** `project.godot [input]` (joypad events), `src/Host/AshfallInputActions.cs`,
`src/Settings/UserSettings.cs`, new `src/UI/SettingsPanel.cs` (or the existing settings surface —
`MainMenuPanel` brief modal per `docs/ui/SURFACE_GAP_REPORT.md`), `docs/ui/DESIGN_SYSTEM_RULES.md`,
`docs/qa/AUDIO_AND_SETTINGS_RECOVERY_SMOKE_TEST.md`, `scripts/ci/verify-fast.sh` neighbours.

### Substeps

1. **Decide the controller scope honestly**: a management UI on a pad needs radial/grid navigation and
   a cursor-like mode, not button remaps of everything. Bound the target (menus, navigation, confirm,
   cancel, hotkeys) and record it — a half-controller mode is worse than none.
2. **Add joypad bindings** to the existing 21 actions (currently 0 `InputEventJoypad` entries) rather
   than a parallel input path.
3. **Rebinding surface**: list every action, accept a key or button press, detect conflicts (37A step
   8), persist through `UserSettingsStore`, and offer per-action and global reset.
4. **Text/UI scale**: expose a scale control that changes the theme's font sizes centrally (not
   per-panel `AddThemeFontSizeOverride` sprinkles — 35 of which appear in just the two sampled
   panels), then re-verify overflow on the top 10 surfaces and the snapshot suite.
5. **Persist and migrate**: new settings keys need defaults + a versioned settings file so a
   corrupted or old settings file degrades cleanly (same envelope discipline as saves; the store
   already reports `HasDiagnosticError`).
6. **Deadzone/latency for stick navigation**, and a distinct "focus" vs "grid cursor" mode so lists
   don't fight the map.
7. **Vibration optional and off by default**, respecting accessibility expectations.
8. **Legibility set**: colour-independent status (already required by Wave 3's 25B step 12), flashing
   limits (photosensitivity), captions for the VO cues produced in 7B, and a "reduce motion" toggle
   for animation/loop start-stop.
9. **Input help screen**: the guidance overlay (17B) shows the current, live bindings — generated
   from the action map, not hand-typed, so rebinding updates the help automatically.
10. **Recovery**: safe-mode boot (hold a key to start with default bindings/scale) — the settings
    equivalent of a corrupted-save fallback, and the thing that makes support tickets survivable.
11. **Tests**: binding persistence, conflict rejection, scale-change overflow probes on the top-10
    panels, controller navigation smoke in the headless UI harness (synthesize joypad events),
    safe-mode reset.
12. **Manual QA**: extend `docs/qa/AUDIO_AND_SETTINGS_RECOVERY_SMOKE_TEST.md` into a real
    input/accessibility checklist and attach the run.
13. **Run the checklist** + `verify-fast.sh` + `ashfall-ui-access`.

**DoD:** pad + keyboard + mouse all complete a day; bindings and text size belong to the player.

---

## Cross-Task Dependencies

```
25A/25B (keyed strings) ──► 37A step 4 & 37C step 9 (labels must be keys, not literals)
17B (guidance overlay)   ──► 37A step 5 (ashfall_help finally opens something)
16A (live panel verdict)  ──► 37B step 7 (only live surfaces get keys)
31B (click-through lines) ──► 37B step 3 (keyboard equivalent required)
28A (manifest)            ──► 37A step 7 (action→route validation belongs to the same table)
   37A (orphans + gate) ──► 37B (focus order) ──► 37C (pad/rebind/scale)
```

**Execution order:** 37A → 37B → 37C. Do not add controller bindings (37C) before focus order
exists (37B) — a pad with nothing to move focus between is a D-pad-shaped disappointment.

---

## Verification Checklist (per task)

```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
2. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
3. dotnet build Ashfall.csproj                                   # 0 errors, 0 warnings
4. godot --headless --path . -- --data-integrity-selftest        # 0 errors
5. godot --headless --path . -- --bridge-selftest                # exits 0
6. bash scripts/ci/input-map-gate.sh                             # (new, 37A step 7)
7. godot --headless --path . -- --player-panels-uitest           # focus/hotkey probes
8. godot --headless --path . -- --playable-shell-selftest        # mouseless day
9. ashfall-ui-access + ashfall-snapshot-diff (scale variants)
10. bash scripts/ci/verify-fast.sh
```

---

## Estimated Effort & Risk

| Task | Files | New gates | Tests | Difficulty | Regression risk |
|---|---|---|---|---|---|
| 37A | ~8 | 1 (input map) | 6–10 | Low–Med | LOW |
| 37B | factory + top 10 panels, then sweep | 0 | 8–12 | Medium | MEDIUM (focus can crash if freed — step 9) |
| 37C | settings + presets + theme | 0 | 8–12 | Medium–High | MEDIUM (scale changes churn snapshots) |

**Guardrails:** no new input framework, no per-panel input sniffing, no parallel action vocabulary
(everything goes through `AshfallInputActions`), no controller scope creep beyond menus/navigation,
and never ship a declared action that does nothing — that is a false affordance with a keybind
attached, the same sin as Wave 1's 30 consoles.
