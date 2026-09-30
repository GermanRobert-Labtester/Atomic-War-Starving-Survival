# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# UI Pass 2 — Encounter-Modal Focus Repair + Rule/Sweep Audit — pkg 19 (2026-09-30)

STATUS: APPROVED BY USER
(Batch mandate: "continue with another pass" — bug-hunt continuation,
2026-09-30 session.)

## Repairs

**ExpeditionPanel encounter modal: nested overlay had no keyboard entry
point** (pkg-12 regression class, one directory over from that gate's
scope). The pkg 12 gate bans bare `.Visible = true` opens only in
`Main.*` partials; the repo-wide sweep found exactly one bypass outside
that scope: `_encounterModal.Visible = true` (src/UI/ExpeditionPanel.cs).
The encounter modal is nested inside the panel and never routed through
ShowPanelLifecycle, so when it surfaced mid-session nothing focused it —
keyboard/controller players had to click before they could commit a
choice, and when the modal drained, focus was left nowhere.

Fix (shared seams only, no panel→Main reach-through):
- Open: `AshfallFocusPolicy.OpenWithFocus(_encounterModal, opener: this)`
  — focuses the first choice button deferred; records the opener.
- Queue-drained hide: `AshfallFocusPolicy.FocusFirstDeferred(this)` —
  returns focus to the host panel. (The full `Close()` path needs no
  change: Main's close flow owns focus restore there.)
- Known remaining limit (documented, not fixed): the modal is not in the
  OverlayPanelCatalog, so Main's Tab trap does not bind inside it; choice
  buttons are still Tab/arrow-reachable within the panel's focus chain.

## Sweep results (no action needed)

- **Core determinism/purity (hard rules)**: no `System.Random`, no
  `Guid.NewGuid`, no wall-clock reads in deterministic paths — only the
  sanctioned `IWallClock` port and doc comments citing the rule.
- **Data integrity**: PASS — 0 errors across 430 catalogs (5 documented
  primary-wins warnings, pre-existing).
- **Double-subscription heuristic**: the two flagged `StateChanged +=
  RefreshView` pairs are the correct unbind-then-rebind idiom.
- **No TODO/FIXME/HACK markers** in the recently touched UI files.
- Runtime probes all PASS: expedition-selftest, expedition-playtest-
  selftest, expedition-encounter-bridge-selftest (0 script errors each),
  day1-selftest, seven-day-slice-selftest 25/25, ui-layout-selftest 0
  FAIL, player-panels-uitest 22/22, ui-snapshot-uitest 32/32 match.

## Verification

1. `dotnet build Ashfall.csproj` — 0 errors.
2. Probes as listed above.
3. Pathspec commit (src/UI/ExpeditionPanel.cs + plan); governance.
