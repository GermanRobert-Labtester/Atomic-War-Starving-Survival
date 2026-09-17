# ASHFALL — Quality Roadmap Batch 79

## Theme: Multi-Save Slot System — Named Saves, Quick Save, Auto-Save

**Priority:** MEDIUM-HIGH
**Risk:** Medium — changes save file management across 30 stores (see Corrected Count note)
**Estimated Effort:** 6–8 sessions (see risk note below — likely underestimated at the real
store count)
**Prerequisites:** All 30 save stores implement `CaptureState/RestoreState`, checksummed
envelopes operational (Batch 65 backup rotation, checksum sweep tests passing)

---

**⚠️ Corrected count:** this plan's header and body say "22 save stores." Direct enumeration
(`grep -rhoE "public static class \w+SaveStore" src/ | sort -u`) finds **30** distinct save
store classes today, writing 30 distinct `*_save.json` filenames under `user://`. Batch 65 had
already corrected the count once before, from 22 to 25; five more stores have been added since
Batch 65 was written (`GreenhouseSaveStore`, `JournalSaveStore`, `ShelterAssignmentSaveStore`,
`StartingLevelSaveStore`, `YearOfAshSaveStore`), bringing the current total to 30. Every "22"
and "23" reference below (including the metadata-checksum-concatenation math in Step 2 and the
"23 total files" deletion count in Step 7) is corrected to reflect **30 stores + 1 metadata file
= 31 total files per slot**. The full current list is:

`CaravanSaveStore`, `CombatSaveStore`, `CraftingSaveStore`, `DailyBriefingSaveStore`,
`DoseLedgerSaveStore`, `DutyRosterSaveStore`, `EconomySaveStore`, `ExpansionHubSaveStore`,
`ExpeditionSaveStore`, `GreenhouseSaveStore`, `HoldfastSaveStore`, `HoldfastTradeSaveStore`,
`InventorySaveStore`, `MaritimeSaveStore`, `MedicalSaveStore`, `MedicalWardSaveStore`,
`MemorialSaveStore`, `MusterSaveStore`, `NarrativeSaveStore`, `PhantomMemorySaveStore`,
`Phase0SaveStore`, `PowerGridSaveStore`, `RadioSaveStore`, `ShelterAssignmentSaveStore`,
`StartingLevelSaveStore`, `SurvivorsSaveStore`, `VerdictSaveStore`, `WorldSaveStore`,
`JournalSaveStore` (in `src/Journal/`), `YearOfAshSaveStore` (in `src/YearOfAsh/`).

**⚠️ Additional finding — `GreenhouseSaveStore` is architecturally inconsistent with the other
29 stores.** Every other store follows the pattern described in this plan: a `FileName` const,
`IJsonSerializer`/`SystemTextJsonSerializer`, and a checksummed envelope. `GreenhouseSaveStore`
(`src/Host/GreenhouseHostSession.cs:180`) instead uses a `SavePath` property (not a `FileName`
const + directory-prefix pattern), calls `System.Text.Json.JsonSerializer` directly instead of
going through `IJsonSerializer`, and writes a bare `GreenhouseState` with no checksum envelope
at all. Step 3's plan to give "each store's constructor" a uniform `SaveContext` parameter will
hit this store as a special case — either fix `GreenhouseSaveStore` to match the standard
pattern *before* Step 3, or explicitly scope it out and track it as follow-up debt. As written,
Step 3 will silently either skip this store (leaving it writing to the un-sloted root `user://`
forever, corrupting the "all stores respect the active slot" invariant) or the implementer will
improvise a one-off fix under time pressure. This must be decided explicitly, not discovered
mid-implementation.

---

### Motivation

The current save system has 30 save stores, each writing to a single hardcoded filename
(`holdfast_s1_save.json`, `expedition_save.json`, etc.) under `user://`. Players have exactly
one save slot. If that save corrupts — or if the player simply wants to try a different strategy
without losing progress — they have no recourse.

Batch 65 added backup file rotation (keep N previous versions of the single save), which
mitigates corruption but doesn't solve the fundamental problem: players need multiple named
saves, auto-save on day advance, and quick-save at will. This batch builds the multi-slot
infrastructure.

