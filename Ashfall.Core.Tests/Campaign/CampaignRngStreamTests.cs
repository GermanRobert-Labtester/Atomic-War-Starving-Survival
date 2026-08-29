using System;
using System.Collections.Generic;
using Ashfall.Core.Campaign;
using Ashfall.Core.Random;
using Xunit;

namespace Ashfall.Core.Tests.Campaign
{
    public class CampaignRngStreamTests
    {
        [Fact]
        public void MasterSeed_ProducesDeterministicDomainStreams()
        {
            var mgr1 = new CampaignRngManager(masterSeed: 1986);
            var mgr2 = new CampaignRngManager(masterSeed: 1986);

            var s1 = mgr1.GetStream(CampaignStreamIds.Weather);
            var s2 = mgr2.GetStream(CampaignStreamIds.Weather);

            Assert.Equal(s1.DerivedBaseSeed, s2.DerivedBaseSeed);

            for (int i = 0; i < 20; i++)
            {
                Assert.Equal(s1.Rng.Next(0, 1000), s2.Rng.Next(0, 1000));
            }
        }

        [Fact]
        public void StreamIsolation_ConsumingOneStreamDoesNotAffectOtherStreams()
        {
            var mgr1 = new CampaignRngManager(masterSeed: 1986);
            var mgr2 = new CampaignRngManager(masterSeed: 1986);

            // In mgr1, consume 50 values from Weather stream
            var weather1 = mgr1.GetStream(CampaignStreamIds.Weather);
            for (int i = 0; i < 50; i++)
            {
                weather1.Rng.Next(0, 1000);
            }

            // Combat stream in both managers should produce identical results
            var combat1 = mgr1.GetStream(CampaignStreamIds.Combat);
            var combat2 = mgr2.GetStream(CampaignStreamIds.Combat);

            for (int i = 0; i < 20; i++)
            {
                Assert.Equal(combat1.Rng.Next(1, 100), combat2.Rng.Next(1, 100));
            }
        }

        [Fact]
        public void Fork_ProvidesStatelessActionOrderIndependence()
        {
            var mgr = new CampaignRngManager(masterSeed: 1986);

            var forkDay5Act1_A = mgr.Fork(CampaignStreamIds.Expedition, day: 5, actionIndex: 1);

            // Consume other streams and forks
            mgr.Fork(CampaignStreamIds.Weather, day: 10, actionIndex: 3).Next(0, 1000);
            mgr.GetStream(CampaignStreamIds.Disease).Rng.Next(0, 1000);

            var forkDay5Act1_B = mgr.Fork(CampaignStreamIds.Expedition, day: 5, actionIndex: 1);

            for (int i = 0; i < 10; i++)
            {
                Assert.Equal(forkDay5Act1_A.Next(0, 500), forkDay5Act1_B.Next(0, 500));
            }
        }

        [Fact]
        public void StreamPositions_SaveAndRestoreCleanly()
        {
            var mgr1 = new CampaignRngManager(masterSeed: 42);
            var weather = mgr1.GetStream(CampaignStreamIds.Weather);
            for (int i = 0; i < 15; i++)
            {
                weather.Rng.Next(0, 1000);
            }
            Assert.Equal(15, weather.Position);

            var positions = mgr1.CapturePositions();

            var mgr2 = new CampaignRngManager(masterSeed: 42);
            var weather2 = mgr2.GetStream(CampaignStreamIds.Weather);
            mgr2.RestorePositions(positions);

            Assert.Equal(15, weather2.Position);

            // Next 10 values from mgr1 and mgr2 should match
            for (int i = 0; i < 10; i++)
            {
                Assert.Equal(weather.Rng.Next(0, 1000), weather2.Rng.Next(0, 1000));
            }
        }

        [Fact]
        public void VersionZero_PreservesLegacySaveSeedMapping()
        {
            Assert.Equal(2026, CampaignRngStream.DeriveSeed(1986, CampaignStreamIds.MoralChoice, version: 0));
            Assert.Equal(2026, CampaignRngStream.DeriveSeed(1986, CampaignStreamIds.Radio, version: 0));
            Assert.Equal(2026, CampaignRngStream.DeriveSeed(1986, CampaignStreamIds.Economy, version: 0));
            Assert.Equal(1986 + 10 * 31 + 2, CampaignRngStream.DeriveSeed(1986, CampaignStreamIds.Shelter, version: 0, day: 10, actionIndex: 2));
        }

        [Fact]
        public void FormatDiagnostics_EmitsStandardizedStreamDiagnosticString()
        {
            var mgr = new CampaignRngManager(masterSeed: 1986);
            string diag = mgr.FormatDiagnostics(CampaignStreamIds.Weather, day: 4, actionIndex: 2);

            Assert.StartsWith("[RNG_STREAM] id='weather' master=1986", diag);
            Assert.Contains("day=4", diag);
            Assert.Contains("action=2", diag);
        }

        [Fact]
        public void Coordinator_PersistsAndRestoresRngState()
        {
            var coord1 = new CampaignDayCoordinator(rng: new CampaignRngManager(masterSeed: 777));
            var stream = coord1.Rng.GetStream(CampaignStreamIds.Disease);
            stream.Rng.Next(0, 1000);
            stream.Rng.Next(0, 1000);

            var save = coord1.CaptureState();
            Assert.Equal(777, save.masterSeed);
            Assert.Equal(1, save.derivationVersion);
            Assert.True(save.streamPositions.ContainsKey(CampaignStreamIds.Disease));
            Assert.Equal(2, save.streamPositions[CampaignStreamIds.Disease]);

            var coord2 = new CampaignDayCoordinator(rng: new CampaignRngManager(masterSeed: save.masterSeed));
            coord2.Rng.GetStream(CampaignStreamIds.Disease); // ensure initialized
            coord2.RestoreState(save);

            Assert.Equal(2, coord2.Rng.GetStream(CampaignStreamIds.Disease).Position);
        }
    }
}
