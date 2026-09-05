using Godot;
using System;
using System.Collections.Generic;
using System.IO;

namespace AtomicWar.GodotApp.Audio
{
    /// <summary>
    /// Complete audio playback service for ASHFALL.
    /// Owns bus topology, stream caching, pooled one-shot players,
    /// cooldown/dedup, music crossfade, ambience looping, and settings integration.
    /// No simulation logic — presentation only.
    /// </summary>
    public partial class AudioManager : Node
    {
        public static AudioManager? Instance { get; internal set; }

        // ── Bus topology ────────────────────────────────────────

        private static readonly string[] AllBuses = {
            AudioBusNames.Music,
            AudioBusNames.Ambience,
            AudioBusNames.Sfx,
            AudioBusNames.Ui,
            AudioBusNames.Voice,
            AudioBusNames.Alerts,
            AudioBusNames.Generator,
            AudioBusNames.Ventilation,
            AudioBusNames.Radio,
            AudioBusNames.Medical,
            AudioBusNames.Surface,
            AudioBusNames.Machinery,
            AudioBusNames.ShelterSocial,
            AudioBusNames.Subterranean,
        };

        // ── Players ─────────────────────────────────────────────

        private AudioStreamPlayer _musicPlayerA = null!;
        private AudioStreamPlayer _musicPlayerB = null!;
        private bool _musicActiveA;
        private float _musicCrossfade;
        private bool _crossfading;

        // Loops are keyed by cue ID. Generator, ventilation, Geiger, weather,
        // and ambience therefore coexist instead of replacing one shared stream.
        private readonly Dictionary<string, AudioStreamPlayer> _loopPlayers = new();
        private readonly List<string> _loopKeys = new();

        // One-shot pool: bounded to prevent unbounded allocation
        private const int MaxOneShotPlayers = 16;
        private readonly Stack<AudioStreamPlayer> _pool = new();
        private readonly List<AudioStreamPlayer> _activeOneShots = new();

        // ── Cache ───────────────────────────────────────────────

        private readonly Dictionary<string, AudioStream> _cache = new();
        private readonly HashSet<string> _loggedMissing = new();

        // ── Cooldown / dedup ────────────────────────────────────

        private readonly Dictionary<string, float> _cooldowns = new();
        private readonly List<string> _expiredCooldowns = new();
        private const float CooldownEpsilon = 0.01f;

        // ── Headless detection ──────────────────────────────────

        private bool _headless;
        private AudioEventBridge? _eventBridge;
        private ExpansionAudioBridge? _expansionEventBridge;
        private AudioConditionHostBridge? _conditionBridge;
        private AudioStateCoordinator? _stateCoordinator;
        private ShelterAudioController? _shelterAudio;
        private SurfaceAmbienceController? _surfaceAmbience;
        private IAudioDomainProvider? _domainProvider;
        private Action? _settingsChangedHandler;

        // ── Lifecycle ───────────────────────────────────────────

        public override void _Ready()
        {
            Instance = this;
            _headless = !DisplayServer.WindowCanDraw();

            SetupBuses();
            SetupPlayers();
            ApplySettings(AudioSettings.Instance);
            _settingsChangedHandler = () => ApplySettings(AudioSettings.Instance);
            AudioSettings.Instance.OnSettingsChanged += _settingsChangedHandler;

            _eventBridge = new AudioEventBridge(this);
            _expansionEventBridge = new ExpansionAudioBridge(this);
            _conditionBridge = new AudioConditionHostBridge(this);
            _stateCoordinator = new AudioStateCoordinator();
            AddChild(_stateCoordinator);
            _shelterAudio = new ShelterAudioController(this);
            _surfaceAmbience = new SurfaceAmbienceController(this);
            _domainProvider = GetParent() as IAudioDomainProvider;
            RefreshDomainBindings();

            GD.Print($"[AudioManager] Ready — {AllBuses.Length + 1} buses, " +
                     $"pool={MaxOneShotPlayers}, headless={_headless}");
        }

        private AudioEffectLowPassFilter? _surfaceOcclusionEffect;
        private bool _bunkerOccluded = true;
        public bool IsBunkerOccluded => _bunkerOccluded;

        private void SetupBuses()
        {
            foreach (string bus in AllBuses)
                EnsureBus(bus);

            SetupDspEffects();
        }

