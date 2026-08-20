using Godot;
using System;
using System.Collections.Generic;

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
        public static AudioManager? Instance { get; private set; }

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
        };

        // ── Players ─────────────────────────────────────────────

        private AudioStreamPlayer _musicPlayerA = null!;
        private AudioStreamPlayer _musicPlayerB = null!;
        private AudioStreamPlayer _ambiencePlayer = null!;
        private bool _musicActiveA;
        private float _musicCrossfade;
        private bool _crossfading;

        // One-shot pool: bounded to prevent unbounded allocation
        private const int MaxOneShotPlayers = 16;
        private readonly Stack<AudioStreamPlayer> _pool = new();
        private readonly List<AudioStreamPlayer> _activeOneShots = new();

        // ── Cache ───────────────────────────────────────────────

        private readonly Dictionary<string, AudioStream> _cache = new();
        private readonly HashSet<string> _loggedMissing = new();

        // ── Cooldown / dedup ────────────────────────────────────

        private readonly Dictionary<string, float> _cooldowns = new();
        private const float CooldownEpsilon = 0.01f;

        // ── Headless detection ──────────────────────────────────

        private bool _headless;

        // ── Lifecycle ───────────────────────────────────────────

        public override void _Ready()
        {
            Instance = this;
            _headless = !DisplayServer.WindowCanDraw();

            SetupBuses();
            SetupPlayers();
            ApplySettings(AudioSettings.Instance);
            AudioSettings.Instance.OnSettingsChanged += () => ApplySettings(AudioSettings.Instance);

            GD.Print($"[AudioManager] Ready — {AllBuses.Length + 1} buses, " +
                     $"pool={MaxOneShotPlayers}, headless={_headless}");
        }

        private void SetupBuses()
        {
            foreach (string bus in AllBuses)
                EnsureBus(bus);
        }

        private static void EnsureBus(string name)
        {
            if (AudioServer.GetBusIndex(name) < 0)
            {
                AudioServer.AddBus(AudioServer.BusCount);
                AudioServer.SetBusName(AudioServer.BusCount - 1, name);
                AudioServer.SetBusSend(AudioServer.BusCount - 1, AudioBusNames.Master);
            }
        }

        private void SetupPlayers()
        {
            _musicPlayerA = new AudioStreamPlayer { Bus = AudioBusNames.Music };
            AddChild(_musicPlayerA);
            _musicPlayerB = new AudioStreamPlayer { Bus = AudioBusNames.Music };
            AddChild(_musicPlayerB);
            _ambiencePlayer = new AudioStreamPlayer { Bus = AudioBusNames.Ambience };
            AddChild(_ambiencePlayer);
        }

        public override void _Process(double delta)
        {
            float dt = (float)delta;

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
            var expired = new List<string>();
            foreach (var kvp in _cooldowns)
            {
                _cooldowns[kvp.Key] = kvp.Value - dt;
                if (_cooldowns[kvp.Key] <= CooldownEpsilon)
                    expired.Add(kvp.Key);
            }
            foreach (string key in expired)
                _cooldowns.Remove(key);

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
            if (Instance == this)
                Instance = null;
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

            var stream = LoadStream(cue.ResourcePath);
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

            if (cue.Loop)
            {
                PlayLoopStream(stream, cue.Bus, effectiveDb);
            }
            else
            {
                PlayOneShotStream(stream, cue.Bus, effectiveDb);
            }
        }

        // ── Legacy convenience API (backward-compatible) ────────

        public void PlayUiClick() => PlayCue(AudioCueCatalog.UiClick);
        public void PlayUiConfirm() => PlayCue(AudioCueCatalog.UiConfirm);
        public void PlayUiWarning() => PlayCue(AudioCueCatalog.UiWarning);
        public void PlayRadiationAlert() => PlayCue(AudioCueCatalog.RadAlertAcute);
        public void PlayWeatherAlert() => PlayCue(AudioCueCatalog.WeatherAlert);

        public void StartGeiger() => PlayCue(AudioCueCatalog.RadGeigerLoop);
        public void StartBunkerAmbience() => PlayCue(AudioCueCatalog.AmbBunker);
        public void StartSurfaceAmbience() => PlayCue(AudioCueCatalog.AmbSurface);

        public void StopAmbience()
        {
            _ambiencePlayer.Stop();
            _ambiencePlayer.Stream = null;
        }

        public void PlayMainMenuMusic() => PlayMusicStream(LoadStream("res://assets/audio/music/main_menu.wav"));
        public void PlayGameplayMusic() => PlayMusicStream(LoadStream("res://assets/audio/music/gameplay_underscore.wav"));

        public void StopMusic()
        {
            _musicPlayerA.Stop();
            _musicPlayerB.Stop();
            _musicPlayerA.Stream = null;
            _musicPlayerB.Stream = null;
        }

        public void PlayRadioStatic() => PlayCue(AudioCueCatalog.RadioStatic);

        public void PlayVoiceOver(string resourceName)
        {
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

            var stream = ResourceLoader.Load<AudioStream>(path);
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

        private void PlayOneShotStream(AudioStream stream, string bus, float volumeDb)
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
            player.Play();
            _activeOneShots.Add(player);
        }

        private void PlayLoopStream(AudioStream stream, string bus, float volumeDb)
        {
            if (stream is AudioStreamWav wav)
                wav.LoopMode = AudioStreamWav.LoopModeEnum.Forward;
            else if (stream is AudioStreamOggVorbis ogg)
                ogg.Loop = true;

            _ambiencePlayer.Stream = stream;
            _ambiencePlayer.Bus = bus;
            _ambiencePlayer.VolumeDb = volumeDb;
            _ambiencePlayer.Play();
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
            if (loop)
                PlayLoopStream(stream, cue.Bus, volumeDb);
            else
                PlayOneShotStream(stream, cue.Bus, volumeDb);
        }

        public void StopCondition(string audioKey)
        {
            if (string.IsNullOrEmpty(audioKey)) return;
            // Stop looped ambience by clearing the ambience player if it matches
            var cue = AudioCueCatalog.Resolve(audioKey);
            if (cue == null) return;

            if (_ambiencePlayer.Playing && _ambiencePlayer.Stream != null)
            {
                // Only stop if this is the currently looping stream
                var currentPath = _ambiencePlayer.Stream.ResourcePath;
                if (currentPath == cue.ResourcePath)
                    _ambiencePlayer.Stop();
            }
        }
    }
}
