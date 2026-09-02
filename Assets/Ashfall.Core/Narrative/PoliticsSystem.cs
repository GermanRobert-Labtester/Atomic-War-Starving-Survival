using System;
using System.Collections.Generic;
using Ashfall.Core.Save;
#pragma warning disable CS8618

namespace Ashfall.Core.Narrative
{
    [Serializable]
    public class PolicyDefinition
    {
        public string policy_id = string.Empty;
        public string name = string.Empty;
        public string description = string.Empty;
        public string category = "Civil";
        public bool is_emergency = false;
        public int enactment_cost_influence = 20;
        public float legitimacy_impact = 0f;
        public int repeal_cooldown_days = 14;
        public List<string> supporter_tags = new List<string>();
        public List<string> opponent_tags = new List<string>();
        public List<string> tags = new List<string>();
    }

    [Serializable]
    public class PoliticalPoliciesCatalog
    {
        public int schema_version = 1;
        public List<PolicyDefinition> policies = new List<PolicyDefinition>();
    }

    [Serializable]
    public class ElectionResult
    {
        public int day = 0;
        public string electedLeaderId = string.Empty;
        public Dictionary<string, int> votesPerCandidate = new Dictionary<string, int>(StringComparer.Ordinal);
        public int totalTurnout = 0;
        public bool wasContested = false;
    }

    [Serializable]
    public class ApprovalFactorBreakdown
    {
        public float nutritionScore; // -20..+20
        public float securityScore;  // -20..+20
        public float policyModifier; // -20..+20
        public float crueltyPenalty; // -30..0
        public float totalApproval;  // 0..100
    }

    [Serializable]
    public class PoliticsState
    {
        public string systemId = "politics_system";
        public string currentLeaderId = string.Empty;
        public string governanceMode = "Democratic"; // Appointed, Democratic, MartialLaw
        public float approvalRating = 60f; // 0..100
        public float legitimacy = 75f; // 0..100
        public List<string> activePolicies = new List<string>();
        public int daysUntilElection = 30;
        public bool isMartialLaw = false;
        public bool isElectionDisputed = false;
        public float coupRisk = 0f; // 0..1
        public int totalElectionsHeld = 0;
        public int totalCoupsAttempted = 0;
        public List<ElectionResult> electionHistory = new List<ElectionResult>();
    }

    public class PoliticsSystem
    {
        public const string SystemId = "politics_system";

        private readonly Dictionary<string, PolicyDefinition> _policies = new Dictionary<string, PolicyDefinition>(StringComparer.Ordinal);
        private readonly HashSet<string> _activePolicies = new HashSet<string>(StringComparer.Ordinal);
        private readonly List<ElectionResult> _history = new List<ElectionResult>();

        private string _currentLeaderId = string.Empty;
        private string _governanceMode = "Democratic";
        private float _approvalRating = 60f;
        private float _legitimacy = 75f;
        private int _daysUntilElection = 30;
        private bool _isMartialLaw = false;
        private bool _isElectionDisputed = false;
        private float _coupRisk = 0f;
        private int _totalElections = 0;
        private int _totalCoups = 0;

        public event Action<string>? OnPolicyEnacted;
        public event Action<string>? OnPolicyRepealed;
        public event Action<ElectionResult>? OnElectionHeld;
        public event Action<string>? OnMartialLawDeclared;
        public event Action? OnMartialLawLifted;
        public event Action<string>? OnCoupTriggered;
        public event Action<bool, string>? OnCoupResolved;
        public event Action<string>? OnLeaderDesignated;

        public IReadOnlyCollection<string> ActivePolicies => _activePolicies;
        public string CurrentLeaderId => _currentLeaderId;
        public string GovernanceMode => _governanceMode;
        public float ApprovalRating => _approvalRating;
        public float Legitimacy => _legitimacy;
        public int DaysUntilElection => _daysUntilElection;
        public bool IsMartialLaw => _isMartialLaw;
        public bool IsElectionDisputed => _isElectionDisputed;
        public float CoupRisk => _coupRisk;
        public int TotalElections => _totalElections;
        public int TotalCoups => _totalCoups;
        public IReadOnlyList<ElectionResult> History => _history;

        public void LoadCatalog(string jsonText, IJsonSerializer serializer)
        {
            if (string.IsNullOrWhiteSpace(jsonText) || serializer == null) return;
            try
            {
                var catalog = serializer.Deserialize<PoliticalPoliciesCatalog>(jsonText);
                if (catalog?.policies != null)
                {
                    _policies.Clear();
                    foreach (var p in catalog.policies)
                    {
                        if (!string.IsNullOrEmpty(p.policy_id))
                            _policies[p.policy_id] = p;
                    }
                }
            }
            catch
            {
                // Graceful fallback
            }
        }

        public PolicyDefinition? GetPolicy(string policyId)
        {
            return _policies.TryGetValue(policyId, out var p) ? p : null;
        }

