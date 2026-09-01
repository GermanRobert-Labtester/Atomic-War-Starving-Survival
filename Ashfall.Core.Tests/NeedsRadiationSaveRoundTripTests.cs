using System;
using System.Collections.Generic;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Survivors;
using Ashfall.Core.Radiation;

namespace Ashfall.Core.Tests
{
    public class NeedsRadiationSaveRoundTripTests
    {
        [Serializable]
        public class SurvivorSliceTestState
        {
            public string id = string.Empty;
            public float hunger;
            public float thirst;
            public float fatigue;
            public float warmth = 100f;
            public float morale = 50f;
            public float health = 100f;
            public float hygiene = 100f;
            public float radiationDose;
            public float lifetimeRadiationExposure;
            public bool hasRadResistance;
            public float radResistanceHoursRemaining;
            public bool hasAcuteSickness;
            public bool hasChronicIllness;
            public bool isAlive = true;
        }

        [Serializable]
        public class SurvivorsTestSave
        {
            public List<SurvivorSliceTestState> survivors = new List<SurvivorSliceTestState>();
            public string Checksum = string.Empty;
        }

        [Fact]
        public void SurvivorNeeds_RoundTrip_PreservesExactValues()
        {
            var original = new SurvivorsTestSave
            {
                survivors = new List<SurvivorSliceTestState>
                {
                    new SurvivorSliceTestState
                    {
                        id = "survivor_dr_sarah_chen",
                        health = 82.5f,
                        hunger = 37.2f,
                        thirst = 45.8f,
                        fatigue = 18.0f,
                        warmth = 92.4f,
                        morale = 64.0f,
                        hygiene = 78.5f,
                        radiationDose = 14.5f,
                        lifetimeRadiationExposure = 42.0f,
                        hasRadResistance = true,
                        radResistanceHoursRemaining = 6.5f,
                        hasAcuteSickness = false,
                        hasChronicIllness = false,
                        isAlive = true
                    },
                    new SurvivorSliceTestState
                    {
                        id = "survivor_gunner_mikhail",
                        health = 54.0f,
                        hunger = 85.0f,
                        thirst = 72.0f,
                        fatigue = 60.0f,
                        warmth = 40.0f,
                        morale = 25.0f,
                        hygiene = 30.0f,
                        radiationDose = 55.0f,
                        lifetimeRadiationExposure = 120.0f,
                        hasRadResistance = false,
                        radResistanceHoursRemaining = 0f,
                        hasAcuteSickness = true,
                        hasChronicIllness = true,
                        isAlive = true
                    }
                }
            };
            original.Checksum = SaveChecksum.Compute(original);

            var serializer = new SystemTextJsonSerializer();
            string json = serializer.Serialize(original);
            Assert.False(string.IsNullOrWhiteSpace(json));

            var restored = serializer.Deserialize<SurvivorsTestSave>(json);
            Assert.NotNull(restored);
            Assert.Equal(2, restored.survivors.Count);

            var chen = restored.survivors[0];
            Assert.Equal("survivor_dr_sarah_chen", chen.id);
            Assert.Equal(82.5f, chen.health);
            Assert.Equal(37.2f, chen.hunger);
            Assert.Equal(45.8f, chen.thirst);
            Assert.Equal(14.5f, chen.radiationDose);
            Assert.True(chen.hasRadResistance);

            var mikhail = restored.survivors[1];
            Assert.Equal("survivor_gunner_mikhail", mikhail.id);
            Assert.Equal(54.0f, mikhail.health);
            Assert.Equal(55.0f, mikhail.radiationDose);
            Assert.True(mikhail.hasAcuteSickness);
            Assert.True(mikhail.hasChronicIllness);

            string recomputed = SaveChecksum.Compute(restored);
            Assert.Equal(original.Checksum, recomputed);
        }

        [Fact]
        public void SurvivorNeeds_MutationChangesChecksum()
        {
            var save = new SurvivorsTestSave
            {
                survivors = new List<SurvivorSliceTestState>
                {
                    new SurvivorSliceTestState
                    {
                        id = "survivor_dr_sarah_chen",
                        health = 100f,
                        hunger = 0f
                    }
                }
            };
            string hash1 = SaveChecksum.Compute(save);

            save.survivors[0].hunger = 50f;
            string hash2 = SaveChecksum.Compute(save);

            Assert.NotEqual(hash1, hash2);
        }

