# Partial-plan production unblock implementation log

Date: 2026-09-19
Authority: direct user request following `PARTIAL_15_PRODUCTION_UNBLOCK_INTEGRATION_PLAN.md`

## Bounded outcome

Complete three partially integrated plans through their existing authorities:

1. Plan 143 / C1[23] — project live medical facts into the canonical duty-fitness verdict.
2. Plan 208 / C2[42] — complete leadership succession/challenge validation, persistence, host commands, and survivor-relations readout.
3. Plan 139 / C1[22] — route completed combat consequences exactly once into the Year of Ash faction-standing authority.

Non-goals: no new save section, no duplicate faction/worker/politics authority, no JSON schema expansion, no edits to foreman ledgers, and no unrelated cleanup.

## Ownership and files

The live ownership ledger has no claim on these exact paths. Shared host/UI seams are treated as the integrator-owned portion of this user-authorized package. `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `KNOWN_DEBT.md` remain untouched.

### Plan 143

- `Assets/Ashfall.Core/Medical/AfflictionDutyBridge.cs`
- `Assets/Ashfall.Core/Survivors/FitnessForDutyModel.cs`
- `src/Main.SurvivorFitness.cs`
- `Ashfall.Core.Tests/Medical/AfflictionDutyBridgeTests.cs`

### Plan 208

- `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs`
- `Assets/Ashfall.Core/Survivors/SurvivorSocialCoordinator.cs`
- `src/Main.SurvivorSocial.cs`
- `src/UI/SurvivorRelationsPanel.cs`
- `Ashfall.Core.Tests/Survivors/LeadershipSuccessionTests.cs`
- `Ashfall.Core.Tests/SurvivorSocialCoordinatorTests.cs`

### Plan 139

- `Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs`
- `Assets/Ashfall.Core/Combat/CombatTypes.cs`
- `Assets/Ashfall.Core/Combat/TacticalCombatSystem.Persistence.cs`
- `src/Host/CombatHostSession.cs`
- `src/Main.Expeditions.cs`
- `src/Main.Muster.cs`
- `src/UI/CombatPanel.cs`
- `Ashfall.Core.Tests/Combat/CombatFactionStandingBridgeTests.cs`

## Baseline

- `LeadershipSuccessionTests.cs`: 6/6 passed.
- `AfflictionDutyBridgeTests.cs`: 6/6 passed.
- `CombatFactionStandingBridgeTests.cs`: 7/7 passed.

The baseline proves the partial islands in isolation; it does not prove production routing, authoritative ownership, or save continuity.

## Phase log

Phases are appended here after each focused verification.

### Phase 1 — Plan 143 / C1[23] complete

- Replaced the standalone affliction/work-efficiency island with a stateless overlay on `FitnessForDutyModel.EvaluateForRole`.
- Consumes `FitnessEvaluationFacts` plus the catalog's typed `HazardClass`; no role-name or affliction-ID substring matching remains.
- Added live respiratory impairment projection from the existing Phase 0 respiratory authority.
- Infectious food/medical work blocks; trauma, respiratory impairment, and precision-work withdrawal warnings shorten shifts without overriding stricter base verdicts.
- Verification: `bash scripts/run_test.sh Ashfall.Core.Tests/Medical/AfflictionDutyBridgeTests.cs` — 6/6 passed.

### Phase 2 — Plan 208 / C2[42] complete for the audited partial scope

- Kept `LeadershipSystem` inside `SurvivorSocialCoordinator` and the existing `survivor_social` save aggregate.
- Added fail-closed alive/self/duplicate challenge checks, stale-target protection, dead-candidate cleanup, deterministic challenge-ID continuation after restore, and deterministic capture ordering.
- Added coordinator and Main command routes for successor, deputy, challenge initiation, and challenge resolution.
- Added successor/deputy/challenge facts to the survivor-social read model and the existing survivor-relations panel.
- Leadership mutations now mark the existing aggregate dirty and refresh the panel immediately.
- Verification: `LeadershipSuccessionTests.cs` — 10/10 passed; `SurvivorSocialCoordinatorTests.cs` — 8/8 passed.

### Phase 3 — Plan 139 / C1[22] complete for the audited partial scope

- Removed the bridge-owned marker set and both competing faction mutation targets.
- The bridge now accepts exactly one injected integer standing sink; production binds it to `YearOfAshHostSession.FactionWar.ModifyStanding`.
- Added deterministic faction/incident ordering, explicit defensive-combat context for expedition ambushes and shelter raids, and fail-closed behavior when the standing sink is unavailable.
- Bumped `CombatState` to save version 4 and persisted self-defense context, applied incident IDs, and the combat consequence report through the existing combat section.
- Restored resolved encounters retry only pending incidents; applied incident IDs suppress replay across save/restore.
- Existing combat and faction views observe the actual applied integer delta.
- Verification: `CombatFactionStandingBridgeTests.cs` — 9/9 passed; `CombatSystemTests.cs` — 22/22 passed; `--combat-selftest` — 26/26 passed.

## Final regression gate

- `Plan24FitnessForDutyTests.cs` — 11/11 passed.
- `Plan24DutyRosterFitnessTests.cs` — 7/7 passed.
- `LeadershipSystemTests.cs` — 20/20 passed.
- `dotnet build Ashfall.csproj --no-restore` — passed with 0 warnings and 0 errors.
- `--duty-roster-selftest` — 40/40 passed at the required 15 FPS.
- `--scene-binding-selftest` — 25/25 passed at the required 15 FPS.
- Initial Godot invocation failed before project startup because the default `user://logs` target was unavailable; rerunning the same bounded commands with an isolated `/tmp` log path passed. No project assertion failed in that first attempt.