---

## Step 1 — Design Save Slot Architecture

**Goal:** Define the directory structure, metadata format, and slot lifecycle (create, load,
rename, copy, delete) for multi-slot saves.

**Implementation:**
- Define directory layout under `user://saves/`:
  ```
  user://saves/
  ├── slot_001/
  │   ├── metadata.json          # slot name, creation time, last-save time, day, survivor count
  │   ├── holdfast_s1_save.json  # same filenames as today, just inside slot dir
  │   ├── expedition_save.json
  │   ├── medical_save.json
  │   ├── narrative_save.json
  │   ├── world_save.json
  │   ├── journal_save.json
  │   ├── ... (all 30 store files — see corrected list at top of this document)
  │   └── screenshot.png         # optional: thumbnail of save moment
  ├── slot_002/
  │   └── ...
  ├── autosave/
  │   └── ... (same structure, overwritten on auto-save)
  └── quicksave/
      └── ... (same structure, overwritten on quick-save)
  ```
- Define `metadata.json` schema:
  ```json
  {
    "schema_version": 1,
    "slot_id": "slot_001",
    "display_name": "Day 47 — Before the Storm",
    "created_at": "2025-01-15T10:30:00Z",
    "last_saved_at": "2025-01-15T14:22:00Z",
    "game_day": 47,
    "survivor_count": 8,
    "play_time_seconds": 14520,
    "difficulty": "standard",
    "checksum": "a3f7c9..."
  }
  ```
- Define slot limits:
  - Manual slots: configurable max (default 10).
  - Auto-save: 1 rotating slot (overwrites).
  - Quick-save: 1 rotating slot (overwrites).
- Define lifecycle operations: `CreateSlot`, `LoadSlot`, `SaveToSlot`, `RenameSlot`,
  `CopySlot`, `DeleteSlot`, `ListSlots`.
- Define migration path: existing single-file saves auto-migrate into `slot_001/` on first
  launch after upgrade.

**Verification:**
- The design document covers all 30 stores by name (use the corrected list at the top of this
  document — do not re-derive it by memory; re-run
  `grep -rhoE "public static class \w+SaveStore" src/ | sort -u` and diff against the list to
  catch any stores added between now and implementation), the metadata format, and all
  lifecycle operations.
- Migration path from current single-file saves is documented.
- Edge cases are addressed: what happens when max slots reached, what if slot dir is partially
  written (crash during save), what if metadata.json is corrupt but save files are intact.
- `GreenhouseSaveStore`'s non-standard pattern (see corrected-count note above) is explicitly
  addressed: either "fixed to standard pattern before Step 3" or "explicitly excluded from
  slotting with a tracked follow-up" — not left ambiguous.

**Done when:** `docs/saves/MULTI_SLOT_ARCHITECTURE.md` exists, is reviewed and approved by a
second person (not just written), covers all 30 stores by name with no omissions, and
explicitly resolves the `GreenhouseSaveStore` special case above. "Complete design" alone is
not a verifiable exit condition — the two bullets above are.

---

## Step 2 — Add SaveSlotManager in Core

**Goal:** Implement the Core-layer save slot manager that handles slot CRUD, metadata, and
directory orchestration — engine-agnostic, using `IFileIO`.

**Implementation:**
- Create `Assets/Ashfall.Core/Saves/SaveSlotManager.cs`:
  ```csharp
  namespace Ashfall.Core.Saves;

  public class SaveSlotManager
  {
      private readonly IFileIO _fileIO;
      private readonly IJsonSerializer _json;
      private readonly IClock _clock;
      private readonly int _maxManualSlots;

      public SaveSlotManager(IFileIO fileIO, IJsonSerializer json, IClock clock,
                             int maxManualSlots = 10) { ... }

      public SaveSlotMetadata CreateSlot(string displayName) { ... }
      public IReadOnlyList<SaveSlotMetadata> ListSlots() { ... }
      public SaveSlotMetadata GetSlotMetadata(string slotId) { ... }
      public void RenameSlot(string slotId, string newName) { ... }
      public void CopySlot(string sourceSlotId, string targetSlotId) { ... }
      public void DeleteSlot(string slotId) { ... }
      public string GetSlotDirectory(string slotId) { ... }
      public string GetAutoSaveDirectory() { ... }
      public string GetQuickSaveDirectory() { ... }
      public bool SlotExists(string slotId) { ... }
      public bool IsAtCapacity() { ... }
  }
  ```
