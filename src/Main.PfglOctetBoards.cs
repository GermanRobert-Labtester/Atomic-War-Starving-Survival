// SPDX-License-Identifier: MIT
using System;
using System.Linq;
using Godot;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Survivors;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        private RomanceFamilyBoardPanel? _romanceFamilyBoard;
        private ColonyOperationsBoardPanel? _colonyOperationsBoard;
        private IdeologicalMediationDeskPanel? _ideologicalMediationDesk;

        private void EnsurePfglOctetBoardPanels()
        {
            if (_romanceFamilyBoard == null)
            {
                _romanceFamilyBoard = new RomanceFamilyBoardPanel();
                AddChild(_romanceFamilyBoard);
            }

            if (_colonyOperationsBoard == null)
            {
                _colonyOperationsBoard = new ColonyOperationsBoardPanel();
                _colonyOperationsBoard.EstablishRequested += EstablishColonyFromBoard;
                _colonyOperationsBoard.SupplyLineStatusRequested += SetColonySupplyLineStatusFromBoard;
                AddChild(_colonyOperationsBoard);
            }

            if (_ideologicalMediationDesk == null)
            {
                _ideologicalMediationDesk = new IdeologicalMediationDeskPanel();
                _ideologicalMediationDesk.MediationRequested += ResolveIdeologicalMediationFromBoard;
                AddChild(_ideologicalMediationDesk);
            }
        }

        private void BindRomanceFamilyBoard()
        {
            EnsurePfglOctetBoardPanels();
            SetupRomanceFamily();
            _romanceFamilyBoard!.Bind(_romanceFamily);
        }

        private void BindColonyOperationsBoard()
        {
            EnsurePfglOctetBoardPanels();
            SetupCampaignDay();
            SetupColony();
            SetupExpeditions();
            var known = _expeditions?.Engine?.CaptureKnownLocations();
            _colonyOperationsBoard!.Bind(_colony?.System, known, _simDay);
        }

        private void BindIdeologicalMediationDesk()
        {
            EnsurePfglOctetBoardPanels();
            SetupSurvivors();
            SetupSurvivorSocial();
            SetupIdeologicalFriction();
            _ideologicalMediationDesk!.Bind(_ideologicalFriction);
        }

        private void EstablishColonyFromBoard(string locationId, string name, ColonyType type)
        {
            SetupCampaignDay();
            SetupColony();
            SetupExpeditions();
            var board = _colonyOperationsBoard;
            if (board == null) return;

            if (_colony?.System == null || _expeditions?.Engine == null)
            {
                board.ReportFeedback("Colony or expedition authority is unavailable.");
                return;
            }

            if (!_expeditions.Engine.IsLocationKnown(locationId))
            {
                board.ReportFeedback("That location is not in the discovered expedition record.");
                board.RefreshView();
                return;
            }

            if (_colony.System.GetColonies().Any(c => string.Equals(c.LocationId, locationId, StringComparison.OrdinalIgnoreCase)))
            {
                board.ReportFeedback("A settlement already occupies that location.");
                board.RefreshView();
                return;
            }

            _colony.System.EstablishColony(
                locationId,
                name,
                type,
                initialGarrison: Array.Empty<string>(),
                currentDay: Math.Max(1, _simDay));
            board.ReportFeedback($"Settlement established at {locationId}.");
            board.Bind(_colony.System, _expeditions.Engine.CaptureKnownLocations(), _simDay);
            SaveAll(playCue: false);
        }

        private void SetColonySupplyLineStatusFromBoard(string lineId, SupplyLineStatus status)
        {
            SetupColony();
            if (_colony?.System == null || !_colony.System.SetSupplyLineStatus(lineId, status))
            {
                _colonyOperationsBoard?.ReportFeedback("The selected supply line is no longer available.");
                _colonyOperationsBoard?.RefreshView();
                return;
            }

            _colonyOperationsBoard?.ReportFeedback($"Supply line {lineId} is now {status.ToString().ToLowerInvariant()}.");
            _colonyOperationsBoard?.RefreshView();
            SaveAll(playCue: false);
        }

        private void ResolveIdeologicalMediationFromBoard(string instanceId, IdeologicalMediationChoice choice)
        {
            SetupSurvivors();
            SetupSurvivorSocial();
            SetupIdeologicalFriction();
            var desk = _ideologicalMediationDesk;
            if (desk == null) return;

            if (_ideologicalFriction == null || _survivorRelationsCore == null || _survivors?.Needs == null)
            {
                desk.ReportFeedback("The mediation or survivor state authority is unavailable.");
                return;
            }

            var incident = _ideologicalFriction.RecentEvents.FirstOrDefault(e =>
                string.Equals(e.instanceId, instanceId, StringComparison.OrdinalIgnoreCase));
            if (incident == null || incident.isResolved ||
                (incident.eventType != IdeologicalEventType.Confrontation && incident.eventType != IdeologicalEventType.MediationQuest))
            {
                desk.ReportFeedback("That event is no longer open for mediation.");
                desk.Bind(_ideologicalFriction);
                return;
            }

            if (!_ideologicalFriction.ResolveConfrontation(
                    instanceId,
                    choice,
                    out float affinityDeltaA,
                    out float affinityDeltaB,
                    out float moraleDelta))
            {
                desk.ReportFeedback("The mediation choice could not be applied.");
                desk.Bind(_ideologicalFriction);
                return;
            }

            // SurvivorRelationsSystem stores one shared affinity per pair. The
            // Core event returns one perspective delta per survivor, so its
            // canonical pair projection is their mean applied exactly once.
            _survivorRelationsCore.ModifyAffinity(
                incident.actorId,
                incident.targetId,
                (affinityDeltaA + affinityDeltaB) * 0.5f);

            if (_survivors.Needs.Get(incident.actorId) != null)
                _survivors.Needs.Modify(incident.actorId, NeedKind.Morale, moraleDelta);
            if (_survivors.Needs.Get(incident.targetId) != null)
                _survivors.Needs.Modify(incident.targetId, NeedKind.Morale, moraleDelta);

            desk.ReportFeedback($"Mediation recorded: {incident.actorId} and {incident.targetId} · {choice}. The shared pair affinity and both morale records were updated through their current owners.");
            desk.Bind(_ideologicalFriction);
            SaveAll(playCue: false);
        }
    }
}
