using System;
using System.Collections.Generic;

namespace AtomicWar.GodotApp.Audio
{
    /// <summary>
    /// Audio bus names used by the cue catalog and playback service.
    /// </summary>
    public static class AudioBusNames
    {
        public const string Master = "Master";
        public const string Music = "Music";
        public const string Ambience = "Ambience";
        public const string Sfx = "SFX";
        public const string Ui = "UI";
        public const string Voice = "Voice";
        public const string Alerts = "Alerts";
    }

    /// <summary>
    /// Defines a single audio cue: stable snake_case ID → resource, bus, behavior.
    /// </summary>
    public sealed class AudioCueDef
    {
        public string Id { get; }
        public string ResourcePath { get; }
        public string Bus { get; }
        public bool Loop { get; }
        public float DefaultVolumeDb { get; }
        public float CooldownSeconds { get; }
        public string? FallbackCueId { get; }

        public AudioCueDef(string id, string resourcePath, string bus,
            bool loop = false, float defaultVolumeDb = 0f,
            float cooldownSeconds = 0f, string? fallbackCueId = null)
        {
            Id = id;
            ResourcePath = resourcePath;
            Bus = bus;
            Loop = loop;
            DefaultVolumeDb = defaultVolumeDb;
            CooldownSeconds = cooldownSeconds;
            FallbackCueId = fallbackCueId;
        }
    }

    /// <summary>
    /// Data-driven catalog of all audio cues.
    /// Stable snake_case IDs. Missing resources fail safely.
    /// </summary>
    public static class AudioCueCatalog
    {
        private static readonly Dictionary<string, AudioCueDef> s_cues = new();

        // ── Cue IDs (stable, snake_case) ────────────────────────

        // UI
        public const string UiClick = "ui_click";
        public const string UiConfirm = "ui_confirm";
        public const string UiWarning = "ui_warning";
        public const string UiCancel = "ui_cancel";
        public const string UiTabChange = "ui_tab_change";
        public const string UiModalOpen = "ui_modal_open";
        public const string UiModalClose = "ui_modal_close";
        public const string UiInvalidAction = "ui_invalid_action";

        // Radiation / Hazards
        public const string RadAlertAcute = "rad_alert_acute";
        public const string RadAlertChronic = "rad_alert_chronic";
        public const string RadGeigerBurst = "rad_geiger_burst";
        public const string RadGeigerLoop = "rad_geiger_loop";
        public const string RadContamination = "rad_contamination";

        // Weather
        public const string WeatherAlert = "weather_alert";
        public const string WeatherFalloutStorm = "weather_fallout_storm";
        public const string WeatherBlackRain = "weather_black_rain";
        public const string WeatherBlizzard = "weather_blizzard";
        public const string WeatherWindGust = "weather_wind_gust";

        // Ambience
        public const string AmbBunker = "amb_bunker";
        public const string AmbSurface = "amb_surface";

        // Music
        public const string MusicMenu = "music_menu";
        public const string MusicGameplay = "music_gameplay";

        // Radio
        public const string RadioStatic = "radio_static";
        public const string RadioTune = "radio_tune";
        public const string RadioSignalLock = "radio_signal_lock";
        public const string RadioMorse = "radio_morse";

        // Shelter / Resources
        public const string ShelterDoorOpen = "shelter_door_open";
        public const string ShelterDoorSeal = "shelter_door_seal";
        public const string ShelterVentilation = "shelter_ventilation";
        public const string ShelterGenerator = "shelter_generator";
        public const string ShelterPipeClang = "shelter_pipe_clang";
        public const string ShelterWaterDrip = "shelter_water_drip";
        public const string ShelterAirFilter = "shelter_air_filter";

        // Actions
        public const string ActionItemPickup = "action_item_pickup";
        public const string ActionCrafting = "action_crafting";
        public const string ActionRepair = "action_repair";
        public const string ActionTrade = "action_trade";
        public const string ActionWaterPour = "action_water_pour";
        public const string ActionPillBottle = "action_pill_bottle";
        public const string ActionInjection = "action_injection";

        // Medical
        public const string MedHeartbeat = "med_heartbeat";
        public const string MedCoughing = "med_coughing";

        // Danger
        public const string DangerExplosion = "danger_explosion";
        public const string DangerAlarmKlaxon = "danger_alarm_klaxon";
        public const string DangerGlassBreak = "danger_glass_break";
        public const string DangerDebris = "danger_debris";

        // Game flow
        public const string GameOver = "game_over";
        public const string SaveSuccess = "save_success";
        public const string DayTransition = "day_transition";

