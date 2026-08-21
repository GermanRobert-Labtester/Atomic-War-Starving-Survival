using System;
using System.Collections.Generic;

namespace Ashfall.Core
{
    /// <summary>
    /// ASHFALL: THE STANDING RECORD — lore strata + recast descriptions.
    /// The gazetteer the save keeps. Three strata per featured site:
    /// pre (Schedule/Overlay arrived), after (lived/palimpsest/scraped), now (active).
    /// Spec: docs/expansions/expansion_03_the_standing_record_plan.md §5.2.
    /// Engine-agnostic; no UnityEngine / Godot / JsonUtility.
    /// </summary>
    [Serializable]
    public class LocationMemoryStratum
    {
        public string siteId;
        public string stratumId;   // "pre" / "after" / "now"
        public string requiredFlag; // mutation that selects this stratum ("" = always)
        public string text;
    }

    [Serializable]
    public class LocationMemoryState
    {
        public string systemId = LocationMemorySystem.SystemId;
        public bool expansionUnlocked;
        public List<LocationMemoryStratum> strata = new List<LocationMemoryStratum>();
        public List<string> activeFlags = new List<string>();
        public List<string> recastHistory = new List<string>();
    }

    /// <summary>
    /// Loads standing_record_memory.json through host ports. Recasts location
    /// descriptions by mutation flags; never says "Morale +2". Holdfast recasts
    /// (desalination occupied) remain Holdfast's — this adds Overlay/lived layers.
    /// </summary>
    public sealed class LocationMemorySystem
    {
        public const string SystemId = "location_memory_system";
        public const string FlagExpUnlocked = "exp_standing_record_unlocked";
        public const string MemoryFile = "standing_record_memory.json";

        // Active stratum flags (spec §4.1 mutations + §3 endings)
        public const string MutationKm19Plated = "mutation_km19_plated";
        public const string MutationKm19Scraped = "mutation_km19_scraped";
        public const string MutationKm19Palimpsest = "mutation_km19_palimpsest";
        public const string MutationTransitMaps = "mutation_transit_maps";
        public const string MutationArchiveDug = "mutation_archive_dug";
        public const string MutationArchiveSunk = "mutation_archive_sunk";
        public const string MutationMinistryRecast = "mutation_ministry_recast";
        public const string MutationWeighLots = "mutation_weigh_lots";
        public const string MutationWeighMassOnly = "mutation_weigh_mass_only";
        public const string MutationVergeNames = "mutation_verge_names";
        public const string MutationBridgeListed = "mutation_bridge_listed";
        public const string MutationBridgeDisturbed = "mutation_bridge_disturbed";
        public const string MutationLockCompleteLie = "mutation_lock_complete_lie";
        public const string MutationLockGaugesFiled = "mutation_lock_gauges_filed";
        public const string MutationLockPlateDown = "mutation_lock_plate_down";
        public const string Mutation12bAddress = "mutation_12b_address";
        public const string Mutation12bKitGone = "mutation_12b_kit_gone";
        public const string MutationPumpLive = "mutation_pump_live";
        public const string MutationPumpCondemned = "mutation_pump_condemned";
        public const string MutationGazetteerStands = "mutation_gazetteer_stands";
        public const string MutationGazetteerLived = "mutation_gazetteer_lived";
        public const string MutationGazetteerPalimpsest = "mutation_gazetteer_palimpsest";
        public const string MutationGazetteerScraped = "mutation_gazetteer_scraped";

        private readonly IFileIO _files;
        private readonly IJsonSerializer _json;
        private readonly ILog _log;
        private readonly List<LocationMemoryStratum> _strata = new List<LocationMemoryStratum>();
        private readonly Dictionary<string, List<LocationMemoryStratum>> _bySite =
            new Dictionary<string, List<LocationMemoryStratum>>();

        private LocationMemoryState _state = new LocationMemoryState();
        private readonly HashSet<string> _activeFlags = new HashSet<string>();

        public event Action<string, string> OnLocationRecast;
        public event Action<LocationMemoryState> OnStateChanged;

