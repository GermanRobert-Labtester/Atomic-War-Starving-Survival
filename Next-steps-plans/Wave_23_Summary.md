# Wave 23 — Summary (Plans 176–180)

## Wave Overview

Five non-duplicative, implementation-ready plans covering survivor aging, subconscious processing, cultural creation, psychological integration, and skill formalization. This wave focuses on **survivor depth** — making each survivor a more complex, evolving individual with a life arc, inner life, creative potential, psychological profile, and formal skill progression.

| Plan | New Capability | Why It Is Not Duplicate | Risk | Key Systems |
| ---- | -------------- | ----------------------- | ---- | ----------- |
| 176 — Aging & Elderly Survivor System | Survivors age through life stages (young adult → elderly), with physical decline, wisdom growth, retirement, mentorship, and age-related death. | Plan 140 (legacy) covers cross-campaign inheritance but not in-campaign aging. Plan 150 (romance/family) covers family but not aging parents. Verified: ZERO matches for `AgingSystem`, `Elderly`, `OldAge` in Core. Only `CaregivingSystem` mentions "elderly care" in description. `GenerationalSuccessionEngine` has aging skeleton but `AdvanceTime()` never called. | LOW | SurvivorLifecycle, CaregivingSystem, SkillProgressionSystem, GenerationalSuccessionEngine, ShelterArchiveSystem, SeasonalEventSystem |
| 177 — Dream & Sleep Event System | Survivors experience dreams, nightmares, and sleep events that process trauma, provide insights, and reveal psychological state. | Plan 147 (per-NPC memory) adds memory but not dream processing. Plan 148 (friction→events) adds friction but not sleep events. Verified: only 2 matches — `ComfortNightmare` quest ID and comfort category list. No dream system exists. `GuiltInsomniaSystem` handles sleep disruption but not dream content. | LOW | GuiltInsomniaSystem, CombatTraumaSystem, MentalHealthCrisisSystem, SomaticFlashbackSystem, NeedsSystem, MoralChoiceSystem |
| 178 — Art & Culture Creation System | Survivors create art, music, literature, and cultural artifacts that boost morale, express personality, and define shelter cultural identity. | Plan 161 (hobbies) adds personal pastimes but not cultural creation. Plan 162 (archive) records history but doesn't create art. Verified: ZERO matches for `ArtSystem`, `CultureSystem`, `CreativeWork` in Core. Only `ApicultureSystem` (beekeeping) found. Survivors consume culture but never create it. | LOW | SkillProgressionSystem, NeedsSystem, ShelterDecorSystem, CraftingSystem, FactionBranchCoordinator, SeasonalEventSystem |
| 179 — Unified Psychology & Phobia System | Integrates 6 existing trauma systems into coherent psychological profiles with phobia development, personality evolution, coping mechanisms, and therapy. | Plan 147 (memory) adds memory but not psychological integration. Plan 148 (friction) adds friction but not psychological arcs. Plan 177 (dreams) adds dream processing but not unified psychology. Verified: ZERO matches for `PhobiaSystem`, `PsychologicalProfile`, `PersonalityChange` in Core. 6 trauma systems exist but operate in isolation. | MEDIUM | All 6 trauma systems (CombatTrauma, SomaticFlashback, GuiltInsomnia, MentalHealthCrisis, PsychologicalContamination, PhantomMemory), NeedsSystem, SurvivorRelationsSystem, DreamSystem |
| 180 — Skill Certification & Tier System | Formal skill tiers (novice → master), certifications (certified medic, master engineer), specializations (combat medic, field engineer), and certification exams. | Plan 136 (trapping→food) mentions "cooking specialization" as follow-on only. Plan 154 (education) covers teaching but not certification. Plan 131 (rumors) mentions "information broker specialization" as follow-on only. Verified: ZERO matches for `SkillTier`, `SkillRank`, `SkillCertification` in Core. Skills are bare floats with no meaningful milestones. | LOW | SkillProgressionSystem, CraftingSystem, ExpeditionSystem, FactionBranchCoordinator, DutyRosterSystem, EducationSystem |

## Strongest Plan to Implement First

**Plan 180 — Skill Certification & Tier System.** It has the lowest risk, clearest scope, and immediate player value (visible skill progression with meaningful milestones). It integrates naturally with the existing `SkillProgressionSystem` by adding tiers on top of existing float values. It also enhances every system that uses skills (crafting, expeditions, combat, education) by providing clear capability unlocks.

## Dependencies Between the 5 Plans

- **Plan 176 (Aging) is standalone** but elderly survivors benefit from certification (180) as mentors.
- **Plan 177 (Dreams) is standalone** but dreams can process phobias from psychology system (179).
- **Plan 178 (Art) is standalone** but art creation can be a coping mechanism in psychology (179).
- **Plan 179 (Psychology) integrates with 177** — dreams process trauma tracked by psychology. Also integrates with 178 — art as coping mechanism.
- **Plan 180 (Certification) is standalone** but aging survivors (176) can earn certifications over lifetime.

## Recommended Implementation Order

1. **Plan 180** — Skill Certification & Tier System (visible progression, lowest risk, enhances all skill-using systems)
2. **Plan 176** — Aging & Elderly Survivor System (life arc, low risk, adds temporal depth)
3. **Plan 177** — Dream & Sleep Event System (subconscious depth, low risk, standalone)
4. **Plan 178** — Art & Culture Creation System (creative expression, low risk, standalone)
5. **Plan 179** — Unified Psychology & Phobia System (psychological integration, medium risk, connects 6 systems)

## Rejected Candidates (Considered but Not Selected)

- **Shelter Construction/Room Building** — Plan 156 (Shelter Expansion Physical Renovation) already covers "new room construction" and shelter renovation. Too much overlap.
- **Burial/Corpse Handling** — `MemorialSystem.cs` already has `Burial` as a memorial outcome type. Partially covered.
- **Cooking Depth** — Plan 136 (Wildlife Trapping→Food Pipeline→Cooking) already covers cooking as part of the food pipeline.
- **Faction AI Autonomy** — `FactionWarSystem.cs` (203 lines) + content catalog (429 lines) already exist. Functional but simplistic — improvement, not new system.
- **Memory/Skill Decay** — `SkillProgressionSystem.cs` already has `BunkerSkillDecayStopped` — some decay exists.

## Why This Wave Materially Expands ASHFALL

These five plans transform ASHFALL's survivors from stat blocks into complex individuals: survivors who age and grow wise (not just work until they die), who dream and process trauma through their sleeping minds (not just reset fatigue), who create art and culture (not just consume), who develop psychological profiles with phobias and coping mechanisms (not just accumulate trauma in isolated systems), and who earn formal certifications that unlock meaningful capabilities (not just watch invisible numbers increase). This is the wave that makes every survivor feel like a real person with a full life arc — from young novice to elderly master, with dreams, art, psychology, and recognized expertise along the way.

## Cumulative Wave Themes (Waves 14–23)

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
| **23** | **Aging, dreams, art, psychology, certifications** | **176–180** |

**Total: 50 plans across 10 waves (131–180), plus 10 wave summaries.**
