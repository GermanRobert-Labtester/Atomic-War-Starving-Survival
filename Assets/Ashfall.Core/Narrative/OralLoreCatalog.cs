// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
#pragma warning disable CS8618

namespace Ashfall.Core.Narrative
{
    /// <summary>
    /// PLAN 155 Task A — one canonical in-memory record for the oral-lore corpus.
    ///
    /// Normalization contract (see docs/content/ORAL_LORE_SCHEMA_RECONCILIATION.md):
    ///   - Batch 1 (oral_lore_codex.json, root "songs"): authored numeric
    ///     <c>tempo_bpm</c> + <c>meter</c>; <c>tempo_descriptor</c> stays empty.
    ///   - Batch 2 (oral_lore_batch_2.json, root "entries"): authored textual
    ///     <c>tempo</c> is preserved verbatim in <c>tempo_descriptor</c>. A
    ///     numeric BPM is extracted ONLY from an explicit authored
    ///     <c>N_BPM</c> label (e.g. STEADY_MARCH_120_BPM → 120); every other
    ///     label (SLOW_MINOR, FREE_TIME, …) is never converted — BPM stays 0
    ///     and the descriptor is the whole truth.
    /// </summary>
    [Serializable]
    public sealed class OralLoreEntry
    {
        public string lore_id;
        public string title;
        public string genre;

        /// <summary>Normalized numeric tempo. Batch 1: the authored value. Batch 2: only from an explicit N_BPM label; otherwise 0 = unspecified.</summary>
        public int tempo_bpm;

        /// <summary>Batch 1 authored meter (e.g. "4/4 Rhythmic Chanted Verse"). Batch 2: empty — the descriptor carries the rhythm.</summary>
        public string meter;

        /// <summary>Batch 2 authored textual tempo, preserved verbatim. Batch 1: empty.</summary>
        public string tempo_descriptor;

        public string performance_context;
        public string lyrics;
        public string[] tags;

        /// <summary>Which source file this record normalized from (provenance; not persisted in saves).</summary>
        public string source_file;

        /// <summary>Player-safe tempo summary: descriptor if authored, else "N BPM" + meter, else "tempo unspecified".</summary>
        public string TempoSummary()
        {
            if (!string.IsNullOrEmpty(tempo_descriptor)) return tempo_descriptor;
            var parts = new List<string>();
            if (tempo_bpm > 0) parts.Add(tempo_bpm + " BPM");
            if (!string.IsNullOrEmpty(meter)) parts.Add(meter);
            return parts.Count > 0 ? string.Join(" — ", parts) : "tempo unspecified";
        }
    }

    /// <summary>Internal raw binding: BOTH source shapes bind through this DTO.</summary>
    [Serializable]
    internal sealed class OralLoreRawEntry
    {
        public string lore_id;
        public string title;
        public string genre;
        public int tempo_bpm;
        public string tempo;
        public string meter;
        public string performance_context;
        public string lyrics;
        public string[] tags;
    }

    [Serializable]
    internal sealed class OralLoreRawFile
    {
        public int schema_version;
        public string collection_id;
        public List<OralLoreRawEntry> songs;
        public List<OralLoreRawEntry> entries;
    }

    /// <summary>PLAN 155 Task B — explicit validation outcome of a Load call.</summary>
    [Serializable]
    public sealed class OralLoreLoadResult
    {
        public int loadedCount;
        public int duplicateSkippedCount;
        public List<string> duplicateIds = new List<string>();
        public bool WasDuplicateSource;
    }

