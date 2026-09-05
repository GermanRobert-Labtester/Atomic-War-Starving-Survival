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
        private bool _hasPowerSnapshot;
        private bool _generatorRunning;
        private bool _brownout;
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

            _hasPowerSnapshot = false;
            SyncPowerState(emitTransitions: false);
            SyncVentilationLoop();
            _filterHazard = _startingLevel?.State.airHazardWarning ?? false;
            if (_filterHazard)
                _playCue(AudioCueCatalog.ShelterAirFilter);
        }

        private void OnPowerChanged(PowerGridEvent evt)
        {
            SyncPowerState(emitTransitions: true);
            if (evt == null)
                return;

            if (evt.Kind == PowerGridEventKind.Tripped)
                _playCue(AudioCueCatalog.ShelterBreakerTrip);
            else if (evt.Kind == PowerGridEventKind.BreakerToggled
                && string.Equals(evt.Detail, "open_to_closed", StringComparison.Ordinal))
                _playCue(AudioCueCatalog.ShelterPowerRestore);
        }

        private void OnTickSummary(PowerGridTickSummary summary)
        {
            SyncPowerState(emitTransitions: true);
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

        private void SyncPowerState(bool emitTransitions)
        {
            bool running = _powerGrid != null
                && _powerGrid.GenerationWatts > 0f
                && _powerGrid.FuelUnits > 0f;
            bool brownout = _powerGrid?.IsBrownout ?? false;

            if (emitTransitions && _hasPowerSnapshot)
            {
                if (running && !_generatorRunning)
                    _playCue(AudioCueCatalog.ShelterGeneratorStart);
                else if (!running && _generatorRunning)
                    _playCue(AudioCueCatalog.ShelterGeneratorStop);

                if (_brownout && !brownout)
                    _playCue(AudioCueCatalog.ShelterPowerRestore);
            }

            _generatorRunning = running;
            _brownout = brownout;
            _hasPowerSnapshot = true;

            SyncGeneratorLoop(running);
            SyncLowPowerLoop(running, brownout);
        }

        private void SyncGeneratorLoop(bool running)
        {
            if (running)
            {
                bool heavyStrain = _powerGrid!.TotalDrawWatts >= _powerGrid.GenerationWatts * 0.85f;
                if (heavyStrain)
                {
                    _playCue(AudioCueCatalog.ShelterGeneratorStrain);
                    _stopCue(AudioCueCatalog.ShelterGenerator);
                }
                else
                {
                    _playCue(AudioCueCatalog.ShelterGenerator);
                    _stopCue(AudioCueCatalog.ShelterGeneratorStrain);
                }
            }
            else
            {
                _stopCue(AudioCueCatalog.ShelterGenerator);
                _stopCue(AudioCueCatalog.ShelterGeneratorStrain);
            }
        }

        private void SyncLowPowerLoop(bool running, bool brownout)
        {
            if (_powerGrid != null && (!running || brownout))
                _playCue(AudioCueCatalog.AmbBunkerLowPower);
            else
                _stopCue(AudioCueCatalog.AmbBunkerLowPower);
        }

        private void SyncVentilationLoop()
        {
            // The Holdfast is sealed, so powered air circulation and filtration is audible
            // whenever the starting-level atmosphere system exists.
            if (_startingLevel != null)
            {
                _playCue(AudioCueCatalog.ShelterVentilation);
                _playCue(AudioCueCatalog.ShelterAirRecycler);
                _playCue(AudioCueCatalog.ShelterWaterFiltration);
            }
            else
            {
                _stopCue(AudioCueCatalog.ShelterVentilation);
                _stopCue(AudioCueCatalog.ShelterAirRecycler);
                _stopCue(AudioCueCatalog.ShelterWaterFiltration);
            }
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
            _hasPowerSnapshot = false;
            _stopCue(AudioCueCatalog.ShelterGenerator);
            _stopCue(AudioCueCatalog.ShelterGeneratorStrain);
            _stopCue(AudioCueCatalog.AmbBunkerLowPower);
            _stopCue(AudioCueCatalog.ShelterVentilation);
            _stopCue(AudioCueCatalog.ShelterAirRecycler);
            _stopCue(AudioCueCatalog.ShelterWaterFiltration);
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(ShelterAudioController));
        }
    }
}