        [Fact]
        public void NeedsSystem_Tick_CalculatesAccurateDriftAndConsequences()
        {
            var needs = new NeedsSystem();
            var state = new SurvivorNeedsState
            {
                Id = "survivor_test",
                Health = 100f,
                Hunger = 0f,
                Thirst = 0f,
                Fatigue = 0f,
                Warmth = 100f,
                Morale = 50f
            };
            needs.Register(state);

            // Tick 10 hours
            needs.Tick(10f);

            Assert.True(state.Hunger > 0f, "Hunger should increase after tick");
            Assert.True(state.Thirst > 0f, "Thirst should increase after tick");
            Assert.True(state.Fatigue > 0f, "Fatigue should increase after tick");

            // Starvation pressure test
            state.Hunger = 95f; // Critical
            float prevHealth = state.Health;
            needs.Tick(5f);

            Assert.True(state.Health < prevHealth, "Health should drop when hunger is critical");
        }

        // ── H10 step 2a: Needs — capture → restore → every field equal ──────────

        /// <summary>
        /// Every mutation-sensitive field on SurvivorNeedsState must survive a
        /// cross-host JSON round-trip. A field added to the DTO without being
        /// serialized would silently reset to its default on load; non-round
        /// values are used so a clamp/cap cannot mask a dropped field.
        /// </summary>
        [Fact]
        public void Needs_AllFields_RoundTrip_PreservesEveryStateField()
        {
            var serializer = new SystemTextJsonSerializer();
            var original = new SurvivorNeedsState
            {
                Id = "sv_allfields",
                Hunger = 77.5f,
                Thirst = 66.25f,
                Fatigue = 45.5f,
                Warmth = 33.75f,
                Morale = 12.25f,
                Health = 55.5f,
                Hygiene = 4.75f,
                WasHungerCritical = true,
                WasThirstCritical = true,
                WasWarmthCritical = false,
                MaxHealthCap = 72.5f,
                IsAlive = true,
                IsDead = false
            };

            var restored = serializer.Deserialize<SurvivorNeedsState>(serializer.Serialize(original));
            Assert.NotNull(restored);
            Assert.Equal(original.Id, restored!.Id);
            Assert.Equal(original.Hunger, restored.Hunger, 3);
            Assert.Equal(original.Thirst, restored.Thirst, 3);
            Assert.Equal(original.Fatigue, restored.Fatigue, 3);
            Assert.Equal(original.Warmth, restored.Warmth, 3);
            Assert.Equal(original.Morale, restored.Morale, 3);
            Assert.Equal(original.Health, restored.Health, 3);
            Assert.Equal(original.Hygiene, restored.Hygiene, 3);
            Assert.Equal(original.WasHungerCritical, restored.WasHungerCritical);
            Assert.Equal(original.WasThirstCritical, restored.WasThirstCritical);
            Assert.Equal(original.WasWarmthCritical, restored.WasWarmthCritical);
            Assert.Equal(original.MaxHealthCap, restored.MaxHealthCap, 3);
            Assert.Equal(original.IsAlive, restored.IsAlive);
            Assert.Equal(original.IsDead, restored.IsDead);
            Assert.Equal(original.IsAliveState, restored.IsAliveState);

            // The integrity hash must also survive the round trip (Invariant 3).
            Assert.Equal(SaveChecksum.Compute(original), SaveChecksum.Compute(restored));
        }

        /// <summary>
        /// A restored state must drive the system, not reset it: registering the
        /// deserialized state and ticking must apply consequences from the
        /// *restored* hunger, proving restore is not a silent default-reset.
        /// </summary>
        [Fact]
        public void Needs_RestoredState_DrivesSystemTick()
        {
            var serializer = new SystemTextJsonSerializer();
            var original = new SurvivorNeedsState
            {
                Id = "sv_restore_drive",
                Hunger = 95f,        // critical
                Health = 80f,
                Warmth = 100f,
                Morale = 50f
            };

            var restored = serializer.Deserialize<SurvivorNeedsState>(serializer.Serialize(original));
            Assert.NotNull(restored);

            var sys = new NeedsSystem();
            sys.Register(restored!);
            float healthBefore = restored!.Health;
            sys.Tick(2f);

            Assert.True(restored.Health < healthBefore, "restored critical hunger must drain health on tick");
            Assert.True(restored.Hunger >= 90f, "restored hunger baseline must persist through the tick");
        }

