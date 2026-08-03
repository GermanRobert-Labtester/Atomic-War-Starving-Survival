using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Data;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Events;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.UI;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Dynamic phase economy + faction trust matrix.
    /// </summary>
    [TestFixture]
    public class FactionEconomyTests
    {
        private const float Eps = 1e-3f;
        private List<FactionSO> _factions;
        private WorldPhase _phase;

        [SetUp]
        public void SetUp()
        {
            _phase = WorldPhase.CivilWar;
            _factions = DynamicEconomySystem.CreateDefaultFactions();
        }

        [TearDown]
        public void TearDown()
        {
            if (_factions == null) return;
            for (int i = 0; i < _factions.Count; i++)
                Object.DestroyImmediate(_factions[i]);
            _factions = null;
        }

        private DynamicEconomySystem MakeEconomy(Shelter shelter = null)
        {
            var eco = new DynamicEconomySystem(() => _phase, shelter, new System.Random(1));
            for (int i = 0; i < _factions.Count; i++)
                eco.RegisterFaction(_factions[i]);
            return eco;
        }

        private static ItemDefinition MakeItem(string id, ItemType type, float tradeValue)
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = id;
            item.displayName = id;
            item.type = type;
            item.tradeValue = tradeValue;
            item.stackMax = 50;
            item.weight = 0.1f;
            return item;
        }

        [Test]
        public void Jewelry_TradeValue_DropsToZero_OnDay30Flashpoint()
        {
            var jewelry = MakeItem("jewelry", ItemType.Trade, 50f);
            var eco = MakeEconomy();

            // Pre-Day 30 (Civil War): jewelry still has value
            _phase = WorldPhase.CivilWar;
            float pre = eco.GetTradeValue(jewelry);
            Assert.That(pre, Is.GreaterThan(0f), "Jewelry must hold value before Flashpoint");
            Assert.That(pre, Is.EqualTo(50f).Within(Eps));

            // Day 30 Flashpoint event: conventional trade goods leave the pool
            _phase = WorldPhase.Flashpoint;
            eco.NotifyPhaseChanged(WorldPhase.Flashpoint);
            float flash = eco.GetTradeValue(jewelry);
            Assert.That(flash, Is.EqualTo(0f).Within(Eps),
                "Item_Jewelry / jewelry trade value must drop to 0 on Day 30 Flashpoint");

            // Nuclear Winter keeps currency worthless
            _phase = WorldPhase.NuclearWinter;
            Assert.That(eco.GetTradeValue(jewelry), Is.EqualTo(0f).Within(Eps));

            Object.DestroyImmediate(jewelry);
        }

        [Test]
        public void PreDay30_FoodExpensive_AntiRadCheap()
        {
            var food = MakeItem("canned_food", ItemType.Food, 12f);
            var anti = MakeItem("anti_rad", ItemType.AntiRad, 8f);
            _phase = WorldPhase.CivilWar;
            var eco = MakeEconomy();

            float foodVal = eco.GetTradeValue(food);
            float antiVal = eco.GetTradeValue(anti);

            Assert.That(foodVal, Is.EqualTo(12f * TradeEconomy.PreFlashpointFoodMultiplier).Within(Eps));
            Assert.That(antiVal, Is.EqualTo(8f * TradeEconomy.PreFlashpointAntiRadMultiplier).Within(Eps));
            Assert.That(foodVal, Is.GreaterThan(antiVal),
                "Pre-Day 30: food should outprice anti-rad (nobody thinks they need pills yet)");

            Object.DestroyImmediate(food);
            Object.DestroyImmediate(anti);
        }

        [Test]
        public void PostDay30_AntiRadAndIodine_TenX_WaterIsGold()
        {
            var anti = MakeItem("anti_rad", ItemType.AntiRad, 8f);
            var iodine = MakeItem("iodine_pills", ItemType.Iodine, 6f);
            var water = MakeItem("clean_water", ItemType.Water, 15f);
            var currency = MakeItem("currency", ItemType.Trade, 20f);
            _phase = WorldPhase.NuclearWinter;
            var eco = MakeEconomy();

            Assert.That(eco.GetTradeValue(anti),
                Is.EqualTo(8f * TradeEconomy.PostFlashpointRadMedMultiplier).Within(Eps));
            Assert.That(eco.GetTradeValue(iodine),
                Is.EqualTo(6f * TradeEconomy.PostFlashpointRadMedMultiplier).Within(Eps));
            Assert.That(eco.GetTradeValue(water),
                Is.EqualTo(15f * TradeEconomy.PostFlashpointWaterMultiplier).Within(Eps));
            Assert.That(eco.GetTradeValue(currency), Is.EqualTo(0f).Within(Eps),
                "Conventional currency removed from trade pool post-Flashpoint");

            Object.DestroyImmediate(anti);
            Object.DestroyImmediate(iodine);
            Object.DestroyImmediate(water);
            Object.DestroyImmediate(currency);
        }

        [Test]
        public void RefuseScout_DropsTrust_RaidAtMinus50()
        {
            var shelter = new Shelter();
            shelter.AddModule(new ShelterModuleInstance("air_filtration", 1) { FilterHealth = 100f });
            shelter.AddModule(new ShelterModuleInstance("radiation_shielding", 1));

            var eco = MakeEconomy(shelter);
            var faction = eco.GetFaction(FactionSO.Ids.ScavengerCamp);
            Assert.IsNotNull(faction);

            // Drive trust to just above raid line, then refuse scout
            eco.SetTrust(faction.id, -25f);
            var runner = new EventRunner();
            eco.BindEventRunner(runner);

            var scout = DynamicEconomySystem.CreateFactionScoutEvent(faction);
            var refuse = scout.choices.Find(c => c.ChoiceId == "refuse_scout");
            Assert.IsNotNull(refuse);

            var ctx = new EventContext();
            runner.ApplyChoice(scout, refuse, ctx);

            Assert.That(eco.GetTrust(faction.id), Is.EqualTo(-55f).Within(Eps),
                "Refuse scout should apply TrustDelta -30");
            Assert.That(eco.GetStance(faction.id), Is.EqualTo(TradeStance.HostileRaid));

            // Explicit raid when already at/below threshold
            float filterBefore = shelter.GetModule("air_filtration").FilterHealth;
            var raid = eco.TryLaunchRaid(faction.id);
            Assert.IsTrue(raid.Launched);
            // Either repelled or damaged hatch
            Assert.That(raid.HatchDamage, Is.GreaterThan(0f));
            if (!raid.Repelled)
            {
                Assert.That(shelter.GetModule("air_filtration").FilterHealth,
                    Is.LessThan(filterBefore));
            }

            Object.DestroyImmediate(scout);
        }

        [Test]
        public void TradeScreen_BarterMath_UsesPhaseAndTrust()
        {
            var food = MakeItem("canned_food", ItemType.Food, 12f);
            var water = MakeItem("clean_water", ItemType.Water, 15f);
            _phase = WorldPhase.CivilWar;
            var eco = MakeEconomy();

            var player = new Inventory { Capacity = 20, MaxWeight = 100f };
            var stock = new Inventory { Capacity = 20, MaxWeight = 100f };
            player.Add(food, 4);
            stock.Add(water, 2);

            var go = new GameObject("TradeScreenTest");
            var ui = go.AddComponent<TradeScreenUI>();
            ui.Bind(eco);
            Assert.IsTrue(ui.Open(FactionSO.Ids.ScavengerCamp, player, stock));

            ui.SetPlayerOffer(food, 2);
            ui.SetFactionAsk(water, 1);
            ui.Recalculate();

            float expectedOffer = eco.GetBarterUnitValue(food, FactionSO.Ids.ScavengerCamp, true) * 2;
            float expectedAsk = eco.GetBarterUnitValue(water, FactionSO.Ids.ScavengerCamp, false) * 1;
            Assert.That(ui.PlayerOfferValue, Is.EqualTo(expectedOffer).Within(Eps));
            Assert.That(ui.FactionAskValue, Is.EqualTo(expectedAsk).Within(Eps));
            Assert.That(ui.Phase, Is.EqualTo(WorldPhase.CivilWar));

            // Flashpoint zeroes trade goods mid-screen and revalues water
            _phase = WorldPhase.Flashpoint;
            ui.Recalculate();
            Assert.That(ui.Phase, Is.EqualTo(WorldPhase.Flashpoint));
            // Food still has base phase value (not Trade type); water spikes
            Assert.That(ui.GetDisplayedUnitValue(water, fromPlayerOffer: false),
                Is.GreaterThan(expectedAsk));

            string summary = ui.BuildQuoteSummary();
            Assert.That(summary, Does.Contain("Phase: Flashpoint"));
            Assert.That(summary, Does.Contain("Trust:"));

            Object.DestroyImmediate(food);
            Object.DestroyImmediate(water);
            Object.DestroyImmediate(go);
        }

        [Test]
        public void FairTrade_TransfersItems_AndNudgesDemand()
        {
            var food = MakeItem("canned_food", ItemType.Food, 20f);
            var tool = MakeItem("tweezers", ItemType.Tool, 10f);
            _phase = WorldPhase.CivilWar;
            var eco = MakeEconomy();
            // Neutral trust for scavenger
            eco.SetTrust(FactionSO.Ids.ScavengerCamp, 0f);

            var player = new Inventory { Capacity = 20, MaxWeight = 100f };
            var stock = new Inventory { Capacity = 20, MaxWeight = 100f };
            player.Add(food, 5);
            stock.Add(tool, 2);

            // Offer enough food value to cover tool
            var offers = new List<BarterLine> { new BarterLine(food, 2) };
            var asks = new List<BarterLine> { new BarterLine(tool, 1) };

            Assert.IsTrue(eco.IsFairTrade(offers, asks, FactionSO.Ids.ScavengerCamp, out float pv, out float fv));
            Assert.That(pv, Is.GreaterThanOrEqualTo(fv - 0.01f));

            Assert.IsTrue(eco.TryExecuteTrade(player, stock, offers, asks, FactionSO.Ids.ScavengerCamp));
            Assert.That(player.Count(tool), Is.EqualTo(1));
            Assert.That(player.Count(food), Is.EqualTo(3));
            Assert.That(stock.Count(food), Is.EqualTo(2));

            Object.DestroyImmediate(food);
            Object.DestroyImmediate(tool);
        }

        [Test]
        public void EconomyState_RoundTripsThroughSave()
        {
            var eco = MakeEconomy();
            eco.SetTrust(FactionSO.Ids.MilitaryRemnants, -12f);
            eco.AdjustDemand("anti_rad", 0.5f);

            var save = eco.CaptureState();
            var eco2 = MakeEconomy();
            eco2.RestoreState(save);

            Assert.That(eco2.GetTrust(FactionSO.Ids.MilitaryRemnants), Is.EqualTo(-12f).Within(Eps));
            Assert.That(eco2.GetDemandMultiplier("anti_rad"), Is.EqualTo(1.5f).Within(Eps));
        }

        [Test]
        public void WorldPhaseSystem_Day30_SetsFlashpoint_ForEconomy()
        {
            var phaseSys = new WorldPhaseSystem();
            WorldPhase observed = WorldPhase.PreWar;
            phaseSys.OnPhaseChanged += p => observed = p;

            phaseSys.OnDayTick(29);
            Assert.That(phaseSys.CurrentPhase, Is.EqualTo(WorldPhase.CivilWar));

            bool exchange = false;
            phaseSys.OnNuclearExchange += () => exchange = true;
            phaseSys.OnDayTick(30);
            Assert.That(phaseSys.CurrentPhase, Is.EqualTo(WorldPhase.Flashpoint));
            Assert.IsTrue(exchange);
            Assert.That(observed, Is.EqualTo(WorldPhase.Flashpoint));

            // Jewelry value under that phase
            var jewelry = MakeItem("jewelry", ItemType.Trade, 50f);
            Assert.That(TradeEconomy.GetEffectiveValue(jewelry, phaseSys.CurrentPhase), Is.EqualTo(0f));
            Object.DestroyImmediate(jewelry);
        }
    }
}
