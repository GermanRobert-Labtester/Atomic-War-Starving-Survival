using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Shelter
{
    public enum PneumaticDispatchPriority
    {
        Normal = 0,
        High = 1,
        Emergency = 2
    }

    [Serializable]
    public sealed class PneumaticStationDefinition
    {
        [JsonPropertyName("station_id")]
        public string StationId { get; set; } = string.Empty;

        [JsonPropertyName("room_id")]
        public string RoomId { get; set; } = string.Empty;

        [JsonPropertyName("blower_id")]
        public string BlowerId { get; set; } = string.Empty;

        [JsonPropertyName("power_demand_watts")]
        public float PowerDemandWatts { get; set; } = 250f;

        [JsonPropertyName("max_cargo_mass_kg")]
        public float MaxCargoMassKg { get; set; } = 5f;

        [JsonPropertyName("max_cargo_volume_litres")]
        public float MaxCargoVolumeLitres { get; set; } = 10f;
    }

    [Serializable]
    public sealed class PneumaticLinkDefinition
    {
        [JsonPropertyName("link_id")]
        public string LinkId { get; set; } = string.Empty;

        [JsonPropertyName("from_station_id")]
        public string FromStationId { get; set; } = string.Empty;

        [JsonPropertyName("to_station_id")]
        public string ToStationId { get; set; } = string.Empty;

        [JsonPropertyName("length_m")]
        public float LengthM { get; set; } = 10f;

        [JsonPropertyName("capsule_standard")]
        public string CapsuleStandard { get; set; } = "capsule_50mm";

        [JsonPropertyName("base_leakage")]
        public float BaseLeakage { get; set; }

        [JsonPropertyName("base_seal_condition")]
        public float BaseSealCondition { get; set; } = 100f;
    }

    [Serializable]
    public sealed class PneumaticCapsuleStandardDefinition
    {
        [JsonPropertyName("standard_id")]
        public string StandardId { get; set; } = string.Empty;

        [JsonPropertyName("diameter_mm")]
        public float DiameterMm { get; set; } = 50f;

        [JsonPropertyName("base_speed_kmh")]
        public float BaseSpeedKmh { get; set; } = 30f;
    }

    [Serializable]
    public sealed class PneumaticVoicePipeDefinition
    {
        [JsonPropertyName("from_station_id")]
        public string FromStationId { get; set; } = string.Empty;

        [JsonPropertyName("to_station_id")]
        public string ToStationId { get; set; } = string.Empty;

        [JsonPropertyName("blackout_safe")]
        public bool BlackoutSafe { get; set; } = true;
    }

    [Serializable]
    public sealed class PneumaticNetworkCatalog
    {
        [JsonPropertyName("schema_version")]
        public int SchemaVersion { get; set; } = 1;

        [JsonPropertyName("stations")]
        public List<PneumaticStationDefinition> Stations { get; set; } =
            new List<PneumaticStationDefinition>();

        [JsonPropertyName("links")]
        public List<PneumaticLinkDefinition> Links { get; set; } =
            new List<PneumaticLinkDefinition>();

        [JsonPropertyName("capsule_standards")]
        public List<PneumaticCapsuleStandardDefinition> CapsuleStandards { get; set; } =
            new List<PneumaticCapsuleStandardDefinition>();

        [JsonPropertyName("voice_pipes")]
        public List<PneumaticVoicePipeDefinition> VoicePipes { get; set; } =
            new List<PneumaticVoicePipeDefinition>();
    }

    [Serializable]
    public sealed class PneumaticCapsuleState
    {
        public string CapsuleId = string.Empty;
        public string SourceStationId = string.Empty;
        public string DestinationStationId = string.Empty;
        public string ItemId = string.Empty;
        public int Amount;
        public float CargoMassKg;
        public float CargoVolumeLitres;
        public float DistanceM;
        public float ProgressHours;
        public float TransitHours;
        public PneumaticDispatchPriority Priority;
        public int QueueSequence;
        public bool Jammed;
        public bool JamResolutionApplied;
        public bool Delivered;
        public int DispatchDay;
        public int DeliveryDay = -1;
    }

    [Serializable]
    public sealed class PneumaticNetworkState
    {
        public string SystemId = PneumaticDispatchSystem.SystemId;
        public List<PneumaticCapsuleState> Capsules = new List<PneumaticCapsuleState>();
        public Dictionary<string, float> LinkSealCondition =
            new Dictionary<string, float>(StringComparer.Ordinal);
        public Dictionary<string, string> DiverterPositions =
            new Dictionary<string, string>(StringComparer.Ordinal);
        public float PressureDifferentialKpa;
        public float BlowerConditionPct = 100f;
        public float SealEfficiency = 1f;
        public bool Blackout;
        public int NextSequence = 1;
        public int LastProcessedDay = -1;
        public List<string> DeliveredMemoIds = new List<string>();
    }

    public sealed class PneumaticEndpoint
    {
        public string StationId = string.Empty;
        public Inventory.Inventory Inventory { get; }

        public PneumaticEndpoint(string stationId, Inventory.Inventory inventory)
        {
            StationId = stationId ?? string.Empty;
            Inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
        }
    }

    public sealed class PneumaticDispatchResult
    {
        public bool Success;
        public string CapsuleId = string.Empty;
        public string FailureCode = string.Empty;
        public float TransitHours;
    }

    public sealed class PneumaticNetworkSnapshot
    {
        public float PressureDifferentialKpa;
        public float BlowerConditionPct;
        public float SealEfficiency;
        public bool Blackout;
        public int QueueCount;
        public int InTransitCount;
        public bool VoicePipesAvailable;
    }

    /// <summary>
    /// Deterministic room-to-room logistics. It owns capsule transit and route
    /// state, while endpoint inventories remain the item authority.
    /// </summary>
    public sealed class PneumaticDispatchSystem
    {
        public const string SystemId = "pneumatic_dispatch";
        public const float NominalPressureDifferentialKpa = 80f;

        private PneumaticNetworkState _state;
        private readonly ISeededRng _rng;
        private readonly ILog _log;
        private readonly Dictionary<string, PneumaticStationDefinition> _stations =
            new Dictionary<string, PneumaticStationDefinition>(StringComparer.Ordinal);
        private readonly Dictionary<string, PneumaticLinkDefinition> _links =
            new Dictionary<string, PneumaticLinkDefinition>(StringComparer.Ordinal);
        private readonly Dictionary<string, PneumaticCapsuleStandardDefinition> _standards =
            new Dictionary<string, PneumaticCapsuleStandardDefinition>(StringComparer.Ordinal);
        private readonly List<PneumaticVoicePipeDefinition> _voicePipes =
            new List<PneumaticVoicePipeDefinition>();
        private readonly Dictionary<string, PneumaticEndpoint> _endpoints =
            new Dictionary<string, PneumaticEndpoint>(StringComparer.Ordinal);

        public PneumaticDispatchSystem(
            ISeededRng rng,
            PneumaticNetworkState? state = null,
            ILog? log = null)
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _state = state ?? new PneumaticNetworkState();
            _log = log ?? NullLog.Instance;
            NormalizeState();
        }

        public PneumaticNetworkState State => _state;
        public IReadOnlyDictionary<string, PneumaticStationDefinition> Stations => _stations;
        public IReadOnlyDictionary<string, PneumaticLinkDefinition> Links => _links;
        public IReadOnlyList<PneumaticCapsuleState> Capsules => _state.Capsules;
        public bool IsPowered => !_state.Blackout && _state.BlowerConditionPct > 0f;

        public event Action<PneumaticCapsuleState>? OnCapsuleDispatched;
        public event Action<PneumaticCapsuleState>? OnCapsuleArrived;
        public event Action<PneumaticCapsuleState>? OnJam;
        public event Action<string>? OnMemoDelivered;

        public void LoadCatalog(PneumaticNetworkCatalog? catalog)
        {
            if (catalog == null) return;
            _stations.Clear();
            _links.Clear();
            _standards.Clear();
            _voicePipes.Clear();
            foreach (var station in catalog.Stations)
            {
                if (station == null || string.IsNullOrWhiteSpace(station.StationId))
                    continue;
                if (station.MaxCargoMassKg <= 0f || station.MaxCargoVolumeLitres <= 0f)
                    continue;
                _stations[station.StationId] = station;
            }
            foreach (var link in catalog.Links)
            {
                if (link == null || string.IsNullOrWhiteSpace(link.LinkId) ||
                    !_stations.ContainsKey(link.FromStationId) ||
                    !_stations.ContainsKey(link.ToStationId) ||
                    link.LengthM <= 0f)
                    continue;
                _links[link.LinkId] = link;
                if (!_state.LinkSealCondition.ContainsKey(link.LinkId))
                    _state.LinkSealCondition[link.LinkId] = Math.Clamp(link.BaseSealCondition, 0f, 100f);
            }
            foreach (var standard in catalog.CapsuleStandards)
            {
                if (standard == null || string.IsNullOrWhiteSpace(standard.StandardId) ||
                    standard.DiameterMm <= 0f || standard.BaseSpeedKmh <= 0f)
                    continue;
                _standards[standard.StandardId] = standard;
            }
            foreach (var pipe in catalog.VoicePipes)
            {
                if (pipe != null && !string.IsNullOrWhiteSpace(pipe.FromStationId) &&
                    !string.IsNullOrWhiteSpace(pipe.ToStationId))
                    _voicePipes.Add(pipe);
            }
        }

        public void RegisterEndpoint(string stationId, Inventory.Inventory inventory)
        {
            if (string.IsNullOrWhiteSpace(stationId) || inventory == null) return;
            _endpoints[stationId] = new PneumaticEndpoint(stationId, inventory);
        }

        public PneumaticDispatchResult Dispatch(
            string sourceStationId,
            string destinationStationId,
            string itemId,
            int amount,
            float cargoMassKg,
            float cargoVolumeLitres,
            PneumaticDispatchPriority priority,
            int day,
            float pressure01 = 1f)
        {
            var result = new PneumaticDispatchResult();
            if (!IsPowered)
            {
                result.FailureCode = "blower_unpowered";
                return result;
            }
            if (!_endpoints.TryGetValue(sourceStationId, out var source) ||
                !_endpoints.ContainsKey(destinationStationId))
            {
                result.FailureCode = "unknown_endpoint";
                return result;
            }
            if (string.IsNullOrWhiteSpace(itemId) || amount <= 0)
            {
                result.FailureCode = "invalid_cargo";
                return result;
            }
            var route = FindRoute(sourceStationId, destinationStationId);
            if (route.Count == 0)
            {
                result.FailureCode = "no_route";
                return result;
            }
            var firstLink = _links[route[0]];
            if (!_stations.TryGetValue(sourceStationId, out var station) ||
                !_standards.TryGetValue(firstLink.CapsuleStandard, out var standard))
            {
                result.FailureCode = "invalid_route_data";
                return result;
            }
            if (cargoMassKg > station.MaxCargoMassKg ||
                cargoVolumeLitres > station.MaxCargoVolumeLitres)
            {
                result.FailureCode = "cargo_limit";
                return result;
            }
            if (!source.Inventory.TryConsume(itemId, amount))
            {
                result.FailureCode = "missing_cargo";
                return result;
            }

            float distanceM = 0f;
            float seal = 1f;
            foreach (var linkId in route)
            {
                var link = _links[linkId];
                distanceM += link.LengthM;
                seal = Math.Min(seal, GetSealCondition(linkId) / 100f);
            }
            float pressure = Math.Clamp(pressure01, 0f, 1f);
            float pressureModifier = 0.45f + pressure * 0.55f;
            float sealModifier = Math.Clamp(0.55f + seal * 0.45f, 0.1f, 1f);
            float speedKmh = standard.BaseSpeedKmh * pressureModifier * sealModifier;
            float hours = Math.Max(0.01f, distanceM / 1000f / Math.Max(0.1f, speedKmh));
            var capsule = new PneumaticCapsuleState
            {
                CapsuleId = $"capsule_{day}_{_state.NextSequence}",
                SourceStationId = sourceStationId,
                DestinationStationId = destinationStationId,
                ItemId = itemId,
                Amount = amount,
                CargoMassKg = Math.Max(0f, cargoMassKg),
                CargoVolumeLitres = Math.Max(0f, cargoVolumeLitres),
                DistanceM = distanceM,
                TransitHours = hours,
                Priority = priority,
                QueueSequence = _state.NextSequence++,
                DispatchDay = day
            };

            float jamRisk = Math.Clamp(
                0.01f + (1f - seal) * 0.12f +
                (cargoMassKg / Math.Max(0.1f, station.MaxCargoMassKg)) * 0.06f +
                (1f - _state.BlowerConditionPct / 100f) * 0.08f, 0f, 0.5f);
            if (_rng.NextDouble() < jamRisk)
            {
                capsule.Jammed = true;
                OnJam?.Invoke(capsule);
            }
            _state.Capsules.Add(capsule);
            SortQueue();
            result.Success = true;
            result.CapsuleId = capsule.CapsuleId;
            result.TransitHours = hours;
            OnCapsuleDispatched?.Invoke(capsule);
            return result;
        }

        public ActionResult ClearJam(string capsuleId)
        {
            var capsule = FindCapsule(capsuleId);
            if (capsule == null)
                return ActionResult.Failed("unknown_capsule", "pneumatic.unknown_capsule");
            if (!capsule.Jammed)
                return ActionResult.Blocked("not_jammed", "pneumatic.not_jammed");
            capsule.Jammed = false;
            capsule.JamResolutionApplied = true;
            DamageSeals(capsule, 3f);
            return ActionResult.Success("pneumatic.jam_cleared");
        }

        public ActionResult Maintain(string linkId, float amount, int day)
        {
            if (!_links.ContainsKey(linkId))
                return ActionResult.Failed("unknown_link", "pneumatic.unknown_link");
            _state.LinkSealCondition[linkId] = Math.Clamp(GetSealCondition(linkId) + amount, 0f, 100f);
            _state.BlowerConditionPct = Math.Clamp(_state.BlowerConditionPct + amount * 0.5f, 0f, 100f);
            return ActionResult.Success("pneumatic.maintained");
        }

        public void SetBlackout(bool blackout)
        {
            _state.Blackout = blackout;
            _state.PressureDifferentialKpa = blackout ? 0f : _state.PressureDifferentialKpa;
        }

        public bool VoicePipeAvailable(string fromStationId, string toStationId)
        {
            if (!_state.Blackout && IsPowered) return true;
            foreach (var pipe in _voicePipes)
            {
                if (!pipe.BlackoutSafe) continue;
                bool sameDirection = pipe.FromStationId == fromStationId && pipe.ToStationId == toStationId;
                bool reverse = pipe.FromStationId == toStationId && pipe.ToStationId == fromStationId;
                if ((sameDirection || reverse) && _stations.ContainsKey(fromStationId) &&
                    _stations.ContainsKey(toStationId))
                    return true;
            }
            return false;
        }

        public bool QueueMemo(string capsuleId, string memoId)
        {
            var capsule = FindCapsule(capsuleId);
            if (capsule == null || string.IsNullOrWhiteSpace(memoId)) return false;
            capsule.ItemId = "memo:" + memoId;
            capsule.Amount = 1;
            return true;
        }

        public void TickDay(int day, float powerAvailability01 = 1f)
        {
            if (day <= _state.LastProcessedDay) return;
            _state.LastProcessedDay = day;
            float power = Math.Clamp(powerAvailability01, 0f, 1f);
            _state.PressureDifferentialKpa = _state.Blackout
                ? 0f
                : NominalPressureDifferentialKpa * power;
            if (_state.Blackout || power <= 0f)
                return;

            SortQueue();
            for (int i = 0; i < _state.Capsules.Count; i++)
            {
                var capsule = _state.Capsules[i];
                if (capsule.Delivered || capsule.Jammed) continue;
                capsule.ProgressHours += 24f * power;
                if (capsule.ProgressHours + 0.0001f < capsule.TransitHours) continue;
                Deliver(capsule, day);
            }
        }

        public PneumaticNetworkSnapshot Snapshot()
        {
            int queue = 0;
            int inTransit = 0;
            foreach (var capsule in _state.Capsules)
            {
                if (capsule.Delivered) continue;
                queue++;
                if (capsule.ProgressHours > 0f) inTransit++;
            }
            bool voice = _voicePipes.Count > 0;
            return new PneumaticNetworkSnapshot
            {
                PressureDifferentialKpa = _state.PressureDifferentialKpa,
                BlowerConditionPct = _state.BlowerConditionPct,
                SealEfficiency = _state.SealEfficiency,
                Blackout = _state.Blackout,
                QueueCount = queue,
                InTransitCount = inTransit,
                VoicePipesAvailable = voice
            };
        }

        public PneumaticNetworkState CaptureState()
        {
            var serializer = new SystemTextJsonSerializer();
            return serializer.Deserialize<PneumaticNetworkState>(serializer.Serialize(_state))
                ?? new PneumaticNetworkState();
        }

        public void RestoreState(PneumaticNetworkState? saved)
        {
            if (saved == null) return;
            var serializer = new SystemTextJsonSerializer();
            _state = serializer.Deserialize<PneumaticNetworkState>(serializer.Serialize(saved))
                ?? new PneumaticNetworkState();
            NormalizeState();
        }

        private void Deliver(PneumaticCapsuleState capsule, int day)
        {
            if (!_endpoints.TryGetValue(capsule.DestinationStationId, out var destination))
                return;
            if (capsule.ItemId.StartsWith("memo:", StringComparison.Ordinal))
            {
                string memo = capsule.ItemId.Substring("memo:".Length);
                if (!_state.DeliveredMemoIds.Contains(memo))
                {
                    _state.DeliveredMemoIds.Add(memo);
                    OnMemoDelivered?.Invoke(memo);
                }
                capsule.Delivered = true;
                capsule.DeliveryDay = day;
                OnCapsuleArrived?.Invoke(capsule);
                return;
            }
            if (!destination.Inventory.AddById(capsule.ItemId, capsule.Amount))
                return;
            capsule.Delivered = true;
            capsule.DeliveryDay = day;
            DamageSeals(capsule, 0.5f);
            OnCapsuleArrived?.Invoke(capsule);
        }

        private List<string> FindRoute(string source, string destination)
        {
            if (source == destination) return new List<string>();
            var frontier = new Queue<string>();
            var previousStation = new Dictionary<string, string>(StringComparer.Ordinal);
            var previousLink = new Dictionary<string, string>(StringComparer.Ordinal);
            frontier.Enqueue(source);
            previousStation[source] = string.Empty;
            while (frontier.Count > 0)
            {
                string current = frontier.Dequeue();
                foreach (var pair in _links)
                {
                    var link = pair.Value;
                    if (link.FromStationId != current || previousStation.ContainsKey(link.ToStationId))
                        continue;
                    previousStation[link.ToStationId] = current;
                    previousLink[link.ToStationId] = pair.Key;
                    if (link.ToStationId == destination)
                    {
                        var result = new List<string>();
                        string at = destination;
                        while (at != source)
                        {
                            result.Add(previousLink[at]);
                            at = previousStation[at];
                        }
                        result.Reverse();
                        return result;
                    }
                    frontier.Enqueue(link.ToStationId);
                }
            }
            return new List<string>();
        }

        private float GetSealCondition(string linkId) =>
            _state.LinkSealCondition.TryGetValue(linkId, out var value) ? value : 100f;

        private void DamageSeals(PneumaticCapsuleState capsule, float amount)
        {
            var route = FindRoute(capsule.SourceStationId, capsule.DestinationStationId);
            foreach (var linkId in route)
                _state.LinkSealCondition[linkId] = Math.Max(0f, GetSealCondition(linkId) - amount);
            float total = 0f;
            foreach (var value in _state.LinkSealCondition.Values) total += value;
            _state.SealEfficiency = _state.LinkSealCondition.Count == 0
                ? 1f
                : Math.Clamp(total / (_state.LinkSealCondition.Count * 100f), 0f, 1f);
        }

        private PneumaticCapsuleState? FindCapsule(string capsuleId)
        {
            foreach (var capsule in _state.Capsules)
                if (capsule.CapsuleId == capsuleId) return capsule;
            return null;
        }

        private void SortQueue()
        {
            _state.Capsules.Sort((left, right) =>
            {
                if (left.Delivered != right.Delivered) return left.Delivered ? 1 : -1;
                int byPriority = ((int)right.Priority).CompareTo((int)left.Priority);
                return byPriority != 0
                    ? byPriority
                    : left.QueueSequence.CompareTo(right.QueueSequence);
            });
        }

        private void NormalizeState()
        {
            _state.Capsules ??= new List<PneumaticCapsuleState>();
            _state.LinkSealCondition ??= new Dictionary<string, float>(StringComparer.Ordinal);
            _state.DiverterPositions ??= new Dictionary<string, string>(StringComparer.Ordinal);
            _state.DeliveredMemoIds ??= new List<string>();
            _state.NextSequence = Math.Max(1, _state.NextSequence);
            foreach (var capsule in _state.Capsules)
            {
                capsule.Amount = Math.Max(0, capsule.Amount);
                capsule.ProgressHours = Math.Max(0f, capsule.ProgressHours);
                capsule.TransitHours = Math.Max(0.01f, capsule.TransitHours);
            }
            foreach (var key in new List<string>(_state.LinkSealCondition.Keys))
                _state.LinkSealCondition[key] = Math.Clamp(_state.LinkSealCondition[key], 0f, 100f);
            _state.BlowerConditionPct = Math.Clamp(_state.BlowerConditionPct, 0f, 100f);
        }
    }

    public static class PneumaticNetworkCatalogLoader
    {
        public const string FileName = "pneumatic_network_catalog.json";

        public static PneumaticNetworkCatalog? Load(
            string dataDir,
            IFileIO fileIO,
            IJsonSerializer serializer)
        {
            if (string.IsNullOrWhiteSpace(dataDir))
                throw new ArgumentNullException(nameof(dataDir));
            if (fileIO == null) throw new ArgumentNullException(nameof(fileIO));
            if (serializer == null) throw new ArgumentNullException(nameof(serializer));
            string path = Path.Combine(dataDir, FileName);
            if (!fileIO.FileExists(path)) return null;
            try
            {
                return serializer.Deserialize<PneumaticNetworkCatalog>(fileIO.ReadAllText(path));
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to load {FileName}: {ex.Message}", ex);
            }
        }
    }
}
