# C2 — Flagship Integration Plan [15]: Input Reality, Focus Navigation, Controller Parity, Rebinding, and Legibility

> **Deliverable:** `C2_planintegration[15].md`
> **Source scope:** Plan 37 — *Hands on the Wheel: Input, Focus, and Controller Reality*
> **Wave:** Continuity Wave 5 — *The Human Interface*
> **Primary objective:** make every declared input action real, centralize input dispatch, establish deterministic focus order across live player surfaces, prove a full campaign day is completable without a mouse, then add controller bindings, user rebinding, text/UI scaling, safe settings recovery, and generated live input help.
> **Required execution order:** **37A → 37B → 37C**
> **Hard gate:** do not add controller bindings before keyboard/focus navigation is functional; a controller cannot be considered supported until focus has valid targets and movement rules.
> **Dependencies:** Plan 17B guidance overlay, Plan 25A/25B localization/keyed strings, Plan 16A live-panel verdicts, Plan 31B click-through interaction parity, Plan 28A manifest/action→route validation.
> **Scope discipline:** no new input framework, no per-panel shortcut sniffing, no parallel action vocabulary outside `AshfallInputActions`, no controller scope creep beyond the deliberately supported management-UI interactions, and no declared input action that lacks a real handler.

---

# 0. Executive Intent

ASHFALL already has the beginning of a coherent input layer:

- a Godot input map,
- named `ashfall_*` actions,
- a typed `AshfallInputActions` wrapper,
- a central game-flow input dispatcher,
- modal focus capture,
- a guidance/help action,
- fixed-layout management UI.

But the implementation is incomplete in exactly the places that determine whether the game is actually operable without a mouse:

- directional navigation actions exist but navigate nothing,
- focus order is effectively absent,
- some typed input predicates have no caller,
- controller bindings do not exist,
- rebinding has no user surface,
- text scaling is not centralized,
- the help action does not reach the guidance overlay,
- focus restoration across modals/session replacement is not a contract,
- action-map and handler-map drift is not gated.

The result is an interface that can be inspected but cannot yet be treated as a coherent keyboard/controller product.

This plan turns input into a product contract.

The intended architecture is:

```text
project.godot input actions
        │
        ▼
AshfallInputActions
        │
        ▼
single global input dispatcher
        │
        ├─ global routes / hotkeys
        ├─ modal semantics
        ├─ focus navigation
        └─ action→PanelRegistry route validation
                │
                ▼
shared focus policy in UI factories
                │
                ├─ tab order
                ├─ arrow/grid navigation
                ├─ focus restoration
                └─ visible focus state
                        │
                        ▼
keyboard-complete campaign day
                        │
                        ▼
joypad bindings on same actions
                        │
                        ▼
rebinding + text/UI scale + accessibility settings
                        │
                        ▼
generated live input-help surface
```

The flagship outcome is:

> **Keyboard, mouse, and supported controller input all operate the same live player surfaces through the same action vocabulary; every action has a handler, every interactive surface has focus order, every binding is discoverable and rebindable, and a full campaign day can be completed without touching the mouse.**

---

# 1. Source Diagnosis

The source plan establishes:

- 21 declared input actions.
- A typed wrapper with 13 predicates.
- Three predicates with no callers.
- Four directional navigation actions with no handler.
- Essentially no UI-wide focus policy.
- Focus grabbing only in a couple of places.
- No joypad bindings.
- No rebinding UI.
- No independent text-size setting.
- Fixed viewport scaling that does not solve low-vision legibility.
- `ashfall_help` exists but does not open the intended guidance surface.
- settings recovery exists only as a manual checklist.

The architectural conclusion is:

```text
no new input concept is required
```

The missing pieces are:

```text
dispatch completeness
+ focus topology
+ controller bindings
+ user-owned bindings
+ user-owned scale
+ regression gates
```

---

# 2. Program-Level Success Criteria

C2[15] is complete only when:

1. Every declared `ashfall_*` action has one intentional disposition.
2. Every retained action has a typed wrapper and at least one live handler.
3. Every global action routes through one central dispatcher.
4. No live panel sniffs global shortcuts independently.
5. `ashfall_help` opens the real guidance overlay.
6. Every live player-facing route supports keyboard focus.
7. Directional navigation actions move focus.
8. Modal close restores focus to the opener.
9. A New Game → assign → craft → dispatch → advance → briefing journey succeeds without mouse input.
10. Controller bindings use the same action vocabulary.
11. User rebinding persists and detects conflicts.
12. Text/UI scale is centralized and persists.
13. Safe-mode settings recovery can restore default bindings/scale.
14. Generated help reflects current bindings automatically.
15. The input-map gate prevents orphan actions, orphan predicates, undeclared handlers, and route mismatches.

