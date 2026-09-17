# D1 Flagship Integration Plan [9]
## Plan 165 — Modding Support & Mod Data Contract

> **Purpose:** Convert ASHFALL's existing JSON-heavy content architecture from "theoretically moddable" into a
> deliberate, deterministic, validated, documented, save-aware community data-mod platform.
>
> **Primary source:** Plan 165 — Modding Support & Mod Data Contract.
>
> **Core design problem:** ASHFALL already stores a large proportion of game content in JSON and already has
> catalog loaders plus a substantial integrity validator, but no mod installation convention, no manifest,
> no dependency graph, no deterministic overlay policy, no compatibility contract, no mod-aware save metadata,
> no tooling, and no statement of what external content is allowed to change.
>
> **Implementation posture:** data-only first, deterministic, schema-driven, fail-safe, namespace-safe,
> save-compatible, tooling-backed, headless-testable, and deliberately separated from arbitrary executable code.
>
> **Critical guardrail:** v1 mod support must not become an unmanaged plugin system. Mods may add or extend
> explicitly supported data catalogs and registered assets. They do not load arbitrary C# assemblies, native
> libraries, scripts, reflection targets, shell commands, or executable code.

---

## 1. Source Problem Statement

The source plan establishes a favorable base:

- game content already lives extensively under `Assets/StreamingAssets/Data/`;
- `CatalogIntegrityValidator.cs` validates content relationships;
- many `*CatalogLoader.cs` classes already deserialize and validate JSON;
- the project is therefore data-driven enough for controlled external overlays.

But data-driven is not the same as moddable.

Without a contract, a community modder cannot reliably answer:

- Where does a mod live?
- How is it identified?
- Which game versions does it support?
- What catalogs may it extend?
- May it replace an existing record?
- What happens if two mods define the same ID?
- What load order wins?
- How are dependencies expressed?
- How does a save remember which mods were active?
- What happens when a required mod is missing?
- How are assets referenced?
- How are broken mods diagnosed?
- How does a mod author validate a package before distribution?

The flagship implementation therefore introduces an explicit pipeline:

```text
Mods/ directory
    ↓
manifest discovery
    ↓
manifest/schema validation
    ↓
dependency graph + compatibility checks
    ↓
deterministic load order
    ↓
catalog-specific overlay/extension operations
    ↓
merged catalog validation
    ↓
content digest / active-mod fingerprint
    ↓
game bootstrap
    ↓
save metadata + diagnostics + UI
```

The merged result must look like ordinary validated game data to downstream systems.

---

## 2. Flagship Success Criteria

The implementation is complete only when all of the following are true:

1. A canonical mod root exists.
2. Every mod has a validated manifest.
3. Mod IDs are globally unique.
4. Mods use deterministic load order.
5. Dependency cycles are detected.
6. Required dependency versions are validated.
7. Game-version compatibility is validated.
8. Only registered data families are moddable.
9. Mods cannot load arbitrary executable code.
10. Mod IDs/content IDs use a namespace policy that prevents accidental collision.
11. Overlay/merge semantics are defined per catalog family.
12. Appending and overriding are not treated as the same operation.
13. Base data remains immutable; merged catalogs are derived.
14. The final merged content set is revalidated by the canonical integrity validator.
15. Save files record the active mod set and versions.
16. Loading a save with missing or changed mods produces a deterministic compatibility report.
17. Critical missing content blocks unsafe loading rather than silently fabricating replacements.
18. Old unmodded saves continue to load.
19. A no-mod run produces the same catalogs/digests as the pre-modding base game.
20. A headless `--modding-selftest` proves loading, dependency ordering, conflicts, merge semantics, and save
    compatibility.
21. Mod tooling can scaffold, validate, and package mods.
22. Example mods demonstrate supported operations without requiring code changes.
23. Documentation defines the stable public data contract and explicitly marks unstable/internal fields.
24. The mod manager is a projection/controller over the Core mod authority, not the owner of load logic.
25. The game can disable a broken optional mod and report why without corrupting base data.

---

## 3. Repository Reconnaissance Before Editing

Create `docs/modding/MODDING_INTEGRATION_AUDIT.md`.

Inspect at minimum:

- `Assets/StreamingAssets/Data/`
- `Assets/Ashfall.Core/CatalogIntegrityValidator.cs`
- every `*CatalogLoader.cs`
- shared JSON serializer options
- schema-version handling
- ID normalization rules
- localization catalogs/loaders
- asset registry/provenance systems
- save DTOs and save migration
- bootstrap/composition root
- content utilization selftests
- data-integrity selftest
- export/staging scripts
- platform-specific writable-data paths
- current use of `StreamingAssets`
- Linux/Windows path normalization behavior
- archive/zip helpers if any
- any existing overlay or data-pack support
- Plan 47-style data-pack/mod contracts if later repository work added them
- catalog hot-reload/debug tooling
- release artifact validation
- sandbox/security assumptions

For each catalog family, record:

| Catalog family | Loader | Root shape | Key/ID field | Can append? | Can replace? | Cross refs | Schema version |
|---|---|---|---|---:|---:|---|---|
| items | ... | ... | ... | | | | |
| locations | ... | ... | ... | | | | |
| quests | ... | ... | ... | | | | |

Do not create generic merge behavior until this inventory exists.

---

## 4. Scope Boundary

### In scope for v1

- data-only mods;
- JSON catalog additions;
- explicitly allowed record overrides;
- localization additions/overrides;
- registered image/audio assets where supported;
- manifest;
- semantic versions;
- game compatibility range;
- dependency graph;
- conflict reporting;
- deterministic load order;
- save mod fingerprint;
- mod manager;
- scaffold/validator/packager CLI;
- example mods;
- modding guide.

### Explicitly out of scope for v1

- arbitrary managed assemblies;
- arbitrary C# scripting;
- native DLL/SO loading;
- arbitrary executable scripts;
- arbitrary network code;
- runtime patching/hooking;
- unrestricted filesystem access;
- workshop/store integration;
- automatic internet download/update;
- code-mod sandbox;
- multiplayer compatibility.

This boundary drastically reduces crash, security, compatibility, and support risk.

---

## 5. Canonical Directory Layout

