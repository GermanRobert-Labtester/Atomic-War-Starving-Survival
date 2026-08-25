using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core
{
    [Serializable]
    public sealed class AudioConditionState
    {
        public string systemId = AudioConditionSystem.SystemId;
        public List<ActiveAudioCondition> activeConditions = new List<ActiveAudioCondition>();
    }

    [Serializable]
    public sealed class ActiveAudioCondition
    {
        public string conditionId = string.Empty;
        public string bus = string.Empty;         // "generator", "ventilation", "radio", "medical", "surface", "ambient"
        public float intensity;                   // 0-1
        public bool isLooping;
        public string audioKey = string.Empty;
        public int startDay = -1;
        public bool isActive = true;
    }

    public sealed class AudioConditionSystem
    {
        public const string SystemId = "audio_conditions";
        private AudioConditionState _state = new AudioConditionState();
        private readonly ILog _log;

        public AudioConditionState State => _state;
        public event Action<ActiveAudioCondition> OnConditionStarted;
        public event Action<ActiveAudioCondition> OnConditionStopped;
        public event Action OnConditionsChanged;

        public AudioConditionSystem(ILog? log = null)
        {
            _log = log ?? NullLog.Instance;
        }

        public ActionResult StartCondition(string conditionId, string bus, string audioKey, float intensity = 1f, bool isLooping = true)
        {
            if (_state.activeConditions.Exists(c => c.conditionId == conditionId && c.isActive))
                return ActionResult.Blocked("already_active", "audio.already_active");

            if (!IsValidBus(bus))
                return ActionResult.Blocked("invalid_bus", "audio.invalid_bus");

            var condition = new ActiveAudioCondition
            {
                conditionId = conditionId, bus = bus, audioKey = audioKey,
                intensity = Math.Clamp(intensity, 0f, 1f), isLooping = isLooping, startDay = -1
            };
            _state.activeConditions.Add(condition);
            _log.Info($"[Audio] condition started: {conditionId} -> {bus} ({audioKey})");
            OnConditionStarted?.Invoke(condition);
            OnConditionsChanged?.Invoke();
            return ActionResult.Success("audio.condition_started");
        }

        public ActionResult StopCondition(string conditionId)
        {
            var condition = _state.activeConditions.Find(c => c.conditionId == conditionId && c.isActive);
            if (condition == null) return ActionResult.Blocked("not_active", "audio.not_active");

            condition.isActive = false;
            _log.Info($"[Audio] condition stopped: {conditionId}");
            OnConditionStopped?.Invoke(condition);
            OnConditionsChanged?.Invoke();
            return ActionResult.Success("audio.condition_stopped");
        }

        public ActionResult SetIntensity(string conditionId, float intensity)
        {
            var condition = _state.activeConditions.Find(c => c.conditionId == conditionId && c.isActive);
            if (condition == null) return ActionResult.Blocked("not_active", "audio.not_active");

            condition.intensity = Math.Clamp(intensity, 0f, 1f);
            OnConditionsChanged?.Invoke();
            return ActionResult.Success("audio.intensity_set",
                new Dictionary<string, double> { { "intensity", condition.intensity } });
        }

        public List<ActiveAudioCondition> GetActiveConditionsForBus(string bus)
        {
            return _state.activeConditions.FindAll(c => c.isActive && c.bus == bus);
        }

        public void ClearStopped()
        {
            _state.activeConditions.RemoveAll(c => !c.isActive);
        }

        public void TickDay(int day)
        {
            // No daily logic — conditions are event-driven
        }

        private static bool IsValidBus(string bus)
        {
            return bus switch
            {
                "generator" or "ventilation" or "radio" or "medical" or "surface" or "ambient" or "music" or "sfx" or "ui" or "alerts" or "voice"
                    => true,
                _ => false
            };
        }

        public AudioConditionState CaptureState() => CloneState(_state);

        public void RestoreState(AudioConditionState saved)
        {
            if (saved == null) return;
            _state = CloneState(saved);
        }

        private static AudioConditionState CloneState(AudioConditionState src)
        {
            if (src == null) return new AudioConditionState();
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(src);
            return s.Deserialize<AudioConditionState>(json) ?? new AudioConditionState();
        }
    }
}
