using System;
using UnityEngine;
using AtomicWar._Game.Economy;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// Optional diegetic VO / static for faction radio intercepts, keyed by
    /// channel tag (CH-7 MILBAND, CH-3 ASH ROAD, …) and intercept kind.
    /// Clips may be left unassigned — resolution and play are null-safe so the
    /// strip works before audio assets exist.
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
        [SerializeField] private AudioClip _defaultStaticClip;
        [SerializeField] private ChannelClip[] _channelClips;
        [SerializeField] private KindClip[] _kindClips;

        /// <summary>Last resolved channel tag (for tests / debug).</summary>
        public string LastChannelTag { get; private set; } = string.Empty;
        /// <summary>Last intercept kind played.</summary>
        public string LastKind { get; private set; } = string.Empty;
        /// <summary>True when the last TryPlay found a non-null clip.</summary>
        public bool LastPlayHadClip { get; private set; }
        /// <summary>How many times TryPlay successfully started a clip.</summary>
        public int PlayCount { get; private set; }

        private void Awake()
        {
            if (_audioSource == null)
                _audioSource = GetComponent<AudioSource>();
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

        /// <summary>Play by explicit channel tag (tests / scripted beats).</summary>
        public bool TryPlayChannel(string channelTag, string kind)
        {
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
            if (!string.IsNullOrEmpty(channelTag) && _channelClips != null)
            {
                for (int i = 0; i < _channelClips.Length; i++)
                {
                    var row = _channelClips[i];
                    if (row == null || row.Clip == null) continue;
                    if (string.Equals(row.ChannelTag, channelTag, StringComparison.OrdinalIgnoreCase))
                        return row.Clip;
                }
            }

            if (!string.IsNullOrEmpty(kind) && _kindClips != null)
            {
                for (int i = 0; i < _kindClips.Length; i++)
                {
                    var row = _kindClips[i];
                    if (row == null || row.Clip == null) continue;
                    if (string.Equals(row.Kind, kind, StringComparison.OrdinalIgnoreCase))
                        return row.Clip;
                }
            }

            return _defaultStaticClip;
        }

        /// <summary>Test / editor helper: assign channel table without inspector.</summary>
        public void SetChannelClips(ChannelClip[] clips) => _channelClips = clips;

        /// <summary>Test / editor helper: assign kind table without inspector.</summary>
        public void SetKindClips(KindClip[] clips) => _kindClips = clips;

        /// <summary>Test / editor helper: default static hiss when no channel match.</summary>
        public void SetDefaultStaticClip(AudioClip clip) => _defaultStaticClip = clip;

        public void SetAudioSource(AudioSource source) => _audioSource = source;
    }
}
