using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Utilities
{
    /// <summary>
    /// Central, cached lookup for item icons, module icons, portraits and audio cues.
    ///
    /// Three things this solves that a bare <c>Resources.Load</c> call site does not:
    ///
    /// 1. <b>Repeat cost.</b> Results are memoised, so drawing an inventory list of 40
    ///    stacks does 40 dictionary hits rather than 40 asset lookups per repaint.
    /// 2. <b>Negative caching.</b> Misses are remembered too. Without this, every
    ///    not-yet-authored sprite costs a real filesystem/bundle probe on every single
    ///    frame that tries to draw it — the failure mode gets *more* expensive the more
    ///    art is missing, which is exactly backwards during production.
    /// 3. <b>Visibility.</b> Every distinct miss is recorded once and logged once, so a
    ///    missing sprite surfaces as an actionable list instead of either a silent blank
    ///    or a per-frame console flood.
    ///
    /// Missing art deliberately does not throw: the game must stay playable while the
    /// art pipeline is still filling in, which is the state this project is in now.
    /// </summary>
    public sealed class GameAssetService
    {
        private readonly IGameAssetProvider _provider;

        // Keyed by path. Separate maps per asset family keep the dictionary values
        // strongly typed without boxing or per-lookup casts.
        private readonly Dictionary<string, Sprite> _sprites = new Dictionary<string, Sprite>();
        private readonly Dictionary<string, AudioClip> _clips = new Dictionary<string, AudioClip>();

        // Paths already known to be absent, so we probe the backing store at most once.
        private readonly HashSet<string> _knownMissing = new HashSet<string>();

        /// <summary>Fallback sprite drawn where art has not been authored yet. Optional.</summary>
        public Sprite PlaceholderSprite { get; set; }

        /// <summary>When true (default), the first miss for a path is logged once.</summary>
        public bool LogMissingOnce { get; set; } = true;

        public GameAssetService(IGameAssetProvider provider = null)
        {
            _provider = provider ?? new ResourcesAssetProvider();
        }

        // ------------------------------------------------------------------
        // Sprites
        // ------------------------------------------------------------------

        /// <summary>Icon for an item id, or the placeholder when not yet authored.</summary>
        public Sprite GetItemIcon(string itemId) => GetSprite(GameAssetKeys.ItemIcon(itemId));

        /// <summary>Icon for a shelter module id, or the placeholder.</summary>
        public Sprite GetShelterModuleIcon(string moduleId) =>
            GetSprite(GameAssetKeys.ShelterModuleIcon(moduleId));

        /// <summary>Emblem for a faction id, or the placeholder.</summary>
        public Sprite GetFactionEmblem(string factionId) =>
            GetSprite(GameAssetKeys.FactionEmblem(factionId));

        /// <summary>Portrait for a survivor portrait id, or the placeholder.</summary>
        public Sprite GetSurvivorPortrait(string portraitId) =>
            GetSprite(GameAssetKeys.SurvivorPortrait(portraitId));

        public Sprite GetSprite(string path)
        {
            if (string.IsNullOrEmpty(path)) return PlaceholderSprite;
            if (_sprites.TryGetValue(path, out var cached)) return cached ?? PlaceholderSprite;
            if (_knownMissing.Contains(path)) return PlaceholderSprite;

            var loaded = _provider.Load<Sprite>(path);
            if (loaded == null)
            {
                RecordMissing(path);
                return PlaceholderSprite;
            }

            _sprites[path] = loaded;
            return loaded;
        }

        /// <summary>
        /// Sprite at <paramref name="path"/>, or <paramref name="fallback"/> when it is
        /// not authored yet. Used by UI call sites that have a legacy direct reference
        /// (e.g. <c>ItemDefinition.iconRef</c>) to degrade to while the Resources/Art
        /// pipeline fills in: new art wins automatically the moment a file lands.
        /// Deliberately bypasses <see cref="PlaceholderSprite"/> so an explicit
        /// per-entry fallback is never masked by the global placeholder.
        /// </summary>
        public Sprite GetSprite(string path, Sprite fallback)
        {
            if (string.IsNullOrEmpty(path)) return fallback;
            if (_sprites.TryGetValue(path, out var cached)) return cached ?? fallback;
            if (_knownMissing.Contains(path)) return fallback;

            var loaded = _provider.Load<Sprite>(path);
            if (loaded == null)
            {
                RecordMissing(path);
                return fallback;
            }

            _sprites[path] = loaded;
            return loaded;
        }

        // ------------------------------------------------------------------
        // Audio
        // ------------------------------------------------------------------

        /// <summary>One-shot SFX clip for a cue id, or null when not authored.</summary>
        public AudioClip GetSfx(string cueId) => GetClip(GameAssetKeys.Sfx(cueId));

        /// <summary>Music track for a track id, or null when not authored.</summary>
        public AudioClip GetMusic(string trackId) => GetClip(GameAssetKeys.Music(trackId));

        /// <summary>Looping ambience bed for an ambience id, or null when not authored.</summary>
        public AudioClip GetAmbience(string ambienceId) => GetClip(GameAssetKeys.Ambience(ambienceId));

        /// <summary>
        /// Audio has no placeholder: silence is an acceptable stand-in, whereas a
        /// stand-in *sound* would be actively misleading during production.
        /// </summary>
        public AudioClip GetClip(string path)
        {
            if (string.IsNullOrEmpty(path)) return null;
            if (_clips.TryGetValue(path, out var cached)) return cached;
            if (_knownMissing.Contains(path)) return null;

            var loaded = _provider.Load<AudioClip>(path);
            if (loaded == null)
            {
                RecordMissing(path);
                return null;
            }

            _clips[path] = loaded;
            return loaded;
        }

        // ------------------------------------------------------------------
        // Diagnostics / lifetime
        // ------------------------------------------------------------------

        /// <summary>
        /// Every path that was requested but does not exist. This is the art/audio
        /// work list, populated by actually playing the game.
        /// </summary>
        public IReadOnlyCollection<string> MissingPaths => _knownMissing;

        /// <summary>Count of assets currently held resident by this service.</summary>
        public int CachedAssetCount => _sprites.Count + _clips.Count;

        private void RecordMissing(string path)
        {
            if (!_knownMissing.Add(path)) return;
            if (LogMissingOnce)
                Debug.LogWarning($"[GameAssetService] No asset at '{path}' — using fallback.");
        }

        /// <summary>
        /// Drop cached references so Unity can unload the underlying assets. Call on
        /// scene transitions; without it the cache is a permanent floor on memory use.
        /// Negative-cache entries are kept: a path that was missing a moment ago is
        /// still missing, and re-probing it would reintroduce the per-frame cost.
        /// </summary>
        public void ClearCache()
        {
            _sprites.Clear();
            _clips.Clear();
        }

        /// <summary>Full reset including the missing-path record. Mainly for tests.</summary>
        public void Reset()
        {
            ClearCache();
            _knownMissing.Clear();
        }
    }
}
