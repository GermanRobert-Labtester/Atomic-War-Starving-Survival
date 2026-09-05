// SPDX-License-Identifier: MIT
// ============================================================================
// Main Partial : Advanced Shelter & Flagship Systems Host Wire & Orchestration
// Subsystems   : Caravan Trade Network, Surgical Ward, Power Subgrids,
//                Perimeter Defense, Hydroponic Biomes, Nuclear Core Lifecycle,
//                Armored Crawlers
// ============================================================================
using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.Defense;
using Ashfall.Core.Economy;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Medical;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private CaravanTradeNetworkSystem? _caravanTradeNetwork;
        private AdvancedSurgicalWardSystem? _surgicalWard;
        private PowerDistributionSubgridSystem? _powerSubgrids;
        private PerimeterDefenseSystem? _perimeterDefense;
        private HydroponicBiomeSystem? _hydroponicBiomes;
        private NuclearCoreLifecycleSystem? _nuclearCore;
        private ArmoredCrawlerExpeditionSystem? _armoredCrawlers;

        private bool _caravanTradeNetworkDirty;
        private bool _surgicalWardDirty;
        private bool _powerSubgridsDirty;
        private bool _perimeterDefenseDirty;
        private bool _hydroponicBiomesDirty;
        private bool _nuclearCoreDirty;
        private bool _armoredCrawlersDirty;

        // ── Plan 85: Caravan Trade Network ──────────────────────────────

        public CaravanTradeNetworkSystem EnsureCaravanTrade()
        {
            if (_caravanTradeNetwork != null) return _caravanTradeNetwork;
            SetupInventory();
            var fileIO = CatalogPath.CreateFileIOForDataDir(_dataDir);
            var json = new SystemTextJsonSerializer();
            var routes = CaravanTradeRouteCatalogLoader.Load(_dataDir, fileIO, json);
            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("caravan_trade") : new SeededRng(85);
            _caravanTradeNetwork = new CaravanTradeNetworkSystem(
                routes,
                _inventory?.Inventory ?? new Ashfall.Core.Inventory.Inventory(),
                rng,
                new GodotLog());

            var saved = CaravanTradeSaveStore.TryLoad();
            if (saved != null)
            {
                _caravanTradeNetwork.RestoreState(saved);
            }

            _caravanTradeNetwork.OnTradeCompleted += (faction, offered, requested) =>
            {
                _journal?.TryAddRawEntry("caravan_trade_completed", $"Trade completed with {faction}: {offered:F0} value offered, {requested:F0} value received.", null!, _simDay);
                _caravanTradeNetworkDirty = true;
            };
            _caravanTradeNetwork.OnCaravanScheduled += _ => _caravanTradeNetworkDirty = true;
            _caravanTradeNetwork.OnCaravanArrived += _ => _caravanTradeNetworkDirty = true;
            _caravanTradeNetwork.OnCaravanDeparted += _ => _caravanTradeNetworkDirty = true;
            _caravanTradeNetwork.OnCaravanHazardResolved += (_, _) => _caravanTradeNetworkDirty = true;
            _caravanTradeNetwork.OnFavoredBarterStatusUnlocked += faction =>
            {
                _journal?.TryAddRawEntry("caravan_favored_status", $"Favored barter status achieved with {faction} (-15% tariffs)!", null!, _simDay);
                _caravanTradeNetworkDirty = true;
            };

            return _caravanTradeNetwork;
        }

        private void SetupCaravanTrade()
        {
            EnsureCaravanTrade();
        }

        private void SaveCaravanTrade()
        {
            if (_caravanTradeNetwork != null)
            {
                CaptureSection("caravan_trade_network", CaravanTradeSaveStore.TryCapturePersisted(_caravanTradeNetwork.CaptureState()));
                _caravanTradeNetworkDirty = false;
            }
        }

        // ── Plan 86: Advanced Surgical Ward ─────────────────────────────

        public AdvancedSurgicalWardSystem EnsureSurgicalWard()
        {
            if (_surgicalWard != null) return _surgicalWard;
            SetupInventory();
            var fileIO = CatalogPath.CreateFileIOForDataDir(_dataDir);
            var json = new SystemTextJsonSerializer();
            var procedures = SurgicalProcedureCatalogLoader.Load(_dataDir, fileIO, json);
            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("surgical_ward") : new SeededRng(86);
            _surgicalWard = new AdvancedSurgicalWardSystem(
                procedures,
                _inventory?.Inventory ?? new Ashfall.Core.Inventory.Inventory(),
                rng,
                new GodotLog());

            var saved = SurgicalWardSaveStore.TryLoad();
            if (saved != null)
            {
                _surgicalWard.RestoreState(saved);
            }

            _surgicalWard.OnOperationStarted += op =>
            {
                _journal?.TryAddRawEntry("surgical_operation_started", $"Surgery '{op.procedure_id}' commenced for patient {op.patient_id}.", null!, _simDay);
                _surgicalWardDirty = true;
            };
            _surgicalWard.OnOperationCompleted += (op, survived) =>
            {
                string outcome = survived ? "successful" : "fatal complication";
                _journal?.TryAddRawEntry("surgical_operation_completed", $"Surgery '{op.procedure_id}' for {op.patient_id} concluded ({outcome}).", null!, _simDay);
                _surgicalWardDirty = true;
            };
            _surgicalWard.OnComplicationEncountered += (op, milestone, detail) =>
            {
                _journal?.TryAddRawEntry("surgical_complication", $"Surgical complication at {milestone} for {op.patient_id}: {detail}.", null!, _simDay);
                _surgicalWardDirty = true;
            };
            _surgicalWard.OnPatientDischarged += _ => _surgicalWardDirty = true;
            _surgicalWard.OnSterileFieldChanged += _ => _surgicalWardDirty = true;

            return _surgicalWard;
        }

        private void SetupSurgicalWard()
        {
            EnsureSurgicalWard();
        }

        private void SaveSurgicalWard()
        {
            if (_surgicalWard != null)
            {
                CaptureSection("surgical_ward", SurgicalWardSaveStore.TryCapturePersisted(_surgicalWard.CaptureState()));
                _surgicalWardDirty = false;
            }
        }

        // ── Plan 87: Power Distribution Sub-Grids ───────────────────────

        public PowerDistributionSubgridSystem EnsurePowerSubgrids()
        {
            if (_powerSubgrids != null) return _powerSubgrids;
            SetupInventory();
            var fileIO = CatalogPath.CreateFileIOForDataDir(_dataDir);
            var json = new SystemTextJsonSerializer();
            var nodes = PowerSubgridCatalogLoader.Load(_dataDir, fileIO, json);
            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("power_subgrids") : new SeededRng(87);
            _powerSubgrids = new PowerDistributionSubgridSystem(
                nodes,
                _inventory?.Inventory ?? new Ashfall.Core.Inventory.Inventory(),
                rng,
                new GodotLog());

            var saved = PowerDistributionSaveStore.TryLoad();
            if (saved != null)
            {
                _powerSubgrids.RestoreState(saved);
            }

            _powerSubgrids.OnNodeThermalWarning += (nodeId, temp) =>
            {
                _journal?.TryAddRawEntry("subgrid_thermal_warning", $"WARNING: Subgrid transformer node {nodeId} reached {temp:F1}°C!", null!, _simDay);
                _powerSubgridsDirty = true;
            };
            _powerSubgrids.OnNodeFuseBlown += nodeId =>
            {
                _journal?.TryAddRawEntry("subgrid_fuse_blown", $"ALERT: Subgrid fuse blown on node {nodeId} due to overcurrent!", null!, _simDay);
                _powerSubgridsDirty = true;
            };
            _powerSubgrids.OnSubgridArcFlash += (nodeId, roomId) =>
            {
                _journal?.TryAddRawEntry("subgrid_arc_flash", $"CRITICAL: Arc flash event at node {nodeId} in room {roomId}!", null!, _simDay);
                _powerSubgridsDirty = true;
            };
            _powerSubgrids.OnTransformerMaintained += _ => _powerSubgridsDirty = true;

            return _powerSubgrids;
        }

        private void SetupPowerSubgrids()
        {
            EnsurePowerSubgrids();
        }

        private void SavePowerSubgrids()
        {
            if (_powerSubgrids != null)
            {
                CaptureSection("power_subgrids", PowerDistributionSaveStore.TryCapturePersisted(_powerSubgrids.CaptureState()));
                _powerSubgridsDirty = false;
            }
        }

        // ── Plan 88: Surface Perimeter Defense ──────────────────────────

        public PerimeterDefenseSystem EnsurePerimeterDefense()
        {
            if (_perimeterDefense != null) return _perimeterDefense;
            SetupInventory();
            var fileIO = CatalogPath.CreateFileIOForDataDir(_dataDir);
            var json = new SystemTextJsonSerializer();
            var defs = PerimeterDefenseCatalogLoader.Load(_dataDir, fileIO, json);
            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("perimeter_defense") : new SeededRng(88);
            _perimeterDefense = new PerimeterDefenseSystem(
                defs,
                _inventory?.Inventory ?? new Ashfall.Core.Inventory.Inventory(),
                rng,
                new GodotLog());

            var saved = PerimeterDefenseSaveStore.TryLoad();
            if (saved != null)
            {
                _perimeterDefense.RestoreState(saved);
            }

            _perimeterDefense.OnEmplacementConstructed += emp =>
            {
                _journal?.TryAddRawEntry("defense_constructed", $"Surface defense emplacement {emp.defense_id} constructed.", null!, _simDay);
                _perimeterDefenseDirty = true;
            };
            _perimeterDefense.OnTurretJammed += emp =>
            {
                _journal?.TryAddRawEntry("defense_turret_jammed", $"WARNING: Automated turret {emp.defense_id} has jammed!", null!, _simDay);
                _perimeterDefenseDirty = true;
            };
            _perimeterDefense.OnEmplacementDestroyed += emp =>
            {
                _journal?.TryAddRawEntry("defense_emplacement_destroyed", $"CRITICAL: Defense emplacement {emp.defense_id} was destroyed!", null!, _simDay);
                _perimeterDefenseDirty = true;
            };
            _perimeterDefense.OnAssaultRepelled += res =>
            {
                _journal?.TryAddRawEntry("defense_assault_repelled", $"Surface assault repelled: {res.AttackersKilled} attackers eliminated.", null!, _simDay);
                _perimeterDefenseDirty = true;
            };
            _perimeterDefense.OnPerimeterBreached += res =>
            {
                _journal?.TryAddRawEntry("defense_perimeter_breached", $"BREACH: Raider assault broke through surface defenses ({res.AttackersBreached} penetrated)!", null!, _simDay);
                _perimeterDefenseDirty = true;
            };
            _perimeterDefense.OnAmmoLoaded += (_, _) => _perimeterDefenseDirty = true;

            return _perimeterDefense;
        }

        private void SetupPerimeterDefense()
        {
            EnsurePerimeterDefense();
        }

        private void SavePerimeterDefense()
        {
            if (_perimeterDefense != null)
            {
                CaptureSection("perimeter_defense", PerimeterDefenseSaveStore.TryCapturePersisted(_perimeterDefense.CaptureState()));
                _perimeterDefenseDirty = false;
            }
        }

        // ── Plan 104: Hydroponic Biomes ─────────────────────────────────

        public HydroponicBiomeSystem EnsureHydroponicBiomes()
        {
            if (_hydroponicBiomes != null) return _hydroponicBiomes;
            SetupInventory();
            var fileIO = CatalogPath.CreateFileIOForDataDir(_dataDir);
            var json = new SystemTextJsonSerializer();
            var cat = HydroponicCropCatalogLoader.Load(_dataDir, fileIO, json)
                ?? new HydroponicCropCatalog(null);
            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("hydroponic_biomes") : new SeededRng(104);
            _hydroponicBiomes = new HydroponicBiomeSystem(
                _inventory?.Inventory ?? new Ashfall.Core.Inventory.Inventory(),
                cat,
                rng,
                new GodotLog(),
                isGridPowered: () => _powerGrid?.System != null && !_powerGrid.System.IsBrownout,
                waterConsume: amount =>
                {
                    var inv = _inventory?.Inventory;
                    int needed = (int)Math.Ceiling(amount);
                    if (inv != null && inv.CountById("clean_water") >= needed)
                    {
                        inv.RemoveById("clean_water", needed);
                        return true;
                    }
                    return false;
                });

            var saved = HydroponicBiomeSaveStore.TryLoad();
            if (saved != null)
            {
                _hydroponicBiomes.RestoreState(saved);
            }

            _hydroponicBiomes.OnCropPlanted += (rack, crop) =>
            {
                _journal?.TryAddRawEntry("hydroponic_crop_planted", $"Planted {crop} in hydroponic {rack}.", null!, _simDay);
                _hydroponicBiomesDirty = true;
            };
            _hydroponicBiomes.OnCropHarvested += (rack, crop, yield) =>
            {
                _journal?.TryAddRawEntry("hydroponic_crop_harvested", $"Harvested {yield}x {crop} from hydroponic {rack}.", null!, _simDay);
                _hydroponicBiomesDirty = true;
            };
            _hydroponicBiomes.OnCropMutated += (rack, trait) =>
            {
                _journal?.TryAddRawEntry("hydroponic_crop_mutated", $"Hydroponic mutation in {rack}: expressed trait '{trait}'.", null!, _simDay);
                _hydroponicBiomesDirty = true;
            };
            _hydroponicBiomes.OnCropDied += rack =>
            {
                _journal?.TryAddRawEntry("hydroponic_crop_died", $"Crop loss: plant died in hydroponic {rack}.", null!, _simDay);
                _hydroponicBiomesDirty = true;
            };

            return _hydroponicBiomes;
        }

        private void SetupHydroponicBiomes()
        {
            EnsureHydroponicBiomes();
        }

        private void SaveHydroponicBiomes()
        {
            if (_hydroponicBiomes != null)
            {
                CaptureSection("hydroponic_biomes", HydroponicBiomeSaveStore.TryCapturePersisted(_hydroponicBiomes.CaptureState()));
                _hydroponicBiomesDirty = false;
            }
        }

        // ── Plan 106: Nuclear Core Lifecycle ────────────────────────────

        public NuclearCoreLifecycleSystem EnsureNuclearCore()
        {
            if (_nuclearCore != null) return _nuclearCore;
            SetupInventory();
            var fileIO = CatalogPath.CreateFileIOForDataDir(_dataDir);
            var json = new SystemTextJsonSerializer();
            var cat = NuclearCoreCatalogLoader.Load(_dataDir, fileIO, json)
                ?? new NuclearCoreCatalog(null);
            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("nuclear_core_lifecycle") : new SeededRng(106);
            _nuclearCore = new NuclearCoreLifecycleSystem(
                _inventory?.Inventory ?? new Ashfall.Core.Inventory.Inventory(),
                cat,
                rng,
                new GodotLog(),
                coolantProvider: amount =>
                {
                    var inv = _inventory?.Inventory;
                    int needed = (int)Math.Ceiling(amount);
                    if (inv != null && inv.CountById("clean_water") >= needed)
                    {
                        inv.RemoveById("clean_water", needed);
                        return true;
                    }
                    return false;
                },
                onRadiationLeakage: (roomId, dose) =>
                {
                    _journal?.TryAddRawEntry("nuclear_radiation_leak", $"WARNING: Radiation leak in {roomId} ({dose:F1} rads)!", null!, _simDay);
                });

            var saved = NuclearCoreSaveStore.TryLoad();
            if (saved != null)
            {
                _nuclearCore.RestoreState(saved);
            }

            _nuclearCore.OnCoreInstalled += (instanceId, profileId) =>
            {
                _journal?.TryAddRawEntry("nuclear_core_installed", $"Reactor core {instanceId} ({profileId}) successfully installed and seated.", null!, _simDay);
                _nuclearCoreDirty = true;
            };
            _nuclearCore.OnReactorScrammed += instanceId =>
            {
                _journal?.TryAddRawEntry("nuclear_reactor_scram", $"EMERGENCY SCRAM: Reactor core {instanceId} control rods dropped to safe cold state!", null!, _simDay);
                _nuclearCoreDirty = true;
            };
            _nuclearCore.OnHeatStateChanged += (instanceId, state) =>
            {
                _journal?.TryAddRawEntry("nuclear_heat_warning", $"Thermal alert: Core {instanceId} thermal state transitioned to '{state}'.", null!, _simDay);
                _nuclearCoreDirty = true;
            };
            _nuclearCore.OnRadiationLeak += (instanceId, dose) =>
            {
                _nuclearCoreDirty = true;
            };

            return _nuclearCore;
        }

        private void SetupNuclearCore()
        {
            EnsureNuclearCore();
        }

        private void SaveNuclearCore()
        {
            if (_nuclearCore != null)
            {
                CaptureSection("nuclear_core_lifecycle", NuclearCoreSaveStore.TryCapturePersisted(_nuclearCore.CaptureState()));
                _nuclearCoreDirty = false;
            }
        }

        // ── Plan 107: Armored Crawlers ──────────────────────────────────

        public ArmoredCrawlerExpeditionSystem EnsureArmoredCrawlers()
        {
            if (_armoredCrawlers != null) return _armoredCrawlers;
            SetupInventory();
            var fileIO = CatalogPath.CreateFileIOForDataDir(_dataDir);
            var json = new SystemTextJsonSerializer();
            var cat = ArmoredCrawlerModuleCatalogLoader.Load(_dataDir, fileIO, json)
                ?? new ArmoredCrawlerModuleCatalog(null);
            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("armored_crawlers") : new SeededRng(107);
            _armoredCrawlers = new ArmoredCrawlerExpeditionSystem(
                _inventory?.Inventory ?? new Ashfall.Core.Inventory.Inventory(),
                cat,
                rng,
                new GodotLog());

            var saved = ArmoredCrawlerSaveStore.TryLoad();
            if (saved != null)
            {
                _armoredCrawlers.RestoreState(saved);
            }

            _armoredCrawlers.OnModuleInstalled += (crawlerId, moduleId) =>
            {
                _journal?.TryAddRawEntry("crawler_module_installed", $"Module {moduleId} installed on armored crawler {crawlerId}.", null!, _simDay);
                _armoredCrawlersDirty = true;
            };
            _armoredCrawlers.OnTrackThrown += crawlerId =>
            {
                _journal?.TryAddRawEntry("crawler_track_thrown", $"WARNING: Armored crawler {crawlerId} threw a track in the field!", null!, _simDay);
                _armoredCrawlersDirty = true;
            };
            _armoredCrawlers.OnCrawlerRepaired += crawlerId =>
            {
                _journal?.TryAddRawEntry("crawler_repaired", $"Armored crawler {crawlerId} field maintenance complete.", null!, _simDay);
                _armoredCrawlersDirty = true;
            };
            _armoredCrawlers.OnCampDeployed += (crawlerId, locationId) =>
            {
                _journal?.TryAddRawEntry("crawler_camp_deployed", $"Forward expedition camp established by {crawlerId} at {locationId}.", null!, _simDay);
                _armoredCrawlersDirty = true;
            };
            _armoredCrawlers.OnCampDismantled += locationId =>
            {
                _journal?.TryAddRawEntry("crawler_camp_dismantled", $"Forward expedition camp at {locationId} packed and dismantled.", null!, _simDay);
                _armoredCrawlersDirty = true;
            };

            return _armoredCrawlers;
        }

        private void SetupArmoredCrawlers()
        {
            EnsureArmoredCrawlers();
        }

        private void SaveArmoredCrawlers()
        {
            if (_armoredCrawlers != null)
            {
                CaptureSection("armored_crawlers", ArmoredCrawlerSaveStore.TryCapturePersisted(_armoredCrawlers.CaptureState()));
                _armoredCrawlersDirty = false;
            }
        }

        // ── Daily Tick Orchestration for Advanced Systems ───────────────

        public void TickAdvancedShelterSystems(int day)
        {
            if (_caravanTradeNetwork != null)
            {
                _caravanTradeNetwork.TickDay(day);
                _caravanTradeNetworkDirty = true;
            }

            if (_surgicalWard != null)
            {
                _surgicalWard.TickDay(day);
                _surgicalWardDirty = true;
            }

            if (_powerSubgrids != null)
            {
                _powerSubgrids.TickDay(day);
                _powerSubgridsDirty = true;
            }

            if (_hydroponicBiomes != null)
            {
                _hydroponicBiomes.TickDay(day);
                _hydroponicBiomesDirty = true;
            }

            if (_nuclearCore != null)
            {
                _nuclearCore.TickDay(day);
                _nuclearCoreDirty = true;
            }

            if (_armoredCrawlers != null)
            {
                _armoredCrawlers.TickDay(day);
                _armoredCrawlersDirty = true;
            }
        }
    }
}
