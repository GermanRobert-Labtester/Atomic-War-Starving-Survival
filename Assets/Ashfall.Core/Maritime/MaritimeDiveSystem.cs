using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.IO;

#pragma warning disable CS8618

namespace Ashfall.Core.Maritime
{
    public enum DiveRoomType { Deckhouse, Companionway, HoldApproach, DeepHold }

    public enum DiveResult { Success, Partial, Contaminated, Failed, CrewLost }

    [Serializable]
    public sealed class DiveRoomNode
    {
        public DiveRoomType roomType;
        public float searchProgress; // 0.0 to 100.0
        public bool isLooted;
        public int hazardLevel; // 1 to 5
    }

    [Serializable]
    public sealed class DiveSite
    {
        public string siteId = string.Empty;
        public string displayName = string.Empty;
        public float depthMeters;
        public float hazardLevel;     // 0-1
        public bool isExplored;
        public bool isHazardous;
        public float radiationLevel;
    }

    [Serializable]
    public sealed class DiveOutcome
    {
        public string siteId = string.Empty;
        public int day;
        public DiveResult result;
        public string recoveredItemId = string.Empty;
        public float radiationDose;
        public string notes = string.Empty;
    }

    [Serializable]
    public class StealthDiveSaveState
    {
        public string systemId = "maritime_dive";
        public bool isActive;
        public string siteId = string.Empty;
        public string diverDwellerId = string.Empty;
        public string compressorOperatorDwellerId = string.Empty;
        public float airSupplySeconds = 120f;
        public float maxAirSupplySeconds = 120f;
        public int currentRoomIndex;
        public int noiseLevel; // 0 to 100
        public bool isCompromised;
        public float decompressionRequiredSeconds;
        public float decompressionProgressSeconds;
        public bool isDecompressing;
        public bool hasDecompressionSickness;
        public float accumulatedRadiationDose;
        public bool diverLost;
        public List<DiveRoomNode> rooms = new List<DiveRoomNode>();
        public List<DiveSite> sites = new List<DiveSite>();
        public List<DiveOutcome> outcomes = new List<DiveOutcome>();
    }

    [Serializable]
    public sealed class MaritimeDiveState : StealthDiveSaveState
    {
    }

    /// <summary>
    /// ASHFALL: THE BLACK FLOTILLA (Expansion 09) — Authoritative Maritime Dive System.
    /// Single authority unifying the 4-chamber stealth dive state machine, air compressor
    /// delivery, acoustic noise detection, deep decompression stages, emergency aborts,
    /// diver loss / asphyxiation triage, radiation dosing, and catalog site registration.
    /// </summary>
    public class MaritimeDiveSystem
    {
        public const string SystemId = "maritime_dive";
        public const float BaseAirPerCrank = 30f; // Seconds of air gained per manual operator crank

        private readonly ISeededRng _rng;
        private readonly ILog _log;
        private int _currentDay;
        private bool _airWarningFired;

        private readonly List<DiveRoomNode> _rooms = new List<DiveRoomNode>();
        private readonly List<DiveSite> _sites = new List<DiveSite>();
        private readonly List<DiveOutcome> _outcomes = new List<DiveOutcome>();

        public DiveSiteContainer Catalog { get; private set; } = new DiveSiteContainer();

        public bool IsActive { get; private set; }
        public string CurrentSiteId { get; private set; } = string.Empty;
        public string DiverDwellerId { get; private set; } = string.Empty;
        public string CompressorOperatorDwellerId { get; private set; } = string.Empty;
        public float AirSupplySeconds { get; private set; }
        public float MaxAirSupplySeconds { get; private set; } = 120f;
        public int CurrentRoomIndex { get; private set; }
        public int NoiseLevel { get; private set; }
        public bool IsCompromised { get; private set; }

        public float DecompressionRequiredSeconds { get; private set; }
        public float DecompressionProgressSeconds { get; private set; }
        public bool IsDecompressing { get; private set; }
        public bool HasDecompressionSickness { get; private set; }
        public float AccumulatedRadiationDose { get; private set; }
        public bool DiverLost { get; private set; }

        public IReadOnlyList<DiveRoomNode> Rooms => _rooms;
        public IReadOnlyList<DiveSite> Sites => _sites;
        public IReadOnlyList<DiveOutcome> Outcomes => _outcomes;

        public MaritimeDiveState State => CaptureState();