        private void SetupDspEffects()
        {
            try
            {
                // 1. Surface bus: Underground bunker wall occlusion filter (default occluded)
                int surfaceIdx = AudioServer.GetBusIndex(AudioBusNames.Surface);
                if (surfaceIdx >= 0)
                {
                    _surfaceOcclusionEffect = new AudioEffectLowPassFilter
                    {
                        CutoffHz = 450f,
                        Resonance = 1.0f
                    };
                    AudioServer.AddBusEffect(surfaceIdx, _surfaceOcclusionEffect);
                }

                // 2. Radio bus: Authentic retro-transceiver bandpass + subtle analog tube warmth
                int radioIdx = AudioServer.GetBusIndex(AudioBusNames.Radio);
                if (radioIdx >= 0)
                {
                    var bpf = new AudioEffectBandPassFilter
                    {
                        CutoffHz = 1400f,
                        Resonance = 1.4f
                    };
                    AudioServer.AddBusEffect(radioIdx, bpf);

                    var dist = new AudioEffectDistortion
                    {
                        Mode = AudioEffectDistortion.ModeEnum.Atan,
                        Drive = 0.10f
                    };
                    AudioServer.AddBusEffect(radioIdx, dist);
                }
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[AudioManager] SetupDspEffects warning: {ex.Message}");
            }
        }

        public void SetBunkerOcclusion(bool occluded)
        {
            _bunkerOccluded = occluded;
            if (_headless || _surfaceOcclusionEffect == null) return;

            float targetCutoff = occluded ? 450f : 20000f;
            var tween = CreateTween();
            tween.TweenProperty(_surfaceOcclusionEffect, "cutoff_hz", targetCutoff, 0.4);
        }

        public void TriggerConcussionDeafness(float durationSeconds = 2.5f)
        {
            if (_headless) return;
            PlayCue(AudioCueCatalog.UiWarning);

            int masterIdx = AudioServer.GetBusIndex(AudioBusNames.Master);
            if (masterIdx >= 0)
            {
                float currentVol = AudioServer.GetBusVolumeDb(masterIdx);
                AudioServer.SetBusVolumeDb(masterIdx, currentVol - 12f);
                var tween = CreateTween();
                tween.TweenMethod(Callable.From<float>(vol => AudioServer.SetBusVolumeDb(masterIdx, vol)), currentVol - 12f, currentVol, durationSeconds);
            }
        }

        private static void EnsureBus(string name)
        {
            if (AudioServer.GetBusIndex(name) < 0)
            {
                AudioServer.AddBus(AudioServer.BusCount);
                AudioServer.SetBusName(AudioServer.BusCount - 1, name);
                string sendTo = name switch
                {
                    AudioBusNames.Machinery => AudioBusNames.Sfx,
                    AudioBusNames.ShelterSocial => AudioBusNames.Sfx,
                    AudioBusNames.Subterranean => AudioBusNames.Ambience,
                    _ => AudioBusNames.Master
                };
                AudioServer.SetBusSend(AudioServer.BusCount - 1, sendTo);
            }
        }

        private void SetupPlayers()
        {
            _musicPlayerA = new AudioStreamPlayer { Bus = AudioBusNames.Music };
            AddChild(_musicPlayerA);
            _musicPlayerB = new AudioStreamPlayer { Bus = AudioBusNames.Music };
            AddChild(_musicPlayerB);
        }

        public override void _Process(double delta)
        {
            float dt = (float)delta;

            // Host sessions can be created or cleared after this node is ready.
            // Reference-equality guards make this cheap when nothing changed.
            RefreshDomainBindings();

            // Music crossfade
            if (_crossfading)
            {
                _musicCrossfade += dt * 0.5f;
                if (_musicCrossfade >= 1.0f)
                {
                    _musicCrossfade = 1.0f;
                    _crossfading = false;
                    (_musicActiveA ? _musicPlayerB : _musicPlayerA).VolumeDb = -80;
                }
                else
                {
                    _musicPlayerA.VolumeDb = _musicActiveA
                        ? Mathf.Lerp(0, -80, _musicCrossfade)
                        : Mathf.Lerp(-80, 0, _musicCrossfade);
                    _musicPlayerB.VolumeDb = _musicActiveA
                        ? Mathf.Lerp(-80, 0, _musicCrossfade)
                        : Mathf.Lerp(0, -80, _musicCrossfade);
                }
            }

            // Cooldown decay
            if (_cooldowns.Count > 0)
            {
                _expiredCooldowns.Clear();
                foreach (var kvp in _cooldowns)
                {
                    float remaining = kvp.Value - dt;
                    if (remaining <= CooldownEpsilon)
                        _expiredCooldowns.Add(kvp.Key);
                    else
                        _cooldowns[kvp.Key] = remaining;
                }
                for (int i = 0; i < _expiredCooldowns.Count; i++)
                    _cooldowns.Remove(_expiredCooldowns[i]);
            }

            // Reclaim finished one-shots to pool
            for (int i = _activeOneShots.Count - 1; i >= 0; i--)
            {
                var p = _activeOneShots[i];
                if (!p.Playing)
                {
                    p.Stream = null;
                    _activeOneShots.RemoveAt(i);
                    if (_pool.Count < MaxOneShotPlayers)
                        _pool.Push(p);
                    else
                        p.QueueFree();
                }
            }
        }

