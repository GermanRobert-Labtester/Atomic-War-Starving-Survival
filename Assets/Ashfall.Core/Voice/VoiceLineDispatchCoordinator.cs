// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Voice
{
    public enum VoiceLinePriority
    {
        AmbientIdle = 0,
        TaskWork = 1,
        NeedsWarning = 2,
        CombatReaction = 3,
        CrisisBark = 4
    }

    public sealed class VoicePlaybackPayload
    {
        public string SpeakerSurvivorId { get; }
        public string LineId { get; }
        public string AudioCueKey { get; }
        public string TextKey { get; }
        public VoiceLinePriority Priority { get; }
        public int DispatchTick { get; }
        public int DurationTicks { get; }
        public bool PreemptedPrevious { get; }

        public VoicePlaybackPayload(
            string speakerSurvivorId,
            string lineId,
            string audioCueKey,
            string textKey,
            VoiceLinePriority priority,
            int dispatchTick,
            int durationTicks,
            bool preemptedPrevious)
        {
            SpeakerSurvivorId = speakerSurvivorId ?? string.Empty;
            LineId = lineId ?? string.Empty;
            AudioCueKey = audioCueKey ?? string.Empty;
            TextKey = textKey ?? string.Empty;
            Priority = priority;
            DispatchTick = dispatchTick;
            DurationTicks = Math.Max(1, durationTicks);
            PreemptedPrevious = preemptedPrevious;
        }
    }

    public sealed class VoiceDispatchResult
    {
        public bool Dispatched { get; }
        public VoicePlaybackPayload? Payload { get; }
        public string RejectionReason { get; }

        private VoiceDispatchResult(bool dispatched, VoicePlaybackPayload? payload, string rejectionReason)
        {
            Dispatched = dispatched;
            Payload = payload;
            RejectionReason = rejectionReason ?? string.Empty;
        }

        public static VoiceDispatchResult Success(VoicePlaybackPayload payload) =>
            new VoiceDispatchResult(true, payload, string.Empty);

        public static VoiceDispatchResult Rejected(string reason) =>
            new VoiceDispatchResult(false, null, reason);
    }

    [Serializable]
    public sealed class VoiceDispatchCoordinatorSaveState
    {
        public int schema_version { get; set; } = 1;
        public Dictionary<string, int> SurvivorLastSpokenTick { get; set; } = new();
        public int GlobalLastSpokenTick { get; set; } = -1000;
        public string? ActiveSpeakerId { get; set; }
        public string? ActiveLineId { get; set; }
        public VoiceLinePriority ActivePriority { get; set; }
        public int ActiveLineEndTick { get; set; } = -1;
    }

    /// <summary>
    /// D22-B / UNBLOCK-03 §4.4 / §14.3: Survivor Voice Line Audio Bridge & Cooldown Dispatcher.
    /// Bridges VoiceLineSelectionEngine to audio cue playback queues.
    /// Coordinates per-character dialogue cooldowns, global spacing, priority preemption
    /// (e.g. crisis bark interrupts ambient idle monologue), and subtitle payload generation.
    /// </summary>
    public sealed class VoiceLineDispatchCoordinator
    {
        public const int DefaultPerCharacterCooldownTicks = 30;
        public const int DefaultGlobalCooldownTicks = 5;

        private readonly Dictionary<string, int> _survivorLastSpokenTick = new(StringComparer.OrdinalIgnoreCase);
        private int _globalLastSpokenTick = -1000;

        private VoicePlaybackPayload? _activePlayback;
        private int _activeLineEndTick = -1;

        public int PerCharacterCooldownTicks { get; set; } = DefaultPerCharacterCooldownTicks;
        public int GlobalCooldownTicks { get; set; } = DefaultGlobalCooldownTicks;

        public VoicePlaybackPayload? ActivePlayback => _activePlayback;
        public bool IsPlaying(int currentTick) => _activePlayback != null && currentTick < _activeLineEndTick;

        public event Action<VoicePlaybackPayload>? OnVoiceDispatched;
        public event Action<VoicePlaybackPayload, string>? OnVoicePreempted;

        public VoiceDispatchResult TryDispatch(
            VoiceLineSelectionResult selection,
            VoiceLinePriority priority,
            int currentTick,
            int durationTicks = 5)
        {
            if (!selection.HasLine)
            {
                return VoiceDispatchResult.Rejected("NoSelectedLine");
            }

            string speakerId = selection.SpeakerSurvivorId;
            if (string.IsNullOrWhiteSpace(speakerId))
            {
                return VoiceDispatchResult.Rejected("InvalidSpeakerId");
            }

            bool isCrisis = priority == VoiceLinePriority.CrisisBark;

            // Check active line preemption
            bool preempted = false;
            if (IsPlaying(currentTick))
            {
                if (priority > _activePlayback!.Priority)
                {
                    // Preempt currently playing line
                    var oldPayload = _activePlayback;
                    _activePlayback = null;
                    _activeLineEndTick = -1;
                    preempted = true;
                    OnVoicePreempted?.Invoke(oldPayload, $"PreemptedBy_{priority}");
                }
                else
                {
                    return VoiceDispatchResult.Rejected("ActiveLineHigherOrEqualPriority");
                }
            }

            // Cooldown checks (Crisis barks bypass regular cooldowns)
            if (!isCrisis)
            {
                if (_survivorLastSpokenTick.TryGetValue(speakerId, out int lastTick))
                {
                    if (currentTick - lastTick < PerCharacterCooldownTicks)
                    {
                        return VoiceDispatchResult.Rejected("PerCharacterCooldownActive");
                    }
                }

                if (currentTick - _globalLastSpokenTick < GlobalCooldownTicks)
                {
                    return VoiceDispatchResult.Rejected("GlobalCooldownActive");
                }
            }

            var payload = new VoicePlaybackPayload(
                speakerId,
                selection.LineId,
                selection.AudioCueKey,
                selection.TextKey,
                priority,
                currentTick,
                durationTicks,
                preempted);

            _activePlayback = payload;
            _activeLineEndTick = currentTick + Math.Max(1, durationTicks);
            _survivorLastSpokenTick[speakerId] = currentTick;
            _globalLastSpokenTick = currentTick;

            OnVoiceDispatched?.Invoke(payload);
            return VoiceDispatchResult.Success(payload);
        }

        public VoiceDispatchResult TrySelectAndDispatch(
            SurvivorVoiceContext survivor,
            string eventKind,
            IEnumerable<VoiceLineCandidate> candidates,
            VoiceLinePriority priority,
            int currentTick,
            int durationTicks = 5,
            int deterministicSeed = 0)
        {
            var selection = VoiceLineSelectionEngine.SelectLine(survivor, eventKind, candidates, deterministicSeed);
            return TryDispatch(selection, priority, currentTick, durationTicks);
        }

        public void StopActivePlayback(int currentTick)
        {
            if (_activePlayback != null)
            {
                var prev = _activePlayback;
                _activePlayback = null;
                _activeLineEndTick = -1;
                OnVoicePreempted?.Invoke(prev, "ManuallyStopped");
            }
        }

        public VoiceDispatchCoordinatorSaveState CaptureState()
        {
            return new VoiceDispatchCoordinatorSaveState
            {
                schema_version = 1,
                SurvivorLastSpokenTick = new Dictionary<string, int>(_survivorLastSpokenTick, StringComparer.OrdinalIgnoreCase),
                GlobalLastSpokenTick = _globalLastSpokenTick,
                ActiveSpeakerId = _activePlayback?.SpeakerSurvivorId,
                ActiveLineId = _activePlayback?.LineId,
                ActivePriority = _activePlayback?.Priority ?? VoiceLinePriority.AmbientIdle,
                ActiveLineEndTick = _activeLineEndTick
            };
        }

        public void RestoreState(VoiceDispatchCoordinatorSaveState? state)
        {
            _survivorLastSpokenTick.Clear();
            _activePlayback = null;
            _activeLineEndTick = -1;
            _globalLastSpokenTick = -1000;

            if (state == null) return;

            if (state.SurvivorLastSpokenTick != null)
            {
                foreach (var kvp in state.SurvivorLastSpokenTick)
                {
                    _survivorLastSpokenTick[kvp.Key] = kvp.Value;
                }
            }

            _globalLastSpokenTick = state.GlobalLastSpokenTick;
            _activeLineEndTick = state.ActiveLineEndTick;

            if (!string.IsNullOrEmpty(state.ActiveSpeakerId) && !string.IsNullOrEmpty(state.ActiveLineId))
            {
                _activePlayback = new VoicePlaybackPayload(
                    state.ActiveSpeakerId,
                    state.ActiveLineId,
                    string.Empty,
                    string.Empty,
                    state.ActivePriority,
                    state.ActiveLineEndTick - 5,
                    5,
                    false);
            }
        }
    }
}
