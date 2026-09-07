// SPDX-License-Identifier: MIT
using System;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Combat;
using Ashfall.Core.Inventory;
using Ashfall.Core.IO;
using Ashfall.Core.Radio;
using Ashfall.Core.Shelter;
using Xunit;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Phase 5 — Plans B86–B89 combined deterministic continuity.
    /// Direct 45-day run vs save-at-day-40 reload of days 41–45 must match.
    /// </summary>
    public class PlansB86ToB89ContinuityTests
    {
        private const int Seed = 8689;
        private const int TotalDays = 45;
        private const int SplitDay = 40;

        private static string FindDataDir()
        {
            string? dir = new DirectoryInfo(AppContext.BaseDirectory).FullName;
            for (int i = 0; i < 10 && dir != null; i++)
            {
                string probe = Path.Combine(dir, "Assets", "StreamingAssets", "Data", "aquaponics_system_catalog.json");
                if (File.Exists(probe))
                    return Path.Combine(dir, "Assets", "StreamingAssets", "Data");
                dir = Directory.GetParent(dir)?.FullName;
            }

            string cwd = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data");
            if (File.Exists(Path.Combine(cwd, "aquaponics_system_catalog.json")))
                return cwd;

            throw new DirectoryNotFoundException(
                "Assets/StreamingAssets/Data not found from " + AppContext.BaseDirectory);
        }

        private sealed class Bundle
        {
            public required AquaponicsSystem Aquaponics { get; init; }
            public required PrecisionMetrologySystem Metrology { get; init; }
            public required SignalTriangulationSystem Df { get; init; }
            public required CombatBreachingEngine Breach { get; init; }
            public required BarrierState Barrier { get; init; }
            public required InventoryContainer Inventory { get; init; }
        }

        private static Bundle Setup()
        {
            string dataDir = FindDataDir();
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var inv = new InventoryContainer { Capacity = 128, MaxWeight = 1000f };
            Assert.True(inv.TryProduce("item_insect_larvae_meal", 40));
            Assert.True(inv.TryProduce("item_biofilter_media", 4));
            Assert.True(inv.TryProduce("machinist_caliper", 1));
            Assert.True(inv.TryProduce("item_gauge_block_set", 1));
            Assert.True(inv.TryProduce("item_mechanical_breach_ram", 1));

            var aqua = new AquaponicsSystem(new SeededRng(Seed), inv, _ => 1f);
            aqua.LoadCatalog(AquaponicsCatalogLoader.Load(dataDir, files, json));
            Assert.True(aqua.CommissionTank("tank_a", "tank_raft_small", "room_greenhouse").IsSuccess);
            Assert.True(aqua.StockFish("tank_a", "species_rad_tilapia", 4f, day: 1).IsSuccess);
            Assert.True(aqua.Feed("tank_a", "feed_insect_meal", 3f, day: 1).IsSuccess);

            var metro = new PrecisionMetrologySystem(new SeededRng(Seed + 1), inv);
            metro.LoadCatalog(PrecisionMetrologyCatalogLoader.Load(dataDir, files, json));
            Assert.True(metro.CalibrateInstrument(
                "workshop_precision", "std_machinist_caliper", day: 1).IsSuccess);

            var dfDto = DirectionFindingCatalogLoader.Load(dataDir, files, json);
            DirectionFindingCatalogLoader.Validate(dfDto);
            var df = new SignalTriangulationSystem();
            df.LoadCatalog(DirectionFindingCatalogLoader.Build(dfDto));
            df.RecordObservation(new RadioObservation
            {
                signalId = "sig_civil_defense",
                stationId = "station_shelter_primary",
                day = 1,
                hour = 12f,
                bearingDegrees = 40f,
                errorDegrees = 1.0f,
                signalStrength = 0.9f,
                noiseLevel = 0.05f,
                frequencyMhz = 12.5f,
                weatherCondition = "Clear",
                operatorSkill = 0.85f
            }, new SeededRng(Seed + 2));

            var breachCatalog = BreachingCatalogLoader.Load(dataDir, files, json);
            BreachingCatalogLoader.Validate(breachCatalog);
            var breach = new CombatBreachingEngine(new SeededRng(Seed + 3));
            breach.LoadCatalog(breachCatalog);
            var barrier = new BarrierState
            {
                Id = "bar_continuity",
                Lane = (int)CombatLane.Center,
                IsPlayer = true,
                ObstacleProfileId = "obstacle_sandbag_redoubt",
                IntegrityPct = 100f,
                PathBlocking = 1f,
                BreachPhase = BreachPhaseIds.Available
            };
            Assert.True(breach.Begin(barrier, "breach_tool_mechanical_ram",
                operatorSkill01: 0.7f, inventory: inv).Success);

            return new Bundle
            {
                Aquaponics = aqua,
                Metrology = metro,
                Df = df,
                Breach = breach,
                Barrier = barrier,
                Inventory = inv
            };
        }

        private static void TickDay(Bundle b, int day)
        {
            if (day % 2 == 0)
                b.Aquaponics.Feed("tank_a", "feed_insect_meal", 2f, day);
            b.Aquaponics.TickDay(day, temperatureModifier: 1f, pumpReliability01: 1f);
            b.Metrology.TickDay(day);

            if (!string.Equals(b.Barrier.BreachPhase, BreachPhaseIds.Cleared, StringComparison.Ordinal)
                && !string.Equals(b.Barrier.BreachPhase, BreachPhaseIds.Failed, StringComparison.Ordinal)
                && !string.Equals(b.Barrier.BreachPhase, BreachPhaseIds.Abandoned, StringComparison.Ordinal))
            {
                b.Breach.Advance(b.Barrier, operatorSkill01: 0.75f, rng: new SeededRng(Seed * 100 + day));
            }

            if (day == 10 || day == 25 || day == 40)
            {
                b.Df.RecordObservation(new RadioObservation
                {
                    signalId = "sig_civil_defense",
                    stationId = day % 2 == 0 ? "station_ridge_outrigger" : "station_shelter_primary",
                    day = day,
                    hour = 14f,
                    bearingDegrees = day % 2 == 0 ? 95f : 42f,
                    errorDegrees = 1.2f,
                    signalStrength = 0.85f,
                    noiseLevel = 0.08f,
                    frequencyMhz = 12.5f,
                    weatherCondition = day == 25 ? "Overcast" : "Clear",
                    operatorSkill = 0.8f
                }, new SeededRng(Seed + day));
            }
        }

        private static string Serialize(object? state) =>
            System.Text.Json.JsonSerializer.Serialize(
                state,
                new System.Text.Json.JsonSerializerOptions { WriteIndented = false });

        [Fact]
        public void Combined45Day_SaveAt40_Reload_MatchesDirectRun()
        {
            var direct = Setup();
            for (int day = 1; day <= TotalDays; day++)
                TickDay(direct, day);

            var split = Setup();
            for (int day = 1; day <= SplitDay; day++)
                TickDay(split, day);

            var aquaSaved = split.Aquaponics.CaptureState();
            var metroSaved = split.Metrology.CaptureState();
            var dfSaved = split.Df.CaptureState();
            var barrierSaved = new BarrierState
            {
                Id = split.Barrier.Id,
                Lane = split.Barrier.Lane,
                IsPlayer = split.Barrier.IsPlayer,
                MaterialId = split.Barrier.MaterialId,
                IntegrityPct = split.Barrier.IntegrityPct,
                ArmorRating = split.Barrier.ArmorRating,
                ObstacleProfileId = split.Barrier.ObstacleProfileId,
                ActiveBreachToolId = split.Barrier.ActiveBreachToolId,
                BreachPhase = split.Barrier.BreachPhase,
                BreachSetupTicksRemaining = split.Barrier.BreachSetupTicksRemaining,
                BreachClearTicksRemaining = split.Barrier.BreachClearTicksRemaining,
                BreachClearTicksTotal = split.Barrier.BreachClearTicksTotal,
                BreachProgress01 = split.Barrier.BreachProgress01,
                PathBlocking = split.Barrier.PathBlocking,
                CoverContribution = split.Barrier.CoverContribution,
                BreachDestroysCover = split.Barrier.BreachDestroysCover
            };

            var restored = Setup();
            restored.Aquaponics.RestoreState(aquaSaved);
            restored.Metrology.RestoreState(metroSaved);
            restored.Df.RestoreState(dfSaved);
            restored.Barrier.Id = barrierSaved.Id;
            restored.Barrier.Lane = barrierSaved.Lane;
            restored.Barrier.IsPlayer = barrierSaved.IsPlayer;
            restored.Barrier.MaterialId = barrierSaved.MaterialId;
            restored.Barrier.IntegrityPct = barrierSaved.IntegrityPct;
            restored.Barrier.ArmorRating = barrierSaved.ArmorRating;
            restored.Barrier.ObstacleProfileId = barrierSaved.ObstacleProfileId;
            restored.Barrier.ActiveBreachToolId = barrierSaved.ActiveBreachToolId;
            restored.Barrier.BreachPhase = barrierSaved.BreachPhase;
            restored.Barrier.BreachSetupTicksRemaining = barrierSaved.BreachSetupTicksRemaining;
            restored.Barrier.BreachClearTicksRemaining = barrierSaved.BreachClearTicksRemaining;
            restored.Barrier.BreachClearTicksTotal = barrierSaved.BreachClearTicksTotal;
            restored.Barrier.BreachProgress01 = barrierSaved.BreachProgress01;
            restored.Barrier.PathBlocking = barrierSaved.PathBlocking;
            restored.Barrier.CoverContribution = barrierSaved.CoverContribution;
            restored.Barrier.BreachDestroysCover = barrierSaved.BreachDestroysCover;

            for (int day = SplitDay + 1; day <= TotalDays; day++)
                TickDay(restored, day);

            Assert.Equal(Serialize(direct.Aquaponics.CaptureState()), Serialize(restored.Aquaponics.CaptureState()));
            Assert.Equal(Serialize(direct.Metrology.CaptureState()), Serialize(restored.Metrology.CaptureState()));
            Assert.Equal(Serialize(direct.Df.CaptureState()), Serialize(restored.Df.CaptureState()));
            Assert.Equal(direct.Barrier.BreachPhase, restored.Barrier.BreachPhase);
            Assert.Equal(direct.Barrier.BreachProgress01, restored.Barrier.BreachProgress01, 4);
            Assert.Equal(direct.Barrier.PathBlocking, restored.Barrier.PathBlocking, 4);
        }

        [Fact]
        public void Combined45Day_ReplayParity_SameSeed()
        {
            var a = Setup();
            var b = Setup();
            for (int day = 1; day <= TotalDays; day++)
            {
                TickDay(a, day);
                TickDay(b, day);
            }

            Assert.Equal(Serialize(a.Aquaponics.CaptureState()), Serialize(b.Aquaponics.CaptureState()));
            Assert.Equal(Serialize(a.Metrology.CaptureState()), Serialize(b.Metrology.CaptureState()));
            Assert.Equal(Serialize(a.Df.CaptureState()), Serialize(b.Df.CaptureState()));
            Assert.Equal(a.Barrier.BreachPhase, b.Barrier.BreachPhase);
            Assert.Equal(a.Barrier.BreachProgress01, b.Barrier.BreachProgress01, 5);
        }
    }
}
