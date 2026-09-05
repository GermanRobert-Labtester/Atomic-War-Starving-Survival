using System;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Muster;
using Ashfall.Core.YearOfAsh;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Flagship Task F8: End-to-End Debt Lifecycle Smoke Test.
    /// Proves that a template-linked debt progresses from offer and signing through
    /// repayment or default, produces real faction and enforcement consequences exactly once,
    /// survives save/reload into a fresh runtime, and guards against duplicate consequences
    /// across subsequent daily simulation ticks.
    /// </summary>
    public class DebtLifecycleSmokeTests
    {
        private const string Debtor = "shelter_prime";
        private const string FactionSupplyCorps = "faction_supply_corps";
        private const string FactionRailwayGuild = "faction_railway_guild";

        private static string DataDir()
        {
            if (CatalogLocator.TryFindDataDirectory(System.IO.Directory.GetCurrentDirectory(), out string found))
                return found;
            if (CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out found))
                return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found");
        }

        private static DebtTemplateCatalog LoadCatalog()
        {
            var catalog = DebtTemplateCatalogLoader.Load(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.True(catalog.Errors.Count == 0, "catalog errors: " + string.Join("; ", catalog.Errors));
            return catalog;
        }

        [Fact]
        public void F8_EndToEnd_FullDebtLifecycle_FromSigningToDefault_SaveRestore_And_OneShotInvariants()
        {
            // ── 1. Authored Catalog Loading ──────────────────────────────────────────
            var catalog = LoadCatalog();
            var tRations = catalog.GetTemplate("debt_supply_corps_rations");
            var tParts = catalog.GetTemplate("debt_railway_guild_parts");
            Assert.NotNull(tRations);
            Assert.NotNull(tParts);
            Assert.Equal(20, tRations.termDays);
            Assert.Equal(35, tParts.termDays);

            // ── 2. Harness Setup (Campaign Day 100) ───────────────────────────────────
            int currentDay = 100;
            var ledger = new LedgerDebtSystem();
            var dispatcher = new DebtConsequenceDispatcher(ledger, catalog);
            dispatcher.SetDayProvider(() => currentDay);

            var factionWar = new FactionWarSystem();
            factionWar.ModifyStanding(FactionSupplyCorps, 10);
            factionWar.ModifyStanding(FactionRailwayGuild, 20);

            var embargoes = new FactionEmbargoLedger();
            var raiders = new IronRaidersSystem();

            var bridge = new DebtConsequenceHostBridge(
                dispatcher,
                factionWar,
                embargoes,
                () => currentDay,
                NullLog.Instance,
                ironRaiders: raiders);

            // ── 3. Presentation Idempotency (Two-Read Rule) ──────────────────────────
            // Reading 1: Draft created, not signable yet
            bool p1 = ledger.PresentContract(
                Debtor,
                tRations.principalQuantity,
                tRations.termDays,
                tRations.rate,
                tRations.forfeitDescription,
                tRations.creditorId,
                tRations.id);
            Assert.True(p1);

            var draft = ledger.GetContract(Debtor);
            Assert.NotNull(draft);
            Assert.Equal(1, draft.readCount);
            Assert.False(draft.signed);

            // Cannot sign with only 1 reading
            bool signPremature = ledger.SignContract(Debtor, currentDay);
            Assert.False(signPremature);

            // Reading 2: Reaches required reading threshold
            bool p2 = ledger.PresentContract(
                Debtor,
                tRations.principalQuantity,
                tRations.termDays,
                tRations.rate,
                tRations.forfeitDescription,
                tRations.creditorId,
                tRations.id);
            Assert.True(p2);
            Assert.Equal(2, draft.readCount);
            Assert.False(draft.signed);

            // Reading 3: Terms updated/reaffirmed, read count increments (>= ReadsRequired)
            bool p3 = ledger.PresentContract(
                Debtor,
                tRations.principalQuantity,
                tRations.termDays,
                tRations.rate,
                tRations.forfeitDescription,
                tRations.creditorId,
                tRations.id);
            Assert.True(p3);
            Assert.Equal(3, draft.readCount);
            Assert.True(draft.readCount >= LedgerDebtSystem.ReadsRequired);

            // ── 4. Contract Signing ──────────────────────────────────────────────────
            bool signed1 = ledger.SignContract(Debtor, currentDay);
            Assert.True(signed1);

            var active1 = ledger.GetContract(Debtor);
            Assert.NotNull(active1);
            Assert.True(active1.signed);
            Assert.Equal(100, active1.signedDay);
            Assert.Equal(20, active1.daysRemaining);

            // ── 5. Timely Repayment & +2 Standing Bonus ──────────────────────────────
            for (int d = 1; d <= 5; d++)
            {
                currentDay = 100 + d;
                ledger.TickDaily(currentDay);
                bridge.TickDaily(currentDay);
            }
            Assert.Equal(15, active1.daysRemaining);

            // Pay debt on day 105 (before 20d term expiry)
            bool paid1 = ledger.PayContract(Debtor, currentDay);
            Assert.True(paid1);
            Assert.True(active1.paid);
            Assert.False(active1.forfeited);

            // One-shot on-time repayment bonus: 10 + 2 = 12
            Assert.Equal(12, factionWar.GetStanding(FactionSupplyCorps));
            Assert.Equal(1, bridge.RepaymentBonusApplications);

            // ── 6. Second Contract Signing & Default Lifecycle ───────────────────────
            currentDay = 110;
            // Presenting a new contract archives the previous paid contract
            ledger.PresentContract(
                Debtor,
                tParts.principalQuantity,
                tParts.termDays,
                tParts.rate,
                tParts.forfeitDescription,
                tParts.creditorId,
                tParts.id);
            Assert.Single(ledger.ClosedContracts);
            Assert.True(ledger.ClosedContracts[0].paid);

            ledger.PresentContract(
                Debtor,
                tParts.principalQuantity,
                tParts.termDays,
                tParts.rate,
                tParts.forfeitDescription,
                tParts.creditorId,
                tParts.id);
            bool signed2 = ledger.SignContract(Debtor, currentDay);
            Assert.True(signed2);

            var active2 = ledger.GetContract(Debtor);
            Assert.NotNull(active2);
            Assert.Equal(35, active2.daysRemaining);
            Assert.Equal(20, factionWar.GetStanding(FactionRailwayGuild));
            Assert.Empty(bridge.Bounties);

            // Tick past 35-day term (36 days to Day 146) -> Forfeit triggers
            for (int d = 1; d <= 36; d++)
            {
                currentDay = 110 + d;
                ledger.TickDaily(currentDay);
                bridge.TickDaily(currentDay);
            }

            Assert.True(active2.forfeited);

            // Consequence conseq_standing_loss_and_embargo dispatched:
            // standingDelta: -10 -> 20 - 10 = 10
            Assert.Equal(10, factionWar.GetStanding(FactionRailwayGuild));
            Assert.True(embargoes.IsEmbargoed(FactionRailwayGuild, currentDay));

            // Bounty records created in bridge via escalation chain (moderate -> severe):
            Assert.Equal(2, bridge.Bounties.Count);
            Assert.Contains(bridge.Bounties, b => b.severity == "moderate" && b.status == DebtBountyStatus.Pending);
            Assert.Contains(bridge.Bounties, b => b.severity == "severe" && b.status == DebtBountyStatus.Pending);
            float expectedBoost = DebtBountySeverity.ModerateBountyBoost + DebtBountySeverity.SevereRaidBoost;
            Assert.Equal(expectedBoost, bridge.CalculateBountyBoost(), 2);

            // ── 7. State Capture & Serialization ─────────────────────────────────────
            var ledgerState = ledger.CaptureState();
            var bridgeState = bridge.CaptureState();
            var factionWarState = factionWar.CaptureState();
            var dispatcherState = dispatcher.CaptureState();

            var serializer = new SystemTextJsonSerializer();
            string ledgerJson = serializer.Serialize(ledgerState);
            string bridgeJson = serializer.Serialize(bridgeState);
            string factionWarJson = serializer.Serialize(factionWarState);
            string dispatcherJson = serializer.Serialize(dispatcherState);

            var restoredLedgerState = serializer.Deserialize<LedgerDebtSystemState>(ledgerJson)!;
            var restoredBridgeState = serializer.Deserialize<DebtConsequenceBridgeState>(bridgeJson)!;
            var restoredFactionWarState = serializer.Deserialize<FactionWarSystemState>(factionWarJson)!;
            var restoredDispatcherState = serializer.Deserialize<DebtDispatcherState>(dispatcherJson)!;

            // ── 8. Restore Into Fresh Runtime ────────────────────────────────────────
            int freshDay = 146;
            var freshLedger = new LedgerDebtSystem();
            freshLedger.RestoreState(restoredLedgerState);

            var freshCatalog = LoadCatalog();
            var freshDispatcher = new DebtConsequenceDispatcher(freshLedger, freshCatalog);
            freshDispatcher.SetDayProvider(() => freshDay);
            freshDispatcher.RestoreState(restoredDispatcherState);

            var freshFactionWar = new FactionWarSystem();
            freshFactionWar.RestoreState(restoredFactionWarState);

            var freshEmbargoes = new FactionEmbargoLedger();
            var freshRaiders = new IronRaidersSystem();
            var freshBridge = new DebtConsequenceHostBridge(
                freshDispatcher,
                freshFactionWar,
                freshEmbargoes,
                () => freshDay,
                NullLog.Instance,
                ironRaiders: freshRaiders);
            freshBridge.RestoreState(restoredBridgeState);

            // Verify Restored Invariants
            Assert.Single(freshLedger.ClosedContracts);
            Assert.True(freshLedger.ClosedContracts[0].paid);

            var restoredActive = freshLedger.GetContract(Debtor);
            Assert.NotNull(restoredActive);
            Assert.True(restoredActive.forfeited);

            Assert.Equal(10, freshFactionWar.GetStanding(FactionRailwayGuild));
            Assert.Equal(12, freshFactionWar.GetStanding(FactionSupplyCorps));

            Assert.Equal(2, freshBridge.Bounties.Count);
            Assert.All(freshBridge.Bounties, b => Assert.Equal(DebtBountyStatus.Pending, b.status));
            Assert.Equal(expectedBoost, freshBridge.CalculateBountyBoost(), 2);

            // ── 9. Post-Reload One-Shot Guard ────────────────────────────────────────
            // Advance 15 additional daily ticks on the fresh runtime
            for (int d = 1; d <= 15; d++)
            {
                freshDay = 146 + d;
                freshLedger.TickDaily(freshDay);
                freshBridge.TickDaily(freshDay);
            }

            // Invariant: Standing remains EXACTLY constant (no repeated penalties)
            Assert.Equal(10, freshFactionWar.GetStanding(FactionRailwayGuild));
            Assert.Equal(12, freshFactionWar.GetStanding(FactionSupplyCorps));

            // Invariant: Bounties remain exactly 2 (no duplicate bounty creation across 15 daily ticks)
            Assert.Equal(2, freshBridge.Bounties.Count);

            // Invariant: Repayment bonuses list preserved without duplicate grant
            Assert.Single(freshBridge.RepaymentBonusGranted);

            // ── 10. Defaulted Debt Settlement & Bounty Cancellation ──────────────────
            freshDay = 175;
            bool paidDefaulted = freshLedger.PayContract(Debtor, freshDay);
            Assert.True(paidDefaulted);

            // Both bounties cancelled upon contract payment
            Assert.All(freshBridge.Bounties, b =>
            {
                Assert.Equal(DebtBountyStatus.Cancelled, b.status);
                Assert.Equal(freshDay, b.cancelledDay);
            });
            Assert.Equal(0f, freshBridge.CalculateBountyBoost());

            // Historical Default Invariant: Default penalty is not reversed,
            // and no timely repayment bonus is granted for defaulted debts.
            Assert.Equal(10, freshFactionWar.GetStanding(FactionRailwayGuild));
            Assert.Equal(0, freshBridge.RepaymentBonusApplications); // Zero new bonuses granted in fresh session
            Assert.Single(freshBridge.RepaymentBonusGranted); // Historical bonus key from Contract 1 preserved
        }
    }
}
