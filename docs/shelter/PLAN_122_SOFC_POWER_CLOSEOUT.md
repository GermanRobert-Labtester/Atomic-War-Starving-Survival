# PLAN 122 — SOFC Power Closeout (Phase 12)

**Verb:** `--sofc-power-selftest` (alias of `--plans-122-125-selftest`) · **Balance report:** `docs/shelter/PLAN_122_SOFC_BALANCE_REPORT.md` · **Authority map:** `docs/shelter/PLAN_122_SOFC_AUTHORITY_MAP.md`

## 1. Files changed

| Layer | Files |
|---|---|
| Data | `Assets/StreamingAssets/Data/sofc_power_catalog.json` (schema_version 1) |
| Core | `Assets/Ashfall.Core/Shelter/SofcPowerCatalog.cs` (loader/validator), `SofcElectrochemistryEngine.cs` (7-mode plant, Capture/Restore) |
| Host | `src/Host/SofcPowerHostSession.cs`, `src/Host/SofcPowerSaveStore.cs`, `src/Main.Plans122to125.cs` (setup/save/panel), `src/Host/HostCli.Plans122to125.cs` (harness/soak) |
| UI | `src/UI/SolidOxideFuelCellPanel.cs` |
| Save | `SaveSectionRegistry` row `sofc_power` (owner `shelter`) + `sofc_power_save.json` filename |
| Tests | `Plan122SofcPowerCatalogTests` (8), `Plan122SofcElectrochemistryEngineTests` (17), `Plans122to125PersistenceTests` (subset) |

## 2. Catalog entries

2 stack profiles (Mk I 18 kW / Mk II 30 kW), 3 fuel profiles (dirty/treated/clean), 2 grade profiles, 2 waste-heat profiles, 2 maintenance profiles. Closed vocabularies: acoustic classes (very_low…high), fuel quality classes. Validators: dup-ID rejection, ordered thermal bands, bp-range checks, internal FKs (stack→fuel/grade/heat/maintenance); item FKs (`install_item_ids`, `feedstock_item_ids`) auto-walked by `CatalogIntegrityValidator` — data-integrity selftest 317/317 clean.

## 3. Tests added (25 focused cases)

Catalog: schema/ranges/dup-ID/FK/deterministic load/empty-migration. Engine: startup grading (no instant start), rated cap, efficiency math, dirty-vs-clean degradation + seal scouring, thermal-cycle seal loss, waste-heat cap, **acoustic signature very_low ≠ undetectable** (hard gate), bounded skill modifier, safe shutdown, maintenance gate, same-seed fault parity + 40-tick replay identity, Derated transition, 180-day soak bounds, generator-dominance characterization (2× better than the 0.30 legacy baseline — not a generator-killer).

## 4. Selftests run

`--plans-122-125-selftest` 41/41 (incl. real-grid contribution + CHP routing), `--late-tech-mobility-selftest` 23/23 (75-day scenario, stages A/B/G/H), `--plans-122-125-balance-soak` 24/24 (180-day characterization), data-integrity 317/317.

## 5. Save/migration matrix

| Save point | Reload behavior | Gate |
|---|---|---|
| missing section | stays Offline (clean old-save migration) | `Sofc_null_save_is_clean_migration` |
| mid-startup | preheat progress + mode preserved | harness Stage H |
| online degraded | health/seal/mode coherent | `Sofc_roundtrip_preserves_mode_health_and_produces_after_restore` + `sofc_save_roundtrip` |
| faulted | remains faulted; maintenance required | engine tests |
| unknown stack profile | guarded (enum/range clamps) | RestoreState guards |

## 6. Determinism evidence

Same seed + same state + same command sequence ⇒ identical outcomes (SeededRngStub byte-parity test); 40-tick replay identity; 75-day harness save@71→replay state-hash parity (`stageH_sofc_hash`).

## 7. Content-utilization evidence

Stack install items (`item_ebpvd_ceramic_target_ingot`, `item_hermetic_hatch_silicone_gasket`, `item_metallurgy_steel_billet`, `item_magnetic_bearing_coil`) and repair items resolve in `items.json`; fuel profiles bind `item_biofuel_low/high/generator_grade`. Rebuild flow is wired (panel + session command); full crafting-consumer registration is the named follow-up below.

## 8. Scene-binding evidence

Route `sofc_power` registered (`PanelRegistryBootstrap` + `PlayerSurfaces` + expanded switch + GameFlow); `PanelRouteGateTests` 20/20; panel-bind lifecycle 16/16; a11y selftest PASS; scene lint 30/0.

## 9. Known limitations

- SOFC waste heat routes through the session seam bound to `ShelterThermalSystem.AddAuxiliaryHeat` at composition time; kitchen targeting is a placeholder policy pending the full thermal-coordinator allocation (heat never mutates consumer stats directly).
- The 180-day soak logged 0 fault events with its seed; fault economics are characterized by unit tests + the 75-day harness ops loop instead.
- **Second-tool review (plan §21) PERFORMED 2026-09-13** — `docs/PLANS_122_125_SECOND_TOOL_REVIEW.md` verdict PASSED; SOFC inventory-fuel binding remains the top open follow-up.

## 10. Follow-ups

Register rebuild items with the crafting/consumption owners (content-utilization claims); wire `FuelConsumer` to the real inventory check (Phase 9+ composition); acoustic-signature consumer integration when a hostile sensing system exists.
