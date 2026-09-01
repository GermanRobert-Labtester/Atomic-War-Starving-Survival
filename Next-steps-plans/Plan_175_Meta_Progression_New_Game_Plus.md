# Plan 175 — Meta Profile & New Game+ Orchestration

## Goal

Create the one player-profile service that carries verified campaign completion, achievement
unlocks, and selected New Game+ options across campaigns. It owns rewards, currency, prestige,
and the profile store; it does not evaluate achievements or define difficulty rules.

## Scope boundary

- Plan 34 owns difficulty presets and `CampaignCompletionRecord` creation.
- Plan 149 owns achievement definitions, evaluation, and run-local state.
- This plan consumes those completed facts once, stores the profile outside campaign slots, and
  applies already-defined difficulty presets plus profile unlocks at the next campaign start.
  It must not create a second achievement catalog, re-check achievement conditions, or invent a
  competing difficulty-modifier engine.

## Task 1 — Meta-profile contract

1. Create `MetaProgressionSystem` and one versioned, checksummed profile store through the
   existing save-service pattern.
2. Define a profile with campaign summaries, completed achievement ids, unlocked profile items,
   meta currency, and prestige. Record source campaign ids so importing completion facts is
   idempotent.
3. Create `meta_unlockables.json` for rewards and cosmetic/start-option definitions. Rewards
   reference Plan 149 achievement ids or Plan 34 completion facts; they never duplicate either.
4. Use pure deterministic calculations for currency and prestige—no random roll is required.

## Task 2 — Reward and New Game+ flow

1. Import a completion record and its completed achievement ids after the epilogue settles.
2. Resolve unlock conditions, currency, and prestige exactly once per source campaign.
3. Offer a New Game+ setup that selects unlocked starting options and an existing Plan 34
   difficulty preset. The campaign itself starts fresh.
4. Apply unlocked options through explicit bootstrap inputs and show their source in setup UI.
5. Keep ironman/scarcity-style challenge rules as data-defined Plan 34 presets or tags, rather
   than a second multiplier model.

## Task 3 — Safety and validation

1. Separate the profile path from save-slot campaign data and preserve old campaigns unchanged.
2. Test duplicate import, corrupted profile rejection, old-profile migration, reward purchase,
   New Game+ input application, and save-slot isolation.
3. Validate every reward target against the owning catalog and test headless import without UI.

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
```

## Definition of Done

- There is exactly one cross-campaign profile store and reward owner.
- Plan 34 completion records and Plan 149 unlock ids import once per campaign.
- New Game+ selects existing difficulty presets and explicit profile unlocks without duplicating
  campaign state, achievements, or tuning rules.
