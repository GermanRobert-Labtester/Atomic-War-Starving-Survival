// SPDX-License-Identifier: MIT
using System;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests.Narrative
{
    public class CeremonySystemTests
    {
        private static string GetCatalogJson()
        {
            string path = Path.Combine(AppContext.BaseDirectory, "Assets/StreamingAssets/Data/ceremonies.json");
            if (!File.Exists(path))
            {
                path = Path.Combine(AppContext.BaseDirectory, "../../../Assets/StreamingAssets/Data/ceremonies.json");
            }
            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }

            return @"{
  ""schema_version"": 1,
  ""ceremonies"": [
    {
      ""id"": ""ceremony_founding_day"",
      ""display_name"": ""Shelter Founding Day Anniversary"",
      ""preparation_days"": 3,
      ""required_room_id"": ""room_common_mess_hall"",
      ""min_population"": 4,
      ""required_items"": [
        { ""item_id"": ""canned_food"", ""quantity"": 10 },
        { ""item_id"": ""clean_water"", ""quantity"": 10 }
      ],
      ""morale_boost"": 25.0,
      ""stress_relief"": 20.0,
      ""truce_duration_days"": 0,
      ""truce_eligible"": false,
      ""disaster_pool"": [
        ""disaster_food_spoilage"",
        ""disaster_drunken_brawl""
      ],
      ""description"": ""Test founding""
    },
    {
      ""id"": ""ceremony_treaty_market"",
      ""display_name"": ""Grand Treaty Barter Fair"",
      ""preparation_days"": 2,
      ""required_room_id"": ""room_common_mess_hall"",
      ""min_population"": 4,
      ""required_items"": [
        { ""item_id"": ""canned_food"", ""quantity"": 5 }
      ],
      ""morale_boost"": 20.0,
      ""stress_relief"": 15.0,
      ""truce_duration_days"": 3,
      ""truce_eligible"": true,
      ""disaster_pool"": [
        ""disaster_faction_snub""
      ],
      ""description"": ""Test market""
    }
  ]
}";
        }

        [Fact]
        public void ScheduleCeremony_ValidConditions_TransitionsToPreparing()
        {
            var sys = new CeremonySystem(new SeededRng(300));
            sys.LoadCatalog(GetCatalogJson());

            bool success = sys.ScheduleCeremony("ceremony_founding_day", 10, 5, out string error);
            Assert.True(success);
            Assert.NotNull(sys.ActiveCeremony);
            Assert.Equal(CeremonyPhase.Preparing, sys.ActiveCeremony.Phase);
            Assert.Equal(3, sys.ActiveCeremony.PreparationDaysRemaining);
        }

        [Fact]
        public void ScheduleCeremony_InsufficientPopulation_Fails()
        {
            var sys = new CeremonySystem(new SeededRng(301));
            sys.LoadCatalog(GetCatalogJson());

            bool success = sys.ScheduleCeremony("ceremony_founding_day", 10, 2, out string error); // min is 4
            Assert.False(success);
            Assert.Contains("population too low", error, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void ScheduleCeremony_ConflictingActiveCeremony_Fails()
        {
            var sys = new CeremonySystem(new SeededRng(302));
            sys.LoadCatalog(GetCatalogJson());

            sys.ScheduleCeremony("ceremony_founding_day", 10, 5, out _);
            bool second = sys.ScheduleCeremony("ceremony_treaty_market", 10, 5, out string error);
            Assert.False(second);
            Assert.Contains("currently scheduled", error, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void ContributeResource_AddsCommittedItems()
        {
            var sys = new CeremonySystem(new SeededRng(303));
            sys.LoadCatalog(GetCatalogJson());

            sys.ScheduleCeremony("ceremony_founding_day", 10, 5, out _);
            bool contributed = sys.ContributeResource("canned_food", 6);
            Assert.True(contributed);

            Assert.Equal(6, sys.ActiveCeremony!.CommittedItems["canned_food"]);

            // Adding more accumulates
            sys.ContributeResource("canned_food", 4);
            Assert.Equal(10, sys.ActiveCeremony.CommittedItems["canned_food"]);
        }

        [Fact]
        public void InviteFaction_NeutralOrPositiveStanding_AcceptsAndRequestsTruce()
        {
            var sys = new CeremonySystem(new SeededRng(304));
            sys.LoadCatalog(GetCatalogJson());

            sys.ScheduleCeremony("ceremony_treaty_market", 15, 6, out _);

            string? truceFaction = null;
            int truceDays = 0;
            sys.OnTruceRequested += (f, d) =>
            {
                truceFaction = f;
                truceDays = d;
            };

            bool invited = sys.InviteFaction("faction_archivists", 10);
            Assert.True(invited);
            Assert.Contains("faction_archivists", sys.ActiveCeremony!.AcceptedFactions);
            Assert.Equal("faction_archivists", truceFaction);
            Assert.Equal(3, truceDays);
        }

        [Fact]
        public void InviteFaction_NegativeStanding_DoesNotAccept()
        {
            var sys = new CeremonySystem(new SeededRng(305));
            sys.LoadCatalog(GetCatalogJson());

            sys.ScheduleCeremony("ceremony_treaty_market", 15, 6, out _);

            bool invited = sys.InviteFaction("faction_hostile_raiders", -45);
            Assert.True(invited);
            Assert.Contains("faction_hostile_raiders", sys.ActiveCeremony!.InvitedFactions);
            Assert.DoesNotContain("faction_hostile_raiders", sys.ActiveCeremony.AcceptedFactions);
        }

        [Fact]
        public void TickDay_PreparationAndReadiness_TransitionsToReady()
        {
            var sys = new CeremonySystem(new SeededRng(306));
            sys.LoadCatalog(GetCatalogJson());

            sys.ScheduleCeremony("ceremony_treaty_market", 20, 6, out _);
            sys.ContributeResource("canned_food", 5); // requirement met

            // 2 preparation days required
            sys.TickDay(21, out _);
            Assert.Equal(CeremonyPhase.Preparing, sys.ActiveCeremony!.Phase);

            sys.TickDay(22, out _);
            Assert.Equal(CeremonyPhase.Ready, sys.ActiveCeremony.Phase);
        }

        [Fact]
        public void TickDay_ReadyPhase_CommencesFestivalAppliesMorale()
        {
            var sys = new CeremonySystem(new SeededRng(307));
            sys.LoadCatalog(GetCatalogJson());

            sys.ScheduleCeremony("ceremony_treaty_market", 20, 6, out _);
            sys.ContributeResource("canned_food", 5);
            sys.TickDay(21, out _);
            sys.TickDay(22, out _); // Now Ready

            float moraleGiven = 0f;
            float stressReliefGiven = 0f;
            sys.OnMoraleBoostRequested += (m, s) =>
            {
                moraleGiven = m;
                stressReliefGiven = s;
            };

            bool completed = false;
            sys.OnCeremonyCompleted += c => completed = true;

            sys.TickDay(23, out string summary);

            Assert.Equal(CeremonyPhase.Completed, sys.ActiveCeremony!.Phase);
            Assert.True(completed);
            Assert.Equal(20.0f, moraleGiven);
            Assert.Equal(15.0f, stressReliefGiven);
            Assert.Equal(1, sys.State.TotalCeremoniesHeld);
        }

        [Fact]
        public void SaveRestore_PreservesScheduledCeremonyAndHistory()
        {
            var sys1 = new CeremonySystem(new SeededRng(308));
            sys1.LoadCatalog(GetCatalogJson());

            sys1.ScheduleCeremony("ceremony_founding_day", 10, 5, out _);
            sys1.ContributeResource("canned_food", 4);
            sys1.InviteFaction("faction_supply_corps", 15);

            var saved = sys1.CaptureState();

            var sys2 = new CeremonySystem(new SeededRng(309));
            sys2.RestoreState(saved);

            Assert.NotNull(sys2.ActiveCeremony);
            Assert.Equal("ceremony_founding_day", sys2.ActiveCeremony.CeremonyId);
            Assert.Equal(4, sys2.ActiveCeremony.CommittedItems["canned_food"]);
            Assert.Single(sys2.ActiveCeremony.AcceptedFactions);
            Assert.Equal("faction_supply_corps", sys2.ActiveCeremony.AcceptedFactions[0]);
        }

        [Fact]
        public void DeterministicReplay_SameSeedProducesIdenticalDisasterOutcomes()
        {
            var sysA = new CeremonySystem(new SeededRng(555));
            var sysB = new CeremonySystem(new SeededRng(555));
            sysA.LoadCatalog(GetCatalogJson());
            sysB.LoadCatalog(GetCatalogJson());

            sysA.ScheduleCeremony("ceremony_founding_day", 10, 5, out _);
            sysB.ScheduleCeremony("ceremony_founding_day", 10, 5, out _);

            sysA.ContributeResource("canned_food", 10);
            sysA.ContributeResource("clean_water", 10);
            sysB.ContributeResource("canned_food", 10);
            sysB.ContributeResource("clean_water", 10);

            for (int i = 0; i < 4; i++)
            {
                sysA.TickDay(10 + i, out _);
                sysB.TickDay(10 + i, out _);
            }

            Assert.Equal(sysA.ActiveCeremony!.OccurredDisasterId, sysB.ActiveCeremony!.OccurredDisasterId);
            Assert.Equal(sysA.State.TotalDisastersEncountered, sysB.State.TotalDisastersEncountered);
        }
    }
}
