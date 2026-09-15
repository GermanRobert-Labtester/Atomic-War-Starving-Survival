# Plans 180 / 185 / 195 — Survivor capability family authority map

**Status:** ACCEPTED — §3 signed 2026-09-12  
**Package:** `DEBT-170-199-REMAINING-FAMILY-MAPS`  
**Date:** 2026-09-12

**Stale historical prose:** `Plan_180_Skill_Certification_Tier_System.md`, `Plan_185_Memory_Knowledge_Decay_System.md`, `Plan_195_Survivor_Specialization_Roles.md`.

---

## 1. Premise

| Concern | Owner | Save |
|---|---|---|
| Skill XP, active/dormant, expert flag | `SkillProgressionSystem` | nested in `apprenticeship.skillProgression` |
| Mentorship XP | `ApprenticeshipSystem` | `apprenticeship` |
| Shift “roles” (five occupancy IDs) | `DutyRosterSystem` | `duty_roster` |
| Morale atrophy (will-to-care) | `SkillAtrophySystem` | `survivor_social.atrophy` |
| Trade profession milestones | `TradeSpecialtySystem` | nested in `phase0` |
| Phantom scavenge echoes | `PhantomMemoryEngine` | `phantom_memory` |
| Library → skill XP | `LibraryStudySystem` | `library_study` |
| Research nodes | `ResearchSystem` | `research` (unlock-only, no decay) |
| `SkillCertificationSystem` / `SurvivorRoleSystem` / `MemoryDecaySystem` | **ABSENT** | |

Certification is **already derived**: XP + `activeSkillIds` / `dormantSkillIds` + expert flag. Duty roster does **not** read skills. `SkillProgressionSystem.TickDaily` (14-day unused → dormant) is **authored but not host-ticked**.

---

## 2. Ownership (proposed)

“Certified / specialist” is a **read of** `SkillProgression` (+ optional `TradeSpecialty`). Duty eligibility may *project* that read. No exam row, no parallel role counter.

---

## 3. Recommended defaults

| Item | Default |
|---|---|
| New cert / role / memory-decay ledgers | **OUT** |
| Fading research, journals, or phantom clarity | **OUT** |
| Silent duty reassignment from skills | **OUT** |
| Certification = derived from existing skill state | **IN** |
| Host-wire existing `SkillProgression.TickDaily` (dormancy) | **IN** (implement later) |
| Optional `DutyRoster.ValidateAssign` reads active skills | **IN** (later, after map) |

**Next implement:** `DEBT-185-SKILL-DORMANCY-TICK` — call existing `TickDaily` from the campaign day owner. No new save section.

---

## 4. Evidence paths

`SkillProgressionSystem.cs`, `SkillDef.cs`, `skills.json`, `ApprenticeshipSystem.cs`, `DutyRosterIds.cs`, `SkillAtrophySystem.cs`, `PhantomMemoryEngine.cs`, `src/UI/SkillMatrixPanel.cs`, `src/Main.CampaignServices.cs`

**Signed 2026-09-12.** Implement packages listed in this map stay unclaimed until separately promoted.
