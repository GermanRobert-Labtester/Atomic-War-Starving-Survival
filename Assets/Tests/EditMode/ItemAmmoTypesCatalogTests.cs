using System.Collections.Generic;
using NUnit.Framework;
using AtomicWar._Game.Core;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Shelter;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Caliber catalog + bullet-mod combat axes: craftable civilian vs military/rebel exclusives,
    /// JHP soft/armor trade-off, AP ignore, API burn, M855A1 dual-role, boat-tail range.
    /// </summary>
    [TestFixture]
    public class ItemAmmoTypesCatalogTests
    {
        private const float Eps = 0.01f;
        private Item_AmmoTypes _ammo;

        [SetUp]
        public void SetUp()
        {
            _ammo = new Item_AmmoTypes();
        }

        [Test]
        public void Catalog_Contains_RequiredCalibers()
        {
            string[] required =
            {
                // Handgun
                "ammo_9x19_fmj", "ammo_380acp_fmj", "ammo_762x25_fmj", "ammo_45acp_fmj",
                // SMG
                "ammo_9x21_fmj", "ammo_765x21_fmj",
                // Shotgun
                "ammo_12ga_buck", "ammo_16ga_buck",
                // Rifle
                "ammo_556x45_fmj", "ammo_762x39_fmj",
                // Battle rifle exclusives
                "ammo_545x39_fmj", "ammo_762x51_fmj", "ammo_300blk_fmj", "ammo_57x28_fmj",
                // Sniper
                "ammo_762x54r_bt", "ammo_338lapua_bt",
                // AV
                "ammo_408cheytac_bt", "ammo_50bmg_ap",
                // PDW late
                "ammo_46x30_fmj"
            };

            foreach (var id in required)
            {
                Assert.IsTrue(Item_AmmoTypes.TryGetLoad(id, out var load), id);
                Assert.AreEqual(id, load.Id);
            }
        }

        [Test]
        public void BattleRifleExclusives_AreVeryRare_NonCraftable_FactionLocked()
        {
            string[] br =
            {
                "ammo_545x39_fmj", "ammo_762x51_fmj", "ammo_300blk_fmj", "ammo_57x28_fmj"
            };

            foreach (var id in br)
            {
                Assert.IsTrue(Item_AmmoTypes.TryGetLoad(id, out var load), id);
                Assert.AreEqual(WeaponAmmoClass.BattleRifle, load.WeaponClass, id);
                Assert.AreEqual(AmmoRarity.VeryRare, load.Rarity, id);
                Assert.IsFalse(load.Craftable, id + " must not be craftable");
                Assert.IsTrue(Item_AmmoTypes.IsExclusiveToFactions(id), id);
                Assert.IsTrue(Item_AmmoTypes.CanFactionField(id, AmmoFactionSource.RebelForces), id);
                Assert.IsTrue(Item_AmmoTypes.CanFactionField(id, AmmoFactionSource.MilitaryForces), id);
                Assert.IsTrue(Item_AmmoTypes.CanFactionField(id, AmmoFactionSource.MercenaryRebel), id);
                Assert.IsTrue(Item_AmmoTypes.CanFactionField(id, AmmoFactionSource.MercenaryMilitary), id);
                Assert.IsTrue(Item_AmmoTypes.CanFactionField(id, AmmoFactionSource.BlackOpsMilitary), id);
                Assert.IsTrue(Item_AmmoTypes.CanFactionField(id, AmmoFactionSource.SpecOpsRebel), id);
                Assert.IsFalse(Item_AmmoTypes.CanFactionField(id, AmmoFactionSource.CivilianCraft), id);
            }
        }

        [Test]
        public void CivilianLoads_AreCraftable_AndNotFactionLocked()
        {
            string[] civilian =
            {
                "ammo_9x19_fmj", "ammo_9x19_jhp", "ammo_12ga_buck", "ammo_45acp_jhp",
                "ammo_762x25_fmj", "ammo_16ga_buck", "ammo_556x45_fmj"
            };

            foreach (var id in civilian)
            {
                Assert.IsTrue(Item_AmmoTypes.IsCraftable(id), id);
                Assert.IsFalse(Item_AmmoTypes.IsExclusiveToFactions(id), id);
                Assert.IsTrue(Item_AmmoTypes.CanFactionField(id, AmmoFactionSource.CivilianCraft), id);
            }
        }

        [Test]
        public void MilitaryMods_Ap_Api_M855A1_AreNonCraftable()
        {
            Assert.IsFalse(Item_AmmoTypes.IsCraftable("ammo_556x45_ap"));
            Assert.IsFalse(Item_AmmoTypes.IsCraftable("ammo_556x45_m855a1"));
            Assert.IsFalse(Item_AmmoTypes.IsCraftable("ammo_762x51_api"));
            Assert.IsFalse(Item_AmmoTypes.IsCraftable("ammo_50bmg_api"));
            Assert.IsFalse(Item_AmmoTypes.IsCraftable("ammo_338lapua_bt"));
        }

        [Test]
        public void Jhp_SoftTargetBonus_And_ArmorPenalty()
        {
            float baseDmg = 100f;

            var soft = _ammo.ResolveHit("ammo_9x19_jhp", baseDmg, targetArmor: 0f);
            Assert.IsTrue(soft.SoftTargetBonusApplied);
            Assert.AreEqual(1f + Item_AmmoTypes.JhpSoftTargetBonus, soft.DamageMultiplier, Eps);
            Assert.AreEqual(baseDmg * 1.5f, soft.FinalDamage, Eps);

            var hard = _ammo.ResolveHit("ammo_9x19_jhp", baseDmg, targetArmor: 20f);
            Assert.IsTrue(hard.ArmorPenaltyApplied);
            Assert.AreEqual(1f - Item_AmmoTypes.JhpArmoredPenalty, hard.DamageMultiplier, Eps);
            // 100 * 0.2 = 20 raw, then armor soak on remaining armor
            Assert.Less(hard.FinalDamage, soft.FinalDamage);
            Assert.Less(hard.FinalDamage, baseDmg * 0.25f + 1f);
        }

        [Test]
        public void Ap_Ignores_SeventyFive_Percent_Armor()
        {
            float baseDmg = 100f;
            float armor = 40f;

            var fmj = _ammo.ResolveHit("ammo_556x45_fmj", baseDmg, armor);
            var ap = _ammo.ResolveHit("ammo_556x45_ap", baseDmg, armor);

            Assert.AreEqual(Item_AmmoTypes.ApArmorIgnore, ap.ArmorIgnored, Eps);
            Assert.AreEqual(armor * (1f - Item_AmmoTypes.ApArmorIgnore), ap.ArmorRemainingAfterIgnore, Eps);
            Assert.Greater(ap.FinalDamage, fmj.FinalDamage,
                "AP should deal more damage through armor than FMJ");
        }

        [Test]
        public void Api_Adds_Burn_And_Lights_Area()
        {
            var hit = _ammo.ResolveHit("ammo_50bmg_api", 70f, targetArmor: 30f);
            Assert.AreEqual(BulletModification.Api, hit.Modification);
            Assert.AreEqual(Item_AmmoTypes.ApiBurnDps, hit.BurnDamagePerSecond, Eps);
            Assert.AreEqual(Item_AmmoTypes.ApiBurnDurationSeconds, hit.BurnDurationSeconds, Eps);
            Assert.IsTrue(hit.LightsArea);
            Assert.AreEqual(Item_AmmoTypes.ApArmorIgnore, hit.ArmorIgnored, Eps);
        }

        [Test]
        public void M855A1_SoftBonus_And_ArmorIgnore_Without_JhpTradeoff()
        {
            var soft = _ammo.ResolveHit("ammo_556x45_m855a1", 100f, targetArmor: 0f);
            Assert.IsTrue(soft.SoftTargetBonusApplied);
            Assert.Greater(soft.DamageMultiplier, 1f);

            var hard = _ammo.ResolveHit("ammo_556x45_m855a1", 100f, targetArmor: 40f);
            Assert.AreEqual(Item_AmmoTypes.M855A1ArmorIgnore, hard.ArmorIgnored, Eps);
            Assert.IsFalse(hard.ArmorPenaltyApplied,
                "M855A1 must not take the JHP armored penalty");
            Assert.Greater(hard.FinalDamage, _ammo.ResolveHit("ammo_556x45_jhp", 100f, 40f).FinalDamage);
        }

        [Test]
        public void BoatTail_Extends_EffectiveRange()
        {
            Assert.IsTrue(Item_AmmoTypes.TryGetLoad("ammo_338lapua_bt", out var bt));
            Assert.IsTrue(Item_AmmoTypes.TryGetLoad("ammo_762x39_fmj", out var fmj));

            float btRange = Item_AmmoTypes.GetEffectiveRange(bt);
            float fmjRange = Item_AmmoTypes.GetEffectiveRange(fmj);
            Assert.Greater(btRange, bt.EffectiveRangeMeters);
            Assert.Greater(btRange, fmjRange);

            // Far beyond FMJ range but within boat-tail envelope: boat-tail keeps more damage.
            float longRange = fmjRange * 1.5f;
            var btHit = _ammo.ResolveHit("ammo_338lapua_bt", 100f, 0f, rangeMeters: longRange);
            var fmjHit = _ammo.ResolveHit("ammo_762x39_fmj", 100f, 0f, rangeMeters: longRange);
            Assert.Greater(btHit.FinalDamage, fmjHit.FinalDamage);
        }

        [Test]
        public void Barrier_Retain_Higher_For_Ap_Than_Jhp()
        {
            var jhp = _ammo.ResolveHit("ammo_9x19_jhp", 100f, 0f, behindBarrier: true);
            var ap = _ammo.ResolveHit("ammo_9x19_ap", 100f, 0f, behindBarrier: true);
            Assert.Less(jhp.BarrierDamageKept, ap.BarrierDamageKept);
            Assert.Greater(ap.FinalDamage, jhp.FinalDamage);
        }

        [Test]
        public void FortyFiveAcp_Fits_Pistol_And_Smg()
        {
            Assert.IsTrue(Item_AmmoTypes.FitsWeaponClass("ammo_45acp_fmj", WeaponAmmoClass.Pistol));
            Assert.IsTrue(Item_AmmoTypes.FitsWeaponClass("ammo_45acp_fmj", WeaponAmmoClass.Smg));
            Assert.IsFalse(Item_AmmoTypes.FitsWeaponClass("ammo_45acp_fmj", WeaponAmmoClass.Sniper));
        }

        [Test]
        public void LegacyAliases_Resolve()
        {
            Assert.IsTrue(Item_AmmoTypes.TryGetLoad("handgun_ammo", out var h));
            Assert.AreEqual("ammo_9x19_fmj", h.Id);
            Assert.IsTrue(Item_AmmoTypes.TryGetLoad("shotgun_shells", out var s));
            Assert.AreEqual("ammo_12ga_buck", s.Id);
            Assert.IsTrue(Item_AmmoTypes.TryGetLoad("ammo_rifle", out var r));
            Assert.AreEqual("ammo_556x45_fmj", r.Id);
        }

        [Test]
        public void HatchDefense_Recognizes_StructuredAmmoIds()
        {
            Assert.IsTrue(HatchDefenseSystem.IsAmmoId("ammo_9x19_jhp"));
            Assert.IsTrue(HatchDefenseSystem.IsAmmoId("ammo_50bmg_api"));
            Assert.IsTrue(HatchDefenseSystem.IsAmmoId("handgun_ammo"));
            Assert.IsFalse(HatchDefenseSystem.IsAmmoId("revolver"));
        }

        [Test]
        public void CaptureRestore_RoundTrips_LegacyState()
        {
            var save = _ammo.CaptureState();
            save.itemIdAP = "ammo_762x51_ap";
            var other = new Item_AmmoTypes();
            other.RestoreState(save);
            Assert.AreEqual("ammo_762x51_ap", other.GetAmmoId(AmmoType.ArmorPiercing));
        }

        [Test]
        public void SniperAndAv_Rarity_Tiers()
        {
            Assert.IsTrue(Item_AmmoTypes.TryGetLoad("ammo_762x54r_bt", out var s1));
            Assert.AreEqual(AmmoRarity.MythicRare, s1.Rarity);
            Assert.IsTrue(Item_AmmoTypes.TryGetLoad("ammo_338lapua_bt", out var s2));
            Assert.AreEqual(AmmoRarity.MythicRare, s2.Rarity);
            Assert.IsTrue(Item_AmmoTypes.TryGetLoad("ammo_408cheytac_bt", out var av1));
            Assert.AreEqual(AmmoRarity.LegendaryVeryRare, av1.Rarity);
            Assert.IsTrue(Item_AmmoTypes.TryGetLoad("ammo_50bmg_ap", out var av2));
            Assert.AreEqual(AmmoRarity.LegendaryVeryRare, av2.Rarity);
            Assert.AreEqual(WeaponAmmoClass.AntiMateriel, av1.WeaponClass);
            Assert.AreEqual(WeaponAmmoClass.AntiMateriel, av2.WeaponClass);
        }

        [Test]
        public void GetCraftableLoads_Excludes_MilitaryExclusives()
        {
            var craftable = Item_AmmoTypes.GetCraftableLoads();
            Assert.Greater(craftable.Count, 10);
            foreach (var load in craftable)
            {
                Assert.IsTrue(load.Craftable);
                Assert.AreNotEqual(BulletModification.Ap, load.Modification);
                Assert.AreNotEqual(BulletModification.Api, load.Modification);
                Assert.AreNotEqual(BulletModification.M855A1, load.Modification);
                Assert.AreNotEqual(BulletModification.JhpAp, load.Modification);
                Assert.AreNotEqual(BulletModification.ExplosiveIncendiary, load.Modification);
                Assert.AreNotEqual(WeaponAmmoClass.BattleRifle, load.WeaponClass);
                Assert.AreNotEqual(WeaponAmmoClass.Sniper, load.WeaponClass);
                Assert.AreNotEqual(WeaponAmmoClass.AntiMateriel, load.WeaponClass);
            }
        }

        [Test]
        public void DualAttribute_Mods_Always_Carry_Two_Flags()
        {
            Assert.AreEqual(2, Item_AmmoTypes.CountAttributes(
                Item_AmmoTypes.GetAttributes(BulletModification.JhpAp)));
            Assert.AreEqual(2, Item_AmmoTypes.CountAttributes(
                Item_AmmoTypes.GetAttributes(BulletModification.ExplosiveIncendiary)));
            Assert.AreEqual(2, Item_AmmoTypes.CountAttributes(
                Item_AmmoTypes.GetAttributes(BulletModification.Api)));

            Assert.AreEqual(
                BulletAttributeFlags.HollowPoint | BulletAttributeFlags.ArmorPiercing,
                Item_AmmoTypes.GetAttributes(BulletModification.JhpAp));
            Assert.AreEqual(
                BulletAttributeFlags.Explosive | BulletAttributeFlags.Incendiary,
                Item_AmmoTypes.GetAttributes(BulletModification.ExplosiveIncendiary));
            Assert.AreEqual(
                BulletAttributeFlags.ArmorPiercing | BulletAttributeFlags.Incendiary,
                Item_AmmoTypes.GetAttributes(BulletModification.Api));

            Assert.IsTrue(Item_AmmoTypes.IsDualAttributeModification(BulletModification.JhpAp));
            Assert.IsTrue(Item_AmmoTypes.IsDualAttributeModification(BulletModification.ExplosiveIncendiary));
            Assert.IsTrue(Item_AmmoTypes.IsDualAttributeModification(BulletModification.Api));
            Assert.IsFalse(Item_AmmoTypes.IsDualAttributeModification(BulletModification.Fmj));
            Assert.IsFalse(Item_AmmoTypes.IsDualAttributeModification(BulletModification.Ap));
        }

        [Test]
        public void BattleRifle_Sniper_Av_Have_Three_Dual_Tactical_Loads()
        {
            // idPrefix → expected weapon class
            var prefixes = new (string prefix, WeaponAmmoClass cls)[]
            {
                ("ammo_545x39", WeaponAmmoClass.BattleRifle),
                ("ammo_762x51", WeaponAmmoClass.BattleRifle),
                ("ammo_300blk", WeaponAmmoClass.BattleRifle),
                ("ammo_57x28", WeaponAmmoClass.BattleRifle),
                ("ammo_762x54r", WeaponAmmoClass.Sniper),
                ("ammo_338lapua", WeaponAmmoClass.Sniper),
                ("ammo_408cheytac", WeaponAmmoClass.AntiMateriel),
                ("ammo_50bmg", WeaponAmmoClass.AntiMateriel)
            };

            string[] dualSuffixes = { "_jhp_ap", "_exi", "_api" };
            BulletModification[] dualMods =
            {
                BulletModification.JhpAp,
                BulletModification.ExplosiveIncendiary,
                BulletModification.Api
            };

            foreach (var (prefix, cls) in prefixes)
            {
                for (int i = 0; i < dualSuffixes.Length; i++)
                {
                    string id = prefix + dualSuffixes[i];
                    Assert.IsTrue(Item_AmmoTypes.TryGetLoad(id, out var load), id);
                    Assert.AreEqual(cls, load.WeaponClass, id);
                    Assert.AreEqual(dualMods[i], load.Modification, id);
                    Assert.IsFalse(load.Craftable, id);
                    Assert.IsTrue(Item_AmmoTypes.IsDualAttributeTacticalLoad(id), id);
                    Assert.IsTrue(Item_AmmoTypes.IsExclusiveToFactions(id), id);
                    Assert.IsFalse(Item_AmmoTypes.CanFactionField(id, AmmoFactionSource.CivilianCraft), id);
                    Assert.IsTrue(Item_AmmoTypes.CanFactionField(id, AmmoFactionSource.MilitaryForces), id);
                    Assert.IsTrue(Item_AmmoTypes.CanFactionField(id, AmmoFactionSource.RebelForces), id);
                }
            }
        }

        [Test]
        public void JhpAp_ResolveHit_SoftBonus_And_PartialArmorIgnore()
        {
            float baseDmg = 100f;
            var soft = _ammo.ResolveHit("ammo_762x51_jhp_ap", baseDmg, targetArmor: 0f);
            Assert.IsTrue(soft.IsDualAttribute);
            Assert.IsTrue(soft.SoftTargetBonusApplied);
            Assert.AreEqual(1f + Item_AmmoTypes.JhpApSoftBonus, soft.DamageMultiplier, Eps);
            Assert.AreEqual(
                BulletAttributeFlags.HollowPoint | BulletAttributeFlags.ArmorPiercing,
                soft.Attributes);

            float armor = 40f;
            var hard = _ammo.ResolveHit("ammo_762x51_jhp_ap", baseDmg, targetArmor: armor);
            Assert.AreEqual(Item_AmmoTypes.JhpApArmorIgnore, hard.ArmorIgnored, Eps);
            Assert.IsFalse(hard.ArmorPenaltyApplied, "JHP+AP must not take full JHP armor penalty");
            Assert.Greater(hard.FinalDamage, _ammo.ResolveHit("ammo_9x19_jhp", baseDmg, armor).FinalDamage);
        }

        [Test]
        public void ExplosiveIncendiary_ResolveHit_Splash_Burn_And_Light()
        {
            var hit = _ammo.ResolveHit("ammo_50bmg_exi", 70f, targetArmor: 20f);
            Assert.IsTrue(hit.IsDualAttribute);
            Assert.IsTrue(hit.HasExplosive);
            Assert.AreEqual(Item_AmmoTypes.ExiExplosiveSplashFraction, hit.ExplosiveSplashFraction, Eps);
            Assert.AreEqual(Item_AmmoTypes.ExiBurnDps, hit.BurnDamagePerSecond, Eps);
            Assert.AreEqual(Item_AmmoTypes.ExiBurnDurationSeconds, hit.BurnDurationSeconds, Eps);
            Assert.IsTrue(hit.LightsArea);
            Assert.AreEqual(
                BulletAttributeFlags.Explosive | BulletAttributeFlags.Incendiary,
                hit.Attributes);
            Assert.Greater(hit.DamageMultiplier, 1f);
        }

        [Test]
        public void Api_Is_Dual_ArmourPiercing_Plus_Incendiary()
        {
            var hit = _ammo.ResolveHit("ammo_762x51_api", 100f, targetArmor: 30f);
            Assert.IsTrue(hit.IsDualAttribute);
            Assert.AreEqual(BulletModification.Api, hit.Modification);
            Assert.AreEqual(
                BulletAttributeFlags.ArmorPiercing | BulletAttributeFlags.Incendiary,
                hit.Attributes);
            Assert.IsTrue(hit.LightsArea);
            Assert.Greater(hit.BurnDamagePerSecond, 0f);
        }

        [Test]
        public void DualAttribute_Tooltip_Shows_Dual_Badge()
        {
            string tip = Item_AmmoTypes.FormatItemTooltip("ammo_338lapua_jhp_ap");
            Assert.IsTrue(tip.Contains("[DUAL]"), tip);
            Assert.IsTrue(tip.Contains("Hollow-point"), tip);
            Assert.IsTrue(tip.Contains("Armour-piercing"), tip);
            Assert.IsTrue(tip.Contains("[MILITARY EXCLUSIVE]"), tip);

            string exi = Item_AmmoTypes.FormatItemTooltip("ammo_408cheytac_exi");
            Assert.IsTrue(exi.Contains("[DUAL]"), exi);
            Assert.IsTrue(exi.Contains("Explosive"), exi);
            Assert.IsTrue(exi.Contains("Incendiary"), exi);
        }

        [Test]
        public void DualTactical_In_ApApiLootPool()
        {
            var pool = Item_AmmoTypes.GetApApiLootIds();
            CollectionAssert.Contains(pool, "ammo_545x39_jhp_ap");
            CollectionAssert.Contains(pool, "ammo_545x39_exi");
            CollectionAssert.Contains(pool, "ammo_50bmg_exi");
            CollectionAssert.Contains(pool, "ammo_762x51_api");
        }

        [Test]
        public void Legacy_GetDamageMultiplier_StillWorks()
        {
            Assert.AreEqual(2f, _ammo.GetDamageMultiplier(AmmoType.HollowPoint, "mutant", false), Eps);
            Assert.AreEqual(0f, _ammo.GetDamageMultiplier(AmmoType.HollowPoint, "warlord", true), Eps);
            Assert.AreEqual(0.5f, _ammo.GetDamageMultiplier(AmmoType.ArmorPiercing, "mutant", true), Eps);
            Assert.AreEqual(1f, _ammo.GetDamageMultiplier(AmmoType.Standard, "animal", false), Eps);
        }
    }
}
