using System.Collections.Generic;
using System.Linq;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Muster;
using Ashfall.Core.YearOfAsh;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Flagship Plan IV — debt consequence integration: dispatcher fired-state
    /// persistence (no side effect repeats after restore), typed consequence
    /// events carrying authored payloads, catalog-driven dispatch (standing vs
    /// collateral), ledger-native forgiveness, the embargo authority, and the
    /// host bridge routing into FactionWarSystem / IronRaidersSystem /
    /// inventory delegates / bounded labor obligations.
    /// </summary>
    public sealed class DebtConsequenceIntegrationTests
    {
        private const string Debtor = "npc_wyn_sabler";

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

        private static DebtTemplateCatalog MercyFixtureCatalog()
        {
            // Deterministic forced path for the rare consequence: the real
            // catalog plus one fixture template that routes to it.
            var catalog = LoadCatalog();
            catalog.Templates.Add(new DebtTemplate
            {
                id = "debt_fixture_mercy",
                creditorId = "faction_supply_corps",
                principalItemId = "canned_food",
                principalQuantity = 4,
                termDays = 10,
                rate = 0.1f,
                forfeitDescription = "four tins held in mercy",
                consequenceId = "conseq_forgiveness_rare",
                displayName = "Fixture Mercy Credit",
                description = "fixture"
            });
            catalog.Templates.Add(new DebtTemplate
            {
                id = "debt_fixture_labor",
                creditorId = "faction_supply_corps",
                principalItemId = "canned_food",
                principalQuantity = 2,
                termDays = 10,
                rate = 0.1f,
                forfeitDescription = "two days at the Lockup",
                consequenceId = "conseq_labor_obligation",
                displayName = "Fixture Labor Credit",
                description = "fixture"
            });
            return catalog;
        }

        private static void ReadTwiceSign(LedgerDebtSystem ledger, string debtor, DebtTemplate template, int day)
        {
            Assert.True(ledger.PresentContract(debtor, template.principalQuantity, template.termDays,
                template.rate, template.forfeitDescription, template.creditorId, template.id));
            Assert.True(ledger.PresentContract(debtor, template.principalQuantity, template.termDays,
                template.rate, template.forfeitDescription, template.creditorId, template.id));
            Assert.True(ledger.SignContract(debtor, day));
        }

        // ── Dispatcher: persistence & idempotency ─────────────────────

        [Fact]
        public void FiredStateRoundtripPreventsRedispatch()
        {
            var catalog = LoadCatalog();
            var ledger = new LedgerDebtSystem();
            var dispatcher = new DebtConsequenceDispatcher(ledger, catalog);
            int dispatched = 0;
            dispatcher.OnConsequenceDispatched += (_, _) => dispatched++;

            var template = catalog.GetTemplate("debt_supply_corps_rations")!;
            ReadTwiceSign(ledger, Debtor, template, 40);
            for (int d = 0; d < template.termDays; d++)
                ledger.TickDaily(41 + d);
            Assert.Equal(1, dispatched);
            Assert.True(ledger.GetContract(Debtor)!.forfeited);

            // Save everything, rebuild, restore, keep ticking: nothing re-fires.
            var json = new SystemTextJsonSerializer();
            var ledgerBlob = json.Serialize(ledger.CaptureState());
            var dispatcherBlob = json.Serialize(dispatcher.CaptureState());

            var restoredLedger = new LedgerDebtSystem();
            restoredLedger.RestoreState(json.Deserialize<LedgerDebtSystemState>(ledgerBlob)!);
            var restoredDispatcher = new DebtConsequenceDispatcher(restoredLedger, catalog);
            restoredDispatcher.RestoreState(json.Deserialize<DebtDispatcherState>(dispatcherBlob)!);
            int restoredDispatched = 0;
            restoredDispatcher.OnConsequenceDispatched += (_, _) => restoredDispatched++;

            for (int d = 0; d < 30; d++)
                restoredLedger.TickDaily(70 + d);
            Assert.Equal(0, restoredDispatched);
        }

        [Fact]
        public void ConsequenceIdentity_IsStable_AndSplitsContractInstances()
        {
            var a = new DebtContract { debtorId = Debtor, signedDay = 40 };
            var b = new DebtContract { debtorId = Debtor, signedDay = 41 };
            Assert.Equal(
                DebtConsequenceDispatcher.ConsequenceIdentity(a, "conseq_x"),
                DebtConsequenceDispatcher.ConsequenceIdentity(a, "conseq_x"));
            Assert.NotEqual(
                DebtConsequenceDispatcher.ConsequenceIdentity(a, "conseq_x"),
                DebtConsequenceDispatcher.ConsequenceIdentity(b, "conseq_x"));
            Assert.NotEqual(
                DebtConsequenceDispatcher.ConsequenceIdentity(a, "conseq_x"),
                DebtConsequenceDispatcher.ConsequenceIdentity(a, "conseq_y"));
        }

        [Fact]
        public void StandingEvent_CarriesAuthoredDelta_AndCreditorFallback()
        {
            var catalog = LoadCatalog();
            var ledger = new LedgerDebtSystem();
            var dispatcher = new DebtConsequenceDispatcher(ledger, catalog);
            DebtConsequence? seen = null;
            string? faction = null;
            dispatcher.OnStandingPenalty += (c, f, _) => { seen = c; faction = f; };

            var template = catalog.GetTemplate("debt_supply_corps_rations")!;
            ReadTwiceSign(ledger, Debtor, template, 40);
            for (int d = 0; d < template.termDays; d++)
                ledger.TickDaily(41 + d);

            Assert.NotNull(seen);
            Assert.Equal("conseq_standing_loss_mild", seen!.id);
            Assert.Equal(-5, seen.standingDelta); // authored, not re-derived
            Assert.Equal("faction_supply_corps", faction); // empty target falls back to creditor
        }

        [Fact]
        public void EscalationChain_DispatchesBountyAfterModerateStanding()
        {
            var catalog = LoadCatalog();
            var ledger = new LedgerDebtSystem();
            var dispatcher = new DebtConsequenceDispatcher(ledger, catalog);
            int standingEvents = 0;
            int bountyEvents = 0;
            dispatcher.OnStandingPenalty += (_, _, _) => standingEvents++;
            dispatcher.OnBountyRequested += (_, _) => bountyEvents++;

            // Ordnance tools: standing_loss_moderate (-12) escalating to bounty_moderate.
            var template = catalog.GetTemplate("debt_ordnance_foundry_tools")!;
            ReadTwiceSign(ledger, Debtor, template, 40);
            for (int d = 0; d < template.termDays; d++)
                ledger.TickDaily(41 + d);

            Assert.Equal(1, standingEvents);
            // The chain is two stages: bounty_moderate, then its escalation
            // raid_severe — both fire the bounty request (and nothing re-fires).
            Assert.Equal(2, bountyEvents);
        }

        [Fact]
        public void CollateralSeizure_FallsBackToTemplatePrincipal()
        {
            var catalog = LoadCatalog();
            var ledger = new LedgerDebtSystem();
            var dispatcher = new DebtConsequenceDispatcher(ledger, catalog);
            string? item = null;
            int qty = 0;
            dispatcher.OnCollateralSeizure += (itemId, quantity, _) => { item = itemId; qty = quantity; };

            // conseq_collateral_seizure authors no collateralItemId — the
            // pledged principal (15 × dried_rations) is what gets taken.
            var template = catalog.GetTemplate("debt_scavengers_food")!;
            ReadTwiceSign(ledger, Debtor, template, 40);
            for (int d = 0; d < template.termDays; d++)
                ledger.TickDaily(41 + d);

            Assert.Equal("dried_rations", item);
            Assert.Equal(15, qty);
        }

        [Fact]
        public void DispatchIsCatalogDriven_NotHardCodedToStanding()
        {
            var catalog = LoadCatalog();
            var ledger = new LedgerDebtSystem();
            var dispatcher = new DebtConsequenceDispatcher(ledger, catalog);
            int standingEvents = 0;
            int seizureEvents = 0;
            dispatcher.OnStandingPenalty += (_, _, _) => standingEvents++;
            dispatcher.OnCollateralSeizure += (_, _, _) => seizureEvents++;

            var template = catalog.GetTemplate("debt_scavengers_food")!;
            ReadTwiceSign(ledger, Debtor, template, 40);
            for (int d = 0; d < template.termDays; d++)
                ledger.TickDaily(41 + d);

            Assert.Equal(0, standingEvents); // bounty_and_seizure: no standing leg
            Assert.Equal(1, seizureEvents);
        }

        // ── Ledger-native forgiveness ─────────────────────────────────

        [Fact]
        public void ForgiveContract_ClearsBalance_WithoutPayment()
        {
            var ledger = new LedgerDebtSystem();
            Assert.True(ledger.PresentContract(Debtor, 8f, 20, 0.15f, "eight tins", "faction_supply_corps", "debt_supply_corps_rations"));
            Assert.True(ledger.PresentContract(Debtor, 8f, 20, 0.15f, "eight tins", "faction_supply_corps", "debt_supply_corps_rations"));
            Assert.True(ledger.SignContract(Debtor, 40));
            Assert.True(System.Math.Abs(ledger.TotalOwed(Debtor) - 9.2f) < 0.001f);

            int forgivenEvents = 0;
            ledger.OnContractForgiven += _ => forgivenEvents++;
            Assert.True(ledger.ForgiveContract(Debtor, 55));

            var contract = ledger.GetContract(Debtor)!;
            Assert.True(contract.forgiven);
            Assert.False(contract.paid);
            Assert.Equal(55, contract.forgivenDay);
            Assert.Equal(0f, ledger.TotalOwed(Debtor));
            Assert.Equal(1, forgivenEvents);
            Assert.False(ledger.ForgiveContract(Debtor, 56)); // mercy does not happen twice
            Assert.False(ledger.PayContract(Debtor, 56)); // cannot pay a forgiven debt
        }

        [Fact]
        public void ForgiveContract_RefusesUnsignedDraft()
        {
            var ledger = new LedgerDebtSystem();
            Assert.True(ledger.PresentContract(Debtor, 8f, 20, 0.15f, "eight tins"));
            Assert.False(ledger.ForgiveContract(Debtor, 40));
        }

        [Fact]
        public void ForgivenState_PersistsRoundtrip_AndAllowsNewDraft()
        {
            var ledger = new LedgerDebtSystem();
            Assert.True(ledger.PresentContract(Debtor, 8f, 20, 0.15f, "eight tins", "faction_supply_corps", "t"));
            Assert.True(ledger.PresentContract(Debtor, 8f, 20, 0.15f, "eight tins", "faction_supply_corps", "t"));
            Assert.True(ledger.SignContract(Debtor, 40));
            Assert.True(ledger.ForgiveContract(Debtor, 50));

            var json = new SystemTextJsonSerializer();
            var restored = new LedgerDebtSystem();
            restored.RestoreState(json.Deserialize<LedgerDebtSystemState>(json.Serialize(ledger.CaptureState()))!);
            Assert.True(restored.GetContract(Debtor)!.forgiven);
            Assert.Equal(50, restored.GetContract(Debtor)!.forgivenDay);
            Assert.Equal(0f, restored.TotalOwed(Debtor));

            // New ink after mercy: the forgiven contract archives and a fresh
            // draft starts its own two readings.
            Assert.True(restored.PresentContract(Debtor, 2f, 10, 0.1f, "two tins"));
            var newDraft = restored.GetContract(Debtor)!;
            Assert.False(newDraft.signed);
            Assert.Equal(1, newDraft.readCount); // first reading of a fresh draft
            Assert.False(newDraft.forgiven);
            Assert.Contains(restored.ClosedContracts, c => c != null && c.forgiven); // archived, record kept
        }

        [Fact]
        public void ForgivenessConsequence_ChangesLedgerState_NotEventOnly()
        {
            var catalog = MercyFixtureCatalog();
            var ledger = new LedgerDebtSystem();
            var dispatcher = new DebtConsequenceDispatcher(ledger, catalog);
            dispatcher.SetDayProvider(() => 60);

            var template = catalog.GetTemplate("debt_fixture_mercy")!;
            ReadTwiceSign(ledger, Debtor, template, 40);
            for (int d = 0; d < template.termDays; d++)
                ledger.TickDaily(41 + d);

            var contract = ledger.GetContract(Debtor)!;
            Assert.True(contract.forgiven); // canonical state transition, not a presentation event
            Assert.Equal(60, contract.forgivenDay); // day provider used, not signed day
            Assert.Equal(0f, ledger.TotalOwed(Debtor));
        }

        // ── Embargo authority ─────────────────────────────────────────

        [Fact]
        public void Embargo_WindowBoundaries_AreDayDerived()
        {
            var embargoes = new FactionEmbargoLedger();
            Assert.True(embargoes.TryAddEmbargo("faction_supply_corps", "creditor_faction", 40, 10, "src1"));
            Assert.True(embargoes.IsEmbargoed("faction_supply_corps", 40)); // first day closed
            Assert.True(embargoes.IsEmbargoed("faction_supply_corps", 49)); // last day closed
            Assert.False(embargoes.IsEmbargoed("faction_supply_corps", 50)); // end day open again
            Assert.False(embargoes.IsEmbargoed("faction_hydro_barons", 45)); // other factions unaffected
        }

        [Fact]
        public void Embargo_SameSourceIsIdempotent()
        {
            var embargoes = new FactionEmbargoLedger();
            Assert.True(embargoes.TryAddEmbargo("f", "creditor_faction", 1, 10, "src"));
            Assert.False(embargoes.TryAddEmbargo("f", "creditor_faction", 1, 10, "src")); // no second record
            Assert.Single(embargoes.Embargoes);
            Assert.True(embargoes.TryAddEmbargo("f", "creditor_faction", 1, 10, "src2")); // distinct source coexists
        }

        [Fact]
        public void Embargo_RestoreRoundtrip()
        {
            var embargoes = new FactionEmbargoLedger();
            embargoes.TryAddEmbargo("faction_supply_corps", "creditor_faction", 40, 10, "src");
            var json = new SystemTextJsonSerializer();
            var restored = new FactionEmbargoLedger();
            restored.RestoreState(json.Deserialize<FactionEmbargoLedgerState>(json.Serialize(embargoes.CaptureState()))!);
            Assert.True(restored.IsEmbargoed("faction_supply_corps", 42));
            Assert.False(restored.IsEmbargoed("faction_supply_corps", 52));
        }

        // ── Host bridge: canonical routing ────────────────────────────

        private sealed class FakeShelterInventory
        {
            public readonly Dictionary<string, int> Items = new();
            public int AddCalls;
            public int RemoveCalls;

            public bool Grant(string id, int qty)
            {
                AddCalls++;
                Items.TryGetValue(id, out int cur);
                Items[id] = cur + qty;
                return true;
            }

            public void Revoke(string id, int qty)
            {
                RemoveCalls++;
                Items.TryGetValue(id, out int cur);
                Items[id] = System.Math.Max(0, cur - qty);
            }

            public int Count(string id) => Items.TryGetValue(id, out int c) ? c : 0;

            public bool TryRemove(string id, int qty)
            {
                if (Count(id) < qty) return false;
                Items[id] = Count(id) - qty;
                return true;
            }
        }

        private static (DebtConsequenceHostBridge Bridge, FactionWarSystem War, FactionEmbargoLedger Embargoes, IronRaidersSystem Raiders, FakeShelterInventory Inv, LedgerDebtSystem Ledger, DebtConsequenceDispatcher Dispatcher) BridgeFixture(DebtTemplateCatalog catalog, int day = 100)
        {
            var ledger = new LedgerDebtSystem();
            var dispatcher = new DebtConsequenceDispatcher(ledger, catalog);
            dispatcher.SetDayProvider(() => day);
            var war = new FactionWarSystem();
            var embargoes = new FactionEmbargoLedger();
            var raiders = new IronRaidersSystem();
            raiders.Activate();
            var inv = new FakeShelterInventory();
            var bridge = new DebtConsequenceHostBridge(
                dispatcher, war, embargoes, () => day, NullLog.Instance,
                ironRaiders: raiders,
                tryRemoveItems: inv.TryRemove,
                countItem: inv.Count,
                selectLaborSurvivor: () => "npc_ivo_fenn");
            return (bridge, war, embargoes, raiders, inv, ledger, dispatcher);
        }

        [Fact]
        public void Bridge_StandingAppliedExactlyOnce_WithClamping()
        {
            var catalog = LoadCatalog();
            var (bridge, war, _, _, _, ledger, _) = BridgeFixture(catalog);
            var template = catalog.GetTemplate("debt_supply_corps_rations")!;
            int before = war.GetStanding("faction_supply_corps");
            ReadTwiceSign(ledger, Debtor, template, 40);
            for (int d = 0; d < template.termDays; d++)
                ledger.TickDaily(41 + d);
            for (int d = 0; d < 30; d++)
                ledger.TickDaily(70 + d); // keep ticking: exactly one application
            Assert.Equal(before - 5, war.GetStanding("faction_supply_corps"));
        }

        [Fact]
        public void Bridge_StandingClampsAtLowerBound()
        {
            var catalog = LoadCatalog();
            var (bridge, war, _, _, _, ledger, _) = BridgeFixture(catalog);
            var template = catalog.GetTemplate("debt_supply_corps_rations")!;
            war.ModifyStanding("faction_supply_corps", -98); // near the floor
            ReadTwiceSign(ledger, Debtor, template, 40);
            for (int d = 0; d < template.termDays; d++)
                ledger.TickDaily(41 + d);
            Assert.Equal(-100, war.GetStanding("faction_supply_corps")); // clamped, never below
        }

        [Fact]
        public void Bridge_EmbargoBlocksCreditor_OtherFactionsUnaffected()
        {
            var (_, war, embargoes, _, _, ledger, _) = BridgeFixture(LoadCatalog());
            var catalog = LoadCatalog();
            var template = catalog.GetTemplate("debt_hydro_barons_water")!; // embargo consequence
            ReadTwiceSign(ledger, Debtor, template, 40);
            for (int d = 0; d < template.termDays; d++)
                ledger.TickDaily(41 + d);

            Assert.True(embargoes.IsEmbargoed("faction_hydro_barons", 100));
            Assert.False(embargoes.IsEmbargoed("faction_supply_corps", 100));
            // 14 authored days: closed through 113, open on 114.
            Assert.True(embargoes.IsEmbargoed("faction_hydro_barons", 113));
            Assert.False(embargoes.IsEmbargoed("faction_hydro_barons", 114));
        }

        [Fact]
        public void Bridge_EmbargoDoesNotDuplicateAfterRestore()
        {
            var catalog = LoadCatalog();
            var json = new SystemTextJsonSerializer();
            var (bridge, _, embargoes, _, _, ledger, dispatcher) = BridgeFixture(catalog);
            var template = catalog.GetTemplate("debt_hydro_barons_water")!;
            ReadTwiceSign(ledger, Debtor, template, 40);
            for (int d = 0; d < template.termDays; d++)
                ledger.TickDaily(41 + d);
            Assert.Single(embargoes.Embargoes);

            // Full save/restore cycle, then the same default re-evaluated.
            var ledgerBlob = json.Serialize(ledger.CaptureState());
            var dispatcherBlob = json.Serialize(dispatcher.CaptureState());
            var embargoBlob = json.Serialize(embargoes.CaptureState());

            var ledger2 = new LedgerDebtSystem();
            ledger2.RestoreState(json.Deserialize<LedgerDebtSystemState>(ledgerBlob)!);
            var dispatcher2 = new DebtConsequenceDispatcher(ledger2, catalog);
            dispatcher2.RestoreState(json.Deserialize<DebtDispatcherState>(dispatcherBlob)!);
            var embargoes2 = new FactionEmbargoLedger();
            embargoes2.RestoreState(json.Deserialize<FactionEmbargoLedgerState>(embargoBlob)!);
            var bridge2 = new DebtConsequenceHostBridge(dispatcher2, new FactionWarSystem(), embargoes2, () => 100, NullLog.Instance);

            for (int d = 0; d < 10; d++)
                ledger2.TickDaily(70 + d);
            Assert.Single(embargoes2.Embargoes); // no second embargo
        }

        [Fact]
        public void Bridge_BountyRoutedToRaidAuthority_OncePerConsequenceStage()
        {
            var (_, _, _, raiders, _, ledger, _) = BridgeFixture(LoadCatalog());
            var catalog = LoadCatalog();
            var template = catalog.GetTemplate("debt_railway_guild_transport")!; // bounty → escalation raid
            ReadTwiceSign(ledger, Debtor, template, 40);
            for (int d = 0; d < template.termDays; d++)
                ledger.TickDaily(41 + d);
            // Two authored stages (bounty_moderate + escalated raid_severe),
            // each provoking once; further ticks add nothing.
            Assert.Equal(2, raiders.RaidsThisSeason);
            for (int d = 0; d < 30; d++)
                ledger.TickDaily(70 + d);
            Assert.Equal(2, raiders.RaidsThisSeason);
        }

        [Fact]
        public void Bridge_CollateralSeizure_RemovesExactlyOnce()
        {
            var (bridge, _, _, _, inv, ledger, _) = BridgeFixture(LoadCatalog());
            var catalog = LoadCatalog();
            var template = catalog.GetTemplate("debt_scavengers_food")!;
            inv.Grant("dried_rations", 20);
            ReadTwiceSign(ledger, Debtor, template, 40);
            for (int d = 0; d < template.termDays; d++)
                ledger.TickDaily(41 + d);
            Assert.Equal(5, inv.Count("dried_rations")); // 20 - 15 seized
            for (int d = 0; d < 30; d++)
                ledger.TickDaily(70 + d);
            Assert.Equal(5, inv.Count("dried_rations")); // never seized twice
        }

        [Fact]
        public void Bridge_CollateralShortfall_SeizesNothing()
        {
            var (_, _, _, _, inv, ledger, _) = BridgeFixture(LoadCatalog());
            var catalog = LoadCatalog();
            var template = catalog.GetTemplate("debt_scavengers_food")!;
            inv.Grant("dried_rations", 3); // partially present only
            ReadTwiceSign(ledger, Debtor, template, 40);
            for (int d = 0; d < template.termDays; d++)
                ledger.TickDaily(41 + d);
            Assert.Equal(3, inv.Count("dried_rations")); // all-or-nothing: shortfall takes nothing
        }

        [Fact]
        public void Bridge_LaborObligation_IsBounded_AndSurvivesSaveLoad()
        {
            var catalog = MercyFixtureCatalog();
            var json = new SystemTextJsonSerializer();
            var (bridge, _, _, _, _, ledger, _) = BridgeFixture(catalog);
            var template = catalog.GetTemplate("debt_fixture_labor")!;
            ReadTwiceSign(ledger, Debtor, template, 40);
            for (int d = 0; d < template.termDays; d++)
                ledger.TickDaily(41 + d);

            Assert.Single(bridge.LaborObligations);
            var obligation = bridge.LaborObligations[0];
            Assert.Equal("npc_ivo_fenn", obligation.survivorId);
            Assert.Equal(7, obligation.laborDays);
            Assert.Equal(100, obligation.startDay);
            Assert.Equal(107, obligation.endDay); // bounded: explicit end, never permanent
            Assert.True(bridge.IsBoundToLabor("npc_ivo_fenn"));
            bridge.TickDaily(106);
            Assert.True(bridge.IsBoundToLabor("npc_ivo_fenn"));
            bridge.TickDaily(107);
            Assert.False(bridge.IsBoundToLabor("npc_ivo_fenn")); // released at end day

            var restored = new DebtConsequenceHostBridge(
                new DebtConsequenceDispatcher(new LedgerDebtSystem(), catalog), new FactionWarSystem(),
                new FactionEmbargoLedger(), () => 1, NullLog.Instance);
            restored.RestoreState(json.Deserialize<DebtConsequenceBridgeState>(json.Serialize(bridge.CaptureState()))!);
            Assert.Single(restored.LaborObligations); // survives the roundtrip
            Assert.Equal(107, restored.LaborObligations[0].endDay);
        }

        [Fact]
        public void Bridge_LaborObligation_DoesNotDuplicateForSameConsequence()
        {
            var catalog = MercyFixtureCatalog();
            var (bridge, _, _, _, _, ledger, dispatcher) = BridgeFixture(catalog);
            var template = catalog.GetTemplate("debt_fixture_labor")!;
            ReadTwiceSign(ledger, Debtor, template, 40);
            for (int d = 0; d < template.termDays; d++)
                ledger.TickDaily(41 + d);
            Assert.Single(bridge.LaborObligations);

            // Same consequence requested again (defensive path): still one.
            var contract = ledger.GetContract(Debtor)!;
            bridge.GetType().GetMethod("HandleLaborObligation",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                .Invoke(bridge, new object[] { "faction_supply_corps", 7, contract });
            Assert.Single(bridge.LaborObligations);
        }

        [Fact]
        public void Bridge_TeardownAndRebuild_DoesNotLeakSubscriptions()
        {
            var catalog = LoadCatalog();
            var war = new FactionWarSystem();
            var ledger = new LedgerDebtSystem();
            var dispatcher = new DebtConsequenceDispatcher(ledger, catalog);
            dispatcher.SetDayProvider(() => 100);

            var bridge1 = new DebtConsequenceHostBridge(dispatcher, war, new FactionEmbargoLedger(), () => 100, NullLog.Instance);
            bridge1.Detach(); // session teardown
            var bridge2 = new DebtConsequenceHostBridge(dispatcher, war, new FactionEmbargoLedger(), () => 100, NullLog.Instance);

            var template = catalog.GetTemplate("debt_supply_corps_rations")!;
            ReadTwiceSign(ledger, Debtor, template, 40);
            for (int d = 0; d < template.termDays; d++)
                ledger.TickDaily(41 + d);

            // Only the live bridge applied the delta — not one per historical session.
            Assert.Equal(-5, war.GetStanding("faction_supply_corps"));
        }

        [Fact]
        public void Bridge_ForgivenessViaBridge_ChangesLedgerState()
        {
            var catalog = MercyFixtureCatalog();
            var (_, _, _, _, _, ledger, _) = BridgeFixture(catalog);
            var template = catalog.GetTemplate("debt_fixture_mercy")!;
            ReadTwiceSign(ledger, Debtor, template, 40);
            for (int d = 0; d < template.termDays; d++)
                ledger.TickDaily(41 + d);
            var contract = ledger.GetContract(Debtor)!;
            Assert.True(contract.forgiven);
            Assert.Equal(0f, ledger.TotalOwed(Debtor));
        }
    }
}