        public override void _ExitTree()
        {
            _eventBridge?.Dispose();
            _expansionEventBridge?.Dispose();
            _eventBridge = null;
            _expansionEventBridge = null;
            _conditionBridge?.Dispose();
            _conditionBridge = null;
            _stateCoordinator?.QueueFree();
            _stateCoordinator = null;
            _shelterAudio?.Dispose();
            _shelterAudio = null;
            _surfaceAmbience?.Dispose();
            _surfaceAmbience = null;
            _domainProvider = null;

            if (_settingsChangedHandler != null)
            {
                AudioSettings.Instance.OnSettingsChanged -= _settingsChangedHandler;
                _settingsChangedHandler = null;
            }

            _loopKeys.Clear();
            foreach (string key in _loopPlayers.Keys)
                _loopKeys.Add(key);
            for (int i = 0; i < _loopKeys.Count; i++)
                StopLoop(_loopKeys[i]);

            if (Instance == this)
                Instance = null;
        }

        private void RefreshDomainBindings()
        {
            if (_eventBridge == null || _domainProvider == null)
                return;

            _eventBridge.SubscribeAll(
                _domainProvider.AudioRadiation,
                _domainProvider.AudioWeather,
                _domainProvider.AudioCombat,
                _domainProvider.AudioCrafting,
                _domainProvider.AudioExpeditions,
                _domainProvider.AudioDisease,
                _domainProvider.AudioSurvivorFate,
                _domainProvider.AudioFlashbacks);

            if (_domainProvider is IExpansionAudioProvider expansionProvider && _expansionEventBridge != null)
            {
                _expansionEventBridge.SubscribeAll(expansionProvider);
            }

            _conditionBridge?.Bind(_domainProvider.AudioConditions);

            _shelterAudio?.Subscribe(
                _domainProvider.AudioPowerGrid,
                _domainProvider.AudioStartingLevel);
            _surfaceAmbience?.Subscribe(_domainProvider.AudioWeather);
        }

        // ── Cue-based playback (primary API) ────────────────────

        /// <summary>
        /// Play a cue by its stable snake_case ID.
        /// Respects cooldown, settings, headless mode, and missing-resource fallback.
        /// </summary>
        public void PlayCue(string cueId)
        {
            var cue = AudioCueCatalog.Resolve(cueId);
            if (cue == null)
            {
                LogMissingOnce($"cue:{cueId}");
                return;
            }
            PlayCueDef(cue);
        }

        /// <summary>
        /// Play a cue definition directly.
        /// </summary>
        public void PlayCueDef(AudioCueDef cue)
        {
            if (_headless) return;

            // Cooldown check
            if (cue.CooldownSeconds > 0 && _cooldowns.ContainsKey(cue.Id))
                return;

            string chosenPath = cue.ResourcePath;
            if (cue.ResourcePaths.Count > 1)
            {
                int idx = (int)(GD.Randi() % (uint)cue.ResourcePaths.Count);
                chosenPath = cue.ResourcePaths[idx];
            }

            var stream = LoadStream(chosenPath);
            if (stream == null)
            {
                // Try fallback
                if (cue.FallbackCueId != null)
                {
                    var fallback = AudioCueCatalog.Resolve(cue.FallbackCueId);
                    if (fallback != null) PlayCueDef(fallback);
                }
                return;
            }

            // Apply cooldown
            if (cue.CooldownSeconds > 0)
                _cooldowns[cue.Id] = cue.CooldownSeconds;

            float effectiveDb = cue.DefaultVolumeDb + GetBusVolumeOffset(cue.Bus);
            if (cue.VolumeJitterDb > 0f)
            {
                effectiveDb += (float)GD.RandRange(-cue.VolumeJitterDb, cue.VolumeJitterDb);
            }
            else if (!cue.Loop && cue.Bus != AudioBusNames.Music && cue.Bus != AudioBusNames.Voice)
            {
                // Subtle anti-fatigue volume micro-jitter for one-shots
                effectiveDb += (float)GD.RandRange(-0.6f, 0.6f);
            }

            float pitchScale = 1f;
            if (cue.PitchMin < cue.PitchMax)
            {
                pitchScale = (float)GD.RandRange(cue.PitchMin, cue.PitchMax);
            }
            else if (!cue.Loop && cue.Bus != AudioBusNames.Music && cue.Bus != AudioBusNames.Voice)
            {
                // Subtle anti-fatigue pitch micro-jitter for one-shots (±3%)
                pitchScale = (float)GD.RandRange(0.97f, 1.03f);
            }

            if (cue.Loop)
            {
                PlayLoopStream(cue.Id, stream, cue.Bus, effectiveDb, pitchScale);
            }
            else
            {
                PlayOneShotStream(stream, cue.Bus, effectiveDb, pitchScale);
            }
        }

