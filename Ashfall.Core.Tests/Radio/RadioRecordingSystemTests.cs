// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core.Radio;
using Xunit;

namespace Ashfall.Core.Tests.Radio
{
    public class RadioRecordingSystemTests
    {
        [Fact]
        public void RecordBroadcast_CreatesCassetteEntry_AndReplayIsNonMutating()
        {
            var recordingSys = new RadioRecordingSystem();
            var broadcast = new ScheduledBroadcastResult
            {
                HasTransmission = true,
                FrequencyMhz = 88.40f,
                BroadcastId = "wiretap_garrison_officer_mutiny",
                Headline = "Wiretap: Garrison Officer Mutiny",
                Message = "Sentry captains debating refusing winter grain requisition orders.",
                SourceName = "Iron Garrison Tap",
                AudioCue = "radio_vo_ch7_milband"
            };

            var recorded = recordingSys.RecordBroadcast(broadcast, day: 33);
            Assert.NotNull(recorded);
            Assert.Equal("wiretap_garrison_officer_mutiny", recorded!.broadcastId);
            Assert.Equal(88.40f, recorded.frequencyMhz);
            Assert.Equal(33, recorded.recordedDay);

            // Replay
            var replayed = recordingSys.ReplayCassette(recorded.cassetteId);
            Assert.NotNull(replayed);
            Assert.Equal(recorded.transcript, replayed!.transcript);
            Assert.Equal(recorded.audioCue, replayed.audioCue);
        }

        [Fact]
        public void CalculateTradeValue_IntelligenceCarriesHigherValueThanRoutine()
        {
            var recordingSys = new RadioRecordingSystem();

            var intelBcast = new ScheduledBroadcastResult
            {
                HasTransmission = true,
                FrequencyMhz = 104.2f,
                Headline = "Intercepted Wiretap: Hydro-Baron Salt Well Embargo",
                Message = "Plan to contaminate competitive wellheads.",
                BroadcastId = "intel_01"
            };
            var intelTape = recordingSys.RecordBroadcast(intelBcast, day: 10);

            var routineBcast = new ScheduledBroadcastResult
            {
                HasTransmission = true,
                FrequencyMhz = 88.5f,
                Headline = "Standard Station Carrier",
                Message = "Routine test transmission.",
                BroadcastId = "routine_01"
            };
            var routineTape = recordingSys.RecordBroadcast(routineBcast, day: 10);

            Assert.Equal(25, recordingSys.CalculateTradeValue(intelTape!.cassetteId));
            Assert.Equal(1, recordingSys.CalculateTradeValue(routineTape!.cassetteId));
        }
    }
}
