using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.AI;
using AtomicWar._Game.AI.Actions;
using AtomicWar._Game.Crafting;
using AtomicWar._Game.Data;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Events;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Flashpoint;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Shelter.Modules;
using AtomicWar._Game.Simulation;
using AtomicWar._Game.UI;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.Core
{
    public partial class GameBootstrap
    {
        /// <summary>
        /// Pick the survivor to treat as the "leader" of the bunker for
        /// affinity bookkeeping. Defaults to the first living survivor
        /// (matches the convention used by MentalBreakSystem); falls back
        /// to the donor if they are the only living survivor.
        /// </summary>
        private Survivor ResolveBunkerLeader()
        {
            if (Survivors == null) return null;
            for (int i = 0; i < Survivors.Count; i++)
            {
                var s = Survivors[i];
                if (s != null && s.IsAlive) return s;
            }
            return null;
        }

        private Survivor FindFirstLivingSurvivor()
        {
            if (Survivors == null) return null;
            for (int i = 0; i < Survivors.Count; i++)
            {
                if (Survivors[i] != null && Survivors[i].IsAlive)
                    return Survivors[i];
            }
            return null;
        }

        /// <summary>
        /// Player API: open corpse dispose panel (bury / fertilizer) from inventory body slot.
        /// </summary>
        public bool OpenCorpseDisposePanel()
        {
            if (_hud == null) return false;
            RefreshInternalHorrorHud();
            var horror = _hud.EnsureInternalHorrorHud();
            if (horror == null || horror.CorpseCount <= 0) return false;
            horror.OpenCorpsePanel();
            return true;
        }

        /// <summary>Player API: choose bury or fertilizer on the open corpse panel.</summary>
        public bool SelectCorpseDispose(AtomicWar._Game.UI.CorpseDisposeChoice choice)
        {
            if (_hud == null) return false;
            var horror = _hud.EnsureInternalHorrorHud();
            return horror != null && horror.SelectCorpseDispose(choice);
        }

        /// <summary>Player API: fight fire in a room (or active fire room).</summary>
        public bool SelectFightFire(string roomId = null)
        {
            if (_hud == null) return false;
            var horror = _hud.EnsureInternalHorrorHud();
            return horror != null && horror.SelectFightFire(roomId);
        }

        /// <summary>Player API: seal bulkhead on a burning room.</summary>
        public bool SelectSealBulkhead(string roomId = null)
        {
            if (_hud == null) return false;
            var horror = _hud.EnsureInternalHorrorHud();
            return horror != null && horror.SelectSealBulkhead(roomId);
        }

        /// <summary>
        /// Simulate inventory strip click at index. Corpse icons open dispose panel.
        /// </summary>
        public bool ActivateInventoryIcon(int index)
        {
            if (_hud == null) return false;
            var strip = _hud.InventoryStripUI;
            return strip != null && strip.ActivateIndex(index);
        }

        /// <summary>Cycle inventory strip focus (keyboard path).</summary>
        public bool SelectNextInventoryIcon()
        {
            if (_hud == null) return false;
            var strip = _hud.InventoryStripUI;
            return strip != null && strip.SelectNext();
        }

        /// <summary>Confirm focused inventory icon (Enter/E). Corpses open dispose.</summary>
        public bool ActivateSelectedInventoryIcon()
        {
            if (_hud == null) return false;
            var strip = _hud.InventoryStripUI;
            return strip != null && strip.ActivateSelected();
        }

        /// <summary>Click first corpse stack in inventory (if any).</summary>
        public bool ActivateFirstCorpseInInventory()
        {
            if (_hud == null) return false;
            RefreshInventoryStrip();
            var strip = _hud.InventoryStripUI;
            return strip != null && strip.ActivateFirstCorpse();
        }

        /// <summary>True when corpse dispose panel is open (input priority).</summary>
        public bool IsCorpseDisposePanelOpen()
        {
            var horror = _hud != null ? _hud.InternalHorrorHUD : null;
            return horror != null && horror.IsCorpsePanelOpen;
        }

        /// <summary>True when fire fight/seal panel is open (input priority).</summary>
        public bool IsFirePanelOpen()
        {
            var horror = _hud != null ? _hud.InternalHorrorHUD : null;
            return horror != null && horror.IsFirePanelOpen;
        }

        /// <summary>
        /// Mental-break binge: consume highest-value food × multiplier from bunker stock.
        /// Hosted in Core so Survivors does not reference Inventory.
        /// </summary>
        private int ForceMentalBreakBingeEat(Survivor sv, MentalBreakSO br)
        {
            if (sv == null || br == null || Inventory == null || Inventory.Slots == null) return 0;
            if (!sv.IsAlive) return 0;

            InventorySlot best = null;
            float bestValue = float.NegativeInfinity;
            int scanned = 0;
            for (int i = 0; i < Inventory.Slots.Count && scanned < MentalBreakSystem.BingeEaterMaxSlotsScanned; i++)
            {
                var slot = Inventory.Slots[i];
                if (slot == null || slot.Item == null || slot.Amount <= 0) continue;
                if (slot.Item.type != ItemType.Food) continue;
                if (slot.Item.hungerRestore < br.minFoodValueForBinge) continue;
                if (slot.Item.hungerRestore > bestValue)
                {
                    best = slot;
                    bestValue = slot.Item.hungerRestore;
                }
                scanned++;
            }
            if (best == null) return 0;

            int wanted = Mathf.Max(1, Mathf.CeilToInt(br.consumptionMultiplier));
            int consumed = Mathf.Min(wanted, best.Amount);
            if (consumed <= 0) return 0;
            Inventory.Remove(best.Item, consumed);
            float restore = best.Item.hungerRestore * consumed;
            sv.Needs.Hunger = Mathf.Max(0f, sv.Needs.Hunger - restore);
            return consumed;
        }

        /// <summary>
        /// Mental-break comfort cure: pick a Comfort item from the
        /// inventory, consume one, and return true. Returns false if
        /// no Comfort item is available. Hosted in Core so Survivors
        /// does not reference Inventory.
        /// </summary>
        private bool ForceMentalBreakComfortCure(Survivor sv, MentalBreakSO br)
        {
            if (sv == null || br == null || Inventory == null || Inventory.Slots == null) return false;

            // Find a Comfort item (e.g. old_book, music_disc). Prefer the
            // one with the highest moraleRestore / sellValue as a stand-in
            // for "high-value".
            InventorySlot best = null;
            float bestValue = float.NegativeInfinity;
            for (int i = 0; i < Inventory.Slots.Count; i++)
            {
                var slot = Inventory.Slots[i];
                if (slot == null || slot.Item == null || slot.Amount <= 0) continue;
                if (slot.Item.type != ItemType.Comfort) continue;
                // Use tradeValue + moraleEffect as a high-value proxy.
                float value = slot.Item.tradeValue + slot.Item.moraleEffect;
                if (value > bestValue)
                {
                    best = slot;
                    bestValue = value;
                }
            }
            if (best == null || best.Item == null) return false;

            // Consume one unit of the comfort item. The system-side
            // TryCureWithComfortItem will then advance mentalBreakCureProgress
            // by br.comfortItemCureAmount and call Cure() if the threshold
            // is met.
            return Inventory.Remove(best.Item, 1);
        }

        private T CreateAction<T>() where T : SurvivorAction
        {
            var action = ScriptableObject.CreateInstance<T>();
            return action;
        }

        private bool TryApplyPedalCost(string id, float fatigueDelta, float hungerDelta)
        {
            if (Survivors == null || string.IsNullOrEmpty(id)) return false;
            Survivor pedaler = null;
            for (int i = 0; i < Survivors.Count; i++)
            {
                if (Survivors[i] != null && Survivors[i].Id == id)
                {
                    pedaler = Survivors[i];
                    break;
                }
            }
            if (pedaler == null || !pedaler.IsAlive || pedaler.Needs == null)
                return false;
            if (pedaler.Needs.Fatigue >= 95f)
                return false;
            pedaler.Needs.Fatigue = Mathf.Clamp(
                pedaler.Needs.Fatigue + fatigueDelta, 0f, 100f);
            pedaler.Needs.Hunger = Mathf.Clamp(
                pedaler.Needs.Hunger + hungerDelta, 0f, 100f);
            return true;
        }

        /// <summary>
        /// High-tier radio/antenna operational for inter-faction wiretaps.
        /// Requires powered radio with remaining fuel and EMP damage below destroy.
        /// </summary>
        private bool IsWiretapAntennaOperational()
        {
            var state = RadioTunerSystem?.State;
            return state != null && state.IsOperational;
        }

        /// <summary>
        /// Vehicle escape project: 50 mechanical_parts + 10 fuel + repaired engine.
        /// Explicit player action (not auto each frame).
        /// </summary>
        public bool TryVehicleEscape()
        {
            if (VictoryProject == null || Inventory == null) return false;
            int day = TimeSystem != null ? TimeSystem.CurrentDay : 1;
            var summary = VictoryProject.TryEscapeByVehicle(
                Inventory,
                id => _itemCatalog?.GetById(id) ?? MakeRuntimeItem(id),
                day,
                Survivors);
            return summary != null && summary.State == EndgameState.Escaped;
        }

        public bool StartScavengeMission(Survivor survivor, LocationDefinitionSO location)
        {
            if (ScavengingSystem == null || survivor == null || location == null) return false;
            return ScavengingSystem.StartMission(survivor, location);
        }

        public bool StartExpeditionMission(Survivor survivor, LocationDefinitionSO location, ExpeditionStance stance = ExpeditionStance.Stealth)
        {
            if (ExpeditionSystem == null || survivor == null || location == null) return false;
            return ExpeditionSystem.StartExpedition(survivor, location, stance);
        }

        /// <summary>Start expedition to a proc-gen map node (weather-scaled travel).</summary>
        public bool StartExpeditionToNode(Survivor survivor, string nodeId, ExpeditionStance stance = ExpeditionStance.Stealth)
        {
            if (ExpeditionSystem == null || survivor == null || GeneratedMap == null) return false;
            var node = GeneratedMap.GetNode(nodeId);
            if (node == null) return false;
            return ExpeditionSystem.StartExpedition(survivor, node, stance);
        }

        /// <summary>Execute a workbench line by 0-based index (keybinds 1-9).</summary>
        public bool ExecuteWorkbenchLine(int lineIndex)
        {
            return _hud?.WorkbenchUI != null && _hud.WorkbenchUI.Execute(lineIndex);
        }

        /// <summary>
        /// Open trade with a faction stockpile (UI). Hostile factions still open
        /// so the player can demand parley after a hatch repel.
        /// </summary>
        public bool OpenTrade(string factionId, Inventory.Inventory factionStock)
        {
            if (_hud?.TradeScreenUI == null || Inventory == null || factionStock == null)
                return false;
            return _hud.TradeScreenUI.Open(factionId, Inventory, factionStock);
        }

        /// <summary>
        /// Open trade using an ephemeral faction stock (created on first use).
        /// Used by the post-repel parley modal so the player need not hunt UI.
        /// </summary>
        public bool OpenTradeWithFaction(string factionId)
        {
            if (string.IsNullOrEmpty(factionId) || Inventory == null) return false;
            return OpenTrade(factionId, GetOrCreateFactionStock(factionId));
        }

        /// <summary>Demand parley / surrender on the open trade screen (keybind P).</summary>
        public bool DemandTradeParley()
        {
            return _hud?.TradeScreenUI != null && _hud.TradeScreenUI.TryDemandParley();
        }

        /// <summary>
        /// Demand parley for a faction. Opens trade when HUD is present so the
        /// strip shows STOOD DOWN; falls back to economy-only when headless.
        /// Used by the post-repel modal.
        /// </summary>
        public bool DemandParleyForFaction(string factionId)
        {
            if (EconomySystem == null || string.IsNullOrEmpty(factionId)) return false;
            if (OpenTradeWithFaction(factionId))
                return DemandTradeParley();
            return EconomySystem.DemandParley(factionId).Applied;
        }

        private Inventory.Inventory GetOrCreateFactionStock(string factionId)
        {
            if (_factionStocks.TryGetValue(factionId, out var existing) && existing != null)
                return existing;
            var stock = new Inventory.Inventory { Capacity = 40, MaxWeight = 200f };
            // Light seed stock so the screen is not empty after a stand-down.
            var water = _itemCatalog?.GetById("clean_water");
            var scrap = _itemCatalog?.GetById("scrap_metal");
            if (water != null) stock.Add(water, 2);
            if (scrap != null) stock.Add(scrap, 4);
            _factionStocks[factionId] = stock;
            return stock;
        }

        /// <summary>Send a survivor to survey a location with a working geiger.</summary>
        public bool StartSurveyMission(Survivor survivor, LocationDefinitionSO location)
        {
            if (ScavengingSystem == null || survivor == null || location == null) return false;
            bool started = ScavengingSystem.StartSurvey(survivor, location);
            if (started) RefreshMapKnowledgeHUD();
            return started;
        }

        /// <summary>
        /// AI/UI hook: survey the least-known location (unsurveyed first, then oldest measure).
        /// </summary>
        public bool RequestSurveyForSurvivor(Survivor survivor)
        {
            if (survivor == null || !survivor.IsAlive || ScavengingSystem == null) return false;
            if (Inventory == null || !Inventory.HasWorkingGeiger()) return false;
            if (_locationCatalog?.locations == null || _locationCatalog.locations.Count == 0) return false;

            LocationDefinitionSO best = null;
            int bestScore = int.MinValue;
            int day = TimeSystem != null ? TimeSystem.CurrentDay : 0;

            foreach (var loc in _locationCatalog.locations)
            {
                if (loc == null) continue;
                var tile = KnowledgeMap?.GetTile(loc.id);
                int score;
                if (tile == null || !tile.Surveyed) score = 1000;
                else score = day - tile.MeasuredAtDay;
                if (score > bestScore)
                {
                    bestScore = score;
                    best = loc;
                }
            }

            return best != null && StartSurveyMission(survivor, best);
        }

        private float GetMapUncertaintyFor(Survivor survivor)
        {
            if (KnowledgeMap == null || survivor == null) return 0.5f;

            bool hasWorkingGeiger = Inventory != null && Inventory.HasWorkingGeiger();
            int day = TimeSystem != null ? TimeSystem.CurrentDay : 0;

            if (ScavengingSystem != null)
            {
                foreach (var mission in ScavengingSystem.ActiveMissions)
                {
                    if (mission?.SurvivorId == survivor.Id)
                    {
                        var view = KnowledgeMap.GetPlayerView(mission.LocationId, day, hasWorkingGeiger);
                        return Mathf.Clamp01(1f - view.Confidence);
                    }
                }
            }

            float totalConfidence = 0f;
            int count = 0;
            foreach (var id in KnowledgeMap.Tiles.Keys)
            {
                var view = KnowledgeMap.GetPlayerView(id, day, hasWorkingGeiger);
                totalConfidence += view.Confidence;
                count++;
            }
            if (count == 0) return hasWorkingGeiger ? 0.5f : 1f;
            return Mathf.Clamp01(1f - (totalConfidence / count));
        }

        /// <summary>
        /// Average RadiationDose across living survivors (0..100). Used by
        /// DynamicEconomySystem for trust-inversion factions (Cult of the Glow).
        /// </summary>
        private float GetPartyAverageRadiationDose()
        {
            if (Survivors == null || Survivors.Count == 0) return 0f;
            float sum = 0f;
            int n = 0;
            for (int i = 0; i < Survivors.Count; i++)
            {
                var s = Survivors[i];
                if (s == null || !s.IsAlive) continue;
                sum += s.RadiationDose;
                n++;
            }
            return n > 0 ? sum / n : 0f;
        }

        /// <summary>
        /// True when any living survivor has Acute Radiation Syndrome (flag or status).
        /// Cult of the Glow ARS reverence (#16 polish).
        /// </summary>
        private bool PartyHasAcuteRadiationSyndrome()
        {
            if (Survivors == null) return false;
            for (int i = 0; i < Survivors.Count; i++)
            {
                var s = Survivors[i];
                if (s == null || !s.IsAlive) continue;
                if (s.HasAcuteRadiationSyndrome
                    || s.HasStatus(SurvivorStatus.AcuteRadiationSyndrome))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// True when any living survivor wears an intact full hazmat suit.
        /// Cult of the Glow sealed-blood contempt (#16 polish).
        /// </summary>
        private bool PartyWearsIntactHazmat()
        {
            if (Survivors == null) return false;
            for (int i = 0; i < Survivors.Count; i++)
            {
                var s = Survivors[i];
                if (s == null || !s.IsAlive) continue;
                if (s.HasFullSuitEquipped) return true;
            }
            // Fallback: equipped protective gear with remaining durability on shared inventory.
            if (Inventory != null && Inventory.GetEquippedProtection() > 0f)
                return true;
            return false;
        }

        private string FindActiveHatchDilemmaExpeditionId()
        {
            // Best-effort: walk the active expeditions and find the one in
            // AtHatchDilemma. If none, return empty (the resolve signal
            // is a no-op without an expeditionId).
            if (ExpeditionSystem == null || ExpeditionSystem.ActiveExpeditions == null) return string.Empty;
            for (int i = 0; i < ExpeditionSystem.ActiveExpeditions.Count; i++)
            {
                var e = ExpeditionSystem.ActiveExpeditions[i];
                if (e != null && e.Phase == ExpeditionPhase.AtHatchDilemma) return e.ExpeditionId;
            }
            return string.Empty;
        }

        /// <summary>True when the survivor is currently on an outdoor expedition (Black Rain exposure).</summary>
        private bool IsSurvivorOnExpedition(Survivor s)
        {
            if (s == null || ExpeditionSystem?.ActiveExpeditions == null) return false;
            for (int i = 0; i < ExpeditionSystem.ActiveExpeditions.Count; i++)
            {
                var e = ExpeditionSystem.ActiveExpeditions[i];
                if (e?.Survivor != null && e.Survivor.Id == s.Id) return true;
            }
            return false;
        }

        /// <summary>
        /// Black Rain hatch listeners: anyone in the entry room, or anyone
        /// underground while the hatch is sealed/open and rain is audible.
        /// Simplified: entry-room assignment OR hatch not Clear during BlackRain.
        /// </summary>
        private bool IsSurvivorHatchListener(Survivor s)
        {
            if (s == null || !s.IsAlive) return false;
            if (string.Equals(s.CurrentRoomId, HatchEntrapmentSystem.EntryRoomId, StringComparison.OrdinalIgnoreCase))
                return true;
            // Sealed hatch transmits the hammer of rain into the bunker.
            if (HatchEntrapmentSystem != null
                && HatchEntrapmentSystem.State != HatchState.Clear
                && BlackRainHazardSystem != null
                && BlackRainHazardSystem.IsActive)
            {
                return true;
            }
            return false;
        }

        private bool ForceAddictionPanicDestroy(Survivor sv, System.Random rng)
        {
            if (sv == null || Inventory?.Slots == null || rng == null) return false;
            if (Inventory.Slots.Count == 0) return false;

            // Destroy 1-3 random inventory items, each from a different slot
            int count = rng.Next(1, 4);
            bool destroyed = false;
            var targetedIndices = new System.Collections.Generic.HashSet<int>();
            for (int i = 0; i < count; i++)
            {
                if (!TryPickPanicDestroySlot(rng, targetedIndices, out int idx, out InventorySlot slot))
                    break;
                targetedIndices.Add(idx);
                int toRemove = rng.Next(1, Mathf.Min(slot.Amount, 3));
                if (!Inventory.Remove(slot.Item, toRemove)) continue;
                destroyed = true;
                Debug.Log($"[Addiction] {sv.DisplayName} destroyed {toRemove}x {slot.Item.id} in a withdrawal panic.");
            }
            return destroyed;
        }

        private bool TryPickPanicDestroySlot(
            System.Random rng,
            System.Collections.Generic.HashSet<int> targetedIndices,
            out int idx,
            out InventorySlot slot)
        {
            idx = -1;
            slot = null;
            for (int attempts = 0; attempts < 20; attempts++)
            {
                idx = rng.Next(0, Inventory.Slots.Count);
                slot = Inventory.Slots[idx];
                bool usable = slot?.Item != null && slot.Amount > 0 && !targetedIndices.Contains(idx);
                if (usable) return true;
            }
            slot = null;
            return false;
        }

        private float ComputeAiRaidThreat(int day)
        {
            if (HatchDefenseSystem == null || day < HatchDefenseSystem.RaidUnlockDay)
                return 0f;

            float raidThreat = 0.25f;
            if (HatchDefenseSystem.GeneratorRunningOutside
                || HatchDefenseSystem.ExternalNoise >= HatchDefenseSystem.ExternalGeneratorNoiseThreshold)
                raidThreat = 0.7f;

            if (EconomySystem == null) return raidThreat;
            foreach (var fac in EconomySystem.Factions.Values)
            {
                if (fac == null) continue;
                if (EconomySystem.GetStance(fac.id) != TradeStance.HostileRaid) continue;
                raidThreat = Mathf.Max(raidThreat, 0.85f);
            }
            return raidThreat;
        }
    }
}
