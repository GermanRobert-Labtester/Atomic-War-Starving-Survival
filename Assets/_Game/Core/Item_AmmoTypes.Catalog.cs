using System;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Static caliber catalog: pistol → SMG → shotgun → rifle → battle rifle →
    /// sniper → PDW → anti-materiel, with craftable civilian loads vs military/rebel exclusives.
    /// </summary>
    public partial class Item_AmmoTypes
    {
        private static readonly AmmoFactionSource[] MilitaryRebelExclusives =
        {
            AmmoFactionSource.RebelForces,
            AmmoFactionSource.MercenaryRebel,
            AmmoFactionSource.MercenaryMilitary,
            AmmoFactionSource.MilitaryForces,
            AmmoFactionSource.BlackOpsMilitary,
            AmmoFactionSource.SpecOpsRebel
        };

        private static readonly AmmoFactionSource[] CivilianOnly =
        {
            AmmoFactionSource.CivilianCraft
        };

        private static void EnsureCatalog()
        {
            if (_catalogBuilt) return;
            lock (Catalog)
            {
                if (_catalogBuilt) return;
                BuildCatalog();
                _catalogBuilt = true;
            }
        }

        private static void BuildCatalog()
        {
            Catalog.Clear();
            LegacyAliases.Clear();

            // ── Handgun / pistol ──────────────────────────────────────────
            // 9x19mm [Uncommon] — early survival baseline (aliases handgun_ammo)
            Add(Load("ammo_9x19_fmj", "9×19mm FMJ", "cal_9x19", "9×19mm Parabellum",
                WeaponAmmoClass.Pistol, AmmoRarity.Uncommon, BulletModification.Fmj,
                craftable: true, dmg: 12f, range: 50f, weight: 0.012f, stack: 30, trade: 2f,
                desc: "Civilian brass-and-lead pistol rounds. Easy bunker craft.",
                sources: CivilianOnly));
            Add(Load("ammo_9x19_jhp", "9×19mm JHP", "cal_9x19", "9×19mm Parabellum",
                WeaponAmmoClass.Pistol, AmmoRarity.Uncommon, BulletModification.Jhp,
                craftable: true, dmg: 12f, range: 45f, weight: 0.012f, stack: 30, trade: 3f,
                desc: "Jacketed hollow points. +50% vs unarmored; −80% vs body armor.",
                sources: CivilianOnly));
            Add(Load("ammo_9x19_ap", "9×19mm AP", "cal_9x19", "9×19mm Parabellum",
                WeaponAmmoClass.Pistol, AmmoRarity.Rare, BulletModification.Ap,
                craftable: false, dmg: 12f, range: 55f, weight: 0.013f, stack: 30, trade: 8f,
                desc: "Hardened-core pistol AP. Military/rebel stock only.",
                sources: MilitaryRebelExclusives));

            // .380 ACP [Uncommon]
            Add(Load("ammo_380acp_fmj", ".380 ACP FMJ", "cal_380acp", ".380 ACP",
                WeaponAmmoClass.Pistol, AmmoRarity.Uncommon, BulletModification.Fmj,
                craftable: true, dmg: 9f, range: 35f, weight: 0.010f, stack: 30, trade: 2f,
                desc: "Compact pistol round. Milder recoil, easier to press.",
                sources: CivilianOnly));
            Add(Load("ammo_380acp_jhp", ".380 ACP JHP", "cal_380acp", ".380 ACP",
                WeaponAmmoClass.Pistol, AmmoRarity.Uncommon, BulletModification.Jhp,
                craftable: true, dmg: 9f, range: 30f, weight: 0.010f, stack: 30, trade: 2.5f,
                desc: "Expanding .380. Soft targets only.",
                sources: CivilianOnly));

            // 7.62×25mm Tokarev [Common]
            Add(Load("ammo_762x25_fmj", "7.62×25mm Tokarev FMJ", "cal_762x25", "7.62×25mm Tokarev",
                WeaponAmmoClass.Pistol, AmmoRarity.Common, BulletModification.Fmj,
                craftable: true, dmg: 11f, range: 60f, weight: 0.011f, stack: 40, trade: 1.5f,
                desc: "Hot surplus pistol/SMG cartridge. Common scavenger scrap.",
                sources: CivilianOnly, alsoFits: new[] { WeaponAmmoClass.Smg }));
            Add(Load("ammo_762x25_jhp", "7.62×25mm Tokarev JHP", "cal_762x25", "7.62×25mm Tokarev",
                WeaponAmmoClass.Pistol, AmmoRarity.Common, BulletModification.Jhp,
                craftable: true, dmg: 11f, range: 50f, weight: 0.011f, stack: 40, trade: 2f,
                desc: "Hand-cut hollow points for Tokarev pistols and SMGs.",
                sources: CivilianOnly, alsoFits: new[] { WeaponAmmoClass.Smg }));

            // .45 ACP [Common] — pistols + SMGs
            Add(Load("ammo_45acp_fmj", ".45 ACP FMJ", "cal_45acp", ".45 ACP",
                WeaponAmmoClass.Pistol, AmmoRarity.Common, BulletModification.Fmj,
                craftable: true, dmg: 14f, range: 40f, weight: 0.015f, stack: 24, trade: 2f,
                desc: "Heavy pistol/SMG ball. Slow, hard-hitting soft-target punch.",
                sources: CivilianOnly, alsoFits: new[] { WeaponAmmoClass.Smg }));
            Add(Load("ammo_45acp_jhp", ".45 ACP JHP", "cal_45acp", ".45 ACP",
                WeaponAmmoClass.Pistol, AmmoRarity.Common, BulletModification.Jhp,
                craftable: true, dmg: 14f, range: 35f, weight: 0.015f, stack: 24, trade: 3f,
                desc: "Expanding .45. Excellent unarmored damage; fails on plates.",
                sources: CivilianOnly, alsoFits: new[] { WeaponAmmoClass.Smg }));
            Add(Load("ammo_45acp_ap", ".45 ACP AP", "cal_45acp", ".45 ACP",
                WeaponAmmoClass.Pistol, AmmoRarity.Rare, BulletModification.Ap,
                craftable: false, dmg: 14f, range: 45f, weight: 0.016f, stack: 24, trade: 9f,
                desc: "Hardened .45 penetrators. Not bunker-craftable.",
                sources: MilitaryRebelExclusives, alsoFits: new[] { WeaponAmmoClass.Smg }));

            // ── SMG exclusive ─────────────────────────────────────────────
            // 9×21mm IMI [Uncommon]
            Add(Load("ammo_9x21_fmj", "9×21mm IMI FMJ", "cal_9x21", "9×21mm IMI",
                WeaponAmmoClass.Smg, AmmoRarity.Uncommon, BulletModification.Fmj,
                craftable: true, dmg: 13f, range: 80f, weight: 0.013f, stack: 30, trade: 3f,
                desc: "SMG-exclusive hotter 9mm family. Workbench pressable.",
                sources: CivilianOnly));
            Add(Load("ammo_9x21_jhp", "9×21mm IMI JHP", "cal_9x21", "9×21mm IMI",
                WeaponAmmoClass.Smg, AmmoRarity.Uncommon, BulletModification.Jhp,
                craftable: true, dmg: 13f, range: 70f, weight: 0.013f, stack: 30, trade: 4f,
                desc: "Expanding SMG load. Soft-target specialist.",
                sources: CivilianOnly));

            // 7.65×21mm [Uncommon]
            Add(Load("ammo_765x21_fmj", "7.65×21mm FMJ", "cal_765x21", "7.65×21mm Parabellum",
                WeaponAmmoClass.Smg, AmmoRarity.Uncommon, BulletModification.Fmj,
                craftable: true, dmg: 10f, range: 70f, weight: 0.010f, stack: 35, trade: 2.5f,
                desc: "Light SMG cartridge. Low recoil, moderate punch.",
                sources: CivilianOnly));
            Add(Load("ammo_765x21_jhp", "7.65×21mm JHP", "cal_765x21", "7.65×21mm Parabellum",
                WeaponAmmoClass.Smg, AmmoRarity.Uncommon, BulletModification.Jhp,
                craftable: true, dmg: 10f, range: 60f, weight: 0.010f, stack: 35, trade: 3f,
                desc: "Hollow-point SMG loads for unarmored raiders.",
                sources: CivilianOnly));

            // ── Shotgun ───────────────────────────────────────────────────
            // 12/70 Gauge [Common] — aliases shotgun_shells
            Add(Load("ammo_12ga_buck", "12/70 Buckshot", "cal_12ga", "12/70 Gauge",
                WeaponAmmoClass.Shotgun, AmmoRarity.Common, BulletModification.SoftLead,
                craftable: true, dmg: 22f, range: 25f, weight: 0.045f, stack: 20, trade: 3f,
                desc: "Lead pellets in cardboard hulls. Early bunker craft staple.",
                sources: CivilianOnly));
            Add(Load("ammo_12ga_slug", "12/70 Slug", "cal_12ga", "12/70 Gauge",
                WeaponAmmoClass.Shotgun, AmmoRarity.Common, BulletModification.Fmj,
                craftable: true, dmg: 26f, range: 45f, weight: 0.050f, stack: 16, trade: 4f,
                desc: "Single lead slug. Better range than buckshot.",
                sources: CivilianOnly));
            Add(Load("ammo_12ga_ap", "12/70 AP Slug", "cal_12ga", "12/70 Gauge",
                WeaponAmmoClass.Shotgun, AmmoRarity.VeryRare, BulletModification.Ap,
                craftable: false, dmg: 26f, range: 50f, weight: 0.055f, stack: 12, trade: 12f,
                desc: "Saboted hardened slug. Breaches light plates and doors.",
                sources: MilitaryRebelExclusives));

            // 16/70 Gauge [Uncommon]
            Add(Load("ammo_16ga_buck", "16/70 Buckshot", "cal_16ga", "16/70 Gauge",
                WeaponAmmoClass.Shotgun, AmmoRarity.Uncommon, BulletModification.SoftLead,
                craftable: true, dmg: 18f, range: 22f, weight: 0.038f, stack: 20, trade: 2.5f,
                desc: "Lighter gauge shells. Easier powder budget than 12ga.",
                sources: CivilianOnly));
            Add(Load("ammo_16ga_slug", "16/70 Slug", "cal_16ga", "16/70 Gauge",
                WeaponAmmoClass.Shotgun, AmmoRarity.Uncommon, BulletModification.Fmj,
                craftable: true, dmg: 20f, range: 40f, weight: 0.040f, stack: 16, trade: 3f,
                desc: "16-gauge solid slug for hunting and hatch defense.",
                sources: CivilianOnly));

            // ── Intermediate rifle ────────────────────────────────────────
            // 5.56×45mm NATO [Rare]
            Add(Load("ammo_556x45_fmj", "5.56×45mm FMJ", "cal_556x45", "5.56×45mm NATO",
                WeaponAmmoClass.Rifle, AmmoRarity.Rare, BulletModification.Fmj,
                craftable: true, dmg: 18f, range: 400f, weight: 0.012f, stack: 30, trade: 5f,
                desc: "Intermediate rifle ball. Craftable at advanced reloader.",
                sources: CivilianOnly, alsoFits: new[] { WeaponAmmoClass.Lmg }));
            Add(Load("ammo_556x45_jhp", "5.56×45mm JHP", "cal_556x45", "5.56×45mm NATO",
                WeaponAmmoClass.Rifle, AmmoRarity.Rare, BulletModification.Jhp,
                craftable: true, dmg: 18f, range: 350f, weight: 0.012f, stack: 30, trade: 6f,
                desc: "Expanding 5.56. Soft-target dump; fails on plates.",
                sources: CivilianOnly, alsoFits: new[] { WeaponAmmoClass.Lmg }));
            Add(Load("ammo_556x45_m855a1", "5.56×45mm M855A1", "cal_556x45", "5.56×45mm NATO",
                WeaponAmmoClass.Rifle, AmmoRarity.VeryRare, BulletModification.M855A1,
                craftable: false, dmg: 20f, range: 500f, weight: 0.012f, stack: 30, trade: 14f,
                desc: "Steel-tip copper slug. Soft and hard targets without the JHP trade-off.",
                sources: MilitaryRebelExclusives, alsoFits: new[] { WeaponAmmoClass.Lmg }));
            Add(Load("ammo_556x45_ap", "5.56×45mm AP", "cal_556x45", "5.56×45mm NATO",
                WeaponAmmoClass.Rifle, AmmoRarity.VeryRare, BulletModification.Ap,
                craftable: false, dmg: 18f, range: 450f, weight: 0.013f, stack: 30, trade: 12f,
                desc: "Hardened 5.56 penetrators. Military issue only.",
                sources: MilitaryRebelExclusives, alsoFits: new[] { WeaponAmmoClass.Lmg }));

            // 7.62×39mm [Rare]
            Add(Load("ammo_762x39_fmj", "7.62×39mm FMJ", "cal_762x39", "7.62×39mm",
                WeaponAmmoClass.Rifle, AmmoRarity.Rare, BulletModification.Fmj,
                craftable: true, dmg: 20f, range: 350f, weight: 0.016f, stack: 30, trade: 5f,
                desc: "Intermediate battle-proven ball. Heavier punch, more recoil.",
                sources: CivilianOnly));
            Add(Load("ammo_762x39_jhp", "7.62×39mm JHP", "cal_762x39", "7.62×39mm",
                WeaponAmmoClass.Rifle, AmmoRarity.Rare, BulletModification.Jhp,
                craftable: true, dmg: 20f, range: 300f, weight: 0.016f, stack: 30, trade: 6f,
                desc: "Expanding 7.62×39. Devastating unarmored; poor vs armor.",
                sources: CivilianOnly));
            Add(Load("ammo_762x39_ap", "7.62×39mm AP", "cal_762x39", "7.62×39mm",
                WeaponAmmoClass.Rifle, AmmoRarity.VeryRare, BulletModification.Ap,
                craftable: false, dmg: 20f, range: 380f, weight: 0.017f, stack: 30, trade: 12f,
                desc: "Steel-core 7.62×39. Rebel cache staple.",
                sources: MilitaryRebelExclusives));

            // ── Battle rifle exclusives [Very Rare] — non-craftable ───────
            // Dual-attribute tactical loads (always two attributes):
            //   JHP+AP · Explosive+Incendiary · AP+Incendiary (API)
            // ~2 named guns per caliber planned later; calibers reviewed later.
            // 5.45×39mm
            Add(Load("ammo_545x39_fmj", "5.45×39mm FMJ", "cal_545x39", "5.45×39mm",
                WeaponAmmoClass.BattleRifle, AmmoRarity.VeryRare, BulletModification.Fmj,
                craftable: false, dmg: 19f, range: 450f, weight: 0.011f, stack: 30, trade: 10f,
                desc: "Battle-rifle exclusive. Military/rebel stock only.",
                sources: MilitaryRebelExclusives));
            Add(Load("ammo_545x39_ap", "5.45×39mm AP", "cal_545x39", "5.45×39mm",
                WeaponAmmoClass.BattleRifle, AmmoRarity.VeryRare, BulletModification.Ap,
                craftable: false, dmg: 19f, range: 480f, weight: 0.012f, stack: 30, trade: 14f,
                desc: "Hardened 5.45 battle-rifle AP.",
                sources: MilitaryRebelExclusives));
            AddDualTactical(
                "ammo_545x39", "5.45×39mm", "cal_545x39", "5.45×39mm",
                WeaponAmmoClass.BattleRifle, AmmoRarity.VeryRare,
                dmg: 19f, range: 460f, weight: 0.012f, stack: 28, trade: 16f);

            // 7.62×51mm NATO
            Add(Load("ammo_762x51_fmj", "7.62×51mm NATO FMJ", "cal_762x51", "7.62×51mm NATO",
                WeaponAmmoClass.BattleRifle, AmmoRarity.VeryRare, BulletModification.Fmj,
                craftable: false, dmg: 28f, range: 600f, weight: 0.025f, stack: 20, trade: 12f,
                desc: "Full-power battle rifle cartridge. Heavy recoil, long reach.",
                sources: MilitaryRebelExclusives, alsoFits: new[] { WeaponAmmoClass.Lmg, WeaponAmmoClass.Sniper }));
            Add(Load("ammo_762x51_ap", "7.62×51mm NATO AP", "cal_762x51", "7.62×51mm NATO",
                WeaponAmmoClass.BattleRifle, AmmoRarity.VeryRare, BulletModification.Ap,
                craftable: false, dmg: 28f, range: 650f, weight: 0.026f, stack: 20, trade: 16f,
                desc: "Full-power AP. Punches plates and light cover.",
                sources: MilitaryRebelExclusives, alsoFits: new[] { WeaponAmmoClass.Lmg, WeaponAmmoClass.Sniper }));
            Add(Load("ammo_762x51_api", "7.62×51mm NATO API", "cal_762x51", "7.62×51mm NATO",
                WeaponAmmoClass.BattleRifle, AmmoRarity.MythicRare, BulletModification.Api,
                craftable: false, dmg: 28f, range: 620f, weight: 0.027f, stack: 16, trade: 22f,
                desc: "Dual: armour-piercing + incendiary. Burn DoT and combat-zone light.",
                sources: MilitaryRebelExclusives, alsoFits: new[] { WeaponAmmoClass.Lmg, WeaponAmmoClass.Sniper }));
            Add(Load("ammo_762x51_jhp_ap", "7.62×51mm NATO JHP+AP", "cal_762x51", "7.62×51mm NATO",
                WeaponAmmoClass.BattleRifle, AmmoRarity.MythicRare, BulletModification.JhpAp,
                craftable: false, dmg: 28f, range: 580f, weight: 0.027f, stack: 16, trade: 20f,
                desc: "Dual: hollow-point + armour-piercing. Soft dump and plate bite.",
                sources: MilitaryRebelExclusives, alsoFits: new[] { WeaponAmmoClass.Lmg, WeaponAmmoClass.Sniper }));
            Add(Load("ammo_762x51_exi", "7.62×51mm NATO EXI", "cal_762x51", "7.62×51mm NATO",
                WeaponAmmoClass.BattleRifle, AmmoRarity.MythicRare, BulletModification.ExplosiveIncendiary,
                craftable: false, dmg: 28f, range: 560f, weight: 0.028f, stack: 14, trade: 24f,
                desc: "Dual: explosive + incendiary. Splash crack and burn.",
                sources: MilitaryRebelExclusives, alsoFits: new[] { WeaponAmmoClass.Lmg, WeaponAmmoClass.Sniper }));

            // .300 Blackout
            Add(Load("ammo_300blk_fmj", ".300 Blackout FMJ", "cal_300blk", ".300 Blackout",
                WeaponAmmoClass.BattleRifle, AmmoRarity.VeryRare, BulletModification.Fmj,
                craftable: false, dmg: 22f, range: 300f, weight: 0.018f, stack: 25, trade: 11f,
                desc: "Battle-rifle exclusive subsonic-capable cartridge.",
                sources: MilitaryRebelExclusives));
            Add(Load("ammo_300blk_ap", ".300 Blackout AP", "cal_300blk", ".300 Blackout",
                WeaponAmmoClass.BattleRifle, AmmoRarity.VeryRare, BulletModification.Ap,
                craftable: false, dmg: 22f, range: 320f, weight: 0.019f, stack: 25, trade: 15f,
                desc: "Hardened .300 BLK. Spec-ops rebel / black-ops issue.",
                sources: MilitaryRebelExclusives));
            AddDualTactical(
                "ammo_300blk", ".300 Blackout", "cal_300blk", ".300 Blackout",
                WeaponAmmoClass.BattleRifle, AmmoRarity.VeryRare,
                dmg: 22f, range: 310f, weight: 0.019f, stack: 22, trade: 17f);

            // 5.7×28mm
            Add(Load("ammo_57x28_fmj", "5.7×28mm FMJ", "cal_57x28", "5.7×28mm",
                WeaponAmmoClass.BattleRifle, AmmoRarity.VeryRare, BulletModification.Fmj,
                craftable: false, dmg: 14f, range: 200f, weight: 0.006f, stack: 40, trade: 10f,
                desc: "High-velocity battle-rifle exclusive PDW-family round.",
                sources: MilitaryRebelExclusives, alsoFits: new[] { WeaponAmmoClass.Pdw, WeaponAmmoClass.Pistol }));
            Add(Load("ammo_57x28_ap", "5.7×28mm AP", "cal_57x28", "5.7×28mm",
                WeaponAmmoClass.BattleRifle, AmmoRarity.VeryRare, BulletModification.Ap,
                craftable: false, dmg: 14f, range: 220f, weight: 0.006f, stack: 40, trade: 14f,
                desc: "Armor-piercing 5.7. Defeats soft armor at pistol distances.",
                sources: MilitaryRebelExclusives, alsoFits: new[] { WeaponAmmoClass.Pdw, WeaponAmmoClass.Pistol }));
            AddDualTactical(
                "ammo_57x28", "5.7×28mm", "cal_57x28", "5.7×28mm",
                WeaponAmmoClass.BattleRifle, AmmoRarity.VeryRare,
                dmg: 14f, range: 210f, weight: 0.0065f, stack: 36, trade: 16f,
                alsoFits: new[] { WeaponAmmoClass.Pdw, WeaponAmmoClass.Pistol });

            // ── PDW late-game (4.6×30mm) ───────────────────────────────────
            Add(Load("ammo_46x30_fmj", "4.6×30mm FMJ", "cal_46x30", "4.6×30mm",
                WeaponAmmoClass.Pdw, AmmoRarity.VeryRare, BulletModification.Fmj,
                craftable: false, dmg: 13f, range: 200f, weight: 0.005f, stack: 40, trade: 11f,
                desc: "High-velocity PDW round. Late-game military stock.",
                sources: MilitaryRebelExclusives));
            Add(Load("ammo_46x30_ap", "4.6×30mm AP", "cal_46x30", "4.6×30mm",
                WeaponAmmoClass.Pdw, AmmoRarity.VeryRare, BulletModification.Ap,
                craftable: false, dmg: 13f, range: 220f, weight: 0.005f, stack: 40, trade: 15f,
                desc: "AP PDW load. Black-ops / spec-ops exclusive.",
                sources: MilitaryRebelExclusives));

            // ── Sniper [Mythic Rare] + dual-attribute tactical ─────────────
            // 7.62×54R
            Add(Load("ammo_762x54r_bt", "7.62×54R Boat Tail", "cal_762x54r", "7.62×54R",
                WeaponAmmoClass.Sniper, AmmoRarity.MythicRare, BulletModification.BoatTail,
                craftable: false, dmg: 32f, range: 800f, weight: 0.024f, stack: 15, trade: 18f,
                desc: "Boat-tail sniper match. Extended range, military/rebel only.",
                sources: MilitaryRebelExclusives));
            Add(Load("ammo_762x54r_ap", "7.62×54R AP", "cal_762x54r", "7.62×54R",
                WeaponAmmoClass.Sniper, AmmoRarity.MythicRare, BulletModification.Ap,
                craftable: false, dmg: 32f, range: 750f, weight: 0.025f, stack: 15, trade: 20f,
                desc: "Sniper AP. Plate and light barrier defeat.",
                sources: MilitaryRebelExclusives));
            AddDualTactical(
                "ammo_762x54r", "7.62×54R", "cal_762x54r", "7.62×54R",
                WeaponAmmoClass.Sniper, AmmoRarity.MythicRare,
                dmg: 32f, range: 780f, weight: 0.025f, stack: 12, trade: 24f);

            // .338 Lapua Magnum
            Add(Load("ammo_338lapua_bt", ".338 Lapua Magnum Boat Tail", "cal_338lapua", ".338 Lapua Magnum",
                WeaponAmmoClass.Sniper, AmmoRarity.MythicRare, BulletModification.BoatTail,
                craftable: false, dmg: 42f, range: 1200f, weight: 0.045f, stack: 10, trade: 28f,
                desc: "Extreme sniper match. Boat-tail aerodynamics; non-craftable.",
                sources: MilitaryRebelExclusives));
            Add(Load("ammo_338lapua_ap", ".338 Lapua Magnum AP", "cal_338lapua", ".338 Lapua Magnum",
                WeaponAmmoClass.Sniper, AmmoRarity.MythicRare, BulletModification.Ap,
                craftable: false, dmg: 42f, range: 1100f, weight: 0.046f, stack: 10, trade: 32f,
                desc: "Hardened .338. Spec-ops / black-ops caches only.",
                sources: MilitaryRebelExclusives));
            AddDualTactical(
                "ammo_338lapua", ".338 Lapua Magnum", "cal_338lapua", ".338 Lapua Magnum",
                WeaponAmmoClass.Sniper, AmmoRarity.MythicRare,
                dmg: 42f, range: 1150f, weight: 0.047f, stack: 8, trade: 36f);

            // ── Extreme long-range AV [Legendary Very Rare] + dual tactical ─
            // .408 CheyTac
            Add(Load("ammo_408cheytac_bt", ".408 CheyTac Boat Tail", "cal_408cheytac", ".408 CheyTac",
                WeaponAmmoClass.AntiMateriel, AmmoRarity.LegendaryVeryRare, BulletModification.BoatTail,
                craftable: false, dmg: 55f, range: 2000f, weight: 0.065f, stack: 8, trade: 45f,
                desc: "Anti-materiel extreme-range match. Legendary military stock.",
                sources: MilitaryRebelExclusives));
            Add(Load("ammo_408cheytac_ap", ".408 CheyTac AP", "cal_408cheytac", ".408 CheyTac",
                WeaponAmmoClass.AntiMateriel, AmmoRarity.LegendaryVeryRare, BulletModification.Ap,
                craftable: false, dmg: 55f, range: 1800f, weight: 0.068f, stack: 8, trade: 50f,
                desc: "Hardened AV penetrator. Non-craftable.",
                sources: MilitaryRebelExclusives));
            AddDualTactical(
                "ammo_408cheytac", ".408 CheyTac", "cal_408cheytac", ".408 CheyTac",
                WeaponAmmoClass.AntiMateriel, AmmoRarity.LegendaryVeryRare,
                dmg: 55f, range: 1850f, weight: 0.070f, stack: 6, trade: 58f);

            // 12.7×99mm NATO (.50 BMG)
            Add(Load("ammo_50bmg_ap", "12.7×99mm NATO AP", "cal_50bmg", "12.7×99mm NATO",
                WeaponAmmoClass.AntiMateriel, AmmoRarity.LegendaryVeryRare, BulletModification.Ap,
                craftable: false, dmg: 70f, range: 1800f, weight: 0.115f, stack: 8, trade: 55f,
                desc: ".50 BMG armor-piercing. Late-game anti-materiel reward.",
                sources: MilitaryRebelExclusives, alsoFits: new[] { WeaponAmmoClass.Lmg }));
            Add(Load("ammo_50bmg_api", "12.7×99mm NATO API", "cal_50bmg", "12.7×99mm NATO",
                WeaponAmmoClass.AntiMateriel, AmmoRarity.LegendaryVeryRare, BulletModification.Api,
                craftable: false, dmg: 70f, range: 1700f, weight: 0.120f, stack: 6, trade: 65f,
                desc: "Dual: armour-piercing + incendiary. Burn, light, and breach.",
                sources: MilitaryRebelExclusives, alsoFits: new[] { WeaponAmmoClass.Lmg }));
            Add(Load("ammo_50bmg_bt", "12.7×99mm NATO Boat Tail", "cal_50bmg", "12.7×99mm NATO",
                WeaponAmmoClass.AntiMateriel, AmmoRarity.LegendaryVeryRare, BulletModification.BoatTail,
                craftable: false, dmg: 65f, range: 2200f, weight: 0.110f, stack: 8, trade: 50f,
                desc: "Long-range .50 match. Extreme trajectory hold.",
                sources: MilitaryRebelExclusives, alsoFits: new[] { WeaponAmmoClass.Lmg }));
            Add(Load("ammo_50bmg_jhp_ap", "12.7×99mm NATO JHP+AP", "cal_50bmg", "12.7×99mm NATO",
                WeaponAmmoClass.AntiMateriel, AmmoRarity.LegendaryVeryRare, BulletModification.JhpAp,
                craftable: false, dmg: 70f, range: 1650f, weight: 0.118f, stack: 6, trade: 62f,
                desc: "Dual: hollow-point + armour-piercing AV load.",
                sources: MilitaryRebelExclusives, alsoFits: new[] { WeaponAmmoClass.Lmg }));
            Add(Load("ammo_50bmg_exi", "12.7×99mm NATO EXI", "cal_50bmg", "12.7×99mm NATO",
                WeaponAmmoClass.AntiMateriel, AmmoRarity.LegendaryVeryRare, BulletModification.ExplosiveIncendiary,
                craftable: false, dmg: 70f, range: 1600f, weight: 0.125f, stack: 5, trade: 70f,
                desc: "Dual: explosive + incendiary AV. Splash, burn, and light.",
                sources: MilitaryRebelExclusives, alsoFits: new[] { WeaponAmmoClass.Lmg }));

            // Legacy aliases used across encounters / hatch defense / bootstrap
            LegacyAliases["handgun_ammo"] = "ammo_9x19_fmj";
            LegacyAliases["shotgun_shells"] = "ammo_12ga_buck";
            LegacyAliases["ammo_9mm"] = "ammo_9x19_fmj";
            LegacyAliases["ammo_pistol"] = "ammo_9x19_fmj";
            LegacyAliases["ammo_shotgun"] = "ammo_12ga_buck";
            LegacyAliases["ammo_rifle"] = "ammo_556x45_fmj";
            LegacyAliases["item_ammo_standard"] = "ammo_9x19_fmj";
            LegacyAliases["item_ammo_ap"] = "ammo_556x45_ap";
            LegacyAliases["item_ammo_hp"] = "ammo_9x19_jhp";
            LegacyAliases["ap_rifle_rounds"] = "ammo_556x45_ap";
            LegacyAliases["high_tier_military_ammo_box"] = "ammo_762x51_ap";
            LegacyAliases["556_ammo_can"] = "ammo_556x45_m855a1";
        }

        /// <summary>
        /// Adds the three dual-attribute tactical loads for BR / sniper / AV calibers:
        /// JHP+AP, Explosive+Incendiary (EXI), AP+Incendiary (API). Each carries two attributes.
        /// Skips ids already present (e.g. when API was authored earlier in the catalog).
        /// </summary>
        private static void AddDualTactical(
            string idPrefix,
            string namePrefix,
            string caliberId,
            string caliberDisplay,
            WeaponAmmoClass weaponClass,
            AmmoRarity rarity,
            float dmg,
            float range,
            float weight,
            int stack,
            float trade,
            WeaponAmmoClass[] alsoFits = null)
        {
            TryAddDual(idPrefix + "_jhp_ap", namePrefix + " JHP+AP", caliberId, caliberDisplay,
                weaponClass, rarity, BulletModification.JhpAp,
                dmg, range, weight, stack, trade,
                "Dual: hollow-point + armour-piercing. Soft-target dump with plate bite.",
                alsoFits);
            TryAddDual(idPrefix + "_exi", namePrefix + " EXI", caliberId, caliberDisplay,
                weaponClass, rarity, BulletModification.ExplosiveIncendiary,
                dmg, range * 0.95f, weight * 1.05f, Math.Max(4, stack - 2), trade + 4f,
                "Dual: explosive + incendiary. Splash crack, burn, and combat light.",
                alsoFits);
            TryAddDual(idPrefix + "_api", namePrefix + " API", caliberId, caliberDisplay,
                weaponClass, rarity, BulletModification.Api,
                dmg, range, weight * 1.04f, Math.Max(4, stack - 2), trade + 6f,
                "Dual: armour-piercing + incendiary. Plate defeat with burn DoT.",
                alsoFits);
        }

        private static void TryAddDual(
            string id,
            string display,
            string caliberId,
            string caliberDisplay,
            WeaponAmmoClass weaponClass,
            AmmoRarity rarity,
            BulletModification mod,
            float dmg,
            float range,
            float weight,
            int stack,
            float trade,
            string desc,
            WeaponAmmoClass[] alsoFits)
        {
            if (Catalog.ContainsKey(id)) return;
            Add(Load(id, display, caliberId, caliberDisplay, weaponClass, rarity, mod,
                craftable: false, dmg, range, weight, stack, trade, desc,
                MilitaryRebelExclusives, alsoFits));
        }

        private static AmmoLoadDefinition Load(
            string id,
            string display,
            string caliberId,
            string caliberDisplay,
            WeaponAmmoClass weaponClass,
            AmmoRarity rarity,
            BulletModification mod,
            bool craftable,
            float dmg,
            float range,
            float weight,
            int stack,
            float trade,
            string desc,
            AmmoFactionSource[] sources,
            WeaponAmmoClass[] alsoFits = null)
        {
            return new AmmoLoadDefinition
            {
                Id = id,
                DisplayName = display,
                CaliberId = caliberId,
                CaliberDisplay = caliberDisplay,
                WeaponClass = weaponClass,
                Rarity = rarity,
                Modification = mod,
                Craftable = craftable,
                BaseDamage = dmg,
                EffectiveRangeMeters = range,
                WeightPerRound = weight,
                StackMax = stack,
                TradeValue = trade,
                Description = desc,
                ExclusiveSources = sources ?? Array.Empty<AmmoFactionSource>(),
                AlsoFits = alsoFits ?? Array.Empty<WeaponAmmoClass>()
            };
        }

        private static void Add(AmmoLoadDefinition load)
        {
            if (load == null || string.IsNullOrEmpty(load.Id)) return;
            Catalog[load.Id] = load;
        }
    }
}
