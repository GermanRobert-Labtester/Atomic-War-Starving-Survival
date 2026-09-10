# ENGINEERING SAVE COMPATIBILITY

## 1. Save System Invariant Analysis
Plan 148 activates the subterranean engineering emergencies and maintenance logs strictly as a read-only data catalog and projection layer.

### Core Save Guarantees
1. **Zero New Save Stores**: No new save store (e.g. `MaintenanceSaveStore`) is introduced. The catalog persists no mutable state.
2. **Zero Schema Mutations**: Existing save files (`HoldfastSaveEnvelope`, `CampaignSave`, `JournalSaveStore`, `WorldSaveStore`) remain byte-for-byte compatible.
3. **No Save Checksum Impact**: Because no fields are added to any serialized save DTOs, `SaveChecksum` hashes are untouched and existing saves pass integrity verification.
4. **Discovery Persistence via Existing Infrastructure**: When an engineering log or glitch is discovered by the player, the discovery state is recorded into the existing `JournalSystem` knowledge ledger using canonical keys:
   `KnowledgeKeys.NarrativeDiscovered(discoveryId)`
   This mechanism is already fully tested, checksummed, and versioned within `JournalSaveStore`.

---

## 2. Cross-Host Compatibility
- The data files (`bunker_maintenance_glitches.json`, `engineering_logs_expansion.json`, etc.) live under `Assets/StreamingAssets/Data/narrative/`.
- Both headless test suites and runtime Godot nodes load these files through `IFileIO` and `IJsonSerializer` via `BunkerMaintenanceCatalog.LoadFromDirectory()`.
- Idempotent loading guarantees that restarting a scene or reloading a game session never duplicates in-memory log entries.

---

## 3. Migration & Rollback Safety
- **Upgrade Path**: Old saves loaded into a build containing Plan 148 will immediately gain access to the engineering logs and glitch queries without any migration script.
- **Rollback Path**: If the binary is rolled back to a previous revision, saves created under Plan 148 will load without error, as no unknown envelope keys or serialized objects were injected into the save file.
