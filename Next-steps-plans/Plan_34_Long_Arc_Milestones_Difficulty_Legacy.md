# Plan 34 — The Long Arc: Difficulty & Campaign Record

## Goal

Make difficulty an explicit, data-driven campaign axis and produce a deterministic completion
record that can be rendered as an epilogue. This plan does not own achievements or
cross-campaign rewards.

## Scope boundary

- **Plan 149** owns the achievement catalog, in-campaign evaluation, and achievement-panel
  projection.
- **Plan 175** owns the separate player meta profile, unlock economy, prestige, and New Game+.
- This plan owns only difficulty presets, the immutable completion record, and its in-campaign
  chronicle/epilogue projection. It must not create `AchievementSystem`, `achievements.json`, a
  meta wallet, or a second cross-campaign save store.

## Evidence

The player-facing difficulty axis is not yet a tuning authority, while campaign outcomes are
spread across save sections and epilogue consumers. A small, stable completion record gives
the ending, the chronicle, and Plan 175 one shared fact source rather than three parallel
summaries.

## Task 34A — Deterministic campaign-completion record

Create a Core `CampaignCompletionRecord` from already persisted campaign facts: final day,
chosen difficulty preset, survivors and deaths, ending id, major faction standing bands, and
the terminal state of registered systems. The record is produced once when the campaign ends,
checksummed with its campaign save, and is immutable thereafter.

1. Define a small DTO in Core with stable ids and no presentation text.
2. Capture only facts already owned by existing systems; do not re-evaluate achievements or
   introduce a parallel flag ledger.
3. Add a pure builder plus round-trip and byte-stability tests.
4. Expose the record to the epilogue/chronicle UI and to Plan 175 through an explicit read-only
   hand-off interface.
5. Verify an old completed campaign receives a conservative default record without changing its
   outcome.

## Task 34B — Difficulty as a real, data-driven axis

Create `difficulty_presets.json` and one Core difficulty contract. Presets define named,
visible multipliers over existing authoritative systems (needs, weather pressure, economy,
combat and save rules); they do not fork those systems.

1. Inventory current hidden/default tuning values and assign each to one existing authority.
2. Define preset ids, display keys, and bounded modifiers in JSON.
3. Apply a selected preset at new-campaign setup through one immutable campaign setting.
4. Surface the active preset in status, save metadata, and the completion record.
5. Keep Plan 175 limited to selecting an existing preset for a New Game+ run; it must not add a
   second modifier engine.
6. Add validation for unknown ids, out-of-range modifiers, deterministic application, and save
   round-trips.

## Task 34C — Campaign chronicle and epilogue projection

Render the completion record into a restrained in-campaign chronicle and ending projection.
This is a view of campaign facts, not a persistent achievement ledger or a reward store.

1. Map record facts to localized chronicle/epilogue entries.
2. Ensure a run can be viewed after completion without recomputing mutable live state.
3. Pass the same record to Plan 175 after the epilogue resolves; Plan 175 decides any profile
   reward, while this plan never grants one.
4. Add UI and headless tests for a minimal, a normal, and a terminal campaign record.

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
```

## Definition of Done

- One data-driven difficulty authority is selected at campaign creation and persisted.
- A completed campaign produces one checksummed, immutable completion record.
- Epilogue/chronicle views consume that record without owning another campaign summary.
- Plan 149 and Plan 175 have no duplicated implementation surface in this plan.
