using System;
using System.Collections.Generic;

namespace Ashfall.Core.Expeditions
{
    public enum ExpeditionStance
    {
        Stealth, // lower encounter chance, standard travel speed
        Speed    // 1.5x travel speed, higher encounter chance
    }

    public enum ExpeditionPhase
    {
        Outbound,  // traveling to the target
        Looting,   // at the site, push-your-luck scavenging
        Inbound,   // returning to shelter
        Completed, // returned with loot unloaded
        Failed     // collapsed or killed
    }

    [Serializable]
    public class ExpeditionLootEntry
    {
        public string itemId = string.Empty;
        public int quantity = 0;
        public float weightKg = 0f;
    }

    /// <summary>Serialized state of one expedition (save/load safe).</summary>
    [Serializable]
    public class ExpeditionState
    {
        public string systemId = ExpeditionSystem.SystemId;
        public string expeditionId = string.Empty;
        public string survivorId = string.Empty;
        public string locationId = string.Empty;
        public string displayName = string.Empty;
        public string stance = "Stealth";
        public int phase = (int)ExpeditionPhase.Outbound;
        public int startedDay = 0;
        public int distanceTicks = 0;
        public int travelTicksCompleted = 0;
        public int lootingTicksCompleted = 0;
        public float stamina = 100f;
        public float maxLootCapacityKg = 40f;
        public float currentWeightKg = 0f;
        public int dangerLevel = 1;
        public float encounterChancePerTick = 0.12f;
        public int encounterCount = 0;
        public bool isPushingLuck = false;
        public bool isNightScavenge = false;
        public bool hasBicycle = false;
        public bool hasFlashlight = false;
        public string outcomeText = string.Empty;
        public List<ExpeditionLootEntry> loot = new List<ExpeditionLootEntry>();
    }

    /// <summary>Data-driven target definition for an expedition.</summary>
    [Serializable]
    public class ExpeditionDefinition
    {
        public string id = string.Empty;
        public string displayName = string.Empty;
        public int distanceTicks = 8;
        public int dangerLevel = 1;
        public float encounterChancePerTick = 0.12f;
        public float baseStaminaDrainPerHour = 2.0f;
        public List<string> lootCategories = new List<string>();
    }

    /// <summary>
    /// Engine-agnostic expedition core (port of the Unity ExpeditionSystem's
    /// travel/looting/inbound mechanics). Tick-based: Outbound travel, arrival,
    /// push-your-luck looting with capacity, auto-retreat, Inbound return,
    /// completion or collapse failure. All rolls go through ISeededRng passed
    /// per tick — the host owns seeding, so the core never stores RNG state.
    /// Zero engine namespaces; events + save/load per the house pattern.
    /// </summary>
    public class ExpeditionSystem
    {
        public const string SystemId = "expedition_system";
        public const float MaxStamina = 100f;
        public const int AutoRetreatAfterLootTicks = 3;
        public const float EncumberPenaltyPerTickMax = 15f;

        private readonly Dictionary<string, ExpeditionState> _active = new Dictionary<string, ExpeditionState>();

        /// <summary>
        /// Optional per-survivor stamina-drain multiplier hook (0..N). Set by the
        /// host so Phase-0 effects (respiratory severe cough, phantom work refusal,
        /// guilt insomnia fatigue) reach the real expedition stamina consumer.
        /// Returns 1.0 when unset or unknown. The multiplier is applied to the
        /// base per-hour drain in ApplyStaminaDrain.
        /// </summary>
        private Func<string, float> _staminaDrainMultiplier;

        public event Action<ExpeditionState> OnExpeditionStarted;
        public event Action<ExpeditionState> OnExpeditionTick;
        public event Action<ExpeditionState> OnPhaseChanged;
        public event Action<ExpeditionState> OnLootAdded;                 // state, itemId, qty
        public event Action<ExpeditionState> OnEncounterTriggered;
        public event Action<ExpeditionState> OnExpeditionCompleted;
        public event Action<ExpeditionState, string> OnExpeditionFailed;
        public event Action<ExpeditionState> OnStateChanged;

        public ExpeditionSystem()
        {
        }

        /// <summary>
        /// Bind the per-survivor stamina-drain multiplier. Hosts wire Phase-0
        /// respiratory/guilt/phantom effects here so they alter real expedition
        /// stamina consumption rather than living in a display value.
        /// </summary>
        public void SetStaminaDrainMultiplier(Func<string, float> multiplier)
        {
            _staminaDrainMultiplier = multiplier;
        }

