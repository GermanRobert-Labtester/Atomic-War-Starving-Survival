// SPDX-License-Identifier: MIT
// ASHFALL Core: deterministic faction intelligence and infiltration.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Ashfall.Core.IO;

namespace Ashfall.Core.Factions
{
    public enum EspionageMissionType
    {
        Infiltration,
        Surveillance,
        SupplyIntelligence,
        DefenseIntelligence,
        CommunicationsIntelligence,
        AbstractSabotage,
        Exfiltration,
        RescueSupport
    }

    public enum EspionageMissionStatus
    {
        Planned,
        Deploying,
        Operating,
        Compromised,
        Captured,
        Extracting,
        Returned,
        Failed,
        Turned
    }

    public enum DoubleAgentStatus
    {
        Stable,
        Compromised,
        Questioned,
        Turned,
        Exposed,
        Recovered
    }

    public enum IntelConfidence
    {
        Unknown,
        Rumor,
        Estimate,
        Confirmed,
        Penetrated
    }

    [Serializable]
    public sealed class EspionageMissionDef
    {
        [JsonPropertyName("id")] public string Id { get; set; } = string.Empty;
        [JsonPropertyName("display_name_key")] public string DisplayNameKey { get; set; } = string.Empty;
        [JsonPropertyName("mission_type")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EspionageMissionType MissionType { get; set; }
        [JsonPropertyName("target_tags")] public List<string> TargetTags { get; set; } = new List<string>();
        [JsonPropertyName("minimum_intel_level")] public int MinimumIntelLevel { get; set; }
        [JsonPropertyName("base_duration_days")] public int BaseDurationDays { get; set; } = 1;
        [JsonPropertyName("base_detection_chance")] public float BaseDetectionChance { get; set; } = 0.1f;
        [JsonPropertyName("intel_gain")] public int IntelGain { get; set; } = 1;
        [JsonPropertyName("network_exposure_gain")] public float NetworkExposureGain { get; set; } = 0.05f;
        [JsonPropertyName("standing_risk")] public float StandingRisk { get; set; } = 0.1f;
        [JsonPropertyName("intel_fact_ids")] public List<string> IntelFactIds { get; set; } = new List<string>();
        [JsonPropertyName("possible_consequence_ids")] public List<string> PossibleConsequenceIds { get; set; } = new List<string>();
        [JsonPropertyName("required_research_id")] public string RequiredResearchId { get; set; } = string.Empty;
        [JsonPropertyName("required_skill_profile")] public string RequiredSkillProfile { get; set; } = string.Empty;
        [JsonPropertyName("tags")] public List<string> Tags { get; set; } = new List<string>();
    }

    [Serializable]
    public sealed class EspionageMissionCatalog
    {
        [JsonPropertyName("schema_version")] public int SchemaVersion { get; set; } = 1;
        [JsonPropertyName("missions")] public List<EspionageMissionDef> Missions { get; set; } = new List<EspionageMissionDef>();
    }

    public static class EspionageMissionCatalogLoader
    {
        public const string FileName = "espionage_missions.json";

        public static List<EspionageMissionDef> Load(string dataDir, IFileIO files, IJsonSerializer serializer)
        {
            var result = new List<EspionageMissionDef>();
            if (files == null || serializer == null || string.IsNullOrWhiteSpace(dataDir)) return result;
            string path = files.Combine(dataDir, FileName);
            if (!files.FileExists(path)) return result;
            string raw = files.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw)) return result;

            try
            {
                var catalog = JsonSerializer.Deserialize<EspionageMissionCatalog>(raw, SystemTextJsonSerializer.Options);
                var seen = new HashSet<string>(StringComparer.Ordinal);
                foreach (var mission in catalog?.Missions ?? new List<EspionageMissionDef>())
                {
                    if (mission != null && !string.IsNullOrWhiteSpace(mission.Id) && seen.Add(mission.Id))
                        result.Add(mission);
                }
            }
            catch (Exception ex)
            {
                CatalogDiagnostics.Warn(FileName, "EspionageMissionCatalogLoader", ex);
            }
            return result;
        }

