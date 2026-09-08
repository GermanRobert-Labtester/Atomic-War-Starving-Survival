# Wave 29 — Summary (Plans 206–210)

## Wave Overview

Five non-duplicative, implementation-ready plans covering mortality and inheritance, maritime exploration, political continuity, internal security, and personal identity. This wave focuses on **individual identity and continuity** — the systems that make each survivor a distinct person with possessions, a leader with succession, a shelter with security, a coast with mysteries, and a death that means something.

| Plan | New Capability | Why It Is Not Duplicate | Risk | Key Systems |
| ---- | -------------- | ----------------------- | ---- | ----------- |
| 206 — Survivor Death, Legacy & Inheritance | Cause-of-death tracking, last wills/testaments, inheritance of possessions, death records, legacy effects on survivors. | `MemorialSystem` (262 lines) handles burial outcomes. `SurvivorFateSystem` tracks alive/dead. But NO cause-of-death, no wills, no inheritance of possessions. Plan 140 covers cross-campaign meta-inheritance, not in-campaign inheritance. Verified: ZERO matches for `DeathSystem`, `InheritanceSystem`, `WillSystem`, `CauseOfDeath` in Core. | LOW | MemorialSystem, SurvivorFateSystem, Inventory, SurvivorRelationsSystem, DeathLegacy |
| 207 — Shelter Reputation & External Perception | Shelter has global reputation (feared/respected/generous/cruel/etc.), notoriety, per-faction perception, reputation effects on visitors/trade/attacks/diplomacy. | `FactionStanceEngine` (172 lines) tracks per-faction trust. Plans 138/166/168 mention "shelter reputation" as concept but don't implement. NO dedicated reputation system, no fame/infamy, no external perception. Verified: ZERO matches for `ShelterReputation`, `ReputationSystem`, `ExternalPerception` in Core. **REPLACED**: original Plan 207 (Maritime Exploration) was invalidated by recon — `MaritimeDiveSystem` (619 lines) + `District8DeepCoastSystem` (741 lines) = complete maritime expansion system. | LOW | FactionStanceEngine, AirlockSecuritySystem, HoldfastTradeSession, MoralChoiceSystem, ExpeditionSystem |
| 208 — Leadership Succession & Challenge | Succession planning, leadership challenges, elections, deputy system, legitimacy tracking, term limits. | `LeadershipSystem` (288 lines) handles leader designation + stress. But NO succession, no challenges, no elections, no deputy, no legitimacy, no transfer on death. Plan 159 (governance) mentions "election" as quest hook but doesn't implement succession mechanics. Verified: ZERO matches for `SuccessionSystem`, `LeadershipChallenge`, `ElectionSystem` in Core. | LOW | LeadershipSystem, SurvivorRelationsSystem, MoralChoiceSystem, SkillProgressionSystem |
| 209 — Shelter Security & Access Control | Room-level security zones, survivor clearance ratings, restricted areas, door locks, alarms, security breaches, lockdown protocol. | `AirlockSecuritySystem` (227 lines) handles external visitors. `PowerGridSystem` (489 lines) manages room power. But NO internal security — no room access control, no clearances, no restricted areas, no alarms, no breaches. Verified: ZERO matches for `ShelterSecurity`, `AccessControl`, `SecurityClearance`, `RoomPermission` in Core. | LOW | AirlockSecuritySystem, PowerGridSystem, DutyRosterSystem, LeadershipSystem, ShelterFireHazardSystem |
| 210 — Survivor Personal Belongings & Effects | Personal possessions per survivor (keepsakes, clothing, tools, mementos), sentimental value, gift-giving, inheritance, favorite items. | `Inventory.cs` manages shared shelter inventory. `EquipmentConditionSystem` (189 lines) tracks equipment. But NO personal possessions — no keepsakes, no sentimental items, no personal ownership, no gift-giving. Verified: ZERO matches for `PersonalBelongings`, `Keepsake`, `SentimentalItem`, `PersonalEffects` in Core. | LOW | Inventory, EquipmentConditionSystem, SurvivorRelationsSystem, MemorialSystem, DeathLegacySystem |

## Strongest Plan to Implement First

