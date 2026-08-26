using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    public class WeatherHostSession : HostSessionBase
    {
        public WeatherSystem System { get; }
        public WeatherSondeSystem Sonde { get; }
        public string LastEvent { get; private set; } = string.Empty;

        private Action<WorldWeatherState>? _onstatechanged_handler;

        public WeatherHostSession(WeatherSystem? system = null, WeatherSondeSystem? sonde = null)
        {
            System = system ?? new WeatherSystem();
            Sonde = sonde ?? new WeatherSondeSystem(System);
            _onstatechanged_handler = state =>
            {
                LastEvent = $"[Weather] State changed: {state.currentKind}";
                RaiseStateChanged();
            };
            Sonde.OnStateChanged += _ => RaiseStateChanged();
            Sonde.OnSondeRecovered += id => { LastEvent = $"Sonde {id} recovered. Forecast updated."; RaiseStateChanged(); };
            Sonde.OnSondeFailed += reason => { LastEvent = $"Sonde failed: {reason}"; RaiseStateChanged(); };
        }

        public override void Save()
        {
            if (!IsDirty) return;
            try
            {
                if (WeatherSaveStore.TrySave(this.System.CaptureState()))
                    IsDirty = false;
            }
            catch (Exception e)
            {
                GD.PrintErr("[Weather] save failed: " + e.Message);
            }
        }

        public void RestoreSave(WorldWeatherState? state)
        {
            if (state == null) return;
            try
            {
                System.RestoreState(state);
                IsDirty = false;
            }
            catch (Exception e)
            {
                GD.PrintErr("[Weather] restore failed: " + e.Message);
            }
        }

        // ── Sonde demo actions ───────────────────────────────────────

        /// <summary>Launch a weather sonde.</summary>
        public string LaunchSondeDemo(int day, float hour = 12f)
        {
            bool ok = Sonde.Launch("sonde_" + day, day, hour, hydrogenAvailable: 1f, batteryAvailable: 1f);
            return ok ? $"Sonde launched at day {day}, hour {hour:F0}." : "Cannot launch sonde (already active or insufficient resources).";
        }

        /// <summary>Advance one sonde flight tick.</summary>
        public string TickSondeDemo()
        {
            if (!Sonde.IsLaunched) return "No active sonde.";
            bool ok = Sonde.Tick(new CoreSeededRng(Sonde.State.launchDay * 31 + Sonde.State.ticksElapsed));
            if (!ok) return "Sonde flight complete.";
            var state = Sonde.State;
            if (state.isFailed) return $"Sonde failed: {state.failureReason}";
            if (state.isRecovered) return $"Sonde recovered at {state.samples[state.samples.Count - 1].altitudeKm:F1} km. Forecast: {state.forecast.Count} days.";
            return $"Sonde tick {state.ticksElapsed}/{state.flightDurationTicks}. Altitude: {Sonde.GetCurrentAltitude():F1} km. Battery: {state.batteryLevel:P0}.";
        }

        /// <summary>Sonde status line.</summary>
        public string SondeStatusLine()
        {
            var state = Sonde.State;
            if (!state.isLaunched) return "Sonde: idle";
            if (state.isFailed) return $"Sonde: FAILED ({state.failureReason})";
            if (state.isRecovered) return $"Sonde: recovered ({state.forecast.Count} day forecast, quality {state.observationQuality:F2})";
            return $"Sonde: in flight (tick {state.ticksElapsed}/{state.flightDurationTicks}, alt {Sonde.GetCurrentAltitude():F1} km, battery {state.batteryLevel:P0})";
        }
    }
}