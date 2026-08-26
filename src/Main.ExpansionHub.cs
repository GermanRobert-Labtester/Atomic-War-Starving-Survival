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
        // ── Expansion Hub / Foundry fields (GAP-ARCH-01 Phase 2) ──
        private ExpansionHostSession _expansions = null!;
        private AtomicWar.GodotApp.SilentFoundryHostSession _silentFoundry = null!;
        private SilentFoundryPanel _silentFoundryPanel = null!;
        private bool _expansionHubDirty;
        private bool _foundryDirty;

        private void SetupExpansions()
        {
            if (_expansions != null) return;
            _expansions = ExpansionHostSession.Create(_dataDir);
            _expansions.StateChanged += () => _expansionHubDirty = true;
            _expansions.OnCrossingStageNarrative += OnCrossingStageNarrative;

            // Cross-host roundtrip for waystation, standing record, crossing vouch,
            // and greenhouse plots.
            var save = ExpansionHubSaveStore.TryLoad();
            if (save != null)
            {
                _expansions.RestoreSave(save);
                _expansionHubDirty = false; // restore just raised state-change events
                GD.Print($"[Ashfall Godot] Expansion hub state restored (day {save.simDay}).");
            }

            _expansions.EnsureGreenhousePlots(3);
            RefreshExpansionsStatus();
            GD.Print("[Ashfall Godot] Expansion hub ready: waystation · standing record · crossing · greenhouse");
        }

        private void OnCrossingStageNarrative(Ashfall.Core.Crossing.CrossingStageNarrativeEvent evt)
        {
            if (evt == null) return;
            string tag = evt.isCompletion ? "[CHARTER COMPLETE]" : $"[NC STAGE {evt.stageIndex + 1}]";
            string line = $"{tag} {evt.questDisplayName}: {evt.stageText}";
            GD.Print($"[Ashfall Godot] Crossing narrative: {line}");
            if (_hostEventAdapter != null)
            {
                string eventId = $"event_crossing_{evt.questId}_{evt.stageIndex}_{(evt.isCompletion ? "complete" : "stage")}";
                _hostEventAdapter.TriggerEvent(eventId, _simDay);
            }
            if (_journal != null)
            {
                _journal.TryAddRawEntry(
                    $"crossing_{evt.questId}_{evt.stageIndex}_{(evt.isCompletion ? "complete" : "stage")}",
                    line,
                    null!,
                    _simDay);
                _journalDirty = true;
            }
            _statusLabel?.SetDeferred(Label.PropertyName.Text, line);
        }

        private void RefreshExpansionsStatus()
        {
            if (_expansions == null || _statusLabel == null) return;
            _statusLabel.Text =
                $"——— EXPANSION HUB (Standing Record · Crossing · Greenhouse) ———\n" +
                _expansions.StandingRecordLine() + "\n" +
                _expansions.CrossingLine() + "\n" +
                _expansions.GreenhouseLine() + "\n" +
                _expansions.WaystationLine() + "\n" +
                _expansions.ArbitrationLine() + "\n" +
                _expansions.LedgerLine() + "\n" +
                DiseaseStatusLine();
        }

        private void OnStandingRecordClicked()
        {
            SetupExpansions();
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("=== STANDING RECORD (Exp 03) ===");
            sb.AppendLine(_expansions.StandingRecordLine());
            sb.AppendLine(_expansions.RecordQuestLine());
            sb.AppendLine("Walk the route: Km 19 → Transit → Archive → Ministry → Weighbridge → Grange → Bridge → Lock → 12-B → Vault.");
            _codexViewer.Text = sb.ToString().TrimEnd();
            RefreshExpansionsStatus();
        }

        private void OnRecordWalkKm19Clicked()
        {
            SetupExpansions();
            var sb = new System.Text.StringBuilder();
            _expansions.UnlockRecord();
            _expansions.ArriveAtSite("loc_cut_kilometre_19");
            _expansions.EnterSiteRoom("room_km19_post");
            _expansions.InspectSiteRoom("room_km19_post");
            _expansions.EnterSiteRoom("room_km19_seam");
            sb.AppendLine(_expansions.RoomLine("loc_cut_kilometre_19", "room_km19_post"));
            sb.AppendLine();
            sb.AppendLine(_expansions.RoomLine("loc_cut_kilometre_19", "room_km19_seam"));
            _statusLabel.Text = sb.ToString().TrimEnd();
        }

        private void OnCrossingVouchClicked()
        {
            SetupExpansions();
            bool granted = _expansions.GrantVouch("npc_osran_kell");
            _statusLabel.Text = granted
                ? "Vouch granted by Osran Kell. The Crossing gate is open."
                : "Vouch refused (already granted, burned, or last resort spent).";
            RefreshExpansionsStatus();
        }

        private void OnCrossingBurnClicked()
        {
            SetupExpansions();
            bool burned = _expansions.BurnVouch();
            _statusLabel.Text = burned
                ? "Vouch burned. The gate is closed again — last resort remains available."
                : "Nothing to burn: no active vouch.";
            RefreshExpansionsStatus();
        }

        private void OnArbitrationLoadBackersClicked()
        {
            SetupExpansions();
            _expansions.LoadDefaultBackerPool();
            _statusLabel.Text = "Backer pool loaded: Osran Kell (principled), Mattis Cray (principled), Halden Mire, Bram Ostrowski, Leva Quist, Dessa Penn.";
            _codexViewer.Text = _expansions.ArbitrationLine();
            RefreshExpansionsStatus();
        }

        private void OnArbitrationCallStandingClicked()
        {
            SetupExpansions();
            if (_expansions.Arbitration.BackerPool.Count == 0)
            {
                _expansions.LoadDefaultBackerPool();
                _statusLabel.Text = "No backer pool — loaded defaults first.";
            }
            int day = _core != null ? _core.Clock.Day : _simDay;
            string topic = "quest_crossing_the_terms";
            bool called = _expansions.Arbitration.CallStanding(topic, day);
            _expansions.Arbitration.DeclareBacker(topic, CrossingIds.NpcOsran);
            _expansions.Arbitration.DeclareBacker(topic, CrossingIds.NpcMattis);
            _expansions.Arbitration.DeclareBacker(topic, "npc_halden_mire");
            _statusLabel.Text = called
                ? $"Standing called on '{topic}' with 3 backers (Osran, Mattis, Halden). Ruling: {_expansions.Arbitration.GetRuling(topic)?.shape}"
                : "Standing already held — overturn first or call a different topic.";
            _codexViewer.Text = _expansions.ArbitrationLine();
            RefreshExpansionsStatus();
        }

        private void OnArbitrationBribeClicked()
        {
            SetupExpansions();
            if (_expansions.Arbitration.BackerPool.Count == 0)
            {
                _expansions.LoadDefaultBackerPool();
                _statusLabel.Text = "No backer pool — loaded defaults first.";
            }
            // Set up a fresh ruling on a new topic
            string topic = CrossingIds.ScaleIntegrity;
            int day = _core != null ? _core.Clock.Day : _simDay;
            _expansions.Arbitration.CallStanding(topic, day);
            _expansions.Arbitration.DeclareBacker(topic, CrossingIds.NpcOsran);
            // Try bribing a principled backer (refused) and an unprincipled one (accepted)
            var resultPrincipled = _expansions.Arbitration.TryBribeBacker(topic, CrossingIds.NpcMattis);
            var resultBought = _expansions.Arbitration.TryBribeBacker(topic, "npc_bram_ostrowski");
            _expansions.Arbitration.DeclareBacker(topic, "npc_leva_quist");
            _statusLabel.Text = $"Bribe results: Mattis={resultPrincipled}, Bram={resultBought}. Ruling: {_expansions.Arbitration.GetRuling(topic)?.shape}";
            _codexViewer.Text = _expansions.ArbitrationLine();
            RefreshExpansionsStatus();
        }

        private void OnArbitrationOverturnClicked()
        {
            SetupExpansions();
            if (_expansions.Arbitration.BackerPool.Count == 0)
            {
                _expansions.LoadDefaultBackerPool();
            }
            string topic = "quest_crossing_the_terms";
            int day = _core != null ? _core.Clock.Day : _simDay;
            // Ensure a ruling exists to overturn
            if (!_expansions.Arbitration.IsRulingActive(topic))
            {
                _expansions.Arbitration.CallStanding(topic, day);
                _expansions.Arbitration.DeclareBacker(topic, CrossingIds.NpcOsran);
                _expansions.Arbitration.DeclareBacker(topic, CrossingIds.NpcMattis);
                _expansions.Arbitration.DeclareBacker(topic, "npc_halden_mire");
            }
            bool overturned = _expansions.Arbitration.OverturnRuling(topic,
                new List<string> { "npc_bram_ostrowski", "npc_leva_quist", "npc_halden_mire" });
            _statusLabel.Text = overturned
                ? "Ruling overturned! Counter-backers (Bram, Leva, Halden) hold the Crossing now."
                : "Overturn failed — need 3+ different, living backers.";
            _codexViewer.Text = _expansions.ArbitrationLine();
            RefreshExpansionsStatus();
        }

        private void OnLedgerSignClicked()
        {
            SetupExpansions();
            string debtor = CrossingIds.NpcWyn;
            bool firstRead = _expansions.Ledger.PresentContract(debtor, 12f, 30, 0.2f, "the pledged grain");
            bool secondRead = _expansions.Ledger.PresentContract(debtor, 12f, 30, 0.2f, "the pledged grain");
            bool signed = _expansions.Ledger.SignContract(debtor, _core != null ? _core.Clock.Day : _simDay);
            _statusLabel.Text = $"Contract for {debtor}: first reading={firstRead}, second reading={secondRead}, signed={signed}.";
            _codexViewer.Text = _expansions.LedgerLine();
            RefreshExpansionsStatus();
        }

        private void OnLedgerTickClicked()
        {
            SetupExpansions();
            int day = _core != null ? _core.Clock.Day : _simDay;
            _expansions.Ledger.TickDaily(day);
            _statusLabel.Text = "Ledger day ticked. " + _expansions.LedgerLine();
            _codexViewer.Text = _expansions.LedgerLine();
            RefreshExpansionsStatus();
        }

        private void OnLedgerPayClicked()
        {
            SetupExpansions();
            string debtor = CrossingIds.NpcWyn;
            bool paid = _expansions.Ledger.PayContract(debtor, _core != null ? _core.Clock.Day : _simDay);
            _statusLabel.Text = paid
                ? $"Contract for {debtor} paid in full. The ink is history."
                : "Payment failed — no signed contract or already paid.";
            _codexViewer.Text = _expansions.LedgerLine();
            RefreshExpansionsStatus();
        }

        private void OnWaystationTickClicked()
        {
            SetupExpansions();
            _expansions.UnlockWaystation();
            bool roadOpen = _core != null && _core.IceRoad.IsOpen;
            _expansions.TickWaystation(roadOpen);
            _statusLabel.Text = "Waystation: " + _expansions.WaystationLine();
            RefreshExpansionsStatus();
        }

        private void OnWaystationWatchClicked()
        {
            SetupExpansions();
            _expansions.UnlockWaystation();
            _expansions.AssignWaystationWatch(new[] { "elena_vasquez", "marcus_olejnik", "suki_tanaka" });
            _expansions.SetWaystationWintering(true);
            _statusLabel.Text = "Watch assigned (Vasquez, Olejnik, Tanaka). Wintering mode on — stove lit, filter degrades faster.";
            RefreshExpansionsStatus();
        }

        private void SaveExpansionHub()
        {
            if (_expansions == null) return;
            int day = _core != null ? _core.Clock.Day : _simDay;
            if (ExpansionHubSaveStore.TrySave(_expansions.CaptureSave(day)))
            {
                _expansionHubDirty = false;
                GD.Print($"[Ashfall Godot] Expansion hub save written (day {day}).");
            }
        }

        private void FlushExpansionHubIfDirty()
        {
            if (_expansionHubDirty || _foundryDirty) SaveExpansionHub();
        }

        private void CloseExpansionsHubPanel()
        {
            if (_expansionsHubPanel != null) _expansionsHubPanel.Visible = false;
        }

        private void CloseStandingRecordPanel()
        {
            if (_standingRecordPanel != null) _standingRecordPanel.Visible = false;
        }

        private void CloseCenturySeedPanel()
        {
            if (_centurySeedPanel != null) _centurySeedPanel.Visible = false;
        }

        private void CloseEpiloguePanel()
        {
            if (_epiloguePanel != null) _epiloguePanel.Visible = false;
        }
    }
}