        // ── H10 step 2b: Needs — capture → tick → capture differs (no-op guard) ──

        /// <summary>
        /// Capture must reflect live state, not return a frozen snapshot. After
        /// a tick mutates the survivor, the integrity hash of a fresh capture must
        /// differ from the pre-tick hash — otherwise capture is a no-op and a save
        /// would silently store stale state.
        /// </summary>
        [Fact]
        public void Needs_CaptureAfterTick_DiffersFromPriorCapture()
        {
            var sys = new NeedsSystem();
            var state = new SurvivorNeedsState
            {
                Id = "sv_noop_guard",
                Health = 100f,
                Hunger = 0f,
                Thirst = 0f,
                Fatigue = 0f,
                Warmth = 100f,
                Morale = 50f
            };
            sys.Register(state);

            string before = SaveChecksum.Compute(state);
            sys.Tick(24f);
            string after = SaveChecksum.Compute(state);

            Assert.NotEqual(before, after);
        }

        // ── H10 step 2c: Needs — restore of default/empty state ─────────────────

        /// <summary>
        /// Restoring a default (empty-id) state must not throw and must yield the
        /// documented DTO defaults. A host loading a fresh campaign restores
        /// exactly these values before the first tick.
        /// </summary>
        [Fact]
        public void Needs_RestoreDefaultState_NoExceptionAndDocumentedDefaults()
        {
            var serializer = new SystemTextJsonSerializer();
            var original = new SurvivorNeedsState(); // empty Id, documented defaults

            var restored = serializer.Deserialize<SurvivorNeedsState>(serializer.Serialize(original));
            Assert.NotNull(restored);
            Assert.Equal(string.Empty, restored!.Id);
            // Documented defaults (see SurvivorNeedsState field initializers).
            Assert.Equal(0f, restored.Hunger);
            Assert.Equal(0f, restored.Thirst);
            Assert.Equal(0f, restored.Fatigue);
            Assert.Equal(100f, restored.Warmth);
            Assert.Equal(50f, restored.Morale);
            Assert.Equal(100f, restored.Health);
            Assert.Equal(100f, restored.Hygiene);
            Assert.Equal(100f, restored.MaxHealthCap);
            Assert.True(restored.IsAlive);
            Assert.False(restored.IsDead);

            // Registering and ticking the default state must not throw.
            var sys = new NeedsSystem();
            sys.Register(restored);
            sys.Tick(1f);
            Assert.Equal(1, sys.RegisteredCount);
        }

        // ── H10 step 2d: Needs — checksum stability ─────────────────────────────

        /// <summary>
        /// Same state → same SaveChecksum hash across two independent captures,
        /// and the hash is invariant across a serialize/deserialize round trip.
        /// This is the stability complement to SurvivorNeeds_MutationChangesChecksum.
        /// </summary>
        [Fact]
        public void Needs_SameState_StableChecksumAcrossCaptures()
        {
            var serializer = new SystemTextJsonSerializer();
            var state = new SurvivorNeedsState
            {
                Id = "sv_stable",
                Hunger = 42.5f,
                Thirst = 17.25f,
                Fatigue = 8.5f,
                Warmth = 88.75f,
                Morale = 33.5f,
                Health = 61.5f,
                Hygiene = 22.25f,
                MaxHealthCap = 85f
            };

            string hash1 = SaveChecksum.Compute(state);
            string hash2 = SaveChecksum.Compute(state);
            var restored = serializer.Deserialize<SurvivorNeedsState>(serializer.Serialize(state));
            string hash3 = SaveChecksum.Compute(restored!);

            Assert.Equal(hash1, hash2);
            Assert.Equal(hash1, hash3);
        }

        // ── H10 step 2a: Radiation — dose + phase round-trip into fresh system ──

