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
    /// Event_* wiring (CaptureState full set): 27 narrative events — API smoke + Capture/Restore + save slots.
    /// </summary>
    [TestFixture]
    public class EventsWiringTests
    {
        private const float Eps = 1e-3f;

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
        public void EuthanasiaPact_Form_Capture()
        {
            var e = new Event_EuthanasiaPact();
            e.FormPact("sv_a", "sv_b");
            Assert.IsTrue(e.IsPactActive());
            Assert.AreEqual(12f, e.GetHoursRemaining(), Eps);
            var save = e.CaptureState();
            Assert.AreEqual("euthanasia_pact", save.eventId);
            Assert.IsTrue(save.isPactActive);
            var e2 = new Event_EuthanasiaPact();
            e2.RestoreState(save);
            Assert.IsTrue(e2.IsPactActive());
            Assert.AreEqual("sv_a", e2.CaptureState().survivor1Id);
        }

        [Test]
        public void FactionMerger_Trigger_Capture()
        {
            var e = new Event_FactionMerger();
            Assert.IsTrue(e.TriggerMerger("fac_a", "fac_b"));
            Assert.AreEqual(30f, e.GetTributeDemand(10f), Eps);
            var save = e.CaptureState();
            Assert.AreEqual("event_faction_merger", save.eventId);
            Assert.IsTrue(save.isMerged);
            Assert.AreEqual("fac_a_fac_b_superfaction", save.superFactionId);
            var e2 = new Event_FactionMerger();
            e2.RestoreState(save);
            Assert.IsTrue(e2.CaptureState().isMerged);
            Assert.AreEqual(2f, e2.GetLootBonus(), Eps);
        }

        [Test]
        public void Mudslide_BurialDig_Capture()
        {
            var e = new Event_Mudslide();
            var st = e.CaptureState();
            st.isHatchBuried = true;
            st.digHoursCompleted = 2f;
            e.RestoreState(st);
            Assert.IsFalse(e.IsHatchAccessible());
            float contam = e.DigOut(2f);
            Assert.Greater(contam, 0f);
            var save = e.CaptureState();
            Assert.AreEqual("event_mudslide", save.eventId);
            Assert.AreEqual(4f, save.digHoursCompleted, Eps);
            var e2 = new Event_Mudslide();
            e2.RestoreState(save);
            Assert.IsTrue(e2.CaptureState().isHatchBuried);
        }

        [Test]
        public void NumbersStation_Sequence_Capture()
        {
            var e = new Event_NumbersStation();
            e.GenerateSequence(new System.Random(42), 4);
            var seq = e.CaptureState().numberSequence.ToArray();
            Assert.AreEqual(4, seq.Length);
            Assert.IsTrue(e.TryMatch(seq));
            Assert.IsTrue(e.IsDecoded());
            var save = e.CaptureState();
            Assert.AreEqual("event_numbers_station", save.eventId);
            Assert.IsTrue(save.isDecoded);
            var e2 = new Event_NumbersStation();
            e2.RestoreState(save);
            Assert.IsTrue(e2.IsDecoded());
        }

        [Test]
        public void ProjectSabotage_Trigger_Capture()
        {
            var e = new Event_ProjectSabotage();
            e.TriggerSabotage("site_reactor", 9);
            e.AssignGuard(1);
            var save = e.CaptureState();
            Assert.AreEqual("event_project_sabotage", save.eventId);
            Assert.IsTrue(save.isActive);
            Assert.AreEqual("site_reactor", save.constructionSiteId);
            Assert.AreEqual(9, save.saboteurStrength);
            Assert.AreEqual(1, save.guardsAssigned);
            var e2 = new Event_ProjectSabotage();
            e2.RestoreState(save);
            Assert.IsTrue(e2.CaptureState().isActive);
            Assert.AreEqual(1, e2.CaptureState().guardsAssigned);
        }

        [Test]
        public void Sinkhole_Collapse_Capture()
        {
            var e = new FoundationSinkholeSystem("event_sinkhole");
            string crushed = null;
            e.TriggerCollapse("room_surface", id => crushed = id);
            Assert.AreEqual("room_surface", crushed);
            var save = e.CaptureState();
            Assert.AreEqual("event_sinkhole", save.eventId);
            Assert.IsTrue(save.isTriggered);
            Assert.AreEqual("room_surface", save.collapsedRoomId);
            var e2 = new FoundationSinkholeSystem("tmp");
            e2.RestoreState(save);
            Assert.IsTrue(e2.CaptureState().isTriggered);
            Assert.AreEqual("room_surface", e2.CaptureState().collapsedRoomId);
        }

        [Test]
        public void Triangulation_Signal_Capture()
        {
            var e = new Event_Triangulation();
            e.ReceiveSignal(12.5f);
            var st = e.CaptureState();
            st.triangulationComplete = true;
            st.claimedByNodeId = "node_a";
            e.RestoreState(st);
            Assert.IsTrue(e.ClaimSupply());
            var save = e.CaptureState();
            Assert.AreEqual("event_triangulation", save.eventId);
            Assert.IsFalse(save.isActive);
            Assert.IsTrue(save.triangulationComplete);
            var e2 = new Event_Triangulation();
            e2.RestoreState(save);
            Assert.IsTrue(e2.CaptureState().triangulationComplete);
            Assert.AreEqual("node_a", e2.CaptureState().claimedByNodeId);
        }

        [Test]
        public void VaultCollision_Dig_Capture()
        {
            var e = new VaultCollisionSystem("event_vault_collision");
            var st = e.CaptureState();
            st.hasCollided = true;
            st.neighborState = "dead";
            e.RestoreState(st);
            Assert.AreEqual("free_loot", e.GetLootOrThreat());
            var save = e.CaptureState();
            Assert.AreEqual("event_vault_collision", save.eventId);
            Assert.IsTrue(save.hasCollided);
            Assert.AreEqual("dead", save.neighborState);
            var e2 = new VaultCollisionSystem("tmp");
            e2.RestoreState(save);
            Assert.AreEqual("dead", e2.GetNeighborState());
        }

        [Test]
        public void WarlordSuccession_Assassinate_Capture()
        {
            var e = new Event_WarlordSuccession();
            Assert.IsTrue(e.AssassinateLeader("war_east", new System.Random(1)));
            Assert.IsTrue(e.PlayFactionsOffEachOther("war_east_splinter_a"));
            var save = e.CaptureState();
            Assert.AreEqual("event_warlord_succession", save.eventId);
            Assert.IsTrue(save.isFractured);
            Assert.IsTrue(save.areAtWar);
            Assert.AreEqual("war_east_splinter_b", save.subFaction2Id);
            var e2 = new Event_WarlordSuccession();
            e2.RestoreState(save);
            Assert.IsTrue(e2.CaptureState().isFractured);
            Assert.AreEqual("war_east", e2.CaptureState().originalFactionId);
        }


        [Test]
        public void MultiEvent_SaveSlot_RoundTrip()
        {
            string dir = SaveSystemTestFactory.TempDir("events_multi");
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

                var merger = new Event_FactionMerger();
                merger.TriggerMerger("north", "south");

                var mud = new Event_Mudslide();
                var mst = mud.CaptureState();
                mst.isHatchBuried = true;
                mst.digHoursCompleted = 3f;
                mud.RestoreState(mst);

                var sink = new FoundationSinkholeSystem("event_sinkhole");
                sink.TriggerCollapse("room_x", null);

                var pact = new Event_EuthanasiaPact();
                pact.FormPact("p1", "p2");

                var war = new Event_WarlordSuccession();
                war.AssassinateLeader("fac_z", new System.Random(2));

                Assert.IsTrue(SaveSystemTestFactory.MakeSave(dir, ss =>
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
                    ss.SetEventEuthanasiaPact(pact);
                    ss.SetEventFactionMerger(merger);
                    ss.SetEventMudslide(mud);
                    ss.SetEventNumbersStation(new Event_NumbersStation());
                    ss.SetEventProjectSabotage(new Event_ProjectSabotage());
                    ss.SetEventSinkhole(sink);
                    ss.SetEventTriangulation(new Event_Triangulation());
                    ss.SetEventVaultCollision(new VaultCollisionSystem("event_vault_collision"));
                    ss.SetEventWarlordSuccession(war);
                }).Save("slot"));

                var brawl2 = new Event_Brawl();
                var feral2 = new Event_FeralRescue();
                var node2 = new Event_NodeCollapse();
                var murder2 = new Event_SpontaneousMurder();
                var schism2 = new Event_Schism();
                var cult2 = new Event_CultOfAI();
                var merger2 = new Event_FactionMerger();
                var mud2 = new Event_Mudslide();
                var sink2 = new FoundationSinkholeSystem("tmp");
                var pact2 = new Event_EuthanasiaPact();
                var war2 = new Event_WarlordSuccession();

                Assert.IsTrue(SaveSystemTestFactory.MakeSave(dir, ss =>
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
                    ss.SetEventEuthanasiaPact(pact2);
                    ss.SetEventFactionMerger(merger2);
                    ss.SetEventMudslide(mud2);
                    ss.SetEventNumbersStation(new Event_NumbersStation());
                    ss.SetEventProjectSabotage(new Event_ProjectSabotage());
                    ss.SetEventSinkhole(sink2);
                    ss.SetEventTriangulation(new Event_Triangulation());
                    ss.SetEventVaultCollision(new VaultCollisionSystem("event_vault_collision"));
                    ss.SetEventWarlordSuccession(war2);
                }).Load("slot"));

                Assert.IsTrue(brawl2.CaptureState().brawlActive);
                Assert.AreEqual(2, feral2.CaptureState().daysElapsed);
                Assert.AreEqual("kid_z", feral2.CaptureState().survivorId);
                Assert.IsTrue(node2.IsActive);
                Assert.Contains("trapped", node2.CaptureState().survivors_inside);
                Assert.IsTrue(murder2.HasMurderOccurred());
                Assert.IsTrue(schism2.CaptureState().isActive);
                Assert.IsTrue(cult2.IsActive);
                Assert.IsTrue(merger2.CaptureState().isMerged);
                Assert.IsTrue(mud2.CaptureState().isHatchBuried);
                Assert.AreEqual(3f, mud2.CaptureState().digHoursCompleted, Eps);
                Assert.IsTrue(sink2.CaptureState().isTriggered);
                Assert.AreEqual("room_x", sink2.CaptureState().collapsedRoomId);
                Assert.IsTrue(pact2.IsPactActive());
                Assert.IsTrue(war2.CaptureState().isFractured);
            }
            finally { try { Directory.Delete(dir, true); } catch { } }
        }
    }
}
