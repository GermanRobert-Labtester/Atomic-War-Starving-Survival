using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Data;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Events;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Core
{
    public partial class ExpeditionSystem
    {
        /// <summary>
        /// Day-30 intercept: sever comms on every active expedition, apply
        /// trauma + acute-dose spike, resolve the survivor's trait-driven
        /// behavior. Idempotent per expedition: re-firing on the same
        /// signal is a no-op (the first call sets the behavior).
        /// </summary>

        private ExpeditionState FindExpeditionById(string expeditionId)
        {
            if (string.IsNullOrEmpty(expeditionId)) return null;
            for (int i = 0; i < _activeExpeditions.Count; i++)
            {
                if (_activeExpeditions[i] != null && _activeExpeditions[i].ExpeditionId == expeditionId)
                    return _activeExpeditions[i];
            }
            return null;
        }

        private void RemoveExpedition(ExpeditionState exp)
        {
            _activeExpeditions.Remove(exp);
        }

        /// <summary>
        /// Advances phase logic. Returns true when the caller should skip the
        /// default encounter roll / tick event for this expedition index.
        /// </summary>
        private float CalculateStaminaDrain(ExpeditionState exp, float hours)
        {
            float drain = BaseStaminaDrainPerHour * hours;

            // Carry weight penalty: up to +15/hr at full capacity
            // Prompt #206 — Pack Mule halves the over-encumbrance portion.
            float loadRatio = exp.CarryingCapacity > 0f ? Mathf.Clamp01(exp.CurrentWeight / exp.CarryingCapacity) : 0f;
            float encumberPenalty = loadRatio * 15f * hours;
            if (_expeditionPerks != null && exp.Survivor != null)
                encumberPenalty *= _expeditionPerks.GetOverEncumberPenaltyMultiplier(exp.Survivor);
            drain += encumberPenalty;

            // Weather penalty
            if (_weatherSystem != null)
            {
                if (_weatherSystem.Current == WeatherKind.Blizzard
                    || _weatherSystem.Current == WeatherKind.FalloutStorm
                    || _weatherSystem.Current == WeatherKind.BlackRain)
                {
                    drain += 10f * hours;
                }
            }

            // Suit wear & degradation (Black Rain melts hazmat aggressively — Prompt #11)
            if (exp.Survivor.HasFullSuitEquipped)
            {
                float suitWearPerHour = 2f;
                if (_weatherSystem != null)
                    suitWearPerHour *= _weatherSystem.HazmatDegradeMultiplier;
                exp.SuitDegradation = Mathf.Clamp(exp.SuitDegradation + suitWearPerHour * hours, 0f, 100f);
                drain += 3f * hours; // suit heat & movement restriction
            }

            // Limp disability: permanently doubles stamina drain during expeditions
            if (exp.Survivor != null && exp.Survivor.HasDisability("limp"))
            {
                drain *= 2f;
            }

            // Prompt #222 — Juggernaut: no encumbrance stamina penalty.
            if (_personalQuests != null && exp.Survivor != null
                && _personalQuests.IgnoresEncumbrance(exp.Survivor))
            {
                drain = BaseStaminaDrainPerHour * hours;
                if (exp.Survivor.HasFullSuitEquipped)
                    drain += 3f * hours;
                if (exp.Survivor.HasDisability("limp"))
                    drain *= 2f;
            }

            // Prompt #224 — Survivalist: 75% less stamina drain when alone on map.
            if (_personalQuests != null && exp.Survivor != null)
            {
                bool alone = _activeExpeditions == null || _activeExpeditions.Count <= 1;
                drain *= _personalQuests.GetAloneStaminaDrainMultiplier(exp.Survivor, alone);
            }

            return drain;
        }

        private void PerformLootRoll(ExpeditionState exp)
        {
            // Prompt #207 — Light Step scavengers do not raise scavenging noise.
            TryRaiseScavengeNoise(exp);

            if (_itemCatalog == null || _itemCatalog.items == null || _itemCatalog.items.Count == 0)
            {
                // Empty catalog still allows Forager guaranteed food.
                TryApplyForagerLoot(exp);
                return;
            }

            float chance = 0.5f + (exp.DangerLevel * 0.05f);
            // Prompt #69 — flooded nodes pay more (high risk, high reward).
            float floodMult = _floodedNodeSystem != null
                ? _floodedNodeSystem.GetLootMultiplier(exp.TargetLocationId)
                : 1f;
            chance = Mathf.Min(0.95f, chance * floodMult);

            int before = exp.CollectedLoot != null ? exp.CollectedLoot.Count : 0;
            int rolls = floodMult > 1.5f ? 2 : 1;
            for (int r = 0; r < rolls; r++)
            {
                if (_rng.NextDouble() >= chance) continue;
                TryAddLootItem(exp);
            }

            int after = exp.CollectedLoot != null ? exp.CollectedLoot.Count : 0;
            if (after <= before)
                TryApplyForagerLoot(exp);
        }

        /// <summary>Prompt #207 — scavenging makes noise unless Light Step.</summary>
        private void TryRaiseScavengeNoise(ExpeditionState exp)
        {
            if (_noiseSystem == null || exp?.Survivor == null) return;
            if (_expeditionPerks != null && _expeditionPerks.SuppressesScavengeNoise(exp.Survivor))
                return;
            bool storm = _isStormActive != null && _isStormActive();
            _noiseSystem.AddNoise(0.35f, storm);
        }

        /// <summary>
        /// Prompt #210 — Forager: empty loot still yields 1–2 Roots or Berries.
        /// Milestone counting for Forest/Swamp is handled on expedition complete.
        /// </summary>
        private void TryApplyForagerLoot(ExpeditionState exp)
        {
            if (exp?.Survivor == null) return;

            // Prompt #223 — Apex Predator: 50 Meat on forest/swamp, no loot roll needed.
            if (_personalQuests != null)
            {
                bool forestSwamp = IsForestOrSwampNode(exp.TargetLocationId);
                int meat = _personalQuests.GetApexPredatorMeatYield(exp.Survivor, forestSwamp);
                if (meat > 0)
                {
                    EnsureApexMeatItem();
                    if (_apexMeat != null)
                    {
                        for (int i = 0; i < meat; i++)
                            exp.TryAddLoot(_apexMeat);
                    }
                }
            }

            if (_expeditionPerks == null) return;
            if (exp.ForagerLootApplied) return;

            int existing = exp.CollectedLoot != null ? exp.CollectedLoot.Count : 0;
            int count = _expeditionPerks.GetForagerGuaranteedFoodCount(exp.Survivor, existing, _rng);
            if (count <= 0) return;

            EnsureForagerFoodItems();
            for (int i = 0; i < count; i++)
            {
                string id = ExpeditionPerkSystem.PickForagerFoodId(_rng);
                var item = string.Equals(id, ExpeditionPerkSystem.BerriesItemId, StringComparison.OrdinalIgnoreCase)
                    ? _foragerBerries
                    : _foragerRoots;
                if (item != null)
                    exp.TryAddLoot(item);
            }
            exp.ForagerLootApplied = true;
        }

        private bool IsForestOrSwampNode(string nodeId)
        {
            if (string.IsNullOrEmpty(nodeId) || _generatedMap == null) return false;
            var node = _generatedMap.GetNode(nodeId);
            if (node?.Tags == null) return false;
            for (int i = 0; i < node.Tags.Count; i++)
            {
                string t = node.Tags[i];
                if (string.Equals(t, ExpeditionPerkSystem.TagForest, StringComparison.OrdinalIgnoreCase)
                    || string.Equals(t, ExpeditionPerkSystem.TagSwamp, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        private void EnsureApexMeatItem()
        {
            if (_apexMeat != null) return;
            _apexMeat = ScriptableObject.CreateInstance<ItemDefinition>();
            _apexMeat.id = PersonalQuestSystem.MeatItemId;
            _apexMeat.displayName = "Meat";
            _apexMeat.description = "Field-dressed game. Heavy, honest calories.";
            _apexMeat.type = ItemType.Food;
            _apexMeat.weight = 0.5f;
            _apexMeat.stackMax = 99;
            _apexMeat.hungerRestore = 20f;
            _apexMeat.tradeValue = 3f;
        }

        private void EnsureForagerFoodItems()
        {
            if (_foragerRoots == null)
                _foragerRoots = CreateForagerFood(ExpeditionPerkSystem.RootsItemId, "Roots", 0.2f, 8f);
            if (_foragerBerries == null)
                _foragerBerries = CreateForagerFood(ExpeditionPerkSystem.BerriesItemId, "Berries", 0.15f, 6f);
        }

        private static ItemDefinition CreateForagerFood(string id, string display, float weight, float hunger)
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = id;
            item.displayName = display;
            item.description = "Low-nutrition forage. Staves off starvation.";
            item.type = ItemType.Food;
            item.weight = weight;
            item.stackMax = 20;
            item.hungerRestore = hunger;
            item.tradeValue = 1f;
            return item;
        }

        private void TryAddLootItem(ExpeditionState exp)
        {
            // Military / ground-zero nodes: chance to inject exclusive AP/API/battle-rifle ammo.
            if (TryInjectFactionAmmoLoot(exp))
                return;

            // Faction caches: world gear + extremely rare loose attachments.
            if (TryInjectFactionWorldLoot(exp))
                return;

            var item = _itemCatalog.items[_rng.Next(_itemCatalog.items.Count)];
            if (item == null) return;

            // Prompt #13 — hostile factions may swap medical loot for poison.
            if (_sabotagedCaches != null && exp.Survivor != null)
            {
                _sabotagedCaches.RecordScavengeLoot(exp.TargetLocationId);
                var outcome = _sabotagedCaches.ProcessLootCandidate(
                    exp.Survivor, item, out var resultItem);
                if (outcome == SabotagedLootOutcome.DetectedAndDiscarded)
                {
                    OnSabotagedCacheDetected?.Invoke(exp,
                        "The seals are wrong. Left the crate.");
                    return;
                }
                if (outcome == SabotagedLootOutcome.Poisoned && resultItem != null)
                {
                    exp.TryAddLoot(resultItem);
                    OnSabotagedCachePlanted?.Invoke(exp);
                    return;
                }
            }

            exp.TryAddLoot(item);
        }

        /// <summary>
        /// On military loot tables / high-danger ground-zero style nodes, roll
        /// non-craftable exclusive ammo (AP/API/battle rifle) into expedition loot.
        /// </summary>
        private bool TryInjectFactionAmmoLoot(ExpeditionState exp)
        {
            LastFactionAmmoLootIds.Clear();
            if (exp == null || _ammoTypes == null) return false;

            string lootTableId = null;
            var node = ResolveMapNode(exp.TargetLocationId);
            if (node != null) lootTableId = node.LootTableId;

            bool militarySite = Item_AmmoTypes.IsMilitaryLootTable(lootTableId)
                || exp.DangerLevel >= 4f
                || (lootTableId != null && lootTableId.IndexOf("military", StringComparison.OrdinalIgnoreCase) >= 0);

            // Also inject on human hostile kills at high danger (rebel/mil caches).
            if (!militarySite && exp.DangerLevel < 3.5f) return false;

            // Base chance scales with danger; military tables are more reliable.
            float chance = militarySite ? 0.45f + exp.DangerLevel * 0.05f : 0.12f + exp.DangerLevel * 0.03f;
            if (_rng.NextDouble() >= Mathf.Clamp01(chance)) return false;

            var source = Item_AmmoTypes.SourceForLootTable(lootTableId);
            if (!Item_AmmoTypes.IsMilitaryOrRebelSource(source))
            {
                // High-danger non-tagged sites still yield military stock sometimes.
                source = AmmoFactionSource.MilitaryForces;
            }

            int count = exp.DangerLevel >= 5f ? 2 : 1;
            var ids = Item_AmmoTypes.RollFactionAmmoLoot(source, _rng, count, preferApApi: true);
            if (ids == null || ids.Count == 0) return false;

            bool any = false;
            for (int i = 0; i < ids.Count; i++)
            {
                string id = ids[i];
                var def = ResolveAmmoItemDefinition(id);
                if (def == null) continue;
                if (exp.TryAddLoot(def))
                {
                    LastFactionAmmoLootIds.Add(id);
                    any = true;
                }
            }
            return any;
        }

        /// <summary>
        /// Inject faction world gear / extremely rare loose attachments on military,
        /// rebel, bandit, insurgent, and high-danger sites.
        /// </summary>
        private bool TryInjectFactionWorldLoot(ExpeditionState exp)
        {
            LastFactionWorldLootIds.Clear();
            if (exp == null || !_worldLootEnabled) return false;

            string lootTableId = null;
            var node = ResolveMapNode(exp.TargetLocationId);
            if (node != null) lootTableId = node.LootTableId;

            bool factionSite = Item_WorldCatalog.IsFactionGearLootTable(lootTableId)
                || Item_AmmoTypes.IsMilitaryLootTable(lootTableId)
                || exp.DangerLevel >= 3.5f;
            if (!factionSite) return false;

            float chance = Item_WorldCatalog.IsFactionGearLootTable(lootTableId)
                || Item_AmmoTypes.IsMilitaryLootTable(lootTableId)
                ? 0.40f + exp.DangerLevel * 0.05f
                : 0.14f + exp.DangerLevel * 0.03f;
            if (_rng.NextDouble() >= Mathf.Clamp01(chance)) return false;

            var faction = Item_WorldCatalog.SourceForLootTable(lootTableId);
            if (faction == WorldLootFaction.Civilian)
            {
                if (exp.DangerLevel >= 5f) faction = WorldLootFaction.Military;
                else if (exp.DangerLevel >= 3.5f) faction = WorldLootFaction.Bandit;
            }

            int count = exp.DangerLevel >= 5f ? 2 : 1;
            var rolls = Item_WorldCatalog.RollFactionWorldLoot(
                faction, _rng, count, allowAttachments: true, dangerLevel: exp.DangerLevel);
            if (rolls == null || rolls.Count == 0) return false;

            bool any = false;
            for (int i = 0; i < rolls.Count; i++)
            {
                string id = rolls[i].ItemId;
                var def = ResolveWorldItemDefinition(id);
                if (def == null) continue;
                if (exp.TryAddLoot(def))
                {
                    LastFactionWorldLootIds.Add(id);
                    any = true;
                }
            }
            return any;
        }

        private ItemDefinition ResolveWorldItemDefinition(string itemId)
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

        private ItemDefinition ResolveAmmoItemDefinition(string ammoId)
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
            // Synthesize a stackable Weapon-typed ammo stack for tests / missing catalog.
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

        private void TryFireForcedLocationEncounter(ExpeditionState exp)
        {
            if (exp == null || exp.LocationEncounterFired) return;
            var forced = FindForcedLocationEncounter(exp.TargetLocationId);
            if (forced == null) return;
            exp.LocationEncounterFired = true;
            OnEncounterTriggered?.Invoke(exp, forced);
            ResolveEncounterWithPsychology(exp, forced);
        }

        /// <summary>Test hook: force the location-bound / flag-driven arrival beat.</summary>
        public bool ForceFireLocationEncounterForTests(ExpeditionState exp)
        {
            if (exp == null) return false;
            exp.LocationEncounterFired = false;
            TryFireForcedLocationEncounter(exp);
            return exp.LocationEncounterFired;
        }

        private void RollAndResolveEncounter(ExpeditionState exp)
        {
            if (_encounterPool.Count == 0) return;

            // Base encounter chance per tick: 30% modified by danger level and stance
            float encounterChance = 0.25f + (exp.DangerLevel * 0.05f);
            if (exp.Stance == ExpeditionStance.Speed) encounterChance *= 1.4f;
            else if (exp.Stance == ExpeditionStance.Stealth) encounterChance *= 0.6f;

            // Prompt #223 Apex Predator / #254 Butcher: full stealth (no random encounters).
            if (_personalQuests != null && exp.Survivor != null)
            {
                float stealth = _personalQuests.GetExpeditionStealthFactor(exp.Survivor);
                if (stealth < 0f)
                    stealth = _personalQuests.GetStealthFactor(exp.Survivor);
                if (stealth >= 1f)
                    encounterChance = 0f;
            }

            // Prompt #70 night risk; Prompt #209 Night Terror stealth at night.
            encounterChance *= NightScavengeSystem.GetEncounterRiskMultiplier(exp);
            if (_expeditionPerks != null && exp.Survivor != null && exp.IsNightScavenge)
            {
                float stealth = _expeditionPerks.GetNightStealthMultiplier(exp.Survivor, isNight: true);
                if (stealth > 1f)
                    encounterChance /= stealth; // better stealth → fewer ambushes
            }

            if (_rng.NextDouble() >= encounterChance) return;

            // Location-filtered weighted pick (Prompt #47)
            EncounterSO selected = PickEncounter(exp.TargetLocationId, exp.Stance, exp.DangerLevel);
            if (selected == null) return;

            // #251 Wasteland Scout: immune to Sniper encounters (small hitbox).
            if (_personalQuests != null && exp.Survivor != null
                && _personalQuests.IsImmuneToSniperEncounters(exp.Survivor)
                && IsSniperEncounter(selected))
            {
                return;
            }

            // #251 Wasteland Scout: crawl collapsed debris / rubble instantly (skip hazard).
            if (_personalQuests != null && exp.Survivor != null
                && _personalQuests.CanCrawlDebrisInstantly(exp.Survivor)
                && IsDebrisEncounter(selected))
            {
                return;
            }

            // #254 Butcher of Day 30: silent assassination of human encounters + loot gear.
            if (_personalQuests != null && exp.Survivor != null
                && _personalQuests.AutoClearsHumanEncounters(exp.Survivor)
                && IsHumanEncounter(selected))
            {
                ResolveButcherAssassination(exp, selected);
                return;
            }

            // #258 Saboteur: auto-disarm traps with no player prompt.
            if (_personalQuests != null && exp.Survivor != null
                && _personalQuests.AutoDisarmsTraps(exp.Survivor)
                && IsTrapEncounter(selected))
            {
                ResolveSilentTrapDisarm(exp, selected);
                return;
            }

            // #259 Ghost Shooter: map-layer ranged kills without Hostile Encounter UI.
            if (_personalQuests != null && exp.Survivor != null
                && _personalQuests.SuppressesHostileEncounterUi(exp.Survivor)
                && IsHostileEncounter(selected))
            {
                ResolveGhostShooterKill(exp, selected);
                return;
            }

            // Prompt #207 — Light Step: skip dogs/ghouls before the trigger event fires.
            if (_expeditionPerks != null && exp.Survivor != null
                && _expeditionPerks.CanBypassEncounter(exp.Survivor, selected.id))
            {
                ResolveEncounterWithPsychology(exp, selected);
                return;
            }

            OnEncounterTriggered?.Invoke(exp, selected);

            // Psychological auto-resolution
            ResolveEncounterWithPsychology(exp, selected);
        }

        private static bool IsTrapEncounter(EncounterSO encounter)
        {
            if (encounter == null) return false;
            string id = encounter.id ?? string.Empty;
            string name = encounter.title ?? string.Empty;
            return id.IndexOf("trap", StringComparison.OrdinalIgnoreCase) >= 0
                || id.IndexOf("mine", StringComparison.OrdinalIgnoreCase) >= 0
                || id.IndexOf("snare", StringComparison.OrdinalIgnoreCase) >= 0
                || name.IndexOf("trap", StringComparison.OrdinalIgnoreCase) >= 0
                || name.IndexOf("mine", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static bool IsHostileEncounter(EncounterSO encounter)
        {
            if (encounter == null) return false;
            if (IsHumanEncounter(encounter)) return true;
            string id = encounter.id ?? string.Empty;
            string name = encounter.title ?? string.Empty;
            return id.IndexOf("hostile", StringComparison.OrdinalIgnoreCase) >= 0
                || id.IndexOf("ambush", StringComparison.OrdinalIgnoreCase) >= 0
                || id.IndexOf("dog", StringComparison.OrdinalIgnoreCase) >= 0
                || id.IndexOf("ghoul", StringComparison.OrdinalIgnoreCase) >= 0
                || name.IndexOf("hostile", StringComparison.OrdinalIgnoreCase) >= 0
                || name.IndexOf("ambush", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        /// <summary>#258 Silent trap disarm — no OnEncounterTriggered UI.</summary>
        private void ResolveSilentTrapDisarm(ExpeditionState exp, EncounterSO selected)
        {
            if (exp == null || selected == null) return;
            // Resolve without surfacing the encounter UI.
            ResolveEncounterWithPsychology(exp, selected);
        }

        /// <summary>#259 Ghost Shooter silent kill — no Hostile Encounter UI.</summary>
        private void ResolveGhostShooterKill(ExpeditionState exp, EncounterSO selected)
        {
            if (exp?.Survivor == null || selected == null) return;
            // Deliberately skip OnEncounterTriggered so the combat UI never opens.
            ResolveEncounterWithPsychology(exp, selected);
        }

        private static bool IsSniperEncounter(EncounterSO encounter)
        {
            if (encounter == null) return false;
            if (!string.IsNullOrEmpty(encounter.id)
                && encounter.id.IndexOf("sniper", StringComparison.OrdinalIgnoreCase) >= 0)
                return true;
            if (!string.IsNullOrEmpty(encounter.title)
                && encounter.title.IndexOf("sniper", StringComparison.OrdinalIgnoreCase) >= 0)
                return true;
            return false;
        }

        private static bool IsDebrisEncounter(EncounterSO encounter)
        {
            if (encounter == null) return false;
            string id = encounter.id ?? string.Empty;
            string name = encounter.title ?? string.Empty;
            return id.IndexOf("rubble", StringComparison.OrdinalIgnoreCase) >= 0
                || id.IndexOf("debris", StringComparison.OrdinalIgnoreCase) >= 0
                || id.IndexOf("collapsed", StringComparison.OrdinalIgnoreCase) >= 0
                || name.IndexOf("rubble", StringComparison.OrdinalIgnoreCase) >= 0
                || name.IndexOf("debris", StringComparison.OrdinalIgnoreCase) >= 0
                || name.IndexOf("collapsed", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static bool IsHumanEncounter(EncounterSO encounter)
        {
            if (encounter == null) return false;
            string id = encounter.id ?? string.Empty;
            string name = encounter.title ?? string.Empty;
            return id.IndexOf("looter", StringComparison.OrdinalIgnoreCase) >= 0
                || id.IndexOf("raider", StringComparison.OrdinalIgnoreCase) >= 0
                || id.IndexOf("bandit", StringComparison.OrdinalIgnoreCase) >= 0
                || id.IndexOf("human", StringComparison.OrdinalIgnoreCase) >= 0
                || id.IndexOf("faction", StringComparison.OrdinalIgnoreCase) >= 0
                || name.IndexOf("looter", StringComparison.OrdinalIgnoreCase) >= 0
                || name.IndexOf("raider", StringComparison.OrdinalIgnoreCase) >= 0
                || name.IndexOf("bandit", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        /// <summary>#254 silent kill: clear human encounter and bring back their gear tag.</summary>
        private void ResolveButcherAssassination(ExpeditionState exp, EncounterSO selected)
        {
            if (exp?.Survivor == null || selected == null) return;
            OnEncounterTriggered?.Invoke(exp, selected);
            // Mark resolved without combat — host loot via gear tag in survivor hidden stash.
            if (exp.Survivor.HiddenItemIds == null)
                exp.Survivor.HiddenItemIds = new System.Collections.Generic.List<string>();
            string gearTag = "butcher_loot_" + (selected.id ?? "human");
            if (!exp.Survivor.HiddenItemIds.Contains(gearTag))
                exp.Survivor.HiddenItemIds.Add(gearTag);
            // Still run psychology path as a no-damage resolve when available.
            ResolveEncounterWithPsychology(exp, selected);
        }

        private void EnsureDeserterStandRifle()
        {
            if (_deserterStandRifle != null) return;
            // Prefer catalog entry if present
            if (_itemCatalog?.items != null)
            {
                for (int i = 0; i < _itemCatalog.items.Count; i++)
                {
                    var it = _itemCatalog.items[i];
                    if (it != null
                        && string.Equals(it.id, DesertersStandSystem.ServiceRifleItemId,
                            StringComparison.OrdinalIgnoreCase))
                    {
                        _deserterStandRifle = it;
                        return;
                    }
                }
            }
            _deserterStandRifle = DesertersStandSystem.CreateServiceRifleDefinition();
        }

        /// <summary>
        /// Prompt #12 — if the target node has UXO and the scavenger is Reckless,
        /// roll detonation after a loot action.
        /// </summary>

        /// <summary>
        /// Prompt #12 — fleeing an encounter on a UXO node may trigger a mine.
        /// </summary>

        /// <summary>
        /// Test / scripted hook: force a UXO check with an explicit detonation decision.
        /// </summary>

    }
}
