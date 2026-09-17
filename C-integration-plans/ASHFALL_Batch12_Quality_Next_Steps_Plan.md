# ASHFALL: 2D Atomic-War Survival — Quality Next Steps Roadmap (Batch 12)

**Generated:** 2026-08-19<br>
**Status:** Repository-grounded integration plan<br>
**Batch:** 12 — Steps 177–192<br>
**Host:** Godot 4.7+ (.NET 8 C#)<br>
**Core:** .NET Standard 2.1<br>
**Data authority:** `Assets/StreamingAssets/Data/`

## Executive outcome

Batch 12 expands industrial chemistry, food preservation, clinical care, animal husbandry, defensive infrastructure, finance, geophysics, archival culture, emergency radio, and music presentation. It is not sixteen independent panels: it is a set of Core state slices that must reuse existing authorities and expose missing prerequisites honestly.

Current authorities to extend are `SilentFoundrySystem`, `CraftingSystem`, `GoodsCatalog`, `WeaponConditionSystem`, `TacticalCombatSystem`, `NeedsSystem`, `WeatherSystem`, `GreenhouseSystem`, `CrossingArbitrationSystem`, `WarlordDoctrineSystem`, `DiseaseSystem`, `JournalSystem`, `ResearchSystem`, `FactionRadioEngine`, the generational engine, and Godot’s existing audio/asset registry. The roadmap names `DynamicEconomySystem`, `PowerGridPanel`, `SoundManager`, and `CenturySeed` conceptually; the active equivalents are `MarketSystem`, host power contracts if Batch 8/9 established them, Godot audio services, and the generational engine under `Assets/Ashfall.Core/Legacy/`.

## Prerequisite work packages

### W0 — Batch 8/9 readiness audit

Before implementation, verify the status of Batch 8/9 rail, power, medical status, shelter transit, radio, profile, and industrial contracts. Re-run the baseline gates and classify each Batch 12 step as ready, gated, or deferred.

### W1 — Industrial resource and process contracts

Required for Steps 177, 178, 185, 186, and 188:

- material reservations and mass conservation;
- process jobs with stages, time, heat, power, operator, condition, and failure outcomes;
- foundry output quality and installation targets;
- explicit explosive-material safety states;
- data-defined industrial modifiers instead of hard-coded multipliers.

### W2 — Clinical, sanitation, and food-state contracts

Required for Steps 179, 181, 183, 185, and 187:

- typed fracture/injury state and treatment result;
- perishable storage condition and contamination state;
- animal cohort and feed conversion state;
- water/plumbing quality and environmental contamination modifiers;
- pest pressure and food-loss accounting.

### W3 — Shelter, faction, and geophysical contracts

Required for Steps 182, 184, and 189:

- generational relationship and diplomacy state;
- defensive trap placement and threat resolution;
- canonical survey grid and discoverable resource nodes.

### W4 — Archival and communication contracts

Required for Steps 190–192:

- document/knowledge archival references;
- radio transmitter power/range/failure state;
- discovered-track profile state and audio asset metadata.

## Step integration matrix

| Step | Authoritative integration | Smallest vertical slice | Gate |
|---|---|---|---|
| 177 Black powder mill | New hazardous-material process over foundry, crafting, goods, inventory, and combat ammo catalogs | Refine one sulfur batch and produce one data-defined powder lot consumed by one ammunition recipe | Safety, mass conservation, operator risk, and no infinite-resource guarantee |
| 178 Bessemer converter | Extend `SilentFoundrySystem` and shelter construction/material state | Process one pig-iron melt into one structural-steel output lot | Requires heat/air/power accounting and a real construction consumer; no unconditional 10x output |
| 179 Orthopedic surgery | New fracture/treatment state over combat trauma, survivor needs, and medical host wiring | Diagnose one fracture, install one fixator, advance healing, and restore one movement capability | Requires an authoritative injury/status model; no UI-only health or mobility restoration |
| 180 Steam fog horns | New acoustic-navigation aid over `WeatherSystem`, expedition state, and Godot audio events | One lost expedition receives a valid bearing during low visibility and resolves a return attempt | Sound is not Core gameplay; model range, weather attenuation, fuel/steam, and failed return |
| 181 Root cellar | New storage-condition state over greenhouse harvests, inventory, needs, and food spoilage | Store one root-crop lot and advance it through one winter interval | Requires perishable-condition authority; no guaranteed 18-month preservation without data support |
| 182 Marriage pacts | New relationship/diplomacy state over generational succession, crossing arbitration, survivor roster, and faction trust | Complete one eligible alliance with consent, dowry settlement, lineage record, and bounded trust effect | Must avoid coercive UI, invalid kinship, duplicate alliances, and magical inherited perks |
| 183 Rabbit warrens | New animal-cohort state over greenhouse biomass, inventory, crafting/tanning, and survivor needs | Feed one rabbit cohort, produce one meat/pelt yield, and record welfare/loss | Requires feed, disease, capacity, breeding, slaughter, and contamination rules; no infinite yield |
| 184 Pit traps | New defensive placement state over locations, tactical combat, and warlord doctrine | Place one trap at one canonical choke point and resolve one convoy approach | Must model detection, maintenance, false triggers, noncombatant risk, and bounded damage |
| 185 Cast-iron pipes | Extend foundry output and water/plumbing quality consumed by disease/environment systems | Replace one pipe segment and reduce one contamination source | “Blood lead to zero” is not valid without a clinical toxicity model; use measurable exposure reduction |
| 186 Banknotes | New currency/issuance state over `MarketSystem`, crafting, journal/audit records, and economy settlement | Engrave one plate, print one controlled note series, and settle one trade | Requires denomination, backing, serial/idempotence, counterfeit, redemption, and inflation rules |
| 187 Pest repellers | New pest-pressure state over greenhouse/pantry storage and power consumption | Cover one room and reduce one data-defined pest-loss rate | No 100% protection; model coverage, battery/power, adaptation, maintenance, and residual loss |
| 188 Babbitt bushings | Extend foundry/research/device-condition state | Broach and install one bushing, changing one machine’s condition/lifespan modifier | Requires real machinery condition and maintenance consumer; no unconditional triple lifespan |
| 189 Magnetometer survey | New geophysical survey state over expeditions, locations, research, and resource nodes | Scan one grid, resolve one anomaly, and discover one canonical resource site | Must use stable survey seeds and canonical location/resource IDs; no arbitrary five-ton reward |
| 190 Microfiche archive | New archival capture state over journal, documents, generational succession, and inventory | Archive one authored document onto one film record and preserve its metadata | Store references/records rather than Godot image objects; future inheritance must be explicit |
| 191 Spark-gap transmitter | Extend radio transmitter state over `RadioHostSession`, `FactionRadioEngine`, and power storage | Send one emergency SOS and resolve one faction/caravan response | No “unjammable” guarantee; model range, interference, power, heat, detection, and duplicate response |
| 192 Vinyl jukebox | Extend discovered-track/profile state over Godot audio, `vinyl_record_archive.json`, and `Theme` | Discover, display, and play one validated track with artwork and optional effects | Audio paths must pass asset registry; Core stores track IDs, not audio nodes or streams |

## Required Core contracts

All new stateful systems must implement `CaptureState()`/`RestoreState()`, schema migration, future-version rejection, checksum validation, deterministic event identities, stable ordinal ordering, typed command results, and explicit failure/partial-completion outcomes.

Use explicit units: `mass_kg`, `volume_l`, `temperature_c`, `pressure_kpa`, `pressure_hpa`, `duration_hours`, `power_kw`, `energy_kwh`, `distance_km`, `dose`, `contamination_fraction`, and `condition_fraction`.

Hazardous production must reserve inputs before starting, release or consume them exactly once, and emit safety incidents through typed Core events. Industrial processes must never create ammunition, currency, food, or construction material without a data-defined input/output ratio and real inventory settlement.

Clinical effects must pass through the medical/survivor status owner. Food, water, disease, pest, and animal systems must distinguish contamination, spoilage, loss, and treatment rather than collapsing them into a single percentage.

## Godot implementation requirements

Each accepted slice receives a typed host session/read model, typed UI panel/modal, `Main.Setup`/`Main.Save`/`Main.Flush` wiring, event-driven refresh, keyboard accessibility, unit-labelled values, and visible blocked/failed/partial states. UI cannot mutate inventory, health, currency, power, disease, or faction trust directly.

Candidate panels include `GunpowderMillPanel`, `BessemerConverterPanel`, `BoneSurgeryModal`, `FogHornPanel`, `RootCellarPanel`, `MarriageAllianceModal`, `RabbitWarrenPanel`, `PitTrapPanel`, `PipeCastingPanel`, `BanknoteEngravingModal`, `PestRepellerPanel`, `BroachingBenchPanel`, `MagnetometerPanel`, `MicroficheCameraModal`, `SparkGapTransmitterPanel`, and `JukeboxPlayerModal`.

## Data authority

New executable definitions should be added only where existing catalogs cannot be extended. Candidate files include:

- `industrial_processes.json`;
- `clinical_procedures.json`;
- `storage_conditions.json`;
- `animal_cohorts.json`;
- `defensive_positions.json`;
- `currency_series.json`;
- `geophysical_anomalies.json`;
- `archive_formats.json`;
- `radio_transmitters.json`;
- `vinyl_record_archive.json` extension.

Every new file and object requires `schema_version`, snake_case IDs, duplicate-ID validation, reference validation, and an explicit migration/default policy. Existing narrative records remain flavor/codex inputs until a loader and Core consumer exist.

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --asset-registry-selftest
godot --headless --path . -- --playable-shell-selftest
godot --headless --path . -- --ui-layout-selftest
godot --headless --path . -- --save-slots-selftest
./scripts/ci/godot-asset-gate.sh
./scripts/ci/no-legacy-residue.sh
```

Feature-specific tests must cover input reservation, process interruption, safety failure, deterministic replay, insertion-order independence, save/checksum migration, duplicate rewards, resource conservation, invalid IDs, and real integration with inventory, medical, weather, shelter, economy, radio, journal, or audio registries.

## Definition of done

Every step is implemented, explicitly deferred, or blocked with evidence. No duplicate authority exists for inventory, health, power, currency, weather, radio, combat, disease, journal, or audio discovery. Hard claims such as “infinite,” “zero toxicity,” “zero loss,” “10x,” “100%,” and “unjammable” are replaced by measurable, data-defined outcomes. All new saves, data, host wiring, typed UI, and canonical Core/Godot gates pass.
