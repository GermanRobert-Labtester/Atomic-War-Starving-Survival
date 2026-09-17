# Wave 8 — B2 Player-Route Panel Wave (premise verification + build record)

> **Package:** `WAVE8-B2-PLAYER-ROUTES` · **Builder/Integrator:** user-authorized
> **Date:** 2026-09-17 · **Status:** COMPLETE (all three systems routed; both
> CLI residual items closed; all mapped rows ✅ 6/6).

This document records the source-level verification of the B2 intent table
**before** any code was written (per `AGENTS.md` rule 7, "a plan or audit is not
proof"), the one premise correction it produced, and the delivered result.

## 1. Verified intent table (HEAD at start)

| System | Core authority | Host driver at HEAD | Save | Route | Classification at start |
|---|---|---|---|---|---|
| `sky_defense_battery` | `SkyDefenseBatterySystem` + ordnance catalog | `Main.FlagshipInstitutions.cs` | `SkyDefenseBatterySaveStore` | none | True player-route gap |
| `dynamic_quests` | `DynamicQuestlineSystem` | `Main.Plans46_49.cs` | `DynamicQuestSaveStore` | none | True player-route gap (read-mostly) |
| `vehicle_garage` | `VehicleGarageSystem` + modifications catalog | `Main.Plans50_53.cs` (construction only) | `VehicleGarageSaveStore` | none | **Mis-classified (see §2)** |

## 2. Premise correction — `vehicle_garage` was not a UI-only gap

The original table called it "live commands with no player route". Verification
showed the commands had **no host caller and no downstream consumer**:

- `InstallModification` / `UninstallModification` / `ServiceChassis` /
  `ServiceEngine` / `ServiceTransmission` / `RecordTripWear` /
  `RegisterRecoveryMission` had **zero callers in `src/`**.
- Every effect query (`GetEffectiveCargoCapacityDelta` / speed / fuel / wear /
  radiation) had **zero consumers** — `ExpeditionVehicleSystem.CreateExpeditionProfile`
  ignored them.
- `isImmobilized` was only set by `RecordTripWear` (never called), so recovery
  was unreachable.

A panel over that surface would have been a fake operational route. The signed
`docs/PLANS_50_53_AUTHORITY_MAP.md` / `docs/VEHICLE_GARAGE_AUTHORITY_MAP.md`
already say the garage "decorates & extends `ExpeditionVehicleSystem`", so the
fix was to **complete the signed seam**, not to invent a new architecture:

- `VehicleGarageSystem.DecorateProfile(profile)` applies fitted-modification
  cargo/speed/fuel effects to the profile the expedition authority builds.
- `ExpeditionHostSession.Garage` (optional) is bound by `Main.Expeditions.cs`;
  unbound ⇒ legacy path byte-identical.
- `ExpeditionHostSession.PrepareVehicleForDispatch` refuses an immobilized
  vehicle, feeds travelled distance into `RecordTripWear`, and opens a recovery
  mission at the destination on catastrophic wear.
- `ExpeditionsCaravansDayOwner` advances recovery missions by 24 ticks/day and
  includes garage state in the pre-day snapshot/restore pair (determinism).

Read model left intentionally unconsumed: `GetEffectiveRadiationProtectionPermille`
(no vehicle→expedition radiation seam exists; the panel shows it only as the
authored modification property, never as an applied effect).

## 3. Delivered

1. **`sky_defense_battery`** — Expanded `SkyDefenseBatteryPanel` bound to the
   campaign-owned system (load magazine / fire volley / service hydraulics /
   crew assign-remove); Core read models only. `--sky-defense-selftest` added.
2. **`dynamic_quests`** — read-only Emergency Dynamic Quests board over the
   host-driven runtime.
3. **`vehicle_garage`** — Expanded `VehicleGaragePanel` (install/uninstall,
   chassis/engine/transmission service, recovery completion) over the completed
   Plan 50 seam. `--vehicle-garage-selftest` added.

All three rows in `docs/architecture/ARCHITECTURE_TEST_MAP.md` are now **✅ 6/6**.

## 4. Files

- **Core:** `Assets/Ashfall.Core/SkyDefense/SkyDefenseBatterySystem.cs`
  (read-only `OrdnanceCatalog`), `Assets/Ashfall.Core/Expeditions/VehicleGarageSystem.cs`
  (`GetRecord`/`IsImmobilized`/`ActiveRecoveries`/`GetInstalledSlots`/`DecorateProfile`/`AdvanceRecoveries`),
  `Assets/Ashfall.Core/HostCliRegistry.cs` (2 selftest descriptors).
- **Host:** `src/Host/ExpeditionHostSession.cs` (Garage seam), `src/Main.Expeditions.cs`,
  `src/Main.CampaignOwners.cs` (recovery day tick + snapshot), `src/Host/HostCli.cs`,
  `src/Main.Application.cs`, `src/Host/HostCli.SkyDefense.cs` (new),
  `src/Host/HostCli.VehicleGarage.cs` (new), `src/Host/UiAccessibilitySelfTest.cs`.
- **UI:** `src/UI/SkyDefenseBatteryPanel.cs`, `src/UI/DynamicQuestlinePanel.cs`,
  `src/UI/VehicleGaragePanel.cs` (all new), `src/Main.SkyDefense.cs`,
  `src/Main.DynamicQuests.cs`, `src/Main.VehicleGarage.cs` (all new).
- **Routing:** `PanelRegistryBootstrap.cs`, `Main.PlayerSurfaces.cs`,
  `Main.GameFlow.cs`, `Main.ExpandedShelterSystems.cs`.
- **Tests:** `Ashfall.Core.Tests/Expeditions/Plan50VehicleGarageIntegrationTests.cs` (new, 5).
- **Generated:** architecture map, UI panel catalog, CLI catalog, self-test
  manifest, player-surface manifest, docs index.

## 5. Verification

| Gate | Result |
|---|---|
| `dotnet build Ashfall.csproj` | 0/0 |
| `--sky-defense-selftest` | 17/17 PASS |
| `--vehicle-garage-selftest` | 19/19 PASS |
| `--ui-accessibility-selftest` (constructs all three panels) | 5/5 PASS |
| `--player-panels-uitest` / `--save-load-ui-failure-selftest` | PASS / PASS |
| `--7-day-smoke-selftest` (day-tick + determinism) | 10/10 PASS |
| PanelRoute / PlayerSurfaceCoverage / BindingPurity / SubscriptionHygiene / NoFabricatedFallback | 20 / 8 / 2 / 1 / 4 |
| ArchitectureTestMap / SelfTestManifest / UiPanelContract / HostCliHelpContract | 5 / 4 / 1 / 2 |
| SkyDefenseBattery / DynamicQuestline / VehicleGarage / Plans50_53 | 13 / 4 / 6 / 3 |
| Plan50VehicleGarageIntegration | 5/5 |
| architecture map / UI catalog / CLI catalog / self-test manifest / docs index `--check` | all in sync |

## 6. Residual / follow-up

- None blocking B2. `GetEffectiveRadiationProtectionPermille` remains a
  documented, intentionally-unconsumed read model (no vehicle→radiation seam).
- `TickPlans50To53` remains uncalled (pre-existing; espionage/mental-health daily
  ticks are outside this package). Garage recovery advancement is owned by
  `ExpeditionsCaravansDayOwner` instead, so the garage is not affected.