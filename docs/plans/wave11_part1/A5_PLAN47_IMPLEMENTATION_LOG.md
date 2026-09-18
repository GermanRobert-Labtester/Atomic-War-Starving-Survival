# WAVE 11 PART 1 — TASK A5 IMPLEMENTATION LOG
## C1[15] Plan 47 — The Mod & Content-Pack Contract: Write Down the Boundary

### 1. Overview & Verification Summary
Task A5 delivers the Plan 47 mod compatibility governance, deterministic overlay contract, typed rejection, content-pack boundary, and updated `MOD_CONTRACT.md`. No loader rewrite. Extension only, under the existing `JsonModLayering` authority.

**Bounded outcome:** Game version range gating, mod contract version range gating, typed rejection codes for all failure modes, dependency ordering (topological sort), circular-dependency detection, content-pack acceptance pipeline integration seam, and a fully updated `MOD_CONTRACT.md` with stability classes.

**Non-goals (preserved):** Steam Workshop; encrypted packs; arbitrary Godot scene replacement; input-map hijacking; per-pack save sections; any loader rewrite.

### 2. Implementation Matrix

| Contract Axis | Existing Behavior | New Delta | Typed Rejection Code | Test |
|---|---|---|---|---|
| **Game version range** | None — all versions accepted | `game_range` / `supported_game_range` field parsed by `ModCompatibilityEvaluator.EvaluateGameVersion` | `IncompatibleGameVersion` / `MalformedVersionRange` | `Plan47ModContractTests.TooOldGameVersion_RejectedWithTypedCode` |
| **Mod contract range** | None | `mod_contract_range` / `supported_mod_contract_range` parsed by `ModCompatibilityEvaluator.EvaluateModContractVersion` | `IncompatibleModContract` | `Plan47ModContractTests.IncompatibleModContractVersion_RejectedWithTypedCode` |
| **Dependency resolution** | None — no `dependencies` field | Topological sort; missing dep → `MissingDependency`; cycle → `CircularDependency` | `MissingDependency` / `CircularDependency` | `Plan47ModContractTests.ModWithMissingDependency_*`, `CircularDependency_*` |
| **Deterministic overlay order** | `load_order` asc → `mod_id` ordinal (no dep sort) | Dependencies resolved first, then `load_order` asc → `mod_id` asc | N/A — correctness | `Plan47ModContractTests.LoadOrderDeterminesOverlayOrder_*` |
| **Typed rejection codes** | Only message strings | `ModRejectionCode` enum on every `ModDiagnostic` | All 17 codes | Full `Plan47ModContractTests` suite |
| **Content-pack acceptance seam** | None | `pack_type: "content_pack"` triggers `customPackAcceptanceValidator` or `validatePackAcceptance` flag | `AcceptancePipelineFailed` | `Plan47ModContractTests.ContentPackWithFailingAcceptanceValidator_*` |
| **Backward compatibility** | Old manifests without new fields default to accept-all | Missing `game_range` / `mod_contract_range` defaults to `*` (any) | N/A | `Plan47ModContractTests.OldManifest_WithoutVersionFields_AcceptedWithDefaults` |
| **MOD_CONTRACT.md** | 57-line stub | Full contract: manifest schema table, field aliases, version range syntax, stability classes, typed rejection table, overlay/determinism invariants, acceptance pipeline section, save compatibility, non-goals | N/A | Document authority |
| **ModCompatibilityEvaluator.cs** | Absent | Pure Core, engine-free; `>=`, `<=`, `>`, `<`, `==`, `^`, `~`, `*` semantics; multi-clause AND evaluation | N/A | `Plan47ModContractTests.*_Theory` (25 theory rows) |

### 3. Files Modified / Created

| File | Action | Description |
|---|---|---|
| `Assets/Ashfall.Core/Mods/JsonModLayering.cs` | Updated | Added `ModRejectionCode` enum; typed `ModDiagnostic.Code`; `game_range`, `mod_contract_range`, `pack_type`, `dependencies`, alias fields on `ModManifest`; compatibility governance phase in `Build`; topological dep sort; acceptance pipeline seam (`validatePackAcceptance`, `customPackAcceptanceValidator`) |
| `Assets/Ashfall.Core/Mods/ModCompatibilityEvaluator.cs` | Created | Pure Core version range evaluator; `>=`/`<=`/`>`/`<`/`==`/`^`/`~`/`*` semantics |
| `Ashfall.Core.Tests/Mods/Plan47ModContractTests.cs` | Created | 38 tests: compatibility governance, deterministic overlay, dependency resolution, typed rejection, content-pack acceptance, modded replay stability, backward compatibility, Theory rows |
| `docs/mods/MOD_CONTRACT.md` | Updated | Full Plan 47 contract: manifest schema, field aliases, version range syntax, stability classes, typed rejection table, overlay/determinism invariants, content-pack acceptance, save compatibility, non-goals |

### 4. Test Evidence
- `Ashfall.Core.Tests/Mods/Plan47ModContractTests.cs`: **38/38 PASS**
- `Ashfall.Core.Tests/Mods/JsonModLayeringTests.cs`: **5/5 PASS** (existing tests unchanged — backward compat maintained)
- Build: `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --nologo` → `Build succeeded` (0 errors, 5 pre-existing warnings unrelated to this task)

### 5. Known Constraints / Debt
- `HostCli.Mods.cs` and `ModRuntime.cs` pass positional args; they use the old signature without version args — no change needed as they default to `"1.0.0"` / contract `1`. Any host caller that wants to pass version info may do so via the optional parameters.
- `validatePackAcceptance=true` path instantiates a synthetic `CatalogEntry` with `RequiredRung=LOADED` — this is conservative (always passes for a properly-constructed overlay). Full acceptance pipeline integration requires host-side wiring if needed; the seam is in place.
