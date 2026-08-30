using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Combat;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Combat Expansion — engine-agnostic Core coverage. Invariants exercised:
    /// data authority is JSON (combat_catalog.json), every roll flows through
    /// ISeededRng (determinism), CaptureState/RestoreState deep round-trips,
    /// and the vertical-slice headless demo passes end to end.
    /// </summary>
    public class CombatSystemTests
    {
        private static string DataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found))
                return found;
            if (CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out found))
                return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        private static bool LoadCombatCatalog()
        {
            return CombatCatalogLoader.Load(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
        }

        // -----------------------------------------------------------------
        // Data authority (combat_catalog.json)
        // -----------------------------------------------------------------

        [Fact]
        public void Catalog_LoadsFromDataAuthority_WithCanonicalIds()
        {
            CombatCatalog.Clear();
            Assert.True(LoadCombatCatalog(), "combat_catalog.json loads from the data directory");

            Assert.NotNull(CombatCatalog.GetWeapon("weapon_pipe_rifle"));
            Assert.NotNull(CombatCatalog.GetWeapon("weapon_assault_rifle"));
            Assert.NotNull(CombatCatalog.GetWeapon("weapon_lmg"));
            Assert.NotNull(CombatCatalog.GetAmmo("ammo_556"));
            Assert.NotNull(CombatCatalog.GetMaterial("material_metal"));
            Assert.NotNull(CombatCatalog.GetMaterial("armor_plate"));

            // JSON is the value authority: mapped values match the authored data.
            var pipe = CombatCatalog.GetWeapon("weapon_pipe_rifle");
            Assert.Equal(12f, pipe.damage);
            Assert.Equal(0.30f, pipe.conditionThreshold);
            Assert.True(pipe.isJuryRigged);
            Assert.Equal("ammo_357", pipe.caliber);
            Assert.True(CombatCatalog.GetAmmo("ammo_556").isMilitaryTier);
        }

        [Fact]
        public void Catalog_SeedDefaults_LoadsTheJsonAuthority()
        {
            // SeedDefaults (called by the TacticalCombatSystem ctor) must populate
            // from the committed JSON, not hardcoded literals (Invariant #6).
            CombatCatalog.Clear();
            CombatCatalog.SeedDefaults();
            Assert.NotNull(CombatCatalog.GetWeapon("weapon_pipe_rifle"));
            Assert.NotNull(CombatCatalog.GetWeapon("weapon_assault_rifle"));
            Assert.NotNull(CombatCatalog.GetAmmo("ammo_308"));
            Assert.NotNull(CombatCatalog.GetMaterial("armor_kevlar"));
            // Not the minimal single-weapon fallback — a full catalog loaded.
            Assert.True(CombatCatalog.WeaponIds.Count >= 5);
        }

        // -----------------------------------------------------------------
        // Vertical-slice demo (covers determinism, save round-trip,
        // ballistics, weapon condition/jam/ash, migration)
        // -----------------------------------------------------------------

        [Fact]
        public void HeadlessDemo_AllChecksPass()
        {
            var report = CombatHeadlessDemo.Run();
            Assert.True(report.Passed, report.Summary);
            Assert.Equal(0, report.FailedCount);
        }

        // -----------------------------------------------------------------
        // Determinism
        // -----------------------------------------------------------------

        [Fact]
        public void SameSeed_ReproducesIdenticalCombat()
        {
            // Build a scenario description (the captured event log) twice from
            // identical seeds; they must be byte-identical.
            var a = RunEncounterLog(4044);
            var b = RunEncounterLog(4044);
            Assert.Equal(a, b);

            var c = RunEncounterLog(9090);
            Assert.NotEqual(a, c); // different seed diverges
        }

        private static string RunEncounterLog(int seed)
        {
            var sys = new TacticalCombatSystem();
            var players = new List<CombatantState>
            {
                new CombatantState { Id = "p1", Name = "Yuki", SurvivorId = "survivor_yuki", IsPlayer = true, Health = 100, MaxHealth = 100, ArmorRating = 0.4f, CoverRating = 0.3f },
                new CombatantState { Id = "p2", Name = "Gunner", SurvivorId = "survivor_gunner", IsPlayer = true, Health = 100, MaxHealth = 100 }
            };
            var pw = new List<WeaponInstanceState>
            {
                new WeaponInstanceState { InstanceId = "wp1", WeaponId = "weapon_assault_rifle", OwnerSurvivorId = "survivor_yuki", ConditionPct = 0.95f, AmmoId = "ammo_556", AmmoRemaining = 60 },
                new WeaponInstanceState { InstanceId = "wp2", WeaponId = "weapon_pipe_rifle", OwnerSurvivorId = "survivor_gunner", ConditionPct = 0.8f, AmmoId = "ammo_357", AmmoRemaining = 40 }
            };
            sys.BeginEncounter("enc_r", "exp", "loc_x", "Loc", 1, seed, players, pw, 2, 40f);
            var rng = new Ashfall.Core.SeededRng(seed);
            sys.ResolveToEnd(rng, 40);
            var sb = new System.Text.StringBuilder();
            foreach (var e in sys.State.Events)
                sb.Append(e.Detail).Append("|").Append(e.Turn).Append(";");
            sb.Append("resolved=").Append(sys.State.Resolved);
            return sb.ToString();
        }

        // -----------------------------------------------------------------
        // Save round-trip + migration
        // -----------------------------------------------------------------

        [Fact]
        public void Save_RoundTrips_ThroughSerializer()
        {
            var sys = new TacticalCombatSystem();
            var players = new List<CombatantState>
            {
                new CombatantState { Id = "p1", Name = "Yuki", SurvivorId = "survivor_yuki", IsPlayer = true, Health = 100, MaxHealth = 100 }
            };
            var pw = new List<WeaponInstanceState>
            {
                new WeaponInstanceState { InstanceId = "wp1", WeaponId = "weapon_pipe_rifle", OwnerSurvivorId = "survivor_yuki", ConditionPct = 0.9f, AmmoId = "ammo_357", AmmoRemaining = 30 }
            };
            sys.BeginEncounter("enc_s", "exp", "loc_x", "Loc", 1, 55, players, pw, 1, 40f);
            sys.ResolveToEnd(new Ashfall.Core.SeededRng(55), 40);

            var json = new SystemTextJsonSerializer();
            var save = sys.CaptureState();
            string encoded = json.Serialize(save);
            var decoded = json.Deserialize<CombatState>(encoded);

            var restored = new TacticalCombatSystem();
            restored.RestoreState(decoded);
            Assert.Equal(sys.State.Resolved, restored.State.Resolved);
            Assert.Equal(sys.State.Phase, restored.State.Phase);
            Assert.Equal(sys.State.Events.Count, restored.CaptureState().Events.Count);
            Assert.Equal(sys.State.Combatants.Count, restored.State.Combatants.Count);
        }

        [Fact]
        public void Migrate_ClampsLegacyShape()
        {
            var legacy = new CombatState
            {
                SaveVersion = 1,
                Phase = 99
            };
            legacy.Combatants.Add(new CombatantState { Id = "l1", Name = "legacy", IsPlayer = true });
            var migrated = TacticalCombatSystem.Migrate(legacy);
            Assert.NotNull(migrated);
            Assert.Equal(CombatState.CurrentSaveVersion, migrated.SaveVersion);
            Assert.InRange(migrated.Phase, (int)CombatPhase.Setup, (int)CombatPhase.Retreated);
            Assert.Single(migrated.Combatants);
        }

        // -----------------------------------------------------------------
        // Data integrity
        // -----------------------------------------------------------------

        [Fact]
        public void CombatCatalog_IntroducesNoDataIntegrityErrors()
        {
            var report = CatalogIntegrityValidator.Validate(DataDir(), new FileSystemIO());
            for (int i = 0; i < report.Errors.Count; i++)
            {
                Assert.DoesNotContain("combat", report.Errors[i]);
            }
        }

        /// <summary>
        /// Regression for a production gap where every weapon's caliber
        /// referenced an ammo id (ammo_357/ammo_12g/ammo_308/ammo_556/
        /// ammo_762) that existed only in combat_catalog.json — items.json
        /// (the actual inventory item registry CombatHostSession.WireRealState
        /// checks via Inventory.CountById) had no matching entries at all. Once
        /// a live ammo-consumption port is wired (the real Main composition
        /// path — see Main.Expeditions.cs's SetupCombat), every weapon fire
        /// silently failed with "No X ammunition" regardless of what the
        /// player actually carried, because the item could never exist.
        /// Every combat weapon's caliber must resolve to a real inventory item.
        /// </summary>
        [Fact]
        public void EveryWeaponCaliber_ResolvesToARealInventoryItem()
        {
            CombatCatalog.Clear();
            Assert.True(LoadCombatCatalog(), "combat_catalog.json loads from the data directory");

            var itemCatalog = Ashfall.Core.Inventory.ItemCatalogLoader.LoadCatalog(
                DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());

            var missing = new List<string>();
            foreach (string weaponId in CombatCatalog.WeaponIds)
            {
                var weapon = CombatCatalog.GetWeapon(weaponId);
                if (weapon == null || string.IsNullOrEmpty(weapon.caliber)) continue;

                if (itemCatalog.Get(weapon.caliber) == null)
                    missing.Add($"{weaponId} -> caliber '{weapon.caliber}' has no items.json entry");
            }

            Assert.True(missing.Count == 0,
                "Weapon calibers with no matching inventory item (combat ammo can never be consumed from the real player inventory):\n  "
                + string.Join("\n  ", missing));
        }

    }
}