Development/source layout:

```text
Mods/
  <mod_id>/
    manifest.json
    data/
      ...
    assets/
      ...
    localization/
      ...
    README.md
```

Packaged/user runtime location may differ from repository root.

Audit exported-build write/read locations. In production, prefer a user-writable application data directory
rather than assuming the installation directory is writable.

Define one abstraction:

```csharp
public interface IModDirectoryProvider
{
    string GetInstalledModsDirectory();
}
```

Development may resolve to repository `Mods/`; exported builds resolve to platform user data.

---

## 6. Manifest Contract

Create `Assets/Ashfall.Core/Mods/ModManifest.cs`.

Recommended shape:

```csharp
public sealed record ModManifest
{
    public int ManifestSchemaVersion { get; init; }
    public string ModId { get; init; }
    public string Name { get; init; }
    public string Version { get; init; }
    public string Author { get; init; }
    public string Description { get; init; }
    public string GameVersionRange { get; init; }

    public IReadOnlyList<ModDependency> Dependencies { get; init; }
    public IReadOnlyList<ModDependency> OptionalDependencies { get; init; }
    public IReadOnlyList<ModContentDeclaration> Content { get; init; }

    public int Priority { get; init; }
    public string? Homepage { get; init; }
    public string? License { get; init; }
}
```

Keep manifest data declarative.

---

## 7. Manifest Schema Version vs Mod Version

Separate:

- `manifestSchemaVersion`: version of ASHFALL's manifest format;
- `version`: semantic version of the mod;
- `gameVersionRange`: compatible game versions.

Do not overload one number for all three.

Example:

```json
{
  "manifestSchemaVersion": 1,
  "modId": "example_survival_pack",
  "name": "Example Survival Pack",
  "version": "1.2.0",
  "gameVersionRange": ">=1.0.0 <2.0.0"
}
```

---

## 8. Mod ID Namespace

Require stable lower-case identifiers.

Recommended:

```text
author_or_group.mod_name
```

or project-standard snake case:

```text
author_mod_name
```

Content IDs added by a mod must be namespaced, for example:

```text
example_survival_pack.item_field_filter
example_survival_pack.quest_broken_well
```

If the base game only supports snake_case identifiers, use:

```text
mod_example_survival_pack_item_field_filter
```

Choose one convention and enforce it everywhere.

---

## 9. Content ID Ownership

A mod owns IDs under its namespace.

Rules:
- a mod may freely add IDs in its namespace;
- it may not claim another mod's namespace;
- base-game override requires explicit override declaration;
- dependency-provided extension requires declared dependency;
- two mods defining the same new ID is a hard conflict unless explicit replacement semantics exist.

This is stronger than "priority decides everything."

---

## 10. Content Declarations

Avoid scanning every JSON file blindly.

Manifest content declaration:

```json
{
  "content": [
    {
      "type": "catalog_patch",
      "catalog": "items",
      "path": "data/items.json"
    }
  ]
}
```

This lets the loader know:
- intended catalog family;
- expected schema;
- merge policy;
- validator.

---

## 11. Supported Mod Operations

Define explicit operations:

### `add`
Adds new record IDs only.

### `replace`
Replaces an existing full record. Use sparingly.

### `patch`
Changes explicitly permitted fields of an existing record.

### `remove`
Recommend **not supporting in v1** unless save/content systems are designed for missing definitions.

### `localize`
Adds/overrides localization keys.

Do not use one generic JSON deep-merge for every catalog.

---

## 12. Why Generic Deep Merge Is Dangerous

The source plan proposes:
- arrays append;
- objects override.

That is too ambiguous for a stable public contract.

Problems:
- arrays may represent ordered rules where appending changes semantics;
- nested objects may contain required invariants;
- replacing one field may invalidate another;
- mod authors cannot know whether a future schema change changes merge behavior.

Therefore merge semantics must be catalog-aware and operation-aware.

---

## 13. Catalog Adapter Contract

Introduce:

```csharp
public interface IModdableCatalogAdapter
{
    string CatalogId { get; }
    int SupportedSchemaVersion { get; }

    ModMergeResult Apply(
        BaseCatalogSnapshot baseCatalog,
        IReadOnlyList<ValidatedModPatch> patches);
}
```

Each catalog adapter defines:
- ID field;
- add rules;
- patchable fields;
- replace rules;
- validation;
- deterministic serialization.

This creates a real public mod API.

---

## 14. Base Catalog Immutability

Never mutate source base JSON on disk.

Pipeline:

```text
base JSON
 -> base catalog snapshot
 -> validate base
 -> apply mod operations in deterministic order
 -> merged snapshot
 -> validate merged
 -> publish to game systems
```

Diagnostics should be able to show which mod produced each final record.

---

## 15. Provenance Tracking

For each merged record, keep provenance:

```csharp
public sealed record ContentProvenance
{
    public string ContentId { get; init; }
    public string CatalogId { get; init; }
    public string SourceKind { get; init; } // base/mod
    public string SourceModId { get; init; }
    public string SourceModVersion { get; init; }
    public IReadOnlyList<string> AppliedPatchModIds { get; init; }
}
```

Useful for:
- conflict diagnostics;
- save compatibility;
- bug reports;
- mod manager.

---

## 16. Deterministic Load Ordering

Load order must never depend on:
- directory enumeration order;
- filesystem case behavior;
- archive order;
- dictionary iteration.

Recommended order:

1. dependency graph constraints;
2. numeric priority;
3. stable mod ID lexical tie-break.

Dependencies always load before dependents regardless of priority.

---

## 17. Dependency Graph

Represent dependencies:

```json
{
  "modId": "addon",
  "versionRange": ">=1.1.0 <2.0.0",
  "required": true
}
```

Validate:
- missing dependency;
- incompatible version;
- self-dependency;
- cycle;
- duplicate dependency declaration.

Use deterministic topological sort.

---

## 18. Dependency Cycles

Example:

```text
mod_a -> mod_b
mod_b -> mod_c
mod_c -> mod_a
```

This must be a hard load error for affected mods.

Diagnostics must show the cycle path.

---

## 19. Optional Dependencies

Optional dependencies allow compatibility patches.