---

# 3. Architectural Invariants

## 3.1 One action vocabulary

All gameplay/global input uses:

```text
ashfall_*
```

through `AshfallInputActions`.

No second controller-only or panel-specific action namespace.

## 3.2 One global dispatcher

Global shortcuts are handled centrally.

Panels should receive routed actions/state, not individually inspect arbitrary input.

## 3.3 Local widgets may handle local semantics only

Acceptable local handling:

- text editing,
- list-local focus,
- modal-local confirm/cancel.

Not acceptable:

- independent global journal/help/expedition hotkey sniffing.

## 3.4 Focus is a UI architecture concern

Shared factories establish default focusability.

Panels only customize explicit navigation where layout requires it.

## 3.5 Controller support depends on focus support

Joypad bindings are not shipped until 37B proves focus topology.

## 3.6 Rebinding updates help automatically

No manually typed key labels.

## 3.7 Key labels are localized presentation

Action IDs remain stable; displayed key/button labels use Plan 25.

## 3.8 Text scale is centralized

No user scale feature implemented by editing hundreds of per-widget font overrides independently.

## 3.9 Focus restoration is lifecycle-safe

No freed control remains the focus return target after session replacement.

## 3.10 Declared action means real affordance

A keybind that does nothing is a product bug and CI failure.

---

# 4. Dependency Graph

```text
17B guidance overlay ──────────────► 37A help route
25A/25B localization ─────────────► live binding labels / help
16A live-panel verdict ───────────► keyboard/controller coverage scope
31B click-through interaction ────► keyboard equivalent
28A manifest / panel registry ────► action→route validation

37A — action completeness
 │
 ▼
37B — focus topology + mouseless day
 │
 ▼
37C — controller + rebinding + scale
```

Required order:

```text
37A → 37B → 37C
```

---

# 5. Baseline Capture

Before edits, capture input/focus reality.

## 5.1 Input action inventory

For each action record:

| Action | Declared | Wrapper predicate | Predicate caller | Dispatcher handler | Target route | Key binding | Joypad binding |
|---|---:|---:|---:|---:|---|---|---|

## 5.2 Focus inventory

For each top/live panel record:

- interactive controls,
- `FocusMode`,
- initial focus,
- tab order,
- directional neighbors,
- modal behavior,
- close focus restore,
- session-swap safety.

## 5.3 Settings inventory

Record:

- settings schema/version,
- persistence path,
- current recovery behavior,
- audio/display settings patterns,
- current per-widget font overrides.

