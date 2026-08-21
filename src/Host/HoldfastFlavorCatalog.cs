using System;
using System.Collections.Generic;
#pragma warning disable CS0649
using Godot;
using Ashfall.Core;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Godot-side flavor overlay keyed by canonical Holdfast item and faction IDs.
    /// Loaded from holdfast_flavor.json at startup. Missing keys fall back to neutral
    /// templates — flavor is an overlay, never a domain dependency.
    /// </summary>
    public sealed class HoldfastFlavorCatalog
    {
        private const string FileName = "holdfast_flavor.json";

        public Dictionary<string, string> ItemMarginalia { get; } = new Dictionary<string, string>(StringComparer.Ordinal);
        public Dictionary<string, FactionVoice> FactionVoices { get; } = new Dictionary<string, FactionVoice>(StringComparer.Ordinal);

        public string GetItemMarginalia(string itemId)
        {
            if (itemId == null) return NeutralItemMarginalia;
            return ItemMarginalia.TryGetValue(itemId, out string text) && !string.IsNullOrEmpty(text)
                ? text
                : NeutralItemMarginalia;
        }

        public FactionVoice GetFactionVoice(string factionId)
        {
            if (factionId == null) return NeutralFactionVoice;
            return FactionVoices.TryGetValue(factionId, out FactionVoice voice) ? voice : NeutralFactionVoice;
        }

        public static HoldfastFlavorCatalog Load(string dataDirectory, ILog log = null)
        {
            log ??= new GodotLog();
            var catalog = new HoldfastFlavorCatalog();
            if (string.IsNullOrEmpty(dataDirectory))
            {
                log.Warn("[Flavor] dataDirectory is null; flavor catalog is empty.");
                return catalog;
            }

            string path = System.IO.Path.Combine(dataDirectory, FileName);
            if (!System.IO.File.Exists(path))
            {
                log.Warn("[Flavor] " + FileName + " not found at " + path + "; flavor catalog is empty.");
                return catalog;
            }

            try
            {
                string json = System.IO.File.ReadAllText(path);
                var serializer = new SystemTextJsonSerializer();
                var root = serializer.Deserialize<FlavorRoot>(json);
                if (root == null)
                {
                    log.Warn("[Flavor] " + FileName + " deserialized to null.");
                    return catalog;
                }

                if (root.factions != null)
                {
                    foreach (var kv in root.factions)
                    {
                        if (!string.IsNullOrEmpty(kv.Key) && kv.Value != null)
                            catalog.FactionVoices[kv.Key] = kv.Value;
                    }
                }

                if (root.items != null)
                {
                    foreach (var kv in root.items)
                    {
                        if (!string.IsNullOrEmpty(kv.Key) && !string.IsNullOrEmpty(kv.Value))
                            catalog.ItemMarginalia[kv.Key] = kv.Value;
                    }
                }

                log.Info("[Flavor] Loaded " + catalog.ItemMarginalia.Count + " item marginalia, " +
                         catalog.FactionVoices.Count + " faction voices.");
            }
            catch (Exception e)
            {
                log.Error("[Flavor] Failed to load " + FileName + ": " + e.Message);
            }

            return catalog;
        }

        public const string NeutralItemMarginalia = "No marginalia on file for this item.";

        public static readonly FactionVoice NeutralFactionVoice = new FactionVoice
        {
            register = "neutral",
            voice = "The counterparty has no recorded voice.",
            rejected = "Transaction declined.",
            sold = "Item accepted."
        };

        public sealed class FactionVoice
        {
            public string register = "neutral";
            public string voice = string.Empty;
            public string rejected = string.Empty;
            public string sold = string.Empty;
        }

        private sealed class FlavorRoot
        {
            public Dictionary<string, FactionVoice> factions;
            public Dictionary<string, string> items;
        }
    }
}
