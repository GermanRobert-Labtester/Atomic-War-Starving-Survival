// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Survivors;

namespace Ashfall.Core.Narrative
{
    public enum CrimeType
    {
        Theft,
        Assault,
        Murder,
        Hoarding,
        Sabotage,
        Desertion
    }

    public enum TrialVerdict
    {
        NotGuilty,
        Guilty,
        Inconclusive
    }

    public enum PunishmentLevel
    {
        Warning,
        Restitution,
        Labor,
        Confinement,
        Banishment,
        Execution
    }

    public enum IncidentStatus
    {
        Unresolved,
        TrialInProgress,
        Resolved,
        VigilanteResolved
    }

    [Serializable]
    public sealed class WastelandLawDef
    {
        public string law_id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string crime_type { get; set; } = "Theft";
        public float min_evidence_confidence { get; set; } = 0.5f;
        public List<string> allowed_punishments { get; set; } = new List<string>();
        public float legitimacy_impact { get; set; } = 5.0f;
        public float fear_impact { get; set; } = 0.0f;
        public float deterrence_rating { get; set; } = 0.35f;
        public string doctrine_tag { get; set; } = "Merciful";
    }

    [Serializable]
    public sealed class WastelandLawsCatalog
    {
        public int schema_version { get; set; } = 1;
        public List<WastelandLawDef> laws { get; set; } = new List<WastelandLawDef>();
    }

    [Serializable]
    public sealed class EvidenceClue
    {
        public string evidenceId { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public float confidenceWeight { get; set; } = 0.3f;
        public int foundDay { get; set; } = 1;
    }

    [Serializable]
    public sealed class CrimeIncident
    {
        public string incidentId { get; set; } = string.Empty;
        public CrimeType crimeType { get; set; } = CrimeType.Theft;
        public string accusedSurvivorId { get; set; } = string.Empty;
        public string? victimSurvivorId { get; set; } = null;
        public int day { get; set; } = 1;
        public List<EvidenceClue> evidenceClues { get; set; } = new List<EvidenceClue>();
        public IncidentStatus status { get; set; } = IncidentStatus.Unresolved;
        public string? assignedLawId { get; set; } = null;
        public TrialVerdict? verdict { get; set; } = null;
        public PunishmentLevel? punishment { get; set; } = null;
    }

    [Serializable]
    public sealed class BanishmentRecord
    {
        public string survivorId { get; set; } = string.Empty;
        public int banishedDay { get; set; } = 1;
        public CrimeType crimeType { get; set; } = CrimeType.Theft;
        public string reason { get; set; } = string.Empty;
        public float grudgeSeverity { get; set; } = 50.0f;
    }

    [Serializable]
    public sealed class TrialDecision
    {
        public string incidentId { get; set; } = string.Empty;
        public TrialVerdict verdict { get; set; } = TrialVerdict.Guilty;
        public PunishmentLevel punishment { get; set; } = PunishmentLevel.Restitution;
    }

    public sealed class TrialResult
    {
        public bool Success { get; set; }
        public string FailureCode { get; set; } = string.Empty;
        public TrialVerdict Verdict { get; set; }
        public PunishmentLevel Punishment { get; set; }
        public float MoraleDelta { get; set; }
        public float FearDelta { get; set; }
        public float LegitimacyDelta { get; set; }

        public static TrialResult Fail(string code) =>
            new TrialResult { Success = false, FailureCode = code };
    }

    [Serializable]
    public sealed class JusticeState
    {
        public int schema_version { get; set; } = 1;
        public List<CrimeIncident> incidents { get; set; } = new List<CrimeIncident>();
        public List<BanishmentRecord> banishments { get; set; } = new List<BanishmentRecord>();
        public List<string> imprisonedSurvivorIds { get; set; } = new List<string>();
        public Dictionary<string, float> survivorGrudges { get; set; } = new Dictionary<string, float>(StringComparer.Ordinal);
        public float vigilantePressure { get; set; } = 0.0f;
        public int totalExecutions { get; set; } = 0;
        public int totalBanishments { get; set; } = 0;
    }

    public sealed class JusticeSystem
    {
        private readonly ISeededRng _rng;
        private readonly Inventory.Inventory _inventory;
        private readonly NeedsSystem? _needs;
        private readonly ILog _log;

        private readonly Dictionary<string, WastelandLawDef> _laws = new Dictionary<string, WastelandLawDef>(StringComparer.Ordinal);
        private JusticeState _state = new JusticeState();

