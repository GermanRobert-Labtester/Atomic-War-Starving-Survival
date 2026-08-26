---
name: ashfall-mod-contract
description: Defines and gates the modding boundary — JSON data authority is mod-safe, Core stays engine-agnostic, no mod breaks SaveChecksum or CatalogIntegrityValidator. Use when exposing new data, adding IDs, or reviewing mod PRs.
---

# ASHFALL Mod Contract

## ROLE
ASHSFALL's mod surface is `Assets/StreamingAssets/Data/` (authority) + `assets/` (presentation). Core (`netstandard2.1`) and save wire format are NOT moddable. You keep mods from forking authority or breaking deterministic saves.

## RULES
1. Invariant 6: Data authority is JSON with `schema_version`; never per-engine forks.
2. Mods may add `item_*`, `quest_*`, `loc_*`, `faction_*` etc. with validated prefixes — never new prefixes without validator update.
3. `SaveChecksum` (float G9, ordinal order, null/empty normalized) must remain stable; mods must not inject unsanitized fields into checksummed DTOs.
4. `ISeededRng` determinism holds even with mod data.

## WORKFLOW
### PHASE 1 — Surface Map
- Enumerate moddable JSON domains, allowed keys, ID prefixes, and loader (`*CatalogLoader.cs`) boundaries.
- Mark non-moddable: `SaveWireContract`, checksum, `ISeededRng` stepping, tick order.

### PHASE 2 — Validator Pin
- Run `godot --headless --path . -- --data-integrity-selftest` with mod sample (duplicate `StreamingAssets/Data` + mod overlay).
- Verify TIER-1/TIER-2 resolution still 0 errors, `schema_version` present, no duplicate ids across base+mod.

### PHASE 3 — Contract Test
- xUnit mod harness: load base+mod, `CaptureState → RestoreState` round-trip, mutate mod item and assert checksum changes; empty mod overlay loads clean.

### PHASE 4 — Verify
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` green, `dotnet build Ashfall.csproj` green.

## OUTPUT
`docs/mods/MOD_CONTRACT.md` — moddable vs locked matrix, ID prefix allowlist, validator result, contract test log, breaking-change policy.

## QUALITY GATE
- Mod sample passes validator 0 errors, round-trip + checksum pin green, no Core TFM/refs changed.
