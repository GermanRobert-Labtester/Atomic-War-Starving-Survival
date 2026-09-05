using System;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Muster;
using Ashfall.Core.YearOfAsh;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class DebtBountyHandoffTests
    {
        private const string Debtor = "shelter_alpha";
        private const string CreditorA = "faction_supply_corps";
        private const string CreditorB = "faction_iron_raiders";

        private static string DataDir()
        {
            if (CatalogLocator.TryFindDataDirectory(System.IO.Directory.GetCurrentDirectory(), out string found))
                return found;
            if (CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out found))
                return found;
            throw new System.IO.DirectoryNotFoundException("Assets/StreamingAssets/Data not found");
        }

        private static DebtTemplateCatalog LoadCatalog()
        {
            var catalog = DebtTemplateCatalogLoader.Load(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.True(catalog.Errors.Count == 0, "catalog errors: " + string.Join("; ", catalog.Errors));
            return catalog;
        }

        private static void ReadTwiceSign(LedgerDebtSystem ledger, string debtor, DebtTemplate template, int day)
        {
            ledger.PresentContract(debtor, template.principalQuantity, template.termDays, template.rate, template.forfeitDescription, template.creditorId, template.id);
            ledger.PresentContract(debtor, template.principalQuantity, template.termDays, template.rate, template.forfeitDescription, template.creditorId, template.id);
            ledger.SignContract(debtor, day);
        }

        private static (DebtConsequenceHostBridge bridge,
                        DebtConsequenceDispatcher dispatcher,
                        FactionWarSystem factionWar,
                        IronRaidersSystem raiders,
                        LedgerDebtSystem ledger)
            CreateHarness(int startingDay = 50)
        {
            int currentDay = startingDay;
            var ledger = new LedgerDebtSystem();
            var catalog = LoadCatalog();
            var dispatcher = new DebtConsequenceDispatcher(ledger, catalog);
            dispatcher.SetDayProvider(() => currentDay);

            var factionWar = new FactionWarSystem();
            var embargoes = new FactionEmbargoLedger();
            var raiders = new IronRaidersSystem();

            var bridge = new DebtConsequenceHostBridge(
                dispatcher,
                factionWar,
                embargoes,
                () => currentDay,
                NullLog.Instance,
                ironRaiders: raiders);

            return (bridge, dispatcher, factionWar, raiders, ledger);
        }

        [Fact]
        public void F5_T1_BountyConsequenceCreatesPendingBountyRecord()
        {
            var (bridge, dispatcher, _, raiders, ledger) = CreateHarness(50);
            var catalog = LoadCatalog();
            var template = catalog.GetTemplate("debt_railway_guild_transport")!;

            ReadTwiceSign(ledger, Debtor, template, 50);

            // Tick past due window to trigger default
            for (int d = 0; d < template.termDays; d++)
                ledger.TickDaily(51 + d);

            Assert.True(raiders.State.isActive);
            var bounty = bridge.Bounties.FirstOrDefault(b => b.severity == "moderate");
            Assert.NotNull(bounty);
            Assert.Equal(DebtBountyStatus.Pending, bounty.status);
            Assert.Equal(50, bounty.issuedDay);
            Assert.Equal(Debtor, bounty.sourceDebtId);
        }

        [Fact]
        public void F5_T2_RaidConsequenceCreatesSeverePendingRecord()
        {
            var (bridge, dispatcher, _, raiders, ledger) = CreateHarness(50);
            var catalog = LoadCatalog();

            // Direct consequence dispatch test for severe raid
            var raidConsequence = catalog.GetConsequence("conseq_raid_severe");
            Assert.NotNull(raidConsequence);

            var contract = new DebtContract
            {
                debtorId = Debtor,
                creditorId = CreditorA,
                signedDay = 50
            };

            dispatcher.DispatchConsequence(raidConsequence, contract);

            var severe = bridge.Bounties.FirstOrDefault(b => b.severity == "severe");
            Assert.NotNull(severe);
            Assert.Equal(DebtBountyStatus.Pending, severe.status);
            Assert.Equal(DebtBountySeverity.SevereRaidBoost, DebtBountySeverity.GetRaidChanceBoost(severe.severity));
        }

        [Fact]
        public void F5_T3_MultiCreditorStacking()
        {
            var (bridge, dispatcher, _, _, _) = CreateHarness(50);
            var catalog = LoadCatalog();

            var modConseq = new DebtConsequence
            {
                id = "conseq_bounty_moderate_test",
                effectType = "bounty",
                bountyLevel = "moderate"
            };
            var severeConseq = catalog.GetConsequence("conseq_raid_severe")!;

            var contractA = new DebtContract { debtorId = "debtor_a", creditorId = CreditorA, signedDay = 50 };
            var contractB = new DebtContract { debtorId = "debtor_b", creditorId = CreditorB, signedDay = 50 };

            dispatcher.DispatchConsequence(modConseq, contractA);
            dispatcher.DispatchConsequence(severeConseq, contractB);

            // 0.15 + 0.30 = 0.45f
            float boost = bridge.CalculateBountyBoost();
            Assert.Equal(0.45f, boost, 2);

            // Add a third bounty to hit and verify clamping at MaxAggregateBountyBoost (0.50f)
            var contractC = new DebtContract { debtorId = "debtor_c", creditorId = "faction_hydro_barons", signedDay = 50 };
            dispatcher.DispatchConsequence(modConseq, contractC);

            float clamped = bridge.CalculateBountyBoost();
            Assert.Equal(DebtBountySeverity.MaxAggregateBountyBoost, clamped, 2);
        }

        [Fact]
        public void F5_T4_PaymentCancelsPendingBounty()
        {
            var (bridge, _, _, _, ledger) = CreateHarness(50);
            var catalog = LoadCatalog();
            var template = catalog.GetTemplate("debt_railway_guild_transport")!;

            ReadTwiceSign(ledger, Debtor, template, 50);

            for (int d = 0; d < template.termDays; d++)
                ledger.TickDaily(51 + d);

            Assert.Contains(bridge.Bounties, b => b.status == DebtBountyStatus.Pending);
            Assert.True(bridge.CalculateBountyBoost() > 0f);

            // Debtor settles debt
            ledger.PayContract(Debtor, 100);

            Assert.All(bridge.Bounties.Where(b => b.sourceDebtId == Debtor),
                b => Assert.Equal(DebtBountyStatus.Cancelled, b.status));
            Assert.Equal(0f, bridge.CalculateBountyBoost());
        }

        [Fact]
        public void F5_T5_ResolvedBountyDoesNotReCancelOrDuplicate()
        {
            var (bridge, _, _, raiders, ledger) = CreateHarness(50);
            var catalog = LoadCatalog();
            var template = catalog.GetTemplate("debt_railway_guild_transport")!;

            ReadTwiceSign(ledger, Debtor, template, 50);

            for (int d = 0; d < template.termDays; d++)
                ledger.TickDaily(51 + d);

            // Raid executes and resolves bounty
            Assert.True(bridge.EvaluateDailyRaidEnforcement(96, forcedRoll: 0f));
            var resolvedBounty = bridge.Bounties.First(b => b.status == DebtBountyStatus.Resolved);
            Assert.Equal(96, resolvedBounty.resolvedDay);

            // Later, player pays the debt
            ledger.PayContract(Debtor, 100);

            // The already resolved bounty must remain Resolved
            Assert.Equal(DebtBountyStatus.Resolved, resolvedBounty.status);
            Assert.Equal(96, resolvedBounty.resolvedDay);
            Assert.Equal(-1, resolvedBounty.cancelledDay);
        }

        [Fact]
        public void F5_T6_RaidExecutionResolvesOnlyCandidateBounty()
        {
            var (bridge, dispatcher, _, raiders, _) = CreateHarness(50);
            var catalog = LoadCatalog();

            var modConseq = new DebtConsequence
            {
                id = "conseq_bounty_moderate_test",
                effectType = "bounty",
                bountyLevel = "moderate"
            };
            var severeConseq = catalog.GetConsequence("conseq_raid_severe")!;

            var contractA = new DebtContract { debtorId = "debtor_mod", creditorId = CreditorA, signedDay = 50 };
            var contractB = new DebtContract { debtorId = "debtor_sev", creditorId = CreditorB, signedDay = 50 };

            dispatcher.DispatchConsequence(modConseq, contractA);
            dispatcher.DispatchConsequence(severeConseq, contractB);

            Assert.Equal(2, bridge.Bounties.Count(b => b.status == DebtBountyStatus.Pending));

            // Force raid execution
            Assert.True(bridge.EvaluateDailyRaidEnforcement(55, forcedRoll: 0f));
            Assert.Equal(1, raiders.RaidsThisSeason);

            // Severe bounty has higher priority and should be resolved
            var severe = bridge.Bounties.First(b => b.sourceDebtId == "debtor_sev");
            var moderate = bridge.Bounties.First(b => b.sourceDebtId == "debtor_mod");

            Assert.Equal(DebtBountyStatus.Resolved, severe.status);
            Assert.Equal(55, severe.resolvedDay);
            Assert.Equal(DebtBountyStatus.Pending, moderate.status);

            // Remaining boost matches moderate only
            Assert.Equal(DebtBountySeverity.ModerateBountyBoost, bridge.CalculateBountyBoost(), 2);
        }

        [Fact]
        public void F5_T7_SaveLoadRoundtrip()
        {
            var (bridge, dispatcher, factionWar, raiders, _) = CreateHarness(50);
            var catalog = LoadCatalog();

            var modConseq = new DebtConsequence
            {
                id = "conseq_bounty_moderate_test",
                effectType = "bounty",
                bountyLevel = "moderate"
            };
            var severeConseq = catalog.GetConsequence("conseq_raid_severe")!;

            var contractA = new DebtContract { debtorId = "debtor_1", creditorId = CreditorA, signedDay = 50 };
            var contractB = new DebtContract { debtorId = "debtor_2", creditorId = CreditorB, signedDay = 50 };

            dispatcher.DispatchConsequence(modConseq, contractA);
            dispatcher.DispatchConsequence(severeConseq, contractB);

            // Resolve contractB
            bridge.EvaluateDailyRaidEnforcement(55, forcedRoll: 0f);

            // Cancel contractA
            bridge.CancelBountiesForDebt("debtor_1", 58);

            // Capture state
            var state = bridge.CaptureState();
            Assert.Equal(2, state.bounties.Count);

            // Restore into fresh bridge
            var freshBridge = new DebtConsequenceHostBridge(
                dispatcher,
                factionWar,
                new FactionEmbargoLedger(),
                () => 60,
                NullLog.Instance,
                ironRaiders: raiders);

            freshBridge.RestoreState(state);

            Assert.Equal(2, freshBridge.Bounties.Count);
            var b1 = freshBridge.Bounties.First(b => b.sourceDebtId == "debtor_1");
            Assert.Equal(DebtBountyStatus.Cancelled, b1.status);
            Assert.Equal(58, b1.cancelledDay);

            var b2 = freshBridge.Bounties.First(b => b.sourceDebtId == "debtor_2");
            Assert.Equal(DebtBountyStatus.Resolved, b2.status);
            Assert.Equal(55, b2.resolvedDay);
        }
    }
}
