using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Inventory
{
    /// <summary>
    /// Legacy coarse ammo category (kept for older callers / saves).
    /// Prefer <see cref="BulletModification"/> + caliber ids for new code.
    /// </summary>
    public enum AmmoType
    {
        Standard,
        ArmorPiercing,
        HollowPoint
    }

    /// <summary>Weapon class a caliber is chambered for.</summary>
    public enum WeaponAmmoClass
    {
        Pistol,
        Smg,
        Shotgun,
        Rifle,
        BattleRifle,
        Sniper,
        Lmg,
        Pdw,
        AntiMateriel
    }

    /// <summary>Loot / craft rarity ladder for ammunition.</summary>
    public enum AmmoRarity
    {
        Common,
        Uncommon,
        Rare,
        VeryRare,
        MythicRare,
        LegendaryVeryRare
    }

    /// <summary>
    /// Bullet construction / tactical load. Civilian craft uses SoftLead / Fmj / Jhp.
    /// Military &amp; rebel exclusives use Ap / Api / M855A1 / BoatTail and dual-attribute
    /// battle-rifle / sniper / AV loads (JhpAp, ExplosiveIncendiary, Api).
    /// </summary>
    public enum BulletModification
    {
        /// <summary>Simple cast lead — early craft baseline.</summary>
        SoftLead = 0,
        /// <summary>Full metal jacket — balanced kinetic round.</summary>
        Fmj = 1,
        /// <summary>Jacketed hollow point — soft-target dump, fails on armor.</summary>
        Jhp = 2,
        /// <summary>Armor-piercing hardened core — non-craftable military/rebel.</summary>
        Ap = 3,
        /// <summary>Armor-piercing + incendiary (dual) — AP + burn + area light.</summary>
        Api = 4,
        /// <summary>Steel/tungsten tip + copper slug — soft and hard targets (M855A1 tier).</summary>
        M855A1 = 5,
        /// <summary>Boat-tail aerodynamic profile — long-range accuracy.</summary>
        BoatTail = 6,
        /// <summary>Hollow-point + armor-piercing (dual) — soft dump and partial plate defeat.</summary>
        JhpAp = 7,
        /// <summary>Explosive + incendiary (dual) — splash/barrier crack + burn + light.</summary>
        ExplosiveIncendiary = 8
    }

    /// <summary>
    /// Attribute flags on a load. Dual-attribute military loads always carry two bits.
    /// </summary>
    [Flags]
    public enum BulletAttributeFlags
    {
        None = 0,
        HollowPoint = 1 << 0,
        ArmorPiercing = 1 << 1,
        Explosive = 1 << 2,
        Incendiary = 1 << 3,
        SoftLead = 1 << 4,
        FullMetalJacket = 1 << 5,
        BoatTail = 1 << 6,
        SteelTip = 1 << 7
    }

    /// <summary>Faction sources that can field exclusive military/rebel loads.</summary>
    public enum AmmoFactionSource
    {
        CivilianCraft = 0,
        RebelForces = 1,
        MercenaryRebel = 2,
        MercenaryMilitary = 3,
        MilitaryForces = 4,
        BlackOpsMilitary = 5,
        SpecOpsRebel = 6
    }

    /// <summary>Serializable save blob (catalog is static; state holds legacy item ids).</summary>
    [Serializable]
    public class AmmoTypeState
    {
        public string itemIdStandard = "item_ammo_standard";
        public string itemIdAP = "item_ammo_ap";
        public string itemIdHP = "item_ammo_hp";
    }

    /// <summary>One caliber + modification loadout (snake_case item id).</summary>
    [Serializable]
    public class AmmoLoadDefinition
    {
        public string Id;
        public string DisplayName;
        public string CaliberId;
        public string CaliberDisplay;
        public WeaponAmmoClass WeaponClass;
        public AmmoRarity Rarity;
        public BulletModification Modification;
        public bool Craftable;
        public float BaseDamage;
        public float EffectiveRangeMeters;
        public float WeightPerRound;
        public int StackMax;
        public float TradeValue;
        public string Description;
        /// <summary>Faction sources allowed to spawn/loot this load (empty = any / civilian).</summary>
        public AmmoFactionSource[] ExclusiveSources = Array.Empty<AmmoFactionSource>();
        /// <summary>Additional weapon classes that can chamber this caliber (e.g. .45 ACP pistol+SMG).</summary>
        public WeaponAmmoClass[] AlsoFits = Array.Empty<WeaponAmmoClass>();
    }

    /// <summary>Resolved combat numbers for a single hit.</summary>
    public struct AmmoHitResult
    {
        public float FinalDamage;
        public float DamageMultiplier;
        public float ArmorIgnored;
        public float ArmorRemainingAfterIgnore;
        public float BarrierDamageKept;
        public float EffectiveRangeMeters;
        public float BurnDamagePerSecond;
        public float BurnDurationSeconds;
        public bool LightsArea;
        public bool SoftTargetBonusApplied;
        public bool ArmorPenaltyApplied;
        public BulletModification Modification;
        public BulletAttributeFlags Attributes;
        public bool IsDualAttribute;
        public bool HasExplosive;
        public float ExplosiveSplashFraction;
        public string AmmoItemId;
    }

    /// <summary>
    /// Progressive caliber + bullet-mod catalog for ASHFALL.
    /// Civilian craft: soft lead / FMJ / JHP on common–uncommon calibers.
    /// Military/rebel exclusives: AP, API, M855A1, boat-tail, battle/sniper/AV tiers.
    /// </summary>
    public partial class Item_AmmoTypes
    {
        public const float JhpSoftTargetBonus = 0.50f;       // +50% vs unarmored
        public const float JhpArmoredPenalty = 0.80f;        // −80% vs armor
        public const float ApArmorIgnore = 0.75f;            // ignore 75% of armor
        public const float SoftLeadBarrierRetain = 0.25f;
        public const float FmjBarrierRetain = 0.45f;
        public const float JhpBarrierRetain = 0.20f;
        public const float ApBarrierRetain = 0.85f;
        public const float ApiBarrierRetain = 0.80f;
        public const float M855A1BarrierRetain = 0.80f;
        public const float BoatTailBarrierRetain = 0.55f;
        public const float M855A1SoftBonus = 0.25f;
        public const float M855A1ArmorIgnore = 0.55f;
        public const float BoatTailRangeMul = 1.35f;
        public const float ApiBurnDps = 4f;
        public const float ApiBurnDurationSeconds = 6f;
        /// <summary>JHP+AP dual: reduced soft bonus (both attributes present).</summary>
        public const float JhpApSoftBonus = 0.30f;
        /// <summary>JHP+AP dual: partial armor ignore (both attributes present).</summary>
        public const float JhpApArmorIgnore = 0.50f;
        public const float JhpApBarrierRetain = 0.65f;
        /// <summary>Explosive+Incendiary dual: splash fraction of final damage (barrier/soft).</summary>
        public const float ExiExplosiveSplashFraction = 0.35f;
        public const float ExiBarrierRetain = 0.70f;
        public const float ExiBurnDps = 5f;
        public const float ExiBurnDurationSeconds = 5f;

        public event Action<string, AmmoType, float> OnDamageModified;
        public event Action<string, AmmoHitResult> OnAmmoHitResolved;

        private readonly AmmoTypeState _state;
        private static readonly Dictionary<string, AmmoLoadDefinition> Catalog =
            new Dictionary<string, AmmoLoadDefinition>(StringComparer.Ordinal);
        private static readonly Dictionary<string, string> LegacyAliases =
            new Dictionary<string, string>(StringComparer.Ordinal);
        private static bool _catalogBuilt;

        private const string TARGET_MUTANT = "mutant";
        private const string TARGET_ANIMAL = "animal";
        private const string TARGET_WARLORD = "warlord";

        public Item_AmmoTypes()
        {
            _state = new AmmoTypeState();
            EnsureCatalog();
        }

        public static IReadOnlyDictionary<string, AmmoLoadDefinition> AllLoads
        {
            get
            {
                EnsureCatalog();
                return Catalog;
            }
        }

        public static bool TryGetLoad(string itemId, out AmmoLoadDefinition load)
        {
            EnsureCatalog();
            load = null;
            if (string.IsNullOrEmpty(itemId)) return false;
            if (LegacyAliases.TryGetValue(itemId, out var alias))
                itemId = alias;
            return Catalog.TryGetValue(itemId, out load);
        }

        public static bool IsAmmoItemId(string id)
        {
            if (string.IsNullOrEmpty(id)) return false;
            EnsureCatalog();
            if (LegacyAliases.ContainsKey(id) || Catalog.ContainsKey(id)) return true;
            return id == "handgun_ammo" || id == "shotgun_shells"
                || id.EndsWith("_ammo", StringComparison.Ordinal)
                || id.EndsWith("_shells", StringComparison.Ordinal)
                || id.StartsWith("ammo_", StringComparison.Ordinal);
        }

        public static bool IsCraftable(string itemId)
        {
            return TryGetLoad(itemId, out var load) && load.Craftable;
        }

        public static bool IsExclusiveToFactions(string itemId)
        {
            return TryGetLoad(itemId, out var load)
                && load.ExclusiveSources != null
                && load.ExclusiveSources.Length > 0
                && !(load.ExclusiveSources.Length == 1
                     && load.ExclusiveSources[0] == AmmoFactionSource.CivilianCraft);
        }

        public static bool CanFactionField(string itemId, AmmoFactionSource faction)
        {
            if (!TryGetLoad(itemId, out var load)) return false;
            if (load.ExclusiveSources == null || load.ExclusiveSources.Length == 0)
                return true;
            for (int i = 0; i < load.ExclusiveSources.Length; i++)
            {
                if (load.ExclusiveSources[i] == faction
                    || load.ExclusiveSources[i] == AmmoFactionSource.CivilianCraft)
                    return true;
            }
            return false;
        }

        public static bool FitsWeaponClass(string itemId, WeaponAmmoClass weaponClass)
        {
            if (!TryGetLoad(itemId, out var load)) return false;
            if (load.WeaponClass == weaponClass) return true;
            if (load.AlsoFits == null) return false;
            for (int i = 0; i < load.AlsoFits.Length; i++)
                if (load.AlsoFits[i] == weaponClass) return true;
            return false;
        }

        /// <summary>Attribute flags for a modification (dual loads always return two bits).</summary>
        public static BulletAttributeFlags GetAttributes(BulletModification mod)
        {
            switch (mod)
            {
                case BulletModification.SoftLead:
                    return BulletAttributeFlags.SoftLead;
                case BulletModification.Fmj:
                    return BulletAttributeFlags.FullMetalJacket;
                case BulletModification.Jhp:
                    return BulletAttributeFlags.HollowPoint;
                case BulletModification.Ap:
                    return BulletAttributeFlags.ArmorPiercing;
                case BulletModification.Api:
                    return BulletAttributeFlags.ArmorPiercing | BulletAttributeFlags.Incendiary;
                case BulletModification.M855A1:
                    return BulletAttributeFlags.SteelTip | BulletAttributeFlags.ArmorPiercing;
                case BulletModification.BoatTail:
                    return BulletAttributeFlags.BoatTail;
                case BulletModification.JhpAp:
                    return BulletAttributeFlags.HollowPoint | BulletAttributeFlags.ArmorPiercing;
                case BulletModification.ExplosiveIncendiary:
                    return BulletAttributeFlags.Explosive | BulletAttributeFlags.Incendiary;
                default:
                    return BulletAttributeFlags.None;
            }
        }

        public static bool IsDualAttributeModification(BulletModification mod)
        {
            return mod == BulletModification.JhpAp
                || mod == BulletModification.ExplosiveIncendiary
                || mod == BulletModification.Api;
        }

        public static int CountAttributes(BulletAttributeFlags flags)
        {
            int n = 0;
            int v = (int)flags;
            while (v != 0)
            {
                n += v & 1;
                v >>= 1;
            }
            return n;
        }

        /// <summary>
        /// True for battle-rifle / sniper / anti-materiel dual-attribute military loads
        /// (JHP+AP, Explosive+Incendiary, AP+Incendiary).
        /// </summary>
        public static bool IsDualAttributeTacticalLoad(string itemId)
        {
            return TryGetLoad(itemId, out var load) && IsDualAttributeModification(load.Modification);
        }

        public static IReadOnlyList<AmmoLoadDefinition> GetLoadsForClass(WeaponAmmoClass weaponClass)
        {
            EnsureCatalog();
            var list = new List<AmmoLoadDefinition>();
            foreach (var kv in Catalog)
            {
                if (FitsWeaponClass(kv.Key, weaponClass))
                    list.Add(kv.Value);
            }
            return list;
        }

        public static IReadOnlyList<AmmoLoadDefinition> GetCraftableLoads()
        {
            EnsureCatalog();
            var list = new List<AmmoLoadDefinition>();
            foreach (var kv in Catalog)
                if (kv.Value.Craftable) list.Add(kv.Value);
            return list;
        }

        /// <summary>
        /// Ballistic combat resolution: soft/armor trade-off, barrier mass retention,
        /// range, and API burn/light effects.
        /// </summary>
        public AmmoHitResult ResolveHit(
            string ammoItemId,
            float baseDamage,
            float targetArmor,
            bool behindBarrier = false,
            float rangeMeters = 0f)
        {
            EnsureCatalog();
            var result = new AmmoHitResult
            {
                AmmoItemId = ammoItemId ?? string.Empty,
                FinalDamage = baseDamage,
                DamageMultiplier = 1f,
                BarrierDamageKept = 1f,
                EffectiveRangeMeters = 50f
            };

            if (!TryGetLoad(ammoItemId, out var load))
            {
                // Unknown id: treat as soft FMJ civilian.
                result.FinalDamage = ApplyArmorReduction(baseDamage, targetArmor, 0f);
                return result;
            }

            result.Modification = load.Modification;
            result.Attributes = GetAttributes(load.Modification);
            result.IsDualAttribute = IsDualAttributeModification(load.Modification);
            result.EffectiveRangeMeters = GetEffectiveRange(load);

            float mul = 1f;
            float armorIgnore = 0f;
            float barrierRetain = FmjBarrierRetain;
            bool armored = targetArmor > 0.01f;

            switch (load.Modification)
            {
                case BulletModification.Jhp:
                    if (!armored)
                    {
                        mul += JhpSoftTargetBonus;
                        result.SoftTargetBonusApplied = true;
                    }
                    else
                    {
                        mul -= JhpArmoredPenalty;
                        result.ArmorPenaltyApplied = true;
                    }
                    barrierRetain = JhpBarrierRetain;
                    break;

                case BulletModification.Ap:
                    armorIgnore = ApArmorIgnore;
                    barrierRetain = ApBarrierRetain;
                    break;

                case BulletModification.Api:
                    // Dual: armour-piercing + incendiary.
                    armorIgnore = ApArmorIgnore;
                    barrierRetain = ApiBarrierRetain;
                    result.BurnDamagePerSecond = ApiBurnDps;
                    result.BurnDurationSeconds = ApiBurnDurationSeconds;
                    result.LightsArea = true;
                    break;

                case BulletModification.JhpAp:
                    // Dual: hollow-point + armour-piercing (both always apply).
                    if (!armored)
                    {
                        mul += JhpApSoftBonus;
                        result.SoftTargetBonusApplied = true;
                    }
                    armorIgnore = JhpApArmorIgnore;
                    barrierRetain = JhpApBarrierRetain;
                    break;

                case BulletModification.ExplosiveIncendiary:
                    // Dual: explosive splash + incendiary burn/light.
                    result.HasExplosive = true;
                    result.ExplosiveSplashFraction = ExiExplosiveSplashFraction;
                    mul += ExiExplosiveSplashFraction;
                    barrierRetain = ExiBarrierRetain;
                    result.BurnDamagePerSecond = ExiBurnDps;
                    result.BurnDurationSeconds = ExiBurnDurationSeconds;
                    result.LightsArea = true;
                    break;

                case BulletModification.M855A1:
                    mul += M855A1SoftBonus;
                    armorIgnore = M855A1ArmorIgnore;
                    barrierRetain = M855A1BarrierRetain;
                    result.SoftTargetBonusApplied = true;
                    break;

                case BulletModification.BoatTail:
                    barrierRetain = BoatTailBarrierRetain;
                    break;

                case BulletModification.SoftLead:
                    barrierRetain = SoftLeadBarrierRetain;
                    if (armored)
                    {
                        mul -= 0.40f; // soft lead mushrooms on hard targets
                        result.ArmorPenaltyApplied = true;
                    }
                    break;

                case BulletModification.Fmj:
                default:
                    barrierRetain = FmjBarrierRetain;
                    break;
            }

            // Range falloff beyond effective range (boat-tail extends that range).
            if (rangeMeters > result.EffectiveRangeMeters && result.EffectiveRangeMeters > 0f)
            {
                float over = (rangeMeters - result.EffectiveRangeMeters) / result.EffectiveRangeMeters;
                mul *= Mathf.Clamp(1f - over * 0.5f, 0.35f, 1f);
            }

            if (behindBarrier)
            {
                result.BarrierDamageKept = barrierRetain;
                mul *= barrierRetain;
            }

            result.DamageMultiplier = Mathf.Max(0f, mul);
            result.ArmorIgnored = armorIgnore;
            float armorAfter = targetArmor * (1f - armorIgnore);
            result.ArmorRemainingAfterIgnore = armorAfter;

            float raw = baseDamage * result.DamageMultiplier;
            result.FinalDamage = ApplyArmorReduction(raw, armorAfter, 0f);

            OnAmmoHitResolved?.Invoke(ammoItemId, result);
            return result;
        }

        public static float GetEffectiveRange(AmmoLoadDefinition load)
        {
            if (load == null) return 50f;
            float r = load.EffectiveRangeMeters;
            if (load.Modification == BulletModification.BoatTail)
                r *= BoatTailRangeMul;
            return r;
        }

        private static float ApplyArmorReduction(float damage, float armor, float ignoreAlreadyApplied)
        {
            // Simple soak: each armor point reduces damage by 0.5, floored at 5% of raw.
            float soak = Mathf.Max(0f, armor) * 0.5f;
            return Mathf.Max(damage * 0.05f, damage - soak);
        }

        /// <summary>
        /// Legacy API: AP ignores Kevlar but 0.5x vs Mutants; HP 2x unarmored mutant/animal, 0 vs armored warlord.
        /// </summary>
        public float GetDamageMultiplier(AmmoType ammo, string targetType, bool hasKevlar)
        {
            float multiplier;
            switch (ammo)
            {
                case AmmoType.ArmorPiercing:
                    multiplier = string.Equals(targetType, TARGET_MUTANT, StringComparison.OrdinalIgnoreCase)
                        ? 0.5f : 1.0f;
                    break;
                case AmmoType.HollowPoint:
                    if (!hasKevlar && (
                        string.Equals(targetType, TARGET_MUTANT, StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(targetType, TARGET_ANIMAL, StringComparison.OrdinalIgnoreCase)))
                        multiplier = 2.0f;
                    else if (hasKevlar && string.Equals(targetType, TARGET_WARLORD, StringComparison.OrdinalIgnoreCase))
                        multiplier = 0.0f;
                    else
                        multiplier = 1.0f;
                    break;
                case AmmoType.Standard:
                default:
                    multiplier = 1.0f;
                    break;
            }
            return multiplier;
        }

        public float ApplyDamage(string targetId, AmmoType ammo, string targetType, bool hasKevlar, float baseDamage)
        {
            float multiplier = GetDamageMultiplier(ammo, targetType, hasKevlar);
            OnDamageModified?.Invoke(targetId, ammo, multiplier);
            return baseDamage * multiplier;
        }

        public string GetAmmoId(AmmoType type)
        {
            switch (type)
            {
                case AmmoType.ArmorPiercing: return _state.itemIdAP;
                case AmmoType.HollowPoint: return _state.itemIdHP;
                case AmmoType.Standard:
                default: return _state.itemIdStandard;
            }
        }

        public static BulletModification ToModification(AmmoType legacy)
        {
            switch (legacy)
            {
                case AmmoType.ArmorPiercing: return BulletModification.Ap;
                case AmmoType.HollowPoint: return BulletModification.Jhp;
                default: return BulletModification.Fmj;
            }
        }

        public AmmoTypeState CaptureState()
        {
            return new AmmoTypeState
            {
                itemIdStandard = _state.itemIdStandard,
                itemIdAP = _state.itemIdAP,
                itemIdHP = _state.itemIdHP
            };
        }

        public void RestoreState(AmmoTypeState saved)
        {
            if (saved == null) return;
            _state.itemIdStandard = saved.itemIdStandard;
            _state.itemIdAP = saved.itemIdAP;
            _state.itemIdHP = saved.itemIdHP;
        }
    }
}
