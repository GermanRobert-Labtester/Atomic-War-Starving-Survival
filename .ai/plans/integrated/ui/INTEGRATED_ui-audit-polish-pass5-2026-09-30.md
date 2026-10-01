# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
>
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
>
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# UI AUDIT POLISH — PASS 5 (theme tokens, disabled affordances, item-art tail)

> **STATUS: APPROVED BY USER** (user directive 2026-09-30: "another pass!")

Fifth bounded pass. Continuation of `claim-ui-audit-polish-pass4-2026-09-30`.

## Pass 5 gap sweep (evidence found before editing)

| Probe | Result | Verdict |
|---|---|---|
| `Esc` handled via raw `InputEventKey` (misses pad B / rebound keys) | 0 panels | **Clean — no change** |
| Raw engine colours in `src/UI` | 2 files | **Real — fixed** |
| Disabled controls with no tooltip | 30+ panels, 251 `.Disabled =` sites | **Real, systematic — one shared seam added** |
| Competing empty-state helpers (`MakeEmptyState` 17 files / `MakeEmptyStateLabel` 14 files / ad-hoc `MakeMetadata("No …")` 32 files) | 3 presentations | **Real consistency debt — deliberately not churned** (see Non-goals) |
| Item rows still art-less | 24 files, but most are survivor/location/research/journal lists, not item lists | **Real item rows isolated and wired** |

## Owned paths (exact, Pass 5)

`src/UI/AshfallUiHelpers.cs`, `src/UI/AshfallUiTheme.cs`,
`src/UI/ShelterDecorPanel.cs`, `src/UI/WorkshopPanel.cs`.

## Delivered

- **Theme-token cleanup** — `ShelterDecorPanel` spelled a raw `Colors.White` for
  modulate identity. Added the named seam `AshfallUiHelpers.ColorNeutral` (with
  the misreading documented: white here is *modulate identity*, not "white
  text"). Raw engine colours in `src/UI` now exist in exactly one named constant
  and comments.
- **Disabled-control affordance** — `AshfallUiTheme.EnforceControlDefaults` now
  gives a disabled button with no authored tooltip a fallback
  ("Currently unavailable — prerequisites not met."). This is explicitly a
  *fallback*: an authored tooltip is never overwritten, and the seam is the one
  already used for other a11y defaults so it covers panels built with raw `new`.
- **Item-art tail** — `WorkshopPanel` protective-gear rows and
  `ShelterDecorPanel` decor chooser rows now carry resolved item art.
- **Authored reason tooltips** — `WorkshopPanel`'s REPAIR button explains *why*
  it is disabled (beyond repair / at repair ceiling / spend the bill) instead of
  relying on the generic fallback.

## Non-goals honored

No Core change, no data JSON change, no new save section, no new gameplay
owner, no commit, no full suite. The three empty-state helper presentations were
**not** consolidated: converging them means either churning 32 files of ad-hoc
fallbacks or changing helper signatures that 14 callers depend on, and neither
is justified by the player-visible gain. Recorded as accepted consistency debt.

## Verification (Pass 5)

```
dotnet build Ashfall.csproj                  0 errors / 6 pre-existing warnings (untouched HostCli.*)
--ui-accessibility-selftest                  PASS (5/5; static lint over 267 UI files)
--player-panels-uitest                       PASS
--panel-bind-lifecycle-selftest              PASS
--shelter-decor-selftest                     PASS (failed=0)
--shelter-maintenance-selftest               PASS (12/12)
--workshop-relic-selftest                    PASS
git diff --check                             clean
```

## Finding recorded (not fixed, outside claimed surface)

`--workshop-relic-selftest` prints at-exit RID/ObjectDB leak diagnostics. Proven
**pre-existing**: `src/Main.UiTests.WorkshopRelic.cs` constructs `_workshopPanel`
and never frees it. My item-icon addition raises the leaked *count* slightly but
does not cause the leak. A control run (`--settings-selftest`, which builds no
nodes) prints none. Fixing it means adding teardown to the selftest harness —
outside this claim; flagged for the foreman rather than silently expanded into.
