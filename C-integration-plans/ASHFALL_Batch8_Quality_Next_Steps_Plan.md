# ASHFALL: 2D Atomic-War Survival — Quality Next Steps Roadmap (Batch 8)

**Generated:** 2026-08-19<br>
**Status:** Repository-grounded integration plan<br>
**Batch:** 8 — Steps 113–128<br>
**Host:** Godot 4.7+ (.NET 8 C#)<br>
**Core:** .NET Standard 2.1<br>
**Data authority:** `Assets/StreamingAssets/Data/`

## Executive outcome

Batch 8 is a dependency-first set of Core/Godot vertical slices covering exterior commerce, dosimetry, bioplastics, emergency expedition shelter, wills, foundry heating, radio relays, herbal medicine, hydro power, officer ranks, textiles, codebooks, a solarium, heavy recovery, UV sanitation, and the campaign scrapbook.

Every slice must have one authoritative Core state owner, typed Godot host/UI wiring, schema-versioned JSON where definitions are needed, deterministic outcomes, checksummed persistence, and headless integration coverage. Existing `MarketSystem`, `TravelingCaravanSystem`, `LocationLayoutSystem`, `RadiationSystem`, `DoseLedgerSystem`, `GreenhouseSystem`, `ResearchSystem`, `CraftingSystem`, `ExpeditionSystem`, `WeatherSystem`, `FinalWishSystem`, `NeedsSystem`, `SilentFoundrySystem`, `FactionRadioEngine`, `DutyRosterSystem`, `TacticalCombatSystem`, `DiseaseSystem`, `JournalSystem`, and the generational engine remain authoritative.

## Prerequisites and work packages

1. **W0 — Baseline audit:** verify current APIs, IDs, save versions, dirty-store wiring, and the status of Batch 7 dependencies.
2. **W1 — Minimal shared seams:** add only the required inventory reservations, `power_kw`/`energy_kwh` accounting, shelter heat queries, sanitation modifiers, and idempotent campaign-event identities.
3. **W2 — Commerce and industry:** Steps 113–115, 118, 120, and 123.
4. **W3 — Expedition, shelter, and communications:** Steps 116, 119, 122, 124, 125, and 126.
5. **W4 — Power and sanitation:** implement the shared power contract, then Steps 121 and 127.
6. **W5 — Campaign memory:** implement Step 128 after journal and event-ledger wiring is stable.

## Step integration matrix

| Step | Authoritative integration | Smallest vertical slice | Gate |
|---|---|---|---|
| 113 Caravanserai | New focused caravanserai state over `TravelingCaravanSystem`, `MarketSystem`, and `LocationLayoutSystem` | One courtyard, one arriving caravan, one pavilion trade, one deterministic auction | Canonical route/location/faction IDs and exact economy settlement |
| 114 Film dosimeters | New badge state reading `DoseLedgerSystem` and `RadiationSystem` | Issue, wear, develop, and clinically interpret one badge | Never mutate dose from the UI; use existing medical authority |
| 115 Bioplastics | `CraftingSystem` extension using `GreenhouseSystem` inputs and `ResearchSystem` unlocks | One researched recipe produces one gasket batch and repairs a real device target | Compressor repair is deferred if no authoritative device condition exists |
| 116 Emergency trenches | `ExpeditionSystem` extension coordinated with `WeatherSystem` and radiation exposure | One active expedition trenches during a storm and resumes afterward | Storm exposure must use existing weather/radiation calculations |
| 117 Wills | `FinalWishSystem` extension integrated with generational succession, inventory, and skills | One testament transfers one owned heirloom and one bounded bequest exactly once | Prevent invalid beneficiaries, duplicate transfers, and item loss |
| 118 Radiators | `SilentFoundrySystem` casting plus a `NeedsSystem` heat-source provider | One radiator is cast, installed, and changes one room’s warmth behavior | A 20°C guarantee requires an authoritative room-temperature model |
| 119 Radio relays | New relay coverage state over `FactionRadioEngine`, locations, and power | One ridge repeater expands coverage and persists condition/battery | Requires canonical nodes and power consumption |
| 120 Herbal medicine | Crafting conversion plus medical/respiratory treatment hooks | One herb batch becomes one data-defined treatment | No direct UI health mutation or unsupported cure claims |
| 121 Hydro sump | New hydro-sump state under shared power storage | One turbine produces bounded power while debris/condition change | Blocked until the power/storage authority exists |
| 122 Officer ranks | New rank state consumed by `DutyRosterSystem` and `TacticalCombatSystem` | One promotion grants one data-defined squad modifier | Do not duplicate survivor skills or combat perks |
| 123 Textile loom | `CraftingSystem` textile jobs using greenhouse fibers and inventory | One loom produces one canvas batch | Recipes and outputs must be JSON-authoritative |
| 124 Codebooks | New radio-codebook state over `FactionRadioEngine` and `WarlordDoctrineSystem` | One captured book decodes one transmission into one warning | No arbitrary future-event or coordinate generation |
| 125 Solarium | New visit/alignment state using shelter rooms and `NeedsSystem` | One visit produces a bounded morale/wellness result | Claustrophobia cure is deferred without a real status authority |
| 126 Heavy recovery | New recovery state extending expedition cargo and inventory capacity | One tow mission recovers and installs one heavy machine | Require mass, fuel, rigging, failure, and cancellation semantics |
| 127 UV rover | New patrol state consumed by `DiseaseSystem` and power storage | One room patrol reduces a data-defined contamination modifier | Blocked until sanitation and power modifiers are authoritative |
| 128 Scrapbook | New milestone state using journal, generational signatures, event ledger, and checksums | One proven milestone creates one immutable entry and optional image reference | Store metadata/resource references, not engine objects or save blobs |

## Cross-cutting contracts

New state must provide `CaptureState()`/`RestoreState()`, version migration, future-version rejection, checksum validation, stable ordinal ordering, deterministic RNG, idempotent event identity, typed success/failure results, and state-change events.

Use explicit units: `mass_kg`, `volume_l`, `power_kw`, `energy_kwh`, `temperature_c`, `duration_hours`, and the project’s existing radiation unit. New UI panels bind to typed host sessions and render blocked, unavailable, busy, failed, partial, and completed states.

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

Each slice requires success, rejection, boundary, deterministic replay, interrupted-save restore, checksum/tamper, migration, future-version, duplicate-event, and real cross-system integration tests. Hard claims such as 80% radiation reduction, 12 kW, 20°C, 100% sterilization, or +20 skill become data-defined targets rather than unconditional guarantees.

## Definition of done

Every step is implemented, explicitly deferred, or blocked with evidence; no duplicate authority exists; all new data is schema-versioned and reference-valid; host setup/save/flush paths are complete; typed UI contains no placeholder data; and all Core/Godot gates pass.
