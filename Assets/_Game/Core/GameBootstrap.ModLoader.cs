// GameBootstrap.ModLoader.cs — Prompt #864 community JSON mod loader boot.
using UnityEngine;
using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.Core
{
    public partial class GameBootstrap
    {
        /// <summary>
        /// Prompt #864 — construct mod loader, mark Active for importers,
        /// scan StreamingAssets/Mods (or persistentDataPath/Mods), load each mod.
        /// </summary>
        private void BootModLoader()
        {
            ModLoader = new System_ModLoader();
            ModLoader.SetAsActive();

            ModLoader.OnModDiscovered += modName =>
                GameLog.Log($"[GameBootstrap] MOD: discovered '{modName}'");
            ModLoader.OnModLoaded += (modName, itemCount) =>
                GameLog.Log($"[GameBootstrap] MOD: loaded '{modName}' ({itemCount} override(s))");
            ModLoader.OnLoadError += (modName, error) =>
                Debug.LogWarning($"[GameBootstrap] MOD: [{modName}] {error}");
            ModLoader.OnOverrideApplied += (dataId, modName) =>
                GameLog.Log($"[GameBootstrap] MOD: override '{dataId}' from '{modName}'");

            string path = System_ModLoader.ResolveDefaultModsPath();
            ModLoader.Initialize(path);
            int loaded = ModLoader.LoadAllMods();
            GameLog.Log($"[GameBootstrap] ModLoader ready: {loaded} mod(s) from '{path}'");
        }
    }
}
