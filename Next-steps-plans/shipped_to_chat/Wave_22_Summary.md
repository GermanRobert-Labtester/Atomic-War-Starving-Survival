# Wave 22 — Summary (Plans 171–175)

## Wave Overview

Five non-duplicative, implementation-ready plans covering procedural content, biological evolution, information culture, biographical depth, and cross-campaign progression. Each plan addresses a verified gap — areas with zero existing systems or only a single data field where a full system should exist.

| Plan | New Capability | Why It Is Not Duplicate | Risk | Key Systems |
| ---- | -------------- | ----------------------- | ---- | ----------- |
| 171 — Dynamic Quest Generation | Quests procedurally generated from game state, survivor traits, faction conditions. Different quests every campaign. | Plan 133 (expedition consequences) adds discovery consequences but not quest generation. Plan 144 (autonomy) adds behavior but not quest creation. Verified: ZERO matches for `DynamicQuest`, `ProceduralQuest`, `QuestGenerat` in Core. All quests are static data. | MEDIUM | QuestSystem, CampaignConsequenceLedger, FactionBranchCoordinator, LocationEvolution, SurvivorRelations, ExpeditionSystem |
| 172 — Radiation Mutation System | Long-term radiation causes permanent genetic changes — beneficial, harmful, neutral, mixed. Risk/reward around radiation zones. | Plan 146 (radiation bridge) mentions mutation only as a follow-on opportunity. No mutation system exists. Verified: all `mutation` matches in Core are code-level "state mutation" — no biological mutation system. `mutation_` prefix recognized by CatalogIntegrityValidator but no catalog exists. | MEDIUM | RadiationSystem, SurvivorLifecycle, DiseaseSystem, NeedsSystem, FactionBranchCoordinator, ResearchSystem |
| 173 — Radio Station & Content Creation | Shelter produces radio programs (news, music, propaganda, entertainment) to influence factions, attract refugees, shape wasteland culture. | Plan 157 (communications) covers radio *infrastructure* (towers, frequencies, signal range). Plan 168 (propaganda) covers propaganda messages but not full programming. Verified: ZERO matches for `RadioStation`, `BroadcastCreate`, `RadioProgram`, `PirateRadio` in Core. Radio is receive-only for the player. | MEDIUM | VerdictRadioSystem, PropagandaSystem, radio infrastructure (Plan 157), FactionBranchCoordinator, NeedsSystem, SkillProgressionSystem |
| 174 — Procedural Survivor Backstories | Each survivor arrives with mechanically-relevant personal history — occupation, experiences, traumas, secrets — that shapes behavior and capabilities. | Plan 144 (autonomy) adds behavior but not biographical depth. Plan 147 (per-NPC memory) adds memory of events but not pre-existing history. Verified: only 1 match — `backstory` string field in `YearOfAshCatalogLoader.cs:66` — narrative flavor only, zero mechanical effects. | LOW | SurvivorLifecycle, SurvivorRelationsSystem, SkillProgressionSystem, MoralChoiceSystem, QuestSystem, JournalSystem |
| 175 — Meta-Progression & New Game+ | Persistent unlocks, meta currency, prestige levels, difficulty modifiers, and campaign history carry across playthroughs. | Plan 140 (legacy) covers in-world cross-generational inheritance (traits passed within fiction). Plan 149 (achievements) tracks milestones but grants no persistent rewards. Verified: ZERO matches for `NewGamePlus`, `MetaProgress`, `CampaignInherit`, `prestige` in Core. Each campaign starts from zero. | MEDIUM | SaveStoreHub, CampaignCalendar, AchievementSystem (Plan 149), ending systems (Plan 145), ShelterIdentitySystem (Plan 166) |

## Strongest Plan to Implement First

**Plan 174 — Procedural Survivor Backstories.** It has the lowest risk, clearest scope, and immediate player value (every survivor becomes a unique person with a story). It integrates naturally with existing survivor systems (skills, relations, quests) without requiring major architectural changes. It also enhances every other system that touches survivors — recruitment becomes exciting, relationships gain depth, and quests become personal.

## Dependencies Between the 5 Plans

- **Plan 171 (Dynamic Quests) is standalone** but benefits from backstories (174) providing quest hooks.
- **Plan 172 (Mutations) is standalone** but mutations could appear in backstories (174) as pre-existing conditions.
- **Plan 173 (Radio Station) is standalone** but radio programs could cover mutation events (172) or quest stories (171).
- **Plan 174 (Backstories) is foundational** — provides quest hooks for dynamic quests (171), content for radio programs (173), and personal stakes for mutation stories (172).
- **Plan 175 (Meta-Progression) is standalone** but meta unlockables could include backstory-related cosmetics (174) or radio station themes (173).

## Recommended Implementation Order

1. **Plan 174** — Procedural Survivor Backstories (biographical depth, lowest risk, foundational)
2. **Plan 171** — Dynamic Quest Generation (procedural content, medium risk, benefits from backstories)
3. **Plan 172** — Radiation Mutation System (biological evolution, medium risk, standalone)
4. **Plan 173** — Radio Station & Content Creation (information culture, medium risk, standalone)
5. **Plan 175** — Meta-Progression & New Game+ (cross-campaign progression, medium risk, standalone)

## Rejected Candidates (Considered but Not Selected)

- **Faction AI Autonomy** — `FactionWarSystem` already exists in `YearOfAsh/` namespace with standing records, branch coordination. Factions already have autonomous decision frameworks.
- **Shelter Architecture/Room Customization** — Plan 156 (shelter expansion) covers physical renovation. "Architecture styles" mentioned only as follow-on. Too much overlap with existing plan.
- **Survivor Specialization/Certification** — Plan 154 (education) covers skill specialization. Certification is just a formal wrapper around existing skill progression.
- **New Game+ (standalone)** — Plan 140 already covers cross-campaign inheritance. Plan 175 absorbs the broader meta-progression concept while acknowledging Plan 140's inheritance mechanics.

## Why This Wave Materially Expands ASHFALL

These five plans transform ASHFALL from a game with static content into one with emergent depth: quests that are different every campaign (not just replaying the same story), survivors who genetically evolve from radiation exposure (not just taking damage), a radio station that gives the shelter a voice in the wasteland (not just receiving broadcasts), survivors who arrive with rich personal histories that shape their gameplay (not just blank stat blocks), and a meta-progression system that rewards long-term play (not just resetting to zero each campaign). This is the wave that makes ASHFALL infinitely replayable — every campaign tells a different story with different survivors, different quests, different mutations, and different outcomes, all building on a persistent foundation of player achievement.

## Cumulative Wave Themes (Waves 14–22)

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
| **22** | **Dynamic quests, mutations, radio, backstories, meta-progression** | **171–175** |

**Total: 45 plans across 9 waves (131–175), plus 9 wave summaries.**
