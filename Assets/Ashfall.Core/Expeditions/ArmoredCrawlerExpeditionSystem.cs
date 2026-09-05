// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Inventory;

namespace Ashfall.Core.Expeditions
{
    [Serializable]
    public sealed class RemoteCampState
    {
        public string campId = string.Empty;
        public string locationId = string.Empty;
        public string crawlerId = string.Empty;
        public int establishedDay;
        public bool hasWorkshop;
        public float defenseBonus;

        public RemoteCampState Clone()
        {
            return new RemoteCampState
            {
                campId = campId,
                locationId = locationId,
                crawlerId = crawlerId,
                establishedDay = establishedDay,
                hasWorkshop = hasWorkshop,
                defenseBonus = defenseBonus
            };
        }
    }

    [Serializable]
    public sealed class ArmoredCrawlerState
    {
        public string crawlerId = string.Empty;
        public string chassisId = "crawler_heavy_chassis_mk1";
        public List<string> installedModuleIds = new List<string>();
        public float hullIntegrity = 100.0f; // 0..100
        public float trackCondition = 100.0f; // 0..100
        public float fuelOnboard = 150.0f;
        public List<string> crewRoster = new List<string>();
        public string currentRouteId = string.Empty;
        public string currentLocationId = string.Empty;
        public bool isImmobilized;
        public string deployedCampId = string.Empty;
        public int maxSlots = 6;
        public float maxMass = 3500.0f;

        public ArmoredCrawlerState Clone()
        {
            return new ArmoredCrawlerState
            {
                crawlerId = crawlerId,
                chassisId = chassisId,
                installedModuleIds = new List<string>(installedModuleIds),
                hullIntegrity = hullIntegrity,
                trackCondition = trackCondition,
                fuelOnboard = fuelOnboard,
                crewRoster = new List<string>(crewRoster),
                currentRouteId = currentRouteId,
                currentLocationId = currentLocationId,
                isImmobilized = isImmobilized,
                deployedCampId = deployedCampId,
                maxSlots = maxSlots,
                maxMass = maxMass
            };
        }
    }

    [Serializable]
    public sealed class ArmoredCrawlerExpeditionSave
    {
        public List<ArmoredCrawlerState> crawlers = new List<ArmoredCrawlerState>();
        public List<RemoteCampState> remoteCamps = new List<RemoteCampState>();
        public int lastTickDay;

        public ArmoredCrawlerExpeditionSave Clone()
        {
            return new ArmoredCrawlerExpeditionSave
            {
                crawlers = crawlers.Select(c => c.Clone()).ToList(),
                remoteCamps = remoteCamps.Select(rc => rc.Clone()).ToList(),
                lastTickDay = lastTickDay
            };
        }
    }

    public sealed class ArmoredCrawlerExpeditionSystem
    {
        private readonly Inventory.Inventory _inventory;
        private readonly ArmoredCrawlerModuleCatalog _catalog;
        private readonly ISeededRng _rng;
        private readonly ILog _log;

        private readonly List<ArmoredCrawlerState> _crawlers = new List<ArmoredCrawlerState>();
        private readonly List<RemoteCampState> _remoteCamps = new List<RemoteCampState>();
        private int _lastTickDay;

        public IReadOnlyList<ArmoredCrawlerState> Crawlers => _crawlers;
        public IReadOnlyList<RemoteCampState> RemoteCamps => _remoteCamps;
        public int LastTickDay => _lastTickDay;

        public event Action<string, string>? OnModuleInstalled;
        public event Action<string, string>? OnModuleRemoved;
        public event Action<string>? OnTrackThrown;
        public event Action<string>? OnCrawlerRepaired;
        public event Action<string, string>? OnCampDeployed;
        public event Action<string>? OnCampDismantled;

        public ArmoredCrawlerExpeditionSystem(
            Inventory.Inventory inventory,
            ArmoredCrawlerModuleCatalog catalog,
            ISeededRng rng,
            ILog? log = null,
            int initialCrawlerCount = 1)
        {
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _log = log ?? NullLog.Instance;

            for (int i = 1; i <= initialCrawlerCount; i++)
            {
                _crawlers.Add(new ArmoredCrawlerState
                {
                    crawlerId = $"crawler_{i:D2}",
                    chassisId = "crawler_heavy_chassis_mk1",
                    installedModuleIds = new List<string>(),
                    hullIntegrity = 100.0f,
                    trackCondition = 100.0f,
                    fuelOnboard = 150.0f,
                    crewRoster = new List<string>(),
                    currentRouteId = string.Empty,
                    currentLocationId = "loc_shelter",
                    isImmobilized = false,
                    deployedCampId = string.Empty,
                    maxSlots = 6,
                    maxMass = 3500.0f
                });
            }
        }

