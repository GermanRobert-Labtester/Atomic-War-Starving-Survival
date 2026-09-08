# Wave 20 — Summary (Plans 161–165)

## Wave Overview

Five non-duplicative, implementation-ready plans covering personality depth, historical memory, exploration, environmental progression, and community extensibility. Each plan addresses a verified gap — areas with zero existing systems or only superficial coverage.

| Plan | New Capability | Why It Is Not Duplicate | Risk | Key Systems |
| ---- | -------------- | ----------------------- | ---- | ----------- |
| 161 — Survivor Hobby & Leisure System | Survivors pursue personal interests, pastimes, and creative activities. Hobbies provide morale, personality depth, and emergent stories. | Plan 12 (social/shelter life) mentions decor but not hobbies. Plan 144 (autonomy) adds behavior but not structured leisure. Verified: survivors have no hobbies, no pastimes, no creative outlets — they are workers, not people. | LOW | NeedsSystem, SkillProgression, SurvivorRelations, Apprenticeship, ShelterExpansion, MentalHealthCrisis |
| 162 — Shelter History & Archive System | Significant events, decisions, and milestones automatically recorded as shelter's collective memory. Institutional archive preserves the community's journey. | Plan 17/51 (environmental storytelling) adds lore but not shelter-specific history. Plan 140 (legacy) adds cross-campaign but not in-campaign record. Verified: no institutional archive, no shelter history, no collective memory — the shelter's journey is forgotten. | LOW | JournalSystem, MemorialSystem, CampaignConsequenceLedger, GovernanceSystem, DisasterResponseSystem, ColonySystem |
| 163 — Wasteland Cartography & Mapping | Fog of war, region discovery, cartography skill, map trading. World is unknown territory to be explored and charted rather than a fixed destination list. | Plan 11 (world exploration) mentions living map but not cartography. Plan 32 (destination wiring) connects destinations but not discovery. Verified: no fog of war, no cartography skill, no map discovery, no map trading — world is known from start. | LOW | ExpeditionSystem, LocationEvolution, SkillProgression, FactionBranchCoordinator, MarketSystem, ColonySystem |
| 164 — Nuclear Winter Progression System | Climate worsens over time through 5 phases with seasonal cycles. Temperature, storms, radiation, and daylight create escalating environmental pressure. | Plan 19/83 (weather/seasons) adds content but not progression. Plan 135 (weather cascade) connects weather but nuclear winter is static. Verified: Day 1 and Day 365 have same climate — no worsening, no seasonal severity, no long-term change. | MEDIUM | WeatherSystem, ShelterThermal, ExpeditionSystem, ClothingWarmth, ResearchSystem, DisasterResponseSystem |
| 165 — Modding Support & Mod Data Contract | Mod loading infrastructure, mod manifest format, data contract, validation, tools, and documentation. Community can create and install mods. | No plan addresses modding. Data is JSON (theoretically mod-safe) but no mod loading, no manifest, no tools, no documentation. Verified: players cannot create or install mods despite data-driven architecture. | MEDIUM | All CatalogLoaders, CatalogIntegrityValidator, SaveSystem, GameBootstrap, UI |

## Strongest Plan to Implement First

**Plan 161 — Survivor Hobby & Leisure System.** It has the lowest risk, clearest scope, and immediate player value (survivors become people with interests, not just workers). It creates emotional depth and emergent stories with minimal system complexity. It also integrates naturally with existing systems (morale, skills, relationships) without requiring major architectural changes.

## Dependencies Between the 5 Plans

- **Plan 161 (Hobbies) is standalone** — no dependencies on other plans in this wave.
- **Plan 162 (Archive) is standalone** but can record hobby achievements (161), mapping discoveries (163), climate events (164), and mod installations (165).
- **Plan 163 (Cartography) is standalone** but discovery events recorded in archive (162).
- **Plan 164 (Nuclear Winter) is standalone** but climate events recorded in archive (162), affect expedition safety (163).
- **Plan 165 (Modding) is standalone** but mods can add hobbies (161), map regions (163), climate phases (164).

## Recommended Implementation Order

1. **Plan 161** — Survivor Hobby & Leisure System (personality depth, low risk)
2. **Plan 162** — Shelter History & Archive System (historical memory, low risk)
3. **Plan 163** — Wasteland Cartography & Mapping (exploration depth, low risk)
4. **Plan 164** — Nuclear Winter Progression System (environmental progression, medium risk)
5. **Plan 165** — Modding Support & Mod Data Contract (community extensibility, medium risk)

## Why This Wave Materially Expands ASHFALL

These five plans transform ASHFALL from a game with functional systems into one with soul: survivors who have hobbies and personalities (not just stats), a shelter that remembers its journey (not just exists), a world that is unknown and waiting to be discovered (not just a menu), an environment that is slowly dying (not just static challenge), and a game that can be extended by its community (not just consumed). This is the wave that turns ASHFALL from a survival management simulation into a living, breathing, memorable, extensible world.
