# ASHFALL JSON mod contract

## Surface

Mods live outside the shipped authority, by default at
`user://mods` (or the `ASHFALL_MODS_DIR` override). Each mod is a directory
with one `manifest.json` and one or more top-level catalog JSON files.

```json
{
  "schema_version": 1,
  "mod_id": "sample_supply",
  "display_name": "Sample Supply",
  "version": "1.0.0",
  "load_order": 20,
  "allow_overrides": false,
  "catalogs": ["items.json"]
}
```

Catalog files must already exist in `Assets/StreamingAssets/Data`, must be
plain `.json` filenames without path traversal, and must contain the same
`schema_version` and one definition array as the base catalog. Definitions
replace by stable ID only when `allow_overrides` is true; otherwise they are
new additions. New IDs must use `CatalogIntegrityRules.IdPrefixes`.

## Locked surfaces

Mods cannot add C# or native code, reference arbitrary paths, modify Core,
modify save envelopes/checksums, change RNG stepping, change tick order, add
new catalog filenames, or write into the shipped data directory.

## Ordering and failure isolation

Accepted manifests sort by `load_order`, then ordinal `mod_id`. Definitions
are applied by ordinal ID order, while replacement keeps the base definition's
position. A malformed or unsafe mod is rejected without discarding other
valid mods. The host materializes accepted overlays into a private staging
directory and only then points existing catalog loaders at the effective
directory.

Settings persist `mods_enabled` and a sanitized `enabled_mods` allowlist.
An empty allowlist means all discovered valid mods are eligible.

## Verification

```bash
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj \
  --filter "FullyQualifiedName~Mods.JsonModLayeringTests"
dotnet build Ashfall.csproj
godot --headless --path . -- --mod-selftest
godot --headless --path . -- --data-integrity-selftest
```

The mod selftest proves valid acceptance, path rejection, deterministic
layering, and catalog-integrity validation of the staged result.