## 5.4 Baseline verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --player-panels-uitest
godot --headless --path . -- --playable-shell-selftest
bash scripts/ci/verify-fast.sh
```

Record current failures/coverage rather than assuming the eventual commands already pass.

---

# 6. Task 37A — Every Declared Action Does Something

## Goal

Create one complete action→predicate→dispatcher→route/state-change chain and gate it.

---

# 7. 37A.1 — Publish the Action Contract Table

Create one row for every action:

| Action ID | Wrapper | Handler | Scope | Target | Route live? | Repeat policy | Binding label |
|---|---|---|---|---|---:|---|---|

No action remains unexplained.

---

# 8. 37A.2 — Orphan Disposition

For each orphan:

```text
WIRE
or
REMOVE
```

Examples from source:

- `IsExpeditions` → likely route to expeditions.
- `IsHoldfast` → likely route to holdfast.
- `IsConfirmOrAccept` → wire to modal/default action if useful, otherwise remove.

Do not preserve unused predicates “just in case.”

---

# 9. 37A.3 — Single Dispatcher Ownership

Move global input handling into one owner.

Preferred host/main seam:

```text
_UnhandledInput
```

or existing central shortcut dispatch region.

The dispatcher owns:

- global navigation shortcuts,
- help,
- journal,
- expeditions,
- holdfast,
- forecast/history,
- events,
- tab switching,
- global close semantics.

---

# 10. 37A.4 — Remove Panel-Level Global Sniffing

Audit panels for direct:

```text
Input.IsAction*
event.IsAction*
```

on global actions.

Refactor global cases into dispatcher.

Allow documented local-widget behavior.

---

# 11. 37A.5 — Wire Help to Guidance Overlay

`ashfall_help` must open/reopen the Plan 17B guidance surface.

Requirements:

- reachable from live shell,
- modal/focus-safe,
- shows current bindings,
- route validated.

This is a high-priority acceptance check.

---

# 12. 37A.6 — Route Hotkeys Through `PanelRegistry`

Global panel actions resolve targets through canonical route IDs.

Do not directly instantiate/open panel classes from input logic.

Gate:

```text
action target route exists
AND
route is PlayerNavigable/live
```

---

# 13. 37A.7 — Visible Shortcut Affordances

Expose shortcuts via:

- tooltips,
- button hints,
- guidance overlay,
- generated keyboard help.

Displayed binding comes from the live input map.

No hardcoded `"[J]"` strings that become stale after rebinding.

---

# 14. 37A.8 — Input Map CI Gate

Create:

```text
scripts/ci/input-map-gate.sh
```

or equivalent generated checker.

Fail when:

1. action declared but no handler/disposition,
2. wrapper predicate exists but no caller/disposition,
3. handler references undeclared action,
4. action targets missing/dead route,
5. duplicate/conflicting global bindings where prohibited.

Register in CI manifest as critical.

---

# 15. 37A.9 — Conflict Rules

Define contexts:

- GLOBAL,
- MODAL,
- PANEL_LOCAL,
- TEXT_ENTRY.

Bindings conflict only where simultaneously active.

Detect:

- same key/action collision,
- shadowing of intended Godot UI defaults,
- impossible confirm/cancel overlaps.

---

# 16. 37A.10 — Godot `ui_*` Reconciliation

Decide relationship between custom nav actions and Godot defaults.

Preferred:

```text
custom `ashfall_nav_*`
→ intentional mapping to focus movement
```

Do not leave two competing directional vocabularies.

---

# 17. 37A.11 — Repeat/Held-Key Policy

Define repeat behavior per action.

Examples:

- navigation: repeat after initial delay,
- confirm: no auto-repeat,
- modal open: debounced,
- tab next: controlled repeat,
- global panel open: one action per press.

Avoid machine-gun UI/audio cues.

---

# 18. 37A.12 — Modal Confirm/Cancel

Define:

```text
Confirm
Cancel/Close
Default button
Destructive confirmation
```

across modals.

Ensure Escape/close semantics are consistent.

---

# 19. 37A.13 — Focus Return to Opener

When a modal opens:

- capture valid opener reference/route identity.

When closed:

- return focus if opener still alive,
- otherwise fall back to panel/root initial focus.

No focus loss.

---

# 20. 37A.14 — Action Injection Tests

One test per action:

```text
inject action
→ verify intended route/state change
```

Use the existing UI test composition root.

Do not only assert predicate returns true.

---

# 21. 37A.15 — Orphan Regression Test

Introduce a fake declared action without handler in test fixture.

Assert input-map gate fails.

Also test handler referencing undeclared action.

---

# 22. 37A Definition of Done

- [ ] 21-action reality table,
- [ ] every orphan dispositioned,
- [ ] global dispatcher centralized,
- [ ] panel-level global sniffing removed,
- [ ] help opens guidance,
- [ ] action targets use PanelRegistry,
- [ ] live binding affordances,
- [ ] input-map gate active,
- [ ] conflicts validated,
- [ ] Godot/default nav reconciled,
- [ ] repeat policy defined,
- [ ] modal confirm/cancel consistent,
- [ ] focus returns to opener,
- [ ] action injection coverage,
- [ ] orphan regression test fails as expected.

---

# 23. Task 37B — Focus Order and Keyboard Navigation

## Goal

A player who never touches the mouse can complete a full campaign day.

---

# 24. 37B.1 — Shared Focus Policy

Update shared UI factories such as:

```text
AshfallUiHelpers.MakeActionButton
AshfallUiHelpers.MakeDataRow
```

Default:

```text
interactive → FocusMode.All
decorative → FocusMode.None
```

Do not configure hundreds of panels individually when a factory can establish the rule.

---

# 25. 37B.2 — Focusable Control Taxonomy

Classify common widgets:

- action button,
- toggle,
- slider,
- list item,
- tab,
- data row with action,
- icon-only button,
- decorative label,
- read-only stat.

Document default focus behavior.

---

# 26. 37B.3 — Initial Focus Contract

Every live panel has a deterministic initial focus target.

Priority:

1. primary action,
2. first meaningful interactive item,
3. navigation/back control.

Empty-state panels still need a valid focus target.

---

# 27. 37B.4 — Reading Order

Default:

```text
top → bottom
left → right within groups
```

Override only where visual structure requires spatial mapping.

---

# 28. 37B.5 — Directional Navigation

Wire:

```text
ashfall_nav_up
ashfall_nav_down
ashfall_nav_left
ashfall_nav_right
```

to focus movement.

Use explicit neighbors for complex/grid panels where automatic layout is unreliable.

---

# 29. 37B.6 — List Navigation

Arrow navigation must work on:

- survivor roster,
- inventory,
- expedition party,
- duty roster,
- map node list,
- briefing entries,
- trade lists,
- crafting recipes,
- treatment lists.

---

# 30. 37B.7 — Grid Navigation

For grid-like layouts:

- left/right remain in row,
- up/down preserve logical column where possible,
- boundary movement is deliberate,
- no focus jumps to unrelated navigation accidentally.

---

# 31. 37B.8 — Visible Focus Style

Add a design-system focus state.

Requirements:

- high contrast,
- survives graphite/brass palette,
- not color-only,
- shape/weight/border change,
- consistent across buttons/lists/tabs.

---

# 32. 37B.9 — Top-10 Surface Pilot

Implement and validate first on:

1. dashboard,
2. survivors,
3. expeditions,
4. inventory,
5. crafting,
6. medical,
7. trade,
8. duty roster,
9. map,
10. briefing.

Do not sweep all panels until pilot pattern is stable.

---

# 33. 37B.10 — Live-Surface Coverage

Use Plan 16A verdicts.

Every `PlayerNavigable` live route must have keyboard reachability.

Shelved/prototype surfaces do not need production keyboard coverage.

---

# 34. 37B.11 — Mouse-Action Parity

For each mouse-clickable player action:

- keyboard focus reaches it,
- confirm activates it,
- relevant alternate action has discoverable key where needed.

No mouse-only dead ends.

---

# 35. 37B.12 — Click-Through Briefing Parity

Plan 31B clickable lines must have keyboard activation.

Focus briefing line:

```text
Enter/confirm
→ same source route/action as click
```

---

# 36. 37B.13 — Route Search / Go-To Palette

If implemented, keep it lightweight.

Reuse:

- PanelRegistry,
- live route metadata,
- localization keys.

One hotkey opens search/navigation.

Do not build a separate routing system.

---

# 37. 37B.14 — Modal Focus Trap

While modal open:

- focus remains inside modal,
- background shortcuts that should be blocked are blocked,
- close restores opener.

---

# 38. 37B.15 — Session-Swap Focus Safety

On New Game/Load:

1. clear invalid focus references,
2. rebuild/rebind panels,
3. restore route-level focus to valid replacement if possible,
4. otherwise select panel initial focus.

Never focus a freed Godot node.

---

# 39. 37B.16 — Accessible Names

Icon-only controls require text-equivalent names.

Color-coded status requires text/icon/shape redundancy.

---

# 40. 37B.17 — Mouseless Campaign-Day Journey

Flagship journey:

```text
New Game
→ choose/confirm setup
→ open survivors
→ assign duty
→ open inventory/crafting
→ craft/use relevant item
→ prepare expedition
→ dispatch
→ advance day
→ navigate briefing
→ open a source panel
```

No mouse injection.

---

# 41. 37B.18 — Focus Order Probes

Per panel, test:

- initial focus,
- next/previous traversal,
- directional traversal,
- close/reopen,
- empty state,
- modal return.

---

# 42. 37B.19 — Generated Keyboard Map

Generate:

```text
docs/ui/KEYBOARD.md
```

from the action map and route/help metadata.

Never hand-maintain.

---

# 43. 37B.20 — 1280×800 Review

Even though primary canvas is fixed, validate keyboard focus and visible focus at reduced window size.

Ensure scrolling does not hide focused control.

---

# 44. 37B Definition of Done

- [ ] shared focus policy,
- [ ] focusable taxonomy,
- [ ] deterministic initial focus,
- [ ] reading order,
- [ ] directional nav live,
- [ ] list navigation,
- [ ] grid navigation,
- [ ] visible focus design,
- [ ] top-10 pilot,
- [ ] every live route keyboard reachable,
- [ ] mouse-action parity,
- [ ] briefing click-through parity,
- [ ] modal focus trap,
- [ ] session-swap focus safety,
- [ ] accessible names,
- [ ] mouseless campaign-day journey passes,
- [ ] per-panel focus probes,
- [ ] generated keyboard map,
- [ ] reduced-window review.

---

# 45. Task 37C — Controller, Rebinding, and Player-Controlled Legibility

## Goal

Add honest controller support, persistent rebinding, centralized text/UI scale, and recoverable accessibility settings.

---

# 46. 37C.1 — Controller Scope ADR

Document what “controller supported” means.

Recommended minimum:

- menus,
- route navigation,
- lists/grids,
- confirm,
- cancel,
- global hotkeys,
- modal interaction,
- map/node focus navigation.

Explicitly document unsupported interaction if any.

Do not claim full controller support while requiring mouse-only actions.

---

# 47. 37C.2 — Same Actions, Joypad Bindings

Add `InputEventJoypad*` bindings to the existing actions.

No controller-specific parallel dispatcher.

Suggested conceptual mapping:

- D-pad/stick → nav,
- A/Cross → confirm,
- B/Circle → close,
- shoulders → tabs,
- menu/view buttons → help/journal/global surfaces where appropriate.

Actual mapping should follow platform norms.

---

# 48. 37C.3 — Stick Deadzone

Define:

- deadzone,
- initial repeat delay,
- repeat cadence,
- analog-to-digital focus movement.

Avoid accidental focus drift.

---

# 49. 37C.4 — Focus vs Map/Grid Cursor Mode

Dense lists and maps may require distinct navigation semantics.

Make mode explicit and visible.

Do not allow stick movement to fight both map movement and UI focus simultaneously.

---

# 50. 37C.5 — Rebinding Surface

Use existing settings surface if viable.

List all rebindable actions with:

- current keyboard binding,
- current controller binding,
- action label,
- reset control.

No second input-settings subsystem.

---

# 51. 37C.6 — Capture Binding Input

Rebinding flow:

```text
select action
→ enter capture mode
→ receive key/button
→ validate
→ detect conflict
→ accept/reject
→ persist
```

Escape/cancel exits capture safely.

---

# 52. 37C.7 — Context-Aware Conflict Detection

Reject true conflicts in the same active context.

Allow deliberate overlaps only where contexts are mutually exclusive and documented.

---

# 53. 37C.8 — Per-Action and Global Reset

Support:

- reset one action,
- reset all bindings.

Defaults originate from canonical input defaults, not a duplicated UI table.

---

# 54. 37C.9 — Versioned Settings Persistence

Extend `UserSettingsStore`.

Persist:

- keyboard bindings,
- controller bindings,
- text/UI scale,
- motion setting,
- vibration setting,
- relevant accessibility options.

Old/corrupt settings degrade to safe defaults.

---

# 55. 37C.10 — Safe-Mode Boot

Provide a documented boot recovery path.

Example:

```text
hold designated safe-mode key at startup
→ ignore custom bindings/scale
→ load defaults
```

Requirements:

- works even if bindings are unusable,
- does not destroy settings until user chooses reset/save,
- documented in support QA.

---

# 56. 37C.11 — Centralized Text/UI Scale

Create a user scale setting.

Apply through:

- theme/global font sizes,
- layout scale variables where necessary.

Do not update dozens of panel-local overrides manually as the primary mechanism.

---

# 57. 37C.12 — Legacy Font Override Migration

Audit per-widget font overrides.

Classify:

- intentional semantic hierarchy,
- accidental hardcoded size.

Move accidental cases to theme tokens.

Preserve headings/status hierarchy.

---

# 58. 37C.13 — Scale Range

Choose bounded user scale.

Example conceptual range:

```text
0.85× – 1.50×
```

Actual values must be validated against layouts.

Provide named presets if helpful:

- Small,
- Standard,
- Large,
- Extra Large.

---

# 59. 37C.14 — Layout Overflow Tests

Run top-10 and snapshot suite at:

- minimum scale,
- standard,
- large,
- maximum supported.

Check:

- clipping,
- scroll reachability,
- modal fit,
- focus visibility,
- button labels,
- tables.

---

# 60. 37C.15 — Reduce Motion

Add a player setting for:

- non-essential animations,
- looping motion,
- transition motion.

Do not disable gameplay-significant state changes.

---

# 61. 37C.16 — Flashing/Photosensitivity Discipline

Document:

- flash frequency limits,
- non-flashing alternatives,
- alert presentation rules.

Use existing warning/icon/audio channels.

---

# 62. 37C.17 — Captions and VO Parity

All VO/audio information with semantic content has text equivalent.

Coordinate with Plan 25C localization.

---

# 63. 37C.18 — Vibration

Optional.

Default off per source scope.

Provide intensity/off if implementation exists.

Do not use vibration as sole signal.

---

# 64. 37C.19 — Live Input Help

Guidance overlay reads current bindings.

Example:

```text
Help: F1
Confirm: Enter / A
Close: Esc / B
```

Values generated from live map.

After rebinding, help changes immediately.

---

# 65. 37C.20 — Controller Smoke Journey

Use synthesized joypad events in headless UI harness.

Journey should cover same critical loop as keyboard, within documented controller scope.

---

# 66. 37C.21 — Settings Recovery Test

Test:

1. save custom binding/scale,
2. reload,
3. verify,
4. corrupt settings,
5. boot safe,
6. defaults applied,
7. diagnostic surfaced,
8. user can reset and persist.

---

# 67. 37C.22 — Accessibility Checklist Upgrade

Extend the manual recovery smoke into a real input/accessibility QA checklist.

Include:

- keyboard,
- controller,
- focus,
- rebind,
- scale,
- safe mode,
- reduce motion,
- captions,
- color-independent state.

---

# 68. 37C Definition of Done

- [ ] controller support scope documented,
- [ ] joypad bindings use existing actions,
- [ ] deadzone/repeat tuned,
- [ ] focus/grid mode semantics,
- [ ] rebinding UI,
- [ ] conflict detection,
- [ ] per-action reset,
- [ ] global reset,
- [ ] versioned settings persistence,
- [ ] safe-mode boot,
- [ ] centralized text/UI scale,
- [ ] font override debt reduced,
- [ ] scale overflow tests,
- [ ] reduce-motion setting,
- [ ] flashing discipline,
- [ ] captions parity,
- [ ] vibration optional/off by default,
- [ ] guidance displays live bindings,
- [ ] controller smoke journey,
- [ ] settings recovery test,
- [ ] accessibility QA checklist.

---

# 69. Integrated Input Pipeline

```text
physical input
   │
   ├─ keyboard
   ├─ mouse
   └─ joypad
   │
   ▼
