# Wave 15 — Summary (Plans 136–140)

## Wave Overview

Five non-duplicative, implementation-ready plans covering dead-end fixes, cross-system bridges, shelter mechanics, and long-term legacy. Each plan addresses a verified gap in the repository — systems that exist but are isolated, mechanics that are missing, or skeletons that need deepening.

| Plan | New Capability | Why It Is Not Duplicate | Risk | Key Systems |
| ---- | -------------- | ----------------------- | ---- | ----------- |
| 136 — Wildlife Trapping → Food Pipeline & Cooking System | Connects dead-end trapping to inventory/needs. Adds cooking with radiation removal, recipes, skill. | Plan 13/36 mention trapping but don't fix the inventory dead end. No cooking system exists. Verified: `WildlifeTrappingSystem` has zero `AddItem` references. | MEDIUM | WildlifeTrapping, Inventory, Needs, KitchenNutrition, Radiation |
| 137 — Needs → Performance Cascade | Hunger/fatigue/thirst/warmth affect combat, work, and expedition performance. | Plan 1/14 mention needs but not performance cascades. Verified: `TacticalCombatSystem`, `DutyRosterSystem`, `ExpeditionSystem` have zero `NeedsSystem` references. | MEDIUM | NeedsSystem, TacticalCombat, DutyRoster, Expedition, SomaticFlashback |
| 138 — Shelter Defense & Visitor/Refugee System | Shelter can be attacked; outsiders arrive seeking shelter/trade/aid. Player decides to admit, defend, or expel. | Plan 29/41 cover shelter rooms/identity but not defense. Plan 45 covers patrols but not shelter attacks. Verified: no `ShelterDefense` system, no `Visitor` system exist. | HIGH | AirlockSecurity, DutyRoster, TacticalCombat, Factions, MoralChoice, Economy |
| 139 — Combat → Faction Standing Bridge | Fighting faction-tagged combatants changes faction standing. Killing allies angers them; helping allies earns favor. | Plan 45/54/63/92 cover combat content but not standing consequences. Verified: `FactionBranchCoordinator` standing is never modified by combat. | MEDIUM | TacticalCombat, CombatCatalog, FactionBranchCoordinator, FactionStanceEngine, Expeditions |
| 140 — Generational Legacy & Campaign Inheritance | Completed campaigns leave persistent traits/shelter/faction memory for future runs. New Game+ inheritance. | Plan 15 mentions "New Game+ legacy inheritance" but doesn't implement it. `GenerationalSuccessionEngine` exists (164 lines) but is thin. No cross-campaign persistence exists. | HIGH | GenerationalSuccession, CohortSystem, HoldfastEndings, EpilogueMatrix, Factions, Shelter |

## Strongest Plan to Implement First

**Plan 136 — Wildlife Trapping → Food Pipeline & Cooking System.** It fixes a verified dead end (trapping produces catches that never reach inventory), has the clearest scope (connect existing system + add cooking layer), lowest risk (builds on existing infrastructure), and immediate player value (trapping becomes a viable food source). It also creates a foundation for Plan 137 (cooked food affects performance).

## Dependencies Between the 5 Plans

- **Plan 136 (Trapping/Cooking) is standalone** but produces food that Plan 137 can use (cooked food improves performance).
- **Plan 137 (Needs→Performance) is standalone** but benefits from Plan 136 (food availability affects hunger penalties).
- **Plan 138 (Shelter Defense/Visitors) is standalone** but visitors can trigger combat (Plan 139) and defense uses needs (Plan 137).
- **Plan 139 (Combat→Faction) is standalone** but faction standing affects visitors (Plan 138) and legacy (Plan 140).
- **Plan 140 (Legacy) depends on all others** — legacy traits are earned from gameplay in Plans 136–139.

## Recommended Implementation Order

1. **Plan 136** — Wildlife Trapping → Food Pipeline & Cooking (dead-end fix, immediate value)
2. **Plan 137** — Needs → Performance Cascade (cross-system bridge, builds on 136)
3. **Plan 139** — Combat → Faction Standing Bridge (cross-system bridge, standalone)
4. **Plan 138** — Shelter Defense & Visitor/Refugee System (complex, uses 137/139)
5. **Plan 140** — Generational Legacy & Campaign Inheritance (meta-system, uses all others)

## Why This Wave Materially Expands ASHFALL

These five plans transform ASHFALL from a collection of isolated systems into a connected, persistent world where survival has meaning (trapping feeds you, needs affect performance), violence has consequences (combat affects factions), shelter is contested (defense/visitors), and campaigns build on each other (legacy inheritance). Each plan closes a verified gap — dead ends fixed, bridges built, skeletons deepened — creating a game where every system feeds into every other and every choice echoes forward.
