using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Inventory;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class ContractorRosterSystemTests
    {
        [Fact] public void GenerateOffer_CreatesOffer()
        {
            var c = Create(out _, out _, out _);
            var r = c.GenerateOffer("drifter_1", "guard", new System.Collections.Generic.List<string>(), 20, 2, 10);
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.Single(c.State.activeOffers);
        }

        [Fact] public void AcceptOffer_WithoutFunds_Blocks()
        {
            var c = Create(out var inv, out _, out _);
            c.GenerateOffer("drifter_1", "guard", new System.Collections.Generic.List<string>(), 20, 2, 10);
            var r = c.AcceptOffer(c.State.activeOffers[0].offerId);
            Assert.Equal(ActionResult.StatusKind.Blocked, r.Status);
        }

        [Fact] public void AcceptOffer_WithFunds_HiresContractor()
        {
            var c = Create(out var inv, out _, out _);
            inv.AddById("scrap_metal", 100);
            c.GenerateOffer("drifter_1", "guard", new System.Collections.Generic.List<string>(), 20, 2, 10);
            var r = c.AcceptOffer(c.State.activeOffers[0].offerId);
            Assert.True(r.Status == ActionResult.StatusKind.Success, "FailureCode: " + r.FailureCode);
            Assert.Single(c.State.contractors);
            Assert.Equal(ContractStatus.Active, c.State.contractors[0].status);
        }

        [Fact] public void TickDay_UnpaidContractor_MissedPayment()
        {
            var c = Create(out var inv, out _, out _);
            inv.AddById("scrap_metal", 100);
            c.GenerateOffer("drifter_1", "guard", new System.Collections.Generic.List<string>(), 20, 2, 10);
            c.AcceptOffer(c.State.activeOffers[0].offerId);
            // Remove funds so next payment fails
            inv.RemoveById("scrap_metal", inv.CountById("scrap_metal"));
            for (int i = 0; i < 5; i++) c.TickDay(i + 2);
            Assert.Equal(3, c.State.contractors[0].missedPayments);
        }

        [Fact] public void TickDay_OnExpiryDay_DoesNotAccrueMissedPayment()
        {
            // Bug-06 regression: on the exact expiry day, missedPayments must
            // not increment even when the player cannot pay — the contractor
            // is leaving anyway and should not suffer a loyalty penalty on
            // their last day.
            var c = Create(out var inv, out _, out _);
            inv.AddById("scrap_metal", 100);
            // Step the system forward to stripe 9 (no contractors yet — payments do nothing).
            for (int d = 0; d <= 9; d++) c.TickDay(d);
            // Hire on day 9 with termDays = 3 → expiryDay = 12.
            c.GenerateOffer("drifter_1", "guard",
                new System.Collections.Generic.List<string>(),
                initialFee: 20, dailyPay: 5, termDays: 3);
            c.AcceptOffer(c.State.activeOffers[0].offerId);
            Assert.Equal(9, c.State.contractors[0].startDay);
            Assert.Equal(12, c.State.contractors[0].expiryDay);
            // Drain funds so payment on the expiry day would fail.
            inv.RemoveById("scrap_metal", inv.CountById("scrap_metal"));
            // Tick the exact expiry day — should expire, not accrue a missed payment.
            c.TickDay(12);
            var contractor = c.State.contractors[0];
            Assert.Equal(ContractStatus.Expired, contractor.status);
            Assert.Equal(0, contractor.missedPayments);
        }

        [Fact] public void Dismiss_RemovesContractor()
        {
            var c = Create(out var inv, out _, out _);
            inv.AddById("scrap_metal", 100);
            c.GenerateOffer("drifter_1", "guard", new System.Collections.Generic.List<string>(), 20, 2, 10);
            c.AcceptOffer(c.State.activeOffers[0].offerId);
            var r = c.Dismiss("drifter_1");
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.Equal(ContractStatus.Dismissed, c.State.contractors[0].status);
        }

        [Fact] public void CaptureRestoreState_PreservesContractors()
        {
            var c = Create(out _, out _, out _);
            c.GenerateOffer("drifter_1", "guard", new System.Collections.Generic.List<string>(), 20, 2, 10);
            var state = c.CaptureState();
            Assert.Single(state.activeOffers);

            var c2 = Create(out _, out _, out _);
            c2.RestoreState(state);
            Assert.Single(c2.State.activeOffers);
        }

        private static ContractorRosterSystem Create(out Inventory.Inventory inv, out DutyRosterSystem roster, out ExpeditionSystem expedition)
        {
            inv = new Inventory.Inventory();
            roster = new DutyRosterSystem();
            expedition = new ExpeditionSystem();
            return new ContractorRosterSystem(new SeededRng(42), inv, roster, expedition);
        }
    }
}
