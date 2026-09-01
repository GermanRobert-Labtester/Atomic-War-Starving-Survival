# Plan 195 — Survivor Specialization Roles

## Goal

Create a survivor specialization and role system where survivors develop formal roles (medic, engineer, scout, leader, technician) with unique bonuses, responsibilities, and progression paths beyond simple duty assignments. Currently `DutyRosterSystem` assigns survivors to work tasks and `ApprenticeshipSystem` (150 lines) enables mentor-apprentice skill transfer, but there is no formal role system — no role-based bonuses, no role progression, no role responsibilities, no role identity. Survivors are interchangeable workers assigned to tasks rather than specialists with expertise. This plan adds character depth and strategic specialization.

## Why

**Repository evidence:** Grep for `SurvivorRole`, `SurvivorSpecialization`, `SurvivorClass`, `SurvivorProfession`, `SurvivorExpertise`, `RoleBonus`, `SpecializationBonus` in Core returns ZERO matches. `DutyRosterSystem` assigns survivors to duty slots (tasks) but doesn't create role identity. `ApprenticeshipSystem` (150 lines) enables skill transfer but doesn't create formal roles. `SkillProgressionSystem` (728 lines) tracks skills but doesn't create role-based bonuses. No role system, no specialization bonuses, no role progression, no role responsibilities.

**What is missing:** No formal role system. No role-based bonuses (medics get healing bonuses, engineers get repair bonuses). No role progression (novice medic → expert medic). No role responsibilities (medics auto-tend wounded, engineers auto-repair). No role identity (survivors known as "the medic" not just "survivor #47"). Survivors are task-assigned workers, not specialists with expertise.

**Why existing plans don't solve it:** Plan 154 (education) adds schooling but not specialization roles. Plan 180 (certifications) adds skill certification but not role bonuses. Plan 188 (daily routines) adds personal schedules but not role identity. Plan 193 (chronic conditions) adds impairments but not role specializations. No plan addresses survivor specialization roles.

**Player value:** Creates character identity (each survivor has a role), adds strategic depth (specialize survivors for efficiency), generates emergent stories (the hero medic, the grizzled engineer), and makes survivors feel unique rather than interchangeable.

## Files / Systems to Inspect

- `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs` — duty assignments
- `Assets/Ashfall.Core/ApprenticeshipSystem.cs` — skill transfer
- `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` — skill system
- `Assets/Ashfall.Core/Survivors/SurvivorLifecycle.cs` — lifecycle
- NEW: `Assets/Ashfall.Core/Survivors/SurvivorRoleSystem.cs`
- NEW: `Assets/StreamingAssets/Data/survivor_roles.json`

## Main Task 1 — Foundation / System Contract

1. Create `SurvivorRoleSystem.cs` in `Assets/Ashfall.Core/Survivors/`
2. Define `SurvivorRole` DTO: `roleId`, `roleName` (medic/engineer/scout/leader/technician/scientist/diplomat/enforcer), `roleCategory` (medical/technical/exploration/leadership/support/combat/social), `requiredSkills` (list of skill_ids with minimum levels), `roleBonuses` (list of capability modifiers), `roleResponsibilities` (list of auto-actions), `roleProgression` (novice/apprentice/journeyman/expert/master), `assignedSurvivorId`
3. Define `RoleBonus` DTO: `bonusId`, `roleId`, `bonusType` (skill_bonus/resource_efficiency/speed_bonus/quality_bonus/success_chance), `affectedCapability` (skill_id or action_type), `bonusValue` (multiplier or flat bonus), `progressionLevel` (which role level unlocks this)
4. Define `RoleResponsibility` DTO: `responsibilityId`, `roleId`, `responsibilityType` (auto_heal/auto_repair/auto_scout/auto_lead/auto_teach/auto_guard), `triggerCondition` (when to auto-act), `actionTaken` (what auto-action occurs), `resourceCost` (items consumed), `successChance` (0-100)
5. Define `RoleProgression` DTO: `progressionId`, `roleId`, `level` (novice/apprentice/journeyman/expert/master), `xpRequired` (experience points to reach this level), `xpEarned` (current XP), `unlockedBonuses` (list of bonus_ids), `unlockedResponsibilities` (list of responsibility_ids)
6. Define `SurvivorRoleState` DTO: list of survivor role assignments, list of role progressions, list of active role bonuses, list of role responsibilities, role settings (auto-assign roles bool, role cap per type)
7. Implement `CaptureState/RestoreState` with schema versioning
8. Define role types (8+ roles):
   - **Medic**: heals wounded, treats disease, performs surgery, bonuses to medical skills
   - **Engineer**: repairs equipment, builds structures, maintains systems, bonuses to crafting/repair
   - **Scout**: explores, maps, detects threats, bonuses to expedition/stealth
   - **Leader**: coordinates survivors, boosts morale, negotiates, bonuses to social/management
   - **Technician**: operates complex systems, maintains tech, hacks devices, bonuses to tech skills
   - **Scientist**: researches, analyzes, experiments, bonuses to research/analysis
   - **Diplomat**: negotiates with factions, trades, resolves conflicts, bonuses to trade/social
   - **Enforcer**: defends shelter, trains combat, maintains order, bonuses to combat/security
