using System;
using System.Collections.Generic;
using AtomicWar._Game.Factions;
using AtomicWar._Game.Quests;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.UI;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Expansions 3&4 + Deep Lore — HUD widget event wiring.
    /// Wires all 11 new widgets to their system events.
    /// </summary>
    public partial class GameBootstrap
    {
        private bool _exp34HudWired;

        private void WireExpansions3to4DeepLoreHud()
        {
            if (_hud == null || _exp34HudWired) return;
            _exp34HudWired = true;

            BindExp34Documents();
            WireExp34Widgets();
            WireDeepLoreWidgets();
        }

        private void BindExp34Documents()
        {
            var doc = _hud.DiegeticHud != null ? _hud.DiegeticHud.Document : null;
            if (doc == null) return;
            _hud.LocationDetailPanel?.BindDocument(doc);
            _hud.ItemConditionBadge?.BindDocument(doc);
            _hud.QuestlineProgressTracker?.BindDocument(doc);
            _hud.SiegeStatusHud?.BindDocument(doc);
            _hud.FactionIntelligencePanel?.BindDocument(doc);
            _hud.VehicleStatusPanel?.BindDocument(doc);
            _hud.TacticalCommandBar?.BindDocument(doc);
            _hud.QuestlineStageTracker?.BindDocument(doc);
            _hud.LoreCodexPanel?.BindDocument(doc);
            _hud.FactionRelationshipMap?.BindDocument(doc);
            _hud.CharacterArcProgressPanel?.BindDocument(doc);
        }

        private void WireExp34Widgets()
        {
            // LocationDetailPanel — wire to MapScreenUI location clicks
            if (_hud.MapScreenUI != null && _hud.LocationDetailPanel != null)
            {
                Action<string> onLoc = locId =>
                {
                    var loc = _locationCatalog?.GetById(locId);
                    if (loc == null) return;
                    _hud.LocationDetailPanel.ShowLocation(locId, loc.displayName,
                        loc.dangerLevel, loc.baseRadsPerHour, 0.3f,
                        LocationEvolutionSystem?.GetLocationState(locId)?.CurrentOwner ?? "none",
                        new List<LootPreviewEntry>());
                };
                _hud.MapScreenUI.OnLocationSelected += onLoc;
                _subscriptions.Track(() => _hud.MapScreenUI.OnLocationSelected -= onLoc);
            }

            // SiegeStatusHUD — wire to HatchDefenseSystem siege events
            if (HatchDefenseSystem != null && _hud.SiegeStatusHud != null)
            {
                Action<float> onIntegrity = v => _hud.SiegeStatusHud.UpdateIntegrity(v);
                HatchDefenseSystem.OnHatchIntegrityChanged += onIntegrity;
                _subscriptions.Track(() => HatchDefenseSystem.OnHatchIntegrityChanged -= onIntegrity);
            }

            // FactionIntelligencePanel
            if (FactionIntel != null && _hud.FactionIntelligencePanel != null)
            {
                Action<IntelEntry> onIntel = e =>
                    _hud.FactionIntelligencePanel.AddIntelEntry(e.Description, e.HoursUntilExpiry);
                FactionIntel.OnIntelReceived += onIntel;
                _subscriptions.Track(() => FactionIntel.OnIntelReceived -= onIntel);
            }

            // VehicleStatusPanel
            if (VehicleMaintenance != null && _hud.VehicleStatusPanel != null)
            {
                var v = VehicleMaintenance.GetVehicle("scout_truck");
                if (v != null)
                    _hud.VehicleStatusPanel.ShowVehicle("Armored Scout Truck",
                        v.ConditionPct, v.FuelLitres, v.MaxFuelCapacity,
                        v.CurrentCargoKg, v.CargoCapacityKg);
            }

            // QuestlineStageTracker — wire to DynamicQuestlineSystem
            if (DynamicQuestlines != null && _hud.QuestlineStageTracker != null)
            {
                Action<DynamicQuestState> onStarted = q =>
                    _hud.QuestlineStageTracker.AddQuest(q.QuestId, q.QuestTitle,
                        q.CurrentStage, q.TotalStages,
                        q.StageDescriptions?[q.CurrentStage] ?? "", 0f);
                DynamicQuestlines.OnQuestStarted += onStarted;
                _subscriptions.Track(() => DynamicQuestlines.OnQuestStarted -= onStarted);
            }
        }

        private void WireDeepLoreWidgets()
        {
            // LoreCodexPanel — wire to JournalSystem knowledge entries
            if (JournalSystem != null && _hud.LoreCodexPanel != null)
            {
                Action<JournalEntry> onEntry = entry =>
                {
                    if (entry?.KnowledgeKey == null) return;
                    if (entry.KnowledgeKey.StartsWith("lore_"))
                        _hud.LoreCodexPanel.UnlockHistoryEntry(entry.KnowledgeKey);
                };
                JournalSystem.OnEntryAdded += onEntry;
                _subscriptions.Track(() => JournalSystem.OnEntryAdded -= onEntry);
            }

            // FactionRelationshipMap
            if (EconomySystem != null && _hud.FactionRelationshipMap != null)
            {
                string[] factions = { "garrison", "militia", "cult", "warlords" };
                foreach (var f in factions)
                    _hud.FactionRelationshipMap.SetFactionNodeData(f, f,
                        EconomySystem.GetStanding(f), "neutral");
            }
        }
    }
}
