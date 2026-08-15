using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class FringeCultsCatalogTests
    {
        private readonly string _narrativeDir;

        public FringeCultsCatalogTests()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _narrativeDir = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "narrative");
            if (!Directory.Exists(_narrativeDir))
            {
                _narrativeDir = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data", "narrative");
            }
        }

        [Fact]
        public void FringeCultsCatalog_LoadsAll30EntriesAcross4Batches()
        {
            Assert.True(Directory.Exists(_narrativeDir), $"Directory not found: {_narrativeDir}");

            var catalog = FringeCultsCatalog.LoadFromDirectory(_narrativeDir);
            Assert.NotNull(catalog);
            Assert.Equal(8, catalog.CobaltLiturgies.Count);
            Assert.Equal(8, catalog.IronSynodCanons.Count);
            Assert.Equal(7, catalog.GeophoneHymnals.Count);
            Assert.Equal(7, catalog.WastelandEpitaphs.Count);
            Assert.Equal(30, catalog.TotalCount);
        }

        [Fact]
        public void FringeCultsCatalog_CobaltLiturgies_Integrity()
        {
            var catalog = FringeCultsCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.CobaltLiturgies)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("liturgy_cobalt_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.CultFaction));
                Assert.False(string.IsNullOrWhiteSpace(item.LiturgyType));
                Assert.True(item.SacredRadThresholdCpm > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetCobaltLiturgy("liturgy_cobalt_psalm_of_blue_glow");
            Assert.NotNull(entry);
            Assert.Equal("ORDER_OF_THE_COBALT_FLAME", entry.CultFaction);
        }

        [Fact]
        public void FringeCultsCatalog_IronSynodCanons_Integrity()
        {
            var catalog = FringeCultsCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.IronSynodCanons)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("canon_synod_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.SynodChapter));
                Assert.False(string.IsNullOrWhiteSpace(item.CanonNumber));
                Assert.True(item.SacredTemperatureCelsius > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetIronSynodCanon("canon_synod_first_law_of_temper");
            Assert.NotNull(entry);
            Assert.Equal("CANON_I", entry.CanonNumber);
        }

        [Fact]
        public void FringeCultsCatalog_GeophoneHymnals_Integrity()
        {
            var catalog = FringeCultsCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.GeophoneHymnals)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("hymnal_geophone_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.MonasteryCircle));
                Assert.False(string.IsNullOrWhiteSpace(item.HymnNumber));
                Assert.True(item.ResonantFrequencyHz > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetGeophoneHymnal("hymnal_geophone_dirge_of_the_p_wave");
            Assert.NotNull(entry);
            Assert.Equal("HYMN_04", entry.HymnNumber);
        }

        [Fact]
        public void FringeCultsCatalog_WastelandEpitaphs_Integrity()
        {
            var catalog = FringeCultsCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.WastelandEpitaphs)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("epitaph_scav_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.GraveSite));
                Assert.False(string.IsNullOrWhiteSpace(item.MarkerMaterial));
                Assert.False(string.IsNullOrWhiteSpace(item.DeceasedIdentity));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetWastelandEpitaph("epitaph_scav_spent_casing_cairn");
            Assert.NotNull(entry);
            Assert.Equal("SENTRY_MARTHA_KLINE", entry.DeceasedIdentity);
        }
    }
}
