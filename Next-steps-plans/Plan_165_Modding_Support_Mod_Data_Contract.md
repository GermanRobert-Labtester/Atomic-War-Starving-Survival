# Plan 165 — Modding Support & Mod Data Contract

## Goal

Create a modding support system and mod data contract that makes ASHFALL extensible by the community. Currently all game data is in JSON files under `Assets/StreamingAssets/Data/` which is theoretically mod-safe, but there is no mod loading infrastructure, no mod documentation, no mod tools, no versioning system for mods, and no clear contract for what can and cannot be modded. This plan adds proper mod support that enables community content creation while maintaining game integrity.

## Why

**Repository evidence:** All game data is in JSON files (items, locations, quests, factions, etc.) under `Assets/StreamingAssets/Data/`. `CatalogIntegrityValidator.cs` (603 lines) validates data integrity. `CatalogLoader` classes load JSON data. But there is no mod loading system, no mod directory, no mod manifest format, no documentation for modders, no tools for creating mods, and no versioning system. The data is mod-safe in theory but not in practice.

**What is missing:** Players cannot create or install mods. There is no mod directory, no mod manifest, no mod loading code, no documentation, no tools. The game cannot be extended by the community despite having data-driven architecture.

**Why existing plans don't solve it:** No plan addresses modding infrastructure. Plan 3 (schema_version sweep) adds versioning to data files but not for mods. Plan 12 (data integrity) validates data but doesn't support external mods. No plan addresses mod loading, mod tools, or mod documentation.

**Player value:** Enables community content creation (new items, locations, quests), extends game lifespan (user-generated content), creates modding community, and provides customization options.

## Files / Systems to Inspect

- `Assets/StreamingAssets/Data/` — all JSON data files
- `Assets/Ashfall.Core/CatalogIntegrityValidator.cs` — data validation
- `Assets/Ashfall.Core/` — all `*CatalogLoader.cs` files
- `Ashfall.csproj` — project configuration
- NEW: `Assets/Ashfall.Core/Mods/ModLoader.cs`
- NEW: `Assets/Ashfall.Core/Mods/ModManifest.cs`
- NEW: `Mods/` directory (root level)
- NEW: `docs/MODDING_GUIDE.md`

## Main Task 1 — Foundation / System Contract

1. Create `Mods/` directory at project root for mod installations
2. Create `ModLoader.cs` in `Assets/Ashfall.Core/Mods/`
3. Create `ModManifest.cs` in `Assets/Ashfall.Core/Mods/`
4. Define `ModManifest` DTO: `modId`, `modName`, `version`, `author`, `description`, `gameVersion` (compatible game version), `dependencies` (list of required mod IDs), `dataFiles` (list of JSON files provided by mod), `priority` (load order)
5. Define `ModState` DTO: list of installed mods, list of active mods, list of disabled mods, mod load order, mod conflicts
6. Implement `CaptureState/RestoreState` with schema versioning
7. Define mod manifest format:
   ```json
   {
     "modId": "example_mod",
     "modName": "Example Mod",
     "version": "1.0.0",
     "author": "ModAuthor",
     "description": "An example mod",
     "gameVersion": "1.0.0",
     "dependencies": [],
     "dataFiles": ["items.json", "locations.json"],
     "priority": 100
   }
   ```
8. Define mod loading mechanics:
   - Mods installed in `Mods/<modId>/` directory
   - Each mod has `manifest.json` at root
   - Mod data files override/extend base game data
   - Mods loaded in priority order (lower = earlier)
   - Dependencies checked before loading
   - Conflicts detected and reported
