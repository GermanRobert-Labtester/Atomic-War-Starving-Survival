using System;
using System.Collections.Generic;

namespace Ashfall.Core.Narrative
{
    [Serializable]
    public sealed class OralLoreEntry
    {
        public string lore_id;
        public string title;
        public string genre;
        public int tempo_bpm;
        public string meter;
        public string performance_context;
        public string lyrics;
        public string[] tags;
    }

    [Serializable]
    public sealed class OralLoreCodexFile
    {
        public int schema_version;
        public string collection_id;
        public List<OralLoreEntry> songs = new List<OralLoreEntry>();
    }

    /// <summary>
    /// Engine-agnostic loader and query interface for The Oral Lore Codex (16 musical/poetic pieces).
    /// </summary>
    public sealed class OralLoreCatalog
    {
        private readonly Dictionary<string, OralLoreEntry> _byLoreId =
            new Dictionary<string, OralLoreEntry>(StringComparer.OrdinalIgnoreCase);

        private readonly List<OralLoreEntry> _allSongs = new List<OralLoreEntry>();

        public IReadOnlyList<OralLoreEntry> AllSongs => _allSongs;

        public void Load(string json, IJsonSerializer serializer)
        {
            if (string.IsNullOrEmpty(json) || serializer == null) return;
            var file = serializer.Deserialize<OralLoreCodexFile>(json);
            if (file?.songs == null) return;

            foreach (var song in file.songs)
            {
                if (song == null || string.IsNullOrEmpty(song.lore_id)) continue;
                _byLoreId[song.lore_id] = song;
                _allSongs.Add(song);
            }
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
            if (string.IsNullOrEmpty(genre)) return results;

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
