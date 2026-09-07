# Plans 202–205 Flagship Reconnaissance — Authority Map
## Plastic Fuel Recovery · Perimeter Defense · Mushroom Cultivation · Cargo Airdrop

> **RENUMBERING NOTE (mandatory):** The original roadmap document proposed "Plans 102–105".
> Those numbers are already consumed: `piagentsplans/102-foundry-accords-expansion.md` through
> `105-trade-specialties-expansion.md`, plus `docs/foundry/PLAN102_*.md` / `PLAN103_*.md`
> closeouts for a different (foundry) domain. The highest used plan block is **198–201**
> (`src/Main.Plans198_201.cs` — CBRN, Comms Array, Ceremonies, Robotics). This flagship is
> therefore numbered **Plans 202–205**:
>
> | Original (conflicting) | Canonical |
> |---|---|
> | Plan 102 — Scrap Plastic Fuel Recovery | **Plan 202** |
> | Plan 103 — Perimeter Defensive Grid | **Plan 203** |
> | Plan 104 — Mushroom Cultivation | **Plan 204** |
> | Plan 105 — Cargo Airdrop Logistics | **Plan 205** |
>
> Implementation order is unchanged: 204 → 202 → 203 → 205, shared reconnaissance first.

Status: **Wave A — reconnaissance complete.** Exit criterion "no duplicate authority planned"
is met **with the amendments below**. No production code was modified.

---

## 1. Headline finding — two of four domains already exist

| Proposed domain | Existing authority | Verdict |
|---|---|---|
| Plastic pyrolysis | `CharcoalPyrolysisCatalog.cs` is **narrative lore** (pre-war coaling mounds), not an industrial system | Plan 202 is **genuinely new** — no conflict |
| Perimeter defense | `Assets/Ashfall.Core/Defense/PerimeterDefenseSystem.cs` (390 ln) + `PerimeterDefenseCatalog.cs` + `perimeter_defenses.json` (8 defenses) — **live, wired, tested, saved** | Plan 203 must **EXTEND**, not create |
| Mushroom cultivation | `Assets/Ashfall.Core/Farming/FungiCultivationSystem.cs` (321 ln) + `underground_flora.json` — **live, wired (`Main.Plans190_193.cs`), saved** | Plan 204 must **EXTEND**, not create |
| Cargo airdrop | Nothing exists | Plan 205 is **genuinely new** |

**Creating the proposed `perimeter_defense_catalog.json` or `mushroom_cultivation_catalog.json`
would fork the data authority** against Invariant 6 and the §1.1 one-authority rule. Both
catalogs are already registered in `ContentUtilizationScanner` (`perimeter_defenses.json` →
`PerimeterDefenseSystem`, `underground_flora.json` → `FungiCultivationSystem`) and in
`SaveSectionRegistry` (`perimeter_defense`, `fungi_cultivation` sections in the campaign envelope).

---

## 2. Authority map (§2 of the roadmap, verified against source)

