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

        // -----------------------------------------------------------------
        // Concept 16 — Cult of the Glow (trust inversion)
        // -----------------------------------------------------------------

        [Test]
        public void CultOfTheGlow_DefaultFaction_HasTrustInversionFields()
        {
            var cult = _factions.Find(f => f.id == FactionSO.Ids.CultOfTheGlow);
            Assert.IsNotNull(cult, "CreateDefaultFactions must include cult_of_the_glow");
            Assert.IsTrue(cult.trustInversion);
            Assert.That(cult.healthyRadiationCeiling, Is.EqualTo(20f).Within(Eps));
            Assert.That(cult.highRadiationFloor, Is.EqualTo(60f).Within(Eps));
            Assert.That(cult.irradiatedWaterValueMultiplier, Is.EqualTo(12f).Within(Eps));
            Assert.That(cult.id, Is.EqualTo("cult_of_the_glow"));
        }

        [Test]
        public void TrustInversion_HealthyParty_IsHostile_HighRadParty_IsFriendly()
        {
            float partyRad = 10f; // healthy
            var eco = MakeEconomy();
            eco.SetPartyRadiationProvider(() => partyRad);
            // Stored trust is mid-range; inversion must override it.
            eco.SetTrust(FactionSO.Ids.CultOfTheGlow, 0f);

            Assert.That(eco.GetEffectiveTrust(FactionSO.Ids.CultOfTheGlow),
                Is.EqualTo(DynamicEconomySystem.MinTrust).Within(Eps));
            Assert.That(eco.GetStance(FactionSO.Ids.CultOfTheGlow),
                Is.EqualTo(TradeStance.HostileRaid));
            Assert.IsFalse(eco.WillTrade(FactionSO.Ids.CultOfTheGlow));

            partyRad = 75f; // highly irradiated
            Assert.That(eco.GetEffectiveTrust(FactionSO.Ids.CultOfTheGlow),
                Is.EqualTo(DynamicEconomySystem.MaxTrust).Within(Eps));
            Assert.That(eco.GetStance(FactionSO.Ids.CultOfTheGlow),
                Is.EqualTo(TradeStance.ShareIntel));
            Assert.IsTrue(eco.WillTrade(FactionSO.Ids.CultOfTheGlow));

            // Non-inversion factions ignore the rad provider.
            eco.SetTrust(FactionSO.Ids.ScavengerCamp, 0f);
            partyRad = 10f;
            Assert.That(eco.GetEffectiveTrust(FactionSO.Ids.ScavengerCamp),
                Is.EqualTo(0f).Within(Eps));
            Assert.That(eco.GetStance(FactionSO.Ids.ScavengerCamp),
                Is.EqualTo(TradeStance.Trade));
        }

        [Test]
        public void TrustInversion_ValuesIrradiatedWaterHeavily_VsNormalFaction()
        {
            float partyRad = 70f; // friendly with the cult so barter opens
            var eco = MakeEconomy();
            eco.SetPartyRadiationProvider(() => partyRad);
            eco.SetTrust(FactionSO.Ids.CultOfTheGlow, 0f);
            eco.SetTrust(FactionSO.Ids.ScavengerCamp, 0f);

            var glowWater = MakeItem("irradiated_water", ItemType.IrradiatedWater, 2f);
            var cleanWater = MakeItem("clean_water", ItemType.Water, 15f);

            float cultIrrad = eco.GetBarterUnitValue(
                glowWater, FactionSO.Ids.CultOfTheGlow, playerSelling: true);
            float scavIrrad = eco.GetBarterUnitValue(
                glowWater, FactionSO.Ids.ScavengerCamp, playerSelling: true);
            float cultClean = eco.GetBarterUnitValue(
                cleanWater, FactionSO.Ids.CultOfTheGlow, playerSelling: true);

            // Cult multiplies irradiated water by SO multiplier (12×) then trust factor.
            // At MaxTrust (high rad), sell factor = 1 + 0.3 = 1.3.
            float expectedCult = 2f * 12f * 1.3f;
            Assert.That(cultIrrad, Is.EqualTo(expectedCult).Within(Eps),
                "Cult of the Glow must value irradiated water heavily");
            Assert.That(cultIrrad, Is.GreaterThan(scavIrrad * 5f),
                "Cult irradiated-water price must dwarf normal-faction price");
            // Clean water is not inverted-prized — no special multiplier.
            Assert.That(cultClean, Is.LessThan(cultIrrad),
                "Irradiated water should outprice clean water at the cult table");

            Object.DestroyImmediate(glowWater);
            Object.DestroyImmediate(cleanWater);
        }

        [Test]
        public void TrustInversion_WithoutRadiationProvider_FallsBackToStoredTrust()
        {
            var eco = MakeEconomy();
            // No SetPartyRadiationProvider — inversion inactive for disposition.
            eco.SetTrust(FactionSO.Ids.CultOfTheGlow, 50f);
            Assert.That(eco.GetEffectiveTrust(FactionSO.Ids.CultOfTheGlow),
                Is.EqualTo(50f).Within(Eps));
            Assert.That(eco.GetStance(FactionSO.Ids.CultOfTheGlow),
                Is.EqualTo(TradeStance.ShareIntel));
        }

        [Test]
        public void TrustInversion_RadDropAcrossHealthyCeiling_LaunchesRaid_WithoutModifyTrust()
        {
            // Cult + rad 70 → 10 crosses healthyRadiationCeiling (20) downward
            // → TryLaunchRaid once. No ModifyTrust call.
            var shelter = new Shelter();
            shelter.AddModule(new ShelterModuleInstance("air_filtration", 1) { FilterHealth = 100f });
            shelter.AddModule(new ShelterModuleInstance("radiation_shielding", 1));

            float partyRad = 70f;
            var eco = MakeEconomy(shelter);
            // Post-activation so cult raids are live (no day provider = active; set explicitly).
            eco.SetDayProvider(() => DynamicEconomySystem.CultActivationDay);
            eco.SetPartyRadiationProvider(() => partyRad);
            eco.SetTrust(FactionSO.Ids.CultOfTheGlow, 0f); // stored trust unused while rad-driven

            int raidLaunches = 0;
            FactionRaidResult lastRaid = null;
            eco.OnRaidResolved += r =>
            {
                lastRaid = r;
                if (r != null && r.Launched) raidLaunches++;
            };

            // Seed baseline at high rad (friendly) — must not raid.
            eco.NotifyPartyRadiationChanged();
            Assert.That(raidLaunches, Is.EqualTo(0), "First radiation sample only seeds baseline");
            Assert.That(eco.GetStance(FactionSO.Ids.CultOfTheGlow),
                Is.EqualTo(TradeStance.ShareIntel));

            // Cross ceiling downward: 70 → 10 (ceiling default 20).
            partyRad = 10f;
            eco.NotifyPartyRadiationChanged();

            Assert.That(raidLaunches, Is.EqualTo(1),
                "Crossing healthyRadiationCeiling downward must TryLaunchRaid once");
            Assert.IsNotNull(lastRaid);
            Assert.That(lastRaid.FactionId, Is.EqualTo(FactionSO.Ids.CultOfTheGlow));
            Assert.IsTrue(lastRaid.Launched);
            Assert.That(eco.GetStance(FactionSO.Ids.CultOfTheGlow),
                Is.EqualTo(TradeStance.HostileRaid));

            // Stay healthy: further notifies must not re-fire the cascade.
            eco.NotifyPartyRadiationChanged();
            Assert.That(raidLaunches, Is.EqualTo(1),
                "Raid cascade fires only on the downward ceiling cross, not every sample");

            // Climb back above ceiling then drop again → second launch.
            partyRad = 50f;
            eco.NotifyPartyRadiationChanged();
            Assert.That(raidLaunches, Is.EqualTo(1));
            partyRad = 5f;
            eco.NotifyPartyRadiationChanged();
            Assert.That(raidLaunches, Is.EqualTo(2),
                "A new downward ceiling cross must launch again");
        }

        // -----------------------------------------------------------------
        // Concept 16 polish — hazmat contempt, ARS reverence, Day≥30 gate
        // -----------------------------------------------------------------

        [Test]
        public void CultPolish_HealthyWithIntactHazmat_FloorsTrust_AndRefusesTrade()
        {
            float partyRad = 10f; // healthy
            bool intactHazmat = true;
            var eco = MakeEconomy();
            eco.SetPartyRadiationProvider(() => partyRad);
            eco.SetPartyIntactHazmatProvider(() => intactHazmat);
            // Stored trust high — contempt must still floor disposition.
            eco.SetTrust(FactionSO.Ids.CultOfTheGlow, 80f);

            Assert.That(eco.GetEffectiveTrust(FactionSO.Ids.CultOfTheGlow),
                Is.EqualTo(DynamicEconomySystem.MinTrust).Within(Eps));
            Assert.That(eco.GetStance(FactionSO.Ids.CultOfTheGlow),
                Is.EqualTo(TradeStance.HostileRaid));
            Assert.IsFalse(eco.WillTrade(FactionSO.Ids.CultOfTheGlow));

            // Damaged / unequipped suit: healthy still hostile via rad inversion,
            // but without hazmat the path is pure rad (still MinTrust at 10).
            intactHazmat = false;
            Assert.That(eco.GetEffectiveTrust(FactionSO.Ids.CultOfTheGlow),
                Is.EqualTo(DynamicEconomySystem.MinTrust).Within(Eps));
        }

        [Test]
        public void CultPolish_IntactHazmat_WithoutRadProvider_FloorsEvenHighStoredTrust()
        {
            var eco = MakeEconomy();
            // No radiation provider — stored trust would normally drive stance.
            eco.SetTrust(FactionSO.Ids.CultOfTheGlow, 90f);
            eco.SetPartyIntactHazmatProvider(() => true);

            Assert.That(eco.GetEffectiveTrust(FactionSO.Ids.CultOfTheGlow),
                Is.EqualTo(DynamicEconomySystem.MinTrust).Within(Eps),
                "Sealed suit alone is heresy when the cult cannot read the glow");
            Assert.IsFalse(eco.WillTrade(FactionSO.Ids.CultOfTheGlow));
        }

        [Test]
        public void CultPolish_PartyArs_ForcesMaxTrust_AndBuyDiscountOnHighValueGear()
        {
            float partyRad = 5f; // would be healthy/hostile without ARS
            bool hasArs = true;
            var eco = MakeEconomy();
            eco.SetPartyRadiationProvider(() => partyRad);
            eco.SetPartyHasArsProvider(() => hasArs);
            eco.SetTrust(FactionSO.Ids.CultOfTheGlow, -50f);

            Assert.That(eco.GetEffectiveTrust(FactionSO.Ids.CultOfTheGlow),
                Is.EqualTo(DynamicEconomySystem.MaxTrust).Within(Eps),
                "Any party ARS must revere to MaxTrust");
            Assert.That(eco.GetStance(FactionSO.Ids.CultOfTheGlow),
                Is.EqualTo(TradeStance.ShareIntel));
            Assert.IsTrue(eco.WillTrade(FactionSO.Ids.CultOfTheGlow));

            var hazmat = MakeItem("hazmat_suit", ItemType.Protective, 40f);
            var food = MakeItem("canned_food", ItemType.Food, 10f);

            // Buy (playerSelling: false): MaxTrust factor = 1 - 0.25 = 0.75, then ARS discount.
            float buyProtective = eco.GetBarterUnitValue(
                hazmat, FactionSO.Ids.CultOfTheGlow, playerSelling: false);
            float expectedProtective = 40f * 0.75f * (1f - DynamicEconomySystem.ArsReverenceBuyDiscount);
            Assert.That(buyProtective, Is.EqualTo(expectedProtective).Within(Eps),
                "ARS reverence must steeply discount high-value gear buys");

            float buyFood = eco.GetBarterUnitValue(
                food, FactionSO.Ids.CultOfTheGlow, playerSelling: false);
            // CivilWar food phase mult (2.5×) then MaxTrust buy factor — no ARS discount.
            float expectedFood = 10f * TradeEconomy.PreFlashpointFoodMultiplier * 0.75f;
            Assert.That(buyFood, Is.EqualTo(expectedFood).Within(Eps),
                "Non high-value items must not receive ARS buy discount");

            // Sell path unchanged by ARS discount constant.
            float sellProtective = eco.GetBarterUnitValue(
                hazmat, FactionSO.Ids.CultOfTheGlow, playerSelling: true);
            Assert.That(sellProtective, Is.EqualTo(40f * 1.3f).Within(Eps));

            // ARS outranks hazmat contempt.
            eco.SetPartyIntactHazmatProvider(() => true);
            Assert.That(eco.GetEffectiveTrust(FactionSO.Ids.CultOfTheGlow),
                Is.EqualTo(DynamicEconomySystem.MaxTrust).Within(Eps),
                "ARS reverence must outrank hazmat contempt");

            hasArs = false;
            Assert.That(eco.GetEffectiveTrust(FactionSO.Ids.CultOfTheGlow),
                Is.EqualTo(DynamicEconomySystem.MinTrust).Within(Eps),
                "Without ARS, healthy + hazmat floors again");

            Object.DestroyImmediate(hazmat);
            Object.DestroyImmediate(food);
        }

        [Test]
        public void CultPolish_DayGate_InactiveBeforeDay30_ActiveAtAndAfter()
        {
            int day = 10;
            float partyRad = 80f; // would be friendly if active
            var eco = MakeEconomy();
            eco.SetDayProvider(() => day);
            eco.SetPartyRadiationProvider(() => partyRad);
            eco.SetTrust(FactionSO.Ids.CultOfTheGlow, 0f);

            Assert.IsFalse(eco.IsFactionActive(FactionSO.Ids.CultOfTheGlow),
                "Cult must be inactive before Day 30");
            Assert.That(eco.GetStance(FactionSO.Ids.CultOfTheGlow),
                Is.EqualTo(TradeStance.Refuse));
            Assert.IsFalse(eco.WillTrade(FactionSO.Ids.CultOfTheGlow));
            // Non-cult factions ignore the day gate.
            Assert.IsTrue(eco.IsFactionActive(FactionSO.Ids.ScavengerCamp));

            day = DynamicEconomySystem.CultActivationDay;
            Assert.IsTrue(eco.IsFactionActive(FactionSO.Ids.CultOfTheGlow));
            Assert.That(eco.GetStance(FactionSO.Ids.CultOfTheGlow),
                Is.EqualTo(TradeStance.ShareIntel),
                "At Day 30 high-rad party must open ShareIntel with the cult");
            Assert.IsTrue(eco.WillTrade(FactionSO.Ids.CultOfTheGlow));

            day = 45;
            Assert.IsTrue(eco.IsFactionActive(FactionSO.Ids.CultOfTheGlow));
        }

        [Test]
        public void CultPolish_PreActivation_BlocksRaidEvenWhenHostile()
        {
            var shelter = new Shelter();
            shelter.AddModule(new ShelterModuleInstance("air_filtration", 1) { FilterHealth = 100f });

            float partyRad = 5f; // hostile under inversion
            int day = 12;
            var eco = MakeEconomy(shelter);
            eco.SetDayProvider(() => day);
            eco.SetPartyRadiationProvider(() => partyRad);
            eco.SetTrust(FactionSO.Ids.CultOfTheGlow, 0f);

            var raid = eco.TryLaunchRaid(FactionSO.Ids.CultOfTheGlow, ignoreDayGate: true);
            Assert.IsFalse(raid.Launched, "Pre-Day-30 cult must not launch hatch raids");
            Assert.That(raid.Message, Does.Contain("not active").IgnoreCase);

            day = DynamicEconomySystem.CultActivationDay;
            raid = eco.TryLaunchRaid(FactionSO.Ids.CultOfTheGlow, ignoreDayGate: true);
            Assert.IsTrue(raid.Launched, "Day ≥ 30 cult may raid when hostile");
        }

        [Test]
        public void CultPolish_IsArsReverenceHighValueItem_MatchesSpecTypes()
        {
            var items = new List<ItemDefinition>
            {
                MakeItem("p", ItemType.Protective, 1f),
                MakeItem("w", ItemType.Weapon, 1f),
                MakeItem("m", ItemType.Medical, 1f),
                MakeItem("a", ItemType.AntiRad, 1f),
                MakeItem("d", ItemType.Device, 1f),
                MakeItem("t", ItemType.Tool, 1f),
                MakeItem("f", ItemType.Food, 1f),
            };
            Assert.IsTrue(DynamicEconomySystem.IsArsReverenceHighValueItem(items[0]));
            Assert.IsTrue(DynamicEconomySystem.IsArsReverenceHighValueItem(items[1]));
            Assert.IsTrue(DynamicEconomySystem.IsArsReverenceHighValueItem(items[2]));
            Assert.IsTrue(DynamicEconomySystem.IsArsReverenceHighValueItem(items[3]));
            Assert.IsTrue(DynamicEconomySystem.IsArsReverenceHighValueItem(items[4]));
            Assert.IsTrue(DynamicEconomySystem.IsArsReverenceHighValueItem(items[5]));
            Assert.IsFalse(DynamicEconomySystem.IsArsReverenceHighValueItem(items[6]));
            Assert.IsFalse(DynamicEconomySystem.IsArsReverenceHighValueItem(null));
            for (int i = 0; i < items.Count; i++)
                Object.DestroyImmediate(items[i]);
        }
    }
}
