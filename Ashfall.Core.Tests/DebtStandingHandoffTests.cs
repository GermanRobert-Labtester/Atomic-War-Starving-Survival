using System;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Muster;
using Ashfall.Core.YearOfAsh;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class DebtStandingHandoffTests
    {
        private const string Debtor = "shelter_prime";
        private const string FactionSupplyCorps = "faction_supply_corps";

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
                        LedgerDebtSystem ledger)
            CreateHarness(int startingDay = 100)
        {
            int currentDay = startingDay;
            var ledger = new LedgerDebtSystem();
            var catalog = LoadCatalog();
            var dispatcher = new DebtConsequenceDispatcher(ledger, catalog);
            dispatcher.SetDayProvider(() => currentDay);

            var factionWar = new FactionWarSystem();
            var embargoes = new FactionEmbargoLedger();

            var bridge = new DebtConsequenceHostBridge(
                dispatcher,
                factionWar,
                embargoes,
                () => currentDay,
                NullLog.Instance);

            return (bridge, dispatcher, factionWar, ledger);
        }

        [Fact]
        public void F6_T1_DirectPenaltyWire_MildReducesStandingBy5()
        {
            var (_, dispatcher, factionWar, _) = CreateHarness();
            var catalog = LoadCatalog();
            var conseq = catalog.GetConsequence("conseq_standing_loss_mild")!;

            factionWar.ModifyStanding(FactionSupplyCorps, 10);
            Assert.Equal(10, factionWar.GetStanding(FactionSupplyCorps));

            var contract = new DebtContract { debtorId = Debtor, creditorId = FactionSupplyCorps, signedDay = 100 };
            dispatcher.DispatchConsequence(conseq, contract);

            Assert.Equal(5, factionWar.GetStanding(FactionSupplyCorps));
        }

        [Fact]
        public void F6_T2_SeverePenaltyWire_SevereReducesStandingBy12()
        {
            var (_, dispatcher, factionWar, _) = CreateHarness();
            var catalog = LoadCatalog();
            var conseq = catalog.GetConsequence("conseq_standing_loss_moderate")!;

            factionWar.ModifyStanding(FactionSupplyCorps, 20);
            var contract = new DebtContract { debtorId = Debtor, creditorId = FactionSupplyCorps, signedDay = 100 };
            dispatcher.DispatchConsequence(conseq, contract);

            Assert.Equal(8, factionWar.GetStanding(FactionSupplyCorps));
        }

        [Fact]
        public void F6_T3_OneShotPenalty_RepeatedTicksDoNotRePenalize()
        {
            var (_, _, factionWar, ledger) = CreateHarness(100);
            var catalog = LoadCatalog();
            var template = catalog.GetTemplate("debt_supply_corps_rations")!;

            factionWar.ModifyStanding(FactionSupplyCorps, 20);
            ReadTwiceSign(ledger, Debtor, template, 100);

            // Tick past expiry (day 121) -> forfeit triggers
            for (int d = 1; d <= 22; d++)
                ledger.TickDaily(100 + d);

            Assert.Equal(15, factionWar.GetStanding(FactionSupplyCorps)); // 20 - 5

            // Tick many more days
            for (int d = 23; d <= 50; d++)
                ledger.TickDaily(100 + d);

            Assert.Equal(15, factionWar.GetStanding(FactionSupplyCorps));
        }

        [Fact]
        public void F6_T4_MultipleDebts_IndependentPenalties()
        {
            var (_, _, factionWar, ledger) = CreateHarness(100);
            var catalog = LoadCatalog();
            var template = catalog.GetTemplate("debt_supply_corps_rations")!;

            factionWar.ModifyStanding(FactionSupplyCorps, 20);

            ReadTwiceSign(ledger, "debtor_1", template, 100);
            ReadTwiceSign(ledger, "debtor_2", template, 100);

            for (int d = 1; d <= 22; d++)
                ledger.TickDaily(100 + d);

            // Both defaulted -> 20 - 5 - 5 = 10
            Assert.Equal(10, factionWar.GetStanding(FactionSupplyCorps));
        }

        [Fact]
        public void F6_T5_Clamping_StandingReductionClampsToMinus100()
        {
            var (_, dispatcher, factionWar, _) = CreateHarness();
            var catalog = LoadCatalog();
            var conseq = catalog.GetConsequence("conseq_standing_loss_moderate")!;

            factionWar.ModifyStanding(FactionSupplyCorps, -95);
            Assert.Equal(-95, factionWar.GetStanding(FactionSupplyCorps));

            var contract = new DebtContract { debtorId = Debtor, creditorId = FactionSupplyCorps, signedDay = 100 };
            dispatcher.DispatchConsequence(conseq, contract);

            Assert.Equal(-100, factionWar.GetStanding(FactionSupplyCorps));
        }

        [Fact]
        public void F6_T6_HostilityThreshold_ReachingMinus50SetsHostile()
        {
            var (_, dispatcher, factionWar, _) = CreateHarness();
            var catalog = LoadCatalog();
            var conseq = catalog.GetConsequence("conseq_standing_loss_mild")!; // -5

            factionWar.ModifyStanding(FactionSupplyCorps, -48);
            Assert.False(factionWar.IsHostile(FactionSupplyCorps));

            var contract = new DebtContract { debtorId = Debtor, creditorId = FactionSupplyCorps, signedDay = 100 };
            dispatcher.DispatchConsequence(conseq, contract);

            Assert.Equal(-53, factionWar.GetStanding(FactionSupplyCorps));
            Assert.True(factionWar.IsHostile(FactionSupplyCorps));
        }

        [Fact]
        public void F6_T7_RepaymentBonus_OnTimeRepaymentGrantsPlus2()
        {
            var (_, _, factionWar, ledger) = CreateHarness(100);
            var catalog = LoadCatalog();
            var template = catalog.GetTemplate("debt_supply_corps_rations")!;

            factionWar.ModifyStanding(FactionSupplyCorps, 10);
            ReadTwiceSign(ledger, Debtor, template, 100);

            // Repay on day 105 (well before 20d term)
            bool paid = ledger.PayContract(Debtor, 105);
            Assert.True(paid);

            Assert.Equal(12, factionWar.GetStanding(FactionSupplyCorps)); // 10 + 2
        }

        [Fact]
        public void F6_T8_RepaymentBonusOneShot_PayingTwiceDoesNotGrantPlus4()
        {
            var (bridge, _, factionWar, ledger) = CreateHarness(100);
            var catalog = LoadCatalog();
            var template = catalog.GetTemplate("debt_supply_corps_rations")!;

            factionWar.ModifyStanding(FactionSupplyCorps, 10);
            ReadTwiceSign(ledger, Debtor, template, 100);

            ledger.PayContract(Debtor, 105);
            Assert.Equal(12, factionWar.GetStanding(FactionSupplyCorps));
            Assert.Equal(1, bridge.RepaymentBonusApplications);

            // Repeated call returns false and does not grant bonus
            bool secondPay = ledger.PayContract(Debtor, 106);
            Assert.False(secondPay);
            Assert.Equal(12, factionWar.GetStanding(FactionSupplyCorps));
            Assert.Equal(1, bridge.RepaymentBonusApplications);
        }

        [Fact]
        public void F6_T9_DefaultedDebtRepayment_PayingAfterDefaultDoesNotGrantBonus()
        {
            var (bridge, _, factionWar, ledger) = CreateHarness(100);
            var catalog = LoadCatalog();
            var template = catalog.GetTemplate("debt_supply_corps_rations")!;

            factionWar.ModifyStanding(FactionSupplyCorps, 10);
            ReadTwiceSign(ledger, Debtor, template, 100);

            // Forfeit at day 121 (10 - 5 = 5)
            for (int d = 1; d <= 22; d++)
                ledger.TickDaily(100 + d);

            Assert.Equal(5, factionWar.GetStanding(FactionSupplyCorps));

            // Debtor settles late at day 130
            ledger.PayContract(Debtor, 130);

            // Standing must remain at 5; no +2 repayment bonus for defaulted debt
            Assert.Equal(5, factionWar.GetStanding(FactionSupplyCorps));
            Assert.Equal(0, bridge.RepaymentBonusApplications);
        }

        [Fact]
        public void F6_T10_DefaultPenaltyNonReversibility_PayingAfterDefaultDoesNotReversePenalty()
        {
            var (_, _, factionWar, ledger) = CreateHarness(100);
            var catalog = LoadCatalog();
            var template = catalog.GetTemplate("debt_supply_corps_rations")!;

            factionWar.ModifyStanding(FactionSupplyCorps, 10);
            ReadTwiceSign(ledger, Debtor, template, 100);

            // Advance to default
            for (int d = 1; d <= 22; d++)
                ledger.TickDaily(100 + d);

            Assert.Equal(5, factionWar.GetStanding(FactionSupplyCorps));

            // Pay debt
            ledger.PayContract(Debtor, 130);

            // Invariant: historical default penalty != reversible collateral
            Assert.Equal(5, factionWar.GetStanding(FactionSupplyCorps));
        }

        [Fact]
        public void F6_T11_ForgivenessStanding_ConsequenceGrantsPlus5()
        {
            var (_, dispatcher, factionWar, ledger) = CreateHarness(100);
            var catalog = LoadCatalog();
            var conseq = catalog.GetConsequence("conseq_forgiveness_rare")!;

            factionWar.ModifyStanding(FactionSupplyCorps, 10);
            var template = catalog.GetTemplate("debt_supply_corps_rations")!;
            ReadTwiceSign(ledger, Debtor, template, 100);

            var contract = ledger.GetContract(Debtor)!;
            dispatcher.DispatchConsequence(conseq, contract);

            // 10 + 5 = 15
            Assert.Equal(15, factionWar.GetStanding(FactionSupplyCorps));
            // Contract marked forgiven
            var active = ledger.GetContract(Debtor);
            Assert.True(active == null || active.forgiven);
        }

        [Fact]
        public void F6_T12_SaveLoadRoundtrip_RepaymentBonusPersists()
        {
            var (bridge, dispatcher, factionWar, ledger) = CreateHarness(100);
            var catalog = LoadCatalog();
            var template = catalog.GetTemplate("debt_supply_corps_rations")!;

            ReadTwiceSign(ledger, Debtor, template, 100);
            ledger.PayContract(Debtor, 105);

            Assert.Single(bridge.RepaymentBonusGranted);

            var state = bridge.CaptureState();

            var freshBridge = new DebtConsequenceHostBridge(
                dispatcher,
                factionWar,
                new FactionEmbargoLedger(),
                () => 110,
                NullLog.Instance);

            freshBridge.RestoreState(state);

            Assert.Single(freshBridge.RepaymentBonusGranted);
            Assert.Equal(Debtor + "@100", freshBridge.RepaymentBonusGranted[0]);
        }
    }
}
