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
            if (string.IsNullOrEmpty(stationId) || string.IsNullOrEmpty(mapNodeId))
                return false;
            if (FindStation(stationId) != null) return false;

            _state.stations.Add(new HeliographStationState
            {
                station_id = stationId,
                map_node_id = mapNodeId,
                condition = Math.Clamp(condition, 0f, 100f),
                is_operational = condition > 0f
            });
            OnStateChanged?.Invoke();
            return true;
        }

        public HeliographStationState? GetStation(string stationId) => FindStation(stationId);

        public bool SetStationCondition(string stationId, float condition)
        {
            var station = FindStation(stationId);
            if (station == null) return false;
            station.condition = Math.Clamp(condition, 0f, 100f);
            station.is_operational = station.condition > 0f;
            OnStateChanged?.Invoke();
            return true;
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
            if (string.IsNullOrEmpty(messageId) || string.IsNullOrEmpty(payloadKey))
                return ActionResult.Blocked("invalid_message", "heliograph.invalid_message");
            if (FindMessage(messageId) != null)
                return ActionResult.Blocked("message_already_recorded", "heliograph.message_already_recorded");

            var message = new HeliographMessageState
            {
                message_id = messageId,
                origin_station_id = originStationId ?? string.Empty,
                target_station_id = targetStationId ?? string.Empty,
                payload_key = payloadKey,
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
            float visibility = _visibility01 == null ? 1f : Math.Clamp(_visibility01(), 0f, 1f);
            if (visibility < MinimumVisibility01) return "weather_visibility_blocked";
            return string.Empty;
        }

        private HeliographStationState? FindStation(string stationId)
        {
            if (string.IsNullOrEmpty(stationId)) return null;
            for (int i = 0; i < _state.stations.Count; i++)
                if (_state.stations[i].station_id == stationId) return _state.stations[i];
            return null;
        }

        private HeliographMessageState? FindMessage(string messageId)
        {
            for (int i = 0; i < _state.messages.Count; i++)
                if (_state.messages[i].message_id == messageId) return _state.messages[i];
            return null;
        }

        private static HeliographState CloneState(HeliographState src)
        {
            if (src == null) return new HeliographState();
            var serializer = new SystemTextJsonSerializer();
            string json = serializer.Serialize(src);
            return serializer.Deserialize<HeliographState>(json) ?? new HeliographState();
        }
    }
}
