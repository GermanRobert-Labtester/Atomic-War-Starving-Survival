using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Radio
{
    public enum NvisCommunicationsMode
    {
        Offline,
        Listening,
        Transmitting,
        AwaitingReply,
        RecallPending
    }

    [Serializable]
    public sealed class NvisChannelDefinition
    {
        public string channel_id = string.Empty;
        public string display_name = string.Empty;
        public int frequency_khz;
        public float range_km = 80f;
        public float base_signal_quality = 0.75f;
        public float required_power_watts = 150f;
        public bool recall_capable = true;
        public bool night_favorable;
    }

    [Serializable]
    public sealed class NvisCommunicationsCatalog
    {
        public int schema_version = 1;
        public List<NvisChannelDefinition> channels = new List<NvisChannelDefinition>();
    }

    public static class NvisCommunicationsCatalogLoader
    {
        public const string FileName = "nvis_communications_catalog.json";

        public static NvisCommunicationsCatalog Load(
            string dataDir,
            IFileIO fileIO,
            IJsonSerializer json,
            ILog? log = null)
        {
            if (fileIO == null || json == null || string.IsNullOrWhiteSpace(dataDir))
                return new NvisCommunicationsCatalog();

            string path = fileIO.Combine(dataDir, FileName);
            if (!fileIO.FileExists(path))
            {
                log?.Warn($"[NVIS] catalog not found at {path}");
                return new NvisCommunicationsCatalog();
            }

            try
            {
                return json.Deserialize<NvisCommunicationsCatalog>(fileIO.ReadAllText(path))
                    ?? new NvisCommunicationsCatalog();
            }
            catch (Exception ex)
            {
                log?.Error($"[NVIS] failed loading catalog: {ex.Message}");
                return new NvisCommunicationsCatalog();
            }
        }
    }

    [Serializable]
    public sealed class NvisTransmissionRecord
    {
        public string transmission_id = string.Empty;
        public string channel_id = string.Empty;
        public string message_kind = string.Empty;
        public string payload = string.Empty;
        public int sent_day;
        public bool delivered;
        public float signal_quality01;
    }

    [Serializable]
    public sealed class NvisRecallRequest
    {
        public string request_id = string.Empty;
        public string survivor_id = string.Empty;
        public int requested_day;
        public bool acknowledged;
        public string result_code = string.Empty;
    }

    [Serializable]
    public sealed class NvisCommunicationsState
    {
        public const int CurrentVersion = 1;
        public int version = CurrentVersion;
        public string system_id = NvisCommunicationsSystem.SystemId;
        public bool installed = true;
        public bool powered = true;
        public NvisCommunicationsMode mode = NvisCommunicationsMode.Offline;
        public string selected_channel_id = string.Empty;
        public float signal_quality01;
        public int last_contact_day = -1;
        public int total_transmissions;
        public int delivered_transmissions;
        public string active_transmission_id = string.Empty;
        public List<NvisTransmissionRecord> transmissions = new List<NvisTransmissionRecord>();
        public List<NvisRecallRequest> recall_requests = new List<NvisRecallRequest>();
    }

    /// <summary>
    /// Regional near-vertical-incidence communications authority. It models
    /// signal quality, power, channel selection, status broadcasts, and a
    /// bounded recall request queue. Expedition phase changes remain owned by
    /// ExpeditionSystem; the host acknowledges a request through that API.
    /// </summary>
    public class NvisCommunicationsSystem
    {
        public const string SystemId = "nvis_communications";

        private readonly ISeededRng _rng;
        private readonly Func<float> _availablePowerWatts;
        private readonly ILog _log;
        private readonly Dictionary<string, NvisChannelDefinition> _channels =
            new Dictionary<string, NvisChannelDefinition>(StringComparer.Ordinal);
        private NvisCommunicationsState _state = new NvisCommunicationsState();

        public NvisCommunicationsState State => _state;
        public IReadOnlyDictionary<string, NvisChannelDefinition> Channels => _channels;
        public event Action? OnStateChanged;
        public event Action<NvisTransmissionRecord>? OnTransmissionCompleted;
        public event Action<NvisRecallRequest>? OnRecallRequested;

        public NvisCommunicationsSystem(
            ISeededRng? rng = null,
            Func<float>? availablePowerWatts = null,
            ILog? log = null)
        {
            _rng = rng ?? new SeededRng(131);
            _availablePowerWatts = availablePowerWatts ?? (() => float.MaxValue);
            _log = log ?? NullLog.Instance;
        }

        public void LoadCatalog(NvisCommunicationsCatalog catalog)
        {
            _channels.Clear();
            foreach (var channel in catalog?.channels ?? new List<NvisChannelDefinition>())
            {
                if (channel == null || string.IsNullOrEmpty(channel.channel_id)) continue;
                _channels[channel.channel_id] = channel;
            }
            if (string.IsNullOrEmpty(_state.selected_channel_id) && _channels.Count > 0)
                _state.selected_channel_id = _channels.Keys.OrderBy(id => id, StringComparer.Ordinal).First();
            OnStateChanged?.Invoke();
        }

        public NvisChannelDefinition? GetChannel(string channelId)
            => _channels.TryGetValue(channelId ?? string.Empty, out var channel) ? channel : null;

        public ActionResult SetPowered(bool powered)
        {
            _state.powered = powered;
            if (!powered) _state.mode = NvisCommunicationsMode.Offline;
            else if (_state.mode == NvisCommunicationsMode.Offline) _state.mode = NvisCommunicationsMode.Listening;
            OnStateChanged?.Invoke();
            return ActionResult.Success(powered ? "nvis.powered_on" : "nvis.powered_off");
        }

        public ActionResult SelectChannel(string channelId)
        {
            if (!installed) return ActionResult.Blocked("not_installed", "nvis.not_installed");
            if (!_channels.ContainsKey(channelId ?? string.Empty))
                return ActionResult.Failed("unknown_channel", "nvis.unknown_channel");
            if (_state.mode == NvisCommunicationsMode.Transmitting)
                return ActionResult.Blocked("busy", "nvis.busy");
            _state.selected_channel_id = channelId;
            _state.mode = _state.powered ? NvisCommunicationsMode.Listening : NvisCommunicationsMode.Offline;
            OnStateChanged?.Invoke();
            return ActionResult.Success("nvis.channel_selected");
        }

        public ActionResult BeginStatusTransmission(string payload, int day, int activeExpeditionCount)
        {
            if (!_state.installed) return ActionResult.Blocked("not_installed", "nvis.not_installed");
            if (!_state.powered) return ActionResult.Blocked("power_off", "nvis.power_off");
            var channel = GetChannel(_state.selected_channel_id);
            if (channel == null) return ActionResult.Blocked("no_channel", "nvis.no_channel");
            if (_availablePowerWatts() < channel.required_power_watts)
                return ActionResult.Blocked("insufficient_power", "nvis.insufficient_power");
            if (_state.mode == NvisCommunicationsMode.Transmitting)
                return ActionResult.Blocked("busy", "nvis.busy");

            float activityPenalty = Math.Clamp(activeExpeditionCount * 0.03f, 0f, 0.25f);
            _state.signal_quality01 = Math.Clamp(channel.base_signal_quality - activityPenalty, 0f, 1f);
            _state.active_transmission_id = $"nvis_{day}_{_state.total_transmissions + 1}";
            _state.mode = NvisCommunicationsMode.Transmitting;
            _state.total_transmissions++;
            OnStateChanged?.Invoke();
            return ActionResult.Success("nvis.transmission_started");
        }

        public ActionResult RequestRecall(string survivorId, int day)
        {
            if (!_state.powered) return ActionResult.Blocked("power_off", "nvis.power_off");
            var channel = GetChannel(_state.selected_channel_id);
            if (channel == null || !channel.recall_capable)
                return ActionResult.Blocked("recall_unavailable", "nvis.recall_unavailable");
            if (string.IsNullOrEmpty(survivorId))
                return ActionResult.Failed("invalid_survivor", "nvis.invalid_survivor");
            if (_state.recall_requests.Any(r => r != null && r.survivor_id == survivorId && !r.acknowledged))
                return ActionResult.Blocked("recall_already_requested", "nvis.recall_already_requested");

            var request = new NvisRecallRequest
            {
                request_id = $"recall_{day}_{_state.recall_requests.Count + 1}",
                survivor_id = survivorId,
                requested_day = day
            };
            _state.recall_requests.Add(request);
            _state.mode = NvisCommunicationsMode.RecallPending;
            OnRecallRequested?.Invoke(request);
            OnStateChanged?.Invoke();
            return ActionResult.Success("nvis.recall_requested");
        }

        public bool AcknowledgeRecall(string survivorId, string resultCode = "acknowledged")
        {
            var request = _state.recall_requests.LastOrDefault(r =>
                r != null && r.survivor_id == survivorId && !r.acknowledged);
            if (request == null) return false;
            request.acknowledged = true;
            request.result_code = resultCode ?? "acknowledged";
            if (!_state.recall_requests.Any(r => r != null && !r.acknowledged))
                _state.mode = _state.powered ? NvisCommunicationsMode.Listening : NvisCommunicationsMode.Offline;
            OnStateChanged?.Invoke();
            return true;
        }

        public void TickDay(int day)
        {
            if (_state.mode == NvisCommunicationsMode.Transmitting)
            {
                var channel = GetChannel(_state.selected_channel_id);
                bool delivered = channel != null && _state.powered
                    && _availablePowerWatts() >= channel.required_power_watts
                    && _rng.NextDouble() <= Math.Clamp(_state.signal_quality01, 0.1f, 1f);
                var record = new NvisTransmissionRecord
                {
                    transmission_id = _state.active_transmission_id,
                    channel_id = _state.selected_channel_id,
                    message_kind = "regional_status",
                    payload = "expedition_status",
                    sent_day = day,
                    delivered = delivered,
                    signal_quality01 = _state.signal_quality01
                };
                _state.transmissions.Add(record);
                _state.active_transmission_id = string.Empty;
                _state.last_contact_day = delivered ? day : _state.last_contact_day;
                if (delivered) _state.delivered_transmissions++;
                _state.mode = delivered ? NvisCommunicationsMode.AwaitingReply : NvisCommunicationsMode.Listening;
                _log.Info($"[NVIS] status transmission {(delivered ? "delivered" : "lost")} on day {day}");
                OnTransmissionCompleted?.Invoke(record);
            }
            OnStateChanged?.Invoke();
        }

        public NvisCommunicationsState CaptureState()
        {
            var serializer = new SystemTextJsonSerializer();
            return serializer.Deserialize<NvisCommunicationsState>(serializer.Serialize(_state))
                ?? new NvisCommunicationsState();
        }

        public void RestoreState(NvisCommunicationsState? state)
        {
            if (state == null) return;
            var serializer = new SystemTextJsonSerializer();
            _state = serializer.Deserialize<NvisCommunicationsState>(serializer.Serialize(state))
                ?? new NvisCommunicationsState();
            _state.transmissions ??= new List<NvisTransmissionRecord>();
            _state.recall_requests ??= new List<NvisRecallRequest>();
            OnStateChanged?.Invoke();
        }

        private bool installed => _state.installed;
    }

    public class NvisC4ISystem : NvisCommunicationsSystem
    {
        public NvisC4ISystem(ISeededRng? rng = null, Func<float>? availablePowerWatts = null, ILog? log = null)
            : base(rng, availablePowerWatts, log) { }
    }
}
