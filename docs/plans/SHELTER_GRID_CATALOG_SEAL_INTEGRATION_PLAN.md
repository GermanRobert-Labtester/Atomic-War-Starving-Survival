# SHELTER GRID CATALOG SEAL — INTEGRATION PLAN (G1–G3)

**Source evidence:** `docs/forensics/SHELTER_CASCADE_SEAMS_FORENSIC_REPORT.md` (2026-09-06)
**Scope:** G1 (fluid power room-ID), G2 (runtime catalog load), G3 (test reconciliation/un-quarantine)
**Explicitly out of scope:** G4 (EMP coupling), G5 (medical/water-treatment power), G6 (failure-effect consumers) — separate design decisions.
**Plan class:** Repair wave — minimal, behavior-restoring, data-authority-sealing.

---

# 1. Objective

Make `power_grid.json` the actual runtime authority for shelter power-room definitions, fix the room-ID mismatch that starves the Plan 168 fluid network of power every campaign day, and restore the quarantined catalog test as a real regression gate.

# 2. Current Reality

| Fact | Evidence |
|---|---|
| `PowerGridHostSession.LoadGridJson()` returns hardcoded `DefaultGrid()` (4 rooms, no failure IDs); JSON never parsed | `src/Host/PowerGridHostSession.cs:119-146` |
| `power_grid.json` ships 6 rooms incl. `room_water_pump`, `room_lighting_main` | `Assets/StreamingAssets/Data/power_grid.json` |
| `TickPlan168Fluid` queries `IsRoomPowered("room_water_treatment")` — unknown ID → always `false` → `power=0f` daily | `src/Main.Plans166_169.cs:121`; `PowerGridSystem.cs:72-78` |
| `Main.World.cs:289` queries `IsRoomPowered("room_workshop")` — `room_workshop` absent from the 6-room catalog → always-false gate (same bug class) | `src/Main.World.cs:289` |
| `room_workshop` is canonical: present in `shelter_rooms.json`, `shelter_room_identities.json`, and referenced by `power_subgrid_nodes.json` (`node_workshop_feed`, `target_room_id: room_workshop`) | data files |
| `PowerGridCatalogTests.cs` quarantined via `<Compile Remove>` (`Ashfall.Core.Tests.csproj:44`); asserts 18 rooms vs. 6 shipped | executed `dotnet test --filter` → "no test matches" |
| Old saves with smaller room sets restore safely: `NormalizeAndValidate` prunes breaker/trip/priority records for unknown IDs and treats absent-from-list as closed-by-default | `Assets/Ashfall.Core/Shelter/PowerGridSystem.cs:371-409` |
| Core loader convention: `XxxCatalogLoader.Load(dataDir, IFileIO, IJsonSerializer)` with `CatalogDiagnostics` warnings | `FluidInfrastructureCatalogLoader` usage in `src/Host/FluidLogisticsHostSession.cs:18` |

# 3. Required Delta

**Existing behavior:** Runtime power rooms are a stale hardcoded host snapshot; catalog edits are no-ops; fluid network always depowered; workshop power gate always false; catalog test quarantined.

**Requested behavior:** Catalog parsed at host startup (fallback to embedded defaults only when the file is absent/unusable); fluid pump power derived from the canonical `room_water_pump` breaker state; `room_workshop` present in the catalog so its gate functions; catalog test compiled and passing against the shipped data.

**Delta:** One Core catalog loader + host wiring swap + two literal/data reconciliations + test restoration. No new systems, no save-format change, no UI change.

# 4. Evidence

See §2 and the forensic report (F3, F7, F8). All claims verified by direct source reads and executed test commands during the 2026-09-06 audit.

# 5. Existing Extension Seams

- `PowerGridHostSession.CreateDefault(ISeededRng)` — single construction site (`Main.World.cs:364`); add a dataDir-aware overload.
- `FluidInfrastructureCatalogLoader.Load(dataDir, FileSystemIO, SystemTextJsonSerializer)` — the exact Core loader pattern to copy.
- `CatalogDiagnostics.Warn(path, shape, ex)` — established malformed-catalog policy (H4 hardening).
- `NormalizeAndValidate` — already handles room-set growth for old saves (closed-by-default).
- `PowerGridCatalogTests` — existing (quarantined) test body to repair, not rewrite.

# 6. Proposed Architecture

```
power_grid.json (authority)
        │  ShelterPowerGridCatalogLoader.Load(dataDir, IFileIO, IJsonSerializer)   [NEW, Core]
        ▼
ShelterPowerGridCatalog (DTO)  ──missing/malformed file──▶ embedded fallback (current values, moved to Core)
        │
        ▼
PowerGridHostSession.CreateDefault(rng, dataDir)   [MODIFIED — hardcoded DefaultGrid() DELETED]
        │
        ▼
PowerGridSystem (unchanged authority)
        ▲
        │ IsRoomPowered("room_water_pump")                 [FIXED literal]
   Plan168FluidDayOwner ── power=0..1 ──▶ FluidLogisticsSystem.Tick
```