- Create `Assets/Ashfall.Core/Saves/SaveSlotMetadata.cs`:
  ```csharp
  [Serializable]
  public class SaveSlotMetadata
  {
      public int SchemaVersion { get; set; } = 1;
      public string SlotId { get; set; }
      public string DisplayName { get; set; }
      public string CreatedAt { get; set; }   // ISO 8601
      public string LastSavedAt { get; set; } // ISO 8601
      public int GameDay { get; set; }
      public int SurvivorCount { get; set; }
      public long PlayTimeSeconds { get; set; }
      public string Difficulty { get; set; }
      public string Checksum { get; set; }
  }
  ```
- Slot ID generation: `slot_001`, `slot_002`, ... (next available, never reuse deleted IDs
  within a session to avoid confusion; IDs can be reused across sessions).
- All file operations go through `IFileIO` — no direct `System.IO` calls.
- JSON operations go through `IJsonSerializer` — no `JsonUtility`.
- Metadata checksum is computed from the concatenation of all 30 store checksums. Note:
  `GreenhouseSaveStore` does not currently produce a checksum (see corrected-count note at the
  top of this document) — this concatenation is undefined until that store is either fixed or
  explicitly excluded. Resolve before implementing, not during.

**Verification:**
```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   # Must compile
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj    # All tests pass
```
- No `UnityEngine.*`, `Godot.*`, or `JsonUtility` references.
- All public methods have XML doc comments.
- Class is testable with mock `IFileIO` and `IJsonSerializer`.

**Done when:** `SaveSlotManager.cs` and `SaveSlotMetadata.cs` exist in Core, compile cleanly,
and have no engine dependencies.

---

## Step 3 — Modify All 30 Save Stores to Accept Slot Path Prefix

**Goal:** Refactor every save store to write into a configurable directory (the active slot's
path) instead of a hardcoded `user://` root.

**Implementation:**
- Note: these are `public static class` stores with static `TrySave`/`TryLoad` methods, not
  instantiable classes with constructors — "modify each store's constructor" (as originally
  written) does not match the actual code shape. The real change is to add a `SaveContext`
  (or `pathOverride`) parameter to each store's static `TrySave`/`TryLoad` methods.
  `StartingLevelSaveStore.TrySave` already has a `string? pathOverride = null` parameter
  precedent (`src/Host/StartingLevelHostSession.cs`) — follow that existing convention rather
  than inventing a new one.
- Define a `SaveContext` class (or extend existing save infrastructure):
  ```csharp
  public class SaveContext
  {
      public string BaseDirectory { get; }  // e.g., "user://saves/slot_001/"
      public SaveContext(string baseDirectory) { BaseDirectory = baseDirectory; }
  }
  ```
- Modify each save store's static save/load methods to accept an optional `SaveContext`:
  - `ExpeditionSaveStore` → `ExpeditionSaveStore.TrySave(state, context = null)`
  - `MedicalSaveStore` → same pattern
  - `NarrativeSaveStore` → same pattern
  - ... all 30 stores (see corrected list at top of this document).
  - `GreenhouseSaveStore` must be fixed to the standard `FileName`-const pattern first, or
    explicitly excluded from this step (see corrected-count note).
- Each store computes its file path as: `context.BaseDirectory + "expedition_save.json"`
  (concatenation replaces the previous hardcoded path).
- The hardcoded filename (e.g., `expedition_save.json`) stays the same — only the directory
  changes.
- Add a `MigrateLegacySave(string legacyPath, SaveContext targetSlot)` utility that moves
  old root-level save files into a slot directory.
- Ensure backward compatibility: if no `SaveContext` is provided (default constructor), fall
  back to legacy root behavior (allows incremental migration).

