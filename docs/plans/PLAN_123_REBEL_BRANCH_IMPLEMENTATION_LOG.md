# Plan 123 — Rebel Branch Implementation Log

## Phase 1 — Reconnaissance

Status: PASS

Confirmed the live `RebelBranchCatalog`, `RebelBranchSystem`,
`RebelBranchIds`, string-based save codec, `FactionBranchCoordinator`, Plan
124 location override schema, Plan 89 epilogue input, and Plan 125 moral flag
schema. Recorded the existing Defector ID collision and the lack of live
branch predicates in the downstream catalogs.

## Phase 2 — Catalog and registry

Status: PASS

Changed:

- `Assets/StreamingAssets/Data/rebel_faction_branch.json`
- `builds/linux/Assets/StreamingAssets/Data/rebel_faction_branch.json`
- `Assets/Ashfall.Core/Factions/RebelBranchIds.cs`
- `Assets/Ashfall.Core/Factions/FactionBranchCoordinator.cs`

Added seven branches, 21 endings, seven PoNR flags, and the canonical
registry arms for IDs 9–15. Preserved the original eight rows and left the
save schema at version 1.

## Phase 3 — Tests and documentation

Status: PASS

Changed:

- `Ashfall.Core.Tests/RebelBranchExpansionTests.cs`
- `Ashfall.Core.Tests/RebelBranchSystemTests.cs`
- `docs/factions/PLAN_123_REBEL_FACTION_BRANCH_EXPANSION_CLOSEOUT.md`

Coverage includes exact count, baseline order, registry parity, uniqueness,
valid ranges, exact seven-band ending partitions, new-branch commitment and
PoNR flags, ending resolution, save round-trip, legacy restore, and
narrative safety.

## Phase 4 — Verification

Status: PASS

- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~RebelBranch"` — 31 passed.
- JSON parser, uniqueness, and partition scripts — passed.
- `git diff --check` on the scoped change — passed.

- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — 10,031 passed.
- `dotnet build Ashfall.csproj` — 0 warnings, 0 errors.
- `godot --headless --path . -- --data-integrity-selftest` — PASS, 0 errors and 0 warnings across 298 catalogs.
- `godot --headless --path . -- --content-utilization-selftest` — PASS.
- `godot --headless --path . -- --bridge-selftest` — PASS.
