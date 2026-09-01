# Wave 27 — Summary (Plans 196–200)

## Wave Overview

Five non-duplicative, implementation-ready plans covering culinary realism, geopolitical depth, medical continuity, demographic dynamics, and character-driven narrative. This wave focuses on **realism and story** — the systems that make ASHFALL's world feel lived-in, dynamic, and personally meaningful through accumulated detail and individual journeys.

| Plan | New Capability | Why It Is Not Duplicate | Risk | Key Systems |
| ---- | -------------- | ----------------------- | ---- | ----------- |
| 196 — Food Type Differentiation & Temperature Spoilage | Different food categories (meat, vegetables, dairy, grains, etc.) spoil at different rates, temperature affects spoilage speed, preservation methods are type-specific. | `KitchenNutritionSystem` (288 lines) has basic spoilage timers and preservation methods but no food type differentiation — all items use same spoilage formula. No temperature-dependent spoilage. No type-specific preservation. Verified: ZERO matches for `FoodType`, `MeatSpoilage`, `DairySpoilage`, `TemperatureSpoilage` in Core. | LOW | KitchenNutritionSystem, ShelterThermalSystem, WeatherSystem, DiseaseSystem, NeedsSystem, InventorySystem |
| 197 — Faction Diplomacy & Treaty System | Negotiate formal agreements (non-aggression pacts, trade alliances, mutual defense, intelligence sharing) with factions, manage diplomatic relations through envoys and negotiations, build alliance networks. | `FactionStanceEngine` tracks trust/trade stances. `FactionBranchCoordinator` (661 lines) coordinates branches. `HoldfastTradeSession` (682 lines) handles faction-gated trade. But no formal diplomacy system, no treaty negotiation, no alliance formation, no diplomatic missions. Verified: ZERO matches for `DiplomacySystem`, `TreatyNegotiation`, `AllianceSystem`, `DiplomaticRelation` in Core. | LOW | FactionStanceEngine, FactionBranchCoordinator, HoldfastTradeSession, RegionalTreatySystem, CombatSystem, ExpeditionSystem |
| 198 — Health History & Medical Records | Each survivor maintains persistent medical history — tracking illnesses, injuries, treatments, vaccinations, radiation exposure, long-term health trends over time. | Medical systems treat each condition as isolated event with no memory of past health issues. `DoseLedgerSystem` tracks radiation dose but not general health. No medical history, no treatment records, no vaccination history, no long-term health tracking. Verified: ZERO matches for `HealthHistory`, `MedicalRecord`, `HealthTracking`, `MedicalHistory` in Core. | LOW | MedicalPipelineCoordinator, DiseaseSystem, RadiationSystem, DoseLedgerSystem, CombatTraumaSystem, ChronicConditionSystem |
| 199 — Seasonal Migration (Human/Faction) | Human populations (refugees, traders, faction members) and caravans follow seasonal movement patterns — migrating to safer/prosperous areas during harsh seasons, returning when conditions improve. | `WildlifeMigrationSystem` (125 lines) tracks wildlife migration. `TravelingCaravanSystem` (268 lines) runs NPC caravans on fixed routes. But no seasonal human migration — no refugee flows, no seasonal trader movements, no faction relocations, no population shifts. Plan 135 mentions "seasonal migration patterns" as follow-on but doesn't implement. Verified: ZERO matches for `SeasonalMigration`, `MigrationRoute`, `PopulationMigration` in Core. | LOW | WildlifeMigrationSystem, TravelingCaravanSystem, WeatherSystem, FactionBranchCoordinator, ExpeditionSystem, HoldfastTradeSession |
| 200 — Survivor Personal Quests & Character Arcs | Each survivor develops individual storylines based on experiences, relationships, traits, history — creating unique character development paths that make each survivor feel like a protagonist with their own journey. | Survivors gain skills, form relationships, experience events, but no personal questlines, no character arcs, no individual story development. Plan 132 (hidden agendas) adds secret motivations but not open character arcs. Plan 147 (memory) adds recall but not narrative development. Verified: ZERO matches for `PersonalQuest`, `CharacterArc`, `SurvivorStory`, `PersonalStoryline` in Core. | LOW | SurvivorRelationsSystem, SkillProgressionSystem, TraitSystem, EventSystem, JournalSystem, PsychologicalProfileSystem |

## Strongest Plan to Implement First

