// SPDX-License-Identifier: MIT
// T32 — the difficulty authority's enemy_damage_mult must reach the combat
// engine (G-08 consumer-binding contract: the scalar flows from the authority,
// no parallel scalar exists). Same seed + doubled lookup = exactly doubled
// enemy hit damage; null lookup stays neutral; per-difficulty sweeps still
// terminate. Player-side damage is untouched by the hook.
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Combat;
using Ashfall.Core.Difficulty;
using Ashfall.Core.IO;

namespace Ashfall.Core.Tests.Combat
{
    public class EnemyDifficultyDamageBindingTests
    {
        public EnemyDifficultyDamageBindingTests()
        {
            CombatCatalog.SeedDefaults();
            CombatArenaCatalog.SeedDefaults();
        }

        private const int SeedCount = 12;
        private const int TickCap = 20000;

        private static TacticalCombatSystem BeginFight(int seed)
        {
            var sys = new TacticalCombatSystem();
            var roster = new List<CombatantState>
            {
                new CombatantState
                {
                    Id = "p0",
                    Name = "Scout",
                    SurvivorId = "survivor_0",
                    IsPlayer = true,
                    Health = 100,
                    MaxHealth = 100,
                    Lane = 1
                }
            };
            var weapons = new List<WeaponInstanceState>
            {
                new WeaponInstanceState
                {
                    InstanceId = "w0",
                    WeaponId = "weapon_assault_rifle",
                    OwnerSurvivorId = "survivor_0",
                    ConditionPct = 0.9f,
                    AmmoId = "ammo_556",
                    AmmoRemaining = 30
                }
            };
            Assert.True(sys.BeginEncounter(
                "enc_diff", "exp_diff", "loc_diff", "Difficulty Yard", 1, seed,
                roster, weapons, enemyCount: 1, enemyHealth: 40));
            return sys;
        }

        /// <summary>Health the player loses to the seeded turn-based enemy volley.</summary>
        private static float VolleyDamage(int seed, float? mult)
        {
            var sys = BeginFight(seed);
            if (mult.HasValue) sys.EnemyDamageMultLookup = () => mult.Value;
            float before = sys.State.Combatants.Find(c => c.IsPlayer).Health;
            sys.EndTurn(new SeededRng(seed));
            float after = sys.State.Combatants.Find(c => c.IsPlayer).Health;
            return before - after;
        }

        [Fact]
        public void DoubledLookup_DoublesDeterministicEnemyHitDamage()
        {
            int checkedSeeds = 0;
            for (int seed = 1; seed <= 60; seed++)
            {
                float neutral = VolleyDamage(seed, null);
                float doubled = VolleyDamage(seed, 2f);
                if (neutral <= 0f) continue; // seed where the enemy misses: skip, deterministic
                Assert.Equal(2f * neutral, doubled, 3);
                checkedSeeds++;
            }
            Assert.True(checkedSeeds >= 5, "seed panel must contain enough hitting seeds for a binding proof");
        }

        [Fact]
        public void NullLookup_IsNeutral_OneTimes()
        {
            bool foundHit = false;
            for (int seed = 1; seed <= 60; seed++)
            {
                float neutral = VolleyDamage(seed, null);
                if (neutral <= 0f) continue;
                Assert.Equal(neutral, VolleyDamage(seed, 1f), 3);
                foundHit = true;
            }
            Assert.True(foundHit, "panel must contain at least one enemy hit");
        }

        [Fact]
        public void OutOfBandLookup_ClampsToValidatedRange()
        {
            bool foundHit = false;
            for (int seed = 1; seed <= 60; seed++)
            {
                float clamped = VolleyDamage(seed, 2.5f);
                float absurd = VolleyDamage(seed, 9f);
                if (clamped <= 0f) continue;
                Assert.Equal(clamped, absurd, 3);
                foundHit = true;
            }
            Assert.True(foundHit, "panel must contain at least one enemy hit");
        }

        [Fact]
        public void PlayerDamage_IsNotScaledByEnemyHook()
        {
            // The hook multiplies enemy-dealt damage only: a player firing with
            // the hook set must land the same damage as without it.
            var plain = BeginFight(11);
            var hooked = BeginFight(11);
            hooked.EnemyDamageMultLookup = () => 2f;
            string enemyId = EnemyId(plain);
            var r1 = plain.PlayerFire(enemyId, new SeededRng(11), "p0");
            var r2 = hooked.PlayerFire(EnemyId(hooked), new SeededRng(11), "p0");
            Assert.Equal(r1.Success, r2.Success);
            if (r1.Success)
                Assert.Equal(EnemyHealth(plain), EnemyHealth(hooked), 3);
        }

        private static string EnemyId(TacticalCombatSystem sys)
        {
            var enemy = sys.State.Combatants.Find(c => !c.IsPlayer);
            Assert.NotNull(enemy);
            return enemy.Id;
        }