        public void SetInitialLeader(string leaderId)
        {
            _currentLeaderId = leaderId;
        }

        public bool EnactPolicy(string policyId, out string failureReason)
        {
            if (!_policies.TryGetValue(policyId, out var def))
            {
                failureReason = "Policy not found in codex";
                return false;
            }

            if (_activePolicies.Contains(policyId))
            {
                failureReason = "Policy is already active";
                return false;
            }

            if (def.is_emergency && !_isMartialLaw)
            {
                failureReason = "Emergency policies require an active state of martial law";
                return false;
            }

            _activePolicies.Add(policyId);
            _legitimacy = Math.Clamp(_legitimacy + def.legitimacy_impact, 0f, 100f);
            failureReason = string.Empty;
            OnPolicyEnacted?.Invoke(policyId);
            return true;
        }

        public bool RepealPolicy(string policyId)
        {
            if (_activePolicies.Remove(policyId))
            {
                OnPolicyRepealed?.Invoke(policyId);
                return true;
            }
            return false;
        }

        public float CalculateVoterScore(string voterId, string candidateId, List<string> voterTraits, List<string> candidateTraits, float foodSat, float secSat)
        {
            float score = 50f; // Baseline neutrality

            // Environmental satisfaction impacts incumbent preference
            if (candidateId == _currentLeaderId)
            {
                score += (foodSat - 0.5f) * 30f;
                score += (secSat - 0.5f) * 30f;
                score += (_approvalRating - 50f) * 0.4f;
            }

            // Trait synergy
            if (voterTraits != null && candidateTraits != null)
            {
                foreach (var vt in voterTraits)
                {
                    if (candidateTraits.Contains(vt)) score += 15f;
                }
            }

            return Math.Clamp(score, 0f, 100f);
        }

        public ElectionResult HoldElection(
            int currentDay,
            List<string> candidateIds,
            List<string> eligibleVoters,
            Func<string, List<string>> getSurvivorTraits,
            float foodSat,
            float secSat,
            ISeededRng rng)
        {
            if (candidateIds == null || candidateIds.Count == 0)
                throw new InvalidOperationException("Elections require at least one candidate");

            var result = new ElectionResult
            {
                day = currentDay,
                wasContested = candidateIds.Count > 1
            };

            foreach (var cid in candidateIds)
            {
                result.votesPerCandidate[cid] = 0;
            }

            if (eligibleVoters != null)
            {
                foreach (var voterId in eligibleVoters)
                {
                    var vTraits = getSurvivorTraits(voterId);
                    string bestCandidate = candidateIds[0];
                    float highestScore = -1f;

                    foreach (var cid in candidateIds)
                    {
                        var cTraits = getSurvivorTraits(cid);
                        float score = CalculateVoterScore(voterId, cid, vTraits, cTraits, foodSat, secSat);

                        // Small deterministic variance
                        score += (float)(rng.NextDouble() * 5.0 - 2.5);

                        if (score > highestScore)
                        {
                            highestScore = score;
                            bestCandidate = cid;
                        }
                    }

                    result.votesPerCandidate[bestCandidate]++;
                    result.totalTurnout++;
                }
            }

            // Determine winner
            string winner = candidateIds[0];
            int maxVotes = -1;
            foreach (var pair in result.votesPerCandidate)
            {
                if (pair.Value > maxVotes)
                {
                    maxVotes = pair.Value;
                    winner = pair.Key;
                }
            }

            result.electedLeaderId = winner;
            _currentLeaderId = winner;
            _daysUntilElection = 30; // Reset election cycle
            _isElectionDisputed = false;
            _legitimacy = Math.Min(100f, _legitimacy + 15f);
            _totalElections++;

            _history.Add(result);
            OnElectionHeld?.Invoke(result);
            OnLeaderDesignated?.Invoke(winner);
            return result;
        }

        public void DeclareMartialLaw()
        {
            _isMartialLaw = true;
            _governanceMode = "MartialLaw";
            _legitimacy = Math.Max(10f, _legitimacy - 25f);
            OnMartialLawDeclared?.Invoke("Settlement Council suspended; Commander declares Martial Law.");
        }

        public void LiftMartialLaw()
        {
            _isMartialLaw = false;
            _governanceMode = "Democratic";
            _daysUntilElection = 14; // Immediate election scheduled
            OnMartialLawLifted?.Invoke();
        }

        public ApprovalFactorBreakdown CalculateApprovalBreakdown(float foodSat, float secSat, float crueltyIndex)
        {
            var b = new ApprovalFactorBreakdown();
            b.nutritionScore = (foodSat - 0.5f) * 40f;   // -20..+20
            b.securityScore = (secSat - 0.5f) * 40f;     // -20..+20
            b.crueltyPenalty = -(crueltyIndex * 0.3f);   // -30..0

            float policyMod = 0f;
            foreach (var pId in _activePolicies)
            {
                if (_policies.TryGetValue(pId, out var p))
                    policyMod += p.legitimacy_impact * 0.5f;
            }
            b.policyModifier = Math.Clamp(policyMod, -20f, 20f);

            b.totalApproval = Math.Clamp(50f + b.nutritionScore + b.securityScore + b.policyModifier + b.crueltyPenalty, 5f, 95f);
            return b;
        }