| Concern | Current authority (verified) | Flagship change |
|---|---|---|
| Industrial production | `Foundry/SilentFoundrySystem.*` (partial-class batch architecture) | **202 extends** — plastic conversion as foundry-family batch profile |
| Power / energy | `Shelter/PowerGridSystem.cs`, `PowerDistributionSubgridSystem.cs` | 202 consumes; offgas credit feeds grid, never net-positive |
| Fuel items | `items.json`: `fuel`, `diesel_fuel`, `fuel_1l`, `fuel_cell`, `fuel_canister`, `item_aviation_fuel_canister` | 202 adds synthetic-fuel item through **catalog only** |
| Generator fuel | Shelter power grid fuel consumers | 202 output via fuel-class compatibility tag, no per-consumer branches |
| Vehicle fuel | `ExpeditionVehicleSystem.Refuel(vehicleId, units)` — **unit-based; inventory consumption is the caller's concern** (`ExpeditionHostSession.cs:500`) | 202 output claim converts item → fuel units at host session; 205 recovery consumes same path |
| Fire/gas hazards | Hazard authority (ventilation + room failure effects) | 202/204 route gas/spore consequences as **typed handoff** |
| Machine condition | Item/fixture degradation authority (B-series scrap economy stream) | 202 batch wear submits to it; no parallel saturation model |
| Base defense | `Defense/DefenseSystem.cs` (raid orchestration, accepts `PerimeterDefenseSystem` at `DefenseSystem.cs:236/294`) | 203 snapshot consumed by existing raid flow |
| Perimeter emplacements | `Defense/PerimeterDefenseSystem.cs` — construct/load-ammo/repair/barrel-service, assault sim, `ISeededRng`, capture/restore, save section `perimeter_defense` | 203 **extends**: sector topology, false alarms, weather wear, alert reset, counterplay |
| Raid/breach combat | `Combat/TacticalCombatSystem.*`, `CombatBreachingEngine.cs` | 203 supplies encounter modifiers only (Trap C respected) |
| Agriculture | `Farming/AgricultureSystem.cs`, `Greenhouse/GreenhouseSystem.cs` | 204 extends `Farming/FungiCultivationSystem` specifically |
| Fungi state | `FungiPlotState` (plot/room/strain/substrate/growth/moisture/sporeDensity/contamination/bloom), `FungiCultivationState`, `FungiSaveStore` | 204 extends: substrate-prep pipeline, temp band, flush count, disposal transaction |
| Spore hazard | `FungiCultivationSystem.OnSporeExposure(roomId, hazard)` → ventilation/medical | 204 keeps this routing; cultivation never subtracts health |
| Nutrition | `KitchenNutritionSystem.cs` (288 ln) | 204 harvest items routed through existing recipes |
| Water | `WaterTreatmentSystem.cs` (649 ln) | 204 humidification consumes real water |
| Ventilation | `VentilationSystem.cs` (622 ln) | 204/202 authoritative for spore/contaminant load |
| Weather | `World/WeatherSystem.cs` (season windows, deterministic reseed). **No wind vector on WeatherSystem itself**; wind exists in `WeatherSondeSystem`, `FalloutSystem`, `AviationSystem` (`CalculateFlightRisk(windSpeedKnots, …)`) | 205 drift reads a wind input from the existing weather/sonde authority — **do not invent a second weather source**; extend WeatherSystem with a deterministic wind vector if needed |
| World events / distress | `Radio/RadioDistressSystem.cs` (799 ln; `OnSignalIntercepted/Triangulated/Expired/Resolved`) | 205 schedules drops from resolved contacts through this lifecycle |
| Map / coordinates | `World/DamagedMapSystem.cs`, `DamagedMapCatalog.cs` | 205 landing targets are world points in this authority |
| Expeditions | `Expeditions/ExpeditionSystem.cs` (1387 ln; `maxLootCapacityKg`, vehicle `cargoCapacityKg`) | 205 recovery uses standard sortie lifecycle; capacity enforced |
| Cargo contents | New: persisted snapshot inside 205's event state (generate-once, never reroll — Trap H) | new state, single owner |
| Save | `Save/SaveSectionRegistry.cs` — new sections registered here; campaign envelope handles atomic writes | extend registry; no mirrored stores |

---

## 3. Required scope amendments (deltas to the roadmap)

### Plan 203 — Perimeter (was "Plan 103")
- **Do NOT create** `perimeter_defense_catalog.json`. Extend `perimeter_defenses.json`
  (schema stays v1-compatible; additive fields only) and `PerimeterDefenseCatalog.cs`.
- Already present (do not rebuild): build costs, ammo loading, barrel wear/jam, repair,
  assault simulation with stealth-neutralization and night accuracy, `ISeededRng` determinism,
  save capture/restore, raid integration via `DefenseSystem`.
- Genuinely missing (the 203 work): per-**sector** grid (current system is a flat emplacement
  list — no north/east/south/west topology), false alarms, weather wear tick, alert-device
  spend/reset lifecycle, attacker counter tags, bounded intrusion log.
- Current data already models abstraction safely (`def_tripwire_flare_line` etc. are abstract
  gameplay devices — Trap D respected; preserve that register).

