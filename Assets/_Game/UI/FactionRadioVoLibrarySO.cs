using System;
using UnityEngine;
using AtomicWar._Game.Economy;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// Assignable VO clip table for faction radio intercepts. Drop authored
    /// WAV stubs (Assets/_Game/Audio/Radio/) here and wire onto HUD /
    /// <see cref="FactionRadioVoHook"/>. Runtime EnsureBuiltInStubs covers
    /// missing clips when no library is assigned.
    /// </summary>
    [CreateAssetMenu(fileName = "FactionRadioVoLibrary", menuName = "ASHFALL/Audio/Faction Radio VO Library")]
    public class FactionRadioVoLibrarySO : ScriptableObject
    {
        [Serializable]
        public class ChannelEntry
        {
            public string ChannelTag;
            public AudioClip Clip;
        }

        [Serializable]
        public class KindEntry
        {
            public string Kind;
            public AudioClip Clip;
        }

        public AudioClip DefaultStaticHiss;
        public ChannelEntry[] ChannelClips;
        public KindEntry[] KindClips;

        /// <summary>Canonical channel tags used by the intercept strip + tuner.</summary>
        public static readonly string[] CanonicalChannelTags =
        {
            "CH-7 MILBAND",
            "CH-3 ASH ROAD",
            "CH-11 STOCKPILE"
        };

        public static string TagForFaction(string factionId) =>
            DynamicEconomySystem.GetParleyChannelTag(factionId);
    }
}
