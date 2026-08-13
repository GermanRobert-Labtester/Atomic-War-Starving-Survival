using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Data;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Factions;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Survivors;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// ASHFALL: NOBODY'S CHARTER — Sprint 0 host couplings:
    /// (a) the BurnCrossingVouch sponsor rule — when the burned sponsor was
    ///     Mattis Cray, Mattis burns with his name (he will not vouch again);
    /// (b) ExpeditionSystem.SetVouchAccessSystem — the viaduct gate refuses
    ///     departure without a vouch, with a diegetic refusal line, and
    ///     opens once a name is on the ledger.
    /// Pure EditMode; mirrors ExpeditionEngineTests construction.
    /// </summary>
    [TestFixture]
    public class CrossingGateCouplingTests
    {
        private NeedsSystem _needsSystem;
        private RadiationSystem _radSystem;
        private Inventory _inventory;
        private ItemCatalogSO _itemCatalog;
        private ExpeditionSystem _expeditionSystem;

        [SetUp]
        public void SetUp()
        {
            var needsProfile = ScriptableObject.CreateInstance<NeedsProfile>();
            needsProfile.hungerPerHour = 2f;
            needsProfile.thirstPerHour = 3f;
            needsProfile.fatiguePerHour = 1.5f;
            _needsSystem = new NeedsSystem(needsProfile, sv => true);
            _radSystem = new RadiationSystem(_needsSystem);
            _inventory = new Inventory { Capacity = 50, MaxWeight = 200f };
            _itemCatalog = ScriptableObject.CreateInstance<ItemCatalogSO>();
            _itemCatalog.items = new List<ItemDefinition>();
            _expeditionSystem = new ExpeditionSystem(_radSystem, _inventory, _itemCatalog, seed: 12345);
        }

        private Survivor NewSurvivor(string id)
        {
            var sv = new Survivor { Id = id, DisplayName = id };
            _needsSystem.Register(sv);
            _radSystem.Register(sv);
            return sv;
        }

        // ── (a) Burn coupling — mirrors BurnCrossingVouch's sponsor rule ──

        [Test]
        public void BurnRule_MattisSponsor_BurnsMattis()
        {
            var vouch = new VouchAccessSystem();
            var mattis = new NPC_MattisCray();
            mattis.Initialise("Mattis Cray");

            vouch.GrantVouch(CrossingIds.Npcs.MattisCray);
            string sponsor = vouch.VouchedBy;
            bool burned = vouch.BurnVouch();
            if (burned && sponsor == CrossingIds.Npcs.MattisCray)
                mattis.BurnMattis();

            Assert.That(vouch.RequiresVouch, Is.True);
            Assert.That(mattis.WillVouch, Is.False, "he does not offer his name a second time");
        }

        [Test]
        public void BurnRule_OtherSponsor_LeavesMattisWilling()
        {
            var vouch = new VouchAccessSystem();
            var mattis = new NPC_MattisCray();
            mattis.Initialise("Mattis Cray");

            vouch.GrantVouch(CrossingIds.Npcs.OsranKell);
            string sponsor = vouch.VouchedBy;
            bool burned = vouch.BurnVouch();
            if (burned && sponsor == CrossingIds.Npcs.MattisCray)
                mattis.BurnMattis();

            Assert.That(mattis.WillVouch, Is.True, "Osran's burn is not Mattis's burn");
            Assert.That(vouch.NeedsLastResort, Is.True, "burned sponsor opens the last-resort path");
        }

        // ── (b) The travel gate ───────────────────────────────────────────

        [Test]
        public void Gate_RefusesCatalogLocation_ThenAllowsAfterVouch()
        {
            var vouch = new VouchAccessSystem();
            _expeditionSystem.SetVouchAccessSystem(vouch);

            var gate = ScriptableObject.CreateInstance<LocationDefinitionSO>();
            gate.id = CrossingIds.Locations.ViaductGate;
            gate.displayName = "The Viaduct Gate";
            gate.travelHours = 8f;
            gate.baseRadsPerHour = 22f;
            gate.dangerLevel = 4.5f;

            var sv = NewSurvivor("sv_crossing_a");
            Assert.That(_expeditionSystem.StartExpedition(sv, gate), Is.False,
                "no name on the ledger, no crossing");
            Assert.That(_expeditionSystem.LastCrossingRefusal, Is.Not.Empty,
                "the refusal is a diegetic line, not a bare false");

            vouch.GrantVouch(CrossingIds.Npcs.MattisCray);
            Assert.That(_expeditionSystem.StartExpedition(sv, gate), Is.True,
                "vouched: the viaduct walk is legal");
            Assert.That(_expeditionSystem.IsOnExpedition(sv.Id), Is.True);
        }

        [Test]
        public void Gate_RefusesRegionTaggedMapNode_AndIgnoresOtherNodes()
        {
            var vouch = new VouchAccessSystem();
            _expeditionSystem.SetVouchAccessSystem(vouch);

            var stallrow = new MapNode
            {
                NodeId = CrossingIds.Locations.Stallrow,
                DisplayName = "Stallrow",
                TrueRad = 20f,
                DangerLevel = 3.5f,
                DistanceFromShelter = 0.5f,
                Tags = new List<string> { CrossingIds.Region }
            };
            var sv = NewSurvivor("sv_crossing_b");
            Assert.That(_expeditionSystem.StartExpedition(sv, stallrow), Is.False,
                "region_crossing nodes are behind the gate");
            Assert.That(_expeditionSystem.LastCrossingRefusal, Is.Not.Empty);

            // The approach waypoints stay walkable without a vouch — the
            // gate is social, not a wall around the whole approach road.
            var elsewhere = new MapNode
            {
                NodeId = "location_abandoned_convoy_yard",
                DisplayName = "Abandoned Convoy Yard",
                TrueRad = 20f,
                DangerLevel = 3f,
                DistanceFromShelter = 6f,
                Tags = new List<string> { "region_warlord" }
            };
            var sv2 = NewSurvivor("sv_crossing_c");
            Assert.That(_expeditionSystem.StartExpedition(sv2, elsewhere), Is.True,
                "non-Crossing nodes ignore the vouch gate");
        }
    }
}
