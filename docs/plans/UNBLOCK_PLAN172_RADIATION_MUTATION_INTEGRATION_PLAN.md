# UNBLOCK INTEGRATION RECORD: Plan 172 — Radiation Mutation & Genetic Instability

**Execution Date:** 2026-09-24  
**Status:** FULLY INTEGRATED & VERIFIED  
**Package:** `XP-WAVE1-RADIATION-MUTATION-SEAL`  
**System Id:** `mutation_tree` / `RadiationMutationHostSession`  

---

## 1. Problem / Blockade Diagnosis
- `MutationSystem` was implemented in pure Core (`Assets/Ashfall.Core/Medical/MutationSystem.cs`), but had no host runtime bridge and no active event listening.
- Radiation exposures (acute and ambient doses) from `RadiationSystem` and `SurvivorsHostSession` were never fed into `AddRadiationExposure`, rendering genetic instability and mutation progression unreachable.
- No daily mutation tick was invoked in campaign day owners.
- No headless CLI probe existed to verify catalog load, instability accumulation, mutation acquisition, gene therapy, and save round-trips.

---

## 2. Integrated Solution & Seams
1. **Event Seam Wired:**
   - In `src/Host/SurvivorsHostSession.cs`: Added `OnSurvivorExposed` event, hooking into `RadiationSystem.onExposed` delegate and acute rad dose application.
   - In `src/Main.Survivors.cs`: Connected `_survivors.OnSurvivorExposed` to call `ApplyRadiationExposure(survivorId, delta, _simDay)`.
2. **Campaign Lifecycle Bound:**
   - In `src/Main.Plans178_181.cs`: Added `TickMutations(day, events)`, `EnsureMutationSession()`, and gene therapy methods.
   - In `src/Main.CampaignOwners.cs`: Added `_m.TickMutations(day, events)` to `MedicalDiseaseDayOwner.TickDay`.
3. **Host Session & CLI Gate:**
   - Created `src/Host/RadiationMutationHostSession.cs` providing typed census, event notifications, and clinical gene therapy handling.
   - Created `src/Host/RadiationMutationSelfTest.cs` with 12 rigorous checks.
   - Created `src/Host/HostCli.RadiationMutation.cs` and wired `--radiation-mutation-selftest` in `src/Host/HostCli.cs` and `src/Main.Application.cs`.
4. **Targeted xUnit Suite:**
   - Created `Ashfall.Core.Tests/Medical/Plan172RadiationMutationTests.cs` (10 tests, 100% green).
5. **Manifest Registration:**
   - Registered `radiation_mutation_selftest` in `docs/ci/SELFTEST_MANIFEST.json`.

---

## 3. Verification Evidence
- `godot --headless -- --radiation-mutation-selftest` passes **12/12 checks** (Exit 0).
- `bash scripts/run_test.sh Ashfall.Core.Tests/Medical/Plan172RadiationMutationTests.cs` passes **10/10 tests**.
- Zero warnings, zero errors on `dotnet build Ashfall.csproj`.
