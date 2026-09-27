# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# EN-07 Completion History Chronicle Host Integration

**STATUS: APPROVED BY USER** (user authorized all previously-gated integrations, 2026-09-26)
**Package:** `EN-07-COMPLETION-HISTORY-SURFACE`
**Gate authority:** `Ashfall.Core.Endgame.CampaignCompletionHistoryService.Summarize` (previously 0 `src/` references)

## Bounded outcome

Give the signed EN-07 read model an operational host surface. It aggregates the append-only
user-level campaign completion history into a pure summary (completions, days, deaths, living,
distinct endings, milestone counts, difficulty breakdown) without IO, mutation, or progression award.

## Delivered

- New `src/Host/HostCli.CompletionHistory.cs` — 8-check probe `CompletionHistorySelfTest`.
- `Assets/Ashfall.Core/HostCliRegistry.cs` — enum `CompletionHistorySelfTest` + descriptor
  `--completion-history-selftest` (alias `--chronicle-summary-selftest`).
- `src/Host/HostCli.cs` — host enum + parse + `PrintHelp` entry.
- `src/Main.Application.cs` — dispatch.

## Verification

Build 0 errors; `godot --headless -- --completion-history-selftest` → 8/8 (zeroed empty; 3 ordered
appends; duplicate rejected; invalid observation rejected; 1200 days/avg 400; deaths/living/endings;
exact milestones; difficulty breakdown + no mutation). Parity 4/4.

## Non-goals

No change to `CampaignCompletionHistoryService`; no persistence of its own (the existing cross-run
store owns serialization); no UI panel.