9. Define role requirements:
   - Each role requires specific skills at minimum levels
   - Example: Medic requires medical_skill ≥ 50, first_aid ≥ 30
   - Example: Engineer requires crafting_skill ≥ 50, repair ≥ 30
   - Example: Scout requires stealth ≥ 40, navigation ≥ 30
   - Survivors must meet requirements to take role
   - Multiple survivors can hold same role (with cap)
10. Define role bonuses:
    - Each role provides bonuses to relevant capabilities
    - Medic: +20% healing effectiveness, +15% disease treatment speed
    - Engineer: +20% repair speed, +15% crafting quality
    - Scout: +25% expedition speed, +20% threat detection
    - Leader: +15% morale bonus to nearby survivors, +10% negotiation success
    - Technician: +20% tech operation speed, +15% hack success
    - Scientist: +25% research speed, +20% analysis accuracy
    - Diplomat: +20% trade profit, +15% faction standing gain
    - Enforcer: +20% combat effectiveness, +15% security response
11. Define role responsibilities (auto-actions):
    - Medic: auto-tend wounded survivors (when injury detected)
    - Engineer: auto-repair damaged equipment (when breakdown detected)
    - Scout: auto-detect threats (when expedition starts)
    - Leader: auto-boost morale (when morale low)
    - Technician: auto-maintain tech systems (when degradation detected)
    - Scientist: auto-conduct research (when research queued)
    - Diplomat: auto-negotiate trade (when trade available)
    - Enforcer: auto-respond to security threats (when attack detected)
    - Responsibilities consume resources (medical supplies, repair parts, etc.)
12. Define role progression:
    - 5 levels: novice → apprentice → journeyman → expert → master
    - XP earned through role-related actions
    - Each level unlocks better bonuses + more responsibilities
    - Master level: maximum bonuses, all responsibilities unlocked
    - Progression tracked per role per survivor
13. Define role identity:
    - Survivors with roles known by role title ("Elena the Medic")
    - Role displayed in survivor detail
    - Role affects survivor dialogue/interactions
    - Role creates character identity
14. Add deterministic seeding: role assignment uses `ISeededRng`
15. Wire into `GameBootstrap`: `SetupSurvivorRoles`, `TickSurvivorRoles`, `SaveSurvivorRoles`

## Main Task 2 — Implementation / Roles / Bonuses / Responsibilities / Progression / UI

1. Implement role assignment:
   - Player selects survivor and role
   - System checks requirements (skills, availability)
   - Role assigned if requirements met
   - Role bonuses applied
   - Assignment logged
2. Implement role bonuses:
   - Calculate bonus modifiers per survivor
   - Apply bonuses to relevant capabilities
   - Bonuses stack with skill bonuses
   - Bonuses displayed in survivor detail
3. Implement role responsibilities:
   - Detect trigger conditions (injury, breakdown, threat, etc.)
   - Auto-assign role-holding survivor to respond
   - Consume required resources
   - Execute auto-action
   - Log responsibility execution
4. Implement role progression:
   - Track XP earned per role per survivor
   - XP gained from role-related actions
   - Level up when XP threshold reached
   - Unlock new bonuses/responsibilities
   - Progression logged
