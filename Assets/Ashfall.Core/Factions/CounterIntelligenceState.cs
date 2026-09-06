using System;
using System.Collections.Generic;

namespace Ashfall.Core.Factions
{
    [Serializable]
    public sealed class CounterIntelligenceState
    {
        public string systemId = CounterIntelligenceSystem.SystemId;
        public List<VettingCandidateState> candidates = new List<VettingCandidateState>();
        public List<UndercoverAgentState> undercoverAgents = new List<UndercoverAgentState>();
        public List<SurveillanceLogEntry> surveillanceLog = new List<SurveillanceLogEntry>();
        public List<string> knownEvidenceIds = new List<string>();
        public List<DetaineeState> detainees = new List<DetaineeState>();
        public List<DefectorAsylumState> defectorAsylum = new List<DefectorAsylumState>();
        public int lastProcessedDay = -1;
    }

    [Serializable]
    public sealed class VettingCandidateState
    {
        public string candidateId = string.Empty;
        public string claimedBackground = string.Empty;
        public float scrutinyRating = 0f;
        public List<string> suspicionFlags = new List<string>();
        public string quarantineClearance = "none"; // none, restricted, full
        public List<string> evidenceIds = new List<string>();
        public int interviewCount = 0;
        public string assignedVetterId = string.Empty;
        public string status = "awaiting_vetting"; // awaiting_vetting, under_review, quarantined, cleared, rejected, detained, exposed, defector_accepted
        public int lastUpdatedDay = 0;
    }

    [Serializable]
    public sealed class UndercoverAgentState
    {
        public string survivorId = string.Empty;
        public string profileId = string.Empty;
        public string sourceFactionId = string.Empty;
        public bool isExposed = false;
        public int exposureDay = -1;
        public bool isInactive = false;
        public int inactivationDay = -1;
    }

    [Serializable]
    public sealed class SurveillanceLogEntry
    {
        public int day = 0;
        public string subjectId = string.Empty;
        public string observerId = string.Empty;
        public string observation = string.Empty;
        public float suspicionDelta = 0f;
    }

    [Serializable]
    public sealed class DetaineeState
    {
        public string suspectId = string.Empty;
        public int detentionDay = 0;
        public string interrogationStatus = "pending"; // pending, completed, refused
        public string confessionOutcome = "none"; // none, full, partial, false
        public int interrogationCount = 0;
    }

    [Serializable]
    public sealed class DefectorAsylumState
    {
        public string candidateId = string.Empty;
        public string claimedFactionId = string.Empty;
        public string status = "pending"; // pending, accepted, rejected, probation
        public int decisionDay = 0;
        public List<string> grantedIntelIds = new List<string>();
    }
}
