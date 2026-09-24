// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Radio
{
    public enum HeliographMessageStatus
    {
        Pending,
        Delivered,
        Blocked,
        Expired
    }

    [Serializable]
    public sealed class HeliographStationState
    {
        public string station_id = string.Empty;
        public string map_node_id = string.Empty;
        public float condition = 100f;
        public bool is_operational = true;
    }

    [Serializable]
    public sealed class HeliographStationDefinition
    {
        public string station_id = string.Empty;
        public string map_node_id = string.Empty;
        public float condition = 100f;
    }

    [Serializable]
    public sealed class HeliographCatalog
    {
        public int schema_version = 1;
        public List<HeliographStationDefinition> stations = new List<HeliographStationDefinition>();
    }

    [Serializable]
    public sealed class HeliographMessageState
    {
        public string message_id = string.Empty;
        public string origin_station_id = string.Empty;
        public string target_station_id = string.Empty;
        public string payload_key = string.Empty;
        public string reveal_location_id = string.Empty;
        public string distress_signal_id = string.Empty;
        public int transmitted_day = -1;
        public int status = (int)HeliographMessageStatus.Pending;
        public string block_reason = string.Empty;
    }

    [Serializable]
    public sealed class HeliographState
    {
        public string system_id = HeliographSystem.SystemId;
        public List<HeliographStationState> stations = new List<HeliographStationState>();
        public List<HeliographMessageState> messages = new List<HeliographMessageState>();
        public int delivered_count;
    }

    /// <summary>
    /// Optical signaling authority. It owns station condition and message
    /// lifecycle, while line of sight, weather visibility, map discovery, and
    /// distress dispatch remain injected boundaries owned by their systems.
    /// </summary>
    public sealed class HeliographSystem
    {
        public const string SystemId = "heliograph";
        public const float MinimumVisibility01 = 0.35f;

        private HeliographState _state = new HeliographState();
        private readonly Func<string, string, bool>? _hasLineOfSight;
        private readonly Func<float>? _visibility01;
        private readonly Func<string, bool>? _isMapNodeKnown;
        private readonly Action<string>? _discoverMapNode;
        private readonly Func<string, bool>? _dispatchDistress;

        public HeliographState State => _state;

        public event Action<HeliographMessageState>? OnMessageDelivered;
        public event Action<HeliographMessageState>? OnMessageBlocked;
        public event Action? OnStateChanged;

        public HeliographSystem(
            Func<string, string, bool>? hasLineOfSight = null,
            Func<float>? visibility01 = null,
            Func<string, bool>? isMapNodeKnown = null,
            Action<string>? discoverMapNode = null,
            Func<string, bool>? dispatchDistress = null)
        {
            _hasLineOfSight = hasLineOfSight;
            _visibility01 = visibility01;
            _isMapNodeKnown = isMapNodeKnown;
            _discoverMapNode = discoverMapNode;
            _dispatchDistress = dispatchDistress;
        }

        public bool RegisterStation(string stationId, string mapNodeId, float condition = 100f)
        {
            if (string.IsNullOrWhiteSpace(stationId) || string.IsNullOrWhiteSpace(mapNodeId))
                return false;
            string canonicalStationId = stationId.Trim();
            if (FindStation(canonicalStationId) != null) return false;

            float boundedCondition = SanitizeRange(condition, 0f, 100f);
            _state.stations.Add(new HeliographStationState
            {
                station_id = canonicalStationId,
                map_node_id = mapNodeId.Trim(),
                condition = boundedCondition,
                is_operational = boundedCondition > 0f
            });
            OnStateChanged?.Invoke();
            return true;
        }

        public HeliographStationState? GetStation(string stationId) => FindStation(stationId);

        public bool SetStationCondition(string stationId, float condition)
        {
            var station = FindStation(stationId);
            if (station == null) return false;
            station.condition = SanitizeRange(condition, 0f, 100f);
            station.is_operational = station.condition > 0f;
            OnStateChanged?.Invoke();
            return true;
        }

        public void LoadCatalog(HeliographCatalog catalog)
        {
            if (catalog == null) return;
            if (catalog.stations == null) return;
            foreach (var station in catalog.stations)
            {
                if (station == null) continue;
                RegisterStation(station.station_id, station.map_node_id, station.condition);
            }
        }

        public ActionResult Transmit(
            string messageId,
            string originStationId,
            string targetStationId,
            string payloadKey,
            int day,
            string revealLocationId = "",
            string distressSignalId = "")
        {
            if (string.IsNullOrWhiteSpace(messageId) || string.IsNullOrWhiteSpace(payloadKey))
                return ActionResult.Blocked("invalid_message", "heliograph.invalid_message");
            string canonicalMessageId = messageId.Trim();
            if (FindMessage(canonicalMessageId) != null)
                return ActionResult.Blocked("message_already_recorded", "heliograph.message_already_recorded");

            var message = new HeliographMessageState
            {
                message_id = canonicalMessageId,
                origin_station_id = originStationId?.Trim() ?? string.Empty,
                target_station_id = targetStationId?.Trim() ?? string.Empty,
                payload_key = payloadKey.Trim(),
                reveal_location_id = revealLocationId ?? string.Empty,
                distress_signal_id = distressSignalId ?? string.Empty,
                transmitted_day = day
            };
            _state.messages.Add(message);

            string blockReason = ValidateTransmission(message);
            if (!string.IsNullOrEmpty(blockReason))
            {
                message.status = (int)HeliographMessageStatus.Blocked;
                message.block_reason = blockReason;
                OnMessageBlocked?.Invoke(message);
                OnStateChanged?.Invoke();
                return ActionResult.Blocked(blockReason, "heliograph.transmission_blocked");
            }

            bool dispatchOk = true;
            if (!string.IsNullOrEmpty(message.distress_signal_id) && _dispatchDistress != null)
                dispatchOk = _dispatchDistress(message.distress_signal_id);
            if (!dispatchOk)
            {
                message.status = (int)HeliographMessageStatus.Blocked;
                message.block_reason = "distress_dispatch_refused";
                OnMessageBlocked?.Invoke(message);
                OnStateChanged?.Invoke();
                return ActionResult.Blocked("distress_dispatch_refused", "heliograph.dispatch_refused");
            }

            message.status = (int)HeliographMessageStatus.Delivered;
            _state.delivered_count++;
            if (!string.IsNullOrEmpty(message.reveal_location_id)
                && (_isMapNodeKnown == null || !_isMapNodeKnown(message.reveal_location_id)))
            {
                _discoverMapNode?.Invoke(message.reveal_location_id);
            }

            OnMessageDelivered?.Invoke(message);
            OnStateChanged?.Invoke();
            return ActionResult.Success("heliograph.transmitted");
        }

        public HeliographState CaptureState() => CloneState(_state);

        public void RestoreState(HeliographState saved)
        {
            if (saved == null) return;
            _state = CloneState(saved);
        }

        private string ValidateTransmission(HeliographMessageState message)
        {
            var origin = FindStation(message.origin_station_id);
            var target = FindStation(message.target_station_id);
            if (origin == null || target == null) return "unknown_station";
            if (!origin.is_operational || !target.is_operational) return "station_offline";
            if (origin.condition < 20f || target.condition < 20f) return "station_degraded";
            if (_hasLineOfSight != null && !_hasLineOfSight(origin.map_node_id, target.map_node_id))
                return "line_of_sight_blocked";
            float visibility = _visibility01 == null ? 1f : _visibility01();
            if (float.IsNaN(visibility) || float.IsInfinity(visibility))
                return "weather_visibility_blocked";
            visibility = Math.Clamp(visibility, 0f, 1f);
            if (visibility < MinimumVisibility01) return "weather_visibility_blocked";
            return string.Empty;
        }

        private HeliographStationState? FindStation(string stationId)
        {
            if (string.IsNullOrWhiteSpace(stationId)) return null;
            string canonicalId = stationId.Trim();
            for (int i = 0; i < _state.stations.Count; i++)
            {
                var station = _state.stations[i];
                if (station != null
                    && string.Equals(station.station_id, canonicalId, StringComparison.OrdinalIgnoreCase))
                    return station;
            }
            return null;
        }

        private HeliographMessageState? FindMessage(string messageId)
        {
            if (string.IsNullOrWhiteSpace(messageId)) return null;
            string canonicalId = messageId.Trim();
            for (int i = 0; i < _state.messages.Count; i++)
            {
                var message = _state.messages[i];
                if (message != null
                    && string.Equals(message.message_id, canonicalId, StringComparison.OrdinalIgnoreCase))
                    return message;
            }
            return null;
        }

        private static HeliographState CloneState(HeliographState? source)
        {
            var copy = new HeliographState
            {
                system_id = string.IsNullOrWhiteSpace(source?.system_id)
                    ? HeliographSystem.SystemId
                    : source!.system_id,
                delivered_count = 0,
                stations = new List<HeliographStationState>(),
                messages = new List<HeliographMessageState>()
            };
            if (source == null) return copy;

            var stationIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (source.stations != null)
            {
                foreach (var station in source.stations)
                {
                    if (station == null || string.IsNullOrWhiteSpace(station.station_id)
                        || string.IsNullOrWhiteSpace(station.map_node_id)
                        || !stationIds.Add(station.station_id.Trim())) continue;
                    float condition = SanitizeRange(station.condition, 0f, 100f);
                    copy.stations.Add(new HeliographStationState
                    {
                        station_id = station.station_id.Trim(),
                        map_node_id = station.map_node_id.Trim(),
                        condition = condition,
                        is_operational = station.is_operational && condition > 0f
                    });
                }
            }

            var messageIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (source.messages != null)
            {
                foreach (var message in source.messages)
                {
                    if (message == null || string.IsNullOrWhiteSpace(message.message_id)
                        || string.IsNullOrWhiteSpace(message.payload_key)
                        || !messageIds.Add(message.message_id.Trim())) continue;
                    var status = Enum.IsDefined(typeof(HeliographMessageStatus), message.status)
                        ? (HeliographMessageStatus)message.status
                        : HeliographMessageStatus.Pending;
                    copy.messages.Add(new HeliographMessageState
                    {
                        message_id = message.message_id.Trim(),
                        origin_station_id = message.origin_station_id?.Trim() ?? string.Empty,
                        target_station_id = message.target_station_id?.Trim() ?? string.Empty,
                        payload_key = message.payload_key.Trim(),
                        reveal_location_id = message.reveal_location_id?.Trim() ?? string.Empty,
                        distress_signal_id = message.distress_signal_id?.Trim() ?? string.Empty,
                        transmitted_day = message.transmitted_day,
                        status = (int)status,
                        block_reason = message.block_reason ?? string.Empty
                    });
                    if (status == HeliographMessageStatus.Delivered)
                        copy.delivered_count++;
                }
            }
            return copy;
        }

        private static float SanitizeRange(float value, float min, float max)
        {
            if (float.IsNaN(value) || float.IsInfinity(value)) return min;
            return Math.Clamp(value, min, max);
        }
    }

    public static class HeliographCatalogLoader
    {
        public const string FileName = "heliograph.json";

        public static HeliographCatalog Load(
            string directory,
            IFileIO fileIO,
            IJsonSerializer serializer,
            ILog? log = null)
        {
            var catalog = new HeliographCatalog();
            if (fileIO == null || serializer == null || string.IsNullOrEmpty(directory))
                return catalog;

            string path = fileIO.Combine(directory, FileName);
            if (!fileIO.FileExists(path)) return catalog;
            try
            {
                return serializer.Deserialize<HeliographCatalog>(fileIO.ReadAllText(path))
                    ?? catalog;
            }
            catch (Exception ex)
            {
                (log ?? NullLog.Instance).Warn($"[HeliographCatalog] failed to load {path}: {ex.Message}");
                return catalog;
            }
        }
    }
}
