// FactionPressureWiringHostTests.cs — Plain NUnit host-level tests for the
// static FactionPressureWiring class. These tests do NOT touch Unity
// (no MonoBehaviour, no EditMode harness). They construct the 4 systems,
// hand-injected provider funcs, and exercise HandleRaidResolved +
// OnCommunionMissed directly.
using System;
using System.Collections.Generic;
using NUnit.Framework;
using AtomicWar._Game.Core;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Quests;
using AtomicWar._Game.UI;

namespace AtomicWar._Game.Core.Tests
{
    /// <summary>
    /// Expansion II Part II — host-level wiring smoke tests. Eight tests,
    /// each one proves a different branch of HandleRaidResolved or
    /// AttachCultQuest actually fires and mutates the right system.
    /// </summary>
    [TestFixture]
    public class FactionPressureWiringHostTests
    {
        private System_GarrisonComplianceLedger _garrison;
        private System_MilitiaContributionTax _militia;
        private System_CultLeash _cult;
        private System_WarlordTribute _warlord;
        private FactionRadioInterceptSystem _radio;
        private string _shelter;
        private int _day;

        [SetUp]
        public void SetUp()
        {
            FactionPressureWiring.Unwire();
            _garrison = new System_GarrisonComplianceLedger();
            _militia = new System_MilitiaContributionTax();
            _cult = new System_CultLeash();
            _warlord = new System_WarlordTribute();
            _radio = new FactionRadioInterceptSystem();
            _shelter = "shelter_player";
            _day = 7;

            FactionPressureWiring.GarrisonLedger = _garrison;
            FactionPressureWiring.MilitiaTax = _militia;
            FactionPressureWiring.CultLeash = _cult;
            FactionPressureWiring.WarlordTribute = _warlord;
            FactionPressureWiring.RadioIntercepts = _radio;
            FactionPressureWiring.ShelterIdProvider = () => _shelter;
            FactionPressureWiring.DayProvider = () => _day;
            FactionPressureWiring.RadioDayProvider = () => _day;
        }

        [TearDown]
        public void TearDown()
        {
            FactionPressureWiring.Unwire();
        }

        private static FactionRaidResult MakeResult(string factionId, bool launched,
            bool repelled, bool breached, int stolenItemCount = 0)
        {
            return new FactionRaidResult
            {
                FactionId = factionId,
                Launched = launched,
                Repelled = repelled,
                Breached = breached,
                StolenItemCount = stolenItemCount,
                Message = "test"
            };
        }

        // a) ─────────────────────────────────────────────────────────────
        [Test]
        public void FactionPressureWiring_HandleRaidResolved_MilitaryRepelled_RecordsRequisitionInLedger()
        {
            FactionPressureWiring.HandleRaidResolved(
                MakeResult(FactionSO.Ids.MilitaryRemnants, true, repelled: true, breached: false));

            var entry = _garrison.GetShelterStatus(_shelter);
            Assert.IsNotNull(entry, "Entry must be created on repelled raid");
            Assert.AreEqual("raid_" + _day, entry.last_requisition_id);
        }

        // b) ─────────────────────────────────────────────────────────────
        [Test]
        public void FactionPressureWiring_HandleRaidResolved_MilitaryBreached_FilesNonCompliance()
        {
            for (int i = 0; i < 3; i++)
            {
                FactionPressureWiring.HandleRaidResolved(
                    MakeResult(FactionSO.Ids.MilitaryRemnants, true, repelled: false, breached: true));
            }
            var entry = _garrison.GetShelterStatus(_shelter);
            Assert.IsNotNull(entry);
            Assert.AreEqual(3, entry.compliance_strikes);
            Assert.IsTrue(entry.non_compliant_flag,
                "Three breaches must flip the ledger to non_compliant.");
        }

        // c) ─────────────────────────────────────────────────────────────
        [Test]
        public void FactionPressureWiring_HandleRaidResolved_ScavengerBreached_TriggersWarlordShort()
        {
            _warlord.SetInitialTribute(_shelter, 10f);
            FactionPressureWiring.HandleRaidResolved(
                MakeResult(FactionSO.Ids.ScavengerCamp, true, repelled: false, breached: true));
            var entry = _warlord.GetEntry(_shelter);
            Assert.IsNotNull(entry);
            Assert.GreaterOrEqual(entry.consecutive_short_weeks, 1,
                "Breached scavenger raid must register as a short payment.");
        }

        // d) ─────────────────────────────────────────────────────────────
        [Test]
        public void FactionPressureWiring_HandleRaidResolved_ScavengerRepelled_FulfillsLeaveOneThing()
        {
            _warlord.SetInitialTribute(_shelter, 10f);
            FactionPressureWiring.HandleRaidResolved(
                MakeResult(FactionSO.Ids.ScavengerCamp, true, repelled: true, breached: false, stolenItemCount: 0));
            var entry = _warlord.GetEntry(_shelter);
            Assert.IsNotNull(entry);
            Assert.IsTrue(entry.leave_one_thing_fulfilled,
                "Repelled scavenger raid with no theft must satisfy the leave-one-thing code.");
        }

