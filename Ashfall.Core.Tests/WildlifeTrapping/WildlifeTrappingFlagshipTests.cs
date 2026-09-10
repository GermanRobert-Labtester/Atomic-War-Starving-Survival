using System;
using System.Collections.Generic;
using Ashfall.Core.Localization;
using Ashfall.Core.Onboarding;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.WildlifeTrapping
{
    public sealed class WildlifeTrappingFlagshipTests
    {
        [Fact]
        public void LocalizationKeysAreStableAndIdDerived()
        {
            Assert.Equal("wildlife.trap.trap_snare.name", WildlifeTrappingLocalization.TrapNameKey("trap_snare"));
            Assert.Equal("wildlife.prey.rad_dog.description", WildlifeTrappingLocalization.PreyDescriptionKey("rad_dog"));
            Assert.Equal("wildlife.bait.bait_grain_lure.name", WildlifeTrappingLocalization.BaitNameKey("bait_grain_lure"));
        }

        [Fact]
        public void ContextualTutorialsAreOneShotAndPersistedByOnboardingAuthority()
        {
            var journey = OnboardingJourney.CreateFirstHour();
            int requested = 0;
            journey.OnContextualTutorialRequested += _ => requested++;

            Assert.True(journey.RequestContextualTutorial(WildlifeTrappingLocalization.FirstSnareTutorialId));
            Assert.False(journey.RequestContextualTutorial(WildlifeTrappingLocalization.FirstSnareTutorialId));
            Assert.Equal(1, requested);
            Assert.Contains(WildlifeTrappingLocalization.FirstSnareTutorialId, journey.ContextualTutorialSeenIds);

            var restored = OnboardingJourney.Restore(journey.CaptureState());
            Assert.Contains(WildlifeTrappingLocalization.FirstSnareTutorialId, restored.ContextualTutorialSeenIds);
            Assert.False(restored.RequestContextualTutorial(WildlifeTrappingLocalization.FirstSnareTutorialId));
        }

        [Fact]
        public void TrapMarkersAreStableHiddenInformationSafeAndReconciled()
        {
            var map = new WastelandMapSystem(
                new WastelandMapState(),
                new[]
                {
                    new MapNode
                    {
                        Id = "loc_holdfast",
                        DisplayName = "Holdfast",
                        PositionX = 500f,
                        PositionY = 300f,
                        StartingUnlocked = true
                    }
                },
                Array.Empty<MapRoute>(),
                new[]
                {
                    new TrapSiteMapLocation
                    {
                        SiteId = "site_north",
                        AnchorNodeId = "loc_holdfast",
                        OffsetX = -70f,
                        OffsetY = -50f
                    }
                });

            Assert.True(map.EnsureTrapMarker("site_north", "trap_snare", "snare", false));
            Assert.True(map.EnsureTrapMarker("site_north", "trap_snare", "snare", true));
            Assert.Single(map.Markers);
            var marker = map.Markers[0];
            Assert.Equal("trap:site_north", marker.MarkerId);
            Assert.Equal("broken", marker.Condition);
            Assert.Equal(430f, marker.PositionX);
            Assert.Equal(250f, marker.PositionY);
            Assert.DoesNotContain("catch", marker.LabelKey, StringComparison.OrdinalIgnoreCase);

            map.ReconcileTrapMarkers(new[]
            {
                new TrapMapMarkerSource
                {
                    SiteId = "site_north",
                    TrapId = "trap_snare",
                    TrapType = "snare",
                    PositionX = 430f,
                    PositionY = 250f,
                    IsBroken = false
                }
            });
            Assert.Single(map.Markers);
            Assert.Equal("healthy", map.Markers[0].Condition);

            var restored = new WastelandMapSystem(
                new WastelandMapState(),
                new[] { new MapNode { Id = "loc_holdfast", PositionX = 500f, PositionY = 300f, StartingUnlocked = true } },
                Array.Empty<MapRoute>(),
                new[] { new TrapSiteMapLocation { SiteId = "site_north", AnchorNodeId = "loc_holdfast", OffsetX = -70f, OffsetY = -50f } });
            restored.RestoreState(map.CaptureState());
            Assert.Single(restored.Markers);
            Assert.Equal("healthy", restored.Markers[0].Condition);
            Assert.True(restored.RemoveTrapMarker("site_north"));
            Assert.Empty(restored.Markers);
        }

        [Fact]
        public void AuthoredMoraleValuesValidateAndDefaultToZero()
        {
            var rabbit = new PreyDefinition { speciesId = "rabbit", moraleEffect = 1f };
            var rat = new PreyDefinition { speciesId = "rat", moraleEffect = -1f };
            var dog = new PreyDefinition { speciesId = "rad_dog", moraleEffect = -3f };
            var fowl = new PreyDefinition { speciesId = "contaminated_fowl", moraleEffect = -2f };
            var legacy = new PreyDefinition { speciesId = "legacy" };

            Assert.True(rabbit.Validate(out _));
            Assert.True(rat.Validate(out _));
            Assert.True(dog.Validate(out _));
            Assert.True(fowl.Validate(out _));
            Assert.True(legacy.Validate(out _));
            Assert.Equal(0f, legacy.moraleEffect);
            Assert.False(new PreyDefinition { speciesId = "invalid", moraleEffect = float.NaN }.Validate(out _));
        }

        [Fact]
        public void TrapLifecycleEventsRemainIdempotentForBrokenAndRemovedStates()
        {
            var system = new WildlifeTrappingSystem(new SeededRng(1986));
            int broken = 0;
            int removed = 0;
            system.OnTrapBroken += _ => broken++;
            system.OnTrapRemoved += _ => removed++;

            Assert.True(system.SetTrap("site", "", "hunter", "snare", "trap_snare", 1, 1).IsSuccess);
            system.TickDay(3, 0f);
            Assert.Equal(1, broken);
            system.TickDay(5, 0f);
            Assert.Equal(1, broken);
            Assert.True(system.RemoveTrap("site").IsSuccess);
            Assert.Equal(1, removed);
            Assert.False(system.RemoveTrap("site").IsSuccess);
        }

        [Fact]
        public void ButcheryEmitsOnePrimaryIdentityAndCannotReplay()
        {
            var system = new WildlifeTrappingSystem(new SeededRng(1986));
            system.RestoreState(new WildlifeTrappingState
            {
                trapSites = new List<TrapSite>
                {
                    new TrapSite
                    {
                        siteId = "site",
                        setDay = 4,
                        deploymentSequence = 9,
                        hasCatch = true,
                        catchSpecies = "rabbit",
                        bycatchSpecies = "rat"
                    }
                },
                nextDeploymentSequence = 9
            });

            int events = 0;
            ButcheryCompletedEvent? completed = null;
            system.OnButcheryCompletedDetailed += e => { events++; completed = e; };

            Assert.True(system.Butcher("site", "survivor_1").IsSuccess);
            Assert.False(system.Butcher("site", "survivor_1").IsSuccess);
            Assert.Equal(1, events);
            Assert.NotNull(completed);
            Assert.Equal("rabbit", completed!.primarySpeciesId);
            Assert.Contains("9", completed.actionId, StringComparison.Ordinal);
            Assert.DoesNotContain("rat", completed.actionId, StringComparison.Ordinal);
        }
    }
}
