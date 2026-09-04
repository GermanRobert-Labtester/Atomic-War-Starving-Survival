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
        Ashfall.Core.AudioConditionSystem? AudioConditions { get; }
        SomaticFlashbackSystem? AudioFlashbacks { get; }
    }

    /// <summary>
    /// Subscribes to Core domain events and maps them to stable audio cue IDs.
    /// The bridge owns and releases its subscriptions; rebinding after a new
    /// campaign cannot leave handlers attached to stale host sessions.
    /// </summary>
    public sealed class AudioEventBridge : IDisposable
    {
        private readonly Action<string> _playCue;
        private readonly Action<string> _stopCue;
        private RadiationSystem? _radiation;
        private WeatherSystem? _weather;
        private TacticalCombatSystem? _combat;
        private CraftingSystem? _crafting;
        private ExpeditionSystem? _expeditions;
        private DiseaseSystem? _disease;
        private SurvivorFateSystem? _survivorFate;
        private SomaticFlashbackSystem? _flashbacks;
        private readonly Dictionary<string, float> _radiationDoseBySurvivor = new(StringComparer.Ordinal);
        private bool _disposed;

        public AudioEventBridge(AudioManager audio)
            : this(
                (audio ?? throw new ArgumentNullException(nameof(audio))).PlayCue,
                audio.StopCue)
        {
        }

        internal AudioEventBridge(Action<string> playCue, Action<string>? stopCue = null)
        {
            _playCue = playCue ?? throw new ArgumentNullException(nameof(playCue));
            _stopCue = stopCue ?? (_ => { });
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
            SurvivorFateSystem? survivorFate = null,
            SomaticFlashbackSystem? flashbacks = null)
        {
            ThrowIfDisposed();
            BindRadiation(radiation);
            BindWeather(weather);
            BindCombat(combat);
            BindCrafting(crafting);
            BindExpeditions(expeditions);
            BindDisease(disease);
            BindSurvivorFate(survivorFate);
            BindFlashbacks(flashbacks);
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
                "fire" => ResolveWeaponFireCue(evt.Detail),
                "suppress" => AudioCueCatalog.WeaponLmgBurst,
                "weapon_jam" => AudioCueCatalog.CombatJam,
                "jam_persist" => AudioCueCatalog.CombatDryFire,
                "weapon_burst" => AudioCueCatalog.CombatWeaponBurst,
                "reload" => evt.Detail.Contains("shotgun") ? AudioCueCatalog.WeaponShotgunRack : AudioCueCatalog.CombatReload,
                "clear_jam" => AudioCueCatalog.CombatReload,
                "downed" => AudioCueCatalog.HeavyImpactFall,
                "death" => AudioCueCatalog.CombatDowned,
                "mutual_kill" => AudioCueCatalog.CombatDowned,
                "victory" => AudioCueCatalog.CombatVictory,
                "defeat" => AudioCueCatalog.CombatDefeat,
                "trap" => AudioCueCatalog.CombatHit,
                "retreat_fail" => AudioCueCatalog.CombatHit,
                "enemy_fire" when evt.Detail.Contains("hits") => AudioCueCatalog.CombatHit,
                "repair" => AudioCueCatalog.ActionRepair,
                "bandage" => AudioCueCatalog.ActionInjection,
                "bleed" => AudioCueCatalog.TraumaHeartbeatRapid,
                "concussion" => AudioCueCatalog.TraumaTinnitus,
                "critical" => AudioCueCatalog.TraumaHeartbeatRapid,
                "last_stand" => AudioCueCatalog.CombatLastStand,
                "decon" => AudioCueCatalog.CombatDeconFlush,
                _ => null,
            };

            if (cueId != null)
                _playCue(cueId);

            if (evt.Kind == "fire")
            {
                _playCue(AudioCueCatalog.CombatCasingDrop);
                string lower = evt.Detail.ToLowerInvariant();
                if (lower.Contains("spear") || lower.Contains("rod"))
                    _playCue(AudioCueCatalog.CombatImprovisedSpear);
                else if (lower.Contains("molotov") || lower.Contains("burn"))
                    _playCue(AudioCueCatalog.CombatImprovisedFire);
                else if (lower.Contains("wood"))
                    _playCue(AudioCueCatalog.CombatImpactWood);
                else if (lower.Contains("armor") || lower.Contains("metal") || lower.Contains("stopped"))
                    _playCue(AudioCueCatalog.CombatImpactMetal);
                else if (lower.Contains("blocked") || lower.Contains("cover") || lower.Contains("concrete"))
                    _playCue(AudioCueCatalog.CombatImpactConcrete);
            }

            if (evt.Detail.Contains("Ricochet") || evt.Detail.Contains("ricochet"))
                _playCue(AudioCueCatalog.BulletWhizRicochet);
        }

        private static string ResolveWeaponFireCue(string detail)
        {
            if (string.IsNullOrEmpty(detail)) return AudioCueCatalog.CombatFire;
            string lower = detail.ToLowerInvariant();
            if (lower.Contains("cz75") || lower.Contains("pistol") || lower.Contains("9x19"))
                return AudioCueCatalog.WeaponCz75Report;
            if (lower.Contains("pipe") || lower.Contains("pipe_rifle"))
                return AudioCueCatalog.WeaponPipeRifleReport;
            if (lower.Contains("shotgun") || lower.Contains("scrap_shotgun"))
                return AudioCueCatalog.WeaponScrapShotgunReport;
            if (lower.Contains("bolt") || lower.Contains("bolt_rifle") || lower.Contains(".308"))
                return AudioCueCatalog.WeaponBoltRifleReport;
            if (lower.Contains("assault") || lower.Contains("assault_rifle") || lower.Contains("5.56"))
                return AudioCueCatalog.WeaponAssaultRifleBurst;
            if (lower.Contains("lmg") || lower.Contains("machine_gun") || lower.Contains("7.62"))
                return AudioCueCatalog.WeaponLmgBurst;
            if (lower.Contains("sniper") || lower.Contains(".50") || lower.Contains("anti_material"))
                return AudioCueCatalog.WeaponSniperHeavyReport;
            return AudioCueCatalog.CombatFire;
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
                _expeditions.OnCampEntered -= OnCampEntered;
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
                _expeditions.OnCampEntered += OnCampEntered;
            }
        }

        private void OnExpeditionStarted(ExpeditionState state)
        {
            _playCue(AudioCueCatalog.ShelterDoorOpen);
            if (state != null && !string.IsNullOrEmpty(state.vehicleId))
            {
                string cue = ResolveVehicleEngineCue(state.vehicleId);
                _playCue(cue);
            }
        }

        private static string ResolveVehicleEngineCue(string vehicleId)
        {
            if (string.IsNullOrEmpty(vehicleId)) return AudioCueCatalog.ExpeditionVehicleEngine;
            string lower = vehicleId.ToLowerInvariant();
            if (lower.Contains("bike") || lower.Contains("cycle"))
                return AudioCueCatalog.ExpeditionVehicleDirtBike;
            if (lower.Contains("truck") || lower.Contains("halftrack") || lower.Contains("base"))
                return AudioCueCatalog.ExpeditionVehicleTruck;
            return AudioCueCatalog.ExpeditionVehicleEngine;
        }

        private void StopAllVehicleLoops()
        {
            _stopCue(AudioCueCatalog.ExpeditionVehicleEngine);
            _stopCue(AudioCueCatalog.ExpeditionVehicleDirtBike);
            _stopCue(AudioCueCatalog.ExpeditionVehicleTruck);
        }

        private void OnEncounterTriggered(ExpeditionState state) => _playCue(AudioCueCatalog.CombatStart);

        private void OnVehicleBreakdown(ExpeditionState state)
        {
            StopAllVehicleLoops();
            _playCue(AudioCueCatalog.ExpeditionVehicleBreakdown);
        }

        private void OnExpeditionCompleted(ExpeditionState state)
        {
            StopAllVehicleLoops();
            _playCue(AudioCueCatalog.ShelterDoorSeal);
            _playCue(AudioCueCatalog.ActionItemPickup);
        }

        private void OnExpeditionFailed(ExpeditionState state, string reason)
        {
            StopAllVehicleLoops();
            _playCue(AudioCueCatalog.DangerDebris);
        }

        private void OnLootAdded(ExpeditionState state) => _playCue(AudioCueCatalog.ActionItemPickup);

        private void OnCampEntered(ExpeditionState state) => _playCue(AudioCueCatalog.ExpeditionCampFire);

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

        public void BindFlashbacks(SomaticFlashbackSystem? flashbacks)
        {
            ThrowIfDisposed();
            if (ReferenceEquals(_flashbacks, flashbacks))
                return;

            if (_flashbacks != null)
            {
                _flashbacks.OnFlashbackTriggered -= OnFlashbackTriggered;
                _flashbacks.OnFlashbackGrounded -= OnFlashbackGrounded;
            }

            _flashbacks = flashbacks;
            if (_flashbacks != null)
            {
                _flashbacks.OnFlashbackTriggered += OnFlashbackTriggered;
                _flashbacks.OnFlashbackGrounded += OnFlashbackGrounded;
            }
        }

        private void OnFlashbackTriggered(string survivorId, float durationHours)
        {
            _playCue(AudioCueCatalog.FlashbackTrigger);
            _playCue(AudioCueCatalog.TraumaTinnitus);
        }

        private void OnFlashbackGrounded(string survivorId, float orig, float red)
        {
            _playCue(AudioCueCatalog.FlashbackGrounded);
        }

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
                _expeditions.OnCampEntered -= OnCampEntered;
                StopAllVehicleLoops();
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
            if (_flashbacks != null)
            {
                _flashbacks.OnFlashbackTriggered -= OnFlashbackTriggered;
                _flashbacks.OnFlashbackGrounded -= OnFlashbackGrounded;
            }

            _radiation = null;
            _radiationDoseBySurvivor.Clear();
            _weather = null;
            _combat = null;
            _crafting = null;
            _expeditions = null;
            _disease = null;
            _survivorFate = null;
            _flashbacks = null;
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
        internal bool HasFlashbacksBinding => _flashbacks != null;
    }
}
