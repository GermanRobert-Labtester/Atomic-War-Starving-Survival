using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// When a survivor dies their body becomes a storage item. Left in the bunker
    /// it grows mold, spreads disease, and drains morale. Disposal: bury outside
    /// (4h daylight + rad risk) or process for greenhouse fertilizer (morale shatter).
    /// Internal Horror — Corpse Management &amp; Rot.
    /// </summary>
    public class CorpseManagementSystem
    {
        public const string CorpseItemId = "corpse";
        public const string FertilizerItemId = "fertilizer";

        /// <summary>Game-hours of daylight required to bury outside.</summary>
        public const float BuryHours = 4f;

        /// <summary>Ambient rad dose applied to the burial party (rads).</summary>
        public const float BuryRadDose = 8f;

        /// <summary>Morale lost per living survivor per hour per corpse in storage.</summary>
        public const float MoraleDrainPerCorpsePerHour = 3f;

        /// <summary>Mold level added per hour per corpse.</summary>
        public const float MoldPerCorpsePerHour = 0.04f;

        /// <summary>Chance per hour per living survivor to catch bacterial infection from rot.</summary>
        public const float DiseaseChancePerHour = 0.06f;

        /// <summary>Group morale hit when processing a corpse for fertilizer.</summary>
        public const float FertilizerMoraleHit = 25f;

        private readonly NeedsSystem _needs;
        private readonly Inventory.Inventory _inventory;
        private readonly MedicalSystem _medical;
        private readonly RadiationSystem _radiation;
        private readonly System.Random _rng;

        private ItemDefinition _corpseDef;
        private ItemDefinition _fertilizerDef;
        private ShelterRoom _storesRoom;
        private Func<IReadOnlyList<Survivor>> _getSurvivors;
        private CombatPerkSystem _combatPerks;
        private bool _boundToDeath;

        /// <summary>Corpse metadata: which survivor the body belonged to (by stack order, best-effort).</summary>
        private readonly List<string> _corpseSourceIds = new List<string>();

        public event Action<Survivor, ItemDefinition> OnCorpseCreated;
        public event Action<string> OnCorpseBuried;
        public event Action<string> OnCorpseProcessedForFertilizer;
        public event Action OnCorpseStateChanged;

        public int CorpseCount => _inventory != null ? _inventory.CountByType(ItemType.Corpse) : 0;

        public CorpseManagementSystem(
            NeedsSystem needs,
            Inventory.Inventory inventory,
            MedicalSystem medical = null,
            RadiationSystem radiation = null,
            System.Random rng = null)
        {
            _needs = needs ?? throw new ArgumentNullException(nameof(needs));
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _medical = medical;
            _radiation = radiation;
            _rng = rng ?? new System.Random(41);
        }

        public void SetItemDefinitions(ItemDefinition corpse, ItemDefinition fertilizer)
        {
            _corpseDef = corpse;
            _fertilizerDef = fertilizer;
        }

        public void SetStoresRoom(ShelterRoom room) => _storesRoom = room;

        public void SetSurvivorProvider(Func<IReadOnlyList<Survivor>> getSurvivors)
        {
            _getSurvivors = getSurvivors;
        }

        /// <summary>Prompt #188 — Desensitized skips corpse morale penalties.</summary>
        public void BindCombatPerks(CombatPerkSystem combatPerks) => _combatPerks = combatPerks;

        /// <summary>Subscribe to NeedsSystem.OnDied once. Safe to call multiple times.</summary>
        public void BindDeathHandler()
        {
            if (_boundToDeath) return;
            _needs.OnDied += HandleDeath;
            _boundToDeath = true;
        }

        public void UnbindDeathHandler()
        {
            if (!_boundToDeath) return;
            _needs.OnDied -= HandleDeath;
            _boundToDeath = false;
        }

        /// <summary>Factory corpse ItemDefinition for bootstrap / tests.</summary>
        public static ItemDefinition CreateCorpseDefinition()
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = CorpseItemId;
            item.displayName = "Body";
            item.description = "Someone who was breathing this morning. Takes a full storage slot.";
            item.type = ItemType.Corpse;
            item.stackMax = 1;
            item.weight = 70f;
            item.tradeValue = 0f;
            item.contamination = 0.6f;
            return item;
        }

        /// <summary>Factory fertilizer ItemDefinition for bootstrap / tests.</summary>
        public static ItemDefinition CreateFertilizerDefinition()
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = FertilizerItemId;
            item.displayName = "Bone Meal Fertilizer";
            item.description = "Greenhouse feed. Nobody will look at the bags.";
            item.type = ItemType.Material;
            item.stackMax = 20;
            item.weight = 2f;
            item.tradeValue = 4f;
            return item;
        }

        private void HandleDeath(Survivor survivor)
        {
            if (survivor == null) return;
            CreateCorpseFrom(survivor);
        }

        /// <summary>Place a corpse item into bunker inventory for a dead survivor.</summary>
        public bool CreateCorpseFrom(Survivor survivor)
        {
            EnsureCorpseDef();
            if (_corpseDef == null || _inventory == null) return false;

            if (!_inventory.Add(_corpseDef, 1))
            {
                // Inventory full: still track so rot can apply via overflow ambient.
                _corpseSourceIds.Add(survivor?.Id ?? "unknown");
                OnCorpseStateChanged?.Invoke();
                return false;
            }

            _corpseSourceIds.Add(survivor?.Id ?? "unknown");
            OnCorpseCreated?.Invoke(survivor, _corpseDef);
            OnCorpseStateChanged?.Invoke();
            return true;
        }

        /// <summary>
        /// Per-hour rot: mold growth, ambient contamination, morale drain, disease rolls.
        /// </summary>
        public void Tick(float gameHours, IReadOnlyList<Survivor> survivors = null)
        {
            if (gameHours <= 0f) return;
            int corpses = CorpseCount;
            if (corpses <= 0) return;

            survivors = survivors ?? _getSurvivors?.Invoke();

            if (_storesRoom != null)
            {
                _storesRoom.HasMold = true;
                _storesRoom.MoldLevel = Mathf.Clamp01(
                    _storesRoom.MoldLevel + MoldPerCorpsePerHour * corpses * gameHours);
                _storesRoom.AmbientContamination = Mathf.Clamp01(
                    _storesRoom.AmbientContamination + 0.02f * corpses * gameHours);
                _storesRoom.Humidity = Mathf.Clamp01(
                    _storesRoom.Humidity + 0.01f * corpses * gameHours);
            }

            if (survivors == null) return;

            for (int i = 0; i < survivors.Count; i++)
            {
                var sv = survivors[i];
                if (sv == null || !sv.IsAlive) continue;

                // Prompt #188 — Desensitized: no morale drain from living with corpses.
                bool immuneCorpseMorale = _combatPerks != null
                    && _combatPerks.IsImmuneToCorpseMorale(sv);
                if (!immuneCorpseMorale)
                {
                    _needs.Modify(sv, NeedKind.Morale,
                        -MoraleDrainPerCorpsePerHour * corpses * gameHours);
                }

                if (_medical != null && _rng.NextDouble() < DiseaseChancePerHour * corpses * gameHours)
                {
                    if (!_medical.HasAffliction(sv, AfflictionSO.Ids.BacterialInfection)
                        && !_medical.HasAffliction(sv, AfflictionSO.Ids.Dysentery))
                    {
                        _medical.Inflict(sv, AfflictionSO.Ids.BacterialInfection);
                    }
                }
            }

            OnCorpseStateChanged?.Invoke();
        }

        /// <summary>
        /// Bury one corpse outside. Costs BuryHours of the digger's fatigue and rad exposure.
        /// </summary>
        public bool BuryTheDead(Survivor digger, float daylightHoursAvailable = BuryHours)
        {
            if (digger == null || !digger.IsAlive) return false;
            if (daylightHoursAvailable < BuryHours) return false;
            if (CorpseCount <= 0) return false;
            EnsureCorpseDef();
            if (_corpseDef == null || !_inventory.Remove(_corpseDef, 1)) return false;

            PopCorpseSource(out string sourceId);

            // Exhaustion from 4 hours of digging
            _needs.Modify(digger, NeedKind.Fatigue, 35f);
            _needs.Modify(digger, NeedKind.Warmth, -10f);

            if (_radiation != null)
                _radiation.Expose(digger, BuryRadDose, 1f);

            // Small morale recovery — the living did something decent
            var survivors = _getSurvivors?.Invoke();
            if (survivors != null)
            {
                for (int i = 0; i < survivors.Count; i++)
                {
                    if (survivors[i] != null && survivors[i].IsAlive)
                        _needs.Modify(survivors[i], NeedKind.Morale, 4f);
                }
            }

            OnCorpseBuried?.Invoke(sourceId);
            OnCorpseStateChanged?.Invoke();
            return true;
        }

        /// <summary>
        /// Cold process: body → fertilizer for the greenhouse. Shatters group morale.
        /// </summary>
        public bool ProcessForFertilizer(Survivor processor)
        {
            if (processor == null || !processor.IsAlive) return false;
            if (CorpseCount <= 0) return false;
            EnsureCorpseDef();
            EnsureFertilizerDef();
            if (_corpseDef == null || !_inventory.Remove(_corpseDef, 1)) return false;

            PopCorpseSource(out string sourceId);

            if (_fertilizerDef != null)
                _inventory.Add(_fertilizerDef, 3);

            var survivors = _getSurvivors?.Invoke();
            if (survivors != null)
            {
                for (int i = 0; i < survivors.Count; i++)
                {
                    var sv = survivors[i];
                    if (sv == null || !sv.IsAlive) continue;
                    // Prompt #188 — Desensitized: no fertilizer-process morale shatter.
                    if (_combatPerks != null && _combatPerks.IsImmuneToCorpseMorale(sv))
                        continue;
                    _needs.Modify(sv, NeedKind.Morale, -FertilizerMoraleHit);
                }
            }
            else if (_combatPerks == null || !_combatPerks.IsImmuneToCorpseMorale(processor))
            {
                _needs.Modify(processor, NeedKind.Morale, -FertilizerMoraleHit);
            }

            OnCorpseProcessedForFertilizer?.Invoke(sourceId);
            OnCorpseStateChanged?.Invoke();
            return true;
        }

        private void PopCorpseSource(out string sourceId)
        {
            if (_corpseSourceIds.Count > 0)
            {
                sourceId = _corpseSourceIds[0];
                _corpseSourceIds.RemoveAt(0);
            }
            else
            {
                sourceId = "unknown";
            }
        }

        private void EnsureCorpseDef()
        {
            if (_corpseDef != null) return;
            // Prefer an existing inventory stack definition if present
            var slot = _inventory?.FindSlot(CorpseItemId);
            if (slot?.Item != null)
            {
                _corpseDef = slot.Item;
                return;
            }
            _corpseDef = CreateCorpseDefinition();
        }

        private void EnsureFertilizerDef()
        {
            if (_fertilizerDef != null) return;
            var slot = _inventory?.FindSlot(FertilizerItemId);
            if (slot?.Item != null)
            {
                _fertilizerDef = slot.Item;
                return;
            }
            _fertilizerDef = CreateFertilizerDefinition();
        }

        public CorpseManagementSave CaptureState()
        {
            return new CorpseManagementSave
            {
                CorpseSourceIds = _corpseSourceIds.ToArray()
            };
        }

        public void RestoreState(CorpseManagementSave save)
        {
            _corpseSourceIds.Clear();
            if (save?.CorpseSourceIds == null) return;
            for (int i = 0; i < save.CorpseSourceIds.Length; i++)
            {
                if (!string.IsNullOrEmpty(save.CorpseSourceIds[i]))
                    _corpseSourceIds.Add(save.CorpseSourceIds[i]);
            }
        }
    }

    [Serializable]
    public class CorpseManagementSave
    {
        public string[] CorpseSourceIds;
    }
}
