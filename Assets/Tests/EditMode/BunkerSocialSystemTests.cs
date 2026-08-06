using System.Collections.Generic;
using NUnit.Framework;
using AtomicWar._Game.Core;
using AtomicWar._Game.Survivors;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>Prompts #469-#478 — interpersonal &amp; leadership social systems.</summary>
    [TestFixture]
    public class BunkerSocialSystemTests
    {
        private static Survivor Make(string id, int morale = 75, int fatigue = 20, int health = 100)
        {
            return new Survivor
            {
                Id = id,
                DisplayName = id,
                State = SurvivorState.Idle,
                CurrentRoomId = "",
            };
        }

        private static void ApplyNeeds(Survivor sv, int morale = 75, int fatigue = 20, int health = 100)
        {
            sv.Needs.Morale = morale;
            sv.Needs.Fatigue = fatigue;
            sv.Needs.Health = health;
        }

        private static List<Survivor> Roster(params Survivor[] sv)
        {
            var list = new List<Survivor>();
            for (int i = 0; i < sv.Length; i++) if (sv[i] != null) list.Add(sv[i]);
            return list;
        }

        private BunkerSocialDirector NewDirector(List<Survivor> survivors)
        {
            var d = new BunkerSocialDirector { Survivors = survivors };
            d.Brig.GetSurvivors = () => survivors;
            d.Tribunal.GetSurvivors = () => survivors;
            return d;
        }

        // ─────────────── #469 LOVERS ───────────────

        [Test]
        public void Romance_Formed_AboveAffinityThreshold_SharedSpace()
        {
            var a = Make("a"); var b = Make("b");
            a.CurrentRoomId = "quarters"; b.CurrentRoomId = "quarters";
            var roster = Roster(a, b);
            var d = NewDirector(roster);
            d.Affinity.Set(a.Id, b.Id, 95f);

            d.Romance.UpdateBondStates(roster);

            Assert.IsTrue(d.Romance.AreLovers("a", "b"));
            Assert.AreEqual("b", d.Romance.GetLoverOf("a"));
            Assert.AreEqual(1, d.Romance.ActiveLoverCount);
        }

        [Test]
        public void Romance_SharedHopeAura_BoostsBoth_Morale()
        {
            var a = Make("a2"); var b = Make("b2");
            a.CurrentRoomId = "q1"; b.CurrentRoomId = "q1";
            a.Needs.Morale = 40f; b.Needs.Morale = 40f;
            var roster = Roster(a, b);
            var d = NewDirector(roster);
            d.Affinity.Set(a.Id, b.Id, 95f);
            d.Romance.UpdateBondStates(roster);

            d.Romance.ApplyAuras(1f, roster);

            Assert.AreEqual(40f + RomanceSystem.LoversHopeAuraPerHour, a.Needs.Morale, 0.001f);
            Assert.AreEqual(40f + RomanceSystem.LoversHopeAuraPerHour, b.Needs.Morale, 0.001f);
            Assert.AreEqual(1.5f, d.Romance.GetFatigueRecoveryMultiplier(a), 0.001f);
        }

        [Test]
        public void Romance_DamageToLover_HitsPartnerWithAnxiety()
        {
            var a = Make("a3"); var b = Make("b3");
            a.CurrentRoomId = "q"; b.CurrentRoomId = "q";
            var roster = Roster(a, b);
            var d = NewDirector(roster);
            d.Affinity.Set(a.Id, b.Id, 95f);
            d.Romance.UpdateBondStates(roster);
            b.Needs.Morale = 70f;

            var hit = d.Romance.ApplyLoverDamageAnxiety(a, roster);

            Assert.AreSame(b, hit);
            Assert.AreEqual(70f - RomanceSystem.LoverDamageAnxietyMoraleHit, b.Needs.Morale, 0.001f);
        }

        [Test]
        public void Romance_LoverDeath_InstantlyBreaksBereaved()
        {
            var a = Make("a4"); var b = Make("b4");
            a.CurrentRoomId = "q"; b.CurrentRoomId = "q";
            var roster = Roster(a, b);
            var d = NewDirector(roster);
            d.Affinity.Set(a.Id, b.Id, 95f);
            d.Romance.UpdateBondStates(roster);

            string appliedBreak = null;
            d.OnGriefMentalBreakApplied += (bereaved, br) => appliedBreak = br;

            d.NotifySurvivorDied(a, new System.Random(1));

            Assert.NotNull(appliedBreak);
            Assert.Contains(appliedBreak, new[] { RomanceSystem.GriefCatatonicBreakId, RomanceSystem.GriefSuicideBreakId });
            Assert.True(b.HasMentalBreak);
            Assert.AreEqual(0, d.Romance.ActiveLoverCount);
        }

        // ─────────────── #470 BREAKUP ───────────────

        [Test]
        public void Breakup_WhenAffinityFallsBelowThreshold_AuraAndCoopRefusal()
        {
            var a = Make("a5"); var b = Make("b5");
            a.CurrentRoomId = "q"; b.CurrentRoomId = "q";
            var roster = Roster(a, b);
            var d = NewDirector(roster);
            d.Affinity.Set(a.Id, b.Id, 95f);
            d.Romance.UpdateBondStates(roster);
            Assert.IsTrue(d.Romance.AreLovers("a5", "b5"));

            d.Affinity.Set(a.Id, b.Id, 30f);
            d.Romance.UpdateBondStates(roster);

            Assert.IsFalse(d.Romance.AreLovers("a5", "b5"));
            Assert.IsTrue(d.Romance.BreakupAuraActive("a5", "b5"));
            Assert.IsTrue(d.Romance.RefusesCooperativeTask("a5", "b5"));
            Assert.IsTrue(d.RefusesCooperation("a5", "b5"));
        }

        // ─────────────── #475 FEUDS ───────────────

        [Test]
        public void Feud_StartsBelowThreshold_And_SabotagesWork()
        {
            var a = Make("f1"); var b = Make("f2");
            a.CurrentRoomId = "kitchen"; b.CurrentRoomId = "kitchen";
            var roster = Roster(a, b);
            var d = NewDirector(roster);
            d.Affinity.Set(a.Id, b.Id, -70f);

            d.Feuds.UpdateFeuds(roster);
            Assert.IsTrue(d.Feuds.AreFeuding("f1", "f2"));

            string sabKind = null;
            d.Feuds.OnSabotageOccurred += (aa, bb, kind) => sabKind = kind;

            int landed = 0;
            int seed = -1;
            for (int s = 0; s < 500; s++)
            {
                landed = d.Feuds.TickSabotage(1f, roster, new System.Random(s));
                if (landed > 0) { seed = s; break; }
            }
            Assert.GreaterOrEqual(seed, 0, "Expected a seed where the feud pair sabotages.");
            Assert.AreEqual("meal_contamination", sabKind);
        }

        // ─────────────── #471 MUTINY ───────────────

        [Test]
        public void Mutiny_TriggeredAtLowWeeklyMorale_TakesControl()
        {
            var m = Make("m0"); ApplyNeeds(m, morale: 15);
            var f = Make("m1"); ApplyNeeds(f, morale: 10);
            var roster4 = Roster(m, f);
            var d = NewDirector(roster4);
            d.Mutiny.LeadershipScore = sv => sv.EffectiveScienceSkill + 1f; // rank by seed, both equal-ish

            for (int day = 1; day <= MutinySystem.MutinyWindowDays; day++)
                d.Mutiny.TickWeekly(day, roster4, new System.Random(3));

            Assert.IsTrue(d.Mutiny.MutinyActive);
            Assert.NotNull(d.Mutiny.LeaderId);
            Assert.IsTrue(d.IsRebel(d.Mutiny.LeaderId));

            // Negotiate restores control.
            Assert.IsTrue(d.ResolveMutinyNegotiate());
            Assert.IsFalse(d.Mutiny.MutinyActive);
        }

        [Test]
        public void Mutiny_YieldResources_RequiresPayment()
        {
            var m = Make("y0"); ApplyNeeds(m, morale: 12);
            var f = Make("y1"); ApplyNeeds(f, morale: 12);
            var roster = Roster(m, f);
            var d = NewDirector(roster);
            for (int day = 1; day <= MutinySystem.MutinyWindowDays; day++)
                d.Mutiny.TickWeekly(day, roster, new System.Random(5));
            Assert.IsTrue(d.Mutiny.MutinyActive);

            // Cannot afford → mutiny continues.
            bool paid = false; d.YieldBunkerControl = u => false;
            Assert.IsFalse(d.ResolveMutinyYield(10));
            Assert.IsTrue(d.Mutiny.MutinyActive);
        }

        // ─────────────── #472 IMPRISONMENT ───────────────

        [Test]
        public void Brig_Imprison_NoLabor_Release()
        {
            var sv = Make("cell1");
            var roster = Roster(sv);
            var d = NewDirector(roster);

            Assert.IsFalse(d.Imprison("cell1")); // no cell yet
            Assert.IsTrue(d.ConvertRoomToCell("brig"));
            Assert.IsTrue(d.Imprison("cell1"));
            Assert.IsTrue(d.Brig.IsImprisoned("cell1"));
            Assert.IsFalse(d.Brig.ProvidesLabor("cell1"));
            Assert.IsTrue(d.Brig.ConsumesFood("cell1"));

            Assert.IsTrue(d.Release("cell1"));
            Assert.IsTrue(d.Brig.ProvidesLabor("cell1"));
        }

        // ─────────────── #473 BANISHMENT ───────────────

        [Test]
        public void Banishment_Records_Banished_And_ReturnCooldown()
        {
            var sv = Make("ban1");
            var roster = Roster(sv);
            var d = NewDirector(roster);

            bool penalize = true;
            d.Banishment.OnBanish += (s, p) => penalize = p;
            Assert.IsTrue(d.Banish(sv, day: 10));
            Assert.IsTrue(penalize); // not a severe threat by default

            // Before the 30-day cooldown no return is possible regardless of seed.
            Assert.AreEqual(0, d.Banishment.TickBanishedReturns(39, new System.Random(1)));

            // After the cooldown there is a 50% chance on some seed.
            int returned = 0;
            for (int s = 0; s < 200; s++)
            {
                returned = d.Banishment.TickBanishedReturns(40, new System.Random(s));
                if (returned > 0) break;
            }
            Assert.AreEqual(1, returned, "Expected a seed where the banished returns as a raider.");
            Assert.IsTrue(d.Banishment.HasReturnedAsRaider("ban1"));
        }

        [Test]
        public void Banishment_SerialKiller_NoMoralePenalty()
        {
            var sv = Make("ban2");
            sv.ArchetypeId = PersonalQuestSystem.SerialKillerId;
            var roster = Roster(sv);
            var d = NewDirector(roster);
            d.IsSevereThreat = s => string.Equals(s.ArchetypeId, PersonalQuestSystem.SerialKillerId);

            bool penalize = true;
            d.Banishment.OnBanish += (s, p) => penalize = p;
            d.Banish(sv, 1);
            Assert.IsFalse(penalize);
        }

        // ─────────────── #476 PREGNANCY ───────────────

        [Test]
        public void Pregnancy_ConceivesAnd_BirthsChild_WithSupplies_HopeBuff()
        {
            var a = Make("p1"); ApplyNeeds(a, fatigue: 10);
            var b = Make("p2"); ApplyNeeds(b, fatigue: 20);
            var roster = Roster(a, b);
            var d = NewDirector(roster);

            int startSeed = -1;
            for (int s = 0; s < 400; s++)
            {
                var sd = NewDirector(roster);
                if (sd.TryStartPregnancy(a, b, new System.Random(s))) { startSeed = s; d = sd; break; }
            }
            Assert.GreaterOrEqual(startSeed, 0, "Expected a seed that starts a pregnancy.");
            Assert.IsTrue(d.Pregnancy.IsPregnant("p1"));

            d.HasPristineMedicalSupplies = _ => true;
            for (int day = 1; day <= PregnancySystem.PregnancyDurationDays; day++)
                d.Pregnancy.TickPregnancy(day, roster, new System.Random(startSeed));

            Assert.IsTrue(d.Pregnancy.ChildBorn);
            Assert.IsTrue(d.Pregnancy.ChildHopeBuffActive);
            Assert.IsFalse(d.Pregnancy.IsPregnant("p1"));
            Assert.Greater(a.Needs.Fatigue, 10f, "Pregnancy should have escalated fatigue.");
        }

        // ─────────────── #477 TRIBUNAL ───────────────

        [Test]
        public void Tribunal_MatchingPunishment_Appropriate_NoTrustLoss_MismatchLoses()
        {
            Assert.AreEqual(PunishmentMatch.Appropriate, TribunalSystem.MatchPunishment(BunkerCrimeSeverity.Severe, BunkerPunishment.Execution));
            Assert.AreEqual(PunishmentMatch.Lenient, TribunalSystem.MatchPunishment(BunkerCrimeSeverity.Severe, BunkerPunishment.RationCut));
            Assert.AreEqual(PunishmentMatch.Excessive, TribunalSystem.MatchPunishment(BunkerCrimeSeverity.Minor, BunkerPunishment.Execution));
        }

        [Test]
        public void Tribunal_JudgesCrime_WithTrust()
        {
            var sv = Make("t1");
            var roster = Roster(sv);
            var d = NewDirector(roster);
            d.RegisterCrime(sv, "ration_theft", BunkerCrimeSeverity.Minor);

            bool mismatched = false;
            d.Tribunal.OnVerdict += (s, pun, match, mm) => mismatched = mm;
            Assert.IsTrue(d.JudgeNext(BunkerPunishment.RationCut));
            Assert.IsFalse(mismatched);
            Assert.IsFalse(d.Tribunal.HasPending);
        }

        // ─────────────── #478 BLACK MARKET ───────────────

        [Test]
        public void BlackMarket_AllianceForms_Smuggles_And_CanBeExposed()
        {
            var a = Make("bm1"); var b = Make("bm2");
            a.Needs.Morale = 25f; b.Needs.Morale = 25f; // below the 40 morale ceiling
            var roster = Roster(a, b);
            var d = NewDirector(roster);
            d.Affinity.Set(a.Id, b.Id, 90f);
            d.AvailableSmuggleResources = _ => new List<string> { "ration" };
            d.SmuggleDrain = _ => "comfort_alcohol";

            int seed = -1;
            for (int s = 0; s < 400; s++)
            {
                d.BlackMarket.TickFormAlliances(roster, new System.Random(s));
                if (d.BlackMarket.HasAlliance("bm1", "bm2")) { seed = s; break; }
            }
            Assert.GreaterOrEqual(seed, 0, "Expected a seed that forms a smuggling alliance.");

            int smuggled = d.BlackMarket.TickSmuggle(roster, new System.Random(seed));
            Assert.Greater(smuggled, 0, "Alliance should have smuggled at least one resource on that seed.");
            Assert.IsTrue(d.BlackMarket.TotalSmuggled > 0);

            Assert.IsTrue(d.ExposeAlliance("bm1", "bm2"));
            Assert.IsFalse(d.BlackMarket.HasAlliance("bm1", "bm2"));
        }

        // ─────────────── SAVE/LOAD ───────────────

        [Test]
        public void Director_SaveRoundTrip_PreservesAllFamilies()
        {
            var a = Make("s1"); var b = Make("s2");
            a.CurrentRoomId = "q"; b.CurrentRoomId = "q";
            var roster = Roster(a, b);
            var d = NewDirector(roster);
            d.Affinity.Set(a.Id, b.Id, 96f);
            d.Romance.UpdateBondStates(roster);
            d.ConvertRoomToCell("brig");
            d.Imprison("s2");
            d.RegisterCrime(a, "murder", BunkerCrimeSeverity.Severe);
            // Quench the lovers bond straight to a feud: Adjust keeps the +96 base.
            d.Affinity.Set(b.Id, a.Id, -90f);
            d.Feuds.UpdateFeuds(roster);

            var save = (BunkerSocialSave)d.CaptureState();
            Assert.AreEqual(1, save.Romance.Lovers.Count);
            Assert.AreEqual(1, save.Brig.Imprisoned.Count);
            Assert.AreEqual(1, save.Tribunal.Pending.Count);

            var fresh = NewDirector(roster);
            fresh.RestoreState(save);

            Assert.IsTrue(fresh.Romance.AreLovers("s1", "s2"));
            Assert.IsTrue(fresh.Brig.IsImprisoned("s2"));
            Assert.IsTrue(fresh.Tribunal.HasPending);
            Assert.IsTrue(fresh.Feuds.AreFeuding("s1", "s2"));
        }
    }
}
