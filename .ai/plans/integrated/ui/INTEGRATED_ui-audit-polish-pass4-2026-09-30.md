# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
>
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
>
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# UI AUDIT POLISH — PASS 4 (developer session depth: detail inspector, sorting)

> **STATUS: APPROVED BY USER** (user directive 2026-09-30: "another pass!")

Fourth bounded pass. Continuation of `claim-ui-audit-polish-pass3-2026-09-30`.
Focus: deepen the developer session the user explicitly asked for — "check all
items and their assets and etc" — rather than churning more panels.

## Pass 4 gap sweep (evidence found before editing)

| Probe | Result | Verdict |
|---|---|---|
| Event subscription leaks (`StateChanged`/`SlotsChanged`/`OnSelected` `+=` without `-=`) | 31 hits — all `_sidebar.OnSelected`, where the sidebar is created by `_shell.SetSidebar(...)` and owned as a panel child; both die together | **False positive — no leak, no change** |
| Panels whose `Open()` does not refresh | 29 hits — but `PanelRegistry.ConfigureActions` runs `bindAction` (→ `Bind` → `RefreshView`) before `openAction` on every open | **False positive — seam is correct, no change** |
| Lists rebuilt with `EmptyChildren` and no empty-state | 20 hits — inspected `SkillMatrixPanel` / `TutorialPanel` / `MapDetailPanel`: all have rich fallbacks or static content | **False positive — no change** |
| Inspector could only *list* entries | Cards showed id/name/kind/path/status with no way to inspect one closely, and no ordering control | **Real gap — detail overlay + sort added** |

Recording the false positives matters: three plausible-looking defect classes
were checked against source and deliberately **not** "fixed".

## Owned paths (exact, Pass 4)

`src/UI/AssetInspectorPanel.cs` only.

## Delivered

- **Detail overlay** — every card gains a keyboard-accessible `VIEW` button that
  opens a full-screen inspector for that entry: 220px art preview, resolved
  asset path (mono), id / kind / art-status / authored stats.
- **Layered Esc** — `Esc` dismisses the detail overlay first, then the panel,
  so a close never skips a surface.
- **Sort control** — `Id` / `Name` / `Missing art first`, re-sorting the visible
  grid in place.
- **Animation** — the detail dialog body uses the shared `UiMotion.AnimateOpen`
  entrance; its exit is deliberately synchronous (the codebase convention:
  dismissal is never delayed by motion), and the scrim does not slide with the
  body.

## Non-goals honored

No Core change, no data JSON change, no new save section, no new gameplay
owner, no commit, no full suite. No other panel touched.

## Verification (Pass 4)

```
dotnet build Ashfall.csproj                  0 errors / 6 pre-existing warnings (untouched HostCli.*)
--ui-accessibility-selftest                  PASS (5/5)
--player-panels-uitest                       PASS (bind lifecycle 22/22)
--settings-selftest                          Failures: 0
--asset-registry-selftest                    PASS (checked=55 passed=55 missing=0 load-failed=0 probe-failures=0)
git diff --check                             clean
```
