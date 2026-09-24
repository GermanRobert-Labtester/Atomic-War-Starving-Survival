# UNBLOCK INTEGRATION RECORD: Plan 151 / 174 — Working Animals & Companion System

**Execution Date:** 2026-09-24  
**Status:** FULLY INTEGRATED & VERIFIED  
**Package:** `XP-WAVE1-WORKING-ANIMALS-SEAL`  
**System Id:** `companion_animals` / `CompanionAnimalHostSession`  

---

## 1. Problem / Blockade Diagnosis
- `CompanionAnimalSystem` existed in Core, but was unexposed to the player surface in `Main.Companion.cs` (no player assignment, feeding, or census query methods).
- No typed host session `CompanionAnimalHostSession` existed.
- No headless CLI probe existed to verify catalog load, companion adoption, role compatibility, handler exclusivity, guard/pack bonuses, canonical feeding, starvation drift, veterinary care, grief shock, and save round-trips.

---

## 2. Integrated Solution & Seams
1. **Player Surface Methods in `Main.Companion.cs`:**
   - Added `EnsureCompanionSession()`, `AssignCompanion(companionId, survivorId, role)`, `FeedCompanion(companionId, day)`, `RegisterCompanion(companionId, speciesId, tamedDay, name)`, `GetAllCompanions()`, `GetGuardModifierTotal()`, and `GetPackCapacityBonusForSurvivor(survivorId)`.
2. **Host Session:**
   - Created `src/Host/CompanionAnimalHostSession.cs` providing `CompanionCensus`, event handling, commands, factory creation, and save/restore capture.
3. **Dedicated 12-Check CLI Probe:**
   - Created `src/Host/WorkingAnimalsSelfTest.cs` with 12 structured checks:
     - Check 1: Catalog load from `companion_animals.json` without errors.
     - Check 2: Companion registration.
     - Check 3: Unknown species rejection.
     - Check 4: Incompatible role assignment rejection (e.g. Cotton Hare cannot Guard).
     - Check 5: Handler assignment exclusivity enforced.
     - Check 6: Guard rating calculation based on condition, training, and bond.
     - Check 7: Pack capacity bonus calculation.
     - Check 8: Feeding consumption from canonical inventory.
     - Check 9: Hunger drift & starvation health damage when unfed.
     - Check 10: Sickness state transition & veterinary treatment consuming canonical medicine.
     - Check 11: Morale support & grief shock calculation within authored bounds.
     - Check 12: Save/restore round-trip parity on section `companion_animals`.
   - Created `src/Host/HostCli.WorkingAnimals.cs`.
   - Wired `HostCliAction.WorkingAnimalsSelfTest` in `src/Host/HostCli.cs` and `src/Main.Application.cs`.
4. **Targeted xUnit Suite:**
   - Created `Ashfall.Core.Tests/World/Plan151WorkingAnimalsTests.cs` (5 tests, 100% green).
5. **Manifest Registration:**
   - Registered `working_animals_selftest` in `docs/ci/SELFTEST_MANIFEST.json`.

---

## 3. Verification Evidence
- `godot --headless -- --working-animals-selftest` passes **12/12 checks** (Exit 0).
- `bash scripts/run_test.sh Ashfall.Core.Tests/World/Plan151WorkingAnimalsTests.cs` passes **5/5 tests**.
- Zero warnings, zero errors on `dotnet build Ashfall.csproj`.
