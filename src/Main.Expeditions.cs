using Godot;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using AtomicWar.Journal;
using Ashfall.Core;
using Ashfall.Core.Combat;
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
        // ── Expedition fields (GAP-ARCH-01 Phase 1) ──
        private ExpeditionHostSession _expeditions = null!;
        private bool _expeditionDirty;
        private Ashfall.Core.Expeditions.EncounterChoiceResolver _encounterChoice = null!;
        private bool _encounterChoiceDirty;
        private CombatHostSession _combat = null!;
        private bool _combatDirty;

        private void FlushExpeditionIfDirty()
        {
            if (_expeditionDirty) SaveExpeditions();
        }

        /// <summary>
        /// Projects the wasteland map authority's water routes (travel_domain
        /// "water" from the home holdfast) into the expedition session so
        /// river-crossing dispatches use the naval craft profile + piracy
        /// weighting. Re-run after world/map restore.
        /// </summary>
        private void SyncWaterRoutes()
        {
            if (_expeditions == null || _world?.WastelandMap == null) return;
            _expeditions.WaterRouteHazards.Clear();
            foreach (var route in _world.WastelandMap.Routes)
            {
                if (route == null || route.From != "loc_holdfast") continue;
                if (!string.Equals(route.TravelDomain, "water", StringComparison.OrdinalIgnoreCase)) continue;
                _expeditions.WaterRouteHazards[route.To] = route.WeatherHazard;
            }
        }

        /// <summary>
        /// Plan 85 — binds the damaged-map authority to the expedition engine
        /// once both hosting sessions exist. Called from both SetupWorld and
        /// SetupExpeditions; composition order then doesn't matter.
        /// </summary>
        private void AttachDamagedMapIfReady()
        {
            if (_expeditions?.Engine == null || _world?.DamagedMap == null) return;
            if (_expeditions.Engine.DamagedMap == _world.DamagedMap) return;
            _expeditions.Engine.DamagedMap = _world.DamagedMap;
        }

        private void SetupExpeditions()
        {
            if (_expeditions != null) return;
            SetupInventory();
            SetupSurvivors();
            // F1–F4 — the expedition encounter flow resolves through the SAME
            // narrative engine the "narrative" save section persists. Without
            // this the session ran an empty catalog and never saved history,
            // depletion, or pending rows.
            EnsureNarrativeSession();
            _expeditions = ExpeditionHostSession.Create(_dataDir, _narrative.Engine);
            _expeditions.Flags = _consequenceLedger;
            BindExpeditionJournalIfReady();
            if (_inventory != null)
            {
                _expeditions.ShelterInventory = _inventory.Inventory;
                _expeditions.Items = _inventory.Catalog;
            }
            AttachDamagedMapIfReady();
            // Plan 52: travel-encounter decisions land in the persisted
            // expansion-quest ledger — the recurring-NPC arc memory authority.
            SetupExpansionQuests();
            if (_expeditions.NarrativeEngine != null)
                _expeditions.NarrativeEngine.QuestLink = _expansionQuests.System;
            _expeditions.StateChanged += () => _expeditionDirty = true;
            _expeditions.OnEncounterSurfaced += OnExpeditionEncounterSurfaced;
            SyncWaterRoutes();
            _expeditions.Engine.OnExpeditionCompleted += state =>
            {
                if (state == null) return;
                if (_inventory != null && state.loot != null)
                {
                    for (int i = 0; i < state.loot.Count; i++)
                    {
                        var item = state.loot[i];
                        if (item != null && !string.IsNullOrEmpty(item.itemId))
                        {
                            _inventory.Add(item.itemId, Math.Max(1, item.quantity));
                        }
                    }
                    SaveInventory();
                }

                if (_survivors != null && !string.IsNullOrEmpty(state.survivorId))
                {
                    var sv = _survivors.Find(state.survivorId);
                    if (sv != null)
                    {
                        // Strenuous expedition exertion adds fatigue
                        _survivors.Needs.Modify(sv, NeedKind.Fatigue, 25f);
                    }
                }

                if (_journal != null)
                {
                    _journal.TryAddRawEntry(
                        $"exp_{state.survivorId}_{_expeditions.CurrentDay}",
                        $"Survivor {state.survivorId} returned from {state.displayName} with {state.loot?.Count ?? 0} salvage items.",
                        null!,
                        _expeditions.CurrentDay);
                }

                GD.Print($"[Ashfall Godot] Expedition completed for {state.survivorId}: {state.loot?.Count ?? 0} loot items deposited into shelter inventory.");
            };
            GD.Print("[Ashfall Godot] Expedition host ready: encounters · dive instance.");
        }

        private void SaveExpeditions()
        {
            if (_expeditions == null) return;
            if (CaptureSection("expedition", ExpeditionSaveStore.TryCapturePersisted(_expeditions.CaptureSaveAggregate())))
            {
                _expeditionDirty = false;
                GD.Print("[Ashfall Godot] Expedition save written.");
            }
        }

        private void SetupCombat()
        {
            if (_combat != null) return;
            SetupInventory();
            SetupSurvivors();
            SetupPhase0();
            _combat = CombatHostSession.Create(_dataDir);
            if (_combat != null)
            {
                _combat.Inventory = _inventory;
                _combat.Survivors = _survivors;
                _combat.Equipment = _equipmentCondition?.System;
                // MarkCombatSurvived is a required combat effect (see
                // CombatHostSession.ValidatePorts / WeaponConditionSystem's
                // UnboundRequiredEffects) that was previously left unwired,
                // so every surviving combatant's hypervigilance/trauma
                // tracking silently no-op'd in production.
                _combat.WireRealState(markCombatSurvived: survivorId => _phase0.RegisterCombatSurvived(survivorId));
                _combat.ValidatePorts();
                _combat.StateChanged += () => _combatDirty = true;
                // Expedition encounters auto-populate a real combat encounter.
                SetupExpeditionCombatHandoff(_combat);
            }
            GD.Print("[Ashfall Godot] Combat host ready: tactical combat expansion.");
        }

        /// <summary>
        /// Refuel a garage vehicle from carried fuel items (1 item = 1 fuel
        /// unit). Consumes the items first so inventory is never drained
        /// without a tank fill.
        /// </summary>
        private string RefuelVehicleFromInventory(string vehicleId, float units)
        {
            if (_expeditions == null || _inventory == null) return "Garage not ready.";
            int needed = Math.Max(1, (int)Math.Ceiling(units));
            var bill = new List<string>();
            for (int i = 0; i < needed; i++) bill.Add("fuel");
            if (!_inventory.Inventory.TryConsumeBill(bill))
                return $"Not enough fuel aboard ({needed} needed).";
            SaveInventory();
            var r = _expeditions.RefuelVehicle(vehicleId, units);
            return r.IsSuccess ? $"Refueled {vehicleId}." : $"Cannot refuel {vehicleId}: {r.FailureCode}.";
        }

        private void SaveCombat()
        {
            if (_combat == null) return;
            if (CaptureSection("combat", CombatSaveStore.TryCapturePersisted(_combat.CaptureSave())))
            {
                _combatDirty = false;
                GD.Print("[Ashfall Godot] Combat save written.");
            }
        }

        private void FlushCombatIfDirty()
        {
            if (_combatDirty) SaveCombat();
        }

        /// <summary>
        /// Wire expedition travel encounters to spawn real combat: when an
        /// expedition triggers an encounter, populate a tactical combat at that
        /// location (if none is already active). This is the raiding/ambush
        /// hand-off from the travel loop into the Combat expansion.
        /// Plan 45: enemy composition is selected from the combat catalog by
        /// the location's danger band (EnemyCompositionSelector → binding
        /// matrix), so ambushes field warlord veterans on high ground and
        /// desperate scavengers on the safe roads instead of template raiders.
        /// </summary>
        private void SetupExpeditionCombatHandoff(CombatHostSession combat)
        {
            if (combat == null) return;
            SetupExpeditions();
            if (_expeditions == null) return;
            _expeditions.Engine.OnEncounterTriggered += state =>
            {
                if (_combat == null) return;
                var cs = _combat.Engine.State;
                bool idle = string.IsNullOrEmpty(cs.EncounterId) || cs.Resolved;
                if (!idle) return;
                var enemyIds = EnemyCompositionSelector.SelectAmbushComposition(
                    state.dangerLevel, CombatHostSession.DefaultAmbushEnemyCount);
                _combat.StartCombat(state.locationId, state.displayName, enemyCombatantIds: enemyIds);
                _combatDirty = true;
                GD.Print($"[Ashfall Godot] Expedition encounter at {state.locationId} spawned combat (danger {state.dangerLevel}: {string.Join(", ", enemyIds)}).");
            };

            // Plan 45 phase 2 — hostile travel-encounter choices escalate to
            // tactical combat: Creature encounters field the wildlife pack for
            // their combatant_tag, Human encounters a raid crew at high danger.
            _expeditions.OnTravelEncounterCombatTriggered += trigger =>
            {
                if (_combat == null || trigger == null) return;
                var cs = _combat.Engine.State;
                bool idle = string.IsNullOrEmpty(cs.EncounterId) || cs.Resolved;
                if (!idle) return;
                _combat.StartCombat(
                    string.IsNullOrEmpty(trigger.LocationId) ? "loc_wilds" : trigger.LocationId,
                    string.IsNullOrEmpty(trigger.Title) ? "Hostile Encounter" : trigger.Title,
                    enemyCombatantIds: trigger.CombatantIds);
                _combatDirty = true;
                GD.Print($"[Ashfall Godot] Travel encounter '{trigger.EncounterId}' escalated to combat: {string.Join(", ", trigger.CombatantIds)}.");
            };
        }

        private void OnExpeditionStartClicked(string locationId)
        {
            SetupExpeditions();
            _statusLabel.Text = _expeditions.StartExpedition("survivor_gunner_mikhail", locationId)
                + "\n" + _expeditions.StatusLine();
        }

        private void OnExpeditionTickClicked()
        {
            SetupExpeditions();
            _statusLabel.Text = _expeditions.TickHours(2f) + "\n" + _expeditions.StatusLine();
        }

        private void OnExpeditionDiveClicked()
        {
            SetupExpeditions();
            _statusLabel.Text = _expeditions.StartDive();
        }

        private void OnExpeditionAdvanceDiveClicked()
        {
            SetupExpeditions();
            _statusLabel.Text = _expeditions.AdvanceDive() + "\n" + _expeditions.DiveStatusLine();
        }

        private void SaveWastelandMap()
        {
            if (_world?.WastelandMap == null) return;
            try
            {
                CaptureSection("wasteland_map", WastelandMapSaveStore.TryCapturePersisted(_world.WastelandMap.CaptureState()));
            }
            catch (Exception e)
            {
                GD.PushWarning("[Ashfall Godot] WastelandMap save failed: " + e.Message);
            }
        }

        private void SaveEncounterChoice()
        {
            if (_encounterChoice == null || !_encounterChoiceDirty) return;
            try
            {
                if (CaptureSection("encounter_choice", EncounterChoiceSaveStore.TryCapturePersisted(_encounterChoice.CaptureState())))
                    _encounterChoiceDirty = false;
            }
            catch (Exception e)
            {
                GD.PushWarning("[Ashfall Godot] EncounterChoice save failed: " + e.Message);
            }
        }


        private void SetupEncounterChoiceResolver()
        {
            if (_encounterChoice != null) return;
            ApplyEncounterChoiceSaveIfAny();
        }

        private void ApplyEncounterChoiceSaveIfAny()
        {
            try
            {
                var saved = EncounterChoiceSaveStore.TryLoad();
                if (saved == null) return;
                _encounterChoice = new Ashfall.Core.Expeditions.EncounterChoiceResolver(saved);
            }
            catch (Exception e)
            {
                GD.PushWarning("[Ashfall Godot] EncounterChoice load failed: " + e.Message);
            }
        }

        private void SetupEncounterChoice()
        {
            if (_encounterChoice != null) return;
            _encounterChoice = new Ashfall.Core.Expeditions.EncounterChoiceResolver(
                new Ashfall.Core.Expeditions.EncounterChoiceState());
            _encounterChoice.OnResolved += _ => _encounterChoiceDirty = true;
        }

        private void CloseExpeditionPanel()
        {
            _expeditionPanel.Visible = false;
        }

        private void OnExpeditionEncounterSurfaced(ExpeditionEncounterBridge.EncounterSurfaced surfaced)
        {
            if (_expeditionPanel != null && _expeditionPanel.Visible)
                _expeditionPanel.ShowEncounterNotice(surfaced);
            // else: panel closed/headless — encounter surfaced without a diegetic surface.
        }

        private void CloseCombatPanel()
        {
            _combatPanel.Visible = false;
        }

        private void CloseMapPanel()
        {
            _mapPanel.Visible = false;
        }

        private void CloseCombatDetailPanel()
        {
            _combatDetailPanel.Visible = false;
        }

        private void CloseCombatHistoryPanel()
        {
            _combatHistoryPanel.Visible = false;
        }

        private void CloseMapDetailPanel()
        {
            _mapDetailPanel.Visible = false;
        }

        // Debounced flush hooks for systems that mutate each frame. They run
        // every tick and bail out unless the matching dirty flag is set.
        internal void FlushEncounterChoiceIfDirty()
        {
            if (!_encounterChoiceDirty) return;
            SaveEncounterChoice();
        }

    }
}