        // ── Legacy convenience API (backward-compatible) ────────

        public void PlayUiClick() => PlayCue(AudioCueCatalog.UiClick);
        public void PlayUiConfirm() => PlayCue(AudioCueCatalog.UiConfirm);
        public void PlayUiWarning() => PlayCue(AudioCueCatalog.UiWarning);
        public void PlayUiSwitch() => PlayCue(AudioCueCatalog.UiSwitchToggle);
        public void PlayUiRotary() => PlayCue(AudioCueCatalog.UiRotaryClick);
        public void PlayRotaryClick() => PlayCue(AudioCueCatalog.UiRotaryClick);
        public void PlayUiCrtPowerOn() => PlayCue(AudioCueCatalog.UiCrtPowerOn);
        public void PlayUiPaper() => PlayCue(AudioCueCatalog.UiPaperRustle);
        public void PlayUiStamp() => PlayCue(AudioCueCatalog.UiStampHeavy);
        public void PlayUiDrawer() => PlayCue(AudioCueCatalog.UiDrawerSlide);
        public void PlayTapeInsert() => PlayCue(AudioCueCatalog.LogTapeInsert);
        public void PlayTapeButton() => PlayCue(AudioCueCatalog.LogTapeButton);
        public void PlayTapeHiss() => PlayCue(AudioCueCatalog.LogTapeHiss);
        public void StopTapeHiss() => StopCue(AudioCueCatalog.LogTapeHiss);
        public void PlayTapeRewind() => PlayCue(AudioCueCatalog.LogTapeRewind);
        public void PlayTapeStop() => PlayCue(AudioCueCatalog.LogTapeStop);
        public void PlayEchoDiscovery() => PlayCue(AudioCueCatalog.EchoDiscovery);
        public void PlayItemHandling(string itemCategoryOrId)
        {
            if (string.IsNullOrEmpty(itemCategoryOrId)) return;
            string lower = itemCategoryOrId.ToLowerInvariant();
            if (lower.Contains("ammo") || lower.Contains("bullet") || lower.Contains("shell") || lower.Contains("weapon"))
                PlayCue(AudioCueCatalog.ItemHandlingAmmo);
            else if (lower.Contains("med") || lower.Contains("pill") || lower.Contains("cure") || lower.Contains("bandage") || lower.Contains("rad"))
                PlayCue(AudioCueCatalog.ItemHandlingMeds);
            else if (lower.Contains("ration") || lower.Contains("food") || lower.Contains("water") || lower.Contains("grain") || lower.Contains("mre"))
                PlayCue(AudioCueCatalog.ItemHandlingRation);
            else
                PlayCue(AudioCueCatalog.ActionItemPickup);
        }

        /// <summary>
        /// Plays material-specific footsteps based on the ground surface or location:
        /// granite (rock/stone/cave/arcology), metal (bunker/deck/steel), dirt (wasteland/soil),
        /// glass (shattered ruins/hospital), or wood (planks/houses/timber).
        /// Each material resolves to a 5-sample acoustic pool with anti-fatigue jitter.
        /// </summary>
        public void PlayFootstep(string materialOrLocationId)
        {
            if (string.IsNullOrEmpty(materialOrLocationId))
            {
                PlayCue(AudioCueCatalog.FootstepDirt);
                return;
            }
            string lower = materialOrLocationId.ToLowerInvariant();
            if (lower.Contains("granite") || lower.Contains("rock") || lower.Contains("stone") || lower.Contains("quarry") || lower.Contains("cave") || lower.Contains("geothermal") || lower.Contains("arcology"))
                PlayCue(AudioCueCatalog.FootstepGranite);
            else if (lower.Contains("metal") || lower.Contains("bunker") || lower.Contains("steel") || lower.Contains("deck") || lower.Contains("plate") || lower.Contains("station"))
                PlayCue(AudioCueCatalog.FootstepMetal);
            else if (lower.Contains("glass") || lower.Contains("hospital") || lower.Contains("window") || lower.Contains("pharmacy"))
                PlayCue(AudioCueCatalog.FootstepGlass);
            else if (lower.Contains("wood") || lower.Contains("house") || lower.Contains("plank") || lower.Contains("suburban") || lower.Contains("timber"))
                PlayCue(AudioCueCatalog.FootstepWood);
            else
                PlayCue(AudioCueCatalog.FootstepDirt);
        }

        public void PlayVehicleRefuel() => PlayCue(AudioCueCatalog.ExpeditionVehicleRefuel);
        public void PlayVehicleRepair() => PlayCue(AudioCueCatalog.ExpeditionVehicleRepair);
        public void PlayRadiationAlert() => PlayCue(AudioCueCatalog.RadAlertAcute);
        public void PlayWeatherAlert() => PlayCue(AudioCueCatalog.WeatherAlert);

