# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# UI §3 Scrim Contrast Closure — pkg 17 (2026-09-29)

STATUS: APPROVED BY USER
(Batch mandate: "Continue working on remaining UI" — 2026-09-29 session;
closes the last open audit lane, ACCESSIBILITY_REPORT_2026-09-29 §3.)

## Method (and a corrected premise)

Worst-case bound: backdrop art cannot exceed sRGB (1,1,1), so compositing
each Ink scrim over pure white and computing WCAG contrast for the
surface's text token bounds every real backdrop. A scratch Python pass
with a wrong gamma threshold (0.5075 instead of 0.03928) initially
understated the failures (e.g. Warm over 0.55 looked like 5.52:1; the
correct value is 2.02:1); the shipped gate uses correct math against live
Core `Theme` tuples.

Premise verification (AGENTS rule 7) also overturned one audit finding:
MainMenuPanel's status/version labels do NOT render over the 0.55
carousel — the whole menu column is inside `MakePanel(520, 0)`. §3's
MainMenu row is a false positive; the backing panel is load-bearing and
now tripwired in the gate. The initially-made Warm recolor of the version
label was reverted as unnecessary.

## Changes

- `src/UI/MapDetailPanel.cs`: scene backdrop scrim 0.74 → 0.90 (Muted
  data rows were 2.74:1 worst-case over bright art).
- `src/UI/ExpeditionPanel.cs`: departure art dim 0.82 → 0.90 (Dim status
  summary was 3.78:1).
- `src/UI/GameOverPanel.cs`: carousel overlay 0.80 → 0.90 (panel-less
  title/cause/stats/hint column; Dim was 3.44:1).
- At Ink-dim ≥ 0.90 every token holds ≥ 4.96:1 over any art — the audit's
  own "≥0.9" acceptance direction, chosen per-surface (all three are
  backdrop/ambiance surfaces; art remains faintly visible).
- New gate `Ashfall.Core.Tests/UI/UiScrimContrastGateTests.cs` (6/6):
  worst-case math for Ink scrims at 0.90/0.92 (Pale+Muted), MainMenu
  backing-panel tripwire, Expedition banner tint math, compound chrome
  chain (card 0.72 → rail 0.55 → row 0.40 darkens monotonically; Dim
  ≥ 5.84), and closed-alpha tripwires for all four surfaces.
- `docs/ui/ACCESSIBILITY_REPORT_2026-09-29.md` §3: closure block appended
  with per-surface results and the false-positive correction.

## Non-goals

- No change to art assets, carousel behavior, or the 0.55 menu overlay
  (art-first surface protected by its backing panel).
- No tokenization of the three tinted §3 variants (crisis red, amber
  banner, Entropy banner) beyond the existing pkg 9 state.

## Verification

1. `dotnet build Ashfall.csproj` — 0 errors.
2. `scripts/run_test.sh .../UiScrimContrastGateTests.cs` — 6/6 PASS.
3. Headless probes: `--ui-layout-selftest` 0 FAIL, `--player-panels-uitest`
   22/22, `--ui-snapshot-uitest` 32/32 match (none of the three touched
   panels are snapshot targets; run confirms no collateral), boot clean.
4. Pathspec commit (src + gate + report + plan); `.ai/state.md`; memory.