        /// <summary>
        /// Optional per-location encounter-chance multiplier (1.0 = authored
        /// rate). Hosts wire faction/territory danger here (e.g. the warlord's
        /// TravelDangerModifier for controlled/contested road) so hostile ground
        /// raises the chance of meeting trouble on a real sortie. The roll is
        /// still seeded and deterministic for a given multiplier.
        /// </summary>
        private Func<string, float> _encounterChanceMultiplier;

        /// <summary>
        /// Bind the per-location encounter-chance multiplier. Returns 1.0 when
        /// unset or unknown. Multipliers clamp the resulting chance to [0,1].
        /// </summary>
        public void SetEncounterChanceMultiplier(Func<string, float> multiplier)
        {
            _encounterChanceMultiplier = multiplier;
        }

        public IReadOnlyDictionary<string, ExpeditionState> Active => _active;
        public int ActiveCount => _active.Count;

        // ── Lifecycle ──────────────────────────────────────────────────

        public bool Start(
            ExpeditionDefinition def,
            string survivorId,
            int day,
            ExpeditionStance stance = ExpeditionStance.Stealth,
            bool isNightScavenge = false,
            bool hasBicycle = false,
            bool hasFlashlight = false)
        {
            if (def == null || string.IsNullOrEmpty(def.id) || string.IsNullOrEmpty(survivorId))
                return false;
            if (_active.ContainsKey(survivorId)) return false; // one expedition per survivor

            var exp = new ExpeditionState
            {
                // Unique per survivor+target (Unity keys expeditions by id).
                expeditionId = survivorId + ":" + def.id,
                survivorId = survivorId,
                locationId = def.id,
                displayName = def.displayName,
                stance = stance.ToString(),
                startedDay = day,
                distanceTicks = Math.Max(1, def.distanceTicks),
                dangerLevel = def.dangerLevel,
                encounterChancePerTick = def.encounterChancePerTick,
                stamina = MaxStamina,
                isNightScavenge = isNightScavenge,
                hasBicycle = hasBicycle,
                hasFlashlight = hasFlashlight
            };
            _active[survivorId] = exp;
            OnExpeditionStarted?.Invoke(exp);
            OnStateChanged?.Invoke(exp);
            return true;
        }

        /// <summary>Advance the sector clock by tick hours for every active expedition.</summary>
        public void TickHours(float hours, ISeededRng rng)
        {
            if (hours <= 0f) return;
            var ids = new List<string>(_active.Keys);
            ids.Sort(string.CompareOrdinal); // deterministic iteration
            for (int i = 0; i < ids.Count; i++)
            {
                var exp = _active[ids[i]];
                if (exp.phase == (int)ExpeditionPhase.Completed || exp.phase == (int)ExpeditionPhase.Failed)
                    continue;

                ApplyStaminaDrain(exp, hours);
                if (exp.stamina <= 0f)
                {
                    Fail(exp, "Collapsed from exhaustion.");
                    continue;
                }

                RollEncounter(exp, rng); // every leg can meet trouble (Unity parity)

                switch ((ExpeditionPhase)exp.phase)
                {
                    case ExpeditionPhase.Outbound:
                        AdvanceOutbound(exp);
                        break;
                    case ExpeditionPhase.Looting:
                        AdvanceLooting(exp, rng);
                        break;
                    case ExpeditionPhase.Inbound:
                        AdvanceInbound(exp);
                        break;
                }

                if (exp.phase == (int)ExpeditionPhase.Completed)
                {
                    OnExpeditionCompleted?.Invoke(exp);
                    _active.Remove(exp.survivorId);
                    OnStateChanged?.Invoke(exp);
                    continue;
                }

                OnExpeditionTick?.Invoke(exp);
                OnStateChanged?.Invoke(exp);
            }
        }

        public bool PushLuck(string survivorId)
        {
            if (!_active.TryGetValue(survivorId, out var exp)) return false;
            if ((ExpeditionPhase)exp.phase != ExpeditionPhase.Looting) return false;
            exp.isPushingLuck = true;
            OnStateChanged?.Invoke(exp);
            return true;
        }

        public bool Retreat(string survivorId)
        {
            if (!_active.TryGetValue(survivorId, out var exp)) return false;
            if ((ExpeditionPhase)exp.phase != ExpeditionPhase.Looting) return false;
            SetPhase(exp, ExpeditionPhase.Inbound);
            OnStateChanged?.Invoke(exp);
            return true;
        }

        // ── Phase mechanics (ported 1:1 from the Unity host) ──────────