        public void StartGeiger(bool intense = false) =>
            PlayCue(intense ? AudioCueCatalog.RadGeigerIntense : AudioCueCatalog.RadGeigerLoop);
        public void StopGeiger()
        {
            StopCue(AudioCueCatalog.RadGeigerLoop);
            StopCue(AudioCueCatalog.RadGeigerIntense);
        }
        public void StartBunkerAmbience()
        {
            _surfaceAmbience?.Stop();
            PlayCue(AudioCueCatalog.AmbBunker);
        }

        /// <summary>
        /// Begins the explicit surface listening mode. Its loop follows weather
        /// while active; an expedition alone never activates it because an
        /// expedition does not establish the player's listening location.
        /// </summary>
        public void StartSurfaceAmbience()
        {
            if (_surfaceAmbience != null)
                _surfaceAmbience.Start();
            else
                PlayCue(AudioCueCatalog.AmbSurface);
        }

        public void StopAmbience()
        {
            _surfaceAmbience?.Stop();
            StopLoopsOnBus(AudioBusNames.Ambience);
            // Shelter infrastructure uses independent buses so players can mix
            // them separately, but it still belongs to the active-run ambience
            // lifecycle and must not bleed through the menu or game-over screen.
            StopCue(AudioCueCatalog.ShelterGenerator);
            StopCue(AudioCueCatalog.ShelterVentilation);
        }

        /// <summary>Stops a loop cue without touching other loops on its bus.</summary>
        public void StopCue(string cueId)
        {
            if (string.IsNullOrWhiteSpace(cueId)) return;
            StopLoop(cueId);
        }

        public void PlayMainMenuMusic() => PlayMusicStream(LoadStream("res://assets/audio/music/main_menu.ogg"));
        public void PlayGameplayMusic() => PlayMusicStream(LoadStream("res://assets/audio/music/gameplay_underscore.ogg"));

        public void StopMusic()
        {
            _musicPlayerA.Stop();
            _musicPlayerB.Stop();
            _musicPlayerA.Stream = null;
            _musicPlayerB.Stream = null;
        }

        public void PlayRadioStatic() => PlayCue(AudioCueCatalog.RadioStatic);
        public void PlayNumbersStation() => PlayCue(AudioCueCatalog.RadioNumbersStation);
        public void StopNumbersStation() => StopCue(AudioCueCatalog.RadioNumbersStation);
        public void PlayEbsAlert() => PlayCue(AudioCueCatalog.RadioEbsAlert);
        public void PlayDeadHandPulse() => PlayCue(AudioCueCatalog.RadioDeadHandPulse);
        public void StopDeadHandPulse() => StopCue(AudioCueCatalog.RadioDeadHandPulse);
        public void PlayDistressBeacon() => PlayCue(AudioCueCatalog.RadioDistressBeacon);
        public void StopDistressBeacon() => StopCue(AudioCueCatalog.RadioDistressBeacon);

        /// <summary>
        /// Play a registered radio voice cue. Keeping radio speech in the cue
        /// catalog gives it the same resource validation, trim, and cooldown
        /// behavior as every other runtime sound.
        /// </summary>
        public void PlayVoiceOverCue(string cueId)
        {
            var cue = AudioCueCatalog.Resolve(cueId);
            if (cue == null || cue.Bus != AudioBusNames.Voice)
            {
                LogMissingOnce($"radio-voice-cue:{cueId}");
                return;
            }

            PlayCueDef(cue);
        }

        public void PlayVoiceOver(string resourceName)
        {
            if (AudioCueCatalog.Contains(resourceName))
            {
                PlayVoiceOverCue(resourceName);
                return;
            }

            string path = $"res://assets/audio/radio/{resourceName}.wav";
            var stream = LoadStream(path);
            if (stream == null) return;
            PlayOneShotStream(stream, AudioBusNames.Voice, GetBusVolumeOffset(AudioBusNames.Voice));
        }

        // ── Settings application ────────────────────────────────

        public void ApplySettings(AudioSettings settings)
        {
            if (settings == null) return;

            SetBusVolume(AudioBusNames.Master, settings.MasterVolume, settings.MasterMute);
            SetBusVolume(AudioBusNames.Music, settings.MusicVolume, settings.MusicMute);
            SetBusVolume(AudioBusNames.Ambience, settings.AmbienceVolume, settings.AmbienceMute);
            SetBusVolume(AudioBusNames.Sfx, settings.SfxVolume, settings.SfxMute);
            SetBusVolume(AudioBusNames.Ui, settings.UiVolume, settings.UiMute);
            SetBusVolume(AudioBusNames.Voice, settings.VoiceVolume, settings.VoiceMute);
            SetBusVolume(AudioBusNames.Alerts, settings.AlertVolume, settings.AlertMute);
            SetBusVolume(AudioBusNames.Generator, settings.GeneratorVolume, settings.GeneratorMute);
            SetBusVolume(AudioBusNames.Ventilation, settings.VentilationVolume, settings.VentilationMute);
            SetBusVolume(AudioBusNames.Radio, settings.RadioVolume, settings.RadioMute);
            SetBusVolume(AudioBusNames.Medical, settings.MedicalVolume, settings.MedicalMute);
            SetBusVolume(AudioBusNames.Surface, settings.SurfaceVolume, settings.SurfaceMute);
        }

