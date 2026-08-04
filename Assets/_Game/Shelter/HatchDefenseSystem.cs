using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Shelter.Modules;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Shelter
{
    /// <summary>
    /// Why a raid was attempted (Prompt #33).
    /// </summary>
    public enum RaidTrigger
    {
        /// <summary>Faction trust dropped to/below raid threshold (#25).</summary>
        FactionTrust,
        /// <summary>High external noise (e.g. generator run outside).</summary>
        Noise,
        /// <summary>Scripted / test force.</summary>
        Forced
    }

    /// <summary>
    /// Pending or resolved hatch raid. Not a combat minigame — a resource/time sink.
    /// </summary>
    [Serializable]
    public class RaidEvent
    {
        public string Id;
        public string FactionId;
        public RaidTrigger Trigger = RaidTrigger.Forced;
        /// <summary>Attack power compared against Defense (security + weapons).</summary>
        public float Strength = 50f;
        public int Day;
        public string Message;
    }

    /// <summary>One stack removed when the hatch is breached.</summary>
    [Serializable]
    public class StolenLootLine
    {
        public string ItemId;
        public int Amount;
        public string DisplayName;
    }

    /// <summary>Full resolution of a RaidEvent vs ShelterSecurity + weapons.</summary>
    [Serializable]
    public class RaidResolution
    {
        public RaidEvent Event;
        public bool Launched;
        public bool Repelled;
        public bool Breached => Launched && !Repelled;

        public float RaidStrength;
        public float ShelterSecurity;
        public float WeaponPower;
        public float DefenseScore;
        public float GuardBonusApplied;

        public float HatchDamage;
        public float MoraleDelta;
        public int AmmoConsumed;
        public float WeaponDurabilityLost;

        public List<StolenLootLine> StolenItems = new List<StolenLootLine>();
        public List<string> TraumatizedSurvivorIds = new List<string>();
        public string Message;
    }

    /// <summary>
    /// Systemic hatch defense (Prompt #33). Post-Day 30 the shelter is a target:
    /// RaidStrength vs ShelterSecurity + EquippedWeapons. Guard duty boosts security
    /// while draining Fatigue. No combat minigame — ammo, durability, loot, trauma.
    /// </summary>
    public class HatchDefenseSystem
    {
        public const int RaidUnlockDay = 30;
        public const float DefaultBaseSecurity = 5f;
        public const float GuardSecurityBonusPerGuard = 15f;
        public const float GuardFatigueDrain = 18f;
        public const float RepelMoraleBoost = 6f;
        public const float BreachMoralePenalty = -12f;
        public const float NoiseRaidStrength = 40f;
        public const float ExternalGeneratorNoiseThreshold = 0.6f;
        /// <summary>Game-hours between outdoor-noise raid rolls post Day 30.</summary>
        public const float NoiseCheckIntervalHours = 6f;
        /// <summary>Minimum game-hours between any two raids (trust or noise).</summary>
        public const float RaidCooldownHours = 12f;

        public const string OutdoorRoomId = "outside";

        /// <summary>Known hatch upgrade module ids and default security if no SO bound.</summary>
        public static readonly string[] HatchModuleIds =
        {
            HatchDefenseModuleSO.ReinforcedLocksId,
            HatchDefenseModuleSO.BlastDoorId,
            HatchDefenseModuleSO.HatchTrapsId
        };

        private readonly Func<Shelter> _getShelter;
        private readonly Func<Inventory.Inventory> _getInventory;
        private readonly Func<IReadOnlyList<Survivor>> _getSurvivors;
        private readonly Func<int> _getDay;
        private readonly Action<Survivor, string> _inflictTrauma;
        private readonly System.Random _rng;

        /// <summary>Active guards this evaluation wave (survivor id → bonus).</summary>
        private readonly Dictionary<string, float> _activeGuards = new Dictionary<string, float>();

        private float _noiseCheckAccumulator;
        private float _hoursSinceLastRaid = RaidCooldownHours;

        /// <summary>0..1 external noise (generator outside, loud work).</summary>
        public float ExternalNoise { get; private set; }

        /// <summary>True when a diesel/generator is running outside the sealed hatch.</summary>
        public bool GeneratorRunningOutside { get; set; }

        /// <summary>Optional test override for aggregate security (−1 = compute normally).</summary>
        public float SecurityOverride = -1f;

        /// <summary>Most recent raid resolution (null until first raid).</summary>
        public RaidResolution LastResolution { get; private set; }

        /// <summary>Short player-facing line for HUD / log.</summary>
        public string LastRaidSummary { get; private set; } = "Hatch quiet.";

        /// <summary>Total raids resolved this campaign (save/load).</summary>
        public int TotalRaidsResolved { get; private set; }

        /// <summary>Total breaches this campaign.</summary>
        public int TotalBreaches { get; private set; }

        public event Action<RaidResolution> OnRaidResolved;
        public event Action OnSecurityChanged;

        public HatchDefenseSystem(
            Func<Shelter> getShelter = null,
            Func<Inventory.Inventory> getInventory = null,
            Func<IReadOnlyList<Survivor>> getSurvivors = null,
            Func<int> getDay = null,
            Action<Survivor, string> inflictTrauma = null,
            System.Random rng = null)
        {
            _getShelter = getShelter;
            _getInventory = getInventory;
            _getSurvivors = getSurvivors;
            _getDay = getDay ?? (() => RaidUnlockDay);
            _inflictTrauma = inflictTrauma;
            _rng = rng ?? new System.Random(33);
        }

        // -----------------------------------------------------------------
        // ShelterSecurity
        // -----------------------------------------------------------------

        /// <summary>
        /// Aggregate hatch security from upgrades + radiation hatch plate + guards.
        /// </summary>
        public float GetShelterSecurity()
        {
            if (SecurityOverride >= 0f)
            {
                return SecurityOverride + GetGuardBonus();
            }

            var shelter = _getShelter != null ? _getShelter() : null;
            float score = DefaultBaseSecurity;

            if (shelter != null)
            {
                // Dedicated hatch defense modules
                for (int i = 0; i < shelter.Modules.Count; i++)
                {
                    var mod = shelter.Modules[i];
                    if (mod == null || !mod.IsOperational) continue;

                    if (mod.Definition is HatchDefenseModuleSO hatchSO)
                    {
                        score += hatchSO.SecurityContribution * Mathf.Max(1, mod.Level);
                        continue;
                    }

                    if (IsHatchModuleId(mod.ModuleId))
                    {
                        float perLevel = DefaultSecurityForModuleId(mod.ModuleId);
                        // Instance may stash contribution in ComfortLevel-like field if set
                        if (mod.SecurityContribution > 0f)
                            perLevel = mod.SecurityContribution;
                        score += perLevel * Mathf.Max(1, mod.Level);
                    }
                }

                // Legacy: radiation shielding doubles as a heavy hatch plate
                var shield = shelter.GetModule("radiation_shielding");
                if (shield != null && shield.IsOperational)
                {
                    score += 5f * Mathf.Max(1, shield.Level);
                }
            }

            score += GetGuardBonus();
            return Mathf.Max(0f, score);
        }

        public float GetGuardBonus()
        {
            float total = 0f;
            foreach (var kv in _activeGuards)
                total += kv.Value;
            return total;
        }

        public int ActiveGuardCount => _activeGuards.Count;

        public static bool IsHatchModuleId(string moduleId)
        {
            if (string.IsNullOrEmpty(moduleId)) return false;
            for (int i = 0; i < HatchModuleIds.Length; i++)
            {
                if (string.Equals(HatchModuleIds[i], moduleId, StringComparison.Ordinal))
                    return true;
            }
            return false;
        }

        public static float DefaultSecurityForModuleId(string moduleId)
        {
            if (moduleId == HatchDefenseModuleSO.BlastDoorId) return 25f;
            if (moduleId == HatchDefenseModuleSO.HatchTrapsId) return 15f;
            if (moduleId == HatchDefenseModuleSO.ReinforcedLocksId) return 10f;
            return 8f;
        }

        // -----------------------------------------------------------------
        // Weapons
        // -----------------------------------------------------------------

        /// <summary>
        /// Defense contribution from weapons and ammo in storage (stockpile pressure).
        /// </summary>
        public float GetWeaponPower(Inventory.Inventory inventory = null)
        {
            var inv = inventory ?? (_getInventory != null ? _getInventory() : null);
            if (inv?.Slots == null) return 0f;

            float power = 0f;
            for (int i = 0; i < inv.Slots.Count; i++)
            {
                var slot = inv.Slots[i];
                if (slot?.Item == null || slot.Amount <= 0) continue;
                power += GetItemDefenseContribution(slot);
            }

            // Equipped gear also counts
            if (inv.Equipped != null)
            {
                for (int i = 0; i < inv.Equipped.Count; i++)
                {
                    var eq = inv.Equipped[i];
                    if (eq?.Item == null) continue;
                    float maxDur = eq.Item.durability > 0f ? eq.Item.durability : 0f;
                    float dur = maxDur > 0f
                        ? Mathf.Clamp01(eq.CurrentDurability / maxDur)
                        : 1f;
                    power += GetWeaponBasePower(eq.Item) * dur;
                }
            }

            return Mathf.Max(0f, power);
        }

        public static bool IsWeaponItem(ItemDefinition item)
        {
            if (item == null) return false;
            if (item.type == ItemType.Weapon) return true;
            return IsWeaponId(item.id);
        }

        public static bool IsAmmoItem(ItemDefinition item)
        {
            if (item == null) return false;
            if (IsAmmoId(item.id)) return true;
            // Ammo stacks are Weapon-typed in items.json but stackMax > 1
            return item.type == ItemType.Weapon && item.stackMax > 1
                && (item.id != null && (item.id.Contains("ammo") || item.id.Contains("shell")));
        }

        public static bool IsWeaponId(string id)
        {
            if (string.IsNullOrEmpty(id)) return false;
            return id == "trench_knife" || id == "pipe_shotgun" || id == "revolver"
                || id == "kevlar_vest";
        }

        public static bool IsAmmoId(string id)
        {
            if (string.IsNullOrEmpty(id)) return false;
            return id == "handgun_ammo" || id == "shotgun_shells"
                || id.EndsWith("_ammo", StringComparison.Ordinal)
                || id.EndsWith("_shells", StringComparison.Ordinal);
        }

        public static float GetWeaponBasePower(ItemDefinition item)
        {
            if (item == null) return 0f;
            if (IsAmmoItem(item) || IsAmmoId(item.id)) return 0f; // counted per-stack below

            switch (item.id)
            {
                case "pipe_shotgun": return 18f;
                case "revolver": return 14f;
                case "trench_knife": return 6f;
                case "kevlar_vest": return 4f;
                default:
                    if (IsWeaponItem(item))
                        return Mathf.Max(5f, item.tradeValue * 0.5f);
                    return 0f;
            }
        }

        private static float GetItemDefenseContribution(InventorySlot slot)
        {
            var item = slot.Item;
            if (IsAmmoItem(item) || IsAmmoId(item.id))
            {
                // Ammo stockpile: soft cap so 200 rounds is not infinite defense
                float raw = slot.Amount * 0.4f;
                return Mathf.Min(20f, raw);
            }

            if (!IsWeaponItem(item) && !IsWeaponId(item.id)) return 0f;

            float basePower = GetWeaponBasePower(item);
            float durFrac = 1f;
            if (slot.CurrentDurability >= 0f && item.durability > 0f)
                durFrac = Mathf.Clamp01(slot.CurrentDurability / item.durability);
            else if (item.durability > 0f && slot.CurrentDurability < 0f)
                durFrac = 1f;

            return basePower * durFrac * Mathf.Max(1, slot.Amount);
        }

        // -----------------------------------------------------------------
        // Guard duty
        // -----------------------------------------------------------------

        /// <summary>
        /// Assign a survivor to guard: temporary security boost, heavy fatigue drain.
        /// </summary>
        public bool AssignGuard(Survivor survivor, float bonus = GuardSecurityBonusPerGuard)
        {
            if (survivor == null || !survivor.IsAlive) return false;
            if (survivor.Needs != null && survivor.Needs.Fatigue >= 95f) return false;

            _activeGuards[survivor.Id] = Mathf.Max(0f, bonus);
            if (survivor.Needs != null)
            {
                survivor.Needs.Fatigue = Mathf.Clamp(
                    survivor.Needs.Fatigue + GuardFatigueDrain, 0f, 100f);
            }

            survivor.State = SurvivorState.Working;
            OnSecurityChanged?.Invoke();
            return true;
        }

        public void ClearGuards()
        {
            if (_activeGuards.Count == 0) return;
            _activeGuards.Clear();
            OnSecurityChanged?.Invoke();
        }

        // -----------------------------------------------------------------
        // Noise / day gates
        // -----------------------------------------------------------------

        private readonly Dictionary<string, float> _raidChanceOverrides = new Dictionary<string, float>();

        public void SetRaidChanceOverride(string factionId, float chance)
        {
            if (string.IsNullOrEmpty(factionId)) return;
            _raidChanceOverrides[factionId] = Mathf.Clamp01(chance);
        }

        public void AdjustRaidChance(string factionId, float delta)
        {
            if (string.IsNullOrEmpty(factionId)) return;
            float cur = _raidChanceOverrides.TryGetValue(factionId, out var val) ? val : 0.2f;
            _raidChanceOverrides[factionId] = Mathf.Clamp01(cur + delta);
        }

        public void SetExternalNoise(float noise01)
        {
            ExternalNoise = Mathf.Clamp01(noise01);
        }

        public bool IsRaidUnlocked(int day = -1)
        {
            if (day < 0)
                day = _getDay != null ? _getDay() : RaidUnlockDay;
            return day >= RaidUnlockDay;
        }

        /// <summary>
        /// If generator is outside + post Day 30, may produce a noise-driven RaidEvent.
        /// Deterministic given rng seed; call from host Tick.
        /// </summary>
        public RaidEvent TryBuildNoiseRaid(int day = -1)
        {
            if (day < 0) day = _getDay != null ? _getDay() : 0;
            if (!IsRaidUnlocked(day)) return null;

            float noise = ExternalNoise;
            if (GeneratorRunningOutside)
                noise = Mathf.Max(noise, 0.85f);

            if (noise < ExternalGeneratorNoiseThreshold) return null;

            // One-in-three chance per check when noisy (caller controls cadence)
            if (_rng.NextDouble() > 0.34 + noise * 0.2) return null;

            return new RaidEvent
            {
                Id = "raid_noise_" + day,
                Trigger = RaidTrigger.Noise,
                Strength = NoiseRaidStrength * (0.75f + noise * 0.5f),
                Day = day,
                Message = "Something heard the generator. Footsteps on the hatch."
            };
        }

        // -----------------------------------------------------------------
        // Raid resolution
        // -----------------------------------------------------------------

        /// <summary>
        /// Resolve a raid: Defense = ShelterSecurity + WeaponPower.
        /// Defense &gt; Raid → repel (ammo/durability cost, morale boost).
        /// Raid &gt; Defense → breach (loot stolen, trauma rolls).
        /// </summary>
        public RaidResolution ResolveRaid(RaidEvent raid, bool ignoreDayGate = false)
        {
            var result = new RaidResolution
            {
                Event = raid,
                Launched = false,
                StolenItems = new List<StolenLootLine>(),
                TraumatizedSurvivorIds = new List<string>()
            };

            if (raid == null)
            {
                result.Message = "No raid event.";
                return result;
            }

            int day = raid.Day > 0 ? raid.Day : (_getDay != null ? _getDay() : 0);
            if (!ignoreDayGate && raid.Trigger != RaidTrigger.Forced && !IsRaidUnlocked(day))
            {
                result.Message = "Pre-Day 30: hatch raids not yet active.";
                return result;
            }

            result.Launched = true;
            result.RaidStrength = Mathf.Max(0f, raid.Strength);
            result.ShelterSecurity = GetShelterSecurity();
            result.GuardBonusApplied = GetGuardBonus();
            result.WeaponPower = GetWeaponPower();
            result.DefenseScore = result.ShelterSecurity + result.WeaponPower;

            // Strict: Defense must exceed raid to repel (equal still breaches under pressure)
            result.Repelled = result.DefenseScore > result.RaidStrength;

            if (result.Repelled)
            {
                ApplyRepelCosts(result);
                result.HatchDamage = 3f + (result.RaidStrength / Mathf.Max(1f, result.DefenseScore)) * 8f;
                result.MoraleDelta = RepelMoraleBoost;
                result.Message = string.IsNullOrEmpty(raid.Message)
                    ? "Hatch held. Brass on the floor, smoke in the stairwell — but they left."
                    : raid.Message + " Held.";
                ApplyMoraleToSurvivors(result.MoraleDelta);
                ApplyHatchWear(result.HatchDamage);
            }
            else
            {
                result.HatchDamage = 15f + (result.RaidStrength - result.DefenseScore) * 0.6f;
                result.MoraleDelta = BreachMoralePenalty;
                result.Message = string.IsNullOrEmpty(raid.Message)
                    ? "Hatch breached. Hands in the stores. Someone is screaming."
                    : raid.Message + " Breached.";
                StealLoot(result);
                RollTrauma(result);
                ApplyMoraleToSurvivors(result.MoraleDelta);
                ApplyHatchWear(result.HatchDamage);
            }

            if (result.Launched)
            {
                LastResolution = result;
                TotalRaidsResolved++;
                if (result.Breached) TotalBreaches++;
                _hoursSinceLastRaid = 0f;
                LastRaidSummary = BuildSummary(result);
            }

            OnRaidResolved?.Invoke(result);
            OnSecurityChanged?.Invoke();
            return result;
        }

        /// <summary>
        /// Convenience: build a faction trust raid and resolve it.
        /// </summary>
        public RaidResolution ResolveFactionRaid(
            string factionId,
            float strength,
            int day = -1,
            bool ignoreDayGate = false)
        {
            if (day < 0) day = _getDay != null ? _getDay() : RaidUnlockDay;
            var evt = new RaidEvent
            {
                Id = "raid_faction_" + (factionId ?? "unknown"),
                FactionId = factionId,
                Trigger = RaidTrigger.FactionTrust,
                Strength = strength,
                Day = day,
                Message = "Hostile faction at the hatch."
            };
            return ResolveRaid(evt, ignoreDayGate);
        }

        private void ApplyRepelCosts(RaidResolution result)
        {
            var inv = _getInventory != null ? _getInventory() : null;
            if (inv?.Slots == null) return;

            // Prefer consuming ammo; fall back to weapon durability
            int ammoNeeded = Mathf.Clamp(Mathf.CeilToInt(result.RaidStrength / 10f), 1, 12);
            int remaining = ammoNeeded;

            for (int i = 0; i < inv.Slots.Count && remaining > 0; i++)
            {
                var slot = inv.Slots[i];
                if (slot?.Item == null) continue;
                if (!IsAmmoItem(slot.Item) && !IsAmmoId(slot.Item.id)) continue;

                int take = Mathf.Min(remaining, slot.Amount);
                if (take <= 0) continue;
                inv.Remove(slot.Item, take);
                remaining -= take;
                result.AmmoConsumed += take;
            }

            // Durability wear on first firearm if ammo short or always light wear
            float wear = 8f + (remaining > 0 ? remaining * 2f : 2f);
            for (int i = 0; i < inv.Slots.Count; i++)
            {
                var slot = inv.Slots[i];
                if (slot?.Item == null) continue;
                if (IsAmmoItem(slot.Item) || IsAmmoId(slot.Item.id)) continue;
                if (!IsWeaponItem(slot.Item) && !IsWeaponId(slot.Item.id)) continue;
                if (slot.Item.id == "kevlar_vest") continue;

                float maxDur = slot.Item.durability > 0f ? slot.Item.durability : 100f;
                if (slot.CurrentDurability < 0f) slot.CurrentDurability = maxDur;
                float before = slot.CurrentDurability;
                slot.CurrentDurability = Mathf.Max(0f, slot.CurrentDurability - wear);
                result.WeaponDurabilityLost += before - slot.CurrentDurability;
                break;
            }
        }

        private void StealLoot(RaidResolution result)
        {
            var inv = _getInventory != null ? _getInventory() : null;
            if (inv?.Slots == null) return;

            // Steal proportional to how badly defense failed
            float deficit = Mathf.Max(1f, result.RaidStrength - result.DefenseScore);
            int stacksToSteal = Mathf.Clamp(Mathf.CeilToInt(deficit / 15f), 1, 6);
            int stolenStacks = 0;

            // Snapshot non-empty slots (steal from end so remove is stable)
            var candidates = new List<int>();
            for (int i = 0; i < inv.Slots.Count; i++)
            {
                var s = inv.Slots[i];
                if (s?.Item != null && s.Amount > 0)
                    candidates.Add(i);
            }

            // Prefer food/water/medical/trade value; skip quest if possible
            candidates.Sort((a, b) =>
            {
                float va = StealPriority(inv.Slots[a].Item);
                float vb = StealPriority(inv.Slots[b].Item);
                return vb.CompareTo(va);
            });

            for (int c = 0; c < candidates.Count && stolenStacks < stacksToSteal; c++)
            {
                var slot = inv.Slots[candidates[c]];
                if (slot?.Item == null || slot.Amount <= 0) continue;
                if (slot.Item.type == ItemType.Quest) continue;

                int take = Mathf.Max(1, Mathf.Min(slot.Amount, Mathf.CeilToInt(slot.Amount * 0.5f)));
                // For high deficit, take whole stack
                if (deficit >= 40f) take = slot.Amount;

                var item = slot.Item;
                string id = item.id;
                string name = item.displayName;
                if (inv.Remove(item, take))
                {
                    result.StolenItems.Add(new StolenLootLine
                    {
                        ItemId = id,
                        Amount = take,
                        DisplayName = name
                    });
                    stolenStacks++;
                }
            }
        }

        private static float StealPriority(ItemDefinition item)
        {
            if (item == null) return 0f;
            float p = item.tradeValue;
            switch (item.type)
            {
                case ItemType.Food: p += 20f; break;
                case ItemType.Water: p += 25f; break;
                case ItemType.Medical: p += 18f; break;
                case ItemType.Fuel: p += 15f; break;
                case ItemType.AntiRad:
                case ItemType.Iodine: p += 12f; break;
            }
            if (IsWeaponItem(item) || IsAmmoItem(item)) p += 10f;
            return p;
        }

        private void RollTrauma(RaidResolution result)
        {
            var survivors = _getSurvivors != null ? _getSurvivors() : null;
            if (survivors == null || survivors.Count == 0) return;

            float deficit = Mathf.Max(0f, result.RaidStrength - result.DefenseScore);
            // Chance scales with breach severity; at least one roll per living survivor
            float chance = Mathf.Clamp01(0.25f + deficit / 100f);

            for (int i = 0; i < survivors.Count; i++)
            {
                var sv = survivors[i];
                if (sv == null || !sv.IsAlive) continue;
                if (_rng.NextDouble() > chance) continue;

                string traumaId = _rng.NextDouble() < 0.5
                    ? "broken_bone"
                    : "gunshot_wound";

                if (_inflictTrauma != null)
                {
                    _inflictTrauma(sv, traumaId);
                }
                else
                {
                    // Fallback: direct health + morale hit when medical not wired
                    sv.Needs.Health = Mathf.Max(1f, sv.Needs.Health - 15f);
                    sv.Needs.Morale = Mathf.Max(0f, sv.Needs.Morale - 10f);
                    if (sv.State == SurvivorState.Idle || sv.State == SurvivorState.Working)
                        sv.State = SurvivorState.Sick;
                }

                result.TraumatizedSurvivorIds.Add(sv.Id);
            }
        }

        private void ApplyMoraleToSurvivors(float delta)
        {
            if (Mathf.Approximately(delta, 0f)) return;
            var survivors = _getSurvivors != null ? _getSurvivors() : null;
            if (survivors == null) return;
            for (int i = 0; i < survivors.Count; i++)
            {
                var sv = survivors[i];
                if (sv?.Needs == null || !sv.IsAlive) continue;
                sv.Needs.Morale = Mathf.Clamp(sv.Needs.Morale + delta, 0f, 100f);
            }
        }

        private void ApplyHatchWear(float damage)
        {
            var shelter = _getShelter != null ? _getShelter() : null;
            if (shelter == null || damage <= 0f) return;

            var air = shelter.GetModule("air_filtration");
            if (air != null)
                air.FilterHealth = Mathf.Max(0f, air.FilterHealth - damage * 0.5f);

            // Blast door / locks take structural wear via Fuel field as integrity proxy if present
            for (int i = 0; i < shelter.Modules.Count; i++)
            {
                var mod = shelter.Modules[i];
                if (mod == null || !IsHatchModuleId(mod.ModuleId)) continue;
                // Optional: degrade contribution slightly by tracking FilterHealth as integrity
                if (mod.FilterHealth > 0f)
                    mod.FilterHealth = Mathf.Max(0f, mod.FilterHealth - damage * 0.25f);
            }

            if (damage >= 40f)
            {
                var shield = shelter.GetModule("radiation_shielding");
                if (shield != null && shield.Level > 0)
                    shield.Level = Mathf.Max(0, shield.Level - 1);
            }
        }

        private static string BuildSummary(RaidResolution result)
        {
            if (result == null || !result.Launched) return "Hatch quiet.";
            if (result.Repelled)
            {
                return $"Repelled (D {result.DefenseScore:0} > R {result.RaidStrength:0})"
                    + (result.AmmoConsumed > 0 ? $", −{result.AmmoConsumed} ammo" : "");
            }

            int stolen = result.StolenItems != null ? result.StolenItems.Count : 0;
            return $"BREACHED (D {result.DefenseScore:0} < R {result.RaidStrength:0}), stole {stolen} stacks";
        }

        // -----------------------------------------------------------------
        // Tick / outdoor generator noise
        // -----------------------------------------------------------------

        /// <summary>
        /// True when a diesel source is running in an outdoor room id.
        /// </summary>
        public static bool IsOutdoorDieselRunning(PowerNetwork power)
        {
            if (power?.Sources == null) return false;
            for (int i = 0; i < power.Sources.Count; i++)
            {
                var src = power.Sources[i];
                if (src == null || !src.IsEnabled || src.Fuel <= 0f) continue;
                bool diesel = (src.Definition != null && src.Definition.Kind == PowerSourceKind.Diesel)
                    || string.Equals(src.SourceId, "diesel_generator", StringComparison.Ordinal);
                if (!diesel) continue;
                if (IsOutdoorRoomId(src.RoomId)) return true;
            }
            return false;
        }

        public static bool IsOutdoorRoomId(string roomId)
        {
            if (string.IsNullOrEmpty(roomId)) return false;
            return string.Equals(roomId, OutdoorRoomId, StringComparison.OrdinalIgnoreCase)
                || string.Equals(roomId, "exterior", StringComparison.OrdinalIgnoreCase)
                || string.Equals(roomId, "yard", StringComparison.OrdinalIgnoreCase)
                || string.Equals(roomId, "surface", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Sync outdoor-generator noise from the power grid. Call each host tick.
        /// </summary>
        public void SyncGeneratorNoise(PowerNetwork power)
        {
            bool outdoor = IsOutdoorDieselRunning(power);
            if (outdoor)
            {
                GeneratorRunningOutside = true;
                ExternalNoise = Mathf.Max(ExternalNoise, 0.85f);
            }
            else if (GeneratorRunningOutside && power != null)
            {
                // Manual flag still counts as noise if set without outdoor room
                ExternalNoise = Mathf.Max(ExternalNoise, 0.85f);
            }
        }

        /// <summary>
        /// Advance noise decay + periodic outdoor raid rolls. Returns a resolution
        /// if a noise raid fired this tick; otherwise null.
        /// </summary>
        public RaidResolution Tick(float gameHours, PowerNetwork power = null)
        {
            if (gameHours <= 0f) return null;

            _hoursSinceLastRaid += gameHours;
            SyncGeneratorNoise(power);

            // Noise decays when the outdoor gen is off
            if (!GeneratorRunningOutside && !IsOutdoorDieselRunning(power))
            {
                ExternalNoise = Mathf.Max(0f, ExternalNoise - 0.08f * gameHours);
            }

            int day = _getDay != null ? _getDay() : 0;
            if (!IsRaidUnlocked(day)) return null;
            if (_hoursSinceLastRaid < RaidCooldownHours) return null;

            _noiseCheckAccumulator += gameHours;
            if (_noiseCheckAccumulator < NoiseCheckIntervalHours) return null;
            _noiseCheckAccumulator = 0f;

            var evt = TryBuildNoiseRaid(day);
            if (evt == null) return null;
            return ResolveRaid(evt);
        }

        // -----------------------------------------------------------------
        // Hatch upgrade install (resource sink)
        // -----------------------------------------------------------------

        /// <summary>
        /// Material cost to install or level-up a hatch defense module.
        /// Uses scrap_metal + mechanical_parts (or mechanical_components alias).
        /// </summary>
        public static void GetUpgradeMaterialCost(string moduleId, int targetLevel, out int scrap, out int mechanical)
        {
            int tier = Mathf.Max(1, targetLevel);
            scrap = 0;
            mechanical = 0;
            if (moduleId == HatchDefenseModuleSO.ReinforcedLocksId)
            {
                scrap = 3 + tier;
                mechanical = 1 + tier / 2;
            }
            else if (moduleId == HatchDefenseModuleSO.BlastDoorId)
            {
                scrap = 6 + tier * 2;
                mechanical = 2 + tier;
            }
            else if (moduleId == HatchDefenseModuleSO.HatchTrapsId)
            {
                scrap = 2 + tier;
                mechanical = 2 + tier;
            }
            else
            {
                scrap = 4;
                mechanical = 2;
            }
        }

        /// <summary>
        /// Install a hatch upgrade or raise its level, consuming scrap + mechanical parts.
        /// </summary>
        public bool TryInstallHatchUpgrade(
            string moduleId,
            Func<string, ItemDefinition> itemLookup,
            Inventory.Inventory inventory = null)
        {
            if (string.IsNullOrEmpty(moduleId) || !IsHatchModuleId(moduleId)) return false;
            var inv = inventory ?? (_getInventory != null ? _getInventory() : null);
            var shelter = _getShelter != null ? _getShelter() : null;
            if (inv == null || shelter == null || itemLookup == null) return false;

            var existing = shelter.GetModule(moduleId);
            int targetLevel = existing != null ? existing.Level + 1 : 1;
            if (existing != null && existing.Definition != null && targetLevel > existing.Definition.MaxLevel)
                return false;
            if (targetLevel > 5) return false;

            GetUpgradeMaterialCost(moduleId, targetLevel, out int scrapNeed, out int mechNeed);
            var scrap = itemLookup("scrap_metal");
            var mech = itemLookup("mechanical_parts") ?? itemLookup("mechanical_components");
            if (scrap == null || mech == null) return false;
            if (inv.Count(scrap) < scrapNeed || inv.Count(mech) < mechNeed) return false;

            inv.Remove(scrap, scrapNeed);
            inv.Remove(mech, mechNeed);

            if (existing != null)
            {
                existing.Level = targetLevel;
                if (existing.SecurityContribution <= 0f)
                    existing.SecurityContribution = DefaultSecurityForModuleId(moduleId);
                existing.FilterHealth = 100f;
                existing.IsEnabled = true;
            }
            else
            {
                shelter.AddModule(new ShelterModuleInstance(moduleId, targetLevel)
                {
                    SecurityContribution = DefaultSecurityForModuleId(moduleId),
                    FilterHealth = 100f,
                    IsEnabled = true,
                    RoomId = "entry"
                });
            }

            OnSecurityChanged?.Invoke();
            return true;
        }

        public bool CanInstallHatchUpgrade(
            string moduleId,
            Func<string, ItemDefinition> itemLookup,
            Inventory.Inventory inventory = null)
        {
            if (string.IsNullOrEmpty(moduleId) || !IsHatchModuleId(moduleId)) return false;
            var inv = inventory ?? (_getInventory != null ? _getInventory() : null);
            var shelter = _getShelter != null ? _getShelter() : null;
            if (inv == null || shelter == null || itemLookup == null) return false;

            var existing = shelter.GetModule(moduleId);
            int targetLevel = existing != null ? existing.Level + 1 : 1;
            if (targetLevel > 5) return false;

            GetUpgradeMaterialCost(moduleId, targetLevel, out int scrapNeed, out int mechNeed);
            var scrap = itemLookup("scrap_metal");
            var mech = itemLookup("mechanical_parts") ?? itemLookup("mechanical_components");
            if (scrap == null || mech == null) return false;
            return inv.Count(scrap) >= scrapNeed && inv.Count(mech) >= mechNeed;
        }

        // -----------------------------------------------------------------
        // Save / load
        // -----------------------------------------------------------------

        public HatchDefenseSave CaptureState()
        {
            return new HatchDefenseSave
            {
                ExternalNoise = ExternalNoise,
                GeneratorRunningOutside = GeneratorRunningOutside,
                HoursSinceLastRaid = _hoursSinceLastRaid,
                NoiseCheckAccumulator = _noiseCheckAccumulator,
                TotalRaidsResolved = TotalRaidsResolved,
                TotalBreaches = TotalBreaches,
                LastRaidSummary = LastRaidSummary ?? "Hatch quiet.",
                LastRaidStrength = LastResolution != null ? LastResolution.RaidStrength : 0f,
                LastDefenseScore = LastResolution != null ? LastResolution.DefenseScore : 0f,
                LastRepelled = LastResolution != null && LastResolution.Repelled,
                LastBreached = LastResolution != null && LastResolution.Breached
            };
        }

        public void RestoreState(HatchDefenseSave save)
        {
            if (save == null) return;
            ExternalNoise = Mathf.Clamp01(save.ExternalNoise);
            GeneratorRunningOutside = save.GeneratorRunningOutside;
            _hoursSinceLastRaid = Mathf.Max(0f, save.HoursSinceLastRaid);
            _noiseCheckAccumulator = Mathf.Max(0f, save.NoiseCheckAccumulator);
            TotalRaidsResolved = Mathf.Max(0, save.TotalRaidsResolved);
            TotalBreaches = Mathf.Max(0, save.TotalBreaches);
            LastRaidSummary = string.IsNullOrEmpty(save.LastRaidSummary)
                ? "Hatch quiet."
                : save.LastRaidSummary;

            if (save.LastRaidStrength > 0f || save.LastBreached || save.LastRepelled)
            {
                LastResolution = new RaidResolution
                {
                    Launched = true,
                    Repelled = save.LastRepelled,
                    RaidStrength = save.LastRaidStrength,
                    DefenseScore = save.LastDefenseScore,
                    Message = LastRaidSummary
                };
            }

            OnSecurityChanged?.Invoke();
        }
    }

    /// <summary>Serializable hatch defense snapshot.</summary>
    [Serializable]
    public class HatchDefenseSave
    {
        public float ExternalNoise;
        public bool GeneratorRunningOutside;
        public float HoursSinceLastRaid;
        public float NoiseCheckAccumulator;
        public int TotalRaidsResolved;
        public int TotalBreaches;
        public string LastRaidSummary;
        public float LastRaidStrength;
        public float LastDefenseScore;
        public bool LastRepelled;
        public bool LastBreached;
    }
}