Single source of truth: the host's private `PowerGridJson`/`DefaultGrid()` are deleted; the fallback lives in Core next to the loader.

# 7. Ownership Matrix

| Concern | Owner |
|---|---|
| Catalog DTOs + loader + fallback defaults | `Ashfall.Core` (`Assets/Ashfall.Core/Shelter/`) |
| Catalog JSON | `Assets/StreamingAssets/Data/power_grid.json` |
| Host construction/session | `src/Host/PowerGridHostSession.cs` |
| Room-ID literals in day owners | Host call sites only (`Main.Plans166_169.cs`) |
| Simulation math, breakers, save envelope | `PowerGridSystem` / `PowerGridSave` (UNCHANGED) |
| Catalog validation tests | `Ashfall.Core.Tests/Shelter/PowerGridCatalogTests.cs` (un-quarantined) |

# 8. Data Flow

Load: `Main.SetupPowerGrid` → `PowerGridHostSession.CreateDefault(rng, _dataDir)` → Core loader parses `power_grid.json` → `PowerGridRoom` list → `PowerGridSystem` (unchanged).
Tick: `Plan168FluidDayOwner` → `IsRoomPowered("room_water_pump")` → `power ∈ {0f, 1f}` → `FluidLogisticsSystem.Tick(day, temp, power)` (unchanged signature).
Save: unchanged (`power_grid` section, envelope, dirty flag).

# 9. State Model

**No new persistent state.** Room set may grow (2 rooms added to catalog); old saves restore via existing `NormalizeAndValidate` semantics: pruned stale IDs, new rooms default to closed breakers, standard priority. Battery/fuel values persist unchanged. No DTO version bump required — the `power_grid` section format is untouched.

# 10. API / Contracts

| Contract | Kind | Notes |
|---|---|---|
| `ShelterPowerGridCatalogDef` | Core DTO | `schema_version`, `generation_watts_default`, `battery_capacity_wh_default`, `fuel_units_default`, `rooms[]` |
| `ShelterPowerGridRoomDef` | Core DTO | `id`, `display_name`, `draw_watts`, `default_priority` (string→enum), `failure_effect_id` |
| `ShelterPowerGridCatalogLoader.Load(dataDir, IFileIO, IJsonSerializer)` | Core static | Returns catalog; falls back to embedded defaults on missing file; throws/diagnoses on malformed content per repo policy |
| `ShelterPowerGridCatalogLoader.FallbackDefault()` | Core static | The current hardcoded values, moved verbatim — single source when file absent |
| `PowerGridHostSession.CreateDefault(ISeededRng, string? dataDir)` | Host overload | Old 1-arg signature retained, delegates with `null` (selftests / headless) |

No new interfaces, events, or enums. Priority string→`PowerGridRoomPriority` mapping: `critical`/`standard`/`low` (match existing JSON + enum ordinals).

# 11. Data Changes

**File: `Assets/StreamingAssets/Data/power_grid.json` (MODIFY — additive)**
- Add one room:
  ```json
  { "id": "room_workshop", "display_name": "Workshop", "draw_watts": 300,
    "default_priority": "low", "failure_effect_id": "fx_workshop_offline" }
  ```
  Rationale: canonical room ID already referenced by `power_subgrid_nodes.json` (`node_workshop_feed`) and queried by `Main.World.cs:289`. Draw 300 W sits between foundry (220) and lab-class loads; **balance parameter — implementer may adjust, but must not leave the room absent.**
- No other entries change. `room_water_treatment` must NOT be added (canonical pump room is `room_water_pump`).
- Schema version stays 1. All IDs snake_case; `failure_effect_id` values remain registered (consumers are G6, out of scope).

# 12. Save/Load

- No envelope or DTO change. Old-save restore of a 7-room set is already safe (§2, `NormalizeAndValidate` evidence).
- Test additions: restore of a pre-wave save (4-room `ClosedBreakers` snapshot) against the 7-room catalog → new rooms closed-by-default, no exceptions, totals correct.

# 13. Determinism

- Catalog load is ordered-list deterministic; no RNG introduced; no iteration-order hazards (loader preserves JSON array order; `PowerGridSystem` already sorts/validates internally).
- `TickPlan168Fluid`'s power argument becomes a deterministic function of persisted breaker state.

# 14. System / Event Wiring

- No new events. `OnPowerChanged`/`OnTickSummary` behavior unchanged.
- The only behavioral delta in simulation: fluid network receives real power state; workshop gate becomes reachable; total draw includes the two previously-missing rooms (+380 W nominal) — **this changes brownout timing in existing campaigns.** Expected and intended; flagged in §21.

