# Wave 24 — Summary (Plans 181–185)

## Wave Overview

Five non-duplicative, implementation-ready plans covering game configuration, social realism, generational depth, player inclusion, and cognitive realism. This wave focuses on **polish and realism** — the systems that make ASHFALL feel like a complete, accessible, living game rather than a collection of mechanics.

| Plan | New Capability | Why It Is Not Duplicate | Risk | Key Systems |
| ---- | -------------- | ----------------------- | ---- | ----------- |
| 181 — Difficulty Settings System | Preset difficulties (Easy/Normal/Hard/Nightmare) and 9 customizable sliders (radiation rate, raid frequency, resource scarcity, etc.). | Plan 175 (meta-progression) adds NG+ difficulty *modifiers* for meta currency but not a general difficulty system. Plan 34 mentions "accessibility parity" but doesn't implement it. Verified: ZERO matches for `DifficultySetting`, `DifficultyLevel`, `GameDifficulty` in Core. | LOW | RadiationSystem, NeedsSystem, MarketSystem, ExpeditionSystem, WeatherSystem, CraftingSystem |
| 182 — Relationship Decay & Drift | Survivor bonds weaken over time without interaction. Friends drift apart, trust erodes, relationships require maintenance. | Plan 147 (per-NPC memory) adds memory but not relationship decay. Plan 150 (romance/family) adds relationship formation but not decay. Plan 179 (psychology) adds psychological profiles but not social decay. Verified: ZERO matches for `RelationshipDecay`, `FriendshipDecay`, `TrustDecay`, `bond_decay` in Core and plans. | LOW | SurvivorRelationsSystem, DutyRosterSystem, ShelterAssignmentSystem, IdeologicalFrictionSystem, NeedsSystem, MentalHealthCrisisSystem |
| 183 — Child Development Stages | Children grow through 5 developmental phases (infant → toddler → child → adolescent → young adult) with age-appropriate capabilities, needs, learning, and events. | Plan 154 (education) adds schooling but not developmental stages. Plan 176 (aging) adds aging for adults but not child development. Plan 150 mentions "child development" but doesn't plan it. Verified: ZERO matches for `ChildDevelopment`, `ChildGrowth`, `Adolescent`, `Teenager` in Core. `CohortSystem.TryMaturation()` is boolean — instant maturation. | LOW | CohortSystem, SurvivorLifecycle, NeedsSystem, SkillProgressionSystem, EducationSystem, AgingSystem |
| 184 — Accessibility Options System | Colorblind modes, font scaling, high contrast, screen reader support, input remapping, reduced motion, audio descriptions, cognitive load reduction. | Plan 34 mentions "accessibility parity" as goal. Plan 25 mentions accessibility audit. No plan implements accessibility options. Verified: ZERO matches for `Accessibility`, `Colorblind`, `ScreenReader`, `HighContrast` as systems in Core/src. Only plan references found. | MEDIUM | All UI panels, audio system, input system, theme system, render system |
| 185 — Memory & Knowledge Decay | Unused skills fade, knowledge is forgotten, memories blur. Practice, review, and reinforcement required to maintain capabilities. | Plan 180 (certifications) protects certified skills from decay but doesn't add decay for uncertified knowledge. Plan 147 (memory) adds memory but not memory decay. Skill dormancy exists (14 unused days) but no broader decay. Verified: only `BunkerSkillDecayStopped` callback in `SkillProgressionSystem.cs`. No memory/knowledge decay system. | LOW | SkillProgressionSystem, PhantomMemoryEngine, JournalSystem, SurvivorRelationsSystem, AgingSystem, SkillCertificationSystem |

## Strongest Plan to Implement First

**Plan 181 — Difficulty Settings System.** It has the lowest risk, clearest scope, and broadest player impact. Difficulty settings are expected in any modern game and make ASHFALL accessible to players of all skill levels. It's also the simplest to implement (multipliers applied to existing systems) with immediate player value.

## Dependencies Between the 5 Plans

- **Plan 181 (Difficulty) is standalone** — applies modifiers to existing systems.
- **Plan 182 (Relationship Decay) is standalone** — extends existing relationship system.
- **Plan 183 (Child Development) integrates with 176** — aging system provides age framework for children.
- **Plan 184 (Accessibility) is standalone** — settings layer over existing UI/audio/input.
- **Plan 185 (Memory Decay) integrates with 180** — certification system protects certified skills from decay.

## Recommended Implementation Order

1. **Plan 181** — Difficulty Settings System (player configuration, lowest risk, broadest impact)
2. **Plan 184** — Accessibility Options System (player inclusion, medium risk, ethical obligation)
3. **Plan 182** — Relationship Decay & Drift (social realism, low risk, extends existing system)
4. **Plan 185** — Memory & Knowledge Decay (cognitive realism, low risk, extends skill system)
5. **Plan 183** — Child Development Stages (generational depth, low risk, extends cohort system)

## Rejected Candidates (Considered but Not Selected)

- **Faction AI Autonomy** — Rejected in Waves 22, 23, and 24. `FactionWarSystem.cs` (203 lines) + content catalog (429 lines) exist. Functional but simplistic. Improvement, not new system.
- **Weather Events** — Plan 135 (Weather Deep Gameplay Cascade) covers weather events extensively. Not a gap.
- **Save Slot Management UI** — `SaveLoadPanel` already exists with full functionality (slot selection, save, load, delete, import). Not a gap.
- **Photo Mode** — Too thin for a full plan. Could be a feature within another system.
- **Bestiary/Codex** — `JournalCodex` + `_codexViewer` already exist. Partial coverage.

## Why This Wave Materially Expands ASHFALL

These five plans transform ASHFALL from a game with mechanics into a game with polish: difficulty settings that welcome players of all skill levels (not just one fixed challenge), relationships that feel real because they require maintenance (not just permanent bonds), children who grow through meaningful developmental stages (not just instant maturation), accessibility options that ensure everyone can play (not just able-bodied players), and memory decay that makes skills and knowledge feel earned and fragile (not just permanent stats). This is the wave that makes ASHFALL feel like a complete, professional, inclusive, realistic game — the difference between a collection of systems and a living, breathing world.

## Cumulative Wave Themes (Waves 14–24)

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
| **24** | **Difficulty, relationship decay, child development, accessibility, memory decay** | **181–185** |

**Total: 55 plans across 11 waves (131–185), plus 11 wave summaries.**
