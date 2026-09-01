# Plan 17 — Environmental State Matrix

Maps location states to their authoritative systems and environmental text selection paths.

## Location State Authorities

| State | Authoritative System | Persistence | Notes |
|-------|---------------------|-------------|-------|
| First visit | `LocationEvolutionSystem.lastVisitedDay` | Yes | 0 = never visited |
| Revisited | `LocationEvolutionSystem.lastVisitedDay > 0` | Yes | Any positive day |
| Post-loot | `LocationEvolutionSystem.lootDepletionFactor` | Yes | 0.0 = fully depleted, 1.0 = full |
| Post-conflict | `LocationEvolutionSystem.activeThreats` | Yes | Empty = cleared |
| Post-strike | `LocationEvolutionSystem.isRuined` | Yes | Boolean flag |
| Occupied | `LocationEvolutionSystem.currentOwner` | Yes | Faction ID or empty |
| Abandoned | `LocationEvolutionSystem.currentOwner == ""` | Yes | No owner |
| Restored | `LocationMemorySystem` mutation flags | Yes | Specific flag per site |
| Destroyed | `LocationEvolutionSystem.isRuined` | Yes | Permanent state |
| Contaminated | `LocationEvolutionSystem.contaminationLevel` | Yes | Numeric level |

## State Selection Precedence

When multiple states apply, use this precedence order (highest first):

1. **Destroyed** — `isRuined == true` overrides all other states
2. **Restored** — mutation flag active (specific per site)
3. **Post-strike** — recent damage event
4. **Post-conflict** — `activeThreats` cleared
5. **Post-loot** — `lootDepletionFactor < 0.3`
6. **Occupied** — `currentOwner` set
7. **Revisited** — `lastVisitedDay > 0`
8. **First visit** — default

## Environmental Text Selection

```
GetAtmosphereText(locationId):
  1. Check LocationEvolutionSystem state
  2. If isRuined → select "destroyed" variant
  3. If mutation flag active → select "restored" variant
  4. If lootDepletionFactor < 0.3 → select "depleted" variant
  5. If currentOwner set → select "occupied" variant
  6. If lastVisitedDay > 0 → select "revisited" variant
  7. Else → select "first_visit" variant
  8. If weather != "clear" → overlay weather variant
  9. Return selected text (or null if no match)
```

## State-Aware Text Variants (Required)

For high-exposure locations, provide these variants:

| State | Variant Tag | Example |
|-------|-------------|---------|
| First visit | `first_visit` | "The corridor stretches ahead, untouched..." |
| Revisited | `revisited` | "You've been here before. The dust has settled..." |
| Post-loot | `depleted` | "The shelves are bare. Nothing of value remains..." |
| Post-conflict | `cleared` | "The threats are gone, but the damage remains..." |
| Post-strike | `damaged` | "Fresh scorch marks blacken the walls..." |
| Occupied | `occupied` | "New boot prints in the dust. Someone's using this place..." |
| Abandoned | `abandoned` | "Cobwebs and silence. No one has been here in months..." |
| Restored | `restored` | "The repairs hold. Someone cared enough to fix this..." |
| Destroyed | `destroyed` | "Nothing left but rubble and the memory of walls..." |

## Weather Overlays

Weather variants modify or replace base text for major locations:

| Weather | Overlay Tag | Priority |
|---------|-------------|----------|
| `fallout_storm` | `storm` | Highest — overrides base |
| `snow` | `snow` | High |
| `rain` | `rain` | Medium |
| `wind` | `wind` | Medium |
| `fog` | `fog` | Low |
| `clear` | (base) | Default |

**Rule:** Weather does not erase important state-specific text. A destroyed location is still destroyed in a storm; the storm adds atmosphere, not contradiction.

## Deterministic Selection

- Same location state + same weather → same text (deterministic)
- Use `ISeededRng` for variety within bounded sets
- Persist selection if it must remain stable after save/load
- No wall-clock time, no unordered iteration, no platform locale

## Save/Load Compatibility

- Old saves: `lastVisitedDay == 0` → first-visit text
- Old saves: `lootDepletionFactor == 0.0` → depleted text (may be inaccurate for old looted locations)
- Old saves: no mutation flags → no restored text
- New fields default safely; no fabricated history

## Location Coverage Tiers

| Tier | Definition | Minimum Variants |
|------|-----------|-----------------|
| Tier 1 | Major location (faction hub, quest hub, deep-lore) | 4+ meaningful states |
| Tier 2 | Recurring location (expedition site, trade route) | 2–3 variants |
| Tier 3 | Minor location (transit, background) | 1 direct or strong tagged variant |

**Note:** Not all locations need unique prose for every state. Tier 3 locations can use generic fallbacks.
