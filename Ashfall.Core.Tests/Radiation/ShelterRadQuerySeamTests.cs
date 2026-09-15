// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Inventory;
using Ashfall.Core.Radiation;
using Xunit;

namespace Ashfall.Core.Tests.Plan20ARadiation
{
    /// <summary>
    /// C2 / Plan 20A (G2) — ShelterRadQuery seam semantics. The seam is the
    /// 20B handoff point: when a context carries an interior-rad query, the
    /// canonical RadiationSystem must consume it in preference to the
    /// shielding-subtraction fallback, with exactly-once dose accumulation
    /// (plan §3.1/§49) and identical gear-protection semantics. 20B's
    /// ShelterShieldingModel will replace the query internals without
    /// changing these callers.
    /// </summary>
    public sealed class ShelterRadQuerySeamTests
    {
        private static ExposureContext Context(
            float zone, float shielding, Func<float, float>? query, List<WornGear>? worn = null)
        {
            return new ExposureContext
            {
                ZoneRadLevel = zone,
                ShelterShielding = shielding,
                ShelterRadQuery = query,
                WornGear = worn ?? new List<WornGear>()
            };
        }

        private static RadiationSystem SystemWith(SurvivorRadState survivor, ExposureContext ctx)
        {
            return new RadiationSystem(exposureContext: _ => ctx);
        }

        [Fact]
        public void QueryPresent_InteriorRadsReplaceShieldingSubtraction()
        {
            var s = new SurvivorRadState { Id = "a" };
            // Interior model: zone 40 → interior 8 (attenuated), regardless of
            // the ShelterShielding fallback value below.
            var sys = SystemWith(s, Context(40f, 999f, zone => MathF.Max(0f, zone - 32f)));
            sys.Register(s);
            sys.Tick(1f);
            Assert.Equal(8f, s.LifetimeRadiationExposure, 2);
        }

        [Fact]
        public void QueryPresent_GearProtectionStillSubtractedFromInterior()
        {
            var s = new SurvivorRadState { Id = "a" };
            var gear = new WornGear { RadProtection = 5f, MaxDurability = 10f, CurrentDurability = 10f };
            var sys = new RadiationSystem(exposureContext: _ =>
                Context(40f, 999f, zone => MathF.Max(0f, zone - 32f), new List<WornGear> { gear }));
            // Give the gear 5 protection via its protected set-up path.
            sys.Register(s);
            sys.Tick(1f);
            Assert.Equal(3f, s.LifetimeRadiationExposure, 2); // 8 interior − 5 gear
        }

        [Fact]
        public void QueryPresent_FallbackShieldingPath_IsNotAlsoApplied_NoDoubleCount()
        {
            var s = new SurvivorRadState { Id = "a" };
            // If both paths applied, dose would be 8 + (40 − 5) = 43.
            var sys = SystemWith(s, Context(40f, 5f, zone => MathF.Max(0f, zone - 32f)));
            sys.Register(s);
            sys.Tick(1f);
            Assert.Equal(8f, s.LifetimeRadiationExposure, 2);
        }

        [Fact]
        public void NoQuery_LegacyShieldingSubtraction_Preserved()
        {
            var s = new SurvivorRadState { Id = "a" };
            var sys = SystemWith(s, Context(40f, 12f, query: null));
            sys.Register(s);
            sys.Tick(1f);
            Assert.Equal(ComputeExpectedLegacy(40f, 12f, 0f), s.LifetimeRadiationExposure, 2);
        }

        [Fact]
        public void QueryNeverNegative_ClampedAtZero()
        {
            var s = new SurvivorRadState { Id = "a" };
            var sys = SystemWith(s, Context(2f, 999f, zone => zone - 40f));
            sys.Register(s);
            sys.Tick(1f);
            Assert.Equal(0f, s.LifetimeRadiationExposure, 2);
        }

        private static float ComputeExpectedLegacy(float zone, float shielding, float gear)
            => RadiationSystem.ComputeExposurePerHour(zone, gear, shielding);
    }
}
