# Generational Hook Matrix — Plan 12A

Documents every lifecycle hook in the generational society system: who produces it, who consumes it, once-only/repeat/cooldown semantics, and the authoritative state owner.

## Cohort/Lifecycle Hooks

| Hook | Producer | Consumer | Semantics | State Owner |
|------|----------|----------|-----------|-------------|
| Child booked | `CohortSystem.BookChild()` | Maturation eligibility, schooling choice | Once per child | CohortSystem |
| Maturation eligible | `CohortSystem` (day ≥ threshold) | `TryMaturation()`, coming-of-age events | Once-only (one-way flag) | CohortSystem |
| Maturation fired | `CohortSystem.TryMaturation()` → `OnMaturation` event | Coming-of-age events, duty eligibility | Once per child, idempotent | CohortSystem |
| Parent IDs recorded | `CohortSystem.BookChild(parentIds)` | Lineage extension, adoption arcs | Set once at booking | CohortSystem |

## Apprenticeship Hooks

| Hook | Producer | Consumer | Semantics | State Owner |
|------|----------|----------|-----------|-------------|
| Pair started | `ApprenticeshipSystem.StartPair()` | Training incident events | Once per pair | ApprenticeshipSystem |
| XP ticked | `ApprenticeshipSystem.TickDay()` | Milestone events | Daily, repeatable | ApprenticeshipSystem |
| Pair completed | `ApprenticeshipSystem` → `OnApprenticeshipCompleted` | Completion events, skill grants | Once per pair | ApprenticeshipSystem |
| Skill granted | `SkillProgressionSystem.RecordAction()` | Skill payoff events | Once per completion | SkillProgressionSystem |
| Mentor unavailable | Mentor death/away | Alternate resolution events | Event-driven | SurvivorFateSystem |

## Schooling Hooks

| Hook | Producer | Consumer | Semantics | State Owner |
|------|----------|----------|-----------|-------------|
| Curriculum choice | `schooling_curriculum_choice` event | Track selection (letters/mechanics/medicine/marksmanship) | Once per child | Event system |
| Track started | Questline entry with `schooling_*_unlock` hook | Track progression events | Once per track | Quest system |
| Track completed | Questline completion | Coming-of-age callbacks | Once per track | Quest system |

## Adoption/Guardianship Hooks

| Hook | Producer | Consumer | Semantics | State Owner |
|------|----------|----------|-----------|-------------|
| Orphan detected | `RequireCasualtyOrphan` condition flag | Adoption events | Event-driven | Flag ledger |
| Guardian candidate available | `RequireCohortAdoptionPending` condition | Adoption choice events | Once per orphan | Flag ledger |
| Adoption concluded | `RequireAdoptionConcluded` condition | Post-adoption events | Once per orphan | Flag ledger |
| Guardian death | SurvivorFateSystem death cascade | Re-orphan detection | Event-driven | SurvivorFateSystem |

## Coming-of-Age Hooks

| Hook | Producer | Consumer | Semantics | State Owner |
|------|----------|----------|-----------|-------------|
| First surface | `coming_of_age_first_surface` event | Expedition eligibility | Once per survivor | Event system |
| First watch | `coming_of_age_first_watch` event | Duty roster eligibility | Once per survivor | Event system |
| Maturation trigger | `trigger_maturation` narrative hook | CohortSystem.TryMaturation | Once per survivor | CohortSystem |

## Condition Flags (Closed Set)

| Flag | Purpose | Used By |
|------|---------|---------|
| `MinDay` | Minimum day threshold | All event types |
| `RequireFalloutStorm` | Weather condition | Environmental events |
| `RequireChildCohort` | Child must exist | Schooling, adoption |
| `RequireMaturationEligible` | Child near maturation | Coming-of-age |
| `RequireCasualtyOrphan` | Parent died, child orphaned | Adoption arcs |
| `RequireChildCohort_Rotational` | Rotational selection | Event pacing |
| `RequireCohortAdoptionPending` | Adoption decision pending | Adoption events |
| `RequireAdoptionConcluded` | Adoption resolved | Post-adoption callbacks |

## Narrative Hooks (Closed Set — 26 values)

All narrativeHook values are pinned by `Plan12AGenerationTests.Questlines_Plan12A_NarrativeHook_AreClosedAndDocumented`. Adding a new hook requires updating the test allowlist.

## Determinism Rules

- Multiple eligible children/mentors: sort by stable survivor ID before selection
- Seeded RNG only where narrative framework expects it
- Never use wall clock, hash-map order, or UI order
- Never reroll already-selected participants after save/load