# 15. Godot Integration

- `Main.World.cs:364`: `SetupPowerGrid` passes `_dataDir` into the new overload.
- No UI/panel changes. PowerGridPanel renders whatever rooms the session carries — it gains 3 rows automatically.

# 16. Narrative / Content Integration

None. No quests, flags, or radio hooks touched.

# 17. Failure Modes

| Case | Expected behavior |
|---|---|
| Catalog file missing (fresh checkout without import) | Embedded fallback defaults; diagnostics warn once; session constructs |
| Malformed JSON / bad schema version | Loader throws with file+field detail per H4 policy; `SetupPowerGrid` catches → fallback defaults + warn (host must never fail boot over a catalog) |
| Duplicate room IDs in JSON | Loader error → fallback defaults + diagnostics |
| Negative `draw_watts` / unknown priority string | Loader error naming the room ID → fallback |
| Old save (4-room era) | Restore prunes stale records; new rooms closed-by-default |
| Save from future with unknown room IDs | Existing prune logic handles (already canonical behavior) |
| `room_water_pump` breaker open in an old save | Fluid power = 0 — correct semantics, now driven by player-visible breaker state |

# 18. Test Strategy

**Core (`Ashfall.Core.Tests/Shelter/`)**
- Un-quarantine + repair `PowerGridCatalogTests`: pin `schema_version=1`, the three defaults, and the full 7-room post-wave inventory (id, draw, priority, failure_effect_id) — replacing the 18-room assertion. Exact-count assertion retained.
- NEW `ShelterPowerGridCatalogLoaderTests`: valid load; file-missing → fallback equals documented defaults; malformed JSON → loader error; duplicate ID → error; negative watts → error; unknown priority → error; deterministic ordering (load twice, same room order).

**Host consistency (structural)**
- The hardcoded `DefaultGrid()` deletion makes catalog/runtime drift impossible by construction; no mirror test needed.

**Integration**
- `TickPlan168Fluid` power derivation: fluid power follows `room_water_pump` breaker state (test via host selftest below).
- Old-save restore with grown room set (§12).

**Headless**
- Small `--power-grid-catalog-selftest` verb (HostCli pattern): construct `CreateDefault(rng, dataDir)`, assert ≥1 room, assert `room_water_pump`/`room_workshop` resolve via `IsRoomPowered` (returns a defined bool, not unknown-room false-by-miss), assert fluid power derivation returns 1f with all breakers closed.

# 19. Dependency-Ordered Phases

**Phase 0 — Baseline verification.** Run: `dotnet test` (Core), `dotnet build Ashfall.csproj`, `--data-integrity-selftest`. Record results. *Gate: green baseline or pre-existing failures documented.* Must not touch: `PowerGridSystem.cs`, `PowerGridSave.cs`, fluid/water/medical systems.

**Phase 1 — Core contract.** Add `ShelterPowerGridCatalogDef`/`RoomDef` DTOs + loader + fallback in `Assets/Ashfall.Core/Shelter/` (namespace `Ashfall.Core.Shelter`). No host changes yet. *Gate: new loader tests pass; Core builds clean.*

**Phase 2 — Test reconciliation (G3).** Un-quarantine `PowerGridCatalogTests` (remove csproj line 44), repair assertions to the shipped catalog + added `room_workshop`. *Gate: test compiles and passes against the JSON. Dependency: room addition from Phase 4 or stage the assertion update with it — see ordering note below.*

**Phase 3 — Host wiring (G2).** `CreateDefault(rng, dataDir)` overload; delete `DefaultGrid()` + private DTOs; `Main.World.cs` passes `_dataDir`. *Gate: host builds; boot selftest; power grid section still saves/loads.*

