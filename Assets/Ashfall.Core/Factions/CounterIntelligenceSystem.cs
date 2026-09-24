// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Factions
{
    /// <summary>
    /// Faction infiltration, counter-intelligence, and defector vetting.
    /// Owns vetting state, undercover-agent exposure, suspicion evidence,
    /// detention/interrogation state, and defector-clearing state.
    /// Consumes existing domain authorities; never duplicates them.
    /// </summary>
    public sealed class CounterIntelligenceSystem
    {
        public const string SystemId = "counter_intelligence";
        public const float SuspicionMin = 0f;
        public const float SuspicionMax = 100f;

        private CounterIntelligenceState _state;
        private readonly ISeededRng _rng;
        private readonly ILog _log;
        private readonly Dictionary<string, InfiltratorProfileDef> _profiles = new(StringComparer.Ordinal);
        private int _currentDay;

        public CounterIntelligenceState State => _state;
        public IReadOnlyDictionary<string, InfiltratorProfileDef> Profiles => _profiles;

        public event Action<string, string>? OnCandidateFlagged;     // candidateId, flag
        public event Action<string>? OnAgentExposed;                 // survivorId
        public event Action<string, string, string>? OnSabotageDiscovered; // target, agentId, day
        public event Action<string>? OnDefectorAccepted;            // candidateId
        public event Action<string, string>? OnInterrogationCompleted; // suspectId, outcome

        public CounterIntelligenceSystem(
            CounterIntelligenceState? state,
            ISeededRng rng,
            ILog? log = null)
        {
            _state = CloneState(state);
            _currentDay = _state.lastProcessedDay;
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _log = log ?? NullLog.Instance;
        }

        // ── Catalog ──────────────────────────────────────────────────

        public void RegisterProfile(InfiltratorProfileDef def)
        {
            if (def != null && !string.IsNullOrEmpty(def.ProfileId))
                _profiles[def.ProfileId] = def;
        }

        public void LoadCatalog(InfiltratorCatalog? catalog)
        {
            if (catalog?.Profiles == null) return;
            foreach (var def in catalog.Profiles)
                RegisterProfile(def);
        }

        // ── Queries ──────────────────────────────────────────────────

        public VettingCandidateState? GetCandidate(string candidateId)
        {
            return _state.candidates.Find(c => c.candidateId == candidateId);
        }

        public UndercoverAgentState? GetAgent(string survivorId)
        {
            return _state.undercoverAgents.Find(a => a.survivorId == survivorId);
        }

        public bool IsDetained(string survivorId)
        {
            return _state.detainees.Exists(d => d.suspectId == survivorId);
        }

        // ── Actions ──────────────────────────────────────────────────

        public VettingResult VetCandidate(string candidateId, string officerId)
        {
            if (string.IsNullOrWhiteSpace(candidateId))
                return VettingResult.Failed("unknown_candidate", "counterintel.unknown_candidate");

            var candidate = GetCandidate(candidateId);
            if (candidate == null)
                return VettingResult.Failed("unknown_candidate", "counterintel.unknown_candidate");

            if (candidate.status != "awaiting_vetting" && candidate.status != "under_review")
                return VettingResult.Failed("already_vetted", "counterintel.already_vetted");

            candidate.status = "under_review";
            candidate.assignedVetterId = officerId;
            candidate.interviewCount++;
            candidate.lastUpdatedDay = _currentDay;

            // Deterministic suspicion computation (no hidden truth exposed)
            float suspicion = SanitizeRating(candidate.scrutinyRating);
            if (candidate.suspicionFlags != null)
            {
                foreach (var flag in candidate.suspicionFlags)
                    suspicion = Math.Min(SuspicionMax, suspicion + 10f);
            }
            suspicion = Math.Clamp(suspicion, SuspicionMin, SuspicionMax);

            if (suspicion >= 70f)
            {
                candidate.quarantineClearance = "restricted";
                candidate.status = "quarantined";
                OnCandidateFlagged?.Invoke(candidateId, "high_suspicion");
            }
            else if (suspicion >= 40f)
            {
                candidate.quarantineClearance = "restricted";
            }
            else
            {
                candidate.quarantineClearance = "full";
                candidate.status = "cleared";
            }

            _log.Info($"[CounterIntel] Vetted {candidateId}: suspicion={suspicion:F1}, status={candidate.status}");
            return VettingResult.Success(suspicion, candidate.status);
        }

        public CiInterrogationResult Interrogate(string suspectId, string officerId, float pressureLevel)
        {
            if (string.IsNullOrWhiteSpace(suspectId))
                return CiInterrogationResult.Failed("not_detained", "counterintel.not_detained");

            var detainee = _state.detainees.Find(d => d != null && d.suspectId == suspectId);
            if (detainee == null)
                return CiInterrogationResult.Failed("not_detained", "counterintel.not_detained");

            if (detainee.interrogationCount >= 3)
                return CiInterrogationResult.Failed("max_interrogations", "counterintel.max_interrogations");

            detainee.interrogationCount++;
            detainee.interrogationStatus = "completed";

            // Deterministic confession check. Invalid pressure and thresholds
            // fail closed instead of turning a malformed call into a guaranteed
            // confession or a non-finite state write.
            float boundedPressure = SanitizeUnit(pressureLevel);
            var agent = GetAgent(suspectId);
            if (agent != null && _profiles.TryGetValue(agent.profileId, out var profile))
            {
                double sample = _rng.NextDouble();
                float roll = SanitizeUnit((float)sample);
                float threshold = SanitizeUnit(profile.ConfessionThreshold);
                if (roll < threshold * boundedPressure)
                {
                    detainee.confessionOutcome = "full";
                    agent.isExposed = true;
                    agent.exposureDay = _currentDay;
                    OnAgentExposed?.Invoke(suspectId);
                    OnInterrogationCompleted?.Invoke(suspectId, "full_confession");
                    return CiInterrogationResult.Success("full_confession", profile.SabotageTargets);
                }
                else if (roll < threshold * 0.6f)
                {
                    detainee.confessionOutcome = "partial";
                    OnInterrogationCompleted?.Invoke(suspectId, "partial_intel");
                    return CiInterrogationResult.Success("partial_intel", new List<string>());
                }
            }

            detainee.confessionOutcome = "none";
            OnInterrogationCompleted?.Invoke(suspectId, "refused");
            return CiInterrogationResult.Success("refused", new List<string>());
        }

        public ActionResult DetainSuspect(string suspectId)
        {
            if (string.IsNullOrWhiteSpace(suspectId))
                return ActionResult.Failed("invalid_suspect", "counterintel.invalid_suspect");
            if (IsDetained(suspectId))
                return ActionResult.Blocked("already_detained", "counterintel.already_detained");

            _state.detainees.Add(new DetaineeState
            {
                suspectId = suspectId,
                detentionDay = _currentDay,
                interrogationStatus = "pending"
            });

            var candidate = GetCandidate(suspectId);
            if (candidate != null)
                candidate.status = "detained";

            _log.Info($"[CounterIntel] Detained {suspectId}");
            return ActionResult.Success("counterintel.detained");
        }

        public ActionResult AcceptDefector(string candidateId)
        {
            var candidate = GetCandidate(candidateId);
            if (candidate == null)
                return ActionResult.Failed("unknown_candidate", "counterintel.unknown_candidate");
            if (candidate.status == "defector_accepted"
                || _state.defectorAsylum.Exists(entry => entry != null
                    && string.Equals(entry.candidateId, candidateId, StringComparison.OrdinalIgnoreCase)))
                return ActionResult.Blocked("already_accepted", "counterintel.already_accepted");

            candidate.status = "defector_accepted";
            _state.defectorAsylum.Add(new DefectorAsylumState
            {
                candidateId = candidateId,
                claimedFactionId = candidate.claimedBackground,
                status = "accepted",
                decisionDay = _currentDay
            });

            OnDefectorAccepted?.Invoke(candidateId);
            _log.Info($"[CounterIntel] Accepted defector {candidateId}");
            return ActionResult.Success("counterintel.defector_accepted");
        }

        // ── Sabotage (consumed by other systems via explicit adapters) ──

        public SabotageResult ResolveSabotage(string agentId)
        {
            if (string.IsNullOrWhiteSpace(agentId))
                return SabotageResult.Failed("unknown_agent", "counterintel.unknown_agent");

            var agent = GetAgent(agentId);
            if (agent == null)
                return SabotageResult.Failed("unknown_agent", "counterintel.unknown_agent");

            if (agent.isExposed || agent.isInactive)
                return SabotageResult.Failed("agent_inactive", "counterintel.agent_inactive");

            if (!_profiles.TryGetValue(agent.profileId, out var profile))
                return SabotageResult.Failed("unknown_profile", "counterintel.unknown_profile");

            var targets = (profile.SabotageTargets ?? new List<string>())
                .Where(target => !string.IsNullOrWhiteSpace(target))
                .ToList();
            if (targets.Count == 0)
                return SabotageResult.Failed("no_sabotage_targets", "counterintel.no_sabotage_targets");

            // Deterministic sabotage target selection with a total index even
            // for a custom/test RNG returning 1.0 or a non-finite sample.
            double sample = _rng.NextDouble();
            int idx = double.IsNaN(sample) || sample <= 0d
                ? 0
                : sample >= 1d
                    ? targets.Count - 1
                    : (int)(sample * targets.Count);
            string target = targets[idx];

            _log.Warn($"[CounterIntel] Sabotage detected: agent={agentId}, target={target}");
            OnSabotageDiscovered?.Invoke(target, agentId, _currentDay.ToString());
            return SabotageResult.Success(target);
        }

        // ── Daily Tick ───────────────────────────────────────────────

        public void TickDay(int day)
        {
            _currentDay = day;
            if (day <= _state.lastProcessedDay) return;
            _state.lastProcessedDay = day;

            // Inactive agents consume no RNG (verified by design: we only resolve
            // sabotage for active, non-exposed agents).
        }

        // ── Persistence ──────────────────────────────────────────────

        public CounterIntelligenceState CaptureState() => CloneState(_state);

        public void RestoreState(CounterIntelligenceState saved)
        {
            if (saved == null) return;
            _state = CloneState(saved);
            _currentDay = _state.lastProcessedDay;
        }

        private static CounterIntelligenceState CloneState(CounterIntelligenceState? src)
        {
            var copy = new CounterIntelligenceState
            {
                systemId = string.IsNullOrWhiteSpace(src?.systemId)
                    ? CounterIntelligenceSystem.SystemId
                    : src!.systemId,
                lastProcessedDay = src?.lastProcessedDay ?? -1,
                candidates = new List<VettingCandidateState>(),
                undercoverAgents = new List<UndercoverAgentState>(),
                surveillanceLog = new List<SurveillanceLogEntry>(),
                knownEvidenceIds = new List<string>(),
                detainees = new List<DetaineeState>(),
                defectorAsylum = new List<DefectorAsylumState>()
            };
            if (src == null) return copy;

            var candidateIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (src.candidates != null)
            {
                foreach (var candidate in src.candidates)
                {
                    if (candidate == null || string.IsNullOrWhiteSpace(candidate.candidateId)
                        || !candidateIds.Add(candidate.candidateId)) continue;
                    copy.candidates.Add(new VettingCandidateState
                    {
                        candidateId = candidate.candidateId,
                        claimedBackground = candidate.claimedBackground ?? string.Empty,
                        scrutinyRating = SanitizeRating(candidate.scrutinyRating),
                        suspicionFlags = CloneDistinct(candidate.suspicionFlags),
                        quarantineClearance = candidate.quarantineClearance ?? "none",
                        evidenceIds = CloneDistinct(candidate.evidenceIds),
                        interviewCount = Math.Max(0, candidate.interviewCount),
                        assignedVetterId = candidate.assignedVetterId ?? string.Empty,
                        status = candidate.status ?? "awaiting_vetting",
                        lastUpdatedDay = candidate.lastUpdatedDay
                    });
                }
            }

            var agentIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (src.undercoverAgents != null)
            {
                foreach (var agent in src.undercoverAgents)
                {
                    if (agent == null || string.IsNullOrWhiteSpace(agent.survivorId)
                        || !agentIds.Add(agent.survivorId)) continue;
                    copy.undercoverAgents.Add(new UndercoverAgentState
                    {
                        survivorId = agent.survivorId,
                        profileId = agent.profileId ?? string.Empty,
                        sourceFactionId = agent.sourceFactionId ?? string.Empty,
                        isExposed = agent.isExposed,
                        exposureDay = agent.exposureDay,
                        isInactive = agent.isInactive,
                        inactivationDay = agent.inactivationDay
                    });
                }
            }

            if (src.surveillanceLog != null)
            {
                foreach (var entry in src.surveillanceLog)
                {
                    if (entry == null || string.IsNullOrWhiteSpace(entry.subjectId)) continue;
                    copy.surveillanceLog.Add(new SurveillanceLogEntry
                    {
                        day = entry.day,
                        subjectId = entry.subjectId,
                        observerId = entry.observerId ?? string.Empty,
                        observation = entry.observation ?? string.Empty,
                        suspicionDelta = SanitizeFinite(entry.suspicionDelta)
                    });
                }
            }

            copy.knownEvidenceIds.AddRange(CloneDistinct(src.knownEvidenceIds));
            var detaineeIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (src.detainees != null)
            {
                foreach (var detainee in src.detainees)
                {
                    if (detainee == null || string.IsNullOrWhiteSpace(detainee.suspectId)
                        || !detaineeIds.Add(detainee.suspectId)) continue;
                    copy.detainees.Add(new DetaineeState
                    {
                        suspectId = detainee.suspectId,
                        detentionDay = detainee.detentionDay,
                        interrogationStatus = detainee.interrogationStatus ?? "pending",
                        confessionOutcome = detainee.confessionOutcome ?? "none",
                        interrogationCount = Math.Max(0, detainee.interrogationCount)
                    });
                }
            }

            var asylumIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (src.defectorAsylum != null)
            {
                foreach (var asylum in src.defectorAsylum)
                {
                    if (asylum == null || string.IsNullOrWhiteSpace(asylum.candidateId)
                        || !asylumIds.Add(asylum.candidateId)) continue;
                    copy.defectorAsylum.Add(new DefectorAsylumState
                    {
                        candidateId = asylum.candidateId,
                        claimedFactionId = asylum.claimedFactionId ?? string.Empty,
                        status = asylum.status ?? "pending",
                        decisionDay = asylum.decisionDay,
                        grantedIntelIds = CloneDistinct(asylum.grantedIntelIds)
                    });
                }
            }
            return copy;
        }

        private static List<string> CloneDistinct(IEnumerable<string>? values)
        {
            var result = new List<string>();
            var seen = new HashSet<string>(StringComparer.Ordinal);
            if (values == null) return result;
            foreach (var value in values)
            {
                if (!string.IsNullOrWhiteSpace(value) && seen.Add(value))
                    result.Add(value);
            }
            return result;
        }

        private static float SanitizeFinite(float value) =>
            float.IsNaN(value) || float.IsInfinity(value) ? 0f : value;

        private static float SanitizeRating(float value) =>
            Math.Clamp(SanitizeFinite(value), SuspicionMin, SuspicionMax);

        private static float SanitizeUnit(float value)
        {
            if (float.IsNaN(value) || float.IsInfinity(value)) return 0f;
            return Math.Clamp(value, 0f, 1f);
        }
    }

    // ── Result DTOs ────────────────────────────────────────────────

    public sealed class VettingResult
    {
        public bool IsSuccess { get; }
        public string FailureCode { get; }
        public string MessageKey { get; }
        public float Suspicion { get; }
        public string Status { get; }

        private VettingResult(bool success, string failureCode, string messageKey, float suspicion, string status)
        {
            IsSuccess = success;
            FailureCode = failureCode;
            MessageKey = messageKey;
            Suspicion = suspicion;
            Status = status;
        }

        public static VettingResult Failed(string code, string key) => new VettingResult(false, code, key, 0f, string.Empty);
        public static VettingResult Success(float suspicion, string status) => new VettingResult(true, string.Empty, string.Empty, suspicion, status);
    }

    public sealed class CiInterrogationResult
    {
        public bool IsSuccess { get; }
        public string FailureCode { get; }
        public string MessageKey { get; }
        public string Outcome { get; }
        public List<string> Intel { get; }

        private CiInterrogationResult(bool success, string failureCode, string messageKey, string outcome, List<string> intel)
        {
            IsSuccess = success;
            FailureCode = failureCode;
            MessageKey = messageKey;
            Outcome = outcome;
            Intel = intel ?? new List<string>();
        }

        public static CiInterrogationResult Failed(string code, string key) => new CiInterrogationResult(false, code, key, string.Empty, new List<string>());
        public static CiInterrogationResult Success(string outcome, List<string> intel) => new CiInterrogationResult(true, string.Empty, string.Empty, outcome, intel);
    }

    public sealed class SabotageResult
    {
        public bool IsSuccess { get; }
        public string FailureCode { get; }
        public string MessageKey { get; }
        public string Target { get; }

        private SabotageResult(bool success, string failureCode, string messageKey, string target)
        {
            IsSuccess = success;
            FailureCode = failureCode;
            MessageKey = messageKey;
            Target = target;
        }

        public static SabotageResult Failed(string code, string key) => new SabotageResult(false, code, key, string.Empty);
        public static SabotageResult Success(string target) => new SabotageResult(true, string.Empty, string.Empty, target);
    }
}
