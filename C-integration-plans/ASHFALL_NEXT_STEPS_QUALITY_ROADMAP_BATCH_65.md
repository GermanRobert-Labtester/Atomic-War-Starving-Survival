# ASHFALL — Quality Roadmap Batch 65

## Theme: Defensive Save Validation — Pre-Write Integrity Checks & Corruption Recovery

| Field | Value |
|-------|-------|
| **Priority** | HIGH |
| **Risk** | Low-Medium — additive safety checks; no existing behavior removed |
| **Category** | Data Integrity / Player Safety |
| **Layer** | `Assets/Ashfall.Core/` (validation logic) + `src/` (Godot host save pipeline) |
| **Depends on** | Existing checksummed envelope system (already in place for 25 save stores — see Corrected Count note below) |
| **Blocks** | Cloud save sync, ironman mode, save export/import |

---

## Problem Statement

The current save system has checksummed envelopes and basic load-time validation, but lacks defense-in-depth against corruption:

1. **No atomic writes** — if the process crashes or power is lost mid-write, the save file is truncated/corrupted with no recovery path. Confirmed by direct inspection of `src/Host/NarrativeSaveStore.cs`'s `TrySave`: it calls `System.IO.File.WriteAllText(path, ...)` directly, with no temp-file/rename step and no fsync. Two *non-save-store* files elsewhere in the host (`src/Audio/AudioSettings.cs:108-110`, `src/Settings/UserSettings.cs:154-158`) already implement a write-to-`.tmp`-then-rename pattern for their own settings files — so the atomic-write idiom is not new to this codebase, it simply hasn't been applied to any of the actual save stores yet. Worth reusing/matching that existing style rather than inventing a third pattern.
2. **No backup rotation** — a single corrupted write destroys the only copy of player progress. Confirmed: no `.bak`, backup-rotation, or `SaveBackupRotator`-equivalent code exists anywhere in `src/` or `Assets/Ashfall.Core/` (verified by grep for `bak\.1|BackupRotat|WriteAtomic|AtomicFileWriter` — zero hits outside the two `.tmp` sites noted above).
3. **No pre-write validation** — `CaptureState()` could produce logically invalid state (null required fields, negative collection lengths) that passes checksum verification but breaks on next load.
4. **No post-load deep validation** — checksum proves the file wasn't tampered with, but doesn't prove the state is self-consistent (e.g., a survivor referenced by ID actually exists in the survivor list).
5. **No recovery pipeline** — if the primary save is corrupt, the player loses everything. There is no fallback chain beyond the legacy bare-state path (confirmed present in `NarrativeSaveStore.TryLoad` and its siblings per AGENTS.md's Save/Load section).
6. **No diagnostic tooling** — no way to inspect save health outside of attempting a full game load.

A single interrupted write can cost a player dozens of hours of progress. This roadmap adds layered defenses that make total save loss nearly impossible.

**⚠️ Corrected count:** the original plan's table header and later text say "22 save stores." Direct enumeration (`find src/Host -iname "*SaveStore.cs"`) finds **25** distinct save store files: `DoseLedgerSaveStore`, `DutyRosterSaveStore`, `ExpansionHubSaveStore`, `HoldfastSaveStore`, `CaravanSaveStore`, `CombatSaveStore`, `CraftingSaveStore`, `EconomySaveStore`, `ExpeditionSaveStore`, `HoldfastTradeSaveStore`, `InventorySaveStore`, `MaritimeSaveStore`, `MedicalSaveStore`, `MusterSaveStore`, `NarrativeSaveStore`, `PhantomMemorySaveStore`, `Phase0SaveStore`, `RadioSaveStore`, `SurvivorsSaveStore`, `VerdictSaveStore`, `WorldSaveStore`, `DailyBriefingSaveStore`, `PowerGridSaveStore`, `MemorialSaveStore`, `MedicalWardSaveStore`. Every "22" reference below is corrected to "25." This matters concretely for Step 1 (rollout scope) and Step 3 (which 5 stores get annotated first — the original plan's "top 5" selection should be re-evaluated against the real list of 25, not 22).

**Also note:** `Assets/Ashfall.Core/Ports.cs`'s current `IFileIO` interface only has `DirectoryExists`, `FileExists`, `ReadAllText`, `WriteAllText`, `Combine` — it has **no** `FlushToDisk` or `MoveReplace` method today. Step 1 below is correct to propose adding them, but implementers should know this is a real interface change to a Core port (touching every host that implements `IFileIO`), not a trivial addition — see Step 1's added risk note.

---

## Step 1 — Add Atomic Write Pattern

### Goal

Ensure that a failed or interrupted write never corrupts the existing save file. The player always has either the old valid save or the new valid save — never a partial write.

### Implementation

