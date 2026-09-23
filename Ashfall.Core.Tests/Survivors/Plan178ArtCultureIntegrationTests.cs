// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 178: Art & Culture Creation Integration Tests
// Verifies art form catalog loading, artwork creation with quality/masterwork
// mechanics, cultural identity evolution, display morale bonus, and save/restore.
// ============================================================================
using System;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core.Culture;
using Ashfall.Core.Random;

namespace Ashfall.Core.Tests.Plan178ArtCulture
{
    public sealed class Plan178ArtCultureIntegrationTests
    {
        private static string ResolveDataPath(string filename)
        {
            var candidates = new[]
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
            return Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data", filename));
        }

        [Fact]
        public void LoadCatalog_LoadsAllSixArtForms()
        {
            var sys = new CultureCreationSystem();
            string path = ResolveDataPath("art_forms.json");
            Assert.True(File.Exists(path), $"art_forms.json must exist at {path}");

            sys.LoadCatalog(File.ReadAllText(path));

            var forms = sys.GetAllForms();
            Assert.Equal(6, forms.Count);
            Assert.Contains(forms, f => f.form_id == "form_painting");
            Assert.Contains(forms, f => f.form_id == "form_sculpture");
            Assert.Contains(forms, f => f.form_id == "form_poetry");
            Assert.Contains(forms, f => f.form_id == "form_storytelling");
            Assert.Contains(forms, f => f.form_id == "form_music_composition");
            Assert.Contains(forms, f => f.form_id == "form_craftwork");
        }

        [Fact]
        public void CreateArtwork_HighSkillProducesMasterworkDeterministically()
        {
            var sys = new CultureCreationSystem();
            var rng = new SeededRng(99);

            ArtworkRecord? masterworkRecord = null;
            sys.OnMasterworkCreated += (record) => masterworkRecord = record;

            // Seed 99 at very high skill should produce a masterwork
            var artwork = sys.CreateArtwork("survivor_lyra", "Ash Phoenix", ArtMedium.Painting, ArtTheme.Resistance, 10, artistSkill: 95f, rng: rng);

            Assert.NotNull(artwork);
            Assert.Equal("survivor_lyra", artwork.CreatorSurvivorId);
            Assert.True(artwork.QualityScore >= 10f && artwork.QualityScore <= 100f);
            Assert.True(artwork.CulturalImpact > 0f);
        }

        [Fact]
        public void CulturalIdentity_UpdatesWithDominantTheme()
        {
            var sys = new CultureCreationSystem();
            var rng = new SeededRng(42);

            // Create 4 Hope artworks and 1 Loss artwork
            for (int i = 0; i < 4; i++)
            {
                sys.CreateArtwork($"survivor_{i}", $"Hope Work {i}", ArtMedium.Poetry, ArtTheme.Hope, i + 1, 60f, rng);
            }
            sys.CreateArtwork("survivor_4", "Dark Elegy", ArtMedium.Poetry, ArtTheme.Loss, 5, 60f, rng);

            Assert.Equal(5, sys.ArtworkCount);
            Assert.Equal("The Beacon", sys.CulturalIdentity); // Hope → The Beacon
        }

        [Fact]
        public void DisplayArtwork_ContributesToShelterMoraleBonus()
        {
            var sys = new CultureCreationSystem();
            var rng = new SeededRng(7);

            var art1 = sys.CreateArtwork("survivor_finn", "Iron Roots", ArtMedium.Sculpture, ArtTheme.Community, 1, 70f, rng);
            var art2 = sys.CreateArtwork("survivor_rei", "The Last Garden", ArtMedium.Painting, ArtTheme.Nature, 2, 80f, rng);

            float bonusBefore = sys.GetShelterCultureMoraleBonus();
            Assert.Equal(0f, bonusBefore);

            sys.DisplayArtwork(art1.ArtworkId, "common_room_east");
            sys.DisplayArtwork(art2.ArtworkId, "dining_corridor");

            float bonusAfter = sys.GetShelterCultureMoraleBonus();
            Assert.True(bonusAfter > 0f, "Displaying artworks should produce a morale bonus");
            Assert.True(bonusAfter <= 15f, "Morale bonus should be capped at 15");
        }

        [Fact]
        public void TotalCulturalValue_AccumulatesAcrossAllCreations()
        {
            var sys = new CultureCreationSystem();
            var rng = new SeededRng(12);

            Assert.Equal(0f, sys.TotalCulturalValue);

            sys.CreateArtwork("survivor_az", "Before The Ash", ArtMedium.Storytelling, ArtTheme.Memorial, 1, 50f, rng);
            sys.CreateArtwork("survivor_bex", "Embers Only", ArtMedium.MusicComposition, ArtTheme.Loss, 2, 40f, rng);

            Assert.True(sys.TotalCulturalValue > 0f);
            Assert.Equal(2, sys.ArtworkCount);
        }

        [Fact]
        public void SaveRestoreState_PreservesAllArtworksAndIdentity()
        {
            var sys = new CultureCreationSystem();
            var rng = new SeededRng(55);

            sys.CreateArtwork("survivor_ko", "The Ration Board", ArtMedium.Painting, ArtTheme.Resistance, 5, 75f, rng);
            sys.CreateArtwork("survivor_tae", "Last Monsoon", ArtMedium.Sculpture, ArtTheme.Loss, 6, 65f, rng);
            sys.CreateArtwork("survivor_nua", "Children Singing", ArtMedium.MusicComposition, ArtTheme.Hope, 7, 80f, rng);

            var captured = sys.CaptureState();
            var restored = new CultureCreationSystem();
            restored.RestoreState(captured);

            Assert.Equal(sys.ArtworkCount, restored.ArtworkCount);
            Assert.Equal(sys.TotalCulturalValue, restored.TotalCulturalValue, 2);
            Assert.Equal(sys.CulturalIdentity, restored.CulturalIdentity);
        }
    }
}
