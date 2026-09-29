// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Excavation;
using Ashfall.Core.IO;
using Ashfall.Core.Quests;
using Ashfall.Core.Radio;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;
using Godot;
using System;
using System.Collections.Generic;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private ShelterRadioStationSystem? _radioStationSystem;
        private bool _radioStationDirty;

        // ── Plan 47: Wasteland Radio Intelligence ───────────────────────

        public ShelterRadioStationSystem EnsureRadioStation()
        {
            if (_radioStationSystem != null) return _radioStationSystem;

            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("radio_station") : new SeededRng(47);
            // Bind the canonical orbital harrow telemetry at construction — the
            // field is readonly and null leaves impact-warning radio cues dead.
            _radioStationSystem = new ShelterRadioStationSystem(rng, EnsureOrbitalHarrowTelemetry(), new GodotLog());

            // Plan B67: radio detection consumes the canonical weather
            // authority — atmospheric interference rises with storm severity.
            // No radio-only weather state is created.
            var weather = _world?.Weather;
            if (weather != null)
            {
                _radioStationSystem.BindWeatherNoiseProvider(() => WeatherNoiseForKind(weather.Current));
            }

            string catalogPath = CatalogPath.ResolveCatalog("radio_intercepts.json");
            var _catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (_catalogIo.FileExists(catalogPath))
            {
                string json = _catalogIo.ReadAllText(catalogPath);
                {
                    _radioStationSystem.LoadCatalog(json);
                }
            }

            var saved = RadioStationSaveStore.TryLoad();
            if (saved != null)
            {
                _radioStationSystem.RestoreState(saved);
            }

            _radioStationSystem.OnLocationTriangulated += (interceptId, locationId) =>
            {
                _journal?.TryAddRawEntry("radio_triangulation", $"Triangulated coordinates for {locationId} via intercept {interceptId}", null!, _simDay);
                _world?.WastelandMap?.Discover(locationId);
                EnsureDynamicQuests().TriggerInvestigateRadioDepotQuest(interceptId, locationId, _simDay);
            };

            _radioStationSystem.OnRadioStateChanged += () => _radioStationDirty = true;
            return _radioStationSystem;
        }

        private void SetupRadioStation()
        {
            EnsureRadioStation();
        }

        /// <summary>
        /// <summary>
        /// Plan B67 — map the canonical weather authority onto radio
        /// atmospheric noise. The attenuation itself is the propagation
        /// engine's authority (<see cref="RadioPropagationEngine.GetWeatherAttenuation"/>);
        /// the weather state stays canonical in the world owner, and only the
        /// noise fraction it produces is composed here.
        /// </summary>
        private static float WeatherNoiseForKind(WeatherKind kind)
        {
            // Engine authority: 1.0 = pristine propagation, 0.15 = worst ducting loss.
            float attenuation = RadioPropagationEngine.GetWeatherAttenuation(kind);
            const float worstAttenuation = 0.15f;
            float noise = 1.0f - attenuation;
            // Normalise against the engine's own worst case so the band stays
            // inside the legacy 0.05..0.45 noise range used by the station.
            float normalised = noise / (1.0f - worstAttenuation);
            return Math.Clamp(0.05f + normalised * 0.40f, 0.05f, 0.45f);
        }

        private void SaveRadioStation()
        {
            if (_radioStationSystem != null)
            {
                CaptureSection("radio_station", RadioStationSaveStore.TryCapturePersisted(_radioStationSystem.CaptureState()));
                _radioStationDirty = false;
            }
        }

    }
}
