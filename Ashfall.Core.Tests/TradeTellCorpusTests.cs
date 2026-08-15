using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Negotiation Table tell corpus: coverage counts, tone lint, band math,
    /// and seed determinism. Mirrors the FactionRadioCorpusTests pattern.
    /// </summary>
    public class TradeTellCorpusTests
    {
        private static TradeTellEngine CreateLoadedEngine()
        {
            string corpusPath = Path.Combine(AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data/trade_tell_lines.json");
            if (!File.Exists(corpusPath))
            {
                corpusPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data/trade_tell_lines.json");
            }

            Assert.True(File.Exists(corpusPath), $"Tell corpus JSON not found at {corpusPath}");
            return TradeTellEngine.LoadFromJson(File.ReadAllText(corpusPath));
        }

        private static string StanceKey(TradeStance stance)
        {
            switch (stance)
            {
                case TradeStance.HostileRaid: return "hostile_raid";
                case TradeStance.Rob: return "rob";
                case TradeStance.Refuse: return "refuse";
                case TradeStance.ShareIntel: return "share_intel";
                default: return "trade";
            }
        }

        [Fact]
        public void Corpus_LoadsFourBandsAndTwentyPools()
        {
            var engine = CreateLoadedEngine();

            Assert.Equal(4, engine.BandCount);
            Assert.Equal(TradeTrustBands.Hostile, engine.Bands[0]);
            Assert.Equal(TradeTrustBands.Wary, engine.Bands[1]);
            Assert.Equal(TradeTrustBands.Neutral, engine.Bands[2]);
            Assert.Equal(TradeTrustBands.Warm, engine.Bands[3]);

            // 5 stances x 4 bands = 20 pools, >= 3 lines each (>= 60 total).
            Assert.Equal(20, engine.PoolCount);
            Assert.True(engine.LineCount >= 60, $"Expected >= 60 tell lines, found {engine.LineCount}");
        }

        [Fact]
        public void Corpus_EveryStanceAndBandSelectsALegibleLine()
        {
            var engine = CreateLoadedEngine();
            var rng = new SeededRng(2026);

            foreach (TradeStance stance in Enum.GetValues(typeof(TradeStance)))
            {
                foreach (float trust in new[] { -100f, -40f, -39f, 0f, 1f, 40f, 41f, 100f })
                {
                    bool selected = engine.TrySelectTell(stance, trust, rng, out var tell);
                    Assert.True(selected, $"No tell for stance={stance} trust={trust}");
                    Assert.False(string.IsNullOrWhiteSpace(tell.Line));
                    Assert.InRange(tell.Line.Length, 20, 140);
                    Assert.Contains(StanceKey(stance), tell.Id);
                    Assert.Contains(engine.BandForTrust(trust), tell.Id);
                }
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

            // Exhaustive: every line in every pool.
            foreach (TradeStance stance in Enum.GetValues(typeof(TradeStance)))
            {
                foreach (var band in engine.Bands)
                {
                    Assert.True(engine.TryGetPoolLines(stance, band, out var lines));
                    foreach (var line in lines)
                    {
                        string lower = " " + line.ToLowerInvariant() + " ";
                        foreach (var forbidden in forbiddenWords)
                        {
                            Assert.DoesNotContain(forbidden, lower);
                        }
                    }
                }
            }
        }

        [Fact]
        public void Corpus_NoDuplicateLinesWithinPool()
        {
            var engine = CreateLoadedEngine();

            foreach (TradeStance stance in Enum.GetValues(typeof(TradeStance)))
            {
                foreach (var band in engine.Bands)
                {
                    Assert.True(engine.TryGetPoolLines(stance, band, out var lines),
                        $"Missing pool for stance={stance} band={band}");
                    Assert.True(lines.Count >= 3, $"Pool stance={stance} band={band} has {lines.Count} lines, expected >= 3");

                    var seen = new HashSet<string>(StringComparer.Ordinal);
                    foreach (var line in lines)
                    {
                        Assert.DoesNotContain(line, seen);
                        seen.Add(line);
                    }
                }
            }
        }

        [Fact]
        public void Engine_BandBoundaries_MapTrustCorrectly()
        {
            var engine = CreateLoadedEngine();

            Assert.Equal(TradeTrustBands.Hostile, engine.BandForTrust(-100f));
            Assert.Equal(TradeTrustBands.Hostile, engine.BandForTrust(-40f));
            Assert.Equal(TradeTrustBands.Wary, engine.BandForTrust(-39f));
            Assert.Equal(TradeTrustBands.Wary, engine.BandForTrust(0f));
            Assert.Equal(TradeTrustBands.Neutral, engine.BandForTrust(1f));
            Assert.Equal(TradeTrustBands.Neutral, engine.BandForTrust(40f));
            Assert.Equal(TradeTrustBands.Warm, engine.BandForTrust(41f));
            Assert.Equal(TradeTrustBands.Warm, engine.BandForTrust(100f));
        }

        [Fact]
        public void Engine_DeterministicRotation_SameSeedSameLine()
        {
            var engine1 = CreateLoadedEngine();
            var engine2 = CreateLoadedEngine();

            var rng1 = new SeededRng(9999);
            var rng2 = new SeededRng(9999);

            for (int i = 0; i < 20; i++)
            {
                foreach (TradeStance stance in new[] { TradeStance.Trade, TradeStance.Refuse })
                {
                    Assert.True(engine1.TrySelectTell(stance, 22f, rng1, out var t1));
                    Assert.True(engine2.TrySelectTell(stance, 22f, rng2, out var t2));
                    Assert.Equal(t1.Id, t2.Id);
                    Assert.Equal(t1.Line, t2.Line);
                }
            }
        }

        [Fact]
        public void Engine_RotationVariesAcrossSeeds()
        {
            var engine = CreateLoadedEngine();

            var lines = new HashSet<string>(StringComparer.Ordinal);
            for (int seed = 1; seed <= 6; seed++)
            {
                Assert.True(engine.TrySelectTell(TradeStance.Trade, 22f, new SeededRng(seed), out var tell));
                lines.Add(tell.Line);
            }

            Assert.True(lines.Count > 1, "Different seeds should rotate through the pool, not pin one line.");
        }
    }
}
