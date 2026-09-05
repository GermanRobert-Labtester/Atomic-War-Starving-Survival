using System;
using System.Linq;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Foundry;
using Ashfall.Core.Medical;
using Ashfall.Core.Radio;
using Ashfall.Core.Random;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private PowderMetallurgyHostSession? _powderMetallurgy;
        private NvisCommunicationsHostSession? _nvisCommunications;
        private LyophilizationHostSession? _lyophilization;
        private DraisineRerailingHostSession? _draisineRerailing;
        private Plans130To133Panel? _plans130To133Panel;

        private void SetupPlans130To133()
        {
            SetupPowderMetallurgy();
            SetupNvisCommunications();
            SetupLyophilization();
            SetupDraisineRerailing();
            SetupPlans130To133Panel();
        }

        private void SetupPowderMetallurgy()
        {
            if (_powderMetallurgy != null) return;
            SetupInventory();
            SetupPowerGrid();

            var rng = _campaignDay != null
                ? _campaignDay.Rng.Fork(CampaignStreamIds.Foundry, 0, 24)
                : new SeededRng(130);
            var system = new PowderMetallurgySystem(
                _inventory.Inventory,
                rng,
                () => _powerGrid?.System.NetWatts ?? 0f,
                new GodotLog());
            system.LoadCatalog(PowderMetallurgyCatalogLoader.Load(
                _dataDir, new FileSystemIO(), new SystemTextJsonSerializer(), new GodotLog()));
            var saved = PowderMetallurgySaveStore.TryLoad();
            if (saved != null) system.RestoreState(saved);
            _powderMetallurgy = new PowderMetallurgyHostSession(system);
            system.OnBatchCompleted += batch =>
                _journal?.TryAddRawEntry(
                    "powder_metallurgy_batch",
                    $"The material press completed {batch.output_units} abstract material unit(s).",
                    null!, _simDay);
        }

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

        private void SetupDraisineRerailing()
        {
            if (_draisineRerailing != null) return;
            SetupInventory();
            SetupPowerGrid();
            SetupRailway();

            var rng = _campaignDay != null
                ? _campaignDay.Rng.Fork(CampaignStreamIds.Expedition, 0, 27)
                : new SeededRng(133);
            var system = new DraisineRerailingSystem(
                _inventory.Inventory,
                EnsureRailway(),
                rng,
                () => _powerGrid?.System.NetWatts ?? 0f,
                new GodotLog());
            system.LoadCatalog(RerailingEquipmentCatalogLoader.Load(
                _dataDir, new FileSystemIO(), new SystemTextJsonSerializer(), new GodotLog()));
            var saved = DraisineRerailingSaveStore.TryLoad();
            if (saved != null) system.RestoreState(saved);
            _draisineRerailing = new DraisineRerailingHostSession(system);
            system.OnRecoveryCompleted += state =>
                _journal?.TryAddRawEntry(
                    "draisine_rerailing",
                    $"Armored draisine {state.train_id} was returned to the rail.",
                    null!, _simDay);
        }

        private void SetupPlans130To133Panel()
        {
            if (_plans130To133Panel == null)
            {
                _plans130To133Panel = new Plans130To133Panel();
                AddChild(_plans130To133Panel);
                _plans130To133Panel.OnClose += () => _plans130To133Panel.Visible = false;
            }

            _plans130To133Panel.Bind(
                _powderMetallurgy!,
                _nvisCommunications!,
                _lyophilization!,
                _draisineRerailing!,
                EnsureRailway(),
                _expeditions,
                () => _simDay,
                AcknowledgeNvisRecall);
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

        private void SavePlans130To133()
        {
            SavePowderMetallurgy();
            SaveNvisCommunications();
            SaveLyophilization();
            SaveDraisineRerailing();
        }

        private void SavePowderMetallurgy()
            => CaptureIfPresent("powder_metallurgy", _powderMetallurgy?.System.CaptureState(),
                PowderMetallurgySaveStore.TryCapturePersisted);

        private void SaveNvisCommunications()
            => CaptureIfPresent("nvis_communications", _nvisCommunications?.System.CaptureState(),
                NvisCommunicationsSaveStore.TryCapturePersisted);

        private void SaveLyophilization()
            => CaptureIfPresent("lyophilization", _lyophilization?.System.CaptureState(),
                LyophilizationSaveStore.TryCapturePersisted);

        private void SaveDraisineRerailing()
            => CaptureIfPresent("draisine_recovery", _draisineRerailing?.System.CaptureState(),
                DraisineRerailingSaveStore.TryCapturePersisted);

        private void CaptureIfPresent<T>(
            string section,
            T? state,
            Func<T, string> capture)
            where T : class
        {
            if (state != null) CaptureSection(section, capture(state));
        }

        private void TickPlans130To133(int day)
        {
            _powderMetallurgy?.TickDay(day);
            _nvisCommunications?.TickDay(day);
            _lyophilization?.TickDay(day);
            _draisineRerailing?.TickDay(day);
        }

        private void ResetPlans130To133Panel()
        {
            _plans130To133Panel?.Unbind();
            if (_plans130To133Panel != null && _plans130To133Panel.IsInsideTree())
                RemoveChild(_plans130To133Panel);
            _plans130To133Panel = null;
        }

        private void OpenPlans130To133Panel()
        {
            SetupPlans130To133();
            _plans130To133Panel!.Open();
        }
    }
}
