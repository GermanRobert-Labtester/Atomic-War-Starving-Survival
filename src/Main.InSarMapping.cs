// SPDX-License-Identifier: MIT
// ============================================================================
// Main Partial : Plan 139 Phase 2 — InSAR deformation-intelligence host wire
// Subsystems   : catalog load, survey passes, repeat-pass processing,
//                weather/terrain decorrelation inputs, travel/excavation
//                projections, dedicated save section. Intelligence only —
//                never mutates world terrain.
// ============================================================================
using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Random;
using Ashfall.Core.World;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private InSarMappingHostSession? _inSarMapping;
        private InSarMappingPanel? _inSarPanel;

        /// <summary>Panel-facing session (Plan 139 Phase 3).</summary>
        public InSarMappingHostSession EnsureInSarMappingSession()
        {
            SetupInSarMapping();
            return _inSarMapping!;
        }

        private void SetupInSarMapping()
        {
            if (_inSarMapping != null) return;

            var catalog = InSarGeodesyCatalogLoader.Load(_dataDir, new FileSystemIO());
            var system = new InSarDeformationEngine(catalog, new GodotLog());

            // Forked campaign stream — processing jitter is deterministic per
            // seed and cannot shift any other stream.
            if (_campaignDay?.Rng != null)
                system.Rng = _campaignDay.Rng.Fork(CampaignStreamIds.InSarDeformation);

            _inSarMapping = new InSarMappingHostSession(system)
            {
                WeatherQualityBpProvider = ResolveInSarWeatherQuality,
                TerrainTagsProvider = ResolveInSarTerrainTags,
                ObservationQualityProvider = (_, sector, day) => InSarObservationQuality(sector, day)
            };

            var saved = InSarMappingSaveStore.TryLoad();
            if (saved != null)
            {
                _inSarMapping.RestoreSave(saved);
                GD.Print("[Ashfall Godot] InSAR deformation intelligence restored.");
            }

            // Panel — created hidden; opened via the expanded-panel route.
            if (_inSarPanel != null && _inSarPanel.IsInsideTree())
                RemoveChild(_inSarPanel);
            _inSarPanel = new InSarMappingPanel
            {
                AppDayProvider = () => _simDay,
                SectorProvider = InSarSurveySectors
            };
            _inSarPanel.Bind(_inSarMapping);
            _inSarPanel.Visible = false;
            AddChild(_inSarPanel);
        }

        /// <summary>
        /// Known survey sectors: discovered map nodes when the player has any,
        /// otherwise the authored node list. Sorted for stable UI ordering.
        /// </summary>
        private IReadOnlyList<string> InSarSurveySectors()
        {
            var list = new List<string>();
            var map = _world?.WastelandMap;
            if (map == null) return list;

            var discovered = map.DiscoveredNodes;
            if (discovered != null && discovered.Count > 0)
            {
                foreach (var id in discovered)
                    if (!string.IsNullOrEmpty(id)) list.Add(id);
            }
            else
            {
                foreach (var node in map.Nodes)
                    if (node != null && !string.IsNullOrEmpty(node.Id)) list.Add(node.Id);
            }

            list.Sort(StringComparer.Ordinal);
            return list;
        }

        /// <summary>
        /// Live weather quality (0..100) from the single weather authority.
        /// Clear sky is 100; storms decorrelate a pass heavily.
        /// </summary>
        private int ResolveInSarWeatherQuality()
        {
            var weather = _world?.Weather;
            if (weather == null) return 100;
            return WeatherQualityForKind(weather.Current);
        }

        internal static int WeatherQualityForKind(WeatherKind kind) => kind switch
        {
            WeatherKind.Clear => 100,
            WeatherKind.Overcast => 85,
            WeatherKind.Rain => 72,
            WeatherKind.AlgaeBloom => 65,
            WeatherKind.ThermalInversion => 62,
            WeatherKind.ParticulateFog => 60,
            WeatherKind.BioFog => 58,
            WeatherKind.Ashfall => 52,
            WeatherKind.AcidSnow => 48,
            WeatherKind.BlackSnow => 46,
            WeatherKind.BlackRain => 44,
            WeatherKind.BloodRain => 44,
            WeatherKind.AshLightning => 42,
            WeatherKind.IceStorm => 34,
            WeatherKind.FalloutStorm => 30,
            WeatherKind.Blizzard => 28,
            WeatherKind.RadHail => 26,
            WeatherKind.GlassStorm => 24,
            WeatherKind.EMPStorm => 20,
            _ => 80
        };

        /// <summary>
        /// Terrain decorrelation tags for a sector. High-danger built-up ruins
        /// are treated as urban rubble; unknown sectors carry no tag.
        /// </summary>
        private IReadOnlyCollection<string> ResolveInSarTerrainTags(string sectorId)
        {
            var map = _world?.WastelandMap;
            var node = map?.GetNode(sectorId);
            if (node != null && node.Danger == MapNodeDanger.High)
                return new[] { "urban_rubble" };
            return Array.Empty<string>();
        }

        /// <summary>
        /// Deterministic authored phase/backscatter read for a sector and day.
        /// The Core engine converts the swing between reads into displacement;
        /// this host value only supplies the observation, never geology truth.
        /// FNV-1a keeps it stable and independent of RNG stream consumption.
        /// </summary>
        internal static int InSarObservationQuality(string sectorId, int day)
        {
            if (string.IsNullOrEmpty(sectorId)) return 50;
            int baseQ = 30 + (int)(StableHash(sectorId) % 40);            // 30..69
            int drift = (int)(StableHash(sectorId + "_drift") % 21) - 10; // -10..10 per 10 days
            int q = baseQ + (day * drift) / 10;
            return Math.Max(0, Math.Min(100, q));
        }

        /// <summary>FNV-1a over the string — deterministic across runs and platforms.</summary>
        private static uint StableHash(string value)
        {
            uint h = 2166136261u;
            foreach (char c in value)
            {
                h ^= c;
                h *= 16777619u;
            }
            return h;
        }

        public string RecordInSarSurveyPass(string sensorProfileId, string sectorId)
        {
            SetupInSarMapping();
            if (_inSarMapping == null) return "InSAR mapping is unavailable.";
            return _inSarMapping.RecordSurveyPass(sensorProfileId, sectorId, _simDay);
        }

        public string ProcessInSarSector(string sectorId)
        {
            SetupInSarMapping();
            if (_inSarMapping == null) return "InSAR mapping is unavailable.";
            return _inSarMapping.ProcessSector(sectorId, processingSkill: 0.5);
        }

        private void SaveInSarMapping()
        {
            if (_inSarMapping == null) return;
            CaptureSection(
                InSarMappingSaveStore.SectionName,
                InSarMappingSaveStore.TryCapturePersisted(_inSarMapping.CaptureSave()));
        }
    }
}