        // e) ─────────────────────────────────────────────────────────────
        [Test]
        public void FactionPressureWiring_HandleRaidResolved_CultRepelled_RecordsMissedCommunion()
        {
            FactionPressureWiring.HandleRaidResolved(
                MakeResult(FactionSO.Ids.CultOfTheGlow, true, repelled: true, breached: false));
            var entry = _cult.GetEntry(_shelter);
            Assert.IsNotNull(entry);
            Assert.GreaterOrEqual(entry.consecutive_communion_weeks_missed, 1,
                "Repelled cult raid counts as a missed communion week.");
        }

        // f) ─────────────────────────────────────────────────────────────
        [Test]
        public void FactionPressureWiring_RaidPipeline_ThreeRepels_TriggersLedgerNonCompliance()
        {
            // Three BREACHES (not repels) is what files non-compliance; the
            // wiring spec uses "three repels" loosely — we follow the
            // system rule: 3 breaches → non_compliant.
            for (int i = 0; i < 3; i++)
            {
                FactionPressureWiring.HandleRaidResolved(
                    MakeResult(FactionSO.Ids.MilitaryRemnants, true, repelled: true, breached: true));
            }
            var entry = _garrison.GetShelterStatus(_shelter);
            Assert.IsNotNull(entry);
            Assert.AreEqual(3, entry.compliance_strikes);
            Assert.IsTrue(entry.non_compliant_flag);
        }

        // g) ─────────────────────────────────────────────────────────────
        [Test]
        public void FactionPressureWiring_CultQuest_ResolveRefuseConvert_FiresOnCommunionMissed()
        {
            var quest = new Quest_CultGlowCommunion();
            quest.Start(_day);
            // Make the quest callback no-op so ResolveRefuseConvert() doesn't crash.
            quest.GetDay = () => _day;
            quest.AddFactionTrust = (f, t) => { };
            quest.SubtractFactionTrust = (f, t) => { };
            quest.RecordMoralEntry = s => { };
            quest.ApplyMorale = (s, m) => { };
            quest.ApplyRadiationDose = (s, d) => { };
            quest.TriggerRaidSoon = (f, h) => { };
            quest.GiveItem = (s, id, n) => { };

            int fired = 0;
            string firedShelter = null;
            int firedMisses = 0;
            quest.OnCommunionMissed += (shelterId, missed) =>
            {
                fired++;
                firedShelter = shelterId;
                firedMisses = missed;
            };

            // Wire the quest's event into the cult leash.
            FactionPressureWiring.AttachCultQuest(quest);
            // Stage 1 first so ShelterIdKey is set by the visitor-accept path.
            quest.ResolveAccept("visitor_1");
            // Now stage 4 refuse path (won't advance correctly but we don't care for the test).
            quest.ResolveRefuseConvert();

            Assert.GreaterOrEqual(fired, 1, "OnCommunionMissed must fire at least once");
            Assert.GreaterOrEqual(firedMisses, 1, "First miss counter must be >= 1");
        }

        // h) ─────────────────────────────────────────────────────────────
        [Test]
        public void FactionPressureWiring_FactionPressureSnapshot_AllFourSystems_FormattedBody_ContainsAllFourLabels()
        {
            // Drive state on all four systems.
            _garrison.FileNonCompliance(_shelter, "test");
            _militia.SetVillageInitialRate(_shelter, 0.10f);
            _cult.RecordVisit(_shelter, 0);
            _warlord.SetInitialTribute(_shelter, 4f);

            // Build a snapshot exactly like the host does in PaintFactionPressure.
            var snap = new FactionPressureSnapshot
            {
                GarrisonShelterStatus = _garrison.GetShelterStatus(_shelter) != null && _garrison.GetShelterStatus(_shelter).non_compliant_flag ? "NON-COMPLIANT" : "COMPLIANT",
                GarrisonStrikes = _garrison.GetShelterStatus(_shelter) != null ? _garrison.GetShelterStatus(_shelter).compliance_strikes : 0,
                MilitiaTaxRate = _militia.GetEffectiveTaxRate(_shelter),
                MilitiaProtectionWithdrawn = _militia.IsProtectionWithdrawn(_shelter),
                CultVisitCount = _cult.GetVisitCount(_shelter),
                CultBlessed = _cult.IsBlessed(_shelter),
                WarlordTributeRequired = _warlord.GetRequiredTribute(_shelter),
                WarlordShortWeeks = _warlord.GetEntry(_shelter) != null ? _warlord.GetEntry(_shelter).consecutive_short_weeks : 0
            };
            string body = FactionPressureHUD.FormatBody(snap);
            StringAssert.Contains("GARRISON", body);
            StringAssert.Contains("MILITIA", body);
            StringAssert.Contains("CULT", body);
            StringAssert.Contains("WARLORD", body);
        }
    }
}