        // ── Events ──────────────────────────────────────────────────────────
        public event Action<float>? OnAirWarning;
        public event Action<int>? OnRoomEntered;
        public event Action<bool>? OnDiveEnded;
        public event Action<float>? OnDecompressionStarted;
        public event Action? OnDecompressionCompleted;
        public event Action<string>? OnDiverLost;
        public event Action<DiveOutcome>? OnDiveCompleted;
        public event Action? OnSitesChanged;

        public MaritimeDiveSystem(ISeededRng? rng = null, ILog? log = null)
        {
            _rng = rng ?? new SeededRng(42);
            _log = log ?? NullLog.Instance;
        }

        public void LoadCatalog(DiveSiteContainer catalog)
        {
            if (catalog?.dive_sites == null) return;
            Catalog = catalog;
            foreach (var s in catalog.dive_sites)
            {
                if (s == null) continue;
                if (!_sites.Exists(existing => existing.siteId == s.site_id))
                {
                    float avgHazard = s.rooms != null && s.rooms.Count > 0
                        ? (float)s.rooms.Average(r => r.hazard_level) / 5f
                        : 0.3f;

                    _sites.Add(new DiveSite
                    {
                        siteId = s.site_id,
                        displayName = s.name,
                        depthMeters = 15f * (s.rooms?.Count ?? 4),
                        hazardLevel = Math.Clamp(avgHazard, 0f, 1f),
                        radiationLevel = Math.Clamp(avgHazard, 0f, 1f) * 80f,
                        isHazardous = avgHazard >= 0.5f
                    });
                }
            }
            OnSitesChanged?.Invoke();
        }

        public void SeedDefaultSites()
        {
            if (_sites.Count > 0) return;
            _sites.Add(new DiveSite
            {
                siteId = "site_exp09_ss_sovereign",
                displayName = "S.S. Sovereign Wreck",
                depthMeters = 45f,
                hazardLevel = 0.4f,
                radiationLevel = 35f,
                isHazardous = false
            });
            _sites.Add(new DiveSite
            {
                siteId = "site_exp09_ferry_terminal",
                displayName = "The Drowned Ferry Terminal",
                depthMeters = 30f,
                hazardLevel = 0.5f,
                radiationLevel = 45f,
                isHazardous = true
            });
            _sites.Add(new DiveSite
            {
                siteId = "site_exp09_barge_flotilla",
                displayName = "The Barge Flotilla",
                depthMeters = 25f,
                hazardLevel = 0.3f,
                radiationLevel = 20f,
                isHazardous = false
            });
            _sites.Add(new DiveSite
            {
                siteId = "site_exp09_naval_patrol",
                displayName = "The Patrol Craft",
                depthMeters = 50f,
                hazardLevel = 0.7f,
                radiationLevel = 60f,
                isHazardous = true
            });
        }

        public ActionResult RegisterSite(string siteId, string displayName, float depthMeters, float hazardLevel)
        {
            if (_sites.Exists(s => s.siteId == siteId))
                return ActionResult.Blocked("site_exists", "dive.site_exists");

            _sites.Add(new DiveSite
            {
                siteId = siteId,
                displayName = displayName,
                depthMeters = depthMeters,
                hazardLevel = Math.Clamp(hazardLevel, 0f, 1f),
                radiationLevel = Math.Clamp(hazardLevel, 0f, 1f) * 100f,
                isHazardous = hazardLevel >= 0.5f
            });
            OnSitesChanged?.Invoke();
            return ActionResult.Success("dive.site_registered");
        }

        public void StartDive(string diverId, string operatorId, float initialAir = 120f, string siteId = "")
        {
            CurrentSiteId = !string.IsNullOrEmpty(siteId) ? siteId : "site_exp09_ss_sovereign";
            DiverDwellerId = diverId ?? string.Empty;
            CompressorOperatorDwellerId = operatorId ?? string.Empty;
            MaxAirSupplySeconds = Math.Max(30f, initialAir);
            AirSupplySeconds = MaxAirSupplySeconds;
            CurrentRoomIndex = 0;
            NoiseLevel = 0;
            IsCompromised = false;
            _airWarningFired = false;
            DecompressionRequiredSeconds = 0f;
            DecompressionProgressSeconds = 0f;
            IsDecompressing = false;
            HasDecompressionSickness = false;
            AccumulatedRadiationDose = 0f;
            DiverLost = false;
            IsActive = true;

            _rooms.Clear();
            _rooms.Add(new DiveRoomNode { roomType = DiveRoomType.Deckhouse, hazardLevel = 1 });
            _rooms.Add(new DiveRoomNode { roomType = DiveRoomType.Companionway, hazardLevel = 2 });
            _rooms.Add(new DiveRoomNode { roomType = DiveRoomType.HoldApproach, hazardLevel = 3 });
            _rooms.Add(new DiveRoomNode { roomType = DiveRoomType.DeepHold, hazardLevel = 4 });

            OnRoomEntered?.Invoke(0);
        }

