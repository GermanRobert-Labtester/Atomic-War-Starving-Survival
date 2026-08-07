using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Survivors;
using ShelterClass = AtomicWar._Game.Shelter.Shelter;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Event_* wiring (CaptureState subset): 18 narrative events — API smoke + Capture/Restore + save slots.
    /// </summary>
    [TestFixture]
    public class EventsWiringTests
    {
        private const float Eps = 1e-3f;

        private static string TempDir(string tag)
        {
            string dir = Path.Combine(Path.GetTempPath(), "ashfall_events_" + tag + "_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
            return dir;
        }

        private static SaveSystem MakeSave(string dir, Action<SaveSystem> wire)
        {
            var profile = ScriptableObject.CreateInstance<NeedsProfile>();
            var needs = new NeedsSystem(profile, sv => true);
            var weather = new WeatherSystem(null, 3);
            var temp = new TemperatureSystem(null, weather);
            var rad = new RadiationSystem(needs);
            var ss = new SaveSystem(new SaveSystem.CoreDeps
            {
                GameState = new GameState(),
                WeatherSystem = weather,
                TemperatureSystem = temp,
                NeedsSystem = needs,
                RadiationSystem = rad,
                Shelter = new ShelterClass(),
                GetSurvivors = () => new List<Survivor>(),
                ItemLookup = id => null,
                ModuleLookup = id => null,
                SavesDir = dir
            });
            wire(ss);
            return ss;
        }

        [Test]
        public void Brawl_Start_Capture()
        {
            var e = new Event_Brawl();
            var pair = e.CheckForBrawl(new List<(string id, float affinityWithOthers)>
            {
                ("sv_a", -1f),
                ("sv_b", -1f)
            }, new System.Random(1));
            Assert.IsNotNull(pair);
            var save = e.CaptureState();
            Assert.AreEqual("event_brawl", save.eventId);
            Assert.IsTrue(save.brawlActive);
            Assert.AreEqual("sv_a", save.fighterAId);
            var e2 = new Event_Brawl();
            e2.RestoreState(save);
            Assert.IsTrue(e2.CaptureState().brawlActive);
            Assert.AreEqual("sv_a", e2.CaptureState().fighterAId);
        }

        [Test]
        public void ComingOfAge_Track_Capture()
        {
            var e = new Event_ComingOfAge();
            for (int i = 0; i < 60; i++)
                e.TickDay("child_1");
            Assert.AreEqual(60, e.GetDaysSurvived("child_1"));
            var pre = e.CaptureState();
            Assert.AreEqual("event_coming_of_age", pre.eventId);
            Assert.Contains("child_1", pre.trackedChildIds);
            Assert.IsTrue(e.TryTransition("child_1", "engineer"));
            Assert.IsTrue(e.IsTriggered);
            var save = e.CaptureState();
            Assert.IsTrue(save.isTriggered);
            var e2 = new Event_ComingOfAge();
            e2.RestoreState(pre);
            Assert.AreEqual(60, e2.GetDaysSurvived("child_1"));
            var e3 = new Event_ComingOfAge();
            e3.RestoreState(save);
            Assert.IsTrue(e3.IsTriggered);
        }

        [Test]
        public void CultBlessing_Capture()
        {
            var e = new Event_CultBlessing();
            e.SurvivorCaptured("sv_glow");
            e.TickHour("sv_glow", new List<string> { "sv_ally" });
            Assert.IsTrue(e.IsGlowing("sv_glow"));
            var save = e.CaptureState();
            Assert.AreEqual("event_cult_blessing", save.event_id);
            Assert.Contains("sv_glow", save.captured_survivor_ids);
            var e2 = new Event_CultBlessing();
            e2.RestoreState(save);
            Assert.IsTrue(e2.IsGlowing("sv_glow"));
        }

        [Test]
        public void CultInitiation_Start_Capture()
        {
            var e = new Event_CultInitiation();
            e.StartInitiation("sv_init", "fast");
            Assert.IsFalse(e.IsInitiationComplete());
            var save = e.CaptureState();
            Assert.AreEqual("event_cult_initiation", save.eventId);
            Assert.IsTrue(save.active);
            Assert.AreEqual("sv_init", save.initiateId);
            var e2 = new Event_CultInitiation();
            e2.RestoreState(save);
            Assert.AreEqual("sv_init", e2.CaptureState().initiateId);
            Assert.IsTrue(e2.CaptureState().active);
        }

        [Test]
        public void CultOfAi_Form_Capture()
        {
            var e = new Event_CultOfAI();
            e.CheckActivation(aiEfficiency: 0.99f, civilianCount: 5);
            Assert.IsTrue(e.IsActive);
            Assert.Greater(e.CultistCount, 0);
            var save = e.CaptureState();
            Assert.IsTrue(save.IsActive);
            var e2 = new Event_CultOfAI();
            e2.RestoreState(save);
            Assert.IsTrue(e2.IsActive);
            Assert.AreEqual(save.CultistCount, e2.CultistCount);
        }

        [Test]
        public void EmpCascade_Trigger_Capture()
        {
            var e = new Event_EMPCascade();
            e.TriggerCascade(
                new List<string> { "exo_1" },
                new List<string> { "pat_1" },
                new List<string> { "left_leg" });
            Assert.IsTrue(e.IsActive());
            var save = e.CaptureState();
            Assert.AreEqual("event_emp_cascade", save.eventId);
            Assert.IsTrue(save.isActive);
            Assert.Contains("exo_1", save.crushedSurvivorIds);
            var e2 = new Event_EMPCascade();
            e2.RestoreState(save);
            Assert.IsTrue(e2.IsActive());
            Assert.Contains("exo_1", e2.CaptureState().crushedSurvivorIds);
        }

        [Test]
        public void FeralRescue_Discover_Capture()
        {
            var e = new Event_FeralRescue();
            e.DiscoverFeral("feral_1");
            e.TickDay();
            var save = e.CaptureState();
            Assert.AreEqual("event_feral_rescue", save.eventId);
            Assert.IsTrue(save.isDiscovered);
            Assert.AreEqual(1, save.daysElapsed);
            var e2 = new Event_FeralRescue();
            e2.RestoreState(save);
            Assert.AreEqual(1, e2.CaptureState().daysElapsed);
            Assert.AreEqual("feral_1", e2.CaptureState().survivorId);
        }

        [Test]
        public void FoundDiary_Blackmail_Capture()
        {
            var e = new Event_FoundDiary();
            e.DiscoverDiary("finder", "owner", hasConfession: true);
            e.StartBlackmail("finder", "owner");
            var save = e.CaptureState();
            Assert.AreEqual("event_found_diary", save.eventId);
            Assert.IsTrue(save.blackmailActive);
            Assert.IsTrue(save.containsConfession);
            var e2 = new Event_FoundDiary();
            e2.RestoreState(save);
            Assert.IsTrue(e2.CaptureState().blackmailActive);
            Assert.AreEqual("owner", e2.CaptureState().ownerOfDiaryId);
        }

        [Test]
        public void GriefCascade_Start_Capture()
        {
            var e = new Event_GriefCascade();
            Assert.IsTrue(e.OnBelovedDeath("sv_beloved", 0.9f));
            e.StartCascade(day: 10);
            Assert.IsTrue(e.IsCascading());
            var save = e.CaptureState();
            Assert.AreEqual("event_grief_cascade", save.eventId);
            Assert.IsTrue(save.cascadeActive);
            Assert.AreEqual("sv_beloved", save.triggerDeathId);
            var e2 = new Event_GriefCascade();
            e2.RestoreState(save);
            Assert.IsTrue(e2.IsCascading());
            Assert.AreEqual("sv_beloved", e2.CaptureState().triggerDeathId);
        }

        [Test]
        public void HungerStrike_Start_Capture()
        {
            var e = new Event_HungerStrike();
            e.TrackEvilChoice();
            e.TrackEvilChoice();
            e.TrackEvilChoice();
            e.CheckForStrike(new List<(string id, float morale, bool empath)>
            {
                ("sv_em", 20f, true),
                ("sv_other", 80f, false)
            });
            var save = e.CaptureState();
            Assert.AreEqual("event_hunger_strike", save.eventId);
            Assert.AreEqual(3, save.evilChoiceCount);
            Assert.Contains("sv_em", save.strikerIds);
            var e2 = new Event_HungerStrike();
            e2.RestoreState(save);
            Assert.Contains("sv_em", e2.CaptureState().strikerIds);
            Assert.AreEqual(3, e2.CaptureState().evilChoiceCount);
        }

        [Test]
        public void NodeCollapse_Trigger_Capture()
        {
            var e = new Event_NodeCollapse();
            e.TriggerCollapse("node_ruins");
            e.RegisterSurvivorInside("sv_in");
            Assert.IsTrue(e.IsActive);
            Assert.AreEqual(10, e.TurnsRemaining);
            var save = e.CaptureState();
            Assert.AreEqual("event_node_collapse", save.event_id);
            Assert.IsTrue(save.is_active);
            Assert.Contains("sv_in", save.survivors_inside);
            var e2 = new Event_NodeCollapse();
            e2.RestoreState(save);
            Assert.IsTrue(e2.IsActive);
            Assert.Contains("sv_in", e2.CaptureState().survivors_inside);
        }

        [Test]
        public void RansomNote_Deliver_Capture()
        {
            var e = new Event_RansomNote();
            e.DeliverNote(100f, "water");
            Assert.AreEqual(100f, e.GetDemand(), Eps);
            Assert.IsTrue(e.PayRansom(150f));
            var save = e.CaptureState();
            Assert.AreEqual("event_ransom_note", save.eventId);
            Assert.AreEqual(1, save.timesPaid);
            var e2 = new Event_RansomNote();
            e2.RestoreState(save);
            Assert.AreEqual(1, e2.CaptureState().timesPaid);
        }

        [Test]
        public void Schism_Trigger_Capture()
        {
            var e = new Event_Schism();
            e.TriggerSchism("zealot_1", "preacher_1", new List<string> { "zealot_1", "preacher_1", "sv_c" });
            e.PickSide("sv_c", isFactionA: true);
            var save = e.CaptureState();
            Assert.AreEqual("event_schism", save.eventId);
            Assert.IsTrue(save.isActive);
            Assert.AreEqual("zealot_1", save.zealotId);
            var e2 = new Event_Schism();
            e2.RestoreState(save);
            Assert.IsTrue(e2.CaptureState().isActive);
            Assert.Contains("sv_c", e2.CaptureState().factionASurvivors);
        }

        [Test]
        public void SecretSociety_Form_Capture()
        {
            var e = new Event_SecretSociety();
            string cliqueId = e.FormClique(new[] { "a", "b", "c" }, day: 5);
            Assert.IsFalse(string.IsNullOrEmpty(cliqueId));
            var save = e.CaptureState();
            Assert.AreEqual(1, save.cliques.Count);
            Assert.AreEqual(5, save.cliques[0].formedDay);
            var e2 = new Event_SecretSociety();
            e2.RestoreState(save);
            Assert.AreEqual(1, e2.CaptureState().cliques.Count);
            Assert.Contains("a", e2.CaptureState().cliques[0].memberIds);
        }

        [Test]
        public void SiblingFeud_Jealousy_Capture()
        {
            var e = new Event_SiblingFeud();
            e.OnSkillXPGained("teen_a", "teen_b");
            Assert.AreEqual(-0.1f, e.GetAffinityPenalty(), Eps);
            var save = e.CaptureState();
            Assert.AreEqual("event_sibling_feud", save.eventId);
            var e2 = new Event_SiblingFeud();
            e2.RestoreState(save);
            Assert.AreEqual(-0.1f, e2.GetAffinityPenalty(), Eps);
        }

        [Test]
        public void SpontaneousMurder_Execute_Capture()
        {
            var e = new Event_SpontaneousMurder();
            e.ExecuteMurder("killer", "victim", "pipe");
            Assert.IsTrue(e.HasMurderOccurred());
            var save = e.CaptureState();
            Assert.AreEqual("event_spontaneous_murder", save.eventId);
            Assert.IsTrue(save.murderOccurred);
            Assert.AreEqual("killer", save.killerId);
            var e2 = new Event_SpontaneousMurder();
            e2.RestoreState(save);
            Assert.IsTrue(e2.HasMurderOccurred());
            Assert.AreEqual("victim", e2.CaptureState().victimId);
        }

        [Test]
        public void TeenRebellion_Trigger_Capture()
        {
            var e = new Event_TeenRebellion();
            e.TriggerRebellion("teen_1", new System.Random(42));
            Assert.IsTrue(e.IsRebelling("teen_1"));
            var save = e.CaptureState();
            Assert.AreEqual("event_teen_rebellion", save.eventId);
            Assert.Contains("teen_1", save.activeRebellionTeenIds);
            var e2 = new Event_TeenRebellion();
            e2.RestoreState(save);
            Assert.IsTrue(e2.IsRebelling("teen_1"));
        }

        [Test]
        public void WitchHunt_Start_Capture()
        {
            var e = new Event_WitchHunt();
            e.StartHunt("accused_1", new List<string> { "acc_a", "acc_b" });
            var save = e.CaptureState();
            Assert.AreEqual("event_witch_hunt", save.eventId);
            Assert.IsTrue(save.huntActive);
            Assert.AreEqual("accused_1", save.accusedId);
            Assert.Contains("acc_a", save.accuserIds);
            var e2 = new Event_WitchHunt();
            e2.RestoreState(save);
            Assert.IsTrue(e2.CaptureState().huntActive);
            Assert.AreEqual("accused_1", e2.CaptureState().accusedId);
        }

        [Test]
        public void MultiEvent_SaveSlot_RoundTrip()
        {
            string dir = TempDir("multi");
            try
            {
                var brawl = new Event_Brawl();
                brawl.CheckForBrawl(new List<(string, float)> { ("x", -1f), ("y", -1f) }, new System.Random(1));

                var feral = new Event_FeralRescue();
                feral.DiscoverFeral("kid_z");
                feral.TickDay();
                feral.TickDay();

                var node = new Event_NodeCollapse();
                node.TriggerCollapse("n1");
                node.RegisterSurvivorInside("trapped");

                var murder = new Event_SpontaneousMurder();
                murder.ExecuteMurder("k", "v", "knife");

                var schism = new Event_Schism();
                schism.TriggerSchism("z", "p", new List<string> { "z", "p", "m" });

                var cult = new Event_CultOfAI();
                cult.CheckActivation(0.99f, 4);

                Assert.IsTrue(MakeSave(dir, ss =>
                {
                    ss.SetEventBrawl(brawl);
                    ss.SetEventFeralRescue(feral);
                    ss.SetEventNodeCollapse(node);
                    ss.SetEventSpontaneousMurder(murder);
                    ss.SetEventSchism(schism);
                    ss.SetEventCultOfAi(cult);
                    ss.SetEventComingOfAge(new Event_ComingOfAge());
                    ss.SetEventCultBlessing(new Event_CultBlessing());
                    ss.SetEventCultInitiation(new Event_CultInitiation());
                    ss.SetEventEmpCascade(new Event_EMPCascade());
                    ss.SetEventFoundDiary(new Event_FoundDiary());
                    ss.SetEventGriefCascade(new Event_GriefCascade());
                    ss.SetEventHungerStrike(new Event_HungerStrike());
                    ss.SetEventRansomNote(new Event_RansomNote());
                    ss.SetEventSecretSociety(new Event_SecretSociety());
                    ss.SetEventSiblingFeud(new Event_SiblingFeud());
                    ss.SetEventTeenRebellion(new Event_TeenRebellion());
                    ss.SetEventWitchHunt(new Event_WitchHunt());
                }).Save("slot"));

                var brawl2 = new Event_Brawl();
                var feral2 = new Event_FeralRescue();
                var node2 = new Event_NodeCollapse();
                var murder2 = new Event_SpontaneousMurder();
                var schism2 = new Event_Schism();
                var cult2 = new Event_CultOfAI();

                Assert.IsTrue(MakeSave(dir, ss =>
                {
                    ss.SetEventBrawl(brawl2);
                    ss.SetEventFeralRescue(feral2);
                    ss.SetEventNodeCollapse(node2);
                    ss.SetEventSpontaneousMurder(murder2);
                    ss.SetEventSchism(schism2);
                    ss.SetEventCultOfAi(cult2);
                    ss.SetEventComingOfAge(new Event_ComingOfAge());
                    ss.SetEventCultBlessing(new Event_CultBlessing());
                    ss.SetEventCultInitiation(new Event_CultInitiation());
                    ss.SetEventEmpCascade(new Event_EMPCascade());
                    ss.SetEventFoundDiary(new Event_FoundDiary());
                    ss.SetEventGriefCascade(new Event_GriefCascade());
                    ss.SetEventHungerStrike(new Event_HungerStrike());
                    ss.SetEventRansomNote(new Event_RansomNote());
                    ss.SetEventSecretSociety(new Event_SecretSociety());
                    ss.SetEventSiblingFeud(new Event_SiblingFeud());
                    ss.SetEventTeenRebellion(new Event_TeenRebellion());
                    ss.SetEventWitchHunt(new Event_WitchHunt());
                }).Load("slot"));

                Assert.IsTrue(brawl2.CaptureState().brawlActive);
                Assert.AreEqual(2, feral2.CaptureState().daysElapsed);
                Assert.AreEqual("kid_z", feral2.CaptureState().survivorId);
                Assert.IsTrue(node2.IsActive);
                Assert.Contains("trapped", node2.CaptureState().survivors_inside);
                Assert.IsTrue(murder2.HasMurderOccurred());
                Assert.IsTrue(schism2.CaptureState().isActive);
                Assert.IsTrue(cult2.IsActive);
            }
            finally { try { Directory.Delete(dir, true); } catch { } }
        }
    }
}
