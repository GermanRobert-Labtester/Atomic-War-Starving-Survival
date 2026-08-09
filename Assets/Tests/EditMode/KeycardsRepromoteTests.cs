using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Survivors;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// REPROMOTE-Item-001 — keycard doors on secure/military expedition nodes.
    /// </summary>
    [TestFixture]
    public class KeycardsRepromoteTests
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
        public void MapGenerator_TagsKeycardDoors_OnSecureNodes()
        {
            var map = MapGenerator.Generate(12345);
            int doors = 0;
            for (int i = 0; i < map.Nodes.Count; i++)
            {
                if (map.Nodes[i] != null && map.Nodes[i].HasTag("keycard_door"))
                    doors++;
            }
            Assert.That(doors, Is.GreaterThan(0));
        }

        [Test]
        public void Keycards_TryOpenDoor_RequiresMatchingColor()
        {
            var cards = new Item_Keycards();
            var owned = new List<KeycardColor> { KeycardColor.Green };
            Assert.IsFalse(cards.TryOpenDoor("sv_a", KeycardColor.Red, owned));
            Assert.IsTrue(cards.TryOpenDoor("sv_a", KeycardColor.Green, owned));
            Assert.IsTrue(cards.IsDoorUnlocked("item_keycard_green_door"));
        }

        [Test]
        public void Expedition_SecureNodeLooting_FindsCardAndUnlocks()
        {
            var needs = new NeedsSystem(_profile, sv => true);
            var rad = new RadiationSystem(needs);
            var inv = new Inventory { Capacity = 20, MaxWeight = 50f };
            var sv = new Survivor { Id = "sv_key", DisplayName = "Tech", State = SurvivorState.Idle };
            needs.Register(sv);
            rad.Register(sv);

            var map = MapGenerator.Generate(12345);
            string secureId = null;
            for (int i = 0; i < map.Nodes.Count; i++)
            {
                if (map.Nodes[i] != null && map.Nodes[i].HasTag("keycard_door"))
                {
                    secureId = map.Nodes[i].NodeId;
                    break;
                }
            }
            Assert.IsNotNull(secureId);

            var expSys = new ExpeditionSystem(rad, inv, null, new ExpeditionSystem.Config
            {
                Seed = 5,
                CreateDefaultEncounters = false
            });
            expSys.SetGeneratedMap(map);

            var cards = new Item_Keycards();
            int found = 0, unlocked = 0;
            cards.OnKeycardFound += (_, __) => found++;
            cards.OnDoorUnlocked += (_, __) => unlocked++;
            expSys.BindKeycards(cards);

            Assert.IsTrue(expSys.StartExpeditionFromPath(sv, new ExpeditionSystem.PathRequest
            {
                NodeId = secureId,
                DisplayName = "Secure Hangar",
                TravelHours = 1f,
                TrueRad = 20f,
                DangerLevel = 2f,
                Stance = ExpeditionStance.Speed,
                MaxLootCapacity = 10f
            }));

            for (int t = 0; t < 20 && unlocked == 0; t++)
                expSys.Tick(1f);

            Assert.That(found, Is.GreaterThanOrEqualTo(1), "must find a keycard on secure node");
            Assert.That(unlocked, Is.GreaterThanOrEqualTo(1), "must unlock door with matching card");
            expSys.UnsubscribeAll();
        }
    }
}
