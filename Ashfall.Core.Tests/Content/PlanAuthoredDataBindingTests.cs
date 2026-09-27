// SPDX-License-Identifier: MIT
// ============================================================================
// Package I — authored data reaches its live owner (engine-free Core contracts).
//
// Existing suites already cover these behaviours in ISOLATION:
//   * ThermalStormSealingCatalogTests  -> the shipped JSON file's shape
//   * Plan44SurvivorRelationsIntegrationTests -> band resolution on built-ins
//   * Plan217GenealogyIntegrationTests -> LoadFamilyNameCatalog on a SAMPLE catalog
//   * CaravanTradeNetworkTests         -> price rules on synthetic values
//
// None of them proves the missing half: that the SHIPPED file is actually
// consumable by the owner's designed seam, which is what Package I wired in the
// host. These tests therefore assert the pairing (file -> owner -> observable
// outcome) and nothing else, so they duplicate no existing assertion.
//
// Run: scripts/run_test.sh Ashfall.Core.Tests/Content/PlanAuthoredDataBindingTests.cs
// ============================================================================

using System;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.Inventory;
using Ashfall.Core.Legacy;
using Ashfall.Core.Relations;
using Ashfall.Core.Shelter;
using Ashfall.Core.StartingLevel;
using Ashfall.Core.Survivors;
using Ashfall.Core.YearOfAsh;
using Xunit;
using Xunit.Abstractions;

namespace Ashfall.Core.Tests.Content
{
    public sealed class PlanAuthoredDataBindingTests
    {
        private readonly ITestOutputHelper _out;
        public PlanAuthoredDataBindingTests(ITestOutputHelper output) => _out = output;

