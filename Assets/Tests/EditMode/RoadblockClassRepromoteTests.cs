using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Events;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Survivors;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// REPROMOTE-Encounter-001 — class roadblock must resolve when SO id is tagged.
    /// </summary>
    [TestFixture]
    public class RoadblockClassRepromoteTests
    {
        private NeedsProfile _profile;
        private List<Object> _destroy;

        [SetUp]
        public void SetUp()
        {
            _destroy = new List<Object>();
            _profile = ScriptableObject.CreateInstance<NeedsProfile>();
            _destroy.Add(_profile);
        }

        [TearDown]
        public void TearDown()
        {
            for (int i = 0; i < _destroy.Count; i++)
            {
                if (_destroy[i] != null) Object.DestroyImmediate(_destroy[i]);
            }
            _destroy = null;
        }

        [Test]
        public void Roadblock_ResolveChoice_PayToll_ConsumesFuelRef()
        {
            var rb = new Encounter_Roadblock();
            int fuel = 20;
            float chassis = 100f;
            Assert.IsTrue(rb.ResolveChoice(RoadblockChoice.PayToll, ref fuel, ref chassis, out float hours));
            Assert.AreEqual(15, fuel);
            Assert.AreEqual(0f, hours, 1e-3f);
        }

        [Test]
        public void Expedition_RoadblockSoId_DispatchesClassTracker()
        {
            var needs = new NeedsSystem(_profile, sv => true);
            var rad = new RadiationSystem(needs);
            var inv = new Inventory { Capacity = 40, MaxWeight = 100f };
            var fuel = ScriptableObject.CreateInstance<ItemDefinition>();
            fuel.id = "fuel";
            fuel.stackMax = 50;
            _destroy.Add(fuel);
            inv.Add(fuel, 20);

            var sv = new Survivor { Id = "sv_rb", DisplayName = "Runner", State = SurvivorState.Idle };
            needs.Register(sv);
            rad.Register(sv);

            var expSys = new ExpeditionSystem(rad, inv, null, new ExpeditionSystem.Config
            {
                Seed = 9,
                CreateDefaultEncounters = false
            });
            expSys.SetItemHandlers(
                countItem: id => inv.CountById(id),
                consumeItem: (id, n) => inv.RemoveById(id, n));

            var roadblock = new Encounter_Roadblock();
            int resolved = 0;
            roadblock.OnRoadblockResolved += (_, __) => resolved++;
            expSys.BindClassRoadblock(roadblock);

            Assert.IsTrue(expSys.StartExpeditionFromPath(sv, new ExpeditionSystem.PathRequest
            {
                NodeId = "highway_roadblock_north",
                DisplayName = "Highway Roadblock",
                TravelHours = 2f,
                TrueRad = 5f,
                DangerLevel = 1f,
                Stance = ExpeditionStance.Stealth,
                MaxLootCapacity = 20f
            }));

            var exp = expSys.GetExpeditionBySurvivor(sv.Id);
            Assert.IsNotNull(exp);

            var so = ScriptableObject.CreateInstance<EncounterSO>();
            _destroy.Add(so);
            so.id = "enc_faction_roadblock";
            so.title = "Faction Roadblock";
            so.category = EncounterCategory.Hazard;
            so.choices = new List<EventChoice>
            {
                new EventChoice { ChoiceId = "pay", Text = "Pay the toll" },
                new EventChoice { ChoiceId = "detour", Text = "Turn back" },
                new EventChoice { ChoiceId = "ram", Text = "Ram it" }
            };

            expSys.ForceResolveEncounterForTests(exp, so);

            Assert.AreEqual(1, resolved,
                "Class roadblock ResolveChoice must fire when SO id contains 'roadblock'");
            expSys.UnsubscribeAll();
        }
    }
}
