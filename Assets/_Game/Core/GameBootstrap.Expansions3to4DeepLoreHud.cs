using System;
using System.Collections.Generic;
using AtomicWar._Game.Data;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Events;
using AtomicWar._Game.Factions;
using AtomicWar._Game.Quests;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.UI;
using AtomicWar._Game.Utilities;
using AtomicWar._Game.World;
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
            bool locationWired = false;
            bool siegeWired = false;

            if (_hud.MapScreenUI != null && _hud.LocationDetailPanel != null)
            {
                Action onMapChanged = PushSelectedLocationToDetailPanel;
                _hud.MapScreenUI.OnMapScreenChanged += onMapChanged;
                _subscriptions.Track(() => _hud.MapScreenUI.OnMapScreenChanged -= onMapChanged);
                locationWired = true;
            }

            if (HatchDefenseSystem != null && _hud.SiegeStatusHud != null)
            {
                Action<float> onIntegrity = v => _hud.SiegeStatusHud.UpdateIntegrity(v);
                HatchDefenseSystem.OnHatchIntegrityChanged += onIntegrity;
                _subscriptions.Track(() => HatchDefenseSystem.OnHatchIntegrityChanged -= onIntegrity);

                Action<float> onBreach = v => _hud.SiegeStatusHud.UpdateBreachProgress(v);
                HatchDefenseSystem.OnBreachProgressChanged += onBreach;
                _subscriptions.Track(() => HatchDefenseSystem.OnBreachProgressChanged -= onBreach);

                Action<string, float> onEffect = (id, dur) => _hud.SiegeStatusHud.AddActiveEffect(id, dur);
                HatchDefenseSystem.OnTacticalEffectActivated += onEffect;
                _subscriptions.Track(() => HatchDefenseSystem.OnTacticalEffectActivated -= onEffect);

                Action<bool> onSiege = under =>
                {
                    if (under)
                    {
                        _hud.SiegeStatusHud.ShowSiege(
                            HatchDefenseSystem.HatchIntegrityPct,
                            HatchDefenseSystem.ReinforcementTier,
                            HatchDefenseSystem.BreachProgress);
                        _hud.TacticalCommandBar?.ShowCommands(
                            new[] { true, true, true, true, HatchDefenseSystem.IsDeconFlushReady },
                            new float[5],
                            OnTacticalCommand);
                    }
                    else
                    {
                        _hud.SiegeStatusHud.HideSiege();
                        _hud.TacticalCommandBar?.HideCommands();
                    }
                };
                HatchDefenseSystem.OnSiegeStateChanged += onSiege;
                _subscriptions.Track(() => HatchDefenseSystem.OnSiegeStateChanged -= onSiege);

                Action<RaidResolution> onRaid = _ => HatchDefenseSystem.SetUnderSiege(true);
                HatchDefenseSystem.OnRaidResolved += onRaid;
                _subscriptions.Track(() => HatchDefenseSystem.OnRaidResolved -= onRaid);
                siegeWired = true;
            }

            // #region agent log
            AgentDebugLog.Write("H2", "GameBootstrap.Expansions3to4DeepLoreHud.WireExp34Widgets", "wire branches",
                "{\"mapScreen\":" + (_hud.MapScreenUI != null ? "true" : "false")
                + ",\"hatchDefense\":" + (HatchDefenseSystem != null ? "true" : "false")
                + ",\"locationTodo\":" + (locationWired ? "false" : "true")
                + ",\"siegeTodo\":" + (siegeWired ? "false" : "true")
                + ",\"factionIntel\":" + (FactionIntel != null ? "true" : "false")
                + ",\"vehicle\":" + (VehicleMaintenance != null ? "true" : "false")
                + ",\"quests\":" + (DynamicQuestlines != null ? "true" : "false") + "}");
            // #endregion

            if (FactionIntel != null && _hud.FactionIntelligencePanel != null)
            {
                Action<IntelEntry> onIntel = e =>
                    _hud.FactionIntelligencePanel.AddIntelEntry(e.Description, e.HoursUntilExpiry);
                FactionIntel.OnIntelReceived += onIntel;
                _subscriptions.Track(() => FactionIntel.OnIntelReceived -= onIntel);
            }

            if (VehicleMaintenance != null && _hud.VehicleStatusPanel != null)
            {
                var v = VehicleMaintenance.GetVehicle("scout_truck");
                if (v != null)
                    _hud.VehicleStatusPanel.ShowVehicle("Armored Scout Truck",
                        v.ConditionPct, v.FuelLitres, v.MaxFuelCapacity,
                        v.CurrentCargoKg, v.CargoCapacityKg);

                Action<string, float> onCond = (id, cond) =>
                {
                    if (id == "scout_truck") _hud.VehicleStatusPanel.UpdateCondition(cond);
                };
                VehicleMaintenance.OnVehicleConditionChanged += onCond;
                _subscriptions.Track(() => VehicleMaintenance.OnVehicleConditionChanged -= onCond);

                Action<string, float> onFuel = (id, fuel) =>
                {
                    var veh = VehicleMaintenance.GetVehicle(id);
                    if (veh != null) _hud.VehicleStatusPanel.UpdateFuel(fuel, veh.MaxFuelCapacity);
                };
                VehicleMaintenance.OnFuelChanged += onFuel;
                _subscriptions.Track(() => VehicleMaintenance.OnFuelChanged -= onFuel);
            }

            if (DynamicQuestlines != null)
            {
                if (_hud.QuestlineStageTracker != null)
                {
                    Action<DynamicQuestState> onStarted = q =>
                        _hud.QuestlineStageTracker.AddQuest(q.QuestId, q.QuestTitle,
                            q.CurrentStage, q.TotalStages,
                            q.StageDescriptions != null && q.CurrentStage >= 0
                            && q.CurrentStage < q.StageDescriptions.Length
                                ? q.StageDescriptions[q.CurrentStage] : "", 0f);
                    DynamicQuestlines.OnQuestStarted += onStarted;
                    _subscriptions.Track(() => DynamicQuestlines.OnQuestStarted -= onStarted);
                }

                if (_hud.QuestlineProgressTracker != null)
                {
                    Action<DynamicQuestState> onProgress = q =>
                        _hud.QuestlineProgressTracker.ShowQuest(
                            q.QuestId, q.QuestTitle, q.StageDescriptions, q.CurrentStage);
                    DynamicQuestlines.OnQuestStarted += onProgress;
                    _subscriptions.Track(() => DynamicQuestlines.OnQuestStarted -= onProgress);
                }
            }
        }

        private void PushSelectedLocationToDetailPanel()
        {
            if (_hud?.LocationDetailPanel == null || _hud.MapScreenUI == null) return;
            string locId = _hud.MapScreenUI.SelectedNodeId;
            if (string.IsNullOrEmpty(locId))
            {
                _hud.LocationDetailPanel.Hide();
                return;
            }

            string displayName = locId;
            float danger = 0f;
            float rads = 0f;
            var catalogLoc = _locationCatalog != null ? _locationCatalog.GetById(locId) : null;
            if (catalogLoc != null)
            {
                displayName = catalogLoc.displayName;
                danger = catalogLoc.dangerLevel;
                rads = catalogLoc.baseRadsPerHour;
            }
            else
            {
                var node = GeneratedMap?.GetNode(locId);
                if (node != null)
                {
                    displayName = node.DisplayName;
                    danger = node.DangerLevel;
                    rads = node.TrueRad;
                }
            }

            string owner = LocationEvolutionSystem?.GetLocationState(locId)?.CurrentOwner ?? "none";
            _hud.LocationDetailPanel.ShowLocation(
                locId, displayName, danger, rads, 0.3f, owner, new List<LootPreviewEntry>());
        }

        private void OnTacticalCommand(int index)
        {
            if (HatchDefenseSystem == null) return;
            switch (index)
            {
                case 0: HatchDefenseSystem.IssueCommand("hold_the_line"); break;
                case 1: HatchDefenseSystem.IssueCommand("tactical_retreat"); break;
                case 2: HatchDefenseSystem.IssueCommand("suppressive_fire"); break;
                case 3: HatchDefenseSystem.DeployBarbedWire(); break;
                case 4: HatchDefenseSystem.TriggerDeconFlush(); break;
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

            if (NarrativeArcSystem != null && _hud.CharacterArcProgressPanel != null)
            {
                Action<Survivor, int, string> onArc = (sv, milestone, branch) =>
                {
                    if (sv == null) return;
                    _hud.CharacterArcProgressPanel.ShowCharacter(
                        sv.Id, sv.DisplayName, sv.PreWarProfessionId, "",
                        null, milestone, branch);
                    if (!string.IsNullOrEmpty(branch))
                        _hud.CharacterArcProgressPanel.SetBranchChosen(branch);
                };
                NarrativeArcSystem.OnArcMilestoneReached += onArc;
                _subscriptions.Track(() => NarrativeArcSystem.OnArcMilestoneReached -= onArc);

                Action<Survivor, string, string> onBranch = (sv, branch, trait) =>
                {
                    if (sv == null) return;
                    _hud.CharacterArcProgressPanel.SetBranchChosen(branch);
                    if (!string.IsNullOrEmpty(trait))
                        _hud.CharacterArcProgressPanel.SetArcComplete(trait, "");
                };
                NarrativeArcSystem.OnArcBranchChosen += onBranch;
                _subscriptions.Track(() => NarrativeArcSystem.OnArcBranchChosen -= onBranch);
            }
        }
    }
}
