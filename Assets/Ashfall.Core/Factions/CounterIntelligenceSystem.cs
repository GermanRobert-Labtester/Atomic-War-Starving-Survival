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
            _state = state ?? new CounterIntelligenceState();
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
            float suspicion = candidate.scrutinyRating;
            foreach (var flag in candidate.suspicionFlags)
                suspicion += 10f;
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
            var detainee = _state.detainees.Find(d => d.suspectId == suspectId);
            if (detainee == null)
                return CiInterrogationResult.Failed("not_detained", "counterintel.not_detained");

            if (detainee.interrogationCount >= 3)
                return CiInterrogationResult.Failed("max_interrogations", "counterintel.max_interrogations");

            detainee.interrogationCount++;
            detainee.interrogationStatus = "completed";

            // Deterministic confession check
            var agent = GetAgent(suspectId);
            if (agent != null && _profiles.TryGetValue(agent.profileId, out var profile))
            {
                float roll = (float)_rng.NextDouble();
                if (roll < profile.ConfessionThreshold * pressureLevel)
                {
                    detainee.confessionOutcome = "full";
                    agent.isExposed = true;
                    agent.exposureDay = _currentDay;
                    OnAgentExposed?.Invoke(suspectId);
                    OnInterrogationCompleted?.Invoke(suspectId, "full_confession");
                    return CiInterrogationResult.Success("full_confession", profile.SabotageTargets);
                }
                else if (roll < profile.ConfessionThreshold * 0.6f)
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
            var agent = GetAgent(agentId);
            if (agent == null)
                return SabotageResult.Failed("unknown_agent", "counterintel.unknown_agent");

            if (agent.isExposed || agent.isInactive)
                return SabotageResult.Failed("agent_inactive", "counterintel.agent_inactive");

            if (!_profiles.TryGetValue(agent.profileId, out var profile))
                return SabotageResult.Failed("unknown_profile", "counterintel.unknown_profile");

            // Deterministic sabotage target selection
            int idx = (int)(_rng.NextDouble() * profile.SabotageTargets.Count);
            string target = profile.SabotageTargets[idx];

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
        }

        private static CounterIntelligenceState CloneState(CounterIntelligenceState src)
        {
            if (src == null) return new CounterIntelligenceState();
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(src);
            return s.Deserialize<CounterIntelligenceState>(json) ?? new CounterIntelligenceState();
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
