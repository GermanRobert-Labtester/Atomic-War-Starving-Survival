// SPDX-License-Identifier: MIT
// Plan 60 / D4 — a vector protocol is maintenance, not a switch you flip once.
// The contract pinned here: an armed protocol lapses on its authored day, the lapse
// is announced exactly once, survives a save round-trip, and never consumes RNG.
// A protocol that can only ever be switched on would make outbreak prevention an
// achievement instead of a chore.
using System;
using Ashfall.Core;
using Ashfall.Core.Disease;
using Xunit;

namespace Ashfall.Core.Tests.Medical
{
    public class DiseaseProtocolExpiryTests
    {
        private const string Water = DiseaseVectorNames.Water;
        private const string Air = DiseaseVectorNames.Air;
        private const string Blood = DiseaseVectorNames.Blood;
        private const string Spore = DiseaseVectorNames.Spore;

        /// <summary>Fresh system bound to a fixture catalog; 0 = no window.</summary>
        private static (DiseaseSystem engine, DiseaseCatalog catalog) MakeSystem(
            int water = 3, int air = 2, int blood = 5, int spore = 4)
        {
            var catalog = new DiseaseCatalog();
            catalog.VectorProtocols.Add(new VectorProtocolFile { vector = Water, duration_days = water });
            catalog.VectorProtocols.Add(new VectorProtocolFile { vector = Air, duration_days = air });
            catalog.VectorProtocols.Add(new VectorProtocolFile { vector = Blood, duration_days = blood });
            catalog.VectorProtocols.Add(new VectorProtocolFile { vector = Spore, duration_days = spore });
            var system = new DiseaseSystem(rng: new SeededRng(7));
            system.BindCatalog(catalog);
            return (system, catalog);
        }

        private static void Apply(DiseaseSystem system, string vector, int day)
        {
            switch (DiseaseVectorNames.Parse(vector))
            {
                case DiseaseVector.Water: system.PurifyWater(day); break;
                case DiseaseVector.Air: system.SealVents(day); break;
                case DiseaseVector.Blood: system.SterilizeTools(day); break;
                case DiseaseVector.Spore: system.SetAirFiltration(true, day); break;
            }
        }

        [Fact]
        public void Protocol_LapsesExactlyOnItsAuthoredDay()
        {
            var (system, _) = MakeSystem();   // water duration: 3
            system.PurifyWater(100);          // holds until day 103

            Assert.True(system.IsVectorBlocked(Water));
            system.TickDaily(102);
            Assert.True(system.IsVectorBlocked(Water));
            system.TickDaily(103);
            Assert.False(system.IsVectorBlocked(Water));
            Assert.Equal(0, system.CaptureState().water_purified_until_day);
        }

        [Fact]
        public void EveryVectorBlock_CanReturnToFalse_OnTheDayTickAlone()
        {
            var scenarios = new[]
            {
                (Water, MakeSystem(water: 3).Item1),
                (Air, MakeSystem(air: 2).Item1),
                (Blood, MakeSystem(blood: 5).Item1),
                (Spore, MakeSystem(spore: 4).Item1),
            };
            foreach (var (vector, system) in scenarios)
            {
                Apply(system, vector, day: 200);
                Assert.True(system.IsVectorBlocked(vector));

                for (int d = 201; d <= 206; d++)
                    system.TickDaily(d, Array.Empty<string>());

                Assert.False(system.IsVectorBlocked(vector));
            }
        }

        [Fact]
        public void Lapse_RaisesTheResetEvent_ExactlyOnce()
        {
            var (system, _) = MakeSystem();
            int resets = 0;
            system.OnEventRaised += (eventId, _) =>
            {
                if (eventId == DiseaseIds.EventProtocolReset) resets++;
            };

            system.SealVents(10);
            system.TickDaily(11);
            system.TickDaily(12);
            system.TickDaily(13);   // lapse
            int afterWindow = resets;
            system.TickDaily(14);   // nothing left to lapse

            Assert.Equal(afterWindow, resets);
            Assert.Equal(1, afterWindow);
        }

        [Fact]
        public void ExpiryDay_SurvivesTheSaveRoundTrip()
        {
            var (system, catalog) = MakeSystem();
            system.PurifyWater(100);
            var saved = system.CaptureState();
            Assert.Equal(103, saved.water_purified_until_day);

            var restored = new DiseaseSystem(saved, rng: new SeededRng(saved.rngSeed));
            restored.BindCatalog(catalog);
            restored.TickDaily(102);
            Assert.True(restored.IsVectorBlocked(Water));
            restored.TickDaily(103);
            Assert.False(restored.IsVectorBlocked(Water));
        }

        [Fact]
        public void PreD4Save_WithBareProtocol_ReArmsFromTheCurrentDay()
        {
            var (system, _) = MakeSystem();
            system.RestoreState(new DiseaseSystemState { water_purified = true, water_purified_until_day = 0 });

            system.TickProtocolExpiry(50);   // bare flag arms from the tick day
            Assert.Equal(53, system.CaptureState().water_purified_until_day);

            system.TickProtocolExpiry(52);
            Assert.True(system.IsVectorBlocked(Water));
            system.TickProtocolExpiry(53);
            Assert.False(system.IsVectorBlocked(Water));
        }

        [Fact]
        public void Lapse_IsPureDayArithmetic_NeverConsumesRng()
        {
            var (system, _) = MakeSystem();
            system.PurifyWater(10);
            long seedBefore = system.CaptureState().rngSeed;

            system.TickDaily(13);
            Assert.False(system.IsVectorBlocked(Water));
            Assert.Equal(seedBefore, system.CaptureState().rngSeed);
        }

        [Fact]
        public void UnauthoredDuration_KeepsTheLegacyPermanentBehaviour()
        {
            var (system, _) = MakeSystem(water: 0);   // no authored window
            system.PurifyWater(100);

            for (int d = 101; d <= 130; d++) system.TickProtocolExpiry(d);
            Assert.True(system.IsVectorBlocked(Water));
            Assert.Equal(int.MaxValue, system.ProtocolDaysRemaining(Water, 155));

            system.SetAirFiltration(false);
            Assert.False(system.IsVectorBlocked(Spore));
        }
    }
}
