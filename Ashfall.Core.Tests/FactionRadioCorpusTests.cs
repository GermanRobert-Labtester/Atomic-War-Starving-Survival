using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Radio;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class FactionRadioCorpusTests
    {
        private static FactionRadioEngine CreateLoadedEngine()
        {
            string corpusPath = Path.Combine(AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data/faction_radio_corpus.json");
            if (!File.Exists(corpusPath))
            {
                corpusPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data/faction_radio_corpus.json");
            }

            Assert.True(File.Exists(corpusPath), $"Corpus JSON not found at {corpusPath}");
            string json = File.ReadAllText(corpusPath);
            var engine = FactionRadioEngine.LoadFromJson(json);
            return engine;
        }

        [Fact]
        public void Corpus_GuildChannelResolvesNotFallback()
        {
            var engine = CreateLoadedEngine();
            var rng = new SeededRng(1009);

            var chatter = engine.GetFactionEvent("faction_silent_foundry", RadioEventKind.InterceptChatter, 200, rng);
            Assert.Equal("faction_silent_foundry", chatter.FactionId);
            Assert.Equal("FOUNDRY FLOOR / CUPOLA SHIFT", chatter.Callsign);
            Assert.NotEqual(RadioEventKind.Silence, chatter.Kind);
            Assert.True(chatter.SignalStrength >= 7);
            Assert.False(string.IsNullOrWhiteSpace(chatter.Message));

            var reaction = engine.GetFactionEvent("faction_silent_foundry", RadioEventKind.TradeReaction, 200, rng);
            Assert.Equal("faction_silent_foundry", reaction.FactionId);
        }

        [Fact]
        public void Corpus_LoadsAllThirteenFactions()
        {
            var engine = CreateLoadedEngine();
            var factions = engine.GetAllFactions();

            Assert.Equal(13, factions.Count);
            var expectedFactions = new[]
            {
                "military_remnants", "cult_of_the_glow", "scavenger_camp", "upland_militia",
                "hydro_barons", "rot_farmers", "wire_heads", "sump_dredgers",
                "custodians", "doomsday_preppers", "echo_bats", "safe_haven_community",
                "faction_silent_foundry"
            };

            foreach (var f in expectedFactions)
            {
                Assert.Contains(f, factions);
                Assert.True(engine.GetFactionFrequency(f) > 0f);
                Assert.False(string.IsNullOrWhiteSpace(engine.GetFactionCallsign(f)));
            }
        }

        [Fact]
        public void Corpus_HasAtLeastTwelveChatterLinesPerFaction_AndNoDuplicates()
        {
            var engine = CreateLoadedEngine();
            var allLines = new HashSet<string>(StringComparer.Ordinal);
            int totalLinesCounted = 0;

            // Silence lines
            Assert.True(engine.SilenceEventCount >= 12, "Expected >= 12 silence lines");

            var rng = new SeededRng(2026);
            foreach (var f in engine.GetAllFactions())
            {
                // Verify frequencies
                float freq = engine.GetFactionFrequency(f);
                Assert.InRange(freq, 50.0f, 150.0f);

                // Intercept chatter
                for (int i = 0; i < 12; i++)
                {
                    var msg = engine.GetFactionEvent(f, RadioEventKind.InterceptChatter, i + 1, rng);
                    Assert.False(string.IsNullOrWhiteSpace(msg.Message));
                    Assert.InRange(msg.Message.Length, 20, 240);
                }

                // Verify Parley
                var parley = engine.GetFactionEvent(f, RadioEventKind.ParleyResolution, 1, rng);
                Assert.False(string.IsNullOrWhiteSpace(parley.Message));

                // Verify Raid Warning
                var raid = engine.GetFactionEvent(f, RadioEventKind.RaidWarning, 1, rng);
                Assert.False(string.IsNullOrWhiteSpace(raid.Message));

                // Verify Trade Reaction
                var trade = engine.GetFactionEvent(f, RadioEventKind.TradeReaction, 1, rng);
                Assert.False(string.IsNullOrWhiteSpace(trade.Message));
            }
        }

        [Fact]
        public void Corpus_ToneLint_NoModernSlangOrAnachronisms()
        {
            var engine = CreateLoadedEngine();
            var forbiddenWords = new[]
            {
                " lol ", " gg ", " bruh ", " meta ", " player ", " respawn ", " nerf ", " buff ", " xp "
            };

            var rng = new SeededRng(100);
            foreach (var f in engine.GetAllFactions())
            {
                for (int d = 1; d <= 30; d++)
                {
                    foreach (RadioEventKind kind in Enum.GetValues(typeof(RadioEventKind)))
                    {
                        var intercept = engine.GetFactionEvent(f, kind, d, rng);
                        string lower = intercept.Message.ToLowerInvariant();
                        foreach (var forbidden in forbiddenWords)
                        {
                            Assert.DoesNotContain(forbidden, lower);
                        }
                    }
                }
            }
        }

        [Fact]
        public void FactionRadioEngine_TuningAndSignalStrength_CalculatesAccurately()
        {
            var engine = CreateLoadedEngine();
            var rng = new SeededRng(42);

            // Exact frequency tuning
            float exactFreq = engine.GetFactionFrequency("military_remnants");
            var exactHit = engine.GetBroadcastAtFrequency(exactFreq, 1, rng);
            Assert.Equal("military_remnants", exactHit.FactionId);
            Assert.True(exactHit.SignalStrength >= 7);

            // Slightly off-frequency
            var offsetHit = engine.GetBroadcastAtFrequency(exactFreq + 0.8f, 1, rng);
            Assert.Equal("military_remnants", offsetHit.FactionId);
            Assert.True(offsetHit.SignalStrength < 7);

            // Off-band dead air
            var deadAir = engine.GetBroadcastAtFrequency(10.0f, 1, rng);
            Assert.Equal(string.Empty, deadAir.FactionId);
            Assert.Equal(RadioEventKind.Silence, deadAir.Kind);
            Assert.Equal(1, deadAir.SignalStrength);
        }

        [Fact]
        public void FactionRadioEngine_DeterministicRotation_ProvableCrossProcess()
        {
            var engine1 = CreateLoadedEngine();
            var engine2 = CreateLoadedEngine();

            var rng1 = new SeededRng(9999);
            var rng2 = new SeededRng(9999);

            for (int i = 0; i < 20; i++)
            {
                var hit1 = engine1.GetBroadcastAtFrequency(88.4f, i, rng1);
                var hit2 = engine2.GetBroadcastAtFrequency(88.4f, i, rng2);

                Assert.Equal(hit1.Message, hit2.Message);
                Assert.Equal(hit1.SignalStrength, hit2.SignalStrength);
                Assert.Equal(hit1.FactionId, hit2.FactionId);
            }
        }
    }
}
