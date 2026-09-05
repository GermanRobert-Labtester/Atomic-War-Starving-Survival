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
        public const string Generator = "Generator";
        public const string Ventilation = "Ventilation";
        public const string Radio = "Radio";
        public const string Medical = "Medical";
        public const string Surface = "Surface";
        public const string Machinery = "Machinery";
        public const string ShelterSocial = "ShelterSocial";
        public const string Subterranean = "Subterranean";
    }

    /// <summary>
    /// Defines a single audio cue: stable snake_case ID → resource, bus, behavior.
    /// </summary>
    public sealed class AudioCueDef
    {
        public string Id { get; }
        public System.Collections.Generic.IReadOnlyList<string> ResourcePaths { get; }
        public string ResourcePath => ResourcePaths.Count > 0 ? ResourcePaths[0] : "";
        public string Bus { get; }
        public bool Loop { get; }
        public float DefaultVolumeDb { get; }
        public float VolumeJitterDb { get; }
        public float PitchMin { get; }
        public float PitchMax { get; }
        public int MaxInstances { get; }
        public int Priority { get; }
        public float CooldownSeconds { get; }
        public float FadeInSeconds { get; }
        public float FadeOutSeconds { get; }
        public string? FallbackCueId { get; }

        public AudioCueDef(string id, string resourcePath, string bus,
            bool loop = false, float defaultVolumeDb = 0f,
            float cooldownSeconds = 0f, string? fallbackCueId = null,
            System.Collections.Generic.IEnumerable<string>? resourcePaths = null,
            float volumeJitterDb = 0f, float pitchMin = 1f, float pitchMax = 1f,
            int maxInstances = 4, int priority = 50, float fadeInSeconds = 0f, float fadeOutSeconds = 0f)
        {
            Id = id;
            if (resourcePaths != null)
            {
                var list = new System.Collections.Generic.List<string>(resourcePaths);
                if (list.Count == 0 && !string.IsNullOrEmpty(resourcePath)) list.Add(resourcePath);
                ResourcePaths = list;
            }
            else
            {
                ResourcePaths = string.IsNullOrEmpty(resourcePath) ? System.Array.Empty<string>() : new[] { resourcePath };
            }
            Bus = bus;
            Loop = loop;
            DefaultVolumeDb = defaultVolumeDb;
            VolumeJitterDb = volumeJitterDb;
            PitchMin = pitchMin;
            PitchMax = pitchMax;
            MaxInstances = maxInstances;
            Priority = priority;
            CooldownSeconds = cooldownSeconds;
            FadeInSeconds = fadeInSeconds;
            FadeOutSeconds = fadeOutSeconds;
            FallbackCueId = fallbackCueId;
        }
    }

    /// <summary>
    /// Data-driven catalog of all audio cues.
    /// Stable snake_case IDs. Missing resources fail safely.
    /// </summary>

    public sealed class AudioCueCatalogDto
    {
        [System.Text.Json.Serialization.JsonPropertyName("schema_version")]
        public int SchemaVersion { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("cues")]
        public System.Collections.Generic.List<AudioCueDto> Cues { get; set; } = new();
    }

    public sealed class AudioCueDto
    {
        [System.Text.Json.Serialization.JsonPropertyName("id")]
        public string Id { get; set; } = "";

        [System.Text.Json.Serialization.JsonPropertyName("resource_paths")]
        public System.Collections.Generic.List<string>? ResourcePaths { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("resource_path")]
        public string ResourcePath { get; set; } = "";

        [System.Text.Json.Serialization.JsonPropertyName("bus")]
        public string Bus { get; set; } = AudioBusNames.Sfx;

        [System.Text.Json.Serialization.JsonPropertyName("loop")]
        public bool Loop { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("default_volume_db")]
        public float DefaultVolumeDb { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("volume_jitter_db")]
        public float VolumeJitterDb { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("pitch_min")]
        public float PitchMin { get; set; } = 1f;

        [System.Text.Json.Serialization.JsonPropertyName("pitch_max")]
        public float PitchMax { get; set; } = 1f;

        [System.Text.Json.Serialization.JsonPropertyName("max_instances")]
        public int MaxInstances { get; set; } = 4;

        [System.Text.Json.Serialization.JsonPropertyName("priority")]
        public int Priority { get; set; } = 50;

        [System.Text.Json.Serialization.JsonPropertyName("cooldown_seconds")]
        public float CooldownSeconds { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("fade_in_seconds")]
        public float FadeInSeconds { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("fade_out_seconds")]
        public float FadeOutSeconds { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("fallback_cue_id")]
        public string? FallbackCueId { get; set; }
    }

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
        public const string UiSwitchToggle = "ui_switch_toggle";
        public const string UiRotaryClick = "ui_rotary_click";
        public const string UiCrtPowerOn = "ui_crt_power_on";
        public const string UiPaperRustle = "ui_paper_rustle";
        public const string UiStampHeavy = "ui_stamp_heavy";
        public const string UiDrawerSlide = "ui_drawer_slide";

        // Radiation / Hazards
        public const string RadAlertAcute = "rad_alert_acute";
        public const string RadAlertChronic = "rad_alert_chronic";
        public const string RadGeigerBurst = "rad_geiger_burst";
        public const string RadGeigerLoop = "rad_geiger_loop";
        public const string RadGeigerIntense = "rad_geiger_intense";
        public const string RadContamination = "rad_contamination";

        // Weather
        public const string WeatherAlert = "weather_alert";
        public const string WeatherFalloutStorm = "weather_fallout_storm";
        public const string WeatherBlackRain = "weather_black_rain";
        public const string WeatherBlizzard = "weather_blizzard";
        public const string WeatherWindGust = "weather_wind_gust";
        public const string WeatherEmpStorm = "weather_emp_storm";
        public const string WeatherGlassStorm = "weather_glass_storm";
        public const string WeatherCorrosivePrecipitation = "weather_corrosive_precipitation";

        // Ambience
        public const string AmbBunker = "amb_bunker";
        public const string AmbBunkerLowPower = "amb_bunker_low_power_loop";
        public const string AmbSurface = "amb_surface";
        public const string AmbSurfaceAshfall = "amb_surface_ashfall_loop";
        public const string AmbSurfaceBlizzard = "amb_surface_blizzard_loop";
        public const string AmbSurfaceFalloutStorm = "amb_surface_fallout_storm_loop";
        public const string AmbSurfaceStorm = "amb_surface_storm";
        public const string AmbLocAbandonedHospital = "amb_loc_abandoned_hospital";
        public const string AmbLocRuralGasStation = "amb_loc_rural_gas_station";
        public const string AmbLocSuburbanRuins = "amb_loc_suburban_ruins";
        public const string AmbLocMilitaryBunker = "amb_loc_military_bunker";
        public const string AmbLocGeothermalRuins = "amb_loc_geothermal_ruins";
        public const string AmbLocArcologySector = "amb_loc_arcology_sector";
        public const string AmbLocGraniteQuarry = "amb_loc_granite_quarry";
        public const string AmbWarzoneDistantShelling = "amb_warzone_distant_shelling";

        // Distant Combat & Shelling
        public const string DistantArtilleryBarrage = "sfx_distant_artillery_barrage";
        public const string DistantGunfireSkirmish = "sfx_distant_gunfire_skirmish";
        public const string ArtilleryIncomingWhistle = "sfx_artillery_incoming_whistle";
        public const string DistantMortarLaunch = "sfx_distant_mortar_launch";

        // Variable Firearms
        public const string WeaponCz75Report = "sfx_weapon_cz75_report";
        public const string WeaponPipeRifleReport = "sfx_weapon_pipe_rifle_report";
        public const string WeaponScrapShotgunReport = "sfx_weapon_scrap_shotgun_report";
        public const string WeaponBoltRifleReport = "sfx_weapon_bolt_rifle_report";
        public const string WeaponAssaultRifleBurst = "sfx_weapon_assault_rifle_burst";
        public const string WeaponLmgBurst = "sfx_weapon_lmg_burst";
        public const string WeaponSniperHeavyReport = "sfx_weapon_sniper_heavy_report";
        public const string WeaponShotgunRack = "sfx_weapon_shotgun_rack";

        // Tactical Foley & Environmental Effects
        public const string BulletWhizRicochet = "sfx_bullet_whiz_ricochet";
        public const string StructuralCollapse = "sfx_structural_collapse";
        public const string AirlockPurgeCycle = "sfx_airlock_purge_cycle";
        public const string HeavyImpactFall = "sfx_heavy_impact_fall";

        // Music
        public const string MusicMenu = "music_menu";
        public const string MusicGameplay = "music_gameplay";

        // Radio
        public const string RadioStatic = "radio_static";
        public const string RadioTune = "radio_tune";
        public const string RadioSignalLock = "radio_signal_lock";
        public const string RadioMorse = "radio_morse";
        public const string RadioNumbersStation = "radio_numbers_station";
        public const string RadioEbsAlert = "radio_ebs_alert";
        public const string RadioDeadHandPulse = "radio_dead_hand_pulse";
        public const string RadioDistressBeacon = "radio_distress_beacon";
        public const string RadioVoCh3AshRoad = "radio_vo_ch3_ash_road";
        public const string RadioVoCh7Milband = "radio_vo_ch7_milband";
        public const string RadioVoCh11Stockpile = "radio_vo_ch11_stockpile";
        public const string RadioVoKindHatch = "radio_vo_kind_hatch";
        public const string RadioVoKindParley = "radio_vo_kind_parley";
        public const string RadioVoVerdictMeter = "radio_vo_verdict_meter";
        public const string RadioVoVerdictEden = "radio_vo_verdict_eden";
        public const string RadioVoVerdictCount = "radio_vo_verdict_count";
        public const string RadioVoVerdictGeophone = "radio_vo_verdict_geophone";
        public const string RadioVoVerdictReckoning = "radio_vo_verdict_reckoning";

        // Shelter / Resources
        public const string ShelterDoorOpen = "shelter_door_open";
        public const string ShelterDoorSeal = "shelter_door_seal";
        public const string ShelterVentilation = "shelter_ventilation";
        public const string ShelterGenerator = "shelter_generator";
        public const string ShelterGeneratorStrain = "shelter_generator_strain";
        public const string ShelterGeneratorStart = "sfx_generator_start";
        public const string ShelterGeneratorStop = "sfx_generator_stop";
        public const string ShelterBreakerTrip = "sfx_breaker_trip";
        public const string ShelterPowerRestore = "sfx_power_restore";
        public const string ShelterPipeClang = "shelter_pipe_clang";
        public const string ShelterWaterDrip = "shelter_water_drip";
        public const string ShelterWaterFiltration = "shelter_water_filtration";
        public const string ShelterAirFilter = "shelter_air_filter";
        public const string ShelterAirRecycler = "shelter_air_recycler";
        public const string ShelterWorkshopTools = "shelter_workshop_tools";

        // Actions
        public const string ActionItemPickup = "action_item_pickup";
        public const string ActionCrafting = "action_crafting";
        public const string ActionRepair = "action_repair";
        public const string ActionTrade = "action_trade";
        public const string ActionWaterPour = "action_water_pour";
        public const string ActionPillBottle = "action_pill_bottle";
        public const string ActionInjection = "action_injection";
        public const string ItemHandlingAmmo = "item_handling_ammo";
        public const string ItemHandlingMeds = "item_handling_meds";
        public const string ItemHandlingRation = "item_handling_ration";

        // Material-Specific Footsteps & Surface Foley
        public const string FootstepGranite = "footstep_granite";
        public const string FootstepMetal = "footstep_metal";
        public const string FootstepDirt = "footstep_dirt";
        public const string FootstepGlass = "footstep_glass";
        public const string FootstepWood = "footstep_wood";

        // Medical
        public const string MedHeartbeat = "med_heartbeat";
        public const string MedCoughing = "med_coughing";
        public const string MedSurvivorDeath = "med_survivor_death";
        public const string MedQuarantineSeal = "med_quarantine_seal";
        public const string MedQuarantineClear = "med_quarantine_clear";
        public const string MedInfirmaryBeep = "med_infirmary_beep";

        // Expeditions & Vehicles
        public const string ExpeditionVehicleEngine = "expedition_vehicle_engine";
        public const string ExpeditionVehicleDirtBike = "expedition_vehicle_dirtbike";
        public const string ExpeditionVehicleTruck = "expedition_vehicle_truck";
        public const string ExpeditionVehicleBreakdown = "expedition_vehicle_breakdown";
        public const string ExpeditionVehicleRefuel = "expedition_vehicle_refuel";
        public const string ExpeditionVehicleRepair = "expedition_vehicle_repair";
        public const string ExpeditionCampFire = "expedition_camp_fire";

        // Psychological Trauma & Stress
        public const string TraumaTinnitus = "trauma_tinnitus";
        public const string TraumaHeartbeatRapid = "trauma_heartbeat_rapid";
        public const string TraumaCabinFever = "trauma_cabin_fever";


        // Shelter Machine Identity Tells (Plan 29 audio hooks, Task 29B.21 —
        // shelter_machine_identities.json quirks[].audio_cue; semantics in
        // docs/shelter/PLAN29_AUDIO_HOOKS.md, assets ElevenLabs-generated).
        public const string HepaIntakeWhistle = "hepa_intake_whistle";
        public const string HepaStormCough = "hepa_storm_cough";
        public const string HepaCoolingTick = "hepa_cooling_tick";
        public const string HepaRadonHum = "hepa_radon_hum";
        public const string FoundryTuyereKnock = "foundry_tuyere_knock";
        public const string FoundryExhaustWhine = "foundry_exhaust_whine";
        public const string FoundryHeatShimmer = "foundry_heat_shimmer";
        public const string FoundryVibrationTune = "foundry_vibration_tune";
        public const string GeneratorFuelCough = "generator_fuel_cough";
        public const string GeneratorRelayChatter = "generator_relay_chatter";
        public const string GeneratorBrownoutFlicker = "generator_brownout_flicker";
        public const string GeneratorVibrationTick = "generator_vibration_tick";
        public const string VentilationRattle = "ventilation_rattle";
        public const string VentilationSootSmell = "ventilation_soot_smell";
        public const string WaterFlutter = "water_flutter";
        public const string WaterDistillationHum = "water_distillation_hum";
        public const string BoilerCutoutSputter = "boiler_cutout_sputter";
        public const string BoilerJacketTick = "boiler_jacket_tick";
        public const string AirlockSealDrag = "airlock_seal_drag";
        public const string AirlockMachineryGrind = "airlock_machinery_grind";

        // Expansion (Plans 186-201)
        public const string BioMutationPulse = "bio_mutation_pulse";
        public const string InterrogationSlam = "action_interrogation_slam";
        public const string HazardToxicSizzle = "hazard_toxic_sizzle";
        public const string TrainScreechCrash = "train_screech_crash";

        // Danger
        public const string DangerExplosion = "danger_explosion";
        public const string DangerAlarmKlaxon = "danger_alarm_klaxon";
        public const string DangerGlassBreak = "danger_glass_break";
        public const string DangerDebris = "danger_debris";

        // Combat
        public const string CombatStart = "combat_start";
        public const string CombatFire = "combat_fire";
        public const string CombatJam = "combat_jam";
        public const string CombatReload = "combat_reload";
        public const string CombatHit = "combat_hit";
        public const string CombatDowned = "combat_downed";
        public const string CombatVictory = "combat_victory";
        public const string CombatDefeat = "combat_defeat";
        public const string CombatWeaponBurst = "combat_weapon_burst";
        public const string CombatDryFire = "combat_dry_fire";
        public const string CombatCasingDrop = "combat_casing_drop";
        public const string CombatLastStand = "combat_last_stand";
        public const string CombatDeconFlush = "combat_decon_flush";
        public const string CombatImpactWood = "combat_impact_wood";
        public const string CombatImpactConcrete = "combat_impact_concrete";
        public const string CombatImpactMetal = "combat_impact_metal";
        public const string CombatImprovisedSpear = "combat_improvised_spear";
        public const string CombatImprovisedFire = "combat_improvised_fire";

        // Somatic Flashbacks & Trauma
        public const string FlashbackTrigger = "flashback_trigger";
        public const string FlashbackGrounded = "flashback_grounded";

        // Diegetic Audio Logs & Echoes
        public const string LogTapeInsert = "log_tape_insert";
        public const string LogTapeButton = "log_tape_button";
        public const string LogTapeRewind = "log_tape_rewind";
        public const string LogTapeStop = "log_tape_stop";
        public const string LogTapeHiss = "log_tape_hiss";
        public const string EchoDiscovery = "echo_discovery";

        // Game flow
        public const string GameOver = "game_over";
        public const string SaveSuccess = "save_success";
        public const string DayTransition = "day_transition";

        // Shelter Operations & Workshop (Plans 46–49)
        public const string WorkshopLatheLoop = "workshop_lathe_loop";
        public const string AmmoPressStamp = "ammo_press_stamp";
        public const string WeaponCleanClick = "weapon_clean_click";
        public const string MachineOverhaulClank = "machine_overhaul_clank";

        // Radio Intelligence (Plan 47)
        public const string RadioTuningHeterodyne = "radio_tuning_heterodyne";
        public const string RadioDecryptedBeep = "radio_decrypted_beep";

        // Shelter Social Dynamics (Plan 48)
        public const string MessHallChatterLoop = "mess_hall_chatter_loop";
        public const string DisputeArgumentShout = "dispute_argument_shout";
        public const string MediationAccordChime = "mediation_accord_chime";

        // Subterranean Excavation & Hazards (Plan 49)
        public const string MethaneAlarmBeep = "methane_alarm_beep";
        public const string WaterPumpHumLoop = "water_pump_hum_loop";
        public const string BulkheadHydraulicSlam = "bulkhead_hydraulic_slam";
        public const string CaveInCollapseRumble = "cave_in_collapse_rumble";

        static AudioCueCatalog()
        {
            RegisterAll();
        }

        private const string PrimaryCatalogPath = "Assets/StreamingAssets/Data/audio_cues.json";
        private const string GodotCatalogPath = "res://assets/StreamingAssets/Data/audio_cues.json";

        public static void RegisterAll()
        {
            s_cues.Clear();
            bool loaded = LoadFromJson(PrimaryCatalogPath);
            if (!loaded)
            {
                LoadFromJson(GodotCatalogPath);
            }
        }

        public static void Reload() => RegisterAll();

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

        public static bool LoadFromJson(string jsonPath)
        {
            try
            {
                string json = "";
                string osPath = Godot.ProjectSettings.GlobalizePath(jsonPath);
                if (System.IO.File.Exists(osPath))
                {
                    json = System.IO.File.ReadAllText(osPath);
                }
                else if (System.IO.File.Exists(jsonPath))
                {
                    json = System.IO.File.ReadAllText(jsonPath);
                }
                else if (Godot.FileAccess.FileExists(jsonPath))
                {
                    using var fa = Godot.FileAccess.Open(jsonPath, Godot.FileAccess.ModeFlags.Read);
                    if (fa != null) json = fa.GetAsText();
                }

                if (string.IsNullOrWhiteSpace(json)) return false;

                var dto = System.Text.Json.JsonSerializer.Deserialize<AudioCueCatalogDto>(json);
                if (dto == null || dto.Cues == null) return false;

                foreach (var cue in dto.Cues)
                {
                    if (string.IsNullOrEmpty(cue.Id)) continue;

                    var paths = cue.ResourcePaths != null && cue.ResourcePaths.Count > 0
                        ? cue.ResourcePaths
                        : (string.IsNullOrEmpty(cue.ResourcePath) ? new List<string>() : new List<string> { cue.ResourcePath });

                    string primaryPath = !string.IsNullOrEmpty(cue.ResourcePath)
                        ? cue.ResourcePath
                        : (paths.Count > 0 ? paths[0] : "");

                    s_cues[cue.Id] = new AudioCueDef(
                        cue.Id,
                        primaryPath,
                        cue.Bus ?? AudioBusNames.Sfx,
                        cue.Loop,
                        cue.DefaultVolumeDb,
                        cue.CooldownSeconds,
                        cue.FallbackCueId,
                        paths,
                        cue.VolumeJitterDb,
                        cue.PitchMin,
                        cue.PitchMax,
                        cue.MaxInstances,
                        cue.Priority,
                        cue.FadeInSeconds,
                        cue.FadeOutSeconds
                    );
                }
                return s_cues.Count > 0;
            }
            catch (Exception ex)
            {
                Godot.GD.PrintErr($"[AudioCueCatalog] Failed to load audio cues from {jsonPath}: {ex.Message}");
                return false;
            }
        }

    }
}