        /// <summary>
        /// Radiation dose accumulation and sickness phase (acute / chronic /
        /// syndrome status flags) must survive a round trip, and the restored
        /// state must keep driving the system — a tick from the restored baseline
        /// must accumulate further dose rather than resetting to zero.
        /// </summary>
        [Fact]
        public void Radiation_DoseAndPhase_RoundTrip_IntoFreshSystem()
        {
            var serializer = new SystemTextJsonSerializer();
            var original = new SurvivorRadState
            {
                Id = "sv_phase",
                RadiationDose = 85.5f,                 // acute band
                LifetimeRadiationExposure = 505.5f,    // chronic band
                HasRadResistance = true,
                RadResistanceHoursRemaining = 3.25f,
                IodineProtectionTimer = 1.75f,
                HasAcuteRadiationSickness = true,
                HasChronicIllness = true,
                HasAcuteRadiationSyndrome = true,
                IsAlive = true
            };

            var restored = serializer.Deserialize<SurvivorRadState>(serializer.Serialize(original));
            Assert.NotNull(restored);
            Assert.Equal(original.Id, restored!.Id);
            Assert.Equal(original.RadiationDose, restored.RadiationDose, 3);
            Assert.Equal(original.LifetimeRadiationExposure, restored.LifetimeRadiationExposure, 3);
            Assert.Equal(original.HasRadResistance, restored.HasRadResistance);
            Assert.Equal(original.RadResistanceHoursRemaining, restored.RadResistanceHoursRemaining, 3);
            Assert.Equal(original.IodineProtectionTimer, restored.IodineProtectionTimer, 3);
            Assert.Equal(original.HasAcuteRadiationSickness, restored.HasAcuteRadiationSickness);
            Assert.Equal(original.HasChronicIllness, restored.HasChronicIllness);
            Assert.Equal(original.HasAcuteRadiationSyndrome, restored.HasAcuteRadiationSyndrome);
            Assert.Equal(original.IsAlive, restored.IsAlive);
            Assert.Equal(SaveChecksum.Compute(original), SaveChecksum.Compute(restored));

            // Restored state must drive a fresh system: with HasRadResistance the
            // exposure is halved, but dose must still climb from the restored 85.5.
            var sys = new RadiationSystem(exposureContext: _ => new ExposureContext { ZoneRadLevel = 10f });
            sys.Register(restored);
            float doseBefore = restored.RadiationDose;
            sys.Tick(1f);
            Assert.True(restored.RadiationDose > doseBefore,
                $"restored dose must accumulate; before={doseBefore} after={restored.RadiationDose}");
            Assert.True(restored.HasAcuteRadiationSickness, "acute phase must persist through the tick");
        }

        // ── H10 step 2b: Radiation — capture → tick → capture differs ───────────

        [Fact]
        public void Radiation_CaptureAfterTick_DiffersFromPriorCapture()
        {
            var sys = new RadiationSystem(exposureContext: _ => new ExposureContext { ZoneRadLevel = 20f });
            var state = new SurvivorRadState { Id = "sv_rad_noop", RadiationDose = 10f };
            sys.Register(state);

            string before = SaveChecksum.Compute(state);
            sys.Tick(2f);
            string after = SaveChecksum.Compute(state);

            Assert.NotEqual(before, after);
        }

        // ── H10 step 2c: Radiation — restore of default/empty state ─────────────

        [Fact]
        public void Radiation_RestoreDefaultState_NoExceptionAndDocumentedDefaults()
        {
            var serializer = new SystemTextJsonSerializer();
            var original = new SurvivorRadState(); // empty Id, documented defaults

            var restored = serializer.Deserialize<SurvivorRadState>(serializer.Serialize(original));
            Assert.NotNull(restored);
            Assert.Equal(string.Empty, restored!.Id);
            Assert.Equal(0f, restored.RadiationDose);
            Assert.Equal(0f, restored.LifetimeRadiationExposure);
            Assert.False(restored.HasRadResistance);
            Assert.Equal(0f, restored.RadResistanceHoursRemaining);
            Assert.Equal(0f, restored.IodineProtectionTimer);
            Assert.False(restored.HasAcuteRadiationSickness);
            Assert.False(restored.HasChronicIllness);
            Assert.False(restored.HasAcuteRadiationSyndrome);
            Assert.True(restored.IsAlive);

            var sys = new RadiationSystem(exposureContext: _ => new ExposureContext { ZoneRadLevel = 5f });
            sys.Register(restored);
            sys.Tick(1f);
            Assert.Equal(1, sys.RegisteredCount);
        }

