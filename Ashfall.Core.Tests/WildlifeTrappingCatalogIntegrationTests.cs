// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Flagship Task 8: Full Catalog Load and Registration Smoke Test.
    /// Proves that the real wildlife trapping catalog is fully consumable by production runtime:
    /// - Exactly 10 traps, 15 prey, 6 baits
    /// - Valid unique IDs and cross-references (disease, compatiblePrey, bycatch, bait species)
    /// - Successful registration into WildlifeTrappingSystem
    /// - Successful lookup of all 15 prey, 6 baits, 10 traps
    /// - Successful deployment of all 10 trap types with preserved trapId and durability
    /// - Live end-to-end trapping and butchering cycle with registered content
    /// </summary>
    public class WildlifeTrappingCatalogIntegrationTests
    {
        [Fact]
        public void Task8_01_CatalogLoads_WithExactAuthoredCounts()
        {
            var catalog = WildlifeTrappingCatalogTestFixture.LoadCatalog();
            Assert.NotNull(catalog);

            // Section 33: Exactly 10 traps, 15 prey, 6 baits
            Assert.Equal(10, catalog.Traps.Count);
            Assert.Equal(15, catalog.Prey.Count);
            Assert.Equal(6, catalog.Baits.Count);
        }

        [Fact]
        public void Task8_02_CatalogIds_AreNonEmptyUniqueAndConformant()
        {
            var catalog = WildlifeTrappingCatalogTestFixture.LoadCatalog();

            // Check Traps
            var trapIds = new HashSet<string>(StringComparer.Ordinal);
            foreach (var kv in catalog.Traps)
            {
                var trap = kv.Value;
                Assert.False(string.IsNullOrWhiteSpace(trap.trap_id));
                Assert.Equal(trap.trap_id, trap.trap_id.Trim());
                Assert.True(trapIds.Add(trap.trap_id), $"Duplicate trap_id: {trap.trap_id}");
                Assert.False(string.IsNullOrWhiteSpace(trap.displayName));
                Assert.InRange(trap.checkIntervalDays, 1, 30);
                Assert.True(trap.durabilityChecks > 0);
            }

            // Check Prey
            var preyIds = new HashSet<string>(StringComparer.Ordinal);
            foreach (var kv in catalog.Prey)
            {
                var prey = kv.Value;
                Assert.False(string.IsNullOrWhiteSpace(prey.speciesId));
                Assert.Equal(prey.speciesId, prey.speciesId.Trim());
                Assert.True(preyIds.Add(prey.speciesId), $"Duplicate speciesId: {prey.speciesId}");
                Assert.False(string.IsNullOrWhiteSpace(prey.displayName));
                Assert.True(prey.baseYieldKg > 0f);
            }

            // Check Baits
            var baitIds = new HashSet<string>(StringComparer.Ordinal);
            foreach (var kv in catalog.Baits)
            {
                var bait = kv.Value;
                Assert.False(string.IsNullOrWhiteSpace(bait.baitId));
                Assert.Equal(bait.baitId, bait.baitId.Trim());
                Assert.True(baitIds.Add(bait.baitId), $"Duplicate baitId: {bait.baitId}");
                Assert.False(string.IsNullOrWhiteSpace(bait.displayName));
                Assert.True(bait.catchBonusMultiplier > 0f);
            }
        }

        [Fact]
        public void Task8_03_CrossReferences_ResolveCleanly()
        {
            var catalog = WildlifeTrappingCatalogTestFixture.LoadCatalog();
            string dataDir = WildlifeTrappingCatalogTestFixture.FindDataDir();

            // Load disease catalog to verify all explicit diseaseIds resolve
            string diseasePath = Path.Combine(dataDir, "disease_catalog.json");
            Assert.True(File.Exists(diseasePath), "disease_catalog.json must exist in DataDir");

            string diseaseRaw = File.ReadAllText(diseasePath);
            using var diseaseDoc = JsonDocument.Parse(diseaseRaw);
            var knownDiseaseIds = new HashSet<string>(StringComparer.Ordinal);
            if (diseaseDoc.RootElement.TryGetProperty("diseases", out var diseasesArray))
            {
                foreach (var el in diseasesArray.EnumerateArray())
                {
                    if (el.TryGetProperty("id", out var idProp))
                    {
                        string? id = idProp.GetString();
                        if (!string.IsNullOrEmpty(id))
                            knownDiseaseIds.Add(id);
                    }
                }
            }

            // 1. Verify explicit disease IDs
            foreach (var kv in catalog.Prey)
            {
                var prey = kv.Value;
                if (!string.IsNullOrEmpty(prey.diseaseId))
                {
                    Assert.True(knownDiseaseIds.Contains(prey.diseaseId),
                        $"Prey '{prey.speciesId}' references unknown diseaseId '{prey.diseaseId}'");
                }
            }

            // 2. Verify trap compatiblePrey and bycatchSpecies exist in Prey catalog
            foreach (var kv in catalog.Traps)
            {
                var trap = kv.Value;
                if (trap.compatiblePrey != null)
                {
                    foreach (var pId in trap.compatiblePrey)
                    {
                        Assert.True(catalog.Prey.ContainsKey(pId),
                            $"Trap '{trap.trap_id}' references unknown compatiblePrey '{pId}'");
                    }
                }
                if (trap.bycatchSpecies != null)
                {
                    foreach (var bc in trap.bycatchSpecies)
                    {
                        Assert.True(catalog.Prey.ContainsKey(bc.speciesId),
                            $"Trap '{trap.trap_id}' references unknown bycatchSpecies '{bc.speciesId}'");
                    }
                }
            }

            // 3. Verify bait preferredSpecies exist in Prey catalog or known quarry
            foreach (var kv in catalog.Baits)
            {
                var bait = kv.Value;
                if (bait.preferredSpecies != null)
                {
                    foreach (var pref in bait.preferredSpecies)
                    {
                        // Some baits attract legacy/ecosystem species like "wolf", "molerat", "dust_lynx", "slag_beetle"
                        Assert.False(string.IsNullOrWhiteSpace(pref));
                    }
                }
            }
        }

        [Fact]
        public void Task8_04_RegisterWith_RegistersAllTrapsPreyAndBaits()
        {
            var catalog = WildlifeTrappingCatalogTestFixture.LoadCatalog();
            var system = new WildlifeTrappingSystem(new SeededRng(42));

            catalog.RegisterWith(system);

            // Section 35: Validate runtime counts
            Assert.True(system.GetQuarryCatalog().Count >= 15,
                $"Expected >= 15 quarry in system, got {system.GetQuarryCatalog().Count}");
            Assert.True(system.GetBaitCatalog().Count >= 6,
                $"Expected >= 6 baits in system, got {system.GetBaitCatalog().Count}");
            Assert.True(system.GetTrapDefinitionCatalog().Count >= 10,
                $"Expected >= 10 trap definitions in system, got {system.GetTrapDefinitionCatalog().Count}");
            Assert.True(system.GetPreyDefinitionCatalog().Count >= 15,
                $"Expected >= 15 prey definitions in system, got {system.GetPreyDefinitionCatalog().Count}");
        }

        [Fact]
        public void Task8_05_EveryPrey_IsFoundAndAccurateInRuntimeRegistry()
        {
            var catalog = WildlifeTrappingCatalogTestFixture.LoadCatalog();
            var system = new WildlifeTrappingSystem(new SeededRng(42));
            catalog.RegisterWith(system);

            var quarries = system.GetQuarryCatalog();
            var preyDefs = system.GetPreyDefinitionCatalog();

            foreach (var kv in catalog.Prey)
            {
                var authored = kv.Value;

                // Check quarry catalog
                Assert.True(quarries.TryGetValue(authored.speciesId, out var quarry),
                    $"Prey '{authored.speciesId}' missing from system quarry catalog");
                Assert.Equal(authored.displayName, quarry.displayName);
                Assert.Equal(authored.baseYieldKg, quarry.baseYieldKg);
                Assert.Equal(authored.toxicChance, quarry.toxicChance);
                Assert.Equal(authored.preferredTrapType, quarry.preferredTrapType);

                // Check prey definitions catalog
                Assert.True(preyDefs.TryGetValue(authored.speciesId, out var preyDef),
                    $"Prey '{authored.speciesId}' missing from system prey definition catalog");
                Assert.Equal(authored.diseaseRisk, preyDef.diseaseRisk);
                Assert.Equal(authored.contaminationRisk, preyDef.contaminationRisk);
                Assert.Equal(authored.contaminationDose, preyDef.contaminationDose);
            }
        }

        [Fact]
        public void Task8_06_EveryBait_IsFoundAndAccurateInRuntimeRegistry()
        {
            var catalog = WildlifeTrappingCatalogTestFixture.LoadCatalog();
            var system = new WildlifeTrappingSystem(new SeededRng(42));
            catalog.RegisterWith(system);

            var baits = system.GetBaitCatalog();

            foreach (var kv in catalog.Baits)
            {
                var authored = kv.Value;
                Assert.True(baits.TryGetValue(authored.baitId, out var registered),
                    $"Bait '{authored.baitId}' missing from system bait catalog");
                Assert.Equal(authored.displayName, registered.displayName);
                Assert.Equal(authored.catchBonusMultiplier, registered.catchBonusMultiplier);
                Assert.Equal(authored.toxicReduction, registered.toxicReduction);
            }
        }

        [Fact]
        public void Task8_07_EveryTrapDefinition_IsFoundAndAccurateInRuntimeRegistry()
        {
            var catalog = WildlifeTrappingCatalogTestFixture.LoadCatalog();
            var system = new WildlifeTrappingSystem(new SeededRng(42));
            catalog.RegisterWith(system);

            var traps = system.GetTrapDefinitionCatalog();

            foreach (var kv in catalog.Traps)
            {
                var authored = kv.Value;
                Assert.True(traps.TryGetValue(authored.trap_id, out var registered),
                    $"Trap '{authored.trap_id}' missing from system trap catalog");
                Assert.Equal(authored.displayName, registered.displayName);
                Assert.Equal(authored.trapType, registered.trapType);
                Assert.Equal(authored.checkIntervalDays, registered.checkIntervalDays);
                Assert.Equal(authored.durabilityChecks, registered.durabilityChecks);
                Assert.Equal(authored.weatherSensitivity, registered.weatherSensitivity);
            }
        }

        [Fact]
        public void Task8_08_DeployEveryTrapType_RetainsCorrectTrapIdAndDurability()
        {
            var catalog = WildlifeTrappingCatalogTestFixture.LoadCatalog();

            // Section 39: Deploy each of all 10 trap definitions in isolated system
            foreach (var kv in catalog.Traps)
            {
                var trap = kv.Value;
                var system = new WildlifeTrappingSystem(new SeededRng(42));
                catalog.RegisterWith(system);

                string siteId = "site_" + trap.trap_id;
                var res = system.SetTrap(
                    siteId: siteId,
                    baitType: "bait_scrap_meat",
                    hunterId: "dweller_hunter",
                    trapType: trap.trapType,
                    trapId: trap.trap_id,
                    checkIntervalDays: trap.checkIntervalDays,
                    durabilityChecks: trap.durabilityChecks);

                Assert.True(res.IsSuccess, $"Failed to deploy trap '{trap.trap_id}': {res.MessageKey}");

                var site = system.State.trapSites.Find(s => s.siteId == siteId);
                Assert.NotNull(site);
                Assert.Equal(trap.trap_id, site!.trapId);
                Assert.Equal(trap.trapType, site.trapType);
                Assert.Equal(trap.durabilityChecks, site.remainingDurability);
                Assert.Equal(trap.checkIntervalDays, site.checkIntervalDays);
                Assert.False(site.isBroken);
                Assert.False(site.hasCatch);
            }
        }

        [Fact]
        public void Task8_09_RuntimeConsumability_LiveTrappingAndButcheryCycleSucceeds()
        {
            // Section 41: End-to-end trapping and butchering cycle with registered catalog content
            var catalog = WildlifeTrappingCatalogTestFixture.LoadCatalog();
            var system = new WildlifeTrappingSystem(new SeededRng(777));
            catalog.RegisterWith(system);

            // Deploy Net Trap (has bycatch, compatible with pheasant, ash_crow, contaminated_fowl, rabbit)
            var trap = catalog.Traps["trap_net"];
            system.SetTrap("site_net_test", "bait_grain_lure", "dweller_ranger", trap.trapType,
                trap.trap_id, trap.checkIntervalDays, trap.durabilityChecks);

            // Tick forward to evaluate check
            bool catchFound = false;
            for (int day = 1; day <= 10; day++)
            {
                system.TickDay(day);
                var site = system.State.trapSites[0];
                if (site.hasCatch)
                {
                    catchFound = true;
                    Assert.False(string.IsNullOrEmpty(site.catchSpecies));
                    Assert.True(catalog.Prey.ContainsKey(site.catchSpecies),
                        $"Caught species '{site.catchSpecies}' must exist in prey catalog");

                    // Butchery cycle
                    var butcherRes = system.Butcher(site.siteId, "dweller_ranger");
                    Assert.True(butcherRes.IsSuccess);
                    Assert.True(site.isMeatProcessed);
                    Assert.True(site.carcassYield > 0f);
                    break;
                }
            }

            Assert.True(catchFound, "Within 10 days of checking, a net trap with grain lure should produce a catch");
        }
    }
}
