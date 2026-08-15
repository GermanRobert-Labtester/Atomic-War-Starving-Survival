using System;
using System.IO;
using Ashfall.Core;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Godot's playable Holdfast boundary. The world session owns existing
    /// Holdfast systems; the Core trade session owns mutable inventory/value/stock.
    /// </summary>
    public sealed class HoldfastRuntimeSession
    {
        public const long DefaultStartingValue = 100;

        public CoreDemoSession World { get; }
        public HoldfastTradeSession Trade { get; }
        public HoldfastCatalog Catalog => World.Catalog;
        public string LastPersistenceMessage { get; private set; } = string.Empty;
        public bool HasPurchasedThisSession { get; set; }

        public event Action StateChanged;

        public HoldfastRuntimeSession(CoreDemoSession world, long startingValue = DefaultStartingValue)
        {
            World = world ?? throw new ArgumentNullException(nameof(world));
            Trade = new HoldfastTradeSession(World.Catalog, startingValue);
            Trade.StateChanged += () => StateChanged?.Invoke();
        }

        public static HoldfastRuntimeSession Create(
            CoreDemoSession world,
            bool seedDevelopmentState = true,
            bool loadTradeSave = true)
        {
            var session = new HoldfastRuntimeSession(world);
            if (loadTradeSave)
            {
                var saved = HoldfastTradeSaveStore.TryLoad();
                if (saved != null)
                {
                    if (!session.Trade.TryRestoreState(saved, out string error))
                        session.LastPersistenceMessage = "Holdfast trade save rejected: " + error;
                    else
                        session.LastPersistenceMessage = "Holdfast player and store state restored.";
                }
                else if (seedDevelopmentState)
                {
                    session.SeedDevelopmentState();
                }
            }
            else if (seedDevelopmentState)
            {
                session.SeedDevelopmentState();
            }
            return session;
        }

        public bool TrySave(string basePathOverride = null, string tradePathOverride = null)
        {
            bool baseSaved = HoldfastSaveStore.TrySave(World.CaptureSave(), basePathOverride);
            bool tradeSaved = HoldfastTradeSaveStore.TrySave(Trade.CaptureState(), tradePathOverride);
            bool saved = baseSaved && tradeSaved;
            LastPersistenceMessage = saved
                ? "Holdfast player, store, and world state saved."
                : "Holdfast save failed; existing state remains in memory.";
            return saved;
        }

        public bool TryReload(string basePathOverride = null, string tradePathOverride = null)
        {
            bool restoredAny = false;
            var baseSave = HoldfastSaveStore.TryLoad(basePathOverride);
            if (baseSave != null)
            {
                World.RestoreSave(baseSave);
                restoredAny = true;
            }

            var tradeSave = HoldfastTradeSaveStore.TryLoad(tradePathOverride);
            if (tradeSave != null)
            {
                if (!Trade.TryRestoreState(tradeSave, out string error))
                {
                    LastPersistenceMessage = "Holdfast trade reload rejected: " + error;
                    return false;
                }
                restoredAny = true;
            }

            LastPersistenceMessage = restoredAny
                ? "Holdfast state reloaded from disk."
                : "No Holdfast save was available to reload.";
            return restoredAny;
        }

        public void SeedDevelopmentState()
        {
            Trade.SeedInventory("item_triplicate_carbon", 1);
        }

        public bool ArchiveAndFreshStart(string basePathOverride = null, string tradePathOverride = null)
        {
            try
            {
                string basePath = basePathOverride ?? HoldfastSaveStore.SavePath;
                string tradePath = tradePathOverride ?? HoldfastTradeSaveStore.SavePath;
                string timestamp = DateTime.UtcNow.ToString("yyyyMMdd-HHmmss", System.Globalization.CultureInfo.InvariantCulture);
                string archiveDir = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(basePath) ?? string.Empty, "holdfast_archive_" + timestamp);

                bool archived = true;
                if (System.IO.File.Exists(basePath))
                {
                    try { System.IO.Directory.CreateDirectory(archiveDir); System.IO.File.Move(basePath, System.IO.Path.Combine(archiveDir, System.IO.Path.GetFileName(basePath))); }
                    catch (Exception) { archived = false; }
                }
                if (System.IO.File.Exists(tradePath))
                {
                    try { System.IO.Directory.CreateDirectory(archiveDir); System.IO.File.Move(tradePath, System.IO.Path.Combine(archiveDir, System.IO.Path.GetFileName(tradePath))); }
                    catch (Exception) { archived = false; }
                }

                // Reset mutable state.
                Trade.ResetToDefaults();
                HasPurchasedThisSession = false;
                LastPersistenceMessage = archived
                    ? "New ledger started. Prior records archived to " + System.IO.Path.GetFileName(archiveDir) + "."
                    : "New ledger started. Prior records could not be archived but have been cleared.";
                StateChanged?.Invoke();
                return true;
            }
            catch (Exception e)
            {
                LastPersistenceMessage = "Fresh start failed: " + e.Message;
                return false;
            }
        }
    }
}
