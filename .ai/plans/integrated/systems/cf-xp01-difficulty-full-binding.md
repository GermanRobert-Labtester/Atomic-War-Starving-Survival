# Feature / Task Plan: CF-XP01 Difficulty Full Binding & Hostile Encounter Coverage

# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**

STATUS: APPROVED BY USER

## 1. Goal & Outcome
- **Goal:** Full integration and sealing of CF-XP01 (Difficulty Full Binding), closing the single remaining scalar regression coverage gap on `hostile_encounter_mult`.
- **Deliverables:**
  1. Add `HostileEncounter_DifficultyMultiplier_ScalesExistingDangerComposition_AndPreservesStandardParity` to `Ashfall.Core.Tests/Difficulty/DifficultyFullBindingTests.cs` (bringing tests from 7 to 8).
  2. Add `hostile_consumer` check to `RunDifficultySelfTest` in `src/Host/HostCli.Difficulty.cs` (bringing checks from 14 to 15).
  3. Validate host and test builds (0 warnings, 0 errors).
  4. Prepend mandatory `FULLY INTEGRATED` header and move plan to `.ai/plans/integrated/systems/` and `docs/plans/integrated/systems/`.
- **Non-Goals:**
  - No new difficulty authorities, no changes to catalog schemas, no new save sections.

## 2. Claimed Paths & Affected Files
- `Ashfall.Core.Tests/Difficulty/DifficultyFullBindingTests.cs`
- `src/Host/HostCli.Difficulty.cs`
- `docs/plans/CF_XP01_DIFFICULTY_FULL_BINDING_INTEGRATION_PLAN.md`
- `INTEGRATION_PLANS.md`
- `AGENTS.md`

## 3. Pre-flight Checks
- [x] Go config/save validator passes (`bin/validate-config`)
- [x] No duplicate/parallel difficulty authority introduced

## 4. Implementation Steps
1. Add test for hostile encounter multiplier in `DifficultyFullBindingTests.cs`.
2. Add hostile encounter check in `HostCli.Difficulty.cs`.
3. Verify with `run_test.sh` and `dotnet build`.
4. Prepend `FULLY INTEGRATED` header multiple times to plan files.
5. Move plans to integrated directory under `systems/`.

## 5. Verification
- [x] Scoped tests pass via `bin/run-scoped-tests` / `scripts/run_test.sh`
- [x] `bin/check-approved-plan` passes
