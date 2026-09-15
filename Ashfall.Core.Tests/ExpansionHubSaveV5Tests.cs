// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Muster;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Hub save v5: the debt-consequence integration sections (dispatcher
    /// fired-set, embargo ledger, labor obligations) round-trip through the
    /// checksummed envelope, and pre-v5 saves migrate forward with empty —
    /// never re-applied — debt state.
    /// </summary>
    public sealed class ExpansionHubSaveV5Tests
    {
        private sealed class HubSystems
        {
            public WaystationSystem Waystation = new();
            public LocationLayoutSystem Layouts = new(new FileSystemIO(), new SystemTextJsonSerializer(), NullLog.Instance);
            public LocationMemorySystem Memory = new(new FileSystemIO(), new SystemTextJsonSerializer(), NullLog.Instance);
            public SiteEncounterSystem SiteEncounters = new(1117);
            public VouchAccessSystem Vouch = new();
            public GreenhouseSystem Greenhouse = new(1117);
            public LedgerDebtSystem Ledger = new();
        }

        private static HubSystems NewSystems() => new();

        [Fact]
        public void DebtSections_RoundTripThroughTheEnvelope()
        {
            var a = NewSystems();
            a.Ledger = new LedgerDebtSystem();

            // A fired consequence, an active embargo, a live labor obligation.
            var catalog = new DebtTemplateCatalog();
            catalog.Templates.Add(new DebtTemplate
            {
                id = "debt_test", creditorId = "faction_supply_corps", principalItemId = "canned_food",
                principalQuantity = 1, termDays = 5, rate = 0.1f, forfeitDescription = "one tin",
                consequenceId = "conseq_test", displayName = "t", description = "t"
            });
            catalog.Consequences.Add(new DebtConsequence
            {
                id = "conseq_test", trigger = "default", effectType = "labor_obligation", laborDays = 7
            });
            var dispatcher = new DebtConsequenceDispatcher(a.Ledger, catalog);
            dispatcher.SetDayProvider(() => 30);
            var bridge = new DebtConsequenceHostBridge(
                dispatcher, new Ashfall.Core.YearOfAsh.FactionWarSystem(), new FactionEmbargoLedger(),
                () => 30, NullLog.Instance, selectLaborSurvivor: () => "npc_ivo_fenn");
            var embargoes = new FactionEmbargoLedger();
            embargoes.TryAddEmbargo("faction_hydro_barons", "creditor_faction", 28, 14, "src-test");

            a.Ledger.PresentContract("npc_wyn_sabler", 1f, 5, 0.1f, "one tin", "faction_supply_corps", "debt_test");
            a.Ledger.PresentContract("npc_wyn_sabler", 1f, 5, 0.1f, "one tin", "faction_supply_corps", "debt_test");
            a.Ledger.SignContract("npc_wyn_sabler", 20);
            for (int d = 0; d < 5; d++)
                a.Ledger.TickDaily(21 + d); // forfeit → labor consequence fires

            var save = ExpansionHubSaveCodec.Capture(
                30, a.Waystation, a.Layouts, a.Memory, a.SiteEncounters, a.Vouch, a.Greenhouse,
                ledger: a.Ledger,
                debtDispatcher: dispatcher, embargoes: embargoes, debtBridge: bridge.CaptureState());

            var jsonSer = new SystemTextJsonSerializer();
            var loaded = ExpansionHubSaveCodec.Decode(ExpansionHubSaveCodec.Encode(save, jsonSer), jsonSer);
            Assert.Equal(ExpansionHubSave.CurrentSaveVersion, loaded.saveVersion);

            var b = NewSystems();
            b.Ledger = new LedgerDebtSystem();
            var dispatcher2 = new DebtConsequenceDispatcher(b.Ledger, catalog);
            var embargoes2 = new FactionEmbargoLedger();
            var bridge2 = new DebtConsequenceHostBridge(
                dispatcher2, new Ashfall.Core.YearOfAsh.FactionWarSystem(), embargoes2, () => 30, NullLog.Instance);
            ExpansionHubSaveCodec.Restore(
                loaded, b.Waystation, b.Layouts, b.Memory, b.SiteEncounters, b.Vouch, b.Greenhouse,
                ledger: b.Ledger,
                debtDispatcher: dispatcher2, embargoes: embargoes2, debtBridge: bridge2);

            Assert.True(embargoes2.IsEmbargoed("faction_hydro_barons", 30)); // embargo survived
            Assert.Single(bridge2.LaborObligations); // obligation survived
            Assert.True(dispatcher2.HasFired(
                DebtConsequenceDispatcher.ConsequenceIdentity(b.Ledger.GetContract("npc_wyn_sabler")!, "conseq_test")));

            // The restored fired-set still suppresses re-dispatch.
            int redispatched = 0;
            dispatcher2.OnConsequenceDispatched += (_, _) => redispatched++;
            for (int d = 0; d < 20; d++)
                b.Ledger.TickDaily(40 + d);
            Assert.Equal(0, redispatched);
        }

        [Fact]
        public void V4Save_MigratesForward_WithEmptyDebtState()
        {
            var jsonSer = new SystemTextJsonSerializer();
            var v4 = new ExpansionHubSaveV4 { simDay = 12 };
            v4.Checksum = SaveChecksum.Compute(v4);
            var text = jsonSer.Serialize(v4);

            var loaded = ExpansionHubSaveCodec.Decode(text, jsonSer);
            Assert.Equal(ExpansionHubSave.CurrentSaveVersion, loaded.saveVersion);
            Assert.Equal(12, loaded.simDay);
            Assert.NotNull(loaded.debtDispatcher);
            Assert.Empty(loaded.debtDispatcher.firedConsequences);
            Assert.NotNull(loaded.embargoes);
            Assert.Empty(loaded.embargoes.embargoes);
            Assert.NotNull(loaded.debtBridge);
            Assert.Empty(loaded.debtBridge.laborObligations);
            Assert.NotNull(loaded.saltMine);
            Assert.Empty(loaded.saltMine.veins);
        }

        [Fact]
        public void V5Save_MigratesForward_WithEmptySaltMine()
        {
            var jsonSer = new SystemTextJsonSerializer();
            var v5 = new ExpansionHubSaveV5 { simDay = 44 };
            v5.Checksum = SaveChecksum.Compute(v5);
            var text = jsonSer.Serialize(v5);

            var loaded = ExpansionHubSaveCodec.Decode(text, jsonSer);
            Assert.Equal(ExpansionHubSave.CurrentSaveVersion, loaded.saveVersion);
            Assert.Equal(44, loaded.simDay);
            Assert.NotNull(loaded.saltMine);
            Assert.Empty(loaded.saltMine.veins);
            Assert.Equal(0f, loaded.saltMine.saltStorage);
        }

        [Fact]
        public void SaltMine_RoundTripsThroughTheEnvelope()
        {
            var a = NewSystems();
            var salt = new Ashfall.Core.Foundry.SaltMineExtractionSystem();
            salt.RegisterVein(new Ashfall.Core.Foundry.SaltMineVeinState
            {
                veinId = "vein_test_halite",
                displayName = "Test Halite",
                isUnlocked = true,
                remainingOre = 500f,
                extractionRate = 10f,
                maxWorkers = 2,
                assignedWorkers = 2,
                drillCondition = 0.8f,
                pumpPressure = 0.9f
            });
            for (int d = 1; d <= 3; d++)
                salt.TickDaily(d, new SeededRng(77));

            var save = ExpansionHubSaveCodec.Capture(
                50, a.Waystation, a.Layouts, a.Memory, a.SiteEncounters, a.Vouch, a.Greenhouse,
                saltMine: salt);
            Assert.Equal(ExpansionHubSave.CurrentSaveVersion, save.saveVersion);
            Assert.True(save.saltMine.saltStorage > 0f || save.saltMine.brineStorage > 0f
                || save.saltMine.veins.Count > 0);

            var jsonSer = new SystemTextJsonSerializer();
            var loaded = ExpansionHubSaveCodec.Decode(ExpansionHubSaveCodec.Encode(save, jsonSer), jsonSer);
            var salt2 = new Ashfall.Core.Foundry.SaltMineExtractionSystem();
            ExpansionHubSaveCodec.Restore(
                loaded, a.Waystation, a.Layouts, a.Memory, a.SiteEncounters, a.Vouch, a.Greenhouse,
                saltMine: salt2);

            Assert.Equal(save.saltMine.saltStorage, salt2.State.saltStorage);
            Assert.Equal(save.saltMine.brineStorage, salt2.State.brineStorage);
            Assert.Single(salt2.State.veins);
            Assert.Equal("vein_test_halite", salt2.State.veins[0].veinId);
            Assert.Equal(2, salt2.State.veins[0].assignedWorkers);
        }
    }
}
