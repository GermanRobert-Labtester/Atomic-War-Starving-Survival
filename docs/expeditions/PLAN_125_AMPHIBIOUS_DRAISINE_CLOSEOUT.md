# PLAN 125 — Amphibious Draisine Closeout (Phase 12)

**Verb:** `--amphibious-draisine-selftest` (alias) · **Crossing balance:** `docs/expeditions/PLAN_125_CROSSING_BALANCE.md` · **Authority map:** `docs/expeditions/PLAN_125_AMPHIBIOUS_AUTHORITY_MAP.md`

## 1. Files changed

| Layer | Files |
|---|---|
| Data | `amphibious_draisine_catalog.json` (schema_version 1) |
| Core | `AmphibiousDraisineCatalog.cs` (loader/validator with **kit↔route-class coherence gate**), `AmphibiousDraisineEngine.cs` (crossing state machine, Capture/Restore) |
| Host | `AmphibiousDraisineHostSession.cs` (vehicle/workshop/inventory/pump-power providers + `IsRouteCapable`), `AmphibiousDraisineSaveStore.cs`, Main wiring, harness/soak |
| UI | `AmphibiousDraisinePanel.cs` |
| Save | `amphibious_draisine` row (owner `expeditions`) + `amphibious_draisine_save.json` |
| Tests | `Plan125AmphibiousDraisineCatalogTests` (9), `Plan125AmphibiousDraisineEngineTests` (18), persistence subset |

## 2. Catalog entries

2 kit profiles (Mk I 0.80 flotation/0.55 tolerance; Mk II 0.90/0.8 + powered pump), 2 pump profiles (manual/battery), 3 route classes (flooded rail / destroyed bridge / submerged causeway), 1 repair profile. Validators: dup-ID, ranges, kit→pump/route FKs, **coherence gate** (a kit must actually clear every route class it unlocks — caught 2 data-binding errors during authoring), item FKs validator-walked. **Schema invariant (test-enforced): no open-water/naval overlap fields.**

## 3. Tests added (27 focused cases)

Catalog: structural validation, meaningful-tradeoff gate (cargo penalty + reduced water speed + non-instant deployment), route-binding coherence, **naval-distinctness schema scan**, item FKs, dup-ID, incoherent-binding rejection, determinism. Engine: incompatible vehicle tag + worn-vehicle refusals, install requires workshop+parts, deployment costs ticks, cargo-overload rejection, flotation-margin math (overloaded/damaged loadouts below the 1500 bp minimum refuse), current-exceeds-tolerance abort, valid crossing completes through Landing→Recovering→LandReady, weather raises hazards, navigator skill bounded (damage reduced but never flawless), seeded pontoon damage + ingress with working-vs-failed pump, ingress ≥ 8000 bp forces emergency recovery, battery pump requires power, abort→recovery, typed route capability (mk1 cannot claim the deep causeway), repair bounded, **same-seed damage parity**, and a reflection gate proving the engine owns no expedition/topology/vehicle-condition truth.

## 4. Selftests run

41/41 (route-capability + deep-route refusal), 23/23 (75-day stages E/F/G/H), 24/24 (54-cell route matrix), data-integrity 317/317.

## 5. Save/migration matrix

| Save point | Reload | Gate |
|---|---|---|
| missing section | no kits | `Amphibious_null_save_is_clean_migration` |
| kit installed | upgrade persists | persistence + harness |
| deployed | deployment phase preserved | transition tests |
| mid-crossing | progress/ingress coherent | `Amphibious_roundtrip_preserves_midcrossing_state` + `amb_save_roundtrip` |
| damaged kit | condition persists | repair/soak bounds |
| disabled pump | pump state preserved (engine-side) | pump gating |
| unknown kit profile | guarded (phase/clamps) | RestoreState |

## 6. Determinism evidence

Same-seed parity over 10 crossing ticks (pontoon/ingress/progress byte-parity); 54-cell matrix reproducible per-cell seeds; 75-day harness `stageH_crossing_hash`.

## 7. Content-utilization evidence

Kit install/repair items (`item_hermetic_hatch_silicone_gasket`, `item_high_tensile_steel_culvert_brace`, `item_diving_suit_vulcanized`, `item_epoxy_injector`, `item_battery_reconditioned`) resolve in `items.json` (validator-walked). Vehicle-integration flows through the existing owners: `VehicleGarageSystem` records, `ArmoredCrawlerModuleCatalog` schema precedent, `ExpeditionVehicleSystem` logistics — the engine mutates only its own kit state (reflection-gated).

## 8. Scene-binding evidence

Route `amphibious_draisine` registered + routed; PanelRouteGate 20/20; lifecycle 16/16; a11y PASS; scene lint 30/0. Panel answers §15.4 (can cross? margin? risk? condition? pump? abort?) with text status; naval-remainder prose present ("boats remain the right tool for open water").

## 9. Known limitations

- Route classes are authored in the catalog; no authored map edges currently carry the water-route tags, so in-game traversal waits on topology authoring (map owner) — capability seam and planner lookups are live and tested.
- Fuel/mass consequences flow through `ExpeditionVehicleSystem` logistics recalculation (typed); the standalone fuel-use delta is part of vehicle logistics, not this engine.
- **Second-tool review (§21) PERFORMED 2026-09-13** — `docs/PLANS_122_125_SECOND_TOOL_REVIEW.md` verdict PASSED.

## 10. Follow-ups

Author flooded-route topology edges/tags in the map data; Mk II matrix extension for the submerged-causeway class; boats/naval distinctness integration test at the route-planner level.
