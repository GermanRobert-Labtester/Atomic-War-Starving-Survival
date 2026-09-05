using System;
using Ashfall.Core;

namespace AtomicWar.GodotApp.Audio
{
    public sealed class AudioConditionHostBridge : IDisposable
    {
        private AudioConditionSystem? _system;
        private readonly AudioManager _audioManager;
        private bool _disposed;

        public AudioConditionHostBridge(AudioManager audioManager)
        {
            _audioManager = audioManager ?? throw new ArgumentNullException(nameof(audioManager));
        }

        public void Bind(AudioConditionSystem? system)
        {
            ThrowIfDisposed();
            if (ReferenceEquals(_system, system)) return;

            if (_system != null)
            {
                _system.OnConditionStarted -= OnConditionStarted;
                _system.OnConditionStopped -= OnConditionStopped;
                _system.OnConditionsChanged -= OnConditionsChanged;
            }

            _system = system;

            if (_system != null)
            {
                _system.OnConditionStarted += OnConditionStarted;
                _system.OnConditionStopped += OnConditionStopped;
                _system.OnConditionsChanged += OnConditionsChanged;

                // Synchronize initial state
                foreach (var condition in _system.State.activeConditions)
                {
                    if (condition.isActive)
                        OnConditionStarted(condition);
                }
            }
        }

        private void OnConditionStarted(ActiveAudioCondition condition)
        {
            _audioManager.RouteCondition(condition.audioKey, condition.bus, condition.intensity, condition.isLooping);
        }

        private void OnConditionStopped(ActiveAudioCondition condition)
        {
            _audioManager.StopCondition(condition.audioKey);
        }

        private void OnConditionsChanged()
        {
            if (_system == null) return;
            foreach (var condition in _system.State.activeConditions)
            {
                if (condition.isActive)
                {
                    _audioManager.SetLoopIntensity(condition.audioKey, condition.intensity);
                }
            }
        }

        public void Dispose()
        {
            if (_disposed) return;
            if (_system != null)
            {
                _system.OnConditionStarted -= OnConditionStarted;
                _system.OnConditionStopped -= OnConditionStopped;
                _system.OnConditionsChanged -= OnConditionsChanged;
            }
            _system = null;
            _disposed = true;
        }

        private void ThrowIfDisposed()
        {
            if (_disposed) throw new ObjectDisposedException(nameof(AudioConditionHostBridge));
        }
    }
}
