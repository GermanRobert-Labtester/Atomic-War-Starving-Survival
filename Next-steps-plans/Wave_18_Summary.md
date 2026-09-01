# Wave 18 — Summary (Plans 151–155)

## Wave Overview

Five non-duplicative, implementation-ready plans covering new gameplay systems, strategic depth, and morally complex economics. Each plan addresses a verified gap in the repository — areas with zero existing systems or data.

| Plan | New Capability | Why It Is Not Duplicate | Risk | Key Systems |
| ---- | -------------- | ----------------------- | ---- | ----------- |
| 151 — Working Animals & Companion System | Tame, train, and deploy animals for shelter defense, expedition support, morale, and labor. | Plan 28 (wildlife ecology) covers ecology but not domestication. Plan 36 (trapping) adds traps but all catches become resources. Verified: zero animal companion/taming systems exist. | MEDIUM | WildlifeTrapping, WildlifeMigration, ExpeditionSystem, ShelterDefense, TacticalCombat, NeedsSystem, MentalHealthCrisis |
| 152 — Vehicle Customization & Mobile Base | Customize vehicles with modules (armor, cargo, living, weapon, utility). Vehicles serve as mobile bases for expeditions. | Plan 60 (vehicle expansion) adds vehicle types but not customization. Plan 10 (combat/expedition) adds vehicles to combat but not mobile base. Verified: no vehicle customization or mobile shelter mechanics exist. | MEDIUM | ExpeditionVehicleSystem, ExpeditionSystem, TacticalCombatSystem, ResearchSystem, ShelterDefenseSystem, NeedsSystem |
| 153 — Faction Espionage & Sabotage | Infiltrate factions, steal intelligence, sabotage operations, conduct covert actions. Shadow war dimension to faction relations. | Plan 134 (territory control) adds faction competition but not espionage. Plan 139 (combat→faction) connects combat to standing but not covert action. Verified: zero espionage/sabotage systems exist. | HIGH | FactionBranchCoordinator, FactionStanceEngine, ExpeditionSystem, MoralChoiceSystem, SurvivorRelationsSystem, TacticalCombatSystem |
| 154 — Survivor Education & Knowledge Transfer | Children grow up educated through structured schooling. Skills pass from generation to generation. Knowledge becomes persistent shelter resource. | Plan 26 (knowledge/skills) externalizes skill data but doesn't address education. Plan 140 (legacy) adds cross-campaign inheritance but not in-campaign education. Verified: CohortSystem maturation is a boolean flag with no education, no skill transfer, no developmental arc. | MEDIUM | CohortSystem, ApprenticeshipSystem, LibraryStudySystem, GenerationalLineageExtension, SkillProgressionSystem, SurvivorRelationsSystem |
| 155 — Black Market & Underground Economy | Illegal trade, contraband smuggling, shadowy dealings for profit. Morally ambiguous economic layer with detection risk and consequences. | Plan 13 (economy survival loop) adds goods/recipes but not illegal trade. Plan 146 (radiation→economy) adds contaminated goods but not black market. Verified: zero black market/contraband/underground economy systems exist. | HIGH | MarketSystem, HoldfastTradeSession, FactionStanceEngine, MoralChoiceSystem, InventorySystem, NeedsSystem |

## Strongest Plan to Implement First

**Plan 151 — Working Animals & Companion System.** It creates an entirely new gameplay layer with zero existing infrastructure to conflict with, has clear scope (taming, training, deployment), moderate risk, and immediate player value (animal companions create emotional bonds and strategic options). It also builds naturally on the existing `WildlifeTrappingSystem` by giving cage-trapped animals a second purpose beyond meat/hides.

## Dependencies Between the 5 Plans

- **Plan 151 (Animals) is standalone** — no dependencies on other plans in this wave.
- **Plan 152 (Vehicles) is standalone** but benefits from Plan 151 (pack animals complement vehicles for cargo).
- **Plan 153 (Espionage) is standalone** but can use Plan 151 animals (scout birds, guard dogs for safehouses) and Plan 152 vehicles (getaway cars, mobile safehouses).
- **Plan 154 (Education) is standalone** but educated survivors make better agents (Plan 153) and can train animal handlers (Plan 151).
- **Plan 155 (Black Market) is standalone** but contraband can include animal parts (Plan 151), vehicle modifications (Plan 152), stolen intelligence (Plan 153), and forged educational credentials (Plan 154).

## Recommended Implementation Order

1. **Plan 151** — Working Animals & Companion System (new gameplay, standalone, emotional engagement)
2. **Plan 154** — Survivor Education & Knowledge Transfer (generational depth, standalone)
3. **Plan 152** — Vehicle Customization & Mobile Base (strategic depth, builds on existing vehicle system)
4. **Plan 153** — Faction Espionage & Sabotage (faction depth, complex but high value)
5. **Plan 155** — Black Market & Underground Economy (moral complexity, economic depth)

## Why This Wave Materially Expands ASHFALL

These five plans add entirely new dimensions to ASHFALL that didn't exist before: animal companions that create emotional bonds and strategic options, vehicles that become customizable mobile bases rather than just transport, a shadow war of espionage and sabotage between factions, an education system that gives children meaning and passes knowledge across generations, and a black market that creates morally ambiguous profit opportunities. Each plan opens up gameplay space that was completely absent — the shelter has dogs on guard duty, children attend school, trucks become rolling fortresses, spies infiltrate rival camps, and contraband flows through back alleys. This is the wave that turns ASHFALL from a survival management game into a living, breathing post-collapse world.
