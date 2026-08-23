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
        // ── Muster fields (GAP-ARCH-01 Phase 1) ──
        private MusterHostSession _muster = null!;
        private CurrentsRosterWidget _currentsRoster = null!;
        private ApproachSelectionModal _approachModal = null!;
        private DeserterCoalitionCampWidget _campWidget = null!;
        private JournalWitnessPanel _witnessPanel = null!;

        private void SetupMuster()
        {
            if (_muster != null) return;
            _muster = MusterHostSession.Create(_dataDir);
            _muster.StateChanged += () => SaveMuster();
            _muster.OnQuestlineResolved += OnMusterQuestlineResolved;

            if (_currentsRoster == null)
            {
                _currentsRoster = new CurrentsRosterWidget();
                _rightColumn.AddChild(_currentsRoster);
            }
            _currentsRoster.Bind(_muster.Roster, _muster.Engine);
            _currentsRoster.RefreshView();

            if (_campWidget == null)
            {
                _campWidget = new DeserterCoalitionCampWidget();
                _rightColumn.AddChild(_campWidget);
            }
            _campWidget.Bind(_muster.Camp);
            _campWidget.RefreshView();

            if (_witnessPanel == null)
            {
                _witnessPanel = new JournalWitnessPanel();
                _rightColumn.AddChild(_witnessPanel);
            }
            _witnessPanel.Bind(_muster.Witnesses);
            _witnessPanel.RefreshView(_yearOfAsh != null ? _yearOfAsh.Timeline.CurrentDay : _simDay, _muster.AuthorBias);

            if (_approachModal == null)
            {
                _approachModal = new ApproachSelectionModal();
                _approachModal.OnApproachChosen += OnMusterApproachChosen;
                _approachModal.OnModalClosed += () =>
                {
                    _approachModal.QueueFree();
                    _approachModal = null!;
                };
                AddChild(_approachModal);
            }

            int day = _yearOfAsh != null ? _yearOfAsh.Timeline.CurrentDay : _simDay;
            _muster.Escalate(day);
            GD.Print("[Ashfall Godot] Muster ready. Day " + day +
                     (_muster.Engine.MusterTriggered ? " — THE MUSTER IS OPEN." : "."));
        }

        public void OnColdCountClicked()
        {
            SetupMuster();
            var cc = _muster.ColdCount;
            _codexViewer.Text =
                "=== FACTION: COLD COUNT (142.850 MHz) ===\n" +
                $"Is Active: {cc.State.isActive}\n" +
                $"Power Supplied Days: {cc.PowerSuppliedDays}/{Ashfall.Core.Muster.ColdCountState.RequiredPowerDays}\n" +
                $"Shielding Delivered: {cc.ShieldingDelivered}/{Ashfall.Core.Muster.ColdCountState.RequiredShieldingUnits}\n" +
                $"Provenance Complete: {cc.ProvenanceDataComplete}\n" +
                $"Broadcast Sent: {cc.BroadcastSent} (Day {cc.State.broadcastDay})\n" +
                $"Trust: {cc.State.trust:F1}\n\n" +
                "The four researchers at loc_low_background_lab hold the isotopic provenance of who fired first.";
            _statusLabel.Text = $"Cold Count: {cc.PowerSuppliedDays}d power, {cc.ShieldingDelivered} shielding units.";
        }

        public void OnHydroBaronsClicked()
        {
            SetupMuster();
            var hb = _muster.HydroBarons;
            _codexViewer.Text =
                "=== FACTION: COASTAL HYDRO-BARONS ===\n" +
                $"Is Active: {hb.State.isActive}\n" +
                $"Rate Card Revised: {hb.RateCardRevised}\n" +
                $"Plant Seized: {hb.PlantSeized}\n" +
                $"Admin Reform: {hb.AdminReform}\n" +
                $"Queue Position: {hb.QueuePosition}\n" +
                $"Trust: {hb.State.trust:F1}\n" +
                $"Approach: {(string.IsNullOrEmpty(hb.State.approach) ? "Unresolved" : hb.State.approach)}\n\n" +
                "The Rate Card War at Desalination Unit 4. The iron chit queue governs fresh water allocation.";
            _statusLabel.Text = $"Hydro-Barons: Queue Pos {hb.QueuePosition}, Approach {hb.State.approach}.";
        }

        public void OnIronRaidersClicked()
        {
            SetupMuster();
            var ir = _muster.IronRaiders;
            _codexViewer.Text =
                "=== FACTION: IRON RAIDERS (DEN DEFENSE) ===\n" +
                $"Is Active: {ir.State.isActive}\n" +
                $"Aggression Level: {ir.AggressionLevel:P0}\n" +
                $"Shelter Visibility: {ir.State.shelterVisibility:P0}\n" +
                $"Raid Chance Today: {ir.EvaluateRaidChance():P0}\n" +
                $"Raids This Season: {ir.RaidsThisSeason}\n\n" +
                "The Toll's den at loc_iron_raiders_den. Fortifying approach routes reduces shelter visibility and raid chance.";
            _statusLabel.Text = $"Iron Raiders: Aggression {ir.AggressionLevel:P0}, Raid Chance {ir.EvaluateRaidChance():P0}.";
        }

        public void OnLongWalkClicked()
        {
            SetupMuster();
            var lw = _muster.LongWalk;
            _codexViewer.Text =
                "=== FACTION: THE LONG WALK (CIRCUIT TRADER) ===\n" +
                $"Is Active: {lw.State.isActive}\n" +
                $"Current Region: {lw.State.currentRegion}\n" +
                $"Days Until Departure: {lw.State.daysUntilDeparture}\n" +
                $"Crossings Completed: {lw.State.crossingsCompleted}\n" +
                $"Escort Count: {lw.State.escortCount}\n" +
                $"Resupply Count: {lw.State.resupplyCount}\n\n" +
                "Osric Fane's circuit trader across six regions. Requests return a deliberately stale situation report.";
            _statusLabel.Text = $"Long Walk: in {lw.State.currentRegion}, departs in {lw.State.daysUntilDeparture} days.";
        }

        public void OnProvisionedClicked()
        {
            SetupMuster();
            var ps = _muster.Provisioned;
            _codexViewer.Text =
                "=== FACTION: THE PROVISIONED (SECOND WINTER) ===\n" +
                $"Is Active: {ps.State.isActive}\n" +
                $"Respect Score: {ps.RespectScore}/{Ashfall.Core.Muster.ProvisionedState.ContactThreshold}\n" +
                $"Contact Made: {ps.HaveMadeContact}\n" +
                $"Unlocked Trades: {ps.State.unlockedTradeIds.Count}\n\n" +
                "Pre-war stockholders behind Quenna Brix at loc_second_winter_homestead. Respect is earned unprompted.";
            _statusLabel.Text = $"The Provisioned: Respect {ps.RespectScore}, Contact: {ps.HaveMadeContact}.";
        }

        public void OnScavengerGuildClicked()
        {
            SetupMuster();
            var sg = _muster.ScavengerGuild;
            _codexViewer.Text =
                "=== FACTION: SCAVENGER GUILD (CLAIM MAP) ===\n" +
                $"Is Active: {sg.State.isActive}\n" +
                $"Claimed Sites: {sg.State.claimedSiteIds.Count}\n" +
                $"Blacklisted Shelters: {sg.State.blacklistedShelterIds.Count}\n" +
                $"Trust: {sg.Trust:F1}\n\n" +
                "Brannick Sten's two-color claim ledger at loc_scavenger_guildhall. Over-stripping permanently blacklists.";
            _statusLabel.Text = $"Scavenger Guild: {sg.State.claimedSiteIds.Count} claims, Trust {sg.Trust:F1}.";
        }

        public void OnMusterEscalateClicked()
        {
            SetupMuster();
            int target = Math.Min(360, _yearOfAsh != null ? _yearOfAsh.Timeline.CurrentDay + 10 : _simDay + 10);
            _statusLabel.Text = _muster.Escalate(target);
            _currentsRoster.RefreshView();
            _campWidget.RefreshView();
        }

        private void OnMusterRallyClicked()
        {
            SetupMuster();
            _statusLabel.Text = _muster.RallyDeserter();
            _campWidget.RefreshView();
        }

        private void OnMusterStrategyBClicked()
        {
            SetupMuster();
            _statusLabel.Text = _muster.SetStrategy(QuestApproach.B);
            _campWidget.RefreshView();
        }

        private void OnMusterStrategyDClicked()
        {
            SetupMuster();
            _statusLabel.Text = _muster.SetStrategy(QuestApproach.D);
            _campWidget.RefreshView();
        }

        private void OnMusterWitnessesClicked()
        {
            SetupMuster();
            int day = _yearOfAsh != null ? _yearOfAsh.Timeline.CurrentDay : _simDay;
            _witnessPanel.RefreshView(day, _muster.AuthorBias);
            _statusLabel.Text = _muster.Witnesses.Count == 0
                ? "No witness accounts loaded."
                : $"Three accounts: {_muster.Witnesses.Count} loaded. Day {day} · {_muster.AuthorBias} author.";
        }

        private void OnMusterAuthorBiasClicked()
        {
            SetupMuster();
            int day = _yearOfAsh != null ? _yearOfAsh.Timeline.CurrentDay : _simDay;
            _statusLabel.Text = _muster.CycleAuthorBias();
            _witnessPanel.RefreshView(day, _muster.AuthorBias);
        }

        private void OnMusterEpiloguesClicked()
        {
            SetupMuster();
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("=== THE EPILOGUE MATRIX (DAY 360) ===");
            for (int i = 0; i < _muster.Epilogues.Count; i++)
            {
                var e = _muster.Epilogues[i];
                bool resolved = _muster.Engine.EndingKeyForAny(e.endingKey);
                sb.AppendLine(resolved
                    ? $"[RESOLVED] {e.title}"
                    : $"[open]     {e.title}");
            }
            sb.AppendLine();
            sb.AppendLine("=== RESOLVED OUTCOMES ===");
            bool any = false;
            for (int i = 0; i < _muster.Epilogues.Count; i++)
            {
                var e = _muster.Epilogues[i];
                string prose = _muster.EndingProseFor(e.endingKey);
                if (_muster.Engine.EndingKeyForAny(e.endingKey) && prose.Length > 0)
                {
                    any = true;
                    sb.AppendLine(prose);
                    sb.AppendLine();
                }
            }
            if (!any) sb.AppendLine("None. The Muster has not resolved an outcome yet.");
            _codexViewer.Text = sb.ToString();
            _statusLabel.Text = $"Epilogue matrix: {_muster.Epilogues.Count} outcomes.";
        }

        private void OnMusterRosterClicked()
        {
            SetupMuster();
            _statusLabel.Text = $"Currents shown: {_muster.Roster.Count} (fifteenth: faction_hydro_barons).";
        }

        private void OpenMusterApproachModal(string questlineId, IReadOnlyList<ApproachOption> approaches)
        {
            _selectedApproachQuestlineId = questlineId;
            if (_approachModal == null)
            {
                _approachModal = new ApproachSelectionModal();
                _approachModal.OnApproachChosen += OnMusterApproachChosen;
                _approachModal.OnModalClosed += () =>
                {
                    _approachModal?.QueueFree();
                    _approachModal = null!;
                };
                AddChild(_approachModal);
            }
            _approachModal.ShowQuestline(questlineId, approaches);
            _statusLabel.Text = $"{questlineId}: choose an approach.";
        }

        private void OnMusterRateCardClicked()
        {
            SetupMuster();
            var def = _muster.Engine.FindDefinition("quest_the_rate_card_war");
            if (def == null)
            {
                _statusLabel.Text = "Rate Card War questline not registered.";
                return;
            }
            OpenMusterApproachModal(def.questlineId, def.approaches);
        }

        private void OnMusterApproachChosen(QuestApproach approach)
        {
            if (_muster == null) return;
            string qId = string.IsNullOrEmpty(_selectedApproachQuestlineId) ? "quest_the_rate_card_war" : _selectedApproachQuestlineId;
            _statusLabel.Text = _muster.SelectApproach(qId, approach);
            _currentsRoster?.RefreshView();
            _musterPanel?.RefreshView();
        }

        private void OnMusterQuestlineResolved(MusterRecord record)
        {
            if (record == null) return;
            string line = $"[MUSTER RESOLVED] {record.questlineId} via {record.selectedApproach} → Ending: {record.endingKey}";
            GD.Print($"[Ashfall Godot] {line}");
            if (_hostEventAdapter != null)
            {
                string eventId = $"event_muster_{record.questlineId}_{record.selectedApproach}";
                _hostEventAdapter.TriggerEvent(eventId, _simDay);
            }
            if (_journal != null)
            {
                _journal.TryAddRawEntry(
                    $"muster_{record.questlineId}_{record.selectedApproach}",
                    line,
                    null!,
                    _simDay);
                _journalDirty = true;
            }
            _statusLabel?.SetDeferred(Label.PropertyName.Text, line);
        }

        /// <summary>Auto-escalate the Muster from the Year-of-Ash clock.</summary>
        private void AutoEscalateMuster()
        {
            if (_yearOfAsh == null) return;
            SetupMuster();
            _muster.Escalate(_yearOfAsh.Timeline.CurrentDay);
            _currentsRoster.RefreshView();
            _campWidget.RefreshView();
            _witnessPanel.RefreshView(_yearOfAsh.Timeline.CurrentDay, _muster.AuthorBias);
        }

        private void SaveMuster()
        {
            if (_muster == null) return;
            if (MusterSaveStore.TrySave(_muster.CaptureSave()))
                GD.Print("[Ashfall Godot] Muster save written.");
        }

        private void CloseMusterPanel()
        {
            if (_musterPanel != null)
                _musterPanel.Visible = false;
        }

    }
}