        public ArmoredCrawlerState? GetCrawler(string crawlerId)
        {
            if (string.IsNullOrEmpty(crawlerId)) return null;
            return _crawlers.FirstOrDefault(c => string.Equals(c.crawlerId, crawlerId, StringComparison.OrdinalIgnoreCase));
        }

        public float ComputeTotalMass(string crawlerId)
        {
            var crawler = GetCrawler(crawlerId);
            if (crawler == null) return 0f;

            float total = 1200.0f; // Base chassis dry mass
            foreach (var modId in crawler.installedModuleIds)
            {
                var def = _catalog.GetModule(modId);
                if (def != null) total += def.mass;
            }
            return total;
        }

        public int GetEffectiveCrewBerths(string crawlerId)
        {
            var crawler = GetCrawler(crawlerId);
            if (crawler == null) return 0;

            int berths = 2; // Base cockpit berths
            foreach (var modId in crawler.installedModuleIds)
            {
                var def = _catalog.GetModule(modId);
                if (def != null) berths += def.crewBerths;
            }
            return berths;
        }

        public bool HasWorkshopCapability(string crawlerId)
        {
            var crawler = GetCrawler(crawlerId);
            if (crawler == null) return false;

            return crawler.installedModuleIds.Any(mid =>
            {
                var def = _catalog.GetModule(mid);
                return def?.workshopCapability == true;
            });
        }

        public bool CanTraverseTerrain(string crawlerId, string terrainTag)
        {
            var crawler = GetCrawler(crawlerId);
            if (crawler == null || crawler.isImmobilized || crawler.trackCondition <= 0f)
                return false;

            var validCrawlerTerrains = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "heavy_tracked", "deep_ash", "snow", "mud", "slag", "road", "wasteland"
            };

            return validCrawlerTerrains.Contains(terrainTag);
        }

        public bool TryInstallModule(string crawlerId, string moduleId)
        {
            var crawler = GetCrawler(crawlerId);
            if (crawler == null) return false;
            if (crawler.installedModuleIds.Count >= crawler.maxSlots) return false;

            var def = _catalog.GetModule(moduleId);
            if (def == null) return false;

            if (ComputeTotalMass(crawlerId) + def.mass > crawler.maxMass) return false;

            var bill = new InventoryBill();
            bill.AddCost(moduleId, 1);

            bool committed = _inventory.TryExecuteTransaction(bill, () =>
            {
                crawler.installedModuleIds.Add(moduleId);
                _log.Info($"[Crawler] Installed module '{moduleId}' on '{crawlerId}'.");
                OnModuleInstalled?.Invoke(crawlerId, moduleId);
            });

            return committed;
        }

        public bool TryUninstallModule(string crawlerId, string moduleId)
        {
            var crawler = GetCrawler(crawlerId);
            if (crawler == null || !crawler.installedModuleIds.Contains(moduleId)) return false;

            var bill = new InventoryBill();
            bill.AddGrant(moduleId, 1);

            bool committed = _inventory.TryExecuteTransaction(bill, () =>
            {
                crawler.installedModuleIds.Remove(moduleId);
                _log.Info($"[Crawler] Removed module '{moduleId}' from '{crawlerId}'.");
                OnModuleRemoved?.Invoke(crawlerId, moduleId);
            });

            return committed;
        }

        public ExpeditionVehicleProfile ProjectToVehicleProfile(string crawlerId)
        {
            var crawler = GetCrawler(crawlerId);
            if (crawler == null)
            {
                return new ExpeditionVehicleProfile { vehicleId = crawlerId, speedMultiplier = 1.0f };
            }

            float cargoCap = 500.0f;
            float fuelMod = 0f;
            float armorMod = 0f;

            foreach (var mid in crawler.installedModuleIds)
            {
                var def = _catalog.GetModule(mid);
                if (def != null)
                {
                    cargoCap += def.cargoModifier;
                    fuelMod += def.fuelModifier;
                    armorMod += def.armorModifier;
                }
            }

            float trackRatio = Math.Clamp(crawler.trackCondition / 100.0f, 0.1f, 1.0f);
            float speedMult = crawler.isImmobilized ? 0f : (1.4f * trackRatio);

            return new ExpeditionVehicleProfile
            {
                vehicleId = crawler.crawlerId,
                speedMultiplier = speedMult,
                cargoCapacityKg = cargoCap,
                breakdownChancePerTick = crawler.trackCondition < 40f ? (40f - crawler.trackCondition) / 150f : 0.015f,
                fuelPerTravelTick = Math.Max(1.0f, 3.0f * (1.0f + fuelMod))
            };
        }

