# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# UI Briefing Deep-Link Keyboard Access — pkg 14 (2026-09-29)

STATUS: APPROVED BY USER
(Batch mandate: "Continue with a larger batch of doing more UI correction and
UI precision work" — 2026-09-29 session; implements the last unimplemented
audit §5b keyboard-reachability finding.)

## Evidence (verified in source 2026-09-29)

- `src/UI/DailyBriefingModal.cs` renders per-entry deep links as
  `[url=<DeepLinkRoute>]>> GOTO[/url]` inside the body RichTextLabel
  (`ComposeText`, line ~253) and activates them only through
  `MetaClicked` (line 113) — a mouse-only path. The audit
  (docs/ui/ACCESSIBILITY_REPORT_2026-09-29.md §5b) records this as
  "no keyboard activation path".
- Enter/Space acknowledge and Tab skips are modal-level bindings
  (`_UnhandledInput`), so links cannot borrow those keys; Godot's
  RichTextLabel has no per-link focus model (engine limitation).
- `OnDeepLinkRequested` is the existing truthful dispatch seam (used by
  `OnMetaClicked`); no new route authority is introduced.

## Changes (src/UI/DailyBriefingModal.cs only)

- A `HFlowContainer` link row between the briefing scroll and the footer
  (hidden when the report carries no deep links).
- On `Show`, the row is rebuilt from the report's unique
  `DeepLinkRoute` values in first-appearance order (deduplicated; capped
  at 8 with no ellipsis guessing — overflow beyond the cap is intentionally
  not rendered rather than silently mislabeled).
- Each link is a standard `MakeButton` ("GOTO <route>") invoking
  `OnDeepLinkRequested` — keyboard focusable, controller-navigable,
  Tab-trap eligible, 28px floor via the existing defaults walk, and it
  rides the same dispatch seam as a mouse click on the RTL link.

## Non-goals

- No per-link focus inside the RichTextLabel (no engine support).
- No change to acknowledge/skip bindings, reveal animation, or report
  content.

## Verification

1. Host build 0 errors.
2. Static gate `Ashfall.Core.Tests/UI/UiBriefingDeepLinkGateTests.cs`:
   link-row seam, dedupe, cap, and OnDeepLinkRequested dispatch present.
3. Headless probes: `--ui-layout-selftest`, `--player-panels-uitest`, boot.
4. Pathspec commit; plan header FULLY INTEGRATED; archive to
   `.ai/plans/integrated/ui/`; `.ai/state.md`; memory.
