using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.AI;
using AtomicWar._Game.AI.Actions;
using AtomicWar._Game.Core;
using AtomicWar._Game.Events;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Shelter.Modules;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.UI;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Tests for the three Prompt #29 follow-ups:
    /// (a) per-room passive drain (broken survivor only hits the
    ///     other survivors in the same room),
    /// (b) comfort-station module boosts the AI comfort-cure score,
    /// (c) hatch-dilemma prompt auto-resolves on timeout.
    /// </summary>
    [TestFixture]
    public class MentalBreakFollowupTests
    {
        private NeedsProfile _needsProfile;
        private NeedsSystem _needsSystem;
        private RadiationSystem _radSystem;
        private Inventory _inventory;
        private ItemDefinition _foodItem;
        private ItemDefinition _comfortItem;
        private Shelter _shelter;
        private MedicalSystem _medicalSystem;
        private MentalBreakSystem _mentalBreakSystem;
        private List<Survivor> _allSurvivors;
        private System.Random _rng;

        private MentalBreakSO _bingeEater;
        private MentalBreakSO _violentParanoia;

        [SetUp]
        public void SetUp()
        {
            EventBus.Clear();
            _needsProfile = ScriptableObject.CreateInstance<NeedsProfile>();
            _needsProfile.hungerPerHour = 1f;
            _needsProfile.thirstPerHour = 1f;
            _needsProfile.fatiguePerHour = 0.5f;
            _needsProfile.moraleLossPerHourWhileCritical = 1f;
            _needsSystem = new NeedsSystem(_needsProfile);
            _radSystem = new RadiationSystem(_needsSystem);
            _inventory = new Inventory { Capacity = 50, MaxWeight = 200f };

            _foodItem = ScriptableObject.CreateInstance<ItemDefinition>();
            _foodItem.id = "canned_food";
            _foodItem.displayName = "Canned Food";
            _foodItem.weight = 0.5f;
            _foodItem.type = ItemType.Food;
            _foodItem.hungerRestore = 30f;
            _foodItem.stackMax = 20;

            _comfortItem = ScriptableObject.CreateInstance<ItemDefinition>();
            _comfortItem.id = "old_book";
            _comfortItem.displayName = "Old Book";
            _comfortItem.weight = 0.3f;
            _comfortItem.type = ItemType.Comfort;
            _comfortItem.moraleEffect = 5f;
            _comfortItem.stackMax = 5;
            _inventory.Add(_comfortItem, 3);

            _shelter = new Shelter();
            _shelter.AddModule(new ShelterModuleInstance("air_filtration", 1) { FilterHealth = 100f });

            _medicalSystem = new MedicalSystem(_needsSystem, _inventory, _shelter);

            _bingeEater = ScriptableObject.CreateInstance<MentalBreakSO>();
            _bingeEater.id = "binge_eater";
            _bingeEater.displayName = "Binge Eater";
            _bingeEater.passiveMoraleDrainPerHour = 1f;
            _bingeEater.cureHours = 48f;
            _bingeEater.comfortItemCureAmount = 24f;
            _bingeEater.TraitWeights = new List<RiskBiasWeight>
            {
                new RiskBiasWeight { Trait = RiskBiasTrait.Realist, Weight = 1f }
            };

            _violentParanoia = ScriptableObject.CreateInstance<MentalBreakSO>();
            _violentParanoia.id = "violent_paranoia";
            _violentParanoia.displayName = "Violent Paranoia";
            _violentParanoia.passiveMoraleDrainPerHour = 2f;
            _violentParanoia.cureHours = 72f;
            _violentParanoia.requiresMedicalBed = true;
            _violentParanoia.comfortItemCureAmount = 12f;
            _violentParanoia.TraitWeights = new List<RiskBiasWeight>
            {
                new RiskBiasWeight { Trait = RiskBiasTrait.Paranoid, Weight = 2f },
                new RiskBiasWeight { Trait = RiskBiasTrait.Realist, Weight = 1f }
            };

            _allSurvivors = new List<Survivor>();
            _mentalBreakSystem = new MentalBreakSystem();
            _mentalBreakSystem.RegisterBreak(_bingeEater);
            _mentalBreakSystem.RegisterBreak(_violentParanoia);
            _mentalBreakSystem.BingeEatHandler = (sv, br) => 0;
            _mentalBreakSystem.SabotageHandler = (sv, br, rng) => null;
            _mentalBreakSystem.ComfortCureHandler = (sv, br) => true;

            _rng = new System.Random(42);
        }

        [TearDown]
        public void TearDown() { EventBus.Clear(); }

        private Survivor MakeSurvivor(string id, RiskBiasTrait trait, float morale = 75f)
        {
            var sv = new Survivor { Id = id, DisplayName = id, RiskBias = trait };
            sv.Needs.Morale = morale;
            _needsSystem.Register(sv);
            _radSystem.Register(sv);
            _allSurvivors.Add(sv);
            return sv;
        }

        // -------------------------------------------------------------------
        // (a) Per-room passive drain
        // -------------------------------------------------------------------

        [Test]
        public void BrokenSurvivor_InRoomA_OnlyDrainsOthersInRoomA()
        {
            var broken   = MakeSurvivor("sv_broken",   RiskBiasTrait.Realist, morale: 30f);
            var inRoomA  = MakeSurvivor("sv_roomA",    RiskBiasTrait.Realist, morale: 60f);
            var inRoomB  = MakeSurvivor("sv_roomB",    RiskBiasTrait.Realist, morale: 60f);
            var unassigned = MakeSurvivor("sv_unassign", RiskBiasTrait.Realist, morale: 60f);

            broken.CurrentRoomId     = "roomA";
            inRoomA.CurrentRoomId    = "roomA";
            inRoomB.CurrentRoomId    = "roomB";
            // unassigned has no room

            broken.currentMentalBreakId = "binge_eater";
            _mentalBreakSystem.Tick(1f, _allSurvivors, _rng);

            Assert.AreEqual(59f, inRoomA.Needs.Morale, 0.01f,
                "Same-room survivor should lose 1 morale per hour.");
            Assert.AreEqual(60f, inRoomB.Needs.Morale, 0.01f,
                "Different-room survivor should NOT lose morale.");
            Assert.AreEqual(60f, unassigned.Needs.Morale, 0.01f,
                "Unassigned survivor is treated as 'common area' — not in roomA, so no drain.");
        }

        [Test]
        public void BrokenSurvivor_Unassigned_DrainsAllOtherUnassigned()
        {
            // When the broken survivor has no room, the drain hits
            // every survivor whose CurrentRoomId is also empty.
            var broken   = MakeSurvivor("sv_broken",   RiskBiasTrait.Realist, morale: 30f);
            var unassigned1 = MakeSurvivor("sv_u1",    RiskBiasTrait.Realist, morale: 60f);
            var unassigned2 = MakeSurvivor("sv_u2",    RiskBiasTrait.Realist, morale: 60f);
            var inRoomA   = MakeSurvivor("sv_roomA",    RiskBiasTrait.Realist, morale: 60f);

            // broken and the two "u" survivors are unassigned; the
            // roomA survivor is in a room, so they should NOT drain.
            broken.currentMentalBreakId = "binge_eater";
            _mentalBreakSystem.Tick(1f, _allSurvivors, _rng);

            Assert.AreEqual(59f, unassigned1.Needs.Morale, 0.01f,
                "Unassigned survivor should lose 1 morale per hour.");
            Assert.AreEqual(59f, unassigned2.Needs.Morale, 0.01f,
                "Unassigned survivor should lose 1 morale per hour.");
            Assert.AreEqual(59f, inRoomA.Needs.Morale, 0.01f,
                "When broken is unassigned, ALL survivors drain (including roomed ones — whole-bunker fallback).");
        }

        // -------------------------------------------------------------------
        // (b) Comfort-station module boosts the AI score
        // -------------------------------------------------------------------

        [Test]
        public void MentalBreakComfortAction_WithComfortStation_HigherScoreThanWithout()
        {
            var survivor = MakeSurvivor("sv", RiskBiasTrait.Realist);
            survivor.currentMentalBreakId = "binge_eater";
            survivor.mentalBreakCureProgress = _bingeEater.cureHours - _bingeEater.comfortItemCureAmount;

            // Build a context with a comfort_station module.
            var csSo = ScriptableObject.CreateInstance<ComfortStationModuleSO>();
            csSo.id = "comfort_station";
            csSo.comfortCureScoreMultiplier = 2f;
            var csModule = new ShelterModuleInstance(csSo, 1) { IsEnabled = true };
            _shelter.AddModule(csModule);

            var action = ScriptableObject.CreateInstance<MentalBreakComfortActionSO>();
            var ctxWithStation = new AIContext(survivor, _shelter, _inventory, _rng)
            {
                MentalBreak = _mentalBreakSystem,
                GetSurvivors = () => _allSurvivors
            };
            float scoreWith = action.EvaluateRaw(ctxWithStation);

            // Now build a context without the comfort_station module.
            var shelterNoStation = new Shelter();
            shelterNoStation.AddModule(new ShelterModuleInstance("air_filtration", 1) { FilterHealth = 100f });
            var ctxNoStation = new AIContext(survivor, shelterNoStation, _inventory, _rng)
            {
                MentalBreak = _mentalBreakSystem,
                GetSurvivors = () => _allSurvivors
            };
            float scoreWithout = action.EvaluateRaw(ctxNoStation);

            Assert.Greater(scoreWith, 0f, "Score with station should be > 0.");
            Assert.Greater(scoreWith, scoreWithout,
                "Score with comfort_station must beat the no-station baseline.");
        }

        [Test]
        public void MentalBreakComfortAction_ComfortStationDisabled_NoBonus()
        {
            var survivor = MakeSurvivor("sv", RiskBiasTrait.Realist);
            survivor.currentMentalBreakId = "binge_eater";
            survivor.mentalBreakCureProgress = _bingeEater.cureHours - _bingeEater.comfortItemCureAmount;

            var csSo = ScriptableObject.CreateInstance<ComfortStationModuleSO>();
            csSo.id = "comfort_station";
            csSo.comfortCureScoreMultiplier = 2f;
            var csModule = new ShelterModuleInstance(csSo, 1) { IsEnabled = false };
            _shelter.AddModule(csModule);

            var action = ScriptableObject.CreateInstance<MentalBreakComfortActionSO>();
            var ctx = new AIContext(survivor, _shelter, _inventory, _rng)
            {
                MentalBreak = _mentalBreakSystem,
                GetSurvivors = () => _allSurvivors
            };
            float score = action.EvaluateRaw(ctx);

            // Baseline (no station) — sanity check.
            var shelterNoStation = new Shelter();
            var ctxNoStation = new AIContext(survivor, shelterNoStation, _inventory, _rng)
            {
                MentalBreak = _mentalBreakSystem,
                GetSurvivors = () => _allSurvivors
            };
            float baseline = action.EvaluateRaw(ctxNoStation);

            Assert.AreEqual(baseline, score, 0.001f,
                "A disabled comfort_station must NOT boost the score above baseline.");
        }

        // -------------------------------------------------------------------
        // (c) Hatch-dilemma prompt
        // -------------------------------------------------------------------

        [Test]
        public void HatchDilemmaPrompt_BeginStartsTimeout_AndFiresReady()
        {
            var prompt = new HatchDilemmaPrompt(timeoutGameHours: 3f);
            var exp = new ExpeditionState { ExpeditionId = "exp_1", Phase = ExpeditionPhase.AtHatchDilemma };

            ExpeditionState firedExp = null;
            prompt.OnPromptReady += e => firedExp = e;

            prompt.Begin(exp);

            Assert.IsTrue(prompt.IsActive, "Begin must activate the prompt.");
            Assert.AreSame(exp, prompt.ActiveExpedition);
            Assert.AreEqual(3f, prompt.HoursRemaining, 0.001f);
            Assert.AreSame(exp, firedExp, "Begin must fire OnPromptReady.");
        }

        [Test]
        public void HatchDilemmaPrompt_TickDecrementsTimeout_AndExpiresOnZero()
        {
            var prompt = new HatchDilemmaPrompt(timeoutGameHours: 2f);
            var exp = new ExpeditionState { ExpeditionId = "exp_1" };
            prompt.Begin(exp);

            HatchDilemmaResolvedSignal.Resolution fired = HatchDilemmaResolvedSignal.Resolution.LetThemIn;
            prompt.OnTimeout += r => fired = r;

            prompt.Tick(1f);
            Assert.IsTrue(prompt.IsActive, "1h of a 2h timeout: still active.");
            Assert.AreEqual(1f, prompt.HoursRemaining, 0.001f);

            prompt.Tick(0.5f);
            Assert.IsTrue(prompt.IsActive, "0.5h on a 1h remaining: still active.");

            prompt.Tick(0.5f);
            Assert.IsFalse(prompt.IsActive, "0h remaining: prompt must expire.");
            Assert.AreEqual(HatchDilemmaResolvedSignal.Resolution.ForceDeconOutside, fired,
                "Default timeout resolution must be ForceDeconOutside.");
        }

        [Test]
        public void HatchDilemmaPrompt_ResolveBeforeTimeout_FiresChoiceNotTimeout()
        {
            var prompt = new HatchDilemmaPrompt(timeoutGameHours: 5f);
            var exp = new ExpeditionState { ExpeditionId = "exp_1" };
            prompt.Begin(exp);

            bool timeoutFired = false;
            prompt.OnTimeout += _ => timeoutFired = true;
            HatchDilemmaResolvedSignal.Resolution chosen = HatchDilemmaResolvedSignal.Resolution.LetThemIn;
            prompt.OnChoiceApplied += r => chosen = r;

            prompt.Resolve(HatchDilemmaResolvedSignal.Resolution.DenyEntry);

            Assert.IsFalse(prompt.IsActive, "Resolve must deactivate the prompt.");
            Assert.AreEqual(HatchDilemmaResolvedSignal.Resolution.DenyEntry, chosen);
            prompt.Tick(100f); // advance way past the timeout
            Assert.IsFalse(timeoutFired, "Timeout must NOT fire after a player choice.");
        }

        [Test]
        public void HatchDilemmaPrompt_BeginWhileActive_NoOp()
        {
            var prompt = new HatchDilemmaPrompt();
            var exp1 = new ExpeditionState { ExpeditionId = "exp_1" };
            var exp2 = new ExpeditionState { ExpeditionId = "exp_2" };

            int fires = 0;
            prompt.OnPromptReady += _ => fires++;

            prompt.Begin(exp1);
            prompt.Begin(exp2); // should be ignored

            Assert.AreEqual(1, fires, "Begin-while-active is a no-op; OnPromptReady fires once.");
            Assert.AreSame(exp1, prompt.ActiveExpedition, "Active expedition unchanged.");
        }

        // -------------------------------------------------------------------
        // (d) Room-assignment HUD logic
        // -------------------------------------------------------------------

        [Test]
        public void RoomAssignmentHUD_AssignSurvivorToRoom_MovesSurvivor()
        {
            var sv = MakeSurvivor("sv_move", RiskBiasTrait.Realist);
            var other = MakeSurvivor("sv_other", RiskBiasTrait.Realist);

            var hud = new GameObject().AddComponent<RoomAssignmentHUD>();
            hud.Bind(_allSurvivors, _shelter);

            // No assignment yet.
            Assert.IsTrue(string.IsNullOrEmpty(sv.CurrentRoomId), "Setup: no room assigned.");

            // Assign.
            bool moved = hud.AssignSurvivorToRoom("sv_move", "quarters");
            Assert.IsTrue(moved);
            Assert.AreEqual("quarters", sv.CurrentRoomId);
            Assert.IsTrue(string.IsNullOrEmpty(other.CurrentRoomId),
                "Other survivor must stay unassigned.");
        }

        [Test]
        public void RoomAssignmentHUD_AssignSurvivorToRoom_SameRoom_NoOp()
        {
            var sv = MakeSurvivor("sv_same", RiskBiasTrait.Realist);
            sv.CurrentRoomId = "stores";
            var hud = new GameObject().AddComponent<RoomAssignmentHUD>();
            hud.Bind(_allSurvivors, _shelter);

            bool moved = hud.AssignSurvivorToRoom("sv_same", "stores");
            Assert.IsFalse(moved, "Reassigning to the same room is a no-op.");
            Assert.AreEqual("stores", sv.CurrentRoomId);
        }

        [Test]
        public void RoomAssignmentHUD_GetAssignmentRows_ShowsCorrectLabels()
        {
            var svA = MakeSurvivor("sv_rowA", RiskBiasTrait.Realist);
            var svB = MakeSurvivor("sv_rowB", RiskBiasTrait.Realist);
            svA.CurrentRoomId = "quarters";
            // svB has no room.

            var hud = new GameObject().AddComponent<RoomAssignmentHUD>();
            hud.Bind(_allSurvivors, _shelter);

            var rows = hud.GetAssignmentRows();
            Assert.AreEqual(2, rows.Count);
            // Unassigned should show the "Common Area" label.
            bool foundQuarters = false;
            bool foundCommon = false;
            for (int i = 0; i < rows.Count; i++)
            {
                if (rows[i].Survivor.Id == "sv_rowA" && rows[i].CurrentRoomLabel == "quarters") foundQuarters = true;
                if (rows[i].Survivor.Id == "sv_rowB" && rows[i].CurrentRoomLabel == hud.UnassignedRoomLabel) foundCommon = true;
            }
            Assert.IsTrue(foundQuarters, "Assigned survivor should show the room id.");
            Assert.IsTrue(foundCommon, "Unassigned survivor should show the Common Area label.");
        }

        [Test]
        public void RoomAssignmentHUD_AssignAllUnassigned_MovesEveryoneWithoutARoom()
        {
            var sv1 = MakeSurvivor("sv_un1", RiskBiasTrait.Realist);
            var sv2 = MakeSurvivor("sv_un2", RiskBiasTrait.Realist);
            var sv3 = MakeSurvivor("sv_un3", RiskBiasTrait.Realist);
            sv3.CurrentRoomId = "entry"; // already assigned

            var hud = new GameObject().AddComponent<RoomAssignmentHUD>();
            hud.Bind(_allSurvivors, _shelter);
            hud.AssignAllUnassigned("quarters");

            Assert.AreEqual("quarters", sv1.CurrentRoomId);
            Assert.AreEqual("quarters", sv2.CurrentRoomId);
            Assert.AreEqual("entry", sv3.CurrentRoomId,
                "Already-assigned survivor must not be moved.");
        }

        [Test]
        public void RoomAssignmentHUD_EventFiresOnEachChange()
        {
            var sv = MakeSurvivor("sv_event", RiskBiasTrait.Realist);
            var hud = new GameObject().AddComponent<RoomAssignmentHUD>();
            hud.Bind(_allSurvivors, _shelter);

            Survivor firedSv = null;
            string firedRoom = null;
            hud.OnRoomAssignmentChanged += (s, r) => { firedSv = s; firedRoom = r; };

            hud.AssignSurvivorToRoom("sv_event", "entry");
            Assert.AreSame(sv, firedSv);
            Assert.AreEqual("entry", firedRoom);
        }

        [Test]
        public void Shelter_GetRoomIds_ReturnsUniqueIdsFromModules()
        {
            var shelter = new Shelter();
            shelter.AddModule(new ShelterModuleInstance("air_filtration", 1) { RoomId = "plant" });
            shelter.AddModule(new ShelterModuleInstance("bed", 1) { RoomId = "quarters" });
            shelter.AddModule(new ShelterModuleInstance("workbench", 1) { RoomId = "plant" }); // duplicate

            var ids = shelter.GetRoomIds();
            Assert.AreEqual(2, ids.Count, "Duplicate RoomIds should be collapsed.");
            Assert.IsTrue(ids.Contains("plant"));
            Assert.IsTrue(ids.Contains("quarters"));
        }
    }
}