    /// <summary>
    /// Engine-agnostic loader and query interface for the oral-lore corpus
    /// (16 canonical pieces + 10 Batch-2 pieces = 26).
    ///
    /// PLAN 155 Task B — lifecycle hardening:
    ///   - Reloading an identical source payload is a no-op (idempotent):
    ///     enumeration never duplicates.
    ///   - Duplicate lore_ids ACROSS sources follow one explicit precedence
    ///     rule: first load wins; later duplicates are skipped and recorded
    ///     (no silent overwrite, no double enumeration).
    ///   - Ordering is deterministic: AllSongs is ordinal-sorted by lore_id
    ///     after every load, independent of load order.
    ///   - Authored IDs are immutable: content additions never alter
    ///     previously known IDs (data, not code).
    /// </summary>
    public sealed class OralLoreCatalog
    {
        // Matches an authored explicit BPM claim inside a textual tempo label.
        private static readonly Regex ExplicitBpmPattern =
            new Regex(@"(\d+)\s*_?\s*BPM", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private readonly Dictionary<string, OralLoreEntry> _byLoreId =
            new Dictionary<string, OralLoreEntry>(StringComparer.OrdinalIgnoreCase);

        private readonly List<OralLoreEntry> _allSongs = new List<OralLoreEntry>();
        private readonly HashSet<int> _loadedSourceHashes = new HashSet<int>();

        public IReadOnlyList<OralLoreEntry> AllSongs => _allSongs;

        /// <summary>
        /// Loads one oral-lore source payload (canonical "songs" root or
        /// Batch-2 "entries" root; numeric or textual tempo). Idempotent per
        /// source payload. Returns the load result.
        /// </summary>
        public OralLoreLoadResult Load(string json, IJsonSerializer serializer, string sourceFile = "")
        {
            var result = new OralLoreLoadResult();
            if (string.IsNullOrEmpty(json) || serializer == null) return result;

            // Idempotence: identical payloads never double-enumerate.
            if (!_loadedSourceHashes.Add(StableHash.Of(json.Trim())))
            {
                result.WasDuplicateSource = true;
                return result;
            }

            var file = serializer.Deserialize<OralLoreRawFile>(json);
            if (file == null) return result;
            var rawSongs = file.songs;
            if (rawSongs == null || rawSongs.Count == 0) rawSongs = file.entries;
            if (rawSongs == null || rawSongs.Count == 0) return result;

            foreach (var raw in rawSongs)
            {
                if (raw == null || string.IsNullOrWhiteSpace(raw.lore_id)) continue;

                var normalized = Normalize(raw, sourceFile);
                if (_byLoreId.ContainsKey(normalized.lore_id))
                {
                    // Explicit precedence: first load wins.
                    result.duplicateSkippedCount++;
                    result.duplicateIds.Add(normalized.lore_id);
                    continue;
                }

                _byLoreId[normalized.lore_id] = normalized;
                _allSongs.Add(normalized);
                result.loadedCount++;
            }

            _allSongs.Sort((a, b) => string.CompareOrdinal(a.lore_id, b.lore_id));
            return result;
        }

        private static OralLoreEntry Normalize(OralLoreRawEntry raw, string sourceFile)
        {
            var entry = new OralLoreEntry
            {
                lore_id = raw.lore_id,
                title = raw.title ?? string.Empty,
                genre = raw.genre ?? string.Empty,
                meter = raw.meter ?? string.Empty,
                tempo_descriptor = raw.tempo ?? string.Empty,
                performance_context = raw.performance_context ?? string.Empty,
                lyrics = raw.lyrics ?? string.Empty,
                tags = raw.tags ?? Array.Empty<string>(),
                source_file = sourceFile ?? string.Empty
            };

            if (raw.tempo_bpm > 0)
            {
                entry.tempo_bpm = raw.tempo_bpm;
            }
            else if (!string.IsNullOrEmpty(raw.tempo))
            {
                // Authoritative extraction: only an explicit authored N_BPM
                // label yields a numeric BPM. Rhythmic adjectives never do.
                var match = ExplicitBpmPattern.Match(raw.tempo);
                if (match.Success && int.TryParse(match.Groups[1].Value, out int bpm))
                    entry.tempo_bpm = bpm;
            }
            return entry;
        }

        public OralLoreEntry? GetById(string loreId)
        {
            if (string.IsNullOrEmpty(loreId)) return null;
            _byLoreId.TryGetValue(loreId, out var song);
            return song;
        }

        public List<OralLoreEntry> GetByTag(string tag)
        {
            var results = new List<OralLoreEntry>();
            if (string.IsNullOrEmpty(tag)) return results;

            for (int i = 0; i < _allSongs.Count; i++)
            {
                var s = _allSongs[i];
                if (s.tags == null) continue;
                for (int j = 0; j < s.tags.Length; j++)
                {
                    if (string.Equals(s.tags[j], tag, StringComparison.OrdinalIgnoreCase))
                    {
                        results.Add(s);
                        break;
                    }
                }
            }
            return results;
        }

        public List<OralLoreEntry> GetByGenre(string genre)
        {
            var results = new List<OralLoreEntry>();
            if (genre == null) return results;

            for (int i = 0; i < _allSongs.Count; i++)
            {
                var s = _allSongs[i];
                if (s.genre != null && s.genre.IndexOf(genre, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    results.Add(s);
                }
            }
            return results;
        }
    }
}
