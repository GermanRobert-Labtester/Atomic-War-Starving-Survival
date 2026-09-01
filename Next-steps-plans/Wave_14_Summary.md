# Wave 14 — Summary (Plans 131–135)

## Wave Overview

Five non-duplicative, implementation-ready plans covering information flow, survivor psychology, expedition consequences, geopolitical dynamics, and environmental depth. Each plan addresses a verified gap in the repository — zero existing systems, zero existing data, zero existing plans covering these capabilities.

| Plan | New Capability | Why It Is Not Duplicate | Risk | Key Systems |
| ---- | -------------- | ----------------------- | ---- | ----------- |
| 131 — Wasteland Information & Rumor Network | Persistent information flow between settlements/factions/player. News, rumors, disinformation propagate via caravans and radio. | No rumor/intel/info system exists. Plan 50/107 (radio distress) is emergency-only. Plan 73 (faction radio) is content, not propagation. | MEDIUM | TravelingCaravan, LocationEvolution, Factions, Economy, Quests |
| 132 — Survivor Hidden Agendas & Betrayal Arc | Survivors carry secret motivations (loyalty/theft/sabotage/escape) that unfold over time. Discovery, confrontation, resolution with 4+ branches each. | Plan 88 (confession secrets) is one-shot forgiveness events. Plan 52 (recurring NPC arcs) covers external NPCs. No plan addresses ongoing internal shelter secrets with investigation mechanics. | HIGH | SurvivorRelations, MoralChoice, Factions, MentalHealth, Quests |
| 133 — Expedition Discovery → Persistent World Consequences | Discoveries permanently alter world state: resource deposits trigger faction interest, cleared threats improve routes, ruins attract scavengers. | Plan 32 (destination wiring) and 76 (destination expansion) add locations, not consequences. Plan 46 (scavenging) adds loot. No plan connects expedition outcomes to persistent world changes. | MEDIUM | LocationEvolution, Factions, Economy, Quests, Caravans |
| 134 — Dynamic Faction Territory & Supply Line Control | Factions expand/contract territory based on military/economic/player pressure. Supply lines connect holdings, can be raided/disrupted. | Plan 44 (territory map) is static data. Plan 45 (patrol encounters) is combat. Plan 124 (location overrides) is content. No plan addresses dynamic territory change or supply-line logistics. | HIGH | Factions, RegionalTreaty, LocationEvolution, Economy, Caravans |
| 135 — Weather → Deep Gameplay Cascade | Weather cascades into shelter damage, expedition risk, faction behavior, economy prices, mental health, location accessibility. | Plan 48 (route gates) adds accessibility gates. Plan 83 (seasons) adds data. No plan addresses weather as a multi-system gameplay driver. | MEDIUM | Shelter, Expeditions, Factions, Economy, MentalHealth, Locations |

## Strongest Plan to Implement First

**Plan 131 — Wasteland Information & Rumor Network.** It has the highest cross-system integration potential (touches factions, locations, economy, quests, expeditions), the clearest repository gap (zero existing systems), the lowest implementation risk (builds on existing caravan/travel infrastructure), and creates the foundation that Plans 132–134 can build on (rumors carry information about survivor agendas, expedition discoveries, and faction territory changes).

## Dependencies Between the 5 Plans

- **Plan 131 (Rumors) is foundational** — Plans 132, 133, and 134 all benefit from an information-flow layer. Survivor agendas spread via rumor. Expedition discoveries generate rumors. Faction territory changes produce rumors.
- **Plan 132 (Hidden Agendas) is standalone** but produces events that feed into Plan 131 (rumors about suspicious behavior).
- **Plan 133 (Discovery Consequences) is standalone** but uses Plan 131 for faction awareness of discoveries.
- **Plan 134 (Territory Control) is standalone** but uses Plan 131 for information about territorial changes.
- **Plan 135 (Weather Cascade) is standalone** — no dependency on other plans in this wave.

## Recommended Implementation Order

1. **Plan 131** — Wasteland Information & Rumor Network (foundation for information flow)
2. **Plan 135** — Weather → Deep Gameplay Cascade (standalone, environmental depth)
3. **Plan 133** — Expedition Discovery → Persistent World Consequences (standalone, uses 131)
4. **Plan 134** — Dynamic Faction Territory & Supply Line Control (standalone, uses 131)
5. **Plan 132** — Survivor Hidden Agendas & Betrayal Arc (most complex, benefits from 131)

## Why This Wave Materially Expands ASHFALL

These five plans transform ASHFALL from a collection of parallel systems into a connected world where information flows, secrets unfold, discoveries matter, factions compete for territory, and weather shapes survival. Each plan creates new player decisions with persistent consequences that ripple across multiple systems — the difference between a simulation and a living world.
