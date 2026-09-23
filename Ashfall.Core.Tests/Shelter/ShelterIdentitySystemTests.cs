// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    public sealed class ShelterIdentitySystemTests
    {
        [Fact]
        public void Defaults_HaveDefaultNameAndSixOrigins()
        {
            var system = new ShelterIdentitySystem();

            Assert.Equal("The Shelter", system.ShelterName);
            Assert.Equal(6, system.Origins.Count);
            Assert.True(system.Origins.ContainsKey("origin_government_bunker"));
            Assert.True(system.Origins.ContainsKey("origin_mining_facility"));
            Assert.True(system.Origins.ContainsKey("origin_school_basement"));
            Assert.True(system.Origins.ContainsKey("origin_private_vault"));
            Assert.True(system.Origins.ContainsKey("origin_improvised_cellar"));
            Assert.True(system.Origins.ContainsKey("origin_military_outpost"));
        }

        [Fact]
        public void SetShelterName_ValidatesLengthAndRejectsInvalid()
        {
            var system = new ShelterIdentitySystem();

            // Valid naming
            var res1 = system.SetShelterName("Haven 101");
            Assert.True(res1.IsSuccess);
            Assert.Equal("Haven 101", system.ShelterName);

            // Empty or whitespace
            var res2 = system.SetShelterName("   ");
            Assert.False(res2.IsSuccess);
            Assert.Equal("empty_name", res2.FailureCode);

            // Too short
            var res3 = system.SetShelterName("X");
            Assert.False(res3.IsSuccess);
            Assert.Equal("invalid_length", res3.FailureCode);

            // Too long (> 40 chars)
            var res4 = system.SetShelterName(new string('A', 41));
            Assert.False(res4.IsSuccess);
            Assert.Equal("invalid_length", res4.FailureCode);
        }

        [Fact]
        public void SelectOrigin_SetsOriginAndProvidesBonuses()
        {
            var system = new ShelterIdentitySystem();

            var res = system.SelectOrigin("origin_government_bunker", day: 2, founderSurvivorId: "surv_colonel");
            Assert.True(res.IsSuccess);
            Assert.Equal("origin_government_bunker", system.OriginId);

            var origin = system.GetSelectedOrigin();
            Assert.NotNull(origin);
            Assert.Equal(2500, origin!.radiation_shielding_bp);
            Assert.Contains("radiation_shielding_bonus", origin.starting_bonuses);
            Assert.Contains("cramped_quarters", origin.starting_drawbacks);
        }

        [Fact]
        public void RecordFactionReputation_ClampsBetweenMinus1000AndPlus1000()
        {
            var system = new ShelterIdentitySystem();

            system.RecordFactionReputation("faction_salvagers", 500);
            Assert.Equal(500, system.GetFactionReputation("faction_salvagers"));

            system.RecordFactionReputation("faction_salvagers", 700);
            Assert.Equal(1000, system.GetFactionReputation("faction_salvagers")); // Clamped to 1000

            system.RecordFactionReputation("faction_raiders", -1200);
            Assert.Equal(-1000, system.GetFactionReputation("faction_raiders")); // Clamped to -1000
        }

        [Fact]
        public void CommunityActions_EvolveEmergentKnownForTags()
        {
            var system = new ShelterIdentitySystem();

            // Initial state: default tag
            var tags0 = system.GetKnownForTags();
            Assert.Contains("Survivors", tags0);

            // Add 5 trade actions
            system.RecordCommunityAction("trade", 5);
            var tags1 = system.GetKnownForTags();
            Assert.Contains("Traders", tags1);

            // Add 5 medical actions
            system.RecordCommunityAction("medical", 5);
            var tags2 = system.GetKnownForTags();
            Assert.Contains("Traders", tags2);
            Assert.Contains("Healers", tags2);

            // Add 5 raid actions (which also raises infamy)
            system.RecordCommunityAction("raid", 5);
            var tags3 = system.GetKnownForTags();
            Assert.Contains("Raiders", tags3);
            Assert.True(system.Infamy > 0);
        }

        [Fact]
        public void FormatText_InterpolatesTokensAccurately()
        {
            var system = new ShelterIdentitySystem();
            system.SetShelterName("Iron Redoubt");
            system.SetMotto("Through Fire We Endure");
            system.SelectOrigin("origin_military_outpost", founderSurvivorId: "Commander Miller");

            string template = "Welcome to {shelter_name}. Founded by {founder} as a {origin_name}. Motto: \"{motto}\".";
            string formatted = system.FormatText(template);

            Assert.Equal("Welcome to Iron Redoubt. Founded by Commander Miller as a Hardened Military Outpost. Motto: \"Through Fire We Endure\".", formatted);
        }

        [Fact]
        public void SaveLoad_RoundTrip_PreservesAllIdentityAndReputation()
        {
            var sys1 = new ShelterIdentitySystem();
            sys1.SetShelterName("Silo 7");
            sys1.SetMotto("Vigilance Forever");
            sys1.SetEmblem("wolf", "crimson");
            sys1.SelectOrigin("origin_mining_facility", day: 4, founderSurvivorId: "Chief Miner");
            sys1.RecordFactionReputation("faction_guild", 350);
            sys1.RecordCommunityAction("trade", 6);
            sys1.AdjustInfamy(15);

            var saved = sys1.CaptureState();

            var sys2 = new ShelterIdentitySystem();
            sys2.RestoreState(saved);

            Assert.Equal("Silo 7", sys2.ShelterName);
            Assert.Equal("Vigilance Forever", sys2.Motto);
            Assert.Equal("wolf", sys2.State.emblem_symbol);
            Assert.Equal("crimson", sys2.State.emblem_color);
            Assert.Equal("origin_mining_facility", sys2.OriginId);
            Assert.Equal(4, sys2.State.founding_day);
            Assert.Equal("Chief Miner", sys2.State.founder_survivor_id);
            Assert.Equal(350, sys2.GetFactionReputation("faction_guild"));
            Assert.Equal(15, sys2.Infamy);
            Assert.Contains("Traders", sys2.GetKnownForTags());
        }
    }
}
