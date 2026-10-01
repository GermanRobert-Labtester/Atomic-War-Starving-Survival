# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
>
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
>
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# UI AUDIT POLISH — PASS 7 (CLI alias drift + warlord harness teardown)

> **STATUS: APPROVED BY USER** (user directive: "another pass!")

Seventh bounded pass. Evidence-driven hunt for real failures across probes not
previously executed, rather than re-treading known areas.

## Sweep results — including honest false positives

| Probe | Result | Verdict |
|---|---|---|
| Panels with `OnClose` but no keyboard close | 100 hits | **False positive** — close is centralized in `Main.PanelLifecycle.CloseAllOverlayPanels`; the 45-panel Esc softlock was fixed centrally in WHOLEGAME-P1B |
| `IBindablePanel` `Bind`/`Unbind` subscription imbalance | 25 hits | **False positive** — `Unbind()` removes via multi-level access (`_host.Roster.X -= …`) that the one-level regex missed; remaining `+=` are child-owned button handlers. Spot-checked `DutyRosterPanel`: 3/3 external subscriptions correctly removed |
| Remaining "item surfaces without art" | 24 hits | **False positive — and it retires a standing claim of mine.** Inspected `TravelingCaravanPanel` (train *voice* lines), `VehicleGaragePanel` (mod/armor-grade ids), `RailwayTerminalPanel` (train ids): none are item catalogs. **The item-art tail I had deferred twice does not exist.** Item art is complete across the true item surfaces |
| Three "competing" empty-state helpers | 3 kinds | **Overstated by me earlier** — `MakeEmptyState` (framed panel) and `MakeEmptyStateLabel` (inline) are two deliberate levels of one design language, not competitors |
| Untested UI probes (`ui-a11y`, `bestiary-ui`, `journal-uitest`, `survivors-uitest`, `save-load-ui-failure`, `warlord-ui`) | 6 run | **2 real defects found** |

## Defect 1 — catalogued CLI flag that does nothing

`--ui-a11y-selftest` is registered as an alias of
`HostCliAction.UiAccessibilitySelfTest` in `HostCliRegistry.cs:2220`, but
`HostCli.cs:812` never parsed it — the host printed
`Unrecognized headless argument(s): --ui-a11y-selftest` and did nothing. Drift
ran **both ways**: the parser also accepted `--ui-access-selftest`, which the
registry never advertised.

Fixed on both sides: the parser now accepts all four spellings, and the registry
alias list carries `--ui-access-selftest` too. Help text updated.

Note this survived `HostCliActionParityGateTests` (4/4 green before and after),
so the alias↔Parse gate has a coverage hole. Recorded rather than silently
widened into a gate change.

## Defect 2 — warlord harness teardown

`RunWarlordUiSelfTest` constructed a `FactionsPanel`, called `_Ready()`/`Bind()`/
`Open()`, then abandoned it: 590 ObjectDB instances, 243 CanvasItem RIDs, 8
DummyTexture, 22 ShapedTextData, 3 FontAdvanced, 11 resources leaked at exit.
Now held outside the `try` and released in a `finally`.

## Generated-output hygiene (policy-compliant)

The alias change invalidated two generated artifacts. Per the "never hand-edit
generated outputs" rule, both were regenerated through their owning generators,
not edited:

```
bash scripts/ci/generate-cli-catalog.sh        # 358 entries / 590 flag tokens
python3 scripts/ci/generate-selftest-manifest.py # 317 tests
... --check for both: OK / OK
```

## Measured

```
                                        before              after
--ui-a11y-selftest                      unrecognised        runs ui_accessibility_selftest
warlord-ui ObjectDB leaked              590                 0
warlord-ui CanvasItem RIDs              243                 0
warlord-ui other RIDs / resources       8/22/3/11           0/0/0/0
generate-cli-catalog.sh --check         FAIL                OK
generate-selftest-manifest.py --check   FAIL                OK
```

## Regression sweep

```
Ashfall.Core.Tests/HostCliActionParityGateTests   4/4 PASS
Ashfall.Core.Tests/HostCliHelpContractTests       2/2 PASS
--ui-accessibility-selftest  PASS  issues=0
--ui-a11y-selftest           PASS  issues=0   (was: unrecognised)
--ui-layout-selftest         Failures: 0      (12 ObjectDB / 6 CanvasItem residue, unchanged)
--player-panels-uitest       PASS  issues=0
--dashboard-uitest           PASS  issues=0
--settings-selftest          Failures: 0
--inventory-uitest           PASS  issues=0
--warlord-ui-selftest        PASS  issues=0   (was: 6 leak lines)
--journal-uitest             PASS
--survivors-uitest           PASS
--save-load-ui-failure-selftest PASS
--bestiary-ui-selftest       PASSED
dotnet build Ashfall.csproj  0 errors
git diff --check             clean
```

## Shared-file note

`src/Host/HostCli.cs` and `Assets/Ashfall.Core/HostCliRegistry.cs` both carried
pre-existing uncommitted foreign edits. My changes are additive single-line
edits in different regions; the foreign work is preserved verbatim.

## Owned paths (exact)

`src/Host/HostCli.cs` (parse condition + help line only),
`src/Host/HostCli.SelfTests.cs` (warlord teardown only),
`Assets/Ashfall.Core/HostCliRegistry.cs` (one alias array),
`docs/cli/HOST_CLI_COMMAND_CATALOG.md` and `docs/ci/SELFTEST_MANIFEST.json`
(regenerated through their owning generators).