**Plan 198 — Health History & Medical Records.** It has the lowest risk, clearest scope, and most immediate value for medical gameplay. Medical records create continuity (survivors have medical histories), inform strategic decisions (medics can see patient history), and add realism (health isn't just current state but accumulated experience). It's also the simplest to implement (auto-generate records from existing medical events) with broad integration across all medical systems.

## Dependencies Between the 5 Plans

- **Plan 196 (Food Types) is standalone** — extends kitchen nutrition with food categories.
- **Plan 197 (Diplomacy) is standalone** — adds treaty layer over existing faction relations.
- **Plan 198 (Health History) integrates with Plan 193** — chronic conditions create medical records.
- **Plan 199 (Seasonal Migration) is standalone** — adds demographic movement to world.
- **Plan 200 (Personal Quests) integrates with multiple systems** — quests based on relationships, skills, events, traits.

## Recommended Implementation Order

1. **Plan 198** — Health History & Medical Records (medical continuity, lowest risk, broadest medical integration)
2. **Plan 196** — Food Type Differentiation & Temperature Spoilage (culinary realism, low risk, extends kitchen)
3. **Plan 197** — Faction Diplomacy & Treaty System (geopolitical depth, low risk, extends factions)
4. **Plan 199** — Seasonal Migration (demographic dynamics, low risk, adds world life)
5. **Plan 200** — Survivor Personal Quests & Character Arcs (character-driven narrative, low risk, deepest integration)

## Rejected Candidates (Considered but Not Selected)

- **Player Radio Station Content** — Plan 157 (Communications Radio Network Infrastructure) covers player radio transmission, antenna construction, network building, message broadcasting. Not a gap.
- **Psychological Profiling** — Plan 179 (Unified Psychology & Phobia System) covers psychological profiles, phobia development, personality evolution. Not a gap.
- **Building Construction Phases** — Plan 156 (Shelter Expansion Physical Renovation) covers construction projects, building phases, construction queues. Not a gap.
- **Radiation Mutations** — Plan 172 (Radiation Mutation System) covers permanent genetic changes from radiation. Not a gap.
- **Underground Bunker Network** — Plan 167 (Underground Tunnel Network) covers tunnel discovery, exploration, mapping, maintenance. Not a gap.
- **Trade Caravan Management** — Plan 192 (Player Trade Route Establishment) covers caravan dispatch, scheduling, management. Not a gap.
- **Burial Ground/Cemetery Management** — `MemorialSystem` (262 lines) handles burial outcomes. Rejected as too thin for full plan, better as MemorialSystem extension.

## Why This Wave Materially Expands ASHFALL

These five plans transform ASHFALL from a game with functional systems into one where the world has continuity, depth, and personal meaning: food that spoils differently based on type and temperature (making culinary management strategic), factions you can build formal relationships with through diplomacy and treaties (making geopolitics meaningful), survivors with medical histories that track their health journey over time (making healthcare continuous), a world where populations move seasonally creating dynamic demographic shifts (making the world feel alive), and survivors who develop personal quests and character arcs based on their unique experiences (making each survivor a protagonist with their own story). This is the wave that makes ASHFALL's world feel continuous, dynamic, and personally meaningful — where every meal has consequences, every faction relationship has depth, every medical treatment is recorded, every season brings new faces, and every survivor has a story worth telling.

## Cumulative Wave Themes (Waves 14–27)

| Wave | Theme | Plans |
| ---- | ----- | ----- |
| 14 | Information flow & hidden knowledge | 131–135 |
| 15 | Dead-end fixes & cross-system bridges | 136–140 |
| 16 | Research, clothing, medical, autonomy, endings | 141–145 |
| 17 | Radiation, memory, friction, achievements, romance | 146–150 |
| 18 | Animals, vehicles, espionage, education, black market | 151–155 |
| 19 | Shelter, communications, disasters, governance, colonies | 156–160 |
| 20 | Hobbies, archive, cartography, nuclear winter, modding | 161–165 |
| 21 | Identity, tunnels, propaganda, audio, celebrations | 166–170 |
| 22 | Dynamic quests, mutations, radio, backstories, meta-progression | 171–175 |
| 23 | Aging, dreams, art, psychology, certifications | 176–180 |
| 24 | Difficulty, relationship decay, child development, accessibility, memory decay | 181–185 |
| 25 | Shelter maintenance, bestiary, survivor routines, water sources, item lore | 186–190 |
| 26 | Item identification, trade routes, chronic conditions, emergency alerts, survivor roles | 191–195 |
| **27** | **Food types, diplomacy, health records, seasonal migration, personal quests** | **196–200** |

**Total: 70 plans across 14 waves (131–200), plus 14 wave summaries.**

## Milestone Achievement

**Wave 27 completes Plan 200** — a significant milestone in the ASHFALL planning journey. From Plan 1's basic systems to Plan 200's character-driven narratives, the plans have built a comprehensive vision for a deep, realistic, personally meaningful post-nuclear survival experience. The next wave (28) will begin a new century of plans, continuing to expand ASHFALL's scope and depth.