        public LocationMemoryState State => _state;
        public bool IsUnlocked => _state.expansionUnlocked;
        public int StratumCount => _strata.Count;

        public LocationMemorySystem(IFileIO files, IJsonSerializer json, ILog log = null!)
        {
            _files = files ?? throw new ArgumentNullException(nameof(files));
            _json = json ?? throw new ArgumentNullException(nameof(json));
            _log = log ?? NullLog.Instance;
        }

        public void Load(string dataDirectory)
        {
            _strata.Clear();
            _bySite.Clear();
            if (string.IsNullOrEmpty(dataDirectory) || !_files.DirectoryExists(dataDirectory))
            {
                _log.Warn("Standing Record memory directory missing: " + dataDirectory);
                return;
            }

            string path = _files.Combine(dataDirectory, MemoryFile);
            if (!_files.FileExists(path))
            {
                _log.Warn("Standing Record memory file missing: " + path);
                return;
            }

            try
            {
                string blob = _files.ReadAllText(path);
                var items = _json.Deserialize<List<LocationMemoryStratum>>(blob);
                if (items == null) return;
                for (int i = 0; i < items.Count; i++)
                {
                    LocationMemoryStratum s = items[i];
                    if (s == null || string.IsNullOrEmpty(s.siteId) || string.IsNullOrEmpty(s.stratumId))
                        continue;
                    _strata.Add(s);
                    List<LocationMemoryStratum> list;
                    if (!_bySite.TryGetValue(s.siteId, out list))
                    {
                        list = new List<LocationMemoryStratum>();
                        _bySite[s.siteId] = list;
                    }
                    list.Add(s);
                }
            }
            catch (Exception e)
            {
                _log.Error("Standing Record memory parse failed: " + e.Message);
            }
        }

        /// <summary>Old saves: sites keep their Holdfast/base descriptions until unlocked.</summary>
        public void Unlock()
        {
            if (_state.expansionUnlocked) return;
            _state.expansionUnlocked = true;
            RaiseChanged();
        }

        public void ApplyMutation(string flag)
        {
            if (string.IsNullOrEmpty(flag)) return;
            if (_activeFlags.Add(flag))
            {
                _state.activeFlags.Add(flag);
                RecastAffectedSites(flag);
            }
        }

        public bool HasMutation(string flag)
        {
            return !string.IsNullOrEmpty(flag) && _activeFlags.Contains(flag);
        }

        /// <summary>
        /// The active "now" text for a site: last matching stratum by flag set
        /// (now wins over after/pre), else the base description is untouched.
        /// </summary>
        public string GetActiveRecast(string siteId)
        {
            if (string.IsNullOrEmpty(siteId)) return null;
            List<LocationMemoryStratum> list;
            if (!_bySite.TryGetValue(siteId, out list)) return null;

            // "now" strata first (they are the current lived layer), then "after".
            // No "pre" fallback: a recast exists only once a mutation re-wrote
            // the place. The unmutated baseline is GetStratumText(site, "pre").
            LocationMemoryStratum? match = null;
            for (int i = 0; i < list.Count; i++)
            {
                LocationMemoryStratum s = list[i];
                if (s == null || s.stratumId != "now") continue;
                if (string.IsNullOrEmpty(s.requiredFlag) || _activeFlags.Contains(s.requiredFlag))
                {
                    // pick the most specific (last matching flag-specific now)
                    match = s;
                }
            }
            if (match != null) return match.text;

            for (int i = 0; i < list.Count; i++)
            {
                LocationMemoryStratum s = list[i];
                if (s == null || s.stratumId != "after") continue;
                if (string.IsNullOrEmpty(s.requiredFlag) || _activeFlags.Contains(s.requiredFlag))
                    return s.text;
            }
            return null;
        }

        public string GetStratumText(string siteId, string stratumId)
        {
            if (string.IsNullOrEmpty(siteId) || string.IsNullOrEmpty(stratumId)) return null;
            List<LocationMemoryStratum> list;
            if (!_bySite.TryGetValue(siteId, out list)) return null;
            for (int i = 0; i < list.Count; i++)
            {
                LocationMemoryStratum s = list[i];
                if (s != null && s.stratumId == stratumId)
                    return s.text;
            }
            return null;
        }

