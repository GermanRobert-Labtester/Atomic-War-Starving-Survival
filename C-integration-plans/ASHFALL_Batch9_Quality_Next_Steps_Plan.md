# ASHFALL: 2D Atomic-War Survival — Quality Next Steps Roadmap (Batch 9)

**Generated:** 2026-08-19<br>
**Status:** Repository-grounded integration plan<br>
**Batch:** 9 — Steps 129–144<br>
**Host:** Godot 4.7+ (.NET 8 C#)<br>
**Core:** .NET Standard 2.1<br>
**Data authority:** `Assets/StreamingAssets/Data/`

## Executive outcome

Batch 9 is an endgame industrial, clinical, communications, and cultural progression batch. It must consume Batch 8 contracts where present, especially rail logistics, power/storage, radio relays, medical status, event identity, and scrapbook/profile persistence.

The active authorities are `ExpeditionSystem`, `LocationLayoutSystem`, `SilentFoundrySystem`, `WeaponConditionSystem`, `TacticalCombatSystem`, `WeatherSystem`, `GreenhouseSystem`, `NeedsSystem`, `CrossingArbitrationSystem`, `NarrativeEncounterSystem`, `FactionRadioEngine`, `JournalSystem`, `EpilogueMatrixRuntime`, and the generational engine. The train yard, geophone, music, rail, and technical material currently present in JSON are narrative/data evidence, not complete runtime systems.

## Dependency work packages

1. **W0 — Batch 8 audit:** verify which rail, power, radio, medical, shelter-transit, and profile contracts actually exist.
2. **W1 — Rail and industrial assets:** Steps 129, 130, and 140.
3. **W2 — Clinical and environmental status:** Steps 131, 132, 135, 138, and 141.
4. **W3 — Shelter operations and communications:** Steps 136, 137, 139, and 143.
5. **W4 — Food, diplomacy, and knowledge:** Steps 133, 134, and 142.
6. **W5 — Ending gallery:** Step 144 after event/profile persistence is stable.

## Step integration matrix

| Step | Authoritative integration | Smallest vertical slice | Gate |
|---|---|---|---|
| 129 Armored train | New rail-asset state over expedition, locations, tactical combat, and Batch 8 rail contracts | Restore one locomotive, assign crew, and move one squad/cargo manifest | Requires route, fuel, crew, mass, cargo reservation, and combat settlement |
| 130 Lathe rifling | Extend `SilentFoundrySystem`, `WeaponConditionSystem`, `CombatCatalog`, and tactical combat | Produce one rifled barrel and install it on one supported weapon | Preserve jam floors; no guaranteed 0% jams or automatic range doubling |
| 131 Cataract surgery | New clinical procedure over dose history, survivor needs, trauma, and medical host state | Diagnose one impairment and apply one bounded post-surgery visual result | Blocked until visual impairment is an authoritative survivor status |
| 132 Barograph | New instrument observation state over `WeatherSystem` and `JournalSystem` | Record pressure trend and issue one data-defined weather warning | No earthquake prediction without an earthquake model; lead time is data-driven |
| 133 Malting | Extend crafting, goods, greenhouse inputs, and survivor needs | Malt one barley batch into one consumable | Nutrition/morale effects must be bounded; no unsupported disease claims |
| 134 Hostage rescue | Persistent case state over crossing arbitration, narrative encounters, tactical combat, and faction relations | Resolve one ransom, negotiation, or breach case | Persist deadlines, health, payment reservations, faction response, and idempotence |
| 135 Soil autoclave | Extend `GreenhouseSystem` with steam, heat, fuel/power, and soil health | Sterilize one bed and reduce one contamination load | No guaranteed 100% sterilization or +30% yield without simulation support |
| 136 Recon kites | New reconnaissance state over expeditions, locations, weather, and map visibility | Launch one kite in valid wind and reveal one bounded threat signal | Model wind, line/device condition, observer risk, and partial failure |
| 137 Elevator | New shelter-transit state consumed by `DutyRosterSystem` and `NeedsSystem` | Move one worker between two canonical levels and record fatigue savings | Blocked until transit time is authoritative; no UI-only productivity bonus |
| 138 Music sessions | New communal activity over caregiving, needs/trauma hooks, inventory, and Godot audio events | Craft one instrument and hold one scheduled session | Audio is presentation-only; effects need a real status/effect consumer |
| 139 Enigma drums | Extend Batch 8 codebook state over `FactionRadioEngine` and `ResearchSystem` | Decode one authored transmission into one canonical intelligence result | Deterministic puzzle; no arbitrary coordinates or future-event fabrication |
| 140 Lost-wax casting | Extend foundry, crafting, goods, and medical-tool definitions | Cast one surgical or precision component with quality state | Surgical reliability remains data-defined, not permanently perfect |
| 141 Geophones | New perimeter-sensor state over shelter hazards and `WarlordDoctrineSystem` | Detect one underground/convoy signature and create one response opportunity | Distinguish signal, uncertainty, false positive, and actual threat |
| 142 Leather tomes | Extend journal, research, and generational succession | Bind one knowledge record into one persistent heirloom token | Future-generation effects are bounded, versioned, and earned |
| 143 Weather beacon | Extend radio host, faction radio, weather, caravan, relay, and power state | Broadcast one warning and resolve one caravan response | Model power, range, weather interference, duplicate rescue, and failure |
| 144 Ending gallery | Extend epilogue evaluation, profile unlocks, audio registry, and Batch 8 scrapbook | Unlock and replay one ending slide with authored audio/branch metadata | Profile-scoped, checksummed, and isolated from active campaign saves |

## Required Core contracts

Every new stateful feature must provide `CaptureState()`/`RestoreState()`, schema migration, future-version rejection, checksum validation, stable ordinal serialization, deterministic RNG, idempotent event identity, typed result objects, and explicit failure/partial-completion outcomes.

Use explicit units: `mass_kg`, `distance_km`, `duration_hours`, `power_kw`, `energy_kwh`, `temperature_c`, `pressure_hpa`, and existing radiation units. Industrial jobs must reserve materials before starting and settle outputs exactly once. Clinical systems must return typed treatment results rather than mutating UI or duplicate health state.

## Godot implementation requirements

Each accepted step receives a typed host session/read model, a typed UI panel/modal, `Main.Setup`/`Main.Save`/`Main.Flush` wiring, event-driven refresh, keyboard accessibility, visible blocked/failed/partial states, and no gameplay mutation in UI code.

Candidate panels include `ArmoredTrainPanel`, `LatheRiflingPanel`, `EyeSurgeryModal`, `BarographPanel`, `MaltingFloorPanel`, `HostageNegotiationModal`, `SoilAutoclavePanel`, `ReconKitePanel`, `ElevatorShaftPanel`, `MusicJamModal`, `EnigmaDecryptionPanel`, `LostWaxCastPanel`, `GeophoneArrayPanel`, `BookbindingPanel`, `WeatherBeaconPanel`, and `CinematicGalleryPanel`.

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

Tests must cover success, rejection, boundary, cancellation, deterministic replay, insertion-order independence, interrupted-save restore, checksum/tamper, migration, future-version rejection, duplicate-event prevention, and real subsystem integration.

## Definition of done

Every step is implemented, explicitly deferred, or blocked with evidence. No duplicate authority exists for routes, power, health, inventory, weather, radio, combat, journal, or endings. New JSON is schema-versioned and reference-valid, host save/flush wiring is complete, UI has no placeholders, and all Core/Godot gates pass.
