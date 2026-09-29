// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Radio;
using Godot;
using System;
using System.Collections.Generic;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private HeliographHostSession? _heliograph;

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

        private void SaveHeliograph()
        {
            if (_heliograph != null)
                CaptureSection("heliograph",
                    HeliographSaveStore.TryCapturePersisted(_heliograph.System.CaptureState()));
        }

    }
}
