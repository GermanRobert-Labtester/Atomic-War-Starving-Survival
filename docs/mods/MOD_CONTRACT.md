# ASHFALL JSON Mod & Content-Pack Contract
# mod_contract_version: 1
# Generated from: Plan 47 / C1[15] — The Mod & Content-Pack Contract
# Authority: Assets/Ashfall.Core/Mods/JsonModLayering.cs + ModCompatibilityEvaluator.cs
# Last updated: 2026-09-18

---

## Surface

Mods and content packs live outside the shipped authority, by default at
`user://mods` (or the `ASHFALL_MODS_DIR` override). Each mod is a directory
with one `manifest.json` and one or more top-level catalog JSON files.

---

## Manifest Schema — `manifest.json`

All fields except `schema_version`, `mod_id`, and `catalogs` are optional
and have stable default values for backward compatibility.

```json
{
  "schema_version": 1,
  "mod_id": "sample_supply",
  "display_name": "Sample Supply",
  "version": "1.0.0",
  "load_order": 20,
  "allow_overrides": false,
  "catalogs": ["items.json"],
  "game_range": ">=1.0 <2.0",
  "mod_contract_range": ">=1 <2",
  "pack_type": "mod",
  "dependencies": []
}
```

### Field Descriptions

| Field | Type | Default | Description |
|---|---|---|---|
| `schema_version` | int | — | **Required.** Must be `1`. Future versions will be signalled here. |
| `mod_id` | string | — | **Required.** Lowercase snake_case, max 48 chars, globally unique. |
| `display_name` | string | `""` | Human-readable display name for UI. |
| `version` | string | `"1.0.0"` | Semantic version of this mod. |
| `load_order` | int | `0` | Lower values are applied first. Ties broken by `mod_id` ordinal. |
| `allow_overrides` | bool | `false` | Whether this mod may replace existing base definitions by ID. |
| `catalogs` | string[] | — | **Required.** List of JSON filenames to overlay. No path traversal. |
| `game_range` | string | `"*"` (any) | Version range the game must satisfy. Uses `>=`, `<`, `^`, `~`, `*`. |
| `mod_contract_range` | string | `"*"` (any) | Required mod contract version range (integer-based). |
| `pack_type` | string | `"mod"` | `"mod"` (default) or `"content_pack"` (runs acceptance pipeline). |
| `dependencies` | string[] | `[]` | `mod_id`s that must be present and accepted before this one. |

**Aliases:** `id` maps to `mod_id`; `name` maps to `display_name`; `supported_game_range` maps to `game_range`; `supported_mod_contract_range` maps to `mod_contract_range`; `content_roots` maps to `catalogs`.

---

## Compatibility Governance

### Version Range Syntax

Supported operators: `>=`, `<=`, `>`, `<`, `==`, `=`, `^` (caret), `~` (tilde), `*` (wildcard).

Multiple clauses in a single `game_range` string are space-separated and **all must pass** (AND semantics).

| Example | Meaning |
|---|---|
| `>=1.2 <2.0` | Any version from 1.2 up to (not including) 2.0 |
| `^1.0.0` | `>=1.0.0 <2.0.0` |
| `~1.2.0` | `>=1.2.0 <1.3.0` |
| `*` or empty | Any version |

### Typed Rejection Codes

A rejected mod always receives a typed rejection code and a diagnostic message. **No silent skip.**