        public LocationMemoryState CaptureState()
        {
            var copy = new LocationMemoryState
            {
                systemId = _state.systemId,
                expansionUnlocked = _state.expansionUnlocked,
                activeFlags = _state.activeFlags != null ? new List<string>(_state.activeFlags) : new List<string>(),
                recastHistory = _state.recastHistory != null ? new List<string>(_state.recastHistory) : new List<string>(),
                strata = new List<LocationMemoryStratum>()
            };
            for (int i = 0; i < _strata.Count; i++)
            {
                LocationMemoryStratum s = _strata[i];
                if (s == null) continue;
                copy.strata.Add(new LocationMemoryStratum
                {
                    siteId = s.siteId,
                    stratumId = s.stratumId,
                    requiredFlag = s.requiredFlag,
                    text = s.text
                });
            }
            return copy;
        }

        public void RestoreState(LocationMemoryState saved)
        {
            if (saved == null) _state = new LocationMemoryState();
            else
            {
                // Deep-copy: the live system must never alias the envelope's lists.
                var fresh = new LocationMemoryState
                {
                    systemId = saved.systemId,
                    expansionUnlocked = saved.expansionUnlocked,
                    activeFlags = saved.activeFlags != null
                        ? new List<string>(saved.activeFlags)
                        : new List<string>(),
                    recastHistory = saved.recastHistory != null
                        ? new List<string>(saved.recastHistory)
                        : new List<string>(),
                    strata = new List<LocationMemoryStratum>()
                };
                if (saved.strata != null)
                {
                    for (int i = 0; i < saved.strata.Count; i++)
                    {
                        LocationMemoryStratum s = saved.strata[i];
                        if (s == null) continue;
                        fresh.strata.Add(new LocationMemoryStratum
                        {
                            siteId = s.siteId,
                            stratumId = s.stratumId,
                            requiredFlag = s.requiredFlag,
                            text = s.text
                        });
                    }
                }
                _state = fresh;
            }
            if (string.IsNullOrEmpty(_state.systemId)) _state.systemId = SystemId;
            _activeFlags.Clear();
            if (_state.activeFlags != null)
            {
                for (int i = 0; i < _state.activeFlags.Count; i++)
                    if (!string.IsNullOrEmpty(_state.activeFlags[i]))
                        _activeFlags.Add(_state.activeFlags[i]);
            }
            if (_state.strata != null)
            {
                _strata.Clear();
                _bySite.Clear();
                for (int i = 0; i < _state.strata.Count; i++)
                {
                    LocationMemoryStratum s = _state.strata[i];
                    if (s == null || string.IsNullOrEmpty(s.siteId)) continue;
                    _strata.Add(s);
                    List<LocationMemoryStratum> list;
                    if (!_bySite.TryGetValue(s.siteId, out list))
                    {
                        list = new List<LocationMemoryStratum>();
                        _bySite[s.siteId] = list;
                    }
                    list.Add(s);
                }
            }
            RaiseChanged();
        }

        private void RecastAffectedSites(string flag)
        {
            List<LocationMemoryStratum> flagsList = new List<LocationMemoryStratum>();
            for (int i = 0; i < _strata.Count; i++)
            {
                LocationMemoryStratum s = _strata[i];
                if (s != null && s.requiredFlag == flag && s.stratumId == "now")
                    flagsList.Add(s);
            }
            for (int f = 0; f < flagsList.Count; f++)
            {
                string siteId = flagsList[f].siteId;
                string active = GetActiveRecast(siteId)!;
                if (string.IsNullOrEmpty(active)) continue;
                if (_state.recastHistory.Contains(siteId)) continue;
                _state.recastHistory.Add(siteId);
                OnLocationRecast?.Invoke(siteId, active);
            }
        }

        private void RaiseChanged() => OnStateChanged?.Invoke(_state);
    }
}