Godot InputMap
   │
   ▼
AshfallInputActions
   │
   ▼
central dispatcher
   │
   ├─ global route action
   ├─ modal action
   └─ focus/navigation action
          │
          ▼
     live UI control
          │
          ├─ visible focus
          ├─ confirm/cancel
          └─ state-versioned command
                 │
                 ▼
          canonical game authority
```

---

# 70. Action Contract

Every action answers:

```text
Is it declared?
Does a typed wrapper exist?
Who dispatches it?
What context owns it?
What state/route does it affect?
What is its repeat policy?
What keyboard binding?
What controller binding?
How is it displayed in help?
```

No unanswered retained action.

---

# 71. Focus Contract

Every live player panel answers:

```text
What gets initial focus?
What is the tab order?
What do arrows do?
What is the empty-state focus?
What happens when a modal opens?
Where does focus return?
What happens after session replacement?
```

---

# 72. Modal Contract

```text
open
→ capture opener
→ focus default control
→ trap focus
→ confirm/cancel
→ close
→ validate opener
→ restore focus or fallback
```

---

# 73. Route Contract

Global action targeting a panel:

```text
action
→ PanelRegistry route ID
→ liveness/maturity validation
→ open/reveal panel
→ focus initial target
```

No class-level direct open bypass.

---

# 74. Rebinding Contract

Stable identity is action ID.

User binding is mutable settings data.

Help/UI never stores duplicated binding labels.

---

# 75. Settings Contract

Input/accessibility settings are:

- versioned,
- validated,
- migrated,
- resettable,
- recoverable.

Corrupt input settings must not soft-lock the game.

---

# 76. Text Scale Contract

Simulation is unaffected.

Only presentation changes.

At all supported scales:

- controls remain reachable,
- focused control remains visible,
- text does not overlap critical actions,
- modals remain closable.

---

# 77. Accessibility Contract

Every critical status/action must have more than one presentation channel where relevant:

- text,
- icon,
- shape,
- focus border,
- sound/caption.

Never color-only.

---

# 78. Determinism Considerations

Input binding changes must not affect simulation determinism.

Equivalent logical action sequence:

```text
keyboard
vs
controller
```

must reach the same domain commands.

No RNG behavior should depend on physical input device.

---

# 79. Performance Considerations

Avoid:

- per-frame route catalog scans,
- rebuilding help text every frame,
- reconstructing full focus graphs continuously.

Prefer:

- static/generated route/action maps,
- recompute help on binding change,
- focus neighbor setup on panel construction/rebind.

---

# 80. Failure Modes

## Declared action does nothing

Input-map gate fails.

## Predicate has no caller

Classify/remove/wire.

## Panel handles global shortcut itself

Move to central dispatcher.

## Modal closes and focus disappears

Restore opener or valid fallback.

## Load leaves focus on freed node

Clear/rebind focus during lifecycle.

## Controller bindings added before focus

Stop; complete 37B first.

## Rebinding creates conflict

Reject before persistence.

## User rebinds confirm/close into unusable state

Offer warnings, required-action validation, and safe mode.

## Text scale makes close button unreachable

Fail layout/accessibility test.

## Help shows stale keys

Generate from live input map.

---

# 81. Risk Register

| Risk | Likelihood | Impact | Mitigation |
|---|---:|---:|---|
| focus crash after node free | Medium | High | lifecycle focus safety |
| hidden mouse-only action | Medium | High | mouseless journey |
| controller scope overreach | Medium | Medium | ADR |
| binding conflicts | High | Medium | context-aware validation |
| safe-mode key also rebound | Low–Med | High | boot-level fixed recovery key |
| scale snapshot churn | High | Medium | bounded variants |
| per-panel focus inconsistency | Medium | Medium | shared factory |
| modal shortcut leakage | Medium | Medium | context dispatcher |
| duplicate Godot/custom nav | Medium | Medium | reconciliation |
| stale help labels | Medium | Low | live generated labels |
| accessibility regression | Medium | High | ashfall-ui-access + snapshots |

---

# 82. Commit Strategy

## C2[15].1 — action/focus/settings baseline

## C2[15].2 — action disposition + central dispatcher

## C2[15].3 — help route + PanelRegistry routing

## C2[15].4 — input-map conflict/orphan gate

## C2[15].5 — repeat/modal/focus-return semantics

## C2[15].6 — action injection tests

### Gate: 37A complete

## C2[15].7 — shared focus factory policy

## C2[15].8 — top-10 focus pilot

## C2[15].9 — directional/list/grid navigation

## C2[15].10 — modal/session focus safety

## C2[15].11 — mouseless-day journey + route sweep

## C2[15].12 — generated keyboard map/accessibility

### Gate: 37B complete

## C2[15].13 — controller scope + joypad bindings

## C2[15].14 — deadzone/grid-cursor behavior

## C2[15].15 — rebinding UI + conflict/reset

## C2[15].16 — settings versioning + safe mode

## C2[15].17 — centralized text/UI scale

## C2[15].18 — motion/captions/vibration/accessibility settings

## C2[15].19 — live binding help + controller smoke

## C2[15].20 — recovery/scale QA closure

### Gate: 37C complete

## C2[15].21 — integrated human-interface closure

---

# 83. Verification Checklist

Run:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
bash scripts/ci/input-map-gate.sh
godot --headless --path . -- --player-panels-uitest
godot --headless --path . -- --playable-shell-selftest
bash scripts/ci/verify-fast.sh
```