Example:
- mod A can run alone;
- if mod B is active, A includes `compat/b_patch.json`.

Do not treat optional dependency absence as load failure.

Conditional content should be explicitly declared, not hidden in code.

---

## 20. Incompatibilities

Manifest may support:

```json
"incompatibleWith": [
  { "modId": "other_total_overhaul", "versionRange": "*" }
]
```

Use only when necessary.

Hard incompatibilities should block simultaneous activation before content merge.

---

## 21. Semantic Versioning

Use standard major/minor/patch rules for mods.

Guidance:

- PATCH: bug fixes/content wording that preserves contract;
- MINOR: backwards-compatible additions;
- MAJOR: save/content-breaking changes.

The game validates syntax; mod author follows semantic meaning.

---

## 22. Game Compatibility

Do not require exact game version only.

Use version ranges.

Example:
```text
>=1.0.0 <1.3.0
```

If repository build IDs are more reliable than semantic game versions, support both:
- human-facing version range;
- optional content contract version.

A mod compatible with data contract v3 may survive several game patches.

---

## 23. Public Mod Contract Version

Introduce a stable:

```text
modDataContractVersion
```

This is distinct from game version.

The game can say:
- game 1.4.2;
- mod data contract 2.

A mod declares supported contract range.

This reduces needless breakage.

---

## 24. Conflict Taxonomy

Not all conflicts are equal.

### Hard conflict
- duplicate new ID;
- incompatible dependencies;
- two full replacements where policy forbids;
- invalid merged catalog.

### Soft conflict
- two mods patch same permitted field;
- later priority wins but player is warned.

### Informational overlap
- mods touch same catalog but different records.

UI should not label every overlap as dangerous.

---

## 25. Conflict Report

Create structured report:

```csharp
public sealed record ModConflict
{
    public ModConflictSeverity Severity { get; init; }
    public string CatalogId { get; init; }
    public string ContentId { get; init; }
    public string FieldPath { get; init; }
    public IReadOnlyList<string> ModIds { get; init; }
    public string Resolution { get; init; }
}
```

Expose to UI and support bundles.

---

## 26. Conflict Resolution Policy

Priority should resolve only explicitly permitted soft conflicts.

Do not automatically resolve:
- duplicate added IDs;
- schema incompatibility;
- dependency cycles;
- invalid references;
- executable content.

Those block affected mods.

---

## 27. Active Mod State

Separate installation from campaign activation.

```csharp
public sealed record ModConfiguration
{
    public IReadOnlyList<string> EnabledModIds { get; init; }
    public IReadOnlyDictionary<string, int> UserPriorityOverrides { get; init; }
}
```

Installation state belongs to environment/user configuration, not necessarily campaign save.

Campaign save records the exact active fingerprint.

---

## 28. Save Mod Fingerprint

Persist:

```csharp
public sealed record SaveModFingerprint
{
    public string ModId { get; init; }
    public string Version { get; init; }
    public string ContentDigest { get; init; }
}
```

Save stores sorted list plus merged-content digest.

This lets the loader classify compatibility.

---

## 29. Save Compatibility Classes

When loading:

### Exact
Same mods/versions/digests.

### Compatible candidate
Same required content IDs exist; version changed but manifest declares compatibility.

### Missing optional content
Mod removed, but save does not reference mod-owned IDs.

### Missing required content
Save references definitions from absent mod.

### Changed destructive content
Referenced ID removed or type changed.

### Unknown
Cannot prove safety.

Unsafe cases should require explicit recovery or block load.

---

## 30. "Saves Work Without Mods" Clarification

The source says saves should work without mods.

That is only safely true when the save does not depend on mod-owned definitions.

Example:
- a mod added item `mod_x_item`;
- save inventory contains that item;
- removing mod means definition is missing.

Do not silently delete it by default.

Provide:
- compatibility report;
- optional recovery/missing-content placeholder only if carefully designed;
- backup before destructive migration.

---

## 31. Missing Content Placeholder Strategy

Potential later compatibility aid:

```text
MissingContentRecord
- original catalog
- original ID
- source mod
```

Use only for non-critical inert content.

Never substitute placeholders for:
- active quests;
- survivor definitions;
- world topology;
- scripted required state;
- critical systems.

v1 may simply block unsafe load.

---

## 32. Mod-Aware Save Migration

Mods may ship data migrations only if a safe declarative migration format exists.

Do not allow arbitrary scripts.

Possible later operations:
- rename ID;
- remap enum/value;
- replace deprecated content ID.

For v1, support manifest `idAliases` if safe:

```json
"idAliases": {
  "old_mod_item": "mod_new_item"
}
```

Validate aliases and prevent loops.

---

## 33. Old Unmodded Saves

Old save:
- no mod fingerprint;
- treat as base-game mod set;
- load normally;
- write empty fingerprint on next save.

No migration should alter gameplay.

---

## 34. No-Mod Parity Gate

Critical test:

```text
base game before mod framework
==
base game with mod framework and zero active mods
```

Compare:
- catalog counts;
- content IDs;
- normalized data digest;
- integrity results.

This prevents mod infrastructure from changing vanilla behavior.

---

## 35. Mod Loader Lifecycle

Recommended:

```text
discover installed mods
 -> parse manifests
 -> validate manifests
 -> resolve enabled configuration
 -> validate compatibility/dependencies
 -> deterministic order
 -> validate declared files
 -> load base catalogs
 -> apply mod overlays
 -> validate merged catalogs
 -> compute digests/provenance
 -> publish final catalog set
```

Do not allow gameplay systems to load catalogs before the mod overlay completes.

---

## 36. Bootstrap Wiring

Expected order:

```text
Setup paths
Setup mod configuration
Resolve mod set
Build merged catalog source
Run integrity validation
Construct gameplay systems from merged catalogs
Restore campaign save
Verify save mod fingerprint
Continue boot
```

Save compatibility check may need to occur before fully restoring game state.

---

## 37. File Path Security

All manifest paths must be relative to mod root.

Reject:
- `../`;
- absolute paths;
- symlink escapes if relevant;
- drive-letter paths;
- URI schemes;
- NUL/invalid path data.

