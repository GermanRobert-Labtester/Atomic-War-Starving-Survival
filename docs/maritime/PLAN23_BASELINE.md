# Plan 23 — Baseline (verified, no-change)

> Captured from repository truth (working tree at baseline). Every claim below was
> verified by reading the named file or running the named command. Working tree had
> ~340 pre-existing in-flight changes from other plans; none of them are Plan 23 work.
> One exception: `Assets/StreamingAssets/Data/dive_sites.json` already carries the
> **uncommitted Plan 10 delta** (4 → 12 sites) — see `PLAN10_PLAN23_DIVE_RECONCILIATION.md`.

## 1. Verification baseline (commands + results)

| Gate | Command | Result |
|---|---|---|
| Tests build | `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | PASS — 0 warnings, 0 errors |
| Test suite | `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | PASS — 5591/5591, 0 failed, 0 skipped (27 s) |
| Godot host build | `dotnet build Ashfall.csproj` | PASS — 0 errors, 0 warnings |
| Data integrity | `godot --headless --path . -- --data-integrity-selftest` | PASS — 0 findings, 6407 ids authored, 151 catalogs, 0 errors |
| Maritime/flotilla | `godot --headless --path . -- --maritime-selftest` | PASS (black_flotilla_selftest) |
| Deep coast | `godot --headless --path . -- --deep-coast-selftest` | PASS — 72/72 |

## 2. Live counts (working tree = live truth)

| Catalog | Count | Notes |
|---|---|---|
| `black_flotilla_items.json` | 24 | Merged into the global item catalog by `ItemCatalogLoader` as a secondary item file. Items are generic trade/scavenge material (paper_scrap, crayon, teddy_bear, spoiled_*…), **not** maritime-specific gear. Consumers: item catalog, ProceduralScavengeSystem (via ContentUtilizationScanner mapping), asset registry. |
| `dive_sites.json` | 12 (worktree) / 4 (HEAD) | Plan 10 delta is in the working tree, uncommitted. |
| `currents.json` | 17 | **NOT sea currents.** `CurrentsCatalog` (Muster) — "the sector's political actors" (Expansion 06). Maritime-adjacent members: `faction_undertow` (offers rescue/salvage_recovery/local_knowledge, inactive), `faction_hydro_barons` (the_coast), `faction_archivists`/`faction_osteophages` (the_drown). |
| `faction_radio_corpus.json` | 13 faction bands | No Black Flotilla band. |
| `characters.json` | 54 NPCs | No Black Flotilla faction members. Coastal NPCs exist: `npc_beacon_keeper_maren`, `npc_coastal_chandler_orlov` (Orlov the Diver), `npc_net_mender_kira` (coastal_shelf), `npc_nomi_fisk` (the_drown), `npc_halden_mire` (Fleet radio, the_shelf), `npc_tamsin_rook` (Harbour Night-Clerk, the_approach), `npc_coastal_chandler_orlov`. |
| `faction_lore.json` | 23 entries | **No Black Flotilla entry.** |
| `holdfast_factions.json` | actions list | Contains `faction_the_fleet` (inactive, the_shelf) and `faction_the_office` — the canonical Fleet referenced by `District8DeepCoastSystem`. |
| `faction_stance` registration | code-side | `FactionStanceEngine.RegisterFaction(FactionThresholds)` — used by Silent Foundry host + trade screen. |
| `hardcore_economy_tuning.json faction_preferences` | 1 entry (central_garrison_remnants) | Faction trade-preference authority (`buys_at_premium`/`refuses`/`trade_currency`). |
| `settlements.json` | 6 | `settlement_cape_beacon` = coastal lighthouse commune; `route_node` = `loc_black_flotilla_outpost`; trader = `npc_coastal_chandler_orlov`. |
| `deep_lore_locations.json` | 10 | Maritime loot tables exist here (VariableLootNode tables). |
| Coastal/drowned `loc_*` | ~12 | `loc_black_flotilla_outpost`, `loc_settlement_cape_beacon`, `loc_shelf_foghorn/perimeter_breakwater/service_channel/deep_berth`, `loc_maritime_icebreaker_dock`, `loc_coastal_fog_signal_station`, `loc_hydro_baron_desal_plant_4`, `loc_frozen_river_ferry_crossing`, `loc_aurora_borealis_grounding_shoal`. |

## 3. Maritime runtime inventory (all live, all tested)

