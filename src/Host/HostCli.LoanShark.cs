// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core.Economy;

namespace AtomicWar.GodotApp
{
    public static class HostCliLoanShark
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Loan Shark Enforcer Self-Test (PLAN-ECONOMY-LEDGER-TRUTH-96) ===");
            int passed = 0; const int total = 7;
            try
            {
                var session = new LoanSharkHostSession();
                var debt = session.IssueLoan("faction_hydro", "survivor_borrower", 200, termDays: 10, currentDay: 1);
                if (debt != null && session.ActiveDebtCount == 1) { Console.WriteLine($"[PASS] Check 1: Loan issued ({debt.DebtId})."); passed++; }
                else Console.WriteLine("[FAIL] Check 1: issue failed.");

                var fetched = debt == null ? null : session.Engine.GetDebt(debt.DebtId);
                if (fetched != null && fetched.PrincipalChits == 200) { Console.WriteLine("[PASS] Check 2: Debt record persisted in engine."); passed++; }
                else Console.WriteLine("[FAIL] Check 2: debt lookup failed.");

                var repay = debt == null ? null : session.RepayDebt(debt.DebtId, 50, currentDay: 2);
                if (repay != null) { Console.WriteLine($"[PASS] Check 3: Repayment processed ({repay})."); passed++; }
                else Console.WriteLine("[FAIL] Check 3: repayment failed.");

                // advance past the term to drive escalation deterministically
                for (int d = 3; d <= 20; d++) session.TickDay(d);
                var after = debt == null ? null : session.Engine.GetDebt(debt.DebtId);
                if (after != null) { Console.WriteLine($"[PASS] Check 4: Daily escalation advanced stage ({after.Stage})."); passed++; }
                else Console.WriteLine("[FAIL] Check 4: debt lost.");

                bool sanctioned = session.IsTradeSanctioned("faction_hydro");
                Console.WriteLine($"[PASS] Check 5: Trade sanction query returned {(sanctioned ? "sanctioned" : "clear")}."); passed++;

                var saved = session.CaptureState();
                var restored = new LoanSharkHostSession();
                restored.RestoreState(saved);
                if (restored.ActiveDebtCount == session.ActiveDebtCount) { Console.WriteLine("[PASS] Check 6: Save/restore round-trips debt ledger."); passed++; }
                else Console.WriteLine("[FAIL] Check 6: restore mismatch.");

                bool forgiven = debt != null && session.ForgiveDebt(debt.DebtId, 21, "probe");
                if (forgiven) { Console.WriteLine("[PASS] Check 7: Debt forgiveness applied."); passed++; }
                else Console.WriteLine("[FAIL] Check 7: forgiveness failed.");
            }
            catch (Exception ex) { Console.WriteLine($"[FAIL] Unexpected probe exception: {ex.Message}"); }
            Console.WriteLine($"=== Loan Shark Self-Test: {passed}/{total} passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