| Code | Meaning |
|---|---|
| `MissingManifest` | `manifest.json` missing |
| `InvalidManifestJson` | Manifest JSON is malformed |
| `UnsafeModId` | `mod_id` fails snake_case safety rules |
| `DuplicateModId` | Two mods share the same `mod_id` |
| `UnsupportedSchemaVersion` | `schema_version` is not 1 |
| `NoCatalogs` | `catalogs` array is empty or missing |
| `IncompatibleGameVersion` | Current game version fails `game_range` |
| `IncompatibleModContract` | Current contract version fails `mod_contract_range` |
| `MalformedVersionRange` | Range expression cannot be parsed |
| `MissingDependency` | A declared dependency is absent or was rejected |
| `CircularDependency` | A dependency cycle was detected |
| `UnsafeCatalogPath` | Catalog path contains `..` or other forbidden patterns |
| `MissingCatalogFile` | Catalog file absent from base or mod directory |
| `DuplicateDefinitionId` | Duplicate ID in a catalog's definition array |
| `DisallowedPrefix` | ID uses a prefix not in `CatalogIntegrityRules.IdPrefixes` |
| `OverrideNotAllowed` | Mod attempts to override a base ID without `allow_overrides: true` |
| `AcceptancePipelineFailed` | Content pack failed the content acceptance pipeline |

---

## Stability Classes

| Layer | Class | Mods May Depend On |
|---|---|---|
| `id`, `load_order`, `allow_overrides`, `catalogs` | **STABLE** | Yes — will not be removed without deprecation notice |
| `game_range`, `mod_contract_range` | **STABLE** | Yes — version semantics are locked |
| `pack_type`, `dependencies` | **STABLE** | Yes |
| `display_name`, `version` | **STABLE** | Yes — informational only, no gameplay effect |
| Internal catalog merge algorithm (field order) | **INTERNAL** | No |
| Save envelope format | **INTERNAL** | No — packs cannot add save-section keys |
| Godot scene tree, input maps, shaders | **FORBIDDEN** | — |

---

## Catalog Rules

Catalog files must:
- already exist in `Assets/StreamingAssets/Data/`;
- be plain `.json` filenames (no path traversal, no `/`, no `..`);
- contain the same `schema_version` as the base catalog;
- contain exactly one definition array at the root level.

Definitions replace by stable ID only when `allow_overrides: true`; otherwise they are new additions.

New IDs must use `CatalogIntegrityRules.IdPrefixes`.

---

## Overlay Ordering & Determinism

Accepted manifests sort by `load_order` (ascending), then ordinal `mod_id` (ascending).

Within each mod, catalog entries sort by ordinal ID (ascending) before being applied.

Dependency ordering is resolved before `load_order` — a dependent mod is always applied after its dependencies, regardless of `load_order`.

**Invariant:** same pack set + same base data + same game seed → identical effective content → identical campaign fingerprint.

---

## Content-Pack Acceptance

A pack with `pack_type: "content_pack"` runs the host-configured acceptance validator before its overlays are committed. If the validator fails, the entire pack is rejected atomically (no partial overlay).

This integrates with the Plan 45 / C1[14] `ContentAcceptancePipeline`. Regular mods (`pack_type: "mod"`) bypass this check.

---

## Locked Surfaces

Mods cannot:
- add C# or native code;
- reference arbitrary file paths;
- modify Core;
- modify save envelopes/checksums;
- change RNG stepping;
- change tick order;
- add new catalog filenames (only overlay existing ones);
- write into the shipped data directory.

---

## Save Compatibility

Saves store merged content state, not mod set identity. When a save is loaded without its original pack:
- missing definitions are surfaced in diagnostics;
- no crash;
- no corrupt save rewrite.

Packs cannot add new save-section keys — only `SaveSectionRegistry` owns those.

---

## Verification

```bash
# Run mod contract unit tests (Plan 47 coverage)
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj \
  --filter "FullyQualifiedName~Mods.Plan47ModContractTests"

# Run original mod layering tests
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj \
  --filter "FullyQualifiedName~Mods.JsonModLayeringTests"

# Build Godot host (compilation gate)
dotnet build Ashfall.csproj

# Runtime mod self-test
godot --headless --path . -- --mod-selftest

# Integrity selftest with mod overlay applied
godot --headless --path . -- --data-integrity-selftest
```

---

## Non-Goals

- Steam Workshop integration (deferred).
- Encrypted pack format.
- Per-pack save sections.
- Code/assembly execution.
- Arbitrary Godot scene replacement.
- Input-map hijacking.
- Shader injection.