        public bool TryRepairTrack(string crawlerId)
        {
            var crawler = GetCrawler(crawlerId);
            if (crawler == null) return false;

            var bill = new InventoryBill();
            bill.AddCost("mechanical_parts", 2);

            bool committed = _inventory.TryExecuteTransaction(bill, () =>
            {
                crawler.trackCondition = 100.0f;
                crawler.isImmobilized = false;
                _log.Info($"[Crawler] Field repair completed on '{crawlerId}' tracks.");
                OnCrawlerRepaired?.Invoke(crawlerId);
            });

            return committed;
        }

        public bool TryDeployCamp(string crawlerId, string locationId)
        {
            var crawler = GetCrawler(crawlerId);
            if (crawler == null || crawler.isImmobilized || string.IsNullOrWhiteSpace(locationId))
                return false;
            if (!string.IsNullOrEmpty(crawler.deployedCampId)) return false;

            string campId = $"camp_{crawlerId}_{locationId}";
            var camp = new RemoteCampState
            {
                campId = campId,
                locationId = locationId,
                crawlerId = crawlerId,
                establishedDay = _lastTickDay,
                hasWorkshop = HasWorkshopCapability(crawlerId),
                defenseBonus = 25.0f
            };

            _remoteCamps.Add(camp);
            crawler.deployedCampId = campId;
            OnCampDeployed?.Invoke(crawlerId, campId);
            return true;
        }

        public bool TryDismantleCamp(string crawlerId)
        {
            var crawler = GetCrawler(crawlerId);
            if (crawler == null || string.IsNullOrEmpty(crawler.deployedCampId)) return false;

            string campId = crawler.deployedCampId;
            _remoteCamps.RemoveAll(c => string.Equals(c.campId, campId, StringComparison.OrdinalIgnoreCase));
            crawler.deployedCampId = string.Empty;
            OnCampDismantled?.Invoke(crawlerId);
            return true;
        }

        public void TickDay(int currentDay, ISeededRng? tickRng = null)
        {
            _lastTickDay = currentDay;
            var rng = tickRng ?? _rng;

            foreach (var crawler in _crawlers)
            {
                if (crawler.isImmobilized) continue;

                // Active route operations wear tracks and consume fuel
                if (!string.IsNullOrEmpty(crawler.currentRouteId))
                {
                    crawler.trackCondition = Math.Max(0f, crawler.trackCondition - 5.0f);
                    crawler.fuelOnboard = Math.Max(0f, crawler.fuelOnboard - 15.0f);

                    if (crawler.trackCondition <= 0f)
                    {
                        crawler.isImmobilized = true;
                        OnTrackThrown?.Invoke(crawler.crawlerId);
                    }
                    else if (crawler.trackCondition < 40f)
                    {
                        // Thrown track risk roll
                        if (rng.NextDouble() < 0.20)
                        {
                            crawler.isImmobilized = true;
                            OnTrackThrown?.Invoke(crawler.crawlerId);
                        }
                    }
                }
            }
        }

        public ArmoredCrawlerExpeditionSave CaptureState()
        {
            return new ArmoredCrawlerExpeditionSave
            {
                crawlers = _crawlers.Select(c => c.Clone()).ToList(),
                remoteCamps = _remoteCamps.Select(rc => rc.Clone()).ToList(),
                lastTickDay = _lastTickDay
            };
        }

        public void RestoreState(ArmoredCrawlerExpeditionSave? save)
        {
            if (save == null) return;

            _lastTickDay = save.lastTickDay;

            _crawlers.Clear();
            if (save.crawlers != null)
            {
                foreach (var c in save.crawlers)
                    _crawlers.Add(c.Clone());
            }

            _remoteCamps.Clear();
            if (save.remoteCamps != null)
            {
                foreach (var rc in save.remoteCamps)
                    _remoteCamps.Add(rc.Clone());
            }
        }
    }
}