Also run:

```text
ashfall-ui-access
ashfall-snapshot-diff at scale variants
keyboard mouseless-day journey
controller smoke journey
settings safe-mode recovery
```

---

# 84. Flagship Definition of Done

## 37A

- [ ] every action has disposition,
- [ ] central dispatcher,
- [ ] no global panel sniffing,
- [ ] help opens guidance,
- [ ] PanelRegistry action targets,
- [ ] live shortcut affordances,
- [ ] orphan/conflict gate,
- [ ] repeat behavior,
- [ ] modal confirm/cancel,
- [ ] focus return,
- [ ] action injection tests,
- [ ] orphan regression proof.

## 37B

- [ ] shared focus factory,
- [ ] focus taxonomy,
- [ ] initial focus,
- [ ] directional navigation,
- [ ] list/grid navigation,
- [ ] visible focus state,
- [ ] top-10 surfaces,
- [ ] every live route reachable,
- [ ] mouse-action parity,
- [ ] keyboard click-through,
- [ ] modal focus trap,
- [ ] session-swap focus safety,
- [ ] accessible names,
- [ ] mouseless day passes,
- [ ] keyboard map generated.

## 37C

- [ ] controller scope documented,
- [ ] same action vocabulary for joypad,
- [ ] deadzone/repeat,
- [ ] rebind surface,
- [ ] conflict validation,
- [ ] per-action/global reset,
- [ ] settings migration,
- [ ] safe-mode boot,
- [ ] centralized scale,
- [ ] scale tests,
- [ ] reduce motion,
- [ ] flashing limits,
- [ ] captions,
- [ ] vibration optional/off by default,
- [ ] live help reflects bindings,
- [ ] controller day/smoke journey,
- [ ] recovery QA.