| System | File | State |
|---|---|---|
| `MaritimeDiveSystem` | `Assets/Ashfall.Core/Maritime/MaritimeDiveSystem.cs` (618 ln) | 4-room stealth dive state machine, air, compressor, noise, decompression, radiation dosing, outcomes, capture/restore. `StealthDiveInstance` is a thin subclass (18 ln). |
| `DiveSiteCatalog` | `Maritime/DiveSiteCatalog.cs` | Loads `dive_sites.json` schema v2. Fields: site_id, name, oxygen_budget_ticks, base_noise_floor, keeper_thread_id, rooms[4]{room_type, hazard_level, search_difficulty}. **No safe/loot/contamination/tide/current/location fields in schema.** |
| `SafeCrackingSystem` | `Maritime/SafeCrackingSystem.cs` (532 ln) | Full tumbler-combination runtime, tool condition, noise/alarm/jam, one-time loot transfer, deterministic seed+safeId combination, capture/restore. **Authored consumers: zero** — only demo registration (`MaritimeHostSession.RegisterSafeDemo` → `safe_km19_oil_tin` at `loc_cut_kilometre_19`). |
| `ProceduralScavengeSystem` | `Maritime/ProceduralScavengeSystem.cs` | Weighted-Poisson loot rolls, day/visit degradation, decontamination; consumes `VariableLootNode` tables. Live consumer: `MaritimeHostSession.SeedLootNodes()` (4 hardcoded nodes) + deep_lore locations. |
| `VariableLootNode` | `Maritime/VariableLootNode.cs` | Loot-table data model (ItemId, Min/MaxQty, SpawnChance, DegradationChance, DegradedItemId). |
| `PsychologicalContaminationSystem` | `Maritime/PsychologicalContaminationSystem.cs` | 4 grounded contamination types, action blocking, mental-break triggers, save-safe. Location-keyed: 5 hardcoded inland locations. **Zero maritime/dive-site consumers.** |
| `District8DeepCoastSystem` | `District8DeepCoastSystem.cs` (741 ln) | Deep-coast route spine (Foghorn 8 → breakwater → channel → berth → icebreaker dock), stage machine, reopening decision w/ Fleet levy, dock-dive handoff, daily contamination decay. 72/72 selftest. |
| `DiveInstanceRunner` | `Expeditions/DiveInstanceRunner.cs` | Flag/event-driven dive runner (keeper trace, Sovereign hold choice, storm-masking noise model). |
| Host wiring | `src/Host/MaritimeHostSession.cs` + `src/Host/MaritimeSaveStore.cs` + `src/Main.Maritime.cs` | Maritime suite save section ("maritime") with checksum envelope. |
| Save | `src/Host/MaritimeSaveStore.cs`, `SaveSectionRegistry` row `maritime` | Dive/scavenge/psych/safecrack states restore; legacy fallback verified. |

## 4. Standing / trade / radio authorities

| Authority | File | Notes |
|---|---|---|
| Faction stance/trust | `Assets/Ashfall.Core/Economy/FactionStanceEngine.cs` + `FactionStanceTypes.cs` | Trust −100..+100, `FactionThresholds` (raid/rob/trade/intel thresholds), host providers. `faction_black_flotilla` is **not registered** anywhere. |
| Faction roster data | `holdfast_factions.json` (Holdfast), `currents.json` (Muster), `faction_lore.json` (23 entries, no Flotilla) | `faction_the_fleet` exists (inactive, the_shelf). The Black Flotilla has **no faction-lore entry, no stance registration, no radio band, no trade preference**. |
| Trade | `hardcore_economy_tuning.json` `faction_preferences` (1 entry), `TradeSpecialtySystem` (survivor crafting specialties — not faction trade), `settlements.json` `economy.trade_specialty` | Settlement trade-specialty vocabulary: e.g. `preserved_food`, `electronics`. Cape Beacon (coastal) uses `electronics`. |
| Radio | `FactionRadioEngine` + `faction_radio_corpus.json` | 13 bands keyed by faction id (`military_remnants`, …, `faction_silent_foundry`). Chatter pools: intercept_chatter / parley_resolutions / raid_warnings / trade_reactions. Deterministic day+freq selection. |
| NPCs | `characters.json` (54) | No Flotilla-faction NPCs. Coastal-adjacent: Halden Mire (Fleet radio, the_shelf), Tamsin Rook (Harbour Night-Clerk, the_approach), Orlov the Diver, Lightkeeper Maren, Kira the Weaver (coastal_shelf). |
| Caravan | `Economy/CaravanCatalogLoader.cs` | `caravan_flotilla_salt_run` route: loc_black_flotilla_outpost → loc_the_shallows_market → … → loc_holdfast. |
| World evolution | `WorldEvolutionEngine` + `world_evolution_events.json` | Day/flag-triggered location/node mutations (`blockade`, `territory_flip`, `site_degradation`, `hazard_bloom`). One Flotilla event exists: "Flotilla Garrison Retreat". |

