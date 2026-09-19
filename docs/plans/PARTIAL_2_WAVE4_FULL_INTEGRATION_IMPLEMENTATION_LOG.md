# Partial Wave 4 — Plans 216 + 202 Integration Log

Date: 2026-09-19
Status: implemented and verified by the integrator

This log records the bounded production integration of the next two ranked
partial plans from `PARTIAL_15_PRODUCTION_UNBLOCK_INTEGRATION_PLAN.md`.

## Plan 216 — Exercise / physical conditioning

The existing `ExerciseSystem` remains the unique owner of conditioning
profiles, workout progression, injury-result facts, and deconditioning. It is
now composed by `SurvivorSocialCoordinator`, not constructed as a second host
system. Its state is captured/restored inside the existing `survivor_social`
aggregate; no new save section was added.

The workout completion event routes fatigue once through
`NeedsSystem.ApplyAttributedDelta` with source `exercise.workout`. The host
command `Main.ExecuteSurvivorWorkout` checks the established fitness model,
uses the campaign Social RNG fork for the injury roll, marks the existing
social section dirty, and refreshes the current social read surface.

`FitnessEvaluationFacts.Conditioning` is a read-only projection of the
persisted exercise profile. `FitnessForDutyModel` contributes a conservative
`low_conditioning` reason and fatigue need when the profile is below its
capability bands; it does not replace health, fatigue, medical, age, or duty
authority. The existing survivor-relations/social read model and panel show the
current conditioning score. Fitness/read-model projections use a non-mutating
lookup, so refreshes do not create new persistent profiles; profiles are not
created while the duty roster is bootstrapping.

## Plan 202 — Interpersonal conflict / grievance

The standalone conflict state remains available for its local contract tests,
but it is not installed as a competing campaign conflict ledger. The new
`InterpersonalConflictSystem.ProjectCanonicalRelations` and
`ProjectCanonicalGrievances` methods produce deterministic typed read models
from `SurvivorRelationsState`'s persisted conflicts, affinity, and resentment.
They do not mutate relations, roll random conflicts, or copy mutable authority.

`Main.GetInterpersonalConflictProjection` and
`Main.GetInterpersonalGrievanceProjection` expose those projections. The host
mediation command accepts either the underlying canonical conflict ID or its
`relations:` projection ID and routes the resolution once through
`SurvivorRelationsSystem.Mediate`, preserving canonical affinity and mediation
history. The existing survivor-relations panel remains the presentation owner;
its canonical mediation event is subscribed once so the typed resolution
morale fact reaches `NeedsSystem` regardless of which command surface initiated
the mediation.

## Files changed

- `Assets/Ashfall.Core/Survivors/SurvivorSocialCoordinator.cs`
- `Assets/Ashfall.Core/Survivors/FitnessForDutyModel.cs`
- `Assets/Ashfall.Core/Survivors/InterpersonalConflictSystem.cs`
- `src/Main.SurvivorFitness.cs`
- `src/Main.ShelterSocial.cs`
- `src/Main.Plans216_202Interpersonal.cs`
- `src/UI/SurvivorRelationsPanel.cs`
- `Ashfall.Core.Tests/Survivors/ExerciseSystemTests.cs`
- `Ashfall.Core.Tests/Survivors/InterpersonalConflictSystemTests.cs`
- `Ashfall.Core.Tests/Survivors/Plan216_202CoordinatorIntegrationTests.cs`

## Focused verification

- `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/ExerciseSystemTests.cs` — **7/7 passed**
- `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/InterpersonalConflictSystemTests.cs` — **7/7 passed**
- `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/Plan216_202CoordinatorIntegrationTests.cs` — **2/2 passed**
- `dotnet build Ashfall.csproj --no-restore` — **succeeded, 0 warnings, 0 errors**

No Godot scene or JSON catalog change was required. The existing social save
section, survivor-relations panel, needs owner, fitness model, and campaign RNG
stream remain the authorities. Plans 163, 210, 219, and 167 remain deferred
placeholders pending their own premise audits.