        public event Action<string, CrimeType, string>? OnCrimeReported;
        public event Action<string, TrialVerdict, PunishmentLevel>? OnTrialConcluded;
        public event Action<string, string>? OnBanishment;
        public event Action<string, string>? OnExecution;
        public event Action<string, string>? OnVigilanteOutbreak;

        public JusticeState State => _state;
        public IReadOnlyDictionary<string, WastelandLawDef> Laws => _laws;
        public float VigilantePressure => _state.vigilantePressure;

        public JusticeSystem(
            ISeededRng? rng = null,
            Inventory.Inventory? inventory = null,
            NeedsSystem? needs = null,
            ILog? log = null)
        {
            _rng = rng ?? new SeededRng(193);
            _inventory = inventory ?? new Inventory.Inventory();
            _needs = needs;
            _log = log ?? NullLog.Instance;
        }

        public void RegisterLaw(WastelandLawDef def)
        {
            if (def == null || string.IsNullOrEmpty(def.law_id)) return;
            _laws[def.law_id] = def;
        }

        public CrimeIncident ReportCrime(string incidentId, CrimeType crime, string accusedId, string? victimId, int currentDay)
        {
            var incident = new CrimeIncident
            {
                incidentId = incidentId,
                crimeType = crime,
                accusedSurvivorId = accusedId,
                victimSurvivorId = victimId,
                day = currentDay,
                status = IncidentStatus.Unresolved
            };

            // Link matching law
            foreach (var kvp in _laws)
            {
                if (string.Equals(kvp.Value.crime_type, crime.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    incident.assignedLawId = kvp.Key;
                    break;
                }
            }

            _state.incidents.Add(incident);
            OnCrimeReported?.Invoke(incidentId, crime, accusedId);
            return incident;
        }

        public ActionResult AddEvidence(string incidentId, string evidenceId, string description, float weight, int currentDay)
        {
            var incident = _state.incidents.Find(i => i.incidentId == incidentId);
            if (incident == null) return ActionResult.Blocked("incident_not_found", "justice.incident_not_found");
            if (incident.status != IncidentStatus.Unresolved && incident.status != IncidentStatus.TrialInProgress)
                return ActionResult.Blocked("incident_already_resolved", "justice.incident_already_resolved");

            incident.evidenceClues.Add(new EvidenceClue
            {
                evidenceId = evidenceId,
                description = description,
                confidenceWeight = Math.Max(0.05f, Math.Min(1.0f, weight)),
                foundDay = currentDay
            });

            return ActionResult.Success("justice.evidence_logged");
        }

        public float CalculateEvidenceStrength(string incidentId)
        {
            var incident = _state.incidents.Find(i => i.incidentId == incidentId);
            if (incident == null || incident.evidenceClues.Count == 0) return 0f;

            float total = 0f;
            for (int i = 0; i < incident.evidenceClues.Count; i++)
            {
                total += incident.evidenceClues[i].confidenceWeight;
            }

            return Math.Min(1.0f, total);
        }

        public TrialResult HoldTrial(TrialDecision decision, int currentDay)
        {
            if (decision == null) return TrialResult.Fail("null_decision");
            var incident = _state.incidents.Find(i => i.incidentId == decision.incidentId);
            if (incident == null) return TrialResult.Fail("incident_not_found");
            if (incident.status == IncidentStatus.Resolved || incident.status == IncidentStatus.VigilanteResolved)
                return TrialResult.Fail("already_resolved");

            float evidenceScore = CalculateEvidenceStrength(decision.incidentId);

            // Validate law threshold if guilty
            if (decision.verdict == TrialVerdict.Guilty && !string.IsNullOrEmpty(incident.assignedLawId))
            {
                if (_laws.TryGetValue(incident.assignedLawId, out var law))
                {
                    if (evidenceScore < law.min_evidence_confidence)
                    {
                        return TrialResult.Fail("insufficient_evidence_for_conviction");
                    }
                    if (!law.allowed_punishments.Contains(decision.punishment.ToString()))
                    {
                        return TrialResult.Fail("punishment_not_permitted_by_law");
                    }
                }
            }

            incident.verdict = decision.verdict;
            incident.punishment = decision.punishment;
            incident.status = IncidentStatus.Resolved;

            float moraleDelta = 0f;
            float fearDelta = 0f;
            float legitimacyDelta = 5f;

            if (decision.verdict == TrialVerdict.Guilty)
            {
                switch (decision.punishment)
                {
                    case PunishmentLevel.Warning:
                        moraleDelta = -2f;
                        legitimacyDelta = 2f;
                        break;
                    case PunishmentLevel.Restitution:
                        moraleDelta = 3f;
                        legitimacyDelta = 8f;
                        // Award victim or shelter restitution in scrap
                        _inventory.AddById("scrap_metal", 20);
                        break;
                    case PunishmentLevel.Labor:
                    case PunishmentLevel.Confinement:
                        moraleDelta = -5f;
                        fearDelta = 10f;
                        legitimacyDelta = 10f;
                        if (!_state.imprisonedSurvivorIds.Contains(incident.accusedSurvivorId))
                        {
                            _state.imprisonedSurvivorIds.Add(incident.accusedSurvivorId);
                        }
                        break;
                    case PunishmentLevel.Banishment:
                        moraleDelta = -10f;
                        fearDelta = 20f;
                        legitimacyDelta = 12f;
                        _state.totalBanishments++;
                        _state.banishments.Add(new BanishmentRecord
                        {
                            survivorId = incident.accusedSurvivorId,
                            banishedDay = currentDay,
                            crimeType = incident.crimeType,
                            reason = $"Banished for {incident.crimeType} in incident {incident.incidentId}",
                            grudgeSeverity = 75f
                        });
                        OnBanishment?.Invoke(incident.accusedSurvivorId, incident.incidentId);
                        break;
                    case PunishmentLevel.Execution:
                        moraleDelta = -25f;
                        fearDelta = 40f;
                        legitimacyDelta = 15f;
                        _state.totalExecutions++;
                        if (_needs != null)
                        {
                            var condemned = _needs.Get(incident.accusedSurvivorId);
                            if (condemned != null)
                            {
                                _needs.Modify(condemned, NeedKind.Health, -100f);
                            }

                            // Morale ripple across other survivors
                            for (int i = 0; i < _needs.Registered.Count; i++)
                            {
                                var dweller = _needs.Registered[i];
                                if (dweller != null && dweller.Id != incident.accusedSurvivorId)
                                {
                                    _needs.Modify(dweller, NeedKind.Morale, -15f);
                                }
                            }
                        }
                        OnExecution?.Invoke(incident.accusedSurvivorId, incident.incidentId);
                        break;
                }
            }
            else
            {
                // Acquittal
                moraleDelta = 5f;
                legitimacyDelta = (evidenceScore > 0.7f) ? -10f : 5f; // if acquitted despite strong evidence, legitimacy drops
            }

            // Reduce vigilante pressure on resolution
            _state.vigilantePressure = Math.Max(0f, _state.vigilantePressure - 30f);

            OnTrialConcluded?.Invoke(decision.incidentId, decision.verdict, decision.punishment);

            return new TrialResult
            {
                Success = true,
                Verdict = decision.verdict,
                Punishment = decision.punishment,
                MoraleDelta = moraleDelta,
                FearDelta = fearDelta,
                LegitimacyDelta = legitimacyDelta
            };
        }

        public void TickDay(int currentDay)
        {
            for (int i = 0; i < _state.incidents.Count; i++)
            {
                var inc = _state.incidents[i];
                if (inc.status != IncidentStatus.Unresolved) continue;

                int daysOpen = currentDay - inc.day;
                if (daysOpen >= 3 && (inc.crimeType == CrimeType.Assault || inc.crimeType == CrimeType.Murder || inc.crimeType == CrimeType.Sabotage))
                {
                    _state.vigilantePressure += 15f;
                }
            }

            if (_state.vigilantePressure >= 60f)
            {
                // Find first unresolved serious crime
                var targetInc = _state.incidents.Find(i => i.status == IncidentStatus.Unresolved);
                if (targetInc != null)
                {
                    targetInc.status = IncidentStatus.VigilanteResolved;
                    _state.vigilantePressure = 0f;

                    if (_needs != null)
                    {
                        var accused = _needs.Get(targetInc.accusedSurvivorId);
                        if (accused != null)
                        {
                            _needs.Modify(accused, NeedKind.Health, -35f);
                            _needs.Modify(accused, NeedKind.Morale, -30f);
                        }
                    }

                    OnVigilanteOutbreak?.Invoke(targetInc.incidentId, targetInc.accusedSurvivorId);
                }
            }
        }

        public void RestoreState(JusticeState state)
        {
            if (state == null) return;
            _state = state;
        }
    }
}