        private void AdvanceOutbound(ExpeditionState exp)
        {
            float step = exp.stance == nameof(ExpeditionStance.Speed) ? 1.5f : 1.0f;
            exp.travelTicksCompleted += (int)Math.Round(step, MidpointRounding.AwayFromZero);
            if (exp.travelTicksCompleted >= exp.distanceTicks)
                SetPhase(exp, ExpeditionPhase.Looting);
        }

        private bool AdvanceLooting(ExpeditionState exp, ISeededRng rng)
        {
            exp.lootingTicksCompleted++;
            PerformLootRoll(exp, rng);
            MaybeAutoRetreat(exp);
            return false;
        }

        private void PerformLootRoll(ExpeditionState exp, ISeededRng rng)
        {
            if (rng == null) return;
            float chance = 0.5f + exp.dangerLevel * 0.05f;
            if (exp.isNightScavenge) chance += 0.1f;   // riskier, richer
            if (rng.NextDouble() >= chance) return;

            // Pick a category (or a generic item when the table is empty).
            string itemId = exp.loot.Count > 0
                ? PickLootCategory(exp, rng)
                : "scrap_metal";
            if (string.IsNullOrEmpty(itemId)) itemId = "scrap_metal";

            const float itemWeight = 1.0f;
            if (exp.currentWeightKg + itemWeight > exp.maxLootCapacityKg)
            {
                exp.outcomeText = "Capacity full; the find stays behind.";
                return;
            }

            AddLoot(exp, itemId, itemWeight);
        }

        private static string PickLootCategory(ExpeditionState exp, ISeededRng rng)
        {
            var def = ExpeditionDefinitionRegistry.Get(exp.locationId);
            if (def != null && def.lootCategories != null && def.lootCategories.Count > 0)
            {
                int idx = rng.Next(0, def.lootCategories.Count);
                return def.lootCategories[idx];
            }
            // Fall back to categories already found, else a generic item.
            int existing = rng.Next(0, exp.loot.Count);
            return exp.loot[existing].itemId;
        }

        private void AddLoot(ExpeditionState exp, string itemId, float weightKg)
        {
            for (int i = 0; i < exp.loot.Count; i++)
            {
                if (exp.loot[i].itemId == itemId)
                {
                    exp.loot[i].quantity++;
                    exp.currentWeightKg += weightKg;
                    OnLootAdded?.Invoke(exp);
                    return;
                }
            }
            exp.loot.Add(new ExpeditionLootEntry { itemId = itemId, quantity = 1, weightKg = weightKg });
            exp.currentWeightKg += weightKg;
            OnLootAdded?.Invoke(exp);
        }

        private void RollEncounter(ExpeditionState exp, ISeededRng rng)
        {
            if (rng == null) return;
            float chance = exp.encounterChancePerTick;
            if (_encounterChanceMultiplier != null && !string.IsNullOrEmpty(exp.locationId))
            {
                float mult = _encounterChanceMultiplier(exp.locationId);
                if (mult >= 0f) chance *= mult; // 0 ⇒ no encounters on this ground
            }
            chance = Math.Clamp(chance, 0f, 1f);
            if (exp.stance == nameof(ExpeditionStance.Stealth)) chance *= 0.5f;
            if (rng.NextDouble() < chance)
            {
                exp.encounterCount++;
                OnEncounterTriggered?.Invoke(exp);
            }
        }

        private void MaybeAutoRetreat(ExpeditionState exp)
        {
            if (exp.isPushingLuck) return;
            if (exp.lootingTicksCompleted >= AutoRetreatAfterLootTicks)
                SetPhase(exp, ExpeditionPhase.Inbound);
        }

        private void AdvanceInbound(ExpeditionState exp)
        {
            float step = exp.stance == nameof(ExpeditionStance.Speed) ? 1.5f : 1.0f;
            if (exp.hasBicycle) step += 0.5f; // faster return on a bicycle
            exp.travelTicksCompleted -= (int)Math.Round(step, MidpointRounding.AwayFromZero);
            if (exp.travelTicksCompleted <= 0)
            {
                exp.travelTicksCompleted = 0;
                SetPhase(exp, ExpeditionPhase.Completed);
            }
        }

        private void ApplyStaminaDrain(ExpeditionState exp, float hours)
        {
            var def = ExpeditionDefinitionRegistry.Get(exp.locationId);
            float baseDrain = def != null ? def.baseStaminaDrainPerHour : 2.0f;
            float drain = baseDrain * hours;
            float loadRatio = exp.maxLootCapacityKg > 0f
                ? Math.Clamp(exp.currentWeightKg / exp.maxLootCapacityKg, 0f, 1f)
                : 0f;
            drain += loadRatio * EncumberPenaltyPerTickMax * hours;

            // Phase-0 effect hook: respiratory severe cough, guilt insomnia
            // fatigue, phantom work refusal etc. increase the drain for this
            // survivor (multiplier defaults to 1.0 when unset/unknown).
            if (_staminaDrainMultiplier != null && !string.IsNullOrEmpty(exp.survivorId))
            {
                float mult = _staminaDrainMultiplier(exp.survivorId);
                if (mult > 0f) drain *= mult;
            }

            exp.stamina = Math.Clamp(exp.stamina - drain, 0f, MaxStamina);
        }

