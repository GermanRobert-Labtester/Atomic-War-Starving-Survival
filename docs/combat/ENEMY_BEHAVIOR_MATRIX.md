# Enemy Behavior & Combatant Matrix

**Document:** `docs/combat/ENEMY_BEHAVIOR_MATRIX.md`
**Catalog Authority:** `Assets/StreamingAssets/Data/combat_catalog.json`
**Runtime System:** [`Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs`](../../Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs)

---

## 1. Authored Combatant Archetypes

| Combatant ID | Kind | HP | Armor / Cover | Preferred Lane | Stance | Special AI Move | Surrender / Flee Threshold | Tactical Identity & Counterplay |
|---|---|---|---|---|---|---|---|---|
| `combatant_burrower_mite` | Fauna | 70 | 0.10 / 0.05 | Lane 0 (Flank) | Advance | Burrow | -1.0 / -1.0 | Aggressive flank predator; punishes static center loadouts. Vulnerable to fast close-range fire. |
| `combatant_spore_hound` | Fauna | 90 | 0.15 / 0.10 | Lane 2 (Flank) | Advance | Spore | -1.0 / 0.30 | Fast pack hunter; deploys contaminating plumes. Flees at 30% health. |
| `combatant_armored_boar` | Fauna | 140 | 0.45 / 0.30 | Lane 1 (Center) | HoldPosition | Charge | -1.0 / -1.0 | Heavy center anchor with thick calcified hide. Requires armor-piercing or high-damage rounds. |
| `combatant_feral_mutt` | Fauna | 60 | 0.05 / 0.10 | Lane 0 (Flank) | Advance | Flank | -1.0 / 0.55 | Fast pack runner with low durability; breaks and flees early when injured (55%). |
| `combatant_pale_crawler` | Mutant | 80 | 0.20 / 0.15 | Lane 2 (Flank) | Advance | Flank | -1.0 / -1.0 | Ambush stalker lurking in ruins; high accuracy (1.10x) and flank positioning. |
| `combatant_chrome_loper` | Mutant | 110 | 0.30 / 0.05 | Lane 1 (Center) | Advance | Charge | -1.0 / 0.25 | Bipedal sprint charger; closes distance rapidly and strikes with hardened forelimbs. |
| `combatant_conscript_levy` | Human | 85 | 0.25 / 0.45 | Lane 1 (Center) | HoldPosition | None | 0.45 / 0.65 | Poorly trained checkpoint guard; surrenders under pressure (45%) or flees (65%). Open to bribery. |
| `combatant_warlord_veteran` | Human | 110 | 0.45 / 0.55 | Lane 1 (Center) | HoldPosition | SuppressiveFire | 0.20 / 0.35 | Disciplined veteran fighter; utilizes cover and suppressive fire. Requires high-penetration tactics or heavy tribute. |
| `combatant_flotilla_marine` | Human | 95 | 0.30 / 0.50 | Lane 1 (Center) | HoldPosition | SuppressiveFire | 0.30 / 0.40 | Coastal specialist trained in close-quarters and cover fire; open to barter/passage agreements. |
| `combatant_desperate_scavenger` | Human | 75 | 0.10 / 0.30 | Lane 2 (Flank) | Retreat | TacticalRetreat | 0.55 / 0.75 | Opportunistic scavenger who retreats when outmatched; highly receptive to bribery, barter, or food bribes. |
