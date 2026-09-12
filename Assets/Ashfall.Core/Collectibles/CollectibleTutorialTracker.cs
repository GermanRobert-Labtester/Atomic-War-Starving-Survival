// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core
{
    /// <summary>
    /// Content definition for an onboarding tutorial entry.
    /// </summary>
    public sealed class CollectibleTutorialEntry
    {
        public string Id { get; }
        public string Title { get; }
        public string Body { get; }
        public string AccessibleSummary { get; }

        public CollectibleTutorialEntry(string id, string title, string body)
        {
            Id = id ?? string.Empty;
            Title = title ?? string.Empty;
            Body = body ?? string.Empty;
            AccessibleSummary = $"Tutorial: {Title}. {Body}";
        }
    }

    /// <summary>
    /// Serializable DTO for persisting tutorial seen and queued states.
    /// </summary>
    [Serializable]
    public sealed class CollectibleTutorialSave
    {
        public int schema_version = 1;
        public string[] seen_tutorials = Array.Empty<string>();
        public string[] queued_tutorials = Array.Empty<string>();
    }

    /// <summary>
    /// Deterministic engine-agnostic tutorial tracker for collectible discoveries.
    /// Governs the "Cultural Artifacts" and "Reading and Discovering" onboarding triggers.
    /// Idempotent, save-safe, and non-spammy (Plan 47 Task 6).
    /// </summary>
    public sealed class CollectibleTutorialTracker
    {
        public const string CulturalArtifactsId = "cultural_artifacts";
        public const string ReadingAndDiscoveringId = "reading_and_discovering";

        public static readonly CollectibleTutorialEntry CulturalArtifacts = new CollectibleTutorialEntry(
            CulturalArtifactsId,
            "Cultural Artifacts",
            "Cultural Artifacts are surviving objects from the pre-war world. Discovering them records their history, and some can unlock knowledge, journal entries, or new locations."
        );

        public static readonly CollectibleTutorialEntry ReadingAndDiscovering = new CollectibleTutorialEntry(
            ReadingAndDiscoveringId,
            "Reading and Discovering",
            "Some artifacts contain useful information. Discovering them can unlock journal entries, knowledge, faction intel, or map locations. These discoveries are recorded permanently."
        );

        private static readonly Dictionary<string, CollectibleTutorialEntry> Catalog =
            new Dictionary<string, CollectibleTutorialEntry>(StringComparer.Ordinal)
            {
                { CulturalArtifactsId, CulturalArtifacts },
                { ReadingAndDiscoveringId, ReadingAndDiscovering }
            };

        private readonly HashSet<string> _seen = new HashSet<string>(StringComparer.Ordinal);
        private readonly Queue<CollectibleTutorialEntry> _queue = new Queue<CollectibleTutorialEntry>();

        public int SeenCount => _seen.Count;
        public int QueueCount => _queue.Count;

        public bool HasSeen(string tutorialId)
        {
            if (string.IsNullOrEmpty(tutorialId)) return false;
            return _seen.Contains(tutorialId);
        }

        public bool MarkSeen(string tutorialId)
        {
            if (string.IsNullOrEmpty(tutorialId)) return false;
            return _seen.Add(tutorialId);
        }

        public IReadOnlyList<CollectibleTutorialEntry> QueuedEntries => _queue.ToArray();

        public bool TryDequeue(out CollectibleTutorialEntry? entry)
        {
            if (_queue.Count > 0)
            {
                entry = _queue.Dequeue();
                return true;
            }
            entry = null;
            return false;
        }

        public static CollectibleTutorialEntry? GetEntry(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return Catalog.TryGetValue(id, out var entry) ? entry : null;
        }

        /// <summary>
        /// Handles a newly registered collectible discovery result.
        /// Queues Cultural Artifacts on first collectible discovery.
        /// Queues Reading and Discovering on first effect-bearing collectible discovery.
        /// When the first collectible is effect-bearing, queues Cultural Artifacts first,
        /// then Reading and Discovering in stable deterministic order.
        /// </summary>
        public void OnCollectibleDiscovered(CollectibleDispatchResult result)
        {
            if (result == null || !result.IsCollectible || !result.DiscoveryRegistered)
                return;

            // Trigger 1: Cultural Artifacts on first-ever collectible discovery
            if (!_seen.Contains(CulturalArtifactsId))
            {
                _seen.Add(CulturalArtifactsId);
                _queue.Enqueue(CulturalArtifacts);
            }

            // Trigger 2: Reading and Discovering on first effect-bearing collectible
            if (result.HasDiscoveryEffects && !_seen.Contains(ReadingAndDiscoveringId))
            {
                _seen.Add(ReadingAndDiscoveringId);
                _queue.Enqueue(ReadingAndDiscovering);
            }
        }

        /// <summary>Captures seen and queued state for persistence.</summary>
        public CollectibleTutorialSave CaptureState()
        {
            var seenArray = new string[_seen.Count];
            _seen.CopyTo(seenArray, 0);
            Array.Sort(seenArray, StringComparer.Ordinal);

            var queuedIds = new List<string>(_queue.Count);
            foreach (var item in _queue)
            {
                queuedIds.Add(item.Id);
            }

            return new CollectibleTutorialSave
            {
                schema_version = 1,
                seen_tutorials = seenArray,
                queued_tutorials = queuedIds.ToArray()
            };
        }

        /// <summary>Restores seen and queued state from save. Historical discoveries do not trigger tutorials.</summary>
        public void RestoreState(CollectibleTutorialSave? save)
        {
            _seen.Clear();
            _queue.Clear();

            if (save == null) return;

            if (save.seen_tutorials != null)
            {
                for (int i = 0; i < save.seen_tutorials.Length; i++)
                {
                    string id = save.seen_tutorials[i];
                    if (!string.IsNullOrEmpty(id)) _seen.Add(id);
                }
            }

            if (save.queued_tutorials != null)
            {
                for (int i = 0; i < save.queued_tutorials.Length; i++)
                {
                    string id = save.queued_tutorials[i];
                    if (!string.IsNullOrEmpty(id) && Catalog.TryGetValue(id, out var entry))
                    {
                        _queue.Enqueue(entry);
                    }
                }
            }
        }
    }
}