### Plan 204 — Fungi (was "Plan 104")
- **Do NOT create** `mushroom_cultivation_catalog.json`. Extend `underground_flora.json`
  (`FungusStrainDef` / `SubstrateDef`) additively.
- Already present: plots with moisture band, darkness requirement, substrate nutrition and
  contamination risk, deterministic growth, toxic-bloom RNG (seeded), spore-hazard event per
  room, harvest→inventory, purge action, save round-trip, wired in `Main.Plans190_193.cs`.
- Genuinely missing: substrate **preparation states** (untreated/prepared/clean/compromised),
  temperature band, humidity-vs-ventilation coupling, **flush count** (current system resets
  plot to fallow after one harvest), disposal of infected substrate as a real inventory/waste
  transaction (current purge deletes via water action), kitchen-recipe routing verification.
- **UI dependency:** `FungiCultivationBedPanel` is a registered **UI-07 stub** (empty
  `RefreshView`). Per the UI audit's closure rules, 204's panel work must implement the real
  domain workflow through the existing stub, not create a second panel.
- Note: a `JusticeSystem`-adjacent `Farming/NutritionDiversitySystem.cs` also exists — check
  recipe ownership there before adding fungi recipes.

### Plan 202 — Plastic pyrolysis (unchanged in scope)
- New Core system + new `plastic_pyrolysis_catalog.json` (no existing authority conflict).
- Architecture: extend the `SilentFoundrySystem` batch pattern (partial-class family) rather
  than inventing a second batch runtime.
- Fuel compatibility: vehicle refuel is **unit-based** (`Refuel(vehicleId, units)`); the
  generic fuel-class tag compatibility lives at the inventory→unit conversion seam
  (`ExpeditionHostSession`), not inside `ExpeditionVehicleSystem`.
- The `AnaerobicBiogasDigesterPanel` is a **UI-06 fake-success prototype** with no Core
  system behind it — it is **not** a fuel-production authority and must not be treated as one.

### Plan 205 — Airdrop (unchanged in scope, one binding note)
- New Core system + new `cargo_airdrop_catalog.json`.
- Wind: `WeatherSystem` has **no wind vector today**. Add a deterministic, seeded,
  save-persisted wind vector to the existing `World/WeatherSystem` (single authority, §8.6)
  rather than reading ad-hoc wind from `AviationSystem`/`FalloutSystem` internals.
- Drop scheduling binds to `RadioDistressSystem` signal-resolved lifecycle; landing targets
  register through `DamagedMapSystem`; recovery runs a standard `ExpeditionSystem` sortie with
  `maxLootCapacityKg` enforcement.

---

## 4. Numbering / documentation hygiene

- All roadmap section references in implementation logs, closeouts, host partial names
  (`Main.Plans202_205.cs`), and test files must use **202–205**.
- Save-section keys remain domain-named (`plastic_pyrolysis`, `cargo_airdrop`; existing
  `fungi_cultivation`, `perimeter_defense` reused) — they are independent of plan numbers.

## 5. Concurrent-work caution

Working tree has in-flight streams (Glassworks B100 tests, CombatBreachingEngine tests,
RailwaySystemTests modifications). Implementation waves must not touch those files and must
re-run reconnaissance diffs before merging (per UI-audit closure rule 7).

## 6. Verified baseline (read-only checks)

- `PerimeterDefenseSystem`: constructed with `ISeededRng` + inventory; assault sim, wear, jam,
  breach events all deterministic; capture/restore complete.
- `FungiCultivationSystem`: seeded tick, deterministic growth modifiers, spore exposure event,
  harvest/purge actions, state restore.
- `SaveSectionRegistry`: `fungi_cultivation` (line 155) and `perimeter_defense` (line 178)
  registered with file names (lines 341/359).
- `ContentUtilizationScanner`: both catalogs gated (lines 818/841).
- Plan-number space: `piagentsplans/` ends at 130; host partials end at `Plans198_201`;
  AGENTS.md references Plans 78–81, 110–113, 146–149, 190–193 → **202–205 is free**.
