using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Data;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Drives scavenging and survey missions to LocationDefinitionSO sites.
    /// Exposure always uses TrueRad (sim truth). Player planning uses
    /// RadiationKnowledgeMap views — the dread of uncertain safety.
    /// </summary>
    public class LocationScavengingSystem
    {
        /// <summary>Hours spent taking readings on a survey mission (on top of travel).</summary>
        public const float SurveyHours = 1f;

        private readonly RadiationSystem _radSystem;
        private readonly Inventory.Inventory _inventory;
        // The composition root supplies the persistent bunker overflow stash. It is
        // deliberately separate from the field bag so a completed mission never
        // silently destroys an item simply because the player came home full.
        private readonly Inventory.Inventory _overflowStash;
        private readonly ItemCatalogSO _itemCatalog;
        private readonly RadiationKnowledgeMap _knowledge;
        private readonly Func<int> _getCurrentDay;
        private readonly System.Random _rng;
        private readonly List<ActiveMission> _active = new List<ActiveMission>();
        private readonly LootTableSO _lootTable;
        private readonly Func<WorldPhase> _getCurrentPhase;
        private ExpeditionPerkSystem _expeditionPerks;
        private Func<string, IList<string>> _getNodeTags;
        private Func<string, string> _getNodeRingName;
        private ItemDefinition _foragerRoots;
        private ItemDefinition _foragerBerries;
        private System.Random _foragerRng = new System.Random(210);
        private Item_AmmoTypes _ammoTypes;
        private Func<string, ItemDefinition> _ammoItemFactory;
        private Func<string, string> _getLocationLootTableId;
        private Func<string, ItemDefinition> _worldItemFactory;
        private bool _worldLootEnabled = true;

        public event Action<ActiveMission> OnMissionStarted;
        public event Action<ActiveMission, List<ItemDefinition>> OnMissionCompleted;
        /// <summary>
        /// Raised after a scavenging mission has placed every recoverable item in
        /// either the field bag or the persistent bunker overflow stash.
        /// </summary>
        public event Action<ScavengeAfterActionReport> OnScavengeAfterActionReady;
        public event Action<ActiveMission, bool> OnSurveyCompleted;

        /// <summary>Last exclusive ammo ids injected into a scavenging roll (tests).</summary>
        public List<string> LastInjectedAmmoIds { get; } = new List<string>();

        /// <summary>Last world-item / attachment ids injected into a scavenging roll (tests).</summary>
        public List<string> LastInjectedWorldLootIds { get; } = new List<string>();

        public LocationScavengingSystem(
            RadiationSystem radSystem,
            Inventory.Inventory inventory,
            ItemCatalogSO itemCatalog,
            int seed = 42,
            RadiationKnowledgeMap knowledge = null,
            Func<int> getCurrentDay = null,
            LootTableSO lootTable = null,
            Func<WorldPhase> getCurrentPhase = null,
            Inventory.Inventory overflowStash = null)
        {
            _radSystem = radSystem;
            _inventory = inventory;
            _overflowStash = overflowStash;
            _itemCatalog = itemCatalog;
            _knowledge = knowledge;
            _getCurrentDay = getCurrentDay ?? (() => 0);
            _rng = new System.Random(seed);
            _lootTable = lootTable;
            _getCurrentPhase = getCurrentPhase ?? (() => WorldPhase.CivilWar);
        }

        public IReadOnlyList<ActiveMission> ActiveMissions => _active;
        public RadiationKnowledgeMap Knowledge => _knowledge;
        /// <summary>Most recent completed scavenging report, retained for save/load and UI replay.</summary>
        public ScavengeAfterActionReport LastAfterActionReport { get; private set; }

        /// <summary>
        /// Prompts #208 / #210 — city surveys (Urban Pathfinder) and forest/swamp
        /// scavenges (Forager). Optional tag/ring resolvers for MapNode metadata.
        /// </summary>
        public void BindExpeditionPerks(
            ExpeditionPerkSystem expeditionPerks,
            Func<string, IList<string>> getNodeTags = null,
            Func<string, string> getNodeRingName = null)
        {
            _expeditionPerks = expeditionPerks;
            _getNodeTags = getNodeTags;
            _getNodeRingName = getNodeRingName;
            EnsureForagerFood();
        }

        /// <summary>
        /// Wire military/rebel exclusive ammo drops into high-danger / military loot tables.
        /// Workbench craft remains civilian-only via Item_AmmoTypes.IsWorkbenchCraftAllowed.
        /// </summary>
        public void BindAmmoTypes(
            Item_AmmoTypes ammoTypes,
            Func<string, ItemDefinition> ammoItemFactory = null,
            Func<string, string> getLocationLootTableId = null)
        {
            _ammoTypes = ammoTypes;
            _ammoItemFactory = ammoItemFactory;
            _getLocationLootTableId = getLocationLootTableId;
        }

        /// <summary>
        /// Wire faction world-item / attachment loot (attachments extremely rare loose).
        /// Uses <see cref="Item_WorldCatalog"/> pools; factory optional for SO lookup.
        /// </summary>
        public void BindWorldLoot(
            Func<string, ItemDefinition> worldItemFactory = null,
            Func<string, string> getLocationLootTableId = null,
            bool enabled = true)
        {
            _worldItemFactory = worldItemFactory;
            if (getLocationLootTableId != null)
                _getLocationLootTableId = getLocationLootTableId;
            _worldLootEnabled = enabled;
        }

        public void SetForagerFoodItems(ItemDefinition roots, ItemDefinition berries)
        {
            if (roots != null) _foragerRoots = roots;
            if (berries != null) _foragerBerries = berries;
        }

        /// <summary>Start a scavenging mission to a location. Returns false if survivor is dead or already on mission.</summary>
        public bool StartMission(Survivor survivor, LocationDefinitionSO location)
        {
            if (survivor == null || !survivor.IsAlive || location == null) return false;
            if (IsOnMission(survivor.Id)) return false;

            float trueRad = ResolveTrueRad(location);
            float exposurePerHour = ResolveMissionExposurePerHour(trueRad);

            var mission = new ActiveMission
            {
                SurvivorId = survivor.Id,
                LocationId = location.id,
                LocationName = location.displayName,
                HoursRemaining = location.travelHours,
                TotalHours = location.travelHours,
                AmbientRadPerHour = trueRad,
                RadPerHour = exposurePerHour,
                DangerLevel = location.dangerLevel,
                Kind = MissionKind.Scavenge,
                StartingRadiationDose = survivor.RadiationDose,
                Survivor = survivor
            };

            survivor.State = SurvivorState.Working;
            _active.Add(mission);
            OnMissionStarted?.Invoke(mission);
            return true;
        }

        /// <summary>
        /// Start a survey mission: travel + SurveyHours of readings with a working geiger.
        /// Fails if no working geiger is in inventory. Exposure uses the equipped-gear
        /// protected rate; the recorded ambient measurement is biased by device calibration.
        /// </summary>
        public bool StartSurvey(Survivor survivor, LocationDefinitionSO location)
        {
            if (survivor == null || !survivor.IsAlive || location == null) return false;
            if (IsOnMission(survivor.Id)) return false;
            if (_inventory == null || !_inventory.HasWorkingGeiger()) return false;

            float trueRad = ResolveTrueRad(location);
            float exposurePerHour = ResolveMissionExposurePerHour(trueRad);
            float hours = location.travelHours + SurveyHours;

            var mission = new ActiveMission
            {
                SurvivorId = survivor.Id,
                LocationId = location.id,
                LocationName = location.displayName,
                HoursRemaining = hours,
                TotalHours = hours,
                AmbientRadPerHour = trueRad,
                RadPerHour = exposurePerHour,
                DangerLevel = location.dangerLevel,
                Kind = MissionKind.Survey,
                StartingRadiationDose = survivor.RadiationDose,
                Survivor = survivor
            };

            survivor.State = SurvivorState.Working;
            _active.Add(mission);
            OnMissionStarted?.Invoke(mission);
            return true;
        }

        /// <summary>
        /// Immediate survey resolution for tests / same-tile bunker checks: no travel,
        /// just attempt a reading with the best working geiger.
        /// </summary>
        public bool TryImmediateSurvey(string locationId, out float measuredRad)
        {
            measuredRad = 0f;
            if (_knowledge == null || string.IsNullOrEmpty(locationId)) return false;
            if (_inventory == null) return false;

            var slot = _inventory.FindBestWorkingDevice("geiger_counter");
            if (slot?.Device == null) return false;

            float trueRad = _knowledge.GetTrueRad(locationId);
            if (!InstrumentDevice.TryRead(slot.Device, trueRad, out measuredRad)) return false;

            int day = _getCurrentDay();
            InstrumentDevice.DrainBattery(slot.Device, InstrumentDevice.BatteryDrainPerSurvey);
            _knowledge.RecordSurvey(locationId, measuredRad, slot.Device.Calibration, day);
            return true;
        }

        /// <summary>Advance active missions over elapsed game hours.</summary>
        public void Tick(float gameHours)
        {
            if (gameHours <= 0f) return;

            for (int i = _active.Count - 1; i >= 0; i--)
            {
                var mission = _active[i];
                // Cap to time actually spent on the mission this tick: any
                // leftover tick budget is "after" the mission ends, so neither
                // the rad exposure nor the countdown should claim it. Without
                // this, a tick larger than HoursRemaining would leave
                // HoursRemaining negative until the loop completes the mission
                // on the same pass, which is fine for behaviour but produces
                // an inconsistent state observable through the public field
                // (and the SaveState round-trip).
                float elapsed = Mathf.Min(gameHours, Mathf.Max(0f, mission.HoursRemaining));
                mission.HoursRemaining -= elapsed;

                // Accumulate radiation during travel/survey — always TrueRad
                if (_radSystem != null && mission.Survivor != null && mission.Survivor.IsAlive
                    && elapsed > 0f)
                {
                    _radSystem.Expose(mission.Survivor, mission.RadPerHour, elapsed);
                }

                if (mission.HoursRemaining <= 0f)
                {
                    // Remove before completing: CompleteMission/CompleteSurvey fire
                    // OnMissionCompleted/OnSurveyCompleted synchronously, and a
                    // subscriber reading ActiveMissions during that callback (e.g.
                    // the scavenge dispatch HUD's block-reason check) must already
                    // see the mission gone, or the just-finished survivor reads as
                    // "already scavenging" for one refresh cycle.
                    _active.RemoveAt(i);
                    if (mission.Kind == MissionKind.Survey)
                    {
                        CompleteSurvey(mission);
                    }
                    else
                    {
                        CompleteMission(mission);
                    }
                }
            }
        }

        private void CompleteMission(ActiveMission mission)
        {
            if (mission.Survivor != null)
                mission.Survivor.State = SurvivorState.Idle;

            var loot = RollLoot(mission.DangerLevel, mission.LocationId);

            // Prompt #210 — forest/swamp scavenge milestone + empty-loot forager food.
            ApplyForagerOnScavenge(mission, loot);

            var report = new ScavengeAfterActionReport
            {
                SurvivorId = mission.SurvivorId,
                SurvivorName = mission.Survivor != null && !string.IsNullOrEmpty(mission.Survivor.DisplayName)
                    ? mission.Survivor.DisplayName
                    : mission.SurvivorId,
                LocationId = mission.LocationId,
                LocationName = mission.LocationName,
                StartingRadiationDose = mission.StartingRadiationDose,
                EndingRadiationDose = mission.Survivor != null ? mission.Survivor.RadiationDose : mission.StartingRadiationDose,
                FaceGear = CaptureGearCondition(EquipSlot.Face),
                BodyGear = CaptureGearCondition(EquipSlot.Body)
            };

            if (loot != null)
            {
                for (int i = 0; i < loot.Count; i++)
                {
                    var item = loot[i];
                    if (item == null) continue;

                    if (_inventory != null && _inventory.Add(item, 1))
                    {
                        report.RecoveredItemIds.Add(item.id);
                    }
                    else if (_overflowStash != null && _overflowStash.Add(item, 1))
                    {
                        report.OverflowItemIds.Add(item.id);
                    }
                    else
                    {
                        // A partial/test host may not wire a stash. Keep this
                        // visible rather than falsely reporting a recovered item.
                        report.UnsecuredItemIds.Add(item.id);
                    }
                }
            }

            report.RadiationGained = Mathf.Max(0f, report.EndingRadiationDose - report.StartingRadiationDose);
            LastAfterActionReport = report;
            OnMissionCompleted?.Invoke(mission, loot);
            OnScavengeAfterActionReady?.Invoke(report);
        }

        private ScavengeGearCondition CaptureGearCondition(EquipSlot slot)
        {
            var equipped = _inventory != null ? _inventory.GetEquipped(slot) : null;
            if (equipped?.Item == null)
                return new ScavengeGearCondition { Slot = slot.ToString().ToLowerInvariant() };

            float durabilityPercent = equipped.Item.durability > 0f
                ? Mathf.Clamp01(equipped.CurrentDurability / equipped.Item.durability) * 100f
                : -1f;
            return new ScavengeGearCondition
            {
                Slot = slot.ToString().ToLowerInvariant(),
                ItemId = equipped.Item.id,
                ItemName = string.IsNullOrEmpty(equipped.Item.displayName) ? equipped.Item.id : equipped.Item.displayName,
                DurabilityPercent = durabilityPercent
            };
        }

        private void CompleteSurvey(ActiveMission mission)
        {
            if (mission.Survivor != null)
                mission.Survivor.State = SurvivorState.Idle;

            bool success = false;
            if (_inventory != null && _knowledge != null)
            {
                var slot = _inventory.FindBestWorkingDevice("geiger_counter");
                if (slot?.Device != null
                    && InstrumentDevice.TryRead(slot.Device, mission.AmbientRadPerHour, out float measured))
                {
                    int day = _getCurrentDay();
                    InstrumentDevice.DrainBattery(slot.Device, InstrumentDevice.BatteryDrainPerSurvey);
                    success = _knowledge.RecordSurvey(
                        mission.LocationId, measured, slot.Device.Calibration, day);
                }
            }

            // Prompt #208 — fully surveying a City node counts toward Urban Pathfinder.
            if (success && _expeditionPerks != null && mission.Survivor != null)
            {
                var tags = _getNodeTags?.Invoke(mission.LocationId);
                string ring = _getNodeRingName?.Invoke(mission.LocationId);
                bool isCity = ExpeditionPerkSystem.IsCityOrRuinNode(tags, ring)
                              || (!string.IsNullOrEmpty(mission.LocationId)
                                  && mission.LocationId.IndexOf("city", StringComparison.OrdinalIgnoreCase) >= 0)
                              || (!string.IsNullOrEmpty(mission.LocationName)
                                  && mission.LocationName.IndexOf("city", StringComparison.OrdinalIgnoreCase) >= 0);
                // Only city (not pure ruin) for the earn condition "City Map Nodes".
                bool isCityEarn = ExpeditionPerkSystem.IsCityOrRuinTags(tags)
                                  || (!string.IsNullOrEmpty(ring)
                                      && ring.IndexOf("city", StringComparison.OrdinalIgnoreCase) >= 0)
                                  || (!string.IsNullOrEmpty(ring)
                                      && ring.IndexOf("outskirt", StringComparison.OrdinalIgnoreCase) >= 0)
                                  || (!string.IsNullOrEmpty(mission.LocationId)
                                      && (mission.LocationId.IndexOf("city", StringComparison.OrdinalIgnoreCase) >= 0
                                          || mission.LocationId.IndexOf("outskirt", StringComparison.OrdinalIgnoreCase) >= 0));
                if (isCityEarn)
                    _expeditionPerks.RecordCityNodeSurvey(mission.Survivor, _getCurrentDay());
            }

            // Survey still returns empty loot list for API symmetry
            OnMissionCompleted?.Invoke(mission, new List<ItemDefinition>());
            OnSurveyCompleted?.Invoke(mission, success);
        }

        private void ApplyForagerOnScavenge(ActiveMission mission, List<ItemDefinition> loot)
        {
            if (_expeditionPerks == null || mission?.Survivor == null || loot == null) return;

            var tags = _getNodeTags?.Invoke(mission.LocationId);
            bool forestSwamp = ExpeditionPerkSystem.IsForestOrSwampTags(tags)
                               || (!string.IsNullOrEmpty(mission.LocationId)
                                   && (mission.LocationId.IndexOf("forest", StringComparison.OrdinalIgnoreCase) >= 0
                                       || mission.LocationId.IndexOf("swamp", StringComparison.OrdinalIgnoreCase) >= 0));

            if (forestSwamp)
                _expeditionPerks.RecordForestOrSwampScavenge(mission.Survivor, _getCurrentDay());

            int add = _expeditionPerks.GetForagerGuaranteedFoodCount(
                mission.Survivor, loot.Count, _foragerRng);
            if (add <= 0) return;

            EnsureForagerFood();
            for (int i = 0; i < add; i++)
            {
                string id = ExpeditionPerkSystem.PickForagerFoodId(_foragerRng);
                var item = string.Equals(id, ExpeditionPerkSystem.BerriesItemId, StringComparison.OrdinalIgnoreCase)
                    ? _foragerBerries
                    : _foragerRoots;
                if (item != null) loot.Add(item);
            }
        }

        private void EnsureForagerFood()
        {
            if (_foragerRoots == null)
            {
                _foragerRoots = ScriptableObject.CreateInstance<ItemDefinition>();
                _foragerRoots.id = ExpeditionPerkSystem.RootsItemId;
                _foragerRoots.displayName = "Roots";
                _foragerRoots.description = "Low-nutrition forage. Staves off starvation.";
                _foragerRoots.type = ItemType.Food;
                _foragerRoots.weight = 0.2f;
                _foragerRoots.stackMax = 20;
                _foragerRoots.hungerRestore = 8f;
                _foragerRoots.tradeValue = 1f;
            }
            if (_foragerBerries == null)
            {
                _foragerBerries = ScriptableObject.CreateInstance<ItemDefinition>();
                _foragerBerries.id = ExpeditionPerkSystem.BerriesItemId;
                _foragerBerries.displayName = "Berries";
                _foragerBerries.description = "Low-nutrition forage. Staves off starvation.";
                _foragerBerries.type = ItemType.Food;
                _foragerBerries.weight = 0.15f;
                _foragerBerries.stackMax = 20;
                _foragerBerries.hungerRestore = 6f;
                _foragerBerries.tradeValue = 1f;
            }
        }

        private float ResolveTrueRad(LocationDefinitionSO location)
        {
            if (_knowledge != null)
            {
                var tile = _knowledge.GetTile(location.id);
                if (tile != null) return tile.TrueRad;
            }
            return location.baseRadsPerHour;
        }

        /// <summary>
        /// Location missions use the same equipped-gear protection calculation as
        /// the field exposure context. Ambient radiation is retained separately
        /// for instruments and save-state inspection; RadPerHour is the actual
        /// dose rate applied to the survivor while this mission is active.
        /// </summary>
        private float ResolveMissionExposurePerHour(float ambientRadPerHour)
        {
            float protection = _inventory != null ? _inventory.GetEquippedProtection() : 0f;
            return RadiationSystem.ComputeExposurePerHour(ambientRadPerHour, protection, 0f);
        }

        private bool IsOnMission(string survivorId)
        {
            for (int i = 0; i < _active.Count; i++)
            {
                if (_active[i].SurvivorId == survivorId) return true;
            }
            return false;
        }

        /// <summary>
        /// Shared search-slot-count / per-slot-recovery-chance curve. This is
        /// the single source of truth for that math: RollLoot rolls against
        /// it directly, and GameBootstrap's dispatch-board preview text reads
        /// it through the same call, so a balance-pass edit here can never
        /// leave the preview quietly out of sync with what missions actually
        /// resolve.
        /// </summary>
        public static void PreviewLootChances(float dangerLevel, out int searchSlots, out float recoveryChancePercent)
        {
            searchSlots = Mathf.Clamp(1 + Mathf.FloorToInt(dangerLevel / 3f), 1, 4);
            recoveryChancePercent = Mathf.Clamp01(0.6f + dangerLevel * 0.03f) * 100f;
        }

        private List<ItemDefinition> RollLoot(float dangerLevel, string locationId)
        {
            var loot = new List<ItemDefinition>();
            LastInjectedAmmoIds.Clear();
            LastInjectedWorldLootIds.Clear();

            PreviewLootChances(dangerLevel, out int itemCount, out float recoveryChancePercent);
            float perSlotChance = recoveryChancePercent / 100f;

            // Military sites / high danger: inject exclusive ammo before generic rolls.
            TryInjectMilitaryAmmo(loot, dangerLevel, locationId);
            // Faction world gear + extremely rare loose attachments.
            TryInjectFactionWorldLoot(loot, dangerLevel, locationId);

            if (_lootTable != null)
            {
                var entries = _lootTable.GetValidEntries(_getCurrentPhase());
                if (entries.Count == 0) return loot;

                float totalWeight = 0f;
                for (int i = 0; i < entries.Count; i++) totalWeight += Mathf.Max(0f, entries[i].weight);
                if (totalWeight <= 0f) return loot;

                for (int i = 0; i < itemCount; i++)
                {
                    if (_rng.NextDouble() >= perSlotChance) continue;

                    double roll = _rng.NextDouble() * totalWeight;
                    for (int e = 0; e < entries.Count; e++)
                    {
                        roll -= Mathf.Max(0f, entries[e].weight);
                        if (roll <= 0f)
                        {
                            loot.Add(entries[e].item);
                            break;
                        }
                    }
                }

                return loot;
            }

            if (_itemCatalog == null || _itemCatalog.items == null || _itemCatalog.items.Count == 0) return loot;

            for (int i = 0; i < itemCount; i++)
            {
                if (_rng.NextDouble() < perSlotChance)
                {
                    var item = _itemCatalog.items[_rng.Next(_itemCatalog.items.Count)];
                    if (item != null)
                    {
                        loot.Add(item);
                    }
                }
            }

            return loot;
        }

        private void TryInjectMilitaryAmmo(List<ItemDefinition> loot, float dangerLevel, string locationId)
        {
            if (_ammoTypes == null || loot == null) return;

            string tableId = null;
            if (_getLocationLootTableId != null && !string.IsNullOrEmpty(locationId))
                tableId = _getLocationLootTableId(locationId);

            bool military = Item_AmmoTypes.IsMilitaryLootTable(tableId) || dangerLevel >= 4f;
            if (!military) return;

            float chance = 0.35f + dangerLevel * 0.06f;
            if (_rng.NextDouble() >= Mathf.Clamp01(chance)) return;

            var source = Item_AmmoTypes.SourceForLootTable(tableId);
            if (!Item_AmmoTypes.IsMilitaryOrRebelSource(source))
                source = AmmoFactionSource.MilitaryForces;

            int count = dangerLevel >= 5f ? 2 : 1;
            var ids = Item_AmmoTypes.RollFactionAmmoLoot(source, _rng, count, preferApApi: true);
            for (int i = 0; i < ids.Count; i++)
            {
                var def = ResolveAmmoDef(ids[i]);
                if (def == null) continue;
                loot.Add(def);
                LastInjectedAmmoIds.Add(ids[i]);
            }
        }

        /// <summary>
        /// Inject faction world gear on military/bandit/insurgent tables and high-danger sites.
        /// Loose attachments use extremely low chance (usually already on guns).
        /// </summary>
        private void TryInjectFactionWorldLoot(List<ItemDefinition> loot, float dangerLevel, string locationId)
        {
            if (!_worldLootEnabled || loot == null) return;

            string tableId = null;
            if (_getLocationLootTableId != null && !string.IsNullOrEmpty(locationId))
                tableId = _getLocationLootTableId(locationId);

            bool factionSite = Item_WorldCatalog.IsFactionGearLootTable(tableId)
                || Item_AmmoTypes.IsMilitaryLootTable(tableId)
                || dangerLevel >= 3.5f;
            if (!factionSite) return;

            float chance = Item_WorldCatalog.IsFactionGearLootTable(tableId)
                ? 0.40f + dangerLevel * 0.05f
                : 0.15f + dangerLevel * 0.04f;
            if (_rng.NextDouble() >= Mathf.Clamp01(chance)) return;

            var faction = Item_WorldCatalog.SourceForLootTable(tableId);
            if (faction == WorldLootFaction.Civilian && dangerLevel >= 4f)
                faction = WorldLootFaction.Military;
            else if (faction == WorldLootFaction.Civilian && dangerLevel >= 3.5f)
                faction = WorldLootFaction.Bandit;

            int count = dangerLevel >= 5f ? 2 : 1;
            var rolls = Item_WorldCatalog.RollFactionWorldLoot(
                faction, _rng, count, allowAttachments: true, dangerLevel: dangerLevel);
            for (int i = 0; i < rolls.Count; i++)
            {
                var roll = rolls[i];
                var def = ResolveWorldItemDef(roll.ItemId);
                if (def == null) continue;
                loot.Add(def);
                LastInjectedWorldLootIds.Add(roll.ItemId);
            }
        }

        private ItemDefinition ResolveAmmoDef(string ammoId)
        {
            if (string.IsNullOrEmpty(ammoId)) return null;
            if (_ammoItemFactory != null)
            {
                var made = _ammoItemFactory(ammoId);
                if (made != null) return made;
            }
            if (_itemCatalog?.items != null)
            {
                for (int i = 0; i < _itemCatalog.items.Count; i++)
                {
                    var it = _itemCatalog.items[i];
                    if (it != null && string.Equals(it.id, ammoId, StringComparison.Ordinal))
                        return it;
                }
            }
            if (!Item_AmmoTypes.TryGetLoad(ammoId, out var load)) return null;
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = load.Id;
            item.displayName = load.DisplayName;
            item.description = load.Description;
            item.type = ItemType.Weapon;
            item.stackMax = load.StackMax;
            item.weight = load.WeightPerRound;
            item.tradeValue = load.TradeValue;
            return item;
        }

        private ItemDefinition ResolveWorldItemDef(string itemId)
        {
            if (string.IsNullOrEmpty(itemId)) return null;
            if (_worldItemFactory != null)
            {
                var made = _worldItemFactory(itemId);
                if (made != null) return made;
            }
            if (_itemCatalog?.items != null)
            {
                for (int i = 0; i < _itemCatalog.items.Count; i++)
                {
                    var it = _itemCatalog.items[i];
                    if (it != null && string.Equals(it.id, itemId, StringComparison.Ordinal))
                        return it;
                }
            }
            return Item_WorldCatalog.CreateItemDefinition(itemId);
        }

        // -----------------------------------------------------------------
        // Save / Load (audit wiring fix)
        // ActiveMission.Survivor is [NonSerialized] — we save SurvivorId instead
        // and restore it after load via the survivor lookup.
        // -----------------------------------------------------------------
        private Func<string, Survivor> _survivorLookup;
        public void SetSurvivorLookup(Func<string, Survivor> lookup) => _survivorLookup = lookup;

        public ScavengingSystemSave CaptureState()
        {
            var missions = new ActiveMissionSave[_active.Count];
            for (int i = 0; i < _active.Count; i++)
            {
                var m = _active[i];
                missions[i] = new ActiveMissionSave
                {
                    SurvivorId = m.SurvivorId,
                    LocationId = m.LocationId,
                    LocationName = m.LocationName,
                    HoursRemaining = m.HoursRemaining,
                    TotalHours = m.TotalHours,
                    AmbientRadPerHour = m.AmbientRadPerHour,
                    RadPerHour = m.RadPerHour,
                    DangerLevel = m.DangerLevel,
                    StartingRadiationDose = m.StartingRadiationDose,
                    Kind = (int)m.Kind
                };
            }
            return new ScavengingSystemSave
            {
                ActiveMissions = missions,
                LastAfterActionReport = LastAfterActionReport
            };
        }

        public void RestoreState(ScavengingSystemSave save)
        {
            _active.Clear();
            LastAfterActionReport = save?.LastAfterActionReport;
            if (save?.ActiveMissions == null) return;
            for (int i = 0; i < save.ActiveMissions.Length; i++)
            {
                var sm = save.ActiveMissions[i];
                if (sm == null || string.IsNullOrEmpty(sm.SurvivorId)) continue;
                var sv = _survivorLookup?.Invoke(sm.SurvivorId);
                var mission = new ActiveMission
                {
                    SurvivorId = sm.SurvivorId,
                    LocationId = sm.LocationId,
                    LocationName = sm.LocationName,
                    HoursRemaining = sm.HoursRemaining,
                    TotalHours = sm.TotalHours,
                    // Older saves did not retain ambient separately, so their
                    // saved applied rate remains the best available reading.
                    AmbientRadPerHour = sm.AmbientRadPerHour > 0f
                        ? sm.AmbientRadPerHour
                        : sm.RadPerHour,
                    RadPerHour = sm.RadPerHour,
                    DangerLevel = sm.DangerLevel,
                    StartingRadiationDose = sm.StartingRadiationDose,
                    Kind = (MissionKind)sm.Kind,
                    Survivor = sv
                };
                _active.Add(mission);
            }
        }
    }

    [Serializable]
    public class ScavengingSystemSave
    {
        public ActiveMissionSave[] ActiveMissions;
        public ScavengeAfterActionReport LastAfterActionReport;
    }

    [Serializable]
    public class ActiveMissionSave
    {
        public string SurvivorId;
        public string LocationId;
        public string LocationName;
        public float HoursRemaining;
        public float TotalHours;
        public float AmbientRadPerHour;
        public float RadPerHour;
        public float DangerLevel;
        public float StartingRadiationDose;
        public int Kind;
    }

    public enum MissionKind
    {
        Scavenge,
        Survey
    }

    [Serializable]
    public class ActiveMission
    {
        public string SurvivorId;
        public string LocationId;
        public string LocationName;
        public float HoursRemaining;
        public float TotalHours;
        /// <summary>Unmitigated location radiation for instruments and audit UI.</summary>
        public float AmbientRadPerHour;
        /// <summary>Mitigated rate actually applied to the survivor during this mission.</summary>
        public float RadPerHour;
        public float DangerLevel;
        /// <summary>Dose at dispatch; retained so the return report is correct after a save/load.</summary>
        public float StartingRadiationDose;
        public MissionKind Kind;
        [NonSerialized] public Survivor Survivor;
    }

    /// <summary>
    /// Save-safe fact record for a completed scavenging mission. Item definitions
    /// are represented by ids so this report never serializes ScriptableObject
    /// references and can be replayed by UI after loading a save.
    /// </summary>
    [Serializable]
    public class ScavengeAfterActionReport
    {
        public string SurvivorId;
        public string SurvivorName;
        public string LocationId;
        public string LocationName;
        public List<string> RecoveredItemIds = new List<string>();
        public List<string> OverflowItemIds = new List<string>();
        public List<string> UnsecuredItemIds = new List<string>();
        public float StartingRadiationDose;
        public float EndingRadiationDose;
        public float RadiationGained;
        public ScavengeGearCondition FaceGear;
        public ScavengeGearCondition BodyGear;
    }

    /// <summary>Equipped item state captured at mission return for the after-action report.</summary>
    [Serializable]
    public class ScavengeGearCondition
    {
        public string Slot;
        public string ItemId;
        public string ItemName;
        /// <summary>-1 when the equipped item has no durability resource.</summary>
        public float DurabilityPercent = -1f;
    }
}
