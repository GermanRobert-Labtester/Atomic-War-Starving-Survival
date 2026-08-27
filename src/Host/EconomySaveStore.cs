// ============================================================================
// Save Store : EconomySaveStore
// Core State : Ashfall.Core.Economy.MarketState
// Host Caller: Main.Economy / EconomyHostSession
// Purpose    : Wasteland market commodity prices, price shocks, supply-demand, and barter balances
// ============================================================================
using System;
#pragma warning disable CS8618
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Economy;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Host-side integrity envelope: the sim contract (MarketState) stays core
    /// versioned state; the checksum lives here so tampered saves are refused.
    /// </summary>
    public class EconomySaveEnvelope
    {
        public string Checksum = string.Empty;
        public MarketState State;
    }

    /// <summary>
    /// Economy (market port) save persistence — thin pattern sibling of the
    /// other host stores: user:// path, try/catch, codec, checksum envelope.
    /// </summary>
    public static class EconomySaveStore
    {
        public const string FileName = "economy_save.json";
        public const string SectionName = "economy";
    /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
    public static string TryCaptureDirect(MarketState state)
    {
        return TryCapture(state);
    }

    /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
    public static MarketState? TryRestoreDirect(string json)
    {
        return TryRestore(json);
    }

    /// <summary>Capture state to JSON without writing to disk.</summary>
    public static string TryCapture(MarketState state)
    {
        try
        {
            if (state == null) return string.Empty;
            return s_json.Serialize(state);
        }
        catch (Exception e)
        {
            GD.PrintErr("[EconomySaveStore] capture failed: " + e.Message);
            return string.Empty;
        }
    }

    /// <summary>Restore state from JSON without reading from disk.</summary>
    public static MarketState? TryRestore(string json)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(json)) return null;
            return s_json.Deserialize<MarketState>(json);
        }
        catch (Exception e)
        {
            GD.PrintErr("[EconomySaveStore] restore failed: " + e.Message);
            return null;
        }
    }


        private static readonly FileSystemIO s_files = new FileSystemIO();
        private static readonly SystemTextJsonSerializer s_json = new SystemTextJsonSerializer();

        public static string SavePath =>
            SaveSlotRoot.Resolve(FileName);

        public static bool Exists => s_files.FileExists(SavePath);

        public static bool TrySave(MarketState state) => TrySave(state, SavePath);

        public static bool TrySave(MarketState state, string path)
        {
            try
            {
                if (state == null) return false;
                string? dir = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(dir) && !System.IO.Directory.Exists(dir))
                    System.IO.Directory.CreateDirectory(dir);
                var envelope = new EconomySaveEnvelope
                {
                    Checksum = SaveChecksum.Compute(state),
                    State = state
                };
                System.IO.File.WriteAllText(path, s_json.Serialize(envelope));
                return true;
            }
            catch (Exception e)
            {
                GD.PrintErr("[Economy] save failed: " + e.Message);
                return false;
            }
        }

        public static MarketState? TryLoad() => TryLoad(SavePath);

        public static MarketState? TryLoad(string path)
        {
            try
            {
                if (!s_files.FileExists(path)) return null;
                string raw = s_files.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(raw)) return null;
                var envelope = s_json.Deserialize<EconomySaveEnvelope>(raw);
                if (envelope != null && envelope.State != null)
                {
                    if (string.IsNullOrEmpty(envelope.Checksum)) return null;
                    // Tamper gate: recompute over the state; mismatch refuses the save.
                    if (!string.Equals(SaveChecksum.Compute(envelope.State), envelope.Checksum,
                            StringComparison.Ordinal))
                        return null;
                    return envelope.State;
                }
                // Legacy migration: a bare MarketState (pre-checksum store shape)
                // has no envelope; accept it so an upgrade never silently loses
                // the economy. Legacy saves carry no checksum by definition.
                var legacy = s_json.Deserialize<MarketState>(raw);
                if (legacy != null && !string.IsNullOrEmpty(legacy.systemId))
                {
                    GD.Print("[Economy] legacy bare save migrated (pre-checksum shape).");
                    return legacy;
                }
                return null;
            }
            catch (Exception e)
            {
                GD.PrintErr("[Economy] load failed: " + e.Message);
                return null;
            }
        }
    }
}
