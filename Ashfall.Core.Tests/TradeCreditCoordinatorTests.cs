using System.Collections.Generic;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.YearOfAsh;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Flagship Plan IV — trade credit: template matching from insufficient-funds
    /// trade contexts, the standing / existing-debt / embargo / principal-relevance
    /// gates, explicit acceptance (two readings, sign, single disbursement),
    /// compensating rollback, and save/reload double-disbursement protection.
    /// </summary>
    public sealed class TradeCreditCoordinatorTests
    {
        private const string Debtor = "npc_wyn_sabler";
        private const string SupplyCorps = "faction_supply_corps";

        private static string DataDir()
        {
            if (CatalogLocator.TryFindDataDirectory(System.IO.Directory.GetCurrentDirectory(), out string found))
                return found;
            if (CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out found))
                return found;
            throw new System.IO.DirectoryNotFoundException("Assets/StreamingAssets/Data not found");
        }

        private sealed class CreditFixture
        {
            public LedgerDebtSystem Ledger = new();
            public DebtTemplateCatalog Catalog = null!;
            public FactionEmbargoLedger Embargoes = new();
            public FactionWarSystem FactionWar = new();
            public FakeInventory Inventory = new();
            public TradeCreditCoordinator Coordinator = null!;
            public int Day = 100;

            public static CreditFixture Create(int startingStanding = 0)
            {
                var f = new CreditFixture
                {
                    Catalog = DebtTemplateCatalogLoader.Load(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer())
                };
                Assert.True(f.Catalog.Errors.Count == 0, "catalog errors: " + string.Join("; ", f.Catalog.Errors));
                if (startingStanding != 0)
                    f.FactionWar.ModifyStanding(SupplyCorps, startingStanding);
                f.Coordinator = new TradeCreditCoordinator(
                    f.Ledger, f.Catalog, f.Embargoes, () => f.Day,
                    f.Inventory.Grant, Debtor,
                    factionWar: f.FactionWar,
                    revokeItems: f.Inventory.Revoke);
                return f;
            }
        }

        private sealed class FakeInventory
        {
            public readonly Dictionary<string, int> Items = new();
            public int GrantCalls;
            public int RevokeCalls;
            public bool FailGrants;

            public bool Grant(string id, int qty)
            {
                GrantCalls++;
                if (FailGrants) return false;
                Items.TryGetValue(id, out int cur);
                Items[id] = cur + qty;
                return true;
            }

            public void Revoke(string id, int qty)
            {
                RevokeCalls++;
                Items.TryGetValue(id, out int cur);
                Items[id] = System.Math.Max(0, cur - qty);
            }

            public int Count(string id) => Items.TryGetValue(id, out int c) ? c : 0;
        }

        // ── Offer building & template matching ────────────────────────

        [Fact]
        public void RationsOffer_BuiltFromTemplate_OnInsufficientFunds()
        {
            var f = CreditFixture.Create();
            var result = f.Coordinator.TryBuildCreditOffer(SupplyCorps, "canned_food");
            Assert.True(result.Eligible, "reason: " + result.Reason);
            var offer = result.Offer!;
            Assert.Equal("debt_supply_corps_rations", offer.TemplateId);
            Assert.Equal(SupplyCorps, offer.CreditorId);
            Assert.Equal("canned_food", offer.PrincipalItemId);
            Assert.Equal(8, offer.PrincipalQuantity);
            Assert.Equal(20, offer.TermDays);
            Assert.Equal(0.15f, offer.Rate);
            Assert.False(string.IsNullOrEmpty(offer.ForfeitDescription));
            Assert.False(string.IsNullOrEmpty(offer.ConsequenceSummary)); // authored consequence text
        }

        [Fact]
        public void ItemAliases_ResolvePrefixedTradeIds()
        {
            var f = CreditFixture.Create();
            Assert.True(f.Coordinator.TryBuildCreditOffer(SupplyCorps, "item_canned_food").Eligible);
        }

        [Fact]
        public void FuelAndMedical_ReachablePerMatchingTable()
        {
            var f = CreditFixture.Create();
            Assert.True(f.Coordinator.TryBuildCreditOffer(SupplyCorps, "fuel").Eligible);
            Assert.True(f.Coordinator.TryBuildCreditOffer(SupplyCorps, "medical_kit").Eligible);
            var fuel = f.Coordinator.TryBuildCreditOffer("faction_railway_guild", "diesel_fuel");
            Assert.True(fuel.Eligible, "reason: " + fuel.Reason);
            Assert.Equal("debt_railway_guild_fuel", fuel.Offer!.TemplateId);
        }

        [Fact]
        public void UnrelatedPrincipal_GetsNoOffer()
        {
            var f = CreditFixture.Create();
            // A rifle (Foundry ammo) cannot be financed with rations credit.
            var result = f.Coordinator.TryBuildCreditOffer(SupplyCorps, "ammo_762");
            Assert.False(result.Eligible);
            Assert.Equal("credit_no_matching_template", result.Reason);
        }

        // ── Gates ──────────────────────────────────────────────────────

        [Fact]
        public void HostileStanding_BlocksOffer_ExactlyAtThreshold()
        {
            var f = CreditFixture.Create(startingStanding: -50); // boundary: hostile by definition
            var result = f.Coordinator.TryBuildCreditOffer(SupplyCorps, "canned_food");
            Assert.False(result.Eligible);
            Assert.Equal("credit_hostile_standing", result.Reason);

            var g = CreditFixture.Create(startingStanding: -49); // one above the line
            Assert.True(g.Coordinator.TryBuildCreditOffer(SupplyCorps, "canned_food").Eligible);
        }

        [Fact]
        public void ExistingSameCreditorDebt_BlocksOffer()
        {
            var f = CreditFixture.Create();
            Assert.True(f.Coordinator.TryAcceptCredit("debt_supply_corps_rations", SupplyCorps).Success);
            var result = f.Coordinator.TryBuildCreditOffer(SupplyCorps, "canned_food");
            Assert.False(result.Eligible);
            Assert.Equal("credit_existing_debt", result.Reason);
        }

        [Fact]
        public void DifferentCreditorDebt_DoesNotBlockOffer()
        {
            var f = CreditFixture.Create();
            // Hydro Barons water debt does not stop the Supply Corps offering rations credit.
            var hydro = f.Catalog.GetTemplate("debt_hydro_barons_water")!;
            Assert.True(f.Ledger.PresentContract(Debtor, hydro.principalQuantity, hydro.termDays, hydro.rate,
                hydro.forfeitDescription, hydro.creditorId, hydro.id));
            Assert.True(f.Ledger.PresentContract(Debtor, hydro.principalQuantity, hydro.termDays, hydro.rate,
                hydro.forfeitDescription, hydro.creditorId, hydro.id));
            Assert.True(f.Ledger.SignContract(Debtor, f.Day));
            Assert.True(f.Coordinator.TryBuildCreditOffer(SupplyCorps, "canned_food").Eligible);
        }

        [Fact]
        public void PaidDebt_DoesNotBlockFutureCredit()
        {
            var f = CreditFixture.Create();
            Assert.True(f.Coordinator.TryAcceptCredit("debt_supply_corps_rations", SupplyCorps).Success);
            Assert.True(f.Ledger.PayContract(Debtor, f.Day + 1));
            var result = f.Coordinator.TryBuildCreditOffer(SupplyCorps, "canned_food");
            Assert.True(result.Eligible, "reason: " + result.Reason);
        }

        [Fact]
        public void ForgivenDebt_DoesNotBlockFutureCredit()
        {
            var f = CreditFixture.Create();
            Assert.True(f.Coordinator.TryAcceptCredit("debt_supply_corps_rations", SupplyCorps).Success);
            Assert.True(f.Ledger.ForgiveContract(Debtor, f.Day + 1));
            var result = f.Coordinator.TryBuildCreditOffer(SupplyCorps, "canned_food");
            Assert.True(result.Eligible, "reason: " + result.Reason);
        }

        [Fact]
        public void ActiveEmbargo_BlocksOffer()
        {
            var f = CreditFixture.Create();
            f.Embargoes.TryAddEmbargo(SupplyCorps, "creditor_faction", f.Day - 1, 10, "test-source");
            var result = f.Coordinator.TryBuildCreditOffer(SupplyCorps, "canned_food");
            Assert.False(result.Eligible);
            Assert.Equal("credit_embargoed", result.Reason);
        }

        [Fact]
        public void ExpiredEmbargo_DoesNotBlockOffer()
        {
            var f = CreditFixture.Create();
            f.Embargoes.TryAddEmbargo(SupplyCorps, "creditor_faction", f.Day - 20, 10, "test-source"); // ended already
            Assert.True(f.Coordinator.TryBuildCreditOffer(SupplyCorps, "canned_food").Eligible);
        }

        // ── Acceptance transaction ────────────────────────────────────

        [Fact]
        public void Acceptance_TwoReadings_SignsAndDisbursesOnce()
        {
            var f = CreditFixture.Create();
            var result = f.Coordinator.TryAcceptCredit("debt_supply_corps_rations", SupplyCorps);
            Assert.True(result.Success);

            var contract = f.Ledger.GetContract(Debtor)!;
            Assert.True(contract.signed);
            Assert.Equal(2, contract.readCount); // the reading rite ran exactly twice
            Assert.Equal(SupplyCorps, contract.creditorId);
            Assert.Equal("debt_supply_corps_rations", contract.templateId);
            Assert.Equal(8f, contract.principal);
            Assert.Equal(20, contract.termDays);
            Assert.Equal(0.15f, contract.rate);
            Assert.Equal(f.Day, contract.signedDay);
            Assert.Equal(8, f.Inventory.Count("canned_food")); // principal received exactly once
            Assert.Equal(1, f.Inventory.GrantCalls);
        }

        [Fact]
        public void Decline_LeavesNoDebtAndNoPrincipal()
        {
            var f = CreditFixture.Create();
            var offer = f.Coordinator.TryBuildCreditOffer(SupplyCorps, "canned_food");
            Assert.True(offer.Eligible); // shown...
            // ...and declined: nothing moved, no state consumed.
            Assert.Null(f.Ledger.GetContract(Debtor));
            Assert.Equal(0, f.Inventory.Count("canned_food"));
            Assert.Equal(0, f.Inventory.GrantCalls);
        }

        [Fact]
        public void GrantFailure_NoContractSigned()
        {
            var f = CreditFixture.Create();
            f.Inventory.FailGrants = true;
            var result = f.Coordinator.TryAcceptCredit("debt_supply_corps_rations", SupplyCorps);
            Assert.False(result.Success);
            Assert.Equal("credit_principal_transfer_failed", result.Reason);
            var contract = f.Ledger.GetContract(Debtor);
            Assert.True(contract == null || !contract.signed); // no ink without goods
            Assert.Equal(0f, f.Ledger.TotalOwed(Debtor));
        }

        [Fact]
        public void StaleOffer_DiesOnRevalidation()
        {
            var f = CreditFixture.Create();
            var offer = f.Coordinator.TryBuildCreditOffer(SupplyCorps, "canned_food");
            Assert.True(offer.Eligible);
            // The world moved while the offer was on screen: an embargo landed.
            f.Embargoes.TryAddEmbargo(SupplyCorps, "creditor_faction", f.Day, 10, "stale-test");
            var result = f.Coordinator.TryAcceptCredit("debt_supply_corps_rations", SupplyCorps);
            Assert.False(result.Success);
            Assert.Equal("credit_stale_offer", result.Reason);
            Assert.Equal(0, f.Inventory.GrantCalls);
        }

        [Fact]
        public void SaveReload_CannotGrantPrincipalTwice()
        {
            var f = CreditFixture.Create();
            Assert.True(f.Coordinator.TryAcceptCredit("debt_supply_corps_rations", SupplyCorps).Success);
            Assert.Equal(8, f.Inventory.Count("canned_food"));

            // Save the ledger, rebuild the coordinator over the restored state.
            var json = new SystemTextJsonSerializer();
            var ledger2 = new LedgerDebtSystem();
            ledger2.RestoreState(json.Deserialize<LedgerDebtSystemState>(json.Serialize(f.Ledger.CaptureState()))!);
            var coordinator2 = new TradeCreditCoordinator(
                ledger2, f.Catalog, f.Embargoes, () => f.Day, f.Inventory.Grant, Debtor,
                factionWar: f.FactionWar, revokeItems: f.Inventory.Revoke);

            var again = coordinator2.TryAcceptCredit("debt_supply_corps_rations", SupplyCorps);
            Assert.False(again.Success); // same-creditor exposure blocks, not a second grant
            Assert.Equal(8, f.Inventory.Count("canned_food"));
            Assert.Equal(1, f.Inventory.GrantCalls);

            var contract = ledger2.GetContract(Debtor)!;
            Assert.True(contract.signed); // the signed contract itself survived
            Assert.Equal("debt_supply_corps_rations", contract.templateId);
        }

        [Fact]
        public void HasUnpaidDebtFromCreditor_StatusMatrix()
        {
            var f = CreditFixture.Create();
            Assert.False(f.Coordinator.HasUnpaidDebtFromCreditor(SupplyCorps));

            Assert.True(f.Coordinator.TryAcceptCredit("debt_supply_corps_rations", SupplyCorps).Success);
            Assert.True(f.Coordinator.HasUnpaidDebtFromCreditor(SupplyCorps)); // active signed blocks

            Assert.True(f.Ledger.PayContract(Debtor, f.Day));
            Assert.False(f.Coordinator.HasUnpaidDebtFromCreditor(SupplyCorps)); // paid does not
        }
    }

    /// <summary>Holdfast trade + embargo authority integration: a suspended
    /// counterparty refuses both directions of trade and the buy preview.</summary>
    public sealed class HoldfastTradeEmbargoTests
    {
        private static string DataDir()
        {
            if (CatalogLocator.TryFindDataDirectory(System.IO.Directory.GetCurrentDirectory(), out string found))
                return found;
            if (CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out found))
                return found;
            throw new System.IO.DirectoryNotFoundException("Assets/StreamingAssets/Data not found");
        }

        private static HoldfastTradeSession Session(long value = 1000)
        {
            var loader = new HoldfastCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer());
            var session = new HoldfastTradeSession(loader.Load(DataDir()), value);
            session.SeedInventory("item_map_sheet_ice_road", 5);
            return session;
        }

        [Fact]
        public void EmbargoedFaction_BuyRefused()
        {
            var session = Session();
            session.SelectFaction("faction_the_office");
            session.EmbargoQuery = _ => true;
            var result = session.Buy("item_map_sheet_ice_road", 1, "faction_the_office");
            Assert.False(result.Success);
            Assert.Equal(HoldfastTradeFailure.Embargoed, result.Failure);
        }

        [Fact]
        public void EmbargoedFaction_SellRefused()
        {
            var session = Session();
            session.SelectFaction("faction_the_office");
            session.EmbargoQuery = _ => true;
            var result = session.Sell("item_map_sheet_ice_road", 1, "faction_the_office");
            Assert.False(result.Success);
            Assert.Equal(HoldfastTradeFailure.Embargoed, result.Failure);
        }

        [Fact]
        public void EmbargoedFaction_PreviewRefused()
        {
            var session = Session();
            session.EmbargoQuery = factionId => factionId == "faction_the_office";
            var preview = session.PreviewBuy("item_map_sheet_ice_road", 1, "faction_the_office");
            Assert.False(preview.IsAvailable);
            Assert.Equal("embargoed", preview.FailureCode);
        }

        [Fact]
        public void UnembargoedFaction_TradesNormally()
        {
            var session = Session();
            session.EmbargoQuery = factionId => factionId == "faction_the_fleet";
            var result = session.Buy("item_map_sheet_ice_road", 1, "none");
            Assert.True(result.Success, result.Message);
        }
    }
}