Normalize then assert resolved path remains under mod directory.

---

## 38. Archive Security

If packaged mods use zip archives, protect against:
- zip-slip (`../`);
- decompression bombs;
- excessive file count;
- huge uncompressed size;
- duplicate paths;
- unsupported executable extensions.

Prefer unpack-to-staging + validate + atomic install.

---

## 39. Executable Content Ban

Reject or ignore:

- `.dll`
- `.so`
- `.dylib`
- `.exe`
- arbitrary scripts
- native plugins

unless a future code-mod plan introduces a sandboxed contract.

Document this clearly.

---

## 40. Asset Support

If v1 supports images/audio:

Manifest declares assets explicitly.

Example:
```json
{
  "type": "asset",
  "assetKind": "image",
  "id": "mod_x_portrait_y",
  "path": "assets/portrait.png"
}
```

Validate:
- extension;
- size;
- ID namespace;
- consumer compatibility.

No arbitrary engine resource/script import.

---

## 41. Asset Registry Integration

If project has `asset_registry.json` or equivalent, create a runtime merged registry.

Mod assets get provenance:
- mod ID;
- version;
- relative path;
- hash.

Do not modify base asset registry on disk.

---

## 42. Localization Mods

Support localization overlays explicitly.

Rules:
- mod namespace keys are free to add;
- overriding base strings requires explicit declaration;
- invalid locale code fails validation;
- placeholder token sets must match;
- deterministic priority resolves permitted overrides.

Localization should not require editing base files.

---

## 43. Supported Catalog Families — Tiering

Not every catalog should be public on day one.

### Tier 1 — additive, relatively safe
- items;
- broadcasts;
- journal/codex entries;
- minor encounters;
- localization.

### Tier 2 — structured gameplay
- locations;
- quests;
- factions;
- recipes;
- research nodes.

### Tier 3 — high-risk topology/contracts
- world topology;
- endings;
- save contracts;
- system rules.

Start with Tier 1 plus carefully validated Tier 2.

---

## 44. Public vs Internal Fields

For each moddable catalog, document:

- stable public fields;
- optional fields;
- enum values;
- references;
- limits;
- fields explicitly not guaranteed.

Do not tell modders "copy any base JSON and change anything" if internal fields may change.

---

## 45. JSON Schemas

Generate or maintain JSON Schema files under:

```text
docs/modding/schemas/
```

Examples:
- `mod_manifest.schema.json`
- `items_mod.schema.json`
- `quests_mod.schema.json`

Validator CLI uses same schema/semantic validation as game.

---

## 46. Semantic Validation Beyond JSON Schema

JSON Schema catches shape, not world validity.

Reuse `CatalogIntegrityValidator` to catch:
- missing IDs;
- invalid references;
- duplicate IDs;
- impossible ranges;
- unresolved localization;
- missing dependent catalog entries.

Mod validation must include both schema and semantic checks.

---

## 47. Merged Validation

Validate mod fragments individually, then validate final merged world.

Why:
- two individually valid mods may conflict together;
- a mod may rely on dependency additions;
- overrides may break references.

Final validation is authoritative.

---

## 48. Error Isolation

A broken optional mod should not necessarily destroy the whole application boot.

Flow:
- validate each mod;
- disable invalid mod and dependents;
- build compatibility report;
- if campaign save requires disabled mod, block campaign load;
- base game remains available.

For new game/main menu, safe recovery is valuable.

---

## 49. Safe Mode

Add a startup option:

```text
--safe-mode
```

or existing equivalent.

Safe mode:
- disables all external mods;
- boots base content;
- does not overwrite user's enabled configuration automatically;
- enables troubleshooting.

This is essential for support.

---

## 50. CLI Mod Directory Override

Developer/testing option:

```text
--mods-dir <path>
```

Only for CLI/test harness.

Production UI uses canonical path.

This enables isolated fixture testing.

---

## 51. Mod Manager UI

Panel shows:

- installed mods;
- enabled/disabled;
- version;
- author;
- compatibility;
- dependency status;
- conflicts;
- effective load order;
- whether restart/new campaign required;
- save compatibility warnings.

The UI does not perform merging itself.

---

## 52. Enable/Disable Semantics

Changing active mods should normally require:
- returning to main menu;
- catalog reload;
- or full restart.

Do not hot-enable mods during an active campaign unless architecture proves safe.

Display this clearly.

---

## 53. Load Order UI

User may adjust priority among mods without dependency violations.

Dependency constraints override user priority.

UI should show:
- requested priority;
- effective order;
- why a mod was moved.

Avoid free-form drag ordering that can violate dependencies silently.

---

## 54. Installation Workflow

For directory install:
1. user selects package/directory;
2. copy/unpack to staging;
3. validate manifest;
4. validate paths;
5. validate content;
6. calculate hashes;
7. atomically move to installed mods directory;
8. mark disabled by default or follow policy.

Never partially install into live mod directory.

---

## 55. Uninstall Workflow

Before uninstall:
- inspect saves? optionally;
- warn if current campaign depends on mod;
- disable first;
- keep backups if practical.

Do not edit saves during uninstall.

---

## 56. Update Workflow

Mod update:
- stage new version;
- validate;
- compare manifest;
- detect compatibility;
- preserve previous version until success;
- update atomically.

If a save requires old version and new is incompatible, warn/block.

---

## 57. Mod State vs Campaign State

Installed/enabled configuration is user/environment state.

Campaign save stores fingerprint.

Do not put the full mod installation database inside every campaign save.

---

## 58. Deterministic Mod Set Digest

Compute digest from sorted:

```text
modId
version
content hashes
effective order
```

Store:
- active mod set digest;
- merged catalog digest.

Use in:
- saves;
- support reports;
- selftests.

---

## 59. Content Hashing

Hash actual normalized content files, not timestamps.

This detects:
- local manual edits without version bump;
- corrupted files;
- different packages claiming same version.

Display "modified/unverified local contents" when version same but digest differs.

---

## 60. Mod Provenance in Bug Reports

Support bundle should include:
- game version;
- mod contract version;
- enabled mod IDs/versions;
- digests;
- conflicts;
- validation warnings;
- effective order.

Do not include arbitrary user file contents.

