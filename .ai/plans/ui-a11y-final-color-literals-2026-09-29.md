# UI A11y — Final Raw-Color-Literal Sweep + Central Text-Overrun Precision

STATUS: APPROVED BY USER

Date: 2026-09-29
Lane: UI precision/correction (audit series package 9; follows
`docs/ui/ACCESSIBILITY_REPORT_2026-09-29.md` §2b/§2c follow-through)

## Outcome

Two bounded parts, no behavior or layout redesign:

**Part A — last raw color literals on UI chrome → theme tokens (8 files, 9 sites).**
Each literal was re-verified in source and mapped to the nearest existing Core
token; none introduce new tokens or change contrast tier:

| File | Site | Old literal | New mapping |
|---|---|---|---|
| `src/UI/TimeCapsulePanel.cs` | capsule note font | `(0.8, 0.8, 0.8)` | `Muted` |
| `src/UI/EmergencyResponseHud.cs` | crisis backdrop (Severe+) | `(0.12, 0.02, 0.02, 0.95)` | `Critical` channels × 0.12, alpha 0.95 |
| `src/UI/BlackProjectsArchivePanel.cs` | classification banner bg | `(0.12, 0.04, 0.04, 0.85)` | `Critical` channels × 0.15, alpha 0.85 |
| `src/UI/ExpeditionPanel.cs` | encounter banner bg | `(0.10, 0.07, 0.04, 0.94)` | `Entropy` channels × 0.12, alpha 0.94 |
| `src/UI/UiBackgroundCarousel.cs` | fallback ColorRect | `(0.035, 0.043, 0.047, 1)` | `Ink` (exact match) |
| `src/UI/BackdropArt.cs` | placeholder dim overlay | `(0.04, 0.05, 0.06, dimAlpha)` | `Ink` channels + existing dimAlpha clamp |
| `src/World/RoomHotspotView.cs` | badge bg / hover bg / hover font | `(0.08,0.1,0.12,0.75)` / `(0.18,0.25,0.32,0.9)` / `(0.95,0.85,0.4)` | `SurfaceCard`+alpha / `Lethe` × 0.5 + alpha / `Hot` |

Deliberately NOT swept (documented, not missed):
- `src/World/MapLocationMarkerView.cs` gray `Modulate`s and
  `src/World/HoldfastInteriorView.cs` day-phase light tints — sprite/lighting
  art factors, not color chrome; mapping them to text tokens would change
  art semantics.
- `src/UI/SnapshotOrchestrator.cs` snapshot bg — test tooling.
- `src/Host/HoldfastTerminalPanel.cs` font shadow — shadow, not a token color.
- `src/Settings/UserSettings.cs` 1.15 brightness modulate — brightness adjust.
- `src/UI/GameDashboardPanel.cs:327` and `MapDetailPanel.cs:227` — already
  adjudicated in the audit (item 11 retraction; §3 stack-dependent, visual lane).

**Part B — central text-overrun precision in `src/UI/AshfallUiHelpers.cs`.**
Godot Labels default to `clip_text = false`: long text draws past its rect and
collides with neighbors. All non-autowrap label factories gain
`ClipText = true` + `TextOverrunBehavior.TrimEllipsis` via one private
`FinishLabel` seam applied per factory (autowrap factories keep wrapping);
`MakeButton` gains `ClipText = true` + `TrimEllipsis` (content-sized buttons
are unaffected; fixed-width buttons truncate with ellipsis instead of
overflowing). This is the same correction already applied ad hoc to
TradeScreenGodotPanel, AshfallMetricCard, SurvivorsPanel, GameDashboardPanel,
AshfallDataGrid, AshfallDashboardShell in earlier packages — now the default
at the factory seam so the ~480 direct `new Label` call sites can be migrated
incrementally without another sweep.

## Non-goals

- No new Core tokens; no contrast-tier changes; no layout changes.
- No migration of direct `new Label` call sites to `FinishLabel` (future).
- No visual-lane work (§3 scrim stack contrast, snapshot regeneration).

## Verification

1. `dotnet build` host target — 0 errors.
2. New static gate `Ashfall.Core.Tests/UI/UiA11yFinalColorGateTests.cs`:
   old literals absent + token reference present per Part A file; helpers
   contain `FinishLabel` + ClipText on MakeButton; token values pinned
   (SurfaceCard/Ink/Lethe/Hot/Muted/Critical/Entropy) to catch token drift.
3. Headless probes: `--ui-layout-selftest`, `--player-panels-uitest`,
   boot `--quit-after 2`.
4. Governance: claim row in `WORKTREE_OWNERSHIP.md`, `.ai/state.md` entry,
   pathspec commit, memory update.
