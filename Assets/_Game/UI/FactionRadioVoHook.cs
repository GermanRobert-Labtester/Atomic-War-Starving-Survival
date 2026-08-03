using System;
using UnityEngine;
using AtomicWar._Game.Economy;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// Diegetic VO / static for faction radio intercepts, keyed by channel tag
    /// (CH-7 MILBAND, CH-3 ASH ROAD, …) and intercept kind.
    /// Prefers assigned library / inspector clips; falls back to procedural
    /// stubs so play works before WAV assets import.
    /// </summary>
    public class FactionRadioVoHook : MonoBehaviour
    {
        [Serializable]
        public class ChannelClip
        {
            [Tooltip("Channel tag from DynamicEconomySystem.GetParleyChannelTag, e.g. CH-7 MILBAND.")]
            public string ChannelTag;
            public AudioClip Clip;
        }

        [Serializable]
        public class KindClip
        {
            [Tooltip("Intercept kind name: Succession, Surrender, Parley, HatchRepel.")]
            public string Kind;
            public AudioClip Clip;
        }

        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private FactionRadioVoLibrarySO _library;
        [SerializeField] private AudioClip _defaultStaticClip;
        [SerializeField] private ChannelClip[] _channelClips;
        [SerializeField] private KindClip[] _kindClips;
        [SerializeField] private bool _autoEnsureStubs = true;

        /// <summary>Last resolved channel tag (for tests / debug).</summary>
        public string LastChannelTag { get; private set; } = string.Empty;
        /// <summary>Last intercept kind played.</summary>
        public string LastKind { get; private set; } = string.Empty;
        /// <summary>True when the last TryPlay found a non-null clip.</summary>
        public bool LastPlayHadClip { get; private set; }
        /// <summary>How many times TryPlay successfully started a clip.</summary>
        public int PlayCount { get; private set; }
        /// <summary>True after EnsureBuiltInStubs filled missing slots.</summary>
        public bool HasBuiltInStubs { get; private set; }

        private void Awake()
        {
            if (_audioSource == null)
                _audioSource = GetComponent<AudioSource>();
            if (_audioSource == null)
                _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.playOnAwake = false;
            _audioSource.spatialBlend = 0f;
            ApplyLibraryIfPresent();
            if (_autoEnsureStubs)
                EnsureBuiltInStubs();
        }

        /// <summary>Optional library asset (prefab / HUD serialized field).</summary>
        public void SetLibrary(FactionRadioVoLibrarySO library)
        {
            _library = library;
            ApplyLibraryIfPresent();
        }

        public FactionRadioVoLibrarySO Library => _library;

        private void ApplyLibraryIfPresent()
        {
            if (_library == null) return;
            if (_library.DefaultStaticHiss != null)
                _defaultStaticClip = _library.DefaultStaticHiss;
            if (_library.ChannelClips != null && _library.ChannelClips.Length > 0)
            {
                _channelClips = new ChannelClip[_library.ChannelClips.Length];
                for (int i = 0; i < _library.ChannelClips.Length; i++)
                {
                    var e = _library.ChannelClips[i];
                    _channelClips[i] = new ChannelClip
                    {
                        ChannelTag = e != null ? e.ChannelTag : string.Empty,
                        Clip = e != null ? e.Clip : null
                    };
                }
            }
            if (_library.KindClips != null && _library.KindClips.Length > 0)
            {
                _kindClips = new KindClip[_library.KindClips.Length];
                for (int i = 0; i < _library.KindClips.Length; i++)
                {
                    var e = _library.KindClips[i];
                    _kindClips[i] = new KindClip
                    {
                        Kind = e != null ? e.Kind : string.Empty,
                        Clip = e != null ? e.Clip : null
                    };
                }
            }
        }

        /// <summary>
        /// Fill any missing channel / kind / default clips with procedural stubs.
        /// Idempotent; does not overwrite assigned WAV assets.
        /// </summary>
        public void EnsureBuiltInStubs()
        {
            if (_defaultStaticClip == null)
                _defaultStaticClip = RadioVoStubFactory.CreateHiss("stub_static_hiss", 0.55f, 0.12f);

            string mil = DynamicEconomySystem.GetParleyChannelTag(FactionSO.Ids.MilitaryRemnants);
            string scav = DynamicEconomySystem.GetParleyChannelTag(FactionSO.Ids.ScavengerCamp);
            string prep = DynamicEconomySystem.GetParleyChannelTag(FactionSO.Ids.DoomsdayPreppers);

            EnsureChannel(mil, () => RadioVoStubFactory.CreateDualTone("stub_ch7_milband", 620f, 480f));
            EnsureChannel(scav, () => RadioVoStubFactory.CreateTone("stub_ch3_ash_road", 280f, 0.35f, 0.2f));
            EnsureChannel(prep, () => RadioVoStubFactory.CreateDualTone("stub_ch11_stockpile", 330f, 440f, 0.16f));

            EnsureKind("Parley", () => RadioVoStubFactory.CreateDualTone("stub_kind_parley", 700f, 900f, 0.12f, 0.16f));
            EnsureKind("HatchRepel", () => RadioVoStubFactory.CreateDualTone("stub_kind_hatch", 180f, 120f, 0.14f, 0.2f));
            EnsureKind("Surrender", () => RadioVoStubFactory.CreateTone("stub_kind_surrender", 200f, 0.4f, 0.14f));
            EnsureKind("Succession", () => RadioVoStubFactory.CreateTone("stub_kind_succession", 510f, 0.28f, 0.18f));

            HasBuiltInStubs = true;
        }

        private void EnsureChannel(string tag, Func<AudioClip> make)
        {
            if (string.IsNullOrEmpty(tag) || FindChannelClip(tag) != null) return;
            int len = _channelClips != null ? _channelClips.Length : 0;
            var next = new ChannelClip[len + 1];
            if (_channelClips != null)
                Array.Copy(_channelClips, next, len);
            next[len] = new ChannelClip { ChannelTag = tag, Clip = make() };
            _channelClips = next;
        }

        private void EnsureKind(string kind, Func<AudioClip> make)
        {
            if (string.IsNullOrEmpty(kind) || FindKindClip(kind) != null) return;
            int len = _kindClips != null ? _kindClips.Length : 0;
            var next = new KindClip[len + 1];
            if (_kindClips != null)
                Array.Copy(_kindClips, next, len);
            next[len] = new KindClip { Kind = kind, Clip = make() };
            _kindClips = next;
        }

        private AudioClip FindChannelClip(string channelTag)
        {
            if (_channelClips == null || string.IsNullOrEmpty(channelTag)) return null;
            for (int i = 0; i < _channelClips.Length; i++)
            {
                var row = _channelClips[i];
                if (row?.Clip == null) continue;
                if (string.Equals(row.ChannelTag, channelTag, StringComparison.OrdinalIgnoreCase))
                    return row.Clip;
            }
            return null;
        }

        private AudioClip FindKindClip(string kind)
        {
            if (_kindClips == null || string.IsNullOrEmpty(kind)) return null;
            for (int i = 0; i < _kindClips.Length; i++)
            {
                var row = _kindClips[i];
                if (row?.Clip == null) continue;
                if (string.Equals(row.Kind, kind, StringComparison.OrdinalIgnoreCase))
                    return row.Clip;
            }
            return null;
        }

        /// <summary>
        /// Resolve and optionally play VO for a faction intercept.
        /// Uses channel-tag clips first, then kind, then default static.
        /// </summary>
        public bool TryPlay(string factionId, string kind)
        {
            string tag = DynamicEconomySystem.GetParleyChannelTag(factionId);
            return TryPlayChannel(tag, kind);
        }

        /// <summary>Play by explicit channel tag (tests / scripted beats / tuner).</summary>
        public bool TryPlayChannel(string channelTag, string kind)
        {
            if (_autoEnsureStubs && !HasBuiltInStubs)
                EnsureBuiltInStubs();

            LastChannelTag = channelTag ?? string.Empty;
            LastKind = kind ?? string.Empty;
            var clip = ResolveClip(channelTag, kind);
            LastPlayHadClip = clip != null;
            if (clip == null) return false;

            if (_audioSource != null && _audioSource.enabled)
                _audioSource.PlayOneShot(clip);
            PlayCount++;
            return true;
        }

        /// <summary>
        /// Resolve clip without playing. Order: channel tag → kind → default static.
        /// </summary>
        public AudioClip ResolveClip(string channelTag, string kind)
        {
            var byChannel = FindChannelClip(channelTag);
            if (byChannel != null) return byChannel;

            var byKind = FindKindClip(kind);
            if (byKind != null) return byKind;

            return _defaultStaticClip;
        }

        /// <summary>Test / editor helper: assign channel table without inspector.</summary>
        public void SetChannelClips(ChannelClip[] clips) => _channelClips = clips;

        /// <summary>Test / editor helper: assign kind table without inspector.</summary>
        public void SetKindClips(KindClip[] clips) => _kindClips = clips;

        /// <summary>Test / editor helper: default static hiss when no channel match.</summary>
        public void SetDefaultStaticClip(AudioClip clip) => _defaultStaticClip = clip;

        public void SetAudioSource(AudioSource source) => _audioSource = source;

        public void SetAutoEnsureStubs(bool enabled) => _autoEnsureStubs = enabled;

        public AudioClip DefaultStaticClip => _defaultStaticClip;
        public ChannelClip[] ChannelClips => _channelClips;
        public KindClip[] KindClips => _kindClips;
    }
}
