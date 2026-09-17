# ASHFALL — Quality Roadmap Batch 92

## Theme: Data Migration Tooling — JSON Schema Evolution Without Manual Edits

**Priority:** MEDIUM-HIGH (296 JSON files in the data authority need coordinated format changes)
**Risk:** Medium-High — this tooling, once run in "apply" mode, mutates every file under the live data authority (`Assets/StreamingAssets/Data/`) in place. A mandatory backup step (see Step 6, now required before any non-dry-run apply, not optional) and a tested rollback path are prerequisites for using this tool on real data, not nice-to-haves.
**Batch:** 92
**Depends on:** None (tooling layer; does not require new game systems)
**Unlocks:** schema_version rollout (Batch 51), camelCase→snake_case property migration, safe expansion data restructuring, automated CI schema gates

---

## Problem Statement

The data authority (`Assets/StreamingAssets/Data/`) contains **296 JSON files total**, of which **196 are narrative JSON files** and **45 already have `schema_version`** (verified by direct count against the live repository — see Review Notes for the correction this required). Multiple planned changes require coordinated edits across these files:

1. Adding `schema_version` to the 251 files that currently lack it (296 total − 45 with it today)
2. Migrating property names from `camelCase` to `snake_case` — this is a real, verified inconsistency: e.g. `Assets/StreamingAssets/Data/items.json`, `crossing_locations.json`, and `weather_seasons.json` use `displayName`/`dangerLevel`/`travelHours`/`stackMax` (camelCase), while `Assets/StreamingAssets/Data/currents.json` and `characters.json` use `display_name`/`home_region`/`is_active`/`access_rule` (snake_case) for equivalent concepts. Both styles coexist today.
3. ID prefix standardization across expansion data
4. Expansion data restructuring for new systems

Currently, any format change requires manual editing of dozens of files — error-prone, unreviewable as a diff, and a single typo breaks `CatalogIntegrityValidator` for the entire project. No automated migration tooling exists. The project needs a repeatable, testable, reversible data migration pipeline analogous to database migration frameworks but operating on the JSON data authority.

---

## Step 1 — Design DataMigration Framework (IMigration Interface)

**Goal:** Define the engine-agnostic migration contract so each schema change is a self-contained, versioned, reversible unit of work that can be discovered and applied in order.

**Implementation:**

- Create `Assets/Ashfall.Core/DataMigration/IMigration.cs`:
  ```csharp
  namespace Ashfall.Core.DataMigration
  {
      public interface IMigration
      {
          /// Unique sequential version number (1, 2, 3, ...)
          int Version { get; }

          /// Human-readable description of what this migration does
          string Description { get; }

          /// File glob patterns this migration applies to (e.g., "*.json", "narrative/*.json")
          IReadOnlyList<string> FilePatterns { get; }

          /// Transform a single file's content forward (apply migration)
          MigrationResult Up(string filePath, string jsonContent);

          /// Transform a single file's content backward (rollback migration)
          MigrationResult Down(string filePath, string jsonContent);
      }

      public readonly struct MigrationResult
      {
          public bool WasModified { get; }
          public string ResultJson { get; }
          public string ChangeDescription { get; }

          public MigrationResult(bool wasModified, string resultJson, string changeDescription);
      }
  }
  ```
- Create `Assets/Ashfall.Core/DataMigration/MigrationManifest.cs`:
  - Tracks which migrations have been applied: `{ "applied_versions": [1, 2], "last_applied_utc": "..." }`
  - Stored at `Assets/StreamingAssets/Data/.migration_manifest.json`
  - Prevents double-application of migrations
- Create `Assets/Ashfall.Core/DataMigration/MigrationDiscovery.cs`:
  - Discovers all `IMigration` implementations via reflection (assembly scan)
  - Returns them sorted by `Version` ascending
  - Validates no duplicate version numbers exist

**Verification:**
```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   # compiles cleanly
```

