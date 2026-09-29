// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Foundry;
using Ashfall.Core.Medical;
using Ashfall.Core.Radio;
using Ashfall.Core.Random;
using AtomicWar.GodotApp.UI;
using Godot;
using System;
using System.Linq;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private NvisCommunicationsHostSession? _nvisCommunications;

        private void SetupNvisCommunications()
        {
            if (_nvisCommunications != null) return;
            SetupPowerGrid();
            SetupRadio();

            var rng = _campaignDay != null
                ? _campaignDay.Rng.Fork(CampaignStreamIds.Radio, 0, 25)
                : new SeededRng(131);
            var system = new NvisCommunicationsSystem(
                rng,
                () => _powerGrid?.System.NetWatts ?? 0f,
                new GodotLog());
            system.LoadCatalog(NvisCommunicationsCatalogLoader.Load(
                _dataDir, new FileSystemIO(), new SystemTextJsonSerializer(), new GodotLog()));
            var saved = NvisCommunicationsSaveStore.TryLoad();
            if (saved != null) system.RestoreState(saved);
            _nvisCommunications = new NvisCommunicationsHostSession(system);
            system.OnRecallRequested += request =>
                _journal?.TryAddRawEntry(
                    "nvis_recall_request",
                    $"Regional communications queued a recall request for {request.survivor_id}.",
                    null!, _simDay);
        }

        private void AcknowledgeNvisRecall(string survivorId)
        {
            if (_nvisCommunications == null) return;
            bool retreated = _expeditions?.Engine?.Retreat(survivorId) ?? false;
            bool acknowledged = _nvisCommunications.System.AcknowledgeRecall(
                survivorId,
                retreated ? "retreated" : "survivor_not_in_field");
            if (acknowledged && retreated)
                _journal?.TryAddRawEntry(
                    "nvis_recall_acknowledged",
                    $"The expedition authority accepted the recall for {survivorId}.",
                    null!, _simDay);
        }

        private void SaveNvisCommunications()
            => CaptureIfPresent("nvis_communications", _nvisCommunications?.System.CaptureState(),
                NvisCommunicationsSaveStore.TryCapturePersisted);

    }
}
