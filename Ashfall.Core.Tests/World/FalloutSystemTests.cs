// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.World
{
    public class FalloutSystemTests
    {
        [Fact]
        public void FalloutSystem_SpawnCloud_InitializesCorrectParameters()
        {
            var system = new FalloutSystem();
            var cloud = system.SpawnCloud("fallout_pattern_strontium_plume", 10f, 20f, "zone_crater");

            Assert.NotNull(cloud);
            Assert.StartsWith("cloud_", cloud.cloudId);
            Assert.Equal("fallout_pattern_strontium_plume", cloud.patternId);
            Assert.Equal(10f, cloud.positionX);
            Assert.Equal(20f, cloud.positionY);
            Assert.True(cloud.active);
            Assert.True(cloud.radius > 0f);
            Assert.True(cloud.toxicity > 0f);
        }

        [Fact]
        public void FalloutSystem_CalculateWindDispersal_AdvectsAndExpandsCorrectly()
        {
            var system = new FalloutSystem();
            var cloud = system.SpawnCloud("fallout_pattern_strontium_plume", 0f, 0f);
            float initialRadius = cloud.radius;

            // Wind blowing East (90 degrees) at 20 km/h for 2 hours
            FalloutSystem.CalculateWindDispersal(cloud, 90f, 20f, 2f);

            // Cloud should have moved East (+X ~ 40km) and Y should be ~0
            Assert.True(cloud.positionX > 35f && cloud.positionX < 45f, $"Expected X ~ 40, got {cloud.positionX}");
            Assert.True(Math.Abs(cloud.positionY) < 1.0f, $"Expected Y ~ 0, got {cloud.positionY}");

            // Radius should have expanded
            Assert.True(cloud.radius > initialRadius);
        }

        [Fact]
        public void FalloutSystem_CalculateWindDispersal_ZeroWind_DiffusesWithoutDisplacement()
        {
            var system = new FalloutSystem();
            var cloud = system.SpawnCloud("fallout_pattern_strontium_plume", 50f, 50f);
            float initialRadius = cloud.radius;

            FalloutSystem.CalculateWindDispersal(cloud, 0f, 0f, 5f);

            Assert.Equal(50f, cloud.positionX);
            Assert.Equal(50f, cloud.positionY);
            Assert.True(cloud.radius > initialRadius);
        }

        [Fact]
        public void FalloutSystem_ZoneOverlapAndRadiationRate_CalculatesDistanceAttenuation()
        {
            var system = new FalloutSystem();
            var cloud = system.SpawnCloud("fallout_pattern_strontium_plume", 0f, 0f);
            cloud.radius = 20f;
            cloud.toxicity = 100f;

            // At origin (dist = 0), rate should be max toxicity
            float rateAtCenter = system.GetZoneRadiationRate("zone_origin", 0f, 0f);
            Assert.Equal(100f, rateAtCenter);

            // At edge (dist = 10), rate should be 50%
            float rateAtMid = system.GetZoneRadiationRate("zone_mid", 10f, 0f);
            Assert.Equal(50f, rateAtMid);

            // Outside radius (dist = 25), rate should be 0
            float rateOutside = system.GetZoneRadiationRate("zone_outside", 25f, 0f);
            Assert.Equal(0f, rateOutside);
        }

        [Fact]
        public void FalloutSystem_ShelterSealing_AttenuatesRadiationRate()
        {
            var system = new FalloutSystem();
            var cloud = system.SpawnCloud("fallout_pattern_strontium_plume", 0f, 0f);
            cloud.radius = 30f;
            cloud.toxicity = 100f;

            float unsealedRate = system.GetZoneRadiationRate("loc_holdfast", 0f, 0f);
            Assert.Equal(100f, unsealedRate);

            // Seal shelter with 80% efficiency for 24 hours
            bool sealedResult = system.SealShelter(24f, 0.80f);
            Assert.True(sealedResult);
            Assert.True(system.IsShelterSealed);

            float sealedRate = system.GetZoneRadiationRate("loc_holdfast", 0f, 0f);
            Assert.Equal(20f, sealedRate, 2);
        }

        [Fact]
        public void FalloutSystem_Tick_TriggersWarningAndGroundwaterTaint()
        {
            var system = new FalloutSystem();
            var cloud = system.SpawnCloud("fallout_pattern_strontium_plume", 0f, 0f);
            cloud.radius = 20f;

            bool warningFired = false;
            bool enteredZone = false;
            bool groundwaterTainted = false;

            system.OnFalloutWarning += (_, _, _) => warningFired = true;
            system.OnFalloutEnteredZone += (_, _) => enteredZone = true;
            system.OnGroundwaterTainted += (_) => groundwaterTainted = true;

            var zones = new Dictionary<string, (float x, float y)>
            {
                { "loc_water_well", (5f, 5f) }
            };

            // Tick for 15 hours (exceeding default 12h taint threshold)
            system.Tick(15f, 0f, 0f, zones);

            Assert.True(warningFired);
            Assert.True(enteredZone);
            Assert.True(groundwaterTainted);
            Assert.Contains("loc_water_well", system.State.taintedWaterSources);
        }

        [Fact]
        public void GetLocationContamination_SumsActiveCloudToxicityForOverlappingZone()
        {
            var system = new FalloutSystem();
            var cloud = system.SpawnCloud("fallout_pattern_strontium_plume", 0f, 0f);
            cloud.toxicity = 80f;
            cloud.activeZoneOverlaps.Add("loc_crater_rim");

            Assert.Equal(80f, system.GetLocationContamination("loc_crater_rim"));
            Assert.Equal(0f, system.GetLocationContamination("loc_unrelated"));
            Assert.Equal(0f, system.GetLocationContamination(""));
        }

        [Fact]
        public void GetLocationContamination_ShelterSealAttenuatesHoldfastDose()
        {
            var system = new FalloutSystem();
            var cloud = system.SpawnCloud("fallout_pattern_strontium_plume", 0f, 0f);
            cloud.toxicity = 100f;
            cloud.activeZoneOverlaps.Add("loc_holdfast");

            Assert.Equal(100f, system.GetLocationContamination("loc_holdfast"));

            Assert.True(system.SealShelter(24f, 0.80f));
            Assert.Equal(20f, system.GetLocationContamination("loc_holdfast"), 2);
        }
    }
}