- Create `Assets/Ashfall.Core/Save/AtomicFileWriter.cs`:
  ```csharp
  public static class AtomicFileWriter
  {
      /// <summary>
      /// Writes content to a temporary file, validates it, then atomically
      /// renames it to the target path. The old file is only replaced after
      /// the new file is fully written and flushed to disk.
      /// </summary>
      public static void WriteAtomic(string targetPath, string content, IFileIO fileIO)
      {
          string tmpPath = targetPath + ".tmp";
          fileIO.WriteAllText(tmpPath, content);
          fileIO.FlushToDisk(tmpPath);          // fsync equivalent
          // Validate the temp file is readable and non-empty
          string readBack = fileIO.ReadAllText(tmpPath);
          if (string.IsNullOrEmpty(readBack))
              throw new SaveCorruptionException("Write verification failed: empty readback");
          fileIO.MoveReplace(tmpPath, targetPath);  // atomic rename
      }
  }
  ```
- Add `FlushToDisk` and `MoveReplace` to `IFileIO` interface (with default implementations for existing adapters). **This is a breaking change to a Core port** (`Assets/Ashfall.Core/Ports.cs`) — every existing `IFileIO` implementation must be updated in the same commit or the build fails. **Correction on review:** the production implementer is `FileSystemIO` (`Assets/Ashfall.Core/HostDefaults.cs:13`), which lives directly inside `Ashfall.Core` as Core's own default BCL-backed adapter — it is not a separate "Godot host adapter"; the Godot host consumes this same Core class as-is rather than maintaining its own. This narrows the update surface to: `FileSystemIO` itself, plus any standalone test-double/mock `IFileIO` implementations in `Ashfall.Core.Tests` (search for `: IFileIO` to find them all — none were found maintaining a separate implementation as of this review, but confirm at implementation time). Update every implementer atomically rather than adding the interface members first and fixing call sites later. If C# default interface implementations are acceptable for this target framework (`net8.0` on Core/Godot host per the actual build output, `net9.0` for tests — confirm this doesn't create a version conflict; note AGENTS.md's stack table claims Core targets `netstandard2.1`, but `dotnet build` output for this review showed `Ashfall.Core -> .../bin/Debug/net8.0/Ashfall.Core.dll`, so verify the actual current TFM before relying on the stack table), consider adding `FlushToDisk`/`MoveReplace` as default-implemented interface members instead of a breaking change, to avoid touching every implementer.
- Update all 25 save stores' `TrySave` methods to use `AtomicFileWriter.WriteAtomic` instead of direct `WriteAllText`. Confirmed current pattern via `NarrativeSaveStore.TrySave`: direct `System.IO.File.WriteAllText(path, ...)` inside a try/catch, no temp file. Migrate one store, verify save/load round-trip for that store specifically, then proceed to the next — do not batch-edit all 25 in one pass, since a subtle bug in the atomic-write helper would otherwise corrupt 25 stores' save paths simultaneously instead of one.
- If `MoveReplace` fails (permissions, cross-device), fall back to write-rename-delete pattern.
- Clean up orphaned `.tmp` files on startup (they indicate a previous interrupted write).

### Verification

- Unit test: simulate crash after `.tmp` write but before rename → original save intact.
- Unit test: successful write → `.tmp` file does not persist.
- Unit test: empty write detected → exception thrown, original save untouched.

```
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~AtomicFileWriter"
```

### Done when

All 25 save stores write atomically; no scenario exists where a crash leaves a truncated/empty primary save file. `IFileIO` implementers (Godot host adapter + any Core test doubles) all compile against the updated interface.

---

## Step 2 — Add Save Backup Rotation

### Goal

Maintain a rolling history of the last 3 valid saves so that even if the latest save is corrupted (by a bug in `CaptureState`, not by a write failure), the player can recover from a recent checkpoint.

### Implementation

- Create `Assets/Ashfall.Core/Save/SaveBackupRotator.cs`:
  ```csharp
  public sealed class SaveBackupRotator
  {
      private readonly IFileIO _fileIO;
      private readonly int _maxBackups;  // default 3

      /// <summary>
      /// Rotates existing saves before a new write:
      /// current → .bak.1, .bak.1 → .bak.2, .bak.2 → .bak.3, .bak.3 deleted.
      /// </summary>
      public void RotateBeforeWrite(string savePath) { ... }

      /// <summary>
      /// Returns backup paths in priority order (newest first).
      /// </summary>
      public IReadOnlyList<string> GetBackupPaths(string savePath) { ... }
  }
  ```
- Integrate into the save pipeline: rotate BEFORE `AtomicFileWriter.WriteAtomic`.
- Rotation is itself atomic: rename operations only, no data copying.
- Backup files use the same checksummed envelope format — they are full valid saves.
- Add a configurable `MaxBackups` (default 3, minimum 1, maximum 10) in save settings.
- On first load after upgrade, existing saves without backups are grandfathered (no backups created retroactively, but next save will start the rotation).

### Verification

- Unit test: after 5 saves, exactly 3 `.bak.N` files exist with correct content.
- Unit test: rotation does not corrupt any backup file.
- Unit test: `GetBackupPaths` returns only files that actually exist.
- Disk space: 3 backups × ~50KB typical save ≈ 150KB — negligible.

```
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~SaveBackupRotator"
```

### Done when

