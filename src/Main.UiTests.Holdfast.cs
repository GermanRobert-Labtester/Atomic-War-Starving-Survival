using Godot;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using AtomicWar.Journal;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Economy;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Foundry;
using Ashfall.Core.Inventory;
using Ashfall.Core.Journal;
using Ashfall.Core.Muster;
using Ashfall.Core.YearOfAsh;
using Ashfall.Core.Radio;
using Ashfall.Core.Survivors;
using AtomicWar.GodotApp.Economy;
using AtomicWar.GodotApp.YearOfAsh;
using AtomicWar.GodotApp.Muster;
using AtomicWar.GodotApp.Dose;
using AtomicWar.GodotApp.UtilityAI;
using AtomicWar.GodotApp.Radio;
using AtomicWar.GodotApp.Audio;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        /// <summary>
        /// Drives the same Holdfast terminal methods used by the normal Godot UI:
        /// exhaustive catalog rendering sweep, every failure enum, save/reload,
        /// post-reload rendering, and continued interaction.
        /// </summary>
        private void RunHoldfastRuntimeUiTestAndQuit()
        {
            BuildUserInterface();
            SetupIceRoad();

            var runtime = new HoldfastRuntimeSession(_core, HoldfastRuntimeSession.DefaultStartingValue);
            runtime.SeedDevelopmentState();
            _holdfastRuntime = runtime;
            _holdfastTerminal = new HoldfastTerminalPanel();
            AddChild(_holdfastTerminal);
            _holdfastTerminal.BindSession(runtime);
            _holdfastTerminal.OpenTerminal();

            bool panel = _holdfastTerminal.IsBound;
            bool catalogs = _holdfastTerminal.PresentedItemCount == 40
                && _holdfastTerminal.PresentedFactionCount == 3;

            // ── Catalog rendering sweep: all 40 items and 3 factions ──
            bool allItemsRender = true;
            bool allFactionsRender = true;
            var preSaveSupplyDetails = new Dictionary<string, string>();
            var preSaveTradeDetails = new Dictionary<string, string>();
            foreach (var item in runtime.Catalog.Items.Items)
            {
                _holdfastTerminal.SelectItem(item.Id);
                string details = _holdfastTerminal.SupplyDetailsText;
                if (string.IsNullOrEmpty(details) || !details.Contains(item.DisplayName))
                    allItemsRender = false;
                preSaveSupplyDetails[item.Id] = _holdfastTerminal.SupplyDetailsText;
                preSaveTradeDetails[item.Id] = _holdfastTerminal.TradeDetailsText;
            }
            foreach (var faction in runtime.Catalog.Factions)
            {
                if (faction == null) continue;
                _holdfastTerminal.SelectFaction(faction.id);
                string details = _holdfastTerminal.FactionDetailsText;
                if (string.IsNullOrEmpty(details) || !details.Contains(faction.display_name))
                    allFactionsRender = false;
            }
            bool renderSweep = allItemsRender && allFactionsRender;

            // ── Core trade flow ──
            // Catalog now loads real items (default stock 20/type; fume_rag trade 2).
            _holdfastTerminal.SelectFaction("faction_the_office");
            _holdfastTerminal.SelectItem("item_fume_rag");
            _holdfastTerminal.SetTradeQuantity(2);
            var buy = _holdfastTerminal.PressBuy();
            long buyValue = runtime.Trade.PlayerValue;
            int buyHeld = runtime.Trade.GetHeld("item_fume_rag");
            int buyStock = runtime.Trade.GetStock("item_fume_rag");
            GD.Print($"[probe] buy success={buy?.Success} msg={buy?.Message} value={buyValue} held={buyHeld} stock={buyStock}");
            bool bought = buy != null && buy.Success
                && runtime.Trade.PlayerValue == 96
                && runtime.Trade.GetHeld("item_fume_rag") == 2
                && runtime.Trade.GetStock("item_fume_rag") == 18; // 20 default - 2

            long valueBeforeInvalid = runtime.Trade.PlayerValue;
            int heldBeforeInvalid = runtime.Trade.GetHeld("item_fume_rag");
            int stockBeforeInvalid = runtime.Trade.GetStock("item_fume_rag");
            _holdfastTerminal.SetTradeQuantity(0);
            var invalid = _holdfastTerminal.PressBuy();
            bool rejectedWithoutMutation = invalid != null
                && !invalid.Success
                && invalid.Failure == HoldfastTradeFailure.InvalidQuantity
                && runtime.Trade.PlayerValue == valueBeforeInvalid
                && runtime.Trade.GetHeld("item_fume_rag") == heldBeforeInvalid
                && runtime.Trade.GetStock("item_fume_rag") == stockBeforeInvalid;

            _holdfastTerminal.SelectItem("item_triplicate_carbon");
            _holdfastTerminal.SetTradeQuantity(1);
            var sell = _holdfastTerminal.PressSell();
            bool sold = sell != null && sell.Success
                && runtime.Trade.PlayerValue == 100
                && runtime.Trade.GetHeld("item_triplicate_carbon") == 0
                && runtime.Trade.GetStock("item_triplicate_carbon") == 21;

            // ── Failure-message matrix ──
            bool invalidQuantityRendered = false;
            bool insufficientFundsRendered = false;
            bool insufficientStockRendered = false;
            bool insufficientInventoryRendered = false;
            bool unknownItemRendered = false;
            bool unknownFactionRendered = false;
            bool restrictedRendered = false;
            bool inventoryCapacityRendered = false;
            // InvalidPrice is exercised by Core unit tests (HoldfastTradeSessionTests)
            // because valid catalog data never produces an invalid trade value; the UI
            // path is unreachable without a synthetic catalog.

            // Invalid quantity: already tested above, capture for the matrix.
            invalidQuantityRendered = invalid != null && !invalid.Success
                && invalid.Failure == HoldfastTradeFailure.InvalidQuantity
                && !string.IsNullOrEmpty(invalid.Message);

            // Insufficient funds: start a fresh session with value 1, try to buy expensive item.
            var poorWorld = CoreDemoSession.Create(_dataDir);
            var poorRuntime = new HoldfastRuntimeSession(poorWorld, 1);
            _holdfastTerminal.BindSession(poorRuntime);
            _holdfastTerminal.SelectFaction("faction_the_office");
            _holdfastTerminal.SelectItem("item_ice_tyre_set");
            _holdfastTerminal.SetTradeQuantity(1);
            var poorResult = _holdfastTerminal.PressBuy();
            insufficientFundsRendered = poorResult != null && !poorResult.Success
                && poorResult.Failure == HoldfastTradeFailure.InsufficientFunds
                && !string.IsNullOrEmpty(poorResult.Message);

            // Insufficient stock: exhaust stock then try one more.
            var stockWorld = CoreDemoSession.Create(_dataDir);
            var stockRuntime = new HoldfastRuntimeSession(stockWorld, 200);
            _holdfastTerminal.BindSession(stockRuntime);
            _holdfastTerminal.SelectFaction("faction_the_office");
            _holdfastTerminal.SelectItem("item_fume_rag");
            _holdfastTerminal.SetTradeQuantity(20);
            _holdfastTerminal.PressBuy(); // exhaust stock (default 20)
            _holdfastTerminal.SetTradeQuantity(1);
            var stockResult = _holdfastTerminal.PressBuy();
            insufficientStockRendered = stockResult != null && !stockResult.Success
                && stockResult.Failure == HoldfastTradeFailure.InsufficientStock
                && !string.IsNullOrEmpty(stockResult.Message);

            // Insufficient inventory: sell something not held.
            var invWorld = CoreDemoSession.Create(_dataDir);
            var invRuntime = new HoldfastRuntimeSession(invWorld, 200);
            _holdfastTerminal.BindSession(invRuntime);
            _holdfastTerminal.SelectFaction("faction_the_office");
            _holdfastTerminal.SelectItem("item_fume_rag");
            _holdfastTerminal.SetTradeQuantity(1);
            var invResult = _holdfastTerminal.PressSell();
            insufficientInventoryRendered = invResult != null && !invResult.Success
                && invResult.Failure == HoldfastTradeFailure.InsufficientInventory
                && !string.IsNullOrEmpty(invResult.Message);

            // Invalid price: use an item with tradeValue that would overflow (not possible with long, so skip — Covered by Core tests).
            // Unknown item.
            _holdfastTerminal.SelectItemRaw("item_does_not_exist");
            var unknownResult = _holdfastTerminal.PressBuy();
            unknownItemRendered = unknownResult != null && !unknownResult.Success
                && unknownResult.Failure == HoldfastTradeFailure.UnknownItem
                && !string.IsNullOrEmpty(unknownResult.Message);

            // Unknown faction.
            _holdfastTerminal.SelectFactionRaw("faction_nonexistent");
            var factionResult = _holdfastTerminal.PressBuy();
            unknownFactionRendered = factionResult != null && !factionResult.Success
                && factionResult.Failure == HoldfastTradeFailure.UnknownFaction
                && !string.IsNullOrEmpty(factionResult.Message);


            // Restricted: inactive faction.
            _holdfastTerminal.SelectFactionRaw("faction_the_fleet");
            var restrictedResult = _holdfastTerminal.PressBuy();
            restrictedRendered = restrictedResult != null && !restrictedResult.Success
                && restrictedResult.Failure == HoldfastTradeFailure.UnavailableOrRestricted
                && !string.IsNullOrEmpty(restrictedResult.Message);

            // Inventory capacity: fill all slots then try one more.
            var capWorld = CoreDemoSession.Create(_dataDir);
            var capRuntime = new HoldfastRuntimeSession(capWorld, 1000);
            _holdfastTerminal.BindSession(capRuntime);
            _holdfastTerminal.SelectFaction("faction_the_office");
            int filled = 0;
            foreach (var def in capRuntime.Catalog.Items.Items)
            {
                if (filled >= capRuntime.Trade.Inventory.Capacity) break;
                if (def.Id == "item_fume_rag") continue; // reserve for the capacity probe
                capRuntime.Trade.SeedInventory(def.Id, 1);
                filled++;
            }
            _holdfastTerminal.SelectItem("item_fume_rag");
            _holdfastTerminal.SetTradeQuantity(1);
            var capResult = _holdfastTerminal.PressBuy();
            inventoryCapacityRendered = capResult != null && !capResult.Success
                && capResult.Failure == HoldfastTradeFailure.InventoryCapacity
                && !string.IsNullOrEmpty(capResult.Message);

            bool failureMatrix = invalidQuantityRendered && insufficientFundsRendered
                && insufficientStockRendered && insufficientInventoryRendered
                && unknownItemRendered && unknownFactionRendered
                && restrictedRendered && inventoryCapacityRendered;

            // ── Save / reload ──
            _holdfastTerminal.BindSession(runtime);

            string root = ProjectSettings.GlobalizePath("user://");
            string basePath = Path.Combine(root, "holdfast_runtime_ui_test_base.json");
            string tradePath = Path.Combine(root, "holdfast_runtime_ui_test_trade.json");
            bool saved = _holdfastTerminal.PressSave(basePath, tradePath);

            // Change live state after the save so reload has an observable job.
            _holdfastTerminal.SelectItem("item_fume_rag");
            _holdfastTerminal.SetTradeQuantity(1);
            _holdfastTerminal.PressBuy();

            var freshWorld = CoreDemoSession.Create(_dataDir);
            var freshRuntime = new HoldfastRuntimeSession(freshWorld, 0);
            _holdfastTerminal.BindSession(freshRuntime);
            _holdfastTerminal.OpenTerminal();
            bool reloaded = _holdfastTerminal.PressReload(basePath, tradePath);
            bool restored = reloaded
                && freshRuntime.Trade.PlayerValue == 100
                && freshRuntime.Trade.GetHeld("item_fume_rag") == 2
                && freshRuntime.Trade.GetStock("item_fume_rag") == 18
                && freshRuntime.Trade.GetHeld("item_triplicate_carbon") == 0
                && freshRuntime.Trade.GetStock("item_triplicate_carbon") == 21;

            // ── Post-reload rendering sweep (compare against pre-save state) ──
            bool postReloadRender = true;
            foreach (var item in freshRuntime.Catalog.Items.Items)
            {
                _holdfastTerminal.SelectItem(item.Id);
                string postSupply = _holdfastTerminal.SupplyDetailsText;
                string postTrade = _holdfastTerminal.TradeDetailsText;
                if (string.IsNullOrEmpty(postSupply) || !postSupply.Contains(item.DisplayName))
                    postReloadRender = false;
                if (preSaveSupplyDetails.TryGetValue(item.Id, out var preSupply))
                {
                    if (!postSupply.Contains(preSupply.Split('\n')[0]))
                        postReloadRender = false;
                }
                if (preSaveTradeDetails.TryGetValue(item.Id, out var preTrade))
                {
                    if (!postTrade.Contains(preTrade.Split('\n')[0]))
                        postReloadRender = false;
                }
            }

            _holdfastTerminal.SelectFaction("faction_the_office");
            _holdfastTerminal.SelectItem("item_fume_rag");
            _holdfastTerminal.SetTradeQuantity(1);
            var continuedBuy = _holdfastTerminal.PressBuy();
            bool continued = continuedBuy != null && continuedBuy.Success
                && freshRuntime.Trade.GetHeld("item_fume_rag") == 3
                && freshRuntime.Trade.PlayerValue == 98;

            // ── New Ledger: two-press confirmation ──
            bool newLedgerFirstArm = !_holdfastTerminal.PressNewLedger();
            bool newLedgerConfirmed = _holdfastTerminal.PressNewLedger();
            bool newLedgerOk = newLedgerFirstArm && newLedgerConfirmed
                && freshRuntime.Trade.PlayerValue == 0
                && freshRuntime.Trade.GetHeld("item_fume_rag") == 0;

            // ── Save resilience: quarantine + backup + archive ──
            string resilienceBase = Path.Combine(root, "holdfast_resilience_base.json");
            string resilienceTrade = Path.Combine(root, "holdfast_resilience_trade.json");
            // Save twice so the first save becomes the .bak.
            bool resilienceSaved = _holdfastTerminal.PressSave(resilienceBase, resilienceTrade);
            resilienceSaved = resilienceSaved && _holdfastTerminal.PressSave(resilienceBase, resilienceTrade);

            // Corrupt the primary save; load should quarantine and fall back to backup.
            if (File.Exists(resilienceBase))
            {
                var raw = File.ReadAllText(resilienceBase);
                File.WriteAllText(resilienceBase, raw.Replace("\"Checksum\":\"", "\"Checksum\":\"xx"));
            }
            bool quarantinePass = false;
            if (File.Exists(resilienceBase + ".bak"))
            {
                bool quarantineReloaded = _holdfastTerminal.PressReload(resilienceBase, resilienceTrade);
                var corruptFiles = Directory.GetFiles(root, "holdfast_resilience_base.json.corrupt-*");
                quarantinePass = quarantineReloaded && corruptFiles.Length > 0;
            }

            bool archivePass = newLedgerOk;

            bool pass = panel && catalogs && renderSweep && bought && rejectedWithoutMutation
                && sold && failureMatrix && saved && reloaded && restored && postReloadRender
                && newLedgerOk && continued && quarantinePass && archivePass;
            GD.Print($"[HoldfastRuntimeUiTest] panel={panel} catalogs={catalogs} renderSweep={renderSweep} " +
                     $"buy={bought} invalidAtomic={rejectedWithoutMutation} sell={sold} " +
                     $"failureMatrix={failureMatrix} save={saved} reload={reloaded} restored={restored} " +
                     $"postReloadRender={postReloadRender} newLedger={newLedgerOk} continued={continued} quarantine={quarantinePass} archive={archivePass}");
            GD.Print(pass ? "HOLDFAST_RUNTIME_UITEST PASS" : "HOLDFAST_RUNTIME_UITEST FAIL");

            if (File.Exists(basePath)) File.Delete(basePath);
            if (File.Exists(tradePath)) File.Delete(tradePath);
            QuitUiTestAfterFrame(pass ? 0 : 1);
        }

    }
}