**Verification:**
```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```
- All existing save/load tests still pass (they use the default/legacy path).
- A new test writes to a slot directory and reads back successfully, for every one of the 30
  stores (not a sample) — a partial migration where some stores respect `SaveContext` and
  others silently keep writing to the un-sloted root is a data-loss bug, not a minor gap.
- The checksum sweep tests in `Ashfall.Core.Tests/SaveStoreChecksumSweepTests.cs` (the actual
  top-level test class in that file is `ExpeditionSaveChecksumTests` — the filename and the
  class name differ; verify by class name, not filename, when scripting CI) still pass.

**Done when:** All 30 save stores (per the corrected list at the top of this document) accept
an optional `SaveContext`/path-override parameter on their static save/load methods, compile
cleanly, pass all existing tests, and a passing test demonstrates each of the 30 stores
independently writing to and reading from an arbitrary non-default slot directory. "Can write to
arbitrary slot directories" is not verifiable without an explicit per-store test — add one.

---

## Step 4 — Add Auto-Save Trigger on Day Advance

**Goal:** Automatically save the game to the auto-save slot every N simulated days (configurable),
protecting players from progress loss without manual intervention.

**Implementation:**
- Add auto-save configuration to game settings:
  ```csharp
  public class AutoSaveSettings
  {
      public bool Enabled { get; set; } = true;
      public int IntervalDays { get; set; } = 3;  // save every 3 game days
      public bool ShowNotification { get; set; } = true;
  }
  ```
- Hook into the day-advance tick in the simulation loop:
  - After `TickSimDay` completes and day counter increments, check:
    `if (autoSaveSettings.Enabled && currentDay % autoSaveSettings.IntervalDays == 0)`
  - If true, trigger a full save to `SaveSlotManager.GetAutoSaveDirectory()`.
- The auto-save operation:
  1. Calls `CaptureState()` on all 30 systems (same as manual save).
  2. Writes all 30 store files into the `autosave/` directory.
  3. Updates `autosave/metadata.json` with current game state.
  4. Optionally shows a brief UI notification ("Auto-saved — Day 47").
- Auto-save must not block the simulation tick — if save takes >100ms, consider async
  write (write to temp, rename atomically). **Risk:** the codebase today has zero async I/O
  anywhere (confirmed: no `Task.Run`, `async`/`await` on any save path, no background thread
  use in `src/` or `Assets/Ashfall.Core/`). Introducing async save here is a bigger step than
  this bullet implies — it would be the first async I/O in the project and needs its own design
  pass (see Batch 80's threading-contract work, which this batch should sequence *after*, not
  in parallel — reversed from the numeric ordering below). If the 100ms budget is not exceeded
  in practice, skip async entirely and say so; do not add concurrency to solve a problem that
  hasn't been measured yet.
- Auto-save respects the existing checksummed envelope format.

**Verification:**
```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```
- Unit test: advance day counter past interval threshold, verify auto-save was triggered.
- Unit test: auto-save disabled, verify no save occurs.
- Unit test: auto-save produces valid checksummed envelopes in the autosave directory for all
  30 stores that support checksums (`GreenhouseSaveStore` excluded until fixed — see corrected-
  count note).

