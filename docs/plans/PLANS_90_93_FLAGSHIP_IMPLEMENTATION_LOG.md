// Implementation Log — Flagship Plans 90–93
// (Cupola Foundry, Vertical Ascent, Acoustic Detection, Mineral-Chemical Production)

# Phase 0 — Forensic Architecture Audit

Status: PASS (complete 2026-09-05)

## Recorded authorities (current repository reality)

| Plan assumption | Repository reality |
|---|---|
| `FoundryProductionSystem` / `MetallurgySystem` | Do **not** exist. Live foundry authority: `SilentFoundrySystem` (`Assets/Ashfall.Core/Foundry/`, batch-heat lifecycle, labor/treaties), catalog `foundry_production.json` + `foundry_items.json`. |
| `CupolaFoundryEngine` | Does not exist — to be created as a **sibling continuous-melt engine** (Plan 90) that shares the item economy with SilentFoundrySystem but owns its own furnace state (refractory, slag, blower, mold queue). |
| `DeepExcavationSystem` | Does not exist. Live authority: `ExcavationSystem` (`Assets/Ashfall.Core/ExcavationSystem.cs`, SystemId `excavation`) — has shoring but **no item-consuming reinforcement action and no inventory dependency**. Plan 90 adds a canonical `TryApplyStructuralReinforcement` action (additive; optional inventory ctor parameter). |
| `TerrainTopologyCatalog` | Does not exist. Live topology: `WastelandMapSystem` (`MapNode`/`MapRoute`, discovery API `Discover(nodeId)`) + `RouteRegionTopology` (`region_route_topology.json`, region `high_scarp`). |
| `RadioTriangulationEngine` | Does not exist. Live triangulation: `SignalTriangulationSystem` (`Ashfall.Core.Radio`, bearing/confidence/uncertainty). Live threat telemetry: `OrbitalHarrowTelemetrySystem` (`OnImpactWarning`, `warningLeadDays=3`). |
| `ChemicalPlantSystem` | Does not exist. Live chemical-process authority: `ChemicalSynthesisSystem` (`Ashfall.Core.Crafting`, full start→tick→harvest→hazard→save pattern, `chemical_syntheses.json`, `synth_*` IDs) — **constructed only in tests; no host wiring, no panel, and its save section is not in `SaveSectionRegistry`**. |
| Plan 93 architecture decision | **Option A selected**: extend `ChemicalSynthesisSystem` (additive `corrosionRating` on definitions, `corrosionLevel` on vessels, purity band at harvest, maintenance action) + author `mineral_acid_synthesis_catalog.json` as an additional catalog file loaded into the same catalog. No new chemical engine. |
| `InductionCupolaFurnacePanel` | Exists as an untyped stub (hardcoded text, `Bind(object?)` ignores parameter, no handlers) — becomes a typed presentation-only panel bound to the cupola engine (AGENTS.md missing-UI table entry removed in the same commit). |
| GameBootstrap | Gone (stale AGENTS.md). Wiring convention = `src/Main.<Area>.cs` partials with `EnsureXxx`/`SetupXxx`/`SaveXxx` triads + `TickXxx` from `Main.CampaignOwners` day loop (template: `src/Main.Plans190_193.cs`). |

## Key conventions to follow

- Inventory billing: `InventoryBill.AddCost/AddGrant` + `Inventory.TryExecuteTransaction(bill, mutate)` (atomic).
- RNG: `_campaignDay.Rng.Fork("<stream>")` with numeric fallback seed; new stream ids added to `CampaignStreamIds`.
- Save: `SaveSectionRegistry.All` tuple + `SectionFileNames` entry → `src/Host/XxxSaveStore.cs` façade via `SaveStoreHub.FromCodec(SchemaVersionedEnvelope<T>.Encode/Decode)` → `CaptureSection(key, store.TryCapturePersisted(state))` → `SetupXxx` in `RestoreAllSubsystemsFromDisk` + `Main.CampaignServices` new-game block.
- Panels: `IBindablePanel`, typed `Bind(system)`, built via `AshfallUiHelpers`, descriptor in `PanelRegistryBootstrap.RegisterAll()`, actions via `PanelRegistry.ConfigureActions`, route in `Main.GameFlow` / `OpenExpandedPanel` switch.
- Integrity: new JSON catalogs need top-level `schema_version`; new id namespaces added to `CatalogIntegrityValidator.IdPrefixes` + `CatalogIntegrityRules.IdPrefixes`; reference fields added to `ReferenceKeys` (both files).
- Content utilization: new catalog file names added to `AuthoritativeCatalogs` + loader patterns; consumer code must genuinely query (QUERIED stage) or content reads dead.
- Traits: `trait_*` snake_case strings queried from survivor definitions (`SurvivorDefinition.traitIds`); systems receive a trait query delegate, never a survivor-system reference.

## Divergences from the pasted plan (adapted, not silent)

1. **Foundry names**: `FoundryProductionSystem`/`MetallurgySystem`/`DeepExcavationSystem`/`RadioTriangulationEngine`/`ChemicalPlantSystem` do not exist. Real authorities recorded above; implementations target them.
2. **Item ID reuse over duplication**: `item_foundry_grey_iron_ingot` (pig iron), `item_foundry_flux` (limestone flux), `item_foundry_firebrick` (refractory), `item_foundry_t_beam` (structural beam), `item_foundry_alloy_part` (machined casting) already exist — Plan 90 reuses them; only genuinely missing items are authored.
3. **Expedition travel** has no route segments; speed reduction implemented via a new convention-following provider hook `SetTravelSpeedMultiplier(Func<string,float>)` (mirrors `SetEncounterChanceMultiplier`), applied in `AdvanceOutbound`/`AdvanceInbound`/`Estimate`, scoped to vertical-route locations by the ascent engine's provider.
4. **Mineral chemistry** persists through the canonical `chemical_synthesis` section (registered into the campaign envelope as part of this milestone) — no duplicate `mineral_chemical_plant` section/state.
5. **Trait names**: PascalCase plan names mapped to repo convention: `trait_foundry_master`, `trait_patternmaker`, `trait_mountaineer`, `trait_rigging_specialist`, `trait_seismologist`, `trait_sonar_technician`, `trait_chemical_engineer`, `trait_industrial_chemist`.
6. **Safety abstraction** (non-negotiable): no real furnace/acid/rigging/targeting procedures — normalized bands and balance values only, per plan §Shared Invariant 6.

# Phase execution journal

(append per slice)
