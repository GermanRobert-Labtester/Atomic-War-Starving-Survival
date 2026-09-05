using System;
using System.Linq;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Survivors;
using Ashfall.Core.Subterranean;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Flagship XI — Plan 156 host wiring: generates the underground network from
    /// the campaign seed, registers underground zones as canonical expedition
    /// destinations, persists the network as the "subterranean" envelope section,
    /// and ticks bridge hazards from a phase-4 day owner after the expeditions tick.
    /// </summary>
    public partial class Main : Control
    {
        private SubterraneanHostSession? _subterranean;

        private void SetupSubterranean()
        {
            if (_subterranean != null) return;

            SetupExpeditions();
            SetupSurvivors();

            var catalog = SubterraneanZoneCatalogLoader.Load(_dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            var system = new SubterraneanSystem(catalog, _inventory.Inventory)
            {
                // Canonical weather read drives underground flood pressure.
                CurrentWeather = () => _world?.Weather?.Current ?? WeatherKind.Clear,
                // Low oxygen requests the canonical health consequence.
                ApplyHealthDelta = (id, delta) => _survivors?.Needs?.Modify(id, NeedKind.Health, delta)
            };

            _subterranean = new SubterraneanHostSession(system);
            _subterranean.ConfigureBridge(
                activeExpeditions: () => _expeditions.Engine.Active,
                applyMoraleDelta: (id, delta) => _survivors?.Needs?.Modify(id, NeedKind.Morale, delta),
                hasClaustrophobia: id => IsClaustrophobe(id));

            var save = SubterraneanSaveStore.TryLoad();
            if (save != null)
            {
                _subterranean.RestoreSave(save);
                GD.Print("[Ashfall Godot] Subterranean network restored.");
            }
            else
            {
                // Generation seed derives from the campaign/world seed; generated
                // once, then the persisted topology is authoritative.
                system.EnsureNetwork(CampaignSeedForGeneration());
            }

            // Underground zones become ordinary expedition destinations.
            _subterranean.RegisterUndergroundExpeditions();
        }

        private int CampaignSeedForGeneration()
        {
            int seed = _campaignDay?.Rng?.MasterSeed ?? 0;
            if (seed == 0) seed = 20260905;
            return seed;
        }

        private bool IsClaustrophobe(string survivorId)
        {
            var entry = _survivors?.Roster?.Roster?.FirstOrDefault(e =>
                e != null && string.Equals(e.survivorId, survivorId, StringComparison.Ordinal));
            if (entry == null) return false;
            var def = _survivors.Roster.FindDefinition(entry.definitionId);
            return def?.traitIds?.Contains(SubterraneanSystem.ClaustrophobiaTraitId) ?? false;
        }

        // ------------------------------------------------------------- commands

        public string ShoreSubterraneanNode(string nodeId)
        {
            SetupSubterranean();
            if (_subterranean == null) return "The under-map is unavailable.";
            return _subterranean.ShoreNode(nodeId);
        }

        public string VentilateSubterraneanNode(string nodeId)
        {
            SetupSubterranean();
            if (_subterranean == null) return "The under-map is unavailable.";
            return _subterranean.InstallVentilation(nodeId);
        }

        public string ClearSubterraneanBlockage(string nodeId)
        {
            SetupSubterranean();
            if (_subterranean == null) return "The under-map is unavailable.";
            return _subterranean.ClearBlockage(nodeId);
        }

        private void SaveSubterranean()
        {
            if (_subterranean == null) return;
            CaptureSection("subterranean",
                SubterraneanSaveStore.TryCapturePersisted(_subterranean.CaptureSave()));
        }
    }
}
