# Wave 8 — B2 Player-Route Panel Wave (premise verification + build order)

> **Package:** `WAVE8-PART2-B2-PLAYER-ROUTES` · **Builder/Integrator:** user-authorized
> **Date:** 2026-09-17 · **Premise:** the B2 intent table from the prior session
> closeout (three settled systems with Core + host + save but no player route).

This note records the source-level verification of that table **before** any
code was written, per `AGENTS.md` rule 7 ("a plan or audit is not proof") and
rule 10 ("stop when authority is missing"). It corrects one premise in the
intent table and re-orders the wave accordingly.

## 1. Verified intent table (HEAD)

| System | Core authority | Host driver at HEAD | Save | Route | Classification | Verified? |
|---|---|---|---|---|---|---|
| `sky_defense_battery` | `SkyDefenseBatterySystem` + ordnance catalog | `Main.FlagshipInstitutions.cs` (`EnsureSkyDefense`, turret/volley/service/tick, crew via institution ledger, telemetry intake) | `SkyDefenseBatterySaveStore` | none | **True player-route gap** | ✅ |
| `dynamic_quests` | `DynamicQuestlineSystem` | `Main.Plans46_49.cs` (workshop/radio/excavation event triggers, complete/fail, daily tick) | `DynamicQuestSaveStore` | none | **True player-route gap (read-mostly)** | ✅ |
| `vehicle_garage` | `VehicleGarageSystem` + modifications catalog | `Main.Plans50_53.cs` only constructs and exposes it | `VehicleGarageSaveStore` | none | **Mis-classified — see §2** | ✅ |

## 2. Premise correction — `vehicle_garage` is not a UI-only gap

The intent table classified `vehicle_garage` as "live commands with no player
route". Source verification shows the commands are **live public API but have
no host caller and no downstream consumer**, so a panel over them would be a
*fake operational route* (`AGENTS.md` UI rule):

- `InstallModification` / `UninstallModification` / `ServiceChassis` /
  `ServiceEngine` / `ServiceTransmission` / `RecordTripWear` /
  `RegisterRecoveryMission` have **zero callers in `src/`**.
- The effect read models (`GetEffectiveCargoCapacityDelta`,
  `GetEffectiveSpeedMultiplierDelta`, `GetEffectiveFuelConsumptionMultiplier`,
  `GetEffectiveWearRateMultiplier`, `GetEffectiveRadiationProtectionPermille`)
  have **zero consumers** outside their own tests — notably
  `ExpeditionVehicleSystem.CreateExpeditionProfile` (the profile that actually
  drives `ExpeditionHostSession.BuildProfile`) does **not** apply them.
- `isImmobilized` is only ever set by `RecordTripWear`, which is never called,
  so recovery missions are unreachable by construction.
- The garage owns a **second wear model** (chassis/engine/transmission
  permille) beside `ExpeditionVehicleSystem.condition`; `docs/PLANS_50_53_AUTHORITY_MAP.md`
  and `docs/VEHICLE_GARAGE_AUTHORITY_MAP.md` say the garage "decorates & extends"
  `ExpeditionVehicleSystem`, but the decoration seam is not implemented.

Building `GarageDetailPanel` without first resolving that seam would let the
player spend scrap/parts for effects the simulation ignores. That needs a
signed integration decision (which wear model is authoritative; where the
decoration applies), so it is **recorded as a blocker**, not improvised.

**Recommendation (needs foreman signature):** complete the signed Plan 50 seam —
garage effects decorate the expedition profile at `ExpeditionHostSession` from
the campaign-owned garage instance; trip distance feeds `RecordTripWear`; the
duplicate `VehicleInstance.condition` wear becomes a projection or is retired.
Then build the panel.

## 3. Build order for this wave

1. **`sky_defense_battery` (this package)** — fully integrated end-to-end
   (telemetry warning → track → volley → `ApplyInterceptionMitigation` →
   `SkyLayerArmorSystem` → residual strike), real commands, real observable
   outcome. Cleanest truthful route; no new architecture decision.
2. **`dynamic_quests`** — host-driven read model (active emergency quests,
   deadlines, stages, progress); read-only panel surface.
3. **`vehicle_garage`** — blocked on the §2 decision.

## 4. `sky_defense_battery` acceptance

Presentation-only surface bound to the campaign-owned `SkyDefenseBatterySystem`
(no new authority, no panel-side intercept math):

- status rail: emplacement state, magazine, barrel heat, hydraulics, tracks,
  interceptions;
- orbital-track list: severity, impact day, target grid, energy, volleys,
  resolved state;
- commands routed to Core: load magazine (`TryLoadMagazine`), fire volley
  (`TryFireVolley`), hydraulic service (`TryServiceHydraulics`), crew
  assign/remove (`TryAssignCrew` / `TryRemoveCrew`);
- intercept preview from the Core read model `PreviewInterceptChance` (never
  recomputed in the panel); refresh on the system's own events and after each
  command.

## 5. Verification plan

- `dotnet build Ashfall.csproj` (0/0)
- `bash scripts/run_test.sh Ashfall.Core.Tests/UI/PanelRouteGateTests.cs`
- `bash scripts/run_test.sh Ashfall.Core.Tests/UI/PlayerSurfaceCoverageGateTests.cs`
- `bash scripts/run_test.sh Ashfall.Core.Tests/SkyDefenseBatteryTests.cs`
- `godot --headless --path . -- --panel-bind-lifecycle-selftest`
- `godot --headless --path . -- --ui-accessibility-selftest`
- `python3 scripts/ci/generate-ui-panel-catalog.py --check`
- `bash scripts/ci/generate-architecture-map.sh --check`