## 5. Tide / current / surge truth

- **Sea currents: none.** The 17-entry `currents.json` is the Muster's wandering-communities roster (`CurrentsCatalogLoader` — "the sector's political actors"). `currents_pamphlets.json` = 16 doctrine pamphlets for those same currents.
- **Tides: none.** No tide state, schema, or consumer exists anywhere (Core/src/data).
- **Storm surge: none.** `WeatherSystem` (Core/World) owns weather state with `WeatherKind` {Clear, Rain, Overcast, Ashfall, FalloutStorm, Blizzard, BlackRain} and `OnWeatherChanged`; `District8DeepCoastSystem.TickDaily(day, WeatherKind)` already consumes weather for contamination decay — the proven weather→coast hook.
- **Map/location mutation authority:** `WorldEvolutionEngine` (day/flag-triggered events over `LocationEvolutionSystem`, `LandmarkDegradationSystem`, `WastelandMapSystem`).

## 6. Baseline exit-gate answers

1. **Has Plan 10 landed?** Working tree: yes (12 live sites). Git HEAD: no (4 committed). Reconciliation target: **14 total** by adding 2 new sites only.
2. **Underused live mechanics:** safe cracking (0 authored safes), psychological contamination (0 maritime consumers), per-site rooms from catalog (MaritimeDiveSystem.StartDive hardcodes room hazards, ignoring per-site room data), loot provenance (4 hardcoded nodes in host), deep-lore coastal loot tables.
3. **Owners:** standing → `FactionStanceEngine`; trade → `hardcore_economy_tuning.json` faction_preferences + settlements; radio → `FactionRadioEngine` + `faction_radio_corpus.json`; dive → `MaritimeDiveSystem` + `DiveSiteCatalog`; noise → dive noise level; safe → `SafeCrackingSystem`; loot → `ProceduralScavengeSystem`/`VariableLootNode`; contamination → `PsychologicalContaminationSystem`; travel/time → deep-coast route + Holdfast/IceRoad; map evolution → `WorldEvolutionEngine`; weather → `WeatherSystem`; save → `MaritimeSaveStore` + `SaveSectionRegistry` ("maritime").
4. **Dive/scavenge state survives save/load** via `MaritimeHostSave` (Dive/Scavenge/Psychology/SafeCrack + checksum). Safe combinations are derived (seed+safeId), never serialized; opened/jammed/looted flags persist.
5. **Live current/tide/weather hooks:** weather → deep-coast contamination decay (live). Muster currents → roster/pamphlets (live, political). Sea-current/tide: none.
6. **Consumed Flotilla content IDs:** the 24 items (global item catalog), dive site IDs (4 committed + 8 working-tree), `loc_black_flotilla_outpost` (caravan route + world evolution event + settlement route node), Expansion 09 registration.

## 7. Divergences from the source plan (repository truth wins)

| Source plan says | Repository truth | Consequence |
|---|---|---|
| "currents.json = 17 sea currents" | 17 Muster political actors (wandering communities) | Task 23C "currents" deliverables reinterpret to the coastal-currents audit (Undertow, Coastal Hydro-Barons, Osteophages…) + the minimal deterministic tide layer the plan explicitly authorizes (§6.2). No fake sea-current catalog is invented. |
| "dive sites = 4" | 12 in working tree (Plan 10 delta uncommitted) | Case C: add only the delta to 14. |
| "Flotilla standing track to create" | `FactionStanceEngine` exists, Flotilla unregistered | Register `faction_black_flotilla` thresholds; no new meter. |
| "trade specialty system" | `settlements.json` trade_specialty + `FactionTradePreference` | Marine salvage preference authored in `hardcore_economy_tuning.json`. |
| "psychological contamination for deep dives" | System keys locations by `location_id` dictionary | Add dive-site contamination mapping via site-anchored location ids — no new runtime. |