## Cross-system

- [ ] localization-safe binding labels,
- [ ] no controller-before-focus shortcut,
- [ ] no mouse-only live action,
- [ ] no declared action that does nothing,
- [ ] no freed-node focus target after session swap,
- [ ] full verification green.

---

# 85. Closure Report Template

```markdown
## C2[15] Closure Report

### Repository
- Start commit:
- End commit:
- Branch:

### Baseline
- Input actions:
- Wrapper predicates:
- Orphan predicates:
- Unhandled actions:
- Directional handlers:
- Joypad bindings:
- Live panels with focus:
- Settings version:
- Font override count:

### 37A
- Action table:
- Orphans wired:
- Orphans removed:
- Dispatcher:
- Help route:
- PanelRegistry targets:
- Input-map gate:
- Conflicts:
- Repeat policy:
- Modal semantics:
- Action injection:
- Result:

### 37B
- Shared focus policy:
- Top-10 panels:
- Live routes keyboard reachable:
- Directional nav:
- Lists:
- Grids:
- Modal focus:
- Session-swap safety:
- Mouseless day:
- Keyboard map:
- Accessibility:
- Result:

### 37C
- Controller scope:
- Joypad mappings:
- Rebinding:
- Conflicts:
- Reset:
- Settings migration:
- Safe mode:
- Text/UI scale:
- Overflow failures:
- Reduce motion:
- Captions:
- Vibration:
- Live help:
- Controller smoke:
- Recovery:
- Result:

### Verification
- Core build:
- Core tests:
- Host build:
- Data integrity:
- Bridge:
- Input-map gate:
- Player panels UI test:
- Playable shell:
- UI access:
- Snapshot variants:
- Verify fast:

### Final Metrics
- DECLARED_ACTIONS:
- UNHANDLED_ACTIONS:
- UNUSED_PREDICATES:
- LIVE_ROUTES_WITH_KEYBOARD:
- CONTROLLER_BOUND_ACTIONS:
- BINDING_CONFLICTS:
- FOCUS_SAFETY_FAILURES:
- SCALE_OVERFLOW_FAILURES:

### Remaining Debt
- Input:
- Focus:
- Controller:
- Rebinding:
- Scale:
- Accessibility:
```

