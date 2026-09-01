# Apprenticeship Content Matrix — Plan 12A

Six authored apprenticeship arcs, each with assignment, mentor eligibility, training incident, setback, payoff, skill progression, relationship consequences, and alternate resolution.

## Arc 1 — Shelter Mechanic (Pipefitting)

| Field | Value |
|-------|-------|
| Quest ID | `quest_apprentice_pipefitting` |
| Completion Event | `apprenticeship_completion_rough_repairs` |
| Skill Granted | `skill_rough_repairs` |
| Narrative Hook | `apprenticeship_completion:skill_rough_repairs` |
| Mentor Eligibility | Qualified in `skill_rough_repairs` (XP ≥ threshold) |
| Training Incident | First solo repair under pressure |
| Setback | Tool not returned / mentor injury |
| Payoff | Functional repair saves shelter resource |
| Relationship | Mentor-apprentice bond (+affinity) |
| Alternate Resolution | If mentor dies: self-taught variant with reduced XP |

## Arc 2 — Field Medic (Dressing)

| Field | Value |
|-------|-------|
| Quest ID | `quest_apprentice_dressing` |
| Completion Event | `apprenticeship_completion_field_dressing` |
| Skill Granted | `skill_field_dressing` |
| Narrative Hook | `apprenticeship_completion:skill_field_dressing` |
| Mentor Eligibility | Qualified in `skill_field_dressing` |
| Training Incident | First assisted surgery |
| Setback | Patient complications / mentor called away |
| Payoff | Life saved under pressure |
| Relationship | Deep trust bond |
| Alternate Resolution | If mentor dies: textbook study variant |

## Arc 3 — Signal Specialist (Radio)

| Field | Value |
|-------|-------|
| Quest ID | `quest_apprentice_radio` |
| Completion Event | `apprenticeship_completion_signal_ear` |
| Skill Granted | `skill_signal_ear` |
| Narrative Hook | `apprenticeship_completion:skill_signal_ear` |
| Mentor Eligibility | Qualified in `skill_signal_ear` |
| Training Incident | First independent signal decode |
| Setback | Radio failure / interference crisis |
| Payoff | Critical message received |
| Relationship | Intellectual mentorship |
| Alternate Resolution | If mentor dies: recorded tapes self-study |

## Arc 4 — Workshop Generalist (Recycling)

| Field | Value |
|-------|-------|
| Quest ID | `quest_apprentice_recycling` |
| Completion Event | `apprenticeship_completion_workshop_sense` |
| Skill Granted | `skill_workshop_sense` |
| Narrative Hook | `apprenticeship_completion:skill_workshop_sense` |
| Mentor Eligibility | Qualified in `skill_workshop_sense` |
| Training Incident | First salvage assessment |
| Setback | Material shortage / workshop accident |
| Payoff | Key component fabricated from scrap |
| Relationship | Practical working bond |
| Alternate Resolution | If mentor dies: reduced-scope self-teaching |

## Arc 5 — Watch Specialist (Hatch)

| Field | Value |
|-------|-------|
| Quest ID | `quest_apprentice_hatch` |
| Completion Event | `apprenticeship_completion_watchful` |
| Skill Granted | `skill_watchful` |
| Narrative Hook | `apprenticeship_completion:skill_watchful` |
| Mentor Eligibility | Qualified in `skill_watchful` |
| Training Incident | First solo watch shift |
| Setback | False alarm / fatigue incident |
| Payoff | Threat detected early |
| Relationship | Discipline/respect bond |
| Alternate Resolution | If mentor dies: paired watch with another guard |

## Arc 6 — Triage Specialist

| Field | Value |
|-------|-------|
| Quest ID | `quest_apprentice_triage` |
| Completion Event | `apprenticeship_completion_steady_hands` |
| Skill Granted | `skill_steady_hands` |
| Narrative Hook | `apprenticeship_completion:skill_steady_hands` |
| Mentor Eligibility | Qualified in `skill_steady_hands` |
| Training Incident | First mass-casualty triage |
| Setback | Resource triage failure / moral injury |
| Payoff | Correct prioritization saves lives |
| Relationship | Heavy trust bond |
| Failure State | Moral injury: apprentice refuses further medical duty (narratively valid failure) |

## Common Mechanics

- **Assignment:** `ApprenticeshipSystem.StartPair(mentorId, apprenticeId, skillId, targetXp)`
- **Progression:** `ApprenticeshipSystem.TickDay(day)` — XP accumulates daily
- **Completion:** When XP ≥ targetXp → `OnApprenticeshipCompleted` fires → `SkillProgressionSystem.RecordAction()` grants canonical skill
- **No parallel counters:** All skill progression routes through `SkillProgressionSystem`
- **Mentor death:** Incomplete apprenticeship preserved in state; narrative alternate resolution triggered
- **Determinism:** Pair selection sorted by stable survivor ID; seeded RNG for any randomization
