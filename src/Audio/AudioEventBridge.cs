using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Combat;
using Ashfall.Core.Crafting;
using Ashfall.Core.Disease;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Radiation;
using Ashfall.Core.Shelter;
using Ashfall.Core.StartingLevel;
using Ashfall.Core.Survivors;
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
        SurvivorFateSystem? AudioSurvivorFate { get; }
        PowerGridSystem? AudioPowerGrid { get; }
        StartingLevelSystem? AudioStartingLevel { get; }
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
        private SurvivorFateSystem? _survivorFate;
        private readonly Dictionary<string, float> _radiationDoseBySurvivor = new(StringComparer.Ordinal);
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
            DiseaseSystem? disease = null,
            SurvivorFateSystem? survivorFate = null)
        {
            ThrowIfDisposed();
            BindRadiation(radiation);
            BindWeather(weather);
            BindCombat(combat);
            BindCrafting(crafting);
            BindExpeditions(expeditions);
            BindDisease(disease);
            BindSurvivorFate(survivorFate);
        }

        public void BindRadiation(RadiationSystem? radiation)
        {
            ThrowIfDisposed();
            if (ReferenceEquals(_radiation, radiation))
                return;

            if (_radiation != null)
            {
                _radiation.OnStatusGained -= OnRadiationStatusGained;
                _radiation.OnDoseChanged -= OnRadiationDoseChanged;
            }

            _radiation = radiation;
            _radiationDoseBySurvivor.Clear();
            if (_radiation != null)
            {
                foreach (SurvivorRadState survivor in _radiation.Registered)
                {
                    if (survivor != null && !string.IsNullOrEmpty(survivor.Id))
                        _radiationDoseBySurvivor[survivor.Id] = survivor.RadiationDose;
                }
                _radiation.OnStatusGained += OnRadiationStatusGained;
                _radiation.OnDoseChanged += OnRadiationDoseChanged;
            }
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
                "repair" => AudioCueCatalog.ActionRepair,
                "bandage" => AudioCueCatalog.ActionInjection,
                "bleed" => AudioCueCatalog.MedHeartbeat,
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
                _expeditions.OnLootAdded -= OnLootAdded;
            }

            _expeditions = expeditions;
            if (_expeditions != null)
            {
                _expeditions.OnExpeditionStarted += OnExpeditionStarted;
                _expeditions.OnEncounterTriggered += OnEncounterTriggered;
                _expeditions.OnVehicleBreakdown += OnVehicleBreakdown;
                _expeditions.OnExpeditionCompleted += OnExpeditionCompleted;
                _expeditions.OnExpeditionFailed += OnExpeditionFailed;
                _expeditions.OnLootAdded += OnLootAdded;
            }
        }

        private void OnExpeditionStarted(ExpeditionState state) => _playCue(AudioCueCatalog.ShelterDoorOpen);
        private void OnEncounterTriggered(ExpeditionState state) => _playCue(AudioCueCatalog.CombatStart);
        private void OnVehicleBreakdown(ExpeditionState state) => _playCue(AudioCueCatalog.DangerAlarmKlaxon);
        private void OnExpeditionCompleted(ExpeditionState state)
        {
            _playCue(AudioCueCatalog.ShelterDoorSeal);
            _playCue(AudioCueCatalog.ActionItemPickup);
        }
        private void OnExpeditionFailed(ExpeditionState state, string reason) => _playCue(AudioCueCatalog.DangerDebris);
        private void OnLootAdded(ExpeditionState state) => _playCue(AudioCueCatalog.ActionItemPickup);

        public void BindDisease(DiseaseSystem? disease)
        {
            ThrowIfDisposed();
            if (ReferenceEquals(_disease, disease))
                return;

            if (_disease != null)
            {
                _disease.OnOutbreakDeclared -= OnOutbreakDeclared;
                _disease.OnInfection -= OnInfection;
                _disease.OnQuarantineStarted -= OnQuarantineStarted;
                _disease.OnQuarantineEnded -= OnQuarantineEnded;
                _disease.OnOutbreakContained -= OnOutbreakContained;
                _disease.OnOutcomeResolved -= OnOutcomeResolved;
            }

            _disease = disease;
            if (_disease != null)
            {
                _disease.OnOutbreakDeclared += OnOutbreakDeclared;
                _disease.OnInfection += OnInfection;
                _disease.OnQuarantineStarted += OnQuarantineStarted;
                _disease.OnQuarantineEnded += OnQuarantineEnded;
                _disease.OnOutbreakContained += OnOutbreakContained;
                _disease.OnOutcomeResolved += OnOutcomeResolved;
            }
        }

        private void OnOutbreakDeclared(string diseaseId) => _playCue(AudioCueCatalog.MedCoughing);
        private void OnInfection(string survivorId, string diseaseId) => _playCue(AudioCueCatalog.MedHeartbeat);
        private void OnQuarantineStarted(string survivorId, string diseaseId) => _playCue(AudioCueCatalog.MedQuarantineSeal);
        private void OnQuarantineEnded(string survivorId, string diseaseId) => _playCue(AudioCueCatalog.MedQuarantineClear);
        private void OnOutbreakContained(string diseaseId, bool prevented) => _playCue(AudioCueCatalog.MedQuarantineClear);

        private void OnOutcomeResolved(string survivorId, string diseaseId, bool recovered)
        {
            // Fatal outcomes enter the all-cause SurvivorFate pipeline, which
            // owns the distinct loss cue. Recovery gets the quieter clearance
            // feedback here; AudioManager's cue cooldown absorbs same-tick
            // recovery + automatic quarantine-release pairs.
            if (recovered)
                _playCue(AudioCueCatalog.MedQuarantineClear);
        }

        /// <summary>
        /// Binds the unified Core death pipeline rather than individual sources.
        /// The fate system emits only after its idempotent all-cause cascade, so
        /// one real campaign loss produces one death cue.
        /// </summary>
        public void BindSurvivorFate(SurvivorFateSystem? survivorFate)
        {
            ThrowIfDisposed();
            if (ReferenceEquals(_survivorFate, survivorFate))
                return;

            if (_survivorFate != null)
                _survivorFate.OnSurvivorFate -= OnSurvivorFate;

            _survivorFate = survivorFate;
            if (_survivorFate != null)
                _survivorFate.OnSurvivorFate += OnSurvivorFate;
        }

        private void OnSurvivorFate(SurvivorFateEvent fate) =>
            _playCue(AudioCueCatalog.MedSurvivorDeath);

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

        private void OnRadiationDoseChanged(SurvivorRadState state, float dose)
        {
            if (state == null) return;
            string id = string.IsNullOrEmpty(state.Id) ? "__anonymous" : state.Id;
            float previousDose = _radiationDoseBySurvivor.TryGetValue(id, out float previous)
                ? previous
                : 0f;
            _radiationDoseBySurvivor[id] = dose;

            // A click denotes new exposure, not treatment or a restored save state.
            if (dose > previousDose + 0.01f)
                _playCue(AudioCueCatalog.RadGeigerBurst);
        }

        private void OnWeatherChanged(WeatherKind kind)
        {
            string? cueId = kind switch
            {
                WeatherKind.FalloutStorm => AudioCueCatalog.WeatherFalloutStorm,
                WeatherKind.BlackRain => AudioCueCatalog.WeatherBlackRain,
                WeatherKind.BloodRain => AudioCueCatalog.WeatherCorrosivePrecipitation,
                WeatherKind.Blizzard => AudioCueCatalog.WeatherBlizzard,
                WeatherKind.IceStorm => AudioCueCatalog.WeatherBlizzard,
                WeatherKind.Ashfall => AudioCueCatalog.WeatherFalloutStorm,
                WeatherKind.AcidSnow => AudioCueCatalog.WeatherCorrosivePrecipitation,
                WeatherKind.BlackSnow => AudioCueCatalog.WeatherBlizzard,
                WeatherKind.EMPStorm => AudioCueCatalog.WeatherEmpStorm,
                WeatherKind.GlassStorm => AudioCueCatalog.WeatherGlassStorm,
                WeatherKind.RadHail => AudioCueCatalog.WeatherGlassStorm,
                WeatherKind.AshLightning => AudioCueCatalog.WeatherEmpStorm,
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
            {
                _radiation.OnStatusGained -= OnRadiationStatusGained;
                _radiation.OnDoseChanged -= OnRadiationDoseChanged;
            }
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
                _expeditions.OnLootAdded -= OnLootAdded;
            }

            if (_disease != null)
            {
                _disease.OnOutbreakDeclared -= OnOutbreakDeclared;
                _disease.OnInfection -= OnInfection;
                _disease.OnQuarantineStarted -= OnQuarantineStarted;
                _disease.OnQuarantineEnded -= OnQuarantineEnded;
                _disease.OnOutbreakContained -= OnOutbreakContained;
                _disease.OnOutcomeResolved -= OnOutcomeResolved;
            }
            if (_survivorFate != null)
                _survivorFate.OnSurvivorFate -= OnSurvivorFate;

            _radiation = null;
            _radiationDoseBySurvivor.Clear();
            _weather = null;
            _combat = null;
            _crafting = null;
            _expeditions = null;
            _disease = null;
            _survivorFate = null;
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
        internal bool HasSurvivorFateBinding => _survivorFate != null;
    }
}
