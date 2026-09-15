# §27 Follow-On Expansion 1 — Atmospheric Water Condenser (B5–B8 continuation)

> The flagship's first §27 follow-on to land: the §9.9 condenser that Phases
> 6/9 flagged as deferred (research node + breakthrough item had zero runtime
> consumers). Built entirely on the contracts the flagship package created.

## What landed

### `AtmosphericCondenserSystem` (`Assets/Ashfall.Core/AtmosphericCondenserSystem.cs`)
- **Build** (`Main.WaterCondenserTryBuild`): live
  `HasCapability("knowledge_water_condenser_blueprint")` → canonical bill
  (1× `item_desal_membrane` + 2× metal_pipe + 4× scrap_metal) consumed
  atomically → Core commit. Research alone never grants the array (§15.3).
- **Power**: 90 W Peltier array registered as a **Standard-class** grid load
  (`room_water_condenser`) — a supplemental production source, shed-able by
  priority (deliberately unlike the critical well pump).
- **Yield**: bounded 15 L/day × deterministic weather humidity index
  (`HumidityIndexFor` — read-only projection of the weather authority's
  current kind; rain 1.0 … blizzard 0.2; the condenser never writes weather
  state) × membrane integrity — pushed into the treatment intake as **raw**
  water via the Plan 189 seam (`source_atmospheric_condenser`). Never potable
  directly; never a second counter.
- **Membrane wear**: 0.05/liter (≈2000 L lifetime); replacement consumes the
  canonical `item_desal_membrane` — the flagged breakthrough item now has a
  build AND a maintenance consumer. Blocked when integrity is full (wasted
  parts are a blocked action).
- **No brine**: condensation is not salt-water desalination — the §9.15
  defer-hold applies by physics, not just by audit.
- **Save**: new `water_condenser` section (192nd, versioned envelope);
  restore re-registers the load without replaying condensing (pinned).
- **§16.4 closure**: the flagged research description ("Peltier condensation
  array blueprint for extracting humidity from shelter exhaust") now
  terminates in the built, powered, membrane-wearing array.

## Verification

| Gate | Result |
|---|---|
| `Ashfall.Core.Tests/Water/AtmosphericCondenserSystemTests.cs` | 9/9 PASS (new) |
| Full `dotnet test` suite | **10,995 / 10,999** — the same 4 pre-existing dirty-worktree failures documented in the completion report; zero new |
| Save gates (counts 192/186, registry consistency, envelope builder) | PASS |
| `generate-architecture-map.py --check` | OK — 192 subsystems, 100% evidence |
| `--data-integrity-selftest` | PASS — 0 findings, 325 catalogs |
| `--content-utilization-selftest` | PASS — 0 hard failures |
| `dotnet build Ashfall.csproj` | 0 warnings, 0 errors |

## Files changed

- `Assets/Ashfall.Core/AtmosphericCondenserSystem.cs` (new Core authority)
- `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs` (+water_condenser row/filename)
- `src/Host/WaterCondenserSaveStore.cs`, `src/Host/WaterCondenserHostSession.cs` (new)
- `src/Main.WaterCondenser.cs` (new triad + build/membrane routes)
- `src/Main.SaveOrchestrator.cs` (2 call sites), `src/Main.ExpandedShelterSystems.cs` (1 tick line)
- `scripts/ci/generate-architecture-map.py` (+node), regenerated map
- Section-count pins → 192/186
- `Ashfall.Core.Tests/Water/AtmosphericCondenserSystemTests.cs` (new, 9 tests)
- `docs/plans/flagship_b5_b8/EXPANSION1_WATER_CONDENSER.md` (this file)

## Follow-on queue (remaining §27 items)

1. Richer seasonal crop catalog + crop rotation (content-heavy; core loop untouched)
2. Source maintenance/failure events riding the standardized power projection
3. Shelter emergency automation / priority presets (needs player-facing proof)
4. Advanced raid engineering (repair crews, breach aftermath) through the defense snapshot
5. Deeper Holdfast brine economy (needs a verified new trade consumer)
6. Disease-specific unsafe-water outbreaks via `DiseaseSystem.TryExpose`
7. Machine identity/condition feedback integration
