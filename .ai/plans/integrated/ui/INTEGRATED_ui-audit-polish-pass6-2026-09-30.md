# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
>
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
>
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# UI AUDIT POLISH — PASS 6 (optional-hook API drift + harness teardown)

> **STATUS: APPROVED BY USER** (user directive: "another pass")

Sixth bounded pass. Targets the two items deliberately left open at the end of
PASS 5 and the leak fix.

## Findings

| Finding | Evidence | Verdict |
|---|---|---|
| 47 panels log `Nonexistent function 'Open'` | `HostCli.AuditPanelInteractivity` and `SnapshotOrchestrator` do `panel.Call("Open")` guarded only by `catch` — the **engine** logs the error before C# throws | **Real — fixed** |
| Is `Open()` part of the panel contract? | `IBindablePanel` declares only `IsBound` and `Unbind()`. `Open()` is a convention | **The audit was calling an API it only assumed existed** |
| Do the 47 panels have *any* render hook? | All 47 expose `public void RefreshView()` | **Yes — the audit can call a hook that exists** |
| Same bug elsewhere? | `SnapshotOrchestrator.cs:227` identical pattern | **Real — fixed** |
| `ui-layout-selftest` at-exit accounting | 24,731 ObjectDB / 7,822 CanvasItem / 4,496 ShapedTextData / 120 Shape2D / 56 Body2D / 64 Area2D / 72 Viewport | **Real teardown gap — fixed** |

## Delivered

1. **`AshfallUiHelpers.InvokePanelHook(node, method, fallback?)`** — reflects
   first and only invokes a hook the panel actually declares. Used at all four
   `Call("Open")` sites. Zero engine error spam, and callers can prefer a hook
   the panel really has.
2. **Interactivity audit** now falls back to `RefreshView()`, so the 47 panels
   that render through it are actually populated before the clickability /
   focusability / truthfulness sweep. Audit coverage widened, gate stayed green
   (`inert=0`, `unreachable=0`, `blankUnboundPanels=0`).
3. **`ui-layout-selftest` 8-resolution loop** built 8 panels per resolution and
   freed none. Now tracked and freed in a `finally`, including on exception.

## Deliberate asymmetry (risk-managed, and why)

`SnapshotOrchestrator` gets **no** `RefreshView()` fallback, unlike the audit.
That path captures **golden snapshots**; a panel that previously rendered only
its `_Ready()` content would start rendering populated content and silently
invalidate the committed baseline. Snapshot behaviour is therefore
**byte-identical to before** — only the error spam is removed. This could not be
verified headlessly (`--ui-snapshot-uitest` fails with "renderer unavailable —
SubViewport reads need a real display/renderer, not --headless"), so the safe
option was taken rather than an unverifiable improvement.

## Measured

```
                                   before          after
"Nonexistent function 'Open'"      47              0
ObjectDB leaked (ui-layout)        24,731          12
CanvasItem RIDs                    7,822           6
ShapedTextData RIDs                4,496           0
Shape2D / Body2D / Area2D          120/56/64       0/0/0
Viewport / DummyTexture / Font     72/102/3        0/0/0
resources still in use             26              0
ui-layout verdict                  Failures: 0     Failures: 0
```

## Regression sweep

```
player-panels-uitest           PASS  issues=0
panel-bind-lifecycle-selftest  PASS  issues=0
dashboard-uitest               PASS  issues=0
inventory-uitest               PASS  issues=0
ui-accessibility-selftest      PASS  issues=0
settings-selftest              PASS  issues=0
economy-uitest                 PASS  issues=0
shelter-decor-selftest         PASS  issues=0
workshop-relic-selftest        PASS  issues=0
barter-selftest                PASS  issues=0
accessibility-settings-selftest PASS (12/12) issues=0
shelter-maintenance-selftest   PASS (12/12) issues=0
ui-layout-selftest             PASS  Failures: 0
bin/run-scoped-tests           2/2 suites
dotnet build Ashfall.csproj    0 errors
git diff --check               clean
```

## Open, unchanged

* `--ui-snapshot-uitest` cannot run headless (needs a real renderer). Its 32
  `no-image` failures are environmental, not regressions.
* The 47 panels still do not expose `Open()`. That is not a defect — it is not
  part of `IBindablePanel` — but if the project *wants* a uniform `Open()` API,
  that is a separate, deliberate contract change, not something to impose from a
  bug-fix pass.

## Owned paths (exact)

`src/UI/AshfallUiHelpers.cs`, `src/UI/SnapshotOrchestrator.cs`,
`src/Host/HostCli.Command.RunUiLayoutSelfTest.cs`.
