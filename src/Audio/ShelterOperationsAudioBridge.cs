// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core.Excavation;
using Ashfall.Core.Radio;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp.Audio
{
    /// <summary>
    /// Connects Plans 46–49 shelter operations (Workshop, Radio Station,
    /// Social Dynamics, Subterranean Excavation Hazards) to the diegetic
    /// soundscape without placing gameplay logic into presentation nodes.
    /// </summary>
    public sealed class ShelterOperationsAudioBridge : IDisposable
    {
        private readonly Action<string> _playCue;
        private readonly Action<string> _stopCue;
        private readonly Action<string, string> _startLoop;
        private readonly Action<string, string, float, float> _updateLoop;
        private readonly Action<string, string> _stopLoop;

        private ShelterWorkshopSystem? _workshop;
        private ShelterRadioStationSystem? _radio;
        private ShelterSocialDynamicsSystem? _social;
        private ExcavationHazardSystem? _excavation;

        private bool _disposed;

        public ShelterOperationsAudioBridge(AudioManager audio)
            : this(
                (audio ?? throw new ArgumentNullException(nameof(audio))).PlayCue,
                audio.StopCue,
                audio.StartLoop,
                audio.UpdateLoop,
                audio.StopLoop)
        {
        }

        public ShelterOperationsAudioBridge(
            Action<string> playCue,
            Action<string>? stopCue = null,
            Action<string, string>? startLoop = null,
            Action<string, string, float, float>? updateLoop = null,
            Action<string, string>? stopLoop = null)
        {
            _playCue = playCue ?? throw new ArgumentNullException(nameof(playCue));
            _stopCue = stopCue ?? (_ => { });
            _startLoop = startLoop ?? ((_, _) => { });
            _updateLoop = updateLoop ?? ((_, _, _, _) => { });
            _stopLoop = stopLoop ?? ((_, _) => { });
        }

        public void BindAll(
            ShelterWorkshopSystem? workshop = null,
            ShelterRadioStationSystem? radio = null,
            ShelterSocialDynamicsSystem? social = null,
            ExcavationHazardSystem? excavation = null)
        {
            ThrowIfDisposed();
            BindWorkshop(workshop);
            BindRadio(radio);
            BindSocial(social);
            BindExcavation(excavation);
        }

        public void BindWorkshop(ShelterWorkshopSystem? workshop)
        {
            ThrowIfDisposed();
            if (ReferenceEquals(_workshop, workshop)) return;

            if (_workshop != null)
            {
                _workshop.OnJobStarted -= OnWorkshopJobStarted;
                _workshop.OnJobCompleted -= OnWorkshopJobCompleted;
                _workshop.OnJobCancelled -= OnWorkshopJobCancelled;
                _workshop.OnMachineStateChanged -= OnMachineStateChanged;
            }

            _workshop = workshop;
            if (_workshop != null)
            {
                _workshop.OnJobStarted += OnWorkshopJobStarted;
                _workshop.OnJobCompleted += OnWorkshopJobCompleted;
                _workshop.OnJobCancelled += OnWorkshopJobCancelled;
                _workshop.OnMachineStateChanged += OnMachineStateChanged;
            }
        }

        public void BindRadio(ShelterRadioStationSystem? radio)
        {
            ThrowIfDisposed();
            if (ReferenceEquals(_radio, radio)) return;

            if (_radio != null)
            {
                _radio.OnInterceptDecrypted -= OnRadioInterceptDecrypted;
                _radio.OnLocationTriangulated -= OnRadioLocationTriangulated;
            }

            _radio = radio;
            if (_radio != null)
            {
                _radio.OnInterceptDecrypted += OnRadioInterceptDecrypted;
                _radio.OnLocationTriangulated += OnRadioLocationTriangulated;
            }
        }

        public void BindSocial(ShelterSocialDynamicsSystem? social)
        {
            ThrowIfDisposed();
            if (ReferenceEquals(_social, social)) return;

            if (_social != null)
            {
                _social.OnIncidentTriggered -= OnSocialIncidentTriggered;
                _social.OnIncidentMediated -= OnSocialIncidentMediated;
            }

            _social = social;
            if (_social != null)
            {
                _social.OnIncidentTriggered += OnSocialIncidentTriggered;
                _social.OnIncidentMediated += OnSocialIncidentMediated;
            }
        }

        public void BindExcavation(ExcavationHazardSystem? excavation)
        {
            ThrowIfDisposed();
            if (ReferenceEquals(_excavation, excavation)) return;

            if (_excavation != null)
            {
                _excavation.OnRescueStarted -= OnExcavationRescueStarted;
                _excavation.OnMethaneIgnition -= OnExcavationMethaneIgnition;
                _excavation.OnSectorFlooded -= OnExcavationSectorFlooded;
            }

            _excavation = excavation;
            if (_excavation != null)
            {
                _excavation.OnRescueStarted += OnExcavationRescueStarted;
                _excavation.OnMethaneIgnition += OnExcavationMethaneIgnition;
                _excavation.OnSectorFlooded += OnExcavationSectorFlooded;
            }
        }

        // ── Workshop Handlers ──────────────────────────────────────────

        private void OnWorkshopJobStarted(WorkshopJobState job)
        {
            if (job == null) return;
            if (_workshop != null && _workshop.Recipes.TryGetValue(job.RecipeId, out var recipe))
            {
                if (recipe.Kind == WorkshopJobKind.AmmunitionReload)
                {
                    _playCue(AudioCueCatalog.AmmoPressStamp);
                }
                else if (recipe.Kind == WorkshopJobKind.Fabrication || recipe.Kind == WorkshopJobKind.ToolOverhaul)
                {
                    _startLoop(AudioCueCatalog.WorkshopLatheLoop, $"workshop:{job.JobId}");
                }
            }
        }

        private void OnWorkshopJobCompleted(WorkshopJobState job)
        {
            if (job == null) return;
            _stopLoop(AudioCueCatalog.WorkshopLatheLoop, $"workshop:{job.JobId}");

            if (_workshop != null && _workshop.Recipes.TryGetValue(job.RecipeId, out var recipe))
            {
                if (recipe.Kind == WorkshopJobKind.WeaponService)
                {
                    _playCue(AudioCueCatalog.WeaponCleanClick);
                }
            }
        }

        private void OnWorkshopJobCancelled(WorkshopJobState job)
        {
            if (job == null) return;
            _stopLoop(AudioCueCatalog.WorkshopLatheLoop, $"workshop:{job.JobId}");
        }

        private void OnMachineStateChanged(WorkshopMachineState machine)
        {
            if (machine == null) return;
            if (machine.ToolingHealth >= 0.95f && machine.Calibration >= 0.95f)
            {
                _playCue(AudioCueCatalog.MachineOverhaulClank);
            }
        }

        // ── Radio Handlers ─────────────────────────────────────────────

        public void NotifyRadioFrequencyChanged(float frequencyKhz, float minKhz = 3000f, float maxKhz = 30000f)
        {
            ThrowIfDisposed();
            float norm = Math.Clamp((frequencyKhz - minKhz) / (maxKhz - minKhz), 0f, 1f);
            float pitch = 0.8f + (norm * 0.6f); // 0.8 to 1.4 bounded
            _updateLoop(AudioCueCatalog.RadioTuningHeterodyne, "radio:tuner", 0f, pitch);
        }

        private void OnRadioInterceptDecrypted(string interceptId)
        {
            _playCue(AudioCueCatalog.RadioDecryptedBeep);
        }

        private void OnRadioLocationTriangulated(string interceptId, string locationId)
        {
            _playCue(AudioCueCatalog.RadioSignalLock);
        }

        // ── Social Handlers ────────────────────────────────────────────

        private void OnSocialIncidentTriggered(SocialIncidentRecord incident)
        {
            _playCue(AudioCueCatalog.DisputeArgumentShout);
        }

        private void OnSocialIncidentMediated(SocialIncidentRecord incident)
        {
            _playCue(AudioCueCatalog.MediationAccordChime);
        }

        public void UpdateMessHallOccupancy(int activeOccupants)
        {
            ThrowIfDisposed();
            if (activeOccupants > 0)
            {
                _startLoop(AudioCueCatalog.MessHallChatterLoop, "social:mess_hall");
            }
            else
            {
                _stopLoop(AudioCueCatalog.MessHallChatterLoop, "social:mess_hall");
            }
        }

        // ── Excavation Hazard Handlers ─────────────────────────────────

        private void OnExcavationRescueStarted(string sectorId, int trappedCount)
        {
            _playCue(AudioCueCatalog.CaveInCollapseRumble);
        }

        private void OnExcavationMethaneIgnition(string sectorId)
        {
            _playCue(AudioCueCatalog.DangerAlarmKlaxon);
        }

        private void OnExcavationSectorFlooded(string sectorId)
        {
            _playCue(AudioCueCatalog.ShelterWaterDrip);
        }

        public void NotifyMethaneWarning(string sectorId, int ppm)
        {
            ThrowIfDisposed();
            if (ppm >= 2500)
            {
                _playCue(AudioCueCatalog.MethaneAlarmBeep);
            }
        }

        public void NotifyBulkheadToggled(string sectorId, bool sealedBulkhead)
        {
            ThrowIfDisposed();
            _playCue(AudioCueCatalog.BulkheadHydraulicSlam);
        }

        public void NotifyPumpStateChanged(string sectorId, bool running)
        {
            ThrowIfDisposed();
            if (running)
            {
                _startLoop(AudioCueCatalog.WaterPumpHumLoop, $"excavation:pump:{sectorId}");
            }
            else
            {
                _stopLoop(AudioCueCatalog.WaterPumpHumLoop, $"excavation:pump:{sectorId}");
            }
        }

        // ── Lifecycle ──────────────────────────────────────────────────

        private void ThrowIfDisposed()
        {
            if (_disposed) throw new ObjectDisposedException(nameof(ShelterOperationsAudioBridge));
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            BindWorkshop(null);
            BindRadio(null);
            BindSocial(null);
            BindExcavation(null);
        }
    }
}
