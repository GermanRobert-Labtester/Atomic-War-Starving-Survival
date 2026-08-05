using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Crafting;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;
using InventoryClass = AtomicWar._Game.Inventory.Inventory;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Prompts #195–#200 — shelter-engineering milestone perks.
    /// </summary>
    [TestFixture]
    public class ShelterPerkTests
    {
        private SkillProgressionSystem _progression;
        private ShelterPerkSystem _perks;
        private Survivor _sv;
        private InventoryClass _inv;

        [SetUp]
        public void SetUp()
        {
            _progression = new SkillProgressionSystem();
            _progression.RegisterDefaultPerks();
            _perks = new ShelterPerkSystem();
            _perks.Bind(_progression);

            _sv = MakeSurvivor("sv_eng", "Engineer");
            _inv = new InventoryClass { Capacity = 80 };
        }

        private static Survivor MakeSurvivor(string id, string name)
        {
            var sv = new Survivor
            {
                Id = id,
                DisplayName = name,
                State = SurvivorState.Idle
            };
            sv.Needs.Morale = 50f;
            sv.Needs.Health = 100f;
            sv.Needs.Fatigue = 10f;
            return sv;
        }

        private static ItemDefinition MakeItem(string id, ItemType type = ItemType.Material)
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = id;
            item.displayName = id;
            item.type = type;
            item.stackMax = 99;
            item.weight = 0.2f;
            item.disassembleYieldFraction = 0.5f;
            return item;
        }

        // ── #195 Jury-Rigger ─────────────────────────────────────────────

        [Test]
        public void JuryRigger_EarnedAfter5JuryRigOrOverclock()
        {
            Assert.IsFalse(_perks.Has(_sv, ShelterPerkSystem.JuryRiggerId));
            for (int i = 0; i < ShelterPerkSystem.JuryRigsForJuryRigger - 1; i++)
                _perks.RecordJuryRigOrOverclock(_sv, 1);
            Assert.IsFalse(_perks.Has(_sv, ShelterPerkSystem.JuryRiggerId));

            _perks.RecordJuryRigOrOverclock(_sv, 1);
            Assert.IsTrue(_perks.Has(_sv, ShelterPerkSystem.JuryRiggerId));
            Assert.IsTrue(_perks.CanSubstituteScrap(_sv));
        }

        [Test]
        public void JuryRigger_ScrapSubstitutePair_MechanicalPartsAndElectronicScrap()
        {
            Assert.IsTrue(ShelterPerkSystem.IsScrapSubstitutePair(
                ShelterPerkSystem.ElectronicScrapId, ShelterPerkSystem.MechanicalPartsId));
            Assert.IsTrue(ShelterPerkSystem.IsScrapSubstitutePair(
                ShelterPerkSystem.MechanicalPartsId, ShelterPerkSystem.ElectronicScrapId));
            Assert.IsFalse(ShelterPerkSystem.IsScrapSubstitutePair(
                ShelterPerkSystem.ElectronicScrapId, ShelterPerkSystem.WoodId));
            Assert.AreEqual(
                ShelterPerkSystem.MechanicalPartsId,
                ShelterPerkSystem.GetScrapSubstituteId(ShelterPerkSystem.ElectronicScrapId));
        }

        [Test]
        public void JuryRigger_WorkbenchRepair_SubstitutesScrap()
        {
            for (int i = 0; i < ShelterPerkSystem.JuryRigsForJuryRigger; i++)
                _perks.RecordJuryRigOrOverclock(_sv, 1);

            var eScrap = MakeItem(ScrapMaterialIds.ElectronicScrap);
            var mParts = MakeItem(ScrapMaterialIds.MechanicalParts);
            var radio = MakeItem("radio", ItemType.Device);
            radio.durability = 100f;

            // Only mechanical parts — not electronic scrap required by default device recipe.
            _inv.Add(mParts, 5);
            _inv.Add(radio, 1);
            var slot = _inv.FindSlot("radio");
            Assert.IsNotNull(slot);
            slot.Device = DeviceState.CreateDefault();
            slot.Device.Broken = true;
            slot.Device.Calibration = 0f;

            var shelter = new Shelter();
            shelter.AddModule(new ShelterModuleInstance("workbench", 1) { IsEnabled = true });
            var wb = new WorkbenchSystem(
                _inv,
                id =>
                {
                    if (id == ScrapMaterialIds.ElectronicScrap) return eScrap;
                    if (id == ScrapMaterialIds.MechanicalParts) return mParts;
                    if (id == "radio") return radio;
                    return null;
                },
                crafting: null,
                getShelter: () => shelter,
                getDay: () => 1);
            wb.BindShelterPerks(_perks, new System.Random(1));

            Assert.IsTrue(wb.CanRepair(slot, _sv),
                "Jury-Rigger should satisfy electronic_scrap cost with mechanical_parts");
            Assert.IsTrue(wb.Repair(slot, _sv));
            Assert.AreEqual(0, _inv.Count(eScrap));
            // Recipe: 2 electronic (sub) + 1 mechanical = 3 mechanical consumed.
            Assert.AreEqual(2, _inv.Count(mParts));
            Assert.IsFalse(slot.Device.Broken);
        }

        [Test]
        public void JuryRigSystem_RecordsOperatorTowardPerk()
        {
            var shelter = new Shelter();
            var mod = new ShelterModuleInstance("heater", 1)
            {
                IsEnabled = false,
                FilterHealth = 0f
            };
            shelter.AddModule(mod);

            var jury = new JuryRigSystem(new System.Random(52));
            jury.Bind(() => shelter);
            jury.BindShelterPerks(_perks, () => 3);

            Assert.IsTrue(jury.JuryRig("heater", _sv));
            Assert.AreEqual(1, _perks.GetCounters(_sv.Id).JuryRigActions);
            Assert.IsTrue(jury.IsJuryRigged("heater"));

            // Overclock alias on a second broken module
            var radio = new ShelterModuleInstance("radio", 1)
            {
                IsEnabled = false,
                FilterHealth = 0f
            };
            shelter.AddModule(radio);
            Assert.IsTrue(jury.Overclock("radio", _sv));
            Assert.AreEqual(2, _perks.GetCounters(_sv.Id).JuryRigActions);
        }

        // ── #196 Structural Engineer ─────────────────────────────────────

        [Test]
        public void StructuralEngineer_EarnedAfter10Struts_HalfWoodAndDoubleCeiling()
        {
            Assert.IsFalse(_perks.Has(_sv, ShelterPerkSystem.StructuralEngineerId));
            for (int i = 0; i < ShelterPerkSystem.StrutsForStructuralEngineer - 1; i++)
                _perks.RecordShoringStrutBuilt(_sv, 1, 1);
            Assert.IsFalse(_perks.Has(_sv, ShelterPerkSystem.StructuralEngineerId));
            _perks.RecordShoringStrutBuilt(_sv, 1, 1);
            Assert.IsTrue(_perks.IsStructuralEngineer(_sv));
            Assert.AreEqual(0.5f, _perks.GetShoringWoodCostMultiplier(_sv), 0.001f);
            Assert.AreEqual(2f, _perks.GetCeilingLoadMultiplier(_sv), 0.001f);
        }

        [Test]
        public void StructuralEngineer_InstallStrut_HalfWood_AndReinforcesRoom()
        {
            for (int i = 0; i < ShelterPerkSystem.StrutsForStructuralEngineer; i++)
                _perks.RecordShoringStrutBuilt(_sv, 1, 1);

            var wood = MakeItem(ShelterPerkSystem.WoodId);
            _inv.Add(wood, 20);

            var shelter = new Shelter();
            var ceiling = new CeilingCollapseSystem();
            ceiling.RegisterRoom("quarters", 5f); // needs strut without mult (5 > 3)

            var structural = new StructuralIntegritySystem();
            structural.Bind(() => shelter);
            structural.BindShelterPerks(_perks, () => 2, ceiling);

            Assert.IsTrue(ceiling.NeedsStrut("quarters", shelter));

            int cost = structural.GetShoringWoodCost(_sv);
            Assert.AreEqual(2, cost); // 4 * 0.5

            Assert.IsTrue(structural.TryInstallShoringStrut(_sv, _inv, wood, "quarters"));
            Assert.AreEqual(2f, ceiling.GetCeilingLoadMultiplier("quarters"), 0.001f);
            // 5 tiles with 2x capacity (6) no longer needs strut once level is enough —
            // with level 1 and capacity 6: Ceil(5/6)=1, so level 1 is enough.
            Assert.IsFalse(ceiling.NeedsStrut("quarters", shelter));
            Assert.AreEqual(18, _inv.Count(wood)); // spent 2
        }

        // ── #197 HVAC Technician ─────────────────────────────────────────

        [Test]
        public void HvacTech_EarnedAfter3StormFilterOps_BoostsVentilation()
        {
            for (int i = 0; i < ShelterPerkSystem.StormFilterOpsForHvac - 1; i++)
                _perks.RecordStormAirFilterOp(_sv, 1);
            Assert.IsFalse(_perks.Has(_sv, ShelterPerkSystem.HvacTechId));
            _perks.RecordStormAirFilterOp(_sv, 1);
            Assert.IsTrue(_perks.Has(_sv, ShelterPerkSystem.HvacTechId));

            var list = new List<Survivor> { _sv };
            Assert.AreEqual(1.3f, _perks.GetVentilationClearMultiplier(list), 0.001f);
        }

        [Test]
        public void HvacTech_VentilationClearsCo2AndMoldFaster()
        {
            for (int i = 0; i < ShelterPerkSystem.StormFilterOpsForHvac; i++)
                _perks.RecordStormAirFilterOp(_sv, 1);

            var room = new ShelterRoom("plant", null)
            {
                Co2Ppm = 100f,
                MoldLevel = 0.5f,
                HasMold = true
            };

            var atmo = new ShelterAtmosphereSystem(new System.Random(1));
            atmo.RegisterRoom(room);

            // Baseline clear 1 hour at 1.0
            var roomBase = new ShelterRoom("plant_b", null)
            {
                Co2Ppm = 100f,
                MoldLevel = 0.5f,
                HasMold = true
            };
            atmo.ApplyVentilationClear(roomBase, 1f, 1f);
            float baseCo2 = 100f - roomBase.Co2Ppm;
            float baseMold = 0.5f - roomBase.MoldLevel;

            float mult = _perks.GetVentilationClearMultiplier(new List<Survivor> { _sv });
            atmo.ApplyVentilationClear(room, 1f, mult);
            float techCo2 = 100f - room.Co2Ppm;
            float techMold = 0.5f - room.MoldLevel;

            Assert.Greater(techCo2, baseCo2 * 1.25f);
            Assert.Greater(techMold, baseMold * 1.25f);
        }

        // ── #198 Scrapper ────────────────────────────────────────────────

        [Test]
        public void Scrapper_EarnedAfter50Disassembles_CanRecoverRare()
        {
            for (int i = 0; i < ShelterPerkSystem.DisassemblesForScrapper - 1; i++)
                _perks.RecordDisassemble(_sv, 1, 1);
            Assert.IsFalse(_perks.Has(_sv, ShelterPerkSystem.ScrapperId));
            _perks.RecordDisassemble(_sv, 1, 1);
            Assert.IsTrue(_perks.CanRecoverRareComponents(_sv));
        }

        [Test]
        public void Scrapper_HighTierDisassemble_MayYieldBatteryOrSpring()
        {
            for (int i = 0; i < ShelterPerkSystem.DisassemblesForScrapper; i++)
                _perks.RecordDisassemble(_sv, 1, 1);

            Assert.IsTrue(ShelterPerkSystem.IsHighTierDisassembleTarget("geiger_counter"));
            Assert.IsTrue(ShelterPerkSystem.IsHighTierDisassembleTarget("hunting_rifle"));

            // Force rare recovery: roll until we get one with a fixed seed that hits.
            int hits = 0;
            var rng = new System.Random(42);
            for (int i = 0; i < 200; i++)
            {
                string rare = _perks.RollRareComponent(_sv, isHighTier: true, rng);
                if (rare != null)
                {
                    hits++;
                    Assert.IsTrue(rare == ShelterPerkSystem.BatteryId
                        || rare == ShelterPerkSystem.SpringId);
                }
            }
            Assert.Greater(hits, 10, "expected ~20% recovery over 200 rolls");

            Assert.IsNull(_perks.RollRareComponent(_sv, isHighTier: false, new System.Random(1)));
        }

        [Test]
        public void Scrapper_WorkbenchDisassemble_RecordsAndMayGrantRare()
        {
            for (int i = 0; i < ShelterPerkSystem.DisassemblesForScrapper; i++)
                _perks.RecordDisassemble(_sv, 1, 1);

            var eScrap = MakeItem(ScrapMaterialIds.ElectronicScrap);
            var mParts = MakeItem(ScrapMaterialIds.MechanicalParts);
            var battery = WorkbenchSystem.CreateBatteryDefinition();
            var spring = WorkbenchSystem.CreateSpringDefinition();
            var radio = MakeItem("shortwave_radio", ItemType.Device);
            radio.disassembleYieldFraction = 1f;

            _inv.Add(radio, 5);

            var shelter = new Shelter();
            shelter.AddModule(new ShelterModuleInstance("workbench", 1) { IsEnabled = true });
            var wb = new WorkbenchSystem(
                _inv,
                id =>
                {
                    if (id == ScrapMaterialIds.ElectronicScrap) return eScrap;
                    if (id == ScrapMaterialIds.MechanicalParts) return mParts;
                    if (id == ShelterPerkSystem.BatteryId) return battery;
                    if (id == ShelterPerkSystem.SpringId) return spring;
                    if (id == "shortwave_radio") return radio;
                    return null;
                },
                getShelter: () => shelter,
                getDay: () => 4);
            wb.BindShelterPerks(_perks, new System.Random(7));
            wb.SetRareComponentItems(battery, spring);

            int before = _perks.GetCounters(_sv.Id).Disassembles;
            Assert.IsTrue(wb.Disassemble(radio, _sv));
            Assert.AreEqual(before + 1, _perks.GetCounters(_sv.Id).Disassembles);
        }

        // ── #199 Sandhog ─────────────────────────────────────────────────

        [Test]
        public void Sandhog_EarnedAfter5Rooms_HalfFatigue_NoCaveIn()
        {
            for (int i = 0; i < ShelterPerkSystem.RoomsClearedForSandhog - 1; i++)
                _perks.RecordRoomCleared(_sv, 1);
            Assert.IsFalse(_perks.IsSandhog(_sv));
            _perks.RecordRoomCleared(_sv, 1);
            Assert.IsTrue(_perks.IsSandhog(_sv));
            Assert.AreEqual(0.5f, _perks.GetExcavationFatigueMultiplier(_sv), 0.001f);
            Assert.IsTrue(_perks.SuppressesCaveInWhileDigging(_sv));

            var rng = new System.Random(0);
            for (int i = 0; i < 50; i++)
                Assert.IsFalse(_perks.RollDigCaveIn(_sv, rng));
        }

        [Test]
        public void Sandhog_Excavation_AppliesHalfFatigueAndRecordsClear()
        {
            for (int i = 0; i < ShelterPerkSystem.RoomsClearedForSandhog; i++)
                _perks.RecordRoomCleared(_sv, 1);

            var excav = new ExcavationSystem(new System.Random(0));
            excav.BindShelterPerks(_perks, () => 1);
            // Clear rate ≈ (1/4)*1.5*1 = 0.375 units/call with shovel; seal tiny pile.
            excav.SealRoom("storage", 0.3f);

            float fatBefore = _sv.Needs.Fatigue;
            float cleared = excav.ClearRubble("storage", _sv, hasShovel: true, hatchBlocked: false);
            Assert.Greater(cleared, 0f);
            float fatGain = _sv.Needs.Fatigue - fatBefore;
            Assert.AreEqual(ExcavationSystem.FatiguePerHour * 0.5f, fatGain, 0.01f);
            Assert.IsTrue(excav.Rooms["storage"].IsCleared);
        }

        // ── #200 Thermodynamics ──────────────────────────────────────────

        [Test]
        public void Thermodynamics_EarnedAfter20WarmDays_FuelBurnsLonger()
        {
            var list = new List<Survivor> { _sv };
            for (int i = 0; i < ShelterPerkSystem.WarmDaysForThermodynamics - 1; i++)
                _perks.RecordWarmDay(16f, list, i + 1);
            Assert.IsFalse(_perks.HasThermodynamics(_sv));
            Assert.AreEqual(19, _perks.ConsecutiveWarmDays);

            _perks.RecordWarmDay(15f, list, 20);
            Assert.IsTrue(_perks.HasThermodynamics(_sv));
            Assert.AreEqual(0.8f, _perks.GetFuelBurnMultiplier(_sv), 0.001f);

            // Cold day resets streak (already granted stays).
            _perks.RecordWarmDay(10f, list, 21);
            Assert.AreEqual(0, _perks.ConsecutiveWarmDays);
            Assert.IsTrue(_perks.HasThermodynamics(_sv));
        }

        [Test]
        public void Thermodynamics_HeaterFuel_BurnsAt80PercentRate()
        {
            for (int i = 0; i < ShelterPerkSystem.WarmDaysForThermodynamics; i++)
                _perks.RecordWarmDay(20f, new List<Survivor> { _sv }, i + 1);

            var heater = new ShelterModuleInstance("heater", 1) { IsEnabled = true };
            heater.AddFuel(10f, _perks.GetFuelBurnMultiplier(_sv));
            Assert.AreEqual(0.8f, heater.FuelBurnMultiplier, 0.001f);

            heater.Tick(1f, null);
            // Default heater burn without SO is 1 fuel/hour * 0.8 mult
            Assert.AreEqual(9.2f, heater.Fuel, 0.01f);
        }

        // ── Save / load ──────────────────────────────────────────────────

        [Test]
        public void ShelterPerks_SaveRestore_RoundTrip()
        {
            for (int i = 0; i < 3; i++) _perks.RecordJuryRigOrOverclock(_sv, 1);
            for (int i = 0; i < 4; i++) _perks.RecordShoringStrutBuilt(_sv, 1, 1);
            _perks.RecordStormAirFilterOp(_sv, 1);
            _perks.RecordDisassemble(_sv, 7, 1);
            _perks.RecordRoomCleared(_sv, 1);
            _perks.RecordWarmDay(18f, new List<Survivor> { _sv }, 1);
            _perks.RecordWarmDay(18f, new List<Survivor> { _sv }, 2);

            var save = _perks.CaptureState();
            var restored = new ShelterPerkSystem();
            restored.Bind(_progression);
            restored.RestoreState(save);

            var c = restored.GetCounters(_sv.Id);
            Assert.AreEqual(3, c.JuryRigActions);
            Assert.AreEqual(4, c.ShoringStrutsBuilt);
            Assert.AreEqual(1, c.StormFilterOps);
            Assert.AreEqual(7, c.Disassembles);
            Assert.AreEqual(1, c.RoomsCleared);
            Assert.AreEqual(2, restored.ConsecutiveWarmDays);
        }
    }
}
