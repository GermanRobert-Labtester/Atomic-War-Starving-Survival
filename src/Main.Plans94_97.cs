using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Radio;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        private GrainProcessingHostSession? _grainProcessing;
        private CryogenicAirSeparationHostSession? _cryogenicAirSeparation;
        private HeliographHostSession? _heliograph;
        private UI.Plans94To97Panel? _plans94To97Panel;

        private void SetupGrainProcessing()
        {
            if (_grainProcessing != null) return;
            SetupInventory();

            var system = new GrainProcessingSystem(_inventory.Inventory, new GodotLog());
            system.LoadCatalog(GrainProcessingCatalogLoader.Load(
                _dataDir, new FileSystemIO(), new SystemTextJsonSerializer(), new GodotLog()));
            var saved = GrainProcessingSaveStore.TryLoad();
            if (saved != null) system.RestoreState(saved);
            _grainProcessing = new GrainProcessingHostSession(system);
        }

        private void SetupCryogenicAirSeparation()
        {
            if (_cryogenicAirSeparation != null) return;
            SetupInventory();
            SetupPowerGrid();

            var system = new CryogenicAirSeparationSystem(
                _inventory.Inventory,
                _campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.Shelter, 0, 17),
                () => _powerGrid?.System.NetWatts ?? 0f,
                new GodotLog());
            system.LoadCatalog(CryogenicAirSeparationCatalogLoader.Load(
                _dataDir, new FileSystemIO(), new SystemTextJsonSerializer(), new GodotLog()));
            var saved = CryogenicAirSeparationSaveStore.TryLoad();
            if (saved != null) system.RestoreState(saved);
            _cryogenicAirSeparation = new CryogenicAirSeparationHostSession(system);
        }

        private void SetupHeliograph()
        {
            if (_heliograph != null) return;
            SetupWorld();
            SetupRadio();

            var system = new HeliographSystem(
                hasLineOfSight: (originNode, targetNode) =>
                    _world.WastelandMap.IsDiscovered(originNode)
                    && _world.WastelandMap.IsDiscovered(targetNode),
                visibility01: ResolveHeliographVisibility,
                isMapNodeKnown: _world.WastelandMap.IsDiscovered,
                discoverMapNode: mapNodeId => { _world.WastelandMap.Discover(mapNodeId); },
                dispatchDistress: signalId => _radio.DistressSystem.DispatchExpedition(signalId));
            system.LoadCatalog(HeliographCatalogLoader.Load(
                _dataDir, new FileSystemIO(), new SystemTextJsonSerializer(), new GodotLog()));

            var saved = HeliographSaveStore.TryLoad();
            if (saved != null) system.RestoreState(saved);
            _heliograph = new HeliographHostSession(system);
            SetupPlans94To97Panel();
        }

        private void SetupPlans94To97Panel()
        {
            if (_plans94To97Panel == null)
            {
                _plans94To97Panel = new UI.Plans94To97Panel();
                AddChild(_plans94To97Panel);
                _plans94To97Panel.OnClose += () => _plans94To97Panel.Visible = false;
            }
            _plans94To97Panel.Bind(_grainProcessing!, _cryogenicAirSeparation!, _heliograph!);
        }

        private float ResolveHeliographVisibility()
        {
            if (_world?.Weather == null) return 1f;
            return _world.Weather.Current switch
            {
                WeatherKind.Blizzard => 0.2f,
                WeatherKind.FalloutStorm => 0.25f,
                WeatherKind.BlackRain => 0.3f,
                WeatherKind.ParticulateFog => 0.35f,
                WeatherKind.BioFog => 0.3f,
                WeatherKind.Ashfall => 0.65f,
                _ => 1f
            };
        }

        private void SaveGrainProcessing()
        {
            if (_grainProcessing != null)
                CaptureSection("grain_processing",
                    GrainProcessingSaveStore.TryCapturePersisted(_grainProcessing.System.CaptureState()));
        }

        private void SaveCryogenicAirSeparation()
        {
            if (_cryogenicAirSeparation != null)
                CaptureSection("cryogenic_air_separation",
                    CryogenicAirSeparationSaveStore.TryCapturePersisted(
                        _cryogenicAirSeparation.System.CaptureState()));
        }

        private void SaveHeliograph()
        {
            if (_heliograph != null)
                CaptureSection("heliograph",
                    HeliographSaveStore.TryCapturePersisted(_heliograph.System.CaptureState()));
        }

        private void TickPlans94To97(int day)
        {
            _grainProcessing?.TickDay(day);
            _cryogenicAirSeparation?.TickDay(day);
        }
    }
}
