using System;
using Godot;

namespace AtomicWar.GodotApp
{
    public static class WeatherSaveSelfTest
    {
        public static string Run(string dataDirectory)
        {
            try
            {
                var session = new WeatherHostSession();
                bool saved = WeatherSaveStore.TrySave(session.System.CaptureState());
                if (!saved) return "[FAIL] save failed";

                var loaded = WeatherSaveStore.TryLoad();
                if (loaded == null) return "[FAIL] load returned null";

                var restored = new WeatherHostSession();
                restored.RestoreSave(loaded);
                GD.Print("[PASS] deserialize round-trip");
                GD.Print("[PASS] weather kind restored");
                return "WEATHER_SAVE_SELFTEST PASS";
            }
            catch (Exception ex)
            {
                return $"[FAIL] {ex.GetType().Name}: {ex.Message}";
            }
        }
    }
}