        public void Tick(float deltaSeconds)
        {
            if (!IsActive) return;

            if (IsDecompressing)
            {
                DecompressionProgressSeconds += deltaSeconds;
                AirSupplySeconds = Math.Max(0f, AirSupplySeconds - deltaSeconds);
                if (DecompressionProgressSeconds >= DecompressionRequiredSeconds)
                {
                    IsDecompressing = false;
                    DecompressionRequiredSeconds = 0f;
                    DecompressionProgressSeconds = 0f;
                    OnDecompressionCompleted?.Invoke();
                }
            }
            else
            {
                int hazard = CurrentRoomIndex < _rooms.Count ? _rooms[CurrentRoomIndex].hazardLevel : 1;
                AirSupplySeconds = Math.Max(0f, AirSupplySeconds - deltaSeconds);
                AccumulatedRadiationDose += (hazard * 0.04f) * deltaSeconds;
            }

            if (AirSupplySeconds <= 30f && !_airWarningFired)
            {
                _airWarningFired = true;
                OnAirWarning?.Invoke(AirSupplySeconds);
            }

            if (AirSupplySeconds <= 0f)
            {
                if (CurrentRoomIndex >= 2)
                {
                    DiverLost = true;
                    OnDiverLost?.Invoke(DiverDwellerId);
                    EndDive(success: false, diverLost: true, reason: "Asphyxiation in deep hold.");
                }
                else
                {
                    EndDive(success: false, diverLost: false, reason: "Air depleted.");
                }
            }
        }

        public void CrankCompressor()
        {
            if (!IsActive) return;
            AirSupplySeconds = Math.Min(MaxAirSupplySeconds, AirSupplySeconds + BaseAirPerCrank);
        }

        public bool AdvanceToNextRoom(int addedNoise)
        {
            if (!IsActive) return false;
            if (CurrentRoomIndex >= _rooms.Count - 1) return false;

            CurrentRoomIndex++;
            NoiseLevel = MathfCompat.Clamp(NoiseLevel + addedNoise, 0, 100);
            if (NoiseLevel >= 80)
            {
                IsCompromised = true;
            }

            if (NoiseLevel >= 100 && CurrentRoomIndex >= 3)
            {
                DiverLost = true;
                OnDiverLost?.Invoke(DiverDwellerId);
                EndDive(success: false, diverLost: true, reason: "Catastrophic acoustic collapse.");
                return true;
            }

            // Decompression stop requirement builds as diver goes deeper
            if (CurrentRoomIndex == 2) DecompressionRequiredSeconds = Math.Max(DecompressionRequiredSeconds, 20f);
            if (CurrentRoomIndex == 3) DecompressionRequiredSeconds = Math.Max(DecompressionRequiredSeconds, 40f);

            OnRoomEntered?.Invoke(CurrentRoomIndex);
            return true;
        }

        public void StartDecompression()
        {
            if (!IsActive || DecompressionRequiredSeconds <= 0f) return;
            IsDecompressing = true;
            OnDecompressionStarted?.Invoke(DecompressionRequiredSeconds - DecompressionProgressSeconds);
        }

        public void AbortDive(bool emergency = false)
        {
            if (!IsActive) return;

            if (emergency)
            {
                if (DecompressionRequiredSeconds > DecompressionProgressSeconds)
                {
                    HasDecompressionSickness = true;
                    AccumulatedRadiationDose += 25f;
                }
                EndDive(success: false, diverLost: false, reason: "Emergency surface ascent.");
            }
            else
            {
                if (DecompressionRequiredSeconds > DecompressionProgressSeconds && AirSupplySeconds > 20f)
                {
                    // Controlled safety stop before final exit
                    StartDecompression();
                }
                else
                {
                    if (DecompressionRequiredSeconds > DecompressionProgressSeconds)
                    {
                        HasDecompressionSickness = true;
                    }
                    EndDive(success: true, diverLost: false, reason: "Controlled dive abort.");
                }
            }
        }

