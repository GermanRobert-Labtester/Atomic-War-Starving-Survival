# Wave 31 — Summary (Plans 216–220)

## Wave Overview

Five non-duplicative, implementation-ready plans covering physical development, familial continuity, historical preservation, creative documentation, and environmental character. This wave focuses on **depth and permanence** — the systems that give survivors physical agency, families generational roots, the shelter historical memory, creative expression, and atmospheric character.

| Plan | New Capability | Why It Is Not Duplicate | Risk | Key Systems |
| ---- | -------------- | ----------------------- | ---- | ----------- |
| 216 — Survivor Exercise & Physical Training | Structured physical fitness activities (cardio, strength, flexibility, endurance, combat drills) that improve physical attributes, maintain fitness, and counteract deconditioning. | No exercise/fitness system exists. Survivor physical capability is static (traits + needs). No training regimens, no fitness tracking, no deconditioning. Verified: ZERO matches for `ExerciseSystem`, `FitnessSystem`, `PhysicalTraining` in Core. | LOW | NeedsSystem, SkillProgressionSystem, TraitSystem, ExpeditionSystem, CombatSystem |
| 217 — Survivor Genealogy & Family Tree **(REWRITTEN)** | Extends existing `GenerationalLineageExtension` (119 lines, Core, unwired) + `GenerationalSuccessionEngine` (150 lines, Core.Legacy, wired to ExpansionHostSession) into full genealogy: wires extension to host, adds sibling/spouse detection, family units, family naming, family events, multi-gen traces, family tree UI, kinship affinity bonuses. | Engine wired to host but extension NEVER instantiated in `src/`. No family units, no naming, no events, no UI, no sibling detection, no spouse tracking. Plans 150/154/183/206 touch family topics but none wire or extend the existing lineage extension. | LOW-MEDIUM | GenerationalLineageExtension, GenerationalSuccessionEngine, ExpansionHostSession, CohortSystem, SurvivorRelationsSystem, SurvivorFateSystem |
| 218 — Shelter Museum & Historical Archive | Collects, preserves, and displays significant artifacts, documents, and records from shelter history in curated exhibitions. | `MemorialSystem` (262 lines) handles burial remembrance. Plan 162 covers archive recording. Plan 178 covers art exhibitions. But NO museum system for displaying historical artifacts. Verified: ZERO matches for `MuseumSystem`, `HistoricalArchive`, `ArtifactDisplay` in Core. | LOW | MemorialSystem, Inventory, SurvivorFateSystem, PersonalBelongingsSystem |
| 219 — Survivor Photography & Documentation | Survivors capture moments through photography, sketching, and written documentation — creating visual/textual record of shelter life. | No photography/documentation system exists. Plan 162 records history automatically. Plan 178 covers art creation. But NO survivor-created documentation. Verified: ZERO matches for `PhotographySystem`, `DocumentationSystem`, `PhotoSystem` in Core. | LOW | JournalSystem, MemorialSystem, Inventory, PersonalBelongingsSystem |
| 220 — Shelter Atmosphere & Ambiance | Tracks shelter's overall mood, personality, and environmental character as composite of conditions (lighting, sound, air, temperature, cleanliness, activity, social energy, decoration). | Individual shelter systems track conditions (thermal, ventilation, power, noise) but NO unified atmosphere system, no composite mood, no ambiance effects, no shelter personality. Verified: ZERO matches for `AtmosphereSystem`, `AmbianceSystem`, `ShelterMood` in Core. | LOW | ShelterThermalSystem, VentilationSystem, PowerGridSystem, ShelterNoiseSystem, SanitationSystem |

## Strongest Plan to Implement First

**Plan 220 — Shelter Atmosphere & Ambiance.** It integrates with the most existing systems (thermal, ventilation, power, noise, sanitation, duty roster, relations), creates immediate visible effects on survivors, and makes the shelter feel like a living environment with character. Atmosphere is the meta-layer that ties all shelter systems together into a unified "feel."

## Dependencies Between the 5 Plans

- **Plan 216 (Exercise) is standalone** — adds physical development.
- **Plan 217 (Genealogy) extends existing Core systems** — wires `GenerationalLineageExtension` to host, extends data model, adds family units/naming/events/UI.
- **Plan 218 (Museum) integrates with Plan 219** — documentation displayed in museum.
- **Plan 219 (Documentation) integrates with Plan 218** — documentation preserved in museum.
- **Plan 220 (Atmosphere) is standalone** — adds environmental character, integrates with many existing systems.

## Recommended Implementation Order

1. **Plan 220** — Shelter Atmosphere & Ambiance (meta-layer, broadest integration, immediate effects)
2. **Plan 216** — Survivor Exercise & Physical Training (physical development, clear gameplay impact)
3. **Plan 217** — Survivor Genealogy & Family Tree (familial continuity, emotional depth)
4. **Plan 218** — Shelter Museum & Historical Archive (historical preservation, cultural depth)
5. **Plan 219** — Survivor Photography & Documentation (creative expression, feeds museum)