        public float CalculateCoupRisk(float foodSat, float crueltyIndex, int guardDeficiency)
        {
            if (_isMartialLaw)
            {
                // Under martial law, legitimacy decay fuels coup plotting
                float baseCoup = (100f - _legitimacy) * 0.007f;
                if (foodSat < 0.3f) baseCoup += 0.25f;
                return Math.Clamp(baseCoup, 0.05f, 0.90f);
            }

            float risk = (100f - _approvalRating) * 0.004f;
            risk += (crueltyIndex / 100f) * 0.25f;
            if (guardDeficiency > 0) risk += guardDeficiency * 0.08f;
            if (_isElectionDisputed) risk += 0.35f;

            return Math.Clamp(risk, 0.01f, 0.85f);
        }

        public void AdvanceDailyPolitics(float foodSat, float secSat, float crueltyIndex, int guardDeficiency, ISeededRng rng)
        {
            // Update approval
            var breakdown = CalculateApprovalBreakdown(foodSat, secSat, crueltyIndex);
            _approvalRating = breakdown.totalApproval;

            // Legitimacy drift
            if (_isMartialLaw)
            {
                _legitimacy = Math.Max(5f, _legitimacy - 3.5f);
            }
            else
            {
                _legitimacy = Math.Clamp(_legitimacy + (_approvalRating > 60f ? 0.8f : -0.5f), 10f, 100f);
            }

            // Countdown to election
            if (!_isMartialLaw)
            {
                _daysUntilElection--;
            }

            // Coup calculation
            _coupRisk = CalculateCoupRisk(foodSat, crueltyIndex, guardDeficiency);
            if (_coupRisk > 0.45f && rng.NextDouble() < _coupRisk * 0.15f)
            {
                _totalCoups++;
                OnCoupTriggered?.Invoke("Disaffected military officers and councilors have launched an armed coup d'etat!");
            }
        }

        public bool ResolveCoup(bool incumbentRetainsPower, ISeededRng rng)
        {
            if (incumbentRetainsPower)
            {
                _legitimacy = Math.Min(100f, _legitimacy + 20f);
                OnCoupResolved?.Invoke(true, "Loyalist forces defeated the conspirators and restored civil order.");
                return true;
            }
            else
            {
                _currentLeaderId = "insurgent_military_tribunal";
                _governanceMode = "Appointed";
                _legitimacy = 30f;
                _activePolicies.Clear();
                OnCoupResolved?.Invoke(false, "Coup successful. The military tribunal has seized control of all shelter assets.");
                return false;
            }
        }

        public PoliticsState CaptureState()
        {
            var state = new PoliticsState
            {
                systemId = SystemId,
                currentLeaderId = _currentLeaderId,
                governanceMode = _governanceMode,
                approvalRating = _approvalRating,
                legitimacy = _legitimacy,
                daysUntilElection = _daysUntilElection,
                isMartialLaw = _isMartialLaw,
                isElectionDisputed = _isElectionDisputed,
                coupRisk = _coupRisk,
                totalElectionsHeld = _totalElections,
                totalCoupsAttempted = _totalCoups
            };
            state.activePolicies.AddRange(_activePolicies);
            state.electionHistory.AddRange(_history);
            return state;
        }

        public void RestoreState(PoliticsState? state)
        {
            _activePolicies.Clear();
            _history.Clear();
            _currentLeaderId = string.Empty;
            _governanceMode = "Democratic";
            _approvalRating = 60f;
            _legitimacy = 75f;
            _daysUntilElection = 30;
            _isMartialLaw = false;
            _isElectionDisputed = false;
            _coupRisk = 0f;
            _totalElections = 0;
            _totalCoups = 0;

            if (state == null) return;

            _currentLeaderId = state.currentLeaderId ?? string.Empty;
            _governanceMode = state.governanceMode ?? "Democratic";
            _approvalRating = state.approvalRating;
            _legitimacy = state.legitimacy;
            _daysUntilElection = state.daysUntilElection;
            _isMartialLaw = state.isMartialLaw;
            _isElectionDisputed = state.isElectionDisputed;
            _coupRisk = state.coupRisk;
            _totalElections = state.totalElectionsHeld;
            _totalCoups = state.totalCoupsAttempted;

            if (state.activePolicies != null)
            {
                foreach (var p in state.activePolicies)
                {
                    if (!string.IsNullOrEmpty(p))
                        _activePolicies.Add(p);
                }
            }
            if (state.electionHistory != null)
            {
                _history.AddRange(state.electionHistory);
            }
        }
    }
}
