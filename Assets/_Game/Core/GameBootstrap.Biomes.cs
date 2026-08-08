// GameBootstrap.Biomes.cs — boot/wire Biome_* expedition terrain modifiers.
using UnityEngine;

namespace AtomicWar._Game.Core
{
    public partial class GameBootstrap
    {
        /// <summary>
        /// Construct all Biome_* trackers. Host hooks are offline-safe logs;
        /// expedition hosts call Enter/Traverse/Scout when parties hit biome tiles.
        /// </summary>
        private void BootBiomes()
        {
            // DEMOTE-Biomes-001 — Biome_* trackers are unticked dormant classes.
            Debug.Log("[GameBootstrap] Biomes demoted (6 dormant trackers).");
        }

        private void WireBiomes()
        {
        }
    }
}
