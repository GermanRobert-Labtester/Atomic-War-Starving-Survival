// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Survivors
{
    public sealed class Plan182RelationshipDecayIntegrationTests
    {
        private static string GetDataPath(string filename)
        {
            var current = new DirectoryInfo(AppContext.BaseDirectory);
            while (current != null)
            {
                string candidate = Path.Combine(current.FullName, "Assets", "StreamingAssets", "Data", filename);
                if (File.Exists(candidate)) return candidate;
                current = current.Parent;
            }
            return Path.Combine("Assets", "StreamingAssets", "Data", filename);
        }

        [Fact]
        public void PairBond_InteractionUpgradesBond_AndNeglectDecaysBond()
        {
            var system = new RelationshipDecaySystem();

            var pair = system.RegisterOrUpdatePair("surv_alpha", "surv_beta", SurvivorBondType.Friend, initialAffinity: 65f, initialTrust: 55f, currentDay: 1);
            Assert.NotNull(pair);
            Assert.Equal(1, system.TrackedPairCount);

            // Interaction elevates bond to CloseFriend
            system.RecordInteraction("surv_alpha", "surv_beta", "conversation", 15f, 1);
            Assert.Equal(80f, pair.Affinity);
            Assert.Equal(SurvivorBondType.CloseFriend, pair.Bond);

            // Tick days without interaction
            for (int day = 2; day <= 100; day++)
            {
                system.TickDay(day);
            }

            Assert.True(pair.DaysWithoutInteraction > 3);
            Assert.True(pair.Affinity < 50f);
            // Social drift event should have been logged
            Assert.NotEmpty(system.DriftHistory);
        }

        [Fact]
        public void RelationshipDecay_PersistenceRoundtrip_PreservesPairsAndDrift()
        {
            var system = new RelationshipDecaySystem();
            system.RegisterOrUpdatePair("surv_1", "surv_2", SurvivorBondType.Rival, 10f, 10f, 1);

            var state = system.CaptureState();
            string json = JsonSerializer.Serialize(state);
            var restored = JsonSerializer.Deserialize<RelationshipDecayState>(json);
            Assert.NotNull(restored);

            var newSystem = new RelationshipDecaySystem();
            newSystem.RestoreState(restored!);

            Assert.Equal(1, newSystem.TrackedPairCount);
            var p = newSystem.GetPair("surv_1", "surv_2");
            Assert.NotNull(p);
            Assert.Equal(SurvivorBondType.Rival, p.Bond);
        }

        [Fact]
        public void LoadCatalog_FromCanonicalJson_LoadsAllBondProfiles()
        {
            var system = new RelationshipDecaySystem();
            string jsonPath = GetDataPath("relationship_decay_profiles.json");

            Assert.True(File.Exists(jsonPath), $"Canonical file must exist at {jsonPath}");
            string json = File.ReadAllText(jsonPath);

            system.LoadCatalog(json);
            Assert.Equal(6, system.Profiles.Count);

            var friendProfile = system.GetProfile(SurvivorBondType.Friend);
            Assert.NotNull(friendProfile);
            Assert.Equal(0.60f, friendProfile.DecayRatePerDay);
            Assert.Equal(4, friendProfile.NeglectThresholdDays);
            Assert.Equal(1.25f, friendProfile.ReconnectionBonusMultiplier);
            Assert.Equal(0.60f, friendProfile.SharedDutyMitigation);

            var familyProfile = system.GetProfile(SurvivorBondType.Family);
            Assert.NotNull(familyProfile);
            Assert.Equal(0.15f, familyProfile.DecayRatePerDay);
            Assert.Equal(7, familyProfile.NeglectThresholdDays);
            Assert.Equal(2.0f, familyProfile.ReconnectionBonusMultiplier);
            Assert.Equal(0.85f, familyProfile.SharedDutyMitigation);
        }

        [Fact]
        public void BondingMitigationProvider_MitigatesDecay_ForSurvivorsSharingDutyOrQuarters()
        {
            var system = new RelationshipDecaySystem();
            string jsonPath = GetDataPath("relationship_decay_profiles.json");
            system.LoadCatalog(File.ReadAllText(jsonPath));

            // Two pairs with Friend bond starting at 50 affinity
            var unmitigatedPair = system.RegisterOrUpdatePair("surv_unmit_a", "surv_unmit_b", SurvivorBondType.Friend, 50f, 40f, 1);
            var mitigatedPair = system.RegisterOrUpdatePair("surv_mit_a", "surv_mit_b", SurvivorBondType.Friend, 50f, 40f, 1);

            // Register mitigation provider for the second pair (60% mitigation)
            system.BondingMitigationProvider = (a, b) =>
            {
                if ((a == "surv_mit_a" && b == "surv_mit_b") || (a == "surv_mit_b" && b == "surv_mit_a"))
                {
                    return 0.60f;
                }
                return 0f;
            };

            // Advance days past neglect threshold (4 days for Friend)
            for (int day = 1; day <= 10; day++)
            {
                system.TickDay(day);
            }

            // Both pairs experienced neglect, but mitigated pair decayed significantly less
            float unmitigatedLoss = 50f - unmitigatedPair.Affinity;
            float mitigatedLoss = 50f - mitigatedPair.Affinity;

            Assert.True(unmitigatedLoss > 0f);
            Assert.True(mitigatedLoss > 0f);
            Assert.True(mitigatedLoss < unmitigatedLoss);
            // With 60% mitigation, loss should be approx 40% of unmitigated
            Assert.Equal(unmitigatedLoss * 0.40f, mitigatedLoss, precision: 2);
        }

        [Fact]
        public void BondDriftBridge_NotifiesExternalSubsystems_WhenDriftOccurs()
        {
            var system = new RelationshipDecaySystem();
            string jsonPath = GetDataPath("relationship_decay_profiles.json");
            system.LoadCatalog(File.ReadAllText(jsonPath));

            var pair = system.RegisterOrUpdatePair("surv_c", "surv_d", SurvivorBondType.CloseFriend, 51f, 40f, 1);

            PairBondState? driftedPair = null;
            SocialDriftType? lastDriftType = null;
            system.BondDriftBridge = (p, drift) =>
            {
                driftedPair = p;
                lastDriftType = drift;
            };

            // Advance until CloseFriend drops below 50f
            for (int day = 1; day <= 10; day++)
            {
                system.TickDay(day);
            }

            Assert.NotNull(driftedPair);
            Assert.Equal(SocialDriftType.FriendshipFaded, lastDriftType);
            Assert.Equal(SurvivorBondType.Friend, pair.Bond);
        }

        [Fact]
        public void ReconnectionBonusMultiplier_AppliesWhenReconnectingAfterNeglect()
        {
            var system = new RelationshipDecaySystem();
            string jsonPath = GetDataPath("relationship_decay_profiles.json");
            system.LoadCatalog(File.ReadAllText(jsonPath));

            var pair = system.RegisterOrUpdatePair("surv_e", "surv_f", SurvivorBondType.Friend, 30f, 30f, 1);

            // Tick 6 days past neglect threshold (Friend threshold is 4)
            for (int day = 2; day <= 7; day++)
            {
                system.TickDay(day);
            }

            float preAffinity = pair.Affinity;
            Assert.True(pair.DaysWithoutInteraction > 4);

            // Friend reconnection multiplier is 1.25x; bonus 20f * 1.25 = 25f
            system.RecordInteraction("surv_e", "surv_f", "shared_project", 20f, currentDay: 8);

            Assert.Equal(preAffinity + 25f, pair.Affinity);
            Assert.Equal(0, pair.DaysWithoutInteraction);
        }
    }
}