## Rejected Candidates (Considered but Not Selected)

- **Wasteland Cartography & Mapping** — `WastelandMapSystem.cs` (408 lines) already exists with map nodes, routes, fog-of-war. Not a gap.
- **Weather Forecasting & Prediction** — `WeatherStationSystem.cs` + `WeatherSystem.PeekForecast()` already exist. Not a gap.
- **Survivor Dreams & Subconscious** — Plan 177 (Dream & Sleep Event System) covers dreams, nightmares, sleep events. Not a planning gap.
- **Shelter Engineering & Infrastructure Maintenance** — Plan 186 (Shelter Maintenance & Degradation) covers maintenance. Not a planning gap.
- **Survivor Friendship & Social Network** — Plan 182 (Relationship Decay & Drift) covers relationship maintenance. `SurvivorRelationsSystem` (191 lines) tracks affinity. Too overlapping.

## Post-Recon Corrections

Both recon agents validated findings:

- **Plan 216 (Exercise)**: confirmed ZERO exercise/fitness systems. No physical training, no fitness tracking, no deconditioning.
- **Plan 217 (Genealogy) — REWRITTEN**: Recon found `GenerationalLineageExtension.cs` (119 lines) already exists in Core with `EstablishLineage`/`PerformSuccession`/`GetLineage`/`GetParent` + `LineageRecord` DTO + `CaptureState/RestoreState`, and `GenerationalSuccessionEngine.cs` (150 lines) in Core.Legacy wired to `ExpansionHostSession.cs:33,61` + `CenturySeedPanel.cs`. BUT extension is NEVER instantiated in `src/` (zero host matches). Original plan would have duplicated existing Core lineage tracking. Plan rewritten to: (1) wire extension to host, (2) extend data model with family units/events/naming/sibling-spouse detection, (3) create `GenealogyBridge` adapter, (4) add family tree UI, (5) add kinship affinity bonuses. Risk upgraded LOW → LOW-MEDIUM (wiring risk).
- **Plan 218 (Museum)**: confirmed ZERO museum/archive display systems. Plan 162 records history, Plan 178 covers art exhibitions, but no historical artifact display.
- **Plan 219 (Documentation)**: confirmed ZERO photography/documentation systems. Only string references in narrative data.
- **Plan 220 (Atmosphere)**: confirmed ZERO atmosphere/ambiance systems. Individual shelter systems track conditions but no unified atmosphere.

## Why This Wave Materially Expands ASHFALL

These five plans transform ASHFALL from a game with functional systems into one where survivors have physical agency, families have generational roots, the shelter has historical memory, creative expression is possible, and the environment has character. Exercise makes physical capability dynamic rather than static. Genealogy (rewritten to extend existing Core lineage systems) makes families persistent across time while wiring unwired infrastructure. Museum makes history visible and meaningful. Documentation makes creative expression possible. Atmosphere makes the shelter feel alive with personality. Together, these plans add depth, permanence, and character to every aspect of shelter life.

## Cumulative Wave Themes (Waves 18–31)

| Wave | Theme | Plans |
| ---- | ----- | ----- |
| 18 | Animals, vehicles, espionage, education, black market | 151–155 |
| 19 | Shelter, communications, disasters, governance, colonies | 156–160 |
| 20 | Hobbies, archive, cartography, nuclear winter, modding | 161–165 |
| 21 | Identity, tunnels, propaganda, audio, celebrations | 166–170 |
| 22 | Dynamic quests, mutations, radio, backstories, meta-progression | 171–175 |
| 23 | Aging, dreams, art, psychology, certifications | 176–180 |
| 24 | Difficulty, relationship decay, child development, accessibility, memory decay | 181–185 |
| 25 | Shelter maintenance, bestiary, survivor routines, water sources, item lore | 186–190 |
| 26 | Item identification, trade routes, chronic conditions, emergency alerts, survivor roles | 191–195 |
| 27 | Food types, diplomacy, health records, seasonal migration, personal quests | 196–200 |
| 28 | Sanitation, interpersonal conflict, intelligence, recruitment, noise discipline | 201–205 |
| 29 | Death/inheritance, shelter reputation, leadership succession, security, personal belongings | 206–210 |
| 30 | Communication, time capsules, barter, visitor integration, resource rationing | 211–215 |
| **31** | **Exercise, genealogy, museum, documentation, atmosphere** | **216–220** |

**Total: 90 plans across 18 waves (131–220), plus 18 wave summaries.**

## Milestone Note

Wave 31 reaches Plan 220 — 90 plans in 18 waves since Plan 131. The planning has now covered: basic systems, content/narrative, integration/depth, societal complexity, individual identity, community operations, and depth/permanence. Each wave builds on the last, creating an increasingly detailed vision of ASHFALL as a game where survivors are physically capable, families persist, history is preserved, creativity flourishes, and the shelter has character.