9. Define mod data contract:
   - Mods can add new items, locations, quests, factions, etc.
   - Mods can extend existing catalogs (add entries)
   - Mods cannot modify core game logic (C# code)
   - Mods must follow JSON schema (validated by `CatalogIntegrityValidator`)
   - Mods must use snake_case IDs with mod prefix (e.g., `mod_example_item`)
   - Mods cannot break save compatibility
10. Define mod versioning:
    - Mods have semantic version (major.minor.patch)
    - Game version compatibility checked
    - Mod dependencies version-checked
    - Breaking changes increment major version
11. Define mod conflict resolution:
    - Multiple mods modifying same data: priority order
    - Conflicts logged and reported to player
    - Player can disable conflicting mods
    - Critical conflicts prevent mod loading
12. Add deterministic loading: mod loading order is deterministic
13. Wire into `GameBootstrap`: `SetupMods`, `LoadMods`, `SaveModState`
14. Create mod UI: mod manager panel (install, enable, disable, configure)
15. Create `docs/MODDING_GUIDE.md`: comprehensive modding documentation

## Main Task 2 — Implementation / Loading / Validation / Tools / Documentation

1. Implement mod directory structure:
   - `Mods/<modId>/manifest.json` — mod manifest
   - `Mods/<modId>/data/` — mod data files (JSON)
   - `Mods/<modId>/assets/` — mod assets (images, audio)
   - `Mods/<modId>/README.md` — mod documentation
2. Implement mod loading:
   - Scan `Mods/` directory for installed mods
   - Load manifests, check dependencies
   - Validate mod data with `CatalogIntegrityValidator`
   - Load mod data in priority order
   - Merge mod data with base game data
   - Report conflicts and errors
3. Implement mod data merging:
   - Mod JSON files merged with base game files
   - Arrays: mod entries appended to base arrays
   - Objects: mod fields override base fields
   - IDs: mod IDs prefixed to prevent conflicts
   - Validation: merged data re-validated
4. Implement mod validation:
   - Manifest format validated
   - Data files validated against schemas
   - IDs checked for conflicts
   - Dependencies checked for availability
   - Game version compatibility checked
5. Implement mod conflict detection:
   - Multiple mods modifying same data detected
   - Conflicts logged with details
   - Player notified of conflicts
   - Critical conflicts prevent loading
6. Implement mod manager UI:
   - List installed mods
   - Enable/disable mods
   - Configure mod load order
   - View mod details and dependencies
   - Check for conflicts
   - Install mods from directory
7. Implement mod tools:
   - Mod template generator (scaffold new mod)
   - Mod validator (check mod before distribution)
   - Mod packager (create distributable mod archive)
   - Mod documentation generator
8. Create modding documentation:
   - `docs/MODDING_GUIDE.md`: comprehensive guide
   - Mod manifest format specification
   - Data file schema documentation
   - ID naming conventions
   - Best practices and examples
   - Troubleshooting common issues
9. Create mod examples:
   - Example mod: adds new items
   - Example mod: adds new location
   - Example mod: adds new quest
   - Example mod: extends existing catalog
10. Implement mod save compatibility:
    - Mods cannot break save format
    - Save files record active mods
    - Loading save with missing mods: warning
    - Loading save with different mod versions: migration
    - Saves work without mods (mod data optional)
11. Implement mod events:
    - "The Installation" — mod installed successfully
    - "The Conflict" — mod conflict detected
    - "The Update" — mod version updated
    - "The Dependency" — missing dependency detected
    - "The Load" — mods loaded for campaign
12. Add mod quest hooks:
    - "The Modder" — create and test custom mod
    - "The Collection" — install mod pack
    - "The Resolution" — resolve mod conflicts
    - "The Update" — update mods to new version
13. Add UI: mod manager panel with install, enable, disable, configure
14. Create mod journal: automatic log of mod events
15. Implement mod tutorial: first mod installation explains system

## Main Task 3 — Integration / Consequences / Validation

1. Wire into catalog loaders: mods extend base catalogs
2. Connect to `CatalogIntegrityValidator`: mod data validated
3. Integrate with save system: saves record active mods
4. Connect to game bootstrap: mods loaded at startup
5. Wire into UI system: mod manager panel
6. Connect to documentation system: modding guide accessible
7. Implement old-save compatibility: existing saves work without mods
8. Add deterministic loading: mod order is deterministic
9. Create exploit prevention: mods validated, cannot break game
10. Add tests: mod loading, validation, merging, conflicts, save round-trip
11. Verify catalog integrity: mod data validates against schemas
12. Test edge cases: no mods (base game), many mods (complex setup)
13. Verify headless behavior: mods load correctly without UI
14. Add data-integrity-selftest: mod manifests validate
15. Create `--modding-selftest` verb for CI validation

## State / System Interaction Model

```text
Modding support system
├─ Mod installation
│  ├─ Mods in Mods/<modId>/ directory
│  ├─ manifest.json at root
│  ├─ Data files in data/ subdirectory
│  └─ Assets in assets/ subdirectory
├─ Mod loading
│  ├─ Scan Mods/ directory
│  ├─ Load manifests
│  ├─ Check dependencies
│  ├─ Validate data
│  ├─ Load in priority order
│  └─ Merge with base data
├─ Mod data contract
│  ├─ Can add new content (items, locations, quests)
│  ├─ Can extend existing catalogs
│  ├─ Cannot modify core logic
│  ├─ Must follow JSON schema
│  ├─ Must use prefixed IDs
│  └─ Cannot break saves
├─ Mod validation
│  ├─ Manifest format validated
│  ├─ Data files validated
│  ├─ IDs checked for conflicts
│  ├─ Dependencies checked
│  └─ Game version checked
├─ Mod conflict resolution
│  ├─ Conflicts detected
│  ├─ Logged and reported
│  ├─ Player can disable
│  └─ Critical conflicts block
├─ Mod manager UI
│  ├─ List installed mods
│  ├─ Enable/disable
│  ├─ Configure load order
│  ├─ View details
│  └─ Check conflicts
└─ Integration
   ├─ Catalog loaders (extend catalogs)
   ├─ Integrity validator (validate data)
   ├─ Save system (record mods)
   ├─ Bootstrap (load at startup)
   └─ UI (mod manager)
```

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --modding-selftest
```

## Risk

**MEDIUM** — Modding complexity can overwhelm players if too many options exist. Risk of mods breaking game balance or save compatibility. Mitigation: strict validation, clear documentation, conflict detection, save compatibility checks, and examples to guide modders.

## Definition of Done

- `ModLoader.cs` and `ModManifest.cs` exist with full `CaptureState/RestoreState`
- `Mods/` directory created at project root
- Mod manifest format defined and validated
- Mod loading mechanics functional (scan, validate, load, merge)
- Mod data contract enforced (add/extend only, no core modification)
- Mod versioning and dependency system working
- Mod conflict detection and resolution
- Mod manager UI panel
- Mod tools (template generator, validator, packager)
- Comprehensive modding documentation (`docs/MODDING_GUIDE.md`)
- Example mods provided
- Save compatibility maintained
- Save/load round-trip tested with mods
- Deterministic mod loading verified
- Old saves load without error
- `--modding-selftest` verb for CI validation

## Follow-On Opportunities

- Mod workshop (community mod sharing platform)
- Mod categories (items, locations, quests, total conversions)
- Mod ratings and reviews
- Mod dependencies resolver (auto-install dependencies)
- Mod update notifications