---

## 61. Mod Template Generator

Tool:

```text
python3 tools/modding/new_mod.py --id author.example --name "Example"
```

Generates:
- manifest;
- data directory;
- README;
- sample catalog patch;
- validation config.

Use repository language/tooling conventions.

---

## 62. Mod Validator CLI

Tool:

```text
python3 tools/modding/validate_mod.py <path>
```

Checks:
- manifest schema;
- version syntax;
- path safety;
- namespace;
- data schemas;
- semantic references;
- dependency declarations;
- asset constraints.

Exit non-zero on error.

---

## 63. Mod Packager

Tool:

```text
python3 tools/modding/package_mod.py <path>
```

Steps:
- validate;
- build manifest lock/digest;
- exclude temp files;
- create archive;
- print contents and checksum.

Do not package executables.

---

## 64. Documentation Generator

Generate catalog field reference from DTO/schema where possible.

This reduces drift between code and `MODDING_GUIDE.md`.

Docs should mark:
- required;
- optional;
- stable;
- experimental;
- reference type.

---

## 65. Example Mod 1 — Item Pack

Demonstrates:
- manifest;
- add-only item definitions;
- localization;
- optional image asset;
- namespace;
- validation.

No base overrides.

---

## 66. Example Mod 2 — Location Pack

Demonstrates:
- location definition;
- required supporting references;
- localization;
- data integrity.

Only include if location contract is public and stable.

---

## 67. Example Mod 3 — Quest Pack

Demonstrates:
- quest ID;
- prerequisites;
- referenced location/item IDs;
- localization;
- dependency declaration if needed.

Must pass quest reachability/integrity checks.

---

## 68. Example Mod 4 — Compatibility Patch

Demonstrates safe override/patch between two example mods.

This is important because it teaches conflict resolution rather than only additions.

---

## 69. Modding Guide Structure

`docs/MODDING_GUIDE.md` should include:

1. What ASHFALL mods can/cannot do.
2. Installing mods.
3. Directory layout.
4. Manifest.
5. IDs/namespaces.
6. Supported catalog families.
7. Add/patch/replace semantics.
8. Dependencies.
9. Load order.
10. Localization.
11. Assets.
12. Save compatibility.
13. Versioning.
14. Validation.
15. Packaging.
16. Troubleshooting.
17. Example mods.
18. Contract stability policy.

---

## 70. Mod Contract Stability Policy

Publish compatibility promise.

Example:
- fields marked stable will not change within a major mod-data-contract version;
- breaking schema changes increment contract major;
- deprecated fields remain supported for defined window;
- internal catalogs may be unsupported.

This is more important than raw feature count.

---

## 71. Deprecation

When a public field is deprecated:
- validator warns;
- docs mark replacement;
- continue loading for at least defined compatibility window;
- migration tooling may rewrite.

Do not abruptly delete public fields in patch releases.

---

## 72. Mod Configuration Files

The source asks "configure mods."

Do not provide arbitrary executable config.

Allow optional per-mod config schema:

```json
"configSchema": "config.schema.json"
```

Settings types:
- bool;
- number with bounds;
- enum;
- string with constraints.

Game passes values only to data selectors that explicitly support them.

This can be deferred from v1 if no consumer model exists.

---

## 73. Deterministic Config

Configuration affecting simulation must be:
- saved with campaign or included in mod fingerprint;
- deterministic;
- validated.

Changing config mid-campaign should trigger compatibility warning.

---

## 74. Security Boundary for Markdown/README

README is display/documentation only.

If rendered in UI:
- sanitize links;
- do not execute HTML/scripts;
- restrict local file navigation;
- do not auto-open arbitrary protocols.

Plain text/Markdown subset is safer.

---

## 75. Asset Size Limits

Set configurable limits:
- max individual image size;
- max audio size;
- max total mod package size for validator warnings;
- max manifest/data file size.

Purpose:
- avoid accidental memory/IO abuse.

Do not need DRM; just safety bounds.

---

## 76. Content Count Limits

Guard against pathological packages:
- max files;
- max records per catalog with warnings;
- max dependency depth.

Use generous limits.

---

## 77. Invalid Mod Recovery

If startup finds broken enabled mod:
- record error;
- disable affected mod for this boot or enter recovery screen;
- disable dependents;
- keep user configuration recoverable;
- offer safe mode.

Never delete mod automatically.

---

## 78. Campaign Mod Lock

Recommended:
once a campaign starts, store its active mod set.

If player changes mods:
- compare fingerprints on load;
- show compatibility report;
- require explicit confirmation for non-critical differences;
- block unsafe differences.

Do not silently run a campaign under different content.

---

## 79. New Campaign Setup

New campaign screen may show:
- active mod count;
- mod set name/preset;
- compatibility warnings.

This makes modded runs explicit.

---

## 80. Mod Presets

Optional follow-on:
save named enabled-mod/load-order sets.

Do not include in initial Core unless UI needs it.

Mod presets are environment configuration, not game content.

---

## 81. Catalog Loader Integration

Do not rewrite every gameplay system.

Prefer replacing loader source:

```text
old:
CatalogLoader -> StreamingAssets file

new:
MergedCatalogProvider -> base + mods -> validated catalog
```

Downstream receives same DTOs.

---

## 82. Loader Adapter Strategy

For each catalog:
1. read existing loader behavior;
2. extract parsing/validation into reusable path;
3. allow merged JSON/object input;
4. keep public DTO unchanged;
5. add parity test.

Avoid forking "base loader" and "mod loader" implementations.

---

## 83. Catalog Initialization Ordering

Cross-catalog references mean load order matters.

Better:
- parse all mod fragments;
- merge per catalog;
- create all merged snapshots;
- run global integrity validation;
- publish catalogs.

Do not validate a quest mod before its dependency's item additions are visible where semantic validation requires
the combined set.

---

## 84. Global Integrity Pass

The final validator should report errors with provenance:

```text
quest mod_x.quest_a references missing item mod_y.item_b
Source: mod_x 1.0.0
Dependency mod_y: missing
```

This transforms supportability.

---

## 85. Conflict Example — Same Field

Base:
```text
item_water.weight = 1
```

