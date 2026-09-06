// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core.Audio;

namespace AtomicWar.GodotApp.Audio
{
    /// <summary>
    /// Bridges the Core ShelterAcousticDirector to the Godot AudioManager presentation layer.
    /// Thin presentation adapter with zero gameplay or simulation logic.
    /// </summary>
    public sealed class ShelterAcousticBridge : IDisposable
    {
        private readonly Action<string> _playCue;
        private readonly Action<string, string> _startLoop;
        private readonly Action<string, string, float, float> _updateLoop;
        private readonly Action<string, string> _stopLoop;

        private ShelterAcousticDirector? _director;
        private bool _disposed;

        public ShelterAcousticBridge(AudioManager audio)
            : this(
                (audio ?? throw new ArgumentNullException(nameof(audio))).PlayCue,
                audio.StartLoop,
                audio.UpdateLoop,
                audio.StopLoop)
        {
        }

        public ShelterAcousticBridge(
            Action<string> playCue,
            Action<string, string>? startLoop = null,
            Action<string, string, float, float>? updateLoop = null,
            Action<string, string>? stopLoop = null)
        {
            _playCue = playCue ?? throw new ArgumentNullException(nameof(playCue));
            _startLoop = startLoop ?? ((_, _) => { });
            _updateLoop = updateLoop ?? ((_, _, _, _) => { });
            _stopLoop = stopLoop ?? ((_, _) => { });
        }

        public void BindDirector(ShelterAcousticDirector? director)
        {
            _director = director;
        }

        public void SyncAcoustics()
        {
            if (_disposed || _director == null) return;

            var snapshot = _director.EvaluateSnapshot();

            // 1. Dispatch one-shots
            foreach (var cueId in snapshot.pendingOneShotCues)
            {
                _playCue(cueId);
            }

            // 2. Continuous loops
            foreach (var kvp in snapshot.continuousLayerIntensities)
            {
                string busId = kvp.Key;
                int permille = kvp.Value;
                float volumeDb = permille <= 0 ? -80f : -30f + (permille / 1000f) * 24f;

                _updateLoop("shelter_acoustic", busId, volumeDb, 1f);
            }
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            _director = null;
        }
    }
}
