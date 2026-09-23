// SPDX-License-Identifier: MIT
// ============================================================================
// Ashfall Core : Plan 157 — Communications & Radio Network Infrastructure
// Subsystem    : CommunicationsSystem / Antennas, Networks, Signals & Interception
// Authority    : Next-steps-plans/Plan_157_Communications_Radio_Network_Infrastructure.md
//                UNBLOCK-PROGRAM-WAVE32-BATCH6-PLANS (DEC-153)
// ============================================================================
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Communications
{
    public enum AntennaType
    {
        BasicWhip       = 0,
        DirectionalYagi = 1,
        ParabolicDish   = 2,
        PhasedArray     = 3
    }

    public enum NetworkStatus
    {
        Active  = 0,
        Jammed  = 1,
        Offline = 2
    }

    public sealed class AntennaDto
    {
        public string AntennaId { get; set; } = string.Empty;
        public AntennaType Type { get; set; } = AntennaType.BasicWhip;
        public string Name { get; set; } = string.Empty;
        public double RangeKm { get; set; } = 8.0;
        public int Sensitivity { get; set; } = 40; // 0..100
        public int MaxChannels { get; set; } = 2;
        public int PowerDrawWatts { get; set; } = 25;
        public double Condition { get; set; } = 100.0; // 0..100
        public bool IsActive { get; set; } = true;

        public AntennaDto Clone()
        {
            return new AntennaDto
            {
                AntennaId = AntennaId,
                Type = Type,
                Name = Name,
                RangeKm = RangeKm,
                Sensitivity = Sensitivity,
                MaxChannels = MaxChannels,
                PowerDrawWatts = PowerDrawWatts,
                Condition = Condition,
                IsActive = IsActive
            };
        }
    }

    public sealed class CommunicationsNetworkDto
    {
        public string NetworkId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public double FrequencyMhz { get; set; } = 144.2;
        public int EncryptionLevel { get; set; } = 20; // 0..100
        public string FactionId { get; set; } = "player_shelter";
        public bool IsPlayerOwned { get; set; } = true;
        public NetworkStatus Status { get; set; } = NetworkStatus.Active;

        public CommunicationsNetworkDto Clone()
        {
            return new CommunicationsNetworkDto
            {
                NetworkId = NetworkId,
                Name = Name,
                FrequencyMhz = FrequencyMhz,
                EncryptionLevel = EncryptionLevel,
                FactionId = FactionId,
                IsPlayerOwned = IsPlayerOwned,
                Status = Status
            };
        }
    }

    public sealed class InterceptedMessageDto
    {
        public string MessageId { get; set; } = string.Empty;
        public string SourceFactionId { get; set; } = string.Empty;
        public double FrequencyMhz { get; set; }
        public int EncryptionLevel { get; set; }
        public string RawText { get; set; } = string.Empty;
        public string DecodedContent { get; set; } = string.Empty;
        public int InterceptDay { get; set; }
        public bool IsDecoded { get; set; }
        public int IntelligenceValue { get; set; }

        public InterceptedMessageDto Clone()
        {
            return new InterceptedMessageDto
            {
                MessageId = MessageId,
                SourceFactionId = SourceFactionId,
                FrequencyMhz = FrequencyMhz,
                EncryptionLevel = EncryptionLevel,
                RawText = RawText,
                DecodedContent = DecodedContent,
                InterceptDay = InterceptDay,
                IsDecoded = IsDecoded,
                IntelligenceValue = IntelligenceValue
            };
        }
    }

    public sealed class OutgoingBroadcastDto
    {
        public string BroadcastId { get; set; } = string.Empty;
        public double FrequencyMhz { get; set; }
        public string Content { get; set; } = string.Empty;
        public int EncryptionLevel { get; set; }
        public int BroadcastDay { get; set; }
        public double AudienceReachKm { get; set; }

        public OutgoingBroadcastDto Clone()
        {
            return new OutgoingBroadcastDto
            {
                BroadcastId = BroadcastId,
                FrequencyMhz = FrequencyMhz,
                Content = Content,
                EncryptionLevel = EncryptionLevel,
                BroadcastDay = BroadcastDay,
                AudienceReachKm = AudienceReachKm
            };
        }
    }

    public sealed class CommunicationsState
    {
        public int SchemaVersion { get; set; } = 1;
        public int TotalMessagesDecoded { get; set; }
        public int TotalBroadcastsSent { get; set; }
        public List<AntennaDto> Antennas { get; set; } = new();
        public List<CommunicationsNetworkDto> Networks { get; set; } = new();
        public List<InterceptedMessageDto> InterceptedMessages { get; set; } = new();
        public List<OutgoingBroadcastDto> OutgoingBroadcasts { get; set; } = new();
    }

    /// <summary>
    /// Pure domain authority managing antenna infrastructure, frequency tuning,
    /// signal interception, cryptanalysis, and two-way broadcast operations.
    /// Zero engine dependencies; deterministic evaluation.
    /// </summary>
    public sealed class CommunicationsSystem
    {
        private readonly Dictionary<string, AntennaDto> _antennas = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, CommunicationsNetworkDto> _networks = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, InterceptedMessageDto> _interceptedMessages = new(StringComparer.OrdinalIgnoreCase);
        private readonly List<OutgoingBroadcastDto> _broadcasts = new();

        public int TotalMessagesDecoded { get; private set; }
        public int TotalBroadcastsSent { get; private set; }

        // Seams for presentation, audio and event logging
        public Action<AntennaDto>? OnAntennaInstalledSeam { get; set; }
        public Action<InterceptedMessageDto>? OnMessageInterceptedSeam { get; set; }
        public Action<InterceptedMessageDto>? OnMessageDecodedSeam { get; set; }
        public Action<OutgoingBroadcastDto>? OnBroadcastTransmittedSeam { get; set; }
        public Action<double, int>? OnFrequencyJammedSeam { get; set; }

        public CommunicationsSystem()
        {
            LoadEmbeddedDefaults();
        }

        public void LoadCatalog(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return;

            try
            {
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                if (root.TryGetProperty("default_networks", out var netElem) && netElem.ValueKind == JsonValueKind.Array)
                {
                    _networks.Clear();
                    foreach (var item in netElem.EnumerateArray())
                    {
                        var netId = item.GetProperty("network_id").GetString() ?? string.Empty;
                        var name = item.GetProperty("name").GetString() ?? string.Empty;
                        var freq = item.GetProperty("frequency_mhz").GetDouble();
                        var enc = item.GetProperty("encryption_level").GetInt32();
                        var facId = item.GetProperty("faction_id").GetString() ?? string.Empty;
                        var isPlayer = item.GetProperty("is_player_owned").GetBoolean();

                        _networks[netId] = new CommunicationsNetworkDto
                        {
                            NetworkId = netId,
                            Name = name,
                            FrequencyMhz = freq,
                            EncryptionLevel = enc,
                            FactionId = facId,
                            IsPlayerOwned = isPlayer,
                            Status = NetworkStatus.Active
                        };
                    }
                }
            }
            catch
            {
                LoadEmbeddedDefaults();
            }
        }

        private void LoadEmbeddedDefaults()
        {
            _networks.Clear();
            _networks["net_shelter_internal"] = new CommunicationsNetworkDto
            {
                NetworkId = "net_shelter_internal",
                Name = "Shelter Primary Comm Loop",
                FrequencyMhz = 144.2,
                EncryptionLevel = 25,
                FactionId = "player_shelter",
                IsPlayerOwned = true,
                Status = NetworkStatus.Active
            };
            _networks["net_wasteland_emergency"] = new CommunicationsNetworkDto
            {
                NetworkId = "net_wasteland_emergency",
                Name = "Wasteland Civil Defense Guard",
                FrequencyMhz = 121.5,
                EncryptionLevel = 0,
                FactionId = "neutral",
                IsPlayerOwned = false,
                Status = NetworkStatus.Active
            };
            _networks["net_garrison_tactical"] = new CommunicationsNetworkDto
            {
                NetworkId = "net_garrison_tactical",
                Name = "Iron Garrison Secure Uplink",
                FrequencyMhz = 462.55,
                EncryptionLevel = 70,
                FactionId = "faction_garrison",
                IsPlayerOwned = false,
                Status = NetworkStatus.Active
            };

            // Start with a basic whip antenna
            InstallAntenna("ant_shelter_whip", AntennaType.BasicWhip, "Shelter Whip Antenna");
        }

        public AntennaDto InstallAntenna(string antennaId, AntennaType type, string? name = null)
        {
            double range = 8.0;
            int sensitivity = 40;
            int channels = 2;
            int power = 25;

            switch (type)
            {
                case AntennaType.DirectionalYagi:
                    range = 25.0;
                    sensitivity = 65;
                    channels = 4;
                    power = 60;
                    break;
                case AntennaType.ParabolicDish:
                    range = 60.0;
                    sensitivity = 85;
                    channels = 6;
                    power = 120;
                    break;
                case AntennaType.PhasedArray:
                    range = 120.0;
                    sensitivity = 95;
                    channels = 10;
                    power = 250;
                    break;
            }

            var antenna = new AntennaDto
            {
                AntennaId = antennaId,
                Type = type,
                Name = name ?? $"Antenna {type}",
                RangeKm = range,
                Sensitivity = sensitivity,
                MaxChannels = channels,
                PowerDrawWatts = power,
                Condition = 100.0,
                IsActive = true
            };

            _antennas[antennaId] = antenna;
            OnAntennaInstalledSeam?.Invoke(antenna);
            return antenna.Clone();
        }

        public bool RepairAntenna(string antennaId, double amount)
        {
            if (!_antennas.TryGetValue(antennaId, out var ant))
                return false;

            ant.Condition = Math.Clamp(ant.Condition + Math.Max(0.0, amount), 0.0, 100.0);
            return true;
        }

        public void DegradeAntennas(double wearAmount)
        {
            foreach (var ant in _antennas.Values)
            {
                ant.Condition = Math.Clamp(ant.Condition - Math.Max(0.0, wearAmount), 0.0, 100.0);
            }
        }

        public double GetEffectiveReceptionRangeKm()
        {
            double maxRange = 0.0;
            foreach (var ant in _antennas.Values)
            {
                if (ant.IsActive && ant.Condition > 20.0)
                {
                    if (ant.RangeKm > maxRange)
                        maxRange = ant.RangeKm;
                }
            }
            return maxRange;
        }

        public int GetEffectiveSensitivity()
        {
            int maxSens = 0;
            foreach (var ant in _antennas.Values)
            {
                if (ant.IsActive && ant.Condition > 20.0)
                {
                    if (ant.Sensitivity > maxSens)
                        maxSens = ant.Sensitivity;
                }
            }
            return maxSens;
        }

        public InterceptedMessageDto? InterceptFactionSignal(
            string factionId,
            ISeededRng rng,
            int currentDay)
        {
            if (rng == null)
                throw new ArgumentNullException(nameof(rng));

            double range = GetEffectiveReceptionRangeKm();
            int sensitivity = GetEffectiveSensitivity();

            if (range < 10.0 || sensitivity < 30)
                return null; // Insufficient communications infrastructure

            // Find or associate target network
            CommunicationsNetworkDto? targetNet = null;
            foreach (var net in _networks.Values)
            {
                if (net.FactionId.Equals(factionId, StringComparison.OrdinalIgnoreCase))
                {
                    targetNet = net;
                    break;
                }
            }

            double freq = targetNet?.FrequencyMhz ?? 462.55;
            int enc = targetNet?.EncryptionLevel ?? 60;

            string messageId = $"msg_{currentDay}_{_interceptedMessages.Count}";
            string raw = $"[SIG_INTERCEPT] Faction {factionId} comm on {freq.ToString("F2", CultureInfo.InvariantCulture)} MHz.";

            var message = new InterceptedMessageDto
            {
                MessageId = messageId,
                SourceFactionId = factionId,
                FrequencyMhz = freq,
                EncryptionLevel = enc,
                RawText = raw,
                DecodedContent = string.Empty,
                InterceptDay = currentDay,
                IsDecoded = (enc == 0),
                IntelligenceValue = Math.Max(25, enc + 10)
            };

            if (message.IsDecoded)
            {
                message.DecodedContent = raw;
                TotalMessagesDecoded++;
            }

            _interceptedMessages[messageId] = message;
            OnMessageInterceptedSeam?.Invoke(message);
            return message.Clone();
        }

        public bool DecodeMessage(string messageId, int cryptanalysisSkill, ISeededRng rng)
        {
            if (rng == null)
                throw new ArgumentNullException(nameof(rng));

            if (!_interceptedMessages.TryGetValue(messageId, out var msg))
                return false;

            if (msg.IsDecoded)
                return true;

            int roll = rng.Next(1, 101);
            int effectiveSkill = cryptanalysisSkill + roll;

            if (effectiveSkill >= msg.EncryptionLevel)
            {
                msg.IsDecoded = true;
                msg.DecodedContent = msg.RawText;
                TotalMessagesDecoded++;
                OnMessageDecodedSeam?.Invoke(msg);
                return true;
            }

            return false;
        }

        public OutgoingBroadcastDto? TransmitBroadcast(
            double frequencyMhz,
            string content,
            int encryptionLevel,
            int currentDay)
        {
            double range = GetEffectiveReceptionRangeKm();
            if (range <= 0.0)
                return null;

            string bcastId = $"bcast_{currentDay}_{_broadcasts.Count}";
            var broadcast = new OutgoingBroadcastDto
            {
                BroadcastId = bcastId,
                FrequencyMhz = frequencyMhz,
                Content = content ?? string.Empty,
                EncryptionLevel = Math.Clamp(encryptionLevel, 0, 100),
                BroadcastDay = currentDay,
                AudienceReachKm = range
            };

            _broadcasts.Add(broadcast);
            TotalBroadcastsSent++;
            OnBroadcastTransmittedSeam?.Invoke(broadcast);
            return broadcast.Clone();
        }

        public void JamFrequency(double frequencyMhz, int durationDays)
        {
            foreach (var net in _networks.Values)
            {
                if (Math.Abs(net.FrequencyMhz - frequencyMhz) < 0.1)
                {
                    net.Status = NetworkStatus.Jammed;
                }
            }

            OnFrequencyJammedSeam?.Invoke(frequencyMhz, durationDays);
        }

        public AntennaDto? GetAntenna(string antennaId)
        {
            if (_antennas.TryGetValue(antennaId, out var a))
                return a.Clone();
            return null;
        }

        public CommunicationsNetworkDto? GetNetwork(string networkId)
        {
            if (_networks.TryGetValue(networkId, out var n))
                return n.Clone();
            return null;
        }

        public InterceptedMessageDto? GetMessage(string messageId)
        {
            if (_interceptedMessages.TryGetValue(messageId, out var m))
                return m.Clone();
            return null;
        }

        public IReadOnlyList<AntennaDto> GetAllAntennas()
        {
            var list = new List<AntennaDto>(_antennas.Count);
            foreach (var a in _antennas.Values)
                list.Add(a.Clone());
            return list;
        }

        public IReadOnlyList<InterceptedMessageDto> GetAllInterceptedMessages()
        {
            var list = new List<InterceptedMessageDto>(_interceptedMessages.Count);
            foreach (var m in _interceptedMessages.Values)
                list.Add(m.Clone());
            return list;
        }

        public CommunicationsState CaptureState()
        {
            var state = new CommunicationsState
            {
                SchemaVersion = 1,
                TotalMessagesDecoded = TotalMessagesDecoded,
                TotalBroadcastsSent = TotalBroadcastsSent
            };
            foreach (var a in _antennas.Values)
                state.Antennas.Add(a.Clone());
            foreach (var n in _networks.Values)
                state.Networks.Add(n.Clone());
            foreach (var m in _interceptedMessages.Values)
                state.InterceptedMessages.Add(m.Clone());
            foreach (var b in _broadcasts)
                state.OutgoingBroadcasts.Add(b.Clone());
            return state;
        }

        public void RestoreState(CommunicationsState state)
        {
            if (state == null)
                return;

            TotalMessagesDecoded = Math.Max(0, state.TotalMessagesDecoded);
            TotalBroadcastsSent = Math.Max(0, state.TotalBroadcastsSent);

            _antennas.Clear();
            if (state.Antennas != null)
            {
                foreach (var a in state.Antennas)
                    _antennas[a.AntennaId] = a.Clone();
            }

            _networks.Clear();
            if (state.Networks != null)
            {
                foreach (var n in state.Networks)
                    _networks[n.NetworkId] = n.Clone();
            }

            _interceptedMessages.Clear();
            if (state.InterceptedMessages != null)
            {
                foreach (var m in state.InterceptedMessages)
                    _interceptedMessages[m.MessageId] = m.Clone();
            }

            _broadcasts.Clear();
            if (state.OutgoingBroadcasts != null)
            {
                foreach (var b in state.OutgoingBroadcasts)
                    _broadcasts.Add(b.Clone());
            }
        }
    }
}