        public void EndDive(bool success)
        {
            EndDive(success, false, success ? "Mission accomplished." : "Mission terminated.");
        }

        public void EndDive(bool success, bool diverLost, string reason)
        {
            if (!IsActive) return;
            IsActive = false;
            DiverLost = diverLost;

            var site = _sites.Find(s => s.siteId == CurrentSiteId);
            if (site != null && success)
            {
                site.isExplored = true;
            }

            DiveResult result;
            if (diverLost) result = DiveResult.CrewLost;
            else if (HasDecompressionSickness || AccumulatedRadiationDose >= 50f) result = DiveResult.Contaminated;
            else if (success && CurrentRoomIndex >= 2) result = DiveResult.Success;
            else if (success) result = DiveResult.Partial;
            else result = DiveResult.Failed;

            string item = success ? RollRecovery(site) : string.Empty;

            var outcome = new DiveOutcome
            {
                siteId = CurrentSiteId,
                day = _currentDay,
                result = result,
                recoveredItemId = item,
                radiationDose = AccumulatedRadiationDose,
                notes = $"{reason} (diver={DiverDwellerId}, rooms={CurrentRoomIndex + 1}/4, noise={NoiseLevel}, air={AirSupplySeconds:F0}s)"
            };
            _outcomes.Add(outcome);
            _log.Info($"[Maritime Dive] {CurrentSiteId}: {result} - {reason} (dose={AccumulatedRadiationDose:F1} mSv)");

            OnDiveEnded?.Invoke(success);
            OnDiveCompleted?.Invoke(outcome);
        }

        public ActionResult ConductDive(string siteId, string diverId, float equipmentQuality)
        {
            var site = _sites.Find(s => s.siteId == siteId);
            if (site == null) return ActionResult.Failed("unknown_site", "dive.unknown_site");

            float successChance = (1f - site.hazardLevel) * equipmentQuality;
            var roll = (float)_rng.NextDouble();
            DiveResult result;
            string recoveredItem = string.Empty;
            float dose = 0;

            if (roll < successChance * 0.7f)
            {
                result = DiveResult.Success;
                recoveredItem = RollRecovery(site);
                dose = site.radiationLevel * 0.1f;
            }
            else if (roll < successChance)
            {
                result = DiveResult.Partial;
                recoveredItem = RollRecovery(site);
                dose = site.radiationLevel * 0.3f;
            }
            else if (roll < successChance + 0.15f)
            {
                result = DiveResult.Contaminated;
                recoveredItem = string.Empty;
                dose = site.radiationLevel * 0.8f;
            }
            else if (roll < successChance + 0.20f)
            {
                result = DiveResult.CrewLost;
                dose = site.radiationLevel * 2f;
            }
            else
            {
                result = DiveResult.Failed;
                dose = site.radiationLevel * 0.2f;
            }

            site.isExplored = result != DiveResult.Failed;
            site.isHazardous = result == DiveResult.Contaminated || result == DiveResult.CrewLost;

            var outcome = new DiveOutcome
            {
                siteId = siteId,
                day = _currentDay,
                result = result,
                recoveredItemId = recoveredItem,
                radiationDose = dose,
                notes = $"diver={diverId}, quality={equipmentQuality:F1}, roll={roll:F2}"
            };
            _outcomes.Add(outcome);
            _log.Info($"[Dive] {site.displayName}: {result} (dose={dose:F1} mSv)");
            OnDiveCompleted?.Invoke(outcome);
            return ActionResult.Success($"dive.{result.ToString().ToLowerInvariant()}",
                new Dictionary<string, double>
                {
                    { "dose", dose },
                    { "result", (int)result }
                });
        }

        private string RollRecovery(DiveSite? site)
        {
            var items = new[] { "salvage_metal", "salvage_electronics", "salvage_fuel", "artifact_fragments", "item_ro_resin", "item_process_barrel" };
            return items[_rng.Next(0, items.Length)];
        }

        public void TickDay(int day)
        {
            _currentDay = day;
        }

