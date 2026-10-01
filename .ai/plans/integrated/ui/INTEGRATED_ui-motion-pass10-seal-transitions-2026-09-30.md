# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
>
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
>
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# UI MOTION — PASS 10 (seal transitions + subtle visual cues)

> **STATUS: APPROVED BY USER** (user directive: "aim to seal UI transitions and
> animate subtle UI visual ques!")

Tenth pass, scoped to motion: make transitions complete ("sealed") and add
restrained visual feedback.

## Finding 1 — the close transition was unsealed at ~100 call sites

The dominant panel close pattern is:

```csharp
_closeButton.Pressed += () => { Visible = false; OnClose?.Invoke(); };
```

The panel hides **itself**, then the close seam runs `UiMotion.AnimateClose`,
which began with:

```csharp
if (!panel.Visible)
    return false;          // ← no animation, ever
```

So `ClosePanelAnimated` always fell through to `panel.Visible = false` and the
panel **hard-popped** despite the animation existing and being correct. Every
`AttachHeaderCloseButton("CLOSE", …)`-style panel was affected.

**Fix at the seam** (not 100 call sites): `AnimateClose` now revives a panel
that hid itself and fades it out. Because the panel has not been drawn since it
hid, the revive is seamless — it was already at full opacity in the previous
frame. Guarded by a new `_opened` set (populated by `AnimateOpen`) so a panel
that was **never shown** can never flash into existence on close.

## Finding 2 — an interaction bug in my own fix, caught before shipping

Reviving `Visible` raises `VisibilityChanged`, whose registered open hook calls
`AnimateOpen` + `EnsureInitialFocus`. Left alone, the panel would have faded **in**
while being faded **out**, and stolen focus from wherever the player had moved.

Fixed in two places: `AnimateClose` registers the closing state **before**
reviving visibility (so `AnimateOpen`'s existing `IsClosing` guard stands down),
and the visibility hook now skips when `IsClosing(captured)`.

## Finding 3 — the open side was already sealed (my concern was unfounded)

Routes that call bare `panel.Open()` looked unanimated. They are not:
`RegisterOpenMotionRecursive` registers a `VisibilityChanged` hook on every
`IBindablePanel` / `IModalPanel` that fires `AnimateOpen` + `EnsureInitialFocus`
whenever a panel becomes visible. Recorded rather than "fixed" into duplication.

## Subtle cues added (deliberately restrained)

| Cue | Behaviour | Why |
|---|---|---|
| Metric-card urgency escalation | settle pulse on Normal→Caution→Warn→Critical | crossing a threshold should register |
| De-escalation | **silent** | good news should not call attention to itself |
| Gauge fills (5 dashboard bars) | eased 0.18 s instead of snapping | a moving meter reads as motion, not a redraw |
| Status + alert lines | settle pulse **on change only** | these change when the *situation* changes |
| Numeric stores summaries | **deliberately unpulsed** | they move every update and would read as jitter |

`SetBarValue` uses one tween per bar (newer updates supersede, never stack) and
falls back to an instant set under ReducedMotion / headless / capture — the
accessible path is always available.

## Verification

```
dotnet build Ashfall.csproj             0 errors / 6 pre-existing warnings (untouched HostCli.*)
--player-panels-uitest                  PASS  issues=0
--ui-accessibility-selftest             PASS  issues=0
--ui-layout-selftest                    Failures: 0   (12 ObjectDB / 6 CanvasItem residue, pre-existing)
--dashboard-uitest                      PASS  issues=0
--settings-selftest                     Failures: 0
--inventory-uitest                      PASS  issues=0
--economy-uitest                        PASS  issues=0
bin/run-scoped-tests                    2/2 suites
git diff --check                        clean
```

## Note on headless verification

`UiMotion.CanAnimate` is false under `--headless`, so `AnimateOpen` /
`AnimateClose` return early and the animations themselves cannot be exercised
by these tests. They verify **no regression**; the motion behaviour is reasoned
from the seam logic above. `--ui-snapshot-uitest` (which would observe rendered
frames) still requires a real renderer.

## Owned paths (exact)

`src/UI/UiMotion.cs`, `src/UI/AshfallMetricCard.cs`, `src/UI/AshfallUiHelpers.cs`
(`SetBarValue` + `SetTextPulsed` usage), `src/UI/GameDashboardPanel.cs` (5 gauge
calls + 2 status lines), `src/Main.PlayerSurfaces.cs` (visibility hook guard
only).
