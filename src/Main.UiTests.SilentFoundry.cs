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
        private void RunSilentFoundryUiTestAndQuit()
        {
            // Self-contained run: persisted user:// saves from earlier runs must
            // not leak foundry/economy/journal state into the assertions.
            foreach (var file in new[] { "expansion_hub_save.json", "economy_save.json", "journal_save.json" })
            {
                string p = Path.Combine(ProjectSettings.GlobalizePath("user://"), file);
                if (System.IO.File.Exists(p)) System.IO.File.Delete(p);
            }

            BuildUserInterface();
            SetupExpansions();
            SetupSilentFoundry();

            bool pass = true;
            void Check(bool cond, string name)
            {
                if (cond) GD.Print($"  [PASS] {name}");
                else { GD.PrintErr($"  [FAIL] {name}"); pass = false; }
            }

            Check(_silentFoundry != null, "host session created");
            Check(_silentFoundryPanel != null, "panel constructed");
            Check(_silentFoundry!.Engine.IsUnlocked == false, "foundry sealed by default");
            // Register foundry items into the shared inventory catalog.
            SetupInventory();
            Check(_inventory.Catalog.Get("item_foundry_plowshare") != null, "foundry items registered in inventory catalog");
            Check(_inventory.Catalog.Get(SilentFoundryIds.ItemScrapMetal) != null, "charge materials registered");

            // Self-contained run: a persisted user:// inventory from earlier runs
            // must not crowd the shared container. Clear and reseed deterministically.
            _inventory.Inventory.Clear();
            _inventory.Inventory.MaxWeight = 500f;
            _inventory.Add(SilentFoundryIds.ItemScrapMetal, 12);
            _inventory.Add(SilentFoundryIds.ItemCoal, 12);
            _inventory.Add(SilentFoundryIds.ItemCleanWater, 60);
            _inventory.Add(SilentFoundryIds.ItemFlux, 3);
            _inventory.Add("item_foundry_green_sand", 4);
            _inventory.Add("item_foundry_firebrick", 6);

            // Open the panel and drive a full heat end-to-end through the host session.
            OpenPlayerPanel("silent_foundry");
            Check(_silentFoundryPanel!.Visible && _silentFoundryPanel.IsBound, "panel opens and binds");

            _silentFoundry.Unlock(_simDay);
            Check(_silentFoundry.Engine.IsUnlocked, "unlock via host session");
            string start = _silentFoundry.StartHeat("foundry_prod_plowshare", 4, 0.6f, _simDay + 1);
            Check(start.StartsWith("Heat started"), "heat starts: " + start);
            int d = _simDay + 2;
            for (int guard = 0; guard < 20 && _silentFoundry.Engine.HeatStage != FoundryHeatStage.Complete; guard++, d++)
            {
                _silentFoundry.Engine.TickDaily(d);
                if (_silentFoundry.Engine.HeatStage == FoundryHeatStage.AtHeat)
                    _silentFoundry.Tap(d);
            }
            Check(_silentFoundry.Engine.TotalProductionCount == 1, "heat completes through the host");
            Check(_silentFoundry.Engine.IsJournalTriggered(SilentFoundryIds.JournalFirstHeat), "first-heat journal triggered");
            Check(_journal != null && _journal.Knowledge.Has(SilentFoundryIds.JournalFirstHeat), "journal knowledge key recorded");
            Check(_silentFoundryPanel.Visible, "panel still open after the heat");
            _silentFoundryPanel.RefreshView();

            // Treaty consequence host path: a missed quota must reach the real
            // stance engine and market surface exactly once, through the host session.
            // (Reset the durable ledger + market demand so repeated runs stay deterministic.)
            _silentFoundry.Engine.RestoreConsequenceState(new SilentFoundryConsequenceState());
            _silentFoundry.SyncGuildStanding();
            _economy.Market.AdjustDemand("item_foundry_brine_pipe", -10f); // floor at the market clamp
            float acidDemandBefore = _economy.Market.GetDemandMultiplier("item_foundry_brine_pipe");
            Check(_silentFoundry.GuildTrust == 0f, "standing reset for the run");
            _silentFoundry.Engine.AssessTreatyCompliance(280); // treaty_05 acid-pipe quota short
            Check(_silentFoundry.GuildTrust < 0f, "host stance engine reflects the standing penalty");
            Check(_silentFoundry.GuildStanceEngine.GetTrust(SilentFoundryIds.FactionId) < 0f, "guild trust moved on the existing stance engine");
            Check(_silentFoundry.GuildStanceEngine.GetTrust("current_10_the_foundry_union") == 0f, "no leak to the foundry union");
            Check(_economy.Market.GetDemandMultiplier("item_foundry_brine_pipe") > acidDemandBefore,
                "market demand moved on the real MarketSystem");
            Check(_silentFoundry.Engine.AppliedConsequences.Count == 1, "consequence applied once");
            _silentFoundry.Engine.AssessTreatyCompliance(280); // idempotent re-assessment
            Check(_silentFoundry.Engine.AppliedConsequences.Count == 1, "re-assessment does not stack");

            // Live trade screen: opens bound to the guild stance engine; the stall
            // stays open while trust sits above the rob floor.
            OpenPlayerPanel("trade");
            Check(_tradePanel.Visible && _tradePanel.HasStanceBadge && _tradePanel.HasTrustMeter,
                "trade screen opens in the live loop with stance + trust rendered");
            Check(_silentFoundry.GuildStance == TradeStance.Trade, "stall open above the rob floor");
            // Drive the guild below the rob floor with repeated missed cycles; the
            // stance must flip to a blocked band (Rob) that the screen's confirm
            // gate rejects (willTrade = Trade | ShareIntel).
            for (int i = 0; i < 10; i++)
                _silentFoundry.Engine.AssessTreatyCompliance(280 + (i + 1) * 30); // missed acid-pipe cycles
            Check(_silentFoundry.GuildStance == TradeStance.Rob || _silentFoundry.GuildStance == TradeStance.HostileRaid,
                "stance blocks the stall after repeated missed cycles");
            Check(_silentFoundry.GuildTrust <= -40f, "trust crossed the rob floor");
            _tradePanel.RefreshView();
            CloseTradePanel();
            Check(!_tradePanel.Visible, "trade screen closes cleanly");

            // Live-campaign reachability: the real TickSimDay loop reaches the
            // day-280 treaty assessment (treaty_05 is inside the playable Year of
            // Ash window, days 180-360). Late treaties (950/330/3650) stay out
            // of the live loop by the documented campaign limit.
            _silentFoundry.Engine.RestoreConsequenceState(new SilentFoundryConsequenceState());
            _silentFoundry.SyncGuildStanding();
            Check(_silentFoundry.Engine.GetTreatyOutcome(SilentFoundryIds.TreatyBrinePipe, 279) == FoundryTreatyOutcome.NotRatified,
                "pre-ratification neutral in the live loop");
            SetupCampaignDay();
            _campaignDay.Calendar.SetDay(276);
            TickSimDay(277);
            TickSimDay(278);
            TickSimDay(279);
            TickSimDay(280);
            Check(_silentFoundry.Engine.IsConsequenceApplied(SilentFoundryIds.TreatyBrinePipe, 280),
                "live TickSimDay reaches the day-280 treaty assessment");
            Check(_silentFoundry.GuildTrust == -6f, "live loop applied the single missed-quota consequence");
            Check(_silentFoundry.Engine.AppliedConsequences.Count == 1, "exactly one consequence from the live window");

            // Late-treaty host path: the foundry's live tick line (TickDaily) is
            // day-agnostic, so a late treaty fires through the FULL host pipeline
            // (stance engine + real market) whenever the campaign supplies the day.
            //
            // The live campaign caps at ~360, so this proves the pipeline, not the
            // campaign reachability, for days 950/330/3650.
            float coalDemandBefore = _economy.Market.GetDemandMultiplier("coal");
            _silentFoundry.Engine.TickDaily(330); // treaty_12 assessment day
            Check(_silentFoundry.Engine.IsConsequenceApplied(SilentFoundryIds.TreatyRoadIron, 330),
                "late-treaty consequence reaches the ledger through the host tick");
            Check(_economy.Market.GetDemandMultiplier("coal") > coalDemandBefore,
                "late-treaty logistics modifier moves the real market");
            Check(_silentFoundry.GuildStanceEngine.GetTrust(SilentFoundryIds.FactionId) < 0f,
                "late-treaty standing reaches the stance engine");

            // Journal author role is preserved from the authored template.
            bool authorRolePreserved = false;
            foreach (var e in _journal!.Entries)
                if (e != null && e.KnowledgeKey == SilentFoundryIds.JournalFirstHeat && e.AuthorName == "Foundryman")
                    authorRolePreserved = true;
            Check(authorRolePreserved, "journal entry preserves the authored author role");

            // Factions panel renders the guild card (data-driven from the authored
            // faction registry entry).
            OpenPlayerPanel("factions");
            Check(_factionsPanel.HasGuildCard, "factions panel renders the Silent Foundry works card");
            _factionsPanel.RefreshView();
            CloseFactionsPanel();

            HostCli.EmitSummary("silent_foundry_uitest", pass, pass ? 0 : 1);
            QuitUiTestAfterFrame(pass ? 0 : 1);
        }

    }
}
