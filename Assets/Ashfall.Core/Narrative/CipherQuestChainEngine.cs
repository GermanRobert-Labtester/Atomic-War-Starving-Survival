using System;
using System.Collections.Generic;
using Ashfall.Core.World;

namespace Ashfall.Core.Narrative
{
    /// <summary>
    /// Persistent state for the cipher hunt quest chains.
    /// </summary>
    [Serializable]
    public sealed class CipherQuestState
    {
        public string chainId = string.Empty;
        public bool isHeard;
        public bool isKeyFound;
        public bool isDecoded;
        public bool isLocationRevealed;
        public bool isResolved;
    }

    /// <summary>
    /// Definition of an authored cipher treasure hunt chain.
    /// </summary>
    public sealed class CipherChainDefinition
    {
        public string ChainId { get; set; } = string.Empty;
        public string QuestId { get; set; } = string.Empty;
        public string BroadcastId { get; set; } = string.Empty;
        public string CipherStationId { get; set; } = string.Empty;
        public string RequiredItemId { get; set; } = string.Empty;
        public string TargetLocationId { get; set; } = string.Empty;
        public string HeardFlag { get; set; } = string.Empty;
        public string KeyFoundFlag { get; set; } = string.Empty;
        public string DecodedFlag { get; set; } = string.Empty;
        public string RevealedFlag { get; set; } = string.Empty;
        public string ResolvedFlag { get; set; } = string.Empty;
    }

    /// <summary>
    /// Engine that orchestrates signal-intelligence cipher hunts, decoding progression,
    /// and map node reveals for Plan 11.
    /// </summary>
    public sealed class CipherQuestChainEngine
    {
        public static readonly List<CipherChainDefinition> Chains = new List<CipherChainDefinition>
        {
            new CipherChainDefinition
            {
                ChainId = "relay_count",
                QuestId = "quest_cipher_relay_count",
                BroadcastId = "radio_broadcast_relay_count",
                CipherStationId = "cipher_station_relay_count",
                RequiredItemId = "item_comm_codebook_alpha",
                TargetLocationId = "loc_hidden_relay_bunker",
                HeardFlag = "flag_sig_relay_heard",
                KeyFoundFlag = "flag_sig_relay_key_found",
                DecodedFlag = "flag_sig_relay_decoded",
                RevealedFlag = "flag_sig_relay_location_revealed",
                ResolvedFlag = "flag_sig_relay_resolved"
            },
            new CipherChainDefinition
            {
                ChainId = "winter_ledger",
                QuestId = "quest_cipher_winter_ledger",
                BroadcastId = "radio_broadcast_winter_ledger",
                CipherStationId = "cipher_station_winter_ledger",
                RequiredItemId = "item_logistics_cipher_sheet",
                TargetLocationId = "loc_logistics_reserve_cache",
                HeardFlag = "flag_sig_winter_heard",
                KeyFoundFlag = "flag_sig_winter_key_found",
                DecodedFlag = "flag_sig_winter_decoded",
                RevealedFlag = "flag_sig_winter_location_revealed",
                ResolvedFlag = "flag_sig_winter_resolved"
            },
            new CipherChainDefinition
            {
                ChainId = "last_rotation",
                QuestId = "quest_cipher_last_rotation",
                BroadcastId = "radio_broadcast_last_rotation",
                CipherStationId = "cipher_station_last_rotation",
                RequiredItemId = "item_archive_index_cylinder",
                TargetLocationId = "loc_deaddrop_command_shelter",
                HeardFlag = "flag_sig_rotation_heard",
                KeyFoundFlag = "flag_sig_rotation_key_found",
                DecodedFlag = "flag_sig_rotation_decoded",
                RevealedFlag = "flag_sig_rotation_location_revealed",
                ResolvedFlag = "flag_sig_rotation_resolved"
            }
        };

        private readonly Dictionary<string, CipherQuestState> _states =
            new Dictionary<string, CipherQuestState>(StringComparer.OrdinalIgnoreCase);

        public event Action<string, string>? OnLocationRevealedByCipher;
        public event Action<string>? OnCipherDecoded;

        public CipherQuestChainEngine()
        {
            foreach (var def in Chains)
            {
                _states[def.ChainId] = new CipherQuestState { chainId = def.ChainId };
            }
        }

        public CipherQuestState GetState(string chainId)
        {
            if (!_states.TryGetValue(chainId, out var state))
            {
                state = new CipherQuestState { chainId = chainId };
                _states[chainId] = state;
            }
            return state;
        }

        public void RecordBroadcastHeard(string broadcastOrStationId, WastelandMapSystem? map = null)
        {
            foreach (var def in Chains)
            {
                if (string.Equals(def.BroadcastId, broadcastOrStationId, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(def.CipherStationId, broadcastOrStationId, StringComparison.OrdinalIgnoreCase))
                {
                    var s = GetState(def.ChainId);
                    s.isHeard = true;
                    EvaluateDecode(def, map);
                }
            }
        }

        public void RecordKeyAcquired(string itemId, WastelandMapSystem? map = null)
        {
            foreach (var def in Chains)
            {
                if (string.Equals(def.RequiredItemId, itemId, StringComparison.OrdinalIgnoreCase))
                {
                    var s = GetState(def.ChainId);
                    s.isKeyFound = true;
                    EvaluateDecode(def, map);
                }
            }
        }

        public bool EvaluateDecode(CipherChainDefinition def, WastelandMapSystem? map = null)
        {
            var s = GetState(def.ChainId);
            if (s.isHeard && s.isKeyFound && !s.isDecoded)
            {
                s.isDecoded = true;
                OnCipherDecoded?.Invoke(def.ChainId);

                if (!s.isLocationRevealed)
                {
                    s.isLocationRevealed = true;
                    map?.Discover(def.TargetLocationId);
                    OnLocationRevealedByCipher?.Invoke(def.ChainId, def.TargetLocationId);
                }
                return true;
            }
            return false;
        }

        public void MarkResolved(string chainId)
        {
            var s = GetState(chainId);
            s.isResolved = true;
        }

        public List<CipherQuestState> CaptureState()
        {
            return new List<CipherQuestState>(_states.Values);
        }

        public void RestoreState(List<CipherQuestState>? savedStates, WastelandMapSystem? map = null)
        {
            if (savedStates == null) return;
            foreach (var saved in savedStates)
            {
                if (saved == null || string.IsNullOrEmpty(saved.chainId)) continue;
                _states[saved.chainId] = saved;
                if (saved.isLocationRevealed)
                {
                    var def = Chains.Find(c => string.Equals(c.ChainId, saved.chainId, StringComparison.OrdinalIgnoreCase));
                    if (def != null && map != null)
                    {
                        map.Discover(def.TargetLocationId);
                    }
                }
            }
        }
    }
}
