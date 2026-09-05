# Plan 45 — Balance Audit

## Encounter Weight Distribution

| Patrol | Weight | Territory | Rationale |
|---|---|---|---|
| garrison_checkpoint (v1/v2/v3) | 0.6 each (1.8 group) | controlled | Common, predictable; presentation variants |
| militia_roadblock | 1.5 | controlled | Common, friendly |
| central_garrison_border | 1.5 | border | Common, bureaucratic |
| warlord_raid (v1/v2/v3) | 0.5 each (1.5 group) | contested | Common, dangerous; presentation variants |
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

## Repetition Guardrails (Plan 45 / F13)

To prevent encounter fatigue and immersion breakage during repetitive travel routes:
1. **Cooldown Groups**:
   - Presentation variants share a unified `cooldown_group` key (e.g. `patrol_garrison_checkpoint`, `patrol_warlord_raid`).
   - Triggering any variant places the entire group on a 5-day cooldown (`currentDay + 5`), preventing back-to-back repetitions.
2. **Normalized Weighting (Strategy A)**:
   - Introducing 3 variants does not triple the encounter category's frequency. Instead, individual variant base weights are divided equally across the family:
     - Garrison Checkpoint: 1.8 base / 3 = 0.6 each.
     - Warlord Raid: 1.5 base / 3 = 0.5 each.
3. **Save Migration**:
   - Legacy saves with individual encounter ID cooldowns are consolidated into their respective group cooldown keys upon load using `Math.Max(groupExpiry, memberExpiry)`.
