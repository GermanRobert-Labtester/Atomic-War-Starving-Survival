# Plan 12 — Baseline Reconnaissance

## Build & Test Status (2026-09-01)

- `dotnet build Ashfall.Core.Tests` — **PASS** (0 errors, 0 warnings)
- `dotnet test --filter Plan12` — **39/39 PASS**
- Plan 12A tests: 7 tests (cohort maturation, apprenticeship skill grants, event authoring, condition flags, questline content, narrative hooks)
- Plan 12B tests: 9 tests (belief sets, conflict reciprocity, sleep penalty, synergy, graffiti catalog, event bundle, choices/effects, world flags, posting triggers)
- Plan 12C tests: 23 tests (assign/remove, memorial plaque, morale delta, save round-trip, save section registry, 12 decor items, catalog loading, player surface manifest)

## Authority Map — Who Owns What

| State | Authoritative Owner | File | Plan 12 Use |
|-------|-------------------|------|-------------|
| Survivor age/cohort | `CohortSystem` | `Assets/Ashfall.Core/CohortSystem.cs` | Coming-of-age eligibility, maturation |
| Parent/lineage | `GenerationalLineageExtension` + `GenerationalSuccessionEngine` | `Assets/Ashfall.Core/GenerationalLineageExtension.cs`, `Assets/Ashfall.Core/Legacy/GenerationalSuccessionEngine.cs` | Raised-by/guardian outcomes, generation tracking |
| Apprenticeship | `ApprenticeshipSystem` | `Assets/Ashfall.Core/ApprenticeshipSystem.cs` | Six authored arcs, skill grants |
| Skill progression | `SkillProgressionSystem` | `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` | Schooling/apprentice payoff |
| Relationship | `SurvivorRelationsSystem` | `Assets/Ashfall.Core/Survivors/SurvivorRelationsSystem.cs` | Mediation/mentor bonds, kinship |
| Ideological friction | `IdeologicalFrictionSystem` | `Assets/Ashfall.Core/Survivors/IdeologicalFrictionSystem.cs` | 4 belief sets, social event eligibility |
| Ration grievance | `RationConflictSystem` | `Assets/Ashfall.Core/Survivors/RationConflictSystem.cs` | Ration disputes/escalation |
| Leadership | `LeadershipSystem` | `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` | Challenge consequences |
| Morale | `NeedsSystem` | `Assets/Ashfall.Core/NeedsSystem.cs` | Localized decor effect |
| Memorial | `MemorialSystem` | `Assets/Ashfall.Core/MemorialSystem.cs` | Plaque provenance |
| Room assignment | `ShelterAssignmentSystem` | `Assets/Ashfall.Core/ShelterAssignmentSystem.cs` | Occupant lookup |
| Decor assignment | `ShelterDecorSystem` | `Assets/Ashfall.Core/Shelter/ShelterDecorSystem.cs` | Room expression |
| Social coordination | `SurvivorSocialCoordinator` | `Assets/Ashfall.Core/Survivors/SurvivorSocialCoordinator.cs` | Orchestrates friction/ration/relations |

## Content Counts — What Exists

### Task 12A — Generational Society
- **Schooling tracks:** 4 (Letters & Records, Mechanics & Maintenance, Medicine & Care, Marksmanship & Watchcraft)
- **Apprenticeship arcs:** 6 (pipefitting, field dressing, radio/signal, recycling/workshop, hatch/watch, triage)
- **Adoption arcs:** 4 (Warmarms, Fierce Mother, Grange, Archive)
- **Coming-of-age events:** 2 (first surface, first watch)
- **Apprenticeship completion events:** 6 (one per arc, each granting canonical skill)
- **Schooling curriculum choice event:** 1
- **Orphan adoption events:** 4
- **Total Plan 12A events:** 13
- **Questlines authored:** 20 (4 child + 6 apprentice + 4 schooling + 4 adoption + 2 coming-of-age)
- **Condition flags:** 8 (MinDay, RequireFalloutStorm, RequireChildCohort, RequireMaturationEligible, RequireCasualtyOrphan, RequireChildCohort_Rotational, RequireCohortAdoptionPending, RequireAdoptionConcluded)
- **Narrative hooks:** 26 unique values in closed set

### Task 12B — Friction & Ration-Conflict
- **Belief sets:** 4 (ration_collectivist, every_soul_alone, faith_in_rebuild, ash_nihilist)
- **Friction events:** 10 (snoring feud, stolen keepsake, forbidden radio, work shirker, stealing sleep, ash sermon, inherited walkout, two sides of chalk, ration observation, stolen dry rations)
- **Ration conflict events:** 6 (uneven scoop, hoarded tins, feast day, sick gets more, theft with evidence, resentment kindling)
- **Escalation events:** 4 (walkout, sabotage, challenge to leadership, walkout reconsidered)
- **Total Plan 12B events:** 20
- **World flags:** 42 in closed set
- **Graffiti postings:** ≥10 (ration complaint, duty reminder, memorial message, ideological slogan, apology, theft warning, praise, leadership criticism, childcare note, communal message)
- **Belief mechanics:** conflict pairs produce sleep penalty, matching beliefs produce synergy (>1.0 multiplier)

### Task 12C — Shelter Decor
- **Core system:** `ShelterDecorSystem` with Assign/Remove/GetSlot/ListRoomPlacements/GetRoomMoraleDelta/ResolvePlaqueItemId/ResolvePlaqueSlot/CaptureState/RestoreState
- **Decor items:** 12 (poster_ration, poster_warning, locomotive_nameplate, carved_memorial, chalk_drawing, pressed_flower, medal_civic, classroom_chart, signal_log, memorial_plaque_generic, memorial_plaque_carving, memorial_plaque_drawing)
- **Save section:** `shelter_decor` registered in SaveSectionRegistry → `shelter_decor_save.json`
- **Host session:** `ShelterDecorHostSession`
- **Self-test:** `--shelter-decor-selftest`
- **UI panel:** `ShelterDecorPanel` with snapshot fixture
- **Memorial bridge:** plaque metadata (survivorId, heirloomId) round-trips through save
- **Morale model:** per-item `decorLocalizedMoraleDelta` summed per room

## What Was Already Implemented Before This Session

All of Tasks 12A, 12B, and 12C were implemented prior to this session with comprehensive test coverage. The only fix needed was adding 3 cipher narrative hooks (`cipher_relay_decode`, `cipher_rotation_decode`, `cipher_winter_decode`) to the Plan 12A allowlist — these were added by other plans and the closed-set test correctly caught them.

## What Remains

### Task 12D — Cross-System Social Continuity
- Cross-hook producer/consumer matrix (documentation)
- Chronology guards (tests verifying ordering constraints)
- Participant validity handling (tests for death/away/incapacity at event resolution)
- Pending-state persistence (tests for save/load at critical boundaries)

### Task 12E — Balance, Frequency & Long-Campaign Simulation
- Deterministic simulation tests (social event frequency over multi-day windows)
- Frequency targets (evidence-based bounds)
- Morale balance comparison (undecorated vs decorated)
- Social failure recovery (high-friction shelter simulation)
- Repetition audit (event/participant distribution over long runs)

### Documentation (15 deliverables)
All 15 docs listed in Plan 12 §2 need to be created in `docs/social/`.

### Completion Report
`docs/social/PLAN12_COMPLETION_REPORT.md` with evidence-backed verification.
