# Plan 149 — In-Campaign Achievement & Milestone System

## Goal

Replace the panel's derived literals with a data-driven Core achievement system that observes
one campaign, shows clear progress, and emits a durable unlock event. It does not own
cross-campaign rewards, currency, prestige, or New Game+.

## Scope boundary

- **Plan 34** owns difficulty and the immutable campaign-completion record.
- **This plan** owns achievement definitions, deterministic condition evaluation, run-local
  unlock state, and the panel projection.
- **Plan 175** owns cross-campaign storage and all reward/meta-progression decisions. This plan
  exports completed achievement ids and never stores a second profile or applies a bonus.

## Evidence

`AchievementsPanel.cs` currently derives milestones from live survivor state and day count; no
Core achievement type or data catalog exists. The missing layer is an observable, testable
achievement contract—not another campaign-profile system.

## Task 1 — Achievement contract and catalog

1. Create `Assets/Ashfall.Core/Achievements/AchievementSystem.cs` and
   `Assets/StreamingAssets/Data/achievements.json`.
2. Define `AchievementDefinition` with an id, localized display keys, category, conditions, and
   optional epilogue tag. Do not include rewards, currency, unlockables, or New Game+ fields.
3. Define run-local `AchievementState`: completed ids, in-progress facts, and emitted event ids.
4. Migrate every existing panel literal into data before adding new definitions.
5. Load through the Core serializer; validate all referenced ids and condition kinds.

## Task 2 — Deterministic observation and UI

1. Evaluate pure conditions from explicit game events and registered state snapshots; never use
   `System.Random`, UI text, or an unversioned reflection scan.
2. Emit `AchievementUnlocked(id, campaignId)` exactly once per run.
3. Persist the run-local state in the campaign save and restore it before the panel binds.
4. Make the panel render catalog definitions plus state only—no hardcoded achievement labels or
   render-time milestone rules.
5. Start with a balanced catalog across survival, combat, social, exploration, economic, and
   moral play; avoid reward text that implies a bonus this plan does not grant.

## Task 3 — Handoff and validation

1. At completion, provide the completed ids to Plan 175 through a read-only export; Plan 175
   may grant profile rewards after its own idempotency check.
2. Add tests for condition evaluation, once-only emission, save/load, panel projection, and
   retroactive evaluation of an old in-progress campaign.
3. Add an achievements self-test that proves every panel entry is catalog-backed and every
   catalog reference resolves.

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
```

## Definition of Done

- The achievement panel contains no derived milestone literals.
- Achievement evaluation is deterministic, run-local, and save-round-trip tested.
- The only cross-campaign hand-off is completed ids; Plan 175 remains the sole meta owner.