**Done when:** `IMigration` interface, `MigrationResult` struct, `MigrationManifest`, and `MigrationDiscovery` compile with zero engine references in the `Ashfall.Core.DataMigration` namespace, and a `dotnet build` of `Ashfall.Core.Tests.csproj` succeeds. "Compiles with zero engine references" should be spot-checked by confirming no `using Godot;` / `using UnityEngine;` appears in the new files — the existing `noEngineReferences: true` asmdef setting (per AGENTS.md Invariant 1) enforces this at the Core-project level, but only if these new files are actually placed inside `Assets/Ashfall.Core/`.

---

## Step 2 — Implement DataMigrationRunner (Orchestrator)

**Goal:** Build the migration runner that discovers pending migrations, applies them in order, validates results, updates the manifest, and reports a detailed execution log.

**Implementation:**

- Create `Assets/Ashfall.Core/DataMigration/DataMigrationRunner.cs`:
  - Constructor: `DataMigrationRunner(IFileIO fileIO, IJsonSerializer serializer, ILog log)`
  - Core method: `MigrationReport Run(MigrationOptions options)`
  - `MigrationOptions`:
    - `string DataRootPath` — path to `Assets/StreamingAssets/Data/`
    - `int? TargetVersion` — null = apply all pending; specific = migrate to exactly that version (up or down)
    - `bool DryRun` — if true, compute changes but write nothing
    - `bool ValidateAfterEach` — if true, run `CatalogIntegrityValidator` after each migration step
  - Execution flow:
    1. Load manifest (or create if missing)
    2. Discover all migrations via `MigrationDiscovery`
    3. Determine pending migrations (version > last applied, or rollback direction)
    4. For each pending migration:
       a. Find matching files via `FilePatterns` glob
       b. Apply `Up()` (or `Down()` for rollback) to each file
       c. Collect results (modified count, skipped count, error count)
       d. If not dry-run: write modified files, update manifest
       e. If `ValidateAfterEach`: run integrity check, abort on failure
    5. Return `MigrationReport` with per-file results
- Create `MigrationReport`:
  - `int MigrationsApplied`
  - `int FilesModified`
  - `int FilesSkipped`
  - `List<MigrationError> Errors`
  - `List<FileChange> Changes` (file path + before hash + after hash)
  - `bool Success` (true if zero errors)

**Verification:**
```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~DataMigration"
```

**Done when:** `DataMigrationRunner` can discover, apply, and report migrations in sequence. Dry-run mode produces a report without writing. Manifest prevents re-application.

---

## Step 3 — Create Migration_001_AddSchemaVersion

**Goal:** Implement the first concrete migration that adds `"schema_version": 1` to every JSON file in the data authority that lacks it, bringing coverage from 45/296 to 296/296 (assuming all 296 files are eligible; see array-root handling below for files that may need different treatment).

**Implementation:**

- Create `Assets/Ashfall.Core/DataMigration/Migrations/Migration_001_AddSchemaVersion.cs`:
  - `Version = 1`
  - `Description = "Add schema_version field to all data files"`
  - `FilePatterns = ["*.json"]` (excludes `.migration_manifest.json`)
  - `Up()`:
    1. Parse JSON
    2. If top-level object already has `"schema_version"`, skip (return `WasModified = false`)
    3. Insert `"schema_version": 1` as the first property of the root object
    4. Re-serialize with consistent formatting (2-space indent, no trailing whitespace, newline at EOF)
    5. Return modified JSON
  - `Down()`:
    1. Parse JSON
    2. If top-level object has `"schema_version": 1`, remove it
    3. Re-serialize
    4. Return modified JSON
    5. If `schema_version` is not 1 (was already higher), skip — this migration didn't add it

