using System;
using Ashfall.Core.Shelter;
using Ashfall.Core.StartingLevel;

namespace AtomicWar.GodotApp.Audio
{
    /// <summary>
    /// Presentation-only lifecycle for continuous shelter sound. It follows
    /// Core state but never changes it: ventilation and generator loops reflect
    /// live infrastructure, while a newly hazardous filter state raises one alert.
    /// </summary>
    public sealed class ShelterAudioController : IDisposable
    {
        private readonly Action<string> _playCue;
        private readonly Action<string> _stopCue;
        private PowerGridSystem? _powerGrid;
        private StartingLevelSystem? _startingLevel;
        private bool _filterHazard;
        private bool _disposed;

        public ShelterAudioController(AudioManager audio)
            : this(
                (audio ?? throw new ArgumentNullException(nameof(audio))).PlayCue,
                audio.StopCue)
        {
        }

        internal ShelterAudioController(Action<string> playCue, Action<string> stopCue)
        {
            _playCue = playCue ?? throw new ArgumentNullException(nameof(playCue));
            _stopCue = stopCue ?? throw new ArgumentNullException(nameof(stopCue));
        }

        /// <summary>
        /// Safe to call every frame. Old Core sessions are detached before a
        /// replacement binds, preventing stale saves from controlling audio.
        /// </summary>
        public void Subscribe(PowerGridSystem? powerGrid, StartingLevelSystem? startingLevel)
        {
            ThrowIfDisposed();
            if (ReferenceEquals(_powerGrid, powerGrid) && ReferenceEquals(_startingLevel, startingLevel))
                return;

            if (_powerGrid != null)
            {
                _powerGrid.OnPowerChanged -= OnPowerChanged;
                _powerGrid.OnTickSummary -= OnTickSummary;
            }
            if (_startingLevel != null)
                _startingLevel.OnStateChanged -= OnStartingLevelChanged;

            _powerGrid = powerGrid;
            _startingLevel = startingLevel;
            if (_powerGrid != null)
            {
                _powerGrid.OnPowerChanged += OnPowerChanged;
                _powerGrid.OnTickSummary += OnTickSummary;
            }
            if (_startingLevel != null)
                _startingLevel.OnStateChanged += OnStartingLevelChanged;

            SyncGeneratorLoop();
            SyncVentilationLoop();
            _filterHazard = _startingLevel?.State.airHazardWarning ?? false;
            if (_filterHazard)
                _playCue(AudioCueCatalog.ShelterAirFilter);
        }

        private void OnPowerChanged(PowerGridEvent evt)
        {
            SyncGeneratorLoop();
            if (evt != null && evt.Kind == PowerGridEventKind.Tripped)
                _playCue(AudioCueCatalog.DangerAlarmKlaxon);
        }

        private void OnTickSummary(PowerGridTickSummary summary)
        {
            SyncGeneratorLoop();
            if (summary != null && summary.IsBrownout)
                _playCue(AudioCueCatalog.DangerAlarmKlaxon);
        }

        private void OnStartingLevelChanged()
        {
            bool hazardous = _startingLevel?.State.airHazardWarning ?? false;
            if (hazardous && !_filterHazard)
                _playCue(AudioCueCatalog.ShelterAirFilter);
            _filterHazard = hazardous;
            SyncVentilationLoop();
        }

        private void SyncGeneratorLoop()
        {
            bool running = _powerGrid != null
                && _powerGrid.GenerationWatts > 0f
                && _powerGrid.FuelUnits > 0f;
            if (running)
                _playCue(AudioCueCatalog.ShelterGenerator);
            else
                _stopCue(AudioCueCatalog.ShelterGenerator);
        }

        private void SyncVentilationLoop()
        {
            // The Holdfast is sealed, so powered air circulation is audible
            // whenever the starting-level atmosphere system exists.
            if (_startingLevel != null)
                _playCue(AudioCueCatalog.ShelterVentilation);
            else
                _stopCue(AudioCueCatalog.ShelterVentilation);
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            if (_powerGrid != null)
            {
                _powerGrid.OnPowerChanged -= OnPowerChanged;
                _powerGrid.OnTickSummary -= OnTickSummary;
            }
            if (_startingLevel != null)
                _startingLevel.OnStateChanged -= OnStartingLevelChanged;
            _powerGrid = null;
            _startingLevel = null;
            _stopCue(AudioCueCatalog.ShelterGenerator);
            _stopCue(AudioCueCatalog.ShelterVentilation);
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(ShelterAudioController));
        }
    }
}
