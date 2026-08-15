// FactionSystemsExpansionIITests.cs — EditMode tests for the four Expansion II
// faction-pressure systems: GarrisonComplianceLedger, MilitiaContributionTax,
// CultLeash, WarlordTribute. Also covers Capture/Restore deep-copy and a
// SaveSystem slot round-trip via SaveSystemTestFactory.
using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Expansion II: The Weight of Factions — strike thresholds, escalation
    /// rates, ritual mechanics, and save/load round-trip across all four
    /// faction-pressure systems.
    /// </summary>
    [TestFixture]
    public class FactionSystemsExpansionIITests
    {
        // ── GarrisonComplianceLedger ────────────────────────────────────

        [Test]
        public void GarrisonLedger_ThreeStrikes_FlipsToNonCompliant_AndDropsPatrolWeight()
        {
            var sys = new System_GarrisonComplianceLedger();
            string struck = null;
            int strikeCount = 0;
            sys.OnStrikeRecorded += (id, n) => { struck = id; strikeCount = n; };
            string nonComp = null;
            sys.OnNonCompliant += id => nonComp = id;

            sys.FileNonCompliance("shelter_alpha", "missed_week_1");
            sys.FileNonCompliance("shelter_alpha", "missed_week_2");
            Assert.AreEqual("shelter_alpha", struck);
            Assert.AreEqual(2, strikeCount);
            Assert.IsNull(nonComp, "Not yet at threshold");
            Assert.AreEqual(System_GarrisonComplianceLedger.CompliantPatrolWeight,
                sys.GetPatrolRouteWeight("shelter_alpha"));

            sys.FileNonCompliance("shelter_alpha", "missed_week_3");
            Assert.AreEqual("shelter_alpha", nonComp);
            Assert.AreEqual(System_GarrisonComplianceLedger.NonCompliantPatrolWeight,
                sys.GetPatrolRouteWeight("shelter_alpha"));
            Assert.IsFalse(sys.GetPatrolRoute().Contains("shelter_alpha"),
                "Non-compliant shelter must not be on the patrol route");
        }

        [Test]
        public void GarrisonLedger_FourCompliantWeeks_Reinstates_AndRestoresRoute()
        {
            var sys = new System_GarrisonComplianceLedger();
            string reinstated = null;
            sys.OnReinstated += id => reinstated = id;

            for (int i = 0; i < 3; i++) sys.FileNonCompliance("shelter_alpha", "x");
            Assert.IsTrue(sys.GetShelterStatus("shelter_alpha").non_compliant_flag);

            for (int w = 0; w < System_GarrisonComplianceLedger.ReinstatedWeeks - 1; w++)
            {
                sys.RecordCompliantVisit("shelter_alpha", w);
                Assert.IsNull(reinstated, "Not yet 4 good weeks");
            }
            sys.RecordCompliantVisit("shelter_alpha", System_GarrisonComplianceLedger.ReinstatedWeeks - 1);
            Assert.AreEqual("shelter_alpha", reinstated);
            Assert.IsFalse(sys.GetShelterStatus("shelter_alpha").non_compliant_flag);
            Assert.IsTrue(sys.GetPatrolRoute().Contains("shelter_alpha"));
        }

        [Test]
        public void GarrisonLedger_RecordRequisition_StoresAuditId()
        {
            var sys = new System_GarrisonComplianceLedger();
            sys.RecordRequisition("shelter_alpha", "req-2026-001");
            Assert.AreEqual("req-2026-001",
                sys.GetShelterStatus("shelter_alpha").last_requisition_id);
        }

        [Test]
        public void GarrisonLedger_CaptureState_IsDeepCopy()
        {
            var a = new System_GarrisonComplianceLedger();
            a.RecordRequisition("shelter_alpha", "req-1");
            a.FileNonCompliance("shelter_alpha", "x");
            a.FileNonCompliance("shelter_alpha", "x");
            a.FileNonCompliance("shelter_alpha", "x");

            var save = a.CaptureState();
            Assert.AreEqual("system_garrison_compliance_ledger", save.system_id);
            Assert.AreEqual(1, save.entries.Count);

            // Mutate after capture must not touch snapshot.
            a.FileNonCompliance("shelter_alpha", "x");
            Assert.AreEqual(1, save.entries.Count);
            Assert.AreEqual(3, save.entries[0].compliance_strikes);
        }

        // ── MilitiaContributionTax ──────────────────────────────────────

        [Test]
        public void MilitiaTax_StartsAtTenPercent_AndEscalatesByFivePerRefusal()
        {
            var sys = new System_MilitiaContributionTax();
            float? got = null;
            sys.OnTaxRateChanged += (id, rate) => got = rate;

            sys.SetVillageInitialRate("village_a", 0.10f);
            Assert.AreEqual(0.10f, sys.GetEffectiveTaxRate("village_a"), 0.0001f);

            sys.RefuseTax("village_a", 0);
            Assert.AreEqual(0.15f, sys.GetEffectiveTaxRate("village_a"), 0.0001f);
            Assert.AreEqual(0.15f, got.Value, 0.0001f);

            sys.RefuseTax("village_a", 1);
            sys.RefuseTax("village_a", 2);
            // After 3 refusals the protection is withdrawn; the rate is now 0.25.
            Assert.AreEqual(0.25f, sys.GetEffectiveTaxRate("village_a"), 0.0001f);
        }

        [Test]
        public void MilitiaTax_RefusalStreakOfThree_DaysWithdrawsProtection()
        {
            var sys = new System_MilitiaContributionTax();
            string withdrawn = null;
            sys.OnProtectionWithdrawn += id => withdrawn = id;
            string reinstated = null;
            sys.OnProtectionReinstated += id => reinstated = id;

            sys.SetVillageInitialRate("village_a", 0.10f);
            for (int i = 0; i < System_MilitiaContributionTax.RefusalGraceDays - 1; i++)
            {
                sys.RefuseTax("village_a", i);
                Assert.IsNull(withdrawn, "Not yet at the grace-day threshold");
            }
            sys.RefuseTax("village_a", System_MilitiaContributionTax.RefusalGraceDays - 1);
            Assert.AreEqual("village_a", withdrawn);
            Assert.IsTrue(sys.IsProtectionWithdrawn("village_a"));

            // Two paid weeks reinstates.
            sys.PayTax("village_a", 0);
            Assert.IsNull(reinstated, "One good week is not enough");
            sys.PayTax("village_a", 1);
            Assert.AreEqual("village_a", reinstated);
            Assert.IsFalse(sys.IsProtectionWithdrawn("village_a"));
        }

        [Test]
        public void MilitiaTax_RateIsCappedAtFiftyPercent()
        {
            var sys = new System_MilitiaContributionTax();
            sys.SetVillageInitialRate("village_a", 0.10f);
            for (int i = 0; i < 100; i++) sys.RefuseTax("village_a", i);
            Assert.AreEqual(System_MilitiaContributionTax.MaxTaxRate,
                sys.GetEffectiveTaxRate("village_a"), 0.0001f);
        }

        // ── CultLeash ──────────────────────────────────────────────────

        [Test]
        public void CultLeash_ThreeVisits_UnlocksBlessing_AndUnderProtection()
        {
            var sys = new System_CultLeash();
            int? visitCount = null;
            string blessed = null;
            sys.OnVisitRecorded += (id, n) => visitCount = n;
            sys.OnBlessed += id => blessed = id;

            sys.RecordVisit("shelter_alpha", 1);
            sys.RecordVisit("shelter_alpha", 2);
            Assert.AreEqual(2, visitCount);
            Assert.IsFalse(sys.AttemptBlessing("shelter_alpha"),
                "Two visits must not be enough");
            Assert.IsNull(blessed);

            sys.RecordVisit("shelter_alpha", 3);
            Assert.IsTrue(sys.AttemptBlessing("shelter_alpha"));
            Assert.AreEqual("shelter_alpha", blessed);
            Assert.IsTrue(sys.IsBlessed("shelter_alpha"));
            Assert.IsTrue(sys.IsUnderProtection("shelter_alpha"));
        }

        [Test]
        public void CultLeash_MissedCommunion_OneWeekWarned_TwoWeeksForbidden()
        {
            var sys = new System_CultLeash();
            for (int i = 0; i < 3; i++) sys.RecordVisit("shelter_alpha", i);
            sys.AttemptBlessing("shelter_alpha");

            Assert.AreEqual(CultLeaveOutcome.Permitted, sys.AttemptLeave("shelter_alpha"),
                "No missed weeks yet — even blessed, the deacon hasn't come");

            sys.RecordMissedCommunion("shelter_alpha", 0);
            Assert.AreEqual(CultLeaveOutcome.Warned, sys.AttemptLeave("shelter_alpha"));

            sys.RecordMissedCommunion("shelter_alpha", 1);
            Assert.AreEqual(CultLeaveOutcome.ForbiddenWithConsequence, sys.AttemptLeave("shelter_alpha"));
        }

        [Test]
        public void CultLeash_CommunionAttendance_ResetsMissStreak()
        {
            var sys = new System_CultLeash();
            for (int i = 0; i < 3; i++) sys.RecordVisit("shelter_alpha", i);
            sys.AttemptBlessing("shelter_alpha");
            sys.RecordMissedCommunion("shelter_alpha", 0);
            sys.RecordMissedCommunion("shelter_alpha", 1);
            Assert.AreEqual(2, sys.GetConsecutiveMissedWeeks("shelter_alpha"));
            sys.RecordCommunionAttendance("shelter_alpha", 2, 4);
            Assert.AreEqual(0, sys.GetConsecutiveMissedWeeks("shelter_alpha"));
            Assert.AreEqual(4, sys.GetEntry("shelter_alpha").children_at_communion.Length);
        }

        // ── WarlordTribute ──────────────────────────────────────────────

        [Test]
        public void WarlordTribute_ShortPaymentEscalatesByOnePointFive()
        {
            var sys = new System_WarlordTribute();
            float? got = null;
            sys.OnTributeSet += (id, amt) => got = amt;
            int? shortStreak = null;
            sys.OnShortPaymentEscalated += (id, n) => shortStreak = n;

            sys.SetInitialTribute("shelter_alpha", 10f);
            Assert.AreEqual(10f, sys.GetRequiredTribute("shelter_alpha"), 0.0001f);

            // 8 of 10 = 0.8 ratio, below 0.9 threshold => short.
            sys.PayShort("shelter_alpha", 8f, 0);
            Assert.AreEqual(15f, sys.GetRequiredTribute("shelter_alpha"), 0.0001f);
            Assert.AreEqual(15f, got.Value, 0.0001f);
            Assert.AreEqual(1, shortStreak);

            // 13 of 15 = 0.866, still short => 22.5
            sys.PayShort("shelter_alpha", 13f, 1);
            Assert.AreEqual(22.5f, sys.GetRequiredTribute("shelter_alpha"), 0.0001f);
        }

        [Test]
        public void WarlordTribute_BorderlinePayment_NotShort_DoesNotEscalate()
        {
            var sys = new System_WarlordTribute();
            sys.SetInitialTribute("shelter_alpha", 10f);
            // 9 of 10 = 0.9 ratio, NOT below 0.9 threshold.
            sys.PayShort("shelter_alpha", 9f, 0);
            Assert.AreEqual(10f, sys.GetRequiredTribute("shelter_alpha"), 0.0001f);
        }

        [Test]
        public void WarlordTribute_GetRequiredTribute_RespectsMaxMultiplierCap()
        {
            var sys = new System_WarlordTribute();
            sys.SetInitialTribute("shelter_alpha", 10f);
            for (int i = 0; i < 20; i++) sys.PayShort("shelter_alpha", 1f, i);
            // 20 short weeks of 1.5x each, but capped at 8 * base.
            float cap = 10f * System_WarlordTribute.MaxTributeMultiplier;
            Assert.AreEqual(cap, sys.GetRequiredTribute("shelter_alpha"), 0.0001f);
        }

        [Test]
        public void WarlordTribute_LeaveOneThing_AndBurnToggle()
        {
            var sys = new System_WarlordTribute();
            string leftOne = null;
            string burned = null;
            sys.OnLeaveOneThingGiven += id => leftOne = id;
            sys.OnShelterBurned += id => burned = id;

            sys.SetInitialTribute("shelter_alpha", 10f);
            sys.FulfillLeaveOneThing("shelter_alpha", "item_heirloom_locket");
            Assert.AreEqual("shelter_alpha", leftOne);
            Assert.IsTrue(sys.GetEntry("shelter_alpha").leave_one_thing_fulfilled);

            sys.ClearLeaveOneThing("shelter_alpha");
            Assert.IsFalse(sys.GetEntry("shelter_alpha").leave_one_thing_fulfilled);

            sys.MarkShelterBurned("shelter_alpha");
            Assert.AreEqual("shelter_alpha", burned);
            Assert.IsTrue(sys.IsShelterBurned("shelter_alpha"));
            Assert.AreEqual(0f, sys.GetRequiredTribute("shelter_alpha"),
                "Burned shelters owe nothing further");
        }

        [Test]
        public void WarlordTribute_PublicCodeConstants_AreStableSnakeCase()
        {
            // Lore: a public string-constant set the radio library quotes.
            Assert.AreEqual("code_no_kill_if_paying", System_WarlordTribute.CodeNoKillIfPaying);
            Assert.AreEqual("code_no_burn_shelters", System_WarlordTribute.CodeNoBurnShelters);
            Assert.AreEqual("code_no_take_children", System_WarlordTribute.CodeNoTakeChildren);
            Assert.AreEqual("code_kill_quickly_if_resisted", System_WarlordTribute.CodeKillQuicklyIfResisted);
            Assert.AreEqual("code_always_leave_one_thing", System_WarlordTribute.CodeAlwaysLeaveOneThing);
        }

        // ── Save/Restore: deep copy + round-trip across all four ────────

        [Test]
        public void AllFour_CaptureState_IsDeepCopy_MutateAfterCapture_DoesNotTouchSnapshot()
        {
            var gl = new System_GarrisonComplianceLedger();
            gl.RecordCompliantVisit("shelter_alpha", 0);
            gl.FileNonCompliance("shelter_alpha", "x");
            var glSave = gl.CaptureState();

            var mt = new System_MilitiaContributionTax();
            mt.SetVillageInitialRate("village_a", 0.10f);
            mt.RefuseTax("village_a", 0);
            var mtSave = mt.CaptureState();

            var cl = new System_CultLeash();
            cl.RecordVisit("shelter_alpha", 0);
            var clSave = cl.CaptureState();

            var wt = new System_WarlordTribute();
            wt.SetInitialTribute("shelter_alpha", 10f);
            wt.PayShort("shelter_alpha", 5f, 0);
            var wtSave = wt.CaptureState();

            // Mutate live systems post-capture.
            gl.FileNonCompliance("shelter_alpha", "x");
            gl.FileNonCompliance("shelter_alpha", "x");
            mt.PayTax("village_a", 0);
            cl.RecordVisit("shelter_alpha", 0);
            cl.RecordVisit("shelter_alpha", 0);
            wt.PayFull("shelter_alpha", 0);

            // Snapshots must be unchanged.
            Assert.AreEqual(1, glSave.entries.Count);
            Assert.AreEqual(1, glSave.entries[0].compliance_strikes);

            Assert.AreEqual(1, mtSave.entries.Count);
            Assert.AreEqual(0.15f, mtSave.entries[0].current_tax_rate, 0.0001f);

            Assert.AreEqual(1, clSave.entries.Count);
            Assert.AreEqual(1, clSave.entries[0].visit_count);

            Assert.AreEqual(1, wtSave.entries.Count);
            Assert.AreEqual(15f, wtSave.entries[0].current_tribute_amount, 0.0001f);
        }

        [Test]
        public void AllFour_RestoreState_Null_Resets()
        {
            var gl = new System_GarrisonComplianceLedger();
            gl.FileNonCompliance("shelter_alpha", "x");
            gl.RestoreState(null);
            Assert.AreEqual(0, gl.EntryCount);

            var mt = new System_MilitiaContributionTax();
            mt.SetVillageInitialRate("village_a", 0.10f);
            mt.RefuseTax("village_a", 0);
            mt.RestoreState(null);
            Assert.AreEqual(0, mt.EntryCount);

            var cl = new System_CultLeash();
            cl.RecordVisit("shelter_alpha", 0);
            cl.RestoreState(null);
            Assert.AreEqual(0, cl.EntryCount);

            var wt = new System_WarlordTribute();
            wt.SetInitialTribute("shelter_alpha", 10f);
            wt.RestoreState(null);
            Assert.AreEqual(0, wt.EntryCount);
        }

        [Test]
        public void SaveSystem_FactionPressure_SlotRoundTrip_ViaSaveSystemTestFactory()
        {
            string dir = SaveSystemTestFactory.TempDir("faction_pressure_ii");
            try
            {
                // Source systems
                var glA = new System_GarrisonComplianceLedger();
                glA.FileNonCompliance("shelter_alpha", "x");
                glA.FileNonCompliance("shelter_alpha", "x");
                glA.FileNonCompliance("shelter_alpha", "x");

                var mtA = new System_MilitiaContributionTax();
                mtA.SetVillageInitialRate("village_a", 0.10f);
                mtA.RefuseTax("village_a", 0);

                var clA = new System_CultLeash();
                clA.RecordVisit("shelter_alpha", 0);
                clA.RecordVisit("shelter_alpha", 1);
                clA.RecordVisit("shelter_alpha", 2);
                clA.AttemptBlessing("shelter_alpha");

                var wtA = new System_WarlordTribute();
                wtA.SetInitialTribute("shelter_alpha", 10f);
                wtA.PayShort("shelter_alpha", 5f, 0);

                SaveSystem Make(System_GarrisonComplianceLedger gl, System_MilitiaContributionTax mt,
                    System_CultLeash cl, System_WarlordTribute wt) =>
                    SaveSystemTestFactory.MakeSave(dir, ss =>
                    {
                        ss.SetGarrisonComplianceLedgerSystem(gl);
                        ss.SetMilitiaContributionTaxSystem(mt);
                        ss.SetCultLeashSystem(cl);
                        ss.SetWarlordTributeSystem(wt);
                    });

                Assert.IsTrue(Make(glA, mtA, clA, wtA).Save("faction_ii_slot"));

                var glB = new System_GarrisonComplianceLedger();
                var mtB = new System_MilitiaContributionTax();
                var clB = new System_CultLeash();
                var wtB = new System_WarlordTribute();
                Assert.IsTrue(Make(glB, mtB, clB, wtB).Load("faction_ii_slot"));

                Assert.AreEqual(3, glB.GetShelterStatus("shelter_alpha").compliance_strikes);
                Assert.IsTrue(glB.GetShelterStatus("shelter_alpha").non_compliant_flag);

                Assert.AreEqual(0.15f, mtB.GetEffectiveTaxRate("village_a"), 0.0001f);

                Assert.IsTrue(clB.IsBlessed("shelter_alpha"));
                Assert.AreEqual(3, clB.GetVisitCount("shelter_alpha"));

                Assert.AreEqual(15f, wtB.GetRequiredTribute("shelter_alpha"), 0.0001f);
            }
            finally
            {
                try { if (Directory.Exists(dir)) Directory.Delete(dir, true); } catch { /* best-effort */ }
            }
        }
    }
}