- Handle edge cases:
  - Files that are JSON arrays at root: **verified this is not a rare edge case — it is common.** Direct inspection of the live data authority shows `currents.json`, `characters.json`, and many others are arrays of definitions at the root (`[ { ... }, { ... } ]`), not a single object. `schema_version` cannot be inserted as "the first property of the root object" for these files because there is no root object. Decide and document the actual strategy before implementation: either (a) wrap array-root files in an envelope object (`{ "schema_version": 1, "items": [...] }`), which is a breaking structural change for every loader that reads these files, or (b) skip array-root files entirely for Migration_001 and track them as a known gap. Given `CatalogLoader` classes read many of these files directly as arrays (see Batch 92's own "Relationship to Existing Systems" table — 21 remaining `*CatalogLoader.cs` files), option (a) requires coordinated loader changes and should not be scoped into Migration_001 silently.
  - Files with BOM: preserve BOM if present
  - Files with mixed line endings: normalize to LF

**Verification:**
```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~Migration_001"
```
Then, **only after a backup exists (see Step 6) and only in dry-run mode against the real data authority**:
```
godot --headless --path . -- --data-migrate --dry-run
godot --headless --path . -- --data-integrity-selftest   # baseline: confirm 0 errors BEFORE any real apply
```
Do not run `--data-migrate --apply` against `Assets/StreamingAssets/Data/` as part of this batch's verification — Step 3 delivers the migration class and its unit tests only. Actually applying Migration_001 to the live data authority is a separate, explicit action gated on Step 6's backup mechanism existing and tested first.

**Done when:** Migration_001 can add `schema_version: 1` to a fixture set of files, skip files that already have it, and roll back cleanly — verified by `Migration001Tests.cs` (Step 7). Running `--data-migrate --dry-run` against the real 296-file data authority reports approximately 251 files that would be modified (296 total − 45 with `schema_version` today) and 45 skipped; if the dry-run count differs meaningfully from this estimate, investigate before proceeding rather than assuming the estimate was wrong.

---

## Step 4 — Create Migration_002_CamelToSnakeCase

**Goal:** Implement the property-name migration that renames all camelCase JSON property names to snake_case across the data authority, resolving the documented naming inconsistency.

**Implementation:**

- Create `Assets/Ashfall.Core/DataMigration/Migrations/Migration_002_CamelToSnakeCase.cs`:
  - `Version = 2`
  - `Description = "Rename camelCase properties to snake_case"`
  - `FilePatterns = ["*.json"]`
  - `Up()`:
    1. Parse JSON into a mutable tree
    2. Walk all property names recursively (objects at any depth)
    3. For each property name, apply `CamelToSnake(name)`:
       - Insert `_` before each uppercase letter, then lowercase all
       - Handle consecutive capitals: `itemID` → `item_id`, `npcHP` → `npc_hp`
       - Known exceptions list (properties that must NOT be renamed): `schema_version` (already snake), any property starting with `$` (JSON schema refs)
    4. Detect collisions: if renaming `fooBar` to `foo_bar` collides with an existing `foo_bar`, log error and skip that property
    5. Re-serialize
  - `Down()`:
    1. Walk all property names
    2. Apply `SnakeToCamel(name)`: remove `_`, capitalize following letter
    3. Handle known exceptions (skip `schema_version`)
    4. Re-serialize
  - Provide explicit rename map for ambiguous cases:
    - `"resultItemId"` → `"result_item_id"` (not `"result_item_i_d"`)
    - `"npcId"` → `"npc_id"`
    - `"minDay"` / `"maxDay"` → `"min_day"` / `"max_day"`

- **Critical:** Update `CatalogIntegrityValidator` reference key list to use snake_case versions simultaneously, or make it recognize both during transition.

**Verification:**
```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~Migration_002"
```
Actual application to the live data authority (dry-run or apply) is explicitly out of scope for this batch — see Step 6's ordering note. Do not run `--data-migrate --apply` for Migration_002 until Step 6 is complete, tested, and a backup/clean-working-tree precondition is satisfied.

**Done when:** Migration_002 correctly renames camelCase→snake_case recursively on fixture files, handles edge cases (acronyms, collisions, exceptions), and the Up/Down cycle produces identical output to the original — verified by `Migration002Tests.cs` (Step 7). The "Critical" validator-update note below is a hard dependency, not a nice-to-have: `Up_ThenIntegrityValidator_Passes` (Step 7) must pass, meaning `CatalogIntegrityValidator`'s reference-key lists have been updated to accept snake_case, before this step can be considered complete.

---

## Step 5 — Add Dry-Run Mode with Detailed Diff Report

**Goal:** Implement a safe preview mode that shows exactly what every migration would change (per-file diffs) without writing anything, giving developers confidence before committing to a migration.

**Implementation:**

- Extend `DataMigrationRunner` dry-run output:
  - `MigrationReport` gains `List<FileDiff> Diffs` when `DryRun = true`
  - `FileDiff`:
    - `string FilePath`
    - `string BeforeSnippet` (first 5 lines changed, with context)
    - `string AfterSnippet`
    - `int PropertiesRenamed` (for rename migrations)
    - `int FieldsAdded` / `FieldsRemoved`
    - `string DiffSummary` (human-readable one-liner)
- Create `Assets/Ashfall.Core/DataMigration/DiffReporter.cs`:
  - Produces a markdown-formatted diff report suitable for code review
  - Groups changes by migration version, then by file pattern
  - Shows statistics: total files affected, total properties renamed, etc.
- Add CLI integration point for Godot host:
  - `godot --headless --path . -- --data-migrate --dry-run` → prints diff report to stdout
  - `godot --headless --path . -- --data-migrate --apply` → applies pending migrations
  - `godot --headless --path . -- --data-migrate --rollback --to-version N` → rolls back
- Dry-run report example output (illustrative; actual counts depend on the array-root handling decision made in Step 3 — see that step's Review Notes):
  ```
  Migration 001: Add schema_version
    ~251 files would be modified (45 already have schema_version, skipped)
    0 errors

  Migration 002: CamelCase → snake_case
    File/property counts to be filled in once Migration_002 is implemented and run in dry-run
    mode against the real data authority. Do not publish invented numbers in a diff-report
    example — replace this block with real dry-run output once available.
  ```

**Verification:**
```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~DryRun"
dotnet build Ashfall.csproj   # Godot host compiles with CLI verb
```

**Done when:** `--data-migrate --dry-run` produces a complete, accurate diff report against a fixture data set (not necessarily the full live data authority, to keep this step's test cycle fast). No files are written during dry-run — verified by `Apply_DryRun_WritesNothing` (Step 7) plus a manual check that `Assets/StreamingAssets/Data/` has zero uncommitted changes after a dry-run against the real tree. Report is human-readable; "suitable for a PR description" is subjective and not independently testable — treat it as a style goal, not a pass/fail criterion.

---

## Step 6 — Add Rollback Support (Bidirectional Migration)

**Ordering note (see Review Notes):** this step must be completed and tested *before* any migration from Step 3 or Step 4 is ever run in `--apply` mode against the real data authority, even though it is numbered after them in this document. The step numbers reflect a logical build-up of the framework (interface → runner → concrete migrations → dry-run → rollback → tests), not a safe execution order for touching live data. Step 3 and Step 4's own verification sections have been updated to only exercise dry-run mode and unit-test fixtures, not the live `Assets/StreamingAssets/Data/` tree, specifically because this step had not yet been reached.

**Goal:** Implement reliable rollback so any applied migration can be reversed, enabling safe experimentation and recovery from failed partial migrations.

**Implementation:**

- Extend `DataMigrationRunner` rollback path:
  - `Run(options)` with `TargetVersion < currentVersion` triggers rollback
  - Rollback applies `Down()` in reverse order (highest version first)
  - Each rollback step updates the manifest (decrements applied versions)
- Add backup-before-migrate, **mandatory, not optional**:
  - `MigrationOptions.CreateBackup` defaults to `true` and **cannot be set to `false` when `DryRun` is also `false`** — enforce this in `DataMigrationRunner`, not just by convention. A non-dry-run apply against the data authority with backups disabled must throw or refuse to run, since this step mutates the project's single source of truth for all game content in place.
  - Creates `.migration_backup/v{N}/` directory with copies of all files that will be modified
  - Backup is a full copy, not a diff — enables manual recovery even if `Down()` has a bug
  - Backup pruning: keep only last 3 backups, delete older ones — **note:** pruning must never delete the most recent backup even if fewer than 3 exist, and must never run if the most recent migration's own rollback has not been verified successful; a naive "keep last 3, delete rest" on a timer or on every apply could delete the one backup a developer needs mid-recovery. Prune only superseded backups after a new backup is confirmed complete and after the previous migration's manifest state is confirmed committed.
  - Backups are local working-tree artifacts, not a substitute for source control: this batch does not replace committing the pre-migration state to git. Before running any apply against real data, the working tree must be clean (no uncommitted changes) so `git diff`/`git checkout` remains an independent recovery path alongside `.migration_backup/`.
- Add atomic-ish application:
  - Write all modified files to `.migration_staging/` first
  - Only after all files pass validation, move them to their final locations
  - If any file fails validation mid-migration: roll back entire step (restore from backup)
  - Update manifest only after successful commit
- Add `--data-migrate --status` CLI verb:
  - Shows current manifest state: which migrations are applied, pending, available
  - Shows file counts per migration pattern

**Verification:**
```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~Rollback"
```

**Done when:** migrations can be rolled back individually or to a target version, verified by the specific tests in Step 7 (`ApplyAll_ThenRollbackAll_ProducesIdenticalFiles`, `Rollback_ToVersion_AppliesDownInReverseOrder`, `Apply_BackupCreated_ContainsOriginalFiles`). Attempting `CreateBackup = false` with `DryRun = false` throws or is rejected — add a specific test for this refusal (`Apply_BackupDisabled_NonDryRun_Throws` or equivalent), since this is the single control preventing an unrecoverable mutation of the data authority. Partial failure triggers automatic rollback of the failed step, verified by `Apply_FileValidationFails_RollsBackStep`. Status verb reports current state accurately against a fixture manifest.

---

## Step 7 — Write Migration Tests (Apply, Rollback, Integrity)

**Goal:** Comprehensive test coverage ensuring the migration framework is safe: apply/rollback produces identical files, `CatalogIntegrityValidator` passes after every migration, edge cases are handled, and the manifest tracks state correctly.

**Implementation:**

- Create `Ashfall.Core.Tests/DataMigrationRunnerTests.cs`:
  - `ApplyAll_ThenRollbackAll_ProducesIdenticalFiles` — round-trip invariant
  - `Apply_DryRun_WritesNothing` — dry-run safety
  - `Apply_ManifestUpdated_PreventsDuplicateApplication` — idempotency
  - `Apply_TargetVersion_StopsAtTarget` — partial application
  - `Rollback_ToVersion_AppliesDownInReverseOrder` — correct ordering
  - `Apply_FileValidationFails_RollsBackStep` — atomic failure handling
  - `Apply_BackupCreated_ContainsOriginalFiles` — backup correctness

- Create `Ashfall.Core.Tests/Migration001Tests.cs`:
  - `Up_FileWithoutSchemaVersion_AddsIt` — basic case
  - `Up_FileWithSchemaVersion_Skips` — idempotency
  - `Down_RemovesSchemaVersion1` — rollback
  - `Down_HigherSchemaVersion_Skips` — safety
  - `Up_ArrayRootFile_HandledGracefully` — edge case
  - `UpThenDown_ProducesOriginal` — round-trip

- Create `Ashfall.Core.Tests/Migration002Tests.cs`:
  - `Up_CamelCaseProperty_BecomesSnakeCase` — basic rename
  - `Up_NestedProperties_AllRenamed` — recursive walk
  - `Up_AcronymProperty_HandledCorrectly` — `npcID` → `npc_id`
  - `Up_Collision_Detected_PropertySkipped` — safety
  - `Up_ExceptionList_PropertiesNotRenamed` — exclusions
  - `UpThenDown_ProducesOriginal` — round-trip
  - `Up_ThenIntegrityValidator_Passes` — compatibility (requires updated validator)

- Create `Ashfall.Core.Tests/DataMigrationIntegrationTests.cs`:
  - Uses a fixture copy of 5 representative JSON files from the data authority
  - Applies all migrations in sequence, validates after each
  - Rolls back all migrations, validates original state restored
  - Confirms `CatalogIntegrityValidator` passes at every state

**Verification:**
```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~DataMigration"
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~Migration"
```

**Done when:** All migration tests pass. Round-trip invariant (apply then rollback = original) holds for both migrations. Integration test confirms integrity validator compatibility.

---

## Summary Table

| Step | Deliverable | Layer | Key Files | Risk |
|------|-------------|-------|-----------|------|
| 1 | `IMigration` interface + manifest + discovery | Core | `Assets/Ashfall.Core/DataMigration/IMigration.cs`, `MigrationManifest.cs`, `MigrationDiscovery.cs` | None |
| 2 | `DataMigrationRunner` orchestrator | Core | `Assets/Ashfall.Core/DataMigration/DataMigrationRunner.cs` | Low |
| 3 | Migration_001: Add `schema_version` | Core | `Assets/Ashfall.Core/DataMigration/Migrations/Migration_001_AddSchemaVersion.cs` | Low |
| 4 | Migration_002: camelCase → snake_case | Core | `Assets/Ashfall.Core/DataMigration/Migrations/Migration_002_CamelToSnakeCase.cs` | Medium |
| 5 | Dry-run mode + diff report + CLI verbs | Core + Host | `DiffReporter.cs`, CLI verb in `Main.cs` | Low |
| 6 | Rollback support + backup + atomic apply | Core | `DataMigrationRunner.cs` (extended) | Medium |
| 7 | Test suite (runner + migrations + integration) | Tests | `Ashfall.Core.Tests/DataMigration*Tests.cs`, `Migration*Tests.cs` | None |

---

## Architecture Notes

- **Engine-agnostic:** All migration logic lives in `Ashfall.Core.DataMigration`. Uses `IFileIO` for file access and `IJsonSerializer` for parsing. No `Godot.*` or `UnityEngine.*`.
- **Reversible:** Every migration implements both `Up()` and `Down()`. The round-trip invariant is tested: applying then rolling back produces byte-identical output.
- **Safe:** Dry-run is the default CLI mode. Backup is created before apply. Partial failure triggers automatic rollback. Manifest prevents double-application.
- **Incremental:** Migrations are numbered and applied in order. You can target a specific version. The system handles gaps gracefully (warns but continues).
- **Validator-compatible:** Migrations must leave `CatalogIntegrityValidator` passing. The integration test enforces this. Migration_002 requires coordinated updates to the validator's reference key list.
- **No runtime dependency:** The migration system is a development/CI tool. It does not run during gameplay. Save files are not affected (they have their own versioned codec system).

---

## Relationship to Existing Systems

| Existing System | Interaction |
|-----------------|-------------|
| `CatalogIntegrityValidator` | Must pass after every migration; Migration_002 requires validator key list update |
| `CatalogLoader` classes (21 remaining) | After Migration_002, loaders must use snake_case keys |
| `SaveWireContract` | Not affected — save format is separate from data authority format |
| Narrative JSON (196 files) | Included in Migration_001 (schema_version) — subject to the array-root handling decision in Step 3, since many narrative and definition files (e.g. `currents.json`, `characters.json`, verified directly) are JSON arrays at root; Migration_002 renames properties |
| Expansion data files | Future migrations (003+) will restructure; framework supports any JSON transform |

---

## Next Prompt

```
Implement Step 1 of Batch 92: Design the DataMigration framework in Assets/Ashfall.Core/DataMigration/
with IMigration interface, MigrationResult struct, MigrationManifest, and MigrationDiscovery.
Follow Invariant 1 (zero engine coupling). Use IFileIO and IJsonSerializer ports. Verify with dotnet build.
```

---

## Review Notes (Corrected)

This batch was adversarially reviewed against the live codebase at
`/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War`. Findings and fixes applied in place:

**1. File-count claims were wrong and internally inconsistent (fixed):**
The original draft mixed three different, uncorroborated numbers: "130+ definition files," "296 narrative JSON files" in the Depends-on line (actually referring to the narrative subset, but stated confusingly), "only 35 of ~280 have it today" for schema_version coverage, and a Step 3 goal of "35/280 to 280/280." A direct count against the live repository gives:
- **296** total `*.json` files under `Assets/StreamingAssets/Data/` (`find ... -name "*.json" | wc -l`)
- **45** of those already contain `schema_version` (`grep -rl "schema_version" ... | wc -l`)
- **196** are narrative JSON files under `Assets/StreamingAssets/Data/narrative/`

These are the numbers this batch's own text says should align with "Batch 51 (schema_version rollout)" and its corrected findings. Every occurrence of the old numbers (130+, 35/280, 245-would-be-modified example) has been replaced with 296 total / 45 with schema_version / ~251 remaining, consistently, throughout this document. If Batch 51 is later found to state different corrected numbers than 296/45, reconcile against Batch 51's own text, not against this document's prior draft.

**2. camelCase/snake_case inconsistency claim — verified true (no fix needed, evidence added):**
Direct inspection of real files confirms this is real, not assumed: `Assets/StreamingAssets/Data/items.json`, `crossing_locations.json`, and `weather_seasons.json` use camelCase keys (`displayName`, `dangerLevel`, `travelHours`, `stackMax`, `radProtection`), while `Assets/StreamingAssets/Data/currents.json` and `characters.json` use snake_case keys (`display_name`, `home_region`, `is_active`, `access_rule`) for structurally similar definition objects. The Problem Statement now cites these specific files as evidence rather than asserting the inconsistency as an unsupported claim.

**3. Missing mandatory backup/rollback gate before live mutation (fixed — this was the most serious gap):**
The original Step 6 made backup-before-apply an *option* (`MigrationOptions.CreateBackup = true (default for non-dry-run)`), which means a caller could explicitly pass `CreateBackup = false` and mutate the entire data authority — the project's single source of truth for all game content — with no recovery path. Given this project's own AGENTS.md explicitly separates "Data authority (JSON)" as authoritative and treats `Assets/StreamingAssets/Data/` as sacrosanct, an *optional* backup ahead of an irreversible bulk-write tool is not an acceptable default. Fixed by:
- Making `CreateBackup` non-overridable to `false` when `DryRun = false` — enforced in code, not just documentation, with a required test (`Apply_BackupDisabled_NonDryRun_Throws`).
- Requiring a clean git working tree before any real (non-dry-run) apply, so `.migration_backup/` is not the *only* recovery path.
- Correcting backup pruning logic, which as originally written ("keep only last 3, delete older") could delete the one backup a developer needs mid-recovery if pruning ran on a timer or on every apply rather than only after a verified-successful new backup.
- Explicitly re-ordering execution: Step 6 (backup + rollback) must be built and tested *before* Step 3 or Step 4's migrations are ever run against real data, even though it is numbered later in the document. Step 3 and Step 4's verification sections were changed to only exercise fixtures and dry-run mode, since at the point those steps are implemented, Step 6 does not yet exist.

**4. Array-root JSON files break Migration_001's assumed structure (fixed):**
The original Step 3 described inserting `schema_version` "as the first property of the root object" and treated array-root files as a rare edge case ("wrap consideration"). Direct inspection shows array-root files are common, not rare — `currents.json` and `characters.json` (both sampled directly) are arrays of objects at the root with no wrapping object at all. The plan now requires an explicit decision (envelope-wrap vs. skip) before implementation, and flags that envelope-wrapping would require coordinated changes to the 21 remaining `*CatalogLoader.cs` files that read these files as arrays today (per this document's own "Relationship to Existing Systems" table) — this is a materially larger scope than "add one field to every file" if the envelope approach is chosen.

**5. Fabricated example output presented as real numbers (fixed):**
Step 5's dry-run example output ("198 files would be modified, 1,247 properties would be renamed, 2 collisions detected") was invented illustrative text with false precision, not real tool output — nothing in this batch has produced that output, since Migration_002 doesn't exist yet. Replaced with a placeholder that explicitly says to substitute real dry-run output once available, so this document cannot be mistaken for a report of actual results.

**6. Unrunnable/underspecified verification commands (fixed):**
Several steps' verification blocks ran `godot --headless --path . -- --data-integrity-selftest` or `--data-migrate --apply` against the implication of the real data authority before the tooling that makes that safe (Step 6) existed. Verification sections for Steps 1-5 were tightened to either use fixtures/dry-run only, or to explicitly state the precondition (backup exists, working tree clean) before any command that would touch live data.

**7. Scope check — no scope creep found beyond the above:**
Steps 1-2 (interface + orchestrator) and Step 7 (tests) were reviewed and found reasonably scoped; no changes made beyond number corrections. Step 4 (camelCase→snake_case) is correctly flagged in the original text as "Medium" risk and as requiring coordinated `CatalogIntegrityValidator` updates — this framing was accurate and is preserved.