Mod A patches:
```text
weight = 2
```

Mod B patches:
```text
weight = 3
```

If field is patchable:
- deterministic priority picks winner;
- report soft conflict;
- provenance records both.

If field is not patchable:
- reject.

---

## 86. Conflict Example — Duplicate Addition

Mod A:
`mod_shared_item`

Mod B:
`mod_shared_item`

Hard conflict unless ownership/dependency contract explicitly allows replacement.

Priority alone should not hide it.

---

## 87. Dependency Example

`quest_pack` depends on `item_pack >=1.2`.

If `item_pack` absent:
- quest pack disabled;
- clear message;
- no partial loading.

If wrong version:
- compatibility error.

---

## 88. Save Compatibility Example — Removed Item Mod

Save inventory contains:
`author.pack.item_field_radio`.

Mod absent.

Expected:
- compatibility check detects reference;
- campaign load blocks or uses explicitly supported placeholder policy;
- save is not silently rewritten.

This protects player data.

---

## 89. Save Compatibility Example — Cosmetic Localization Mod Removed

No structural IDs from mod are persisted.

Expected:
- warning optional;
- load safe;
- base strings/assets used.

Classify as non-critical.

---

## 90. Mod Manager Diagnostics

For each mod show:
- valid;
- disabled;
- missing dependency;
- incompatible;
- conflicting;
- modified locally;
- required by current save.

This is much more useful than generic "mod failed."

---

## 91. Headless Modes

Support:

```text
--mods-dir <path>
--disable-mods
--validate-mod <path>
--modding-selftest
```

CI can run fixtures without UI.

---

## 92. Data Integrity Self-Test Integration

Extend standard selftest to:
- validate built-in mod schemas/tools;
- validate example mods;
- validate no-mod merged parity;
- validate public schemas match DTO expectations.

Do not require user-installed mods in ordinary CI.

---

## 93. Dedicated `--modding-selftest`

Selftest should:

1. boot with no mods;
2. verify vanilla digest;
3. load additive item fixture;
4. load additive location/quest fixture where supported;
5. load dependency fixture;
6. verify topological order;
7. detect missing dependency;
8. detect cycle;
9. detect duplicate ID;
10. verify soft patch conflict ordering;
11. verify merged integrity;
12. save with mod fingerprint;
13. reload exact mod set;
14. detect missing required mod;
15. verify old unmodded save;
16. verify safe mode;
17. exit non-zero on any mismatch.

---

## 94. Unit Test Matrix

### Manifest
- valid;
- missing ID;
- invalid version;
- invalid game range;
- invalid path;
- duplicate content declarations.

### Dependencies
- simple chain;
- multiple dependencies;
- optional dependency;
- missing;
- wrong version;
- cycle.

### Ordering
- priority;
- dependency overrides priority;
- lexical tie-break;
- platform consistency.

### Merge
- add;
- patch allowed;
- patch forbidden;
- replace;
- duplicate add;
- two patches.

### Save
- empty fingerprint;
- exact;
- version changed;
- mod missing;
- digest changed.

### Security
- path traversal;
- absolute path;
- executable file;
- archive traversal if archive support.

---

## 95. Golden Mod Fixture Corpus

Create test mods under:

```text
Ashfall.Core.Tests/Fixtures/Mods/
```

Suggested:
- `valid_items_mod`
- `valid_quest_mod`
- `dependency_base`
- `dependency_child`
- `missing_dependency`
- `dependency_cycle_a/b`
- `duplicate_id_a/b`
- `soft_patch_a/b`
- `invalid_manifest`
- `invalid_reference`
- `path_traversal`
- `forbidden_executable`
- `save_required_mod`

---

## 96. Property / Fuzz Testing

Generate manifest graphs.

Properties:
- deterministic order;
- dependency precedes dependent;
- cycle detected;
- no duplicate final IDs;
- merged catalog always validates before publication;
- path normalization never escapes mod root;
- no executable files accepted.

Use fixed seeds in CI.

---

## 97. Performance Budget

Mod loading happens at boot.

Requirements:
- manifests parsed once;
- hashes cached when file metadata unchanged if safe;
- no repeated disk scan during gameplay;
- merged catalogs cached;
- no runtime per-frame mod lookup.

Measure:
- 0 mods;
- 10 mods;
- 50 small mods;
- one large content pack.

Set reasonable boot-time budget.

---

## 98. Memory Budget

Avoid storing:
- raw JSON for every mod after parse if unnecessary;
- duplicate full base catalogs per mod.

Use:
- base snapshot;
- ordered patch operations;
- final merged catalog;
- compact provenance.

---

## 99. Logging

Structured diagnostics:

```text
ModDiscovered id=<id> version=<v>
ModDisabled id=<id> reason=<code>
ModDependencyResolved id=<id> dependency=<id>
ModConflict severity=<x> catalog=<id> content=<id>
ModSetResolved digest=<hash> count=<n>
MergedCatalogValidated errors=0
SaveModCompatibility status=<status>
```

Avoid logging arbitrary README/file contents.

---

## 100. Mod Journal — Reject as In-World Feature

The source proposes a "mod journal" and in-game narrative events like "The Installation."

Do **not** put technical mod installation/conflict events into the survivor in-world journal by default.

They belong in:
- mod manager;
- system log;
- support diagnostics.

Modding is a player/tooling layer, not canonically a shelter-world event unless intentionally designed as a
meta joke.

This is an important boundary correction.

---

## 101. Mod Quest Hooks — Reject From Core Mod Framework

Likewise, quests such as "The Modder" or "install a mod pack" should not be part of the core in-world simulation
unless ASHFALL explicitly embraces fourth-wall-breaking content.

Keep modding infrastructure separate from diegetic quest state.

Example mods may add ordinary game-world quests.

---

## 102. Tutorial Model

First-use mod manager help may explain:
- install location;
- enable/disable;
- dependencies;
- save compatibility;
- safe mode.

This is product help, not a gameplay tutorial.

---

## 103. Platform Packaging

Exported builds must include:
- public schemas/docs if desired;
- default empty mod directory creation or path initialization;
- no assumption that StreamingAssets is writable.

Test on Linux and Windows export targets.

---

