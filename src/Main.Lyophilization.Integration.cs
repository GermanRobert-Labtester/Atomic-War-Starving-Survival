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
        private LyophilizationHostSession? _lyophilization;

        private void SetupLyophilization()
        {
            if (_lyophilization != null) return;
            SetupInventory();
            SetupPowerGrid();
            EnsureMedicalPipeline();

            var rng = _campaignDay != null
                ? _campaignDay.Rng.Fork(CampaignStreamIds.Medical, 0, 26)
                : new SeededRng(132);
            var system = new LyophilizationSystem(
                _inventory.Inventory,
                rng,
                () => _powerGrid?.System.NetWatts ?? 0f,
                new GodotLog());
            system.LoadCatalog(LyophilizationCatalogLoader.Load(
                _dataDir, new FileSystemIO(), new SystemTextJsonSerializer(), new GodotLog()));
            var saved = LyophilizationSaveStore.TryLoad();
            if (saved != null) system.RestoreState(saved);
            _lyophilization = new LyophilizationHostSession(system);
            RegisterLyophilizationProtocols(system);
            system.OnBatchCompleted += batch =>
            {
                RegisterLyophilizedProtocol(system, batch.batch_id);
                _journal?.TryAddRawEntry(
                    "lyophilization_batch",
                    "A preserved biologic batch was sealed and entered the medical ledger.",
                    null!, _simDay);
            };
        }

        private void RegisterLyophilizationProtocols(LyophilizationSystem system)
        {
            if (_medical?.Pipeline == null) return;
            foreach (var batch in system.State.batches.Where(batch => batch != null && !batch.spoiled))
                RegisterLyophilizedProtocol(system, batch.batch_id);
        }

        private void RegisterLyophilizedProtocol(LyophilizationSystem system, string batchId)
        {
            var pipeline = _medical?.Pipeline;
            if (pipeline == null) return;
            system.RegisterMedicalProtocol(
                pipeline,
                $"protocol_lyophilization_{batchId}",
                batchId,
                1,
                () => _simDay);
        }

        private void SaveLyophilization()
            => CaptureIfPresent("lyophilization", _lyophilization?.System.CaptureState(),
                LyophilizationSaveStore.TryCapturePersisted);

    }
}