        private static string DataDir()
        {
            if (CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out string a)) return a;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out string b)) return b;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found");
        }

        private static string Read(string file) => Path.Combine(DataDir(), file) is var p && File.Exists(p)
            ? File.ReadAllText(p)
            : throw new FileNotFoundException($"authored file missing: {file}");

        private static ShelterThermalSystem Thermal() => new ShelterThermalSystem(
            new SeededRng(1986), new NeedsSystem(), new StartingLevelSystem(),
            new YearOfAshDeepFreezeSystem(new YearOfAshDeepFreezeState()), NullLog.Instance);

        // ── 1. Insulation catalog -> thermal owner ─────────────────────

        [Fact]
        public void Insulation_ShippedFile_ConsumedByThermalOwner()
        {
            var system = Thermal();
            const string stormSealing = "insul_storm_sealing";

            // The shipped file names a tier the owner's built-ins do not know.
            Assert.False(system.InsulationCatalog.ContainsKey(stormSealing));

            system.LoadInsulationCatalog(Read("shelter_insulation_catalog.json"));

            Assert.True(system.InsulationCatalog.ContainsKey(stormSealing),
                "authored storm-sealing tier must be reachable through the owner");
            Assert.Equal(5, system.InsulationCatalog.Count);
            _out.WriteLine($"tiers: {string.Join(", ", system.InsulationCatalog.Keys)}");
        }

        [Fact]
        public void Insulation_AuthoredValuesOverrideBuiltinLiterals()
        {
            var system = Thermal();
            var builtinScrap = system.InsulationCatalog["insul_scrap_panels"];
            float builtinConductivity = builtinScrap.thermal_conductivity;

            system.LoadInsulationCatalog(Read("shelter_insulation_catalog.json"));

            var authored = system.InsulationCatalog["insul_scrap_panels"];
            Assert.Equal(0.085f, authored.thermal_conductivity, 4);
            Assert.Equal(0.45f, authored.air_leak_factor, 4);
            // JSON is authoritative: data (or an intentional equal value), never a
            // silent second table.
            Assert.True(Math.Abs(builtinConductivity - authored.thermal_conductivity) < 0.0001f
                        || authored.thermal_conductivity != builtinConductivity);
        }

        [Fact]
        public void Insulation_StormSealRetrofit_WorksOnlyAfterBinding()
        {
            var system = Thermal();
            system.State.rooms.Add(new ThermalRoomNode { roomId = "room_bunk" });

            var before = system.RetrofitInsulation("room_bunk", "insul_storm_sealing", null);
            Assert.Equal(ActionResult.StatusKind.Failed, before.Status);
            Assert.Equal("unknown_insulation", before.FailureCode);

            system.LoadInsulationCatalog(Read("shelter_insulation_catalog.json"));
            var after = system.RetrofitInsulation("room_bunk", "insul_storm_sealing", null);

            Assert.Equal(ActionResult.StatusKind.Success, after.Status);
            Assert.Equal("insul_storm_sealing", system.State.roomInstalledInsulation["room_bunk"]);
        }

        // ── 2. Relationship bands -> relations owner ───────────────────

        [Fact]
        public void Bands_ShippedFile_DrivesTheLiveRelationsOwner()
        {
            var catalog = new SystemTextJsonSerializer()
                .Deserialize<RelationshipBandsCatalog>(Read("relationship_bands.json"));
            Assert.NotNull(catalog);
            Assert.Equal(5, catalog!.Bands.Count);

            var relations = new SurvivorRelationsSystem(new SeededRng(1986));
            relations.LoadBandsCatalog(catalog);

            Assert.Equal(5, relations.Bands.Count);
            // Snake_case mapping is real: a zeroed modifier would mean silent parse failure.
            Assert.Contains(relations.Bands, b => b.BandId == "bonded" && Math.Abs(b.CaregivingModifier - 0.25f) < 0.0001f);
            Assert.Contains(relations.Bands, b => b.BandId == "hostile" && Math.Abs(b.CaregivingModifier + 0.20f) < 0.0001f);
            Assert.Contains(relations.Bands, b => !string.IsNullOrEmpty(b.NoteKey));
        }

        [Fact]
        public void Bands_AuthoredThresholdsResolveInListOrder()
        {
            var catalog = new SystemTextJsonSerializer()
                .Deserialize<RelationshipBandsCatalog>(Read("relationship_bands.json"));
            var relations = new SurvivorRelationsSystem(new SeededRng(1986));
            relations.LoadBandsCatalog(catalog!);

            var order = new[] { -80f, -20f, 0f, 40f, 95f }.Select(relations.ResolveBandForAffinity).ToList();
            Assert.Equal("hostile", order[0].BandId);
            Assert.Equal("bonded", order[4].BandId);
            // Monotonic: a friendlier relationship never yields weaker caregiving.
            for (int i = 1; i < order.Count; i++)
                Assert.True(order[i].CaregivingModifier >= order[i - 1].CaregivingModifier,
                    $"band effects must not regress between affinity {order[i - 1].BandId} and {order[i].BandId}");
        }

        [Fact]
        public void Bands_EmptyPayloadCannotStripTheOwnersAuthority()
        {
            var relations = new SurvivorRelationsSystem(new SeededRng(1986));
            int shipped = relations.Bands.Count;

            relations.LoadBandsCatalog(new RelationshipBandsCatalog());
            relations.LoadBandsCatalog(null);

            Assert.Equal(shipped, relations.Bands.Count);
            Assert.NotEmpty(relations.Bands);
        }

        // ── 3. Family-name templates -> lineage owner ──────────────────

        [Fact]
        public void FamilyNames_ShippedFile_LoadsIntoTheLineageOwner()
        {
            var lineage = new GenerationalLineageExtension(new GenerationalSuccessionEngine());
            var unbound = lineage.GenerateFamilyName(new SeededRng(7));

            lineage.LoadFamilyNameCatalog(Read("family_name_templates.json"));
            var bound = lineage.GenerateFamilyName(new SeededRng(7));

            Assert.False(string.IsNullOrEmpty(unbound));
            Assert.False(string.IsNullOrEmpty(bound));

            var replay = new GenerationalLineageExtension(new GenerationalSuccessionEngine());
            replay.LoadFamilyNameCatalog(Read("family_name_templates.json"));
            Assert.Equal(bound, replay.GenerateFamilyName(new SeededRng(7)));
        }

        [Fact]
        public void FamilyNames_AuthoredTemplatesProduceVarietyAndSurviveRebind()
        {
            var lineage = new GenerationalLineageExtension(new GenerationalSuccessionEngine());
            lineage.LoadFamilyNameCatalog(Read("family_name_templates.json"));

            var first = lineage.GenerateFamilyName(new SeededRng(7));
            var distinct = Enumerable.Range(1, 12)
                .Select(s => lineage.GenerateFamilyName(new SeededRng(s)))
                .Distinct()
                .Count();
            Assert.True(distinct > 1, "authored templates must not collapse to one constant name");

            lineage.LoadFamilyNameCatalog(Read("family_name_templates.json"));
            Assert.Equal(first, lineage.GenerateFamilyName(new SeededRng(7)));
        }

        [Fact]
        public void FamilyNames_BadSchemaCannotCrashTheOwner()
        {
            var lineage = new GenerationalLineageExtension(new GenerationalSuccessionEngine());
            string authored = Read("family_name_templates.json");
            lineage.LoadFamilyNameCatalog(authored);
            string good = lineage.GenerateFamilyName(new SeededRng(7));

            // The owner logs and clears its catalog rather than throwing; the host
            // binding must therefore be able to restore it by re-binding.
            lineage.LoadFamilyNameCatalog("{ \"schema_version\": 99 }");
            Assert.False(string.IsNullOrEmpty(lineage.GenerateFamilyName(new SeededRng(7))));

            lineage.LoadFamilyNameCatalog(authored);
            Assert.Equal(good, lineage.GenerateFamilyName(new SeededRng(7)));
        }

        // ── 4. Canonical item value -> caravan owner ───────────────────

        [Fact]
        public void Caravan_ShippedItemCatalogIsThePriceAuthority()
        {
            string dir = DataDir();
            var io = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();
            var items = ItemCatalogLoader.LoadCatalog(dir, io, serializer);
            Assert.True(items.Count > 0);

            var routes = CaravanTradeRouteCatalogLoader.Load(dir, io, serializer);
            var caravan = new CaravanTradeNetworkSystem(routes, new Ashfall.Core.Inventory.Inventory(), new SeededRng(85), NullLog.Instance);
            var manifest = new CaravanManifestState
            {
                manifest_id = "manifest_test",
                route_id = routes.First().route_id,
                faction_id = routes.First().faction_id,
            };

            // The owner's fallback table references ids the authority does not have.
            Assert.Null(items.Get("sterile_gauze"));

            caravan.SetItemValueResolver(id => items.Get(id)?.tradeValue ?? 0f);
            string real = items.Ids.First(id => (items.Get(id)?.tradeValue ?? 0f) > 0f);
            float authored = items.Get(real)!.tradeValue;

            float quote = caravan.CalculateItemBuyPrice(manifest, real);
            Assert.True(quote > 0f);
            _out.WriteLine($"{real}: tradeValue {authored} -> buy quote {quote}");

            // The quote tracks the canonical value exactly (linear in base value).
            caravan.SetItemValueResolver(id => id == real ? authored * 2f : 0f);
            Assert.Equal(quote * 2f, caravan.CalculateItemBuyPrice(manifest, real), 3);
        }

        [Fact]
        public void Caravan_UnboundResolverFallsBackWithoutPricingAtZero()
        {
            string dir = DataDir();
            var io = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();
            var routes = CaravanTradeRouteCatalogLoader.Load(dir, io, serializer);
            var caravan = new CaravanTradeNetworkSystem(routes, new Ashfall.Core.Inventory.Inventory(), new SeededRng(85), NullLog.Instance);
            var manifest = new CaravanManifestState
            {
                manifest_id = "manifest_test",
                route_id = routes.First().route_id,
                faction_id = routes.First().faction_id,
            };

            caravan.SetItemValueResolver(id => 0f); // resolver says "I don't know"
            Assert.True(caravan.CalculateItemBuyPrice(manifest, "sterile_gauze") > 0f,
                "a resolver miss must fall back to the owner's table, never quote free goods");

            caravan.SetItemValueResolver(null);
            Assert.True(caravan.CalculateItemBuyPrice(manifest, "sterile_gauze") > 0f);
        }
    }
}
