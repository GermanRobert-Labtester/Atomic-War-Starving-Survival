// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Economy;
using Ashfall.Core.Factions;
using Xunit;

namespace Ashfall.Core.Tests.Economy
{
    public class LoanSharkEnforcerEngineTests
    {
        [Fact]
        public void IssueLoan_ValidInput_DisbursesFundsAndTracksDebt()
        {
            var funds = new FundsLedger(initialBalance: 50);
            var engine = new LoanSharkEnforcerEngine(fundsLedger: funds);

            var debt = engine.IssueLoan(
                creditorFactionId: "syndicate_black_market",
                debtorId: "player_shelter",
                principalChits: 300,
                termDays: 7,
                dailyInterestPermille: 20,
                gracePeriodDays: 3,
                currentDay: 5);

            Assert.NotNull(debt);
            Assert.Equal(350, funds.Balance); // 50 initial + 300 disbursed
            Assert.Equal(300, debt.PrincipalChits);
            Assert.Equal(300, debt.CurrentBalanceChits);
            Assert.Equal(5, debt.IssuedDay);
            Assert.Equal(12, debt.DueDay); // 5 + 7
            Assert.Equal(3, debt.GracePeriodDays);
            Assert.Equal(LoanEscalationStage.Current, debt.Stage);
            Assert.Single(engine.ActiveDebts);
        }

        [Fact]
        public void DailyInterestAccrual_CompoundsDeterministically_FloorsAtOneChit()
        {
            var engine = new LoanSharkEnforcerEngine();
            // Principal 1000 chits, 20 permille (2% daily)
            var debt = engine.IssueLoan("syndicate_loan", "player", 1000, 10, 20, 3, currentDay: 1);

            // Day 2 (1 day elapsed): 1000 * 20 / 1000 = 20 interest -> 1020
            engine.ProcessDailyTick(currentDay: 2);
            Assert.Equal(1020, debt.CurrentBalanceChits);
            Assert.Equal(20, debt.TotalInterestAccrued);

            // Day 3 (1 day elapsed): 1020 * 20 / 1000 = 20 interest -> 1040
            engine.ProcessDailyTick(currentDay: 3);
            Assert.Equal(1040, debt.CurrentBalanceChits);
            Assert.Equal(40, debt.TotalInterestAccrued);

            // Small balance: 10 chits, 20 permille -> 10 * 20 / 1000 = 0 -> floors to 1 chit
            var smallDebt = engine.IssueLoan("syndicate_loan", "player", 10, 10, 20, 3, currentDay: 3);
            engine.ProcessDailyTick(currentDay: 4);
            Assert.Equal(11, smallDebt.CurrentBalanceChits);
        }

        [Fact]
        public void EscalationStages_ProgressFromCurrentToGraceToDelinquentToDefault()
        {
            var bountySystem = new FactionBountySystem();
            var engine = new LoanSharkEnforcerEngine(bountySystem: bountySystem);

            // Issued day 1, term 5 days -> Due day 6. Grace period 2 days (days 7-8). Delinquent days 9-13. Default day 14+
            var debt = engine.IssueLoan("syndicate_enforcers", "player", 600, termDays: 5, dailyInterestPermille: 10, gracePeriodDays: 2, currentDay: 1);

            // Day 5: Still within term
            engine.ProcessDailyTick(5);
            Assert.Equal(LoanEscalationStage.Current, debt.Stage);
            Assert.False(engine.IsTradeSanctioned("syndicate_enforcers"));

            // Day 7: Past due day 6, within grace period (due + 2 = 8)
            engine.ProcessDailyTick(7);
            Assert.Equal(LoanEscalationStage.Grace, debt.Stage);
            Assert.False(engine.IsTradeSanctioned("syndicate_enforcers"));

            // Day 9: Past grace period -> Delinquent
            engine.ProcessDailyTick(9);
            Assert.Equal(LoanEscalationStage.Delinquent, debt.Stage);
            Assert.True(engine.IsTradeSanctioned("syndicate_enforcers"));

            // Day 14: Past delinquency threshold (8 + 5 = 13) -> Defaulted
            engine.ProcessDailyTick(14);
            Assert.Equal(LoanEscalationStage.Defaulted, debt.Stage);
            Assert.True(debt.BountyPlaced);
            Assert.NotNull(debt.AssociatedBountyId);
            Assert.True(debt.EnforcerRaidRiskPermille >= 150);

            // Bounty verified in FactionBountySystem
            var activeBounties = bountySystem.GetActiveBounties();
            Assert.Single(activeBounties);
            Assert.Equal(debt.AssociatedBountyId, activeBounties[0].BountyId);
            Assert.Equal(FactionBountySeverity.Severe, activeBounties[0].Severity); // 600+ chits -> -15 Severe
        }

        [Fact]
        public void RepayDebt_SettlesBalanceViaFundsLedger_ClearsSanctionsAndRaidRisk()
        {
            var funds = new FundsLedger(initialBalance: 1000);
            var engine = new LoanSharkEnforcerEngine(fundsLedger: funds);

            var debt = engine.IssueLoan("loan_shark", "player", 400, 5, 0, 1, currentDay: 1);
            Assert.Equal(1400, funds.Balance);

            // Fast forward to delinquent stage
            engine.ProcessDailyTick(10);
            Assert.Equal(LoanEscalationStage.Delinquent, debt.Stage);
            Assert.True(engine.IsTradeSanctioned("loan_shark"));

            // Partial repayment
            var partialRes = engine.RepayDebt(debt.DebtId, 150, currentDay: 10);
            Assert.True(partialRes.Success);
            Assert.Equal(250, debt.CurrentBalanceChits);
            Assert.Equal(1250, funds.Balance);
            Assert.Equal(LoanEscalationStage.Delinquent, debt.Stage);

            // Full repayment
            var fullRes = engine.RepayDebt(debt.DebtId, 300, currentDay: 10); // 300 exceeds 250, clamped to 250
            Assert.True(fullRes.Success);
            Assert.Equal(0, debt.CurrentBalanceChits);
            Assert.Equal(1000, funds.Balance);
            Assert.Equal(LoanEscalationStage.Settled, debt.Stage);
            Assert.Equal(0, debt.EnforcerRaidRiskPermille);
            Assert.False(engine.IsTradeSanctioned("loan_shark"));
            Assert.Empty(engine.ActiveDebts);
        }

        [Fact]
        public void SaveRestoreRoundTrip_PreservesAllDebtRecordsAndEscalationState()
        {
            var engine1 = new LoanSharkEnforcerEngine();
            var debt1 = engine1.IssueLoan("syndicate_alpha", "player", 500, 3, 25, 2, currentDay: 1);
            engine1.ProcessDailyTick(8); // past grace -> Delinquent

            var saveState = engine1.CaptureState();
            Assert.NotNull(saveState);
            Assert.Single(saveState.Debts);

            var engine2 = new LoanSharkEnforcerEngine();
            engine2.RestoreState(saveState);

            Assert.Single(engine2.Debts);
            var restored = engine2.GetDebt(debt1.DebtId);
            Assert.NotNull(restored);
            Assert.Equal(debt1.DebtId, restored.DebtId);
            Assert.Equal(debt1.CreditorFactionId, restored.CreditorFactionId);
            Assert.Equal(debt1.CurrentBalanceChits, restored.CurrentBalanceChits);
            Assert.Equal(debt1.Stage, restored.Stage);
            Assert.Equal(debt1.TotalInterestAccrued, restored.TotalInterestAccrued);
        }
    }
}