Every successful save creates a backup rotation; `GetBackupPaths` returns valid recovery candidates; oldest backup is pruned at the configured limit.

---

## Step 3 — Add Pre-Write State Validation

### Goal

Catch logically invalid state BEFORE it is serialized and written to disk. A save that passes pre-write validation is guaranteed to be loadable and self-consistent.

### Implementation

- Create `Assets/Ashfall.Core/Save/SaveStateValidator.cs`:
  ```csharp
  public sealed class SaveStateValidator
  {
      /// <summary>
      /// Validates captured state before serialization.
      /// Returns a list of validation errors (empty = valid).
      /// </summary>
      public IReadOnlyList<string> ValidatePreWrite(SystemState state) { ... }
  }
  ```
- Validation rules (per system state DTO):
  - Required fields are non-null (marked with `[SaveRequired]` attribute).
  - Collection fields have non-negative `Count` / `Length`.
  - Numeric fields are within declared `[SaveRange(min, max)]` bounds.
  - ID reference fields match known ID prefixes (`item_`, `loc_`, `faction_`, etc.).
  - Day counters are non-negative and ≤ current sim day.
  - Enum fields contain valid enum values (not out-of-range cast integers).
- Add `[SaveRequired]`, `[SaveRange]`, `[SaveIdRef(prefix)]` attributes to `Assets/Ashfall.Core/Save/`.
- Annotate existing state DTOs incrementally. **The original plan says "start with the 5 highest-traffic save stores" without defining "highest-traffic" or naming them — this is unmeasurable as written.** Pick a concrete, checkable criterion instead: e.g. the 5 stores whose systems tick every in-game day (candidates to verify against the actual tick registration in `Main.cs`: `SurvivorsSaveStore`, `InventorySaveStore`, `EconomySaveStore`, `WorldSaveStore`, `MedicalSaveStore` — confirm these five are actually the ones ticked/saved most frequently by reading `Main.cs`'s tick-dispatch code before committing to this list; do not assume it without checking). **Additional caveat found on review:** `Main.cs`'s `CommitAdvance()` (the day-advance entry point, ~line 6374) only calls `SaveAll()` conditionally, gated on `UserSettingsStore.Current.AutoSaveOnDay` — so "ticks every in-game day" (a `_core.TickDay()` / Core-simulation concern) and "saves every in-game day" (a `SaveAll()` / persistence concern, off by default unless the player has auto-save enabled) are two different claims. Verify each of the 5 candidate stores against *simulation* tick frequency (does the underlying system's state change every day, regardless of whether autosave is on), not save frequency, since save frequency depends on a player setting this step has no control over. This distinction matters for the audit doc: "highest write frequency" and "highest state-churn frequency" can pick different stores when autosave is off. Whatever 5 are chosen, name them explicitly in the audit doc so "done when" is verifiable.

- If validation fails, the save is ABORTED (old save preserved via atomic write pattern — depends on Step 1 landing first for this specific guarantee to hold) and an error is logged with the specific validation failures.

### Verification

- Unit test: state with null required field → validation fails with specific message.
- Unit test: state with negative collection length → validation fails.
- Unit test: valid state → validation passes, save proceeds.
- Integration: corrupt a field in `CaptureState` output → save aborted, old save intact.

```
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~SaveStateValidator"
```

### Done when

The 5 specifically-named save stores chosen above have annotated DTOs (not merely "the top 5" left undefined); pre-write validation catches ≥10 categories of invalid state; aborted saves do not corrupt existing files.

---

## Step 4 — Add Post-Load Deep Validation

### ⚠️ Corrected assumption

The original design assumes a single monolithic `GameState fullState` object that `SaveConsistencyChecker` validates in one pass, and that `SaveRecoveryPipeline` (Step 5) loads from one primary save file. **That does not match the actual save architecture.** Confirmed by direct inspection (`NarrativeSaveStore.cs` and the full list of 25 `*SaveStore.cs` files under `src/Host/`): there is no single "the save file" — there are 25 independent save stores, each owning its own file (e.g. `narrative_save.json`), its own envelope type (e.g. `NarrativeHostSave`), and its own checksum. `Main.cs`'s `SaveAll()` (line ~6227) calls all ~30 `SaveXxx()` methods (25 of which correspond to these stores) independently; there is no aggregation point that produces one `GameState`.

This means:
- Cross-system consistency checks (e.g. "does this survivor ID referenced by the Economy ledger exist in the Survivors roster") require reading and correlating output from *multiple independently-loaded stores* (at minimum `SurvivorsSaveStore` + `EconomySaveStore` + `InventorySaveStore` + whichever store owns the reference being checked) — not one `GameState` blob.
- "Corruption recovery" (Step 5) is not a single primary-vs-backup decision across one file; it is a **per-store** decision. If `NarrativeSaveStore`'s file is corrupt but `SurvivorsSaveStore`'s is fine, does the game refuse to load entirely, or load survivors and reset narrative state? The original plan never addresses this and it materially changes the design. This must be decided explicitly before implementation — recommended default: per-store recovery (each store attempts its own primary→backup→legacy chain independently), with a game-level report showing which stores recovered from where, rather than an all-or-nothing "the save" recovery.
- `SaveConsistencyChecker`'s signature below is corrected to operate on the aggregate of per-store loaded states the host already holds after calling each store's `TryLoad()`, not on a fictional `GameState` type.

### Goal

After loading and deserializing saves from all 25 stores, verify that the aggregate restored state is self-consistent before the game resumes. Catch corruption that passes each store's individual checksum verification but represents impossible cross-store game states.

### Implementation

- Create `Assets/Ashfall.Core/Save/SaveConsistencyChecker.cs`:
  ```csharp
  public sealed class SaveConsistencyChecker
  {
      /// <summary>
      /// Deep-validates the aggregate of per-store restored states for
      /// cross-system consistency. Callers pass in whichever already-loaded
      /// per-store state objects are relevant to a given rule (e.g. survivor
      /// roster + economy ledger for a dangling-survivor-reference check) —
      /// there is no single aggregate "GameState" type in this codebase.
      /// Returns errors (empty = consistent).
      /// </summary>
      public IReadOnlyList<string> CheckConsistency(SaveConsistencyInputs inputs) { ... }
  }

  /// <summary>
  /// Bag of the specific per-store states needed for cross-system checks.
  /// Extend as new consistency rules require new store references. Fields
  /// are nullable — a store that failed to load entirely is a distinct
  /// condition from a store that loaded but is internally inconsistent.
  /// Confirm each referenced state DTO's real type name against the actual
  /// save store before wiring this class — names below are placeholders.
  /// </summary>
  public sealed class SaveConsistencyInputs
  {
      public object Survivors;   // from SurvivorsSaveStore — confirm real DTO type
      public object Economy;     // from EconomySaveStore — confirm real DTO type
      public object Inventory;   // from InventorySaveStore — confirm real DTO type
      // ...extend per rule.
  }
  ```
- Consistency rules (cross-system) — confirm each referenced system/DTO actually exists with this shape before implementing the rule; do not assume the type names below are exact:
  - Every survivor ID referenced by any system exists in the survivor roster.
  - Every item ID referenced by inventory/equipment exists in the item catalog.
  - Location references resolve to valid locations in the world state.
  - Quest state references (active quests, completed steps) are internally consistent.
  - Economy ledger entries reference valid faction/trade-partner IDs.
  - Radiation dose ledger entries have non-negative dose values.
  - Day-based timestamps are monotonically ordered where expected.
  - No duplicate IDs within collections that require uniqueness.
- Severity levels: `Error` (cannot continue — trigger recovery) vs `Warning` (degraded but playable — log and continue).
- On `Error`: trigger the corruption recovery pipeline (Step 5), scoped to the specific store(s) implicated per the per-store recovery model above.
- On `Warning`: log to player-visible notification, continue play.

### Verification

- Unit test: state with dangling survivor reference → Error detected.
- Unit test: state with duplicate item IDs → Error detected.
- Unit test: state with slightly-off timestamp ordering → Warning (not Error).
- Unit test: fully valid state → zero errors, zero warnings.

```
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~SaveConsistencyChecker"
```

### Done when

Post-load validation covers ≥8 cross-system consistency rules; errors trigger recovery; warnings are logged but non-blocking; no false positives on valid saves from the existing test suite; the per-store vs. aggregate recovery decision (see Corrected Assumption above) is explicitly documented in code comments and in a new `docs/save-consistency-model.md` file, not left implicit. **Correction:** `docs/save-consistency-model.md` does not exist in the repository today (confirmed — `docs/` has 14 existing `.md` files, none with this name); this Done-when requires creating it as a new deliverable of this step, not updating an existing doc. Add it to the File Inventory table below and to this step's Implementation list explicitly, rather than leaving its creation implicit inside a Done-when bullet.

---

## Step 5 — Add Corruption Recovery Pipeline

### Goal

When a save store fails to load (checksum mismatch, deserialization failure, or deep validation error), automatically attempt recovery from that store's own backups before reporting loss to the player for that store's data.

### Implementation

- Create `Assets/Ashfall.Core/Save/SaveRecoveryPipeline.cs`. **Corrected from the original design:** this operates per-store, not on one monolithic save path, since (per Step 4's corrected assumption) there are 25 independent store files, not one:
  ```csharp
  public sealed class SaveRecoveryPipeline
  {
      /// <summary>
      /// Attempts to load a valid save for ONE store from its recovery chain
      /// (primary file → its own backups → its own legacy fallback). Callers
      /// invoke this once per save store (e.g. once for NarrativeSaveStore's
      /// path, once for SurvivorsSaveStore's path, etc.) — there is no single
      /// game-wide save path to recover in one call.
      /// </summary>
      public RecoveryResult TryRecover(string savePath, Func<string, object> deserializeAndValidate)
      {
          // Priority order (per store):
          // 1. Primary save (latest write)
          // 2. .bak.1 (previous save)
          // 3. .bak.2
          // 4. .bak.3
          // 5. Legacy bare-state fallback (pre-checksum format — confirmed
          //    this pattern already exists per-store, e.g. NarrativeSaveStore.TryLoad)
          // 6. Fail — return null with full diagnostic report for this store
      }
  }

  public sealed class RecoveryResult
  {
      public bool Success { get; }
      public object RecoveredState { get; }     // the specific store's state DTO; null if failed
      public string RecoveredFrom { get; }       // which file succeeded
      public IReadOnlyList<string> Warnings { get; }
      public IReadOnlyList<RecoveryAttempt> Attempts { get; }  // full audit trail
  }

  public sealed class RecoveryAttempt
  {
      public string FilePath { get; }
      public string FailureReason { get; }      // null if succeeded
  }
  ```
- The host (`Main.cs` or a new save-orchestration helper) calls `TryRecover` once per store during the load sequence, and aggregates the 25 `RecoveryResult`s into a single load-time report for the player/diagnostics — that aggregate report, not a `GameState`, is what Step 6's health-check command should consume.
- Recovery attempts are logged with full detail (which file, what failed, why).
- If recovered from a backup, notify the player: "Save recovered from backup (X minutes of progress may be lost)" — scoped per-store if only some stores needed recovery (e.g. "Narrative progress recovered from backup; other systems loaded normally").
- If all attempts fail for a given store, decide (and document the decision) whether that store starts fresh (acceptable for e.g. `DailyBriefingSaveStore`, likely not acceptable for `SurvivorsSaveStore`) rather than failing the entire game load — this decision varies per store and must not be treated as one uniform policy.
- Wire into the Godot host load path: replace each store's direct `TryLoad` call with `SaveRecoveryPipeline.TryRecover` for that store.
- Preserve corrupted files (rename to `.corrupt.timestamp`) for post-mortem analysis — do not delete them.

### Verification

- Unit test: primary corrupt, .bak.1 valid → recovers from .bak.1 (for a single store).
- Unit test: all backups corrupt, legacy bare-state valid → recovers from legacy.
- Unit test: all sources corrupt for one store → returns null with complete diagnostic for that store, while other stores load normally.
- Unit test: recovered state passes deep validation (Step 4) once aggregated with the other stores' states.
- Integration: corrupt one save store's file on disk, launch game → recovery succeeds from backup for that store; other 24 stores unaffected.

```
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~SaveRecoveryPipeline"
```

### Done when

Full fallback chain implemented per store (primary → .bak.1 → .bak.2 → .bak.3 → legacy → fail); player never sees "save corrupted" for a given store without that store's recovery chain being exhausted first; corrupted files preserved for analysis; the per-store-failure policy (fresh-start vs. hard-fail) is explicitly decided and documented per store, not left as an implicit uniform behavior.

---

## Step 6 — Add Save Health Diagnostic Command

### Goal

Provide a headless CLI command that inspects all 25 save stores' files, validates their integrity, checks consistency, and reports health status — usable in CI, by support staff, or by players troubleshooting issues.

### Implementation

- Add CLI verb following the existing dispatch pattern confirmed in `src/Host/HostCli.cs` (which already parses `--bridge-selftest`, `--data-integrity-selftest`, and many other `--xxx-selftest` verbs via a simple `Has(args, "--flag-name")` check feeding an enum, dispatched from a switch in `Main.cs` — see `docs/ASHFALL_CODE_INDEX.md`'s "Add a selftest gate" row for the exact recipe: add the flag parse to `HostCli.cs`, add an enum value, add the dispatch case in `Main.cs`). Verb name: `--save-health-check [save-directory]`.
- Default directory: the standard save location for the current platform (confirm the actual `user://` resolution helper already used by every save store, e.g. `ProjectSettings.GlobalizePath("user://")` as seen in `NarrativeSaveStore.SavePath` — reuse it, don't invent a new path resolution).
- Report health **per store** (25 entries) plus an overall rollup, since (per Steps 4–5's corrections) there is no single save file to report on:
  ```
  === Save Health Report ===
  [Narrative]  Primary: VALID (checksum OK)   Backups: 2/3 present   Status: HEALTHY
  [Survivors]  Primary: VALID (checksum OK)   Backups: 3/3 present   Status: HEALTHY
  [Economy]    Primary: CORRUPT (checksum mismatch)  Backups: 1/3 present, .bak.1 VALID   Status: AT_RISK
  ... (22 more stores) ...
  Orphaned .tmp files: 0

  Overall Status: AT_RISK (1 of 25 stores at risk, 0 corrupt with no recovery path)
  ```
- Health levels per store: `HEALTHY` (primary + ≥1 backup valid), `DEGRADED` (primary valid but no backups), `AT_RISK` (primary invalid but backup available), `CORRUPT` (no valid saves found for that store). Overall status is the worst status across all 25 stores.
- Additional checks:
  - File size anomalies (suspiciously small or suspiciously large).
  - File modification timestamps (detect clock skew or tampering).
  - Orphaned `.tmp` files (indicate previous interrupted writes).
  - Schema version compatibility (can this game version load the save?).
- Exit codes: 0 = healthy, 1 = degraded, 2 = at-risk, 3 = corrupt (based on overall/worst status).
- Machine-readable JSON output with `--json` flag for CI integration.

### Verification

```
godot --headless --path . -- --save-health-check --json    # exits 0 with valid JSON
```
- Unit test: directory with valid saves across all 25 stores → HEALTHY overall status.
- Unit test: directory with one corrupted store, rest healthy → AT_RISK or DEGRADED overall (per worst-status rule), with the specific store identified in output.
- Unit test: empty directory → CORRUPT status for all stores that have no legacy fallback.
- CI integration: add to the verification checklist as an optional step (note: this is a 6th check beyond AGENTS.md's canonical 5-step checklist — do not silently fold it into the "canonical" list without the project owner's sign-off, since AGENTS.md explicitly enumerates exactly 5 steps as "the canonical path").

### Done when

`--save-health-check` runs headless, reports accurate health for all 25 save stores' scenarios, returns correct exit codes based on the worst per-store status, and produces parseable output for CI.

---

## Step 7 — Write Save Corruption Resilience Tests

### Goal

Comprehensive test suite that simulates real-world corruption scenarios and verifies the entire defensive pipeline (atomic write, backup rotation, pre-write validation, post-load validation, recovery) works end-to-end.

### Implementation

- Create `Ashfall.Core.Tests/Save/SaveCorruptionResilienceTests.cs`:

  **Atomic write tests:**
  - `InterruptedWrite_OriginalSavePreserved` — mock `IFileIO` that throws mid-write; verify original file unchanged.
  - `EmptyWrite_Detected_SaveAborted` — write produces empty content; original preserved.
  - `TmpFileCleanup_OnStartup` — orphaned `.tmp` files are removed.

  **Backup rotation tests:**
  - `ThreeSaves_ProducesThreeBackups` — verify rotation chain.
  - `BackupContentMatchesPreviousSave` — backups contain correct historical state.
  - `RotationFailure_DoesNotBlockSave` — if rename fails, save still proceeds (degraded but not blocked).

  **Pre-write validation tests:**
  - `NullRequiredField_SaveAborted` — validation catches null, save does not proceed.
  - `NegativeCollectionLength_SaveAborted` — impossible state caught.
  - `OutOfRangeEnum_SaveAborted` — invalid enum value detected.
  - `ValidState_SaveProceeds` — no false positives.

  **Post-load validation tests:**
  - `DanglingSurvivorRef_ErrorReported` — cross-reference integrity.
  - `DuplicateItemIds_ErrorReported` — uniqueness constraint.
  - `ValidSave_NoErrorsNoWarnings` — clean bill of health.

  **Recovery pipeline tests:**
  - `PrimaryCorrupt_RecoverFromBackup1` — standard recovery.
  - `AllBackupsCorrupt_RecoverFromLegacy` — deep fallback.
  - `AllSourcesCorrupt_ReturnsNull_WithDiagnostic` — total failure path.
  - `RecoveredState_PassesDeepValidation` — recovered state is playable.

  **End-to-end integration:**
  - `FullCycle_Save_Corrupt_Recover_Continue` — save valid state, corrupt the file, load triggers recovery, game continues from backup.
  - `BitFlip_InChecksum_Detected` — single-bit corruption in checksum field.
  - `BitFlip_InPayload_Detected` — single-bit corruption in state data.
  - `Truncation_Detected_RecoveryTriggered` — file truncated at random offset.

### Verification

```
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~SaveCorruption"
```
All tests pass; no flaky tests; coverage of the full corruption→recovery pipeline.

### Done when

≥20 tests covering all corruption scenarios; zero false positives on valid saves from existing tests; full pipeline exercised end-to-end; all tests deterministic and fast (<5 seconds total).

---

## Summary Table

| Step | Description | Risk | Key Deliverable |
|------|-------------|------|-----------------|
| 1 | Atomic write pattern | Low-Medium (touches a shared Core port interface — see Step 1's implementer-update note) | `AtomicFileWriter.cs` — crash-safe writes for all 25 stores |
| 2 | Save backup rotation | Low | `SaveBackupRotator.cs` — rolling 3-backup history |
| 3 | Pre-write state validation | Low | `SaveStateValidator.cs` — catch invalid state before write |
| 4 | Post-load deep validation | Low-Medium | `SaveConsistencyChecker.cs` — cross-system consistency |
| 5 | Corruption recovery pipeline | Medium | `SaveRecoveryPipeline.cs` — automatic fallback chain |
| 6 | Save health diagnostic command | Low | `--save-health-check` CLI verb with exit codes |
| 7 | Save corruption resilience tests | Low | ≥20 tests covering full corruption→recovery pipeline |

---

## Success Criteria

- A crash during save NEVER corrupts the existing save file (atomic writes).
- Players always have ≥3 recent valid saves to recover from (backup rotation).
- Invalid state is caught BEFORE being written to disk (pre-write validation).
- Corrupted saves are detected and recovered automatically (recovery pipeline).
- Save health is inspectable via CLI for support/debugging (diagnostic command).
- All existing save/load tests continue to pass (no behavioral regression).
- Zero player-facing "save corrupted" messages without exhausting recovery first.

---

## Estimated Effort

| Step | Effort |
|------|--------|
| 1 | 3–4 hours |
| 2 | 2–3 hours |
| 3 | 4–6 hours |
| 4 | 4–6 hours |
| 5 | 4–5 hours |
| 6 | 3–4 hours |
| 7 | 4–6 hours |
| **Total** | **24–34 hours** |

---

## File Inventory (new files created by this roadmap)

| File | Location | Purpose |
|------|----------|---------|
| `AtomicFileWriter.cs` | `Assets/Ashfall.Core/Save/` | Crash-safe write-tmp-rename pattern |
| `SaveBackupRotator.cs` | `Assets/Ashfall.Core/Save/` | Rolling backup management |
| `SaveStateValidator.cs` | `Assets/Ashfall.Core/Save/` | Pre-write validation with attributes |
| `SaveRequiredAttribute.cs` | `Assets/Ashfall.Core/Save/` | Marks non-nullable save fields |
| `SaveRangeAttribute.cs` | `Assets/Ashfall.Core/Save/` | Marks numeric bounds for save fields |
| `SaveIdRefAttribute.cs` | `Assets/Ashfall.Core/Save/` | Marks ID reference fields with prefix |
| `SaveConsistencyChecker.cs` | `Assets/Ashfall.Core/Save/` | Post-load cross-system validation (operates on `SaveConsistencyInputs`, not a monolithic `GameState` — see Step 4) |
| `SaveRecoveryPipeline.cs` | `Assets/Ashfall.Core/Save/` | Per-store fallback recovery chain (invoked once per save store — see Step 5) |
| `SaveCorruptionException.cs` | `Assets/Ashfall.Core/Save/` | Domain exception for save failures |
| `SaveCorruptionResilienceTests.cs` | `Ashfall.Core.Tests/Save/` | ≥20 corruption scenario tests |
| `save-consistency-model.md` | `docs/` | New doc (does not exist today — confirmed against the 14 existing files in `docs/`) recording the per-store vs. aggregate recovery/consistency design decision required by Step 4's Done-when |

Note: `Assets/Ashfall.Core/` does not currently have a `Save/` subdirectory — the existing `SaveChecksum.cs` lives directly at `Assets/Ashfall.Core/SaveChecksum.cs`. Creating `Assets/Ashfall.Core/Save/` for these new files is fine (additive, no collision), but consider whether `SaveChecksum.cs` should move into the new subdirectory for consistency — that's an optional cleanup, not a requirement of this batch, and moving it is a separate small task since it would change a namespace/using path referenced by every save store.

---

## Review Notes (Corrected)

This batch was adversarially reviewed against the live codebase at `/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War`. Findings:

1. **Save store count corrected: 22 → 25.** Direct enumeration of `src/Host/*SaveStore.cs` (excluding `.uid` sidecar files) found 25 distinct save stores, not the 22 the plan repeatedly cited. All references to "22" (dependency line, Step 1's "update all 22 save stores," its Done-when, and the summary table) were corrected to 25 and the full list was named explicitly.
2. **The single-file, single-save-path assumption underlying Steps 4 and 5 was false and is the most significant correction in this batch.** The original `SaveConsistencyChecker.CheckConsistency(GameState fullState)` and `SaveRecoveryPipeline.TryRecover(string savePath)` both assume one monolithic save file/state object. Confirmed by reading `NarrativeSaveStore.cs` in full and cross-checking against the 25-store list: there is no `GameState` type anywhere in the codebase, and there is no single save path — each of the 25 stores owns an independent file, envelope type, and checksum, saved/loaded independently by `Main.cs`. This is not a cosmetic naming issue: it changes the actual design of both steps (recovery must be per-store, consistency-checking must correlate across independently-loaded per-store objects) and, if implemented as originally written, the code would not compile (there is no `GameState` type to reference) and the design would not match how saves are structured. Both steps were rewritten to operate per-store with an explicit "who decides fresh-start-vs-hard-fail-per-store" design question flagged for the implementer to resolve and document rather than leave implicit.
3. **`IFileIO`'s proposed new members (`FlushToDisk`, `MoveReplace`) are a real, uncontested gap** — confirmed the current interface (`Assets/Ashfall.Core/Ports.cs`) has neither. However, the original plan understated (and this document's own first-pass correction mischaracterized) the blast radius: adding members to a Core port interface is a breaking change, but the sole production implementer is `FileSystemIO` (`Assets/Ashfall.Core/HostDefaults.cs`) — Core's own default adapter, not a distinct "Godot host adapter" as this document previously stated in Step 1's Implementation section; the Godot host reuses Core's class directly. Step 1 was tightened to require finding and updating every implementer atomically (that one class, plus any test doubles in `Ashfall.Core.Tests`), or using C# default interface implementations to avoid the breaking change if the target frameworks allow it (worth checking: a live `dotnet build` during this review showed `Ashfall.Core` actually building against `net8.0`, not `netstandard2.1` as AGENTS.md's stack table claims — confirm the real current TFM before choosing a default-interface-member approach, since AGENTS.md itself may be stale here).
4. **Vague Done-when criteria fixed:** Step 3's "start with the 5 highest-traffic save stores" named no stores and defined no traffic metric, making its Done-when unverifiable. Corrected to require naming the specific 5 stores (with a concrete, checkable selection criterion — daily-tick frequency — and an instruction to verify the candidate list against `Main.cs`'s actual tick dispatch before committing to it) so the Done-when has something concrete to check against.
5. **Existing atomic-write precedent found and cited:** `src/Audio/AudioSettings.cs:108-110` and `src/Settings/UserSettings.cs:154-158` already implement a write-`.tmp`-then-rename pattern for their own (non-save-store) settings files. The plan didn't mention this existing precedent; Step 1 now points implementers at it so the new `AtomicFileWriter` is consistent with established local idiom rather than inventing a third pattern independently.
6. **CLI verb addition (Step 6) tightened with a real implementation pointer.** The original plan's `godot --headless --path . -- --save-health-check` verb is plausible and consistent with the project's actual CLI dispatch mechanism, confirmed via `src/Host/HostCli.cs` (which already parses ~15+ `--xxx-selftest` verbs the same way) and `docs/ASHFALL_CODE_INDEX.md`'s documented recipe for adding one. The plan previously gave no pointer to where this code should actually go; now it does. Also flagged: this would be a 6th verification step beyond AGENTS.md's canonical, explicitly-enumerated 5-step checklist — adding it to CI should get the project owner's sign-off rather than being silently treated as part of "the" checklist.
7. **Risk ratings adjusted:** Step 1 raised from Low to Low-Medium (breaking Core port interface change, not just "additive safety checks" as the batch's overall risk line claimed). Steps 4 and 5 raised from Low-Medium/Medium with the specific new risk identified (per-store vs. aggregate design ambiguity) rather than the generic "structural refactor" risk language originally given.
8. **Everything else in the plan held up under review:** the checksummed-envelope precedent, the legacy bare-state fallback pattern, and the general shape of Steps 2, 3, 6, and 7 (backup rotation, pre-write validation via attributes, health-check CLI, resilience test suite) are structurally sound given the corrections above and needed no premise-level rewrite — only the count fix, the vague-criterion fix, and pointers to real files/precedent already in the codebase.

---

## Second Review Pass — Additional Corrections

A second adversarial pass re-verified every claim in the Review Notes above directly against the live codebase (re-running the same greps and reading the same files independently) and found the first pass's corrections to be accurate, with two additional gaps not caught the first time:

9. **`docs/save-consistency-model.md`, cited in Step 4's Done-when as a place to document the per-store-vs-aggregate design decision, does not exist anywhere in the repository.** Confirmed: `docs/` contains 14 `.md` files (`AI_DISCLOSURE.md`, `ASHFALL_CODE_INDEX.md`, `ASHFALL_DEEP_CODE_AUDIT_2026-08-14.md`, `ASHFALL_DEEP_CODE_AUDIT_2_2026-08-14.md`, `ASHFALL_The_Long_Ash_Followup_Three_Model_Prompts.md`, `CI.md`, `DEBUG_LOOPS_LOG.md`, `GODOT_MIGRATION_STATUS.md`, `HOLDFAST_100_PERCENT_REPORT.md`, `HoldfastManualPlaytest.md`, `HoldfastPlaytestHandoff.md`, `HUMAN_AUTHORSHIP.md`, `MUSTER_INTEGRATION_PREP.md`, `NEW_CONTENT_DELIVERY.md`), none matching this name. The first review pass didn't check whether this reference actually resolved to a real file — it does not. Step 4's Done-when and the File Inventory table have been corrected to state explicitly that this is a **new** file this batch must create, not an existing doc to update.
10. **Step 3's "5 highest-traffic save stores" fix conflated two different frequencies.** The first pass correctly flagged the vagueness and proposed "daily-tick frequency" as a concrete criterion, naming 5 candidate stores to verify against `Main.cs`. This second pass confirmed `Main.cs`'s `CommitAdvance()` (~line 6374) is the day-advance entry point, but found that its call to `SaveAll()` is conditional on `UserSettingsStore.Current.AutoSaveOnDay` — a player setting, off by default in many game genres' conventions (not independently confirmed here whether Ashfall defaults it on or off, but the point stands regardless: it's a setting, not a guarantee). This means "ticks every day" (a simulation/Core concern, always true) and "saves every day" (a persistence/host concern, conditional on a setting) are two different claims that the first pass's fix didn't distinguish. Step 3 has been amended to direct the implementer to verify the 5 candidates against simulation state-churn frequency specifically, not save-call frequency, since the latter depends on a setting outside this step's control.
11. **Verification commands and baseline build were spot-checked and confirmed runnable.** `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` was actually executed against the live repo during this review (not just assumed): it succeeds with 0 warnings, 0 errors, confirming the plan's assumed clean baseline and that its cited verification commands are not aspirational or broken.
