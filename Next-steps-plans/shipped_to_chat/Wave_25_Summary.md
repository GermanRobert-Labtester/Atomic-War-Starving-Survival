# Wave 25 — Summary (Plans 186–190)

## Wave Overview

Five non-duplicative, implementation-ready plans covering shelter realism, discovery tracking, personal depth, resource management, and narrative enrichment. This wave focuses on **infrastructure and story** — the systems that make ASHFALL's world feel lived-in, tracked, and meaningful through accumulated history.

| Plan | New Capability | Why It Is Not Duplicate | Risk | Key Systems |
| ---- | -------------- | ----------------------- | ---- | ----------- |
| 186 — Shelter Maintenance & Degradation | Bunker components (air filters, walls, power, water recycler, blast doors) deteriorate over time and require maintenance, repairs, and part replacements. | Plan 158 (disaster response) handles acute crises but not gradual degradation. Plan 135 (weather cascade) affects shelter through modifiers but not component wear. Verified: ZERO matches for `ShelterMaintenance`, `BuildingDegradation`, `AirFilterDegradation`, `WallIntegrity`, `BunkerCondition` in Core. | LOW | ShelterThermal, PowerGrid, WaterTreatment, Ventilation, Radiation, Weather |
| 187 — Bestiary & Creature Encounter Tracking | Discover creatures, track encounters/kills/sightings, unlock behavioral notes, build a comprehensive wasteland fauna catalog. | `WastelandBestiaryCatalog.cs` (105 lines) provides static creature data but no encounter tracking, no kill counts, no sighting logs, no discovery system, no bestiary UI. Verified: ZERO matches for `CreatureEncounter`, `CreatureSighting`, `KillCount`, `CreatureDiscovery`, `bestiary_panel` in Core and plans. | LOW | WastelandBestiaryCatalog, Journal, Expedition, Combat |
| 188 — Individual Survivor Daily Routines | Each survivor has a personal schedule (wake/sleep/work/meal/social/personal time) with preferences (chronotype, work style) and satisfaction tracking. | `ShelterScheduleSystem.cs` (240 lines) handles shelter-level phases (Day/Night/Curfew/Emergency) and `DutyRosterSystem` assigns work tasks, but no individual routines, no wake/sleep times, no meal scheduling, no personal time blocks. Verified: ZERO matches for `DailyRoutine`, `PersonalSchedule`, `WakeTime`, `SleepTime`, `SurvivorRoutine` in Core and plans. | LOW | ShelterSchedule, DutyRoster, Needs, SurvivorRelations, MentalHealthCrisis |
| 189 — Water Source Management & Contamination Network | Discover, monitor, and manage multiple water sources (wells, rivers, springs, rain collectors) with individual contamination levels, flow rates, infrastructure, and source switching. | `WaterTreatmentSystem.cs` (634 lines) handles purification with single `incomingContaminationLevel`. `LocationEvolutionSystem` tracks per-location contamination. `HydroGeologyCatalog` has static well data. But no unified water source network, no individual source tracking, no contamination propagation, no infrastructure management. Verified: ZERO matches for `WaterSource`, `WaterNetwork`, `WaterInfrastructure`, `WellManagement` in Core and plans. | LOW | WaterTreatment, LocationEvolution, Disease, Weather, Expedition, Greenhouse |
| 190 — Item Lore & Provenance Tracking | Significant items carry history — who crafted them, where found, who owned them, what events they witnessed. Items accumulate lore and significance over time. | `ProceduralItemInstance.cs` tracks condition/contamination/caloric value but no history, no provenance, no lore. Items are interchangeable units with no memory. Verified: ZERO matches for `ItemLore`, `ItemHistory`, `ItemProvenance`, `CraftedHistory`, `ItemOrigin`, `OwnershipChain` in Core and plans. | LOW | ProceduralItemInstance, Crafting, Expedition, Combat, SurvivorRelations, Trade |

## Strongest Plan to Implement First

**Plan 186 — Shelter Maintenance & Degradation System.** It has the lowest risk, clearest scope, and most immediate gameplay impact. Shelter maintenance creates a tangible survival loop (components degrade → player maintains → failures create drama) that integrates with nearly every existing shelter system. It's also the most visually representable (condition bars, warning indicators, repair animations) and generates the most emergent stories (filter failure during storm, wall crack letting in radiation).

## Dependencies Between the 5 Plans

- **Plan 186 (Shelter Maintenance) is standalone** — extends existing shelter systems.
- **Plan 187 (Bestiary) is standalone** — tracks encounters from existing expedition/combat systems.
- **Plan 188 (Survivor Routines) integrates with shelter schedule** — personal routines respect shelter phases.
- **Plan 189 (Water Sources) integrates with shelter maintenance** — water infrastructure requires maintenance.
- **Plan 190 (Item Lore) is standalone** — attaches lore to items from any system.

## Recommended Implementation Order

1. **Plan 186** — Shelter Maintenance & Degradation (shelter realism, lowest risk, broadest integration)
2. **Plan 189** — Water Source Management (resource management, low risk, extends water treatment)
3. **Plan 187** — Bestiary & Creature Tracking (discovery tracking, low risk, extends expedition/combat)
4. **Plan 188** — Individual Survivor Routines (personal depth, low risk, extends shelter schedule)
5. **Plan 190** — Item Lore & Provenance (narrative enrichment, low risk, extends inventory)

## Rejected Candidates (Considered but Not Selected)

- **Faction AI Autonomy** — Rejected in Waves 22, 23, 24, and 25. Plan 134 (Dynamic Faction Territory) covers faction expansion/contraction. Plan 30 documents that `FactionWarSystem.SimulateDailyFriction` exists but has zero callers — this is a wiring gap, not a missing system. Improvement, not new system.
- **Discrete Weather Events** — Plan 135 (Weather Deep Gameplay Cascade) covers weather events extensively with `WeatherEvent` DTO, weather cascade, and weather shelter events. Not a gap.
- **Nutritional Modeling (Calories/Macros)** — `KitchenNutritionSystem.cs` already exists with spoilage tracking, preservation methods (RootCellar/Refrigeration/Fermentation/Smoking/Canning), nutrition scoring, and caloric data in items (`CaloricValueKcal` in `ProceduralItemInstance`). Partial coverage exists.
- **Individual Water Contamination** — Subsumed into Plan 189 (Water Source Management). Not separate.
- **Scouting/Reconnaissance Missions** — "Scout" appears as a duty type and in narrative text. Plan 151 (working animals) adds scout birds. Plan 133 (expedition consequences) mentions scouting. Too thin for a full plan, better as a feature within expedition system.

## Why This Wave Materially Expands ASHFALL

These five plans transform ASHFALL from a game with static systems into one where the world has memory and infrastructure ages: shelter components that deteriorate and need care (making the bunker feel like a living structure), creatures that are discovered and tracked (making exploration feel rewarding), survivors with personal rhythms and preferences (making them feel like individuals), water sources that can be found, monitored, and managed (making resource strategy deeper), and items that carry history and significance (making inventory feel like a collection of stories). This is the wave that makes ASHFALL's world feel accumulated and lived-in — where every component has a condition, every creature has a sighting log, every survivor has a schedule, every water source has a contamination level, and every significant item has a story to tell.

## Cumulative Wave Themes (Waves 14–25)

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
| **25** | **Shelter maintenance, bestiary, survivor routines, water sources, item lore** | **186–190** |

**Total: 60 plans across 12 waves (131–190), plus 12 wave summaries.**
