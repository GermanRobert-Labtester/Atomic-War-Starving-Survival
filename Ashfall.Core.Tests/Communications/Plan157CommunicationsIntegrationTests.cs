// SPDX-License-Identifier: MIT
// ============================================================================
// Ashfall Core Tests : Plan 157 — Communications & Radio Network Infrastructure
// Subsystem          : CommunicationsSystem / Antennas & Interception Tests
// Authority          : Next-steps-plans/Plan_157_Communications_Radio_Network_Infrastructure.md
//                      UNBLOCK-PROGRAM-WAVE32-BATCH6-PLANS (DEC-153)
// ============================================================================
using System;
using System.IO;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Communications;

namespace Ashfall.Core.Tests.Plan157Comms
{
    public sealed class Plan157CommunicationsIntegrationTests
    {
        private static string ResolveDataPath(string filename)
        {
            string[] candidates = new[]
            {
                Path.Combine(AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data", filename),
                Path.Combine(AppContext.BaseDirectory, "../../../Assets/StreamingAssets/Data", filename),
                Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data", filename),
                Path.Combine(Directory.GetCurrentDirectory(), "../Assets/StreamingAssets/Data", filename)
            };
            foreach (var c in candidates)
            {
                if (File.Exists(c)) return Path.GetFullPath(c);
            }
            return Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data", filename);
        }

        [Fact]
        public void Catalog_Loads_DefaultNetworksSuccessfully()
        {
            var system = new CommunicationsSystem();
            string path = ResolveDataPath("communications_networks.json");
            Assert.True(File.Exists(path), $"communications_networks.json missing at {path}");

            string json = File.ReadAllText(path);
            system.LoadCatalog(json);

            var net = system.GetNetwork("net_garrison_tactical");
            Assert.NotNull(net);
            Assert.Equal(462.55, net.FrequencyMhz);
            Assert.Equal("faction_garrison", net.FactionId);
            Assert.False(net.IsPlayerOwned);
        }

        [Fact]
        public void AntennaInstallation_ExpandsEffectiveReceptionRange()
        {
            var system = new CommunicationsSystem();
            // Starts with basic whip (8.0 km)
            Assert.Equal(8.0, system.GetEffectiveReceptionRangeKm());

            // Install directional Yagi array (25.0 km)
            system.InstallAntenna("ant_yagi_01", AntennaType.DirectionalYagi, "North Ridge Yagi");
            Assert.Equal(25.0, system.GetEffectiveReceptionRangeKm());

            // Install parabolic dish (60.0 km)
            system.InstallAntenna("ant_dish_01", AntennaType.ParabolicDish, "Bunker Parabolic Dish");
            Assert.Equal(60.0, system.GetEffectiveReceptionRangeKm());
        }

        [Fact]
        public void SignalInterception_RequiresAdequateAntennaInfrastructure()
        {
            var system = new CommunicationsSystem();
            var rng = new SeededRng(12345);

            // Baseline whip antenna (8 km) is insufficient to intercept long-range faction comms (requires > 10 km)
            var failIntercept = system.InterceptFactionSignal("faction_garrison", rng, currentDay: 2);
            Assert.Null(failIntercept);

            // Install Yagi array (25 km range, 65 sensitivity)
            system.InstallAntenna("ant_yagi_main", AntennaType.DirectionalYagi);

            var successIntercept = system.InterceptFactionSignal("faction_garrison", rng, currentDay: 3);
            Assert.NotNull(successIntercept);
            Assert.Equal("faction_garrison", successIntercept.SourceFactionId);
            Assert.False(successIntercept.IsDecoded);
            Assert.True(successIntercept.IntelligenceValue > 0);
        }

        [Fact]
        public void Cryptanalysis_DecodesEncryptedFactionMessage()
        {
            var system = new CommunicationsSystem();
            var rng = new SeededRng(999);
            system.InstallAntenna("ant_dish", AntennaType.ParabolicDish);

            var msg = system.InterceptFactionSignal("faction_garrison", rng, currentDay: 4);
            Assert.NotNull(msg);
            Assert.False(msg.IsDecoded);

            // Decode attempt with high cryptanalysis skill
            bool decoded = system.DecodeMessage(msg.MessageId, cryptanalysisSkill: 75, rng);
            Assert.True(decoded);

            var decodedMsg = system.GetMessage(msg.MessageId)!;
            Assert.True(decodedMsg.IsDecoded);
            Assert.False(string.IsNullOrEmpty(decodedMsg.DecodedContent));
            Assert.Equal(1, system.TotalMessagesDecoded);
        }

        [Fact]
        public void OutgoingBroadcast_ReachesAcrossAntennaRange()
        {
            var system = new CommunicationsSystem();
            system.InstallAntenna("ant_array", AntennaType.PhasedArray);
            double maxRange = system.GetEffectiveReceptionRangeKm();
            Assert.Equal(120.0, maxRange);

            var bcast = system.TransmitBroadcast(144.2, "ALERT: Safe haven trading open at Sector 4.", encryptionLevel: 0, currentDay: 10);
            Assert.NotNull(bcast);
            Assert.Equal(120.0, bcast.AudienceReachKm);
            Assert.Equal(1, system.TotalBroadcastsSent);
        }

        [Fact]
        public void FrequencyJammingAndSaveRestore_PreservesState()
        {
            var system = new CommunicationsSystem();
            system.InstallAntenna("ant_yagi", AntennaType.DirectionalYagi);
            system.JamFrequency(462.55, durationDays: 3);

            var net = system.GetNetwork("net_garrison_tactical")!;
            Assert.Equal(NetworkStatus.Jammed, net.Status);

            var state = system.CaptureState();
            Assert.Equal(1, state.SchemaVersion);

            var restoredSystem = new CommunicationsSystem();
            restoredSystem.RestoreState(state);

            var restoredNet = restoredSystem.GetNetwork("net_garrison_tactical")!;
            Assert.Equal(NetworkStatus.Jammed, restoredNet.Status);
            Assert.Equal(system.GetEffectiveReceptionRangeKm(), restoredSystem.GetEffectiveReceptionRangeKm());
        }
    }
}
