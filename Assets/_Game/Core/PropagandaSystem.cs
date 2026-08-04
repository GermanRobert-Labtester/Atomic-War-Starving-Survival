using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Propaganda Broadcasting (Prompt #74). Using the RadioTransmitter (high
    /// power cost), the player can broadcast propaganda to manipulate Faction
    /// Trust. A survivor with high Morale can redirect hostile factions into
    /// fighting each other instead of raiding the bunker.
    /// Save/load safe. Plain C#.
    /// </summary>
    public class PropagandaSystem
    {
        public const string RadioTransmitterItemId = "radio_transmitter";

        /// <summary>Power consumed per broadcast (watts × hours).</summary>
        public const float BroadcastPowerCost = 40f;

        /// <summary>Minimum broadcaster Morale to attempt propaganda.</summary>
        public const float RequiredMorale = 60f;

        /// <summary>Trust shift applied to target faction per broadcast.</summary>
        public const float TrustShiftPerBroadcast = 12f;

        /// <summary>Trust penalty applied to the target's rival faction.</summary>
        public const float RivalTrustPenalty = 15f;

        /// <summary>Raid chance reduction per successful broadcast.</summary>
        public const float RaidChanceReduction = 0.3f;

        /// <summary>Cooldown in hours between broadcasts.</summary>
        public const float BroadcastCooldownHours = 48f;

        /// <summary>Hours until a broadcast's effects fade.</summary>
        public const float BroadcastEffectDurationHours = 96f;

        /// <summary>Active propaganda broadcasts.</summary>
        public class PropagandaBroadcast
        {
            public string TargetFactionId;
            public string RivalFactionId;
            public float RemainingEffectHours;
            public float TrustApplied;
            public string BroadcasterId;
        }

        private readonly List<PropagandaBroadcast> _activeBroadcasts = new List<PropagandaBroadcast>();
        private float _cooldownRemaining;

        // -- Events --
        public event Action<PropagandaBroadcast> OnBroadcastSent;
        public event Action<PropagandaBroadcast> OnBroadcastExpired;
        public event Action OnStateChanged;

        public IReadOnlyList<PropagandaBroadcast> ActiveBroadcasts => _activeBroadcasts;
        public bool IsOnCooldown => _cooldownRemaining > 0f;

        public PropagandaSystem() { }

        /// <summary>
        /// Send a propaganda broadcast. Requires RadioTransmitter, power,
        /// and a high-morale survivor.
        /// Returns the broadcast if successful, null otherwise.
        /// </summary>
        public PropagandaBroadcast SendBroadcast(
            string targetFactionId,
            string rivalFactionId,
            string broadcasterId,
            float broadcasterMorale,
            Func<string, bool> hasItem,
            Func<float, bool> consumePower)
        {
            if (string.IsNullOrEmpty(targetFactionId)) return null;
            if (IsOnCooldown) return null;
            if (broadcasterMorale < RequiredMorale) return null;

            // Requires RadioTransmitter.
            if (hasItem != null && !hasItem(RadioTransmitterItemId)) return null;

            // Consume power.
            if (consumePower != null && !consumePower(BroadcastPowerCost)) return null;

            var broadcast = new PropagandaBroadcast
            {
                TargetFactionId = targetFactionId,
                RivalFactionId = rivalFactionId ?? string.Empty,
                RemainingEffectHours = BroadcastEffectDurationHours,
                TrustApplied = TrustShiftPerBroadcast,
                BroadcasterId = broadcasterId ?? string.Empty
            };
            _activeBroadcasts.Add(broadcast);
            _cooldownRemaining = BroadcastCooldownHours;

            OnBroadcastSent?.Invoke(broadcast);
            OnStateChanged?.Invoke();
            return broadcast;
        }

        /// <summary>
        /// Tick broadcast effects. Fade after duration expires.
        /// </summary>
        public void Tick(float gameHours,
            Action<string, float> modifyFactionTrust = null,
            Action<string, float> reduceRaidChance = null)
        {
            // Cooldown.
            if (_cooldownRemaining > 0f)
                _cooldownRemaining = Mathf.Max(0f, _cooldownRemaining - gameHours);

            for (int i = _activeBroadcasts.Count - 1; i >= 0; i--)
            {
                var b = _activeBroadcasts[i];
                b.RemainingEffectHours -= gameHours;

                if (b.RemainingEffectHours <= 0f)
                {
                    // Reverse the trust bonus (fades).
                    modifyFactionTrust?.Invoke(b.TargetFactionId, -b.TrustApplied);
                    if (!string.IsNullOrEmpty(b.RivalFactionId))
                        modifyFactionTrust?.Invoke(b.RivalFactionId, RivalTrustPenalty); // Reversal: restore

                    OnBroadcastExpired?.Invoke(b);
                    _activeBroadcasts.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Get current raid chance reduction from active propaganda against a faction.
        /// </summary>
        public float GetRaidChanceReduction(string factionId)
        {
            if (string.IsNullOrEmpty(factionId)) return 0f;
            float total = 0f;
            for (int i = 0; i < _activeBroadcasts.Count; i++)
            {
                if (_activeBroadcasts[i].TargetFactionId == factionId)
                    total += RaidChanceReduction;
            }
            return Mathf.Min(total, 0.9f);
        }

        /// <summary>
        /// Whether the RadioTransmitter is available for broadcasting.
        /// </summary>
        public static bool HasTransmitter(Func<string, bool> hasItem)
        {
            return hasItem != null && hasItem(RadioTransmitterItemId);
        }

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------

        public PropagandaSave CaptureState()
        {
            var broadcasts = new PropagandaEntrySave[_activeBroadcasts.Count];
            for (int i = 0; i < _activeBroadcasts.Count; i++)
            {
                var b = _activeBroadcasts[i];
                broadcasts[i] = new PropagandaEntrySave
                {
                    TargetFactionId = b.TargetFactionId,
                    RivalFactionId = b.RivalFactionId,
                    RemainingEffectHours = b.RemainingEffectHours,
                    TrustApplied = b.TrustApplied,
                    BroadcasterId = b.BroadcasterId
                };
            }
            return new PropagandaSave
            {
                Broadcasts = broadcasts,
                CooldownRemaining = _cooldownRemaining
            };
        }

        public void RestoreState(PropagandaSave save)
        {
            _activeBroadcasts.Clear();
            _cooldownRemaining = 0f;
            if (save == null) return;
            _cooldownRemaining = save.CooldownRemaining;
            if (save.Broadcasts != null)
            {
                for (int i = 0; i < save.Broadcasts.Length; i++)
                {
                    var b = save.Broadcasts[i];
                    if (b == null) continue;
                    _activeBroadcasts.Add(new PropagandaBroadcast
                    {
                        TargetFactionId = b.TargetFactionId,
                        RivalFactionId = b.RivalFactionId,
                        RemainingEffectHours = b.RemainingEffectHours,
                        TrustApplied = b.TrustApplied,
                        BroadcasterId = b.BroadcasterId
                    });
                }
            }
        }
    }

    [Serializable]
    public class PropagandaSave
    {
        public PropagandaEntrySave[] Broadcasts;
        public float CooldownRemaining;
    }

    [Serializable]
    public class PropagandaEntrySave
    {
        public string TargetFactionId;
        public string RivalFactionId;
        public float RemainingEffectHours;
        public float TrustApplied;
        public string BroadcasterId;
    }
}