## 104. Case Sensitivity

Linux is case-sensitive; Windows commonly is not.

Normalize IDs case-insensitively or require lowercase.

Paths remain platform-normalized.

Validator should reject mod packages whose files differ only by case if that can break cross-platform use.

---

## 105. Line Endings / Encoding

Require UTF-8 JSON.

Reject invalid encodings.

Normalize text digests in a defined way or hash raw bytes consistently.

Document UTF-8 requirement.

---

## 106. Deterministic JSON Normalization

For content digests:
- parse;
- canonical property ordering;
- normalized number/string representation if practical.

Or hash raw package files plus manifest lock.

Choose one and test cross-platform.

---

## 107. Mod Lockfile

Optional generated:

```text
mod.lock.json
```

Contains:
- manifest digest;
- file hashes;
- package digest.

Useful for package verification and support.

Do not require cryptographic signing in v1.

---

## 108. Trust and Signing — Follow-On

Future platform/workshop distribution may add:
- signatures;
- trusted publisher metadata;
- repository checksums.

Do not conflate with v1 local mod loading.

---

## 109. Tooling Exit Codes

CLI tools must:
- return 0 on success;
- non-zero on validation/package failure;
- print machine-readable JSON option for CI.

Example:

```text
validate_mod --json
```

---

## 110. CI Fixture Integration

Run:
- example mod validation;
- package/unpack round-trip;
- loader selftest;
- no-mod parity;
- dependency graph tests.

Keep fast tests small; large mod corpus can run nightly.

---

## 111. Mod API Documentation Version

Every generated guide should state:

```text
Game version
Mod data contract version
Schema build/hash
```

This allows modders to match docs to game.

---

## 112. Public Changelog

Maintain:
`docs/modding/MOD_DATA_CONTRACT_CHANGELOG.md`

Document:
- new catalogs;
- new stable fields;
- deprecations;
- breaking changes;
- migration notes.

---

## 113. Development-Only Mod Reload

If developers need faster iteration:
- reload from main menu/debug command;
- rebuild merged catalogs;
- validate;
- do not apply mid-campaign unless explicitly safe.

Mark unsupported in release UI.

---

## 114. Content Utilization With Mods

Existing content-utilization tools should optionally report:
- base content;
- mod content;
- selected/effect-produced status;
- source mod.

This helps mod authors know whether their definitions are wired.

---

## 115. Mod Reachability Validation

For quests/locations/encounters, structural validity is not enough.

Where existing reachability validators exist:
- run them on merged catalogs;
- attribute failures to source mod;
- reject critical unreachable content if policy requires.

Do not promise reachability where no validator exists.

---

## 116. Mod Balance

The framework should not attempt to enforce game balance beyond sanity constraints.

Mods may be intentionally overpowered.

Validator enforces:
- legal ranges;
- non-crashing values;
- references.

Documentation can warn that modded balance is mod-author responsibility.

---

## 117. Save Integrity vs Balance

A mod can be wildly unbalanced and still save safely.

Do not mark "powerful item" as compatibility error.

Separate:
- technical validity;
- gameplay balance.

---

## 118. Error Severity Levels

Use:
- info;
- warning;
- error;
- fatal.

Examples:
- deprecated field: warning;
- soft patch overlap: warning;
- missing optional dependency: info/warning;
- missing required dependency: error;
- invalid merged catalog: fatal for affected mod set.

---

## 119. Mod Disable Cascade

If mod B depends on A and A is disabled:
- B becomes disabled/inactive;
- explain cascade.

Do not leave B half-loaded.

---

## 120. Configuration Persistence

Store enabled mods/load-order configuration separately from campaign save.

Example:
`user://mods/config.json`

Exact location follows application settings authority.

Campaign fingerprint remains immutable historical record.

---

## 121. Read-Only Base Content

Mods cannot overwrite files under `Assets/StreamingAssets/Data/`.

All changes are virtual overlays.

This simplifies uninstall, patching, and recovery.

---

## 122. User-Created Loose Mods vs Packaged Mods

Support development loose directories first.

Packaged archive support can use same manifest/data contract.

Loader normalizes both into one `ResolvedModPackage`.

---

## 123. `ResolvedModPackage`

```csharp
public sealed record ResolvedModPackage
{
    public ModManifest Manifest { get; init; }
    public string RootPath { get; init; }
    public string PackageDigest { get; init; }
    public IReadOnlyList<ResolvedModFile> Files { get; init; }
}
```

Only validated packages enter dependency/merge stages.

---

## 124. Mod Loader State Machine

```text
discovered
 -> manifest_valid
 -> dependency_valid
 -> content_valid
 -> ordered
 -> merged
 -> active
```

Failure states:
- invalid_manifest;
- incompatible;
- missing_dependency;
- conflict;
- invalid_content.

UI can project this cleanly.

---

## 125. Implementation Phase A — Audit and Contract Definition

Tasks:
1. inventory catalog loaders;
2. classify public-safe catalogs;
3. document ID policies;
4. define mod root/provider;
5. define manifest;
6. define mod contract version;
7. define content operation declarations;
8. define security boundary.

Exit:
public contract draft exists before loader code.

---

## 126. Implementation Phase B — Discovery and Manifest Validation

Tasks:
1. directory scan;
2. manifest parse;
3. schema validation;
4. version parser;
5. path normalization;
6. namespace validation;
7. package hashing;
8. diagnostics.

Exit:
installed mods are discovered safely but not yet merged.

---

## 127. Implementation Phase C — Dependency Resolver

Tasks:
1. required deps;
2. optional deps;
3. version ranges;
4. incompatibilities;
5. cycle detection;
6. deterministic topological order;
7. priority;
8. stable tie-break.

Exit:
effective load order is deterministic.

---

## 128. Implementation Phase D — Catalog Overlay Framework

Tasks:
1. base snapshot;
2. moddable catalog adapters;
3. add operation;
4. patch operation;
5. replace operation where allowed;
6. provenance;
7. conflict report;
8. merged validation.

Exit:
one or two Tier-1 catalogs work end to end.

---

## 129. Implementation Phase E — Expand Supported Catalogs

Add catalog families incrementally.

