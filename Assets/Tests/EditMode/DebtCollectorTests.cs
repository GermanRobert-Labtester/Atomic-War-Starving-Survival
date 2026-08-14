using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Events;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;
using Ashfall.Core;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Prompt #18 — Debt Collector: day+20 after faction dig-out, demand half
    /// fuel + half clean water; pay clears debt; refuse sabotages catchment
    /// and cuts antenna (no combat).
    /// </summary>
    [TestFixture]
    public class DebtCollectorTests
    {
        private const float Eps = 1e-3f;

        private List<FactionSO> _factions;
        private List<Object> _toDestroy;
        private WorldPhase _phase;
        private int _day;

        [SetUp]
        public void SetUp()
        {
            _phase = WorldPhase.NuclearWinter;
            _day = 10;
            _factions = DynamicEconomySystem.CreateDefaultFactions();
            _toDestroy = new List<Object>();
            for (int i = 0; i < _factions.Count; i++)
                _toDestroy.Add(_factions[i]);
        }

        [TearDown]
        public void TearDown()
        {
            if (_toDestroy == null) return;
            for (int i = 0; i < _toDestroy.Count; i++)
            {
                if (_toDestroy[i] != null)
                    Object.DestroyImmediate(_toDestroy[i]);
            }
            _toDestroy = null;
            _factions = null;
        }

        private DynamicEconomySystem MakeEconomy()
        {
            var eco = new DynamicEconomySystem(() => _phase, null, new System.Random(1));
            eco.SetDayProvider(() => _day);
            for (int i = 0; i < _factions.Count; i++)
                eco.RegisterFaction(_factions[i]);
            return eco;
        }

        private FactionRadioInterceptSystem MakeRadio(DynamicEconomySystem eco)
        {
            var radio = new FactionRadioInterceptSystem();
            radio.Bind(eco, () => _day);
            return radio;
        }

        private ItemDefinition MakeFuelItem(string id = "fuel_can")
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = id;
            item.displayName = "Fuel Can";
            item.type = ItemType.Fuel;
            item.stackMax = 50;
            item.weight = 0.5f;
            _toDestroy.Add(item);
            return item;
        }

        private (DebtCollectorSystem sys, Shelter shelter, WaterStorage water, Inventory inv, RadioState radio)
            MakeStack(DynamicEconomySystem eco = null, FactionRadioInterceptSystem intercepts = null)
        {
            eco = eco ?? MakeEconomy();
            intercepts = intercepts ?? MakeRadio(eco);

            var shelter = new Shelter();
            var heater = new ShelterModuleInstance(DebtCollectorSystem.HeaterModuleId, 1) { Fuel = 20f };
            var radioMod = new ShelterModuleInstance(DebtCollectorSystem.RadioModuleId, 1) { Fuel = 10f };
            var catchment = new ShelterModuleInstance(DebtCollectorSystem.CatchmentModuleId, 1)
            {
                FilterHealth = 100f,
                IsEnabled = true
            };
            shelter.AddModule(heater);
            shelter.AddModule(radioMod);
            shelter.AddModule(catchment);

            var water = new WaterStorage { CleanWater = 40f };
            var inv = new Inventory { Capacity = 40, MaxWeight = 200f };
            inv.Add(MakeFuelItem(), 10);

            var radioState = new RadioState
            {
                AvailableFuel = 10f,
                EmpDamage = 0f,
                PowerConsumptionPerHour = 0.5f
            };

            var sys = new DebtCollectorSystem();
            sys.Bind(
                eco,
                intercepts,
                getDay: () => _day,
                shelter: shelter,
                water: water,
                inventory: inv,
                radioState: radioState);

            return (sys, shelter, water, inv, radioState);
        }

        [Test]
        public void ScheduleDebt_SetsCollectorDayPlus20()
        {
            var (sys, _, _, _, _) = MakeStack();
            var entry = sys.ScheduleDebt(FactionSO.Ids.MilitaryRemnants, scheduleDay: 10);

            Assert.IsNotNull(entry);
            Assert.That(entry.ScheduledDay, Is.EqualTo(10));
            Assert.That(entry.CollectorDay, Is.EqualTo(10 + DebtCollectorSystem.CollectionDelayDays));
            Assert.That(entry.CollectorDay, Is.EqualTo(30));
            Assert.IsFalse(entry.Resolved);
            Assert.IsFalse(entry.Presented);
            Assert.IsTrue(sys.HasPendingDebtFor(FactionSO.Ids.MilitaryRemnants));
        }

        [Test]
        public void TickDay_BeforeCollectorDay_DoesNotPresent()
        {
            var (sys, _, _, _, _) = MakeStack();
            int arrived = 0;
            sys.OnCollectorArrived += (_, __) => arrived++;

            sys.ScheduleDebt(FactionSO.Ids.ScavengerCamp, scheduleDay: 10);
            for (int d = 10; d < 30; d++)
            {
                _day = d;
                sys.TickDay(d);
            }

            Assert.That(arrived, Is.EqualTo(0));
            Assert.IsNull(sys.ActiveCollection);
        }

        [Test]
        public void TickDay_OnCollectorDay_PresentsPayRefuseEventAndDemandSnapshot()
        {
            var (sys, shelter, water, inv, _) = MakeStack();
            DebtEntry arrivedDebt = null;
            GameEvent arrivedEv = null;
            sys.OnCollectorArrived += (d, ev) =>
            {
                arrivedDebt = d;
                arrivedEv = ev;
            };

            float fuelBefore = DebtCollectorSystem.MeasureTotalFuel(shelter, inv);
            float waterBefore = water.CleanWater;
            Assert.That(fuelBefore, Is.EqualTo(40f).Within(Eps)); // 20 heater + 10 radio + 10 inv

            var entry = sys.ScheduleDebt(FactionSO.Ids.MilitaryRemnants, scheduleDay: 10);
            _day = entry.CollectorDay;
            sys.TickDay(_day);

            Assert.IsNotNull(arrivedDebt);
            Assert.IsNotNull(arrivedEv);
            Assert.IsTrue(arrivedDebt.Presented);
            Assert.That(arrivedDebt.FuelDemanded, Is.EqualTo(fuelBefore * 0.5f).Within(Eps));
            Assert.That(arrivedDebt.WaterDemanded, Is.EqualTo(waterBefore * 0.5f).Within(Eps));
            Assert.That(arrivedEv.id, Does.StartWith(DebtCollectorSystem.EventIdPrefix));
            Assert.That(arrivedEv.choices.Count, Is.EqualTo(2));
            Assert.That(arrivedEv.choices.Exists(c => c.ChoiceId == DebtCollectorSystem.ChoicePay));
            Assert.That(arrivedEv.choices.Exists(c => c.ChoiceId == DebtCollectorSystem.ChoiceRefuse));
            Assert.IsNotNull(sys.ActiveCollection);

            Object.DestroyImmediate(arrivedEv);
        }

        [Test]
        public void Pay_ConsumesHalfFuelAndWater_ClearsDebt_RecoversTrust()
        {
            var eco = MakeEconomy();
            var (sys, shelter, water, inv, _) = MakeStack(eco);
            eco.SetTrust(FactionSO.Ids.MilitaryRemnants, 40f);
            float trustBefore = eco.GetTrust(FactionSO.Ids.MilitaryRemnants);

            var entry = sys.ScheduleDebt(FactionSO.Ids.MilitaryRemnants, scheduleDay: 10);
            _day = entry.CollectorDay;
            var ev = sys.PresentCollector(entry);
            Assert.IsNotNull(ev);

            float fuelBefore = DebtCollectorSystem.MeasureTotalFuel(shelter, inv);
            float waterBefore = water.CleanWater;
            float fuelDemand = entry.FuelDemanded;
            float waterDemand = entry.WaterDemanded;

            var flags = new EventContext { CurrentDay = _day };
            flags.SetEventFlag(HatchEntrapmentSystem.FlagFactionDigOutDebt, true);

            Assert.IsTrue(sys.TryPay(entry, flags));
            Assert.IsTrue(entry.Resolved);
            Assert.IsTrue(entry.Paid);
            Assert.IsFalse(entry.Refused);

            float fuelAfter = DebtCollectorSystem.MeasureTotalFuel(shelter, inv);
            Assert.That(fuelAfter, Is.EqualTo(fuelBefore - fuelDemand).Within(0.5f));
            Assert.That(water.CleanWater, Is.EqualTo(waterBefore - waterDemand).Within(Eps));
            Assert.That(eco.GetTrust(FactionSO.Ids.MilitaryRemnants),
                Is.EqualTo(trustBefore + DebtCollectorSystem.PayTrustRecover).Within(Eps));
            Assert.IsFalse(flags.HasEventFlag(HatchEntrapmentSystem.FlagFactionDigOutDebt),
                "Paying should clear the short-term dig-out debt flag.");
            Assert.IsFalse(sys.HasPendingDebtFor(FactionSO.Ids.MilitaryRemnants));
            Assert.IsNull(sys.ActiveCollection);

            Object.DestroyImmediate(ev);
        }

        [Test]
        public void Refuse_SabotagesCatchmentAndCutsAntenna_NoCombat()
        {
            var (sys, shelter, water, inv, radio) = MakeStack();
            float heaterFuelBefore = shelter.GetModule(DebtCollectorSystem.HeaterModuleId).Fuel;
            int invFuelBefore = inv.CountByType(ItemType.Fuel);
            float waterBefore = water.CleanWater;

            var entry = sys.ScheduleDebt(FactionSO.Ids.ScavengerCamp, scheduleDay: 5);
            _day = entry.CollectorDay;
            var ev = sys.PresentCollector(entry);

            var flags = new EventContext { CurrentDay = _day };
            Assert.IsTrue(sys.TryRefuse(entry, flags));
            Assert.IsTrue(entry.Resolved);
            Assert.IsTrue(entry.Refused);
            Assert.IsFalse(entry.Paid);

            // Refuse is not a resource payment: heater + inventory fuel and cistern stay.
            // Antenna cut may zero radio-module fuel as part of the sabotage.
            Assert.That(shelter.GetModule(DebtCollectorSystem.HeaterModuleId).Fuel,
                Is.EqualTo(heaterFuelBefore).Within(Eps));
            Assert.That(inv.CountByType(ItemType.Fuel), Is.EqualTo(invFuelBefore));
            Assert.That(water.CleanWater, Is.EqualTo(waterBefore).Within(Eps));

            Assert.IsTrue(flags.HasEventFlag(DebtCollectorSystem.FlagCatchmentSabotaged));
            Assert.IsTrue(flags.HasEventFlag(DebtCollectorSystem.FlagAntennaCut));
            Assert.IsTrue(DebtCollectorSystem.IsCatchmentSabotaged(shelter));
            Assert.IsTrue(DebtCollectorSystem.IsAntennaCut(shelter, radio));

            var catchment = shelter.GetModule(DebtCollectorSystem.CatchmentModuleId);
            Assert.IsNotNull(catchment);
            Assert.IsFalse(catchment.IsEnabled);
            Assert.That(catchment.FilterHealth, Is.EqualTo(0f).Within(Eps));

            var radioMod = shelter.GetModule(DebtCollectorSystem.RadioModuleId);
            Assert.IsNotNull(radioMod);
            Assert.IsFalse(radioMod.IsEnabled);
            Assert.That(radioMod.Fuel, Is.EqualTo(0f).Within(Eps));
            Assert.That(radio.EmpDamage, Is.GreaterThanOrEqualTo(100f));
            Assert.IsFalse(radio.IsOperational);

            Object.DestroyImmediate(ev);
        }

        [Test]
        public void ApplyChoiceFromEvent_PayAndRefuse_RouteCorrectly()
        {
            var (sys, _, _, _, _) = MakeStack();
            var payDebt = sys.ScheduleDebt(FactionSO.Ids.MilitaryRemnants, scheduleDay: 1);
            _day = payDebt.CollectorDay;
            var payEv = sys.PresentCollector(payDebt);
            Assert.IsTrue(sys.ApplyChoiceFromEvent(payEv,
                payEv.choices.Find(c => c.ChoiceId == DebtCollectorSystem.ChoicePay)));
            Assert.IsTrue(payDebt.Paid);

            var refuseDebt = sys.ScheduleDebt(FactionSO.Ids.ScavengerCamp, scheduleDay: 1);
            _day = refuseDebt.CollectorDay;
            var refuseEv = sys.PresentCollector(refuseDebt);
            Assert.IsTrue(sys.ApplyChoiceFromEvent(refuseEv,
                refuseEv.choices.Find(c => c.ChoiceId == DebtCollectorSystem.ChoiceRefuse)));
            Assert.IsTrue(refuseDebt.Refused);

            Object.DestroyImmediate(payEv);
            Object.DestroyImmediate(refuseEv);
        }

        [Test]
        public void CaptureRestore_PreservesPendingAndResolvedDebts()
        {
            var (sys, _, _, _, _) = MakeStack();
            var pending = sys.ScheduleDebt(FactionSO.Ids.MilitaryRemnants, scheduleDay: 10);
            var resolved = sys.ScheduleDebt(FactionSO.Ids.ScavengerCamp, scheduleDay: 1);
            _day = resolved.CollectorDay;
            sys.PresentCollector(resolved);
            sys.TryRefuse(resolved);

            var save = sys.CaptureState();
            Assert.That(save.Debts.Length, Is.EqualTo(2));

            var (sys2, _, _, _, _) = MakeStack();
            sys2.RestoreState(save);

            Assert.That(sys2.Debts.Count, Is.EqualTo(2));
            var p = sys2.FindDebt(pending.Id);
            var r = sys2.FindDebt(resolved.Id);
            Assert.IsNotNull(p);
            Assert.IsNotNull(r);
            Assert.IsFalse(p.Resolved);
            Assert.That(p.CollectorDay, Is.EqualTo(30));
            Assert.IsTrue(r.Resolved);
            Assert.IsTrue(r.Refused);
        }

        [Test]
        public void HatchRescue_OnFactionRescueApplied_SchedulesDebt()
        {
            var (sys, _, _, _, _) = MakeStack();
            var hatch = new HatchEntrapmentSystem();
            hatch.OnFactionRescueApplied += factionId =>
            {
                if (!string.IsNullOrEmpty(factionId))
                    sys.ScheduleDebt(factionId);
            };

            // Inject buried hatch + scheduled rescuer via save restore (public API).
            hatch.RestoreState(new HatchEntrapmentSave
            {
                State = HatchState.Buried,
                ContinuousHazardHours = HatchEntrapmentSystem.HazardHoursToSeal,
                LastHazardWeather = WeatherKind.Blizzard,
                BuriedEventFired = true,
                FactionRescueScheduled = true,
                FactionRescueFactionId = FactionSO.Ids.MilitaryRemnants
            });

            Assert.IsTrue(hatch.ApplyFactionRescue());
            Assert.IsTrue(sys.HasPendingDebtFor(FactionSO.Ids.MilitaryRemnants));
            var debt = sys.Debts[0];
            Assert.That(debt.CollectorDay, Is.EqualTo(_day + DebtCollectorSystem.CollectionDelayDays));
            Assert.That(debt.FactionId, Is.EqualTo(FactionSO.Ids.MilitaryRemnants));
        }

        [Test]
        public void MeasureTotalFuel_SumsHeaterRadioAndInventory()
        {
            var shelter = new Shelter();
            shelter.AddModule(new ShelterModuleInstance("heater", 1) { Fuel = 5f });
            shelter.AddModule(new ShelterModuleInstance("radio", 1) { Fuel = 3f });
            var inv = new Inventory { Capacity = 20, MaxWeight = 100f };
            inv.Add(MakeFuelItem(), 4);

            Assert.That(DebtCollectorSystem.MeasureTotalFuel(shelter, inv), Is.EqualTo(12f).Within(Eps));
        }

        [Test]
        public void ConsumeFuel_DrainsHeaterThenRadioThenInventory()
        {
            var shelter = new Shelter();
            var heater = new ShelterModuleInstance("heater", 1) { Fuel = 5f };
            var radio = new ShelterModuleInstance("radio", 1) { Fuel = 3f };
            shelter.AddModule(heater);
            shelter.AddModule(radio);
            var inv = new Inventory { Capacity = 20, MaxWeight = 100f };
            inv.Add(MakeFuelItem(), 4);

            float taken = DebtCollectorSystem.ConsumeFuel(10f, shelter, inv);
            Assert.That(taken, Is.EqualTo(10f).Within(0.5f));
            Assert.That(heater.Fuel, Is.EqualTo(0f).Within(Eps));
            Assert.That(radio.Fuel, Is.EqualTo(0f).Within(Eps));
            // 5 + 3 = 8 from modules; 2 more from inventory (ceil).
            Assert.That(inv.CountByType(ItemType.Fuel), Is.LessThanOrEqualTo(2));
        }
    }
}
