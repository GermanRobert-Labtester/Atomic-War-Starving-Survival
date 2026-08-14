using System;
using System.Collections.Generic;
using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Codec roundtrip and integrity tests for the expansion-hub save envelope
    /// (Ashfall.Core.ExpansionHubSave / ExpansionHubSaveCodec).
    /// </summary>
    public class ExpansionHubSaveTests
    {
        private static ExpansionHostSessionLike NewSystems()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            return new ExpansionHostSessionLike
            {
                Waystation = new WaystationSystem(),
                Layouts = new LocationLayoutSystem(files, json, NullLog.Instance),
                Memory = new LocationMemorySystem(files, json, NullLog.Instance),
                SiteEncounters = new SiteEncounterSystem(1117),
                Vouch = new VouchAccessSystem(),
                Greenhouse = new GreenhouseSystem(1117)
            };
        }

        [Fact]
        public void RoundTrip_RestoresEveryHubSurface()
        {
            var a = NewSystems();
            a.Waystation.Unlock();
            a.Waystation.AssignWatch(new[] { "elena_vasquez" });
            a.Layouts.Unlock();
            a.Vouch.GrantVouch("npc_osran_kell", isLastResort: false);
            a.Greenhouse.EnsurePlots(3);
            a.Greenhouse.Plant(0, "item_seed_tuber", 12, out _);
            a.Greenhouse.Water(0, 60f, tainted: false);
            a.Arbitration = new CrossingArbitrationSystem();
            a.Ledger = new LedgerDebtSystem();
            a.Arbitration.LoadBackerPool(new List<BackerDef>
            {
                new BackerDef { id = "npc_osran_kell", displayName = "Osran Kell", principled = true },
                new BackerDef { id = "npc_mattis_cray", displayName = "Mattis Cray", principled = true },
                new BackerDef { id = "npc_halden_mire", displayName = "Halden Mire", principled = false }
            });
            a.Arbitration.CallStanding("quest_crossing_the_terms", 12);
            a.Arbitration.DeclareBacker("quest_crossing_the_terms", "npc_osran_kell");
            a.Arbitration.DeclareBacker("quest_crossing_the_terms", "npc_mattis_cray");
            a.Arbitration.DeclareBacker("quest_crossing_the_terms", "npc_halden_mire");
            a.Ledger.PresentContract("wyn_loomis", 12f, 30, 0.2f, "the pledged grain");
            a.Ledger.PresentContract("wyn_loomis", 12f, 30, 0.2f, "the pledged grain");
            a.Ledger.SignContract("wyn_loomis", 12);

            var save = ExpansionHubSaveCodec.Capture(
                13, a.Waystation, a.Layouts, a.Memory, a.SiteEncounters, a.Vouch, a.Greenhouse,
                a.Arbitration, a.Ledger);
            var json = new SystemTextJsonSerializer();
            var loaded = ExpansionHubSaveCodec.Decode(
                ExpansionHubSaveCodec.Encode(save, json), json);

            var b = NewSystems();
            b.Arbitration = new CrossingArbitrationSystem();
            b.Ledger = new LedgerDebtSystem();
            ExpansionHubSaveCodec.Restore(
                loaded, b.Waystation, b.Layouts, b.Memory, b.SiteEncounters, b.Vouch, b.Greenhouse,
                b.Arbitration, b.Ledger);

            Assert.True(b.Waystation.Unlocked);
            Assert.True(b.Vouch.HasAccess);
            Assert.True(b.Layouts.State.expansionUnlocked);
            Assert.Equal(3, b.Greenhouse.PlotCount);
            Assert.Equal("item_seed_tuber", b.Greenhouse.State.plots[0].seedItemId);
            Assert.True(b.Arbitration.State.rulingsCalled >= 1, "arbitration rulings restored");
            Assert.True(b.Arbitration.IsRulingActive("quest_crossing_the_terms"),
                "arbitration active ruling restored");
            var restoredContract = b.Ledger.GetContract("wyn_loomis");
            Assert.NotNull(restoredContract);
            Assert.True(restoredContract.signed, "ledger contract signed after restore");
        }

        [Fact]
        public void Decode_RejectsTamperedChecksum()
        {
            var a = NewSystems();
            var save = ExpansionHubSaveCodec.Capture(
                13, a.Waystation, a.Layouts, a.Memory, a.SiteEncounters, a.Vouch, a.Greenhouse);
            var json = new SystemTextJsonSerializer();
            string text = ExpansionHubSaveCodec.Encode(save, json);

            string tampered = text.Replace("\"simDay\":13", "\"simDay\":1");
            Assert.NotEqual(text, tampered);
            Assert.Throws<InvalidOperationException>(
                () => ExpansionHubSaveCodec.Decode(tampered, json));
        }

        private sealed class ExpansionHostSessionLike
        {
            public WaystationSystem Waystation;
            public LocationLayoutSystem Layouts;
            public LocationMemorySystem Memory;
            public SiteEncounterSystem SiteEncounters;
            public VouchAccessSystem Vouch;
            public GreenhouseSystem Greenhouse;
            public CrossingArbitrationSystem Arbitration;
            public LedgerDebtSystem Ledger;
        }
    }
}
