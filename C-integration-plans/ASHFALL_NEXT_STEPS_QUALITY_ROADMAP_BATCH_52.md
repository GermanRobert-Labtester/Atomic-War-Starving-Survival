# ASHFALL Quality Roadmap — Batch 52
## Theme: Save/Load Round-Trip Test Coverage (H10, H11)

**Priority:** HIGH (H10, H11 — save data loss risk)
**Risk:** Low — tests only, no production code changes
**Prerequisite:** None (can run in parallel with other batches)

---

## Rationale

The premise of this batch as originally written is **substantially stale**. It claims:
- "NeedsSystem & RadiationSystem have 58 behavior tests but zero save/load round-trip tests" (H10)
- "JournalSystem has save-store integrity tests but zero core behavior tests" (H11)
- "PowerGrid, ShelterAssignment, MedicalWard, Memorial, WastelandMap, CampaignDay lack dedicated round-trip coverage"

Verified against the actual repository, this is **false for 4 of the 5 named "lacks coverage" systems and both H10/H11 items**:

- `Ashfall.Core.Tests/NeedsRadiationSystemTests.cs` already contains a nested `SaveRoundTripTests` class (verified, line ~207) with 3 tests: `SurvivorNeedsState_RoundTrips_MutatedValues`, `SurvivorRadState_RoundTrips_MutatedValues`, `RadiationSystem_RegisteredDoseSurvivesRoundTrip`. **H10 is not "zero round-trip tests" — it already has coverage**, though see the corrected Step 1/2 below for the real remaining gaps.
- `Ashfall.Core.Tests/JournalSystemTests.cs` already exists (contradicting "JournalSystem... zero core behavior tests") with 9 tests covering dedup, empty-key rejection, raw entries, max-entry eviction, codex unlocks, ping/read-state, a full `CaptureRestore_RoundTrips_EntriesKnowledgeAndFlags` test, and `Clear()`/null-restore handling. Its own doc comment states: "H11 hardening: JournalSystem previously had zero tests." **H11 is already resolved**, not a live gap.
- `Ashfall.Core.Tests/Shelter/PowerGridSystemTests.cs` already has `CaptureRestore_RoundTripPreservesState`, `Save_RoundTrip_ChecksumStable`, `Save_TamperedChecksumRejected`, and `Save_EmptyChecksumRejected`.
- `Ashfall.Core.Tests/Shelter/ShelterAssignmentSystemTests.cs` already has the same four-test pattern: `CaptureRestore_RoundTrip`, `Save_RoundTrip_ChecksumStable`, `Save_TamperedChecksumRejected`, `Save_EmptyChecksumRejected`.
- `Ashfall.Core.Tests/Memorial/MemorialSystemTests.cs` already has `CaptureRestore_RoundTrip`.
- `Ashfall.Core.Tests/World/WastelandMapSystemTests.cs` already has `CaptureRestore_RoundTrip`.

This batch is **rewritten** below to (a) close the real, narrower gaps that remain (checksum-envelope tests for Memorial/WastelandMap, which have plain `CaptureRestore_RoundTrip` but not the tamper/empty-checksum pattern used elsewhere; edge cases like empty rosters and multi-survivor batches not yet covered even where a round-trip test exists) and (b) avoid creating duplicate test classes/files that collide with what is already there. **MedicalWardSaveStore** and **CampaignDay** were not verified to have existing round-trip tests as of this review and remain real gaps — see corrected Steps 4 and 7.

No JournalSystem method named `GetEntries()`, `GetEntriesByDay()`, or `GetEntriesByTag()` exists — the real API exposes `Entries` (an `IReadOnlyList<JournalEntry>` property), `EntryCount`, and no tag concept at all (entries are keyed by a `KnowledgeKey` string, not a "tag"). Step 3 as originally written cannot be implemented against the real class and has been rewritten against the verified API.

---

## Step 1 — NeedsSystem Save/Load Round-Trip Tests (gap-fill only)