**Plan 206 — Survivor Death, Legacy & Inheritance.** It makes death meaningful (possessions don't vanish), adds emotional weight (survivors name heirs), and integrates naturally with existing memorial/inventory/relations systems. Death is the one event every survivor faces — making it leave a trace transforms how players value each life.

## Dependencies Between the 5 Plans

- **Plan 206 (Death/Inheritance) is foundational** — Plan 210 (belongings) feeds inheritance on death.
- **Plan 207 (Reputation) is standalone** — adds external perception layer.
- **Plan 208 (Leadership Succession) is standalone** — extends existing leadership.
- **Plan 209 (Security) is standalone** — adds internal security layer.
- **Plan 210 (Belongings) integrates with Plan 206** — belongings inherited on death.

## Recommended Implementation Order

1. **Plan 206** — Death, Legacy & Inheritance (meaningful mortality, broadest emotional impact)
2. **Plan 210** — Personal Belongings & Effects (individual identity, feeds inheritance)
3. **Plan 207** — Shelter Reputation & External Perception (shelter identity, affects all external interactions)
4. **Plan 208** — Leadership Succession & Challenge (political continuity, extends existing system)
5. **Plan 209** — Shelter Security & Access Control (internal security, completes shelter management)

## Rejected Candidates (Considered but Not Selected)

- **Power/Energy/Fuel Logistics** — `PowerGridSystem.cs` (489 lines) already exists with rooms, priorities, fuel, battery, brownout, save/load. Not a gap.
- **Air Quality/Ventilation** — `VentilationSystem.cs` (270 lines) + `StartingLevelSystem` air filter + `YearOfAshRadonSystem` + `RespiratoryDegenerationSystem`. Comprehensive. Not a gap.
- **Shelter Fire Safety** — `ShelterFireHazardSystem.cs` (472 lines) handles fire detection, spread, suppression, smoke/CO. Not a gap.
- **Chemical Dependency Treatment** — `ChemicalDependencySystem.cs` (532 lines) handles substance-specific dependency, tolerance, withdrawal, stress relapse. Not a gap.
- **Working Animals & Companion Training** — Plan 151 already covers taming, training, guard dogs, animal companions. Not a planning gap.
- **Structural Integrity & Maintenance** — Plan 186 already covers shelter maintenance, degradation, repair. Not a planning gap.
- **Decontamination Operations** — `DecontaminationSystem.cs` (244 lines) handles radiation decontamination. Not a gap.
- **Child Development & Education** — Plan 183 already covers child aging, education, skill development. Not a planning gap.
- **Expedition Vehicle Customization** — Plan 152 already covers vehicle customization, mobile base. Not a planning gap.

## Post-Recon Corrections

Both recon agents validated findings:

- **Plan 206 (Death/Inheritance)**: `SurvivorFateSystem` (438 lines) already handles 8-step death cascade with 8 cause types, memorial entry, journal entry, grief morale. The gap is ONLY inheritance/wills/legacy — not death records. Plan should focus on inheritance mechanics, not death tracking.
- **Plan 207 (Reputation, REPLACED)**: Original Plan 207 (Maritime Exploration) was invalidated — `MaritimeDiveSystem` (619 lines) + `District8DeepCoastSystem` (741 lines) = complete maritime expansion system (Expansion 09 "The Black Flotilla"). Replaced with Shelter Reputation & External Perception.
- **Plan 208 (Leadership)**: confirmed `LeadershipSystem` (288 lines) handles designation + stress only. No succession, no challenges, no elections. Plan 159 mentions "election" as quest hook but doesn't implement.
- **Plan 209 (Security)**: confirmed ZERO internal security systems. `AirlockSecuritySystem` handles external visitors only. No room-level access control exists.
- **Plan 210 (Belongings)**: confirmed ZERO personal possession systems. All items in shared inventory. No keepsakes, no sentimental items, no personal ownership.

## Why This Wave Materially Expands ASHFALL

These five plans transform ASHFALL's survivors from functional game pieces into individuals with identity, continuity, and consequence. Death leaves a legacy (possessions inherited, wills executed). The shelter is known by the outside world (reputation affects visitors, trade, attacks, diplomacy). Leadership has continuity (succession planned, challenges managed, legitimacy tracked). The shelter has internal security (who goes where, restricted areas, breach response). Each survivor has personal possessions (keepsakes, gifts, favorites that matter). Together, these plans make every survivor feel like a person — with stuff they care about, a leader they follow (or challenge), rooms they can't enter, a reputation that precedes them, and a death that leaves a mark.

## Cumulative Wave Themes (Waves 16–29)

| Wave | Theme | Plans |
| ---- | ----- | ----- |
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
| 27 | Food types, diplomacy, health records, seasonal migration, personal quests | 196–200 |
| 28 | Sanitation, interpersonal conflict, intelligence, recruitment, noise discipline | 201–205 |
| **29** | **Death/inheritance, shelter reputation, leadership succession, security, personal belongings** | **206–210** |

**Total: 80 plans across 16 waves (131–210), plus 16 wave summaries.**

## Milestone Note

Wave 29 reaches Plan 210 — 80 plans in 16 waves since Plan 131. The planning has evolved from basic system gaps (early waves) through content/narrative depth (mid waves) to the current focus on individual identity, continuity, and consequence. Each wave builds on the last, creating an increasingly detailed vision of ASHFALL as a game where every survivor matters, every death leaves a trace, and every choice echoes forward.
