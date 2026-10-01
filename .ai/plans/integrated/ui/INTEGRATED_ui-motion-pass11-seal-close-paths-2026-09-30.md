# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
>
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
>
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# UI MOTION — PASS 11 (seal every close path + navigation cue)

> **STATUS: APPROVED BY USER** (user directive: "another pass! aim to seal UI
> transitions and animate subtle UI visual ques!")

Eleventh pass, continuing the motion remit. PASS 10 sealed the *seam*; this pass
closes the *call sites* that never reached it.

## Finding — 45 `Close*` methods still hard-hid their panel

PASS 10 fixed `UiMotion.AnimateClose` so a panel that hid itself could still be
faded. But a second class remained: `Main.*.cs` close handlers that hid the
panel **without ever calling the animated seam at all**:

```csharp
private void CloseOpeningProtocolModal()
{
    _openingProtocolModal.Visible = false;      // ← never reaches AnimateClose
}
```

45 such methods across 25 `Main.*.cs` files, e.g. `CloseTradePanel`,
`CloseEconomyPanel`, `CloseExpeditionPanel`, `CloseMapPanel`,
`CloseCombatPanel`, `CloseChroniclePanel`, `CloseDutyRosterPanel`,
`CloseWorkshopPanel`, `CloseInventoryOverlay`, `CloseSurvivorsOverlay`.
Meanwhile 53 sibling methods *did* route through `ClosePanelAnimated` — the
close path was inconsistent, so some panels faded out and their neighbours
popped.

## Fix — one mechanical, verified transform

Every `Close*` method whose body hid directly and contained no animation call
had its hide rewritten to the existing shared helper:

```csharp
_workshopPanel.Visible = false;                    →  ClosePanelAnimated(_workshopPanel);
if (_somePanel != null) _somePanel.Visible = false; →  ClosePanelAnimated(_somePanel);
```

`ClosePanelAnimated` (already in the `Main` partial) null-checks, validity-checks
and delegates to `AnimateClose`, so the `if (x != null)` guard collapses away.

* `ClosePanelAnimated` call sites: **53 → 98**
* `Close*` methods still hiding directly: **45 → 0** (verified by re-running the
  same scanner)
* Mechanical transform, so it was validated by **build + the full UI battery**
  rather than by reading 45 diffs. The transform collapsed the opening brace
  onto the statement line for single-line bodies; a follow-up pass normalised the
  indentation, and `git diff --check` is clean.

## Subtle cue — navigation selection

`AshfallSidebar.SetRowHighlight` swapped background, border and label colour
instantly. Selecting a nav row now also plays a restrained settle on the row's
**label** — deliberately not the row, because rows are full-width and scaling
one would overflow its column. Routed through the shared `UiPanelFlow.Pulse`
seam, so it is a no-op under ReducedMotion.

## Verification

```
dotnet build Ashfall.csproj              0 errors
--player-panels-uitest                   PASS  issues=0
--ui-accessibility-selftest              PASS  issues=0
--ui-layout-selftest                     Failures: 0  issues=0
--dashboard-uitest                       PASS  issues=0
--settings-selftest                      Failures: 0
--inventory-uitest                       PASS  issues=0
--economy-uitest                         PASS  issues=0
--panel-bind-lifecycle-selftest          PASS  issues=0
--shelter-decor-selftest                 PASS (19 checks)
--workshop-relic-selftest                PASS
--warlord-ui-selftest                    PASS (17 checks)
--shelter-maintenance-selftest           PASS (12 checks)
--barter-selftest                        PASS (7 checks)
bin/run-scoped-tests                     3/3 suites
git diff --check                         clean
```

## Verification limit (unchanged from PASS 10)

`UiMotion.CanAnimate` is false under `--headless`, so `AnimateClose` returns
early and the fades themselves cannot be exercised here. These tests prove **no
regression and no behavioural break**; the motion is reasoned from the seam.

## Owned paths (exact)

`src/Main.*.cs` (25 files — mechanical close routing only),
`src/UI/AshfallSidebar.cs` (selection settle).
