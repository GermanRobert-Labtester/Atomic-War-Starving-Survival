// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Narrative;

namespace Ashfall.Core.Radio
{
    [Serializable]
    public sealed class PatrolRadioHooksState
    {
        public List<string> ConsumedSignals { get; set; } = new();
        public List<string> PendingSignals { get; set; } = new();
    }

    /// <summary>
    /// Canonical bridge connecting patrol travel encounters to radio broadcasts.
    /// Manages one-shot signal emission, radio-capable faction origination rules,
    /// pending queues, and save/load persistence.
    /// </summary>
    public sealed class PatrolRadioHooks
    {
        private readonly ILog _log;
        private readonly HashSet<string> _consumedSignals = new(StringComparer.Ordinal);
        private readonly Queue<string> _pendingSignals = new();
        private bool _subscribed;
        private TravelEncounterSystem? _subscribedSystem;

        public static readonly HashSet<string> RadioCapableFactions = new(StringComparer.OrdinalIgnoreCase)
        {
            "iron_garrison",
            "military_remnants",
            "faction_central_garrison",
            "faction_railway_guild",
            "upland_militia"
        };

        public static readonly HashSet<string> NonRadioCapableFactions = new(StringComparer.OrdinalIgnoreCase)
        {
            "faction_scavengers",
            "cult_of_ash_sign",
            "cult_of_the_glow"
        };

        private static readonly Dictionary<string, string> EncounterToRadioMap = new(StringComparer.OrdinalIgnoreCase)
        {
            { "enc_patrol_garrison_checkpoint", "radio_patrol_garrison_checkpoint" },
            { "enc_patrol_garrison_checkpoint_v2", "radio_patrol_garrison_checkpoint" },
            { "enc_patrol_garrison_checkpoint_v3", "radio_patrol_garrison_checkpoint" },
            { "patrol_garrison_checkpoint", "radio_patrol_garrison_checkpoint" },

            { "enc_patrol_warlord_raid", "radio_patrol_warlord_raid" },
            { "enc_patrol_warlord_raid_v2", "radio_patrol_warlord_raid" },
            { "enc_patrol_warlord_raid_v3", "radio_patrol_warlord_raid" },
            { "patrol_warlord_raid", "radio_patrol_warlord_raid" },

            { "enc_patrol_central_garrison_border", "radio_patrol_border_closed" },
            { "enc_patrol_railway_convoy", "radio_patrol_convoy_attacked" },
            { "enc_patrol_warlord_press_gang", "radio_patrol_press_gang" }
        };

        public PatrolRadioHooks(ILog? log = null)
        {
            _log = log ?? NullLog.Instance;
        }

        public IReadOnlyCollection<string> ConsumedSignals => _consumedSignals;
        public IReadOnlyList<string> PendingSignals => _pendingSignals.ToList();
        public int PendingCount => _pendingSignals.Count;

        public static bool IsFactionRadioCapable(string factionId)
        {
            if (string.IsNullOrWhiteSpace(factionId)) return false;
            return RadioCapableFactions.Contains(factionId.Trim());
        }

        public static bool TryGetRadioSignalForEncounter(string encounterIdOrGroup, out string radioBroadcastId)
        {
            radioBroadcastId = string.Empty;
            if (string.IsNullOrWhiteSpace(encounterIdOrGroup)) return false;
            return EncounterToRadioMap.TryGetValue(encounterIdOrGroup.Trim(), out radioBroadcastId!);
        }

        public void Subscribe(TravelEncounterSystem travelSys)
        {
            if (travelSys == null) throw new ArgumentNullException(nameof(travelSys));
            if (_subscribed && _subscribedSystem != null)
            {
                Unsubscribe(_subscribedSystem);
            }

            _subscribedSystem = travelSys;
            _subscribedSystem.OnChoiceResolved += HandleChoiceResolved;
            _subscribed = true;
        }

        public void Unsubscribe(TravelEncounterSystem? travelSys = null)
        {
            var target = travelSys ?? _subscribedSystem;
            if (target != null && _subscribed)
            {
                target.OnChoiceResolved -= HandleChoiceResolved;
            }
            _subscribed = false;
            _subscribedSystem = null;
        }

        public bool QueueSignal(string broadcastId)
        {
            if (string.IsNullOrWhiteSpace(broadcastId)) return false;
            string id = broadcastId.Trim();

            if (_consumedSignals.Contains(id))
            {
                _log.Info($"[PatrolRadioHooks] Signal '{id}' already consumed; ignoring.");
                return false;
            }

            if (_pendingSignals.Contains(id))
            {
                _log.Info($"[PatrolRadioHooks] Signal '{id}' already in pending queue.");
                return false;
            }

            _pendingSignals.Enqueue(id);
            _log.Info($"[PatrolRadioHooks] Queued radio signal '{id}'.");
            return true;
        }

        private void HandleChoiceResolved(string encounterId, string choiceId)
        {
            if (TryGetRadioSignalForEncounter(encounterId, out string broadcastId))
            {
                QueueSignal(broadcastId);
            }
        }

        /// <summary>
        /// Dequeues all pending radio signals, marks them as consumed (one-shot),
        /// and returns them for broadcast playback or UI display.
        /// </summary>
        public IReadOnlyList<string> TickRadio()
        {
            var dispatched = new List<string>();
            while (_pendingSignals.Count > 0)
            {
                string sig = _pendingSignals.Dequeue();
                _consumedSignals.Add(sig);
                dispatched.Add(sig);
                _log.Info($"[PatrolRadioHooks] Dispatched and consumed radio signal '{sig}'.");
            }
            return dispatched;
        }

        public PatrolRadioHooksState CaptureState()
        {
            return new PatrolRadioHooksState
            {
                ConsumedSignals = new List<string>(_consumedSignals),
                PendingSignals = new List<string>(_pendingSignals)
            };
        }

        public void RestoreState(PatrolRadioHooksState? state)
        {
            _consumedSignals.Clear();
            _pendingSignals.Clear();

            if (state == null) return;

            if (state.ConsumedSignals != null)
            {
                foreach (var s in state.ConsumedSignals)
                {
                    if (!string.IsNullOrWhiteSpace(s))
                    {
                        _consumedSignals.Add(s.Trim());
                    }
                }
            }

            if (state.PendingSignals != null)
            {
                foreach (var s in state.PendingSignals)
                {
                    if (!string.IsNullOrWhiteSpace(s) && !_consumedSignals.Contains(s.Trim()))
                    {
                        _pendingSignals.Enqueue(s.Trim());
                    }
                }
            }
        }

        public void ResetForTest()
        {
            _consumedSignals.Clear();
            _pendingSignals.Clear();
        }
    }
}