        static AudioCueCatalog()
        {
            RegisterAll();
        }

        private static void RegisterAll()
        {
            // UI
            Reg(UiClick, "res://assets/audio/ui/ui_click.wav", AudioBusNames.Ui, cooldown: 0.05f);
            Reg(UiConfirm, "res://assets/audio/ui/ui_confirm.wav", AudioBusNames.Ui, cooldown: 0.1f);
            Reg(UiWarning, "res://assets/audio/ui/ui_warning.wav", AudioBusNames.Ui, cooldown: 0.3f);
            Reg(UiCancel, "res://assets/audio/ui/ui_click.wav", AudioBusNames.Ui, cooldown: 0.05f);
            Reg(UiTabChange, "res://assets/audio/ui/ui_click.wav", AudioBusNames.Ui, cooldown: 0.05f);
            Reg(UiModalOpen, "res://assets/audio/ui/ui_confirm.wav", AudioBusNames.Ui, vol: -3f);
            Reg(UiModalClose, "res://assets/audio/ui/ui_click.wav", AudioBusNames.Ui);
            Reg(UiInvalidAction, "res://assets/audio/ui/ui_warning.wav", AudioBusNames.Ui, vol: -6f, cooldown: 0.5f);

            // Radiation
            Reg(RadAlertAcute, "res://assets/audio/sfx/radiation_alert.wav", AudioBusNames.Alerts, vol: -2f, cooldown: 5f);
            Reg(RadAlertChronic, "res://assets/audio/sfx/radiation_alert.wav", AudioBusNames.Alerts, vol: -6f, cooldown: 10f);
            Reg(RadGeigerBurst, "res://assets/audio/sfx/sfx_geiger_burst.mp3", AudioBusNames.Sfx, cooldown: 2f);
            Reg(RadGeigerLoop, "res://assets/audio/sfx/geiger.wav", AudioBusNames.Sfx, loop: true, vol: -10f);
            Reg(RadContamination, "res://assets/audio/sfx/sfx_contamination_warning.mp3", AudioBusNames.Alerts, cooldown: 5f);

            // Weather
            Reg(WeatherAlert, "res://assets/audio/sfx/weather_alert.wav", AudioBusNames.Alerts, vol: -2f, cooldown: 5f);
            Reg(WeatherFalloutStorm, "res://assets/audio/sfx/sfx_fallout_storm_approach.mp3", AudioBusNames.Sfx, cooldown: 10f);
            Reg(WeatherBlackRain, "res://assets/audio/sfx/sfx_contamination_warning.mp3", AudioBusNames.Alerts, cooldown: 10f);
            Reg(WeatherBlizzard, "res://assets/audio/sfx/sfx_wind_gust_harsh.mp3", AudioBusNames.Sfx, cooldown: 10f);
            Reg(WeatherWindGust, "res://assets/audio/sfx/sfx_wind_gust_harsh.mp3", AudioBusNames.Sfx, vol: -8f, cooldown: 3f);

            // Ambience
            Reg(AmbBunker, "res://assets/audio/ambience/bunker_ambience.wav", AudioBusNames.Ambience, loop: true, vol: -3f);
            Reg(AmbSurface, "res://assets/audio/ambience/surface_ambience.wav", AudioBusNames.Ambience, loop: true, vol: -4f);

            // Music
            Reg(MusicMenu, "res://assets/audio/music/main_menu.wav", AudioBusNames.Music, vol: -6f);
            Reg(MusicGameplay, "res://assets/audio/music/gameplay_underscore.wav", AudioBusNames.Music, vol: -8f);

            // Radio
            Reg(RadioStatic, "res://assets/audio/radio/radio_static_hiss.wav", AudioBusNames.Voice, vol: -8f, cooldown: 0.5f);
            Reg(RadioTune, "res://assets/audio/sfx/sfx_radio_tune.mp3", AudioBusNames.Voice, cooldown: 1f);
            Reg(RadioSignalLock, "res://assets/audio/sfx/sfx_radio_signal_lock.mp3", AudioBusNames.Voice, cooldown: 1f);
            Reg(RadioMorse, "res://assets/audio/sfx/sfx_morse_key.mp3", AudioBusNames.Voice, cooldown: 0.5f);

            // Shelter
            Reg(ShelterDoorOpen, "res://assets/audio/sfx/sfx_bunker_door_open.mp3", AudioBusNames.Sfx, cooldown: 2f);
            Reg(ShelterDoorSeal, "res://assets/audio/sfx/sfx_bunker_door_seal.mp3", AudioBusNames.Sfx, cooldown: 2f);
            Reg(ShelterVentilation, "res://assets/audio/sfx/sfx_ventilation_fan.mp3", AudioBusNames.Ambience, loop: true, vol: -12f);
            Reg(ShelterGenerator, "res://assets/audio/sfx/sfx_generator_cough.mp3", AudioBusNames.Ambience, vol: -10f, cooldown: 8f);
            Reg(ShelterPipeClang, "res://assets/audio/sfx/sfx_pipe_clang.mp3", AudioBusNames.Sfx, vol: -6f, cooldown: 5f);
            Reg(ShelterWaterDrip, "res://assets/audio/sfx/sfx_water_drip_cave.mp3", AudioBusNames.Ambience, loop: true, vol: -15f);
            Reg(ShelterAirFilter, "res://assets/audio/sfx/sfx_air_filter_degrade.mp3", AudioBusNames.Alerts, cooldown: 10f);

            // Actions
            Reg(ActionItemPickup, "res://assets/audio/sfx/sfx_item_pickup_metal.mp3", AudioBusNames.Sfx, vol: -4f, cooldown: 0.2f);
            Reg(ActionCrafting, "res://assets/audio/sfx/sfx_crafting_assemble.mp3", AudioBusNames.Sfx, cooldown: 1f);
            Reg(ActionRepair, "res://assets/audio/sfx/sfx_repair_wrench.mp3", AudioBusNames.Sfx, cooldown: 0.5f);
            Reg(ActionTrade, "res://assets/audio/sfx/sfx_trade_exchange.mp3", AudioBusNames.Sfx, cooldown: 0.5f);
            Reg(ActionWaterPour, "res://assets/audio/sfx/sfx_water_pour.mp3", AudioBusNames.Sfx, cooldown: 0.5f);
            Reg(ActionPillBottle, "res://assets/audio/sfx/sfx_pill_bottle.mp3", AudioBusNames.Sfx, cooldown: 0.3f);
            Reg(ActionInjection, "res://assets/audio/sfx/sfx_injection.mp3", AudioBusNames.Sfx, cooldown: 0.5f);

            // Medical
            Reg(MedHeartbeat, "res://assets/audio/sfx/sfx_heartbeat_slow.mp3", AudioBusNames.Sfx, vol: -6f, cooldown: 5f);
            Reg(MedCoughing, "res://assets/audio/sfx/sfx_coughing_fit.mp3", AudioBusNames.Sfx, vol: -4f, cooldown: 8f);

            // Danger
            Reg(DangerExplosion, "res://assets/audio/sfx/sfx_distant_explosion.mp3", AudioBusNames.Sfx, cooldown: 15f);
            Reg(DangerAlarmKlaxon, "res://assets/audio/sfx/sfx_alarm_klaxon.mp3", AudioBusNames.Alerts, cooldown: 10f);
            Reg(DangerGlassBreak, "res://assets/audio/sfx/sfx_glass_break_small.mp3", AudioBusNames.Sfx, cooldown: 1f);
            Reg(DangerDebris, "res://assets/audio/sfx/sfx_debris_impact.mp3", AudioBusNames.Sfx, cooldown: 3f);

            // Game flow
            Reg(GameOver, "res://assets/audio/music/main_menu.wav", AudioBusNames.Music, vol: -10f);
            Reg(SaveSuccess, "res://assets/audio/ui/ui_confirm.wav", AudioBusNames.Ui, vol: -10f, cooldown: 1f);
            Reg(DayTransition, "res://assets/audio/sfx/sfx_pipe_clang.mp3", AudioBusNames.Sfx, vol: -12f, cooldown: 2f);
        }

        private static void Reg(string id, string path, string bus,
            bool loop = false, float vol = 0f, float cooldown = 0f, string? fallback = null)
        {
            s_cues[id] = new AudioCueDef(id, path, bus, loop, vol, cooldown, fallback);
        }

        // ── Lookup ──────────────────────────────────────────────

        public static AudioCueDef? Resolve(string cueId)
        {
            if (string.IsNullOrEmpty(cueId)) return null;
            return s_cues.TryGetValue(cueId, out var cue) ? cue : null;
        }

        public static bool Contains(string cueId) =>
            !string.IsNullOrEmpty(cueId) && s_cues.ContainsKey(cueId);

        public static IReadOnlyDictionary<string, AudioCueDef> All => s_cues;

        public static int Count => s_cues.Count;
    }
}