        public static bool Validate(IEnumerable<EspionageMissionDef> missions, out string error)
        {
            error = string.Empty;
            var seen = new HashSet<string>(StringComparer.Ordinal);
            foreach (var mission in missions ?? Enumerable.Empty<EspionageMissionDef>())
            {
                if (mission == null || string.IsNullOrWhiteSpace(mission.Id))
                {
                    error = "Espionage mission has an empty id.";
                    return false;
                }
                if (!seen.Add(mission.Id))
                {
                    error = "Duplicate espionage mission id: " + mission.Id;
                    return false;
                }
                if (mission.BaseDurationDays <= 0 || mission.IntelGain < 0
                    || mission.BaseDetectionChance < 0f || mission.BaseDetectionChance > 1f
                    || mission.NetworkExposureGain < 0f || mission.NetworkExposureGain > 1f
                    || mission.StandingRisk < 0f || mission.StandingRisk > 1f)
                {
                    error = "Espionage mission has an invalid duration, gain, or probability: " + mission.Id;
                    return false;
                }
            }
            return true;
        }
    }

    [Serializable]
    public sealed class SpyNetworkState
    {
        public string networkId = string.Empty;
        public string targetFactionId = string.Empty;
        public string infiltratorId = string.Empty;
        public int establishedDay;
        public float coverStrength01 = 0.5f;
        public int intelLevel;
        public float networkExposure01;
        public float counterIntelPressure01;
        public float loyaltyRisk01;
        public EspionageMissionStatus status = EspionageMissionStatus.Deploying;
        public List<string> knownStrongholdIds = new List<string>();
        public List<string> knownCapabilityFacts = new List<string>();
        public DoubleAgentStatus doubleAgentStatus = DoubleAgentStatus.Stable;
    }

    [Serializable]
    public sealed class EspionageMissionState
    {
        public string missionInstanceId = string.Empty;
        public string missionDefinitionId = string.Empty;
        public string networkId = string.Empty;
        public int startedDay;
        public int lastTickDay;
        public int progressDays;
        public EspionageMissionStatus status = EspionageMissionStatus.Deploying;
    }

    [Serializable]
    public sealed class EspionageIntelFactState
    {
        public string factId = string.Empty;
        public string factionId = string.Empty;
        public IntelConfidence confidence;
        public int discoveredDay;
        public int lastConfirmedDay;
    }

    [Serializable]
    public sealed class CapturedAgentState
    {
        public string agentId = string.Empty;
        public string networkId = string.Empty;
        public string targetFactionId = string.Empty;
        public int captureDay;
        public int executionDeadlineDay;
        public string status = "captured";
    }

    [Serializable]
    public sealed class EspionageFactionSecurityState
    {
        public string factionId = string.Empty;
        public float paranoia01;
        public float counterIntel01;
        public int recentIntrusions;
        public int lastIncidentDay;
    }

    [Serializable]
    public sealed class EspionageState
    {
        public string systemId = EspionageSystem.SystemId;
        public List<SpyNetworkState> networks = new List<SpyNetworkState>();
        public List<EspionageMissionState> activeMissions = new List<EspionageMissionState>();
        public List<CapturedAgentState> capturedAgents = new List<CapturedAgentState>();
        public List<EspionageIntelFactState> knownIntel = new List<EspionageIntelFactState>();
        public List<EspionageMissionState> missionHistory = new List<EspionageMissionState>();
        public List<EspionageFactionSecurityState> factionSecurity = new List<EspionageFactionSecurityState>();
    }

    [Serializable]
    public sealed class EspionageMissionPreview
    {
        public string missionId = string.Empty;
        public string canonicalFactionId = string.Empty;
        public int durationDays;
        public float detectionChance;
        public float coverStrength01;
        public bool isAvailable;
        public string failureCode = string.Empty;
    }

    [Serializable]
    public sealed class EspionageConsequenceIntent
    {
        public string incidentId = string.Empty;
        public string targetFactionId = string.Empty;
        public string consequenceType = string.Empty;
        public float magnitude;
        public int startDay;
        public int expiryDay;
    }

    /// <summary>
    /// Owns spy networks, mission risk, intel confidence, and captured-agent
    /// state. FactionWarSystem remains the authority for faction standing and
    /// territory; this system only emits bounded consequence intents.
    /// </summary>
    public sealed class EspionageSystem
    {
        public const string SystemId = "espionage";

        private EspionageState _state;
        private readonly Dictionary<string, EspionageMissionDef> _catalog =
            new Dictionary<string, EspionageMissionDef>(StringComparer.Ordinal);
        private readonly ILog _log;
        private ISeededRng _rng;
        private Func<string, bool> _isAgentAlive = _ => true;
        private Func<string, bool> _isAgentAvailable = _ => true;
        private Func<string, float> _getAgentCapability = _ => 1f;
        private Action<string, bool> _setAgentAway = (_, __) => { };
        private Func<string, bool> _isResearchComplete = _ => false;
        private Func<string, string> _canonicalizeFaction = FactionStandingIdResolver.ToSystemsId;
        private int _sequence;

        public EspionageState State => _state;
        public IReadOnlyDictionary<string, EspionageMissionDef> Catalog => _catalog;

        public event Action<EspionageIntelFactState>? OnIntelDiscovered;
        public event Action<EspionageConsequenceIntent>? OnConsequenceIntent;
        public event Action<CapturedAgentState>? OnAgentCaptured;
        public event Action? OnStateChanged;

        public EspionageSystem(ILog? log = null, EspionageState? state = null)
        {
            _log = log ?? NullLog.Instance;
            _state = state ?? new EspionageState();
            _rng = new SeededRng(167);
        }

        public void BindRng(ISeededRng rng) => _rng = rng ?? new SeededRng(167);
        public void BindAgentAvailability(Func<string, bool> isAlive, Func<string, bool> isAvailable, Action<string, bool>? setAway = null)
        {
            _isAgentAlive = isAlive ?? (_ => true);
            _isAgentAvailable = isAvailable ?? (_ => true);
            _setAgentAway = setAway ?? ((_, __) => { });
        }
        public void BindAgentCapability(Func<string, float> getCapability) => _getAgentCapability = getCapability ?? (_ => 1f);
        public void BindResearchGate(Func<string, bool> isComplete) => _isResearchComplete = isComplete ?? (_ => false);
        public void BindFactionResolver(Func<string, string> resolver) => _canonicalizeFaction = resolver ?? FactionStandingIdResolver.ToSystemsId;

        public void LoadMissionCatalog(IEnumerable<EspionageMissionDef> missions)
        {
            _catalog.Clear();
            foreach (var mission in missions ?? Enumerable.Empty<EspionageMissionDef>())
            {
                if (mission != null && !string.IsNullOrWhiteSpace(mission.Id) && !_catalog.ContainsKey(mission.Id))
                    _catalog[mission.Id] = mission;
            }
        }

        public EspionageMissionPreview PreviewDeployAgent(string agentId, string factionId, string missionId, int day)
        {
            var preview = new EspionageMissionPreview { missionId = missionId ?? string.Empty };
            if (string.IsNullOrWhiteSpace(agentId)) { preview.failureCode = "invalid_agent"; return preview; }
            if (!_isAgentAlive(agentId)) { preview.failureCode = "agent_not_alive"; return preview; }
            if (!_isAgentAvailable(agentId) || IsAgentCommitted(agentId)) { preview.failureCode = "agent_unavailable"; return preview; }
            string canonicalFaction = _canonicalizeFaction(factionId);
            if (string.IsNullOrWhiteSpace(canonicalFaction)) { preview.failureCode = "invalid_faction"; return preview; }
            if (!_catalog.TryGetValue(missionId ?? string.Empty, out var definition)) { preview.failureCode = "unknown_mission"; return preview; }
            if (!string.IsNullOrWhiteSpace(definition.RequiredResearchId) && !_isResearchComplete(definition.RequiredResearchId))
            {
                preview.failureCode = "research_locked";
                return preview;
            }
            if (definition.MissionType == EspionageMissionType.Exfiltration)
            {
                preview.failureCode = "invalid_deploy_mission";
                return preview;
            }

            var security = GetOrCreateSecurity(canonicalFaction, create: false);
            float capability = Math.Clamp(_getAgentCapability(agentId), 0.25f, 2f);
            float counterIntel = security?.counterIntel01 ?? 0f;
            preview.canonicalFactionId = canonicalFaction;
            preview.durationDays = Math.Max(1, definition.BaseDurationDays);
            preview.coverStrength01 = Math.Clamp(0.5f + (capability - 1f) * 0.2f, 0.1f, 0.95f);
            preview.detectionChance = Math.Clamp(
                definition.BaseDetectionChance + counterIntel * 0.25f - preview.coverStrength01 * 0.2f,
                0.01f, 0.95f);
            preview.isAvailable = true;
            return preview;
        }

        public ActionResult DeployAgent(string agentId, string factionId, string missionId, int day)
        {
            var preview = PreviewDeployAgent(agentId, factionId, missionId, day);
            if (!preview.isAvailable)
                return ActionResult.Blocked(preview.failureCode, "espionage.deploy_unavailable");

            string networkId = "spy_network_" + (++_sequence).ToString("D4");
            string missionInstanceId = "spy_mission_" + _sequence.ToString("D4");
            var security = GetOrCreateSecurity(preview.canonicalFactionId, create: true)!;
            var network = new SpyNetworkState
            {
                networkId = networkId,
                targetFactionId = preview.canonicalFactionId,
                infiltratorId = agentId,
                establishedDay = day,
                coverStrength01 = preview.coverStrength01,
                counterIntelPressure01 = security.counterIntel01,
                status = EspionageMissionStatus.Deploying
            };
            _state.networks.Add(network);
            _state.activeMissions.Add(new EspionageMissionState
            {
                missionInstanceId = missionInstanceId,
                missionDefinitionId = missionId,
                networkId = networkId,
                startedDay = day,
                lastTickDay = day,
                status = EspionageMissionStatus.Deploying
            });
            _setAgentAway(agentId, true);
            OnStateChanged?.Invoke();
            return ActionResult.Success("espionage.agent_deployed", new Dictionary<string, double>
            {
                { "duration_days", preview.durationDays },
                { "detection_chance", preview.detectionChance }
            });
        }

        public void Tick(int newDay)
        {
            if (newDay < 0) return;
            bool changed = false;
            foreach (var mission in _state.activeMissions.OrderBy(m => m.missionInstanceId, StringComparer.Ordinal).ToList())
            {
                if (mission == null || mission.status == EspionageMissionStatus.Captured || mission.status == EspionageMissionStatus.Returned) continue;
                if (!_catalog.TryGetValue(mission.missionDefinitionId, out var definition)) continue;
                if (!(_state.networks.FirstOrDefault(n => n.networkId == mission.networkId) is SpyNetworkState network)) continue;
                int fromDay = Math.Max(mission.lastTickDay + 1, mission.startedDay + 1);
                for (int day = fromDay; day <= newDay; day++)
                {
                    if (mission.status == EspionageMissionStatus.Captured || mission.status == EspionageMissionStatus.Failed) break;
                    mission.lastTickDay = day;
                    mission.progressDays++;
                    changed = true;
                    mission.status = EspionageMissionStatus.Operating;
                    network.status = EspionageMissionStatus.Operating;
                    network.networkExposure01 = Math.Clamp(network.networkExposure01 + definition.NetworkExposureGain, 0f, 1f);
                    int gain = (int)Math.Floor(definition.IntelGain * Math.Clamp(_getAgentCapability(network.infiltratorId), 0.25f, 2f)
                        * Math.Clamp(network.coverStrength01, 0.2f, 1f)
                        * (1f - network.counterIntelPressure01 * 0.5f));
                    if (definition.IntelGain > 0) gain = Math.Max(1, gain);
                    if (gain > 0) network.intelLevel = Math.Min(4, network.intelLevel + gain);
                    DiscoverFacts(network, definition, day);

                    float detectionChance = Math.Clamp(
                        definition.BaseDetectionChance + network.counterIntelPressure01 * 0.25f
                        + network.networkExposure01 * 0.2f - network.coverStrength01 * 0.2f,
                        0.01f, 0.95f);
                    if (_rng.NextDouble() < detectionChance)
                    {
                        Capture(network, mission, day);
                        changed = true;
                        break;
                    }
                }

                if (mission.status == EspionageMissionStatus.Operating
                    && mission.progressDays >= Math.Max(1, definition.BaseDurationDays))
                {
                    mission.status = EspionageMissionStatus.Returned;
                    network.status = EspionageMissionStatus.Operating;
                    _state.missionHistory.Add(CloneMission(mission));
                    _state.activeMissions.Remove(mission);
                    _setAgentAway(network.infiltratorId, false);
                    changed = true;
                }
            }
            changed |= ExpireCapturedAgents(newDay);
            if (changed) OnStateChanged?.Invoke();
        }

        public ActionResult Exfiltrate(string networkId, int day)
        {
            var network = FindNetwork(networkId);
            if (network == null) return ActionResult.Blocked("unknown_network", "espionage.unknown_network");
            var mission = _state.activeMissions.FirstOrDefault(m => m.networkId == networkId);
            if (mission == null || mission.status == EspionageMissionStatus.Captured)
                return ActionResult.Blocked("agent_not_operating", "espionage.agent_not_operating");
            mission.status = EspionageMissionStatus.Returned;
            network.status = EspionageMissionStatus.Returned;
            _state.missionHistory.Add(CloneMission(mission));
            _state.activeMissions.Remove(mission);
            _setAgentAway(network.infiltratorId, false);
            OnStateChanged?.Invoke();
            return ActionResult.Success("espionage.agent_returned", new Dictionary<string, double> { { "day", day } });
        }

        public ActionResult ExecuteAbstractSabotage(string networkId, string missionId, int day)
        {
            var network = FindNetwork(networkId);
            if (network == null) return ActionResult.Blocked("unknown_network", "espionage.unknown_network");
            if (!_catalog.TryGetValue(missionId ?? string.Empty, out var definition)
                || definition.MissionType != EspionageMissionType.AbstractSabotage)
                return ActionResult.Blocked("invalid_sabotage", "espionage.invalid_sabotage");
            if (network.intelLevel < Math.Max(0, definition.MinimumIntelLevel))
                return ActionResult.Blocked("insufficient_intel", "espionage.insufficient_intel");

            float chance = Math.Clamp(0.45f + network.intelLevel * 0.08f - network.counterIntelPressure01 * 0.25f, 0.05f, 0.9f);
            if (_rng.NextDouble() >= chance)
            {
                network.networkExposure01 = Math.Clamp(network.networkExposure01 + 0.15f, 0f, 1f);
                OnStateChanged?.Invoke();
                return ActionResult.Failed("sabotage_failed", "espionage.sabotage_failed");
            }

            string consequence = definition.PossibleConsequenceIds.Count > 0
                ? definition.PossibleConsequenceIds[_rng.Next(0, definition.PossibleConsequenceIds.Count)]
                : "supply_disruption";
            var intent = new EspionageConsequenceIntent
            {
                incidentId = "espionage_incident_" + (++_sequence).ToString("D4"),
                targetFactionId = network.targetFactionId,
                consequenceType = consequence,
                magnitude = Math.Clamp(0.1f + network.intelLevel * 0.05f, 0.1f, 0.35f),
                startDay = day,
                expiryDay = day + 3
            };
            OnConsequenceIntent?.Invoke(intent);
            network.networkExposure01 = Math.Clamp(network.networkExposure01 + 0.1f, 0f, 1f);
            OnStateChanged?.Invoke();
            return ActionResult.Success("espionage.sabotage_complete", new Dictionary<string, double>
            {
                { "magnitude", intent.magnitude },
                { "expiry_day", intent.expiryDay }
            });
        }

        public ActionResult PayRansom(string agentId, int day)
        {
            var captured = _state.capturedAgents.FirstOrDefault(c => string.Equals(c.agentId, agentId, StringComparison.Ordinal));
            if (captured == null) return ActionResult.Blocked("agent_not_captured", "espionage.agent_not_captured");
            captured.status = "ransomed";
            var network = FindNetwork(captured.networkId);
            var mission = _state.activeMissions.FirstOrDefault(m => m != null && string.Equals(m.networkId, captured.networkId, StringComparison.Ordinal));
            if (mission != null)
            {
                mission.status = EspionageMissionStatus.Returned;
                _state.missionHistory.Add(CloneMission(mission));
                _state.activeMissions.Remove(mission);
            }
            if (network != null) network.status = EspionageMissionStatus.Returned;
            _state.capturedAgents.Remove(captured);
            _setAgentAway(agentId, false);
            OnStateChanged?.Invoke();
            return ActionResult.Success("espionage.ransom_paid", new Dictionary<string, double> { { "day", day } });
        }

        public bool IsAgentCommitted(string agentId)
        {
            return _state.networks.Any(n => n != null && string.Equals(n.infiltratorId, agentId, StringComparison.Ordinal)
                && n.status != EspionageMissionStatus.Returned && n.status != EspionageMissionStatus.Failed)
                || _state.capturedAgents.Any(c => c != null && string.Equals(c.agentId, agentId, StringComparison.Ordinal));
        }

        private void DiscoverFacts(SpyNetworkState network, EspionageMissionDef definition, int day)
        {
            int threshold = network.intelLevel >= 4 ? (definition.IntelFactIds.Count > 0 ? 1 : int.MaxValue)
                : network.intelLevel >= 2 ? 1 : int.MaxValue;
            if (threshold == int.MaxValue) return;
            for (int i = 0; i < definition.IntelFactIds.Count; i++)
            {
                string factId = definition.IntelFactIds[i] ?? string.Empty;
                if (string.IsNullOrWhiteSpace(factId) || network.knownCapabilityFacts.Contains(factId, StringComparer.Ordinal)) continue;
                network.knownCapabilityFacts.Add(factId);
                var fact = new EspionageIntelFactState
                {
                    factId = factId,
                    factionId = network.targetFactionId,
                    confidence = network.intelLevel >= 4 ? IntelConfidence.Confirmed : IntelConfidence.Estimate,
                    discoveredDay = day,
                    lastConfirmedDay = day
                };
                _state.knownIntel.Add(fact);
                OnIntelDiscovered?.Invoke(fact);
            }
        }

        private void Capture(SpyNetworkState network, EspionageMissionState mission, int day)
        {
            mission.status = EspionageMissionStatus.Captured;
            network.status = EspionageMissionStatus.Captured;
            network.doubleAgentStatus = DoubleAgentStatus.Compromised;
            network.loyaltyRisk01 = Math.Clamp(network.loyaltyRisk01 + 0.25f, 0f, 1f);
            var security = GetOrCreateSecurity(network.targetFactionId, true)!;
            security.paranoia01 = Math.Clamp(security.paranoia01 + 0.1f, 0f, 1f);
            security.recentIntrusions++;
            security.lastIncidentDay = day;
            var captured = new CapturedAgentState
            {
                agentId = network.infiltratorId,
                networkId = network.networkId,
                targetFactionId = network.targetFactionId,
                captureDay = day,
                executionDeadlineDay = day + 3
            };
            _state.capturedAgents.Add(captured);
            OnAgentCaptured?.Invoke(captured);
        }

        private bool ExpireCapturedAgents(int day)
        {
            bool changed = false;
            foreach (var captured in _state.capturedAgents.ToList())
            {
                if (captured == null || day <= captured.executionDeadlineDay) continue;
                captured.status = "executed";
                _state.capturedAgents.Remove(captured);
                var network = FindNetwork(captured.networkId);
                if (network != null) network.status = EspionageMissionStatus.Failed;
                _setAgentAway(captured.agentId, false);
                changed = true;
            }
            return changed;
        }

        private SpyNetworkState? FindNetwork(string networkId)
            => _state.networks.FirstOrDefault(n => n != null && string.Equals(n.networkId, networkId, StringComparison.Ordinal));

        private EspionageFactionSecurityState? GetOrCreateSecurity(string factionId, bool create)
        {
            string canonical = _canonicalizeFaction(factionId);
            var security = _state.factionSecurity.FirstOrDefault(s => s != null && string.Equals(s.factionId, canonical, StringComparison.Ordinal));
            if (security == null && create)
            {
                security = new EspionageFactionSecurityState { factionId = canonical };
                _state.factionSecurity.Add(security);
            }
            return security;
        }

        private static EspionageMissionState CloneMission(EspionageMissionState source)
            => new EspionageMissionState
            {
                missionInstanceId = source.missionInstanceId,
                missionDefinitionId = source.missionDefinitionId,
                networkId = source.networkId,
                startedDay = source.startedDay,
                lastTickDay = source.lastTickDay,
                progressDays = source.progressDays,
                status = source.status
            };

        public EspionageState CaptureState()
        {
            return new EspionageState
            {
                systemId = _state.systemId,
                networks = _state.networks.Where(n => n != null).Select(CloneNetwork).ToList(),
                activeMissions = _state.activeMissions.Where(m => m != null).Select(CloneMission).ToList(),
                capturedAgents = _state.capturedAgents.Where(c => c != null).Select(CloneCaptured).ToList(),
                knownIntel = _state.knownIntel.Where(i => i != null).Select(CloneIntel).ToList(),
                missionHistory = _state.missionHistory.Where(m => m != null).Select(CloneMission).ToList(),
                factionSecurity = _state.factionSecurity.Where(s => s != null).Select(CloneSecurity).ToList()
            };
        }

        public void RestoreState(EspionageState saved)
        {
            if (saved == null) return;
            _state = new EspionageState
            {
                systemId = string.IsNullOrWhiteSpace(saved.systemId) ? SystemId : saved.systemId,
                networks = saved.networks?.Where(n => n != null).Select(CloneNetwork).ToList() ?? new List<SpyNetworkState>(),
                activeMissions = saved.activeMissions?.Where(m => m != null).Select(CloneMission).ToList() ?? new List<EspionageMissionState>(),
                capturedAgents = saved.capturedAgents?.Where(c => c != null).Select(CloneCaptured).ToList() ?? new List<CapturedAgentState>(),
                knownIntel = saved.knownIntel?.Where(i => i != null).Select(CloneIntel).ToList() ?? new List<EspionageIntelFactState>(),
                missionHistory = saved.missionHistory?.Where(m => m != null).Select(CloneMission).ToList() ?? new List<EspionageMissionState>(),
                factionSecurity = saved.factionSecurity?.Where(s => s != null).Select(CloneSecurity).ToList() ?? new List<EspionageFactionSecurityState>()
            };
            _sequence = Math.Max(_sequence, InferSequence());
            OnStateChanged?.Invoke();
        }

        private int InferSequence()
        {
            int max = 0;
            foreach (var network in _state.networks)
                if (TryReadSequence(network.networkId, out int value)) max = Math.Max(max, value);
            foreach (var mission in _state.activeMissions)
                if (TryReadSequence(mission.missionInstanceId, out int value)) max = Math.Max(max, value);
            return max;
        }

        private static bool TryReadSequence(string id, out int value)
        {
            value = 0;
            if (string.IsNullOrWhiteSpace(id)) return false;
            int underscore = id.LastIndexOf('_');
            return underscore >= 0 && int.TryParse(id.Substring(underscore + 1), out value);
        }

        private static SpyNetworkState CloneNetwork(SpyNetworkState source)
            => new SpyNetworkState
            {
                networkId = source.networkId,
                targetFactionId = source.targetFactionId,
                infiltratorId = source.infiltratorId,
                establishedDay = source.establishedDay,
                coverStrength01 = Math.Clamp(source.coverStrength01, 0f, 1f),
                intelLevel = Math.Clamp(source.intelLevel, 0, 4),
                networkExposure01 = Math.Clamp(source.networkExposure01, 0f, 1f),
                counterIntelPressure01 = Math.Clamp(source.counterIntelPressure01, 0f, 1f),
                loyaltyRisk01 = Math.Clamp(source.loyaltyRisk01, 0f, 1f),
                status = source.status,
                knownStrongholdIds = source.knownStrongholdIds != null ? new List<string>(source.knownStrongholdIds) : new List<string>(),
                knownCapabilityFacts = source.knownCapabilityFacts != null ? new List<string>(source.knownCapabilityFacts) : new List<string>(),
                doubleAgentStatus = source.doubleAgentStatus
            };

        private static CapturedAgentState CloneCaptured(CapturedAgentState source)
            => new CapturedAgentState
            {
                agentId = source.agentId,
                networkId = source.networkId,
                targetFactionId = source.targetFactionId,
                captureDay = source.captureDay,
                executionDeadlineDay = source.executionDeadlineDay,
                status = source.status
            };

        private static EspionageIntelFactState CloneIntel(EspionageIntelFactState source)
            => new EspionageIntelFactState
            {
                factId = source.factId,
                factionId = source.factionId,
                confidence = source.confidence,
                discoveredDay = source.discoveredDay,
                lastConfirmedDay = source.lastConfirmedDay
            };

        private static EspionageFactionSecurityState CloneSecurity(EspionageFactionSecurityState source)
            => new EspionageFactionSecurityState
            {
                factionId = source.factionId,
                paranoia01 = Math.Clamp(source.paranoia01, 0f, 1f),
                counterIntel01 = Math.Clamp(source.counterIntel01, 0f, 1f),
                recentIntrusions = Math.Max(0, source.recentIntrusions),
                lastIncidentDay = source.lastIncidentDay
            };
    }
}
