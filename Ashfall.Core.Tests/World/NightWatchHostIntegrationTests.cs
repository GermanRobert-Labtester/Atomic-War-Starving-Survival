// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Ashfall.Core;
using Ashfall.Core.Defense;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.World
{
    public sealed class NightWatchHostIntegrationTests
    {
        private static string RepoFile(params string[] parts)
        {
            string candidate = Path.Combine(AppContext.BaseDirectory, "../../../..", Path.Combine(parts));
            if (File.Exists(candidate)) return candidate;
            return Path.Combine(Directory.GetCurrentDirectory(), Path.Combine(parts));
        }

        [Fact]
        public void WatchShiftState_IsCanonicalRosterState_AndRoundTrips()
        {
            var roster = new DutyRosterSystem();
            roster.RestoreState(new DutyRosterSystemState
            {
                expansionUnlocked = true,
                rows = new List<DutyRosterRow>
                {
                    new DutyRosterRow { survivorId = "sv_watch", displayName = "Watcher", status = DutyRosterIds.StatusHome }
                }
            });
            var result = roster.AssignWatchShift("shift_1", "watch_post_main_gate", "sv_watch", 4, 18, 4);
            Assert.True(result.IsSuccess, result.FailureCode);
            Assert.Equal(1, roster.GetWatchShiftCount("watch_post_main_gate", 4));
            Assert.Equal(1, roster.GetWatchShifts().Count);

            var saved = roster.CaptureState();
            var restored = new DutyRosterSystem();
            restored.RestoreState(saved);
            Assert.Equal(1, restored.GetWatchShiftCount("watch_post_main_gate", 4));
            Assert.Equal("sv_watch", restored.GetWatchShifts()[0].survivorId);

            var serializer = new SystemTextJsonSerializer();
            var encoded = DutyRosterSaveCodec.Encode(
                DutyRosterSaveCodec.Capture(
                    roster,
                    new MoraleMarkSystem(),
                    new ShelterEncounterSystem(1208),
                    new SimClock(4)),
                serializer);
            var decoded = DutyRosterSaveCodec.Decode(encoded, serializer);
            var codecRestored = new DutyRosterSystem();
            DutyRosterSaveCodec.Restore(
                decoded,
                codecRestored,
                new MoraleMarkSystem(),
                new ShelterEncounterSystem(1208),
                new SimClock(0));
            Assert.Equal(1, codecRestored.GetWatchShiftCount("watch_post_main_gate", 4));

            var newer = roster.CaptureState();
            newer.watch_schema_version = 99;
            Assert.Throws<InvalidOperationException>(() => new DutyRosterSystem().RestoreState(newer));
        }

        [Fact]
        public void WatchShiftAssignment_PreservesFatigueAndRejectsCrossDayShifts()
        {
            var roster = new DutyRosterSystem();
            roster.RestoreState(new DutyRosterSystemState
            {
                rows = new List<DutyRosterRow>
                {
                    new DutyRosterRow { survivorId = "sv_watch", displayName = "Watcher", status = DutyRosterIds.StatusHome }
                }
            });

            var assigned = roster.AssignWatchShift(
                "shift_fatigue",
                "watch_post_main_gate",
                "sv_watch",
                day: 4,
                startHour: 18,
                durationHours: 4,
                fatigueBeforePermille: 640);
            Assert.True(assigned.IsSuccess, assigned.FailureCode);
            Assert.Equal(640, roster.GetWatchShifts()[0].fatigue_before_permille);

            var crossesDay = roster.AssignWatchShift(
                "shift_crosses_day",
                "watch_post_main_gate",
                "sv_watch",
                day: 4,
                startHour: 23,
                durationHours: 2);
            Assert.False(crossesDay.IsSuccess);
            Assert.Equal("watch.invalid_shift_time", crossesDay.MessageKey);
        }

        [Fact]
        public void ReadinessEvaluation_EmitsOnlyWhenDerivedSnapshotChanges()
        {
            var perimeter = new PerimeterDefenseSystem(
                Array.Empty<PerimeterDefenseDefinition>(),
                new Ashfall.Core.Inventory.Inventory(),
                new SeededRng(361));
            int readinessEvents = 0;
            perimeter.OnEventRaised += name =>
            {
                if (string.Equals(name, "watch.readiness_evaluated", StringComparison.Ordinal))
                    readinessEvents++;
            };
            var snapshot = NightWatchReadinessProjection.Evaluate(new NightWatchSectorFacts
            {
                SectorId = "gate",
                AssignedPatrollers = 1,
                ActivePostCount = 1,
                PostConditionPermille = 1000,
                NightVisionPermille = 250,
                GateStaffPermille = 500,
                GateMechanismPermille = 900,
                AlarmOnline = true
            });

            perimeter.RecordWatchReadiness(3, snapshot);
            Assert.Equal(1, readinessEvents);
            perimeter.RecordWatchReadiness(3, snapshot);
            Assert.Equal(1, readinessEvents);
        }

        [Fact]
        public void LegacyDutyRosterChecksum_RemainsReadableAfterWatchFieldsWereAdded()
        {
            var serializer = new SystemTextJsonSerializer();
            var save = new DutyRosterSave
            {
                saveVersion = DutyRosterSave.CurrentSaveVersion,
                simDay = 12,
                roster = new DutyRosterSystemState { expansionUnlocked = true },
                marks = new MoraleMarkSystemState(),
                encounters = new ShelterEncounterSystemState(),
                overflow = new DutyRosterOverflowState(),
                quests = new DutyRosterQuestState()
            };
            var legacyMethod = typeof(DutyRosterSaveCodec).GetMethod(
                "ComputeLegacyV3Checksum",
                BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(legacyMethod);
            save.Checksum = (string)legacyMethod!.Invoke(null, new object[] { save })!;

            var restored = DutyRosterSaveCodec.Decode(serializer.Serialize(save), serializer);
            Assert.Equal(DutyRosterSave.CurrentSaveVersion, restored.saveVersion);
            Assert.True(restored.roster.expansionUnlocked);
        }

        [Fact]
        public void CanonicalOwnerComposition_ProducesReadinessWithoutShadowState()
        {
            var catalog = NightWatchOperationsCatalogLoader.LoadFromJson(File.ReadAllText(RepoFile("Assets", "StreamingAssets", "Data", "night_watch_operations.json"))).Catalog!;
            var perimeter = new PerimeterDefenseSystem(
                Array.Empty<PerimeterDefenseDefinition>(),
                new Ashfall.Core.Inventory.Inventory(),
                new SeededRng(360));
            perimeter.BindWatchCatalog(catalog);
            var roster = new DutyRosterSystem();
            roster.RestoreState(new DutyRosterSystemState
            {
                rows = new List<DutyRosterRow>
                {
                    new DutyRosterRow { survivorId = "sv_watch", displayName = "Watcher", status = DutyRosterIds.StatusHome }
                }
            });
            Assert.True(roster.AssignWatchShift("shift_1", "watch_post_main_gate", "sv_watch", 4, 18, 4).IsSuccess);
            var facts = new NightWatchSectorFacts
            {
                SectorId = "gate",
                AssignedPatrollers = roster.GetWatchShiftCount("watch_post_main_gate", 4),
                ActivePostCount = perimeter.GetWatchPostCount("gate"),
                PostConditionPermille = perimeter.GetAverageWatchPostCondition("gate"),
                NightVisionPermille = perimeter.GetAverageWatchPostNightVision("gate"),
                GateStaffPermille = 500,
                GateMechanismPermille = perimeter.GetAverageWatchPostCondition("gate"),
                AlarmOnline = false
            };
            var snapshot = NightWatchReadinessProjection.Evaluate(facts);
            Assert.True(snapshot.PatrolCoverage.DetectionProbabilityPermille > 0);
            Assert.False(snapshot.GateReadiness.IsGateReady);
            Assert.Equal(0, perimeter.WatchOperations.posts.Find(x => x.post_id == "watch_post_main_gate")!.shifts_completed);
        }

        [Fact]
        public void HostSurface_UsesRealCommandsAndRoutedPanel()
        {
            string main = File.ReadAllText(RepoFile("src", "Main.NightWatch.cs"));
            string expanded = File.ReadAllText(RepoFile("src", "Main.ExpandedShelterSystems.cs"));
            string panel = File.ReadAllText(RepoFile("src", "UI", "NightWatchPanel.cs"));
            string registry = File.ReadAllText(RepoFile("Assets", "Ashfall.Core", "UI", "PanelRegistryBootstrap.cs"));
            string roster = File.ReadAllText(RepoFile("Assets", "Ashfall.Core", "DutyRoster", "DutyRosterSystem.cs"));
            string perimeter = File.ReadAllText(RepoFile("Assets", "Ashfall.Core", "Defense", "PerimeterDefenseSystem.cs"));
            Assert.Contains("SetupNightWatch", expanded);
            Assert.Contains("SaveNightWatch", expanded);
            Assert.Contains("case \"night_watch\"", expanded);
            Assert.Contains("night_watch", registry);
            Assert.Contains("AssignWatchShift", panel);
            Assert.Contains("SealGate", panel);
            Assert.Contains("WalkRoute", panel);
            Assert.Contains("RunDrill", panel);
            Assert.Contains("watch_shifts", roster);
            Assert.Contains("watch_operations", perimeter);
        }
    }
}
