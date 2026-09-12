// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    /// <summary>
    /// SHELTER_GRID_CATALOG_SEAL Phase 2: pins the shipped power_grid.json
    /// authority as consumed by ShelterPowerGridCatalogLoader. The historical
    /// 18-room assertion was removed from compilation (csproj quarantine)
    /// because it disagreed with the shipped 6-room data; this repaired file
    /// pins the shipped rooms (plus room_workshop added by the seal wave) so
    /// future catalog edits are deliberate, test-reviewed changes.
    /// </summary>
    public sealed class PowerGridCatalogTests
    {
        private static string FindDataDir()
        {
            string dataDir;
            if (!CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out dataDir))
                CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out dataDir);
            return dataDir ?? string.Empty;
        }

        private static ShelterPowerGridCatalogDef LoadCatalog()
        {
            var ok = ShelterPowerGridCatalogLoader.TryLoad(FindDataDir(), new FileSystemIO(),
                new SystemTextJsonSerializer(), out var catalog, out var error);
            Assert.True(ok, $"power_grid.json must load strictly: {error}");
            return catalog!;
        }

        [Fact]
        public void Catalog_HasValidSchemaVersion()
        {
            var catalog = LoadCatalog();
            Assert.Equal(1, catalog.SchemaVersion);
            Assert.Equal(800f, catalog.GenerationWattsDefault);
            Assert.Equal(4000f, catalog.BatteryCapacityWhDefault);
            Assert.Equal(100f, catalog.FuelUnitsDefault);
        }

        [Fact]
        public void Catalog_HasAtLeastSevenRooms()
        {
            // Lower bound, not exact: the catalog is a living authority and
            // concurrent streams add rooms (e.g. room_cryo_vault). The pinned
            // per-room contract below is the real gate.
            var catalog = LoadCatalog();
            Assert.True(catalog.Rooms.Count >= 7, $"expected >= 7 rooms, got {catalog.Rooms.Count}");
        }

        [Fact]
        public void Catalog_PropagatesSurgeTuning()
        {
            // SHELTER_HARDENING: surge tuning is catalog-driven (fields optional).
            var catalog = LoadCatalog();
            Assert.True(catalog.EmpStormSeverity is >= 0f and <= 1f,
                $"emp_storm_severity out of range: {catalog.EmpStormSeverity}");
            Assert.True(catalog.SurgeBatteryDrainFraction is >= 0f and <= 1f,
                $"surge_battery_drain_fraction out of range: {catalog.SurgeBatteryDrainFraction}");
        }

        [Fact]
        public void Catalog_MissingSurgeTuning_FallsBackToDefaults()
        {
            var catalog = ShelterPowerGridCatalogLoader.FallbackDefault();
            Assert.Null(catalog.EmpStormSeverity); // absent → system defaults apply
            Assert.Null(catalog.SurgeBatteryDrainFraction);
            // Loader validation still passes (optional fields).
            var ok = ShelterPowerGridCatalogLoader.Validate(catalog, out var error);
            Assert.True(ok, error);
        }

        [Fact]
        public void Catalog_QuarantineWardVentilationIsCriticalTier()
        {
            // SHELTER_EMP_MEDICAL_POWER: quarantine ventilation is life-safety —
            // it must sit in the critical tier so surge shedding sheds it last.
            var catalog = LoadCatalog();
            var room = Assert.Single(catalog.Rooms, r => r.Id == "room_ward_quarantine");
            Assert.Equal("critical", room.DefaultPriority);
            Assert.True(room.DrawWatts > 0);
            Assert.Equal("fx_quarantine_ventilation_off", room.FailureEffectId);
        }

        [Fact]
        public void Catalog_EveryFailureEffectId_HasANamedConsumer()
        {
            // SHELTER_FAILURE_EFFECTS (G6 closure): every authored failure effect
            // must map to the owner that consumes the outage state. A new fx id
            // without an entry here fails the suite — the G6 bug class (authored
            // vocabulary with no consumer) cannot ship silently.
            var consumers = new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["fx_filtration_off"] = "StartingLevelSystem.TickDay powerAvailability01 (Main.CampaignOwners airPower)",
                ["fx_clinic_off"] = "MedicalPipelineCoordinator clinicPowerCheck (Main.Medical) + AdvanceScheduled scaling (MedicalDiseaseDayOwner)",
                ["fx_water_pressure_drop"] = "Plan168FluidDayOwner power derivation (Main.Plans166_169)",
                ["fx_grow_lights_off"] = "BuildAgricultureEnvironment LightingAvailabilityPermille (Main.Plans162_165)",
                ["fx_foundry_standstill"] = "SilentFoundryHostSession power gate (room_foundry/room_workshop)",
                ["fx_lighting_dim"] = "ShelterScheduleSystem brownout lighting demand",
                ["fx_workshop_offline"] = "Main.World workshop power gate (room_workshop)",
                ["fx_cryo_vault_unpowered"] = "CryoVaultSystem powerAvailableProvider ← Main.PlansB68_B69.IsCryoVaultPowered (room_cryo_vault)",
                ["fx_quarantine_ventilation_off"] = "DiseaseQuarantineCoordinator isolationPowerCheck (Main.SetupDisease)",
                // Plan 71 — nine new powered rooms. Consumers that were already
                // querying these IDs before the entries existed (the dead-query
                // bug class) become live with the data alone; the rest are wired
                // at existing host call sites (see PLAN71_COMPLETION_REPORT.md).
                ["fx_heating_off"] = "ResourceMassBalanceSimulator isNearHeatSource (room_heating)",
                ["fx_kitchen_off"] = "ResourceMassBalanceSimulator kitchenPower (room_kitchen)",
                ["fx_water_filtration_off"] = "ResourceMassBalanceSimulator waterPower (room_water_filtration)",
                ["fx_airlock_decon_off"] = "PerimeterDefenseSystem assault airlock power (InfrastructureHeadlessDemo defense path)",
                ["fx_radio_tuner_off"] = "ShelterRadioStationSystem.TickDay monitoring pause (Main.Plans46_49)",
                ["fx_laboratory_offline"] = "ResearchHostSession.StartGate research-start block (Main.PlayerSurfaces)",
                ["fx_precision_metrology_off"] = "PrecisionMetrology drift/projection pause (Main.PlansB86_B89 TickPrecisionMetrology)",
                ["fx_common_mess_cold"] = "ShelterDecorHostSession.ApplyDailyMorale pause (Main.CampaignOwners SurvivorsNeedsDayOwner)",
                ["fx_armory_service_off"] = "DefenseSystem isEmplacementPowered armory circuit (Main.Plans162_165 SetupDefense)",
            };

            var catalog = LoadCatalog();
            foreach (var room in catalog.Rooms)
            {
                var fx = room.FailureEffectId;
                Assert.True(
                    !string.IsNullOrEmpty(fx) && consumers.ContainsKey(fx),
                    $"room '{room.Id}' authored failure_effect_id '{fx}' has no registered consumer. " +
                    "Add the consuming system to this map (and wire it) before shipping.");
            }
        }

        [Fact]
        public void Catalog_PreservesShippedRoomsInOrder()
        {
            var catalog = LoadCatalog();

            Assert.Equal("room_air_filtration", catalog.Rooms[0].Id);
            Assert.Equal("Air Filtration", catalog.Rooms[0].DisplayName);
            Assert.Equal(180f, catalog.Rooms[0].DrawWatts);
            Assert.Equal("critical", catalog.Rooms[0].DefaultPriority);
            Assert.Equal("fx_filtration_off", catalog.Rooms[0].FailureEffectId);

            Assert.Equal("room_clinic", catalog.Rooms[1].Id);
            Assert.Equal("Clinic", catalog.Rooms[1].DisplayName);
            Assert.Equal(120f, catalog.Rooms[1].DrawWatts);
            Assert.Equal("critical", catalog.Rooms[1].DefaultPriority);
            Assert.Equal("fx_clinic_off", catalog.Rooms[1].FailureEffectId);

            Assert.Equal("room_water_pump", catalog.Rooms[2].Id);
            Assert.Equal("Water Pump", catalog.Rooms[2].DisplayName);
            Assert.Equal(100f, catalog.Rooms[2].DrawWatts);
            Assert.Equal("critical", catalog.Rooms[2].DefaultPriority);
            Assert.Equal("fx_water_pressure_drop", catalog.Rooms[2].FailureEffectId);

            Assert.Equal("room_greenhouse", catalog.Rooms[3].Id);
            Assert.Equal("Greenhouse", catalog.Rooms[3].DisplayName);
            Assert.Equal(160f, catalog.Rooms[3].DrawWatts);
            Assert.Equal("standard", catalog.Rooms[3].DefaultPriority);
            Assert.Equal("fx_grow_lights_off", catalog.Rooms[3].FailureEffectId);

            Assert.Equal("room_foundry", catalog.Rooms[4].Id);
            Assert.Equal("Silent Foundry", catalog.Rooms[4].DisplayName);
            Assert.Equal(220f, catalog.Rooms[4].DrawWatts);
            Assert.Equal("low", catalog.Rooms[4].DefaultPriority);
            Assert.Equal("fx_foundry_standstill", catalog.Rooms[4].FailureEffectId);

            Assert.Equal("room_lighting_main", catalog.Rooms[5].Id);
            Assert.Equal("Main Lighting", catalog.Rooms[5].DisplayName);
            Assert.Equal(80f, catalog.Rooms[5].DrawWatts);
            Assert.Equal("low", catalog.Rooms[5].DefaultPriority);
            Assert.Equal("fx_lighting_dim", catalog.Rooms[5].FailureEffectId);

            Assert.Equal("room_workshop", catalog.Rooms[6].Id);
            Assert.Equal("Workshop", catalog.Rooms[6].DisplayName);
            Assert.Equal(300f, catalog.Rooms[6].DrawWatts);
            Assert.Equal("low", catalog.Rooms[6].DefaultPriority);
            Assert.Equal("fx_workshop_offline", catalog.Rooms[6].FailureEffectId);
        }

        [Fact]
        public void Catalog_RoomIdsAreUnique()
        {
            var catalog = LoadCatalog();
            var ids = new HashSet<string>(catalog.Rooms.Select(r => r.Id), StringComparer.Ordinal);
            Assert.Equal(catalog.Rooms.Count, ids.Count);
        }

        [Fact]
        public void Catalog_CanonicalConsumerRoomIdsResolve()
        {
            // Room IDs queried by host code (fluid day owner, workshop gate,
            // agriculture environment) must exist in the catalog — the seal
            // wave's G1 class of bug is an unknown room ID silently reading false.
            var catalog = LoadCatalog();
            var ids = new HashSet<string>(catalog.Rooms.Select(r => r.Id), StringComparer.Ordinal);
            Assert.Contains("room_water_pump", ids);
            Assert.Contains("room_workshop", ids);
            Assert.Contains("room_greenhouse", ids);
            Assert.Contains("room_clinic", ids);
            // Plan 71: room IDs queried by downstream systems before their grid
            // entries existed (the dead-query class) must now resolve.
            Assert.Contains("room_heating", ids);
            Assert.Contains("room_kitchen", ids);
            Assert.Contains("room_water_filtration", ids);
            Assert.Contains("room_airlock", ids);
            // Plan 71: Plan 41 shelter rooms with electrical profiles.
            Assert.Contains("room_radio_tuner", ids);
            Assert.Contains("room_laboratory_research", ids);
            Assert.Contains("room_workshop_precision", ids);
            Assert.Contains("room_common_mess_hall", ids);
            Assert.Contains("room_armory_munitions", ids);
        }
    }
}
