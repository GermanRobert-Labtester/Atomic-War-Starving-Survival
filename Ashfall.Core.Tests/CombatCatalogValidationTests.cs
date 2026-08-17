using System;
using System.IO;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Combat;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Loader validation (Invariant #6 / data-authority): a future schema is
    /// rejected (throw, never silently guessed), and canonical snake_case id /
    /// cross-reference violations surface as errors so a malformed catalog can
    /// never slip into the simulation.
    /// </summary>
    public class CombatCatalogValidationTests
    {
        private static string TempDir()
        {
            string dir = Path.Combine(Path.GetTempPath(), "ashfall_combat_catalog_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
            return dir;
        }

        private static bool TryLoad(string dir)
        {
            return CombatCatalogLoader.Load(dir, new FileSystemIO(), new SystemTextJsonSerializer());
        }

        /// <summary>Restore the canonical JSON-backed registry so shared-state
        /// combat tests never see the throwaway fixtures left by validation tests.</summary>
        private static void RestoreDefaults()
        {
            CombatCatalog.Clear();
            CombatCatalog.SeedDefaults();
        }

        private static void WriteJson(string dir, string json)
        {
            File.WriteAllText(Path.Combine(dir, CombatCatalogLoader.FileName), json);
        }

        [Fact]
        public void Load_WithEmptyFile_ReturnsFalse()
        {
            string dir = TempDir();
            WriteJson(dir, "");
            Assert.False(TryLoad(dir));
        }

        [Fact]
        public void Validate_RejectsFutureSchema()
        {
            string dir = TempDir();
            WriteJson(dir, "{ \"schema_version\": 99, \"weapons\": [], \"ammo\": [], \"materials\": [] }");
            Assert.Throws<System.IO.InvalidDataException>(() => TryLoad(dir));
        }

        [Fact]
        public void Validate_RejectsNonCanonicalWeaponId()
        {
            string dir = TempDir();
            WriteJson(dir, "{ \"schema_version\": 1, \"weapons\": [ { \"id\": \"boomstick\" } ], \"ammo\": [], \"materials\": [] }");
            try { Assert.Throws<FormatException>(() => TryLoad(dir)); }
            finally { RestoreDefaults(); }
        }

        [Fact]
        public void Validate_RejectsUnknownCaliberReference()
        {
            string dir = TempDir();
            WriteJson(dir,
                "{ \"schema_version\": 1, \"weapons\": [ { \"id\": \"weapon_x\", \"caliber\": \"ammo_nope\" } ], \"ammo\": [ { \"id\": \"ammo_357\" } ], \"materials\": [] }");
            try { Assert.Throws<FormatException>(() => TryLoad(dir)); }
            finally { RestoreDefaults(); }
        }

        [Fact]
        public void Load_ValidCatalog_RegistersEntities()
        {
            string dir = TempDir();
            WriteJson(dir, string.Join("\n",
                "{ \"schema_version\": 1,",
                "  \"weapons\": [ { \"id\": \"weapon_x\", \"display_name\": \"X\", \"caliber\": \"ammo_9\" } ],",
                "  \"ammo\": [ { \"id\": \"ammo_9\" } ],",
                "  \"materials\": [ { \"id\": \"material_rock\" } ] }"));
            try
            {
                Assert.True(TryLoad(dir));
                Assert.NotNull(CombatCatalog.GetWeapon("weapon_x"));
                Assert.NotNull(CombatCatalog.GetAmmo("ammo_9"));
                Assert.NotNull(CombatCatalog.GetMaterial("material_rock"));
            }
            finally { RestoreDefaults(); }
        }
    }
}
