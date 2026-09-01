# Plan 180 — Skill Certification & Tier System

## Goal

Create a skill certification and tier system where survivors earn formal qualifications that unlock new capabilities, specializations, and social roles. Currently `SkillProgressionSystem.cs` tracks skills as bare floats with no tiers, no certifications, no formal progression framework. Skills are implicit numeric values used by specific systems (trapping, reverse engineering) but there is no unified skill progression with meaningful milestones. This plan adds structured skill development and formal recognition.

## Why

**Repository evidence:** Grep for `SkillTier`, `SkillRank`, `SkillCertification`, `SkillTree` in Core returns ZERO matches. `SkillProgressionSystem.cs` tracks skill levels as floats. `BunkerSkillDecayStopped` callback exists but no tier system. All 56 matches for "certification/specialization/license" in Core are SPDX license headers — no skill certification. Skills are bare numbers with no meaningful progression milestones.

**What is missing:** No skill tiers. No certifications. No formal skill progression. No skill specializations. No skill-based roles or qualifications. Skills are invisible numbers that affect hidden calculations. Players can't see meaningful skill progression.

**Why existing plans don't solve it:** Plan 136 (trapping→food) mentions "cooking specialization (survivor skill tree)" as follow-on only. Plan 154 (education) covers skill teaching but not formal certification. Plan 131 (rumor network) mentions "information broker specialization" as follow-on only. No plan addresses formal skill tiers or certification.

