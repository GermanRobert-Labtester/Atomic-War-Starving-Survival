using System;
using AtomicWar._Game.Events;
using AtomicWar._Game.Factions;
using AtomicWar._Game.Quests;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.UI;
using AtomicWar._Game.Utilities;
using UnityEngine.UIElements;

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
            if (doc == null)
            {
                // #region agent log
                AgentDebugLog.Write("H4", "GameBootstrap.Expansions3to4DeepLoreHud.BindExp34Documents", "no document",
                    "{\"diegeticNull\":" + (_hud.DiegeticHud == null ? "true" : "false") + "}");
                // #endregion
                return;
            }
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
            // #region agent log
            var root = doc.rootVisualElement;
            string[] names = {
                "location-detail-panel","item-condition-badge","questline-tracker","siege-status",
                "faction-intelligence-panel","vehicle-status-panel","tactical-command-bar",
                "questline-stage-tracker","lore-codex-panel","faction-relationship-map",
                "character-arc-panel","radiation-phase-root","keepsake-slot-root"
            };
            var found = new System.Text.StringBuilder();
            found.Append('{');
            for (int i = 0; i < names.Length; i++)
            {
                if (i > 0) found.Append(',');
                bool ok = root != null && root.Q(names[i]) != null;
                found.Append('"').Append(names[i]).Append("\":").Append(ok ? "true" : "false");
            }
            found.Append(",\"childCount\":").Append(root != null ? root.childCount : -1);
            found.Append(",\"uxml\":\"").Append(doc.visualTreeAsset != null ? doc.visualTreeAsset.name : "null").Append('"');
            found.Append(",\"hudWidgets\":{");
            found.Append("\"location\":").Append(_hud.LocationDetailPanel != null ? "true" : "false");
            found.Append(",\"siege\":").Append(_hud.SiegeStatusHud != null ? "true" : "false");
            found.Append(",\"tactical\":").Append(_hud.TacticalCommandBar != null ? "true" : "false");
            found.Append(",\"arc\":").Append(_hud.CharacterArcProgressPanel != null ? "true" : "false");
            found.Append("}}");
            AgentDebugLog.Write("H1", "GameBootstrap.Expansions3to4DeepLoreHud.BindExp34Documents", "exp34 bind probe", found.ToString());
            // #endregion
        }

        private void WireExp34Widgets()
        {
            // #region agent log
            AgentDebugLog.Write("H2", "GameBootstrap.Expansions3to4DeepLoreHud.WireExp34Widgets", "wire branches",
                "{\"mapScreen\":" + (_hud.MapScreenUI != null ? "true" : "false")
                + ",\"hatchDefense\":" + (HatchDefenseSystem != null ? "true" : "false")
                + ",\"locationTodo\":true,\"siegeTodo\":true"
                + ",\"factionIntel\":" + (FactionIntel != null ? "true" : "false")
                + ",\"vehicle\":" + (VehicleMaintenance != null ? "true" : "false")
                + ",\"quests\":" + (DynamicQuestlines != null ? "true" : "false") + "}");
            // #endregion
            // LocationDetailPanel — TODO: MapScreenUI has no OnLocationSelected event yet;
            // wire when the event is added to MapScreenUI.

            // SiegeStatusHUD — TODO: HatchDefenseSystem has no OnHatchIntegrityChanged event yet.

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
                        EconomySystem.GetTrust(f), "neutral");
            }
        }
    }
}