For each:
1. adapter;
2. schema;
3. stable-field documentation;
4. fixture;
5. conflict tests;
6. save-reference analysis.

Do not expose all catalogs at once.

---

## 130. Implementation Phase F — Save Compatibility

Tasks:
1. mod fingerprint DTO;
2. active-set digest;
3. save write;
4. exact load;
5. missing mod report;
6. changed version report;
7. old save;
8. critical reference detection.

Exit:
modded saves cannot silently drift.

---

## 131. Implementation Phase G — Mod Manager

Tasks:
1. list;
2. enable/disable;
3. dependencies;
4. conflicts;
5. effective order;
6. required-by-save badge;
7. safe mode;
8. accessibility.

Exit:
players can understand and recover mod configuration.

---

## 132. Implementation Phase H — Tools and Examples

Tasks:
1. scaffold;
2. validator;
3. packager;
4. docs generator;
5. item example;
6. location/quest example if supported;
7. compatibility patch example;
8. package round-trip test.

Exit:
a community author can create a mod without reading game source.

---

## 133. Implementation Phase I — Documentation

Publish:
- guide;
- manifest spec;
- schemas;
- catalog references;
- conflict rules;
- versioning;
- save rules;
- troubleshooting;
- contract changelog.

Exit:
public contract is usable and explicit.

---

## 134. Implementation Phase J — CI Hardening

Tasks:
1. no-mod parity;
2. `--modding-selftest`;
3. fixture mods;
4. path security;
5. dependency fuzz;
6. save compatibility tests;
7. Linux/Windows path tests;
8. performance.

Exit:
mod layer is deterministic and supportable.

---

## 135. Verification Commands

Baseline:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --modding-selftest
```

Add:
```bash
python3 tools/modding/validate_mod.py examples/mods/example_items
python3 tools/modding/package_mod.py examples/mods/example_items
```

Use repository-native wrappers if present.

---

## 136. Definition of Done — Flagship

### Foundation
- [ ] mod directory provider
- [ ] `ModLoader.cs`
- [ ] `ModManifest.cs`
- [ ] manifest schema
- [ ] mod data contract version
- [ ] deterministic loader

### Security
- [ ] relative paths only
- [ ] path traversal rejected
- [ ] arbitrary executable code rejected
- [ ] archive safety if archives supported
- [ ] asset constraints

### Dependencies
- [ ] version ranges
- [ ] required deps
- [ ] optional deps
- [ ] cycles
- [ ] incompatibilities
- [ ] deterministic order

### Data
- [ ] catalog-aware adapters
- [ ] add
- [ ] controlled patch
- [ ] controlled replace
- [ ] provenance
- [ ] conflicts
- [ ] merged integrity

### Save
- [ ] mod fingerprint
- [ ] active-set digest
- [ ] exact compatibility
- [ ] missing/changed mod report
- [ ] old saves
- [ ] no silent destructive load

### UI
- [ ] installed list
- [ ] enable/disable
- [ ] dependency/conflict status
- [ ] load order
- [ ] save-required warning
- [ ] safe mode

### Tools
- [ ] scaffold
- [ ] validator
- [ ] packager
- [ ] examples
- [ ] generated/reference docs

### CI
- [ ] no-mod parity
- [ ] selftest
- [ ] fixture corpus
- [ ] security tests
- [ ] save tests
- [ ] headless

---

## 137. Follow-On Task 165-A — Public Catalog Contract Expansion

Goal:
expand moddable catalogs only after v1 proves stable.

Substeps:
1. measure demand;
2. audit next catalog;
3. define stable fields;
4. build adapter;
5. schema/docs;
6. fixtures;
7. migration risk;
8. add contract changelog entry.

---

## 138. Follow-On Task 165-B — Declarative Mod Migrations

Goal:
support safe ID/data migrations without scripts.

Possible operations:
- rename ID;
- map enum;
- transform bounded field;
- retire content.

Requirements:
- versioned;
- deterministic;
- reversible where possible;
- tested against save fixtures.

---

## 139. Follow-On Task 165-C — Mod Presets

Goal:
allow named mod sets.

Substeps:
1. preset DTO;
2. enabled list;
3. priority;
4. digest;
5. UI;
6. campaign creation integration;
7. no save-authority duplication.

---

## 140. Follow-On Task 165-D — Workshop / Community Distribution

Goal:
connect the local mod contract to a distribution platform.

Prerequisite:
local packages, validation, dependency handling, and save fingerprint must already be stable.

Do not redesign the mod format for each store.

---

## 141. Follow-On Task 165-E — Signed Packages

Goal:
optional authenticity verification.

Substeps:
1. package digest;
2. signature metadata;
3. trusted publisher keys;
4. verification UI;
5. unsigned local mods remain possible if policy allows.

Not required for v1.

---

## 142. Follow-On Task 165-F — Code Mod Sandbox

Only if strongly justified later.

Would require a separate security architecture:
- API capability sandbox;
- versioned scripting API;
- resource limits;
- permission model;
- crash isolation.

Do not bolt arbitrary C# assembly loading onto this plan.

---

## 143. Final Guardrails

- No arbitrary C# assembly loading.
- No native plugin loading.
- No generic deep merge for every catalog.
- No filesystem-order load behavior.
- No priority resolution of hard conflicts.
- No base-file mutation.
- No silent missing-mod save repair.
- No deleting mod-owned save content without explicit migration.
- No dependency cycles.
- No path traversal.
- No executable files in data mods.
- No undocumented public fields.
- No exact-game-version lock when a contract range is sufficient.
- No mod manager business logic duplicating Core.
- No hot enabling during a live campaign unless proven safe.
- No fourth-wall "mod quests" in the core simulation by default.
- No assumption exported install directory is writable.
- No zero-mod gameplay drift.
- No mod package accepted without final merged catalog validation.

When complete, ASHFALL will have a real community data-mod contract rather than an accidental collection of JSON
files. Mod authors will know what is stable, what they can extend, how IDs and dependencies work, how conflicts
are resolved, how saves remain safe, and how to validate a package before sharing it. Players will gain a mod
manager and recovery path, while the base game retains deterministic, validated catalogs and a strict boundary
against arbitrary executable code.
