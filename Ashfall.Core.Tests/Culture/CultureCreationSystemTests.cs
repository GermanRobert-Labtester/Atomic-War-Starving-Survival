// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Culture;
using Xunit;

namespace Ashfall.Core.Tests.Culture
{
    public sealed class CultureCreationSystemTests
    {
        [Fact]
        public void CreateArtwork_ComputesQualityScore_AndFiresEvent()
        {
            var system = new CultureCreationSystem();
            ArtworkRecord? created = null;
            system.OnArtworkCreated += a => created = a;

            var art = system.CreateArtwork("artist_elena", "Dawn Over Crater", ArtMedium.Painting, ArtTheme.Hope, day: 5, artistSkill: 60f);

            Assert.NotNull(art);
            Assert.Equal(created, art);
            Assert.Equal("artist_elena", art.CreatorSurvivorId);
            Assert.Equal("Dawn Over Crater", art.Title);
            Assert.Equal(ArtMedium.Painting, art.Medium);
            Assert.Equal(ArtTheme.Hope, art.Theme);
            Assert.Equal(60f, art.QualityScore); // 30 + 60*0.5
            Assert.True(art.CulturalImpact > 0f);
            Assert.Equal(1, system.ArtworkCount);
        }

        [Fact]
        public void CreateArtwork_Masterwork_TriggersMasterworkEventAndHigherImpact()
        {
            var system = new CultureCreationSystem();
            ArtworkRecord? masterwork = null;
            system.OnMasterworkCreated += m => masterwork = m;

            // Skill 120 -> base quality 30 + 60 = 90 >= 85 (masterwork!)
            var art = system.CreateArtwork("master_sculptor", "Memorial to the Lost", ArtMedium.Sculpture, ArtTheme.Memorial, day: 10, artistSkill: 120f);

            Assert.True(art.IsMasterwork);
            Assert.Equal(masterwork, art);
            Assert.Equal(art.QualityScore * 0.2f, art.CulturalImpact);
        }

        [Fact]
        public void DisplayArtwork_ProvidesShelterMoraleBonus()
        {
            var system = new CultureCreationSystem();
            var art = system.CreateArtwork("dweller_poet", "Song of Iron", ArtMedium.Poetry, ArtTheme.Resistance, day: 1, artistSkill: 50f);

            Assert.Equal(0f, system.GetShelterCultureMoraleBonus());

            system.DisplayArtwork(art.ArtworkId, "common_bunk");

            float bonus = system.GetShelterCultureMoraleBonus();
            Assert.True(bonus > 0f);
        }

        [Fact]
        public void CreateArtwork_UpdatesCulturalIdentity_BasedOnPredominantTheme()
        {
            var system = new CultureCreationSystem();

            system.CreateArtwork("artist_1", "Peaceful Sunrise", ArtMedium.Painting, ArtTheme.Hope, day: 1);
            system.CreateArtwork("artist_2", "New Crops", ArtMedium.Poetry, ArtTheme.Hope, day: 2);
            system.CreateArtwork("artist_3", "A New Generation", ArtMedium.Storytelling, ArtTheme.Hope, day: 3);

            Assert.Equal("The Beacon", system.CulturalIdentity);
        }

        [Fact]
        public void CaptureState_And_RestoreState_RoundTripsAccurately()
        {
            var system1 = new CultureCreationSystem();
            var art = system1.CreateArtwork("dweller_crafter", "Woven Tapestry", ArtMedium.Craftwork, ArtTheme.Nature, day: 4);
            system1.DisplayArtwork(art.ArtworkId, "mess_hall");

            var state = system1.CaptureState();
            Assert.Single(state.Artworks);

            var system2 = new CultureCreationSystem();
            system2.RestoreState(state);

            Assert.Equal(1, system2.ArtworkCount);
            Assert.Equal(system1.TotalCulturalValue, system2.TotalCulturalValue);
        }
    }
}