**Done when:** Auto-save triggers reliably on day advance at the configured interval, writes to
the dedicated autosave slot for all in-scope stores, has passing unit tests, and does not
silently drop any store from the autosave payload (verify against the count, not "looks
complete").

---

## Step 5 — Add Quick-Save Command

**Goal:** Allow players to immediately save their current state to the quick-save slot via a
single command (hotkey-bindable), overwriting the previous quick-save.

**Implementation:**
- Add `QuickSaveCommand` in the Godot host layer (`src/`):
  ```csharp
  public class QuickSaveCommand
  {
      private readonly SaveSlotManager _slotManager;
      private readonly Action _saveAllToSlot;  // delegate that runs the full save pipeline

      public void Execute()
      {
          var dir = _slotManager.GetQuickSaveDirectory();
          _saveAllToSlot(dir);
          // Update metadata
          // Show notification: "Quick-saved"
      }
  }
  ```
- Wire to Godot input action: default key `F5` for quick-save, `F9` for quick-load.
  - Register input actions in `project.godot` or via code in the host.
  - Input handling in the Godot host layer (not in Core — Core has no input concept).
- Quick-load:
  ```csharp
  public class QuickLoadCommand
  {
      public void Execute()
      {
          var dir = _slotManager.GetQuickSaveDirectory();
          if (!_slotManager.SlotExists("quicksave")) { /* notify: no quick-save */ return; }
          _loadAllFromSlot(dir);
          // Show notification: "Quick-loaded — Day 47"
      }
  }
  ```
- Guard against quick-save during state transitions (mid-tick, during another save, during
  load). Use a `SaveLock` flag.
- Quick-save uses the same checksummed envelope format as all other saves.

**Verification:**
```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```
- Test: quick-save produces all 30 store files in the quicksave directory (per corrected count;
  `GreenhouseSaveStore` excluded/fixed per the note above).
- Test: quick-load restores state correctly from quicksave directory.
- Test: quick-save during lock is rejected gracefully (no crash, no partial write).

**Done when:** Quick-save and quick-load commands work, are wired to input actions, produce
valid saves for all in-scope stores, and have passing tests that assert the exact file count
written (30, or 29 + explicit `GreenhouseSaveStore` exclusion) rather than "looks complete."

---

## Step 6 — Add Save Browser UI Panel

**Goal:** Create a UI panel that displays all save slots with metadata (day, survivor count,
timestamp, play time) and allows the player to load, rename, copy, or delete slots.

**Implementation:**
- Create `src/UI/Panels/SaveBrowserPanel.cs` (programmatic, no .tscn):
  - Uses `AshfallDashboardShell` as container (if applicable) or standalone panel.
  - Lists all slots from `SaveSlotManager.ListSlots()`.
  - Each slot row displays:
    - Slot name (editable on double-click or rename button).
    - Game day + survivor count.
    - Last saved timestamp (relative: "2 hours ago" or absolute).
    - Play time (formatted as HH:MM).
    - Difficulty badge.
  - Slot actions (buttons per row):
    - **Load** — loads the slot and closes the browser.
    - **Rename** — inline edit of display name.
    - **Copy** — duplicates into a new slot (if not at capacity).
    - **Delete** — with confirmation dialog ("Delete 'Day 47 — Before the Storm'?").
  - Footer actions:
    - **New Save** — saves current state into a new slot (prompts for name).
    - **Overwrite Current** — saves into the currently-loaded slot.
  - Special rows for auto-save and quick-save slots (visually distinct, not deletable, only
    loadable or copyable-to-manual-slot).
  - Empty state: "No saves yet. Start a new game to create your first save."
- Sorting: most recent first (by `last_saved_at`).
- Capacity indicator: "7 / 10 slots used" with warning when near max.

**Verification:**
```
dotnet build Ashfall.csproj   # Godot host compiles with the new panel
```
- Panel renders without errors. Note: `godot --headless` scene instantiation for a
  programmatic (non-`.tscn`) panel is not a defined, runnable verification step as written —
  either name the actual headless self-test verb this project uses for host UI panels (see
  existing precedent, e.g. `--survivors-selftest`-style verbs registered in `src/Main.cs`, and
  add a comparable `--save-browser-selftest` verb) or replace this bullet with an explicit
  manual QA checklist. "Otherwise visual smoke test" is not a pass/fail criterion — pick one.
- All button actions call the correct `SaveSlotManager` methods.
- Delete confirmation prevents accidental deletion.
- Panel correctly shows empty state when no slots exist.

**Done when:** `SaveBrowserPanel.cs` exists, compiles, integrates with `SaveSlotManager`,
provides full slot management UI with load/rename/copy/delete/create functionality, and has a
named, runnable headless self-test verb (not an undefined "visual smoke test") demonstrating it
loads without error.

---

## Step 7 — Write Multi-Slot Tests

**Goal:** Create comprehensive tests ensuring multi-slot saves are isolated, concurrent slots
don't bleed state, slot deletion removes all files, and the migration path works.

**Implementation:**
- Create `Ashfall.Core.Tests/SaveSlotManagerTests.cs`:
  ```csharp
  // Slot isolation
  [Fact] public void TwoSlots_DifferentState_DoNotBleed() { ... }
  [Fact] public void SaveToSlotA_LoadFromSlotB_GetsSlotBState() { ... }

  // CRUD operations
  [Fact] public void CreateSlot_GeneratesMetadata_WithCorrectFields() { ... }
  [Fact] public void RenameSlot_UpdatesDisplayName_PreservesContent() { ... }
  [Fact] public void CopySlot_ProducesIdenticalContent_DifferentId() { ... }
  [Fact] public void DeleteSlot_RemovesAll22Files_AndMetadata() { ... }
  [Fact] public void DeleteSlot_NonexistentSlot_ThrowsOrNoOp() { ... }

  // Capacity
  [Fact] public void CreateSlot_AtMaxCapacity_Rejected() { ... }
  [Fact] public void IsAtCapacity_ReturnsTrue_WhenFull() { ... }

  // Auto-save and quick-save
  [Fact] public void AutoSave_OverwritesPreviousAutoSave() { ... }
  [Fact] public void QuickSave_OverwritesPreviousQuickSave() { ... }
  [Fact] public void AutoSave_NotCountedAgainstManualSlotLimit() { ... }

  // Migration
  [Fact] public void LegacySave_MigratedIntoSlot001_OnFirstLaunch() { ... }
  [Fact] public void LegacyMigration_Idempotent_SecondRunNoOp() { ... }

  // Integrity
  [Fact] public void SavedSlot_HasValidChecksum_InMetadata() { ... }
  [Fact] public void CorruptMetadata_IntactSaveFiles_StillLoadable() { ... }

  // Crash safety
  [Fact] public void PartialWrite_DetectedAsCorrupt_NotLoadedSilently() { ... }
  ```
- Use mock `IFileIO` that simulates a real filesystem (in-memory dictionary).
- Test with multiple save stores writing to the same slot directory to ensure no filename
  collisions.
- Verify that deleting a slot removes exactly 30 store files + 1 metadata file (31 total, per
  the corrected count — adjust to 30 total if `GreenhouseSaveStore` is excluded rather than
  fixed).

**Verification:**
```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   # Must compile
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj    # All tests pass (including new ones)
```
- All 15+ new tests pass.
- No existing tests regress.
- Coverage: slot isolation, CRUD, capacity, auto/quick-save, migration, integrity, crash safety.

**Done when:** `SaveSlotManagerTests.cs` exists with 15+ tests covering all multi-slot scenarios,
all passing, no regressions in existing save tests, and the fixture list of stores under test
matches the corrected 30-store list above (re-verify by grep at implementation time, since the
count has already changed twice — 22 → 25 → 30 — across the life of this roadmap).

---

## Summary Table

| Step | Deliverable | Type | Risk | Depends On |
|------|-------------|------|------|------------|
| 1 | `docs/saves/MULTI_SLOT_ARCHITECTURE.md` | Design Doc | None | — |
| 2 | `Assets/Ashfall.Core/Saves/SaveSlotManager.cs` | Code (Core) | Low | Step 1 |
| 3 | All 30 save stores accept `SaveContext` | Code (Core + Host) | Medium-High (see note) | Step 2 |
| 4 | Auto-save on day advance | Code (Core + Host) | Low | Steps 2, 3 |
| 5 | Quick-save / quick-load commands | Code (Host) | Low | Steps 2, 3 |
| 6 | `SaveBrowserPanel.cs` | Code (Host UI) | Low | Steps 2, 3 |
| 7 | `SaveSlotManagerTests.cs` | Tests | None | Steps 2–5 |

Step 3's risk is raised from Medium to Medium-High relative to the original estimate: at 30
stores (not 22) with one of them (`GreenhouseSaveStore`) not following the standard pattern,
this step touches more surface area and has one structural outlier that must be resolved, not
just "modified like the rest." The 6–8 session estimate at the top of this document was set
against the (already-stale) 22-store premise; re-estimate against 30 stores + 1 non-conforming
store before committing to a session budget.

---

## Risk Mitigation

| Risk | Impact | Mitigation |
|------|--------|------------|
| Partial write during save (crash/power loss) | Corrupt slot | Write to temp dir, atomic rename on completion. Old slot intact until rename succeeds. |
| 30 stores writing sequentially is slow | Noticeable pause on save | Profile; if >200ms, batch writes or use background thread with lock. Note: this codebase has zero existing async/background I/O (see Batch 80 audit) — introducing one here is a bigger step than "use background thread" implies; do not casually add the project's first async I/O path without the threading-contract groundwork this implies. |
| Migration breaks existing saves | Players lose progress | Migration copies (not moves) legacy files; original untouched until verified. |
| Slot directory deleted externally | Slot vanishes from browser | `ListSlots` gracefully handles missing dirs; shows "damaged" indicator. |
| Metadata.json corrupt but saves intact | Can't show slot in browser | Fall back to reading first available store file for basic metadata (day, etc.). |
| **Partial store migration in Step 3** (new) | Some of the 30 stores respect `SaveContext`, others silently keep writing to the un-sloted root `user://` — the "damaged"/incomplete slot appears to save successfully but loses state from the stores that weren't migrated | Track migration completion per-store in a checklist (all 30 named), gate Step 3's "done" on the full list, not a sampled subset. |
| **`GreenhouseSaveStore` non-conformance** (new) | This store has no checksum and doesn't use `IJsonSerializer`; slotting it as-is either breaks the metadata-checksum-concatenation math in Step 2 or silently excludes greenhouse state from every save slot | Decide explicitly (fix-first vs. exclude-and-track) before Step 2 is implemented, not discovered during Step 3. |
| **Rollback** (missing from original) | If Steps 2–3 ship a regression that corrupts saves in production, there is no stated rollback path | Add: ship Step 3 behind a feature flag/config toggle that defaults to legacy single-file behavior; multi-slot only activates when explicitly enabled, so a broken release can be disabled without a code revert. State this explicitly rather than relying on "the legacy fallback path stays" as an implicit safety net. |

---

## Notes

- Step 1 is design-only; no code changes until the architecture is approved.
- Steps 2–3 are the structural core — everything else builds on them.
- Steps 4 and 5 are independent of each other (both depend on 2+3).
- Step 6 is pure UI — can be deferred if the underlying system ships first.
- Step 7 should be written incrementally alongside Steps 2–5, not only at the end.
- The existing checksum sweep tests (in `Ashfall.Core.Tests/SaveStoreChecksumSweepTests.cs`;
  the actual top-level test class in that file is named `ExpeditionSaveChecksumTests`, not
  `SaveStoreChecksumSweepTests` — the file/class name mismatch is pre-existing and should be
  fixed opportunistically if this batch touches that file, but is not itself part of this
  batch's scope) must continue passing throughout.
- This batch does NOT change the save wire format — each store's JSON content is identical;
  only the directory it lives in changes. Exception: `GreenhouseSaveStore`, whose format would
  need to change if brought into checksum conformance (see corrected-count note) — flag this
  exception explicitly rather than letting the blanket "wire format unchanged" claim mislead
  reviewers.
- Cloud save sync is out of scope for this batch (future work once multi-slot is stable).


---

## Review Notes (Corrected)

**Review method:** direct enumeration against the codebase at
`/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War` (not against other planning
documents). Ground truth commands used:
`grep -rhoE "public static class \w+SaveStore" src/ | sort -u` (30 results) and
`grep -rn 'public const string FileName' src/` (28 direct hits; the remaining 2 —
`GreenhouseSaveStore` and `StartingLevelSaveStore` — use `SavePath`/`SaveFileName` variants
instead of a `FileName` const, confirmed by reading both files directly).

### Factual errors found and corrected
1. **Save store count was stale at "22."** The real count is **30**, confirmed by direct file
   enumeration. This document's own dependency chain shows the drift over time: Batch 65
   (an earlier roadmap entry, already reviewed once) corrected the same "22" claim to "25" and
   listed all 25 stores by name. Five more stores (`GreenhouseSaveStore`, `JournalSaveStore`,
   `ShelterAssignmentSaveStore`, `StartingLevelSaveStore`, `YearOfAshSaveStore`) have been added
   to the codebase since Batch 65 was written, bringing the total to 30. This is exactly in the
   "27–30" range flagged as plausible before this review — confirmed at the top of that range.
   All step-level references to "22" (directory listings, checksum concatenation math, deletion
   file counts, test descriptions) have been corrected to 30 (31 total files per slot including
   metadata.json).
2. **Store shape was wrong, not just the count.** The original Step 3 described stores as
   having constructors (`ExpeditionSaveStore(SaveContext context, ...)`). In the real code,
   every store is a `public static class` with static `TrySave`/`TryLoad` methods — there is no
   constructor to modify. `StartingLevelSaveStore` already has a `pathOverride` parameter
   precedent that the plan should have cited and followed instead of inventing a constructor-
   injection pattern that doesn't match the codebase's actual shape.
3. **`GreenhouseSaveStore` is a structural outlier** the original plan didn't account for: it
   uses `System.Text.Json.JsonSerializer` directly (not `IJsonSerializer`), has no checksum
   envelope, and uses a `SavePath` property instead of the `FileName`-const pattern every other
   store follows. Step 2's metadata-checksum math and Step 3's "modify every store uniformly"
   framing both silently assumed uniformity that doesn't exist. This is now called out as a
   blocking decision (fix-first vs. exclude-and-track) rather than left to be discovered
   mid-implementation.
4. **Test file/class name mismatch:** the file is `Ashfall.Core.Tests/SaveStoreChecksumSweepTests.cs`
   but its actual top-level class is `ExpeditionSaveChecksumTests`. The plan (and AGENTS.md)
   refer to "`SaveStoreChecksumSweepTests`" as if it were the class name; scripts or reviewers
   grepping for that class name by itself will find nothing. Confirmed 12 `[Fact]`/`[Theory]`
   attributes in that file, matching the plan's "12 tests" claim — that specific number was
   already correct.

### Vague or unrunnable Done-when / verification criteria tightened
- Step 1: "complete design" replaced with an explicit, checkable requirement (names all 30
  stores, resolves the `GreenhouseSaveStore` question, reviewed by a second person).
- Step 3: "can write to arbitrary slot directories" replaced with "a passing test demonstrates
  each of the 30 stores independently" — the original criterion could be satisfied by testing
  one store and asserting success for all.
- Step 6: `godot --headless` scene instantiation "if feasible, otherwise visual smoke test" was
  not a runnable command or a pass/fail criterion — replaced with a requirement to either define
  a named self-test verb (following this project's existing `--*-selftest` convention in
  `src/Main.cs`) or explicitly use a manual QA checklist, but not both framed as interchangeable.
- Step 7: "removes exactly 22 store files + 1 metadata file (23 total)" corrected to the real
  30/31 count.

### Missing risk/rollback added
- **Partial migration risk:** Step 3 migrating "most" of 30 stores while missing a few would
  silently drop state for the missed stores — added as an explicit risk with a per-store
  checklist mitigation.
- **Rollback path:** the original risk table had no rollback story if Steps 2–3 ship a
  regression. Added: gate multi-slot behind a config flag defaulting to legacy single-file
  behavior, so a broken release can be disabled without a revert.
- **Cross-batch conflict surfaced:** Step 4's casual suggestion to "consider async write" for
  auto-save collides directly with Batch 80 (Thread Safety Audit), which documents that this
  codebase has zero async I/O today and treats introducing it as requiring a threading contract
  first. This batch should either explicitly defer async auto-save until after Batch 80's
  groundwork lands, or drop the async suggestion entirely until the >100ms budget is measured
  and shown to be exceeded. As written, an implementer could add the project's first async I/O
  path here without any of the safeguards Batch 80 is meant to establish.

### Ordering / dependency notes
- Step 3's risk level was raised from Medium to Medium-High given the real store count (30, not
  22) and the one non-conforming store, which increases both surface area and structural
  complexity beyond the original estimate. The stated 6–8 session effort estimate should be
  revisited against the corrected scope before this batch is scheduled.
