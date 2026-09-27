// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Triple Package J — consolidated Core-contract suite.
    ///
    /// Covers the three newly host-integrated Core authorities through their own
    /// public contracts (Ashfall.Core.Tests stays engine-free; host wiring is
    /// proven by the headless CLI probes):
    ///   1. VoluntaryRegisterSystem — high-dose volunteer signatures.
    ///   2. WorldEvolutionEngine    — authored world-state evolution events.
    /// </summary>
    public sealed class PlanTriplePackageJCoreTests
    {
        // ── 1. VoluntaryRegisterSystem ──────────────────────────────────────

        [Fact]
        public void VoluntaryRegister_SignatureIsPerTaskNotPerSurvivor()
        {
            var system = new VoluntaryRegisterSystem();

            Assert.True(system.Volunteer("survivor_a", "surface_radiation_work", 10));
            // A second, different task for the same survivor is a new signature.
            Assert.True(system.Volunteer("survivor_a", "tunnel_scouting", 11));
            Assert.Equal(2, system.Entries.Count);
        }

        [Fact]
        public void VoluntaryRegister_DoubleSigningSameTaskIsRejected()
        {
            var system = new VoluntaryRegisterSystem();

            Assert.True(system.Volunteer("survivor_a", "surface_radiation_work", 10));
            // Re-signing the identical task must not duplicate or diverge the saved list.
            Assert.False(system.Volunteer("survivor_a", "surface_radiation_work", 11));
            Assert.Equal(1, system.Entries.Count);
        }

        [Fact]
        public void VoluntaryRegister_CompletionBanksDoseExactlyOnce()
        {
            var system = new VoluntaryRegisterSystem();
            system.Volunteer("survivor_a", "surface_radiation_work", 10);

            Assert.True(system.CompleteVolunteer("survivor_a", "surface_radiation_work", 42f, 15));
            // A second completion for the same signature must be refused.
            Assert.False(system.CompleteVolunteer("survivor_a", "surface_radiation_work", 99f, 16));

            var entry = system.Entries[0];
            Assert.True(entry.completed);
            Assert.Equal(42f, entry.doseIncurred);
            Assert.Equal(15, entry.completedDay);
        }

        [Fact]
        public void VoluntaryRegister_CaptureRestoreIsExact()
        {
            var system = new VoluntaryRegisterSystem();
            system.Volunteer("survivor_a", "surface_radiation_work", 10);
            system.CompleteVolunteer("survivor_a", "surface_radiation_work", 42f, 15);

            var captured = system.CaptureState();

            system.RestoreState(new VoluntaryRegisterSystemState());
            Assert.Empty(system.Entries);

            system.RestoreState(captured);

            Assert.Single(system.Entries);
            Assert.True(system.Entries[0].completed);
            Assert.Equal(42f, system.Entries[0].doseIncurred);
        }

        [Fact]
        public void VoluntaryRegister_EventsFireForSignupAndCompletion()
        {
            var system = new VoluntaryRegisterSystem();
            string? signedUpSurvivor = null;
            float bankedDose = 0f;
            system.OnVolunteered += (id, _) => signedUpSurvivor = id;
            system.OnVolunteerCompleted += (_, dose) => bankedDose = dose;

            system.Volunteer("survivor_a", "surface_radiation_work", 10);
            system.CompleteVolunteer("survivor_a", "surface_radiation_work", 42f, 15);

            Assert.Equal("survivor_a", signedUpSurvivor);
            Assert.Equal(42f, bankedDose);
        }

        // ── 2. WorldEvolutionEngine ─────────────────────────────────────────

        private static WorldEvolutionEngine NewEvolutionEngine()
        {
            string dataDir = System.IO.Path.Combine("Assets", "StreamingAssets", "Data");
            return new WorldEvolutionEngine(dataDir);
        }

        [Fact]
        public void WorldEvolution_AuthoredCatalogReachesTheEngine()
        {
            var engine = NewEvolutionEngine();
            Assert.True(engine.Events.Count >= 10,
                $"Expected the authored world-evolution catalog (>= 10 events), got {engine.Events.Count}");
        }

        [Fact]
        public void WorldEvolution_EventFiresOnAuthoredDayAndIsNotRepeated()
        {
            var engine = NewEvolutionEngine();

            // The earliest *ungated* authored day: an event with an empty
            // required_flag fires on its authored day with no flag set.
            int firstUngatedDay = int.MaxValue;
            foreach (var evt in engine.Events)
                if (string.IsNullOrEmpty(evt.required_flag))
                    firstUngatedDay = Math.Min(firstUngatedDay, evt.trigger_day);

            Assert.True(firstUngatedDay < int.MaxValue && firstUngatedDay >= 0,
                "Authored catalog must declare at least one ungated event with a non-negative trigger day");

            engine.TickDay(firstUngatedDay, new HashSet<string>(StringComparer.OrdinalIgnoreCase), null, null, null);
            var afterFirst = new HashSet<string>(engine.TriggeredEventIds, StringComparer.OrdinalIgnoreCase);
            Assert.True(afterFirst.Count >= 1, "Expected at least one authored event to fire on its authored day");

            // Ticking the SAME day again must be idempotent: no event re-fires.
            engine.TickDay(firstUngatedDay, new HashSet<string>(StringComparer.OrdinalIgnoreCase), null, null, null);
            Assert.Equal(afterFirst.Count, engine.TriggeredEventIds.Count);

            // Later days may fire additional DISTINCT authored events, but a
            // triggered event is never removed and never re-runs its body.
            engine.TickDay(firstUngatedDay + 5, new HashSet<string>(StringComparer.OrdinalIgnoreCase), null, null, null);
            foreach (string id in afterFirst)
                Assert.Contains(id, engine.TriggeredEventIds);
            Assert.True(engine.TriggeredEventIds.Count >= afterFirst.Count);
        }

        [Fact]
        public void WorldEvolution_CaptureRestoreKeepsTriggeredSetAndDoesNotReplay()
        {
            var engine = NewEvolutionEngine();
            int firstUngatedDay = int.MaxValue;
            foreach (var evt in engine.Events)
                if (string.IsNullOrEmpty(evt.required_flag))
                    firstUngatedDay = Math.Min(firstUngatedDay, evt.trigger_day);

            engine.TickDay(firstUngatedDay, new HashSet<string>(StringComparer.OrdinalIgnoreCase), null, null, null);
            int triggered = engine.TriggeredEventIds.Count;
            Assert.True(triggered >= 1);

            var captured = engine.CaptureState();

            var fresh = NewEvolutionEngine();
            Assert.Empty(fresh.TriggeredEventIds);
            fresh.RestoreState(captured, null);
            Assert.Equal(triggered, fresh.TriggeredEventIds.Count);

            // Restoring must not re-run the event bodies.
            fresh.TickDay(firstUngatedDay + 1, new HashSet<string>(StringComparer.OrdinalIgnoreCase), null, null, null);
            Assert.Equal(triggered, fresh.TriggeredEventIds.Count);
        }

        [Fact]
        public void WorldEvolution_RequiredFlagGateBlocksUnownedEvent()
        {
            var engine = NewEvolutionEngine();

            // Find the first event that declares a required flag, then tick far past
            // its trigger day with an EMPTY flag set. A missing flag is a closed gate.
            WorldEvolutionEventDef? gated = null;
            foreach (var evt in engine.Events)
            {
                if (!string.IsNullOrEmpty(evt.required_flag)) { gated = evt; break; }
            }

            if (gated == null) return; // No flag-gated event authored — nothing to assert.

            int day = Math.Max(gated.trigger_day + 1, 1);
            engine.TickDay(day, new HashSet<string>(StringComparer.OrdinalIgnoreCase), null, null, null);
            Assert.DoesNotContain(gated.id, engine.TriggeredEventIds);

            // Opening the authored flag fires it.
            engine.TickDay(day + 1, new HashSet<string>(StringComparer.OrdinalIgnoreCase) { gated.required_flag! }, null, null, null);
            Assert.Contains(gated.id, engine.TriggeredEventIds);
        }

    }
}