        private static float EnemyHealth(TacticalCombatSystem sys)
        {
            var enemy = sys.State.Combatants.Find(c => !c.IsPlayer);
            return enemy == null ? 0f : enemy.Health;
        }

        [Fact]
        public void RealtimeSweep_DoubledEnemyDamage_AllFightsTerminateOrVerifyExit()
        {
            // Per-difficulty sweep verification: at 2× enemy damage every seed
            // still terminates (loss resolves) or keeps the extract exit live.
            for (int seed = 1; seed <= SeedCount; seed++)
            {
                var sys = BeginFight(seed);
                sys.EnableRealtime(CombatArenaCatalog.DefaultArenaId, new SeededRng(seed));
                sys.EnemyDamageMultLookup = () => 2f;
                var rng = new SeededRng(seed);
                const float dt = TacticalCombatSystem.RealtimeSimDt;
                var input = new CombatInputFrame { SubjectId = "p0", FireHeld = true };
                int resolvedAt = -1;
                for (int i = 0; i < TickCap; i++)
                {
                    if (sys.State.Resolved) { resolvedAt = i; break; }
                    var tick = sys.TickRealtime(dt, input, rng);
                    Assert.True(tick.Success, $"seed {seed} tick {i}: {tick.Message}");
                }
                if (resolvedAt < 0)
                    Assert.True(sys.RequestFlee().Success,
                        $"seed {seed} DEAD-END: 2x enemy damage unresolved with no working exit");
                else
                    Assert.Contains(sys.State.Phase, new[]
                        { (int)CombatPhase.Won, (int)CombatPhase.Lost, (int)CombatPhase.Retreated });
            }
        }

        [Fact]
        public void AuthoredPresets_CarryValidEnemyDamageScalar()
        {
            string path = Path.Combine(FindDataDir(), "difficulty_presets.json");
            var catalog = DifficultyPresetCatalogLoader.LoadFromJson(
                new FileSystemIO().ReadAllText(path));
            Assert.NotNull(catalog);
            Assert.True(catalog.presets.Count >= 4, "all four authored presets must carry the scalar");

            foreach (var preset in catalog.presets)
            {
                Assert.True(preset.scalars.Validate(out string error),
                    preset.id + ": " + error);
            }

            var standard = FindPreset(catalog, "difficulty_standard");
            Assert.Equal(1f, standard.enemy_damage_mult, 3);
            var sparing = FindPreset(catalog, "difficulty_sparing");
            var dirge = FindPreset(catalog, "difficulty_dirge");
            Assert.True(dirge.enemy_damage_mult > standard.enemy_damage_mult,
                "dirge must hit harder than standard");
            Assert.True(standard.enemy_damage_mult > sparing.enemy_damage_mult,
                "standard must hit harder than sparing");
        }

        private static DifficultyScalars FindPreset(DifficultyPresetCatalog catalog, string id)
        {
            foreach (var p in catalog.presets)
                if (p.id == id) return p.scalars;
            throw new KeyNotFoundException(id);
        }

        [Fact]
        public void LegacySettingsState_MissingScalar_NormalizesToNeutral()
        {
            // Pre-T32 saves persist custom scalars without enemy_damage_mult;
            // restore must not crash GetEffectiveProvider on the unset 0f.
            var legacy = new DifficultySettingsState
            {
                ActivePresetId = "difficulty_custom",
                IsCustom = true,
                CustomScalars = new DifficultyScalars
                {
                    hunger_rate_mult = 1.5f,
                    thirst_rate_mult = 1.4f,
                    radiation_gain_mult = 1.3f,
                    disease_onset_mult = 1.2f,
                    hostile_encounter_mult = 1.5f,
                    market_price_mult = 1.1f,
                    equipment_decay_mult = 1.2f,
                    crisis_deadline_mult = 0.9f
                }
            };
            var system = new DifficultySettingsSystem();
            system.RestoreState(legacy);

            var provider = system.GetEffectiveProvider(); // must not throw
            Assert.Equal(1f, provider.EnemyDamageMult, 3);
        }

        private static string FindDataDir()
        {
            string? dir = new DirectoryInfo(AppContext.BaseDirectory).FullName;
            for (int i = 0; i < 10 && dir != null; i++)
            {
                string probe = Path.Combine(dir, "Assets", "StreamingAssets", "Data", "difficulty_presets.json");
                if (File.Exists(probe))
                    return Path.Combine(dir, "Assets", "StreamingAssets", "Data");
                dir = Directory.GetParent(dir)?.FullName;
            }
            string cwd = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data");
            if (File.Exists(Path.Combine(cwd, "difficulty_presets.json")))
                return cwd;
            throw new DirectoryNotFoundException(
                "Assets/StreamingAssets/Data/difficulty_presets.json not found from " + AppContext.BaseDirectory);
        }
    }
}
