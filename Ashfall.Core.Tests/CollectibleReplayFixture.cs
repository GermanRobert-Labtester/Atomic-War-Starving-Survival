using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Inventory;
using Ashfall.Core.Journal;
using Ashfall.Core.Survivors;
using Ashfall.Core.World;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Trace entry recording a single collectible interaction or effect dispatch (Section 6).
    /// Contains only deterministic domain data — no timestamps or object references.
    /// </summary>
    public sealed class CollectibleReplayTraceEntry
    {
        public int SequenceIndex { get; set; }
        public string LocationId { get; set; } = string.Empty;
        public string CollectibleItemId { get; set; } = string.Empty;
        public string EffectType { get; set; } = string.Empty;
        public string EffectTargetOrKey { get; set; } = string.Empty;
        public string EffectPayloadNormalized { get; set; } = string.Empty;
        public bool WasFirstDiscovery { get; set; }

        public override string ToString() =>
            $"[{SequenceIndex}] loc={LocationId} item={CollectibleItemId} effect={EffectType} target={EffectTargetOrKey} payload={EffectPayloadNormalized} first={WasFirstDiscovery}";
    }

    /// <summary>
    /// Normalized final snapshot of collectible and sink state for deterministic equality and hashing.
    /// </summary>
    public sealed class CollectibleReplaySnapshot
    {
        public string[] DiscoveredIds { get; set; } = Array.Empty<string>();
        public float SurvivorMorale { get; set; }
        public string[] UnlockedKnowledge { get; set; } = Array.Empty<string>();
        public int JournalEntryCount { get; set; }
        public int CodexUnlockCount { get; set; }
        public string[] RevealedMapNodes { get; set; } = Array.Empty<string>();
        public List<CollectibleReplayTraceEntry> Trace { get; set; } = new List<CollectibleReplayTraceEntry>();
    }

    /// <summary>
    /// Computes canonical UTF-8 SHA256 hashes of collectible replay snapshots.
    /// </summary>
    public static class CollectibleStateHasher
    {
        public static string ComputeHash(CollectibleReplaySnapshot snapshot)
        {
            if (snapshot == null) return string.Empty;

            var sb = new StringBuilder();
            sb.Append("DISCOVERED:");
            var sortedIds = (string[])snapshot.DiscoveredIds.Clone();
            Array.Sort(sortedIds, StringComparer.Ordinal);
            for (int i = 0; i < sortedIds.Length; i++)
            {
                sb.Append(sortedIds[i]).Append(';');
            }
            sb.Append('\n');

            sb.Append("MORALE:").Append(snapshot.SurvivorMorale.ToString("F3", System.Globalization.CultureInfo.InvariantCulture)).Append('\n');

            sb.Append("KNOWLEDGE:");
            var sortedKnowledge = (string[])snapshot.UnlockedKnowledge.Clone();
            Array.Sort(sortedKnowledge, StringComparer.Ordinal);
            for (int i = 0; i < sortedKnowledge.Length; i++)
            {
                sb.Append(sortedKnowledge[i]).Append(';');
            }
            sb.Append('\n');

            sb.Append("JOURNAL:").Append(snapshot.JournalEntryCount).Append(':').Append(snapshot.CodexUnlockCount).Append('\n');

            sb.Append("MAP:");
            var sortedMap = (string[])snapshot.RevealedMapNodes.Clone();
            Array.Sort(sortedMap, StringComparer.Ordinal);
            for (int i = 0; i < sortedMap.Length; i++)
            {
                sb.Append(sortedMap[i]).Append(';');
            }
            sb.Append('\n');

            sb.Append("TRACE_COUNT:").Append(snapshot.Trace.Count).Append('\n');
            for (int i = 0; i < snapshot.Trace.Count; i++)
            {
                var t = snapshot.Trace[i];
                sb.Append(t.SequenceIndex).Append('|')
                  .Append(t.LocationId).Append('|')
                  .Append(t.CollectibleItemId).Append('|')
                  .Append(t.EffectType).Append('|')
                  .Append(t.EffectTargetOrKey).Append('|')
                  .Append(t.EffectPayloadNormalized).Append('|')
                  .Append(t.WasFirstDiscovery).Append('\n');
            }

            using var sha = SHA256.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(sb.ToString());
            byte[] hash = sha.ComputeHash(bytes);
            var hashStr = new StringBuilder(hash.Length * 2);
            for (int i = 0; i < hash.Length; i++)
            {
                hashStr.Append(hash[i].ToString("x2"));
            }
            return hashStr.ToString();
        }
    }

    /// <summary>
    /// First-divergence diagnostic helper (Section 9).
    /// </summary>
    public static class CollectibleReplayDiagnostic
    {
        public static string? FindFirstDivergence(
            IReadOnlyList<CollectibleReplayTraceEntry> expectedTrace,
            IReadOnlyList<CollectibleReplayTraceEntry> actualTrace,
            int restoreIndex = -1)
        {
            int minLen = Math.Min(expectedTrace.Count, actualTrace.Count);
            for (int i = 0; i < minLen; i++)
            {
                var exp = expectedTrace[i];
                var act = actualTrace[i];

                if (!string.Equals(exp.CollectibleItemId, act.CollectibleItemId, StringComparison.Ordinal))
                {
                    bool afterRestore = restoreIndex >= 0 && i >= restoreIndex;
                    string prev = i > 0 ? $"Previous matching: {expectedTrace[i - 1]}" : "No previous entry";
                    return $"Divergence at index {i} (Location {exp.LocationId}, {(afterRestore ? "AFTER" : "BEFORE")} restore):\n" +
                           $"  Expected item: '{exp.CollectibleItemId}'\n" +
                           $"  Actual item:   '{act.CollectibleItemId}'\n" +
                           $"  {prev}";
                }

                if (!string.Equals(exp.EffectType, act.EffectType, StringComparison.Ordinal) ||
                    !string.Equals(exp.EffectTargetOrKey, act.EffectTargetOrKey, StringComparison.Ordinal) ||
                    !string.Equals(exp.EffectPayloadNormalized, act.EffectPayloadNormalized, StringComparison.Ordinal) ||
                    exp.WasFirstDiscovery != act.WasFirstDiscovery)
                {
                    bool afterRestore = restoreIndex >= 0 && i >= restoreIndex;
                    string prev = i > 0 ? $"Previous matching: {expectedTrace[i - 1]}" : "No previous entry";
                    return $"Divergence at index {i} (Item {exp.CollectibleItemId}, {(afterRestore ? "AFTER" : "BEFORE")} restore):\n" +
                           $"  Expected effect: type={exp.EffectType}, target={exp.EffectTargetOrKey}, payload={exp.EffectPayloadNormalized}, first={exp.WasFirstDiscovery}\n" +
                           $"  Actual effect:   type={act.EffectType}, target={act.EffectTargetOrKey}, payload={act.EffectPayloadNormalized}, first={act.WasFirstDiscovery}\n" +
                           $"  {prev}";
                }
            }

            if (expectedTrace.Count != actualTrace.Count)
            {
                return $"Trace length divergence: expected {expectedTrace.Count}, actual {actualTrace.Count}.";
            }

            return null;
        }
    }

    /// <summary>
    /// Reusable fixture managing campaign composition, scavenging progression,
    /// deterministic RNG, and save/restore for collectible replay verification.
    /// </summary>
    public sealed class CollectibleReplayFixture
    {
        public static readonly string DataDir = FindDataDir();
        private static readonly IFileIO FileIO = new FileSystemIO();
        private static readonly IJsonSerializer Serializer = new SystemTextJsonSerializer();

        public int Seed { get; }
        public Ashfall.Core.SeededRng Rng { get; }
        public CollectibleCatalog Catalog { get; }
        public ScavengingTableCatalog Tables { get; }
        public CollectibleDiscoveryState Discovery { get; }
        public UniqueItemClaimRegistry Claims { get; }
        public NeedsSystem Needs { get; }
        public ResearchSystem Research { get; }
        public JournalSystem Journal { get; }
        public WastelandMapSystem Map { get; }
        public CollectibleEffectDispatcher Dispatcher { get; }
        public List<CollectibleReplayTraceEntry> Trace { get; } = new List<CollectibleReplayTraceEntry>();
        public bool RecordTrace { get; set; } = true;

        private static string FindDataDir()
        {
            string? dir = new DirectoryInfo(AppContext.BaseDirectory).FullName;
            while (dir != null)
            {
                string probe = Path.Combine(dir, "Assets", "StreamingAssets", "Data", "collectibles.json");
                if (File.Exists(probe)) return Path.Combine(dir, "Assets", "StreamingAssets", "Data");
                dir = Path.GetDirectoryName(dir.TrimEnd(Path.DirectorySeparatorChar));
            }
            throw new DirectoryNotFoundException("data authority not found");
        }

        public CollectibleReplayFixture(int seed = 42)
        {
            Seed = seed;
            Rng = new Ashfall.Core.SeededRng(seed);

            Catalog = CollectibleCatalogLoader.Load(DataDir, FileIO, Serializer)
                ?? throw new InvalidOperationException("collectibles.json must load");

            Tables = ScavengingTableCatalog.LoadFromDirectory(DataDir, FileIO, Serializer)
                ?? throw new InvalidOperationException("scavenging tables must load");

            Discovery = new CollectibleDiscoveryState();

            var uniqueIds = new List<string>();
            foreach (var kv in Catalog.ByItemId)
            {
                if (kv.Value.unique) uniqueIds.Add(kv.Key);
            }
            Claims = new UniqueItemClaimRegistry(uniqueIds);

            Needs = new NeedsSystem();
            Needs.Register(new SurvivorNeedsState { Id = "survivor_alpha", Morale = 50f });

            Research = new ResearchSystem();
            ResearchKnowledgeCatalogLoader.LoadAndRegister(Research, DataDir, FileIO, Serializer);

            Journal = new JournalSystem();

            var mapCatalog = WastelandMapCatalogLoader.Load(DataDir, FileIO, Serializer);
            Map = new WastelandMapSystem(new WastelandMapState(), mapCatalog.nodes, mapCatalog.routes);

            Dispatcher = new CollectibleEffectDispatcher(
                Catalog,
                Discovery,
                needsProvider: () => Needs,
                researchProvider: () => Research,
                journalProvider: () => Journal,
                mapProvider: () => Map,
                dayProvider: () => 1);
        }

        /// <summary>
        /// Restore constructor: restores an existing campaign state into a fresh fixture.
        /// </summary>
        public CollectibleReplayFixture(
            int seed,
            ulong rngState,
            CollectibleDiscoverySave discoverySave,
            UniqueClaimSave uniqueSave,
            SurvivorNeedsState survivorState,
            ResearchState researchState,
            JournalSave journalSave,
            WastelandMapState mapState,
            List<CollectibleReplayTraceEntry> priorTrace)
            : this(seed)
        {
            // Restore RNG state
            Rng = new Ashfall.Core.SeededRng(seed, rngState);

            // Restore states
            Discovery.RestoreState(discoverySave);
            Claims.RestoreState(uniqueSave);

            if (Needs.Registered.Count > 0)
            {
                var s = Needs.Registered[0];
                s.Morale = survivorState.Morale;
                s.Health = survivorState.Health;
                s.Hunger = survivorState.Hunger;
                s.Thirst = survivorState.Thirst;
            }

            Research.RestoreState(researchState);
            Journal.RestoreState(journalSave);
            Map.RestoreState(mapState);

            Trace.AddRange(priorTrace);
        }

        /// <summary>
        /// Scavenges a specific location table.
        /// </summary>
        public ScavengingRollResult? ScavengeLocation(string locationId, string tableId)
        {
            var roll = Tables.RollLoot(tableId, Rng, id => Claims.IsAvailable(id));
            if (roll == null) return null;

            if (Catalog.IsCollectible(roll.ItemId))
            {
                var def = Catalog.GetByItemId(roll.ItemId)!;
                bool wasFirst = !Discovery.IsDiscovered(roll.ItemId);

                var dispatchResult = Dispatcher.DispatchOnAcquire(roll.ItemId);
                if (def.unique && dispatchResult.EffectApplied)
                {
                    Claims.TryClaim(roll.ItemId);
                }

                if (RecordTrace)
                {
                    Trace.Add(new CollectibleReplayTraceEntry
                    {
                        SequenceIndex = Trace.Count,
                        LocationId = locationId,
                        CollectibleItemId = roll.ItemId,
                        EffectType = def.effect_type,
                        EffectTargetOrKey = def.effect_target,
                        EffectPayloadNormalized = def.effect_value.ToString("F1", System.Globalization.CultureInfo.InvariantCulture),
                        WasFirstDiscovery = wasFirst
                    });
                }
            }

            return roll;
        }

        public CollectibleReplaySnapshot ExtractSnapshot()
        {
            var save = Discovery.CaptureState();
            var discovered = save?.discovered_ids ?? Array.Empty<string>();

            float morale = Needs.Registered.Count > 0 ? Needs.Registered[0].Morale : 0f;

            var knowledgeList = new List<string>(Research.State.unlockedIds);
            knowledgeList.Sort(StringComparer.Ordinal);

            var mapNodes = new List<string>(Map.State.Discovered);
            mapNodes.Sort(StringComparer.Ordinal);

            return new CollectibleReplaySnapshot
            {
                DiscoveredIds = discovered,
                SurvivorMorale = morale,
                UnlockedKnowledge = knowledgeList.ToArray(),
                JournalEntryCount = Journal.EntryCount,
                CodexUnlockCount = Journal.CodexUnlockCount,
                RevealedMapNodes = mapNodes.ToArray(),
                Trace = new List<CollectibleReplayTraceEntry>(Trace)
            };
        }
    }
}
