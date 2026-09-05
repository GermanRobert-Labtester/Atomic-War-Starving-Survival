using System.Globalization;
using System.Threading;
using Ashfall.Core.Flags;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class FlagLedgerDeterminismTests
    {
        [Theory]
        [InlineData("FLAG_BUNKER_SEALED", "flag_bunker_sealed")]
        [InlineData("  Flag_Water_Purified  ", "flag_water_purified")]
        [InlineData("EVENT_STORM_PASSED_2026", "event_storm_passed_2026")]
        public void InMemoryFlagLedger_CaseInsensitive_AndTrimmed(string setKey, string queryKey)
        {
            var ledger = new InMemoryFlagLedger();
            Assert.False(ledger.IsSet(queryKey));

            ledger.Set(setKey);
            Assert.True(ledger.IsSet(queryKey));
            Assert.True(ledger.IsSet(setKey));

            ledger.Clear(queryKey);
            Assert.False(ledger.IsSet(queryKey));
            Assert.False(ledger.IsSet(setKey));
        }

        [Fact]
        public void InMemoryFlagLedger_Counters_NormalizeKeys()
        {
            var ledger = new InMemoryFlagLedger();
            ledger.Increment("COUNTER_RADIATION_DOSES", 5);
            ledger.Increment("counter_radiation_doses", 3);

            Assert.Equal(8, ledger.GetCounter("counter_radiation_doses"));
            Assert.Equal(8, ledger.GetCounter("COUNTER_RADIATION_DOSES"));

            ledger.SetCounter("COUNTER_RADIATION_DOSES", 2);
            Assert.Equal(2, ledger.GetCounter("counter_radiation_doses"));
        }

        [Theory]
        [InlineData("FLAG_MUSTER_ASSEMBLED", "flag_muster_assembled")]
        [InlineData("  Consequence_Truce_Signed  ", "consequence_truce_signed")]
        public void CampaignConsequenceLedger_CaseInsensitive_AndIdempotent(string setKey, string queryKey)
        {
            var ledger = new CampaignConsequenceLedger();
            int firedCount = 0;
            ledger.OnConsequenceRecorded += _ => firedCount++;

            ledger.Set(setKey, originSystem: "diplomacy", day: 4);
            Assert.True(ledger.IsSet(queryKey));
            Assert.Equal(1, firedCount);

            // Repeat set must be idempotent: does not record duplicate consequence event
            ledger.Set(queryKey, originSystem: "diplomacy", day: 5);
            Assert.True(ledger.IsSet(queryKey));
            Assert.Equal(1, firedCount);

            ledger.Clear(queryKey);
            Assert.False(ledger.IsSet(queryKey));
        }

        [Fact]
        public void FlagNormalization_CultureInvariant_UnderTurkishLocale()
        {
            var prevCulture = Thread.CurrentThread.CurrentCulture;
            try
            {
                // Turkish has dotted 'i' and dotless 'I' which breaks standard culture-sensitive ToLower
                Thread.CurrentThread.CurrentCulture = new CultureInfo("tr-TR");

                var memoryLedger = new InMemoryFlagLedger();
                memoryLedger.Set("FLAG_RADIO_INTERCEPT");
                Assert.True(memoryLedger.IsSet("flag_radio_intercept"));

                var consequenceLedger = new CampaignConsequenceLedger();
                consequenceLedger.Set("FLAG_RADIO_INTERCEPT");
                Assert.True(consequenceLedger.IsSet("flag_radio_intercept"));
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = prevCulture;
            }
        }
    }
}
