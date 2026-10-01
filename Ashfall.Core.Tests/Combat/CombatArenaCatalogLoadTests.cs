// SPDX-License-Identifier: MIT
// T30 — the authored arena catalog must load from the authoritative data
// directory (combat_arenas.json) instead of silently fighting every session
// on the procedural default. Also pins the absent-file fallback contract.
using System;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Combat;
using Ashfall.Core.IO;

namespace Ashfall.Core.Tests.Combat
{
    public class CombatArenaCatalogLoadTests
    {
        private static string FindDataDir()
        {
            string? dir = new DirectoryInfo(AppContext.BaseDirectory).FullName;
            for (int i = 0; i < 10 && dir != null; i++)
            {
                string probe = Path.Combine(dir, "Assets", "StreamingAssets", "Data", "combat_arenas.json");
                if (File.Exists(probe))
                    return Path.Combine(dir, "Assets", "StreamingAssets", "Data");
                dir = Directory.GetParent(dir)?.FullName;
            }
            string cwd = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data");
            if (File.Exists(Path.Combine(cwd, "combat_arenas.json")))
                return cwd;
            throw new DirectoryNotFoundException(
                "Assets/StreamingAssets/Data/combat_arenas.json not found from " + AppContext.BaseDirectory);
        }

        [Fact]
        public void Load_FromAuthoritativeDataDir_ResolvesAuthoredArena()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            bool loaded = CombatArenaCatalog.Load(FindDataDir(), files, json);

            Assert.True(loaded, "combat_arenas.json ships in the authoritative data dir and must load");
            Assert.Contains(CombatArenaCatalog.DefaultArenaId, CombatArenaCatalog.AllIds);

            var arena = CombatArenaCatalog.GetOrDefault(CombatArenaCatalog.DefaultArenaId);
            Assert.Equal(24f, arena.width);
            Assert.Equal(5.2f, arena.flee_speed);
            Assert.Equal(3, arena.lane_spines.Count);
            Assert.Equal(3, arena.player_spawns.Count);
            Assert.Equal(3, arena.enemy_spawns.Count);
            Assert.Equal(2, arena.cover_nodes.Count);
            Assert.NotNull(arena.extract_volume);
        }

        [Fact]
        public void Load_MissingDirectory_FallsBackToProceduralDefault()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            bool loaded = CombatArenaCatalog.Load(
                Path.Combine(Path.GetTempPath(), "ashfall_no_such_data_dir_" + Guid.NewGuid().ToString("N")),
                files, json);

            Assert.False(loaded, "absent data must report fallback, not success");
            Assert.Contains(CombatArenaCatalog.DefaultArenaId, CombatArenaCatalog.AllIds);
            Assert.True(CombatArenaCatalog.GetOrDefault(null).width > 0f);
        }
    }
}