        public MaritimeDiveState CaptureState()
        {
            var save = new MaritimeDiveState
            {
                systemId = SystemId,
                isActive = IsActive,
                siteId = CurrentSiteId,
                diverDwellerId = DiverDwellerId,
                compressorOperatorDwellerId = CompressorOperatorDwellerId,
                airSupplySeconds = AirSupplySeconds,
                maxAirSupplySeconds = MaxAirSupplySeconds,
                currentRoomIndex = CurrentRoomIndex,
                noiseLevel = NoiseLevel,
                isCompromised = IsCompromised,
                decompressionRequiredSeconds = DecompressionRequiredSeconds,
                decompressionProgressSeconds = DecompressionProgressSeconds,
                isDecompressing = IsDecompressing,
                hasDecompressionSickness = HasDecompressionSickness,
                accumulatedRadiationDose = AccumulatedRadiationDose,
                diverLost = DiverLost
            };

            foreach (var r in _rooms)
            {
                save.rooms.Add(new DiveRoomNode
                {
                    roomType = r.roomType,
                    searchProgress = r.searchProgress,
                    isLooted = r.isLooted,
                    hazardLevel = r.hazardLevel
                });
            }

            foreach (var s in _sites)
            {
                save.sites.Add(new DiveSite
                {
                    siteId = s.siteId,
                    displayName = s.displayName,
                    depthMeters = s.depthMeters,
                    hazardLevel = s.hazardLevel,
                    isExplored = s.isExplored,
                    isHazardous = s.isHazardous,
                    radiationLevel = s.radiationLevel
                });
            }

            foreach (var o in _outcomes)
            {
                save.outcomes.Add(new DiveOutcome
                {
                    siteId = o.siteId,
                    day = o.day,
                    result = o.result,
                    recoveredItemId = o.recoveredItemId,
                    radiationDose = o.radiationDose,
                    notes = o.notes
                });
            }

            return save;
        }

        public void RestoreState(StealthDiveSaveState? state)
        {
            _rooms.Clear();
            if (state == null) return;

            IsActive = state.isActive;
            CurrentSiteId = state.siteId ?? string.Empty;
            DiverDwellerId = state.diverDwellerId ?? string.Empty;
            CompressorOperatorDwellerId = state.compressorOperatorDwellerId ?? string.Empty;
            AirSupplySeconds = state.airSupplySeconds;
            MaxAirSupplySeconds = state.maxAirSupplySeconds > 0 ? state.maxAirSupplySeconds : 120f;
            CurrentRoomIndex = MathfCompat.Clamp(state.currentRoomIndex, 0, MathfCompat.Max(0, state.rooms != null ? state.rooms.Count - 1 : 3));
            NoiseLevel = MathfCompat.Clamp(state.noiseLevel, 0, 100);
            IsCompromised = state.isCompromised;
            _airWarningFired = state.airSupplySeconds <= 30f;
            DecompressionRequiredSeconds = state.decompressionRequiredSeconds;
            DecompressionProgressSeconds = state.decompressionProgressSeconds;
            IsDecompressing = state.isDecompressing;
            HasDecompressionSickness = state.hasDecompressionSickness;
            AccumulatedRadiationDose = state.accumulatedRadiationDose;
            DiverLost = state.diverLost;

            if (state.rooms != null && state.rooms.Count > 0)
            {
                foreach (var r in state.rooms)
                {
                    _rooms.Add(new DiveRoomNode
                    {
                        roomType = r.roomType,
                        searchProgress = r.searchProgress,
                        isLooted = r.isLooted,
                        hazardLevel = r.hazardLevel
                    });
                }
            }

            if (state.sites != null && state.sites.Count > 0)
            {
                _sites.Clear();
                foreach (var s in state.sites)
                {
                    _sites.Add(new DiveSite
                    {
                        siteId = s.siteId,
                        displayName = s.displayName,
                        depthMeters = s.depthMeters,
                        hazardLevel = s.hazardLevel,
                        isExplored = s.isExplored,
                        isHazardous = s.isHazardous,
                        radiationLevel = s.radiationLevel
                    });
                }
            }

            if (state.outcomes != null && state.outcomes.Count > 0)
            {
                _outcomes.Clear();
                foreach (var o in state.outcomes)
                {
                    _outcomes.Add(new DiveOutcome
                    {
                        siteId = o.siteId,
                        day = o.day,
                        result = o.result,
                        recoveredItemId = o.recoveredItemId,
                        radiationDose = o.radiationDose,
                        notes = o.notes
                    });
                }
            }
        }
    }
}