---

# 86. Final Execution Directive

Execute Plan 37 as a human-interface continuity repair.

The critical sequence is:

```text
make every action real
→ route global input through one dispatcher
→ gate action-map drift
→ establish shared focus policy
→ make all live surfaces keyboard reachable
→ prove a full mouseless campaign day
→ only then add controller bindings
→ let the player own their bindings and scale
→ make recovery possible if settings break
```

Do not count a joypad mapping as controller support if focus has nowhere valid to move.

Do not count a keybinding as a feature if the action has no handler.

Do not count a panel as keyboard accessible if one of its meaningful mouse actions remains unreachable.

The strongest input rule is:

> **Every declared action has one real, tested behavior and one central dispatch path.**

The strongest focus rule is:

> **Every live player-facing surface has deterministic focus order, visible focus, safe modal restoration, and session-swap-safe focus ownership.**

The strongest controller rule is:

> **Controller input reuses the keyboard action vocabulary and becomes supported only after the same focus/navigation model can complete a campaign day.**

The flagship acceptance scenario is:

> **Start a new campaign and complete a full operational day once with keyboard only and once with the supported controller path. Assign, craft, dispatch, advance, inspect the briefing, follow a source route, open help, and recover from a modal without touching the mouse; the same live systems must receive the same domain actions in both runs.**
