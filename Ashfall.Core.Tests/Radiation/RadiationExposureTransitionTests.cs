// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Reflection;
using Ashfall.Core.Radiation;
using Xunit;

namespace Ashfall.Core.Tests.Plan17CRadiation
{
    /// <summary>
    /// C2 / Plan 17C Phase G — the explicit exposure-end lifecycle. Pins the
    /// Core contract the geiger loop depends on: start once, sustained exposure
    /// does not re-fire, end fires exactly once, repeated end is harmless,
    /// unregister closes the transition, restore never replays events, and the
    /// tracking set is transient (no save-schema churn).
    /// </summary>
    public sealed class RadiationExposureTransitionTests
    {
        private sealed class Zone
        {
            public float Rad;
            public ExposureContext Ctx => new() { ZoneRadLevel = Rad, ShelterShielding = 0f };
        }

        private static (RadiationSystem Sys, SurvivorRadState S, Zone Z) Make(float initialRad = 0f)
        {
            var z = new Zone { Rad = initialRad };
            var sys = new RadiationSystem(_ => z.Ctx);
            var s = new SurvivorRadState { Id = "s1" };
            sys.Register(s);
            return (sys, s, z);
        }

        [Fact]
        public void ExposureBegin_FiresStarted_ExactlyOnce()
        {
            var (sys, _, z) = Make();
            int starts = 0;
            sys.OnExposureStarted += _ => starts++;

            z.Rad = 12f;
            sys.Tick(24f);
            sys.Tick(24f); // sustained exposure — no restart spam

            Assert.Equal(1, starts);
            Assert.True(sys.IsExposureActive("s1"));
        }

        [Fact]
        public void ExposureEnd_FiresEnded_AndIsIdempotent()
        {
            var (sys, _, z) = Make(12f);
            int ends = 0;
            sys.OnExposureEnded += _ => ends++;

            sys.Tick(24f);          // starts
            z.Rad = 0f;
            sys.Tick(24f);          // ends
            sys.Tick(24f);          // repeated end — harmless

            Assert.Equal(1, ends);
            Assert.False(sys.IsExposureActive("s1"));
        }

        [Fact]
        public void FullCycle_RestartsOnNewExposure()
        {
            var (sys, _, z) = Make();
            int starts = 0, ends = 0;
            sys.OnExposureStarted += _ => starts++;
            sys.OnExposureEnded += _ => ends++;

            z.Rad = 10f; sys.Tick(24f);
            z.Rad = 0f; sys.Tick(24f);
            z.Rad = 10f; sys.Tick(24f);

            Assert.Equal(2, starts);
            Assert.Equal(1, ends);
        }

        [Fact]
        public void FreshSystem_NoEventsWithoutTick()
        {
            // Registering alone (the restore path) never replays begin/end —
            // C2 §14.4: capture/restore must not spuriously replay transitions.
            var (sys, _, z) = Make(25f);
            int events = 0;
            sys.OnExposureStarted += _ => events++;
            sys.OnExposureEnded += _ => events++;

            Assert.Equal(0, events);
            Assert.False(sys.IsExposureActive("s1"));
        }

        [Fact]
        public void Unregister_ClosesTransition()
        {
            var (sys, s, z) = Make(15f);
            int ends = 0;
            sys.OnExposureEnded += _ => ends++;

            sys.Tick(24f);
            sys.Unregister(s);

            Assert.Equal(1, ends);
            Assert.False(sys.IsExposureActive("s1"));
        }

        [Fact]
        public void MultipleSurvivors_IndependentTransitions()
        {
            var z = new Zone { Rad = 10f };
            var sys = new RadiationSystem(_ => z.Ctx);
            sys.Register(new SurvivorRadState { Id = "a" });
            sys.Register(new SurvivorRadState { Id = "b" });

            var exposed = new HashSet<string>();
            sys.OnExposureStarted += s => exposed.Add(s.Id);
            sys.OnExposureEnded += s => exposed.Remove(s.Id);

            sys.Tick(24f);
            Assert.Equal(2, exposed.Count);
        }

        [Fact]
        public void ExposureTracking_IsTransient_NoPublicPersistence()
        {
            // The tracking set must not surface in any capture/restore API
            // (no save-schema churn; restore re-derives on the next tick).
            var field = typeof(RadiationSystem).GetField("_exposureActiveBySurvivor",
                BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(field);
            Assert.True(field!.IsPrivate, "exposure tracking must stay internal/transient");
            foreach (var member in typeof(RadiationSystem).GetMembers(BindingFlags.Public | BindingFlags.Instance))
                Assert.DoesNotContain("ExposureActiveBySurvivor", member.Name);
        }
    }
}