**Goal:** Close the *remaining* gaps in `NeedsSystem`/`SurvivorNeedsState` round-trip coverage. The existing `SaveRoundTripTests` class in `Ashfall.Core.Tests/NeedsRadiationSystemTests.cs` already covers a single-survivor mutated-value round-trip via direct `SystemTextJsonSerializer` (de)serialization. It does **not** yet cover: a full `NeedsSystem` instance with multiple survivors ticked forward, an empty roster, or a `NeedsSystem`-level `CaptureState()/RestoreState()` call (as opposed to serializing the DTO directly) — confirm whether `NeedsSystem` even exposes `CaptureState()/RestoreState()` methods (verify with `grep -n "CaptureState\|RestoreState" Assets/Ashfall.Core/Survivors/NeedsSystem.cs` before writing tests that assume they exist; if `NeedsSystem` has no roster-level capture and each `SurvivorNeedsState` is captured individually by the host, the tests must be written against that real shape, not an assumed `NeedsSystem.CaptureState()`).

**Implementation:**
Add to the **existing** `Ashfall.Core.Tests/NeedsRadiationSystemTests.cs` file (do not create a new file that duplicates the `SaveRoundTripTests` class name/namespace — confirm the class is `public partial` or add tests as new `[Fact]` methods inside the existing `SaveRoundTripTests` class):
1. **MultiSurvivorRoundTrip:** 3 survivors with distinct hunger/thirst/fatigue/warmth/morale/health/hygiene values, serialize each `SurvivorNeedsState` independently (matching the DTO-level pattern already used), assert all 7 need values match per survivor after deserialize.
2. **CriticalThresholdRoundTrip:** Set `WasHungerCritical = true, WasThirstCritical = true, WasWarmthCritical = true` on a state already at critical need levels, round-trip, verify the critical-flag booleans persist (the existing test already exercises this field but only with `WasHungerCritical = true, WasThirstCritical = false` — add the all-true case).
3. **DeathStateRoundTrip:** Set `IsDead = true, IsAlive = false` (verified fields — there is no separate "cause of death" field on `SurvivorNeedsState`; the original plan's "verify death flag and cause are preserved" cannot be implemented as written since no cause field exists), round-trip, verify `IsDead`/`IsAlive`/`IsAliveState` all persist correctly.
4. **EmptyRosterRoundTrip:** If a roster-level container exists in the host (not in `NeedsSystem` itself per the note above), round-trip an empty collection and assert no exception and zero entries. If no such container exists in Core, mark this test N/A and note where the host-level roster is actually owned (likely `SurvivorsHostSession` — verify before writing).

**Verification:**
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "SaveRoundTripTests"` — all pass (existing 3 + new tests)
- No changes to production code

**Risk / rollback:** Low risk — additive tests only. Rollback: delete the added `[Fact]` methods; no production code touched.

**Done when:** the new tests pass, the roster-level ownership question in point 4 is answered with a verified file/class reference (not assumed), and no duplicate `SaveRoundTripTests` class/namespace collision is introduced.

---

## Step 2 — RadiationSystem Save/Load Round-Trip Tests (gap-fill only)

**Goal:** Close remaining gaps in `RadiationSystem`/`SurvivorRadState` round-trip coverage beyond the existing `SurvivorRadState_RoundTrips_MutatedValues` and `RadiationSystem_RegisteredDoseSurvivesRoundTrip` tests.

**Implementation:**
Add to the existing `SaveRoundTripTests` class in `Ashfall.Core.Tests/NeedsRadiationSystemTests.cs`:
1. **PhaseProgressionRoundTrip:** Verify what "phase" actually means in the real `SurvivorRadState`/`RadiationSystem` — the existing DTO exposes `HasAcuteRadiationSickness`, `HasChronicIllness`, `HasAcuteRadiationSyndrome` as discrete booleans, not a single ordered phase enum (mild→moderate→severe) as the original plan assumed. Rewrite this test against the real boolean-flag shape: apply escalating dose until each flag flips, round-trip after each transition, verify all three booleans persist independently (they are not mutually exclusive in the type as declared).
2. **GearMitigationStateRoundTrip:** Verify gear-mitigation state lives on `SurvivorRadState` (fields like `HasRadResistance`, `RadResistanceHoursRemaining` already exist and are already covered by the existing test) versus on a separate `WornGear`/`Inventory.WornGear` structure per AGENTS.md's H2 note on the `Radiation.WornGear.FromInventory` bridge. If gear state is not part of `SurvivorRadState`'s own capture, this test belongs against `InventoryGearBridgeTests` (already referenced in AGENTS.md as existing) rather than here — confirm before duplicating coverage.
3. **MultiSurvivorDoseRoundTrip:** 5 survivors with different `RadiationDose`/`LifetimeRadiationExposure` values, round-trip each, verify no cross-contamination between instances (a real risk if a shared/static field were accidentally used).
4. **ZeroDoseRoundTrip:** Fresh `SurvivorRadState` with all dose fields at default (0f/false), round-trip, verify no phantom dose appears (guards against a serializer that treats absent JSON properties incorrectly for a struct-like DTO).

**Verification:**
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "SaveRoundTripTests"` — all pass
- H10 fully addressed only after both Step 1 and Step 2 gap-fills land; it was **not** true that H10 was fully unaddressed before this batch

**Risk / rollback:** Low risk — additive tests only, and item 2 above may reveal that gear-mitigation coverage already exists elsewhere, in which case skip it rather than duplicate.

**Done when:** new tests pass, the phase-vs-boolean-flag correction in point 1 is reflected in the actual test names/assertions (not the original plan's ordered "mild→moderate→severe" language, which does not match the type), and the gear-mitigation ownership question in point 2 is resolved before writing a possibly-duplicate test.

---

## Step 3 — JournalSystem Core Behavior Tests

**Goal:** Test `JournalSystem` logic: entry creation, filtering, search, day-stamping, capacity limits.

**Implementation:**
Create `Ashfall.Core.Tests/JournalSystemBehaviorTests.cs`:
1. **AddEntry_AppendsToLog:** Add entry, verify it appears in `GetEntries()`.
## Step 3 — JournalSystem Core Behavior Tests

**Status: this step's premise is false. `Ashfall.Core.Tests/JournalSystemTests.cs` already exists** with 9 tests: `TryDiscover_DeduplicatesPerKnowledgeKey`, `TryDiscover_RejectsEmptyKey`, `TryAddRawEntry_RecordsFreeformText_OncePerKey`, `MaxEntries_EvictsOldest`, `CodexUnlock_RecordsAndFlags`, `MarkReadAndAcknowledgePing_ClearFlags`, `CaptureRestore_RoundTrips_EntriesKnowledgeAndFlags`, `Clear_ResetsEverything`, `RestoreState_HandlesNull`. Its own doc comment reads: "H11 hardening: JournalSystem previously had zero tests." **H11 is already resolved** and this step is retired.

Additionally, the originally-planned API does not exist on `JournalSystem` (verified against `Assets/Ashfall.Core/Journal/JournalSystem.cs`):
- No `GetEntries()` — the real accessor is the `Entries` property (`IReadOnlyList<JournalEntry>`).
- No `GetEntriesByDay(int)` — entries carry a `Day` field but there is no built-in day filter method; a caller would `Entries.Where(e => e.Day == day)` itself.
- No `GetEntriesByTag(...)` and no "tag" concept at all — entries are deduplicated by a `KnowledgeKey` string (e.g. `k_found_radio`), not tagged.
- No separate "capacity eviction" test needed — `MaxEntries_EvictsOldest` in the existing file already covers `JournalSystem.MaxEntries` (= 64) eviction.
- "DuplicateEntry_Handling" already exists in spirit via `TryDiscover_DeduplicatesPerKnowledgeKey` and `TryAddRawEntry_RecordsFreeformText_OncePerKey`, both of which assert the second add returns `null` and the entry count does not grow.

**Remaining real gap (if any):** confirm whether `TryDiscover`/`TryAddRawEntry` are the only two entry-creation paths, and if the host (`src/Main.cs` `SetupJournal`/`SaveJournal`, lines ~1308/~1374) calls a third path not covered by unit tests. If so, add a focused test for that specific host-adjacent path only — do not recreate the 9 tests that already exist.

**Verification:**
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "JournalSystemTests"` — confirm existing 9/9 still pass (regression check, not new work)

**Done when:** it is confirmed (not assumed) that H11 is already resolved by the existing file, and either no further action is taken or a single, narrowly-scoped test is added for a verified host-path gap — never a full duplicate `JournalSystemBehaviorTests.cs` file.

---

## Step 4 — PowerGridSystem Round-Trip Tests

**Status: this step's premise is false. `Ashfall.Core.Tests/Shelter/PowerGridSystemTests.cs` already exists** with `CaptureRestore_RoundTripPreservesState`, `Save_RoundTrip_ChecksumStable`, `Save_TamperedChecksumRejected`, `Save_EmptyChecksumRejected`, plus behavioral tests (`TickDay_NetNegative_DrainsBattery`, `TickDay_OverloadTripsBreakerDeterministic`, `Determinism_SameSeed_IdenticalTickSummary`).

The real `PowerGridSystem` (verified in `Assets/Ashfall.Core/Shelter/PowerGridSystem.cs`) models generation watts, fuel units, and a battery reserve against a fixed list of rooms with breakers — it has no concept of "2 generators" or "generator fuel" as separate per-generator units; fuel is a single pooled `FuelUnits` value, and load is per-room draw, not per-generator. The original plan's "Configure grid with 2 generators, set fuel levels" does not match the real model.

**Remaining real gap:** the existing `TickDay_OverloadTripsBreakerDeterministic` test already exercises sustained overload/brownout over 30 days, but there is no test that captures state *mid-brownout* (i.e., while `IsBrownout` is actively true) and round-trips it — the existing `CaptureRestore_RoundTripPreservesState` test round-trips after a breaker toggle and fuel add, not during an active brownout. Add one focused test:
1. **BrownoutStateRoundTrip:** Force sustained overload until `grid.IsBrownout` is true, `CaptureState()`, restore into a fresh instance, assert the restored grid's computed `IsBrownout`/`NetWatts` match the original at the moment of capture.

**Verification:**
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "PowerGridSystemTests"` — existing 15 tests + 1 new pass

**Done when:** the brownout-mid-state gap is either closed with the one test above, or confirmed already covered and this step is retired with no file created.

---

## Step 5 — ShelterAssignmentSystem Round-Trip Tests

**Status: this step's premise is false. `Ashfall.Core.Tests/Shelter/ShelterAssignmentSystemTests.cs` already exists** with `CaptureRestore_RoundTrip`, `Save_RoundTrip_ChecksumStable`, `Save_TamperedChecksumRejected`, `Save_EmptyChecksumRejected`, `DeDuplicates_SurvivorAssignments_OnNormalize`, plus behavioral tests for assign/unassign/capacity/occupancy.

**Remaining real gap:** the existing `CaptureRestore_RoundTrip` test only covers two assigned survivors in different rooms; it does not explicitly cover an *unassigned* survivor persisting as unassigned (the "no assignment" case) through a round-trip, or a room filled exactly to capacity. Add:
1. **UnassignedSurvivorRoundTrip:** Assign 2 of 4 known survivor IDs, leave 2 unassigned, round-trip, assert `GetAssignmentForSurvivor(...)` returns `null` for the 2 unassigned IDs both before and after restore (guards against an accidental default-assignment on restore).
2. **RoomCapacityRoundTrip:** Fill `room_bunks` (capacity 4, per the existing `MakeGrid` helper) to exactly 4 occupants, round-trip, assert `GetRoomOccupancy("room_bunks") == 4` after restore and that a 5th `Assign` call still correctly fails with `"room_full"` post-restore (guards against capacity being recomputed incorrectly after restore).

**Verification:**
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "ShelterAssignmentSystemTests"` — existing 13 tests + 2 new pass

**Done when:** both new tests pass and no duplicate file is created; extend the existing test class in place.

---

## Step 6 — MemorialSystem and WastelandMapSystem Checksum-Envelope Tests

**Status: this step's premise is partly false.** `Ashfall.Core.Tests/Memorial/MemorialSystemTests.cs` already has `CaptureRestore_RoundTrip` (verified), and `Ashfall.Core.Tests/World/WastelandMapSystemTests.cs` already has `CaptureRestore_RoundTrip` (verified) plus `PlanRoute_DeterministicForSameState`. Both systems, however, verifiably **lack** the checksum-envelope tests (`Save_RoundTrip_ChecksumStable`, `Save_TamperedChecksumRejected`, `Save_EmptyChecksumRejected`) that PowerGrid, ShelterAssignment, and MedicalWard all already have — **this is the real, narrower gap**, not a missing plain round-trip.

Two corrections to the original plan's field assumptions (verified against `Assets/Ashfall.Core/Memorial/MemorialSystem.cs` and `Assets/Ashfall.Core/World/WastelandMapSystem.cs`):
- `MemorialEntry` has an `Epitaph` field, not "eulogy" — use the real field name in test names/assertions.
- `WastelandMapSystem` is node/route based (`Discover(nodeId)`, `IsDiscovered(nodeId)`), not a grid of "cells" with "fog of war" coverage percentages — there is no "FullyExploredRoundTrip over all cells" concept; the closest real equivalent is discovering every `MapNode` in the fixed node list.

**Implementation:**
Extend the two **existing** test files (do not create `MemorialSystemSaveTests.cs` / `WastelandMapSaveTests.cs` — these names would sit alongside `MemorialSystemTests.cs`/`WastelandMapSystemTests.cs` and fragment coverage of the same class):
1. **Memorial — `Save_RoundTrip_ChecksumStable`:** build a `MemorialSave` envelope (confirm the real DTO/codec name via `grep -n "class MemorialSave\|MemorialSaveCodec" Assets/Ashfall.Core/Memorial/*.cs` before writing — do not assume it matches the `PowerGridSave`/`PowerGridSaveCodec` naming pattern without checking), encode/decode, assert checksum stability.
2. **Memorial — `Save_TamperedChecksumRejected`** and **`Save_EmptyChecksumRejected`:** mirror the pattern already used in `PowerGridSystemTests.cs`/`ShelterAssignmentSystemTests.cs`.
3. **Memorial — `EpitaphTextRoundTrip`:** verify a long `Epitaph` string (e.g. 500+ characters, including punctuation) survives serialization intact — this is a legitimate remaining edge case even though the field name in the original plan ("eulogy") was wrong.
4. **WastelandMap — same three checksum tests**, using the verified real save/codec type name.
5. **WastelandMap — `AllNodesDiscoveredRoundTrip`:** discover every node in the fixture's node list (not "cells"), round-trip, assert every node's `IsDiscovered` remains true and `PlanRoute` results are unchanged.

**Verification:**
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "MemorialSystemTests|WastelandMapSystemTests"` — all pass

**Risk / rollback:** Low risk, additive only. If no `MemorialSave`/`WastelandMapSave` checksum-envelope codec exists yet at the host layer (unlike PowerGrid/ShelterAssignment/MedicalWard), this step becomes a **production-code gap**, not just a test gap — flag that finding explicitly rather than writing a test against a codec that doesn't exist. Verify via `grep -rn "MemorialSave\|WastelandMapSave" src/Host/ Assets/Ashfall.Core/Memorial/ Assets/Ashfall.Core/World/` before starting.

**Done when:** either the checksum tests are added against a confirmed-real codec, or the absence of a checksum-envelope codec for these two systems is reported as a new, separate finding (a real gap this plan was right to look for, even though it named the wrong systems as missing basic round-trip coverage).

---

## Step 7 — CampaignDayCoordinator: Confirm No Round-Trip Test Applies

**Status: this step did not exist in the original plan under this name; CampaignDay was listed as lacking "dedicated round-trip coverage" in the Rationale, but this is not the right framing.** `Ashfall.Core.Tests/Campaign/CampaignDayCoordinatorTests.cs` already exists and covers what the real class actually does: `Register_ThrowsOnDuplicateId`, `Register_SortsOwnersByIdDeterministic`, `Advance_ReturnsNullWhenAlreadyAdvancing`, `Advance_TicksAllOwnersExactlyOnceAndInOrder`, `Advance_CollectsTypedEvents`, `Advance_IsolatesOwnerFailures`, `Advance_CallsPersistenceBeforeReturning`, `Advance_RaisesOnDayAdvanced`, `Advance_AllEvents_IteratesEveryEventFromEveryOwner`.

Verified against `Assets/Ashfall.Core/Campaign/CampaignDayCoordinator.cs`: this class is a **pure orchestrator** — it registers per-domain day-tick owners and ticks them in a deterministic order, raising typed events and calling a persistence callback. It has **no `CaptureState()`/`RestoreState()` of its own** because it owns no state to save; each domain owner (Expeditions, Economy, etc.) captures its own state independently, already covered by that owner's own tests. A "round-trip test" for `CampaignDayCoordinator` is not a meaningful concept — there is nothing for it to round-trip.

**Done when:** this is confirmed against the source (as done above) and the item is retired from the "coverage gap" list rather than assigned a test that cannot meaningfully exist.

---

## Summary

| Step | System | Real status before this batch | Work in this corrected plan |
|------|--------|-------------------------------|------------------------------|
| 1 | NeedsSystem | Already has 1 round-trip test (`SurvivorNeedsState_RoundTrips_MutatedValues`) | Add ~3 gap-fill tests (multi-survivor, all-critical-flags, death-state) inside existing file |
| 2 | RadiationSystem | Already has 2 round-trip tests | Add ~3–4 gap-fill tests (boolean-flag transitions, multi-survivor, zero-dose); gear-mitigation test only if not already covered by `InventoryGearBridgeTests` |
| 3 | JournalSystem | **Already fully resolved** (9 tests incl. round-trip) | None, or at most 1 narrowly-scoped host-path test |
| 4 | PowerGridSystem | **Already has round-trip + checksum tests** (15 tests) | Add 1 gap-fill test (mid-brownout capture) |
| 5 | ShelterAssignmentSystem | **Already has round-trip + checksum tests** (13 tests) | Add 2 gap-fill tests (unassigned survivor, room-at-capacity) |
| 6 | MemorialSystem + WastelandMapSystem | Have plain round-trip only, **missing checksum-envelope tests** | Add checksum-envelope tests (3 each) + 1 corrected-terminology edge case each, contingent on confirming the save codec exists |
| 7 | CampaignDayCoordinator | **Already has 9 orchestration tests; no save state exists to round-trip** | None — retire this item from the "coverage gap" list |

**Corrected end state:** this batch, as originally written, claimed 33 new tests would resolve H10/H11 and close 5 coverage gaps that mostly do not exist. The actual remaining work is on the order of **10–13 gap-fill tests** across NeedsSystem/RadiationSystem/PowerGrid/ShelterAssignment, plus **up to 8 checksum-envelope tests** for Memorial/WastelandMap *if and only if* a save codec for those two systems is confirmed to exist (Step 6) — otherwise that becomes a production-code finding, not a test-writing task. H10 and H11 were largely already resolved before this batch began; do not report them as newly "resolved" by this batch without noting they were already substantially covered.


## Review Notes (Corrected)

This batch plan was adversarially reviewed against the real repository at `/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War`. The following issues were found and fixed in place:

1. **False premise for H10.** The plan claimed NeedsSystem/RadiationSystem have "zero save/load round-trip tests." Verified: `Ashfall.Core.Tests/NeedsRadiationSystemTests.cs` already contains a `SaveRoundTripTests` class with 3 round-trip tests. Steps 1–2 rewritten as gap-fill work against the existing class, not new files.
2. **False premise for H11.** The plan claimed JournalSystem has "zero core behavior tests." Verified: `Ashfall.Core.Tests/JournalSystemTests.cs` already exists with 9 tests including a full capture/restore round-trip, and its own doc comment states it was written specifically to resolve H11. Step 3 retired.
3. **Nonexistent API referenced.** Step 3 assumed `JournalSystem.GetEntries()`, `GetEntriesByDay()`, `GetEntriesByTag()`, and a "tag" concept. None exist — verified the real API exposes `Entries` (property), `EntryCount`, and dedup by `KnowledgeKey` string, no tags. Step 3 rewritten against the real API surface.
4. **False "lacks coverage" claim for PowerGridSystem.** Verified `Ashfall.Core.Tests/Shelter/PowerGridSystemTests.cs` already has `CaptureRestore_RoundTripPreservesState` plus 3 checksum-envelope tests. Step 4 narrowed to one real gap (mid-brownout capture) instead of a full new test file.
5. **False "lacks coverage" claim for ShelterAssignmentSystem.** Verified `Ashfall.Core.Tests/Shelter/ShelterAssignmentSystemTests.cs` already has round-trip + 3 checksum tests. Step 5 narrowed to 2 real gaps (unassigned-survivor persistence, room-at-capacity persistence).
6. **Wrong model assumed for PowerGridSystem.** The plan's Step 4 implementation described "2 generators" and "generator fuel levels." Verified the real model is a single pooled `FuelUnits` value against per-room draw with breakers — there is no per-generator concept. Corrected in the rewritten step.
7. **Partially false claim for MemorialSystem/WastelandMapSystem.** The plan said these "lack dedicated round-trip coverage." Verified both already have a `CaptureRestore_RoundTrip` test. The real, narrower gap is that neither has the checksum-envelope tests (`Save_RoundTrip_ChecksumStable`/`Save_TamperedChecksumRejected`/`Save_EmptyChecksumRejected`) that PowerGrid/ShelterAssignment/MedicalWard have. Step 6 rewritten to target that actual gap, contingent on confirming a save codec exists for these two systems at all (flagged as a possible production-code finding, not just a test gap).
8. **Wrong field name.** The plan referenced a memorial "eulogy" field. Verified the real field is `Epitaph` on `MemorialEntry`. Corrected throughout Step 6.
9. **Wrong data model for WastelandMapSystem.** The plan described "fog-of-war," "map cells," and "FullyExploredRoundTrip over all cells." Verified the real system is node/route based (`MapNode`, `Discover(nodeId)`, `IsDiscovered(nodeId)`) with a fixed set of named locations, not a grid of cells with fog coverage. Corrected in Step 6.
10. **Unverified claim for MedicalWard.** The Rationale listed MedicalWard among systems lacking round-trip coverage. Verified `Ashfall.Core.Tests/Medical/MedicalWardSystemTests.cs` already has full round-trip + checksum coverage (`CaptureRestore_RoundTrip`, `Save_RoundTrip_ChecksumStable`, `Save_TamperedChecksumRejected`, `Save_EmptyChecksumRejected`). Removed from the corrected plan's remaining-work list entirely.
11. **Conceptually invalid target for CampaignDay.** The Rationale listed CampaignDay as lacking round-trip coverage. Verified `CampaignDayCoordinator` is a pure stateless orchestrator with no `CaptureState()/RestoreState()` of its own — it has nothing to round-trip. New Step 7 added explaining why this item should be retired rather than assigned a test, backed by the coordinator's own already-existing 9-test suite covering what it actually does (ordering, isolation, event collection).
12. **Vague/inflated Done-when criteria tightened.** Original steps used bare test counts ("5/5 pass", "8/8 pass") as the only completion signal without accounting for tests that may already exist or duplicate coverage. Every step's Done-when now requires confirming against the real file/class before counting new tests, and explicitly forbids creating duplicate test files/classes for systems that already have coverage.
13. **Missing risk/rollback notes.** The original plan rated overall risk "Low — tests only" but gave no guidance on what to do if a step's assumed gap turns out to be a real production-code issue (e.g., Step 6's checksum-codec question). Added explicit risk/rollback language distinguishing "add a test" work from "flag a production gap" work, since the latter is not zero-risk and needs its own review.
14. **Corrected summary and end-state claim.** The original "33 new round-trip tests, H10 and H11 resolved" summary is replaced with an accurate accounting: most of the claimed gap was already closed before this batch started, and the real remaining work is on the order of 10–13 gap-fill tests plus a conditional checksum-envelope addition for 2 systems.