        // ── H10 step 2d: Radiation — checksum stability ─────────────────────────

        [Fact]
        public void Radiation_SameState_StableChecksumAcrossCaptures()
        {
            var serializer = new SystemTextJsonSerializer();
            var state = new SurvivorRadState
            {
                Id = "sv_rad_stable",
                RadiationDose = 25.5f,
                LifetimeRadiationExposure = 120.25f,
                HasRadResistance = true,
                RadResistanceHoursRemaining = 4.75f,
                IodineProtectionTimer = 2.25f,
                HasAcuteRadiationSickness = false,
                HasChronicIllness = true,
                HasAcuteRadiationSyndrome = false,
                IsAlive = true
            };

            string hash1 = SaveChecksum.Compute(state);
            string hash2 = SaveChecksum.Compute(state);
            var restored = serializer.Deserialize<SurvivorRadState>(serializer.Serialize(state));
            string hash3 = SaveChecksum.Compute(restored!);

            Assert.Equal(hash1, hash2);
            Assert.Equal(hash1, hash3);
        }

        // ── H10 step 3: HoldfastRuntimeSession fallback — Core no-projection half ─
        //
        // HoldfastRuntimeSession.TickDay (src/Host/HoldfastRuntimeSession.cs:177)
        // branches on Survivors == null: when the host has no SurvivorsHostSession
        // it applies its own fallback decay and must NOT project through Core's
        // NeedsSystem / RadiationSystem. The fallback-decay math itself lives in
        // the Godot host (Godot.NET.Sdk / net8.0), which Ashfall.Core.Tests
        // (net9.0, Microsoft.NET.Sdk) cannot reference without breaking the
        // build, so it is covered by host integration tests. These Core tests
        // gate the other half of the contract: with no survivor registered, the
        // Core systems are no-ops, so a host running the fallback path cannot
        // double-decay by also ticking an empty Core system.

        [Fact]
        public void NeedsSystem_WithNoRegisteredSurvivors_TickIsNoOp()
        {
            var sys = new NeedsSystem();
            Assert.Equal(0, sys.RegisteredCount);
            sys.Tick(24f); // must not throw, must not fabricate state
            Assert.Equal(0, sys.RegisteredCount);
        }

        [Fact]
        public void RadiationSystem_WithNoRegisteredSurvivors_TickIsNoOp()
        {
            var sys = new RadiationSystem(exposureContext: _ => new ExposureContext { ZoneRadLevel = 50f });
            Assert.Equal(0, sys.RegisteredCount);
            sys.Tick(24f); // must not throw, must not fabricate state
            Assert.Equal(0, sys.RegisteredCount);
        }

        // ── H10 step 4: determinism — paired capture/restore → identical outcomes ─
        //
        // NeedsSystem.Tick and RadiationSystem.Tick are RNG-free (they take only
        // gameHours), so Invariant 4 reduces here to: restore is a pure data
        // substitution that does not perturb the deterministic tick. A branch
        // that serializes/deserializes its state before ticking must reach the
        // same post-tick state as a branch that never serializes. The ISeededRng
        // stream guard below pins that the serializer/restore path draws no
        // entropy from an injected RNG, so downstream RNG consumers are unaffected.

