using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Core;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// M-1: Developer diagnostics overlay. Shows real-time system metrics
    /// (FPS, memory, system counts, save status, watchdog counters, unticked
    /// systems) in the top-left corner. Toggled with F11. Disabled by default
    /// in release builds — only shows when both the inspector bool and the
    /// F11 toggle are active.
    ///
    /// Design (audit M-1): ASHFALL had no developer-facing diagnostics beyond
    /// the AI debug HUD. This overlay surfaces the system-level telemetry
    /// that the audit needed to measure: FPS, GC pressure, system count,
    /// save integrity, and the C-1 dead-system check.
    /// </summary>
    public class DiagnosticsOverlay : MonoBehaviour
    {
        [Header("Toggle")]
        [SerializeField] private bool _devOverlayEnabled;
        [SerializeField] private KeyCode _toggleKey = KeyCode.F11;

        [Header("References")]
        [SerializeField] private GameBootstrap _bootstrap;

        private bool _visible;
        private float _smoothedFps;
        private string _lastSaveSlot = "none";
        private int _frameCount;
        private float _frameTimer;

        // Style cache (lazy init in OnGUI to avoid Awake-order issues).
        private GUIStyle _boxStyle;
        private GUIStyle _labelStyle;
        private GUIStyle _warnStyle;
        private bool _stylesInitialized;

        private void Update()
        {
            if (Input.GetKeyDown(_toggleKey))
                _visible = !_visible;

            if (!_devOverlayEnabled || !_visible) return;

            // FPS smoothing
            _frameCount++;
            _frameTimer += Time.unscaledDeltaTime;
            if (_frameTimer >= 0.5f)
            {
                _smoothedFps = _frameCount / _frameTimer;
                _frameCount = 0;
                _frameTimer = 0f;
            }
        }

        private void OnGUI()
        {
            if (!_devOverlayEnabled || !_visible) return;

            InitStyles();

            float y = 10f;
            float x = 10f;
            float width = 340f;
            float lineHeight = 20f;

            GUI.Box(new Rect(x - 4f, y - 4f, width + 8f, 420f), "", _boxStyle);
            y = DrawHeader(x, y, width, lineHeight);

            if (_bootstrap == null)
            {
                GUI.Label(new Rect(x, y, width, lineHeight),
                    "Bootstrap not assigned — assign in Inspector.", _warnStyle);
                return;
            }

            y = DrawPerformance(x, y, width, lineHeight);
            y = DrawGameState(x, y, width, lineHeight);
            y = DrawSystems(x, y, width, lineHeight);
            y = DrawSave(x, y, width, lineHeight);
            y = DrawWatchdog(x, y, width, lineHeight);
            y += 8f;
            y = DrawUntickedSystems(x, y, width, lineHeight);
            y += 8f;
            y = DrawWeather(x, y, width, lineHeight);
            y = DrawSurvivors(x, y, width, lineHeight);
            DrawSaveables(x, y, width, lineHeight);
        }

        private float DrawHeader(float x, float y, float width, float lineHeight)
        {
            GUI.Label(new Rect(x, y, width, lineHeight),
                "=== ASHFALL DIAGNOSTICS (F11 to hide) ===", _labelStyle);
            return y + lineHeight + 4f;
        }

        private float DrawPerformance(float x, float y, float width, float lineHeight)
        {
            GUI.Label(new Rect(x, y, width, lineHeight),
                $"FPS: {_smoothedFps:F1}  |  GC Alloc: {GC.GetTotalMemory(false) / 1048576f:F1} MB",
                _labelStyle);
            return y + lineHeight;
        }

        private float DrawGameState(float x, float y, float width, float lineHeight)
        {
            var gs = _bootstrap.GameState;
            var ts = _bootstrap.TimeSystem;
            GUI.Label(new Rect(x, y, width, lineHeight),
                $"Day: {(gs != null ? gs.Day.ToString() : "?")}  " +
                $"Hour: {(ts != null ? ts.CurrentHourFloat.ToString("F1") : "?")}  " +
                $"Phase: {(gs != null ? gs.Phase.ToString() : "?")}  " +
                $"Scale: {(ts != null ? ts.TimeScale.ToString("F0") + "x" : "?")}",
                _labelStyle);
            return y + lineHeight;
        }

        private float DrawSystems(float x, float y, float width, float lineHeight)
        {
            var reg = _bootstrap.Registry;
            GUI.Label(new Rect(x, y, width, lineHeight),
                $"Systems — PerSubstep: {_bootstrap.PerSubstepTickCount}  " +
                $"Daily: {_bootstrap.DailyTickCount}  " +
                $"TotalReg: {(reg != null ? reg.TickedSystemCount.ToString() : "?")}",
                _labelStyle);
            return y + lineHeight;
        }

        private float DrawSave(float x, float y, float width, float lineHeight)
        {
            var saveSys = _bootstrap.SaveSystem;
            GUI.Label(new Rect(x, y, width, lineHeight),
                $"Save — Last: {_lastSaveSlot}  " +
                $"Version: {(saveSys != null ? "V" + SaveSystem.CurrentSaveVersion : "?")}  " +
                $"Saveables: {(saveSys != null ? saveSys.SaveableCount.ToString() : "?")}",
                _labelStyle);
            return y + lineHeight;
        }

        private float DrawWatchdog(float x, float y, float width, float lineHeight)
        {
            GUI.Label(new Rect(x, y, width, lineHeight),
                $"Watchdog — Drops: {_bootstrap.DropEventCount}  " +
                $"TotalHrs: {_bootstrap.TotalDroppedGameHours:F1}  " +
                $"PeakSteps: {_bootstrap.PeakSubstepsInOneFrame}",
                _labelStyle);
            return y + lineHeight;
        }

        private float DrawUntickedSystems(float x, float y, float width, float lineHeight)
        {
            var unticked = _bootstrap.GetUntickedSystemNames();
            if (unticked == null || unticked.Count == 0)
            {
                GUI.Label(new Rect(x, y, width, lineHeight),
                    "✓ All systems ticked (no C-1 warnings)", _labelStyle);
                return y + lineHeight;
            }

            GUI.Label(new Rect(x, y, width, lineHeight),
                $"⚠ C-1: {unticked.Count} UNTICKED SYSTEM(S) ⚠", _warnStyle);
            y += lineHeight;
            for (int i = 0; i < unticked.Count && i < 10; i++)
            {
                GUI.Label(new Rect(x + 12f, y, width, lineHeight),
                    $"- {unticked[i]}", _warnStyle);
                y += lineHeight;
            }
            return y;
        }

        private float DrawWeather(float x, float y, float width, float lineHeight)
        {
            var weather = _bootstrap.WeatherSystem;
            var temp = _bootstrap.TemperatureSystem;
            GUI.Label(new Rect(x, y, width, lineHeight),
                $"Weather: {(weather != null ? weather.Current.ToString() : "?")}  " +
                $"Indoor: {(temp != null ? temp.GetIndoorTemperature(_bootstrap.Shelter).ToString("F0") + "°C" : "?")}",
                _labelStyle);
            return y + lineHeight;
        }

        private float DrawSurvivors(float x, float y, float width, float lineHeight)
        {
            var survivors = _bootstrap.Survivors;
            int alive = 0, dead = 0;
            if (survivors != null)
            {
                for (int i = 0; i < survivors.Count; i++)
                {
                    var sv = survivors[i];
                    if (sv == null) continue;
                    if (sv.IsAlive) alive++; else dead++;
                }
            }
            GUI.Label(new Rect(x, y, width, lineHeight),
                $"Survivors: {alive} alive, {dead} dead",
                _labelStyle);
            return y + lineHeight;
        }

        private void DrawSaveables(float x, float y, float width, float lineHeight)
        {
            var saveSys = _bootstrap.SaveSystem;
            GUI.Label(new Rect(x, y, width, lineHeight),
                $"Saveables: {(saveSys != null ? saveSys.SaveableCount.ToString() : "?")}  " +
                $"Pool Version: V{SaveSystem.CurrentSaveVersion}",
                _labelStyle);
        }

        /// <summary>Notify the overlay that a save just completed.</summary>
        public void NotifySave(string slotId)
        {
            _lastSaveSlot = slotId;
        }

        private void InitStyles()
        {
            if (_stylesInitialized) return;
            _stylesInitialized = true;

            _boxStyle = new GUIStyle(GUI.skin.box);
            _boxStyle.normal.background = MakeTex(2, 2, new Color(0f, 0f, 0f, 0.75f));
            _boxStyle.padding = new RectOffset(8, 8, 8, 8);

            _labelStyle = new GUIStyle(GUI.skin.label);
            _labelStyle.fontSize = 13;
            _labelStyle.normal.textColor = new Color(0.7f, 0.9f, 0.7f, 1f);

            _warnStyle = new GUIStyle(GUI.skin.label);
            _warnStyle.fontSize = 13;
            _warnStyle.normal.textColor = new Color(1f, 0.6f, 0.3f, 1f);
            _warnStyle.fontStyle = FontStyle.Bold;
        }

        private static Texture2D MakeTex(int width, int height, Color color)
        {
            var pixels = new Color[width * height];
            for (int i = 0; i < pixels.Length; i++) pixels[i] = color;
            var tex = new Texture2D(width, height);
            tex.SetPixels(pixels);
            tex.Apply();
            return tex;
        }
    }
}