5. Implement role caps:
   - Limit number of survivors per role (prevent all survivors being medics)
   - Configurable role caps
   - Caps enforced during assignment
   - Caps displayed in UI
6. Implement role identity:
   - Display role title with survivor name
   - Role affects dialogue options
   - Role affects interactions
   - Role creates character depth
7. Implement role UI:
   - Survivor detail: role display, bonuses, responsibilities, progression
   - Role assignment panel: select role, check requirements, assign
   - Role management panel: view all roles, caps, assignments
   - Role progression panel: XP, levels, unlocks
   - Role responsibility log: auto-actions taken
8. Implement role training:
   - Survivors can train for roles (gain required skills)
   - ApprenticeshipSystem integration (Plan 195)
   - Training logged
   - Training affects role readiness
9. Implement role consequences:
   - Role-holding survivors excel at role tasks
   - Role-holding survivors may be less effective at non-role tasks
   - Role specialization creates strategic choices
   - Role identity affects survivor morale (pride in expertise)
10. Create role events:
    - "The Assignment" — survivor assigned to role
    - "The Promotion" — survivor leveled up in role
    - "The Mastery" — survivor reached master level
    - "The Responsibility" — auto-action executed
    - "The Expertise" — role bonus applied
    - "The Identity" — survivor known by role title
    - "The Training" — survivor training for role
    - "The Team" — multiple roles working together
11. Add role quest hooks:
    - "The Specialist" — assign 5 survivors to different roles
    - "The Master" — reach master level in any role
    - "The Team" — have all 8 roles filled
    - "The Expert" — reach expert level in 3 roles
    - "The Mentor" — train 3 apprentices to journeyman
    - "The Response" — execute 50 role responsibilities
    - "The Identity" — 10 survivors known by role titles
12. Implement role tutorial: first role assignment explains system
13. Add role tooltips: hover over role shows bonuses, requirements, progression
14. Create role definitions in data file (8+ roles)
15. Implement role persistence: roles saved with survivor state

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `DutyRosterSystem`: roles complement duty assignments
2. Connect to `ApprenticeshipSystem`: role training through apprenticeship
3. Integrate with `SkillProgressionSystem`: role requirements check skills
4. Connect to `NeedsSystem`: role responsibilities affect needs
5. Wire into `CombatSystem`: enforcer role bonuses
6. Connect to `ExpeditionSystem`: scout role bonuses
7. Implement old-save compatibility: existing saves get no roles (all survivors unassigned)
8. Add deterministic seeding: role assignment uses `ISeededRng`
9. Create exploit prevention: roles require skill investment, can't be gamed
10. Add tests: role assignment, bonuses, responsibilities, progression, caps, save round-trip
11. Verify all role types work correctly
12. Test edge cases: no roles (current behavior), all roles filled
13. Verify headless behavior: roles process correctly without UI
14. Add data-integrity-selftest: roles validate against skill catalogs
15. Create `--survivor-role-selftest` verb for CI validation

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --survivor-role-selftest
```

## Risk

**LOW** — Survivor roles are straightforward with clear inputs (skills, assignment) and outputs (bonuses, responsibilities). Risk of roles feeling restrictive rather than empowering. Mitigation: make roles optional, allow role changes, show clear benefits, and ensure roles add depth not complexity.

## Definition of Done

- `SurvivorRoleSystem.cs` exists with full `CaptureState/RestoreState`
- 8+ role types (medic, engineer, scout, leader, technician, scientist, diplomat, enforcer)
- Role requirements (skill minimums)
- Role bonuses (capability modifiers)
- Role responsibilities (auto-actions)
- Role progression (5 levels: novice → master)
- Role caps (limit per role type)
- Role identity (title display, dialogue effects)
- Role events and quest hooks
- Save/load round-trip tested
- Deterministic role assignment verified
- Old saves load with no roles assigned
- Role definitions in data authority
- UI role assignment panel, management panel, progression panel
- Cross-system integration (duty roster, apprenticeship, skills, needs, combat, expedition)

## Follow-On Opportunities

- Role specialization (unique role variants)
- Role legacy (famous role-holders remembered)
- Role quests (specific role goals)
- Role events (role conflicts, role synergies)
- Role trading (transfer role knowledge between survivors)
