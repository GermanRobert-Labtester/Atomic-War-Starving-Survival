# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**

# Feature / Task Plan: Flag cleanup — Endgame zero-caller/dead fields, Verdict reckoning profile+persistence, coordinator inventory rollback, Drowned Coast F4

> **STATUS: APPROVED BY USER**

> **Resolution (2026-10-01):** all four flags closed. See the appended
> "Resolution" section at the bottom for the exact edits and verification.

## 1. Goal & Outcome
- **Goal:** close four open flags carried in `.ai/state.md`:
  1. **Endgame** — wire the zero-caller `EndgameSystem.EvaluateEndingWithProfile`
     and persist/populate the dead `ChapterRecord.sealedDay` / `profileId` fields.
  2. **Verdict** — wire `ReckoningSystem.ConfigureFromProfile` through
     `VerdictHostSession` (chapter-profile timing + `reckoning_offset`) and
     persist `ReckoningOffset` in the `verdict` save section (v4→v5 + frozen
     v4 migration).
  3. **Coordinator** — make a fail-closed day-advance retry roll back inventory
     (and the inventory-mutating producers) via `IPreDaySnapshotRestore`, so a
     retry cannot double-consume rations or lose producer output.
  4. **Drowned Coast F4** — correct the stale "two naval owners" row now that
     `ExpeditionHostSession._naval` is the single owner.
- **Non-Goals:** no new save section beyond the `verdict` field bump; no balance
  change; no UI route; no Unity; no full-suite run.

## 2. Claimed Paths & Affected Files
- **Endgame Core:** `Assets/Ashfall.Core/Endgame/EndgameSystem.cs`
- **Endgame host:** `src/Host/EndgameHostSession.cs`, `src/Main.Endgame.cs`
- **Verdict Core:** `Assets/Ashfall.Core/Verdict/VerdictSave.cs`
- **Verdict host:** `src/Host/VerdictHostSession.cs`, `src/Main.Verdict.cs`
- **Coordinator/day owners:** `src/Main.CampaignOwners.cs`,
  `src/Host/GreenhouseHostSession.cs` (add `RestoreSave` if missing)
- **Docs:** `docs/expansions/expansion_drowned_coast_plan.md`,
  `docs/architecture/TRIAD_GATE_AND_SAVE_OWNERSHIP.md`, `KNOWN_DEBT.md`,
  `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, `.ai/state.md`
- **Tests:** focused additions under `Ashfall.Core.Tests/` only if a contract
  is not already covered.

## 3. Pre-flight Checks
- [x] Premise verified in source (see evidence below).
- [x] No equivalent existing system found.
- [x] `EndgameSystem.cs`, `VerdictHostSession.cs`, `VerdictSave.cs`,
      `expansion_drowned_coast_plan.md` are clean in the worktree.

## 4. Implementation Steps
1. **B** — Drowned Coast F4 row + §2.1 bullet → single naval owner (`RESOLVED`).
2. **C** — `ContinueChapter` sets `sealedDay`/`profileId`; `CaptureState`/
   `RestoreState` clone them; add `EndgameHostSession.EvaluateEndingWithProfile`
   and call it in `Main.Endgame.CheckAndTriggerEndgame` (project → log → commit).
3. **D** — `VerdictHostSession.ConfigureFromProfile(profile)` calls
   `Reckoning.ConfigureFromProfile` + sets `ReckoningOffset`; `Main.Verdict`
   invokes it where the profile is resolved; persist `reckoningOffset` in
   `VerdictSave` (+v5 frozen v4 migration) and restore it.
4. **A** — add a central `InventoryDayOwner` (`IPreDaySnapshotRestore`, phase 0,
   restores last) plus `IPreDaySnapshotRestore` on the inventory-mutating owners
   (`StartingLevelRations`, `CraftingProduction`, `GreenhouseFoundry`,
   `Aquaponics`).
5. Focused verification after each.

## 5. Verification
- [x] `dotnet build Ashfall.csproj --no-restore` — 0 errors / 0 new warnings.
- [x] `--verdict-selftest` PASS; `--year-two-chapter-selftest` 7/7;
      `--7-day-smoke-selftest` 10/10.
- [x] xUnit: `EndgameSystemTests` 9/9, `YearTwoPlayOnTests` 9/9 (extended with
      chapter-field round-trip), `ChapterProfileTests` 17/17,
      `VerdictSaveMigrationTests` 14/14 (extended v4→v5 + offset round-trip),
      `VerdictSystemTests` 54/54, `CampaignDayCoordinatorTests` 20/20, and the
      new `CampaignDayCoordinatorSourceGateTests.SourceGate_InventoryMutatingOwners_RollBackOnRetry`.
- [x] `git diff --check` clean; no full suite.

## Resolution (2026-10-01)

1. **Endgame** — `EndgameSystem.ContinueChapter` now stamps `ChapterRecord.sealedDay`
   (epilogue sealed day, falling back to reading day) and `profileId`;
   `CaptureState`/`RestoreState` clone both; `ChroniclePanel` renders them;
   `EndgameHostSession.EvaluateEndingWithProfile` pass-through added and called
   by `Main.Endgame.CheckAndTriggerEndgame` to journal the projected closure
   before the single authority commits it.
2. **Verdict** — `VerdictHostSession.ConfigureFromProfile` wires
   `Reckoning.ConfigureFromProfile` + `ReckoningOffset`, invoked from
   `Main.SetupVerdict`/`SetupEndgame`; `ReckoningOffset` persists in the
   `verdict` section (`VerdictSave` v5 + frozen `VerdictSaveV4` migration).
3. **Coordinator** — new `inventory_custody` day owner snapshots/restores the
   whole inventory container; the inventory-mutating owners
   (`StartingLevelRations`, `CraftingProduction`, `GreenhouseFoundry`,
   `Aquaponics`) now implement `IPreDaySnapshotRestore`, so a fail-closed retry
   cannot double-consume rations or lose producer output. Pinned by a source gate.
4. **Drowned Coast F4** — row + §2.1 bullet corrected to the single naval owner
   (`ExpeditionHostSession._naval`); `Main.EnsureNavalSystem`/partial deleted.
