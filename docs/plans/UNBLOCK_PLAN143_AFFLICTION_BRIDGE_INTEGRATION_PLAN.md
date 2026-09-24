# UNBLOCK — Plan 143: Medical Afflictions → Quest & Work Bridge

**Status:** SEALED — user-authorized full host integration completed
**Claim:** `claim-unblock-plan-143-affliction-bridge-2026-09-24`
**Package:** `UNBLOCK-PLAN-143-AFFLICTION-BRIDGE`
**Scope rule:** no `PARTIAL` closeout. The Medical Afflictions to Quest and Work Bridge is fully integrated across Core, Host Session, Duty Roster System, Survivor Fitness Evaluation, UI Panels, and Host CLI self-test.

## Outcome

Eliminated the partial/blockade state where `AfflictionQuestWorkBridge` and `affliction_bridge_rules.json` existed in pure Core without host integration. Bound the catalog and canonical live affliction IDs to the existing duty assignment and quest availability owners, then surfaced truthful reasons in the existing medical and work/quest panels while preserving single-owner domain boundaries (Rule 5):

- **Pure Domain (`Ashfall.Core`):**
  - `Assets/Ashfall.Core/Medical/AfflictionQuestWorkBridge.cs`: Stateless projection over active medical state; loads `affliction_bridge_rules.json`.
  - Added `QueryQuestGate(questTag, activeAfflictions)`: pure query without side-effects or event invocation for safe UI/preview use.
  - Added `IsRoleExcluded(activeAfflictionIds, roleId, out excludedDuties)`: maps duty roles (`expedition`, `mess`, `night_watch`, `ward`, `hatch_opener`, `intake_sleeper`) to excluded duties.
  - Added `AfflictionBridgeCensus` with `AuthoredWorkModifiersCount`, `AuthoredQuestGatesCount`, `SchemaVersion`, and convenience accessors.
- **Godot Host (`src/`):**
  - `src/Host/MedicalHostSession.cs`: Holds `Bridge` property (`AfflictionQuestWorkBridge`), automatically loads `affliction_bridge_rules.json` upon `Create()`, and provides `LoadBridgeRules()`.
  - `src/Main.Medical.cs`: Implements `GetActiveAfflictionIds(survivorId)` (aggregates pipeline episodes, trauma, dependency, and limb injuries), `IsQuestBlockedForSurvivor()`, and `GetUnlockedQuestsForSurvivor()`.
  - `src/Main.DutyRoster.cs`: Integrates `_dutyRoster.Roster.WorkSpeedMultiplierLookup` factoring in `_medical.Bridge.CalculateWorkModifiers(...)` (clamped to floor 0.1).
  - `src/Main.SurvivorFitness.cs`: Integrates `_medical.Bridge.IsRoleExcluded(...)` into `EvaluateDutyRoleFitness()`, injecting blockers and zeroing recommended hours when duties are excluded.
  - `src/UI/AfflictionsPanel.cs`: Visual projection displaying truthful work speed and quality capacity reductions, duty exclusions, and unlocked medical quests.
  - `src/Host/HostCli.cs` & `src/Main.Application.cs`: Adds `HostCliAction.AfflictionBridgeSelfTest` and CLI flag `--affliction-bridge-selftest` (aliases: `--affliction-bridges-selftest`, `--affliction-quest-work-selftest`).
  - `src/Host/AfflictionBridgeSelfTest.cs`: 12-check comprehensive engine self-test.
- **Verification Manifest:**
  - `docs/ci/SELFTEST_MANIFEST.json`: Registered `--affliction-bridge-selftest` under Host Domains & Save Stores.

## Focused Verification

- `bash scripts/run_test.sh Ashfall.Core.Tests/Medical/Plan143AfflictionBridgeIntegrationTests.cs`: 6/6 PASS (38ms)
- `bash scripts/run_test.sh Ashfall.Core.Tests/Medical/Plan143AfflictionBridgeHostIntegrationTests.cs`: 5/5 PASS (38ms)
- `dotnet build Ashfall.csproj`: 0 Warnings, 0 Errors
- `godot --headless --path . -- --affliction-bridge-selftest`: 12/12 PASS
- `godot --headless --path . -- --affliction-bridges-selftest`: 12/12 PASS
- `python3 scripts/ci/agent-fast-verify.py`: 10/10 PASS

## Invariant and Architecture Preservation

- **Rule 2 (Core engine-free):** Pure Core contains zero Godot/UnityEngine dependencies.
- **Rule 3 (JSON authority):** Uses `Assets/StreamingAssets/Data/affliction_bridge_rules.json` as authoritative rule definitions.
- **Rule 4 & 5 (Determinism & Single Owner):** No parallel save section created. The bridge is a stateless projection over the existing medical authorities (`MedicalPipelineCoordinator`, `RadiationSystem`, `RespiratoryDegenerationSystem`, `ChemicalDependencySystem`, `AmputationSystem`).
