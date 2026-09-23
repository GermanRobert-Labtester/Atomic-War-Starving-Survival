// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.Memorial;
using Ashfall.Core.Muster;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class Plan84_69MusterMemorialIntegrationTests : CatalogTestBase
    {
        [Fact]
        public void MusterAndEpitaphCatalogs_LoadCleanly_WithoutSchemaDrift()
        {
            // Plan 84: 27 Muster Witnesses across 4 threads and factions
            var witnesses = WitnessCatalogLoader.LoadWitnesses(
                DataDirectory, new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.NotNull(witnesses);
            Assert.Equal(27, witnesses.Count);

            // Plan 69: 30 Wasteland Grave Epitaphs
            var epitaphCatalog = GraveEpitaphCatalog.LoadFromDataDir(
                DataDirectory, new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.NotNull(epitaphCatalog);
            Assert.Equal(30, epitaphCatalog.TotalCount);

            // Ensure baseline and expanded causes exist
            var causes = epitaphCatalog.AllEntries.Select(e => e.cause).Distinct().ToList();
            Assert.Contains("combat", causes);
            Assert.Contains("execution", causes);
            Assert.Contains("drowning", causes);
            Assert.Contains("frostbite", causes);
            Assert.Contains("starvation", causes);
            Assert.Contains("radiation", causes);
        }

        [Fact]
        public void GraveEpitaphCatalog_DeterministicSelectionAndFallback()
        {
            var epitaphCatalog = GraveEpitaphCatalog.LoadFromDataDir(
                DataDirectory, new FileSystemIO(), new SystemTextJsonSerializer());

            // Deterministic selection with identical seed
            string ep1 = epitaphCatalog.SelectEpitaph("combat", new SeededRng(42));
            string ep2 = epitaphCatalog.SelectEpitaph("combat", new SeededRng(42));
            Assert.Equal(ep1, ep2);
            Assert.False(string.IsNullOrWhiteSpace(ep1));

            // Unknown cause fallback
            string fallback = epitaphCatalog.SelectEpitaph("non_existent_cause", new SeededRng(100));
            Assert.False(string.IsNullOrWhiteSpace(fallback));

            // Pool counts by cause
            var combatPool = epitaphCatalog.GetEpitaphsForCause("combat");
            Assert.True(combatPool.Count >= 2);
            var executionPool = epitaphCatalog.GetEpitaphsForCause("execution");
            Assert.Single(executionPool);
            Assert.Equal("They brought him out to the gravel pit at sunrise and read no charges from the ledger.", executionPool[0].epitaph);
        }

        [Fact]
        public void MemorialSystem_EpitaphIntegration_AutoPopulatesFromCause()
        {
            var epitaphCatalog = GraveEpitaphCatalog.LoadFromDataDir(
                DataDirectory, new FileSystemIO(), new SystemTextJsonSerializer());

            var state = new MemorialState();
            var memorialSystem = new MemorialSystem(state)
            {
                EpitaphCatalog = epitaphCatalog,
                EpitaphRng = new SeededRng(999)
            };

            // Memorialize survivor with empty epitaph
            var entry = memorialSystem.Memorialize(new MemorialInput
            {
                SurvivorId = "survivor_checkpoint_guard",
                Cause = "execution",
                Day = 245,
                BirthDay = 200,
                Epitaph = string.Empty, // Empty, triggers auto-population
                MoraleDelta = -6f,
                DeathQuality = DeathQuality.Unattended,
                Outcome = MemorialOutcome.Burial
            });

            Assert.NotNull(entry);
            Assert.Equal("They brought him out to the gravel pit at sunrise and read no charges from the ledger.", entry.Epitaph);
            Assert.Equal(DeathQuality.Unattended, entry.DeathQuality);

            // Capture and restore
            var captured = memorialSystem.CaptureState();
            var restoredState = new MemorialState();
            var restoredSystem = new MemorialSystem(restoredState);
            restoredSystem.RestoreState(captured);

            Assert.Single(restoredSystem.Entries);
            Assert.Equal(entry.Epitaph, restoredSystem.Entries[0].Epitaph);
            Assert.Equal("execution", restoredSystem.Entries[0].Cause);
        }

        [Fact]
        public void MusterWitnessTragedyToGraveMemorial_CrossSystemLinkage()
        {
            var witnesses = WitnessCatalogLoader.LoadWitnesses(
                DataDirectory, new FileSystemIO(), new SystemTextJsonSerializer());
            var epitaphCatalog = GraveEpitaphCatalog.LoadFromDataDir(
                DataDirectory, new FileSystemIO(), new SystemTextJsonSerializer());

            var memorialSystem = new MemorialSystem(new MemorialState())
            {
                EpitaphCatalog = epitaphCatalog,
                EpitaphRng = new SeededRng(54321)
            };

            // 1. Grain Convoy massacre witness (Tomas) recounts ambush casualties
            var tomas = witnesses.FirstOrDefault(w => w.id == "witness_convoy_driver_tomas");
            Assert.NotNull(tomas);
            Assert.Contains("seed rye", tomas.body);
            Assert.Contains("culvert", tomas.body);

            // Memorialize a casualty from that convoy ambush
            var convoyVictim = memorialSystem.Memorialize(new MemorialInput
            {
                SurvivorId = "dweller_convoy_casualty",
                Cause = "combat",
                Day = tomas.dayMin,
                BirthDay = 210,
                MoraleDelta = -8f,
                DeathQuality = DeathQuality.Rushed
            });
            Assert.False(string.IsNullOrWhiteSpace(convoyVictim.Epitaph));
            var combatPool = epitaphCatalog.GetEpitaphsForCause("combat").Select(e => e.epitaph).ToHashSet();
            Assert.Contains(convoyVictim.Epitaph, combatPool);

            // 2. Coastal Evacuation witness (Captain Maren) recounts drownings in freezing surf
            var maren = witnesses.FirstOrDefault(w => w.id == "witness_trawler_captain_maren");
            Assert.NotNull(maren);
            Assert.Contains("lifeboats", maren.body);

            // Memorialize a passenger lost in the surf
            var seaVictim = memorialSystem.Memorialize(new MemorialInput
            {
                SurvivorId = "dweller_coastal_drowned",
                Cause = "drowning",
                Day = maren.dayMin,
                BirthDay = 215,
                MoraleDelta = -7f,
                DeathQuality = DeathQuality.Unattended
            });
            Assert.Equal("The pontoon rope snapped in the spring current before anyone on the bank could throw another.", seaVictim.Epitaph);

            // 3. Nurse witness recounts infection during winter triage
            var nurse = witnesses.FirstOrDefault(w => w.id == "witness_coastal_refugee_nurse");
            Assert.NotNull(nurse);
            Assert.Contains("penicillin", nurse.body);

            var infectionVictim = memorialSystem.Memorialize(new MemorialInput
            {
                SurvivorId = "dweller_infection_casualty",
                Cause = "infection",
                Day = nurse.dayMin,
                BirthDay = 220,
                MoraleDelta = -5f,
                DeathQuality = DeathQuality.Peaceful
            });
            Assert.Equal("We boiled the needle three times, but the red line crept past his elbow anyway.", infectionVictim.Epitaph);

            // Mourn the sea victim in the shelter
            var mournRes = memorialSystem.Mourn("dweller_coastal_drowned", nurse.dayMin + 2);
            Assert.Equal(ActionResult.StatusKind.Success, mournRes.Status);
            Assert.Equal(nurse.dayMin + 2, seaVictim.MournedDay);

            // Verify all three memorial entries persist together
            Assert.Equal(3, memorialSystem.Entries.Count);
        }
    }
}