        private void SetBusVolume(string bus, float percent, bool mute)
        {
            int idx = AudioServer.GetBusIndex(bus);
            if (idx < 0) return;
            AudioServer.SetBusMute(idx, mute);
            AudioServer.SetBusVolumeDb(idx, AudioSettings.PercentToDb(percent));
        }

        private float GetBusVolumeOffset(string bus)
        {
            var s = AudioSettings.Instance;
            float categoryVol = bus switch
            {
                AudioBusNames.Music => s.MusicVolume,
                AudioBusNames.Ambience => s.AmbienceVolume,
                AudioBusNames.Sfx => s.SfxVolume,
                AudioBusNames.Ui => s.UiVolume,
                AudioBusNames.Voice => s.VoiceVolume,
                AudioBusNames.Alerts => s.AlertVolume,
                AudioBusNames.Generator => s.GeneratorVolume,
                AudioBusNames.Ventilation => s.VentilationVolume,
                AudioBusNames.Radio => s.RadioVolume,
                AudioBusNames.Medical => s.MedicalVolume,
                AudioBusNames.Surface => s.SurfaceVolume,
                _ => s.MasterVolume,
            };
            // Return 0 — bus volume is handled by AudioServer, not per-player offset
            return 0f;
        }

        // ── Internal playback ───────────────────────────────────

        private AudioStream? LoadStream(string path)
        {
            if (_cache.TryGetValue(path, out var cached))
                return cached;

            var stream = ResourceLoader.Load<AudioStream>(path) ?? LoadDirectStream(path);
            if (stream != null)
            {
                _cache[path] = stream;
            }
            else
            {
                LogMissingOnce(path);
            }
            return stream;
        }

        public static AudioStream? LoadDirectStream(string resPath)
        {
            try
            {
                string osPath = ProjectSettings.GlobalizePath(resPath);
                if (!File.Exists(osPath)) return null;

                if (resPath.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase))
                {
                    var mp3 = new AudioStreamMP3();
                    mp3.Data = File.ReadAllBytes(osPath);
                    return mp3;
                }
                if (resPath.EndsWith(".ogg", StringComparison.OrdinalIgnoreCase))
                {
                    return AudioStreamOggVorbis.LoadFromFile(osPath);
                }
                if (resPath.EndsWith(".wav", StringComparison.OrdinalIgnoreCase))
                {
                    return LoadWavStream(osPath);
                }
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[AudioManager] Direct stream load failed for {resPath}: {ex.Message}");
            }
            return null;
        }

        public static AudioStreamWav? LoadWavStream(string osPath)
        {
            try
            {
                byte[] bytes = File.ReadAllBytes(osPath);
                if (bytes.Length < 44)
                {
                    GD.PrintErr($"[AudioManager] WAV file too small: {osPath}");
                    return null;
                }

                if (bytes[0] != 'R' || bytes[1] != 'I' || bytes[2] != 'F' || bytes[3] != 'F' ||
                    bytes[8] != 'W' || bytes[9] != 'A' || bytes[10] != 'V' || bytes[11] != 'E')
                {
                    GD.PrintErr($"[AudioManager] Invalid RIFF/WAVE header: {osPath}");
                    return null;
                }

                int channels = 1;
                int sampleRate = 44100;
                int bitsPerSample = 16;
                byte[]? pcmData = null;

                int offset = 12;
                while (offset + 8 <= bytes.Length)
                {
                    string chunkId = System.Text.Encoding.ASCII.GetString(bytes, offset, 4);
                    int chunkSize = BitConverter.ToInt32(bytes, offset + 4);
                    offset += 8;

                    if (chunkSize < 0 || offset + chunkSize > bytes.Length)
                    {
                        break;
                    }

                    if (chunkId == "fmt " && chunkSize >= 16)
                    {
                        channels = BitConverter.ToInt16(bytes, offset + 2);
                        sampleRate = BitConverter.ToInt32(bytes, offset + 4);
                        bitsPerSample = BitConverter.ToInt16(bytes, offset + 14);
                    }
                    else if (chunkId == "data")
                    {
                        pcmData = new byte[chunkSize];
                        Buffer.BlockCopy(bytes, offset, pcmData, 0, chunkSize);
                        break;
                    }

                    offset += chunkSize;
                    if ((chunkSize & 1) != 0)
                    {
                        offset++;
                    }
                }

                if (pcmData == null)
                {
                    GD.PrintErr($"[AudioManager] No data chunk found in WAV: {osPath}");
                    return null;
                }

                return new AudioStreamWav
                {
                    Data = pcmData,
                    Format = (bitsPerSample == 16) ? AudioStreamWav.FormatEnum.Format16Bits : AudioStreamWav.FormatEnum.Format8Bits,
                    MixRate = sampleRate,
                    Stereo = (channels == 2)
                };
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[AudioManager] WAV container parsing failed for {osPath}: {ex.Message}");
                return null;
            }
        }

