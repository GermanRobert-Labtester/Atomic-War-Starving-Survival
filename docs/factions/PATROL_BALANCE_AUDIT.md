# Plan 45 — Balance Audit

## Encounter Weight Distribution

| Patrol | Weight | Territory | Rationale |
|---|---|---|---|
| garrison_checkpoint | 1.8 | controlled | Common, predictable |
| militia_roadblock | 1.5 | controlled | Common, friendly |
| central_garrison_border | 1.5 | border | Common, bureaucratic |
| warlord_raid | 1.5 | contested | Common, dangerous |
| railway_convoy | 1.2 | controlled | Uncommon, beneficial |
| foundry_supply | 1.2 | controlled | Uncommon, trade |
| hydro_escort | 1.0 | controlled | Uncommon, beneficial |
| supply_corps_convoy | 1.0 | controlled | Uncommon, beneficial |
| ash_sign_scouts | 1.0 | contested | Uncommon, informational |
| scavenger_raid | 1.0 | contested | Uncommon, dangerous |
| warlord_press_gang | 0.8 | contested | Rare, morally difficult |
| refugee_eviction | 0.8 | controlled | Rare, morally difficult |
| cult_recon | 0.8 | contested | Rare, atmospheric |
| penal_battalion | 0.8 | contested | Rare, atmospheric |
| black_ops_ambush | 0.6 | contested | Very rare, severe |

## Stance Weight Effects
- **Cautious**: boosted for checkpoints/borders, reduced for raids
- **Aggressive**: boosted for raids, reduced for checkpoints
- **Rapid**: reduced for all patrols (trying to pass through quickly)
- **Scavenging**: variable by patrol type

## Standing Delta Range
- Minimum: -20 (Black Ops fight)
- Maximum: +8 (Warlord press gang side_with)
- Most choices: -5 to +3
- Combat choices: -12 to -20

## Cost Range
- Cheapest: 1×bandage, 1×canned_food, 1×currency
- Most expensive: 5×canned_food (press gang negotiate)
- All costs are within normal expedition carry capacity
