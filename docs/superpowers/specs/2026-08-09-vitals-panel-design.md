# Vitals Panel Design

**Date:** 2026-08-09
**Status:** Approved (scope chosen: extend the diegetic HUD)

## Problem

The gameplay scene boots, the clock runs, needs decay, and the player can now
press keys — but almost none of it is visible. Only 3 of 28 classes under
`Assets/_Game/UI/` contain any draw code:

| Class | Draws? |
| --- | --- |
| `DiegeticHudController` / `DiegeticHudView` | Yes — UI Toolkit |
| `UtilityAIDebugHUD` | Yes — IMGUI debug overlay |
| The other 25 (`NeedsBar`, `DosimeterHUD`, `EventModalUI`, …) | No |

The other 25 are formatting and state models with no view. `NeedsBar` holds a
dictionary of `NeedBarData` with display names, values and critical flags;
`DosimeterHUD` holds `CumulativeDose` and `CurrentRate`. Both are fed live
data already. Nothing renders them.

So the player sees the diegetic panels (hatch, field contact, stores) and no
indication of hunger, thirst, fatigue, warmth, radiation, or the time of day —
the entire core loop is invisible.

## Scope

Extend the existing diegetic HUD with **one new panel**. Not one panel per
widget: that would be ~25 new UXML/USS pairs plus a layout and z-order scheme,
and it would commit the project to a HUD shape nobody has designed yet.

The vitals panel shows:

- Day and clock time
- Cumulative dose and current rate
- Four need bars: hunger, thirst, fatigue, warmth
- A keybind hint line, so the input added in the previous increment is discoverable

Explicitly **out of scope**: the other 24 model classes, any new panel, any
change to what data the models hold, and any restyle of the three existing
panels.

## Architecture

The existing diegetic HUD has a three-part shape that this follows exactly:

```
DiegeticHud.uxml        declarative tree, the UIDocument's source asset
DiegeticHudView.cs      pure C# view: name constants, Build(), BindExisting(), Paint*()
DiegeticHudController   MonoBehaviour: owns the UIDocument, binds sources, calls Paint*()
```

`BindExisting(docRoot)` binds the view's fields to the UXML-cloned tree by
element name, falling back to `Build(docRoot)` when the UXML is absent. So the
UXML and `DiegeticHudView.Build` describe the same tree twice and must stay in
sync — the element-name constants are the contract between them. A test asserts
both paths produce the same element set, so drift fails rather than silently
degrading to the fallback.

### Data flow

Everything needed is already live; nothing new needs plumbing from the systems:

```
NeedsSystem.OnNeedChanged   -> GameBootstrap -> HUD.Bind(survivor)        -> NeedsBar.SetNeeds(...)
RadiationSystem.OnDoseChanged -> GameBootstrap -> HUD.OnRadiationUpdated() -> DosimeterHUD.SetReading(...)
```

`HUD` already holds both models and both are updated on every change.

### The repaint gap

`RefreshDiegeticHud()` → `DiegeticHudController.Paint()` is called only from
discrete actions: mission accept/complete, a few UI actions, and late init. It
is **not** called when needs or dose change. A vitals panel painted only
through the existing path would show whatever was true at the last mission
event and then sit frozen while the player starved.

Fix: repaint from the two places that already fire on change — the tail of
`HUD.Bind(...)` and `HUD.OnRadiationUpdated(...)`. This keeps the project's
stated convention that readouts update when system events fire rather than
polling every frame, and costs one label update per changed need.

### Time

`NeedsBar` and `DosimeterHUD` carry no clock. The day/time line needs
`TimeSystem`, which `HUD` does not hold. Rather than give `HUD` a bootstrap
reference — a dependency inversion it has carefully avoided, since every other
value is pushed into it — the day and hour are pushed in as two floats by the
existing caller, on the same event that already repaints.

## Error handling

- A null `NeedsBar`, `DosimeterHUD` or view paints nothing and throws nothing:
  the HUD must never take down the simulation.
- A need absent from the dictionary renders as `--`, not `0%`. Zero is a
  meaningful value; missing is not the same thing and must not look like
  starvation.
- The panel is visible from the first paint. It is the one panel with no
  toggle — the others hide until their subsystem is relevant.

## Testing

EditMode, against `DiegeticHudView` directly — it builds a `VisualElement` tree
with no `UIDocument`, which is why the existing view is testable at all:

1. `Build` produces the vitals panel and its labels.
2. `BindExisting` finds every element the UXML declares (drift guard).
3. Painting known values produces the expected label text.
4. A missing need renders `--` rather than `0%`.
5. Painting with nulls throws nothing.

PlayMode, against the real scene: after some frames the vitals labels hold
non-placeholder text, proving the repaint path is actually connected — the
class of bug that the last two increments were both about.

## What this does not fix

The remaining 24 model classes still have no view. This panel does not
establish a pattern for panels that need input focus, scrolling, or modal
behaviour; the event modal in particular will need a different treatment, since
it must take keyboard focus and block. That is a separate design.