        private void PlayOneShotStream(AudioStream stream, string bus, float volumeDb, float pitchScale = 1f)
        {
            AudioStreamPlayer player;
            if (_pool.Count > 0)
            {
                player = _pool.Pop();
            }
            else if (_activeOneShots.Count < MaxOneShotPlayers)
            {
                player = new AudioStreamPlayer();
                AddChild(player);
            }
            else
            {
                // Pool exhausted — skip rather than allocate unbounded
                return;
            }

            player.Stream = stream;
            player.Bus = bus;
            player.VolumeDb = volumeDb;
            player.PitchScale = pitchScale;
            player.Play();
            _activeOneShots.Add(player);
        }

        private void PlayLoopStream(string loopKey, AudioStream stream, string bus, float volumeDb, float pitchScale = 1f, float fadeIn = 0f)
        {
            if (stream is AudioStreamWav wav)
                wav.LoopMode = AudioStreamWav.LoopModeEnum.Forward;
            else if (stream is AudioStreamOggVorbis ogg)
                ogg.Loop = true;
            else if (stream is AudioStreamMP3 mp3)
                mp3.Loop = true;

            if (!_loopPlayers.TryGetValue(loopKey, out AudioStreamPlayer? player))
            {
                player = new AudioStreamPlayer();
                _loopPlayers.Add(loopKey, player);
                AddChild(player);
            }

            bool streamChanged = !ReferenceEquals(player.Stream, stream);
            player.Stream = stream;
            player.Bus = bus;
            player.VolumeDb = volumeDb;
            player.PitchScale = pitchScale;
                        if (streamChanged || !player.Playing)
            {
                if (fadeIn > 0f)
                {
                    player.VolumeDb = -80f;
                    player.Play();
                    var tween = CreateTween();
                    tween.TweenProperty(player, "volume_db", volumeDb, fadeIn);
                }
                else
                {
                    player.Play();
                }
            }
        }

        private void StopLoop(string loopKey, float fadeOut = 0f)
        {
            if (!_loopPlayers.Remove(loopKey, out AudioStreamPlayer? player))
                return;


            if (fadeOut > 0f)
            {
                var tween = CreateTween();
                tween.TweenProperty(player, "volume_db", -80f, fadeOut);
                tween.TweenCallback(Callable.From(() => {
                    player.Stop();
                    player.Stream = null;
                    player.QueueFree();
                }));
            }
            else
            {
                player.Stop();
                player.Stream = null;
                player.QueueFree();
            }
        }

        private void StopLoopsOnBus(string bus)
        {
            _loopKeys.Clear();
            foreach (var entry in _loopPlayers)
            {
                if (entry.Value.Bus == bus)
                    _loopKeys.Add(entry.Key);
            }

            for (int i = 0; i < _loopKeys.Count; i++)
                StopLoop(_loopKeys[i]);
        }

        private void PlayMusicStream(AudioStream? stream)
        {
            if (stream == null) return;

            if (_musicActiveA)
            {
                _musicPlayerB.Stream = stream;
                _musicPlayerB.VolumeDb = -80;
                _musicPlayerB.Play();
            }
            else
            {
                _musicPlayerA.Stream = stream;
                _musicPlayerA.VolumeDb = -80;
                _musicPlayerA.Play();
            }
            _musicCrossfade = 0f;
            _crossfading = true;
            _musicActiveA = !_musicActiveA;
        }

        // ── Diagnostics ─────────────────────────────────────────

        private void LogMissingOnce(string key)
        {
            if (_loggedMissing.Add(key))
                GD.PrintErr($"[AudioManager] Missing: {key}");
        }

        public int MissingAssetCount => _loggedMissing.Count;
        public int ActiveOneShotCount => _activeOneShots.Count;
        public int ActiveLoopCount => _loopPlayers.Count;
        public int PoolAvailable => _pool.Count;
        public bool IsHeadless => _headless;

        // ── Core condition bridge ──────────────────────────────

