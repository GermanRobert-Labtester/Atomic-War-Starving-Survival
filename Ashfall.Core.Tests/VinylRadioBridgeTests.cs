using System.Collections.Generic;
using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class VinylRadioBridgeTests
    {
        private static VinylMoraleSystem CreateSystem()
        {
            var sys = new VinylMoraleSystem();
            sys.LoadCatalog(new List<VinylRecordDefinition>
            {
                new VinylRecordDefinition { record_id = "vinyl_classical_rare", display_name = "Beethoven: Symphony No.9", genre = "classical", morale_daily_bonus = 5f },
                new VinylRecordDefinition { record_id = "vinyl_folk_common", display_name = "Dust Bowl Lullaby", genre = "folk", morale_daily_bonus = 2f },
                new VinylRecordDefinition { record_id = "vinyl_jazz_rare", display_name = "Midnight in the Bunker", genre = "jazz", morale_daily_bonus = 3f, flashback_suppression = 0.2f }
            });
            sys.AcquireRecord("vinyl_classical_rare");
            sys.AcquireRecord("vinyl_folk_common");
            sys.AcquireRecord("vinyl_jazz_rare");
            return sys;
        }

        [Fact]
        public void PlayRareVinyl_TriggersCulturalBroadcast()
        {
            var sys = CreateSystem();
            int broadcastFired = 0;
            VinylRecordDefinition broadcastRecord = null!;
            int broadcastDay = -1;
            sys.OnCulturalBroadcast += (rec, day) => { broadcastFired++; broadcastRecord = rec; broadcastDay = day; };

            var res = sys.Play("vinyl_classical_rare", 10);
            Assert.True(res.IsSuccess);
            Assert.Equal(1, broadcastFired);
            Assert.Equal("vinyl_classical_rare", broadcastRecord.record_id);
            Assert.Equal(10, broadcastDay);
            Assert.Equal(1, sys.State.broadcastCount);
            Assert.Equal("vinyl_classical_rare", sys.State.lastBroadcastRecordId);
            Assert.Equal(10, sys.State.lastBroadcastDay);
            Assert.True(sys.State.lastBroadcastSignalStrength > 0.5f);
        }

        [Fact]
        public void PlayCommonVinyl_DoesNotTriggerBroadcast()
        {
            var sys = CreateSystem();
            int broadcastFired = 0;
            sys.OnCulturalBroadcast += (_, __) => broadcastFired++;

            var res = sys.Play("vinyl_folk_common", 10);
            Assert.True(res.IsSuccess);
            Assert.Equal(0, broadcastFired);
            Assert.Equal(0, sys.State.broadcastCount);
            Assert.Equal(string.Empty, sys.State.lastBroadcastRecordId);
        }

        [Fact]
        public void PlayJazzRare_TriggersBroadcastViaGenre()
        {
            var sys = CreateSystem();
            int broadcastFired = 0;
            sys.OnCulturalBroadcast += (_, __) => broadcastFired++;
            // jazz with bonus 3 but genre jazz should still be rare
            var res = sys.Play("vinyl_jazz_rare", 5);
            Assert.True(res.IsSuccess);
            Assert.Equal(1, broadcastFired);
        }

        [Fact]
        public void Stop_ClearsBroadcastSignal()
        {
            var sys = CreateSystem();
            sys.Play("vinyl_classical_rare", 10);
            Assert.True(sys.State.lastBroadcastSignalStrength > 0.5f);
            sys.Stop();
            Assert.Equal(0f, sys.State.lastBroadcastSignalStrength);
            Assert.False(sys.IsPlaying);
        }

        [Fact]
        public void SaveRoundTrip_PreservesBroadcastState()
        {
            var sys1 = CreateSystem();
            sys1.Play("vinyl_classical_rare", 12);
            sys1.ApplyDailyEffect(12);
            var state = sys1.CaptureState();
            var sys2 = CreateSystem();
            sys2.RestoreState(state);
            Assert.Equal(1, sys2.State.broadcastCount);
            Assert.Equal("vinyl_classical_rare", sys2.State.lastBroadcastRecordId);
            Assert.Equal(12, sys2.State.lastBroadcastDay);
            Assert.True(sys2.State.lastBroadcastSignalStrength > 0.5f);
        }

        [Fact]
        public void IsRareCulturalRecord_ChecksBonusAndGenre()
        {
            var sys = CreateSystem();
            var rare = sys.GetRecord("vinyl_classical_rare");
            var common = sys.GetRecord("vinyl_folk_common");
            var jazz = sys.GetRecord("vinyl_jazz_rare");
            Assert.True(sys.IsRareCulturalRecord(rare!));
            Assert.False(sys.IsRareCulturalRecord(common!));
            Assert.True(sys.IsRareCulturalRecord(jazz!));
            Assert.False(sys.IsRareCulturalRecord(null!));
        }

        [Fact]
        public void SecondRarePlay_IncrementsBroadcastCount()
        {
            var sys = CreateSystem();
            sys.Play("vinyl_classical_rare", 10);
            sys.Stop();
            sys.Play("vinyl_jazz_rare", 11);
            Assert.Equal(2, sys.State.broadcastCount);
            Assert.Equal("vinyl_jazz_rare", sys.State.lastBroadcastRecordId);
            Assert.Equal(11, sys.State.lastBroadcastDay);
        }
    }
}