**Player value:** Creates visible progression (skill tiers players can see), adds strategic depth (certified survivors unlock capabilities), provides goals (work toward certifications), generates emergent stories (earning qualifications), and makes skill development more meaningful.

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` — current skill system
- `Assets/Ashfall.Core/Survivors/SurvivorLifecycle.cs` — survivor management
- `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` — crafting (skill-gated)
- `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` — expeditions (skill-gated)
- `Assets/StreamingAssets/Data/skills.json` — skill definitions
- NEW: `Assets/Ashfall.Core/Survivors/SkillCertificationSystem.cs`
- NEW: `Assets/StreamingAssets/Data/skill_certifications.json`

## Main Task 1 — Foundation / System Contract

1. Create `SkillCertificationSystem.cs` in `Assets/Ashfall.Core/Survivors/`
2. Define `SkillTier` DTO: `tierId`, `tierName` (novice/competent/proficient/expert/master), `skillLevel` (float threshold), `bonusModifier` (1.0-2.0), `unlockedCapabilities` (list), `description`
3. Define `SkillCertification` DTO: `certId`, `certName` (certified_medic/master_engineer/expert_tracker/etc.), `requiredSkill` (skill ID), `requiredTier` (tier ID), `requiredExperience` (list of experience requirements), `benefits` (list of unlocks), `examDifficulty` (0-100)
4. Define `SurvivorCertification` DTO: `survivorId`, `certId`, `earnedDay`, `certifyingSurvivorId` (who administered exam), `benefits` (active unlocks)
5. Define `SkillSpecialization` DTO: `specId`, `specName` (combat_medic/field_engineer/master_tracker/etc.), `parentSkill` (skill ID), `requiredCerts` (list of cert IDs), `uniqueAbilities` (list), `description`
6. Define `CertificationState` DTO: list of survivor certifications, list of specializations earned, certification exam log, skill tier progress per survivor
7. Implement `CaptureState/RestoreState` with schema versioning
8. Define skill tiers (5 levels per skill):
   - **Novice** (0-20): basic capability, no bonuses
   - **Competent** (20-40): +10% effectiveness, basic capabilities unlocked
   - **Proficient** (40-60): +25% effectiveness, intermediate capabilities
   - **Expert** (60-80): +50% effectiveness, advanced capabilities
   - **Master** (80-100): +100% effectiveness, unique capabilities
9. Define skill certifications:
   - **Certified Medic**: Medical skill Expert + first aid experience → unlocks advanced medical procedures
   - **Master Engineer**: Technical skill Master + repair experience → unlocks advanced construction
   - **Expert Tracker**: Survival skill Expert + hunting experience → unlocks rare game tracking
   - **Certified Trader**: Trade skill Proficient + market experience → unlocks faction trade negotiations
   - **Master Combatant**: Combat skill Master + battle experience → unlocks tactical leadership
   - **Certified Teacher**: Education skill Proficient + teaching experience → unlocks skill acceleration
   - **Master Chef**: Cooking skill Expert + meal preparation → unlocks advanced recipes
   - **Certified Leader**: Leadership skill Expert + command experience → unlocks governance roles
10. Define skill specializations:
    - **Combat Medic**: Certified Medic + Combat skill Proficient → battlefield medicine
    - **Field Engineer**: Master Engineer + Survival skill Competent → field repairs
    - **Master Tracker**: Expert Tracker + Navigation skill Proficient → long-range tracking
    - **Trade Master**: Certified Trader + Leadership skill Competent → trade empire
    - **Tactical Commander**: Master Combatant + Certified Leader → expedition command
    - **Academic**: Certified Teacher + 3 knowledge skills Proficient → research acceleration
11. Define certification exams:
    - Exam required to earn certification
    - Exam difficulty based on certification level
    - Exam administered by certified survivor (or self-study)
    - Exam success: skill check + experience verification
    - Exam failure: can retry after cooldown
12. Define certification benefits:
    - Unlocks new capabilities (advanced procedures, constructions)
    - Skill bonus modifier
    - Social role (certified survivors take on roles)
    - Reputation bonus
    - Teaching capability (certified can certify others)
13. Add deterministic seeding: exams use `ISeededRng`
14. Wire into `GameBootstrap`: `SetupCertifications`, `TickCertifications`, `SaveCertifications`
15. Create `SkillCertificationCatalogLoader` for certification definitions

## Main Task 2 — Implementation / Tiers / Certifications / Specializations / UI

1. Implement skill tier tracking:
   - Each skill has tier based on float level
   - Tier determines bonus modifier
   - Tier determines unlocked capabilities
   - Tier displayed in survivor detail
   - Tier progression automatic (based on skill level)
2. Implement certification earning:
   - Player initiates certification exam
   - System checks prerequisites (skill tier, experience)
   - Exam skill check with `ISeededRng`
   - Success: certification earned, benefits unlocked
   - Failure: cooldown, can retry
   - Certification recorded in survivor profile
3. Implement certification benefits:
   - Unlocked capabilities activated
   - Skill bonus applied
   - Social role assigned
   - Reputation bonus applied
   - Benefits visible in UI
4. Implement specialization:
   - Specialization requires multiple certifications
   - Specialization unlocks unique abilities
   - Specialization displayed in profile
   - Specialization is pinnacle of skill development
5. Implement certification exams:
   - Exam requires certified examiner (or self-study)
   - Exam difficulty scales with certification
   - Exam combines skill check + experience verification
   - Exam result determined deterministically
   - Exam logged in certification record
6. Implement teaching/certifying others:
   - Certified survivors can administer exams
   - Teaching skill affects exam success bonus
   - Certified teachers accelerate student learning
   - Teaching strengthens relationships
7. Implement skill decay interaction:
   - Certified skills decay slower
   - Certification protects against skill loss
   - Master-level skills never decay below Expert
   - Certification is permanent (even if skill decays)
8. Create certification events:
   - "The Exam" — certification exam taken
   - "The Certification" — certification earned
   - "The Specialization" — specialization achieved
   - "The Tier" — skill tier reached
   - "The Teacher" — survivor certifies another
   - "The Mastery" — master tier reached
   - "The Role" — social role assumed
9. Add certification quest hooks:
    - "The Student" — earn first certification
    - "The Expert" — reach Expert tier in any skill
    - "The Master" — reach Master tier in any skill
    - "The Specialist" — earn specialization
    - "The Teacher" — certify 5 survivors
    - "The Polymath" — earn certifications in 5 different skills
    - "The Legend" — earn all specializations
10. Implement certification UI:
    - Skill detail: tier, progress, capabilities
    - Certification panel: earned certifications, benefits
    - Exam panel: initiate exams, view results
    - Specialization display: earned specializations
    - Certification path: visual skill tree
11. Add certification journal: automatic log of certification events
12. Implement certification tutorial: first tier explains system
13. Add certification tooltips: hover shows requirements and benefits
14. Create 8 certification definitions + 6 specialization definitions

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `SkillProgressionSystem`: tiers based on skill levels
2. Connect to `CraftingSystem`: certifications unlock recipes
3. Integrate with `ExpeditionSystem`: certifications enable expedition roles
4. Connect to `FactionBranchCoordinator`: certifications affect faction roles
5. Wire into `DutyRosterSystem`: certifications enable duty assignments
6. Connect to `EducationSystem` (Plan 154): certified teachers
7. Implement old-save compatibility: existing survivors get tier assignments
8. Add deterministic seeding: exams use `ISeededRng`
9. Create exploit prevention: exams require prerequisites and cooldown
10. Add tests: tier progression, certification exams, specializations, benefits, save round-trip
11. Verify catalog integrity: all certification/specialization IDs resolve
12. Test edge cases: no certifications (novice), many certifications (expert survivor)
13. Verify headless behavior: certifications process correctly without UI
14. Add data-integrity-selftest: certifications validate against skill catalogs
15. Create `--certification-selftest` verb for CI validation

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --certification-selftest
```

## Risk

**LOW** — Certification is additive with clear inputs (skill levels, experience) and outputs (tiers, certifications, specializations). Risk of certifications feeling like arbitrary gates. Mitigation: ensure certifications unlock meaningful capabilities, show clear progression paths, and make specialization feel like an achievement.

## Definition of Done

- `SkillCertificationSystem.cs` exists with full `CaptureState/RestoreState`
- 5 skill tiers implemented (novice through master)
- 8 skill certifications defined and functional
- 6 skill specializations defined and functional
- Certification exam system with prerequisites
- Certification benefits (unlocks, bonuses, roles)
- Teaching/certifying others mechanic
- Skill decay interaction (certified skills protected)
- Certification events and quest hooks
- Save/load round-trip tested
- Deterministic exams verified
- Old saves get tier assignments
- 8 certifications + 6 specializations in data authority
- UI skill detail, certification panel, exam panel
- Cross-system integration (skills, crafting, expeditions, factions, duty roster, education)

## Follow-On Opportunities

- Certification specializations (unique certification combinations)
- Certification legacy (famous certified survivors remembered)
- Certification quests (specific certification goals)
- Certification trading (certified survivors traded between settlements)
- Certification research (study to create new certifications)