        /// <summary>
        /// Route a Core AudioConditionSystem condition to the appropriate bus.
        /// Called by the host session when Core raises audio events.
        /// </summary>
        public void RouteCondition(string audioKey, string bus, float intensity = 1f, bool loop = false)
        {
            if (string.IsNullOrEmpty(audioKey)) return;
            var cue = AudioCueCatalog.Resolve(audioKey);
            if (cue == null) return;

            var stream = LoadStream(cue.ResourcePath);
            if (stream == null) return;

            float volumeDb = cue.DefaultVolumeDb + GetBusVolumeOffset(cue.Bus);
            if (cue.VolumeJitterDb > 0) volumeDb += (float)Godot.GD.RandRange(-cue.VolumeJitterDb, cue.VolumeJitterDb);
            float pitch = 1f;
            if (cue.PitchMin < cue.PitchMax) pitch = (float)Godot.GD.RandRange(cue.PitchMin, cue.PitchMax);

            if (loop)
                PlayLoopStream(audioKey, stream, cue.Bus, volumeDb, pitch, cue.FadeInSeconds);
            else
                PlayOneShotStream(stream, cue.Bus, volumeDb, pitch);
        }

        public void SetLoopIntensity(string loopKey, float intensity01)
        {
            if (!_loopPlayers.TryGetValue(loopKey, out var player)) return;
            var cue = AudioCueCatalog.Resolve(loopKey);
            if (cue == null) return;

            // Map intensity to volume attenuation (e.g. 0 intensity = -40dB, 1 intensity = base volume)
            float baseVol = cue.DefaultVolumeDb + GetBusVolumeOffset(cue.Bus);
            float attenuation = Mathf.Lerp(-40f, 0f, intensity01);
            player.VolumeDb = baseVol + attenuation;
        }

        public void StopCondition(string audioKey)
        {
            if (string.IsNullOrEmpty(audioKey)) return;
            var cue = AudioCueCatalog.Resolve(audioKey);
            if (cue == null) return;
            StopLoop(cue.Id, cue.FadeOutSeconds);
        }

        public void SetSnapshot(AudioSnapshot snapshot) => _stateCoordinator?.SetSnapshot(snapshot);

        // ── Explicit Context-Owned Loop Lifecycle (Task 5 / Plan 46–49) ──

        public void StartLoop(string cueId, string ownerKey)
        {
            if (string.IsNullOrEmpty(cueId) || string.IsNullOrEmpty(ownerKey)) return;
            string loopKey = $"{ownerKey}:{cueId}";
            var cue = AudioCueCatalog.Resolve(cueId);
            if (cue == null) return;

            var stream = LoadStream(cue.ResourcePath);
            if (stream == null && !string.IsNullOrEmpty(cue.FallbackCueId))
            {
                var fallbackCue = AudioCueCatalog.Resolve(cue.FallbackCueId);
                if (fallbackCue != null) stream = LoadStream(fallbackCue.ResourcePath);
            }
            if (stream == null) return;

            float volumeDb = cue.DefaultVolumeDb + GetBusVolumeOffset(cue.Bus);
            PlayLoopStream(loopKey, stream, cue.Bus, volumeDb, 1f, cue.FadeInSeconds);
        }

        public void UpdateLoop(string cueId, string ownerKey, float volumeDbOffset = 0f, float pitchScale = 1f)
        {
            if (string.IsNullOrEmpty(cueId) || string.IsNullOrEmpty(ownerKey)) return;
            string loopKey = $"{ownerKey}:{cueId}";
            if (_loopPlayers.TryGetValue(loopKey, out var player))
            {
                var cue = AudioCueCatalog.Resolve(cueId);
                float baseVol = cue != null ? cue.DefaultVolumeDb + GetBusVolumeOffset(cue.Bus) : 0f;
                player.VolumeDb = baseVol + volumeDbOffset;
                player.PitchScale = Mathf.Clamp(pitchScale, 0.1f, 4f);
            }
        }

        public void StopLoop(string cueId, string ownerKey)
        {
            if (string.IsNullOrEmpty(cueId) || string.IsNullOrEmpty(ownerKey)) return;
            string loopKey = $"{ownerKey}:{cueId}";
            var cue = AudioCueCatalog.Resolve(cueId);
            StopLoop(loopKey, cue?.FadeOutSeconds ?? 0f);
        }

        public void UpdateRadioTuningHeterodyne(float frequencyKhz, float minKhz = 3000f, float maxKhz = 30000f)
        {
            float norm = Mathf.Clamp((frequencyKhz - minKhz) / (maxKhz - minKhz), 0f, 1f);
            float pitch = Mathf.Lerp(0.8f, 1.4f, norm);
            UpdateLoop(AudioCueCatalog.RadioTuningHeterodyne, "radio:tuner", 0f, pitch);
        }
    }
}