**Phase 4 — Room-ID reconciliation (G1).** `Main.Plans166_169.cs:121` literal → `room_water_pump`; add `room_workshop` room to `power_grid.json` (Phase 2's updated assertions land with this commit). *Gate: fluid selftest shows power=1f nominal; data-integrity selftest passes.*

**Phase 5 — Headless selftest.** Add `--power-grid-catalog-selftest`. *Gate: verb passes; `--save-load-ui-failure-selftest` (or nearest canonical save gate) still green.*

**Phase 6 — Full verification.** All five AGENTS.md gates + full Core suite.

Ordering note: Phases 2 and 4 share the room-count assertion; implement the JSON addition and the test-assertion update in the same commit to keep the suite green.

# 20. File Impact Map

| File | Action | Reason | Risk |
|---|---|---|---|
| `Assets/Ashfall.Core/Shelter/ShelterPowerGridCatalog.cs` (+Loader) | CREATE | Core loader per convention | LOW |
| `Assets/StreamingAssets/Data/power_grid.json` | MODIFY | Add `room_workshop` | LOW (balance) |
| `src/Host/PowerGridHostSession.cs` | MODIFY | dataDir overload; delete hardcoded grid | MEDIUM |
| `src/Main.World.cs` | MODIFY (1 line) | Pass `_dataDir` | LOW |
| `src/Main.Plans166_169.cs` | MODIFY (1 line) | Room-ID fix | LOW |
| `Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | MODIFY | Remove quarantine line | LOW |
| `Ashfall.Core.Tests/Shelter/PowerGridCatalogTests.cs` | MODIFY | Pin 7-room baseline | LOW |
| `Ashfall.Core.Tests/Shelter/ShelterPowerGridCatalogLoaderTests.cs` | CREATE | Loader coverage | LOW |
| `src/Host/HostCli.*.cs` | MODIFY | New selftest verb | LOW |
| `PowerGridSystem.cs`, `PowerGridSave.cs`, fluid/water/medical code | READ ONLY | Authority untouched | — |

# 21. Risks

| Risk | Mitigation |
|---|---|
| +380 W nominal draw shifts brownout timing in existing campaigns (rooms were invisible to the grid) | Intended correction; document in changelog; battery/fuel persist so players recover by shedding |
| Save/restore edge cases on grown room set | Covered by `NormalizeAndValidate` + new restore test |
| Loader boot failure on malformed catalog | Fallback-defaults policy; host never fails boot |
| Quarantined test may encode a real intended 18-room design | Deferred: expanding the catalog is a balance/design decision (out of scope); the repaired test pins today's shipped authority and any future expansion updates it deliberately |
| `room_workshop` gate flipping from always-false to real may change workshop feature behavior | Verify `Main.World.cs:289` consumer behavior in Phase 4; the current always-false state is a bug, not a feature |

# 22. Out of Scope

- G4 EMP→grid surge coupling; G5 water-treatment/medical power dependency; G6 `FailureEffectId` consumers (authored IDs remain inert).
- The `growLightHours: 6f` legacy fallbacks (G7, documented).
- Expanding the catalog to 18 rooms; any rebalancing of existing draws.
- PowerGridPanel UI work; subgrid system changes; `power_subgrid_nodes.json`.
- Any refactor of `PowerGridSystem` internals.

# 23. Rollback Strategy

- Each phase is an independent commit; revert order: selftest → room-ID literal → host wiring → catalog JSON + test reconciliation → loader.
- The loader falls back to embedded defaults, so deleting the JSON post-ship cannot brick saves.
- Save format unchanged — no migration rollback concerns.

# 24. Definition of Done

- [ ] `power_grid.json` is parsed at host construction; hardcoded `DefaultGrid()` deleted.
- [ ] Fluid network power derives from `room_water_pump` breaker state (nominal 1f; 0f when breaker open/tripped/brownout).
- [ ] `room_workshop` resolves via `IsRoomPowered` from catalog data.
- [ ] `PowerGridCatalogTests` compiled, passing, pinning the shipped catalog.
- [ ] Loader tests cover valid/missing/malformed/duplicate/negative/ordering.
- [ ] Old-save restore with grown room set verified.
- [ ] `--power-grid-catalog-selftest` passes.
- [ ] Full AGENTS.md verification checklist green.

# 25. Implementation Handoff

**MUST PRESERVE**
- `PowerGridSystem`/`PowerGridSave` untouched (Core authority + envelope).
- Fallback-defaults boot policy (host never fails on catalog problems).
- Old-save restore semantics (closed-by-default for unseen rooms).
- Existing selftest verbs and their exit codes.

**MUST ADD**
- Core loader + DTOs + fallback (single source when file absent).
- `CreateDefault(rng, dataDir)` overload; `_dataDir` at the call site.
- `room_workshop` catalog entry; `room_water_pump` literal fix.
- Loader tests; repaired+un-quarantined catalog tests; one host selftest verb.

**MUST NOT DO**
- No EMP/medical/water-treatment power work (G4–G6).
- No UI changes, no rebalancing of existing draw values, no Core simulation changes.
- No new RNG, events, or save-section formats.

**VERIFY WITH**
```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --power-grid-catalog-selftest
godot --headless --path . -- --bridge-selftest
```

**FIRST SAFE IMPLEMENTATION STEP**
Phase 0 baseline: run the Core test suite and `dotnet build Ashfall.csproj`; record results. Then create the Core loader with its tests before touching any host file.

**Suggested next prompt after accepting this plan:**
"Execute the SHELTER_GRID_CATALOG_SEAL plan via ashfall-implement, phase by phase, with the implementation journal in docs/plans/SHELTER_GRID_CATALOG_SEAL_IMPLEMENTATION_LOG.md."
