using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.UI;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Internal Horror player surfaces: corpse dispose, fire fight/seal,
    /// status icon strip. Pure formatter + view-model tests (no scene systems).
    /// </summary>
    [TestFixture]
    public class InternalHorrorHudTests
    {
        private GameObject _go;
        private InternalHorrorHUD _hud;
        private InventoryStripUI _strip;
        private List<Object> _toDestroy;

        [SetUp]
        public void SetUp()
        {
            _toDestroy = new List<Object>();
            _go = new GameObject("InternalHorrorHud_Test");
            _hud = _go.AddComponent<InternalHorrorHUD>();
            _strip = _go.AddComponent<InventoryStripUI>();
        }

        [TearDown]
        public void TearDown()
        {
            if (_go != null) Object.DestroyImmediate(_go);
            for (int i = 0; i < _toDestroy.Count; i++)
            {
                if (_toDestroy[i] != null) Object.DestroyImmediate(_toDestroy[i]);
            }
        }

        // ── Formatter ───────────────────────────────────────────────────

        [Test]
        public void Formatter_CorpseStatus_EmptyWhenNone()
        {
            Assert.That(InternalHorrorHudFormatter.FormatCorpseStatus(0), Is.EqualTo(string.Empty));
        }

        [Test]
        public void Formatter_CorpseStatus_ShowsBodyCount()
        {
            Assert.That(InternalHorrorHudFormatter.FormatCorpseStatus(1), Does.Contain("BODY"));
            Assert.That(InternalHorrorHudFormatter.FormatCorpseStatus(3), Does.Contain("3"));
        }

        [Test]
        public void Formatter_CorpseDetail_MentionsBuryOrLight()
        {
            string can = InternalHorrorHudFormatter.FormatCorpseDetail(1, canBury: true, daylightHours: 8f);
            Assert.That(can, Does.Contain("Bury").IgnoreCase);

            string dark = InternalHorrorHudFormatter.FormatCorpseDetail(1, canBury: false, daylightHours: 1f);
            Assert.That(dark, Does.Contain("light").IgnoreCase);
            Assert.That(dark, Does.Contain("fertilizer").IgnoreCase);
        }

        [Test]
        public void Formatter_FireAlert_EmptyWhenClear()
        {
            Assert.That(InternalHorrorHudFormatter.FormatFireAlert(0), Is.EqualTo(string.Empty));
        }

        [Test]
        public void Formatter_FireAlert_ShowsWhenBurning()
        {
            Assert.That(InternalHorrorHudFormatter.FormatFireAlert(1), Does.Contain("FIRE"));
            Assert.That(InternalHorrorHudFormatter.FormatFireAlert(2), Does.Contain("2"));
        }

        [Test]
        public void Formatter_IconStrip_CombinesActiveThreats()
        {
            string strip = InternalHorrorHudFormatter.BuildIconStrip(
                corpses: 1, fires: 1, comas: 1, comaUrgent: true, contaminated: 2);
            Assert.That(strip, Does.Contain("[FIRE]"));
            Assert.That(strip, Does.Contain("[BODY]"));
            Assert.That(strip, Does.Contain("[CARE!]"));
            Assert.That(strip, Does.Contain("[RUST"));
        }

        [Test]
        public void Formatter_IconStrip_EmptyWhenAllClear()
        {
            Assert.That(
                InternalHorrorHudFormatter.BuildIconStrip(0, 0, 0, false, 0),
                Is.EqualTo(string.Empty));
        }

        [Test]
        public void Formatter_ComaStatus_MarksOverdue()
        {
            string urgent = InternalHorrorHudFormatter.FormatComaStatus(
                1, worstHoursSinceCare: 7f, careInterval: 6f, urgent: true);
            Assert.That(urgent, Does.Contain("overdue").IgnoreCase);

            string calm = InternalHorrorHudFormatter.FormatComaStatus(
                2, worstHoursSinceCare: 1f, careInterval: 6f, urgent: false);
            Assert.That(calm, Does.Contain("2"));
            Assert.That(calm, Does.Not.Contain("overdue").IgnoreCase);
        }

        [Test]
        public void Formatter_ContaminatedFood_LabelsRustedCans()
        {
            Assert.That(InternalHorrorHudFormatter.FormatContaminatedFood(0), Is.EqualTo(string.Empty));
            Assert.That(InternalHorrorHudFormatter.FormatContaminatedFood(1), Does.Contain("rusted").IgnoreCase);
            Assert.That(InternalHorrorHudFormatter.FormatContaminatedFood(4), Does.Contain("4"));
        }

        // ── HUD snapshot / actions ──────────────────────────────────────

        [Test]
        public void ApplySnapshot_PopulatesCorpseAndIcons()
        {
            var snap = new InternalHorrorSnapshot
            {
                CorpseCount = 2,
                CanBury = true,
                DaylightHoursAvailable = 10f,
                ContaminatedFoodCount = 3,
                CareIntervalHours = 6f
            };
            _hud.ApplySnapshot(snap);

            Assert.That(_hud.CorpseCount, Is.EqualTo(2));
            Assert.IsTrue(_hud.CanBury);
            Assert.IsTrue(_hud.CanProcessFertilizer);
            // Plural form for count > 1 ("BODIES ×2").
            Assert.That(_hud.CorpseStatusLine, Does.Contain("BODIES"));
            Assert.That(_hud.ContaminatedFoodCount, Is.EqualTo(3));
            Assert.That(_hud.StatusIconStrip, Does.Contain("[BODY"));
            Assert.That(_hud.StatusIconStrip, Does.Contain("[RUST"));
        }

        [Test]
        public void ApplySnapshot_FireOpensAlertState()
        {
            var snap = new InternalHorrorSnapshot
            {
                Fires = new[]
                {
                    new FireRoomSnapshot
                    {
                        RoomId = "plant",
                        IsOnFire = true,
                        Intensity = 0.7f,
                        OxygenFraction = 0.18f
                    }
                }
            };
            _hud.ApplySnapshot(snap);

            Assert.IsTrue(_hud.HasActiveFire);
            Assert.That(_hud.FireRooms.Count, Is.EqualTo(1));
            Assert.That(_hud.FireAlertLine, Does.Contain("FIRE"));
            Assert.That(_hud.ActiveFireRoomId, Is.EqualTo("plant"));
            // Rising-edge ping marks [FIRE!] until the panel is opened.
            Assert.That(_hud.StatusIconStrip, Does.Contain("FIRE"));
            Assert.IsTrue(_hud.FireNotificationPing);
        }

        [Test]
        public void SelectCorpseDispose_Bury_FiresCallback_WhenAllowed()
        {
            int buryCalls = 0;
            int fertCalls = 0;
            _hud.OnBuryRequested += () => buryCalls++;
            _hud.OnProcessFertilizerRequested += () => fertCalls++;

            _hud.ApplySnapshot(new InternalHorrorSnapshot
            {
                CorpseCount = 1,
                CanBury = true,
                DaylightHoursAvailable = 8f
            });

            Assert.IsTrue(_hud.SelectCorpseDispose(CorpseDisposeChoice.Bury));
            Assert.That(buryCalls, Is.EqualTo(1));
            Assert.That(fertCalls, Is.EqualTo(0));

            Assert.IsTrue(_hud.SelectCorpseDispose(CorpseDisposeChoice.ProcessFertilizer));
            Assert.That(fertCalls, Is.EqualTo(1));
        }

        [Test]
        public void SelectCorpseDispose_Bury_BlockedWithoutDaylight()
        {
            int buryCalls = 0;
            _hud.OnBuryRequested += () => buryCalls++;
            _hud.ApplySnapshot(new InternalHorrorSnapshot
            {
                CorpseCount = 1,
                CanBury = false,
                DaylightHoursAvailable = 1f
            });

            Assert.IsFalse(_hud.SelectCorpseDispose(CorpseDisposeChoice.Bury));
            Assert.That(buryCalls, Is.EqualTo(0));
            // Fertilizer still available in the dark
            Assert.IsTrue(_hud.SelectCorpseDispose(CorpseDisposeChoice.ProcessFertilizer));
        }

        [Test]
        public void SelectFightAndSeal_Fire_FiresCallbacks()
        {
            string fought = null;
            string sealedRoom = null;
            string extinguished = null;
            _hud.OnFightFireRequested += id => fought = id;
            _hud.OnSealBulkheadRequested += id => sealedRoom = id;
            _hud.OnExtinguishFireRequested += id => extinguished = id;

            _hud.ApplySnapshot(new InternalHorrorSnapshot
            {
                Fires = new[]
                {
                    new FireRoomSnapshot
                    {
                        RoomId = "quarters",
                        IsOnFire = true,
                        Intensity = 0.5f,
                        OxygenFraction = 0.2f
                    }
                }
            });

            Assert.IsTrue(_hud.SelectFightFire());
            Assert.That(fought, Is.EqualTo("quarters"));
            Assert.IsTrue(_hud.SelectSealBulkhead("quarters"));
            Assert.That(sealedRoom, Is.EqualTo("quarters"));
            Assert.IsTrue(_hud.SelectExtinguishFire("quarters"));
            Assert.That(extinguished, Is.EqualTo("quarters"));
        }

        [Test]
        public void SelectFightFire_FailsWhenNoFire()
        {
            int calls = 0;
            _hud.OnFightFireRequested += _ => calls++;
            _hud.ApplySnapshot(new InternalHorrorSnapshot());
            Assert.IsFalse(_hud.SelectFightFire());
            Assert.That(calls, Is.EqualTo(0));
        }

        [Test]
        public void ComaSnapshot_SetsUrgentIcon()
        {
            _hud.ApplySnapshot(new InternalHorrorSnapshot
            {
                CareIntervalHours = 6f,
                ComaCareUrgent = true,
                Comas = new[]
                {
                    new ComaPatientSnapshot
                    {
                        SurvivorId = "sv_a",
                        DisplayName = "Elena",
                        HoursSinceLastCare = 5.5f,
                        NeedsCare = true
                    }
                }
            });

            Assert.That(_hud.ComaPatientCount, Is.EqualTo(1));
            Assert.IsTrue(_hud.ComaCareUrgent);
            Assert.That(_hud.ComaStatusLine, Does.Contain("overdue").IgnoreCase);
            Assert.That(_hud.StatusIconStrip, Does.Contain("[CARE!]"));
        }

        [Test]
        public void OpenCorpsePanel_RequiresBody()
        {
            _hud.ApplySnapshot(new InternalHorrorSnapshot { CorpseCount = 0 });
            _hud.OpenCorpsePanel();
            Assert.IsFalse(_hud.IsCorpsePanelOpen);

            _hud.ApplySnapshot(new InternalHorrorSnapshot { CorpseCount = 1, CanBury = true });
            _hud.OpenCorpsePanel();
            Assert.IsTrue(_hud.IsCorpsePanelOpen);
        }

        // ── Inventory strip flags ───────────────────────────────────────

        [Test]
        public void InventoryStrip_FlagsCorpseAndContaminatedFood()
        {
            var inv = new Inventory { Capacity = 20 };
            var corpse = ScriptableObject.CreateInstance<ItemDefinition>();
            _toDestroy.Add(corpse);
            corpse.id = "corpse";
            corpse.displayName = "Body";
            corpse.type = ItemType.Corpse;
            corpse.stackMax = 1;
            corpse.weight = 70f;

            var rust = ScriptableObject.CreateInstance<ItemDefinition>();
            _toDestroy.Add(rust);
            rust.id = "contaminated_food";
            rust.displayName = "Rusted Can";
            rust.type = ItemType.ContaminatedFood;
            rust.stackMax = 20;
            rust.weight = 0.4f;

            inv.Add(corpse, 1);
            inv.Add(rust, 4);
            _strip.Sync(inv);

            Assert.That(_strip.CorpseCount(), Is.EqualTo(1));
            Assert.That(_strip.ContaminatedFoodCount(), Is.EqualTo(4));
            Assert.IsNotNull(_strip.FindFirstCorpseIcon());
            Assert.IsTrue(_strip.FindFirstCorpseIcon().HasDisposeActions);
            Assert.That(_strip.StripSummary, Does.Contain("dispose").IgnoreCase);
            Assert.That(_strip.StripSummary, Does.Contain("rust").IgnoreCase);
        }

        [Test]
        public void InventoryStrip_ActivateCorpse_RaisesClickAndMarksSelected()
        {
            var inv = new Inventory { Capacity = 20 };
            var food = ScriptableObject.CreateInstance<ItemDefinition>();
            _toDestroy.Add(food);
            food.id = "food_can";
            food.displayName = "Can";
            food.type = ItemType.Food;
            food.stackMax = 20;
            food.weight = 0.4f;

            var corpse = ScriptableObject.CreateInstance<ItemDefinition>();
            _toDestroy.Add(corpse);
            corpse.id = "corpse";
            corpse.displayName = "Body";
            corpse.type = ItemType.Corpse;
            corpse.stackMax = 1;
            corpse.weight = 70f;

            inv.Add(food, 2);
            inv.Add(corpse, 1);
            _strip.Sync(inv);

            InventoryIcon activated = null;
            _strip.OnIconActivated += icon => activated = icon;

            int corpseIdx = _strip.FindFirstCorpseIndex();
            Assert.That(corpseIdx, Is.GreaterThanOrEqualTo(0));
            Assert.IsTrue(_strip.ActivateIndex(corpseIdx));
            Assert.IsNotNull(activated);
            Assert.IsTrue(activated.IsCorpse);
            Assert.IsTrue(activated.HasDisposeActions);
            Assert.That(_strip.SelectedIndex, Is.EqualTo(corpseIdx));
            Assert.That(_strip.StripSummary, Does.Contain(">"));
        }

        [Test]
        public void InventoryStrip_ActivateFirstCorpse_IsClickEntryPoint()
        {
            var inv = new Inventory { Capacity = 10 };
            var corpse = ScriptableObject.CreateInstance<ItemDefinition>();
            _toDestroy.Add(corpse);
            corpse.id = "corpse";
            corpse.displayName = "Body";
            corpse.type = ItemType.Corpse;
            corpse.stackMax = 1;
            corpse.weight = 70f;
            inv.Add(corpse, 1);
            _strip.Sync(inv);

            int clicks = 0;
            _strip.OnIconActivated += _ => clicks++;
            Assert.IsTrue(_strip.ActivateFirstCorpse());
            Assert.That(clicks, Is.EqualTo(1));
        }

        [Test]
        public void FireRisingEdge_RaisesHorrorPing_ClearsWhenPanelOpened()
        {
            HorrorPingKind last = HorrorPingKind.None;
            int pings = 0;
            _hud.OnHorrorPing += k => { last = k; pings++; };

            _hud.ApplySnapshot(new InternalHorrorSnapshot
            {
                Fires = new[]
                {
                    new FireRoomSnapshot
                    {
                        RoomId = "plant",
                        IsOnFire = true,
                        Intensity = 0.5f,
                        OxygenFraction = 0.18f
                    }
                }
            });

            Assert.IsTrue(_hud.FireNotificationPing);
            Assert.That(last, Is.EqualTo(HorrorPingKind.Fire));
            Assert.That(pings, Is.EqualTo(1));
            Assert.That(_hud.StatusIconStrip, Does.Contain("FIRE"));

            _hud.OpenFirePanel("plant");
            Assert.IsFalse(_hud.FireNotificationPing);
            Assert.That(_hud.FirePanelChoicesLine, Does.Contain("[1]"));
            Assert.That(_hud.FirePanelChoicesLine, Does.Contain("[2]"));
            Assert.That(_hud.FirePanelChoicesLine, Does.Contain("[3]"));
        }

        [Test]
        public void CareRisingEdge_RaisesCarePing()
        {
            HorrorPingKind last = HorrorPingKind.None;
            _hud.OnHorrorPing += k => last = k;

            _hud.ApplySnapshot(new InternalHorrorSnapshot
            {
                CareIntervalHours = 6f,
                ComaCareUrgent = true,
                Comas = new[]
                {
                    new ComaPatientSnapshot
                    {
                        SurvivorId = "sv_a",
                        DisplayName = "Elena",
                        HoursSinceLastCare = 6f,
                        NeedsCare = true
                    }
                }
            });

            Assert.IsTrue(_hud.CareNotificationPing);
            Assert.That(last, Is.EqualTo(HorrorPingKind.CareUrgent));
            Assert.That(_hud.CarePingCount, Is.EqualTo(1));

            // Same urgency again must not re-fire.
            last = HorrorPingKind.None;
            _hud.ApplySnapshot(new InternalHorrorSnapshot
            {
                CareIntervalHours = 6f,
                ComaCareUrgent = true,
                Comas = new[]
                {
                    new ComaPatientSnapshot
                    {
                        SurvivorId = "sv_a",
                        DisplayName = "Elena",
                        HoursSinceLastCare = 6.5f,
                        NeedsCare = true
                    }
                }
            });
            Assert.That(last, Is.EqualTo(HorrorPingKind.None));
            Assert.That(_hud.CarePingCount, Is.EqualTo(1));
        }

        [Test]
        public void CorpsePanel_ShowsHotkeyChoices()
        {
            _hud.ApplySnapshot(new InternalHorrorSnapshot
            {
                CorpseCount = 1,
                CanBury = true,
                DaylightHoursAvailable = 8f
            });
            _hud.OpenCorpsePanel();
            Assert.That(_hud.CorpsePanelChoicesLine, Does.Contain("Bury"));
            Assert.That(_hud.CorpsePanelChoicesLine, Does.Contain("fertilizer").IgnoreCase);
            Assert.That(_hud.CorpsePanelChoicesLine, Does.Contain("[1]"));
            Assert.That(_hud.CorpsePanelChoicesLine, Does.Contain("[2]"));
        }

        [Test]
        public void KeybindDocs_PlayerInputHandler_ExposesHorrorInventoryKeys()
        {
            var go = new GameObject("HorrorInputKeys");
            _toDestroy.Add(go);
            var input = go.AddComponent<AtomicWar._Game.Core.PlayerInputHandler>();
            Assert.That(input.CorpseDisposeKey, Is.EqualTo(KeyCode.C));
            Assert.That(input.InventoryCycleKey, Is.EqualTo(KeyCode.I));
            Assert.That(input.InventoryActivateKey, Is.EqualTo(KeyCode.E));
            Assert.That(input.ClosePanelKey, Is.EqualTo(KeyCode.Escape));
        }
    }
}
