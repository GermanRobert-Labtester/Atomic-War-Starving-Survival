using System;
using Ashfall.Core;
using Ashfall.Core.Combat;
using Ashfall.Core.Crafting;
using Ashfall.Core.Disease;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Radiation;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp.Audio
{
    /// <summary>
    /// Narrow host contract used by AudioManager to discover the live Core
    /// systems without owning gameplay state or depending on Main directly.
    /// </summary>
    public interface IAudioDomainProvider
    {
        RadiationSystem? AudioRadiation { get; }
        WeatherSystem? AudioWeather { get; }
        TacticalCombatSystem? AudioCombat { get; }
        CraftingSystem? AudioCrafting { get; }
        ExpeditionSystem? AudioExpeditions { get; }
        DiseaseSystem? AudioDisease { get; }
    }

    /// <summary>
    /// Subscribes to Core domain events and maps them to stable audio cue IDs.
    /// The bridge owns and releases its subscriptions; rebinding after a new
    /// campaign cannot leave handlers attached to stale host sessions.
    /// </summary>
    public sealed class AudioEventBridge : IDisposable
    {
        private readonly Action<string> _playCue;
        private RadiationSystem? _radiation;
        private WeatherSystem? _weather;
        private TacticalCombatSystem? _combat;
        private CraftingSystem? _crafting;
        private ExpeditionSystem? _expeditions;
        private DiseaseSystem? _disease;
        private bool _disposed;

        public AudioEventBridge(AudioManager audio)
            : this((audio ?? throw new ArgumentNullException(nameof(audio))).PlayCue)
        {
        }

        internal AudioEventBridge(Action<string> playCue)
        {
            _playCue = playCue ?? throw new ArgumentNullException(nameof(playCue));
        }

        /// <summary>
        /// Bind both domains. Safe to call every frame: unchanged references
        /// are no-ops, while replaced sessions are unsubscribed before binding.
        /// </summary>
        public void SubscribeAll(
            RadiationSystem? radiation = null,
            WeatherSystem? weather = null,
            TacticalCombatSystem? combat = null,
            CraftingSystem? crafting = null,
            ExpeditionSystem? expeditions = null,
            DiseaseSystem? disease = null)
        {
            ThrowIfDisposed();
            BindRadiation(radiation);
            BindWeather(weather);
            BindCombat(combat);
            BindCrafting(crafting);
            BindExpeditions(expeditions);
            BindDisease(disease);
        }

        public void BindRadiation(RadiationSystem? radiation)
        {
            ThrowIfDisposed();
            if (ReferenceEquals(_radiation, radiation))
                return;

            if (_radiation != null)
                _radiation.OnStatusGained -= OnRadiationStatusGained;

            _radiation = radiation;
            if (_radiation != null)
                _radiation.OnStatusGained += OnRadiationStatusGained;
        }

        public void BindWeather(WeatherSystem? weather)
        {
            ThrowIfDisposed();
            if (ReferenceEquals(_weather, weather))
                return;

            if (_weather != null)
                _weather.OnWeatherChanged -= OnWeatherChanged;

            _weather = weather;
            if (_weather != null)
                _weather.OnWeatherChanged += OnWeatherChanged;
        }

        public void BindCombat(TacticalCombatSystem? combat)
        {
            ThrowIfDisposed();
            if (ReferenceEquals(_combat, combat))
                return;

            if (_combat != null)
                _combat.OnCombatEvent -= OnCombatEvent;

            _combat = combat;
            if (_combat != null)
                _combat.OnCombatEvent += OnCombatEvent;
        }

        private void OnCombatEvent(CombatState state, CombatEvent evt)
        {
            string? cueId = evt.Kind switch
            {
                "encounter_start" => AudioCueCatalog.CombatStart,
                "fire" => AudioCueCatalog.CombatFire,
                "suppress" => AudioCueCatalog.CombatFire,
                "weapon_jam" => AudioCueCatalog.CombatJam,
                "reload" => AudioCueCatalog.CombatReload,
                "clear_jam" => AudioCueCatalog.CombatReload,
                "downed" => AudioCueCatalog.CombatDowned,
                "death" => AudioCueCatalog.CombatDowned,
                "mutual_kill" => AudioCueCatalog.CombatDowned,
                "victory" => AudioCueCatalog.CombatVictory,
                "defeat" => AudioCueCatalog.CombatDefeat,
                "trap" => AudioCueCatalog.CombatHit,
                "retreat_fail" => AudioCueCatalog.CombatHit,
                "enemy_fire" when evt.Detail.Contains("hits") => AudioCueCatalog.CombatHit,
                _ => null,
            };

            if (cueId != null)
                _playCue(cueId);
        }

        public void BindCrafting(CraftingSystem? crafting)
        {
            ThrowIfDisposed();
            if (ReferenceEquals(_crafting, crafting))
                return;

            if (_crafting != null)
                _crafting.OnCraftCompleted -= OnCraftCompleted;

            _crafting = crafting;
            if (_crafting != null)
                _crafting.OnCraftCompleted += OnCraftCompleted;
        }

        private void OnCraftCompleted(Recipe recipe)
        {
            _playCue(AudioCueCatalog.ActionCrafting);
        }

        public void BindExpeditions(ExpeditionSystem? expeditions)
        {
            ThrowIfDisposed();
            if (ReferenceEquals(_expeditions, expeditions))
                return;

            if (_expeditions != null)
            {
                _expeditions.OnExpeditionStarted -= OnExpeditionStarted;
                _expeditions.OnEncounterTriggered -= OnEncounterTriggered;
                _expeditions.OnVehicleBreakdown -= OnVehicleBreakdown;
                _expeditions.OnExpeditionCompleted -= OnExpeditionCompleted;
                _expeditions.OnExpeditionFailed -= OnExpeditionFailed;
            }

            _expeditions = expeditions;
            if (_expeditions != null)
            {
                _expeditions.OnExpeditionStarted += OnExpeditionStarted;
                _expeditions.OnEncounterTriggered += OnEncounterTriggered;
                _expeditions.OnVehicleBreakdown += OnVehicleBreakdown;
                _expeditions.OnExpeditionCompleted += OnExpeditionCompleted;
                _expeditions.OnExpeditionFailed += OnExpeditionFailed;
            }
        }

        private void OnExpeditionStarted(ExpeditionState state) => _playCue(AudioCueCatalog.ShelterDoorOpen);
        private void OnEncounterTriggered(ExpeditionState state) => _playCue(AudioCueCatalog.CombatStart);
        private void OnVehicleBreakdown(ExpeditionState state) => _playCue(AudioCueCatalog.DangerAlarmKlaxon);
        private void OnExpeditionCompleted(ExpeditionState state) => _playCue(AudioCueCatalog.ActionItemPickup);
        private void OnExpeditionFailed(ExpeditionState state, string reason) => _playCue(AudioCueCatalog.DangerDebris);

        public void BindDisease(DiseaseSystem? disease)
        {
            ThrowIfDisposed();
            if (ReferenceEquals(_disease, disease))
                return;

            if (_disease != null)
            {
                _disease.OnOutbreakDeclared -= OnOutbreakDeclared;
                _disease.OnInfection -= OnInfection;
            }

            _disease = disease;
            if (_disease != null)
            {
                _disease.OnOutbreakDeclared += OnOutbreakDeclared;
                _disease.OnInfection += OnInfection;
            }
        }

        private void OnOutbreakDeclared(string diseaseId) => _playCue(AudioCueCatalog.MedCoughing);
        private void OnInfection(string survivorId, string diseaseId) => _playCue(AudioCueCatalog.MedHeartbeat);

        private void OnRadiationStatusGained(SurvivorRadState state, SurvivorStatus status)
        {
            string? cueId = status switch
            {
                SurvivorStatus.AcuteRadiationSickness => AudioCueCatalog.RadAlertAcute,
                SurvivorStatus.ChronicIllness => AudioCueCatalog.RadAlertChronic,
                _ => null,
            };

            if (cueId != null)
                _playCue(cueId);
        }

        private void OnWeatherChanged(WeatherKind kind)
        {
            string? cueId = kind switch
            {
                WeatherKind.FalloutStorm => AudioCueCatalog.WeatherFalloutStorm,
                WeatherKind.BlackRain => AudioCueCatalog.WeatherBlackRain,
                WeatherKind.BloodRain => AudioCueCatalog.WeatherBlackRain,
                WeatherKind.Blizzard => AudioCueCatalog.WeatherBlizzard,
                WeatherKind.IceStorm => AudioCueCatalog.WeatherBlizzard,
                WeatherKind.Ashfall => AudioCueCatalog.WeatherFalloutStorm,
                WeatherKind.AcidSnow => AudioCueCatalog.WeatherBlackRain,
                WeatherKind.BlackSnow => AudioCueCatalog.WeatherBlizzard,
                WeatherKind.EMPStorm => AudioCueCatalog.WeatherAlert,
                WeatherKind.GlassStorm => AudioCueCatalog.WeatherAlert,
                WeatherKind.RadHail => AudioCueCatalog.WeatherAlert,
                WeatherKind.AshLightning => AudioCueCatalog.WeatherAlert,
                WeatherKind.BioFog => AudioCueCatalog.WeatherWindGust,
                WeatherKind.ParticulateFog => AudioCueCatalog.WeatherWindGust,
                _ => null,
            };

            if (cueId != null)
                _playCue(cueId);
        }

        /// <summary>
        /// Fire a one-shot cue for game-flow events without adding domain logic.
        /// </summary>
        public void NotifyGameFlow(string cueId)
        {
            ThrowIfDisposed();
            _playCue(cueId);
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            if (_radiation != null)
                _radiation.OnStatusGained -= OnRadiationStatusGained;
            if (_weather != null)
                _weather.OnWeatherChanged -= OnWeatherChanged;
            if (_combat != null)
                _combat.OnCombatEvent -= OnCombatEvent;
            if (_crafting != null)
                _crafting.OnCraftCompleted -= OnCraftCompleted;
            if (_expeditions != null)
            {
                _expeditions.OnExpeditionStarted -= OnExpeditionStarted;
                _expeditions.OnEncounterTriggered -= OnEncounterTriggered;
                _expeditions.OnVehicleBreakdown -= OnVehicleBreakdown;
                _expeditions.OnExpeditionCompleted -= OnExpeditionCompleted;
                _expeditions.OnExpeditionFailed -= OnExpeditionFailed;
            }

            if (_disease != null)
            {
                _disease.OnOutbreakDeclared -= OnOutbreakDeclared;
                _disease.OnInfection -= OnInfection;
            }

            _radiation = null;
            _weather = null;
            _combat = null;
            _crafting = null;
            _expeditions = null;
            _disease = null;
            _disposed = true;
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(AudioEventBridge));
        }

        internal bool HasRadiationBinding => _radiation != null;
        internal bool HasWeatherBinding => _weather != null;
        internal bool HasCombatBinding => _combat != null;
        internal bool HasCraftingBinding => _crafting != null;
        internal bool HasExpeditionsBinding => _expeditions != null;
        internal bool HasDiseaseBinding => _disease != null;
    }
}
