# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
>
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
>
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# FIX — free-during-signal orphan leak (the PASS-5 flagged bug)

> **STATUS: APPROVED BY USER** (user directive: "fix the flag!")

Closes the item flagged in `.ai/state.md` as
`FLAGGED FOR BUG VALIDATOR`: `--workshop-relic-selftest` printed at-exit
RID/ObjectDB leak diagnostics.

## Root cause — proved, not guessed

The original flag said "the selftest never frees its panel". **That diagnosis was
wrong**, and I recorded the correction rather than acting on it. Evidence:

* `--player-panels-uitest` and `--dashboard-uitest` build the same UI tree and
  report **zero** leaks — so "the harness doesn't free panels" cannot be it.
* `--verbose` on the failing test named the actual orphans: two `Button`s with
  **empty node paths**, plus `Image`/`ImageTexture`/`StyleBoxFlat`/`TextParagraph`.
* The engine error, when surfaced, was unambiguous:

```
ERROR: Object is locked and can't be freed.
ERROR: Invalid call. Nonexistent function 'free' in base 'Godot.Button'.
  WorkshopPanel.RefreshView() :159  ← AshfallUiHelpers.EmptyChildren → child.Free()
  ← WorkshopReverseEngineeringSystem.StartRepair :410   (Core event)
  ← WorkshopPanel.RenderRelicDetail>b__0 :444            (the Button's own Pressed handler)
```

**Root cause:** `WorkshopPanel` rebuilds its own container *from inside the
signal dispatch of a button that lives in that container*. `EmptyChildren`
calls `Free()` on the very button being dispatched. Godot locks objects during
dispatch, so the free is **refused** — but `RemoveChild` has already run, so the
node is detached and never freed: a permanent orphan. Two such buttons ⇒ the two
orphaned `Button`s.

## Fix (three parts, each measured)

1. **`AshfallUiHelpers.FreeDetached`** — `EmptyChildren` / `EmptyChildrenExcept`
   now free through a helper that calls `Free()` and, **only if the node is still
   valid afterwards** (i.e. the free was refused as locked), falls back to
   `QueueFree()`. Immediate-free semantics are preserved, so ownership gates that
   assert "freed with its owner" still pass; the refused case can no longer
   orphan a node.
2. **`WorkshopPanel`** — the panel must not rebuild from inside its own button's
   dispatch. `RefreshView` is no longer subscribed directly to
   `OnWorkshopChanged` / `OnWorkshopStateChanged` (those fire *within* the
   dispatch via `StartRepair`); a `RefreshViewDeferred` wrapper is used instead,
   and the five button handlers that called `RefreshView()` do the same. This
   removes the engine error at its source rather than merely surviving it.
3. **`Main.UiTests.WorkshopRelic.cs`** — teardown releases the built UI tree, and
   the harness flushes the now-deferred rebuild before asserting on rebuilt UI.

Also fixed while in the shared texture seam: `TryLoadTexture` dropped the native
`Godot.Image` returned by `Image.LoadFromFile` without disposing it, leaking one
`Image` per filesystem-fallback load (two call sites). `ImageTexture` copies the
pixel data, so the `Image` is now disposed immediately.

## Measured result

```
                              before                              after
ObjectDB leaked               30                                  0
CanvasItem RIDs               2                                   0
DummyTexture RIDs             3                                   0
ShapedTextData RIDs           6                                   0
FontAdvanced RIDs             1                                   0
resources still in use        1                                   0
"Object is locked"            yes                                 none
"Nonexistent function 'free'" yes                                 none
assertions                    14/14 PASS                          14/14 PASS
```

## Regression sweep (global seam changed, so this was the priority)

`FreeDetached` is reached by every panel rebuild in the game.

```
player-panels-uitest           PASS  issues=0     (Gate 19 "freed with owner" intact)
panel-bind-lifecycle-selftest  PASS  issues=0
dashboard-uitest               PASS  issues=0
inventory-uitest               PASS  issues=0
ui-accessibility-selftest      PASS  issues=0
settings-selftest              PASS  issues=0
economy-uitest                 PASS  issues=0
shelter-decor-selftest         PASS  issues=0
workshop-relic-selftest        PASS  issues=0
workshop-relic-uitest          PASS  issues=0
barter-selftest                PASS  issues=0
accessibility-settings-selftest PASS (12/12) issues=0
shelter-maintenance-selftest   PASS (12/12) issues=0
ui-layout-selftest             PASS  Failures: 0
bin/run-scoped-tests InventorySystemTests CraftingSystemTests UserSettingsRecoveryTests  3/3 suites
dotnet build Ashfall.csproj    0 errors
git diff --check               clean
```

### Regression I caused and fixed during this work (recorded honestly)

My first attempt changed `EmptyChildren` to always `QueueFree()`. That **broke**
`--player-panels-uitest` Gate 19 ("panel leaked 3 node(s) after Ready/Free") in
5 panels, because a queued free outlives the gate's ownership assertion. It was
reverted in favour of the `Free()`-with-`QueueFree()`-fallback above, which keeps
the immediate-free contract. Gate 19 is green again.

## Pre-existing findings observed but NOT changed (outside this bug)

* `--ui-layout-selftest` calls `panel.Call("Open")` on every panel and **47
  panels define no `Open()`**, logging "Nonexistent function 'Open'". It still
  reports `Failures: 0`. Test-vs-API drift in `HostCli.AuditPanelInteractivity`.
* The same test leaves large at-exit RID accounting (4496 ShapedTextData, 120
  Shape2D, 56 Body2D, 64 Area2D, 72 Viewport) from instantiating the whole panel
  set across 8 resolutions without teardown.

Both predate this work and are unaffected by it.

## Owned paths (exact)

`src/UI/AshfallUiHelpers.cs`, `src/UI/WorkshopPanel.cs`,
`src/Main.UiTests.WorkshopRelic.cs`.
