# Plan 20 — Travel Encounters

> **Data authority:** `Assets/StreamingAssets/Data/travel_encounters.json`
> **Core systems:** `Assets/Ashfall.Core/Narrative/TravelEncounterCatalog.cs`, `Assets/Ashfall.Core/Narrative/TravelEncounterSystem.cs`
> **Schema version:** 1

## Purpose

Travel encounters fire during expedition transit, giving the player meaningful decisions between map nodes. Encounters are filtered by region, danger level, season, and expedition stance. Multi-stage chained encounters build narrative continuity across multiple expeditions.

## Catalog Summary

**24 standalone encounters + 4 chains (12 chain stages)**

### Encounter Categories

- **Creature** — encounters with fauna; choices unlock field guide entries
- **Faction** — human opposition or contact; doctrine weighting applies
- **Environmental** — weather, terrain, resource opportunities
- **Scavenge** — salvage and cache finds
- **Chained** — multi-stage narrative sequences

### Chains

| Chain ID | Stages | Theme |
|----------|--------|-------|
| `chain_wandering_pilgrim` | 3 | Help an elder pilgrim reach The Pilgrim's Hearth; unlocks safe route intel |
| `chain_deserter_code` | 3 | A garrison deserter seeking asylum; moral choices compound |
| `chain_merchant_ambush` | 3 | Convoy merchant under raider pursuit; protect or divert |
| `chain_radio_ghost` | 3 | Mysterious repeating radio signal leading to buried cache |

## Stance Weighting

Each encounter carries `stance_weights` — multipliers applied to the expedition's current stance. Aggressive stance weights up combat and confrontation encounters; Cautious weights up evasion-focused options.

| Stance | Effect |
|--------|--------|
| Cautious | High evasion, low violence, slower |
| Balanced | Default 1.0× on all weights |
| Aggressive | High combat, higher loot, higher injury risk |
| Scavenging | High resource finds, moderate danger |
| Rapid | Low encounter frequency, skip opportunities |

## Selection Algorithm

1. Filter by region tag, danger level range, and season tag.
2. For chained encounters: filter by `prereq_chain_stage == current_chain_stage`.
3. Apply `base_weight * stance_weight[currentStance]`.
4. Deterministic weighted selection using `ISeededRng`.
5. Cooldown: same encounter cannot repeat within `cooldown_days` (default 3).

## Chain Stage Logic

- `chain_stage` — which stage this encounter represents (1-based).
- `prereq_chain_stage` — the current chain stage value required for this encounter to be eligible. Stage 1 has `prereq_chain_stage: 0` (default, meaning "chain not yet started"). Stage 2 has `prereq_chain_stage: 2` (meaning "stage 1 has been completed and chain advanced to 2").
- `advances_chain_stage` on a choice — sets the chain to this stage value on resolution.

## Engine Model

```csharp
// Selection
var enc = system.SelectEncounter(state, region, dangerLevel, season, stance);
// Eligibility check
bool ok = system.IsEncounterEligible(enc, region, dangerLevel, season, daysAgo);
// Choice resolution
system.ResolveChoice(state, enc.Id, choiceId, out int moraleDelta, out int guiltDelta, out string fieldGuideUnlock);
// Chain state
int stage = system.GetChainStage(state, "chain_wandering_pilgrim");
// Save state
var state = system.CaptureState();
system.RestoreState(state);
```

## Design Rules

1. Every creature encounter should unlock a field guide entry via `unlocks_field_guide_id` on at least one choice.
2. Non-violent choices must always exist (at least one `is_nonviolent: true` option per encounter).
3. Chain encounters must use `prereq_chain_stage == chain_stage` (not the previous stage's number).
4. All new encounters must have an `id` with `enc_` prefix registered in the catalog.
5. `chain_id` must be in `DefinitionKeys` — adding a new chain requires updating `CatalogIntegrityRules.cs`.