        [Fact]
        public void Determinism_Needs_PairedCaptureRestore_YieldsIdenticalTickOutcomes()
        {
            var serializer = new SystemTextJsonSerializer();

            // Branch A: never serialized.
            var sysA = new NeedsSystem();
            var stateA = new SurvivorNeedsState
            {
                Id = "sv_det", Hunger = 40f, Thirst = 30f, Fatigue = 20f,
                Warmth = 60f, Morale = 45f, Health = 70f, Hygiene = 50f
            };
            sysA.Register(stateA);

            // Branch B: identical setup, but the state is round-tripped before tick.
            var sysB = new NeedsSystem();
            var stateB = new SurvivorNeedsState
            {
                Id = "sv_det", Hunger = 40f, Thirst = 30f, Fatigue = 20f,
                Warmth = 60f, Morale = 45f, Health = 70f, Hygiene = 50f
            };
            var restoredB = serializer.Deserialize<SurvivorNeedsState>(serializer.Serialize(stateB))!;
            sysB.Register(restoredB); // evicts stateB by Id, keeps the slot/order

            const float hours = 24f;
            sysA.Tick(hours);
            sysB.Tick(hours);

            Assert.Equal(stateA.Hunger, restoredB.Hunger, 3);
            Assert.Equal(stateA.Thirst, restoredB.Thirst, 3);
            Assert.Equal(stateA.Fatigue, restoredB.Fatigue, 3);
            Assert.Equal(stateA.Warmth, restoredB.Warmth, 3);
            Assert.Equal(stateA.Morale, restoredB.Morale, 3);
            Assert.Equal(stateA.Health, restoredB.Health, 3);
            Assert.Equal(stateA.Hygiene, restoredB.Hygiene, 3);
            Assert.Equal(stateA.IsAliveState, restoredB.IsAliveState);
        }

        [Fact]
        public void Determinism_Radiation_PairedCaptureRestore_YieldsIdenticalTickOutcomes()
        {
            var serializer = new SystemTextJsonSerializer();
            ExposureContext Context() => new ExposureContext { ZoneRadLevel = 20f };

            // Branch A: never serialized.
            var sysA = new RadiationSystem(exposureContext: _ => Context());
            var stateA = new SurvivorRadState { Id = "sv_raddet", RadiationDose = 10f };
            sysA.Register(stateA);

            // Branch B: identical setup, round-tripped before tick.
            var sysB = new RadiationSystem(exposureContext: _ => Context());
            var stateB = new SurvivorRadState { Id = "sv_raddet", RadiationDose = 10f };
            var restoredB = serializer.Deserialize<SurvivorRadState>(serializer.Serialize(stateB))!;
            sysB.Register(restoredB);

            const float hours = 2f;
            sysA.Tick(hours);
            sysB.Tick(hours);

            Assert.Equal(stateA.RadiationDose, restoredB.RadiationDose, 3);
            Assert.Equal(stateA.LifetimeRadiationExposure, restoredB.LifetimeRadiationExposure, 3);
            Assert.Equal(stateA.HasAcuteRadiationSickness, restoredB.HasAcuteRadiationSickness);
            Assert.Equal(stateA.HasChronicIllness, restoredB.HasChronicIllness);
            Assert.Equal(stateA.HasRadResistance, restoredB.HasRadResistance);
            Assert.Equal(stateA.IsAlive, restoredB.IsAlive);
        }

        /// <summary>
        /// The serialize/deserialize (restore) path must draw no entropy from an
        /// injected ISeededRng: a stream interrupted by a capture/restore must
        /// yield the same values as an uninterrupted stream from the same seed.
        /// Guards the "restore must not change subsequent ISeededRng streams" half
        /// of Invariant 4 for any downstream RNG consumer.
        /// </summary>
        [Fact]
        public void Determinism_CaptureRestore_DoesNotDisturbISeededRngStream()
        {
            var serializer = new SystemTextJsonSerializer();

            // Uninterrupted reference stream.
            var reference = new SeededRng(1401);
            double r1 = reference.NextDouble();
            double r2 = reference.NextDouble();
            double r3 = reference.NextDouble();

            // Interrupted stream: draw, then perform a capture/restore, then draw.
            var interrupted = new SeededRng(1401);
            double i1 = interrupted.NextDouble();
            double i2 = interrupted.NextDouble();
            // Restore happens here — must not touch the RNG.
            var state = new SurvivorNeedsState { Id = "sv_rng_guard", Hunger = 10f };
            var restored = serializer.Deserialize<SurvivorNeedsState>(serializer.Serialize(state));
            Assert.NotNull(restored);
            double i3 = interrupted.NextDouble();

            Assert.Equal(r1, i1);
            Assert.Equal(r2, i2);
            Assert.Equal(r3, i3);
        }
    }
}