        private void Fail(ExpeditionState exp, string reason)
        {
            SetPhase(exp, ExpeditionPhase.Failed);
            exp.outcomeText = reason;
            OnExpeditionFailed?.Invoke(exp, reason);
            _active.Remove(exp.survivorId);
            OnStateChanged?.Invoke(exp);
        }

        private void SetPhase(ExpeditionState exp, ExpeditionPhase phase)
        {
            if (exp.phase == (int)phase) return;
            exp.phase = (int)phase;
            OnPhaseChanged?.Invoke(exp);
        }

        // ── Save / Load ────────────────────────────────────────────────

        /// <summary>Snapshot of the single active-envelope shape: one state per active expedition, ordinal-ordered.</summary>
        public List<ExpeditionState> CaptureState()
        {
            var copy = new List<ExpeditionState>();
            var ids = new List<string>(_active.Keys);
            ids.Sort(string.CompareOrdinal);
            for (int i = 0; i < ids.Count; i++)
                copy.Add(CloneExpedition(_active[ids[i]]));
            return copy;
        }

        public void RestoreState(List<ExpeditionState> saved)
        {
            _active.Clear();
            if (saved == null) return;
            for (int i = 0; i < saved.Count; i++)
            {
                var s = saved[i];
                if (s == null || string.IsNullOrEmpty(s.survivorId) || string.IsNullOrEmpty(s.expeditionId))
                    continue;
                var exp = CloneExpedition(s);
                _active[exp.survivorId] = exp;
            }
            OnStateChanged?.Invoke(null!);
        }

        private static ExpeditionState CloneExpedition(ExpeditionState src)
        {
            var copy = new ExpeditionState
            {
                systemId = src.systemId,
                expeditionId = src.expeditionId,
                survivorId = src.survivorId,
                locationId = src.locationId,
                displayName = src.displayName,
                stance = src.stance,
                phase = Math.Clamp(src.phase, (int)ExpeditionPhase.Outbound, (int)ExpeditionPhase.Failed),
                startedDay = src.startedDay,
                distanceTicks = src.distanceTicks,
                travelTicksCompleted = src.travelTicksCompleted,
                lootingTicksCompleted = src.lootingTicksCompleted,
                stamina = Math.Clamp(src.stamina, 0f, MaxStamina),
                maxLootCapacityKg = src.maxLootCapacityKg,
                currentWeightKg = Math.Max(0f, src.currentWeightKg),
                dangerLevel = src.dangerLevel,
                encounterChancePerTick = src.encounterChancePerTick,
                encounterCount = src.encounterCount,
                isPushingLuck = src.isPushingLuck,
                isNightScavenge = src.isNightScavenge,
                hasBicycle = src.hasBicycle,
                hasFlashlight = src.hasFlashlight,
                outcomeText = src.outcomeText
            };
            if (src.loot != null)
            {
                var ordered = new List<ExpeditionLootEntry>(src.loot);
                ordered.Sort((a, b) => string.CompareOrdinal(a.itemId, b.itemId));
                for (int i = 0; i < ordered.Count; i++)
                    copy.loot.Add(new ExpeditionLootEntry
                    {
                        itemId = ordered[i].itemId,
                        quantity = Math.Max(0, ordered[i].quantity),
                        weightKg = ordered[i].weightKg
                    });
            }
            return copy;
        }
    }

    /// <summary>
    /// Registry of expedition definitions so the core stays data-free while
    /// still resolving loot tables by location id (hosts register the JSON
    /// catalogs they load; tests register inline tables).
    /// </summary>
    public static class ExpeditionDefinitionRegistry
    {
        private static readonly Dictionary<string, ExpeditionDefinition> s_defs =
            new Dictionary<string, ExpeditionDefinition>();

        public static void Register(ExpeditionDefinition def)
        {
            if (def == null || string.IsNullOrEmpty(def.id)) return;
            s_defs[def.id] = def;
        }

        public static ExpeditionDefinition? Get(string id)
        {
            return !string.IsNullOrEmpty(id) && s_defs.TryGetValue(id, out var def) ? def : null;
        }

        public static void Clear() => s_defs.Clear();
    }
}
